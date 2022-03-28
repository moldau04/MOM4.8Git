using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Programs.Controllers
{
    [Area("Programs")]
    public class CustomLabelsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}