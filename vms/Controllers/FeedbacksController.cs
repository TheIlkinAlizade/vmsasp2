using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vms.Data;
using vms.Models;

[Route("api/[controller]")]
[ApiController]
public class FeedbacksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FeedbacksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all feedback for a specific opportunity
    [HttpGet("opportunity/{opportunityId}")]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbackByOpportunity(int opportunityId)
    {
        var feedbacks = await _context.Feedbacks
            .Include(f => f.User)
            .Where(f => f.OpportunityId == opportunityId)
            .ToListAsync();

        if (!feedbacks.Any())
        {
            return NotFound("No feedback found for this opportunity.");
        }

        return feedbacks;
    }

    // Get all feedback by a specific user
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbackByUser(int userId)
    {
        var feedbacks = await _context.Feedbacks
            .Include(f => f.Opportunity)
            .Where(f => f.UserId == userId)
            .ToListAsync();

        if (!feedbacks.Any())
        {
            return NotFound("No feedback found for this user.");
        }

        return feedbacks;
    }

    // Create new feedback
    [HttpPost]
    public async Task<ActionResult<Feedback>> CreateFeedback([FromBody] Feedback feedback)
    {
        if (feedback == null)
        {
            return BadRequest("Invalid feedback data.");
        }

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFeedbackByUser), new { userId = feedback.UserId }, feedback);
    }

    // Update feedback
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateFeedback(int id, [FromBody] Feedback feedback)
    {
        if (id != feedback.Id)
        {
            return BadRequest("Feedback ID mismatch.");
        }

        _context.Entry(feedback).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Feedbacks.Any(f => f.Id == id))
            {
                return NotFound("Feedback not found.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // Delete feedback
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeedback(int id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);
        if (feedback == null)
        {
            return NotFound("Feedback not found.");
        }

        _context.Feedbacks.Remove(feedback);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpPatch("feedback/{id}")]
    public async Task<IActionResult> SendFeedback(int id, [FromBody] string feedback)
    {
        var application = await _context.VolunteerApplications.FindAsync(id);
        if (application == null || !application.IsAccepted)
        {
            return BadRequest("Feedback can only be sent to accepted applicants.");
        }

        application.Feedback = feedback;
        await _context.SaveChangesAsync();
        return Ok("Feedback sent successfully.");
    }


}
