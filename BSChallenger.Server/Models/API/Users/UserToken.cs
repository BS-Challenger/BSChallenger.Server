namespace BSChallenger.Server.Models.API.Users
{
	public class UserToken
	{
		public int Exp { get; set; }
		public int Iat { get; set; }
		public string BeatLeaderId { get; set; }
	}
}
