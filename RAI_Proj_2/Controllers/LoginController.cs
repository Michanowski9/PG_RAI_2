using Microsoft.AspNetCore.Mvc;
using RAI_Proj_2.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RAI_Proj_2.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public ActionResult LogIn(string login)
		{
			foreach (var user in Globals.Users)
			{
				if (user.login == login)
				{
					HttpContext.Session.SetString("CurrentUserName", user.login);
					return RedirectToAction("List", "User");
				}
			}
			return RedirectToAction("Index", "Home");
		}
	}
}