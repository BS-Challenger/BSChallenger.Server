using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
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
		[Key, AutoIncrement]
		public int Id { get; set; }
		public string Username { get; set; }
		public int BeatLeaderId;

		public string PasswordHash { get; set; }

		//Dynamically set
		public DateTime LastCheckDate { get; set; }
	}
}
