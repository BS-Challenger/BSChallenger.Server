using BSChallenger.Server.MapFeatures;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Scan;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
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
        private readonly JWTProvider _jwtProvider;
        private readonly BeatLeaderApiProvider _beatleaderAPI;

		private readonly List<IMapFeature> _features = MapFeatureFactory.CreateInstancesFromCurrentAssembly();

        public ScanController(
            Database database,
			JWTProvider jwtProvider,
			BeatLeaderApiProvider beatleaderAPI)
        {
            _database = database;
            _jwtProvider = jwtProvider;
            _beatleaderAPI = beatleaderAPI;
        }

        //TODO: Split this up into seperate methods
        [HttpPost("/scan")]
        public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest request)
        {
            var token = _jwtProvider.GetUserToken(request.JWTToken);
            if (token != null && token.Exp > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                var user = _database.Users.FirstOrDefault(x=>x.BeatLeaderId==token.BeatLeaderId);
                if (user != null)
                {
                    var scores = await _beatleaderAPI.GetScoresAsync(user.BeatLeaderId, user.LastScanDate);
                    var ranking = _database.EagerLoadRankings().Find(x => x.Name == request.Ranking);
                    Level latestLevelPassed = null;
                    foreach (var level in ranking.Levels.OrderBy(x => x.LevelNumber))
                    {
                        //Amount of Linq nesting is insane
                        var validScores = scores.Where(x =>
                        {
                            return level.AvailableForPass.Any(y =>
                            {
                                bool isMatching = string.Equals(x.Leaderboard.Song.Hash, y.Hash, StringComparison.OrdinalIgnoreCase) && x.Leaderboard.Difficulty.DifficultyName == y.Difficulty && x.Leaderboard.Difficulty.ModeName.Replace("-PinkPlay_Controllable", "") == y.Characteristic;
                                var featuresToCheck = _features.Where(x => y.Features.Any(z => z.Type == x.GetName())).ToList();
                                //TODO: Error Checking
                                featuresToCheck.ForEach(z => isMatching &= z.GetValid(x, y.Features.First(x => x.Type == z.GetName()).Data) == MapFeatureResult.Pass);
                                return isMatching;
                            });
                        });
                        if (validScores != null && validScores.Any())
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

                    return new ScanResponse("Scan Request Passed");
                }
            }
            return new ScanResponse("Scan Request Failed");
        }
    }
}
