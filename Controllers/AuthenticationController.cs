using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
//using Aula6.Data;
//using Aula6.Models;
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
        public IActionResult Login(LoginRequest loginRequest)   //Recebe E-mail e Senha
        {
            //var user = new User{Email = loginRequest.Email, Password = loginRequest.Password};
            var user = _DBContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email); //Checa E-Mail e Senha e Retorna o Usuário

            if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return BadRequest("Usuário ou senha inválido(s)");
            }
            
            var token = GenerateToken(user);    //Caso usuário exista, Gera o Token

            return Ok(token);                   //Retorna o Token ao usuário
        }

        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();               //Objeto JWT para o acesso ao Token
            var key = Encoding.ASCII.GetBytes("chave-autenticacao-tdp");    //Transformaa Chave de String para Bytes (Array)
            var tokenDescriptor = new SecurityTokenDescriptor
            {                                                               //Informações do Token
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                    //new Claim("orders", "c-r-u-d")


                }),
                Expires = DateTime.UtcNow.AddHours(1),                      //Tempo de Validade do Token (1Hora)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //Tipo de Algorítimo da Assinatura - Sha256
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);          //Cria o Token
            return tokenHandler.WriteToken(token);                          //Transforma o Token em String para ser Retornado
        }
    }
}
