using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOMWebAPI.Areas.Schedule.Models;
using MOMWebAPI.Utility;

namespace MOMWebAPI.Areas.Schedule.Controllers
{
    [Route("api/GoogleMap")]
    [ApiController]
    public class GoogleMapController : ControllerBase
    {
        private IGoogleMapRepository _IGoogleMapRepository;

        public GoogleMapController(IGoogleMapRepository GoogleMapRepository) {

            _IGoogleMapRepository = GoogleMapRepository;
        }

         
        [HttpPost]
        public IActionResult get([FromBody] GPSLiveDataPram _GPSLiveDataPram)
        {
           try
            {

                string webconfig = "nECc3SAUapvYK/O9wyvbft+sGLynaSVA//TMFOEb8XnZiMZUiUfhgwE/MLNNRHUcXo1kWVotmY6NkHzLnmVdvk1q3C7PDyUblgCo6oWQMDI="; 
                string fUser = "ARTIE"; 
                string date = "2021-02-12"; 
                string cat = ""; 
                string iscall = "0";

                string _webconfig = SSTCryptographer.Decrypt(webconfig, "webconfig");

                return Ok(_IGoogleMapRepository.GetGPSData(fUser, date, cat, iscall, _webconfig));
            }
            catch {  return NotFound(); }

        } 


      

    }
}