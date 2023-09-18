using BeatSaberPlaylistsLib.Types;
using BSChallenger.Server.MapFeatures;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Scan;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
    [ApiController]
    [Route("[controller]")]
    public class ScanController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<ScanController>();
        private readonly Database _database;
        private readonly JWTProvider _jwtProvider;
        private readonly BeatLeaderApiProvider _beatleaderAPI;

        public ScanController(
            Database database,
			JWTProvider jwtProvider,
			BeatLeaderApiProvider beatleaderAPI)
        {
            _database = database;
            _jwtProvider = jwtProvider;
            _beatleaderAPI = beatleaderAPI;
        }

        //TODO: Split this up into seperate methods
        [HttpPost("/scan")]
        public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest request)
        {
			var IdIdentity = HttpContext.User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type.Contains("nameidentifier"));
            Console.WriteLine(IdIdentity);
			var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == IdIdentity.Value);
			if (user != null)
			{
				var scores = await _beatleaderAPI.GetScoresAsync(user.BeatLeaderId, user.LastScanDate);
				var ranking = _database.EagerLoadRankings().FirstOrDefault(x => x.Name == request.Ranking);

				user.LastScanDate = DateTime.UtcNow;
				return Ok(new ScanResponse("Scan Request Passed"));
			}
			return Unauthorized();
		}
    }
}
