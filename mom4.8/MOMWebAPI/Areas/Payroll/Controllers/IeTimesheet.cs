using BusinessEntity.APModels;
using BusinessEntity.payroll;
using System;
using BusinessEntity;
using System.Collections.Generic;
using System.Data;
using BusinessEntity.Payroll;
using BusinessEntity.InventoryModel;

namespace MOMWebAPI.Areas.Payroll
{
   public interface IeTimesheet
    {
        //etimesheet api's
        List<UserViewModel> GetUserById(GetUserByIdParam _GetUserByIdParam, string ConnectionString);
        public List<UserViewModel> getSavedTimesheet(getTimesheetParam _getSavedTimesheetParam, string ConnectionString); 
        public List<JobTypeViewModel> getDepartment(GetDepartmentParam _getDepartmentParam, string ConnectionString);
        public List<UserViewModel> getEMPSuper(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<UserViewModel> getEMPwithDeviceID(getTimesheetParam _getTimesheetParam, string ConnectionString); 
        public List<eTimesheetViewModel> getTimesheetEmp(getTimesheetParam _getTimesheetParam, int eTimeSheet, string ConnectionString);

        public List<eTimesheetViewModel> getSavedTimesheetEmp(getTimesheetParam _getTimesheetParam, string ConnectionString);

        public List<eTimesheetViewModel> GetTimesheetTicketsByEmp(getTimesheetParam _getTimesheetParam, int eTimesheet, string ConnectionString);
        public void AddTimesheet(AddTimesheetParam _AddTimesheetParam, string ConnectionString);

        public List<SageExportTickets> getGetSageExportTickets(getTimesheetParam _getTimesheetParam, string ConnectionString);

        public List<UserViewModel> GetCoCode(getConnectionConfigParam _user,string ConnectionString);

        public void UpdateCoCode(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<PayrollViewModel> getPayRoll(getTimesheetParam _getTimesheetParam, string ConnectionString);

        public List<GeneralViewModel> getSagelatsync(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);

        //User api's
        public int getLoginSuper(AddUserParam _AddUserParam, string ConnectionString);

        public int getISSuper(AddUserParam _AddUserParam, string ConnectionString);
        public List<SuperUserViewModel> getUserForSupervisor(AddUserParam _AddUserParam, string ConnectionString);

        //customer report api's
        public List<CustomerReportViewModel> GetReportDetailById(CustomerReportParam _CustomerReportParam, string ConnectionString);
        public List<CustomerReportViewModel> GetCustomerType(GetCustomerTypeParam _CustomerReportParam, string ConnectionString);
        public List<CustomerFilterViewModel> GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString);

        public ListGetCustReportFiltersValue GetCustReportFiltersValue(GetCustReportFiltersValueParam _GetCustReportFiltersValue, string ConnectionString); 
        public List<CustomerFilterViewModel> GetCustomerName(GetCustomerNameParam _CustomerReportParam, string ConnectionString); 

        public List<CustomerFilterViewModel> GetCustomerAddress(GetCustomerAddressParam _CustomerReportParam, string ConnectionString); 
        public List<CustomerFilterViewModel> GetCustomerCity(GetCustomerCityParam _CustomerReportParam, string ConnectionString); 

        public List<CustomerReportViewModel> GetDynamicReports(GetDynamicReportsParam _CustomerReportParam, string ConnectionString);
        //till here
        public List<CustomerFilterViewModel> GetReportColByRepId(GetReportColByRepIdParam _customerreport, string ConnectionString);
        public List<CustomerFilterViewModel> GetReportFiltersByRepId(GetReportFiltersByRepIdParam _customerreport, string ConnectionString); 
        public List<CustomerFilterViewModel> getCustomerDetailsTest(getCustomerDetailsTestParam _getConnectionConfigParam, string ConnectionString); 
        public bool CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString); 
        public List<CustomerReportViewModel> InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString);
        public bool IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString);
        public void UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString);
        public void DeleteCustomerReport(DeleteCustomerReportParam _customerreport, string ConnectionString);
        public List<CustomerFilterViewModel> GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString);
        public List<HeaderFooterDetailViewModel> GetHeaderFooterDetail(GetHeaderFooterDetailParam _CustomerReportParam, string ConnectionString); 
        
        public List<CustomerReportViewModel> GetColumnWidthByReportId(GetColumnWidthByReportIdParam _customerreport ,string ConnectionString);
        public void UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _customerreport, string ConnectionString);

        public List<TicketViewModel> GetTicketList(PJ objPJ, string ConnectionString);
        public List<GetControlViewModel> getControl(getConnectionConfigParam _user, string ConnectionString); 
        public List<SMTPEmailViewModel> getSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString); 



    }
}
