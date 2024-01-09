using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Principal;

namespace BSChallenger.Server.Providers
{
	public class AuthBuilder
	{
		private readonly Database _database;

		public AuthBuilder(
			Database database)
		{
			_database = database;
		}

		public User WithUser(string blID)
		{
			return _database.Users.FirstOrDefault(x => x.BeatLeaderId == blID);
		}

		public void WithHTTPUser(HttpContext ctx, Action<User> onFind)
		{
			if (ctx.User.Identity.IsAuthenticated)
			{
				var Identities = ctx.User.Identities;
				var IdIdentity = Identities.SelectMany(x => x.Claims).FirstOrDefault(x => x.Type.Contains("nameidentifier"));
				if (IdIdentity != null)
				{
					onFind(WithUser(IdIdentity.Value));
					return;
				}
			}
			onFind(null);
		}
	}
}
