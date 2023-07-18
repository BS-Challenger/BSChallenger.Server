using BSChallenger.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication.BeatLeader
{
    [ApiController]
    [Route("[controller]")]
    public class BeatLeaderAuthenticateController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<BeatLeaderAuthenticateController>();
        private readonly Database _database;
        private readonly BeatleaderAPI _beatleaderAPI;

        public BeatLeaderAuthenticateController(
            Database database,
            BeatleaderAPI beatleaderAPI)
        {
            _database = database;
            _beatleaderAPI = beatleaderAPI;
        }

        [HttpGet("GenerateBLToken")]
        public async Task Get(string code, string state)
        {
            if (code is null || state is null)
                throw new HttpResponseException(400);
        }
    }
}
