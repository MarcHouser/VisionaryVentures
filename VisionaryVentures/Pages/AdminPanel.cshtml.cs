using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net.Mail;

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
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string EmailAddress { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public string StreetAddress { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string State { get; set; }
        [BindProperty]
        public int PostalCode { get; set; }
        [BindProperty]
        public string Country { get; set; }

        public async void OnGetAsync()
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

            return RedirectToPage("/AdminPanel");
        }
        public IActionResult OnPostCreateUser()
        {
            short userType = 2;
            string userTypeDescription = "General";
            DBClassWriters.CreateHashedUser(Username, Password);
            DBClassWriters.AddUserWithAccount(userType, userTypeDescription, FirstName, LastName, EmailAddress, PhoneNumber, StreetAddress, City, State, PostalCode, Country);
            DBClassWriters.LabOneDBConnection.Close();

            OnGetAsync();
            return Page();
        }
    }

}
