using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Users;
using BSChallenger.Server.Providers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.API
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly Database _database;
        private readonly AuthBuilder _authBuilder;

		public UsersController(
            Database database,
            AuthBuilder authBuilder)
        {
            _database = database;
			_authBuilder = authBuilder;

		}

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return _database.Users.Select(x => x).ToList();
        }

        [HttpGet("{id}")]
		[EnableCors(PolicyName = "website")]
		public ActionResult<User> GetUser(string id)
		{
            if (HttpContext.User.Identity.IsAuthenticated && id == "-1")
            {
				User ret = null;
				_authBuilder.WithHTTPUser(HttpContext, (user) =>
				{
					ret = user;
				});
				return ret != null ? Ok(ret) : NotFound("No user for this token");
			}
            else
            {
                return _authBuilder.WithUser(id);
            }
		}
	}
}
