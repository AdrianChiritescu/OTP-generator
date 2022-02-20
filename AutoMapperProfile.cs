using AutoMapper;
using OTP_generator.Models;
using OTP_generator.DTOs.OTP;

namespace OTP_generator
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddOtpDto, OtpModel>();
        }
    }
}