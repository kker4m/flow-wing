using FlowWing.API.Helpers;
using FlowWing.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using FlowWing.Business.Abstract;
using FlowWing.Entities;
using System.Text;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                string token = JwtHelper.GenerateJwtToken(model.Email, SecretKey, 60); // Örnek: 60 gun geçerli bir token oluşturuyoruz.

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
                return BadRequest("Invalid email or password");
            }

            String password = PasswordHasher.HashPassword(model.Password);

            if (model.Email == user.Email && password == user.Password)
            {
                string token = JwtHelper.GenerateJwtToken(model.Email, SecretKey, 60); // Örnek: 60 gun geçerli bir token oluşturuyoruz.

                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // HTTPS üzerinde çalıştığınızda true olarak ayarlanmalıdır.
                    SameSite = SameSiteMode.Strict, // Güvenlik için SameSiteMode kullanılabilir.
                    Expires = DateTime.UtcNow.AddDays(15) // Örnek: Tokenin geçerlilik süresi 15 gün.
                });
                
                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest("Invalid email or password");
            }
        }
    }
}
