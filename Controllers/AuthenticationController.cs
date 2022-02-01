using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Aula6.Requests;
using Auth0.ManagementApi.Models.Rules;
using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Treinando.Data;
using Treinando.Models;
using LoginRequest = Aula6.Requests.LoginRequest;

namespace Aula6.Controllers
{
    [ApiController]
    [Route("authentication")]

    public class AuthenticationController : ControllerBase
    {
        private readonly DBContext _DBContext;

        public AuthenticationController(DBContext DBContext)
        {
            _DBContext = DBContext;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest loginRequest)   
        {
           
            var user = _DBContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email); 

            if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return BadRequest("Usuário ou senha inválido(s)");
            }
            
            var token = GenerateToken(user);    

            return Ok(token);                   
        }

        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();               
            var key = Encoding.ASCII.GetBytes("chave-autenticacao-tdp");    
            var tokenDescriptor = new SecurityTokenDescriptor
            {                                                               
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                    

                }),
                Expires = DateTime.UtcNow.AddHours(1),                     
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) 
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);          
            return tokenHandler.WriteToken(token);                          
        }
    }
}
