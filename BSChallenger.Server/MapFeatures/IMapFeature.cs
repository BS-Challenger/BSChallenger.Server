using BSChallenger.Server.Models;

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
