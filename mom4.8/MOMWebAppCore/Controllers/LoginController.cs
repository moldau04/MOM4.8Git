using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MOMWebAppCore.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.MOMBaseWebAPIURL = Startup.MOMBaseWebAPIURL;
            return View();
        }

         
        [HttpPost]
        public IActionResult PleaseLogin(string username , string password  )
        {
            if (username != null && password != null && username.Equals("admin") && password.Equals("admin"))
            { 

                string UserToken = "zyPu8vuvNNMRpSvJ81hSvDSw0vWOzTQoB/WP/DX9t82ZbJUHe3dDijl9wskCzW45YK67nh3DhWyy4oCOJSDfUD7/3tfUjZh7";

                HttpContext.Session.SetString("UserToken", UserToken);

                return RedirectToAction("index", "home", new { area = "dashboard" });
              
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return RedirectToAction("Logout");
            }
         
        }

       
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserToken");
            return RedirectToAction("Index");
        }
    }
}