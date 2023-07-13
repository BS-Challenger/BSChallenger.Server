using Serilog;
using System.Net.Http;

namespace BSChallenger.Server.Models
{
    public class BeatleaderAPI
    {
        private const string BeatleaderEndpoint = "https://api.beatleader.xyz/";

        private readonly ILogger _logger = Log.ForContext<BeatleaderAPI>();
        private readonly HttpClient _httpClient = new HttpClient();
        private Secrets _secrets;

		public BeatleaderAPI(Secrets secrets)
        {
            _secrets = secrets;
        }

		// TODO: figure out beatleader api, need scores and oauth2

        public int GetUserIdentity()
        {

            return 0;
        }
	}
}
