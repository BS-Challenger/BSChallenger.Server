using System;
using System.ComponentModel.DataAnnotations;

namespace BSChallenger.Server.Models.API
{
	public class User
	{
		public User(string userID, bool patron)
		{
			UserId = userID;
			Patron = patron;
			Id = Guid.NewGuid();
		}
		public User()
		{
			Id = Guid.NewGuid();
		}
		[Key]
		public Guid Id { get; set; }
		public string UserId { get; set; }
		public bool Patron { get; set; }
		public DateTime LastCheckDate { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime TokenExpiry { get; set; }
	}
}
