using FlowWing.Business.Abstract;
using FlowWing.Business.Concrete;
using FlowWing.DataAccess.Abstract;
using FlowWing.API.Helpers;
using FlowWing.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IEmailLogService _emailLogService;
        private AppSettings _appSettings;

        public UsersController(IUserService userService, IEmailLogService emailLogService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _emailLogService = emailLogService;
            _appSettings = appSettings.Value;
        }

        
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        /// <summary>
        /// Authorize olmus user'in user loglarini getiren method
        /// </summary>
        /// <returns></returns>
        [HttpGet("logs")]
        public async Task<IActionResult> GetUserLogs()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (JwtHelper.TokenIsValid(token,secretKey:_appSettings.SecretKey))
            {
                (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                var user = await _userService.GetUserByEmailAsync(UserEmail);
                if (user == null)
                {
                    return NotFound();
                }
                var logs = await _emailLogService.GetEmailLogsByUserIdAsync(int.Parse(UserId));
                return Ok(logs);
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// Create an User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Update an User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var existingUser = await _userService.GetUserByIdAsync(user.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            var updatedUser = await _userService.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Delete an User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToDelete = await _userService.GetUserByIdAsync(id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(userToDelete);
            return Ok();
        }
    }
}
