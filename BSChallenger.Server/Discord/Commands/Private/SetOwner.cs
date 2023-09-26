using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Rankings;
using Discord.Interactions;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands.Private
{
	public class SetOwner : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public SetOwner(Database database)
		{
			_database = database;
		}

		[SlashCommand("set-owner", "Changes Visiblity of a ranking")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking, string userId)
		{
			if (Context.User.Id != 741727188809810181)
			{
				await RespondAsync("Insufficient Permissions!", ephemeral: true);
				return;
			}
			var user = _database.EagerLoadUsers().FirstOrDefault(x => x.DiscordId == Context.User.Id.ToString());
			if (user == null)
			{
				await RespondAsync("No BSChallenger account linked to your discord!", ephemeral: true);
				return;
			}
			var rankingObj = _database.EagerLoadRankings().FirstOrDefault(x => x.Identifier == ranking);
			if (rankingObj == null)
			{
				await RespondAsync("Ranking not found!", ephemeral: true);
				return;
			}
			var member = rankingObj?.RankTeamMembers?.FirstOrDefault(x => x.Role == RankTeamRole.Owner);
			if (member == null)
			{
				var newOwner = new RankTeamMember()
				{
					Ranking = rankingObj,
					User = user
				};
				user.AssignedRankings.Add(newOwner);
				rankingObj.RankTeamMembers.Add(newOwner);
				await _database.SaveChangesAsync();
			}
		}
	}
}
