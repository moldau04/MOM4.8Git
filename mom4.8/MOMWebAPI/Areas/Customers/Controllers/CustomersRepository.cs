using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.InventoryModel;
using BusinessEntity.payroll;
using BusinessEntity.Payroll;
using BusinessLayer;
using BusinessLayer.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public class CustomersRepository : ICustomersRepository
    {
        /// <summary>
        /// For Customers List Screen : Customers.aspx / Customers.aspx.cs
        /// </summary>
        /// API's Naming Conventions : CustomersList_Method Name(Parameter)
        /// 

        public List<CustomViewModel> CustomersList_GetCustomFields(getCustomFieldsParam _getCustomFields, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_General().getCustomFields(_getCustomFields, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetControlViewModel> CustomersList_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getControl(_getConnectionConfig, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<UserViewModel> CustomersList_GetUserPermissionByUserID(GetUserByIdParam _GetUserById, string ConnectionString)
        //{
        //    try
        //    {
        //        return new BusinessLayer.BL_User().GetUserPermissionByUserID(_GetUserById, ConnectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<CompanyOfficeViewModel> CustomersList_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Company().getCompanyByCustomer(_GetCompanyByCustomerParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerReportViewModel> CustomersList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string connectionString)
        {
            try
            {
                return new BusinessLayer.BL_ReportsData().GetStockReports(_GetStockReportsParam, connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetCustomerTypeViewModel> CustomersList_GetCustomerType(getCustomerTypeParam _GetCustomerType, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCustomerType(_GetCustomerType, ConnectionString);

        }

        public void CustomersList_DeleteCustomer(DeleteCustomerParam _DeleteCustomer, string ConnectionString)
        {
            new BusinessLayer.BL_User().DeleteCustomer(_DeleteCustomer, ConnectionString);

        }

        public void CustomersList_AddQBCustomerType(AddQBCustomerTypeParam _AddQBCustomerType, string ConnectionString)
        {
            new BusinessLayer.BL_User().AddQBCustomerType(_AddQBCustomerType, ConnectionString);

        }

        public List<GetMSMCustomertypeViewModel> CustomersList_GetMSMCustomertype(GetMSMCustomertypeParam _GetMSMCustomertype, string ConnectionString)
        {
           return new BusinessLayer.BL_User().getMSMCustomertype(_GetMSMCustomertype, ConnectionString);
        }

        public void CustomersList_UpdateQBcustomertypeID(UpdateQBcustomertypeIDParam _UpdateQBcustomertypeID, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateQBcustomertypeID(_UpdateQBcustomertypeID, ConnectionString);

        }

        public void CustomersList_AddQBLocType(AddQBLocTypeParam _AddQBLocType, string ConnectionString)
        {
            new BusinessLayer.BL_User().AddQBLocType(_AddQBLocType, ConnectionString);

        }

        public List<GetMSMLoctypeViewModel> CustomersList_GetMSMLoctype(GetMSMLoctypeParam _GetMSMLoctype, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getMSMLoctype(_GetMSMLoctype, ConnectionString);
        }

        public void CustomersList_UpdateQBJobtypeID(UpdateQBJobtypeIDParam _UpdateQBJobtypeID, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateQBJobtypeID(_UpdateQBJobtypeID, ConnectionString);

        }

        public void CustomersList_AddQBSalesTax(AddQBSalesTaxParam _AddQBSalesTax, string ConnectionString)
        {
            new BusinessLayer.BL_User().AddQBSalesTax(_AddQBSalesTax, ConnectionString);

        }
        public List<GetMSMSalesTaxViewModel> CustomersList_GetMSMSalesTax(GetMSMSalesTaxParam _GetMSMSalesTax, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getMSMSalesTax(_GetMSMSalesTax, ConnectionString);
        }

        public void CustomersList_UpdateQBsalestaxID(UpdateQBsalestaxIDParam _UpdateQBsalestaxID, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateQBsalestaxID(_UpdateQBsalestaxID, ConnectionString);
        }

        public void CustomersList_AddCustomerQB(AddCustomerQBParam _AddCustomerQB, string ConnectionString)
        {
            new BusinessLayer.BL_User().AddCustomerQB(_AddCustomerQB, ConnectionString);
        }

        public void CustomersList_AddQBLocation(AddQBLocationParam _AddQBLocation, string ConnectionString)
        {
            new BusinessLayer.BL_User().AddQBLocation(_AddQBLocation, ConnectionString);
        }

        public List<GetMSMCustomersViewModel> CustomersList_GetMSMCustomers(GetMSMCustomersParam _GetMSMCustomers, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getMSMCustomers(_GetMSMCustomers, ConnectionString);
        }

        public void CustomersList_UpdateQBCustomerID(UpdateQBCustomerIDParam _UpdateQBCustomerID, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateQBCustomerID(_UpdateQBCustomerID, ConnectionString);
        }
        public List<GetQBCustomersViewModel> CustomersList_GetQBCustomers(GetQBCustomersParam _GetQBCustomers, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getQBCustomers(_GetQBCustomers, ConnectionString);
        }

        public List<GetMSMLocationViewModel> CustomersList_GetMSMLocation(GetMSMLocationParam _GetMSMLocation, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getMSMLocation(_GetMSMLocation, ConnectionString);
        }

        public void CustomersList_UpdateQBLocationID(UpdateQBLocationIDParam _UpdateQBLocationID, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateQBLocationID(_UpdateQBLocationID, ConnectionString);
        }
        public List<GetQBLocationViewModel> CustomersList_GetQBLocation(GetQBLocationParam _GetQBLocation, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getQBLocation(_GetQBLocation, ConnectionString);
        }

        public void CustomersList_UpdateQBLastSync(UpdateQBLastSyncParam _UpdateQBLastSync, string ConnectionString)
        {
            new BusinessLayer.BL_General().UpdateQBLastSync(_UpdateQBLastSync, ConnectionString);
        }

        public List<GeneralViewModel> CustomersList_GetSagelatsync(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            return new BusinessLayer.BL_General().getSagelatsync(_getConnectionConfig, ConnectionString);
        }

        public List<GetCustomerSearchViewModel> CustomersList_GetCustomerSearch(GetCustomerSearchParam _GetCustomerSearch, Int32 IsSalesAsigned, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCustomerSearch(_GetCustomerSearch, IsSalesAsigned, ConnectionString);
        }

        public List<GetCustomersViewModel> CustomersList_GetCustomers(GetCustomersParam _GetCustomers, Int32 IsSalesAsigned, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCustomers(_GetCustomers, IsSalesAsigned, ConnectionString);
        }

        /// <summary>
        /// For AddCustomer Screen : AddCustomer.aspx / AddCustomer.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddCustomer_Method Name(Parameter)
        /// 

        public ListGetCustomerByID AddCustomer_GetCustomerByID(GetCustomerByIDParam _GetCustomerByID, Int32 IsSalesAsigned, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCustomerByID(_GetCustomerByID, IsSalesAsigned, ConnectionString);
        }

        public List<GetAllLocationOnCustomerViewModel> AddCustomer_GetAllLocationOnCustomer(GetAllLocationOnCustomerParam _GetAllLocationOnCustomer, int _ownerId, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getAllLocationOnCustomer(_GetAllLocationOnCustomer, _ownerId, ConnectionString);
        }

        public void AddCustomer_UpdateCustomer(UpdateCustomerParam _UpdateCustomer, bool CopyToLocAndJob, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateCustomer(_UpdateCustomer, CopyToLocAndJob, ConnectionString);
        }

        public void AddCustomer_AddCustomer(AddCustomerParam _AddCustomer, string ConnectionString)
        {
            new BusinessLayer.BL_User().AddCustomer(_AddCustomer, ConnectionString);
        }

        public void AddCustomer_UpdateCustomerContactRecordLog(UpdateCustomerContactRecordLogParam _UpdateCustomerContactRecordLog, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateCustomerContactRecordLog(_UpdateCustomerContactRecordLog, ConnectionString);
        }

        public void AddCustomer_DeleteLocation(DeleteLocationParam _DeleteLocation, string ConnectionString)
        {
            new BusinessLayer.BL_User().DeleteLocation(_DeleteLocation, ConnectionString);
        }

        public List<GetElevViewModel> AddCustomer_GetElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            return new BusinessLayer.BL_User().getElev(_GetElev, ConnectionString, IsSalesAsigned);
        }

        public void AddCustomer_DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString)
        {
            new BusinessLayer.BL_User().DeleteEquipment(_DeleteEquipment, ConnectionString);
        }

        public List<GetCategoryViewModel> AddCustomer_GetCategory(GetCategoryParam _GetCategory, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getCategory(_GetCategory, ConnectionString);
        }

        public List<GetCallHistoryViewModel> AddCustomer_GetCallHistory(GetCallHistoryParam _GetCallHistory, string ConnectionString, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0)
        {
            return new BL_Tickets().getCallHistory(_GetCallHistory, ConnectionString, IsSalesAsigned, IsCallForTicketReport);
        }

        public ListGetARRevenueCustShowAll AddCustomer_GetARRevenueCust(GetARRevenueCustParam _GetARRevenueCust, string ConnectionString)
        {
            return new BusinessLayer.BL_Invoice().GetARRevenueCust(_GetARRevenueCust, ConnectionString);
        }

        public ListGetARRevenueCustShowAll AddCustomer_GetARRevenueCustShowAll(GetARRevenueCustShowAllParam _GetARRevenueCustShowAll, string ConnectionString)
        {
            return new BusinessLayer.BL_Invoice().GetARRevenueCustShowAll(_GetARRevenueCustShowAll, ConnectionString);
        }

        public ListGetProspectByID AddCustomer_GetProspectByID(GetProspectByIDParam _GetProspectByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getProspectByID(_GetProspectByID, ConnectionString);
        }

        public List<JobTypeViewModel> AddCustomer_GetDepartment(GetDepartmentParam _GetDepartment, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getDepartment(_GetDepartment, ConnectionString);
        }

        public List<GetLocationByCustomerIDViewModel> AddCustomer_GetLocationByCustomerID(GetLocationByCustomerIDParam _GetLocationByCustomerID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocationByCustomerID(_GetLocationByCustomerID, ConnectionString);
        }

        public void AddCustomer_AddFile(AddFileParam _AddFile, string ConnectionString) 
        { 
            new BusinessLayer.BL_MapData().AddFile(_AddFile, ConnectionString);
        }

        public void AddCustomer_UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfo, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateDocInfo(_UpdateDocInfo, ConnectionString);
        }

        public List<GetDocumentsViewModel> AddCustomer_GetDocuments(GetDocumentsParam _GetDocuments, string ConnectionString)
        {
            return new BusinessLayer.BL_MapData().GetDocuments(_GetDocuments, ConnectionString);
        }

        public void AddCustomer_DeleteFile(DeleteFileParam _DeleteFile, string ConnectionString)
        {
            new BusinessLayer.BL_MapData().DeleteFile(_DeleteFile, ConnectionString);
        }

        public List<GetCustomersLogsViewModel> AddCustomer_GetCustomersLogs(GetCustomersLogsParam _GetCustomersLogs, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetCustomersLogs(_GetCustomersLogs, ConnectionString);
        }

        public List<GetContactLogByCustomerIDViewModel> AddCustomer_GetContactLogByCustomerID(GetContactLogByCustomerIDParam _GetContactLogByCustomerID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getContactLogByCustomerID(_GetContactLogByCustomerID, ConnectionString);
        }

        public List<GetContactByRolIDViewModel> AddCustomer_GetContactByRolID(GetContactByRolIDParam _GetContactByRolID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getContactByRolID(_GetContactByRolID, ConnectionString);
        }

        public void AddCustomer_DeleteOpportunity(DeleteOpportunityParam _DeleteOpportunity, string ConnectionString)
        {
            new BusinessLayer.BL_Customer().DeleteOpportunity(_DeleteOpportunity, ConnectionString);
        }
        public List<GetOpportunityOfCustomerViewModel> AddCustomer_GetOpportunityOfCustomer(GetOpportunityOfCustomerParam _GetOpportunityOfCustomer, string ConnectionString)
        {
            return new BusinessLayer.BL_Customer().getOpportunityOfCustomer(_GetOpportunityOfCustomer, ConnectionString);
        }

        public List<GetJobProjectViewModel> AddCustomer_GetJobProject(GetJobProjectParam _GetJobProject, string ConnectionString, Int32 IsSalesAsigned = 0, int IncludeClose = 1)
        {
            return new BusinessLayer.BL_Customer().getJobProject(_GetJobProject, ConnectionString, IsSalesAsigned, IncludeClose);
        }


        /// <summary>
        /// For CompletedTicketReport Screen : CompletedTicketReport.aspx / CompletedTicketReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : AddCustomer_ServiceHistoryReport_Method Name(Parameter)
        /// 
        public List<GetCompanyDetailsViewModel> AddCustomer_ServiceHistoryReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetCompanyDetails(_GetCompanyDetails, ConnectionString);
        }
        public List<GetCompletedTicketViewModel> AddCustomer_ServiceHistoryReport_GetCompletedTicket(GetCompletedTicketParam _GetCompletedTicket, List<RetainFilter> filters, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetCompletedTicket(_GetCompletedTicket, filters, ConnectionString);
        }
        public List<SMTPEmailViewModel> AddCustomer_ServiceHistoryReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_GetSMTPByUserID, ConnectionString);
        }


        /// <summary>
        /// For CustomerLabel5160 Screen : CustomerLabel5160.aspx / CustomerLabel5160.aspx.cs
        /// </summary>
        /// API's Naming Conventions : CustomerReport_Method Name(Parameter)
        /// 

        public List<GetCustomerLabelViewModel> CustomerReport_GetCustomerLabel(GetCustomerLabelParam _GetCustomerLabel, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            return new BusinessLayer.BL_Report().GetCustomerLabel(_GetCustomerLabel, ConnectionString, IsSalesAsigned);
        }

    }
}
