using BSChallenger.Server.Models.API.Users;

namespace BSChallenger.Server.Models.API.Rankings
{
    public class RankTeamMember
    {
        public int RankingId { get; set; }
        public Ranking Ranking { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
