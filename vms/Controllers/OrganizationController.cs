using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vms.Data;
using vms.Models;

namespace vms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrganizationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Register a new organization
        [HttpPost("register")]
        public async Task<ActionResult<Organization>> RegisterOrganization([FromBody] Organization organization)
        {
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
            return Ok(organization);
        }
        //Log in a organization
        [HttpPost("organization/login")]
        public async Task<IActionResult> OrganizationLogin([FromBody] LoginRequest request)
        {
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Email == request.Email && o.Password == request.Password);

            if (organization == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            return Ok(new { Message = "Login successful", Organization = organization });
        }

        // Get all organizations
        [HttpGet]
        public async Task<ActionResult<List<Organization>>> GetOrganizations()
        {
            return await _context.Organizations.ToListAsync();
        }

        // Get organization by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }

        // Update organization information
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganization(int id, [FromBody] Organization organization)
        {
            if (id != organization.Id)
            {
                return BadRequest();
            }

            _context.Entry(organization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Organizations.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // Delete an organization
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
