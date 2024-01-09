using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Cors;
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
		private readonly BeatLeaderApiProvider _beatleaderAPI;
		private readonly JwtProvider _jwtProvider;
		private readonly Database _database;
		private readonly AuthBuilder _authBuilder;

		public AuthenticateController(
			BeatLeaderApiProvider beatleaderAPI,
			JwtProvider jwtProvider,
			Database database,
			AuthBuilder authBuilder)
		{
			_beatleaderAPI = beatleaderAPI;
			_jwtProvider = jwtProvider;
			_database = database;
			_authBuilder = authBuilder;
		}

		[HttpGet("/identity")]
		[EnableCors(PolicyName = "website")]
		public ActionResult<User> IdentityAsync()
		{
			User user = null;
			_authBuilder.WithHTTPUser(HttpContext, (_user) => user = _user);
			return user != null ? Ok(user) : NotFound();
		}

		[HttpPost("/login")]
		[EnableCors(PolicyName = "website")]
		public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request)
		{
			var identity = await _beatleaderAPI.GetUserIdentityAsync(request.BeatLeaderToken, request.RedirectURL);
			if (identity == "-1")
			{
				return BadRequest(new BadRequestObjectResult("Beatleader token is stale! Please re-authenticate"));
			}
			var user = _authBuilder.WithUser(identity);
			if (user == null)
			{
				user = new User
				{
					BeatLeaderId = identity
				};
				var playerInfo = await _beatleaderAPI.GetPlayerInfoAsync(identity);
				//Migrate existing socials
				if (playerInfo.Socials != null)
				{
					var discord = playerInfo.Socials.Find(x => x.Service == "Discord");
					if (discord != null)
					{
						user.DiscordId = discord.UserId;
					}
				}

				user.Username = playerInfo.Name;
				user.Avatar = playerInfo.Avatar;
				user.Platform = playerInfo.Platform;
				user.Country = playerInfo.Country;

				await _database.Users.AddAsync(user);
				await _database.SaveChangesAsync();
			}

			return Ok(new LoginResponse(_jwtProvider.GenerateJWT(user.BeatLeaderId)));
		}
	}
}