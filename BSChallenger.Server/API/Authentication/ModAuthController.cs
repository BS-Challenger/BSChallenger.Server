using Azure.Core;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("[controller]")]
	public class ModAuthController : ControllerBase
	{
		private readonly byte[] _authPage = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "HTML/ModAuth.html"));
		private readonly UserProvider _userProvider;
		private readonly BeatLeaderApiProvider _beatleaderAPI;
		private readonly JwtProvider _jwtProvider;

		public ModAuthController(UserProvider userProvider, BeatLeaderApiProvider beatleaderAPI, JwtProvider jwtProvider)
		{
			_userProvider = userProvider;
			_beatleaderAPI = beatleaderAPI;
			_jwtProvider = jwtProvider;
		}

		[HttpGet("/mod-socket")]
		public FileContentResult Index()
		{
			return File(_authPage, "text/html");
		}

		[HttpGet("/mod-auth")]
		public async Task<ActionResult> RedirectBeatleader([FromQuery(Name = "code")] string code, [FromQuery(Name = "iss")] string iss)
		{
			if (string.IsNullOrEmpty(code))
			{
				return Redirect("https://api.beatleader.xyz/oauth2/authorize?client_id=BSChallengerClientID&response_type=code&redirect_uri=https://localhost:8081/mod-auth&scope=profile");
			}

			if (iss != "https://api.beatleader.xyz/")
			{
				return Forbid("Invalid Issuer");
			}

			var identity = await _beatleaderAPI.GetUserIdentityAsync(code, iss);
			if(identity == "-1")
			{
				return this.Unauthorized("Beatleader token is invalid");
			}
			var user = await _userProvider.GetOrCreateUser(identity);

			var jwt = _jwtProvider.GenerateJWT(user.BeatLeaderId);
			return Redirect($"mod-socket#{jwt}");
		}
	}
}
