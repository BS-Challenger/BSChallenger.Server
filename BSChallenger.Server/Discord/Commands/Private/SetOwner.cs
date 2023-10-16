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

		[SlashCommand("set-owner", "Sets the Owner of a ranking")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking, string userId)
		{
			if (Context.User.Id != 1163263452098347118)
			{
				await RespondAsync("Insufficient Permissions!", ephemeral: true);
				return;
			}
			var user = _database.EagerLoadUsers().FirstOrDefault(x => x.DiscordId == userId);
			if (user == null)
			{
				await RespondAsync("No BSChallenger account linked to this discord user!", ephemeral: true);
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
			else
			{
				member.User = user;
				await _database.SaveChangesAsync();
			}
		}
	}
}
