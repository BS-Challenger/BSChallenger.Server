using Microsoft.AspNetCore.Mvc;

namespace BSChallenger.Server.Models.API.Authentication
{
	public class AccessTokenRequest
	{
		public string RefreshToken { get; set; }
	}
}
