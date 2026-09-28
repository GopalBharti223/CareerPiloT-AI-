using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CareerPilotAIDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly PasswordHasher<User> _passwordHasher =
            new PasswordHasher<User>();

        public AuthController(
            CareerPilotAIDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterRequestDTO request)
        {
            var existingUser = _context.Users
                .FirstOrDefault(u =>
                    u.Email.ToLower() == request.Email.ToLower());

            if (existingUser != null)
            {
                return BadRequest("Email already registered");
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

            return Ok("User Registered Successfully");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequestDTO request)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("Invalid email or password");
            }

            var passwordVerificationResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    request.Password);

            if (passwordVerificationResult ==
                PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid email or password");
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

            return Ok(new
            {
                Token = tokenString,
                Message = "User Logged In Successfully"
            });
        }
    }
}

