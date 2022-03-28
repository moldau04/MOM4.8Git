using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace DataLayer
{
    public class DL_Deposit
    {
        public DataSet GetAllInvoices(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.ref is not null \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByLocID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" SELECT DISTINCT i.Ref,l.Owner, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.Status AS StatusID, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                0 AS TransID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS PrevDueAmount, \n");
            varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                0 AS PaymentID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS DueAmount,    \n");
            varname1.Append("                isnull(i.Total,0.00) AS OrigAmount, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                i.loc, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append(" FROM   Invoice i \n");
            else
                varname1.Append(" FROM   MS_Invoice i  \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            //varname1.Append("        INNER JOIN Trans t \n");
            //varname1.Append("        	    ON t.Ref = i.Ref where Type = 1 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.Loc='" + objPropContracts.Loc + "' AND i.Status != 1 AND i.Status != 2  \n");

            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append(" SELECT Loc, isnull(Balance,0) as Balance FROM Loc WHERE Loc =" + objPropContracts.Loc + " \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByRef(Invoices _objInv)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objInv.ConnConfig, CommandType.Text, "SELECT fDate,Ref,fDesc,Amount,STax,Total,TaxRegion,TaxRate,TaxFactor,Taxable,Type,Job,Loc,Terms,PO,Status,Batch,Remarks,TransID,GTax,Mech,Pricing,TaxRegion2,TaxRate2,BillToOpt,BillTo,Custom1,Custom2,IDate,fUser,Custom3,QBInvoiceID,LastUpdateDate FROM Invoice WHERE Ref=" + _objInv.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateInvoice(Invoices _objInv)
        {
            try
            {
                string query = "UPDATE Invoice SET Status = @Status WHERE Ref = @Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objInv.Ref));
                parameters.Add(new SqlParameter("@Status", _objInv.Status));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objInv.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPaymentDetails(PaymentDetails _objPayment)
        {
            try
            {
                string query = "INSERT INTO PaymentDetails(ReceivedPaymentID,TransID,InvoiceID)VALUES(@ReceivedPaymentID,@TransID,@InvoiceID)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReceivedPaymentID", _objPayment.ReceivedPaymentID));
                parameters.Add(new SqlParameter("@TransID", _objPayment.TransID));
                parameters.Add(new SqlParameter("@InvoiceID", _objPayment.InvoiceID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPayment.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByInvoiceID(Transaction _objTrans)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT Acct, AcctSub, Amount, Batch, EN, ID, Line, Ref, Sel, Status, Type, VDoub, VInt, fDate, fDesc, strRef FROM Trans WHERE Type=1 AND Ref=" + _objTrans.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllReceivePayment(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT rp.ID,isnull(l.Owner,0) as Owner,isnull(ro.Name,'') AS customerName,rp.Loc,l.Tag,rp.Amount,rp.PaymentReceivedDate,rp.fDesc, \n");
                varname.Append("(CASE rp.PaymentMethod  \n");
                varname.Append(" WHEN 0 THEN 'Check' \n");
                varname.Append(" WHEN 1 THEN 'Cash' END) AS PaymentMethod, \n");
                varname.Append(" (case isnull(rp.Status,0) WHEN 0 then 'Open' WHEN 1 then 'Deposited' END) as StatusName,  \n");
                varname.Append(" rp.CheckNumber,rp.AmountDue,isnull(rp.Status,0) as Status FROM ReceivedPayment rp \n");
                //varname.Append(" left outer join Loc l on l.Loc=rp.Loc  \n");
                //varname.Append(" left outer join Rol r on r.ID=l.Rol \n");
                varname.Append(" LEFT JOIN owner o ON rp.Owner = o.ID \n");
                varname.Append(" LEFT JOIN rol ro ON o.Rol = ro.ID  \n");
                varname.Append(" LEFT JOIN Loc l ON rp.loc = l.loc \n");
                varname.Append(" WHERE (rp.PaymentReceivedDate >= '" + _objReceiPmt.StartDate + "') AND (rp.PaymentReceivedDate <= '" + _objReceiPmt.EndDate + "')");
                //varname.Append(" WHERE NOT EXISTS (SELECT * FROM DepositDetails dep WHERE dep.ReceivedPaymentID = rp.ID) \n");
                varname.Append(" ORDER BY rp.ID \n");
                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePaymentByID(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT r.ID,isnull(r.Owner,0) as Owner,isnull(ro.Name,'') As RolName,r.Loc,isnull(l.Tag,'') as Tag,r.Amount,r.PaymentReceivedDate,  \n");
                varname.Append("            r.PaymentMethod,r.CheckNumber,r.AmountDue,r.fDesc, isnull(r.Status,0) as Status, isnull(ro.EN,0) as EN, isnull(B.Name,'') as Company,isnull(d.DepID,0) as DepID  \n");
                varname.Append("            FROM ReceivedPayment r      \n");
                varname.Append("            LEFT JOIN owner o ON r.Owner = o.ID     \n");
                varname.Append("            LEFT JOIN rol ro ON o.Rol = ro.ID      \n");
                varname.Append("            LEFT JOIN Branch B on ro.EN = B.ID     \n");
                varname.Append("            LEFT JOIN Loc l ON r.loc = l.loc     \n");
                varname.Append("            LEFT JOIN DepositDetails d on d.ReceivedPaymentID=r.ID     \n");
                varname.Append("                 WHERE r.ID=" + _objReceiPmt.ID);
                //if(_objReceiPmt.Loc!=0)
                //{
                //    if (_objReceiPmt.page == "addlocation")
                //    {
                //        varname.Append(" and r.Loc=" + _objReceiPmt.Loc);
                //    }
                //    else
                //    {
                //        varname.Append(" and r.Owner=" + _objReceiPmt.Loc);
                //    }

                //}
                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetReceivePaymentByID(GetReceivePaymentByIDParam _GetReceivePaymentByID, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT r.ID,isnull(r.Owner,0) as Owner,isnull(ro.Name,'') As RolName,r.Loc,isnull(l.Tag,'') as Tag,r.Amount,r.PaymentReceivedDate,  \n");
                varname.Append("            r.PaymentMethod,r.CheckNumber,r.AmountDue,r.fDesc, isnull(r.Status,0) as Status, isnull(ro.EN,0) as EN, isnull(B.Name,'') as Company,isnull(d.DepID,0) as DepID  \n");
                varname.Append("            FROM ReceivedPayment r      \n");
                varname.Append("            LEFT JOIN owner o ON r.Owner = o.ID     \n");
                varname.Append("            LEFT JOIN rol ro ON o.Rol = ro.ID      \n");
                varname.Append("            LEFT JOIN Branch B on ro.EN = B.ID     \n");
                varname.Append("            LEFT JOIN Loc l ON r.loc = l.loc     \n");
                varname.Append("            LEFT JOIN DepositDetails d on d.ReceivedPaymentID=r.ID     \n");
                varname.Append("                 WHERE r.ID=" + _GetReceivePaymentByID.ID);
                //if(_objReceiPmt.Loc!=0)
                //{
                //    if (_objReceiPmt.page == "addlocation")
                //    {
                //        varname.Append(" and r.Loc=" + _objReceiPmt.Loc);
                //    }
                //    else
                //    {
                //        varname.Append(" and r.Owner=" + _objReceiPmt.Loc);
                //    }

                //}
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePaymentLogs(ReceivedPayment _objReceiPmt)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + _objReceiPmt.ID + "  and Screen='ReceivePayment' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetReceivePaymentLogs(GetReceivePaymentLogsParam _GetReceivePaymentLogs, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from Log2 where ref =" + _GetReceivePaymentLogs.ID + "  and Screen='ReceivePayment' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentByReceivedID(PaymentDetails _objPayment)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPayment.ConnConfig, CommandType.Text, "SELECT ID,ReceivedPaymentID,TransID,InvoiceID FROM PaymentDetails WHERE ReceivedPaymentID=" + _objPayment.ReceivedPaymentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref,                     \n");
            varname1.Append("                fDate,                     \n");
            varname1.Append("                l.ID,                      \n");
            varname1.Append("                l.Tag,                     \n");
            varname1.Append("                i.Amount,                  \n");
            varname1.Append("                i.STax,                    \n");
            varname1.Append("                i.Total,                   \n");
            varname1.Append("                i.Status AS StatusID,              \n");
            varname1.Append("                i.custom1 as manualInv,            \n");
            varname1.Append("                " + objPropContracts.TransID + " AS TransID,                   \n");
            varname1.Append("                " + objPropContracts.PaymentID + " AS PaymentID,               \n");
            //varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                (isnull((Select tra.Amount FROM Trans as tra WHERE tra.Type = 98 AND tra.ID=" + objPropContracts.TransID + "),0.00) +                          \n");
            varname1.Append("                isnull((select (i.Total -sum(t.amount)) from (select distinct t.Amount from PaymentDetails p, Trans t where p.InvoiceID = t.ref and t.type = 98 and p.InvoiceID = " + objPropContracts.Ref + ") t), i.Total)) AS PrevDueAmount,     \n");
            varname1.Append("                isnull((Select tra.Amount FROM Trans as tra WHERE tra.Type = 98 AND tra.ID=" + objPropContracts.TransID + "),0.00) AS paymentAmt,              \n");
            //varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 AND tr.Ref=i.Ref),i.Total) AS DueAmount,");
            //varname1.Append("                isnull((select (i.Total - sum(t.Amount)) from PaymentDetails p inner join Trans t on p.InvoiceID = t.ref where p.InvoiceID = i.Ref and t.type = 98), i.Total)  AS DueAmount,");
            //varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 6 AND tr.ID=" + objPropContracts.TransID + "),i.Total) AS DueAmount,");
            varname1.Append("                isnull((select (i.Total -sum(t.amount)) from (select distinct t.Amount from PaymentDetails p, Trans t where p.InvoiceID = t.ref and t.type = 98 and p.InvoiceID = " + objPropContracts.Ref + ") t), i.Total)  AS DueAmount,  \n");
            varname1.Append("                isnull(i.Total,0) AS OrigAmount,       \n");
            varname1.Append("                (CASE i.status                         \n");
            varname1.Append("                  WHEN 0 THEN 'Open'           \n");
            varname1.Append("                  WHEN 1 THEN 'Paid'           \n");
            varname1.Append("                  WHEN 2 THEN 'Voided'         \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending'      \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card'    \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid'         \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status,      \n");
            varname1.Append("                i.PO,                                      \n");
            varname1.Append("                r.Name                  AS customername,   \n");
            varname1.Append("                i.loc,                                     \n");
            varname1.Append("                (SELECT Type                               \n");
            varname1.Append("                 FROM   JobType jt                         \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type,           \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i                           \n");
            else
                varname1.Append("FROM   MS_Invoice i                        \n");
            varname1.Append("       INNER JOIN Loc l                        \n");
            varname1.Append("               ON l.Loc = i.Loc                \n");
            varname1.Append("       INNER JOIN owner o                      \n");
            varname1.Append("               ON o.id = l.owner               \n");
            varname1.Append("       INNER JOIN rol r                        \n");
            varname1.Append("               ON o.rol = r.id                 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip    \n");
            varname1.Append("               ON i.ref = ip.ref where i.Ref=" + objPropContracts.Ref + "      \n");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePaymentDetailsByID(ReceivedPayment _objReceiPmt)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" SELECT rp.ID,rp.Owner,ro.ID as Rol,isnull(rp.Loc,0) as Loc,ro.Name AS customerName,l.Tag,rp.Amount,rp.PaymentReceivedDate,rp.fDesc,  \n");
                varname.Append("(CASE rp.PaymentMethod  \n");
                varname.Append(" WHEN 0 THEN 'Check' \n");
                varname.Append(" WHEN 1 THEN 'Cash' END) AS PaymentMethod, \n");
                varname.Append("    isnull(ro.EN,0) as EN,                  \n");
                varname.Append("    isnull(B.Name,'') as Company,           \n");
                varname.Append(" rp.CheckNumber,rp.AmountDue FROM ReceivedPayment rp \n");
                varname.Append("     left outer join Owner o on o.ID = rp.Owner \n");
                //varname.Append("     left outer join Loc lo on lo.Owner = o.ID \n");
                varname.Append("     left outer join Rol ro on ro.ID = o.Rol \n");
                varname.Append("     left outer join Loc l on l.Loc=rp.Loc  \n");
                varname.Append("     left outer join Branch B on ro.EN = B.ID \n");
                varname.Append("     WHERE rp.ID=" + _objReceiPmt.ID + "  ORDER BY rp.ID ");
                //varname.Append(" left outer join Loc l on l.Loc=rp.Loc  \n");
                //varname.Append(" left outer join Rol r on r.ID=l.Rol WHERE rp.ID=" + _objReceiPmt.ID + " ORDER BY rp.ID \n");

                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
                //return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, "SELECT ID,Loc,Amount,PaymentReceivedDate,PaymentMethod,CheckNumber,AmountDue,fDesc FROM ReceivedPayment WHERE ID=" + _objReceiPmt.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddDeposit(Dep _objDep)
        {
            try
            {
                string query = "DECLARE @Ref INT; SELECT @Ref=ISNULL(MAX(Ref),0)+1 FROM Dep; INSERT INTO Dep(Ref,fDate,Bank,fDesc,Amount,TransID)VALUES(@Ref,@fDate,@Bank,@fDesc,@Amount,@TransID); SELECT @Ref AS DepID;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                //parameters.Add(new SqlParameter("@ReceivedPaymentID", _objDep.ReceivedPaymentID));
                parameters.Add(new SqlParameter("@fDate", _objDep.fDate));
                parameters.Add(new SqlParameter("@Bank", _objDep.Bank));
                parameters.Add(new SqlParameter("@fDesc", _objDep.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objDep.Amount));
                parameters.Add(new SqlParameter("@TransID", _objDep.TransID));
                _objDep.DsID = SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
                return Convert.ToInt32(_objDep.DsID.Tables[0].Rows[0]["DepID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDeposit(Dep _objDep)
        {
            try
            {
                string query = "UPDATE Dep SET fDate = @fDate, Bank = @Bank, fDesc = @fDesc, Amount = @Amount WHERE Ref=@Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                parameters.Add(new SqlParameter("@fDate", _objDep.fDate));
                parameters.Add(new SqlParameter("@Bank", _objDep.Bank));
                parameters.Add(new SqlParameter("@fDesc", _objDep.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objDep.Amount));
                //parameters.Add(new SqlParameter("@TransID", _objDep.TransID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateDeposit(DepositInfor_UpdateDepositParam _DepositInfor_UpdateDeposit, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Dep SET fDate = @fDate, Bank = @Bank, fDesc = @fDesc, Amount = @Amount WHERE Ref=@Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _DepositInfor_UpdateDeposit.Ref));
                parameters.Add(new SqlParameter("@fDate", _DepositInfor_UpdateDeposit.fDate));
                parameters.Add(new SqlParameter("@Bank", _DepositInfor_UpdateDeposit.Bank));
                parameters.Add(new SqlParameter("@fDesc", _DepositInfor_UpdateDeposit.fDesc));
                parameters.Add(new SqlParameter("@Amount", _DepositInfor_UpdateDeposit.Amount));
                //parameters.Add(new SqlParameter("@TransID", _objDep.TransID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddDepositDetails(DepositDetails _objDepDetails)
        {
            try
            {
                string query = "INSERT INTO DepositDetails(DepID,ReceivedPaymentID)VALUES(@DepID,@ReceivedPaymentID);";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                parameters.Add(new SqlParameter("@DepID", _objDepDetails.DepID));
                parameters.Add(new SqlParameter("@ReceivedPaymentID", _objDepDetails.ReceivedPaymentID));

                SqlHelper.ExecuteDataset(_objDepDetails.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllDeposits(Dep _objDep)
        {
            try
            {
                string query = "SELECT 0 as Batch, d.Ref,d.fDate,d.Bank,d.fDesc,isnull(d.Amount,0) as Amount,d.TransID,b.fDesc As BankName, isnull(t.sel,0) as IsRecon, (case when (isnull(t.sel,0) = 0) then 'Open' else 'Reconciled' end) as Status,isnull(r.EN,0) as EN, isnull(BR.Name,'') as Company " +
                    "FROM Dep d with (Nolock) " +
                    "LEFT JOIN Bank b with (Nolock) ON d.Bank = b.ID " +
                    "LEFT JOIN Trans t with (Nolock) ON t.ID = d.TransID " +
                    "LEFT JOIN Rol r with (Nolock) on r.ID = b.Rol  " +
                    "LEFT JOIN Branch BR with (Nolock) on r.EN = BR.ID  ";
                if (_objDep.EN == 1)
                {
                    query += "     LEFT JOIN tblUserCo UC ON UC.CompanyID = r.EN ";
                }
                if ((_objDep.StartDate != DateTime.MinValue) && (_objDep.EndDate != DateTime.MinValue))
                {
                    query += "  WHERE (d.fDate >= '" + _objDep.StartDate + "') AND (d.fDate <= '" + _objDep.EndDate + "') ";
                }
                if(_objDep.EN == 1)
                {
                    query += "     AND UC.IsSel = 1 and UC.UserID = " + _objDep.UserID;
                }
                query += "      ORDER BY Ref";
                return SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAllDeposits(GetAllDepositsParam _GetAllDeposits, string ConnectionString)
        {
            try
            {
                string query = "SELECT 0 as Batch, d.Ref,d.fDate,d.Bank,d.fDesc,isnull(d.Amount,0) as Amount,d.TransID,b.fDesc As BankName, isnull(t.sel,0) as IsRecon, (case when (isnull(t.sel,0) = 0) then 'Open' else 'Reconciled' end) as Status,isnull(r.EN,0) as EN, isnull(BR.Name,'') as Company " +
                    "FROM Dep d with (Nolock) " +
                    "LEFT JOIN Bank b with (Nolock) ON d.Bank = b.ID " +
                    "LEFT JOIN Trans t with (Nolock) ON t.ID = d.TransID " +
                    "LEFT JOIN Rol r with (Nolock) on r.ID = b.Rol  " +
                    "LEFT JOIN Branch BR with (Nolock) on r.EN = BR.ID  ";
                if (_GetAllDeposits.EN == 1)
                {
                    query += "     LEFT JOIN tblUserCo UC ON UC.CompanyID = r.EN ";
                }
                if ((_GetAllDeposits.StartDate != DateTime.MinValue) && (_GetAllDeposits.EndDate != DateTime.MinValue))
                {
                    query += "  WHERE (d.fDate >= '" + _GetAllDeposits.StartDate + "') AND (d.fDate <= '" + _GetAllDeposits.EndDate + "') ";
                }
                if (_GetAllDeposits.EN == 1)
                {
                    query += "     AND UC.IsSel = 1 and UC.UserID = " + _GetAllDeposits.UserID;
                }
                query += "      ORDER BY Ref";
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDepByID(Dep _objDep)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.Text, "SELECT d.Ref,d.fDate,d.Bank,d.fDesc,d.Amount,d.TransID,d.EN, isnull(t.Sel,0) as IsRecon FROM Dep d LEFT JOIN Trans t ON t.ID = d.TransID WHERE d.Ref=" + _objDep.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetDepByID(GetDepByIDParam _GetDepByID, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT d.Ref,d.fDate,d.Bank,d.fDesc,d.Amount,d.TransID,d.EN, isnull(t.Sel,0) as IsRecon FROM Dep d LEFT JOIN Trans t ON t.ID = d.TransID WHERE d.Ref=" + _GetDepByID.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDepHeadByID(Dep _objDep)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.Text, "SELECT d.Ref,d.fDate,(Select fDesc from Bank b where b.ID = d.Bank) As BankName,d.fDesc,d.Amount,d.TransID,d.EN, isnull(t.Sel,0) as IsRecon FROM Dep d LEFT JOIN Trans t ON t.ID = d.TransID WHERE d.Ref=" + _objDep.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetDepHeadByID(GetDepHeadByIDParam _GetDepHeadByID, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT d.Ref,d.fDate,(Select fDesc from Bank b where b.ID = d.Bank) As BankName,d.fDesc,d.Amount,d.TransID,d.EN, isnull(t.Sel,0) as IsRecon FROM Dep d LEFT JOIN Trans t ON t.ID = d.TransID WHERE d.Ref=" + _GetDepHeadByID.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivedPaymentByDep(ReceivedPayment objReceivePay)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "dep";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objReceivePay.DepID;

                return objReceivePay.Ds = SqlHelper.ExecuteDataset(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spGetDepositByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetReceivedPaymentByDep(GetReceivedPaymentByDepParam _GetReceivedPaymentByDep, string ConnectionString)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "dep";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _GetReceivedPaymentByDep.DepID;

                return _GetReceivedPaymentByDep.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetDepositByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllReceivePaymentForDep(ReceivedPayment objReceivePay)
        {
            try
            {

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@PaymentReceivedDate";
                param.SqlDbType = SqlDbType.DateTime;
                param.Value = objReceivePay.PaymentReceivedDate;

                return SqlHelper.ExecuteDataset(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spGetAllReceivePaymentForDep", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAllReceivePaymentForDep(GetAllReceivePaymentForDepParam _GetAllReceivePaymentForDep, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT rp.owner, rp.ID,    \n");
                varname.Append("		o.Rol,    \n");
                varname.Append("		r.Name AS customerName,     \n");
                varname.Append("        isnull(rp.loc,0) as loc, \n");
                varname.Append("        isnull(lo.Tag,'') as Tag,  \n");
                varname.Append("    isnull(r.EN,0) as EN,         \n");
                varname.Append("    isnull(B.Name,'') as Company,  \n");
                varname.Append("        rp.Amount,rp.PaymentReceivedDate,rp.fDesc,  \n");
                varname.Append("        (CASE rp.PaymentMethod      \n");
                varname.Append("         WHEN 0 THEN 'Check'    \n");
                varname.Append("         WHEN 1 THEN 'Cash'     \n");
                varname.Append("         WHEN 2 THEN 'Wire Transfer'     \n");
                varname.Append("         WHEN 3 THEN 'ACH'      \n");
                varname.Append("         WHEN 4 THEN 'Credit Card'  \n");
                varname.Append("         WHEN 5 THEN 'e-Transfer'  \n");
                varname.Append("         WHEN 6 THEN 'Lockbox' END) AS PaymentMethod   \n");
                varname.Append("         ,rp.CheckNumber,rp.AmountDue FROM ReceivedPayment rp   \n");
                varname.Append("         LEFT JOIN owner o ON o.ID =rp.Owner     \n");
                //varname.Append("         LEFT JOIN Loc l ON l.Owner = o.ID      \n");
                varname.Append("         LEFT JOIN Rol r on r.ID = o.Rol        \n");
                varname.Append("         LEFT JOIN Loc lo ON lo.Loc = rp.Loc \n");
                varname.Append("        LEFT JOIN Branch B on r.EN = B.ID  \n");
                varname.Append("         WHERE NOT EXISTS (SELECT * FROM DepositDetails dep WHERE dep.ReceivedPaymentID = rp.ID)    \n");
                varname.Append("         AND rp.PaymentReceivedDate <= '" + _GetAllReceivePaymentForDep.PaymentReceivedDate + "' \n");
                varname.Append("         ORDER BY rp.ID    \n");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddOpenARDetails(OpenAR _objOpenAR)
        {
            try
            {
                string query = "INSERT INTO OpenAR(Loc,fDate,Due,Type,Ref,fDesc,Original,Balance,Selected,TransID,InvoiceID) VALUES(@Loc,@fDate,@Due,@Type,@Ref,@fDesc,@Original,@Balance,@Selected,@TransID,@InvoiceID)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objOpenAR.Ref));
                parameters.Add(new SqlParameter("@fDate", _objOpenAR.fDate));
                parameters.Add(new SqlParameter("@Loc", _objOpenAR.Loc));
                //parameters.Add(new SqlParameter("@fDate", _objOpenAR.fDate));
                parameters.Add(new SqlParameter("@Due", _objOpenAR.Due));
                parameters.Add(new SqlParameter("@Type", _objOpenAR.Type));
                parameters.Add(new SqlParameter("@fDesc", _objOpenAR.fDesc));
                parameters.Add(new SqlParameter("@Original", _objOpenAR.Original));
                parameters.Add(new SqlParameter("@Balance", _objOpenAR.Balance));
                parameters.Add(new SqlParameter("@Selected", _objOpenAR.Selected));
                parameters.Add(new SqlParameter("@TransID", _objOpenAR.TransID));
                parameters.Add(new SqlParameter("@InvoiceID", _objOpenAR.InvoiceID));

                SqlHelper.ExecuteDataset(_objOpenAR.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentDetailsByReceivedID(PaymentDetails _objPayment) // with Payment details from Trans table, Invoice table and PaymentDetails table 
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPayment.ConnConfig, CommandType.Text, "SELECT p.ID,p.ReceivedPaymentID,p.TransID,p.InvoiceID,t.Amount As PaidAmount,i.Total AS TotalAmount, i.fDate As InvoiceDate,i.DDate As DueDate, i.Loc FROM PaymentDetails p, Trans t, Invoice i WHERE p.TransID=t.ID AND p.InvoiceID=i.Ref AND p.ReceivedPaymentID=" + _objPayment.ReceivedPaymentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      


        public DataSet GetPaymentByReceivedBatch(PJ _objPj)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPj.ConnConfig, CommandType.Text, "SELECT P.TransID,T.ID, FROM PaymentDetails P JOIN Trans T on P.TransID=t.ID WHERE t.Batch=" + _objPj.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatch(Transaction _objTrans)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct FROM Trans WHERE Batch=" + _objTrans.BatchID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetByReceivedPaymentByTransID(PaymentDetails _objPmtDetail)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPmtDetail.ConnConfig, CommandType.Text, "SELECT R.ID,R.Loc,R.Amount,R.PaymentReceivedDate,R.PaymentMethod,R.CheckNumber,R.AmountDue,R.fDesc,P.ReceivedPaymentID FROM ReceivedPayment R Join PaymentDetails P on R.ID=P.ReceivedPaymentID WHERE P.TransID=" + _objPmtDetail.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDepRecon(Dep _objDep)
        {
            try
            {
                string query = "UPDATE Dep SET IsRecon=@IsRecon WHERE Ref=@Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objDep.IsRecon));
                parameters.Add(new SqlParameter("@Ref", _objDep.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepositDetails(Dep _objDep)
        {
            try
            {
                var para = new SqlParameter[2];

                //para[1] = new SqlParameter
                //{
                //    ParameterName = "@Year",
                //    SqlDbType = SqlDbType.Int,
                //    Value = _objDep.fDateYear
                //};
                para[0] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objDep.Bank
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objDep.fDate
                };

                return _objDep.Ds = SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.StoredProcedure, "spGetDepositDetailsByBank", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePayment(ReceivedPayment objReceivePay)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objReceivePay.ConnConfig, "spDeleteReceivedPayment", objReceivePay.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void DeletePayment(DeletePaymentParam _DeletePayment, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "spDeleteReceivedPayment", _DeletePayment.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteDeposit(Dep _objDep)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objDep.Ref;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@isDeleteReceive";
                para[1].SqlDbType = SqlDbType.Bit;
                para[1].Value = _objDep.isDeleteReceive;

               SqlHelper.ExecuteNonQuery(_objDep.ConnConfig, CommandType.StoredProcedure, "spDeleteDeposit", para);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public void DeleteDeposit(DeleteDepositParam _DeleteDeposit, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _DeleteDeposit.Ref;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@isDeleteReceive";
                para[1].SqlDbType = SqlDbType.Bit;
                para[1].Value = _DeleteDeposit.isDeleteReceive;

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spDeleteDeposit", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateReceivedPayStatus(ReceivedPayment objReceivePay)
        {
            try
            {
                string query = "UPDATE ReceivedPayment SET Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Status", objReceivePay.Status));
                parameters.Add(new SqlParameter("@ID", objReceivePay.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(objReceivePay.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateReceivedPayStatus(UpdateReceivedPayStatusParam _UpdateReceivedPayStatus, string ConnectionString)
        {
            try
            {
                string query = "UPDATE ReceivedPayment SET Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Status", _UpdateReceivedPayStatus.Status));
                parameters.Add(new SqlParameter("@ID", _UpdateReceivedPayStatus.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePayment(ReceivedPayment _objReceivePay,int locCredit=0)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[11];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePay";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = _objReceivePay.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "id";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _objReceivePay.ID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _objReceivePay.Loc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "amount";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = _objReceivePay.Amount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "dueAmount";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = _objReceivePay.AmountDue;

                para[5] = new SqlParameter();
                para[5].ParameterName = "payDate";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = _objReceivePay.PaymentReceivedDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "payMethod";
                para[6].SqlDbType = SqlDbType.SmallInt;
                para[6].Value = _objReceivePay.PaymentMethod;

                para[7] = new SqlParameter();
                para[7].ParameterName = "checknum";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = _objReceivePay.CheckNumber;

                para[8] = new SqlParameter();
                para[8].ParameterName = "fDesc";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = _objReceivePay.fDesc;

                para[9] = new SqlParameter();
                para[9].ParameterName = "@UpdatedBy";
                para[9].SqlDbType = SqlDbType.VarChar;
                para[9].Value = _objReceivePay.MOMUSer;

                para[10] = new SqlParameter();
                para[10].ParameterName = "@LocCredit";
                para[10].SqlDbType = SqlDbType.Int;
                para[10].Value =locCredit;

                SqlHelper.ExecuteNonQuery(_objReceivePay.ConnConfig, CommandType.StoredProcedure, "spUpdateReceivePay", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateReceivePayment(UpdateReceivePaymentParam _UpdateReceivePayment, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[10];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePay";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = _UpdateReceivePayment.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "id";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _UpdateReceivePayment.ID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _UpdateReceivePayment.Loc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "amount";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = _UpdateReceivePayment.Amount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "dueAmount";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = _UpdateReceivePayment.AmountDue;

                para[5] = new SqlParameter();
                para[5].ParameterName = "payDate";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = _UpdateReceivePayment.PaymentReceivedDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "payMethod";
                para[6].SqlDbType = SqlDbType.SmallInt;
                para[6].Value = _UpdateReceivePayment.PaymentMethod;

                para[7] = new SqlParameter();
                para[7].ParameterName = "checknum";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = _UpdateReceivePayment.CheckNumber;

                para[8] = new SqlParameter();
                para[8].ParameterName = "fDesc";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = _UpdateReceivePayment.fDesc;

                para[9] = new SqlParameter();
                para[9].ParameterName = "@UpdatedBy";
                para[9].SqlDbType = SqlDbType.VarChar;
                para[9].Value = _UpdateReceivePayment.MOMUSer;

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateReceivePay", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceByCustID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref,l.Owner, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.Status AS StatusID, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                0 AS TransID, \n");
            varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                0 AS PaymentID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS DueAmount,");
            varname1.Append("                isnull(i.Total,0.00) AS OrigAmount, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i  \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            //varname1.Append("        INNER JOIN Trans t \n");
            //varname1.Append("        	    ON t.Ref = i.Ref where Type = 1 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.Loc='" + objPropContracts.Loc + "' AND i.Status != 1 AND i.Status != 2  \n");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByCustomerID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref,l.Owner, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.Status AS StatusID, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                0 AS TransID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS PrevDueAmount, \n");
            varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                0 AS PaymentID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS DueAmount,");
            varname1.Append("                isnull(i.Total,0.00) AS OrigAmount, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                i.loc, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i  \n");
            varname1.Append("       LEFT JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       LEFT JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       LEFT JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            //varname1.Append("        INNER JOIN Trans t \n");
            //varname1.Append("        	    ON t.Ref = i.Ref where Type = 1 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.Status != 1 AND i.Status != 2 AND o.ID= " + objPropContracts.Rol + "\n");

            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT ID, isnull(Balance,0) as Balance FROM Owner WHERE ID =" + objPropContracts.Rol + " \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetInvoiceByCustomerID(GetInvoiceByCustomerIDParam _GetInvoiceByCustomerID, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT i.Ref,l.Owner, \n");
            varname1.Append("                fDate, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                i.Status AS StatusID, \n");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                0 AS TransID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS PrevDueAmount, \n");
            varname1.Append("                0.00 as paymentAmt, \n");
            varname1.Append("                0 AS PaymentID, \n");
            varname1.Append("                isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS DueAmount,");
            varname1.Append("                isnull(i.Total,0.00) AS OrigAmount, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                i.loc, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance \n");
            if (_GetInvoiceByCustomerID.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i  \n");
            varname1.Append("       LEFT JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       LEFT JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       LEFT JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            //varname1.Append("        INNER JOIN Trans t \n");
            //varname1.Append("        	    ON t.Ref = i.Ref where Type = 1 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref where i.Status != 1 AND i.Status != 2 AND o.ID= " + _GetInvoiceByCustomerID.Rol + "\n");

            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT ID, isnull(Balance,0) as Balance FROM Owner WHERE ID =" + _GetInvoiceByCustomerID.Rol + " \n");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePayment(ReceivedPayment _objReceivePay)
        {
            try
            {
                string query = "UPDATE ReceivedPayment SET Owner = @Owner WHERE ID=" + _objReceivePay.ID;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Owner", _objReceivePay.Rol));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objReceivePay.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentID(ReceivedPayment _objReceivePay)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append(" \n");
                varname1.Append("SELECT ID, isnull(Balance,0) as Balance FROM Owner WHERE ID =" + _objReceivePay.Loc + " \n");

                return SqlHelper.ExecuteDataset(_objReceivePay.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentCustomer(ReceivedPayment _objReceivePay)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append(" \n");
                varname1.Append("SELECT Owner FROM Loc WHERE Loc=" + _objReceivePay.Loc + " \n");

                return SqlHelper.ExecuteDataset(_objReceivePay.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesByReceivedPay(PaymentDetails objPayment)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePayId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPayment.ReceivedPaymentID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "owner";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPayment.Rol;

                para[2] = new SqlParameter();
                para[2].ParameterName = "loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objPayment.Loc;

                return SqlHelper.ExecuteDataset(objPayment.ConnConfig, CommandType.StoredProcedure, "spGetInvoicesByReceivedPay", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetInvoicesByReceivedPay(GetInvoicesByReceivedPayParam _GetInvoicesByReceivedPay, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePayId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _GetInvoicesByReceivedPay.ReceivedPaymentID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "owner";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _GetInvoicesByReceivedPay.Rol;

                para[2] = new SqlParameter();
                para[2].ParameterName = "loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _GetInvoicesByReceivedPay.Loc;

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetInvoicesByReceivedPay", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddReceivePayment(ReceivedPayment objReceivePay, int LocCredit=0)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[12];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePay";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objReceivePay.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "loc";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objReceivePay.Loc;

                para[2] = new SqlParameter();
                para[2].ParameterName = "owner";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objReceivePay.Rol;

                para[3] = new SqlParameter();
                para[3].ParameterName = "amount";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = objReceivePay.Amount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "dueAmount";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = objReceivePay.AmountDue;

                para[5] = new SqlParameter();
                para[5].ParameterName = "payDate";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = objReceivePay.PaymentReceivedDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "payMethod";
                para[6].SqlDbType = SqlDbType.SmallInt;
                para[6].Value = objReceivePay.PaymentMethod;

                para[7] = new SqlParameter();
                para[7].ParameterName = "checknum";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = objReceivePay.CheckNumber;

                para[8] = new SqlParameter();
                para[8].ParameterName = "fDesc";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = objReceivePay.fDesc;

                para[9] = new SqlParameter();
                para[9].ParameterName = "@UpdatedBy";
                para[9].SqlDbType = SqlDbType.VarChar;
                para[9].Value = objReceivePay.MOMUSer;

                para[10] = new SqlParameter();
                para[10].ParameterName = "@receivepaymentId";
                para[10].SqlDbType = SqlDbType.Int;
                para[10].Value = 0;
                para[10].Direction = ParameterDirection.Output;

                para[11] = new SqlParameter();
                para[11].ParameterName = "@LocCredit";
                para[11].SqlDbType = SqlDbType.Int;
                para[11].Value = LocCredit;
         
                SqlHelper.ExecuteNonQuery(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spAddReceivePay", para);
                return Convert.ToInt32(para[10].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public int AddReceivePayment(AddReceivePaymentParam _AddReceivePayment, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[11];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePay";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = _AddReceivePayment.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "loc";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _AddReceivePayment.Loc;

                para[2] = new SqlParameter();
                para[2].ParameterName = "owner";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _AddReceivePayment.Rol;

                para[3] = new SqlParameter();
                para[3].ParameterName = "amount";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = _AddReceivePayment.Amount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "dueAmount";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = _AddReceivePayment.AmountDue;

                para[5] = new SqlParameter();
                para[5].ParameterName = "payDate";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = _AddReceivePayment.PaymentReceivedDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "payMethod";
                para[6].SqlDbType = SqlDbType.SmallInt;
                para[6].Value = _AddReceivePayment.PaymentMethod;

                para[7] = new SqlParameter();
                para[7].ParameterName = "checknum";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = _AddReceivePayment.CheckNumber;

                para[8] = new SqlParameter();
                para[8].ParameterName = "fDesc";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = _AddReceivePayment.fDesc;

                para[9] = new SqlParameter();
                para[9].ParameterName = "@UpdatedBy";
                para[9].SqlDbType = SqlDbType.VarChar;
                para[9].Value = _AddReceivePayment.MOMUSer;

                para[10] = new SqlParameter();
                para[10].ParameterName = "@receivepaymentId";
                para[10].SqlDbType = SqlDbType.Int;
                para[10].Value = 0;
                para[10].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddReceivePay", para);
                return Convert.ToInt32(para[10].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDepositTrans(Dep objDep)
        {
            try
            {
                string query = "UPDATE Dep SET TransID = @TransID WHERE Ref=@Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objDep.Ref));
                parameters.Add(new SqlParameter("@TransID", objDep.TransID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(objDep.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllReceivePaymentAjaxSearch(ReceivedPayment _objReceiPmt,int intEN)
        {
            DataSet ds = new DataSet();
            try
            {
                _objReceiPmt.ConnConfig = HttpContext.Current.Session["config"].ToString();
                _objReceiPmt.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());

                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT rp.ID as ID,   \n");
                varname.Append("    isnull(l.Owner,0) as Owner,             \n");
                varname.Append("    isnull(ro.Name,'') AS customerName,     \n");
                varname.Append("    isnull(rp.Loc,0) as Loc,                \n");
                varname.Append("    isnull(l.Tag,'') as Tag,                \n");
                varname.Append("    isnull(rp.Amount,0) as Amount,          \n");
                varname.Append("    isnull(ro.EN,0) as EN,                  \n");
                varname.Append("    isnull(B.Name,'') as Company,           \n");
                varname.Append("    rp.PaymentReceivedDate,                 \n");
                varname.Append("    rp.fDesc,                               \n");
                varname.Append("    (CASE rp.PaymentMethod                  \n");
                varname.Append("        WHEN 0 THEN 'Check'                 \n");
                varname.Append("        WHEN 1 THEN 'Cash'                  \n");
                varname.Append("        WHEN 2 THEN 'Wire Transfer'         \n");
                varname.Append("        WHEN 3 THEN 'ACH'                   \n");
                varname.Append("        WHEN 4 THEN 'Credit Card'           \n");
                varname.Append("        WHEN 5 THEN 'e-Transfer'           \n");
                varname.Append("        WHEN 6 THEN 'Lockbox'           \n");
                varname.Append("            END) AS PaymentMethod,          \n");
                varname.Append("    (CASE isnull(rp.Status,0) WHEN 0 then 'Open' WHEN 1 then 'Deposited'  When 2 then 'Applied' END) as StatusName,  \n");
                varname.Append("    rp.CheckNumber,                         \n");
                varname.Append("    isnull(rp.AmountDue,0) as AmountDue,    \n");
                varname.Append("    isnull(rp.Status,0) as Status ,          \n");
               varname.Append("    isnull(d.DepID,0) as DepID,           \n");
                varname.Append("    isnull(rp.Batch,0) as BatchReceipt,      \n");
                varname.Append("    CONVERT(varchar, rp.ID) as Ref      \n");
                
                varname.Append("    FROM ReceivedPayment rp                 \n");
                varname.Append("        LEFT JOIN owner o ON rp.Owner = o.ID \n");
                varname.Append("        LEFT JOIN rol ro ON o.Rol = ro.ID  \n");
                varname.Append("        LEFT JOIN Branch B on ro.EN = B.ID \n");
                varname.Append("        LEFT JOIN Loc l ON rp.loc = l.loc \n");
                varname.Append("        LEFT JOIN DepositDetails d on d.ReceivedPaymentID=rp.ID \n");
                if (intEN == 1)
                {
                    varname.Append("        LEFT JOIN tblUserCo UC ON UC.CompanyID = ro.EN  \n");
                    varname.Append("        WHERE UC.IsSel= 1 and UC.UserID = " + _objReceiPmt.UserID);
                }
                varname.Append("        ORDER BY rp.ID \n");

                ds = SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());

                _objReceiPmt.Ds = ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }
        public void UpdateDepositTransBank(Transaction objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Acct = @Acct, AcctSub = @AcctSub , fdate=@fdate WHERE ID=@ID; Update Trans set fdate=@fdate where batch in (select batch from Trans where ID=@ID)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objTrans.ID));
                parameters.Add(new SqlParameter("@Acct", objTrans.Acct));
                parameters.Add(new SqlParameter("@AcctSub", objTrans.AcctSub));
                parameters.Add(new SqlParameter("@fDate", objTrans.TransDate));
                int rowsAffected = SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateDepositTransBank(UpdateDepositTransBankParam _UpdateDepositTransBank, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Trans SET Acct = @Acct, AcctSub = @AcctSub , fdate=@fdate WHERE ID=@ID; Update Trans set fdate=@fdate where batch in (select batch from Trans where ID=@ID)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _UpdateDepositTransBank.ID));
                parameters.Add(new SqlParameter("@Acct", _UpdateDepositTransBank.Acct));
                parameters.Add(new SqlParameter("@AcctSub", _UpdateDepositTransBank.AcctSub));
                parameters.Add(new SqlParameter("@fDate", _UpdateDepositTransBank.TransDate));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceNos(PaymentDetails objPayment)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[4];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Invoice";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = objPayment.strInvoiceId;

                para[1] = new SqlParameter();
                para[1].ParameterName = "ReceivedPayID";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPayment.ReceivedPaymentID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Owner";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objPayment.Owner;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Loc";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = objPayment.Loc;

                return SqlHelper.ExecuteDataset(objPayment.ConnConfig, CommandType.StoredProcedure, "spGetInvoiceNos", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceNosChange(PaymentDetails objPayment)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Invoice";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = objPayment.strInvoiceId;

           

                return SqlHelper.ExecuteDataset(objPayment.ConnConfig, CommandType.StoredProcedure, "spGetInvoiceNosChange", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetInvoiceNosChange(GetInvoiceNosChangeParam _GetInvoiceNosChange, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Invoice";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = _GetInvoiceNosChange.strInvoiceId;



                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetInvoiceNosChange", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceByList(string conn, string invoiceId,String checkNumber,Boolean isSeparate)
        {
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetInvoicesByListInvoice";
            cmd.Parameters.Add("@Invoice", SqlDbType.VarChar).Value = invoiceId;
            cmd.Parameters.Add("@CheckNumber", SqlDbType.VarChar).Value = checkNumber;
            cmd.Parameters.Add("@IsSeparate", SqlDbType.Bit).Value = isSeparate;
            cmd.Connection = con;
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }

        //API
        public DataSet GetInvoiceByList(GetInvoiceByListParam _GetInvoiceByList, string ConnectionString, string invoiceId, String checkNumber, Boolean isSeparate)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetInvoicesByListInvoice";
            cmd.Parameters.Add("@Invoice", SqlDbType.VarChar).Value = invoiceId;
            cmd.Parameters.Add("@CheckNumber", SqlDbType.VarChar).Value = checkNumber;
            cmd.Parameters.Add("@IsSeparate", SqlDbType.Bit).Value = isSeparate;
            cmd.Connection = con;
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }

        public int AddMultiReceivePayment(ReceivedPayment objReceivePay,int bank,Boolean createDeposit)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePayMulti";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objReceivePay.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "payDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = objReceivePay.PaymentReceivedDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "payMethod";
                para[2].SqlDbType = SqlDbType.SmallInt;
                para[2].Value = objReceivePay.PaymentMethod;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@UpdatedBy";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = objReceivePay.MOMUSer;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@bank";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value =bank;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@depId";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = 0;
                para[5].Direction = ParameterDirection.Output;

                para[6] = new SqlParameter();
                para[6].ParameterName = "@createDeposit";
                para[6].SqlDbType = SqlDbType.Bit;
                para[6].Value = createDeposit;


                SqlHelper.ExecuteNonQuery(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spAddReceivePayMulti", para);
                return Convert.ToInt32(para[5].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetInvoicesByReceivedPayMulti(string conn, int owner, string loc,string invoice)
        {
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetInvoicesByReceivedPayMulti";
            cmd.Parameters.Add("@owner", SqlDbType.Int).Value = owner;
            cmd.Parameters.Add("@loc", SqlDbType.VarChar).Value = loc;
            cmd.Parameters.Add("@invoice", SqlDbType.VarChar).Value = invoice;

            cmd.Connection = con;
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }

        //API
        public DataSet GetInvoicesByReceivedPayMulti(GetInvoicesByReceivedPayMultiParam _GetInvoicesByReceivedPayMulti, string ConnectionString, int owner, string loc, string invoice)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetInvoicesByReceivedPayMulti";
            cmd.Parameters.Add("@owner", SqlDbType.Int).Value = owner;
            cmd.Parameters.Add("@loc", SqlDbType.VarChar).Value = loc;
            cmd.Parameters.Add("@invoice", SqlDbType.VarChar).Value = invoice;

            cmd.Connection = con;
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }


        public DataSet GetAllInvoiceByDep(string conn, int depId)
        {
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllInvoiceInDep";
            cmd.Parameters.Add("@dep", SqlDbType.VarChar).Value = depId;           
            cmd.Connection = con;
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }

        //API
        public DataSet GetAllInvoiceByDep(GetAllInvoiceByDepParam _GetAllInvoiceByDep, string ConnectionString, int depId)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllInvoiceInDep";
            cmd.Parameters.Add("@dep", SqlDbType.VarChar).Value = depId;
            cmd.Connection = con;
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }

        public void UpdateDeposit(String conn, int depId, DataTable dtDelete, DataTable dtNew, DataTable dtDeleteGL, DataTable dtNewGL,String UpdatedBy)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[6];

                para[0] = new SqlParameter();
                para[0].ParameterName = "tblDelete";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = dtDelete;

                para[1] = new SqlParameter();
                para[1].ParameterName = "tblNew";
                para[1].SqlDbType = SqlDbType.Structured;
                para[1].Value = dtNew;

                para[2] = new SqlParameter();
                para[2].ParameterName = "tblDeleteGL";
                para[2].SqlDbType = SqlDbType.Structured;
                para[2].Value = dtDeleteGL;

                para[3] = new SqlParameter();
                para[3].ParameterName = "tblNewGL";
                para[3].SqlDbType = SqlDbType.Structured;
                para[3].Value = dtNewGL;

                para[4] = new SqlParameter();
                para[4].ParameterName = "depId";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = depId;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@UpdatedBy";
                para[5].SqlDbType = SqlDbType.VarChar;
                para[5].Value = UpdatedBy;



                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "spUpdateReceiveForDeposit", para);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateDeposit(UpdateDepositParam _UpdateDeposit, string ConnectionString, int depId, DataTable dtDelete, DataTable dtNew, DataTable dtDeleteGL, DataTable dtNewGL, String UpdatedBy)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[6];

                para[0] = new SqlParameter();
                para[0].ParameterName = "tblDelete";
                para[0].SqlDbType = SqlDbType.Structured;
                //para[0].Value = dtDelete;
                if (_UpdateDeposit.dtDelete.Rows.Count > 0)
                {
                    if (_UpdateDeposit.dtDelete.Rows[0]["Owner"].ToString() != "0")
                    {
                        para[0].Value = dtDelete;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Owner", typeof(int));
                        dt.Columns.Add("ID", typeof(int));
                        dt.Columns.Add("InvoiceID", typeof(int));
                        dt.Columns.Add("Rol", typeof(int));
                        dt.Columns.Add("customerName", typeof(String));
                        dt.Columns.Add("loc", typeof(int));
                        dt.Columns.Add("Tag", typeof(String));
                        dt.Columns.Add("En", typeof(int));
                        dt.Columns.Add("Company", typeof(String));
                        dt.Columns.Add("Amount", typeof(double));
                        dt.Columns.Add("PaymentReceivedDate", typeof(DateTime));
                        dt.Columns.Add("fDesc", typeof(String));
                        dt.Columns.Add("PaymentMethod", typeof(String));
                        dt.Columns.Add("CheckNumber", typeof(String));
                        dt.Columns.Add("AmountDue", typeof(double));
                        para[0].Value = dt;
                    }
                }

                para[1] = new SqlParameter();
                para[1].ParameterName = "tblNew";
                para[1].SqlDbType = SqlDbType.Structured;
                //para[1].Value = dtNew;
                if (_UpdateDeposit.dtNew.Rows.Count > 0)
                {
                    if (_UpdateDeposit.dtNew.Rows[0]["Owner"].ToString() != "0")
                    {
                        para[1].Value = dtNew;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Owner", typeof(int));
                        dt.Columns.Add("ID", typeof(int));
                        dt.Columns.Add("InvoiceID", typeof(int));
                        dt.Columns.Add("Rol", typeof(int));
                        dt.Columns.Add("customerName", typeof(String));
                        dt.Columns.Add("loc", typeof(int));
                        dt.Columns.Add("Tag", typeof(String));
                        dt.Columns.Add("En", typeof(int));
                        dt.Columns.Add("Company", typeof(String));
                        dt.Columns.Add("Amount", typeof(double));
                        dt.Columns.Add("PaymentReceivedDate", typeof(DateTime));
                        dt.Columns.Add("fDesc", typeof(String));
                        dt.Columns.Add("PaymentMethod", typeof(String));
                        dt.Columns.Add("CheckNumber", typeof(String));
                        dt.Columns.Add("AmountDue", typeof(double));
                        para[1].Value = dt;
                    }
                }

                para[2] = new SqlParameter();
                para[2].ParameterName = "tblDeleteGL";
                para[2].SqlDbType = SqlDbType.Structured;
                //para[2].Value = dtDeleteGL;
                if (_UpdateDeposit.dtDeleteGL.Rows.Count > 0)
                {
                    if (_UpdateDeposit.dtDeleteGL.Rows[0]["ID"].ToString() != "0")
                    {
                        para[2].Value = dtDeleteGL;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID", typeof(int));
                        dt.Columns.Add("Amount", typeof(double));
                        dt.Columns.Add("Description", typeof(String));
                        para[2].Value = dt;
                    }
                }

                para[3] = new SqlParameter();
                para[3].ParameterName = "tblNewGL";
                para[3].SqlDbType = SqlDbType.Structured;
                //para[3].Value = dtNewGL;
                if (_UpdateDeposit.dtNewGL.Rows.Count > 0)
                {
                    if (_UpdateDeposit.dtNewGL.Rows[0]["ID"].ToString() != "0")
                    {
                        para[3].Value = dtNewGL;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID", typeof(int));
                        dt.Columns.Add("Amount", typeof(double));
                        dt.Columns.Add("Description", typeof(String));
                        para[3].Value = dt;
                    }
                }

                para[4] = new SqlParameter();
                para[4].ParameterName = "depId";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = depId;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@UpdatedBy";
                para[5].SqlDbType = SqlDbType.VarChar;
                para[5].Value = UpdatedBy;



                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateReceiveForDeposit", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetGLAccount(string conn, string acct)
        {
            
            String str= "SELECT Chart.Acct, Chart.fDesc, Chart.Type, Chart.ID, Chart.CAlias, Chart.Sub FROM Chart WHERE Chart.Status=0 AND Chart.Control=0 AND Chart.Acct Like '%" + acct+ "%' ORDER BY Chart.fDesc ";
            
            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetGLAccountForDeposit(string conn, string acct)
        {

            String str = "SELECT Chart.Acct, Chart.fDesc, Chart.Type, Chart.ID, Chart.CAlias, Chart.Sub FROM Chart WHERE Chart.Status=0 AND Chart.Control=0 AND Chart.Type <> 6 AND Chart.Acct Like '%" + acct + "%' ORDER BY Chart.fDesc ";

            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetGLAccount(GetGLAccountParam _GetGLAccount, string ConnectionString, string acct)
        {

            String str = "SELECT Chart.Acct, Chart.fDesc, Chart.Type, Chart.ID, Chart.CAlias, Chart.Sub FROM Chart WHERE Chart.Status=0 AND Chart.Control=0 AND  Chart.Acct Like '%" + acct + "%' ORDER BY Chart.fDesc ";

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddDepositWithGL(Dep _objDep)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];

                para[0] = new SqlParameter();
                para[0].ParameterName = "payDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = _objDep.fDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "bank";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _objDep.Bank;

                para[2] = new SqlParameter();
                para[2].ParameterName = "receiptPay";
                para[2].SqlDbType = SqlDbType.Structured;
                para[2].Value = _objDep._dtReceipt;

                para[3] = new SqlParameter();
                para[3].ParameterName = "glAccountPay";
                para[3].SqlDbType = SqlDbType.Structured;
                para[3].Value = _objDep._dtGlAccount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "fDesc";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _objDep.fDesc;

                para[5] = new SqlParameter();
                para[5].ParameterName = "TotalAmount";
                para[5].SqlDbType = SqlDbType.Float;
                para[5].Value = _objDep.Amount;
               

                para[6] = new SqlParameter();
                para[6].ParameterName = "depId";
                para[6].SqlDbType = SqlDbType.Int;
                para[6].Value = _objDep.Ref;
                para[6].Direction = ParameterDirection.Output;




                SqlHelper.ExecuteNonQuery(_objDep.ConnConfig, CommandType.StoredProcedure, "spAddDeposit", para);
                return Convert.ToInt32(para[6].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public int AddDepositWithGL(AddDepositWithGLParam _AddDepositWithGL, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];

                para[0] = new SqlParameter();
                para[0].ParameterName = "payDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = _AddDepositWithGL.fDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "bank";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _AddDepositWithGL.Bank;

                para[2] = new SqlParameter();
                para[2].ParameterName = "receiptPay";
                para[2].SqlDbType = SqlDbType.Structured;
                para[2].Value = _AddDepositWithGL._dtReceipt;

                para[3] = new SqlParameter();
                para[3].ParameterName = "glAccountPay";
                para[3].SqlDbType = SqlDbType.Structured;
                para[3].Value = _AddDepositWithGL._dtGlAccount;

                para[4] = new SqlParameter();
                para[4].ParameterName = "fDesc";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _AddDepositWithGL.fDesc;

                para[5] = new SqlParameter();
                para[5].ParameterName = "TotalAmount";
                para[5].SqlDbType = SqlDbType.Float;
                para[5].Value = _AddDepositWithGL.Amount;


                para[6] = new SqlParameter();
                para[6].ParameterName = "depId";
                para[6].SqlDbType = SqlDbType.Int;
                para[6].Value = _AddDepositWithGL.Ref;
                para[6].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddDeposit", para);
                return Convert.ToInt32(para[6].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerUnAppliedCredit(string conn, int userId,int filter)
        {

            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@UserID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = userId;
                para[1] = new SqlParameter();
                para[1].ParameterName = "@filterby";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = filter;


                return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetCustomerUnAppliedCredit", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerUnAppliedCredit(GetCustomerUnAppliedCreditParam _GetCustomerUnAppliedCredit, string ConnectionString, int userId, int filter)
        {

            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@UserID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = userId;
                para[1] = new SqlParameter();
                para[1].ParameterName = "@filterby";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = filter;


                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCustomerUnAppliedCredit", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReceivedPaymentByDepSlip(ReceivedPayment objReceivePay)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "dep";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objReceivePay.DepID;

                return objReceivePay.Ds = SqlHelper.ExecuteDataset(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spGetDepositSlipByID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepositListByDate(Dep _objDep, bool incZeroAmount)
        {
            try
            {
                SqlParameter paramStart = new SqlParameter();
                paramStart.ParameterName = "startDate";
                paramStart.SqlDbType = SqlDbType.DateTime;
                paramStart.Value = _objDep.StartDate;

                SqlParameter paramEnd = new SqlParameter();
                paramEnd.ParameterName = "endDate";
                paramEnd.SqlDbType = SqlDbType.DateTime;
                paramEnd.Value = _objDep.EndDate;

                SqlParameter paramincZeroAmount = new SqlParameter();
                paramincZeroAmount.ParameterName = "incZeroAmount";
                paramincZeroAmount.SqlDbType = SqlDbType.Bit;
                paramincZeroAmount.Value = incZeroAmount;

                return _objDep.Ds = SqlHelper.ExecuteDataset(_objDep.ConnConfig, CommandType.StoredProcedure, "spGetDepositListByDate", paramStart, paramEnd, paramincZeroAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetDepositListByDate(GetDepositListByDateParam _GetDepositListByDate, string ConnectionString, bool incZeroAmount)
        {
            try
            {
                SqlParameter paramStart = new SqlParameter();
                paramStart.ParameterName = "startDate";
                paramStart.SqlDbType = SqlDbType.DateTime;
                paramStart.Value = _GetDepositListByDate.StartDate;

                SqlParameter paramEnd = new SqlParameter();
                paramEnd.ParameterName = "endDate";
                paramEnd.SqlDbType = SqlDbType.DateTime;
                paramEnd.Value = _GetDepositListByDate.EndDate;

                SqlParameter paramincZeroAmount = new SqlParameter();
                paramincZeroAmount.ParameterName = "incZeroAmount";
                paramincZeroAmount.SqlDbType = SqlDbType.Bit;
                paramincZeroAmount.Value = incZeroAmount;

                return _GetDepositListByDate.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetDepositListByDate", paramStart, paramEnd, paramincZeroAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void writeOffInvoice(WriteOff obj)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = obj.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "AcctWriteOff";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = obj.Acct;

                para[2] = new SqlParameter();
                para[2].ParameterName = "fDesc";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = obj.Desc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "WriteOffDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = obj.fDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "CreateBy";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = obj.CreateBy;

                para[5] = new SqlParameter();
                para[5].ParameterName = "fDescwriteoff";
                para[5].SqlDbType = SqlDbType.VarChar;
                para[5].Value = obj.WriteoffDesc;

                para[6] = new SqlParameter();
                para[6].ParameterName = "CheckNo";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = obj.CheckNo;
                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spWriteOffInvoice", para);       
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void writeOffInvoice(writeOffInvoiceParam _writeOffInvoice, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _writeOffInvoice.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "AcctWriteOff";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _writeOffInvoice.Acct;

                para[2] = new SqlParameter();
                para[2].ParameterName = "fDesc";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = _writeOffInvoice.Desc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "WriteOffDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = _writeOffInvoice.fDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "CreateBy";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _writeOffInvoice.CreateBy;
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spWriteOffInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void writeOffInvoiceMulti(WriteOff obj)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[8];
                para[0] = new SqlParameter();
                para[0].ParameterName = "listInvoice";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = obj.ListInvoice;

                para[1] = new SqlParameter();
                para[1].ParameterName = "AcctWriteOff";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = obj.Acct;

                para[2] = new SqlParameter();
                para[2].ParameterName = "fDesc";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = obj.Desc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "WriteOffDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = obj.fDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "CreateBy";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = obj.CreateBy;

                para[5] = new SqlParameter();
                para[5].ParameterName = "WriteOffAmount";
                para[5].SqlDbType = SqlDbType.Float;
                para[5].Value = obj.WriteOffAmount;

                para[6] = new SqlParameter();
                para[6].ParameterName = "fDescwriteoff";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = obj.WriteoffDesc;

                para[7] = new SqlParameter();
                para[7].ParameterName = "CheckNo";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = obj.CheckNo;

                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spWriteOffInvoiceMulti", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void writeOffInvoiceMulti(writeOffInvoiceMultiParam _writeOffInvoiceMulti, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[6];
                para[0] = new SqlParameter();
                para[0].ParameterName = "listInvoice";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = _writeOffInvoiceMulti.ListInvoice;

                para[1] = new SqlParameter();
                para[1].ParameterName = "AcctWriteOff";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _writeOffInvoiceMulti.Acct;

                para[2] = new SqlParameter();
                para[2].ParameterName = "fDesc";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = _writeOffInvoiceMulti.Desc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "WriteOffDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = _writeOffInvoiceMulti.fDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "CreateBy";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _writeOffInvoiceMulti.CreateBy;

                para[5] = new SqlParameter();
                para[5].ParameterName = "WriteOffAmount";
                para[5].SqlDbType = SqlDbType.Float;
                para[5].Value = _writeOffInvoiceMulti.WriteOffAmount;



                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spWriteOffInvoiceMulti", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddBatchReceivePayment(ReceivedPayment objReceivePay, int bank, Boolean createDeposit)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[6];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePayMulti";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objReceivePay.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "payDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = objReceivePay.PaymentReceivedDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "payMethod";
                para[2].SqlDbType = SqlDbType.SmallInt;
                para[2].Value = objReceivePay.PaymentMethod;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@UpdatedBy";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = objReceivePay.MOMUSer;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@bank";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = bank;               

                para[5] = new SqlParameter();
                para[5].ParameterName = "@createDeposit";
                para[5].SqlDbType = SqlDbType.Bit;
                para[5].Value = createDeposit;

                return  SqlHelper.ExecuteDataset(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spAddBatchReceivePayment", para);
                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBatchReceivePayment(string conn, int batch)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "batchReceipt";
                param.SqlDbType = SqlDbType.Int;
                param.Value = batch;               

                return  SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetBatchReceivePayment", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet UpdateBatchReceivePayment(ReceivedPayment objReceivePay, int bank, Boolean createDeposit, int batchReceipt)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];

                para[0] = new SqlParameter();
                para[0].ParameterName = "receivePayMulti";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objReceivePay.DtPay;

                para[1] = new SqlParameter();
                para[1].ParameterName = "payDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = objReceivePay.PaymentReceivedDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "payMethod";
                para[2].SqlDbType = SqlDbType.SmallInt;
                para[2].Value = objReceivePay.PaymentMethod;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@UpdatedBy";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = objReceivePay.MOMUSer;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@bank";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = bank;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@createDeposit";
                para[5].SqlDbType = SqlDbType.Bit;
                para[5].Value = createDeposit;

                para[6] = new SqlParameter();
                para[6].ParameterName = "@batchReceipt";
                para[6].SqlDbType = SqlDbType.Int;
                para[6].Value = batchReceipt;

                return SqlHelper.ExecuteDataset(objReceivePay.ConnConfig, CommandType.StoredProcedure, "spUpdateBatchReceivePayment", para);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void TransferPayment(string conn, string strRef, int newLoc)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "lsRef";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = strRef;

                para[1] = new SqlParameter();
                para[1].ParameterName = "newLoc";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = newLoc;
               
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "spTransferPayment", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void TransferPayment(TransferPaymentParam _TransferPayment, string ConnectionString, string strRef, int newLoc)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "lsRef";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = strRef;

                para[1] = new SqlParameter();
                para[1].ParameterName = "newLoc";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = newLoc;

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spTransferPayment", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UnapplyPayment(string conn, int Ref, string MOMUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Ref;

                para[1] = new SqlParameter();
                para[1].ParameterName = "UpdatedBy";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = MOMUser;

                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "spUnapplyPayment", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UnapplyPayment(UnapplyPaymentParam _UnapplyPayment, string ConnectionString, int Ref)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Ref;

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUnapplyPayment", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetReceivePaymentReport(ReceivedPayment _objReceiPmt, List<RetainFilter> filters, int intEN)
        {
            DataSet ds = new DataSet();
            try
            {
                _objReceiPmt.ConnConfig = HttpContext.Current.Session["config"].ToString();
                _objReceiPmt.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());

                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT rp.ID,                               \n");
                varname.Append("    ISNULL(l.Owner,0) AS Owner,             \n");
                varname.Append("    ISNULL(ro.Name,'') AS CustomerName,     \n");
                varname.Append("    ISNULL(rp.Loc,0) AS Loc,                \n");
                varname.Append("    ISNULL(l.Tag,'') AS Tag,                \n");
                varname.Append("    ISNULL(rp.Amount,0) AS Amount,          \n");
                varname.Append("    ISNULL(ro.EN,0) AS EN,                  \n");
                varname.Append("    ISNULL(B.Name,'') AS Company,           \n");
                varname.Append("    rp.PaymentReceivedDate,                 \n");
                varname.Append("    rp.fDesc,                               \n");
                varname.Append("    (CASE rp.PaymentMethod                  \n");
                varname.Append("        WHEN 0 THEN 'Check'                 \n");
                varname.Append("        WHEN 1 THEN 'Cash'                  \n");
                varname.Append("        WHEN 2 THEN 'Wire Transfer'         \n");
                varname.Append("        WHEN 3 THEN 'ACH'                   \n");
                varname.Append("        WHEN 4 THEN 'Credit Card'           \n");
                varname.Append("        WHEN 5 THEN 'e-Transfer'            \n");
                varname.Append("        WHEN 6 THEN 'Lockbox'               \n");
                varname.Append("            END) AS PaymentMethod,          \n");
                varname.Append("    (CASE ISNULL(rp.Status,0) WHEN 0 THEN 'Open' WHEN 1 THEN 'Deposited' WHEN 2 THEN 'Applied' END) AS StatusName,  \n");
                varname.Append("    rp.CheckNumber,                         \n");
                varname.Append("    ISNULL(rp.AmountDue,0) AS AmountDue,    \n");
                varname.Append("    ISNULL(rp.Status,0) AS Status ,         \n");
                varname.Append("    ISNULL(d.DepID,0) AS DepID,             \n");
                varname.Append("    ISNULL(rp.Batch,0) AS BatchReceipt      \n");
                varname.Append("FROM ReceivedPayment rp                     \n");
                varname.Append("    LEFT JOIN Owner o ON rp.Owner = o.ID    \n");
                varname.Append("    LEFT JOIN Rol ro ON o.Rol = ro.ID       \n");
                varname.Append("    LEFT JOIN Branch B on ro.EN = B.ID      \n");
                varname.Append("    LEFT JOIN Loc l ON rp.loc = l.loc       \n");
                varname.Append("    LEFT JOIN DepositDetails d on d.ReceivedPaymentID = rp.ID   \n");
                if (intEN == 1)
                {
                    varname.Append("        LEFT JOIN tblUserCo UC ON UC.CompanyID = ro.EN      \n");
                }

                varname.Append("WHERE rp.PaymentReceivedDate >= '" + _objReceiPmt.StartDate + "'\n");
                varname.Append("    AND rp.PaymentReceivedDate <= '" + _objReceiPmt.EndDate + "'\n");

                // Filter
                if (filters.Count > 0)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            varname.Append("    AND rp.ID LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "PaymentReceivedDate")
                        {
                            varname.Append("    AND rp.PaymentReceivedDate LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "CheckNumber")
                        {
                            varname.Append("    AND rp.CheckNumber LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "customerName")
                        {
                            varname.Append("    AND ro.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tag")
                        {
                            varname.Append("    AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "PaymentMethod")
                        {
                            varname.Append("    AND (CASE rp.PaymentMethod              \n");
                            varname.Append("        WHEN 0 THEN 'Check'                 \n");
                            varname.Append("        WHEN 1 THEN 'Cash'                  \n");
                            varname.Append("        WHEN 2 THEN 'Wire Transfer'         \n");
                            varname.Append("        WHEN 3 THEN 'ACH'                   \n");
                            varname.Append("        WHEN 4 THEN 'Credit Card'           \n");
                            varname.Append("        WHEN 5 THEN 'e-Transfer'            \n");
                            varname.Append("        WHEN 6 THEN 'Lockbox'               \n");
                            varname.Append("    END) LIKE '%" + filter.FilterValue + "%'\n");
                        }
                        if (filter.FilterColumn == "StatusName")
                        {
                            varname.Append("    AND  (CASE ISNULL(rp.Status,0) WHEN 0 THEN 'Open' WHEN 1 THEN 'Deposited' WHEN 2 THEN 'Applied' END) LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Amount")
                        {
                            varname.Append("    AND rp.Amount = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "DepID")
                        {
                            varname.Append("    AND d.DepID = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "BatchReceipt")
                        {
                            varname.Append("    AND rp.Batch = " + filter.FilterValue + " \n");
                        }
                    }
                }

                if (intEN == 1)
                {
                    varname.Append("    AND UC.IsSel = 1 AND UC.UserID = " + _objReceiPmt.UserID);
                }

                varname.Append("    ORDER BY rp.ID                          \n");

                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllReceivePayment(ReceivedPayment _objReceiPmt, List<RetainFilter> filters, int intEN)
        {
            DataSet ds = new DataSet();
            try
            {
                _objReceiPmt.ConnConfig = HttpContext.Current.Session["config"].ToString();
                _objReceiPmt.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());

                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT convert(varchar(200),rp.ID) as ID,                               \n");
                varname.Append("    ISNULL(l.Owner,0) AS Owner,             \n");
                varname.Append("    ISNULL(ro.Name,'') AS CustomerName,     \n");
                varname.Append("    ISNULL(rp.Loc,0) AS Loc,                \n");
                varname.Append("    ISNULL(l.Tag,'') AS Tag,                \n");
                varname.Append("    ISNULL(rp.Amount,0) AS Amount,          \n");
                varname.Append("    ISNULL(ro.EN,0) AS EN,                  \n");
                varname.Append("    ISNULL(B.Name,'') AS Company,           \n");
                varname.Append("    rp.PaymentReceivedDate,                 \n");
                varname.Append("    rp.fDesc,                               \n");
                varname.Append("    (CASE rp.PaymentMethod                  \n");
                varname.Append("        WHEN 0 THEN 'Check'                 \n");
                varname.Append("        WHEN 1 THEN 'Cash'                  \n");
                varname.Append("        WHEN 2 THEN 'Wire Transfer'         \n");
                varname.Append("        WHEN 3 THEN 'ACH'                   \n");
                varname.Append("        WHEN 4 THEN 'Credit Card'           \n");
                varname.Append("        WHEN 5 THEN 'e-Transfer'            \n");
                varname.Append("        WHEN 6 THEN 'Lockbox'               \n");
                varname.Append("            END) AS PaymentMethod,          \n");
                varname.Append("    (CASE ISNULL(rp.Status,0) WHEN 0 THEN 'Open' WHEN 1 THEN 'Deposited' WHEN 2 THEN 'Applied' END) AS StatusName,  \n");
                varname.Append("    rp.CheckNumber,                         \n");
                varname.Append("    ISNULL(rp.AmountDue,0) AS AmountDue,    \n");
                varname.Append("    ISNULL(rp.Status,0) AS Status ,         \n");
                varname.Append("    ISNULL(d.DepID,0) AS DepID,             \n");
                varname.Append("    ISNULL(rp.Batch,0) AS BatchReceipt      \n");
                varname.Append("FROM ReceivedPayment rp                     \n");
                varname.Append("    LEFT JOIN Owner o ON rp.Owner = o.ID    \n");
                varname.Append("    LEFT JOIN Rol ro ON o.Rol = ro.ID       \n");
                varname.Append("    LEFT JOIN Branch B on ro.EN = B.ID      \n");
                varname.Append("    LEFT JOIN Loc l ON rp.loc = l.loc       \n");
                varname.Append("    LEFT JOIN DepositDetails d on d.ReceivedPaymentID = rp.ID   \n");
                if (intEN == 1)
                {
                    varname.Append("        LEFT JOIN tblUserCo UC ON UC.CompanyID = ro.EN      \n");
                }

                varname.Append("WHERE rp.PaymentReceivedDate >= '" + _objReceiPmt.StartDate + "'\n");
                varname.Append("    AND rp.PaymentReceivedDate <= '" + _objReceiPmt.EndDate + "'\n");

                // Filter
                if (filters.Count > 0)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            varname.Append("    AND rp.ID = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "PaymentReceivedDate")
                        {
                            varname.Append("    AND rp.PaymentReceivedDate = '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "CheckNumber")
                        {
                            varname.Append("    AND rp.CheckNumber LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "customerName")
                        {
                            varname.Append("    AND ro.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tag")
                        {
                            varname.Append("    AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "PaymentMethod")
                        {
                            varname.Append("    AND (CASE rp.PaymentMethod              \n");
                            varname.Append("        WHEN 0 THEN 'Check'                 \n");
                            varname.Append("        WHEN 1 THEN 'Cash'                  \n");
                            varname.Append("        WHEN 2 THEN 'Wire Transfer'         \n");
                            varname.Append("        WHEN 3 THEN 'ACH'                   \n");
                            varname.Append("        WHEN 4 THEN 'Credit Card'           \n");
                            varname.Append("        WHEN 5 THEN 'e-Transfer'            \n");
                            varname.Append("        WHEN 6 THEN 'Lockbox'               \n");
                            varname.Append("    END) LIKE '%" + filter.FilterValue + "%'\n");
                        }
                        if (filter.FilterColumn == "StatusName")
                        {
                            varname.Append("    AND  (CASE ISNULL(rp.Status,0) WHEN 0 THEN 'Open' WHEN 1 THEN 'Deposited' WHEN 2 THEN 'Applied' END) LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Amount")
                        {
                            varname.Append("    AND rp.Amount = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "DepID")
                        {
                            varname.Append("    AND d.DepID = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "BatchReceipt")
                        {
                            varname.Append("    AND rp.Batch = " + filter.FilterValue + " \n");
                        }
                    }
                }

                if (intEN == 1)
                {
                    varname.Append("    AND UC.IsSel = 1 AND UC.UserID = " + _objReceiPmt.UserID);
                }

                varname.Append("    ORDER BY rp.ID                          \n");

                return SqlHelper.ExecuteDataset(_objReceiPmt.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        //public DataSet GetAllReceivePayment(GetAllReceivePaymentParam _GetAllReceivePayment, string ConnectionString, List<RetainFilter> filters, int intEN)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        //_GetAllReceivePayment.ConnConfig = HttpContext.Current.Session["config"].ToString();
        //        //_GetAllReceivePayment.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());

        //        _GetAllReceivePayment.ConnConfig = ConnectionString;
        //        _GetAllReceivePayment.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());

        //        StringBuilder varname = new StringBuilder();
        //        varname.Append("SELECT convert(varchar(200),rp.ID) as ID,                               \n");
        //        varname.Append("    ISNULL(l.Owner,0) AS Owner,             \n");
        //        varname.Append("    ISNULL(ro.Name,'') AS CustomerName,     \n");
        //        varname.Append("    ISNULL(rp.Loc,0) AS Loc,                \n");
        //        varname.Append("    ISNULL(l.Tag,'') AS Tag,                \n");
        //        varname.Append("    ISNULL(rp.Amount,0) AS Amount,          \n");
        //        varname.Append("    ISNULL(ro.EN,0) AS EN,                  \n");
        //        varname.Append("    ISNULL(B.Name,'') AS Company,           \n");
        //        varname.Append("    rp.PaymentReceivedDate,                 \n");
        //        varname.Append("    rp.fDesc,                               \n");
        //        varname.Append("    (CASE rp.PaymentMethod                  \n");
        //        varname.Append("        WHEN 0 THEN 'Check'                 \n");
        //        varname.Append("        WHEN 1 THEN 'Cash'                  \n");
        //        varname.Append("        WHEN 2 THEN 'Wire Transfer'         \n");
        //        varname.Append("        WHEN 3 THEN 'ACH'                   \n");
        //        varname.Append("        WHEN 4 THEN 'Credit Card'           \n");
        //        varname.Append("        WHEN 5 THEN 'e-Transfer'            \n");
        //        varname.Append("        WHEN 6 THEN 'Lockbox'               \n");
        //        varname.Append("            END) AS PaymentMethod,          \n");
        //        varname.Append("    (CASE ISNULL(rp.Status,0) WHEN 0 THEN 'Open' WHEN 1 THEN 'Deposited' WHEN 2 THEN 'Applied' END) AS StatusName,  \n");
        //        varname.Append("    rp.CheckNumber,                         \n");
        //        varname.Append("    ISNULL(rp.AmountDue,0) AS AmountDue,    \n");
        //        varname.Append("    ISNULL(rp.Status,0) AS Status ,         \n");
        //        varname.Append("    ISNULL(d.DepID,0) AS DepID,             \n");
        //        varname.Append("    ISNULL(rp.Batch,0) AS BatchReceipt      \n");
        //        varname.Append("FROM ReceivedPayment rp                     \n");
        //        varname.Append("    LEFT JOIN Owner o ON rp.Owner = o.ID    \n");
        //        varname.Append("    LEFT JOIN Rol ro ON o.Rol = ro.ID       \n");
        //        varname.Append("    LEFT JOIN Branch B on ro.EN = B.ID      \n");
        //        varname.Append("    LEFT JOIN Loc l ON rp.loc = l.loc       \n");
        //        varname.Append("    LEFT JOIN DepositDetails d on d.ReceivedPaymentID = rp.ID   \n");
        //        if (intEN == 1)
        //        {
        //            varname.Append("        LEFT JOIN tblUserCo UC ON UC.CompanyID = ro.EN      \n");
        //        }

        //        varname.Append("WHERE rp.PaymentReceivedDate >= '" + _GetAllReceivePayment.StartDate + "'\n");
        //        varname.Append("    AND rp.PaymentReceivedDate <= '" + _GetAllReceivePayment.EndDate + "'\n");

        //        // Filter
        //        if (filters.Count > 0)
        //        {
        //            foreach (var filter in filters)
        //            {
        //                if (filter.FilterColumn == "ID")
        //                {
        //                    varname.Append("    AND rp.ID = " + filter.FilterValue + " \n");
        //                }
        //                if (filter.FilterColumn == "PaymentReceivedDate")
        //                {
        //                    varname.Append("    AND rp.PaymentReceivedDate LIKE '%" + filter.FilterValue + "%' \n");
        //                }
        //                if (filter.FilterColumn == "CheckNumber")
        //                {
        //                    varname.Append("    AND rp.CheckNumber LIKE '%" + filter.FilterValue + "%' \n");
        //                }
        //                if (filter.FilterColumn == "customerName")
        //                {
        //                    varname.Append("    AND ro.Name LIKE '%" + filter.FilterValue + "%' \n");
        //                }
        //                if (filter.FilterColumn == "Tag")
        //                {
        //                    varname.Append("    AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
        //                }
        //                if (filter.FilterColumn == "PaymentMethod")
        //                {
        //                    varname.Append("    AND (CASE rp.PaymentMethod              \n");
        //                    varname.Append("        WHEN 0 THEN 'Check'                 \n");
        //                    varname.Append("        WHEN 1 THEN 'Cash'                  \n");
        //                    varname.Append("        WHEN 2 THEN 'Wire Transfer'         \n");
        //                    varname.Append("        WHEN 3 THEN 'ACH'                   \n");
        //                    varname.Append("        WHEN 4 THEN 'Credit Card'           \n");
        //                    varname.Append("        WHEN 5 THEN 'e-Transfer'            \n");
        //                    varname.Append("        WHEN 6 THEN 'Lockbox'               \n");
        //                    varname.Append("    END) LIKE '%" + filter.FilterValue + "%'\n");
        //                }
        //                if (filter.FilterColumn == "StatusName")
        //                {
        //                    varname.Append("    AND  (CASE ISNULL(rp.Status,0) WHEN 0 THEN 'Open' WHEN 1 THEN 'Deposited' WHEN 2 THEN 'Applied' END) LIKE '%" + filter.FilterValue + "%' \n");
        //                }
        //                if (filter.FilterColumn == "Amount")
        //                {
        //                    varname.Append("    AND rp.Amount = " + filter.FilterValue + " \n");
        //                }
        //                if (filter.FilterColumn == "DepID")
        //                {
        //                    varname.Append("    AND d.DepID = " + filter.FilterValue + " \n");
        //                }
        //                if (filter.FilterColumn == "BatchReceipt")
        //                {
        //                    varname.Append("    AND rp.Batch = " + filter.FilterValue + " \n");
        //                }
        //            }
        //        }

        //        if (intEN == 1)
        //        {
        //            varname.Append("    AND UC.IsSel = 1 AND UC.UserID = " + _GetAllReceivePayment.UserID);
        //        }

        //        varname.Append("    ORDER BY rp.ID                          \n");

        //        return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void writeOffCredit(WriteOff obj)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = obj.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "AcctWriteOff";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = obj.Acct;

                para[2] = new SqlParameter();
                para[2].ParameterName = "fDesc";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = obj.Desc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "WriteOffDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = obj.fDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "CreateBy";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = obj.CreateBy;

                para[5] = new SqlParameter();
                para[5].ParameterName = "fDescwriteoff";
                para[5].SqlDbType = SqlDbType.VarChar;
                para[5].Value = obj.WriteoffDesc;

                para[6] = new SqlParameter();
                para[6].ParameterName = "CheckNo";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = obj.CheckNo;
                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spWriteOffCredit", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void writeOffDeposit(WriteOff obj)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[8];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = obj.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "AcctWriteOff";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = obj.Acct;

                para[2] = new SqlParameter();
                para[2].ParameterName = "fDesc";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = obj.Desc;

                para[3] = new SqlParameter();
                para[3].ParameterName = "WriteOffDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = obj.fDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "CreateBy";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = obj.CreateBy;

                para[5] = new SqlParameter();
                para[5].ParameterName = "TransID";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = obj.TransID;

                para[6] = new SqlParameter();
                para[6].ParameterName = "fDescwriteoff";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = obj.WriteoffDesc;

                para[7] = new SqlParameter();
                para[7].ParameterName = "CheckNo";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = obj.CheckNo;
                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spWriteOffDeposit", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
