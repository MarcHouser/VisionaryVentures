using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;

namespace VisionaryVentures.Pages
{
    public class AdminPanelModel : PageModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<KnowledgeGroup> knowledgeGroups { get; set; } = new List<KnowledgeGroup>();
        public List<Plan> Plans { get; set; } = new List<Plan>();

        [BindProperty]
        public string KnowledgeGroupTitle { get; set; }
        [BindProperty]
        public string KnowledgeGroupDescription { get; set; }

        public void OnGet()
        {
            // Fetch all users from the database
            using (var reader = DBClassReaders.UserReader())
            {
                while (reader.Read())
                {
                    Users.Add(new User
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

            // Fetch all knowledge groups from the database
            using (var reader = DBClassReaders.KnowledgeGroupReader())
            {
                while (reader.Read())
                {
                    knowledgeGroups.Add(new KnowledgeGroup
                    {
                        KnowledgeGroupID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }
                DBClassReaders.LabOneDBConnection.Close();
            }
        }

        // Build new knowledge group
        public void OnPostCreateGroup()
        {
            // Insert new knowledge group into the database
            DBClassWriters.CreateKnowledgeGroup(KnowledgeGroupTitle, KnowledgeGroupDescription);
        }

        // Assign user to group
        public IActionResult OnPostAssignUserToGroup(int UserID, int KnowledgeGroupID)
        {
            if (UserID <= 0 || KnowledgeGroupID <= 0)
            {
                // Optionally, add error handling or messaging here
                return Page();
            }

            DBClassWriters.AddUserToKnowledgeGroup(UserID, KnowledgeGroupID);
            // Optionally, add success message or redirect logic here
            return RedirectToPage();
        }

    }
}
