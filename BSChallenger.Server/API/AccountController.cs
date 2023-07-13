using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
	[ApiController]
	[Route("/accounts")]
	public class AccountController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<AccountController>();
		private readonly Database _database;

		public AccountController(
			Database database)
		{
			_database = database;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserView>>> Get()
		{
			return _database.Users.Select(x => (UserView)x).ToList();
		}
	}
}
