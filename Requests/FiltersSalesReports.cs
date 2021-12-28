using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Enums;
using Treinando.Models;

namespace Aula6.Requests
{
    public class FiltersSalesReports
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<OrderStatus> Statuses { get; set; }
        public List<string> Users { get; set; }
    }
}
