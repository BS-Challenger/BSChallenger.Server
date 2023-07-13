using static BSChallenger.Server.Models.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Cryptography;
using System;
using System.Runtime.Serialization;
using System.Linq;
using BSChallenger.Server.Models;

namespace BSChallenger.Server.Views.API
{
    public class RankingView
    {
        public RankingView(string name, string desc, string iconURL)
        {
            Name = name;
			Description = desc;
            IconURL = iconURL;
        }
        public string Name { get; set; }
		public string Description { get; set; }
		public string IconURL { get; set; }
        public List<LevelView> Levels { get; set; }

		public static RankingView ConvertToView(Models.API.Ranking rnk, Database Database)
		{
			var rankingView = new RankingView(rnk.Name, rnk.Description, rnk.IconURL);
			rankingView.Levels = Database.Levels.Where(x => x.RankingId == rnk.Id).Select(x=>LevelView.ConvertToView(x, Database)).ToList();
			return rankingView;
		}
	}
}
