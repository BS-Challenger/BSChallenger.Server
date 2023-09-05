namespace BSChallenger.Server.Models.BeatLeader.Scores
{
	public class Leaderboard
	{
		public string Id { get; set; }
		public Song Song { get; set; }
		public Difficulty Difficulty { get; set; }
	}
}
