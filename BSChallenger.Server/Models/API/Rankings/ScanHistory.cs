using BSChallenger.Server.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSChallenger.Server.Models.API.Rankings
{
    [PrimaryKey("Id")]
    public class ScanHistory
    {
        public ScanHistory(DateTime week)
        {
            Time = week;
        }

        public ScanHistory()
        {
        }

        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.ScanHistory, Id);
        public DateTime Time { get; set; }

        [JsonIgnore]
        public int RankingId { get; set; }
        [JsonIgnore]
        public Ranking Ranking { get; set; }
    }
}
