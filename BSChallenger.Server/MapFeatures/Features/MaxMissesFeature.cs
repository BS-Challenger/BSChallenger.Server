using BSChallenger.Server.Models.BeatLeader.Scores;

namespace BSChallenger.Server.MapFeatures.Features
{
    public class MaxMissesFeature : IMapFeature
    {
        public string GetName() => "max_misses";
		public string GetDesc() => "Must get less than or equal to this many misses.";
		public string GetExample() => "23";

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
