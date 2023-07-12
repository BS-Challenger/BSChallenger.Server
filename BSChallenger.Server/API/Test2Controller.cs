using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
	[ApiController]
	[Route("/test2")]
	public class Test2Controller : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<AuthenticateController>();
		private readonly Database _database;

		public Test2Controller(
			Database database)
		{
			_database = database;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<LevelView>>> Get()
		{
			return _database.Rankings.First(x => x.Name == "Poodle Saber").Levels.Select(x=>(LevelView)x).ToList();
		}
	}
}
