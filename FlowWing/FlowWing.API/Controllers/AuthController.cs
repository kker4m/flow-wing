using FlowWing.API.Helpers;
using FlowWing.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using FlowWing.Business.Abstract;
using FlowWing.Entities;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;

namespace FlowWing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        public AuthController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
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
                    IsApplicationUser = false,
                    LastLoginDate = DateTime.UtcNow,
                    CreationDate = DateTime.UtcNow
                };


                User user = await _userService.CreateUserAsync(newUser);
                string token = JwtHelper.GenerateJwtToken(user.Id, user.Email, _appSettings.SecretKey, 30);
                UserResponseModel response = new UserResponseModel
                {
                    Message = "Kayit Basarili",
                    Email = user.Email,
                    Username = user.Username,
                    Token = token
                };

                return Ok(response);
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
                return NotFound();
            }
            String password = PasswordHasher.HashPassword(model.Password);

            if (model.Email == user.Email && password == user.Password)
            {
                string token = JwtHelper.GenerateJwtToken(user.Id, user.Email, _appSettings.SecretKey, 30);
                UserResponseModel response = new UserResponseModel
                {
                    Message = "Giris Basarili",
                    Email = user.Email,
                    Username = user.Username,
                    Token = token
                };

                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                });

                return Ok(response);

            }
            else
            {
                return BadRequest("Yanlis email veya sifre");
            }
        }


        /// <summary>
        /// Arcelik sicili ile kullanici giriş işlemleri burada gerçekleştirilir
        /// </summary>
        /// <param name="sicil"></param>
        /// <returns></returns>
        [HttpPost("loginWithArcelikID/{sicil}")]
        public async Task<IActionResult> LoginWithArcelikID(string sicil)
        {
            // Kullanıcı giriş işlemleri burada gerçekleştirilir
            User user = await _userService.GetUserByEmailAsync(sicil + "@arcelik.com");
            if (user == null)
            {
                //create user with arcelik id
                User newUser = new User
                {
                    Email = sicil + "@arcelik.com",
                    Password = PasswordHasher.HashPassword(sicil),
                    Username = sicil,
                    IsApplicationUser = false,
                    LastLoginDate = DateTime.UtcNow,
                    CreationDate = DateTime.UtcNow
                };

                User createdUser = await _userService.CreateUserAsync(newUser);
                string token = JwtHelper.GenerateJwtToken(createdUser.Id, createdUser.Email, _appSettings.SecretKey, 30);
                UserResponseModel response = new UserResponseModel
                {
                    Message = "Giris Basarili",
                    Email = createdUser.Email,
                    Username = createdUser.Username,
                    Token = token
                };

                return Ok(response);

            }
            else
            {
                string email = sicil + "@arcelik.com";

                if (email == user.Email)
                {
                    string token = JwtHelper.GenerateJwtToken(user.Id, user.Email, _appSettings.SecretKey, 30);
                    UserResponseModel response = new UserResponseModel
                    {
                        Message = "Giris Basarili",
                        Email = user.Email,
                        Username = user.Username,
                        Token = token
                    };

                    Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = false,
                        SameSite = SameSiteMode.Strict
                    });

                    return Ok(response);

                }
                else
                {
                    return BadRequest("Yanlis email veya sifre");
                }

            }
        }
    }
}
