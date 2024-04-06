namespace VisionaryVentures.Pages.DataClasses
{
    public class SWOTWithDataAnalysis
    {
        public int SWOTAnalysisID { get; set; }
        public string implications { get; set; }
        public string strategies { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string Notes { get; set; }
        public int KnowledgeGroupID { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string Opportunities { get; set; }
        public string Threats { get; set; }
        public int ReportID { get; set; }
    }
}
