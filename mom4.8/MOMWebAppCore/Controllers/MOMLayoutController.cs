using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Controllers
{
    public class MOMLayoutController : Controller
    {
        public IActionResult MOMLayout()
        {
            return View();
        }
    }
}