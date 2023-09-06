using BSChallenger.Server.Models.BeatLeader.Scores;

namespace BSChallenger.Server.MapFeatures.Features
{
    public class MinMaxComboFeature : IMapFeature
    {
        public string GetName() => "min_max_combo";
		public string GetDesc() => "Must get this combo at some point during the play.";
		public string GetExample() => "120";

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
