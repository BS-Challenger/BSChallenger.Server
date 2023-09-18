using BSChallenger.Server.Models.API.Maps;
using BSChallenger.Server.Models.API.Scan;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSChallenger.Server.Models.API.Users
{
	[PrimaryKey("Id")]
	public class UserLevel
	{
		public UserLevel(int level, string ranking)
		{
			Level = level;
			RankingId = ranking;
		}

		public UserLevel()
		{
		}

		[Key, JsonIgnore]
		public int Id { get; set; }

		public string RankingId { get; set; }
		public int Level { get; set; }

		[JsonIgnore]
		public int UserId { get; set; }
		[JsonIgnore]
		public User User { get; set; }
	}
}
