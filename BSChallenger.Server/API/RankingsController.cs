using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
	[ApiController]
	[Route("/rankings")]
	public class RankingsController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<RankingsController>();
		private readonly Database _database;

		public RankingsController(
			Database database)
		{
			_database = database;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RankingView>>> Get()
		{
			return _database.Rankings.Select(x=>(RankingView)x).ToList();
		}
	}
}
