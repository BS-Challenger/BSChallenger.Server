using Microsoft.AspNetCore.Mvc;

namespace BSChallenger.Server.Models.API.Authentication
{
	public class AuthResponse
	{
		public AuthResponse(string response, bool isValid, Token token)
		{
			Response = response;
			IsValid = isValid;
			RefreshToken = token;
		}

		public string Response { get; set; }
		public bool IsValid { get; set; }
		public Token RefreshToken { get; set; }
	}
}
