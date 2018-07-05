using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Surgicalogic.Common.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Surgicalogic.Services.Services
{
    public static class TokenService
    {
        public static string GenerateToken(string email)
        {
            var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti,email)
                    };

            var token = new JwtSecurityToken
            (
                //issuer: _configuration["Token:Issuer"],
                //audience: _configuration["Token:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(AppSettings.TokenValidityPeriodInMinutes),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(AppSettings.TokenSecurityKey)),
                        SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            Thread.CurrentPrincipal = principal;
            if (validatedToken == null || validatedToken.ValidTo < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateActor = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                                                               (AppSettings.TokenSecurityKey))
            };
        }
    }
}
