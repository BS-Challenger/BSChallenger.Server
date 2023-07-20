using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Views.API
{
	public class UserView
	{
		public UserView(string userID, string beatLeaderId)
		{
			UserId = userID;
			BeatLeaderId = beatLeaderId;
		}
		public string UserId { get; set; }
		public string BeatLeaderId { get; set; }

		public static implicit operator UserView(Models.API.User usr)
		{
			return new UserView(usr.Username, usr.BeatLeaderId);
		}
	}
}
