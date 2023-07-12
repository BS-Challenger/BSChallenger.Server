using BSChallenger.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<AuthenticateController>();
        private readonly Database _database;
        private readonly BeatleaderAPI _beatleaderAPI;

        public AuthenticateController(
            Database database,
            BeatleaderAPI beatleaderAPI)
        {
            _database = database;
            _beatleaderAPI = beatleaderAPI;
        }

        [HttpGet]
        public async Task Get(string code, string state)
        {
            if (code is null || state is null)
                throw new HttpResponseException(400);

            var validationResponse = await _beatleaderAPI.ValidateOAuth(code);

            if (validationResponse.error is not null)
            {
                throw new HttpResponseException(500);
            }

            Database.User user = await _database.Users.FindAsync(state);
            if (user is null)
            {
                user = new Database.User
                {
                    UserId = state
                };

                _database.Users.Add(user);
            }

            user.AccessToken = validationResponse.access_token;
            user.RefreshToken = validationResponse.refresh_token;
            user.TokenExpiry = DateTime.Now.AddSeconds(validationResponse.expires_in);

            await _database.SaveChangesAsync();
        }
    }
}
