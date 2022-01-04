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
        private readonly AddressServices _addressServices;
        private readonly DBContext _dbContext;
        //Injeção de Dependências
        public UsersController(DBContext dbContext, UserServices userServices, AddressServices addressServices)
        {
            _dbContext = dbContext;
            _userServices = userServices;
            _addressServices = addressServices;
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
        
        [HttpGet("id/{userId}")]  
        public IActionResult GetById(int userId)    //Retorna Usuário pelo Id
        {
            if (_userServices.GetUserById(userId) is null)
            {
                return Ok("Usuário NÃO Cadastrado!");
            }
            return Ok(_userServices.GetUserById(userId));
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

        [HttpGet("{userId}/orders")]
        public IActionResult GetUserOrders(int userId)    //Retorna Usuário e Todos seus Pedidos pelo seu Id
        {
            if (_userServices.GetUserById(userId) is null)
            {
                return Ok("Usuário NÃO Cadastrado!");
            }

            if (_userServices.GetUserWithOrders(userId).Count == 0)
            {
                return Ok("Nenhum Pedido encontrado para o Usuário Especificado!");
            }
            return Ok(_userServices.GetUserWithOrders(userId));
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

        
        [HttpPut("user/{userId}/address/{addressId}")]
        public IActionResult UpdateUserAddress(int userId, int addressId)
        {
            if (_userServices.UpdateUserAddress(userId, addressId))
            {
                return Ok("Endereço Alterado com Sucesso!");
            }
            return Ok("Usuário ou Endereço NÃO cadastrado!");
        }
        

        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)        
        {
            if (_userServices.DeleteUser(userId))
            {
                return Ok($"Usuário de ID: {userId} Removido com Sucesso !");
            }
            else
            {
                return BadRequest($"Usuário de ID: {userId} NÃO Encontrado ou Possui Algum Pedido Registrado em seu Nome!");
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