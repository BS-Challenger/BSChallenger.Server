using Discord.Interactions;
using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using BSChallenger.Server.Models;
using System.Linq;

namespace BSChallenger.Server.Discord.Autocompletes
{
	public class RankingIdentifierAutoComplete : AutocompleteHandler
	{
		private Database _dbContext;
		public RankingIdentifierAutoComplete(Database db)
		{
			_dbContext = db;
		}

		public override Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
		{

			autocompleteInteraction.Data.Options.ToList().ForEach(x=>Console.WriteLine(x.Name));
			IEnumerable<AutocompleteResult> results = _dbContext.Rankings.Select(x=>new AutocompleteResult(x.Name, x.Identifier));
			return Task.FromResult(AutocompletionResult.FromSuccess(results.Take(25)));
		}
	}
}
