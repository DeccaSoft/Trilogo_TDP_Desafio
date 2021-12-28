using System;
using System.Collections.Generic;
using Treinando.Data;
using Treinando.Controllers;
using Aula6.Enums;

namespace Treinando.Models
{
    public class OrderSearch
    {
        public int Id{get; set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<OrderStatus> Status { get; set; }
        public List<int> UserIds { get; set; }
    }
}