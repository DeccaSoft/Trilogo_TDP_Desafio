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
        public IActionResult GetUsers()        //Lista Todos os Usuários
        {
            if (_userServices.GetListUsers().Count == 0)
            {
                return Ok("Nenhum Usuário Cadastrado!");
            }
            return Ok(_userServices.GetListUsers());
        }
        
        [HttpGet("id/{id}")]  
        public IActionResult GetById(int id)    //Retorna Usuário pelo Id
        {
            if (_userServices.GetUserById(id) is null)
            {
                return Ok("Usuário NÃO Cadastrado!");
            }
            return Ok(_userServices.GetUserById(id));
        }

        [HttpGet("login/{login}")]
        public IActionResult GetByLogin(string login)    //Retorna Usuário pelo Login
        {
            if (_userServices.GetUserByLogin(login) is null)
            {
                return Ok("Usuário NÃO Cadastrado!");
            }
            return Ok(_userServices.GetUserByLogin(login));
        }

        //PLUS

        [HttpGet("{id}/orders")]
        public IActionResult GetUserOrders(int id)    //Retorna Usuário e Todos seus Pedidos pelo seu Id
        {
            if (_userServices.GetUserById(id) is null)
            {
                return Ok("Usuário NÃO Cadastrado!");
            }

            if (_userServices.GetUserWithOrders(id).Count == 0)
            {
                return Ok("Nenhum Pedido encontrado para o Usuário Especificado!");
            }
            return Ok(_userServices.GetUserWithOrders(id));
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] User user)    
        {
            if (_userServices.CreateUser(user))
            {
                return Ok(user);
            }
            return Ok("Usuário JÁ Cadastrado");
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            if (_userServices.GetUserById(user.Id) is null)
            {
                return BadRequest("Usuário NÃO Cadastrado");
            }
            return Ok(_userServices.UpdateUser(user));
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
        public IActionResult Search([FromQuery]string searchTerm, int initialRecord = 0, int limitPerPage = 10) 
        {
            if(_userServices.SearchUsers(searchTerm, initialRecord, limitPerPage).Count == 0)
            {
                return Ok("Nenhum Usuário encontrado para essa Pesquisa!");
            }
            return Ok(_userServices.SearchUsers(searchTerm, initialRecord, limitPerPage));
        }

        [HttpGet("cpf/{cpf}")]
        public IActionResult GetByCPF(string cpf)    //Retorna Usuário pelo CPF
        {
            if (_userServices.GetUserByCPF(cpf) is null)
            {
                return Ok("Usuário NÃO Cadastrado!");
            }
            return Ok(_userServices.GetUserByCPF(cpf));
        }

        [HttpGet("email/{email}")]
        public IActionResult GetByEmail(string email)    //Retorna Usuário pelo E-Mail
        {
            if (_userServices.GetUserByEmail(email) is null)
            {
                return Ok("Usuário NÃO Cadastrado!");
            }
            return Ok(_userServices.GetUserByEmail(email));
        }

        [HttpGet("birthday/{birthday}")]
        public IActionResult GetByBirthday(string birthday)    //Retorna Usuário pela Data de Nascimento
        {
            if (_userServices.GetUsersByBirthday(birthday).Count == 0)
            {
                return Ok("Nenhum Usuário Encontrado!");
            }
            return Ok(_userServices.GetUsersByBirthday(birthday));
        }
    }
}