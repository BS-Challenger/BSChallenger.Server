using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.Discord;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BSChallenger.Server.Models
{
    public class Database : DbContext
    {
        private SecretProvider _secrets;

        public Database(DbContextOptions<Database> options, SecretProvider secrets) : base(options)
        {
            _secrets = secrets;
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Guild> DiscordBotGuilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(string.Format("Host={0};Database={1};Username={2};Password={3}", _secrets.Secrets.Database.Host, _secrets.Secrets.Database.DatabaseName, _secrets.Secrets.Database.Username, _secrets.Secrets.Database.Password));
        }
    }
}
