using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq;
using Tensorflow;

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
        public List<KnowledgeGroup> AllGroups { get; set; } = new List<KnowledgeGroup>();
        public string? SelectedTitleInformation { get; set; } = string.Empty;
        public bool ShowAddForm { get; set; } = false;
        public bool ShowEditForm { get; set; } = false;
        public bool ShowDeleteForm { get; set; } = false;
        public bool ShowAddSWOTForm { get; set; } = false;

        public async Task OnGetAsync(int? selectedKnowledgeGroup, string? selectedTitle)
        {
            // Fetch all knowledge groups based on user
            using (var reader = DBClassReaders.KnowledgeGroupReaderByUser(HttpContext.Session.GetInt32("userid")))
            {
                while (reader.Read())
                {
                    AllGroups.Add(new KnowledgeGroup
                    {
                        KnowledgeGroupID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }

                DBClassReaders.LabOneDBConnection.Close();
            }

            // Fetches all knowledge items adds them to list
            //using (var reader = DBClassReaders.KnowledgeReader())
            //{
            //    while (reader.Read())
            //    {
            //        var item = new KnowledgeItem
            //        {
            //            // Assuming these are the correct column indexes
            //            KnowledgeItemID = reader.GetInt32(0),
            //            UserID = reader.GetInt32(1),
            //            KnowledgeGroupID = reader.GetInt32(2),
            //            Title = reader.GetString(3),
            //            Information = reader.GetString(4),
            //            DateCreated = reader.GetDateTime(5),
            //            LastDateModified = reader.GetDateTime(6)
            //        };

            //        knowledgeItems.Add(item);
            //        if (!AllGroups.Contains(item.KnowledgeGroupID))
            //        {
            //            AllGroups.Add(item.KnowledgeGroupID);
            //        }
            //    }

            //    DBClassReaders.LabOneDBConnection.Close();
            //}

            if (selectedKnowledgeGroup != 0)
            {
                knowledgeItems = knowledgeItems.Where(item => item.KnowledgeGroupID == selectedKnowledgeGroup).ToList();
            }
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
                    ShowAddSWOTForm = false;
                    break;
                default:
                    // Handle unexpected action
                    break;
            }
            DBClassWriters.LabOneDBConnection.Close();

            await OnGetAsync(null, null); // Reinitialize the page data

            return Page();
        }
    }
}