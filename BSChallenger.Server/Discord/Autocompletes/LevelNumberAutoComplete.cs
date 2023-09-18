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
    public class LevelNumberAutoComplete : AutocompleteHandler
    {
        private Database _dbContext;
        public LevelNumberAutoComplete(Database db)
        {
            _dbContext = db;
        }

        public override Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            string rankingId = (string)autocompleteInteraction.Data.Options?.FirstOrDefault(x => x.Name == "ranking")?.Value;
            var ranking = _dbContext.EagerLoadRankings().FirstOrDefault(x => x.Identifier == rankingId);
            if (ranking != null)
            {
                IEnumerable<AutocompleteResult> results = ranking.Levels.Select(x => new AutocompleteResult(x.LevelNumber.ToString(), x.LevelNumber));
                return Task.FromResult(AutocompletionResult.FromSuccess(results.Take(25)));
            }
            return Task.FromResult(AutocompletionResult.FromSuccess(Enumerable.Empty<AutocompleteResult>()));
        }
    }
}
