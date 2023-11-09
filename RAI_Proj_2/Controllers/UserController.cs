using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RAI_Proj_2.Models;
using System.Diagnostics;
using System.Text;

namespace RAI_Proj_2.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult LogOut()
		{
			HttpContext.Session.Remove("CurrentUserName");
			return RedirectToAction("Index", "Home");
		}
		public IActionResult List()
		{
			var currentUser = GetCurrentUser();
			return View("~/Views/User/Friends/List.cshtml", currentUser);
		}
		public IActionResult Add()
		{
			return View("~/Views/User/Friends/Add.cshtml");
		}

		[HttpPost]
		public ActionResult Add(string login)
		{
			var currentUser = GetCurrentUser();
			if (currentUser == null) return RedirectToAction("Index", "Home");

			ViewBag.Success = false;
			foreach (var user in Globals.Users)
			{
				if (user.login == login && user != currentUser)
				{
					currentUser.friends.Add(user);
					ViewBag.Success = true;
					break;
				}
			}

			return View("~/Views/User/Friends/Result.cshtml");
		}
		[HttpPost]
		public ActionResult Delete(string login, IFormCollection collection)
		{
			var currentUser = GetCurrentUser();
			if (currentUser == null) return RedirectToAction("Index", "Home");

			try
			{
				currentUser.friends.RemoveAll(user => user.login == login);
				ViewBag.Success = true;
			}
			catch
			{
				ViewBag.Success = false;
			}
			return View("~/Views/User/Friends/Result.cshtml");
		}
		[HttpPost]
		public ActionResult Import(IFormFile postedFile)
		{
			var currentUser = GetCurrentUser();
			if (currentUser == null) return RedirectToAction("Index", "Home");

			currentUser.friends.Clear();

			var newFriends = new List<string>();
			using (var reader = new StreamReader(postedFile.OpenReadStream()))
			{
				while (reader.Peek() >= 0)
				{
					newFriends.Add(reader.ReadLine() ?? "");
				}
			}
			currentUser.friends.AddRange(Globals.Users.Where(user => newFriends.Contains(user.login)));

			return View("~/Views/User/Friends/List.cshtml", currentUser);
		}
		public ActionResult Export()
		{
			var currentUser = GetCurrentUser();
			if (currentUser == null) return RedirectToAction("Index", "Home");

			var friends = currentUser.friends.Select(user => user.login).ToList();

			string filePath = $"exported_friends_" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + ".txt";

			byte[] fileContent = Encoding.ASCII.GetBytes(String.Join("\n", friends.ToArray()));

			return new FileContentResult(fileContent, "text/txt")
			{
				FileDownloadName = filePath
			};
		}

		private User? GetCurrentUser()
		{
			var currentUserName = HttpContext.Session.GetString("CurrentUserName");
			if (currentUserName == null)
			{
				return null;
			}

			return Globals.Users.Find(i => i.login == currentUserName);
		}
	}
}