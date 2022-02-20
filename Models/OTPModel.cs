namespace OTP_generator.Models
{
    public class OtpModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int ExpiresIn { get; set; }
        public string? OTP { get; set; }
    }
}