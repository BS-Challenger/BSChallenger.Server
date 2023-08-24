using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.Server.Models
{
    public class BeatleaderAPI
    {
        private const string BeatleaderEndpoint = "https://api.beatleader.xyz/";

        private readonly ILogger _logger = Log.ForContext<BeatleaderAPI>();
        private readonly HttpClient _httpClient = new HttpClient();
        private SecretProvider _secrets;

        public BeatleaderAPI(SecretProvider secrets)
        {
            _secrets = secrets;
        }

        public async Task<int> GetBLUserIdentity(string code)
        {
            var content = new StringContent("grant_type=authorization_code&client_id=BSChallengerClientID&client_secret=" + _secrets.Secrets.BLclientSecret + "&code=" + code + "&redirect_uri=http://localhost:8080/beatleader-callback", Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(BeatleaderEndpoint + "oauth2/token", content);
            _logger.Information(res.Content.ToString());

            return 0;
        }

        public async Task<List<BeatLeaderScore>> GetSinceDateScores(string userId, DateTime date)
        {
            TimeSpan t = date - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            int AmountLeft = 100000000;
            int page = 1;
            List<BeatLeaderScore> scores = new();
            while (AmountLeft > 0)
            {
                var res = await _httpClient.GetAsync(BeatleaderEndpoint + string.Format("player/{0}/scores?sortBy=date&page={1}&count=300", userId, page, secondsSinceEpoch));
                var obj = JsonConvert.DeserializeObject<Root>(await res.Content.ReadAsStringAsync());
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
    }
    public class BeatLeaderScore
    {
        public Leaderboard Leaderboard { get; set; }
        public double AccLeft { get; set; }
        public double AccRight { get; set; }
        public int Id { get; set; }
        public int BaseScore { get; set; }
        public int ModifiedScore { get; set; }
        public double Accuracy { get; set; }
        public string PlayerId { get; set; }
        public int Rank { get; set; }
        public string Country { get; set; }
        public double FcAccuracy { get; set; }
        public string Modifiers { get; set; }
        public int BadCuts { get; set; }
        public int MissedNotes { get; set; }
        public int BombCuts { get; set; }
        public int WallsHit { get; set; }
        public int Pauses { get; set; }
        public bool FullCombo { get; set; }
        public string Platform { get; set; }
        public int MaxCombo { get; set; }
        public int? MaxStreak { get; set; }
        public int Hmd { get; set; }
        public int Controller { get; set; }
        public string LeaderboardId { get; set; }
        public string Timeset { get; set; }
        public int PlayCount { get; set; }
    }

    public class Difficulty
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int Mode { get; set; }
        public string DifficultyName { get; set; }
        public string ModeName { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public float Njs { get; set; }
        public float Nps { get; set; }
        public int Notes { get; set; }
        public int Bombs { get; set; }
        public int Walls { get; set; }
        public int MaxScore { get; set; }
        public int Duration { get; set; }
        public int Requirements { get; set; }
    }

    public class Leaderboard
    {
        public string Id { get; set; }
        public Song Song { get; set; }
        public Difficulty Difficulty { get; set; }
    }

    public class Metadata
    {
        public int ItemsPerPage { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
    }

    public class Root
    {
        public Metadata Metadata { get; set; }
        public List<BeatLeaderScore> Data { get; set; }
    }

    public class Song
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public string Author { get; set; }
        public string Mapper { get; set; }
        public int MapperId { get; set; }
        public string CoverImage { get; set; }
        public string FullCoverImage { get; set; }
        public string DownloadUrl { get; set; }
        public double Bpm { get; set; }
        public int Duration { get; set; }
        public string Tags { get; set; }
        public int UploadTime { get; set; }
        public List<Difficulty> Difficulties { get; set; }
    }

}
