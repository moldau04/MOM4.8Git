using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Financials.Controllers
{
    public class ManageChecksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}