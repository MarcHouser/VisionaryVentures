namespace VisionaryVentures.Pages.DataClasses
{
    public class Dataset
    {

        public int DatasetID { get; set; }
        public int UserID { get; set; }
        public String? FileName { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
