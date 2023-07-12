using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Views.API
{
	public class MapView
	{
		public MapView(string hash, string chari, string diff)
		{
			Hash = hash;
			Characteristic = chari;
			Difficulty = diff;
		}
		public string Hash { get; set; }
		public string Characteristic { get; set; }
		public string Difficulty { get; set; }

		public static implicit operator MapView(Models.API.Map lvl)
		{
			return new MapView(lvl.Hash, lvl.Characteristic, lvl.Difficulty);
		}
	}
}
