using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlowWing.API.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(int userId, string email, string secretKey, int expiryInDays)
        {
            string StringUserId = userId.ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.NameIdentifier, StringUserId),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddDays(expiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool TokenIsValid(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenValidationParameters = GetTokenValidationParameters(secretKey);
            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static (string?, string?) GetJwtPayloadInfo(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                // token geçerli değilse veya boşsa hata işleme alınabilir
                return (null,null);
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                // Token payload (veri) bilgileri
                var payload = jsonToken.Payload;

                // Payload'daki istediğiniz bilgilere erişebilirsiniz
                var email = payload["email"];
                var userId = payload["nameid"];
                string StringUserId = userId.ToString();
                string StringEmail = email.ToString();
                return (StringEmail, StringUserId);
            }
            else
            {
                return (null,null);
            }
        }

        public static TokenValidationParameters GetTokenValidationParameters(string secretKey)
            {
                return new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            
        }
    }
}