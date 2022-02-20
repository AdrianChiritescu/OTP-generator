using OTP_generator.Models;

namespace OTP_generator.Services
{
    public interface IOtpService
    {
        Task<ServiceResponse<string>> GetLastGeneratedOtp();
        Task<ServiceResponse<string>> GenerateNewOtp(OtpModel otpModel);
    }
}