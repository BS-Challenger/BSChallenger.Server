using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Users;
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

        public UsersController(
            Database database)
        {
            _database = database;
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
                var Identities = HttpContext.User.Identities;
                var IdIdentity = Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type.Contains("nameidentifier"));
                if (IdIdentity != null)
                {
                    return _database.EagerLoadUsers().FirstOrDefault(x => x.BeatLeaderId == IdIdentity.Value);
				}
                else
                {
                    return NotFound("No user for this token");
                }
            }
            else
            {
                return _database.EagerLoadUsers().FirstOrDefault(x => x.BeatLeaderId == id);
            }
		}
	}
}
