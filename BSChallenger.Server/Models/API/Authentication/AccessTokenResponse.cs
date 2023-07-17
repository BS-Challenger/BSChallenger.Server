using Microsoft.AspNetCore.Mvc;

namespace BSChallenger.Server.Models.API.Authentication
{
	public class AccessTokenResponse
	{
		public AccessTokenResponse(string accessToken, string refreshToken)
		{
			AccessToken = accessToken;
			NewRefreshToken = refreshToken;
		}
		public string AccessToken { get; set; }
		public string NewRefreshToken { get; set; }
	}
}
