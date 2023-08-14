using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Scan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		private readonly ILogger _logger = Log.ForContext<ScanController>();
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
            var token = _database.Tokens
                .Include(x=>x.User)
                .FirstOrDefault(x => x.TokenValue == request.AccessToken && x.TokenType == TokenType.AccessToken);
			if (token != null && token.ExpiryTime > DateTime.UtcNow)
            {
				var user = token.User;
                if (user != null)
                {
					var scores = await _beatleaderAPI.GetSinceDateScores(user.BeatLeaderId, user.LastCheckDate);
                    var ranking = _database.EagerLoadRankings(true).Find(x => x.Name == request.Ranking);
					_logger.Information(scores.Count().ToString());
					Level latestLevelPassed = null;
                    foreach (var level in ranking.Levels.OrderBy(x => x.LevelNumber))
                    {
                        var validScores = scores.Where(x => {
                            bool ret = level.AvailableForPass.Any(y => {
                                return string.Equals(x.Leaderboard.Song.Hash, y.Hash, StringComparison.OrdinalIgnoreCase) && x.Leaderboard.Difficulty.DifficultyName == y.Difficulty && x.Leaderboard.Difficulty.ModeName.Replace("-PinkPlay_Controllable", "") == y.Characteristic;
                            });
                            return ret;
                        });
                        if (validScores != null && validScores.Count() > 0)
                        {
                            //Check for map features here
                            latestLevelPassed = level;
                        }
                        else
                        {
                            //Did not pass
                            break;
                        }
                    }
                    test.Stop();

                    return new ScanResponse("Scan Request Passed");
                }
            }
            return new ScanResponse("Scan Request Failed");
        }
    }
}
