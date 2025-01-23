using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            var passwordHasher = new PasswordHasher<object>();
            var user = _context.Users.FirstOrDefault(u => u.UserName == userLoginDto.UserName);
            //var user = _context.Users.First(u => u.UserName == userLoginDto.UserName);
            var verificatonResult = passwordHasher.VerifyHashedPassword(null, user.Password, userLoginDto.Password);

            // Mengecek jika user tidak ada atau password salah
            if (user == null || verificatonResult == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid username and password");
            }

            return GenerateAccessToken(user.UserName, user.Name);
        }

        public void UserRegistration(UserRegisterDto userRegisterDto)
        {
            var passwordHasher = new PasswordHasher<object>();

            var newUser = new User
            {
                Name = userRegisterDto.Name,
                UserName = userRegisterDto.UserName,
                Password = passwordHasher.HashPassword(null, userRegisterDto.Password)
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public bool UserExistByUsername(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        private string GenerateAccessToken(string username, string name)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("name", name)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(12), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private string HashPassword(string password) 
        //{
        //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

        //    string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password!,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 100000,
        //        numBytesRequested: 256 / 8
        //        ));

        //    return hashedPassword;
        //}

        //private bool VerifyPassword(string passwordAttempt, string hashPassword)
        //{
        //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

        //    string hashPasswordAttempt = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: passwordAttempt!,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 100000,
        //        numBytesRequested: 256 / 8
        //        ));

        //    return hashPasswordAttempt == hashPassword;
        //}
    }
}
