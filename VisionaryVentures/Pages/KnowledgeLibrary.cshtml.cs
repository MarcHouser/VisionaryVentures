using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq;
using Tensorflow;
using VisionaryVentures.Pages.ReportGeneration;

namespace VisionaryVentures.Pages
{
    public class KnowledgeLibraryModel : PageModel
    {
        [BindProperty]
        public int KnowledgeItemID { get; set; }

        // Report Properties
        [BindProperty]
        public string ReportTitle { get; set; }
        [BindProperty]
        public string ReportDescription { get; set; }

        //SWOT Properties
        [BindProperty]
        public string SWOTStrengths { get; set; }
        [BindProperty]
        public string SWOTWeaknesses { get; set; }
        [BindProperty]
        public string SWOTOpportunities { get; set; }
        [BindProperty]
        public string SWOTThreats { get; set; }
        [BindProperty]
        public string SWOTImplications { get; set; }
        [BindProperty]
        public string Strategy { get; set; }
        [BindProperty]
        public string SWOTNotes { get; set; }

        //PEST Properties
        [BindProperty]
        public string Category { get; set; }
        [BindProperty]
        public string Factor { get; set; }
        [BindProperty]
        public string PESTImplications { get; set; }
        [BindProperty]
        public string PossibleActions { get; set; }
        [BindProperty]
        public string PESTNotes { get; set; }


        [BindProperty]
        public int SelectedKnowledgeGroupID { get; set; }

        // Initialize knowledgeItems lists and form buttons
        public List<KnowledgeItem> knowledgeItems { get; set; } = new List<KnowledgeItem>();
        public SWOT SwotAnalysis { get; set; } = new SWOT();
        public List<KnowledgeGroup> AllGroups { get; set; } = new List<KnowledgeGroup>();

        private readonly PdfGenerator _pdfGenerator;

        public async Task OnGetAsync(int? selectedKnowledgeGroup)
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

            //if (selectedKnowledgeGroup != 0)
            //{
            //    knowledgeItems = knowledgeItems.Where(item => item.KnowledgeGroupID == selectedKnowledgeGroup).ToList();
            //}

            if (selectedKnowledgeGroup.HasValue)
            {
                HttpContext.Session.SetInt32("selectedKnowledgeGroup", (int)selectedKnowledgeGroup);
                TempData["ShowMainBody"] = true; // Ensure this is set whenever you want the main body to be shown
            }
            else
            {
                TempData["ShowMainBody"] = false; // Or decide based on other conditions
            }
        }

        public async Task<IActionResult> OnPostSaveReportAsync()
        {
            int SelectedKGID = (int)HttpContext.Session.GetInt32("selectedKnowledgeGroup");

            DBClassWriters.BuildReport(DateTime.Now, ReportTitle, ReportDescription, SWOTImplications, Strategy, DateTime.Now, SWOTNotes, SelectedKGID, 
                SWOTStrengths, SWOTWeaknesses, SWOTOpportunities, SWOTThreats, Category, Factor, PESTImplications, PossibleActions, DateTime.Now, PESTNotes);
            DBClassWriters.LabOneDBConnection.Close();

            return RedirectToPage("/Reports");
        }
    }
}