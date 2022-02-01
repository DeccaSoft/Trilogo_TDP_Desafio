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
using Aula6.Interfaces;

namespace Treinando.Controllers
{
    [ApiController]
    [Route("/orders")]
    [Authorize(Roles = "Funcionario, Gerente, Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderServices;
        private readonly IUserService _userServices;
        private readonly IProductService _productServices;
        private readonly DBContext _dbContext;
        
        public OrdersController(DBContext dbContext, IOrderService orderServices, IUserService userServices, IProductService productServices)
        {
            _dbContext = dbContext;
            _orderServices = orderServices;
            _userServices = userServices;
            _productServices = productServices;
        }

        [HttpGet("userId/{userId}")]
        public IActionResult ListOrdersByUserId(int userId)             
        {
            var userModel = _userServices.GetUserById(userId);
            if(userModel is null)
            {
                return BadRequest("Usuário NÃO Cadastrado!");
            }
            if(_orderServices.getOrders(userModel).Count == 0)
            {
                return BadRequest("Este Usuário NÃO possui Nenhum Pedido Cadastrado em seu Nome!");
            }
            return Ok(_orderServices.getOrders(userModel));
        }

        [HttpGet("login/{login}")]
        public IActionResult ListOrdersByUserLogin(string login)        
        {
            var userModel = _userServices.GetUserByLogin(login);
            if (userModel is null)
            {
                return BadRequest("Usuário NÃO Cadastrado!");
            }
            if (_orderServices.getOrders(userModel).Count == 0)
            {
                return BadRequest("Este Usuário NÃO possui Nenhum Pedido Cadastrado em seu Nome!");
            }
            return Ok(_orderServices.getOrders(userModel));
        }

        
        [HttpPost("{login}")]
        public IActionResult CreateOrderForUser(string login)
        {
            var userModel = _userServices.GetUserByLogin(login);
            if(userModel is null)
            {
                return BadRequest("Usuário NÃO Cadastrado!");
            }

            if (_orderServices.GetUnfinishedOrder(userModel) == null)
            {
                return Ok(_orderServices.createCart(userModel));
            }
            else 
            {
                return Ok("Existe Pedido em Aberto: \n" + _orderServices.GetUnfinishedOrder(userModel) + " \n Primeiro Você precisa Cancelar ou Finalizar este Pedido!");
            }
            
        }

        
        [HttpPut("{login}")]
        public IActionResult updateItem(string login, int prodId, int qtd)     
        {
            User user = _userServices.GetUserByLogin(login);
            if(user is null) { return BadRequest("Usuário NÃO Cadastrado"); }
            Order order = _orderServices.GetUnfinishedOrder(user);
            if (order is null) { return BadRequest("Este Usuário NÃO possui Pedido em Aberto!"); }
            Product product = _productServices.GetProductById(prodId);
            if(product is null) { return BadRequest("Produto NÃO Cadastrado!"); }
            if (_orderServices.updateCart(user, order, product, qtd))
            {
                return Ok("Item Incluído com Sucesso! ");
            }
            return Ok("Quantidade Insuficiente em Estoque...");
        }

        [HttpPut("remove/{itemId}")]
        public IActionResult downgradeItem(int userId, int itemId, int prodId)          
        {
            User user = _userServices.GetUserById(userId);
            if (user is null) { return BadRequest("Usuário NÃO Cadastrado"); }
            Order order = _orderServices.GetUnfinishedOrder(user);
            if(order is null) { return BadRequest("Este Usuário NÃO possui Pedido em Aberto!"); }
            Product product = _productServices.GetProductById(prodId);
            if (product is null) { return BadRequest("Produto NÃO Cadastrado!"); }
            if (_orderServices.downgradeCart(user, order, product, itemId))
            {
                return Ok("Produto Removido do Carrinho...");
            }
            else
            {
                return BadRequest("Este produto não se encontra nesse Carrinho...");
            }
        }

        
        [HttpPut("item/{qtd}")]
        public IActionResult changeItemQuantity(int userId, int itemId, int prodId, int qtd)
        {
            User user = _userServices.GetUserById(userId);
            if (user is null) { return BadRequest("Usuário NÃO Cadastrado"); }
            Order orderModel = _orderServices.GetUnfinishedOrder(user);
            if (orderModel is null) { return BadRequest("Este Usuário NÃO possui Pedido em Aberto!"); }
            Product product = _productServices.GetProductById(prodId);
            if (product is null) { return BadRequest("Produto NÃO Cadastrado!"); }
            if (_orderServices.changeQuantity(user, orderModel, product, itemId, qtd))
            {
                return Ok("Quantidade do Produto Alterada com Sucesso...");
            }
            else
            {
                return BadRequest("Alguma coisa está errada... Verifique a existência deste item ou a quantidade deste produto no Carrinho...");
            }
        }

        
        [HttpDelete]
        [Route("cancel")]
        public IActionResult cancelOrder(int userId)
        {
            User user = _userServices.GetUserById(userId);
            if(user is null)
            {
                return BadRequest("Usuário Não Encontrado!");
            }
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

        
        [HttpPut]
        [Route("finish")]
        public IActionResult finishOrder(int userId)
        {
            User user = _userServices.GetUserById(userId);
            if(user is null)
            {
                return BadRequest("Usuário NÃO Cadastrado!");
            }
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

        
        [HttpPut]
        [Route("changePay/{userId}")]
        public IActionResult ChangePaymentOrder(int userId, Payment pay)
        {
            User user = _userServices.GetUserById(userId);
            if (user is null)
            {
                return BadRequest("Usuário NÃO Cadastrado!");
            }
            Order order = _orderServices.GetUnfinishedOrder(user);
            if (_orderServices.ChangePay(order, pay))
            {
                return Ok("Forma de Pagamento Alterada com Sucesso...");
            }
            else
            {
                return BadRequest("Pedido inexistente, cancelado ou já está finalizado! ");
            }
        }

        //RELATÓRIOS.........

       
        [HttpGet("report/status/{status}")]
        public IActionResult ListOrdersByStatus(OrderStatus status)
        {
            if (_orderServices.getOrdersByStatus(status).Count == 0)
            {
                return BadRequest("Não Existe nenhum pedido com esse Status!");
            }
            return Ok(_orderServices.getOrdersByStatus(status));
        }

        
        [HttpGet("report/payment/{payment}")]
        public IActionResult ListOrdersByPayment(Payment payment)
        {
            if (_orderServices.getOrdersByPayment(payment).Count == 0)
            {
                return BadRequest("Não Existe nenhum pedido com essa Forma de Pagamento!");
            }
            return Ok(_orderServices.getOrdersByPayment(payment));
        }

        
        [HttpGet("report/criation/{criation}")]
        public IActionResult ListOrdersByCriation(string criation)
        {
            if (_orderServices.getOrdersByCriation(criation).Count == 0)
            {
                return BadRequest("NÃO há Nenhum Registro com essa Data de Criação!");
            }
            return Ok(_orderServices.getOrdersByCriation(criation));
        }

        
        [HttpGet("report/cancellation/{cancellation}")]
        public IActionResult ListOrdersByCancellation(string cancellation)
        {
            if (_orderServices.getOrdersByCancellation(cancellation).Count == 0)
            {
                return BadRequest("NÃO há Nenhum Registro com essa Data de Cancelamento!");
            }
            return Ok(_orderServices.getOrdersByCancellation(cancellation));
        }

        
        [HttpGet("report/finalization/{finalization}")]
        public IActionResult ListOrdersByFinalization(string finalization)
        {
            if (_orderServices.getOrdersByFinalization(finalization).Count == 0)
            {
                return BadRequest ("NÃO há Nenhum Registro com essa Data de Cancelamento!");
            }
            return Ok(_orderServices.getOrdersByFinalization(finalization));
        }
    }
}