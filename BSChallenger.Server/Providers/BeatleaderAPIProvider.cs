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

		public async Task<string> GetUserIdentityAsync(string code, string iss)
		{
			var token = await GetBLOauthTokenAsync(code, iss);
			_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
			var result = await _httpClient.GetAsync(BeatleaderEndpoint + "oauth2/identity");
			if (!result.IsSuccessStatusCode)
			{
				_logger.Error(result.ReasonPhrase);
			}
			_httpClient.DefaultRequestHeaders.Remove("Authorization");

			var content = await result.Content.ReadAsStringAsync();
			var obj = JsonConvert.DeserializeObject<BLIdentityResponse>(content);
			return obj == null ? "-1" : obj.id;
		}

		private async Task<string> GetBLOauthTokenAsync(string code, string iss)
		{
			var reqContent = new StringContent("grant_type=authorization_code&client_id=BSChallengerClientID&client_secret=" + _secrets.Secrets.BLclientSecret + "&code=" + code + "&redirect_uri=" + iss, Encoding.UTF8, "application/x-www-form-urlencoded");
			var res = await _httpClient.PostAsync(BeatleaderEndpoint + "oauth2/token", reqContent);
			if(!res.IsSuccessStatusCode)
			{
				_logger.Error(res.ReasonPhrase);
			}
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
			DateTime StartTime = DateTime.Now;
			while (AmountLeft > 0)
			{
				var res = await _httpClient.GetAsync(BeatleaderEndpoint + $"player/{userId}/scores?sortBy=date&page={page}&count={Math.Min(AmountLeft, 100)}");
				if (!res.IsSuccessStatusCode)
				{
					_logger.Error(res.ReasonPhrase);
				}
				var content = await res.Content.ReadAsStringAsync();
				if(content.Contains("API calls quota exceeded!"))
				{
					await Task.Delay(10250 - Math.Abs(StartTime.Subtract(DateTime.Now).Milliseconds));
					continue;
				}
				var obj = JsonConvert.DeserializeObject<Root>(content);
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
