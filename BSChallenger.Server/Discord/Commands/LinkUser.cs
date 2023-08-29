using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.Net.Queue;
using Discord.WebSocket;
using Microsoft.AspNetCore.Server.IIS.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
	public class linkUser : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public linkUser(Database database)
		{
			_database = database;
		}

		[SlashCommand("link-user", "Link your discord account to your challenger account")]
		public async Task Executed()
		{
			var ranking = _database.Rankings.FirstOrDefault(x => x.GuildId == this.Context.Guild.Id);
			await RespondAsync($"", new Embed[] { RankingEmbed.Build(ranking, _database) });
		}
	}
}
