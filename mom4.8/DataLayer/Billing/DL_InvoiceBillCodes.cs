using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Text;

namespace DataLayer.Billing
{
    public class DL_BillCodes
    {

        public DataSet GetAutoCompleteBillCodes(User objPropUser, string prefixText = "")
       {
            string strQuery = "  select top 100 * from (select Inv.id as id, Inv.Name as Name, Inv.fDesc as fDesc, Inv.type as type , isnull(Hand,0) Hand , case Inv.type when 1 then Inv.Name when 0 then Inv.Name+ ' : Part'   else Inv.Name   end as BillType, Inv.Status as Status ,Price1 ,c.Status as AStatus from Inv left join Chart c on inv.SAcct=c.ID";

            strQuery += " where  1 = 1";

            //if (!string.IsNullOrEmpty(objPropUser.Type))
            //{
            //    strQuery += " and  type=" + objPropUser.Type;
            //} 

            strQuery += " and Name is not null ) X where 1 = 1";

            if (!string.IsNullOrEmpty(prefixText))
            {
                strQuery += " and (  x.name like '%" + prefixText + "%'" + "  or  x.fDesc like  '%" + prefixText + "%'   or  x.BillType like  '%" + prefixText + "%' )";
            }
            strQuery += " and Status=0";

            strQuery += " and type <> 2 ";//ref ES-699
            if (!new DL_Inventory().ISINVENTORYTRACKINGISON(objPropUser.ConnConfig))
            {
                strQuery += " and type <> 0 ";//ref ES-699
            }
            strQuery += " order by Name";
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAutoCompleteBillCodes(GetAutoCompleteBillCodesParam _GetAutoCompleteBillCodes, string ConnectionString, string prefixText = "")
        {
            string strQuery = "  select top 100 * from (select Inv.id as id, Inv.Name as Name, Inv.fDesc as fDesc, Inv.type as type , isnull(Hand,0) Hand , case Inv.type when 1 then Inv.Name when 0 then Inv.Name+ ' : Part'   else Inv.Name   end as BillType, Inv.Status as Status ,Price1 ,c.Status as AStatus from Inv left join Chart c on inv.SAcct=c.ID";

            strQuery += " where  1 = 1";

            //if (!string.IsNullOrEmpty(objPropUser.Type))
            //{
            //    strQuery += " and  type=" + objPropUser.Type;
            //} 

            strQuery += " and Name is not null ) X where 1 = 1";

            if (!string.IsNullOrEmpty(prefixText))
            {
                strQuery += " and (  x.name like '%" + prefixText + "%'" + "  or  x.fDesc like  '%" + prefixText + "%'   or  x.BillType like  '%" + prefixText + "%' )";
            }
            strQuery += " and Status=0";

            strQuery += " and type <> 2 ";//ref ES-699
            if (!new DL_Inventory().ISINVENTORYTRACKINGISON(ConnectionString))
            {
                strQuery += " and type <> 0";//ref ES-699
            }
            strQuery += " order by Name";
            try
            {
                return _GetAutoCompleteBillCodes.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllBillCodes(User objPropUser)        // get all active inactive billing code
        {
            string strQuery = "select  (select top 1 acct+' : '+fdesc from chart where id = sacct)as account, sacct, case status when 0 then 'Active' else 'Inactive' end as Status,status as statusid, (select top 1 name from warehouse where id= warehouse) as warehousename,warehouse, id, Name,fDesc,Cat,Balance,Measure,Remarks, (select type from JobType where ID=Cat)as jobtype,  case type when 1 then Name when 0 then Name+ ' : Part'   else Name   end as BillType, Price1 from Inv ";//+ space(20-len(Name))

            strQuery += " where  1=1";
            if (!string.IsNullOrEmpty(objPropUser.Type))
            {
                strQuery += " and  type=" + objPropUser.Type;
            }


            strQuery += " and Name is not null";


            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getBillCodes(User objPropUser)
        {
            string strQuery = "select (select top 1 acct+' : '+fdesc from chart where id = sacct)as account, sacct, case isnull(status,0) when 0 then 'Active' else 'Inactive' end as Status,isnull(status,0) as statusid, (select top 1 name from warehouse where id= warehouse) as warehousename,warehouse, id, Name,fDesc,Cat,Balance,Measure,Remarks, (select type from JobType where ID=Cat)as jobtype,  case type when 1 then Name when 0 then Name+ ' : Part'   else Name   end as BillType, Price1,Type from Inv  ";//+ space(20-len(Name))
            strQuery += " where  1=1";
            if (!string.IsNullOrEmpty(objPropUser.Type))
            {
                strQuery += " and  type=" + objPropUser.Type;
            }

            if(objPropUser.GLAccount > 0)
            strQuery += " and ID ="+objPropUser.GLAccount;

            strQuery += " and Name is not null";


            strQuery += " order by name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getBillCodesByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select id, Name,fDesc,Cat,Balance,Measure,Remarks, (select type from JobType where ID=Cat)as jobtype, Price1, status as statusid from Inv where id=" + objPropUser.BillCode + " order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteBillingCode(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   InvoiceI \n");
            varname1.Append("               WHERE  Acct = " + objPropUser.BillCode + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("       IF NOT EXISTS (SELECT 1 FROM   Inv WHERE  name in ('recurring', 'time spent', 'mileage', 'expenses') and ID=" + objPropUser.BillCode+")\n");
            varname1.Append("            BEGIN \n");
            varname1.Append("                DELETE FROM Inv \n");
            varname1.Append("                   WHERE  ID = " + objPropUser.BillCode + " and name not in ('recurring', 'time spent', 'mileage', 'expenses') \n");
            varname1.Append("           END \n");
            varname1.Append("        ELSE \n");
            varname1.Append("             BEGIN \n");
            varname1.Append("                RAISERROR('This is default billing code. You cannot delete!',16,1) \n");
            varname1.Append("             END ");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('This billing code is in use and cannot be deleted!',16,1) \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteBillingCodebyListID(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   InvoiceI \n");
            varname1.Append("               WHERE  Acct =( select ID from inv where isnull(qbinvid,'')<>'' and  qbinvid = '" + objPropUser.QBInvID + "' )) \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM Inv \n");
            varname1.Append("      WHERE isnull(qbinvid,'')<>'' and  qbinvid = '" + objPropUser.QBInvID + "' \n");
            varname1.Append("  END \n");
            varname1.Append("else \n");
            varname1.Append("  BEGIN");
            varname1.Append("  UPDATE Inv SET Status=1 WHERE Isnull( qbinvid,'')<>'' AND  qbinvid = '" + objPropUser.QBInvID + "' \n");
            varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetActiveBillingCode(String conn)
        {
            try
            {
                return  SqlHelper.ExecuteDataset(conn, "spGetActiveBillingCode");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetActiveBillingCode(GetActiveBillingCodeParam _GetActiveBillingCode, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetActiveBillingCode");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDefaultBillingCode(string ConnConfig, int DefaultBillingCode, string DefaultBillingCodeDesc)
        {
            try
            {
                SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "UPDATE Control SET DefaultBillingCode=" + DefaultBillingCode + " , DefaultBillingCodeDesc='" + DefaultBillingCodeDesc + "'");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
