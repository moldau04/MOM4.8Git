using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Project.Controllers
{
    [Area("Project")]
    public class TemplatesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}