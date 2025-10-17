namespace DonationAPI.Models
{
    public class Donor
    {
        public int Id { get; set; }                             // Unique donor ID
        public string? Name { get; set; }                       // Donor name
        public string? Email { get; set; }                      // Donor email
        public string? Phone { get; set; }                      // Donor phone number

        public List<DonationRecord>? Donations { get; set; }    // List of donations made by donor
    }
}
