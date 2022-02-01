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

            //Solução Antiga... Deixei aqui para estudo posterior. (Aprendendo com meus erros)

            /*
            if (DateTime.Compare(startDate, endDate) < 0)
            {
                return null;
            }
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
            */
        }
    }
}
