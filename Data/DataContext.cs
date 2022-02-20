using Microsoft.EntityFrameworkCore;
using OTP_generator.Models;

namespace OTP_generator.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<OtpModel> OTPs { get; set; }
    }
}