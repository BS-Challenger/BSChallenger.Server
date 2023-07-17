using Microsoft.AspNetCore.Mvc;

namespace BSChallenger.Server.Models.API.Authentication
{
	public class AccessTokenResponse
	{
		public AccessTokenResponse(string accessToken)
		{
			AccessToken = accessToken;
		}
		public string AccessToken { get; set; }
	}
}
