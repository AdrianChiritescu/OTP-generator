namespace OTP_generator.DTOs.OTP
{
    public class AddOtpDto
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int ExpiresIn { get; set; }
        public string? OTP { get; set; }
    }
}