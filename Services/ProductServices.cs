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
           
        }
        public List<Product> ListProducts()        
        {
            return _dbContext.Products.ToList();
        }

        public Product GetProductById(int id)    
        {
            return _dbContext.Products.Find(id);
        }

        public List<Product> GetProductByName(string name)   
        {
            return _dbContext.Products.Where(n => n.Name.Equals(name)).ToList();
        }

        public bool CreateProduct(Product product)          
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

        public Product UpdateProduct(Product product)       
        {
            var productModel = _dbContext.Products.Find(product.Id);
            
            _dbContext.Entry(productModel).CurrentValues.SetValues(product);
            _dbContext.SaveChanges();
            return product;
        }

        public bool DeleteProduct(int id)       
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

        
        public int GetProductInventory(int id)   
        {
            return _dbContext.Products.Find(id).Quantity;
        }

       
        public List<Product> ListProductsWithStockBelowTheMinimum()        
        {
            return _dbContext.Products.Where(p => p.Quantity <= p.StockMinimum).ToList();
        }

       
        public List<Product> ListProductsPricedUpToPrice(decimal price)        
        {
            return _dbContext.Products.Where(p => p.Price <= price).ToList();
        }
    }
}
