using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API.Users
{
    [PrimaryKey("Id")]
    public class User
    {
        public User(string username)
        {
            Username = username;
        }
        public User()
        {
        }

        //ID
        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.User, Id);

        //Player Info
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Country { get; set; }
        public string Platform { get; set; }

		//Links
		public string BeatLeaderId { get; set; }
        public string DiscordId { get; set; }

        //Extras
        public DateTime LastScanDate { get; set; }
        public List<RankTeamMember> AssignedRankings { get; set; } = new();
    }
}