using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Enums;
using Aula6.Interfaces;
using Aula6.Requests;
using Aula6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Controllers
{
    [ApiController]
    [Route("report")]
    [Authorize(Roles = "Admin, Gerente")]
    public class OrdersReportsController : ControllerBase
    {
        private readonly IReportService _reportServices;
        private readonly DBContext _dbContext;
        public OrdersReportsController(IReportService reportServices, DBContext dBContext)
        {
            _reportServices = reportServices;
            _dbContext = dBContext;
        }

        [HttpPost]
        public IActionResult GetSearchReport(DateTime startDate, DateTime endDate, [FromQuery] List<int> usersId, [FromQuery] List<OrderStatus> statuses)
        {

            OrderReport report = _reportServices.CreateGeneralReport(startDate, endDate, statuses, usersId);
            if (report == null)
            {
                return BadRequest("Você passou algum Parâmetro Inválido! Tente Novamente...");
            }
            return Ok(report);
        }
    }
}
