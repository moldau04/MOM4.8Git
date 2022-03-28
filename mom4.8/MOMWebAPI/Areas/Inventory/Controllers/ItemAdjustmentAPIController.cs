using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.InventoryModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOMWebAPI.Utility;
using Newtonsoft.Json;

namespace MOMWebAPI.Areas.Inventory.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ItemAdjustmentAPIController : Controller
    {

        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IItemAdjustmentRepository _IItemAdjustmentRepository;
        public ItemAdjustmentAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IItemAdjustmentRepository IItemAdjustmentRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IItemAdjustmentRepository = IItemAdjustmentRepository;
        }



        /// <summary>
        /// For InventoryAdjustments Screen : InventoryAdjustments.aspx / InventoryAdjustments.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryAdjustmentsList_Method Name(Parameter)
        /// 


        [HttpPost]
        [Route("[action]")]
        public IActionResult InventoryAdjustmentsList_GetAllInventoryAdjustmentByDate([FromBody]APIRequest _APIRequest)
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

                GetAllInventoryAdjustmentByDateParam _GetAllInvAdjustmentByDate = JsonConvert.DeserializeObject<GetAllInventoryAdjustmentByDateParam>(_APIUtility.Param);

                List<GetAllInvAdjustmentByDateViewModel> _lstGetAllInvAdjustmentByDate = _IItemAdjustmentRepository.InventoryAdjustmentsList_GetAllInventoryAdjustmentByDate(_GetAllInvAdjustmentByDate, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAllInvAdjustmentByDate);

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
        public IActionResult InventoryAdjustmentsList_DeleteAdjustment([FromBody]APIRequest _APIRequest)
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

                DeleteAdjustmentParam _DeleteAdjustment = JsonConvert.DeserializeObject<DeleteAdjustmentParam>(_APIUtility.Param);

                int returnVal = _IItemAdjustmentRepository.InventoryAdjustmentsList_DeleteAdjustment(_DeleteAdjustment, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(returnVal);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For AddInventoryAdjustments Screen : AddInventoryAdjustments.aspx / AddInventoryAdjustments.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddInventoryAdjustments_Method Name(Parameter)
        /// 



        [HttpPost]
        [Route("[action]")]
        public IActionResult AddInventoryAdjustments_GetInventoryAdjustmentByID([FromBody]APIRequest _APIRequest)
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

                GetInventoryAdjustmentByIDParam _GetInventoryAdjustmentByID = JsonConvert.DeserializeObject<GetInventoryAdjustmentByIDParam>(_APIUtility.Param);

                List<GetInventoryAdjustmentByIDViewModel> _lstGetInventoryAdjustmentByID = _IItemAdjustmentRepository.AddInventoryAdjustments_GetInventoryAdjustmentByID(_GetInventoryAdjustmentByID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInventoryAdjustmentByID);

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
        public IActionResult AddInventoryAdjustments_CreateInventoryAdjustments([FromBody]APIRequest _APIRequest)
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

                CreateInventoryAdjustmentsParam _CreateInventoryAdjustments = JsonConvert.DeserializeObject<CreateInventoryAdjustmentsParam>(_APIUtility.Param);

                int returnVal = _IItemAdjustmentRepository.AddInventoryAdjustments_CreateInventoryAdjustments(_CreateInventoryAdjustments, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(returnVal);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For POWeeklyReport Screen : POWeeklyReport.aspx / POWeeklyReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : POWeeklyReport_Method Name(Parameter)
        ///

        [HttpPost]
        [Route("[action]")]
        public IActionResult POWeeklyReport_GetSMTPByUserID([FromBody]APIRequest _APIRequest)
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

                GetSMTPByUserIDParam _user = JsonConvert.DeserializeObject<GetSMTPByUserIDParam>(_APIUtility.Param);

                List<SMTPEmailViewModel> _lstSMTPEmailViewModel = _IItemAdjustmentRepository.POWeeklyReport_GetSMTPByUserID(_user, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstSMTPEmailViewModel);

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
        public IActionResult POWeeklyReport_GetCompanyDetails([FromBody] APIRequest _APIRequest)
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

                GetCompanyDetailsParam _GetCompanyDetailsParam = JsonConvert.DeserializeObject<GetCompanyDetailsParam>(_APIUtility.Param);

                List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = _IItemAdjustmentRepository.POWeeklyReport_GetCompanyDetails(_GetCompanyDetailsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetCompanyDetailsViewModel);

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
        public IActionResult POWeeklyReport_GetControl([FromBody]APIRequest _APIRequest)
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

                getConnectionConfigParam _getConnectionConfigParam = JsonConvert.DeserializeObject<getConnectionConfigParam>(_APIUtility.Param);

                List<GetControlViewModel> _lstGetControlViewModel = _IItemAdjustmentRepository.POWeeklyReport_GetControl(_getConnectionConfigParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetControlViewModel);

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
        public IActionResult POWeeklyReport_GetPOWeekly([FromBody]APIRequest _APIRequest)
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

                GetPOWeeklyParam _GetPOWeekly = JsonConvert.DeserializeObject<GetPOWeeklyParam>(_APIUtility.Param);

                List<GetPOWeeklyViewModel> _lstGetPOWeekly = _IItemAdjustmentRepository.POWeeklyReport_GetPOWeekly(_GetPOWeekly, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetPOWeekly);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For CustomersReport Screen : CustomersReport.aspx / CustomersReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : CustomersReport_Method Name(Parameter)
        /// 
        [HttpPost]
        [Route("[action]")]
        public IActionResult CustomersReport_GetReportDetailById([FromBody]APIRequest _APIRequest)
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

                GetReportDetailByIdParam _GetReportDetailById = JsonConvert.DeserializeObject<GetReportDetailByIdParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IItemAdjustmentRepository.CustomersReport_GetReportDetailById(_GetReportDetailById, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerReport);

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
        public IActionResult CustomersReport_GetCustomerType([FromBody]APIRequest _APIRequest)
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

                GetCustomerTypeParam _getCustomerType = JsonConvert.DeserializeObject<GetCustomerTypeParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IItemAdjustmentRepository.CustomersReport_GetCustomerType(_getCustomerType, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerReport);

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
        public IActionResult CustomersReport_GetCustReportFiltersValue([FromBody]APIRequest _APIRequest)
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

                GetCustReportFiltersValueParam _GetCustReportFiltersValue = JsonConvert.DeserializeObject<GetCustReportFiltersValueParam>(_APIUtility.Param);

                ListGetCustReportFiltersValue _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetCustReportFiltersValue(_GetCustReportFiltersValue, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetCustomerName([FromBody]APIRequest _APIRequest)
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

                GetCustomerNameParam _GetCustomerName = JsonConvert.DeserializeObject<GetCustomerNameParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetCustomerName(_GetCustomerName, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetCustomerAddress([FromBody]APIRequest _APIRequest)
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

                GetCustomerAddressParam _GetCustomerAddress = JsonConvert.DeserializeObject<GetCustomerAddressParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetCustomerAddress(_GetCustomerAddress, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetCustomerCity([FromBody]APIRequest _APIRequest)
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

                GetCustomerCityParam _GetCustomerCity = JsonConvert.DeserializeObject<GetCustomerCityParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetCustomerCity(_GetCustomerCity, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetDynamicReports([FromBody]APIRequest _APIRequest)
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

                GetDynamicReportsParam _GetDynamicReports = JsonConvert.DeserializeObject<GetDynamicReportsParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IItemAdjustmentRepository.CustomersReport_GetDynamicReports(_GetDynamicReports, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerReport);

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
        public IActionResult CustomersReport_GetReportColByRepId([FromBody]APIRequest _APIRequest)
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

                GetReportColByRepIdParam _GetReportColByRepId = JsonConvert.DeserializeObject<GetReportColByRepIdParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetReportColByRepId(_GetReportColByRepId, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetReportFiltersByRepId([FromBody]APIRequest _APIRequest)
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

                GetReportFiltersByRepIdParam _GetReportFiltersByRepId = JsonConvert.DeserializeObject<GetReportFiltersByRepIdParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetReportFiltersByRepId(_GetReportFiltersByRepId, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_getCustomerDetailsTest([FromBody]APIRequest _APIRequest)
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

                getCustomerDetailsTestParam _getCustomerDetailsTest = JsonConvert.DeserializeObject<getCustomerDetailsTestParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_getCustomerDetailsTest(_getCustomerDetailsTest, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetAccountSummaryListingDetail([FromBody]APIRequest _APIRequest)
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

                GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetail = JsonConvert.DeserializeObject<GetAccountSummaryListingDetailParam>(_APIUtility.Param);

                List<UserViewModel> _lstUser = _IItemAdjustmentRepository.CustomersReport_GetAccountSummaryListingDetail(_GetAccountSummaryListingDetail, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstUser);

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
        public IActionResult CustomersReport_CheckExistingReport([FromBody]APIRequest _APIRequest)
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

                CheckExistingReportParam _CheckExistingReport = JsonConvert.DeserializeObject<CheckExistingReportParam>(_APIUtility.Param);

                bool returnVal = _IItemAdjustmentRepository.CustomersReport_CheckExistingReport(_CheckExistingReport, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(returnVal);

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
        public IActionResult CustomersReport_InsertCustomerReport([FromBody]APIRequest _APIRequest)
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

                InsertCustomerReportParam _InsertCustomerReport = JsonConvert.DeserializeObject<InsertCustomerReportParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IItemAdjustmentRepository.CustomersReport_InsertCustomerReport(_InsertCustomerReport, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerReport);

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
        public IActionResult CustomersReport_IsStockReportExist([FromBody]APIRequest _APIRequest)
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

                IsStockReportExistParam _IsStockReportExist = JsonConvert.DeserializeObject<IsStockReportExistParam>(_APIUtility.Param);

                bool _returnVal = _IItemAdjustmentRepository.CustomersReport_IsStockReportExist(_IsStockReportExist, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_returnVal);

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
        public IActionResult CustomersReport_UpdateCustomerReport([FromBody]APIRequest _APIRequest)
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

                UpdateCustomerReportParam _UpdateCustomerReport = JsonConvert.DeserializeObject<UpdateCustomerReportParam>(_APIUtility.Param);

                _IItemAdjustmentRepository.CustomersReport_UpdateCustomerReport(_UpdateCustomerReport, _APIUtility.ConnectionString);

                string JsonData = "Successfully Udated customer Report";

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
        public IActionResult CustomersReport_DeleteCustomerReport([FromBody]APIRequest _APIRequest)
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

                DeleteCustomerReportParam _DeleteCustomerReport = JsonConvert.DeserializeObject<DeleteCustomerReportParam>(_APIUtility.Param);

               _IItemAdjustmentRepository.CustomersReport_DeleteCustomerReport(_DeleteCustomerReport, _APIUtility.ConnectionString);

                string JsonData = "Successfully Deleted Customer Report";

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
        public IActionResult CustomersReport_GetControlForReports([FromBody]APIRequest _APIRequest)
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

                getConnectionConfigParam _getConnectionConfig = JsonConvert.DeserializeObject<getConnectionConfigParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetControlForReports(_getConnectionConfig, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetHeaderFooterDetail([FromBody]APIRequest _APIRequest)
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

                GetHeaderFooterDetailParam _GetHeaderFooterDetail = JsonConvert.DeserializeObject<GetHeaderFooterDetailParam>(_APIUtility.Param);

                List<HeaderFooterDetailViewModel> _lstHeaderFooterDetail= _IItemAdjustmentRepository.CustomersReport_GetHeaderFooterDetail(_GetHeaderFooterDetail, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstHeaderFooterDetail);

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
        public IActionResult CustomersReport_GetOwners([FromBody]APIRequest _APIRequest)
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

                GetOwnersParam _GetOwners = JsonConvert.DeserializeObject<GetOwnersParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IItemAdjustmentRepository.CustomersReport_GetOwners(_GetOwners, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

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
        public IActionResult CustomersReport_GetColumnWidthByReportId([FromBody]APIRequest _APIRequest)
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

                GetColumnWidthByReportIdParam _GetColumnWidthByReportId = JsonConvert.DeserializeObject<GetColumnWidthByReportIdParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IItemAdjustmentRepository.CustomersReport_GetColumnWidthByReportId(_GetColumnWidthByReportId, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerReport);

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
        public IActionResult CustomersReport_UpdateCustomerReportResizedWidth([FromBody]APIRequest _APIRequest)
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

                UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth = JsonConvert.DeserializeObject<UpdateCustomerReportResizedWidthParam>(_APIUtility.Param);

                _IItemAdjustmentRepository.CustomersReport_UpdateCustomerReportResizedWidth(_UpdateCustomerReportResizedWidth, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Customer Report Resized width";

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