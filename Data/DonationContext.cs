using Microsoft.EntityFrameworkCore;      
using DonationAPI.Models;                

namespace DonationAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<DonationRecord> Donations { get; set; }  // Table for donations
        public DbSet<Donor> Donors { get; set; }              // Table for donors
    }
}
