using BSChallenger.Server.Models.API;
using Scrypt;

namespace BSChallenger.Server.Services
{
	public class PasswordService
	{
		private static ScryptEncoder encoder = new ScryptEncoder();
		public static string CreateHash(string password)
		{
			return encoder.Encode(password);
		}

		public static bool Verify(string password, User user)
		{
			return encoder.Compare(password, user.PasswordHash);
		}
	}
}
