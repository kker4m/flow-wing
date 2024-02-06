using FlowWing.Business.Abstract;
using FlowWing.Entities;
using FlowWing.API.Helpers;
using FlowWing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class EmailLogsController : ControllerBase
    {
        private IEmailLogService _emailLogService;
        private IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly EmailSenderService _emailSenderService;

        public EmailLogsController(IEmailLogService emailLogService, IUserService userService, IOptions<AppSettings> appSettings, EmailSenderService emailSenderService)
        {
            _emailLogService = emailLogService;
            _userService = userService;
            _appSettings = appSettings.Value;
            _emailSenderService = emailSenderService;
        }

        ///<summary>
        ///  Get emails which is comes to the user
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserSentEmails")]
        [Authorize]
        public async Task<IActionResult> GetUserSentEmails()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                var userEmails = await _emailLogService.GetEmailLogsByRecipientsEmailAsync(UserEmail);

                var result = new { User = user, UserEmails = userEmails, Username = user.Username };

                return Ok(result);
            }

            return Unauthorized();
        }

        ///<summary>
        /// Get Emails which is user sent
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserReceivedEmails")]
        [Authorize]
        public async Task<IActionResult> GetUserReceivedEmails()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                var userEmails = await _emailLogService.GetEmailLogsByUserIdAsync(int.Parse(UserId));

                var result = new { User = user, UserEmails = userEmails };

                return Ok(result);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Get All Email Logs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllEmailLogs()
        {
            var emailLogs = await _emailLogService.GetAllEmailLogsAsync();
            return Ok(emailLogs);
        }

        /// <summary>
        /// Get Email Log By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmailLogById(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                bool Sender;
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                if (await _emailLogService.GetEmailLogByIdAsync(id) == null)
                {
                    return NotFound();
                }


                var emailLog = await _emailLogService.GetEmailLogByIdAsync(id);
                emailLog.User = user;

                if (emailLog.SenderEmail == UserEmail)
                {
                    Sender = true;
                }
                else if (emailLog.RecipientsEmail.Contains(UserEmail))
                {
                    Sender = false;
                }
                else
                {
                    return NotFound();
                }

                var result = new { emailLog = emailLog, Sender = Sender };

                return Ok(result);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Create an Email Log
        /// </summary>
        /// <param name="emailLog"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmailLog([FromBody] EmailLogModel emailLogModel)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                EmailLog NewEmailLog = new EmailLog
                {
                    UserId = int.Parse(UserId),
                    CreationDate = DateTime.UtcNow,
                    SentDateTime = DateTime.UtcNow,
                    RecipientsEmail = emailLogModel.RecipientsEmail,
                    SenderEmail = UserEmail,
                    EmailSubject = emailLogModel.EmailSubject,
                    SentEmailBody = emailLogModel.EmailBody,
                    Status = true,
                    IsScheduled = false,
                    User = user
                };

                var createdEmailLog = await _emailLogService.CreateEmailLogAsync(NewEmailLog);

                _emailSenderService.SendEmail(emailLogModel.RecipientsEmail, emailLogModel.EmailSubject, emailLogModel.EmailBody, createdEmailLog,emailLogModel.Attachments);


                return CreatedAtAction(nameof(GetEmailLogById), new { id = createdEmailLog.Id }, createdEmailLog);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Update an Email Log
        /// </summary>
        /// <param name="emailLogModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateEmailLog([FromBody] EmailLogModel emailLogModel, int EmailLogId)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);

                //get email log by emaillogId
                var emailLog = await _emailLogService.GetEmailLogByIdAsync(EmailLogId);

                //get user's email logs and check if the email log exists
                var user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                var userLogs = await _emailLogService.GetEmailLogsByUserIdAsync(user.Id);
                if (!userLogs.Contains(emailLog))
                {
                    //return 404
                    return NotFound();
                }


                //update email log
                emailLog.RecipientsEmail = emailLogModel.RecipientsEmail;
                emailLog.EmailSubject = emailLogModel.EmailSubject;
                emailLog.SentEmailBody = emailLogModel.EmailBody;
                emailLog.Status = false;
                emailLog.IsScheduled = false;
                await _emailLogService.UpdateEmailLogAsync(emailLog);

                return CreatedAtAction(nameof(UpdateEmailLog), new { id = emailLog.Id }, emailLog);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Delete an Email Log
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailLog(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {

                if (await _emailLogService.GetEmailLogByIdAsync(id) == null)
                {
                    return NotFound();
                }
                await _emailLogService.DeleteEmailLogAsync(id);
                return Ok();
            }
            return Unauthorized();
        }
    }
}
