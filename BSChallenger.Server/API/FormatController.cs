using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Rankings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BSChallenger.Server.API
{
    [ApiController]
    [Route("/rankings")]
    public class RankingsController : ControllerBase
    {
        private readonly Database _database;

        public RankingsController(
            Database database)
        {
            _database = database;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ranking>> Get()
        {
            return Ok(_database.EagerLoadRankings().Where(x => !x.Private));
        }
    }
}