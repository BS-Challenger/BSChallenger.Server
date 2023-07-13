using BSChallenger.Server.Models;
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

		public static LevelView ConvertToView(Models.API.Level lvl, Database Database)
		{
			var levelView = new LevelView(lvl.LevelNumber, lvl.MapsReqForPass, lvl.IconURL);
			levelView.AvailableForPass = Database.Maps.Where(x => x.LevelId == lvl.Id).Select(x=>(MapView)x).ToList();
			return levelView;
		}
	}
}
