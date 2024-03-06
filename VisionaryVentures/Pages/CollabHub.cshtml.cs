using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisionaryVentures.Pages
{
    public class CollabHubModel : PageModel
    {
        public string UserFirstName { get; set; }
        public List<Collaboration> Collaborations { get; set; }

        [BindProperty]
        public Collaboration NewCollaboration { get; set; }
        
        [BindProperty]
        public int CollabID { get; set; }

        public IActionResult OnGet()
        {
            Collaborations = new List<Collaboration>();

            using (var reader = DBClassReaders.CollaborationReader())
            {
                while (reader.Read())
                {
                    Collaborations.Add(new Collaboration
                    {
                        CollaborationID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }
                DBClassReaders.LabOneDBConnection.Close();
            }

            //var username = HttpContext.Session.GetString("username");
            //UserFirstName = DBClassReaders.GetFirstNameByUsername(username);
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // If model state is not valid, return to the page
                return Page();
            }

            // Insert the new collaboration into the database
            //DBClassWriters.AddCollaboration(NewCollaboration);

            // Redirect to the CollabHub page to display the updated list of collaborations
            return RedirectToPage("CollabHub");
        }
    }
}
