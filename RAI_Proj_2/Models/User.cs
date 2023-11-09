namespace RAI_Proj_2.Models
{
	public class User
	{
		public string login { get; set; }
		public DateTime creationTime { get; set; }
		public List<User> friends { get; set; }
		public bool isAdmin { get; }

		public User(string login, DateTime? creationTime = null, List<User>? friends = null, bool isAdmin = false)
		{
			this.login = login;
			this.creationTime = creationTime != null ? (DateTime)creationTime : DateTime.Now;

			this.friends = new List<User>();
			if (friends != null)
			{
				foreach (User user in friends)
				{
					this.friends.Add(user);
				}
			}

			this.isAdmin = isAdmin;
		}

	}
}