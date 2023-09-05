using System.Collections.Generic;

namespace BSChallenger.Server.Models.BeatLeader.Player
{
	public class PlayerInfo
	{
		public int MapperId { get; set; }
		public string ExternalProfileUrl { get; set; }
		public int LastWeekPp { get; set; }
		public int LastWeekRank { get; set; }
		public int LastWeekCountryRank { get; set; }
		public object EventsParticipating { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Platform { get; set; }
		public string Avatar { get; set; }
		public string Country { get; set; }
		public bool Bot { get; set; }
		public int Pp { get; set; }
		public int Rank { get; set; }
		public int CountryRank { get; set; }
		public string Role { get; set; }
		public List<Social> Socials { get; set; }
	}
}
