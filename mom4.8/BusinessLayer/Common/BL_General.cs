using DataLayer;
using BusinessEntity;
using System.Data;
using BusinessEntity.payroll;
using System.Collections.Generic;
using System;
using BusinessEntity.APModels;
using BusinessEntity.Recurring;

namespace BusinessLayer
{
    public class BL_General
    {
        DL_General objDL_General = new DL_General();

        public void RegisterDevice(General objGeneral)
        {
            objDL_General.RegisterDevice(objGeneral);
        }
        public void RegisterDeviceNew(General objGeneral)
        {
            objDL_General.RegisterDeviceNew(objGeneral);
        }
        public void PingResponse(General objGeneral)
        {
            objDL_General.PingResponse(objGeneral);
        }
        public void PingResponseNew(General objGeneral)
        {
            objDL_General.PingResponseNew(objGeneral);
        }
        public string GetRegID(General objGeneral)
        {
            return objDL_General.GetRegID(objGeneral);
        }

        public int GetUpdateTicketSP(General objGeneral)
        {
            return objDL_General.GetUpdateTicketSP(objGeneral);
        }

        public int GetFunctionSpecialChars(General objGeneral)
        {
            return objDL_General.GetFunctionSpecialChars(objGeneral);
        }

        public void InsetLatLngRole(General objGeneral)
        {
            objDL_General.InsetLatLngRole(objGeneral);
        }

        public void UpdatePDAField(General objGeneral)
        {
            objDL_General.UpdatePDAField(objGeneral);
        }

        public void UpdateCustom(General objGeneral)
        {
            objDL_General.UpdateCustom(objGeneral);
        }

        public object GetLocCredit(int LID, string ConnConfig)
        {
            return objDL_General.GetLocCredit(LID, ConnConfig);
        }

        public DataSet getCustomFields(General objGeneral)
        {
            return objDL_General.getCustomFields(objGeneral);
        }

        //Get All Category List
        public DataSet getAllCategoryList(General objGeneral)
        {
            return objDL_General.getAllCategoryList(objGeneral);
        }

        //Get All Customer(Owner) List
        public DataSet getAllCustomerList(General objGeneral)
        {
            return objDL_General.getAllCustomerList(objGeneral);
        }
        public List<CustomViewModel> getCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString)
        {
            DataSet ds = objDL_General.getCustomFields(_getCustomFieldsParam, ConnectionString);
            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCustomViewModel.Add(
                    new CustomViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Label = Convert.ToString(dr["Label"]),
                        Number = Convert.ToInt32(DBNull.Value.Equals(dr["Number"]) ? 0 : dr["Number"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        GstRate = Convert.ToDouble(DBNull.Value.Equals(dr["GstRate"]) ? 0 : dr["GstRate"]),
                    }
                    );
            }

            return _lstCustomViewModel;
        }
        public string getCode(General objGeneral)
        {
            return objDL_General.getCode(objGeneral);
        }

        public DataSet getDiagnosticCategory(General objGeneral)
        {
            return objDL_General.getDiagnosticCategory(objGeneral);
        }

        //API
        public List<GetDiagnosticCategoryViewModel> getDiagnosticCategory(GetDiagnosticCategoryParam _GetDiagnosticCategory, string ConnectionString)
        {
            DataSet ds = objDL_General.getDiagnosticCategory(_GetDiagnosticCategory, ConnectionString);
            List<GetDiagnosticCategoryViewModel> _lstGetDiagnosticCategory = new List<GetDiagnosticCategoryViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetDiagnosticCategory.Add(
                    new GetDiagnosticCategoryViewModel()
                    {
                        Category = Convert.ToString(dr["Category"]),
                    }
                    );
            }

