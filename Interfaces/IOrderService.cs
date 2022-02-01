using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Enums;
using Treinando.Models;

namespace Aula6.Interfaces
{
    public interface IOrderService
    {
        List<Order> getOrders(User user);
        Order getOrder(int orderId);
        Order GetUnfinishedOrder(User user);
        Order createCart(User user);
        bool updateCart(User user, Order order, Product product, int qtd);
        bool downgradeCart(User user, Order order, Product product, int itemId);
        bool changeQuantity(User user, Order order, Product product, int itemId, int qtd);
        bool isCancelled(Order order);
        bool isFinished(Order order);
        bool ChangePay(Order order, Payment pay);
        List<Order> getOrdersByStatus(OrderStatus status);
        List<Order> getOrdersByPayment(Payment payment);
        List<Order> getOrdersByCriation(string criation);
        List<Order> getOrdersByCancellation(string cancellation);
        List<Order> getOrdersByFinalization(string finalization);

    }
}
