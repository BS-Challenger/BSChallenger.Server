namespace BSChallenger.Server.Models.API.Authentication
{
	public class DiscordAuthRequest
	{
		public string DiscordOauthCode { get; set; }
		public string DiscordRedirectURL { get; set; }
	}
}
