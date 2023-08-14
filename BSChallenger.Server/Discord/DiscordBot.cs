using BSChallenger.Server.Discord.Commands;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.Discord;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord
{
    public class DiscordBot : IHostedService
    {
        private DiscordSocketClient _client;
        private readonly SecretProvider _secrets;
        private readonly Database _db;
		private InteractionService _interactions;
		private readonly IServiceProvider _services;

        public DiscordBot(SecretProvider provider, Database db, IServiceProvider services)
        {
            _secrets = provider;
            _db = db;
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var token = _secrets.Secrets.DiscordBotToken;

            _client = new DiscordSocketClient();

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

            _client.Ready += async () =>
            {
                Console.WriteLine("Discord Bot Ready");

                _interactions = new InteractionService(_client);
                await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

                foreach (var guildId in _client.Guilds.Select(x=>x.Id))
                {
                    await _interactions.RegisterCommandsToGuildAsync(guildId);
                }

                _client.InteractionCreated += async interaction =>
                {
                    var scope = _services.CreateScope();
                    var ctx = new SocketInteractionContext(_client, interaction);
                    await _interactions.ExecuteCommandAsync(ctx, scope.ServiceProvider);
                };
            };
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DisposeAsync();
        }
    }
}
