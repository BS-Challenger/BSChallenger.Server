using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands.Global
{
    public class CreateRanking : InteractionModuleBase<SocketInteractionContext>
    {
		private Database _database;
		public CreateRanking(Database database)
		{
			_database = database;
		}

		[SlashCommand("create-ranking", "Create Ranking")]
        public async Task Create()
        {
			var user = _database.EagerLoadUsers().FirstOrDefault(x => x.DiscordId == Context.User.Id.ToString());
			if (user == null)
			{
				await RespondAsync("No BSChallenger account linked to your discord!", ephemeral: true);
				return;
			}
			var builder = new ModalBuilder()
                            .WithCustomId("create_ranking")
                            .WithTitle("Create Ranking")
                            .AddTextInput("Name", "name", required: true, minLength: 5, maxLength: 15)
                            .AddTextInput("Description", "desc", required: true, placeholder: "Detail what your ranking contains and map styles", minLength: 50, maxLength: 100)
                            .AddTextInput("IconURL", "icon_url", required: true, placeholder: "URL to an image of your icon", minLength: 15)
                            .AddTextInput("Discord Server ID", "server_id", required: true, placeholder: "ID of your discord server", minLength: 15)
                            .AddTextInput("Discord Server Invite", "server_inv", required: false, placeholder: "Invite to yourdiscord server", minLength: 15);
			await RespondWithModalAsync(builder.Build());
        }
    }
}
