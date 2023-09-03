﻿using BSChallenger.Server.Models.API.Rankings;
using BSChallenger.Server.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API.Users
{
    [PrimaryKey("Id")]
    public class User
    {
        public User(string username)
        {
            Username = username;
        }
        public User()
        {
        }
        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.User, Id);
        public string Username { get; set; }
        public string BeatLeaderId { get; set; }
        public string PasswordHash { get; set; }
        //Dynamically set
        public DateTime LastCheckDate { get; set; }
        public List<Token> Tokens { get; set; } = new();
        public List<RankTeamMember> AssignedRankings { get; set; } = new();
    }
}