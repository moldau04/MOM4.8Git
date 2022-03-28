using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BusinessEntity.Utility;
using MOMWebAPI.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using BusinessEntity.Programs;

namespace MOMWebAPI.Areas.Programs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APISetupController : Controller
    {

        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private ISetupRepository _ISetupRepository;
        public APISetupController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, ISetupRepository ISetupRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _ISetupRepository = ISetupRepository;
        }
        
        [HttpPost]
        [Route("[action]")]
        public IActionResult GetAddEditServiceTypeViewDLLData([FromBody] APIRequest _APIRequest)
        {
            APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

            APIResponse _response = new APIResponse();

            _response.contentType = "application/json";

            _response.statusCode = StatusCodes.Status200OK.ToString();

            UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

            if (!_US.IsValid)
            {
                _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
            }

            if (!ModelState.IsValid)
            {
                _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
            }

            //List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();


            //string JsonData = JsonConvert.SerializeObject(_ServiceTypeDDLData);

            //_response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

            //_response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);



            ServiceTypeDDL _ServiceTypeDDL = JsonConvert.DeserializeObject<ServiceTypeDDL>(_APIUtility.Param);

            List<ServiceTypeDDLData> _ServiceTypeDDLData = _ISetupRepository.GetServiceTypeViewDataModel(_APIUtility.ConnectionString, _ServiceTypeDDL.SearchBy, _ServiceTypeDDL.Case);

            string JsonData = JsonConvert.SerializeObject(_ServiceTypeDDLData);

            _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

            _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);


        }

    }
}