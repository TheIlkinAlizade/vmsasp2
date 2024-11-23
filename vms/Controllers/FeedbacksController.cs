using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vms.Data;
using vms.Models;
using System.Linq;
using System.Threading.Tasks;

namespace vms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Submit feedback about a user for a specific opportunity
        [HttpPost("submit")]
        public async Task<ActionResult<Feedback>> SubmitFeedback([FromBody] Feedback feedback)
        {
            // Verify the organization owns the opportunity
            var opportunity = await _context.VolunteerOpportunities
                .FirstOrDefaultAsync(o => o.Id == feedback.OpportunityId && o.OrganizationId == feedback.OrganizationId);

            if (opportunity == null)
            {
                return BadRequest("You are not authorized to give feedback for this opportunity.");
            }

            // Verify the user was accepted for this opportunity
            var application = await _context.VolunteerApplications
                .FirstOrDefaultAsync(a => a.UserId == feedback.UserId && a.VolunteerOpportunityId == feedback.OpportunityId && a.IsAccepted);

            if (application == null)
            {
                return BadRequest("The user was not accepted for this opportunity.");
            }

            // Save feedback
            feedback.CreatedAt = DateTime.Now;
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id }, feedback);
        }

        // Get feedback by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedbackById(int id)
        {
            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feedback == null)
            {
                return NotFound("Feedback not found.");
            }

            return Ok(feedback);
        }

        // Get all feedbacks given by an organization
        [HttpGet("organization/{organizationId}")]
        public async Task<ActionResult<IQueryable<Feedback>>> GetFeedbacksByOrganization(int organizationId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.OrganizationId == organizationId)
                .ToListAsync();

            return Ok(feedbacks);
        }

        // Get feedback about a specific user for a specific opportunity
        [HttpGet("opportunity/{opportunityId}/user/{userId}")]
        public async Task<ActionResult<Feedback>> GetFeedbackForUserInOpportunity(int opportunityId, int userId)
        {
            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.OpportunityId == opportunityId && f.UserId == userId);

            if (feedback == null)
            {
                return NotFound("Feedback not found for this user in this opportunity.");
            }

            return Ok(feedback);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetFeedbacksForUser(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { message = "Invalid user ID" });
            }

            var feedbacks = _context.Feedbacks
                .Where(f => f.UserId == userId)
                .ToList();

            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound(new { message = "No feedbacks found for the specified user ID" });
            }

            return Ok(feedbacks);
        }

    }
}
