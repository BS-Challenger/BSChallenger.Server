using BSChallenger.Server.Models.BeatLeader.Scores;
using System;
using System.Linq;

namespace BSChallenger.Server.MapFeatures.Features
{
	public class RequiredModifiersFeature : IMapFeature
	{
		public string GetName() => "req_modifiers";
		public string GetDesc() => "Must play with these modifiers (more modifiers allowed).";
		public string GetExample() => "DA,FS,SS,SF,GN,NA,NB,NF,NO,PM,SC";

		public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
		{
			try
			{
				string[] requiredModifiers = featureData.Trim().Split(",");
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
