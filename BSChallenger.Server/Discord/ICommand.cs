using BSChallenger.Server.Models;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord
{
    public interface ICommand
    {
        public abstract string GetName();

        public abstract SlashCommandBuilder Build();

        public abstract Task Executed(SocketSlashCommand command, Database database);
    }
}
