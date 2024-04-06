using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using System.Data.SqlClient;
namespace VisionaryVentures.Pages
{
    public class PrintReportModel : PageModel
    {
        public Report Report { get; set; } = new Report();
        public SWOT SWOT { get; set; } = new SWOT();
        public PEST PEST { get; set; } = new PEST();
        public async Task OnGetAsync(int SelectedReportID)
        {
            if (SelectedReportID > 0)
            {
                using (var reader = DBClassReaders.GetReportsByReportID(SelectedReportID))
                {
                    while (reader.Read())
                    {
                        Report = new Report
                        {
                            ReportID = reader.GetInt32(0),
                            DateCreated = reader.GetDateTime(1),
                            Title = reader.GetString(2),
                            Description = reader.GetString(3),
                            KnowledgeGroupID = reader.GetInt32(4)
                        };
                        SWOT = new SWOT
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
                        };
                        PEST = new PEST
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
                        };
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }
            }
        }
        public async Task<IActionResult> OnPostDownloadPDFAsync(int reportID)
        {
            // Logic to generate PDF based on selected report data
            return Page(); // Placeholder for PDF generation
        }
    }
}
