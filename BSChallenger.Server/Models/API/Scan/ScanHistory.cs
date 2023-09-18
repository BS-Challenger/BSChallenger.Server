using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSChallenger.Server.Models.API.Scan
{
    [PrimaryKey("Id")]
    public class ScanHistory
    {
        public ScanHistory(DateTime week, List<ScoreData> scores)
        {
            Time = week;
            Scores = scores;
        }

        public ScanHistory()
        {
        }

        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.ScanHistory, Id);
        public DateTime Time { get; set; }

        public List<ScoreData> Scores { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
