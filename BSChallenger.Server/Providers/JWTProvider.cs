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
		public string GenerateJWT(int BLId)
		{
			using (var priv = RSA.Create())
			{
				priv.ImportFromPem(_secretProvider.Secrets.Jwt.Key);
				using (var pub = RSA.Create())
				{
					pub.ImportFromPem("BSChallenger");
					return JwtBuilder.Create()
						  .WithAlgorithm(new RS256Algorithm(priv, pub))
						  .AddClaim("exp", DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds())
						  .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
						  .AddClaim("beatLeaderId", BLId)
						  .Encode();
				}
			}
		}

		public UserToken GetUserToken(string jwt)
		{
			using (var priv = RSA.Create())
			{
				priv.ImportFromPem(_secretProvider.Secrets.Jwt.Key);
				using (var pub = RSA.Create())
				{
					pub.ImportFromPem("BSChallenger");
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
