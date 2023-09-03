using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly Database _database;
        private readonly TokenProvider _tokenProvider;
        private readonly PasswordProvider _passwordProvider;

        public AuthenticateController(
            Database database,
            TokenProvider tokenProvider,
            PasswordProvider passwordProvider)
        {
            _database = database;
            _tokenProvider = tokenProvider;
            _passwordProvider = passwordProvider;
        }

        [HttpPost("AccessToken")]
        public async Task<ActionResult<AccessTokenResponse>> PostGenerateAccessToken(AccessTokenRequest request)
        {
            var refreshToken = _database.Tokens.AsEnumerable().FirstOrDefault(x => x.TokenValue == request.RefreshToken);

            if (refreshToken != null && refreshToken.TokenType == TokenType.RefreshToken && refreshToken.ExpiryTime > DateTime.UtcNow)
            {
                var user = _database.Users.Find(refreshToken.UserId);
                var token = await _tokenProvider.GetAccessToken(user);
                var newRefrshtoken = await _tokenProvider.GetRefreshToken(user);
                return new AccessTokenResponse(token.TokenValue, newRefrshtoken.TokenValue);
            }
            return new AccessTokenResponse("Request Failed", "");
        }

        [HttpPost("Signup")]
        public async Task<ActionResult<AuthResponse>> PostSignup(NamePasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Username))
            {
                return new AuthResponse("Username Or Password is empty", false, null);
            }
            if (_database.Users.Any(x => x.Username == request.Username))
            {
                return new AuthResponse("Username Already Exists", false, null);
            }
            var user = new User(request.Username);

            user.PasswordHash = _passwordProvider.CreateHash(request.Password);

            await _database.Users.AddAsync(user);
            await _database.SaveChangesAsync();
            return new AuthResponse("Success", true, await _tokenProvider.GetRefreshToken(user));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> PostLogin(NamePasswordRequest request)
        {
            var user = _database.Users.FirstOrDefault(x => x.Username == request.Username);

            if (user != null && _passwordProvider.Verify(request.Password, user))
            {
                //Refresh tokens will last for 1 month
                return new AuthResponse("Success", true, await _tokenProvider.GetRefreshToken(user));
            }
            return new AuthResponse("Username or Password is Incorrect", false, null);
        }
        [HttpPost("Identity")]
        public ActionResult<IdentityResponse> PostIdentity(AuthenticatedRequest request)
        {
            var token = _database.Tokens.FirstOrDefault(x => x.TokenValue == request.AccessToken && x.TokenType == TokenType.AccessToken);

            if (token != null && token.ExpiryTime > DateTime.UtcNow)
            {
                var user = token.User;
                if (user != null)
                {
                    return new IdentityResponse(user.Id, user.Username);
                }
            }
            return new IdentityResponse(-1, "Identity Request Failed");
        }
    }
}