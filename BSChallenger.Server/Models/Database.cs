using BSChallenger.Server.Models.API;
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
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Token> Tokens { get; set; }
	}
}
