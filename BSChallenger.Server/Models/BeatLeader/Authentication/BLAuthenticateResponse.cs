namespace BSChallenger.Server.Models.BeatLeader.Authentication
{
    public class BLAuthenticateResponse
    {
        public BLAuthenticateResponse(string result)
        {
            this.result = result;
        }

        public string result { get; set; }
    }
}
