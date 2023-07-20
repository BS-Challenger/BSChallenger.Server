using Serilog;
using SQLitePCL;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.Server.Models
{
    public class BeatleaderAPI
    {
        private const string BeatleaderEndpoint = "https://api.beatleader.xyz/";

        private readonly ILogger _logger = Log.ForContext<BeatleaderAPI>();
        private readonly HttpClient _httpClient = new HttpClient();
        private SecretProvider _secrets;

		public BeatleaderAPI(SecretProvider secrets)
        {
            _secrets = secrets;
        }

        public async Task<int> GetUserIdentity(string code)
        {
			var content = new StringContent("grant_type=authorization_code&client_id=BSChallengerClientID&client_secret=" + _secrets.clientSecret + "&code=" + code + "&redirect_uri=http://localhost:8080/beatleader-callback", Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(BeatleaderEndpoint + "oauth2/token", content);
            _logger.Information(res.Content.ToString());

			return 0;
        }
	}
}
