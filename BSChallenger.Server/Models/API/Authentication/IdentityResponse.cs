namespace BSChallenger.Server.Models.API.Authentication
{
    public class IdentityResponse
    {
        public IdentityResponse(string Id, string username)
        {
            ID = Id;
            Username = username;
        }

        public string ID { get; set; }
        public string Username { get; set; }
    }
}
