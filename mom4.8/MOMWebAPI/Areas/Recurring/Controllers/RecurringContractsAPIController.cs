using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Recurring;
using BusinessEntity.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOMWebAPI.Utility;
using Newtonsoft.Json;

namespace MOMWebAPI.Areas.Recurring.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class RecurringContractsAPIController : Controller
    {
        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IRecurringContractsRepository _IRecurringContractsRepository;
        public RecurringContractsAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IRecurringContractsRepository IRecurringContractsRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IRecurringContractsRepository = IRecurringContractsRepository;
        }


        /// <summary>
        /// For Recurring Contracts List Screen : RecContracts.aspx / RecContracts.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsList_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult RecContractsList_GetRoute([FromBody] APIRequest _APIRequest)
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

                GetRouteParam _GetRoute = JsonConvert.DeserializeObject<GetRouteParam>(_APIUtility.Param);

                ListGetRouteViewModel _ListGetRouteViewModel = _IRecurringContractsRepository.RecContractsList_GetRoute(_GetRoute, _APIUtility.ConnectionString, _GetRoute.IsActive, _GetRoute.LocID, _GetRoute.ContractID);

                string JsonData = JsonConvert.SerializeObject(_ListGetRouteViewModel);

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
        public IActionResult RecContractsList_GetActiveServiceType([FromBody] APIRequest _APIRequest)
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

                GetActiveServiceTypeParam _GetActiveServiceType = JsonConvert.DeserializeObject<GetActiveServiceTypeParam>(_APIUtility.Param);

                List<GetActiveServiceTypeViewModel> _lstGetActiveServiceType = _IRecurringContractsRepository.RecContractsList_GetActiveServiceType(_GetActiveServiceType, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetActiveServiceType);

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
        public IActionResult RecContractsList_GetCompanyByUserID([FromBody] APIRequest _APIRequest)
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

                GetCompanyByUserIDParam _GetCompanyByUserID = JsonConvert.DeserializeObject<GetCompanyByUserIDParam>(_APIUtility.Param);

                List<GetCompanyByUserIDViewModel> _lstGetCompanyByUserID = _IRecurringContractsRepository.RecContractsList_GetCompanyByUserID(_GetCompanyByUserID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetCompanyByUserID);

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
        public IActionResult RecContractsList_GetContractsData([FromBody] APIRequest _APIRequest)
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

                GetContractsDataParam _GetContractsData = JsonConvert.DeserializeObject<GetContractsDataParam>(_APIUtility.Param);

                List<GetContractsDataViewModel> _lstGetContractsData = _IRecurringContractsRepository.RecContractsList_GetContractsData(_GetContractsData, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetContractsData);

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
        public IActionResult RecContractsList_DeleteContract([FromBody] APIRequest _APIRequest)
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

                DeleteContractParam _DeleteContract = JsonConvert.DeserializeObject<DeleteContractParam>(_APIUtility.Param);

                _IRecurringContractsRepository.RecContractsList_DeleteContract(_DeleteContract, _APIUtility.ConnectionString);

                string JsonData = "Successfully Deleted Contracts";

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
        public IActionResult RecContractsList_UpdateExpirationDate([FromBody] APIRequest _APIRequest)
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

                UpdateExpirationDateParam _UpdateExpirationDate = JsonConvert.DeserializeObject<UpdateExpirationDateParam>(_APIUtility.Param);

                _IRecurringContractsRepository.RecContractsList_UpdateExpirationDate(_UpdateExpirationDate, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Expiration Date";

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
        public IActionResult RecContractsList_GetMultipleStockReports([FromBody] APIRequest _APIRequest)
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

                GetMultipleStockReportsParam _GetMultipleStockReports = JsonConvert.DeserializeObject<GetMultipleStockReportsParam>(_APIUtility.Param);

                List<GetMultipleStockReportsViewModel> _lstGetMultipleStockReports = _IRecurringContractsRepository.RecContractsList_GetMultipleStockReports(_GetMultipleStockReports, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetMultipleStockReports);

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
        public IActionResult RecContractsList_GetEscalationContracts([FromBody] APIRequest _APIRequest)
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

                GetEscalationContractsParam _GetEscalationContracts = JsonConvert.DeserializeObject<GetEscalationContractsParam>(_APIUtility.Param);

                List<GetEscalationContractsViewModel> _lstGetEscalationContracts = _IRecurringContractsRepository.RecContractsList_GetEscalationContracts(_GetEscalationContracts, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEscalationContracts);

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
        public IActionResult RecContractsList_EscalateContract([FromBody] APIRequest _APIRequest)
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

                EscalateContractParam _EscalateContract = JsonConvert.DeserializeObject<EscalateContractParam>(_APIUtility.Param);

                _IRecurringContractsRepository.RecContractsList_EscalateContract(_EscalateContract, _APIUtility.ConnectionString);

                string JsonData = "Successfully Escalated Contract";

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
        public IActionResult RecContractsList_GetDefaultWorkerHeader([FromBody] APIRequest _APIRequest)
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

                GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader = JsonConvert.DeserializeObject<GetDefaultWorkerHeaderParam>(_APIUtility.Param);

                string getValue = _IRecurringContractsRepository.RecContractsList_GetDefaultWorkerHeader(_GetDefaultWorkerHeader, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(getValue);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Add Recurring Contracts Screen : AddRecContract.aspx / AddRecContract.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddRecContract_Method Name(Parameter)
        ///

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddRecContract_GetContract([FromBody] APIRequest _APIRequest)
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

                GetContractParam _GetContract = JsonConvert.DeserializeObject<GetContractParam>(_APIUtility.Param);

                List<GetContractViewModel> _lstGetContract = _IRecurringContractsRepository.AddRecContract_GetContract(_GetContract, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetContract);

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
        public IActionResult AddRecContract_GetElevContract([FromBody] APIRequest _APIRequest)
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

                GetElevContractParam _GetElevContract = JsonConvert.DeserializeObject<GetElevContractParam>(_APIUtility.Param);

                List<GetElevContractViewModel> _lstGetElevContract = _IRecurringContractsRepository.AddRecContract_GetElevContract(_GetElevContract, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetElevContract);

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
        public IActionResult AddRecContract_GetEstimateByID([FromBody] APIRequest _APIRequest)
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

                GetEstimateByIDParam _GetEstimateByID = JsonConvert.DeserializeObject<GetEstimateByIDParam>(_APIUtility.Param);

                ListGetEstimateByID _lstGetEstimateByID = _IRecurringContractsRepository.AddRecContract_GetEstimateByID(_GetEstimateByID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEstimateByID);

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
        public IActionResult AddRecContract_getlocationType([FromBody] APIRequest _APIRequest)
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

                getlocationTypeParam _getlocationType = JsonConvert.DeserializeObject<getlocationTypeParam>(_APIUtility.Param);

                List<GetLocationTypeViewModel> _lstGetLocationType = _IRecurringContractsRepository.AddRecContract_getlocationType(_getlocationType, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetLocationType);

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
        public IActionResult AddRecContract_GetRecurringCustom([FromBody] APIRequest _APIRequest)
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

                GetRecurringCustomParam _GetRecurringCustom = JsonConvert.DeserializeObject<GetRecurringCustomParam>(_APIUtility.Param);

                ListGetRecurringCustom _lstGetRecurringCustom = _IRecurringContractsRepository.AddRecContract_GetRecurringCustom(_GetRecurringCustom, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetRecurringCustom);

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
        public IActionResult AddRecContract_GetActiveServiceTypeContract([FromBody] APIRequest _APIRequest)
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

                GetActiveServiceTypeContractParam _GetActiveServiceTypeContract = JsonConvert.DeserializeObject<GetActiveServiceTypeContractParam>(_APIUtility.Param);

                List<GetActiveServiceTypeViewModel> _lstGetActiveServiceType = _IRecurringContractsRepository.AddRecContract_GetActiveServiceTypeContract(_GetActiveServiceTypeContract, _APIUtility.ConnectionString, _GetActiveServiceTypeContract.LocType, _GetActiveServiceTypeContract.EditSType, _GetActiveServiceTypeContract.department = -1, _GetActiveServiceTypeContract.route = -1);

                string JsonData = JsonConvert.SerializeObject(_lstGetActiveServiceType);

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
        public IActionResult AddRecContract_GetEquiptype([FromBody] APIRequest _APIRequest)
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

                GetEquiptypeParam _GetEquiptype = JsonConvert.DeserializeObject<GetEquiptypeParam>(_APIUtility.Param);

                List<GetEquiptypeViewModel> _lstGetEquiptype = _IRecurringContractsRepository.AddRecContract_GetEquiptype(_GetEquiptype, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEquiptype);

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
        public IActionResult AddRecContract_GetEquipmentCategory([FromBody] APIRequest _APIRequest)
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

                GetEquipmentCategoryParam _GetEquipmentCategory = JsonConvert.DeserializeObject<GetEquipmentCategoryParam>(_APIUtility.Param);

                List<GetEquiptypeViewModel> _lstGetEquiptype = _IRecurringContractsRepository.AddRecContract_GetEquipmentCategory(_GetEquipmentCategory, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEquiptype);

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
        public IActionResult AddRecContract_GetBuildingElev([FromBody] APIRequest _APIRequest)
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

                GetBuildingElevParam _GetBuildingElev = JsonConvert.DeserializeObject<GetBuildingElevParam>(_APIUtility.Param);

                List<GetBuildingElevViewModel> _lstGetBuildingElev = _IRecurringContractsRepository.AddRecContract_GetBuildingElev(_GetBuildingElev, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetBuildingElev);

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
        public IActionResult AddRecContract_GetJstatus([FromBody] APIRequest _APIRequest)
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

                GetJstatusParam _GetJstatus = JsonConvert.DeserializeObject<GetJstatusParam>(_APIUtility.Param);

                List<GetJstatusViewModel> _lstGetJstatus = _IRecurringContractsRepository.AddRecContract_GetJstatus(_GetJstatus, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetJstatus);

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
        public IActionResult AddRecContract_GetElev([FromBody] APIRequest _APIRequest)
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

                GetElevParam _GetElev = JsonConvert.DeserializeObject<GetElevParam>(_APIUtility.Param);

                List<GetElevViewModel> _lstGetElev = _IRecurringContractsRepository.AddRecContract_GetElev(_GetElev, _APIUtility.ConnectionString, _GetElev.IsSalesAsigned);

                string JsonData = JsonConvert.SerializeObject(_lstGetElev);

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
        public IActionResult AddRecContract_IsExistContractByLoc([FromBody] APIRequest _APIRequest)
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

                IsExistContractByLocParam _IsExistContractByLoc = JsonConvert.DeserializeObject<IsExistContractByLocParam>(_APIUtility.Param);

                bool returnVal = _IRecurringContractsRepository.AddRecContract_IsExistContractByLoc(_IsExistContractByLoc, _APIUtility.ConnectionString);

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
        public IActionResult AddRecContract_UpdateContract([FromBody] APIRequest _APIRequest)
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

                UpdateContractParam _UpdateContract = JsonConvert.DeserializeObject<UpdateContractParam>(_APIUtility.Param);

                _IRecurringContractsRepository.AddRecContract_UpdateContract(_UpdateContract, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Contracts";

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
        public IActionResult AddRecContract_AddContract([FromBody] APIRequest _APIRequest)
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

                AddContractParam _AddContract = JsonConvert.DeserializeObject<AddContractParam>(_APIUtility.Param);

                _IRecurringContractsRepository.AddRecContract_AddContract(_AddContract, _APIUtility.ConnectionString);

                string JsonData = "Successfully Added Contract";

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
        public IActionResult AddRecContract_GetLocationByID([FromBody] APIRequest _APIRequest)
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

                GetLocationByIDParam _GetLocationByID = JsonConvert.DeserializeObject<GetLocationByIDParam>(_APIUtility.Param);

                ListGetLocationByID _lstGetLocationByID = _IRecurringContractsRepository.AddRecContract_GetLocationByID(_GetLocationByID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetLocationByID);

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
        public IActionResult AddRecContract_GetLocationAutojquery([FromBody] APIRequest _APIRequest)
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

                GetLocationAutojqueryParam _GetLocationAutojquery = JsonConvert.DeserializeObject<GetLocationAutojqueryParam>(_APIUtility.Param);

                List<GetLocationAutojqueryViewModel> _lstGetLocationAutojquery = _IRecurringContractsRepository.AddRecContract_GetLocationAutojquery(_GetLocationAutojquery, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetLocationAutojquery);

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
        public IActionResult AddRecContract_GetAllLocationOnCustomer([FromBody] APIRequest _APIRequest)
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

                GetAllLocationOnCustomerParam _GetAllLocationOnCustomer = JsonConvert.DeserializeObject<GetAllLocationOnCustomerParam>(_APIUtility.Param);

                List<GetAllLocationOnCustomerViewModel> _lstGetAllLocationOnCustomer = _IRecurringContractsRepository.AddRecContract_GetAllLocationOnCustomer(_GetAllLocationOnCustomer, _GetAllLocationOnCustomer.ownerId, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAllLocationOnCustomer);

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
        public IActionResult AddRecContract_GetServiceTypeByType([FromBody] APIRequest _APIRequest)
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

                GetServiceTypeByTypeParam _GetServiceTypeByType = JsonConvert.DeserializeObject<GetServiceTypeByTypeParam>(_APIUtility.Param);

                List<GetServiceTypeByTypeViewModel> _lstGetServiceTypeByType = _IRecurringContractsRepository.AddRecContract_GetServiceTypeByType(_GetServiceTypeByType, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetServiceTypeByType);

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
        public IActionResult AddRecContract_GetDiagnosticCategory([FromBody] APIRequest _APIRequest)
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

                GetDiagnosticCategoryParam _GetDiagnosticCategory = JsonConvert.DeserializeObject<GetDiagnosticCategoryParam>(_APIUtility.Param);

                List<GetDiagnosticCategoryViewModel> _lstGetDiagnosticCategory = _IRecurringContractsRepository.AddRecContract_GetDiagnosticCategory(_GetDiagnosticCategory, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetDiagnosticCategory);

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
        public IActionResult AddRecContract_GetControl([FromBody]APIRequest _APIRequest)
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

                List<GetControlViewModel> _lstGetControlViewModel = _IRecurringContractsRepository.AddRecContract_GetControl(_getConnectionConfigParam, _APIUtility.ConnectionString);

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
        public IActionResult AddRecContract_GetRecurringContractLogs([FromBody]APIRequest _APIRequest)
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

                GetRecurringContractLogsParam _GetRecurringContractLogs = JsonConvert.DeserializeObject<GetRecurringContractLogsParam>(_APIUtility.Param);

                List<GetRecurringContractLogsViewModel> _lstGetRecurringContractLogs = _IRecurringContractsRepository.AddRecContract_GetRecurringContractLogs(_GetRecurringContractLogs, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetRecurringContractLogs);

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
        public IActionResult AddRecContract_GetTerritory([FromBody] APIRequest _APIRequest)
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

                GetTerritoryParam _GetTerritory = JsonConvert.DeserializeObject<GetTerritoryParam>(_APIUtility.Param);

                List<GetTerritoryViewModel> _lstGetTerritory = _IRecurringContractsRepository.AddRecContract_GetTerritory(_GetTerritory, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetTerritory);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For Recurring Reports List Screen : RecContractsModule.aspx / RecContractsModule.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        [HttpPost]
        [Route("[action]")]
        public IActionResult RecContractsReport_GetReportDetailById([FromBody] APIRequest _APIRequest)
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

                GetReportDetailByIdParam _GetReportDetailByIdParam = JsonConvert.DeserializeObject<GetReportDetailByIdParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IRecurringContractsRepository.RecContractsReport_GetReportDetailById(_GetReportDetailByIdParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetCustomerType([FromBody] APIRequest _APIRequest)
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

                GetCustomerTypeParam _lstGetCustomerType = JsonConvert.DeserializeObject<GetCustomerTypeParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IRecurringContractsRepository.RecContractsReport_GetCustomerType(_lstGetCustomerType, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetCustomerName([FromBody] APIRequest _APIRequest)
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

                GetCustomerNameParam _lstCustomerReportParam = JsonConvert.DeserializeObject<GetCustomerNameParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IRecurringContractsRepository.RecContractsReport_GetCustomerName(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetCustomerAddress([FromBody] APIRequest _APIRequest)
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

                GetCustomerAddressParam _lstCustomerReportParam = JsonConvert.DeserializeObject<GetCustomerAddressParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IRecurringContractsRepository.RecContractsReport_GetCustomerAddress(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetCustomerCity([FromBody] APIRequest _APIRequest)
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

                GetCustomerCityParam _lstCustomerReportParam = JsonConvert.DeserializeObject<GetCustomerCityParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IRecurringContractsRepository.RecContractsReport_GetCustomerCity(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetDynamicReports([FromBody] APIRequest _APIRequest)
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

                GetDynamicReportsParam _lstCustomerReportParam = JsonConvert.DeserializeObject<GetDynamicReportsParam>(_APIUtility.Param);
                //string type = "Customer";

                List<CustomerReportViewModel> _lstCustomerReport = _IRecurringContractsRepository.RecContractsReport_GetDynamicReports(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetReportColByRepId([FromBody] APIRequest _APIRequest)
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

                GetReportColByRepIdParam _lstCustomerReportParam = JsonConvert.DeserializeObject<GetReportColByRepIdParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IRecurringContractsRepository.RecContractsReport_GetReportColByRepId(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetReportFiltersByRepId([FromBody] APIRequest _APIRequest)
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

                GetReportFiltersByRepIdParam _lstCustomerReportParam = JsonConvert.DeserializeObject<GetReportFiltersByRepIdParam>(_APIUtility.Param);

                List<CustomerFilterViewModel> _lstCustomerFilter = _IRecurringContractsRepository.RecContractsReport_GetReportFiltersByRepId(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetRecurringDetails([FromBody] APIRequest _APIRequest)
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

                GetRecurringDetailsParam _GetRecurringDetails = JsonConvert.DeserializeObject<GetRecurringDetailsParam>(_APIUtility.Param);

                List<GetRecurringDetailsViewModel> _lstGetRecurringDetails = _IRecurringContractsRepository.RecContractsReport_GetRecurringDetails(_GetRecurringDetails, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetRecurringDetails);

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
        public IActionResult RecContractsReport_GetAccountSummaryListingDetail([FromBody] APIRequest _APIRequest)
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

                GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam = JsonConvert.DeserializeObject<GetAccountSummaryListingDetailParam>(_APIUtility.Param);

                List<UserViewModel> _lstUserViewModel = _IRecurringContractsRepository.RecContractsReport_GetAccountSummaryListingDetail(_GetAccountSummaryListingDetailParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstUserViewModel);

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
        public IActionResult RecContractsReport_CheckExistingReport([FromBody] APIRequest _APIRequest)
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

                //string reportAction = "";
                CheckExistingReportParam _CheckExistingReportParam = JsonConvert.DeserializeObject<CheckExistingReportParam>(_APIUtility.Param);

                bool returnVal = _IRecurringContractsRepository.RecContractsReport_CheckExistingReport(_CheckExistingReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_InsertCustomerReport([FromBody] APIRequest _APIRequest)
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

                InsertCustomerReportParam _InsertCustomerReportParam = JsonConvert.DeserializeObject<InsertCustomerReportParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IRecurringContractsRepository.RecContractsReport_InsertCustomerReport(_InsertCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_IsStockReportExist([FromBody] APIRequest _APIRequest)
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

                //string reportAction = "";
                IsStockReportExistParam _IsStockReportExistParam = JsonConvert.DeserializeObject<IsStockReportExistParam>(_APIUtility.Param);

                bool customertViewModel = _IRecurringContractsRepository.RecContractsReport_IsStockReportExist(_IsStockReportExistParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(customertViewModel);

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
        public IActionResult RecContractsReport_UpdateCustomerReport([FromBody] APIRequest _APIRequest)
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

                UpdateCustomerReportParam _UpdateCustomerReportParam = JsonConvert.DeserializeObject<UpdateCustomerReportParam>(_APIUtility.Param);

                _IRecurringContractsRepository.RecContractsReport_UpdateCustomerReport(_UpdateCustomerReportParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Customer Report";

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
        public IActionResult RecContractsReport_DeleteCustomerReport([FromBody] APIRequest _APIRequest)
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

                DeleteCustomerReportParam _CustomerReportParam = JsonConvert.DeserializeObject<DeleteCustomerReportParam>(_APIUtility.Param);

                _IRecurringContractsRepository.RecContractsReport_DeleteCustomerReport(_CustomerReportParam, _APIUtility.ConnectionString);

                string JsonData = "Deleted Successfully Customer Report";

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
        public IActionResult RecContractsReport_GetControlForReports([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IRecurringContractsRepository.RecContractsReport_GetControlForReports(_getConnectionConfigParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetHeaderFooterDetail([FromBody] APIRequest _APIRequest)
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

                GetHeaderFooterDetailParam _customer = JsonConvert.DeserializeObject<GetHeaderFooterDetailParam>(_APIUtility.Param);

                List<HeaderFooterDetailViewModel> _lstHeaderFooterDetailViewModel = _IRecurringContractsRepository.RecContractsReport_GetHeaderFooterDetail(_customer, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstHeaderFooterDetailViewModel);

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
        public IActionResult RecContractsReport_GetOwners([FromBody] APIRequest _APIRequest)
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

                GetOwnersParam _GetOwnersParam = JsonConvert.DeserializeObject<GetOwnersParam>(_APIUtility.Param);

                //string query = "SELECT distinct equip FROM CustomerReportDetails";

                List<CustomerFilterViewModel> _lstCustomerFilter = _IRecurringContractsRepository.RecContractsReport_GetOwners(_GetOwnersParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetColumnWidthByReportId([FromBody] APIRequest _APIRequest)
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

                GetColumnWidthByReportIdParam _customer = JsonConvert.DeserializeObject<GetColumnWidthByReportIdParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReport = _IRecurringContractsRepository.RecContractsReport_GetColumnWidthByReportId(_customer, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_UpdateCustomerReportResizedWidth([FromBody] APIRequest _APIRequest)
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

                UpdateCustomerReportResizedWidthParam _customer = JsonConvert.DeserializeObject<UpdateCustomerReportResizedWidthParam>(_APIUtility.Param);

                _IRecurringContractsRepository.RecContractsReport_UpdateCustomerReportResizedWidth(_customer, _APIUtility.ConnectionString);

                string JsonData = "Successfully updated Customer Reports Resized Width";

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For Recurring Reports List Screen : EscalationListingReport.aspx / EscalationListingReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///
        [HttpPost]
        [Route("[action]")]
        public IActionResult RecContractsReport_GetUserEmail([FromBody]APIRequest _APIRequest)
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

                GetUserEmailParam _GetUserEmail = JsonConvert.DeserializeObject<GetUserEmailParam>(_APIUtility.Param);

                string returnVal = _IRecurringContractsRepository.RecContractsReport_GetUserEmail(_GetUserEmail, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetEscalationReportFiltersValue([FromBody]APIRequest _APIRequest)
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

                GetEscalationReportFiltersValueParam _GetEscalationReportFiltersValue = JsonConvert.DeserializeObject<GetEscalationReportFiltersValueParam>(_APIUtility.Param);

                ListGetEscalationReportFiltersValue _lstGetEscalationReportFiltersValue = _IRecurringContractsRepository.RecContractsReport_GetEscalationReportFiltersValue(_GetEscalationReportFiltersValue, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEscalationReportFiltersValue);

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
        public IActionResult RecContractsReport_GetEscalationDetails([FromBody]APIRequest _APIRequest)
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

                GetEscalationDetailsParam _GetEscalationDetails = JsonConvert.DeserializeObject<GetEscalationDetailsParam>(_APIUtility.Param);

                List<GetEscalationDetailsViewModel> _lstGetEscalationDetails = _IRecurringContractsRepository.RecContractsReport_GetEscalationDetails(_GetEscalationDetails, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEscalationDetails);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// For Recurring Reports List Screen : MonthlyRecurringHoursReport.aspx / MonthlyRecurringHoursReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        /// <summary>
        /// For Recurring Reports List Screen : MonthlyRecurringHoursByRouteReport.aspx / MonthlyRecurringHoursByRouteReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        [HttpPost]
        [Route("[action]")]
        public IActionResult RecContractsReport_GetMonthlyRecurringHoursTEI([FromBody] APIRequest _APIRequest)
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

                GetMonthlyRecurringHoursTEIParam _GetMonthlyRecurringHoursTEI = JsonConvert.DeserializeObject<GetMonthlyRecurringHoursTEIParam>(_APIUtility.Param);

                List<GetMonthlyRecurringHoursTEIViewModel> _lstGetMonthlyRecurringHoursTEI = _IRecurringContractsRepository.RecContractsReport_GetMonthlyRecurringHoursTEI(_GetMonthlyRecurringHoursTEI, _APIUtility.ConnectionString, _GetMonthlyRecurringHoursTEI.routes);

                string JsonData = JsonConvert.SerializeObject(_lstGetMonthlyRecurringHoursTEI);

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
        public IActionResult RecContractsReport_GetMonthlyRecurringHours([FromBody] APIRequest _APIRequest)
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

                GetMonthlyRecurringHoursParam _GetMonthlyRecurringHours = JsonConvert.DeserializeObject<GetMonthlyRecurringHoursParam>(_APIUtility.Param);

                List<GetMonthlyRecurringHoursViewModel> _lstGetMonthlyRecurringHours = _IRecurringContractsRepository.RecContractsReport_GetMonthlyRecurringHours(_GetMonthlyRecurringHours, _APIUtility.ConnectionString, _GetMonthlyRecurringHours.routes);

                string JsonData = JsonConvert.SerializeObject(_lstGetMonthlyRecurringHours);

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
        public IActionResult RecContractsReport_GetRouteActive([FromBody] APIRequest _APIRequest)
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

                GetRouteActiveParam _GetRouteActive = JsonConvert.DeserializeObject<GetRouteActiveParam>(_APIUtility.Param);

                List<GetRouteActiveViewModel> _lstGetRouteActive = _IRecurringContractsRepository.RecContractsReport_GetRouteActive(_GetRouteActive, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetRouteActive);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Recurring Reports List Screen : OpenMaintenanceByEquipmentReport.aspx / OpenMaintenanceByEquipmentReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : RecContractsReport_Method Name(Parameter)
        ///

        [HttpPost]
        [Route("[action]")]
        public IActionResult RecContractsReport_GetCompanyDetails([FromBody] APIRequest _APIRequest)
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

                List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = _IRecurringContractsRepository.RecContractsReport_GetCompanyDetails(_GetCompanyDetailsParam, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetDefaultCategory([FromBody] APIRequest _APIRequest)
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

                GetDefaultCategoryParam _GetDefaultCategory = JsonConvert.DeserializeObject<GetDefaultCategoryParam>(_APIUtility.Param);

                String returnVal = _IRecurringContractsRepository.RecContractsReport_GetDefaultCategory(_GetDefaultCategory, _APIUtility.ConnectionString);

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
        public IActionResult RecContractsReport_GetOpenMaintenanceByEquipment([FromBody] APIRequest _APIRequest)
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

                GetOpenMaintenanceByEquipmentParam _GetOpenMaintenanceByEquipment = JsonConvert.DeserializeObject<GetOpenMaintenanceByEquipmentParam>(_APIUtility.Param);

                List<GetOpenMaintenanceByEquipmentViewModel> _lstGetOpenMaintenanceByEquipment = _IRecurringContractsRepository.RecContractsReport_GetOpenMaintenanceByEquipment(_GetOpenMaintenanceByEquipment, _APIUtility.ConnectionString, _GetOpenMaintenanceByEquipment.defaultCategory);

                string JsonData = JsonConvert.SerializeObject(_lstGetOpenMaintenanceByEquipment);

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
        public IActionResult RecContractsReport_GetSMTPByUserID([FromBody]APIRequest _APIRequest)
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

                GetSMTPByUserIDParam _GetSMTPByUserID = JsonConvert.DeserializeObject<GetSMTPByUserIDParam>(_APIUtility.Param);

                List<SMTPEmailViewModel> _lstSMTPEmailViewModel = _IRecurringContractsRepository.RecContractsReport_GetSMTPByUserID(_GetSMTPByUserID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstSMTPEmailViewModel);

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