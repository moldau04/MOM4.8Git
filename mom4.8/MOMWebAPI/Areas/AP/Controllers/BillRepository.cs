using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.payroll;

namespace MOMWebAPI.Areas.AP.Controllers
{
    public class BillRepository : IBillRepository
    {
        /// <summary>
        /// For Managebills Page : Managebills.aspx / Managebills.aspx.cs
        /// </summary>
        /// 

        public List<GetAllPJRecurrDetailsViewModel> BillsList_GetAllPJRecurrDetails(GetAllPJRecurrDetailsParam _GetAllPJRecurrDetailsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetAllPJRecurrDetails(_GetAllPJRecurrDetailsParam, ConnectionString);
        }

        public List<GetAllPJDetailsViewModel> BillsList_GetAllPJDetails(GetAllPJDetailsParam _GetAllPJDetailsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetAllPJDetails(_GetAllPJDetailsParam, ConnectionString);
        }

        public int BillsList_ProcessRecurBill(ProcessRecurBillParam _GetAllPJDetailsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().ProcessRecurBill(_GetAllPJDetailsParam, ConnectionString);
        }

        public void BillsList_DeleteAPBillRecurr(DeleteAPBillRecurrParam _DeleteAPBillRecurrParam, string ConnectionString)
        {
            new BusinessLayer.BL_Bills().DeleteAPBillRecurr(_DeleteAPBillRecurrParam, ConnectionString);
        }

        public List<CDViewModel> BillsList_GetProcessRecurrCount(GetProcessRecurrCountParam _GetProcessRecurrCountParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetProcessRecurrCount(_GetProcessRecurrCountParam, ConnectionString);
        }

        public void BillsList_DeleteAPBill(DeleteAPBillParam _DeleteAPBillParam, string ConnectionString)
        {
            new BusinessLayer.BL_Bills().DeleteAPBill(_DeleteAPBillParam, ConnectionString);
        }

        public void BillsList_UpdateAPDates(UpdateAPDatesParam _UpdateAPDatesParam, string ConnectionString)
        {
            new BusinessLayer.BL_Bills().UpdateAPDates(_UpdateAPDatesParam, ConnectionString);
        }

        public List<GetPJAcctDetailByIDViewModel> BillsList_GetPJAcctDetailByID(GetPJAcctDetailByIDParam _GetPJAcctDetailByID, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetPJAcctDetailByID(_GetPJAcctDetailByID, ConnectionString);
        }

        public List<GetControlViewModel> BillsList_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }

        public List<CustomerReportViewModel> BillsList_GetStockReports(GetStockReportsParam _GetStockReportsParam, string connectionString)
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


        /// <summary>
        /// For AddBill Page : Addbill.aspx / Addbill.aspx.cs
        /// </summary>
        /// 

        public ListGetOutStandingPOById AddBills_GetOutStandingPOById(string ConnectionString, GetOutStandingPOByIdParam _GetOutStandingPOByIdParam)
        {
            return new BusinessLayer.BL_Bills().GetOutStandingPOById(ConnectionString,_GetOutStandingPOByIdParam); 
        }

        public List<VendorViewModel> AddBills_GetVendorSearch(string ConnectionString, GetVendorSearchParam _GetVendorSearchParam)
        {
            return new BusinessLayer.BL_Vendor().GetVendorSearch(ConnectionString, _GetVendorSearchParam);
        }

        public List<GetReceivePOListViewModel> AddBills_GetReceivePOList(string ConnectionString, GetReceivePOListParam _GetReceivePOListParam)
        {
            return new BusinessLayer.BL_Bills().GetReceivePOList(ConnectionString, _GetReceivePOListParam);
        }

        public void AddBills_AddReceivePOItem(string ConnectionString, AddReceivePOItemParam _AddReceivePOItemParam)
        {
             new BusinessLayer.BL_Bills().AddReceivePOItem(ConnectionString, _AddReceivePOItemParam);
        }

        public void AddBills_UpdatePOItemBalance(string ConnectionString, UpdatePOItemBalanceParam _UpdatePOItemBalanceParam)
        {
            new BusinessLayer.BL_Bills().UpdatePOItemBalance(ConnectionString, _UpdatePOItemBalanceParam);
        }


