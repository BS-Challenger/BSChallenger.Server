using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Scan;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("[controller]")]
	public class AuthenticateController : ControllerBase
	{
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

		[HttpPost("/login")]
		[RequireHttps]
		public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request)
		{
			var identity = await _beatleaderAPI.GetUserIdentityAsync(request.BeatLeaderToken);
			var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == identity);
			if (user != null)
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

			return Ok(_jwtProvider.GenerateJWT(user.BeatLeaderId));
		}
	}
}