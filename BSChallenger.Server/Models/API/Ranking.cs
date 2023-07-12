using static BSChallenger.Server.Models.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Cryptography;
using System;

namespace BSChallenger.Server.Models.API
{
    [PrimaryKey("Id")]
    public class Ranking
    {
        public Ranking(string name, string iconURL)
        {
            Name = name;
            IconURL = iconURL;
            Id = Guid.NewGuid();
        }
		public Ranking()
		{
			Id = Guid.NewGuid();
		}
		[Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IconURL { get; set; }
        public DbSet<Level> Levels { get; set; }
    }
}
