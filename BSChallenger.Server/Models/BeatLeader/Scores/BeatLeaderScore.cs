namespace BSChallenger.Server.Models.BeatLeader.Scores
{
	public class BeatLeaderScore
	{
		public Leaderboard Leaderboard { get; set; }
		public double AccLeft { get; set; }
		public double AccRight { get; set; }
		public int Id { get; set; }
		public int BaseScore { get; set; }
		public int ModifiedScore { get; set; }
		public double Accuracy { get; set; }
		public string PlayerId { get; set; }
		public int Rank { get; set; }
		public string Country { get; set; }
		public double FcAccuracy { get; set; }
		public string Modifiers { get; set; }
		public int BadCuts { get; set; }
		public int MissedNotes { get; set; }
		public int BombCuts { get; set; }
		public int WallsHit { get; set; }
		public int Pauses { get; set; }
		public bool FullCombo { get; set; }
		public string Platform { get; set; }
		public int MaxCombo { get; set; }
		public int? MaxStreak { get; set; }
		public int Hmd { get; set; }
		public int Controller { get; set; }
		public string LeaderboardId { get; set; }
		public string Timeset { get; set; }
		public int PlayCount { get; set; }
	}
}
