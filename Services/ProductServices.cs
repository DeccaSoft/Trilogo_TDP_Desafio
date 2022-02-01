using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Interfaces;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Services
{
    public class ProductServices : IProductService
    {
        private readonly DBContext _dbContext;
        public ProductServices(DBContext dbContext)
        {
            _dbContext = dbContext;
            //_dbContext.Database.EnsureCreated();
        }
        public List<Product> ListProducts()        //Lista Todos os Produtos
        {
            return _dbContext.Products.ToList();
        }

        public Product GetProductById(int id)    //Retorna Produto pelo Id
        {
            return _dbContext.Products.Find(id);
        }

        public List<Product> GetProductByName(string name)    //Retorna Produto pelo Nome
        {
            return _dbContext.Products.Where(n => n.Name.Equals(name)).ToList();
        }

        public bool CreateProduct(Product product)          //Cadastra um Produto
        {
            if (_dbContext.Products.FirstOrDefault(p => p.Id.Equals(product.Id)) != null
                || _dbContext.Products.FirstOrDefault(p => p.Name.Equals(product.Name) && p.Description.Equals(product.Description)
                    && p.Quantity.Equals(product.Quantity) && p.Price.Equals(product.Price) && p.StockMinimum.Equals(product.StockMinimum)) != null)
            {
                return false;
            }
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return true;
        }

        public Product UpdateProduct(Product product)       //Edita um Produto
        {
            var productModel = _dbContext.Products.Find(product.Id);
            //_dbContext.Products.Update(product);
            _dbContext.Entry(productModel).CurrentValues.SetValues(product);
            _dbContext.SaveChanges();
            return product;
        }

        public bool DeleteProduct(int id)       //Apaga um Produto
        {
            var product = _dbContext.Products.Find(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        //Lista produtos que contenham o 'term' em seu Nome,ou Descrição... Paginando a partir do produto 'offset', listando de 'limit' em 'limit' produtos
        public List<Product> SearchProducts(string searchTerm, int initialRecord, int limitPerPage)
        {
            List<Product> products = _dbContext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).Skip(initialRecord).Take(limitPerPage).ToList();
            return products;
        }

        //PLUS

        //Lista Quantidade de um produto em estoque
        public int GetProductInventory(int id)    //Retorna Estoque do Produto pelo Id
        {
            return _dbContext.Products.Find(id).Quantity;
        }

        //Listar Produtos com estoque abaixo do Mínimo
        public List<Product> ListProductsWithStockBelowTheMinimum()        //Lista Todos os Produtos com estoque abaixo do Mínimo
        {
            return _dbContext.Products.Where(p => p.Quantity <= p.StockMinimum).ToList();
        }

        //Listar produto por faixa de preço
        public List<Product> ListProductsPricedUpToPrice(decimal price)        //Lista Todos os Produtos com preço menor ou igual a 'price'
        {
            return _dbContext.Products.Where(p => p.Price <= price).ToList();
        }
    }
}
