using SQLite;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Ranking
	{
		public Ranking(string name, string desc, string iconURL)
		{
			Name = name;
			Description = desc;
			IconURL = iconURL;
		}
		public Ranking()
		{
		}
		[Key, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string IconURL { get; set; }
	}
}
