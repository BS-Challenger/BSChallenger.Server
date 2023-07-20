using System;
using System.IO;

namespace BSChallenger.Server.Models
{
	public class SecretProvider
	{
		public SecretProvider()
		{
			clientSecret = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "blsecret"));
		}
		public string clientSecret { get; set; }
	}
}
