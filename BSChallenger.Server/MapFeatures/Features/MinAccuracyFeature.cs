using BSChallenger.Server.Models.BeatLeader.Scores;

namespace BSChallenger.Server.MapFeatures.Features
{
    public class MinAccuracyFeature : IMapFeature
    {
        public string GetName() => "min_acc";
		public string GetDesc() => "Player must pass the map with at least this much accuracy.";
		public string GetExample() => "98.2%";

		public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
        {
            string trimmedData = featureData.Replace("%", "");
            if (float.TryParse(featureData, out float accvalue))
            {
                bool passed = score.Accuracy > accvalue;
                return passed ? MapFeatureResult.Pass : MapFeatureResult.Fail;
            }
            return MapFeatureResult.Error;
        }
    }
}
