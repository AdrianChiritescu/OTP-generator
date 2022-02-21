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

        public async Task<ServiceResponse<GetOtpDto>> GetCurrentOtp(string userId)
        {
            var userOtp = RetrieveOtpForUser(userId);

            if(userOtp == null || (userOtp != null && IsExpired(userOtp)))
            {
                return new ServiceResponse<GetOtpDto>
                {
                    Success = false,
                    Message = "There is no valid password. Please generate one."
                };
            }

            var getOtpDto = _mapper.Map<GetOtpDto>(userOtp);

            return new ServiceResponse<GetOtpDto>
            {
                Data = getOtpDto,
                Message = "Password retrived successfully."
            };
        }

        public async Task<ServiceResponse<GetOtpDto>> AddNewOtp(AddOtpDto addOtpDto)
        {
            var userOtp = RetrieveOtpForUser(addOtpDto.UserId);

            if (userOtp != null && !IsExpired(userOtp)) 
            {                 
                return new ServiceResponse<GetOtpDto>
                { 
                    Success = false,
                    Message = "Cannot generate new OTP yet."
                }; 
            }

            var OTP = GenerateOtp();

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

        private string GenerateOtp()
        {
            var random = new Random();
            var otp = Enumerable.Repeat("0123456789", 6)
                                .Select(diggits => diggits[random.Next(diggits.Length)])
                                .ToArray();

            return new string(otp);
        }

        private bool IsExpired(OtpModel OtpModel)
        {
            var elapsedTime = (DateTime.Now - OtpModel.Timestamp).TotalSeconds;

            return elapsedTime >= 30;
        }

        private OtpModel RetrieveOtpForUser(string userId)
        {
            return _dataContext.OTPs.Where(x => x.UserId == userId)
                                    .OrderByDescending(x => x.Id)
                                    .FirstOrDefault();
        }
    }
}