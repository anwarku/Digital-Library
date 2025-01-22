using System.Security.Cryptography;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.Services
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserHasLoggedDto GetUserById(Guid id)
        {
            var user = _context.Users.Select(u => new UserHasLoggedDto()
            {
                Id = u.Id,
                Name = u.Name,
                UserName = u.UserName
            }).FirstOrDefault();

            return user;
        }

        // Fungsi ini mengembalikan akses token JWT
        public string UserLogin(UserLoginDto userLoginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userLoginDto.UserName);

            // Mengecek jika user tidak ada atau password salah
            if (user == null || !VerifyPassword(user.Password, userLoginDto.Password))
            {
                throw new Exception("Invalid username and password");
            }

            return user.UserName;
        }

        public void UserRegistration(UserRegisterDto userRegisterDto)
        {
            var newUser = new User
            {
                Name = userRegisterDto.Name,
                UserName = userRegisterDto.UserName,
                Password = HashPassword(userRegisterDto.Password)
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public bool UserExistByUsername(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        //private string GenerateAccessToken()
        //{

        //}

        private string HashPassword(string password) 
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                ));

            return hashedPassword;
        }

        private bool VerifyPassword(string passwordAttempt, string hashPassword)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string hashPasswordAttempt = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordAttempt!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                ));

            return hashPasswordAttempt == hashPassword;
        }
    }
}
