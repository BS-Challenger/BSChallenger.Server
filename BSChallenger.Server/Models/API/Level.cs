using SQLite;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Level
	{
		public Level(Ranking ranking, int level, int reqMaps, string iconURL, string color)
		{
			RankingId = ranking.Id;
			LevelNumber = level;
			MapsReqForPass = reqMaps;
			IconURL = iconURL;
			Color = color;
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
		public string Color { get; set; }
	}
}