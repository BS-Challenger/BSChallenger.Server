using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return Ok(_database.EagerLoadRankings().Where(x=>!x.Private));
        }
    }
}