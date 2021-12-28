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
    //[Authorize(Roles = "Gerente")]
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
        public List<Product> GetProducts()        //Lista Todos os Produtos
        {
            return _productServices.ListProducts();
        }

        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)    //Retorna Produto pelo Id
        {
            return Ok(_productServices.GetProductById(id));
        }

        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)    //Retorna Produto pelo Nome
        {
            return Ok(_productServices.GetProductByName(name));
        }

        [HttpPost]
        public Product Create([FromBody] Product product)   
        {
            return _productServices.CreateProduct(product);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Product product)   
        {
            if (product == null)
            {
                return BadRequest("Produto NÃO encontrado");
            }
            else
            {
                return Ok(_productServices.UpdateProduct(product));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)        
        {
            if (_productServices.DeleteProduct(id))
            {
                return Ok($"O Produto de ID: {id} Removido com Sucesso !");
            }
            else
            {
                return BadRequest($"Produto de ID: {id} NÃO Encontrado !");
            }
        }

        // users/search?term="test"&page=1
        [HttpGet("search")]
        //Lista produtos que contenham o 'term' em seu Nome,ou Descrição... Paginando a partir do produto 'offset', listando de 'limit' em 'limit' produtos

        public List<Product> Search([FromQuery] string term, int offset, int limit)   
        {
            return _productServices.SearchProducts(term, offset, limit);
        }

        //PLUS

        //Lista Quantidade de um produto em estoque
        [HttpGet("inventory/{id}")]
        public int GetInventory(int id)    //Retorna Estoque do Produto pelo Id
        {
            return _productServices.GetProductInventory(id);
        }

        //Listar Produtos com estoque abaixo do Mínimo
        [HttpGet("stock")]
        public List<Product> ListStocksBelowTheMinimum()        //Lista Todos os Produtos com estoque abaixo do Mínimo
        {
            return _productServices.ListProductsWithStockBelowTheMinimum();
        }

        //Listar produto por faixa de preço (R$ 0.00 até 'price')
        [HttpGet("price/{price}")] // /products/price/100
        public List<Product> ListProductsPricedUpToXValue(decimal price)        //Lista Todos os Produtos com preço até o valor x 'price' predeterminado
        {
            return _productServices.ListProductsPricedUpToPrice(price);
        }
    }
}
