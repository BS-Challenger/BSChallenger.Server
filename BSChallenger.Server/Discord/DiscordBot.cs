using BSChallenger.Server.Discord.Commands;
using BSChallenger.Server.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord
{
	public class DiscordBot : IHostedService
	{
		private DiscordSocketClient _client;
		private SecretProvider _secrets;

		public DiscordBot(SecretProvider provider)
		{
			_secrets = provider;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var token = _secrets.Secrets.DiscordBotToken;

			_client = new DiscordSocketClient();

			_client.Ready += async () =>
			{
				await new Hello().Create(_client);
			};

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
		}
	}
}
