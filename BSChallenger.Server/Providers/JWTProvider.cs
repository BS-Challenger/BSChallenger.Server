using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using System.Collections.Generic;
using JWT.Builder;
using System;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.DataProtection;
using NuGet.Common;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Models;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace BSChallenger.Server.Providers
{
	public class JWTProvider
	{
		private readonly Database _database;
		private readonly SecretProvider _secretProvider;
		public JWTProvider(
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
				Claims = new Dictionary<string, object?>()
				{
					[ClaimTypes.NameIdentifier] = BLId,
				},
				Expires = DateTime.UtcNow.AddMonths(1),
				IssuedAt = DateTime.UtcNow,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretProvider.Secrets.Jwt.Key)), SecurityAlgorithms.HmacSha256Signature)
			});

			return token.RawData;
		}

		public UserToken GetUserToken(string jwt)
		{
			using (var priv = RSA.Create())
			{
				priv.ImportFromPem(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "private.pem")));
				using (var pub = RSA.Create())
				{
					pub.ImportFromPem(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "public.pem")));
					return JwtBuilder.Create()
						.WithAlgorithm(new RS256Algorithm(priv, pub))
						.MustVerifySignature()
						.Decode<UserToken>(jwt);
				}
			}
		}

		public User FindUser(UserToken token) => _database.Users.FirstOrDefault(x => x.BeatLeaderId == token.BeatLeaderId);
	}
}
