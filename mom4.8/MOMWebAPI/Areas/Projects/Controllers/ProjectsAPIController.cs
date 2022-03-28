using System.Collections.Generic;
using BusinessEntity.Projects;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BusinessEntity.Utility;
using MOMWebAPI.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MOMWebAPI.Areas.Projects.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsAPIController : Controller
    {   

        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IProjectRepository _IProjectRepository;
        public ProjectsAPIController( IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository , IProjectRepository IProjectRepository)
        {
          
            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IProjectRepository = IProjectRepository;
        }

         
        [HttpPost]
        [Route("[action]")]
        public IActionResult GetProjectListData([FromBody] APIRequest _APIRequest) 
        {
            APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

            APIResponse _response = new APIResponse();            

            _response.contentType = "application/json";

            _response.statusCode = StatusCodes.Status200OK.ToString();                        

            UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication( _APIUtility);

            if (!_US.IsValid)
            {
                _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
            }

            if (!ModelState.IsValid)
            {
                _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
            }


            ProjectListGridParam _ProjectParam = JsonConvert.DeserializeObject<ProjectListGridParam>(_APIUtility.Param);


            List<ProjectListGridModel> jobs = _IProjectRepository.spGetProjectListDataMVC(_ProjectParam , _APIUtility.ConnectionString);
             

            string JsonData = JsonConvert.SerializeObject(jobs.Take(19));             

            _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);  

            _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);


        }

        
    }
     
}
