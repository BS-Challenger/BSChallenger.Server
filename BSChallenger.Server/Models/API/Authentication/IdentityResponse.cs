namespace BSChallenger.Server.Models.API.Authentication
{
	public class IdentityResponse
	{
		public IdentityResponse(int Id, string username)
		{
			ID = Id;
			Username = username;
		}

		public int ID { get; set; }
		public string Username { get; set; }
	}
}
