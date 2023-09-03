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
    public static class RankingEmbed
    {
        public static Embed Build(Ranking ranking, Database database) => new EmbedBuilder()
            .WithTitle(ranking.Name)
            .WithColor(new Color(114, 75, 27))
            .WithThumbnailUrl(ranking.IconURL)
            .WithFields(new List<EmbedFieldBuilder>() {
                new EmbedFieldBuilder()
                    .WithName("Active Users")
                    .WithIsInline(true)
                    .WithValue("45"),
                new EmbedFieldBuilder()
                    .WithName("Weekly Scores")
                    .WithIsInline(true)
                    .WithValue("15"),
                new EmbedFieldBuilder()
                    .WithName("Levels")
                    .WithIsInline(true)
                    .WithValue(database.EagerLoadRankings().Find(x=>x.Identifier==ranking.Identifier).Levels.Count().ToString())
            })
            .WithFooter("v" + Program.Version)
            .WithCurrentTimestamp()
            .Build();
    }
}
