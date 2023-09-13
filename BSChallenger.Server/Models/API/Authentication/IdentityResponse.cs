namespace BSChallenger.Server.Models.API.Authentication
{
    public class IdentityResponse
    {
        public IdentityResponse(string Id, string username, string avatar)
        {
            ID = Id;
            Username = username;
            Avatar = avatar;
		}

        public string ID { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
	}
}
