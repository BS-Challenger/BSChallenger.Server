using Newtonsoft.Json;
using System;
using System.IO;

namespace BSChallenger.Server.Models
{
	public class SecretProvider
	{
		public string SecretPath => Path.Combine(Environment.CurrentDirectory, "secrets");
		public SecretProvider()
		{
			if (File.Exists(SecretPath))
			{
				Secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText(SecretPath));
			}
			else
			{
				Secrets = new Secrets();
				Save();
			}
		}

		public void Save()
		{
			File.WriteAllText(SecretPath, JsonConvert.SerializeObject(Secrets));
		}
		public Secrets Secrets { get; set; }
	}

	public class Secrets
	{
		public string BLclientSecret { get; set; }
		public string DiscordBotToken { get; set; }
	}
}