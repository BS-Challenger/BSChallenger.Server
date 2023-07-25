using BSChallenger.Server.API.Authentication.BeatLeader;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Views.API;
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
	public class TestController3 : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<BeatLeaderAuthenticateController>();
		private readonly Database _database;
		private readonly BPListParser _parser;

		public TestController3(
			Database database,
			BPListParser parser)
		{
			_database = database;
			_parser = parser;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RankingView>>> Get()
		{
			Ranking testRanking = new Ranking(1127403937222369381, "Ranked Saber", "", "https://cdn.discordapp.com/icons/1046997157959442533/484935f214d4705dae0df8509084b0b9.png");
			await _database.Rankings.AddAsync(testRanking);
			await _database.SaveChangesAsync();
			var colors = GenerateDissimilarColors(23);
			for (int i = 1; i < 23; i++)
			{
				var level = new Level(testRanking, i, 1, "Default", colors[i]);
				await _database.Levels.AddAsync(level);
				await _database.SaveChangesAsync();
				string path = GetPath(i);
				await _parser.Parse(level, System.IO.File.OpenRead(path));
				_logger.Information(path);
			}
			return _database.Rankings.Select(x => RankingView.ConvertToView(x, _database)).ToList();
		}

		public static string GetPath(int number)
		{
			return Path.Combine(Environment.CurrentDirectory, "Playlists", String.Format("{0:000}", number) + "_Rank Saber.bplist");
		}

		//Silly color funcs, Temporary ofc
		public static List<string> GenerateDissimilarColors(int count)
		{
			List<Color> colors = new List<Color>();
			for (int i = 0; i < count; i++)
			{
			Generate:
				var color = GenerateNewColor();
				if (colors.Any(x => isClose(x, color)))
				{
					goto Generate;
				}
				else
				{
					colors.Add(color);
				}
			}
			return colors.Select(x => x.R.ToString("X2") + x.G.ToString("X2") + x.B.ToString("X2")).ToList();
		}

		public static double map(double s, double a1, double a2, double b1, double b2)
		{
			return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
		}

		public static Color GenerateNewColor()
		{
			return ColorFromHSV(Random.Shared.NextDouble() * 360f, map(Random.Shared.NextDouble(), 0f, 1f, 0.6f, 1f), map(Random.Shared.NextDouble(), 0f, 1f, 0.6f, 1f));
		}

		public static bool isClose(Color a, Color z, int threshold = 10)
		{
			int r = a.R - z.R,
				g = a.G - z.G,
				b = a.B - z.B;
			return (r * r + g * g + b * b) <= threshold * threshold;
		}
		public static Color ColorFromHSV(double hue, double saturation, double value)
		{
			int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
			double f = hue / 60 - Math.Floor(hue / 60);

			value = value * 255;
			int v = Convert.ToInt32(value);
			int p = Convert.ToInt32(value * (1 - saturation));
			int q = Convert.ToInt32(value * (1 - f * saturation));
			int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

			if (hi == 0)
				return Color.FromArgb(255, v, t, p);
			else if (hi == 1)
				return Color.FromArgb(255, q, v, p);
			else if (hi == 2)
				return Color.FromArgb(255, p, v, t);
			else if (hi == 3)
				return Color.FromArgb(255, p, q, v);
			else if (hi == 4)
				return Color.FromArgb(255, t, p, v);
			else
				return Color.FromArgb(255, v, p, q);
		}
	}
}
