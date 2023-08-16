using BSChallenger.Server.Models;

namespace BSChallenger.Server.MapFeatures
{
	public interface IMapFeature
	{
		public string GetName();
		public bool GetValid(Datum score, string feature);
	}
}
