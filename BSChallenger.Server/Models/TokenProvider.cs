using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Models
{
	public class TokenProvider
	{
		private readonly Database _database;

		public TokenProvider(
			Database database)
		{
			_database = database;
		}

		public async Task<Token> GetRefreshToken(User user)
		{
			var token = new Token(user, DateTime.Now.AddMonths(1), false);
			RemoveOldToken(user, false);
			await _database.Tokens.AddAsync(token);
			await _database.SaveChangesAsync();
			return token;
		}

		public async Task<Token> GetAccessToken(User user)
		{
			var token = new Token(user, DateTime.Now.AddMinutes(5), true);
			RemoveOldToken(user, true);
			await _database.Tokens.AddAsync(token);
			await _database.SaveChangesAsync();
			return token;
		}

		private void RemoveOldToken(User user, bool access)
		{
			if (_database.Tokens.AsEnumerable().Any(x => x.UserId == user.Id && (x.isAccessToken == access)))
			{
				var oldToken = _database.Tokens.AsEnumerable().First(x => x.UserId == user.Id && (x.isAccessToken == access));
				_database.Tokens.Remove(oldToken);
			}
		}
	}
}
