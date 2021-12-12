using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace JWT.API
{
    public class TokenManager
    {

        private static string SecretKey = "!QAZ@WSX#EDC4rfv";

        public static string GenerateToken(string userName)
        {
            byte[] key = Convert.FromBase64String(SecretKey);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName, ClaimTypes.Email, "test@test.com") }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}