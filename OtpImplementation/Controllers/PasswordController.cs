using BLL;
using Microsoft.AspNetCore.Mvc;

namespace OtpImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly UserService _userService;

        public PasswordController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("sendotp")]
        public IActionResult SendOTP([FromBody] SendOTPRequest request)
        {
            try
            {
                _userService.SendOTP(request.Email);
                return Ok("OTP sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send OTP: {ex.Message}");
            }
        }

        public class SendOTPRequest
        {
            public string Email { get; set; }
        }

        [HttpPost("verifyotp")]
        public IActionResult VerifyOTP([FromBody] VerifyOTPRequest request)
        {
            try
            {
                bool isVerified = _userService.VerifyOTP(request.Email, request.OTP);
                return Ok(new { isVerified });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to verify OTP: {ex.Message}");
            }
        }

        [HttpPost("updatepassword")]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            try
            {
                _userService.UpdatePassword(request.Email, request.NewPassword);
                return Ok("Password updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update password: {ex.Message}");
            }
        }
    }

    public class VerifyOTPRequest
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }

    public class UpdatePasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }

    }
}
