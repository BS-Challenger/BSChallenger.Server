using BSChallenger.Server.Models.BeatLeader.Scores;

namespace BSChallenger.Server.MapFeatures
{
	public interface IMapFeature
	{
		public string GetName();
		public string GetDesc();
		public string GetExample();
		public MapFeatureResult GetValid(BeatLeaderScore score, string featureData);
	}

	public enum MapFeatureResult
	{
		Pass,
		Fail,
		Error
	}
}