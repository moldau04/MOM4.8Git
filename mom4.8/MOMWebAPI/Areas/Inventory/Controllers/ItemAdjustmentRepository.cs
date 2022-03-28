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
    public class ItemAdjustmentRepository : IItemAdjustmentRepository
    {
        /// <summary>
        /// For InventoryAdjustments Screen : InventoryAdjustments.aspx / InventoryAdjustments.aspx.cs
        /// </summary>
        /// API's Naming Conventions : InventoryAdjustmentsList_Method Name(Parameter)
        /// 

        public List<GetAllInvAdjustmentByDateViewModel> InventoryAdjustmentsList_GetAllInventoryAdjustmentByDate(GetAllInventoryAdjustmentByDateParam _GetAllInvAdjustmentByDateParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetAllInventoryAdjustmentByDate(_GetAllInvAdjustmentByDateParam, ConnectionString);
        }

        public int InventoryAdjustmentsList_DeleteAdjustment(DeleteAdjustmentParam _DeleteAdjustment, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().DeleteAdjustment(_DeleteAdjustment, ConnectionString);
        }


        /// <summary>
        /// For AddInventoryAdjustments Screen : AddInventoryAdjustments.aspx / AddInventoryAdjustments.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddInventoryAdjustments_Method Name(Parameter)
        /// 

        public List<GetInventoryAdjustmentByIDViewModel> AddInventoryAdjustments_GetInventoryAdjustmentByID(GetInventoryAdjustmentByIDParam _GetInventoryAdjustmentByIDParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().GetInventoryAdjustmentByID(_GetInventoryAdjustmentByIDParam, ConnectionString);
        }

        public int AddInventoryAdjustments_CreateInventoryAdjustments(CreateInventoryAdjustmentsParam _CreateInventoryAdjustmentsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Inventory().CreateInventoryAdjustments(_CreateInventoryAdjustmentsParam, ConnectionString);
        }


        /// <summary>
        /// For POWeeklyReport Screen : POWeeklyReport.aspx / POWeeklyReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : POWeeklyReport_Method Name(Parameter)
        ///

        public List<GetCompanyDetailsViewModel> POWeeklyReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
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

        public List<SMTPEmailViewModel> POWeeklyReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_user, ConnectionString);
        }

        public List<GetControlViewModel> POWeeklyReport_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }


        public List<GetPOWeeklyViewModel> POWeeklyReport_GetPOWeekly(GetPOWeeklyParam _GetPOWeekly, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetPOWeekly(_GetPOWeekly, ConnectionString);
        }


        /// <summary>
        /// For CustomersReport Screen : CustomersReport.aspx / CustomersReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : CustomersReport_Method Name(Parameter)
        /// 

        public List<CustomerReportViewModel> CustomersReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportDetailById(_GetReportDetailByIdParam, ConnectionString);
        }
        public List<CustomerReportViewModel> CustomersReport_GetCustomerType(GetCustomerTypeParam _getCustomerType, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerType(_getCustomerType, ConnectionString);
        }

        public ListGetCustReportFiltersValue CustomersReport_GetCustReportFiltersValue(GetCustReportFiltersValueParam _GetCustReportFiltersValue, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustReportFiltersValue(_GetCustReportFiltersValue, ConnectionString);
        }

        public List<CustomerFilterViewModel> CustomersReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerName(_GetCustomerNameParam, ConnectionString);
        }

        public List<CustomerFilterViewModel> CustomersReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerAddress(_GetCustomerAddressParam, ConnectionString);
        }

        public List<CustomerFilterViewModel> CustomersReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerCity(_GetCustomerCityParam, ConnectionString);
        }

        public List<CustomerReportViewModel> CustomersReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetDynamicReports(_GetDynamicReportsParam, ConnectionString);
        }

        public List<CustomerFilterViewModel> CustomersReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportColByRepId(_GetReportColByRepId, ConnectionString);
        }
        public List<CustomerFilterViewModel> CustomersReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportFiltersByRepId(_GetReportFiltersByRepId, ConnectionString);
        }

        public List<CustomerFilterViewModel> CustomersReport_getCustomerDetailsTest(getCustomerDetailsTestParam _getCustomerDetailsTest, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().getCustomerDetailsTest(_getCustomerDetailsTest, ConnectionString);
        }

        public List<UserViewModel> CustomersReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetail, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetAccountSummaryListingDetail(_GetAccountSummaryListingDetail, ConnectionString);
        }

        public bool CustomersReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReport, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().CheckExistingReport(_CheckExistingReport, ConnectionString);
        }

        public List<CustomerReportViewModel> CustomersReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReport, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().InsertCustomerReport(_InsertCustomerReport, ConnectionString);
        }

        public bool CustomersReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExist, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().IsStockReportExist(_IsStockReportExist, ConnectionString);
        }

        public void CustomersReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReport, string ConnectionString)
        {
             new BusinessLayer.BL_ReportsData().UpdateCustomerReport(_UpdateCustomerReport, ConnectionString);
        }

        public void CustomersReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().DeleteCustomerReport(_DeleteCustomerReport, ConnectionString);
        }
        public List<CustomerFilterViewModel> CustomersReport_GetControlForReports(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetControlForReports(_getConnectionConfig, ConnectionString);
        }
        public List<HeaderFooterDetailViewModel> CustomersReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _GetHeaderFooterDetail, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetHeaderFooterDetail(_GetHeaderFooterDetail, ConnectionString);
        }
        public List<CustomerFilterViewModel> CustomersReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetOwners(_GetOwnersParam, ConnectionString);
        }
        public List<CustomerReportViewModel> CustomersReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetColumnWidthByReportId(_GetColumnWidthByReportId, ConnectionString);
        }
        public void CustomersReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReportResizedWidth(_UpdateCustomerReportResizedWidth, ConnectionString);
        }
    }
}
