using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Text.Json.Serialization;
using BSChallenger.Server.Models.API.Maps;

namespace BSChallenger.Server.Models.API.Scan
{
	[PrimaryKey("Id")]
	public class ScoreData
	{
		public ScoreData(Map map, bool passed, int score, float accuracy, string modifiers)
		{
			Map = map;
			Passed = passed;
			Score = score;
			Accuracy = accuracy;
			Modifiers = modifiers;
		}

		public ScoreData()
		{
		}

		[Key, JsonIgnore]
		public int Id { get; set; }

		public Map Map { get; set; }
		public bool Passed { get; set; }
		public int Score { get; set; }
		public float Accuracy { get; set; }
		public string Modifiers { get; set; }

		[JsonIgnore]
		public int ScanHistoryId { get; set; }
		[JsonIgnore]
		public ScanHistory ScanHistory { get; set; }
	}
}
