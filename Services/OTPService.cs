using OTP_generator.Models;
using OTP_generator.DTOs.OTP;
using OTP_generator.Data;
using AutoMapper;

namespace OTP_generator.Services
{
    public class OtpService : IOtpService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public OtpService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetOtpDto>> GetCurrentOtp()
        {
            return new ServiceResponse<GetOtpDto>();
        }

        public async Task<ServiceResponse<GetOtpDto>> AddNewOtp(AddOtpDto newOtp)
        {
            if (true) // TODO: Check if allowed to generate a new OTP.
            {

            }

            var otp = GenerateOtp();
            newOtp.OTP = otp;
            newOtp.ExpiresIn = 30; // Seconds.

            _dataContext.OTPs.Add(_mapper.Map<OtpModel>(newOtp));
            await _dataContext.SaveChangesAsync();

            return new ServiceResponse<GetOtpDto>
            {
                Data = new GetOtpDto
                {
                    OTP = new string(otp),
                    SecondsToExpire = 3 * 10 // Seconds
                }
            };
        }

        private string GenerateOtp()
        {
            var random = new Random();
            var otp = Enumerable.Repeat("123456789", 6)
                                .Select(diggits => diggits[random.Next(diggits.Length)])
                                .ToArray();

            return new string(otp);
        }
    }
}