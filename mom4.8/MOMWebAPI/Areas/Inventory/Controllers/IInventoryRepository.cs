using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.InventoryModel;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Inventory.Controllers
{
    public interface IInventoryRepository
    {

        /// <summary>
        /// For Inventory List Screen : Inventory.aspx / Inventory.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryList_Method Name(Parameter)

        ListGetInventory InventoryList_GetInventory(string ConnectionString, GetInventoryParam _GetInventoryParam);

        ListGetSearchInventory InventoryList_GetSearchInventory(string ConnectionString, GetSearchInventoryParam _GetSearchInventoryParam);

         List<CommodityViewModel> InventoryList_ReadAllCommodity(
             string ConnectionString, ReadAllCommodityParam _ReadAllCommodityParam);
        List<VendorViewModel> InventoryList_GetAllVendor(string ConnectionString, GetAllVendorParam _GetAllVendorParam);
        string InventoryList_DeleteInventoryByInvID(string ConnectionString, DeleteInventoryByInvIDParam _DeleteInventoryByInvIDParam);

        ListGetALLInventory InventoryList_GetALLInventory(string ConnectionString, GetALLInventoryParam _GetALLInventoryParam);

        List<CustomerReportViewModel> InventoryList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string ConnectionString);


        /// <summary>
        /// For AddInventory Screen : AddInventory.aspx / AddInventory.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddInventory_Method Name(Parameter)

        string AddInventory_GetPartNumber(GetPartNumberParam _GetPartNumberParam, string ConnectionString);
        bool AddInventory_chkInvForOpen(checkkInvForOpenParam _checkkInvForOpenParam, string ConnectionString);
        void AddInventory_UpdateInventory(UpdateInventoryParam _UpdateInventoryParam, string ConnectionString); 
        //InventoryViewModel AddInventory_CreateInventory(CreateInventoryParam _CreateInventoryParam, string ConnectionString); 
        void AddInventory_CreateInvMergeWarehouse(CreateInvMergeWarehouseParam _CreateInvMergeWarehouseParam, string ConnectionString);
        void AddInventory_CreateInventoryParts(CreateInventoryPartsParam _CreateInventoryPartsParam, string ConnectionString);
        void AddInventory_DeleteInventoryParts(DeleteInventoryPartsParam _DeleteInventoryPartsParam, string ConnectionString);
        void AddInventory_AddFile(AddFileParam _AddFileParam, string ConnectionString);
        void AddInventory_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfoParam, string ConnectionString);
        List<GetDocumentsViewModel> AddInventory_GetDocuments(GetDocumentsParam _GetDocumentsParam, string ConnectionString);
        void AddInventory_DeleteFile(DeleteFileParam _DeleteFileParam, string ConnectionString);
        List<Chart> AddInventory_GetChartByType(GetChartByTypeParam _GetChartByTypeParam, string ConnectionString);
        List<GetInventoryActiveWarehouseViewModel> AddInventory_GetInventoryActiveWarehouse(GetInventoryActiveWarehouseParam _GetInventoryActiveWarehouseParam, string ConnectionString);
        List<CheckStatusOfChartViewModel> AddInventory_chkStatusOfChart(chkStatusOfChartParam _chkStatusOfChartParam, string ConnectionString);
        int AddInventory_CheckWarehouseIsActive(CheckWarehouseIsActiveParam _CheckWarehouseIsActiveParam, string ConnectionString);
        List<GetItemPurchaseOrderByInvIDViewModel> AddInventory_GetItemPurchaseOrderByInvID(GetItemPurchaseOrderByInvIDParam _GetItemPurchaseOrderByInvIDParam, string ConnectionString);
        List<GetAllItemQuantityByInvIDViewModel> AddInventory_GetAllItemQuantityByInvID(GetAllItemQuantityByInvIDParam _GetAllItemQuantityByInvIDParam, string ConnectionString);

        //InventoryViewModel AddInventory_GetInventoryByID(GetInventoryByIDParam _GetInventoryByIDParam);

        List<InventoryViewModel> AddInventory_GetInvManufacturerInfoByInvAndVendorId(GetInvManufacturerInfoByInvAndVendorIdParam _GetInvManufacturerInfoByInvAndVendorIdParam, string ConnectionString);
        void AddInventory_CreateItemRevision(CreateItemRevisionParam _CreateItemRevisionParam, string ConnectionString);
        void AddInventory_DeleteInventoryWareHouse(DeleteInventoryWareHouseParam _DeleteInventoryWareHouseParam, string ConnectionString);
        List<InventoryTransactionByInvIDViewModel> AddInventory_GetInventoryTransactionByInvID(GetInventoryTransactionByInvIDParam _GetInventoryTransactionByInvIDParam, string ConnectionString);



        /// <summary>
        /// For InventoryReport Screen : InventoryReport.aspx / InventoryReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryReport_Method Name(Parameter)

        List<GetCompanyDetailsViewModel> InventoryReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString);
        List<GetInventoryTransViewModel> InventoryReport_GetInventoryTrans(GetInventoryTransParam _GetInventoryTransParam, string ConnectionString);
        List<SMTPEmailViewModel> InventoryReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString);
        List<GetControlViewModel> InventoryReport_GetControl(getConnectionConfigParam _user, string ConnectionString);
        
    }
}
