using BSChallenger.Server.Models;

namespace BSChallenger.Server.MapFeatures.Features
{
	public class MaxMissesFeature : IMapFeature
	{
		public string GetName() => "max_misses";

		public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
		{
			if (int.TryParse(featureData, out int misses))
			{
				bool passed = score.MissedNotes <= misses;
				return passed ? MapFeatureResult.Pass : MapFeatureResult.Fail;
			}
			return MapFeatureResult.Error;
		}
	}
}
