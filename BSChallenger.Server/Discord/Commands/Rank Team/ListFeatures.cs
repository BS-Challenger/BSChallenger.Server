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
    public class ListFeatures : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("list-features", "Lists all features")]
        public async Task Create()
        {
			await RespondAsync($"", new Embed[] { ListFeaturesEmbed.Build() });
		}
    }
}
