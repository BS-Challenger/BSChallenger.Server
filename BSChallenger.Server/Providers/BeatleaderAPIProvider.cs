using BSChallenger.Server.Models.BeatLeader.Authentication;
using BSChallenger.Server.Models.BeatLeader.Player;
using BSChallenger.Server.Models.BeatLeader.Scores;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.Server.Providers
{
	public class BeatLeaderApiProvider
	{
		private const string BeatleaderEndpoint = "https://api.beatleader.xyz/";

		private readonly ILogger _logger = Log.ForContext<BeatLeaderApiProvider>();
		private readonly HttpClient _httpClient = new HttpClient();
		private SecretProvider _secrets;

		public BeatLeaderApiProvider(SecretProvider secrets)
		{
			_secrets = secrets;
		}

		public async Task<string> GetUserIdentityAsync(string code)
		{
			var token = await GetBLOauthTokenAsync(code);
			_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
			var result = await _httpClient.GetAsync(BeatleaderEndpoint + "oauth2/identity");
			_httpClient.DefaultRequestHeaders.Remove("Authorization");

			var content = await result.Content.ReadAsStringAsync();
			var obj = JsonConvert.DeserializeObject<BLIdentityResponse>(content);
			return obj.id;
		}

		private async Task<string> GetBLOauthTokenAsync(string code)
		{
			var reqContent = new StringContent("grant_type=authorization_code&client_id=BSChallengerClientID&client_secret=" + _secrets.Secrets.BLclientSecret + "&code=" + code + "&redirect_uri=http%3A%2F%2Flocalhost%3A8080%2Fbeatleader-callback", Encoding.UTF8, "application/x-www-form-urlencoded");
			var res = await _httpClient.PostAsync(BeatleaderEndpoint + "oauth2/token", reqContent);
			var content = await res.Content.ReadAsStringAsync();
			var obj = JsonConvert.DeserializeObject<BLOauthTokenResponse>(content);
			return obj.access_token;
		}

		public async Task<List<BeatLeaderScore>> GetScoresAsync(string userId, DateTime date)
		{
			TimeSpan t = date - new DateTime(1970, 1, 1);
			int secondsSinceEpoch = (int)t.TotalSeconds;
			int AmountLeft = 100000000;
			int page = 1;
			List<BeatLeaderScore> scores = new();
			while (AmountLeft > 0)
			{
				var res = await _httpClient.GetAsync(BeatleaderEndpoint + $"player/{userId}/scores?sortBy=date&page={page}&count=300");
				var obj = JsonConvert.DeserializeObject<Root>(await res.Content.ReadAsStringAsync());
				//If this is first iteration then set the real total
				if (AmountLeft == 100000000)
				{
					AmountLeft = obj.Metadata.Total;
				}
				AmountLeft -= 100;
				scores.AddRange(obj.Data);
				page++;
			}
			return scores;
		}

		public async Task<PlayerInfo> GetPlayerInfoAsync(string BeatLeaderId)
		{
			var res = await _httpClient.GetAsync(BeatleaderEndpoint + $"player/{BeatLeaderId}?stats=true&keepOriginalId=false");
			return JsonConvert.DeserializeObject<PlayerInfo>(await res.Content.ReadAsStringAsync());
		}
	}
}
