﻿using BSChallenger.Server.Discord.Autocompletes;
using BSChallenger.Server.Models;
using Discord.Interactions;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Discord.Commands.Private
{
	public class PartnerRanking : InteractionModuleBase<SocketInteractionContext>
	{
		private Database _database;
		public PartnerRanking(Database database)
		{
			_database = database;
		}

		[SlashCommand("partner-ranking", "Partners a ranking")]
		[RequireOwner]
		public async Task Create([Autocomplete(typeof(RankingIdentifierAutoComplete))] string ranking)
		{
			await RespondAsync(ranking, ephemeral: true);
			var rankingObj = _database.Rankings.FirstOrDefault(x=>x.Name == ranking);
			rankingObj.Partnered = true;
			await _database.SaveChangesAsync();
			await RespondAsync("Server Partnered!", ephemeral: true);
		}
	}
}
