﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Areas.Statements.Controllers
{
    public class ComparativeReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}