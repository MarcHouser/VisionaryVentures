using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace VisionaryVentures.Pages
{
    public class MessagesModel : PageModel
    {
       
        [BindProperty]
        public String? MessageContent { get; set; }
        [BindProperty]
        public int CollaborationID { get; set; }
        [BindProperty]
        public int UserID { get; set; }

        public List<Message> Messages { get; set; }
        public List<Collaboration>? Collaborations { get; set; }


        public async Task OnGetAsync(int? SelectedCollaborationID)
        {
            Messages = new List<Message>();
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

            if (SelectedCollaborationID.HasValue)
            {
                using (var reader = DBClassReaders.MessageReader(SelectedCollaborationID))
                {
                    while (reader.Read())
                    {
                        Messages.Add(new Message
                        {
                            MessageID = reader.GetInt32(0),
                            CollaborationID = reader.GetInt32(1),
                            SentFrom = reader.GetInt32(2),
                            MessageContent = reader.GetString(3),
                            DateCreated = reader.GetDateTime(4),
                            FullName = $"{reader.GetString(5)} {reader.GetString(6)}"
                        });
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }
                HttpContext.Session.SetInt32("collaborationid", (int)SelectedCollaborationID);
            }
        }

        // Add a message to the database
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(MessageContent))
            {
                UserID = (int)HttpContext.Session.GetInt32("userid");
                CollaborationID = (int)HttpContext.Session.GetInt32("collaborationid");
                DBClassWriters.AddMessage(CollaborationID, UserID, MessageContent, DateTime.Now);
                return RedirectToPage("/Messages", new { SelectedCollaborationID = CollaborationID });
            }
            else
            {
                return Page();
            }
        }

    }
}

