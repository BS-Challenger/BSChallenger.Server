namespace BSChallenger.Server.Models.API.Scan
{
    public class ScanResponse
    {
        public ScanResponse(string result)
        {
            Result = result;
        }

        public string Result { get; set; }
    }
}
