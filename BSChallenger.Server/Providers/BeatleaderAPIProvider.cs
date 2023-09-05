using BSChallenger.Server.Models.BeatLeader.Player;
using BSChallenger.Server.Models.BeatLeader.Scores;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
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

        public async Task<int> GetUserIdentityAsync(string code)
        {
            var content = new StringContent("grant_type=authorization_code&client_id=BSChallengerClientID&client_secret=" + _secrets.Secrets.BLclientSecret + "&code=" + code + "&redirect_uri=http://localhost:8080/beatleader-callback", Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(BeatleaderEndpoint + "oauth2/token", content);
            if (int.TryParse(await res.Content.ReadAsStringAsync(), out var identity))
            {
                return identity;
			}
            return 0;
        }

        public async Task<List<BeatLeaderScore>> GetScoresAsync(int userId, DateTime date)
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

        public async Task<PlayerInfo> GetPlayerInfoAsync(int BeatLeaderId)
        {
            var res = await _httpClient.GetAsync(BeatleaderEndpoint + $"player/{BeatLeaderId}?stats=true&keepOriginalId=false");
            return JsonConvert.DeserializeObject<PlayerInfo>(await res.Content.ReadAsStringAsync());
        }
    }
}
