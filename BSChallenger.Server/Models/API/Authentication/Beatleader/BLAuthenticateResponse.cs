namespace BSChallenger.Server.Models.API.Authentication.Beatleader
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
