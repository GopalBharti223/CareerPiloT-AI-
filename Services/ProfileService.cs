using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;

namespace CareerPilot_AI.Services
{
    public class ProfileService
    {
        private readonly CareerPilotAIDbContext _context;

        public ProfileService(CareerPilotAIDbContext context)
        {
            _context = context;
        }

        public ProfileResponseDTO GetProfile(int userId)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return null;
            }

            ProfileResponseDTO userDto = new ProfileResponseDTO();

            userDto.UserName = user.UserName;
            userDto.Email = user.Email;
            userDto.MobileNumber = user.MobileNumber;
            userDto.CreatedAt = user.CreatedAt;

            return userDto;
        }

        public string UpdateProfile(
            int userId,
            UpdateProfileDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return "User not found";
            }

            var existingUser = _context.Users
                .FirstOrDefault(u =>
                    u.Email == dto.Email &&
                    u.UserId != userId);

            if (existingUser != null)
            {
                return "Email already registered";
            }

            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.MobileNumber = dto.MobileNumber;

            _context.SaveChanges();

            return "Profile updated successfully";
        }
    }
}