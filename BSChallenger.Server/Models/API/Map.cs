using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

		public Map(string hash, string chari, string diff, List<string> features)
		{
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
		public string Hash { get; set; }
		public string Characteristic { get; set; }
		public string Difficulty { get; set; }
		public List<string> Features { get; set; } = new();
	}
}