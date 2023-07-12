using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Level
	{
		public Level(int level, int reqMaps, string iconURL)
		{
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
		public Guid Id { get; set; }
		public int LevelNumber { get; set; }
		public int MapsReqForPass { get; set; }
		public string IconURL { get; set; }
		public DbSet<Map> AvailableForPass { get; set; }
	}
}
