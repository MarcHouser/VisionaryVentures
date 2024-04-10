namespace VisionaryVentures.Pages.DataClasses
{
    public class ReportWithAnalysis
    {
        public int ReportID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string AnalysisTextFilePath { get; set; }
        public string AnalysisImageFilePath { get; set; }
        public int KnowledgeGroupID { get; set; }
        public string Title { get; set; }
    }
}
