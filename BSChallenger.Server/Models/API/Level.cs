using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Level
	{
		public Level(Ranking ranking, int level, int reqMaps, string iconURL)
		{
			RankingId = ranking.Id;
			LevelNumber = level;
			MapsReqForPass = reqMaps;
			IconURL = iconURL;
			Id = Guid.NewGuid();
		}
		public Level()
		{
			Id = Guid.NewGuid();
		}
		[Key]
		[IgnoreDataMember]
		public Guid Id { get; set; }
		public Guid RankingId { get; set; }
		public int LevelNumber { get; set; }
		public int MapsReqForPass { get; set; }
		public string IconURL { get; set; }
	}
}
