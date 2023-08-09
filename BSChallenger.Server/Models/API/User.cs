using System;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class User
	{
		public User(string username)
		{
			Username = username;
		}
		public User()
		{
		}
		[Key]
		public int Id { get; set; }
		public string Username { get; set; }
		public string BeatLeaderId { get; set; }

		public string PasswordHash { get; set; }

		//Dynamically set
		public DateTime LastCheckDate { get; set; }
	}
}