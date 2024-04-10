using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using System.Data.SqlClient;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace VisionaryVentures.Pages
{
    public class PrintReportModel : PageModel
    {
        public Report Report { get; set; } = new Report();
        public SWOT SWOT { get; set; } = new SWOT();
        public PEST PEST { get; set; } = new PEST();

        public ReportWithAnalysis ReportWithAnalysis { get; set; } = new ReportWithAnalysis();
        public SWOTWithDataAnalysis SWOTWithDataAnalysis { get; set; } = new SWOTWithDataAnalysis();
        public PESTWithDataAnalysis PESTWithDataAnalysis { get; set; } = new PESTWithDataAnalysis();

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

                //using (var reader = DBClassReaders.GetReportsByReportID(SelectedReportID))
                //{
                //    while (reader.Read())
                //    {
                //        ReportWithAnalysis = new ReportWithAnalysis
                //        {
                //            ReportID = reader.GetInt32(0),
                //            Description = reader.GetString(1),
                //            DateCreated = reader.GetDateTime(2),
                //            AnalysisTextFilePath = reader.GetString(3),
                //            AnalysisImageFilePath = reader.GetString(4),
                //            KnowledgeGroupID = reader.GetInt32(5),
                //            Title = reader.GetString(6)
                //        };
                //        SWOTWithDataAnalysis = new SWOTWithDataAnalysis
                //        {
                //            SWOTAnalysisID = reader.GetInt32(7),
                //            implications = reader.GetString(8),
                //            strategies = reader.GetString(9),
                //            AnalysisDate = reader.GetDateTime(10),
                //            Notes = reader.GetString(11),
                //            KnowledgeGroupID = reader.GetInt32(12),
                //            Strengths = reader.GetString(13),
                //            Weaknesses = reader.GetString(14),
                //            Opportunities = reader.GetString(15),
                //            Threats = reader.GetString(16),
                //            ReportID = reader.GetInt32(17)
                //        };
                //        PESTWithDataAnalysis = new PESTWithDataAnalysis
                //        {
                //            PESTAnalysisID = reader.GetInt32(18),
                //            Category = reader.GetString(19),
                //            Factor = reader.GetString(20),
                //            Implications = reader.GetString(21),
                //            PossibleActions = reader.GetString(22),
                //            AnalysisDate = reader.GetDateTime(23),
                //            Notes = reader.GetString(24),
                //            KnowledgeGroupID = reader.GetInt32(25),
                //            ReportID = reader.GetInt32(26)
                //        };
                //    }
                //    DBClassReaders.LabOneDBConnection.Close();
                //}
            }
        }
    }
}
