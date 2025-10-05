using System;
using System.Collections.Generic;

namespace QRK3y.Core.Models
{
    public class User
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }
        public List<CredentialSet> CredentialSets { get; set; } = new List<CredentialSet>();

        public User()
        {
            UserId = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            LastLogin = DateTime.Now;
            CredentialSets = new List<CredentialSet>();
        }

        public User(string username, string passwordHash)
        {
            UserId = Guid.NewGuid().ToString();
            Username = username;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.Now;
            LastLogin = DateTime.Now;
            CredentialSets = new List<CredentialSet>();
        }
    }
}