using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
			//ToList because the expression was too silly for SQL
			var refreshToken = _database.Tokens.AsEnumerable().FirstOrDefault(x => x.token == request.RefreshToken);

			if (refreshToken != null && refreshToken.tokenType == TokenType.RefreshToken)
			{
				var user = _database.Users.FirstOrDefault(x => x.Id == refreshToken.UserId);
				var token = await _tokenProvider.GetAccessToken(user);
				var newRefrshtoken = await _tokenProvider.GetRefreshToken(user);
				return new AccessTokenResponse(token.token, newRefrshtoken.token);
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
			return new AuthResponse("Success", true, (TokenView)await _tokenProvider.GetRefreshToken(user));
		}

		[HttpPost("Login")]
		public async Task<ActionResult<AuthResponse>> PostLogin(NamePasswordRequest request)
		{
			if (_database.Users.Any(x => x.Username == request.Username))
			{
				var user = _database.Users.First(x => x.Username == request.Username);
				if (_passwordProvider.Verify(request.Password, user))
				{
					//Refresh tokens will last for 1 month
					return new AuthResponse("Success", true, await _tokenProvider.GetRefreshToken(user));
				}
			}
			return new AuthResponse("Username or Password is Incorrect", false, null);
		}
		[HttpPost("Identity")]
		public ActionResult<IdentityResponse> PostIdentity(AuthenticatedRequest request)
		{
			var token = _database.Tokens.FirstOrDefault(x => x.token == request.AccessToken && x.tokenType == TokenType.AccessToken);

			if (token != null)
			{
				var user = _database.Users.FirstOrDefault(x => x.Id == token.UserId);
				if (user != null)
				{
					return new IdentityResponse(user.Id, user.Username);
				}
			}
			return new IdentityResponse(-1, "Identity Request Failed");
		}
	}
}