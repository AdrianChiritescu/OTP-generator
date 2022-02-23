using Microsoft.EntityFrameworkCore;
using AutoMapper;
using OTP_generator.Models;
using OTP_generator.DTOs.OTP;
using OTP_generator.Data;

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
            var userOtp = await _dataContext.OTPs.FirstOrDefaultAsync(x => x.UserId == userId);

            if (userOtp == null || (userOtp != null && IsExpired(userOtp)))
            {
                await DeleteOtp(userId);

                return new ServiceResponse<GetOtpDto>
                {
                    Data = null,
                    Success = false,
                    Message = "The password is expired. Please generate a new password."
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
            var serviceResponse = new ServiceResponse<GetOtpDto> { Message = "Password generated successfully." };

            try
            {
                var userOtp = await _dataContext.OTPs.FirstOrDefaultAsync(x => x.UserId == addOtpDto.UserId);

                if (userOtp != null && !IsExpired(userOtp))
                {            
                    serviceResponse.Data = _mapper.Map<GetOtpDto>(userOtp);
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Cannot generate new OTP yet.";

                    return serviceResponse;
                }

                await DeleteOtp(addOtpDto.UserId);

                var OTP = GenerateOtp();

                addOtpDto.OTP = OTP;
                addOtpDto.ExpiresIn = 30; // Seconds
                addOtpDto.Timestamp = DateTime.Now;

                _dataContext.OTPs.Add(_mapper.Map<OtpModel>(addOtpDto));
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = new GetOtpDto
                {
                    OTP = OTP,
                    SecondsToExpire = 30 // Seconds
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetOtpDto>> DeleteOtp(string userId)
        {
            var serviceResponse = new ServiceResponse<GetOtpDto> { Message = "OTP deleted successfully." };

            try
            {
                var OTP = await _dataContext.OTPs.FirstOrDefaultAsync(x => x.UserId == userId);

                if (OTP == null) 
                { 
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No OTP to be deleted.";
                    return serviceResponse;
                }

                _dataContext.OTPs.Remove(OTP);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        private bool IsExpired(OtpModel OtpModel)
        {
            var elapsedTime = (DateTime.Now - OtpModel.Timestamp).TotalSeconds;

            return elapsedTime >= 30;
        }

        private string GenerateOtp()
        {
            var random = new Random();
            var otp = Enumerable.Repeat("0123456789", 6)
                                .Select(diggits => diggits[random.Next(diggits.Length)])
                                .ToArray();

            return new string(otp);
        }
    }
}