        public void AddBills_UpdatePOItemQuan(string ConnectionString, UpdatePOItemQuanParam _UpdatePOItemQuanParam)
        {
            new BusinessLayer.BL_Bills().UpdatePOItemQuan(ConnectionString, _UpdatePOItemQuanParam);
        }

        public void AddBills_AddEditReceivePO(string ConnectionString, AddEditReceivePOParam _AddEditReceivePOParam)
        {
            new BusinessLayer.BL_Bills().AddEditReceivePO(ConnectionString, _AddEditReceivePOParam);
        }

        public void AddBills_updateJobComm(string ConnectionString, updateJobCommParam _updateJobCommParam)
        {
           // new BusinessLayer.BL_Bills().UpdateJobComm(ConnectionString, _updateJobCommParam);
        }

        public ListGetPOReceivePOById AddBills_GetPOReceivePOById(string ConnectionString, GetPOReceivePOByIdParam _GetPOReceivePOByIdParam)
        {
           return  new BusinessLayer.BL_Bills().GetPOReceivePOById(ConnectionString, _GetPOReceivePOByIdParam);
        }

        public List<GetPJDetailByIDViewModel> AddBills_GetPJDetailByID(GetPJDetailByIDParam _GetPJDetailByIDParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetPJDetailByID(_GetPJDetailByIDParam, ConnectionString);
        }


        public List<GetBillTransDetailsViewModel> AddBills_GetBillTransDetails(GetBillTransDetailsParam _GetBillTransDetailsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetBillTransDetails(_GetBillTransDetailsParam, ConnectionString);
        }

       public List<GetPJRecurrDetailByIDViewModel> AddBills_GetPJRecurrDetailByID(GetPJRecurrDetailByIDParam _GetPJRecurrDetailByIDParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetPJRecurrDetailByID(_GetPJRecurrDetailByIDParam, ConnectionString);
        }

        public List<GetBillRecurrTransactionsViewModel> AddBills_GetBillRecurrTransactions(GetBillRecurrTransactionsParam _GetBillRecurrTransactionsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetBillRecurrTransactions(_GetBillRecurrTransactionsParam, ConnectionString);
        }

        public void AddBills_UpdatePOStatus(string ConnectionString, UpdatePOStatusParam _UpdatePOStatusParam)
        {
            new BusinessLayer.BL_Bills().UpdatePOStatus(ConnectionString, _UpdatePOStatusParam);
        }

        public void AddBills_UpdateReceivePOStatusByPOID(string ConnectionString, UpdateReceivePOStatusByPOIDParam _UpdateReceivePOStatusByPOIDParam)
        {
            new BusinessLayer.BL_Bills().UpdateReceivePOStatusByPOID(ConnectionString, _UpdateReceivePOStatusByPOIDParam);
        }

        public void AddBills_UpdateReceivePOStatus(string ConnectionString, UpdateReceivePOStatusParam _UpdateReceivePOStatusParam)
        {
            new BusinessLayer.BL_Bills().UpdateReceivePOStatus(ConnectionString, _UpdateReceivePOStatusParam);
        }

       public List<GetBillHistoryPaymentViewModel> AddBills_GetBillHistoryPayment(GetBillHistoryPaymentParam _GetBillHistoryPaymentParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetBillHistoryPayment(_GetBillHistoryPaymentParam, ConnectionString);
        }

        public bool AddBills_IsExistPO(string ConnectionString, IsExistPOParam _IsExistPOParam)
        {
            return new BusinessLayer.BL_Bills().IsExistPO(ConnectionString, _IsExistPOParam);
        }

       public string AddBills_GetClosePOCheck(string ConnectionString, GetClosePOCheckParam _GetClosePOCheckParam)
        {
            return new BusinessLayer.BL_Bills().GetClosePOCheck(ConnectionString, _GetClosePOCheckParam);
        }

        public  int AddBills_GetMaxReceivePOId(string ConnectionString, GetMaxReceivePOIdParam _GetMaxReceivePOIdParam)
        {
            return new BusinessLayer.BL_Bills().GetMaxReceivePOId(ConnectionString, _GetMaxReceivePOIdParam);
        }

