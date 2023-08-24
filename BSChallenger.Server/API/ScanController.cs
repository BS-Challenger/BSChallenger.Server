using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.MapFeatures;
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

        private List<IMapFeature> _features = MapFeatureFactory.CreateInstancesFromCurrentAssembly();

        public ScanController(
            Database database,
            BeatleaderAPI beatleaderAPI)
        {
            _database = database;
            _beatleaderAPI = beatleaderAPI;
        }

		//TODO: Split this up into seperate methods
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
                        //Amount of Linq nesting is insane
                        var validScores = scores.Where(x => {
                            return level.AvailableForPass.Any(y => {
                                bool isMatching = string.Equals(x.Leaderboard.Song.Hash, y.Hash, StringComparison.OrdinalIgnoreCase) && x.Leaderboard.Difficulty.DifficultyName == y.Difficulty && x.Leaderboard.Difficulty.ModeName.Replace("-PinkPlay_Controllable", "") == y.Characteristic;
                                var featuresToCheck = _features.Where(x=>y.Features.Any(z=>z.Type==x.GetName())).ToList();
                                //TODO: Error Checking
                                featuresToCheck.ForEach(z => { isMatching &= z.GetValid(x, y.Features.First(x => x.Type == z.GetName()).Data) == MapFeatureResult.Pass; });
								return isMatching;
							});
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
