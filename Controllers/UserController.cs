using Azure.Messaging;
using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CareerPilotAIDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public UserController(CareerPilotAIDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }


        
        [HttpPost("RegisterRequest")]

        public IActionResult Register(RegisterRequestDTO request)
        {
            User user = new User();
            user.UserName = request.UserName;
            user.Email = request.Email;
            user.MobileNumber = request.MobileNumber;
            user.Password = request.Password;
            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User Registered Successfully");
        }

      
        [HttpPost("login")]
        public IActionResult Login(LoginRequestDTO request)
        {
          

            User user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("Email not found");
            }

            if (user.Password!=request.Password)
            {
                return BadRequest("Not Matched");
            }

            var claims = new[]
             {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
             };

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

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

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Message = "User Logged In Successfully"
            });
        }




        [Authorize]
        [HttpGet("Profile")]
        public IActionResult profile()
        {

            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
             );
            User user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            ProfileResponseDTO UserDto = new ProfileResponseDTO();
            UserDto.UserName = user.UserName;
            UserDto.Email = user.Email;
            UserDto.MobileNumber = user.MobileNumber;
            UserDto.CreatedAt = user.CreatedAt;

           return Ok(UserDto);
        }


        [Authorize]
        [HttpPut("UpdateProfile")]
        public IActionResult UpdateProfile(UpdateProfileDTO dto)
        {
            int findId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User userTable = _context.Users.FirstOrDefault(u => u.UserId == findId);

            if (userTable == null)
            {
                return NotFound("User not found");
            }

            userTable.UserName = dto.UserName;
            userTable.Email = dto.Email;
            userTable.MobileNumber = dto.MobileNumber;

            _context.SaveChanges();

            return Ok("Profile updated successfully");
        }


        //Delete Action pending

        //Change Password

        [Authorize]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDTO dto)
        {
            int findId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User userTable = _context.Users.FirstOrDefault(u => u.UserId == findId);

           

            if (userTable == null)
            {
                return NotFound("User not found");
            }

           if(userTable.Password != dto.CurrentPassword)
            {
                return BadRequest("Old password is incorrect");
            } 
           

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest("New password and confirm password do not match");
            }

            userTable.Password = dto.NewPassword;

            _context.SaveChanges();

            return Ok("Password changed successfully");
        }



        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordDTO dto)
        {

            User user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return BadRequest("Email not found");
            }

            PasswordResetOTP pwdreset = _context.PasswordResetOTPs.FirstOrDefault(p => p.UserId == user.UserId);

           
            
            Random random = new Random();
            int genratedOTP = random.Next(100000, 1000000);

            if (pwdreset != null)
            {
                // Update the existing OTP and expiration time
                pwdreset.OTP = genratedOTP.ToString();
                pwdreset.CreatedAt = DateTime.Now;
                pwdreset.ExpiresAt = DateTime.Now.AddMinutes(5);
                _context.SaveChanges();

                _emailService.SendEmail(
                user.Email,
                "CareerPilot AI - Password Reset OTP",
                $"Your OTP is: {genratedOTP}"
                );

            }

            else
            {
                // Create a new OTP entry
                PasswordResetOTP newPwdReset = new PasswordResetOTP();
                newPwdReset.UserId = user.UserId;
                newPwdReset.OTP = genratedOTP.ToString();
                newPwdReset.CreatedAt = DateTime.Now;
                newPwdReset.ExpiresAt = DateTime.Now.AddMinutes(5);
                _context.PasswordResetOTPs.Add(newPwdReset);
                _context.SaveChanges();

                _emailService.SendEmail(
                    user.Email,
                    "CareerPilot AI - Password Reset OTP",
                    $"Your OTP is: {genratedOTP}"
                );

            }
        
            return Ok("Password reset link sent to your email");

        }


        [HttpPost("VerifyOtp")]

        public IActionResult verifyOtp(VerifyOtpDTO dto)
        {

                    User user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            // Check if the user exists

            if (user == null)
                    {
                        return BadRequest("User not found");
                    }

                    PasswordResetOTP pro = _context.PasswordResetOTPs.FirstOrDefault(p => p.UserId == user.UserId);

            //check if PasswordResetOTP record exists for the user
                    if (pro == null) 
                    {
                        return BadRequest("No OTP found for this user");
                    }
                    //checking if the OTP is expired or not

                   

                    if(DateTime.Now > pro.ExpiresAt)
                    {
                        return BadRequest("OTP has expired");
                    }
                    else if (dto.OTP == pro.OTP)
                        {
                            return Ok("OTP verified successfully");
                        }
                         else
                          {
                              return BadRequest("Invalid OTP");
                          }       

        }

        //Reset Password
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordDTO dto)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            PasswordResetOTP pro = _context.PasswordResetOTPs.FirstOrDefault(p => p.UserId == user.UserId);
            if (pro == null)
            {
                return BadRequest("No OTP found for this user");
            }
            if (DateTime.Now > pro.ExpiresAt)
            {
                return BadRequest("OTP has expired");
            }
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest("New password and confirm password do not match");
            }
            user.Password = dto.NewPassword;
            //_context.SaveChanges();
            // Optionally, you can delete the OTP record after successful password reset
            _context.PasswordResetOTPs.Remove(pro);
            _context.SaveChanges();
            return Ok("Password reset successfully");
        }

    }
}