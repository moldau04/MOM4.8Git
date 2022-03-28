using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_Chart
    {
        DL_Chart _objDLChart = new DL_Chart();
        public void AddChart(Chart objChart, Bank objBank)
        {
            _objDLChart.AddChart(objChart, objBank);    
        }

        //public DataSet getCOA(Chart objChart)
        //{
        //    return objDL_Chart.GetAllCOA(objChart);
        //}
        public void UpdateChart(Chart objChart, Bank objBank)
        {
            _objDLChart.UpdateChart(objChart, objBank);
        }
        public DataSet GetChart(Chart objChart)
        {
            return _objDLChart.GetChart(objChart);
        }
        public DataSet GetChartDetail(Chart objChart)
        {
            return _objDLChart.GetChartDetail(objChart);
        }

        public Int32 GetCharID(Chart objChart)
        {
            return _objDLChart.GetCharID(objChart);
        }

        public List<ChartViewModel> GetChart(GetChartParam _GetChartParam, string ConnectionString)
        {
            DataSet ds = _objDLChart.GetChart( _GetChartParam,  ConnectionString);
            List<ChartViewModel> _lstChartViewModel = new List<ChartViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstChartViewModel.Add(
                    new ChartViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Department = Convert.ToInt32(DBNull.Value.Equals(dr["Department"]) ? 0 : dr["Department"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Sub = Convert.ToString(dr["Sub"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Control = Convert.ToInt32(DBNull.Value.Equals(dr["Control"]) ? 0 : dr["Control"]),
                        InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        Detail = Convert.ToInt32(DBNull.Value.Equals(dr["Detail"]) ? 0 : dr["Detail"]),
                        CAlias = Convert.ToString(dr["CAlias"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Sub2 = Convert.ToString(dr["Sub2"]),
                        Branch = Convert.ToInt32(DBNull.Value.Equals(dr["Branch"]) ? 0 : dr["Branch"]),
                        CostCenter = Convert.ToInt32(DBNull.Value.Equals(dr["CostCenter"]) ? 0 : dr["CostCenter"]),
                        QBAccountID = Convert.ToString(dr["Department"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        AcctRoot = Convert.ToString(dr["Balance"]),
                        DAT = Convert.ToInt32(DBNull.Value.Equals(dr["DAT"]) ? 0 : dr["DAT"]),
                        company = Convert.ToString(dr["Balance"]),
                        DefaultNo = Convert.ToString(dr["DefaultNo"]),
                    }
                    );
            }

            return _lstChartViewModel;
        }
        public void DeleteChart(Chart objChart)
        {
            _objDLChart.DeleteChart(objChart);
        }
        public DataSet GetAll(Chart objChart)
        {
            return _objDLChart.GetAllCOA(objChart);
        }
        
            public DataSet GetAllChartByAsset(Chart objChart)
        {
            return _objDLChart.GetAllChartByAsset(objChart);
        }
        public DataSet GetAllStatus(Chart objChart)
        {
            return _objDLChart.GetStatus(objChart);
        }
        public bool IsExistAcct(Chart objChart)
        {
            return _objDLChart.IsExistAcct(objChart);
        }
        public bool IsExistAcctForEdit(Chart objChart)
        {
            return _objDLChart.IsExistAcctForEdit(objChart);
        }
        public bool IsExistAcctNameForEdit(Chart objChart)
        {
            return _objDLChart.IsExistAcctNameForEdit(objChart);
        }

        public bool IsExistAcctORBANKNameForEdit(Chart objChart)
        {
            return _objDLChart.IsExistAcctORBANKNameForEdit(objChart);
        } 
        
        public bool IsExistAcctNameExists(Chart objChart)
        {
            return _objDLChart.IsExistAcctNameExists(objChart);
        }
        public DataSet GetAutoFillAccount(Chart objChart)
        {
            return _objDLChart.GetAutoFillAccount(objChart);
        }
        public DataSet GetAutoFillAccountJE(Chart objChart)
        {
            return _objDLChart.GetAutoFillAccountJE(objChart);
        }
        public DataSet spGetAccountSearchAP(Chart objChart)
        {
            return _objDLChart.spGetAccountSearchAP(objChart);        }
        
        public DataSet GetAccountData(Chart objChart)
        {
            return _objDLChart.GetAccountData(objChart);
        }
        public DataSet GetUndepositeAcct(Chart objChart)
        {
            return _objDLChart.GetUndepositeAcct(objChart);
        }

        //API
        public List<GetUndepositeAcctViewModel> GetUndepositeAcct(GetUndepositeAcctParam _GetUndepositeAcct, string ConnectionString)
        {
            DataSet ds = _objDLChart.GetUndepositeAcct(_GetUndepositeAcct, ConnectionString);

            List<GetUndepositeAcctViewModel> _lstGetUndepositeAcct = new List<GetUndepositeAcctViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetUndepositeAcct.Add(
                    new GetUndepositeAcctViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Department = Convert.ToInt32(DBNull.Value.Equals(dr["Department"]) ? 0 : dr["Department"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Sub = Convert.ToString(dr["Sub"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Control = Convert.ToInt16(DBNull.Value.Equals(dr["Control"]) ? 0 : dr["Control"]),
                        InUse = Convert.ToInt16(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        Detail = Convert.ToInt16(DBNull.Value.Equals(dr["Detail"]) ? 0 : dr["Detail"]),
                        CAlias = Convert.ToString(dr["CAlias"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Sub2 = Convert.ToString(dr["Sub2"]),
                        DAT = Convert.ToInt16(DBNull.Value.Equals(dr["DAT"]) ? 0 : dr["DAT"]),
                        Branch = Convert.ToInt16(DBNull.Value.Equals(dr["Branch"]) ? 0 : dr["Branch"]),
                        CostCenter = Convert.ToInt16(DBNull.Value.Equals(dr["CostCenter"]) ? 0 : dr["CostCenter"]),
                        AcctRoot = Convert.ToString(dr["AcctRoot"]),
                        QBAccountID = Convert.ToString(dr["QBAccountID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        DefaultNo = Convert.ToString(dr["DefaultNo"]),
                        TimeStamp = Convert.ToString(dr["TimeStamp"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                    }
                    );
            }

            return _lstGetUndepositeAcct;
        }
        public DataSet GetAcctReceivable(Chart objChart)
        {
            return _objDLChart.GetAcctReceivable(objChart);
        }
        public DataSet GetBankAcct(Chart objChart)
        {
            return _objDLChart.GetBankAcct(objChart);
        }
        public DataSet GetSalesTaxAcct(Chart objChart)
        {
            return _objDLChart.GetSalesTaxAcct(objChart);
        }
        public DataSet GetAcctPayable(Chart objChart)
        {
            return _objDLChart.GetAcctPayable(objChart);
        }
        public List<ChartViewModel> GetAcctPayable(GetAcctPayableParam objGetAcctPayableParam, string ConnectionString)
        {
            DataSet ds = _objDLChart.GetAcctPayable(objGetAcctPayableParam, ConnectionString);

            List<ChartViewModel> _lstChartViewModel = new List<ChartViewModel>();
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstChartViewModel.Add(
                    new ChartViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 :dr["ID"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Department = Convert.ToInt32(DBNull.Value.Equals(dr["Department"]) ? 0 : dr["Department"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Sub = Convert.ToString(dr["Sub"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Control = Convert.ToInt32(DBNull.Value.Equals(dr["Control"]) ? 0 : dr["Control"]),
                        InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        Detail = Convert.ToInt32(DBNull.Value.Equals(dr["Detail"]) ? 0 : dr["Detail"]),
                        CAlias = Convert.ToString(dr["CAlias"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Sub2 = Convert.ToString(dr["Sub2"]),
                        Branch = Convert.ToInt32(DBNull.Value.Equals(dr["Branch"]) ? 0 : dr["Branch"]),
                        CostCenter = Convert.ToInt32(DBNull.Value.Equals(dr["CostCenter"]) ? 0 : dr["CostCenter"]),
                        QBAccountID = Convert.ToString(dr["QBAccountID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        AcctRoot = Convert.ToString(dr["AcctRoot"]),
                        DAT = Convert.ToInt32(DBNull.Value.Equals(dr["DAT"]) ? 0 : dr["DAT"]),
                        DefaultNo = Convert.ToString(dr["DefaultNo"]),
                    }
                    );
            }

            return _lstChartViewModel;
        }
        public void UpdateChartBalance(Chart objChart)
        {
            _objDLChart.UpdateChartBalance(objChart);
        }

        public void UpdateChartBalance(UpdateChartBalanceParam objUpdateChartBalanceParam,string ConnectionString)
        {
            _objDLChart.UpdateChartBalance(objUpdateChartBalanceParam, ConnectionString);
        }
        public DataSet GetChartByID(Chart objChart) //By Viral
        {
            return _objDLChart.GetChartByID(objChart);
        }
        public DataSet GetStock(Chart objChart)
        {
            return _objDLChart.GetStock(objChart);
        }
        public DataSet GetCurrentEarn(Chart objChart)
        {
            return _objDLChart.GetCurrentEarn(objChart);
        }
        public DataSet GetDistribution(Chart objChart)
        {
            return _objDLChart.GetDistribution(objChart);
        }
        public DataSet GetRetainedEarn(Chart objChart)
        {
            return _objDLChart.GetRetainedEarn(objChart);
        }
        public DataSet GetAllChartByDate(Chart objChart)
        {
            return _objDLChart.GetAllChartByDate(objChart);
        }
        public DataSet GetMinTransDate(Chart objChart)
        {
            return _objDLChart.GetMinTransDate(objChart);

        }
        public double GetSumOfRevenueByDate(Chart objChart)
        {
            return _objDLChart.GetSumOfRevenueByDate(objChart);
        }
        public double GetSumOfCostSalesByDate(Chart objChart)
        {
            return _objDLChart.GetSumOfCostSalesByDate(objChart);
        }
        public double GetSumOfExpenseByDate(Chart objChart)
        {
            return _objDLChart.GetSumOfExpenseByDate(objChart);
        }
        public bool IsChartBankAcct(Chart objChart)
        {
            return _objDLChart.IsChartBankAcct(objChart);
        }
        public void UpdateBankBalance(Chart objChart)
        {
            _objDLChart.UpdateBankBalance(objChart);
        }
        public DataSet GetAccountLedger(Chart objChart)
        {
            return _objDLChart.GetAccountLedger(objChart);
        }
        public void CalChartBalance(Chart objChart)
        {
            _objDLChart.CalChartBalance(objChart);
        }
        public DataSet GetBankCharge(Chart objChart)
        {
            return _objDLChart.GetBankCharge(objChart);
        }
        public DataSet GetPOAcct(Chart objChart)
        {
            return _objDLChart.GetPOAcct(objChart);
        }
        public DataSet GetChartByAcct(Chart objChart)
        {
            return _objDLChart.GetChartByAcct(objChart);
        }
        public double GetSumOfRevenueByAsOfDate(Chart objChart)
        {
            return _objDLChart.GetSumOfRevenueByAsOfDate(objChart);
        }
        public double GetSumOfCostSalesByAsOfDate(Chart objChart)
        {
            return _objDLChart.GetSumOfCostSalesByAsOfDate(objChart);
        }
        public double GetSumOfExpenseByAsOfDate(Chart objChart)
        {
            return _objDLChart.GetSumOfExpenseByAsOfDate(objChart);
        }
        public DataSet GetGeneralLedger(Chart _objChart)
        {
            return _objDLChart.GetGeneralLedger(_objChart);
        }
        public DataSet GetAllAccountDetails(Chart _objChart)
        {
            return _objDLChart.GetAllAccountDetails(_objChart);
        }
        public DataSet GetGeneralLedgerByDate(Chart _objChart) //To get Revenue Total details
        {
            return _objDLChart.GetGeneralLedgerByDate(_objChart);
        }
        public Int32 GetBankAcctID(Chart objChart)
        {
            return _objDLChart.GetBankAcctID(objChart);
        }

        public Int32 GetBankAcctID(GetBankAcctIDParam objGetBankAcctIDParam, string ConnectionString)
        {
            int BankGL = _objDLChart.GetBankAcctID(objGetBankAcctIDParam, ConnectionString);
            return BankGL;
        }

        public DataSet GetAccountLedgerPaging(Chart objChart)
        {
            return _objDLChart.GetAccountLedgerPaging(objChart);
        }
    }
}
