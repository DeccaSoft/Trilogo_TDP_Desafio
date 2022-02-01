using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Enums;
using Treinando.Models;

namespace Aula6.Interfaces
{
    public interface IReportService
    {
        public OrderReport CreateGeneralReport(DateTime startDate, DateTime endDate, List<OrderStatus> statuses, List<int> usersId);

    }
}
