using BusinessEntity;
using BusinessEntity.CustomersModel;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_Invoice
    {
        DL_Invoice _objDLInvoice = new DL_Invoice();

        public DataSet InvoicingFromTicketScreen(InvoicingFromTicketScreen _objInvoice)
        {
            return _objDLInvoice.InvoicingFromTicketScreen(_objInvoice);
        }


        public DataSet GetInvByID(Inv _objInv)
        {
            return _objDLInvoice.GetInvByID(_objInv);
        }
        //public void UpdateInvoiceTransDetails(Invoices _objInvoices)
        //{
        //    _objDLInvoice.UpdateInvoiceTransDetails(_objInvoices);
        //}
        public void DeleteTransInvoiceByRef(Transaction _objTrans)
        {
            _objDLInvoice.DeleteTransInvoiceByRef(_objTrans);
        }
        public DataSet GetInvoiceByID(Invoices _objInvoice)
        {
            return _objDLInvoice.GetInvoiceByID(_objInvoice);
        }
        public DataSet GetARRevenue(Contracts objContract)
        {
            return _objDLInvoice.GetARRevenue(objContract);
        }

        //API
        public ListGetARRevenue GetARRevenue(GetARRevenueParam _GetARRevenue, string ConnectionString)
        {
            DataSet ds = _objDLInvoice.GetARRevenue(_GetARRevenue, ConnectionString);

            ListGetARRevenue _ds = new ListGetARRevenue();
            List<GetARRevenueTable1> _lstTable1 = new List<GetARRevenueTable1>();
            List<GetARRevenueTable2> _lstTable2 = new List<GetARRevenueTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetARRevenueTable1()
                {
                    CustName = Convert.ToString(dr["CustName"]),
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    Credits = Convert.ToDouble(DBNull.Value.Equals(dr["Credits"]) ? 0 : dr["Credits"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Link = Convert.ToString(dr["Link"]),
                    linkTo = Convert.ToInt32(DBNull.Value.Equals(dr["linkTo"]) ? 0 : dr["linkTo"]),
                    LocName = Convert.ToString(dr["LocName"]),
                    owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                    REF = Convert.ToString(dr["REF"]),
                    Status = Convert.ToString(dr["Status"]),
                    TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                    Type = Convert.ToString(dr["Type"]),
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetARRevenueTable2()
                {
                    Column1 = Convert.ToDouble(DBNull.Value.Equals(dr["Column1"]) ? 0 : dr["Column1"]),
                });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }

        public DataSet GetReceivePayInvoice(Invoices objInvoice)
        {
            return _objDLInvoice.GetReceivePayInvoice(objInvoice);
        }
        public DataSet GetAppliedDeposit(Invoices objInvoice)
        {
            return _objDLInvoice.GetAppliedDeposit(objInvoice);
        }

        public DataSet GetSalesTax(Invoices objInvoice, bool isSummary = false)
        {
            return _objDLInvoice.GetSalesTax(objInvoice, isSummary);
        }
        public DataSet GetSalesTaxByVendor(Invoices objInvoice, bool isSummary = false)
        {
            return _objDLInvoice.GetSalesTaxByVendor(objInvoice, isSummary);
        }
        public DataSet GetSalesTax2(Invoices objInvoice)
        {
            return _objDLInvoice.GetSalesTax2(objInvoice);
        }
        public DataSet GetARRevenueCust(Contracts objContract)
        {
            return _objDLInvoice.GetARRevenueCust(objContract);
        }

        //API
        public ListGetARRevenueCustShowAll GetARRevenueCust(GetARRevenueCustParam _GetARRevenueCust, string ConnectionString)
        {
            DataSet ds = _objDLInvoice.GetARRevenueCust(_GetARRevenueCust, ConnectionString);

            ListGetARRevenueCustShowAll _ds = new ListGetARRevenueCustShowAll();
            List<GetARRevenueCustShowAllTable1> _lstTable1 = new List<GetARRevenueCustShowAllTable1>();
            List<GetARRevenueCustShowAllTable2> _lstTable2 = new List<GetARRevenueCustShowAllTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(new GetARRevenueCustShowAllTable1()
                {
                    CustName = Convert.ToString(dr["CustName"]),
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    Credits = Convert.ToDouble(DBNull.Value.Equals(dr["Credits"]) ? 0 : dr["Credits"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Link = Convert.ToString(dr["Link"]),
                    linkTo = Convert.ToInt32(DBNull.Value.Equals(dr["linkTo"]) ? 0 : dr["linkTo"]),
                    LocID = Convert.ToInt32(DBNull.Value.Equals(dr["LocID"]) ? 0 : dr["LocID"]),
                    LocName = Convert.ToString(dr["LocName"]),
                    owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                    Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                    Status = Convert.ToString(dr["Status"]),
                    TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                    Type = Convert.ToString(dr["Type"]),
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(new GetARRevenueCustShowAllTable2()
                {
                    PrevRunTotal = Convert.ToDouble(DBNull.Value.Equals(dr["PrevRunTotal"]) ? 0 : dr["PrevRunTotal"]),
                });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public DataSet GetARRevenueCustShowAll(Contracts objContract)
        {
            return _objDLInvoice.GetARRevenueCustShowAll(objContract);
        }

        //API
        public ListGetARRevenueCustShowAll GetARRevenueCustShowAll(GetARRevenueCustShowAllParam _GetARRevenueCustShowAll, string ConnectionString)
        {
            DataSet ds = _objDLInvoice.GetARRevenueCustShowAll(_GetARRevenueCustShowAll, ConnectionString);

            ListGetARRevenueCustShowAll _ds = new ListGetARRevenueCustShowAll();
            List<GetARRevenueCustShowAllTable1> _lstTable1 = new List<GetARRevenueCustShowAllTable1>();
            List<GetARRevenueCustShowAllTable2> _lstTable2 = new List<GetARRevenueCustShowAllTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(new GetARRevenueCustShowAllTable1()
                {
                    CustName = Convert.ToString(dr["CustName"]),
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    Credits = Convert.ToDouble(DBNull.Value.Equals(dr["Credits"]) ? 0 : dr["Credits"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    fDesc = Convert.ToString(dr["fDesc"]),
                    Link = Convert.ToString(dr["Link"]),
                    linkTo = Convert.ToInt32(DBNull.Value.Equals(dr["linkTo"]) ? 0 : dr["linkTo"]),
                    LocID = Convert.ToInt32(DBNull.Value.Equals(dr["LocID"]) ? 0 : dr["LocID"]),
                    LocName  = Convert.ToString(dr["LocName"]),
                    owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                    Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                    Status = Convert.ToString(dr["Status"]),
                    TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                    Type = Convert.ToString(dr["Type"]),
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(new GetARRevenueCustShowAllTable2()
                {
                    PrevRunTotal = Convert.ToDouble(DBNull.Value.Equals(dr["PrevRunTotal"]) ? 0 : dr["PrevRunTotal"]),
                });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }

        public DataSet GetSalesTaxCollected(Invoices objInvoice)
        {
            return _objDLInvoice.GetSalesTaxCollected(objInvoice);
        }

        public DataSet GetSalesTaxCollectedByVendor(Invoices objInvoice)
        {
            return _objDLInvoice.GetSalesTaxCollectedByVendor(objInvoice);
        }

        public DataSet GetSalesTaxCollectedDetail(Invoices objInvoice)
        {
            return _objDLInvoice.GetSalesTaxCollectedDetail(objInvoice);
        }
        public DataSet GetSalesTaxCollectedDetailByVendor(Invoices objInvoice)
        {
            return _objDLInvoice.GetSalesTaxCollectedDetailByVendor(objInvoice);
        }

        public DataSet GetHistoryTransaction(string conn, int id, int type, int owner, int loc,string status,int tid)
        {
            return _objDLInvoice.GetHistoryTransaction(conn,id, type, owner, loc, status,tid);
        }
    }
}
