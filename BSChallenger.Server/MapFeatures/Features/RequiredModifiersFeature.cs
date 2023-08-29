using BSChallenger.Server.Providers;
using System;
using System.Linq;

namespace BSChallenger.Server.MapFeatures.Features
{
    public class RequiredModifiersFeature : IMapFeature
    {
        public string GetName() => "req_modifiers";

        public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
        {
            try
            {
                string[] requiredModifiers = featureData.Split(",");
                string[] setScoreModifiers = score.Modifiers.Split(",");

                bool pass = true;

                foreach (var reqModifier in requiredModifiers)
                {
                    pass &= setScoreModifiers.Contains(reqModifier);
                }

                return pass ? MapFeatureResult.Pass : MapFeatureResult.Fail;
            }
            catch
            {
                return MapFeatureResult.Error;
            }
        }
    }
}
