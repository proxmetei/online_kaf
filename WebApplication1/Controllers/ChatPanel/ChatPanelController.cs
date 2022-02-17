using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineChat.Models.Chats;
using OnlineChat.Models.Users;
using OnlineChat.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using OnlineChat.ViewModels;
using Microsoft.AspNetCore.SignalR;
using OnlineChat.Hubs;
using Microsoft.AspNetCore.Http;
using System.IO;
using OnlineChat.Models.Documents;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineChat.Controllers.ChatPanel
{
    public class ChatPanelController : Controller
    {
        UserDAO repos;
        ChatDAO repos1;
        MessageDAO repos3;
        IHubContext<ChatHub> hubContext;
        public ChatPanelController(UserDAO _repos, ChatDAO _repos1, MessageDAO _repos3, IHubContext<ChatHub> hubContext)
        {
            repos = _repos;
            repos1 = _repos1;
            repos3 = _repos3;
            this.hubContext = hubContext;
        }
        public IActionResult ChatPanel()
        {
            ViewBag.Chats = new List<Chat>();
            User user = repos.GetUserOnEmail(this.User.FindFirstValue(ClaimTypes.Name));
            List<Chat> chats = repos.GetChats(user.Id);
            foreach (Chat chat in chats)
            {
               chat.Name = repos.GetOtherUser(chat.Id, user.Id);

                if (chat.Name != null)
                {
                    ViewBag.Chats.Add(chat);
                }
            }
            //ViewBag.Chats = chats;
           
            ViewBag.User = user;
            return View();
        }
        public IActionResult OpenChat(CreateModel model)
        {
            return RedirectToAction("Chat", "Chat", new { GUID = model.Text, id = Convert.ToInt32(model.Name) });
        }
    }
}
