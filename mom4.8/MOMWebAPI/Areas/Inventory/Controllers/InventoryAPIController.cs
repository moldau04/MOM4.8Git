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
    public class InventoryAPIController : Controller
    {
        private IUtilityRepository _IUtilityRepository;

        private IUserAuthenticationRepository _IUserAuthenticationRepository;

        private IInventoryRepository _IInventoryRepository;
        public InventoryAPIController(IUtilityRepository IUtilityRepository, IUserAuthenticationRepository IUserAuthenticationRepository, IInventoryRepository IInventoryRepository)
        {

            _IUtilityRepository = IUtilityRepository;
            _IUserAuthenticationRepository = IUserAuthenticationRepository;
            _IInventoryRepository = IInventoryRepository;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult InventoryList_GetInventory([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetInventoryParam _GetInventoryParam = JsonConvert.DeserializeObject<GetInventoryParam>(_APIUtility.Param);

                ListGetInventory _lstInventoryViewModel = _IInventoryRepository.InventoryList_GetInventory(_APIUtility.ConnectionString,_GetInventoryParam);


                string JsonData = JsonConvert.SerializeObject(_lstInventoryViewModel);

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
        public IActionResult InventoryList_GetSearchInventory([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetSearchInventoryParam _GetSearchInventoryParam = JsonConvert.DeserializeObject<GetSearchInventoryParam>(_APIUtility.Param);

                ListGetSearchInventory _lstGetSearchInventory = _IInventoryRepository.InventoryList_GetSearchInventory(_APIUtility.ConnectionString, _GetSearchInventoryParam);


                string JsonData = JsonConvert.SerializeObject(_lstGetSearchInventory);

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
        public IActionResult InventoryList_ReadAllCommodity([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                ReadAllCommodityParam _ReadAllCommodityParam = JsonConvert.DeserializeObject<ReadAllCommodityParam>(_APIUtility.Param);

                List<CommodityViewModel> _lstCommodityViewModel = _IInventoryRepository.InventoryList_ReadAllCommodity(_APIUtility.ConnectionString, _ReadAllCommodityParam);


                string JsonData = JsonConvert.SerializeObject(_lstCommodityViewModel);

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
        public IActionResult InventoryList_DeleteInventoryByInvID([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                DeleteInventoryByInvIDParam _DeleteInventoryByInvIDParam = JsonConvert.DeserializeObject<DeleteInventoryByInvIDParam>(_APIUtility.Param);

                 _IInventoryRepository.InventoryList_DeleteInventoryByInvID(_APIUtility.ConnectionString, _DeleteInventoryByInvIDParam);


                //string JsonData = JsonConvert.SerializeObject(_lstCommodityViewModel);
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
        public IActionResult InventoryList_GetAllInventory([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetALLInventoryParam _GetALLInventoryParam = JsonConvert.DeserializeObject<GetALLInventoryParam>(_APIUtility.Param);

                ListGetALLInventory _lstGetALLInventory = _IInventoryRepository.InventoryList_GetALLInventory(_APIUtility.ConnectionString, _GetALLInventoryParam);


                string JsonData = JsonConvert.SerializeObject(_lstGetALLInventory);
               // string JsonData = "Successfully Deleted Record";

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
        public IActionResult InventoryList_GetAllVendor([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetAllVendorParam _GetAllVendorParam = JsonConvert.DeserializeObject<GetAllVendorParam>(_APIUtility.Param);

                List<VendorViewModel> _lstVendorViewModel = _IInventoryRepository.InventoryList_GetAllVendor(_APIUtility.ConnectionString, _GetAllVendorParam);


                string JsonData = JsonConvert.SerializeObject(_lstVendorViewModel);
                // string JsonData = "Successfully Deleted Record";

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
        public IActionResult InventoryList_GetStockReports([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

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

                List<CustomerReportViewModel> _lstCustomerReport = _IInventoryRepository.InventoryList_GetStockReports(_GetStockReportsParam, _APIUtility.ConnectionString);


                string JsonData = JsonConvert.SerializeObject(_lstCustomerReport);
                // string JsonData = "Successfully Deleted Record";

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// For AddInventory Screen : AddInventory.aspx / AddInventory.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryList_Method Name(Parameter)
        /// 

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddInventory_GetPartNumber([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                GetPartNumberParam _GetPartNumberParam = JsonConvert.DeserializeObject<GetPartNumberParam>(_APIUtility.Param);

                string strPartNo = _IInventoryRepository.AddInventory_GetPartNumber(_GetPartNumberParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(strPartNo);

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
        public IActionResult AddInventory_chkInvForOpen([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                checkkInvForOpenParam _checkkInvForOpenParam = JsonConvert.DeserializeObject<checkkInvForOpenParam>(_APIUtility.Param);

                bool isInvOpen = _IInventoryRepository.AddInventory_chkInvForOpen(_checkkInvForOpenParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(isInvOpen);

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
        public IActionResult AddInventory_UpdateInventory([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                UpdateInventoryParam _UpdateInventoryParam = JsonConvert.DeserializeObject<UpdateInventoryParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_UpdateInventory(_UpdateInventoryParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Inventory";

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
        //public IActionResult AddInventory_CreateInventory([FromBody] APIRequest _APIRequest)
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


        //        CreateInventoryParam _CreateInventoryParam = JsonConvert.DeserializeObject<CreateInventoryParam>(_APIUtility.Param);

        //        List<InventoryViewModel> _lstInventory = _IInventoryRepository.AddInventory_CreateInventory(_CreateInventoryParam, _APIUtility.ConnectionString);

        //        string JsonData = JsonConvert.SerializeObject(_lstInventory);

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
        public IActionResult AddInventory_CreateInvMergeWarehouse([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                CreateInvMergeWarehouseParam _CreateInvMergeWarehouseParam = JsonConvert.DeserializeObject<CreateInvMergeWarehouseParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_CreateInvMergeWarehouse(_CreateInvMergeWarehouseParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Created Inventory Merge Warehouse";

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
        public IActionResult AddInventory_CreateInventoryParts([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                CreateInventoryPartsParam _CreateInventoryPartsParam = JsonConvert.DeserializeObject<CreateInventoryPartsParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_CreateInventoryParts(_CreateInventoryPartsParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Created Inventory Merge Warehouse";

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
        public IActionResult AddInventory_DeleteInventoryParts([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                DeleteInventoryPartsParam _DeleteInventoryPartsParam = JsonConvert.DeserializeObject<DeleteInventoryPartsParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_DeleteInventoryParts(_DeleteInventoryPartsParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Deleted Inventory Parts";

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
        public IActionResult AddInventory_AddFile([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                AddFileParam _AddFileParam = JsonConvert.DeserializeObject<AddFileParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_AddFile(_AddFileParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Created Inventory Merge Warehouse";

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
        public IActionResult AddInventory_UpdateDocInfo([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }


                UpdateDocInfoParam _UpdateDocInfoParam = JsonConvert.DeserializeObject<UpdateDocInfoParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_UpdateDocInfo(_UpdateDocInfoParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Updated Document Information";

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
        public IActionResult AddInventory_GetDocuments([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetDocumentsParam _GetDocumentsParam = JsonConvert.DeserializeObject<GetDocumentsParam>(_APIUtility.Param);

                List<GetDocumentsViewModel> _lstGetDocuments = _IInventoryRepository.AddInventory_GetDocuments(_GetDocumentsParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetDocuments);

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
        public IActionResult AddInventory_DeleteFile([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                DeleteFileParam _DeleteFileParam = JsonConvert.DeserializeObject<DeleteFileParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_DeleteFile(_DeleteFileParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Deleted File";

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
        public IActionResult AddInventory_GetChartByType([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetChartByTypeParam _GetChartByTypeParam = JsonConvert.DeserializeObject<GetChartByTypeParam>(_APIUtility.Param);

                List<Chart> _lstChart = _IInventoryRepository.AddInventory_GetChartByType(_GetChartByTypeParam,_APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstChart);

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
        public IActionResult AddInventory_GetInventoryActiveWarehouse([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetInventoryActiveWarehouseParam _GetInventoryActiveWarehouseParam = JsonConvert.DeserializeObject<GetInventoryActiveWarehouseParam>(_APIUtility.Param);

                List<GetInventoryActiveWarehouseViewModel> _lstGetInventoryActiveWarehouse = _IInventoryRepository.AddInventory_GetInventoryActiveWarehouse(_GetInventoryActiveWarehouseParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInventoryActiveWarehouse);

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
        public IActionResult AddInventory_chkStatusOfChart([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                chkStatusOfChartParam _chkStatusOfChartParam = JsonConvert.DeserializeObject<chkStatusOfChartParam>(_APIUtility.Param);

                List<CheckStatusOfChartViewModel> _lstStatusOfChart = _IInventoryRepository.AddInventory_chkStatusOfChart(_chkStatusOfChartParam,_APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstStatusOfChart);

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
        public IActionResult AddInventory_CheckWarehouseIsActive([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                CheckWarehouseIsActiveParam _CheckWarehouseIsActiveParam = JsonConvert.DeserializeObject<CheckWarehouseIsActiveParam>(_APIUtility.Param);

                int chkWarehouseIsActive = _IInventoryRepository.AddInventory_CheckWarehouseIsActive(_CheckWarehouseIsActiveParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(chkWarehouseIsActive);

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
        public IActionResult AddInventory_GetItemPurchaseOrderByInvID([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetItemPurchaseOrderByInvIDParam _GetItemPurchaseOrderByInvIDParam = JsonConvert.DeserializeObject<GetItemPurchaseOrderByInvIDParam>(_APIUtility.Param);

                List<GetItemPurchaseOrderByInvIDViewModel> _lstItemPurchaseOrder = _IInventoryRepository.AddInventory_GetItemPurchaseOrderByInvID(_GetItemPurchaseOrderByInvIDParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstItemPurchaseOrder);

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
        public IActionResult AddInventory_GetAllItemQuantityByInvID([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetAllItemQuantityByInvIDParam _GetAllItemQuantityByInvIDParam = JsonConvert.DeserializeObject<GetAllItemQuantityByInvIDParam>(_APIUtility.Param);

                List<GetAllItemQuantityByInvIDViewModel> _lstAllItemQuantity = _IInventoryRepository.AddInventory_GetAllItemQuantityByInvID(_GetAllItemQuantityByInvIDParam,_APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstAllItemQuantity);

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
        //public IActionResult AddInventory_GetInventoryByID([FromBody] APIRequest _APIRequest)
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

        //        GetInventoryByIDParam _GetInventoryByIDParam = JsonConvert.DeserializeObject<GetInventoryByIDParam>(_APIUtility.Param);

        //        List<BusinessEntity.Inventory> _lstInventory = _IInventoryRepository.AddInventory_GetInventoryByID(_GetInventoryByIDParam);

        //        string JsonData = JsonConvert.SerializeObject(_lstInventory);

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
        public IActionResult AddInventory_GetInvManufacturerInfoByInvAndVendorId([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetInvManufacturerInfoByInvAndVendorIdParam _GetInvManufacturerInfoByInvAndVendorIdParam = JsonConvert.DeserializeObject<GetInvManufacturerInfoByInvAndVendorIdParam>(_APIUtility.Param);

                List<InventoryViewModel> _lstInvManufacturerInfoByInvAndVendorIdParam = _IInventoryRepository.AddInventory_GetInvManufacturerInfoByInvAndVendorId(_GetInvManufacturerInfoByInvAndVendorIdParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstInvManufacturerInfoByInvAndVendorIdParam);

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
        public IActionResult AddInventory_CreateItemRevision([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                CreateItemRevisionParam _CreateItemRevisionParam = JsonConvert.DeserializeObject<CreateItemRevisionParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_CreateItemRevision(_CreateItemRevisionParam,_APIUtility.ConnectionString);

                string JsonData = "Successfully Created Item Revision";

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
        public IActionResult AddInventory_DeleteInventoryWareHouse([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                DeleteInventoryWareHouseParam _DeleteInventoryWareHouseParam = JsonConvert.DeserializeObject<DeleteInventoryWareHouseParam>(_APIUtility.Param);

                _IInventoryRepository.AddInventory_DeleteInventoryWareHouse(_DeleteInventoryWareHouseParam, _APIUtility.ConnectionString);

                string JsonData = "Successfully Deleted Inventory WareHouse";

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
        public IActionResult AddInventory_GetInventoryTransactionByInvID([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetInventoryTransactionByInvIDParam _GetInventoryTransactionByInvIDParam = JsonConvert.DeserializeObject<GetInventoryTransactionByInvIDParam>(_APIUtility.Param);

               List<InventoryTransactionByInvIDViewModel> _lstInventoryTransaction = _IInventoryRepository.AddInventory_GetInventoryTransactionByInvID(_GetInventoryTransactionByInvIDParam,_APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstInventoryTransaction);

                _response.ResponseData = _IUtilityRepository.Encrypt(JsonData);

                _response.statusCode = StatusCodes.Status200OK.ToString(); _response.value = "OK"; return Json(_response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// For InventoryReport Screen : InventoryReport.aspx / InventoryReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryReport_Method Name(Parameter)


        [HttpPost]
        [Route("[action]")]
        public IActionResult InventoryReport_GetCompanyDetails([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

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

                List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = _IInventoryRepository.InventoryReport_GetCompanyDetails(_GetCompanyDetailsParam, _APIUtility.ConnectionString);

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
        public IActionResult InventoryReport_GetInventoryTrans([FromBody] APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

                UserAuthentication _US = _IUserAuthenticationRepository.GetUserAuthentication(_APIUtility);

                if (!_US.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "Invalid User"; return Json(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.statusCode = StatusCodes.Status400BadRequest.ToString(); _response.value = "No Record found"; return BadRequest(_response);
                }

                GetInventoryTransParam _GetInventoryTransParam = JsonConvert.DeserializeObject<GetInventoryTransParam>(_APIUtility.Param);

                List<GetInventoryTransViewModel> _lstGetInventoryTrans = _IInventoryRepository.InventoryReport_GetInventoryTrans(_GetInventoryTransParam, _APIUtility.ConnectionString);

                string JsonData = JsonConvert.SerializeObject(_lstGetInventoryTrans);

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
        public IActionResult InventoryReport_GetSMTPByUserID([FromBody]APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

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

                List<SMTPEmailViewModel> _lstSMTPEmailViewModel = _IInventoryRepository.InventoryReport_GetSMTPByUserID(_user, _APIUtility.ConnectionString);

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
        public IActionResult InventoryReport_GetControl([FromBody]APIRequest _APIRequest)
        {
            try
            {
                APIUtility _APIUtility = _IUtilityRepository.GetAPIUtility(_APIRequest);

                APIResponse _response = new APIResponse();

                _response.contentType = "application/json";

                _response.statusCode = StatusCodes.Status200OK.ToString();

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

                List<GetControlViewModel> _lstGetControlViewModel = _IInventoryRepository.InventoryReport_GetControl(_getConnectionConfigParam, _APIUtility.ConnectionString);

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