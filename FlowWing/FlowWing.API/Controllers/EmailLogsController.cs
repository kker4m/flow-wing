using System.Reflection;
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
        private readonly IAttachmentService _attachmentService;
        private readonly EmailSenderService _emailSenderService;

        public EmailLogsController(IEmailLogService emailLogService, IUserService userService, IOptions<AppSettings> appSettings, IAttachmentService attachmentService, EmailSenderService emailSenderService)
        {
            _emailLogService = emailLogService;
            _userService = userService;
            _appSettings = appSettings.Value;
            _attachmentService = attachmentService;
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
                bool Sender;
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                //find attachments by user's email
                var userEmails = await _emailLogService.GetEmailLogsByRecipientsEmailAsync(UserEmail);
                var resultEmails = new List<object>();
                foreach (var email in userEmails)
                {
                    email.User = user;
                    IEnumerable<Entities.Attachment?> attachments = await _attachmentService.GetAttachmentsByEmailLogIdAsync(email.Id);
                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                        {
                            attachment.EmailLog = email;
                        }
                    }

                    if (email.SenderEmail == UserEmail)
                    {
                        Sender = true;
                    }
                    else if (email.RecipientsEmail.Contains(UserEmail))
                    {
                        Sender = false;
                    }
                    else
                    {
                        return NotFound();
                    }


                    resultEmails.Add(new { EmailLog = email, Sender = Sender, Attachments = attachments });
                }

                var result = new { User = user, UserEmails = resultEmails, Username = UserEmail };

                return Ok(result);
            }

            return Unauthorized();
        }
        private class answerEmail
        {
            public EmailLog emailLog { get; set; }
            public IEnumerable<Entities.Attachment>? attachmentInfos { get; set; }
            public EmailLog? forwardedEmailLog { get; set; }
            public answerEmail? answer { get; set; }

        }

        ///<summary>
        ///  Get email by id and the answers or forwarded emails of the email
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmailAndAnswersByEmailLogId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmailInformatinByEmailLogId(int id)
        {
            IEnumerable<Entities.Attachment>? emailAttachments;
            IEnumerable<Entities.Attachment>? answerAttachments;
            answerEmail createdAnswerEmail = null;
            answerEmail holdingAnswerEmail = null;
            EmailLog? forwardedEmailLog;
            EmailLog? emailLog;
            EmailLog? answer;
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                emailLog = await _emailLogService.GetEmailLogByIdAsync(id);

                if (emailLog == null || !emailLog.Status)
                {
                    return NotFound();
                }

                if (emailLog.ForwardedFrom != null)
                {
                    forwardedEmailLog = await _emailLogService.GetEmailLogByIdAsync(emailLog.ForwardedFrom);
                }
                else
                {
                    forwardedEmailLog = null;
                }

                emailAttachments = await _attachmentService.GetAttachmentsByEmailLogIdAsync(emailLog.Id);
                if (emailAttachments != null)
                {
                    foreach (var attachment in emailAttachments)
                    {
                        attachment.EmailLog = emailLog;
                    }
                }
                answerEmail firstAnswerEmail = new answerEmail
                {
                    emailLog = emailLog,
                    attachmentInfos = emailAttachments,
                    forwardedEmailLog = forwardedEmailLog,
                    answer = null
                };

                //EmailLog'un answer'larini tek tek gez ( recursive ) 
                while (emailLog.Answer != null)
                {
                    answer = await _emailLogService.GetEmailLogByIdAsync(emailLog.Answer);
                    if (answer != null)
                    {
                        if (answer.Status == false)
                        {
                            break;
                        }

                        if (answer.ForwardedFrom != null)
                        {
                            forwardedEmailLog = await _emailLogService.GetEmailLogByIdAsync(answer.ForwardedFrom);
                        }
                        else
                        {
                            forwardedEmailLog = null;
                        }

                        answerAttachments = _attachmentService.GetAttachmentsByEmailLogIdAsync(answer.Id).Result;
                        if (answerAttachments != null)
                        {
                            foreach (var attachment in answerAttachments)
                            {
                                attachment.EmailLog = answer;
                            }
                        }
                        createdAnswerEmail = new answerEmail
                        {
                            emailLog = answer,
                            attachmentInfos = answerAttachments,
                            forwardedEmailLog = forwardedEmailLog,
                        };

                        if (firstAnswerEmail.answer == null)
                        {
                            firstAnswerEmail.answer = createdAnswerEmail;
                            holdingAnswerEmail = createdAnswerEmail;
                        }
                        else
                        {
                            holdingAnswerEmail.answer = createdAnswerEmail;
                            holdingAnswerEmail = createdAnswerEmail;
                        }

                        emailLog = answer;
                    }
                    else if(answer == null)
                    {
                        return BadRequest("Email's answer not found");
                    }
                
                }

                return Ok(firstAnswerEmail);
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
                bool Sender;
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                EmailLog? forwardedEmailLog;
                var userEmails = await _emailLogService.GetEmailLogsByUserIdAsync(int.Parse(UserId));
                var resultEmails = new List<object>();
                foreach (var email in userEmails)
                {
                    email.User = user;
                    if (email.ForwardedFrom != null)
                    {
                        forwardedEmailLog = await _emailLogService.GetEmailLogByIdAsync(email.ForwardedFrom);
                    }
                    else
                    {
                        forwardedEmailLog = null;
                    }


                    if (email.SenderEmail == UserEmail)
                    {
                        Sender = true;
                    }
                    else if (email.RecipientsEmail.Contains(UserEmail))
                    {
                        Sender = false;
                    }
                    else
                    {
                        return NotFound();
                    }

                    IEnumerable<Entities.Attachment?> attachments = await _attachmentService.GetAttachmentsByEmailLogIdAsync(email.Id);
                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                        {
                            attachment.EmailLog = email;
                        }
                    }
                    resultEmails.Add(new { EmailLog = email, Sender = Sender, ForwardedEmailLog = forwardedEmailLog, Attachments = attachments });
                }
                var result = new { User = user, UserEmails = resultEmails, Username = UserEmail };

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
                var attachments = await _attachmentService.GetAttachmentsByEmailLogIdAsync(emailLog.Id);
                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        attachment.EmailLog = emailLog;
                        attachment.EmailLog.User = user;
                    }
                }

                var result = new { emailLog = emailLog, Sender = Sender, Attachments = attachments };

                return Ok(result);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Create an Email Log
        /// </summary>
        /// <param name="emailLogModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmailLog([FromForm] EmailLogModel emailLogModel)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                EmailLog createdEmailLog;
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                var formFiles = HttpContext.Request.Form.Files;
                string attachmentIds = "";

                foreach (string recipient in emailLogModel.RecipientsEmail.Split(','))
                {
                    if (!recipient.Contains("@arcelik.com"))
                    {
                        return BadRequest("Yalnızca arcelik maillerine mail gönderilebilmektedir");
                    }
                }

                createdEmailLog = new EmailLog
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

                if (emailLogModel.RepliedEmailId != null)
                {
                    int RepliedEmailId = (int)emailLogModel.RepliedEmailId;
                    EmailLog RepliedEmail = await _emailLogService.GetEmailLogByIdAsync(RepliedEmailId);

                    if (RepliedEmail == null) 
                    {
                        //RepliedEmailId'si verilen email log bulunamadıysa kullaniciya bu durumu bildiriyoruz.
                        return NotFound("Replied Email Not Found");
                    }

                    createdEmailLog = await _emailLogService.CreateEmailLogAsync(createdEmailLog);

                    if (RepliedEmail.Answer == null)
                    {
                        RepliedEmail.Answer = createdEmailLog.Id;
                    }
                    else
                    {
                        return BadRequest("Replied Email has already been answered");
                    }
                    _emailLogService.UpdateEmailLog(RepliedEmail);
                }
                else
                {
                    createdEmailLog = await _emailLogService.CreateEmailLogAsync(createdEmailLog);
                }

                foreach (var formFile in formFiles)
                {
                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                        byte[] bytes = stream.ToArray();

                        var attachment = new Entities.Attachment
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

                await _emailSenderService.UpdateStatus(createdEmailLog);


                return CreatedAtAction(nameof(GetEmailLogById), new { id = createdEmailLog.Id }, createdEmailLog);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Create an Forwarded Email Log
        /// </summary>
        /// <param name="forwardedEmailLogModel"></param>
        /// <returns></returns>
        [HttpPost("CreateForwardedEmailLog")]
        public async Task<IActionResult> CreateForwardedEmailLog([FromForm] ForwardedEmailLogModel forwardedEmailLogModel)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                EmailLog createdEmailLog;
                EmailLog? forwardedEmailLog;
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                var formFiles = HttpContext.Request.Form.Files;
                string attachmentIds = "";

                foreach (string recipient in forwardedEmailLogModel.RecipientsEmail.Split(','))
                {
                    if (!recipient.Contains("@arcelik.com"))
                    {
                        return BadRequest("Yalnızca arcelik maillerine mail gönderilebilmektedir");
                    }
                }

                forwardedEmailLog = await _emailLogService.GetEmailLogByIdAsync(forwardedEmailLogModel.ForwardedEmailId);
                if (forwardedEmailLog == null)
                {
                    return (BadRequest("Iletilmek istenen mail bulunamadi."));
                }

                createdEmailLog = new EmailLog
                {
                    UserId = int.Parse(UserId),
                    CreationDate = DateTime.UtcNow,
                    SentDateTime = DateTime.UtcNow,
                    RecipientsEmail = forwardedEmailLogModel.RecipientsEmail,
                    SenderEmail = UserEmail,
                    EmailSubject = forwardedEmailLog.EmailSubject,
                    SentEmailBody = forwardedEmailLogModel.EmailBody,
                    ForwardedFrom = forwardedEmailLogModel.ForwardedEmailId,
                    Status = true,
                    IsScheduled = false,
                    User = user
                };
                createdEmailLog = await _emailLogService.CreateEmailLogAsync(createdEmailLog);

                foreach (var formFile in formFiles)
                {
                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                        byte[] bytes = stream.ToArray();

                        var attachment = new Entities.Attachment
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
                //_emailSenderService.SendEmail(createdEmailLog.RecipientsEmail, createdEmailLog.EmailSubject, createdEmailLog.SentEmailBody, createdEmailLog);


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
        public async Task<IActionResult> UpdateEmailLog([FromForm] EmailLogModel emailLogModel, int EmailLogId)
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