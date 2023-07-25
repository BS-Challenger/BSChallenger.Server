using Discord;
using Discord.Net;
using Discord.Net.Queue;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
	public class Hello
	{
		public async Task Create(DiscordSocketClient client)
		{
			var guild = client.GetGuild(1127403937222369381);
			var guildCommand = new SlashCommandBuilder();
			guildCommand.WithName("hello");
			guildCommand.WithDescription("This is my first guild slash command!");

			try
			{
				await guild.CreateApplicationCommandAsync(guildCommand.Build());
			}
			catch (ApplicationCommandException exception)
			{
				// If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
				var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

				// You can send this error somewhere or just print it to the console, for this example we're just going to print it.
				Console.WriteLine(json);
			}
		}
	}
}
