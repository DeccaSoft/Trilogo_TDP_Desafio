using Treinando.Data;
using Treinando.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Treinando.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Quantity { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; } = 0;

        public int StockMinimum { get; set; } = 10;
    }
}