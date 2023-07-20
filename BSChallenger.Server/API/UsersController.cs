using BSChallenger.Server.Models;
using BSChallenger.Server.Views.API;
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
        public ActionResult<IEnumerable<UserView>> Get()
        {
            return _database.Users.Select(x => (UserView)x).ToList();
        }
    }
}
