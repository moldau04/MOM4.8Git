using BusinessEntity.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MOMWebAPI.Utility
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityAPIController : Controller
    {

        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;


        public UtilityAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;

        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult InsertSessionData([FromBody] APIRequest _APIRequest)
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


            Core_Session_Data _Core_Session_Data = JsonConvert.DeserializeObject<Core_Session_Data>(_APIUtility.Param);

           // _IUtilityRepository.InsertSessionData(_Core_Session_Data, _APIUtility.ConnectionString);


            _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

        }
    }
}