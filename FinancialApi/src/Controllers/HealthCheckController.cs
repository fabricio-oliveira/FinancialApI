using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApi.Controllers
{
    [Route("healthchecks")]
    public class HealthCheckController : Controller
    {
        [HttpGet("ping")]
        public string Get(int id)
        {
            return "FinancialApi";
        }
    }
}
