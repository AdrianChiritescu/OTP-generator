namespace OTP_generator.Models
{
    public class OTPModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime Date { get; set; }  = DateTime.Now;
    }
}