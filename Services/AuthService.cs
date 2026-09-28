using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareerPilot_AI.Services
{
    public class AuthService
    {
        private readonly CareerPilotAIDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly PasswordHasher<User> _passwordHasher =
            new PasswordHasher<User>();

        public AuthService(
            CareerPilotAIDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Register(RegisterRequestDTO request)
        {
            var existingUser = _context.Users
                .FirstOrDefault(u =>
                    u.Email.ToLower() == request.Email.ToLower());

            if (existingUser != null)
            {
                return "Email already registered";
            }

            User user = new User();

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.MobileNumber = request.MobileNumber;

            user.Password = _passwordHasher.HashPassword(
                user,
                request.Password);

            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            _context.SaveChanges();

            return "User Registered Successfully";
        }

        public string Login(LoginRequestDTO request)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return "Invalid email or password";
            }

            var passwordVerificationResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    request.Password);

            if (passwordVerificationResult ==
                PasswordVerificationResult.Failed)
            {
                return "Invalid email or password";
            }

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName),

                new Claim(
                    ClaimTypes.Email,
                    user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            var tokenString =
                new JwtSecurityTokenHandler()
                    .WriteToken(token);

            return tokenString;
        }
    }
}