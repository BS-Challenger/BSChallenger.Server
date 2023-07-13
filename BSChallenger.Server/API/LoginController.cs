using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Services;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
    [ApiController]
	[Route("/accounts/Login")]
	public class LoginController : ControllerBase
	{
		private readonly ILogger _logger = Log.ForContext<LoginController>();
		private readonly Database _database;
		private readonly TokenProvider _tokenProvider;

		public LoginController(
			Database database,
			TokenProvider tokenProvider)
		{
			_database = database;
			_tokenProvider = tokenProvider;
		}
		[HttpPost]
		public async Task<ActionResult<AuthResponse>> PostLogin(NamePasswordRequest request)
		{
			if(_database.Users.Any(x=>x.UserId==request.Username))
			{
				var user = _database.Users.First(x => x.UserId == request.Username);
				if (PasswordService.Verify(request.Password, user))
				{
					//Refresh tokens will last for 1 month
					return new AuthResponse("Success", true, await _tokenProvider.GetRefreshToken(user));
				}
				else
				{
					return new AuthResponse("Username or Password is Incorrect", false, null);
				}
			}
			else
			{
				return new AuthResponse("Username or Password is Incorrect", false, null);
			}
		}
	}
}
