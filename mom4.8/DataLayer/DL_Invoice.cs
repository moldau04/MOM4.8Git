using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer
{
    public class DL_Invoice
    {
        public DataSet GetInvByID(Inv _objInv)
        {
            try
            {
                return _objInv.Ds = SqlHelper.ExecuteDataset(_objInv.ConnConfig, CommandType.Text, "SELECT ID,Name,fDesc,Part,Status,SAcct,Measure,Tax,Balance,Price1,Price2,Price3,Price4,Price5,Remarks,Cat,LVendor,LCost,AllowZero,Type,InUse,EN,Hand,Aisle,fOrder,Min,Shelf,Bin,Requ,Warehouse,Price6,Committed,QBInvID,LastUpdateDate,QBAccountID FROM Inv WHERE ID=" + _objInv.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateInvoiceTransDetails(Invoices _objInvoices)
        //{
        //    try
        //    {
        //        //string query = "UPDATE Invoice SET Batch=@Batch, TransID=@TransID WHERE Ref=@Ref";
        //        //List<SqlParameter> parameters = new List<SqlParameter>();
        //        //parameters.Add(new SqlParameter("@Ref", _objInvoices.Ref));
        //        //parameters.Add(new SqlParameter("@Batch", _objInvoices.Batch));
        //        //parameters.Add(new SqlParameter("@TransID", _objInvoices.TransID));
        //        string query = "UPDATE Trans SET Ref=@Ref WHERE Batch=@Batch";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@Ref", _objInvoices.Ref));
        //        parameters.Add(new SqlParameter("@Batch", _objInvoices.Batch));
        //        int rowsAffected = SqlHelper.ExecuteNonQuery(_objInvoices.ConnConfig, CommandType.Text, query, parameters.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet InvoicingFromTicketScreen(InvoicingFromTicketScreen _objInvoice)
        {
            SqlParameter[] para = new SqlParameter[5];


            para[0] = new SqlParameter();
            para[0].ParameterName = "Project";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = _objInvoice.Project;

            para[1] = new SqlParameter();
            para[1].ParameterName = "TicketID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _objInvoice.TicketID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Workorderonly";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = _objInvoice.Workorderonly;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Combind";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = _objInvoice.Combind;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Reviewonly";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = _objInvoice.Reviewonly;


            try
            {
                return SqlHelper.ExecuteDataset(_objInvoice.ConnConfig, CommandType.StoredProcedure, "spInvoicingFromTicketScreen", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTransInvoiceByRef(Transaction _objTrans)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, " DELETE FROM Trans WHERE Ref = " + _objTrans.Ref + " AND Batch = " + _objTrans.BatchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceByID(Invoices _objInvoice)
        {
            try
            {
                return _objInvoice.Ds = SqlHelper.ExecuteDataset(_objInvoice.ConnConfig, CommandType.Text, "SELECT fDate,Ref,Batch,TransID FROM Invoice WHERE Ref=" + _objInvoice.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARRevenue(Contracts objContract)
        {
            try
            {
                var para = new SqlParameter[8];

                para[0] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.CustID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.Loc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.Date,
                    Value = objContract.StartDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.Date,
                    Value = objContract.EndDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.SearchValue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "searchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.SearchBy
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "filterBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.filterBy
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "filterValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.filterValue
                };
                return objContract.Ds = SqlHelper.ExecuteDataset(objContract.ConnConfig, CommandType.StoredProcedure, "spGetARRevenue", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARRevenue(GetARRevenueParam _GetARRevenue, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[8];

                para[0] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetARRevenue.CustID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetARRevenue.Loc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.Date,
                    Value = _GetARRevenue.StartDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.Date,
                    Value = _GetARRevenue.EndDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenue.SearchValue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "searchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenue.SearchBy
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "filterBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenue.filterBy
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "filterValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenue.filterValue
                };
                return _GetARRevenue.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARRevenue", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReceivePayInvoice(Invoices objInvoice)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.Text, "select * from PaymentDetails where InvoiceID = " + objInvoice.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAppliedDeposit(Invoices objInvoice)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.Text, "select * from trans t inner join (select t.Ref, d.Status from DepApply d inner join trans t on t.ID = d.TransID where d.type = 0 and t.ref = '" + objInvoice.Ref + "') d    on d.status = t.status and t.type = 6   ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesTax(Invoices objInvoice, bool isSummary = false)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!isSummary)
                {
                    sb.Append("SELECT \n");
                    sb.Append("	i.fDate, \n");
                    sb.Append("	i.Ref, \n");
                    sb.Append("	l.tag as locName, \n");
                    sb.Append("	r.Name As CustName, \n");
                    sb.Append("	i.fDesc, \n");
                    sb.Append("	i.Amount, \n");
                    sb.Append("	i.Stax, \n");
                    sb.Append("	i.Total, \n");
                    sb.Append("	i.TaxRegion, \n");
                    sb.Append("	i.TaxRate, \n");
                    sb.Append("	i.TaxFactor, \n");
                    sb.Append("	i.Taxable, \n");
                    sb.Append("	l.ID, \n");
                    sb.Append("	s.State, \n");
                    sb.Append("	s.fDesc As sDesc, \n");
                    sb.Append("	s.Name As sName, \n");
                    sb.Append("	s.Rate As sRate  \n");
                    sb.Append("FROM Invoice i  \n");
                    sb.Append("	INNER JOIN Loc l ON l.Loc = i.Loc  \n");
                    sb.Append("	INNER JOIN Owner o ON o.id = l.owner \n");
                    sb.Append("	INNER JOIN Rol r ON r.id = o.Rol \n");
                    sb.Append("	INNER JOIN STax s ON i.TaxRegion = s.Name  \n");
                    sb.Append("WHERE i.Status <> 2 AND s.UType = 0 \n");
                    sb.Append("	AND  i.fDate >= '" + objInvoice.StartDate + "' \n");
                    sb.Append("	And i.fDate <= '" + objInvoice.EndDate + "' \n");
                    sb.Append("ORDER BY s.Name, i.fDate \n");
                }
                else
                {
                    sb.Append("SELECT \n");
                    sb.Append("	SUM(i.Amount) AS Amount, \n");
                    sb.Append("	SUM(i.Stax) AS Stax, \n");
                    sb.Append("	SUM(i.Total) AS Total, \n");
                    sb.Append("	SUM(i.Taxable) AS Taxable, \n");
                    sb.Append("	COUNT(*) AS ICount, \n");
                    sb.Append("	s.State, \n");
                    sb.Append("	s.fDesc As sDesc, \n");
                    sb.Append("	s.Name As sName, \n");
                    sb.Append("	s.Rate As sRate  \n");
                    sb.Append("FROM Invoice i \n");
                    sb.Append("	INNER JOIN Loc l ON l.Loc = i.Loc \n");
                    sb.Append("	INNER JOIN Owner o ON o.id = l.owner \n");
                    sb.Append("	INNER JOIN Rol r ON r.id = o.Rol \n");
                    sb.Append("	INNER JOIN STax s ON i.TaxRegion = s.Name \n");
                    sb.Append("WHERE i.Status <> 2 AND s.UType = 0 \n");
                    sb.Append("	AND  i.fDate >= '" + objInvoice.StartDate + "' \n");
                    sb.Append("	And i.fDate <= '" + objInvoice.EndDate + "' \n");
                    sb.Append("GROUP BY s.State, s.fDesc, s.Name, s.Rate\n");
                    sb.Append("ORDER BY s.Name\n");
                }

                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesTaxByVendor(Invoices objInvoice, bool isSummary = false)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!isSummary)
                {
                    sb.Append("SELECT \n");
                    sb.Append("	i.fDate, \n");
                    sb.Append("	i.Ref, \n");
                    sb.Append("	l.tag as locName, \n");
                    sb.Append("	r.Name As CustName, \n");
                    sb.Append("	i.fDesc, \n");
                    sb.Append("	i.Amount, \n");
                    sb.Append("	i.Stax, \n");
                    sb.Append("	i.Total, \n");
                    sb.Append("	i.TaxRegion, \n");
                    sb.Append("	i.TaxRate, \n");
                    sb.Append("	i.TaxFactor, \n");
                    sb.Append("	i.Taxable, \n");
                    sb.Append("	l.ID, \n");
                    sb.Append("	s.State, \n");
                    sb.Append("	s.fDesc As sDesc, \n");
                    sb.Append("	s.Name As sName, \n");
                    sb.Append("	s.Rate As sRate,  \n");
                    sb.Append("v.Acct as Vendor, \n");
                    sb.Append("	CONVERT(varchar(50), (select top 1  PaymentReceivedDate from ReceivedPayment where ID in(select ReceivedPaymentID from PaymentDetails where InvoiceID=i.Ref AND IsInvoice = 1) order by PaymentReceivedDate desc), 101) AS InvoiceDate \n");
                    sb.Append("FROM Invoice i  \n");
                    sb.Append("	INNER JOIN Loc l ON l.Loc = i.Loc  \n");
                    sb.Append("	INNER JOIN Owner o ON o.id = l.owner \n");
                    sb.Append("	INNER JOIN Rol r ON r.id = o.Rol \n");
                    sb.Append("	INNER JOIN STax s ON i.TaxRegion = s.Name  \n");
                    sb.Append("LEFT JOIN Vendor v ON s.Vendor = v.ID \n");
                    sb.Append("WHERE i.Status <> 2 AND s.UType = 0 \n");
                    sb.Append("	AND  i.fDate >= '" + objInvoice.StartDate + "' \n");
                    sb.Append("	And i.fDate <= '" + objInvoice.EndDate + "' \n");
                    sb.Append("ORDER BY s.Name, i.fDate \n");
                }
                else
                {
                    sb.Append("SELECT \n");
                    sb.Append("	SUM(i.Amount) AS Amount, \n");
                    sb.Append("	SUM(i.Stax) AS Stax, \n");
                    sb.Append("	SUM(i.Total) AS Total, \n");
                    sb.Append("	SUM(i.Taxable) AS Taxable, \n");
                    sb.Append("	COUNT(*) AS ICount, \n");
                    sb.Append("	s.State, \n");
                    sb.Append("	s.fDesc As sDesc, \n");
                    sb.Append("	s.Name As sName, \n");
                    sb.Append("	s.Rate As sRate,  \n");
                    sb.Append("v.Acct as Vendor \n");
                    sb.Append("FROM Invoice i \n");
                    sb.Append("	INNER JOIN Loc l ON l.Loc = i.Loc \n");
                    sb.Append("	INNER JOIN Owner o ON o.id = l.owner \n");
                    sb.Append("	INNER JOIN Rol r ON r.id = o.Rol \n");
                    sb.Append("	INNER JOIN STax s ON i.TaxRegion = s.Name \n");
                    sb.Append("LEFT JOIN Vendor v ON s.Vendor = v.ID \n");
                    sb.Append("WHERE i.Status <> 2 AND s.UType = 0 \n");
                    sb.Append("	AND  i.fDate >= '" + objInvoice.StartDate + "' \n");
                    sb.Append("	And i.fDate <= '" + objInvoice.EndDate + "' \n");
                    sb.Append("GROUP BY s.State, s.fDesc, s.Name, s.Rate,v.Acct\n");
                    sb.Append("ORDER BY s.Name\n");
                }

                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesTax2(Invoices objInvoice)
        {
            try
            {
                string sql = "Select I.fDate, I.Ref,l.tag as locName, r.Name As CustName, I.fDesc,";
                sql = sql + "I.Amount,I.Stax, (I.Taxable*I.TaxFactor*I.TaxRate2)/10000 AS STax2, I.Total, I.TaxRegion,S.Name,S.Rate AS TaxRate2,I.TaxFactor,I.Taxable,";
                sql = sql + "L.ID,S.State,S.fDesc As sDesc,S.Name As sName,S.Rate As sRate from Invoice I ";
                sql = sql + "Inner Join Loc L On L.Loc = I.Loc  inner join owner o on o.id = l.owner inner join rol r ";
                sql = sql + "on r.id = o.Rol Inner Join STax S On L.STax2 LIKE '%' + S.Name + '%'  AND I.fDate >= '" + objInvoice.StartDate + "' And I.fDate <= '" + objInvoice.EndDate + "'  ORDER BY S.Name, I.fDate";
                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARRevenueCust(Contracts objContract)
        {
            try
            {
                var para = new SqlParameter[8];

                para[0] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.CustID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.Loc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.Date,
                    Value = objContract.StartDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.Date,
                    Value = objContract.EndDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.SearchValue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "searchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.SearchBy
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "filterBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.filterBy
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "filterValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.filterValue
                };
                return objContract.Ds = SqlHelper.ExecuteDataset(objContract.ConnConfig, CommandType.StoredProcedure, "spGetARRevenueCust", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARRevenueCust(GetARRevenueCustParam _GetARRevenueCust, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[8];

                para[0] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetARRevenueCust.CustID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetARRevenueCust.Loc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.Date,
                    Value = _GetARRevenueCust.StartDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.Date,
                    Value = _GetARRevenueCust.EndDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenueCust.SearchValue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "searchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenueCust.SearchBy
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "filterBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenueCust.filterBy
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "filterValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetARRevenueCust.filterValue
                };
                return _GetARRevenueCust.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARRevenueCust", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARRevenueCustShowAll(Contracts objContract)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.CustID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = objContract.Loc
                };

                return objContract.Ds = SqlHelper.ExecuteDataset(objContract.ConnConfig, CommandType.StoredProcedure, "spGetARRevenueCustShowAll", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARRevenueCustShowAll(GetARRevenueCustShowAllParam _GetARRevenueCustShowAll, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetARRevenueCustShowAll.CustID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetARRevenueCustShowAll.Loc
                };

                return _GetARRevenueCustShowAll.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARRevenueCustShowAll", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesTaxCollected(Invoices objInvoice)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.EndDate
                };

                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.StoredProcedure, "spGetSalesTaxCollected", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesTaxCollectedDetail(Invoices objInvoice)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.EndDate
                };

                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.StoredProcedure, "spGetSalesTaxCollectedDetail", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesTaxCollectedByVendor(Invoices objInvoice)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.EndDate
                };

                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.StoredProcedure, "spGetSalesTaxCollectedByVendor", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesTaxCollectedDetailByVendor(Invoices objInvoice)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.EndDate
                };

                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.StoredProcedure, "spGetSalesTaxCollectedDetailByVendor", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetHistoryTransaction(String conn, int id, int type, int owner, int loc, String status, int tid)
        {
            try
            {
                var para = new SqlParameter[6];

                para[0] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.Int,
                    Value = id
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.Int,
                    Value = type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = owner
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = loc
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "status",
                    SqlDbType = SqlDbType.VarChar,
                    Value = status
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "transID",
                    SqlDbType = SqlDbType.Int,
                    Value = tid
                };
                return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetHistoryTransaction", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
