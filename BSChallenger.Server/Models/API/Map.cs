using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
    [PrimaryKey("Id")]
    public class Map
    {
        public Map(string hash, string chari, string diff)
        {
            Hash = hash;
            Characteristic = chari;
            Difficulty = diff;
        }

        public Map(string hash, string chari, string diff, List<MapFeature> features)
        {
            Hash = hash;
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
		public string Identifier => IDGenerator.GenerateID(IDType.Map, Id);
		public string Hash { get; set; }
        public string Characteristic { get; set; }
        public string Difficulty { get; set; }
        public virtual List<MapFeature> Features { get; set; } = new();

		[JsonIgnore]
		public int LevelId { get; set; }
		[JsonIgnore]
		public Level Level { get; set; }
	}
}