using BusinessEntity;
using BusinessEntity.CustomersModel;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_Deposit
    {
        DL_Deposit objDL_Deposit = new DL_Deposit();
        public DataSet GetAllInvoices(Contracts objPropContracts)
        {
            return objDL_Deposit.GetAllInvoices(objPropContracts);
        }
        public DataSet GetInvoiceByLocID(Contracts objPropContracts)
        {
            return objDL_Deposit.GetInvoiceByLocID(objPropContracts);
        }
        public DataSet GetInvoiceByRef(Invoices objInvoice)
        {
            return objDL_Deposit.GetInvoiceByRef(objInvoice);
        }
        public void UpdateInvoice(Invoices objInv)
        {
            objDL_Deposit.UpdateInvoice(objInv);
        }
        //public int AddReceivedPayment(ReceivedPayment _objReceiPmt)
        //{
        //    return objDL_Deposit.AddReceivedPayment(_objReceiPmt);
        //}
        public void AddPaymentDetails(PaymentDetails _objPayment)
        {
            objDL_Deposit.AddPaymentDetails(_objPayment);
        }
        public DataSet GetTransByInvoiceID(Transaction _objTrans)
        {
            return objDL_Deposit.GetTransByInvoiceID(_objTrans);
        }
        public DataSet GetAllReceivePayment(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetAllReceivePayment(_objReceiPmt);
        }
        public DataSet GetReceivePaymentByID(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetReceivePaymentByID(_objReceiPmt);
        }

        //API
        public List<GetReceivePaymentByIDViewModel> GetReceivePaymentByID(GetReceivePaymentByIDParam _GetReceivePaymentByID, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetReceivePaymentByID(_GetReceivePaymentByID, ConnectionString);

            List<GetReceivePaymentByIDViewModel> _lstGetReceivePaymentByID = new List<GetReceivePaymentByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetReceivePaymentByID.Add(
                    new GetReceivePaymentByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        RolName = Convert.ToString(dr["RolName"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        PaymentReceivedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PaymentReceivedDate"]) ? null : dr["PaymentReceivedDate"]),
                        PaymentMethod = Convert.ToInt16(DBNull.Value.Equals(dr["PaymentMethod"]) ? 0 : dr["PaymentMethod"]),
                        CheckNumber = Convert.ToString(dr["CheckNumber"]),
                        AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        DepID = Convert.ToInt32(DBNull.Value.Equals(dr["DepID"]) ? 0 : dr["DepID"]),
                    }
                    );
            }

            return _lstGetReceivePaymentByID;
        }
        public DataSet GetReceivePaymentLogs(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetReceivePaymentLogs(_objReceiPmt);
        }

        //API
        public List<GetLocationLogViewModel> GetReceivePaymentLogs(GetReceivePaymentLogsParam _GetReceivePaymentLogs, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetReceivePaymentLogs(_GetReceivePaymentLogs, ConnectionString);

            List<GetLocationLogViewModel> _lstGetLocationLog = new List<GetLocationLogViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationLog.Add(
                    new GetLocationLogViewModel()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }

            return _lstGetLocationLog;
        }
        public DataSet GetPaymentByReceivedID(PaymentDetails _objPayment)
        {
            return objDL_Deposit.GetPaymentByReceivedID(_objPayment);
        }
        public DataSet GetInvoiceByID(Contracts objPropContracts)
        {
            return objDL_Deposit.GetInvoiceByID(objPropContracts);
        }
        public DataSet GetReceivePaymentDetailsByID(ReceivedPayment _objReceiPmt)
        {
            return objDL_Deposit.GetReceivePaymentDetailsByID(_objReceiPmt);
        }
        public int AddDeposit(Dep _objDep)
        {
            return objDL_Deposit.AddDeposit(_objDep);
        }
        public void UpdateDeposit(Dep _objDep)
        {
            objDL_Deposit.UpdateDeposit(_objDep);
        }

        //API
        public void UpdateDeposit(DepositInfor_UpdateDepositParam _DepositInfor_UpdateDeposit, string ConnectionString)
        {
            objDL_Deposit.UpdateDeposit(_DepositInfor_UpdateDeposit, ConnectionString);
        }


        public void AddDepositDetails(DepositDetails _objDepDetails)
        {
            objDL_Deposit.AddDepositDetails(_objDepDetails);
        }
        public DataSet GetAllDeposits(Dep _objDep)
        {
            return objDL_Deposit.GetAllDeposits(_objDep);
        }

        //API
        public List<GetAllDepositsViewModel> GetAllDeposits(GetAllDepositsParam _GetAllDeposits, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetAllDeposits(_GetAllDeposits, ConnectionString);

            List<GetAllDepositsViewModel> _lstGetAllDeposits = new List<GetAllDepositsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllDeposits.Add(
                    new GetAllDepositsViewModel()
                    {
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Bank = Convert.ToInt32(DBNull.Value.Equals(dr["Bank"]) ? 0 : dr["Bank"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        IsRecon = Convert.ToInt16(DBNull.Value.Equals(dr["IsRecon"]) ? 0 : dr["IsRecon"]),
                        BankName = Convert.ToString(dr["fDesc"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Company = Convert.ToString(dr["Company"]),
                        Status = Convert.ToString(dr["Status"]),
                    }
                    );
            }

            return _lstGetAllDeposits;
        }
        public DataSet GetDepByID(Dep _objDep)
        {
            return objDL_Deposit.GetDepByID(_objDep);
        }

        //API
        public List<GetDepByIDViewModel> GetDepByID(GetDepByIDParam _GetDepByID, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetDepByID(_GetDepByID, ConnectionString);

            List<GetDepByIDViewModel> _lstGetDepByID = new List<GetDepByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetDepByID.Add(
                    new GetDepByIDViewModel()
                    {
                         Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                         fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                         Bank = Convert.ToInt32(DBNull.Value.Equals(dr["Bank"]) ? 0 : dr["Bank"]),
                         fDesc = Convert.ToString(dr["fDesc"]),
                         Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                         TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                         EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                         IsRecon = Convert.ToInt16(DBNull.Value.Equals(dr["IsRecon"]) ? 0 : dr["IsRecon"]),
                    }
                    );
            }

            return _lstGetDepByID;
        }
        public DataSet GetDepHeadByID(Dep _objDep)
        {
            return objDL_Deposit.GetDepHeadByID(_objDep);
        }

        //API
        public List<GetDepHeadByIDViewModel> GetDepHeadByID(GetDepHeadByIDParam _GetDepHeadByID, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetDepHeadByID(_GetDepHeadByID, ConnectionString);

            List<GetDepHeadByIDViewModel> _lstGetDepHeadByID = new List<GetDepHeadByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetDepHeadByID.Add(
                    new GetDepHeadByIDViewModel()
                    {
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        BankName = Convert.ToString(dr["BankName"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        IsRecon = Convert.ToInt16(DBNull.Value.Equals(dr["IsRecon"]) ? 0 : dr["IsRecon"]),
                    }
                    );
            }

            return _lstGetDepHeadByID;
        }
        public DataSet GetReceivedPaymentByDep(ReceivedPayment objReceivePay)
        {
            return objDL_Deposit.GetReceivedPaymentByDep(objReceivePay);
        }

        //API
        public ListGetReceivedPaymentByDep GetReceivedPaymentByDep(GetReceivedPaymentByDepParam _GetReceivedPaymentByDep, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetReceivedPaymentByDep(_GetReceivedPaymentByDep, ConnectionString);

            ListGetReceivedPaymentByDep _ds = new ListGetReceivedPaymentByDep();
            List<GetReceivedPaymentByDepTable1> _lstTable1 = new List<GetReceivedPaymentByDepTable1>();
            List<GetReceivedPaymentByDepTable2> _lstTable2 = new List<GetReceivedPaymentByDepTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetReceivedPaymentByDepTable1()
                    {
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        InvoiceID = Convert.ToInt32(DBNull.Value.Equals(dr["InvoiceID"]) ? 0 : dr["InvoiceID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        En = Convert.ToInt32(DBNull.Value.Equals(dr["En"]) ? 0 : dr["En"]),
                        Company = Convert.ToString(dr["Company"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        PaymentReceivedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PaymentReceivedDate"]) ? null : dr["PaymentReceivedDate"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
                        PaymentMethodID = Convert.ToString(dr["PaymentMethodID"]),
                        CheckNumber = Convert.ToString(dr["CheckNumber"]),
                        AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
                        isChecked = Convert.ToBoolean(DBNull.Value.Equals(dr["isChecked"]) ? false : dr["isChecked"]),
                        OrderNo = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetReceivedPaymentByDepTable2()
                    {
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        fTitle = Convert.ToString(dr["fTitle"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        ischecked = Convert.ToInt32(DBNull.Value.Equals(dr["ischecked"]) ? 0 : dr["ischecked"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        orderNo = Convert.ToInt32(DBNull.Value.Equals(dr["orderNo"]) ? 0 : dr["orderNo"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Tag = Convert.ToString(dr["Tag"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public DataSet GetAllReceivePaymentForDep(ReceivedPayment objReceivePay)
        {
            return objDL_Deposit.GetAllReceivePaymentForDep(objReceivePay);
        }

        //API
        public List<GetAllReceivePaymentForDepViewModel> GetAllReceivePaymentForDep(GetAllReceivePaymentForDepParam _GetAllReceivePaymentForDep, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetAllReceivePaymentForDep(_GetAllReceivePaymentForDep, ConnectionString);

            List<GetAllReceivePaymentForDepViewModel> _lstGetAllReceivePaymentForDep = new List<GetAllReceivePaymentForDepViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllReceivePaymentForDep.Add(
                    new GetAllReceivePaymentForDepViewModel()
                    {
                        owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        PaymentReceivedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PaymentReceivedDate"]) ? null : dr["PaymentReceivedDate"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
                        CheckNumber = Convert.ToString(dr["CheckNumber"]),
                        AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
                    }
                    );
            }

            return _lstGetAllReceivePaymentForDep;
        }
        public void AddOpenARDetails(OpenAR _objOpenAR)
        {
            objDL_Deposit.AddOpenARDetails(_objOpenAR);
        }
        public DataSet GetPaymentDetailsByReceivedID(PaymentDetails _objPayment)
        {
            return objDL_Deposit.GetPaymentDetailsByReceivedID(_objPayment);
        }
        public DataSet GetPaymentByReceivedBatch(PJ _objPj)
        {
            return objDL_Deposit.GetPaymentByReceivedBatch(_objPj);
        }
        public DataSet GetTransByBatch(Transaction _objTrans)
        {
            return objDL_Deposit.GetTransByBatch(_objTrans);
        }
        public DataSet GetByReceivedPaymentByTransID(PaymentDetails _objPmtDetail)
        {
            return objDL_Deposit.GetByReceivedPaymentByTransID(_objPmtDetail);
        }
        public void UpdateDepRecon(Dep _objDep)
        {
            objDL_Deposit.UpdateDepRecon(_objDep);
        }
        public DataSet GetDepositDetails(Dep _objDep)
        {
            return objDL_Deposit.GetDepositDetails(_objDep);
        }
        public void DeletePayment(ReceivedPayment objReceivePay)
        {
            objDL_Deposit.DeletePayment(objReceivePay);
        }

        //API
        public void DeletePayment(DeletePaymentParam _DeletePayment, string ConnectionString)
        {
            objDL_Deposit.DeletePayment(_DeletePayment, ConnectionString);
        }
        public void DeleteDeposit(Dep _objDep)
        {
            objDL_Deposit.DeleteDeposit(_objDep);
        }

        //API
        public void DeleteDeposit(DeleteDepositParam _DeleteDeposit, string ConnectionString)
        {
            objDL_Deposit.DeleteDeposit(_DeleteDeposit, ConnectionString);
        }
        public void UpdateReceivedPayStatus(ReceivedPayment objReceivePay)
        {
            objDL_Deposit.UpdateReceivedPayStatus(objReceivePay);
        }

        //API
        public void UpdateReceivedPayStatus(UpdateReceivedPayStatusParam _UpdateReceivedPayStatus, string ConnectionString)
        {
            objDL_Deposit.UpdateReceivedPayStatus(_UpdateReceivedPayStatus, ConnectionString);
        }
        public void UpdateReceivePayment(ReceivedPayment objReceivePay, int locCredit = 0)
        {
            objDL_Deposit.UpdateReceivePayment(objReceivePay, locCredit);
        }

        //API
        public void UpdateReceivePayment(UpdateReceivePaymentParam _UpdateReceivePayment, string ConnectionString)
        {
            objDL_Deposit.UpdateReceivePayment(_UpdateReceivePayment, ConnectionString);
        }

        //public void UpdateReceivedPayment(ReceivedPayment _objReceiPmt)
        //{
        //    objDL_Deposit.UpdateReceivedPayment(_objReceiPmt);
        //}
        public DataSet GetInvoiceByCustomerID(Contracts objPropContracts)
        {
            return objDL_Deposit.GetInvoiceByCustomerID(objPropContracts);
        }

        //API
        public ListGetInvoiceByCustomerID GetInvoiceByCustomerID(GetInvoiceByCustomerIDParam _GetInvoiceByCustomerID, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetInvoiceByCustomerID(_GetInvoiceByCustomerID, ConnectionString);

            ListGetInvoiceByCustomerID _ds = new ListGetInvoiceByCustomerID();
            List<GetInvoiceByCustomerIDTable1> _lstTable1 = new List<GetInvoiceByCustomerIDTable1>();
            List<GetInvoiceByCustomerIDTable2> _lstTable2 = new List<GetInvoiceByCustomerIDTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetInvoiceByCustomerIDTable1()
                    {
                         Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                         Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                         fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                         ID = Convert.ToString(dr["ID"]),
                         Tag = Convert.ToString(dr["Tag"]),
                         Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                         STax = Convert.ToDouble(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                         Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                         StatusID = Convert.ToInt16(DBNull.Value.Equals(dr["StatusID"]) ? 0 : dr["StatusID"]),
                         manualInv = Convert.ToString(dr["manualInv"]),
                         TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                         PrevDueAmount = Convert.ToDouble(DBNull.Value.Equals(dr["PrevDueAmount"]) ? 0 : dr["PrevDueAmount"]),
                         paymentAmt = Convert.ToDouble(DBNull.Value.Equals(dr["paymentAmt"]) ? 0 : dr["paymentAmt"]),
                         PaymentID = Convert.ToInt32(DBNull.Value.Equals(dr["PaymentID"]) ? 0 : dr["PaymentID"]),
                         DueAmount = Convert.ToDouble(DBNull.Value.Equals(dr["DueAmount"]) ? 0 : dr["DueAmount"]),
                         OrigAmount = Convert.ToDouble(DBNull.Value.Equals(dr["OrigAmount"]) ? 0 : dr["OrigAmount"]),
                         status = Convert.ToString(dr["status"]),
                         PO = Convert.ToString(dr["PO"]),
                         loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                         customername = Convert.ToString(dr["customername"]),
                         type = Convert.ToString(dr["type"]),
                         balance = Convert.ToDouble(DBNull.Value.Equals(dr["balance"]) ? 0 : dr["balance"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetInvoiceByCustomerIDTable2()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public void UpdatePayment(ReceivedPayment _objReceivePay)
        {
            objDL_Deposit.UpdatePayment(_objReceivePay);
        }
        public DataSet GetPaymentCustomer(ReceivedPayment objReceivePay)
        {
            return objDL_Deposit.GetPaymentCustomer(objReceivePay);
        }
        public DataSet GetInvoicesByReceivedPay(PaymentDetails objPayment)
        {
            return objDL_Deposit.GetInvoicesByReceivedPay(objPayment);
        }

        //API
        public ListGetInvoicesByReceivedPay GetInvoicesByReceivedPay(GetInvoicesByReceivedPayParam _GetInvoicesByReceivedPay, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetInvoicesByReceivedPay(_GetInvoicesByReceivedPay, ConnectionString);

            ListGetInvoicesByReceivedPay _ds = new ListGetInvoicesByReceivedPay();
            List<GetInvoicesByReceivedPayTable1> _lstTable1 = new List<GetInvoicesByReceivedPayTable1>();
            List<GetInvoicesByReceivedPayTable2> _lstTable2 = new List<GetInvoicesByReceivedPayTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetInvoicesByReceivedPayTable1()
                    {
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        ID = Convert.ToString(dr["ID"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        STax = Convert.ToDouble(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        StatusID = Convert.ToInt16(DBNull.Value.Equals(dr["StatusID"]) ? 0 : dr["StatusID"]),
                        manualInv = Convert.ToString(dr["manualInv"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        PrevDueAmount = Convert.ToDouble(DBNull.Value.Equals(dr["PrevDueAmount"]) ? 0 : dr["PrevDueAmount"]),
                        paymentAmt = Convert.ToDouble(DBNull.Value.Equals(dr["paymentAmt"]) ? 0 : dr["paymentAmt"]),
                        PaymentID = Convert.ToInt32(DBNull.Value.Equals(dr["PaymentID"]) ? 0 : dr["PaymentID"]),
                        DueAmount = Convert.ToDouble(DBNull.Value.Equals(dr["DueAmount"]) ? 0 : dr["DueAmount"]),
                        OrigAmount = Convert.ToDouble(DBNull.Value.Equals(dr["OrigAmount"]) ? 0 : dr["OrigAmount"]),
                        status = Convert.ToString(dr["status"]),
                        PO = Convert.ToString(dr["PO"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        customername = Convert.ToString(dr["customername"]),
                        type = Convert.ToString(dr["type"]),
                        balance = Convert.ToDouble(DBNull.Value.Equals(dr["balance"]) ? 0 : dr["balance"]),
                        IsCredit = Convert.ToInt32(DBNull.Value.Equals(dr["IsCredit"]) ? 0 : dr["IsCredit"]),
                        OpenARType = Convert.ToInt16(DBNull.Value.Equals(dr["OpenARType"]) ? 0 : dr["OpenARType"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        ReceivePayId = Convert.ToInt32(DBNull.Value.Equals(dr["ReceivePayId"]) ? 0 : dr["ReceivePayId"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;

            if (ds.Tables.Count == 2)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    _lstTable2.Add(
                        new GetInvoicesByReceivedPayTable2()
                        {
                            Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        }
                        );
                }
                _ds.lstTable2 = _lstTable2;
            }
           
            return _ds;
        }
        public int AddReceivePayment(ReceivedPayment objReceivePay, int locCredit = 0)
        {
            return objDL_Deposit.AddReceivePayment(objReceivePay, locCredit);
        }

        //API
        public int AddReceivePayment(AddReceivePaymentParam _AddReceivePayment, string ConnectionString)
        {
            return objDL_Deposit.AddReceivePayment(_AddReceivePayment, ConnectionString);
        }
        public void UpdateDepositTrans(Dep objDep)
        {
            objDL_Deposit.UpdateDepositTrans(objDep);
        }

        public DataSet GetAllReceivePaymentAjaxSearch(ReceivedPayment objReceivePay, int intEN)
        {
            return objDL_Deposit.GetAllReceivePaymentAjaxSearch(objReceivePay, intEN);
        }
        public void UpdateDepositTransBank(Transaction objTrans)
        {
            objDL_Deposit.UpdateDepositTransBank(objTrans);
        }

        //API
        public void UpdateDepositTransBank(UpdateDepositTransBankParam _UpdateDepositTransBank, string ConnectionString)
        {
            objDL_Deposit.UpdateDepositTransBank(_UpdateDepositTransBank, ConnectionString);
        }
        public DataSet GetInvoiceNos(PaymentDetails objPayment)
        {
            return objDL_Deposit.GetInvoiceNos(objPayment);
        }

        public DataSet GetInvoiceNosChange(PaymentDetails objPayment)
        {
            return objDL_Deposit.GetInvoiceNosChange(objPayment);
        }

        //API
        public List<GetInvoiceNosChangeViewModel> GetInvoiceNosChange(GetInvoiceNosChangeParam _GetInvoiceNosChange, string ConnectionString)
        {
            DataSet ds = objDL_Deposit.GetInvoiceNosChange(_GetInvoiceNosChange, ConnectionString);

            List<GetInvoiceNosChangeViewModel> _lstGetInvoiceNosChange = new List<GetInvoiceNosChangeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInvoiceNosChange.Add(
                    new GetInvoiceNosChangeViewModel()
                    {
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        ID = Convert.ToString(dr["ID"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    }
                    );
            }

            return _lstGetInvoiceNosChange;
        }


        public DataSet GetInvoiceByList(string conn, string invoiceId,string checkNumber, bool isSeparate)
        {
            return objDL_Deposit.GetInvoiceByList(conn,invoiceId, checkNumber, isSeparate);
        }

        //API
        public List<GetInvoiceByListViewModel> GetInvoiceByList(GetInvoiceByListParam _GetInvoiceByList, string ConnectionString, string invoiceId, String checkNumber, Boolean isSeparate)
        {
            DataSet ds = objDL_Deposit.GetInvoiceByList(_GetInvoiceByList, ConnectionString, invoiceId, checkNumber, isSeparate);

            List<GetInvoiceByListViewModel> _lstGetInvoiceByList = new List<GetInvoiceByListViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInvoiceByList.Add(
                    new GetInvoiceByListViewModel()
                    {
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        STax = Convert.ToDouble(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        PrevDueAmount = Convert.ToDouble(DBNull.Value.Equals(dr["PrevDueAmount"]) ? 0 : dr["PrevDueAmount"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
                        paymentAmt = Convert.ToDouble(DBNull.Value.Equals(dr["paymentAmt"]) ? 0 : dr["paymentAmt"]),
                        Invoice = Convert.ToString(dr["Invoice"]),
                        CheckNumber = Convert.ToString(dr["CheckNumber"]),
                        BatchReceive = Convert.ToString(dr["BatchReceive"]),
                        OrderNo = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                        isChecked = Convert.ToInt16(DBNull.Value.Equals(dr["isChecked"]) ? 0 : dr["isChecked"]),
                    }
                    );
            }

            return _lstGetInvoiceByList;
        }

        public int AddMultiReceivePayment(ReceivedPayment objReceivePay, int bank,bool createDeposit)
        {
             return objDL_Deposit.AddMultiReceivePayment(objReceivePay, bank, createDeposit);
        }
        public DataSet GetInvoicesByReceivedPayMulti(string conn,int owner,string loc,string invoice)
        {
            return objDL_Deposit.GetInvoicesByReceivedPayMulti(conn,owner, loc, invoice);
        }

        //API
        public List<GetInvoicesByReceivedPayMultiViewModel> GetInvoicesByReceivedPayMulti(GetInvoicesByReceivedPayMultiParam _GetInvoicesByReceivedPayMulti, string ConnectionString, int owner, string loc, string invoice)
        {
            DataSet ds = objDL_Deposit.GetInvoicesByReceivedPayMulti(_GetInvoicesByReceivedPayMulti, ConnectionString, owner, loc, invoice);

            List<GetInvoicesByReceivedPayMultiViewModel> _lstGetInvoicesByReceivedPayMulti = new List<GetInvoicesByReceivedPayMultiViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInvoicesByReceivedPayMulti.Add(
                    new GetInvoicesByReceivedPayMultiViewModel()
                    {
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        ID = Convert.ToString(dr["ID"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
                        Invoice = Convert.ToInt32(DBNull.Value.Equals(dr["Invoice"]) ? 0 : dr["Invoice"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                    }
                    );
            }

            return _lstGetInvoicesByReceivedPayMulti;
        }


        public DataSet GetAllInvoiceByDep(string conn, int depId)
        {
            return objDL_Deposit.GetAllInvoiceByDep(conn, depId);
        }

        //API
        public ListGetAllInvoiceByDep GetAllInvoiceByDep(GetAllInvoiceByDepParam _GetAllInvoiceByDep, string ConnectionString, int depId)
        {
            DataSet ds = objDL_Deposit.GetAllInvoiceByDep(_GetAllInvoiceByDep, ConnectionString, depId);

            ListGetAllInvoiceByDep _ds = new ListGetAllInvoiceByDep();
            List<GetAllInvoiceByDepTable1> _lstTable1 = new List<GetAllInvoiceByDepTable1>();
            List<GetAllInvoiceByDepTable2> _lstTable2 = new List<GetAllInvoiceByDepTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetAllInvoiceByDepTable1()
                    {
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        InvoiceID = Convert.ToInt32(DBNull.Value.Equals(dr["InvoiceID"]) ? 0 : dr["InvoiceID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        En = Convert.ToInt32(DBNull.Value.Equals(dr["En"]) ? 0 : dr["En"]),
                        Company = Convert.ToString(dr["Company"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        PaymentReceivedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PaymentReceivedDate"]) ? null : dr["PaymentReceivedDate"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
                        PaymentMethodID = Convert.ToString(dr["PaymentMethodID"]),
                        CheckNumber = Convert.ToString(dr["CheckNumber"]),
                        AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
                        isChecked = Convert.ToBoolean(DBNull.Value.Equals(dr["isChecked"]) ? false : dr["isChecked"]),
                        OrderNo = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetAllInvoiceByDepTable2()
                    {
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        fTitle = Convert.ToString(dr["fTitle"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        ischecked = Convert.ToInt32(DBNull.Value.Equals(dr["ischecked"]) ? 0 : dr["ischecked"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        orderNo = Convert.ToInt32(DBNull.Value.Equals(dr["orderNo"]) ? 0 : dr["orderNo"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Tag = Convert.ToString(dr["Tag"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public void UpdateDeposit(string conn, int depId, DataTable dtDelete, DataTable dtNew, DataTable dtDeleteGL, DataTable dtNewGL,string UpdatedBy)
        {
            objDL_Deposit.UpdateDeposit(conn,depId, dtDelete, dtNew, dtDeleteGL, dtNewGL, UpdatedBy);
        }

        //API
        public void UpdateDeposit(UpdateDepositParam _UpdateDeposit, string ConnectionString, int depId, DataTable dtDelete, DataTable dtNew, DataTable dtDeleteGL, DataTable dtNewGL, String UpdatedBy)
        {
            objDL_Deposit.UpdateDeposit(_UpdateDeposit, ConnectionString, depId, dtDelete, dtNew, dtDeleteGL, dtNewGL, UpdatedBy);
        }
        public DataSet GetGLAccount(string conn,  string acct)
        {
            return objDL_Deposit.GetGLAccountForDeposit(conn, acct);
        }

        //API
        public List<GetGLAccountViewModel> GetGLAccount(GetGLAccountParam _GetGLAccount, string ConnectionString, string acct)
        {
            DataSet ds = objDL_Deposit.GetGLAccount(_GetGLAccount, ConnectionString, acct);

            List<GetGLAccountViewModel> _lstGetGLAccount = new List<GetGLAccountViewModel>();
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstGetGLAccount.Add(
                    new GetGLAccountViewModel()
                    {
                        Acct = Convert.ToString(dr["Acct"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        CAlias = Convert.ToString(dr["CAlias"]),
                        Sub = Convert.ToString(dr["Sub"]),
                    }
                    );
            }
            return _lstGetGLAccount;
        }
        public int AddDepositWithGL(Dep _objDep)
        {
            return objDL_Deposit.AddDepositWithGL(_objDep);
        }

        //API
        public int AddDepositWithGL(AddDepositWithGLParam _AddDepositWithGL, string ConnectionString)
        {
            return objDL_Deposit.AddDepositWithGL(_AddDepositWithGL, ConnectionString);
        }

        public DataSet GetCustomerUnAppliedCredit(string conn, int userId, int filter)
        {
            return objDL_Deposit.GetCustomerUnAppliedCredit(conn, userId, filter);
        }

        //API
        public List<GetCustomerUnAppliedCreditViewModel> GetCustomerUnAppliedCredit(GetCustomerUnAppliedCreditParam _GetCustomerUnAppliedCredit, string ConnectionString, int userId, int filter)
        {
            DataSet ds = objDL_Deposit.GetCustomerUnAppliedCredit(_GetCustomerUnAppliedCredit, ConnectionString, userId, filter);
            List<GetCustomerUnAppliedCreditViewModel> _lstGetCustomerUnAppliedCredit = new List<GetCustomerUnAppliedCreditViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerUnAppliedCredit.Add(
                    new GetCustomerUnAppliedCreditViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                    }
                    );
            }

            return _lstGetCustomerUnAppliedCredit;

        }

        public DataSet GetReceivedPaymentByDepSlip(ReceivedPayment objReceivePay)
        {
            return objDL_Deposit.GetReceivedPaymentByDepSlip(objReceivePay);
        }
        public DataSet GetDepositListByDate(Dep _objDep,bool incZeroAmount)
        {
            return objDL_Deposit.GetDepositListByDate(_objDep, incZeroAmount);
        }

        //API
        public List<GetDepositListByDateViewModel> GetDepositListByDate(GetDepositListByDateParam _GetDepositListByDate, string ConnectionString, bool incZeroAmount)
        {
            DataSet ds = objDL_Deposit.GetDepositListByDate(_GetDepositListByDate, ConnectionString, incZeroAmount);
            List<GetDepositListByDateViewModel> _lstGetDepositListByDate = new List<GetDepositListByDateViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetDepositListByDate.Add(
                    new GetDepositListByDateViewModel()
                    {
                        DepID = Convert.ToInt32(DBNull.Value.Equals(dr["DepID"]) ? 0 : dr["DepID"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        InvoiceID = Convert.ToInt32(DBNull.Value.Equals(dr["InvoiceID"]) ? 0 : dr["InvoiceID"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Company = Convert.ToString(dr["Company"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        PaymentReceivedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PaymentReceivedDate"]) ? null : dr["PaymentReceivedDate"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
                        OrderNo = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Department = Convert.ToString(dr["Department"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        DefaultSalePerson = Convert.ToString(dr["DefaultSalePerson"]),
                        CheckNumber = Convert.ToString(dr["CheckNumber"]),
                        PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
                        AccountChart = Convert.ToString(dr["AccountChart"]),
                        DepDate = Convert.ToDateTime(DBNull.Value.Equals(dr["DepDate"]) ? null : dr["DepDate"]),
                        Bank = Convert.ToString(dr["Bank"]),
                        ProjectID = Convert.ToString(dr["ProjectID"]),
                    }
                    );
            }

            return _lstGetDepositListByDate;
        }
        public void writeOffInvoice( WriteOff obj)
        {
            objDL_Deposit.writeOffInvoice( obj);
        }

        //API
        public void writeOffInvoice(writeOffInvoiceParam _writeOffInvoice, string ConnectionString)
        {
            objDL_Deposit.writeOffInvoice(_writeOffInvoice, ConnectionString);
        }

        public void writeOffInvoiceMulti(WriteOff obj)
        {
            objDL_Deposit.writeOffInvoiceMulti(obj);
        }

        //API
        public void writeOffInvoiceMulti(writeOffInvoiceMultiParam _writeOffInvoiceMulti, string ConnectionString)
        {
            objDL_Deposit.writeOffInvoiceMulti(_writeOffInvoiceMulti, ConnectionString);
        }
        public DataSet AddBatchReceivePayment(ReceivedPayment objReceivePay, int bank, bool createDeposit)
        {
            return objDL_Deposit.AddBatchReceivePayment(objReceivePay, bank, createDeposit);
        }
        public DataSet GetBatchReceivePayment(string conn, int batch)
        {
            return objDL_Deposit.GetBatchReceivePayment(conn,batch);
        }
        public DataSet UpdateBatchReceivePayment(ReceivedPayment objReceivePay, int bank, bool createDeposit, int batchReceipt)
        {
            return objDL_Deposit.UpdateBatchReceivePayment(objReceivePay, bank, createDeposit, batchReceipt);
        }

        public void TransferPayment(string conn, string strRef, int newLoc)
        {
            objDL_Deposit.TransferPayment(conn,strRef, newLoc);
        }

        //API
        public void TransferPayment(TransferPaymentParam _TransferPayment, string ConnectionString, string strRef, int newLoc)
        {
            objDL_Deposit.TransferPayment(_TransferPayment, ConnectionString, strRef, newLoc);
        }
        public void UnapplyPayment(string conn, int Ref, string MOMUser)
        {
            objDL_Deposit.UnapplyPayment(conn, Ref, MOMUser);
        }

        //API
        public void UnapplyPayment(UnapplyPaymentParam _UnapplyPayment, string ConnectionString, int Ref)
        {
            objDL_Deposit.UnapplyPayment(_UnapplyPayment, ConnectionString, Ref);
        }

        public DataSet GetReceivePaymentReport(ReceivedPayment objReceivePay, List<RetainFilter> filters, int intEN)
        {
            return objDL_Deposit.GetReceivePaymentReport(objReceivePay, filters, intEN);
        }

        public DataSet GetAllReceivePayment(ReceivedPayment objReceivePay, List<RetainFilter> filters, int intEN)
        {
            return objDL_Deposit.GetAllReceivePayment(objReceivePay, filters, intEN);
        }

        //API
        //public List<GetAllReceivePaymentViewModel> GetAllReceivePayment(GetAllReceivePaymentParam _GetAllReceivePayment, string ConnectionString, List<RetainFilter> filters, int intEN)
        //{
        //    DataSet ds = objDL_Deposit.GetAllReceivePayment(_GetAllReceivePayment, ConnectionString, filters, intEN);

        //    List<GetAllReceivePaymentViewModel> _lstGetAllReceivePayment = new List<GetAllReceivePaymentViewModel>();

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        _lstGetAllReceivePayment.Add(
        //            new GetAllReceivePaymentViewModel()
        //            {
        //                ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
        //                Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
        //                CustomerName = Convert.ToString(dr["CustomerName"]),
        //                Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
        //                Tag = Convert.ToString(dr["Tag"]),
        //                Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
        //                EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
        //                Company = Convert.ToString(dr["Company"]),
        //                PaymentReceivedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PaymentReceivedDate"]) ? null : dr["PaymentReceivedDate"]),
        //                fDesc = Convert.ToString(dr["fDesc"]),
        //                PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
        //                StatusName = Convert.ToString(dr["StatusName"]),
        //                CheckNumber = Convert.ToString(dr["CheckNumber"]),
        //                AmountDue = Convert.ToDouble(DBNull.Value.Equals(dr["AmountDue"]) ? 0 : dr["AmountDue"]),
        //                Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
        //                DepID = Convert.ToInt32(DBNull.Value.Equals(dr["DepID"]) ? 0 : dr["DepID"]),
        //                BatchReceipt = Convert.ToInt32(DBNull.Value.Equals(dr["BatchReceipt"]) ? 0 : dr["BatchReceipt"]),
        //            }
        //            );
        //    }

        //    return _lstGetAllReceivePayment;

        //}

        public void writeOffCredit(WriteOff obj)
        {
            objDL_Deposit.writeOffCredit(obj);
        }
        public void writeOffDeposit(WriteOff obj)
        {
            objDL_Deposit.writeOffDeposit(obj);
        }
    }
}
