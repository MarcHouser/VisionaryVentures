namespace VisionaryVentures.Pages.DataClasses
{
    public class KnowledgeItem
    {

        public int KnowledgeItemID { get; set; }
        public int UserID { get; set; }
        public String? Category { get; set; }
        public String? Title { get; set; }
        public String? Information { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateModified { get; set; }
    }
}
