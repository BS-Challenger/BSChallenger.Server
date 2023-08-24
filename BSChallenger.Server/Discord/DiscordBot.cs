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

        public DiscordBot(SecretProvider provider, DiscordSocketClient client, InteractionService interactions, Database db, IServiceProvider services)
        {
            _secrets = provider;
            _db = db;
            _services = services;
            _interactions = interactions;
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var token = _secrets.Secrets.DiscordBotToken;

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _interactions.Modules.ToList().ForEach((x) => { Console.WriteLine(x.Name); });

			_client.Ready += async () =>
            {
                Console.WriteLine("Discord Bot Ready");

				await _interactions.RegisterCommandsGloballyAsync(true);

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
