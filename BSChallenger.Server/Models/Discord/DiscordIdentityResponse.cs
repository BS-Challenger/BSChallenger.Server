namespace BSChallenger.Server.Models.Discord
{
    public class DiscordIdentityResponse
    {
		public string Id { get; set; }
		public string Username { get; set; }
		public string Avatar { get; set; }
		public string Discriminator { get; set; }
		public int PublicFlags { get; set; }
		public int PremiumType { get; set; }
		public int Flags { get; set; }
		public object Banner { get; set; }
		public int AccentColor { get; set; }
		public string GlobalName { get; set; }
		public object AvatarDecorationData { get; set; }
		public string BannerColor { get; set; }
		public bool MfaEnabled { get; set; }
		public string Locale { get; set; }
	}
}
