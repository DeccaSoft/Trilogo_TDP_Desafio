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
        private readonly ProductServices _productServices;
        private readonly DBContext _dbContext;
        //Injeção de Dependências
        public OrdersController(DBContext dbContext, OrderServices orderServices, UserServices userServices, ProductServices productServices)
        {
            _dbContext = dbContext;
            _orderServices = orderServices;
            _userServices = userServices;
            _productServices = productServices;
        }

        [HttpGet("userId/{userId}")]
        public IActionResult ListOrdersByUserId(int userId)             //Lista Pedidos do Usuário (Pega pelo Id do usuário)
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
        public IActionResult ListOrdersByUserLogin(string login)        //Lista Pedidos do Usuário (Pega pelo login do usuário)
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

        /*
        [HttpGet("order/{orderId}")]
        public IActionResult ListItemsByOrderId(int orderId)         //Lista os items de um pedido (pega pelo id do pedido)
        {
            var order = _orderServices.getOrder(orderId);

            if (order == null)
            {
                return BadRequest("Pedido Inexistente!");
            }

            if (_orderServices.ListItems(order).Count == 0 || _orderServices.ListItems(order) is null)
            {
                return BadRequest("Este Pedido Ainda NÃO possui nenhum Item Cadastrado!");
            }
            return Ok(_orderServices.ListItems(order));
        }
        */
        /*
        [HttpGet("{orderId}/items")]
        public IActionResult GetOrderItems(int orderId)    //Retorna Pedido e Todos seus Items
        {
            if (_orderServices.getOrder(orderId) is null)
            {
                return Ok("Pedido Inexistente!");
            }

            if (_orderServices.getOrderWithItems(orderId).Count == 0)
            {
                return Ok("Este Pedido ainda NÃO contém nenhum Item!");
            }
            return Ok(_orderServices.getOrderWithItems(orderId));
        }
        */

        //Se não houver pedido aberto -> Cria pedido em branco
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

        //Se já Existir pedido -> Edita o pedido
        [HttpPut("{login}")]
        public IActionResult updateItem(string login, int prodId, int qtd)     //Inclui um Item
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

        [HttpPut("order/{itemId}")]
        public IActionResult downgradeItem(int userId, int itemId, int prodId)          //Remove um Item
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

        //Edita Quantidades dos Ítens (Para mais) ou (para menos - bastando colocar sinal negativo)
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

        //Cancelar Pedido
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

        //Finlaizar Pedido
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

        //Mudar Forma de Pagamento
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

        // Listar pedidos por Status
        [HttpGet("report/status/{status}")]
        public IActionResult ListOrdersByStatus(OrderStatus status)
        {
            if (_orderServices.getOrdersByStatus(status).Count == 0)
            {
                return BadRequest("Não Existe nenhum pedido com esse Status!");
            }
            return Ok(_orderServices.getOrdersByStatus(status));
        }

        // Listar pedidos por Forma de Pagamento
        [HttpGet("report/payment/{payment}")]
        public IActionResult ListOrdersByPayment(Payment payment)
        {
            if (_orderServices.getOrdersByPayment(payment).Count == 0)
            {
                return BadRequest("Não Existe nenhum pedido com essa Forma de Pagamento!");
            }
            return Ok(_orderServices.getOrdersByPayment(payment));
        }

        // Listar Pedidos por Datas de...

        //Lista pedidos por data de Criação
        [HttpGet("report/criation/{criation}")]
        public IActionResult ListOrdersByCriation(string criation)
        {
            if (_orderServices.getOrdersByCriation(criation).Count == 0)
            {
                return BadRequest("NÃO há Nenhum Registro com essa Data de Criação!");
            }
            return Ok(_orderServices.getOrdersByCriation(criation));
        }

        //Lista pedidos por data de Cancelamento
        [HttpGet("report/cancellation/{cancellation}")]
        public IActionResult ListOrdersByCancellation(string cancellation)
        {
            if (_orderServices.getOrdersByCancellation(cancellation).Count == 0)
            {
                return BadRequest("NÃO há Nenhum Registro com essa Data de Cancelamento!");
            }
            return Ok(_orderServices.getOrdersByCancellation(cancellation));
        }

        //Lista pedidos por data de Finalização
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