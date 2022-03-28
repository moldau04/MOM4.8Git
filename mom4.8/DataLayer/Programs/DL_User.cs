using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using System.Web;
using System.Collections;
using BusinessEntity.Utility;


namespace DataLayer
{
    public class DL_User
    {
        public DataSet getUserAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spLoginAuthorization", objPropUser.Username, objPropUser.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void getUserAuthorization_New(UserAuthentication _UA)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[10];
                para[0] = new SqlParameter
                {
                    ParameterName = "Token",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UA.Token
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Domain_Name",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UA.Domain_Name
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "User_Id",
                    SqlDbType = SqlDbType.Int,
                    Value = _UA.User_Id
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "company",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UA.company
                };

                SqlHelper.ExecuteNonQuery(_UA.Connectionstring, CommandType.StoredProcedure, "sp_Core_CreateUserToken", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomerType(User objPropUser)
        {
            try
            {
                string query = string.Format("SELECT DISTINCT {0} FROM Owner", objPropUser.Type);

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationType(User objPropUser)
        {
            try
            {
                string query = string.Format("SELECT DISTINCT {0} FROM Loc Where Type<>''", objPropUser.Type);

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetLocationType(GetLocationTypeParam _GetLocationType, string ConnectionString)
        {
            try
            {
                string query = string.Format("SELECT DISTINCT {0} FROM Loc Where Type<>''", _GetLocationType.Type);

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, query);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getAdminAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select username, password from tbluser where username='" + objPropUser.Username + "' and password='" + objPropUser.Password + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDatabases(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') and name='" + objPropUser.DBName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CheckDB(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select table_name from information_schema.TABLES where table_name = 'Control' select column_name from information_schema.columns where table_name = 'Control' and COLUMN_NAME='MSM'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getTSUserAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spTSLoginAuthorization", objPropUser.Username, objPropUser.Password, objPropUser.DBName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserLoginAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spLoginAuthorization", objPropUser.Username, objPropUser.Password, objPropUser.DBName, objPropUser.DBType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetDefaultData(User objPropUser)
        {
            try
            {


                if (objPropUser.DBType == "MSM")  ////  ---- Only For MOM 5.0 Customer 
                {
                    int IsRunDefaultScript = 0;

                    IsRunDefaultScript = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "UPDATE [Control] set IsRunDefaultScript = 1 where IsRunDefaultScript is null    select isnull(IsRunDefaultScript,0) from  [Control]  "));

                    if (IsRunDefaultScript == 1)
                    {
                        try
                        {
                            SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "MOM_GenericScript_For_Insert_DefaultData");


                            SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "Spupdatedefaultdata", objPropUser.DBType);


                            SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "MOM_GenericScript_PrimaryAndIndex");


                            SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update [Control] set IsRunDefaultScript=0");
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMP(User objPropUser)
        {
            try
            {
                string str = " select x.fDesc ,x.id,x.Status from ( select upper(w.fDesc)as fdesc, w.id ,w.Status from tblwork w where w.id is not null ";//w.status=0

                if (objPropUser.Status == 0)
                {
                    str += "  and w.status=0 ";
                }

                if (objPropUser.Supervisor != null && objPropUser.Supervisor != string.Empty)
                {
                    str += " and w.super='" + objPropUser.Supervisor + "'";
                }

                if (objPropUser.Username != null && objPropUser.Username != string.Empty)
                {
                    str += " union select upper(w.fDesc) as fdesc, w.id , w.Status from tblwork w where w.fDesc='" + objPropUser.Username.Replace("'", "''") + "'";
                }

                if (objPropUser.WorkId != 0)
                {
                    str += " union select upper(w.fDesc) as fdesc, w.id ,w.Status from tblwork w where w.id=" + objPropUser.WorkId;
                }

                //str += " order by upper(w.fDesc)";

                str += " ) x order by x.Status asc ,x.fDesc asc ";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getEMP(GetEMPParam _GetEMP, string ConnectionString)
        {
            try
            {
                string str = " select x.fDesc ,x.id,x.Status from ( select upper(w.fDesc)as fdesc, w.id ,w.Status from tblwork w where w.id is not null ";//w.status=0

                if (_GetEMP.Status == 0)
                {
                    str += "  and w.status=0 ";
                }

                if (_GetEMP.Supervisor != null && _GetEMP.Supervisor != string.Empty)
                {
                    str += " and w.super='" + _GetEMP.Supervisor + "'";
                }

                if (_GetEMP.Username != null && _GetEMP.Username != string.Empty)
                {
                    str += " union select upper(w.fDesc) as fdesc, w.id , w.Status from tblwork w where w.fDesc='" + _GetEMP.Username.Replace("'", "''") + "'";
                }

                if (_GetEMP.WorkId != 0)
                {
                    str += " union select upper(w.fDesc) as fdesc, w.id ,w.Status from tblwork w where w.id=" + _GetEMP.WorkId;
                }

                //str += " order by upper(w.fDesc)";

                str += " ) x order by x.Status asc ,x.fDesc asc ";

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getEMPStatus(User objPropUser)
        {
            try
            {
                string str = "select w.status from tblwork w where w.fdesc = '" + objPropUser.Username.Replace("'", "''") + "'";

                return Convert.ToInt16(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, str));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMPwithDeviceID(User objPropUser)
        {
            try
            {
                string str = "select upper(w.fDesc)as fdesc,w.id from tblwork w inner join emp e on e.callsign=w.fdesc where 1=1";

                //isnull(e.deviceID,'') <> '' ";//w.status=0 and

                if (objPropUser.Supervisor != null && objPropUser.Supervisor != string.Empty)
                {
                    str += " and w.super='" + objPropUser.Supervisor + "'";
                }

                str += " order by w.fdesc";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet getEMPwithDeviceID(getTimesheetParam objPropUser, string ConnectionString)
        {
            try
            {
                string str = "select upper(w.fDesc)as fdesc,w.id from tblwork w inner join emp e on e.callsign=w.fdesc where 1=1";

                //isnull(e.deviceID,'') <> '' ";//w.status=0 and

                if (objPropUser.Supervisor != null && objPropUser.Supervisor != string.Empty)
                {
                    str += " and w.super='" + objPropUser.Supervisor + "'";
                }

                str += " order by w.fdesc";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMPScheduler(User objPropUser)
        {
            try
            {
                string str = "select upper(w.fDesc)as fdesc,w.ID,u.ProfileImage from tblwork w inner join tblUser u on w.fDesc=u.fUser and u.status = 0"; //

                //if (objPropUser.IsTS == "MSM")
                //{
                //    str += " inner join tblUser u on w.fDesc=u.fUser";
                //}

                str += " where w.Status=0";

                //if (objPropUser.IsTS == "TS")
                //{
                str += " and w.dboard=1 and (w.type=0 or w.type=1) ";
                //}
                //else if (objPropUser.IsTS == "MSM")
                //{
                //    str += " and SUBSTRING ( Ticket ,1, 1) = 'Y'";
                //}

                if (objPropUser.Supervisor != null && objPropUser.Supervisor != string.Empty)
                {
                    str += " and super='" + objPropUser.Supervisor + "'";
                }

                str += " order by fdesc";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMPSuper(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct upper(Super)as Super from tblwork where status=0 and Super is not null and Super <>'' order by Super");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet getEMPSuper(getConnectionConfigParam objPropUser, string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select distinct upper(Super)as Super from tblwork where status=0 and Super is not null and Super <>'' order by Super");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getLoginSuper(User objPropUser)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select distinct 1 from tblWork where Super='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //api
        public int getLoginSuper(AddUserParam objPropUser, string ConnectionString)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "select distinct 1 from tblWork where Super='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getISSuper(User objPropUser)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select count(1) from tblWork where Super='" + objPropUser.Username + "' and fdesc <> '" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //api
        public int getISSuper(AddUserParam objPropUser, string ConnectionString)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "select count(1) from tblWork where Super='" + objPropUser.Username + "' and fdesc <> '" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getAlltblWork(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblWork where Status=0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getControl(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select c.*,isnull(JobCostLabor,0) as JobCostLabor1, isnull(msemail,0) as msemailnull, isnull(QBFirstSync,1) as EmpSync, isnull(msrep,0) as msreptemp, isnull(tinternet,0) as tinternett, businessstart, businessend,isnull(MSIsTaskCodesRequired,0) as TaskCode , c.YE As Year,ISNULL(IsUseTaxAPBill,0) as IsUseTaxAPBills,ISNULL(IsSalesTaxAPBill,0) as IsSalesTaxAPBills, TargetHPermission, r.Email as PwResetAdminEmail, u.fUser as PwResetUsername  from control  c left join tblUser u on u.ID = c.PwResetUserID left join emp e on e.CallSign = u.fUser left join Rol r on r.id = e.Rol");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetControlForQB(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select *,isnull(JobCostLabor,0) as JobCostLabor1, isnull(msemail,0) as msemailnull, isnull(QBFirstSync,1) as EmpSync, isnull(msrep,0) as msreptemp, isnull(tinternet,0) as tinternett, businessstart, businessend,isnull(MSIsTaskCodesRequired,0) as TaskCode , YE As Year from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getControl(getConnectionConfigParam objPropUser, string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetcontrol");
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select c.*,isnull(JobCostLabor,0) as JobCostLabor1, isnull(msemail,0) as msemailnull, isnull(QBFirstSync,1) as EmpSync, isnull(msrep,0) as msreptemp, isnull(tinternet,0) as tinternett, businessstart, businessend,isnull(MSIsTaskCodesRequired,0) as TaskCode , c.YE As Year,ISNULL(IsUseTaxAPBill,0) as IsUseTaxAPBills,ISNULL(IsSalesTaxAPBill,0) as IsSalesTaxAPBills, TargetHPermission, r.Email as PwResetAdminEmail, u.fUser as PwResetUsername  from control  c left join tblUser u on u.ID = c.PwResetUserID left join emp e on e.CallSign = u.fUser left join Rol r on r.id = e.Rol");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String getCompanyAddress(User objPropUser)
        {
            String RetVal = "";
            try
            {
                return RetVal = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select Address from Control where DBName='" + objPropUser.DBName + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet getLogo(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select logo from " + objPropUser.DBName + ".dbo.Control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getControlBranch(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF EXISTS(SELECT 1 \n");
            varname1.Append("          FROM   branch b \n");
            varname1.Append("                 INNER JOIN Rol r \n");
            varname1.Append("                         ON r.EN = b.ID \n");
            varname1.Append("                 INNER JOIN loc l \n");
            varname1.Append("                         ON l.Rol = r.ID \n");
            varname1.Append("          WHERE  l.Loc = " + objPropUser.LocID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      SELECT ( (SELECT TOP 1 NAME \n");
            varname1.Append("                FROM   Control) \n");
            varname1.Append("               + ',' + Space(1) + b.NAME ) AS NAME, \n");
            varname1.Append("             b.Address, \n");
            varname1.Append("             b.City, \n");
            varname1.Append("             b.State, \n");
            varname1.Append("             b.Zip, \n");
            varname1.Append("             b.Phone, \n");
            varname1.Append("             (SELECT TOP 1 EMail \n");
            varname1.Append("              FROM   Control)              AS EMail, \n");
            varname1.Append("             b.Fax, \n");
            varname1.Append("             b.Logo \n");
            varname1.Append("      FROM   branch b \n");
            varname1.Append("             INNER JOIN Rol r \n");
            varname1.Append("                     ON r.EN = b.ID \n");
            varname1.Append("             INNER JOIN loc l \n");
            varname1.Append("                     ON l.Rol = r.ID \n");
            varname1.Append("      WHERE  l.Loc = " + objPropUser.LocID + " \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      SELECT NAME, \n");
            varname1.Append("             Address, \n");
            varname1.Append("             City, \n");
            varname1.Append("             State, \n");
            varname1.Append("             Zip, \n");
            varname1.Append("             Phone, \n");
            varname1.Append("             EMail, \n");
            varname1.Append("             Fax, \n");
            varname1.Append("             Logo \n");
            varname1.Append("      FROM   Control \n");
            varname1.Append("  END ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getControlBranch(GetControlBranchParam objGetControlBranchParam, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF EXISTS(SELECT 1 \n");
            varname1.Append("          FROM   branch b \n");
            varname1.Append("                 INNER JOIN Rol r \n");
            varname1.Append("                         ON r.EN = b.ID \n");
            varname1.Append("                 INNER JOIN loc l \n");
            varname1.Append("                         ON l.Rol = r.ID \n");
            varname1.Append("          WHERE  l.Loc = " + objGetControlBranchParam.LocID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      SELECT ( (SELECT TOP 1 NAME \n");
            varname1.Append("                FROM   Control) \n");
            varname1.Append("               + ',' + Space(1) + b.NAME ) AS NAME, \n");
            varname1.Append("             b.Address, \n");
            varname1.Append("             b.City, \n");
            varname1.Append("             b.State, \n");
            varname1.Append("             b.Zip, \n");
            varname1.Append("             b.Phone, \n");
            varname1.Append("             (SELECT TOP 1 EMail \n");
            varname1.Append("              FROM   Control)              AS EMail, \n");
            varname1.Append("             b.Fax, \n");
            varname1.Append("             b.Logo \n");
            varname1.Append("      FROM   branch b \n");
            varname1.Append("             INNER JOIN Rol r \n");
            varname1.Append("                     ON r.EN = b.ID \n");
            varname1.Append("             INNER JOIN loc l \n");
            varname1.Append("                     ON l.Rol = r.ID \n");
            varname1.Append("      WHERE  l.Loc = " + objGetControlBranchParam.LocID + " \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      SELECT NAME, \n");
            varname1.Append("             Address, \n");
            varname1.Append("             City, \n");
            varname1.Append("             State, \n");
            varname1.Append("             Zip, \n");
            varname1.Append("             Phone, \n");
            varname1.Append("             EMail, \n");
            varname1.Append("             Fax, \n");
            varname1.Append("             Logo \n");
            varname1.Append("      FROM   Control \n");
            varname1.Append("  END ");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet getAdminControl(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select * from tblcontrol");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAdminControlByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select * from tblcontrol where id=" + objPropUser.CtrlID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet getSysAllDB(User objPropUser)
        //{
        //    try
        //    {
        //        return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, SELECT name FROM sys.databases where name=);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet getRoute(User objPropUser, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
        {
            try
            {
                string str = "Select * , Name + case   when Name=mechname then '' else'-'+mechname   end as label  from (";

                if (IsActive == 1)
                {
                    str += @"select distinct * from( select name,id,remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname 
                    
                      from route    where (select top 1 status from tblwork where id = mech) = 0 ";

                    if (LocID > 0)
                    {
                        str += @" union all

                           SELECT NAME, id, remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname   
                           
                           FROM   route   where id = (SELECT Route FROM Loc  where loc=" + LocID + ") ";
                    }

                    if (ContractID > 0)
                    {
                        str += @" union all

                           SELECT NAME, id, remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname   

                           FROM   route   where id = (SELECT loc.route   FROM job inner join loc on loc.loc=job.loc  where job.ID=" + ContractID + ") ";
                    }

                    str += "   ) x       ";
                }
                else
                {
                    str += @"select name,id,remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname 
        
                     from route      ";

                }

                str += "   ) xxx  order by Name     ";

                str += @"     select top 1 u.fUser , r.ID  from tblUser u 

                       inner join tblwork w on w.fdesc = u.fUser

                       inner join route r on r.mech = w.id  where u.DefaultWorker = 1 ";


                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getRoute(GetRouteParam _GetRoute, string ConnectionString, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
        {
            try
            {
                string str = "Select * , Name + case   when Name=mechname then '' else'-'+mechname   end as label  from (";

                if (IsActive == 1)
                {
                    str += @"select distinct * from( select name,id,remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname 
                    
                      from route    where (select top 1 status from tblwork where id = mech) = 0 ";

                    if (LocID > 0)
                    {
                        str += @" union all

                           SELECT NAME, id, remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname   
                           
                           FROM   route   where id = (SELECT Route FROM Loc  where loc=" + LocID + ") ";
                    }

                    if (ContractID > 0)
                    {
                        str += @" union all

                           SELECT NAME, id, remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname   

                           FROM   route   where id = (SELECT loc.route   FROM job inner join loc on loc.loc=job.loc  where job.ID=" + ContractID + ") ";
                    }

                    str += "   ) x       ";
                }
                else
                {
                    str += @"select name,id,remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname 
        
                     from route      ";

                }

                str += "   ) xxx  order by Name     ";

                str += @"     select top 1 u.fUser , r.ID  from tblUser u 

                       inner join tblwork w on w.fdesc = u.fUser

                       inner join route r on r.mech = w.id  where u.DefaultWorker = 1 ";


                return _GetRoute.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getRouteActive(User objPropUser)
        {
            try
            {
                string str = @"SELECT r.*, w.fDesc AS MechName FROM Route r INNER JOIN tblWork w ON r.Mech = w.ID WHERE r.Status = 1 AND w.Status = 0 ORDER BY r.Name";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getRouteActive(GetRouteActiveParam _GetRouteActive, string ConnectionString)
        {
            try
            {
                string str = @"SELECT r.*, w.fDesc AS MechName FROM Route r INNER JOIN tblWork w ON r.Mech = w.ID WHERE r.Status = 1 AND w.Status = 0 ORDER BY r.Name";

                return _GetRouteActive.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getRoutesGrid(User objPropUser, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
        {
            try
            {
                string str = string.Empty;
                if (IsActive == 1)
                {
                    //str = @"select * from( select name,id,remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname from route
                    //  where (select top 1 status from tblwork where id = mech) = 0 ";
                    str = @"select Distinct * from( select (CASE WHEN ro.Status=1 THEN 'Active' ELSE 'Inactive' END) As Status, ro.name,ro.id,ro.remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname,r.EN, isnull(B.Name, '') As Company from route ro Left join Loc l on ro.id = l.Route Left join Rol r on l.Rol = r.ID Left Join 
                            Branch B on B.ID = r.EN Left join tblUserCo UC on UC.CompanyID = r.EN
                      where (select top 1 status from tblwork where ro.id = mech) = 0 ";
                    if (objPropUser.EN == 1)
                    {
                        str += "And  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;
                    }
                    if (!objPropUser.IsActiveInactive)
                    {
                        str += " And ro.Status=1 ";
                    }
                    if (LocID > 0)
                    {
                        //str += @" union all
                        //SELECT NAME, id, remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname   FROM   route   where id = (SELECT Route FROM Loc  where loc=" + LocID + ") ";
                        str += @" union all
                           SELECT (CASE WHEN ro.Status=1 THEN 'Active' ELSE 'Inactive' END) As Status, ro.NAME, ro.id, ro.remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname, r.EN, isnull(B.Name, '') As Company   FROM   route  ro Left join Loc l on ro.id = l.Route Left join Rol r on l.Rol = r.ID Left Join 
                           Branch B on B.ID = r.EN Left join tblUserCo UC on UC.CompanyID = r.EN  where ro.id = (SELECT Route FROM Loc  where loc=" + LocID + ") ";
                        if (objPropUser.EN == 1)
                        {
                            str += "And  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;
                        }
                        if (!objPropUser.IsActiveInactive)
                        {
                            str += " And ro.Status=1 ";
                        }
                    }

                    if (ContractID > 0)
                    {
                        //str += @" union all
                        //SELECT NAME, id, remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname   FROM   route   where id = (SELECT Custom20 FROM job  where ID=" + ContractID + ") ";
                        str += @" union all
                           SELECT (CASE WHEN ro.Status=1 THEN 'Active' ELSE 'Inactive' END) As Status,ro.NAME, ro.id, ro.remarks,isnull(Color,'FFF') as Color, (SELECT TOP 1 fdesc  FROM   tblwork    WHERE  id = mech) AS mechname, r.EN, isnull(B.Name, '') As Company FROM   route ro Left join Loc l on ro.id = l.Route Left join Rol r on l.Rol = r.ID Left Join 
                           Branch B on B.ID = r.EN Left join tblUserCo UC on UC.CompanyID = r.EN   where ro.id = (SELECT Custom20 FROM job  where ID=" + ContractID + ") ";
                        if (objPropUser.EN == 1)
                        {
                            str += "And UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;
                        }
                        if (!objPropUser.IsActiveInactive)
                        {
                            str += " WHERE ro.Status=1 ";
                        }
                    }

                    str += " ) x  order by x.Name select fUser from tblUser where DefaultWorker = 1 ";
                }
                else
                {
                    // str = @" select name,id,remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname from route     order by name
                    //  select fUser from tblUser where DefaultWorker = 1 ";
                    str = @" select Distinct (CASE WHEN ro.Status=1 THEN 'Active' ELSE 'Inactive' END) As Status, ro.name,ro.id,ro.remarks,isnull(Color,'FFF') as Color,(select top 1 fdesc from tblwork where id = mech) as mechname,(Select TOP 1 r.EN From Rol r inner Join Loc l on r.ID = l.Rol Where l.Route = ro.id AND r.EN > 0) AS EN, (Select TOP 1 B.Name From Branch B inner join  Rol r on B.ID = r.EN inner Join Loc l on l.Rol = r.ID Where l.Route = ro.id AND B.Name is not NULL) AS Company from route ro Left join Loc l on ro.id = l.Route Left join Rol r on l.Rol = r.ID Left Join 
                             Branch B on B.ID = r.EN Left join tblUserCo UC on UC.CompanyID = r.EN ";
                    if (objPropUser.EN == 1)
                    {
                        if (!objPropUser.IsActiveInactive)
                        {
                            str += "Where ro.Status=1 AND UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;
                        }
                    }
                    else
                    {
                        if (!objPropUser.IsActiveInactive)
                        {
                            str += "Where ro.Status=1 ";
                        }
                    }
                    str += " order by ro.name select fUser from tblUser where DefaultWorker = 1 ";
                }

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getTerritory(User objPropUser, Int32 IsSalesAsigned = 0, int EstimateID = 0, int OpportunityID = 0, string orderby = "t.name")
        {
            try
            {
                string str = "select t.Name,t.ID,t.SDesc from terr t where (select count(*) from emp where id = SMan)>0 ";

                if (EstimateID > 0) //  InActive Terr Show in DDl in Edit mode For Existing Estimate
                {
                    str += " or t.id in (  select isNull(EstimateUserId,0) from Estimate where id = " + EstimateID + " )";
                }

                if (OpportunityID > 0) //  InActive Terr Show in DDl in Edit mode For Existing Opportunity
                {
                    str += " or t.id in (  select isnull(AssignedToID,0) from Lead where ID = " + OpportunityID + " )";
                }

                if (IsSalesAsigned > 0)
                {
                    str += " and t.Name =(SELECT fUser FROM tblUser WHERE id=" + IsSalesAsigned + " )";

                    //  Get Default SalesPerson OR Second salespersond On Edit Locaton Screen 
                    if (objPropUser.LocID > 0)
                    {
                        str += " or t.id in (  SELECT  terr   FROM loc  WHERE loc = " + objPropUser.LocID + " )";
                        str += " or t.id in (  SELECT  terr2   FROM loc  WHERE loc = " + objPropUser.LocID + " )";
                    }
                }

                if (!string.IsNullOrEmpty(orderby))
                {
                    str += string.Format(" order by {0} ", orderby);
                }
                //str += " order by t.name ";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);//inner join Emp e on e.ID=t.sman where Sales='1'
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesPerson(User objPropUser, Int32 IsSalesAsigned = 0, int refID = 0, string screen = "", string orderby = "t.name")
        {
            try
            {
                string str = "select t.Name,t.ID,t.SDesc " +
                    //",case when u.fUser is null or u.Status = 1 then 1 else 0 end Status " +
                    "from terr t " +
                    "inner join emp ON t.SMan = emp.id " +
                    "inner join tblUser u On u.fUser = emp.CallSign " +
                    "Where u.Status <> 1  AND emp.Sales = 1 ";

                if (IsSalesAsigned > 0)
                {
                    str += " and u.ID=" + IsSalesAsigned + " ";
                }

                if (!string.IsNullOrEmpty(screen))
                {
                    if (screen.ToUpper() == "ESTIMATE" && refID > 0)
                    {
                        str += "union " +
                            "select t.Name,t.ID,t.SDesc " +
                            "from terr t " +
                            "inner join Estimate e on e.EstimateUserId = t.id " +
                            "Where e.id = " + refID + " ";
                    }
                    else if (screen.ToUpper() == "OPPORTUNITY" && refID > 0)
                    {
                        str += "union " +
                            "select t.Name,t.ID,t.SDesc " +
                            "from terr t " +
                            "inner join Lead l on l.AssignedToID = t.id " +
                            "Where l.id = " + refID + " ";
                    }
                    else if (screen.ToUpper() == "LEAD" && refID > 0)
                    {
                        str += "union " +
                            "select t.Name,t.ID,t.SDesc " +
                            "from terr t " +
                            "inner join Prospect p on p.Terr = t.ID " +
                            "Where p.id = " + refID + " ";
                    }
                    else if (screen.ToUpper() == "LOCATION" && refID > 0)
                    {
                        str += "union " +
                            "select t.Name,t.ID,t.SDesc " +
                            "from terr t " +
                            "inner join loc l on l.Terr = t.ID OR l.Terr2 = t.ID " +
                            "Where l.Loc = " + refID + " ";

                        //str += "union " +
                        //    "select t.Name,t.ID,t.SDesc " +
                        //    "from terr t " +
                        //    "inner join loc l on l.Terr2 = t.ID " +
                        //    "Where l.Loc = " + refID + " ";
                    }
                    else if (screen.ToUpper() == "INVOICE" && refID > 0)
                    {
                        str += "union " +
                            "select t.Name,t.ID,t.SDesc " +
                            "from terr t " +
                            "inner join invoice l on l.AssignedTo = t.ID " +
                            "Where l.Ref = " + refID + " ";
                    }
                    else if (screen.ToUpper() == "JOB" && refID > 0)
                    {
                        str += "union " +
                            "select t.Name,t.ID,t.SDesc " +
                            "from terr t " +
                            "inner join loc l on l.Terr = t.ID OR l.Terr2 = t.ID " +
                            "inner join job j on j.Loc = l.Loc " +
                            "Where j.ID = " + refID + " ";
                    }
                }

                if (!string.IsNullOrEmpty(orderby))
                {
                    str = string.Format("select * from ({0}) t order by {1} ", str, orderby);
                }

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);//inner join Emp e on e.ID=t.sman where Sales='1'
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet getTerritory(GetTerritoryParam _GetTerritory, string ConnectionString, Int32 IsSalesAsigned = 0, int EstimateID = 0, int OpportunityID = 0)
        {
            try
            {
                string str = "select t.Name,t.ID,t.SDesc from terr t where (select top 1 status from emp where id = SMan) = 0 ";

                if (EstimateID > 0) //  InActive Terr Show in DDl in Edit mode For Existing Estimate
                {
                    str += " or t.id in (  select isNull(EstimateUserId,0) from Estimate where id = " + EstimateID + " )";
                }

                if (OpportunityID > 0) //  InActive Terr Show in DDl in Edit mode For Existing Opportunity
                {
                    str += " or t.id in (  select isnull(AssignedToID,0) from Lead where ID = " + OpportunityID + " )";
                }

                if (IsSalesAsigned > 0)
                {
                    str += " and t.Name =(SELECT fUser FROM tblUser WHERE id=" + IsSalesAsigned + " )";

                    //  Get Default SalesPerson OR Second salespersond On Edit Locaton Screen 
                    if (_GetTerritory.LocID > 0)
                    {
                        str += " or t.id in (  SELECT  terr   FROM loc  WHERE loc = " + _GetTerritory.LocID + " )";
                        str += " or t.id in (  SELECT  terr2   FROM loc  WHERE loc = " + _GetTerritory.LocID + " )";
                    }
                }
                str += " order by t.name ";
                return _GetTerritory.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);//inner join Emp e on e.ID=t.sman where Sales='1'
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllTerritory(User objPropUser)
        {
            try
            {
                string str = "select t.Name,t.ID,t.SDesc from terr t ";
                str += " order by t.name ";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAllTerritory(GetAllTerritoryParam _GetAllTerritory, string ConnectionString)
        {
            try
            {
                string str = "select t.Name,t.ID,t.SDesc from terr t ";
                str += " order by t.name ";

                return _GetAllTerritory.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetUsers", DBNull.Value, DBNull.Value, objPropUser.DBName, objPropUser.IsSuper, objPropUser.Supervisor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserForSupervisor(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT e.ID, \n");
            varname1.Append("       e.fFirst, \n");
            varname1.Append("       e.Last, \n");
            varname1.Append("       w.ID AS userid, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       u.Status, \n");
            varname1.Append("       w.super, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 'Office' \n");
            varname1.Append("         ELSE 'Field' \n");
            varname1.Append("       END  AS usertype, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 0 \n");
            varname1.Append("         ELSE 1 \n");
            varname1.Append("       END  AS usertypeid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN '0_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("         ELSE '1_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("       END  AS userkey \n");
            varname1.Append("FROM   tblUser u \n");
            varname1.Append("       LEFT OUTER JOIN Emp e \n");
            varname1.Append("                    ON u.fUser = e.CallSign \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  Super = '" + objPropUser.Supervisor + "' and fuser <> '" + objPropUser.Supervisor + "'");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet getUserForSupervisor(AddUserParam objPropUser, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT e.ID, \n");
            varname1.Append("       e.fFirst, \n");
            varname1.Append("       e.Last, \n");
            varname1.Append("       w.ID AS userid, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       u.Status, \n");
            varname1.Append("       w.super, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 'Office' \n");
            varname1.Append("         ELSE 'Field' \n");
            varname1.Append("       END  AS usertype, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 0 \n");
            varname1.Append("         ELSE 1 \n");
            varname1.Append("       END  AS usertypeid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN '0_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("         ELSE '1_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("       END  AS userkey \n");
            varname1.Append("FROM   tblUser u \n");
            varname1.Append("       LEFT OUTER JOIN Emp e \n");
            varname1.Append("                    ON u.fUser = e.CallSign \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  Super = '" + objPropUser.Supervisor + "' and fuser <> '" + objPropUser.Supervisor + "'");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSelectedUser(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT e.ID, \n");
            varname1.Append("       e.fFirst, \n");
            varname1.Append("       e.Last, \n");
            varname1.Append("       w.ID AS userid, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       u.Status, \n");
            varname1.Append("       w.super, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 'Office' \n");
            varname1.Append("         ELSE 'Field' \n");
            varname1.Append("       END  AS usertype, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 0 \n");
            varname1.Append("         ELSE 1 \n");
            varname1.Append("       END  AS usertypeid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN '0_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("         ELSE '1_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("       END  AS userkey \n");
            varname1.Append("FROM   tblUser u \n");
            varname1.Append("       LEFT OUTER JOIN Emp e \n");
            varname1.Append("                    ON u.fUser = e.CallSign \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  w.id in (" + objPropUser.Address + ")");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getUsersSuper(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT e.ID, \n");
            varname1.Append("       e.fFirst, \n");
            varname1.Append("       e.Last, \n");
            varname1.Append("       w.ID AS userid, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       u.Status, \n");
            varname1.Append("       w.super, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 'Office' \n");
            varname1.Append("         ELSE 'Field' \n");
            varname1.Append("       END  AS usertype, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 0 \n");
            varname1.Append("         ELSE 1 \n");
            varname1.Append("       END  AS usertypeid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN '0_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("         ELSE '1_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("       END  AS userkey \n");
            varname1.Append("FROM   tblUser u \n");
            varname1.Append("       LEFT OUTER JOIN Emp e \n");
            varname1.Append("                    ON u.fUser = e.CallSign \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  fuser <> '" + objPropUser.Supervisor + "'");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public DataSet getSupervisor(User objPropUser)
        {
            //select fuser from tbluser
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct upper(super) as fuser from tblwork where super <> ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Active Supervisor
        public DataSet getSupervisorActive(User objPropUser)
        {
            //select fuser from tbluser
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct upper(super) as fuser from tblwork where super <> ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getFieldUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblwork order by fdesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUnassignedCalls(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT t.id, \n");
            varname1.Append("                CASE \n");
            varname1.Append("                  WHEN t.Owner IS NULL THEN LDesc2 \n");
            varname1.Append("                  ELSE (SELECT top 1 l.Tag FROM   Loc l  WHERE  l.Loc = t.LID) \n");
            varname1.Append("                END                                                        AS ldesc1, \n");
            varname1.Append("                cdate, \n");
            varname1.Append("                edate, \n");
            varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
            varname1.Append("                t.cat, \n");
            varname1.Append("                r.EN, \n");
            varname1.Append("                Isnull(B.Name, '')       As Company, \n");
            varname1.Append("                Isnull(t.high, 0)                                          AS high, \n");
            varname1.Append("                (SELECT r.Lat \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lat, \n");
            varname1.Append("                (SELECT r.Lng \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lng, \n");
            varname1.Append("                CONVERT(VARCHAR(max), t.fdesc)                             AS fdesc, \n");
            varname1.Append("                Isnull((SELECT Isnull(dispalert, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS dispalert, \n");
            varname1.Append("                Isnull((SELECT Isnull(credit, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS credithold, \n");
            varname1.Append("               r.Name, \n");
            varname1.Append("               r.Phone, \n");
            varname1.Append("               r.Contact, \n");
            varname1.Append("               r.City, \n");
            varname1.Append("               t.Assigned \n");
            varname1.Append("FROM   TicketO t \n");
            varname1.Append(" INNER JOIN Loc l  ON l.Loc = t.LID \n");
            varname1.Append(" LEFT JOIN Rol r on r.ID = l.Rol \n");
            varname1.Append(" LEFT JOIN Branch B on B.ID = r.EN \n");
            varname1.Append(" LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN \n");
            // if (objPropUser.Locationname != string.Empty) { varname1.Append(" INNER JOIN Loc l  ON l.Loc = t.LID \n"); } 

            //varname1.Append("WHERE  Assigned = 0 \n");
            varname1.Append("WHERE 1 = 1  \n");
            if (!string.IsNullOrEmpty(objPropUser.SearchValue))
            {
                varname1.Append(" AND ( r.Name like '%" + objPropUser.SearchValue + "%'  \n");
                varname1.Append(" OR r.Address like '%" + objPropUser.SearchValue + "%'  \n");
                varname1.Append(" OR r.Contact like '%" + objPropUser.SearchValue + "%'  \n");
                varname1.Append(" OR r.Phone like '%" + objPropUser.SearchValue + "%'  \n");
                varname1.Append(" OR l.City like '%" + objPropUser.SearchValue + "%'  \n");
                varname1.Append(" OR l.Tag like '%" + objPropUser.SearchValue + "%'  \n");
                varname1.Append(" OR t.WorkOrder like '%" + objPropUser.SearchValue + "%'  \n");
                varname1.Append(" OR t.ID like '%" + objPropUser.SearchValue + "%' ) \n");
            }

            if (!string.IsNullOrEmpty(objPropUser.SearchValueUnAssignedCalls))
            {
                varname1.Append(" AND ( r.Address like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
                varname1.Append(" OR l.City like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
                varname1.Append(" OR l.Tag like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
                varname1.Append(" OR t.ID like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
                varname1.Append(" OR t.CDate like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
                varname1.Append(" OR t.Cat like '%" + objPropUser.SearchValueUnAssignedCalls + "%' )  \n");
            }

            if (!string.IsNullOrEmpty(objPropUser.CategoryName)) { varname1.Append(" AND   t.Cat = '" + objPropUser.CategoryName + "' \n"); }

            if (objPropUser.EN == 1) { varname1.Append("AND UC.IsSel = 1 and UC.UserID = " + objPropUser.UserID); }

            varname1.Append("         ORDER  BY EDate ");


            try
            {//"select distinct t.id, ldesc1,cdate ,edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address,t.cat , r.Lat, r.Lng from TicketO t inner join Loc l on t.LID=l.Loc inner join Rol r on r.ID=l.Rol   where Assigned =0  order by EDate"
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());// Assigned <>4  and DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropUser.Edate + "' and DWork='" + objPropUser.FieldEmp + "' and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getOpenCallsOnMap(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT  TOP 1000  t.dwork,t.assigned, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname,t.id, \n");
            varname1.Append("                CASE \n");
            varname1.Append("                  WHEN t.Owner IS NULL THEN LDesc2 \n");
            varname1.Append("                  ELSE ldesc1 \n");
            varname1.Append("                END                                                        AS ldesc1, \n");
            varname1.Append("                cdate, \n");
            varname1.Append("                edate, \n");
            varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
            varname1.Append("                t.cat, \n");
            varname1.Append("                r.EN, \n");
            varname1.Append("                isnull(B.Name, '') As Company, \n");
            varname1.Append("                Isnull(t.high, 0)                                          AS high, \n");
            varname1.Append("                (SELECT isnull(r.Lat,'') as lat \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lat, \n");
            varname1.Append("                (SELECT isnull( r.Lng,'') as lng \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lng, \n");
            varname1.Append("                CONVERT(VARCHAR(max), t.fdesc)                             AS fdesc, \n");
            varname1.Append("                Isnull((SELECT Isnull(dispalert, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS dispalert, \n");
            varname1.Append("                Isnull((SELECT Isnull(credit, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS credithold \n");
            varname1.Append("FROM   TicketO t \n");
            varname1.Append("INNER JOIN Loc l ON l.Loc = t.lid INNER JOIN Rol r  ON l.Rol = r.ID  \n");
            varname1.Append("LEFT OUTER JOIN Branch B on B.ID = r.EN LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN  \n");
            varname1.Append("WHERE  Assigned <> 4 \n");

            if (objPropUser.EN == 1)
            {
                varname1.Append(" AND  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID);
            }

            if (objPropUser.CategoryName != "NoCat")
                varname1.Append(" AND   t.Cat IN (" + objPropUser.CategoryName + ") \n");

            if (objPropUser.FieldEmp != string.Empty) { varname1.Append(" AND   t.dwork = '" + objPropUser.FieldEmp + "' \n"); }

            varname1.Append("             ORDER  BY EDate,assigned ");


            try
            {//"select distinct t.id, ldesc1,cdate ,edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address,t.cat , r.Lat, r.Lng from TicketO t inner join Loc l on t.LID=l.Loc inner join Rol r on r.ID=l.Rol   where Assigned =0  order by EDate"
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());// Assigned <>4  and DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropUser.Edate + "' and DWork='" + objPropUser.FieldEmp + "' and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpenCallsMapScreen(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT t.dwork,t.assigned, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname,t.id, \n");
            varname1.Append("                CASE \n");
            varname1.Append("                  WHEN t.Owner IS NULL THEN LDesc2 \n");
            varname1.Append("                  ELSE ldesc1 \n");
            varname1.Append("                END                                                        AS ldesc1, \n");
            varname1.Append("                cdate, \n");
            varname1.Append("                edate, \n");
            varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
            varname1.Append("                t.cat, \n");
            varname1.Append("                r.EN, \n");
            varname1.Append("                isnull(B.Name, '') As Company, \n");
            varname1.Append("                Isnull(t.high, 0)                                          AS high, \n");
            varname1.Append("                (SELECT isnull(r.Lat,'') as lat \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lat, \n");
            varname1.Append("                (SELECT isnull( r.Lng,'') as lng \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lng, \n");
            varname1.Append("                CONVERT(VARCHAR(max), t.fdesc)                             AS fdesc, \n");
            varname1.Append("                Isnull((SELECT Isnull(dispalert, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS dispalert, \n");
            varname1.Append("                Isnull((SELECT Isnull(credit, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS credithold \n");
            varname1.Append("FROM   TicketO t \n");
            varname1.Append("INNER JOIN Loc l ON l.Loc = t.lid INNER JOIN Rol r  ON l.Rol = r.ID  \n");
            varname1.Append("LEFT OUTER JOIN Branch B on B.ID = r.EN LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN  \n");
            varname1.Append("WHERE  Assigned <> 4 \n");

            if (objPropUser.EN == 1)
            {
                varname1.Append(" AND  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID);
            }

            if (objPropUser.CategoryName != string.Empty) { varname1.Append(" AND   t.Cat IN (" + objPropUser.CategoryName + ") \n"); }

            if (objPropUser.FieldEmp != string.Empty) { varname1.Append(" AND   t.dwork = '" + objPropUser.FieldEmp + "' \n"); }

            varname1.Append("             ORDER  BY EDate,assigned ");


            try
            {//"select distinct t.id, ldesc1,cdate ,edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address,t.cat , r.Lat, r.Lng from TicketO t inner join Loc l on t.LID=l.Loc inner join Rol r on r.ID=l.Rol   where Assigned =0  order by EDate"
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());// Assigned <>4  and DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropUser.Edate + "' and DWork='" + objPropUser.FieldEmp + "' and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomers(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {


                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomers", DBNull.Value, DBNull.Value, objPropUser.DBName, IsSalesAsigned, objPropUser.EN, objPropUser.UserID, objPropUser.inclInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getCustomers(GetCustomersParam _GetCustomers, Int32 IsSalesAsigned, string ConnectionString)
        {
            try
            {


                return _GetCustomers.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetCustomers", DBNull.Value, DBNull.Value, _GetCustomers.DBName, IsSalesAsigned, _GetCustomers.EN, _GetCustomers.UserID, _GetCustomers.inclInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMCustomers(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NULL ");

            //or LastUpdateDate >= (select QBLastSync from Control)
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getMSMCustomers(GetMSMCustomersParam _GetMSMCustomers, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NULL ");

            //or LastUpdateDate >= (select QBLastSync from Control)
            try
            {
                return _GetMSMCustomers.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMCustomersMapping(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NULL and isnull(o.CreateDate, '01/01/1753') >= '05/09/2017' ");

            //and CreatedBy = 'MOM'");

            //or LastUpdateDate >= (select QBLastSync from Control)
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMLocation(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT qbcustomerid \n");
            varname1.Append("        FROM   Owner \n");
            varname1.Append("        WHERE  ID = o.Owner)   AS qbcustomerid, \n");
            varname1.Append("       o.tag, \n");
            varname1.Append("       o.Loc                   AS ID, \n");
            varname1.Append("       o.QBLocID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       o.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBlocTypeID \n");
            varname1.Append("        FROM   LocType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBlocTypeID, \n");
            varname1.Append("       (SELECT QBStaxID \n");
            varname1.Append("        FROM   stax t \n");
            varname1.Append("        WHERE  name = o.stax)AS QBstaxID, \n");
            varname1.Append("       o.Address               AS shipaddress, \n");
            varname1.Append("       o.City                  AS shipcity, \n");
            varname1.Append("       o.State                 AS shipstate, \n");
            varname1.Append("       o.Zip                   AS shipzip, \n");
            varname1.Append(" (SELECT QBCustomertypeID FROM   OType t WHERE  t.Type = (SELECT Owner.Type FROM   Owner WHERE  Owner.ID = o.Owner)) AS QBCustomertypeID \n");
            varname1.Append("FROM   Loc o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBLocID IS NULL ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getMSMLocation(GetMSMLocationParam _GetMSMLocation, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT qbcustomerid \n");
            varname1.Append("        FROM   Owner \n");
            varname1.Append("        WHERE  ID = o.Owner)   AS qbcustomerid, \n");
            varname1.Append("       o.tag, \n");
            varname1.Append("       o.Loc                   AS ID, \n");
            varname1.Append("       o.QBLocID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       o.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBlocTypeID \n");
            varname1.Append("        FROM   LocType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBlocTypeID, \n");
            varname1.Append("       (SELECT QBStaxID \n");
            varname1.Append("        FROM   stax t \n");
            varname1.Append("        WHERE  name = o.stax)AS QBstaxID, \n");
            varname1.Append("       o.Address               AS shipaddress, \n");
            varname1.Append("       o.City                  AS shipcity, \n");
            varname1.Append("       o.State                 AS shipstate, \n");
            varname1.Append("       o.Zip                   AS shipzip, \n");
            varname1.Append(" (SELECT QBCustomertypeID FROM   OType t WHERE  t.Type = (SELECT Owner.Type FROM   Owner WHERE  Owner.ID = o.Owner)) AS QBCustomertypeID \n");
            varname1.Append("FROM   Loc o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBLocID IS NULL ");

            try
            {
                return _GetMSMLocation.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMLocationMapping(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT qbcustomerid \n");
            varname1.Append("        FROM   Owner \n");
            varname1.Append("        WHERE  ID = o.Owner)   AS qbcustomerid, \n");
            varname1.Append("       o.tag, \n");
            varname1.Append("       o.Loc                   AS ID, \n");
            varname1.Append("       o.QBLocID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       o.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBlocTypeID \n");
            varname1.Append("        FROM   LocType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBlocTypeID, \n");
            varname1.Append("       (SELECT QBStaxID \n");
            varname1.Append("        FROM   stax t \n");
            varname1.Append("        WHERE  name = o.stax)AS QBstaxID, \n");
            varname1.Append("       o.Address               AS shipaddress, \n");
            varname1.Append("       o.City                  AS shipcity, \n");
            varname1.Append("       o.State                 AS shipstate, \n");
            varname1.Append("       o.Zip                   AS shipzip, \n");
            varname1.Append(" (SELECT QBCustomertypeID FROM   OType t WHERE  t.Type = (SELECT Owner.Type FROM   Owner WHERE  Owner.ID = o.Owner)) AS QBCustomertypeID \n");
            varname1.Append("FROM   Loc o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBLocID IS NULL and isnull(o.CreateDate, '01/01/1753') >= '05/09/2017'");

            // and CreatedBy = 'MOM'");
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBCustomers(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       r.LastUpdateDate, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID, \n");
            varname1.Append("       O.Type \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NOT NULL and LastUpdateDate >= (select QBLastSync from Control) and QBCustomerID like '________-__________'");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getQBCustomers(GetQBCustomersParam _GetQBCustomers, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       r.LastUpdateDate, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID, \n");
            varname1.Append("       O.Type \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NOT NULL and LastUpdateDate >= (select QBLastSync from Control) and QBCustomerID like '________-__________'");

            try
            {
                return _GetQBCustomers.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getCustomersSageAdd(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT o.ID, \n");
            //varname1.Append("       Substring(ownerID, 1, 10)                                        AS customer, \n");
            //varname1.Append("       Substring(r.NAME, 1, 50)                                         AS NAME, \n");
            //varname1.Append("       --Substring(r.Address, 1, 30)                                       AS Address1,   \n");
            //varname1.Append("       --Substring(r.Address, 31, 30)                                      AS Address2,   \n");
            //varname1.Append("       --Substring(r.Address, 61, 30)                                      AS Address3,   \n");
            //varname1.Append("       --Substring(r.Address, 91, 30)                                      AS Address4,   \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 1), 1, 30)                            AS Address1, \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 2), 1, 30)                            AS Address2, \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 3), 1, 30)                            AS Address3, \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 4), 1, 30)                            AS Address4, \n");
            //varname1.Append("       Substring(r.City, 1, 30)                                         AS City, \n");
            //varname1.Append("       Substring(r.Contact, 1, 15)                                      AS Contact, \n");
            //varname1.Append("       Substring(r.EMail, 1, 50)                                        AS EMail, \n");
            //varname1.Append("       Substring(r.Phone, 1, 15)                                        AS Phone, \n");
            //varname1.Append("       Remarks, \n");
            //varname1.Append("       Substring(r.State, 1, 4)                                         AS State, \n");
            //varname1.Append("       Substring(r.Zip, 1, 10)                                          AS Zip, \n");
            //varname1.Append("       CASE o.Status \n");
            //varname1.Append("         WHEN 0 THEN 'Active' \n");
            //varname1.Append("         ELSE 'Inactive' \n");
            //varname1.Append("       END                                                              AS Status, \n");
            //varname1.Append("       CASE Isnull(o.type, '') \n");
            //varname1.Append("         WHEN '' THEN 'Standard' \n");
            //varname1.Append("         ELSE Substring(o.Type, 1, 20) \n");
            //varname1.Append("       END                                                              AS Type \n");
            //varname1.Append("FROM   Owner o \n");
            //varname1.Append("       LEFT OUTER JOIN Rol r \n");
            //varname1.Append("                    ON o.Rol = r.ID \n");
            //varname1.Append("WHERE  Isnull(SageID, 'NA') = 'NA' \n");
            //varname1.Append("       AND Isnull(ownerID, '') <> '' ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "spGetCustomersSageAdd");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update owner set sageid = '" + objPropUser.Custom1 + "' where id=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocSageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update loc set sageid = '" + objPropUser.Custom1 + "' where loc=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomersSageUpdate(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT o.ID, \n");
            //varname1.Append("       Isnull(LastUpdateDate, '01/01/1900')  AS LastUpdateDate, \n");
            //varname1.Append("       Substring(r.NAME, 1, 50)              AS NAME, \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 1), 1, 30) AS Address1, \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 2), 1, 30) AS Address2, \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 3), 1, 30) AS Address3, \n");
            //varname1.Append("       Substring((SELECT items \n");
            //varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            //varname1.Append("                  WHERE  spl.id = 4), 1, 30) AS Address4, \n");
            //varname1.Append("       Substring(r.City, 1, 30)              AS City, \n");
            //varname1.Append("       Substring(r.Contact, 1, 15)           AS Contact, \n");
            //varname1.Append("       Substring(r.EMail, 1, 50)             AS EMail, \n");
            //varname1.Append("       Substring(r.Phone, 1, 15)             AS Phone, \n");
            //varname1.Append("       Remarks, \n");
            //varname1.Append("       Substring(r.State, 1, 4)              AS State, \n");
            //varname1.Append("       Substring(r.Zip, 1, 10)               AS Zip, \n");
            //varname1.Append("       CASE o.Status \n");
            //varname1.Append("         WHEN 0 THEN 'Active' \n");
            //varname1.Append("         ELSE 'Inactive' \n");
            //varname1.Append("       END                                   AS Status, \n");
            //varname1.Append("       CASE Isnull(o.type, '') \n");
            //varname1.Append("         WHEN '' THEN 'Standard' \n");
            //varname1.Append("         ELSE Substring(o.Type, 1, 20) \n");
            //varname1.Append("       END                                   AS type \n");
            //varname1.Append("FROM   Owner o \n");
            //varname1.Append("       LEFT OUTER JOIN Rol r \n");
            //varname1.Append("                    ON o.Rol = r.ID \n");
            //varname1.Append("WHERE  SageID IS NOT NULL \n");
            //varname1.Append("       AND LastUpdateDate >= (SELECT SageLastSync \n");
            //varname1.Append("                              FROM   Control) ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "spGetCustomersSageUpdate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomersForSageDelete(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select Id, SageID from owner where isnull(SageID,'') <> '' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsSageAdd(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spAddLocationsSage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getLocationsSageNA(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " SELECT Loc AS ID FROM Loc l WHERE SageID='NA' UPDATE LOC SET SageID=NULL  WHERE SageID='NA' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet geCustomersSageNA(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " SELECT ID, OwnerID FROM owner WHERE SageID='NA'  UPDATE OWNER SET SageID=NULL  WHERE SageID='NA' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsSageUpdate(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spUpdateLocationsSage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMCPTemplatesByMechanic(User objPropUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Worker";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = objPropUser.EmName;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TicketID";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPropUser.UpdateTicket;

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetMCPTemplatesByMechanic", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsForSageDelete(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select loc, SageID from loc where isnull(SageID,'') <> '' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBLocation(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT (SELECT qbcustomerid \n");
            //varname1.Append("        FROM   Owner \n");
            //varname1.Append("        WHERE  ID = o.Owner)   AS qbcustomerid, \n");
            //varname1.Append("       o.Loc                   AS ID, \n");
            //varname1.Append("       o.QBLocID, \n");
            //varname1.Append("       r.Address, \n");
            //varname1.Append("       o.Tag, \n");
            //varname1.Append("       r.Cellular, \n");
            //varname1.Append("       r.City, \n");
            //varname1.Append("       r.Contact, \n");
            //varname1.Append("       r.Country, \n");
            //varname1.Append("       r.EMail, \n");
            //varname1.Append("       r.Fax, \n");
            //varname1.Append("       r.Name, \n");
            //varname1.Append("       r.Phone, \n");
            //varname1.Append("       r.Remarks, \n");
            //varname1.Append("       r.State, \n");
            //varname1.Append("       r.Zip, \n");
            //varname1.Append("       o.Status, \n");
            //varname1.Append("       isnull(o.balance,0) as balance , \n");
            //varname1.Append("       r.LastUpdateDate, \n");
            //varname1.Append("       o.Address               AS shipaddress, \n");
            //varname1.Append("       o.City                  AS shipcity, \n");
            //varname1.Append("       o.State                 AS shipstate, \n");
            //varname1.Append("       o.Zip                   AS shipzip, \n");
            //varname1.Append("       (SELECT QBStaxID \n");
            //varname1.Append("        FROM   stax \n");
            //varname1.Append("        WHERE  name = o.stax)AS QBstaxID, \n");
            //varname1.Append("       (SELECT QBlocTypeID \n");
            //varname1.Append("        FROM   LocType t \n");
            //varname1.Append("        WHERE  t.Type = o.Type)AS QBlocTypeID ,\n");
            //varname1.Append("  (SELECT QBCustomertypeID FROM   OType t WHERE  t.Type = (SELECT Owner.Type FROM   Owner WHERE  Owner.ID = o.Owner)) AS QBCustomertypeID  \n");
            //varname1.Append("FROM   Loc o \n");
            //varname1.Append("       LEFT OUTER JOIN Rol r \n");
            //varname1.Append("                    ON o.Rol = r.ID \n");
            //varname1.Append("WHERE  QBLocID IS NOT NULL and LastUpdateDate >= (select QBLastSync from Control) \n");
            //varname1.Append("       AND QBLocID <> (SELECT qbcustomerid \n");/*For excluding the locations which doesnt exist in QB, which are same as parent.*/
            //varname1.Append("                       FROM   Owner \n");
            //varname1.Append("                       WHERE  ID = o.Owner) ");


            try
            {
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetQBLocation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getQBLocation(GetQBLocationParam _GetQBLocation, string ConnectionString)
        {
            try
            {
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
                return _GetQBLocation.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetQBLocation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getLocations(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocations", DBNull.Value, DBNull.Value, objPropUser.DBName, IsSalesAsigned, objPropUser.EN, objPropUser.UserID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {


                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetlocations", objPropUser.SearchBy, objPropUser.SearchValue, objPropUser.DBName, IsSalesAsigned, objPropUser.EN, objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsData(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetDynamicDataLocations", DBNull.Value, DBNull.Value, objPropUser.DBName, IsSalesAsigned, objPropUser.EN, objPropUser.UserID, objPropUser.inclInactive);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLocationsData(GetLocationsDataParam _GetLocationsData, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return _GetLocationsData.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetDynamicDataLocations", DBNull.Value, DBNull.Value, _GetLocationsData.DBName, IsSalesAsigned, _GetLocationsData.EN, _GetLocationsData.UserID, _GetLocationsData.inclInactive);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationDataSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {


                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetDynamicDataLocations", objPropUser.SearchBy, objPropUser.SearchValue, objPropUser.DBName, IsSalesAsigned, objPropUser.EN, objPropUser.UserID, objPropUser.inclInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLocationDataSearch(GetLocationDataSearchParam _GetLocationDataSearch, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {


                return _GetLocationDataSearch.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetDynamicDataLocations", _GetLocationDataSearch.SearchBy, _GetLocationDataSearch.SearchValue, _GetLocationDataSearch.DBName, IsSalesAsigned, _GetLocationDataSearch.EN, _GetLocationDataSearch.UserID, _GetLocationDataSearch.inclInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getUserSearch(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetUsers", objPropUser.SearchBy, objPropUser.SearchValue, objPropUser.DBName, objPropUser.IsSuper, objPropUser.Supervisor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomerSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomers", objPropUser.SearchBy, objPropUser.SearchValue, objPropUser.DBName, IsSalesAsigned, objPropUser.EN, objPropUser.UserID, objPropUser.inclInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getCustomerSearch(GetCustomerSearchParam _GetCustomerSearch, Int32 IsSalesAsigned, string ConnectionString)
        {
            try
            {

                return _GetCustomerSearch.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetCustomers", _GetCustomerSearch.SearchBy, _GetCustomerSearch.SearchValue, _GetCustomerSearch.DBName, IsSalesAsigned, _GetCustomerSearch.EN, _GetCustomerSearch.UserID, _GetCustomerSearch.inclInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomerAuto(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                string str = @"select distinct o.ID,r.Name,r.Address from [Owner] o left outer join Rol r on o.Rol=r.ID LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN where o.status=0 and ((NAME LIKE '%" + objPropUser.SearchValue + "%') OR (Address LIKE '%" + objPropUser.SearchValue + "%') OR (Phone LIKE '" + objPropUser.SearchValue + "%') OR (City LIKE '" + objPropUser.SearchValue + "%') OR (EMail LIKE '%" + objPropUser.SearchValue + "%'))";
                if (objPropUser.EN == 1)
                {
                    str += @" and  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;
                }
                if (IsSalesAsigned > 0)
                {
                    str += @" and o.ID in (select l.Owner from loc l where (
 Terr =(convert(nvarchar(10),(select isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + "))))) or isnull(terr2,0) =(convert(nvarchar(10),(select isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id= " + IsSalesAsigned + ")))))";
                }

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getAccountAuto(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select id,acct,fdesc, (acct+' : '+fdesc)account from chart  where status=0 and ((acct LIKE '" + objPropUser.SearchValue + "%') OR (fdesc LIKE '%" + objPropUser.SearchValue + "%'))");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomerAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            //String strBuilder = "select distinct o.ID as value,r.Name as label,(r.Contact+', '+r.Address+', '+r.City+', '+r.[State]+', '+r.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc] from [Owner] o left outer join Rol r on o.Rol=r.ID left outer join Loc l on l.Owner=o.ID  where ((l.tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (NAME LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')) order by name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomerSearch", objPropUser.SearchValue, 0, objPropUser.EN, objPropUser.UserID, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerWithInactive(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            //String strBuilder = "select distinct o.ID as value,r.Name as label,(r.Contact+', '+r.Address+', '+r.City+', '+r.[State]+', '+r.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc] from [Owner] o left outer join Rol r on o.Rol=r.ID left outer join Loc l on l.Owner=o.ID  where ((l.tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (NAME LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')) order by name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomerSearchWithInactive", objPropUser.SearchValue, 0, objPropUser.EN, objPropUser.UserID, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomerProspectAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            //String strBuilder = " select distinct 0 as prospect, o.ID as value,r.Name as label,(ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc] from [Owner] o left outer join Rol r on o.Rol=r.ID left outer join Loc l on l.Owner=o.ID  left outer join Rol rl on l.Rol=rl.ID  ";
            //strBuilder += " where ((r.NAME LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.state = '" + objPropUser.SearchValue.Replace("'", "''") + "') ";
            //strBuilder += "  OR (l.tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (l.ID LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (rl.Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (rl.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (rl.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')   OR (rl.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (dbo.RemoveSpecialChars(rl.Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (rl.EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (l.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (l.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')   OR (l.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (rl.state = '" + objPropUser.SearchValue.Replace("'", "''") + "') ) ";
            //strBuilder += " union ";
            //strBuilder += " select distinct 1 as prospect, o.ID as value,r.Name as label, (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc] from Prospect o left outer join Rol r on o.Rol=r.ID ";
            //strBuilder += " where ((NAME LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (state = '" + objPropUser.SearchValue.Replace("'", "''") + "')) order by r.name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomerSearch", objPropUser.SearchValue, 1, objPropUser.EN, objPropUser.UserID, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTaskContactsSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetTaskRolSearch", objPropUser.SearchValue, objPropUser.EN, objPropUser.UserID, IsSalesAsigned);//"spGetTaskContactsSearch"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContactsSearchbyRolid(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "SpgetContactsSearchbyRolid", objPropUser.SearchValue, objPropUser.RoleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                //string str = "select distinct l.loc as value,l.tag as label,(r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc] from loc l left outer join Rol r on l.Rol=r.ID  inner join owner o on o.id = l.owner where  r.type=4 and (((select top 1 name from rol where id=o.rol) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')) ";

                //if (objPropUser.CustomerID != 0)
                //{
                //    str += " and [owner]=" + objPropUser.CustomerID;
                //}

                //str += " order by tag ";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationSearch", objPropUser.SearchValue, objPropUser.CustomerID, objPropUser.EN, objPropUser.UserID, IsSalesAsigned);//[owner]=" + objPropUser.CustomerID + " and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLocationAutojquery(GetLocationAutojqueryParam _GetLocationAutojquery, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                //string str = "select distinct l.loc as value,l.tag as label,(r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc] from loc l left outer join Rol r on l.Rol=r.ID  inner join owner o on o.id = l.owner where  r.type=4 and (((select top 1 name from rol where id=o.rol) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')) ";

                //if (objPropUser.CustomerID != 0)
                //{
                //    str += " and [owner]=" + objPropUser.CustomerID;
                //}

                //str += " order by tag ";

                return _GetLocationAutojquery.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetLocationSearch", _GetLocationAutojquery.SearchValue, _GetLocationAutojquery.CustomerID, _GetLocationAutojquery.EN, _GetLocationAutojquery.UserID, IsSalesAsigned);//[owner]=" + objPropUser.CustomerID + " and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationWithInactive(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationSearchWithInactive", objPropUser.SearchValue, objPropUser.CustomerID, objPropUser.EN, objPropUser.UserID, IsSalesAsigned);//[owner]=" + objPropUser.CustomerID + " and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationProspectSearch(User objPropUser, Int32 IsSalesAsigned = 0, Int32 IsProspect = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationProspectSearch", objPropUser.SearchValue, objPropUser.CustomerID, objPropUser.EN, objPropUser.UserID, IsSalesAsigned, IsProspect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet MainSearch(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spMainSearch", objPropUser.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCompany(User objPropUser)
        {
            string strCommandText = "select companyname,Upper(dbname+':'+type) as dbname from tblcontrol ";

            if (!string.IsNullOrEmpty(objPropUser.DBName))
            {
                strCommandText += " where dbname in (" + objPropUser.DBName + ")";
            }

            strCommandText += " order by companyname ";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, strCommandText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCompany(string connectionString)
        {
            string strCommandText = "select companyname,Upper(dbname+':'+type) as dbname from tblcontrol ";

            //if (!string.IsNullOrEmpty(objPropUser.DBName))
            //{
            //    strCommandText += " where dbname in (" + objPropUser.DBName + ")";
            //}

            strCommandText += " order by companyname ";

            try
            {
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, strCommandText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetUserByID", objPropUser.UserID, objPropUser.TypeID, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getUserByID(GetUserByIdParam objPropUser, string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetUserByID", objPropUser.UserID, objPropUser.TypeID, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool getPRUserByID(User objPropUser)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(PR,0) from tbluser where ID='" + objPropUser.UserID + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getSMTPByUserID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetSTMP", objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSMTPByUserID(GetSMTPByUserIDParam objPropUser, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetSTMP", objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserLangByID(User objPropUser)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select lang from tbluser where fuser='" + objPropUser.Username + "'")).ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerByID(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomerByID", objPropUser.CustomerID, objPropUser.DBName, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getCustomerByID(GetCustomerByIDParam _GetCustomerByID, Int32 IsSalesAsigned, string ConnectionString)
        {
            try
            {
                return _GetCustomerByID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetCustomerByID", _GetCustomerByID.CustomerID, _GetCustomerByID.DBName, IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerLocationContacts(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomerLocationContacts", objPropUser.CustomerID, objPropUser.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerAddress(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select r.Name,r.City,r.State,r.Zip,r.Phone, r.Fax,r.Contact,r.Address,r.EMail,r.Country from rol r inner join Owner o on o.Rol=r.ID where o.ID=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCustomerEmail(User objPropUser)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(r.EMail,'') as email from owner o inner join Rol r on r.ID=o.Rol where o.ID=" + objPropUser.CustomerID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByCustomerID(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetLocationByCustID", objPropUser.CustomerID, objPropUser.DBName, objPropUser.RoleID);
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationByCustID", objPropUser.CustomerID, objPropUser.DBName, IsSalesAsigned);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getLocationActiveByCustomerID(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationActiveByCustID", objPropUser.CustomerID, IsSalesAsigned);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLocationByCustomerID(GetLocationByCustomerIDParam _GetLocationByCustomerID, string ConnectionString)
        {
            try
            {
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetLocationByCustID", objPropUser.CustomerID, objPropUser.DBName, objPropUser.RoleID);
                return _GetLocationByCustomerID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetLocationByCustID", _GetLocationByCustomerID.CustomerID, _GetLocationByCustomerID.DBName, _GetLocationByCustomerID.IsSalesAsigned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationByID", objPropUser.LocID, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString)
        {
            try
            {
                return _GetLocationByID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetLocationByID", _GetLocationByID.LocID, _GetLocationByID.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAllLocation(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Loc where Status=1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Loc where Loc=" + objPropUser.RolId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getGCandHowerLocID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "getGCandHowerLocID", objPropUser.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getGCandHowerLocID(GetGCandHowerLocIDParam _GetGCandHowerLocID, string ConnectionString)
        {
            try
            {
                return _GetGCandHowerLocID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "getGCandHowerLocID", _GetGCandHowerLocID.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByIDReport(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT ID, Tag AS Name, (l.Address+', '+l.City+', '+l.State+', '+l.Zip) AS AddressFull FROM Loc l WHERE l.Loc = " + objPropUser.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCategory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select type,isnull(Status,1) as Status from category where Status =1 or Status is null  order by Status, type");
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetCategory");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet getCategory(GetCategoryParam _GetCategory, string ConnectionString)
        {
            try
            {
                return _GetCategory.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select type,isnull(Status,1) as Status from category order by Status, type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String getDefaultCategory(User objPropUser)
        {
            String DefaultCat = "";
            try
            {
                return DefaultCat = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select type from category where  ISDefault=1"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API

        public String getDefaultCategory(GetDefaultCategoryParam _GetDefaultCategory, string ConnectionString)
        {
            String DefaultCat = "";
            try
            {
                return DefaultCat = Convert.ToString(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "select type from category where  ISDefault=1"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String getDefaultCategoryAPI(User objPropUser)
        {
            String DefaultCat = "";
            try
            {
                return DefaultCat = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetDefaultCategory"));
                // return DefaultCat = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "spGetDefaultCategory"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevUnit(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select unit,id from elev where loc =" + objPropUser.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getequipByID(User objPropUser)
        {
            try
            {
                // return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select (select tag from Loc where Loc=e.Loc) as location, Loc, Owner, Unit, fDesc, Type, Cat, Manuf, Serial, State, Since, Last, Price, Status, Building, Remarks, fGroup, Template, InstallBy, install, category from Elev e where ID=" + objPropUser.EquipID + "  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.Elev= " + objPropUser.EquipID    );
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetEquipByID", objPropUser.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getequipByID(GetequipByIDParam _GetequipByID, string ConnectionString)
        {
            try
            {
                // return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select (select tag from Loc where Loc=e.Loc) as location, Loc, Owner, Unit, fDesc, Type, Cat, Manuf, Serial, State, Since, Last, Price, Status, Building, Remarks, fGroup, Template, InstallBy, install, category from Elev e where ID=" + objPropUser.EquipID + "  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.Elev= " + objPropUser.EquipID    );
                return _GetequipByID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetEquipByID", _GetequipByID.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLeadEquipByID(User objPropUser)
        {
            try
            {
                // return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select (select tag from Loc where Loc=e.Loc) as location, Loc, Owner, Unit, fDesc, Type, Cat, Manuf, Serial, State, Since, Last, Price, Status, Building, Remarks, fGroup, Template, InstallBy, install, category from Elev e where ID=" + objPropUser.EquipID + "  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.Elev= " + objPropUser.EquipID    );
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLeadEquipByID", objPropUser.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLeadEquipByID(GetLeadEquipByIDParam _GetLeadEquipByID, string ConnectionString)
        {
            try
            {
                // return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select (select tag from Loc where Loc=e.Loc) as location, Loc, Owner, Unit, fDesc, Type, Cat, Manuf, Serial, State, Since, Last, Price, Status, Building, Remarks, fGroup, Template, InstallBy, install, category from Elev e where ID=" + objPropUser.EquipID + "  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.Elev= " + objPropUser.EquipID    );
                return _GetLeadEquipByID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetLeadEquipByID", _GetLeadEquipByID.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getequipREPDetails(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT CASE \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketDPDA \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 2 \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketO \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 0 \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketD \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 1 \n");
            varname1.Append("                 ELSE 0 \n");
            varname1.Append("               END AS comp)     AS comp, \n");
            //varname1.Append("        ( CASE \n");
            //varname1.Append("               WHEN EXISTS (SELECT 1 \n");
            //varname1.Append("                            FROM   TicketDPDA \n");
            //varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
            //varname1.Append("                                                        FROM   TicketDPDA \n");
            //varname1.Append("                                                        WHERE  ID = ticketID) \n");
            //varname1.Append("               WHEN EXISTS (SELECT 1 \n");
            //varname1.Append("                            FROM   TicketD \n");
            //varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
            //varname1.Append("                                                        FROM   TicketD \n");
            //varname1.Append("                                                        WHERE  ID = ticketID) \n");
            //varname1.Append("               ELSE 0 \n");
            //varname1.Append("             END ) as internet, ");
            varname1.Append("         (SELECT fDesc \n");
            varname1.Append("         FROM   tblWork w \n");
            varname1.Append("         WHERE  w.id = rd.fwork) AS fwork, \n");
            varname1.Append("         (SELECT fDesc \n");
            varname1.Append("         FROM   EquipTemp \n");
            varname1.Append("         WHERE  id = eti.EquipT) AS Template, \n");
            varname1.Append("         rd.Lastdate, \n");
            varname1.Append("         rd.NextDateDue, \n");
            varname1.Append("         rd.ticketID, \n");
            varname1.Append("         rd.Code, \n");
            varname1.Append("         eti.fDesc, \n");
            varname1.Append("         CASE eti.Frequency \n");
            varname1.Append("         WHEN 0 THEN 'Daily' \n");
            varname1.Append("         WHEN 1 THEN 'Weekly' \n");
            varname1.Append("         WHEN 2 THEN 'Bi-Weekly' \n");
            varname1.Append("         WHEN 3 THEN 'Monthly' \n");
            varname1.Append("         WHEN 4 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 5 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 6 THEN 'Semi-Annually ' \n");
            varname1.Append("         WHEN 7 THEN 'Annually' \n");
            varname1.Append("         WHEN 8 THEN 'One Time' \n");
            varname1.Append("         WHEN 9 THEN '3 Times a Year' \n");
            varname1.Append("         WHEN 10 THEN 'Every 2 Year' \n");
            varname1.Append("         WHEN 11 THEN 'Every 3 Year' \n");
            varname1.Append("         WHEN 12 THEN 'Every 5 Year' \n");
            varname1.Append("         WHEN 13 THEN 'Every 7 Year' \n");
            varname1.Append("         WHEN 14 THEN 'On-Demand' \n");
            varname1.Append("         END                      AS freq, \n");
            varname1.Append("         (select unit from elev e where e.id= eti.elev ) as equip, status, comment, section \n");
            varname1.Append("         FROM   RepDetail rd \n");
            varname1.Append("         inner JOIN EquipTItem eti \n");
            varname1.Append("         ON eti.ID = rd.EquipTItem ");
            //varname1.Append("       ON eti.Elev = rd.Elev and eti.Code=rd.Code       ");
            varname1.Append("         Where rd.id is not null \n");

            if (objPropUser.EquipID != 0)
                varname1.Append(" and  rd.Elev = " + objPropUser.EquipID + " \n");

            if (!string.IsNullOrEmpty(objPropUser.StartDate))
            {
                DateTime datetime;
                if (DateTime.TryParse(objPropUser.StartDate, out datetime))
                {
                    if (objPropUser.Status == 1)
                    {
                        varname1.Append(" and rd.NextDateDue >= '" + objPropUser.StartDate + "'");
                    }
                    else
                    {
                        varname1.Append(" and rd.LastDate >= '" + objPropUser.StartDate + "'");
                    }
                }
            }
            if (!string.IsNullOrEmpty(objPropUser.EndDate))
            {
                DateTime datetime;
                if (DateTime.TryParse(objPropUser.EndDate, out datetime))
                {
                    if (objPropUser.Status == 1)
                    {
                        varname1.Append(" and rd.NextDateDue <= '" + objPropUser.EndDate + "'");
                    }
                    else
                    {
                        varname1.Append(" and rd.LastDate <= '" + objPropUser.EndDate + "'");
                    }
                }
            }

            if (!string.IsNullOrEmpty(objPropUser.SearchBy))
            {
                if (!string.IsNullOrEmpty(objPropUser.SearchValue))
                {
                    if (objPropUser.SearchBy.Trim() == "rd.ticketID" || objPropUser.SearchBy.Trim() == "eti.frequency")
                        varname1.Append("and  " + objPropUser.SearchBy + " = " + objPropUser.SearchValue.Trim() + " \n");
                    else if (objPropUser.SearchBy.Trim() == "eti.fDesc")
                        varname1.Append("and  " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue.Trim() + "%' \n");
                    else if (objPropUser.SearchBy.Trim() == "fwork")
                        varname1.Append("and ( SELECT fDesc FROM tblWork w WHERE  w.id = rd.fwork) like '" + objPropUser.SearchValue.Trim() + "%' \n");
                    else if (objPropUser.SearchBy.Trim() == "template")
                        varname1.Append("and eti.EquipT = " + objPropUser.SearchValue.Trim() + " \n");
                    else
                        varname1.Append("and  " + objPropUser.SearchBy + " like '" + objPropUser.SearchValue.Trim() + "%' \n");
                }
            }

            if (objPropUser.Cust != 0)
            {
                varname1.Append("       AND ( CASE \n");
                varname1.Append(
                    "               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketDPDA \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
                varname1.Append("                                                        FROM   TicketDPDA \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketD \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
                varname1.Append("                                                        FROM   TicketD \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               ELSE 0 \n");
                varname1.Append("             END ) = 1  ");
                varname1.Append("       AND ( CASE \n");
                varname1.Append("               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketD \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(ClearCheck,0) \n");
                varname1.Append("                                                        FROM   TicketD \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               ELSE 0 \n");
                varname1.Append("             END ) = 1  ");
            }

            varname1.Append(" ORDER  BY rd.NextDateDue DESC ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getequipREPDetails(GetequipREPDetailsParam _GetequipREPDetails, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT CASE \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketDPDA \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 2 \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketO \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 0 \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketD \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 1 \n");
            varname1.Append("                 ELSE 0 \n");
            varname1.Append("               END AS comp)     AS comp, \n");
            //varname1.Append("        ( CASE \n");
            //varname1.Append("               WHEN EXISTS (SELECT 1 \n");
            //varname1.Append("                            FROM   TicketDPDA \n");
            //varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
            //varname1.Append("                                                        FROM   TicketDPDA \n");
            //varname1.Append("                                                        WHERE  ID = ticketID) \n");
            //varname1.Append("               WHEN EXISTS (SELECT 1 \n");
            //varname1.Append("                            FROM   TicketD \n");
            //varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
            //varname1.Append("                                                        FROM   TicketD \n");
            //varname1.Append("                                                        WHERE  ID = ticketID) \n");
            //varname1.Append("               ELSE 0 \n");
            //varname1.Append("             END ) as internet, ");
            varname1.Append("         (SELECT fDesc \n");
            varname1.Append("         FROM   tblWork w \n");
            varname1.Append("         WHERE  w.id = rd.fwork) AS fwork, \n");
            varname1.Append("         (SELECT fDesc \n");
            varname1.Append("         FROM   EquipTemp \n");
            varname1.Append("         WHERE  id = eti.EquipT) AS Template, \n");
            varname1.Append("         rd.Lastdate, \n");
            varname1.Append("         rd.NextDateDue, \n");
            varname1.Append("         rd.ticketID, \n");
            varname1.Append("         rd.Code, \n");
            varname1.Append("         eti.fDesc, \n");
            varname1.Append("         CASE eti.Frequency \n");
            varname1.Append("         WHEN 0 THEN 'Daily' \n");
            varname1.Append("         WHEN 1 THEN 'Weekly' \n");
            varname1.Append("         WHEN 2 THEN 'Bi-Weekly' \n");
            varname1.Append("         WHEN 3 THEN 'Monthly' \n");
            varname1.Append("         WHEN 4 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 5 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 6 THEN 'Semi-Annually ' \n");
            varname1.Append("         WHEN 7 THEN 'Annually' \n");
            varname1.Append("         WHEN 8 THEN 'One Time' \n");
            varname1.Append("         WHEN 9 THEN '3 Times a Year' \n");
            varname1.Append("         WHEN 10 THEN 'Every 2 Year' \n");
            varname1.Append("         WHEN 11 THEN 'Every 3 Year' \n");
            varname1.Append("         WHEN 12 THEN 'Every 5 Year' \n");
            varname1.Append("         WHEN 13 THEN 'Every 7 Year' \n");
            varname1.Append("         WHEN 14 THEN 'On-Demand' \n");
            varname1.Append("         END                      AS freq, \n");
            varname1.Append("         (select unit from elev e where e.id= eti.elev ) as equip, status, comment, section \n");
            varname1.Append("         FROM   RepDetail rd \n");
            varname1.Append("         inner JOIN EquipTItem eti \n");
            varname1.Append("         ON eti.ID = rd.EquipTItem ");
            //varname1.Append("       ON eti.Elev = rd.Elev and eti.Code=rd.Code       ");
            varname1.Append("         Where rd.id is not null \n");

            if (_GetequipREPDetails.EquipID != 0)
                varname1.Append(" and  rd.Elev = " + _GetequipREPDetails.EquipID + " \n");

            if (!string.IsNullOrEmpty(_GetequipREPDetails.StartDate))
            {
                DateTime datetime;
                if (DateTime.TryParse(_GetequipREPDetails.StartDate, out datetime))
                {
                    if (_GetequipREPDetails.Status == 1)
                    {
                        varname1.Append(" and rd.NextDateDue >= '" + _GetequipREPDetails.StartDate + "'");
                    }
                    else
                    {
                        varname1.Append(" and rd.LastDate >= '" + _GetequipREPDetails.StartDate + "'");
                    }
                }
            }
            if (!string.IsNullOrEmpty(_GetequipREPDetails.EndDate))
            {
                DateTime datetime;
                if (DateTime.TryParse(_GetequipREPDetails.EndDate, out datetime))
                {
                    if (_GetequipREPDetails.Status == 1)
                    {
                        varname1.Append(" and rd.NextDateDue <= '" + _GetequipREPDetails.EndDate + "'");
                    }
                    else
                    {
                        varname1.Append(" and rd.LastDate <= '" + _GetequipREPDetails.EndDate + "'");
                    }
                }
            }

            if (!string.IsNullOrEmpty(_GetequipREPDetails.SearchBy))
            {
                if (!string.IsNullOrEmpty(_GetequipREPDetails.SearchValue))
                {
                    if (_GetequipREPDetails.SearchBy.Trim() == "rd.ticketID" || _GetequipREPDetails.SearchBy.Trim() == "eti.frequency")
                        varname1.Append("and  " + _GetequipREPDetails.SearchBy + " = " + _GetequipREPDetails.SearchValue.Trim() + " \n");
                    else if (_GetequipREPDetails.SearchBy.Trim() == "eti.fDesc")
                        varname1.Append("and  " + _GetequipREPDetails.SearchBy + " like '%" + _GetequipREPDetails.SearchValue.Trim() + "%' \n");
                    else if (_GetequipREPDetails.SearchBy.Trim() == "fwork")
                        varname1.Append("and ( SELECT fDesc FROM tblWork w WHERE  w.id = rd.fwork) like '" + _GetequipREPDetails.SearchValue.Trim() + "%' \n");
                    else if (_GetequipREPDetails.SearchBy.Trim() == "template")
                        varname1.Append("and eti.EquipT = " + _GetequipREPDetails.SearchValue.Trim() + " \n");
                    else
                        varname1.Append("and  " + _GetequipREPDetails.SearchBy + " like '" + _GetequipREPDetails.SearchValue.Trim() + "%' \n");
                }
            }

            if (_GetequipREPDetails.Cust != 0)
            {
                varname1.Append("       AND ( CASE \n");
                varname1.Append(
                    "               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketDPDA \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
                varname1.Append("                                                        FROM   TicketDPDA \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketD \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
                varname1.Append("                                                        FROM   TicketD \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               ELSE 0 \n");
                varname1.Append("             END ) = 1  ");
                varname1.Append("       AND ( CASE \n");
                varname1.Append("               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketD \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(ClearCheck,0) \n");
                varname1.Append("                                                        FROM   TicketD \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               ELSE 0 \n");
                varname1.Append("             END ) = 1  ");
            }

            varname1.Append(" ORDER  BY rd.NextDateDue DESC ");

            try
            {
                return _GetequipREPDetails.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElev(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            string str = "select distinct e.state, e.cat,e.category,e.Classification,e.manuf,e.price,e.last,e.since, e.Install,e.id,e.unit,e.serial,e.type,e.fdesc, e.status " +
                ", CASE isnull(e.shut_down,0) WHEN 0 THEN 'No' ELSE 'Yes' END shut_down" +
                ", e.ShutdownReason ,e.building,r.EN,B.Name As Company,r.name,l.id as locid,l.tag ,(l.address+', '+l.city+', '+l.state+', '+l.zip) as address" +
                ", l.Loc,e.ID as unitid FROM elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id left Outer join Branch B on r.EN = B.ID ";
            if (objPropUser.EN == 1)
                str += " left Outer join tblUserCO UC on UC.CompanyID = B.ID ";
            str += "  WHERE e.id IS NOT NULL ";
            if (objPropUser.SearchBy != string.Empty)
            {
                if (objPropUser.SearchBy == "address")
                {
                    str += " and (l.address+', '+l.city+', '+l.state+', '+l.zip) like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "r.name" || objPropUser.SearchBy == "l.id" || objPropUser.SearchBy == "l.tag" || objPropUser.SearchBy == "l.state")
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "e.unit")
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "B.Name" && objPropUser.EN == 1)
                {
                    str += " and  UC.IsSel = 1  and r.EN = " + objPropUser.SearchValue + " and UC.UserID =" + objPropUser.UserID;
                }
                else if (objPropUser.SearchBy == "e.fdesc")
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "e.status")
                {
                    str += " and e.Status = " + objPropUser.SearchValue;
                }
                else
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
            }
            if (!string.IsNullOrEmpty(objPropUser.InstallDate))
            {
                str += " and e.since='" + objPropUser.InstallDate + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.InstallDateString))
            {
                str += " and CONVERT(date,e.since) " + objPropUser.InstallDateString + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.ServiceDate))
            {
                str += " and e.last ='" + objPropUser.ServiceDate + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.ServiceDateString))
            {
                str += " and CONVERT(date,e.last) " + objPropUser.ServiceDateString + "'";
            }
            if (objPropUser.Manufacturer != string.Empty)
            {
                str += " and e.manuf like '" + objPropUser.Manufacturer + "%'";
            }
            if (!string.IsNullOrEmpty(objPropUser.Price))
            {
                str += " and e.price='" + objPropUser.Price + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.PriceString))
            {
                str += " and e.price " + objPropUser.PriceString + "'";
            }
            if (objPropUser.LocID != 0)
            {
                str += " and e.loc=" + objPropUser.LocID + "";
            }
            if (objPropUser.CustomerID != 0)
            {
                str += " and e.owner=" + objPropUser.CustomerID + "";
            }

            if (objPropUser.RoleID != 0)
                str += " and isnull(l.roleid,0)=" + objPropUser.RoleID;

            if (!string.IsNullOrEmpty(objPropUser.Category))
                str += " and e.category = '" + objPropUser.Category + "'";

            if (!string.IsNullOrEmpty(objPropUser.Type))
                str += " and e.type = '" + objPropUser.Type + "'";

            if (!string.IsNullOrEmpty(objPropUser.Classification))
                str += " and e.Classification = '" + objPropUser.Classification + "'";

            if (objPropUser.Status != -1)
                str += " and e.status = " + objPropUser.Status;

            if (objPropUser.EN == 1)
                str += " and  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;

            if (!string.IsNullOrEmpty(objPropUser.building) && objPropUser.building != "All")
                str += " and e.building = '" + objPropUser.building + "'";

            if (IsSalesAsigned > 0)
            {
                str += "  and  ( l.Terr=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))) or  isnull(l.Terr2,0)=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))))";
            }

            if (objPropUser.IsAssignedProject == true)
            {
                str += " and l.Loc in (select loc from Job where ProjectManagerUserID=" + objPropUser.EmpId + ")";
            }


            str += " order by e.unit";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            string str = "select distinct e.state, e.cat,e.category,e.Classification,e.manuf,e.price,e.last,e.since, e.Install,e.id,e.unit,e.type,e.fdesc,e.status" +
                ", CASE isnull(e.shut_down,0) WHEN 0 THEN 'No' ELSE 'Yes' END shut_down" +
                ", e.ShutdownReason ,e.building,r.EN,B.Name As Company,r.name,l.id as locid,l.tag ,(l.address+', '+l.city+', '+l.state+', '+l.zip) as address" +
                ", l.Loc,e.ID as unitid FROM elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id left Outer join Branch B on r.EN = B.ID ";
            if (_GetElev.EN == 1)
                str += " left Outer join tblUserCO UC on UC.CompanyID = B.ID ";
            str += "  WHERE e.id IS NOT NULL ";
            if (_GetElev.SearchBy != string.Empty)
            {
                if (_GetElev.SearchBy == "address")
                {
                    str += " and (l.address+', '+l.city+', '+l.state+', '+l.zip) like '%" + _GetElev.SearchValue + "%'";
                }
                else if (_GetElev.SearchBy == "r.name" || _GetElev.SearchBy == "l.id" || _GetElev.SearchBy == "l.tag" || _GetElev.SearchBy == "l.state")
                {
                    str += " and " + _GetElev.SearchBy + " like '%" + _GetElev.SearchValue + "%'";
                }
                else if (_GetElev.SearchBy == "e.unit")
                {
                    str += " and " + _GetElev.SearchBy + " like '%" + _GetElev.SearchValue + "%'";
                }
                else if (_GetElev.SearchBy == "B.Name" && _GetElev.EN == 1)
                {
                    str += " and  UC.IsSel = 1  and r.EN = " + _GetElev.SearchValue + " and UC.UserID =" + _GetElev.UserID;
                }
                else if (_GetElev.SearchBy == "e.fdesc")
                {
                    str += " and " + _GetElev.SearchBy + " like '%" + _GetElev.SearchValue + "%'";
                }
                else if (_GetElev.SearchBy == "e.status")
                {
                    str += " and e.Status = " + _GetElev.SearchValue;
                }
                else
                {
                    str += " and " + _GetElev.SearchBy + " like '%" + _GetElev.SearchValue + "%'";
                }
            }
            if (!string.IsNullOrEmpty(_GetElev.InstallDate))
            {
                str += " and e.since='" + _GetElev.InstallDate + "'";
            }
            if (!string.IsNullOrEmpty(_GetElev.InstallDateString))
            {
                str += " and CONVERT(date,e.since) " + _GetElev.InstallDateString + "'";
            }
            if (!string.IsNullOrEmpty(_GetElev.ServiceDate))
            {
                str += " and e.last ='" + _GetElev.ServiceDate + "'";
            }
            if (!string.IsNullOrEmpty(_GetElev.ServiceDateString))
            {
                str += " and CONVERT(date,e.last) " + _GetElev.ServiceDateString + "'";
            }
            if (_GetElev.Manufacturer != string.Empty)
            {
                str += " and e.manuf like '" + _GetElev.Manufacturer + "%'";
            }
            if (!string.IsNullOrEmpty(_GetElev.Price))
            {
                str += " and e.price='" + _GetElev.Price + "'";
            }
            if (!string.IsNullOrEmpty(_GetElev.PriceString))
            {
                str += " and e.price " + _GetElev.PriceString + "'";
            }
            if (_GetElev.LocID != 0)
            {
                str += " and e.loc=" + _GetElev.LocID + "";
            }
            if (_GetElev.CustomerID != 0)
            {
                str += " and e.owner=" + _GetElev.CustomerID + "";
            }

            if (_GetElev.RoleID != 0)
                str += " and isnull(l.roleid,0)=" + _GetElev.RoleID;

            if (!string.IsNullOrEmpty(_GetElev.Category))
                str += " and e.category = '" + _GetElev.Category + "'";

            if (!string.IsNullOrEmpty(_GetElev.Type))
                str += " and e.type = '" + _GetElev.Type + "'";

            if (!string.IsNullOrEmpty(_GetElev.Classification))
                str += " and e.Classification = '" + _GetElev.Classification + "'";

            if (_GetElev.Status != -1)
                str += " and e.status = " + _GetElev.Status;

            if (_GetElev.EN == 1)
                str += " and  UC.IsSel = 1 and UC.UserID =" + _GetElev.UserID;

            if (!string.IsNullOrEmpty(_GetElev.building) && _GetElev.building != "All")
                str += " and e.building = '" + _GetElev.building + "'";

            if (IsSalesAsigned > 0)
            {
                str += "  and  ( l.Terr=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))) or  isnull(l.Terr2,0)=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))))";
            }

            if (_GetElev.IsAssignedProject == true)
            {
                str += " and l.Loc in (select loc from Job where ProjectManagerUserID=" + _GetElev.EmpId + ")";
            }


            str += " order by e.unit";

            try
            {
                return _GetElev.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLeadEquip(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            string str = "select distinct e.state, e.cat,e.category,e.Classification,e.manuf,e.price,e.last,e.since" +
                ", e.Install,e.id,e.unit,e.type,e.fdesc,e.status" +
                ", CASE isnull(e.shut_down,0) WHEN 0 THEN 'No' ELSE 'Yes' END shut_down" +
                ", e.ShutdownReason ,e.building,e.ID as unitid FROM LeadEquip e ";
            //if (objPropUser.EN == 1)
            //    str += " left Outer join tblUserCO UC on UC.CompanyID = B.ID ";
            str += "  WHERE e.id IS NOT NULL ";
            if (objPropUser.ProspectID != 0)
            {
                str += " and e.Lead=" + objPropUser.ProspectID + "";
            }
            //if (objPropUser.SearchBy != string.Empty)
            //{
            //    if (objPropUser.SearchBy == "address")
            //    {
            //        str += " and (l.address+', '+l.city+', '+l.state+', '+l.zip) like '%" + objPropUser.SearchValue + "%'";
            //    }
            //    else if (objPropUser.SearchBy == "r.name" || objPropUser.SearchBy == "l.id" || objPropUser.SearchBy == "l.tag" || objPropUser.SearchBy == "l.state")
            //    {
            //        str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
            //    }
            //    else if (objPropUser.SearchBy == "e.unit")
            //    {
            //        str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
            //    }
            //    else if (objPropUser.SearchBy == "B.Name" && objPropUser.EN == 1)
            //    {
            //        str += " and  UC.IsSel = 1  and r.EN = " + objPropUser.SearchValue + " and UC.UserID =" + objPropUser.UserID;
            //    }
            //    else if (objPropUser.SearchBy == "e.fdesc")
            //    {
            //        str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
            //    }
            //    else
            //    {
            //        str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
            //    }
            //}
            //if (!string.IsNullOrEmpty(objPropUser.InstallDate))
            //{
            //    str += " and e.since='" + objPropUser.InstallDate + "'";
            //}
            //if (!string.IsNullOrEmpty(objPropUser.InstallDateString))
            //{
            //    str += " and CONVERT(date,e.since) " + objPropUser.InstallDateString + "'";
            //}
            //if (!string.IsNullOrEmpty(objPropUser.ServiceDate))
            //{
            //    str += " and e.last ='" + objPropUser.ServiceDate + "'";
            //}
            //if (!string.IsNullOrEmpty(objPropUser.ServiceDateString))
            //{
            //    str += " and CONVERT(date,e.last) " + objPropUser.ServiceDateString + "'";
            //}
            //if (objPropUser.Manufacturer != string.Empty)
            //{
            //    str += " and e.manuf like '" + objPropUser.Manufacturer + "%'";
            //}
            //if (!string.IsNullOrEmpty(objPropUser.Price))
            //{
            //    str += " and e.price='" + objPropUser.Price + "'";
            //}
            //if (!string.IsNullOrEmpty(objPropUser.PriceString))
            //{
            //    str += " and e.price " + objPropUser.PriceString + "'";
            //}
            //if (objPropUser.LocID != 0)
            //{
            //    str += " and e.loc=" + objPropUser.LocID + "";
            //}
            //if (objPropUser.CustomerID != 0)
            //{
            //    str += " and e.owner=" + objPropUser.CustomerID + "";
            //}

            //if (objPropUser.RoleID != 0)
            //    str += " and isnull(l.roleid,0)=" + objPropUser.RoleID;

            //if (!string.IsNullOrEmpty(objPropUser.Category))
            //    str += " and e.category = '" + objPropUser.Category + "'";

            //if (!string.IsNullOrEmpty(objPropUser.Type))
            //    str += " and e.type = '" + objPropUser.Type + "'";

            //if (!string.IsNullOrEmpty(objPropUser.Classification))
            //    str += " and e.Classification = '" + objPropUser.Classification + "'";

            //if (objPropUser.Status != -1)
            //    str += " and e.status = " + objPropUser.Status;

            //if (objPropUser.EN == 1)
            //    str += " and  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;

            //if (!string.IsNullOrEmpty(objPropUser.building) && objPropUser.building != "All")
            //    str += " and e.building = '" + objPropUser.building + "'";

            //if (IsSalesAsigned > 0)
            //{
            //    str += "  and  ( l.Terr=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))) or  isnull(l.Terr2,0)=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))))";
            //}

            //str += " order by e.unit";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevSearch(User objPropUser)
        {
            string str = "select e.unit as label, e.id as value, e.state, e.cat,e.category,e.manuf,e.price,e.last,e.since, e.id,e.unit,e.type,e.fdesc,e.status, isnull(e.shut_down,0) shut_down, e.ShutdownReason ,l.tag ,l.ID as LID,e.Loc, l.Owner, (select top 1  name from rol where ID = (select top 1 rol from owner where ID = l.owner)) as custname FROM elev e INNER JOIN loc l ON l.Loc = e.Loc  WHERE e.id IS NOT NULL ";

            if (!string.IsNullOrEmpty(objPropUser.SearchValue))
            {
                str += " and e.state like '%" + objPropUser.SearchValue + "%'";
            }

            str += " order by e.unit";

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int AddUser(User objPropUser, string createdBy)
        {
            try
            {
                object objfire = DBNull.Value;
                object objhire = DBNull.Value;
                object objMerchant = DBNull.Value;
                object objSDate = DBNull.Value;
                object objEDate = DBNull.Value;
                object objDateOfBirth = DBNull.Value;
                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    objfire = objPropUser.DtFired;
                }
                if (objPropUser.DtHired != System.DateTime.MinValue)
                {
                    objhire = objPropUser.DtHired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }
                if (objPropUser.FStart != System.DateTime.MinValue)
                {
                    objSDate = objPropUser.FStart;
                }
                if (objPropUser.FEnd != System.DateTime.MinValue)
                {
                    objEDate = objPropUser.FEnd;
                }
                if (objPropUser.DBirth != System.DateTime.MinValue)
                {
                    objDateOfBirth = objPropUser.DBirth;
                }

                SqlParameter para = new SqlParameter();
                para.ParameterName = "@WageItems";
                para.SqlDbType = SqlDbType.Structured;
                para.Value = objPropUser.DtWage;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spAddUser",
                    objPropUser.Username,
                    objPropUser.Password,
                    objPropUser.PDA,
                    objPropUser.Field,
                    objPropUser.Status,
                    objPropUser.FirstName,
                    objPropUser.MiddleName,
                    objPropUser.LastNAme,
                    objPropUser.Address,
                    objPropUser.City,
                    objPropUser.State,
                    objPropUser.Zip,
                    objPropUser.Tele,
                    objPropUser.Cell,
                    objPropUser.Email,
                    objhire,
                    objfire,
                    objPropUser.CreateTicket,
                    objPropUser.WorkDate,
                    objPropUser.LocationRemarks,
                    objPropUser.ServiceHist,
                    objPropUser.PurchaseOrd,
                    objPropUser.Expenses,
                    objPropUser.ProgFunctions,
                    objPropUser.AccessUser,
                    objPropUser.Remarks,
                    objPropUser.Mapping,
                    objPropUser.Schedule,
                    objPropUser.DeviceID,
                    objPropUser.Pager,
                    objPropUser.Supervisor,
                    objPropUser.Salesperson,
                    objPropUser.UserLic,
                    objPropUser.UserLicID,
                    objPropUser.Lang,
                    objMerchant,
                    objPropUser.DefaultWorker,
                    objPropUser.Dispatch,
                    objPropUser.SalesMgr,
                    objPropUser.MassReview,
                    objPropUser.MOMUSer,
                    objPropUser.MOMPASS,
                    objPropUser.InServer,
                    "POP3",
                    objPropUser.InUsername,
                    objPropUser.InPassword,
                    objPropUser.InPort,
                    objPropUser.OutServer,
                    objPropUser.OutUsername,
                    objPropUser.OutPassword,
                    objPropUser.OutPort,
                    objPropUser.SSL,
                    objPropUser.BccEmail,
                    objPropUser.EmailAccount,
                    objPropUser.HourlyRate,
                    objPropUser.EmpMaintenance,
                    objPropUser.Timestampfix,
                    objPropUser.PayMethod,
                    objPropUser.PHours,
                    objPropUser.Salary,
                    objPropUser.Department,
                    objPropUser.EmpRefID,
                    objPropUser.PayPeriod,
                    objPropUser.MileageRate,
                    objPropUser.AddEquip,
                    objPropUser.EditEquip,
                    objPropUser.FChart,
                    objPropUser.AddChart,
                    objPropUser.EditChart,
                    objPropUser.ViewChart,
                    objPropUser.FGLAdj,
                    objPropUser.AddGLAdj,
                    objPropUser.EditGLAdj,
                    objPropUser.ViewGLAdj,
                    objPropUser.FDeposit,
                    objPropUser.AddDeposit,
                    objPropUser.EditDeposit,
                    objPropUser.ViewDeposit,
                    objPropUser.FCustomerPayment,
                    objPropUser.AddCustomerPayment,
                    objPropUser.EditCustomerPayment,
                    objPropUser.ViewCustomerPayment,
                    objPropUser.FinanStatement,
                    objSDate, objEDate,
                    objPropUser.APVendor,
                    objPropUser.APBill,
                    objPropUser.APBillPay,
                    objPropUser.APBillSelect,
                    para,
                    objPropUser.CustomerPermissions,
                    objPropUser.LocationrPermissions,
                    objPropUser.ProjectPermissions,
                    objPropUser.DeleteEquip,
                    objPropUser.ViewEquip,
                    objPropUser.MSAuthorisedDeviceOnly,
                    objPropUser.TicketDelete,
                    objPropUser.ProjectListPermission,
                    objPropUser.FinancePermission,
                    objPropUser.BOMPermission,
                    objPropUser.WIPPermission,
                    objPropUser.MilestonesPermission,
                    objPropUser.InventoryItemPermissions,
                    objPropUser.InventoryAdjustmentPermissions,
                    objPropUser.InventoryWarehousePermissions,
                    objPropUser.InventorysetupPermissions,
                    objPropUser.InventoryFinancePermissions,
                    objPropUser.DocumentPermissions,
                    objPropUser.ContactPermission,
                    objPropUser.SalesAssigned,
                    objPropUser.ProjectTempPermissions,
                    objPropUser.NotificationOnAddOpportunity,
                    objPropUser.VendorsPermission,
                    objPropUser.POLimit,
                    objPropUser.POApprove,
                    objPropUser.POApproveAmt,
                    objPropUser.Lng,
                    objPropUser.Lat,
                    objPropUser.Country,
                    objPropUser.authdevID,
                    objPropUser.EmNum,
                    objPropUser.EmName,
                    objPropUser.Title,
                    objPropUser.InvoivePermissions,
                    objPropUser.BillingCodesPermission,
                    objPropUser.POPermission,
                    objPropUser.Purchasingmodule,
                    objPropUser.Billingmodule,
                    objPropUser.RPOPermission,
                    objPropUser.TakeASentEmailCopy,
                    objPropUser.AccountPayablemodule,
                    objPropUser.PaymentHistoryPermission,
                    objPropUser.Customermodule,
                    objPropUser.ApplyPermissions,
                    objPropUser.DepositPermissions,
                    objPropUser.CollectionsPermissions,
                    objPropUser.Financialmodule,
                    objPropUser.ChartPermissions,
                    objPropUser.JournalEntryPermissions,
                    objPropUser.BankReconciliationPermissions,
                    objPropUser.Recurringmodule,
                    objPropUser.RecurringContractsPermission,
                    objPropUser.ProcessC,
                    objPropUser.ProcessT,
                    objPropUser.SafetyTestsPermission,
                    objPropUser.RenewEscalatePermission,
                    objPropUser.Schedulemodule,
                    objPropUser.ScheduleBoardPermission,
                    objPropUser.TicketPermission,
                    objPropUser.TicketResolvedPermission,
                    objPropUser.MTimesheetPermission,
                    objPropUser.ETimesheetPermission,
                    objPropUser.MapRPermission,
                    objPropUser.RouteBuilderPermission,
                    objPropUser.MassTimesheetCheck,
                    objPropUser.CreditHoldPermission,
                    objPropUser.CreditFlagPermission,
                    objPropUser.SSN,
                    objPropUser.Sex,
                    objDateOfBirth,
                    objPropUser.Race,
                    objPropUser.SalesPermission,
                    objPropUser.TasksPermission,
                    objPropUser.CompleteTasksPermission,
                    objPropUser.FollowUpPermission,
                    objPropUser.ProposalPermission,
                    objPropUser.EstimatePermission,
                    objPropUser.ConvertEstimatePermission,
                    objPropUser.SalesSetupPermission,
                    objPropUser.PONotification,
                    objPropUser.JobClosePermission,
                    objPropUser.Inventorymodule,
                    objPropUser.Projectmodule,
                    objPropUser.JobCompletedPermission,
                    objPropUser.JobReopenPermission,
                    objPropUser.wirteOff,
                    objPropUser.IsProjectManager,
                    objPropUser.IsAssignedProject,
                    objPropUser.IsReCalculateLaborExpense,
                    objPropUser.MinAmount,
                    objPropUser.MaxAmount,
                    createdBy,
                    objPropUser.TicketVoidPermission,
                    objPropUser.Employee,
                    objPropUser.PRProcess,
                    objPropUser.PRRegister,
                    objPropUser.PRReport,
                    objPropUser.PRWage,
                    objPropUser.PRDeduct,
                    objPropUser.PR,
                    objPropUser.RoleID,
                    objPropUser.ApplyUserRolePermission,
                    objPropUser.MassPayrollTicket,
                    objPropUser.ViolationPermission,
                    objPropUser.EstApproveProposal 
                    ));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateUser(User objPropUser, string UpdatedBy)
        {
            try
            {
                object obj = DBNull.Value;
                object objMerchant = DBNull.Value;
                object objSDate = DBNull.Value;
                object objEDate = DBNull.Value;
                object objDateOfBirth = DBNull.Value;
                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    obj = objPropUser.DtFired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }
                if (objPropUser.FStart != System.DateTime.MinValue)
                {
                    objSDate = objPropUser.FStart;
                }
                if (objPropUser.FEnd != System.DateTime.MinValue)
                {
                    objEDate = objPropUser.FEnd;
                }
                if (objPropUser.DBirth != System.DateTime.MinValue)
                {
                    objDateOfBirth = objPropUser.DBirth;
                }
                SqlParameter para = new SqlParameter();
                para.ParameterName = "@WageItems";
                para.SqlDbType = SqlDbType.Structured;
                para.Value = objPropUser.DtWage;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateUser"
                    , objPropUser.Username
                    , objPropUser.PDA
                    , objPropUser.Field
                    , objPropUser.Status
                    , objPropUser.FirstName
                    , objPropUser.MiddleName
                    , objPropUser.LastNAme
                    , objPropUser.Address
                    , objPropUser.City
                    , objPropUser.State
                    , objPropUser.Zip
                    , objPropUser.Tele
                    , objPropUser.Cell
                    , objPropUser.Email
                    , objPropUser.DtHired
                    , obj
                    , objPropUser.CreateTicket
                    , objPropUser.WorkDate
                    , objPropUser.LocationRemarks
                    , objPropUser.ServiceHist
                    , objPropUser.PurchaseOrd
                    , objPropUser.Expenses
                    , objPropUser.ProgFunctions
                    , objPropUser.AccessUser
                    , objPropUser.UserID
                    , objPropUser.RolId
                    , objPropUser.WorkId
                    , objPropUser.EmpId
                    , objPropUser.Mapping
                    , objPropUser.Schedule
                    , objPropUser.Password
                    , objPropUser.DeviceID
                    , objPropUser.Pager
                    , objPropUser.Supervisor
                    , objPropUser.Salesperson
                    , objPropUser.UserLic
                    , objPropUser.UserLicID
                    , objPropUser.Remarks
                    , objPropUser.Lang
                    , objMerchant
                    , objPropUser.DefaultWorker
                    , objPropUser.Dispatch
                    , objPropUser.SalesMgr
                    , objPropUser.MassReview
                    , objPropUser.MOMUSer
                    , objPropUser.MOMPASS
                    , objPropUser.InServer
                    , "POP3"
                    , objPropUser.InUsername
                    , objPropUser.InPassword
                    , objPropUser.InPort
                    , objPropUser.OutServer
                    , objPropUser.OutUsername
                    , objPropUser.OutPassword
                    , objPropUser.OutPort
                    , objPropUser.SSL
                    , objPropUser.BccEmail
                    , objPropUser.EmailAccount
                    , objPropUser.HourlyRate
                    , objPropUser.EmpMaintenance
                    , objPropUser.Timestampfix
                    , objPropUser.PayMethod
                    , objPropUser.PHours
                    , objPropUser.Salary
                    , objPropUser.Department
                    , objPropUser.EmpRefID
                    , objPropUser.PayPeriod
                    , objPropUser.MileageRate
                    , objPropUser.AddEquip
                    , objPropUser.EditEquip
                    , objPropUser.FChart
                    , objPropUser.FGLAdj
                    , objPropUser.AddChart
                    , objPropUser.EditChart
                    , objPropUser.ViewChart
                    , objPropUser.AddGLAdj
                    , objPropUser.EditGLAdj
                    , objPropUser.ViewGLAdj
                    , objPropUser.FDeposit
                    , objPropUser.AddDeposit
                    , objPropUser.EditDeposit
                    , objPropUser.ViewDeposit
                    , objPropUser.FCustomerPayment
                    , objPropUser.AddCustomerPayment
                    , objPropUser.EditCustomerPayment
                    , objPropUser.ViewCustomerPayment
                    , objPropUser.FinanStatement
                    , objSDate
                    , objEDate
                    , objPropUser.APVendor
                    , objPropUser.APBill
                    , objPropUser.APBillSelect
                    , objPropUser.APBillPay
                    , para
                    , objPropUser.CustomerPermissions
                    , objPropUser.LocationrPermissions
                    , objPropUser.ProjectPermissions
                    , objPropUser.DeleteEquip
                    , objPropUser.ViewEquip
                    , objPropUser.MSAuthorisedDeviceOnly
                    , objPropUser.TicketDelete
                    , objPropUser.ProjectListPermission
                    , objPropUser.FinancePermission
                    , objPropUser.BOMPermission
                    , objPropUser.WIPPermission
                    , objPropUser.MilestonesPermission
                    , objPropUser.InventoryItemPermissions
                    , objPropUser.InventoryAdjustmentPermissions
                    , objPropUser.InventoryWarehousePermissions
                    , objPropUser.InventorysetupPermissions
                    , objPropUser.InventoryFinancePermissions
                    , objPropUser.DocumentPermissions
                    , objPropUser.ContactPermission
                    , objPropUser.SalesAssigned
                    , objPropUser.ProjectTempPermissions
                    , objPropUser.NotificationOnAddOpportunity
                    , objPropUser.VendorsPermission
                    , objPropUser.POLimit
                    , objPropUser.POApprove
                    , objPropUser.POApproveAmt
                    , objPropUser.Lng
                    , objPropUser.Lat
                    , objPropUser.Country
                    , objPropUser.authdevID
                    , objPropUser.EmNum
                    , objPropUser.EmName
                    , objPropUser.Title
                    , objPropUser.InvoivePermissions
                    , objPropUser.BillingCodesPermission
                    , objPropUser.POPermission
                    , objPropUser.Purchasingmodule
                    , objPropUser.Billingmodule
                    , objPropUser.RPOPermission
                    , objPropUser.TakeASentEmailCopy
                   , objPropUser.AccountPayablemodule
                   , objPropUser.PaymentHistoryPermission
                   , objPropUser.Customermodule
                   , objPropUser.ApplyPermissions
                   , objPropUser.OnlinePaymentPermissions 
                   , objPropUser.DepositPermissions
                   , objPropUser.CollectionsPermissions
                   , objPropUser.Financialmodule
                   , objPropUser.ChartPermissions
                   , objPropUser.JournalEntryPermissions
                   , objPropUser.BankReconciliationPermissions
                   , objPropUser.Recurringmodule
                   , objPropUser.RecurringContractsPermission
                   , objPropUser.ProcessC
                   , objPropUser.ProcessT
                   , objPropUser.SafetyTestsPermission
                   , objPropUser.RenewEscalatePermission
                   , objPropUser.MinAmount
                   , objPropUser.MaxAmount
                   , objPropUser.Schedulemodule
                   , objPropUser.ScheduleBoardPermission
                   , objPropUser.TicketPermission
                   , objPropUser.TicketResolvedPermission
                   , objPropUser.MTimesheetPermission
                   , objPropUser.ETimesheetPermission
                   , objPropUser.MapRPermission
                   , objPropUser.RouteBuilderPermission
                   , objPropUser.MassTimesheetCheck
                   , objPropUser.CreditHoldPermission
                   , objPropUser.CreditFlagPermission
                   , objPropUser.SSN
                   , objPropUser.Sex
                   , objDateOfBirth
                   , objPropUser.Race
                   , objPropUser.SalesPermission
                   , objPropUser.TasksPermission
                   , objPropUser.CompleteTasksPermission
                   , objPropUser.FollowUpPermission
                   , objPropUser.ProposalPermission
                   , objPropUser.EstimatePermission
                   , objPropUser.ConvertEstimatePermission
                   , objPropUser.SalesSetupPermission
                   , objPropUser.PONotification
                   , objPropUser.JobClosePermission
                   , objPropUser.Inventorymodule
                   , objPropUser.Projectmodule
                   , objPropUser.JobCompletedPermission
                   , objPropUser.JobReopenPermission
                   , objPropUser.wirteOff
                    , objPropUser.IsProjectManager
                    , objPropUser.IsAssignedProject
                    , objPropUser.IsReCalculateLaborExpense
                    , UpdatedBy
                    , objPropUser.TicketVoidPermission
                    , objPropUser.Employee
                    , objPropUser.PRProcess,
                    objPropUser.PRRegister,
                    objPropUser.PRReport
                    , objPropUser.PRWage
                    , objPropUser.PRDeduct
                    , objPropUser.PR
                    , objPropUser.RoleID
                    , objPropUser.ApplyUserRolePermission
                    , objPropUser.MassPayrollTicket
                    , objPropUser.ViolationPermission
                    , objPropUser.EstApproveProposal
                    );

                UpdateUserCustomFieldsValue(objPropUser.ConnConfig, objPropUser.UserID, objPropUser.Cus_UserCustomValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTSUser(User objPropUser)
        {
            try
            {
                object obj = DBNull.Value;
                object objMerchant = DBNull.Value;
                object objSDate = DBNull.Value;
                object objEDate = DBNull.Value;

                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    obj = objPropUser.DtFired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }
                if (objPropUser.FStart != System.DateTime.MinValue)
                {
                    objSDate = objPropUser.FStart;
                }
                if (objPropUser.FEnd != System.DateTime.MinValue)
                {
                    objEDate = objPropUser.FEnd;
                }

                SqlParameter para = new SqlParameter();
                para.ParameterName = "@WageItems";
                para.SqlDbType = SqlDbType.Structured;
                para.Value = objPropUser.DtWage;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spupdateTSUser"
                    , objPropUser.Username
                    , objPropUser.PDA
                    , objPropUser.Field
                    , objPropUser.Status
                    , objPropUser.FirstName
                    , objPropUser.MiddleName
                    , objPropUser.LastNAme
                    , objPropUser.Address
                    , objPropUser.City
                    , objPropUser.State
                    , objPropUser.Zip
                    , objPropUser.Tele
                    , objPropUser.Cell
                    , objPropUser.Email
                    , objPropUser.DtHired
                    , obj
                    , objPropUser.CreateTicket
                    , objPropUser.WorkDate
                    , objPropUser.LocationRemarks
                    , objPropUser.ServiceHist
                    , objPropUser.PurchaseOrd
                    , objPropUser.Expenses
                    , objPropUser.ProgFunctions
                    , objPropUser.AccessUser
                    , objPropUser.UserID
                    , objPropUser.RolId
                    , objPropUser.WorkId
                    , objPropUser.EmpId
                    , objPropUser.Mapping
                    , objPropUser.Schedule
                    , objPropUser.Password
                    , objPropUser.DeviceID
                    , objPropUser.Pager
                    , objPropUser.Supervisor
                    , objPropUser.Salesperson
                    , objPropUser.UserLic
                    , objPropUser.UserLicID
                    , objPropUser.Remarks
                    , objPropUser.Lang
                    , objMerchant
                    , objPropUser.DefaultWorker
                    , objPropUser.Dispatch
                    , objPropUser.SalesMgr
                    , objPropUser.MassReview
                    , objPropUser.MOMUSer
                    , objPropUser.MOMPASS
                    , objPropUser.InServer
                    , "POP3"
                    , objPropUser.InUsername
                    , objPropUser.InPassword
                    , objPropUser.InPort
                    , objPropUser.OutServer
                    , objPropUser.OutUsername
                    , objPropUser.OutPassword
                    , objPropUser.OutPort
                    , objPropUser.SSL
                    , objPropUser.BccEmail
                    , objPropUser.EmailAccount
                    , objPropUser.HourlyRate
                    , objPropUser.EmpMaintenance
                    , objPropUser.Timestampfix
                    , objPropUser.PayMethod
                    , objPropUser.PHours
                    , objPropUser.Salary
                    , objPropUser.Department
                    , objPropUser.EmpRefID
                    , objPropUser.PayPeriod
                    , objPropUser.MileageRate
                    , objPropUser.AddEquip
                    , objPropUser.EditEquip
                    , objPropUser.FChart
                    , objPropUser.FGLAdj
                    , objPropUser.AddChart
                    , objPropUser.EditChart
                    , objPropUser.ViewChart
                    , objPropUser.AddGLAdj
                    , objPropUser.EditGLAdj
                    , objPropUser.ViewGLAdj
                    , objPropUser.FDeposit
                    , objPropUser.AddDeposit
                    , objPropUser.EditDeposit
                    , objPropUser.ViewDeposit
                    , objPropUser.FCustomerPayment
                    , objPropUser.AddCustomerPayment
                    , objPropUser.EditCustomerPayment
                    , objPropUser.ViewCustomerPayment
                    , objPropUser.FinanStatement
                    , objSDate
                    , objEDate
                    , objPropUser.APVendor
                    , objPropUser.APBill
                    , objPropUser.APBillSelect
                    , objPropUser.APBillPay
                    , para
                    , objPropUser.CustomerPermissions
                    , objPropUser.LocationrPermissions
                    , objPropUser.ProjectPermissions
                    , objPropUser.DeleteEquip
                    , objPropUser.ViewEquip
                    , objPropUser.MSAuthorisedDeviceOnly
                    , objPropUser.TicketDelete
                    , objPropUser.ProjectListPermission
                    , objPropUser.FinancePermission
                    , objPropUser.BOMPermission
                    , objPropUser.MilestonesPermission
                    , objPropUser.InventoryItemPermissions
                    , objPropUser.InventoryAdjustmentPermissions
                    , objPropUser.InventoryWarehousePermissions
                    , objPropUser.InventorysetupPermissions
                    , objPropUser.InventoryFinancePermissions
                    , objPropUser.DocumentPermissions
                    , objPropUser.ContactPermission
                    , objPropUser.SalesAssigned
                    , objPropUser.ProjectTempPermissions
                    , objPropUser.NotificationOnAddOpportunity
                    , objPropUser.VendorsPermission
                    , objPropUser.POLimit
                    , objPropUser.POApprove
                    , objPropUser.POApproveAmt
                    , objPropUser.Lng
                    , objPropUser.Lat
                    , objPropUser.Country
                    , objPropUser.authdevID
                    , objPropUser.EmNum
                    , objPropUser.EmName
                    , objPropUser.Title
                    , objPropUser.InvoivePermissions
                    , objPropUser.BillingCodesPermission
                    , objPropUser.POPermission
                    , objPropUser.Purchasingmodule
                    , objPropUser.Billingmodule
                    , objPropUser.RPOPermission
                    , objPropUser.TakeASentEmailCopy
                   , objPropUser.AccountPayablemodule
                   , objPropUser.PaymentHistoryPermission
                   , objPropUser.Customermodule
                   , objPropUser.ApplyPermissions
                   , objPropUser.DepositPermissions
                   , objPropUser.CollectionsPermissions
                   , objPropUser.Financialmodule
                   , objPropUser.ChartPermissions
                   , objPropUser.JournalEntryPermissions
                   , objPropUser.BankReconciliationPermissions
                   , objPropUser.Recurringmodule
                   , objPropUser.RecurringContractsPermission
                   , objPropUser.ProcessC
                   , objPropUser.ProcessT
                   , objPropUser.SafetyTestsPermission
                   , objPropUser.RenewEscalatePermission
                    , objPropUser.MinAmount
                    , objPropUser.MaxAmount
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateUserProfile(User objPropUser)
        {
            try
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName="@UserName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Username},
                    new SqlParameter{ParameterName="@Field",SqlDbType= SqlDbType.SmallInt,Value=objPropUser.Field},
                    new SqlParameter{ParameterName="@FName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.FirstName},
                    new SqlParameter{ParameterName="@MName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.MiddleName},
                    new SqlParameter{ParameterName="@LName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.LastNAme},
                    new SqlParameter{ParameterName="@Address",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Address},
                    new SqlParameter{ParameterName="@City",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.City},
                    new SqlParameter{ParameterName="@State",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.State},
                    new SqlParameter{ParameterName="@Zip",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Zip},
                    new SqlParameter{ParameterName="@Tel",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Tele},
                    new SqlParameter{ParameterName="@Cell",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Cell},
                    new SqlParameter{ParameterName="@Email",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Email},
                    new SqlParameter{ParameterName="@Pager",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Pager},
                    new SqlParameter{ParameterName="@UserID",SqlDbType= SqlDbType.Int,Value=objPropUser.UserID},
                    new SqlParameter{ParameterName="@Password",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Password},
                    new SqlParameter{ParameterName="@salesp",SqlDbType= SqlDbType.Int,Value=objPropUser.Salesperson},
                    new SqlParameter{ParameterName="@InServer",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.InServer},
                    new SqlParameter{ParameterName="@InServerType",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.InServerType},
                    new SqlParameter{ParameterName="@InUsername",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.InUsername},
                    new SqlParameter{ParameterName="@InPassword",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.InPassword},
                    new SqlParameter{ParameterName="@InPort",SqlDbType= SqlDbType.Int,Value=objPropUser.InPort},
                    new SqlParameter{ParameterName="@OutServer",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.OutServer},
                    new SqlParameter{ParameterName="@OutUsername",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.OutUsername},
                    new SqlParameter{ParameterName="@OutPassword",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.OutPassword},
                    new SqlParameter{ParameterName="@OutPort",SqlDbType= SqlDbType.Int,Value=objPropUser.OutPort},
                    new SqlParameter{ParameterName="@SSL",SqlDbType= SqlDbType.Bit,Value=objPropUser.SSL},
                    new SqlParameter{ParameterName="@BccEmail",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.BccEmail},
                    new SqlParameter{ParameterName="@EmailAccount",SqlDbType= SqlDbType.Int,Value=objPropUser.EmailAccount},
                    new SqlParameter{ParameterName="@Lng",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Lng},
                    new SqlParameter{ParameterName="@Lat",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Lat},
                    new SqlParameter{ParameterName="@Country",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Country},
                    new SqlParameter{ParameterName="@EmNum",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.EmNum},
                    new SqlParameter{ParameterName="@EmName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.EmName},
                    new SqlParameter{ParameterName="@Title",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Title},
                    new SqlParameter{ParameterName="@ProfileImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.ProfileImage},
                    new SqlParameter{ParameterName="@CoverImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.CoverImage},
                    new SqlParameter{ParameterName="@TakeASentEmailCopy",SqlDbType= SqlDbType.Bit,Value=objPropUser.TakeASentEmailCopy}
                };

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateUserProfile", param.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUserAvatar(User objPropUser)
        {
            try
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName="@UserName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Username},
                    new SqlParameter{ParameterName="@Field",SqlDbType= SqlDbType.SmallInt,Value=objPropUser.Field},
                    new SqlParameter{ParameterName="@UserID",SqlDbType= SqlDbType.Int,Value=objPropUser.UserID},
                    new SqlParameter{ParameterName="@ProfileImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.ProfileImage}
                };

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateUserAvatar", param.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUserCoverImage(User objPropUser)
        {
            try
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName="@UserName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Username},
                    new SqlParameter{ParameterName="@Field",SqlDbType= SqlDbType.SmallInt,Value=objPropUser.Field},
                    new SqlParameter{ParameterName="@UserID",SqlDbType= SqlDbType.Int,Value=objPropUser.UserID},
                    new SqlParameter{ParameterName="@CoverImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.CoverImage}
                };

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateUserCoverImage", param.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void UpdateUserCustomerAvatar(User objPropUser)
        //{
        //    try
        //    {
        //        var param = new List<SqlParameter>
        //        {
        //            new SqlParameter{ParameterName="@UserName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Username},
        //            new SqlParameter{ParameterName="@CustomerId",SqlDbType= SqlDbType.Int,Value=objPropUser.UserID},
        //            new SqlParameter{ParameterName="@ProfileImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.ProfileImage}
        //        };

        //        SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateUserCustomerAvatar", param.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void UpdateUserCustomerCoverImage(User objPropUser)
        //{
        //    try
        //    {
        //        var param = new List<SqlParameter>
        //        {
        //            new SqlParameter{ParameterName="@UserName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Username},
        //            new SqlParameter{ParameterName="@CustomerId",SqlDbType= SqlDbType.Int,Value=objPropUser.UserID},
        //            new SqlParameter{ParameterName="@CoverImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.CoverImage}
        //        };

        //        SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateUserCustomerCoverImage", param.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateUserCustomerProfile(User objPropUser)
        {
            try
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName="@UserName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Username},
                    new SqlParameter{ParameterName="@Password",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Password},
                    new SqlParameter{ParameterName="@FName",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.FirstName},
                    new SqlParameter{ParameterName="@Address",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Address},
                    new SqlParameter{ParameterName="@City",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.City},
                    new SqlParameter{ParameterName="@State",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.State},
                    new SqlParameter{ParameterName="@Zip",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Zip},
                    new SqlParameter{ParameterName="@country",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Country},
                    new SqlParameter{ParameterName="@Title",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Title},
                    new SqlParameter{ParameterName="@Lat",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Lat},
                    new SqlParameter{ParameterName="@Lng",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Lng},
                    new SqlParameter{ParameterName="@CustomerId",SqlDbType= SqlDbType.Int,Value=objPropUser.UserID},
                    new SqlParameter{ParameterName="@phone",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Tele},
                    new SqlParameter{ParameterName="@email",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Email},
                    new SqlParameter{ParameterName="@Cell",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.Cell},
                    new SqlParameter{ParameterName="@ProfileImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.ProfileImage},
                    new SqlParameter{ParameterName="@CoverImage",SqlDbType= SqlDbType.NVarChar,Value=objPropUser.CoverImage},

                };

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomerUserProfile", param.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddQBUser(User objPropUser)
        {
            try
            {
                object objfire = DBNull.Value;
                object objhire = DBNull.Value;
                object objMerchant = DBNull.Value;

                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    objfire = objPropUser.DtFired;
                }
                if (objPropUser.DtHired != System.DateTime.MinValue)
                {
                    objhire = objPropUser.DtHired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }


                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spAddQBUser", objPropUser.Username, objPropUser.Password, objPropUser.PDA, objPropUser.Field, objPropUser.Status, objPropUser.FirstName, objPropUser.MiddleName, objPropUser.LastNAme, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Cell, objPropUser.Email, objhire, objfire, objPropUser.CreateTicket, objPropUser.WorkDate, objPropUser.LocationRemarks, objPropUser.ServiceHist, objPropUser.PurchaseOrd, objPropUser.Expenses, objPropUser.ProgFunctions, objPropUser.AccessUser, objPropUser.Remarks, objPropUser.Mapping, objPropUser.Schedule, objPropUser.DeviceID, objPropUser.Pager, objPropUser.Supervisor, objPropUser.Salesperson, objPropUser.UserLic, objPropUser.UserLicID, objPropUser.Lang, objMerchant, objPropUser.QBEmployeeID, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddCustomer(User objPropUser)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[41];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Internet";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.Internet;

            para[14] = new SqlParameter();
            para[14].ParameterName = "contact";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.MainContact;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Phone";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Phone;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Website";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Website;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Email";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Email;

            para[18] = new SqlParameter();
            para[18].ParameterName = "cell";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Cell;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Type";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.Type;

            para[20] = new SqlParameter();
            para[20].ParameterName = "returnval";
            para[20].SqlDbType = SqlDbType.Int;
            para[20].Direction = ParameterDirection.ReturnValue;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Equipment";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = objPropUser.EquipID;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.AccountNo;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Billing";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = objPropUser.Billing;

            para[24] = new SqlParameter();
            para[24].ParameterName = "@grpbywo";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropUser.grpbyWO;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@openticket";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = objPropUser.openticket;

            para[26] = new SqlParameter();
            para[26].ParameterName = "BillRate";
            para[26].SqlDbType = SqlDbType.Decimal;
            para[26].Value = objPropUser.BillRate;

            para[27] = new SqlParameter();
            para[27].ParameterName = "OT";
            para[27].SqlDbType = SqlDbType.Decimal;
            para[27].Value = objPropUser.RateOT;

            para[28] = new SqlParameter();
            para[28].ParameterName = "NT";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = objPropUser.RateNT;

            para[29] = new SqlParameter();
            para[29].ParameterName = "DT";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = objPropUser.RateDT;

            para[30] = new SqlParameter();
            para[30].ParameterName = "Travel";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = objPropUser.RateTravel;

            para[31] = new SqlParameter();
            para[31].ParameterName = "Mileage";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropUser.MileageRate;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Fax";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = objPropUser.Fax;

            para[33] = new SqlParameter();
            para[33].ParameterName = "EN";
            para[33].SqlDbType = SqlDbType.Int;
            para[33].Value = objPropUser.EN;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Lat";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = objPropUser.Lat;

            para[35] = new SqlParameter();
            para[35].ParameterName = "Lng";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = objPropUser.Lng;

            para[36] = new SqlParameter();
            para[36].ParameterName = "@UpdatedBy";
            para[36].SqlDbType = SqlDbType.VarChar;
            para[36].Value = objPropUser.MOMUSer;

            para[37] = new SqlParameter();
            para[37].ParameterName = "Custom1";
            para[37].SqlDbType = SqlDbType.VarChar;
            para[37].Value = objPropUser.Custom1;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Custom2";
            para[38].SqlDbType = SqlDbType.VarChar;
            para[38].Value = objPropUser.Custom2;

            para[39] = new SqlParameter();
            para[39].ParameterName = "ShutdownAlert";
            para[39].SqlDbType = SqlDbType.SmallInt;
            para[39].Value = objPropUser.ShutdownAlert;

            para[40] = new SqlParameter();
            para[40].ParameterName = "ShutdownMessage";
            para[40].SqlDbType = SqlDbType.VarChar;
            para[40].Value = objPropUser.ShutdownReason;
            try
            {
                //custID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para));                
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para);
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddRol", para);

                custID = Convert.ToInt32(para[20].Value);
                objPropUser.CustomerID = custID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddCustomer(AddCustomerParam _AddCustomer, string ConnectionString)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[41];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _AddCustomer.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _AddCustomer.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = _AddCustomer.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = _AddCustomer.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _AddCustomer.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = _AddCustomer.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = _AddCustomer.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = _AddCustomer.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = _AddCustomer.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = _AddCustomer.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = _AddCustomer.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = _AddCustomer.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = _AddCustomer.ContactData;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Internet";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = _AddCustomer.Internet;

            para[14] = new SqlParameter();
            para[14].ParameterName = "contact";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = _AddCustomer.MainContact;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Phone";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _AddCustomer.Phone;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Website";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _AddCustomer.Website;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Email";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _AddCustomer.Email;

            para[18] = new SqlParameter();
            para[18].ParameterName = "cell";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _AddCustomer.Cell;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Type";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _AddCustomer.Type;

            para[20] = new SqlParameter();
            para[20].ParameterName = "returnval";
            para[20].SqlDbType = SqlDbType.Int;
            para[20].Direction = ParameterDirection.ReturnValue;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Equipment";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = _AddCustomer.EquipID;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = _AddCustomer.AccountNo;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Billing";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = _AddCustomer.Billing;

            para[24] = new SqlParameter();
            para[24].ParameterName = "@grpbywo";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = _AddCustomer.grpbyWO;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@openticket";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = _AddCustomer.openticket;

            para[26] = new SqlParameter();
            para[26].ParameterName = "BillRate";
            para[26].SqlDbType = SqlDbType.Decimal;
            para[26].Value = _AddCustomer.BillRate;

            para[27] = new SqlParameter();
            para[27].ParameterName = "OT";
            para[27].SqlDbType = SqlDbType.Decimal;
            para[27].Value = _AddCustomer.RateOT;

            para[28] = new SqlParameter();
            para[28].ParameterName = "NT";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = _AddCustomer.RateNT;

            para[29] = new SqlParameter();
            para[29].ParameterName = "DT";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = _AddCustomer.RateDT;

            para[30] = new SqlParameter();
            para[30].ParameterName = "Travel";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = _AddCustomer.RateTravel;

            para[31] = new SqlParameter();
            para[31].ParameterName = "Mileage";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = _AddCustomer.MileageRate;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Fax";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = _AddCustomer.Fax;

            para[33] = new SqlParameter();
            para[33].ParameterName = "EN";
            para[33].SqlDbType = SqlDbType.Int;
            para[33].Value = _AddCustomer.EN;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Lat";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = _AddCustomer.Lat;

            para[35] = new SqlParameter();
            para[35].ParameterName = "Lng";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = _AddCustomer.Lng;

            para[36] = new SqlParameter();
            para[36].ParameterName = "@UpdatedBy";
            para[36].SqlDbType = SqlDbType.VarChar;
            para[36].Value = _AddCustomer.MOMUSer;

            para[37] = new SqlParameter();
            para[37].ParameterName = "Custom1";
            para[37].SqlDbType = SqlDbType.VarChar;
            para[37].Value = _AddCustomer.Custom1;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Custom2";
            para[38].SqlDbType = SqlDbType.VarChar;
            para[38].Value = _AddCustomer.Custom2;


            para[39] = new SqlParameter();
            para[39].ParameterName = "ShutdownAlert";
            para[39].SqlDbType = SqlDbType.SmallInt;
            para[39].Value = _AddCustomer.ShutdownAlert;

            para[40] = new SqlParameter();
            para[40].ParameterName = "ShutdownMessage";
            para[40].SqlDbType = SqlDbType.VarChar;
            para[40].Value = _AddCustomer.ShutdownReason;
            try
            {

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddCustomer", para);

                custID = Convert.ToInt32(para[20].Value);
                _AddCustomer.CustomerID = custID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomerQB(User objPropUser)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[22];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustomerID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = objPropUser.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "SpAddQBcustomer", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddCustomerQB(AddCustomerQBParam _AddCustomerQB, string ConnectionString)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[22];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _AddCustomerQB.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _AddCustomerQB.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = _AddCustomerQB.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = _AddCustomerQB.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _AddCustomerQB.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = _AddCustomerQB.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = _AddCustomerQB.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = _AddCustomerQB.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = _AddCustomerQB.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = _AddCustomerQB.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = _AddCustomerQB.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = _AddCustomerQB.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = _AddCustomerQB.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = _AddCustomerQB.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = _AddCustomerQB.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _AddCustomerQB.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _AddCustomerQB.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _AddCustomerQB.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _AddCustomerQB.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustomerID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _AddCustomerQB.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = _AddCustomerQB.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = _AddCustomerQB.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "SpAddQBcustomer", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomerQBMapping(User objPropUser)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[23];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustomerID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = objPropUser.Balance;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBacctID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.QBAccountNumber;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "SpAddQBcustomerMapping", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddCustomerSage(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "SageKeyID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = objPropUser.Balance;

            para[22] = new SqlParameter();
            para[22].ParameterName = "returnval";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Direction = ParameterDirection.ReturnValue;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Customer";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropUser.Custom1;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "SpAddSagecustomer", para);
                int custid = 0;
                if (para[22].Value != DBNull.Value)
                {
                    custid = Convert.ToInt32(para[22].Value);
                }
                return custid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomertest(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddLocationTest", objPropUser.Address, objPropUser.City, objPropUser.Remarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSupervisorUser(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update tblwork set super = '" + objPropUser.Supervisor + "' where id =" + objPropUser.WorkId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 AddEquipment(User objPropUser)
        {
            SqlParameter[] param = new SqlParameter[26];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Loc";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = objPropUser.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = objPropUser.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = objPropUser.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = objPropUser.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = objPropUser.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = objPropUser.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = objPropUser.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = objPropUser.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@since";
            param[8].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = objPropUser.InstallDateTime;
            }

            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = objPropUser.LastServiceDate;
            }

            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = objPropUser.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = objPropUser.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@Remarks";
            param[12].SqlDbType = SqlDbType.Text;
            param[12].Value = objPropUser.Remarks;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Install";
            param[13].SqlDbType = SqlDbType.DateTime;

            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                param[13].Value = DBNull.Value;
            }
            else
            {
                param[13].Value = objPropUser.InstallDateimport;
            }

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Category";
            param[14].SqlDbType = SqlDbType.VarChar;
            param[14].Value = objPropUser.Category;

            param[15] = new SqlParameter();
            param[15].ParameterName = "@template";
            param[15].SqlDbType = SqlDbType.Int;
            param[15].Value = objPropUser.CustomTemplateID;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@UpdatedBy";
            param[16].SqlDbType = SqlDbType.VarChar;
            param[16].Value = objPropUser.MOMUSer;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@items";
            param[17].SqlDbType = SqlDbType.Structured;
            param[17].Value = objPropUser.DtItems;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@CustomItems";
            param[18].SqlDbType = SqlDbType.Structured;
            param[18].Value = objPropUser.dtcustom;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@EquipIDOut";
            param[19].SqlDbType = SqlDbType.Int;
            param[19].Value = 0;
            param[19].Direction = ParameterDirection.Output;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = objPropUser.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@Classification";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = objPropUser.Classification;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Shutdown";
            param[22].SqlDbType = SqlDbType.Bit;
            param[22].Value = objPropUser.Shutdown;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@ShutdownReason";
            param[23].SqlDbType = SqlDbType.VarChar;
            param[23].Value = objPropUser.ShutdownReason;
            if (objPropUser.Shutdown)
            {
                param[23].Value = objPropUser.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[23].Value = string.Empty;
            }

            param[24] = new SqlParameter();
            param[24].ParameterName = "@UserID";
            param[24].SqlDbType = SqlDbType.Int;
            param[24].Value = objPropUser.UserID;

            param[25] = new SqlParameter();
            param[25].ParameterName = "@ShutdownLongDesc";
            param[25].SqlDbType = SqlDbType.VarChar;
            if (objPropUser.Shutdown)
            {
                param[25].Value = objPropUser.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[25].Value = string.Empty;
            }

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddEquipment", param);
                return Convert.ToInt32(param[19].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public Int32 AddEquipment(AddEquipmentParam _AddEquipment, string ConnectionString)
        {
            SqlParameter[] param = new SqlParameter[26];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Loc";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = _AddEquipment.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = _AddEquipment.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = _AddEquipment.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = _AddEquipment.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = _AddEquipment.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = _AddEquipment.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = _AddEquipment.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = _AddEquipment.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@since";
            param[8].SqlDbType = SqlDbType.DateTime;
            if (_AddEquipment.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = _AddEquipment.InstallDateTime;
            }

            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (_AddEquipment.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = _AddEquipment.LastServiceDate;
            }

            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = _AddEquipment.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = _AddEquipment.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@Remarks";
            param[12].SqlDbType = SqlDbType.Text;
            param[12].Value = _AddEquipment.Remarks;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Install";
            param[13].SqlDbType = SqlDbType.DateTime;

            if (_AddEquipment.InstallDateimport == System.DateTime.MinValue)
            {
                param[13].Value = DBNull.Value;
            }
            else
            {
                param[13].Value = _AddEquipment.InstallDateimport;
            }

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Category";
            param[14].SqlDbType = SqlDbType.VarChar;
            param[14].Value = _AddEquipment.Category;

            param[15] = new SqlParameter();
            param[15].ParameterName = "@template";
            param[15].SqlDbType = SqlDbType.Int;
            param[15].Value = _AddEquipment.CustomTemplateID;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@UpdatedBy";
            param[16].SqlDbType = SqlDbType.VarChar;
            param[16].Value = _AddEquipment.MOMUSer;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@items";
            param[17].SqlDbType = SqlDbType.Structured;

            if (_AddEquipment.DtItems.Rows.Count > 0)
            {
                if (_AddEquipment.DtItems.Rows[0]["ID"].ToString() != "0")
                {
                    param[17].Value = _AddEquipment.DtItems;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Code", typeof(string));
                    //dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("EquipT", typeof(int));
                    dt.Columns.Add("Elev", typeof(int));
                    dt.Columns.Add("fDesc", typeof(string));
                    dt.Columns.Add("Line", typeof(int));
                    dt.Columns.Add("Lastdate", typeof(DateTime));
                    dt.Columns.Add("NextDateDue", typeof(DateTime));
                    dt.Columns.Add("Frequency", typeof(int));
                    dt.Columns.Add("Section", typeof(string));
                    dt.Columns.Add("Notes", typeof(string));
                    dt.Columns.Add("LeadEquip", typeof(int));
                    param[17].Value = dt;
                }
            }


            param[18] = new SqlParameter();
            param[18].ParameterName = "@CustomItems";
            param[18].SqlDbType = SqlDbType.Structured;

            if (_AddEquipment.dtcustom.Rows.Count > 0)
            {
                if (_AddEquipment.dtcustom.Rows[0]["ID"].ToString() != "0")
                {
                    param[18].Value = _AddEquipment.dtcustom;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("ElevT", typeof(int));
                    dt.Columns.Add("Elev", typeof(int));
                    dt.Columns.Add("fDesc", typeof(string));
                    dt.Columns.Add("Line", typeof(int));
                    dt.Columns.Add("value", typeof(string));
                    dt.Columns.Add("Format", typeof(string));
                    dt.Columns.Add("LastUpdated", typeof(string));
                    dt.Columns.Add("LastUpdateUser", typeof(string));
                    dt.Columns.Add("OrderNo", typeof(int));
                    dt.Columns.Add("LeadEquip", typeof(int));
                    param[18].Value = dt;
                }
            }


            param[19] = new SqlParameter();
            param[19].ParameterName = "@EquipIDOut";
            param[19].SqlDbType = SqlDbType.Int;
            param[19].Value = 0;
            param[19].Direction = ParameterDirection.Output;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = _AddEquipment.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@Classification";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = _AddEquipment.Classification;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Shutdown";
            param[22].SqlDbType = SqlDbType.Bit;
            param[22].Value = _AddEquipment.Shutdown;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@ShutdownReason";
            param[23].SqlDbType = SqlDbType.VarChar;
            param[23].Value = _AddEquipment.ShutdownReason;
            if (_AddEquipment.Shutdown)
            {
                param[23].Value = _AddEquipment.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[23].Value = string.Empty;
            }

            param[24] = new SqlParameter();
            param[24].ParameterName = "@UserID";
            param[24].SqlDbType = SqlDbType.Int;
            param[24].Value = _AddEquipment.UserID;

            param[25] = new SqlParameter();
            param[25].ParameterName = "@ShutdownLongDesc";
            param[25].SqlDbType = SqlDbType.VarChar;
            if (_AddEquipment.Shutdown)
            {
                param[25].Value = _AddEquipment.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[25].Value = string.Empty;
            }

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddEquipment", param);
                return Convert.ToInt32(param[19].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 AddEquipmentForLead(User objPropUser)
        {
            SqlParameter[] param = new SqlParameter[26];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Lead";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = objPropUser.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = objPropUser.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = objPropUser.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = objPropUser.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = objPropUser.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = objPropUser.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = objPropUser.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = objPropUser.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@since";
            param[8].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = objPropUser.InstallDateTime;
            }

            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = objPropUser.LastServiceDate;
            }

            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = objPropUser.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = objPropUser.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@Remarks";
            param[12].SqlDbType = SqlDbType.Text;
            param[12].Value = objPropUser.Remarks;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Install";
            param[13].SqlDbType = SqlDbType.DateTime;

            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                param[13].Value = DBNull.Value;
            }
            else
            {
                param[13].Value = objPropUser.InstallDateimport;
            }

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Category";
            param[14].SqlDbType = SqlDbType.VarChar;
            param[14].Value = objPropUser.Category;

            param[15] = new SqlParameter();
            param[15].ParameterName = "@template";
            param[15].SqlDbType = SqlDbType.Int;
            param[15].Value = objPropUser.CustomTemplateID;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@UpdatedBy";
            param[16].SqlDbType = SqlDbType.VarChar;
            param[16].Value = objPropUser.MOMUSer;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@items";
            param[17].SqlDbType = SqlDbType.Structured;
            param[17].Value = objPropUser.DtItems;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@CustomItems";
            param[18].SqlDbType = SqlDbType.Structured;
            param[18].Value = objPropUser.dtcustom;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@EquipIDOut";
            param[19].SqlDbType = SqlDbType.Int;
            param[19].Value = 0;
            param[19].Direction = ParameterDirection.Output;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = objPropUser.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@Classification";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = objPropUser.Classification;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Shutdown";
            param[22].SqlDbType = SqlDbType.Bit;
            param[22].Value = objPropUser.Shutdown;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@ShutdownReason";
            param[23].SqlDbType = SqlDbType.VarChar;
            param[23].Value = objPropUser.ShutdownReason;
            if (objPropUser.Shutdown)
            {
                param[23].Value = objPropUser.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[23].Value = string.Empty;
            }

            param[24] = new SqlParameter();
            param[24].ParameterName = "@UserID";
            param[24].SqlDbType = SqlDbType.Int;
            param[24].Value = objPropUser.UserID;

            param[25] = new SqlParameter();
            param[25].ParameterName = "@ShutdownLongDesc";
            param[25].SqlDbType = SqlDbType.VarChar;
            if (objPropUser.Shutdown)
            {
                param[25].Value = objPropUser.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[25].Value = string.Empty;
            }

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddLeadEquipment", param);
                return Convert.ToInt32(param[19].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public Int32 AddEquipmentForLead(AddEquipmentForLeadParam _AddEquipmentForLead, string ConnectionString)
        {
            SqlParameter[] param = new SqlParameter[26];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Lead";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = _AddEquipmentForLead.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = _AddEquipmentForLead.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = _AddEquipmentForLead.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = _AddEquipmentForLead.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = _AddEquipmentForLead.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = _AddEquipmentForLead.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = _AddEquipmentForLead.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = _AddEquipmentForLead.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@since";
            param[8].SqlDbType = SqlDbType.DateTime;
            if (_AddEquipmentForLead.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = _AddEquipmentForLead.InstallDateTime;
            }

            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (_AddEquipmentForLead.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = _AddEquipmentForLead.LastServiceDate;
            }

            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = _AddEquipmentForLead.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = _AddEquipmentForLead.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@Remarks";
            param[12].SqlDbType = SqlDbType.Text;
            param[12].Value = _AddEquipmentForLead.Remarks;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Install";
            param[13].SqlDbType = SqlDbType.DateTime;

            if (_AddEquipmentForLead.InstallDateimport == System.DateTime.MinValue)
            {
                param[13].Value = DBNull.Value;
            }
            else
            {
                param[13].Value = _AddEquipmentForLead.InstallDateimport;
            }

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Category";
            param[14].SqlDbType = SqlDbType.VarChar;
            param[14].Value = _AddEquipmentForLead.Category;

            param[15] = new SqlParameter();
            param[15].ParameterName = "@template";
            param[15].SqlDbType = SqlDbType.Int;
            param[15].Value = _AddEquipmentForLead.CustomTemplateID;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@UpdatedBy";
            param[16].SqlDbType = SqlDbType.VarChar;
            param[16].Value = _AddEquipmentForLead.MOMUSer;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@items";
            param[17].SqlDbType = SqlDbType.Structured;
            param[17].Value = _AddEquipmentForLead.DtItems;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@CustomItems";
            param[18].SqlDbType = SqlDbType.Structured;
            param[18].Value = _AddEquipmentForLead.dtcustom;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@EquipIDOut";
            param[19].SqlDbType = SqlDbType.Int;
            param[19].Value = 0;
            param[19].Direction = ParameterDirection.Output;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = _AddEquipmentForLead.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@Classification";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = _AddEquipmentForLead.Classification;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Shutdown";
            param[22].SqlDbType = SqlDbType.Bit;
            param[22].Value = _AddEquipmentForLead.Shutdown;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@ShutdownReason";
            param[23].SqlDbType = SqlDbType.VarChar;
            param[23].Value = _AddEquipmentForLead.ShutdownReason;
            if (_AddEquipmentForLead.Shutdown)
            {
                param[23].Value = _AddEquipmentForLead.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[23].Value = string.Empty;
            }

            param[24] = new SqlParameter();
            param[24].ParameterName = "@UserID";
            param[24].SqlDbType = SqlDbType.Int;
            param[24].Value = _AddEquipmentForLead.UserID;

            param[25] = new SqlParameter();
            param[25].ParameterName = "@ShutdownLongDesc";
            param[25].SqlDbType = SqlDbType.VarChar;
            if (_AddEquipmentForLead.Shutdown)
            {
                param[25].Value = _AddEquipmentForLead.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[25].Value = string.Empty;
            }

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddLeadEquipment", param);
                return Convert.ToInt32(param[19].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEquipmentImport(User objPropUser)
        {
            SqlParameter paraLastServ = new SqlParameter();
            paraLastServ.ParameterName = "Last";
            paraLastServ.SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                paraLastServ.Value = DBNull.Value;
            }
            else
            {
                paraLastServ.Value = objPropUser.LastServiceDate;
            }

            SqlParameter paraSince = new SqlParameter();
            paraSince.ParameterName = "since";
            paraSince.SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                paraSince.Value = DBNull.Value;
            }
            else
            {
                paraSince.Value = objPropUser.InstallDateTime;
            }

            SqlParameter paraInstalled = new SqlParameter();
            paraInstalled.ParameterName = "Install";
            paraInstalled.SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                paraInstalled.Value = DBNull.Value;
            }
            else
            {
                paraInstalled.Value = objPropUser.InstallDateimport;
            }



            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddEquipmentImport", objPropUser.Locationname, objPropUser.Unit, objPropUser.FirstName, objPropUser.Type, objPropUser.Cat, objPropUser.Manufacturer, objPropUser.Serial, objPropUser.UniqueID, paraSince, paraLastServ, objPropUser.EquipPrice, objPropUser.Status, objPropUser.Remarks, paraInstalled, objPropUser.Category);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEquipment(User objPropUser)
        {
            SqlParameter[] param = new SqlParameter[28];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Loc";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = objPropUser.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = objPropUser.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = objPropUser.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = objPropUser.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = objPropUser.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = objPropUser.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = objPropUser.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = objPropUser.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@Since";
            param[8].SqlDbType = SqlDbType.DateTime;

            if (objPropUser.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = objPropUser.InstallDateTime;
            }


            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = objPropUser.LastServiceDate;
            }


            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = objPropUser.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = objPropUser.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@ID";
            param[12].SqlDbType = SqlDbType.Int;
            param[12].Value = objPropUser.EquipID;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Remarks";
            param[13].SqlDbType = SqlDbType.Text;
            param[13].Value = objPropUser.Remarks;

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Install";
            param[14].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                param[14].Value = DBNull.Value;
            }
            else
            {
                param[14].Value = objPropUser.InstallDateimport;
            }

            param[15] = new SqlParameter();
            param[15].ParameterName = "@Category";
            param[15].SqlDbType = SqlDbType.VarChar;
            param[15].Value = objPropUser.Category;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@items";
            param[16].SqlDbType = SqlDbType.Structured;
            param[16].Value = objPropUser.DtItems;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@ItemsOnly";
            param[17].SqlDbType = SqlDbType.Int;
            param[17].Value = objPropUser.ItemsOnly;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@template";
            param[18].SqlDbType = SqlDbType.Int;
            param[18].Value = objPropUser.CustomTemplateID;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@CustomItems";
            param[19].SqlDbType = SqlDbType.Structured;
            param[19].Value = objPropUser.dtcustom;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = objPropUser.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@UpdatedBy";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = objPropUser.MOMUSer;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Classification";
            param[22].SqlDbType = SqlDbType.VarChar;
            param[22].Value = objPropUser.Classification;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@Shutdown";
            param[23].SqlDbType = SqlDbType.Bit;
            param[23].Value = objPropUser.Shutdown;

            param[24] = new SqlParameter();
            param[24].ParameterName = "@ShutdownReason";
            param[24].SqlDbType = SqlDbType.VarChar;
            if (objPropUser.Shutdown)
            {
                param[24].Value = objPropUser.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[24].Value = string.Empty;
            }

            param[25] = new SqlParameter();
            param[25].ParameterName = "@UserID";
            param[25].SqlDbType = SqlDbType.Int;
            param[25].Value = objPropUser.UserID;

            param[26] = new SqlParameter();
            param[26].ParameterName = "@ShutdownLongDesc";
            param[26].SqlDbType = SqlDbType.VarChar;
            //param[26].Value = objPropUser.ShutdownLongDesc;
            if (objPropUser.Shutdown)
            {
                param[26].Value = objPropUser.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[26].Value = string.Empty;
            }

            param[27] = new SqlParameter();
            param[27].ParameterName = "@PlannedShutdown";
            param[27].SqlDbType = SqlDbType.Bit;
            param[27].Value = objPropUser.PlannedShutdown;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateEquipment", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateEquipment(UpdateEquipmentParam _UpdateEquipment, string ConnectionString)
        {
            SqlParameter[] param = new SqlParameter[28];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Loc";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = _UpdateEquipment.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = _UpdateEquipment.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = _UpdateEquipment.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = _UpdateEquipment.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = _UpdateEquipment.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = _UpdateEquipment.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = _UpdateEquipment.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = _UpdateEquipment.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@Since";
            param[8].SqlDbType = SqlDbType.DateTime;

            if (_UpdateEquipment.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = _UpdateEquipment.InstallDateTime;
            }


            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (_UpdateEquipment.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = _UpdateEquipment.LastServiceDate;
            }


            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = _UpdateEquipment.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = _UpdateEquipment.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@ID";
            param[12].SqlDbType = SqlDbType.Int;
            param[12].Value = _UpdateEquipment.EquipID;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Remarks";
            param[13].SqlDbType = SqlDbType.Text;
            param[13].Value = _UpdateEquipment.Remarks;

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Install";
            param[14].SqlDbType = SqlDbType.DateTime;
            if (_UpdateEquipment.InstallDateimport == System.DateTime.MinValue)
            {
                param[14].Value = DBNull.Value;
            }
            else
            {
                param[14].Value = _UpdateEquipment.InstallDateimport;
            }

            param[15] = new SqlParameter();
            param[15].ParameterName = "@Category";
            param[15].SqlDbType = SqlDbType.VarChar;
            param[15].Value = _UpdateEquipment.Category;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@items";
            param[16].SqlDbType = SqlDbType.Structured;
            param[16].Value = _UpdateEquipment.DtItems;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@ItemsOnly";
            param[17].SqlDbType = SqlDbType.Int;
            param[17].Value = _UpdateEquipment.ItemsOnly;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@template";
            param[18].SqlDbType = SqlDbType.Int;
            param[18].Value = _UpdateEquipment.CustomTemplateID;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@CustomItems";
            param[19].SqlDbType = SqlDbType.Structured;
            param[19].Value = _UpdateEquipment.dtcustom;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = _UpdateEquipment.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@UpdatedBy";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = _UpdateEquipment.MOMUSer;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Classification";
            param[22].SqlDbType = SqlDbType.VarChar;
            param[22].Value = _UpdateEquipment.Classification;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@Shutdown";
            param[23].SqlDbType = SqlDbType.Bit;
            param[23].Value = _UpdateEquipment.Shutdown;

            param[24] = new SqlParameter();
            param[24].ParameterName = "@ShutdownReason";
            param[24].SqlDbType = SqlDbType.VarChar;
            if (_UpdateEquipment.Shutdown)
            {
                param[24].Value = _UpdateEquipment.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[24].Value = string.Empty;
            }

            param[25] = new SqlParameter();
            param[25].ParameterName = "@UserID";
            param[25].SqlDbType = SqlDbType.Int;
            param[25].Value = _UpdateEquipment.UserID;

            param[26] = new SqlParameter();
            param[26].ParameterName = "@ShutdownLongDesc";
            param[26].SqlDbType = SqlDbType.VarChar;
            //param[26].Value = _UpdateEquipment.ShutdownLongDesc;
            if (_UpdateEquipment.Shutdown)
            {
                param[26].Value = _UpdateEquipment.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[26].Value = string.Empty;
            }

            param[27] = new SqlParameter();
            param[27].ParameterName = "@PlannedShutdown";
            param[27].SqlDbType = SqlDbType.Bit;
            param[27].Value = _UpdateEquipment.PlannedShutdown;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateEquipment", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateLeadEquipment(User objPropUser)
        {
            SqlParameter[] param = new SqlParameter[27];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Lead";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = objPropUser.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = objPropUser.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = objPropUser.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = objPropUser.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = objPropUser.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = objPropUser.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = objPropUser.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = objPropUser.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@Since";
            param[8].SqlDbType = SqlDbType.DateTime;

            if (objPropUser.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = objPropUser.InstallDateTime;
            }


            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = objPropUser.LastServiceDate;
            }


            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = objPropUser.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = objPropUser.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@ID";
            param[12].SqlDbType = SqlDbType.Int;
            param[12].Value = objPropUser.EquipID;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Remarks";
            param[13].SqlDbType = SqlDbType.Text;
            param[13].Value = objPropUser.Remarks;

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Install";
            param[14].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                param[14].Value = DBNull.Value;
            }
            else
            {
                param[14].Value = objPropUser.InstallDateimport;
            }

            param[15] = new SqlParameter();
            param[15].ParameterName = "@Category";
            param[15].SqlDbType = SqlDbType.VarChar;
            param[15].Value = objPropUser.Category;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@items";
            param[16].SqlDbType = SqlDbType.Structured;
            param[16].Value = objPropUser.DtItems;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@ItemsOnly";
            param[17].SqlDbType = SqlDbType.Int;
            param[17].Value = objPropUser.ItemsOnly;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@template";
            param[18].SqlDbType = SqlDbType.Int;
            param[18].Value = objPropUser.CustomTemplateID;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@CustomItems";
            param[19].SqlDbType = SqlDbType.Structured;
            param[19].Value = objPropUser.dtcustom;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = objPropUser.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@UpdatedBy";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = objPropUser.MOMUSer;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Classification";
            param[22].SqlDbType = SqlDbType.VarChar;
            param[22].Value = objPropUser.Classification;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@Shutdown";
            param[23].SqlDbType = SqlDbType.Bit;
            param[23].Value = objPropUser.Shutdown;

            param[24] = new SqlParameter();
            param[24].ParameterName = "@ShutdownReason";
            param[24].SqlDbType = SqlDbType.VarChar;
            if (objPropUser.Shutdown)
            {
                param[24].Value = objPropUser.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[24].Value = string.Empty;
            }

            param[25] = new SqlParameter();
            param[25].ParameterName = "@UserID";
            param[25].SqlDbType = SqlDbType.Int;
            param[25].Value = objPropUser.UserID;

            param[26] = new SqlParameter();
            param[26].ParameterName = "@ShutdownLongDesc";
            param[26].SqlDbType = SqlDbType.VarChar;
            if (objPropUser.Shutdown)
            {
                param[26].Value = objPropUser.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[26].Value = string.Empty;
            }

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateLeadEquipment", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateLeadEquipment(UpdateLeadEquipmentParam _UpdateLeadEquipment, string ConnectionString)
        {
            SqlParameter[] param = new SqlParameter[27];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Lead";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = _UpdateLeadEquipment.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = _UpdateLeadEquipment.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = _UpdateLeadEquipment.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = _UpdateLeadEquipment.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = _UpdateLeadEquipment.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = _UpdateLeadEquipment.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = _UpdateLeadEquipment.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = _UpdateLeadEquipment.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@Since";
            param[8].SqlDbType = SqlDbType.DateTime;

            if (_UpdateLeadEquipment.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = _UpdateLeadEquipment.InstallDateTime;
            }


            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (_UpdateLeadEquipment.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = _UpdateLeadEquipment.LastServiceDate;
            }


            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = _UpdateLeadEquipment.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = _UpdateLeadEquipment.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@ID";
            param[12].SqlDbType = SqlDbType.Int;
            param[12].Value = _UpdateLeadEquipment.EquipID;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Remarks";
            param[13].SqlDbType = SqlDbType.Text;
            param[13].Value = _UpdateLeadEquipment.Remarks;

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Install";
            param[14].SqlDbType = SqlDbType.DateTime;
            if (_UpdateLeadEquipment.InstallDateimport == System.DateTime.MinValue)
            {
                param[14].Value = DBNull.Value;
            }
            else
            {
                param[14].Value = _UpdateLeadEquipment.InstallDateimport;
            }

            param[15] = new SqlParameter();
            param[15].ParameterName = "@Category";
            param[15].SqlDbType = SqlDbType.VarChar;
            param[15].Value = _UpdateLeadEquipment.Category;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@items";
            param[16].SqlDbType = SqlDbType.Structured;
            param[16].Value = _UpdateLeadEquipment.DtItems;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@ItemsOnly";
            param[17].SqlDbType = SqlDbType.Int;
            param[17].Value = _UpdateLeadEquipment.ItemsOnly;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@template";
            param[18].SqlDbType = SqlDbType.Int;
            param[18].Value = _UpdateLeadEquipment.CustomTemplateID;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@CustomItems";
            param[19].SqlDbType = SqlDbType.Structured;
            param[19].Value = _UpdateLeadEquipment.dtcustom;

            param[20] = new SqlParameter();
            param[20].ParameterName = "@Building";
            param[20].SqlDbType = SqlDbType.VarChar;
            param[20].Value = _UpdateLeadEquipment.building;

            param[21] = new SqlParameter();
            param[21].ParameterName = "@UpdatedBy";
            param[21].SqlDbType = SqlDbType.VarChar;
            param[21].Value = _UpdateLeadEquipment.MOMUSer;

            param[22] = new SqlParameter();
            param[22].ParameterName = "@Classification";
            param[22].SqlDbType = SqlDbType.VarChar;
            param[22].Value = _UpdateLeadEquipment.Classification;

            param[23] = new SqlParameter();
            param[23].ParameterName = "@Shutdown";
            param[23].SqlDbType = SqlDbType.Bit;
            param[23].Value = _UpdateLeadEquipment.Shutdown;

            param[24] = new SqlParameter();
            param[24].ParameterName = "@ShutdownReason";
            param[24].SqlDbType = SqlDbType.VarChar;
            if (_UpdateLeadEquipment.Shutdown)
            {
                param[24].Value = _UpdateLeadEquipment.ShutdownReason;
            }
            else// Remove description when return an equipment
            {
                param[24].Value = string.Empty;
            }

            param[25] = new SqlParameter();
            param[25].ParameterName = "@UserID";
            param[25].SqlDbType = SqlDbType.Int;
            param[25].Value = _UpdateLeadEquipment.UserID;

            param[26] = new SqlParameter();
            param[26].ParameterName = "@ShutdownLongDesc";
            param[26].SqlDbType = SqlDbType.VarChar;
            if (_UpdateLeadEquipment.Shutdown)
            {
                param[26].Value = _UpdateLeadEquipment.ShutdownLongDesc;
            }
            else// Remove description when return an equipment
            {
                param[26].Value = string.Empty;
            }

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateLeadEquipment", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddMassMCP(User objPropUser)
        {
            SqlParameter param = new SqlParameter()
            {
                ParameterName = "@items",
                SqlDbType = SqlDbType.Structured,
                Value = objPropUser.DtItems
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddEquipmentMCPItems", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddMassMCP(AddMassMCPParam _AddMassMCP, string ConnectionString)
        {
            SqlParameter param = new SqlParameter()
            {
                ParameterName = "@items",
                SqlDbType = SqlDbType.Structured,
                Value = _AddMassMCP.DtItems
            };

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddEquipmentMCPItems", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[62];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Route";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropUser.Route;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Terr";
            para[8].SqlDbType = SqlDbType.Int;
            para[8].Value = objPropUser.Territory;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "contactname";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.MainContact;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Phone";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Phone;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Fax";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Fax;

            para[13] = new SqlParameter();
            para[13].ParameterName = "cellular";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.Cell;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Email";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Email;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolAddress";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolAddress;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolCity";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolCity;

            para[18] = new SqlParameter();
            para[18].ParameterName = "RolState";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.RolState;

            para[19] = new SqlParameter();
            para[19].ParameterName = "RolZip";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.RolZip;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ContactData";
            para[20].SqlDbType = SqlDbType.Structured;
            para[20].Value = objPropUser.ContactData;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Owner";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropUser.CustomerID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Stax";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropUser.Stax;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Lat";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropUser.Lat;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Lng";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropUser.Lng;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Custom1";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropUser.Custom1;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Custom2";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropUser.Custom2;

            para[28] = new SqlParameter();
            para[28].ParameterName = "To";
            para[28].SqlDbType = SqlDbType.VarChar;
            para[28].Value = objPropUser.ToMail;

            para[29] = new SqlParameter();
            para[29].ParameterName = "CC";
            para[29].SqlDbType = SqlDbType.VarChar;
            para[29].Value = objPropUser.CCMail;

            para[30] = new SqlParameter();
            para[30].ParameterName = "ToInv";
            para[30].SqlDbType = SqlDbType.VarChar;
            para[30].Value = objPropUser.MailToInv;

            para[31] = new SqlParameter();
            para[31].ParameterName = "CCInv";
            para[31].SqlDbType = SqlDbType.VarChar;
            para[31].Value = objPropUser.MailCCInv;

            para[32] = new SqlParameter();
            para[32].ParameterName = "CreditHold";
            para[32].SqlDbType = SqlDbType.TinyInt;
            para[32].Value = objPropUser.CreditHold;

            para[33] = new SqlParameter();
            para[33].ParameterName = "DispAlert";
            para[33].SqlDbType = SqlDbType.TinyInt;
            para[33].Value = objPropUser.DispAlert;

            para[34] = new SqlParameter();
            para[34].ParameterName = "CreditReason";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = objPropUser.CreditReason;

            para[35] = new SqlParameter();
            para[35].ParameterName = "prospectID";
            para[35].SqlDbType = SqlDbType.Int;
            para[35].Value = objPropUser.ProspectID;

            para[37] = new SqlParameter();
            para[37].ParameterName = "ContractBill";
            para[37].SqlDbType = SqlDbType.TinyInt;
            para[37].Value = objPropUser.ContractBill;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Terms";
            para[38].SqlDbType = SqlDbType.Int;
            para[38].Value = objPropUser.TermsID;

            para[39] = new SqlParameter();
            para[39].ParameterName = "BillRate";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = objPropUser.BillRate;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OT";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = objPropUser.RateOT;

            para[41] = new SqlParameter();
            para[41].ParameterName = "NT";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = objPropUser.RateNT;

            para[42] = new SqlParameter();
            para[42].ParameterName = "DT";
            para[42].SqlDbType = SqlDbType.Decimal;
            para[42].Value = objPropUser.RateDT;

            para[43] = new SqlParameter();
            para[43].ParameterName = "Travel";
            para[43].SqlDbType = SqlDbType.Decimal;
            para[43].Value = objPropUser.RateTravel;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Mileage";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = objPropUser.MileageRate;

            para[45] = new SqlParameter();
            para[45].ParameterName = "returnval";
            para[45].SqlDbType = SqlDbType.Int;
            para[45].Direction = ParameterDirection.ReturnValue;

            para[46] = new SqlParameter();
            para[46].ParameterName = "tblGCandHomeOwner";
            para[46].SqlDbType = SqlDbType.Structured;
            para[46].Value = objPropUser.tblGCandHomeOwner;

            para[47] = new SqlParameter();
            para[47].ParameterName = "EmailInvoice";
            para[47].SqlDbType = SqlDbType.Bit;
            para[47].Value = objPropUser.EmailInvoice;

            para[48] = new SqlParameter();
            para[48].ParameterName = "PrintInvoice";
            para[48].SqlDbType = SqlDbType.Bit;
            para[48].Value = objPropUser.PrintInvoice;

            para[49] = new SqlParameter();
            para[49].ParameterName = "EN";
            para[49].SqlDbType = SqlDbType.Int;
            para[49].Value = objPropUser.EN;

            para[50] = new SqlParameter();
            para[50].ParameterName = "Terr2";
            para[50].SqlDbType = SqlDbType.Int;
            para[50].Value = objPropUser.Territory2;

            para[51] = new SqlParameter();
            para[51].ParameterName = "STax2";
            para[51].SqlDbType = SqlDbType.VarChar;
            para[51].Value = objPropUser.STax2;

            para[52] = new SqlParameter();
            para[52].ParameterName = "UTax";
            para[52].SqlDbType = SqlDbType.VarChar;
            para[52].Value = objPropUser.UTax;

            para[53] = new SqlParameter();
            para[53].ParameterName = "Zone";
            para[53].SqlDbType = SqlDbType.Int;
            para[53].Value = objPropUser.Zone;

            para[54] = new SqlParameter();
            para[54].ParameterName = "@UpdatedBy";
            para[54].SqlDbType = SqlDbType.VarChar;
            para[54].Value = objPropUser.MOMUSer;

            para[55] = new SqlParameter();
            para[55].ParameterName = "Consult";
            para[55].SqlDbType = SqlDbType.Int;
            para[55].Value = objPropUser.Consult;

            para[56] = new SqlParameter();
            para[56].ParameterName = "@Country";
            para[56].SqlDbType = SqlDbType.VarChar;
            para[56].Value = objPropUser.Country;

            para[57] = new SqlParameter();
            para[57].ParameterName = "@RolCountry";
            para[57].SqlDbType = SqlDbType.VarChar;
            para[57].Value = objPropUser.RolCountry;

            para[58] = new SqlParameter();
            para[58].ParameterName = "@NoCustomerStatement";
            para[58].SqlDbType = SqlDbType.Bit;
            para[58].Value = objPropUser.NoCustomerStatement;

            para[59] = new SqlParameter();
            para[59].ParameterName = "@BusinessTypeID";
            para[59].SqlDbType = SqlDbType.Int;
            para[59].Value = objPropUser.BusinessTypeID;

            para[60] = new SqlParameter();
            para[60].ParameterName = "CreditFlag";
            para[60].SqlDbType = SqlDbType.TinyInt;
            para[60].Value = objPropUser.CreditFlag;

            para[61] = new SqlParameter();
            para[61].ParameterName = "EstimateID";
            para[61].SqlDbType = SqlDbType.Int;
            para[61].Value = objPropUser.EstimateID;

            int locid = 0;
            try
            {
                locid = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddLocation", para));
                objPropUser.LocID = locid;
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddLocation", para);
                return Convert.ToInt32(para[45].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// UpdateLocGeocode
        /// </summary>
        /// <param name="_objLoc"></param>
        public void spUpdateLocGeocode(Loc _objLoc)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objLoc.LocID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Geocode";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _objLoc.geoCode;

                para[2] = new SqlParameter();
                para[2].ParameterName = "CreatedBy";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = _objLoc.MOMUSer;

                SqlHelper.ExecuteNonQuery(_objLoc.ConnConfig, "spUpdateLocGeocode", para);
                //SqlHelper.ExecuteNonQuery(_objPRReg.ConnConfig, CommandType.Text,  "UPDATE PRReg SET Ref = '" + _objPRReg.Ref + "' WHERE ID = " + _objPRReg.ID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public int AddLocation(AddLocationParam _AddLocation, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[60];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _AddLocation.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _AddLocation.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _AddLocation.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = _AddLocation.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _AddLocation.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = _AddLocation.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = _AddLocation.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Route";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = _AddLocation.Route;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Terr";
            para[8].SqlDbType = SqlDbType.Int;
            para[8].Value = _AddLocation.Territory;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = _AddLocation.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "contactname";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = _AddLocation.MainContact;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Phone";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = _AddLocation.Phone;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Fax";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = _AddLocation.Fax;

            para[13] = new SqlParameter();
            para[13].ParameterName = "cellular";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = _AddLocation.Cell;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Email";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = _AddLocation.Email;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _AddLocation.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolAddress";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _AddLocation.RolAddress;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolCity";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _AddLocation.RolCity;

            para[18] = new SqlParameter();
            para[18].ParameterName = "RolState";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _AddLocation.RolState;

            para[19] = new SqlParameter();
            para[19].ParameterName = "RolZip";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _AddLocation.RolZip;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ContactData";
            para[20].SqlDbType = SqlDbType.Structured;
            if (_AddLocation.ContactData.Rows.Count > 0)
            {
                if (_AddLocation.ContactData.Rows[0]["ContactID"].ToString() != "0")
                {
                    para[20].Value = _AddLocation.ContactData;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ContactID", typeof(int));
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("Phone", typeof(string));
                    dt.Columns.Add("Fax", typeof(string));
                    dt.Columns.Add("Cell", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("Title", typeof(string));
                    dt.Columns.Add("EmailTicket", typeof(bool));
                    dt.Columns.Add("EmailRecInvoice", typeof(bool));
                    dt.Columns.Add("ShutdownAlert", typeof(bool));
                    dt.Columns.Add("EmailRecTestProp", typeof(bool));
                    para[20].Value = dt;
                }
            }
            //para[20].Value = _AddLocation.ContactData;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = _AddLocation.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Owner";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = _AddLocation.CustomerID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Stax";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = _AddLocation.Stax;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Lat";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = _AddLocation.Lat;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Lng";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = _AddLocation.Lng;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Custom1";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = _AddLocation.Custom1;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Custom2";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = _AddLocation.Custom2;

            para[28] = new SqlParameter();
            para[28].ParameterName = "To";
            para[28].SqlDbType = SqlDbType.VarChar;
            para[28].Value = _AddLocation.ToMail;

            para[29] = new SqlParameter();
            para[29].ParameterName = "CC";
            para[29].SqlDbType = SqlDbType.VarChar;
            para[29].Value = _AddLocation.ToMail;

            para[30] = new SqlParameter();
            para[30].ParameterName = "ToInv";
            para[30].SqlDbType = SqlDbType.VarChar;
            para[30].Value = _AddLocation.MailToInv;

            para[31] = new SqlParameter();
            para[31].ParameterName = "CCInv";
            para[31].SqlDbType = SqlDbType.VarChar;
            para[31].Value = _AddLocation.MailCCInv;

            para[32] = new SqlParameter();
            para[32].ParameterName = "CreditHold";
            para[32].SqlDbType = SqlDbType.TinyInt;
            para[32].Value = _AddLocation.CreditHold;

            para[33] = new SqlParameter();
            para[33].ParameterName = "DispAlert";
            para[33].SqlDbType = SqlDbType.TinyInt;
            para[33].Value = _AddLocation.DispAlert;

            para[34] = new SqlParameter();
            para[34].ParameterName = "CreditReason";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = _AddLocation.CreditReason;

            para[35] = new SqlParameter();
            para[35].ParameterName = "prospectID";
            para[35].SqlDbType = SqlDbType.Int;
            para[35].Value = _AddLocation.ProspectID;

            para[37] = new SqlParameter();
            para[37].ParameterName = "ContractBill";
            para[37].SqlDbType = SqlDbType.TinyInt;
            para[37].Value = _AddLocation.ContractBill;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Terms";
            para[38].SqlDbType = SqlDbType.Int;
            para[38].Value = _AddLocation.TermsID;

            //para[23] = new SqlParameter();
            //para[23].ParameterName = "MAPAddress";
            //para[23].SqlDbType = SqlDbType.VarChar;
            //para[23].Value = _AddLocation.MAPAddress;
            para[39] = new SqlParameter();
            para[39].ParameterName = "BillRate";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = _AddLocation.BillRate;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OT";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = _AddLocation.RateOT;

            para[41] = new SqlParameter();
            para[41].ParameterName = "NT";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = _AddLocation.RateNT;

            para[42] = new SqlParameter();
            para[42].ParameterName = "DT";
            para[42].SqlDbType = SqlDbType.Decimal;
            para[42].Value = _AddLocation.RateDT;

            para[43] = new SqlParameter();
            para[43].ParameterName = "Travel";
            para[43].SqlDbType = SqlDbType.Decimal;
            para[43].Value = _AddLocation.RateTravel;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Mileage";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = _AddLocation.MileageRate;

            para[45] = new SqlParameter();
            para[45].ParameterName = "returnval";
            para[45].SqlDbType = SqlDbType.Int;
            para[45].Direction = ParameterDirection.ReturnValue;

            para[46] = new SqlParameter();
            para[46].ParameterName = "tblGCandHomeOwner";
            para[46].SqlDbType = SqlDbType.Structured;

            if (_AddLocation.tblGCandHomeOwner.Rows.Count > 0)
            {
                if (_AddLocation.tblGCandHomeOwner.Rows[0]["ID"].ToString() != "0")
                {
                    para[46].Value = _AddLocation.tblGCandHomeOwner;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("NAME", typeof(string));
                    dt.Columns.Add("City", typeof(string));
                    dt.Columns.Add("State", typeof(string));
                    dt.Columns.Add("Zip", typeof(string));
                    dt.Columns.Add("Phone", typeof(string));
                    dt.Columns.Add("Fax", typeof(string));
                    dt.Columns.Add("Contact", typeof(string));
                    dt.Columns.Add("Remarks", typeof(string));
                    dt.Columns.Add("Country", typeof(string));
                    dt.Columns.Add("Cellular", typeof(string));
                    dt.Columns.Add("EMail", typeof(string));
                    dt.Columns.Add("Type", typeof(int));
                    dt.Columns.Add("Address", typeof(string));
                    para[46].Value = dt;
                }
            }

            para[47] = new SqlParameter();
            para[47].ParameterName = "EmailInvoice";
            para[47].SqlDbType = SqlDbType.Bit;
            para[47].Value = _AddLocation.EmailInvoice;

            para[48] = new SqlParameter();
            para[48].ParameterName = "PrintInvoice";
            para[48].SqlDbType = SqlDbType.Bit;
            para[48].Value = _AddLocation.PrintInvoice;

            para[49] = new SqlParameter();
            para[49].ParameterName = "EN";
            para[49].SqlDbType = SqlDbType.Int;
            para[49].Value = _AddLocation.EN;

            para[50] = new SqlParameter();
            para[50].ParameterName = "Terr2";
            para[50].SqlDbType = SqlDbType.Int;
            para[50].Value = _AddLocation.Territory2;

            para[51] = new SqlParameter();
            para[51].ParameterName = "STax2";
            para[51].SqlDbType = SqlDbType.VarChar;
            para[51].Value = _AddLocation.STax2;

            para[52] = new SqlParameter();
            para[52].ParameterName = "UTax";
            para[52].SqlDbType = SqlDbType.VarChar;
            para[52].Value = _AddLocation.UTax;

            para[53] = new SqlParameter();
            para[53].ParameterName = "Zone";
            para[53].SqlDbType = SqlDbType.Int;
            para[53].Value = _AddLocation.Zone;

            para[54] = new SqlParameter();
            para[54].ParameterName = "@UpdatedBy";
            para[54].SqlDbType = SqlDbType.VarChar;
            para[54].Value = _AddLocation.MOMUSer;

            para[55] = new SqlParameter();
            para[55].ParameterName = "Consult";
            para[55].SqlDbType = SqlDbType.Int;
            para[55].Value = _AddLocation.Consult;

            para[56] = new SqlParameter();
            para[56].ParameterName = "@Country";
            para[56].SqlDbType = SqlDbType.VarChar;
            para[56].Value = _AddLocation.Country;

            para[57] = new SqlParameter();
            para[57].ParameterName = "@RolCountry";
            para[57].SqlDbType = SqlDbType.VarChar;
            para[57].Value = _AddLocation.RolCountry;

            para[58] = new SqlParameter();
            para[58].ParameterName = "@NoCustomerStatement";
            para[58].SqlDbType = SqlDbType.Bit;
            para[58].Value = _AddLocation.NoCustomerStatement;

            para[59] = new SqlParameter();
            para[59].ParameterName = "@BusinessTypeID";
            para[59].SqlDbType = SqlDbType.Int;
            para[59].Value = _AddLocation.BusinessTypeID;

            int locid = 0;
            try
            {
                locid = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "spAddLocation", para));
                _AddLocation.LocID = locid;
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddLocation", para);
                return Convert.ToInt32(para[45].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void AddQBLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Owner";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.CustomerID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "QBLocationID";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.QBlocationID;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBstax";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.Stax;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Balance";
            para[23].SqlDbType = SqlDbType.Money;
            para[23].Value = objPropUser.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spQBAddLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddQBLocation(AddQBLocationParam _AddQBLocation, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _AddQBLocation.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _AddQBLocation.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _AddQBLocation.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = _AddQBLocation.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _AddQBLocation.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = _AddQBLocation.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = _AddQBLocation.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = _AddQBLocation.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = _AddQBLocation.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = _AddQBLocation.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = _AddQBLocation.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = _AddQBLocation.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = _AddQBLocation.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Owner";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = _AddQBLocation.CustomerID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = _AddQBLocation.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _AddQBLocation.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _AddQBLocation.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _AddQBLocation.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "QBLocationID";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _AddQBLocation.QBlocationID;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _AddQBLocation.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = _AddQBLocation.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = _AddQBLocation.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBstax";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = _AddQBLocation.Stax;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Balance";
            para[23].SqlDbType = SqlDbType.Money;
            para[23].Value = _AddQBLocation.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spQBAddLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBLocationMapping(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[25];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Owner";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.CustomerID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "QBLocationID";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.QBlocationID;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBstax";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.Stax;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Balance";
            para[23].SqlDbType = SqlDbType.Money;
            para[23].Value = objPropUser.Balance;

            para[24] = new SqlParameter();
            para[24].ParameterName = "QBacctID";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropUser.QBAccountNumber;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spQBAddLocationMapping", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddCompany(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spADDCompany", objPropUser.FirstName, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Fax, objPropUser.Email, objPropUser.Website, objPropUser.MSM, objPropUser.DSN, objPropUser.DBName, objPropUser.Username, objPropUser.Password, objPropUser.ContactName, objPropUser.Remarks, objPropUser.Lat, objPropUser.Lng);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Add User Log
        public void AddUserLog(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddUserLogTracking", objPropUser.UserID, objPropUser.Name, objPropUser.LastNAme, objPropUser.DBName, objPropUser.Username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCompany(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateCompany", objPropUser.FirstName
                    , objPropUser.Address
                    , objPropUser.City
                    , objPropUser.State
                    , objPropUser.Zip
                    , objPropUser.Tele
                    , objPropUser.Fax
                    , objPropUser.Email
                    , objPropUser.Website
                    , objPropUser.ContactName
                    , objPropUser.Remarks
                    , objPropUser.Logo
                    , objPropUser.CustWeb
                    , objPropUser.QBPath
                    , objPropUser.MultiLang
                    , objPropUser.QBInteg
                    , objPropUser.EmailMS
                    , objPropUser.QBFirstSync
                    , objPropUser.QBSalesTaxID
                    , objPropUser.QbserviceItemlabor
                    , objPropUser.QBserviceItemExp
                    , objPropUser.YE
                    , objPropUser.GSTReg
                    , objPropUser.Lat
                    , objPropUser.Lng
                    , objPropUser.ConsultAPI
                    , objPropUser.ApplyPasswordRules
                    , objPropUser.ApplyPwRulesToFieldUser
                    , objPropUser.ApplyPwRulesToOfficeUser
                    , objPropUser.ApplyPwRulesToCustomerUser
                    , objPropUser.ApplyPwResetDays
                    , objPropUser.PwResetDays
                    , objPropUser.PwResetting
                    , objPropUser.UserID
                    , objPropUser.Payroll
                    , objPropUser.IsOnlinePaymentApply
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAnnualAmount(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update control set GrossInc =" + objPropUser.AnnualAmount + " , Month = " + objPropUser.Month + ", SalesAnnual = " + objPropUser.SalesAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateControl(User objPropUser)
        {
            string strQuery = string.Empty;
            strQuery = "update control set msrep =" + objPropUser.REPtemplateID + " , tinternet = " + objPropUser.Internet + ", JobCostLabor = " + objPropUser.JobCostLabor + ", MSIsTaskCodesRequired=" + Convert.ToInt16(objPropUser.TaskCode);
            if (objPropUser.bstart != DateTime.MinValue)
                strQuery += ", businessstart='" + objPropUser.bstart + "'";
            if (objPropUser.bend != DateTime.MinValue)
                strQuery += ", businessend='" + objPropUser.bend + "' ";

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// UpdateControlProjectDefaults
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <param name="ContactType"></param>
        /// 
        public void UpdateControlProjectDefaults(User objPropUser, int ContactType)
        {
            string strQuery = string.Empty;
            strQuery = "update control set codes=" + Convert.ToInt16(objPropUser.codes) + " , ContactType =  " + Convert.ToInt16(ContactType) + " , TargetHPermission = " + Convert.ToInt16(objPropUser.TargetHPermission);
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCompany(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "delete from tblcontrol where id='" + objPropUser.CtrlID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomerType(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into otype (type, remarks) values ('" + objPropUser.CustomerType + "','" + objPropUser.Remarks + "')");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spADDCusttype", objPropUser.CustomerType, objPropUser.Remarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddVendorType(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into otype (type, remarks) values ('" + objPropUser.CustomerType + "','" + objPropUser.Remarks + "')");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spADDVendortype", objPropUser.CustomerType, objPropUser.Remarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBCustomerType(User objPropUser)
        {

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   OType \n");
            varname1.Append("              WHERE  QBCustomerTypeID = '" + objPropUser.QBCustomerTypeID + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      INSERT INTO otype \n");
            varname1.Append("                  (type, \n");
            varname1.Append("                   remarks, \n");
            varname1.Append("                   QBCustomerTypeID) \n");
            //varname1.Append("                   LastUpdateDate) \n");
            varname1.Append("      VALUES      ('" + objPropUser.CustomerType + "', \n");
            varname1.Append("                   '" + objPropUser.Remarks + "', \n");
            varname1.Append("                   '" + objPropUser.QBCustomerTypeID + "') \n");
            //varname1.Append("                   Getdate()) \n");
            varname1.Append("  END \n");
            //varname1.Append("ELSE \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      UPDATE OType \n");
            //varname1.Append("      SET    Remarks = '" + objPropUser.Remarks + "' \n");
            ////varname1.Append("             LastUpdateDate = Getdate() \n");
            //varname1.Append("      WHERE  QBCustomerTypeID = '" + objPropUser.QBCustomerTypeID + "' \n");
            //varname1.Append("  END ");
            ////Type = '" + objPropUser.CustomerType + "', \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddQBCustomerType(AddQBCustomerTypeParam _AddQBCustomerType, string ConnectionString)
        {

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   OType \n");
            varname1.Append("              WHERE  QBCustomerTypeID = '" + _AddQBCustomerType.QBCustomerTypeID + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      INSERT INTO otype \n");
            varname1.Append("                  (type, \n");
            varname1.Append("                   remarks, \n");
            varname1.Append("                   QBCustomerTypeID) \n");
            //varname1.Append("                   LastUpdateDate) \n");
            varname1.Append("      VALUES      ('" + _AddQBCustomerType.CustomerType + "', \n");
            varname1.Append("                   '" + _AddQBCustomerType.Remarks + "', \n");
            varname1.Append("                   '" + _AddQBCustomerType.QBCustomerTypeID + "') \n");
            //varname1.Append("                   Getdate()) \n");
            varname1.Append("  END \n");
            //varname1.Append("ELSE \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      UPDATE OType \n");
            //varname1.Append("      SET    Remarks = '" + objPropUser.Remarks + "' \n");
            ////varname1.Append("             LastUpdateDate = Getdate() \n");
            //varname1.Append("      WHERE  QBCustomerTypeID = '" + objPropUser.QBCustomerTypeID + "' \n");
            //varname1.Append("  END ");
            ////Type = '" + objPropUser.CustomerType + "', \n");

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBLocType(User objPropUser)
        {

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   LocType \n");
            //varname1.Append("              WHERE  QBlocTypeID = '" + objPropUser.QBCustomerTypeID + "') \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      INSERT INTO LocType \n");
            //varname1.Append("                  (type, \n");
            //varname1.Append("                   remarks, \n");
            //varname1.Append("                   QBlocTypeID) \n");
            ////varname1.Append("                   LastUpdateDate) \n");
            //varname1.Append("      VALUES      ('" + objPropUser.CustomerType + "', \n");
            //varname1.Append("                   '" + objPropUser.Remarks + "', \n");
            //varname1.Append("                   '" + objPropUser.QBCustomerTypeID + "') \n");
            ////varname1.Append("                   Getdate()) \n");
            //varname1.Append("  END \n");
            ////varname1.Append("ELSE \n");
            ////varname1.Append("  BEGIN \n");
            ////varname1.Append("      UPDATE LocType \n");
            ////varname1.Append("      SET    Remarks = '" + objPropUser.Remarks + "', \n");
            ////varname1.Append("             LastUpdateDate = Getdate() \n");
            ////varname1.Append("      WHERE  QBlocTypeID = '" + objPropUser.QBCustomerTypeID + "' \n");
            ////varname1.Append("  END ");
            //////Type = '" + objPropUser.CustomerType + "', \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQBjobtype", objPropUser.CustomerType, objPropUser.Remarks, objPropUser.QBCustomerTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddQBLocType(AddQBLocTypeParam _AddQBLocType, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "spAddQBjobtype", _AddQBLocType.CustomerType, _AddQBLocType.Remarks, _AddQBLocType.QBCustomerTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCategory(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "Spaddcategory", objPropUser.CustomerType, objPropUser.Remarks, objPropUser.Logo, objPropUser.Chargeable, objPropUser.Default, objPropUser.ScheduleCategoryStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddEquipType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, " if not exists(select 1 from ElevatorSpec where edesc ='" + objPropUser.EquipType + "' and ecat = 1) begin insert into ElevatorSpec (ecat, edesc) values (1,'" + objPropUser.EquipType + "') End else BEGIN  RAISERROR ('Equipment Type already exists, please use different equipment !',16,1)  RETURN END ");
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into ElevatorSpec (ecat, edesc) values (1,'" + objPropUser.EquipType + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEquipBuilding(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, " if not exists(select 1 from ElevatorSpec where edesc ='" + objPropUser.building + "' and ecat = 2) begin insert into ElevatorSpec (ecat, edesc) values (2,'" + objPropUser.building + "') End else BEGIN  RAISERROR ('Equipment Building already exists, please use different equipment !',16,1)  RETURN END ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddEquipCateg(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, " if not exists(select 1 from ElevatorSpec where edesc ='" + objPropUser.EquipType + "' and ecat = 0) begin insert into ElevatorSpec (ecat, edesc) values (0,'" + objPropUser.EquipType + "') End else BEGIN  RAISERROR ('Equipment category already exists, please use different name !',16,1)  RETURN END ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMCPS(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, " if not exists(select 1 from tblMCPStatus where name ='" + objPropUser.EquipType + "' ) begin insert into tblMCPStatus (name) values ('" + objPropUser.EquipType + "') End else BEGIN  RAISERROR ('MCP Status already exists, please use different name !',16,1)  RETURN END ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddServiceType(string ConnConfig, string TYPE, string FDESC, string REMARKS, int REG, int OT, int NT, int DT, int STATUS, string LocType, int ExpenseGL, int InterestGL, int LaborWageC, int InvID, string route, string strddldepartment)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@Type";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = TYPE;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Description";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = FDESC;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Remarks";
                param[2].SqlDbType = SqlDbType.VarChar;
                param[2].Value = REMARKS;


                param[5] = new SqlParameter();
                param[5].ParameterName = "@RT";
                param[5].SqlDbType = SqlDbType.Int;
                param[5].Value = REG;

                param[6] = new SqlParameter();
                param[6].ParameterName = "@OT";
                param[6].SqlDbType = SqlDbType.Int;
                param[6].Value = OT;

                param[7] = new SqlParameter();
                param[7].ParameterName = "@NT";
                param[7].SqlDbType = SqlDbType.Int;
                param[7].Value = NT;

                param[8] = new SqlParameter();
                param[8].ParameterName = "@DT";
                param[8].SqlDbType = SqlDbType.Int;
                param[8].Value = DT;

                param[4] = new SqlParameter();
                param[4].ParameterName = "@Status";
                param[4].SqlDbType = SqlDbType.SmallInt;
                param[4].Value = STATUS;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@LocType";
                param[3].SqlDbType = SqlDbType.NVarChar;
                param[3].Value = LocType;

                param[9] = new SqlParameter();
                param[9].ParameterName = "@ExpenseGL";
                param[9].SqlDbType = SqlDbType.Int;
                param[9].Value = ExpenseGL;

                param[10] = new SqlParameter();
                param[10].ParameterName = "@InterestGL";
                param[10].SqlDbType = SqlDbType.Int;
                param[10].Value = InterestGL;

                param[11] = new SqlParameter();
                param[11].ParameterName = "@LaborWageC";
                param[11].SqlDbType = SqlDbType.Int;
                param[11].Value = LaborWageC;

                param[12] = new SqlParameter();
                param[12].ParameterName = "@InvID";
                param[12].SqlDbType = SqlDbType.Int;
                param[12].Value = InvID;


                param[13] = new SqlParameter();
                param[13].ParameterName = "@route";
                param[13].SqlDbType = SqlDbType.NVarChar;
                param[13].Value = route;

                param[14] = new SqlParameter();
                param[14].ParameterName = "@Department";
                param[14].SqlDbType = SqlDbType.NVarChar;
                param[14].Value = strddldepartment;
                SqlHelper.ExecuteNonQuery(ConnConfig, CommandType.StoredProcedure, "AddServiceType", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateServiceType(string ConnConfig, string TYPE, string FDESC, string REMARKS, int REG, int OT, int NT, int DT, int STATUS, string LocType, int ExpenseGL, int InterestGL, int LaborWageC, int InvID, string route, int Flage, string strddldepartment, string userName)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[17];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@Type";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = TYPE;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Description";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = FDESC;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Remarks";
                param[2].SqlDbType = SqlDbType.VarChar;
                param[2].Value = REMARKS;


                param[5] = new SqlParameter();
                param[5].ParameterName = "@RT";
                param[5].SqlDbType = SqlDbType.Int;
                param[5].Value = REG;

                param[6] = new SqlParameter();
                param[6].ParameterName = "@OT";
                param[6].SqlDbType = SqlDbType.Int;
                param[6].Value = OT;

                param[7] = new SqlParameter();
                param[7].ParameterName = "@NT";
                param[7].SqlDbType = SqlDbType.Int;
                param[7].Value = NT;

                param[8] = new SqlParameter();
                param[8].ParameterName = "@DT";
                param[8].SqlDbType = SqlDbType.Int;
                param[8].Value = DT;

                param[4] = new SqlParameter();
                param[4].ParameterName = "@Status";
                param[4].SqlDbType = SqlDbType.SmallInt;
                param[4].Value = STATUS;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@LocType";
                param[3].SqlDbType = SqlDbType.NVarChar;
                param[3].Value = LocType;

                param[9] = new SqlParameter();
                param[9].ParameterName = "@ExpenseGL";
                param[9].SqlDbType = SqlDbType.Int;
                param[9].Value = ExpenseGL;

                param[10] = new SqlParameter();
                param[10].ParameterName = "@InterestGL";
                param[10].SqlDbType = SqlDbType.Int;
                param[10].Value = InterestGL;

                param[11] = new SqlParameter();
                param[11].ParameterName = "@LaborWageC";
                param[11].SqlDbType = SqlDbType.Int;
                param[11].Value = LaborWageC;

                param[12] = new SqlParameter();
                param[12].ParameterName = "@InvID";
                param[12].SqlDbType = SqlDbType.Int;
                param[12].Value = InvID;


                param[13] = new SqlParameter();
                param[13].ParameterName = "@route";
                param[13].SqlDbType = SqlDbType.NVarChar;
                param[13].Value = route;

                param[14] = new SqlParameter();
                param[14].ParameterName = "@Flage";
                param[14].SqlDbType = SqlDbType.Int;
                param[14].Value = Flage;

                param[15] = new SqlParameter();
                param[15].ParameterName = "@Department";
                param[15].SqlDbType = SqlDbType.NVarChar;
                param[15].Value = strddldepartment;

                param[16] = new SqlParameter();
                param[16].ParameterName = "@UpdatedBy";
                param[16].SqlDbType = SqlDbType.NVarChar;
                param[16].Value = userName;

                SqlHelper.ExecuteNonQuery(ConnConfig, CommandType.StoredProcedure, "spUpdateServiceType", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceType(string ConnConfig, string ServiceType)
        {
            try
            {

                return SqlHelper.ExecuteDataset(ConnConfig, "spGetServiceType", ServiceType);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddLocationType(User objPropUser)
        {
            try
            {

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spADDLoctype", objPropUser.CustomerType, objPropUser.Remarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddZone(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "SpAddZone", objPropUser.Name, objPropUser.fDesc, objPropUser.Bonus, objPropUser.Price1, objPropUser.Count, objPropUser.Tax, objPropUser.Remarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateZone(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateZone", objPropUser.ZoneID, objPropUser.Name, objPropUser.fDesc, objPropUser.Bonus, objPropUser.Price1, objPropUser.Count, objPropUser.Tax, objPropUser.Remarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteZone(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Loc where Zone = '" + objPropUser.ZoneID + "' ) begin delete from Zone where ID='" + objPropUser.ZoneID + "'  end else begin RAISERROR ('Cannot delete %s it is in use !', 16, 1,'" + objPropUser.Name + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  otype set remarks='" + objPropUser.Remarks + "' where type= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateVendorType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  VType set remarks='" + objPropUser.Remarks + "' where type= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateServiceType(User objPropUser, string RT, string OT, string NT, string DT, string status)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  ltype set fdesc='" + objPropUser.Locationname + "', remarks='" + objPropUser.LocationRemarks + "', InvID='" + objPropUser.InvID + "' , Reg='" + RT + "' , OT='" + OT + "' , NT='" + NT + "' , DT='" + DT + "' , Status='" + status + "'  where type= '" + objPropUser.EquipType + "'");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public void UpdateCategory(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "Spupdatecategory", objPropUser.CustomerType, objPropUser.Remarks, objPropUser.Logo, objPropUser.Chargeable, objPropUser.Default, objPropUser.ScheduleCategoryStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEquipType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  ElevatorSpec set edesc='" + objPropUser.EquipType + "' where edesc= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocationType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  loctype set remarks='" + objPropUser.Remarks + "' where type= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update consultant
        public void UpdateConsultant(User objPropUser, tblConsult objPropConsult)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update tblConsult set Username='" + objPropConsult.Username + "',Password ='" + objPropConsult.Password + "',IP = '" + objPropConsult.IP + "',Name='" + objPropUser.FirstName + "' where ID = '" + objPropConsult.ID + "'");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update Rol set Name='" + objPropUser.FirstName + "',Address='" + objPropUser.Address + "',City ='" + objPropUser.City + "',State = '" + objPropUser.State + "',Zip='" + objPropUser.Zip + "',country='" + objPropUser.Country + "',Contact='" + objPropUser.MainContact + "',Phone='" + objPropUser.Phone + "',Cellular='" + objPropUser.Cell + "',Fax='" + objPropUser.Fax + "',Email='" + objPropUser.Email + "',Website='" + objPropUser.Website + "' where ID='" + objPropUser.ID + "'");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCustomerType(User objPropUser)
        {
            try
            {

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Owner where Type = '" + objPropUser.CustomerType + "' ) begin delete from otype where Type='" + objPropUser.CustomerType + "' end else begin RAISERROR ('Cannot delete %s type it is in use !', 16, 1,'" + objPropUser.CustomerType + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteVendorType(User objPropUser)
        {
            try
            {

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Vendor where Type = '" + objPropUser.CustomerType + "' ) begin delete from vtype where Type='" + objPropUser.CustomerType + "' end else begin RAISERROR ('Cannot delete %s type it is in use !', 16, 1,'" + objPropUser.CustomerType + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerTypeByListID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  otype where isnull(qbcustomertypeid,'')<>'' and qbcustomertypeid= '" + objPropUser.QBCustomerTypeID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void DeleteCategory(User objPropUser)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  category where type= '" + objPropUser.CustomerType + "'");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet getTicketInvoiceEmail(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * From Email Where Type= '" + objPropUser.Type + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddTicketInvoiceEmail(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddTicketInvoiceEmail", objPropUser.Type, objPropUser.Subject, objPropUser.Body, objPropUser.BitMap, objPropUser.BodyMulitple, objPropUser.Fields);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTicketInvoiceEmail(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateTicketInvoiceEmail", objPropUser.EmailID, objPropUser.Type, objPropUser.Subject, objPropUser.Body, objPropUser.BitMap, objPropUser.BodyMulitple, objPropUser.Fields);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEquiptype(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Elev where Type = '" + objPropUser.EquipType + "' ) begin delete from ElevatorSpec where EDesc='" + objPropUser.EquipType + "' and ecat=1 end else begin RAISERROR ('Cannot delete %s it is in use !', 16, 1,'" + objPropUser.EquipType + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEquipBuilding(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Elev where Building = '" + objPropUser.building + "' ) begin delete from ElevatorSpec where EDesc='" + objPropUser.building + "' and ecat=2 end else begin RAISERROR ('Cannot delete %s it is in use !', 16, 1,'" + objPropUser.building + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEquipCateg(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Elev where Category = '" + objPropUser.Category + "' ) begin delete from ElevatorSpec where EDesc='" + objPropUser.Category + "' and ecat=0 end else begin RAISERROR ('Cannot delete %s it is in use !', 16, 1,'" + objPropUser.Category + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteMCPS(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from repdetail where status = '" + objPropUser.EquipType + "' ) begin delete from tblmcpstatus where name='" + objPropUser.EquipType + "'  end else begin RAISERROR ('MCP Status is in use!', 16, 1) RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteRoutes(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   Loc \n");
            varname1.Append("              WHERE  Route = '" + objPropUser.ID + "' \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Job \n");
            varname1.Append("              WHERE  Custom20 = '" + objPropUser.ID + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM Route \n");
            varname1.Append("      WHERE  ID = '" + objPropUser.ID + "' \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Route is in use and can therefore not be deleted!',16,1) \n");
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
        public void DeleteServicetype(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   Elev \n");
            varname1.Append("               WHERE  Cat = '" + objPropUser.EquipType + "' \n");
            varname1.Append("               UNION \n");
            varname1.Append("               SELECT 1 \n");
            varname1.Append("               FROM   Job \n");
            varname1.Append("               WHERE  CType = '" + objPropUser.EquipType + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM LType \n");
            varname1.Append("      WHERE  Type = '" + objPropUser.EquipType + "' \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR ('Service type is in use!',16,1) \n");
            varname1.Append(" \n");
            varname1.Append("      RETURN \n");
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

        public void DeleteSalesTax(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   Loc \n");
            varname1.Append("               WHERE  STax = '" + objPropUser.SalesTax + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM STax \n");
            varname1.Append("      WHERE  Name = '" + objPropUser.SalesTax + "' \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Sales tax is in use!',16,1) \n");
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

        public void DeleteSalesTaxByListID(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   Loc \n");
            varname1.Append("               WHERE  STax = (select Name from STax where  isnull(qbstaxid,'')<>'' and  qbstaxid = '" + objPropUser.QBSalesTaxID + "')) \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM STax \n");
            varname1.Append("      WHERE isnull(qbstaxid,'')<>'' and  qbstaxid = '" + objPropUser.QBSalesTaxID + "' \n");
            varname1.Append("  END \n");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE STax SET Status=1 WHERE Isnull( qbstaxid,'')<>'' AND  qbstaxid = '" + objPropUser.QBSalesTaxID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteDepartment(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE  Type = " + objPropUser.DepartmentID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Type = " + objPropUser.DepartmentID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Invoice \n");
            varname1.Append("              WHERE  Type = " + objPropUser.DepartmentID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM JobType \n");
            varname1.Append("      WHERE  ID = " + objPropUser.DepartmentID + " and ID <> 0 \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Department is in use!',16,1) \n");
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

        public void DeleteDepartmentByListID(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE  Type = (SELECT ID \n");
            varname1.Append("                             FROM   JobType \n");
            varname1.Append("                             WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("                                    AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "') \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Type = (SELECT ID \n");
            varname1.Append("                             FROM   JobType \n");
            varname1.Append("                             WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("                                    AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "') \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Invoice \n");
            varname1.Append("              WHERE  Type = (SELECT ID \n");
            varname1.Append("                             FROM   JobType \n");
            varname1.Append("                             WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("                                    AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "')) \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM JobType \n");
            varname1.Append("      WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("             AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "' \n");
            varname1.Append("  END ");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE JobType SET Status=1 WHERE Isnull( QBJobTypeID,'')<>'' AND  QBJobTypeID = '" + objPropUser.QBJobtypeID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteCategory(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE  Cat = '" + objPropUser.Category + "' \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Cat = '" + objPropUser.Category + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM Category \n");
            varname1.Append("      WHERE  Type = '" + objPropUser.Category + "' \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Category is in use!',16,1) \n");
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

        public void DeleteUser(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  tbluser where id= '" + objPropUser.UserID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomer(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   Loc \n");
            //varname1.Append("              WHERE  Owner = " + objPropUser.CustomerID + ") \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      DELETE FROM rol \n");
            //varname1.Append("      WHERE  id = (SELECT TOP 1 rol \n");
            //varname1.Append("                   FROM   owner \n");
            //varname1.Append("                   WHERE  id = " + objPropUser.CustomerID + ") \n");
            //varname1.Append(" \n");
            //varname1.Append("      DELETE FROM Owner \n");
            //varname1.Append("      WHERE  ID = " + objPropUser.CustomerID + " \n");
            //varname1.Append("  END \n");
            //varname1.Append("ELSE \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      RAISERROR ('Locations are assigned to the selected customer, customer cannot be deleted!',16,1) \n");
            //varname1.Append(" \n");
            //varname1.Append("      RETURN \n");
            //varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteCustomer", objPropUser.CustomerID);// "if not exists(select 1 from Loc where Owner = " + objPropUser.CustomerID + ") begin delete from rol where id=(select top 1 rol from owner where id =" + objPropUser.CustomerID + ") delete from Owner where ID=" + objPropUser.CustomerID + " end else begin RAISERROR ('Locations are assigned to the selected customer, customer cannot be deleted!', 16, 1) RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void DeleteCustomer(DeleteCustomerParam _DeleteCustomer, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "spDeleteCustomer", _DeleteCustomer.CustomerID);// "if not exists(select 1 from Loc where Owner = " + objPropUser.CustomerID + ") begin delete from rol where id=(select top 1 rol from owner where id =" + objPropUser.CustomerID + ") delete from Owner where ID=" + objPropUser.CustomerID + " end else begin RAISERROR ('Locations are assigned to the selected customer, customer cannot be deleted!', 16, 1) RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerByListID(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   Loc \n");
            //varname1.Append("              WHERE  Owner = (SELECT TOP 1 ID \n");
            //varname1.Append("                   FROM   owner \n");
            //varname1.Append("                   WHERE isnull( QBCustomerID,'')<>'' and  QBCustomerID = '" + objPropUser.QBCustomerID + "')) \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      DELETE FROM rol \n");
            //varname1.Append("      WHERE  id = (SELECT TOP 1 rol \n");
            //varname1.Append("                   FROM   owner \n");
            //varname1.Append("                   WHERE isnull( QBCustomerID,'')<>'' and QBCustomerID = '" + objPropUser.QBCustomerID + "') \n");
            //varname1.Append(" \n");
            //varname1.Append("      DELETE FROM Owner \n");
            //varname1.Append("                   WHERE isnull( QBCustomerID,'')<>'' and QBCustomerID = '" + objPropUser.QBCustomerID + "' \n");
            //varname1.Append("  END \n");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE owner SET Status = 1 WHERE Isnull( QBCustomerID,'')<>'' AND  QBCustomerID = '" + objPropUser.QBCustomerID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteCustomerByListID", objPropUser.QBCustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerBySageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteCustomerBySageID", objPropUser.QBCustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocationBySageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteLocationBySageID", objPropUser.QBCustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEquipment(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from TicketO where LElev = " + objPropUser.EquipID + " union select 1 from TicketD where Elev=" + objPropUser.EquipID + " union select 1 from tblJoinElevJob where elev=" + objPropUser.EquipID + " ) begin delete from Elev where ID=" + objPropUser.EquipID + " end else begin RAISERROR ('Selected equipment is in use, equipment cannot be deleted!', 16, 1) RETURN end");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteEquipment", objPropUser.EquipID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from TicketO where LElev = " + objPropUser.EquipID + " union select 1 from TicketD where Elev=" + objPropUser.EquipID + " union select 1 from tblJoinElevJob where elev=" + objPropUser.EquipID + " ) begin delete from Elev where ID=" + objPropUser.EquipID + " end else begin RAISERROR ('Selected equipment is in use, equipment cannot be deleted!', 16, 1) RETURN end");
                SqlHelper.ExecuteNonQuery(ConnectionString, "spDeleteEquipment", _DeleteEquipment.EquipID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLeadEquipment(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from TicketO where LElev = " + objPropUser.EquipID + " union select 1 from TicketD where Elev=" + objPropUser.EquipID + " union select 1 from tblJoinElevJob where elev=" + objPropUser.EquipID + " ) begin delete from Elev where ID=" + objPropUser.EquipID + " end else begin RAISERROR ('Selected equipment is in use, equipment cannot be deleted!', 16, 1) RETURN end");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteLeadEquipment", objPropUser.EquipID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocation(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   TicketO \n");
            //varname1.Append("              WHERE ltype=0 and  LID = " + objPropUser.LocID + " \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   TicketD \n");
            //varname1.Append("              WHERE  Loc = " + objPropUser.LocID + " \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   job \n");
            //varname1.Append("              WHERE  Loc = " + objPropUser.LocID + " \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Elev \n");
            //varname1.Append("              WHERE  Loc = " + objPropUser.LocID + " \n");
            //varname1.Append("UNION SELECT 1 FROM Lead ld INNER JOIN Loc l ON l.Rol=ld.Rol WHERE l.Loc=" + objPropUser.LocID + " \n");
            //varname1.Append("UNION SELECT 1 FROM ToDo t INNER JOIN Loc l ON l.Rol=t.Rol WHERE l.Loc=" + objPropUser.LocID + " \n");
            //varname1.Append("UNION SELECT 1 FROM Done d INNER JOIN Loc l ON l.Rol=d.Rol WHERE l.Loc=" + objPropUser.LocID + " \n");
            //varname1.Append("UNION SELECT 1 FROM Estimate e INNER JOIN Loc l ON l.Rol=e.RolID WHERE l.Loc=" + objPropUser.LocID + " \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Invoice \n");
            //varname1.Append("              WHERE  Loc = " + objPropUser.LocID + ") \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("    delete from rol where id=(select top 1 rol from loc where loc =" + objPropUser.LocID + ")   DELETE FROM Loc \n");
            //varname1.Append("      WHERE  loc = " + objPropUser.LocID + " \n");
            //varname1.Append("  END \n");
            //varname1.Append("ELSE \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      RAISERROR ('Selected location is in use, location cannot be deleted!',16,1) \n");
            //varname1.Append(" \n");
            //varname1.Append("      RETURN \n");
            //varname1.Append("  END ");


            try
            {

                SqlParameter[] para = new SqlParameter[1];

                para[0] = new SqlParameter();
                para[0].ParameterName = "LocID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropUser.LocID;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spDeleteLocationbyLocID", para);// "if not exists(select 1 from TicketO where LID=" + objPropUser.LocID + " union select 1 from TicketD where Loc=" + objPropUser.LocID + "union select 1 from job where Loc=" + objPropUser.LocID + ") begin delete from Loc where loc=" + objPropUser.LocID + " end else begin RAISERROR ('Selected location is in use, location cannot be deleted!', 16, 1) RETURN end"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void DeleteLocation(DeleteLocationParam _DeleteLocation, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE ltype=0 and  LID = " + _DeleteLocation.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Loc = " + _DeleteLocation.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   job \n");
            varname1.Append("              WHERE  Loc = " + _DeleteLocation.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Elev \n");
            varname1.Append("              WHERE  Loc = " + _DeleteLocation.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM Lead ld INNER JOIN Loc l ON l.Rol=ld.Rol WHERE l.Loc=" + _DeleteLocation.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM ToDo t INNER JOIN Loc l ON l.Rol=t.Rol WHERE l.Loc=" + _DeleteLocation.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM Done d INNER JOIN Loc l ON l.Rol=d.Rol WHERE l.Loc=" + _DeleteLocation.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM Estimate e INNER JOIN Loc l ON l.Rol=e.RolID WHERE l.Loc=" + _DeleteLocation.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Invoice \n");
            varname1.Append("              WHERE  Loc = " + _DeleteLocation.LocID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("    delete from rol where id=(select top 1 rol from loc where loc =" + _DeleteLocation.LocID + ")   DELETE FROM Loc \n");
            varname1.Append("      WHERE  loc = " + _DeleteLocation.LocID + " \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR ('Selected location is in use, location cannot be deleted!',16,1) \n");
            varname1.Append(" \n");
            varname1.Append("      RETURN \n");
            varname1.Append("  END ");


            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname1.ToString());// "if not exists(select 1 from TicketO where LID=" + objPropUser.LocID + " union select 1 from TicketD where Loc=" + objPropUser.LocID + "union select 1 from job where Loc=" + objPropUser.LocID + ") begin delete from Loc where loc=" + objPropUser.LocID + " end else begin RAISERROR ('Selected location is in use, location cannot be deleted!', 16, 1) RETURN end"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteLocationByListID(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   TicketO \n");
            //varname1.Append("              WHERE  LID = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   TicketD \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Job \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Elev \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Invoice \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "')) \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      DELETE FROM Rol \n");
            //varname1.Append("      WHERE  ID = (SELECT TOP 1 rol \n");
            //varname1.Append("                   FROM   loc \n");
            //varname1.Append("                   WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append(" \n");
            //varname1.Append("      DELETE FROM Loc \n");
            //varname1.Append("      WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "' \n");

            //varname1.Append("INSERT INTO tblSyncDeleted \n");
            //varname1.Append("            (Tbl, \n");
            //varname1.Append("             NAME, \n");
            //varname1.Append("             RefID, \n");
            //varname1.Append("             QBID) \n");
            //varname1.Append("VALUES      ('LOC', \n");
            //varname1.Append("             (select tag from loc where WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "'), \n");
            //varname1.Append("             (select loc from loc where WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "'), \n");
            //varname1.Append("             '" + objPropUser.QBlocationID + "' ) \n");

            //varname1.Append("  END ");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE Loc SET Status=1 WHERE Isnull( QBLocID,'')<>'' AND  QBLocID = '" + objPropUser.QBlocationID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteLocationByListID", objPropUser.QBlocationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEmployeeByListID(User objPropUser)
        {
            string strQuery = " update tbluser set status = 1 where isnull(qbemployeeid,'')<>'' and qbemployeeid= '" + objPropUser.QBEmployeeID + "' ";
            strQuery += " update tblWork set Status= 1 where fDesc = (select fuser from tblUser where isnull(qbemployeeid,'')<>'' and qbemployeeid= '" + objPropUser.QBEmployeeID + "') ";
            strQuery += " update Emp set Status= 1 where CallSign = (select fuser from tblUser where isnull(qbemployeeid,'')<>'' and qbemployeeid= '" + objPropUser.QBEmployeeID + "') ";

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocType(User objPropUser)
        {
            try
            {
                // SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  loctype where type= '" + objPropUser.CustomerType + "'");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Loc where Type = '" + objPropUser.CustomerType + "' ) begin delete from LocType where Type='" + objPropUser.CustomerType + "' end else begin RAISERROR ('Cannot delete %s type it is in use !', 16, 1,'" + objPropUser.CustomerType + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //delete consultant
        public void DeleteConsultant(tblConsult objPropConsult)
        {
            try
            {
                // SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  loctype where type= '" + objPropUser.CustomerType + "'");
                SqlHelper.ExecuteNonQuery(objPropConsult.ConnConfig, CommandType.Text, "delete from tblConsult where ID='" + objPropConsult.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteLocTypeByListID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from loctype where isnull(qbloctypeid,'')<>'' and qbloctypeid= '" + objPropUser.QBJobtypeID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteContactByID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "Delete From Phone Where ID=" + objPropUser.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void CreateDatabase(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spCreateDB", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddDatabaseName(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spAddControl", objPropUser.FirstName, objPropUser.DBName, objPropUser.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDatabaseName(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spUpdateControl", objPropUser.FirstName, objPropUser.DBName, objPropUser.CtrlID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateAdminPassword(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tbluser set username='" + objPropUser.Username + "', password='" + objPropUser.Password + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocPrintEmail(User objPropUser)
        {
            try
            {
                String printInvoice = objPropUser.PrintInvoice == true ? "1" : "0";
                String emailInvoice = objPropUser.EmailInvoice == true ? "1" : "0";
                String Query = "update Loc Set PrintInvoice=" + printInvoice + ", EmailInvoice=" + emailInvoice + ", Custom12='" + objPropUser.Custom1 + "', Custom13='" + objPropUser.Custom2 + "', NoCustomerStatement='" + objPropUser.NoCustomerStatement + "'  Where Loc=" + objPropUser.RolId;
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocCustom12(User objPropUser)
        {
            try
            {
                String Query = "update Loc Set Custom12='" + objPropUser.Custom1 + "' Where Loc=" + objPropUser.RolId;
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAdminPassword(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select username, password from tbluser");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocation(User objPropUser, bool CopyToLocAndJob = false, int ApplyServiceTypeRule = 0, string ServiceTypeName = "", int ProjectPerDepartmentCount = 0)
        {
            SqlParameter[] para = new SqlParameter[65];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Route";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropUser.Route;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Terr";
            para[8].SqlDbType = SqlDbType.Int;
            para[8].Value = objPropUser.Territory;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "contactname";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.MainContact;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Phone";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Phone;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Fax";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Fax;

            para[13] = new SqlParameter();
            para[13].ParameterName = "cellular";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.Cell;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Email";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Email;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolAddress";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolAddress;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolCity";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolCity;

            para[18] = new SqlParameter();
            para[18].ParameterName = "RolState";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.RolState;

            para[19] = new SqlParameter();
            para[19].ParameterName = "RolZip";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.RolZip;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ContactData";
            para[20].SqlDbType = SqlDbType.Structured;
            para[20].Value = objPropUser.ContactData;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Locid";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropUser.LocID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Owner";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = objPropUser.CustomerID;

            para[24] = new SqlParameter();
            para[24].ParameterName = "stax";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropUser.Stax;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Lat";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropUser.Lat;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Lng";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropUser.Lng;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Custom1";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropUser.Custom1;

            para[28] = new SqlParameter();
            para[28].ParameterName = "Custom2";
            para[28].SqlDbType = SqlDbType.VarChar;
            para[28].Value = objPropUser.Custom2;

            para[29] = new SqlParameter();
            para[29].ParameterName = "To";
            para[29].SqlDbType = SqlDbType.VarChar;
            para[29].Value = objPropUser.ToMail;

            para[30] = new SqlParameter();
            para[30].ParameterName = "CC";
            para[30].SqlDbType = SqlDbType.VarChar;
            para[30].Value = objPropUser.CCMail;

            para[31] = new SqlParameter();
            para[31].ParameterName = "ToInv";
            para[31].SqlDbType = SqlDbType.VarChar;
            para[31].Value = objPropUser.MailToInv;

            para[32] = new SqlParameter();
            para[32].ParameterName = "CCInv";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = objPropUser.MailCCInv;

            para[33] = new SqlParameter();
            para[33].ParameterName = "CreditHold";
            para[33].SqlDbType = SqlDbType.TinyInt;
            para[33].Value = objPropUser.CreditHold;

            para[34] = new SqlParameter();
            para[34].ParameterName = "DispAlert";
            para[34].SqlDbType = SqlDbType.TinyInt;
            para[34].Value = objPropUser.DispAlert;

            para[35] = new SqlParameter();
            para[35].ParameterName = "CreditReason";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = objPropUser.CreditReason;

            para[36] = new SqlParameter();
            para[36].ParameterName = "ContractBill";
            para[36].SqlDbType = SqlDbType.TinyInt;
            para[36].Value = objPropUser.ContractBill;

            para[37] = new SqlParameter();
            para[37].ParameterName = "terms";
            para[37].SqlDbType = SqlDbType.Int;
            para[37].Value = objPropUser.TermsID;

            para[38] = new SqlParameter();
            para[38].ParameterName = "@Docs";
            para[38].SqlDbType = SqlDbType.Structured;
            para[38].Value = objPropUser.dtDocs;

            para[39] = new SqlParameter();
            para[39].ParameterName = "BillRate";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = objPropUser.BillRate;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OT";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = objPropUser.RateOT;

            para[41] = new SqlParameter();
            para[41].ParameterName = "NT";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = objPropUser.RateNT;

            para[42] = new SqlParameter();
            para[42].ParameterName = "DT";
            para[42].SqlDbType = SqlDbType.Decimal;
            para[42].Value = objPropUser.RateDT;

            para[43] = new SqlParameter();
            para[43].ParameterName = "Travel";
            para[43].SqlDbType = SqlDbType.Decimal;
            para[43].Value = objPropUser.RateTravel;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Mileage";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = objPropUser.MileageRate;

            para[45] = new SqlParameter();
            para[45].ParameterName = "@Alerts";
            para[45].SqlDbType = SqlDbType.Structured;
            para[45].Value = objPropUser.dtAlerts;

            para[46] = new SqlParameter();
            para[46].ParameterName = "@AlertContacts";
            para[46].SqlDbType = SqlDbType.Structured;
            para[46].Value = objPropUser.dtAlertContacts;

            para[47] = new SqlParameter();
            para[47].ParameterName = "@tblGCandHomeOwner";
            para[47].SqlDbType = SqlDbType.Structured;
            para[47].Value = objPropUser.tblGCandHomeOwner;

            para[48] = new SqlParameter();
            para[48].ParameterName = "EmailInvoice";
            para[48].SqlDbType = SqlDbType.Bit;
            para[48].Value = objPropUser.EmailInvoice;

            para[49] = new SqlParameter();
            para[49].ParameterName = "PrintInvoice";
            para[49].SqlDbType = SqlDbType.Bit;
            para[49].Value = objPropUser.PrintInvoice;

            para[50] = new SqlParameter();
            para[50].ParameterName = "CopyToLocAndJob";
            para[50].SqlDbType = SqlDbType.Bit;
            para[50].Value = CopyToLocAndJob;

            para[51] = new SqlParameter();
            para[51].ParameterName = "Terr2";
            para[51].SqlDbType = SqlDbType.Int;
            para[51].Value = objPropUser.Territory2;

            para[52] = new SqlParameter();
            para[52].ParameterName = "STax2";
            para[52].SqlDbType = SqlDbType.VarChar;
            para[52].Value = objPropUser.STax2;

            para[53] = new SqlParameter();
            para[53].ParameterName = "UTax";
            para[53].SqlDbType = SqlDbType.VarChar;
            para[53].Value = objPropUser.UTax;

            para[54] = new SqlParameter();
            para[54].ParameterName = "Zone";
            para[54].SqlDbType = SqlDbType.Int;
            para[54].Value = objPropUser.Zone;

            para[55] = new SqlParameter();
            para[55].ParameterName = "@UpdatedBy";
            para[55].SqlDbType = SqlDbType.VarChar;
            para[55].Value = objPropUser.MOMUSer;

            para[56] = new SqlParameter();
            para[56].ParameterName = "Consult";
            para[56].SqlDbType = SqlDbType.Int;
            para[56].Value = objPropUser.Consult;

            para[57] = new SqlParameter();
            para[57].ParameterName = "@Country";
            para[57].SqlDbType = SqlDbType.VarChar;
            para[57].Value = objPropUser.Country;

            para[58] = new SqlParameter();
            para[58].ParameterName = "@RolCountry";
            para[58].SqlDbType = SqlDbType.VarChar;
            para[58].Value = objPropUser.RolCountry;

            para[59] = new SqlParameter();
            para[59].ParameterName = "@NoCustomerStatement";
            para[59].SqlDbType = SqlDbType.Bit;
            para[59].Value = objPropUser.NoCustomerStatement;

            para[60] = new SqlParameter();
            para[60].ParameterName = "@BusinessTypeID";
            para[60].SqlDbType = SqlDbType.Int;
            para[60].Value = objPropUser.BusinessTypeID;

            para[61] = new SqlParameter();
            para[61].ParameterName = "@ApplyServiceTypeRule";
            para[61].SqlDbType = SqlDbType.Int;
            para[61].Value = ApplyServiceTypeRule;

            para[62] = new SqlParameter();
            para[62].ParameterName = "@ServiceTypeName";
            para[62].SqlDbType = SqlDbType.VarChar;
            para[62].Value = ServiceTypeName;

            para[63] = new SqlParameter();
            para[63].ParameterName = "@ProjectPerDepartmentCount";
            para[63].SqlDbType = SqlDbType.Int;
            para[63].Value = ProjectPerDepartmentCount;

            para[64] = new SqlParameter();
            para[64].ParameterName = "CreditFlag";
            para[64].SqlDbType = SqlDbType.TinyInt;
            para[64].Value = objPropUser.CreditFlag;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Update_Loc_Terr(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[3];


            para[0] = new SqlParameter();
            para[0].ParameterName = "Terr";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropUser.Territory;

            para[1] = new SqlParameter();
            para[1].ParameterName = "locID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.LocID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Terr2";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropUser.Territory2;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "Update_Loc_Terr", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public void UpdateLocation(UpdateLocationParam _UpdateLocation, string ConnectionString, bool CopyToLocAndJob = false, int ApplyServiceTypeRule = 0, string ServiceTypeName = "", int ProjectPerDepartmentCount = 0)
        {
            SqlParameter[] para = new SqlParameter[64];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _UpdateLocation.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _UpdateLocation.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _UpdateLocation.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = _UpdateLocation.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _UpdateLocation.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = _UpdateLocation.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = _UpdateLocation.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Route";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = _UpdateLocation.Route;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Terr";
            para[8].SqlDbType = SqlDbType.Int;
            para[8].Value = _UpdateLocation.Territory;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = _UpdateLocation.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "contactname";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = _UpdateLocation.MainContact;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Phone";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = _UpdateLocation.Phone;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Fax";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = _UpdateLocation.Fax;

            para[13] = new SqlParameter();
            para[13].ParameterName = "cellular";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = _UpdateLocation.Cell;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Email";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = _UpdateLocation.Email;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _UpdateLocation.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolAddress";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _UpdateLocation.RolAddress;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolCity";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _UpdateLocation.RolCity;

            para[18] = new SqlParameter();
            para[18].ParameterName = "RolState";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _UpdateLocation.RolState;

            para[19] = new SqlParameter();
            para[19].ParameterName = "RolZip";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _UpdateLocation.RolZip;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ContactData";
            para[20].SqlDbType = SqlDbType.Structured;
            para[20].Value = _UpdateLocation.ContactData;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = _UpdateLocation.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Locid";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = _UpdateLocation.LocID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Owner";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = _UpdateLocation.CustomerID;

            para[24] = new SqlParameter();
            para[24].ParameterName = "stax";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = _UpdateLocation.Stax;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Lat";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = _UpdateLocation.Lat;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Lng";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = _UpdateLocation.Lng;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Custom1";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = _UpdateLocation.Custom1;

            para[28] = new SqlParameter();
            para[28].ParameterName = "Custom2";
            para[28].SqlDbType = SqlDbType.VarChar;
            para[28].Value = _UpdateLocation.Custom2;

            para[29] = new SqlParameter();
            para[29].ParameterName = "To";
            para[29].SqlDbType = SqlDbType.VarChar;
            para[29].Value = _UpdateLocation.ToMail;

            para[30] = new SqlParameter();
            para[30].ParameterName = "CC";
            para[30].SqlDbType = SqlDbType.VarChar;
            para[30].Value = _UpdateLocation.CCMail;

            para[31] = new SqlParameter();
            para[31].ParameterName = "ToInv";
            para[31].SqlDbType = SqlDbType.VarChar;
            para[31].Value = _UpdateLocation.MailToInv;

            para[32] = new SqlParameter();
            para[32].ParameterName = "CCInv";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = _UpdateLocation.MailCCInv;

            para[33] = new SqlParameter();
            para[33].ParameterName = "CreditHold";
            para[33].SqlDbType = SqlDbType.TinyInt;
            para[33].Value = _UpdateLocation.CreditHold;

            para[34] = new SqlParameter();
            para[34].ParameterName = "DispAlert";
            para[34].SqlDbType = SqlDbType.TinyInt;
            para[34].Value = _UpdateLocation.DispAlert;

            para[35] = new SqlParameter();
            para[35].ParameterName = "CreditReason";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = _UpdateLocation.CreditReason;

            para[36] = new SqlParameter();                   //added by Mayuri 24th dec,15
            para[36].ParameterName = "ContractBill";
            para[36].SqlDbType = SqlDbType.TinyInt;
            para[36].Value = _UpdateLocation.ContractBill;

            para[37] = new SqlParameter();
            para[37].ParameterName = "terms";
            para[37].SqlDbType = SqlDbType.Int;
            para[37].Value = _UpdateLocation.TermsID;

            para[38] = new SqlParameter();
            para[38].ParameterName = "@Docs";
            para[38].SqlDbType = SqlDbType.Structured;
            if (_UpdateLocation.dtDocs.Rows.Count > 0)
            {
                if (_UpdateLocation.dtDocs.Rows[0]["ID"].ToString() != "0")
                {
                    para[38].Value = _UpdateLocation.dtDocs;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Portal", typeof(int));
                    dt.Columns.Add("Remarks", typeof(string));
                    para[38].Value = dt;
                }
            }

            //para[24] = new SqlParameter();
            //para[24].ParameterName = "MAPAddress";
            //para[24].SqlDbType = SqlDbType.VarChar;
            //para[24].Value = _UpdateLocation.MAPAddress;
            para[39] = new SqlParameter();
            para[39].ParameterName = "BillRate";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = _UpdateLocation.BillRate;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OT";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = _UpdateLocation.RateOT;

            para[41] = new SqlParameter();
            para[41].ParameterName = "NT";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = _UpdateLocation.RateNT;

            para[42] = new SqlParameter();
            para[42].ParameterName = "DT";
            para[42].SqlDbType = SqlDbType.Decimal;
            para[42].Value = _UpdateLocation.RateDT;

            para[43] = new SqlParameter();
            para[43].ParameterName = "Travel";
            para[43].SqlDbType = SqlDbType.Decimal;
            para[43].Value = _UpdateLocation.RateTravel;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Mileage";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = _UpdateLocation.MileageRate;

            para[45] = new SqlParameter();
            para[45].ParameterName = "@Alerts";
            para[45].SqlDbType = SqlDbType.Structured;
            if (_UpdateLocation.dtAlerts.Rows.Count > 0)
            {
                if (_UpdateLocation.dtAlerts.Rows[0]["AlertID"].ToString() != "0")
                {
                    para[45].Value = _UpdateLocation.dtAlerts;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("AlertID", typeof(int));
                    dt.Columns.Add("AlertCode", typeof(string));
                    dt.Columns.Add("AlertName", typeof(string));
                    dt.Columns.Add("AlertSubject", typeof(string));
                    dt.Columns.Add("AlertMessage", typeof(string));
                    para[45].Value = dt;
                }
            }

            para[46] = new SqlParameter();
            para[46].ParameterName = "@AlertContacts";
            para[46].SqlDbType = SqlDbType.Structured;
            if (_UpdateLocation.dtAlertContacts.Rows.Count > 0)
            {
                if (_UpdateLocation.dtAlertContacts.Rows[0]["id"].ToString() != "0")
                {
                    para[46].Value = _UpdateLocation.dtAlertContacts;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("id", typeof(int));
                    dt.Columns.Add("screenid", typeof(int));
                    dt.Columns.Add("screenname", typeof(string));
                    dt.Columns.Add("alertid", typeof(int));
                    dt.Columns.Add("name", typeof(string));
                    dt.Columns.Add("email", typeof(bool));
                    dt.Columns.Add("text", typeof(bool));
                    dt.Columns.Add("alertcode", typeof(string));
                    para[46].Value = dt;
                }
            }

            para[47] = new SqlParameter();
            para[47].ParameterName = "@tblGCandHomeOwner";
            para[47].SqlDbType = SqlDbType.Structured;
            if (_UpdateLocation.tblGCandHomeOwner.Rows.Count > 0)
            {
                if (_UpdateLocation.tblGCandHomeOwner.Rows[0]["ID"].ToString() != "0")
                {
                    para[47].Value = _UpdateLocation.tblGCandHomeOwner;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("NAME", typeof(string));
                    dt.Columns.Add("City", typeof(string));
                    dt.Columns.Add("State", typeof(string));
                    dt.Columns.Add("Zip", typeof(string));
                    dt.Columns.Add("Phone", typeof(string));
                    dt.Columns.Add("Fax", typeof(string));
                    dt.Columns.Add("Contact", typeof(string));
                    dt.Columns.Add("Remarks", typeof(string));
                    dt.Columns.Add("Country", typeof(string));
                    dt.Columns.Add("Cellular", typeof(string));
                    dt.Columns.Add("EMail", typeof(string));
                    dt.Columns.Add("Type", typeof(int));
                    dt.Columns.Add("Address", typeof(string));
                    para[47].Value = dt;
                }
            }

            para[48] = new SqlParameter();
            para[48].ParameterName = "EmailInvoice";
            para[48].SqlDbType = SqlDbType.Bit;
            para[48].Value = _UpdateLocation.EmailInvoice;


            para[49] = new SqlParameter();
            para[49].ParameterName = "PrintInvoice";
            para[49].SqlDbType = SqlDbType.Bit;
            para[49].Value = _UpdateLocation.PrintInvoice;


            para[50] = new SqlParameter();
            para[50].ParameterName = "CopyToLocAndJob";
            para[50].SqlDbType = SqlDbType.Bit;
            para[50].Value = CopyToLocAndJob;


            para[51] = new SqlParameter();
            para[51].ParameterName = "Terr2";
            para[51].SqlDbType = SqlDbType.Int;
            para[51].Value = _UpdateLocation.Territory2;

            para[52] = new SqlParameter();
            para[52].ParameterName = "STax2";
            para[52].SqlDbType = SqlDbType.VarChar;
            para[52].Value = _UpdateLocation.STax2;

            para[53] = new SqlParameter();
            para[53].ParameterName = "UTax";
            para[53].SqlDbType = SqlDbType.VarChar;
            para[53].Value = _UpdateLocation.UTax;

            para[54] = new SqlParameter();
            para[54].ParameterName = "Zone";
            para[54].SqlDbType = SqlDbType.Int;
            para[54].Value = _UpdateLocation.Zone;

            para[55] = new SqlParameter();
            para[55].ParameterName = "@UpdatedBy";
            para[55].SqlDbType = SqlDbType.VarChar;
            para[55].Value = _UpdateLocation.MOMUSer;

            para[56] = new SqlParameter();
            para[56].ParameterName = "Consult";
            para[56].SqlDbType = SqlDbType.Int;
            para[56].Value = _UpdateLocation.Consult;

            para[57] = new SqlParameter();
            para[57].ParameterName = "@Country";
            para[57].SqlDbType = SqlDbType.VarChar;
            para[57].Value = _UpdateLocation.Country;

            para[58] = new SqlParameter();
            para[58].ParameterName = "@RolCountry";
            para[58].SqlDbType = SqlDbType.VarChar;
            para[58].Value = _UpdateLocation.RolCountry;

            para[59] = new SqlParameter();
            para[59].ParameterName = "@NoCustomerStatement";
            para[59].SqlDbType = SqlDbType.Bit;
            para[59].Value = _UpdateLocation.NoCustomerStatement;


            para[60] = new SqlParameter();
            para[60].ParameterName = "@BusinessTypeID";
            para[60].SqlDbType = SqlDbType.Int;
            para[60].Value = _UpdateLocation.BusinessTypeID;

            para[61] = new SqlParameter();
            para[61].ParameterName = "@ApplyServiceTypeRule";
            para[61].SqlDbType = SqlDbType.Int;
            para[61].Value = ApplyServiceTypeRule;

            para[62] = new SqlParameter();
            para[62].ParameterName = "@ServiceTypeName";
            para[62].SqlDbType = SqlDbType.VarChar;
            para[62].Value = ServiceTypeName;


            para[63] = new SqlParameter();
            para[63].ParameterName = "@ProjectPerDepartmentCount";
            para[63].SqlDbType = SqlDbType.Int;
            para[63].Value = ProjectPerDepartmentCount;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomer(User objPropUser, bool CopyToLocAndJob = false)
        {
            SqlParameter[] para = new SqlParameter[44];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            para[12] = new SqlParameter();
            para[12].ParameterName = "ContactData";
            para[12].SqlDbType = SqlDbType.Structured;
            para[12].Value = objPropUser.ContactData;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Internet";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.Internet;

            para[14] = new SqlParameter();
            para[14].ParameterName = "CustomerId";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropUser.CustomerID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "contact";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.MainContact;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Phone";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Phone;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Website";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Website;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Email";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Email;

            para[19] = new SqlParameter();
            para[19].ParameterName = "cell";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.Cell;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Type";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objPropUser.Type;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Equipment";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = objPropUser.EquipID;

            //para[21] = new SqlParameter();
            //para[21].ParameterName = "TS";
            //para[21].SqlDbType = SqlDbType.SmallInt;
            //para[21].Value = objPropUser.IsTSDatabase;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageOwnerID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.AccountNo;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Billing";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = objPropUser.Billing;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Central";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropUser.Central;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@grpbywo";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = objPropUser.grpbyWO;

            para[26] = new SqlParameter();
            para[26].ParameterName = "@Docs";
            para[26].SqlDbType = SqlDbType.Structured;
            para[26].Value = objPropUser.dtDocs;

            para[27] = new SqlParameter();
            para[27].ParameterName = "@openticket";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = objPropUser.openticket;

            para[28] = new SqlParameter();
            para[28].ParameterName = "BillRate";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = objPropUser.BillRate;

            para[29] = new SqlParameter();
            para[29].ParameterName = "OT";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = objPropUser.RateOT;

            para[30] = new SqlParameter();
            para[30].ParameterName = "NT";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = objPropUser.RateNT;

            para[31] = new SqlParameter();
            para[31].ParameterName = "DT";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropUser.RateDT;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Travel";
            para[32].SqlDbType = SqlDbType.Decimal;
            para[32].Value = objPropUser.RateTravel;

            para[33] = new SqlParameter();
            para[33].ParameterName = "Mileage";
            para[33].SqlDbType = SqlDbType.Decimal;
            para[33].Value = objPropUser.MileageRate;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Fax";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = objPropUser.Fax;

            para[35] = new SqlParameter();
            para[35].ParameterName = "CopyToLocAndJob";
            para[35].SqlDbType = SqlDbType.Bit;
            para[35].Value = CopyToLocAndJob;

            para[36] = new SqlParameter();
            para[36].ParameterName = "EN";
            para[36].SqlDbType = SqlDbType.Int;
            para[36].Value = objPropUser.EN;


            para[37] = new SqlParameter();
            para[37].ParameterName = "Lat";
            para[37].SqlDbType = SqlDbType.VarChar;
            para[37].Value = objPropUser.Lat;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Lng";
            para[38].SqlDbType = SqlDbType.VarChar;
            para[38].Value = objPropUser.Lng;

            para[39] = new SqlParameter();
            para[39].ParameterName = "@UpdatedBy";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = objPropUser.MOMUSer;

            para[40] = new SqlParameter();
            para[40].ParameterName = "Custom1";
            para[40].SqlDbType = SqlDbType.VarChar;
            para[40].Value = objPropUser.Custom1;

            para[41] = new SqlParameter();
            para[41].ParameterName = "Custom2";
            para[41].SqlDbType = SqlDbType.VarChar;
            para[41].Value = objPropUser.Custom2;

            para[42] = new SqlParameter();
            para[42].ParameterName = "shutdownAlert";
            para[42].SqlDbType = SqlDbType.SmallInt;
            para[42].Value = objPropUser.ShutdownAlert;

            para[43] = new SqlParameter();
            para[43].ParameterName = "ShutdownMessage";
            para[43].SqlDbType = SqlDbType.VarChar;
            para[43].Value = objPropUser.ShutdownReason;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomer", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateCustomer(UpdateCustomerParam _UpdateCustomer, bool CopyToLocAndJob, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[44];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _UpdateCustomer.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _UpdateCustomer.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = _UpdateCustomer.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = _UpdateCustomer.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _UpdateCustomer.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = _UpdateCustomer.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = _UpdateCustomer.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = _UpdateCustomer.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = _UpdateCustomer.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = _UpdateCustomer.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = _UpdateCustomer.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = _UpdateCustomer.Schedule;

            para[12] = new SqlParameter();
            para[12].ParameterName = "ContactData";
            para[12].SqlDbType = SqlDbType.Structured;
            if (_UpdateCustomer.ContactData.Rows.Count > 0)
            {
                if (_UpdateCustomer.ContactData.Rows[0]["ContactID"].ToString() != "0")
                {
                    para[12].Value = _UpdateCustomer.ContactData;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ContactID", typeof(int));
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("Phone", typeof(string));
                    dt.Columns.Add("Fax", typeof(string));
                    dt.Columns.Add("Cell", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("Title", typeof(string));
                    dt.Columns.Add("EmailTicket", typeof(byte));
                    dt.Columns.Add("EmailRecInvoice", typeof(byte));
                    dt.Columns.Add("ShutdownAlert", typeof(byte));
                    dt.Columns.Add("EmailRecTestProp", typeof(byte));
                    para[12].Value = dt;
                }
            }

            para[13] = new SqlParameter();
            para[13].ParameterName = "Internet";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = _UpdateCustomer.Internet;

            para[14] = new SqlParameter();
            para[14].ParameterName = "CustomerId";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = _UpdateCustomer.CustomerID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "contact";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _UpdateCustomer.MainContact;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Phone";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _UpdateCustomer.Phone;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Website";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _UpdateCustomer.Website;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Email";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _UpdateCustomer.Email;

            para[19] = new SqlParameter();
            para[19].ParameterName = "cell";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _UpdateCustomer.Cell;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Type";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = _UpdateCustomer.Type;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Equipment";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = _UpdateCustomer.EquipID;

            //para[21] = new SqlParameter();
            //para[21].ParameterName = "TS";
            //para[21].SqlDbType = SqlDbType.SmallInt;
            //para[21].Value = _UpdateCustomer.IsTSDatabase;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageOwnerID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = _UpdateCustomer.AccountNo;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Billing";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = _UpdateCustomer.Billing;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Central";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = _UpdateCustomer.Central;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@grpbywo";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = _UpdateCustomer.grpbyWO;

            para[26] = new SqlParameter();
            para[26].ParameterName = "@Docs";
            para[26].SqlDbType = SqlDbType.Structured;

            if (_UpdateCustomer.dtDocs.Rows.Count > 0)
            {
                if (_UpdateCustomer.dtDocs.Rows[0]["ID"].ToString() != "0")
                {
                    para[26].Value = _UpdateCustomer.dtDocs;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Portal", typeof(int));
                    dt.Columns.Add("Remarks", typeof(string));
                    para[26].Value = dt;
                }
            }

            para[27] = new SqlParameter();
            para[27].ParameterName = "@openticket";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = _UpdateCustomer.openticket;

            para[28] = new SqlParameter();
            para[28].ParameterName = "BillRate";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = _UpdateCustomer.BillRate;

            para[29] = new SqlParameter();
            para[29].ParameterName = "OT";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = _UpdateCustomer.RateOT;

            para[30] = new SqlParameter();
            para[30].ParameterName = "NT";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = _UpdateCustomer.RateNT;

            para[31] = new SqlParameter();
            para[31].ParameterName = "DT";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = _UpdateCustomer.RateDT;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Travel";
            para[32].SqlDbType = SqlDbType.Decimal;
            para[32].Value = _UpdateCustomer.RateTravel;

            para[33] = new SqlParameter();
            para[33].ParameterName = "Mileage";
            para[33].SqlDbType = SqlDbType.Decimal;
            para[33].Value = _UpdateCustomer.MileageRate;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Fax";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = _UpdateCustomer.Fax;

            para[35] = new SqlParameter();
            para[35].ParameterName = "CopyToLocAndJob";
            para[35].SqlDbType = SqlDbType.Bit;
            para[35].Value = CopyToLocAndJob;

            para[36] = new SqlParameter();
            para[36].ParameterName = "EN";
            para[36].SqlDbType = SqlDbType.Int;
            para[36].Value = _UpdateCustomer.EN;


            para[37] = new SqlParameter();
            para[37].ParameterName = "Lat";
            para[37].SqlDbType = SqlDbType.VarChar;
            para[37].Value = _UpdateCustomer.Lat;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Lng";
            para[38].SqlDbType = SqlDbType.VarChar;
            para[38].Value = _UpdateCustomer.Lng;

            para[39] = new SqlParameter();
            para[39].ParameterName = "@UpdatedBy";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = _UpdateCustomer.MOMUSer;

            para[40] = new SqlParameter();
            para[40].ParameterName = "Custom1";
            para[40].SqlDbType = SqlDbType.VarChar;
            para[40].Value = _UpdateCustomer.Custom1;

            para[41] = new SqlParameter();
            para[41].ParameterName = "Custom2";
            para[41].SqlDbType = SqlDbType.VarChar;
            para[41].Value = _UpdateCustomer.Custom2;

            para[42] = new SqlParameter();
            para[42].ParameterName = "shutdownAlert";
            para[42].SqlDbType = SqlDbType.SmallInt;
            para[42].Value = _UpdateCustomer.ShutdownAlert;

            para[43] = new SqlParameter();
            para[43].ParameterName = "ShutdownMessage";
            para[43].SqlDbType = SqlDbType.VarChar;
            para[43].Value = _UpdateCustomer.ShutdownReason;
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateCustomer", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerContact(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.RolId;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateContact", para[0], para[1]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocationContact(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.RolId;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateLocContact", para[0], para[1]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCollectionContact(CollectionContacts objPropUser)
        {
            SqlParameter[] para = new SqlParameter[16];

            para[0] = new SqlParameter();
            para[0].ParameterName = "IsUpdate";
            para[0].SqlDbType = SqlDbType.Bit;
            para[0].Value = objPropUser.IsUpdate;

            para[1] = new SqlParameter();
            para[1].ParameterName = "ID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.ID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Name";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Name;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Phone";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.Phone;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Fax";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Fax;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Cell";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.Cell;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Email";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Email;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Title";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Title;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Tickets";
            para[8].SqlDbType = SqlDbType.Bit;
            para[8].Value = objPropUser.Tickets;

            para[9] = new SqlParameter();
            para[9].ParameterName = "InvoiceStatements";
            para[9].SqlDbType = SqlDbType.Bit;
            para[9].Value = objPropUser.InvoiceStatements;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Shutdown";
            para[10].SqlDbType = SqlDbType.Bit;
            para[10].Value = objPropUser.Shutdown;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Tests";
            para[11].SqlDbType = SqlDbType.Bit;
            para[11].Value = objPropUser.Tests;

            para[12] = new SqlParameter();
            para[12].ParameterName = "CType";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.CType;

            para[13] = new SqlParameter();
            para[13].ParameterName = "LocID";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.LocID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "CustID";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropUser.CustID;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddUpdateContacts", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddContactFromProjectScreen(User objPropUser, string ContactType, int ContactTypeID, int JobID)
        {
            SqlParameter[] para = new SqlParameter[5];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "ContactType";
            para[1].SqlDbType = SqlDbType.NVarChar;
            para[1].Value = ContactType;

            para[2] = new SqlParameter();
            para[2].ParameterName = "ContactTypeID";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = ContactTypeID;

            para[3] = new SqlParameter();
            para[3].ParameterName = "JobID";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = JobID;



            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddContactFromProjectScreen", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUserPermission(User objPropUser)
        {
            try
            {
                SqlParameter para = new SqlParameter();
                para.ParameterName = "@UserID";
                para.SqlDbType = SqlDbType.Int;
                para.Value = objPropUser.UserID;

                SqlParameter para1 = new SqlParameter();
                para1.ParameterName = "@Pages";
                para1.SqlDbType = SqlDbType.Structured;
                para1.Value = objPropUser.dtPageData;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddPagePermission", para, para1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBCustomerID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update owner set QBCustomerID='" + objPropUser.QBCustomerID + "' where ID=" + objPropUser.CustomerID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateQBCustomerID(UpdateQBCustomerIDParam _UpdateQBCustomerID, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "update owner set QBCustomerID='" + _UpdateQBCustomerID.QBCustomerID + "' where ID=" + _UpdateQBCustomerID.CustomerID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBsalestaxID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update stax set QBstaxID='" + objPropUser.QBSalesTaxID + "' where name='" + objPropUser.SalesTax + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateQBsalestaxID(UpdateQBsalestaxIDParam _UpdateQBsalestaxID, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "update stax set QBstaxID='" + _UpdateQBsalestaxID.QBSalesTaxID + "' where name='" + _UpdateQBsalestaxID.SalesTax + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBDepartmentID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update jobtype set QBjobtypeID='" + objPropUser.QBJobtypeID + "' where ID='" + objPropUser.DepartmentID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBInvID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update inv set QBinvID='" + objPropUser.QBInvID + "' where ID='" + objPropUser.BillCode + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBTermsID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update tblterms set QBtermsID='" + objPropUser.QBTermsID + "' where ID='" + objPropUser.TermsID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBAccountID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update chart set QBAccountID='" + objPropUser.QBAccountID + "' where ID='" + objPropUser.AccountID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBVendorID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update vendor set QBvendorID='" + objPropUser.QBAccountID + "' where ID='" + objPropUser.AccountID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBWageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update prwage set QBwageID='" + objPropUser.QBAccountID + "' where ID='" + objPropUser.AccountID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateQBJobtypeID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update loctype set QBloctypeID='" + objPropUser.QBCustomerTypeID + "' where type='" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateQBJobtypeID(UpdateQBJobtypeIDParam _UpdateQBJobtypeID, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "update loctype set QBloctypeID='" + _UpdateQBJobtypeID.QBCustomerTypeID + "' where type='" + _UpdateQBJobtypeID.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBcustomertypeID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update otype set QBcustomertypeID='" + objPropUser.QBCustomerTypeID + "' where type='" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateQBcustomertypeID(UpdateQBcustomertypeIDParam _UpdateQBcustomertypeID, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "update otype set QBcustomertypeID='" + _UpdateQBcustomertypeID.QBCustomerTypeID + "' where type='" + _UpdateQBcustomertypeID.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBLocationID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update loc set QBlocID='" + objPropUser.QBlocationID + "' where loc=" + objPropUser.LocID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateQBLocationID(UpdateQBLocationIDParam _UpdateQBLocationID, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "update loc set QBlocID='" + _UpdateQBLocationID.QBlocationID + "' where loc=" + _UpdateQBLocationID.LocID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreateDBObjects(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, objPropUser.Script);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Type,Remarks, (select Count(1)from Owner where Type= t.Type ) as Count from otype t");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getCustomerType(getCustomerTypeParam _getCustomerType, string ConnectionString)
        {
            try
            {
                return _getCustomerType.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select Type,Remarks, (select Count(1)from Owner where Type= t.Type ) as Count from otype t");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getVendorType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Type,Remarks, (select Count(1)from Vendor where Type= t.Type ) as Count from vtype t order by Type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCentral(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Central order by SortOrder");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getcategoryAll(User objPropUser)
        {
            string strCommandtext = "select Type As ImageID, REPLACE(Type, '$', '&') As Type, Remarks,icon, ((select Count(1)from TicketO where Cat= t.Type )+ (select COUNT(1)from TicketD where Cat=t.type)) as Count, isnull(Chargeable,0) as Chargeable, isnull(isdefault,0) as isdefault, case isnull(Status,1) when 1 then 'Active' when 0 then 'Inactive' End as Status from Category t";

            if (!string.IsNullOrEmpty(objPropUser.Cat))
            {
                strCommandtext += " where type='" + objPropUser.Cat + "'";
            }
            strCommandtext += " order by type";
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strCommandtext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEquiptype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from elev where type=e.edesc) as Count from ElevatorSpec e  where ecat=1 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getEquiptype(GetEquiptypeParam _GetEquiptype, string ConnectionString)
        {
            try
            {
                return _GetEquiptype.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select edesc,Label,(select count(1) from elev where type=e.edesc) as Count from ElevatorSpec e  where ecat=1 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLeadEquiptype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from LeadEquip where type=e.edesc) as Count from ElevatorSpec e  where ecat=1 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLeadEquiptype(GetLeadEquiptypeParam _GetLeadEquiptype, string ConnectionString)
        {
            try
            {
                return _GetLeadEquiptype.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select edesc,Label,(select count(1) from LeadEquip where type=e.edesc) as Count from ElevatorSpec e  where ecat=1 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getEquipmentCategory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from elev WITH (NOLOCK) where Category=e.edesc) as Count from ElevatorSpec e WITH (NOLOCK) WHERE ecat=0 ORDER BY edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getEquipmentCategory(GetEquipmentCategoryParam _GetEquipmentCategory, string ConnectionString)
        {
            try
            {
                return _GetEquipmentCategory.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select edesc,Label,(select count(1) from elev WITH (NOLOCK) where Category=e.edesc) as Count from ElevatorSpec e WITH (NOLOCK) WHERE ecat=0 ORDER BY edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLeadEquipmentCategory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from LeadEquip where Category=e.edesc) as Count from ElevatorSpec e WHERE ecat=0 ORDER BY edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLeadEquipmentCategory(GetLeadEquipmentCategoryParam _GetLeadEquipmentCategory, string ConnectionString)
        {
            try
            {
                return _GetLeadEquipmentCategory.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select edesc,Label,(select count(1) from LeadEquip where Category=e.edesc) as Count from ElevatorSpec e WHERE ecat=0 ORDER BY edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEquipShutdownReason(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from elev where Category=e.edesc) as Count from ElevatorSpec e WHERE ecat=0 ORDER BY edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMCPS(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT ID,Name  FROM tblMCPStatus ORDER BY name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getBuilding(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct building  from Elev where building !='' order by  building asc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getBuildingElev(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select EDesc,Label, (select count(1) from elev where Building=e.edesc) as Count from ElevatorSpec e  where ecat=2 order by EDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getBuildingElev(GetBuildingElevParam _GetBuildingElev, string ConnectionString)
        {
            try
            {
                return _GetBuildingElev.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select EDesc,Label, (select count(1) from elev where Building=e.edesc) as Count from ElevatorSpec e  where ecat=2 order by EDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getBuildingLeadEquip(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select EDesc,Label, (select count(1) from LeadEquip where Building=e.edesc) as Count from ElevatorSpec e  where ecat=2 order by EDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getBuildingLeadEquip(GetBuildingLeadEquipParam _GetBuildingLeadEquip, string ConnectionString)
        {
            try
            {
                return _GetBuildingLeadEquip.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select EDesc,Label, (select count(1) from LeadEquip where Building=e.edesc) as Count from ElevatorSpec e  where ecat=2 order by EDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getDepartment(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from jobtype order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getPayFrequencies(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "[payroll].[spGetPayFrequencies]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet fillSupervisor(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Emp order by CallSign");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Get Title
        public DataSet getTitle(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct CASE WHEN Emp.Title is null THEN 'ABC-Null' WHEN Emp.Title = '' THEN 'XYZ-Empty' When Emp.Title is not null and Emp.Title != '' then Emp.Title  END AS Title from Emp");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet getDepartment(GetDepartmentParam objPropUser, string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from jobtype order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDepartmentPercent(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetJobTypePercent");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public DataSet getSalesTax(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename,c.fDesc as AcctDesc,r.Name as VendorName from stax s INNER JOIN  Chart c ON s.GL=c.ID left JOIN Vendor v ON v.ID = s.Vendor left JOIN Rol r ON r.ID = v.Rol order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesTaxByTaxType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename, c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and  UType=" + objPropUser.UType + "  order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMSalesTax(User objPropUser)
        {
            string strQuery = "select isnull(IsTaxable,0) as IsTax,*, (select QBvendorID from vendor where acct='Mobile Service Manager' and QBvendorID is not null) as QBvendorID from stax  ";

            if (objPropUser.SearchValue == "1")
            {
                strQuery += " where QBStaxID is not null and LastUpdateDate >= (select QBLastSync from Control)";
            }
            else if (objPropUser.SearchValue == "0")
            {
                strQuery += " where QBStaxID is null";
            }
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

        //API
        public DataSet getMSMSalesTax(GetMSMSalesTaxParam _GetMSMSalesTax, string ConnectionString)
        {
            string strQuery = "select isnull(IsTaxable,0) as IsTax,*, (select QBvendorID from vendor where acct='Mobile Service Manager' and QBvendorID is not null) as QBvendorID from stax  ";

            if (_GetMSMSalesTax.SearchValue == "1")
            {
                strQuery += " where QBStaxID is not null and LastUpdateDate >= (select QBLastSync from Control)";
            }
            else if (_GetMSMSalesTax.SearchValue == "0")
            {
                strQuery += " where QBStaxID is null";
            }
            strQuery += " order by name";

            try
            {
                return _GetMSMSalesTax.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBSalesTax(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from stax where QBStaxID is not null order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMLoctype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from loctype where QBloctypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet getMSMLoctype(GetMSMLoctypeParam _GetMSMLoctype, string ConnectionString)
        {
            try
            {
                return _GetMSMLoctype.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from loctype where QBloctypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMDepartment(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from jobtype where QBjobtypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMBillcode(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetMSMBillcode", Convert.ToInt32(objPropUser.SearchValue));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMterms(User objPropUser)
        {
            string strQuery = "select *,(Name +' (' +CAST( ID as varchar(50)) + ')') as dupname from tblterms where QBtermsID is null ";

            if (!string.IsNullOrEmpty(objPropUser.SearchValue))
            {
                strQuery += "and ID in (" + objPropUser.SearchValue.Trim() + ")";
            }

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

        public DataSet getMSMAccount(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spQBGetAccount");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMPatrollWage(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spQBGetPayrollItem");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMVendor(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spQBGetVendor");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTerms(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblterms order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getTerms(GetTermsParam _GetTermsParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblterms order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getQBDepartment(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from jobtype WHERE  QBjobtypeID IS NOT NULL and LastUpdateDate >= (select QBLastSync from Control) order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMCustomertype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from otype where QBCustomerTypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getMSMCustomertype(GetMSMCustomertypeParam _GetMSMCustomertype, string ConnectionString)
        {
            try
            {
                return _GetMSMCustomertype.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from otype where QBCustomerTypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBCustomertype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from otype where QBCustomerTypeID is not null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesTaxByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from stax where name='" + objPropUser.Stax + "' order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddSalesTax(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("IF NOT Exists (select 1 from STax where Name = '" + objPropUser.SalesTax + "' )\n");
            QueryText.Append("BEGIN \n");
            QueryText.Append("INSERT INTO STax \n");
            QueryText.Append("            (Name, \n");
            QueryText.Append("             fDesc, \n");
            QueryText.Append("             Rate, \n");
            QueryText.Append("             State, \n");
            QueryText.Append("             GL, \n");
            QueryText.Append("             Type, \n");
            QueryText.Append("             UType, \n");
            QueryText.Append("             PstReg, \n");
            QueryText.Append("             lastupdatedate, \n");
            QueryText.Append("             Remarks,Vendor) \n");
            QueryText.Append("VALUES      ( '" + objPropUser.SalesTax + "', \n");
            QueryText.Append("              '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("              " + objPropUser.SalesRate + ", \n");
            QueryText.Append("              '" + objPropUser.State + "', \n");
            QueryText.Append("              '" + objPropUser.GLAccount + "', \n");
            QueryText.Append("              " + objPropUser.sType + ", \n");
            QueryText.Append("              '" + objPropUser.UType + "', \n");
            QueryText.Append("              '" + objPropUser.PSTReg + "', \n");
            QueryText.Append("              getdate(), \n");
            QueryText.Append("              '" + objPropUser.Remarks + "' , \n");
            QueryText.Append("              '" + objPropUser.Vendor + "' ) \n");
            QueryText.Append("END \n");
            QueryText.Append("ELSE \n");
            QueryText.Append("BEGIN \n");
            QueryText.Append("RAISERROR ('Name already exists, please use different name !',16,1) \n");
            QueryText.Append("RETURN \n");
            QueryText.Append("END \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSalesTax(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("UPDATE STax \n");
            QueryText.Append("SET    fDesc = '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("       Rate = " + objPropUser.SalesRate + ", \n");
            QueryText.Append("       State = '" + objPropUser.State + "', \n");
            QueryText.Append("       lastupdatedate = getdate(), \n");
            QueryText.Append("       Remarks = '" + objPropUser.Remarks + "', \n");
            QueryText.Append("       Type = " + objPropUser.sType + ", \n");
            QueryText.Append("       PSTReg = '" + objPropUser.PSTReg + "', \n");
            QueryText.Append("       GL = " + objPropUser.GLAccount + ", \n");
            QueryText.Append("       UType = " + objPropUser.UType + ", \n");
            QueryText.Append("       Vendor = " + objPropUser.Vendor + " \n");
            QueryText.Append("WHERE  Name = '" + objPropUser.SalesTax + "' ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //consultant

        public DataSet IsConsultNameExist(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select count(*) as name from Rol where Name='" + objPropUser.FirstName + "'");
                ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddConsult(tblConsult objPropConsult)
        {
            SqlParameter[] para = new SqlParameter[7];
            para[0] = new SqlParameter();
            para[0].ParameterName = "RolName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropConsult.RolName;

            para[1] = new SqlParameter();
            para[1].ParameterName = "RolID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropConsult.RolID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Count";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropConsult.Count;

            para[3] = new SqlParameter();
            para[3].ParameterName = "API";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropConsult.API;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Username";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropConsult.Username;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Password";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropConsult.Password;

            para[6] = new SqlParameter();
            para[6].ParameterName = "IP";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropConsult.IP;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropConsult.ConnConfig, CommandType.StoredProcedure, "spAddConsultant", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int AddRol(User objPropUser)
        {
            int rolID;
            SqlParameter[] para = new SqlParameter[37];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Internet";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.Internet;

            para[14] = new SqlParameter();
            para[14].ParameterName = "contact";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.MainContact;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Phone";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Phone;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Website";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Website;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Email";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Email;

            para[18] = new SqlParameter();
            para[18].ParameterName = "cell";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Cell;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Type";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.Type;

            para[20] = new SqlParameter();
            para[20].ParameterName = "returnval";
            para[20].SqlDbType = SqlDbType.Int;
            para[20].Direction = ParameterDirection.ReturnValue;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Equipment";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = objPropUser.EquipID;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.AccountNo;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Billing";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = objPropUser.Billing;

            para[24] = new SqlParameter();
            para[24].ParameterName = "@grpbywo";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropUser.grpbyWO;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@openticket";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = objPropUser.openticket;

            para[26] = new SqlParameter();
            para[26].ParameterName = "BillRate";
            para[26].SqlDbType = SqlDbType.Decimal;
            para[26].Value = objPropUser.BillRate;

            para[27] = new SqlParameter();
            para[27].ParameterName = "OT";
            para[27].SqlDbType = SqlDbType.Decimal;
            para[27].Value = objPropUser.RateOT;

            para[28] = new SqlParameter();
            para[28].ParameterName = "NT";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = objPropUser.RateNT;

            para[29] = new SqlParameter();
            para[29].ParameterName = "DT";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = objPropUser.RateDT;

            para[30] = new SqlParameter();
            para[30].ParameterName = "Travel";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = objPropUser.RateTravel;

            para[31] = new SqlParameter();
            para[31].ParameterName = "Mileage";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropUser.MileageRate;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Fax";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = objPropUser.Fax;

            para[33] = new SqlParameter();
            para[33].ParameterName = "EN";
            para[33].SqlDbType = SqlDbType.Int;
            para[33].Value = objPropUser.EN;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Lat";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = objPropUser.Lat;

            para[35] = new SqlParameter();
            para[35].ParameterName = "Lng";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = objPropUser.Lng;

            para[36] = new SqlParameter();
            para[36].ParameterName = "@UpdatedBy";
            para[36].SqlDbType = SqlDbType.VarChar;
            para[36].Value = objPropUser.MOMUSer;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddRol", para);

                rolID = Convert.ToInt32(para[20].Value);
                objPropUser.CustomerID = rolID;
                return rolID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void AddQBSalesTax(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   STax \n");
            varname1.Append("              WHERE  QBStaxID = '" + objPropUser.QBSalesTaxID + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      INSERT INTO STax \n");
            varname1.Append("                  (Name, \n");
            varname1.Append("                   fDesc, \n");
            varname1.Append("                   Rate, \n");
            varname1.Append("                   State, \n");
            varname1.Append("                   IsTaxable, \n");
            varname1.Append("                   GL, \n");
            varname1.Append("                   Type, \n");
            varname1.Append("                   UType, \n");
            varname1.Append("                   PstReg, \n");
            varname1.Append("                   QBStaxID, \n");
            varname1.Append("                   Remarks) \n");
            varname1.Append("VALUES      ( '" + objPropUser.SalesTax + "', \n");
            varname1.Append("              '" + objPropUser.SalesDescription + "', \n");
            varname1.Append("              " + objPropUser.SalesRate + ", \n");
            varname1.Append("              '" + objPropUser.State + "', \n");
            varname1.Append("              " + objPropUser.IsTaxable + ", \n");
            varname1.Append("              (select top 1 ID from chart where fdesc='Mobile Service Manager'), \n");
            varname1.Append("              0, \n");
            varname1.Append("              1, \n");
            varname1.Append("              '', \n");
            varname1.Append("              '" + objPropUser.QBSalesTaxID + "', \n");
            varname1.Append("              '" + objPropUser.Remarks + "' ) \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      UPDATE STax \n");
            varname1.Append("      SET    fDesc = '" + objPropUser.SalesDescription + "', \n");
            varname1.Append("             Rate = " + objPropUser.SalesRate + " \n");
            varname1.Append("      WHERE  QBStaxID = '" + objPropUser.QBSalesTaxID + "' AND Isnull(LastUpdateDate, '01/01/1900') < '" + objPropUser.LastUpdateDate + "' \n");
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

        //API
        public void AddQBSalesTax(AddQBSalesTaxParam _AddQBSalesTax, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   STax \n");
            varname1.Append("              WHERE  QBStaxID = '" + _AddQBSalesTax.QBSalesTaxID + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      INSERT INTO STax \n");
            varname1.Append("                  (Name, \n");
            varname1.Append("                   fDesc, \n");
            varname1.Append("                   Rate, \n");
            varname1.Append("                   State, \n");
            varname1.Append("                   IsTaxable, \n");
            varname1.Append("                   GL, \n");
            varname1.Append("                   Type, \n");
            varname1.Append("                   UType, \n");
            varname1.Append("                   PstReg, \n");
            varname1.Append("                   QBStaxID, \n");
            varname1.Append("                   Remarks) \n");
            varname1.Append("VALUES      ( '" + _AddQBSalesTax.SalesTax + "', \n");
            varname1.Append("              '" + _AddQBSalesTax.SalesDescription + "', \n");
            varname1.Append("              " + _AddQBSalesTax.SalesRate + ", \n");
            varname1.Append("              '" + _AddQBSalesTax.State + "', \n");
            varname1.Append("              " + _AddQBSalesTax.IsTaxable + ", \n");
            varname1.Append("              (select top 1 ID from chart where fdesc='Mobile Service Manager'), \n");
            varname1.Append("              0, \n");
            varname1.Append("              1, \n");
            varname1.Append("              '', \n");
            varname1.Append("              '" + _AddQBSalesTax.QBSalesTaxID + "', \n");
            varname1.Append("              '" + _AddQBSalesTax.Remarks + "' ) \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      UPDATE STax \n");
            varname1.Append("      SET    fDesc = '" + _AddQBSalesTax.SalesDescription + "', \n");
            varname1.Append("             Rate = " + _AddQBSalesTax.SalesRate + " \n");
            varname1.Append("      WHERE  QBStaxID = '" + _AddQBSalesTax.QBSalesTaxID + "' AND Isnull(LastUpdateDate, '01/01/1900') < '" + _AddQBSalesTax.LastUpdateDate + "' \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddDepartment(User objPropUser)
        {
            //StringBuilder QueryText = new StringBuilder();
            //QueryText.Append("update JobType set isdefault =0  INSERT INTO JobType \n");
            ////QueryText.Append("            (ID, \n");
            //QueryText.Append("            ( Type, \n");
            //QueryText.Append("              isdefault, \n");
            //QueryText.Append("             Remarks, \n");
            //QueryText.Append("             LastUpdateDate) \n");
            ////QueryText.Append("VALUES      ( 0, \n");
            //QueryText.Append("VALUES      ( '" + objPropUser.Type + "', \n");
            //QueryText.Append("       " + objPropUser.Default + ", \n");
            ////QueryText.Append("              '"+objPropUser.Type+"', \n");
            //QueryText.Append("              '" + objPropUser.Remarks + "' , ");
            //QueryText.Append("              GETDATE() ) ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddDepartment", objPropUser.Type, objPropUser.Default, objPropUser.Remarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddWage(Wage _objWage)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, "spAddWage", _objWage.Name, _objWage.Field, _objWage.Reg, _objWage.OT1, _objWage.OT2, _objWage.TT, _objWage.FIT, _objWage.FICA, _objWage.MEDI, _objWage.FUTA, _objWage.SIT, _objWage.Vac, _objWage.WC, _objWage.Uni, _objWage.GL, _objWage.NT, _objWage.MileageGL, _objWage.ReimGL, _objWage.ZoneGL, _objWage.Globe, _objWage.Status, _objWage.CReg, _objWage.COT, _objWage.CDT, _objWage.CNT, _objWage.CTT, _objWage.Remarks, _objWage.RegGL, _objWage.OTGL, _objWage.NTGL, _objWage.DTGL, _objWage.TTGL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBDepartment(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   JobType \n");
            //varname1.Append("              WHERE  QBJobTypeID = '" + objPropUser.QBJobtypeID + "') \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      INSERT INTO JobType \n");
            //varname1.Append("                  (type, \n");
            //varname1.Append("                   remarks, \n");
            //varname1.Append("                   isdefault, \n");
            //varname1.Append("                   QBJobTypeID) \n");
            ////varname1.Append("                   LastUpdateDate) \n");
            //varname1.Append("      VALUES      ('" + objPropUser.Type + "', \n");
            //varname1.Append("                   '" + objPropUser.Remarks + "', \n");
            //varname1.Append("                   0, \n");
            //varname1.Append("                   '" + objPropUser.QBJobtypeID + "') \n");
            ////varname1.Append("                   Getdate()) \n");
            //varname1.Append("  END \n");           

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "AddQbJobType", objPropUser.QBJobtypeID, objPropUser.Type, objPropUser.Remarks, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBTerms(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQbTerms", objPropUser.QBTermsID, objPropUser.Type, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBPayrollWage(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQBPayrollWage", objPropUser.QBWageID, objPropUser.Type, objPropUser.LastUpdateDate, objPropUser.QBAccountID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDepartment(User objPropUser)
        {
            ////StringBuilder QueryText = new StringBuilder();
            ////QueryText.Append("UPDATE JobType \n");
            ////QueryText.Append("SET    Remarks = '" + objPropUser.Remarks + "' \n");            
            ////QueryText.Append("WHERE  type = '" + objPropUser.Type + "' ");

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("update JobType set isdefault =0  UPDATE JobType \n");
            //varname1.Append("SET    Remarks = '" + objPropUser.Remarks + "', \n");
            //varname1.Append("    isdefault = " + objPropUser.Default + ", \n");
            //varname1.Append("       Type = '" + objPropUser.Type + "', \n");
            //varname1.Append("       LastUpdateDate = GETDATE() \n");
            //varname1.Append("WHERE  ID = " + objPropUser.JobtypeID + " ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateDepartment", objPropUser.Type, objPropUser.Default, objPropUser.Remarks, objPropUser.JobtypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateWage(Wage _objWage)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[33];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objWage.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Name";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _objWage.Name;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Field";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _objWage.Field;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Reg";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = _objWage.Reg;

                para[4] = new SqlParameter();
                para[4].ParameterName = "OT1";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = _objWage.OT1;

                para[5] = new SqlParameter();
                para[5].ParameterName = "OT2";
                para[5].SqlDbType = SqlDbType.Decimal;
                para[5].Value = _objWage.OT2;

                para[6] = new SqlParameter();
                para[6].ParameterName = "TT";
                para[6].SqlDbType = SqlDbType.Decimal;
                para[6].Value = _objWage.TT;

                para[7] = new SqlParameter();
                para[7].ParameterName = "FIT";
                para[7].SqlDbType = SqlDbType.SmallInt;
                para[7].Value = _objWage.FIT;

                para[8] = new SqlParameter();
                para[8].ParameterName = "FICA";
                para[8].SqlDbType = SqlDbType.SmallInt;
                para[8].Value = _objWage.FICA;

                para[9] = new SqlParameter();
                para[9].ParameterName = "MEDI";
                para[9].SqlDbType = SqlDbType.SmallInt;
                para[9].Value = _objWage.MEDI;

                para[10] = new SqlParameter();
                para[10].ParameterName = "FUTA";
                para[10].SqlDbType = SqlDbType.SmallInt;
                para[10].Value = _objWage.FUTA;

                para[11] = new SqlParameter();
                para[11].ParameterName = "SIT";
                para[11].SqlDbType = SqlDbType.SmallInt;
                para[11].Value = _objWage.SIT;

                para[12] = new SqlParameter();
                para[12].ParameterName = "Vac";
                para[12].SqlDbType = SqlDbType.SmallInt;
                para[12].Value = _objWage.Vac;

                para[13] = new SqlParameter();
                para[13].ParameterName = "WC";
                para[13].SqlDbType = SqlDbType.SmallInt;
                para[13].Value = _objWage.WC;

                para[14] = new SqlParameter();
                para[14].ParameterName = "Uni";
                para[14].SqlDbType = SqlDbType.SmallInt;
                para[14].Value = _objWage.Uni;

                para[15] = new SqlParameter();
                para[15].ParameterName = "GL";
                para[15].SqlDbType = SqlDbType.Int;
                para[15].Value = _objWage.GL;

                para[16] = new SqlParameter();
                para[16].ParameterName = "NT";
                para[16].SqlDbType = SqlDbType.Decimal;
                para[16].Value = _objWage.NT;

                para[17] = new SqlParameter();
                para[17].ParameterName = "MileageGL";
                para[17].SqlDbType = SqlDbType.Int;
                para[17].Value = _objWage.MileageGL;

                para[18] = new SqlParameter();
                para[18].ParameterName = "ReimGL";
                para[18].SqlDbType = SqlDbType.Int;
                para[18].Value = _objWage.ReimGL;

                para[19] = new SqlParameter();
                para[19].ParameterName = "ZoneGL";
                para[19].SqlDbType = SqlDbType.Int;
                para[19].Value = _objWage.ZoneGL;

                para[20] = new SqlParameter();
                para[20].ParameterName = "Globe";
                para[20].SqlDbType = SqlDbType.SmallInt;
                para[20].Value = _objWage.Globe;

                para[21] = new SqlParameter();
                para[21].ParameterName = "Status";
                para[21].SqlDbType = SqlDbType.SmallInt;
                para[21].Value = _objWage.Status;

                para[22] = new SqlParameter();
                para[22].ParameterName = "CReg";
                para[22].SqlDbType = SqlDbType.Decimal;
                para[22].Value = _objWage.CReg;

                para[23] = new SqlParameter();
                para[23].ParameterName = "COT";
                para[23].SqlDbType = SqlDbType.Decimal;
                para[23].Value = _objWage.COT;

                para[24] = new SqlParameter();
                para[24].ParameterName = "CDT";
                para[24].SqlDbType = SqlDbType.Decimal;
                para[24].Value = _objWage.CDT;

                para[25] = new SqlParameter();
                para[25].ParameterName = "CNT";
                para[25].SqlDbType = SqlDbType.Decimal;
                para[25].Value = _objWage.CNT;

                para[26] = new SqlParameter();
                para[26].ParameterName = "CTT";
                para[26].SqlDbType = SqlDbType.Decimal;
                para[26].Value = _objWage.CTT;

                para[27] = new SqlParameter();
                para[27].ParameterName = "Remarks";
                para[27].SqlDbType = SqlDbType.VarChar;
                para[27].Value = _objWage.Remarks;

                para[28] = new SqlParameter();
                para[28].ParameterName = "RegGL";
                para[28].SqlDbType = SqlDbType.Int;
                para[28].Value = _objWage.RegGL;

                para[29] = new SqlParameter();
                para[29].ParameterName = "OTGL";
                para[29].SqlDbType = SqlDbType.Int;
                para[29].Value = _objWage.OTGL;

                para[30] = new SqlParameter();
                para[30].ParameterName = "NTGL";
                para[30].SqlDbType = SqlDbType.Int;
                para[30].Value = _objWage.NTGL;

                para[31] = new SqlParameter();
                para[31].ParameterName = "DTGL";
                para[31].SqlDbType = SqlDbType.Int;
                para[31].Value = _objWage.DTGL;

                para[32] = new SqlParameter();
                para[32].ParameterName = "TTGL";
                para[32].SqlDbType = SqlDbType.Int;
                para[32].Value = _objWage.TTGL;

                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, CommandType.StoredProcedure, "spUpdateWage", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddBillCode(User objPropUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[10];

                para[0] = new SqlParameter();
                para[0].ParameterName = "BillCode";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropUser.BillCode;

                para[1] = new SqlParameter();
                para[1].ParameterName = "ContactName";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = objPropUser.ContactName;

                para[2] = new SqlParameter();
                para[2].ParameterName = "SalesDescription";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = objPropUser.SalesDescription;

                para[3] = new SqlParameter();
                para[3].ParameterName = "CatStatus";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = objPropUser.CatStatus;

                para[4] = new SqlParameter();
                para[4].ParameterName = "Balance";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = objPropUser.Balance;

                para[5] = new SqlParameter();
                para[5].ParameterName = "Measure";
                para[5].SqlDbType = SqlDbType.VarChar;
                para[5].Value = objPropUser.Measure;


                para[6] = new SqlParameter();
                para[6].ParameterName = "Type";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = objPropUser.Type;

                para[7] = new SqlParameter();
                para[7].ParameterName = "sAcct";
                para[7].SqlDbType = SqlDbType.Int;
                para[7].Value = objPropUser.sAcct;

                para[8] = new SqlParameter();
                para[8].ParameterName = "Remarks";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = objPropUser.Remarks;

                para[9] = new SqlParameter();
                para[9].ParameterName = "WarehouseID";
                para[9].SqlDbType = SqlDbType.VarChar;
                para[9].Value = objPropUser.WarehouseID;



                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddBillCode", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }





            //StringBuilder QueryText = new StringBuilder();
            //QueryText.Append("INSERT INTO Inv \n");
            //QueryText.Append("            (Name, \n");
            //QueryText.Append("             fDesc, \n");
            //QueryText.Append("             status, \n");
            ////QueryText.Append("             Balance, \n");
            //QueryText.Append("             Price1, \n");
            //QueryText.Append("             Measure, \n");
            //QueryText.Append("             tax, \n");
            //QueryText.Append("             AllowZero, \n");
            //QueryText.Append("             inuse, \n");
            //QueryText.Append("             type, \n");
            //QueryText.Append("             sacct, \n");
            //QueryText.Append("             Remarks, \n");
            //QueryText.Append("             lastupdatedate, \n");
            //QueryText.Append("             cat, warehouse) \n");
            //QueryText.Append("VALUES      ( '" + objPropUser.ContactName + "', \n");
            //QueryText.Append("              '" + objPropUser.SalesDescription + "', \n");
            //QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            //QueryText.Append("              " + objPropUser.Balance + ", \n");
            //QueryText.Append("              '" + objPropUser.Measure + "', \n");
            //QueryText.Append("              0, \n");
            //QueryText.Append("              0, \n");
            //QueryText.Append("              0, \n");
            //QueryText.Append("              " + objPropUser.Type + ", \n");
            //QueryText.Append("              " + objPropUser.sAcct + ", \n");
            //QueryText.Append("              '" + objPropUser.Remarks + " ', ");
            //QueryText.Append("              getdate(), ");
            //QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            //QueryText.Append("              '" + objPropUser.WarehouseID + " ') ");

            //try
            //{
            //    SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void AddQBBillCode(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("IF NOT EXISTS(SELECT 1 \n");
            QueryText.Append("              FROM   Inv \n");
            QueryText.Append("              WHERE  QBInvID = '" + objPropUser.QBInvID + "') \n");
            QueryText.Append("  BEGIN \n");
            QueryText.Append("INSERT INTO Inv \n");
            QueryText.Append("            (Name, \n");
            QueryText.Append("             fDesc, \n");
            QueryText.Append("             Cat, \n");
            QueryText.Append("             status, \n");
            //QueryText.Append("             Balance, \n");
            QueryText.Append("             Price1, \n");
            QueryText.Append("             Measure, \n");
            QueryText.Append("             tax, \n");
            QueryText.Append("             AllowZero, \n");
            QueryText.Append("             inuse, \n");
            QueryText.Append("             type, \n");
            QueryText.Append("             sacct, \n");
            QueryText.Append("             Remarks, \n");
            QueryText.Append("             warehouse, \n");
            QueryText.Append("             QBAccountID, \n");
            QueryText.Append("                   QBInvID) \n");
            QueryText.Append("VALUES      ( '" + objPropUser.ContactName + "', \n");
            QueryText.Append("              '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            QueryText.Append("              " + objPropUser.Balance + ", \n");
            QueryText.Append("              '" + objPropUser.Measure + "', \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              " + objPropUser.Type + ", \n");
            QueryText.Append("              10, \n");
            QueryText.Append("              '" + objPropUser.Remarks + " ', ");
            QueryText.Append("              '" + objPropUser.WarehouseID + " ', ");
            QueryText.Append("              '" + objPropUser.QBAccountID + " ', ");
            QueryText.Append("                   '" + objPropUser.QBInvID + "') \n");
            QueryText.Append("  END \n");

            QueryText.Append("  else \n");
            QueryText.Append("  begin \n");
            QueryText.Append("UPDATE Inv \n");
            QueryText.Append("SET Name = '" + objPropUser.ContactName + "', \n");
            QueryText.Append("    fDesc = '" + objPropUser.SalesDescription + "', \n");
            //QueryText.Append("    Balance = " + objPropUser.Balance + ", \n");
            QueryText.Append("    Price1 = " + objPropUser.Balance + ", \n");
            QueryText.Append("    QBAccountID = '" + objPropUser.QBAccountID + "', \n");
            QueryText.Append("    Remarks = '" + objPropUser.Remarks + "' \n");
            QueryText.Append("WHERE  QBInvID = '" + objPropUser.QBInvID + "' \n");
            QueryText.Append("       AND Isnull(LastUpdateDate, '01/01/1900') < '" + objPropUser.LastUpdateDate + "' ");
            QueryText.Append("  END \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQBBillCode", objPropUser.QBInvID, objPropUser.ContactName, objPropUser.SalesDescription, objPropUser.CatStatus, objPropUser.Balance, objPropUser.Measure, objPropUser.Type, objPropUser.Remarks, objPropUser.WarehouseID, objPropUser.QBAccountID, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void UpdateBillCode(User objPropUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[10];

                para[0] = new SqlParameter();
                para[0].ParameterName = "BillCode";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropUser.BillCode;

                para[1] = new SqlParameter();
                para[1].ParameterName = "ContactName";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = objPropUser.ContactName;

                para[2] = new SqlParameter();
                para[2].ParameterName = "SalesDescription";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = objPropUser.SalesDescription;

                para[3] = new SqlParameter();
                para[3].ParameterName = "CatStatus";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = objPropUser.CatStatus;

                para[4] = new SqlParameter();
                para[4].ParameterName = "Balance";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = objPropUser.Balance;

                para[5] = new SqlParameter();
                para[5].ParameterName = "Measure";
                para[5].SqlDbType = SqlDbType.VarChar;
                para[5].Value = objPropUser.Measure;


                para[6] = new SqlParameter();
                para[6].ParameterName = "Type";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = objPropUser.Type;

                para[7] = new SqlParameter();
                para[7].ParameterName = "sAcct";
                para[7].SqlDbType = SqlDbType.Int;
                para[7].Value = objPropUser.sAcct;

                para[8] = new SqlParameter();
                para[8].ParameterName = "Remarks";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = objPropUser.Remarks;

                para[9] = new SqlParameter();
                para[9].ParameterName = "WarehouseID";
                para[9].SqlDbType = SqlDbType.VarChar;
                para[9].Value = objPropUser.WarehouseID;



                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddBillCode", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //StringBuilder QueryText = new StringBuilder();
            //QueryText.Append("UPDATE Inv \n");
            //QueryText.Append("SET    Name = '" + objPropUser.ContactName + "', \n");
            //QueryText.Append("       fDesc = '" + objPropUser.SalesDescription + "', \n");
            //QueryText.Append("       status = " + objPropUser.CatStatus + ", \n");
            //QueryText.Append("       cat = " + objPropUser.CatStatus + ", \n");
            ////QueryText.Append("       Balance = " + objPropUser.Balance + ", \n");
            //QueryText.Append("       Price1 = " + objPropUser.Balance + ", \n");
            //QueryText.Append("       Measure = '" + objPropUser.Measure + "', \n");
            //QueryText.Append("       sacct = " + objPropUser.sAcct + ", \n");
            //QueryText.Append("       Remarks = '" + objPropUser.Remarks + " ', \n");
            //QueryText.Append("       lastupdatedate = getdate(), \n");
            //QueryText.Append("       warehouse = '" + objPropUser.WarehouseID + " ' \n");
            //QueryText.Append("WHERE  ID = " + objPropUser.BillCode + " ");

            //try
            //{
            //    SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void UpdateWarehouse(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update Warehouse set Name='" + objPropUser.WarehouseName + "' ,Remarks='" + objPropUser.Remarks + "' where ID='" + objPropUser.WarehouseID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertWareHouse(User objPropUser)
        {
            try
            {

                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("IF NOT Exists (select 1 from Warehouse where ID = '" + objPropUser.WarehouseID + "' )\n");
                QueryText.Append("BEGIN \n");
                QueryText.Append("INSERT INTO Warehouse \n");
                QueryText.Append("            (ID, \n");
                QueryText.Append("             Name, \n");
                QueryText.Append("             Remarks) \n");

                QueryText.Append("VALUES      ( '" + objPropUser.WarehouseID + "', \n");
                QueryText.Append("              '" + objPropUser.WarehouseName + "', \n");
                QueryText.Append("              '" + objPropUser.Remarks + "') \n");
                QueryText.Append("END \n");
                QueryText.Append("ELSE \n");
                QueryText.Append("BEGIN \n");
                QueryText.Append("RAISERROR ('ID already exists, please use different id !',16,1) \n");
                QueryText.Append("RETURN \n");
                QueryText.Append("END \n");


                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into Warehouse(ID,Name,Remarks) values('" + objPropUser.WarehouseID + "','" + objPropUser.WarehouseName + "','" + objPropUser.Remarks + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void DeleteDiagnostic(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete Diagnostic where category='" + objPropUser.Category + "'  and type='" + objPropUser.DiagnosticType + "'  and fdesc='" + objPropUser.Remarks + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteCustomerQB(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from Rol where ID=(select Rol from Owner where QBCustomerID='" + objPropUser.QBCustomerID + "') delete from Owner where QBCustomerID='" + objPropUser.QBCustomerID + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getlocationType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Type,Remarks, (select Count(1)from Loc where Type= t.Type ) as Count from loctype t order by Type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getlocationType(getlocationTypeParam _getlocationType, string ConnectionString)
        {
            try
            {
                return _getlocationType.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select Type,Remarks, (select Count(1)from Loc where Type= t.Type ) as Count from loctype t order by Type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //get consultant
        public DataSet getConsultant(tblConsult objPropConsult)
        {
            try
            {
                return objPropConsult.DsConsultant = SqlHelper.ExecuteDataset(objPropConsult.ConnConfig, CommandType.Text, "SELECT * FROM tblConsult INNER JOIN Rol ON tblConsult.Rol = Rol.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSingleConsultant(tblConsult objPropConsult)
        {
            try
            {
                return objPropConsult.DsConsultant = SqlHelper.ExecuteDataset(objPropConsult.ConnConfig, CommandType.Text, "SELECT * FROM tblConsult Order By Name ASC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getSingleConsultant(GetSingleConsultantParam _GetSingleConsultant, string ConnectionString)
        {
            try
            {
                return _GetSingleConsultant.DsConsultant = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT * FROM tblConsult Order By Name ASC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomerForReport(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Name, City, State, Zip, Address, GeoLock, Remarks, o.Type, Country, fLogin, Password, Status, TicketO, TicketD, Internet, Rol, Contact, Phone, Website,EMail, Cellular,(Address+', '+City+', '+State+', '+Zip) as addressfull from Owner o left outer join Rol r on o.Rol=r.ID where o.ID=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet gettrial(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select * from tblAuth");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet gettrialUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select ua.* from tbluserAuth ua inner join tblJoinAuth ja on ua.ID=ja.lid where ja.status=0 and ua.used=1 and ja.userid=" + objPropUser.UserID + " and ja.dbname='" + objPropUser.DBName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLicenseInfoUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "SELECT ID as lid,str,DBname,used,dateupdate FROM tblUserAuth where dbname='" + objPropUser.DBName + "' and used=0 order by dateupdate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTrial(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tblauth set str='" + objPropUser.Reg + "', [date]=GETDATE() , [first]=1 where first=0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReg(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tblauth set [date]=GETDATE(), str='" + objPropUser.Reg + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateRegUser(User objPropUser)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "if not exists(select 1 from tblUserAuth where UserID=" + objPropUser.UserID + " and DBname='" + objPropUser.DBName + "') begin insert into tblUserAuth ( DBname, UserID, str ) values ( '" + objPropUser.DBName + "', " + objPropUser.UserID + ", '" + objPropUser.Reg + "' ) end else begin update tbluserAuth set str='" + objPropUser.Reg + "' where UserID=" + objPropUser.UserID + " and DBname='" + objPropUser.DBName + "' end");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateRegUser(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spCheck", objPropUser.UserID, objPropUser.Reg, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRolCoordinates(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "IF NOT EXISTS(SELECT column_name 'Column_Name' FROM information_schema.columns WHERE table_name = 'rol' AND column_name = 'lng') BEGIN ALTER TABLE rol ADD lat VARCHAR(50) NULL, lng VARCHAR(50) NULL END    update Rol set lat='" + objPropUser.Lat + "' , lng='" + objPropUser.Lng + "' where id=" + objPropUser.RolId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWarehouse(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Warehouse");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserEmail(User objPropUser)
        {
            try
            {
                return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(r.email,'') as email from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where fuser='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public string getUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString)
        {
            try
            {
                return _GetUserEmail.Email = Convert.ToString(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "select isnull(r.email,'') as email from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where fuser='" + _GetUserEmail.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserEmailByUserId(User objPropUser)
        {
            try
            {
                return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(r.email,'') as email from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where u.EmailAccount = 1 and u.id='" + objPropUser.UserID + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserPager(User objPropUser)
        {
            try
            {
                return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(e.pager,'') as pager from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where fuser='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserDeviceID(User objPropUser)
        {
            try
            {
                return objPropUser.DeviceID = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select deviceid from emp where callsign='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDefaultWorkerLocation(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update Loc set Route =(select top 1 ID from Route where Name='" + objPropUser.Username + "' ) where Address like '%" + objPropUser.Address + "%' and City like '" + objPropUser.City + "%'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocationAddress(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateLocationAddress", objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Lat, objPropUser.Lng, objPropUser.LocID, objPropUser.RolId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UserRegistrationTransfer(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUserReg", objPropUser.UserLic, objPropUser.UserLicID, objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetUserSyncStatus(User objPropUser)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, " select isnull(QBFirstSync,1) as QBFirstSync from control"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSyncItems(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " select isnull(QBFirstSync,1) as QBFirstSync, isnull(SyncInvoice,0) as SyncInvoice, isnull(SyncTimesheet,0) as SyncTimesheet from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEquipmentTemplate(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[5];

            para[0] = new SqlParameter();
            para[0].ParameterName = "fdesc";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Lang;

            para[1] = new SqlParameter();
            para[1].ParameterName = "remarks";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Remarks;

            para[2] = new SqlParameter();
            para[2].ParameterName = "items";
            para[2].SqlDbType = SqlDbType.Structured;
            para[2].Value = objPropUser.DtItems;

            para[3] = new SqlParameter();
            para[3].ParameterName = "equipt";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropUser.REPtemplateID;

            para[4] = new SqlParameter();
            para[4].ParameterName = "mode";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropUser.Mode;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddEquipTemplate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomTemplate(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[7];

            para[0] = new SqlParameter();
            para[0].ParameterName = "fdesc";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Lang;

            para[1] = new SqlParameter();
            para[1].ParameterName = "remarks";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Remarks;

            para[2] = new SqlParameter();
            para[2].ParameterName = "items";
            para[2].SqlDbType = SqlDbType.Structured;
            para[2].Value = objPropUser.DtItems;

            para[3] = new SqlParameter();
            para[3].ParameterName = "equipt";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropUser.REPtemplateID;

            para[4] = new SqlParameter();
            para[4].ParameterName = "mode";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropUser.Mode;

            para[5] = new SqlParameter();
            para[5].ParameterName = "ItemsDeleted";
            para[5].SqlDbType = SqlDbType.Structured;
            para[5].Value = objPropUser.DtItemsDeleted;

            para[6] = new SqlParameter();
            para[6].ParameterName = "@CustomValues";
            para[6].SqlDbType = SqlDbType.Structured;
            para[6].Value = objPropUser.dtCustomValues;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddCustomTemplate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomTemplateForLeadEquip(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[7];

            para[0] = new SqlParameter();
            para[0].ParameterName = "fdesc";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Lang;

            para[1] = new SqlParameter();
            para[1].ParameterName = "remarks";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Remarks;

            para[2] = new SqlParameter();
            para[2].ParameterName = "items";
            para[2].SqlDbType = SqlDbType.Structured;
            para[2].Value = objPropUser.DtItems;

            para[3] = new SqlParameter();
            para[3].ParameterName = "equipt";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropUser.REPtemplateID;

            para[4] = new SqlParameter();
            para[4].ParameterName = "mode";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropUser.Mode;

            para[5] = new SqlParameter();
            para[5].ParameterName = "ItemsDeleted";
            para[5].SqlDbType = SqlDbType.Structured;
            para[5].Value = objPropUser.DtItemsDeleted;

            para[6] = new SqlParameter();
            para[6].ParameterName = "@CustomValues";
            para[6].SqlDbType = SqlDbType.Structured;
            para[6].Value = objPropUser.dtCustomValues;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddCustomTemplateForLeadEquip", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesPerson(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            try
            {
                //string str = " Select fuser,u.id From tbluser u inner join emp e on u.fuser = e.callsign where e.sales = 1 ";
                string str = "  Select e.Last+', '+e.fFirst as fuser,u.id From tbluser u inner join emp e on u.fuser = e.callsign where e.sales = 1 and e.Status=0 ";

                if (IsSalesAsigned > 0)
                {
                    str += " and u.id=" + IsSalesAsigned;
                }
                str += " order by e.Last ";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTaskUsers(User objPropUser)
        {
            try
            {
                // Get all users
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " Select e.Last+', '+e.fFirst as fuser,u.fuser as username From tbluser u inner join emp e on u.fuser = e.callsign where e.Status=0 order by e.Last");
                // Get all sale users
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " Select e.Last+', '+e.fFirst as fuser,u.fuser as username From tbluser u inner join emp e on u.fuser = e.callsign where e.sales = 1 and e.Status=0 order by e.Last");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void UpdateCustomerUser(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[23];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            para[12] = new SqlParameter();
            para[12].ParameterName = "CustomerId";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.CustomerID;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "GroupData";
            para[19].SqlDbType = SqlDbType.Structured;
            para[19].Value = objPropUser.dtGroupdata;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Equipment";
            para[20].SqlDbType = SqlDbType.Int;
            para[20].Value = objPropUser.EquipID;

            para[21] = new SqlParameter();
            para[21].ParameterName = "@grpbywo";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = objPropUser.grpbyWO;

            para[22] = new SqlParameter();
            para[22].ParameterName = "@openticket";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropUser.openticket;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomerUser", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTimesheetEmp(User objPropUser, int Etimesheet)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetTimesheetEmp", objPropUser.Startdt, objPropUser.Enddt, objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.UserID, objPropUser.WorkId, Etimesheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //api
        public DataSet getTimesheetEmp(getTimesheetParam objPropUser, int Etimesheet, string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetTimesheetEmp", objPropUser.Startdt, objPropUser.Enddt, objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.UserID, objPropUser.WorkId, Etimesheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getSavedTimesheetEmp(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetSavedTimesheet", objPropUser.Startdt, objPropUser.Enddt, objPropUser.Supervisor, objPropUser.DepartmentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet getSavedTimesheetEmp(getTimesheetParam objPropUser, string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetSavedTimesheet", objPropUser.Startdt, objPropUser.Enddt, objPropUser.Supervisor, objPropUser.DepartmentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSavedTimesheet(getTimesheetParam objPropUser, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "Select isnull(processed, 0) as processed from tbltimesheet where startdate = '" + objPropUser.Startdt + "' and enddate= '" + objPropUser.Enddt + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSavedTimesheet(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select isnull(processed, 0) as processed from tbltimesheet where startdate = '" + objPropUser.Startdt + "' and enddate= '" + objPropUser.Enddt + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //api
        //public DataSet getSavedTimesheet(getSavedTimesheetParam objPropUser, string ConnectionString)
        //{
        //    try
        //    {
        //        return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "Select isnull(processed, 0) as processed from tbltimesheet where startdate = '" + objPropUser.Startdt + "' and enddate= '" + objPropUser.Enddt + "'");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetTimesheetTicketsByEmp(User objPropUser, int Etimesheet)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetTimesheetTicketsByEmp", objPropUser.Startdt, objPropUser.Enddt, objPropUser.EmpId, objPropUser.saved, objPropUser.unsaved, Etimesheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetTimesheetTicketsByEmp(getTimesheetParam objPropUser, int Etimesheet, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetTimesheetTicketsByEmp", objPropUser.Startdt, objPropUser.Enddt, objPropUser.EmpId, objPropUser.saved, objPropUser.unsaved, Etimesheet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ProcessTimesheet(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update tbltimesheet set processed = 1 where startdate = '" + objPropUser.Startdt + "' and enddate= '" + objPropUser.Enddt + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTimesheet(User objPropUser)
        {
            try
            {
                SqlParameter[] paraEmpData = new SqlParameter[5];

                paraEmpData[0] = new SqlParameter();
                paraEmpData[0].ParameterName = "@StartDate";
                paraEmpData[0].SqlDbType = SqlDbType.DateTime;
                paraEmpData[0].Value = objPropUser.Startdt;

                paraEmpData[1] = new SqlParameter();
                paraEmpData[1].ParameterName = "@EndDate";
                paraEmpData[1].SqlDbType = SqlDbType.DateTime;
                paraEmpData[1].Value = objPropUser.Enddt;

                paraEmpData[2] = new SqlParameter();
                paraEmpData[2].ParameterName = "Processed";
                paraEmpData[2].SqlDbType = SqlDbType.Int;
                paraEmpData[2].Value = objPropUser.IsSuper;

                paraEmpData[3] = new SqlParameter();
                paraEmpData[3].ParameterName = "EmpData";
                paraEmpData[3].SqlDbType = SqlDbType.Structured;
                paraEmpData[3].Value = objPropUser.EmpData;

                paraEmpData[4] = new SqlParameter();
                paraEmpData[4].ParameterName = "TicketData";
                paraEmpData[4].SqlDbType = SqlDbType.Structured;
                paraEmpData[4].Value = objPropUser.dtTicketData;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddTimesheetEmp", paraEmpData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //api
        public void AddTimesheet(AddTimesheetParam objPropUser, string ConnectionString)
        {
            try
            {
                SqlParameter[] paraEmpData = new SqlParameter[5];

                paraEmpData[0] = new SqlParameter();
                paraEmpData[0].ParameterName = "@StartDate";
                paraEmpData[0].SqlDbType = SqlDbType.DateTime;
                paraEmpData[0].Value = objPropUser.Startdt;

                paraEmpData[1] = new SqlParameter();
                paraEmpData[1].ParameterName = "@EndDate";
                paraEmpData[1].SqlDbType = SqlDbType.DateTime;
                paraEmpData[1].Value = objPropUser.Enddt;

                paraEmpData[2] = new SqlParameter();
                paraEmpData[2].ParameterName = "Processed";
                paraEmpData[2].SqlDbType = SqlDbType.Int;
                paraEmpData[2].Value = objPropUser.IsSuper;

                paraEmpData[3] = new SqlParameter();
                paraEmpData[3].ParameterName = "EmpData";
                paraEmpData[3].SqlDbType = SqlDbType.Structured;
                paraEmpData[3].Value = objPropUser.EmpData;

                paraEmpData[4] = new SqlParameter();
                paraEmpData[4].ParameterName = "TicketData";
                paraEmpData[4].SqlDbType = SqlDbType.Structured;
                paraEmpData[4].Value = objPropUser.dtTicketData;

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddTimesheetEmp", paraEmpData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getScreens(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT p.id,p.pagename,p.url,isnull(pp.access,0)as access ,ISNULL( pp.edit,0) as edit, ISNULL( pp.[VIEW],0) as [VIEW], ISNULL( pp.[add],0) as [add], isnull (pp.[DELETE],0) as [DELETE] FROM tblPages p INNER JOIN tblpagepermissions pp ON p.id=pp.page AND pp.[USER]=" + objPropUser.UserID + " where p.URL NOT IN ('ticketlistview.aspx','addticket.aspx','equipments.aspx','addequipment.aspx','project.aspx','addproject.aspx','vendors.aspx','addvendor.aspx')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getScreensByUser(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT Isnull(pp.access, 0)    AS access, \n");
            varname1.Append("       Isnull(pp.edit, 0)      AS edit, \n");
            varname1.Append("       Isnull(pp.[VIEW], 0)    AS [VIEW], \n");
            varname1.Append("       Isnull(pp.[add], 0)     AS [add], \n");
            varname1.Append("       Isnull (pp.[DELETE], 0) AS [DELETE] \n");
            varname1.Append("FROM   tblpagepermissions pp \n");
            varname1.Append("WHERE  pp.[USER] = (SELECT id \n");
            varname1.Append("                    FROM   tbluser \n");
            varname1.Append("                    WHERE  fuser = '" + objPropUser.Username + "') \n");
            varname1.Append("       AND Page = (SELECT id \n");
            varname1.Append("                   FROM   tblpages \n");
            varname1.Append("                   WHERE  url = '" + objPropUser.PageName + "') ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getScreensByUser(GetScreensByUserParam _GetScreensByUser, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT Isnull(pp.access, 0)    AS access, \n");
            varname1.Append("       Isnull(pp.edit, 0)      AS edit, \n");
            varname1.Append("       Isnull(pp.[VIEW], 0)    AS [VIEW], \n");
            varname1.Append("       Isnull(pp.[add], 0)     AS [add], \n");
            varname1.Append("       Isnull (pp.[DELETE], 0) AS [DELETE] \n");
            varname1.Append("FROM   tblpagepermissions pp \n");
            varname1.Append("WHERE  pp.[USER] = (SELECT id \n");
            varname1.Append("                    FROM   tbluser \n");
            varname1.Append("                    WHERE  fuser = '" + _GetScreensByUser.Username + "') \n");
            varname1.Append("       AND Page = (SELECT id \n");
            varname1.Append("                   FROM   tblpages \n");
            varname1.Append("                   WHERE  url = '" + _GetScreensByUser.PageName + "') ");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddSageLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Website";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.Website;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "SageOwner";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.SageCustID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "SageKeyID";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objPropUser.SageLocID;

            //para[21] = new SqlParameter();
            //para[21].ParameterName = "returnval";
            //para[21].SqlDbType = SqlDbType.Int;
            //para[21].Direction = ParameterDirection.ReturnValue;

            para[21] = new SqlParameter();
            para[21].ParameterName = "LastUpdateDate";
            para[21].SqlDbType = SqlDbType.DateTime;
            para[21].Value = objPropUser.LastUpdateDate;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageCustomer";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.Custom2;

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddSageLocation", para);

                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddSageLocation", para);
                //int locid = 0;
                //if (para[21].Value != DBNull.Value)
                //{
                //    locid = Convert.ToInt32(para[21].Value);
                //}
                //return locid;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getGetSageExportTickets(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetSageExportTickets", objPropUser.Startdt, objPropUser.Enddt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet getGetSageExportTickets(getTimesheetParam objPropUser, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetSageExportTickets", objPropUser.Startdt, objPropUser.Enddt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePeriodClosedDate(User objPropUser)
        {
            try
            {
                string query = "UPDATE tblUser SET fStart=@fStart, fEnd=@fEnd";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fStart", objPropUser.FStart));
                parameters.Add(new SqlParameter("@fEnd", objPropUser.FEnd));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUserAddress(User objPropUser)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT \n");
                varname.Append(" u.ID as userid,  \n");
                varname.Append(" r.ID as rolid,  \n");
                varname.Append(" r.City,");
                varname.Append(" r.State,");
                varname.Append(" r.Zip,");
                varname.Append(" r.Phone,");
                varname.Append(" r.Address,");
                varname.Append(" r.name as fFirst,");
                varname.Append(" r.name as Middle,");
                varname.Append(" r.name as Last,");
                varname.Append(" u.Status,");
                varname.Append(" r.Remarks");
                varname.Append(" FROM  Owner u 	");
                varname.Append(" LEFT OUTER JOIN Rol r ON u.Rol=r.ID ");
                varname.Append(" WHERE u.Status=0 AND u.ID=" + objPropUser.UserID);
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillCodeSearch(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetBillingCodeSearch", objPropUser.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet updateServiceTypeByjobType(string Sessionconfig, int jobID, string ServiceType, int ExpenseGLValue, int InterestGLValue, int BillingValue, int LaborWageValue)
        {
            try
            {
                return SqlHelper.ExecuteDataset(Sessionconfig, "spupdateServiceTypeByjobType", jobID, ServiceType, ExpenseGLValue, InterestGLValue, BillingValue, LaborWageValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceTypeByType(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select l.type,l.fdesc,l.remarks,(select count(1) from elev where cat=l.type) as Count,l.InvID, isnull(i.Sacct,'') as Sacct, isnull((select c.fdesc from chart c where c.ID=i.sacct),'') as GLAcct from ltype l left join Inv i on l.InvID=i.ID where l.type = '" + objPropUser.Type + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetServiceTypeByType(GetServiceTypeByTypeParam _GetServiceTypeByType, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select l.type,l.fdesc,l.remarks,(select count(1) from elev where cat=l.type) as Count,l.InvID, isnull(i.Sacct,'') as Sacct, isnull((select c.fdesc from chart c where c.ID=i.sacct),'') as GLAcct from ltype l left join Inv i on l.InvID=i.ID where l.type = '" + _GetServiceTypeByType.Type + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWage(User objPropUser)                                                // get Active/Inactive Wage
        {
            string strQuery = "select id,fdesc,remarks,ID as value, fDesc as Name,ID as LabItem, fDesc as LabDesc from PRWage order by fdesc"; // where Field = 1

            if (objPropUser.SearchValue != "")
            {
                strQuery = "select id,fdesc,remarks,ID as value, fDesc as Name,ID as LabItem, fDesc as LabDesc from PRWage where fdesc like'%" + objPropUser.SearchValue + "%' ";
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getSTax(User objPropUser)
        {
            string strQuery = "select s.*,s.Name+ Space(5)+ CONVERT(VARCHAR,s.Rate)+'%' AS StaxName,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType = 0 order by name";
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getSTax(getSTaxParam _getSTaxParam, string ConnectionString)
        {
            string strQuery = "select s.*,s.Name+ Space(5)+ CONVERT(VARCHAR,s.Rate)+'%' AS StaxName,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType = 0 order by name";
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getSalesTax2(User objPropUser)
        {
            string strQuery = "select s.*,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType <> 1 order by name";
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getSalesTax2(getSalesTax2Param _getSalesTax2, string ConnectionString)
        {
            string strQuery = "select s.*,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType <> 1 order by name";
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUseTax(User objPropUser)
        {
            string strQuery = "select s.*,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType = 1 order by name";
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUseTax(getUseTaxParam _getUseTaxParam, string ConnectionString)
        {
            string strQuery = "select s.*,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType = 1 order by name";
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getZone(User objPropUser)
        {
            string strQuery = "Select * From Zone order by name";
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getZone(GetZoneParam _GetZone, string ConnectionString)
        {
            string strQuery = "Select * From Zone order by name";
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetWageByID(Wage _objWage)
        {
            try
            {
                return _objWage.Ds = SqlHelper.ExecuteDataset(_objWage.ConnConfig, "spGetWageByID", _objWage.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteWageByID(Wage _objWage)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, CommandType.Text, "DELETE FROM PRWage WHERE ID=" + _objWage.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllWage(Wage _objWage) // display all active wage details
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objWage.ConnConfig, CommandType.Text, "SELECT *, ID as Wage, ID as value, fDesc, fDesc as label, ID as LabItem, fDesc as LabDesc FROM PRWage WHERE ISNULL(Status,1) <> 1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet IsWageRateIsUsed(Wage _objWage, Int32 userID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objWage.ConnConfig, CommandType.Text, @"SELECT top 1 fWork,WageC,* FROM TicketD where  fWork=(select ID from tblWork where fdesc=(select fuser from tblUser where id =" + userID + ") ) and WageC=" + _objWage.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateDocInfo(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@Docs";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.dtDocs;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@UpdatedBy";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Username;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateDocInfo", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfoParam, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@Docs";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = _UpdateDocInfoParam.dtDocs;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@UpdatedBy";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _UpdateDocInfoParam.Username;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateDocInfo", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserSearch(User _objUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objUser.ConnConfig, "spGetUserSearch", _objUser.SearchValue, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserSearch(User _objUser, string userRoleName)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objUser.ConnConfig, "spGetUserSearch", _objUser.SearchValue, userRoleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserfForEstimate(User _objUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objUser.ConnConfig, "spGetUserForEstimate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevByLoc(User objPropUser)
        {
            string str = "select distinct e.state, e.cat,e.category,e.manuf,e.price,e.last,e.since,e.Install, e.id,e.unit,e.type,e.fdesc,e.status,r.name,l.id as locid,l.tag ,(l.address+', '+l.city+', '+l.state+', '+l.zip) as address, l.Loc,e.ID as unitid FROM elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id WHERE e.id IS NOT NULL ";
            str += " and e.loc=" + objPropUser.LocID + "";
            str += " order by e.unit";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllTc(User objPropUser)
        {
            string str = " SELECT t.*, p.PageName from [t&c] as t INNER JOIN [tblPages] as p ON t.tblPageID = p.ID ORDER BY ID ";
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSearchPages(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetPageSearch", objPropUser.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddTerms(User _objUser)
        {
            try
            {
                string query = "INSERT INTO [T&C] (tblPageID, TermsConditions) VALUES (@tblPageID, @TermsCondition) ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@tblPageID", _objUser.PageID));
                parameters.Add(new SqlParameter("@TermsCondition", _objUser.TermsConditions));
                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTerms(User _objUser)
        {
            try
            {
                string query = "UPDATE [T&C] SET tblPageID=@tblPageID, TermsConditions=@TermsCondition WHERE ID='" + _objUser.TermsID + "' ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@tblPageID", _objUser.PageID));
                parameters.Add(new SqlParameter("@TermsCondition", _objUser.TermsConditions));
                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistPage(User _objUser)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objUser.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM [T&C] WHERE tblPageID = '" + _objUser.PageID + "')THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistPageForUpdate(User _objUser)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objUser.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM [T&C] WHERE tblPageID = '" + _objUser.PageID + "' AND ID != '" + _objUser.TermsID + "')THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTermsCondition(User _objUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objUser.ConnConfig, CommandType.Text, " DELETE FROM [T&C] WHERE ID='" + _objUser.TermsID + "'  ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String CreateWarehouse(User objPropUser)
        {
            try
            {

                String ID = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spCreateWarehouse", objPropUser.WarehouseID, objPropUser.WarehouseName, objPropUser.Type, objPropUser.LocID, objPropUser.Remarks, objPropUser.IsMultiValue, objPropUser.EN, objPropUser.Status));
                return ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateInventoryWarehouse(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spUpdateWarehouse", objPropUser.WarehouseID, objPropUser.WarehouseName, objPropUser.Type, objPropUser.LocID, objPropUser.Remarks, objPropUser.IsMultiValue, objPropUser.EN, objPropUser.Status);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInventoryWarehouse(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetInventoryWarehouse", objPropUser.EN, objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInventoryActiveWarehouse(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetInventoryActiveWarehouse", objPropUser.EN, objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet GetInventoryActiveWarehouse(GetInventoryActiveWarehouseParam _GetInventoryActiveWarehouseParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetInventoryActiveWarehouse", _GetInventoryActiveWarehouseParam.EN, _GetInventoryActiveWarehouseParam.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public DataSet GetInventoryWarehouseByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetInventoryWarehouseByID", objPropUser.WarehouseID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteInventoryWareHouse(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@WareHouseID";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.WarehouseID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@InvID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.AccountID;
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteInventoryWareHouse", objPropUser.WarehouseID);
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteInventoryWareHouse", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public void DeleteInventoryWareHouse(DeleteInventoryWareHouseParam _DeleteInventoryWareHouseParam, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@WareHouseID";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _DeleteInventoryWareHouseParam.WarehouseID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@InvID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _DeleteInventoryWareHouseParam.AccountID;
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteInventoryWareHouse", objPropUser.WarehouseID);
                SqlHelper.ExecuteNonQuery(ConnectionString, "spDeleteInventoryWareHouse", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreateInventoryCategory(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spCreateInventoryCategory", objPropUser.CategoryName, objPropUser.CategoryCount, objPropUser.CategoryRemarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateInventoryCategory(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spUpdateInventoryCategory", objPropUser.CategoryTypeID, objPropUser.CategoryName, objPropUser.CategoryCount, objPropUser.CategoryRemarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInventoryCategory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetInventoryCategory");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteInventoryCategory(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  IType where ID= '" + objPropUser.CategoryTypeID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string CreateWareHouseLocation(User objPropUser)
        {
            String retVal = "";
            try
            {
                retVal = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spCreateWHLoc", objPropUser.WarehouseID, objPropUser.WareHouseLocation, objPropUser.IsEdit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        public string UpdateWareHouseLocation(User objPropUser)
        {
            String retVal = "";
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@WHLocID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropUser.WHLocID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@WareHouseLocation",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropUser.WareHouseLocation
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@WarehouseID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropUser.WarehouseID
                };
                retVal = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spUpdateWHLoc", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        public DataSet GetWareHouseLocation(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetWHLoc", objPropUser.WarehouseID);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void DeleteWareHouseLocation(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  WHLoc where ID= '" + objPropUser.WHLocID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllUseTax(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename, c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID AND s.Utype = 1 order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllUseTaxSearch(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename, c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID AND s.Utype = 1 and (Name like '%" + objPropUser.SearchValue + "%' OR Rate like '%" + objPropUser.SearchValue + "%') order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobBillRatesById(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select isnull(BillRate, 0) as BillRate, isnull(RateOT, 0) as RateOT, isnull(RateNT, 0) as RateNT, isnull(RateDT, 0) as RateDT, isnull(RateMileage, 0) as RateMileage, isnull(RateTravel,0) as RateTravel from Job where ID=" + objPropUser.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllWageForEstimate(Wage _objWage) // display all active wage details
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objWage.ConnConfig, CommandType.Text, "SELECT *, ID as value, fDesc, fDesc as Name FROM PRWage WHERE Status = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetGetEstimateRoleSpecificDetails(Rol id)
        {
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();
                return SqlHelper.ExecuteDataset(constring, "spGetEstimateRoleSpecificDetails", id.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimatePhoneContactSpecificDetails(int id, int estimateId)
        {
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();
                return SqlHelper.ExecuteDataset(constring, "GetEstimatePhoneContactSpecificDetails", id, estimateId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getequipCustomItemsByElevID(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT fDesc,Value,LastUpdated,LastUpdateUser   FROM   ElevTItem    WHERE  Elev = " + objPropUser.EquipID + "    ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getContractType(User objPropUser)
        {
            string str = "select ctype from job where id=(select top 1 job from tblJoinElevJob where elev = " + objPropUser.EquipID + ")";
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, str));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public string getContractType(GetContractTypeParam _GetContractType, string ConnectionString)
        {
            string str = "select ctype from job where id=(select top 1 job from tblJoinElevJob where elev = " + _GetContractType.EquipID + ")";
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, str));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getGCCustomer(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select r.Name,r.City,r.State,r.Zip,r.Phone, r.Fax,r.Contact,r.Address,r.EMail,r.Country,r.remarks,r.Cellular ,r.id as rol, o.type from rol r inner join Owner o on o.Rol=r.ID where o.ID=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getGCCustomer(GetGCCustomerParam _GetGCCustomer, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select r.Name,r.City,r.State,r.Zip,r.Phone, r.Fax,r.Contact,r.Address,r.EMail,r.Country,r.remarks,r.Cellular ,r.id as rol, o.type from rol r inner join Owner o on o.Rol=r.ID where o.ID=" + _GetGCCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getGCAutojquery(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetGCSearch", objPropUser.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getHomeOwnerAutojquery(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "Spgethomeownersearch", objPropUser.SearchValue, objPropUser.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEmpWageItems(User objPropUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];

                para[0] = new SqlParameter()
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropUser.EmpId
                };
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetEmpWageItemsByID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllWageRate(string con, string text)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];

                para[0] = new SqlParameter()
                {
                    ParameterName = "@text",
                    SqlDbType = SqlDbType.NChar
                    ,
                    Value = text
                };
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "spGetAllWageRate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEquipmentTests(User objPropUser)
        {
            try
            {

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spReadTestsByEquipmentId", objPropUser.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllTestByEquipmentID(User objPropUser)
        {
            try
            {

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetAllTestByEquipmentID", objPropUser.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEquipmentTests(GetEquipmentTestsParam _GetEquipmentTests, string ConnectionString)
        {
            try
            {

                return _GetEquipmentTests.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spReadTestsByEquipmentId", _GetEquipmentTests.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddSageContracts(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[16];

            para[0] = new SqlParameter()
            {
                ParameterName = "@remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.Remarks
            };
            para[1] = new SqlParameter()
            {
                ParameterName = "@BStart",
                SqlDbType = SqlDbType.DateTime,
                Value = (objPropUser.BStartdt == DateTime.MinValue ? Convert.ToDateTime("01/01/1900") : objPropUser.BStartdt)
            };
            para[2] = new SqlParameter()
            {
                ParameterName = "@Bcycle",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.BCycle
            };
            para[3] = new SqlParameter()
            {
                ParameterName = "@BAmt",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropUser.AnnualAmount
            };
            para[4] = new SqlParameter()
            {
                ParameterName = "@SStart",
                SqlDbType = SqlDbType.DateTime,
                Value = (objPropUser.SStartdt == DateTime.MinValue ? Convert.ToDateTime("01/01/1900") : objPropUser.SStartdt)
            };
            para[5] = new SqlParameter()
            {
                ParameterName = "@Cycle",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.SCycle
            };
            para[6] = new SqlParameter()
            {
                ParameterName = "@Stime",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropUser.Stime
            };
            para[7] = new SqlParameter()
            {
                ParameterName = "@Route",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.SageRoute
            };
            para[8] = new SqlParameter()
            {
                ParameterName = "@hours",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropUser.HourlyRate
            };
            para[9] = new SqlParameter()
            {
                ParameterName = "@fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.Description
            };
            para[10] = new SqlParameter()
            {
                ParameterName = "@SagelocKey",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.SageLocKey
            };
            para[11] = new SqlParameter()
            {
                ParameterName = "@SageJobKey",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.SagejobKey
            };
            para[12] = new SqlParameter()
            {
                ParameterName = "@SageID",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.SageID
            };
            para[13] = new SqlParameter()
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            para[14] = new SqlParameter()
            {
                ParameterName = "@servicetype",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropUser.ServiceType
            };
            para[15] = new SqlParameter()
            {
                ParameterName = "@LastUpdateDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropUser.LastUpdateDate
            };

            try
            {

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddSageContract", para);
                int Jobid = 0;
                if (para[13].Value != DBNull.Value)
                {
                    Jobid = Convert.ToInt32(para[13].Value);
                }
                return Jobid;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUpdateContractsForSage(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateContractsSage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateDoc(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update Documents set MSVisible = case isnull( MSVisible ,0) when 1 then 0 when 0 then 1 end  where id = " + objPropUser.DocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getRouteByID(User objPropUser)
        {
            try
            {
                string createQuery = @"select 
                (SELECT COUNT(1) FROM Loc WHERE Route=ro.ID) As LocCount,
                (SELECT COUNT(1) FROM Contract c INNER JOIN Loc l ON c.Loc=l.Loc
                WHERE l.Route=ro.ID) AS ContCount,
                ro.Status, ro.name, ro.id, ro.remarks, ro.Color,
                (select top 1 fdesc from tblwork where id = ro.mech) as mechname, 
                ro.mech from route ro where ro.ID = " + objPropUser.Route;

                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, createQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRouteLogs(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objPropUser.Route + "  and Screen='Route' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddRoute(User objPropUser)
        {
            int res = 0;
            try
            {
                SqlParameter[] paraEmpData = new SqlParameter[7];

                paraEmpData[0] = new SqlParameter();
                paraEmpData[0].ParameterName = "@name";
                paraEmpData[0].SqlDbType = SqlDbType.VarChar;
                paraEmpData[0].Value = objPropUser.ContactName;

                paraEmpData[1] = new SqlParameter();
                paraEmpData[1].ParameterName = "@mech";
                paraEmpData[1].SqlDbType = SqlDbType.Int;
                paraEmpData[1].Value = objPropUser.WorkId;

                paraEmpData[2] = new SqlParameter();
                paraEmpData[2].ParameterName = "@remarks";
                paraEmpData[2].SqlDbType = SqlDbType.VarChar;
                paraEmpData[2].Value = objPropUser.Remarks;

                paraEmpData[3] = new SqlParameter();
                paraEmpData[3].ParameterName = "@id";
                paraEmpData[3].SqlDbType = SqlDbType.Int;
                paraEmpData[3].Value = objPropUser.Route;

                paraEmpData[4] = new SqlParameter();
                paraEmpData[4].ParameterName = "@Color";
                paraEmpData[4].SqlDbType = SqlDbType.VarChar;
                paraEmpData[4].Value = objPropUser.Color;

                paraEmpData[5] = new SqlParameter();
                paraEmpData[5].ParameterName = "@UpdatedBy";
                paraEmpData[5].SqlDbType = SqlDbType.VarChar;
                paraEmpData[5].Value = objPropUser.MOMUSer;

                paraEmpData[6] = new SqlParameter();
                paraEmpData[6].ParameterName = "@Status";
                paraEmpData[6].SqlDbType = SqlDbType.Bit;
                paraEmpData[6].Value = Convert.ToBoolean(objPropUser.Status);

                res = Convert.ToInt16(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddRoute", paraEmpData));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public DataSet getDefaultRouteTerr(User objPropUser)
        {
            string str = "select top 1 id from route where mech = (select top 1 ID from tblWork where fDesc = (select top 1 fUser from tbluser where DefaultWorker = 1))";
            str += "select top 1 id from Terr where SMan = (select top 1 ID from tbluser where DefaultWorker = 1)";

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getDefaultRouteTerr(GetDefaultRouteTerrParam _GetDefaultRouteTerr, string ConnectionString)
        {
            string str = "select top 1 id from route where mech = (select top 1 ID from tblWork where fDesc = (select top 1 fUser from tbluser where DefaultWorker = 1))";
            str += "select top 1 id from Terr where SMan = (select top 1 ID from tbluser where DefaultWorker = 1)";

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSTaxByName(User objPropUser)
        {
            string strQuery = "select s.*,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType = 0 and s.Name= '" + objPropUser.Stax + "'";
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddPreferences(User objPropUser)
        {

            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@UserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.UserID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@PreferenceID",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.PreferenceID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@PageID",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.PageID
            };

            para[3] = new SqlParameter
            {
                ParameterName = "@Values",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.PreferenceValues
            };

            try
            {
                SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddPreferences", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPreferences(User objPropUser)
        {

            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@UserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.UserID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@PreferenceID",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.PreferenceID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@PageID",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.PageID
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetPreferences", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEquipmentTypeCount(User objPropUser)
        {
            //string strQuery = "Select Type as TypeName, count(Type) Total from [dbo].[Elev] where status = 0 group by Type";
            string strQuery = "SELECT t1.EDesc AS TypeName, COUNT(t2.ID) AS Total FROM ElevatorSpec t1 LEFT JOIN Elev t2 ON t2.Type = t1.EDesc WHERE  t1.ECat = 1 AND t2.Status = 0 GROUP BY t1.EDesc UNION SELECT 'Unassigned' AS Building, COUNT(*) FROM [Elev] WHERE (Type IS NULL OR Type = 'None' OR Type = '') AND Status = 0";

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public DataSet GetEquipmentTypeCountSP(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetEquipmentTypeCount");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEquipmentBuildingCount(User objPropUser)
        {
            //string strQuery = "select CASE WHEN Building IS NULL THEN max([ElevatorSpec].EDesc) ELSE Building END as Building, count(*) as Total from [Elev] left join [ElevatorSpec] on [Elev].building = [ElevatorSpec].Edesc  group by [Building]";
            string strQuery = "SELECT t1.EDesc AS Building, COUNT(t2.ID) AS Total FROM [ElevatorSpec] t1 LEFT JOIN [Elev] t2 ON t2.Building = t1.Edesc WHERE t1.ECat = 2 AND t2.Status = 0 GROUP BY t1.EDesc UNION SELECT 'Unassigned' AS Building, COUNT(*) FROM [Elev] WHERE (Building IS NULL OR Building = 'None' OR Building = '') AND Status = 0";

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public DataSet GetEquipmentBuildingCountSP(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetEquipmentBuildingCount");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet GetLocationsStatus(User objPropUser)
        {
            string strQuery = "Select [dbo].[Loc].loc, [dbo].[Contract].status,[dbo].[Contract].BStart from  [dbo].[Loc] left join contract on [dbo].[Loc].loc = [dbo].[Contract].loc where [dbo].[Loc].Status=0 and   [dbo].[Contract].BStart  >= Dateadd(Month, Datediff(Month, 0, DATEADD(m, -6, current_timestamp)), 0)";

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetLocationsStatusList(User objPropUser)
        {
            string strQuery = "Select [dbo].[Loc].loc, [dbo].[Contract].status,[dbo].[Contract].BStart from  [dbo].[Loc] left join contract on [dbo].[Loc].loc = [dbo].[Contract].loc where [dbo].[Loc].Status=0 and   [dbo].[Contract].BStart  >= Dateadd(Month, Datediff(Month, 0, DATEADD(m, -6, current_timestamp)), 0)";

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] GetTicketStatus(User objPropUser)
        {
            try
            {
                var counts = new int[2];

                var currentDate = DateTime.Now;

                string strQuery = $"SELECT count(*) FROM TicketD WHERE MONTH(CDate) = {currentDate.Month} AND YEAR(CDate) = {currentDate.Year}";

                Int32 ticketsClosed = (Int32)SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, strQuery);
                counts[0] = ticketsClosed;

                strQuery = "select count(*) from Ticketo WHERE MONTH(CDate) = {currentDate.Month} AND YEAR(CDate) = {currentDate.Year}";

                Int32 ticketsOpen = (Int32)SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, strQuery);

                counts[1] = ticketsOpen;

                return counts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int[] GetSixtyPlusAR(User objPropUser)
        {
            try
            {
                var counts = new int[2];
                var currentDate = DateTime.Now;
                string strQuery = $"SELECT count(*) FROM TicketD WHERE MONTH(CDate) = {currentDate.Month} AND YEAR(CDate) = {currentDate.Year}";
                Int32 ticketsClosed = (Int32)SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, strQuery);
                counts[0] = ticketsClosed;

                strQuery = "select count(*) from Ticketo WHERE MONTH(CDate) = {currentDate.Month} AND YEAR(CDate) = {currentDate.Year}";
                Int32 ticketsOpen = (Int32)SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, strQuery);
                counts[1] = ticketsOpen;

                return counts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Get12MonthActualvsBudgetData(User objPropUser, string selectedBudget)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@StartDate",
                SqlDbType = SqlDbType.Date,
                Value = new DateTime(DateTime.Now.Year, 1, 1)
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@EndDate",
                SqlDbType = SqlDbType.Date,
                Value = DateTime.Now
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@BudgetName",
                SqlDbType = SqlDbType.VarChar,
                Value = selectedBudget
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "Get12MonthActualvsBudgetData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet Get12MonthActualvsBudgetGraphData(User objPropUser, int? budgetID, int fiscalYear)
        {
            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@StartDate",
                SqlDbType = SqlDbType.Date,
                Value = objPropUser.FStart
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@EndDate",
                SqlDbType = SqlDbType.Date,
                Value = objPropUser.FEnd
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@BudgetID",
                SqlDbType = SqlDbType.Int,
                Value = budgetID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@FiscalYear",
                SqlDbType = SqlDbType.Int,
                Value = fiscalYear
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "Get12MonthActualvsBudgetGraphData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetLast12MonthActualvsBudgetData(User objPropUser, string fiscalYear, int endMonth)
        {
            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@StartDate",
                SqlDbType = SqlDbType.Date,
                Value = objPropUser.FStart
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@EndDate",
                SqlDbType = SqlDbType.Date,
                Value = objPropUser.FEnd
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@FiscalYear",
                SqlDbType = SqlDbType.Text,
                Value = fiscalYear
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@MonthEndYear",
                SqlDbType = SqlDbType.Int,
                Value = endMonth
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "GetLast12MonthActualvsBudgetData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable[] GetEstimatedAndTotalHoursCompleted(User objPropUser)
        {
            try
            {
                DataTable[] result = new DataTable[3];

                //--------1 Show first bar Total Hours = Add the TicketO.EST and TicketD.Est

                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1);

                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	SUM(ISNULL(t.Est,0)) AS Total,  \n");
                sb.Append("	r.Name AS Route  \n");
                sb.Append("FROM TicketO t \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = t.LID \n");
                sb.Append("	LEFT JOIN route r ON l.Route = r.ID   \n");
                sb.Append("	LEFT JOIN Category c ON t.Cat = c.Type  \n");
                sb.Append("WHERE EDate >= '" + startDate + "' AND EDate < '" + endDate + "'  \n");
                sb.Append(" AND c.ISDefault = 1  \n");
                sb.Append("GROUP BY r.Name  \n");
                sb.Append("UNION ALL   \n");
                sb.Append("SELECT   \n");
                sb.Append("	SUM(ISNULL(t.Est,0)) AS Total, \n");
                sb.Append("	r.Name AS Route  \n");
                sb.Append("FROM TicketD t \n");
                sb.Append("	LEFT JOIN Loc l ON t.Loc = l.Loc  \n");
                sb.Append("	LEFT JOIN Route r ON l.Route = r.ID  \n");
                sb.Append("	LEFT JOIN Category c ON t.Cat = c.Type  \n");
                sb.Append("WHERE EDate >= '" + startDate + "' AND EDate < '" + endDate + "'  \n");
                sb.Append("	AND c.ISDefault = 1  \n");
                sb.Append("GROUP BY r.Name \n");

                result[0] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, sb.ToString()).Tables[0];

                //------ - 2 Show second bar Completed = TicketDPDA.Total + TicketD.Total
                StringBuilder sb1 = new StringBuilder();

                sb1.Append("SELECT  \n");
                sb1.Append("    SUM(ISNULL(t.Total,0)) AS Total, \n");
                sb1.Append("	r.Name AS Route   \n");
                sb1.Append("FROM TicketDPDA t \n");
                sb1.Append("	LEFT JOIN loc l ON t.Loc = l.Loc  \n");
                sb1.Append("	LEFT JOIN Route r ON r.id = l.Route  \n");
                sb1.Append("	LEFT JOIN Category c ON t.Cat = c.Type  \n");
                sb1.Append("WHERE EDate >= '" + startDate + "' AND EDate < '" + endDate + "'  \n");
                sb1.Append("	AND c.ISDefault = 1  \n");
                sb1.Append("GROUP BY r.Name  \n");
                sb1.Append("UNION ALL \n");
                sb1.Append("SELECT  \n");
                sb1.Append("	SUM(ISNULL(t.Total,0)) as Total, \n");
                sb1.Append("	r.Name as Route  \n");
                sb1.Append("FROM TicketD t \n");
                sb1.Append("	LEFT JOIN Loc l ON t.Loc = l.Loc  \n");
                sb1.Append("	LEFT JOIN Route r ON r.ID = l.Route  \n");
                sb1.Append("	LEFT JOIN Category c ON t.Cat = c.Type  \n");
                sb1.Append("where EDate >= '" + startDate + "' AND EDate < '" + endDate + "'  \n");
                sb1.Append("	AND c.ISDefault = 1  \n");
                sb1.Append("GROUP BY r.Name \n");

                result[1] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, sb1.ToString()).Tables[0];

                // ------3 Total Hours for open tickets
                StringBuilder sb2 = new StringBuilder();

                sb2.Append("SELECT ");
                sb2.Append("	SUM(ISNULL(t.Est,0)) AS Total, \n");
                sb2.Append("	r.Name AS Route \n");
                sb2.Append("FROM TicketO t \n");
                sb2.Append("	INNER JOIN Loc l ON l.Loc = t.LID \n");
                sb2.Append("	LEFT JOIN route r on L.Route = r.ID \n");
                sb2.Append("	LEFT JOIN Category c on t.Cat = c.Type \n");
                sb2.Append("WHERE Assigned <> 4 AND EDate >= '" + startDate + "' AND EDate < '" + endDate + "' \n");
                sb2.Append("	AND c.ISDefault = 1 \n");
                sb2.Append("GROUP BY r.Name \n");

                result[2] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, sb2.ToString()).Tables[0];

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEstimatedAndTotalHoursCompletedDS(User objPropUser)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1);
            SqlParameter[] para = new SqlParameter[2];
            try
            {
                para[0] = new SqlParameter();
                para[0].ParameterName = "@StartDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = startDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@EndDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = endDate;

                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spRecurringHoursRemainingChart", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable[] GetTicketRecurringOpenAndCompleted(User objPropUser)
        {
            try
            {
                DataTable[] result = new DataTable[2];

                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1);

                if (objPropUser.EN == 1)
                {
                    StringBuilder openTicketQuery = new StringBuilder();
                    openTicketQuery.Append("SELECT                                              \n");
                    openTicketQuery.Append("   wk.fDesc AS DWork,                               \n");
                    openTicketQuery.Append("   SUM(ISNULL(Est,0)) AS Total                                \n");
                    openTicketQuery.Append("FROM TicketO tk                                     \n");
                    openTicketQuery.Append("   INNER JOIN tblWork wk ON tk.fWork = wk.ID        \n");
                    openTicketQuery.Append("   INNER JOIN Category ct ON tk.Cat = ct.Type       \n");
                    openTicketQuery.Append("   INNER JOIN Loc l on l.Loc = tk.LID               \n");
                    openTicketQuery.Append("   INNER JOIN Owner ow on l.Owner = ow.ID           \n");
                    openTicketQuery.Append("   INNER JOIN Rol rl  ON rl.ID = ow.Rol             \n");
                    openTicketQuery.Append("   INNER JOIN tblUserCo uc on uc.CompanyID = rl.EN  \n");
                    openTicketQuery.Append("WHERE Assigned<> 4                                  \n");
                    openTicketQuery.Append("   AND Edate >= '" + startDate + "'                \n");
                    openTicketQuery.Append("   AND Edate < '" + endDate + "'                    \n");
                    openTicketQuery.Append("   AND ct.ISDefault = 1                             \n");
                    openTicketQuery.Append("   AND uc.IsSel = 1                                 \n");
                    openTicketQuery.Append("   AND uc.UserID = " + objPropUser.UserID + "       \n");
                    openTicketQuery.Append("   GROUP BY wk.fDesc                                \n");

                    result[0] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, openTicketQuery.ToString()).Tables[0];

                    StringBuilder completeTicketQuery = new StringBuilder();
                    completeTicketQuery.Append("SELECT                                              \n");
                    completeTicketQuery.Append("   wk.fDesc AS DWork,                               \n");
                    completeTicketQuery.Append("   SUM(ISNULL(Total,0)) AS Total                              \n");
                    completeTicketQuery.Append("FROM TicketDPDA tk                                  \n");
                    completeTicketQuery.Append("   INNER JOIN tblWork wk ON tk.fWork = wk.ID        \n");
                    completeTicketQuery.Append("   INNER JOIN Category ct ON tk.Cat = ct.Type       \n");
                    completeTicketQuery.Append("   INNER JOIN Loc l on l.Loc = tk.Loc               \n");
                    completeTicketQuery.Append("   INNER JOIN Owner ow on l.Owner = ow.ID           \n");
                    completeTicketQuery.Append("   INNER JOIN Rol rl  ON rl.ID = ow.Rol             \n");
                    completeTicketQuery.Append("   INNER JOIN tblUserCo uc on uc.CompanyID = rl.EN  \n");
                    completeTicketQuery.Append("WHERE Edate >= '" + startDate + "'                  \n");
                    completeTicketQuery.Append("   AND Edate < '" + endDate + "'                    \n");
                    completeTicketQuery.Append("   AND ct.ISDefault = 1                             \n");
                    completeTicketQuery.Append("   AND uc.IsSel = 1                                 \n");
                    completeTicketQuery.Append("   AND uc.UserID = " + objPropUser.UserID + "       \n");
                    completeTicketQuery.Append("   GROUP BY wk.fDesc                                \n");
                    completeTicketQuery.Append("UNION ALL                                           \n");
                    completeTicketQuery.Append("SELECT                                              \n");
                    completeTicketQuery.Append("   wk.fDesc AS DWork,                               \n");
                    completeTicketQuery.Append("   SUM(ISNULL(Total,0)) AS Total                              \n");
                    completeTicketQuery.Append("FROM TicketD tk                                     \n");
                    completeTicketQuery.Append("   INNER JOIN tblWork wk ON tk.fWork = wk.ID        \n");
                    completeTicketQuery.Append("   INNER JOIN Category ct ON tk.Cat = ct.Type       \n");
                    completeTicketQuery.Append("   INNER JOIN Loc l on l.Loc = tk.Loc               \n");
                    completeTicketQuery.Append("   INNER JOIN Owner ow on l.Owner = ow.ID           \n");
                    completeTicketQuery.Append("   INNER JOIN Rol rl  ON rl.ID = ow.Rol             \n");
                    completeTicketQuery.Append("   INNER JOIN tblUserCo uc on uc.CompanyID = rl.EN  \n");
                    completeTicketQuery.Append("WHERE Edate >= '" + startDate + "'                  \n");
                    completeTicketQuery.Append("   AND Edate < '" + endDate + "'                    \n");
                    completeTicketQuery.Append("   AND ct.ISDefault = 1                             \n");
                    completeTicketQuery.Append("   AND uc.IsSel = 1                                 \n");
                    completeTicketQuery.Append("   AND uc.UserID = " + objPropUser.UserID + "       \n");
                    completeTicketQuery.Append("   GROUP BY wk.fDesc                                \n");

                    result[1] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, completeTicketQuery.ToString()).Tables[0];
                }
                else
                {
                    // Open Ticket
                    var openTicketQuery = $"SELECT tblWork.fDesc AS DWork, SUM(ISNULL(Est,0)) AS Total FROM TicketO LEFT JOIN tblWork ON TicketO.fWork = tblWork.ID LEFT JOIN Category ON TicketO.Cat = Category.Type WHERE Assigned<> 4 AND Edate >= '{startDate}' AND Edate< '{endDate}' AND Category.ISDefault = 1 GROUP BY tblWork.fDesc";

                    result[0] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, openTicketQuery).Tables[0];

                    // Complete Ticket
                    var completeTicketQuery = $"SELECT tblWork.fDesc AS DWork, SUM(ISNULL(Total,0)) AS Total FROM TicketDPDA LEFT JOIN tblWork ON TicketDPDA.fWork = tblWork.ID LEFT JOIN Category ON TicketDPDA.Cat = Category.Type WHERE edate >= '{startDate}' AND edate< '{endDate}' AND Category.ISDefault = 1 GROUP BY tblWork.fDesc";
                    completeTicketQuery += $" UNION ALL ";
                    completeTicketQuery += $"SELECT tblWork.fDesc AS DWork, SUM(ISNULL(Total,0)) AS Total FROM TicketD LEFT JOIN tblWork ON TicketD.fWork = tblWork.ID LEFT JOIN Category ON TicketD.Cat = Category.Type WHERE edate >= '{startDate}' AND edate< '{endDate}' AND Category.ISDefault = 1 GROUP BY tblWork.fDesc";

                    result[1] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, completeTicketQuery).Tables[0];
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public DataSet GetTicketRecurringOpenAndCompletedDS(User objPropUser)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1);
            SqlParameter[] para = new SqlParameter[3];
            try
            {
                para[0] = new SqlParameter();
                para[0].ParameterName = "@EN";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropUser.EN;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@StartDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = startDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@EndDate";
                para[2].SqlDbType = SqlDbType.DateTime;
                para[2].Value = endDate;

                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetTicketRecurringChart", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetListBudgetName(User objPropUser)
        {
            string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
            int? yearEnd;
            int financialYear;

            try
            {
                var ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);

                if (ds.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                    yearEnd = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                else
                    yearEnd = DateTime.Now.Month - 1;

                var currentMonth = DateTime.Now.Month - 1;

                if (currentMonth > yearEnd)
                {
                    financialYear = DateTime.Now.Year + 1;
                }
                else
                {
                    financialYear = DateTime.Now.Year;
                }

                strQuery = $"Select [dbo].[Budget].[BudgetID], [dbo].[Budget].Budget from [dbo].[Budget] where Year = {financialYear}";
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public DataSet GetBudgetNameList(User objPropUser)
        {
            return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetListBudgetName");
        }

        public ArrayList GetAvgEstimate(User objPropUser)
        {

            var endDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day));
            var startDate = endDate.AddYears(-1);


            string avgFirstYear = $"; with aa as( Select isnull(Avg( DATEDIFF(day, e.Fdate, j.fdate)),0)  days from estimate e join job j on e.job = j.id join terr t on t.ID = e.estimateUserId join emp em on em.id = t.sman where j.fdate is not null and e.Fdate >= '{startDate}' and e.Fdate<='{endDate}' Group by em.Name )select ISNULL(AVG(days),0) days from aa ";

            endDate = endDate.AddMonths(-1);
            startDate = endDate.AddYears(-1);

            string avgSecondYear = $";with aa as( Select isnull(Avg( DATEDIFF(day, e.Fdate, j.fdate)),0)  days from estimate e join job j on e.job = j.id join terr t on t.ID = e.estimateUserId join emp em on em.id = t.sman where j.fdate is not null and e.Fdate >= '{startDate}' and e.Fdate<='{endDate}' Group by em.Name )select ISNULL(AVG(days),0) days from aa ";

            ArrayList listAverage = new ArrayList();
            try
            {
                double avgIYear = double.Parse(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, avgFirstYear).ToString());
                double avgIIYear = double.Parse(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, avgSecondYear).ToString());
                string result;
                if (avgIYear > avgIIYear)
                {
                    result = "Increment";
                }
                else if (avgIYear == avgIIYear)
                {
                    result = "Increment";
                }
                else
                {
                    result = "Decrement";
                }

                listAverage.Add(avgIYear);
                listAverage.Add(avgIIYear);
                listAverage.Add(result);
                return listAverage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public AvgEstimateResponse GetAvgEstimateList(User objPropUser)
        {
            AvgEstimateResponse _objAvgEstimateResponse = new AvgEstimateResponse();
            var endDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day));
            var startDate = endDate.AddYears(-1);
            var endDate1 = endDate.AddMonths(-1);
            var startDate1 = endDate.AddYears(-1);
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@startDate";
            para[0].SqlDbType = SqlDbType.DateTime;
            para[0].Value = startDate;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@endDate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = endDate;

            para[2] = new SqlParameter();
            para[2].ParameterName = "@startDate1";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = startDate1;

            para[3] = new SqlParameter();
            para[3].ParameterName = "@endDate1";
            para[3].SqlDbType = SqlDbType.DateTime;
            para[3].Value = endDate1;
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetAvgEstimate", para);

                double avgIYear = Convert.ToDouble(ds.Tables[0].Rows[0].Field<int>("avgFirstYear"));
                double avgIIYear = Convert.ToDouble(ds.Tables[1].Rows[0].Field<int>("avgSecondYear"));
                string result;
                if (avgIYear > avgIIYear)
                {
                    result = "Increment";
                }
                else if (avgIYear == avgIIYear)
                {
                    result = "Increment";
                }
                else
                {
                    result = "Decrement";
                }

                _objAvgEstimateResponse.avgIYear = avgIYear;
                _objAvgEstimateResponse.avgIIYear = avgIIYear;
                _objAvgEstimateResponse.result = result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _objAvgEstimateResponse;
        }
        public DataTable[] ConvertedEstimatesBySalespersonAverageDays(User objPropUser)
        {
            DataTable[] result = new DataTable[2];

            var endDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day));
            var startDate = endDate.AddYears(-1);

            string avgFirstYear = $" declare @t table (SalesPerson varchar(100),Avg int); insert into @t Select em.Name as SalesPerson ,Avg( DATEDIFF(day, e.Fdate, j.fdate)) Avg from estimate e join job j on e.job = j.id join terr t on t.ID = e.estimateUserId join emp em on em.id = t.sman where j.fdate is not null and  e.Fdate >='{startDate}' and e.Fdate <= '{endDate}' Group by em.Name;declare @c int=(select count(*) from @t); if(@c=0) begin insert into @t select '',1 end select * from @t ";

            result[0] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, avgFirstYear).Tables[0];

            endDate = endDate.AddMonths(-1);
            startDate = endDate.AddYears(-1);

            string avgSecondYear = $"  declare @t table (SalesPerson varchar(100),Avg int); insert into @t Select em.Name as SalesPerson ,Avg( DATEDIFF(day, e.Fdate, j.fdate)) Avg from estimate e join job j on e.job = j.id join terr t on t.ID = e.estimateUserId join emp em on em.id = t.sman where j.fdate is not null and  e.Fdate >='{startDate}' and e.Fdate <= '{endDate}' Group by em.Name;declare @c int=(select count(*) from @t); if(@c=0) begin insert into @t select '',1 end select * from @t ";

            result[1] = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, avgFirstYear).Tables[0];


            // DataSet ds = ConvertedEstimatesBySalespersonAverageDaysList(objPropUser);
            return result;

        }

        public DataSet ConvertedEstimatesBySalespersonAverageDaysList(User objPropUser)
        {
            RecurringHoursChartResponse obj = new RecurringHoursChartResponse();
            var endDate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day));
            var startDate = endDate.AddYears(-1);

            var endDate1 = endDate.AddMonths(-1);
            var startDate1 = endDate.AddYears(-1);
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@startDate";
            para[0].SqlDbType = SqlDbType.DateTime;
            para[0].Value = startDate;

            para[1] = new SqlParameter();
            para[1].ParameterName = "@endDate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = endDate;

            para[2] = new SqlParameter();
            para[2].ParameterName = "@startDate1";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = startDate1;

            para[3] = new SqlParameter();
            para[3].ParameterName = "@endDate1";
            para[3].SqlDbType = SqlDbType.DateTime;
            para[3].Value = endDate1;

            return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spRecurringHoursChart", para);

        }
        public DataSet GetUserDatePermission(User objPropUser)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("   Declare @CountFStart int = 0;");
            sql.Append("   Declare @CheckFStartNull int = 0;");
            sql.Append("   Declare @CountFEnd int = 0;");
            sql.Append("   Declare @CheckFEndNull int = 0;");
            sql.Append("   SET @CountFStart = (Select  count ( distinct fStart)  from tblUser)");
            sql.Append("   SET @CheckFStartNull = (Select  count (*)  from tblUser where fStart is null)");
            sql.Append("   SET @CountFEnd = (Select  count ( distinct fEnd)  from tblUser)");
            sql.Append("   SET @CheckFEndNull = (Select  count (*)  from tblUser where fEnd is null)");
            sql.Append(" If(@CountFStart = 1 AND @CheckFStartNull = 0 AND @CountFEnd = 1 AND @CheckFEndNull = 0 )");
            sql.Append(" BEGIN");
            sql.Append(" 	Select distinct fStart, fEnd from tblUser");
            sql.Append(" END");
            sql.Append(" ELSE");
            sql.Append(" BEGIN");
            sql.Append(" Select 0");
            sql.Append(" END");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetCloseOutDate(User objPropUser)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" Update tblUSer");
            sql.Append(" Set CODt ='" + objPropUser.CODt + "'");

            try
            {
                objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckNullCODt(User objPropUser)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" DECLARE @COUNT INT  = (Select count(*) from tblUser where CODt is null)");
                sql.Append(" IF @COUNT > 0");
                sql.Append(" BEGIN");
                sql.Append(" Select 1");
                sql.Append(" END");
                sql.Append(" ELSE");
                sql.Append(" BEGIN");
                sql.Append(" Select 0");
                sql.Append(" END");

                return objPropUser.CheckNullCODt = Convert.ToBoolean(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, sql.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCODt(User objPropUser)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" Select top 1 CODt from tblUser");
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomersLogs(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objPropUser.CustomerID + "  and ( Screen='Customer' or Screen='iCollections Popup') order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomersLogs(GetCustomersLogsParam _GetCustomersLogs, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from Log2 where ref =" + _GetCustomersLogs.CustomerID + "  and ( Screen='Customer' or Screen='iCollections Popup') order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPeriodCloseoutLogs(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Log2 where Screen='PeriodCloseout' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPeriodCloseoutLogs(User objPropUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[11];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@CloseOutDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = objPropUser.CODt;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@YearEndClose";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPropUser.YearEndClose;

                para[2] = new SqlParameter();
                para[2].ParameterName = "RetainedGLAcct";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = objPropUser.RetainedGLAcct;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@CurrentGLAcct";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = objPropUser.CurrentGLAcct;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@StartDate";
                para[4].SqlDbType = SqlDbType.DateTime;
                para[4].Value = objPropUser.FStart;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@EndDate";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = objPropUser.FEnd;


                para[6] = new SqlParameter();
                para[6].ParameterName = "@UserID";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = objPropUser.UserID;

                para[7] = new SqlParameter();
                para[7].ParameterName = "@UpdatedBy";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = objPropUser.MOMUSer;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateLog2PeriodCloseout", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getTeamByMonUser(String connConfig, String user)
        {
            DataSet result = new DataSet("Time");

            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlConnection con = new SqlConnection(connConfig);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetTeamByMomUser";
                cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = user;
                cmd.Connection = con;
                try
                {
                    con.Open();
                    da.SelectCommand = cmd;
                    da.Fill(result);

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
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getTeamByListID(String connConfig, String lsID)
        {
            try
            {
                String sql = "SELECT *  FROM team where '" + lsID + "' like '%' + CAST([ID] AS VARCHAR(20)) +'%'";
                return SqlHelper.ExecuteDataset(connConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMonthlyRevenueByCompany(User objPropUser, int? companyId)
        {
            try
            {
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1);
                string strQuery = string.Empty;

                if (companyId != null)
                {
                    strQuery = $"SELECT jtype.Type AS Department, ISNULL(temp.Revenue,0) AS Revenue FROM JobType jtype LEFT JOIN (SELECT jt.Type, SUM(ISNULL(j.Rev,0)) AS Revenue FROM JobType jt INNER JOIN Job j ON jt.ID = j.Type INNER JOIN Loc l ON j.Loc = l.Loc INNER JOIN Rol r ON l.Rol = r.ID WHERE j.fDate >= '{startDate}' AND j.fDate < '{endDate}'  AND r.EN = {companyId} GROUP BY jt.Type) AS temp ON temp.Type = jtype.Type ORDER BY jtype.Type";
                }
                else
                {
                    strQuery = $"SELECT jobT.Type AS Department, SUM(ISNULL(job.Rev,0)) AS Revenue FROM JobType jobT LEFT JOIN Job job ON jobT.ID = job.Type AND job.fDate >= '{startDate}' AND job.fDate < '{endDate}' GROUP BY jobT.Type";
                }

                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetRevenueByCompany(User objPropUser, int? companyId)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1);
            SqlParameter[] para = new SqlParameter[3];
            try
            {
                para[0] = new SqlParameter();
                para[0].ParameterName = "@StartDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = startDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@EndDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = endDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@CompanyId";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = companyId;
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetRevenueByCompany", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTroubleCallsByEquipment(User objPropUser, TroubleCallsByEquipmentGraphRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@StartDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = request.StartDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@EndDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = request.EndDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@Top";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = request.TopSelect;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@CallTimes";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = request.CallTimes;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@UserID";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = request.UserID;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@EN";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = request.EN;

                para[6] = new SqlParameter();
                para[6].ParameterName = "@Categories";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = request.Categories;

                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "GetTroubleCallsByEquipmentData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetTroubleCallsByEquipment(TroubleCallsByEquipmentGraphRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@StartDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = request.StartDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@EndDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = request.EndDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@Top";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = request.TopSelect;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@CallTimes";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = request.CallTimes;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@UserID";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = request.UserID;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@EN";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = request.EN;

                para[6] = new SqlParameter();
                para[6].ParameterName = "@Categories";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = request.Categories;

                return SqlHelper.ExecuteDataset(request.ConnConfig, CommandType.StoredProcedure, "GetTroubleCallsByEquipmentData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketCountByCategory(User objPropUser, string categories)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@StartDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = objPropUser.StartDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@EndDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = objPropUser.EndDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@UserID";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objPropUser.UserID;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@EN";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = objPropUser.EN;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Categories";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = categories;

                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetTicketCountByCategoryAndDateRange", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public int AddDashboard(User _objUser, string name, bool isDefault)
        {
            try
            {
                string query = "INSERT INTO [Dashboard](UserID, Name, IsDefault) VALUES(@UserID, @Name, @IsDefault); SELECT SCOPE_IDENTITY()";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserID", _objUser.UserID));
                parameters.Add(new SqlParameter("@Name", name));
                parameters.Add(new SqlParameter("@IsDefault", isDefault));

                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDashboard(User _objUser, int dashboardID, string dashboardName, bool isDefault)
        {
            try
            {
                string query = "UPDATE [Dashboard] SET UserID = @UserID, Name = @Name, IsDefault = @IsDefault WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserID", _objUser.UserID));
                parameters.Add(new SqlParameter("@ID", dashboardID));
                parameters.Add(new SqlParameter("@Name", dashboardName));
                parameters.Add(new SqlParameter("@IsDefault", isDefault));

                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDashboardDockStates(User _objUser, int dashboardID, string dockStates)
        {
            try
            {
                string query = "UPDATE [Dashboard] SET DockStates = @DockStates WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", dashboardID));
                parameters.Add(new SqlParameter("@DockStates", dockStates));

                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateDashboardDockStatesySP(UpdateDashboardDockStatesParam _obj)
        {
            String strConnString = _obj.ConnString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateDashboardDockStates";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _obj.DashboardId;
            cmd.Parameters.Add("@DockStates", SqlDbType.NVarChar).Value = _obj.DockStates;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
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
        }

        public void UnsetDashboardDefault(User _objUser)
        {
            try
            {
                string query = "UPDATE [Dashboard] SET IsDefault = 0 WHERE UserID = @UserID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserID", _objUser.UserID));

                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteDashboard(User _objUser, int dashboardID)
        {
            try
            {
                string query = "DELETE FROM UserDash WHERE Dashboard = @Dashboard; DELETE FROM Dashboard WHERE ID = @Dashboard;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Dashboard", dashboardID));

                SqlHelper.ExecuteNonQuery(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListDashboard(User objPropUser)
        {
            try
            {
                string query = $"SELECT * FROM Dashboard WHERE UserID = {objPropUser.UserID}";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<Dashboard> GetDashboardByUserId(GetDashboardParam _GetDashboardByIDParam, string ConnectionString)
        {
            List<Dashboard> _lstDashboard = new List<Dashboard>();
            try
            {

                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetDashboardByIDParam.UserId
                };
                DataSet ds = SqlHelper.ExecuteDataset(_GetDashboardByIDParam.ConnConfig, CommandType.StoredProcedure, "spGetDashboardByUserId", param);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstDashboard.Add(new Dashboard()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"]) ? 0 : dr["UserID"]),
                        Name = Convert.ToString(dr["Name"].ToString()),
                        DockStates = Convert.ToString(dr["DockStates"].ToString()),
                        IsDefault = Convert.ToBoolean(dr["IsDefault"])
                    });
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return _lstDashboard;
        }

        public DataSet GetDashboardByID(User objPropUser, int dashboardID)
        {
            try
            {
                string query = $"SELECT * FROM Dashboard WHERE ID = {dashboardID}";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<Dashboard> GetDashboard(GetDashboardParam _GetDashboardByIDParam, string ConnectionString)
        {
            List<Dashboard> _lstDashboard = new List<Dashboard>();
            try
            {

                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@DashboardId",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetDashboardByIDParam.DashboardId
                };
                DataSet ds = SqlHelper.ExecuteDataset(_GetDashboardByIDParam.ConnConfig, CommandType.StoredProcedure, "spGetDashboardByID", param);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstDashboard.Add(new Dashboard()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"]) ? 0 : dr["UserID"]),
                        Name = Convert.ToString(dr["Name"].ToString()),
                        DockStates = Convert.ToString(dr["DockStates"].ToString()),
                        IsDefault = Convert.ToBoolean(dr["IsDefault"])
                    });
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return _lstDashboard;
        }

        public DataSet GetDashboardDefault(User objPropUser)
        {
            try
            {
                string query = $"SELECT * FROM Dashboard WHERE UserID = {objPropUser.UserID} AND IsDefault = 1";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddUserDashboard(User _objUser, UserDash request)
        {
            try
            {
                string query = "INSERT INTO [UserDash](UserID, KPIID, Dashboard, Section, Position) VALUES(@UserID, @KPIID, @Dashboard, @Section, @Position)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserID", request.UserID));
                parameters.Add(new SqlParameter("@KPIID", request.KPIID));
                parameters.Add(new SqlParameter("@Dashboard", request.Dashboard));
                parameters.Add(new SqlParameter("@Section", request.Section));
                parameters.Add(new SqlParameter("@Position", request.Position));

                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListKPIs(User objPropUser)
        {
            try
            {
                string query = "SELECT * FROM KPI";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListDashKPI(User objPropUser, int dashboardID)
        {
            try
            {
                string query = $"SELECT KPI.* FROM UserDash INNER JOIN KPI ON KPI.ID = UserDash.KPIID WHERE UserDash.Dashboard = {dashboardID}";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<KPI> GetListDashKPI(GetListDashKPIParam _GetListDashKPIParam, string ConnectionString)
        {
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@DashboardId",
                SqlDbType = SqlDbType.Int,
                Value = _GetListDashKPIParam.dashboardId
            };
            DataSet ds = SqlHelper.ExecuteDataset(_GetListDashKPIParam.ConnConfig, CommandType.StoredProcedure, "spGetListDashKPI", param);
            List<KPI> _KPI = new List<KPI>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _KPI.Add(new KPI()
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = Convert.ToString(dr["Name"].ToString()),
                    Module = Convert.ToString(dr["Module"].ToString()),
                    Screen = Convert.ToString(dr["Screen"].ToString()),
                    //Type = Convert.ToInt32(dr["Type"].ToString()),
                    UserControl = Convert.ToString(dr["UserControl"].ToString())
                });
            }
            return _KPI;
        }

        public void DeleteUserDash(User objPropUser, int dashboardID)
        {
            try
            {
                string query = $"DELETE FROM UserDash WHERE Dashboard = {dashboardID}";

                SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region "Classification"
        public DataSet getEquipClassification(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from elev where Classification=e.edesc) as Count, (Case isnull(status,1) when 1 then 'Active' else 'Inactive' End) as Status from ElevatorSpec e  where ecat=3 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getEquipClassificationActive(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from elev where Classification=e.edesc) as Count, (Case isnull(status,1) when 1 then 'Active' else 'Inactive' End) as Status from ElevatorSpec e  where ecat=3 and isnull(status,1)=1 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getEquipClassificationLikeName(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from elev where Classification=e.edesc) as Count, (Case isnull(status,1) when 1 then 'Active' else 'Inactive' End) as Status from ElevatorSpec e  where ecat=3  and LOWER(REPLACE(LTRIM(RTRIM(edesc)), ' ', '')) like'" + objPropUser.Classification.Replace(" ", "").ToLower() + "%' order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public DataSet getEquipClassification(GetEquipClassificationParam _GetEquipClassification, string ConnectionString)
        {
            try
            {
                return _GetEquipClassification.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select edesc,Label,(select count(1) from elev where Classification=e.edesc) as Count, (Case isnull(status,1) when 1 then 'Active' else 'Inactive' End) as Status from ElevatorSpec e  where ecat=3 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLeadEquipClassification(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,Label,(select count(1) from LeadEquip where Classification=e.edesc) as Count from ElevatorSpec e  where ecat=3 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLeadEquipClassification(GetLeadEquipClassificationParam _GetLeadEquipClassification, string ConnectionString)
        {
            try
            {
                return _GetLeadEquipClassification.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select edesc,Label,(select count(1) from LeadEquip where Classification=e.edesc) as Count from ElevatorSpec e  where ecat=3 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddEquipClassification(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddEquipmentClassification", objPropUser.Classification, Convert.ToBoolean(objPropUser.Status));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EditEquipClassification(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateEquipmentClassification", objPropUser.oldClassification, objPropUser.Classification, Convert.ToBoolean(objPropUser.Status));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEquipClassification(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Elev where Classification = '" + objPropUser.Classification + "' ) begin delete from ElevatorSpec where EDesc='" + objPropUser.Classification + "' and ecat=3 end else begin RAISERROR ('Cannot delete %s it is in use !', 16, 1,'" + objPropUser.Classification + "') RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region "EquipmentTestPricing"

        public int AddEquipmentTestPricing(String connConfig, EquipTestPrice equipTest)
        {
            String strConnString = connConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAddEquipmentTestPricing";
            cmd.Parameters.Add("@Classification", SqlDbType.VarChar).Value = equipTest.Classification.Trim();
            cmd.Parameters.Add("@TestTypeId", SqlDbType.Int).Value = equipTest.TestTypeId;
            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = equipTest.Amount;
            cmd.Parameters.Add("@Override", SqlDbType.Decimal).Value = equipTest.OverrideAmount;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = equipTest.CreatedBy;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = equipTest.Remarks;
            cmd.Parameters.Add("@DefaultHour", SqlDbType.Decimal).Value = equipTest.DefaultHour;
            cmd.Parameters.Add("@PriceYear", SqlDbType.Int).Value = equipTest.PriceYear;
            cmd.Parameters.Add("@ThirdPartyRequired", SqlDbType.Bit).Value = equipTest.IsThirdPartyRequired;
            cmd.Parameters.Add("@UpdateType", SqlDbType.Int).Value = equipTest.UpdateType;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
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
            if (param.Value != DBNull.Value)
                return Convert.ToInt32(param.Value);
            else
                return -1;
        }

        public void UpdateEquipmentTestPricing(String connConfig, EquipTestPrice equipTest)
        {
            String strConnString = connConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateEquipmentTestPricing";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = equipTest.Id;
            cmd.Parameters.Add("@Classification", SqlDbType.VarChar).Value = equipTest.Classification.Trim();
            cmd.Parameters.Add("@TestTypeId", SqlDbType.Int).Value = equipTest.TestTypeId;
            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = equipTest.Amount;
            cmd.Parameters.Add("@Override", SqlDbType.Decimal).Value = equipTest.OverrideAmount;
            cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = equipTest.UpdatedBy;
            cmd.Parameters.Add("@UpdateType", SqlDbType.Int).Value = equipTest.UpdateType;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = equipTest.Remarks;
            cmd.Parameters.Add("@DefaultHour", SqlDbType.Decimal).Value = equipTest.DefaultHour;
            cmd.Parameters.Add("@PriceYear", SqlDbType.Int).Value = equipTest.PriceYear;
            cmd.Parameters.Add("@ThirdPartyRequired", SqlDbType.Bit).Value = equipTest.IsThirdPartyRequired;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
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
        }

        public void DeleteEquipmentTestPricingById(String connConfig, int Id)
        {

            String strConnString = connConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spDeleteEquipmentTestPricingById";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Id;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
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
        }

        public DataSet GetAllEquipmentTestPricing(String connConfig)
        {
            DataSet ds = new DataSet();
            String strConnString = connConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllEquipmentTestPricing";
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "EquipmentTestPrice");
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

        public DataSet ValidateEquipmentTestPricing(String connConfig, String classification, int testTypeId, int priceYear)
        {

            if (priceYear == 0) priceYear = DateTime.Now.Year;

            try
            {
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@Classification";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = classification;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@TestTypeId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testTypeId;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@PriceYear";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = priceYear;


                return SqlHelper.ExecuteDataset(connConfig, CommandType.StoredProcedure, "spGetTestPriceByYear", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }




        }
        public DataSet DuplicateEquipTestPrice(String connConfig, String classification, int testTypeId, int priceYear)
        {

            if (priceYear == 0) priceYear = DateTime.Now.Year;

            try
            {
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@Classification";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = classification;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@TestTypeId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testTypeId;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@PriceYear";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = priceYear;


                return SqlHelper.ExecuteDataset(connConfig, CommandType.StoredProcedure, "spDuplicateEquipTestPrice", para);
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
        #endregion

        public DataSet GetShutdownReasons(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT * FROM ElevShutdownReason order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetShutdownReasons(GetShutdownReasonsParam _GetShutdownReasons, string ConnectionString)
        {
            try
            {
                return _GetShutdownReasons.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT * FROM ElevShutdownReason order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetShutdownReasonByID(User objPropUser, int eqsdReasonID)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, string.Format("SELECT * FROM ElevShutdownReason WHERE ID = {0}", eqsdReasonID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetShutdownReasonByID(GetShutdownReasonByIDParam _GetShutdownReasonByID, string ConnectionString, int eqsdReasonID)
        {
            try
            {
                return _GetShutdownReasonByID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, string.Format("SELECT * FROM ElevShutdownReason WHERE ID = {0}", eqsdReasonID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddShutdownReason(User objPropUser, string eqsdReason, bool eqsdPlanned)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ShutdownReason";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = eqsdReason;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Planned";
                param[1].SqlDbType = SqlDbType.Bit;
                param[1].Value = eqsdPlanned;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Username";
                param[2].SqlDbType = SqlDbType.VarChar;
                param[2].Value = objPropUser.MOMUSer;
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddShutdownReason", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddShutdownReason(AddShutdownReasonParam _AddShutdownReason, string ConnectionString, string eqsdReason, bool eqsdPlanned)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ShutdownReason";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = eqsdReason;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Planned";
                param[1].SqlDbType = SqlDbType.Bit;
                param[1].Value = eqsdPlanned;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Username";
                param[2].SqlDbType = SqlDbType.VarChar;
                param[2].Value = _AddShutdownReason.MOMUSer;
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddShutdownReason", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditShutdownReason(User objPropUser, int eqsdID, string eqsdReason, bool eqsdPlanned)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = eqsdID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@ShutdownReason";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = eqsdReason;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Planned";
                param[2].SqlDbType = SqlDbType.Bit;
                param[2].Value = eqsdPlanned;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@Username";
                param[3].SqlDbType = SqlDbType.VarChar;
                param[3].Value = objPropUser.MOMUSer;
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spEditShutdownReason", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void EditShutdownReason(EditShutdownReasonParam _EditShutdownReason, string ConnectionString, int eqsdID, string eqsdReason, bool eqsdPlanned)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = eqsdID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@ShutdownReason";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = eqsdReason;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Planned";
                param[2].SqlDbType = SqlDbType.Bit;
                param[2].Value = eqsdPlanned;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@Username";
                param[3].SqlDbType = SqlDbType.VarChar;
                param[3].Value = _EditShutdownReason.MOMUSer;
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spEditShutdownReason", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEquipShutdownLogs(User objPropUser)
        {
            try
            {
                // return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select (select tag from Loc where Loc=e.Loc) as location, Loc, Owner, Unit, fDesc, Type, Cat, Manuf, Serial, State, Since, Last, Price, Status, Building, Remarks, fGroup, Template, InstallBy, install, category from Elev e where ID=" + objPropUser.EquipID + "  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.Elev= " + objPropUser.EquipID    );
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetEquipmentShutdownLogs", objPropUser.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEquipShutdownLogs(GetEquipShutdownLogsParam _GetEquipShutdownLogs, string ConnectionString)
        {
            try
            {
                // return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select (select tag from Loc where Loc=e.Loc) as location, Loc, Owner, Unit, fDesc, Type, Cat, Manuf, Serial, State, Since, Last, Price, Status, Building, Remarks, fGroup, Template, InstallBy, install, category from Elev e where ID=" + objPropUser.EquipID + "  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.Elev= " + objPropUser.EquipID    );
                return _GetEquipShutdownLogs.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetEquipmentShutdownLogs", _GetEquipShutdownLogs.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getPayRoll(User objPropUser)
        {
            var para = new SqlParameter[2];

            para[0] = new SqlParameter
            {
                ParameterName = "@fDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropUser.FStart
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@eDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropUser.FEnd
            };


            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetPayroll", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getPayRoll(getTimesheetParam objPropUser, string ConnectionString)
        {
            var para = new SqlParameter[2];

            para[0] = new SqlParameter
            {
                ParameterName = "@fDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropUser.FStart
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@eDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropUser.FEnd
            };


            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetPayroll", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateCoCode(User objPropUser)
        {
            try
            {

                string query = "Update Control SET CoCode=@code";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@code", objPropUser.CoCode));

                SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public void UpdateCoCode(getConnectionConfigParam objPropUser, string ConnectionString)
        {
            try
            {

                string query = "Update Control SET CoCode=@code";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@code", objPropUser.CoCode));

                SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCoCode(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT top 1 CoCode FROM Control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetCoCode(getConnectionConfigParam objPropUser, string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT top 1 CoCode FROM Control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Get Users in MOM
        /// status= 0: Get Active
        /// status= 1: Get InActive
        /// other: Both Active/InActive
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetUsersForTeamMemberList(User objPropUser)
        {
            var para = new SqlParameter[2];

            para[0] = new SqlParameter
            {
                ParameterName = "@status",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.Status
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@job",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.JobId
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetUsersForTeamMemberList", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetUsersAndRolesForTeamMemberList(User objPropUser)
        {
            var para = new SqlParameter[2];

            para[0] = new SqlParameter
            {
                ParameterName = "@status",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.Status
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@job",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.JobId
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetUsersAndRolesForTeamMemberList", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetTeamMemberFromTemplate(User objPropUser)//, int customLabelId)
        {
            var para = new SqlParameter[2];
            //var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@job",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.JobId
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@jobt",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.JobtypeID
            };

            //para[2] = new SqlParameter
            //{
            //    ParameterName = "@customLabelId",
            //    SqlDbType = SqlDbType.Int,
            //    Value = customLabelId
            //};

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetTeamMemberFromTemplate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetLocInfoForEstimateByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocInfoForEstimateByID", objPropUser.LocID, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetGridUserSettings(User objPropUser)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@userId",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.UserID
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@pageName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropUser.PageName
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@gridId",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropUser.GridId
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetGridUserSettings", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet getLevels(User objPropUser)
        {
            try
            {
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select Distinct l.ID, l.Name, (l.Name + ' - ' + l.Label) As Label, o.Level  From Labels l  inner join TicketO o on l.Name=o.Level Left Join TicketD d on d.Level = l.Name Left Join TicketDPDA p on p.Level = l.Name  Where l.screen = 'Level'");
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select Distinct l.ID, l.Name, (l.Name + ' - ' + l.Label) As Label  From Labels l   Where l.screen = 'Level'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Mitsubishi
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public string getUserEmailFromTS(User objPropUser)
        {
            try
            {
                return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select Remarks as email from tbluser where fuser='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void UpdateReapproveFromFDESC(string connConfig)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(connConfig, CommandType.StoredProcedure, "USP_UpdateReapproveFromFDESC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddUpdateCustomerQB(User objPropUser)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[22];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustomerID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = objPropUser.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddUpdateQBCustomer", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddUpdateQBLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Owner";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.CustomerID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "QBLocationID";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.QBlocationID;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBstax";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.Stax;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Balance";
            para[23].SqlDbType = SqlDbType.Money;
            para[23].Value = objPropUser.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddUpdateQBLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddUpdateQBLocation", para);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateLocationContactRecordLog(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.RolId;

            para[2] = new SqlParameter();
            para[2].ParameterName = "UpdatedBy";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.MOMUSer;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateLocContactRecordLog", para[0], para[1], para[2]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateLocationContactRecordLog(UpdateLocationContactRecordLogParam _UpdateLocationContactRecordLog, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = _UpdateLocationContactRecordLog.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _UpdateLocationContactRecordLog.RolId;

            para[2] = new SqlParameter();
            para[2].ParameterName = "UpdatedBy";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _UpdateLocationContactRecordLog.MOMUSer;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateLocContactRecordLog", para[0], para[1], para[2]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationLog(User objPropUser)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@locID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropUser.LocID
                };


                try
                {
                    return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetLocationLog", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getLocationLog(GetLocationLogParam _GetLocationLog, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@locID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetLocationLog.LocID
                };
                try
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetLocationLog", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getContactLogByLocID(User objPropUser)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@locID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropUser.LocID
                };


                try
                {
                    return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spContactLogByLocID", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getContactLogByLocID(GetContactLogByLocIDParam _GetContactLogByLocID, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@locID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetContactLogByLocID.LocID
                };
                try
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spContactLogByLocID", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getLocContactByRolID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetlocContactByRolID", objPropUser.RolId, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetLocContactByRolIDCustomer(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocContactByRolIDCustomer", objPropUser.RolId, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public DataSet getLocContactByRolID(GetLocContactByRolIDParam _GetLocContactByRolID, string ConnectionString)
        {
            try
            {
                return _GetLocContactByRolID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetlocContactByRolID", _GetLocContactByRolID.RolId, _GetLocContactByRolID.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateCustomerContactRecordLog(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.RolId;

            para[2] = new SqlParameter();
            para[2].ParameterName = "UpdatedBy";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.MOMUSer;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomerContactRecordLog", para[0], para[1], para[2]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateCustomerContactRecordLog(UpdateCustomerContactRecordLogParam _UpdateCustomerContactRecordLog, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            if (_UpdateCustomerContactRecordLog.ContactData.Rows.Count > 0)
            {
                if (_UpdateCustomerContactRecordLog.ContactData.Rows[0]["ContactID"].ToString() != "0")
                {
                    para[0].Value = _UpdateCustomerContactRecordLog.ContactData;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ContactID", typeof(int));
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("Phone", typeof(string));
                    dt.Columns.Add("Fax", typeof(string));
                    dt.Columns.Add("Cell", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("Title", typeof(string));
                    dt.Columns.Add("EmailTicket", typeof(byte));
                    dt.Columns.Add("EmailRecInvoice", typeof(byte));
                    dt.Columns.Add("ShutdownAlert", typeof(byte));
                    dt.Columns.Add("EmailRecTestProp", typeof(byte));
                    para[0].Value = dt;
                }
            }
            //para[0].Value = _UpdateCustomerContactRecordLog.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _UpdateCustomerContactRecordLog.RolId;

            para[2] = new SqlParameter();
            para[2].ParameterName = "UpdatedBy";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _UpdateCustomerContactRecordLog.MOMUSer;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateCustomerContactRecordLog", para[0], para[1], para[2]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getContactLogByCustomerID(User objPropUser)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@CusID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropUser.LocID
                };


                try
                {
                    return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spCustomerContactLogByCusID", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getContactLogByCustomerID(GetContactLogByCustomerIDParam _GetContactLogByCustomerID, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@CusID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetContactLogByCustomerID.LocID
                };


                try
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spCustomerContactLogByCusID", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getContactByRolID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetContactByRolID", objPropUser.RolId, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public DataSet getContactByRolID(GetContactByRolIDParam _GetContactByRolID, string ConnectionString)
        {
            try
            {
                return _GetContactByRolID.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetContactByRolID", _GetContactByRolID.RolId, _GetContactByRolID.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateUserGridCustomSettings(User objPropUser, String gridCustomSettings)
        {
            SqlParameter[] para = new SqlParameter[4];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserId";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropUser.UserID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "PageName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.PageName;

            para[2] = new SqlParameter();
            para[2].ParameterName = "GridId";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.GridId;

            para[3] = new SqlParameter();
            para[3].ParameterName = "GridCustomSettings";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = gridCustomSettings;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateUserGridCustomSettings", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet DeleteUserGridCustomSettings(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserId";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropUser.UserID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "PageName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.PageName;

            para[2] = new SqlParameter();
            para[2].ParameterName = "GridId";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.GridId;

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spDeleteUserGridCustomSettings", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDefaultGridCustomSettings(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "PageName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.PageName;

            para[1] = new SqlParameter();
            para[1].ParameterName = "GridId";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.GridId;

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetDefaultColumnSettings", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUserExchangeContacts(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropUser.UserID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "ContactsList";
            para[1].SqlDbType = SqlDbType.Structured;
            para[1].Value = objPropUser.ContactData;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateUserExchangeContacts", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserExchangeContacts(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[1];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropUser.UserID;

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetUserExchangeContacts", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCategoryActive(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select type from category where isnull(status,1)=1 order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePassword(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateUserPassword", objPropUser.Username, objPropUser.Password, objPropUser.NewPassword, objPropUser.DBName, objPropUser.DBType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateForgotPassword(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateUserForgotPassword", objPropUser.Username, objPropUser.NewPassword, objPropUser.DBName, objPropUser.DBType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCompanyByName(User objPropUser)
        {
            string strCommandText = "select companyname,Upper(dbname+':'+type) as dbname, ApplyPasswordRules from tblcontrol ";

            if (!string.IsNullOrEmpty(objPropUser.DBName))
            {
                strCommandText += " where dbname in (" + objPropUser.DBName + ")";
            }

            strCommandText += " order by companyname ";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, strCommandText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserInfoByUsername(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetUserInfoByUsername", objPropUser.Username, objPropUser.DBName, objPropUser.DBType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserInfoByUsernameAndEmail(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetUserInfoByUsernameAndEmail", objPropUser.Username, objPropUser.Email, objPropUser.DBName, objPropUser.DBType, objPropUser.ForgotPwRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUsersForResetPwAdmin(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetUsersForResetPwAdmin");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTeamMemberTitle(User objPropUser, bool isIncludeProjectTeamMemberTitle = false)
        {
            try
            {
                if (isIncludeProjectTeamMemberTitle)
                {
                    var query = "SELECT t.RoleName As Title FROM [dbo].[tblRole] t  WHERE ISNULL(t.Status, 0) = 0" +
                        "UNION " +
                        "SELECT t.Title FROM Team t " +
                        "inner join Job j WITH(NOLOCK) on  t.jobid = j.id  " +
                        "inner join loc l WITH(NOLOCK)   on l.loc=j.loc " +
                        "inner join Owner o WITH(NOLOCK)   on o.ID=l.owner  " +
                        "inner join rol r WITH(NOLOCK)   on o.Rol=r.ID";
                    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
                }
                else
                {
                    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT t.RoleName As Title FROM [dbo].[tblRole] t  WHERE ISNULL(t.Status, 0) = 0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddProjectTeamMemberTitle(TeamMemberTitle objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddProjectTeamMemberTitle", objPropUser.Title, objPropUser.IsDefault, objPropUser.Remarks);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateProjectTeamMemberTitle(TeamMemberTitle objPropUser)
        {
            SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateProjectTeamMemberTitle", objPropUser.Id, objPropUser.Title, objPropUser.IsDefault, objPropUser.Remarks);
        }

        public void DeleteProjectTeamMemberTitleById(TeamMemberTitle objPropUser)
        {
            SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteTeamMemberTitle", objPropUser.Id);
        }
        public DataSet GetTeamMemberTitleSearch(User _objUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objUser.ConnConfig, "spGetTeamMemberTitleSearch", _objUser.SearchValue, _objUser.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region User Role
        public DataSet GetRoleByName(UserRole userRole)
        {
            SqlParameter[] para = new SqlParameter[1];

            para[0] = new SqlParameter();
            para[0].ParameterName = "RoleName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = userRole.RoleName;

            try
            {
                return SqlHelper.ExecuteDataset(userRole.ConnConfig, CommandType.StoredProcedure, "spGetRoleByName", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRoleByID(UserRole userRole)
        {
            SqlParameter[] para = new SqlParameter[1];

            para[0] = new SqlParameter();
            para[0].ParameterName = "RoleID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = userRole.RoleID;

            try
            {
                return SqlHelper.ExecuteDataset(userRole.ConnConfig, CommandType.StoredProcedure, "spGetRoleByID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUsersOfRoleByID(UserRole userRole)
        {
            SqlParameter[] para = new SqlParameter[1];

            para[0] = new SqlParameter();
            para[0].ParameterName = "RoleID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = userRole.RoleID;

            try
            {
                return SqlHelper.ExecuteDataset(userRole.ConnConfig, CommandType.StoredProcedure, "spGetUsersOfRoleByID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUsersOfRoleByName(UserRole userRole)
        {
            SqlParameter[] para = new SqlParameter[1];

            para[0] = new SqlParameter();
            para[0].ParameterName = "RoleName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = userRole.RoleName;

            try
            {
                return SqlHelper.ExecuteDataset(userRole.ConnConfig, CommandType.StoredProcedure, "spGetUsersOfRoleByName", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetRoles(UserRole userRole)
        //{
        //    try
        //    {
        //        return SqlHelper.ExecuteDataset(userRole.ConnConfig, CommandType.Text, "SELECT * FROM tblRole Order By RoleName");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public int AddUpdateUserRole(UserRole userRole, User objPropUser)
        {
            try
            {
                SqlParameter usersPara = new SqlParameter();
                usersPara.ParameterName = "@Users";
                usersPara.SqlDbType = SqlDbType.Structured;
                usersPara.Value = userRole.Users;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spAddUpdateUserRole",
                    userRole.RoleID,
                    userRole.RoleName,
                    userRole.RoleDescription,
                    userRole.Status,
                    userRole.UserName,
                    usersPara,
                    objPropUser.PurchaseOrd,
                    objPropUser.Expenses,
                    objPropUser.ProgFunctions,
                    objPropUser.AccessUser,
                    //objPropUser.Mapping,
                    //objPropUser.Schedule,
                    objPropUser.Salesperson,
                    objPropUser.Dispatch,
                    objPropUser.SalesMgr,
                    objPropUser.MassReview,
                    objPropUser.EmpMaintenance,
                    objPropUser.Timestampfix,
                    //objPropUser.Department,
                    objPropUser.AddEquip,
                    objPropUser.EditEquip,
                    objPropUser.FChart,
                    objPropUser.AddChart,
                    objPropUser.EditChart,
                    objPropUser.ViewChart,
                    objPropUser.FGLAdj,
                    objPropUser.AddGLAdj,
                    objPropUser.EditGLAdj,
                    objPropUser.ViewGLAdj,
                    objPropUser.FDeposit,
                    objPropUser.AddDeposit,
                    objPropUser.EditDeposit,
                    objPropUser.ViewDeposit,
                    objPropUser.FCustomerPayment,
                    objPropUser.AddCustomerPayment,
                    objPropUser.EditCustomerPayment,
                    objPropUser.ViewCustomerPayment,
                    objPropUser.FinanStatement,
                    objPropUser.APVendor,
                    objPropUser.APBill,
                    objPropUser.APBillPay,
                    objPropUser.APBillSelect,
                    objPropUser.CustomerPermissions,
                    objPropUser.LocationrPermissions,
                    objPropUser.ProjectPermissions,
                    objPropUser.DeleteEquip,
                    objPropUser.ViewEquip,
                    objPropUser.TicketDelete,
                    objPropUser.ProjectListPermission,
                    objPropUser.FinancePermission,
                    objPropUser.BOMPermission,
                    objPropUser.WIPPermission,
                    objPropUser.MilestonesPermission,
                    objPropUser.InventoryItemPermissions,
                    objPropUser.InventoryAdjustmentPermissions,
                    objPropUser.InventoryWarehousePermissions,
                    objPropUser.InventorysetupPermissions,
                    objPropUser.InventoryFinancePermissions,
                    objPropUser.DocumentPermissions,
                    objPropUser.ContactPermission,
                    objPropUser.SalesAssigned,
                    objPropUser.ProjectTempPermissions,
                    objPropUser.NotificationOnAddOpportunity,
                    objPropUser.VendorsPermission,
                    objPropUser.POLimit,
                    objPropUser.POApprove,
                    objPropUser.POApproveAmt,
                    objPropUser.InvoivePermissions,
                    objPropUser.BillingCodesPermission,
                    objPropUser.POPermission,
                    objPropUser.Purchasingmodule,
                    objPropUser.Billingmodule,
                    objPropUser.RPOPermission,
                    objPropUser.AccountPayablemodule,
                    objPropUser.PaymentHistoryPermission,
                    objPropUser.Customermodule,
                    objPropUser.ApplyPermissions,
                    objPropUser.DepositPermissions,
                    objPropUser.CollectionsPermissions,
                    objPropUser.Financialmodule,
                    objPropUser.ChartPermissions,
                    objPropUser.JournalEntryPermissions,
                    objPropUser.BankReconciliationPermissions,
                    objPropUser.Recurringmodule,
                    objPropUser.RecurringContractsPermission,
                    objPropUser.ProcessC,
                    objPropUser.ProcessT,
                    objPropUser.SafetyTestsPermission,
                    objPropUser.RenewEscalatePermission,
                    objPropUser.Schedulemodule,
                    objPropUser.ScheduleBoardPermission,
                    objPropUser.TicketPermission,
                    objPropUser.TicketResolvedPermission,
                    objPropUser.MTimesheetPermission,
                    objPropUser.ETimesheetPermission,
                    objPropUser.MapRPermission,
                    objPropUser.RouteBuilderPermission,
                    objPropUser.MassTimesheetCheck,
                    objPropUser.CreditHoldPermission,
                    objPropUser.CreditFlagPermission,
                    objPropUser.SalesPermission,
                    objPropUser.TasksPermission,
                    objPropUser.CompleteTasksPermission,
                    objPropUser.FollowUpPermission,
                    objPropUser.ProposalPermission,
                    objPropUser.EstimatePermission,
                    objPropUser.ConvertEstimatePermission,
                    objPropUser.SalesSetupPermission,
                    objPropUser.PONotification,
                    objPropUser.JobClosePermission,
                    objPropUser.Inventorymodule,
                    objPropUser.Projectmodule,
                    objPropUser.JobCompletedPermission,
                    objPropUser.JobReopenPermission,
                    objPropUser.wirteOff,
                    objPropUser.IsProjectManager,
                    objPropUser.IsAssignedProject,
                    objPropUser.MinAmount,
                    objPropUser.MaxAmount,
                    objPropUser.TicketVoidPermission,
                    objPropUser.Employee,
                    objPropUser.PRProcess,
                    objPropUser.PRRegister,
                    objPropUser.PRReport,
                    objPropUser.PRWage,
                    objPropUser.PRDeduct,
                    objPropUser.PR,
                    objPropUser.ViolationPermission,
                    objPropUser.EstApproveProposal,
                    objPropUser.MassPayrollTicket
                    ));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUsersForRole(User objPropUser)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@status",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.Status
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@isCusIncluced",
                SqlDbType = SqlDbType.Int,
                Value = 0
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@RoleId",
                SqlDbType = SqlDbType.Int,
                Value = objPropUser.RoleID
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetUsersForRole", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRoleSearch(UserRole objPropUser, bool IsIncInactive)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetRolesSearch", objPropUser.SearchBy, objPropUser.SearchValue, IsIncInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataSet getFilterCategory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct type as cat from category order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetUsersEmailByTypeAndUserID(User objPropUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "type";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropUser.TypeID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "userID";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPropUser.UserID;



                para[2] = new SqlParameter();
                para[2].ParameterName = "@email";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = 0;
                para[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetUsersEmailByTypeAndUserID", para);
                return para[2].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getListEmailByListByTypeAndUserID(String connConfig, String lsStr)
        {
            return SqlHelper.ExecuteDataset(connConfig, "spGetListEmailByListByTypeAndUserID", lsStr);
        }
        public DataSet GetEmailFromListRoleID(String connConfig, String lsRole)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "lsRole";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = lsRole;
                return SqlHelper.ExecuteDataset(connConfig, CommandType.StoredProcedure, "spGetEmailFromListRoleID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Start-- API Changes : Juily:04/06/2020 --//
        public void UpdateForAPIIntegrationEnable(String connConfig, int ID, int Integration)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(connConfig, "spUpdateCoreAPIIntegration", ID, Integration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAPIIntegrationEnable(String connConfig)
        {
            try
            {
                return SqlHelper.ExecuteDataset(connConfig, CommandType.StoredProcedure, "spGetCoreAPIIntegration");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //End-- API Changes : Juily:04/06/2020 --//


        public DataSet GetEquipment(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            string str = "select distinct e.state, e.cat,e.category,e.Serial,e.Classification,e.manuf,e.price,e.last,e.since, e.Install,e.id,e.unit,e.type,e.fdesc,case e.status when 0 then 'Active' else 'Inactive' end as Status" +
                ", CASE isnull(e.shut_down,0) WHEN 0 THEN 'No' ELSE 'Yes' END shut_down" +
                ", e.ShutdownReason ,e.building,r.EN,B.Name As Company,r.name,l.id as locid,l.tag ,(l.address+', '+l.city+', '+l.state+', '+l.zip) as address" +
                ",(l.city+', '+l.state+', '+l.zip) as cityzipstate" +
                ", l.Loc,e.ID as unitid,tr.Name AS Salesperson,isnull(rt.Name  +  (select ( case   when tblwork.fdesc=rt.Name then '' else'-'+ tblwork.fdesc   end)   from tblwork where tblwork.id=rt.mech   ),'Unassigned')  AS RouteName FROM elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id left Outer join Branch B on r.EN = B.ID  LEFT JOIN  Terr tr  ON l.Terr = tr.ID  LEFT JOIN  Route rt  ON l.Route = rt.ID  ";
            if (objPropUser.EN == 1)
                str += " left Outer join tblUserCO UC on UC.CompanyID = B.ID ";
            str += "  WHERE e.id IS NOT NULL ";
            if (objPropUser.SearchBy != string.Empty)
            {
                if (objPropUser.SearchBy == "address")
                {
                    str += " and (l.address+', '+l.city+', '+l.state+', '+l.zip) like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "r.name" || objPropUser.SearchBy == "l.id" || objPropUser.SearchBy == "l.tag" || objPropUser.SearchBy == "l.state")
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "e.unit")
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "B.Name" && objPropUser.EN == 1)
                {
                    str += " and  UC.IsSel = 1  and r.EN = " + objPropUser.SearchValue + " and UC.UserID =" + objPropUser.UserID;
                }
                else if (objPropUser.SearchBy == "e.fdesc")
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "e.status")
                {
                    str += " and e.Status = " + objPropUser.SearchValue;
                }
                else if (objPropUser.SearchBy == "rt.Name")
                {
                    if (objPropUser.SearchValue != "")
                    {
                        str += " and l.Route in( " + objPropUser.SearchValue + ")";
                    }

                }
                else if (objPropUser.SearchBy == "tr.Name")
                {
                    if (objPropUser.SearchValue != "")
                    {
                        str += " and l.Terr = " + objPropUser.SearchValue;
                    }

                }
                else
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
            }
            if (!string.IsNullOrEmpty(objPropUser.InstallDate))
            {
                str += " and e.since='" + objPropUser.InstallDate + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.InstallDateString))
            {
                str += " and CONVERT(date,e.since) " + objPropUser.InstallDateString + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.ServiceDate))
            {
                str += " and e.last ='" + objPropUser.ServiceDate + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.ServiceDateString))
            {
                str += " and CONVERT(date,e.last) " + objPropUser.ServiceDateString + "'";
            }
            if (objPropUser.Manufacturer != string.Empty)
            {
                str += " and e.manuf like '" + objPropUser.Manufacturer + "%'";
            }
            if (!string.IsNullOrEmpty(objPropUser.Price))
            {
                str += " and e.price='" + objPropUser.Price + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.PriceString))
            {
                str += " and e.price " + objPropUser.PriceString + "'";
            }
            if (objPropUser.LocID != 0)
            {
                str += " and e.loc=" + objPropUser.LocID + "";
            }
            if (objPropUser.CustomerID != 0)
            {
                str += " and e.owner=" + objPropUser.CustomerID + "";
            }

            if (objPropUser.RoleID != 0)
                str += " and isnull(l.roleid,0)=" + objPropUser.RoleID;

            if (!string.IsNullOrEmpty(objPropUser.Category))
                str += " and e.category = '" + objPropUser.Category + "'";

            if (!string.IsNullOrEmpty(objPropUser.Type))
                str += " and e.type = '" + objPropUser.Type + "'";

            if (!string.IsNullOrEmpty(objPropUser.Classification))
                str += " and e.Classification = '" + objPropUser.Classification + "'";

            if (objPropUser.Status != -1)
                str += " and e.status = " + objPropUser.Status;

            if (objPropUser.EN == 1)
                str += " and  UC.IsSel = 1 and UC.UserID =" + objPropUser.UserID;

            if (!string.IsNullOrEmpty(objPropUser.building) && objPropUser.building != "All")
                str += " and e.building = '" + objPropUser.building + "'";

            if (IsSalesAsigned > 0)
            {
                str += "  and  ( l.Terr=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))) or  isnull(l.Terr2,0)=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))))";
            }

            if (objPropUser.IsAssignedProject == true)
            {
                str += " and l.Loc in (select loc from Job where ProjectManagerUserID=" + objPropUser.EmpId + ")";
            }


            str += " order by e.unit";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEquipment(GetEquipmentParam _GetEquipment, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            string str = "select distinct e.state, e.cat,e.category,e.Classification,e.manuf,e.price,e.last,e.since, e.Install,e.id,e.unit,e.type,e.fdesc,case e.status when 0 then 'Active' else 'Inactive' end as Status" +
                ", CASE isnull(e.shut_down,0) WHEN 0 THEN 'No' ELSE 'Yes' END shut_down" +
                ", e.ShutdownReason ,e.building,r.EN,B.Name As Company,r.name,l.id as locid,l.tag ,(l.address+', '+l.city+', '+l.state+', '+l.zip) as address" +
                ", l.Loc,e.ID as unitid FROM elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id left Outer join Branch B on r.EN = B.ID ";
            if (_GetEquipment.EN == 1)
                str += " left Outer join tblUserCO UC on UC.CompanyID = B.ID ";
            str += "  WHERE e.id IS NOT NULL ";
            if (_GetEquipment.SearchBy != string.Empty)
            {
                if (_GetEquipment.SearchBy == "address")
                {
                    str += " and (l.address+', '+l.city+', '+l.state+', '+l.zip) like '%" + _GetEquipment.SearchValue + "%'";
                }
                else if (_GetEquipment.SearchBy == "r.name" || _GetEquipment.SearchBy == "l.id" || _GetEquipment.SearchBy == "l.tag" || _GetEquipment.SearchBy == "l.state")
                {
                    str += " and " + _GetEquipment.SearchBy + " like '%" + _GetEquipment.SearchValue + "%'";
                }
                else if (_GetEquipment.SearchBy == "e.unit")
                {
                    str += " and " + _GetEquipment.SearchBy + " like '%" + _GetEquipment.SearchValue + "%'";
                }
                else if (_GetEquipment.SearchBy == "B.Name" && _GetEquipment.EN == 1)
                {
                    str += " and  UC.IsSel = 1  and r.EN = " + _GetEquipment.SearchValue + " and UC.UserID =" + _GetEquipment.UserID;
                }
                else if (_GetEquipment.SearchBy == "e.fdesc")
                {
                    str += " and " + _GetEquipment.SearchBy + " like '%" + _GetEquipment.SearchValue + "%'";
                }
                else if (_GetEquipment.SearchBy == "e.status")
                {
                    str += " and e.Status = " + _GetEquipment.SearchValue;
                }
                else
                {
                    str += " and " + _GetEquipment.SearchBy + " like '%" + _GetEquipment.SearchValue + "%'";
                }
            }
            if (!string.IsNullOrEmpty(_GetEquipment.InstallDate))
            {
                str += " and e.since='" + _GetEquipment.InstallDate + "'";
            }
            if (!string.IsNullOrEmpty(_GetEquipment.InstallDateString))
            {
                str += " and CONVERT(date,e.since) " + _GetEquipment.InstallDateString + "'";
            }
            if (!string.IsNullOrEmpty(_GetEquipment.ServiceDate))
            {
                str += " and e.last ='" + _GetEquipment.ServiceDate + "'";
            }
            if (!string.IsNullOrEmpty(_GetEquipment.ServiceDateString))
            {
                str += " and CONVERT(date,e.last) " + _GetEquipment.ServiceDateString + "'";
            }
            if (_GetEquipment.Manufacturer != string.Empty)
            {
                str += " and e.manuf like '" + _GetEquipment.Manufacturer + "%'";
            }
            if (!string.IsNullOrEmpty(_GetEquipment.Price))
            {
                str += " and e.price='" + _GetEquipment.Price + "'";
            }
            if (!string.IsNullOrEmpty(_GetEquipment.PriceString))
            {
                str += " and e.price " + _GetEquipment.PriceString + "'";
            }
            if (_GetEquipment.LocID != 0)
            {
                str += " and e.loc=" + _GetEquipment.LocID + "";
            }
            if (_GetEquipment.CustomerID != 0)
            {
                str += " and e.owner=" + _GetEquipment.CustomerID + "";
            }

            if (_GetEquipment.RoleID != 0)
                str += " and isnull(l.roleid,0)=" + _GetEquipment.RoleID;

            if (!string.IsNullOrEmpty(_GetEquipment.Category))
                str += " and e.category = '" + _GetEquipment.Category + "'";

            if (!string.IsNullOrEmpty(_GetEquipment.Type))
                str += " and e.type = '" + _GetEquipment.Type + "'";

            if (!string.IsNullOrEmpty(_GetEquipment.Classification))
                str += " and e.Classification = '" + _GetEquipment.Classification + "'";

            if (_GetEquipment.Status != -1)
                str += " and e.status = " + _GetEquipment.Status;

            if (_GetEquipment.EN == 1)
                str += " and  UC.IsSel = 1 and UC.UserID =" + _GetEquipment.UserID;

            if (!string.IsNullOrEmpty(_GetEquipment.building) && _GetEquipment.building != "All")
                str += " and e.building = '" + _GetEquipment.building + "'";

            if (IsSalesAsigned > 0)
            {
                str += "  and  ( l.Terr=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))) or  isnull(l.Terr2,0)=convert(nvarchar(10),(select id from  Terr where Name=(select fUser from  tblUser where id=" + @IsSalesAsigned + "))))";
            }

            if (_GetEquipment.IsAssignedProject == true)
            {
                str += " and l.Loc in (select loc from Job where ProjectManagerUserID=" + _GetEquipment.EmpId + ")";
            }


            str += " order by e.unit";

            try
            {
                return _GetEquipment.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllEquipHasTheSameTest(String conn, int Loc, int testType, int year, bool Chargeable, string Classification, int ElevID)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[6];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@ElevID";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = ElevID;
                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetAllEquipHasTheSameTest", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllEquipmentHaveSameTestCover(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetAllEquipHasTheSameTestCover", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetExistTestCoverInLocByTestType(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetExistTestCoverInLocByTestType", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllTestCoverInLocationWithClassification(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetAllTestCoverInLocationWithClassification", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllEquipmentCoverByTest(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetAllEquipCoverByTest", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllEquipmentHaveSameTestChargable(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetAllEquipHasTheSameTestChargable", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetExistTestInLocByTestTypeAndChargable(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetExistTestInLocByTestTypeAndChargable", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllTestInLocationByChargableAndClassification(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Loc";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = Loc;

                para[1] = new SqlParameter();
                para[1].ParameterName = "TestType";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testType;

                para[2] = new SqlParameter();
                para[2].ParameterName = "YearProposal";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = year;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Chargeable";
                para[3].SqlDbType = SqlDbType.Bit;
                para[3].Value = Chargeable;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@Classification";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = Classification;

                try
                {
                    return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetAllTestInLocationByChargableAndClassification", para);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDefaultTestPricingForEquipment(String connConfig, int elevId, int testTypeId, int priceYear = 0)
        {

            if (priceYear == 0) priceYear = DateTime.Now.Year;

            try
            {
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@ElevId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = elevId;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@TestTypeId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = testTypeId;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@PriceYear";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = priceYear;


                return SqlHelper.ExecuteDataset(connConfig, CommandType.StoredProcedure, "spGetDefaultTestPriceByYear", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserEmailSignature(User user)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@UserId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = user.ID;

                return SqlHelper.ExecuteDataset(user.ConnConfig, CommandType.StoredProcedure, "spGetUserEmailSignature", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEmailSignatureById(EmailSignature eSignature)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@Id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = eSignature.Id;

                return SqlHelper.ExecuteDataset(eSignature.ConnConfig, CommandType.StoredProcedure, "spGetEmailSignatureById", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEmailSignature(EmailSignature eSignature)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@Id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = eSignature.Id;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@UserId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = eSignature.UserId;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@Name";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = eSignature.Name;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Body";
                para[3].SqlDbType = SqlDbType.NVarChar;
                para[3].Value = eSignature.Body;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@IsDefault";
                para[4].SqlDbType = SqlDbType.Bit;
                para[4].Value = eSignature.IsDefault;

                SqlHelper.ExecuteNonQuery(eSignature.ConnConfig, CommandType.StoredProcedure, "spAddUpdateEmailSignature", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEmailSignature(EmailSignature eSignature)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@Id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = eSignature.Id;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@UserId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = eSignature.UserId;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@Name";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = eSignature.Name;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Body";
                para[3].SqlDbType = SqlDbType.NVarChar;
                para[3].Value = eSignature.Body;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@IsDefault";
                para[4].SqlDbType = SqlDbType.Bit;
                para[4].Value = eSignature.IsDefault;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(eSignature.ConnConfig, CommandType.StoredProcedure, "spAddUpdateEmailSignature", para).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEmailSignature(EmailSignature eSignature)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@Id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = eSignature.Id;

                SqlHelper.ExecuteNonQuery(eSignature.ConnConfig, CommandType.StoredProcedure, "spDeleteEmailSignature", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserEmailInfoByUserId(User objPropUser)
        {
            try
            {
                //return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(r.email,'') as email from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where u.EmailAccount = 1 and u.id='" + objPropUser.UserID + "'"));
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text,
                    "select isnull(r.email,'') as email, s.SignContent from tbluser u " +
                    "left outer join Emp e  on u.fUser=e.CallSign " +
                    "left outer join Rol r on e.Rol=r.ID " +
                    "left outer join tblEmailSignature s on s.UserId=u.ID and s.IsDefault = 1 " +
                    "where " +
                    //"u.EmailAccount = 1 " +
                    //"u.EmailAccount = 1 " +
                    //"and " +
                    "u.id='" + objPropUser.UserID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDefaultUserEmailSignature(User objPropUser)
        {
            try
            {
                //return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(r.email,'') as email from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where u.EmailAccount = 1 and u.id='" + objPropUser.UserID + "'"));
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text,
                    "select Top 1 s.SignContent from tblEmailSignature s " +
                    "where  s.UserId='" + objPropUser.UserID + "' and s.IsDefault = 1 "));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpportunitiesForProjectLinking(User objPropUser)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@JobId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropUser.JobId;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@SearchText";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = objPropUser.SearchValue;

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetListOppForProjectLinking", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateLoginLog(int UserId, string UserName, string Config, string URL, string IP, string MAC)
        {
            SqlParameter[] parameterCheck = {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@LoginIPAddress", IP),
                    new SqlParameter("@LoginMacAddress", MAC),
                    new SqlParameter("@LoginURL", URL),
                };

            try
            {
                SqlHelper.ExecuteNonQuery(Config, CommandType.StoredProcedure, "[dbo].[spCreateLoginLog]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateUserCustomFieldsValue(string conn, int userId, DataTable dtUserCustomItemValue)
        {
            int success = 0;
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateUserCustomFieldsValue";
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
            cmd.Parameters.Add("@UserCustomFieldValue", SqlDbType.Structured).Value = dtUserCustomItemValue;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return success;
        }


    }
}