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
    public class ReceivePaymentAPIController : Controller
    {
        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IReceivePaymentRepository _IReceivePaymentRepository;
        public ReceivePaymentAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IReceivePaymentRepository IReceivePaymentRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IReceivePaymentRepository = IReceivePaymentRepository;
        }


        /// <summary>
        /// For ReceivePayment List Screen : ReceivePayment.aspx / ReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ReceivePaymentList_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult ReceivePaymentList_UpdateCustomerBalance([FromBody] APIRequest _APIRequest)
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

                UpdateCustomerBalanceParam _UpdateCustomerBalance = JsonConvert.DeserializeObject<UpdateCustomerBalanceParam>(_APIUtility.Param);

                _IReceivePaymentRepository.ReceivePaymentList_UpdateCustomerBalance(_UpdateCustomerBalance, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Customer Balance";

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
        public IActionResult ReceivePaymentList_DeletePayment([FromBody] APIRequest _APIRequest)
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

                DeletePaymentParam _DeletePayment = JsonConvert.DeserializeObject<DeletePaymentParam>(_APIUtility.Param);

                _IReceivePaymentRepository.ReceivePaymentList_DeletePayment(_DeletePayment, _APIUtility.ConnectionString);

                string JsonData = "Successfully Deleted Payment";

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpPost]
        //[Route("[action]")]
        //public IActionResult ReceivePaymentList_GetAllReceivePayment([FromBody] APIRequest _APIRequest)
        //{
        //    try
        //    {
        //        APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

        //        APIResponse _response = new APIResponse();

        //        _response.contentType = "application/json";

        //        _response.statusCode = StatusCodes.Status200OK.ToString();

        //        UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

        //        if (!_US.IsValid)
        //        {
        //            _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
        //        }

        //        GetAllReceivePaymentParam _GetAllReceivePayment = JsonConvert.DeserializeObject<GetAllReceivePaymentParam>(_APIUtility.Param);

        //        List<GetAllReceivePaymentViewModel> _lstGetAllReceivePayment = _IReceivePaymentRepository.ReceivePaymentList_GetAllReceivePayment(_GetAllReceivePayment, _APIUtility.ConnectionString, _GetAllReceivePayment.filters, _GetAllReceivePayment.intEN);

        //        string JsonData = JsonConvert.SerializeObject(_lstGetAllReceivePayment);

        //        _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

        //        _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        /// <summary>
        /// For AddReceivePayment Screen : AddReceivePayment.aspx / AddReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddReceivePayment_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddReceivePayment_GetInvoiceByCustomerID([FromBody] APIRequest _APIRequest)
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

                GetInvoiceByCustomerIDParam _GetInvoiceByCustomerID = JsonConvert.DeserializeObject<GetInvoiceByCustomerIDParam>(_APIUtility.Param);

                ListGetInvoiceByCustomerID _lstGetInvoiceByCustomerID = _IReceivePaymentRepository.AddReceivePayment_GetInvoiceByCustomerID(_GetInvoiceByCustomerID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoiceByCustomerID);

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
        public IActionResult AddReceivePayment_GetInvoiceNosChange([FromBody] APIRequest _APIRequest)
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

                GetInvoiceNosChangeParam _GetInvoiceNosChange = JsonConvert.DeserializeObject<GetInvoiceNosChangeParam>(_APIUtility.Param);

                List<GetInvoiceNosChangeViewModel> _lstGetInvoiceNosChange = _IReceivePaymentRepository.AddReceivePayment_GetInvoiceNosChange(_GetInvoiceNosChange, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoiceNosChange);

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
        public IActionResult AddReceivePayment_UpdateReceivePayment([FromBody] APIRequest _APIRequest)
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

                UpdateReceivePaymentParam _UpdateReceivePayment = JsonConvert.DeserializeObject<UpdateReceivePaymentParam>(_APIUtility.Param);

                _IReceivePaymentRepository.AddReceivePayment_UpdateReceivePayment(_UpdateReceivePayment, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Receive Payment";

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
        public IActionResult AddReceivePayment_AddReceivePayment([FromBody] APIRequest _APIRequest)
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

                AddReceivePaymentParam _AddReceivePayment = JsonConvert.DeserializeObject<AddReceivePaymentParam>(_APIUtility.Param);

                int returnVal = _IReceivePaymentRepository.AddReceivePayment_AddReceivePayment(_AddReceivePayment, _APIUtility.ConnectionString);

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
        public IActionResult AddReceivePayment_GetInvoicesByReceivedPay([FromBody] APIRequest _APIRequest)
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

                GetInvoicesByReceivedPayParam _GetInvoicesByReceivedPay = JsonConvert.DeserializeObject<GetInvoicesByReceivedPayParam>(_APIUtility.Param);

                ListGetInvoicesByReceivedPay _lstGetInvoicesByReceivedPay = _IReceivePaymentRepository.AddReceivePayment_GetInvoicesByReceivedPay(_GetInvoicesByReceivedPay, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoicesByReceivedPay);

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
        public IActionResult AddReceivePayment_GetUndepositeAcct([FromBody] APIRequest _APIRequest)
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

                GetUndepositeAcctParam _GetUndepositeAcct = JsonConvert.DeserializeObject<GetUndepositeAcctParam>(_APIUtility.Param);

                List<GetUndepositeAcctViewModel> _lstGetUndepositeAcct = _IReceivePaymentRepository.AddReceivePayment_GetUndepositeAcct(_GetUndepositeAcct, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetUndepositeAcct);

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
        public IActionResult AddReceivePayment_GetReceivePaymentByID([FromBody] APIRequest _APIRequest)
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

                GetReceivePaymentByIDParam _GetReceivePaymentByID = JsonConvert.DeserializeObject<GetReceivePaymentByIDParam>(_APIUtility.Param);

                List<GetReceivePaymentByIDViewModel> _lstGetReceivePaymentByID = _IReceivePaymentRepository.AddReceivePayment_GetReceivePaymentByID(_GetReceivePaymentByID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetReceivePaymentByID);

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
        public IActionResult AddReceivePayment_GetScreensByUser([FromBody] APIRequest _APIRequest)
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

                GetScreensByUserParam _GetScreensByUser = JsonConvert.DeserializeObject<GetScreensByUserParam>(_APIUtility.Param);

                List<GetScreensByUserViewModel> _lstGetScreensByUser = _IReceivePaymentRepository.AddReceivePayment_GetScreensByUser(_GetScreensByUser, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetScreensByUser);

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
        public IActionResult AddReceivePayment_GetReceivePaymentLogs([FromBody] APIRequest _APIRequest)
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

                GetReceivePaymentLogsParam _GetReceivePaymentLogs = JsonConvert.DeserializeObject<GetReceivePaymentLogsParam>(_APIUtility.Param);

                List<GetLocationLogViewModel> _lstGetReceivePaymentLogs = _IReceivePaymentRepository.AddReceivePayment_GetReceivePaymentLogs(_GetReceivePaymentLogs, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetReceivePaymentLogs);

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
        public IActionResult AddReceivePayment_GetCustomerUnAppliedCredit([FromBody] APIRequest _APIRequest)
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

                GetCustomerUnAppliedCreditParam _GetCustomerUnAppliedCredit = JsonConvert.DeserializeObject<GetCustomerUnAppliedCreditParam>(_APIUtility.Param);

                List<GetCustomerUnAppliedCreditViewModel> _lstGetCustomerUnAppliedCredit = _IReceivePaymentRepository.AddReceivePayment_GetCustomerUnAppliedCredit(_GetCustomerUnAppliedCredit, _APIUtility.ConnectionString, _GetCustomerUnAppliedCredit.userId, _GetCustomerUnAppliedCredit.filter);

                string JsonData = JsonConvert.SerializeObject(_lstGetCustomerUnAppliedCredit);

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
        public IActionResult AddReceivePayment_GetInvoicesByID([FromBody] APIRequest _APIRequest)
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

                ListGetInvoicesByID _lstGetInvoicesByID = _IReceivePaymentRepository.AddReceivePayment_GetInvoicesByID(_GetInvoicesByID, _APIUtility.ConnectionString);

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
        public IActionResult AddReceivePayment_GetActiveBillingCode([FromBody] APIRequest _APIRequest)
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

                List<GetActiveBillingCodeViewModel> _lstGetActiveBillingCode = _IReceivePaymentRepository.AddReceivePayment_GetActiveBillingCode(_GetActiveBillingCode, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetActiveBillingCode);

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
        public IActionResult AddReceivePayment_writeOffInvoiceMulti([FromBody] APIRequest _APIRequest)
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

                writeOffInvoiceMultiParam _writeOffInvoiceMulti = JsonConvert.DeserializeObject<writeOffInvoiceMultiParam>(_APIUtility.Param);

                _IReceivePaymentRepository.AddReceivePayment_writeOffInvoiceMulti(_writeOffInvoiceMulti, _APIUtility.ConnectionString);

                string JsonData = "Successfully write Off Invoice Multi";

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
        public IActionResult AddReceivePayment_writeOffInvoice([FromBody] APIRequest _APIRequest)
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

                _IReceivePaymentRepository.AddReceivePayment_writeOffInvoice(_writeOffInvoice, _APIUtility.ConnectionString);

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
        public IActionResult AddReceivePayment_GetLocationByID([FromBody] APIRequest _APIRequest)
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

                ListGetLocationByID _lstGetLocationByID = _IReceivePaymentRepository.AddReceivePayment_GetLocationByID(_GetLocationByID, _APIUtility.ConnectionString);

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
        public IActionResult AddReceivePayment_TransferPayment([FromBody] APIRequest _APIRequest)
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

                TransferPaymentParam _TransferPayment = JsonConvert.DeserializeObject<TransferPaymentParam>(_APIUtility.Param);

                _IReceivePaymentRepository.AddReceivePayment_TransferPayment(_TransferPayment, _APIUtility.ConnectionString, _TransferPayment.strRef, _TransferPayment.newLoc);

                string JsonData = "Successfully Transfer Payment";

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
        public IActionResult AddReceivePayment_UnapplyPayment([FromBody] APIRequest _APIRequest)
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

                UnapplyPaymentParam _UnapplyPayment = JsonConvert.DeserializeObject<UnapplyPaymentParam>(_APIUtility.Param);

                _IReceivePaymentRepository.AddReceivePayment_UnapplyPayment(_UnapplyPayment, _APIUtility.ConnectionString, _UnapplyPayment.Ref);

                string JsonData = "Successfully Unapply Payment";

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For AddMultiReceivePayment Screen : AddMultiReceivePayment.aspx / AddMultiReceivePayment.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddMultiReceivePayment_Method Name(Parameter)
        /// 
        [HttpPost]
        [Route("[action]")]
        public IActionResult AddMultiReceivePayment_GetAllBankNames([FromBody] APIRequest _APIRequest)
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

                GetAllBankNamesParam _GetAllBankNames = JsonConvert.DeserializeObject<GetAllBankNamesParam>(_APIUtility.Param);

                List<GetAllBankNamesViewModel> _lstGetAllBankNames = _IReceivePaymentRepository.AddMultiReceivePayment_GetAllBankNames(_GetAllBankNames,_APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAllBankNames);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpPost]
        //[Route("[action]")]
        //public IActionResult AddMultiReceivePayment_AddBatchReceivePayment([FromBody] APIRequest _APIRequest)
        //{
        //    try
        //    {
        //        APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

        //        APIResponse _response = new APIResponse();

        //        _response.contentType = "application/json";

        //        _response.statusCode = StatusCodes.Status200OK.ToString();

        //        UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

        //        if (!_US.IsValid)
        //        {
        //            _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
        //        }

        //        AddBatchReceivePaymentParam _AddBatchReceivePayment = JsonConvert.DeserializeObject<AddBatchReceivePaymentParam>(_APIUtility.Param);

        //        List<AddBatchReceivePaymentViewModel> _lstAddBatchReceivePayment = _IReceivePaymentRepository.AddMultiReceivePayment_AddBatchReceivePayment(_AddBatchReceivePayment, _APIUtility.ConnectionString);

        //        string JsonData = JsonConvert.SerializeObject(_lstAddBatchReceivePayment);

        //        _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

        //        _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddMultiReceivePayment_GetDepByID([FromBody] APIRequest _APIRequest)
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

                GetDepByIDParam _GetDepByID = JsonConvert.DeserializeObject<GetDepByIDParam>(_APIUtility.Param);

                List<GetDepByIDViewModel> _lstGetDepByID = _IReceivePaymentRepository.AddMultiReceivePayment_GetDepByID(_GetDepByID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetDepByID);

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
        public IActionResult AddMultiReceivePayment_GetDepHeadByID([FromBody] APIRequest _APIRequest)
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

                GetDepHeadByIDParam _GetDepHeadByID = JsonConvert.DeserializeObject<GetDepHeadByIDParam>(_APIUtility.Param);

                List<GetDepHeadByIDViewModel> _lstGetDepHeadByID = _IReceivePaymentRepository.AddMultiReceivePayment_GetDepHeadByID(_GetDepHeadByID, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetDepHeadByID);

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
        public IActionResult AddMultiReceivePayment_GetReceivedPaymentByDep([FromBody] APIRequest _APIRequest)
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

                GetReceivedPaymentByDepParam _GetReceivedPaymentByDep = JsonConvert.DeserializeObject<GetReceivedPaymentByDepParam>(_APIUtility.Param);

                ListGetReceivedPaymentByDep _lstGetReceivedPaymentByDep = _IReceivePaymentRepository.AddMultiReceivePayment_GetReceivedPaymentByDep(_GetReceivedPaymentByDep, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetReceivedPaymentByDep);

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
        public IActionResult AddMultiReceivePayment_GetInvoiceByList([FromBody] APIRequest _APIRequest)
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

                GetInvoiceByListParam _GetInvoiceByList = JsonConvert.DeserializeObject<GetInvoiceByListParam>(_APIUtility.Param);

                List<GetInvoiceByListViewModel> _lstGetInvoiceByList = _IReceivePaymentRepository.AddMultiReceivePayment_GetInvoiceByList(_GetInvoiceByList, _APIUtility.ConnectionString, _GetInvoiceByList.invoiceId, _GetInvoiceByList.checkNumber, _GetInvoiceByList.isSeparate);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoiceByList);

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
        public IActionResult AddMultiReceivePayment_GetInvoicesByReceivedPayMulti([FromBody] APIRequest _APIRequest)
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

                GetInvoicesByReceivedPayMultiParam _GetInvoicesByReceivedPayMulti = JsonConvert.DeserializeObject<GetInvoicesByReceivedPayMultiParam>(_APIUtility.Param);

                List<GetInvoicesByReceivedPayMultiViewModel> _lstGetInvoicesByReceivedPayMulti = _IReceivePaymentRepository.AddMultiReceivePayment_GetInvoicesByReceivedPayMulti(_GetInvoicesByReceivedPayMulti, _APIUtility.ConnectionString, _GetInvoicesByReceivedPayMulti.owner, _GetInvoicesByReceivedPayMulti.loc, _GetInvoicesByReceivedPayMulti.invoice);

                string JsonData = JsonConvert.SerializeObject(_lstGetInvoicesByReceivedPayMulti);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For ReceivePaymentListReport Screen : ReceivePaymentListReport.aspx / ReceivePaymentListReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ReceivePaymentListReport_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult ReceivePaymentListReport_GetCompanyDetails([FromBody] APIRequest _APIRequest)
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

                GetCompanyDetailsParam _GetCompanyDetails = JsonConvert.DeserializeObject<GetCompanyDetailsParam>(_APIUtility.Param);

                List<GetCompanyDetailsViewModel> _lstGetCompanyDetails = _IReceivePaymentRepository.ReceivePaymentListReport_GetCompanyDetails(_GetCompanyDetails, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetCompanyDetails);

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
        public IActionResult ReceivePaymentListReport_GetSMTPByUserID([FromBody]APIRequest _APIRequest)
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

                List<SMTPEmailViewModel> _lstSMTPEmailViewModel = _IReceivePaymentRepository.ReceivePaymentListReport_GetSMTPByUserID(_GetSMTPByUserID, _APIUtility.ConnectionString);

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
        public IActionResult ReceivePaymentListReport_GetControl([FromBody]APIRequest _APIRequest)
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

                List<GetControlViewModel> _lstGetControlViewModel = _IReceivePaymentRepository.ReceivePaymentListReport_GetControl(_getConnectionConfigParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetControlViewModel);

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