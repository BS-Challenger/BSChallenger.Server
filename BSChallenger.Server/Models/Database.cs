﻿using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BSChallenger.Server.Models
{
    public class Database : DbContext
    {
        private readonly SecretProvider _secrets;

        public Database(DbContextOptions<Database> options, SecretProvider secrets) : base(options)
        {
            _secrets = secrets;
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public List<Ranking> EagerLoadRankings(bool loadPasses = false)
        {
            if (loadPasses)
            {
                return Rankings
                    .Include(x => x.Levels)
                    .ThenInclude(x => x.AvailableForPass)
                    .ToList();
            }
            else
            {
                return Rankings
                    .Include(x => x.Levels)
                    .ToList();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(string.Format("Host={0};Database={1};Username={2};Password={3}", _secrets.Secrets.Database.Host, _secrets.Secrets.Database.DatabaseName, _secrets.Secrets.Database.Username, _secrets.Secrets.Database.Password));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.AssignedRankings)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        }
    }
}
