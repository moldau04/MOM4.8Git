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
    public interface IItemAdjustmentRepository
    {
        
         /// <summary>
         /// For InventoryAdjustments Screen : InventoryAdjustments.aspx / InventoryAdjustments.aspx.cs
         /// </summary>
         /// API's Naming Conventions : InventoryAdjustmentsList_Method Name(Parameter)
         /// 

        List<GetAllInvAdjustmentByDateViewModel> InventoryAdjustmentsList_GetAllInventoryAdjustmentByDate(GetAllInventoryAdjustmentByDateParam _GetAllInvAdjustmentByDateParam, string ConnectionString);
        int InventoryAdjustmentsList_DeleteAdjustment(DeleteAdjustmentParam _DeleteAdjustment, string ConnectionString);


        /// <summary>
        /// For AddInventoryAdjustments Screen : AddInventoryAdjustments.aspx / AddInventoryAdjustments.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddInventoryAdjustments_Method Name(Parameter)
        /// 

        List<GetInventoryAdjustmentByIDViewModel> AddInventoryAdjustments_GetInventoryAdjustmentByID(GetInventoryAdjustmentByIDParam _GetInventoryAdjustmentByIDParam, string ConnectionString);
        int AddInventoryAdjustments_CreateInventoryAdjustments(CreateInventoryAdjustmentsParam _CreateInventoryAdjustmentsParam, string ConnectionString);


        /// <summary>
        /// For POWeeklyReport Screen : POWeeklyReport.aspx / POWeeklyReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : POWeeklyReport_Method Name(Parameter)
        /// 

        public List<SMTPEmailViewModel> POWeeklyReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);
        List<GetCompanyDetailsViewModel> POWeeklyReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString);
        public List<GetControlViewModel> POWeeklyReport_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString);
        public List<GetPOWeeklyViewModel> POWeeklyReport_GetPOWeekly(GetPOWeeklyParam _GetPOWeekly, string ConnectionString);


        /// <summary>
        /// For CustomersReport Screen : CustomersReport.aspx / CustomersReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : CustomersReport_Method Name(Parameter)
        /// 

        List<CustomerReportViewModel> CustomersReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString);
        public List<CustomerReportViewModel> CustomersReport_GetCustomerType(GetCustomerTypeParam _getCustomerType, string ConnectionString);
        public ListGetCustReportFiltersValue CustomersReport_GetCustReportFiltersValue(GetCustReportFiltersValueParam _GetCustReportFiltersValue, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString);
        public List<CustomerReportViewModel> CustomersReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_getCustomerDetailsTest(getCustomerDetailsTestParam _getCustomerDetailsTest, string ConnectionString);
        List<UserViewModel> CustomersReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString);
        public bool CustomersReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString);
        public List<CustomerReportViewModel> CustomersReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString);
        public bool CustomersReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString);
        public void CustomersReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString);
        public void CustomersReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<HeaderFooterDetailViewModel> CustomersReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _CustomerReportParam, string ConnectionString);
        public List<CustomerFilterViewModel> CustomersReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString);
        public List<CustomerReportViewModel> CustomersReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString);
        public void CustomersReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString);

    }
}
