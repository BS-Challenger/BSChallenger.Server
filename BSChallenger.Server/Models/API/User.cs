﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class User
	{
		public User(string userID)
		{
			UserId = userID;
			Id = Guid.NewGuid();
		}
		public User()
		{
			Id = Guid.NewGuid();
		}
		[Key]
		[IgnoreDataMember]
		public Guid Id { get; set; }

		//Username
		[Key]
		public string UserId { get; set; }
		public int BeatLeaderId;

		public string PasswordHash { get; set; }

		//Dynamically set
		public DateTime LastCheckDate { get; set; }
	}
}
