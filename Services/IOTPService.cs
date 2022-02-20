using OTP_generator.Models;

namespace OTP_generator.Services
{
    public interface IOTPService
    {
        Task<ServiceResponse<string>> GetLastGeneratedOTP();
        Task<ServiceResponse<string>> GenerateNewOTP(OTPModel otpModel);
    }
}