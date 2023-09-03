using BSChallenger.Server.MapFeatures;
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
    public static class ListFeaturesEmbed
	{
        public static Embed Build() => new EmbedBuilder()
            .WithTitle("Available Features")
            .WithDescription("Lists all availabe features (map requirements) that you can apply to your ranked maps")
            .WithColor(new Color(114, 75, 27))
            .WithFields(
                MapFeatureFactory.CreateInstancesFromCurrentAssembly().Select(x => {
                    return new EmbedFieldBuilder()
                        .WithName(x.GetName());
                })
            )
            .WithFooter("v" + Program.Version)
            .WithCurrentTimestamp()
            .Build();
    }
}
