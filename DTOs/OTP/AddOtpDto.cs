namespace OTP_generator.DTOs.OTP
{
    public class AddOtpDto
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}