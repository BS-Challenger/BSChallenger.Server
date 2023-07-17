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
			token = Guid.NewGuid().ToString().Replace("-", string.Empty).Replace("+", string.Empty);
			Id = Guid.NewGuid();
		}

		public Token()
		{
			Id = Guid.NewGuid();
		}
		[Key]
		public Guid Id { get; set; }
		public int UserId { get; set; }
		public string token { get; set; }
		public DateTime expiryTime { get; set; }
		public bool isAccessToken { get; private set; }
	}
}
