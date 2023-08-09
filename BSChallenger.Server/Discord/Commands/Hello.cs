using BSChallenger.Server.Models;
using Discord;
using Discord.Net;
using Discord.Net.Queue;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
    public class Hello : ICommand
    {
        public string GetName() => "hello";
        public SlashCommandBuilder Build()
        {
            return new SlashCommandBuilder()
                        .WithName("hello")
                        .WithDescription("say hello");
        }

        public async Task Executed(SocketSlashCommand command, Database database)
        {
            await command.RespondAsync($"Hello {command.User.GlobalName}");
        }
    }
}
