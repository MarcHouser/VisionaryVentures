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

            // Instantiate Collaboration from DB Reader
            using (var reader = DBClassReaders.CollaborationReader())
            {
                if (reader.Read())
                {
                    Collaboration = new Collaboration
                    {
                        CollaborationID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2)
                    };
                }

                DBClassReaders.LabOneDBConnection.Close();
            }

            // Instantiate KnowledgeItems from DB Reader
            using (var reader = DBClassReaders.KnowledgeByCollaboration(CollaborationID))
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

            using (var reader = DBClassReaders.PlanByCollaboration(CollaborationID))
            {
                while (reader.Read())
                {
                    PlanContents.Add(new PlanContent
                    {
                        PlanContentsID = reader.GetInt32(0),
                        PlanID = reader.GetInt32(1),
                        PlanStep = reader.GetInt32(2),
                        ContentDescription = reader.GetString(3)
                    });
                }

                DBClassReaders.LabOneDBConnection.Close();
            }

            using (var reader = DBClassReaders.UserReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        EmailAddress = reader.GetString(3),
                        PhoneNumber = reader.GetString(4),
                        StreetAddress = reader.GetString(5),
                        City = reader.GetString(6),
                        State = reader.GetString(7),
                        PostalCode = reader.GetInt32(8),
                        Country = reader.GetString(9)
                    });
                }

                DBClassReaders.LabOneDBConnection.Close();
            }
        }
    }
}
