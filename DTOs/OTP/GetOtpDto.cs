namespace OTP_generator.DTOs.OTP
{
    public class GetOtpDto
    {
        public string OTP { get; set; } = String.Empty;
        public int ExpiresIn { get; set; } // Seconds
    }
}