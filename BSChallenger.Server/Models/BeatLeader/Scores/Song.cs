using System.Collections.Generic;

namespace BSChallenger.Server.Models.BeatLeader.Scores
{
	public class Song
	{
		public string Id { get; set; }
		public string Hash { get; set; }
		public string Name { get; set; }
		public string SubName { get; set; }
		public string Author { get; set; }
		public string Mapper { get; set; }
		public int MapperId { get; set; }
		public string CoverImage { get; set; }
		public string FullCoverImage { get; set; }
		public string DownloadUrl { get; set; }
		public double Bpm { get; set; }
		public int Duration { get; set; }
		public string Tags { get; set; }
		public int UploadTime { get; set; }
		public List<Difficulty> Difficulties { get; set; }
	}
}
