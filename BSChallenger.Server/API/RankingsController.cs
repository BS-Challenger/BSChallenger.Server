using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
	[ApiController]
	[Route("/rankings")]
	public class RankingsController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<AuthenticateController>();
		private readonly Database _database;

		public RankingsController(
			Database database)
		{
			_database = database;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Ranking>>> Get()
		{
			return _database.Rankings;
		}
	}
}
