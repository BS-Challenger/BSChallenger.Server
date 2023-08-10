using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BSChallenger.Server.Models.API
{
    [PrimaryKey("Id")]
    public class Token
    {
        public Token(User user, DateTime expiry, TokenType type)
        {
            UserId = user.Id;
            TokenType = type;
            ExpiryTime = expiry;
            TokenValue = type == TokenType.BLAuthToken ?
                    GenerateAuthToken() :
                    Guid.NewGuid().ToString().Replace("-", string.Empty).Replace("+", string.Empty);
            Id = Guid.NewGuid();
        }

        public Token()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string TokenValue { get; set; }
        public DateTime ExpiryTime { get; set; }
        public TokenType TokenType { get; private set; }
        public int UserId { get; set; }
        public User User { get; set; }

        private static string GenerateAuthToken()
        {
            byte[] buffer = new byte[12];
            Random.Shared.NextBytes(buffer);
            return Convert.ToBase64String(buffer).Substring(0, 12);
        }
    }

    public enum TokenType
    {
        RefreshToken,
        AccessToken,
        BLAuthToken
    }
}
