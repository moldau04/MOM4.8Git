using MOMWebAPPCoreReport.Models;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using BusinessEntity;
using MapData = BusinessEntity.MapData;
using BusinessLayer;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace MOMWebAPPCoreReport.ReportsBL
{
    public class Auth
    { 
        MapData objMapData = new MapData();
        BL_User objBL_User = new BL_User();
        public string Decrypt(string strEncrypted)
        {
            try
            {
                string strKey = "core";
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = strKey;

                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

                byteBuff = Convert.FromBase64String(strEncrypted);
                string strDecrypted = ASCIIEncoding.ASCII.GetString(objDESCrypto.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;

                return strDecrypted;
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }
        public bool Checkuser(ReqParams usr)
        
        {
            bool result = false;
            int reslt = 0;
            try
            {

                using (SqlConnection con = new SqlConnection(usr.Constring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Core_UserToken", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Token", SqlDbType.VarChar).Value = usr.Token;
                    cmd.Parameters.Add("@Domain_Name", SqlDbType.VarChar).Value = usr.Domain_Name;
                    cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = usr.User_Id;
                    cmd.Parameters.Add("@company", SqlDbType.VarChar).Value = usr.company;
                    reslt = (int)cmd.ExecuteScalar();
                    if (reslt == 2020)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        //Schedule_TimerECAPReport//
        public DataSet GetReportData(ReqParams Rpara)
        {
            MapData objPropMapData = new MapData();
            objPropMapData.ConnConfig = Rpara.Constring;

            if (Rpara.Screen == "ticketList" && Rpara.ticketFilter != null)
            {
                // objPropMapData = (MapData)Rpara.ticketFilter.ToString();
            }

            int IsSalesAsigned = 0;// new GeneralFunctions().GetSalesAsigned();

          

            string stdate = Rpara.startdate + " 00:00:00";
            string enddate = Rpara.EndDate + " 23:59:59";
            DateTime EndDate = DateTime.ParseExact(enddate, "MM/dd/yyyy HH:mm:ss", null);
            DateTime StartDate = DateTime.ParseExact(stdate, "MM/dd/yyyy hh:mm:ss", null);
            string strD = string.Empty;

            #region FILTER FOR TICKETD

            strD += " AND t.EDate >='" + StartDate.ToString("MM/dd/yyyy") + "'";
            strD += " AND t.EDate <='" + EndDate.ToString("MM/dd/yyyy") + "'";

            objPropMapData.UserID = Convert.ToInt32(Rpara.User_Id);

            if (Rpara.Defalutcmp == "1")
            {
                objMapData.EN = 1;
            }
            else
            {
                objMapData.EN = 0;
            }

            if (objPropMapData.EN == 1) // check for company
            {
                strD += " AND UC.IsSel = 1 AND UC.UserID =" + objPropMapData.UserID;
            }
            if (Rpara.ddlDeprt!= "-1")
            {
                strD += " AND t.Type=" + objPropMapData.Department;
            }
            if (IsSalesAsigned > 0)
            {
                strD += " AND (l.Terr=(" + " SELECT  ID FROM Terr WHERE Name = ( SELECT  fUser FROM  tblUser WHERE ID=" + IsSalesAsigned + ")) or ISNULL(l.Terr2, 0)=(" + " SELECT  ID FROM Terr WHERE Name = ( SELECT  fUser FROM  tblUser WHERE ID =" + IsSalesAsigned + ")))";
            }

            if (Rpara.Screen == "ticketList")
            {
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {
                    if (objPropMapData.Worker == "Active")
                    {
                        strD += " AND t.fwork in ( SELECT  w.ID FROM tblWork w WHERE w.Status = 0 )";
                    }
                    else if (objPropMapData.Worker == "Inactive")
                    {
                        strD += " AND t.fwork in ( SELECT  w.ID FROM tblWork w WHERE w.Status = 1 )";
                    }
                    else
                    {
                        strD += " AND ( SELECT  w.fdesc FROM tblWork w WHERE w.ID = t.fWork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                    }
                }
                if (objPropMapData.LocID != 0)
                {
                    strD += " AND l.Loc=" + objPropMapData.LocID;
                }
                if (objPropMapData.CustID != 0)
                {
                    strD += " AND l.Owner=" + objPropMapData.CustID;
                }
                if (objPropMapData.jobid != 0)
                {
                    strD += " AND t.Job =" + objPropMapData.jobid;
                }
                if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                {
                    strD += " AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                    if (objPropMapData.FilterCharge == "1")
                    {
                        strD += " or ISNULL(Invoice,0) <> 0)";
                    }
                    else
                    {
                        strD += " AND ISNULL(Invoice,0) = 0)";
                    }
                }
                if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                {
                    strD += " AND ( SELECT  Super FROM tblWork w WHERE w.ID = t.fwork ) = '" + objPropMapData.Supervisor + "'";
                }
                if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                {
                    strD += " AND ( SELECT  Super FROM tblWork w WHERE w.ID = t.fwork ) <> '" + objPropMapData.NonSuper + "'";
                }
                if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                {
                    strD += " AND ISNULL( ClearCheck, 0) =" + Convert.ToInt32(objPropMapData.FilterReview);
                }
                if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                {
                    strD += " AND t.Workorder = '" + objPropMapData.Workorder + "'";
                }
                if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                {
                    strD += " AND t.Cat IN (" + objPropMapData.Category + ")";
                }
            }

            #endregion FILTER FOR TICKETD

            #region FILTER FOR TICKETO

            string strO = string.Empty;
            strO += " AND DPDA.EDate >='" + StartDate.ToString("MM/dd/yyyy") + "'";
            strO += " AND DPDA.EDate <='" + EndDate.ToString("MM/dd/yyyy") + "'";

            objPropMapData.UserID = Convert.ToInt32(Rpara.User_Id);

            if (Rpara.Defalutcmp == "1")
            {
                objMapData.EN = 1;
            }
            else
            {
                objMapData.EN = 0;
            }

            if (objPropMapData.EN == 1) // check for company
            {
                strO += " AND UC.IsSel = 1 AND UC.UserID =" + objPropMapData.UserID;
            }
            if (Rpara.ddlDeprt != "-1")
            {
                strO += " AND DPDA.Type = " + objPropMapData.Department;
            }
            if (IsSalesAsigned > 0)
            {
                strO += " AND  (l.Terr = (" + "SELECT  ID FROM  Terr WHERE Name=(SELECT  fUser FROM tblUser WHERE ID = " + IsSalesAsigned + ")) OR ISNULL(l.Terr2,0) = (" + "SELECT  ID FROM Terr WHERE Name = (SELECT  fUser FROM  tblUser WHERE ID = " + IsSalesAsigned + ")))";
            }
            if (Rpara.Screen== "ticketList")
            {
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {

                    if (objPropMapData.Worker == "Active")
                    {
                        strO += " AND t.fWork IN (SELECT w.ID FROM tblWork w WHERE  w.Status = 0 )";
                    }
                    else if (objPropMapData.Worker == "Inactive")
                    {
                        strO += " AND t.fWork IN (SELECT w.ID FROM tblWork w WHERE w.Status = 1 )";
                    }
                    else
                    {
                        strO += " AND (SELECT w.fDesc FROM tblWork w WHERE w.ID = t.fWork )= '" + objPropMapData.Worker.Replace("'", "''") + "'";
                    }
                }
                if (objPropMapData.LocID != 0)
                {
                    strO += " AND l.Loc = " + objPropMapData.LocID;
                }
                if (objPropMapData.CustID != 0)
                {
                    strO += " AND l.Owner = " + objPropMapData.CustID;
                }
                if (objPropMapData.jobid != 0)
                {
                    strO += " AND DPDA.job = " + objPropMapData.jobid;
                }
                if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                {
                    strO += " AND (ISNULL(DPDA.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                    if (objPropMapData.FilterCharge == "1")
                    {
                        strO += " or ISNULL(Invoice,0) <> 0)";
                    }
                    else
                    {
                        strO += " AND ISNULL(Invoice,0) = 0)";
                    }
                }
                if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                {
                    strO += " AND (SELECT Super FROM tblWork w WHERE w.ID = t.fWork ) ='" + objPropMapData.Supervisor + "'";
                }
                if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                {
                    strO += " AND (SELECT Super FROM tblWork w WHERE w.ID = t.fWork ) <>'" + objPropMapData.NonSuper + "'";
                }
                if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                {
                    strO += " AND ISNULL(ClearCheck, 0) = " + Convert.ToInt32(objPropMapData.FilterReview);
                }
                if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                {
                    strO += " AND t.Workorder = '" + objPropMapData.Workorder + "'";
                }
                if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                {

                    strO += " AND t.Cat IN (" + objPropMapData.Category + ")";
                }
            }

            #endregion FILTER FOR TICKETO

            try
            {
                #region  QUERY  
                string query =
                    @"SELECT
                    e.Ref AS EmpID,
                    (e.Last + ', ' + e.fFirst) AS EmpName, 
                    tbl.fDesc AS CallSign,
                    CASE e.Field WHEN 1 THEN 'Field' ELSE 'Office' END AS Type, 
                    tbl.*
			    FROM
                (
                SELECT  
                    SUM(ISNULL(Reg,0)) AS RT, 
                    SUM(ISNULL(OT,0)) AS OT , 
                    SUM(ISNULL(DT,0)) AS DT, 
                    SUM(ISNULL(TT,0)) AS TT, 
                    SUM(ISNULL(NT,0)) AS NT, 
                    SUM(ISNULL(Total,0)) AS Total, 
                    SUM(ISNULL(Zone,0)) AS Zone, 
                    SUM(ISNULL(Toll,0)) AS Toll, 
                    SUM(ISNULL(OtherE,0)) AS Misc,
                    fDesc
	            FROM 
	            (
		            SELECT  
                        w.fDesc, 
		                Reg, 
                        OT, 
                        DT, 
                        TT, 
                        NT, 
                        Total, 
                        t.Zone, 
                        t.Toll, 
                        t.OtherE  
		            FROM tblWork w
		            LEFT OUTER JOIN TicketD t ON w.ID = t.fWork 
                    INNER JOIN Loc l ON l.Loc = t.Loc
                    INNER JOIN Owner o ON l.Owner = o.ID 
                    INNER JOIN Rol r ON r.ID = l.Rol 
                    LEFT OUTER JOIN Branch B ON B.ID = r.EN
                    LEFT OUTER JOIN Elev e ON e.ID = t.Elev";

                // Check for company
                if (objPropMapData.EN == 1)
                {
                    strD += " LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN ";
                }

                query += " WHERE 1 = 1 ";

                // ES-5179: Report exclude Location "Survey MR" just for TEI
                //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Transel"))
                //{
                    query += " AND l.Tag NOT LIKE 'Survey MR'";
                ///}

                query += strD;

                if (Rpara.Screen == "ticketList")
                {
                    query +=
                        @" UNION ALL
		            SELECT  
                        ISNULL((SELECT w.fdesc FROM tblWork w WITH(NOLOCK)WHERE t.fWork = w.id),'') AS fDesc,  
                        DPDA.Reg, 
                        DPDA.OT, 
                        DPDA.DT, 
                        DPDA.TT, 
                        DPDA.NT, 
                        DPDA.Total, 
                        DPDA.Zone, 
                        DPDA.Toll, 
                        OtherE  
		            FROM TicketO t
		            LEFT OUTER JOIN TicketDPDA DPDA ON t.ID = DPDA.ID  
                    INNER JOIN Loc l ON l.Loc = DPDA.Loc  
                    INNER JOIN Owner o ON l.Owner = o.ID 
                    INNER JOIN Rol r ON r.ID = l.Rol 
                    LEFT OUTER JOIN Branch B ON B.ID = r.EN  ";

                    // Check for company
                    if (objPropMapData.EN == 1)
                    {
                        strO += " LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN ";
                    }

                    query += " WHERE  1=1 AND t.id IS NOT NULL AND t.Owner IS NOT NULL";

                    // ES-5179: Report exclude Location "Survey MR" just for TEI
                    if (Rpara.AppValue=="Transel")
                    {
                        query += " AND l.Tag NOT LIKE 'Survey MR'";
                    }

                    query += strO;
                }

                query += @" 
	            ) AS tickets	 
                LEFT JOIN Emp AS e ON fdesc = e.CallSign 
                GROUP BY fDesc
                ) tbl
                LEFT JOIN Emp e ON tbl.fdesc = e.CallSign AND e.Status = 0";

                #endregion

                return objPropMapData.Ds = DataOps.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, query);
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
                return objPropUser.DsUserAuthorization = DataOps.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select c.*,isnull(JobCostLabor,0) as JobCostLabor1, isnull(msemail,0) as msemailnull, isnull(QBFirstSync,1) as EmpSync, isnull(msrep,0) as msreptemp, isnull(tinternet,0) as tinternett, businessstart, businessend,isnull(MSIsTaskCodesRequired,0) as TaskCode , c.YE As Year,ISNULL(IsUseTaxAPBill,0) as IsUseTaxAPBills,ISNULL(IsSalesTaxAPBill,0) as IsSalesTaxAPBills, TargetHPermission, r.Email as PwResetAdminEmail, u.fUser as PwResetUsername  from control  c left join tblUser u on u.ID = c.PwResetUserID left join emp e on e.CallSign = u.fUser left join Rol r on r.id = e.Rol");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}



