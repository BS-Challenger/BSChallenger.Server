using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Models;
using Discord.Interactions;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands.Private
{
	public class ChangeRankingVisibility : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public ChangeRankingVisibility(Database database)
		{
			_database = database;
		}

		[SlashCommand("change-visibility", "Changes Visiblity of a ranking")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking, bool isPrivate)
		{
			if(Context.User.Id != 1191170302944759878)
			{
				await RespondAsync("Insufficient Permissions!", ephemeral: true);
				return;
			}
			var rankingObj = _database.Rankings.Find(ranking);
			rankingObj.Private = isPrivate;
			await _database.SaveChangesAsync();
			string visibility = isPrivate ? "Private" : "Public";
			await RespondAsync($"Server Visibility changed to {visibility}!", ephemeral: true);
		}
	}
}
