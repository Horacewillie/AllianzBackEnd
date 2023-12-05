using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Domain.Helpers;
using AllianzBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Core.Managers
{
    public class UserManager
    {
        private readonly IUserRepository _userRepository;
        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<User> RegisterUser(CreateUserRequest request)
        {
            //check if user already exists
            //if yes, throw error, tell user to sign in
            //otherwise add user to database and save changes
            var existingUser = await _userRepository.FindUserByEmailAddress(request.Email);
            if (existingUser is not null) throw new Exception("Please proceed to sign in");

            var newUser = new User(request);

            (string passwordHash, string salt) = HashPassword(request.Password);

            newUser.Password = passwordHash;
            newUser.Salt = salt;

            var createdUser = _userRepository.Add(newUser);

            await _userRepository.SaveChanges();

            return createdUser;
        }

        public async Task<User> Login(LoginRequest request)
        {
            //check if user already exists
            //if yes, throw error, tell user to sign in
            //otherwise add user to database and save changes
            var existingUser = await _userRepository.FindUserByEmailAddress(request.Email);
            if (existingUser is null) throw new Exception("Please proceed to register");

            var isPasswordValid = VerifyPassword(request.Password, existingUser.Password, existingUser.Salt);
            if (!isPasswordValid) throw new Exception("Password is incorrect");
           
            return existingUser;
        }




        public static (string Hash, string Salt) HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(salt));
        }


        private static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);

                // Compare the computed hash with the stored hash
                for (int i = 0; i < 20; i++)
                {
                    if (hash[i] != Convert.FromBase64String(storedHash)[i + 16])
                    {
                        return false; // Passwords don't match
                    }
                }
                return true; // Passwords match
            }
        }


    }
}
