using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;
using BSChallenger.Server.Models.API.Authentication;
using BSChallenger.Server.Services;
using BSChallenger.Server.Views.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.Server.API.Authentication
{
    [ApiController]
    [Route("/accounts/Signup")]
    public class SignupController : ControllerBase
    {
        private readonly Database _database;
        private readonly TokenProvider _tokenProvider;

        public SignupController(
            Database database,
            TokenProvider tokenProvider)
        {
            _database = database;
            _tokenProvider = tokenProvider;
        }
        [HttpPost]
        public async Task<ActionResult<AuthResponse>> PostSignup(NamePasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Username))
            {
                return new AuthResponse("Username Or Password is empty", false, null);
            }
            if (_database.Users.Any(x => x.Username == request.Username))
            {
                return new AuthResponse("Username Already Exists", false, null);
            }
            var user = new User(request.Username);

            user.PasswordHash = PasswordService.CreateHash(request.Password);

            await _database.Users.AddAsync(user);
            await _database.SaveChangesAsync();
            return new AuthResponse("Success", true, (TokenView)await _tokenProvider.GetRefreshToken(user));
        }
    }
}
