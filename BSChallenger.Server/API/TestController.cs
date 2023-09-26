
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
	[ApiController]
	[Route("/test3")]
	public class TestController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<TestController>();
		private readonly Database _database;
		private readonly BPListParserProvider _parser;
		public TestController(
			Database database,
			BPListParserProvider parser)
		{
			_database = database;
			_parser = parser;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Ranking>>> Get()
		{
			Ranking testRanking = new Ranking(1127403937222369381, "Challenge Community", "", "https://cdn.discordapp.com/icons/1046997157959442533/484935f214d4705dae0df8509084b0b9.png");
			await _database.Rankings.AddAsync(testRanking);
			await _database.SaveChangesAsync();
			for (int i = 1; i < 32; i++)
			{
				var level = new Level(i, 1, "Default", "#FFFFFF");
				testRanking.Levels.Add(level);
				await _database.SaveChangesAsync();
				string path = GetPath(i);
				Console.WriteLine(path);
				await _parser.Parse(level, System.IO.File.OpenRead(path));
			}
			return Ok(_database.EagerLoadRankings());
		}
		public static string GetPath(int number)
		{
			return Path.Combine(Environment.CurrentDirectory, "playlists", String.Format("{0:000}", number) + "_Beat Saber Challenge Community.bplist");
		}
	}
}