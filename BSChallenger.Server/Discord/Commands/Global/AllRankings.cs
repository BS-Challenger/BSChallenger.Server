using BSChallenger.Server.Discord.Embeds;
using Discord.Interactions;
using Discord;
using System.Threading.Tasks;
using BSChallenger.Server.Models;
using System.Linq;

namespace BSChallenger.Server.Discord.Commands.Global
{
	public class AllRankings : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public AllRankings(Database database)
		{
			_database = database;
		}

		[SlashCommand("all-rankings", "Get All Available Rankings")]
		public async Task Executed()
		{
			await RespondAsync($"", new Embed[] { AllRankingsEmbed.Build(_database) });
		}
	}
}
