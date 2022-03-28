using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.AccountPayable.Controllers
{
    [Area("AccountPayable")]
    public class BillsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}