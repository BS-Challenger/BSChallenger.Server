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
		private readonly ILogger _logger = Log.ForContext<AuthenticateController>();
		private readonly BeatLeaderApiProvider _beatleaderAPI;
		private readonly JWTProvider _jwtProvider;
		private readonly Database _database;

		public AuthenticateController(
			BeatLeaderApiProvider beatleaderAPI,
			JWTProvider jwtProvider,
			Database database)
		{
			_beatleaderAPI = beatleaderAPI;
			_jwtProvider = jwtProvider;
			_database = database;
		}

		[HttpGet("/identity")]
		[EnableCors(PolicyName = "website")]
		public async Task<ActionResult<IdentityResponse>> IdentityAsync()
		{
			var Identities = HttpContext.User.Identities;
			var IdIdentity = Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type.Contains("nameidentifier"));
			if (IdIdentity != null)
			{
				var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == IdIdentity.Value);
				return Ok(new IdentityResponse(IdIdentity.Value, user?.Username, user?.Avatar));
			}
			return NotFound();
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
			var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == identity);
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