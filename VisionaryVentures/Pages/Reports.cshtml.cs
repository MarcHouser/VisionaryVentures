using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;

namespace VisionaryVentures.Pages
{
    public class ReportsModel : PageModel
    {
        public List<KnowledgeGroup> AllGroups { get; set; } = new List<KnowledgeGroup>();
        public List<Report> Reports { get; set; } = new List<Report>();
        public List<SWOT> SWOTs { get; set; } = new List<SWOT>();
        public List<PEST> PESTs { get; set; } = new List<PEST>();

        public async Task OnGetAsync()
        {
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

            foreach (var group in AllGroups)
            {
                using (var reader = DBClassReaders.GetReportsByKnowledgeGroup((int)group.KnowledgeGroupID))
                {
                    while (reader.Read())
                    {
                        Reports.Add(new Report
                        {
                            ReportID = reader.GetInt32(0),
                            DateCreated = reader.GetDateTime(1),
                            Title = reader.GetString(2),
                            Description = reader.GetString(3),
                            KnowledgeGroupID = reader.GetInt32(4)
                        });
                        SWOTs.Add(new SWOT
                        {
                            SWOTAnalysisID = reader.GetInt32(5),
                            implications = reader.GetString(6),
                            strategies = reader.GetString(7),
                            AnalysisDate = reader.GetDateTime(8),
                            Notes = reader.GetString(9),
                            KnowledgeGroupID = reader.GetInt32(10),
                            Strengths = reader.GetString(11),
                            Weaknesses = reader.GetString(12),
                            Opportunities = reader.GetString(13),
                            Threats = reader.GetString(14),
                            ReportID = reader.GetInt32(15)
                        });
                        PESTs.Add(new PEST
                        {
                            PESTAnalysisID = reader.GetInt32(16),
                            Category = reader.GetString(17),
                            Factor = reader.GetString(18),
                            Implications = reader.GetString(19),
                            PossibleActions = reader.GetString(20),
                            AnalysisDate = reader.GetDateTime(21),
                            Notes = reader.GetString(22),
                            KnowledgeGroupID = reader.GetInt32(23),
                            ReportID = reader.GetInt32(24)
                        });
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }
            }
        }
    }
}
