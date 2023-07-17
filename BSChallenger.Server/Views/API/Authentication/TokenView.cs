using BSChallenger.Server.Models.API;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Views.API
{
	public class TokenView
	{
		public TokenView(string token, string type)
		{
			Token = token;
			Type = type;
		}
		public string Token { get; set; }
		public string Type { get; set; }

		public static implicit operator TokenView(Models.API.Token tkn)
		{
			return new TokenView(tkn.token, tkn.isAccessToken ? "Access" : "Refresh");
		}
	}
}
