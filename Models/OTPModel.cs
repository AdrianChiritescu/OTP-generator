namespace OTP_generator.Models
{
    public class OtpModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int ExpiresIn { get; set; } = 3 * 10; // Seconds
        public string? OTP { get; set; }
    }
}