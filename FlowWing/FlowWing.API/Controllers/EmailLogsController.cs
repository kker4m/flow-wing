using FlowWing.Business.Abstract;
using FlowWing.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class EmailLogsController : ControllerBase
    {
        private IEmailLogService _emailLogService;

        public EmailLogsController(IEmailLogService emailLogService)
        {
            _emailLogService = emailLogService;
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
        public async Task<IActionResult> GetEmailLogById(int id)
        {
            var emailLog = await _emailLogService.GetEmailLogByIdAsync(id);
            if (emailLog == null)
            {
                return NotFound();
            }
            return Ok(emailLog);
        }

        /// <summary>
        /// Create an Email Log
        /// </summary>
        /// <param name="emailLog"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmailLog([FromBody] EmailLog emailLog)
        {
            var createdEmailLog = await _emailLogService.CreateEmailLogAsync(emailLog);
            return CreatedAtAction(nameof(GetEmailLogById), new { id = createdEmailLog.Id }, createdEmailLog);
        }

        /// <summary>
        /// Update an Email Log
        /// </summary>
        /// <param name="emailLog"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEmailLog([FromBody] EmailLog emailLog)
        {
            var updatedEmailLog = await _emailLogService.UpdateEmailLogAsync(emailLog);
            return Ok(updatedEmailLog);
        }

        /// <summary>
        /// Delete an Email Log
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailLog(int id)
        {
            if (await _emailLogService.GetEmailLogByIdAsync(id) == null)
            {
                return NotFound();
            }
            await _emailLogService.DeleteEmailLogAsync(id);
            return Ok();
        }
    }
}
