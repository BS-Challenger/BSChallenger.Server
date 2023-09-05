namespace BSChallenger.Server.Models.BeatLeader.Scores
{
	public class Difficulty
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public int Mode { get; set; }
		public string DifficultyName { get; set; }
		public string ModeName { get; set; }
		public int Status { get; set; }
		public int Type { get; set; }
		public float Njs { get; set; }
		public float Nps { get; set; }
		public int Notes { get; set; }
		public int Bombs { get; set; }
		public int Walls { get; set; }
		public int MaxScore { get; set; }
		public int Duration { get; set; }
		public int Requirements { get; set; }
	}
}
