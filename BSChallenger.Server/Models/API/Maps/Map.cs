using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BSChallenger.Server.Providers;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API.Maps
{
    [PrimaryKey("Id")]
    public class Map
    {
        public Map(string id, string chari, string diff)
        {
			BeatSaverId = id;
            Characteristic = chari;
            Difficulty = diff;
        }

        public Map(string id, string chari, string diff, List<MapFeature> features)
        {
			BeatSaverId = id;
            Characteristic = chari;
            Difficulty = diff;
            Features = features;
        }

        public Map()
        {
        }

        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.Map, Id);
        public string BeatSaverId { get; set; }
        public string Characteristic { get; set; }
        public string Difficulty { get; set; }
        public virtual List<MapFeature> Features { get; set; } = new();

        [JsonIgnore]
        public int LevelId { get; set; }
        [JsonIgnore]
        public Level Level { get; set; }
    }
}