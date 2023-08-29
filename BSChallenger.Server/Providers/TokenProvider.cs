using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Providers
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
            var token = new Token(user, DateTime.Now.ToUniversalTime().AddMonths(1), TokenType.RefreshToken);
            await _database.Tokens.AddAsync(token);
            user.Tokens.Add(token);
            await _database.SaveChangesAsync();

            return token;
        }

        public async Task<Token> GetAccessToken(User user)
        {
            await RemoveOldToken(user, TokenType.AccessToken);
            var token = new Token(user, DateTime.Now.ToUniversalTime().AddMinutes(5), TokenType.AccessToken);
            await _database.Tokens.AddAsync(token);
            user.Tokens.Add(token);
            await _database.SaveChangesAsync();
            return token;
        }

        public async Task<Token> GetBLAuthToken(User user)
        {
            await RemoveOldToken(user, TokenType.BLAuthToken);
            var token = new Token(user, DateTime.Now.ToUniversalTime().AddMinutes(5), TokenType.BLAuthToken);
            await _database.Tokens.AddAsync(token);
            user.Tokens.Add(token);
            await _database.SaveChangesAsync();
            return token;
        }

        private async Task RemoveOldToken(User user, TokenType type)
        {
            if (user?.Tokens?.Any(x => x?.TokenType == type) == true)
            {
                var oldToken = user.Tokens.First(x => x?.TokenType == type);
                //_database.Tokens.Remove(oldToken);
                user.Tokens.Remove(oldToken);
                await _database.SaveChangesAsync();
            }
        }
    }
}