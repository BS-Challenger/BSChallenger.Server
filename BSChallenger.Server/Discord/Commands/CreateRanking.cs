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
        [SlashCommand("create-ranking", "Create Ranking")]
        [RequireRole("Ranking Owners")]
		[RequireRole("Ranking Admin")]
		public async Task Create()
        {
            Console.WriteLine("x");
            var builder = new ModalBuilder()
                            .WithCustomId("create_ranking")
                            .WithTitle("Create Ranking")
                            .AddTextInput("Name", "name", required: true, minLength: 5, maxLength: 15)
                            .AddTextInput("Description", "desc", required: true, placeholder: "Detail what your ranking contains and map styles", minLength: 10, maxLength: 40)
                            .AddTextInput("IconURL", "icon_url", required: true, placeholder: "URL to an image of your icon", minLength: 15)
                            .AddTextInput("Discord Server ID", "server_id", required: true, placeholder: "ID of your discord server", minLength: 15);
			Console.WriteLine("x2");
			await RespondWithModalAsync(builder.Build());
			Console.WriteLine("x3");
		}
    }
}
