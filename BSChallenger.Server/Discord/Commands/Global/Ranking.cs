using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands.Global
{
	public class Ranking : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public Ranking(Database database)
		{
			_database = database;
		}

		[SlashCommand("ranking", "Get info for a ranking")]
		public async Task Executed([Autocomplete(typeof(RankingIdentifierAutoComplete))] string rankingId)
		{
			var ranking = _database.Rankings.FirstOrDefault(x => x.Identifier == rankingId);
			await RespondAsync($"", new Embed[] { RankingEmbed.Build(ranking, _database) });
		}
	}
}
