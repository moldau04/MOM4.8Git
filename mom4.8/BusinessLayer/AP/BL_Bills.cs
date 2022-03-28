using BusinessEntity;
using BusinessEntity.APModels;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_Bills
    {
        DL_Bills _objDL_Bills = new DL_Bills();
        public void AddOpenAP(OpenAP _objOpenAP)
        {
            _objDL_Bills.AddOpenAP(_objOpenAP);
        }
        public void AddOpenAP(AddOpenAPParam _objAddOpenAPParam,string ConnectionString)
        {
            _objDL_Bills.AddOpenAP(_objAddOpenAPParam, ConnectionString);
        }
        public int AddPJ(PJ _objPJ)
        {
            return _objDL_Bills.AddPJ(_objPJ);
        }
        public int AddPJ(AddPJParam _objAddPJParam, string ConnectionString)
        {
           int _newPJID = _objDL_Bills.AddPJ(_objAddPJParam, ConnectionString);
            return _newPJID;
        }
        public void AddJobI(JobI _objJobI)
        {
            _objDL_Bills.AddJobI(_objJobI);
        }
        public void AddJobI(AddJobIParam _objAddJobIParam,string ConnectionString)
        {
            _objDL_Bills.AddJobI(_objAddJobIParam, ConnectionString);
        }
        public DataSet GetAllPJDetails(PJ _objPJ)
        {
            return _objDL_Bills.GetAllPJDetails(_objPJ);
        }

        public List<GetAllPJDetailsViewModel> GetAllPJDetails(GetAllPJDetailsParam _GetAllPJDetailsParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAllPJDetails(_GetAllPJDetailsParam, ConnectionString);

            List<GetAllPJDetailsViewModel> _lstPJViewModel = new List<GetAllPJDetailsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstPJViewModel.Add(
                    new GetAllPJDetailsViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        PostingDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PostingDate"]) ? null : dr["PostingDate"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),

                        //fDate = Convert.ToString(dr["fDate"]),
                        //PostingDate = Convert.ToString(dr["PostingDate"]),
                        //IDate = Convert.ToString(dr["IDate"]),
                        //Date = Convert.ToString(dr["Date"]),

                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Status1 = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        //Due = Convert.ToString(dr["Due"]),
                    }
                    );

            }
            return _lstPJViewModel;
        }
        public DataSet GetAllPJRecurrDetails(PJ _objPJ)
        {
            return _objDL_Bills.GetAllPJRecurrDetails(_objPJ);
        }

        public List<GetAllPJRecurrDetailsViewModel> GetAllPJRecurrDetails(GetAllPJRecurrDetailsParam _GetAllPJRecurrDetailsParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAllPJRecurrDetails(_GetAllPJRecurrDetailsParam, ConnectionString);

            List<GetAllPJRecurrDetailsViewModel> _lstPJViewModel = new List<GetAllPJRecurrDetailsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstPJViewModel.Add(
                    new GetAllPJRecurrDetailsViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        PostingDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PostingDate"]) ? null : dr["PostingDate"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Status1 = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        VendorName = dr["VendorName"].ToString(),
                        //Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                    }
                    );
            }
            return _lstPJViewModel;
        }
        public DataSet GetBillHistoryPayment(PJ objPJ)
        {
            return _objDL_Bills.GetBillHistoryPayment(objPJ);
        }
        public List<GetBillHistoryPaymentViewModel> GetBillHistoryPayment(GetBillHistoryPaymentParam _GetBillHistoryPaymentParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetBillHistoryPayment(_GetBillHistoryPaymentParam, ConnectionString);
            List<GetBillHistoryPaymentViewModel> _lstGetBillHistoryPayment = new List<GetBillHistoryPaymentViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBillHistoryPayment.Add(
                    new GetBillHistoryPaymentViewModel()
                    {
                        Type = Convert.ToString(dr["Type"]),
                        ReceivedPaymentID = Convert.ToInt32(DBNull.Value.Equals(dr["ReceivedPaymentID"]) ? 0 : dr["ReceivedPaymentID"]),
                        PaymentDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PaymentDate"]) ? null : dr["PaymentDate"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        link = Convert.ToString(dr["link"]),
                    }
                    );

            }
            return _lstGetBillHistoryPayment;
        }
        public DataSet GetPJDetailByID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJDetailByID(_objPJ);
        }
        public List<GetPJDetailByIDViewModel> GetPJDetailByID(GetPJDetailByIDParam _GetPJDetailByIDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetPJDetailByID(_GetPJDetailByIDParam, ConnectionString);
            List<GetPJDetailByIDViewModel> _lstGetPJDetailByID = new List<GetPJDetailByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetPJDetailByID.Add(
                    new GetPJDetailByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        IfPaid = Convert.ToInt32(DBNull.Value.Equals(dr["IfPaid"]) ? 0 : dr["IfPaid"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                        STaxType = Convert.ToInt16(DBNull.Value.Equals(dr["STaxType"]) ? 0 : dr["STaxType"]),
                        STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        ReceivePO = Convert.ToInt32(DBNull.Value.Equals(dr["ReceivePO"]) ? 0 : dr["ReceivePO"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        VendorID = Convert.ToInt32(DBNull.Value.Equals(dr["VendorID"]) ? 0 : dr["VendorID"]),
                        State = Convert.ToString(dr["State"]),
                        ReceivedAmount = Convert.ToDouble(DBNull.Value.Equals(dr["ReceivedAmount"]) ? 0 : dr["ReceivedAmount"]),
                        POAmount = Convert.ToDouble(DBNull.Value.Equals(dr["POAmount"]) ? 0 : dr["POAmount"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        CrPaid = Convert.ToDouble(DBNull.Value.Equals(dr["CrPaid"]) ? 0 : dr["CrPaid"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                    }
                    );

            }
            return _lstGetPJDetailByID;
        }
        public DataSet GetPJAcctDetailByID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJAcctDetailByID(_objPJ);
        }
        public List<GetPJAcctDetailByIDViewModel> GetPJAcctDetailByID(GetPJAcctDetailByIDParam _GetPJAcctDetailByID, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetPJAcctDetailByID(_GetPJAcctDetailByID, ConnectionString);
            List<GetPJAcctDetailByIDViewModel> _lstPJViewModel = new List<GetPJAcctDetailByIDViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstPJViewModel.Add(
                    new GetPJAcctDetailByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? 0 : dr["fDate"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Line= Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        AcctSub = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
                        Status = Convert.ToString(dr["Status"]),
                        Sel = Convert.ToInt16(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        VInt = Convert.ToInt16(DBNull.Value.Equals(dr["VInt"]) ? 0 : dr["VInt"]),
                        VDoub = Convert.ToInt16(DBNull.Value.Equals(dr["VDoub"]) ? 0 : dr["VDoub"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        strRef = Convert.ToString(dr["strRef"]),
                        //TimeStamp = Convert.ToInt16(DBNull.Value.Equals(dr["TimeStamp"]) ? 0 : dr["TimeStamp"]),
                        AcctName = Convert.ToString(dr["AcctName"]),
                    }
                    );
            }
            return _lstPJViewModel;
        }
        public DataSet GetPJRecurrDetailByID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJRecurrDetailByID(_objPJ);
        }

        public List<GetPJRecurrDetailByIDViewModel> GetPJRecurrDetailByID(GetPJRecurrDetailByIDParam _GetPJRecurrDetailByIDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetPJRecurrDetailByID(_GetPJRecurrDetailByIDParam, ConnectionString);
            List<GetPJRecurrDetailByIDViewModel> _lstGetPJRecurrDetail = new List<GetPJRecurrDetailByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetPJRecurrDetail.Add(
                    new GetPJRecurrDetailByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        IfPaid = Convert.ToInt32(DBNull.Value.Equals(dr["IfPaid"]) ? 0 : dr["IfPaid"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                        STaxType = Convert.ToInt16(DBNull.Value.Equals(dr["STaxType"]) ? 0 : dr["STaxType"]),
                        STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        ReceivePO = Convert.ToInt32(DBNull.Value.Equals(dr["ReceivePO"]) ? 0 : dr["ReceivePO"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        VendorID = Convert.ToInt32(DBNull.Value.Equals(dr["VendorID"]) ? 0 : dr["VendorID"]),
                        State = Convert.ToString(dr["State"]),
                        ReceivedAmount = Convert.ToDouble(DBNull.Value.Equals(dr["ReceivedAmount"]) ? 0 : dr["ReceivedAmount"]),
                        POAmount = Convert.ToDouble(DBNull.Value.Equals(dr["POAmount"]) ? 0 : dr["POAmount"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        CrPaid = Convert.ToDouble(DBNull.Value.Equals(dr["CrPaid"]) ? 0 : dr["CrPaid"]),
                        
                    }
                    );

            }
            return _lstGetPJRecurrDetail;
        }
        public DataSet GetCreditBillVendor(Vendor _objVendor)
        {
            return _objDL_Bills.GetCreditBillVendor(_objVendor);
        }
        public List<OpenAPViewModel> GetCreditBillVendor(GetCreditBillVendorParam _GetCreditBillVendorParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetCreditBillVendor(_GetCreditBillVendorParam, ConnectionString);
            List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstOpenAPViewModel.Add(
                    new OpenAPViewModel()
                    {
                        RolName =Convert.ToString(dr["Name"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    }
                    );
            }

            return _lstOpenAPViewModel;
        }

        public void DeleteJobI(JobI _objJobI)
        {
            _objDL_Bills.DeleteJobI(_objJobI);
        }
        public void UpdatePJ(PJ _objPJ)
        {
            _objDL_Bills.UpdatePJ(_objPJ);
        }
        public void UpdateOpenAP(OpenAP _objOpenAP)
        {
            _objDL_Bills.UpdateOpenAP(_objOpenAP);
        }
        public void UpdateOpenAPBalance(OpenAP _objOpenAP)
        {
            _objDL_Bills.UpdateOpenAPBalance(_objOpenAP);
        }

        public void UpdateOpenAPBalance(UpdateOpenAPBalanceParam _objUpdateOpenAPBalanceParam,string ConnectionString)
        {
            _objDL_Bills.UpdateOpenAPBalance(_objUpdateOpenAPBalanceParam, ConnectionString);
        }

        public DataSet GetLastCDRef(CD _objCD)
        {
            return _objDL_Bills.GetLastCDRef(_objCD);
        }
        public DataSet GetBillsByVendor(OpenAP _objOpenAP)
        {
            return _objDL_Bills.GetBillsByVendor(_objOpenAP);
        }
        public List<OpenAPViewModel> GetBillsByVendor(GetBillsByVendorParam _GetBillsByVendorParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetBillsByVendor(_GetBillsByVendorParam,ConnectionString);
            List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstOpenAPViewModel.Add(
                    new OpenAPViewModel()
                    {
                        RolName = Convert.ToString(dr["Name"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Selected = Convert.ToDouble(DBNull.Value.Equals(dr["Selected"]) ? 0 : dr["Selected"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Discount = Convert.ToDouble(DBNull.Value.Equals(dr["Discount"]) ? 0 : dr["Discount"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        Payment = Convert.ToDouble(DBNull.Value.Equals(dr["Payment"]) ? 0 : dr["Payment"]),
                        billDesc = Convert.ToString(dr["billDesc"]),
                        IsSelected = Convert.ToInt32(DBNull.Value.Equals(dr["IsSelected"]) ? 0 : dr["IsSelected"]),
                        Duepayment = Convert.ToDouble(DBNull.Value.Equals(dr["Duepayment"]) ? 0 : dr["Duepayment"]),
                        VendorType = Convert.ToString(dr["VendorType"]),
                    }
                    );
            }

            return _lstOpenAPViewModel;
        }
        public DataSet GetPJDetailByBatch(PJ _objPJ)
        {
            return _objDL_Bills.GetPJDetailByBatch(_objPJ);
        }
        public List<GetPJDetailByBatchViewModel> GetPJDetailByBatch(GetPJDetailByBatchParam _objGetPJDetailByBatchParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetPJDetailByBatch(_objGetPJDetailByBatchParam, ConnectionString);

            List<GetPJDetailByBatchViewModel> _lstGetPJDetailByBatchViewModel = new List<GetPJDetailByBatchViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetPJDetailByBatchViewModel.Add(
                    new GetPJDetailByBatchViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? null : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        ReceivePo = Convert.ToInt32(DBNull.Value.Equals(dr["ReceivePo"]) ? 0 : dr["ReceivePo"]),
                    }
                    );
            }

            return _lstGetPJDetailByBatchViewModel;
        }
        public DataSet GetAllCD(CD _objCD)
        {
            return _objDL_Bills.GetAllCD(_objCD);
        }
        public List<GetAllCDViewModel> GetAllCD(GetAllCDParam _objGetAllCDParam,string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAllCD(_objGetAllCDParam, ConnectionString);

            List<GetAllCDViewModel> _lstGetAllCD= new List<GetAllCDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllCD.Add(
                    new GetAllCDViewModel()
                    {
                        Company = Convert.ToString(dr["Company"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        RowNumber = Convert.ToInt64(DBNull.Value.Equals(dr["RowNumber"]) ? 0 : dr["RowNumber"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToInt64(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        Bank = Convert.ToInt32(DBNull.Value.Equals(dr["Bank"]) ? 0 : dr["Bank"]),
                        BankName = Convert.ToString(dr["BankName"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        ACH = Convert.ToInt16(DBNull.Value.Equals(dr["ACH"]) ? 0 : dr["ACH"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        French = Convert.ToString(dr["French"]),
                        Memo = Convert.ToString(dr["Memo"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        Sel = Convert.ToInt32(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        TypeName = Convert.ToString(dr["TypeName"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        TotalRow = Convert.ToInt32(DBNull.Value.Equals(dr["TotalRow"]) ? 0 : dr["TotalRow"]),
                    }
                    );
            }

            return _lstGetAllCD;
        }
        public DataSet GetCheckRecurrDetails(CD _objCD)
        {
            return _objDL_Bills.GetCheckRecurrDetails(_objCD);
        }

        public List<CDRecurrViewModel> GetCheckRecurrDetails(GetCheckRecurrDetailsParam _objGetCheckRecurrDetailsParam,string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetCheckRecurrDetails(_objGetCheckRecurrDetailsParam, ConnectionString);

            List<CDRecurrViewModel> _lstCDRecurrViewModel = new List<CDRecurrViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCDRecurrViewModel.Add(
                    new CDRecurrViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        Bank = Convert.ToInt32(DBNull.Value.Equals(dr["Bank"]) ? 0 : dr["Bank"]),
                        BankName = Convert.ToString(dr["BankName"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        ACH = Convert.ToInt16(DBNull.Value.Equals(dr["ACH"]) ? 0 : dr["ACH"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        French = Convert.ToString(dr["French"]),
                        Memo = Convert.ToString(dr["Memo"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        Sel = Convert.ToInt32(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        Frequency = Convert.ToInt32(DBNull.Value.Equals(dr["Frequency"]) ? 0 : dr["Frequency"]),
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                        TypeName = Convert.ToString(dr["TypeName"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        TotalRow = Convert.ToInt32(DBNull.Value.Equals(dr["TotalRow"]) ? 0 : dr["TotalRow"]),

                    }
                    );
            }

            return _lstCDRecurrViewModel;
        }
        //public DataSet GetAllCDByBankDate(CD _objCD) //Commented by Mayuri 8th Dec
        //{
        //    return _objDL_Bills.GetAllCDByBankDate(_objCD);
        //}
        public void UpdateCDRecon(CD _objCD)
        {
            _objDL_Bills.UpdateCDRecon(_objCD);
        }
        public DataSet GetChecksDetails(CD _objCD)
        {
            return _objDL_Bills.GetChecksDetails(_objCD);
        }
        public DataSet GetCheckDetailsByBankAndRef(CD _objCD)
        {
            return _objDL_Bills.GetCheckDetailsByBankAndRef(_objCD);
        }

        public DataSet GetPRCheckDetailsByBankAndRef(CD _objCD)
        {
            return _objDL_Bills.GetPRCheckDetailsByBankAndRef(_objCD);
        }

        public DataSet GetPRCheckDetailsByBankAndRefDecustion(CD _objCD)
        {
            return _objDL_Bills.GetPRCheckDetailsByBankAndRefDed(_objCD);
        }

        //public List<GetCheckDetailsByBankAndRefViewModel> GetCheckDetailsByBankAndRef(GetCheckDetailsByBankAndRefParam _objGetCheckDetailsByBankAndRefParam, string ConnectionString)
        //{
        //    DataSet ds = _objDL_Bills.GetCheckDetailsByBankAndRef(_objGetCheckDetailsByBankAndRefParam, ConnectionString);

        //    List<GetCheckDetailsByBankAndRefViewModel> _lstGetCheckDetailsByBankAndRefViewModel = new List<GetCheckDetailsByBankAndRefViewModel>();

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        _lstGetCheckDetailsByBankAndRefViewModel.Add(
        //            new GetCheckDetailsByBankAndRefViewModel()
        //            {
        //                Ref = Convert.ToString(dr["Ref"]),
        //                InvoiceDate = Convert.ToDateTime(DBNull.Value.Equals(dr["InvoiceDate"]) ? null : dr["InvoiceDate"]),
        //                Refrerence = Convert.ToString(dr["Refrerence"]),
        //                Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
        //                Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
        //                AmountPay = Convert.ToDouble(DBNull.Value.Equals(dr["AmountPay"]) ? 0 : dr["AmountPay"]),
        //                PayDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PayDate"]) ? null : dr["PayDate"]),
        //                CheckNo = Convert.ToInt32(DBNull.Value.Equals(dr["CheckNo"]) ? 0 : dr["CheckNo"]),
        //                Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
        //                VendorName = Convert.ToString(dr["VendorName"]),
        //                Type = Convert.ToString(dr["Type"]),
        //                Description = Convert.ToString(dr["Description"]),
        //            }
        //            );
        //    }

        //    foreach (DataRow dr in ds.Tables[1].Rows)
        //    {
        //        _lstGetCheckDetailsByBankAndRefViewModel.Add(
        //            new GetCheckDetailsByBankAndRefViewModel()
        //            {
        //                Pay = Convert.ToDouble(DBNull.Value.Equals(dr["Pay"]) ? 0 : dr["Pay"]),
        //                ToOrder = Convert.ToString(dr["ToOrder"]),
        //                Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
        //                CheckAmount = Convert.ToDouble(DBNull.Value.Equals(dr["CheckAmount"]) ? 0 : dr["CheckAmount"]),
        //                ToOrerAddress = Convert.ToString(dr["ToOrerAddress"]),
        //                State = Convert.ToString(dr["State"]),
        //                Zip = Convert.ToString(dr["Zip"]),
        //                VendorAddress = Convert.ToString(dr["VendorAddress"]),
        //                RemitAddress = Convert.ToString(dr["RemitAddress"]),
        //                Memo = Convert.ToString(dr["Memo"]),
        //                Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
        //                CheckNo = Convert.ToInt32(DBNull.Value.Equals(dr["CheckNo"]) ? 0 : dr["CheckNo"]),
        //                Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
        //            }
        //            );
        //    }

        //    return _lstGetCheckDetailsByBankAndRefViewModel;
        //}
        public ListCheckDetailsByBankAndRef GetCheckDetailsByBankAndRef(GetCheckDetailsByBankAndRefParam _objGetCheckDetailsByBankAndRefParam,string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetCheckDetailsByBankAndRef(_objGetCheckDetailsByBankAndRefParam, ConnectionString);

            ListCheckDetailsByBankAndRef _lstCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();
            List<CheckDetailsByBankAndRefTable1> _lsttable1 = new List<CheckDetailsByBankAndRefTable1>();
            List<CheckDetailsByBankAndRefTable2> _lsttable2 = new List<CheckDetailsByBankAndRefTable2>();

            #region
            ////table 1
            //List<Table1> objList1 = new List<Table1>();
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    Table1 objTable1 = new Table1();

            //    objList1.Add(objTable1);
            //}
            //objlist.listTable1 = objList1;

            ////table 2
            //List<Table2> objList2 = new List<Table2>();
            //foreach (DataRow dr in ds.Tables[1].Rows)
            //{
            //    Table2 objTable2 = new Table2();

            //    objList2.Add(objTable2);
            //}
            //objlist.listTable1 = objList1;
            //objlist.listTable2 = objList2;
            #endregion

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lsttable1.Add(new CheckDetailsByBankAndRefTable1()
                {
                    Ref = (!string.IsNullOrEmpty(dr["Ref"].ToString())) ? dr["Ref"].ToString() : string.Empty,
                    InvoiceDate = Convert.ToDateTime(DBNull.Value.Equals(dr["InvoiceDate"]) ? null : dr["InvoiceDate"]),
                    Refrerence = (!string.IsNullOrEmpty(dr["Refrerence"].ToString())) ? dr["Refrerence"].ToString() : string.Empty,
                    Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                    Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                    AmountPay = Convert.ToDouble(DBNull.Value.Equals(dr["AmountPay"]) ? 0 : dr["AmountPay"]),
                    PayDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PayDate"]) ? null : dr["PayDate"]),
                    CheckNo = Convert.ToInt32(DBNull.Value.Equals(dr["CheckNo"]) ? 0 : dr["CheckNo"]),
                    Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                    VendorName = (!string.IsNullOrEmpty(dr["VendorName"].ToString())) ? dr["VendorName"].ToString() : string.Empty,
                    Type = (!string.IsNullOrEmpty(dr["Type"].ToString())) ? dr["Type"].ToString() : string.Empty,
                    Description = (!string.IsNullOrEmpty(dr["Description"].ToString())) ? dr["Description"].ToString() : string.Empty,
                });                
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lsttable2.Add(new CheckDetailsByBankAndRefTable2()
                {
                    Pay = Convert.ToDouble(DBNull.Value.Equals(dr["Pay"]) ? 0 : dr["Pay"]),
                    ToOrder = Convert.ToString(dr["ToOrder"]),
                    Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
                    CheckAmount = Convert.ToDouble(DBNull.Value.Equals(dr["CheckAmount"]) ? 0 : dr["CheckAmount"]),
                    ToOrerAddress = Convert.ToString(dr["ToOrerAddress"]),
                    State = Convert.ToString(dr["State"]),
                    Zip = Convert.ToString(dr["Zip"]),
                    VendorAddress = Convert.ToString(dr["VendorAddress"]),
                    RemitAddress = Convert.ToString(dr["RemitAddress"]),
                    Memo = Convert.ToString(dr["Memo"]),
                    Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                    CheckNo = Convert.ToInt32(DBNull.Value.Equals(dr["CheckNo"]) ? 0 : dr["CheckNo"]),
                    Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                });
            }

            _lstCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1 = _lsttable1;
            _lstCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2 = _lsttable2;


            return _lstCheckDetailsByBankAndRef;
        }
        public DataSet GetCDByID(CD _objCD)
        {
            return _objDL_Bills.GetCDByID(_objCD);
        }
        public DataSet GetRecurCDByID(CD _objCD)
        {
            return _objDL_Bills.GetRecurCDByID(_objCD);
        }
        public List<GetRecurCDByIDViewModel> GetRecurCDByID(GetRecurCDByIDParam _GetRecurCDByIDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetRecurCDByID(_GetRecurCDByIDParam, ConnectionString);

            List<GetRecurCDByIDViewModel> _lstGetRecurCDByIDViewModel = new List<GetRecurCDByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetRecurCDByIDViewModel.Add(
                    new GetRecurCDByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Bank = Convert.ToInt32(DBNull.Value.Equals(dr["Bank"]) ? 0 : dr["Bank"]),
                        Memo = Convert.ToString(dr["Memo"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        ACH = Convert.ToInt16(DBNull.Value.Equals(dr["ACH"]) ? 0 : dr["ACH"]),
                        BankName = Convert.ToString(dr["BankName"]),
                        Sel = Convert.ToInt32(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        IsRecon = Convert.ToBoolean(DBNull.Value.Equals(dr["IsRecon"]) ? false : dr["IsRecon"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        AcctNumber = Convert.ToString(dr["Acct#"]),
                        Type = Convert.ToString(dr["Type"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                        BillRef = Convert.ToString(dr["BillRef"]),
                        BillfDesc = Convert.ToString(dr["BillfDesc"]),
                        BillAmount = Convert.ToDouble(DBNull.Value.Equals(dr["BillAmount"]) ? 0 : dr["BillAmount"]),
                        BillUseTax = Convert.ToDouble(DBNull.Value.Equals(dr["BillUseTax"]) ? 0 : dr["BillUseTax"]),
                        Frequency = Convert.ToInt32(DBNull.Value.Equals(dr["Frequency"]) ? 0 : dr["Frequency"]),
                        PaymentType = Convert.ToInt32(DBNull.Value.Equals(dr["PaymentType"]) ? 0 : dr["PaymentType"]),
                    }
                    );
            }

            return _lstGetRecurCDByIDViewModel;
        }
        public List<GetCDByIDViewModel> GetCDByID(GetCDByIDParam _objGetCDByIDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetCDByID(_objGetCDByIDParam, ConnectionString);

            List<GetCDByIDViewModel> _lstGetCDByIDViewModel = new List<GetCDByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCDByIDViewModel.Add(
                    new GetCDByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Bank = Convert.ToInt32(DBNull.Value.Equals(dr["Bank"]) ? 0 : dr["Bank"]),
                        Memo = Convert.ToString(dr["Memo"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        ACH = Convert.ToInt16(DBNull.Value.Equals(dr["ACH"]) ? 0 : dr["ACH"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Type = Convert.ToString(dr["Type"]),
                        BankName = Convert.ToString(dr["BankName"]),
                        Sel = Convert.ToInt32(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        Acct = Convert.ToString(dr["Acct#"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        PaymentType = Convert.ToInt32(DBNull.Value.Equals(dr["PaymentType"]) ? 0 : dr["PaymentType"]),
                    }
                    );
            }

            return _lstGetCDByIDViewModel;

        }
        public DataSet GetDataTypeCD(CD _objCD)
        {
            return _objDL_Bills.GetDataTypeCD(_objCD);
        }

        public List<CDViewModel> GetDataTypeCD(GetDataTypeCDParam _objGetDataTypeCDParam,string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetDataTypeCD(_objGetDataTypeCDParam, ConnectionString);
            List<CDViewModel> _lstCDViewModel = new List<CDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCDViewModel.Add(
                    new CDViewModel()
                    {
                        COLUMN_NAME = Convert.ToString(dr["COLUMN_NAME"]),
                        DATA_TYPE = Convert.ToString(dr["DATA_TYPE"]),
                    }
                    );
            }

            return _lstCDViewModel;
        }
        public DataSet GetPaidDetailByID(Paid _objPaid)
        {
            return _objDL_Bills.GetPaidDetailByID(_objPaid);
        }
        public DataSet GetRecurrBillDetailByID(Paid _objPaid)
        {
            return _objDL_Bills.GetRecurrBillDetailByID(_objPaid);
        }
        public List<GetRecurrBillDetailByIDViewModel> GetRecurrBillDetailByID(GetRecurrBillDetailByIDParam _GetRecurrBillDetailByIDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetRecurrBillDetailByID(_GetRecurrBillDetailByIDParam, ConnectionString);

            List<GetRecurrBillDetailByIDViewModel> _lstGetRecurrBillDetailByIDViewModel = new List<GetRecurrBillDetailByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetRecurrBillDetailByIDViewModel.Add(
                    new GetRecurrBillDetailByIDViewModel()
                    {
                        rowid = Convert.ToInt32(DBNull.Value.Equals(dr["rowid"]) ? 0 : dr["rowid"]),
                        AcctID = Convert.ToInt32(DBNull.Value.Equals(dr["AcctID"]) ? 0 : dr["AcctID"]),
                        amount = Convert.ToString(dr["amount"]),
                        Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                        batch = Convert.ToInt16(DBNull.Value.Equals(dr["batch"]) ? 0 : dr["batch"]),
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        line = Convert.ToInt16(DBNull.Value.Equals(dr["line"]) ? 0 : dr["line"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        sel = Convert.ToInt16(DBNull.Value.Equals(dr["sel"]) ? 0 : dr["sel"]),
                        type = Convert.ToInt16(DBNull.Value.Equals(dr["type"]) ? 0 : dr["type"]),
                        PhaseID = Convert.ToInt16(DBNull.Value.Equals(dr["PhaseID"]) ? 0 : dr["PhaseID"]),
                        JobId = Convert.ToInt32(DBNull.Value.Equals(dr["JobId"]) ? 0 : dr["JobId"]),
                        strRef = Convert.ToString(dr["strRef"]),
                        AcctNo = Convert.ToString(dr["AcctNo"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        AcctName = Convert.ToString(dr["AcctName"]),
                        UseTax = Convert.ToString(dr["UseTax"]),
                        UtaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["UtaxGL"]) ? 0 : dr["UtaxGL"]),
                        UName = Convert.ToString(dr["UName"]),
                        jobName = Convert.ToString(dr["jobName"]),
                        phase = Convert.ToString(dr["phase"]),
                        loc = Convert.ToString(dr["loc"]),
                        ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                        ItemDesc = Convert.ToString(dr["ItemDesc"]),
                        TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"]) ? 0 : dr["TypeID"]),
                        Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                        OpSq = Convert.ToString(dr["OpSq"]),
                        PrvIn = Convert.ToDouble(DBNull.Value.Equals(dr["PrvIn"]) ? 0 : dr["PrvIn"]),
                        PrvInQuan = Convert.ToDouble(DBNull.Value.Equals(dr["PrvInQuan"]) ? 0 : dr["PrvInQuan"]),
                        OutstandQuan = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandQuan"]) ? 0 : dr["OutstandQuan"]),
                        OutstandBalance = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandBalance"]) ? 0 : dr["OutstandBalance"]),
                        STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        STaxRate = Convert.ToDecimal(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        STaxAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["STaxAmt"]) ? 0 : dr["STaxAmt"]),
                        STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                        GSTRate = Convert.ToDecimal(DBNull.Value.Equals(dr["GSTRate"]) ? 0 : dr["GSTRate"]),
                        GTaxAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["GTaxAmt"]) ? 0 : dr["GTaxAmt"]),
                        GSTTaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["GSTTaxGL"]) ? 0 : dr["GSTTaxGL"]),
                        STaxType = Convert.ToInt32(DBNull.Value.Equals(dr["STaxType"]) ? 0 : dr["STaxType"]),
                        UTaxType = Convert.ToInt32(DBNull.Value.Equals(dr["UTaxType"]) ? 0 : dr["UTaxType"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        WHLocID = Convert.ToInt32(DBNull.Value.Equals(dr["WHLocID"]) ? 0 : dr["WHLocID"]),
                        Warehousefdesc = Convert.ToString(dr["Warehousefdesc"]),
                        Locationfdesc = Convert.ToString(dr["Locationfdesc"]),
                    }
                    );
            }

            return _lstGetRecurrBillDetailByIDViewModel;
        }

        public List<PaidViewModel> GetPaidDetailByID(GetPaidDetailByIDParam _objGetPaidDetailByIDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetPaidDetailByID(_objGetPaidDetailByIDParam, ConnectionString);

            List<PaidViewModel> _lstPaidViewModel = new List<PaidViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstPaidViewModel.Add(
                    new PaidViewModel()
                    {
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                        PITR = Convert.ToInt32(DBNull.Value.Equals(dr["PITR"]) ? 0 : dr["PITR"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Paid1 = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        TBalance = Convert.ToDouble(DBNull.Value.Equals(dr["TBalance"]) ? 0 : dr["TBalance"]),
                    }
                    );
            }

            return _lstPaidViewModel;
        }
        public void UpdateCDDate(CD _objCD)
        {
            _objDL_Bills.UpdateCDDate(_objCD);
        }
        public void UpdateAPCDDate(CD _objCD)
        {
            _objDL_Bills.UpdateAPCDDate(_objCD);
        }
        public void UpdateAPCDDate(UpdateAPCDDateParam _UpdateAPCDDateParam, string ConnectionString)
        {
            _objDL_Bills.UpdateAPCDDate(_UpdateAPCDDateParam, ConnectionString);
        }
        public void UpdateAPCDVoidLog(CD _objCD)
        {
            _objDL_Bills.UpdateAPCDVoidLog(_objCD);
        }
        public void UpdateAPCDVoidLog(UpdateAPCDVoidLogParam _objUpdateAPCDVoidLogParam,string ConnectionString)
        {
            _objDL_Bills.UpdateAPCDVoidLog(_objUpdateAPCDVoidLogParam, ConnectionString);
        }

        public void UpdateCDVoid(CD _objCD)
        {
            _objDL_Bills.UpdateCDVoid(_objCD);
        }
        public void UpdateCDVoid(UpdateCDVoidParam _objUpdateCDVoidParam, string ConnectionString)
        {
            _objDL_Bills.UpdateCDVoid(_objUpdateCDVoidParam, ConnectionString);
        }
        public void UpdateCDVoidOpen(CD _objCD)
        {
            _objDL_Bills.UpdateCDVoidOpen(_objCD);
        }

        public void UpdateCDVoidOpen(UpdateCDVoidOpenParam _objUpdateCDVoidOpenParam,string ConnectionString)
        {
            _objDL_Bills.UpdateCDVoidOpen(_objUpdateCDVoidOpenParam, ConnectionString);
        }

        public DataSet GetOpenAPByPJID(OpenAP _objOpenAP)
        {
            return _objDL_Bills.GetOpenAPByPJID(_objOpenAP);
        }
        public List<OpenAPViewModel> GetOpenAPByPJID(GetOpenAPByPJIDParam _objGetOpenAPByPJIDParam,string ConnectionString)
        {
            DataSet ds= _objDL_Bills.GetOpenAPByPJID(_objGetOpenAPByPJIDParam, ConnectionString);

            List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstOpenAPViewModel.Add(
                    new OpenAPViewModel()
                    {
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Selected = Convert.ToDouble(DBNull.Value.Equals(dr["Selected"]) ? 0 : dr["Selected"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Ref = Convert.ToString(dr["Ref"]),
                    }
                    );
            }

            return _lstOpenAPViewModel;
        }
        public void DeleteOpenAPByPJID(OpenAP _objOpenAP)
        {
            _objDL_Bills.DeleteOpenAPByPJID(_objOpenAP);
        }
        public void UpdatePJOnVoidCheck(PJ _objPJ)
        {
            _objDL_Bills.UpdatePJOnVoidCheck(_objPJ);
        }
        public void UpdatePJOnVoidCheck(UpdatePJOnVoidCheckParam _objUpdatePJOnVoidCheckParam, string ConnectionString)
        {
            _objDL_Bills.UpdatePJOnVoidCheck(_objUpdatePJOnVoidCheckParam, ConnectionString);
        }
        public void UpdatePaidOnVoidCheck(Paid _objPaid)
        {
            _objDL_Bills.UpdatePaidOnVoidCheck(_objPaid);
        }
        public void UpdatePaidOnVoidCheck(UpdatePaidOnVoidCheckParam _objUpdatePaidOnVoidCheckParam,string ConnectionString)
        {
            _objDL_Bills.UpdatePaidOnVoidCheck(_objUpdatePaidOnVoidCheckParam, ConnectionString);
        }
        public DataSet GetJobIByTransID(JobI _objJobI)
        {
            return _objDL_Bills.GetJobIByTransID(_objJobI);
        }
        public List<JobIViewModel> GetJobIByTransID(GetJobIByTransIDParam _objGetJobIByTransIDParam,string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetJobIByTransID(_objGetJobIByTransIDParam, ConnectionString);

            List<JobIViewModel> _lstJobIViewModel = new List<JobIViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstJobIViewModel.Add(
                    new JobIViewModel()
                    {
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        Phase = Convert.ToInt16(DBNull.Value.Equals(dr["Phase"]) ? 0 : dr["Phase"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToInt32(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Labor = Convert.ToInt16(DBNull.Value.Equals(dr["Labor"]) ? 0 : dr["Labor"]),
                        Billed = Convert.ToInt32(DBNull.Value.Equals(dr["Billed"]) ? 0 : dr["Billed"]),
                        Invoice = Convert.ToInt32(DBNull.Value.Equals(dr["Invoice"]) ? 0 : dr["Invoice"]),
                        UseTax = Convert.ToInt32(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        APTicket = Convert.ToInt32(DBNull.Value.Equals(dr["APTicket"]) ? 0 : dr["APTicket"]),
                    }
                    );
            }

            return _lstJobIViewModel;
        }
        public DataSet GetCDByTransID(CD _objCD)
        {
            return _objDL_Bills.GetCDByTransID(_objCD);
        }
        public DataSet GetPJByTransID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJByTransID(_objPJ);
        }
        public void DeleteCheckDetails(CD _objCD)
        {
            _objDL_Bills.DeleteCheckDetails(_objCD);
        }
        public void DeleteCheckDetails(DeleteCheckDetailsParam _objDeleteCheckDetailsParam,string ConnectionString)
        {
            _objDL_Bills.DeleteCheckDetails(_objDeleteCheckDetailsParam, ConnectionString);
        }
        public void DeleteRecurrCheck(CD _objCD)
        {
            _objDL_Bills.DeleteRecurrCheck(_objCD);
        }
        public void DeleteRecurrCheck(DeleteRecurrCheckParam _objDeleteRecurrCheckParam,string ConnectionString)
        {
            _objDL_Bills.DeleteRecurrCheck(_objDeleteRecurrCheckParam, ConnectionString);
        }
        public DataSet GetPJByID(PJ _objPJ)
        {
            return _objDL_Bills.GetPJByID(_objPJ);
        }
        public void AddPJItem(PJ _objPJ)
        {
            _objDL_Bills.AddPJItem(_objPJ);
        }
        public void DeletePJItem(PJ _objPJ)
        {
            _objDL_Bills.DeletePJItem(_objPJ);
        }
        public void DeleteAPBill(PJ _objPJ)
        {
            _objDL_Bills.DeleteAPBill(_objPJ);
        }
        public void DeleteAPBill(DeleteAPBillParam _DeleteAPBillParam, string ConnectionString)
        {
            _objDL_Bills.DeleteAPBill(_DeleteAPBillParam, ConnectionString);
        }
        public void DeleteAPBillRecurr(PJ _objPJ)
        {
            _objDL_Bills.DeleteAPBillRecurr(_objPJ);
        }
        public void DeleteAPBillRecurr(DeleteAPBillRecurrParam _DeleteAPBillRecurrParam, string ConnectionString)
        {
            _objDL_Bills.DeleteAPBillRecurr(_DeleteAPBillRecurrParam, ConnectionString);
        }
        public DataSet GetProcessRecurrCount(PJ _objPJ)
        {
            return _objDL_Bills.GetProcessRecurrCount(_objPJ);
        }

        public List<CDViewModel> GetProcessRecurrCount(GetProcessRecurrCountParam _GetProcessRecurrCountParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetProcessRecurrCount(_GetProcessRecurrCountParam, ConnectionString);

            List<CDViewModel> _lstCDViewModel = new List<CDViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCDViewModel.Add(
                    new CDViewModel()
                    {
                        CountRecur = Convert.ToInt32(DBNull.Value.Equals(dr["CountRecur"]) ? 0 : dr["CountRecur"]),
                    }
                    );
            }
            return _lstCDViewModel;
        }
        public DataSet GetProcessRecurrCheckCount(CD _objCD)
        {
            return _objDL_Bills.GetProcessRecurrCheckCount(_objCD);
        }
        public List<CDViewModel> GetProcessRecurrCheckCount(GetProcessRecurrCheckCountParam _objGetProcessRecurrCheckCountParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetProcessRecurrCheckCount(_objGetProcessRecurrCheckCountParam, ConnectionString);
            List<CDViewModel> _lstCDViewModel = new List<CDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCDViewModel.Add(
                    new CDViewModel()
                    {
                        CountRecur = Convert.ToInt32(DBNull.Value.Equals(dr["CountRecur"]) ? 0 : dr["CountRecur"]),
                    }
                    );
            }

            return _lstCDViewModel;
        }

        public bool IsExistCheckNum(CD _objCD)
        {
            return _objDL_Bills.IsExistCheckNum(_objCD);
        }
        public bool IsExistCheckNum(IsExistCheckNumParam _IsExistCheckNumParam, string ConnectionString)
        {
            return _objDL_Bills.IsExistCheckNum(_IsExistCheckNumParam, ConnectionString);
        }
        public bool IsExistCheckNumOnEdit(CD _objCD)
        {
            return _objDL_Bills.IsExistCheckNumOnEdit(_objCD);
        }

        //API
        public bool IsExistCheckNumOnEdit(IsExistCheckNumOnEditParam _IsExistCheckNumOnEdit, string ConnectionString)
        {
            return _objDL_Bills.IsExistCheckNumOnEdit(_IsExistCheckNumOnEdit, ConnectionString);
        }

        public int GetBankID(CD _objCD)
        {
            return _objDL_Bills.GetBankID(_objCD);
        }
        public void UpdateCDCheckNo(CD _objCD)
        {
            _objDL_Bills.UpdateCDCheckNo(_objCD);
        }
        public void UpdateCDCheckNo(UpdateCDCheckNoParam _UpdateCDCheckNoParam, string ConnectionString)
        {
            _objDL_Bills.UpdateCDCheckNo(_UpdateCDCheckNoParam, ConnectionString);
        }
        public DataSet GetBillsDetailsByDue(PJ _objPJ)
        {
            return _objDL_Bills.GetBillsDetailsByDue(_objPJ);
        }
        public DataSet GetBillsDetailsByDate(PJ _objPJ)
        {
            return _objDL_Bills.GetBillsDetailsByDate(_objPJ);
        }
        public List<GetBillsDetailsByDueViewModel> GetBillsDetailsByDue(GetBillsDetailsByDueParam _GetBillsDetailsByDueParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetBillsDetailsByDue(_GetBillsDetailsByDueParam, ConnectionString);
            List<GetBillsDetailsByDueViewModel> _lstGetBillsDetailsByDue = new List<GetBillsDetailsByDueViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBillsDetailsByDue.Add(
                    new GetBillsDetailsByDueViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        VendorID = Convert.ToInt32(DBNull.Value.Equals(dr["VendorID"]) ? 0 : dr["VendorID"]),
                        Vendor = Convert.ToString(dr["Vendor"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        SevenDay2 = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                        ThirtyDay2 = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay2 = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                    }
                    );
            }

            return _lstGetBillsDetailsByDue;
        }
        public DataSet GetBillsDetails360ByDue(PJ _objPJ)
        {
            return _objDL_Bills.GetBillsDetails360ByDue(_objPJ);
        }

        //API
        public List<GetBillsDetails360ByDueViewModel> GetBillsDetails360ByDue(GetBillsDetails360ByDueParam _GetBillsDetails360ByDue, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetBillsDetails360ByDue(_GetBillsDetails360ByDue, ConnectionString);

            List<GetBillsDetails360ByDueViewModel> _lstGetBillsDetails360ByDue = new List<GetBillsDetails360ByDueViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBillsDetails360ByDue.Add(
                    new GetBillsDetails360ByDueViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        VendorID = Convert.ToInt32(DBNull.Value.Equals(dr["VendorID"]) ? 0 : dr["VendorID"]),
                        Vendor = Convert.ToString(dr["Vendor"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                        VoidR = Convert.ToString(dr["VoidR"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        DueIn = Convert.ToString(dr["DueIn"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        OverThreeSixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OverThreeSixtyDay"]) ? 0 : dr["OverThreeSixtyDay"]),
                        Status1 = Convert.ToInt16(DBNull.Value.Equals(dr["Status1"]) ? 0 : dr["Status1"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        ThreeSixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThreeSixtyDay"]) ? 0 : dr["ThreeSixtyDay"]),
                    }
                    );
            }

            return _lstGetBillsDetails360ByDue;
        }

        public DataSet GetAllPO(PO _objPO)
        {
            return _objDL_Bills.GetAllPO(_objPO);
        }
        public DataSet GetPOById(PO _objPO)
        {
            return _objDL_Bills.GetPOById(_objPO);
        }
        public DataSet GetListPO(PO _objPO, string pos)
        {
            return _objDL_Bills.GetListPO(_objPO, pos);
        }
        public DataSet GetPOByIdAjax(PO _objPO)
        {
            return _objDL_Bills.GetPOByIdAjax(_objPO);
        }
        public DataSet GetOutStandingPOById(PO _objPO)
        {
            return _objDL_Bills.GetOutStandingPOById(_objPO);
        }

        public ListGetOutStandingPOById GetOutStandingPOById(string ConnectionString, GetOutStandingPOByIdParam _GetOutStandingPOByIdParam)
        {
            DataSet ds = _objDL_Bills.GetOutStandingPOById(ConnectionString, _GetOutStandingPOByIdParam);

            ListGetOutStandingPOById _ds = new ListGetOutStandingPOById();
            List<GetOutStandingPOByIdTable1> _lsttable1 = new List<GetOutStandingPOByIdTable1>();
            List<GetOutStandingPOByIdTable2> _lsttable2 = new List<GetOutStandingPOByIdTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lsttable1.Add(new GetOutStandingPOByIdTable1()
                {
                    PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0.0000 : dr["Amount"]),
                    Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                    Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    ShipVia = Convert.ToString(dr["ShipVia"]),
                    PaymentTerms = Convert.ToInt16(DBNull.Value.Equals(dr["PaymentTerms"]) ? 0 : dr["PaymentTerms"]),
                    FOB = Convert.ToString(dr["FOB"]),
                    ShipTo = Convert.ToString(dr["ShipTo"]),
                    Approved = Convert.ToInt16(DBNull.Value.Equals(dr["Approved"]) ? 0 : dr["Approved"]),
                    Custom1 = Convert.ToString(dr["Custom1"]),
                    Custom2 = Convert.ToString(dr["Custom2"]),
                    ApprovedBy = Convert.ToString(dr["ApprovedBy"]),
                    ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                    fBy = Convert.ToString(dr["fBy"]),
                    PORevision = Convert.ToString(dr["PORevision"]),
                    CourrierAcct = Convert.ToString(dr["CourrierAcct"]),
                    POReasonCode = Convert.ToString(dr["POReasonCode"]),
                    VendorName = Convert.ToString(dr["VendorName"]),
                    Address = Convert.ToString(dr["Address"]),
                    VendorCity = Convert.ToString(dr["VendorCity"]),
                    VendorAddress = Convert.ToString(dr["VendorAddress"]),
                    VendorState = Convert.ToString(dr["VendorState"]),
                    VendorZip = Convert.ToString(dr["VendorZip"]),
                    Terms = Convert.ToString(dr["Terms"]),
                    State = Convert.ToString(dr["State"]),
                    StatusName = Convert.ToString(dr["StatusName"]),
                    TC = Convert.ToString(dr["TC"]),
                    Days = Convert.ToInt16(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                    Term = Convert.ToInt32(DBNull.Value.Equals(dr["Term"]) ? 0 : dr["Term"]),
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lsttable2.Add(new GetOutStandingPOByIdTable2()
                {
                    RowID = Convert.ToInt32(DBNull.Value.Equals(dr["RowID"]) ? 0 : dr["RowID"]),
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                    AcctID = Convert.ToInt32(DBNull.Value.Equals(dr["AcctID"]) ? 0 : dr["AcctID"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    TotalQuan = Convert.ToDouble(DBNull.Value.Equals(dr["TotalQuan"]) ? 0 : dr["TotalQuan"]),
                    Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                    TotalAmount = Convert.ToDouble(DBNull.Value.Equals(dr["TotalAmount"]) ? 0 : dr["TotalAmount"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    //Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                    Quan = Convert.ToString(dr["Quan"]),
                    JobID = Convert.ToInt32(DBNull.Value.Equals(dr["JobID"]) ? 0 : dr["JobID"]),
                    JobName = Convert.ToString(dr["JobName"]),
                    PhaseID = Convert.ToInt16(DBNull.Value.Equals(dr["PhaseID"]) ? 0 : dr["PhaseID"]),
                    Phase = Convert.ToString(dr["Phase"]),
                    Inv = Convert.ToInt32(DBNull.Value.Equals(dr["Inv"]) ? 0 : dr["Inv"]),
                    Freight = Convert.ToDouble(DBNull.Value.Equals(dr["Freight"]) ? 0.00 : dr["Freight"]),
                    Rquan = Convert.ToDouble(DBNull.Value.Equals(dr["Rquan"]) ? 0.00 : dr["Rquan"]),
                    Billed = Convert.ToInt32(DBNull.Value.Equals(dr["Billed"]) ? 0 : dr["Billed"]),
                    //Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                    Ticket = Convert.ToString(dr["Ticket"]),
                    Loc = Convert.ToString(dr["Loc"]),
                    AcctNo = Convert.ToString(dr["AcctNo"]),
                    Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                    Usetax = Convert.ToDouble(DBNull.Value.Equals(dr["Usetax"]) ? 0 : dr["Usetax"]),
                    UName = Convert.ToString(dr["UName"]),
                    UtaxGL = Convert.ToString(dr["UtaxGL"]),
                    ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                    ItemDesc = Convert.ToString(dr["ItemDesc"]),
                    TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"]) ? 0 : dr["TypeID"]),
                    LocName = Convert.ToString(dr["LocName"]),
                    LocationID = Convert.ToInt32(DBNull.Value.Equals(dr["LocationID"]) ? 0 : dr["LocationID"]),
                    WarehouseID = Convert.ToString(dr["WarehouseID"]),
                    OpSq = Convert.ToString(dr["OpSq"]),
                    OutstandBalance = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandBalance"]) ? 0 : dr["OutstandBalance"]),
                    OutstandQuan = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandQuan"]) ? 0 : dr["OutstandQuan"]),
                    PrvIn = Convert.ToDouble(DBNull.Value.Equals(dr["PrvIn"]) ? 0 : dr["PrvIn"]),
                    PrvInQuan = Convert.ToDouble(DBNull.Value.Equals(dr["PrvInQuan"]) ? 0 : dr["PrvInQuan"]),
                    STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? false : dr["STax"]),
                    STaxName = Convert.ToString(dr["STaxName"]),
                    STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0.0000 : dr["STaxRate"]),
                    STaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["STaxAmt"]) ? 0.0000 : dr["STaxAmt"]),
                    STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                    GSTRate = Convert.ToDouble(DBNull.Value.Equals(dr["GSTRate"]) ? 0.0000 : dr["GSTRate"]),
                    GSTTaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["GSTTaxGL"]) ? 0 : dr["GSTTaxGL"]),
                    GTaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["GTaxAmt"]) ? 0.0000 : dr["GTaxAmt"]),
                    Warehouse = Convert.ToString(dr["Warehouse"]),
                    WHLocID = Convert.ToInt32(DBNull.Value.Equals(dr["WHLocID"]) ? 0 : dr["WHLocID"]),
                    Warehousefdesc = Convert.ToString(dr["Warehousefdesc"]),
                    Locationfdesc = Convert.ToString(dr["Locationfdesc"]),
                });
            }

            _ds.lstTable1 = _lsttable1;
            _ds.lstTable2 = _lsttable2;

            return _ds;
        }
        public DataSet GetPOApproveDetails(PO _objPO)
        {
            return _objDL_Bills.GetPOApproveDetails(_objPO);
        }
        public DataSet POApproveDetails(ApprovePOStatus _objApprovePOStatus)
        {
            return _objDL_Bills.POApproveDetails(_objApprovePOStatus);
        }
        public DataSet GetPODetailsForMailALL(ApprovePOStatus _objApprovePOStatus)
        {
            return _objDL_Bills.GetPODetailsForMailALL(_objApprovePOStatus);
        }
        public DataSet GetVenderDetailsForMailALL(ApprovePOStatus _objApprovePOStatus)
        {
            return _objDL_Bills.GetVenderDetailsForMailALL(_objApprovePOStatus);
        }
        public DataSet GetPOSign(ApprovePOStatus _objApprovePOStatus)
        {
            return _objDL_Bills.GetPOSign(_objApprovePOStatus);
        }
        public DataSet GetPOByTicketId(PO _objPO)
        {
            return _objDL_Bills.GetPOByTicketId(_objPO);
        }
        public void DeletePOById(PO _objPO)
        {
            _objDL_Bills.DeletePOById(_objPO);
        }
        public void AddPO(PO _objPO)
        {
            _objDL_Bills.AddPO(_objPO);
        }
        public void UpdatePO(PO _objPO)
        {
            _objDL_Bills.UpdatePO(_objPO);
        }
        public int GetMaxPOId(PO _objPO)
        {
            return _objDL_Bills.GetMaxPOId(_objPO);
        }
        public bool IsFirstPo(PO _objPO)
        {
            return _objDL_Bills.IsFirstPo(_objPO);
        }
        public void UpdatePOBalance(PO _objPO)
        {
            _objDL_Bills.UpdatePOBalance(_objPO);
        }
        public DataSet GetPOByVendor(PO _objPO)
        {
            return _objDL_Bills.GetPOByVendor(_objPO);
        }
        public DataSet GetPOItemByPO(PO _objPO)
        {
            return _objDL_Bills.GetPOItemByPO(_objPO);
        }
        public void UpdatePOStatusById(PO _objPO)
        {
            _objDL_Bills.UpdatePOStatusById(_objPO);
        }
        public DataSet GetAddPOTerms(PO _objPO)
        {
            return _objDL_Bills.GetAddPOTerms(_objPO);
        }
        public bool IsBillExistForInsert(PJ _objPJ)
        {
            return _objDL_Bills.IsBillExistForInsert(_objPJ);
        }
        public bool IsBillRecurrExistForInsert(PJ _objPJ)
        {
            return _objDL_Bills.IsBillRecurrExistForInsert(_objPJ);
        }
        
        public bool IsBillExistForEdit(PJ _objPJ)
        {
            return _objDL_Bills.IsBillExistForEdit(_objPJ);
        }
        public bool IsBillRecurrExistForEdit(PJ _objPJ)
        {
            return _objDL_Bills.IsBillRecurrExistForEdit(_objPJ);
        }
        public int GetMaxReceivePOId(PO _objPO)
        {
            return _objDL_Bills.GetMaxReceivePOId(_objPO);
        }
        public int GetMaxReceivePOId(string ConnectionString, GetMaxReceivePOIdParam _GetMaxReceivePOIdParam)
        {
            int ID_NEW = _objDL_Bills.GetMaxReceivePOId(ConnectionString, _GetMaxReceivePOIdParam);
            return ID_NEW;
        }
        public void AddReceivePO(PO _objPO)
        {
            _objDL_Bills.AddReceivePO(_objPO);
        }
        public void UpdatePOStatus(PO _objPO)
        {
            _objDL_Bills.UpdatePOStatus(_objPO);
        }
        public void UpdatePOStatus(string ConnectionString, UpdatePOStatusParam _UpdatePOStatusParam)
        {
            _objDL_Bills.UpdatePOStatus(ConnectionString, _UpdatePOStatusParam);
        }
        public void UpdatePOItemBalance(PO _objPO)
        {
            _objDL_Bills.UpdatePOItemBalance(_objPO);
        }
        public void UpdatePOItemBalance(string ConnectionString, UpdatePOItemBalanceParam _UpdatePOItemBalanceParam)
        {
            _objDL_Bills.UpdatePOItemBalance(ConnectionString, _UpdatePOItemBalanceParam);
        }
        public void AddReceiveInventoryWHTrans(InventoryWHTrans obj)
        {
            _objDL_Bills.AddReceiveInventoryWHTrans(obj);
        }
        public void AddReceiveInventoryWHTrans(AddReceiveInventoryWHTransParam _AddReceiveInventoryWHTrans, string ConnectionString)
        {
            _objDL_Bills.AddReceiveInventoryWHTrans(_AddReceiveInventoryWHTrans, ConnectionString);
        }


        public void ReverseReceivePOInvetoryItem(int RPOID, string conString,string userid)
        {
            _objDL_Bills.ReverseReceivePOInvetoryItem(RPOID, conString,userid);
        }


        public void AddReceivePOItem(PO _objPO)
        {
              _objDL_Bills.AddReceivePOItem(_objPO);
        }
        public void AddReceivePOItem(string ConnectionString, AddReceivePOItemParam _AddReceivePOItemParam)
        {
            _objDL_Bills.AddReceivePOItem(ConnectionString, _AddReceivePOItemParam);
        }
        public DataSet GetAllReceivePO(PO _objPO)
        {
            return _objDL_Bills.GetAllReceivePO(_objPO);
        }
        public DataSet GetReceivePoById(PO _objPO)
        {
            return _objDL_Bills.GetReceivePoById(_objPO);
        }
        public DataSet GetListReceivePO(PO _objPO)
        {
            return _objDL_Bills.GetListReceivePO(_objPO);
        }
        public DataSet GetListReceivePOBySearch(PO _objPO)
        {
            return _objDL_Bills.GetListReceivePOBySearch(_objPO);
        }
        public DataSet GetListReceivePOProjectBySearch(PO _objPO, List<RetainFilter> filters)
        {
            return _objDL_Bills.GetListReceivePOProjectBySearch(_objPO, filters);
        }
        public DataSet GetListReceivePOBySearchByID(PO _objPO)
        {
            return _objDL_Bills.GetListReceivePOBySearchByID(_objPO);
        }
        public DataTable GetAllPOByDue(PO _objPO)
        {
            return _objDL_Bills.GetAllPOByDue(_objPO);
        }
        public void UpdatePOItemQuan(PO _objPO)
        {
            _objDL_Bills.UpdatePOItemQuan(_objPO);
        }
        public void UpdatePOItemQuan(string ConnectionString, UpdatePOItemQuanParam _UpdatePOItemQuanParam)
        {
            _objDL_Bills.UpdatePOItemQuan(ConnectionString, _UpdatePOItemQuanParam);
        }
        public void UpdatePOItemWarehouseLocation(PO _objPO)
        {
            _objDL_Bills.UpdatePOItemWarehouseLocation(_objPO);
        }
        public void UpdatePOItemWarehouseLocation(UpdatePOItemWarehouseLocationParam _UpdatePOItemWarehouseLocationParam, string ConnectionString)
        {
            _objDL_Bills.UpdatePOItemWarehouseLocation(_UpdatePOItemWarehouseLocationParam, ConnectionString);
        }
        public bool IsClosedPO(PO _objPO)
        {
            return _objDL_Bills.IsClosedPO(_objPO);
        }
        public bool IsExistRPOForInsert(PO objPO)
        {
            return _objDL_Bills.IsExistRPOForInsert(objPO);
        }
        public DataSet GetPOList(PO objPO)
        {
            return _objDL_Bills.GetPOList(objPO);
        }
        public DataSet GetOpsqList(PO objPO)
        {
            return _objDL_Bills.GetOpsqList(objPO);
        }
        public DataSet GetReceivePOList(PO objPO)
        {
            return _objDL_Bills.GetReceivePOList(objPO);
        }
        public DataSet GetReceivePOListSearch(PO objPO)
        {
            return _objDL_Bills.GetReceivePOListSearch(objPO);
        }

        public List<GetReceivePOListViewModel> GetReceivePOList(string ConnectionString, GetReceivePOListParam _GetReceivePOListParam)
        {
            DataSet ds = _objDL_Bills.GetReceivePOList(ConnectionString, _GetReceivePOListParam);
            List<GetReceivePOListViewModel> _lstGetReceivePOList = new List<GetReceivePOListViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetReceivePOList.Add(
                    new GetReceivePOListViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Value = Convert.ToInt32(DBNull.Value.Equals(dr["Value"]) ? 0 : dr["Value"]),
                        ReceivedAmount = Convert.ToDouble(DBNull.Value.Equals(dr["ReceivedAmount"]) ? 0 : dr["ReceivedAmount"]),
                        ReceiveDate = Convert.ToDateTime(DBNull.Value.Equals(dr["ReceiveDate"]) ? null : dr["ReceiveDate"]),
                    }
                    );
            }
            return _lstGetReceivePOList;
        }
        public DataSet GetPOReceivePOById(PO objPO)
        {
            return _objDL_Bills.GetPOReceivePOById(objPO);
        }
        public ListGetPOReceivePOById GetPOReceivePOById(string ConnectionString, GetPOReceivePOByIdParam _GetPOReceivePOByIdParam)
        {
            DataSet ds = _objDL_Bills.GetPOReceivePOById(ConnectionString, _GetPOReceivePOByIdParam);

            ListGetPOReceivePOById _ds = new ListGetPOReceivePOById();
            List<GetPOReceivePOByIdTable1> _lsttable1 = new List<GetPOReceivePOByIdTable1>();
            List<GetPOReceivePOByIdTable2> _lsttable2 = new List<GetPOReceivePOByIdTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lsttable1.Add(new GetPOReceivePOByIdTable1()
                {
                    PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                    Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                    ShipVia = Convert.ToString(dr["ShipVia"]),
                    Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                    FOB = Convert.ToString(dr["FOB"]),
                    ShipTo = Convert.ToString(dr["ShipTo"]),
                    Approved = Convert.ToInt16(DBNull.Value.Equals(dr["Approved"]) ? 0 : dr["Approved"]),
                    Custom1 = Convert.ToString(dr["Custom1"]),
                    Custom2 = Convert.ToString(dr["Custom2"]),
                    ApprovedBy = Convert.ToString(dr["ApprovedBy"]),
                    ReqBy = Convert.ToInt32(DBNull.Value.Equals(dr["ReqBy"]) ? 0 : dr["ReqBy"]),
                    fBy = Convert.ToString(dr["fBy"]),
                    PORevision = Convert.ToString(dr["PORevision"]),
                    CourrierAcct = Convert.ToString(dr["CourrierAcct"]),
                    POReasonCode = Convert.ToString(dr["POReasonCode"]),
                    VendorName = Convert.ToString(dr["VendorName"]),
                    Address = Convert.ToString(dr["Address"]),
                    Days = Convert.ToInt16(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                    Term = Convert.ToInt32(DBNull.Value.Equals(dr["Term"]) ? 0 : dr["Term"]),
                    Comments = Convert.ToString(dr["Comments"]),
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    ReceivedAmount = Convert.ToDouble(DBNull.Value.Equals(dr["ReceivedAmount"]) ? 0 : dr["ReceivedAmount"]),
                    ReceiveDate = Convert.ToDateTime(DBNull.Value.Equals(dr["ReceiveDate"]) ? null : dr["ReceiveDate"]),
                    Ref = Convert.ToString(dr["Ref"]),
                    WB = Convert.ToString(dr["WB"]),
                    State = Convert.ToString(dr["State"]),
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lsttable2.Add(new GetPOReceivePOByIdTable2()
                {
                    RowID = Convert.ToInt32(DBNull.Value.Equals(dr["RowID"]) ? 0 : dr["RowID"]),
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    AcctID = Convert.ToInt32(DBNull.Value.Equals(dr["AcctID"]) ? 0 : dr["AcctID"]),
                    AcctNo = Convert.ToString(dr["AcctNo"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    GSTRate = Convert.ToDouble(DBNull.Value.Equals(dr["GSTRate"]) ? 0 : dr["GSTRate"]),
                    GSTTaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["GSTTaxGL"]) ? 0 : dr["GSTTaxGL"]),
                    GTaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["GTaxAmt"]) ? 0 : dr["GTaxAmt"]),
                    ItemDesc = Convert.ToString(dr["ItemDesc"]),
                    ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                    JobID = Convert.ToInt32(DBNull.Value.Equals(dr["JobID"]) ? 0 : dr["JobID"]),
                    JobName = Convert.ToString(dr["JobName"]),
                    Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                    Loc = Convert.ToString(dr["Loc"]),
                    Locationfdesc = Convert.ToString(dr["Locationfdesc"]),
                    OpSq = Convert.ToString(dr["OpSq"]),
                    OutstandBalance = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandBalance"]) ? 0 : dr["OutstandBalance"]),
                    OutstandQuan = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandQuan"]) ? 0 : dr["OutstandQuan"]),
                    PhaseID = Convert.ToInt16(DBNull.Value.Equals(dr["PhaseID"]) ? 0 : dr["PhaseID"]),
                    Phase = Convert.ToString(dr["Phase"]),
                    PrvIn = Convert.ToDouble(DBNull.Value.Equals(dr["PrvIn"]) ? 0 : dr["PrvIn"]),
                    PrvInQuan = Convert.ToDouble(DBNull.Value.Equals(dr["PrvInQuan"]) ? 0 : dr["PrvInQuan"]),
                    Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                    STaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["STaxAmt"]) ? 0 : dr["STaxAmt"]),
                    Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                    TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"]) ? 0 : dr["TypeID"]),
                    UName = Convert.ToString(dr["UName"]),
                    Usetax = Convert.ToDouble(DBNull.Value.Equals(dr["Usetax"]) ? 0 : dr["Usetax"]),
                    UtaxGL = Convert.ToString(dr["UtaxGL"]),
                    STaxName = Convert.ToString(dr["STaxName"]),
                    Warehouse = Convert.ToString(dr["Warehouse"]),
                    STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                    Warehousefdesc = Convert.ToString(dr["Warehousefdesc"]),
                    STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? false : dr["STax"]),
                    STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                    WHLocID = Convert.ToInt32(DBNull.Value.Equals(dr["WHLocID"]) ? 0 : dr["WHLocID"]),
                    Account = Convert.ToString(dr["Account"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                    ReceivePO = Convert.ToInt32(DBNull.Value.Equals(dr["ReceivePO"]) ? 0 : dr["ReceivePO"]),
                });
            }
            
            _ds.lstTable1 = _lsttable1;
            _ds.lstTable2 = _lsttable2;

            return _ds;
        }
        public String GetClosePOCheck(PO objPO)
        {
            return _objDL_Bills.GetClosePOCheck(objPO);
        }
        public String GetClosePOCheck(string ConnectionString, GetClosePOCheckParam _GetClosePOCheckParam)
        {
            string Retval = _objDL_Bills.GetClosePOCheck(ConnectionString, _GetClosePOCheckParam);
            return Retval;
        }
        public string AddBills(PJ objPJ)
        {
            return _objDL_Bills.AddBills(objPJ);
        }
        public string AddRemitTaxBills(PJ objPJ)
        {
            return _objDL_Bills.AddRemitTaxBills(objPJ);
        }

        public string AddBills(AddBillsParam _AddBillsParam, string ConnectionString)
        {
            //string strpjid = _objDL_Bills.AddBills(_AddBillsParam, ConnectionString);
            // return strpjid;
            return _objDL_Bills.AddBills(_AddBillsParam, ConnectionString);
        }
        public string UpdateRecurrBills(PJ objPJ)
        {
            return _objDL_Bills.UpdateRecurrBills(objPJ);
        }
        public string UpdateRecurrBills(UpdateRecurrBillsParam _UpdateRecurrBillsParam, string ConnectionString)
        {
            string returnVal = _objDL_Bills.UpdateRecurrBills(_UpdateRecurrBillsParam, ConnectionString);
            return returnVal;
        }
        public int ProcessRecurBill(PJ objPJ)
        {
            return _objDL_Bills.ProcessRecurBill(objPJ);
        }
        public int ProcessRecurBill(ProcessRecurBillParam _ProcessRecurBillParam, string ConnectionString)
        {
            int ID = _objDL_Bills.ProcessRecurBill(_ProcessRecurBillParam, ConnectionString);
            return ID;

        }
        public int ProcessRecurCheck(CD objCD)
        {
            return _objDL_Bills.ProcessRecurCheck(objCD);
        }
        public int ProcessRecurCheck(ProcessRecurCheckParam objProcessRecurCheckParam,string ConnectionString)
        {
            int RecurCount = _objDL_Bills.ProcessRecurCheck(objProcessRecurCheckParam, ConnectionString);
            return RecurCount;
        }
        public void UpdateBills(PJ objPJ)
        {
            _objDL_Bills.UpdateBills(objPJ);
        }
        public void UpdateBills(UpdateBillsParam _UpdateBillsParam, string ConnectionString)
        {
            _objDL_Bills.UpdateBills(_UpdateBillsParam, ConnectionString);
        }
        public void UpdateReceivePOStatus(PO objPO)
        {
            _objDL_Bills.UpdateReceivePOStatus(objPO);
        }

        public void UpdateReceivePOStatus(string ConnectionString, UpdateReceivePOStatusParam _UpdateReceivePOStatusParam)
        {
            _objDL_Bills.UpdateReceivePOStatus(ConnectionString, _UpdateReceivePOStatusParam);
        }
        public void UpdateReceivePOStatusByPOID(PO objPO)
        {
            _objDL_Bills.UpdateReceivePOStatusByPOID(objPO);
        }
        public void UpdateReceivePOStatusByPOID(string ConnectionString, UpdateReceivePOStatusByPOIDParam _UpdateReceivePOStatusByPOIDParam)
        {
            _objDL_Bills.UpdateReceivePOStatusByPOID(ConnectionString, _UpdateReceivePOStatusByPOIDParam);
        }
        public bool IsExistPO(PO objPO)
        {
            return _objDL_Bills.IsExistPO(objPO);
        }
        public bool IsExistPO(string ConnectionString, IsExistPOParam _IsExistPOParam)
        {
            return _objDL_Bills.IsExistPO(ConnectionString, _IsExistPOParam);
        }
        public DataSet GetAllPOAjaxSearch(PO _objPO)
        {
            return _objDL_Bills.GetAllPOAjaxSearch(_objPO);
        }
        public DataSet GetAllPOAjaxSearchSP(PO _objPO)
        {
            return _objDL_Bills.GetAllPOAjaxSearchSP(_objPO);
        }
        public DataSet GetPOItemInfoAjaxSearch(PO _objPO)
        {
            return _objDL_Bills.GetPOItemInfoAjaxSearch(_objPO);
        }
        public DataSet GetAPExpenses(Vendor objVendor)
        {
            return _objDL_Bills.GetAPExpenses(objVendor);
        }
        public ListGetAPExpenses GetAPExpenses(GetAPExpensesParam _GetAPExpensesParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAPExpenses(_GetAPExpensesParam, ConnectionString);

            ListGetAPExpenses _ds = new ListGetAPExpenses();
            List<GetAPExpensesTable1> _lstTable1 = new List<GetAPExpensesTable1>();
            List<GetAPExpensesTable2> _lstTable2 = new List<GetAPExpensesTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetAPExpensesTable1()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        RunTotal = Convert.ToDouble(DBNull.Value.Equals(dr["RunTotal"]) ? 0 : dr["RunTotal"]),
                        Type = Convert.ToString(dr["Type"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        Debit = Convert.ToDouble(DBNull.Value.Equals(dr["Debit"]) ? 0 : dr["Debit"]),
                        Credit = Convert.ToDouble(DBNull.Value.Equals(dr["Credit"]) ? 0 : dr["Credit"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                   new GetAPExpensesTable2()
                   {
                       Column1 = Convert.ToDouble(DBNull.Value.Equals(dr["Column1"]) ? 0 : dr["Column1"]),
                   }
                   );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public void UpdatePODue(PO objPO)
        {
            _objDL_Bills.UpdatePODue(objPO);
        }
        //Rahil's Implementation
        public DataSet GetBankCD(Bank _objBank)
        {
            return _objDL_Bills.GetAllBankCD(_objBank);
        }
        public List<BankViewModel> GetBankCD(GetBankCDParam _GetBankCDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAllBankCD(_GetBankCDParam, ConnectionString);
            List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstBankViewModel.Add(
                    new BankViewModel()
                    {
                        RolName = Convert.ToString(dr["Name"]),
                        RolAddress = Convert.ToString(dr["Address"]),
                        RolState = Convert.ToString(dr["State"]),
                        RolCity = Convert.ToString(dr["City"]),
                        RolZip = Convert.ToString(dr["Zip"]),
                        NBranch = Convert.ToString(dr["NBranch"]),
                        NAcct = Convert.ToString(dr["NAcct"]),
                        NRoute = Convert.ToString(dr["NRoute"]),
                    }
                    );
            }

            return _lstBankViewModel;
        }
        public DataSet GetCheckByPaidBill(PJ objPJ)
        {
            return _objDL_Bills.GetCheckByPaidBill(objPJ);
        }
        public DataSet GetBillTransDetails(PJ objPJ)
        {
            return _objDL_Bills.GetBillTransDetails(objPJ);
        }
        public List<GetBillTransDetailsViewModel> GetBillTransDetails(GetBillTransDetailsParam _GetBillTransDetailsParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetBillTransDetails(_GetBillTransDetailsParam, ConnectionString);
            List<GetBillTransDetailsViewModel> _lstGetBillTransDetails = new List<GetBillTransDetailsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBillTransDetails.Add(
                    new GetBillTransDetailsViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        AcctID = Convert.ToString(dr["AcctID"]),
                        AcctName = Convert.ToString(dr["AcctName"]),
                        AcctNo = Convert.ToString(dr["AcctNo"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToString(dr["Amount"]),
                        GSTRate = Convert.ToString(dr["GSTRate"]),
                        GSTTaxGL = Convert.ToString(dr["GSTTaxGL"]),
                        GTaxAmt = Convert.ToString(dr["GTaxAmt"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        IsPO = Convert.ToInt32(DBNull.Value.Equals(dr["IsPO"]) ? 0 : dr["IsPO"]),
                        ItemDesc= Convert.ToString(dr["ItemDesc"]),
                        ItemID = Convert.ToString(dr["ItemID"]),
                        JobId = Convert.ToInt32(DBNull.Value.Equals(dr["JobId"]) ? 0 : dr["JobId"]),
                        jobName = Convert.ToString(dr["jobName"]),
                        line = Convert.ToString(dr["line"]),
                        loc = Convert.ToString(dr["loc"]),
                        Locationfdesc = Convert.ToString(dr["Locationfdesc"]),
                        OpSq = Convert.ToString(dr["OpSq"]),
                        OutstandBalance = Convert.ToString(dr["OutstandBalance"]),
                        OutstandQuan = Convert.ToString(dr["OutstandQuan"]),
                        PhaseID = Convert.ToString(dr["PhaseID"]),
                        phase = Convert.ToString(dr["phase"]),
                        PrvIn = Convert.ToString(dr["PrvIn"]),
                        PrvInQuan = Convert.ToString(dr["PrvInQuan"]),
                        Quan = Convert.ToString(dr["Quan"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        Sel = Convert.ToString(dr["Sel"]),
                        STaxAmt = Convert.ToString(dr["STaxAmt"]),
                        STaxType = Convert.ToString(dr["STaxType"]),
                        strRef = Convert.ToString(dr["strRef"]),
                        Ticket = Convert.ToString(dr["Ticket"]),
                        Type = Convert.ToString(dr["Type"]),
                        TypeID = Convert.ToString(dr["TypeID"]),
                        UName = Convert.ToString(dr["UName"]),
                        UseTax = Convert.ToString(dr["UseTax"]),
                        UtaxGL = Convert.ToString(dr["UtaxGL"]),
                        UTaxType = Convert.ToString(dr["UTaxType"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        STaxGL = Convert.ToString(dr["STaxGL"]),
                        Warehousefdesc = Convert.ToString(dr["Warehousefdesc"]),
                        STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        STaxRate = Convert.ToString(dr["STaxRate"]),
                        WHLocID = Convert.ToString(dr["WHLocID"]),
                        GTax = Convert.ToInt16(DBNull.Value.Equals(dr["GTax"]) ? 0 : dr["GTax"]),
                    }
                    );

            }
            return _lstGetBillTransDetails;
        }
        public DataSet GetBillRecurrTransactions(PJ objPJ)
        {
            return _objDL_Bills.GetBillRecurrTransactions(objPJ);
        }
        public List<GetBillRecurrTransactionsViewModel> GetBillRecurrTransactions(GetBillRecurrTransactionsParam _GetBillRecurrTransactionsParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetBillRecurrTransactions(_GetBillRecurrTransactionsParam, ConnectionString);

            List<GetBillRecurrTransactionsViewModel> _lstGetBillRecurrTrans = new List<GetBillRecurrTransactionsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBillRecurrTrans.Add(
                    new GetBillRecurrTransactionsViewModel()
                    {
                        rowid = Convert.ToInt32(DBNull.Value.Equals(dr["rowid"]) ? 0 : dr["rowid"]),
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        AcctID = Convert.ToInt32(DBNull.Value.Equals(dr["AcctID"]) ? 0 : dr["AcctID"]),
                        AcctName = Convert.ToString(dr["AcctName"]),
                        AcctNo = Convert.ToString(dr["AcctNo"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        amount = Convert.ToString(dr["amount"]),
                        GSTRate = Convert.ToDouble(DBNull.Value.Equals(dr["GSTRate"]) ? 0 : dr["GSTRate"]),
                        GSTTaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["GSTTaxGL"]) ? 0 : dr["GSTTaxGL"]),
                        GTaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["GTaxAmt"]) ? 0 : dr["GTaxAmt"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        IsPO = Convert.ToInt32(DBNull.Value.Equals(dr["IsPO"]) ? 0 : dr["IsPO"]),
                        ItemDesc = Convert.ToString(dr["ItemDesc"]),
                        ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                        JobId = Convert.ToInt32(DBNull.Value.Equals(dr["JobId"]) ? 0 : dr["JobId"]),
                        jobName = Convert.ToString(dr["jobName"]),
                        line = Convert.ToInt16(DBNull.Value.Equals(dr["line"]) ? 0 : dr["line"]),
                        loc = Convert.ToString(dr["loc"]),
                        Locationfdesc = Convert.ToString(dr["Locationfdesc"]),
                        OpSq = Convert.ToString(dr["OpSq"]),
                        OutstandBalance = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandBalance"]) ? 0 : dr["OutstandBalance"]),
                        OutstandQuan = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandQuan"]) ? 0 : dr["OutstandQuan"]),
                        PhaseID = Convert.ToInt16(DBNull.Value.Equals(dr["PhaseID"]) ? 0 : dr["PhaseID"]),
                        phase = Convert.ToString(dr["phase"]),
                        PrvIn = Convert.ToDouble(DBNull.Value.Equals(dr["PrvIn"]) ? 0 : dr["PrvIn"]),
                        PrvInQuan = Convert.ToDouble(DBNull.Value.Equals(dr["PrvInQuan"]) ? 0 : dr["PrvInQuan"]),
                        Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        sel = Convert.ToInt16(DBNull.Value.Equals(dr["sel"]) ? 0 : dr["sel"]),
                        STaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["STaxAmt"]) ? 0 : dr["STaxAmt"]),
                        STaxType = Convert.ToInt32(DBNull.Value.Equals(dr["STaxType"]) ? 0 : dr["STaxType"]),
                        strRef = Convert.ToString(dr["strRef"]),
                        Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                        type = Convert.ToInt16(DBNull.Value.Equals(dr["type"]) ? 0 : dr["type"]),
                        TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"]) ? 0 : dr["TypeID"]),
                        UName = Convert.ToString(dr["UName"]),
                        UseTax = Convert.ToString(dr["UseTax"]),
                        UtaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["UtaxGL"]) ? 0 : dr["UtaxGL"]),
                        UTaxType = Convert.ToInt32(DBNull.Value.Equals(dr["UTaxType"]) ? 0 : dr["UTaxType"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                        Warehousefdesc = Convert.ToString(dr["Warehousefdesc"]),
                        STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? false : dr["STax"]),
                        STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        WHLocID = Convert.ToInt32(DBNull.Value.Equals(dr["WHLocID"]) ? 0 : dr["WHLocID"]),
                        GTax = Convert.ToInt16(DBNull.Value.Equals(dr["GTax"]) ? 0 : dr["GTax"]),
                    }
                    );

            }
            return _lstGetBillRecurrTrans;
        }

        public DataSet GetAPAgingByDate(PJ objPJ)
        {
            return _objDL_Bills.GetAPAgingByDate(objPJ);
        }

        public DataSet GetAPAgingByBasedDate(PJ objPJ)
        {
            return _objDL_Bills.GetAPAgingByBasedDate(objPJ);
        }

        public DataSet GetAPAgingOver90DaysReport(PJ objPJ)
        {
            return _objDL_Bills.GetAPAgingOver90DaysReport(objPJ);
        }

        //Project Labor Cost Report
        public DataSet GetProjectLaborCostReport(PJ objPJ)
        {
            return _objDL_Bills.GetProjectLaborCostReport(objPJ);
        }

        public DataSet GetOpenTicketbyRoutesReport(Customer objCust)
        {
            return _objDL_Bills.GetOpenTicketbyRoutesReport(objCust);
        }

        public List<GetAPAgingByDateViewModel> GetAPAgingByDate(GetAPAgingByDateParam _GetAPAgingByDateParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAPAgingByDate(_GetAPAgingByDateParam, ConnectionString);
            List<GetAPAgingByDateViewModel> _lstGetAPAgingByDateViewModel = new List<GetAPAgingByDateViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAPAgingByDateViewModel.Add(
                    new GetAPAgingByDateViewModel()
                    {
                        VendorID = Convert.ToInt32(DBNull.Value.Equals(dr["VendorID"]) ? 0 : dr["VendorID"]),
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                        Vendor = Convert.ToString(dr["Vendor"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                        SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        //isIssueDate = Convert.ToInt32(DBNull.Value.Equals(dr["isIssueDate"]) ? 0 : dr["isIssueDate"]),
                    }
                    );

            }
            return _lstGetAPAgingByDateViewModel;
        }

        public DataSet GetAPAging360ByDate(PJ objPJ)
        {
            return _objDL_Bills.GetAPAging360ByDate(objPJ);
        }

        //API
        public List<GetAPAging360ByDateViewModel> GetAPAging360ByDate(GetAPAging360ByDateParam _GetAPAging360ByDate, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAPAging360ByDate(_GetAPAging360ByDate, ConnectionString);

            List<GetAPAging360ByDateViewModel> _lstGetAPAging360ByDate = new List<GetAPAging360ByDateViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAPAging360ByDate.Add(
                    new GetAPAging360ByDateViewModel()
                    {
                        VendorID = Convert.ToInt32(DBNull.Value.Equals(dr["VendorID"]) ? 0 : dr["VendorID"]),
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                        Vendor = Convert.ToString(dr["Vendor"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        ThreeSixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThreeSixtyDay"]) ? 0 : dr["ThreeSixtyDay"]),
                        OverThreeSixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OverThreeSixtyDay"]) ? 0 : dr["OverThreeSixtyDay"]),
                    }
                    );

            }
            return _lstGetAPAging360ByDate;
        }

        public void UpdateAPDates(PJ objPJ)
        {
            _objDL_Bills.UpdateAPDates(objPJ);
        }
        public void UpdateAPDates(UpdateAPDatesParam _UpdateAPDatesParam, string ConnectionString)
        {
            _objDL_Bills.UpdateAPDates(_UpdateAPDatesParam, ConnectionString);
        }
        public void UpdateBillsJobDetails(PJ objPJ)
        {
            _objDL_Bills.UpdateBillsJobDetails(objPJ);
        }
        public void UpdateBillsJobDetails(UpdateBillsJobDetailsParam _UpdateBillsJobDetailsParam, string connectionString)
        {
            _objDL_Bills.UpdateBillsJobDetails(_UpdateBillsJobDetailsParam, connectionString);
        }
        public int AddCheck(CD objCD)
        {
            return _objDL_Bills.AddCheck(objCD);
        }
        public int AddCheck(AddCheckParam _AddCheckParam, string ConnectionString)
        {
            int ID = _objDL_Bills.AddCheck(_AddCheckParam,ConnectionString);
            return ID;
        }
        public int AddCheckRecurr(CD objCD)
        {
            return _objDL_Bills.AddCheckRecurr(objCD);
        }
        public int UpdateCheckRecurr(CD objCD)
        {
            return _objDL_Bills.UpdateCheckRecurr(objCD);
        }
        public int UpdateCheckRecurr(UpdateCheckRecurrParam _UpdateCheckRecurrParam, string ConnectionString)
        {
            int returnVal = _objDL_Bills.UpdateCheckRecurr(_UpdateCheckRecurrParam, ConnectionString);
            return returnVal;
        }
        public int AddCheckRecurr(AddCheckRecurrParam _AddCheckRecurrParam, string ConnectionString)
        {
            int ID = _objDL_Bills.AddCheckRecurr(_AddCheckRecurrParam, ConnectionString);
            return ID;
        }
       
        
        public int ApplyCredit(CD objCD)
        {
            return _objDL_Bills.ApplyCredit(objCD);
        }
        public int ApplyCredit(ApplyCreditParam _ApplyCreditParam, string ConnectionString)
        {
            int ID = _objDL_Bills.ApplyCredit(_ApplyCreditParam, ConnectionString);
            return ID;
        }
        public int UpdateApplyCreditDate(CD objCD)
        {
            return  _objDL_Bills.UpdateApplyCreditDate(objCD);
        }

        public int UpdateApplyCreditDate(UpdateApplyCreditDateParam _UpdateApplyCreditDateParam, string ConnectionString)
        {
            return _objDL_Bills.UpdateApplyCreditDate(_UpdateApplyCreditDateParam, ConnectionString);
        }
        //public void UpdateJobComm(PO objPO)
        //{
        //    _objDL_Bills.updateJobComm(objPO);
        //}
        //public void UpdateJobComm(string ConnectionString, updateJobCommParam _updateJobCommParam)
        //{
        //    _objDL_Bills.updateJobComm(ConnectionString, _updateJobCommParam);
        //}
        public void updateCheckTemplate(User objUser)
        {
            _objDL_Bills.updateCheckTemplate(objUser);
        }
        public void updateCheckTemplate(updateCheckTemplateParam _updateCheckTemplateParam, string ConnectionString)
        {
            _objDL_Bills.updateCheckTemplate(_updateCheckTemplateParam, ConnectionString);
        }
        public DataSet GetCheckTemplate(User objUser)
        {
            return _objDL_Bills.GetCheckTemplate(objUser);
        }
        public List<UserViewModel> GetCheckTemplate(GetCheckTemplateParam _GetCheckTemplateParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetCheckTemplate(_GetCheckTemplateParam, ConnectionString);

            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstUserViewModel.Add(
                    new UserViewModel()
                    {
                        CD_Template = Convert.ToString(dr["CD_Template"]),
                    }
                    );
            }

            return _lstUserViewModel;
        }
        public DataSet AutoSelectPayment(CD objCD, string company)
        {
            return _objDL_Bills.AutoSelectPayment(objCD, company);
        }
        public ListAutoSelectPayment AutoSelectPayment(AutoSelectPaymentParam _AutoSelectPaymentParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.AutoSelectPayment(_AutoSelectPaymentParam, ConnectionString);

            ListAutoSelectPayment _ds = new ListAutoSelectPayment();
            List<AutoSelectPaymentTable1> _lsttable1 = new List<AutoSelectPaymentTable1>();
            List<AutoSelectPaymentTable2> _lsttable2 = new List<AutoSelectPaymentTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lsttable1.Add(new AutoSelectPaymentTable1()
                {
                    Name = Convert.ToString(dr["Name"]),
                    Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                    Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                    Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    Selected = Convert.ToDouble(DBNull.Value.Equals(dr["Selected"]) ? 0 : dr["Selected"]),
                    Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                    PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                    TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                    Discount = Convert.ToDouble(DBNull.Value.Equals(dr["Discount"]) ? 0 : dr["Discount"]),
                    Ref = Convert.ToString(dr["Ref"]),
                    Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                    StatusName = Convert.ToString(dr["StatusName"]),
                    Payment = Convert.ToDouble(DBNull.Value.Equals(dr["Payment"]) ? 0 : dr["Payment"]),
                    billDesc = Convert.ToString(dr["billDesc"]),
                    IsSelected = Convert.ToBoolean(DBNull.Value.Equals(dr["IsSelected"]) ? false : dr["IsSelected"]),
                    //Duepayment = Convert.ToDouble(DBNull.Value.Equals(dr["Duepayment"]) ? 0 : dr["Duepayment"]),
                    
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lsttable2.Add(new AutoSelectPaymentTable2()
                {
                    NCount = Convert.ToInt32(DBNull.Value.Equals(dr["NCount"]) ? 0 : dr["NCount"]),
                    NAmt = Convert.ToDouble(DBNull.Value.Equals(dr["NAmt"]) ? 0 : dr["NAmt"]),
                });
            }

            _ds.lstTable1 = _lsttable1;
            _ds.lstTable2 = _lsttable2;


            return _ds;
        }
        public DataSet GetAutoSelectPayment(CD objCD, string company)
        {
            return _objDL_Bills.GetAutoSelectPayment(objCD, company);
        }
        public ListGetAutoSelectPayment GetAutoSelectPayment(GetAutoSelectPaymentParam _GetAutoSelectPaymentParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAutoSelectPayment(_GetAutoSelectPaymentParam,ConnectionString);

            ListGetAutoSelectPayment _ds = new ListGetAutoSelectPayment();
            List<GetAutoSelectPaymentTable1> _lsttable1 = new List<GetAutoSelectPaymentTable1>();
            List<GetAutoSelectPaymentTable2> _lsttable2 = new List<GetAutoSelectPaymentTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lsttable1.Add(new GetAutoSelectPaymentTable1()
                {
                    Name = Convert.ToString(dr["Name"]),
                    Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                    Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                    Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    Selected = Convert.ToDouble(DBNull.Value.Equals(dr["Selected"]) ? 0 : dr["Selected"]),
                    Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                    PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                    TRID = Convert.ToInt32(DBNull.Value.Equals(dr["TRID"]) ? 0 : dr["TRID"]),
                    Discount = Convert.ToDouble(DBNull.Value.Equals(dr["Discount"]) ? 0 : dr["Discount"]),
                    Ref = Convert.ToString(dr["Ref"]),
                    Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    Spec = Convert.ToInt16(DBNull.Value.Equals(dr["Spec"]) ? 0 : dr["Spec"]),
                    StatusName = Convert.ToString(dr["StatusName"]),
                    Payment = Convert.ToDouble(DBNull.Value.Equals(dr["Payment"]) ? 0 : dr["Payment"]),
                    billDesc = Convert.ToString(dr["billDesc"]),
                    IsSelected = Convert.ToInt32(DBNull.Value.Equals(dr["IsSelected"]) ? 0 : dr["IsSelected"]),
                    Duepayment = Convert.ToDouble(DBNull.Value.Equals(dr["Duepayment"]) ? 0 : dr["Duepayment"]),
                   
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lsttable2.Add(new GetAutoSelectPaymentTable2()
                {
                    NCount = Convert.ToInt32(DBNull.Value.Equals(dr["NCount"]) ? 0 : dr["NCount"]),
                    NAmt = Convert.ToDouble(DBNull.Value.Equals(dr["NAmt"]) ? 0 : dr["NAmt"]),
                });
            }

            _ds.lstTable1 = _lsttable1;
            _ds.lstTable2 = _lsttable2;

            return _ds;
        }
        public DataSet GetBillingItems(PJ _objPJ)
        {
            return _objDL_Bills.GetBillingItems(_objPJ);
        }

        //API
        public ListGetBillingItems GetBillingItems(GetBillingItemsParam _GetBillingItemsParam, string connectionString)
        {
            DataSet ds = _objDL_Bills.GetBillingItems(_GetBillingItemsParam, connectionString);

            ListGetBillingItems _ds = new ListGetBillingItems();
            List<GetBillingItemsTable1> _lsttable1 = new List<GetBillingItemsTable1>();
            List<GetBillingItemsTable2> _lsttable2 = new List<GetBillingItemsTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lsttable1.Add(new GetBillingItemsTable1()
                {
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    AcctID = Convert.ToInt32(DBNull.Value.Equals(dr["AcctID"]) ? 0 : dr["AcctID"]),
                    AcctNo = Convert.ToString(dr["AcctNo"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    GSTRate = Convert.ToDouble(DBNull.Value.Equals(dr["GSTRate"]) ? null : dr["GSTRate"]),
                    GSTTaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["GSTTaxGL"]) ? 0 : dr["GSTTaxGL"]),
                    GTaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["GTaxAmt"]) ? 0 : dr["GTaxAmt"]),
                    ItemDesc = Convert.ToString(dr["ItemDesc"]),
                    ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                    JobID = Convert.ToInt32(DBNull.Value.Equals(dr["JobID"]) ? 0 : dr["JobID"]),
                    JobName = Convert.ToString(dr["JobName"]),
                    Loc = Convert.ToString(dr["Loc"]),
                    OpSq = Convert.ToString(dr["OpSq"]),
                    OutstandBalance = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandBalance"]) ? 0 : dr["OutstandBalance"]),
                    OutstandQuan = Convert.ToDouble(DBNull.Value.Equals(dr["OutstandQuan"]) ? 0 : dr["OutstandQuan"]),
                    Phase = Convert.ToString(dr["Phase"]),
                    PhaseID = Convert.ToInt16(DBNull.Value.Equals(dr["PhaseID"]) ? 0 : dr["PhaseID"]),
                    PrvIn = Convert.ToDouble(DBNull.Value.Equals(dr["PrvIn"]) ? 0 : dr["PrvIn"]),
                    PrvInQuan = Convert.ToDouble(DBNull.Value.Equals(dr["PrvInQuan"]) ? 0 : dr["PrvInQuan"]),
                    Quan = Convert.ToInt32(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                    STaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["STaxAmt"]) ? 0 : dr["STaxAmt"]),
                    Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                    TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"]) ? 0 : dr["TypeID"]),
                    UtaxGL = Convert.ToString(dr["UtaxGL"]),
                    STaxName = Convert.ToString(dr["STaxName"]),
                    STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                    STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? false : dr["STax"]),
                    STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                    RowNo = Convert.ToInt32(DBNull.Value.Equals(dr["RowNo"]) ? 0 : dr["RowNo"]),
                    Uname = Convert.ToString(dr["Uname"]),
                    UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["UseTax"]) ? 0 : dr["UseTax"]),
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lsttable2.Add(new GetBillingItemsTable2()
                {
                    AcctNo = Convert.ToString(dr["AcctNo"]),
                    Amount = Convert.ToString(dr["Amount"]),
                    Code = Convert.ToString(dr["Code"]),
                    ItemDis = Convert.ToString(dr["ItemDis"]),
                    ProjNo = Convert.ToString(dr["ProjNo"]),
                    RowNo = Convert.ToInt32(DBNull.Value.Equals(dr["RowNo"]) ? 0 : dr["RowNo"]),
                });
            }

            _ds.lstTable1 = _lsttable1;
            _ds.lstTable2 = _lsttable2;

            return _ds;

        }
        public DataSet GetBillsLogs(PJ _objPJ)
        {
            return _objDL_Bills.GetBillsLogs(_objPJ);
        }

        public List<LogViewModel> GetBillsLogs(GetBillsLogsParam _GetBillsLogs, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetBillsLogs(_GetBillsLogs, ConnectionString);
            List<LogViewModel> _lstLogViewModel = new List<LogViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstLogViewModel.Add(
                    new LogViewModel()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }

            return _lstLogViewModel;

        }
        public DataSet GetAPCheckLogs(CD objCD)
        {
            return _objDL_Bills.GetAPCheckLogs(objCD);
        }
        public List<LogViewModel> GetAPCheckLogs(GetAPCheckLogsParam _GetAPCheckLogsParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAPCheckLogs( _GetAPCheckLogsParam,  ConnectionString);
            List<LogViewModel> _lstLogViewModel = new List<LogViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstLogViewModel.Add(
                    new LogViewModel()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }

            return _lstLogViewModel;
        }
        public DataSet GetPOLogs(PO _objPO)
        {
            return _objDL_Bills.GetPOLogs(_objPO);
        }
        public DataSet GetReceivePOLogs(PO _objPO)
        {
            return _objDL_Bills.GetReceivePOLogs(_objPO);
        }
        public int AddRPO(PO _objPO)
        {
            return _objDL_Bills.AddRPO(_objPO);
        }
        public int EditRPO(PO _objPO)
        {
            return _objDL_Bills.EditRPO(_objPO);
        }
        public int AddEditReceivePO(PO _objPO)
        {
           return _objDL_Bills.AddEditReceivePO(_objPO);
        }
        public int AddEditReceivePO(string ConnectionString, AddEditReceivePOParam _AddEditReceivePOParam)
        {
            return _objDL_Bills.AddEditReceivePO(ConnectionString, _AddEditReceivePOParam);
        }
        public void UpdateReceivePODue(PO _objPO)
        {
            _objDL_Bills.UpdateReceivePODue(_objPO);
        }

        public void DeleteReceivePO(PO _objPO)
        {
            _objDL_Bills.DeleteReceivePO(_objPO);
        }

        public void UpdateReceivePOItem(PO _objPO)
        {
            _objDL_Bills.UpdateReceivePOItem(_objPO);
        }

        public void UpdateReceivePO(PO _objPO)
        {
            _objDL_Bills.UpdateReceivePO(_objPO);
        }

        public void AutoUpdatePOStatus(PO _objPO)
        {
            _objDL_Bills.AutoUpdatePOStatus(_objPO);
        }
        public void UpdateWriteCheckOpenAPpayment(OpenAP _OpenAP)
        {
            _objDL_Bills.UpdateWriteCheckOpenAPpayment(_OpenAP);
        }
        public void UpdateWriteCheckOpenAPpayment(UpdateWriteCheckOpenAPpaymentParam _UpdateWriteCheckOpenAPpaymentParam, string ConnectionString)
        {
            _objDL_Bills.UpdateWriteCheckOpenAPpayment(_UpdateWriteCheckOpenAPpaymentParam, ConnectionString);
        }
        public DataSet GetRunningBalanceCounts(CD objCD)
        {
            return _objDL_Bills.GetRunningBalanceCounts(objCD);
        }

        public List<OpenAPViewModel> GetRunningBalanceCounts(GetRunningBalanceCountsParam _GetRunningBalanceCountsParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetRunningBalanceCounts(_GetRunningBalanceCountsParam,ConnectionString);

            List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstOpenAPViewModel.Add(
                    new OpenAPViewModel()
                    {
                        RunningBalance = Convert.ToDouble(DBNull.Value.Equals(dr["RunningBalance"]) ? 0 : dr["RunningBalance"]),
                        Counts = Convert.ToInt32(DBNull.Value.Equals(dr["Counts"]) ? 0 : dr["Counts"]),
                        TotVendor = Convert.ToInt32(DBNull.Value.Equals(dr["TotVendor"]) ? 0 : dr["TotVendor"]),
                    }
                    );
            }

            return _lstOpenAPViewModel;
        }
        public DataSet GetSelectedOpenAPPJID(CD objCD)
        {
            return _objDL_Bills.GetSelectedOpenAPPJID(objCD);
        }
        public List<OpenAPViewModel> GetSelectedOpenAPPJID(GetSelectedOpenAPPJIDParam _GetSelectedOpenAPPJIDParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetSelectedOpenAPPJID(_GetSelectedOpenAPPJIDParam, ConnectionString);
            List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstOpenAPViewModel.Add(
                    new OpenAPViewModel()
                    {
                        PJID = Convert.ToInt32(DBNull.Value.Equals(dr["PJID"]) ? 0 : dr["PJID"]),
                    }
                    );
            }

            return _lstOpenAPViewModel;
        }
        public string GetCustomProgramForMitsu(string connString)
        {
            return _objDL_Bills.GetCustomProgramForMitsu(connString);

        }

        public DataSet GetAPGLReg(PJ objPJ, List<RetainFilter> filters, bool inclClose)
        {
            return _objDL_Bills.GetAPGLReg(objPJ, filters, inclClose);
        }
        public List<GetAPGLRegViewModel> GetAPGLReg(GetAPGLRegParam _GetAPGLRegParam, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetAPGLReg( _GetAPGLRegParam,  ConnectionString);
            List<GetAPGLRegViewModel> _lstGetAPGLRegViewModel = new List<GetAPGLRegViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAPGLRegViewModel.Add(
                    new GetAPGLRegViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        GLAcct = Convert.ToString(dr["GLAcct"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        ReceivePO = Convert.ToInt32(DBNull.Value.Equals(dr["ReceivePO"]) ? 0 : dr["ReceivePO"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                    }
                    );

            }
            return _lstGetAPGLRegViewModel;
        }
        public DataSet GetInventoryItemStatus(Inventory _objInv)
        {
            return _objDL_Bills.GetInventoryItemStatus(_objInv);
        }
        public DataTable GetInventoryItemStatusbyIds(string conn, string invIds)
        {
            return _objDL_Bills.GetInventoryItemStatusbyIds(conn, invIds);
        }
        public DataTable GetChartStatusbyIds(string conn,string chartIds)
        {
            return _objDL_Bills.GetChartStatusbyIds(conn, chartIds);
        }
        public ListGetInventoryItemStatus GetInventoryItemStatus(string ConnectionString, GetInventoryItemStatusParam _GetInventoryItemStatusParam)
        {
            DataSet ds = _objDL_Bills.GetInventoryItemStatus(ConnectionString, _GetInventoryItemStatusParam);

            ListGetInventoryItemStatus _lstInventory = new ListGetInventoryItemStatus();
            List<GetInventoryItemStatusTable1> _lstTable1 = new List<GetInventoryItemStatusTable1>();
            List<GetInventoryItemStatusTable2> _lstTable2 = new List<GetInventoryItemStatusTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetInventoryItemStatusTable1()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Name = Convert.ToString(dr["Name"]),
                        Part = Convert.ToString(dr["Part"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StrStatus = Convert.ToString(dr["StrStatus"]),
                        SAcct = Convert.ToInt32(DBNull.Value.Equals(dr["SAcct"]) ? 0 : dr["SAcct"]),
                        Measure = Convert.ToString(dr["Measure"]),
                        Tax = Convert.ToInt32(DBNull.Value.Equals(dr["Tax"]) ? 0 : dr["Tax"]),
                        Price1 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        Price2 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price2"]) ? 0 : dr["Price2"]),
                        Price3 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price3"]) ? 0 : dr["Price3"]),
                        Price4 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price4"]) ? 0 : dr["Price4"]),
                        Price5 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price5"]) ? 0 : dr["Price5"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Cat = Convert.ToInt32(DBNull.Value.Equals(dr["Cat"]) ? 0 : dr["Cat"]),
                        LVendor = Convert.ToInt32(DBNull.Value.Equals(dr["LVendor"]) ? 0 : dr["LVendor"]),
                        LCost = Convert.ToDecimal(DBNull.Value.Equals(dr["LCost"]) ? 0 : dr["LCost"]),
                        AllowZero = Convert.ToInt32(DBNull.Value.Equals(dr["AllowZero"]) ? 0 : dr["AllowZero"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Aisle = Convert.ToString(dr["Aisle"]),
                        Min = Convert.ToDecimal(DBNull.Value.Equals(dr["Min"]) ? 0 : dr["Min"]),
                        Shelf = Convert.ToString(dr["Shelf"]),
                        Bin = Convert.ToString(dr["Bin"]),
                        Requ = Convert.ToDecimal(DBNull.Value.Equals(dr["Requ"]) ? 0 : dr["Requ"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        Price6 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price6"]) ? 0 : dr["Price6"]),
                        QBInvID = Convert.ToString(dr["QBInvID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        QBAccountID = Convert.ToString(dr["QBAccountID"]),
                        IssuedOpenJobs = Convert.ToDecimal(DBNull.Value.Equals(dr["IssuedOpenJobs"]) ? 0 : dr["IssuedOpenJobs"]),
                        Description2 = Convert.ToString(dr["Description2"]),
                        Description3 = Convert.ToString(dr["Description3"]),
                        Description4 = Convert.ToString(dr["Description4"]),
                        DateCreated = Convert.ToDateTime(DBNull.Value.Equals(dr["DateCreated"]) ? null : dr["DateCreated"]),
                        Class = Convert.ToString(dr["Description4"]),
                        Specification = Convert.ToString(dr["Specification"]),
                        Specification2 = Convert.ToString(dr["Specification2"]),
                        Specification3 = Convert.ToString(dr["Specification3"]),
                        Specification4 = Convert.ToString(dr["Specification4"]),
                        Revision = Convert.ToString(dr["Revision"]),
                        LastRevisionDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastRevisionDate"]) ? null : dr["LastRevisionDate"]),
                        Eco = Convert.ToString(dr["Eco"]),
                        Drawing = Convert.ToString(dr["Drawing"]),
                        Reference = Convert.ToString(dr["Reference"]),
                        Length = Convert.ToString(dr["Length"]),
                        Width = Convert.ToString(dr["Width"]),
                        Weight = Convert.ToString(dr["Weight"]),
                        InspectionRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["InspectionRequired"]) ? 0 : dr["InspectionRequired"]),
                        CoCRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["CoCRequired"]) ? 0 : dr["CoCRequired"]),
                        ShelfLife = Convert.ToDecimal(DBNull.Value.Equals(dr["ShelfLife"]) ? 0 : dr["ShelfLife"]),
                        SerializationRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["SerializationRequired"]) ? 0 : dr["SerializationRequired"]),
                        GLcogs = Convert.ToString(dr["GLcogs"]),
                        GLPurchases = Convert.ToString(dr["GLPurchases"]),
                        ABCClass = Convert.ToString(dr["ABCClass"]),
                        OHValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OHValue"]) ? 0 : dr["OHValue"]),
                        OOValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OOValue"]) ? 0 : dr["OOValue"]),
                        OverIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["OverIssueAllowance"]) ? 0 : dr["OverIssueAllowance"]),
                        UnderIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["UnderIssueAllowance"]) ? 0 : dr["UnderIssueAllowance"]),
                        InventoryTurns = Convert.ToDecimal(DBNull.Value.Equals(dr["InventoryTurns"]) ? 0 : dr["InventoryTurns"]),
                        MOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["MOQ"]) ? 0 : dr["MOQ"]),
                        EOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["EOQ"]) ? 0 : dr["EOQ"]),
                        MinInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MinInvQty"]) ? 0 : dr["MinInvQty"]),
                        MaxInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxInvQty"]) ? 0 : dr["MaxInvQty"]),
                        Commodity = Convert.ToString(dr["Commodity"]),
                        LastReceiptDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReceiptDate"]) ? null : dr["LastReceiptDate"]),
                        MPN = Convert.ToString(dr["MPN"]),
                        ApprovedManufacturer = Convert.ToString(dr["ApprovedManufacturer"]),
                        ApprovedVendor = Convert.ToString(dr["ApprovedVendor"]),
                        EAU = Convert.ToDecimal(DBNull.Value.Equals(dr["EAU"]) ? 0 : dr["EAU"]),
                        EOLDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EOLDate"]) ? null : dr["EOLDate"]),
                        WarrantyPeriod = Convert.ToInt32(DBNull.Value.Equals(dr["WarrantyPeriod"]) ? 0 : dr["WarrantyPeriod"]),
                        PODueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PODueDate"]) ? null : dr["PODueDate"]),
                        DefaultReceivingLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultReceivingLocation"]) ? 0 : dr["DefaultReceivingLocation"]),
                        DefaultInspectionLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultInspectionLocation"]) ? 0 : dr["DefaultInspectionLocation"]),
                        LastSalePrice = Convert.ToDecimal(DBNull.Value.Equals(dr["LastSalePrice"]) ? 0 : dr["LastSalePrice"]),
                        AnnualSalesQty = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesQty"]) ? 0 : dr["AnnualSalesQty"]),
                        AnnualSalesAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesAmt"]) ? 0 : dr["AnnualSalesAmt"]),
                        QtyAllocatedToSO = Convert.ToDecimal(DBNull.Value.Equals(dr["QtyAllocatedToSO"]) ? 0 : dr["QtyAllocatedToSO"]),
                        MaxDiscountPercentage = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxDiscountPercentage"]) ? 0 : dr["MaxDiscountPercentage"]),
                        Height = Convert.ToString(dr["Height"]),
                        GLSales = Convert.ToString(dr["GLSales"]),
                        leadTime = Convert.ToInt32(DBNull.Value.Equals(dr["leadTime"]) ? 0 : dr["leadTime"]),
                        DateLastPurchase = Convert.ToDateTime(DBNull.Value.Equals(dr["DateLastPurchase"]) ? null : dr["DateLastPurchase"]),
                        WarehouseCount = Convert.ToInt32(DBNull.Value.Equals(dr["WarehouseCount"]) ? 0 : dr["WarehouseCount"]),
                        Hand = Convert.ToDecimal(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
                        Balance = Convert.ToDecimal(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        fOrder = Convert.ToDecimal(DBNull.Value.Equals(dr["fOrder"]) ? 0 : dr["fOrder"]),
                        Committed = Convert.ToDecimal(DBNull.Value.Equals(dr["Committed"]) ? 0 : dr["Committed"]),
                        Available = Convert.ToDecimal(DBNull.Value.Equals(dr["Available"]) ? 0 : dr["Available"]),
                        UnitCost = Convert.ToDecimal(DBNull.Value.Equals(dr["UnitCost"]) ? 0 : dr["UnitCost"]),
                        catName = Convert.ToString(dr["catName"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetInventoryItemStatusTable2()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        DisplayName = Convert.ToString(dr["DisplayName"]),
                        MappingColumn = Convert.ToString(dr["MappingColumn"]),
                    }
                    );
            }

            _lstInventory.lstTable1 = _lstTable1;
            _lstInventory.lstTable2 = _lstTable2;

            return _lstInventory;
        }
        public DataSet GetHistoryTransaction(string conn, string id, string type, int vendor, int loc, string status, int tid)
        {
            return _objDL_Bills.GetHistoryTransaction(conn, id, type, vendor, loc, status, tid);
        }

        //API
        public List<GetHistoryTransactionViewModel> GetHistoryTransaction(GetHistoryTransactionParam _GetHistoryTransaction, string ConnectionString)
        {
            DataSet ds = _objDL_Bills.GetHistoryTransaction(_GetHistoryTransaction, ConnectionString);

            List<GetHistoryTransactionViewModel> _lstGetHistoryTransaction = new List<GetHistoryTransactionViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetHistoryTransaction.Add(
                    new GetHistoryTransactionViewModel()
                    {
                        line = Convert.ToInt32(DBNull.Value.Equals(dr["line"]) ? 0 : dr["line"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Type = Convert.ToString(dr["Type"]),
                        LinkTo = Convert.ToString(dr["LinkTo"]),
                    }
                    );

            }
            return _lstGetHistoryTransaction;
        }
        public DataTable spGetOpenPODetailforRPO(PO _objPO)
        {
            return _objDL_Bills.spGetOpenPODetailforRPO(_objPO);
        }
        public void UpdtReceivePOAmnt(PO _objPO)
        {
             _objDL_Bills.UpdtReceivePOAmnt(_objPO);
        }
    }
}
