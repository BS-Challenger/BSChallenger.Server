﻿using BSChallenger.Server.Models.API.Maps;
using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Providers;
using Sqids;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
    [PrimaryKey("Id")]
    public class Level
    {
        public Level(int level, int reqMaps, string iconURL, string color)
        {
            LevelNumber = level;
            MapsReqForPass = reqMaps;
            IconURL = iconURL;
            Color = color;
        }
        public Level()
        {

        }
        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.Level, Id);
        public int LevelNumber { get; set; }
        public int MapsReqForPass { get; set; }
        public string IconURL { get; set; }
        public string Color { get; set; }
        public string RequiredCategoryData { get; set; }
        public List<Map> AvailableForPass { get; set; } = new List<Map>();

        [JsonIgnore]
        public int RankingId { get; set; }
        [JsonIgnore]
        public Ranking Ranking { get; set; }
    }
}