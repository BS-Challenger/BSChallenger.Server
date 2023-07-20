using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Token
	{
		public Token(User user, DateTime expiry, TokenType type)
		{
			UserId = user.Id;
			tokenType = type;
			expiryTime = expiry;
			token = type == TokenType.BLAuthToken ?
					GenerateAuthToken() :
					Guid.NewGuid().ToString().Replace("-", string.Empty).Replace("+", string.Empty);
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
		public TokenType tokenType { get; private set; }

		private static string GenerateAuthToken()
		{
			byte[] buffer = new byte[12];
			Random.Shared.NextBytes(buffer);
			return Convert.ToBase64String(buffer).Substring(0, 12);
		}
	}

	public enum TokenType
	{
		RefreshToken,
		AccessToken,
		BLAuthToken
	}
}
