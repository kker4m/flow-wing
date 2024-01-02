using FlowWing.Business.Abstract;
using FlowWing.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using FlowWing.API.Helpers;
using FlowWing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowAll")]
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
        public async Task<IActionResult> GetAllScheduledEmails()
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
        public async Task<IActionResult> CreateScheduledEmail([FromBody] ScheduledEmailLogModel scheduledEmail)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (JwtHelper.TokenIsValid(token,"FlowWingSecretKeyFlowWingSecretKeyFlowWingSecretKeyFlowWingSecretKey"))
            {
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                var NewScheduledEmailLog = new ScheduledEmail
                {
                    IsRepeating = false,
                    NextSendingDate = scheduledEmail.NextSendingDate
                };
                
                var NewEmailLog = new EmailLog
                {
                    CreationDate = DateTime.UtcNow,
                    RecipientsEmail = scheduledEmail.EmailLog.RecipientsEmail,
                    SenderEmail = UserEmail,
                    EmailSubject = scheduledEmail.EmailLog.EmailSubject,
                    SentEmailBody = scheduledEmail.EmailLog.EmailBody,
                    Status = false,
                    IsScheduled = true,
                    //ScheduledEmail = NewScheduledEmailLog
                };
                
                var createdScheduledEmail = await _scheduledEmailService.CreateScheduledEmailAsync(NewScheduledEmailLog);
                
                
                return CreatedAtAction(nameof(CreateScheduledEmail), new { id = createdScheduledEmail.Id }, createdScheduledEmail);
            }
            return Unauthorized(); 
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
