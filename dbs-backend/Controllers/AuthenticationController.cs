using dbs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace dbs.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("sign-in")]
        public ActionResult<object> Authenticate([FromBody] LoginRequest login)
        {
            var loginResponse = new LoginResponse { };
            LoginRequest loginrequest = new()
            {
                UserName = login.UserName.ToLower(),
                Password = login.Password
            };

            bool isUsernamePasswordValid = false;

            if (login != null)
            {
                // make await call to the Database to check username and password. here we only checking if password value is admin
                isUsernamePasswordValid = loginrequest.UserName == "admin" & loginrequest.Password == "admin" ? true : false;
            }
            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                string token = CreateToken(loginrequest.UserName);

                loginResponse.Token = token;
                loginResponse.responseMsg = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                //return the token
                return Ok(new { loginResponse });
            }
            else
            {
                // if username/password are not valid send unauthorized status code in response               
                return BadRequest("Username or Password Invalid!");
            }
        }

        private string CreateToken(string username)
        {

            List<Claim> claims = new()
            {                    
                //list of Claims - we only checking username - more claims can be added.
                new Claim("username", Convert.ToString(username)),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                //expires: DateTime.Now.AddSeconds(20),
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}