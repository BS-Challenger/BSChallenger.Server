using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Views.API
{
	public class UserView
	{
		public UserView(string userID)
		{
			UserId = userID;
		}
		public string UserId { get; set; }

		public static implicit operator UserView(Models.API.User usr)
		{
			return new UserView(usr.UserId);
		}
	}
}
