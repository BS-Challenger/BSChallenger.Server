using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
	public class ImportFromPlaylist : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public ImportFromPlaylist(Database database)
		{
			_database = database;
		}
		[SlashCommand("import-playlist", "Import Level From Playlist")]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string rankingId, [Autocomplete(typeof(LevelNumberAutoComplete))] int level)
		{
			var ranking = _database.Rankings.FirstOrDefault(x => x.Identifier == rankingId);
			var user = ranking?.RankTeamMembers.FirstOrDefault(x => x.User.DiscordId == Context.User.Id.ToString());
			if (user == null || (int)user.Role < 1)
			{
				return;
			}

			var builder = new ModalBuilder()
							.WithCustomId("import_playlist")
							.WithTitle("Import Level From Playlist")
							.AddTextInput("Ranking ID", "ranking", required: true, value: rankingId)
							.AddTextInput("Level Number", "level", required: true, value: level.ToString())
							.AddTextInput("Playlist URL", "url", required: true, minLength: 15);
			await RespondWithModalAsync(builder.Build());
		}
	}
}
