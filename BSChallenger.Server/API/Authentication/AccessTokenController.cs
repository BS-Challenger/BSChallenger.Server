using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
	[ApiController]
	[Route("/accounts/Access")]
	public class AccessTokenController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<AccessTokenController>();
		private readonly Database _database;
		private readonly TokenProvider _tokenProvider;

		public AccessTokenController(
			Database database,
			TokenProvider tokenProvider)
		{
			_database = database;
			_tokenProvider = tokenProvider;
		}
		[HttpPost]
		public async Task<ActionResult<AccessTokenResponse>> Post(AccessTokenRequest request)
		{
			//ToList because the expression was too silly for SQL
			var refreshToken = _database.Tokens.ToList().FirstOrDefault(x => x.token == request.RefreshToken);

			if (refreshToken != null && !refreshToken.isAccessToken)
			{
				var user = _database.Users.FirstOrDefault(x => x.Id == refreshToken.UserId);
				var token = await _tokenProvider.GetAccessToken(user);
				return new AccessTokenResponse(token.token);
			}
			return new AccessTokenResponse("Request Failed");
		}
	}
}