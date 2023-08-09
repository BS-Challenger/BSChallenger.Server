using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Map
	{
		public Map(Level level, string hash, string chari, string diff)
		{
			LevelId = level.Id;
			Hash = hash;
			Characteristic = chari;
			Difficulty = diff;
		}

		public Map(Level level, string hash, string chari, string diff, List<string> features)
		{
			LevelId = level.Id;
			Hash = hash;
			Characteristic = chari;
			Difficulty = diff;
			Features = features;
		}

		public Map()
		{
		}

		[Key]
		public int Id { get; set; }
		public int LevelId { get; set; }
		public string Hash { get; set; }
		public string Characteristic { get; set; }
		public string Difficulty { get; set; }
		public List<string> Features { get; set; }
	}
}