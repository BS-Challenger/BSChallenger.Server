using Microsoft.AspNetCore.Mvc;

namespace BSChallenger.Server.Models.API.Authentication
{
	public class AccessTokenRequest
	{
		public Token RefreshToken { get; set; }
	}
}
