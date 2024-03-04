using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace VisionaryVentures.Pages
{
    public class CollaborationModel : PageModel
    {
        // Initialize Binding Properties
        [BindProperty]
        public String? Title { get; set; }
        [BindProperty]
        public String? MessageContent { get; set; }
        [BindProperty]
        public String? PlanStep { get; set; }
        [BindProperty]
        public String? Description { get; set; }
        [BindProperty]
        public int CollaborationID { get; set; }
        [BindProperty]
        public int UserID { get; set; }

        public List<Message> Messages { get; set; }
        public List<Collaboration>? Collaborations { get; set; }
        public List<PlanContent> SelectedPlanContent { get; set; }
        public Plan SelectedPlan { get; set; }
        public List<Plan>? plans { get; set; } = new List<Plan>();
        public List<PlanContent>? planContents { get; set; } = new List<PlanContent>();
        public List<string> AllTitles { get; set; } = new List<string>();
        public string? SelectedPlanDescription { get; set; }

        public List<KnowledgeItem> KnowledgeItems { get; set; } = new List<KnowledgeItem>();
        public List<KnowledgeItem> SelectedKnowledgeItems { get; set; }
        public List<Dataset> Datasets { get; set; } = new List<Dataset>();
        public List<Dataset> SelectedDatasets { get; set; }

        // Initialization & Instantiation for all knowledge items, datasets, and plans
        public List<Plan> AllPlans { get; set; } = new List<Plan>();
        public List<KnowledgeItem> AllKnowledgeItems { get; set; } = new List<KnowledgeItem>();
        public List<Dataset> AllDatasets { get; set; } = new List<Dataset>();
        public List<SWOT> AllSWOTItems { get; set; } = new List<SWOT>();

        [BindProperty]
        public int? SelectedPlanId { get; set; }
        [BindProperty]
        public int? SelectedKnowledgeItemId { get; set; }
        [BindProperty]
        public int? SelectedDatasetId { get; set; }
        [BindProperty]
        public int? SelectedSWOTId { get; set; }

        public bool ShowAddForm { get; set; } = false;
        public bool ShowAddKnowledgeForm { get; set; } = false;

        //[BindProperty(SupportsGet = true)]
        //public string SearchTerm { get; set; }

        public async Task OnGetAsync(int? SelectedPlanID, int? SelectedCollaborationID, int? SelectedKnowledgeItemID, int? SelectedDataSetID)
        {
            Messages = new List<Message>();
            Collaborations = new List<Collaboration>();

            using (var reader = DBClassReaders.CollaborationReader())
            {
                while (reader.Read())
                {
                    Collaborations.Add(new Collaboration
                    {
                        CollaborationID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }
                DBClassReaders.LabOneDBConnection.Close();
            }

            if (SelectedCollaborationID.HasValue)
            {
                using (var reader = DBClassReaders.MessageReader(SelectedCollaborationID))
                {
                    while (reader.Read())
                    {
                        Messages.Add(new Message
                        {
                            MessageID = reader.GetInt32(0),
                            CollaborationID = reader.GetInt32(1),
                            SentFrom = reader.GetInt32(2),
                            MessageContent = reader.GetString(3),
                            DateCreated = reader.GetDateTime(4),
                            FullName = $"{reader.GetString(5)} {reader.GetString(6)}"
                        });
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }

                using (var reader = DBClassReaders.PlanByCollaboration(SelectedCollaborationID))
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

                    DBClassReaders.LabOneDBConnection.Close();
                }

                using (var reader = DBClassReaders.KnowledgeByCollaboration(SelectedCollaborationID))
                {
                    while (reader.Read())
                    {
                        KnowledgeItems.Add(new KnowledgeItem
                        {
                            KnowledgeItemID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            Category = reader.GetString(2),
                            Title = reader.GetString(3),
                            Information = reader.GetString(4),
                            DateCreated = reader.GetDateTime(5),
                            LastDateModified = reader.GetDateTime(6)
                        });
                    }

                    DBClassReaders.LabOneDBConnection.Close();
                }

                using (var reader = DBClassReaders.DatasetByCollaboration(SelectedCollaborationID))
                {
                    while (reader.Read())
                    {
                        Datasets.Add(new Dataset
                        {
                            DatasetID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            FileName = reader.GetString(2),
                            DateUploaded = reader.GetDateTime(3)
                        });
                    }

                    DBClassReaders.LabOneDBConnection.Close();
                }

                // Populate lists for plans, knowledge items, and datasets from all data in the database
                using (var reader = DBClassReaders.PlanReader())
                {
                    while (reader.Read())
                    {
                        AllPlans.Add(new Plan
                        {
                            PlanID = reader.GetInt32(0),
                            PlanName = reader.GetString(1),
                            PlanDescription = reader.GetString(2),
                            DateCreated = reader.GetDateTime(3),
                        });
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }

                using (var reader = DBClassReaders.KnowledgeReader())
                {
                    while (reader.Read())
                    {
                        AllKnowledgeItems.Add(new KnowledgeItem
                        {
                            KnowledgeItemID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            Category = reader.GetString(2),
                            Title = reader.GetString(3),
                            Information = reader.GetString(4),
                            DateCreated = reader.GetDateTime(5),
                            LastDateModified = reader.GetDateTime(6)
                        });
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }

                using (var reader = DBClassReaders.DatasetReader())
                {
                    while (reader.Read())
                    {
                        AllDatasets.Add(new Dataset
                        {
                            DatasetID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            FileName = reader.GetString(2),
                            DateUploaded = reader.GetDateTime(3)
                        });
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }

                HttpContext.Session.SetInt32("collaborationid", (int)SelectedCollaborationID);
            }

            // Load the selected plan, knowledge item, and dataset
            if (SelectedPlanID.HasValue)
            {
                LoadSelectedPlan(SelectedPlanID);
            }

            if (SelectedKnowledgeItemID.HasValue)
            {
                LoadSelectedKnowledge(SelectedKnowledgeItemID);
            }

            if (SelectedDataSetID.HasValue)
            {
                LoadSelectedDataset(SelectedDataSetID);
            }

            //GetCollaborations(SearchTerm);
        }

        // Add a message to the database
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(MessageContent))
            {
                UserID = (int)HttpContext.Session.GetInt32("userid");
                CollaborationID = (int)HttpContext.Session.GetInt32("collaborationid");
                DBClassWriters.AddMessage(CollaborationID, UserID, MessageContent, DateTime.Now);
                return RedirectToPage("/Collaboration", new { SelectedCollaborationID = CollaborationID });
            }
            else
            {
                return Page();
            }
        }

        // Add a Collaboration to the database
        public async Task<IActionResult> OnPostAddCollab(string action)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            switch (action)
            {
                case "ShowAddCollab":
                    ShowAddForm = true;
                    break;
                case "AddCollab":
                    DBClassWriters.AddCollaboration(Title, Description);
                    //DBClassWriters.LabOneDBConnection.Close();
                    ShowAddForm = false;
                    break;
                case "CancelAddCollab":
                    ShowAddForm = false;
                    break;
                default:
                    // Handle unexpected action
                    break;
            }
            DBClassWriters.LabOneDBConnection.Close();

            await OnGetAsync(null, null, null, null); // Reinitialize the page data

            return Page();
        }

        // Print report
        public IActionResult OnPostPrintReport()
        {

            return RedirectToPage("/PrintReport", new {CollaborationID = (int)HttpContext.Session.GetInt32("collaborationid")} );
        }

        public async Task<IActionResult> OnPostAssociateItemsAsync()
        {
            CollaborationID = (int)HttpContext.Session.GetInt32("collaborationid");

            if (SelectedPlanId.HasValue && !DBClassReaders.PlanIsAlreadyAssociated(CollaborationID, SelectedPlanId.Value))
            {
                DBClassWriters.AddPlanToCollaboration(CollaborationID, SelectedPlanId.Value);
            }
            // Assume similar methods exist for knowledge items and datasets
            if (SelectedKnowledgeItemId.HasValue && !DBClassReaders.KnowledgeItemIsAlreadyAssociated(CollaborationID, SelectedKnowledgeItemId.Value))
            {
                DBClassWriters.AddKnowledgeItemToCollaboration(SelectedKnowledgeItemId.Value, CollaborationID);
            }
            if (SelectedDatasetId.HasValue && !DBClassReaders.DatasetIsAlreadyAssociated(CollaborationID, SelectedDatasetId.Value))
            {
                DBClassWriters.AddDatasetToCollaboration(CollaborationID, SelectedDatasetId.Value);
            }

            return RedirectToPage("/Collaboration", new { SelectedCollaborationID = CollaborationID });
        }

        // Load the selected plan
        private void LoadSelectedPlan(int? SelectedPlanID)
        {
            var plan = plans.FirstOrDefault(x => x.PlanID == SelectedPlanID);

            SelectedPlan = plan;

            SelectedPlanContent = planContents.Where(x => x.PlanID == SelectedPlanID).ToList();
        }

        // Load the selected knowledge item
        private void LoadSelectedKnowledge(int? SelectedKnowledgeItemID)
        {
            var knowledgeItem = KnowledgeItems.FirstOrDefault(x => x.KnowledgeItemID == SelectedKnowledgeItemID);

            SelectedKnowledgeItems = KnowledgeItems.Where(x => x.KnowledgeItemID == SelectedKnowledgeItemID).ToList();
        }

        // Load the selected dataset
        private void LoadSelectedDataset(int? SelectedDatasetID)
        {
            var dataset = Datasets.FirstOrDefault(x => x.DatasetID == SelectedDatasetID);

            SelectedDatasets = Datasets.Where(x => x.DatasetID == SelectedDatasetID).ToList();
        }
    }
}

