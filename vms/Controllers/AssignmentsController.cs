using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vms.Data;
using vms.Models;

[Route("api/[controller]")]
[ApiController]
public class AssignmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AssignmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all assignments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignments()
    {
        return await _context.Assignments
            .Include(a => a.User)
            .Include(a => a.Opportunity)
            .ToListAsync();
    }

    // Get assignments for a specific user
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignmentsByUser(int userId)
    {
        var assignments = await _context.Assignments
            .Include(a => a.Opportunity)
            .Where(a => a.UserId == userId)
            .ToListAsync();

        if (!assignments.Any())
        {
            return NotFound("No assignments found for this user.");
        }

        return assignments;
    }

    // Get assignments for a specific opportunity
    [HttpGet("opportunity/{opportunityId}")]
    public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignmentsByOpportunity(int opportunityId)
    {
        var assignments = await _context.Assignments
            .Include(a => a.User)
            .Where(a => a.OpportunityId == opportunityId)
            .ToListAsync();

        if (!assignments.Any())
        {
            return NotFound("No assignments found for this opportunity.");
        }

        return assignments;
    }

    // Create a new assignment
    [HttpPost]
    public async Task<ActionResult<Assignment>> CreateAssignment([FromBody] Assignment assignment)
    {
        if (assignment == null)
        {
            return BadRequest("Invalid assignment data.");
        }

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAssignments), new { id = assignment.Id }, assignment);
    }

    // Update an assignment
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAssignment(int id, [FromBody] Assignment assignment)
    {
        if (id != assignment.Id)
        {
            return BadRequest("Assignment ID mismatch.");
        }

        _context.Entry(assignment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Assignments.Any(a => a.Id == id))
            {
                return NotFound("Assignment not found.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // Delete an assignment
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        var assignment = await _context.Assignments.FindAsync(id);
        if (assignment == null)
        {
            return NotFound("Assignment not found.");
        }

        _context.Assignments.Remove(assignment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
