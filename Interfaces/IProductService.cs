using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treinando.Models;

namespace Aula6.Interfaces
{
    public interface IProductService
    {
        List<Product> ListProducts();
        Product GetProductById(int id);
        List<Product> GetProductByName(string name);
        bool CreateProduct(Product product);
        Product UpdateProduct(Product product);
        bool DeleteProduct(int id);
        List<Product> SearchProducts(string searchTerm, int initialRecord, int limitPerPage);
        int GetProductInventory(int id);
        List<Product> ListProductsWithStockBelowTheMinimum();
        List<Product> ListProductsPricedUpToPrice(decimal price);

    }
}
