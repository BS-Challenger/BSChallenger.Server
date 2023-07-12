using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace BSChallenger.Server.Views.API
{
	public class LevelView
	{
		public LevelView(int level, int reqMaps, string iconURL)
		{
			LevelNumber = level;
			MapsReqForPass = reqMaps;
			IconURL = iconURL;
		}
		public int LevelNumber { get; set; }
		public int MapsReqForPass { get; set; }
		public string IconURL { get; set; }
		public List<MapView> AvailableForPass { get; set; }

		public static implicit operator LevelView(Models.API.Level lvl)
		{
			var levelView = new LevelView(lvl.LevelNumber, lvl.MapsReqForPass, lvl.IconURL);
			levelView.AvailableForPass = lvl.AvailableForPass?.Select(x => (MapView)x).ToList();
			return levelView;
		}
	}
}
