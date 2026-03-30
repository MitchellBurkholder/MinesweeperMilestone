using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace MinesweeperMilestone.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

        // needed to grab password off of input.
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public string Groups { get; set; }

        public void SetPassword(string password)
        {
            
            Salt = RandomNumberGenerator.GetBytes(16);
            PasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password, Salt, KeyDerivationPrf.HMACSHA256, 100000, 32));
        }

        public bool VerifyPassword(string password)
        {
            
            if (password != null && PasswordHash == Convert.ToBase64String(KeyDerivation.Pbkdf2(password, Salt, KeyDerivationPrf.HMACSHA256, 100000, 32)))
            {
                return true;
            }

            return false;
        }

    }
}
