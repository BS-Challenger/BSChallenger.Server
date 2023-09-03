using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
	public class AddFeature : InteractionModuleBase<SocketInteractionContext>
	{
		[SlashCommand("add-feature", "Add Feature to Map")]
		[RequireRole("Rank Team")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking, string map, string feature)
		{
			var builder = new ModalBuilder()
							.WithCustomId("add_map")
							.WithTitle("Add level to ranking")
							.AddTextInput("Ranking ID", "ranking", required: true, value: ranking)
							.AddTextInput("Map ID", "map", required: true, value: map)
							.AddTextInput("Featue Name", "feature", required: true, value: feature)
							.AddTextInput("Feature Data", "data", required: true);
			await RespondWithModalAsync(builder.Build());
		}
	}
}