        public  List<BOMTViewModel> AddBills_GetBomType(GetBomTypeParam _GetBomTypeParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Job().GetBomType(_GetBomTypeParam ,ConnectionString);
        }

        public List<GeneralViewModel> AddBills_GetInvDefaultAcct(GetInvDefaultAcctParam _GetInvDefaultAcctParam, string ConnectionString)
        {
            return new BusinessLayer.BL_General().getInvDefaultAcct(_GetInvDefaultAcctParam, ConnectionString);
        }

        public ListGetInventoryItemStatus AddBills_GetInventoryItemStatus(string ConnectionString, GetInventoryItemStatusParam _GetInventoryItemStatusParam)
        {
            return new BusinessLayer.BL_Bills().GetInventoryItemStatus(ConnectionString, _GetInventoryItemStatusParam);
        }

         public void AddBills_UpdatePOItemWarehouseLocation(UpdatePOItemWarehouseLocationParam _UpdatePOItemWarehouseLocationParam, string ConnectionString)
        {
           new BusinessLayer.BL_Bills().UpdatePOItemWarehouseLocation(_UpdatePOItemWarehouseLocationParam, ConnectionString);
        }

         public void AddBills_AddReceiveInventoryWHTrans(AddReceiveInventoryWHTransParam _AddReceiveInventoryWHTrans, string ConnectionString)
        {
            new BusinessLayer.BL_Bills().AddReceiveInventoryWHTrans(_AddReceiveInventoryWHTrans, ConnectionString);
        }

        public void AddBills_CreateReceivePOInvWarehouse(CreateReceivePOInvWarehouseTransParam _CreateReceivePOInvWarehouseTransParam, string ConnectionString)
        {
            new BusinessLayer.BL_Inventory().CreateReceivePOInvWarehouse(_CreateReceivePOInvWarehouseTransParam, ConnectionString);
        }

        public List<CustomViewModel> AddBills_GetCustomField(getCustomFieldParam _getCustomFieldParam, string ConnectionString)
        {
            return new BusinessLayer.BL_General().getCustomField(_getCustomFieldParam, ConnectionString);
        }

