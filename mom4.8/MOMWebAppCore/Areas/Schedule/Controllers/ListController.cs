using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Schedule.Controllers
{
    [Area("Schedule")]
    public class ListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}