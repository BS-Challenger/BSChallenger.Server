using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BSChallenger.Server.Providers
{
	public class UserProvider
	{
		private readonly BeatLeaderApiProvider _beatleaderAPI;
		private readonly Database _database;

		public UserProvider(BeatLeaderApiProvider blProvider, Database database)
		{
			_beatleaderAPI = blProvider;
			_database = database;
		}

		public async Task<User> GetOrCreateUser(string id)
		{
			var user = _database.Users.FirstOrDefault(x => x.BeatLeaderId == id);
			if (user == null)
			{
				user = new User
				{
					BeatLeaderId = id
				};
				var playerInfo = await _beatleaderAPI.GetPlayerInfoAsync(id);
				//Migrate existing socials
				if (playerInfo.Socials != null)
				{
					var discord = playerInfo.Socials.Find(x => x.Service == "Discord");
					if (discord != null)
					{
						user.DiscordId = discord.UserId;
					}
				}

				user.Username = playerInfo.Name;
				user.Avatar = playerInfo.Avatar;
				user.Platform = playerInfo.Platform;
				user.Country = playerInfo.Country;

				await _database.Users.AddAsync(user);
				await _database.SaveChangesAsync();
			}
			return user;
		}
	}
}
