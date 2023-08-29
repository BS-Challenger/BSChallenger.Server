using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrimaryKeyAttribute = Microsoft.EntityFrameworkCore.PrimaryKeyAttribute;

namespace BSChallenger.Server.Models.API
{
    [PrimaryKey("Id")]
    public class Ranking
    {
        public Ranking(ulong guild, string name, string desc, string iconURL)
        {
            GuildId = guild;
            Name = name;
            Description = desc;
            IconURL = iconURL;
        }
        public Ranking()
        {
        }
        [Key, JsonIgnore]
        public int Id { get; set; }
        [Key]
        public string Identifier => SqidProvider.GenerateID(IDType.Ranking, Id);
        public ulong GuildId { get; set; }
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconURL { get; set; }
        public ICollection<Level> Levels { get; set; } = new List<Level>();
    }
}