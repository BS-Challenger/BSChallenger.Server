using Newtonsoft.Json;
using System;
using System.IO;

namespace BSChallenger.Server.Providers
{
    //I don't care
    //IConfiguration made me want to rip my hair out
    public class SecretProvider
    {
        public string SecretPath => Path.Combine(Environment.CurrentDirectory, "secrets.json");
        public SecretProvider()
        {
            Console.WriteLine(Environment.CurrentDirectory);
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
        public string BLclientSecret { get; set; } = "";
        public string DiscordBotToken { get; set; } = "";
        public string DiscordOauthSecret { get; set; } = "";
        public string DiscordOauthClientId { get; set; } = "";
		public DBInfo Database { get; set; } = new DBInfo();
        public JwtInfo Jwt { get; set; } = new JwtInfo();
    }

    public class DBInfo
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string DatabaseName { get; set; } = "";
        public string Host { get; set; } = "";
    }

    public class JwtInfo
    {
        public string Key { get; set; } = "";
    }
}