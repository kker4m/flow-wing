using FlowWing.Business.Abstract;
using FlowWing.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledEmailsController : ControllerBase
    {
        private IScheduledEmailService _scheduledEmailService;

        public ScheduledEmailsController(IScheduledEmailService scheduledEmailService)
        {
            _scheduledEmailService = scheduledEmailService;
        }

        /// <summary>
        /// Get All Scheduled Emails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var scheduledEmails = await _scheduledEmailService.GetAllScheduledEmailsAsync();
            return Ok(scheduledEmails);
        }

        /// <summary>
        /// Get Scheduled Email By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduledEmailById(int id)
        { 
            var scheduledEmail = await _scheduledEmailService.GetScheduledEmailByIdAsync(id);
            if (scheduledEmail == null)
            {
                return NotFound();
            }
            return Ok(scheduledEmail);
        }

        /// <summary>
        /// Create an Scheduled Email
        /// </summary>
        /// <param name="scheduledEmail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateScheduledEmail([FromBody] ScheduledEmail scheduledEmail)
        {
            var createdScheduledEmail = await _scheduledEmailService.CreateScheduledEmailAsync(scheduledEmail);
            return CreatedAtAction(nameof(GetScheduledEmailById), new { id = createdScheduledEmail.Id }, createdScheduledEmail);
        }

        /// <summary>
        /// Update an Scheduled Email
        /// </summary>
        /// <param name="scheduledEmail"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateScheduledEmail([FromBody] ScheduledEmail scheduledEmail)
        {
            var existingScheduledEmail = await _scheduledEmailService.GetScheduledEmailByIdAsync(scheduledEmail.Id);
            if (existingScheduledEmail == null)
            {
                return NotFound();
            }
            var updatedScheduledEmail = await _scheduledEmailService.UpdateScheduledEmailAsync(scheduledEmail);
            return Ok(updatedScheduledEmail);
        }

        /// <summary>
        /// Delete an Scheduled Email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduledEmail(int id)
        {
            if (await _scheduledEmailService.GetScheduledEmailByIdAsync(id) == null)
            {
                return NotFound();
            }
            await _scheduledEmailService.DeleteScheduledEmailAsync(id);
            return Ok();
        }
    }
}
