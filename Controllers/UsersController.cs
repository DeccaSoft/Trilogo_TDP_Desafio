using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Treinando.Models;
using Treinando.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aula6.Services;
using Microsoft.AspNetCore.Authorization;

namespace Treinando.Controllers
{
    [ApiController]
    [Route("/users")]
    //[Authorize(Roles = "Gerente, Funcionario")]
    public class UsersController : ControllerBase
    {
        private readonly UserServices _userServices;
        private readonly DBContext _dbContext;
        //Injeção de Dependências
        public UsersController(DBContext dbContext, UserServices userServices)
        {
            _dbContext = dbContext;
            _userServices = userServices;
        }

        [HttpGet]
        public List<User> GetUsers()        //Lista Todos os Usuários
        {
            return _userServices.GetListUsers();
        }
        
        [HttpGet("id/{id}")]  
        public User GetById(int id)    //Retorna Usuário pelo Id
        {
            return _userServices.GetUserById(id);
        }

        [HttpGet("login/{login}")]
        public IActionResult GetByLogin(string login)    //Retorna Usuário pelo Login
        {
            return Ok(_userServices.GetUserByLogin(login));
        }

        //PLUS

        [HttpGet("{login}/orders")]
        public IActionResult GetUserOrders(string login)    //Retorna Usuário e Todos seus Pedidos pelo Login
        {
            return Ok(_userServices.GetUserWithOrders(login));
        }
        
        [HttpPost]
        public User Create([FromBody] User user)    
        {
            return _userServices.CreateUser(user);
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)   
        {
            if (user == null)
            {
                return BadRequest("Usuário NÃO encontrado");
            }
            else
            {
                return Ok(_userServices.UpdateUser(user));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)        
        {
            if (_userServices.DeleteUser(id))
            {
                return Ok($"Usuário de ID: {id} Removido com Sucesso !");
            }
            else
            {
                return BadRequest($"Usuário de ID: {id} NÃO Encontrado ou Possui Algum Pedido Registrado em seu Nome!");
            }
        }
        
        // users/search?term="test"&page=1
        [HttpGet("/search")]
        //Lista usuários que contenham o 'term' em seu Nome, Login ou Email... Paginando a partir do usuário 'offset', listando de 'limit' em 'limit' usuários
        public List<User> Search([FromQuery]string term, int offset, int limit) 
        {
            return _userServices.SearchUsers(term, offset, limit);
        }

    }
}