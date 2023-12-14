using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Maps;
using BSChallenger.Server.Models.API.Rankings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace BSChallenger.Server.API
{
    [ApiController]
    [Route("/format/{map}")]
    public class FormatController : ControllerBase
    {
        private readonly Database _database;

        public FormatController(
            Database database)
        {
            _database = database;
        }

        IEnumerable<string> GetFeatures(Map map)
        {
			foreach (var feature in map.Features)
			{
                //TODO, use formatter function + proper formatted name
                yield return feature.Type + ": " + feature.Data;
			}
		}

        [HttpGet]
        public ActionResult<string> Get([FromRoute] string map)
        {
            var allMaps = _database.EagerLoadRankings().SelectMany(x => x.Levels).SelectMany(x => x.AvailableForPass);
            var target = allMaps.FirstOrDefault(x => x.Identifier == map);

			if (target != null)
            {
                return GetFeatures(target).Aggregate((x1, x2) => x1 + "\n" + x2);
            }
            return map;
        }
    }
}