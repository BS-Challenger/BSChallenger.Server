﻿using BSChallenger.Server.Providers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API.Rankings
{
    [PrimaryKey("Id")]
    public class Ranking
    {
        public Ranking(ulong guild, string name, string desc, string iconURL)
        {
            GuildId = guild;
            Name = name;
            Description = desc;
            IconURL = iconURL;
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
        public ICollection<Level> Levels { get; set; } = new List<Level>();
        public ICollection<RankTeamMember> RankTeamMembers { get; set; } = new List<RankTeamMember>();
        public ICollection<ScanHistory> History { get; set; } = new List<ScanHistory>();
    }
}