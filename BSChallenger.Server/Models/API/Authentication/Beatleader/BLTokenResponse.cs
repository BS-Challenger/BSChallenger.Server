namespace BSChallenger.Server.Models.API.Authentication.Beatleader
{
	public class BLTokenResponse
	{
		public BLTokenResponse(string token)
		{
			this.token = token;
		}

		public string token { get; set; }
	}
}
