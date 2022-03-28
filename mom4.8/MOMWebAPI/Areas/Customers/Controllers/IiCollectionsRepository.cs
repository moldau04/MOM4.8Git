using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public interface IiCollectionsRepository
    {
        /// <summary>
        /// For iCollections List Screen : iCollections.aspx / iCollections.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsList_Method Name(Parameter)
        /// 
        public List<CustomViewModel> iCollectionsList_GetCustomFields(getCustomFieldsParam _getCustomFields, string ConnectionString);
        public List<JobTypeViewModel> iCollectionsList_GetDepartment(GetDepartmentParam _GetDepartment, string ConnectionString);
        public List<CompanyOfficeViewModel> iCollectionsList_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomer, string ConnectionString);
        public List<UserViewModel> iCollectionsList_getUserDefaultCompany(getUserDefaultCompanyParam _getUserDefaultCompany, string ConnectionString);
        public List<GetCollectionsViewModel> iCollectionsList_GetCollections(GetCollectionsParam _GetCollections, string ConnectionString);
        public List<GetInvoicesByRefViewModel> iCollectionsList_GetInvoicesByRef(GetInvoicesByRefParam _GetInvoicesByRef, string ConnectionString);
        public List<GetControlViewModel> iCollectionsList_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString);
        public List<GetInvoiceItemByRefViewModel> iCollectionsList_GetInvoiceItemByRef(GetInvoiceItemByRefParam _GetInvoiceItemByRef, string ConnectionString);
        public List<UserViewModel> iCollectionsList_GetControlBranch(GetControlBranchParam _GetControlBranch, string ConnectionString);
        public List<GetTicketIDViewModel> iCollectionsList_GetTicketID(GetTicketIDParam _GetTicketID, string ConnectionString);
        //public List<GetTicketByIDViewModel> iCollectionsList_GetTicketByID(GetTicketByIDParam _GetTicketByID, string ConnectionString);
        public ListGetLocationByID iCollectionsList_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString);
        public List<GetElevByTicketViewModel> iCollectionsList_GetElevByTicket(GetElevByTicketParam _GetElevByTicket, string ConnectionString);
        public List<GetequipREPDetailsViewModel> iCollectionsList_GetequipREPDetails(GetequipREPDetailsParam _GetequipREPDetails, string ConnectionString);
        public List<GetElevByTicketViewModel> iCollectionsList_GetElevByTicketID(GetElevByTicketIDParam _GetElevByTicketID, string ConnectionString);
        public List<GetCustomerStatementByLocViewModel> iCollectionsList_GetCustomerStatementByLoc(GetCustomerStatementByLocParam _GetCustomerStatementByLoc, string ConnectionString);
        public void iCollectionsList_AddEmailLog(AddEmailLogParam _AddEmailLog, string ConnectionString);
        public List<GetCustStatementInvSouthernViewModel> iCollectionsList_GetCustomerStatementInvoicesSouthern(GetCustomerStatementInvoicesSouthernParam _GetCustStatementInvSouthern, string ConnectionString, bool includeCredit);
        public List<GetCustStatementInvSouthernViewModel> iCollectionsList_GetCustomerStatementInvoices(GetCustomerStatementInvoicesParam _GetCustomerStatementInvoices, string ConnectionString, bool includeCredit);
        public List<GetEmailDetailByLocViewModel> iCollectionsList_GetEmailDetailByLoc(GetEmailDetailByLocParam _GetEmailDetailByLoc, string ConnectionString);
        public List<GetCompanyDetailsViewModel> iCollectionsList_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetails, string ConnectionString);
        public List<GetCustomerStatementByLocViewModel> iCollectionsList_GetCustomerStatementByLocs(GetCustomerStatementByLocsParam _GetCustomerStatementByLocs, string ConnectionString);
        public List<GetCustStatementInvSouthernViewModel> iCollectionsList_GetCustomerStatementInvoicesByLocation(GetCustStatementInvByLocationParam _GetCustStatementInvByLocation, string ConnectionString, bool includeCredit);
        public List<GetGLAccountViewModel> iCollectionsList_GetGLAccount(GetGLAccountParam _GetGLAccount, string ConnectionString, string acct);
        public void iCollectionsList_writeOffInvoice(writeOffInvoiceParam _writeOffInvoice, string ConnectionString);
        public ListGetInvoicesByID iCollectionsList_GetInvoicesByID(GetInvoicesByIDParam _GetInvoicesByID, string ConnectionString);
        public List<GetAutoCompleteBillCodesViewModel> iCollectionsList_GetAutoCompleteBillCodes(GetAutoCompleteBillCodesParam _GetAutoCompleteBillCodes, string ConnectionString, string prefixText = "");
        public List<GetEmailLogsViewModel> iCollectionsList_GetEmailLogs(GetEmailLogsParam _GetEmailLogs, string ConnectionString);
        public List<GetTicketIDViewModel> iCollectionsList_GetAllTicketID(GetAllTicketIDParam _GetAllTicketID, string ConnectionString);
        public List<GetElevByTicketIDsViewModel> iCollectionsList_GetElevByTicketIDs(GetElevByTicketIDsParam _GetElevByTicketIDs, string ConnectionString, string ticketIDs);
        public List<GetActiveBillingCodeViewModel> iCollectionsList_GetActiveBillingCode(GetActiveBillingCodeParam _GetActiveBillingCode, string ConnectionString);

        /// <summary>
        /// For ARAgingReportCollection List Screen : ARAgingReportCollection.aspx / ARAgingReportCollection.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<SMTPEmailViewModel> iCollectionsReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString);

        /// <summary>
        /// For CustomerStatementCollectionReport List Screen : CustomerStatementCollectionReport.aspx / CustomerStatementCollectionReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetCustomerStatementCollectionViewModel> iCollectionsReport_GetCustomerStatementCollection(GetCustomerStatementCollectionParam _GetCustomerStatementCollection, string ConnectionString, bool includeCredit, bool includeCustomerCredit);

        /// <summary>
        /// For ARAgingReportByTerritoryCollection List Screen : ARAgingReportByTerritoryCollection.aspx / ARAgingReportByTerritoryCollection.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public ListGetARAgingByTerritory iCollectionsReport_GetARAgingByTerritory(GetARAgingByTerritoryParam _GetARAgingByTerritory, string ConnectionString, string territories);
        public List<GetTerritoryViewModel> iCollectionsReport_GetAllTerritory(GetAllTerritoryParam _GetAllTerritory, string ConnectionString);


        /// <summary>
        /// For ARAgingReportCust List Screen : ARAgingReportCust.aspx / ARAgingReportCust.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public ListGetARAging iCollectionsReport_GetARAging(GetARAgingParam _GetARAging, string ConnectionString);
        public ListGetARAgingByAsOfDate iCollectionsReport_GetARAgingByAsOfDate(GetARAgingByAsOfDateParam _GetARAgingByAsOfDate, string ConnectionString);

        /// <summary>
        /// For AgedReceivableReport List Screen : AgedReceivableReport.aspx / AgedReceivableReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetInvoiceByDateViewModel> iCollectionsReport_GetInvoiceByDate(GetInvoiceByDateParam _GetInvoiceByDate, string ConnectionString);

        /// <summary>
        /// For ARAgingReportByLocType List Screen : ARAgingReportByLocType.aspx / ARAgingReportByLocType.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetLocationTypeViewModel> iCollectionsReport_getlocationType(getlocationTypeParam _getlocationType, string ConnectionString);
        public List<GetARAgingByLocTypeViewModel> iCollectionsReport_GetARAgingByLocType(GetARAgingByLocTypeParam _GetARAgingByLocType, string ConnectionString);
        public List<GetARAgingByLocTypeDetailViewModel> iCollectionsReport_GetARAgingByLocTypeDetail(GetARAgingByLocTypeDetailParam _GetARAgingByLocTypeDetail, string ConnectionString);


        /// <summary>
        /// For ARAgingReport360ByLocation List Screen : ARAgingReport360ByLocation.aspx / ARAgingReport360ByLocation.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public ListGetARAging360ByAsOfDate iCollectionsReport_GetARAging360ByAsOfDate(GetARAging360ByAsOfDateParam _GetARAging360ByAsOfDate, string ConnectionString);


        /// <summary>
        /// For ARAgingReportDep Screen : ARAgingReportDep.aspx / ARAgingReportDep.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetARAgingDepViewModel> iCollectionsReport_GetARAgingDep(GetARAgingDepParam _GetARAgingDep, string ConnectionString);
        public ListGetARAgingByAsOfDateDep iCollectionsReport_GetARAgingByAsOfDateDep(GetARAgingByAsOfDateDepParam _GetARAgingByAsOfDateDep, string ConnectionString);
    }
}
