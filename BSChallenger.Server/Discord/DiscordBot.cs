using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Providers;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TokenType = Discord.TokenType;

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

            _interactions.Log += (x) =>
            {
                Console.WriteLine(x);
                return Task.CompletedTask;
            };

            _client.Log += (x) =>
            {
                Console.WriteLine(x);
                return Task.CompletedTask;
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var token = _secrets.Secrets.DiscordBotToken;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.Ready += async () =>
            {
                Console.WriteLine("Discord Bot Ready");

                await _interactions.RegisterCommandsToGuildAsync(1127403937222369381, true);

                _client.InteractionCreated += async interaction =>
                {
                    var ctx = new SocketInteractionContext(_client, interaction);
                    await _interactions.ExecuteCommandAsync(ctx, _services);
                };
            };

            _client.ModalSubmitted += async (x) =>
            {
                //TODO: Seperate this into classes
                if (x.Data.CustomId == "create_ranking")
                {
					await (x.User as SocketGuildUser).RemoveRoleAsync(1147688876106842193);
					List<SocketMessageComponentData> components = x.Data.Components.ToList();
                    string name = GetModalItem(components, "name");
                    string desc = GetModalItem(components, "desc");
                    string iconURL = GetModalItem(components, "icon_url");
                    string guildId = GetModalItem(components, "server_id");

                    if (ulong.TryParse(guildId, out var id))
                    {
                        var ranking = new Ranking(id, name, desc, iconURL);
                        await _db.Rankings.AddAsync(ranking);
                        await _db.SaveChangesAsync();

                        await x.RespondAsync("Successfully created ranking!", ephemeral: true);
                    }
                    else
                    {
                        await x.RespondAsync("GuildID is in incorrect format", ephemeral: true);
                    }
                }
            };
        }

        private string GetModalItem(List<SocketMessageComponentData> components, string id) => components.First(x => x.CustomId == id).Value;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DisposeAsync();
        }
    }
}
