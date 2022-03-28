using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity.APModels;
using BusinessEntity;
using BusinessEntity.Payroll;
using BusinessEntity.payroll;

namespace MOMWebAPI.Areas.AP.Controllers
{
    public interface IManageChecksRepository
    {
        /// <summary>
        /// For ManageChecks List Screen : ManageChecks.aspx / ManageChecks.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ManageChecksList_Method Name(Parameter)

        List<GetAllBankNamesViewModel> CheckList_GetAllBankNames(GetAllBankNamesParam _GetAllBankNamesParam, string ConnectionString);
        List<GetCDByIDViewModel> CheckList_GetCDByID(GetCDByIDParam _GetCDByIDParam, string ConnectionString);
        void CheckList_UpdateCDVoid(UpdateCDVoidParam _UpdateCDVoidParam, string ConnectionString);
        List<TransactionViewModel> CheckList_GetTransByID(GetTransByIDParam _GetTransByIDParam, string ConnectionString);
        void CheckList_UpdateTransVoidCheck(UpdateTransVoidCheckParam _UpdateTransVoidCheckParam, string ConnectionString);
        void CheckList_UpdateTransVoidCheckByBatch(UpdateTransVoidCheckByBatchParam _UpdateTransVoidCheckByBatchParam, string ConnectionString);
        List<PaidViewModel> CheckList_GetPaidDetailByID(GetPaidDetailByIDParam _GetPaidDetailByIDParam, string ConnectionString);
        List<TransactionViewModel> CheckList_GetTransByBatch(GetTransByBatchParam _GetTransByBatchParam, string ConnectionString);
        int CheckList_GetMaxTransBatch(GetMaxTransBatchParam _GetMaxTransBatchParam, string ConnectionString);
        int CheckList_GetMaxTransRef(GetMaxTransRefParam _GetMaxTransRefParam, string ConnectionString);
        void CheckList_AddGLA(AddGLAParam _AddGLAParam, string ConnectionString);
        List<ChartViewModel> CheckList_GetAcctPayable(GetAcctPayableParam _GetAcctPayableParam, string ConnectionString);
        int CheckList_AddJournalTrans(AddJournalTransParam _AddJournalTransParam, string ConnectionString);
        void CheckList_UpdateChartBalance(UpdateChartBalanceParam _UpdateChartBalanceParam, string ConnectionString);
        int CheckList_GetBankAcctID(GetBankAcctIDParam _GetBankAcctIDParam, string ConnectionString);
        List<JobIViewModel> CheckList_GetJobIByTransID(GetJobIByTransIDParam _GetJobIByTransIDParam, string ConnectionString);
        void CheckList_AddJobI(AddJobIParam _AddJobIParam, string ConnectionString);
        List<GetPJDetailByBatchViewModel> CheckList_GetPJDetailByBatch(GetPJDetailByBatchParam _GetPJDetailByBatchParam, string ConnectionString);
        int CheckList_AddPJ(AddPJParam _AddPJParam, string ConnectionString);
        void CheckList_UpdatePJOnVoidCheck(UpdatePJOnVoidCheckParam _UpdatePJOnVoidCheckParam, string ConnectionString);
        List<OpenAPViewModel> CheckList_GetOpenAPByPJID(GetOpenAPByPJIDParam _GetOpenAPByPJIDParam, string ConnectionString);
        void CheckList_AddOpenAP(AddOpenAPParam _AddOpenAPParam, string ConnectionString);
        void CheckList_UpdateVendorBalance(UpdateVendorBalanceParam _UpdateVendorBalanceParam, string ConnectionString);
        List<GetAllCDViewModel> CheckList_GetAllCD(GetAllCDParam _GetAllCDParam, string ConnectionString);
        void CheckList_DeleteRecurrCheck(DeleteRecurrCheckParam _DeleteRecurrCheckParam, string ConnectionString);
        List<CDViewModel> CheckList_GetProcessRecurrCheckCount(GetProcessRecurrCheckCountParam _GetProcessRecurrCheckCountParam, string ConnectionString);
        void CheckList_DeleteCheckDetails(DeleteCheckDetailsParam _DeleteCheckDetailsParam, string ConnectionString);
        List<CDRecurrViewModel> CheckList_GetCheckRecurrDetails(GetCheckRecurrDetailsParam _GetCheckRecurrDetailsParam, string ConnectionString);
        void CheckList_UpdateOpenAPBalance(UpdateOpenAPBalanceParam _UpdateOpenAPBalanceParam, string ConnectionString);
        List<CDViewModel> CheckList_GetDataTypeCD(GetDataTypeCDParam _GetDataTypeCDParam, string ConnectionString);
        ListCheckDetailsByBankAndRef CheckList_GetCheckDetailsByBankAndRef(GetCheckDetailsByBankAndRefParam _GetCheckDetailsByBankAndRefParam, string ConnectionString);
        List<VendorViewModel> CheckList_GetVendorRolDetails(GetVendorRolDetailsParam _GetVendorRolDetailsParam, string ConnectionString);
        List<UserViewModel> CheckList_GetControlBranch(GetControlBranchParam _GetControlBranchParam, string ConnectionString);
        void CheckList_UpdateCDVoidOpen(UpdateCDVoidOpenParam _UpdateCDVoidOpenParam, string ConnectionString);
        void CheckList_UpdateAPCDVoidLog(UpdateAPCDVoidLogParam _UpdateAPCDVoidLogParam, string ConnectionString);
        void CheckList_UpdateTransVoidCheckOpen(UpdateTransVoidCheckOpenParam _UpdateTransVoidCheckOpenParam, string ConnectionString);
        void CheckList_UpdateTransVoidCheckByBatchOpen(UpdateTransVoidCheckByBatchOpenParam _UpdateTransVoidCheckByBatchOpenParam, string ConnectionString);
        int CheckList_ProcessRecurCheck(ProcessRecurCheckParam _ProcessRecurCheckParam, string ConnectionString);
        void CheckList_UpdatePaidOnVoidCheck(UpdatePaidOnVoidCheckParam _UpdatePaidOnVoidCheckParam, string ConnectionString);
        public List<GetControlViewModel> CheckList_GetControl(getConnectionConfigParam _user, string ConnectionString);
        List<UserViewModel> CheckList_GetUserById(GetUserByIdParam _GetUserByIdParam, string ConnectionString);
        void CheckList_UpdateCDCheckNo(UpdateCDCheckNoParam _UpdateCDCheckNoParam, string ConnectionString);
        void CheckList_UpdateTransCheckNoByBatch(UpdateTransCheckNoByBatchParam _UpdateTransCheckNoByBatchParam, string ConnectionString);


