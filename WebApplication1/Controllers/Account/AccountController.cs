using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineChat.Models.Users;
using OnlineChat.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using OnlineChat.Models;
using log4net;
namespace OnlineChat.Controllers.Account
{
	public class AccountController : Controller
	{
		UserDAO repos;
		LogFactory logFactory;
		private ILog log;
		public AccountController(UserDAO _repos, LogFactory _logFactory)
		{
			repos = _repos;
			logFactory = _logFactory;
			log = logFactory.GetLogger();
		}
		public ActionResult Register()
		{		
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Registrate(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				User user = repos.GetUserOnEmail(model.Email);		
				if(user!=null)
				{
					ModelState.AddModelError("", "Аккаунт с данным Email уже зарегистрирован");
					return View("Register");
				}
				else
				{
					User user2 = new User
					{
						Email = model.Email,
						Password =BCrypt.Net.BCrypt.HashPassword( model.Password),
						FIO = model.FIO,
						BirthDate = model.Birthday.Date,
						Status = "User"
					};
					if (repos.GetAllUsers().Count == 0)
					{
						user2.Status = "Admin";
					}
					repos.AddUser(user2);
					log.Info("New user" + " " + model.Email);
					await Authenticate(model.Email,user2.Status);
				}
			}		
			return RedirectToAction("Login","Account");
		}

		
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogIn(LoginModel model)
		{
			if(ModelState.IsValid)
			{
				User user = repos.GetUserOnEmail(model.Email);
				if (user != null)
				{
					if (BCrypt.Net.BCrypt.Verify( model.Password, user.Password))					
					{
	
						await Authenticate(model.Email,user.Status);
						return RedirectToAction("Index", "Home");
					}
					else
					{
						log.Info("Неверно введенный пароль " + '\n' + "Email: " + model.Email);
						ModelState.AddModelError("", "Неверный логин или пароль");
					}
										
				}
				
			}
			return RedirectToAction("Login","Account") ;
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account");
		}
		private async Task Authenticate(string userName,string role)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, role)

			};
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}
	}
}
