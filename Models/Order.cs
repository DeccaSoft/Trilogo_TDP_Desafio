using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Treinando.Data;
using Treinando.Controllers;
using Aula6.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Treinando.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalValue { get; set; } = 0;

        [Required]
        public List<Item>? Items { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;  //Padrão: Data da Criação do Pedido
        public DateTime? CancelDate { get; set; } = null;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Open;
        public DateTime? FinishedDate { get; set; } = null;

        public Payment Payment { get; set; } = Payment.Cash;
    }
}