using Microsoft.AspNetCore.Mvc;
using OTP_generator.Services;
using OTP_generator.Models;

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

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<string>>> GetOtp()
        {
            return Ok(await _otpService.GetLastGeneratedOtp());
        }

        [HttpPost("GenerateNew")]
        public async Task<ActionResult<ServiceResponse<string>>> GenerateNewOtp(OtpModel otpModel)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                serviceResponse = await _otpService.GenerateNewOtp(otpModel);
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