using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;

namespace VisionaryVentures.Pages
{
    public class PrintReportModel : PageModel
    {
        public Collaboration Collaboration { get; set; }
        public List<KnowledgeItem> KnowledgeItems { get; set; } = new List<KnowledgeItem>();
        public List<PlanContent> PlanContents { get; set; } = new List<PlanContent>();
        public List<User> users { get; set; } = new List<User>();

        public void OnGet(int? CollaborationID)
        {

            using (var reader = DBClassReaders.UserReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserID = reader.GetInt32(0),
                        AccountID = reader.GetInt32(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        EmailAddress = reader.GetString(4),
                        PhoneNumber = reader.GetString(5),
                        StreetAddress = reader.GetString(6),
                        City = reader.GetString(7),
                        State = reader.GetString(8),
                        PostalCode = reader.GetInt32(9),
                        Country = reader.GetString(10)
                    });
                }

                DBClassReaders.LabOneDBConnection.Close();
            }
        }
    }
}
