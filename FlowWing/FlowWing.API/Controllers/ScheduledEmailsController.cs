using System.Xml;
using FlowWing.Business.Abstract;
using FlowWing.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using FlowWing.API.Helpers;
using FlowWing.API.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class ScheduledEmailsController : ControllerBase
    {
        private IScheduledEmailService _scheduledEmailService;
        private IEmailLogService _emailLogService;
        private IUserService _userService;
        private IAttachmentService _attachmentService;
        private AppSettings _appSettings;
        private ScheduledMailHelper _scheduledMailHelper;
        public ScheduledEmailsController(IScheduledEmailService scheduledEmailService, IEmailLogService emailLogService, IUserService userService, IOptions<AppSettings> appSettings, IAttachmentService attachmentService, ScheduledMailHelper scheduledMailHelper)
        {
            _scheduledEmailService = scheduledEmailService;
            _emailLogService = emailLogService;
            _appSettings = appSettings.Value;
            _scheduledMailHelper = scheduledMailHelper;
            _userService = userService;
            _attachmentService = attachmentService;
        }

        /// <summary>
        /// Get All Scheduled Emails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
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
        [HttpPost("CreateScheduledEmail")]
        [Authorize]
        public async Task<IActionResult> CreateScheduledEmail([FromForm] ScheduledEmailLogModel scheduledEmail)
        {
            (string UserEmail, string UserId) = (HttpContext.Items["UserEmail"] as string, HttpContext.Items["UserId"] as string);
            EmailLog newEmailLog;
            EmailLog createdEmailLog;
            ScheduledEmail newScheduledEmail;
            string attachmentIds = "";

            User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
            var formFiles = HttpContext.Request.Form.Files;

            foreach (string recipient in scheduledEmail.RecipientsEmail.Split(','))
            {
                if (!recipient.Contains("@arcelik.com"))
                {
                    return BadRequest("Yalnızca arcelik maillerine mail gönderilebilmektedir");
                }
            }

            newEmailLog = new EmailLog
            {
                UserId = int.Parse(UserId),
                CreationDate = DateTime.UtcNow,
                SentDateTime = scheduledEmail.SentDateTime,
                RecipientsEmail = scheduledEmail.RecipientsEmail,
                SenderEmail = UserEmail,
                EmailSubject = scheduledEmail.EmailSubject,
                SentEmailBody = scheduledEmail.EmailBody,
                Status = false,
                IsScheduled = true,
                User = user
            };

            createdEmailLog = await _emailLogService.CreateEmailLogAsync(newEmailLog);

            newScheduledEmail = new ScheduledEmail
            {
                EmailLogId = createdEmailLog.Id,
                IsRepeating = false,
                NextSendingDate = scheduledEmail.SentDateTime,
            };

            var createdScheduledEmail = await _scheduledEmailService.CreateScheduledEmailAsync(newScheduledEmail);

            foreach (var formFile in formFiles)
            {
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);
                    byte[] bytes = stream.ToArray();

                    var attachment = new Attachment
                    {
                        EmailLogId = createdEmailLog.Id,
                        FileName = formFile.FileName,
                        FileSize = formFile.Length,
                        ContentType = formFile.ContentType,
                        Data = bytes,
                    };

                    await _attachmentService.CreateAttachmentAsync(attachment);
                    attachmentIds += attachment.Id + ",";
                }
            }

            if (attachmentIds.Length > 0)
            {
                attachmentIds = attachmentIds.Remove(attachmentIds.Length - 1);
                createdEmailLog.AttachmentIds = attachmentIds;
                _emailLogService.UpdateEmailLog(createdEmailLog);
            }

            // Hangfire'da işi planla
            _scheduledMailHelper.ScheduleScheduledEmail(createdEmailLog, scheduledEmail);

            return CreatedAtAction(nameof(CreateScheduledEmail), new { id = createdScheduledEmail.Id }, createdScheduledEmail);
        }

        /// <summary>
        /// Create an Repeating scheduled email
        /// </summary>
        /// <param name="repeatingEmail"></param>
        /// <returns></returns>
        [HttpPost("CreateScheduledRepeatingEmail")]
        [Authorize]
        public async Task<IActionResult> CreateScheduledRepeatingEmail([FromForm] ScheduledRepeatingEmailModel scheduledRepeatingEmailModel)
        {
            (string UserEmail, string UserId) = (HttpContext.Items["UserEmail"] as string, HttpContext.Items["UserId"] as string);
            EmailLog newEmailLog;
            EmailLog createdEmailLog;
            ScheduledEmail createdScheduledEmail;
            String attachmentIds = "";
            foreach (string recipient in scheduledRepeatingEmailModel.RecipientsEmail.Split(','))
            {
                if (!recipient.Contains("@arcelik.com"))
                {
                    return BadRequest("Yalnızca arcelik maillerine mail gönderilebilmektedir");
                }
            }
            User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
            var formFiles = HttpContext.Request.Form.Files;
            newEmailLog = new EmailLog
            {
                UserId = int.Parse(UserId),
                CreationDate = DateTime.UtcNow,
                SentDateTime = scheduledRepeatingEmailModel.NextSendingDate,
                RecipientsEmail = scheduledRepeatingEmailModel.RecipientsEmail,
                SenderEmail = UserEmail,
                EmailSubject = scheduledRepeatingEmailModel.EmailSubject,
                SentEmailBody = scheduledRepeatingEmailModel.EmailBody,
                Status = false,
                IsScheduled = true,
                User = user
            };

            createdEmailLog = await _emailLogService.CreateEmailLogAsync(newEmailLog);

            ScheduledEmail newScheduledRepeatingEmail = new ScheduledEmail
            {
                EmailLogId = createdEmailLog.Id,
                IsRepeating = true,
                NextSendingDate = scheduledRepeatingEmailModel.NextSendingDate,
                RepeatInterval = scheduledRepeatingEmailModel.RepeatInterval,
                RepeatEndDate = scheduledRepeatingEmailModel.RepeatEndDate
            };

            createdScheduledEmail = await _scheduledEmailService.CreateScheduledEmailAsync(newScheduledRepeatingEmail);
            foreach (var formFile in formFiles)
            {
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);
                    byte[] bytes = stream.ToArray();

                    var attachment = new Attachment
                    {
                        EmailLogId = createdEmailLog.Id,
                        FileName = formFile.FileName,
                        FileSize = formFile.Length,
                        ContentType = formFile.ContentType,
                        Data = bytes,
                    };

                    await _attachmentService.CreateAttachmentAsync(attachment);
                    attachmentIds += attachment.Id + ",";
                }
            }
            createdEmailLog.repeatingLogId = createdScheduledEmail.Id;
            if (attachmentIds.Length > 0)
            {
                attachmentIds = attachmentIds.Remove(attachmentIds.Length - 1);
                createdEmailLog.AttachmentIds = attachmentIds;
            }

            _scheduledMailHelper.ScheduleRepeatingEmail(createdEmailLog, scheduledRepeatingEmailModel);

            return CreatedAtAction(nameof(CreateScheduledEmail), new { id = createdScheduledEmail.Id }, createdScheduledEmail);
        }

        /// <summary>
        /// Update an Scheduled Email
        /// </summary>
        /// <param name="scheduledEmail"></param>
        /// <returns></returns>
        [HttpPut("ScheduledEmail")]
        public async Task<IActionResult> UpdateScheduledEmail([FromForm] ScheduledEmailLogModel scheduledEmail, int scheduledEmailId)
        {

            (string UserEmail, string UserId) = (HttpContext.Items["UserEmail"] as string, HttpContext.Items["UserId"] as string);

            //get email log by emaillogId
            var scheduledEmailLog = await _scheduledEmailService.GetScheduledEmailByIdAsync(scheduledEmailId);
            var emailLog = await _emailLogService.GetEmailLogByIdAsync(scheduledEmailLog.EmailLogId);

            //get user's email logs and check if the email log exists
            var user = await _userService.GetUserByIdAsync(int.Parse(UserId));
            var userLogs = await _emailLogService.GetEmailLogsByUserIdAsync(user.Id);
            if (!userLogs.Contains(emailLog))
            {
                //return 404
                return NotFound();
            }

            //update scheduled email log
            scheduledEmailLog.NextSendingDate = scheduledEmail.SentDateTime;
            scheduledEmailLog.IsRepeating = false;
            scheduledEmailLog.RepeatEndDate = null;
            scheduledEmailLog.RepeatInterval = null;
            emailLog.RecipientsEmail = scheduledEmail.RecipientsEmail;
            emailLog.EmailSubject = scheduledEmail.EmailSubject;
            emailLog.SentEmailBody = scheduledEmail.EmailBody;

            await _scheduledEmailService.UpdateScheduledEmailAsync(scheduledEmailLog);

            await _emailLogService.UpdateEmailLogAsync(emailLog);

            return CreatedAtAction(nameof(UpdateScheduledEmail), new { id = emailLog.Id }, emailLog);
        }

        /// <summary>
        /// Update an Scheduled Repeating Email
        /// </summary>
        /// <param name="scheduledRepeatingEmail"></param>
        /// <returns></returns>
        [HttpPut("ScheduledRepeatingEmail")]
        public async Task<IActionResult> UpdateScheduledRepeatingEmail([FromForm] ScheduledRepeatingEmailModel scheduledRepeatingEmail, int scheduledRepeatingEmailId)
        {

            (string UserEmail, string UserId) = (HttpContext.Items["UserEmail"] as string, HttpContext.Items["UserId"] as string);

            //get email log by emaillogId
            var scheduledEmailLog = await _scheduledEmailService.GetScheduledEmailByIdAsync(scheduledRepeatingEmailId);
            var emailLog = await _emailLogService.GetEmailLogByIdAsync(scheduledEmailLog.EmailLogId);

            //get user's email logs and check if the email log exists
            var user = await _userService.GetUserByIdAsync(int.Parse(UserId));
            var userLogs = await _emailLogService.GetEmailLogsByUserIdAsync(user.Id);
            if (!userLogs.Contains(emailLog))
            {
                //return 404
                return NotFound();
            }

            //update scheduled email log
            scheduledEmailLog.NextSendingDate = scheduledRepeatingEmail.NextSendingDate;
            scheduledEmailLog.IsRepeating = true;
            scheduledEmailLog.RepeatEndDate = scheduledRepeatingEmail.RepeatEndDate;
            scheduledEmailLog.RepeatInterval = scheduledRepeatingEmail.RepeatInterval;
            emailLog.RecipientsEmail = scheduledRepeatingEmail.RecipientsEmail;
            emailLog.EmailSubject = scheduledRepeatingEmail.EmailSubject;
            emailLog.SentEmailBody = scheduledRepeatingEmail.EmailBody;

            await _scheduledEmailService.UpdateScheduledEmailAsync(scheduledEmailLog);

            await _emailLogService.UpdateEmailLogAsync(emailLog);

            return CreatedAtAction(nameof(UpdateScheduledEmail), new { id = emailLog.Id }, emailLog);
        }


        /// <summary>
        /// Delete an Scheduled Email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("ScheduledEmail/{id}")]
        public async Task<IActionResult> DeleteScheduledEmail(int id)
        {
            if (await _scheduledEmailService.GetScheduledEmailByIdAsync(id) == null)
            {
                return NotFound();
            }
            await _scheduledEmailService.DeleteScheduledEmailAsync(id);

            return Ok();
        }

        /// <summary>
        /// Delete an Scheduled Repeating Email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("ScheduledRepeatingEmail/{id}")]
        public async Task<IActionResult> DeleteScheduledRepeatingEmail(int id)
        {
            if (await _scheduledEmailService.GetScheduledEmailByIdAsync(id) == null)
            {
                return NotFound();
            }

            await _scheduledEmailService.DeleteScheduledRepeatingEmailAsync(id);
            
            return Ok();
        }
    }
}
