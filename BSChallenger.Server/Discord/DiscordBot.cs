using BSChallenger.Server.Discord.Commands;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.Discord;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord
{
	public class DiscordBot : IHostedService
	{
		private DiscordSocketClient _client;
		private readonly SecretProvider _secrets;
		private readonly Database _db;

		private readonly List<ICommand> commands = new()
		{
			new Hello(),
			new Ranking()
		};

		public DiscordBot(SecretProvider provider, Database db)
		{
			_secrets = provider;
			_db = db;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var token = _secrets.Secrets.DiscordBotToken;

			_client = new DiscordSocketClient();

			Console.WriteLine("u");
			_client.Ready += async () =>
			{
				Console.WriteLine("Ready");
				Console.WriteLine(_db.DiscordBotGuilds.Count());
				foreach(var guildId in _db.DiscordBotGuilds)
				{
					Console.WriteLine(guildId.GuildId.ToString());
					var guild = _client.GetGuild(guildId.GuildId);
					Console.WriteLine(guild.Name);
					foreach (var command in commands)
					{
						Console.WriteLine(command.GetName());
						await guild.CreateApplicationCommandAsync(command.Build().Build());
					}
				}
			};

			_client.SlashCommandExecuted += SlashCommandHandler;

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();
		}

		private async Task SlashCommandHandler(SocketSlashCommand command)
		{
			var localCommand = commands.Find(x => x.GetName() == command.Data.Name);
			if(localCommand != null)
			{
				await localCommand.Executed(command, _db);
			}
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _client.DisposeAsync();
		}
	}
}
