using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity.APModels;
using BusinessEntity;
using System.Text;
using System.Reflection;
using BusinessEntity.Payroll;
using BusinessEntity.payroll;

namespace MOMWebAPI.Areas.AP.Controllers
{
    public class ManageChecksRepository : IManageChecksRepository
    {
        /// <summary>
        /// For ManageChecks List Screen : ManageChecks.aspx / ManageChecks.aspx.cs
        /// </summary>
        /// API's Naming Conventions : ManageChecksList_Method Name(Parameter)
        /// 
        public List<GetAllBankNamesViewModel> CheckList_GetAllBankNames(GetAllBankNamesParam _GetAllBankNamesParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_BankAccount().GetAllBankNames(_GetAllBankNamesParam,ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<GetCDByIDViewModel> CheckList_GetCDByID(GetCDByIDParam _GetCDByIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetCDByID(_GetCDByIDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public void CheckList_UpdateCDVoid(UpdateCDVoidParam _UpdateCDVoidParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdateCDVoid(_UpdateCDVoidParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TransactionViewModel> CheckList_GetTransByID(GetTransByIDParam _GetTransByIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_JournalEntry().GetTransByID(_GetTransByIDParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateTransVoidCheck(UpdateTransVoidCheckParam _UpdateTransVoidCheckParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_JournalEntry().UpdateTransVoidCheck(_UpdateTransVoidCheckParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateTransVoidCheckByBatch(UpdateTransVoidCheckByBatchParam _UpdateTransVoidCheckByBatchParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_JournalEntry().UpdateTransVoidCheckByBatch(_UpdateTransVoidCheckByBatchParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PaidViewModel> CheckList_GetPaidDetailByID(GetPaidDetailByIDParam _GetPaidDetailByIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetPaidDetailByID(_GetPaidDetailByIDParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TransactionViewModel> CheckList_GetTransByBatch(GetTransByBatchParam _GetTransByBatchParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_JournalEntry().GetTransByBatch(_GetTransByBatchParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckList_GetMaxTransBatch(GetMaxTransBatchParam _GetMaxTransBatchParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_JournalEntry().GetMaxTransBatch(_GetMaxTransBatchParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckList_GetMaxTransRef(GetMaxTransRefParam _GetMaxTransRefParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_JournalEntry().GetMaxTransRef(_GetMaxTransRefParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_AddGLA(AddGLAParam _AddGLAParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_JournalEntry().AddGLA(_AddGLAParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ChartViewModel> CheckList_GetAcctPayable(GetAcctPayableParam _GetAcctPayableParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Chart().GetAcctPayable(_GetAcctPayableParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //private static List<T> ConvertDataTable<T>(DataTable dt)
        //{
        //    List<T> data = new List<T>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        T item = GetItem<T>(row);
        //        data.Add(item);
        //    }
        //    return data;
        //}

        //private static T GetItem<T>(DataRow dr)  
        //{  
        //   Type temp = typeof(T);
        //   T obj = Activator.CreateInstance<T>();  
        //   foreach (DataColumn column in dr.Table.Columns)  
        //   {  
        //      foreach (PropertyInfo pro in temp.GetProperties())  
        //      {  
        //         if (pro.Name == column.ColumnName)  
        //         pro.SetValue(obj, DBNull.Value.Equals(dr[column.ColumnName]) ? 0 : Convert.ChangeType(dr[column.ColumnName],pro.PropertyType), null);  
        //         else  
        //         continue;  
        //      }
        //}  
        //   return obj;  
        //}  
        public int CheckList_AddJournalTrans(AddJournalTransParam _AddJournalTransParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_JournalEntry().AddJournalTrans(_AddJournalTransParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateChartBalance(UpdateChartBalanceParam _UpdateChartBalanceParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Chart().UpdateChartBalance(_UpdateChartBalanceParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckList_GetBankAcctID(GetBankAcctIDParam _GetBankAcctIDParam, string ConnectionString)
        {
            try
            {
               int BankGL = new BusinessLayer.BL_Chart().GetBankAcctID(_GetBankAcctIDParam, ConnectionString);
                return BankGL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobIViewModel> CheckList_GetJobIByTransID(GetJobIByTransIDParam _GetJobIByTransID, string ConnectionString)
        {
            try
            {
                return  new BusinessLayer.BL_Bills().GetJobIByTransID(_GetJobIByTransID, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_AddJobI(AddJobIParam _AddJobIParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().AddJobI(_AddJobIParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetPJDetailByBatchViewModel> CheckList_GetPJDetailByBatch(GetPJDetailByBatchParam _GetPJDetailByBatchParam, string ConnectionString)
        {
            try
            {
                return  new BusinessLayer.BL_Bills().GetPJDetailByBatch(_GetPJDetailByBatchParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckList_AddPJ(AddPJParam _AddPJParam, string ConnectionString)
        {
            try
            {
                int _newPJID = new BusinessLayer.BL_Bills().AddPJ(_AddPJParam, ConnectionString);
                return _newPJID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdatePJOnVoidCheck(UpdatePJOnVoidCheckParam _UpdatePJOnVoidCheckParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdatePJOnVoidCheck(_UpdatePJOnVoidCheckParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OpenAPViewModel> CheckList_GetOpenAPByPJID(GetOpenAPByPJIDParam _GetOpenAPByPJIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetOpenAPByPJID(_GetOpenAPByPJIDParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_AddOpenAP(AddOpenAPParam _AddOpenAPParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().AddOpenAP(_AddOpenAPParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateVendorBalance(UpdateVendorBalanceParam _UpdateVendorBalanceParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Vendor().UpdateVendorBalance(_UpdateVendorBalanceParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetAllCDViewModel> CheckList_GetAllCD(GetAllCDParam _GetAllCDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetAllCD(_GetAllCDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_DeleteRecurrCheck(DeleteRecurrCheckParam _DeleteRecurrCheckParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().DeleteRecurrCheck(_DeleteRecurrCheckParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CDViewModel> CheckList_GetProcessRecurrCheckCount(GetProcessRecurrCheckCountParam _GetProcessRecurrCheckCountParam, string ConnectionString)
        {
            try
            {
               return new BusinessLayer.BL_Bills().GetProcessRecurrCheckCount(_GetProcessRecurrCheckCountParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_DeleteCheckDetails(DeleteCheckDetailsParam _DeleteCheckDetailsParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().DeleteCheckDetails(_DeleteCheckDetailsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CDRecurrViewModel> CheckList_GetCheckRecurrDetails(GetCheckRecurrDetailsParam _GetCheckRecurrDetailsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetCheckRecurrDetails(_GetCheckRecurrDetailsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateOpenAPBalance(UpdateOpenAPBalanceParam _UpdateOpenAPBalanceParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdateOpenAPBalance(_UpdateOpenAPBalanceParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CDViewModel> CheckList_GetDataTypeCD(GetDataTypeCDParam _GetDataTypeCDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetDataTypeCD(_GetDataTypeCDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListCheckDetailsByBankAndRef CheckList_GetCheckDetailsByBankAndRef(GetCheckDetailsByBankAndRefParam _GetCheckDetailsByBankAndRefParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetCheckDetailsByBankAndRef(_GetCheckDetailsByBankAndRefParam, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<VendorViewModel> CheckList_GetVendorRolDetails(GetVendorRolDetailsParam _GetVendorRolDetailsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetVendorRolDetails(_GetVendorRolDetailsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserViewModel> CheckList_GetControlBranch(GetControlBranchParam _GetControlBranchParam, string ConnectionString)
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

        public void CheckList_UpdateCDVoidOpen(UpdateCDVoidOpenParam _UpdateCDVoidOpenParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdateCDVoidOpen(_UpdateCDVoidOpenParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateAPCDVoidLog(UpdateAPCDVoidLogParam _UpdateAPCDVoidLogParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdateAPCDVoidLog(_UpdateAPCDVoidLogParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateTransVoidCheckOpen(UpdateTransVoidCheckOpenParam _UpdateTransVoidCheckOpenParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_JournalEntry().UpdateTransVoidCheckOpen(_UpdateTransVoidCheckOpenParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdateTransVoidCheckByBatchOpen(UpdateTransVoidCheckByBatchOpenParam _UpdateTransVoidCheckByBatchOpenParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_JournalEntry().UpdateTransVoidCheckByBatchOpen(_UpdateTransVoidCheckByBatchOpenParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckList_ProcessRecurCheck(ProcessRecurCheckParam _ProcessRecurCheckParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().ProcessRecurCheck(_ProcessRecurCheckParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckList_UpdatePaidOnVoidCheck(UpdatePaidOnVoidCheckParam _UpdatePaidOnVoidCheckParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdatePaidOnVoidCheck(_UpdatePaidOnVoidCheckParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetControlViewModel> CheckList_GetControl(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getControl(_getConnectionConfigParam, ConnectionString);
        }

        public List<UserViewModel> CheckList_GetUserById(GetUserByIdParam _GetUserByIdParam, string connectionString)
        {
            return new BusinessLayer.BL_User().getUserByID(_GetUserByIdParam, connectionString);
        }

        public void CheckList_UpdateCDCheckNo(UpdateCDCheckNoParam _UpdateCDCheckNoParam, string ConnectionString)
        {
            new BusinessLayer.BL_Bills().UpdateCDCheckNo(_UpdateCDCheckNoParam, ConnectionString);
        }

        public void CheckList_UpdateTransCheckNoByBatch(UpdateTransCheckNoByBatchParam _UpdateTransCheckNoByBatchParam, string ConnectionString)
        {
            new BusinessLayer.BL_JournalEntry().UpdateTransCheckNoByBatch(_UpdateTransCheckNoByBatchParam, ConnectionString);
        }


        /// <summary>
        /// For ChecksReport Page : ChecksReport.aspx / ChecksReport.aspx.cs
        /// </summary>
        /// 

        public List<GetCompanyDetailsViewModel> ChecksReport_GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
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

        public List<CDViewModel> ChecksReport_GetChecksReportData(GetChecksReportDataParam _GetChecksReportDataParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Report().GetChecksReportData(_GetChecksReportDataParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SMTPEmailViewModel> ChecksReport_GetSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getSMTPByUserID(_user, ConnectionString);
        }



        /// <summary>
        /// For WriteCheck Page : WriteCheck.aspx / WriteCheck.aspx.cs
        /// </summary>
        /// 

        public bool AddCheck_ISINVENTORYTRACKINGISON(string ConnectionString)
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

        public List<VendorViewModel> AddCheck_GetVendorByName(GetVendorByNameParam _GetVendorByNameParam, string ConnectionString)
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

        public List<UserViewModel> AddCheck_getUserDefaultCompany(getUserDefaultCompanyParam _getUserDefaultCompanyParam, string ConnectionString)
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

        public List<VendorViewModel> AddCheck_GetOpenBillVendorByCompany(GetOpenBillVendorByCompanyParam _GetOpenBillVendorByCompanyParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetOpenBillVendorByCompany(_GetOpenBillVendorByCompanyParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BankViewModel> AddCheck_GetAllBankNamesByCompany(GetAllBankNamesByCompanyParam _GetAllBankNamesByCompanyParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_BankAccount().GetAllBankNamesByCompany(_GetAllBankNamesByCompanyParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorViewModel> AddCheck_GetOpenBillVendor(GetOpenBillVendorParam _GetOpenBillVendorParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetOpenBillVendor(_GetOpenBillVendorParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OpenAPViewModel> AddCheck_GetRunningBalanceCounts(GetRunningBalanceCountsParam _GetRunningBalanceCountsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetRunningBalanceCounts(_GetRunningBalanceCountsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserViewModel> AddCheck_GetCheckTemplate(GetCheckTemplateParam _GetCheckTemplateParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetCheckTemplate(_GetCheckTemplateParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListGetAutoSelectPayment AddCheck_GetAutoSelectPayment(GetAutoSelectPaymentParam _GetAutoSelectPaymentParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetAutoSelectPayment(_GetAutoSelectPaymentParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorViewModel> AddCheck_GetVendor(GetVendorParam _GetVendorParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetVendor(_GetVendorParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OpenAPViewModel> AddCheck_GetBillsByVendor(GetBillsByVendorParam _GetBillsByVendorParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetBillsByVendor(_GetBillsByVendorParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddCheck(AddCheckParam _AddCheckParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().AddCheck(_AddCheckParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetBankByIDViewModel> AddCheck_GetBankByID(GetBankByIDParam _GetBankByIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_BankAccount().GetBankByID(_GetBankByIDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddCheck_ApplyCredit(ApplyCreditParam _ApplyCreditParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().ApplyCredit(_ApplyCreditParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OpenAPViewModel> AddCheck_GetCreditBillVendor(GetCreditBillVendorParam _GetCreditBillVendorParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetCreditBillVendor(_GetCreditBillVendorParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BankViewModel> AddCheck_GetBankCD(GetBankCDParam _GetBankCDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetBankCD(_GetBankCDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetVendorAcctList> AddCheck_GetVendorAcct(GetVendorAcctParam _GetVendorAcctParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Vendor().GetVendorAcct(_GetVendorAcctParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCheck_updateCheckTemplate(updateCheckTemplateParam _updateCheckTemplateParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().updateCheckTemplate(_updateCheckTemplateParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListAutoSelectPayment AddCheck_AutoSelectPayment(AutoSelectPaymentParam _AutoSelectPaymentParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().AutoSelectPayment(_AutoSelectPaymentParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddCheck_AddBills(AddBillsParam _AddBillsParam, string ConnectionString)
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

        public int AddCheck_AddCheckRecurr(AddCheckRecurrParam _AddCheckRecurrParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().AddCheckRecurr(_AddCheckRecurrParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OpenAPViewModel> AddCheck_GetSelectedOpenAPPJID(GetSelectedOpenAPPJIDParam _GetSelectedOpenAPPJIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetSelectedOpenAPPJID(_GetSelectedOpenAPPJIDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCheck_UpdateWriteCheckOpenAPpayment(UpdateWriteCheckOpenAPpaymentParam _UpdateWriteCheckOpenAPpaymentParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdateWriteCheckOpenAPpayment(_UpdateWriteCheckOpenAPpaymentParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomViewModel> AddCheck_GetCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString)
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

        public List<CustomViewModel> AddCheck_GetCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString)
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


        public List<ChartViewModel> AddCheck_GetChart(GetChartParam _GetChartParam, string ConnectionString)
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

        public List<GeneralViewModel> AddCheck_GetInvDefaultAcct(GetInvDefaultAcctParam _GetInvDefaultAcctParam, string ConnectionString)
        {
            return new BusinessLayer.BL_General().getInvDefaultAcct(_GetInvDefaultAcctParam, ConnectionString);
        }

        public List<CompanyOfficeViewModel> AddCheck_GetCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString)
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

        public bool AddCheck_IsExistCheckNum(IsExistCheckNumParam _IsExistCheckNumParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().IsExistCheckNum(_IsExistCheckNumParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomViewModel> AddCheck_GetCustomField(getCustomFieldParam _getCustomFieldParam, string ConnectionString)
        {
            return new BusinessLayer.BL_General().getCustomField(_getCustomFieldParam, ConnectionString);
        }


        /// <summary>
        /// For WriteCheckRecurr Page : WriteCheckRecurr.aspx / WriteCheckRecurr.aspx.cs
        /// </summary>
        /// 

        public List<GetRecurCDByIDViewModel> AddCheckRecurr_GetRecurCDByID(GetRecurCDByIDParam _GetRecurCDByIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetRecurCDByID(_GetRecurCDByIDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int AddCheckRecurr_UpdateCheckRecurr(UpdateCheckRecurrParam _UpdateCheckRecurrParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().UpdateCheckRecurr(_UpdateCheckRecurrParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetRecurrBillDetailByIDViewModel> AddCheckRecurr_GetRecurrBillDetailByID(GetRecurrBillDetailByIDParam _GetRecurrBillDetailByIDParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetRecurrBillDetailByID(_GetRecurrBillDetailByIDParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AddCheckRecurr_UpdateRecurrBills(UpdateRecurrBillsParam _UpdateRecurrBillsParam, string ConnectionString)
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


        /// <summary>
        /// For EditCheck Page : EditCheck.aspx / EditCheck.aspx.cs
        /// </summary>
        /// API's Naming Conventions :EditCheck_Method Name(Parameters)
        public List<TransactionViewModel> EditCheck_GetTransByBatchType(GetTransByBatchTypeParam _GetTransByBatchTypeParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_JournalEntry().GetTransByBatchType(_GetTransByBatchTypeParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditCheck_UpdateAPCDDate(UpdateAPCDDateParam _UpdateAPCDDateParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_Bills().UpdateAPCDDate(_UpdateAPCDDateParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EditCheck_UpdateNextCheck(UpdateNextCheckParam _UpdateNextCheckParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_BankAccount().UpdateNextCheck(_UpdateNextCheckParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EditCheck_UpdateTransDateByBatch(UpdateTransDateByBatchParam _UpdateTransDateByBatchParam, string ConnectionString)
        {
            try
            {
                new BusinessLayer.BL_JournalEntry().UpdateTransDateByBatch(_UpdateTransDateByBatchParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<LogViewModel> EditCheck_GetAPCheckLogs(GetAPCheckLogsParam _GetAPCheckLogsParam, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().GetAPCheckLogs(_GetAPCheckLogsParam, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EditCheck_IsExistCheckNumOnEdit(IsExistCheckNumOnEditParam _IsExistCheckNumOnEdit, string ConnectionString)
        {
            try
            {
                return new BusinessLayer.BL_Bills().IsExistCheckNumOnEdit(_IsExistCheckNumOnEdit, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
 