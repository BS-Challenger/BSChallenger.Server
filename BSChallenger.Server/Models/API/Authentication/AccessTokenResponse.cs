using Microsoft.AspNetCore.Mvc;

namespace BSChallenger.Server.Models.API.Authentication
{
	public class AccessTokenResponse
	{
		public Token AccessToken { get; set; }
	}
}
