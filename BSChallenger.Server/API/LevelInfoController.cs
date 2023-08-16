using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API.Scan;
using Discord.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
	[ApiController]
	[Route("[controller]")]
	public class LevelInfoController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<LevelInfoController>();
		private readonly Database _database;

		public LevelInfoController(
			Database database)
		{
			_database = database;
		}

		[HttpGet("/GetInfo")]
		public IActionResult GetInfo([FromQuery(Name = "ranking")] string rankingName, [FromQuery(Name = "level")] int level)
		{
			_logger.Information(rankingName);
			var ranking = _database.EagerLoadRankings(true).Find(x => x.Name == rankingName);
			if (ranking == null)
			{
				return NotFound();
			}
			return Ok(ranking.Levels.ElementAt(level-1));
		}
	}
}
