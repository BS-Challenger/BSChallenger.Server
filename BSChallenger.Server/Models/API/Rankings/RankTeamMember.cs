using BSChallenger.Server.Models.API.Users;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BSChallenger.Server.Models.API.Rankings
{
	[PrimaryKey("Id")]
	public class RankTeamMember
    {
        public int Id { get; set; }
        public int RankingId { get; set; }
		[JsonIgnore]
		public Ranking Ranking { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public RankTeamRole Role { get; set; }
	}

    public enum RankTeamRole
    {
		None,
		RankTeam,
		Owner,
    }
}
