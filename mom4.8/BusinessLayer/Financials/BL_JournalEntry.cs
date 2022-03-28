using BusinessEntity;
using BusinessEntity.APModels;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_JournalEntry
    {
        DL_JournalEntry _objDLJournal = new DL_JournalEntry();
        public void DeleteGLA(Journal objJournal)
        {
            _objDLJournal.DeleteGLA(objJournal);
        }

        public DataSet GetYearEndClosedOutData(Journal objJournal)
        {
            return _objDLJournal.GetYearEndClosedOutData(objJournal);
        }

        //public void DeleteTrans(Journal objJournal)
        //{
        //    _objDLJournal.DeleteTrans(objJournal);
        //}
        //public void DeleteTransByID(Transaction objTrans)
        //{
        //    _objDLJournal.DeleteTransByID(objTrans);
        //}
        public DataSet GetDataByRef(Journal objJournal)
        {
            return _objDLJournal.GetDataByRef(objJournal);
        }
        public DataSet GetDataByBatch(Journal objJournal)
        {
            return _objDLJournal.GetDataByBatch(objJournal);
        }
        public int GetMaxTransID(Journal objJournal)
        {
            return _objDLJournal.GetMaxTransID(objJournal);
        }
        public int GetMaxTransRef(Journal objJournal)
        {
            return _objDLJournal.GetMaxTransRef(objJournal);
        }
        public int GetMaxTransRef(GetMaxTransRefParam objGetMaxTransRefParam, string ConnectionString)
        {
            int _refGL = _objDLJournal.GetMaxTransRef(objGetMaxTransRefParam, ConnectionString);
            return _refGL;
        }
        public int GetMaxTransBatch(Journal objJournal)
        {
            return _objDLJournal.GetMaxTransBatch(objJournal);
        }

        public int GetMaxTransBatch(GetMaxTransBatchParam objGetMaxTransBatchParam, string ConnectionString)
        {
            int _batch = _objDLJournal.GetMaxTransBatch(objGetMaxTransBatchParam, ConnectionString);
            return _batch;
        }
        public void AddGLA(Journal objJournal)
        {
            _objDLJournal.AddGLA(objJournal);
        }
        public void AddGLA(AddGLAParam objAddGLAParam,string ConnectionString)
        {
            _objDLJournal.AddGLA(objAddGLAParam, ConnectionString);
        }
        public int AddJournalTrans(Transaction objTrans)
        {
            return _objDLJournal.AddJournalTrans(objTrans);
        }
        public int AddJournalTrans(AddJournalTransParam objAddJournalTransParam,string ConnectionString)
        {
            int _transID = _objDLJournal.AddJournalTrans(objAddJournalTransParam, ConnectionString);
            return _transID;
        }
        //public DataSet GetDataByBatchRef(Transaction objTrans)
        //{
        //    return _objDLJournal.GetDataByBatchRef(objTrans);
        //}
        public void UpdateGLA(Journal objJournal)
        {
            _objDLJournal.UpdateGLA(objJournal);
        }
        public void UpdateJournalTrans(Transaction objTrans)
        {
            _objDLJournal.UpdateJournalTrans(objTrans);
        }
        //public DataSet GetAllJE(Journal objJournal)
        //{
        //    return _objDLJournal.GetAllJE(objJournal);
        //}
        public DataSet GetJobsLoc(Transaction objTrans)
        {
            return _objDLJournal.GetJobsLoc(objTrans);
        }
        public DataSet GetLocByJobID(Transaction objTrans)
        {
            return _objDLJournal.GetLocByJobID(objTrans);
        }
        public DataSet GetAllJEByDate(Journal objJournal)
        {
            return _objDLJournal.GetAllJEByDate(objJournal);
        }
        public DataSet GetJobDetailByID(Transaction objTrans)
        {
            return _objDLJournal.GetJobDetailByID(objTrans);
        }
        public DataSet GetPhaseByID(Transaction objTrans)
        {
            return _objDLJournal.GetPhaseByID(objTrans);
        }
        public void UpdateJournalTransAmount(Transaction objTrans)
        {
            _objDLJournal.UpdateJournalTransAmount(objTrans);
        }
        public DataSet GetPaymentTransByBatchRef(Transaction objTrans)
        {
            return _objDLJournal.GetPaymentTransByBatchRef(objTrans);
        }
        public DataSet GetTransByID(Transaction objTrans)
        {
            return _objDLJournal.GetTransByID(objTrans);
        }
        public List<TransactionViewModel> GetTransByID(GetTransByIDParam objGetTransByIDParam, string ConnectionString)
        {
            DataSet ds = _objDLJournal.GetTransByID(objGetTransByIDParam, ConnectionString);

            List<TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTransactionViewModel.Add(
                    new TransactionViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        BatchID = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Line = Convert.ToInt32(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        AcctSub = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
                        Status = Convert.ToString(dr["Status"]),
                        Sel = Convert.ToInt16(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        VInt = Convert.ToInt32(DBNull.Value.Equals(dr["VInt"]) ? 0 : dr["VInt"]),
                        VDoub = Convert.ToInt32(DBNull.Value.Equals(dr["VDoub"]) ? 0 : dr["VDoub"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        strRef = Convert.ToString(dr["strRef"]),
                    }
                    );
            }

            return _lstTransactionViewModel;
        }
        public void UpdateInvoiceTransDetails(Transaction _objTrans)
        {
            _objDLJournal.UpdateInvoiceTransDetails(_objTrans);
        }
        public void UpdateBillTrans(Transaction _objTrans)
        {
            _objDLJournal.UpdateBillTrans(_objTrans);
        }
        //public DataSet GetOpenTrans(Transaction _objOpenTrans)
        //{
        //    return _objDLJournal.GetOpenTrans(_objOpenTrans);
        //}
        public void AddTransBankAdj(TransBankAdj _objTrans)
        {
            _objDLJournal.AddTransBankAdj(_objTrans);
        }
        public void UpdateTransCheckRecon(TransBankAdj _objTrans)
        {
            _objDLJournal.UpdateTransCheckRecon(_objTrans);
        }
        public void UpdateTransDepositRecon(TransBankAdj _objTrans)
        {
            _objDLJournal.UpdateTransDepositRecon(_objTrans);
        }
        public void UpdateClearItem(Transaction _objTrans)
        {
            _objDLJournal.UpdateClearItem(_objTrans);
        }
        public DataSet GetTransByBatchRef(Transaction _objTrans)
        {
            return _objDLJournal.GetTransByBatchRef(_objTrans);
        }
        public DataSet GetTransByBatch(Transaction _objTrans)
        {
            return _objDLJournal.GetTransByBatch(_objTrans);
        }
        public List<TransactionViewModel> GetTransByBatch(GetTransByBatchParam _objGetTransByBatchParam, string ConnectionString)
        {
            DataSet ds = _objDLJournal.GetTransByBatch(_objGetTransByBatchParam, ConnectionString);

            List<TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTransactionViewModel.Add(
                    new TransactionViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        AcctSub = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        BatchID = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Line = Convert.ToInt32(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Sel = Convert.ToInt16(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        Status = Convert.ToString(dr["Status"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        VDoub = Convert.ToDouble(DBNull.Value.Equals(dr["VDoub"]) ? null : dr["VDoub"]),
                        VInt = Convert.ToInt32(DBNull.Value.Equals(dr["VInt"]) ? 0 : dr["VInt"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? 0 : dr["fDate"]),
                        strRef = Convert.ToString(dr["strRef"]),
                        AcctNo = Convert.ToString(dr["AcctNo"]),
                        AcctName = Convert.ToString(dr["AcctName"]),
                    }
                    );
            }

            return _lstTransactionViewModel;
        }
        public DataSet GetBillAPTransByBatch(Transaction _objTrans)
        {
            return _objDLJournal.GetBillAPTransByBatch(_objTrans);
        }
        public void UpdateTransDateByBatch(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransDateByBatch(_objTrans);
        }
        public void UpdateTransDateByBatch(UpdateTransDateByBatchParam _UpdateTransDateByBatchParam, string ConnectionString)
        {
            _objDLJournal.UpdateTransDateByBatch( _UpdateTransDateByBatchParam,  ConnectionString);
        }
        public void UpdateTransVoidCheck(Transaction _objTransaction)
        {
            _objDLJournal.UpdateTransVoidCheck(_objTransaction);
        }
        public void UpdateTransVoidCheck(UpdateTransVoidCheckParam _objUpdateTransVoidCheckParam, string ConnectionString)
        {
            _objDLJournal.UpdateTransVoidCheck(_objUpdateTransVoidCheckParam, ConnectionString);
        }
        public void UpdateTransVoidCheckOpen(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransVoidCheckOpen(_objTrans);
        }

        public void UpdateTransVoidCheckOpen(UpdateTransVoidCheckOpenParam _objUpdateTransVoidCheckOpenParam,string ConnectionString)
        {
            _objDLJournal.UpdateTransVoidCheckOpen(_objUpdateTransVoidCheckOpenParam, ConnectionString);
        }


        public void UpdateTransVoidCheckByBatch(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransVoidCheckByBatch(_objTrans);
        }
        public void UpdateTransVoidCheckByBatch(UpdateTransVoidCheckByBatchParam _objUpdateTransVoidCheckByBatchParam, string ConnectionString)
        {
            _objDLJournal.UpdateTransVoidCheckByBatch(_objUpdateTransVoidCheckByBatchParam, ConnectionString);
        }
        public void UpdateTransVoidCheckByBatchOpen(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransVoidCheckByBatchOpen(_objTrans);
        }
        public void UpdateTransVoidCheckByBatchOpen(UpdateTransVoidCheckByBatchOpenParam _objUpdateTransVoidCheckByBatchOpenParam,string ConnectionString)
        {
            _objDLJournal.UpdateTransVoidCheckByBatchOpen(_objUpdateTransVoidCheckByBatchOpenParam, ConnectionString);
        }

        public DataSet GetTransByBatchType(Transaction _objTrans)
        {
            return _objDLJournal.GetTransByBatchType(_objTrans);
        }
        public List<TransactionViewModel> GetTransByBatchType(GetTransByBatchTypeParam _GetTransByBatchTypeParam, string ConnectionString)
        {
            DataSet ds = _objDLJournal.GetTransByBatchType(_GetTransByBatchTypeParam, ConnectionString);

            List<TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTransactionViewModel.Add(
                    new TransactionViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        BatchID = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Line = Convert.ToInt32(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        AcctSub = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
                        Status = Convert.ToString(dr["Status"]),
                        Sel = Convert.ToInt16(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        VInt = Convert.ToInt32(DBNull.Value.Equals(dr["VInt"]) ? 0 : dr["VInt"]),
                        VDoub = Convert.ToInt32(DBNull.Value.Equals(dr["VDoub"]) ? 0 : dr["VDoub"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        strRef = Convert.ToString(dr["strRef"]),
                    }
                    );
            }

            return _lstTransactionViewModel;
        }
        public bool ValidateByTimeStamp(Transaction _objTrans)
        {
            return _objDLJournal.ValidateByTimeStamp(_objTrans);
        }
        public void DeleteBillTrans(Transaction _objTrans)
        {
            _objDLJournal.DeleteBillTrans(_objTrans);
        }
        public void DeleteTransDeposit(Transaction _objTrans)
        {
            _objDLJournal.DeleteTransDeposit(_objTrans);
        }
        public void DeleteTransChecks(Transaction _objTrans)
        {
            _objDLJournal.DeleteTransChecks(_objTrans);
        }
        public void UpdateTransCheckNoByBatch(Transaction _objTrans)
        {
            _objDLJournal.UpdateTransCheckNoByBatch(_objTrans);
        }
        public void UpdateTransCheckNoByBatch(UpdateTransCheckNoByBatchParam _UpdateTransCheckNoByBatchParam, string ConnectionString)
        {
            _objDLJournal.UpdateTransCheckNoByBatch(_UpdateTransCheckNoByBatchParam, ConnectionString);
        }
        public DataSet GetPhaseByJobId(Transaction objTrans)
        {
            return _objDLJournal.GetPhaseByJobId(objTrans);
        }
        //public DataSet GetPhaseExpByJobType(Transaction objTrans)
        //{
        //    return _objDLJournal.GetPhaseExpByJobType(objTrans);
        //}
        public DataSet GetTransDataByBatch(Transaction objTrans)
        {
            return _objDLJournal.GetTransDataByBatch(objTrans);
        }
        public DataSet GetAllPhaseByJobID(Transaction objTrans)
        {
            return _objDLJournal.GetAllPhaseByJobID(objTrans);
        }
        public int ProcessRecurJE(Journal objJournal)
        {
            return _objDLJournal.ProcessRecurJE(objJournal);
        }
        public int AddJE(Journal objJournal)
        {
            return _objDLJournal.AddJE(objJournal);
        }
        public int UpdateJE(Journal objJournal)
        {
            return _objDLJournal.UpdateJE(objJournal);
        }

        public DataSet GetJournalEntryItemData(Journal objJournal)
        {
            return _objDLJournal.GetJournalEntryItemData(objJournal);
        }

        public DataSet GetJournalEntryDetailed(Journal objJournal, List<RetainFilter> filters)
        {
            return _objDLJournal.GetJournalEntryDetailed(objJournal, filters);
        }

        public DataSet GetJournalEntryByEntryNo(Journal objJournal)
        {
            return _objDLJournal.GetJournalEntryByEntryNo(objJournal);
        }
    }
}
