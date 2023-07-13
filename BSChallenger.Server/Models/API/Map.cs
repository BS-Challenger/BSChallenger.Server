using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
			Id = Guid.NewGuid();
		}
		public Map()
		{
			Id = Guid.NewGuid();
		}
		[Key]
		[IgnoreDataMember]
		public Guid Id { get; set; }
		public Guid LevelId { get; set; }
		public string Hash { get; set; }
		public string Characteristic { get; set; }
		public string Difficulty { get; set; }
	}
}
