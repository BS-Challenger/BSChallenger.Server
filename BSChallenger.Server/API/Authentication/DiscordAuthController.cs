using Azure.Core;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("[controller]")]
	public class DiscordAuthController : ControllerBase
	{
		private readonly Database _database;
		private readonly SecretProvider _secretProvider;
		private readonly HttpClient _client = new();

		public DiscordAuthController(Database database, SecretProvider secretProvider)
		{
			_database = database;
			_secretProvider = secretProvider;
		}

		[HttpPost("/discord-auth")]
		public async Task<ActionResult> RedirectDiscord(DiscordAuthRequest request	)
		{
			/*var nvc = new List<KeyValuePair<string, string>>();
			nvc.Add(new KeyValuePair<string, string>("client_id", "1163555058479272006"));
			nvc.Add(new KeyValuePair<string, string>("client_secret", "4szTTvYxPTtCVSRkFKHg4AmiNeNWTBwS"));
			nvc.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
			nvc.Add(new KeyValuePair<string, string>("redirect_uri", request.DiscordRedirectURL));
			nvc.Add(new KeyValuePair<string, string>("scope", "identify"));
			nvc.Add(new KeyValuePair<string, string>("code", request.DiscordOauthCode));
			var req = new HttpRequestMessage(HttpMethod.Post, "https://discord.com/api/v10/oauth2/token") { Content = new FormUrlEncodedContent(nvc) };
			var res = await _client.SendAsync(req);

			_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + res.Content.ReadAsStringAsync());
			var result = await _client.GetAsync(BeatleaderEndpoint + "oauth2/identity");
			if (!result.IsSuccessStatusCode)
			{
				_logger.Error(result.ReasonPhrase);
			}
			_client.DefaultRequestHeaders.Remove("Authorization");

			var nvc2 = new List<KeyValuePair<string, string>>();
			nvc.Add(new KeyValuePair<string, string>("client_id", "1163555058479272006"));
			nvc.Add(new KeyValuePair<string, string>("client_secret", "4szTTvYxPTtCVSRkFKHg4AmiNeNWTBwS"));
			nvc.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
			nvc.Add(new KeyValuePair<string, string>("redirect_uri", request.DiscordRedirectURL));
			nvc.Add(new KeyValuePair<string, string>("scope", "identify"));
			nvc.Add(new KeyValuePair<string, string>("code", request.DiscordOauthCode));
			var req2 = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/v10/oauth2/token") { Content = new FormUrlEncodedContent(nvc) };
			var res2 = await _client.SendAsync(req);*/


			return Ok();
			/*var Identities = HttpContext.User.Identities;
			var IdIdentity = Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type.Contains("nameidentifier"));
			if (IdIdentity != null)
			{
				var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == IdIdentity.Value);

				var nvc = new List<KeyValuePair<string, string>>();
				nvc.Add(new KeyValuePair<string, string>("client_id", "1163555058479272006"));
				nvc.Add(new KeyValuePair<string, string>("client_secret", "4szTTvYxPTtCVSRkFKHg4AmiNeNWTBwS"));
				nvc.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
				nvc.Add(new KeyValuePair<string, string>("redirect_uri", request.DiscordRedirectURL));
				nvc.Add(new KeyValuePair<string, string>("scope", "identify"));
				nvc.Add(new KeyValuePair<string, string>("code", request.DiscordOauthCode));
				var req = new HttpRequestMessage(HttpMethod.Post, "https://discord.com/api/v10/oauth2/token") { Content = new FormUrlEncodedContent(nvc) };
				var res = await _client.SendAsync(req);

				return Ok(new IdentityResponse(IdIdentity.Value, user?.Username, user?.Avatar));
			}
			return NotFound();*/
		}
	}
}
