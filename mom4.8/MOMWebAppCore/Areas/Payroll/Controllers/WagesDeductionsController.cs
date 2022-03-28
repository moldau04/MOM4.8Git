using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Payroll.Controllers
{
    public class WagesDeductionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}