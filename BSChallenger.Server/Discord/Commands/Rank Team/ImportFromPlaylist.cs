using BSChallenger.Server.Discord.Autocompletes;
using Discord.Interactions;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
	public class ImportFromPlaylist : InteractionModuleBase<SocketInteractionContext>
	{
		[SlashCommand("add-map", "Add Map to Ranking")]
		[RequireRole("Rank Team")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking, [Autocomplete(typeof(LevelNumberAutoComplete))] int level)
		{
			var builder = new ModalBuilder()
							.WithCustomId("add_map")
							.WithTitle("Add level to ranking")
							.AddTextInput("Ranking ID", "ranking", required: true, value: ranking)
							.AddTextInput("Level Number", "level", required: true, value: level.ToString())
							.AddTextInput("Hash", "hash", required: true, minLength: 15)
							.AddTextInput("Characteristic", "char", required: true, minLength: 5)
							.AddTextInput("Difficulty", "difficulty", required: true, placeholder: "Easy, Normal, Hard, Expert, Expert+", minLength: 4);
			await RespondWithModalAsync(builder.Build());
		}
	}
}
