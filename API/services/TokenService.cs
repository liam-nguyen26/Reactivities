using Domain;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace API.services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Email, user.Email),
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])); //12 chars at least

            //signing by server
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //expiration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(10),
                SigningCredentials = creds,
            };

            var tokenHanlder = new JwtSecurityTokenHandler();

            var token = tokenHanlder.CreateToken(tokenDescriptor);

            return tokenHanlder.WriteToken(token);
        }
    }
}
