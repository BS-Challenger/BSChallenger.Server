using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Models.API;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using BSChallenger.Server.Models.API.Authentication.Beatleader;

namespace BSChallenger.Server.API.Authentication.BeatLeader
{
    [ApiController]
    [Route("[controller]")]
    public class BeatLeaderAuthenticateController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<BeatLeaderAuthenticateController>();
        private readonly Database _database;
        private readonly BeatleaderAPI _beatleaderAPI;
        private readonly TokenProvider _tokenProvider;

        public BeatLeaderAuthenticateController(
            Database database,
            BeatleaderAPI beatleaderAPI,
            TokenProvider tokenProvider)
        {
            _database = database;
            _beatleaderAPI = beatleaderAPI;
            _tokenProvider = tokenProvider;
        }

        [HttpPost("GetBLToken")]
        public async Task<ActionResult<BLTokenResponse>> PostGetToken(AuthenticatedRequest request)
        {
            _logger.Information(request.AccessToken);
            var token = _database.Tokens.FirstOrDefault(x => x.TokenValue == request.AccessToken && x.TokenType == TokenType.AccessToken);

            if (token != null)
            {
                var user = token.User;
                if (user != null)
                {
                    return new BLTokenResponse((await _tokenProvider.GetBLAuthToken(user)).TokenValue);
                }
            }
            return new BLTokenResponse("Failed");
        }

        [HttpPost("BLAuthenticate")]
        public async Task<ActionResult<BLAuthenticateResponse>> PostAuthenticate(BLAuthenticateRequest request)
        {
            _logger.Information(request.BLCode);
            _logger.Information(request.GeneratedCode);
            if (request.BLCode is null)
                throw new HttpResponseException(400);
            var blAuthToken = _database.Tokens.AsEnumerable().FirstOrDefault(x => x.TokenValue == request.GeneratedCode);

            if (blAuthToken?.TokenType == TokenType.BLAuthToken)
            {
                var user = blAuthToken.User;
                if (user != null)
                {
                    var identity = await _beatleaderAPI.GetBLUserIdentity(request.BLCode);
                    user.BeatLeaderId = identity.ToString();
                    await _database.SaveChangesAsync();
                }
            }
            return new BLAuthenticateResponse("Failed");
        }
    }
}
