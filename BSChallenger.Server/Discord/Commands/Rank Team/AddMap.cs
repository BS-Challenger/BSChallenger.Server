using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
    public class AddMap : InteractionModuleBase<SocketInteractionContext>
    {
		private Database _database;
		public AddMap(Database database)
		{
			_database = database;
		}
		[SlashCommand("add-map", "Add Map to Ranking")]
        public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string rankingId, [Autocomplete(typeof(LevelNumberAutoComplete))] int level)
        {
			var ranking = _database.Rankings.FirstOrDefault(x => x.Identifier == rankingId);
			var user = ranking?.RankTeamMembers.FirstOrDefault(x => x.User.DiscordId == Context.User.Id.ToString());
			if (user == null || (int)user.Role < 1)
			{
				return;
			}
			var builder = new ModalBuilder()
                            .WithCustomId("add_map")
                            .WithTitle("Add level to ranking")
                            .AddTextInput("Ranking ID", "ranking", required: true, value: rankingId)
                            .AddTextInput("Level Number", "level", required: true, value: level.ToString())
                            .AddTextInput("Hash", "hash", required: true, minLength: 15)
                            .AddTextInput("Characteristic", "char", required: true, minLength: 5)
                            .AddTextInput("Difficulty", "difficulty", required: true, placeholder: "Easy, Normal, Hard, Expert, Expert+", minLength: 4);
            await RespondWithModalAsync(builder.Build());
        }
    }
}
