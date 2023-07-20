using BSChallenger.Server.Models.API;
using Scrypt;

namespace BSChallenger.Server.Models
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
