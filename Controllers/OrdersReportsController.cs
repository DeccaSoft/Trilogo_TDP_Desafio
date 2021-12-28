using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Requests;
using Aula6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Treinando.Models;

namespace Aula6.Controllers
{
    [ApiController]
    [Route("report")]
    //[Authorize(Roles = "Admin, Gerente")]
    public class OrdersReportsController : ControllerBase
    {
        private readonly ReportServices _reportServices;
        public OrdersReportsController(ReportServices reportServices)
        {
            _reportServices = reportServices;
        }

        [HttpGet]
        public IActionResult GetSearchReport([FromBody] FiltersSalesReports search)
        {
            OrderReport report = _reportServices.CreateGeneralReport(search.StartDate, search.EndDate, search.Statuses, search.Users);
            if(report == null)
            {
                return BadRequest("Você passou algum Parâmetro Inválido! Tente Novamente...");
            }
            return Ok(report);
        }
    }
}
