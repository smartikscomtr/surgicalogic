using Microsoft.IdentityModel.Tokens;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Services;
using Surgicalogic.Data.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Surgicalogic.Services.Common
{
    public class TokenService : ITokenService
    {
        public object GenerateToken(string email, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.TokenSecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(AppSettings.TokenValidityPeriodInMinutes);

            var token = new JwtSecurityToken(
                AppSettings.Issuer,
                AppSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new { token = accessToken, refreshToken = GetRefreshToken(email, accessToken), expiresIn = expires };
        }

        public string GetEmailFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.TokenSecurityKey)),
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) || jwtSecurityToken.ValidTo < DateTime.UtcNow.AddHours(-1))
            {
                return string.Empty;
            }

            foreach (var item in principal.Claims)
            {
                if (item.Type == JwtRegisteredClaimNames.Sub)
                {
                    return item.Value;
                }
            }

            return string.Empty;
        }

        public string GetRefreshToken(string email, string token)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(email + token +  AppSettings.TokenSecurityKey);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
