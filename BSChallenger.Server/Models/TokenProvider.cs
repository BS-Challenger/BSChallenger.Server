using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using System;
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
			await _database.Tokens.AddAsync(token);
			await _database.SaveChangesAsync();
			return token;
		}
	}
}
