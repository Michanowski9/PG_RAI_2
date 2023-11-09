using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RAI_Proj_2.Models;
using System.Diagnostics;

namespace RAI_Proj_2.Controllers
{
	[Route("Error")]
	public class ErrorController : Controller
	{
		[Route("404")]
		public IActionResult Error404()
		{
			return View();
		}
	}
}