            return _lstGetDiagnosticCategory;
        }

        public DataSet getDiagnostic(General objGeneral)
        {
            return objDL_General.getDiagnostic(objGeneral);
        }

        public DataSet getDiagnosticAll(General objGeneral)
        {
            return objDL_General.getDiagnosticAll(objGeneral);
        }

        public DataSet getCodesAll(General objGeneral)
        {
            return objDL_General.getCodesAll(objGeneral);
        }

        public DataSet getPing(General objGeneral)
        {
            return objDL_General.getPing(objGeneral);
        }
        public DataSet getPingNew(General objGeneral)
        {
            return objDL_General.getPingNew(objGeneral);
        }

        public void InsertDiagnostic(General objGeneral)
        {
            objDL_General.InsertDiagnostic(objGeneral);
        }

        public void UpdateDiagnostic(General objGeneral)
        {
            objDL_General.UpdateDiagnostic(objGeneral);
        }

        public void InsertQuickCodes(General objGeneral)
        {
            //objDL_General.InsertQuickCodes(objGeneral);
            objDL_General.AddUpdateQuickCode(objGeneral, false);
        }

        public void UpdateQuickCodes(General objGeneral)
        {
            //objDL_General.UpdateQuickCodes(objGeneral);
            objDL_General.AddUpdateQuickCode(objGeneral, true);
        }

        public void DeleteQuickCodes(General objGeneral)
        {
            objDL_General.DeleteQuickCodes(objGeneral);
        }

        public void InsertGPSInterval(General objGeneral)
        {
            objDL_General.InsertGPSInterval(objGeneral);
        }

        public string GetGPSInterval(General objGeneral)
        {
            return objDL_General.GetGPSInterval(objGeneral);
        }
        public string GetGPSIntervalSP(General objGeneral)
        {
            return objDL_General.GetGPSIntervalSP(objGeneral);
        }

        public string GetDeviceTokenID(General objGeneral)
        {
            return objDL_General.GetDeviceTokenID(objGeneral);
        }
        public string GetDeviceTokenbyuser(string username, string ConnConfig)
        {
            return objDL_General.GetDeviceTokenbyuser(username, ConnConfig);
        }
        public DataSet GetPingResponse(string ConnConfig, string fuser, string Randomid)
        {
            return objDL_General.GetPingResponse(ConnConfig, fuser, Randomid);
        }

        public DataSet GetDeletedTickets(string ConnConfig)
        {
            return objDL_General.GetDeletedTickets(ConnConfig);
        }


        public string GetDeviceType(General objGeneral)
        {
            return objDL_General.GetDeviceType(objGeneral);
        }

        public void LogError(General objGeneral)
        {
            objDL_General.LogError(objGeneral);
        }

        public void UpdateQBLastSync(General objGeneral)
        {
            objDL_General.UpdateQBLastSync(objGeneral);
        }

        //API
        public void UpdateQBLastSync(UpdateQBLastSyncParam _UpdateQBLastSync, string ConnectionString)
        {
            objDL_General.UpdateQBLastSync(_UpdateQBLastSync, ConnectionString);
        }

        public void UpdateQBLastSync1(General objGeneral)
        {
            objDL_General.UpdateQBLastSync1(objGeneral);
        }

        public void UpdateSageLastSync(General objGeneral)
        {
            objDL_General.UpdateSageLastSync(objGeneral);
        }

        public void AddQBErrorLog(General objGeneral)
        {
            objDL_General.AddQBErrorLog(objGeneral);
        }

        public DataSet getQBlatsync(General objGeneral)
        {
            return objDL_General.getQBlatsync(objGeneral);
        }

        //API
        public List<QBLastSyncResParam> GetQBlatSync(General objGeneral)
        {
            return objDL_General.GetQBlatSync(objGeneral);
        }
        public DataSet getSagelatsync(General objGeneral)
        {
            return objDL_General.getSagelatsync(objGeneral);
        }

        //api
        public List<GeneralViewModel> getSagelatsync(getConnectionConfigParam objGeneral, string ConnectionString)
        {
            DataSet ds = objDL_General.getSagelatsync(objGeneral, ConnectionString);

            List<GeneralViewModel> _generalViewModel = new List<GeneralViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _generalViewModel.Add(
                    new GeneralViewModel()
                    {
                        SageLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["SageLastSync"]) ? null : dr["SageLastSync"]),
                        sageintegration = Convert.ToInt32(DBNull.Value.Equals(dr["sageintegration"]) ? 0 : dr["sageintegration"]),
                    }
                    );
            }
            return _generalViewModel;
        }

        public DataSet GetMails(General objGeneral)
        {
            return objDL_General.GetMails(objGeneral);
        }

        public int GetMailsCount(General objGeneral)
        {
            return objDL_General.GetMailsCount(objGeneral);
        }

        public int AddEmails(General objGeneral)
        {
            return objDL_General.AddEmails(objGeneral);
        }

        public DataSet GetEmailAcc(General objGeneral)
        {
            return objDL_General.GetEmailAcc(objGeneral);
        }

        public DataSet GetEmailAccounts(General objGeneral)
        {
            return objDL_General.GetEmailAccounts(objGeneral);
        }

        public int GetMAXEmailUID(General objGeneral)
        {
            return objDL_General.GetMAXEmailUID(objGeneral);
        }

        public DataSet GetMsgUID(General objGeneral)
        {
            return objDL_General.GetMsgUID(objGeneral);
        }

        public DataSet getCRMEmails(General objGeneral)
        {
            return objDL_General.getCRMEmails(objGeneral);
        }

        public DataSet ExecQuery(General objGeneral)
        {
            return objDL_General.ExecQuery(objGeneral);
        }
        public DataSet getCustomFieldsControl(General objPropGeneral)
        {
            return objDL_General.getCustomFieldsControl(objPropGeneral);
        }

        public DataSet getCustomField(General objPropGeneral, string fieldName)
        {
            return objDL_General.getCustomField(objPropGeneral, fieldName);
        }

        public List<CustomViewModel> getCustomField(getCustomFieldParam _getCustomFieldParam, string ConnectionString)
        {
            DataSet ds = objDL_General.getCustomField(_getCustomFieldParam, ConnectionString);

            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCustomViewModel.Add(
                    new CustomViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Label = Convert.ToString(dr["Label"]),
                        Number = Convert.ToInt32(DBNull.Value.Equals(dr["Number"]) ? 0 : dr["Number"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        //GstRate = Convert.ToDouble(DBNull.Value.Equals(dr["GstRate"]) ? 0 : dr["GstRate"]),
                    }
                    );
            }

            return _lstCustomViewModel;
        }

        public List<CustomViewModel> getCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString)
        {
            DataSet ds = objDL_General.getCustomFieldsControl(_getCustomFieldsControlParam, ConnectionString);
            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCustomViewModel.Add(
                    new CustomViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Label = Convert.ToString(dr["Label"]),
                        Number = Convert.ToInt32(DBNull.Value.Equals(dr["Number"]) ? 0 : dr["Number"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        //GstRate = Convert.ToDouble(DBNull.Value.Equals(dr["GstRate"]) ? 0 : dr["GstRate"]),
                    }
                    );
            }

            return _lstCustomViewModel;
        }

        public DataSet getInvDefaultAcct(General objPropGeneral)
        {
            return objDL_General.getInvDefaultAcct(objPropGeneral);
        }
        public List<GeneralViewModel> getInvDefaultAcct(GetInvDefaultAcctParam __GetInvDefaultAcctParam, string ConnectionString)
        {
            DataSet ds = objDL_General.getInvDefaultAcct(__GetInvDefaultAcctParam, ConnectionString);
            List<GeneralViewModel> _generalViewModel = new List<GeneralViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _generalViewModel.Add(
                    new GeneralViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        //SageLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["SageLastSync"]) ? 0 : dr["SageLastSync"]),
                        //sageintegration = Convert.ToInt32(DBNull.Value.Equals(dr["sageintegration"]) ? 0 : dr["sageintegration"]),
                    }
                    );
            }
            return _generalViewModel;
        }

        public DataSet GetInventoryByTypeName(General objPropGeneral)
        {
            return objDL_General.GetInventoryByTypeName(objPropGeneral);
        }
        public DataSet getCustomFieldsControlBranch(General objPropGeneral)
        {
            return objDL_General.getCustomFieldsControlBranch(objPropGeneral);
        }

        public void UpdateDiagnosticOrder(General objGeneral)
        {
            objDL_General.UpdateDiagnosticOrder(objGeneral);
        }

        public DataSet getCompanyCountry(General objPropGeneral)
        {
            return objDL_General.getCompanyCountry(objPropGeneral);
        }

        public void UpdateCustomFields(General objGeneral)
        {
            objDL_General.UpdateCustomFields(objGeneral);
        }

        public DataSet GetScreenCustomFields(General objGeneral)
        {
            return objDL_General.GetScreenCustomFields(objGeneral);
        }

        public DataSet GetStageItemsById(General objGeneral)
        {
            return objDL_General.GetStageItemsById(objGeneral);
        }
        public void UpdateCustomPRLast(string connConfig, string startDate, string endDate)
        {
            objDL_General.UpdateCustomPRLast(connConfig, startDate, endDate);
        }

        public void UpdateSalesApproveEstimate(string connConfig, bool isApprove)
        {
            objDL_General.UpdateSalesApproveEstimate(connConfig, isApprove);
        }

        public bool GetSalesApproveEstimate(string connConfig)
        {
            return objDL_General.GetSalesApproveEstimate(connConfig);
        }

        public string GetEmailTemplate(EmailTemplate emailTemplate)
        {
            return objDL_General.GetEmailTemplate(emailTemplate);
        }
    }
}
