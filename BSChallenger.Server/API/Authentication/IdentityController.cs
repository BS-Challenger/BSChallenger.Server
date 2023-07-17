using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("/accounts/Identity")]
	public class IdentityController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<LoginController>();
		private readonly Database _database;

		public IdentityController(
			Database database)
		{
			_database = database;
		}

		//Why is this post? Idk but http client doesnt let me pass a body for get requests
		[HttpPost]
		public ActionResult<IdentityResponse> Post(IdentityRequest request)
		{
			var token = _database.Tokens.FirstOrDefault(x => x.token == request.AccessToken && x.isAccessToken);

			if (token != null)
			{
				var user = _database.Users.FirstOrDefault(x => x.Id == token.UserId);
				if (user != null)
				{
					return new IdentityResponse(user.Id, user.Username);
				}
			}
			return new IdentityResponse(-1, "Identity Request Failed");
		}
	}
}
