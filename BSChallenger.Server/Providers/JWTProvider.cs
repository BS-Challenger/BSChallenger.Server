using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BSChallenger.Server.Providers
{
	public class JwtProvider
	{
		private readonly Database _database;
		private readonly SecretProvider _secretProvider;
		public JwtProvider(
			Database database,
			SecretProvider provider)
		{
			_database = database;
			_secretProvider = provider;
		}
		public string GenerateJWT(string BLId)
		{
			var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(new SecurityTokenDescriptor()
			{
				Claims = new Dictionary<string, object>()
				{
					[ClaimTypes.NameIdentifier] = BLId,
				},
				Expires = DateTime.UtcNow.AddMonths(1),
				IssuedAt = DateTime.UtcNow,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretProvider.Secrets.Jwt.Key)), SecurityAlgorithms.HmacSha256Signature)
			});

			return token.RawData;
		}
	}
}
