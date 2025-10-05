using System;

namespace QRK3y.Core.Models
{
    public class CredentialSet
    {
        public string SetId { get; set; } = string.Empty;
        public string SetName { get; set; } = string.Empty;
        public CredentialType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public string? EncryptedUsername { get; set; }
        public string? EncryptedPassword { get; set; }
        public string? EncryptedWord { get; set; }

        public CredentialSet()
        {
            SetId = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }

        public CredentialSet(string setName, CredentialType type)
        {
            SetId = Guid.NewGuid().ToString();
            SetName = setName;
            Type = type;
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }

    public enum CredentialType
    {
        UsernamePassword,
        SingleWord
    }
}