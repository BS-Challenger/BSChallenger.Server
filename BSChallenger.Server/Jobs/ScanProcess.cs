using BSChallenger.Server.MapFeatures;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Maps;
using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Models.API.Scan;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Models.BeatLeader.Scores;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Razor;
using Serilog.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Jobs
{
	public class ScanProcess
	{
		private readonly List<IMapFeature> _features = MapFeatureFactory.CreateInstancesFromCurrentAssembly();

		private List<BeatLeaderScore> CurrentScanningScores { get; set; }

		public void ScanUser(User user, Ranking ranking, List<BeatLeaderScore> scores)
		{
			CurrentScanningScores = scores;
			List<ScoreData> scanHistoryScores = new();
			Level latestLevelPassed = null;
			foreach (var level in ranking.Levels.OrderBy(x => x.LevelNumber))
			{
				var ScoresToCheck = ScanLevel(level);

				if(ScoresToCheck.Count > 0)
				{
					var anyPassed = true;
					foreach(var score in ScoresToCheck)
					{
						var passed = ScanMap(score.map, score.score, level);
						scanHistoryScores.Add(new ScoreData(score.map, passed, score.score.ModifiedScore, (float)score.score.Accuracy, score.score.Modifiers));
						anyPassed &= passed;
					}
					if(anyPassed)
					{
						latestLevelPassed = level;
					}
				}
				else
				{
					break;
				}
			}
			if(latestLevelPassed != null)
			{
				SetUserLevelForRanking(user, latestLevelPassed, ranking);
			}
			user.ScanHistory.Add(new ScanHistory(DateTime.UtcNow, scanHistoryScores));
			CurrentScanningScores = null;
		}

		private static void SetUserLevelForRanking(User user, Level level, Ranking ranking)
		{
			var existing = user.UserLevels.FirstOrDefault(x => x.RankingId == ranking.Identifier);
			if(existing == null)
			{
				user.UserLevels.Add(new UserLevel(level.LevelNumber, ranking.Identifier));
			}
			else
			{
				existing.Level = level.LevelNumber;
			}
		}

		//TODO: Check categories
		private bool ScanMap(Map map, BeatLeaderScore score, Level level)
		{
			var featuresToCheck = _features.Where(x => map.Features.Any(z => z.Type == x.GetName())).ToList();

			bool passing = true;
			featuresToCheck.ForEach(z => passing &= z.GetValid(score, map.Features.First(x => x.Type == z.GetName()).Data) == MapFeatureResult.Pass);


			return passing;
		}

		private List<ScorePair> ScanLevel(Level level)
		{
			List<ScorePair> ScoresToCheck = new();
			foreach (var score in CurrentScanningScores)
			{
				var map = level.AvailableForPass.FirstOrDefault(y => string.Equals(score.Leaderboard.Song.Hash, y.Hash, StringComparison.OrdinalIgnoreCase) && string.Equals(score.Leaderboard.Difficulty.DifficultyName, y.Difficulty, StringComparison.OrdinalIgnoreCase) && string.Equals(score.Leaderboard.Difficulty.ModeName.Replace("-PinkPlay_Controllable", ""), y.Characteristic, StringComparison.OrdinalIgnoreCase));
				if (map != null)
				{
					ScoresToCheck.Add(new ScorePair(map, score));
				}
			}

			return ScoresToCheck;
		}

		private struct ScorePair
		{
			internal readonly Map map;
			internal readonly BeatLeaderScore score;

			public ScorePair(Map map, BeatLeaderScore score)
			{
				this.map = map;
				this.score = score;
			}
		}
	}
}
