using Treinando.Data;
using Treinando.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Treinando.Models
{
    public class Item
    {
        public int Id{get; set;}
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; } = 0;
    }
}