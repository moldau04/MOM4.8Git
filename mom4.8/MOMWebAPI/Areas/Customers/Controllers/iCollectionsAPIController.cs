using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOMWebAPI.Utility;
using Newtonsoft.Json;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class iCollectionsAPIController : Controller
    {
        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IiCollectionsRepository _IiCollectionsRepository;
        public iCollectionsAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IiCollectionsRepository IiCollectionsRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IiCollectionsRepository = IiCollectionsRepository;
        }


        /// <summary>
        /// For iCollections List Screen : iCollections.aspx / iCollections.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsList_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsList_GetCustomFields([FromBody] APIRequest _APIRequest)
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

                getCustomFieldsParam _getCustomFieldsParam = JsonConvert.DeserializeObject<getCustomFieldsParam>(_APIUtility.Param);

                List<CustomViewModel> _lstCustom = _IiCollectionsRepository.iCollectionsList_GetCustomFields(_getCustomFieldsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustom);

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
        public IActionResult iCollectionsList_GetDepartment([FromBody] APIRequest _APIRequest)
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

                GetDepartmentParam _GetDepartment = JsonConvert.DeserializeObject<GetDepartmentParam>(_APIUtility.Param);

                List<JobTypeViewModel> _lstJobType = _IiCollectionsRepository.iCollectionsList_GetDepartment(_GetDepartment, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstJobType);

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
        public IActionResult iCollectionsList_GetCompanyByCustomer([FromBody] APIRequest _APIRequest)
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

                GetCompanyByCustomerParam _GetCompanyByCustomerParam = JsonConvert.DeserializeObject<GetCompanyByCustomerParam>(_APIUtility.Param);

                List<CompanyOfficeViewModel> _lstCompanyOfficeViewModel = _IiCollectionsRepository.iCollectionsList_GetCompanyByCustomer(_GetCompanyByCustomerParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCompanyOfficeViewModel);

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
        public IActionResult iCollectionsList_getUserDefaultCompany([FromBody] APIRequest _APIRequest)
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

                getUserDefaultCompanyParam _getUserDefaultCompanyParam = JsonConvert.DeserializeObject<getUserDefaultCompanyParam>(_APIUtility.Param);

                List<UserViewModel> _lstUserViewModel = _IiCollectionsRepository.iCollectionsList_getUserDefaultCompany(_getUserDefaultCompanyParam, _APIUtility.ConnectionString);

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
        public IActionResult iCollectionsList_GetCollections([FromBody] APIRequest _APIRequest)
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

                GetCollectionsParam _GetCollections = JsonConvert.DeserializeObject<GetCollectionsParam>(_APIUtility.Param);

                List<GetCollectionsViewModel> _lstGetCollections = _IiCollectionsRepository.iCollectionsList_GetCollections(_GetCollections, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetCollections);

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
        public IActionResult iCollectionsList_GetInvoicesByRef([FromBody] APIRequest _APIRequest)
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

                GetInvoicesByRefParam _GetInvoicesByRef = JsonConvert.DeserializeObject<GetInvoicesByRefParam>(_APIUtility.Param);

                List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = _IiCollectionsRepository.iCollectionsList_GetInvoicesByRef(_GetInvoicesByRef, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoicesByRef);

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
        public IActionResult iCollectionsList_GetControl([FromBody] APIRequest _APIRequest)
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

                List<GetControlViewModel> _lstGetControl = _IiCollectionsRepository.iCollectionsList_GetControl(_getConnectionConfig, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetControl);

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
        public IActionResult iCollectionsList_GetInvoiceItemByRef([FromBody] APIRequest _APIRequest)
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

                GetInvoiceItemByRefParam _GetInvoiceItemByRef = JsonConvert.DeserializeObject<GetInvoiceItemByRefParam>(_APIUtility.Param);

                List<GetInvoiceItemByRefViewModel> _lstGetInvoiceItemByRef = _IiCollectionsRepository.iCollectionsList_GetInvoiceItemByRef(_GetInvoiceItemByRef, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoiceItemByRef);

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
        public IActionResult iCollectionsList_GetControlBranch([FromBody] APIRequest _APIRequest)
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

                GetControlBranchParam _GetControlBranchParam = JsonConvert.DeserializeObject<GetControlBranchParam>(_APIUtility.Param);

                List<UserViewModel> _lstUserViewModel = _IiCollectionsRepository.iCollectionsList_GetControlBranch(_GetControlBranchParam, _APIUtility.ConnectionString);

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
        public IActionResult iCollectionsList_GetTicketID([FromBody] APIRequest _APIRequest)
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

                GetTicketIDParam _GetTicketID = JsonConvert.DeserializeObject<GetTicketIDParam>(_APIUtility.Param);

                List<GetTicketIDViewModel> _lstGetTicketID = _IiCollectionsRepository.iCollectionsList_GetTicketID(_GetTicketID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetTicketID);

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
        public IActionResult iCollectionsList_GetLocationByID([FromBody] APIRequest _APIRequest)
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

                ListGetLocationByID _lstGetLocationByID = _IiCollectionsRepository.iCollectionsList_GetLocationByID(_GetLocationByID, _APIUtility.ConnectionString);

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
        public IActionResult iCollectionsList_GetElevByTicket([FromBody] APIRequest _APIRequest)
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

                GetElevByTicketParam _GetElevByTicket = JsonConvert.DeserializeObject<GetElevByTicketParam>(_APIUtility.Param);

                List<GetElevByTicketViewModel> _lstGetElevByTicket = _IiCollectionsRepository.iCollectionsList_GetElevByTicket(_GetElevByTicket, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetElevByTicket);

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
        public IActionResult iCollectionsList_GetequipREPDetails([FromBody] APIRequest _APIRequest)
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

                GetequipREPDetailsParam _GetequipREPDetails = JsonConvert.DeserializeObject<GetequipREPDetailsParam>(_APIUtility.Param);

                List<GetequipREPDetailsViewModel> _lstGetequipREPDetails = _IiCollectionsRepository.iCollectionsList_GetequipREPDetails(_GetequipREPDetails, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetequipREPDetails);

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
        public IActionResult iCollectionsList_GetElevByTicketID([FromBody] APIRequest _APIRequest)
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

                GetElevByTicketIDParam _GetElevByTicketID = JsonConvert.DeserializeObject<GetElevByTicketIDParam>(_APIUtility.Param);

                List<GetElevByTicketViewModel> _lstGetElevByTicketID = _IiCollectionsRepository.iCollectionsList_GetElevByTicketID(_GetElevByTicketID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetElevByTicketID);

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
        public IActionResult iCollectionsList_GetCustomerStatementByLoc([FromBody] APIRequest _APIRequest)
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

                GetCustomerStatementByLocParam _GetCustomerStatementByLoc = JsonConvert.DeserializeObject<GetCustomerStatementByLocParam>(_APIUtility.Param);

                List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLoc = _IiCollectionsRepository.iCollectionsList_GetCustomerStatementByLoc(_GetCustomerStatementByLoc, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetCustomerStatementByLoc);

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
        public IActionResult iCollectionsList_AddEmailLog([FromBody] APIRequest _APIRequest)
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

                AddEmailLogParam _AddEmailLog = JsonConvert.DeserializeObject<AddEmailLogParam>(_APIUtility.Param);

                _IiCollectionsRepository.iCollectionsList_AddEmailLog(_AddEmailLog, _APIUtility.ConnectionString);

                string JsonData = "Successfully Added Email Log";

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
        public IActionResult iCollectionsList_GetCustomerStatementInvoicesSouthern([FromBody] APIRequest _APIRequest)
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

                GetCustomerStatementInvoicesSouthernParam _GetCustStatementInvSouthern = JsonConvert.DeserializeObject<GetCustomerStatementInvoicesSouthernParam>(_APIUtility.Param);

                List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvSouthern = _IiCollectionsRepository.iCollectionsList_GetCustomerStatementInvoicesSouthern(_GetCustStatementInvSouthern, _APIUtility.ConnectionString, _GetCustStatementInvSouthern.includeCredit);

                string JsonData = JsonConvert.SerializeObject(_lstGetCustStatementInvSouthern);

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
        public IActionResult iCollectionsList_GetCustomerStatementInvoices([FromBody] APIRequest _APIRequest)
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

                GetCustomerStatementInvoicesParam _GetCustomerStatementInvoices = JsonConvert.DeserializeObject<GetCustomerStatementInvoicesParam>(_APIUtility.Param);

                List<GetCustStatementInvSouthernViewModel> _lstGetCustomerStatementInvoices = _IiCollectionsRepository.iCollectionsList_GetCustomerStatementInvoices(_GetCustomerStatementInvoices, _APIUtility.ConnectionString, _GetCustomerStatementInvoices.includeCredit);

                string JsonData = JsonConvert.SerializeObject(_lstGetCustomerStatementInvoices);

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
        public IActionResult iCollectionsList_GetEmailDetailByLoc([FromBody] APIRequest _APIRequest)
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

                GetEmailDetailByLocParam _GetEmailDetailByLoc = JsonConvert.DeserializeObject<GetEmailDetailByLocParam>(_APIUtility.Param);

                List<GetEmailDetailByLocViewModel> _lstGetEmailDetailByLoc = _IiCollectionsRepository.iCollectionsList_GetEmailDetailByLoc(_GetEmailDetailByLoc, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEmailDetailByLoc);

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
        public IActionResult iCollectionsList_GetCompanyDetails([FromBody] APIRequest _APIRequest)
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

                List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = _IiCollectionsRepository.iCollectionsList_GetCompanyDetails(_GetCompanyDetailsParam, _APIUtility.ConnectionString);

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
        public IActionResult iCollectionsList_GetCustomerStatementByLocs([FromBody] APIRequest _APIRequest)
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

                GetCustomerStatementByLocsParam _GetCustomerStatementByLocs = JsonConvert.DeserializeObject<GetCustomerStatementByLocsParam>(_APIUtility.Param);

                List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLoc = _IiCollectionsRepository.iCollectionsList_GetCustomerStatementByLocs(_GetCustomerStatementByLocs, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetCustomerStatementByLoc);

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
        public IActionResult iCollectionsList_GetCustomerStatementInvoicesByLocation([FromBody] APIRequest _APIRequest)
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

                GetCustStatementInvByLocationParam _GetCustStatementInvByLocation = JsonConvert.DeserializeObject<GetCustStatementInvByLocationParam>(_APIUtility.Param);

                List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvByLocation = _IiCollectionsRepository.iCollectionsList_GetCustomerStatementInvoicesByLocation(_GetCustStatementInvByLocation, _APIUtility.ConnectionString, _GetCustStatementInvByLocation.includeCredit);

                string JsonData = JsonConvert.SerializeObject(_lstGetCustStatementInvByLocation);

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
        public IActionResult iCollectionsList_GetGLAccount([FromBody] APIRequest _APIRequest)
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

                GetGLAccountParam _GetGLAccount = JsonConvert.DeserializeObject<GetGLAccountParam>(_APIUtility.Param);

                List<GetGLAccountViewModel> _lstGetGLAccount = _IiCollectionsRepository.iCollectionsList_GetGLAccount(_GetGLAccount, _APIUtility.ConnectionString, _GetGLAccount.acct);

                string JsonData = JsonConvert.SerializeObject(_lstGetGLAccount);

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
        public IActionResult iCollectionsList_writeOffInvoice([FromBody] APIRequest _APIRequest)
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

                writeOffInvoiceParam _writeOffInvoice = JsonConvert.DeserializeObject<writeOffInvoiceParam>(_APIUtility.Param);

                _IiCollectionsRepository.iCollectionsList_writeOffInvoice(_writeOffInvoice, _APIUtility.ConnectionString);

                string JsonData = "Successfully write Off Invoice";

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
        public IActionResult iCollectionsList_GetInvoicesByID([FromBody] APIRequest _APIRequest)
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

                GetInvoicesByIDParam _GetInvoicesByID = JsonConvert.DeserializeObject<GetInvoicesByIDParam>(_APIUtility.Param);

                ListGetInvoicesByID _lstGetInvoicesByID = _IiCollectionsRepository.iCollectionsList_GetInvoicesByID(_GetInvoicesByID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoicesByID);

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
        public IActionResult iCollectionsList_GetAutoCompleteBillCodes([FromBody] APIRequest _APIRequest)
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

                GetAutoCompleteBillCodesParam _GetAutoCompleteBillCodes = JsonConvert.DeserializeObject<GetAutoCompleteBillCodesParam>(_APIUtility.Param);

                List<GetAutoCompleteBillCodesViewModel> _lstGetAutoCompleteBillCodes =  _IiCollectionsRepository.iCollectionsList_GetAutoCompleteBillCodes(_GetAutoCompleteBillCodes, _APIUtility.ConnectionString, _GetAutoCompleteBillCodes.prefixText);

                string JsonData = JsonConvert.SerializeObject(_lstGetAutoCompleteBillCodes);

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
        public IActionResult iCollectionsList_GetEmailLogs([FromBody] APIRequest _APIRequest)
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

                GetEmailLogsParam _GetEmailLogs = JsonConvert.DeserializeObject<GetEmailLogsParam>(_APIUtility.Param);

                List<GetEmailLogsViewModel> _lstGetEmailLogs = _IiCollectionsRepository.iCollectionsList_GetEmailLogs(_GetEmailLogs, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetEmailLogs);

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
        public IActionResult iCollectionsList_GetAllTicketID([FromBody] APIRequest _APIRequest)
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

                GetAllTicketIDParam _GetAllTicketID = JsonConvert.DeserializeObject<GetAllTicketIDParam>(_APIUtility.Param);

                List<GetTicketIDViewModel> _lstGetTicketID = _IiCollectionsRepository.iCollectionsList_GetAllTicketID(_GetAllTicketID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetTicketID);

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
        public IActionResult iCollectionsList_GetElevByTicketIDs([FromBody] APIRequest _APIRequest)
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

                GetElevByTicketIDsParam _GetElevByTicketIDs = JsonConvert.DeserializeObject<GetElevByTicketIDsParam>(_APIUtility.Param);

                List<GetElevByTicketIDsViewModel> _lstGetElevByTicketIDs = _IiCollectionsRepository.iCollectionsList_GetElevByTicketIDs(_GetElevByTicketIDs, _APIUtility.ConnectionString, _GetElevByTicketIDs.ticketIDs);

                string JsonData = JsonConvert.SerializeObject(_lstGetElevByTicketIDs);

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
        public IActionResult iCollectionsList_GetActiveBillingCode([FromBody] APIRequest _APIRequest)
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

                GetActiveBillingCodeParam _GetActiveBillingCode = JsonConvert.DeserializeObject<GetActiveBillingCodeParam>(_APIUtility.Param);

                List<GetActiveBillingCodeViewModel> _lstGetActiveBillingCode = _IiCollectionsRepository.iCollectionsList_GetActiveBillingCode(_GetActiveBillingCode, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetActiveBillingCode);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For ARAgingReportCollection List Screen : ARAgingReportCollection.aspx / ARAgingReportCollection.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 


        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_GetSMTPByUserID([FromBody]APIRequest _APIRequest)
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

                List<SMTPEmailViewModel> _lstSMTPEmailViewModel = _IiCollectionsRepository.iCollectionsReport_GetSMTPByUserID(_GetSMTPByUserID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstSMTPEmailViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For CustomerStatementCollectionReport List Screen : CustomerStatementCollectionReport.aspx / CustomerStatementCollectionReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 
        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_GetCustomerStatementCollection([FromBody]APIRequest _APIRequest)
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

                GetCustomerStatementCollectionParam _GetCustomerStatementCollection = JsonConvert.DeserializeObject<GetCustomerStatementCollectionParam>(_APIUtility.Param);

                List<GetCustomerStatementCollectionViewModel> _lstGetCustomerStatementCollection = _IiCollectionsRepository.iCollectionsReport_GetCustomerStatementCollection(_GetCustomerStatementCollection, _APIUtility.ConnectionString, _GetCustomerStatementCollection.includeCredit, _GetCustomerStatementCollection.includeCustomerCredit);

                string JsonData = JsonConvert.SerializeObject(_lstGetCustomerStatementCollection);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For ARAgingReportByTerritoryCollection List Screen : ARAgingReportByTerritoryCollection.aspx / ARAgingReportByTerritoryCollection.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_GetARAgingByTerritory([FromBody]APIRequest _APIRequest)
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

                GetARAgingByTerritoryParam _GetARAgingByTerritory = JsonConvert.DeserializeObject<GetARAgingByTerritoryParam>(_APIUtility.Param);

                ListGetARAgingByTerritory _lstGetARAgingByTerritoryViewModel = _IiCollectionsRepository.iCollectionsReport_GetARAgingByTerritory(_GetARAgingByTerritory, _APIUtility.ConnectionString, _GetARAgingByTerritory.territories);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAgingByTerritoryViewModel);

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
        public IActionResult iCollectionsReport_GetAllTerritory([FromBody]APIRequest _APIRequest)
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

                GetAllTerritoryParam _GetAllTerritory = JsonConvert.DeserializeObject<GetAllTerritoryParam>(_APIUtility.Param);

                List<GetTerritoryViewModel> _lstGetTerritory = _IiCollectionsRepository.iCollectionsReport_GetAllTerritory(_GetAllTerritory, _APIUtility.ConnectionString);

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
        /// For ARAgingReportCust List Screen : ARAgingReportCust.aspx / ARAgingReportCust.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_GetARAging([FromBody]APIRequest _APIRequest)
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

                GetARAgingParam _GetARAging = JsonConvert.DeserializeObject<GetARAgingParam>(_APIUtility.Param);

                ListGetARAging _lstGetARAging = _IiCollectionsRepository.iCollectionsReport_GetARAging(_GetARAging, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAging);

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
        public IActionResult iCollectionsReport_GetARAgingByAsOfDate([FromBody]APIRequest _APIRequest)
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

                GetARAgingByAsOfDateParam _GetARAgingByAsOfDate = JsonConvert.DeserializeObject<GetARAgingByAsOfDateParam>(_APIUtility.Param);

                ListGetARAgingByAsOfDate _lstGetARAgingByAsOfDate = _IiCollectionsRepository.iCollectionsReport_GetARAgingByAsOfDate(_GetARAgingByAsOfDate, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAgingByAsOfDate);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For AgedReceivableReport List Screen : AgedReceivableReport.aspx / AgedReceivableReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 
        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_GetInvoiceByDate([FromBody] APIRequest _APIRequest)
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

                GetInvoiceByDateParam _GetInvoiceByDate = JsonConvert.DeserializeObject<GetInvoiceByDateParam>(_APIUtility.Param);

                List<GetInvoiceByDateViewModel> _lstGetInvoiceByDate = _IiCollectionsRepository.iCollectionsReport_GetInvoiceByDate(_GetInvoiceByDate, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoiceByDate);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For ARAgingReportByLocType List Screen : ARAgingReportByLocType.aspx / ARAgingReportByLocType.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_getlocationType([FromBody] APIRequest _APIRequest)
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

                List<GetLocationTypeViewModel> _lstGetLocationType = _IiCollectionsRepository.iCollectionsReport_getlocationType(_getlocationType, _APIUtility.ConnectionString);

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
        public IActionResult iCollectionsReport_GetARAgingByLocType([FromBody] APIRequest _APIRequest)
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

                GetARAgingByLocTypeParam _GetARAgingByLocType = JsonConvert.DeserializeObject<GetARAgingByLocTypeParam>(_APIUtility.Param);

                List<GetARAgingByLocTypeViewModel> _lstGetARAgingByLocType = _IiCollectionsRepository.iCollectionsReport_GetARAgingByLocType(_GetARAgingByLocType, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAgingByLocType);

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
        public IActionResult iCollectionsReport_GetARAgingByLocTypeDetail([FromBody] APIRequest _APIRequest)
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

                GetARAgingByLocTypeDetailParam _GetARAgingByLocTypeDetail = JsonConvert.DeserializeObject<GetARAgingByLocTypeDetailParam>(_APIUtility.Param);

                List<GetARAgingByLocTypeDetailViewModel> _lstGetARAgingByLocTypeDetail = _IiCollectionsRepository.iCollectionsReport_GetARAgingByLocTypeDetail(_GetARAgingByLocTypeDetail, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAgingByLocTypeDetail);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For ARAgingReport360ByLocation List Screen : ARAgingReport360ByLocation.aspx / ARAgingReport360ByLocation.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_GetARAging360ByAsOfDate([FromBody] APIRequest _APIRequest)
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

                GetARAging360ByAsOfDateParam _GetARAging360ByAsOfDate = JsonConvert.DeserializeObject<GetARAging360ByAsOfDateParam>(_APIUtility.Param);

                ListGetARAging360ByAsOfDate _lstGetARAging360ByAsOfDate = _IiCollectionsRepository.iCollectionsReport_GetARAging360ByAsOfDate(_GetARAging360ByAsOfDate, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAging360ByAsOfDate);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For ARAgingReportDep Screen : ARAgingReportDep.aspx / ARAgingReportDep.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult iCollectionsReport_GetARAgingDep([FromBody] APIRequest _APIRequest)
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

                GetARAgingDepParam _GetARAgingDep = JsonConvert.DeserializeObject<GetARAgingDepParam>(_APIUtility.Param);

                List<GetARAgingDepViewModel> _lstGetARAgingDep = _IiCollectionsRepository.iCollectionsReport_GetARAgingDep(_GetARAgingDep, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAgingDep);

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
        public IActionResult iCollectionsReport_GetARAgingByAsOfDateDep([FromBody] APIRequest _APIRequest)
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

                GetARAgingByAsOfDateDepParam _GetARAgingByAsOfDateDep = JsonConvert.DeserializeObject<GetARAgingByAsOfDateDepParam>(_APIUtility.Param);

                ListGetARAgingByAsOfDateDep _lstGetARAgingByAsOfDateDep = _IiCollectionsRepository.iCollectionsReport_GetARAgingByAsOfDateDep(_GetARAgingByAsOfDateDep, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetARAgingByAsOfDateDep);

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