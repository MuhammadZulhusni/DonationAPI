using DonationAPI.Data;                
using DonationAPI.Models;             
using Microsoft.AspNetCore.Mvc;        
using Microsoft.EntityFrameworkCore;  

namespace DonationAPI.Controllers
{
    [Route("api/[controller]")]         // Base route: api/donation
    [ApiController]                    // Mark as API controller
    public class DonationController : ControllerBase
    {
        private readonly AppDbContext _context;   // Database context

        public DonationController(AppDbContext context)
        {
            _context = context;                   // Inject database context
        }

        // GET: api/donation
        [HttpGet]
        public async Task<IActionResult> GetAllDonations()
        {
            // Get all donations with donor details
            var donations = await _context.Donations
                .Include(d => d.Donor)
                .Select(d => new
                {
                    d.Id,
                    d.Amount,
                    d.Date,
                    DonorId = d.DonorId,
                    DonorName = d.Donor != null ? d.Donor.Name : null,
                    DonorEmail = d.Donor != null ? d.Donor.Email : null,
                    DonorPhone = d.Donor != null ? d.Donor.Phone : null
                })
                .ToListAsync();

            return Ok(donations);                  // Return list of donations
        }

        // GET: api/donation/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonationById(int id)
        {
            // Get donation by ID with donor info
            var donation = await _context.Donations
                .Include(d => d.Donor)
                .Select(d => new
                {
                    d.Id,
                    d.Amount,
                    d.Date,
                    DonorId = d.DonorId,
                    DonorName = d.Donor != null ? d.Donor.Name : null,
                    DonorEmail = d.Donor != null ? d.Donor.Email : null,
                    DonorPhone = d.Donor != null ? d.Donor.Phone : null
                })
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();                 // Return 404 if not found

            return Ok(donation);                   // Return donation details
        }

        // POST: api/donation
        [HttpPost]
        public async Task<IActionResult> AddDonation([FromBody] DonationRecord donation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);     // Validate request data

            _context.Donations.Add(donation);      // Add new donation
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonationById), new { id = donation.Id }, donation); // Return created donation
        }

        // PUT: api/donation/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonation(int id, [FromBody] DonationRecord donation)
        {
            if (id != donation.Id)
                return BadRequest();               // Check ID match

            _context.Entry(donation).State = EntityState.Modified; // Update existing record
            await _context.SaveChangesAsync();

            return NoContent();                    // Return 204 No Content
        }

        // DELETE: api/donation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();                 // Return 404 if not found

            _context.Donations.Remove(donation);   // Delete donation
            await _context.SaveChangesAsync();

            return NoContent();                    // Return 204 No Content
        }
    }
}
