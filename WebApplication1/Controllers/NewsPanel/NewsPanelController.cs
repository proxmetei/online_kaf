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

namespace OnlineChat.Controllers.NewsPanel
{
    public class NewsPanelController : Controller
    {

        LecturerRepository repos;
        UserDAO repos1;
        NewsRepository repos2;
        List<User> users;
        [TempData]
        public string Users { get; set; }
       
        public NewsPanelController(LecturerRepository _repos, UserDAO _repos1, NewsRepository _repos2)
        {
            repos = _repos;
            repos1 = _repos1;
            repos2 = _repos2;
        }
        public IActionResult NewsPanel(CreateModel model)
        {
            ViewBag.News = new NewsModel();
            List<NewsModel> mynews = new List<NewsModel>();
            int index;
            if (model.Name == null)
            {
                index = 0;
            
            }
            else
            {
                index = Convert.ToInt32(model.Name);
            }
            List<NewsModel> news = repos2.GetAllNews();

            bool finish = false, start = false ;
            for (int i = 0; i < 10; i++)
            {
                int j = news.Count() - index - i-1;
                if (j >= 0)
                {
                    mynews.Add(news.ElementAt(j));
                    ViewBag.NewIndex=i+index+1;
                }
                else
                {
                    finish = true;
                }
            }
            ViewBag.PrevIndex = index - 10;
            if (this.User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                ViewBag.Admin = true;
            }
            else
            {
                ViewBag.Admin = false;
            }
            if (index == 0)
                start = true;
            ViewBag.News = mynews;
            ViewBag.Finish = finish;
            ViewBag.Start = start;
            return View();
        }
        public async Task<IActionResult> ShowNews(string id)
        {

            return RedirectToAction("NewsWall", "NewsWall",new { id=Convert.ToInt32(id)});
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateModel model)
        {
            NewsModel news = new NewsModel();
            if (model.News.Title == null)
            {
                ModelState.AddModelError("", "Нет заголвка");
            }
            if (model.News.Content == null)
            {
                ModelState.AddModelError("", "Нет описания новости");
            }
            news.Title = model.News.Title;
            news.Content = model.News.Content;

            if (model.File != null)
            {
                news.PictureName = model.File.ContentType;
                using (var binaryReader = new BinaryReader(model.File.OpenReadStream()))
                {
                    news.PictureBytes = binaryReader.ReadBytes((int)model.File.Length);
                }
            }
            
            if (model.File1 != null)
            {
                news.DocName = model.File1.FileName;
                using (var binaryReader = new BinaryReader(model.File1.OpenReadStream()))
                {
                    news.DocBytes = binaryReader.ReadBytes((int)model.File1.Length);
                }
                
            }
            repos2.AddNews(news);
            
  
            return RedirectToAction("NewsPanel", "NewsPanel");
        }
            

        }
}

