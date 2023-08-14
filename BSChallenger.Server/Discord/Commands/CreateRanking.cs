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
		public string GetName() => "createRanking";
		public SlashCommandBuilder Build()
		{
			return new SlashCommandBuilder()
						.WithName("createRanking")
						.AddOption("Name", ApplicationCommandOptionType.String, "Name of the ranking", true)
						.AddOption("Desc", ApplicationCommandOptionType.String, "Description of the ranking", true)
						.WithDescription("Create Ranking");
		}

		[SlashCommand("Create Ranking", "Create Ranking")]
		public async Task Create()
		{
			await RespondWithModalAsync(new ModalBuilder()
											.WithTitle("Create Ranking")
											.Build());
		}
	}
}
