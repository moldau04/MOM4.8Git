using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity;
using BusinessEntity.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOMWebAPI.Areas.AP.Controllers;
using MOMWebAPI.Utility;
using Newtonsoft.Json;
using BusinessEntity.APModels;
using BusinessEntity.payroll;
using BusinessEntity.Payroll;

namespace MOMWebAPI.Areas.AP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillAPIController : Controller
    {
        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IBillRepository _IBillRepository;
        public BillAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IBillRepository IBillRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IBillRepository = IBillRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// For ManageBills Screen : ManageBills.aspx / ManageBills.aspx.cs
        /// </summary>
        /// 


        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsList_GetAllPJDetails([FromBody] APIRequest _APIRequest)
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

                GetAllPJDetailsParam _GetAllPJDetailsParam = JsonConvert.DeserializeObject<GetAllPJDetailsParam>(_APIUtility.Param);

                List<GetAllPJDetailsViewModel> _lstPJViewModel = _IBillRepository.BillsList_GetAllPJDetails(_GetAllPJDetailsParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstPJViewModel);

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
        public IActionResult BillsList_GetAllPJRecurrDetails([FromBody] APIRequest _APIRequest)
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

                GetAllPJRecurrDetailsParam _GetAllPJRecurrDetailsParam = JsonConvert.DeserializeObject<GetAllPJRecurrDetailsParam>(_APIUtility.Param);

                List<GetAllPJRecurrDetailsViewModel> _lstPJViewModel = _IBillRepository.BillsList_GetAllPJRecurrDetails(_GetAllPJRecurrDetailsParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstPJViewModel);

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
        public IActionResult BillsList_ProcessRecurBill([FromBody] APIRequest _APIRequest)
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

                ProcessRecurBillParam _ProcessRecurBillParam = JsonConvert.DeserializeObject<ProcessRecurBillParam>(_APIUtility.Param);

                int RecurCount  = _IBillRepository.BillsList_ProcessRecurBill(_ProcessRecurBillParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(RecurCount);

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
        public IActionResult BillsList_DeleteAPBillRecurr([FromBody] APIRequest _APIRequest)
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

                DeleteAPBillRecurrParam _DeleteAPBillRecurrParam = JsonConvert.DeserializeObject<DeleteAPBillRecurrParam>(_APIUtility.Param);

                _IBillRepository.BillsList_DeleteAPBillRecurr(_DeleteAPBillRecurrParam, _APIUtility.ConnectionString);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully Deleted Record";

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
        public IActionResult BillsList_GetProcessRecurrCount([FromBody] APIRequest _APIRequest)
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

                GetProcessRecurrCountParam _GetProcessRecurrCountParam = JsonConvert.DeserializeObject<GetProcessRecurrCountParam>(_APIUtility.Param);

                List<CDViewModel> _lstCDViewModel = _IBillRepository.BillsList_GetProcessRecurrCount(_GetProcessRecurrCountParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCDViewModel);

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
        public IActionResult BillsList_DeleteAPBill([FromBody] APIRequest _APIRequest)
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


                DeleteAPBillParam _DeleteAPBillParam = JsonConvert.DeserializeObject<DeleteAPBillParam>(_APIUtility.Param);

                _IBillRepository.BillsList_DeleteAPBill(_DeleteAPBillParam, _APIUtility.ConnectionString);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully Deleted Record";

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
        public IActionResult BillsList_UpdateAPDates([FromBody] APIRequest _APIRequest)
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
                UpdateAPDatesParam _UpdateAPDatesParam = JsonConvert.DeserializeObject<UpdateAPDatesParam>(_APIUtility.Param);

                _IBillRepository.BillsList_UpdateAPDates(_UpdateAPDatesParam, _APIUtility.ConnectionString);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully Deleted Record";

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
        public IActionResult BillsList_GetPJAcctDetailByID([FromBody] APIRequest _APIRequest)
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

                GetPJAcctDetailByIDParam _GetPJAcctDetailByIDParam = JsonConvert.DeserializeObject<GetPJAcctDetailByIDParam>(_APIUtility.Param);

                List<GetPJAcctDetailByIDViewModel> _lstGetPJAcctDetailByID = _IBillRepository.BillsList_GetPJAcctDetailByID(_GetPJAcctDetailByIDParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstGetPJAcctDetailByID);

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
        public IActionResult BillsList_GetControl([FromBody]APIRequest _APIRequest)
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

                List<GetControlViewModel> _lstGetControlViewModel = _IBillRepository.BillsList_GetControl(_getConnectionConfigParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsList_GetStockReports([FromBody] APIRequest _APIRequest)
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

                GetStockReportsParam _GetStockReportsParam = JsonConvert.DeserializeObject<GetStockReportsParam>(_APIUtility.Param);

                List<CustomerReportViewModel> _lstCustomerReportViewModel = _IBillRepository.BillsList_GetStockReports(_GetStockReportsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerReportViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For AddBills Screen : AddBills.aspx / AddBills.aspx.cs
        /// </summary>
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddBills_GetOutStandingPOById([FromBody] APIRequest _APIRequest)   
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

                GetOutStandingPOByIdParam _GetOutStandingPOByIdParam = JsonConvert.DeserializeObject<GetOutStandingPOByIdParam>(_APIUtility.Param);

                ListGetOutStandingPOById _lstGetOutStandingPOById = _IBillRepository.AddBills_GetOutStandingPOById( _APIUtility.ConnectionString, _GetOutStandingPOByIdParam);

                string JsonData = JsonConvert.SerializeObject(_lstGetOutStandingPOById);

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
        public IActionResult AddBills_GetVendorSearch([FromBody] APIRequest _APIRequest)
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

                GetVendorSearchParam _GetVendorSearchParam = JsonConvert.DeserializeObject<GetVendorSearchParam>(_APIUtility.Param);

                List<VendorViewModel> _lstVendorViewModel = _IBillRepository.AddBills_GetVendorSearch(_APIUtility.ConnectionString, _GetVendorSearchParam);


                string JsonData = JsonConvert.SerializeObject(_lstVendorViewModel);

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
        public IActionResult AddBills_GetReceivePOList([FromBody] APIRequest _APIRequest)
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

                GetReceivePOListParam _GetReceivePOListParam = JsonConvert.DeserializeObject<GetReceivePOListParam>(_APIUtility.Param);

                List<GetReceivePOListViewModel> _lstGetReceivePOList= _IBillRepository.AddBills_GetReceivePOList(_APIUtility.ConnectionString, _GetReceivePOListParam);


                string JsonData = JsonConvert.SerializeObject(_lstGetReceivePOList);

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
        public IActionResult AddBills_AddReceivePOItem([FromBody] APIRequest _APIRequest)
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

                AddReceivePOItemParam _AddReceivePOItemParam = JsonConvert.DeserializeObject<AddReceivePOItemParam>(_APIUtility.Param);

                _IBillRepository.AddBills_AddReceivePOItem(_APIUtility.ConnectionString, _AddReceivePOItemParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully Added Record";

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
        public IActionResult AddBills_UpdatePOItemBalance([FromBody] APIRequest _APIRequest)
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

                UpdatePOItemBalanceParam _UpdatePOItemBalanceParam = JsonConvert.DeserializeObject<UpdatePOItemBalanceParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdatePOItemBalance(_APIUtility.ConnectionString,_UpdatePOItemBalanceParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully updated Record";

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
        public IActionResult AddBills_UpdatePOItemQuan([FromBody] APIRequest _APIRequest)
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

                UpdatePOItemQuanParam _UpdatePOItemQuanParam = JsonConvert.DeserializeObject<UpdatePOItemQuanParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdatePOItemQuan(_APIUtility.ConnectionString, _UpdatePOItemQuanParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully updated Record";

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
        public IActionResult AddBills_AddEditReceivePO([FromBody] APIRequest _APIRequest)
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

                AddEditReceivePOParam _AddEditReceivePOParam = JsonConvert.DeserializeObject<AddEditReceivePOParam>(_APIUtility.Param);

                _IBillRepository.AddBills_AddEditReceivePO(_APIUtility.ConnectionString, _AddEditReceivePOParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully Added Record";

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
        public IActionResult AddBills_updateJobComm([FromBody] APIRequest _APIRequest)
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

                updateJobCommParam _updateJobCommParam = JsonConvert.DeserializeObject<updateJobCommParam>(_APIUtility.Param);

                _IBillRepository.AddBills_updateJobComm(_APIUtility.ConnectionString, _updateJobCommParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully update Record";

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
        public IActionResult AddBills_GetPOReceivePOById([FromBody] APIRequest _APIRequest)
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

                GetPOReceivePOByIdParam _GetPOReceivePOByIdParam = JsonConvert.DeserializeObject<GetPOReceivePOByIdParam>(_APIUtility.Param);

                ListGetPOReceivePOById _lstPOReceivePOById = _IBillRepository.AddBills_GetPOReceivePOById(_APIUtility.ConnectionString, _GetPOReceivePOByIdParam);


                string JsonData = JsonConvert.SerializeObject(_lstPOReceivePOById);

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
        public IActionResult AddBills_GetPJDetailByID([FromBody] APIRequest _APIRequest)
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

                GetPJDetailByIDParam _GetPJDetailByIDParam = JsonConvert.DeserializeObject<GetPJDetailByIDParam>(_APIUtility.Param);

                List<GetPJDetailByIDViewModel> _lstGetPJDetailByID = _IBillRepository.AddBills_GetPJDetailByID(_GetPJDetailByIDParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstGetPJDetailByID);

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
        public IActionResult AddBills_GetBillTransDetails([FromBody] APIRequest _APIRequest)
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

                GetBillTransDetailsParam _GetBillTransDetailsParam = JsonConvert.DeserializeObject<GetBillTransDetailsParam>(_APIUtility.Param);

                List<GetBillTransDetailsViewModel> _lstGetBillTransDetails = _IBillRepository.AddBills_GetBillTransDetails(_GetBillTransDetailsParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstGetBillTransDetails);

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
        public IActionResult AddBills_GetPJRecurrDetailByID([FromBody] APIRequest _APIRequest)
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

                GetPJRecurrDetailByIDParam _GetPJRecurrDetailByIDParam = JsonConvert.DeserializeObject<GetPJRecurrDetailByIDParam>(_APIUtility.Param);

                List<GetPJRecurrDetailByIDViewModel> _lstGetPJRecurrDetail = _IBillRepository.AddBills_GetPJRecurrDetailByID(_GetPJRecurrDetailByIDParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstGetPJRecurrDetail);

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
        public IActionResult AddBills_GetBillRecurrTransactions([FromBody] APIRequest _APIRequest)
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

                GetBillRecurrTransactionsParam _GetBillRecurrTransactionsParam = JsonConvert.DeserializeObject<GetBillRecurrTransactionsParam>(_APIUtility.Param);

                List<GetBillRecurrTransactionsViewModel> _lstGetBillRecurrTransactions = _IBillRepository.AddBills_GetBillRecurrTransactions(_GetBillRecurrTransactionsParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstGetBillRecurrTransactions);

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
        public IActionResult AddBills_UpdatePOStatus([FromBody] APIRequest _APIRequest)
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

                UpdatePOStatusParam _UpdatePOStatusParam = JsonConvert.DeserializeObject<UpdatePOStatusParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdatePOStatus(_APIUtility.ConnectionString, _UpdatePOStatusParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully updated Record";

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
        public IActionResult AddBills_UpdateReceivePOStatusByPOID([FromBody] APIRequest _APIRequest)
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

                UpdateReceivePOStatusByPOIDParam _UpdateReceivePOStatusByPOIDParam = JsonConvert.DeserializeObject<UpdateReceivePOStatusByPOIDParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdateReceivePOStatusByPOID(_APIUtility.ConnectionString, _UpdateReceivePOStatusByPOIDParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully updated Record";

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
        public IActionResult AddBills_UpdateReceivePOStatus([FromBody] APIRequest _APIRequest)
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

                UpdateReceivePOStatusParam _UpdateReceivePOStatusParam = JsonConvert.DeserializeObject<UpdateReceivePOStatusParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdateReceivePOStatus(_APIUtility.ConnectionString, _UpdateReceivePOStatusParam);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully updated Record";

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
        public IActionResult AddBills_GetBillHistoryPayment([FromBody] APIRequest _APIRequest)
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

                GetBillHistoryPaymentParam _GetBillHistoryPaymentParam = JsonConvert.DeserializeObject<GetBillHistoryPaymentParam>(_APIUtility.Param);

                List<GetBillHistoryPaymentViewModel> _lstPJViewModel = _IBillRepository.AddBills_GetBillHistoryPayment(_GetBillHistoryPaymentParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstPJViewModel);

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
        public IActionResult AddBills_IsExistPO([FromBody] APIRequest _APIRequest)
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

                IsExistPOParam _IsExistPOParam = JsonConvert.DeserializeObject<IsExistPOParam>(_APIUtility.Param);

                bool returnVal = _IBillRepository.AddBills_IsExistPO(_APIUtility.ConnectionString, _IsExistPOParam);


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
        public IActionResult AddBills_GetClosePOCheck([FromBody] APIRequest _APIRequest)
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

                GetClosePOCheckParam _GetClosePOCheckParam = JsonConvert.DeserializeObject<GetClosePOCheckParam>(_APIUtility.Param);

                string returnVal =_IBillRepository.AddBills_GetClosePOCheck(_APIUtility.ConnectionString, _GetClosePOCheckParam);


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
        public IActionResult AddBills_GetMaxReceivePOId([FromBody] APIRequest _APIRequest)
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

                GetMaxReceivePOIdParam _GetMaxReceivePOIdParam = JsonConvert.DeserializeObject<GetMaxReceivePOIdParam>(_APIUtility.Param);

                int returnVal = _IBillRepository.AddBills_GetMaxReceivePOId(_APIUtility.ConnectionString, _GetMaxReceivePOIdParam);


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
        public IActionResult AddBills_GetBomType([FromBody] APIRequest _APIRequest)
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

                GetBomTypeParam _GetBomTypeParam = JsonConvert.DeserializeObject<GetBomTypeParam>(_APIUtility.Param);

                List<BOMTViewModel> _lstPJViewModel = _IBillRepository.AddBills_GetBomType(_GetBomTypeParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstPJViewModel);

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
        public IActionResult AddBills_GetInvDefaultAcct([FromBody] APIRequest _APIRequest)
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

                GetInvDefaultAcctParam _GetInvDefaultAcctParam = JsonConvert.DeserializeObject<GetInvDefaultAcctParam>(_APIUtility.Param);

                List<GeneralViewModel> _lstGeneralViewModel = _IBillRepository.AddBills_GetInvDefaultAcct(_GetInvDefaultAcctParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstGeneralViewModel);

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
        public IActionResult AddBills_GetInventoryItemStatus([FromBody] APIRequest _APIRequest)
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

                GetInventoryItemStatusParam _GetInventoryItemStatusParam = JsonConvert.DeserializeObject<GetInventoryItemStatusParam>(_APIUtility.Param);

                ListGetInventoryItemStatus _lstInventory = _IBillRepository.AddBills_GetInventoryItemStatus( _APIUtility.ConnectionString, _GetInventoryItemStatusParam);


                string JsonData = JsonConvert.SerializeObject(_lstInventory);

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
        public IActionResult AddBills_UpdatePOItemWarehouseLocation([FromBody] APIRequest _APIRequest)
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

                UpdatePOItemWarehouseLocationParam _UpdatePOItemWarehouseLocationParam = JsonConvert.DeserializeObject<UpdatePOItemWarehouseLocationParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdatePOItemWarehouseLocation( _UpdatePOItemWarehouseLocationParam, _APIUtility.ConnectionString);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully update Record";

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
        public IActionResult AddBills_AddReceiveInventoryWHTrans([FromBody] APIRequest _APIRequest)
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

                AddReceiveInventoryWHTransParam _AddReceiveInventoryWHTransParam = JsonConvert.DeserializeObject<AddReceiveInventoryWHTransParam>(_APIUtility.Param);

                _IBillRepository.AddBills_AddReceiveInventoryWHTrans(_AddReceiveInventoryWHTransParam, _APIUtility.ConnectionString);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully update Record";

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
        public IActionResult AddBills_CreateReceivePOInvWarehouse([FromBody] APIRequest _APIRequest)
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

                CreateReceivePOInvWarehouseTransParam _CreateReceivePOInvWarehouseTransParam = JsonConvert.DeserializeObject<CreateReceivePOInvWarehouseTransParam>(_APIUtility.Param);

                _IBillRepository.AddBills_CreateReceivePOInvWarehouse(_CreateReceivePOInvWarehouseTransParam, _APIUtility.ConnectionString);


                //string JsonData = JsonConvert.SerializeObject(RecurCount);
                string JsonData = "Successfully Create Receive PO Inventory Warehouse";

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
        public IActionResult AddBills_GetCustomField([FromBody] APIRequest _APIRequest)
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

                getCustomFieldParam _getCustomFieldParam = JsonConvert.DeserializeObject<getCustomFieldParam>(_APIUtility.Param);

                List<CustomViewModel> _lstCustomViewModels = _IBillRepository.AddBills_GetCustomField(_getCustomFieldParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomViewModels);

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
        public IActionResult AddBills_GetSTax([FromBody] APIRequest _APIRequest)
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

                getSTaxParam _getSTaxParam = JsonConvert.DeserializeObject<getSTaxParam>(_APIUtility.Param);

                List<STaxViewModel> _lstSTaxViewModel = _IBillRepository.AddBills_GetSTax(_getSTaxParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstSTaxViewModel);

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
        public IActionResult AddBills_GetCustomFieldsControl([FromBody] APIRequest _APIRequest)
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

                getCustomFieldsControlParam _getCustomFieldsControlParam = JsonConvert.DeserializeObject<getCustomFieldsControlParam>(_APIUtility.Param);

                List<CustomViewModel> _lstCustomViewModel = _IBillRepository.AddBills_GetCustomFieldsControl(_getCustomFieldsControlParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomViewModel);

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
        public IActionResult AddBills_GetChart([FromBody] APIRequest _APIRequest)
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

                GetChartParam _GetChartParam = JsonConvert.DeserializeObject<GetChartParam>(_APIUtility.Param);

                List<ChartViewModel> _lstChartViewModel = _IBillRepository.AddBills_GetChart(_GetChartParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstChartViewModel);

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
        public IActionResult AddBills_GetVendorByName([FromBody] APIRequest _APIRequest)
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

                GetVendorByNameParam _GetVendorByNameParam = JsonConvert.DeserializeObject<GetVendorByNameParam>(_APIUtility.Param);

                List<VendorViewModel> _lstVendorViewModel = _IBillRepository.AddBills_GetVendorByName(_GetVendorByNameParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstVendorViewModel);

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
        public IActionResult AddBills_GetAutoFillOnHandBalance([FromBody] APIRequest _APIRequest)
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

                GetAutoFillOnHandBalanceParam _GetAutoFillOnHandBalanceParam = JsonConvert.DeserializeObject<GetAutoFillOnHandBalanceParam>(_APIUtility.Param);

                List<GetAutoFillOnHandBalanceViewModel> _lstGetAutoFillOnHandBalanceViewModel = _IBillRepository.AddBills_GetAutoFillOnHandBalance(_GetAutoFillOnHandBalanceParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAutoFillOnHandBalanceViewModel);

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
        public IActionResult AddBills_AddBills([FromBody] APIRequest _APIRequest)
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

                AddBillsParam _AddBillsParam = JsonConvert.DeserializeObject<AddBillsParam>(_APIUtility.Param);

                string strpjid = _IBillRepository.AddBills_AddBills(_AddBillsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(strpjid);

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
        public IActionResult AddBills_UpdateRecurrBills([FromBody] APIRequest _APIRequest)
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

                UpdateRecurrBillsParam _UpdateRecurrBillsParam = JsonConvert.DeserializeObject<UpdateRecurrBillsParam>(_APIUtility.Param);

                string returnVal = _IBillRepository.AddBills_UpdateRecurrBills(_UpdateRecurrBillsParam, _APIUtility.ConnectionString);

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
        public IActionResult AddBills_UpdateBills([FromBody] APIRequest _APIRequest)
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

                UpdateBillsParam _UpdateBillsParam = JsonConvert.DeserializeObject<UpdateBillsParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdateBills(_UpdateBillsParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Bills";

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
        public IActionResult AddBills_GetCustomFields([FromBody] APIRequest _APIRequest)
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

                List<CustomViewModel> _lstCustom = _IBillRepository.AddBills_GetCustomFields(_getCustomFieldsParam, _APIUtility.ConnectionString);

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
        public IActionResult AddBills_CreateReceivePOInvWarehouseTrans([FromBody] APIRequest _APIRequest)
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

                ReceivePOInvWarehouseTransParam _ReceivePOInvWarehouseTransParam = JsonConvert.DeserializeObject<ReceivePOInvWarehouseTransParam>(_APIUtility.Param);

                _IBillRepository.AddBills_CreateReceivePOInvWarehouseTrans(_ReceivePOInvWarehouseTransParam, _APIUtility.ConnectionString);

                string JsonData = "Succesfully Created Receive PO Inventory WareHouse Transaction";

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
        public IActionResult AddBills_UpdateVendorSTax([FromBody] APIRequest _APIRequest)
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

                UpdateVendorSTaxParam _UpdateVendorSTaxParam = JsonConvert.DeserializeObject<UpdateVendorSTaxParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdateVendorSTax(_UpdateVendorSTaxParam, _APIUtility.ConnectionString);

                string JsonData = "Succesfully Update Vendor STax";

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
        public IActionResult AddBills_ISINVENTORYTRACKINGISON([FromBody] APIRequest _APIRequest)
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

                bool returnVal = _IBillRepository.AddBills_ISINVENTORYTRACKINGISON(_APIUtility.ConnectionString);

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
        public IActionResult AddBills_GetUserById(APIRequest _APIRequest)
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

                GetUserByIdParam _GetUserByIdParam = JsonConvert.DeserializeObject<GetUserByIdParam>(_APIUtility.Param);

                List<UserViewModel> _lstUserViewModel = _IBillRepository.AddBills_GetUserById(_GetUserByIdParam, _APIUtility.ConnectionString);

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
        public IActionResult AddBills_UpdateBillsJobDetails(APIRequest _APIRequest)
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

                UpdateBillsJobDetailsParam _UpdateBillsJobDetailsParam = JsonConvert.DeserializeObject<UpdateBillsJobDetailsParam>(_APIUtility.Param);

                _IBillRepository.AddBills_UpdateBillsJobDetails(_UpdateBillsJobDetailsParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Bills Job Details";

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
        public IActionResult AddBills_GetBillingItems(APIRequest _APIRequest)
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

                GetBillingItemsParam _GetBillingItemsParam = JsonConvert.DeserializeObject<GetBillingItemsParam>(_APIUtility.Param);

                ListGetBillingItems _lstGetBillingItems = _IBillRepository.AddBills_GetBillingItems(_GetBillingItemsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetBillingItems);

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
        public IActionResult AddBills_GetControl([FromBody]APIRequest _APIRequest)
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

                List<GetControlViewModel> _lstGetControlViewModel = _IBillRepository.AddBills_GetControl(_getConnectionConfigParam, _APIUtility.ConnectionString);

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
        public IActionResult AddBills_GetBillsLogs([FromBody]APIRequest _APIRequest)
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

                GetBillsLogsParam _GetBillsLogs = JsonConvert.DeserializeObject<GetBillsLogsParam>(_APIUtility.Param);

                List<LogViewModel> _lstLogViewModel = _IBillRepository.AddBills_GetBillsLogs(_GetBillsLogs, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstLogViewModel);

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
        public IActionResult AddBills_UpdateApplyCreditDate([FromBody]APIRequest _APIRequest)
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

                UpdateApplyCreditDateParam _UpdateApplyCreditDateParam = JsonConvert.DeserializeObject<UpdateApplyCreditDateParam>(_APIUtility.Param);

                int rowsAffected = _IBillRepository.AddBills_UpdateApplyCreditDate(_UpdateApplyCreditDateParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(rowsAffected);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For Reports API's : 
        /// </summary>


        /// <summary>
        /// 1) BillListingRepor.aspx / BillListingRepor.aspx.cs
        /// </summary>
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetBillReportFiltersValue([FromBody] APIRequest _APIRequest)
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

                GetBillReportFiltersValueParam _GetBillReportFiltersValueParam = JsonConvert.DeserializeObject<GetBillReportFiltersValueParam>(_APIUtility.Param);

                ListGetBillReportFiltersValue _lstBillReportDetails = _IBillRepository.BillsReport_GetBillReportFiltersValue(_GetBillReportFiltersValueParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstBillReportDetails);

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
        public IActionResult BillsReport_GetBillDetails([FromBody] APIRequest _APIRequest)
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

                GetBillDetailsParam _GetBillDetailsParam = JsonConvert.DeserializeObject<GetBillDetailsParam>(_APIUtility.Param);

                List<BillReportDetails> _lstBillReportDetails = _IBillRepository.BillsReport_GetBillDetails(_GetBillDetailsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstBillReportDetails);

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
        public IActionResult BillsReport_GetAccountSummaryListingDetail([FromBody] APIRequest _APIRequest)
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

                List<UserViewModel> _lstUserViewModel = _IBillRepository.BillsReport_GetAccountSummaryListingDetail(_GetAccountSummaryListingDetailParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetReportDetailById([FromBody] APIRequest _APIRequest)
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

                List<CustomerReportViewModel> _lstCustomerReport = _IBillRepository.BillsReport_GetReportDetailById(_GetReportDetailByIdParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetCustomerType([FromBody] APIRequest _APIRequest)
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

                List<CustomerReportViewModel> _lstCustomerReport = _IBillRepository.BillsReport_GetCustomerType(_lstGetCustomerType, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetCustomerName([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IBillRepository.BillsReport_GetCustomerName(_lstCustomerReportParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerFilter);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //--//


        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetCustomerAddress([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IBillRepository.BillsReport_GetCustomerAddress(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetCustomerCity([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IBillRepository.BillsReport_GetCustomerCity(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetDynamicReports([FromBody] APIRequest _APIRequest)
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

                List<CustomerReportViewModel> _lstCustomerReport = _IBillRepository.BillsReport_GetDynamicReports(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetReportColByRepId([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IBillRepository.BillsReport_GetReportColByRepId(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetReportFiltersByRepId([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IBillRepository.BillsReport_GetReportFiltersByRepId(_lstCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_CheckExistingReport([FromBody] APIRequest _APIRequest)
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

                bool returnVal = _IBillRepository.BillsReport_CheckExistingReport(_CheckExistingReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_InsertCustomerReport([FromBody] APIRequest _APIRequest)
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

                List<CustomerReportViewModel> _lstCustomerReport = _IBillRepository.BillsReport_InsertCustomerReport(_InsertCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_IsStockReportExist([FromBody] APIRequest _APIRequest)
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
                IsStockReportExistParam  _IsStockReportExistParam = JsonConvert.DeserializeObject<IsStockReportExistParam>(_APIUtility.Param);

                bool customertViewModel = _IBillRepository.BillsReport_IsStockReportExist(_IsStockReportExistParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_UpdateCustomerReport([FromBody] APIRequest _APIRequest)
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

                _IBillRepository.BillsReport_UpdateCustomerReport(_UpdateCustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_DeleteCustomerReport([FromBody] APIRequest _APIRequest)
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

                _IBillRepository.BillsReport_DeleteCustomerReport(_CustomerReportParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetControlForReports([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IBillRepository.BillsReport_GetControlForReports(_getConnectionConfigParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetHeaderFooterDetail([FromBody] APIRequest _APIRequest)
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

                List<HeaderFooterDetailViewModel> _lstHeaderFooterDetailViewModel = _IBillRepository.BillsReport_GetHeaderFooterDetail(_customer, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetOwners([FromBody] APIRequest _APIRequest)
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

                List<CustomerFilterViewModel> _lstCustomerFilter = _IBillRepository.BillsReport_GetOwners(_GetOwnersParam, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetColumnWidthByReportId([FromBody] APIRequest _APIRequest)
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

                List<CustomerReportViewModel> _lstCustomerReport = _IBillRepository.BillsReport_GetColumnWidthByReportId(_customer, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_UpdateCustomerReportResizedWidth([FromBody] APIRequest _APIRequest)
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

                _IBillRepository.BillsReport_UpdateCustomerReportResizedWidth(_customer, _APIUtility.ConnectionString);

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
        /// 2) BillsReport.aspx / BillsReport.aspx.cs
        /// </summary>
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetSMTPByUserID([FromBody]APIRequest _APIRequest)
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

                List<SMTPEmailViewModel> _lstSMTPEmailViewModel = _IBillRepository.BillsReport_GetSMTPByUserID(_user, _APIUtility.ConnectionString);

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
        public IActionResult BillsReport_GetCompanyDetails([FromBody] APIRequest _APIRequest)
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

                List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = _IBillRepository.BillsReport_GetCompanyDetails(_GetCompanyDetailsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetCompanyDetailsViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 3) APAgingReport.aspx / APAgingReport.aspx.cs
        /// </summary>

        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetBillsDetailsByDue([FromBody] APIRequest _APIRequest)
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

                GetBillsDetailsByDueParam _GetBillsDetailsByDueParam = JsonConvert.DeserializeObject<GetBillsDetailsByDueParam>(_APIUtility.Param);

                List<GetBillsDetailsByDueViewModel> _lstGetBillsDetailsByDue = _IBillRepository.BillsReport_GetBillsDetailsByDue(_GetBillsDetailsByDueParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetBillsDetailsByDue);

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
        public IActionResult BillsReport_GetAPAgingByDate([FromBody] APIRequest _APIRequest)
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

                GetAPAgingByDateParam _GetAPAgingByDateParam = JsonConvert.DeserializeObject<GetAPAgingByDateParam>(_APIUtility.Param);

                List<GetAPAgingByDateViewModel> _lstGetAPAgingByDateViewModel = _IBillRepository.BillsReport_GetAPAgingByDate(_GetAPAgingByDateParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAPAgingByDateViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 4) PurchaseJournalReport.aspx / PurchaseJournalReport.aspx.cs
        /// </summary>
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetPurchaseJournal([FromBody] APIRequest _APIRequest)
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

                GetPurchaseJournalParam _GetPurchaseJournalParam = JsonConvert.DeserializeObject<GetPurchaseJournalParam>(_APIUtility.Param);

                List<OpenAPViewModel> _lstOpenAPViewModel = _IBillRepository.BillsReport_GetPurchaseJournal(_GetPurchaseJournalParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstOpenAPViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 5) UseTaxReport.aspx / UseTaxReport.aspx.cs
        /// </summary>
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetUseTax([FromBody] APIRequest _APIRequest)
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

                GetUseTaxForReportsParam _GetUseTaxForReportsParam = JsonConvert.DeserializeObject<GetUseTaxForReportsParam>(_APIUtility.Param);

                List<GetUseTaxViewModel> _lstGetUseTax = _IBillRepository.BillsReport_GetUseTax(_GetUseTaxForReportsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetUseTax);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 6) UTaxLocReport.aspx / UTaxLocReport.aspx.cs
        /// </summary>

        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetUTaxLocReport([FromBody] APIRequest _APIRequest)
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

                GetUTaxLocReportParam _GetUTaxLocReportParam = JsonConvert.DeserializeObject<GetUTaxLocReportParam>(_APIUtility.Param);

                List<GetUTaxLocReportViewModel> _lstGetUTaxLocReportViewModel = _IBillRepository.BillsReport_GetUTaxLocReport(_GetUTaxLocReportParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetUTaxLocReportViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 7) PrintBillRegisterGL.aspx / PrintBillRegisterGL.aspx.cs
        /// </summary>
        /// 


        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetAPGLReg([FromBody] APIRequest _APIRequest)
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

                GetAPGLRegParam _GetAPGLRegParam = JsonConvert.DeserializeObject<GetAPGLRegParam>(_APIUtility.Param);

                List<GetAPGLRegViewModel> _lstGetAPGLRegViewModel = _IBillRepository.BillsReport_GetAPGLReg(_GetAPGLRegParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAPGLRegViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 8) APAging360Report.aspx / APAging360Report.aspx.cs
        /// </summary>
        /// 
        [HttpPost]
        [Route("[action]")]
        public IActionResult BillsReport_GetBillsDetails360ByDue([FromBody] APIRequest _APIRequest)
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

                GetBillsDetails360ByDueParam _GetBillsDetails360ByDue = JsonConvert.DeserializeObject<GetBillsDetails360ByDueParam>(_APIUtility.Param);

                List<GetBillsDetails360ByDueViewModel> _lstGetBillsDetails360ByDue = _IBillRepository.BillsReport_GetBillsDetails360ByDue(_GetBillsDetails360ByDue, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetBillsDetails360ByDue);

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
        public IActionResult BillsReport_GetAPAging360ByDate([FromBody] APIRequest _APIRequest)
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

                GetAPAging360ByDateParam _GetAPAging360ByDate = JsonConvert.DeserializeObject<GetAPAging360ByDateParam>(_APIUtility.Param);

                List<GetAPAging360ByDateViewModel> _lstGetAPAging360ByDate = _IBillRepository.BillsReport_GetAPAging360ByDate(_GetAPAging360ByDate, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAPAging360ByDate);

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