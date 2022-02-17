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
namespace OnlineChat.Controllers.Chats
{
    public class ChatController : Controller
    {
        UserDAO repos;
        ChatDAO repos1;
        MessageDAO repos3;
        IHubContext<ChatHub> hubContext;
        public ChatController(UserDAO _repos,ChatDAO _repos1, MessageDAO _repos3, IHubContext<ChatHub> hubContext)
		{
            repos = _repos;
            repos1 = _repos1;
            repos3 = _repos3;
            this.hubContext = hubContext;
        }
        //[Authorize]
        //public async Task<IActionResult> ForLogged(string name, string email)
        //{
        //    User user = repos.GetUserOnEmail(email);
        //    Chat chat = repos1.GetChatOnName(name);
        //    return RedirectToAction("Chat", "Chat", new { chat.GUID });
        //}
       
        [TempData]
        public int Count { get; set; }
        [TempData]
        public int AnotherUserId { get; set; }
        [TempData]
        public bool Start { get; set; }
        public IActionResult OpenChat(CreateModel model)
        {
            User user = repos.GetUserOnEmail(this.User.FindFirstValue(ClaimTypes.Name));
            Chat chat = repos1.GetOnUsers(Convert.ToInt32(model.Name),user.Id);
           return RedirectToAction("Chat", "Chat", new { GUID = chat?.GUID, id = Convert.ToInt32(model.Name) });
        }
            public IActionResult Chat(string GUID,int id)
        {
            if (GUID != null)
            {
                ViewBag.GUID = GUID;
            Chat myChat = repos1.GetOnGUID(GUID);
            string name = repos1.GetOnGUID(GUID).Name;
            //ViewBag.ChatName = name;
           
                if (User.Identity.IsAuthenticated && this.User.FindFirstValue(ClaimTypes.Role) != "User")
                {

                    User user1 = repos.GetUserOnEmail(this.User.FindFirstValue(ClaimTypes.Name));
                    //if (user1 == null)
                    //{
                    //    user1 = repos.GetUserOnEmail(this.User.FindFirstValue(ClaimTypes.Name));
                    //}
                    //List<Chat> mychats = repos.GetChats(user1.Id);
                    //bool Join = true;
                    //foreach (Chat mychat in mychats)
                    //{
                    //    if (mychat.Name == name)
                    //        Join = false;
                    //}
                    //ViewBag.Join = Join;
                    ViewBag.Login = user1.Email;
                    ViewBag.UserId = user1.Id;
                }
                //Chat myChat = repos1.GetChatOnName(name);
                ViewBag.Name = name;
                ViewBag.Id = myChat.Id;
                List<Message> messages = repos1.GetMessages(myChat.Id);
                List<Message> texts = new List<Message>();
                List<string> logins = new List<string>();
                int kolvo = Count;
                TempData.Keep("Count");
                int i = 0;
                int messagesBegin = messages.Count() - kolvo - 10;
                int k = 0;
                foreach (Message mes in messages)
                {

                    if (mes.UserId != 0 && k > messagesBegin)
                    {
                        if (mes.DocId != 0)
                            mes.Doc = repos3.GetDoc(mes.DocId);
                        texts.Add(mes);
                        User user = repos.Get(mes.UserId);
                        logins.Add(user.FIO);

                    }
                    k++;
                }
                bool load = messages.Count() > kolvo + 10;
                ViewBag.Load = load;
                //if (User.Identity.IsAuthenticated)
                //{
                //    ViewBag.Auth = true;
                //}
                //else
                //{
                //    ViewBag.Auth = false;
                //}
                ViewBag.Messages = messages;
                ViewBag.Texts = texts;
                ViewBag.Names = logins;
                Start = true;
                TempData.Keep();
                ViewBag.Start = true;
            }
            else
            {
                Start = false;
                TempData.Keep();
                ViewBag.Start = false;
                AnotherUserId = id;
                ViewBag.Load = false;
            }
           
            return View();
        }
        public async Task<IActionResult> Loadmore(CreateModel model)
        {
           
            Count += 10;
            TempData.Keep("Count");
            Chat chat = repos1.GetOnGUID(model.Name);
            return RedirectToAction("Chat", "Chat", new { chat.GUID });
        }
        //public async Task<IActionResult> JoinChat(CreateModel model)
        //{
        //    User user = repos.Get(Convert.ToInt32(model.Email));
        //    Chat chat = repos1.GetOnGUID(model.Name);
        //    repos1.Join(user.Email,chat);
        //    return RedirectToAction("Chat", "Chat", new { chat.GUID });
        //}
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            Chat chat = new Chat();
            if (Start)
            {
                TempData.Keep();
                chat = repos1.GetOnGUID(model.Name);
            }
            else
            {
                Chat chat1 = new Chat();
                chat1.GUID = Guid.NewGuid().ToString();
                repos1.Create(chat1, this.User.FindFirstValue(ClaimTypes.Name));
                chat = repos1.GetOnGUID(chat1.GUID);
                repos1.Join(AnotherUserId,chat.Id);
            }
            byte[] post = null;
            if(model.File!=null)
                using (var binaryReader = new BinaryReader(model.File.OpenReadStream()))
                {
                    post = binaryReader.ReadBytes((int)model.File.Length);
                }
            User user = repos.GetUserOnEmail(this.User.FindFirstValue(ClaimTypes.Name));
            Message message = new Message ();
            message.ChatId = chat.Id;
            message.Text = model.Text;
            message.UserId = user.Id;
            //if (this.User.FindFirstValue(ClaimTypes.Role) == "Admin")
            //{
            //    message.Color = 1;
            //}
                string login = user.FIO;
            if (model.File == null)
            {
                repos3.CreateInChat(message);
            }
            else
            {

                Document doc = new Document { GUID = Guid.NewGuid().ToString(), Name=model.File.FileName,Data=post};
                repos3.CreateWithDoc(message,doc);
            }
            message.Doc = repos3.GetDoc(message.DocId);
            //message.Doc = null;
            await hubContext.Clients.Group(chat.GUID).SendAsync("ReceiveMessage", login, message);
            return RedirectToAction("Chat", "Chat", new { chat.GUID });
        }
        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyToAsync(target);
                return target.ToArray();
            }
        }

        public FileResult GetBytes(string GUID)
        {
            Document doc = repos3.GetDocument(GUID);
            byte[] mas = doc.Data;
            string type = "";
            for (int i = doc.Name.LastIndexOf('.'); i < doc.Name.Length; i++)
            {
                type += GUID[i];
            }
            string file_type = "application/"+type;
            string file_name = doc.Name ;
            return File(mas, file_type, file_name);
        }
        }
}
