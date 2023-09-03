using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSChallenger.Server.Models.API.Maps
{
    [PrimaryKey("Id")]
    public class MapFeature
    {
        [Key, JsonIgnore]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
