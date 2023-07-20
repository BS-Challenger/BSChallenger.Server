using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Models
{
	public class TokenProvider
	{
		private readonly ILogger _logger = Log.ForContext<TokenProvider>();
		private readonly Database _database;

		public TokenProvider(
			Database database)
		{
			_database = database;
		}

		public async Task<Token> GetRefreshToken(User user)
		{
			await RemoveOldToken(user, TokenType.RefreshToken);
			var token = new Token(user, DateTime.Now.AddMonths(1), TokenType.RefreshToken);
			await _database.Tokens.AddAsync(token);
			await _database.SaveChangesAsync();

			return token;
		}

		public async Task<Token> GetAccessToken(User user)
		{
			await RemoveOldToken(user, TokenType.AccessToken);
			var token = new Token(user, DateTime.Now.AddMinutes(5), TokenType.AccessToken);
			await _database.Tokens.AddAsync(token);
			await _database.SaveChangesAsync();
			return token;
		}

		public async Task<Token> GetBLAuthToken(User user)
		{
			await RemoveOldToken(user, TokenType.BLAuthToken);
			var token = new Token(user, DateTime.Now.AddMinutes(5), TokenType.BLAuthToken);
			await _database.Tokens.AddAsync(token);
			await _database.SaveChangesAsync();
			return token;
		}

		private async Task RemoveOldToken(User user, TokenType type)
		{
			if (_database.Tokens.AsEnumerable().Any(x => x.UserId == user.Id && (x.tokenType == type)))
			{
				var oldToken = _database.Tokens.AsEnumerable().First(x => x.UserId == user.Id && (x.tokenType == type));
				_logger.Information("Old Token:" + oldToken.token);
				_database.Tokens.Remove(oldToken);
				await _database.SaveChangesAsync();
			}
		}
	}
}
