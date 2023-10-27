namespace BSChallenger.Server.Models.API.Authentication
{
    public class LoginRequest
    {
        public string BeatLeaderToken { get; set; }
        public string RedirectURL { get; set; }
	}
}
