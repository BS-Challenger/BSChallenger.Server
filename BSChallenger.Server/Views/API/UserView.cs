using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Views.API
{
	public class UserView
	{
		public UserView(string userID, bool patron)
		{
			UserId = userID;
			Patron = patron;
		}
		public string UserId { get; set; }
		public bool Patron { get; set; }
		public DateTime LastCheckDate { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime TokenExpiry { get; set; }

		public static implicit operator UserView(Models.API.User usr)
		{
			return new UserView(usr.UserId, usr.Patron);
		}
	}
}
