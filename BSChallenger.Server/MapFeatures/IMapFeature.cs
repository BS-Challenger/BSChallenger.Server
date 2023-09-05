using BSChallenger.Server.Models.BeatLeader.Scores;
using BSChallenger.Server.Providers;

namespace BSChallenger.Server.MapFeatures
{
    public interface IMapFeature
    {
        public string GetName();
        public MapFeatureResult GetValid(BeatLeaderScore score, string featureData);
    }

    public enum MapFeatureResult
    {
        Pass,
        Fail,
        Error
    }
}
