using static BSChallenger.Server.Models.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Cryptography;
using System;
using System.Runtime.Serialization;

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
		[IgnoreDataMember]
		public Guid Id { get; set; }
        [Key]
        public string Name { get; set; }
        public string IconURL { get; set; }
        public List<Level> Levels { get; set; }
    }
}
