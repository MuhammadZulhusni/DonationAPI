namespace DonationAPI.Models
{
    public class DonationRecord
    {
        public int Id { get; set; }              // Unique donation ID
        public int DonorId { get; set; }         // Foreign key to donor
        public decimal Amount { get; set; }      // Donation amount
        public DateTime Date { get; set; }       // Date of donation

        public Donor? Donor { get; set; }        // Navigation property to Donor
    }
}
