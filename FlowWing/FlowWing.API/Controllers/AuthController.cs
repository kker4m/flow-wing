using FlowWing.API.Helpers;
using FlowWing.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using FlowWing.Business.Abstract;
using FlowWing.Entities;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private const string SecretKey = "FlowWingSecretKeyFlowWingSecretKeyFlowWingSecretKeyFlowWingSecretKey";

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Kullanıcı kaydı işlemleri burada gerçekleştirilir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            // Kullanıcı kaydı gerçekleştirilir...
            try
            {
                User newUser = new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Username,
                    LastLoginDate = DateTime.Now,
                    CreationDate = DateTime.Now
                };


                await _userService.CreateUserAsync(newUser);

                // Kullanıcı başarıyla kaydedildiğinde JWT oluşturulabilir
                string token = JwtHelper.GenerateJwtToken(newUser.Id, model.Email, SecretKey, 15); // Örnek: 60 gun geçerli bir token oluşturuyoruz.

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Kullanıcı giriş işlemleri burada gerçekleştirilir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            // Kullanıcı giriş işlemleri burada gerçekleştirilir
            User user = await _userService.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Yanlis email veya sifre");
            }
            String password = PasswordHasher.HashPassword(model.Password);

            if (model.Email == user.Email && password == user.Password)
            {
                string token = JwtHelper.GenerateJwtToken(user.Id, user.Email, SecretKey, 1); // Örnek: 1 gun geçerli bir token oluşturuyoruz.
                UserResponseModel response = new UserResponseModel
                {
                    Email = user.Email,
                    Username = user.Username,
                    Token = token
                };
                return Ok(response);

            }
            else
            {
                return BadRequest("Yanlis email veya sifre");
            }
        }
    }
}
