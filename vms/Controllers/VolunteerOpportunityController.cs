using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vms.Data;
using vms.Models;

namespace vms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerOpportunityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VolunteerOpportunityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new volunteer opportunity
        [HttpPost]
        public async Task<ActionResult<VolunteerOpportunity>> CreateOpportunity([FromBody] VolunteerOpportunity opportunity)
        {
            _context.VolunteerOpportunities.Add(opportunity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOpportunity), new { id = opportunity.Id }, opportunity);
        }

        // Get all volunteer opportunities
        [HttpGet]
        public async Task<ActionResult<List<VolunteerOpportunity>>> GetOpportunities()
        {
            return await _context.VolunteerOpportunities.ToListAsync();
        }

        // Get a specific opportunity by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VolunteerOpportunity>> GetOpportunity(int id)
        {
            var opportunity = await _context.VolunteerOpportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }
            return Ok(opportunity);
        }

        // Update a volunteer opportunity
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpportunity(int id, [FromBody] VolunteerOpportunity opportunity)
        {
            if (id != opportunity.Id)
            {
                return BadRequest();
            }

            _context.Entry(opportunity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.VolunteerOpportunities.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // Delete a volunteer opportunity
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpportunity(int id)
        {
            var opportunity = await _context.VolunteerOpportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }

            _context.VolunteerOpportunities.Remove(opportunity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("organization/{organizationId}")]
        public async Task<ActionResult<List<int>>> GetOpportunitiesByOrganization(int organizationId)
        {
            var opportunityIds = await _context.VolunteerOpportunities
                .Where(o => o.OrganizationId == organizationId)
                .Select(o => o.Id)
                .ToListAsync();

            if (opportunityIds == null || !opportunityIds.Any())
            {
                return NotFound($"No opportunities found for organizationId: {organizationId}");
            }

            return opportunityIds;
        }
        [HttpGet("name/{id}")]
        public async Task<ActionResult<string>> GetOpportunityNameById(int id)
        {
            var opportunity = await _context.VolunteerOpportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound("Opportunity not found.");
            }

            return Ok(opportunity.Title); 
        }

    }
}