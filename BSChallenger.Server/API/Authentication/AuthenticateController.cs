using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Scan;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Serilog;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("[controller]")]
	public class AuthenticateController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<ScanController>();
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
		[Authorize]
		public async Task<ActionResult<IdentityResponse>> IdentityAsync()
		{
			var thingy = HttpContext.User.Identities;
			thingy.ToList().ForEach(x => {
				Console.WriteLine(x.Name);
				x.Claims.ToList().ForEach(x=>Console.WriteLine(" - " + x.Type + ": " + x.Value));
				});
			return new IdentityResponse(5, "hi");
		}

		[HttpPost("/login")]
		public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request)
		{
			_logger.Information(request.BeatLeaderToken);
			var identity = await _beatleaderAPI.GetUserIdentityAsync(request.BeatLeaderToken);
			_logger.Information(identity.ToString());
			var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == identity);
			_logger.Information((user == null).ToString());
			if (user == null)
			{
				user = new User();
				user.BeatLeaderId = identity;
				var playerInfo = await _beatleaderAPI.GetPlayerInfoAsync(identity);
				//Migrate existing socials
				if(playerInfo.Socials != null)
				{
					var discord = playerInfo.Socials.FirstOrDefault(x => x.Service == "Discord");
					if(discord != null)
					{
						user.DiscordId = discord.Id;
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