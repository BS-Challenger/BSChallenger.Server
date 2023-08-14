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
    public class Ranking : InteractionModuleBase<SocketInteractionContext>
    {
        private Database _database;
        public Ranking(Database database)
        {
            _database = database;
        }

        [SlashCommand("ranking", "Get the ranking for this server")]
        public async Task Executed()
        {
            var ranking = _database.Rankings.FirstOrDefault(x => x.GuildId == this.Context.Guild.Id);
            await RespondAsync($"", new Embed[] { RankingEmbed.Build(ranking, _database) });
        }
    }
}
