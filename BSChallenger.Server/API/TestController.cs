﻿using BSChallenger.Server.API.Authentication.BeatLeader;
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
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
    [ApiController]
	[Route("/test2")]
	public class TestController2 : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<AuthenticateController>();
		private readonly Database _database;

		public TestController2(
			Database database)
		{
			_database = database;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RankingView>>> Get()
		{
			Ranking testRanking = new Ranking("Modifier Saber", "", "https://cdn.discordapp.com/icons/1115430594327879780/12f50ae1c875576077c0691ebf8ab40f.png");
			await _database.Rankings.AddAsync(testRanking);
			await _database.SaveChangesAsync();
			var colors = GenerateDissimilarColors(4);
			for (int i = 1; i < 4; i++)
			{
				var level = new Level(testRanking, i, 1, "Default", colors[i]);
				await _database.Levels.AddAsync(level);
				await _database.SaveChangesAsync();
				switch (i)
				{
					case 1:

						await _database.Maps.AddAsync(new Map(level, "6ee71d3d49f5c3a2b3be71dd143f93890e383870", "Standard", "ExpertPlus"));
						await _database.Maps.AddAsync(new Map(level, "ca8f10245fdc772388d3bc6e0e956edbd791f395", "Standard", "Expert"));
						await _database.Maps.AddAsync(new Map(level, "332a9151fa207d3a7a9da369a134cdd6f7dc20f4", "Standard", "Hard"));
						await _database.Maps.AddAsync(new Map(level, "429ff959e81100e5afaf2019c921ef5218634bc6", "Standard", "Expert"));
						await _database.Maps.AddAsync(new Map(level, "429ff959e81100e5afaf2019c921ef5218634bc6", "Standard", "ExpertPlus"));
						break;
					case 2:
						await _database.Maps.AddAsync(new Map(level, "3f567bc5cc7ada8c9b5bc1436960c65c92355972", "Standard", "ExpertPlus"));
						await _database.Maps.AddAsync(new Map(level, "c930e8245776f2a51de2eff0e7ba036d74068cba", "Standard", "ExpertPlus"));
						await _database.Maps.AddAsync(new Map(level, "47d574d2274c36b45a63a7b808bcf74b49a0a3ba", "Standard", "ExpertPlus"));
						await _database.Maps.AddAsync(new Map(level, "b4f3f4e5cacd3422eda0ef199a24ee5d957ab53f", "Standard", "ExpertPlus"));
						break;
					case 3:
						await _database.Maps.AddAsync(new Map(level, "ad6c9f88d63259a95e39397c31be2981c4beb744", "Standard", "ExpertPlus"));
						await _database.Maps.AddAsync(new Map(level, "b467c05ba4dcf28d242aca6994d1591b02eeff47", "Standard", "ExpertPlus"));
						await _database.Maps.AddAsync(new Map(level, "eaddeb51358bbd688a57923ddc23a230ca81609c", "Standard", "ExpertPlus"));
						break;
				}
			}
			return _database.Rankings.Select(x => RankingView.ConvertToView(x, _database)).ToList();
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

		public static bool isClose(Color a, Color z, int threshold = 15)
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
