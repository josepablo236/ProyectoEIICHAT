using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JWT.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        // GET: api/User/5
        [HttpGet("{cadena}", Name = "Get")]
        public string Get(string cadena)
        {
            string[] data = cadena.Split('|');
            User userdata = new User();
            userdata.name = data[0];
            userdata.password = data[1];
            string jsontext = JsonConvert.SerializeObject(userdata);
            jsontext = jsontext.Replace("\r", String.Empty);
            jsontext = jsontext.Replace("\n", String.Empty);
            var sKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(data[2]));
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
    public class User
    {
        public string name;
        public string password;
    }
}
