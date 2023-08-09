using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Net;
using Discord.Net.Queue;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
	public class Ranking : ICommand
	{
		public string GetName() => "ranking";
		public SlashCommandBuilder Build()
		{
			return new SlashCommandBuilder()
						.WithName("ranking")
						.WithDescription("Get the ranking for this server");
		}

		public async Task Executed(SocketSlashCommand command, Database database)
		{
			var ranking = database.Rankings.FirstOrDefault(x => x.GuildId == command.GuildId);
			await command.RespondAsync($"", new Embed[] { RankingEmbed.Build(ranking, command, database) });
		}
	}
}
