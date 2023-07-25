using SQLite;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.Discord
{
	[PrimaryKey("GuildId")]
	public class Guild
	{
		public Guild(ulong guildID)
		{
			GuildId = guildID;
		}
		public Guild()
		{
		}
		[Key]
		public ulong GuildId { get; set; }
	}
}