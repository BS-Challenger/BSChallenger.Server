using BSChallenger.Server.Models.BeatLeader.Scores;
using System;
using System.Linq;

namespace BSChallenger.Server.MapFeatures.Features
{
	public class CategoryFeature : IMapFeature
	{
		public string GetName() => "category";
		public string GetDesc() => "Labels the subcategory of the map";
		public string GetExample() => "speed";

		public MapFeatureResult GetValid(BeatLeaderScore score, string featureData)
		{
			return MapFeatureResult.Pass;
		}
	}
}
