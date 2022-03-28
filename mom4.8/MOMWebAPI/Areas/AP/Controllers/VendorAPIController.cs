using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MOMWebAPI.Utility;
using Microsoft.AspNetCore.Http;
using BusinessEntity.Utility;
using BusinessEntity.APModels;
using Newtonsoft.Json;
using BusinessEntity;
using BusinessEntity.Payroll;

namespace MOMWebAPI.Areas.AP.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class VendorAPIController : Controller
    {
        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IVendorRepository _IVendorRepository;
        public VendorAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IVendorRepository IVendorRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IVendorRepository = IVendorRepository;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            return Json("Successfully Running WebAPI");
        }

        /// <summary>
        /// For Vendor Screen : Vendor.aspx / Vendor.aspx.cs
        /// </summary>

        [HttpPost]
        [Route("[action]")]
        public IActionResult VendorList_getVendorType([FromBody] APIRequest _APIRequest)
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

                getVendorTypeParam _getVendorTypeParam = JsonConvert.DeserializeObject<getVendorTypeParam>(_APIUtility.Param);

                List<GetVendorTypeViewModel> _GetVendorTypeViewModel = _IVendorRepository.VendorList_getVendorType(_getVendorTypeParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_GetVendorTypeViewModel);

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
        public IActionResult VendorList_GetUserById(APIRequest _APIRequest)
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

                List<UserViewModel> _lstUserViewModel = _IVendorRepository.VendorList_GetUserById(_GetUserByIdParam, _APIUtility.ConnectionString);

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
        public IActionResult VendorList_GetStockReports([FromBody] APIRequest _APIRequest)
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

                List<CustomerReportViewModel> _lstCustomerReportViewModel = _IVendorRepository.VendorList_GetStockReports(_GetStockReportsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstCustomerReportViewModel);

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
        public IActionResult VendorList_GetAllVenderAjaxSearch([FromBody] APIRequest _APIRequest)
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

                GetAllVenderAjaxSearchParam _GetAllVenderAjaxSearchParam = JsonConvert.DeserializeObject<GetAllVenderAjaxSearchParam>(_APIUtility.Param);

                List<GetAllVenderAjaxSearchModel> _lstGetAllVenderAjaxSearch = _IVendorRepository.VendorList_GetAllVenderAjaxSearch(_GetAllVenderAjaxSearchParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAllVenderAjaxSearch);

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
        public IActionResult VendorList_IsExistVendorDetails([FromBody] APIRequest _APIRequest)
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

                IsExistVendorDetailsParam _IsExistVendorDetailsParam = JsonConvert.DeserializeObject<IsExistVendorDetailsParam>(_APIUtility.Param);

                bool _IsExist = _IVendorRepository.VendorList_IsExistVendorDetails(_IsExistVendorDetailsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_IsExist);

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
        public IActionResult VendorList_DeleteRolByID([FromBody] APIRequest _APIRequest)
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

                DeleteRolByIDParam _DeleteRolByIDParam = JsonConvert.DeserializeObject<DeleteRolByIDParam>(_APIUtility.Param);

                _IVendorRepository.VendorList_DeleteRolByID(_DeleteRolByIDParam, _APIUtility.ConnectionString);

                //string JsonData = JsonConvert.SerializeObject(_Rol);
                string JsonData = "Successfully Deleted Rol Record !!";

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
        public IActionResult VendorList_DeleteVendor([FromBody] APIRequest _APIRequest)
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

                DeleteVendorParam _DeleteVendorParam = JsonConvert.DeserializeObject<DeleteVendorParam>(_APIUtility.Param);

                _IVendorRepository.VendorList_DeleteVendor(_DeleteVendorParam, _APIUtility.ConnectionString);

                //string JsonData = JsonConvert.SerializeObject(_lstUser);
                string JsonData = "Successfully Deleted Vendor Record !!";

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For Add Vendor Screen : AddVendor.aspx / AddVendor.aspx.cs
        /// </summary>

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddVendor_GetCompanyByCustomer([FromBody] APIRequest _APIRequest)
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

                List<CompanyOfficeViewModel> _lstCompanyOfficeViewModel = _IVendorRepository.AddVendor_GetCompanyByCustomer(_GetCompanyByCustomerParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_GetStates([FromBody] APIRequest _APIRequest)
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

                GetStatesParam _GetStatesParam = JsonConvert.DeserializeObject<GetStatesParam>(_APIUtility.Param);

                List<StateViewModel> _lstStateViewModel = _IVendorRepository.AddVendor_GetStates(_GetStatesParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstStateViewModel);

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
        public IActionResult AddVendor_GetTerms([FromBody] APIRequest _APIRequest)
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

                GetTermsParam _GetTermsParam = JsonConvert.DeserializeObject<GetTermsParam>(_APIUtility.Param);

                List<TermsViewModel> _lstTermsViewModel = _IVendorRepository.AddVendor_GetTerms(_GetTermsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstTermsViewModel);

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
        public IActionResult AddVendor_GetUseTax([FromBody] APIRequest _APIRequest)
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

                getUseTaxParam _getUseTaxParam = JsonConvert.DeserializeObject<getUseTaxParam>(_APIUtility.Param);

                List<STaxViewModel> _lstUseTaxViewModel = _IVendorRepository.AddVendor_GetUseTax(_getUseTaxParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstUseTaxViewModel);

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
        public IActionResult AddVendor_GetSTax([FromBody] APIRequest _APIRequest)
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

                List<STaxViewModel> _lstSTaxViewModel = _IVendorRepository.AddVendor_GetSTax(_getSTaxParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_GetCustomFields([FromBody] APIRequest _APIRequest)
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

                List<CustomViewModel> _lstCustom = _IVendorRepository.AddVendor_GetCustomFields(_getCustomFieldsParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_GetCustomFieldsControl([FromBody] APIRequest _APIRequest)
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

                List<CustomViewModel> _lstCustom = _IVendorRepository.AddVendor_GetCustomFieldsControl(_getCustomFieldsControlParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_GetChart([FromBody] APIRequest _APIRequest)
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

                List<ChartViewModel> _lstChartViewModel = _IVendorRepository.AddVendor_GetChart(_GetChartParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_GetVendorEdit([FromBody] APIRequest _APIRequest)
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

                GetVendorEditParam _GetVendorEditParam = JsonConvert.DeserializeObject<GetVendorEditParam>(_APIUtility.Param);

                List<VendorViewModel> _lstVendorViewModel = _IVendorRepository.AddVendor_GetVendorEdit(_GetVendorEditParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_GetVendorContactByRolID([FromBody] APIRequest _APIRequest)
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

                getVendorContactByRolIDParam _getVendorContactByRolIDParam = JsonConvert.DeserializeObject<getVendorContactByRolIDParam>(_APIUtility.Param);

                List<GetVendorContactByRolIDViewModel> _lstGetVendorContactByRolIDViewModel = _IVendorRepository.AddVendor_GetVendorContactByRolID(_getVendorContactByRolIDParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetVendorContactByRolIDViewModel);

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
        public IActionResult AddVendor_GetAPExpenses([FromBody] APIRequest _APIRequest)
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

                GetAPExpensesParam _GetAPExpensesParam = JsonConvert.DeserializeObject<GetAPExpensesParam>(_APIUtility.Param);

                ListGetAPExpenses _lstGetAPExpenses = _IVendorRepository.AddVendor_GetAPExpenses(_GetAPExpensesParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetAPExpenses);

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
        public IActionResult AddVendor_GetVendorLogs([FromBody] APIRequest _APIRequest)
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

                GetVendorLogsParam _GetVendorLogsParam = JsonConvert.DeserializeObject<GetVendorLogsParam>(_APIUtility.Param);

                List<LogViewModel> _lstLogViewModel = _IVendorRepository.AddVendor_GetVendorLogs(_GetVendorLogsParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_UpdateRole([FromBody] APIRequest _APIRequest)
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

                UpdateRolParam _UpdateRolParam = JsonConvert.DeserializeObject<UpdateRolParam>(_APIUtility.Param);

                _IVendorRepository.AddVendor_UpdateRol(_UpdateRolParam, _APIUtility.ConnectionString);

                //string JsonData = JsonConvert.SerializeObject(_lstLog);
                string JsonData = "Successfully Updated Rol Record!!!";

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
        public IActionResult AddVendor_UpdateRoles([FromBody] APIRequest _APIRequest)
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

                UpdateRolesParam _UpdateRolesParam = JsonConvert.DeserializeObject<UpdateRolesParam>(_APIUtility.Param);

                _IVendorRepository.AddVendor_UpdateRoles(_UpdateRolesParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Roles";

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
        public IActionResult AddVendor_isExistForUpdateVendor([FromBody] APIRequest _APIRequest)
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

                IsExistForUpdateVendorParam _IsExistForUpdateVendorParam = JsonConvert.DeserializeObject<IsExistForUpdateVendorParam>(_APIUtility.Param);

                List<VendorViewModel> _lstVendorViewModel = _IVendorRepository.AddVendor_isExistForUpdateVendor(_IsExistForUpdateVendorParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_UpdateVendor([FromBody] APIRequest _APIRequest)
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

                UpdateVendorParam _UpdateVendorParam = JsonConvert.DeserializeObject<UpdateVendorParam>(_APIUtility.Param);

                _IVendorRepository.AddVendor_UpdateVendor(_UpdateVendorParam, _APIUtility.ConnectionString);

                //string JsonData = JsonConvert.SerializeObject(_lstVendor);
                string JsonData = "Successfully Updated Vendor Record!!!";

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
        public IActionResult AddVendor_UpdateVendorTax([FromBody] APIRequest _APIRequest)
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

                UpdateVendorTaxParam _UpdateVendorTaxParam = JsonConvert.DeserializeObject<UpdateVendorTaxParam>(_APIUtility.Param);

                _IVendorRepository.AddVendor_UpdateVendorTax(_UpdateVendorTaxParam, _APIUtility.ConnectionString);

                //string JsonData = JsonConvert.SerializeObject(_lstVendor);
                string JsonData = "Successfully Updated Vendor Tax Record!!!";

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
        public IActionResult AddVendor_AddRol([FromBody] APIRequest _APIRequest)
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

                AddRolParam _AddRolParam = JsonConvert.DeserializeObject<AddRolParam>(_APIUtility.Param);

                int returnval = _IVendorRepository.AddVendor_AddRol(_AddRolParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(returnval);

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
        public IActionResult AddVendor_isExistsForInsertVendor([FromBody] APIRequest _APIRequest)
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

                IsExistsForInsertVendorParam _IsExistsForInsertVendorParam = JsonConvert.DeserializeObject<IsExistsForInsertVendorParam>(_APIUtility.Param);

                List<VendorViewModel> _lstVendorViewModel = _IVendorRepository.AddVendor_isExistsForInsertVendor(_IsExistsForInsertVendorParam, _APIUtility.ConnectionString);

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
        public IActionResult AddVendor_AddVendor([FromBody] APIRequest _APIRequest)
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

                AddVendorParam _AddVendorParam = JsonConvert.DeserializeObject<AddVendorParam>(_APIUtility.Param);

                int VendorID = _IVendorRepository.AddVendor_AddVendor(_AddVendorParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(VendorID);

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
        public IActionResult AddVendor_UpdateVendorContact([FromBody] APIRequest _APIRequest)
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

                UpdateVendorContactParam _UpdateVendorContactParam = JsonConvert.DeserializeObject<UpdateVendorContactParam>(_APIUtility.Param);

                _IVendorRepository.AddVendor_UpdateVendorContact(_UpdateVendorContactParam, _APIUtility.ConnectionString);

                //string JsonData = JsonConvert.SerializeObject(_lstVendor);
                string JsonData = "Successfully Updated Vendor Contact Record!!!";

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For VendorLbl5160 Page : VendorLbl5160.aspx / VendorLbl5160.aspx.cs
        /// </summary>
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult VendorReport_GetCompanyDetails([FromBody] APIRequest _APIRequest)
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

                List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = _IVendorRepository.VendorReport_GetCompanyDetails(_GetCompanyDetailsParam, _APIUtility.ConnectionString);

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
        public IActionResult VendorReport_GetVendorLabel([FromBody] APIRequest _APIRequest)
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

                GetVendorLabelParam _GetVendorLabelParam = JsonConvert.DeserializeObject<GetVendorLabelParam>(_APIUtility.Param);

                List<RolViewModel> _lstRolViewModel = _IVendorRepository.VendorReport_GetVendorLabel(_GetVendorLabelParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstRolViewModel);

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
        public IActionResult VendorReport_GetControl([FromBody]APIRequest _APIRequest)
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

                List<GetControlViewModel> _lstGetControlViewModel = _IVendorRepository.VendorReport_GetControl(_getConnectionConfigParam, _APIUtility.ConnectionString);

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
        public IActionResult VendorReport_GetSMTPByUserID([FromBody]APIRequest _APIRequest)
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

                List<SMTPEmailViewModel> _lstSMTPEmailViewModel = _IVendorRepository.VendorReport_GetSMTPByUserID(_user, _APIUtility.ConnectionString);

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
        /// For Vendor1099 Page : Vendor1099.aspx / Vendor1099.aspx.cs
        /// </summary>
        ///


        [HttpPost]
        [Route("[action]")]
        public IActionResult VendorReport_GetFederalReport([FromBody] APIRequest _APIRequest)
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

                GetFederalReportParam _GetFederalReportParam = JsonConvert.DeserializeObject<GetFederalReportParam>(_APIUtility.Param);

                List<VendorViewModel> _lstVendorViewModel = _IVendorRepository.VendorReport_GetFederalReport(_GetFederalReportParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstVendorViewModel);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// For Vendor Transaction History Tab : VendorTransactionHistory.aspx / VendorTransactionHistory.aspx.cs
        /// </summary>
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult VendorTransaction_GetHistoryTransaction([FromBody] APIRequest _APIRequest)
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

                GetHistoryTransactionParam _GetHistoryTransaction = JsonConvert.DeserializeObject<GetHistoryTransactionParam>(_APIUtility.Param);

                List<GetHistoryTransactionViewModel> _lstGetHistoryTransaction = _IVendorRepository.VendorTransaction_GetHistoryTransaction(_GetHistoryTransaction, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetHistoryTransaction);

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