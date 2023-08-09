using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
	[PrimaryKey("Id")]
	public class Ranking
	{
		public Ranking(ulong guild, string name, string desc, string iconURL)
		{
			GuildId = guild;
			Name = name;
			Description = desc;
			IconURL = iconURL;
		}
		public Ranking()
		{
		}
		[Key]
		public int Id { get; set; }
		public ulong GuildId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string IconURL { get; set; }
		public List<Level> Levels { get; set; } = new List<Level>();
	}
}