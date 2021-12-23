using LunarDeckFoxyApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LunarDeckFoxyApi.Authentication
{
    public class JwtTokenBuilder
    {
        private readonly IConfiguration _configuration;

        public JwtTokenBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Builds a JWT token using claims (email, phone number) from properties of the user obejct
        /// TODO: add <c>lunarId</c> as a claimtype
        /// </summary>
        /// <param name="user"></param>
        /// <returns>JWT token as string</returns>
        public string Build(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "0.0.0")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Auth:JwtIssuer").Value,
                audience: _configuration.GetSection("Auth:JwtIssuer").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: new SigningCredentials(new JwtSecurityKey(_configuration).Create(), SecurityAlgorithms.HmacSha256)
            );

            return tokenHandler.WriteToken(token);
        }
    }
}
