using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineChat.Models.Chats;
using OnlineChat.Models.Users;
using OnlineChat.Models.Lecturer;
using OnlineChat.Models.News;
using Microsoft.AspNetCore.Authorization;
using OnlineChat.ViewModels;
using Microsoft.AspNetCore.SignalR;
using OnlineChat.Hubs;
using Microsoft.AspNetCore.Http;
using System.IO;
using OnlineChat.Models.Documents;
using System.Security.Claims;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineChat.Controllers.NewsWall
{
    public class NewsWallController : Controller
    {
        LecturerRepository repos;
        UserDAO repos1;
        NewsRepository repos2;
        List<User> users;
        [TempData]
        public string Users { get; set; }

        public NewsWallController(LecturerRepository _repos, UserDAO _repos1, NewsRepository _repos2)
        {
            repos = _repos;
            repos1 = _repos1;
            repos2 = _repos2;
        }
        // GET: /<controller>/
        public IActionResult NewsWall(int id)
        {
            ViewBag.News = repos2.GetNews(id);
            if (this.User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                ViewBag.Admin = true;
            }
            else
            {
                ViewBag.Admin = false;
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditWall(CreateModel model)
        {
            ViewBag.Edit = true;
            ViewBag.News = repos2.GetNews(Convert.ToInt32(model.Name));
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteNews(CreateModel model)
        {
            
            ViewBag.News = repos2.DeleteNews(Convert.ToInt32(model.Name));
            return RedirectToAction("NewsPanel","NewsPanel");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(CreateModel model)
        {
            NewsModel news = new NewsModel();
            news= model.News;

            if (model.File != null)
                using (var binaryReader = new BinaryReader(model.File.OpenReadStream()))
                {
                    news.PictureName = model.File.ContentType;
                    news.PictureBytes = binaryReader.ReadBytes((int)model.File.Length);
                }
            if (model.File1 != null)
                using (var binaryReader = new BinaryReader(model.File1.OpenReadStream()))
                {
                    news.DocName = model.File1.FileName;
                    news.DocBytes = binaryReader.ReadBytes((int)model.File1.Length);
                }
            repos2.UpdateNews(news);
            return RedirectToAction("NewsWall", "NewsWall", new { id = news.Id });
        }
        public FileResult GetBytes(int id)
        {
            NewsModel news = repos2.GetNews(id);
            byte[] mas = news.DocBytes;
            string type = "";
            for (int i = news.DocName.LastIndexOf('.'); i < news.DocName.Length; i++)
            {
                type += news.DocName[i];
            }
            string file_type = "application/" + type;
            string file_name = news.DocName;
            return File(mas, file_type, file_name);
        }
    }
}
