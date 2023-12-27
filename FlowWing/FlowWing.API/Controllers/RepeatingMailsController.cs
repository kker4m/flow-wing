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
    public class RepeatingMailsController : ControllerBase
    {
        private IRepeatingMailService _repeatingMailService;

        public RepeatingMailsController(IRepeatingMailService repeatingMailService)
        {
            _repeatingMailService = repeatingMailService;
        }
        /// <summary>
        /// Get All Repeating Mails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRepeatingMails()
        {
            var repeatingMails = await _repeatingMailService.GetAllRepeatingMailsAsync();
            return Ok(repeatingMails);
        }

        /// <summary>
        /// Get Repeating Mail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRepeatingMailById(int id)
        {   
            var repeatingMail = await _repeatingMailService.GetRepeatingMailByIdAsync(id);
            if (repeatingMail == null)
            {
                return NotFound();
            }
            return Ok(repeatingMail);
        }

        /// <summary>
        /// Create an Repeating Mail
        /// </summary>
        /// <param name="repeatingMail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRepeatingMail([FromBody] RepeatingMail repeatingMail)
        {
            var createdRepeatingMail = await _repeatingMailService.CreateRepeatingMailAsync(repeatingMail);
            return CreatedAtAction(nameof(GetRepeatingMailById), new { id = createdRepeatingMail.Id }, createdRepeatingMail);
        }

        /// <summary>
        /// Update an Repeating Mail
        /// </summary>
        /// <param name="repeatingMail"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateRepeatingMail([FromBody] RepeatingMail repeatingMail)
        {
            var updatedRepeatingMail = await _repeatingMailService.UpdateRepeatingMailAsync(repeatingMail);
            return NoContent();
        }

        /// <summary>
        /// Delete an Repeating Mail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepeatingMail(int id)
        {
            if (await _repeatingMailService.GetRepeatingMailByIdAsync(id) == null)
            {
                return NotFound();
            }
            await _repeatingMailService.DeleteRepeatingMailAsync(id);
            return NoContent();
        }
    }
}
