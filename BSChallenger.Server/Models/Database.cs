using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BSChallenger.Server.Models
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Level> Levels { get; set; }

        public class Level
        {
            public int LevelID { get; set; }
            public int MapCount { get; set; }
            public List<Map> RequiredToPass { get; set; }
        }

        public class Map
        {
            public string BeatsaverID { get; set; }
            public string Hash { get; set; }
            public string Characteristic { get; set; }
            public string Difficulty { get; set; }
        }

        public class User
        {
            public string UserId { get; set; }
            public bool Patron { get; set; }
            public DateTime LastCheckDate { get; set; }
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public DateTime TokenExpiry { get; set; }
        }
    }
}
