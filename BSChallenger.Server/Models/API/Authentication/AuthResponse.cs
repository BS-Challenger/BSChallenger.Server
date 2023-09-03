using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;

namespace BSChallenger.Server.Models.API.Authentication
{
    public class AuthResponse
    {
        public AuthResponse(string response, bool isValid, Token token)
        {
            Response = response;
            IsValid = isValid;
            if (token != null)
                RefreshToken = token.TokenValue;
        }

        public string Response { get; set; }
        public bool IsValid { get; set; }
        public string RefreshToken { get; set; }
    }
}
