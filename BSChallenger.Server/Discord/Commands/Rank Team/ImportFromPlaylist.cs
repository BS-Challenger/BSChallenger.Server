using BSChallenger.Server.Discord.Autocompletes;
using Discord;
using Discord.Interactions;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
	public class ImportFromPlaylist : InteractionModuleBase<SocketInteractionContext>
	{
		[SlashCommand("import-playlist", "Import Level From Playlist")]
		[RequireRole("Rank Team")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking, [Autocomplete(typeof(LevelNumberAutoComplete))] int level)
		{
			var builder = new ModalBuilder()
							.WithCustomId("import_playlist")
							.WithTitle("Import Level From Playlist")
							.AddTextInput("Ranking ID", "ranking", required: true, value: ranking)
							.AddTextInput("Level Number", "level", required: true, value: level.ToString())
							.AddTextInput("Playlist URL", "url", required: true, minLength: 15);
			await RespondWithModalAsync(builder.Build());
		}
	}
}
