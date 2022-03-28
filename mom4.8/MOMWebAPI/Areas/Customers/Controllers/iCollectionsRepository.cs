using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessLayer.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Customers.Controllers
{
    public class iCollectionsRepository : IiCollectionsRepository
    {
        /// <summary>
        /// For iCollections List Screen : iCollections.aspx / iCollections.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsList_Method Name(Parameter)
        /// 

        public List<CustomViewModel> iCollectionsList_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_General().getCustomFields(_getCustomFieldsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobTypeViewModel> iCollectionsList_GetDepartment(GetDepartmentParam _GetDepartment, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getDepartment(_GetDepartment, ConnectionString);
        }

        public List<CompanyOfficeViewModel> iCollectionsList_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString)
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

        public List<UserViewModel> iCollectionsList_getUserDefaultCompany(getUserDefaultCompanyParam _getUserDefaultCompanyParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Company().getUserDefaultCompany(_getUserDefaultCompanyParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetCollectionsViewModel> iCollectionsList_GetCollections(GetCollectionsParam _GetCollections, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Collection().GetCollections(_GetCollections, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetInvoicesByRefViewModel> iCollectionsList_GetInvoicesByRef(GetInvoicesByRefParam _GetInvoicesByRef, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().GetInvoicesByRef(_GetInvoicesByRef, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetControlViewModel> iCollectionsList_GetControl(getConnectionConfigParam _getConnectionConfig, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfig, ConnectionString);
        }
        public List<GetInvoiceItemByRefViewModel> iCollectionsList_GetInvoiceItemByRef(GetInvoiceItemByRefParam _GetInvoiceItemByRef, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetInvoiceItemByRef(_GetInvoiceItemByRef, ConnectionString);
        }
        public List<UserViewModel> iCollectionsList_GetControlBranch(GetControlBranchParam _GetControlBranchParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getControlBranch(_GetControlBranchParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetTicketIDViewModel> iCollectionsList_GetTicketID(GetTicketIDParam _GetTicketID, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().GetTicketID(_GetTicketID, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ListGetLocationByID iCollectionsList_GetLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getLocationByID(_GetLocationByID, ConnectionString);
        }
        public List<GetElevByTicketViewModel> iCollectionsList_GetElevByTicket(GetElevByTicketParam _GetElevByTicket, string ConnectionString)
        {
            return new BusinessLayer.BL_MapData().getElevByTicket(_GetElevByTicket, ConnectionString);
        }
        public List<GetequipREPDetailsViewModel> iCollectionsList_GetequipREPDetails(GetequipREPDetailsParam _GetequipREPDetails, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getequipREPDetails(_GetequipREPDetails, ConnectionString);
        }
        public List<GetElevByTicketViewModel> iCollectionsList_GetElevByTicketID(GetElevByTicketIDParam _GetElevByTicketID, string ConnectionString)
        {
            return new BusinessLayer.BL_MapData().getElevByTicketID(_GetElevByTicketID, ConnectionString);
        }
        public List<GetCustomerStatementByLocViewModel> iCollectionsList_GetCustomerStatementByLoc(GetCustomerStatementByLocParam _GetCustomerStatementByLoc, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetCustomerStatementByLoc(_GetCustomerStatementByLoc, ConnectionString);
        }
        public void iCollectionsList_AddEmailLog(AddEmailLogParam _AddEmailLog, string ConnectionString)
        {
             new BusinessLayer.BL_EmailLog().AddEmailLog(_AddEmailLog, ConnectionString);
        }
        public List<GetCustStatementInvSouthernViewModel> iCollectionsList_GetCustomerStatementInvoicesSouthern(GetCustomerStatementInvoicesSouthernParam _GetCustStatementInvSouthern, string ConnectionString, bool includeCredit)
        {
            return new BusinessLayer.BL_Contracts().GetCustomerStatementInvoicesSouthern(_GetCustStatementInvSouthern, ConnectionString, includeCredit);
        }
        public List<GetCustStatementInvSouthernViewModel> iCollectionsList_GetCustomerStatementInvoices(GetCustomerStatementInvoicesParam _GetCustomerStatementInvoices, string ConnectionString, bool includeCredit)
        {
            return new BusinessLayer.BL_Contracts().GetCustomerStatementInvoices(_GetCustomerStatementInvoices, ConnectionString, includeCredit);
        }
        public List<GetEmailDetailByLocViewModel> iCollectionsList_GetEmailDetailByLoc(GetEmailDetailByLocParam _GetEmailDetailByLoc, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetEmailDetailByLoc(_GetEmailDetailByLoc, ConnectionString);
        }
        public List<GetCompanyDetailsViewModel> iCollectionsList_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
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
        public List<GetCustomerStatementByLocViewModel> iCollectionsList_GetCustomerStatementByLocs(GetCustomerStatementByLocsParam _GetCustomerStatementByLocs, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().GetCustomerStatementByLocs(_GetCustomerStatementByLocs, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetCustStatementInvSouthernViewModel> iCollectionsList_GetCustomerStatementInvoicesByLocation(GetCustStatementInvByLocationParam _GetCustStatementInvByLocation, string ConnectionString, bool includeCredit)
        {
            try
            {
                return new BusinessLayer.BL_Contracts().GetCustomerStatementInvoicesByLocation(_GetCustStatementInvByLocation, ConnectionString, includeCredit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetGLAccountViewModel> iCollectionsList_GetGLAccount(GetGLAccountParam _GetGLAccount, string ConnectionString, string acct)
        {
            try
            {
                return new BusinessLayer.BL_Deposit().GetGLAccount(_GetGLAccount, ConnectionString, acct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void iCollectionsList_writeOffInvoice(writeOffInvoiceParam _writeOffInvoice, string ConnectionString)
        {
            new BusinessLayer.BL_Deposit().writeOffInvoice(_writeOffInvoice, ConnectionString);
        }

        public ListGetInvoicesByID iCollectionsList_GetInvoicesByID(GetInvoicesByIDParam _GetInvoicesByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetInvoicesByID(_GetInvoicesByID, ConnectionString);
        }
        public List<GetAutoCompleteBillCodesViewModel> iCollectionsList_GetAutoCompleteBillCodes(GetAutoCompleteBillCodesParam _GetAutoCompleteBillCodes, string ConnectionString, string prefixText = "")
        {
            return new BL_BillCodes().GetAutoCompleteBillCodes(_GetAutoCompleteBillCodes, ConnectionString, prefixText );
        }
        public List<GetEmailLogsViewModel> iCollectionsList_GetEmailLogs(GetEmailLogsParam _GetEmailLogs, string ConnectionString)
        {
            return new BusinessLayer.BL_EmailLog().GetEmailLogs(_GetEmailLogs, ConnectionString);
        }
        public List<GetTicketIDViewModel> iCollectionsList_GetAllTicketID(GetAllTicketIDParam _GetAllTicketID, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetAllTicketID(_GetAllTicketID, ConnectionString);
        }
        public List<GetElevByTicketIDsViewModel> iCollectionsList_GetElevByTicketIDs(GetElevByTicketIDsParam _GetElevByTicketIDs, string ConnectionString, string ticketIDs)
        {
            return new BusinessLayer.BL_MapData().GetElevByTicketIDs(_GetElevByTicketIDs, ConnectionString, ticketIDs);
        }
        public List<GetActiveBillingCodeViewModel> iCollectionsList_GetActiveBillingCode(GetActiveBillingCodeParam _GetActiveBillingCode, string ConnectionString)
        {
            return new BL_BillCodes().GetActiveBillingCode(_GetActiveBillingCode, ConnectionString);
        }

        /// <summary>
        /// For ARAgingReportCollection List Screen : ARAgingReportCollection.aspx / ARAgingReportCollection.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<SMTPEmailViewModel> iCollectionsReport_GetSMTPByUserID(GetSMTPByUserIDParam _GetSMTPByUserID, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_GetSMTPByUserID, ConnectionString);
        }

        /// <summary>
        /// For CustomerStatementCollectionReport List Screen : CustomerStatementCollectionReport.aspx / CustomerStatementCollectionReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetCustomerStatementCollectionViewModel> iCollectionsReport_GetCustomerStatementCollection(GetCustomerStatementCollectionParam _GetCustomerStatementCollection, string ConnectionString, bool includeCredit, bool includeCustomerCredit)
        {
            return new BusinessLayer.BL_Contracts().GetCustomerStatementCollection(_GetCustomerStatementCollection, ConnectionString, includeCredit, includeCustomerCredit);
        }


        /// <summary>
        /// For ARAgingReportByTerritoryCollection List Screen : ARAgingReportByTerritoryCollection.aspx / ARAgingReportByTerritoryCollection.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public ListGetARAgingByTerritory iCollectionsReport_GetARAgingByTerritory(GetARAgingByTerritoryParam _GetARAgingByTerritory, string ConnectionString, string territories)
        {
            return new BusinessLayer.BL_Contracts().GetARAgingByTerritory(_GetARAgingByTerritory, ConnectionString, territories);
        }
        public List<GetTerritoryViewModel> iCollectionsReport_GetAllTerritory(GetAllTerritoryParam _GetAllTerritory, string ConnectionString)
        {
            return new BusinessLayer.BL_User().GetAllTerritory(_GetAllTerritory, ConnectionString);
        }


        /// <summary>
        /// For ARAgingReportCust List Screen : ARAgingReportCust.aspx / ARAgingReportCust.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public ListGetARAging iCollectionsReport_GetARAging(GetARAgingParam _GetARAging, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetARAging(_GetARAging, ConnectionString);
        }

        public ListGetARAgingByAsOfDate iCollectionsReport_GetARAgingByAsOfDate(GetARAgingByAsOfDateParam _GetARAgingByAsOfDate, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetARAgingByAsOfDate(_GetARAgingByAsOfDate, ConnectionString);
        }

        /// <summary>
        /// For AgedReceivableReport List Screen : AgedReceivableReport.aspx / AgedReceivableReport.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetInvoiceByDateViewModel> iCollectionsReport_GetInvoiceByDate(GetInvoiceByDateParam _GetInvoiceByDate, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetInvoiceByDate(_GetInvoiceByDate, ConnectionString);
        }

        /// <summary>
        /// For ARAgingReportByLocType List Screen : ARAgingReportByLocType.aspx / ARAgingReportByLocType.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetLocationTypeViewModel> iCollectionsReport_getlocationType(getlocationTypeParam _getlocationType, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getlocationType(_getlocationType, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetARAgingByLocTypeViewModel> iCollectionsReport_GetARAgingByLocType(GetARAgingByLocTypeParam _GetARAgingByLocType, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetARAgingByLocType(_GetARAgingByLocType, ConnectionString);
        }

        public List<GetARAgingByLocTypeDetailViewModel> iCollectionsReport_GetARAgingByLocTypeDetail(GetARAgingByLocTypeDetailParam _GetARAgingByLocTypeDetail, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetARAgingByLocTypeDetail(_GetARAgingByLocTypeDetail, ConnectionString);
        }


        /// <summary>
        /// For ARAgingReport360ByLocation List Screen : ARAgingReport360ByLocation.aspx / ARAgingReport360ByLocation.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public ListGetARAging360ByAsOfDate iCollectionsReport_GetARAging360ByAsOfDate(GetARAging360ByAsOfDateParam _GetARAging360ByAsOfDate, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetARAging360ByAsOfDate(_GetARAging360ByAsOfDate, ConnectionString);
        }


        /// <summary>
        /// For ARAgingReportDep Screen : ARAgingReportDep.aspx / ARAgingReportDep.aspx.cs
        /// </summary>
        /// API's Naming Conventions : iCollectionsReport_Method Name(Parameter)
        /// 

        public List<GetARAgingDepViewModel> iCollectionsReport_GetARAgingDep(GetARAgingDepParam _GetARAgingDep, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetARAgingDep(_GetARAgingDep, ConnectionString);
        }
        public ListGetARAgingByAsOfDateDep iCollectionsReport_GetARAgingByAsOfDateDep(GetARAgingByAsOfDateDepParam _GetARAgingByAsOfDateDep, string ConnectionString)
        {
            return new BusinessLayer.BL_Contracts().GetARAgingByAsOfDateDep(_GetARAgingByAsOfDateDep, ConnectionString);
        }
    }
}
