using System.Collections.Generic;

namespace BSChallenger.Server.Models.BeatLeader.Scores
{
	public class Root
	{
		public Metadata Metadata { get; set; }
		public List<BeatLeaderScore> Data { get; set; }
	}
}
