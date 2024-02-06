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
    public class SetLevelIcon : InteractionModuleBase<SocketInteractionContext>
    {
		private Database _database;
		public SetLevelIcon(Database database)
		{
			_database = database;
		}
		[SlashCommand("set-icon", "Set Icon of a level")]
        public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string rankingId, int level, string url)
        {
			var ranking = _database.Rankings.FirstOrDefault(x => x.Identifier == rankingId);
            var user = ranking?.RankTeamMembers.FirstOrDefault(x => x.User.DiscordId == Context.User.Id.ToString());
			if (user == null)
			{
				await RespondAsync("No BSChallenger account linked to your discord!", ephemeral: true);
				return;
			}
			if ((int)user.Role < 1)
            {
				await RespondAsync("Insufficient Permissions!", ephemeral: true);
				return;
            }

			var levelObj = ranking.Levels.FirstOrDefault(x => x.LevelNumber == level);
			if(levelObj == null)
			{
				await RespondAsync($"No Level {level} for ranking {rankingId}!", ephemeral: true);
				return;
			}

			levelObj.IconURL = url;
			await _database.SaveChangesAsync();

			if (url == "Default")
			{
				await RespondAsync($"Success! Icon URL set to default");
			}
			else
			{
				await RespondAsync($"Success! [Icon URL]({url})");
			}
        }
    }
}