        /// <summary>
        /// For ChecksReport Page : ChecksReport.aspx / ChecksReport.aspx.cs
        /// </summary>
        /// 

        List<GetCompanyDetailsViewModel> ChecksReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString);

        List<CDViewModel> ChecksReport_GetChecksReportData(GetChecksReportDataParam _GetChecksReportDataParam, string ConnectionString);
        public List<SMTPEmailViewModel> ChecksReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString);




        /// <summary>
        /// For WriteCheck Page : WriteCheck.aspx / WriteCheck.aspx.cs
        /// </summary>
        /// 

        bool AddCheck_ISINVENTORYTRACKINGISON(string ConnectionString);
        List<VendorViewModel> AddCheck_GetVendorByName(GetVendorByNameParam _GetVendorByNameParam, string ConnectionString);
        List<UserViewModel> AddCheck_getUserDefaultCompany(getUserDefaultCompanyParam _getUserDefaultCompanyParam, string ConnectionString);
        List<VendorViewModel> AddCheck_GetOpenBillVendorByCompany(GetOpenBillVendorByCompanyParam _GetOpenBillVendorByCompanyParam, string ConnectionString);
        List<BankViewModel> AddCheck_GetAllBankNamesByCompany(GetAllBankNamesByCompanyParam _GetAllBankNamesByCompanyParam, string ConnectionString);
        List<VendorViewModel> AddCheck_GetOpenBillVendor(GetOpenBillVendorParam _GetOpenBillVendorParam, string ConnectionString);
        List<OpenAPViewModel> AddCheck_GetRunningBalanceCounts(GetRunningBalanceCountsParam _GetRunningBalanceCountsParam, string ConnectionString);
        List<UserViewModel> AddCheck_GetCheckTemplate(GetCheckTemplateParam _GetCheckTemplateParam, string ConnectionString);
        ListGetAutoSelectPayment AddCheck_GetAutoSelectPayment(GetAutoSelectPaymentParam _GetAutoSelectPaymentParam, string ConnectionString);
        List<VendorViewModel> AddCheck_GetVendor(GetVendorParam _GetVendorParam, string ConnectionString);
        List<OpenAPViewModel> AddCheck_GetBillsByVendor(GetBillsByVendorParam _GetBillsByVendorParam, string ConnectionString);
        int AddCheck(AddCheckParam _AddCheckParam, string ConnectionString);
        List<GetBankByIDViewModel> AddCheck_GetBankByID(GetBankByIDParam _GetBankByIDParam, string ConnectionString);
        int AddCheck_ApplyCredit(ApplyCreditParam _ApplyCreditParam, string ConnectionString);
        List<OpenAPViewModel> AddCheck_GetCreditBillVendor(GetCreditBillVendorParam _GetCreditBillVendorParam, string ConnectionString);
        List<BankViewModel> AddCheck_GetBankCD(GetBankCDParam _GetBankCDParam, string ConnectionString);
        List<GetVendorAcctList> AddCheck_GetVendorAcct(GetVendorAcctParam _GetVendorAcctParam, string ConnectionString);
        void AddCheck_updateCheckTemplate(updateCheckTemplateParam _updateCheckTemplateParam, string ConnectionString);
        ListAutoSelectPayment AddCheck_AutoSelectPayment(AutoSelectPaymentParam _AutoSelectPaymentParam, string ConnectionString);
        string AddCheck_AddBills(AddBillsParam _AddBillsParam, string ConnectionString);
        int AddCheck_AddCheckRecurr(AddCheckRecurrParam _AddCheckRecurrParam, string ConnectionString);
        List<OpenAPViewModel> AddCheck_GetSelectedOpenAPPJID(GetSelectedOpenAPPJIDParam _GetSelectedOpenAPPJIDParam, string ConnectionString);
        void AddCheck_UpdateWriteCheckOpenAPpayment(UpdateWriteCheckOpenAPpaymentParam _UpdateWriteCheckOpenAPpaymentParam, string ConnectionString);
        List<CustomViewModel> AddCheck_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString);
        List<CustomViewModel> AddCheck_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString);
        List<ChartViewModel> AddCheck_GetChart(GetChartParam _GetChartParam, string ConnectionString);
        List<GeneralViewModel> AddCheck_GetInvDefaultAcct(GetInvDefaultAcctParam _GetInvDefaultAcctParam, string ConnectionString);
        List<CompanyOfficeViewModel> AddCheck_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString);
        bool AddCheck_IsExistCheckNum(IsExistCheckNumParam _IsExistCheckNumParam, string ConnectionString);
        List<CustomViewModel> AddCheck_GetCustomField(getCustomFieldParam _getCustomFieldParam, string ConnectionString);



        /// <summary>
        /// For WriteCheckRecurr Page : WriteCheckRecurr.aspx / WriteCheckRecurr.aspx.cs
        /// </summary>
        /// 

        List<GetRecurCDByIDViewModel> AddCheckRecurr_GetRecurCDByID(GetRecurCDByIDParam _GetRecurCDByIDParam, string ConnectionString);
        int AddCheckRecurr_UpdateCheckRecurr(UpdateCheckRecurrParam _UpdateCheckRecurrParam, string ConnectionString);

        List<GetRecurrBillDetailByIDViewModel> AddCheckRecurr_GetRecurrBillDetailByID(GetRecurrBillDetailByIDParam _GetRecurrBillDetailByIDParam, string ConnectionString);

        string AddCheckRecurr_UpdateRecurrBills(UpdateRecurrBillsParam _UpdateRecurrBillsParam, string ConnectionString);



        /// <summary>
        /// For EditCheck Page : EditCheck.aspx / EditCheck.aspx.cs
        /// </summary>
        /// 

        List<TransactionViewModel> EditCheck_GetTransByBatchType(GetTransByBatchTypeParam _GetTransByBatchTypeParam, string ConnectionString);
        void EditCheck_UpdateAPCDDate(UpdateAPCDDateParam _UpdateAPCDDateParam, string ConnectionString);
        void EditCheck_UpdateNextCheck(UpdateNextCheckParam _UpdateNextCheckParam, string ConnectionString);
        void EditCheck_UpdateTransDateByBatch(UpdateTransDateByBatchParam _UpdateTransDateByBatchParam, string ConnectionString);
        List<LogViewModel> EditCheck_GetAPCheckLogs(GetAPCheckLogsParam _GetAPCheckLogsParam, string ConnectionString);
        bool EditCheck_IsExistCheckNumOnEdit(IsExistCheckNumOnEditParam _IsExistCheckNumOnEdit, string ConnectionString);
    }
}
