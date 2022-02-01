using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Treinando.Data;
using Treinando.Controllers;

namespace Treinando.Models
{
    public class OrderReport
    {
        public int Id{get; set;}
        [Required]
        public decimal FinishedOrdersAmount { get; set; }
        [Required]
        public decimal CancelledOrdersAmount { get; set; }
        [Required]
        public decimal OrdersTotalValue { get; set; }

        [Required]
        public List<Order> Orders { get; set; }

       
    }
}