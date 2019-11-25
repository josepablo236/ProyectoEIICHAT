using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JWTController : ControllerBase
    {

        public string GenerateToken(string jsontext, string key, string algorithm)
        {
            jsontext = jsontext.Replace("\r", String.Empty);
            jsontext = jsontext.Replace("\n", String.Empty);
            var sKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingcredential = new Microsoft.IdentityModel.Tokens.SigningCredentials(sKey, "HS256");
            //HEADER
            var header = new JwtHeader(signingcredential);
            //PAYLOAD
            var payload = new JwtPayload();
            payload = JwtPayload.Deserialize(jsontext);
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);
                          
            string token = tokenString;
            return token;
        }
    }
}
