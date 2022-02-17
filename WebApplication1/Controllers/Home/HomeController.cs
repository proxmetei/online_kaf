using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineChat.Models.Chats;
using OnlineChat.Models.Users;
using OnlineChat.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace OnlineChat.Controllers
{
	//[Authorize]
	public class HomeController : Controller
	{
		UserDAO repos;
		public HomeController(UserDAO _repos)
		{
			repos = _repos;
		}
		public  IActionResult Index()
		{
			if(TempData["Count"]!=null)
			TempData["Count"] = 0;
			ViewBag.Admin = false;
		   if (this.User.FindFirstValue(ClaimTypes.Role) == "Admin")
			{
				ViewBag.Admin = true;
			}
			User user = repos.GetUserOnEmail(this.User.FindFirstValue(ClaimTypes.Name));
			return View();
		}
		//[HttpPost]
		//public async Task<IActionResult> Create(CreateModel model)
		//{

		//	ViewBag.Admin = false;
		//	if (this.User.FindFirstValue(ClaimTypes.Role) == "Admin")
		//	{
		//		ViewBag.Admin = true;
		//	}
		//	Chat chat = repos1.GetChatOnName(model.Name);
		//	if (chat != null)
		//	{
		//		string email = model.Email;
		//		User user = repos.GetUserOnEmail(email);
		//		List<Chat> chats = repos.GetChats(user.UserId);
		//		List<string> names = new List<string>();
		//		foreach (Chat chat1 in chats)
		//		{
		//			names.Add(chat1.Name);
		//		}
		//		ViewBag.Chats = names;
		//		ViewBag.Email = email;
		//		ViewBag.Error = "Чат с данным именем существует";
		//		return View("Index");
		//	}
		//	repos1.Create(new Chat {Name=model.Name,GUID=Guid.NewGuid().ToString()},model.Email);
		//	return RedirectToAction("Index", "Home");
		//}
		//public async Task<IActionResult> Go(CreateModel model)
		//{
		//	return RedirectToAction("ForLogged", "Chat", new { model.Name, model.Email});
		//}
	}
}
