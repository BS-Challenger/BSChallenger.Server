using BSChallenger.Server.API.Authentication;
using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
    public class AddMap : InteractionModuleBase<SocketInteractionContext>
    {
		private Database _database;
		private readonly ILogger _logger = Log.ForContext<AddMap>();
		public AddMap(Database database)
		{
			_database = database;
		}
		[SlashCommand("add-map", "Add Map to Ranking")]
        public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string rankingId, [Autocomplete(typeof(LevelNumberAutoComplete))] int level)
        {
			var ranking = _database.EagerLoadRankings().AsEnumerable().FirstOrDefault(x => x.Identifier == rankingId);
			var members =  ranking?.RankTeamMembers.AsEnumerable();
			var user = members.FirstOrDefault(x => x.User.DiscordId == Context.User.Id.ToString());

			if (user == null || (int)user.Role < 1)
			{
				await RespondAsync("Insufficient Permissions!", ephemeral: true);
				return;
			}
			var builder = new ModalBuilder()
                            .WithCustomId("add_map")
                            .WithTitle("Add level to ranking")
                            .AddTextInput("Ranking ID", "ranking", required: true, value: rankingId)
                            .AddTextInput("Level Number", "level", required: true, value: level.ToString())
                            .AddTextInput("BeatSaver Key", "id", required: true, placeholder: "25f", minLength: 15)
                            .AddTextInput("Characteristic", "char", required: true, placeholder: "Standard, One Saber, 360, 90, Lawless", minLength: 5)
                            .AddTextInput("Difficulty", "difficulty", required: true, placeholder: "Easy, Normal, Hard, Expert, Expert+", minLength: 4);
            await RespondWithModalAsync(builder.Build());
        }
    }
}
