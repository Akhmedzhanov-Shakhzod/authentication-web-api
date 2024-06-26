﻿using AuthenticationWebApi.Helpers.Constant;
using AuthenticationWebApi.Mappers.Account;
using AuthenticationWebApi.Models.Account;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationWebApi.Helpers.Jwt.Impl
{
    public class DefaultJwtUtils(IOptions<AppSettings> appSettings, IRoleMapper roleMapper) : IJwtUtils
    {
        public string GenerateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("id", account.Id),
                    new Claim("roles", string.Join(",", roleMapper.ToRoles(account)))
                }),
                Expires = DateTime.Now.AddMinutes(appSettings.Value.JwtTokenTTL),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public (string?, List<string>?) ValidateJwtToken(string token)
        {
            if (token is null)
                return (null, null);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Value.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;
                var accountRoles = jwtToken.Claims.First(x => x.Type == "roles").Value.Split(',').ToList();

                // return account id from JWT token if validation successful
                return (accountId, accountRoles);
            }
            catch
            {
                // return null if validation fails
                return (null, null);
            }
        }
    }
}
