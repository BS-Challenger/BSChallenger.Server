using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Token
	{
		public Token(User user, DateTime expiry, bool access)
		{
			UserId = user.Id;
			isAccessToken = access;
			expiryTime = expiry;
			Id = Guid.NewGuid();
		}
		public Token(User user)
		{
			UserId = user.Id;
			Id = Guid.NewGuid();
		}
		[Key]
		[IgnoreDataMember]
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string token;
		public DateTime expiryTime;
		public bool isAccessToken;
	}
}
