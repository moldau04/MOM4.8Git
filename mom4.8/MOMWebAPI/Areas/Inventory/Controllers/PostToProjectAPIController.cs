using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessEntity.InventoryModel;
using BusinessEntity.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOMWebAPI.Utility;
using Newtonsoft.Json;

namespace MOMWebAPI.Areas.Inventory.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class PostToProjectAPIController : Controller
    {
        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IPostToProjectRepository _IPostToProjectRepository;
        public PostToProjectAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IPostToProjectRepository IPostToProjectRepository)
        {
            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IPostToProjectRepository = IPostToProjectRepository;
        }


        /// <summary>
        /// For PostToProject Screen : PostToProject.aspx / PostToProject.aspx.cs
        /// </summary>
        /// API's Naming Conventions : PostToProject_Method Name(Parameter)
        ///

        [HttpPost]
        [Route("[action]")]
        public IActionResult PostToProject_PostInventoryItemsToProject([FromBody]APIRequest _APIRequest)
        {
            try
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

                PostInventoryItemsToProjectParam _PostInventoryItemsToProject = JsonConvert.DeserializeObject<PostInventoryItemsToProjectParam>(_APIUtility.Param);

                ListPostInventoryItemsToProject _lstPostInventoryItemsToProject = _IPostToProjectRepository.PostToProject_PostInventoryItemsToProject(_PostInventoryItemsToProject, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstPostInventoryItemsToProject);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult PostToProject_GetEMP([FromBody]APIRequest _APIRequest)
        {
            try
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

                GetEMPParam _GetEMP = JsonConvert.DeserializeObject<GetEMPParam>(_APIUtility.Param);

                List<GetEMPViewModel> _lstGetEMP = _IPostToProjectRepository.PostToProject_GetEMP(_GetEMP, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEMP);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}