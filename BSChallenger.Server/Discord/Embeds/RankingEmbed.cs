using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Rankings;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Color = Discord.Color;

namespace BSChallenger.Server.Discord.Embeds
{
    public static class RankingsEmbed
    {
        public static Embed Build(Database database) => new EmbedBuilder()
            .WithTitle("All Rankings")
            .WithColor(new Color(114, 75, 27))
            .WithFields(
                database.EagerLoadRankings().Select((x) =>
                {
                    return new EmbedFieldBuilder()
                        .WithName(x.Name)
                        .WithValue("Levels: " + x.Levels.Count);
                })
            )
            .WithFooter("v" + Program.Version)
            .WithCurrentTimestamp()
            .Build();
    }
}
