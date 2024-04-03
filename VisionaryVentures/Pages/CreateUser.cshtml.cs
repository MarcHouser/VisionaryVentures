using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;

namespace VisionaryVentures.Pages
{
    public class CreateUserModel : PageModel
    {


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

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            short userType = 2;
            string userTypeDescription = "General";

            DBClassWriters.CreateHashedUser(Username, Password);
            DBClassWriters.AddUserWithAccount(userType, userTypeDescription, FirstName, LastName, EmailAddress, PhoneNumber, StreetAddress, City, State, PostalCode, Country);
            DBClassWriters.LabOneDBConnection.Close();


            return RedirectToPage("Home");
        }
    }
}
