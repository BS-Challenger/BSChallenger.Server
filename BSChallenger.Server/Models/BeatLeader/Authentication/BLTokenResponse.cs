namespace BSChallenger.Server.Models.BeatLeader.Authentication
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
