using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Sales.Controllers
{
    public class EstimateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}