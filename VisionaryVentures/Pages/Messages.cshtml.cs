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
        public int ChatID { get; set; }
        [BindProperty]
        public int UserID { get; set; }
        [BindProperty]
        public string NewChatTitle { get; set; }

        public List<Message> Messages { get; set; }
        public List<Chat>? Chats { get; set; }
        public string SelectedChatTitle { get; set; }


        public async Task OnGetAsync(int? SelectedChatID)
        {
            Messages = new List<Message>();
            Chats = new List<Chat>();

            using (var reader = DBClassReaders.ChatReader())
            {
                while (reader.Read())
                {
                    Chats.Add(new Chat
                    {
                        ChatID = reader.GetInt32(0),
                        DateCreated = reader.GetDateTime(1),
                        Title = reader.GetString(2)
                    });
                }
                DBClassReaders.LabOneDBConnection.Close();
            }

            if (SelectedChatID.HasValue)
            {
                var selectedChat = Chats.FirstOrDefault(c => c.ChatID == SelectedChatID.Value);
                if (selectedChat != null)
                {
                    SelectedChatTitle = selectedChat.Title; // Set the title of the selected collaboration
                }

                using (var reader = DBClassReaders.MessageReader(SelectedChatID))
                {
                    while (reader.Read())
                    {
                        Messages.Add(new Message
                        {
                            MessageID = reader.GetInt32(0),
                            ChatID = reader.GetInt32(1),
                            SentFrom = reader.GetInt32(2),
                            MessageContent = reader.GetString(3),
                            DateCreated = reader.GetDateTime(4),
                            FullName = $"{reader.GetString(5)} {reader.GetString(6)}"
                        });
                    }
                    DBClassReaders.LabOneDBConnection.Close();
                }
                HttpContext.Session.SetInt32("chatid", (int)SelectedChatID);
            }
        }

        // Add a message to the database
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(MessageContent))
            {
                UserID = (int)HttpContext.Session.GetInt32("userid");
                ChatID = (int)HttpContext.Session.GetInt32("chatid");
                DBClassWriters.AddMessage(ChatID, UserID, MessageContent, DateTime.Now);
                return RedirectToPage("/Messages", new { SelectedChatID = ChatID });
            }
            else
            {
                return Page();
            }
        }

        // Add a chat to the database
        public IActionResult OnPostAddChat()
        {
            if (!string.IsNullOrEmpty(NewChatTitle))
            {
                DBClassWriters.AddChat(NewChatTitle, DateTime.Now);
                return RedirectToPage("/Messages");
            }
            else
            {
                return Page();
            }
        }

    }
}

