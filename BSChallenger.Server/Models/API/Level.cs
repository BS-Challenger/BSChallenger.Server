using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

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
		}
		public Level()
		{
		}
		[Key, AutoIncrement]
		public int Id { get; set; }
		public int RankingId { get; set; }
		public int LevelNumber { get; set; }
		public int MapsReqForPass { get; set; }
		public string IconURL { get; set; }
	}
}
