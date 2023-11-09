using Microsoft.AspNetCore.Http;

namespace RAI_Proj_2.Models
{
    public class Globals
    {
		public static List<User> Users { get; set; } = new List<User>()
		{
			new User("admin", isAdmin: true)
		};
	}
}