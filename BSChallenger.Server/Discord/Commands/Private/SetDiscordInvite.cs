using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Rankings;
using Discord.Interactions;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands.Private
{
	public class SetDiscordInvite : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public SetDiscordInvite(Database database)
		{
			_database = database;
		}

		static Regex InviteRegex = new Regex("(https?:\\/\\/|http?:\\/\\/)?(www.)?(discord.(gg|io|me|li)|discordapp.com\\/invite|discord.com\\/invite)\\/[^\\s\\/]+?(?=\\b)");

		[SlashCommand("set-invite", "Sets the Discord Invite of a ranking")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking, string url)
		{
			if (Context.User.Id != 1191170302944759878)
			{
				await RespondAsync("Insufficient Permissions!", ephemeral: true);
				return;
			}

			var rankingObj = _database.EagerLoadRankings().FirstOrDefault(x => x.Identifier == ranking);
			if (rankingObj == null)
			{
				await RespondAsync("Ranking not found!", ephemeral: true);
				return;
			}

			if(InviteRegex.IsMatch(url))
			{
				await RespondAsync("Invite does not seem to be a valid discord invite! Setting anyways...");
			}

			rankingObj.DiscordURL = url;
			await _database.SaveChangesAsync();

			await RespondAsync($"Success! Discord invite set to <{url}>");
		}
	}
}
