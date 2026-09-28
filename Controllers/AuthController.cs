using CareerPilot_AI.DTOs;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterRequestDTO request)
        {
            string result = _authService.Register(request);

            if (result == "Email already registered")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequestDTO request)
        {
            string token = _authService.Login(request);

            if (token == "Invalid email or password")
            {
                return BadRequest(token);
            }

            return Ok(new
            {
                Token = token,
                Message = "User Logged In Successfully"
            });
        }
    }
}

