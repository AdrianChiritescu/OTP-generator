using OTP_generator.Models;
using OTP_generator.DTOs.OTP;

namespace OTP_generator.Services
{
    public class OtpService : IOtpService
    {
        public async Task<ServiceResponse<GetOtpDto>> GetLastGeneratedOtp()
        {
            return new ServiceResponse<GetOtpDto>();
        }

        public async Task<ServiceResponse<GetOtpDto>> GenerateNewOtp(AddOtpDto addOtpDto)
        {
            var generatedOtp = Enumerable.Repeat("0123456789", 6)
                                .Select(diggitsString => diggitsString[new Random().Next(diggitsString.Length)])
                                .ToArray();

            return new ServiceResponse<GetOtpDto>
            {
                Data = new GetOtpDto
                {
                    OTP = new string(generatedOtp),
                    SecondsToExpire = 3 * 10 // Seconds
                }
            };
        }
    }
}