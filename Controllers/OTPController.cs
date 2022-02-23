using Microsoft.AspNetCore.Mvc;
using OTP_generator.Services;
using OTP_generator.Models;
using OTP_generator.DTOs.OTP;

namespace OTP_generator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ServiceResponse<GetOtpDto>>> GetOtp(string userId)
        {
            var response = await _otpService.GetCurrentOtp(userId);

            if (response.Data == null) { return Ok(response); }

            return Ok(response);
        }

        [HttpPost("GenerateNew")]
        public async Task<ActionResult<ServiceResponse<GetOtpDto>>> GenerateNewOtp(AddOtpDto addOtpDto)
        {
            var response = await _otpService.AddNewOtp(addOtpDto);

            return Ok(response);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<ServiceResponse<GetOtpDto>>> DeleteOtp(string userId)
        {
            var response = await _otpService.DeleteOtp(userId);

            return Ok(response);
        }
    }
}