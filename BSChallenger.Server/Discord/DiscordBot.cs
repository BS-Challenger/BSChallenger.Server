using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Maps;
using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Providers;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
		private readonly HttpClient _httpClient = new HttpClient();
		private readonly BPListParserProvider _parser;

		public DiscordBot(SecretProvider provider, DiscordSocketClient client, InteractionService interactions, Database db, IServiceProvider services, BPListParserProvider parser)
		{
			_secrets = provider;
			_db = db;
			_services = services;
			_interactions = interactions;
			_client = client;
			_parser = parser;

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
				List<SocketMessageComponentData> components = x.Data.Components.ToList();
				//TODO: Seperate this into classes
				if (x.Data.CustomId == "create_ranking")
				{
					var user = _db.EagerLoadUsers().FirstOrDefault(y => y.DiscordId == x.User.Id.ToString());

					string name = GetModalItem(components, "name");
					string desc = GetModalItem(components, "desc");
					string iconURL = GetModalItem(components, "icon_url");
					string guildId = GetModalItem(components, "server_id");

					if (ulong.TryParse(guildId, out var id))
					{
						var ranking = new Ranking(id, name, desc, iconURL)
						{
							Private = true,
							RankTeamMembers = new List<RankTeamMember>()
							{
								new RankTeamMember()
								{
									User = user
								}
							}
						};
						await _db.Rankings.AddAsync(ranking);
						await _db.SaveChangesAsync();

						var channel = await _client.GetChannelAsync(1151308540712075336) as IMessageChannel;
						await channel.SendMessageAsync("New Ranking Created!", embed: RankingEmbed.Build(ranking, _db, false));

						await x.RespondAsync("Successfully created ranking!", ephemeral: true);
					}
					else
					{
						await x.RespondAsync("GuildID is in incorrect format", ephemeral: true);
					}
				}

				if (x.Data.CustomId == "add_map")
				{
					string rankingId = GetModalItem(components, "ranking");
					int lvl = int.Parse(GetModalItem(components, "level"));
					string id = GetModalItem(components, "id");
					string chari = GetModalItem(components, "char");
					string diff = GetModalItem(components, "difficulty");

					var ranking = _db.EagerLoadRankings().FirstOrDefault(x => x.Identifier == rankingId);
					if (ranking != null)
					{
						Level level = ranking.Levels.FirstOrDefault(x => x.LevelNumber == lvl);
						if (level == null)
						{
							level = new Level(lvl, 1, "", "");
							ranking.Levels.Add(level);
						}
						var map = new Map(id, chari, diff);
						level.AvailableForPass.Add(map);
					}
					else
					{
						await x.RespondAsync("Failed to add map!");
					}
					await _db.SaveChangesAsync();
				}

				if (x.Data.CustomId == "import_playlist")
				{
					string rankingId = GetModalItem(components, "ranking");
					int lvl = int.Parse(GetModalItem(components, "level"));
					string url = GetModalItem(components, "url");

					var ranking = _db.EagerLoadRankings().FirstOrDefault(x => x.Identifier == rankingId);
					if (ranking != null)
					{
						var playlist = await _httpClient.GetAsync(url);
						if (playlist.IsSuccessStatusCode)
						{
							Level level = ranking.Levels.FirstOrDefault(x => x.LevelNumber == lvl);
							if (level == null)
							{
								level = new Level(lvl, 1, "", "");
								ranking.Levels.Add(level);
							}
							var stream = await playlist.Content.ReadAsStreamAsync();
							await _parser.Parse(level, stream);
							await _db.SaveChangesAsync();
						}
						else
						{
							await x.RespondAsync("Failed to download playlist!");
						}
					}
					else
					{
						await x.RespondAsync("Failed to find ranking!");
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
