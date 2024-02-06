using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API.Rankings
{
    [PrimaryKey("Id")]
    public class Ranking
    {
        public Ranking(ulong guild, string name, string desc, string iconURL, string discordURL)
        {
            GuildId = guild;
            Name = name;
            Description = desc;
            IconURL = iconURL;
            DiscordURL = discordURL;
		}
        public Ranking()
        {
        }

        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.Ranking, Id);
        public ulong GuildId { get; set; }
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconURL { get; set; }
        public bool Private { get; set; }
        public bool Partnered { get; set; }
        public string DiscordURL { get; set; }
		public List<Level> Levels { get; set; } = new();
        public List<RankTeamMember> RankTeamMembers { get; set; } = new();

        //Will be updated daily by quartz job
        public int ActiveUsers { get; set; }
        public int WeeklyScans { get; set; }
	}
}