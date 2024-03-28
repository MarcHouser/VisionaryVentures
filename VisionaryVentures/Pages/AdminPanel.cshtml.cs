using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;

namespace VisionaryVentures.Pages
{
    public class AdminPanelModel : PageModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Collaboration> Collaborations { get; set; } = new List<Collaboration>();
        public List<Plan> Plans { get; set; } = new List<Plan>();

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
        }
    }
}
