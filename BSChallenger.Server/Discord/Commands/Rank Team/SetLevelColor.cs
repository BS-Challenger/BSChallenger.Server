using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Discord.Embeds;
using BSChallenger.Server.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands
{
    public class SetLevelColor : InteractionModuleBase<SocketInteractionContext>
    {
		private Database _database;
		public SetLevelColor(Database database)
		{
			_database = database;
		}

		static Regex ColorRegex = new Regex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");

		[SlashCommand("set-color", "Set Color of a level")]
        public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string rankingId, int level, string color)
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
			if (ColorRegex.IsMatch(color))
			{
				if (color == "")
				{
					double map(double s, double a1, double a2, double b1, double b2)
					{
						return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
					}

					System.Drawing.Color GenerateNewColor()
					{
						return Utils.ColorUtils.ColorFromHSV(Random.Shared.NextDouble() * 360f, map(Random.Shared.NextDouble(), 0f, 1f, 0.6f, 1f), map(Random.Shared.NextDouble(), 0f, 1f, 0.6f, 1f));
					}

					var genColor = GenerateNewColor();
					levelObj.Color = "#" + genColor.R.ToString("X2") + genColor.G.ToString("X2") + genColor.B.ToString("X2");
					await _database.SaveChangesAsync();

					await RespondAsync($"Success! Icon Color set to random color... ({levelObj.Color})");
				}
				else
				{
					levelObj.Color = color;
					await _database.SaveChangesAsync();

					await RespondAsync($"Success!");
				}
			}
			else
			{
				await RespondAsync("Color must be a valid hexadecimal color with a # in front! I.E #FFFFFF or #000000", ephemeral: true);
			}
        }
    }
}
