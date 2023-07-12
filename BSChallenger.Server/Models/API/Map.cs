using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

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
			Id = Guid.NewGuid();
		}
		public Map()
		{
			Id = Guid.NewGuid();
		}
		[Key]
		public Guid Id { get; set; }
		public string Hash { get; set; }
		public string Characteristic { get; set; }
		public string Difficulty { get; set; }
	}
}
