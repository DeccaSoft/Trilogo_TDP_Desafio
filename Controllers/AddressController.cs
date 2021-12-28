using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Controllers
{
    [ApiController]
    [Route("/address")]
    //[Authorize(Roles = "Funcionario, Gerente")]
    public class AddressController : ControllerBase
    {
        private readonly AddressServices _addressServices;
        private readonly DBContext _dbContext;
        //Injeção de Dependências
        public AddressController(DBContext dbContext, AddressServices addressServices)
        {
            _dbContext = dbContext;
            _addressServices = addressServices;
        }

        [HttpGet]
        public List<Address> GetAddresses()        //Lista Todos os Endereços
        {
            return _addressServices.ListAddress();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)    //Retorna Endereço pelo Id
        {
            return Ok(_addressServices.GetAddressById(id));
        }

        [HttpGet("street/{street}")]
        public IActionResult GetByStreet(string street)    //Retorna Endereços pelo Nome da Rua
        {
            return Ok(_addressServices.GetAddressByStreet(street));
        }

        [HttpGet("neighborhood/{neighborhood}")]
        public IActionResult GetByNeighborhood(string neighborhood)    //Retorna Endereços pelo Bairro
        {
            return Ok(_addressServices.GetAddressByNeighborhood(neighborhood));
        }

        [HttpPost]
        public Address Create([FromBody] Address address)
        {
            return _addressServices.CreateAddress(address);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Address address)
        {
            if (address == null)
            {
                return BadRequest("Endereço NÃO encontrado");
            }
            else
            {
                return Ok(_addressServices.UpdateAddress(address));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_addressServices.DeleteAddress(id))
            {
                return Ok($"O Endereço de ID: {id} Removido com Sucesso !");
            }
            else
            {
                return BadRequest($"Endereço de ID: {id} NÃO Encontrado !");
            }
        }

        // users/search?term="test"&page=1
        [HttpGet("search")]
        //Lista endereços que contenham o 'term' em sua Rua, Número, Bairro, Cidade ou Estado... Paginando a partir do endereço 'offset', listando de 'limit' em 'limit' endereços

        public List<Address> Search([FromQuery] string term, int offset, int limit)
        {
            return _addressServices.SearchAddress(term, offset, limit);
        }
    }
}
