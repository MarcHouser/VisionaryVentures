namespace VisionaryVentures.Pages.DataClasses
{
    public class Message
    {

        public int MessageID { get; set; }
        public int CollaborationID { get; set; }
        public int SentFrom { get; set; }
        public String? MessageContent { get; set; }
        public DateTime DateCreated { get; set; }
        public string FullName { get; set; }
    }
}
