using BSChallenger.Server.Jobs;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Scan;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
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

		[HttpPost("/scan")]
		public async Task<ActionResult<ScanResponse>> PostScan(ScanRequest request)
		{
			var IdIdentity = HttpContext.User.Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type.Contains("nameidentifier"));
			Console.WriteLine(IdIdentity);
			var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == IdIdentity.Value);
			if (user != null)
			{
				Console.WriteLine(IdIdentity);
				Console.WriteLine(request.Ranking);
				var scores = await _beatleaderAPI.GetScoresAsync(user.BeatLeaderId, user.LastScanDate);
				var ranking = _database.EagerLoadRankings().FirstOrDefault(x => x.Identifier == request.Ranking);
				_ = Task.Run(async () =>
				{
					var proc = new ScanProcess();
					proc.ScanUser(user, ranking, scores);
					user.LastScanDate = DateTime.UtcNow;
					await _database.SaveChangesAsync();
				});
				return Ok(new ScanResponse("Scan Request Ran"));
			}
			return Unauthorized();
		}
	}
}
