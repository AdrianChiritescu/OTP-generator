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

        public async Task<ServiceResponse<GetOtpDto>> AddNewOtp(AddOtpDto addOtpDto)
        {
            var OTP = GenerateOtp();

            if(OTP == null)
            {
                return new ServiceResponse<GetOtpDto>
                { 
                    Success = false,
                    Message = "Cannot generate new OTP yet."
                };
            }
            
            addOtpDto.OTP = OTP;
            addOtpDto.ExpiresIn = 30; // Seconds
            addOtpDto.Timestamp = DateTime.Now;

            _dataContext.OTPs.Add(_mapper.Map<OtpModel>(addOtpDto));
            await _dataContext.SaveChangesAsync();

            return new ServiceResponse<GetOtpDto>
            {
                Data = new GetOtpDto
                {
                    OTP = OTP,
                    SecondsToExpire = 30 // Seconds
                },
                Message = "Password generated successfully."
            };
        }

        private string? GenerateOtp()
        {
            if (!CanGenerate()) { return null; }

            var random = new Random();
            var otp = Enumerable.Repeat("0123456789", 6)
                                .Select(diggits => diggits[random.Next(diggits.Length)])
                                .ToArray();

            return new string(otp);
        }

        private bool CanGenerate()
        {
            var lastOtpEntry = _dataContext.OTPs.OrderByDescending(x => x.Id)
                            .FirstOrDefault();

            if (lastOtpEntry == null) { return true; }

            var elapsedTime = (DateTime.Now - lastOtpEntry.Timestamp).TotalSeconds;

            return elapsedTime >= 30;
        }
    }
}