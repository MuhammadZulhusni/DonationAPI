using DonationAPI.Data;                
using DonationAPI.Models;             
using Microsoft.AspNetCore.Mvc;        
using Microsoft.EntityFrameworkCore;   

namespace DonationAPI.Controllers
{
    [Route("api/[controller]")]         // Define base route as api/donor
    [ApiController]                    // Mark class as an API controller
    public class DonorController : ControllerBase
    {
        private readonly AppDbContext _context;   // Database context

        public DonorController(AppDbContext context)
        {
            _context = context;                   // Inject database context
        }

        // GET: api/donor
        [HttpGet]
        public async Task<IActionResult> GetAllDonors()
        {
            // Retrieve all donors with their donation details
            var donors = await _context.Donors
                .Include(d => d.Donations)
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.Email,
                    d.Phone,
                    TotalDonations = d.Donations != null ? d.Donations.Count : 0,
                    TotalAmount = d.Donations != null ? d.Donations.Sum(x => x.Amount) : 0,
                    Donations = d.Donations != null
                        ? d.Donations.Select(x => new { x.Id, x.Amount, x.Date }).Cast<object>()
                        : Enumerable.Empty<object>()
                })
                .ToListAsync();

            return Ok(donors);                     // Return list of donors
        }

        // GET: api/donor/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonorById(int id)
        {
            // Retrieve specific donor by ID with donation info
            var donor = await _context.Donors
                .Include(d => d.Donations)
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.Email,
                    d.Phone,
                    TotalDonations = d.Donations != null ? d.Donations.Count : 0,
                    TotalAmount = d.Donations != null ? d.Donations.Sum(x => x.Amount) : 0,
                    Donations = d.Donations != null
                        ? d.Donations.Select(x => new { x.Id, x.Amount, x.Date }).Cast<object>()
                        : Enumerable.Empty<object>()
                })
                .FirstOrDefaultAsync();

            if (donor == null)
                return NotFound();                 // Return 404 if donor not found

            return Ok(donor);                      // Return donor data
        }

        // POST: api/donor
        [HttpPost]
        public async Task<IActionResult> AddDonor([FromBody] Donor donor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);     // Validate request body

            _context.Donors.Add(donor);            // Add new donor to database
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonorById), new { id = donor.Id }, donor); // Return created donor
        }

        // PUT: api/donor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonor(int id, [FromBody] Donor donor)
        {
            if (id != donor.Id)
                return BadRequest();               // Check ID match

            _context.Entry(donor).State = EntityState.Modified; // Update donor info
            await _context.SaveChangesAsync();
            return NoContent();                    // Return 204 No Content
        }

        // DELETE: api/donor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            var donor = await _context.Donors.FindAsync(id);
            if (donor == null)
                return NotFound();                 // Return 404 if donor not found

            _context.Donors.Remove(donor);         // Remove donor from database
            await _context.SaveChangesAsync();
            return NoContent();                    // Return 204 No Content
        }
    }
}
