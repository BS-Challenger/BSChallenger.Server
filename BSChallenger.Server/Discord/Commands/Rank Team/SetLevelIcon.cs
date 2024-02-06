using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
    public class SetLevelIcon : InteractionModuleBase<SocketInteractionContext>
    {
		private Database _database;
		public SetLevelIcon(Database database)
		{
			_database = database;
		}
		[SlashCommand("add-feature", "Add Feature to Map")]
        public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string rankingId, string map, string feature)
        {
			var ranking = _database.Rankings.FirstOrDefault(x => x.Identifier == rankingId);
            var user = ranking?.RankTeamMembers.FirstOrDefault(x => x.User.DiscordId == Context.User.Id.ToString());
			if (user == null)
			{
				await RespondAsync("No BSChallenger account linked to your discord!", ephemeral: true);
				return;
			}
			if ((int)user.Role < 1)
            {
				await RespondAsync("Insufficient Permissions!", ephemeral: true);
				return;
            }

			var builder = new ModalBuilder()
                            .WithCustomId("add_feature")
                            .WithTitle("Add feature to map")
                            .AddTextInput("Ranking ID", "ranking", required: true, value: rankingId)
                            .AddTextInput("Map ID", "map", required: true, value: map)
                            .AddTextInput("Feature Name", "feature", required: true, value: feature)
                            .AddTextInput("Feature Data", "data", required: true);
            await RespondWithModalAsync(builder.Build());
        }
    }
}
