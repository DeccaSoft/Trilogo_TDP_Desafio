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

        public OrderReport CreateGeneralReport(DateTime startDate, DateTime endDate, List<OrderStatus> statusesSearch, List<string> usersSearch)
        {
            if (DateTime.Compare(startDate, endDate) < 0)
            {
                return null;
            }

            List<Order> orders = _dbContext.Orders.Include(o => o.User).Where(o => o.CreationDate >= startDate && o.CreationDate <= endDate).ToList();
            List<Order> OrdersSearch = new List<Order>();

            if (usersSearch == null)
            {
                return null;
            }

            usersSearch.ForEach(userLogin => OrdersSearch = OrdersSearch.Union(orders.Where(o => o.User.Login == userLogin).ToList()).ToList());
            orders = OrdersSearch;
            OrdersSearch = new List<Order>();

            if (statusesSearch == null)
            {
                return null;
            }
            statusesSearch.ForEach(status => OrdersSearch = OrdersSearch.Union(orders.Where(o => o.Status == status).ToList()).ToList());
            orders = OrdersSearch;

            int totalCompleted = orders.Aggregate(0, (sum, order) => order.Status == OrderStatus.Completed ? sum + 1 : sum);
            int totalCanceled = orders.Aggregate(0, (sum, order) => order.Status == OrderStatus.Canceled ? sum + 1 : sum);
            decimal ordersTotalValue = orders.Aggregate(0m, (sum, order) => sum + order.TotalValue);
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
