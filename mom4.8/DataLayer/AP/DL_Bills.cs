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
    public class DL_Bills
    {
        public void AddOpenAP(OpenAP _objOpenAP)
        {
            try
            {
                string query = "INSERT INTO OpenAP(Vendor,fDate,Due,Type,fDesc,Original,Balance,Selected,Disc,PJID,TRID,Ref)"
                + "VALUES(@Vendor,@fDate,@Due,@Type,@fDesc,@Original,@Balance,@Selected,@Disc,@PJID,@TRID,@Ref)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Vendor", _objOpenAP.Vendor));
                parameters.Add(new SqlParameter("@fDate", _objOpenAP.fDate));
                parameters.Add(new SqlParameter("@Due", _objOpenAP.Due));
                parameters.Add(new SqlParameter("@Type", _objOpenAP.Type));
                parameters.Add(new SqlParameter("@fDesc", _objOpenAP.fDesc));
                parameters.Add(new SqlParameter("@Original", _objOpenAP.Original));
                parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@Selected", _objOpenAP.Selected));
                parameters.Add(new SqlParameter("@Disc", _objOpenAP.Disc));
                parameters.Add(new SqlParameter("@PJID", _objOpenAP.PJID));
                parameters.Add(new SqlParameter("@TRID", _objOpenAP.TRID));
                parameters.Add(new SqlParameter("@Ref", _objOpenAP.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddOpenAP(AddOpenAPParam _objAddOpenAPParam, string ConnectionString)
        {
            try
            {
                string query = "INSERT INTO OpenAP(Vendor,fDate,Due,Type,fDesc,Original,Balance,Selected,Disc,PJID,TRID,Ref)"
                + "VALUES(@Vendor,@fDate,@Due,@Type,@fDesc,@Original,@Balance,@Selected,@Disc,@PJID,@TRID,@Ref)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Vendor", _objAddOpenAPParam.Vendor));
                parameters.Add(new SqlParameter("@fDate", _objAddOpenAPParam.fDate));
                parameters.Add(new SqlParameter("@Due", _objAddOpenAPParam.Due));
                parameters.Add(new SqlParameter("@Type", _objAddOpenAPParam.Type));
                parameters.Add(new SqlParameter("@fDesc", _objAddOpenAPParam.fDesc));
                parameters.Add(new SqlParameter("@Original", _objAddOpenAPParam.Original));
                parameters.Add(new SqlParameter("@Balance", _objAddOpenAPParam.Balance));
                parameters.Add(new SqlParameter("@Selected", _objAddOpenAPParam.Selected));
                parameters.Add(new SqlParameter("@Disc", _objAddOpenAPParam.Disc));
                parameters.Add(new SqlParameter("@PJID", _objAddOpenAPParam.PJID));
                parameters.Add(new SqlParameter("@TRID", _objAddOpenAPParam.TRID));
                parameters.Add(new SqlParameter("@Ref", _objAddOpenAPParam.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddPJ(PJ _objPJ)
        {
            try
            {
                string query = "DECLARE @ID INT; SELECT @ID=ISNULL(MAX(ID),0)+1 FROM PJ; "
                + "INSERT INTO PJ(ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR, ReceivePO)"
                + "VALUES (@ID, @fDate,@Ref, @fDesc,@Amount, @Vendor,@Status, @Batch,@Terms, @PO,@TRID,@Spec,@IDate,@UseTax,@Disc,@Custom1,@Custom2,@ReqBy,@VoidR,@ReceivePO) SELECT @ID AS PJID;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                parameters.Add(new SqlParameter("@fDate", _objPJ.fDate));
                parameters.Add(new SqlParameter("@Ref", _objPJ.Ref));
                parameters.Add(new SqlParameter("@fDesc", _objPJ.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objPJ.Amount));
                parameters.Add(new SqlParameter("@Vendor", _objPJ.Vendor));
                parameters.Add(new SqlParameter("@Status", _objPJ.Status));
                parameters.Add(new SqlParameter("@Batch", _objPJ.Batch));
                parameters.Add(new SqlParameter("@Terms", _objPJ.Terms));
                parameters.Add(new SqlParameter("@PO", _objPJ.PO));
                parameters.Add(new SqlParameter("@TRID", _objPJ.TRID));
                parameters.Add(new SqlParameter("@Spec", _objPJ.Spec));
                parameters.Add(new SqlParameter("@IDate", _objPJ.IDate));
                parameters.Add(new SqlParameter("@UseTax", _objPJ.UseTax));
                parameters.Add(new SqlParameter("@Disc", _objPJ.Disc));
                parameters.Add(new SqlParameter("@Custom1", _objPJ.Custom1));
                parameters.Add(new SqlParameter("@Custom2", _objPJ.Custom2));
                parameters.Add(new SqlParameter("@ReqBy", _objPJ.ReqBy));
                parameters.Add(new SqlParameter("@VoidR", _objPJ.VoidR));
                parameters.Add(new SqlParameter("@ReceivePO", _objPJ.ReceivePo));
                DataSet _dsID = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());

                return Convert.ToInt32(_dsID.Tables[0].Rows[0]["PJID"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddPJ(AddPJParam _objAddPJParam, string ConnectionString)
        {
            try
            {
                string query = "DECLARE @ID INT; SELECT @ID=ISNULL(MAX(ID),0)+1 FROM PJ; "
                + "INSERT INTO PJ(ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR, ReceivePO)"
                + "VALUES (@ID, @fDate,@Ref, @fDesc,@Amount, @Vendor,@Status, @Batch,@Terms, @PO,@TRID,@Spec,@IDate,@UseTax,@Disc,@Custom1,@Custom2,@ReqBy,@VoidR,@ReceivePO) SELECT @ID AS PJID;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                parameters.Add(new SqlParameter("@fDate", _objAddPJParam.fDate));
                parameters.Add(new SqlParameter("@Ref", _objAddPJParam.Ref));
                parameters.Add(new SqlParameter("@fDesc", _objAddPJParam.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objAddPJParam.Amount));
                parameters.Add(new SqlParameter("@Vendor", _objAddPJParam.Vendor));
                parameters.Add(new SqlParameter("@Status", _objAddPJParam.Status));
                parameters.Add(new SqlParameter("@Batch", _objAddPJParam.Batch));
                parameters.Add(new SqlParameter("@Terms", _objAddPJParam.Terms));
                parameters.Add(new SqlParameter("@PO", _objAddPJParam.PO));
                parameters.Add(new SqlParameter("@TRID", _objAddPJParam.TRID));
                parameters.Add(new SqlParameter("@Spec", _objAddPJParam.Spec));
                parameters.Add(new SqlParameter("@IDate", _objAddPJParam.IDate));
                parameters.Add(new SqlParameter("@UseTax", _objAddPJParam.UseTax));
                parameters.Add(new SqlParameter("@Disc", _objAddPJParam.Disc));
                parameters.Add(new SqlParameter("@Custom1", _objAddPJParam.Custom1));
                parameters.Add(new SqlParameter("@Custom2", _objAddPJParam.Custom2));
                parameters.Add(new SqlParameter("@ReqBy", _objAddPJParam.ReqBy));
                parameters.Add(new SqlParameter("@VoidR", _objAddPJParam.VoidR));
                parameters.Add(new SqlParameter("@ReceivePO", _objAddPJParam.ReceivePo));
                DataSet _dsID = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, query, parameters.ToArray());

                return Convert.ToInt32(_dsID.Tables[0].Rows[0]["PJID"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddJobI(JobI _objJobI)
        {
            try
            {
                string query = "INSERT INTO JobI(Job,Phase,fDate,Ref,fDesc,Amount,TransID,Type,UseTax,APTicket)"
                + " VALUES(@Job, @Phase,@fDate, @Ref,@fDesc,@Amount, @TransID,@Type, @UseTax,@APTicket)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Job", _objJobI.Job));
                parameters.Add(new SqlParameter("@Phase", _objJobI.Phase));
                parameters.Add(new SqlParameter("@fDate", _objJobI.fDate));
                parameters.Add(new SqlParameter("@Ref", _objJobI.Ref));
                parameters.Add(new SqlParameter("@fDesc", _objJobI.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objJobI.Amount));
                parameters.Add(new SqlParameter("@TransID", _objJobI.TransID));
                parameters.Add(new SqlParameter("@Type", _objJobI.Type));
                //parameters.Add(new SqlParameter("@Labor", _objJobI.Labor));
                //parameters.Add(new SqlParameter("@Billed", _objJobI.Billed));
                //parameters.Add(new SqlParameter("@Invoice", _objJobI.Invoice));
                parameters.Add(new SqlParameter("@UseTax", _objJobI.UseTax));
                parameters.Add(new SqlParameter("@APTicket", _objJobI.vAPTicket));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objJobI.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddJobI(AddJobIParam _objAddJobIParam, string ConnectionString)
        {
            try
            {
                string query = "INSERT INTO JobI(Job,Phase,fDate,Ref,fDesc,Amount,TransID,Type,UseTax,APTicket)"
                + " VALUES(@Job, @Phase,@fDate, @Ref,@fDesc,@Amount, @TransID,@Type, @UseTax,@APTicket)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Job", _objAddJobIParam.Job));
                parameters.Add(new SqlParameter("@Phase", _objAddJobIParam.Phase));
                parameters.Add(new SqlParameter("@fDate", _objAddJobIParam.fDate));
                parameters.Add(new SqlParameter("@Ref", _objAddJobIParam.Ref));
                parameters.Add(new SqlParameter("@fDesc", _objAddJobIParam.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objAddJobIParam.Amount));
                parameters.Add(new SqlParameter("@TransID", _objAddJobIParam.TransID));
                parameters.Add(new SqlParameter("@Type", _objAddJobIParam.Type));
                //parameters.Add(new SqlParameter("@Labor", _objJobI.Labor));
                //parameters.Add(new SqlParameter("@Billed", _objJobI.Billed));
                //parameters.Add(new SqlParameter("@Invoice", _objJobI.Invoice));
                parameters.Add(new SqlParameter("@UseTax", _objAddJobIParam.UseTax));
                parameters.Add(new SqlParameter("@APTicket", _objAddJobIParam.vAPTicket));

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPJDetails(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT distinct p.ID,p.fDate, p.fDate as PostingDate,p.IDate, p.IDate as Date,p.Ref,p.fDesc,isnull(p.Amount,0) as Amount,p.Vendor,r.EN, LTRIM(RTRIM(B.Name)) As Company, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open' ");
                varname.Append("                            WHEN 1 THEN 'Closed' ");
                varname.Append("                            WHEN 2 THEN 'Void' WHEN 3 THEN 'Partial' END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,isnull(p.PO,0) as PO,isnull(p.ReceivePO,0) as RPO,p.TRID,p.Spec,isnull(p.UseTax,0) as UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName,IsNull((SELECT (ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) AS Balance, o.Due");
                varname.Append("    ,(SELECT MAX(cd.fDate) FROM CD cd LEFT JOIN Paid pd ON pd.PITR = cd.ID WHERE pd.TRID = p.TRID ) AS PayDate");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                if (_objPJ.EN == 1)
                    varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append("     left outer join Branch B on B.ID = r.EN");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                if (_objPJ.ProjectNumber > 0)
                {
                    varname.Append("     inner join trans  t on p.Batch= t.Batch ");
                }
                if (_objPJ.Terms != 99)
                {
                    varname.Append("     inner join trans  t on p.Batch= t.Batch ");
                    varname.Append("     LEFT JOIN Job as j ON t.VInt = j.ID ");
                    varname.Append("     LEFT JOIN JobTItem as jT ON jT.Job = j.ID ");
                }
                if (_objPJ.Custom == "0")
                {
                    varname.Append("    WHERE (p.fDate >= '" + _objPJ.StartDate + "') AND (p.fDate <= '" + _objPJ.EndDate + "') ");
                }
                if (_objPJ.Custom == "1")
                {
                    varname.Append("  LEFT JOIN Paid pd ON pd.TRID = p.TRID LEFT JOIN CD ON CD.ID= pd.PITR  WHERE (CD.fDate >= '" + _objPJ.StartDate + "') AND (CD.fDate <= '" + _objPJ.EndDate + "') AND CD.fDate IS NOT NULL ");
                }
                if (_objPJ.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = " + _objPJ.Vendor);
                }
                if (_objPJ.SearchValue.Equals(1))
                {
                    varname.Append("       AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("       AND o.Due<='" + DateTime.Now.ToShortDateString() + "'");
                }
                else if (_objPJ.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("      AND o.Due <= '" + _objPJ.SearchDate + "'");
                }
                if (!string.IsNullOrEmpty(_objPJ.Ref))
                {
                    varname.Append("      AND  p.Ref like '%" + _objPJ.Ref + "%'");

                }
                if (_objPJ.PO > 0)
                {
                    varname.Append("      AND  p.PO like '%" + _objPJ.PO + "%'");
                }
                if (_objPJ.ProjectNumber > 0)
                {
                    varname.Append("      AND  t.vint = " + _objPJ.ProjectNumber);
                }
                if (_objPJ.Amount > 0)
                {
                    varname.Append("      AND  p.Amount like '%" + _objPJ.Amount + "%'");
                }
                if (!string.IsNullOrEmpty(_objPJ.vendorName))
                {
                    //  varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                    varname.Append("      AND   r.Name like '%" + _objPJ.vendorName + "%'");
                }
                //if (_objPJ.Due != null)
                //{
                //    varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                //}
                if (_objPJ.SearchwithmultipleStatus != "99")
                {

                    varname.Append("      AND   p.Status in " + _objPJ.SearchwithmultipleStatus + "");
                }
                if (_objPJ.Terms != 99)
                {
                    varname.Append("      AND   jT.Type = " + _objPJ.Terms + "");
                }
                if (_objPJ.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _objPJ.EN + " AND UC.UserID = " + _objPJ.UserID + " ");
                }
                if (!string.IsNullOrEmpty(_objPJ.Custom1))
                {
                    varname.Append("      AND p.Custom1 like '%" + _objPJ.Custom1 + "%' ");
                }
                if (!string.IsNullOrEmpty(_objPJ.Custom2))
                {
                    varname.Append("      AND p.Custom2 like '%" + _objPJ.Custom2 + "%' ");
                }
                if (!string.IsNullOrEmpty(_objPJ.VendorType))
                {
                    varname.Append("      AND   v.Type = '" + _objPJ.VendorType + "'");
                }
                varname.Append("    ORDER BY p.ID");
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName FROM PJ AS p, Vendor AS v, Rol AS r WHERE v.Rol=r.ID ORDER BY p.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllPJDetails(GetAllPJDetailsParam _GetAllPJDetailsParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT distinct p.ID,p.fDate, p.fDate as PostingDate,p.IDate, p.IDate as Date,p.Ref,p.fDesc,isnull(p.Amount,0) as Amount,p.Vendor,r.EN, LTRIM(RTRIM(B.Name)) As Company, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open' ");
                varname.Append("                            WHEN 1 THEN 'Closed' ");
                varname.Append("                            WHEN 2 THEN 'Void' WHEN 3 THEN 'Partial' END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,isnull(p.PO,0) as PO,p.TRID,p.Spec,isnull(p.UseTax,0) as UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName,IsNull((SELECT (ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) AS Balance, o.Due");
                //varname.Append("    p.Batch,p.Terms,isnull(p.PO,0) as PO,p.TRID,p.Spec,isnull(p.UseTax,0) as UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName,CASE p.Status WHEN 0 THEN IsNull((SELECT (ISNull(o.Original,0) -  (ISNULL(o.Selected,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) WHEN 1 THEN IsNull((SELECT (ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) WHEN 2 THEN IsNull((SELECT (ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) WHEN 3 THEN IsNull((SELECT (ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) END AS Balance, o.Due");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                if (_GetAllPJDetailsParam.EN == 1)
                    varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append("     left outer join Branch B on B.ID = r.EN");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                if (_GetAllPJDetailsParam.ProjectNumber > 0)
                {
                    varname.Append("     inner join trans  t on p.Batch= t.Batch ");
                }
                if (_GetAllPJDetailsParam.Terms != 99)
                {
                    varname.Append("     inner join trans  t on p.Batch= t.Batch ");
                    varname.Append("     LEFT JOIN Job as j ON t.VInt = j.ID ");
                    varname.Append("     LEFT JOIN JobTItem as jT ON jT.Job = j.ID ");
                }
                varname.Append("    WHERE (p.fDate >= '" + _GetAllPJDetailsParam.StartDate + "') AND (p.fDate <= '" + _GetAllPJDetailsParam.EndDate + "') ");
                if (_GetAllPJDetailsParam.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = " + _GetAllPJDetailsParam.Vendor);
                }
                if (_GetAllPJDetailsParam.SearchValue.Equals(1))
                {
                    varname.Append("       AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("       AND o.Due<='" + DateTime.Now.ToShortDateString() + "'");
                }
                else if (_GetAllPJDetailsParam.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("      AND o.Due <= '" + _GetAllPJDetailsParam.SearchDate + "'");
                }
                if (!string.IsNullOrEmpty(_GetAllPJDetailsParam.Ref))
                {
                    varname.Append("      AND  p.Ref like '%" + _GetAllPJDetailsParam.Ref + "%'");

                }
                if (_GetAllPJDetailsParam.PO > 0)
                {
                    varname.Append("      AND  p.PO like '%" + _GetAllPJDetailsParam.PO + "%'");
                }
                if (_GetAllPJDetailsParam.ProjectNumber > 0)
                {
                    varname.Append("      AND  t.vint = " + _GetAllPJDetailsParam.ProjectNumber);
                }
                if (_GetAllPJDetailsParam.Amount > 0)
                {
                    varname.Append("      AND  p.Amount like '%" + _GetAllPJDetailsParam.Amount + "%'");
                }
                if (!string.IsNullOrEmpty(_GetAllPJDetailsParam.vendorName))
                {
                    //  varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                    varname.Append("      AND   r.Name like '%" + _GetAllPJDetailsParam.vendorName + "%'");
                }
                //if (_objPJ.Due != null)
                //{
                //    varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                //}
                if (_GetAllPJDetailsParam.SearchwithmultipleStatus != "99")
                {
                    varname.Append("      AND   p.Status in " + _GetAllPJDetailsParam.SearchwithmultipleStatus + "");
                }
                if (_GetAllPJDetailsParam.Terms != 99)
                {
                    varname.Append("      AND   jT.Type = " + _GetAllPJDetailsParam.Terms + "");
                }
                if (_GetAllPJDetailsParam.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _GetAllPJDetailsParam.EN + " AND UC.UserID = " + _GetAllPJDetailsParam.UserID + " ");
                }
                if (!string.IsNullOrEmpty(_GetAllPJDetailsParam.Custom1))
                {
                    varname.Append("      AND p.Custom1 like '%" + _GetAllPJDetailsParam.Custom1 + "%' ");
                }
                if (!string.IsNullOrEmpty(_GetAllPJDetailsParam.Custom2))
                {
                    varname.Append("      AND p.Custom2 like '%" + _GetAllPJDetailsParam.Custom2 + "%' ");
                }
                varname.Append("    ORDER BY p.ID");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName FROM PJ AS p, Vendor AS v, Rol AS r WHERE v.Rol=r.ID ORDER BY p.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPJRecurrDetails(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT distinct p.ID,p.fDate, p.fDate as PostingDate,p.IDate, p.IDate as Date,p.Ref,p.fDesc,isnull(p.Amount,0) as Amount,isnull(p.Amount,0) as Balance,p.Vendor,r.EN, LTRIM(RTRIM(B.Name)) As Company, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open' ");
                varname.Append("                            WHEN 1 THEN 'Closed' ");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.frequency as Batch,p.Terms,isnull(p.PO,0) as PO,isnull(p.ReceivePO,0) as RPO,p.TRID,p.Spec,isnull(p.UseTax,0) as UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName, o.Due, NULL AS PayDate");
                varname.Append("    FROM PJRecurr AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                if (_objPJ.EN == 1)
                    varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append("     left outer join Branch B on B.ID = r.EN");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                if (_objPJ.ProjectNumber > 0)
                {
                    varname.Append("     inner join PJRecurrI  t on p.ID= t.PJID ");
                }
                if (_objPJ.Terms != 99)
                {
                    varname.Append("     inner join trans  t on p.Batch= t.Batch ");
                    varname.Append("     LEFT JOIN Job as j ON t.VInt = j.ID ");
                    varname.Append("     LEFT JOIN JobTItem as jT ON jT.Job = j.ID ");
                }
                varname.Append("    WHERE (p.fDate >= '" + _objPJ.StartDate + "') AND (p.fDate <= '" + _objPJ.EndDate + "') ");
                varname.Append("    AND p.ID NOT IN (SELECT PJID FROM CDRecurr) ");
                if (_objPJ.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = " + _objPJ.Vendor);
                }
                if (_objPJ.SearchValue.Equals(1))
                {
                    varname.Append("       AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("       AND o.Due<='" + DateTime.Now.ToShortDateString() + "'");
                }
                else if (_objPJ.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("      AND o.Due <= '" + _objPJ.SearchDate + "'");
                }
                if (!string.IsNullOrEmpty(_objPJ.Ref))
                {
                    varname.Append("      AND  p.Ref like '%" + _objPJ.Ref + "%'");

                }
                if (_objPJ.PO > 0)
                {
                    varname.Append("      AND  p.PO like '%" + _objPJ.PO + "%'");
                }
                if (_objPJ.ProjectNumber > 0)
                {
                    varname.Append("      AND  t.vint = " + _objPJ.ProjectNumber);
                }
                if (_objPJ.Amount > 0)
                {
                    varname.Append("      AND  p.Amount like '%" + _objPJ.Amount + "%'");
                }
                if (!string.IsNullOrEmpty(_objPJ.vendorName))
                {
                    //  varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                    varname.Append("      AND   r.Name like '%" + _objPJ.vendorName + "%'");
                }
                //if (_objPJ.Due != null)
                //{
                //    varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                //}
                if (_objPJ.Status != 99)
                {
                    varname.Append("      AND   p.Status = " + _objPJ.Status + "");
                }
                if (_objPJ.Terms != 99)
                {
                    varname.Append("      AND   jT.Type = " + _objPJ.Terms + "");
                }
                if (_objPJ.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _objPJ.EN + " AND UC.UserID = " + _objPJ.UserID + " ");
                }
                if (!string.IsNullOrEmpty(_objPJ.Custom1))
                {
                    varname.Append("      AND p.Custom1 like '%" + _objPJ.Custom1 + "%' ");
                }
                if (!string.IsNullOrEmpty(_objPJ.Custom2))
                {
                    varname.Append("      AND p.Custom2 like '%" + _objPJ.Custom2 + "%' ");
                }
                if (_objPJ.VendorType != "")
                {
                    varname.Append("      AND   v.Type = '" + _objPJ.VendorType + "'");
                }
                varname.Append("    ORDER BY p.ID");
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName FROM PJ AS p, Vendor AS v, Rol AS r WHERE v.Rol=r.ID ORDER BY p.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllPJRecurrDetails(GetAllPJRecurrDetailsParam _GetAllPJRecurrDetailsParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT distinct p.ID,p.fDate, p.fDate as PostingDate,p.IDate, p.IDate as Date,p.Ref,p.fDesc,isnull(p.Amount,0) as Amount,p.Vendor,r.EN, LTRIM(RTRIM(B.Name)) As Company, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open' ");
                varname.Append("                            WHEN 1 THEN 'Closed' ");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.frequency as Batch,p.Terms,isnull(p.PO,0) as PO,p.TRID,p.Spec,isnull(p.UseTax,0) as UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName, o.Due");
                varname.Append("    FROM PJRecurr AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                if (_GetAllPJRecurrDetailsParam.EN == 1)
                    varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append("     left outer join Branch B on B.ID = r.EN");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                if (_GetAllPJRecurrDetailsParam.ProjectNumber > 0)
                {
                    varname.Append("     inner join PJRecurrI  t on p.ID= t.PJID ");
                }
                if (_GetAllPJRecurrDetailsParam.Terms != 99)
                {
                    varname.Append("     inner join trans  t on p.Batch= t.Batch ");
                    varname.Append("     LEFT JOIN Job as j ON t.VInt = j.ID ");
                    varname.Append("     LEFT JOIN JobTItem as jT ON jT.Job = j.ID ");
                }
                varname.Append("    WHERE (p.fDate >= '" + _GetAllPJRecurrDetailsParam.StartDate + "') AND (p.fDate <= '" + _GetAllPJRecurrDetailsParam.EndDate + "') ");
                varname.Append("    AND p.ID NOT IN (SELECT PJID FROM CDRecurr) ");
                if (_GetAllPJRecurrDetailsParam.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = " + _GetAllPJRecurrDetailsParam.Vendor);
                }
                if (_GetAllPJRecurrDetailsParam.SearchValue.Equals(1))
                {
                    varname.Append("       AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("       AND o.Due<='" + DateTime.Now.ToShortDateString() + "'");
                }
                else if (_GetAllPJRecurrDetailsParam.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Balance<>0 AND o.Original<>o.Selected  ");
                    varname.Append("      AND o.Due <= '" + _GetAllPJRecurrDetailsParam.SearchDate + "'");
                }
                if (!string.IsNullOrEmpty(_GetAllPJRecurrDetailsParam.Ref))
                {
                    varname.Append("      AND  p.Ref like '%" + _GetAllPJRecurrDetailsParam.Ref + "%'");

                }
                if (_GetAllPJRecurrDetailsParam.PO > 0)
                {
                    varname.Append("      AND  p.PO like '%" + _GetAllPJRecurrDetailsParam.PO + "%'");
                }
                if (_GetAllPJRecurrDetailsParam.ProjectNumber > 0)
                {
                    varname.Append("      AND  t.vint = " + _GetAllPJRecurrDetailsParam.ProjectNumber);
                }
                if (_GetAllPJRecurrDetailsParam.Amount > 0)
                {
                    varname.Append("      AND  p.Amount like '%" + _GetAllPJRecurrDetailsParam.Amount + "%'");
                }
                if (!string.IsNullOrEmpty(_GetAllPJRecurrDetailsParam.vendorName))
                {
                    //  varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                    varname.Append("      AND   r.Name like '%" + _GetAllPJRecurrDetailsParam.vendorName + "%'");
                }
                //if (_objPJ.Due != null)
                //{
                //    varname.Append("      AND   v.ACCT like '%" + _objPJ.vendorName + "%'");
                //}
                if (_GetAllPJRecurrDetailsParam.Status != 99)
                {
                    varname.Append("      AND   p.Status = " + _GetAllPJRecurrDetailsParam.Status + "");
                }
                if (_GetAllPJRecurrDetailsParam.Terms != 99)
                {
                    varname.Append("      AND   jT.Type = " + _GetAllPJRecurrDetailsParam.Terms + "");
                }
                if (_GetAllPJRecurrDetailsParam.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _GetAllPJRecurrDetailsParam.EN + " AND UC.UserID = " + _GetAllPJRecurrDetailsParam.UserID + " ");
                }
                if (!string.IsNullOrEmpty(_GetAllPJRecurrDetailsParam.Custom1))
                {
                    varname.Append("      AND p.Custom1 like '%" + _GetAllPJRecurrDetailsParam.Custom1 + "%' ");
                }
                if (!string.IsNullOrEmpty(_GetAllPJRecurrDetailsParam.Custom2))
                {
                    varname.Append("      AND p.Custom2 like '%" + _GetAllPJRecurrDetailsParam.Custom2 + "%' ");
                }
                varname.Append("    ORDER BY p.ID");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName FROM PJ AS p, Vendor AS v, Rol AS r WHERE v.Rol=r.ID ORDER BY p.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateOpenAP(OpenAP _objOpenAP)
        {
            try
            {
                string query = "UPDATE OpenAP"
                + " SET Vendor = @Vendor, fDate = @fDate, Due = @Due, fDesc = @fDesc, Original = @Original, Balance = @Balance, Disc = @Disc, Ref = @Ref WHERE PJID = @PJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PJID", _objOpenAP.PJID));
                parameters.Add(new SqlParameter("@Vendor", _objOpenAP.Vendor));
                parameters.Add(new SqlParameter("@fDate", _objOpenAP.fDate));
                parameters.Add(new SqlParameter("@Due", _objOpenAP.Due));
                parameters.Add(new SqlParameter("@fDesc", _objOpenAP.fDesc));
                parameters.Add(new SqlParameter("@Original", _objOpenAP.Original));
                parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@Disc", _objOpenAP.Disc));
                parameters.Add(new SqlParameter("@Ref", _objOpenAP.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateOpenAPBalance(OpenAP _objOpenAP)
        {
            try
            {
                string query = "UPDATE OpenAP"
                + " SET Selected = @Selected, IsSelected = 0 WHERE PJID = @PJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PJID", _objOpenAP.PJID));
                //parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@Selected", _objOpenAP.Selected));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateOpenAPBalance(UpdateOpenAPBalanceParam _objUpdateOpenAPBalanceParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE OpenAP"
                + " SET Selected = @Selected, IsSelected = 0 WHERE PJID = @PJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PJID", _objUpdateOpenAPBalanceParam.PJID));
                //parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@Selected", _objUpdateOpenAPBalanceParam.Selected));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePJ(PJ _objPJ)
        {
            try
            {
                string query = "UPDATE PJ "
                + "SET fDate = @fDate, Ref = @Ref, fDesc = @fDesc,Amount = @Amount, Vendor = @Vendor, Terms = @Terms,PO = @PO, IDate = @IDate, UseTax = @UseTax, Disc = @Disc, Custom1 = @Custom1, Custom2 = @Custom2, Spec = @Spec WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                parameters.Add(new SqlParameter("@fDate", _objPJ.fDate));
                parameters.Add(new SqlParameter("@fDesc", _objPJ.fDesc));
                parameters.Add(new SqlParameter("@Amount", _objPJ.Amount));
                parameters.Add(new SqlParameter("@Vendor", _objPJ.Vendor));
                parameters.Add(new SqlParameter("@Terms", _objPJ.Terms));
                parameters.Add(new SqlParameter("@PO", _objPJ.PO));
                parameters.Add(new SqlParameter("@IDate", _objPJ.IDate));
                parameters.Add(new SqlParameter("@UseTax", _objPJ.UseTax));
                parameters.Add(new SqlParameter("@Disc", _objPJ.Disc));
                parameters.Add(new SqlParameter("@Ref", _objPJ.Ref));
                parameters.Add(new SqlParameter("@Custom1", _objPJ.Custom1));
                parameters.Add(new SqlParameter("@Custom2", _objPJ.Custom2));
                parameters.Add(new SqlParameter("@Spec", _objPJ.Spec));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillHistoryPayment(PJ objPJ)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "@ID";
                paraDate.SqlDbType = SqlDbType.Int;
                paraDate.Value = objPJ.ID;

                return SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.StoredProcedure, "spGetBillHistoryPayment", paraDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillHistoryPayment(GetBillHistoryPaymentParam _GetBillHistoryPaymentParam, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "@ID";
                paraDate.SqlDbType = SqlDbType.Int;
                paraDate.Value = _GetBillHistoryPaymentParam.ID;

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetBillHistoryPayment", paraDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJDetailByID(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();

                varname.Append("SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,v.ID AS VendorID,ISNULL(p.STaxName,'') AS STaxName,ISNULL(p.STaxRate,0) AS STaxRate,st.Type AS STaxType, ISNULL(p.STaxGL,0) AS STaxGL,r.Name AS VendorName,p.ReceivePO,p.IfPaid,r.State as [State],  \n");
                varname.Append("       isnull(o.Due, p.Idate) as Due, ");
                varname.Append("       isnull((select sum(Amount) from PJ where PO = p.po),0) as ReceivedAmount, \n");
                varname.Append("       isnull((select Amount from PO where PO = p.po),0) as POAmount, pa.Paid ,CASE WHEN p.Status = 1 THEN cp.Paid ELSE 0 END AS CrPaid,v.Type VendorType  \n");
                varname.Append("     FROM PJ AS p  \n");
                varname.Append("        LEFT JOIN Vendor AS v ON p.Vendor=v.ID  \n");
                varname.Append("        LEFT JOIN STax AS st ON st.Name = p.STaxName  \n");
                varname.Append("        LEFT JOIN Rol AS r  ON v.Rol=r.ID       \n");
                varname.Append("        LEFT JOIN OpenAP o ON o.PJID = p.ID     \n");
                varname.Append("        LEFT JOIN Paid pa ON pa.TRID = p.TRID AND pa.PITR <> (SELECT ID FROM CD WHERE Status not in (2) and id = PA.PITR)  \n");
                varname.Append("        LEFT JOIN CreditPaid cp ON cp.TRID = p.TRID  \n");
                varname.Append("     WHERE   p.ID=" + _objPJ.ID + "  \n");

                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPJDetailByID(GetPJDetailByIDParam _GetPJDetailByIDParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();

                varname.Append("SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,v.ID AS VendorID,ISNULL(p.STaxName,'') AS STaxName,ISNULL(p.STaxRate,0) AS STaxRate,st.Type AS STaxType, ISNULL(p.STaxGL,0) AS STaxGL,r.Name AS VendorName,p.ReceivePO,p.IfPaid,r.State as [State],  \n");
                varname.Append("       isnull(o.Due, p.Idate) as Due, ");
                varname.Append("       isnull((select sum(Amount) from PJ where PO = p.po),0) as ReceivedAmount, \n");
                varname.Append("       isnull((select Amount from PO where PO = p.po),0) as POAmount, pa.Paid ,CASE WHEN p.Status = 1 THEN cp.Paid ELSE 0 END AS CrPaid   \n");
                varname.Append("     FROM PJ AS p  \n");
                varname.Append("        LEFT JOIN Vendor AS v ON p.Vendor=v.ID  \n");
                varname.Append("        LEFT JOIN STax AS st ON st.Name = p.STaxName  \n");
                varname.Append("        LEFT JOIN Rol AS r  ON v.Rol=r.ID       \n");
                varname.Append("        LEFT JOIN OpenAP o ON o.PJID = p.ID     \n");
                varname.Append("        LEFT JOIN Paid pa ON pa.TRID = p.TRID   \n");
                varname.Append("        LEFT JOIN CreditPaid cp ON cp.TRID = p.TRID  \n");
                varname.Append("     WHERE   p.ID=" + _GetPJDetailByIDParam.ID + "  \n");

                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPJAcctDetailByID(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT t.*,c.Acct+' '+c.fDesc as AcctName FROM PJ pj INNER JOIN Trans t  ON pj.Batch = t.Batch INNER JOIN Chart c ON c.ID = t.Acct  WHERE pj.ID = " + _objPJ.ID + " AND t.Type = 41 AND c.Status = 1");
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJAcctDetailByID(GetPJAcctDetailByIDParam _GetPJAcctDetailByID, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT t.*,c.Acct+' '+c.fDesc as AcctName FROM PJ pj INNER JOIN Trans t  ON pj.Batch = t.Batch INNER JOIN Chart c ON c.ID = t.Acct  WHERE pj.ID = " + _GetPJAcctDetailByID.ID + " AND t.Type = 41 AND c.Status = 1");
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJRecurrDetailByID(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();

                varname.Append("SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Frequency as Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.State as [State],ISNULL(p.STaxName,'') AS STaxName,ISNULL(p.STaxRate,0) AS STaxRate,st.Type AS STaxType, ISNULL(p.STaxGL,0) AS STaxGL,v.ID AS VendorID,r.Name AS VendorName,p.ReceivePO,p.IfPaid,v.Type VendorType,  \n");
                varname.Append("        DATEADD(D,p.Terms, p.fDate) as Due, ");
                varname.Append("       0 as ReceivedAmount, \n");
                varname.Append("       0 as POAmount, 0.00 as Paid ,0 AS CrPaid   \n");
                varname.Append("     FROM PJRecurr AS p  \n");
                varname.Append("        LEFT JOIN Vendor AS v ON p.Vendor=v.ID  \n");
                varname.Append("        LEFT JOIN Rol AS r  ON v.Rol=r.ID       \n");
                varname.Append("        LEFT JOIN STax AS st ON st.Name = v.STax  \n");
                //varname.Append("        LEFT JOIN OpenAP o ON o.PJID = p.ID     \n");
                //varname.Append("        LEFT JOIN Paid pa ON pa.TRID = p.TRID   \n");
                //varname.Append("        LEFT JOIN CreditPaid cp ON cp.TRID = p.TRID  \n");
                varname.Append("     WHERE   p.ID=" + _objPJ.ID + "  \n");

                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPJRecurrDetailByID(GetPJRecurrDetailByIDParam _GetPJRecurrDetailByIDParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();

                varname.Append("SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Frequency as Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.State as [State],ISNULL(p.STaxName,'') AS STaxName,ISNULL(p.STaxRate,0) AS STaxRate,st.Type AS STaxType, ISNULL(p.STaxGL,0) AS STaxGL,v.ID AS VendorID,r.Name AS VendorName,p.ReceivePO,p.IfPaid,  \n");
                varname.Append("        DATEADD(D,p.Terms, p.fDate) as Due, ");
                varname.Append("       0 as ReceivedAmount, \n");
                varname.Append("       0 as POAmount, 0.00 as Paid ,0 AS CrPaid   \n");
                varname.Append("     FROM PJRecurr AS p  \n");
                varname.Append("        LEFT JOIN Vendor AS v ON p.Vendor=v.ID  \n");
                varname.Append("        LEFT JOIN Rol AS r  ON v.Rol=r.ID       \n");
                varname.Append("        LEFT JOIN STax AS st ON st.Name = v.STax  \n");
                //varname.Append("        LEFT JOIN OpenAP o ON o.PJID = p.ID     \n");
                //varname.Append("        LEFT JOIN Paid pa ON pa.TRID = p.TRID   \n");
                //varname.Append("        LEFT JOIN CreditPaid cp ON cp.TRID = p.TRID  \n");
                varname.Append("     WHERE   p.ID=" + _GetPJRecurrDetailByIDParam.ID + "  \n");

                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobIByTransID(JobI _objJobI)
        {
            try
            {
                return _objJobI.Ds = SqlHelper.ExecuteDataset(_objJobI.ConnConfig, CommandType.Text, "SELECT Job,Phase,fDate,Ref,fDesc,Amount,TransID,Type,Labor,Billed,Invoice,UseTax,APTicket FROM JobI WHERE TransID =" + _objJobI.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobIByTransID(GetJobIByTransIDParam _objGetJobIByTransIDParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Job,Phase,fDate,Ref,fDesc,Amount,TransID,Type,Labor,Billed,Invoice,UseTax,APTicket FROM JobI WHERE TransID =" + _objGetJobIByTransIDParam.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteJobI(JobI _objJobI)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objJobI.ConnConfig, CommandType.Text, " DELETE FROM JobI WHERE TransID = " + _objJobI.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillsByVendor(OpenAP _objOpenAP)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT DISTINCT r.Name\n,o.Vendor, \n");
                if (_objOpenAP.Company.ToUpper() == "INNOVATIVE")
                {
                    varname1.Append("       p.fDate, \n");
                }
                else
                {
                    varname1.Append("       o.fDate, \n");
                }
                varname1.Append("       o.Due, \n");
                varname1.Append("       o.Type, \n");
                varname1.Append("       o.fDesc, \n");
                varname1.Append("       o.Original, \n");
                //varname1.Append("       o.Balance, \n");
                //varname1.Append("       o.Selected, \n");
                varname1.Append("       o.Original - (o.Selected + o.Disc) as Balance, \n");
                varname1.Append("       o.Selected + o.Disc as Selected, \n");
                varname1.Append("       o.Disc, \n");
                varname1.Append("       o.PJID, \n");
                varname1.Append("       o.TRID, \n");
                varname1.Append("       o.Disc, CASE WHEN p.Disc = 0 THEN 0  WHEN p.Disc > 0  AND GETDATE() <= DATEADD(D,ISNULL(IfPaid,0),o.fDate) THEN ISNull(o.Original,0)*ISNull(p.Disc,0)/100 ELSE 0 END as Discount,\n");
                varname1.Append("       o.Ref, \n");
                varname1.Append("       p.Status, p.Spec, (CASE p.Spec WHEN 0 THEN 'Input Only' ");
                varname1.Append("       WHEN 1 THEN 'Hold - No Invoices' ");
                varname1.Append("       WHEN 2 THEN 'Hold - No Materials' ");
                varname1.Append("       WHEN 3 THEN 'Hold - Other' ");
                varname1.Append("       WHEN 4 THEN 'Verified' ");
                varname1.Append("       WHEN 5 THEN 'Selected' END) as StatusName, \n");
                varname1.Append("      '0.00' AS Payment, ");
                varname1.Append("      p.fDesc AS billDesc,IsNull(o.IsSelected,0) As IsSelected, (ISNull(o.Original,0) - ISNULL(o.Balance,0) - (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) As Duepayment,v.Type AS VendorType ");
                varname1.Append("      FROM OpenAP o, PJ p ,vendor v  LEFT JOIN Rol r ON r.ID = v.Rol\n");
                // varname1.Append("      , Paid pa \n");
                //varname1.Append("      WHERE NOT EXISTS (SELECT * FROM Paid pa WHERE pa.TRID = o.TRID) AND p.ID=o.PJID AND o.Balance<>0 AND o.Original<>o.Selected AND o.Vendor=" + _objOpenAP.Vendor);
                varname1.Append("       WHERE p.ID=o.PJID and o.Vendor=v.ID\n");

                //varname1.Append("      AND o.Original<>(isnull(o.Selected,0)+isnull(o.Disc,0))  \n");
                varname1.Append("    AND p.Status not in (1,2) AND ((o.Original<>(isnull(o.Selected,0)+isnull(o.Disc,0))) OR o.Original =0)  \n");
                varname1.Append("      AND o.Vendor=" + _objOpenAP.Vendor);
                //varname1.Append("       AND p.Status = 0 ");          // commented by dev on 9th of April, 2016
                if (_objOpenAP.SearchValue.Equals(1))
                {
                    varname1.Append("      AND o.Due <='" + DateTime.Now.ToShortDateString() + "'");
                }
                if (_objOpenAP.SearchValue.Equals(2))
                {
                    varname1.Append("      AND o.Due <='" + _objOpenAP.SearchDate + "'");
                }

                return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, varname1.ToString());
                //return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT o.Vendor,o.fDate,o.Due,o.Type,o.fDesc,o.Original,o.Balance,o.Selected,o.Disc,o.PJID,o.TRID,o.Ref,p.Status,(CASE p.Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END) as StatusName,'0.00' AS Disc, '0.00' AS Payment FROM OpenAP o, PJ p WHERE p.ID=o.PJID AND o.Vendor=" + _objOpenAP.Vendor);
                //return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT o.Vendor,o.fDate,o.Due,o.Type,o.fDesc,o.Original,o.Balance,o.Selected,o.Disc,o.PJID,o.TRID,o.Ref,p.Status,(CASE p.Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END) as StatusName,'0.00' AS Disc, '0.00' AS Payment FROM OpenAP o, PJ p WHERE (SELECT sum(pa.Paid) FROM Paid AS pa WHERE pa.TRID = o.TRID) p.ID=o.PJID AND o.Vendor=" + _objOpenAP.Vendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillsByVendor(GetBillsByVendorParam _GetBillsByVendorParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT DISTINCT r.Name\n,o.Vendor, \n");
                varname1.Append("       o.fDate, \n");
                varname1.Append("       o.Due, \n");
                varname1.Append("       o.Type, \n");
                varname1.Append("       o.fDesc, \n");
                varname1.Append("       o.Original, \n");
                //varname1.Append("       o.Balance, \n");
                //varname1.Append("       o.Selected, \n");
                varname1.Append("       o.Original - (o.Selected + o.Disc) as Balance, \n");
                varname1.Append("       o.Selected + o.Disc as Selected, \n");
                varname1.Append("       o.Disc, \n");
                varname1.Append("       o.PJID, \n");
                varname1.Append("       o.TRID, \n");
                varname1.Append("       o.Disc, CASE WHEN p.Disc = 0 THEN 0  WHEN p.Disc > 0  AND GETDATE() <= DATEADD(D,ISNULL(IfPaid,0),o.fDate) THEN ISNull(o.Original,0)*ISNull(p.Disc,0)/100 ELSE 0 END as Discount,\n");
                varname1.Append("       o.Ref, \n");
                varname1.Append("       p.Status, p.Spec, (CASE p.Spec WHEN 0 THEN 'Input Only' ");
                varname1.Append("       WHEN 1 THEN 'Hold - No Invoices' ");
                varname1.Append("       WHEN 2 THEN 'Hold - No Materials' ");
                varname1.Append("       WHEN 3 THEN 'Hold - Other' ");
                varname1.Append("       WHEN 4 THEN 'Verified' ");
                varname1.Append("       WHEN 5 THEN 'Selected' END) as StatusName, \n");
                varname1.Append("      '0.00' AS Payment, ");
                varname1.Append("      p.fDesc AS billDesc,IsNull(o.IsSelected,0) As IsSelected, (ISNull(o.Original,0) - ISNULL(o.Balance,0) - (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) As Duepayment,v.Type AS VendorType");
                varname1.Append("      FROM OpenAP o, PJ p ,vendor v  LEFT JOIN Rol r ON r.ID = v.Rol\n");
                // varname1.Append("      , Paid pa \n");
                //varname1.Append("      WHERE NOT EXISTS (SELECT * FROM Paid pa WHERE pa.TRID = o.TRID) AND p.ID=o.PJID AND o.Balance<>0 AND o.Original<>o.Selected AND o.Vendor=" + _objOpenAP.Vendor);
                varname1.Append("       WHERE p.ID=o.PJID and o.Vendor=v.ID\n");
                //varname1.Append("      AND pa.TRID = o.TRID AND o.Balance<>0 ");
                varname1.Append("      AND o.Original<>(isnull(o.Selected,0)+isnull(o.Disc,0))  \n");
                varname1.Append("      AND o.Vendor=" + _GetBillsByVendorParam.Vendor);
                //varname1.Append("       AND p.Status = 0 ");          // commented by dev on 9th of April, 2016
                if (_GetBillsByVendorParam.SearchValue.Equals(1))
                {
                    varname1.Append("      AND o.Due <='" + DateTime.Now.ToShortDateString() + "'");
                }
                if (_GetBillsByVendorParam.SearchValue.Equals(2))
                {
                    varname1.Append("      AND o.Due <='" + _GetBillsByVendorParam.SearchDate + "'");
                }

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
                //return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT o.Vendor,o.fDate,o.Due,o.Type,o.fDesc,o.Original,o.Balance,o.Selected,o.Disc,o.PJID,o.TRID,o.Ref,p.Status,(CASE p.Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END) as StatusName,'0.00' AS Disc, '0.00' AS Payment FROM OpenAP o, PJ p WHERE p.ID=o.PJID AND o.Vendor=" + _objOpenAP.Vendor);
                //return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT o.Vendor,o.fDate,o.Due,o.Type,o.fDesc,o.Original,o.Balance,o.Selected,o.Disc,o.PJID,o.TRID,o.Ref,p.Status,(CASE p.Status WHEN 0 THEN 'Input Only' WHEN 1 THEN 'Hold - No Invoices' WHEN 2 THEN 'Hold - No Materials' WHEN 3 THEN 'Hold - Other' WHEN 4 THEN 'Verified' WHEN 5 THEN 'Selected' END) as StatusName,'0.00' AS Disc, '0.00' AS Payment FROM OpenAP o, PJ p WHERE (SELECT sum(pa.Paid) FROM Paid AS pa WHERE pa.TRID = o.TRID) p.ID=o.PJID AND o.Vendor=" + _objOpenAP.Vendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJDetailByBatch(PJ _objPJ)
        {
            try
            {
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR, ReceivePO FROM PJ WHERE Batch=" + _objPJ.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPJDetailByBatch(GetPJDetailByBatchParam _objGetPJDetailByBatchParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR, ReceivePO FROM PJ WHERE Batch=" + _objGetPJDetailByBatchParam.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllCD(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[9];

                para[1] = new SqlParameter
                {
                    ParameterName = "sdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "edate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.EndDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "searchterm",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.searchterm
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchvalue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.searchvalue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.EN
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.UserID
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.PageNumber
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.PageSize
                };


                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetCheckDetailsPaging", para);
                //return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT c.ID, c.fDate, c.Ref, c.fDesc, c.Amount, c.Bank, c.Type, c.Status, c.TransID, c.Vendor, c.French, c.Memo, c.VoidR, c.ACH, r.Name AS VendorName , b.fDesc AS BankName, t.Batch, isnull(t.Sel,0) as Sel FROM CD AS c, Bank AS b, Vendor AS v, Rol AS r, Trans AS t WHERE c.Bank = b.ID AND c.Vendor=v.ID AND v.Rol=r.ID AND c.Status=0 AND c.TransID=t.ID order by fDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllCD(GetAllCDParam _objGetAllCDParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[9];

                para[1] = new SqlParameter
                {
                    ParameterName = "sdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objGetAllCDParam.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "edate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objGetAllCDParam.EndDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "searchterm",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objGetAllCDParam.searchterm
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchvalue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objGetAllCDParam.searchvalue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetAllCDParam.EN
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetAllCDParam.UserID
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetAllCDParam.PageNumber
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetAllCDParam.PageSize
                };

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCheckDetailsPaging", para);
                //return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCheckDetails", para);
                //return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT c.ID, c.fDate, c.Ref, c.fDesc, c.Amount, c.Bank, c.Type, c.Status, c.TransID, c.Vendor, c.French, c.Memo, c.VoidR, c.ACH, r.Name AS VendorName , b.fDesc AS BankName, t.Batch, isnull(t.Sel,0) as Sel FROM CD AS c, Bank AS b, Vendor AS v, Rol AS r, Trans AS t WHERE c.Bank = b.ID AND c.Vendor=v.ID AND v.Rol=r.ID AND c.Status=0 AND c.TransID=t.ID order by fDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCheckRecurrDetails(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[9];

                para[1] = new SqlParameter
                {
                    ParameterName = "sdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "edate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.EndDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "searchterm",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.searchterm
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchvalue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.searchvalue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.EN
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.UserID
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.PageNumber
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.PageSize
                };


                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetCheckRecurrDetailsPaging", para);
                //return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT c.ID, c.fDate, c.Ref, c.fDesc, c.Amount, c.Bank, c.Type, c.Status, c.TransID, c.Vendor, c.French, c.Memo, c.VoidR, c.ACH, r.Name AS VendorName , b.fDesc AS BankName, t.Batch, isnull(t.Sel,0) as Sel FROM CD AS c, Bank AS b, Vendor AS v, Rol AS r, Trans AS t WHERE c.Bank = b.ID AND c.Vendor=v.ID AND v.Rol=r.ID AND c.Status=0 AND c.TransID=t.ID order by fDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCheckRecurrDetails(GetCheckRecurrDetailsParam _objGetCheckRecurrDetailsParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[9];

                para[1] = new SqlParameter
                {
                    ParameterName = "sdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objGetCheckRecurrDetailsParam.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "edate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objGetCheckRecurrDetailsParam.EndDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "searchterm",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objGetCheckRecurrDetailsParam.searchterm
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "searchvalue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objGetCheckRecurrDetailsParam.searchvalue
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetCheckRecurrDetailsParam.EN
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetCheckRecurrDetailsParam.UserID
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetCheckRecurrDetailsParam.PageNumber
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetCheckRecurrDetailsParam.PageSize
                };

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCheckRecurrDetailsPaging", para);
                //return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCheckRecurrDetails", para);
                //return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT c.ID, c.fDate, c.Ref, c.fDesc, c.Amount, c.Bank, c.Type, c.Status, c.TransID, c.Vendor, c.French, c.Memo, c.VoidR, c.ACH, r.Name AS VendorName , b.fDesc AS BankName, t.Batch, isnull(t.Sel,0) as Sel FROM CD AS c, Bank AS b, Vendor AS v, Rol AS r, Trans AS t WHERE c.Bank = b.ID AND c.Vendor=v.ID AND v.Rol=r.ID AND c.Status=0 AND c.TransID=t.ID order by fDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLastCDRef(CD _objCD)
        {
            try
            {
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT fDesc,Bank,Memo,fDate,Ref,ACH FROM CD order by fDate DESC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCDByRef(CD _objCD)
        {
            try
            {
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT fDesc,Bank,Memo,fDate,Ref,ACH FROM CD WHERE Ref=" + _objCD.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDRecon(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET IsRecon=@IsRecon WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objCD.IsRecon));
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetChecksDetails(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[2];

                //para[1] = new SqlParameter
                //{
                //    ParameterName = "@Year",
                //    SqlDbType = SqlDbType.Int,
                //    Value = _objCD.fDateYear
                //};
                para[0] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.Bank
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.fDate
                };
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetCheckDetailsByBank", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCheckDetailsByBankAndRef(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[3];

                //para[1] = new SqlParameter
                //{
                //    ParameterName = "@Year",
                //    SqlDbType = SqlDbType.Int,
                //    Value = _objCD.fDateYear
                //};
                para[0] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.Bank
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Ref1",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objCD.Ref
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref2",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objCD.NextC
                };
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetCheckDetailsByBankAndRef", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPRCheckDetailsByBankAndRef(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.Bank
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Ref1",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objCD.Ref
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref2",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objCD.NextC
                };
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetPRCheckDetailsByBankAndRef", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPRCheckDetailsByBankAndRefDed(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.Bank
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Ref1",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objCD.Ref
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref2",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objCD.NextC
                };
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.StoredProcedure, "spGetPRCheckDetailsByBankAndRefDed", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet GetCheckDetailsByBankAndRef(GetCheckDetailsByBankAndRefParam _objGetCheckDetailsByBankAndRefParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[3];

                //para[1] = new SqlParameter
                //{
                //    ParameterName = "@Year",
                //    SqlDbType = SqlDbType.Int,
                //    Value = _objCD.fDateYear
                //};
                para[0] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _objGetCheckDetailsByBankAndRefParam.Bank
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Ref1",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objGetCheckDetailsByBankAndRefParam.Ref
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref2",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objGetCheckDetailsByBankAndRefParam.NextC
                };
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCheckDetailsByBankAndRef", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCDByID(CD _objCD)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, isnull(t.Sel,0) as Sel, t.Batch,c.TransID, isnull(c.Amount,0) as Amount,CASE c.Type WHEN 0 THEN 'Check'");
                //varname.Append("WHEN 1  THEN 'Cash'");
                //varname.Append("WHEN 2  THEN 'Wire Transfer'");
                //varname.Append("WHEN 3  THEN 'ACH'");
                //varname.Append("ELSE 'Credit Card' END");
                //varname.Append(" AS Type FROM CD as c, Bank as b, Trans as t WHERE c.Bank=b.ID AND c.TransID=t.ID AND c.ID=" + _objCD.ID);

                varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, isnull(t.Sel,0) as Sel, t.Batch,c.TransID, isnull(c.Amount,0) as Amount,rv.Name as VendorName, v.Acct# AS Acct#,c.Type as PaymentType,CASE c.Type WHEN 0 THEN 'Check'");
                varname.Append("WHEN 1  THEN 'Cash'");
                varname.Append("WHEN 2  THEN 'Wire Transfer'");
                varname.Append("WHEN 3  THEN 'ACH'");
                varname.Append("ELSE 'Credit Card' END");
                varname.Append(" AS Type,r.EN,br.Name As Company FROM CD as c");
                varname.Append(" inner join Bank  b on  c.Bank=b.ID");
                varname.Append(" inner join Trans  t on  c.TransID=t.ID");
                varname.Append(" inner join Rol  r on  b.Rol=r.ID");
                varname.Append(" inner join Vendor v on v.ID=c.Vendor inner join Rol rv on rv.ID=v.Rol");
                varname.Append(" left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append(" left outer join Branch br on br.ID = r.EN");
                varname.Append(" WHERE  c.ID=" + _objCD.ID);
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurCDByID(CD _objCD)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, isnull(t.Sel,0) as Sel, t.Batch,c.TransID, isnull(c.Amount,0) as Amount,CASE c.Type WHEN 0 THEN 'Check'");
                //varname.Append("WHEN 1  THEN 'Cash'");
                //varname.Append("WHEN 2  THEN 'Wire Transfer'");
                //varname.Append("WHEN 3  THEN 'ACH'");
                //varname.Append("ELSE 'Credit Card' END");
                //varname.Append(" AS Type FROM CD as c, Bank as b, Trans as t WHERE c.Bank=b.ID AND c.TransID=t.ID AND c.ID=" + _objCD.ID);

                varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, 0 as Sel, 0 AS Batch,c.TransID,ISNULL(c.IsRecon,0) as IsRecon, isnull(c.Amount,0) as Amount,rv.Name as VendorName, v.Acct# AS Acct#,c.Type as PaymentType,CASE c.Type WHEN 0 THEN 'Check'");
                varname.Append("WHEN 1  THEN 'Cash'");
                varname.Append("WHEN 2  THEN 'Wire Transfer'");
                varname.Append("WHEN 3  THEN 'ACH'");
                varname.Append("ELSE 'Credit Card' END");
                varname.Append(" AS Type,r.EN,br.Name As Company,c.PJID,pj.Ref as BillRef,pj.fDesc as BillfDesc,pj.Amount as BillAmount,pj.UseTax as BillUseTax,c.Frequency FROM CDRecurr as c");
                varname.Append(" inner join Bank  b on  c.Bank=b.ID");
                varname.Append(" inner JOIN PJRecurr as pj on pj.ID = c.PJID");
                varname.Append(" inner join Rol  r on  b.Rol=r.ID");
                varname.Append(" inner join Vendor v on v.ID=c.Vendor inner join Rol rv on rv.ID=v.Rol");
                varname.Append(" left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append(" left outer join Branch br on br.ID = r.EN");
                varname.Append(" WHERE  c.ID=" + _objCD.ID);
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurCDByID(GetRecurCDByIDParam _GetRecurCDByIDParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, isnull(t.Sel,0) as Sel, t.Batch,c.TransID, isnull(c.Amount,0) as Amount,CASE c.Type WHEN 0 THEN 'Check'");
                //varname.Append("WHEN 1  THEN 'Cash'");
                //varname.Append("WHEN 2  THEN 'Wire Transfer'");
                //varname.Append("WHEN 3  THEN 'ACH'");
                //varname.Append("ELSE 'Credit Card' END");
                //varname.Append(" AS Type FROM CD as c, Bank as b, Trans as t WHERE c.Bank=b.ID AND c.TransID=t.ID AND c.ID=" + _objCD.ID);

                varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, 0 as Sel, 0 AS Batch,c.TransID,ISNULL(c.IsRecon,0) as IsRecon, isnull(c.Amount,0) as Amount,rv.Name as VendorName, v.Acct# AS Acct#,c.Type as PaymentType,CASE c.Type WHEN 0 THEN 'Check'");
                varname.Append("WHEN 1  THEN 'Cash'");
                varname.Append("WHEN 2  THEN 'Wire Transfer'");
                varname.Append("WHEN 3  THEN 'ACH'");
                varname.Append("ELSE 'Credit Card' END");
                varname.Append(" AS Type,r.EN,br.Name As Company,c.PJID,pj.Ref as BillRef,pj.fDesc as BillfDesc,pj.Amount as BillAmount,pj.UseTax as BillUseTax,c.Frequency FROM CDRecurr as c");
                varname.Append(" inner join Bank  b on  c.Bank=b.ID");
                varname.Append(" inner JOIN PJRecurr as pj on pj.ID = c.PJID");
                varname.Append(" inner join Rol  r on  b.Rol=r.ID");
                varname.Append(" inner join Vendor v on v.ID=c.Vendor inner join Rol rv on rv.ID=v.Rol");
                varname.Append(" left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append(" left outer join Branch br on br.ID = r.EN");
                varname.Append(" WHERE  c.ID=" + _GetRecurCDByIDParam.ID);
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCDByID(GetCDByIDParam _objGetCDByIDParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, isnull(t.Sel,0) as Sel, t.Batch,c.TransID, isnull(c.Amount,0) as Amount,CASE c.Type WHEN 0 THEN 'Check'");
                //varname.Append("WHEN 1  THEN 'Cash'");
                //varname.Append("WHEN 2  THEN 'Wire Transfer'");
                //varname.Append("WHEN 3  THEN 'ACH'");
                //varname.Append("ELSE 'Credit Card' END");
                //varname.Append(" AS Type FROM CD as c, Bank as b, Trans as t WHERE c.Bank=b.ID AND c.TransID=t.ID AND c.ID=" + _objCD.ID);

                varname.Append("SELECT c.ID,c.Vendor,c.fDesc,c.Bank,c.Memo,c.fDate,c.Ref,c.ACH,b.fDesc as BankName, isnull(t.Sel,0) as Sel, t.Batch,c.TransID, isnull(c.Amount,0) as Amount,rv.Name as VendorName, v.Acct# AS Acct#,c.Type as PaymentType,CASE c.Type WHEN 0 THEN 'Check'");
                varname.Append("WHEN 1  THEN 'Cash'");
                varname.Append("WHEN 2  THEN 'Wire Transfer'");
                varname.Append("WHEN 3  THEN 'ACH'");
                varname.Append("ELSE 'Credit Card' END");
                varname.Append(" AS Type,r.EN,br.Name As Company FROM CD as c");
                varname.Append(" inner join Bank  b on  c.Bank=b.ID");
                varname.Append(" inner join Trans  t on  c.TransID=t.ID");
                varname.Append(" inner join Rol  r on  b.Rol=r.ID");
                varname.Append(" inner join Vendor v on v.ID=c.Vendor inner join Rol rv on rv.ID=v.Rol");
                varname.Append(" left outer join tblUserCo UC on UC.CompanyID = r.EN");
                varname.Append(" left outer join Branch br on br.ID = r.EN");
                varname.Append(" WHERE  c.ID=" + _objGetCDByIDParam.ID);
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataTypeCD(CD _objCD)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT COLUMN_NAME, DATA_TYPE");
                sql.Append(" FROM INFORMATION_SCHEMA.COLUMNS");
                sql.Append(" WHERE TABLE_NAME = 'CD'");
                sql.Append(" ORDER BY ORDINAL_POSITION");
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataTypeCD(GetDataTypeCDParam _objGetDataTypeCDParam, string ConnectionString)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT COLUMN_NAME, DATA_TYPE");
                sql.Append(" FROM INFORMATION_SCHEMA.COLUMNS");
                sql.Append(" WHERE TABLE_NAME = 'CD'");
                sql.Append(" ORDER BY ORDINAL_POSITION");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet GetPaidDetailByID(Paid _objPaid)
        {
            try
            {
                return _objPaid.Ds = SqlHelper.ExecuteDataset(_objPaid.ConnConfig, CommandType.Text, "SELECT pj.ID AS PJID,p.PITR,p.fDate,p.Type,p.Line,p.fDesc,p.Original,p.Balance,p.Disc,p.Paid,p.TRID,p.Ref,t.Batch,ISNULL(o.Original,0)-ISNULL(o.Balance,0)-ISNULL(o.Selected,0)+ISNULL(p.Paid,0) as TBalance,pj.fDate as Test FROM Paid AS p INNER JOIN Trans AS t ON p.TRID=t.ID INNER JOIN PJ as pj ON pj.Batch = t.Batch INNER JOIN OpenAP o ON pj.ID = o.PJID WHERE p.PITR=" + _objPaid.PITR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRecurrBillDetailByID(Paid _objPaid)
        {
            try
            {
                var para = new SqlParameter[1];

                //para[1] = new SqlParameter
                //{
                //    ParameterName = "@Year",
                //    SqlDbType = SqlDbType.Int,
                //    Value = _objCD.fDateYear
                //};
                para[0] = new SqlParameter
                {
                    ParameterName = "@CDID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPaid.PITR
                };

                return _objPaid.Ds = SqlHelper.ExecuteDataset(_objPaid.ConnConfig, CommandType.StoredProcedure, "spGetCheckRecurr", para);
                //return _objPaid.Ds = SqlHelper.ExecuteDataset(_objPaid.ConnConfig, CommandType.Text, "SELECT pj.ID AS PJID,cd.ID as PITR,cd.fDate,0 as Type,0 as Line,pj.fDesc,pj.Amount as Original,pj.Amount as Balance,pj.Disc as Disc,0 as Paid,ISNULL(pj.TRID,0) as TRID, pj.Ref,0 as Batch,pj.Amount as TBalance FROM PJRecurr AS pj INNER JOIN  CDRecurr as cd ON pj.ID = cd.PJID WHERE cd.ID=" + _objPaid.PITR);

            }
            catch (Exception ex)
            {
                throw ex;
            }




        }

        public DataSet GetRecurrBillDetailByID(GetRecurrBillDetailByIDParam _GetRecurrBillDetailByIDParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[1];

                //para[1] = new SqlParameter
                //{
                //    ParameterName = "@Year",
                //    SqlDbType = SqlDbType.Int,
                //    Value = _objCD.fDateYear
                //};
                para[0] = new SqlParameter
                {
                    ParameterName = "@CDID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetRecurrBillDetailByIDParam.PITR
                };

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCheckRecurr", para);
                //return _objPaid.Ds = SqlHelper.ExecuteDataset(_objPaid.ConnConfig, CommandType.Text, "SELECT pj.ID AS PJID,cd.ID as PITR,cd.fDate,0 as Type,0 as Line,pj.fDesc,pj.Amount as Original,pj.Amount as Balance,pj.Disc as Disc,0 as Paid,ISNULL(pj.TRID,0) as TRID, pj.Ref,0 as Batch,pj.Amount as TBalance FROM PJRecurr AS pj INNER JOIN  CDRecurr as cd ON pj.ID = cd.PJID WHERE cd.ID=" + _objPaid.PITR);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetPaidDetailByID(GetPaidDetailByIDParam _objGetPaidDetailByIDParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT pj.ID AS PJID,p.PITR,p.fDate,p.Type,p.Line,p.fDesc,p.Original,p.Balance,p.Disc,p.Paid,p.TRID,p.Ref,t.Batch,ISNULL(o.Original,0)-ISNULL(o.Balance,0)-ISNULL(o.Selected,0)+ISNULL(p.Paid,0) as TBalance FROM Paid AS p INNER JOIN Trans AS t ON p.TRID=t.ID INNER JOIN PJ as pj ON pj.Batch = t.Batch INNER JOIN OpenAP o ON pj.ID = o.PJID WHERE p.PITR=" + _objGetPaidDetailByIDParam.PITR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDDate(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET fDate=@fDate, Ref=@Ref WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                parameters.Add(new SqlParameter("@fDate", _objCD.fDate));
                parameters.Add(new SqlParameter("@Ref", _objCD.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateAPCDDate(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objCD.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _objCD.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.MOMUSer
                };
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.StoredProcedure, "spUpdateAPCheckDate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAPCDDate(UpdateAPCDDateParam _UpdateAPCDDateParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateAPCDDateParam.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateAPCDDateParam.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _UpdateAPCDDateParam.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateAPCDDateParam.MOMUSer
                };
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateAPCheckDate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDVoid(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET fDesc=@fDesc, Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objCD.fDesc));
                parameters.Add(new SqlParameter("@Status", _objCD.Status));
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDVoid(UpdateCDVoidParam _objUpdateCDVoidParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE CD SET fDesc=@fDesc, Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objUpdateCDVoidParam.fDesc));
                parameters.Add(new SqlParameter("@Status", _objUpdateCDVoidParam.Status));
                parameters.Add(new SqlParameter("@ID", _objUpdateCDVoidParam.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDVoidOpen(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET fDesc=@fDesc, Status=@Status,Amount= @Amount WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objCD.fDesc));
                parameters.Add(new SqlParameter("@Status", _objCD.Status));
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                parameters.Add(new SqlParameter("@Amount", _objCD.Amount));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCDVoidOpen(UpdateCDVoidOpenParam _objUpdateCDVoidOpenParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE CD SET fDesc=@fDesc, Status=@Status,Amount= @Amount WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objUpdateCDVoidOpenParam.fDesc));
                parameters.Add(new SqlParameter("@Status", _objUpdateCDVoidOpenParam.Status));
                parameters.Add(new SqlParameter("@ID", _objUpdateCDVoidOpenParam.ID));
                parameters.Add(new SqlParameter("@Amount", _objUpdateCDVoidOpenParam.Amount));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateAPCDVoidLog(CD _objCD)
        {
            try
            {
                var para = new SqlParameter[6];
                para[0] = new SqlParameter
                {
                    ParameterName = "@fUser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.MOMUSer
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Screen",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.French
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.Int,
                    Value = _objCD.ID
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "@Field",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.Memo
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@OldVal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.searchterm
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@NewVal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objCD.searchvalue
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.StoredProcedure, "Log2_Insert", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAPCDVoidLog(UpdateAPCDVoidLogParam _objUpdateAPCDVoidLogParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[6];
                para[0] = new SqlParameter
                {
                    ParameterName = "@fUser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objUpdateAPCDVoidLogParam.MOMUSer
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Screen",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objUpdateAPCDVoidLogParam.French
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.Int,
                    Value = _objUpdateAPCDVoidLogParam.ID
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "@Field",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objUpdateAPCDVoidLogParam.Memo
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@OldVal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objUpdateAPCDVoidLogParam.searchterm
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@NewVal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objUpdateAPCDVoidLogParam.searchvalue
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "Log2_Insert", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByPaidTrans(Paid _objPaid)
        {
            try
            {
                return _objPaid.Ds = SqlHelper.ExecuteDataset(_objPaid.ConnConfig, CommandType.Text, "SELECT p.PITR,p.fDate,p.Type,p.Line,p.fDesc,p.Original,p.Balance,p.Disc,p.Paid,p.TRID,p.Ref,t.Batch FROM Paid AS p, Trans AS t WHERE p.TRID = t.ID PITR=" + _objPaid.PITR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOpenAPByPJID(OpenAP _objOpenAP)
        {
            try
            {
                return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, "SELECT Vendor, fDate, Due, Type, fDesc, Original, Balance, Selected, Disc, PJID, TRID, Ref FROM OpenAP WHERE PJID = " + _objOpenAP.PJID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenAPByPJID(GetOpenAPByPJIDParam _objGetOpenAPByPJIDParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Vendor, fDate, Due, Type, fDesc, Original, Balance, Selected, Disc, PJID, TRID, Ref FROM OpenAP WHERE PJID = " + _objGetOpenAPByPJIDParam.PJID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteOpenAPByPJID(OpenAP _objOpenAP)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objOpenAP.ConnConfig, CommandType.Text, " DELETE FROM OpenAP WHERE PJID = " + _objOpenAP.PJID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePJOnVoidCheck(PJ _objPJ)
        {
            try
            {
                string query = "UPDATE PJ SET fDesc=@fDesc, Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objPJ.fDesc));
                parameters.Add(new SqlParameter("@Status", _objPJ.Status));
                parameters.Add(new SqlParameter("@ID", _objPJ.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePJOnVoidCheck(UpdatePJOnVoidCheckParam _objUpdatePJOnVoidCheckParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE PJ SET fDesc=@fDesc, Status=@Status WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fDesc", _objUpdatePJOnVoidCheckParam.fDesc));
                parameters.Add(new SqlParameter("@Status", _objUpdatePJOnVoidCheckParam.Status));
                parameters.Add(new SqlParameter("@ID", _objUpdatePJOnVoidCheckParam.ID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePaidOnVoidCheck(Paid _objPaid)
        {
            try
            {
                string query = "UPDATE Paid SET Paid=0 WHERE PITR=@PITR AND TRID = @TRID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PITR", _objPaid.PITR));
                parameters.Add(new SqlParameter("@TRID", _objPaid.TRID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPaid.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePaidOnVoidCheck(UpdatePaidOnVoidCheckParam _objUpdatePaidOnVoidCheckParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Paid SET Paid=0 WHERE PITR=@PITR AND TRID = @TRID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PITR", _objUpdatePaidOnVoidCheckParam.PITR));
                parameters.Add(new SqlParameter("@TRID", _objUpdatePaidOnVoidCheckParam.TRID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCDByTransID(CD _objCD)
        {
            try
            {
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT ID,fDesc,Bank,Memo,fDate,Ref,ACH,TransID FROM CD WHERE TransID=" + _objCD.TransID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJByTransID(PJ _objPJ)
        {
            try
            {
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, " SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE TRID=" + _objPJ.TRID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCheckDetails(CD _objCD)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objCD.ConnConfig, "spDeleteCheckDetails", _objCD.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCheckDetails(DeleteCheckDetailsParam _objDeleteCheckDetailsParam, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteDataset(ConnectionString, "spDeleteCheckDetails", _objDeleteCheckDetailsParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteRecurrCheck(CD _objCD)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objCD.ConnConfig, "spDeleteRecurrCheck", _objCD.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteRecurrCheck(DeleteRecurrCheckParam _objDeleteRecurrCheckParam, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteDataset(ConnectionString, "spDeleteRecurrCheck", _objDeleteRecurrCheckParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPJByID(PJ _objPJ)
        {
            try
            {
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, " SELECT ID,fDate,Ref,fDesc,Amount,Vendor,Status,Batch,Terms,PO,TRID,Spec,IDate,UseTax,Disc,Custom1,Custom2,ReqBy,VoidR FROM PJ WHERE ID=" + _objPJ.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPJItem(PJ _objPJ)
        {
            try
            {
                string query = "INSERT INTO PJItem(TRID,Stax,Amount,UseTax)"
               + " VALUES(@TRID, @Stax, @Amount, @UseTax)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@TRID", _objPJ.TRID));
                parameters.Add(new SqlParameter("@Stax", _objPJ.UtaxName));
                parameters.Add(new SqlParameter("@Amount", _objPJ.Amount));
                parameters.Add(new SqlParameter("@UseTax", _objPJ.UseTax));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePJItem(PJ _objPJ)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, CommandType.Text, " DELETE FROM PJItem WHERE TRID = " + _objPJ.TRID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteAPBill(PJ _objPJ)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, "spDeleteAPBill", _objPJ.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteAPBill(DeleteAPBillParam _DeleteAPBillParam, string ConnectionString)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, "spDeleteAPBill", _DeleteAPBillParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteAPBillRecurr(PJ _objPJ)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPJ.ConnConfig, "spDeleteAPBillRecurr", _objPJ.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteAPBillRecurr(DeleteAPBillRecurrParam _DeleteAPBillRecurrParam, string ConnectionString)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, "spDeleteAPBillRecurr", _DeleteAPBillRecurrParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProcessRecurrCount(PJ _objPJ)
        {
            try
            {
                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT Count(*) AS CountRecur FROM PJRecurr Where fDate <='" + System.DateTime.Now + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProcessRecurrCount(GetProcessRecurrCountParam _GetProcessRecurrCountParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Count(*) AS CountRecur FROM PJRecurr Where fDate <='" + System.DateTime.Now + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProcessRecurrCheckCount(CD _objCD)
        {
            try
            {
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "SELECT Count(*) AS CountRecur FROM CDRecurr Where fDate <='" + System.DateTime.Now + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProcessRecurrCheckCount(GetProcessRecurrCheckCountParam _objGetProcessRecurrCheckCountParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Count(*) AS CountRecur FROM CDRecurr Where fDate <='" + System.DateTime.Now + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExistCheckNum(CD _objCD)
        {
            try
            {
                return _objCD.IsExistCheckNo = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objCD.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Ref FROM CD WHERE Ref= " + _objCD.Ref + " AND Bank=" + _objCD.Bank + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExistCheckNum(IsExistCheckNumParam _IsExistCheckNumParam, string ConnectionString)
        {
            try
            {
                return _IsExistCheckNumParam.IsExistCheckNo = Convert.ToBoolean(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Ref FROM CD WHERE Ref= " + _IsExistCheckNumParam.Ref + " AND Bank=" + _IsExistCheckNumParam.Bank + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistCheckNumOnEdit(CD _objCD)
        {
            try
            {
                return _objCD.IsExistCheckNo = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objCD.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Ref FROM CD WHERE Ref= " + _objCD.Ref + " AND Bank=" + _objCD.Bank + " AND ID <> " + _objCD.ID + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public bool IsExistCheckNumOnEdit(IsExistCheckNumOnEditParam _IsExistCheckNumOnEdit, string ConnectionString)
        {
            try
            {
                return _IsExistCheckNumOnEdit.IsExistCheckNo = Convert.ToBoolean(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Ref FROM CD WHERE Ref= " + _IsExistCheckNumOnEdit.Ref + " AND Bank=" + _IsExistCheckNumOnEdit.Bank + " AND Type=" + _IsExistCheckNumOnEdit.Vendor + " AND ID <> " + _IsExistCheckNumOnEdit.ID + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetBankID(CD _objCD)
        {
            try
            {
                return _objCD.Bank = Convert.ToInt32(SqlHelper.ExecuteScalar(_objCD.ConnConfig, CommandType.Text, "SELECT Bank FROM CD WHERE ID=" + _objCD.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCDCheckNo(CD _objCD)
        {
            try
            {
                string query = "UPDATE CD SET Ref=@Ref WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objCD.ID));
                //parameters.Add(new SqlParameter("@fDate", _objCD.fDate));
                parameters.Add(new SqlParameter("@Ref", _objCD.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCDCheckNo(UpdateCDCheckNoParam _UpdateCDCheckNoParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE CD SET Ref=@Ref WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _UpdateCDCheckNoParam.ID));
                //parameters.Add(new SqlParameter("@fDate", _objCD.fDate));
                parameters.Add(new SqlParameter("@Ref", _UpdateCDCheckNoParam.Ref));

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillsDetailsByDue(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.ID,p.fDate,o.Due, p.Ref,p.fDesc,o.Balance As Total,p.Vendor As VendorID,r.Name AS Vendor, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open'                      \n");
                varname.Append("                            WHEN 1 THEN 'Closed'                    \n");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR, \n");
                varname.Append("    case when isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0 then 0 else isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) end AS DueIn,    \n");
                varname.Append("    p.Amount,   \n");
                varname.Append("    CASE WHEN ((isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 0) OR (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0)) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 7) \n");
                varname.Append("        THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    ELSE 0          \n");
                varname.Append("    END as SevenDay,     \n");
                varname.Append("    CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 8) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 30)   \n");
                varname.Append("    	THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    	ELSE 0      \n");
                varname.Append("     END as ThirtyDay,   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 60)   \n");
                varname.Append("    	THEN   \n");
                varname.Append("    	o.Balance  \n");
                varname.Append("    	ELSE 0   \n");
                varname.Append("     END as SixtyDay,	   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 61) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <=90)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END as NintyDay,  \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 91)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END as NintyOneDay  \n");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                varname.Append("     WHERE  o.Balance<>0    \n");

                if (_objPJ.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Due <='" + _objPJ.SearchDate + "'           \n");
                }
                if (_objPJ.Vendor > 0)
                {
                    varname.Append("      AND p.Vendor = " + _objPJ.Vendor);
                }

                varname.Append("    ORDER BY p.ID");

                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillsDetailsByDate(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.ID,p.fDate, p.IDate,o.Due, p.Ref,p.fDesc,o.Balance As Total,p.Vendor As VendorID,r.Name AS Vendor, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open'                      \n");
                varname.Append("                            WHEN 1 THEN 'Closed'                    \n");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR, \n");
                varname.Append("    case when isnull(DATEDIFF(day,p.IDate,GETDATE()),0) < 0 then 0 else isnull(DATEDIFF(day,p.IDate,GETDATE()),0) end AS DueIn,    \n");
                varname.Append("    p.Amount,   \n");
                varname.Append("    CASE WHEN (isnull(DATEDIFF(day,p.IDate,GETDATE()),0) >= 0) AND (isnull(DATEDIFF(day,p.IDate,GETDATE()),0) <= 30)   \n");
                varname.Append("    	THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    	ELSE 0      \n");
                varname.Append("     END as ThirtyDay,   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,p.IDate,GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,p.IDate,GETDATE()),0) <= 60)   \n");
                varname.Append("    	THEN   \n");
                varname.Append("    	o.Balance  \n");
                varname.Append("    	ELSE 0   \n");
                varname.Append("     END as SixtyDay,	   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,p.IDate,GETDATE()),0) >= 61) AND (isnull(DATEDIFF(day,p.IDate,GETDATE()),0) <=90)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END as NintyDay,  \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,p.IDate,GETDATE()),0) >= 91)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END as NintyOneDay  \n");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                varname.Append("     WHERE  o.Balance<>0    \n");

                if (_objPJ.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Due <='" + _objPJ.SearchDate + "'           \n");
                }
                if (_objPJ.Vendor > 0)
                {
                    varname.Append("      AND p.Vendor = " + _objPJ.Vendor);
                }

                varname.Append("    ORDER BY p.ID");

                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillsDetailsByDue(GetBillsDetailsByDueParam _GetBillsDetailsByDueParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.ID,p.fDate,o.Due, p.Ref,p.fDesc,o.Balance As Total,p.Vendor As VendorID,r.Name AS Vendor, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open'                      \n");
                varname.Append("                            WHEN 1 THEN 'Closed'                    \n");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR, \n");
                varname.Append("    case when isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0 then 0 else isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) end AS DueIn,    \n");
                varname.Append("    p.Amount,   \n");
                varname.Append("    CASE WHEN ((isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 0) OR (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0)) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 7) \n");
                varname.Append("        THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    ELSE 0          \n");
                varname.Append("    END as SevenDay,     \n");
                varname.Append("    CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 8) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 30)   \n");
                varname.Append("    	THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    	ELSE 0      \n");
                varname.Append("     END as ThirtyDay,   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 60)   \n");
                varname.Append("    	THEN   \n");
                varname.Append("    	o.Balance  \n");
                varname.Append("    	ELSE 0   \n");
                varname.Append("     END as SixtyDay,	   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 61) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <=90)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END as NintyDay,  \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 91)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END as NintyOneDay  \n");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 0) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 7) , o.Balance, 0)) as SevenDay     ");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 8) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 30) , o.Balance, 0)) as ThirtyDay   ");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,o.Due,GETDATE()),0) <= 60) , o.Balance, 0)) as SixtyDay   ");
                //varname.Append("    ,(IIF((isnull(DATEDIFF(day,o.Due,GETDATE()),0) >= 61) , o.Balance , 0)) as SixtyOneDay  ");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     inner join Vendor AS v on p.Vendor = v.ID");
                varname.Append("     inner join Rol AS r on v.Rol = r.ID");
                varname.Append("    left join openAP AS o on p.ID = o.PJID");
                //varname.Append("    WHERE (p.fDate >= '" + _objPJ.StartDate + "') AND (p.fDate <= '" + _objPJ.EndDate + "')     \n");
                //varname.Append("      AND p.Status = 0 ");
                varname.Append("     WHERE  o.Balance<>0    \n");
                //varname.Append("          AND o.Original<>o.Selected     \n");
                //if (_objPJ.SearchValue.Equals(1))
                //{
                //    varname.Append("      AND o.Due <='" + DateTime.Now.ToShortDateString() + "'    \n");
                //}
                if (_GetBillsDetailsByDueParam.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Due <='" + _GetBillsDetailsByDueParam.SearchDate + "'           \n");
                }
                if (_GetBillsDetailsByDueParam.Vendor > 0)
                {
                    varname.Append("      AND p.Vendor = " + _GetBillsDetailsByDueParam.Vendor);
                }

                varname.Append("    ORDER BY p.ID");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
                //return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "SELECT p.ID,p.fDate,p.Ref,p.fDesc,p.Amount,p.Vendor,p.Status,p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR,r.Name AS VendorName FROM PJ AS p, Vendor AS v, Rol AS r WHERE v.Rol=r.ID ORDER BY p.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillsDetails360ByDue(PJ _objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.ID,p.fDate,o.Due, p.Ref,p.fDesc,o.Balance As Total,p.Vendor As VendorID,r.Name AS Vendor, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open'                      \n");
                varname.Append("                            WHEN 1 THEN 'Closed'                    \n");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR, \n");
                varname.Append("    case when isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0 then 0 else isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) end AS DueIn,    \n");
                varname.Append("    p.Amount,   \n");
                varname.Append("    CASE WHEN ((isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 0) OR (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0)) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 30) \n");
                varname.Append("        THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    ELSE 0          \n");
                varname.Append("    END AS ThirtyDay,     \n");
                varname.Append("    CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 90)   \n");
                varname.Append("    	THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    	ELSE 0      \n");
                varname.Append("     END AS NintyDay,   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 91) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 360)   \n");
                varname.Append("    	THEN   \n");
                varname.Append("    	o.Balance  \n");
                varname.Append("    	ELSE 0   \n");
                varname.Append("     END AS ThreeSixtyDay,	   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) > 360)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END AS OverThreeSixtyDay   \n");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     INNER JOIN Vendor AS v ON p.Vendor = v.ID");
                varname.Append("     INNER JOIN Rol AS r ON v.Rol = r.ID");
                varname.Append("     LEFT JOIN OpenAP AS o ON p.ID = o.PJID");
                varname.Append("    WHERE  o.Balance <> 0    \n");

                if (_objPJ.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Due <='" + _objPJ.SearchDate + "'           \n");
                }
                if (_objPJ.Vendor > 0)
                {
                    varname.Append("      AND p.Vendor = " + _objPJ.Vendor);
                }

                varname.Append("    ORDER BY p.ID");

                return _objPJ.Ds = SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetBillsDetails360ByDue(GetBillsDetails360ByDueParam _GetBillsDetails360ByDue, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.ID,p.fDate,o.Due, p.Ref,p.fDesc,o.Balance As Total,p.Vendor As VendorID,r.Name AS Vendor, \n");
                varname.Append("    p.Status, \n");
                varname.Append("    p.Status,(CASE p.Status WHEN 0 THEN 'Open'                      \n");
                varname.Append("                            WHEN 1 THEN 'Closed'                    \n");
                varname.Append("                            WHEN 2 THEN 'Void'  END) AS StatusName, \n");
                varname.Append("    p.Batch,p.Terms,p.PO,p.TRID,p.Spec,p.IDate,p.UseTax,p.Disc,p.Custom1,p.Custom2,p.ReqBy,p.VoidR, \n");
                varname.Append("    case when isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0 then 0 else isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) end AS DueIn,    \n");
                varname.Append("    p.Amount,   \n");
                varname.Append("    CASE WHEN ((isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 0) OR (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) < 0)) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 30) \n");
                varname.Append("        THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    ELSE 0          \n");
                varname.Append("    END AS ThirtyDay,     \n");
                varname.Append("    CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 90)   \n");
                varname.Append("    	THEN        \n");
                varname.Append("    	o.Balance   \n");
                varname.Append("    	ELSE 0      \n");
                varname.Append("     END AS NintyDay,   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) >= 91) AND (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) <= 360)   \n");
                varname.Append("    	THEN   \n");
                varname.Append("    	o.Balance  \n");
                varname.Append("    	ELSE 0   \n");
                varname.Append("     END AS ThreeSixtyDay,	   \n");
                varname.Append("     CASE WHEN (isnull(DATEDIFF(day,ISNULL(o.Due,DATEADD(DAY,p.Terms,p.fDate)),GETDATE()),0) > 360)  \n");
                varname.Append("     THEN       \n");
                varname.Append("     	o.Balance   \n");
                varname.Append("        ELSE 0      \n");
                varname.Append("     END AS OverThreeSixtyDay   \n");
                varname.Append("    FROM PJ AS p ");
                varname.Append("     INNER JOIN Vendor AS v ON p.Vendor = v.ID");
                varname.Append("     INNER JOIN Rol AS r ON v.Rol = r.ID");
                varname.Append("     LEFT JOIN OpenAP AS o ON p.ID = o.PJID");
                varname.Append("    WHERE  o.Balance <> 0    \n");

                if (_GetBillsDetails360ByDue.SearchValue.Equals(2))
                {
                    varname.Append("      AND o.Due <='" + _GetBillsDetails360ByDue.SearchDate + "'           \n");
                }
                if (_GetBillsDetails360ByDue.Vendor > 0)
                {
                    varname.Append("      AND p.Vendor = " + _GetBillsDetails360ByDue.Vendor);
                }

                varname.Append("    ORDER BY p.ID");

                return _GetBillsDetails360ByDue.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllPO(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select p.*,                                                 \n");
                varname.Append("        r.Name as VendorName,                                   \n");
                varname.Append("         (CASE isnull(p.Status,0) WHEN 0 THEN 'Open'            \n");
                varname.Append("            WHEN 1 THEN 'Closed'                                \n");
                varname.Append("            WHEN 2 THEN 'Void'                                  \n");
                varname.Append("            WHEN 3 THEN 'Partial-Quantity'                      \n");
                varname.Append("            WHEN 4 THEN 'Partial-Amount'                        \n");
                varname.Append("            WHEN 5 THEN 'Closed At Receive PO' END) AS StatusName,     \n");
                varname.Append("           isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( poitem.Job as nvarchar)     \n");
                varname.Append("            FROM poitem where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Projectnumber,     \n");
                varname.Append("            isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( Loc.ID as nvarchar)     \n");
                varname.Append("            FROM POItem inner join Job on POItem.Job=Job.ID     \n");
                varname.Append("            inner join Loc on Job.Loc=Loc.Loc where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Location,     \n");
                varname.Append("            isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( poitem.fDesc as nvarchar)     \n");
                varname.Append("             FROM poitem where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Part    \n");
                varname.Append("        FROM PO as p                                            \n");
                varname.Append("            		left join Vendor as v on p.Vendor = v.ID    \n");
                varname.Append("                    left join Rol as r on v.Rol = r.ID          \n");
                varname.Append("                    order by p.PO       \n");


                //varname.Append("    select p.po, p.Amount as poamt,pj.amount, case when pj.Amount is null then 'Open'   \n");
                //varname.Append("    			when (pj.amount = p.Amount) or (pj.amount > p.Amount) then 'Closed'     \n");
                //varname.Append("    			when pj.amount < p.Amount then 'Partial-Amount' end as Status           \n");
                //varname.Append("    , p.* from po as p left join pj as pj on p.PO = pj.PO                               \n");
                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOById(PO _objPO)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "PO";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _objPO.POID;

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "EN";
                param1.SqlDbType = SqlDbType.Int;
                param1.Value = _objPO.EN;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = Convert.ToInt32(_objPO.UserID);


                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPOById", param, param1, paramUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListPO(PO _objPO, string pos)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "POs";
                param.SqlDbType = SqlDbType.Text;
                param.Value = pos;

                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetListPO", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPOByIdAjax(PO _objPO)
        {
            try
            {


                SqlParameter param = new SqlParameter();
                param.ParameterName = "PO";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _objPO.POID;

                //SqlParameter param1 = new SqlParameter();
                //param1.ParameterName = "EN";
                //param1.SqlDbType = SqlDbType.Int;
                //param1.Value = _objPO.EN;

                //SqlParameter paramUserID = new SqlParameter();
                //paramUserID.ParameterName = "UserID";
                //paramUserID.SqlDbType = SqlDbType.Int;
                //paramUserID.Value = Convert.ToInt32(_objPO.UserID);


                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPOByIdAjax", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetOutStandingPOById(PO _objPO)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "PO";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _objPO.POID;


                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetOutStandingPOById", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOutStandingPOById(string ConnectionString, GetOutStandingPOByIdParam _GetOutStandingPOByIdParam)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "PO";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _GetOutStandingPOByIdParam.POID;


                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetOutStandingPOById", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOSign(ApprovePOStatus _objApprovePO)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "PO";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _objApprovePO.POID;

                return SqlHelper.ExecuteDataset(_objApprovePO.ConnConfig, CommandType.StoredProcedure, "spGetPOSign", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOApproveDetails(PO _objPO)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "UserID";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _objPO.UserID;

                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPOApproveDetails", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet POApproveDetails(ApprovePOStatus _objApprovePOStatus)
        {
            try
            {
                var para = new SqlParameter[7];

                para[0] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objApprovePOStatus.POID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _objApprovePOStatus.Status
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objApprovePOStatus.UserID
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "Comments",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _objApprovePOStatus.Comments
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "Signature",
                    SqlDbType = SqlDbType.Image,
                    Value = _objApprovePOStatus.Signature
                };

                para[5] = new SqlParameter
                {
                    ParameterName = "FilePath",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _objApprovePOStatus.FilePath
                };

                para[6] = new SqlParameter
                {
                    ParameterName = "FileName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _objApprovePOStatus.FileName
                };

                return SqlHelper.ExecuteDataset(_objApprovePOStatus.ConnConfig, CommandType.StoredProcedure, "spUpdatePOApproveDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPODetailsForMailALL(ApprovePOStatus _objApprovePOStatus)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "POIDs",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objApprovePOStatus.POIDs
                };
                return SqlHelper.ExecuteDataset(_objApprovePOStatus.ConnConfig, CommandType.StoredProcedure, "spGetPODetailsForMailALL", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVenderDetailsForMailALL(ApprovePOStatus _objApprovePOStatus)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "POIDs",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objApprovePOStatus.POIDs
                };
                return SqlHelper.ExecuteDataset(_objApprovePOStatus.ConnConfig, CommandType.StoredProcedure, "spGetVenderDetailsForMailALL", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOByTicketId(PO _objPO)
        {
            try
            {

                SqlParameter param = new SqlParameter();
                param.ParameterName = "TicketId";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _objPO.POID;


                return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPOByTicketId", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePOById(PO _objPO)
        {
            try
            {
                //StringBuilder varname = new StringBuilder();
                //varname.Append("    DELETE FROM POItem WHERE PO = '" + _objPO.POID + "' ");
                //SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());

                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "POID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "POAmount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "PODescription",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Status
                };

                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spDeletePO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[27];

                para[0] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "VendorId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Status
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "ShipVia",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipVia
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Terms",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Terms
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "FOB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.FOB
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "ShipTo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipTo
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Approved",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Approved
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "ApprovedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ApprovedBy
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "ReqBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.ReqBy
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "fBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fBy
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "POReasonCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.POReasonCode
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "CourrierAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.CourrierAcct
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "PORevision",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.PORevision
                };
                para[20] = new SqlParameter
                {
                    ParameterName = "POItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objPO.PODt
                };
                para[21] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };
                para[22] = new SqlParameter
                {
                    ParameterName = "RequestedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.RequestedBy
                };
                para[23] = new SqlParameter
                {
                    ParameterName = "@SalesOrderNo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.SalesOrderNo
                };
                para[24] = new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };
                para[25] = new SqlParameter
                {
                    ParameterName = "@IsPOClose",
                    SqlDbType = SqlDbType.Bit,
                    Value = _objPO.IsPOClose
                };
                para[26] = new SqlParameter
                {
                    ParameterName = "@IsAddReceivePO",
                    SqlDbType = SqlDbType.Bit,
                    Value = _objPO.IsAddReceivePO
                };
                

                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spAddPO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[28];

                para[0] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "VendorId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Status
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "ShipVia",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipVia
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Terms",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Terms
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "FOB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.FOB
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "ShipTo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ShipTo
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Approved",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = _objPO.Approved
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "ApprovedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.ApprovedBy
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "ReqBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.ReqBy
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "fBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.fBy
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "POReasonCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.POReasonCode
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "CourrierAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.CourrierAcct
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "PORevision",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.PORevision
                };
                para[20] = new SqlParameter
                {
                    ParameterName = "POItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objPO.PODt
                };
                para[21] = new SqlParameter
                {
                    ParameterName = "ApprovalStatus",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.ApprovalStatus
                };
                para[22] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };
                para[23] = new SqlParameter
                {
                    ParameterName = "@RequestedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.RequestedBy
                };
                para[24] = new SqlParameter
                {
                    ParameterName = "@IsPOClose",
                    SqlDbType = SqlDbType.Bit,
                    Value = _objPO.IsPOClose
                };
                para[25] = new SqlParameter
                {
                    ParameterName = "@SalesOrderNo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.SalesOrderNo
                };
                para[26] = new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };
                para[27] = new SqlParameter
                {
                    ParameterName = "@IsAddReceivePO",
                    SqlDbType = SqlDbType.Bit,
                    Value = _objPO.IsAddReceivePO
                };
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spUpdatePO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxPOId(PO _objPO)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, "SELECT isnull(max(PO),0) +1 as PO FROM PO"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsFirstPo(PO _objPO)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PO)THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOBalance(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE Chart  \n");
                varname.Append("    SET Balance = ISNULL (p.Balance , 0)    \n");
                varname.Append("          FROM Chart c LEFT JOIN            \n");
                varname.Append("            (SELECT Sum(Amount) AS Balance  \n");
                varname.Append("                FROM PO) p                      \n");
                varname.Append("                ON c.DefaultNo = 'D9991' AND Status = 0   \n");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOByVendor(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT p.*, r.Name as VendorName, r.Address +', '+ CHAR(13)+ r.City +', '+ r.State+', '+ r.Zip as Address                 \n");
                varname.Append("            FROM PO as p INNER JOIN Vendor as v         \n");
                varname.Append("                ON p.Vendor = v.ID                      \n");
                varname.Append("                INNER JOIN Rol as r ON v.Rol = r.ID         \n");
                varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN  \n");
                varname.Append("     left outer join Branch B on B.ID = r.EN  \n");
                varname.Append("                WHERE p.Vendor = '" + _objPO.Vendor + "' AND (p.Status=0 OR p.Status=3 OR p.Status=4)  \n");
                if (_objPO.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
                }
                varname.Append("        SELECT  \n");
                varname.Append("            poi.PO, poi.Line, poi.Quan, poi.fDesc, poi.Price, poi.Amount, poi.Job, poi.Phase, poi.due,J.fDesc As JobName,Loc.tag As LocationName,   \n");
                varname.Append("            poi.Amount as Ordered,                      \n");
                varname.Append("            poi.Selected as PrvIn,                      \n");
                varname.Append("            poi.Balance as Outstanding,                 \n");
                varname.Append("            0.00 as Received,                           \n");
                varname.Append("            poi.Quan as OrderedQuan,            \n");
                varname.Append("            poi.SelectedQuan as PrvInQuan,      \n");
                varname.Append("            poi.BalanceQuan as OutstandQuan,    \n");
                varname.Append("            0.00 as ReceivedQuan, 0 As IsReceiveIssued,              \n");
                varname.Append("            poi.Inv, poi.GL, poi.Freight, poi.Rquan, poi.Billed, poi.Ticket,poi.WarehouseID,poi.LocationID  ,       \n");



                varname.Append("    isNULL((SELECT top 1  1 FROM INV WHERE ID = (poi.Inv)and type = 0),0) IsItemsExistsInInventory  ,                 \n");
                varname.Append("    ( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW inner join Warehouse AS Wh on Wh.ID = INW.WarehouseID   where  INW.InvID=poi.Inv  and    INW.WareHouseID=poi.WarehouseID) As WarehouseName  ,             \n");
                varname.Append("    (Select top 1 Name from WHLoc WH  where WH.WareHouseID = poi.WarehouseID and id = poi.LocationID) As WarehouseLoc   ,    \n");
                varname.Append("     (                 \n");
                varname.Append("   SELECT  top 1                 \n");
                varname.Append(" isnull(bt.Type, '') as Phase                 \n");
                varname.Append(" FROM POItem as ppp                 \n");
                varname.Append(" LEFT JOIN JobTItem as jt ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                 \n");
                varname.Append(" INNER JOIN BOM as b ON b.JobTItemID = jt.ID                 \n");
                varname.Append(" LEFT JOIN Inv as i on i.ID = ppp.Inv and b.matitem = i.id                 \n");
                varname.Append(" inner join BOMT bt on bt.ID = b.Type                 \n");
                varname.Append(" WHERE ppp.PO = poi.PO and ppp.Line = poi.Line  ) as IsInventoryCode                 \n");



                varname.Append("            FROM POItem as poi LEFT JOIN PO as p ON poi.PO = p.PO                   \n");
                varname.Append("            left outer JOIN Job as J ON poi.Job = J.ID   \n");
                varname.Append("    left outer JOIN Loc as Loc ON Loc.Loc = J.Loc   \n");
                varname.Append("             INNER JOIN Vendor as v    ON p.Vendor = v.ID                      \n");
                varname.Append("                INNER JOIN Rol as r ON v.Rol = r.ID         \n");
                varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN  \n");
                varname.Append("     left outer join Branch B on B.ID = r.EN  \n");
                varname.Append("                WHERE poi.PO = (select top 1 po from PO WHERE Vendor = '" + _objPO.Vendor + "' order by PO)   \n");
                if (_objPO.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
                }
                varname.Append("                ORDER by poi.line       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOItemByPO(PO _objPO)
        {
            try
            {
                //StringBuilder varname = new StringBuilder();
                //varname.Append("    select poi.PO, poi.Line, poi.Quan, poi.fDesc, poi.Price, poi.Amount, poi.Job, poi.Phase, poi.due,poi.Inv,J.fDesc AS JobName,Loc.Tag As LocationName ,   \n");
                //varname.Append("    poi.Amount as Ordered,         \n");
                //varname.Append("    isnull(poi.Selected,0.00) as PrvIn,                      \n");
                //varname.Append("    isnull(poi.Balance,poi.Amount) as Outstanding,                \n");
                //varname.Append("    0.00 as Received,                   \n");
                //varname.Append("    poi.Quan as OrderedQuan,            \n");
                //varname.Append("    isnull(poi.SelectedQuan,0.00) as PrvInQuan,      \n");
                //varname.Append("    isnull(poi.BalanceQuan,poi.Quan) as OutstandQuan,    \n");
                //varname.Append("    0.00 as ReceivedQuan,0 As IsReceiveIssued,               \n");
                //varname.Append("    poi.Inv, poi.GL, poi.Freight, poi.Rquan, poi.Billed, poi.Ticket,poi.WarehouseID,poi.LocationID ,   \n");



                //varname.Append("    isNULL((SELECT top 1  1 FROM INV WHERE ID = (poi.Inv)and type = 0),0) IsItemsExistsInInventory  ,                 \n");

                //varname.Append("    ( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW inner join Warehouse AS Wh on Wh.ID = INW.WarehouseID   where  INW.InvID=poi.Inv  and    INW.WareHouseID=poi.WarehouseID) As WarehouseName  ,             \n");

                //varname.Append("    (Select top 1 Name from WHLoc WH  where WH.WareHouseID = poi.WarehouseID and id = poi.LocationID) As WarehouseLoc   ,    \n");
                //varname.Append("     (                 \n");
                //varname.Append("   SELECT  top 1                 \n");
                //varname.Append(" isnull(bt.Type, '') as Phase                 \n");
                //varname.Append(" FROM POItem as ppp                 \n");
                //varname.Append(" LEFT JOIN JobTItem as jt ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                 \n");
                //varname.Append(" INNER JOIN BOM as b ON b.JobTItemID = jt.ID                 \n");
                //varname.Append(" LEFT JOIN Inv as i on i.ID = ppp.Inv and b.matitem = i.id                 \n");
                //varname.Append(" inner join BOMT bt on bt.ID = b.Type                 \n");
                //varname.Append(" WHERE ppp.PO = poi.PO and ppp.Line = poi.Line  ) as IsInventoryCode                 \n");



                //varname.Append("    FROM POItem as poi LEFT JOIN PO as p ON poi.PO = p.PO   \n");
                //varname.Append("    left outer JOIN Job as J ON poi.Job = J.ID   \n");
                //varname.Append("    left outer JOIN Loc as Loc ON Loc.Loc = J.Loc   \n");
                //varname.Append("             INNER JOIN Vendor as v    ON p.Vendor = v.ID                      \n");
                //varname.Append("                INNER JOIN Rol as r ON v.Rol = r.ID         \n");
                //if (_objPO.EN == 1)
                //{
                //    varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN  \n");
                //}
                //varname.Append("     left outer join Branch B on B.ID = r.EN  \n");
                //varname.Append("        WHERE poi.PO = '" + _objPO.POID + "'    \n");
                //if (_objPO.EN == 1)
                //{
                //    varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
                //}
                ////varname.Append("        AND (poi.Balance <> '0' AND poi.BalanceQuan <> '0')   \n");  by Ravinder for price 0.00 check
                //varname.Append("        ORDER by poi.line                       \n");
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "POID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.EN
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };

                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPOItemByPO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOStatusById(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE PO SET Amount = '" + _objPO.Amount + "', fDesc = '" + _objPO.fDesc + "', Status = '" + _objPO.Status + "' WHERE PO ='" + _objPO.POID + "' ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAddPOTerms(PO _objPO)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, " SELECT t.* FROM [T&C] AS t INNER JOIN tblPages AS p ON t.tblPageID = p.ID WHERE p.PageName='Add/Edit PO' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsBillExistForInsert(PJ _objPJ)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJ WHERE Ref='" + _objPJ.Ref + "' AND Vendor='" + _objPJ.Vendor + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsBillExistForEdit(PJ _objPJ)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJ WHERE Ref='" + _objPJ.Ref + "' AND Vendor='" + _objPJ.Vendor + "' AND ID<>'" + _objPJ.ID + "')THEN 1  ELSE 0  END AS BIT)"));
                // return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJ WHERE Ref='" + _objPJ.Ref + "' AND Vendor<>'" + _objPJ.Vendor + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsBillRecurrExistForInsert(PJ _objPJ)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJRecurr WHERE Ref='" + _objPJ.Ref + "' AND Vendor='" + _objPJ.Vendor + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsBillRecurrExistForEdit(PJ _objPJ)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJRecurr WHERE Ref='" + _objPJ.Ref + "' AND Vendor='" + _objPJ.Vendor + "' AND ID<>'" + _objPJ.ID + "')THEN 1  ELSE 0  END AS BIT)"));
                // return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPJ.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM PJ WHERE Ref='" + _objPJ.Ref + "' AND Vendor<>'" + _objPJ.Vendor + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxReceivePOId(PO _objPO)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, "SELECT isnull(max(ID),0) +1 as ID FROM ReceivePO"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxReceivePOId(string ConnectionString, GetMaxReceivePOIdParam _GetMaxReceivePOIdParam)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT isnull(max(ID),0) +1 as ID FROM ReceivePO"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddReceivePO(PO _objPO)
        {
            try
            {
                string query = "SET IDENTITY_INSERT [ReceivePO] ON ";
                query += " INSERT INTO ReceivePO(ID, PO, Ref, WB, Comments, Amount, fDate,Batch) ";
                query += " VALUES(@ID, @PO, @Ref, @WB, @Comments, @Amount, @fDate,@Batch) ";
                query += " SET IDENTITY_INSERT [ReceivePO] OFF ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objPO.RID));
                parameters.Add(new SqlParameter("@PO", _objPO.POID));
                parameters.Add(new SqlParameter("@Ref", _objPO.Ref));
                parameters.Add(new SqlParameter("@WB", _objPO.WB));
                parameters.Add(new SqlParameter("@Comments", _objPO.Comments));
                parameters.Add(new SqlParameter("@Amount", _objPO.Amount));
                parameters.Add(new SqlParameter("@fDate", _objPO.fDate));
                parameters.Add(new SqlParameter("@Batch", _objPO.BatchID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOStatus(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE PO SET Status = '" + _objPO.Status + "' WHERE PO ='" + _objPO.POID + "' ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOStatus(string ConnectionString, UpdatePOStatusParam _UpdatePOStatusParam)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE PO SET Status = '" + _UpdatePOStatusParam.Status + "' WHERE PO ='" + _UpdatePOStatusParam.POID + "' ");
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOItemBalance(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET Selected='" + _objPO.Selected + "', Balance='" + _objPO.Balance + "' WHERE PO='" + _objPO.POID + "' AND Line='" + _objPO.Line + "'  ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePOItemBalance(string ConnectionString, UpdatePOItemBalanceParam _UpdatePOItemBalanceParam)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET Selected='" + _UpdatePOItemBalanceParam.Selected + "', Balance='" + _UpdatePOItemBalanceParam.Balance + "' WHERE PO='" + _UpdatePOItemBalanceParam.POID + "' AND Line='" + _UpdatePOItemBalanceParam.Line + "'  ");
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddReceiveInventoryWHTrans(InventoryWHTrans _obj)
        {
            try
            {


                var para = new SqlParameter[13];
                para[0] = new SqlParameter
                {
                    ParameterName = "InvID",
                    SqlDbType = SqlDbType.Int,
                    Value = _obj.InvID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "WarehouseID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _obj.WarehouseID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "LocationID",
                    SqlDbType = SqlDbType.Int,
                    Value = _obj.LocationID
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Hand",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _obj.Hand
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Balance",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _obj.Balance
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "fOrder",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _obj.fOrder
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Committed",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _obj.Committed
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Available",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _obj.Available
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Screen",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _obj.Screen
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "ScreenID",
                    SqlDbType = SqlDbType.Int,
                    Value = _obj.ScreenID
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Mode",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _obj.Mode
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "TransType",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _obj.TransType
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Batch",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _obj.Batch
                };
                SqlHelper.ExecuteDataset(_obj.ConnConfig, CommandType.StoredProcedure, "spInsertInInvWHTrans", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddReceiveInventoryWHTrans(AddReceiveInventoryWHTransParam _AddReceiveInventoryWHTrans, string ConnectionString)
        {
            try
            {


                var para = new SqlParameter[13];
                para[0] = new SqlParameter
                {
                    ParameterName = "InvID",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddReceiveInventoryWHTrans.InvID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "WarehouseID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddReceiveInventoryWHTrans.WarehouseID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "LocationID",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddReceiveInventoryWHTrans.LocationID
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Hand",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddReceiveInventoryWHTrans.Hand
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Balance",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddReceiveInventoryWHTrans.Balance
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "fOrder",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddReceiveInventoryWHTrans.fOrder
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Committed",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddReceiveInventoryWHTrans.Committed
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Available",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddReceiveInventoryWHTrans.Available
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Screen",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddReceiveInventoryWHTrans.Screen
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "ScreenID",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddReceiveInventoryWHTrans.ScreenID
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Mode",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddReceiveInventoryWHTrans.Mode
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "TransType",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddReceiveInventoryWHTrans.TransType
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Batch",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddReceiveInventoryWHTrans.Batch
                };
                SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spInsertInInvWHTrans", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ReverseReceivePOInvetoryItem(int RPOID, string conString, string userid)
        {
            try
            {
                var para = new SqlParameter[2];
                para[0] = new SqlParameter
                {
                    ParameterName = "RPOID",
                    SqlDbType = SqlDbType.Int,
                    Value = RPOID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "UserId",
                    SqlDbType = SqlDbType.VarChar,
                    Value = userid
                };
                SqlHelper.ExecuteNonQuery(conString, CommandType.StoredProcedure, "spReverseRPOInvetoryItem", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddReceivePOItem(PO _objPO)
        {
            try
            {

                string query = " INSERT INTO RPOItem(ReceivePO, POLine, Quan, Amount,IsReceiveIssued) ";
                query += " VALUES (@ReceivePO, @POLine, @Quan, @Amount,@IsReceiveIssued) ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReceivePO", _objPO.ReceivePOId));
                parameters.Add(new SqlParameter("@POLine", _objPO.Line));
                parameters.Add(new SqlParameter("@Quan", _objPO.Quan));
                parameters.Add(new SqlParameter("@Amount", _objPO.Amount));
                parameters.Add(new SqlParameter("@IsReceiveIssued", _objPO.IsReceiveIssued));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddReceivePOItem(string ConnectionString, AddReceivePOItemParam _AddReceivePOItemParam)
        {
            try
            {

                string query = " INSERT INTO RPOItem(ReceivePO, POLine, Quan, Amount,IsReceiveIssued) ";
                query += " VALUES (@ReceivePO, @POLine, @Quan, @Amount,@IsReceiveIssued) ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReceivePO", _AddReceivePOItemParam.ReceivePOId));
                parameters.Add(new SqlParameter("@POLine", _AddReceivePOItemParam.Line));
                parameters.Add(new SqlParameter("@Quan", _AddReceivePOItemParam.Quan));
                parameters.Add(new SqlParameter("@Amount", _AddReceivePOItemParam.Amount));
                parameters.Add(new SqlParameter("@IsReceiveIssued", _AddReceivePOItemParam.IsReceiveIssued));

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllReceivePO(PO _objPO)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, " SELECT * FROM ReceivePO as r INNER JOIN PO as p ON r.PO=p.PO where r.ID = '" + _objPO.RID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePoById(PO _objPO)
        {
            try
            {
                //StringBuilder varname = new StringBuilder();
                //varname.Append("    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(r.Status,0) as Status, p.Due, p.ShipVia,           \n");
                //varname.Append("         p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,    \n");
                //varname.Append("         p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate, \n");
                //varname.Append("         ro.Address +', '+ CHAR(13)+ ro.City +', '+ ro.State+', '+ ro.Zip as Address    \n");
                //varname.Append("         FROM ReceivePO as r                             \n");
                //varname.Append("         INNER JOIN PO as p ON r.PO=p.PO                 \n");
                //varname.Append("         INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
                //varname.Append("         INNER JOIN Rol as ro ON v.Rol = ro.ID           \n");
                //varname.Append("         WHERE r.ID = '" + _objPO.RID + "'      \n");
                //varname.Append("    SELECT p.PO,p.Line, p.Quan, p.fDesc, p.Price, p.Job, p.Phase,    \n");
                //varname.Append("         p.Rquan, p.Billed, p.Ticket, p.Due, p.GL, p.Freight, p.Inv,J.fDesc As JobName,Loc.Tag As LocationName, \n");
                //varname.Append("         p.Amount as Ordered,                       \n");
                //varname.Append("         p.Selected as PrvIn,                       \n");
                //varname.Append("         p.Balance as Outstanding,                  \n");
                //varname.Append("         rp.Amount as Received,                     \n");
                //varname.Append("         p.fDesc as ItemName,         \n");
                //varname.Append("         p.Quan as OrderedQuan,                     \n");
                //varname.Append("         p.SelectedQuan as PrvInQuan,               \n");
                //varname.Append("         p.BalanceQuan as OutstandQuan,             \n");
                //varname.Append("         isnull(rp.Quan,0) as ReceivedQuan,p.WarehouseID,p.LocationID,                    \n");
                //varname.Append("         rp.POLine,                                 \n");
                //varname.Append("         rp.ReceivePO,rp.IsReceiveIssued ,                              \n");



                //varname.Append("    isNULL((SELECT top 1  1 FROM INV WHERE ID = (p.Inv)and type = 0),0) IsItemsExistsInInventory  ,                 \n");

                //varname.Append("    ( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW inner join Warehouse AS Wh on Wh.ID = INW.WarehouseID   where  INW.InvID=p.Inv  and    INW.WareHouseID=p.WarehouseID) As WarehouseName  ,             \n");
                //varname.Append("    (Select top 1 Name from WHLoc WH  where WH.WareHouseID = p.WarehouseID and id = p.LocationID) As WarehouseLoc   ,    \n");
                //varname.Append("     (                 \n");
                //varname.Append("   SELECT  top 1                 \n");
                //varname.Append(" isnull(bt.Type, '') as Phase                 \n");
                //varname.Append(" FROM POItem as ppp                 \n");
                //varname.Append(" LEFT JOIN JobTItem as jt ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                 \n");
                //varname.Append(" INNER JOIN BOM as b ON b.JobTItemID = jt.ID                 \n");
                //varname.Append(" LEFT JOIN Inv as i on i.ID = ppp.Inv and b.matitem = i.id                 \n");
                //varname.Append(" inner join BOMT bt on bt.ID = b.Type                 \n");
                //varname.Append(" WHERE ppp.PO = p.PO and ppp.Line = p.Line  ) as IsInventoryCode                 \n");




                //varname.Append("         FROM ReceivePO AS r          \n");
                //varname.Append("        RIGHT JOIN RPOItem AS rp on rp.ReceivePO = r.ID                 \n");
                //varname.Append("        INNER JOIN POItem AS p ON p.Line = rp.POLine                    \n");
                //varname.Append("        left outer JOIN Job AS J ON p.Job = J.ID                  \n");
                //varname.Append("        left outer JOIN LOC AS LOC ON LOC.Loc = J.Loc                  \n");
                //varname.Append("        WHERE r.ID = '" + _objPO.RID + "' and p.PO = r.PO                   \n");
                //return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "RPOID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.RID
                };

                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetRPOByID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet GetListReceivePO(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.PO, r.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(p.Status,0) as Status, p.Due, p.ShipVia,           \n");
                varname.Append("        case when isnull(r.Status,0) = 0 then 'Open' when isnull(r.status,0) = 1 then 'Closed' else '' End as StatusName,               \n");
                varname.Append("        p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,             \n");
                varname.Append("        p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate   \n");
                varname.Append("        FROM ReceivePO as r                             \n");
                varname.Append("        INNER JOIN PO as p ON r.PO=p.PO                 \n");
                varname.Append("        INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
                varname.Append("        INNER JOIN Rol as ro ON v.Rol = ro.ID    ORDER BY r.ID       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListReceivePOBySearch(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.PO, r.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(p.Status,0) as Status, p.Due, p.ShipVia,           \n");
                varname.Append("        case when isnull(r.Status,0) = 0 then 'Open' when isnull(r.status,0) = 1 then 'Closed' else '' End as StatusName,               \n");
                varname.Append("        p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,             \n");
                //varname.Append("        p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, case when (r.Amount)=0 then dbo.GetReceiveAmt(r.id) else r.Amount end  as ReceivedAmount,dbo.GetReceiveQuan(r.id) As Quan ,r.fDate as ReceiveDate,ro.EN, LTRIM(RTRIM(B.Name)) As Company   \n");
                varname.Append("        p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, ISNULL(r.Amount,0) as ReceivedAmount,dbo.GetReceiveQuan(r.id) As Quan ,r.fDate as ReceiveDate,ro.EN, LTRIM(RTRIM(B.Name)) As Company   \n");
                varname.Append("        FROM ReceivePO as r                             \n");
                varname.Append("        INNER JOIN PO as p ON r.PO=p.PO                 \n");
                varname.Append("        INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
                varname.Append("        INNER JOIN Rol as ro ON v.Rol = ro.ID          \n");
                if (_objPO.EN == 1)
                    varname.Append("     left outer join tblUserCo UC on UC.CompanyID = ro.EN  \n");
                varname.Append("     left outer join Branch B on B.ID = ro.EN  \n");
                //varname.Append("WHERE ");
                if (_objPO.ReceiveStartDate != string.Empty && _objPO.ReceiveEndDate != string.Empty)
                {
                    varname.Append("WHERE r.fDate >= '" + _objPO.ReceiveStartDate + "' and r.fDate <= '" + _objPO.ReceiveEndDate + "' \n");
                }
                if (_objPO.SearchBy != string.Empty && _objPO.SearchValue != string.Empty)
                {
                    if ((_objPO.SearchBy).ToLower() == "p.po" || (_objPO.SearchBy).ToLower() == "r.id" || (_objPO.SearchBy).ToLower() == "p.vendor")
                    {
                        varname.Append("   and " + _objPO.SearchBy + " = '" + _objPO.SearchValue + "'       \n");
                    }
                    else if ((_objPO.SearchBy).ToLower() == "ro.name")
                    {
                        varname.Append("   and " + _objPO.SearchBy + " LIKE '%" + _objPO.SearchValue + "%' \n");
                    }
                    else if ((_objPO.SearchBy).ToLower() == "r.status")
                    {
                        varname.Append("   and " + "isnull(" + _objPO.SearchBy + ",0) = '" + _objPO.SearchValue + "' \n");
                    }
                    else if ((_objPO.SearchBy).ToLower() == "p.inventorytype")
                    {
                        //varname.Append("   and " + "isnull(" + _objPO.SearchBy + ",0) = '" + _objPO.SearchValue + "' \n");
                        varname.Append("   and  p.po in (SELECT PO FROM POItem WHERE TypeID = 8 AND PO = p.PO) \n");
                    }


                }
                if (_objPO.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
                }
                varname.Append("    ORDER BY r.ID       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListReceivePOProjectBySearch(PO _objPO, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT p.PO, r.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(p.Status,0) as Status,pot.job As Project,pot.fdesc As ItemName,job.fdesc As ProjectType, Loc.Tag As LocationName , p.Due, p.ShipVia,           \n");
                varname.Append("    case when isnull(r.Status,0) = 0 then 'Open' when isnull(r.status,0) = 1 then 'Closed' else '' End as StatusName,               \n");
                varname.Append("    p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,             \n");
                varname.Append("    p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, case when (r.Amount)=0 then dbo.GetReceiveAmt(r.id) else r.Amount end  as ReceivedAmount, rpot.Quan As Quan ,r.fDate as ReceiveDate,ro.EN, LTRIM(RTRIM(B.Name)) As Company,isnull(pot.BalanceQuan,pot.Quan) as OutstandQuan   \n");
                varname.Append("FROM ReceivePO r                             \n");
                varname.Append("INNER JOIN PO p ON r.PO = p.PO                 \n");
                varname.Append("INNER JOIN Vendor v ON p.Vendor = v.ID       \n");
                varname.Append("INNER JOIN RPOItem rpot on rpot.ReceivePO = r.ID      \n");
                varname.Append("LEFT OUTER JOIN POItem pot on pot.PO =r.PO  AND  pot.Line = rpot.POLine       \n");
                varname.Append("LEFT OUTER JOIN Job job on job.ID = pot.Job       \n");
                varname.Append("LEFT OUTER JOIN Loc loc on loc.Loc = job.Loc      \n");
                varname.Append("INNER JOIN Rol ro ON v.Rol = ro.ID          \n");

                if (_objPO.EN == 1)
                {
                    varname.Append("LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = ro.EN  \n");
                }

                varname.Append("LEFT OUTER JOIN Branch B on B.ID = ro.EN  \n");

                varname.Append("WHERE 1 = 1 ");
                if (_objPO.ReceiveStartDate != string.Empty && _objPO.ReceiveEndDate != string.Empty)
                {
                    varname.Append("   AND r.fDate >= '" + _objPO.ReceiveStartDate + "' AND r.fDate <= '" + _objPO.ReceiveEndDate + "' \n");
                }

                if (_objPO.SearchBy != string.Empty && _objPO.SearchValue != string.Empty)
                {
                    if ((_objPO.SearchBy).ToLower() == "p.po" || (_objPO.SearchBy).ToLower() == "r.id" || (_objPO.SearchBy).ToLower() == "p.vendor")
                    {
                        varname.Append("   AND " + _objPO.SearchBy + " = '" + _objPO.SearchValue + "'       \n");
                    }
                    else if ((_objPO.SearchBy).ToLower() == "ro.name")
                    {
                        varname.Append("   AND " + _objPO.SearchBy + " LIKE '%" + _objPO.SearchValue + "%' \n");
                    }
                    else if ((_objPO.SearchBy).ToLower() == "r.status")
                    {
                        varname.Append("   AND " + "isnull(" + _objPO.SearchBy + ",0) = '" + _objPO.SearchValue + "' \n");
                    }
                }

                if (_objPO.EN == 1)
                {
                    varname.Append("    AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
                }

                if (filters.Count > 0)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            varname.Append("    AND CONVERT(varchar, r.ID) LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fDate")
                        {
                            varname.Append("    AND CONVERT(varchar, r.fDate, 1) LIKE '%" + filter.FilterValue + "%'  \n");
                        }
                        if (filter.FilterColumn == "Ref")
                        {
                            varname.Append("    AND r.Ref LIKE '%" + filter.FilterValue + "%'  \n");
                        }
                        if (filter.FilterColumn == "fDesc")
                        {
                            varname.Append("    AND p.fDesc LIKE '%" + filter.FilterValue + "%'  \n");
                        }
                        if (filter.FilterColumn == "VendorName")
                        {
                            varname.Append("    AND ro.Name LIKE '%" + filter.FilterValue + "%'  \n");
                        }
                        if (filter.FilterColumn == "StatusName")
                        {
                            varname.Append("    AND (CASE WHEN ISNULL(r.Status,0) = 0 THEN 'Open' WHEN r.Status = 1 THEN 'Closed' ELSE '' END) LIKE '%" + filter.FilterValue + "%'  \n");
                        }
                        if (filter.FilterColumn == "PO")
                        {
                            varname.Append("    AND CONVERT(varchar, r.PO) LIKE '%" + filter.FilterValue + "%'  \n");
                        }
                        if (filter.FilterColumn == "ReceivedAmount")
                        {
                            varname.Append("    AND r.Amount = " + filter.FilterValue + "  \n");
                        }
                    }
                }

                varname.Append("ORDER BY r.ID       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListReceivePOBySearchByID(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT p.PO, r.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(p.Status,0) as Status,pot.job As Project,pot.fdesc As ItemName,job.fdesc As ProjectType, Loc.Tag As LocationName ,p.Due, p.ShipVia,           \n");
                varname.Append("        case when isnull(r.Status,0) = 0 then 'Open' when isnull(r.status,0) = 1 then 'Closed' else '' End as StatusName,               \n");
                varname.Append("        p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,             \n");
                varname.Append("        p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, case when (r.Amount)=0 then dbo.GetReceiveAmt(r.id) else r.Amount end  as ReceivedAmount,rpot.Quan As Quan, r.fDate as ReceiveDate,ro.EN, LTRIM(RTRIM(B.Name)) As Company,isnull(pot.BalanceQuan,pot.Quan) as OutstandQuan   \n");
                varname.Append("        FROM ReceivePO as r                             \n");
                varname.Append("        INNER JOIN PO as p ON r.PO=p.PO                 \n");
                varname.Append("        INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
                varname.Append("        inner join rpoitem rpot on rpot.receivepo=r.id      \n");
                varname.Append("        left outer join POItem pot on pot.po=r.po  and  pot.line=rpot.poline       \n");
                varname.Append("        left outer join job job on job.ID=pot.job       \n");
                varname.Append("        left outer join loc loc on loc.loc=job.loc      \n");
                varname.Append("        INNER JOIN Rol as ro ON v.Rol = ro.ID          \n");
                if (_objPO.EN == 1)
                    varname.Append("     left outer join tblUserCo UC on UC.CompanyID = ro.EN  \n");
                varname.Append("     left outer join Branch B on B.ID = ro.EN  \n");
                //varname.Append("WHERE ");

                if (_objPO.SearchBy != string.Empty && _objPO.SearchValue != string.Empty)
                {
                    if ((_objPO.SearchBy).ToLower() == "p.po" || (_objPO.SearchBy).ToLower() == "r.id" || (_objPO.SearchBy).ToLower() == "p.vendor")
                    {
                        varname.Append("   WHERE " + _objPO.SearchBy + " = '" + _objPO.SearchValue + "'       \n");
                    }
                    else if ((_objPO.SearchBy).ToLower() == "ro.name")
                    {
                        varname.Append("   and " + _objPO.SearchBy + " LIKE '%" + _objPO.SearchValue + "%' \n");
                    }
                    else if ((_objPO.SearchBy).ToLower() == "r.status")
                    {
                        varname.Append("   and " + "isnull(" + _objPO.SearchBy + ",0) = '" + _objPO.SearchValue + "' \n");
                    }


                }
                if (_objPO.EN == 1)
                {
                    varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
                }
                varname.Append("    ORDER BY r.ID       \n");
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllPOByDue(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.EN
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "VendorID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Vendor
                };
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetOpenPOforRPO", para).Tables[0];


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetAllPOByDue(PO _objPO)
        //{
        //    try
        //    {
        //        //StringBuilder varname = new StringBuilder();
        //        //varname.Append("    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, p.Status, p.Due, p.ShipVia,           \n");
        //        //varname.Append("        p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,         \n");
        //        //varname.Append("        p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate   \n");
        //        //varname.Append("        FROM ReceivePO as r                             \n");
        //        //varname.Append("        INNER JOIN PO as p ON r.PO=p.PO                 \n");
        //        //varname.Append("        INNER JOIN Vendor as v ON p.Vendor = v.ID       \n");
        //        //varname.Append("        INNER JOIN Rol as ro ON v.Rol = ro.ID           \n");
        //        //varname.Append("        WHERE (p.Status=0 OR p.Status=3 OR p.Status=4)  ORDER BY p.Due \n");
        //        StringBuilder varname = new StringBuilder();
        //        varname.Append("        SELECT p.*, r.Name as VendorName,v.Type VendorType, r.Address +', '+ CHAR(13)+ r.City +', '+ r.State+', '+ r.Zip as Address                \n");
        //        varname.Append("            FROM PO as p INNER JOIN Vendor as v         \n");
        //        varname.Append("                ON p.Vendor = v.ID                      \n");
        //        varname.Append("                INNER JOIN Rol as r ON v.Rol = r.ID             \n");
        //        varname.Append("     left outer join tblUserCo UC on UC.CompanyID = r.EN  \n");
        //        varname.Append("     left outer join Branch B on B.ID = r.EN  \n");
        //        varname.Append("                WHERE (p.Status=0 OR p.Status=3 OR p.Status=4)  \n");
        //        if (_objPO.EN == 1)
        //        {
        //            varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
        //        }
        //        return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void UpdatePOItemQuan(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET SelectedQuan ='" + _objPO.SelectedQuan + "', BalanceQuan ='" + _objPO.BalanceQuan + "' WHERE PO='" + _objPO.POID + "' AND Line='" + _objPO.Line + "'  ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOItemQuan(string ConnectionString, UpdatePOItemQuanParam _UpdatePOItemQuanParam)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET SelectedQuan ='" + _UpdatePOItemQuanParam.SelectedQuan + "', BalanceQuan ='" + _UpdatePOItemQuanParam.BalanceQuan + "' WHERE PO='" + _UpdatePOItemQuanParam.POID + "' AND Line='" + _UpdatePOItemQuanParam.Line + "'  ");
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOItemWarehouseLocation(PO _objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET WarehouseID ='" + _objPO.WarehouseID + "', LocationID ='" + _objPO.LocationID + "' WHERE PO='" + _objPO.POID + "' AND Line='" + _objPO.Line + "'  ");
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePOItemWarehouseLocation(UpdatePOItemWarehouseLocationParam _UpdatePOItemWarehouseLocationParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE POItem SET WarehouseID ='" + _UpdatePOItemWarehouseLocationParam.WarehouseID + "', LocationID ='" + _UpdatePOItemWarehouseLocationParam.LocationID + "' WHERE PO='" + _UpdatePOItemWarehouseLocationParam.POID + "' AND Line='" + _UpdatePOItemWarehouseLocationParam.Line + "'  ");
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsClosedPO(PO _objPO)
        {
            StringBuilder varname = new StringBuilder();
            varname.Append("    SELECT CAST(CASE WHEN EXISTS(SELECT sum(selected) as total FROM POItem poi right join PO p ON p.PO = poi.PO WHERE p.Amount = (Select sum(amount) from POItem where po = '" + _objPO.POID + "') and poi.PO = '" + _objPO.POID + "') THEN 1  ELSE 0  END AS BIT) ");
            return _objPO.IsClosed = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPO.ConnConfig, CommandType.Text, varname.ToString()));
        }
        public bool IsExistRPOForInsert(PO objPO)
        {
            try
            {
                //return Convert.ToBoolean(SqlHelper.ExecuteScalar(objPO.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT TOP 1 1 FROM ReceivePO WHERE Ref='" + objPO.Ref + "')THEN 1  ELSE 0  END AS BIT)"));
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objPO.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT Top 1 p.Vendor From PO p inner Join ReceivePO rp on p.PO = rp.PO WHERE rp.Ref ='" + objPO.Ref + "' AND p.Vendor ='" + objPO.Vendor + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOList(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" SELECT TOP 100 p.PO, p.Vendor, r.Name AS VendorName, p.Status, p.Amount,v.Type VendorType, isnull((select sum(Amount) from PJ where PO = p.po),0) as ReceivedAmount,ISNULL(p.POReceiveBy,9) AS POReceiveBy  \n");
                varname.Append(" FROM PO AS p INNER JOIN Vendor AS v ON p.Vendor = v.ID \n");
                varname.Append("    INNER JOIN Rol AS r ON r.ID = v.Rol                 \n");
                varname.Append("    WHERE p.Vendor <> 0 AND p.Status <> 1 AND  p.Status <> 2          \n");
                if (objPO.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = '" + objPO.Vendor + "'   \n");
                }
                if (objPO.POID > 0)
                {
                    varname.Append("    AND p.PO like '%" + objPO.POID + "%'    \n");
                }
                varname.Append("    ORDER BY p.PO           \n");
                return SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpsqList(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select jobtitem.Code,jobtitem.fDesc, jobtitem.Line from jobtitem  jobtitem  \n");
                varname.Append("    inner join bom bom on bom.jobtitemID=jobtitem.ID                \n");
                varname.Append("        WHERE  jobtitem.job= '" + objPO.jobID + "'   and bom.Matitem='" + objPO.ItemID + "'     \n");
                return SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePOList(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT r.ID, CAST(r.ID as varchar(40)) as Value, r.Amount as ReceivedAmount,CONVERT(varchar, r.fDate , 101) As ReceiveDate, r.Ref         \n");
                varname.Append("    FROM ReceivePO As r INNER JOIN PO AS p ON p.PO = r.PO                \n");
                varname.Append("        WHERE isnull(r.Status,0) <> 1     \n");
                if (objPO.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = '" + objPO.Vendor + "'   \n");
                }
                if (objPO.POID > 0)
                {
                    varname.Append("    AND r.PO like '%" + objPO.POID + "%'   \n");
                }
                varname.Append("        ORDER BY r.ID     \n");
                return SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReceivePOListSearch(PO objPO)
        {
            try
            {
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objPO.Vendor
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = objPO.POID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "SearchText",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPO.SearchValue
                };
                return SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.StoredProcedure, "spGetReceivePOListSearch", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePOList(string ConnectionString, GetReceivePOListParam _GetReceivePOListParam)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT r.ID, CAST(r.ID as varchar(40)) as Value, r.Amount as ReceivedAmount,CONVERT(varchar, r.fDate , 101) As ReceiveDate          \n");
                varname.Append("    FROM ReceivePO As r INNER JOIN PO AS p ON p.PO = r.PO                \n");
                varname.Append("        WHERE isnull(r.Status,0) <> 1     \n");
                if (_GetReceivePOListParam.Vendor > 0)
                {
                    varname.Append("    AND p.Vendor = '" + _GetReceivePOListParam.Vendor + "'   \n");
                }
                if (_GetReceivePOListParam.POID > 0)
                {
                    varname.Append("    AND r.PO like '%" + _GetReceivePOListParam.POID + "%'   \n");
                }
                varname.Append("        ORDER BY r.ID     \n");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOReceivePOById(PO objPO)  // Get to fill in bill screen
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "RID";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objPO.RID;

                return objPO.Ds = SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.StoredProcedure, "spGetReceivePoById", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOReceivePOById(string ConnectionString, GetPOReceivePOByIdParam _GetPOReceivePOByIdParam)  // Get to fill in bill screen
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "RID";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _GetPOReceivePOByIdParam.RID;

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetReceivePoById", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public String GetClosePOCheck(PO objPO)  // Get to fill in bill screen
        {
            var para = new SqlParameter[2];
            String Retval = "";
            try
            {


                para[0] = new SqlParameter
                {
                    ParameterName = "ReceiptNo",
                    SqlDbType = SqlDbType.Int,
                    Value = objPO.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = objPO.POID
                };

                return Retval = Convert.ToString(SqlHelper.ExecuteScalar(objPO.ConnConfig, CommandType.StoredProcedure, "spClosePOCheck", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetClosePOCheck(string ConnectionString, GetClosePOCheckParam _GetClosePOCheckParam)  // Get to fill in bill screen
        {
            var para = new SqlParameter[2];
            String Retval = "";
            try
            {


                para[0] = new SqlParameter
                {
                    ParameterName = "ReceiptNo",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetClosePOCheckParam.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetClosePOCheckParam.POID
                };

                return Retval = Convert.ToString(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "spClosePOCheck", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ProcessRecurBill(PJ objPJ)
        {
            SqlParameter paraRef = new SqlParameter();
            paraRef.ParameterName = "RefId";
            paraRef.SqlDbType = SqlDbType.Int;
            paraRef.Value = objPJ.ID;

            SqlParameter paraReturn = new SqlParameter();
            paraReturn.ParameterName = "returnval";
            paraReturn.SqlDbType = SqlDbType.Int;
            paraReturn.Direction = ParameterDirection.ReturnValue;

            try
            {
                //SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.StoredProcedure, "spProcessRecurBill", paraRef, paraReturn);
                return Convert.ToInt32(paraReturn.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ProcessRecurBill(ProcessRecurBillParam _ProcessRecurBillParam, string ConnectionString)
        {
            SqlParameter paraRef = new SqlParameter();
            paraRef.ParameterName = "RefId";
            paraRef.SqlDbType = SqlDbType.Int;
            paraRef.Value = _ProcessRecurBillParam.ID;

            SqlParameter paraReturn = new SqlParameter();
            paraReturn.ParameterName = "returnval";
            paraReturn.SqlDbType = SqlDbType.Int;
            paraReturn.Direction = ParameterDirection.ReturnValue;

            try
            {
                //SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spProcessRecurBill", paraRef, paraReturn);
                return Convert.ToInt32(paraReturn.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ProcessRecurCheck(CD objCD)
        {

            var para = new SqlParameter[3];
            int Retval = 0;
            try
            {


                para[0] = new SqlParameter
                {
                    ParameterName = "RefId",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCD.MOMUSer
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                return Retval = Convert.ToInt32(SqlHelper.ExecuteScalar(objCD.ConnConfig, CommandType.StoredProcedure, "spProcessRecurCheck", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public int ProcessRecurCheck(ProcessRecurCheckParam objProcessRecurCheckParam, string ConnectionString)
        {

            var para = new SqlParameter[3];
            int Retval = 0;
            try
            {


                para[0] = new SqlParameter
                {
                    ParameterName = "RefId",
                    SqlDbType = SqlDbType.Int,
                    Value = objProcessRecurCheckParam.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objProcessRecurCheckParam.MOMUSer
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                return Retval = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "spProcessRecurCheck", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public string UpdateRecurrBills(PJ objPJ)
        {
            try
            {
                var para = new SqlParameter[31];

                para[0] = new SqlParameter
                {
                    ParameterName = "APBillslineItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Vendor
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.PostDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.Due
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Ref
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.fDesc
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Terms
                };
                if (objPJ.PO > 0)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.PO
                    };
                }
                if (objPJ.ReceivePo > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.ReceivePo
                    };
                }
                para[10] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Status
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.Disc
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.ReturnValue
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.MOMUSer
                };
                if (objPJ.IfPaid > 0)
                {
                    para[16] = new SqlParameter
                    {
                        ParameterName = "IfPaid",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.IfPaid
                    };
                }
                para[17] = new SqlParameter
                {
                    ParameterName = "@Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Frequency
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.ID
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "@IsRecur",
                    SqlDbType = SqlDbType.Bit,
                    Value = objPJ.IsRecurring
                };

                para[20] = new SqlParameter
                {
                    ParameterName = "@PJSTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STax
                };
                //}


                para[21] = new SqlParameter
                {
                    ParameterName = "@PJSTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.STaxName
                };


                //if (objPJ.STaxGL > 0)
                //{
                para[22] = new SqlParameter
                {
                    ParameterName = "@PJSTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.STaxGL
                };
                //}
                //if (objPJ.STaxRate > 0)
                //{
                para[23] = new SqlParameter
                {
                    ParameterName = "@PJSTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STaxRate
                };
                //}
                //if (objPJ.UTax > 0)
                //{
                para[24] = new SqlParameter
                {
                    ParameterName = "@PJUTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTax
                };
                //}


                para[25] = new SqlParameter
                {
                    ParameterName = "@PJUTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.UTaxName
                };

                //if (objPJ.UTaxGL > 0)
                //{
                para[26] = new SqlParameter
                {
                    ParameterName = "@PJUTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UTaxGL
                };
                //}
                //if (objPJ.UTaxRate > 0)
                //{
                para[27] = new SqlParameter
                {
                    ParameterName = "@PJUTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTaxRate
                };
                //}
                para[28] = new SqlParameter
                {
                    ParameterName = "@PJGST",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GST
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@PJGSTRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTRate
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@PJGSTGL",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTGL
                };

                SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spUpdateRecurrBills", para);




                return (para[14].Value).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateRecurrBills(UpdateRecurrBillsParam _UpdateRecurrBillsParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[31];

                para[0] = new SqlParameter
                {
                    ParameterName = "APBillslineItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = _UpdateRecurrBillsParam.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateRecurrBillsParam.Vendor
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateRecurrBillsParam.fDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateRecurrBillsParam.PostDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateRecurrBillsParam.Due
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateRecurrBillsParam.Ref
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateRecurrBillsParam.fDesc
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateRecurrBillsParam.Terms
                };
                if (_UpdateRecurrBillsParam.PO > 0)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = _UpdateRecurrBillsParam.PO
                    };
                }
                if (_UpdateRecurrBillsParam.ReceivePo > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = _UpdateRecurrBillsParam.ReceivePo
                    };
                }
                para[10] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _UpdateRecurrBillsParam.Status
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.Disc
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateRecurrBillsParam.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateRecurrBillsParam.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.ReturnValue
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateRecurrBillsParam.MOMUSer
                };
                if (_UpdateRecurrBillsParam.IfPaid > 0)
                {
                    para[16] = new SqlParameter
                    {
                        ParameterName = "IfPaid",
                        SqlDbType = SqlDbType.Int,
                        Value = _UpdateRecurrBillsParam.IfPaid
                    };
                }
                para[17] = new SqlParameter
                {
                    ParameterName = "@Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateRecurrBillsParam.Frequency
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateRecurrBillsParam.ID
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "@IsRecur",
                    SqlDbType = SqlDbType.Bit,
                    Value = _UpdateRecurrBillsParam.IsRecurring
                };
                para[20] = new SqlParameter
                {
                    ParameterName = "@PJSTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.STax
                };
                //}


                para[21] = new SqlParameter
                {
                    ParameterName = "@PJSTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateRecurrBillsParam.STaxName
                };


                //if (objPJ.STaxGL > 0)
                //{
                para[22] = new SqlParameter
                {
                    ParameterName = "@PJSTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateRecurrBillsParam.STaxGL
                };
                //}
                //if (objPJ.STaxRate > 0)
                //{
                para[23] = new SqlParameter
                {
                    ParameterName = "@PJSTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.STaxRate
                };
                //}
                //if (objPJ.UTax > 0)
                //{
                para[24] = new SqlParameter
                {
                    ParameterName = "@PJUTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.UTax
                };
                //}


                para[25] = new SqlParameter
                {
                    ParameterName = "@PJUTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateRecurrBillsParam.UTaxName
                };

                //if (objPJ.UTaxGL > 0)
                //{
                para[26] = new SqlParameter
                {
                    ParameterName = "@PJUTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateRecurrBillsParam.UTaxGL
                };
                //}
                //if (objPJ.UTaxRate > 0)
                //{
                para[27] = new SqlParameter
                {
                    ParameterName = "@PJUTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.UTaxRate
                };
                //}
                para[28] = new SqlParameter
                {
                    ParameterName = "@PJGST",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.GST
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@PJGSTRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.GSTRate
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@PJGSTGL",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateRecurrBillsParam.GSTGL
                };

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateRecurrBills", para);

                return (para[14].Value).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AddRemitTaxBills(PJ objPJ)
        {
            try
            {
                var para = new SqlParameter[31];

                para[0] = new SqlParameter
                {
                    ParameterName = "APBillslineItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Vendor
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.PostDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.Due
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Ref
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.fDesc
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Terms
                };
                if (objPJ.PO > 0)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.PO
                    };
                }
                if (objPJ.ReceivePo > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.ReceivePo
                    };
                }
                para[10] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Status
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.Disc
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.ReturnValue
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.MOMUSer
                };
                if (objPJ.IfPaid > 0)
                {
                    para[16] = new SqlParameter
                    {
                        ParameterName = "IfPaid",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.IfPaid
                    };
                }
                para[17] = new SqlParameter
                {
                    ParameterName = "@Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Frequency
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@IsRecur",
                    SqlDbType = SqlDbType.Bit,
                    Value = objPJ.IsRecurring
                };
                //if (objPJ.STax > 0)
                //{
                para[19] = new SqlParameter
                {
                    ParameterName = "@PJSTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STax
                };
                //}


                para[20] = new SqlParameter
                {
                    ParameterName = "@PJSTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.STaxName
                };


                //if (objPJ.STaxGL > 0)
                //{
                para[21] = new SqlParameter
                {
                    ParameterName = "@PJSTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.STaxGL
                };
                //}
                //if (objPJ.STaxRate > 0)
                //{
                para[22] = new SqlParameter
                {
                    ParameterName = "@PJSTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STaxRate
                };
                //}
                //if (objPJ.UTax > 0)
                //{
                para[23] = new SqlParameter
                {
                    ParameterName = "@PJUTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTax
                };
                //}


                para[24] = new SqlParameter
                {
                    ParameterName = "@PJUTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.UTaxName
                };

                //if (objPJ.UTaxGL > 0)
                //{
                para[25] = new SqlParameter
                {
                    ParameterName = "@PJUTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UTaxGL
                };
                //}
                //if (objPJ.UTaxRate > 0)
                //{
                para[26] = new SqlParameter
                {
                    ParameterName = "@PJUTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTaxRate
                };
                //}
                para[27] = new SqlParameter
                {
                    ParameterName = "@PJGST",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GST
                };
                para[28] = new SqlParameter
                {
                    ParameterName = "@PJGSTRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTRate
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@PJGSTGL",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTGL
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@IsPOClose",
                    SqlDbType = SqlDbType.Bit,
                    Value = objPJ.IsPOClose
                };
                SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spAddRemitTaxBill", para);




                return (para[14].Value).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AddBills(PJ objPJ)
        {
            try
            {
                var para = new SqlParameter[31];

                para[0] = new SqlParameter
                {
                    ParameterName = "APBillslineItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Vendor
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.PostDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.Due
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Ref
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.fDesc
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Terms
                };
                if (objPJ.PO > 0)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.PO
                    };
                }
                if (objPJ.ReceivePo > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.ReceivePo
                    };
                }
                para[10] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Status
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.Disc
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.ReturnValue
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.MOMUSer
                };
                if (objPJ.IfPaid > 0)
                {
                    para[16] = new SqlParameter
                    {
                        ParameterName = "IfPaid",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.IfPaid
                    };
                }
                para[17] = new SqlParameter
                {
                    ParameterName = "@Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Frequency
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@IsRecur",
                    SqlDbType = SqlDbType.Bit,
                    Value = objPJ.IsRecurring
                };
                //if (objPJ.STax > 0)
                //{
                para[19] = new SqlParameter
                {
                    ParameterName = "@PJSTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STax
                };
                //}


                para[20] = new SqlParameter
                {
                    ParameterName = "@PJSTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.STaxName
                };


                //if (objPJ.STaxGL > 0)
                //{
                para[21] = new SqlParameter
                {
                    ParameterName = "@PJSTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.STaxGL
                };
                //}
                //if (objPJ.STaxRate > 0)
                //{
                para[22] = new SqlParameter
                {
                    ParameterName = "@PJSTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STaxRate
                };
                //}
                //if (objPJ.UTax > 0)
                //{
                para[23] = new SqlParameter
                {
                    ParameterName = "@PJUTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTax
                };
                //}


                para[24] = new SqlParameter
                {
                    ParameterName = "@PJUTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.UTaxName
                };

                //if (objPJ.UTaxGL > 0)
                //{
                para[25] = new SqlParameter
                {
                    ParameterName = "@PJUTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UTaxGL
                };
                //}
                //if (objPJ.UTaxRate > 0)
                //{
                para[26] = new SqlParameter
                {
                    ParameterName = "@PJUTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTaxRate
                };
                //}
                para[27] = new SqlParameter
                {
                    ParameterName = "@PJGST",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GST
                };
                para[28] = new SqlParameter
                {
                    ParameterName = "@PJGSTRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTRate
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@PJGSTGL",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTGL
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@IsPOClose",
                    SqlDbType = SqlDbType.Bit,
                    Value = objPJ.IsPOClose
                };
                SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spAddBills", para);




                return (para[14].Value).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddBills(AddBillsParam _AddBillsParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[31];

                para[0] = new SqlParameter
                {
                    ParameterName = "APBillslineItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = _AddBillsParam.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddBillsParam.Vendor
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddBillsParam.fDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddBillsParam.PostDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddBillsParam.Due
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddBillsParam.Ref
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddBillsParam.fDesc
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddBillsParam.Terms
                };
                if (_AddBillsParam.PO > 0)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = _AddBillsParam.PO
                    };
                }
                if (_AddBillsParam.ReceivePo > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = _AddBillsParam.ReceivePo
                    };
                }
                para[10] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _AddBillsParam.Status
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.Disc
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddBillsParam.Custom1
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddBillsParam.Custom2
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.ReturnValue
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddBillsParam.MOMUSer
                };
                if (_AddBillsParam.IfPaid > 0)
                {
                    para[16] = new SqlParameter
                    {
                        ParameterName = "IfPaid",
                        SqlDbType = SqlDbType.Int,
                        Value = _AddBillsParam.IfPaid
                    };
                }
                para[17] = new SqlParameter
                {
                    ParameterName = "@Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddBillsParam.Frequency
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@IsRecur",
                    SqlDbType = SqlDbType.Bit,
                    Value = _AddBillsParam.IsRecurring
                };
                //if (objPJ.STax > 0)
                //{
                para[19] = new SqlParameter
                {
                    ParameterName = "@PJSTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.STax
                };
                //}


                para[20] = new SqlParameter
                {
                    ParameterName = "@PJSTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddBillsParam.STaxName
                };


                //if (objPJ.STaxGL > 0)
                //{
                para[21] = new SqlParameter
                {
                    ParameterName = "@PJSTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddBillsParam.STaxGL
                };
                //}
                //if (objPJ.STaxRate > 0)
                //{
                para[22] = new SqlParameter
                {
                    ParameterName = "@PJSTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.STaxRate
                };
                //}
                //if (objPJ.UTax > 0)
                //{
                para[23] = new SqlParameter
                {
                    ParameterName = "@PJUTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.UTax
                };
                //}


                para[24] = new SqlParameter
                {
                    ParameterName = "@PJUTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddBillsParam.UTaxName
                };

                //if (objPJ.UTaxGL > 0)
                //{
                para[25] = new SqlParameter
                {
                    ParameterName = "@PJUTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddBillsParam.UTaxGL
                };
                //}
                //if (objPJ.UTaxRate > 0)
                //{
                para[26] = new SqlParameter
                {
                    ParameterName = "@PJUTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.UTaxRate
                };
                //}
                para[27] = new SqlParameter
                {
                    ParameterName = "@PJGST",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.GST
                };
                para[28] = new SqlParameter
                {
                    ParameterName = "@PJGSTRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.GSTRate
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@PJGSTGL",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddBillsParam.GSTGL
                };

                para[30] = new SqlParameter
                {
                    ParameterName = "@IsPOClose",
                    SqlDbType = SqlDbType.Bit,
                    Value = _AddBillsParam.IsPOClose
                };

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddBills", para);




                return (para[14].Value).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBills(PJ objPJ)
        {
            try
            {
                var para = new SqlParameter[32];

                para[0] = new SqlParameter
                {
                    ParameterName = "APBillslineItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.ID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Vendor
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.PostDate
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.Due
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Ref
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.fDesc
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Terms
                };
                if (objPJ.PO > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.PO
                    };
                }
                if (objPJ.ReceivePo > 0)
                {
                    para[10] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.ReceivePo
                    };
                }
                para[11] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPJ.Status
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.Disc
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom1
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Custom2
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Batch
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "TransId",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.TRID
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.MOMUSer
                };
                if (objPJ.IfPaid > 0)
                {
                    para[19] = new SqlParameter
                    {
                        ParameterName = "IfPaid",
                        SqlDbType = SqlDbType.Int,
                        Value = objPJ.IfPaid
                    };
                }
                para[20] = new SqlParameter
                {
                    ParameterName = "@PJSTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STax
                };
                //}


                para[21] = new SqlParameter
                {
                    ParameterName = "@PJSTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.STaxName
                };


                //if (objPJ.STaxGL > 0)
                //{
                para[22] = new SqlParameter
                {
                    ParameterName = "@PJSTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.STaxGL
                };
                //}
                //if (objPJ.STaxRate > 0)
                //{
                para[23] = new SqlParameter
                {
                    ParameterName = "@PJSTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.STaxRate
                };
                //}
                //if (objPJ.UTax > 0)
                //{
                para[24] = new SqlParameter
                {
                    ParameterName = "@PJUTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTax
                };
                //}


                para[25] = new SqlParameter
                {
                    ParameterName = "@PJUTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.UTaxName
                };

                //if (objPJ.UTaxGL > 0)
                //{
                para[26] = new SqlParameter
                {
                    ParameterName = "@PJUTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UTaxGL
                };
                //}
                //if (objPJ.UTaxRate > 0)
                //{
                para[27] = new SqlParameter
                {
                    ParameterName = "@PJUTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.UTaxRate
                };
                //}
                para[28] = new SqlParameter
                {
                    ParameterName = "@PJGST",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GST
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@PJGSTRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTRate
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@PJGSTGL",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPJ.GSTGL
                };
                para[31] = new SqlParameter
                {
                    ParameterName = "@IsPOClose",
                    SqlDbType = SqlDbType.Bit,
                    Value = objPJ.IsPOClose
                };


                SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spUpdateBills", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBills(UpdateBillsParam _UpdateBillsParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[32];

                para[0] = new SqlParameter
                {
                    ParameterName = "APBillslineItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = _UpdateBillsParam.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateBillsParam.ID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateBillsParam.Vendor
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateBillsParam.fDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "PostingDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateBillsParam.PostDate
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateBillsParam.Due
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsParam.Ref
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsParam.fDesc
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "DueIn",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _UpdateBillsParam.Terms
                };
                if (_UpdateBillsParam.PO > 0)
                {
                    para[9] = new SqlParameter
                    {
                        ParameterName = "PO",
                        SqlDbType = SqlDbType.Int,
                        Value = _UpdateBillsParam.PO
                    };
                }
                if (_UpdateBillsParam.ReceivePo > 0)
                {
                    para[10] = new SqlParameter
                    {
                        ParameterName = "ReceivePO",
                        SqlDbType = SqlDbType.Int,
                        Value = _UpdateBillsParam.ReceivePo
                    };
                }
                para[11] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _UpdateBillsParam.Status
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Disc",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.Disc
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsParam.Custom1
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsParam.Custom2
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateBillsParam.Batch
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "TransId",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateBillsParam.TRID
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsParam.MOMUSer
                };
                if (_UpdateBillsParam.IfPaid > 0)
                {
                    para[19] = new SqlParameter
                    {
                        ParameterName = "IfPaid",
                        SqlDbType = SqlDbType.Int,
                        Value = _UpdateBillsParam.IfPaid
                    };
                }
                para[20] = new SqlParameter
                {
                    ParameterName = "@PJSTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.STax
                };
                //}


                para[21] = new SqlParameter
                {
                    ParameterName = "@PJSTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsParam.STaxName
                };


                //if (objPJ.STaxGL > 0)
                //{
                para[22] = new SqlParameter
                {
                    ParameterName = "@PJSTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateBillsParam.STaxGL
                };
                //}
                //if (objPJ.STaxRate > 0)
                //{
                para[23] = new SqlParameter
                {
                    ParameterName = "@PJSTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.STaxRate
                };
                //}
                //if (objPJ.UTax > 0)
                //{
                para[24] = new SqlParameter
                {
                    ParameterName = "@PJUTax",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.UTax
                };
                //}


                para[25] = new SqlParameter
                {
                    ParameterName = "@PJUTaxName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsParam.UTaxName
                };

                //if (objPJ.UTaxGL > 0)
                //{
                para[26] = new SqlParameter
                {
                    ParameterName = "@PJUTaxGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateBillsParam.UTaxGL
                };
                //}
                //if (objPJ.UTaxRate > 0)
                //{
                para[27] = new SqlParameter
                {
                    ParameterName = "@PJUTaxRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.UTaxRate
                };
                //}
                para[28] = new SqlParameter
                {
                    ParameterName = "@PJGST",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.GST
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@PJGSTRate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.GSTRate
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@PJGSTGL",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateBillsParam.GSTGL
                };

                para[31] = new SqlParameter
                {
                    ParameterName = "@IsPOClose",
                    SqlDbType = SqlDbType.Bit,
                    Value = _UpdateBillsParam.IsPOClose
                };
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateBills", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePOStatus(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE ReceivePO SET Status = '" + objPO.Status + "' WHERE ID = '" + objPO.RID + "'     ");
                SqlHelper.ExecuteNonQuery(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePOStatus(string ConnectionString, UpdateReceivePOStatusParam _UpdateReceivePOStatusParam)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE ReceivePO SET Status = '" + _UpdateReceivePOStatusParam.Status + "' WHERE ID = '" + _UpdateReceivePOStatusParam.RID + "'     ");
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePOStatusByPOID(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE ReceivePO SET Status = '" + objPO.Status + "' WHERE PO = '" + objPO.POID + "'     ");
                SqlHelper.ExecuteNonQuery(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePOStatusByPOID(string ConnectionString, UpdateReceivePOStatusByPOIDParam _UpdateReceivePOStatusByPOIDParam)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    UPDATE ReceivePO SET Status = '" + _UpdateReceivePOStatusByPOIDParam.Status + "' WHERE PO = '" + _UpdateReceivePOStatusByPOIDParam.POID + "'     ");
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistPO(PO objPO)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objPO.ConnConfig, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT PO FROM PO WHERE PO = '" + objPO.POID + "') THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistPO(string ConnectionString, IsExistPOParam _IsExistPOParam)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT PO FROM PO WHERE PO = '" + _IsExistPOParam.POID + "') THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPOAjaxSearchSP(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[11];
                para[0] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.EN
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.EndDate
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.SearchBy
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.SearchValue
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "ApproveFrom",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO._ApprovePOStatus.ApproveFrom
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "ApproveTo",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO._ApprovePOStatus.ApproveTo
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO._ApprovePOStatus.Status
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "ApUserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO._ApprovePOStatus.UserID
                };
               

                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "GetAllPOAjaxSearch", para);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPOAjaxSearch(PO _objPO)
        {
            DataSet ds = new DataSet();
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select DISTINCT p.PO,CONVERT(VARCHAR(10),p.fDate,101)fDate,p.fDate as fDates, p.fDesc,p.Amount,p.Vendor,p.Status,p.Due,p.ShipVia,p.Terms,p.FOB,p.ShipTo,p.Approved,p.Custom1,p.Custom2,p.ApprovedBy,p.ReqBy,p.fBy,p.PORevision,p.CourrierAcct,p.POReasonCode, isnull(p.RequestedBy,'') as RequestedBy,                                            \n");
                varname.Append("        r.Name as VendorName, v.Acct,r.Address,                                \n");
                varname.Append("         (CASE isnull(p.Status,0) WHEN 0 THEN 'Open'            \n");
                varname.Append("            WHEN 1 THEN 'Closed'                                \n");
                varname.Append("            WHEN 2 THEN 'Void'                                  \n");
                varname.Append("            WHEN 3 THEN 'Partial-Quantity'                      \n");
                varname.Append("            WHEN 4 THEN 'Partial-Amount'                        \n");
                varname.Append("            WHEN 5 THEN 'Closed At Receive PO' END) AS StatusName,     \n");
                varname.Append("           isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( poitem.Job as nvarchar)     \n");
                varname.Append("            FROM poitem where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Projectnumber,     \n");
                varname.Append("            isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( Loc.ID as nvarchar)     \n");
                varname.Append("            FROM POItem inner join Job on POItem.Job=Job.ID     \n");
                varname.Append("            inner join Loc on Job.Loc=Loc.Loc where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Location,     \n");
                varname.Append("            isnull((SELECT  STUFF((SELECT  distinct ',' + CAST( poitem.fDesc as nvarchar)     \n");
                varname.Append("             FROM poitem where POItem.PO=p.PO FOR XML PATH('')) ,1,1,'') AS Txt ),'') as Part, r.EN, LTRIM(RTRIM(B.Name)) As Company,     \n");
                //varname.Append("            (select ISNULL((SELECT CASE Status WHEN 1 THEN 'Approved' WHEN 2 THEN 'Decline' END FROM vw_ApprovalStatus WHERE PO = p.PO),'Pending')) AS ApproveStatus       \n");
                varname.Append("            CASE vs.Status WHEN 1 THEN 'Approved' WHEN 3 THEN 'Reapprove' WHEN 2 THEN 'Decline' ELSE 'Pending' END AS ApproveStatus,        \n");

                varname.Append("            CASE WHEN isnull((SELECT  STUFF((SELECT  Top 1 ',' + CAST(poitem.Job as nvarchar)   \n");
                varname.Append("      FROM poitem where POItem.PO = p.PO ORDER BY Line FOR XML PATH('')), 1, 1, '') AS Txt ),'') = '0' THEN NULL ELSE      \n");
                varname.Append("            isnull((SELECT  STUFF((SELECT  Top 1 ',' + CAST(poitem.Job as nvarchar) FROM poitem where POItem.PO = p.PO ORDER BY Line FOR XML PATH('')), 1, 1, '') AS Txt ),'') END as Project, \n");

                varname.Append("            CASE WHEN isnull((SELECT  STUFF((SELECT  Top 1 ',' + CAST(poitem.Job as nvarchar)  \n");
                varname.Append("    FROM poitem where POItem.PO = p.PO ORDER BY Line FOR XML PATH('')), 1, 1, '') AS Txt ),'') = '0' THEN NULL  ELSE \n");


                 varname.Append("   isnull((SELECT  STUFF((SELECT  Top 1 ',' + CAST(JobType.Type as nvarchar)       \n");
                varname.Append("            FROM poitem INNER JOIN Job ON POItem.Job = Job.ID  LEFT JOIN JobType ON job.Type = JobType.ID where POItem.PO = p.PO  ORDER BY Line FOR XML PATH('')), 1, 1, '') AS Txt ),'') END as Department,       \n");

                varname.Append("            isnull((SELECT  STUFF((SELECT  Top 1 ',' + CAST(BOMT.Type as nvarchar)        \n");
                varname.Append("            FROM poitem Left JOIN BOMT ON POItem.TypeID = BOMT.ID where POItem.PO = p.PO  ORDER BY Line FOR XML PATH('')), 1, 1, '') AS Txt ),'') as Code,        \n");
                varname.Append("            ISNULL((SELECT SUM(ISNULL(Balance, 0)) FROM POItem WHERE POItem.PO = p.PO),0) AS OpenAmount  \n");

             

                varname.Append("        FROM PO as p                                            \n");
                varname.Append("            		left join Vendor as v on p.Vendor = v.ID    \n");
              

          

                varname.Append("                    left join Rol as r on v.Rol = r.ID          \n");
                if (_objPO.EN == 1)
                    varname.Append("                    left outer join tblUserCo UC on UC.CompanyID = r.EN          \n");
                varname.Append("                    left outer join Branch B on B.ID = r.EN          \n");
                varname.Append("                    LEFT outer JOIN vw_ApprovalStatus vs on P.PO = vs.PO        \n");
                varname.Append("                    where 1 = 1       \n");
                if (_objPO.StartDate != DateTime.MinValue && _objPO.EndDate != DateTime.MinValue)
                    varname.Append("                    AND  fDate>='" + _objPO.StartDate + "' and fDate<='" + _objPO.EndDate + "'            \n");
                //else if (_objPO.StartDate != DateTime.MinValue)
                //    varname.Append("                    where  fDate>='" + _objPO.StartDate + "'            \n");
                if (_objPO.EN == 1)
                    varname.Append("                    AND UC.IsSel =" + _objPO.EN + " and UC.UserID=" + _objPO.UserID + "   \n");
                if (_objPO.SearchBy != string.Empty && _objPO.SearchValue != string.Empty)
                {
                    if (_objPO.SearchBy == "p.PO" || _objPO.SearchBy == "p.Vendor")
                    {
                        varname.Append("   and " + _objPO.SearchBy + " = '" + _objPO.SearchValue + "'       \n");
                    }
                    else if (_objPO.SearchBy == "r.Name" || _objPO.SearchBy == "v.Acct")
                    {
                        varname.Append("   and " + _objPO.SearchBy + " LIKE '%" + _objPO.SearchValue + "%' \n");
                    }
                    else if (_objPO.SearchBy == "p.Status")
                    {
                        varname.Append("   and " + "isnull(" + _objPO.SearchBy + ",0) = '" + _objPO.SearchValue + "' \n");
                    }
                    else if (_objPO.SearchBy == "vs.Status")
                    {
                        varname.Append("                    AND  fDate>='" + _objPO._ApprovePOStatus.ApproveFrom + "' and fDate<='" + _objPO._ApprovePOStatus.ApproveTo + "'            \n");
                    }
                    else if (_objPO.SearchBy == "p.fBy" || _objPO.SearchBy == "p.RequestedBy")
                    {
                        varname.Append("   and " + _objPO.SearchBy + " LIKE '%" + _objPO.SearchValue + "%' \n");
                    }
                    else if (_objPO.SearchBy == "p.Custom1")
                    {
                        varname.Append("   and p.Custom1 LIKE '%" + _objPO.SearchValue + "%' \n");
                    }
                    else if (_objPO.SearchBy == "p.Custom2")
                    {
                        varname.Append("   and p.Custom2 LIKE '%" + _objPO.SearchValue + "%' \n");
                    }
                }

                if (_objPO._ApprovePOStatus.Status != null)
                {
                    if (_objPO._ApprovePOStatus.Status == 0)
                        varname.Append("                    AND vs.Status IS NULL      \n");
                    else
                        varname.Append("                    AND vs.Status = " + _objPO._ApprovePOStatus.Status + "    \n");
                }
                if (_objPO._ApprovePOStatus.UserID != null)
                    varname.Append("                    AND vs.UserID = " + _objPO._ApprovePOStatus.UserID + "      \n");
                if (_objPO._ApprovePOStatus.ApproveFrom != null)
                    varname.Append("                    OR vs.ApproveDate >= '" + _objPO._ApprovePOStatus.ApproveFrom + "'      \n");
                if (_objPO._ApprovePOStatus.ApproveTo != null)
                    varname.Append("                    OR vs.ApproveDate <= '" + _objPO._ApprovePOStatus.ApproveTo + "'      \n");
                varname.Append("                    order by p.PO       \n");
                //New -12-2-22


                //varname.Append("select r.id,v.Rol as verndorrol,r.Address,l.Tag from po left join  Vendor v on po.po=v.ID left join rol r on v.Rol=r.ID inner join loc l on l.Loc=r.ID");
                //varname.Append("r.id,v.Rol as verndorrol,r.Address,l.Tag  \n");
                //  varname.Append("from po left join  Vendor v on po.po=v.ID \n");
                //varname.Append("left join rol r on v.Rol=r.ID inner join loc l on l.Loc=r.ID\n");

                //varname.Append("    select p.po, p.Amount as poamt,pj.amount, case when pj.Amount is null then 'Open'   \n");
                //varname.Append("    			when (pj.amount = p.Amount) or (pj.amount > p.Amount) then 'Closed'     \n");
                //varname.Append("    			when pj.amount < p.Amount then 'Partial-Amount' end as Status           \n");
                //varname.Append("    , p.* from po as p left join pj as pj on p.PO = pj.PO                               \n");
                // return _objPO.Ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
                 return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());


                // _objPO.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return ds;
        }

        public DataSet GetPOItemInfoAjaxSearch(PO _objPO)
        {
            DataSet ds = new DataSet();
            try
            {
                _objPO.ConnConfig = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetPoitemInfo");



                _objPO.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetAPExpenses(Vendor objVendor)
        {
            try
            {
                var para = new SqlParameter[6];
                para[0] = new SqlParameter
                {
                    ParameterName = "vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objVendor.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objVendor.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objVendor.EndDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objVendor.SearchValue
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objVendor.SearchBy
                };

                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.StoredProcedure, "spGetAPExpenses", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAPExpenses(GetAPExpensesParam _GetAPExpensesParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[6];
                para[0] = new SqlParameter
                {
                    ParameterName = "vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAPExpensesParam.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _GetAPExpensesParam.StartDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _GetAPExpensesParam.EndDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetAPExpensesParam.SearchValue
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetAPExpensesParam.SearchBy
                };

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetAPExpenses", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePODue(PO objPO)
        {
            try
            {
                string query = "UPDATE PO SET Due=@Due WHERE PO=@PO";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Due", objPO.Due));
                parameters.Add(new SqlParameter("@PO", objPO.POID));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objPO.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RAHIL IMPLEMENTATION
        public DataSet GetAllBankCD(Bank _objBank)
        {
            try
            {
                //string sql = "Select CD.Ref ";
                //string sql = "Select CASE ";
                //sql = sql + " WHEN LEN(CD.Ref) = 1 ";
                //sql = sql + " THEN '00000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 2 ";
                //sql = sql + " THEN '0000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 3 ";
                //sql = sql + " THEN '000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 4 ";
                //sql = sql + " THEN '00000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 5 ";
                //sql = sql + " THEN '0000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 6 ";
                //sql = sql + " THEN '000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 7 ";
                //sql = sql + " THEN '00' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 8 ";
                //sql = sql + " THEN '0' +  CAST(CD.Ref AS VARCHAR(9))	 ";
                //sql = sql + " ELSE '000000000'  End As Ref ";
                //sql = sql + " , Rol.Name, Rol.Address,Rol.State,Rol.City,Rol.Zip, Bank.NBranch, Bank.NAcct, Bank.NRoute ";
                //sql = sql + " from CD, Bank, Rol where CD.Bank = Bank.ID and Bank.Rol=Rol.ID and Rol.Type = 2 and CD.Ref = '" + _objCD.Ref + "'";

                string sql = "Select Rol.Name, Rol.Address,Rol.State,Rol.City,Rol.Zip, Bank.NBranch, Bank.NAcct, Bank.NRoute ";
                sql = sql + " from Bank, Rol Where Bank.Rol = Rol.ID and  Bank.ID = '" + _objBank.ID + "'";
                return SqlHelper.ExecuteDataset(_objBank.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllBankCD(GetBankCDParam _GetBankCDParam, string ConnectionString)
        {
            try
            {
                //string sql = "Select CD.Ref ";
                //string sql = "Select CASE ";
                //sql = sql + " WHEN LEN(CD.Ref) = 1 ";
                //sql = sql + " THEN '00000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 2 ";
                //sql = sql + " THEN '0000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 3 ";
                //sql = sql + " THEN '000000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 4 ";
                //sql = sql + " THEN '00000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 5 ";
                //sql = sql + " THEN '0000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 6 ";
                //sql = sql + " THEN '000' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 7 ";
                //sql = sql + " THEN '00' +  CAST(CD.Ref AS VARCHAR(9)) ";
                //sql = sql + " WHEN LEN(CD.Ref) = 8 ";
                //sql = sql + " THEN '0' +  CAST(CD.Ref AS VARCHAR(9))	 ";
                //sql = sql + " ELSE '000000000'  End As Ref ";
                //sql = sql + " , Rol.Name, Rol.Address,Rol.State,Rol.City,Rol.Zip, Bank.NBranch, Bank.NAcct, Bank.NRoute ";
                //sql = sql + " from CD, Bank, Rol where CD.Bank = Bank.ID and Bank.Rol=Rol.ID and Rol.Type = 2 and CD.Ref = '" + _objCD.Ref + "'";

                string sql = "Select Rol.Name, Rol.Address,Rol.State,Rol.City,Rol.Zip, Bank.NBranch, Bank.NAcct, Bank.NRoute ";
                sql = sql + " from Bank, Rol Where Bank.Rol = Rol.ID and  Bank.ID = '" + _GetBankCDParam.ID + "'";
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCheckByPaidBill(PJ objPJ)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select p.PITR from trans t inner join paid p on p.TRID = t.ID       \n");
                varname.Append("        where t.Batch = '" + objPJ.Batch + "' and t.Type = 40             \n");

                return SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillTransDetails(PJ objPJ)
        {
            try
            {
                //return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, "spGetBillTransactions", objPJ.Batch);
                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, "spGetAPBillItem", objPJ.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillTransDetails(GetBillTransDetailsParam _GetBillTransDetailsParam, string ConnectionString)
        {
            try
            {
                //return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, "spGetBillTransactions", objPJ.Batch);
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetAPBillItem", _GetBillTransDetailsParam.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillRecurrTransactions(PJ objPJ)
        {
            try
            {
                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, "spGetBillRecurrTransactions", objPJ.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillRecurrTransactions(GetBillRecurrTransactionsParam _GetBillRecurrTransactionsParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetBillRecurrTransactions", _GetBillRecurrTransactionsParam.Batch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAPAgingByDate(PJ objPJ)
        {
            try
            {
                var param = new SqlParameter[7];
                param[0] = new SqlParameter()
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.EN
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UserID
                };
                param[3] = new SqlParameter()
                {
                    ParameterName = "Inclzero",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Frequency
                };

                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.StoredProcedure, "spGetAPAgingByDate", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAPAgingByBasedDate(PJ objPJ)
        {
            try
            {
                var param = new SqlParameter[4];
                param[0] = new SqlParameter()
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.EN
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UserID
                };
                param[3] = new SqlParameter()
                {
                    ParameterName = "Inclzero",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Frequency
                };

                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.StoredProcedure, "spGetAPAgingBasedDate", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAPAgingOver90DaysReport(PJ objPJ)
        {
            try
            {
                var param = new SqlParameter[4];
                param[0] = new SqlParameter()
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.EN
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UserID
                };
                param[3] = new SqlParameter()
                {
                    ParameterName = "Inclzero",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Frequency
                };

                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.StoredProcedure, "spGetAPAgingOver90DaysReport", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectLaborCostReport(PJ objPJ)
        {
            //Gable Build Check in
            try
            {
                var param = new SqlParameter[3];
                param[0] = new SqlParameter()
                {
                    ParameterName = "@Job",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ._dt
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "@startdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.StartDate
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "@endDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.EndDate
                };

                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.StoredProcedure, "spGetAllProjectTickets", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenTicketbyRoutesReport(Customer objCust)
        {
            //MEI Build Check in
            try
            {
                var param = new SqlParameter[3];
                param[0] = new SqlParameter()
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objCust.StartDate
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objCust.EndDate
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "@Routes",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCust.RouteSequence
                };

                return objCust.DsCustomer = SqlHelper.ExecuteDataset(objCust.ConnConfig, CommandType.StoredProcedure, "spGetOpenTicketByRoutes", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAPAging360ByDate(PJ objPJ)
        {
            try
            {
                var param = new SqlParameter[6];
                param[0] = new SqlParameter()
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.EN
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.UserID
                };

                return objPJ.Ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.StoredProcedure, "spGetAPAging360ByDate", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAPAging360ByDate(GetAPAging360ByDateParam _GetAPAging360ByDate, string ConnectionString)
        {
            try
            {
                var param = new SqlParameter[6];
                param[0] = new SqlParameter()
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _GetAPAging360ByDate.fDate
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAPAging360ByDate.EN
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAPAging360ByDate.UserID
                };

                return _GetAPAging360ByDate.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetAPAging360ByDate", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAPAgingByDate(GetAPAgingByDateParam _GetAPAgingByDateParam, string ConnectionString)
        {
            try
            {
                var param = new SqlParameter[6];
                param[0] = new SqlParameter()
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _GetAPAgingByDateParam.fDate
                };
                param[1] = new SqlParameter()
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAPAgingByDateParam.EN
                };
                param[2] = new SqlParameter()
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAPAgingByDateParam.UserID
                };

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetAPAgingByDate", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePJDate(PJ objPJ)
        {
            try
            {
                string query = "UPDATE PJ SET fDate = @PostingDate, IDate = @IDate WHERE ID=@PJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@PostingDate", objPJ.PostDate));
                parameters.Add(new SqlParameter("@IDate", objPJ.IDate));
                parameters.Add(new SqlParameter("@PJID", objPJ.ID));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateAPDates(PJ objPJ)
        {
            var para = new SqlParameter[7];

            para[0] = new SqlParameter
            {
                ParameterName = "PJID",
                SqlDbType = SqlDbType.Int,
                Value = objPJ.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "Batch",
                SqlDbType = SqlDbType.Int,
                Value = objPJ.Batch
            };
            para[2] = new SqlParameter
            {
                ParameterName = "TRID",
                SqlDbType = SqlDbType.Int,
                Value = objPJ.TRID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "PostDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPJ.PostDate
            };
            para[4] = new SqlParameter
            {
                ParameterName = "IDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPJ.IDate
            };
            para[5] = new SqlParameter
            {
                ParameterName = "Due",
                SqlDbType = SqlDbType.DateTime,
                Value = objPJ.Due
            };
            para[6] = new SqlParameter
            {
                ParameterName = "UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objPJ.MOMUSer
            };
            SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spUpdateBillDate", para);
        }

        public void UpdateAPDates(UpdateAPDatesParam _UpdateAPDatesParam, string ConnectionString)
        {
            var para = new SqlParameter[6];

            para[0] = new SqlParameter
            {
                ParameterName = "PJID",
                SqlDbType = SqlDbType.Int,
                Value = _UpdateAPDatesParam.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "Batch",
                SqlDbType = SqlDbType.Int,
                Value = _UpdateAPDatesParam.Batch
            };
            para[2] = new SqlParameter
            {
                ParameterName = "TRID",
                SqlDbType = SqlDbType.Int,
                Value = _UpdateAPDatesParam.TRID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "PostDate",
                SqlDbType = SqlDbType.DateTime,
                Value = _UpdateAPDatesParam.PostDate
            };
            para[4] = new SqlParameter
            {
                ParameterName = "IDate",
                SqlDbType = SqlDbType.DateTime,
                Value = _UpdateAPDatesParam.IDate
            };
            para[5] = new SqlParameter
            {
                ParameterName = "Due",
                SqlDbType = SqlDbType.DateTime,
                Value = _UpdateAPDatesParam.Due
            };

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateBillDate", para);
        }
        public void UpdateBillsJobDetails(PJ objPJ)
        {
            try
            {
                var para = new SqlParameter[6];

                para[0] = new SqlParameter
                {
                    ParameterName = "GLItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPJ.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = objPJ.Batch
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.MOMUSer
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPJ.fDesc
                };

                SqlHelper.ExecuteNonQuery(objPJ.ConnConfig, CommandType.StoredProcedure, "spUpdateBillsJobDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBillsJobDetails(UpdateBillsJobDetailsParam _UpdateBillsJobDetailsParam, string connectionString)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "GLItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _UpdateBillsJobDetailsParam.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Date",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateBillsJobDetailsParam.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UpdateBillsJobDetailsParam.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateBillsJobDetailsParam.Batch
                };

                SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "spUpdateBillsJobDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddCheck(CD objCD)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "BillItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objCD.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objCD.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Bank
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.Memo
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.BigInt,
                    Value = objCD.NextC
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.DiscGL
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Type
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = objCD.MOMUSer
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                SqlHelper.ExecuteNonQuery(objCD.ConnConfig, CommandType.StoredProcedure, "spAddCheck", para);
                return Convert.ToInt32(para[8].Value);
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
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "BillItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _AddCheckParam.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddCheckParam.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddCheckParam.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckParam.Bank
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckParam.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddCheckParam.Memo
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _AddCheckParam.NextC
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckParam.DiscGL
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckParam.Type
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = _AddCheckParam.MOMUSer
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddCheck", para);
                return Convert.ToInt32(para[8].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int AddCheckRecurr(CD objCD)
        {
            try
            {
                var para = new SqlParameter[13];


                para[0] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objCD.fDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Bank
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Vendor
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.Memo
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.BigInt,
                    Value = objCD.NextC
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.DiscGL
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Type
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = objCD.MOMUSer
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "TotalPay",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objCD.Amount
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.fDateYear
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.TransID
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                SqlHelper.ExecuteNonQuery(objCD.ConnConfig, CommandType.StoredProcedure, "spAddCheckRecurr", para);
                return Convert.ToInt32(para[12].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int UpdateCheckRecurr(CD objCD)
        {
            try
            {
                var para = new SqlParameter[14];


                para[0] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objCD.fDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Bank
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Vendor
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.Memo
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.BigInt,
                    Value = objCD.NextC
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.DiscGL
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Type
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = objCD.MOMUSer
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "TotalPay",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objCD.Amount
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.fDateYear
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.TransID
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "CDID",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.ID
                };
                SqlHelper.ExecuteNonQuery(objCD.ConnConfig, CommandType.StoredProcedure, "spUpdateCheckRecurr", para);
                return Convert.ToInt32(para[12].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public int UpdateCheckRecurr(UpdateCheckRecurrParam _UpdateCheckRecurrParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[14];


                para[0] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _UpdateCheckRecurrParam.fDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _UpdateCheckRecurrParam.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateCheckRecurrParam.Bank
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateCheckRecurrParam.Vendor
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _UpdateCheckRecurrParam.Memo
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _UpdateCheckRecurrParam.NextC
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateCheckRecurrParam.DiscGL
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateCheckRecurrParam.Type
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = _UpdateCheckRecurrParam.MOMUSer
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "TotalPay",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _UpdateCheckRecurrParam.Amount
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateCheckRecurrParam.fDateYear
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateCheckRecurrParam.TransID
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "CDID",
                    SqlDbType = SqlDbType.Int,
                    Value = _UpdateCheckRecurrParam.ID
                };
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateCheckRecurr", para);
                return Convert.ToInt32(para[12].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public int AddCheckRecurr(AddCheckRecurrParam _AddCheckRecurrParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[13];


                para[0] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddCheckRecurrParam.fDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddCheckRecurrParam.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckRecurrParam.Bank
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckRecurrParam.Vendor
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _AddCheckRecurrParam.Memo
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.BigInt,
                    Value = _AddCheckRecurrParam.NextC
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckRecurrParam.DiscGL
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckRecurrParam.Type
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = _AddCheckRecurrParam.MOMUSer
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "TotalPay",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddCheckRecurrParam.Amount
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckRecurrParam.fDateYear
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "PJID",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddCheckRecurrParam.TransID
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddCheckRecurr", para);
                return Convert.ToInt32(para[12].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int ApplyCredit(CD objCD)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "BillItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objCD.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objCD.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Bank
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objCD.Memo
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.NextC
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.DiscGL
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = objCD.Type
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = objCD.MOMUSer
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                SqlHelper.ExecuteNonQuery(objCD.ConnConfig, CommandType.StoredProcedure, "spApplyCredit", para);
                return Convert.ToInt32(para[8].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ApplyCredit(ApplyCreditParam _ApplyCreditParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "BillItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _ApplyCreditParam.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _ApplyCreditParam.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _ApplyCreditParam.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = _ApplyCreditParam.Bank
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _ApplyCreditParam.Vendor
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Memo",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _ApplyCreditParam.Memo
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "NextC",
                    SqlDbType = SqlDbType.Int,
                    Value = _ApplyCreditParam.NextC
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "DiscGL",
                    SqlDbType = SqlDbType.Int,
                    Value = _ApplyCreditParam.DiscGL
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.Int,
                    Value = _ApplyCreditParam.Type
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "fUser",
                    SqlDbType = SqlDbType.Text,
                    Value = _ApplyCreditParam.MOMUSer
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spApplyCredit", para);
                return Convert.ToInt32(para[8].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int UpdateApplyCreditDate(CD objCD)
        {
            try
            {
                string query = "UPDATE CreditPaid"
                + " SET fDate = @fDate WHERE fDate = @fDate1 AND FromPJID = @FromPJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@PITR", objCD.ID));
                //parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@fDate", objCD.fDate));
                parameters.Add(new SqlParameter("@fDate1", objCD.EndDate));
                parameters.Add(new SqlParameter("@FromPJID", objCD.TransID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(objCD.ConnConfig, CommandType.Text, query, parameters.ToArray());
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateApplyCreditDate(UpdateApplyCreditDateParam _UpdateApplyCreditDateParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE CreditPaid"
                + " SET fDate = @fDate WHERE fDate = @fDate1 AND FromPJID = @FromPJID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                //parameters.Add(new SqlParameter("@PITR", objCD.ID));
                //parameters.Add(new SqlParameter("@Balance", _objOpenAP.Balance));
                parameters.Add(new SqlParameter("@fDate", _UpdateApplyCreditDateParam.fDate));
                parameters.Add(new SqlParameter("@fDate1", _UpdateApplyCreditDateParam.EndDate));
                parameters.Add(new SqlParameter("@FromPJID", _UpdateApplyCreditDateParam.TransID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void updateJobComm(PO _objPO)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteDataset(_objPO.ConnConfig, "spUpdateJobCommExp", _objPO.jobID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public void updateJobComm(string ConnectionString, updateJobCommParam _updateJobCommParam)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteDataset(ConnectionString, "spUpdateJobCommExp", _updateJobCommParam.jobID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void updateCheckTemplate(User _objUser)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(" UPDATE tbluser SET ");
                builder.AppendLine(" CD_Template = @UserLic ");
                builder.AppendLine(" WHERE fUser = @Username ");

                List<SqlParameter> parameters = new List<SqlParameter>();
                string User = _objUser.MOMUSer == "Maintenance" ? "ADMIN" : _objUser.MOMUSer;
                parameters.Add(new SqlParameter("@Username", User));
                parameters.Add(new SqlParameter("@UserLic", _objUser.UserLic));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objUser.ConnConfig, CommandType.Text, builder.ToString(), parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void updateCheckTemplate(updateCheckTemplateParam _updateCheckTemplateParam, string ConnectionString)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(" UPDATE tbluser SET ");
                builder.AppendLine(" CD_Template = @UserLic ");
                builder.AppendLine(" WHERE fUser = @Username ");

                List<SqlParameter> parameters = new List<SqlParameter>();
                string User = _updateCheckTemplateParam.MOMUSer == "Maintenance" ? "ADMIN" : _updateCheckTemplateParam.MOMUSer;
                parameters.Add(new SqlParameter("@Username", User));
                parameters.Add(new SqlParameter("@UserLic", _updateCheckTemplateParam.UserLic));

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, builder.ToString(), parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCheckTemplate(User objUser)
        {
            try
            {
                string User = objUser.MOMUSer == "Maintenance" ? "ADMIN" : objUser.MOMUSer;
                return SqlHelper.ExecuteDataset(objUser.ConnConfig, CommandType.Text, "SELECT ISNULL(CD_Template,'') AS CD_Template FROM tblUser WHERE fUser ='" + User + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCheckTemplate(GetCheckTemplateParam _GetCheckTemplateParam, string ConnectionString)
        {
            try
            {
                string User = _GetCheckTemplateParam.MOMUSer == "Maintenance" ? "ADMIN" : _GetCheckTemplateParam.MOMUSer;
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ISNULL(CD_Template,'') AS CD_Template FROM tblUser WHERE fUser ='" + User + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet AutoSelectPayment(CD objCD, string company)
        {
            try
            {
                return objCD.Ds = SqlHelper.ExecuteDataset(objCD.ConnConfig, "spAutoSelectPayment", objCD.updateBy, objCD.updateByValue, objCD.isVH, objCD.isDisc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AutoSelectPayment(AutoSelectPaymentParam _AutoSelectPaymentParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spAutoSelectPayment", _AutoSelectPaymentParam.updateBy, _AutoSelectPaymentParam.updateByValue, _AutoSelectPaymentParam.isVH, _AutoSelectPaymentParam.isDisc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAutoSelectPayment(CD objCD, string company)
        {
            try
            {
                StringBuilder varname = new StringBuilder();

                varname.Append("   SELECT r.Name, o.Vendor, \n");
                if (company.ToUpper() == "INNOVATIVE")
                {
                    varname.Append("	p.fDate, \n");
                }
                else
                {
                    varname.Append("	o.fDate, \n");
                }
                varname.Append("	 o.Due,  o.Type,  o.fDesc,   o.Original,   o.Balance,  o.Selected,  o.Disc,  o.PJID,  o.TRID,  o.Disc, 0 as Discount, o.Ref,  p.Status, p.Spec, (CASE p.Spec WHEN 0 THEN 'Input Only'        WHEN 1 THEN 'Hold - No Invoices'       WHEN 2 THEN 'Hold - No Materials'       WHEN 3 THEN 'Hold - Other'       WHEN 4 THEN 'Verified'       WHEN 5 THEN 'Selected' END) as StatusName, '0.00' AS Payment,       p.fDesc AS billDesc  , IsNull(o.IsSelected,0)   As IsSelected, (ISNull(o.Original,0) - ISNULL(o.Balance,0) - (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) As Duepayment ,v.Type AS VendorType FROM OpenAP o, PJ p ,vendor v  LEFT JOIN Rol r ON r.ID = v.Rol WHERE p.ID=o.PJID and o.Vendor=v.id and o.isSelected=1 and o.Type = 0 AND o.Original<>o.Selected  group by  r.Name,o.Vendor , ");
                if (company.ToUpper() == "INNOVATIVE")
                {
                    varname.Append("	p.fDate, \n");
                }
                else
                {
                    varname.Append("	o.fDate, \n");
                }
                varname.Append("	  o.Due,  o.Type, o.fDesc,   o.Original,   o.Balance,   o.Selected,  o.Disc,    o.PJID,   o.TRID,  o.Disc,   o.Ref,   p.Status, p.Spec, p.fDesc ,o.isSelected,v.Type \n   SELECT count(OpenAP.Ref) as NCount, Sum(OpenAP.Selected) as NAmt FROM OpenAP INNER JOIN (Vendor INNER JOIN Rol ON Vendor.Rol = Rol.ID) ON OpenAP.Vendor = Vendor.ID WHERE OpenAP.IsSelected=1 ");


                return SqlHelper.ExecuteDataset(objCD.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAutoSelectPayment(GetAutoSelectPaymentParam _GetAutoSelectPaymentParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("   SELECT r.Name, o.Vendor, o.fDate, o.Due,  o.Type,  o.fDesc,   o.Original,   o.Balance,  o.Selected,  o.Disc,  o.PJID,  o.TRID,  o.Disc, 0 as Discount, o.Ref,  p.Status, p.Spec, (CASE p.Spec WHEN 0 THEN 'Input Only'        WHEN 1 THEN 'Hold - No Invoices'       WHEN 2 THEN 'Hold - No Materials'       WHEN 3 THEN 'Hold - Other'       WHEN 4 THEN 'Verified'       WHEN 5 THEN 'Selected' END) as StatusName, '0.00' AS Payment,       p.fDesc AS billDesc  , IsNull(o.IsSelected,0)   As IsSelected, (ISNull(o.Original,0) - ISNULL(o.Balance,0) - (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) As Duepayment FROM OpenAP o, PJ p ,vendor v  LEFT JOIN Rol r ON r.ID = v.Rol WHERE p.ID=o.PJID and o.Vendor=v.id and o.isSelected=1 and o.Type = 0 AND o.Original<>o.Selected  group by  r.Name,o.Vendor ,o.fdate,  o.Due,  o.Type, o.fDesc,   o.Original,   o.Balance,   o.Selected,  o.Disc,    o.PJID,   o.TRID,  o.Disc,   o.Ref,   p.Status, p.Spec, p.fDesc ,o.isSelected \n   SELECT count(OpenAP.Ref) as NCount, Sum(OpenAP.Selected) as NAmt FROM OpenAP INNER JOIN (Vendor INNER JOIN Rol ON Vendor.Rol = Rol.ID) ON OpenAP.Vendor = Vendor.ID WHERE OpenAP.IsSelected=1 ");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillingItems(PJ _objPJ)
        {
            try
            {
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "CSVItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objPJ.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPJ.EN
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPJ.UserID
                };
                return SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.StoredProcedure, "spGetBillItemData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillingItems(GetBillingItemsParam _GetBillingItemsParam, string connectionString)
        {
            try
            {
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "CSVItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _GetBillingItemsParam.Dt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetBillingItemsParam.EN
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetBillingItemsParam.UserID
                };
                return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "spGetBillItemData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillsLogs(PJ _objPJ)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPJ.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + _objPJ.ID + "  and Screen='Bills' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillsLogs(GetBillsLogsParam _GetBillsLogs, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from Log2 where ref =" + _GetBillsLogs.ID + "  and Screen='Bills' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAPCheckLogs(CD _objCD)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + _objCD.Ref + " and Screen='APCheck' and CreatedStamp is not null order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAPCheckLogs(GetAPCheckLogsParam _GetAPCheckLogsParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from Log2 where ref =" + _GetAPCheckLogsParam.Ref + " and Screen='APCheck' and CreatedStamp is not null order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOLogs(PO _objPO)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + _objPO.POID + "  and Screen='PO' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReceivePOLogs(PO _objPO)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + _objPO.RID + "  and Screen='ReceivePO' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddRPO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[14];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                    Value = _objPO.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@WB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.WB
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Comments",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Comments
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@POReceiveBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.IsReceiveIssued
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "@Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Vendor
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "@RPOItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objPO.Dt
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "@IsAddReceivePO",
                    SqlDbType = SqlDbType.Bit,
                    Value = _objPO.IsAddReceivePO
                };
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spAddReceivePO", para);
                int Rpoid = Convert.ToInt32(para[0].Value);
                return Rpoid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int EditRPO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[13];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    //Direction = ParameterDirection.Output,
                    Value = _objPO.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@WB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.WB
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Comments",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Comments
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@POReceiveBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.IsReceiveIssued
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "@Vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.Vendor
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "@RPOItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objPO.Dt
                };
               
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spEditReceivePO", para);
                int Rpoid = Convert.ToInt32(para[0].Value);
                return Rpoid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddEditReceivePO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                    Value = _objPO.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@WB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.WB
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Comments",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Comments
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.BatchID
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@POReceiveBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.IsReceiveIssued
                };
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spAddEditReceivePO", para);
                int Rpoid = Convert.ToInt32(para[0].Value);
                return Rpoid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEditReceivePO(string ConnectionString, AddEditReceivePOParam _AddEditReceivePOParam)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddEditReceivePOParam.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddEditReceivePOParam.POID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddEditReceivePOParam.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@WB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddEditReceivePOParam.WB
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Comments",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddEditReceivePOParam.Comments
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _AddEditReceivePOParam.Amount
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddEditReceivePOParam.fDate
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddEditReceivePOParam.BatchID
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddEditReceivePOParam.Due
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _AddEditReceivePOParam.MOMUSer
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@POReceiveBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _AddEditReceivePOParam.IsReceiveIssued
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddEditReceivePO", para);
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReceivePODue(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spUpdateReceivePODue", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteReceivePO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@RPOId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.RID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spDeleteReceivePO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReceivePOItem(PO _objPO)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(" UPDATE RPOItem SET ");
                builder.AppendLine(" Amount = @Amount ");
                builder.AppendLine(", Quan = @Quan ");
                builder.AppendLine(", IsReceiveIssued = @IsReceiveIssued ");
                builder.AppendLine(" WHERE ReceivePO = @ReceivePO AND POLine = @POLine ");

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReceivePO", _objPO.ReceivePOId));
                parameters.Add(new SqlParameter("@POLine", _objPO.Line));
                parameters.Add(new SqlParameter("@Quan", _objPO.Quan));
                parameters.Add(new SqlParameter("@Amount", _objPO.Amount));
                parameters.Add(new SqlParameter("@IsReceiveIssued", _objPO.IsReceiveIssued));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, builder.ToString(), parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void UpdateReceivePO(PO _objPO)
        //{
        //    try
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        builder.AppendLine(" UPDATE [ReceivePO] SET ");
        //        builder.AppendLine(" Amount = @Amount ");
        //        builder.AppendLine(", Batch = @Batch ");
        //        builder.AppendLine(" WHERE ID = @RID AND PO = @POID ");

        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@RID", _objPO.RID));
        //        parameters.Add(new SqlParameter("@Amount", _objPO.Amount));
        //        parameters.Add(new SqlParameter("@POID", _objPO.POID));
        //        parameters.Add(new SqlParameter("@Batch", _objPO.BatchID));

        //        int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.Text, builder.ToString(), parameters.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateReceivePO(PO _objPO)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@RID", _objPO.RID));
                parameters.Add(new SqlParameter("@Amount", _objPO.Amount));
                parameters.Add(new SqlParameter("@POID", _objPO.POID));
                parameters.Add(new SqlParameter("@Batch", _objPO.BatchID));
                parameters.Add(new SqlParameter("@Due", _objPO.Due));
                parameters.Add(new SqlParameter("@UpdatedBy", _objPO.MOMUSer));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spUpdateReceivePO", parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AutoUpdatePOStatus(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@POId",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spAutoUpdatePOStatus", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateWriteCheckOpenAPpayment(OpenAP _OpenAP)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append("    UPDATE OpenAP SET Balance ='" + _OpenAP.Balance + "', Disc ='" + _OpenAP.Disc + "', IsSelected ='" + _OpenAP.IsSelected + "' WHERE PJID='" + _OpenAP.PJID + "' AND Ref='" + _OpenAP.Ref + "'  ");
                varname.Append("    UPDATE OpenAP SET Balance ='" + _OpenAP.Balance + "', IsSelected ='" + _OpenAP.IsSelected + "' WHERE PJID='" + _OpenAP.PJID + "' AND Ref='" + _OpenAP.Ref + "'  ");
                SqlHelper.ExecuteNonQuery(_OpenAP.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateWriteCheckOpenAPpayment(UpdateWriteCheckOpenAPpaymentParam _UpdateWriteCheckOpenAPpaymentParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append("    UPDATE OpenAP SET Balance ='" + _OpenAP.Balance + "', Disc ='" + _OpenAP.Disc + "', IsSelected ='" + _OpenAP.IsSelected + "' WHERE PJID='" + _OpenAP.PJID + "' AND Ref='" + _OpenAP.Ref + "'  ");
                varname.Append("    UPDATE OpenAP SET Balance ='" + _UpdateWriteCheckOpenAPpaymentParam.Balance + "', IsSelected ='" + _UpdateWriteCheckOpenAPpaymentParam.IsSelected + "' WHERE PJID='" + _UpdateWriteCheckOpenAPpaymentParam.PJID + "' AND Ref='" + _UpdateWriteCheckOpenAPpaymentParam.Ref + "'  ");
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRunningBalanceCounts(CD objCD)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" Select ISNULL(SUM(ISNull(Original,0) - IsNull(Selected,0) - IsNull(Balance,0)),0) AS RunningBalance, COUNT(*) As Counts,COUNT(DISTINCT vendor) as TotVendor  From OpenAP Where ISNULL(IsSelected,0) = 1");

                return SqlHelper.ExecuteDataset(objCD.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetRunningBalanceCounts(GetRunningBalanceCountsParam _GetRunningBalanceCountsParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" Select ISNULL(SUM(ISNull(Original,0) - IsNull(Selected,0) - IsNull(Balance,0)),0) AS RunningBalance, COUNT(*) As Counts,COUNT(DISTINCT vendor) as TotVendor  From OpenAP Where ISNULL(IsSelected,0) = 1");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSelectedOpenAPPJID(CD objCD)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" SELECT OpenAP.PJID FROM ((OpenAP INNER JOIN Vendor ON OpenAP.Vendor = Vendor.ID) INNER JOIN PJ ON OpenAP.PJID = PJ.ID) INNER JOIN Rol ON Vendor.Rol = Rol.ID WHERE Vendor.Status =0 and OpenAp.Type = 0  AND OpenAp.Original<>OpenAp.Selected AND IsSelected = 1");

                return SqlHelper.ExecuteDataset(objCD.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSelectedOpenAPPJID(GetSelectedOpenAPPJIDParam _GetSelectedOpenAPPJIDParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" SELECT OpenAP.PJID FROM ((OpenAP INNER JOIN Vendor ON OpenAP.Vendor = Vendor.ID) INNER JOIN PJ ON OpenAP.PJID = PJ.ID) INNER JOIN Rol ON Vendor.Rol = Rol.ID WHERE Vendor.Status =0 and OpenAp.Type = 0  AND OpenAp.Original<>OpenAp.Selected AND IsSelected = 1");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetCustomProgramForMitsu(string connConfig)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("  SELECT   Top 1 isnull(Label,'') FROM  [Custom]   WHERE isnull(Label,'') <> ''  and [Name] ='ProgramCustom'  ");
                var strCustomProgram = SqlHelper.ExecuteScalar(connConfig, CommandType.Text, varname.ToString());
                if (strCustomProgram != null)
                {
                    return strCustomProgram.ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCreditBillVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT r.Name ,o.Vendor AS ID FROM OpenAP o, PJ p ,vendor v  LEFT JOIN Rol r ON r.ID = v.Rol WHERE p.ID=o.PJID and o.Vendor=v.ID AND o.Original<>(isnull(o.Selected,0)+isnull(o.Disc,0))  AND o.Balance < 0 AND v.Status = 0 AND o.Type = 0  ORDER BY R.Name ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCreditBillVendor(GetCreditBillVendorParam _GetCreditBillVendorParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT DISTINCT r.Name ,o.Vendor AS ID FROM OpenAP o, PJ p ,vendor v  LEFT JOIN Rol r ON r.ID = v.Rol WHERE p.ID=o.PJID and o.Vendor=v.ID AND o.Original<>(isnull(o.Selected,0)+isnull(o.Disc,0))  AND o.Balance < 0 AND v.Status = 0 AND o.Type = 0  ORDER BY R.Name ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAPGLReg(PJ objPJ, List<RetainFilter> filters, bool inclClose)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT DISTINCT \n");
                sb.Append("	p.ID, \n");
                sb.Append("	c.ID AS Acct, \n");
                sb.Append("	c.Acct + ' - ' + c.fDesc AS GLAcct, \n");
                sb.Append("	p.fDate, \n");
                sb.Append("	p.Ref, \n");
                sb.Append("	p.PO, \n");
                sb.Append("	p.ReceivePO, \n");
                sb.Append("	i.PK AS BillItemID, \n");
                sb.Append("	i.fDesc, \n");
                sb.Append("	i.JobId, \n");
                sb.Append("	j.fDesc AS JobDesc, \n");
                sb.Append("	l.Tag AS LocationName, \n");
                sb.Append("	l.STax, \n");
                sb.Append("	ISNULL(i.Amount,0) AS Amount, \n");
                sb.Append("	r.Name AS VendorName, \n");
                sb.Append("	v.Type AS VendorType \n");
                sb.Append("FROM PJ p \n");
                sb.Append(" INNER JOIN Vendor v ON p.Vendor = v.ID \n");
                sb.Append(" INNER JOIN Rol r ON v.Rol = r.ID \n");
                sb.Append(" INNER JOIN APBillItem i ON i.Batch = p.Batch \n");
                sb.Append(" LEFT JOIN Chart c ON CONVERT(varchar, c.ID) = i.AcctID \n");
                sb.Append(" LEFT JOIN Job j ON j.ID = i.JobId  \n");
                sb.Append(" LEFT JOIN Loc l ON l.Loc = j.Loc \n");
                sb.Append(" LEFT JOIN OpenAP o ON p.ID = o.PJID \n");

                if (objPJ.EN == 1)
                {
                    sb.Append(" LEFT OUTER JOIN tblUserCo uc ON uc.CompanyId = r.EN \n");
                }

                if (objPJ.Terms != 99)
                {
                    sb.Append(" INNER JOIN Trans t ON p.Batch = t.Batch ");
                    sb.Append(" LEFT JOIN Job j1 ON t.VInt = j1.ID ");
                    sb.Append(" LEFT JOIN JobTItem jt ON jt.Job = j1.ID ");
                }

                if (objPJ.Custom == "0")
                {
                    sb.Append("WHERE p.fDate >= '" + objPJ.StartDate + "' AND p.fDate <= '" + objPJ.EndDate + "' \n");

                }
                else
                {
                    sb.Append("LEFT JOIN Paid pd ON pd.TRID = p.TRID \n");
                    sb.Append("LEFT JOIN CD ON CD.ID = pd.PITR \n");
                    sb.Append("WHERE (CD.fDate >= '" + objPJ.StartDate + "') AND (CD.fDate <= '" + objPJ.EndDate + "') AND CD.fDate IS NOT NULL \n");
                }

                // Include close
                if (!inclClose)
                {
                    sb.Append(" AND (p.Status = 0 OR p.Status = 3) \n");
                }

                // Search
                if (objPJ.Vendor > 0)
                {
                    sb.Append(" AND p.Vendor = " + objPJ.Vendor + "\n");
                }

                if (objPJ.SearchValue.Equals(1))
                {
                    sb.Append(" AND o.Balance <> 0 AND o.Original <> o.Selected  \n");
                    sb.Append(" AND o.Due <= '" + DateTime.Now.ToShortDateString() + "' \n");
                }
                else if (objPJ.SearchValue.Equals(2))
                {
                    sb.Append(" AND o.Balance<>0 AND o.Original<>o.Selected  \n");
                    sb.Append(" AND o.Due <= '" + objPJ.SearchDate + "'\n");
                }

                if (!string.IsNullOrEmpty(objPJ.Ref))
                {
                    sb.Append(" AND p.Ref LIKE '%" + objPJ.Ref + "%' \n");
                }

                if (objPJ.PO > 0)
                {
                    sb.Append(" AND p.PO LIKE '%" + objPJ.PO + "%' \n");
                }

                if (objPJ.ProjectNumber > 0)
                {
                    sb.Append(" AND  j.ID = " + objPJ.ProjectNumber + "\n");
                }

                if (objPJ.Amount > 0)
                {
                    sb.Append(" AND  p.Amount LIKE '%" + objPJ.Amount + "%' \n");
                }

                if (!string.IsNullOrEmpty(objPJ.vendorName))
                {
                    sb.Append(" AND r.Name LIKE '%" + objPJ.vendorName + "%' \n");
                }

                if (objPJ.Status != 99)
                {
                    sb.Append(" AND p.Status = " + objPJ.Status + " \n");
                }

                if (objPJ.Terms != 99)
                {
                    sb.Append(" AND jt.Type = " + objPJ.Terms + " \n");
                }

                if (objPJ.EN == 1)
                {
                    sb.Append(" AND uc.IsSel = " + objPJ.EN + " AND uc.UserID = " + objPJ.UserID + " \n");
                }

                if (!string.IsNullOrEmpty(objPJ.Custom1))
                {
                    sb.Append(" AND p.Custom1 LIKE '%" + objPJ.Custom1 + "%' \n");
                }

                if (!string.IsNullOrEmpty(objPJ.Custom2))
                {
                    sb.Append(" AND p.Custom2 LIKE '%" + objPJ.Custom2 + "%' \n");
                }

                // Filters
                if (filters.Count > 0)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "fdate")
                        {
                            sb.Append(" AND CONVERT(VARCHAR(25), p.fDate, 126) LIKE '%" + filter.FilterValue + "%' \n");
                        }

                        if (filter.FilterColumn == "Ref")
                        {
                            sb.Append(" AND p.Ref LIKE '%" + filter.FilterValue + "%' \n");
                        }

                        if (filter.FilterColumn == "fDesc")
                        {
                            sb.Append(" AND p.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }

                        if (filter.FilterColumn == "VendorName")
                        {
                            sb.Append(" AND r.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }

                        if (filter.FilterColumn == "StatusName")
                        {
                            sb.Append(" AND (CASE p.Status WHEN 0 THEN 'Open'WHEN 1 THEN 'Closed' WHEN 2 THEN 'Void' WHEN 3 THEN 'Partial' END) LIKE '%" + filter.FilterValue + "%' \n");
                        }

                        if (filter.FilterColumn == "Amount")
                        {
                            sb.Append(" AND p.Amount =" + filter.FilterValue + " \n");
                        }

                        if (filter.FilterColumn == "Balance")
                        {
                            sb.Append(" AND ISNULL((SELECT (ISNULL(o.Original,0) -  (ISNULL(o.Selected,0) + ISNULL(o.Disc,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) =" + filter.FilterValue + " \n");
                        }

                        if (filter.FilterColumn == "PayDate")
                        {
                            sb.Append(" AND CONVERT(VARCHAR(25), (SELECT MAX(cd1.fDate) FROM CD cd1 LEFT JOIN Paid pd1 ON pd1.PITR = cd1.ID WHERE pd1.TRID = p.TRID), 126) LIKE '%" + filter.FilterValue + "%' \n");
                        }

                        if (filter.FilterColumn == "po")
                        {
                            sb.Append(" AND p.PO =" + filter.FilterValue + " \n");
                        }

                        if (filter.FilterColumn == "usetax")
                        {
                            sb.Append(" AND p.UseTax =" + filter.FilterValue + " \n");
                        }
                    }
                }

                return SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAPGLReg(GetAPGLRegParam _GetAPGLRegParam, string ConnectionString)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	p.ID, \n");
                sb.Append("	t.Acct, \n");
                sb.Append("	c.Acct + ' - ' + c.fDesc AS GLAcct, \n");
                sb.Append("	p.fDate, \n");
                sb.Append("	p.Ref, \n");
                sb.Append("	p.PO, \n");
                sb.Append("	p.ReceivePO, \n");
                sb.Append("	t.fDesc, \n");
                sb.Append("	ISNULL(t.Amount,0) AS Amount, \n");
                sb.Append("	r.Name AS VendorName \n");
                sb.Append("FROM PJ p \n");
                sb.Append("INNER JOIN Vendor v ON p.Vendor = v.ID \n");
                sb.Append("INNER JOIN Rol r ON v.Rol = r.ID \n");
                sb.Append("INNER JOIN Trans t ON t.Batch = p.Batch AND t.Type = 41 AND t.fDesc NOT IN ('Use Tax Payable' ,'Sales Tax Payable', 'GST Payable') \n");
                sb.Append("LEFT JOIN Chart c ON c.ID = t.Acct \n");
                sb.Append("WHERE p.fDate >= '" + _GetAPGLRegParam.StartDate + "' AND p.fDate <= '" + _GetAPGLRegParam.EndDate + "' \n");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetInventoryItemStatusbyIds(string conn, string invids)
        {
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "InvID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = invids
                };

                DataSet dsetInvStatus = new DataSet();
                dsetInvStatus = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "SPGetInvItemStatus", para);
                return dsetInvStatus.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetChartStatusbyIds(string conn, string chartIds)
        {
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "ID",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = chartIds
                };
                DataSet dsetChartStatus = new DataSet();
                dsetChartStatus = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "SPGetChartStatus", para);
                return dsetChartStatus.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInventoryItemStatus(Inventory objInv)
        {
            try
            {
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "Id",
                    SqlDbType = SqlDbType.Int,
                    Value = objInv.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = 0
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objInv.UserID
                };
                return objInv.Ds = SqlHelper.ExecuteDataset(objInv.ConnConfig, CommandType.StoredProcedure, "spGetInventory", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInventoryItemStatus(string ConnectionString, GetInventoryItemStatusParam _GetInventoryItemStatusParam)
        {
            try
            {
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "Id",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetInventoryItemStatusParam.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = 0
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetInventoryItemStatusParam.UserID
                };
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetInventory", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetHistoryTransaction(String conn, string id, string type, int vendor, int loc, String status, int tid)
        {
            try
            {
                var para = new SqlParameter[6];

                para[0] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = id
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.VarChar,
                    Value = type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = vendor
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
                return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetVendorTransactionHistory", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetHistoryTransaction(GetHistoryTransactionParam _GetHistoryTransaction, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[6];

                para[0] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetHistoryTransaction.id
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetHistoryTransaction.type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "vendor",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetHistoryTransaction.vendor
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "loc",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetHistoryTransaction.loc
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "status",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetHistoryTransaction.status
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "transID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetHistoryTransaction.tid
                };
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetVendorTransactionHistory", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable spGetOpenPODetailforRPO(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.EN
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.UserID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };

                return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure, "spGetOpenPODetailforRPO", para).Tables[0];


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdtReceivePOAmnt(PO _objPO)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.RID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@PO",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.POID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Ref
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@WB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.WB
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Comments",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.Comments
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = _objPO.Amount
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.fDate
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@Batch",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.BatchID
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@Due",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objPO.Due
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objPO.MOMUSer
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@POReceiveBy",
                    SqlDbType = SqlDbType.Int,
                    Value = _objPO.IsReceiveIssued
                };
                SqlHelper.ExecuteNonQuery(_objPO.ConnConfig, CommandType.StoredProcedure, "spUpdtReceivePOAmnt", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}