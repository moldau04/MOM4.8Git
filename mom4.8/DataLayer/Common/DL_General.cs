using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using System.Collections.Generic;

namespace DataLayer
{
    public class DL_General
    {
        public void RegisterDevice(General objPropGeneral)
        {
            try
            {
                InsertColumnIfNotExists();
                SqlHelper.ExecuteNonQuery(Config.MS, "spDeviceRegistration", objPropGeneral.DeviceID, objPropGeneral.RegID, objPropGeneral.DeviceType, objPropGeneral.fuser, objPropGeneral.userid, objPropGeneral.database);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RegisterDeviceNew(General objPropGeneral)
        {
            try
            {
                InsertColumnIfNotExists();
                SqlHelper.ExecuteNonQuery(Config.MS, "spDeviceRegistrationNew", objPropGeneral.DeviceID, objPropGeneral.RegID, objPropGeneral.DeviceType, objPropGeneral.fuser, objPropGeneral.userid, objPropGeneral.database);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PingResponse(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "Sppingdevice", objPropGeneral.DeviceID, objPropGeneral.RegID, objPropGeneral.IsRunning, objPropGeneral.IsGPSEnabled, objPropGeneral.backgroundRefresh);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PingResponseNew(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "SppingdeviceNew", objPropGeneral.DeviceID, objPropGeneral.RegID, objPropGeneral.sentdate, objPropGeneral.IsRunning, objPropGeneral.IsGPSEnabled, objPropGeneral.backgroundRefresh, objPropGeneral.fuser, objPropGeneral.userid, objPropGeneral.database);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetRegID(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.RegID = Convert.ToString(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "select m.reg from [MSM2_Admin].dbo.tbldeviceregistration m inner join emp e on m.deviceId=e.deviceid where m.callsign=" + objPropGeneral.EmpID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetUpdateTicketSP(General objPropGeneral)
        {   //spUpdateTicket
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "SELECT 1 FROM sys.procedures where name='" + objPropGeneral.FunctionName + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetFunctionSpecialChars(General objPropGeneral)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "SELECT 1 AS function_name FROM sys.objects WHERE type_desc LIKE '%FUNCTION%' and name='" + objPropGeneral.FunctionName + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsetLatLngRole(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.Text, "IF NOT EXISTS(SELECT column_name 'Column_Name' FROM information_schema.columns WHERE table_name = 'rol' AND column_name = 'lng') BEGIN ALTER TABLE rol ADD lat VARCHAR(50) NULL, lng VARCHAR(50) NULL END");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePDAField(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.Text, "ALTER TABLE emp ALTER COLUMN PDASerialNumber varchar(100) null");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustom(General objPropGeneral)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("DECLARE @isExist bit = 0; ");
                stringBuilder.AppendLine("DECLARE @ID int = 0; ");
                stringBuilder.AppendLine("DECLARE @IsIdentity int = 0; ");
                stringBuilder.AppendFormat("SELECT @isExist = 1 FROM custom WHERE name ='{0}' ", objPropGeneral.CustomName);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("IF @isExist = 1 ");
                stringBuilder.AppendLine("BEGIN ");
                stringBuilder.AppendFormat(" UPDATE custom SET label ='{0}' WHERE name ='{1}' ", objPropGeneral.CustomLabel, objPropGeneral.CustomName);
                if (objPropGeneral.CustomName == "NextPO")
                {
                    stringBuilder.AppendFormat(" DBCC CHECKIDENT (PO, RESEED, " + (Convert.ToInt32(objPropGeneral.CustomLabel)-1) + ")");
                }
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("END ");
                stringBuilder.AppendLine("ELSE ");
                stringBuilder.AppendLine("BEGIN ");
                stringBuilder.AppendLine("SET @IsIdentity = COLUMNPROPERTY(object_id('custom'),'Id','IsIdentity') ");
                stringBuilder.AppendLine("IF @IsIdentity = 0 ");
                stringBuilder.AppendLine("BEGIN ");
                stringBuilder.AppendLine("SELECT @ID=MAX(ID)+1 FROM custom ");
                stringBuilder.AppendFormat("INSERT INTO custom (Label,Name,ID) VALUES ('{0}','{1}', @ID) ", objPropGeneral.CustomLabel, objPropGeneral.CustomName);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("END ");
                stringBuilder.AppendLine("ELSE ");
                stringBuilder.AppendLine("BEGIN ");
                stringBuilder.AppendFormat("INSERT INTO custom (Label,Name) VALUES ('{0}','{1}') ", objPropGeneral.CustomLabel, objPropGeneral.CustomName);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("END");
                stringBuilder.AppendLine("END");

                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.Text, stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomFields(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select *, case when (select Label from custom where name ='Country') = 1 	then Convert(numeric(30,2),(select Label As GstRate from custom where Name='GSTRate'))	else 0.00  end As GstRate from custom where name = '" + objPropGeneral.CustomName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetLocCredit(int LID , string ConnConfig)
        {
            try
            {
                return SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "IF ((SELECT COUNT(LoadTestItem.LID) cSkip FROM LoadTestItem INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc WHERE Loc.Credit=1 AND LoadTestItem.LID="+ LID + ")=0 ) 	BEGIN  	  SELECT 0	END	ELSE 	BEGIN	 SELECT 1 	END");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get All Category List 
        public DataSet getAllCategoryList(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select type from category where isnull(status,1)=1 order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get All Customer(Owner) List 
        public DataSet getAllCustomerList(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "Select Owner.ID as ID,Rol.Name as Name from Owner Inner Join Rol ON Owner.Rol=Rol.ID order by Rol.Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomFields(getCustomFieldsParam _getCustomFieldsParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select *, case when (select Label from custom where name ='Country') = 1 	then Convert(numeric(30,2),(select Label As GstRate from custom where Name='GSTRate'))	else 0.00  end As GstRate from custom where name = '" + _getCustomFieldsParam.CustomName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCode(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.CodeDesc = Convert.ToString(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "select text from codes where code = '" + objPropGeneral.Code + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCodesAll(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select code,text from codes");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getPing(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select isnull(isrunning,0) as isrunning,  isnull( IsGPSEnabled,0)  as IsGPSEnabled ,backgroundRefresh  from tblpingdevice where deviceID='" + objPropGeneral.DeviceID + "' and randomid='" + objPropGeneral.RegID + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getPingNew(General objPropGeneral)
        {
            try
            {

                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select isnull(isrunning,0) as isrunning,  isnull( IsGPSEnabled,0)  as IsGPSEnabled ,backgroundRefresh  from tblpingdevice where fuser='" + objPropGeneral.fuser + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getDiagnosticCategory(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select distinct Category from Diagnostic ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getDiagnosticCategory(GetDiagnosticCategoryParam _GetDiagnosticCategory, string ConnectionString)
        {
            try
            {
                return _GetDiagnosticCategory.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select distinct Category from Diagnostic ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDiagnostic(General objPropGeneral)
        {
            string query = "select fdesc, Category from Diagnostic where Type = " + objPropGeneral.CodeType + "";

            if (objPropGeneral.CodeCategory != "ALL")
            {
                query += "and Category='" + objPropGeneral.CodeCategory + "' ";
            }

            query += "order by Category,orderno, fdesc";

            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDiagnosticAll(General objPropGeneral)
        {
            string query = "  SELECT isnull(orderno,0) as orderno,*, CASE type WHEN  1 THEN 'Resolution' WHEN 0 THEN 'Reason' END AS typecode FROM Diagnostic d order by  category,d.orderno ,type";

            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertDiagnostic(General objGeneral)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "insert into Diagnostic(category,type,fdesc) values('" + objGeneral.Category + "','" + objGeneral.DiagnosticType + "','" + objGeneral.Remarks + "')");
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, "spAddDiagnostic", objGeneral.Remarks, objGeneral.Category, objGeneral.DiagnosticType, 0
                    , objGeneral.DiagnosticCategoryOld, objGeneral.DiagnosticTypeOld);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDiagnostic(General objGeneral)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update Diagnostic set category='" + objGeneral.Category + "' ,type='" + objGeneral.DiagnosticType + "' where fdesc='" + objGeneral.Remarks + "'");
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, "spAddDiagnostic", objGeneral.Remarks, objGeneral.Category, objGeneral.DiagnosticType, 1
                    , objGeneral.DiagnosticCategoryOld, objGeneral.DiagnosticTypeOld);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertQuickCodes(General objGeneral)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "insert into codes(code,text) values('" + objGeneral.Code + "','" + objGeneral.CodeDesc + "')");
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "IF NOT EXISTS(SELECT 1 FROM codes WHERE  code = '" + objGeneral.Code + "')  BEGIN insert into codes(code,text) values('" + objGeneral.Code + "','" + objGeneral.CodeDesc + "') End ELSE BEGIN RAISERROR ('Code already exists, please use different code !',16,1) RETURN END   ");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQuickCodes(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update codes set Text='" + objGeneral.CodeDesc + "'  where code='" + objGeneral.Code + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddUpdateQuickCode(General objGeneral, bool isUpdate)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter
                {
                    ParameterName = "@Code",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objGeneral.Code
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@CodeDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objGeneral.CodeDesc
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "@IsUpdate",
                    SqlDbType = SqlDbType.Bit,
                    Value = isUpdate
                };

                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.StoredProcedure, "spAddUpdateQuickCode", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteQuickCodes(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "delete from codes  where code='" + objGeneral.Code + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGPSInterval(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tblauth set GPSInterval=" + objGeneral.GPSInterval);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetGPSInterval(General objGeneral)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, "select gpsinterval from tblauth"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public string GetGPSIntervalSP(General objGeneral)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(Config.MS, CommandType.StoredProcedure, "spGetGPSInterval"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetDeviceTokenID(General objGeneral)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, "select tokenid from PushNotifications where deviceID='" + objGeneral.DeviceID + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetDeviceTokenbyuser(string username, string ConnConfig)
        {
            try
            {

                return Convert.ToString(SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "select TokenId from PushNotifications where fuser='" + username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPingResponse(string ConnConfig, string fuser, string Randomid)
        {
            try
            {

                return SqlHelper.ExecuteDataset(ConnConfig, CommandType.Text, "select isnull(isrunning,0) as isrunning,  isnull( IsGPSEnabled,0)  as IsGPSEnabled ,backgroundRefresh , (select top 1 DeviceType from  PushNotifications where FUser='" + fuser + "') DeviceType  from tblpingdevice where fuser='" + fuser + "' and  randomID='" + Randomid + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDeletedTickets(string ConnConfig)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnConfig, CommandType.Text, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetDeviceType(General objGeneral)
        {
            try
            {
                InsertColumnIfNotExists();
                return Convert.ToString(SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, "select DeviceType from PushNotifications where deviceID='" + objGeneral.DeviceID + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check Column is exists for Ios Ping Feature
        /// </summary>
        public void InsertColumnIfNotExists()
        {
            try
            {
                /// DeviceType
                SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, @"IF  NOT EXISTS(SELECT *
           FROM   INFORMATION_SCHEMA.COLUMNS
           WHERE  TABLE_NAME = 'PushNotifications'
           AND COLUMN_NAME = 'DeviceType') 
           begin 
           Alter table MSM2_Admin.dbo.PushNotifications add[DeviceType][varchar](100) NULL CONSTRAINT[DF_PushNotifications_DeviceType]  DEFAULT('Android') 
           end");

                SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, @"IF  NOT EXISTS(SELECT *
           FROM   INFORMATION_SCHEMA.COLUMNS
           WHERE  TABLE_NAME = 'PushNotifications'
           AND COLUMN_NAME = 'fuser') 
           begin 
           Alter table MSM2_Admin.dbo.PushNotifications add[fuser][varchar](50) NULL  
           end");

                SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, @"IF  NOT EXISTS(SELECT *
           FROM   INFORMATION_SCHEMA.COLUMNS
           WHERE  TABLE_NAME = 'PushNotifications'
           AND COLUMN_NAME = 'userId') 
           begin 
           Alter table MSM2_Admin.dbo.PushNotifications add[userId][int]  
           end");

                /// BackgroundRefresh
                SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, @"IF  NOT EXISTS(SELECT *
           FROM   INFORMATION_SCHEMA.COLUMNS
           WHERE  TABLE_NAME = 'tblPingDevice'
           AND COLUMN_NAME = 'BackgroundRefresh') 
           begin 
           Alter table MSM2_Admin.dbo.tblPingDevice add[BackgroundRefresh]int NULL 
           end");

                /// fake

                SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, @"IF  NOT EXISTS(SELECT *
           FROM   INFORMATION_SCHEMA.COLUMNS
           WHERE  TABLE_NAME = 'MapData'
           AND COLUMN_NAME = 'fake') 
           begin 
           Alter table MSM2_Admin.dbo.MapData add[fake]int NULL 
           end");

                /// Accuracy

                SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, @"IF  NOT EXISTS(SELECT *
           FROM   INFORMATION_SCHEMA.COLUMNS
           WHERE  TABLE_NAME = 'MapData'
           AND COLUMN_NAME = 'Accuracy') 
           begin 
           Alter table MSM2_Admin.dbo.MapData add[Accuracy] VARCHAR (50) NULL 
           end");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void LogError(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "insert into tblServiceErrorLog (ServiceName,Error)values('" + objGeneral.ServiceName + "','" + objGeneral.Error + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBLastSync(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update Control set QBLastSync=GETDATE()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateQBLastSync(UpdateQBLastSyncParam _UpdateQBLastSync, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, "update Control set QBLastSync=GETDATE()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBLastSync1(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, string.Format("update Control set QBLastSync='{0}'", objGeneral.date));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSageLastSync(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update Control set SageLastSync=GETDATE()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBErrorLog(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, "spAddQBErrorLog", objGeneral.QBapi, objGeneral.QBRequestID, objGeneral.QBStatusCode, objGeneral.QBStatusSeverity, objGeneral.QBStatusMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBlatsync(General objPropGeneral)
        {
            string query = "select isnull(QBLastSync,'')QBLastSync ,isnull(qbintegration,0)qbintegration  from Control";

            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public List<QBLastSyncResParam> GetQBlatSync(General objGeneral)
        {
            List<QBLastSyncResParam> lst = new List<QBLastSyncResParam>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.StoredProcedure, "spGetQBlatSync");
                QBLastSyncResParam obj = new QBLastSyncResParam();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lst.Add(new QBLastSyncResParam()
                    {
                        QbIntegration = Convert.ToInt32(dr["QbIntegration"].ToString()),
                        QBLastSync = Convert.ToDateTime(dr["QBLastSync"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }

        public DataSet getSagelatsync(General objPropGeneral)
        {
            string query = "select isnull(SageLastSync,'')SageLastSync ,isnull(sageintegration,0)sageintegration  from Control";

            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet getSagelatsync(getConnectionConfigParam objPropGeneral, string ConnectionString)
        {
            string query = "select isnull(SageLastSync,'')SageLastSync ,isnull(sageintegration,0)sageintegration  from Control";

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetMails(General objPropGeneral)
        {
            string strQuery = "select * from tblemail where ID is not null";
            if (objPropGeneral.type != -1 && objPropGeneral.type != -2)
            {
                strQuery += " and isnull(type,0)= " + objPropGeneral.type;
            }
            if (objPropGeneral.rol != 0)
            {
                ////strQuery += " and isnull(rol,0)= " + objPropGeneral.rol;
                strQuery += " and ID in (select Email From tblEmailRol where Rol = " + objPropGeneral.rol + ")";
            }
            if (!string.IsNullOrEmpty(objPropGeneral.RegID))
            {
                strQuery += " and CHARINDEX('" + objPropGeneral.RegID + "', [Subject] ) > 0";
            }
            ////if (objPropGeneral.userid != 0)
            ////{
            ////    strQuery += " and isnull([user],0)= " + objPropGeneral.userid;
            ////}


            if (!string.IsNullOrEmpty(objPropGeneral.OrderBy))
            {
                strQuery += " order by " + objPropGeneral.OrderBy;
            }
            else
            {
                strQuery += " order by recdate desc";
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMailsCount(General objPropGeneral)
        {
            string strQuery = "select count(1) from tblemail where ID is not null";
            if (objPropGeneral.type != -1 && objPropGeneral.type != -2)
            {
                strQuery += " and isnull(type,0)= " + objPropGeneral.type;
            }
            if (objPropGeneral.rol != 0)
            {
                strQuery += " and ID in (select Email From tblEmailRol where Rol = " + objPropGeneral.rol + ")";
            }
            if (!string.IsNullOrEmpty(objPropGeneral.RegID))
            {
                strQuery += " and CHARINDEX('" + objPropGeneral.RegID + "', [Subject] ) > 0";
            }
            ////if (objPropGeneral.userid != 0)
            ////{
            ////    strQuery += " and isnull([user],0)= " + objPropGeneral.userid;
            ////}

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, strQuery));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEmails(General objGeneral)
        {
            try
            {
                return Convert.ToInt16(SqlHelper.ExecuteScalar(objGeneral.ConnConfig, "spADDEmail", objGeneral.From, objGeneral.to, objGeneral.cc, objGeneral.bcc, objGeneral.subject, objGeneral.sentdate, objGeneral.Attachments, objGeneral.msgid, objGeneral.uid, objGeneral.GUID, objGeneral.type, objGeneral.userid, objGeneral.AccountID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEmailAcc(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, "select ea.* from tblEmailAccounts ea inner join tbluser u on u.id=ea.userid where u.status=0 and u.emailaccount=1 and u.id=" + objGeneral.userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetEmailAcc(General objGeneral)
        //{
        //    try
        //    {
        //        return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, "select * from tblEmailAccounts where UserId=" + objGeneral.userid);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetEmailAccounts(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, "select ea.* from tblEmailAccounts ea inner join tbluser u on u.id=ea.userid where u.status=0 and u.emailaccount=1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMAXEmailUID(General objGeneral)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objGeneral.ConnConfig, CommandType.Text, "select isnull(MAX(uid),0) from tblEmail where AccountID='" + objGeneral.AccountID + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMsgUID(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, "select UID from tblemail where accountid = '" + objGeneral.AccountID + "' and type = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCRMEmails(General objGeneral)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT Rtrim(Ltrim(EMail)) AS email \n");
            varname1.Append("FROM   Rol \n");
            varname1.Append("WHERE  Type IN ( 0, 3, 4 ) \n");
            varname1.Append("       AND Isnull(Rtrim(Ltrim(EMail)), '') <> '' \n");
            varname1.Append("       AND email LIKE '%_@__%.__%' \n");
            varname1.Append("UNION \n");
            varname1.Append("SELECT DISTINCT Rtrim(Ltrim(EMail)) AS email \n");
            varname1.Append("FROM   Phone \n");
            varname1.Append("WHERE  Rol IN (SELECT DISTINCT Rol \n");
            varname1.Append("               FROM   Rol \n");
            varname1.Append("               WHERE  Type IN ( 0, 3, 4 )) \n");
            varname1.Append("       AND Isnull(Rtrim(Ltrim(EMail)), '') <> '' \n");
            varname1.Append("       AND email LIKE '%_@__%.__%' \n");
            varname1.Append("UNION \n");
            varname1.Append("SELECT DISTINCT Rtrim(Ltrim(EMail)) AS email \n");
            varname1.Append("FROM   tblEmailAddresses \n");
            varname1.Append("WHERE  email LIKE '%_@__%.__%' ");


            try
            {
                return objGeneral.Ds = SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, varname1.ToString()); //"select distinct EMail from Rol where Type in (0,3,4)  and ISNULL(EMail,'')<> ''"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ExecQuery(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, objGeneral.TextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomFieldsControl(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select * from custom where Name='GSTRate' or Name ='GSTGL' or Name='Country' or Name='InvGL' or Name='DefaultInvGLAcct' or  Name='SalesTax2' or Name='UseTax' or Name='Zone' or Name='NextInv' or Name='NextPO'  or Name='NextEst' or Name='StateID' or Name = 'FederalID' or Name = 'ProvincialID' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomField(General objPropGeneral, string fieldName)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select distinct * from custom where Name='" + fieldName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomField(getCustomFieldParam _getCustomFieldParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select distinct * from custom where Name='" + _getCustomFieldParam.fieldName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomFieldsControl(getCustomFieldsControlParam _getCustomFieldsControlParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from custom where Name='GSTRate' or Name ='GSTGL' or Name='Country' or Name='InvGL' or Name='DefaultInvGLAcct' or  Name='SalesTax2' or Name='UseTax' or Name='Zone' or Name='NextInv' or Name='NextPO'  or Name='NextEst' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getInvDefaultAcct(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select Chart.ID, chart.Acct+' - '+Chart.fDesc as Acct from custom inner join chart on chart.ID=custom.label where Name='DefaultInvGLAcct'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getInvDefaultAcct(GetInvDefaultAcctParam ___GetInvDefaultAcctParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select Chart.ID, chart.Acct+' - '+Chart.fDesc as Acct from custom inner join chart on chart.ID=custom.label where Name='DefaultInvGLAcct'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInventoryByTypeName(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.StoredProcedure, "SpGetInventoryByTypeName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomFieldsControlBranch(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select * from custom where Name='MultiOffice' or Name='Branch'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void UpdateDiagnosticOrder(General objPropGeneral)
        {
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter
            {
                ParameterName = "@Items",
                SqlDbType = SqlDbType.Structured,
                Value = objPropGeneral.dtDiagnostic
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.StoredProcedure, "spOrderDiagnostic", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCompanyCountry(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select top 1 case Label when '1' then 'Canada' when 0 then 'United States' when '2' then 'United Kingdom' end as Country from custom where Name='Country'  ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomFields(General objPropGeneral)
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter
            {
                ParameterName = "@Screen",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropGeneral.Screen
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@CustomItems",
                SqlDbType = SqlDbType.Structured,
                Value = objPropGeneral.CustomItems
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@CustomItemsValue",
                SqlDbType = SqlDbType.Structured,
                Value = objPropGeneral.CustomItemsValue
            };

            para[3] = new SqlParameter
            {
                ParameterName = "@CustomItemsDelete",
                SqlDbType = SqlDbType.Structured,
                Value = objPropGeneral.CustomItemsDelete
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomFields", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetScreenCustomFields(General objPropGeneral)
        {
            //var strQuery = string.Format("SELECT * FROM tblCommonCustomFields WHERE ISNULL(IsDeleted, 0) = 0 AND Screen = '{0}'", objProp_Customer.Screen);
            var para = new SqlParameter[2];
            para[0] = new SqlParameter
            {
                ParameterName = "@Screen",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropGeneral.Screen
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@RefID",
                SqlDbType = SqlDbType.Int,
                Value = objPropGeneral.ScreenRefID
            };
            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.StoredProcedure, "spGetScreenCustomFields", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetStageItemsById(General objPropGeneral)
        {
            //var strQuery = string.Format("SELECT * FROM tblCommonCustomFields WHERE ISNULL(IsDeleted, 0) = 0 AND Screen = '{0}'", objProp_Customer.Screen);
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@RefID",
                SqlDbType = SqlDbType.Int,
                Value = objPropGeneral.ScreenRefID
            };
            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.StoredProcedure, "spGetStageItemsById", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomPRLast(string connConfig,string startDate, string endDate)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = startDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = endDate
                };
                try
                {
                     SqlHelper.ExecuteNonQuery(connConfig, CommandType.StoredProcedure, "spUpdatePayrollDate", para);
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

        public void UpdateSalesApproveEstimate(string connConfig, bool isApprove)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@isApprove",
                    SqlDbType = SqlDbType.Bit,
                    Value = isApprove
                };
                SqlHelper.ExecuteNonQuery(connConfig, CommandType.Text, "UPDATE Control SET SalesApproveEstimate = @isApprove", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetSalesApproveEstimate(string connConfig)
        {
            try
            {
                bool isApprove = (bool)SqlHelper.ExecuteScalar(connConfig, CommandType.Text, "SELECT TOP 1 ISNULL(SalesApproveEstimate, 0) SalesApproveEstimate FROM Control");
                return isApprove;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetEmailTemplate(EmailTemplate emailTemplate)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Screen",
                    SqlDbType = SqlDbType.VarChar,
                    Value = emailTemplate.Screen
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@FunctionName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = emailTemplate.FunctionName
                };
                String templateStr = (string)SqlHelper.ExecuteScalar(emailTemplate.ConnConfig, CommandType.StoredProcedure, "spGetEmailTemplate", para);
                return templateStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
