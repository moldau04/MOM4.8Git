using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.AP.Controllers
{
    public interface IVendorRepository
    { 
        /// <summary>
        /// For Vendor Screen : Vendor.aspx / Vendor.aspx.cs
        /// </summary>
        List<GetVendorTypeViewModel> VendorList_getVendorType(getVendorTypeParam _getVendorTypeParam, string ConnectionString);
        List<UserViewModel> VendorList_GetUserById(GetUserByIdParam _GetUserByIdParam, string ConnectionString);
        List<CustomerReportViewModel> VendorList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string ConnectionString);
        List<GetAllVenderAjaxSearchModel> VendorList_GetAllVenderAjaxSearch(GetAllVenderAjaxSearchParam _GetAllVenderAjaxSearchParam, string ConnectionString);
        bool VendorList_IsExistVendorDetails(IsExistVendorDetailsParam _IsExistVendorDetailsParam, string ConnectionString);
        void VendorList_DeleteRolByID(DeleteRolByIDParam _DeleteRolByIDParam, string ConnectionString);
        void VendorList_DeleteVendor(DeleteVendorParam _DeleteVendorParam, string ConnectionString);



        /// <summary>
        /// For Add Vendor Screen : AddVendor.aspx / AddVendor.aspx.cs
        /// </summary>

        List<CompanyOfficeViewModel> AddVendor_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString);
        List<StateViewModel> AddVendor_GetStates(GetStatesParam _GetStatesParam, string ConnectionString);
        List<TermsViewModel> AddVendor_GetTerms(GetTermsParam _GetTermsParam, string ConnectionString);
        List<STaxViewModel> AddVendor_GetUseTax(getUseTaxParam _getUseTaxParam, string ConnectionString);
        List<STaxViewModel> AddVendor_GetSTax(getSTaxParam _getSTaxParam, string ConnectionString);
        List<CustomViewModel> AddVendor_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString);
        List<CustomViewModel> AddVendor_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString);
        List<ChartViewModel> AddVendor_GetChart(GetChartParam _GetChartParam, string ConnectionString);
        List<VendorViewModel> AddVendor_GetVendorEdit(GetVendorEditParam _GetVendorEditParam, string ConnectionString);
        List<GetVendorContactByRolIDViewModel> AddVendor_GetVendorContactByRolID(getVendorContactByRolIDParam _getVendorContactByRolIDParam, string ConnectionString);
        ListGetAPExpenses AddVendor_GetAPExpenses(GetAPExpensesParam _GetAPExpensesParam, string ConnectionString);
        List<LogViewModel> AddVendor_GetVendorLogs(GetVendorLogsParam _GetVendorLogsParam, string ConnectionString);
        void AddVendor_UpdateRol(UpdateRolParam _UpdateRolParam, string ConnectionString);
        void AddVendor_UpdateRoles(UpdateRolesParam _UpdateRolesParam, string ConnectionString);
        List<VendorViewModel> AddVendor_isExistForUpdateVendor(IsExistForUpdateVendorParam _IsExistForUpdateVendorParam, string ConnectionString);
        void AddVendor_UpdateVendor(UpdateVendorParam _UpdateVendorParam, string ConnectionString);
        void AddVendor_UpdateVendorTax(UpdateVendorTaxParam _UpdateVendorTaxParam, string ConnectionString);
        int AddVendor_AddRol(AddRolParam _AddRolParam, string ConnectionString);
        List<VendorViewModel> AddVendor_isExistsForInsertVendor(IsExistsForInsertVendorParam _IsExistsForInsertVendorParam, string ConnectionString);
        int AddVendor_AddVendor(AddVendorParam _AddVendorParam, string ConnectionString);
        void AddVendor_UpdateVendorContact(UpdateVendorContactParam _UpdateVendorContactParam, string ConnectionString);

        /// <summary>
        /// For VendorLbl5160 Page : VendorLbl5160.aspx / VendorLbl5160.aspx.cs
        /// </summary>
        /// 
        List<GetCompanyDetailsViewModel> VendorReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString);
        List<RolViewModel> VendorReport_GetVendorLabel(GetVendorLabelParam _GetVendorLabelParam, string ConnectionString);
        public List<GetControlViewModel> VendorReport_GetControl(getConnectionConfigParam _user, string ConnectionString);
        public List<SMTPEmailViewModel> VendorReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString);

        /// <summary>
        /// For Vendor1099 Page : Vendor1099.aspx / Vendor1099.aspx.cs
        /// </summary>
        /// 

        List<VendorViewModel> VendorReport_GetFederalReport(GetFederalReportParam _GetFederalReportParam, string ConnectionString);

        /// <summary>
        /// For Vendor Transaction History Tab : VendorTransactionHistory.aspx / VendorTransactionHistory.aspx.cs
        /// </summary>
        /// 
        public List<GetHistoryTransactionViewModel> VendorTransaction_GetHistoryTransaction(GetHistoryTransactionParam _GetHistoryTransaction, string ConnectionString);

    }
}
