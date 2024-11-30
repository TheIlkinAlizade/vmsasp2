using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.Data;
using vms.Models;

namespace vms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VolunteerApplicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Apply for a volunteer opportunity (User)
        [HttpPost("apply")]
        public async Task<ActionResult<VolunteerApplication>> ApplyForOpportunity([FromBody] VolunteerApplication application)
        {
            var opportunity = await _context.VolunteerOpportunities
                .Include(o => o.VolunteerApplications)
                .FirstOrDefaultAsync(o => o.Id == application.VolunteerOpportunityId);

            if (opportunity == null)
            {
                return NotFound("Opportunity not found.");
            }

            if (opportunity.VolunteerApplications.Count >= opportunity.MaxApplicants)
            {
                return BadRequest("The maximum number of applicants has been reached for this opportunity.");
            }

            var existingApplication = await _context.VolunteerApplications
                .FirstOrDefaultAsync(a => a.UserId == application.UserId && a.VolunteerOpportunityId == application.VolunteerOpportunityId);

            if (existingApplication != null)
            {
                return BadRequest("You have already applied for this opportunity.");
            }

            _context.VolunteerApplications.Add(application);
            await _context.SaveChangesAsync();

            return Ok(application);
        }


        // Get all applications for a specific volunteer opportunity (Organization)
        [HttpGet("applications/{opportunityId}")]
        public async Task<ActionResult<List<VolunteerApplication>>> GetApplicationsForOpportunity(int opportunityId)
        {
            var applications = await _context.VolunteerApplications
                .Include(a => a.User) // Include user details
                .Where(a => a.VolunteerOpportunityId == opportunityId)
                .ToListAsync();

            if (applications == null || applications.Count == 0)
            {
                return NotFound("No applications found for this opportunity.");
            }

            return Ok(applications);
        }

        // Accept or Reject an application (Organization)
        [HttpPatch("accept/{applicationId}")]
        public async Task<ActionResult> AcceptApplication(int applicationId, [FromBody] bool isAccepted)
        {
            var application = await _context.VolunteerApplications
                .Include(a => a.VolunteerOpportunity)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
            {
                return NotFound("Application not found.");
            }

            var opportunity = application.VolunteerOpportunity;

            if (opportunity == null)
            {
                return NotFound("Opportunity not found.");
            }

            if (isAccepted && opportunity.VolunteerApplications.Count(a => a.IsAccepted) >= opportunity.MaxApplicants)
            {
                return BadRequest("The maximum number of accepted applicants has been reached for this opportunity.");
            }

            application.IsAccepted = isAccepted;

            _context.VolunteerApplications.Update(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get all applications for a specific organization
        [HttpGet("organization/{organizationId}/applications")]
        public async Task<ActionResult<List<object>>> GetApplicationsForOrganization(int organizationId)
        {
            var applications = await _context.VolunteerApplications
                .Include(a => a.User)
                .Include(a => a.VolunteerOpportunity)
                .Where(a => a.VolunteerOpportunity.OrganizationId == organizationId)
                .Select(a => new
                {
                    ApplicationId = a.Id,
                    User = new
                    {
                        a.User.Id,
                        a.User.Name,
                        a.User.Surname,
                        a.User.Email
                    },
                    Opportunity = new
                    {
                        a.VolunteerOpportunity.Id,
                        a.VolunteerOpportunity.Title,
                        a.VolunteerOpportunity.Description
                    },
                    a.IsAccepted,
                    a.Feedback
                })
                .ToListAsync();

            if (applications == null || applications.Count == 0)
            {
                return NotFound("No applications found for this organization.");
            }

            return Ok(applications);
        }

        [HttpGet("user/{userId}/applications")]
        public async Task<ActionResult<List<VolunteerApplication>>> GetUserApplications(int userId)
        {
            var applications = await _context.VolunteerApplications
                .Include(a => a.VolunteerOpportunity) // Include opportunity details
                .Where(a => a.UserId == userId)
                .ToListAsync();

            if (applications == null || applications.Count == 0)
            {
                return NotFound("No applications found for this user.");
            }

            return Ok(applications);
        }


        [HttpDelete("delete/{applicationId}")]
        public async Task<IActionResult> DeleteApplication(int applicationId)
        {
            var application = await _context.VolunteerApplications.FindAsync(applicationId);

            if (application == null)
            {
                return NotFound("Application not found.");
            }

            _context.VolunteerApplications.Remove(application);
            await _context.SaveChangesAsync();

            return Ok("Applicant deleted successfully.");
        }
        [HttpGet("check")]
        public async Task<IActionResult> CheckApplication([FromQuery] int userId, [FromQuery] int opportunityId)
        {
            var applicationExists = await _context.VolunteerApplications
                .AnyAsync(a => a.UserId == userId && a.VolunteerOpportunityId == opportunityId);

            if (applicationExists)
            {
                return Ok(new { Applied = true, Message = "User has already applied for this opportunity." });
            }

            return Ok(new { Applied = false, Message = "User has not applied for this opportunity." });
        }
        [HttpGet("organization/{organizationId}/all-applicants")]
        public async Task<ActionResult<List<object>>> GetAllApplicantsForOrganization(int organizationId)
        {
            var opportunityIds = await _context.VolunteerOpportunities
                .Where(o => o.OrganizationId == organizationId)
                .Select(o => o.Id)
                .ToListAsync();

            if (!opportunityIds.Any())
            {
                return NotFound($"No opportunities found for organizationId: {organizationId}");
            }

            var allApplicants = await _context.VolunteerApplications
                .Include(a => a.User)
                .Include(a => a.VolunteerOpportunity)
                .Where(a => opportunityIds.Contains(a.VolunteerOpportunityId))
                .Select(a => new
                {
                    OpportunityId = a.VolunteerOpportunityId,
                    Applicant = new
                    {
                        a.User.Id,
                        a.User.Name,
                        a.User.Email,
                        a.IsAccepted
                    }
                })
                .ToListAsync();

            if (!allApplicants.Any())
            {
                return NotFound($"No applicants found for the organization's opportunities.");
            }

            // Group by opportunity ID for easier organization
            var groupedApplicants = allApplicants
                .GroupBy(a => a.OpportunityId)
                .Select(group => new
                {
                    OpportunityId = group.Key,
                    Applicants = group.Select(a => a.Applicant).ToList()
                })
                .ToList();

            return Ok(groupedApplicants);
        }


    }
}