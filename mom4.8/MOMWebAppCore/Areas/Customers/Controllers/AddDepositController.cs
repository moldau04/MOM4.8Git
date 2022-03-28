using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class AddDepositController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}