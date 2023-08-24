using BSChallenger.Server.Models;

namespace BSChallenger.Server.MapFeatures.Features
{
    public class MinScoreFeature : IMapFeature
    {
        public string GetName() => "min_score";

        public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
        {
            if (int.TryParse(featureData, out int scorevalue))
            {
                bool passed = score.ModifiedScore > scorevalue;
                return passed ? MapFeatureResult.Pass : MapFeatureResult.Fail;
            }
            return MapFeatureResult.Error;
        }
    }
}
