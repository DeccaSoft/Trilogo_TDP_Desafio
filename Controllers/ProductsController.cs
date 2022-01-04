using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Controllers
{
    [ApiController]
    [Route("/products")]
    [Authorize(Roles = "Gerente")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductServices _productServices;
        private readonly DBContext _dbContext;
        //Injeção de Dependências
        public ProductsController(DBContext dbContext, ProductServices productServices)
        {
            _dbContext = dbContext;
            _productServices = productServices;
        }

        [HttpGet]
        public IActionResult GetProducts()        //Lista Todos os Produtos
        {
            if(_productServices.ListProducts().Count == 0)
            {
                return Ok("Nenhum Produto Cadastro !");
            }
            return Ok(_productServices.ListProducts());
        }

        [HttpGet("id/{prodId}")]
        public IActionResult GetById(int prodId)    //Retorna Produto pelo Id
        {
            if (_productServices.GetProductById(prodId) is null)
            {
                return Ok("Produto NÃO encontrado!");
            }
            return Ok(_productServices.GetProductById(prodId));
        }

        [HttpGet("name/{prodName}")]
        public IActionResult GetByName(string prodName)    //Retorna Produto pelo Nome
        {
            if (_productServices.GetProductByName(prodName).Count == 0)
            {
                return Ok("Nenhum Produto Encontrado com este Nome!");
            }
            return Ok(_productServices.GetProductByName(prodName));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Product product)   
        {
            if (_productServices.CreateProduct(product))
            {
                return Ok(product);
            }
            return Ok("Produto JÁ Cadastrado !");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Product product)   
        {
            if(_productServices.GetProductById(product.Id) is null)
            {
                return BadRequest("Produto NÃO Cadastrado");
            }
            return Ok(_productServices.UpdateProduct(product));
        }

        [HttpDelete("{prodId}")]
        public IActionResult Delete(int prodId)        
        {
            if (_productServices.DeleteProduct(prodId))
            {
                return Ok($"O Produto de ID: {prodId} Removido com Sucesso !");
            }
            else
            {
                return BadRequest($"Produto de ID: {prodId} NÃO Encontrado !");
            }
        }

        // users/search?term="test"&page=1
        [HttpGet("search")]
        //Lista produtos que contenham o 'term' em seu Nome,ou Descrição... Paginando a partir do produto 'offset', listando de 'limit' em 'limit' produtos

        public IActionResult Search([FromQuery] string searchTerm, int initialRecord = 0, int limitPerPage = 10)   
        {
            if(_productServices.SearchProducts(searchTerm, initialRecord, limitPerPage).Count == 0)
            {
                return Ok("Nenhum Produto Encontrado para essa Pesquisa!");
            }
            return Ok(_productServices.SearchProducts(searchTerm, initialRecord, limitPerPage));
        }

        //PLUS

        //Lista Quantidade de um produto em estoque
        [HttpGet("inventory/{prodId}")]
        public IActionResult GetInventory(int prodId)    //Retorna Estoque do Produto pelo Id
        {
            if(_productServices.GetProductById(prodId) is null)
            {
                return Ok("Produto NÃO Encontrado!");
            }
            return Ok("Total em Estoque: " + _productServices.GetProductInventory(prodId));
        }

        //Listar Produtos com estoque abaixo do Mínimo
        [HttpGet("stockBelowMin")]
        public IActionResult ListStocksBelowTheMinimum()        //Lista Todos os Produtos com estoque abaixo do Mínimo
        {
            if(_productServices.ListProductsWithStockBelowTheMinimum().Count == 0)
            {
                return Ok("Nenhum Produto com Estoque Abaixo do Mínimo");
            }
            return Ok(_productServices.ListProductsWithStockBelowTheMinimum());
        }

        //Listar produto por faixa de preço (R$ 0.00 até 'price')
        [HttpGet("price/{price}")] // /products/price/100
        public IActionResult ListProductsPricedUpToXValue(decimal price)        //Lista Todos os Produtos com preço até o valor x 'price' predeterminado
        {
            if (_productServices.ListProductsPricedUpToPrice(price).Count == 0)
            {
                return Ok("Nenhum Produto encontrado com Valor Menor ou Igual ao Pesquisado");
            }
            return Ok(_productServices.ListProductsPricedUpToPrice(price));
        }
    }
}
