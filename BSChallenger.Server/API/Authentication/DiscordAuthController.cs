﻿using Azure.Core;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.Discord;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
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
		public async Task<ActionResult> RedirectDiscord(DiscordAuthRequest request)
		{
			var Identities = HttpContext.User.Identities;
			var IdIdentity = Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type.Contains("nameidentifier"));
			if (IdIdentity != null)
			{
				var nvc = new List<KeyValuePair<string, string>>();
				nvc.Add(new KeyValuePair<string, string>("client_id", "1176563245755154532"));
				nvc.Add(new KeyValuePair<string, string>("client_secret", _secretProvider.Secrets.DiscordOauthSecret));
				nvc.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
				nvc.Add(new KeyValuePair<string, string>("redirect_uri", request.DiscordRedirectURL));
				nvc.Add(new KeyValuePair<string, string>("scope", "identify"));
				nvc.Add(new KeyValuePair<string, string>("code", request.DiscordOauthCode));
				var req = new HttpRequestMessage(HttpMethod.Post, "https://discord.com/api/v10/oauth2/token") { Content = new FormUrlEncodedContent(nvc) };
				var res = await _client.SendAsync(req);

				var token = JsonConvert.DeserializeObject<DiscordTokenResponse>(await res.Content.ReadAsStringAsync());

				_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
				var result = await _client.GetAsync("https://discord.com/api/users/@me");
				_client.DefaultRequestHeaders.Remove("Authorization");

				var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == IdIdentity.Value);

				var userInfo = JsonConvert.DeserializeObject<DiscordIdentityResponse>(await result.Content.ReadAsStringAsync());

				user.DiscordId = userInfo.Id;

				await _database.SaveChangesAsync();

				return Ok();
			}
			return Unauthorized();
		}
	}
}
