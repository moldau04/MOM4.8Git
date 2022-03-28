using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.InventoryModel;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Inventory.Controllers
{
    public class InventoryRepository : IInventoryRepository
    {

        /// <summary>
        /// For Inventory List Screen : Inventory.aspx / Inventory.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryList_Method Name(Parameter)

        public ListGetInventory InventoryList_GetInventory(string ConnectionString, GetInventoryParam _GetInventoryParam)
        {
            return new BusinessLayer.BL_Inventory().GetInventory( ConnectionString, _GetInventoryParam);
        }


        public ListGetSearchInventory InventoryList_GetSearchInventory(string ConnectionString, GetSearchInventoryParam _GetSearchInventoryParam)
        {
            return new BusinessLayer.BL_Inventory().GetSearchInventory(ConnectionString, _GetSearchInventoryParam);
        }

        public List<CommodityViewModel> InventoryList_ReadAllCommodity(string ConnectionString, ReadAllCommodityParam _ReadAllCommodityParam)
        {
            return new BusinessLayer.BL_Inventory().ReadAllCommodity(ConnectionString, _ReadAllCommodityParam);
        }

        public List<VendorViewModel> InventoryList_GetAllVendor(string ConnectionString, GetAllVendorParam _GetAllVendorParam)
        {
            DataSet ds = new DataLayer.DL_Vendor().GetAllVendor(ConnectionString, _GetAllVendorParam);

            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        VType= Convert.ToString(dr["Type"]),
                        Status= Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        ShipVia= Convert.ToString(dr["ShipVia"]),
                        Balance= Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        CLimit= Convert.ToDouble(DBNull.Value.Equals(dr["CLimit"]) ? 0 : dr["CLimit"]),
                        Terms= Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        Days= Convert.ToInt16(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                    }
                    );
            }
            return _lstVendorViewModel;
        }


        public string InventoryList_DeleteInventoryByInvID(string ConnectionString, DeleteInventoryByInvIDParam _DeleteInventoryByInvIDParam)
        {
            return new BusinessLayer.BL_Inventory().DeleteInventoryByInvID(ConnectionString, _DeleteInventoryByInvIDParam);
        }

        public ListGetALLInventory InventoryList_GetALLInventory(string ConnectionString, GetALLInventoryParam _GetALLInventoryParam)
        {
            return new BusinessLayer.BL_Inventory().GetAllInventory(ConnectionString, _GetALLInventoryParam);
        }

        public List<CustomerReportViewModel> InventoryList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetStockReports(_GetStockReportsParam, ConnectionString);
        }


        /// <summary>
        /// For AddInventory Screen : AddInventory.aspx / AddInventory.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryList_Method Name(Parameter)

        public string AddInventory_GetPartNumber(GetPartNumberParam _GetPartNumberParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetPartNumber(_GetPartNumberParam, ConnectionString);
        }

        public bool AddInventory_chkInvForOpen(checkkInvForOpenParam _checkkInvForOpenParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().chkInvForOpen(_checkkInvForOpenParam, ConnectionString);
        }
        public void AddInventory_UpdateInventory(UpdateInventoryParam _UpdateInventoryParam, string ConnectionString)
        {
            new BusinessLayer.BL_Inventory().UpdateInventory(_UpdateInventoryParam, ConnectionString);
        }
        //public InventoryViewModel AddInventory_CreateInventory(CreateInventoryParam _CreateInventoryParam, string ConnectionString)
        //{
        //    return new BusinessLayer.BL_Inventory().CreateInventory(_CreateInventoryParam, ConnectionString);
        //}

        public void AddInventory_CreateInvMergeWarehouse(CreateInvMergeWarehouseParam _CreateInvMergeWarehouseParam, string ConnectionString)
        {
            new BusinessLayer.BL_Inventory().CreateInvMergeWarehouse(_CreateInvMergeWarehouseParam, ConnectionString);
        }
        public void AddInventory_CreateInventoryParts(CreateInventoryPartsParam _CreateInventoryPartsParam, string ConnectionString)
        {
            new BusinessLayer.BL_Inventory().CreateInventoryParts(_CreateInventoryPartsParam, ConnectionString);
        }
        public void AddInventory_DeleteInventoryParts(DeleteInventoryPartsParam _DeleteInventoryPartsParam, string ConnectionString)
        {
            new BusinessLayer.BL_Inventory().DeleteInventoryParts(_DeleteInventoryPartsParam, ConnectionString);
        }
        public void AddInventory_AddFile(AddFileParam _AddFileParam, string ConnectionString)
        {
            new BusinessLayer.BL_MapData().AddFile(_AddFileParam, ConnectionString);
        }
        public void AddInventory_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfoParam, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateDocInfo(_UpdateDocInfoParam, ConnectionString);
        }

        public List<GetDocumentsViewModel> AddInventory_GetDocuments(GetDocumentsParam _GetDocumentsParam, string ConnectionString)
        {
           return new BusinessLayer.BL_MapData().GetDocuments(_GetDocumentsParam, ConnectionString);
        }

        public void AddInventory_DeleteFile(DeleteFileParam _DeleteFileParam, string ConnectionString)
        {
            new BusinessLayer.BL_MapData().DeleteFile(_DeleteFileParam, ConnectionString);
        }
        public List<Chart> AddInventory_GetChartByType(GetChartByTypeParam _GetChartByTypeParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetChartByType(_GetChartByTypeParam, ConnectionString);
        }

        public List<GetInventoryActiveWarehouseViewModel> AddInventory_GetInventoryActiveWarehouse(GetInventoryActiveWarehouseParam _GetInventoryActiveWarehouseParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetInventoryActiveWarehouse(_GetInventoryActiveWarehouseParam, ConnectionString);
        }
        public List<CheckStatusOfChartViewModel> AddInventory_chkStatusOfChart(chkStatusOfChartParam _chkStatusOfChartParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().chkStatusOfChart(_chkStatusOfChartParam, ConnectionString);
        }

        public int AddInventory_CheckWarehouseIsActive(CheckWarehouseIsActiveParam _CheckWarehouseIsActiveParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().CheckWarehouseIsActive(_CheckWarehouseIsActiveParam, ConnectionString);
        }
        public List<GetItemPurchaseOrderByInvIDViewModel> AddInventory_GetItemPurchaseOrderByInvID(GetItemPurchaseOrderByInvIDParam _GetItemPurchaseOrderByInvIDParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetItemPurchaseOrderByInvID(_GetItemPurchaseOrderByInvIDParam, ConnectionString);
        }

        public List<GetAllItemQuantityByInvIDViewModel> AddInventory_GetAllItemQuantityByInvID(GetAllItemQuantityByInvIDParam _GetAllItemQuantityByInvIDParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetAllItemQuantityByInvID(_GetAllItemQuantityByInvIDParam, ConnectionString);
        }

        //public InventoryViewModel AddInventory_GetInventoryByID(GetInventoryByIDParam _GetInventoryByIDParam)
        //{
        //    return new BusinessLayer.BL_Inventory().GetInventoryByID(_GetInventoryByIDParam);
        //}

        public List<InventoryViewModel> AddInventory_GetInvManufacturerInfoByInvAndVendorId(GetInvManufacturerInfoByInvAndVendorIdParam _GetInvManufacturerInfoByInvAndVendorIdParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetInvManufacturerInfoByInvAndVendorId(_GetInvManufacturerInfoByInvAndVendorIdParam, ConnectionString);
        }

        public void AddInventory_CreateItemRevision(CreateItemRevisionParam _CreateItemRevisionParam, string ConnectionString)
        {
            new BusinessLayer.BL_Inventory().CreateItemRevision(_CreateItemRevisionParam, ConnectionString);
        }
        public void AddInventory_DeleteInventoryWareHouse(DeleteInventoryWareHouseParam _DeleteInventoryWareHouseParam, string ConnectionString)
        {
            new BusinessLayer.BL_User().DeleteInventoryWareHouse(_DeleteInventoryWareHouseParam,ConnectionString);
        }

        public List<InventoryTransactionByInvIDViewModel> AddInventory_GetInventoryTransactionByInvID(GetInventoryTransactionByInvIDParam _GetInventoryTransactionByInvIDParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetInventoryTransactionByInvID(_GetInventoryTransactionByInvIDParam, ConnectionString);
        }



        /// <summary>
        /// For InventoryReport Screen : InventoryReport.aspx / InventoryReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryReport_Method Name(Parameter)


        public List<GetCompanyDetailsViewModel> InventoryReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetCompanyDetails(_GetCompanyDetailsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetInventoryTransViewModel> InventoryReport_GetInventoryTrans(GetInventoryTransParam _GetInventoryTransParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Inventory().GetInventoryTrans(_GetInventoryTransParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SMTPEmailViewModel> InventoryReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_user, ConnectionString);
        }

        public List<GetControlViewModel> InventoryReport_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }
    }
}
