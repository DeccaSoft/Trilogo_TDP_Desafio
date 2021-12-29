using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Treinando.Models;
using Treinando.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Aula6.Enums;
using Aula6.Services;
using System;

namespace Treinando.Controllers
{
    [ApiController]
    [Route("/orders")]
    //[Authorize(Roles = "Funcionario, Gerente")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderServices _orderServices;
        private readonly UserServices _userServices;
        private readonly DBContext _dbContext;
        //Injeção de Dependências
        public OrdersController(DBContext dbContext, OrderServices orderServices, UserServices userServices)
        {
            _dbContext = dbContext;
            _orderServices = orderServices;
            _userServices = userServices;
        }

        [HttpGet("userId/{userId}")]
        public IActionResult ListOrdersByUserId(int userId)             //Lista Pedidos do Usuário (Pega pelo Id do usuário)
        {
            var userModel = _userServices.GetUserById(userId);
            return Ok(_orderServices.getOrders(userModel));
        }

        [HttpGet("login/{login}")]
        public IActionResult ListOrdersByUserLogin(string login)        //Lista Pedidos do Usuário (Pega pelo login do usuário)
        {
            var userModel = _userServices.GetUserByLogin(login);
            return Ok(_orderServices.getOrders(userModel));
        }

        
        [HttpGet("order/{id}")]
        public IActionResult ListItemsByOrderId(int id)         //Lista os items de um pedido (pega pelo id do pedido)
        {
            var order = _orderServices.getOrder(id);

            if (order == null)
            {
                return BadRequest("Pedido Inexistente!");
            }
            else
            {
                return Ok(order.Items);
            }
        }

        //Se não houver pedido aberto -> Cria pedido em branco
        [HttpPost("{login}")]
        public IActionResult CreateOrderForUser(string login)
        {
            var userModel = _userServices.GetUserByLogin(login);
            if (_orderServices.GetUnfinishedOrder(userModel) == null)
            {
                return Ok(_orderServices.createCart(userModel));
            }
            else 
            {
                return Ok(_orderServices.GetUnfinishedOrder(userModel));
            }
            
        }
        //Se já Existir pedido -> Edita o pedido
        [HttpPut("{login}")]
        public IActionResult updateItem([FromBody] Product product, string login, int qtd)
        {
            User user = _userServices.GetUserByLogin(login);
            Order orderModel = _orderServices.GetUnfinishedOrder(user);
            return Ok(_orderServices.updateCart(orderModel, product, qtd));
        }

        [HttpPut("order/{id}")]
        public IActionResult downgradeItem(int userId, int prodId)
        {
            User user = _userServices.GetUserById(userId);
            Order orderModel = _orderServices.GetUnfinishedOrder(user);
            if(_orderServices.downgradeCart(orderModel, prodId))
            {
                return Ok("Produto Removido do Carrinho...");
            }
            else
            {
                return BadRequest("Este produto não se encontra nesse Carrinho...");
            }
        }

        //Edita Quantidades dos Ítens (Para mais) ou (para menos - bastando colocar sinal negativo)
        [HttpPut("item/{qtd}")]
        public IActionResult changeItemQuantity(int userId, int prodId, int qtd)
        {
            User user = _userServices.GetUserById(userId);
            Order orderModel = _orderServices.GetUnfinishedOrder(user);
            if (_orderServices.changeQuantity(orderModel, prodId, qtd))
            {
                return Ok("Produto Removido do Carrinho...");
            }
            else
            {
                return BadRequest("Este produto não se encontra nesse Carrinho...");
            }
        }

        //Cancelar Pedido
        [HttpDelete]
        [Route("cancel")]
        public IActionResult cancelOrder(int userId)
        {
            User user = _userServices.GetUserById(userId);
            Order order = _orderServices.GetUnfinishedOrder(user);
            if (_orderServices.isCancelled(order))
            {
                return Ok("Pedido Cancelado com Sucesso...");
            }
            else
            {
                return BadRequest("Pedido não existe ou já está finalizado! ");
            }
        }
        //Finlaizar Pedido
        [HttpPut]
        [Route("finish")]
        public IActionResult finishOrder(int userId)
        {
            User user = _userServices.GetUserById(userId);
            Order order = _orderServices.GetUnfinishedOrder(user);
            if (_orderServices.isFinished(order))
            {
                return Ok("Pedido Finalizado com Sucesso...");
            }
            else
            {
                return BadRequest("Pedido inexistente, cancelado ou já está finalizado! ");
            }
        }


        //RELATÓRIOS.........

        // Listar pedidos por Status
        [HttpGet("report/status/{status}")]
        public IActionResult ListOrdersByStatus(string status)
        {
            return Ok(_orderServices.getOrdersByStatus(status));
        }

        // Listar pedidos por Forma de Pagamento
        [HttpGet("report/payment/{payment}")]
        public IActionResult ListOrdersByPayment(string payment)
        {
            return Ok(_orderServices.getOrdersByPayment(payment));
        }

        // Listar Pedidos por Datas de...

        //Lista pedidos por data de Criação
        [HttpGet("report/criation/{criation}")]
        public IActionResult ListOrdersByCriation(DateTime criation)
        {
            return Ok(_orderServices.getOrdersByCriation(criation));
        }

        //Lista pedidos por data de Cancelamento
        [HttpGet("report/cancellation/{cancellation}")]
        public IActionResult ListOrdersByCancellation(DateTime cancellation)
        {
            return Ok(_orderServices.getOrdersByCancellation(cancellation));
        }

        //Lista pedidos por data de Finalização
        [HttpGet("report/finalization/{finalization}")]
        public IActionResult ListOrdersByFinalization(DateTime finalization)
        {
            return Ok(_orderServices.getOrdersByFinalization(finalization));
        }
    }
}