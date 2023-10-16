using Azure.Core;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("[controller]")]
	public class DiscordAuthController : ControllerBase
	{
		private readonly UserProvider _userProvider;
		private readonly BeatLeaderApiProvider _beatleaderAPI;
		private readonly JWTProvider _jwtProvider;

		public DiscordAuthController(UserProvider userProvider, BeatLeaderApiProvider beatleaderAPI, JWTProvider jwtProvider)
		{
			_userProvider = userProvider;
			_beatleaderAPI = beatleaderAPI;
			_jwtProvider = jwtProvider;
		}

		[HttpGet("/discord-auth")]
		public async Task<ActionResult> RedirectDiscord([FromQuery(Name = "code")] string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return Redirect("https://discord.com/api/oauth2/authorize?client_id=1163555058479272006&redirect_uri=http%3A%2F%2Flocalhost%3A8081%2Fdiscord-auth&response_type=code&scope=identify");
			}

			HttpContext.Response.Cookies.Append("discordCode", code, new CookieOptions
			{
				MaxAge = TimeSpan.FromSeconds(180), //give user time to login to beatleader
				Secure = true,
				IsEssential = true,
			});

			return Redirect("");
		}
	}
}
