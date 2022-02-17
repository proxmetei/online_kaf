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

namespace OnlineChat.Controllers.TeacherCardEditController
{
    public class TeacherCardEditController : Controller
    {


            LecturerRepository repos;
        UserDAO repos1;
        List<LecturerModel> lecs;
        [TempData]
            public string Users { get; set; }
        [TempData]
        public Boolean Edit { get; set; }
        public TeacherCardEditController(LecturerRepository _repos,UserDAO _repos1)
            {
                repos = _repos;
            repos1 = _repos1;
            }
        public async Task<IActionResult> ShowLec(string id)
        {

            return RedirectToAction("Wall", "LecturerWall", new { id = Convert.ToInt32(id) });
        }
        public IActionResult LecturerPanel()
        {
            if (Users == null)
            {
                ViewBag.Users = repos.GetAllLecturers();
                lecs = new List<LecturerModel>();
                foreach (LecturerModel lec in ViewBag.Users)
                {
                    User user = new User();
                    user = repos1.Get(lec.UserId);
                    lec.FIO = user.FIO;
                    lecs.Add(lec);
                }
                ViewBag.Users = lecs;
            }
            else
            {
                ViewBag.Users = JsonConvert.DeserializeObject<List<LecturerModel>>(Users);
                lecs = new List<LecturerModel>();
                foreach (LecturerModel lec in ViewBag.Users)
                {

                    lecs.Add(lec);
                }
                ViewBag.Users = lecs;
            }
                return View();
            }
            public async Task<IActionResult> ShowCard(CreateModel model)
            {
            Edit = true;
                return RedirectToAction("LecturerRoleController", "Admin");
            }
            //public async Task<IActionResult> Search(CreateModel model)
            //{
            //    if (model.Text != null)
            //        Users = JsonConvert.SerializeObject(repos.SearchUsers(model.Text, this.User.FindFirstValue(ClaimTypes.Name)));
            //    return RedirectToAction("AdminPanel", "Admin");
            //}
        
    }

}
