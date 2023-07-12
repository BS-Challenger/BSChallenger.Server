using Serilog;
using System.Net.Http;

namespace BSChallenger.Server.Models
{
    public class BeatleaderAPI
    {
        private const string BeatleaderEndpoint = "https://api.beatleader.xyz/";

        private readonly ILogger _logger = Log.ForContext<BeatleaderAPI>();
        private static readonly HttpClient _httpClient = new HttpClient();

        // TODO: figure out beatleader api, need scores and oauth2
    }
}
