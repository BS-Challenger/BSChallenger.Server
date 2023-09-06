using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Users;
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
        public ActionResult<IEnumerable<User>> Get()
        {
            return _database.Users.Select(x => x).ToList();
        }
    }
}
