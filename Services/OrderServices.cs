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

        public Order getOrder(int orderId)           //Retorna todos os ítens de um pedido
        {
            var order = _dbContext.Orders.Find(orderId);
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

        public bool updateCart(User user, Order order, Product product, int qtd)
        {
            Item item = new Item();
            order.UserId = user.Id;
            item.OrderId = order.Id;
            item.ProductId = product.Id;
            int qtdModel = product.Quantity;
            if(qtdModel < qtd) { return false; }
            item.Quantity = qtd;
            product.Quantity -= qtd;
            item.Price = product.Price * qtd;
            order.Items.Add(item);
            order.TotalValue += item.Price;
            order.Status = OrderStatus.InProgress;
            _dbContext.Add(item);
            _dbContext.SaveChanges();
            return true;
        }

        public bool downgradeCart(User user, Order order, Product product, int itemId)
        {
            Item itemModel = order.Items.Find(i => i.Id == itemId);
            if (itemModel == null)
            {
                return false;
            }
            product.Quantity += itemModel.Quantity;
            order.TotalValue -= product.Price * itemModel.Quantity;
            order.Items.Remove(itemModel);
            _dbContext.SaveChanges();
            return true;
        }

        public bool changeQuantity(User user, Order order, Product product, int itemId, int qtd)
        {
            Item itemModel = order.Items.Find(i => i.Id == itemId);
            if (itemModel == null)
            {
                return false;
            }
            int newQtd = itemModel.Quantity + qtd;
            if ((newQtd) < 0)
            {
                return false;
            }
            itemModel.Quantity += qtd;
            product.Quantity -= qtd;
            itemModel.Price += qtd * product.Price;
            order.TotalValue += itemModel.Price;
            _dbContext.SaveChanges();
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
                order.CancelDate = DateTime.Now;
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
                order.FinishedDate = DateTime.Now;
                _dbContext.SaveChanges();
                return true;
            }
        }

        public bool ChangePay(Order order, Payment pay)
        {
            if (order == null || order.Status == OrderStatus.Completed || order.Status == OrderStatus.Canceled)
            {
                return false;
            }

            if(pay.Equals(Payment.Cash)) { order.Payment = Payment.Cash; }
            if(pay.Equals(Payment.CreditCard)) { order.Payment = Payment.CreditCard; }
            if(pay.Equals(Payment.Slip)) { order.Payment = Payment.Slip; }

            _dbContext.SaveChanges();
            return true;
        }

        public List<Order> getOrdersByStatus(OrderStatus status)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.Status.Equals(status)).ToList();
        }

        public List<Order> getOrdersByPayment(Payment payment)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.Payment.Equals(payment)).ToList();
        }

        public List<Order> getOrdersByCriation(string criation)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.CreationDate.ToString().Contains(criation)).ToList();
        }

        public List<Order> getOrdersByCancellation(string cancellation)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.CancelDate.ToString().Contains(cancellation)).ToList();
        }

        public List<Order> getOrdersByFinalization(string finalization)
        {
            return _dbContext.Orders.Include(o => o.Items).Where(o => o.FinishedDate.ToString().Contains(finalization)).ToList();
        }
    }
}
