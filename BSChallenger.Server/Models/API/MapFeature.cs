using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BSChallenger.Server.Models.API
{
    [PrimaryKey("Id")]
    public class MapFeature
    {
		[Key]
		public int Id { get; set; }
		public string Type { get; set; }
        public string Data { get; set; }
	}
}
