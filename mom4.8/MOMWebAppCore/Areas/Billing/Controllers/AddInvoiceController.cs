using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Billing.Controllers
{
    [Area("Billing")]
    public class AddInvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}