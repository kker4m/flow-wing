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
        private readonly EmailSenderService _emailSenderService;
        private readonly IAttachmentService _attachmentService;

        public EmailLogsController(IEmailLogService emailLogService, IUserService userService, IOptions<AppSettings> appSettings, EmailSenderService emailSenderService,IAttachmentService attachmentService)
        {
            _emailLogService = emailLogService;
            _userService = userService;
            _appSettings = appSettings.Value;
            _emailSenderService = emailSenderService;
            _attachmentService = attachmentService;
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
                //find attachments by user's email
                var userEmails = await _emailLogService.GetEmailLogsByRecipientsEmailAsync(UserEmail);
                var resultEmails = new List<object>();
                foreach (var email in userEmails)
                {
                    email.User= user;
                    IEnumerable<Attachment?> attachments = await _attachmentService.GetAttachmentsByEmailLogIdAsync(email.Id);
                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                        {
                            attachment.EmailLog = email;
                        }   
                    }
                    
                    resultEmails.Add(new { EmailLog = email, Attachments = attachments });
                }
                
                var result = new { User = user, UserEmails = resultEmails, Username = UserEmail};

                return Ok(result);
            }

            return Unauthorized();
        }
        private class answerEmail
        {
            public EmailLog emailLog { get; set; }
            public IEnumerable<Attachment>? attachments { get; set; }
            public answerEmail? answer { get; set; }
        }

        ///<summary>
        ///  Get email by id and the answers of the email
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmailAndAnswersByEmailLogId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmailAndAnswersByEmailLogId(int id)
        {
            IEnumerable<Attachment>? emailAttachments;
            IEnumerable<Attachment>? answerAttachments;
            EmailLog? emailLog;
            EmailLog? answer;
            List<answerEmail> answerEmails = new List<answerEmail>();
            
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                emailLog = await _emailLogService.GetEmailLogByIdAsync(id);
                
                if (emailLog == null)
                {
                    return NotFound();
                }
                
                emailAttachments = await _attachmentService.GetAttachmentsByEmailLogIdAsync(emailLog.Id);
                if (emailAttachments != null)
                {
                    foreach (Attachment attachment in emailAttachments)
                    {
                        attachment.EmailLog = emailLog;
                    }
                }
                answerEmails.Add(new answerEmail
                {
                    emailLog = emailLog,
                    attachments = emailAttachments,
                    answer = null
                });
                
                while (emailLog.Answers != null)
                {
                    answer = await _emailLogService.GetEmailLogByIdAsync(int.Parse(emailLog.Answers));
                    answerAttachments = _attachmentService.GetAttachmentsByEmailLogIdAsync(answer.Id).Result;
                    if (answerAttachments != null)
                    {
                        foreach (Attachment attachment in answerAttachments)
                        {
                            attachment.EmailLog = answer;
                        }
                    }

                    answerEmails.Add( new answerEmail
                    {
                        emailLog = answer,
                        attachments = answerAttachments,
                        answer = null
                    });
                    emailLog = answer;
                }

                for(int i=0; i<answerEmails.Count-1; i++)
                {
                    answerEmails[i].answer = answerEmails[i+1];
                }
                
                return Ok(answerEmails[0]);
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
                var resultEmails = new List<object>();
                foreach (var email in userEmails)
                {
                    email.User= user;
                    IEnumerable<Attachment?> attachments = await _attachmentService.GetAttachmentsByEmailLogIdAsync(email.Id);
                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                        {
                            attachment.EmailLog = email;
                        }   
                    }
                    resultEmails.Add(new { EmailLog = email, Attachments = attachments });
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

                foreach (var attachment in attachments)
                {
                    attachment.EmailLog = emailLog;
                    attachment.EmailLog.User = user;
                }
                
                var result = new { emailLog = emailLog, Sender = Sender, Attachments = attachments };

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
        public async Task<IActionResult> CreateEmailLog([FromForm] EmailLogModel emailLogModel)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
            {
                EmailLog NewEmailLog;
                EmailLog createdEmailLog;
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                User user = await _userService.GetUserByIdAsync(int.Parse(UserId));
                var formFiles = HttpContext.Request.Form.Files;
                string attachmentIds = "";

                NewEmailLog = new EmailLog
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
                    
                    if(RepliedEmail == null)
                    { 
                        //RepliedEmailId'si verilen email log bulunamadıysa kullaniciya bu durumu bildiriyoruz.
                        return NotFound("Replied Email Not Found");
                    }
                    
                    createdEmailLog = await _emailLogService.CreateEmailLogAsync(NewEmailLog);
                    
                    if (RepliedEmail.Answers == null)
                    {
                        RepliedEmail.Answers = NewEmailLog.Id.ToString();
                    }
                    else
                    {
                        return BadRequest("Replied Email has already been answered");
                    }
                    _emailLogService.UpdateEmailLog(RepliedEmail);
                }
                else
                {
                    createdEmailLog = await _emailLogService.CreateEmailLogAsync(NewEmailLog);
                }
                
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
                
                _emailSenderService.SendEmail(emailLogModel.RecipientsEmail, emailLogModel.EmailSubject, emailLogModel.EmailBody, createdEmailLog);


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
