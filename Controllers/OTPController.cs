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
        public async Task<ActionResult<ServiceResponse<string>>> GetOtp(string userId)
        {
            return Ok(await _otpService.GetCurrentOtp(userId));
        }

        [HttpPost("GenerateNew")]
        public async Task<ActionResult<ServiceResponse<GetOtpDto>>> GenerateNewOtp(AddOtpDto addOtpDto)
        {
            var serviceResponse = new ServiceResponse<GetOtpDto>();

            try
            {
                serviceResponse = await _otpService.AddNewOtp(addOtpDto);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return Ok(serviceResponse);
        }
    }
}