using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSChallenger.Server.Models.API.Maps
{
    [PrimaryKey("Id")]
    public class MapFeature
    {
		public MapFeature()
		{
		}

		public MapFeature(string type, string data)
		{
			Type = type;
			Data = data;
		}

		[Key, JsonIgnore]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
