using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace VisionaryVentures.Pages
{
    public class PlansModel : PageModel
    {
        [BindProperty]
        public int PlanID { get; set; }
        [BindProperty]
        public String? PlanName { get; set; }
        [BindProperty]
        public String? PlantContentsID { get; set; }
        [BindProperty]
        public int PlanStep { get; set; }
        [BindProperty]
        public String? PlanDescription { get; set; }
        [BindProperty]
        public String? ContentDescription { get; set; }

        public List<PlanContent> SelectedPlanContent { get; set; }
        public Plan SelectedPlan { get; set;}
        public List<Plan>? plans { get; set; } = new List<Plan>();
        public List<PlanContent>? planContents { get; set; } = new List<PlanContent>();

        public bool ShowAddPlanForm { get; set; } = false;
        public bool ShowAddPlanContent { get; set; } = false;

        //[BindProperty(SupportsGet = true)]
        //public string SearchTerm { get; set; }

        // Page is loaded
        public async Task OnGetAsync(int? SelectedPlanID)
        {
            // Get plans from the database
            using (var reader = DBClassReaders.PlanReader())
            {
                while (reader.Read())
                {
                    plans.Add(new Plan
                    {
                        PlanID = reader.GetInt32(0),
                        PlanName = reader.GetString(1),
                        PlanDescription = reader.GetString(2),
                        DateCreated = reader.GetDateTime(3)
                    });

                }
            }
            
            DBClassReaders.LabOneDBConnection.Close();

            // Get plan contents from the database
            using (var reader = DBClassReaders.PlanContentReader())
            {
                while (reader.Read())
                {
                    planContents.Add(new PlanContent
                    {
                        PlanContentsID = reader.GetInt32(0),
                        PlanID = reader.GetInt32(1),
                        PlanStep = reader.GetInt32(2),
                        ContentDescription = reader.GetString(3)
                    });

                }
            }

            DBClassReaders.LabOneDBConnection.Close();

            // If a plan is selected, load the plan
            if (SelectedPlanID.HasValue)
            {
                LoadSelectedPlan(SelectedPlanID.Value);
            }

            //GetPlans(SearchTerm);
        }
        
        // OnPost for adding a plan
        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            switch (action)
            {
                case "ShowAdd":
                    ShowAddPlanForm = true;
                    break;
                case "Add":
                    DBClassWriters.AddPlan(PlanName, PlanDescription, DateTime.Now);
                    ShowAddPlanForm = false;
                    break;
                case "CancelAdd":
                    ShowAddPlanForm = false;
                    break;

            }

            DBClassWriters.LabOneDBConnection.Close();

            await OnGetAsync(null);

            return Page();
        }

        // OnPost for adding plan content
        public async Task<IActionResult> OnPostAddContent(string action)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            switch (action)
            {
                case "ShowAddContent":
                    ShowAddPlanContent = true;
                    break;
                case "AddContent":
                    DBClassWriters.AddPlanContents(PlanID, PlanStep, ContentDescription);
                    ShowAddPlanContent = false;
                    break;
                case "CancelAddContent":
                    ShowAddPlanContent = false;
                    break;
            }

            DBClassWriters.LabOneDBConnection.Close();

            await OnGetAsync(null);

            return Page();
        }

        // Load the selected plan
        private void LoadSelectedPlan(int SelectedPlanID)
        {
            var plan = plans.FirstOrDefault(x => x.PlanID == SelectedPlanID);

            SelectedPlan = plan;

            SelectedPlanContent = planContents.Where(x => x.PlanID == SelectedPlanID).ToList();
        }

        //private void GetPlans(string searchTerm)
        //{
        //    string planQuery = @"SELECT Plans.PlanID, Plans.PlanName, Plans.PlanDescription
        //                            FROM Plans";

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        planQuery += " WHERE Plans.PlanName LIKE @searchTerm OR Plans.PlanDescription LIKE @searchTerm";
        //    }

        //    using (var connection = new SqlConnection(DBClassReaders.ConnectionString))
        //    {
        //        connection.Open();
        //        using (var command = new SqlCommand(planQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

        //            using (var reader = command.ExecuteReader())
        //            {
        //                plans.Clear();
        //                while (reader.Read())
        //                {
        //                    plans.Add(new Plan
        //                    {
        //                        PlanID = Convert.ToInt32(reader["PlanID"]),
        //                        PlanName = reader["PlanName"].ToString(),
        //                        PlanDescription = reader["PlanDescription"].ToString(),
        //                    });
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
