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
    public class OrderServices
    {
        private readonly DBContext _dbContext;
        public OrderServices(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Order> getOrders(User user)     //Retorna todos os pedidos do usuário
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.UserId == user.Id).ToList();
        }

        public Order getOrder(int id)           //Retorna todos os ítens de um pedido
        {
            var order = _dbContext.Orders.Find(id);
            return order;
        }

        public Order GetUnfinishedOrder(User user)
        {
            return _dbContext.Orders.Include(o => o.Items).FirstOrDefault(o => o.UserId == user.Id && (o.Status == OrderStatus.Open || o.Status == OrderStatus.InProgress));
        }
        public Order createCart(User user)
        {
            Order orderModel = new Order();
            orderModel.UserId = user.Id;
            _dbContext.Orders.Add(orderModel);
            _dbContext.SaveChanges();
            return orderModel;
        }

        public Item updateCart(Order order, Product product, int qtd)
        {
            Item item = new Item();
            order.Items.Add(item);
            item.OrderId = order.Id;
            item.ProductId = product.Id;
            item.Quantity = qtd;
            item.Price = product.Price * qtd;
            _dbContext.Add(item);
            _dbContext.SaveChanges();
            return item;
        }

        public bool downgradeCart(Order order, int prodId)
        {
            Item itemModel = order.Items.FirstOrDefault(i => i.ProductId == prodId);
            if (itemModel == null)
            {
                return false;
            }
            order.Items.Remove(itemModel);
            order.TotalValue -= itemModel.Price * itemModel.Quantity;
            return true;
        }

        public bool changeQuantity(Order order, int prodId, int qtd)
        {
            Item itemModel = order.Items.FirstOrDefault(i => i.ProductId == prodId);
            if (itemModel == null)
            {
                return false;
            }
            itemModel.Quantity += qtd;
            if(itemModel.Quantity < 0)
            {
                return false;
            }
            order.TotalValue += itemModel.Price * itemModel.Quantity;
            return true;
        }

        public bool isCancelled(Order order)
        {
            if (order == null || order.Status == OrderStatus.Completed)
            {
                return false;
            }
            else
            {
                order.Status = OrderStatus.Canceled;
                _dbContext.SaveChanges();
                return true;
            }
        }

        public bool isFinished(Order order)
        {
            if (order == null || order.Status == OrderStatus.Completed || order.Status == OrderStatus.Canceled)
            {
                return false;
            }
            else
            {
                order.Status = OrderStatus.Completed;
                _dbContext.SaveChanges();
                return true;
            }
        }

        public List<Order> getOrdersByStatus(string status)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.Status.Equals(status)).ToList();
        }

        public List<Order> getOrdersByPayment(string payment)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.Status.Equals(payment)).ToList();
        }

        public List<Order> getOrdersByCriation(DateTime criation)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.CreationDate.Equals(criation)).ToList();
        }

        public List<Order> getOrdersByCancellation(DateTime cancellation)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.CancelDate.Equals(cancellation)).ToList();
        }

        public List<Order> getOrdersByFinalization(DateTime finalization)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.FinishedDate.Equals(finalization)).ToList();
        }
    }
}
