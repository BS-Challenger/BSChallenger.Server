﻿using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Scan;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
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
		[EnableCors(PolicyName = "website")]
		public async Task<ActionResult<IdentityResponse>> IdentityAsync()
		{
			var x = HttpContext.Request.Headers.Authorization;
			Console.WriteLine(x);
			if (HttpContext.Request.Method == "OPTIONS") return null;
			var Identities = HttpContext.User.Identities;

			Console.WriteLine(Identities.Count());

			var IdIdentity = Identities.SelectMany(x => x.Claims).FirstOrDefault(x => {
				Console.WriteLine(x.Type);
				return x.Type.Contains("nameidentifier");
			});
			if(IdIdentity != null)
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
			var identity = await _beatleaderAPI.GetUserIdentityAsync(request.BeatLeaderToken);
			if(identity == "-1")
			{
				return BadRequest(new BadRequestObjectResult("Beatleader token is stale! Please re-authenticate"));
			}
			var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == identity);
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