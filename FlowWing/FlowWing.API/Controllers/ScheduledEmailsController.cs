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
    [EnableCors("AllowAll")]
    public class ScheduledEmailsController : ControllerBase
    {
        private IScheduledEmailService _scheduledEmailService;
        private IEmailLogService _emailLogService;

        public ScheduledEmailsController(IScheduledEmailService scheduledEmailService, IEmailLogService emailLogService)
        {
            _scheduledEmailService = scheduledEmailService;
            _emailLogService = emailLogService;
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
        [Authorize]
        public async Task<IActionResult> CreateScheduledEmail([FromBody] ScheduledEmailLogModel scheduledEmail)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (JwtHelper.TokenIsValid(token,"FlowWingSecretKeyFlowWingSecretKeyFlowWingSecretKeyFlowWingSecretKey"))
            {
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);

                EmailLog newEmailLog = new EmailLog
                {
                    UserId = int.Parse(UserId),
                    CreationDate = DateTime.UtcNow,
                    SentDateTime = scheduledEmail.SentDateTime,
                    RecipientsEmail = scheduledEmail.RecipientsEmail,
                    SenderEmail = UserEmail,
                    EmailSubject = scheduledEmail.EmailSubject,
                    SentEmailBody = scheduledEmail.EmailBody,
                    Status = false,
                    IsScheduled = true
                };

                var createdEmailLog = await _emailLogService.CreateEmailLogAsync(newEmailLog);

                ScheduledEmail newScheduledEmail = new ScheduledEmail
                {
                    EmailLogId = createdEmailLog.Id,
                    IsRepeating = false,
                    NextSendingDate = scheduledEmail.SentDateTime,
                };

                var createdScheduledEmail = await _scheduledEmailService.CreateScheduledEmailAsync(newScheduledEmail);

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
