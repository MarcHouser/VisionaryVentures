using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace VisionaryVentures.Pages
{
    public class KnowledgeLibraryModel : PageModel
    {
        [BindProperty]
        public int KnowledgeItemID { get; set; }
        [BindProperty]
        public string? Category { get; set; }
        [BindProperty]
        public string? Title { get; set; }
        [BindProperty]
        public string? Information { get; set; }
        [BindProperty]
        public string? Strengths { get; set; }
        [BindProperty]
        public string? Weaknesses { get; set; }
        [BindProperty]
        public string? Opportunities { get; set; }
        [BindProperty]
        public string? Threats { get; set; }
        [BindProperty]
        public int SelectedKnowledgeItemId { get; set; }
        //[BindProperty(SupportsGet = true)]
        //public string SearchTerm { get; set; }

        // Initialize knowledgeItems lists and form buttons
        public List<KnowledgeItem> knowledgeItems { get; set; } = new List<KnowledgeItem>();
        public SWOT SwotAnalysis { get; set; } = new SWOT();
        public List<string>? AllCategories { get; set; } = new List<string>();
        public string? SelectedTitleInformation { get; set; } = string.Empty;
        public bool ShowAddForm { get; set; } = false;
        public bool ShowEditForm { get; set; } = false;
        public bool ShowDeleteForm { get; set; } = false;
        public bool ShowAddSWOTForm { get; set; } = false;

        public async Task OnGetAsync(string? selectedCategory, string? selectedTitle)
        {

            // Fetches all knowledge items adds them to list
            using (var reader = DBClassReaders.KnowledgeReader())
            {
                while (reader.Read())
                {
                    var item = new KnowledgeItem
                    {
                        // Assuming these are the correct column indexes
                        KnowledgeItemID = reader.GetInt32(0),
                        UserID = reader.GetInt32(1),
                        Category = reader.GetString(2),
                        Title = reader.GetString(3),
                        Information = reader.GetString(4),
                        DateCreated = reader.GetDateTime(5),
                        LastDateModified = reader.GetDateTime(6)
                    };

                    knowledgeItems.Add(item);
                    if (!AllCategories.Contains(item.Category))
                    {
                        AllCategories.Add(item.Category);
                    }
                }

                DBClassReaders.LabOneDBConnection.Close();
            }

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                knowledgeItems = knowledgeItems.Where(item => item.Category == selectedCategory).ToList();
            }

            if (!string.IsNullOrEmpty(selectedTitle))
            {
                var selectedItem = knowledgeItems.FirstOrDefault(item => item.Title == selectedTitle);
                if (selectedItem != null)
                {
                    SelectedTitleInformation = selectedItem.Information;
                    KnowledgeItemID = selectedItem.KnowledgeItemID; // Ensure this is set for fetching SWOT

                    // Fetch SWOT Analysis for the selected item
                    var swotQuery = "SELECT * FROM SwotAnalyses WHERE KnowledgeItemID = @KnowledgeItemID";
                    using (SqlConnection connection = DBClassReaders.LabOneDBConnection) //new SqlConnection(DBClassReaders.LabOneDBConnection))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(swotQuery, connection))
                        {
                            command.Parameters.AddWithValue("@KnowledgeItemID", selectedItem.KnowledgeItemID);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    SwotAnalysis = new SWOT
                                    {
                                        // Assuming these are the correct column indexes for your SWOT data
                                        Strengths = reader["Strengths"].ToString(),
                                        Weaknesses = reader["Weaknesses"].ToString(),
                                        Opportunities = reader["Opportunities"].ToString(),
                                        Threats = reader["Threats"].ToString(),
                                    };
                                }
                                else
                                {
                                    // Reset SwotAnalysis if no data found for the selected item
                                    SwotAnalysis = new SWOT();
                                }
                            }
                        }
                    }
                }
            }
            //GetCategory(SearchTerm);
        }

        //Used for adding knowledge items
        public async Task<IActionResult> OnPostAsync(string action)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            switch (action)
            {
                case "ShowAdd":
                    ShowAddForm = true;
                    break;
                case "Add":
                    DBClassWriters.AddKnowledgeItem(1, Category, Title, Information, DateTime.Now, DateTime.Now);
                    ShowAddForm = false;
                    break;
                case "CancelAdd":
                    ShowAddForm = false; 
                    break;
            }

            DBClassWriters.LabOneDBConnection.Close();

            await OnGetAsync(null, null);

            return Page(); 
        }
        //Used for editing items
        public async Task<IActionResult> OnPostEditItem(string action)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            switch (action)
            {
                case "ShowEdit":
                    var itemToEdit = knowledgeItems.FirstOrDefault(item => item.Title == SelectedTitleInformation);
                    if (itemToEdit != null)
                    {
                        Information = itemToEdit.Information;
                    }
                    ShowEditForm = true;
                    break;
                case "Edit":
                    DBClassWriters.EditKnowledgeItem(KnowledgeItemID, Information, DateTime.Now);
                    ShowEditForm = false;
                    break;
                case "CancelEdit":
                    ShowEditForm = false; 
                    break;
            }

            DBClassWriters.LabOneDBConnection.Close();

            await OnGetAsync(null, null);

            return Page();
        }

        //Used for adding SWOT
        public async Task<IActionResult> OnPostAddSWOT(string action)
        {
            switch (action)
            {
                case "ShowAddSWOT":
                    ShowAddSWOTForm = true;
                    break;
                case "Add":
                    DBClassWriters.UpsertSwotAnalysis(SelectedKnowledgeItemId, Strengths, Weaknesses, Opportunities, Threats); ;
                    DBClassWriters.LabOneDBConnection.Close();
                    ShowAddForm = false;
                    break;
                case "CancelAddSWOT":
                    ShowAddForm = false;
                    break;
                default:
                    // Handle unexpected action
                    break;
            }
            DBClassWriters.LabOneDBConnection.Close();

            await OnGetAsync(null, null); // Reinitialize the page data

            return Page();
        }



        //private void GetCategory(string searchTerm)
        //{
        //    string categoryQuery = @"SELECT KnowledgeItems.KnowledgeItemID, KnowledgeItems.Category, KnowledgeItems.Title
        //                            FROM KnowledgeItems";

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        categoryQuery += " WHERE KnowledgeItems.Category LIKE @searchTerm OR KnowledgeItems.Title LIKE @searchTerm";
        //    }

        //    using (var connection = new SqlConnection(DBClassReaders.ConnectionString))
        //    {
        //        connection.Open();
        //        using (var command = new SqlCommand(categoryQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

        //            using (var reader = command.ExecuteReader())
        //            {
        //                knowledgeItems.Clear();
        //                while (reader.Read())
        //                {
        //                    knowledgeItems.Add(new KnowledgeItem
        //                    {
        //                        KnowledgeItemID = Convert.ToInt32(reader["KnowledgeItemID"]),
        //                        Category = reader["Category"].ToString(),
        //                        Title = reader["Title"].ToString(),
        //                    });
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
