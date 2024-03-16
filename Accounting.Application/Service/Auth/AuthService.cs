using Accounting.Application.Service.Auth.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ServiceResponse<TokenInfo> GenerateToken(List<Claim> claims)
        {
            var expiresInMinutes = _configuration["JwtSettings:ExpiresInMinutes"];
            if (string.IsNullOrWhiteSpace(expiresInMinutes))
            {
                return new ServiceResponse<TokenInfo>(null, false, "Jwt Settings are not set up");
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, "HS256");
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expiresInMinutes));
            var issuer = _configuration["JwtSettings:Issuer"];
            JwtSecurityToken token = new JwtSecurityToken(issuer, issuer, claims, null, expires, signingCredentials);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return new ServiceResponse<TokenInfo>(new TokenInfo
            {
                Token = tokenStr,
                Expire = expires
            }, true, string.Empty);
        }

    }
}
