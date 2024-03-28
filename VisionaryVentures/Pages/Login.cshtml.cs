using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;

namespace VisionaryVentures.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public String Username { get; set; }
        [BindProperty]
        public String Password { get; set; }
        [BindProperty]
        public int UserID { get; set; }
        public String Message { get; set; }

        public IActionResult OnGet(String logout)
        {
            if (logout != null)
            {
                HttpContext.Session.Clear();
                ViewData["LoginMessage"] = "Successfully Logged Out!";
            }

            return Page();
        }

        public IActionResult OnPost()
        {

            if (Username != null && Password!=null) 
            {
                if (DBClassReaders.HashedParameterLogin(Username, Password) && DBClassReaders.SPLogin(Username))
                {
                    var UserID = DBClassReaders.GetUserID(Username);
                    var UserType = DBClassReaders.GetUserType(UserID);

                    HttpContext.Session.SetString("username", Username);
                    HttpContext.Session.SetInt32("userid", UserID);
                    HttpContext.Session.SetInt32("usertype", UserType);

                    ViewData["LoginMessage"] = "Login Successful!";
                    DBClassReaders.LabOneDBConnection.Close();
                    DBClassReaders.AuthConn.Close();
                    return RedirectToPage("/Home");
                }
                else
                {
                    ViewData["LoginMessage"] = "Username and/or Password Incorrect";
                    DBClassReaders.LabOneDBConnection.Close();
                    return Page();
                }
            }
            else
            {
                ViewData["LoginMessage"] = "Please Enter A Username and Password";
                return Page();
            }
            
        }
        public IActionResult OnPostPopulateHandler()
        {
            if(!ModelState.IsValid){
                ModelState.Clear();

                Username = "admin";
                Password = "password";
            }
            return Page();
        }
        public IActionResult OnPostClearHandler()
        {
            return RedirectToPage("/Login");
        }

        public IActionResult OnPostLogoutHandler()
        {
            HttpContext.Session.Clear();
            DBClassReaders.LabOneDBConnection.Close();
            DBClassReaders.AuthConn.Close();
            TempData["LogoutMessage"] = "You have been successfully logged out.";
            return RedirectToPage("/Login");
        }
        public IActionResult OnPostCreateUserHandler()
        {
            return RedirectToPage("/CreateUser");
        }
    }
}
