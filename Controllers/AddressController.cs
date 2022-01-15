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
    //[Authorize(Roles = "Funcionario, Gerente, Admin")]
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
        public IActionResult GetAddresses()        //Lista Todos os Endereços
        {
            if(_addressServices.ListAddress().Count == 0)
            {
                return Ok("Nenhum Endereço Encontrado !!!");
            }
            return Ok(_addressServices.ListAddress());
        }

        [HttpGet("{idAddress}")]
        public IActionResult GetById(int idAddress)    //Retorna Endereço pelo Id
        {
            if (_addressServices.GetAddressById(idAddress) is null)
            {
                return Ok("Endereço NÃO encontrado!");
            }
            return Ok(_addressServices.GetAddressById(idAddress));
        }

        [HttpGet("street/{street}")]
        public IActionResult GetByStreet(string street)    //Retorna Endereços pelo Nome da Rua
        {
            if(_addressServices.GetAddressByStreet(street).Count == 0)
            {
                return Ok("Nenhum Endereço cadastrado nessa Rua !");
            }
            return Ok(_addressServices.GetAddressByStreet(street));
        }

        [HttpGet("neighborhood/{neighborhood}")]
        public IActionResult GetByNeighborhood(string neighborhood)    //Retorna Endereços pelo Bairro
        {
            if (_addressServices.GetAddressByNeighborhood(neighborhood).Count == 0)
            {
                return Ok("Nenhum Endereço cadastrado nesse Bairro !");
            }
            return Ok(_addressServices.GetAddressByNeighborhood(neighborhood));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Address address)
        {
            if (_addressServices.CreateAddress(address))
            {
                return Ok(address);
            }
            return Ok("Endereço Já Cadastrado !");

        }
                
        [HttpPut]
        public IActionResult Update([FromBody] Address address)
        {
            if (_addressServices.GetAddressById(address.Id) is null)
            {
                return BadRequest("Endereço NÃO encontrado");
            }
            return Ok(_addressServices.UpdateAddress(address));
            
        }
        
        [HttpDelete("{idAddress}")]
        public IActionResult Delete(int idAddress)
        {
            if (_addressServices.DeleteAddress(idAddress))
            {
                return Ok($"O Endereço de ID: {idAddress} Removido com Sucesso !");
            }
            else
            {
                return BadRequest($"Endereço de ID: {idAddress} NÃO Encontrado !");
            }
        }

        // users/search?term="test"&page=1
        [HttpGet("search")]
        //Lista endereços que contenham o 'term' em sua Rua, Número, Bairro, Cidade ou Estado... Paginando a partir do endereço 'offset', listando de 'limit' em 'limit' endereços

        public IActionResult Search([FromQuery] string searchTerm, int initialRecord = 0, int limitPerPage = 10)
        {
            {
                if (_addressServices.SearchAddress(searchTerm, initialRecord, limitPerPage).Count == 0)
                {
                    return Ok("Nenhum Endereço Encontrado para essa Pesquisa!");
                }
                return Ok(_addressServices.SearchAddress(searchTerm, initialRecord, limitPerPage));
            }
        }
    }
}
