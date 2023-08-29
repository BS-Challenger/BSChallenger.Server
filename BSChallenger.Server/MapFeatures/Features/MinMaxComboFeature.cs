using BSChallenger.Server.Providers;

namespace BSChallenger.Server.MapFeatures.Features
{
    public class MinMaxComboFeature : IMapFeature
    {
        public string GetName() => "min_max_combo";

        public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
        {
            if (int.TryParse(featureData, out int maxCombo))
            {
                bool passed = score.MaxCombo <= maxCombo;
                return passed ? MapFeatureResult.Pass : MapFeatureResult.Fail;
            }
            return MapFeatureResult.Error;
        }
    }
}
