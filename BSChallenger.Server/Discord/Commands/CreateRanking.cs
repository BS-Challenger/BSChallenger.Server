using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
    public class CreateRanking : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("createRanking", "Create Ranking")]
        public async Task Create()
        {
            await RespondWithModalAsync(new ModalBuilder()
                                            .WithTitle("Create Ranking")
                                            .Build());
        }
    }
}
