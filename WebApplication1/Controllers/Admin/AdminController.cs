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
using Newtonsoft.Json;

namespace OnlineChat.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        UserDAO repos;
        [TempData]
        public string Users { get; set; }
        public AdminController(UserDAO _repos)
        {
            repos = _repos;
        }
        public IActionResult AdminPanel()
        {
            if (Users == null)
                ViewBag.Users = repos.GetAllUsersExceptYourself(this.User.FindFirstValue(ClaimTypes.Name));
            else
            {
                ViewBag.Users = JsonConvert.DeserializeObject<List<User>>(Users); ;
            }
            return View();
        }
        public async Task<IActionResult> ChangeStatus(CreateModel model)
        {
            repos.ChangeStatus(model.Text,model.Name);
          return  RedirectToAction("AdminPanel","Admin");
        }
        public async Task<IActionResult> Search(CreateModel model)
        {
            if(model.Text!=null)
          Users=JsonConvert.SerializeObject(repos.SearchUsers(model.Text, this.User.FindFirstValue(ClaimTypes.Name)));
            return RedirectToAction("AdminPanel", "Admin");
        }
    }
}
