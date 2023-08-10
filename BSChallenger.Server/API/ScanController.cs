using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Scan;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
    [ApiController]
    [Route("[controller]")]
    public class ScanController : ControllerBase
    {
		private readonly Database _database;
        private readonly BeatleaderAPI _beatleaderAPI;

        public ScanController(
            Database database,
            BeatleaderAPI beatleaderAPI)
        {
            _database = database;
            _beatleaderAPI = beatleaderAPI;
        }

        [HttpPost("/scan")]
        public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest request)
        {
            Stopwatch test = new();
            test.Start();
            var token = _database.Tokens.FirstOrDefault(x => x.TokenValue == request.AccessToken && x.TokenType == TokenType.AccessToken);

            if (token != null && token.ExpiryTime > DateTime.UtcNow)
            {
                var user = token.User;
                if (user != null)
                {
                    var scores = await _beatleaderAPI.GetSinceDateScores(user.BeatLeaderId, user.LastCheckDate);
                    var ranking = _database.Rankings.ToList().Find(x => x.Name == request.Ranking);
                    Level latestLevelPassed = null;
                    foreach (var level in ranking.Levels.OrderBy(x => x.LevelNumber))
                    {
                        var validScores = scores.Data.Where(x => level.AvailableForPass.Any(y => string.Equals(x.Leaderboard.Song.Hash, y.Hash, StringComparison.OrdinalIgnoreCase) && string.Equals(x.Leaderboard.Difficulty.DifficultyName, y.Difficulty, StringComparison.OrdinalIgnoreCase) && string.Equals(x.Leaderboard.Difficulty.ModeName.Replace("-PinkPlay_Controllable", "") == y.Characteristic, StringComparison.OrdinalIgnoreCase)));
                        if (validScores != null && validScores.Count() > 0)
                        {
                            //Check for map features here
                            latestLevelPassed = level;
                            Console.WriteLine("passed level " + level.LevelNumber);
                        }
                        else
                        {
                            Console.WriteLine("stopped at level " + level.LevelNumber);
                            //Did not pass
                            break;
                        }
                    }
                    Console.WriteLine("result level " + latestLevelPassed.LevelNumber);
                    test.Stop();
                    Console.WriteLine("scan request took " + test.Elapsed.Seconds.ToString() + "s to process");

                    return new ScanResponse("Scan Request Passed");
                }
            }
            return new ScanResponse("Scan Request Failed");
        }
    }
}
