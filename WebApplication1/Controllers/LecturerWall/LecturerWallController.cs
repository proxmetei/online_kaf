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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineChat.Controllers.LecturerWall
{
    public class LecturerWallController : Controller
    {
        LecturerRepository repos;
        WallMessageRepository repos1;
        UserDAO repos2;
        //[TempData]
        //public byte[] Photo { get; set; }
        public LecturerWallController(LecturerRepository _repos,WallMessageRepository _repos1,UserDAO _repos2)
        {
            repos = _repos;
            repos1 = _repos1;
            repos2 = _repos2;
        }
        // GET: /<controller>/
        public IActionResult Wall(int id)
        {
            ViewBag.Edit = false;
            if (this.User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                ViewBag.Admin = true;
            }
            else
            {
                ViewBag.Admin = false;
            }
            LecturerModel lec = repos.GetLecturerByUserId(id);
            ViewBag.User = repos2.Get(lec.UserId);
            ViewBag.Lecturer = lec;
            ViewBag.News = repos1.GetAllPosts(lec.Id);
            ViewBag.Photo = null;
            if(lec.Photo!=null)
            ViewBag.Photo = Convert.ToBase64String(lec.Photo);
            ViewBag.PhotoString = lec.PhotoName;
            if (User.Identity.IsAuthenticated)
            {
                User user1 = repos2.GetUserOnEmail(User.FindFirstValue(ClaimTypes.Name));

                ViewBag.UserId = user1.Id;

            
            }
            if (User.Identity.IsAuthenticated&&this.User.FindFirstValue(ClaimTypes.Role) != "User")
            {
                ViewBag.Write = true;
            }
            else
            {
                ViewBag.Write = false;
            }
            //Photo = lec.Photo;
            return View();
        }
        public IActionResult MyWall(int id)
        {
            ViewBag.Edit = false;
            if (this.User.FindFirstValue(ClaimTypes.Role) == "Admin")
            {
                ViewBag.Admin = true;
            }
            else
            {
                ViewBag.Admin = false;
            }
            LecturerModel lec = repos.GetLecturerByUserId(id);
            ViewBag.User = repos2.Get(lec.UserId);
            //Photo = lec.Photo;
            ViewBag.Photo = null;
            if (lec.Photo != null)
                ViewBag.Photo = Convert.ToBase64String(lec.Photo);
            ViewBag.PhotoString = lec.PhotoName;
            ViewBag.Lecturer = lec;
            ViewBag.News = repos1.GetAllPosts(lec.Id);
            if (User.Identity.IsAuthenticated && this.User.FindFirstValue(ClaimTypes.Role) != "User")
            {
                ViewBag.Write = true;
            }
            else
            {
                ViewBag.Write = false;
            }
            return View("Wall");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditWall(CreateModel model)
        {
            ViewBag.Edit = true;
            LecturerModel lec = repos.GetLecturerByUserId(Convert.ToInt32(model.Name));
            ViewBag.User = repos2.Get(lec.UserId);
            ViewBag.Photo = null;
            if (lec.Photo != null)
                ViewBag.Photo = Convert.ToBase64String(lec.Photo);
            ViewBag.PhotoString = lec.PhotoName;
            //Photo = lec.Photo;
            ViewBag.Lecturer = lec;
            ViewBag.News = repos1.GetAllPosts(lec.Id);
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(LeturerCreateModel model)
        {
            LecturerModel lec = new LecturerModel();
            lec.Id = model.Id;
            lec.Publications = model.Publications;
            lec.Achivements = model.Achivements;
            lec.TeachingInfo = model.TeachingInfo;
            lec.UserId = model.UserId;
            
            if (model.Photo != null)
                using (var binaryReader = new BinaryReader(model.Photo.OpenReadStream()))
                {
                    lec.PhotoName = model.Photo.ContentType;
                    lec.Photo = binaryReader.ReadBytes((int)model.Photo.Length);
                }
            repos.UpdateLecturer(lec);
            CreateModel model1 = new CreateModel();
            model1.Name = lec.UserId.ToString();
            return RedirectToAction("MyWall", "LecturerWall", new {id = lec.UserId});
        }
       
        public IActionResult AddPost(CreateModel model)
        {
            WallMessageModel message = new WallMessageModel();
            LecturerModel lec = repos.GetLecturerByUserId(Convert.ToInt32(model.Name));
            message.LecturerId =lec.Id;
            message.Content = model.Text;
            repos1.AddPost(message);
            return RedirectToAction("MyWall", "LecturerWall", new { id = lec.UserId });
        }
    }
}
