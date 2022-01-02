using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Enums;
using Microsoft.EntityFrameworkCore;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Services
{
    public class ReportServices
    {
        private readonly DBContext _dbContext;
        public ReportServices(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OrderReport CreateGeneralReport(DateTime startDate, DateTime endDate, List<OrderStatus> statuses, List<int> usersId)
        {
            if (DateTime.Compare(startDate, endDate) < 0)
            {
                return null;
            }
            List<Order> orders = _dbContext.Orders.Where(o => o.CreationDate >= startDate && o.FinishedDate <= endDate).ToList();
            for (int i=0; i < statuses.Count; i++)
            {
                for (int j=0; j < usersId.Count; j++)
                {
                    orders.RemoveAll(o => o.Status != statuses[i] || o.UserId != usersId[j]);
                }
            }

            int totalCompleted = 0;
            int totalCanceled = 0;
            decimal ordersTotalValue = 0;
            for (int i=0; i < orders.Count; i++)
            {
                if(orders[i].Status == OrderStatus.Completed) { totalCompleted++; }
                if(orders[i].Status == OrderStatus.Canceled) { totalCanceled++; }
                ordersTotalValue += orders[i].TotalValue;
            }

            return new OrderReport
            {
                FinishedOrdersAmount = totalCompleted,
                CancelledOrdersAmount = totalCanceled,
                OrdersTotalValue = ordersTotalValue,
                Orders = orders
            };
        }
    }
}
