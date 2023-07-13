namespace BSChallenger.Server.Models.API.Authentication
{
	public class NamePasswordRequest
	{
		public string Username { get; set; }
		//NEVER STORE THIS!
		public string Password { get; set; }
	}
}
