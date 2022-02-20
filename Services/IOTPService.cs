using OTP_generator.Models;
using OTP_generator.DTOs.OTP;

namespace OTP_generator.Services
{
    public interface IOtpService
    {
        Task<ServiceResponse<GetOtpDto>> GetLastGeneratedOtp();
        Task<ServiceResponse<GetOtpDto>> GenerateNewOtp(AddOtpDto addOtpDto);
    }
}