using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RAI_Proj_2.Models;
using System.Diagnostics;

namespace RAI_Proj_2.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			if (!hasAccess()) return RedirectToAction("Index", "Home");

			return View();
		}
		public ActionResult Add()
		{
			if (!hasAccess()) return RedirectToAction("Index", "Home");

			return View();
		}
		public ActionResult List()
		{
			if (!hasAccess()) return RedirectToAction("Index", "Home");

			return View(Globals.Users);
		}
		public ActionResult Init()
		{
			if (!hasAccess()) return RedirectToAction("Index", "Home");

			List<User> users = new List<User>();
			users.Add(new RAI_Proj_2.Models.User("user_1"));
			users.Add(new RAI_Proj_2.Models.User("user_2", friends: new List<User>() { users[0] }));
			users[0].friends.Add(users[1]);
			users.Add(new RAI_Proj_2.Models.User("user_3", friends: new List<User>() { users[1] }));
			users.Add(new RAI_Proj_2.Models.User("user_4", friends: new List<User>() { users[0], users[1], users[2] }));
			users.Add(new RAI_Proj_2.Models.User("user_5", friends: new List<User>() { users[1], users[2], users[3] }));
			users.Add(new RAI_Proj_2.Models.User("user_6", friends: new List<User>() { users[2], users[3], users[4] }));

			foreach (var user in users)
			{
				Globals.Users.Add(user);
			}
			return RedirectToAction("List", "Admin");
		}

		[HttpPost]
		public ActionResult Add(string login)
		{
			if (!hasAccess()) return RedirectToAction("Index", "Home");

			if (!Globals.Users.Select(user => user.login).Contains(login))
			{
				Globals.Users.Add(new User(login));
			}
			return RedirectToAction("List", "Admin");
		}
		[HttpPost]
		public ActionResult Delete(string login, IFormCollection collection)
		{
			if (!hasAccess()) return RedirectToAction("Index", "Home");

			Globals.Users.RemoveAll(user => user.login == login && user.isAdmin == false);
			return RedirectToAction("List", "Admin");
		}

		private bool hasAccess()
		{
			var currentUserName = HttpContext.Session.GetString("CurrentUserName");
			if (currentUserName == null) return false;

			var currentUser = Globals.Users.Find(i => i.login == currentUserName);
			if (currentUser == null) return false;

			return currentUser.isAdmin;
		}
	}
}