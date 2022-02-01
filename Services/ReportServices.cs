using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Enums;
using Aula6.Interfaces;
using Microsoft.EntityFrameworkCore;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Services
{
    public class ReportServices : IReportService
    {
        private readonly DBContext _dbContext;
        public ReportServices(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OrderReport CreateGeneralReport(DateTime startDate, DateTime endDate, List<OrderStatus> statuses, List<int> usersId)
        {
            if (endDate < startDate)
            {
                return null;
            }

            List<Order> orders = _dbContext.Orders.Where(o => o.CreationDate >= startDate && o.CreationDate <= endDate
                && usersId.Contains(o.UserId) && statuses.Contains(o.Status)).ToList();

            return new OrderReport
            {
                FinishedOrdersAmount = orders.Count(o => o.Status == OrderStatus.Completed),
                CancelledOrdersAmount = orders.Count(o => o.Status == OrderStatus.Canceled),
                OrdersTotalValue = orders.Sum(o => o.TotalValue),
                Orders = orders
            };

           
        }
    }
}
