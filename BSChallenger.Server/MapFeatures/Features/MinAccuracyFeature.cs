using BSChallenger.Server.Models;

namespace BSChallenger.Server.MapFeatures.Features
{
	public class MinAccuracyFeature : IMapFeature
	{
		public string GetName() => "min_acc";

		public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
		{
			if(float.TryParse(featureData, out float accvalue))
			{
				bool passed = score.Accuracy > accvalue;
				return passed ? MapFeatureResult.Pass : MapFeatureResult.Fail;
			}
			return MapFeatureResult.Error;
		}
	}
}
