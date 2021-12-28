using Treinando.Data;
using Treinando.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Treinando.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set;}

        public decimal Price { get; set; }

        //Plus
        public int StockMinimum { get; set; }
    }
}