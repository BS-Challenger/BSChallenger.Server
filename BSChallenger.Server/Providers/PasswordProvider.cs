using BSChallenger.Server.Models.API.Users;
using Scrypt;

namespace BSChallenger.Server.Providers
{
    //TODO: Move to Argon2 or BCrypt
    public class PasswordProvider
    {
        private readonly ScryptEncoder encoder = new();
        public string CreateHash(string password)
        {
            return encoder.Encode(password);
        }

        public bool Verify(string password, User user)
        {
            return encoder.Compare(password, user.PasswordHash);
        }
    }
}