        public List<STaxViewModel> AddBills_GetSTax(getSTaxParam _getSTaxParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_User().getSTax(_getSTaxParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomViewModel> AddBills_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_General().getCustomFieldsControl(_getCustomFieldsControlParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ChartViewModel> AddBills_GetChart(GetChartParam _GetChartParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Chart().GetChart(_GetChartParam, ConnectionString);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<VendorViewModel> AddBills_GetVendorByName(GetVendorByNameParam _GetVendorByNameParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetVendorByName(_GetVendorByNameParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetAutoFillOnHandBalanceViewModel> AddBills_GetAutoFillOnHandBalance(GetAutoFillOnHandBalanceParam _GetAutoFillOnHandBalanceParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Inventory().GetAutoFillOnHandBalance(_GetAutoFillOnHandBalanceParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddBills_AddBills(AddBillsParam _AddBillsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().AddBills(_AddBillsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddBills_UpdateRecurrBills(UpdateRecurrBillsParam _UpdateRecurrBillsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().UpdateRecurrBills(_UpdateRecurrBillsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddBills_UpdateBills(UpdateBillsParam _UpdateBillsParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdateBills(_UpdateBillsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomViewModel> AddBills_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString)
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
        public void AddBills_UpdateVendorSTax(UpdateVendorSTaxParam _UpdateVendorSTaxParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Vendor().UpdateVendorSTax(_UpdateVendorSTaxParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddBills_CreateReceivePOInvWarehouseTrans(ReceivePOInvWarehouseTransParam _ReceivePOInvWarehouseTransParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Inventory().CreateReceivePOInvWarehouseTrans(_ReceivePOInvWarehouseTransParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AddBills_ISINVENTORYTRACKINGISON(string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Inventory().ISINVENTORYTRACKINGISON(ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserViewModel> AddBills_GetUserById(GetUserByIdParam _GetUserByIdParam, string connectionString)
        {
            return new BusinessLayer.BL_User().getUserByID(_GetUserByIdParam, connectionString);
        }

        public void AddBills_UpdateBillsJobDetails(UpdateBillsJobDetailsParam _UpdateBillsJobDetailsParam, string connectionString)
        {
             new BusinessLayer.BL_Bills().UpdateBillsJobDetails(_UpdateBillsJobDetailsParam, connectionString);
        }

        public ListGetBillingItems AddBills_GetBillingItems(GetBillingItemsParam _GetBillingItemsParam, string connectionString)
        {
            return new BusinessLayer.BL_Bills().GetBillingItems(_GetBillingItemsParam, connectionString);
        }

        public List<GetControlViewModel> AddBills_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }

        public List<LogViewModel> AddBills_GetBillsLogs(GetBillsLogsParam _GetBillsLogs, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetBillsLogs(_GetBillsLogs, ConnectionString);
        }

        public int AddBills_UpdateApplyCreditDate(UpdateApplyCreditDateParam _UpdateApplyCreditDateParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().UpdateApplyCreditDate(_UpdateApplyCreditDateParam, ConnectionString);
        }


        /// <summary>
        /// For Reports API's : 
        /// </summary>


        /// <summary>
        /// 1) BillListingRepor.aspx / BillListingRepor.aspx.cs
        /// </summary>
        /// 

        public ListGetBillReportFiltersValue BillsReport_GetBillReportFiltersValue(GetBillReportFiltersValueParam _GetBillReportFiltersValueParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetBillReportFiltersValue(_GetBillReportFiltersValueParam, ConnectionString);
        }

        public List<BillReportDetails> BillsReport_GetBillDetails(GetBillDetailsParam _GetBillDetailsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetBillDetails(_GetBillDetailsParam, ConnectionString);
        }
        public List<UserViewModel> BillsReport_GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetAccountSummaryListingDetail(_GetAccountSummaryListingDetailParam, ConnectionString);
        }
        public List<CustomerReportViewModel> BillsReport_GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportDetailById(_GetReportDetailByIdParam, ConnectionString);
        }

        public List<CustomerReportViewModel> BillsReport_GetCustomerType(GetCustomerTypeParam _GetCustomerType, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerType(_GetCustomerType, ConnectionString);

        }

        public List<CustomerFilterViewModel> BillsReport_GetCustomerName(GetCustomerNameParam _GetCustomerNameParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerName(_GetCustomerNameParam, ConnectionString);

        }
         ///-- ///
        public List<CustomerFilterViewModel> BillsReport_GetCustomerAddress(GetCustomerAddressParam _GetCustomerAddressParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerAddress(_GetCustomerAddressParam, ConnectionString);

        }
        public List<CustomerFilterViewModel> BillsReport_GetCustomerCity(GetCustomerCityParam _GetCustomerCityParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetCustomerCity(_GetCustomerCityParam, ConnectionString);

        }
        public List<CustomerReportViewModel> BillsReport_GetDynamicReports(GetDynamicReportsParam _GetDynamicReportsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetDynamicReports(_GetDynamicReportsParam, ConnectionString);

        }
        public List<CustomerFilterViewModel> BillsReport_GetReportColByRepId(GetReportColByRepIdParam _GetReportColByRepId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportColByRepId(_GetReportColByRepId, ConnectionString);

        }
        public List<CustomerFilterViewModel> BillsReport_GetReportFiltersByRepId(GetReportFiltersByRepIdParam _GetReportFiltersByRepId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetReportFiltersByRepId(_GetReportFiltersByRepId, ConnectionString);

        }
        public bool BillsReport_CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().CheckExistingReport(_CheckExistingReportParam, ConnectionString);

        }
        public List<CustomerReportViewModel> BillsReport_InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().InsertCustomerReport(_InsertCustomerReportParam, ConnectionString);

        }
        public bool BillsReport_IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().IsStockReportExist(_IsStockReportExistParam, ConnectionString);

        }
        public void BillsReport_UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReport(_UpdateCustomerReportParam, ConnectionString);

        }
        public void BillsReport_DeleteCustomerReport(DeleteCustomerReportParam _DeleteCustomerReport, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().DeleteCustomerReport(_DeleteCustomerReport, ConnectionString);

        }
        public List<CustomerFilterViewModel> BillsReport_GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetControlForReports(_getConnectionConfigParam, ConnectionString);

        }
        public List<HeaderFooterDetailViewModel> BillsReport_GetHeaderFooterDetail(GetHeaderFooterDetailParam _GetHeaderFooterDetail, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetHeaderFooterDetail(_GetHeaderFooterDetail, ConnectionString);

        }
        public List<CustomerFilterViewModel> BillsReport_GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetOwners(_GetOwnersParam, ConnectionString);

        }
        public List<CustomerReportViewModel> BillsReport_GetColumnWidthByReportId(GetColumnWidthByReportIdParam _GetColumnWidthByReportId, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetColumnWidthByReportId(_GetColumnWidthByReportId, ConnectionString);

        }
        public void BillsReport_UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReportResizedWidth(_UpdateCustomerReportResizedWidth, ConnectionString);

        }




        /// <summary>
        /// 2) BillsReport.aspx / BillsReport.aspx.cs
        /// </summary>

        public List<GetCompanyDetailsViewModel> BillsReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
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
        public List<SMTPEmailViewModel> BillsReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_user, ConnectionString);
        }



        /// <summary>
        /// 3) APAgingReport.aspx / APAgingReport.aspx.cs
        /// </summary>

        public List<GetBillsDetailsByDueViewModel> BillsReport_GetBillsDetailsByDue(GetBillsDetailsByDueParam _GetBillsDetailsByDueParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetBillsDetailsByDue(_GetBillsDetailsByDueParam, ConnectionString);
        }

        public List<GetAPAgingByDateViewModel> BillsReport_GetAPAgingByDate(GetAPAgingByDateParam _GetAPAgingByDateParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetAPAgingByDate(_GetAPAgingByDateParam, ConnectionString);
        }



        /// <summary>
        /// 4) PurchaseJournalReport.aspx / PurchaseJournalReport.aspx.cs
        /// </summary>

        public List<OpenAPViewModel> BillsReport_GetPurchaseJournal(GetPurchaseJournalParam _GetPurchaseJournalParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Report().GetPurchaseJournal(_GetPurchaseJournalParam, ConnectionString);
        }


        /// <summary>
        /// 5) UseTaxReport.aspx / UseTaxReport.aspx.cs
        /// </summary>

        public List<GetUseTaxViewModel> BillsReport_GetUseTax(GetUseTaxForReportsParam _GetUseTaxForReportsParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetUseTax(_GetUseTaxForReportsParam, ConnectionString);
        }

        /// <summary>
        /// 6) UTaxLocReport.aspx / UTaxLocReport.aspx.cs
        /// </summary>

        public List<GetUTaxLocReportViewModel> BillsReport_GetUTaxLocReport(GetUTaxLocReportParam _GetUTaxLocReportParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetUTaxLocReport(_GetUTaxLocReportParam, ConnectionString);
        }


        /// <summary>
        /// 7) PrintBillRegisterGL.aspx / PrintBillRegisterGL.aspx.cs
        /// </summary>

        public List<GetAPGLRegViewModel> BillsReport_GetAPGLReg(GetAPGLRegParam _GetAPGLRegParam, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetAPGLReg(_GetAPGLRegParam, ConnectionString);
        }

        /// <summary>
        /// 8) APAging360Report.aspx / APAging360Report.aspx.cs
        /// </summary>

        public List<GetBillsDetails360ByDueViewModel> BillsReport_GetBillsDetails360ByDue(GetBillsDetails360ByDueParam _GetBillsDetails360ByDue, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetBillsDetails360ByDue(_GetBillsDetails360ByDue, ConnectionString);
        }

        public List<GetAPAging360ByDateViewModel> BillsReport_GetAPAging360ByDate(GetAPAging360ByDateParam _GetAPAging360ByDate, string ConnectionString)
        {
            return new BusinessLayer.BL_Bills().GetAPAging360ByDate(_GetAPAging360ByDate, ConnectionString);
        }
    }
}
