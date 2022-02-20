using Microsoft.AspNetCore.Mvc;
using OTP_generator.Services;
using OTP_generator.Models;

namespace OTP_generator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OTPController : ControllerBase
    {
        private readonly IOTPService _otpService;

        public OTPController(IOTPService otpService)
        {
            _otpService = otpService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<string>>> GetOTP()
        {
            return Ok(await _otpService.GetLastGeneratedOTP());
        }

        [HttpPost("GenerateNew")]
        public async Task<ActionResult<ServiceResponse<string>>> GenerateNewOTP(OTPModel otpModel)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                serviceResponse = await _otpService.GenerateNewOTP(otpModel);
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