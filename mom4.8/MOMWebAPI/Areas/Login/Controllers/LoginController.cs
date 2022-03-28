using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using System.Data;
using BusinessEntity.Login;
using BusinessEntity.UserModel;

namespace MOMWebAPI.Areas.Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {


        [HttpGet]
        [EnableCors("_allowSpecificOrigins")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return Json("Successfully Running WebAPI");
        }

        [HttpGet]
        [EnableCors("_allowSpecificOrigins")]
        [Route("[action]")]
        public IActionResult GetDatabaselist()
        {
            try
            {

                DataSet ds = new DataLayer.DL_User().getCompany(Startup.MSconnectionString);

                List<Databaselist> _Databaselist = new List<Databaselist>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _Databaselist.Add(new Databaselist()
                    {
                        Value = Convert.ToString(dr["dbname"]),
                        Text = Convert.ToString(dr["companyname"]),

                    });
                }

                return Json(_Databaselist);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [EnableCors("_allowSpecificOrigins")]
        [Route("[action]")]
        public IActionResult UserLogin([FromBody] UserAuth _UserAuth)
        {
             UserModel _UserModel = new UserModel();

            _UserModel.UserToken = "zyPu8vuvNNMRpSvJ81hSvDSw0vWOzTQoB/WP/DX9t82ZbJUHe3dDijl9wskCzW45YK67nh3DhWyy4oCOJSDfUD7/3tfUjZh7";
            _UserModel.Status = 1; 

            return Json(_UserModel);
        }
    }
}