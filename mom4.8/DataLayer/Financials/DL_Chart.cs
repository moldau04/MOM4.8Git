using BusinessEntity;
using BusinessEntity.APModels;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace DataLayer
{
    public class DL_Chart
    {
        public void AddChart(Chart objChart, Bank objBank)
        {
            try
            {
                var para = new SqlParameter[62];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Acct",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objChart.Acct
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDesc",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objChart.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@AcType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.AcType
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@Sub",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objChart.Sub
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Sub2",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objChart.Sub2
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Remarks
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Status
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@City",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.City
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@State",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.State
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@Zip",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Zip
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@Phone",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Phone
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "@Fax",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Fax
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "@Contact",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Contact
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "@Address",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Address
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.EMail
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@Website",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Website
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "@Country",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Country
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "@Cellular",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Cellular
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Type
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "@GeoLock",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.GeoLock
                };
                if (objChart.Since != DateTime.MinValue)
                {
                    para[20] = new SqlParameter
                    {
                        ParameterName = "@Since",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Since
                    };
                }
                if (objChart.Last != DateTime.MinValue)
                {
                    para[21] = new SqlParameter
                    {
                        ParameterName = "@Last",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Last
                    };
                }
                para[22] = new SqlParameter
                {
                    ParameterName = "@NBranch",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NBranch
                };
                para[23] = new SqlParameter
                {
                    ParameterName = "@NAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NAcct
                };
                para[24] = new SqlParameter
                {
                    ParameterName = "@NRoute",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NRoute
                };
                para[25] = new SqlParameter
                {
                    ParameterName = "@NextC",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextC
                };
                para[26] = new SqlParameter
                {
                    ParameterName = "@NextD",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextD
                };
                para[27] = new SqlParameter
                {
                    ParameterName = "@NextE",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextE
                };
                para[28] = new SqlParameter
                {
                    ParameterName = "@Rate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Rate
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@CLimit",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.CLimit
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@Warn",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Warn
                };
                para[31] = new SqlParameter
                {
                    ParameterName = "@Recon",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Recon
                };
                para[32] = new SqlParameter
                {
                    ParameterName = "@Department",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.Department
                };
                para[33] = new SqlParameter
                {
                    ParameterName = "@BankName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.BankName
                };

                para[34] = new SqlParameter
                {
                    ParameterName = "@Lat",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Lat
                };

                para[35] = new SqlParameter
                {
                    ParameterName = "@Long",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Long
                };

                para[36] = new SqlParameter
                {
                    ParameterName = "@EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.EN
                };
                para[37] = new SqlParameter
                {
                    ParameterName = "@NoJE",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.NoJE
                };
                para[38] = new SqlParameter
                {
                    ParameterName = "@ACHFileHeaderStringA",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileHeaderStringA
                };
                para[39] = new SqlParameter
                {
                    ParameterName = "@ACHFileHeaderStringB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileHeaderStringB
                };
                para[40] = new SqlParameter
                {
                    ParameterName = "@ACHFileHeaderStringC",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileHeaderStringC
                };
                para[41] = new SqlParameter
                {
                    ParameterName = "@ACHCompanyHeaderString1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHCompanyHeaderString1
                };
                para[42] = new SqlParameter
                {
                    ParameterName = "@ACHCompanyHeaderString2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHCompanyHeaderString2
                };
                para[43] = new SqlParameter
                {
                    ParameterName = "@ACHBatchControlString1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHBatchControlString1
                };
                para[44] = new SqlParameter
                {
                    ParameterName = "@ACHBatchControlString2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHBatchControlString2
                };
                para[45] = new SqlParameter
                {
                    ParameterName = "@ACHBatchControlString3",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHBatchControlString3
                };
                para[46] = new SqlParameter
                {
                    ParameterName = "@ACHFileControlString1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileControlString1
                };
                para[47] = new SqlParameter
                {
                    ParameterName = "@APACHCompanyID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.APACHCompanyID
                };
                para[48] = new SqlParameter
                {
                    ParameterName = "@APImmediateOrigin",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.APImmediateOrigin
                };
                para[49] = new SqlParameter
                {
                    ParameterName = "@NextACH",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.NextACH
                };

                para[50] = new SqlParameter
                {
                    ParameterName = "@TraceNo1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TraceNo1
                };
                para[51] = new SqlParameter
                {
                    ParameterName = "@TraceNo2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TraceNo2
                };
                para[52] = new SqlParameter
                {
                    ParameterName = "@RecordTypeCode1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.RecordTypeCode1
                };
                para[53] = new SqlParameter
                {
                    ParameterName = "@RecordTypeCode2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.RecordTypeCode2
                };
                para[54] = new SqlParameter
                {
                    ParameterName = "@TransactionCode1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TransactionCode1
                };
                para[55] = new SqlParameter
                {
                    ParameterName = "@TransactionCode2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TransactionCode2
                };
                para[56] = new SqlParameter
                {
                    ParameterName = "@EndRecordIndicator1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.EndRecordIndicator1
                };
                para[57] = new SqlParameter
                {
                    ParameterName = "@EndRecordIndicator2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.EndRecordIndicator2
                };
                para[58] = new SqlParameter
                {
                    ParameterName = "@OriginatorStatusCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.OriginatorStatusCode
                };
                para[59] = new SqlParameter
                {
                    ParameterName = "@RecordTypeCode3",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.RecordTypeCode3
                };
                para[60] = new SqlParameter
                {
                    ParameterName = "@BatchNumber",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.BatchNumber
                };
                para[61] = new SqlParameter
                {
                    ParameterName = "@JulianDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.JulianDate
                };
                //para[32] = new SqlParameter
                //{
                //    ParameterName = "returnval",
                //    SqlDbType = SqlDbType.Int,
                //    Value = ParameterDirection.ReturnValue
                //};

                //return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.StoredProcedure, "spAddChart", para);
                SqlHelper.ExecuteNonQuery(objChart.ConnConfig, CommandType.StoredProcedure, "spAddChart", para);
                //return Convert.ToInt32(para[32].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllCOA(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                //varname.Append(" \n");
                //varname.Append(" SELECT ID, Acct, fDesc, ISNULL(Balance,0) as Balance,                                      \n");
                //varname.Append("        CASE WHEN ISNULL(Balance,0) > 0 THEN ISNULL(Balance,0) ELSE 0 END AS Debit,         \n");
                //varname.Append("        CASE WHEN ISNULL(Balance,0) < 0 THEN ISNULL(Balance,0) * -1 ELSE 0 END AS Credit,   \n");
                //varname.Append("       Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status,                          \n");
                //varname.Append("       (CASE isnull(Status,0) WHEN 0 THEN 'Active'                                          \n");
                //varname.Append("       WHEN 1 THEN 'InActive'                                                               \n");
                //varname.Append("       WHEN 2 THEN 'Hold' END) AS AcctStatus,                                               \n");
                //varname.Append("       (CASE Type WHEN 0 THEN 'Asset'                                                       \n");
                //varname.Append("        WHEN 1 THEN 'Liability'                                                             \n");
                //varname.Append("        WHEN 2 THEN 'Equity'                                                                \n");
                //varname.Append("        WHEN 3 THEN 'Revenue'                                                               \n");
                //varname.Append("        WHEN 4 THEN 'Cost'                                                                  \n");
                //varname.Append("        WHEN 5 THEN 'Expense'                                                               \n");
                //varname.Append("        WHEN 6 THEN 'Bank'  WHEN 7 THEN 'Non-Posting' END) AS AcctType,                     \n");
                //varname.Append("        Sub2, DAT, Branch, CostCenter, AcctRoot,QBAccountID,LastUpdateDate                  \n");
                //varname.Append("            FROM Chart                                                                      \n");
                //varname.Append("                Order by Acct       \n");

                ///By Ravinder

                //varname.Append("   SELECT     ID,      \n");
                //varname.Append("           Acct,       \n");
                //varname.Append("           fDesc,      \n");
                //varname.Append("           case when Department = '-1' then 'Prorated' \n");
                //varname.Append("           when Department = '-2' then 'By Transaction' \n");
                //varname.Append("           else \n");
                //varname.Append("           (select Type from JobType where id = Department) \n");
                //varname.Append("           end \n");
                //varname.Append("           as [Department], \n");

                //varname.Append("           CASE WHEN ISNULL(Balance,0) > 0     \n");
                //varname.Append("               THEN ISNULL(Balance,0)          \n");
                //varname.Append("               ELSE 0                          \n");
                //varname.Append("           END AS Debit,                       \n");
                //varname.Append("           CASE WHEN ISNULL(Balance,0) < 0     \n");
                //varname.Append("               THEN ISNULL(Balance,0) * -1     \n");
                //varname.Append("               ELSE 0                          \n");
                //varname.Append("           END AS Credit,                      \n");
                //varname.Append("           ISNULL(Balance,0) as Balance,       \n");
                //varname.Append("           Type,                               \n");
                //varname.Append("           Sub,                                \n");
                //varname.Append("           InUse,                              \n");
                //varname.Append("           Status,                             \n");
                //varname.Append("          (CASE ISNULL(Status,0)               \n");
                //varname.Append("                  WHEN 0 THEN 'Active'         \n");
                //varname.Append("                  WHEN 1 THEN 'InActive'       \n");
                //varname.Append("                  WHEN 2 THEN 'Hold' END) AS AcctStatus,               \n");
                //varname.Append("          (CASE Type WHEN 0 THEN 'Asset'       \n");
                //varname.Append("                  WHEN 1 THEN 'Liability'      \n");
                //varname.Append("                  WHEN 2 THEN 'Equity'         \n");
                //varname.Append("                  WHEN 3 THEN 'Revenue'        \n");
                //varname.Append("                  WHEN 4 THEN 'Cost'           \n");
                //varname.Append("                  WHEN 5 THEN 'Expense'        \n");
                //varname.Append("                  WHEN 6 THEN 'Bank'           \n");
                //varname.Append("                  WHEN 7 THEN 'Non-Posting'    \n");
                //varname.Append("              END) AS AcctType,                \n");
                //varname.Append("             Sub2                              \n");
                //varname.Append("       FROM Chart ORDER BY Acct                \n");

                varname.Append("   SELECT    Ch.ID,      \n");
                varname.Append("           Ch.Acct,       \n");
                varname.Append("           Ch.fDesc,       \n");
                varname.Append("           (select CentralName from Central where id = Department) \n");
                varname.Append("           as [Department], \n");
                varname.Append("            CASE WHEN ISNULL(Ch.Balance,0) > 0      \n");
                varname.Append("               THEN ISNULL(Ch.Balance,0)          \n");
                varname.Append("               ELSE 0                          \n");
                varname.Append("           END AS Debit,                       \n");
                varname.Append("           CASE WHEN ISNULL(Ch.Balance,0) < 0     \n");
                varname.Append("               THEN ISNULL(Ch.Balance,0) * -1     \n");
                varname.Append("               ELSE 0                          \n");
                varname.Append("           END AS Credit,                      \n");
                varname.Append("           ISNULL(Ch.Balance,0) as Balance,       \n");
                varname.Append("            Ch.Type,                               \n");
                varname.Append("           Ch.Sub,                                \n");
                varname.Append("            Ch.InUse,                               \n");
                varname.Append("            Ch.Status,                             \n");
                varname.Append("          (CASE ISNULL(Ch.Status,0)               \n");
                varname.Append("                  WHEN 0 THEN 'Active'         \n");
                varname.Append("                  WHEN 1 THEN 'InActive'       \n");
                varname.Append("                  WHEN 2 THEN 'Hold' END) AS AcctStatus,               \n");
                varname.Append("          (CASE Ch.Type WHEN 0 THEN 'Asset'       \n");
                varname.Append("                  WHEN 1 THEN 'Liability'      \n");
                varname.Append("                  WHEN 2 THEN 'Equity'         \n");
                varname.Append("                  WHEN 3 THEN 'Revenue'        \n");
                varname.Append("                  WHEN 4 THEN 'Cost'           \n");
                varname.Append("                  WHEN 5 THEN 'Expense'        \n");
                varname.Append("                  WHEN 6 THEN 'Bank'           \n");
                varname.Append("                  WHEN 7 THEN 'Non-Posting'    \n");
                varname.Append("                  END) AS AcctType,                \n");
                varname.Append("                  Sub2,Rol.EN, LTRIM(RTRIM(B.Name)) As Company                               \n");
                varname.Append("                  FROM Chart Ch                 \n");
                varname.Append("                   left join Bank bk on ch.ID=bk.Chart                 \n");
                varname.Append("                    Left Join Rol on bk.Rol=Rol.ID                 \n");
                if (_objChart.EN == 1)
                    varname.Append("                    left outer join tblUserCo UC on UC.CompanyID = Rol.EN or UC.CompanyID=Ch.EN                \n");
                varname.Append("                    left Outer join Branch B on B.ID = Rol.EN or B.ID=Ch.EN                 \n");
                if (_objChart.EN == 1)
                {
                    varname.Append("      Where UC.IsSel = " + _objChart.EN + " and UC.UserID= " + _objChart.UserID + " \n");
                }
                varname.Append("                    ORDER BY Ch.Acct                \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllChartByAsset(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();


                varname.Append("   SELECT     ID,      \n");
                varname.Append("           Acct,       \n");
                varname.Append("           fDesc,      \n");
                varname.Append("           (select CentralName from Central where id = Department) \n");
                varname.Append("           as [Department], \n");
                varname.Append("           CASE WHEN ISNULL(Balance,0) > 0     \n");
                varname.Append("               THEN ISNULL(Balance,0)          \n");
                varname.Append("               ELSE 0                          \n");
                varname.Append("           END AS Debit,                       \n");
                varname.Append("           CASE WHEN ISNULL(Balance,0) < 0     \n");
                varname.Append("               THEN ISNULL(Balance,0) * -1     \n");
                varname.Append("               ELSE 0                          \n");
                varname.Append("           END AS Credit,                      \n");
                varname.Append("           ISNULL(Balance,0) as Balance,       \n");
                varname.Append("           Type,                               \n");
                varname.Append("           Sub,                                \n");
                varname.Append("           InUse,                              \n");
                varname.Append("           Status,                             \n");
                varname.Append("          (CASE ISNULL(Status,0)               \n");
                varname.Append("                  WHEN 0 THEN 'Active'         \n");
                varname.Append("                  WHEN 1 THEN 'InActive'       \n");
                varname.Append("                  WHEN 2 THEN 'Hold' END) AS AcctStatus,               \n");
                varname.Append("          (CASE Type WHEN 0 THEN 'Asset'       \n");
                varname.Append("                  WHEN 1 THEN 'Liability'      \n");
                varname.Append("                  WHEN 2 THEN 'Equity'         \n");
                varname.Append("                  WHEN 3 THEN 'Revenue'        \n");
                varname.Append("                  WHEN 4 THEN 'Cost'           \n");
                varname.Append("                  WHEN 5 THEN 'Expense'        \n");
                varname.Append("                  WHEN 6 THEN 'Bank'           \n");
                varname.Append("                  WHEN 7 THEN 'Non-Posting'    \n");
                varname.Append("              END) AS AcctType,                \n");
                varname.Append("             Sub2                              \n");
                varname.Append("       FROM Chart where Type=0 ORDER BY Acct                \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateChart(Chart objChart, Bank objBank)
        {
            try
            {
                var para = new SqlParameter[65];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Acct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Acct
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@AcType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.AcType
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@Sub",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Sub
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Sub2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Sub2
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@Remarks",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Remarks
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@Status",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Status
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@City",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.City
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@State",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.State
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "@Zip",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Zip
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@Phone",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Phone
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "@Fax",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Fax
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "@Contact",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Contact
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "@Address",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Address
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.EMail
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "@Website",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Website
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "@Country",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Country
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "@Cellular",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Cellular
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Type
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "@GeoLock",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.GeoLock
                };
                if (objChart.Since != DateTime.MinValue)
                {
                    para[20] = new SqlParameter
                    {
                        ParameterName = "@Since",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Since
                    };
                }
                if (objChart.Last != DateTime.MinValue)
                {
                    para[21] = new SqlParameter
                    {
                        ParameterName = "@Last",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objChart.Last
                    };
                }
                para[22] = new SqlParameter
                {
                    ParameterName = "@NBranch",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NBranch
                };
                para[23] = new SqlParameter
                {
                    ParameterName = "@NAcct",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NAcct
                };
                para[24] = new SqlParameter
                {
                    ParameterName = "@NRoute",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.NRoute
                };
                para[25] = new SqlParameter
                {
                    ParameterName = "@NextC",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextC
                };
                para[26] = new SqlParameter
                {
                    ParameterName = "@NextD",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextD
                };
                para[27] = new SqlParameter
                {
                    ParameterName = "@NextE",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.NextE
                };
                para[28] = new SqlParameter
                {
                    ParameterName = "@Rate",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Rate
                };
                para[29] = new SqlParameter
                {
                    ParameterName = "@CLimit",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.CLimit
                };
                para[30] = new SqlParameter
                {
                    ParameterName = "@Warn",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.Warn
                };
                para[31] = new SqlParameter
                {
                    ParameterName = "@Recon",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objChart.Recon
                };
                para[32] = new SqlParameter
                {
                    ParameterName = "@Rol",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.Rol
                };
                para[33] = new SqlParameter
                {
                    ParameterName = "@Bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.Bank
                };
                para[34] = new SqlParameter
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.ID
                };

                para[35] = new SqlParameter
                {
                    ParameterName = "@Department",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.Department
                };
                para[36] = new SqlParameter
                {
                    ParameterName = "@BankName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.BankName
                };
                para[37] = new SqlParameter
                {
                    ParameterName = "@Lat",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Lat
                };
                para[38] = new SqlParameter
                {
                    ParameterName = "@Long",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.Long
                };
                para[39] = new SqlParameter
                {
                    ParameterName = "@EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objChart.EN
                };
                para[40] = new SqlParameter
                {
                    ParameterName = "@NoJE",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objChart.NoJE
                };

                para[41] = new SqlParameter
                {
                    ParameterName = "@ACHFileHeaderStringA",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileHeaderStringA
                };
                para[42] = new SqlParameter
                {
                    ParameterName = "@ACHFileHeaderStringB",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileHeaderStringB
                };
                para[43] = new SqlParameter
                {
                    ParameterName = "@ACHFileHeaderStringC",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileHeaderStringC
                };
                para[44] = new SqlParameter
                {
                    ParameterName = "@ACHCompanyHeaderString1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHCompanyHeaderString1
                };
                para[45] = new SqlParameter
                {
                    ParameterName = "@ACHCompanyHeaderString2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHCompanyHeaderString2
                };
                para[46] = new SqlParameter
                {
                    ParameterName = "@ACHBatchControlString1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHBatchControlString1
                };
                para[47] = new SqlParameter
                {
                    ParameterName = "@ACHBatchControlString2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHBatchControlString2
                };
                para[48] = new SqlParameter
                {
                    ParameterName = "@ACHBatchControlString3",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHBatchControlString3
                };
                para[49] = new SqlParameter
                {
                    ParameterName = "@ACHFileControlString1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.ACHFileControlString1
                };
                para[50] = new SqlParameter
                {
                    ParameterName = "@APACHCompanyID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.APACHCompanyID
                };
                para[51] = new SqlParameter
                {
                    ParameterName = "@APImmediateOrigin",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.APImmediateOrigin
                };
                para[52] = new SqlParameter
                {
                    ParameterName = "@NextACH",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.NextACH
                };


                para[53] = new SqlParameter
                {
                    ParameterName = "@TraceNo1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TraceNo1
                };
                para[54] = new SqlParameter
                {
                    ParameterName = "@TraceNo2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TraceNo2
                };
                para[55] = new SqlParameter
                {
                    ParameterName = "@RecordTypeCode1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.RecordTypeCode1
                };
                para[56] = new SqlParameter
                {
                    ParameterName = "@RecordTypeCode2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.RecordTypeCode2
                }; 
                para[57] = new SqlParameter
                {
                    ParameterName = "@TransactionCode1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TransactionCode1
                };
                para[58] = new SqlParameter
                {
                    ParameterName = "@TransactionCode2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.TransactionCode2
                };
                para[59] = new SqlParameter
                {
                    ParameterName = "@EndRecordIndicator1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.EndRecordIndicator1
                };
                para[60] = new SqlParameter
                {
                    ParameterName = "@EndRecordIndicator2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.EndRecordIndicator2
                };
                para[61] = new SqlParameter
                {
                    ParameterName = "@OriginatorStatusCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.OriginatorStatusCode
                };
                para[62] = new SqlParameter
                {
                    ParameterName = "@RecordTypeCode3",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.RecordTypeCode3
                };
                para[63] = new SqlParameter
                {
                    ParameterName = "@BatchNumber",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.BatchNumber
                };
                para[64] = new SqlParameter
                {
                    ParameterName = "@JulianDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objBank.JulianDate
                };
                

                SqlHelper.ExecuteNonQuery(objChart.ConnConfig, CommandType.StoredProcedure, "spUpdateChart", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetChart(Chart _objChart)
        {
            try
            {
                // return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT [ID],[Acct],[fDesc],[Department],[Balance],[Type],[Sub],[Remarks],[Control],[InUse],[Detail],[CAlias],[Status],[Sub2],[DAT],[Branch],[CostCenter],[AcctRoot],[QBAccountID],[LastUpdateDate],ISNULL(DefaultNo,0) AS DefaultNo FROM Chart WHERE ID = " + _objChart.ID);
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT ch.ID,[Acct],ch.fDesc,[Department],ch.Balance,ch.Type,[Sub],ch.Remarks,ch.Control,ch.InUse,[Detail],[CAlias],ch.Status,[Sub2],[DAT],[Branch],ch.CostCenter,[AcctRoot],[QBAccountID],ch.LastUpdateDate,isnull(Rol.EN,Ch.EN) As EN,B.Name As company,ISNULL(DefaultNo,0) AS DefaultNo,ISNULL(ch.NoJE,1) AS NoJE FROM  Bank bk Right join Chart ch on ch.ID=bk.Chart Left Join Rol on bk.Rol=Rol.ID  left Outer join Branch B on B.ID = Rol.EN or B.ID=Ch.EN WHERE ch.ID =" + _objChart.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetChartDetail(Chart _objChart)
        {
            try
            {
                // return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT [ID],[Acct],[fDesc],[Department],[Balance],[Type],[Sub],[Remarks],[Control],[InUse],[Detail],[CAlias],[Status],[Sub2],[DAT],[Branch],[CostCenter],[AcctRoot],[QBAccountID],[LastUpdateDate],ISNULL(DefaultNo,0) AS DefaultNo FROM Chart WHERE ID = " + _objChart.ID);
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT ch.ID,[Acct],ch.fDesc,[Department],ch.Balance,ch.Type,[Sub],ch.Remarks,ch.Control,ch.InUse,[Detail],[CAlias],ch.Status,[Sub2],[DAT],[Branch],ch.CostCenter,[AcctRoot],[QBAccountID],ch.LastUpdateDate,isnull(Rol.EN,Ch.EN) As EN,B.Name As company,ISNULL(DefaultNo,0) AS DefaultNo,(SELECT ISNULL(COUNT(*),0) FROM JOB WHERE GL = ch.ID) AS JobInAcct,(SELECT ISNULL(COUNT(*),0) FROM Vendor WHERE DA = ch.ID) AS VendorInAcct,(SELECT ISNULL(COUNT(*),0) FROM Trans WHERE ID = ch.ID) AS TransInAcct FROM  Bank bk Right join Chart ch on ch.ID=bk.Chart Left Join Rol on bk.Rol=Rol.ID  left Outer join Branch B on B.ID = Rol.EN or B.ID=Ch.EN WHERE ch.ID =" + _objChart.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //17-11-21
        public Int32 GetCharID(Chart _objChart)
        {
            //Int32 COAID = 0;
            string Acct = _objChart.Acct;
            try
            {
                // return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT [ID],[Acct],[fDesc],[Department],[Balance],[Type],[Sub],[Remarks],[Control],[InUse],[Detail],[CAlias],[Status],[Sub2],[DAT],[Branch],[CostCenter],[AcctRoot],[QBAccountID],[LastUpdateDate],ISNULL(DefaultNo,0) AS DefaultNo FROM Chart WHERE ID = " + _objChart.ID);
                //var COAID = SqlHelper.ExecuteNonQuery(_objChart.ConnConfig, CommandType.Text, "select Id from chart as ch where ch.Acct ='" _objChart.Acct + "')
                //   return COAID;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, " SELECT ID FROM Chart where Acct='" + _objChart.Acct+ "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetChart(GetChartParam _GetChartParam, string ConnectionString)
        {
            try
            {
                // return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT [ID],[Acct],[fDesc],[Department],[Balance],[Type],[Sub],[Remarks],[Control],[InUse],[Detail],[CAlias],[Status],[Sub2],[DAT],[Branch],[CostCenter],[AcctRoot],[QBAccountID],[LastUpdateDate],ISNULL(DefaultNo,0) AS DefaultNo FROM Chart WHERE ID = " + _objChart.ID);
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ch.ID,[Acct],ch.fDesc,[Department],ch.Balance,ch.Type,[Sub],ch.Remarks,ch.Control,ch.InUse,[Detail],[CAlias],ch.Status,[Sub2],[DAT],[Branch],ch.CostCenter,[AcctRoot],[QBAccountID],ch.LastUpdateDate,isnull(Rol.EN,Ch.EN) As EN,B.Name As company,ISNULL(DefaultNo,0) AS DefaultNo FROM  Bank bk Right join Chart ch on ch.ID=bk.Chart Left Join Rol on bk.Rol=Rol.ID  left Outer join Branch B on B.ID = Rol.EN or B.ID=Ch.EN WHERE ch.ID =" + _GetChartParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteChart(Chart _objChart)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(_objChart.ConnConfig, CommandType.Text, " DELETE FROM Chart WHERE ID = " + _objChart.ID);
                SqlHelper.ExecuteNonQuery(_objChart.ConnConfig, "spDeleteChart", _objChart.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStatus(Chart _objChart)
        {
            try
            {
                return _objChart.DsStatus = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT  IDENTITY (INT, 0, 1) AS ID, Status INTO #tempStatus FROM [Status] SELECT * FROM #tempStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistAcct(Chart objChart)
        {
            try
            {
                //return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, "declare @fdesc nvarchar(max)=  ;  SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Chart WHERE Acct= '" + objChart.Acct + "' )THEN 1  ELSE 0  END AS BIT)"));
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, " SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Chart WHERE Acct= '" + objChart.Acct + "')THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistAcctForEdit(Chart objChart)
        {
            try
            {
                //return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Chart WHERE Acct= '" + objChart.Acct + "' AND ID <> " + objChart.ID + " )THEN 1  ELSE 0  END AS BIT)"));
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, " SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Chart WHERE Acct= '" + objChart.Acct + "' AND ID <> " + objChart.ID + " )THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistAcctNameForEdit(Chart objChart)
        {
            try
            {
             
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, " SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Bank WHERE fDesc='" + objChart.fDesc.Replace("'", "''") + "' AND Chart <> " + objChart.ID + " )THEN 1  ELSE 0  END AS BIT)"));
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExistAcctORBANKNameForEdit(Chart objChart)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "ID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.fDesc
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@AcType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objChart.SearchValue
                };
               

                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.StoredProcedure, "spIsExistAcctORBANKNameForEdit", para));

               

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistAcctNameExists(Chart objChart)
        {
            try
            {
                //check account name
                 return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, " SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Bank WHERE fDesc= '" + objChart.fDesc.Replace("'", "''") + "' )THEN 1  ELSE 0  END AS BIT)"));
               // return Convert.ToBoolean(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, " SELECT  CAST( CASE WHEN EXISTS(SELECT ID FROM Bank WHERE fDesc= '" + objChart.fDesc + "' )THEN 1  ELSE 0  END AS BIT)"));
              
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAutoFillAccount(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spGetAccountSearch", _objChart.SearchValue);
                //SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "SELECT ID, Acct, fDesc, Balance, Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status, Sub2, DAT, Branch, CostCenter, AcctRoot,QBAccountID,LastUpdateDate FROM Chart Order by Acct");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAutoFillAccountJE(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spGetAccountSearchJE", _objChart.SearchValue);
                //SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "SELECT ID, Acct, fDesc, Balance, Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status, Sub2, DAT, Branch, CostCenter, AcctRoot,QBAccountID,LastUpdateDate FROM Chart Order by Acct");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet spGetAccountSearchAP(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spGetAccountSearchAP", _objChart.SearchValue);
                //SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "SELECT ID, Acct, fDesc, Balance, Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status, Sub2, DAT, Branch, CostCenter, AcctRoot,QBAccountID,LastUpdateDate FROM Chart Order by Acct");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccountData(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spGetChartSearch", _objChart.SearchIndex, _objChart.SearchBy, _objChart.Condition, _objChart.SearchAcctType, _objChart.Sub, _objChart.SearchStatus, _objChart.EN, _objChart.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankAcct(Chart _objChart)             // Cash in bank account
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D1000%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D1000' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAcctReceivable(Chart _objChart)       // Account Receivable
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D1200%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUndepositeAcct(Chart _objChart)       // Undeposited Fund
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D1100%' AND Status=0 ORDER BY ID");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetUndepositeAcct(GetUndepositeAcctParam _GetUndepositeAcct, string ConnectionString)  // Undeposited Fund
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D1100%' AND Status=0 ORDER BY ID");
                return _GetUndepositeAcct.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D1100' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAcctPayable(Chart _objChart)          // Account Payable
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D2000%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D2000' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAcctPayable(GetAcctPayableParam _objGetAcctPayableParam, string ConnectionString)       // Account Payable
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D2000%' AND Status=0 ORDER BY ID ");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D2000' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesTaxAcct(Chart _objChart)         // Sales tax
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D2100%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D2100' AND Status=0 ORDER BY ID "); //D2120
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRetainedEarn(Chart _objChart)         // Retained Earning
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D3920%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D3920' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCurrentEarn(Chart _objChart)          // Current Earning
        {
            try
            {
                var query = "SELECT TOP 1 c.*, (CASE c.Type WHEN 0 THEN 'Asset' WHEN 1 THEN 'Liability'WHEN 2 THEN 'Equity'WHEN 3 THEN 'Revenues'WHEN 4 THEN 'Cost of Sales'WHEN 5 THEN 'Expenses' WHEN 6 THEN 'Bank' END) AS TypeName, ISNULL(ct.CentralName,'Undefined') AS CentralName " +
                    "FROM Chart c LEFT JOIN Central ct ON c.Department = ct.ID WHERE c.DefaultNo='D3130' AND c.Status = 0 ORDER BY ID";
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStock(Chart _objChart)                // Stock 
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D3110%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D3110' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDistribution(Chart _objChart)         // Distribution
        {
            try
            {
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE Acct LIKE '%D3140%' AND Status=0 ORDER BY ID ");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D3140' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateChartBalance(Chart _objChart)
        {
            SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spUpdateChartBalance", _objChart.ID, _objChart.Amount);
        }
        public void UpdateChartBalance(UpdateChartBalanceParam _objUpdateChartBalanceParam,string ConnectionString)
        {
            SqlHelper.ExecuteDataset(ConnectionString, "spUpdateChartBalance", _objUpdateChartBalanceParam.ID, _objUpdateChartBalanceParam.Amount);
        }
        public DataSet GetChartByID(Chart _objChart) // By Viral
        {
            try
            {
                StringBuilder varname11 = new StringBuilder();
                varname11.Append(" \n");
                varname11.Append("select c.ID, \n");
                varname11.Append("       t.Acct, \n");
                varname11.Append("       t.fDate, \n");
                varname11.Append("       t.Batch, \n");
                varname11.Append("       t.Ref, \n");
                varname11.Append("       (CASE t.Type WHEN 50 THEN '1'");
                varname11.Append("       WHEN 40 THEN '2' ");
                varname11.Append("       WHEN 41 THEN '2' ");
                varname11.Append("       WHEN 21 THEN '3' ");
                varname11.Append("       WHEN 20 THEN '3' ");
                varname11.Append("       WHEN 5 THEN '4' ");
                varname11.Append("       WHEN 6 THEN '4' ");
                varname11.Append("       WHEN 5 THEN '5' ");
                varname11.Append("        WHEN 6 THEN '5' ");
                varname11.Append("       WHEN 1 THEN '6' ");
                varname11.Append("       WHEN 2 THEN '6' ");
                varname11.Append("       WHEN 3 THEN '6' ");
                varname11.Append("       WHEN 40 THEN '8' ");
                varname11.Append("       WHEN 40 THEN '8' ");
                varname11.Append("       WHEN 98 THEN '9' ");
                varname11.Append("       WHEN 99 THEN '9' END) as Type, \n");
                varname11.Append("       t.Ref, \n");
                varname11.Append("       c.fDesc as ChartfDesc, \n");
                varname11.Append("       c.Department, \n");
                varname11.Append("       t.fDesc, \n");
                varname11.Append("         t.Amount, \n");
                varname11.Append("         c.Balance \n");
                varname11.Append("        FROM   Chart c \n");
                varname11.Append("        INNER JOIN Trans t \n");
                varname11.Append("        on c.ID=t.Acct \n");
                varname11.Append("        WHERE  c.ID=" + _objChart.ID + " Order by t.fDate");

                //return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "select c.ID,t.Acct,t.fDate,t.Batch,t.Ref,(CASE t.Type WHEN 50 THEN '1' WHEN 40 THEN '2' WHEN 41 THEN '2'  WHEN 21 THEN '3' WHEN 20 THEN '3' WHEN 5 THEN '4' WHEN 6 THEN '4' WHEN 5 THEN '5' WHEN 6 THEN '5' WHEN 1 THEN '6' WHEN 2 THEN '6' WHEN 3 THEN '6' WHEN 40 THEN '8' WHEN 41 THEN '8' end)As Type,t.Ref,c.fDesc as ChartfDesc,t.fDesc,t.Amount,c.Balance from Chart c join Trans t on c.ID=t.Acct where c.ID=" + objChart.ID);          
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllChartByDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" \n");
                varname.Append("select c.ID, \n");
                varname.Append("       t.Acct, \n");
                varname.Append("       t.fDate, \n");
                varname.Append("       t.Batch, \n");
                varname.Append("       t.Ref, \n");
                varname.Append("       (CASE t.Type WHEN 50 THEN '1'");
                varname.Append("       WHEN 40 THEN '2' ");
                varname.Append("       WHEN 41 THEN '2' ");
                varname.Append("       WHEN 21 THEN '3' ");
                varname.Append("       WHEN 20 THEN '3' ");
                varname.Append("       WHEN 5 THEN '4' ");
                varname.Append("       WHEN 6 THEN '4' ");
                varname.Append("       WHEN 5 THEN '5' ");
                varname.Append("       WHEN 6 THEN '5' ");
                varname.Append("       WHEN 1 THEN '6' ");
                varname.Append("       WHEN 2 THEN '6' ");
                varname.Append("       WHEN 3 THEN '6' ");
                varname.Append("       WHEN 40 THEN '8' ");
                varname.Append("       WHEN 41 THEN '8' ");
                varname.Append("       WHEN 98 THEN '9' ");
                varname.Append("       WHEN 99 THEN '9' ");
                varname.Append("       WHEN 30 THEN '7' ");
                varname.Append("       WHEN 31 THEN '7' ");
                varname.Append("       ELSE t.Type END) as Type, \n");
                varname.Append("       t.Ref, \n");
                varname.Append("       c.fDesc as ChartfDesc, \n");
                varname.Append("       c.Department, \n");
                varname.Append("       t.fDesc, \n");
                varname.Append("         t.Amount, \n");
                varname.Append("         c.Balance \n");
                varname.Append("        FROM   Chart c \n");
                varname.Append("        INNER JOIN Trans t \n");
                varname.Append("        on c.ID=t.Acct \n");
                varname.Append("        WHERE c.ID=" + _objChart.ID + " AND (t.fDate >= '" + _objChart.StartDate + "') AND (t.fDate <= '" + _objChart.EndDate + "') ORDER BY t.fDate");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
                //return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "select c.ID,t.Acct,t.fDate,t.Batch,t.Ref,(CASE t.Type WHEN 50 THEN '1' WHEN 40 THEN '2' WHEN 41 THEN '2'  WHEN 21 THEN '3' WHEN 20 THEN '3' WHEN 5 THEN '4' WHEN 6 THEN '4' WHEN 5 THEN '5' WHEN 6 THEN '5' WHEN 1 THEN '6' WHEN 2 THEN '6' WHEN 3 THEN '6' WHEN 40 THEN '8' WHEN 41 THEN '8' end)As Type,t.Ref,c.fDesc as ChartfDesc,t.fDesc,t.Amount,c.Balance from Chart c join Trans t on c.ID=t.Acct where (t.fDate >= '" + objChart.StartDate + "') AND (t.fDate <= '" + objChart.EndDate + "') ORDER BY t.fDate");
                //return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT DISTINCT g.fDate, g.Ref, g.Internal, g.fDesc, g.Batch FROM GLA as g, Trans as t where (g.Batch = t.Batch) AND (g.Batch = t.Batch) AND (t.Type = 50) AND (g.fDate >= '" + objJournal.StartDate + "') AND (g.fDate <= '" + objJournal.EndDate + "') ORDER BY g.Ref DESC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMinTransDate(Chart _objChart)
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT Min(fDate) AS MinDate FROM Trans");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfRevenueByDate(Chart _objChart) //To get Revenue Total details
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("	ISNULL(SUM(t.Amount),0.00) \n");
                varname.Append("    FROM Trans as t, Chart as c  \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 3 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate > '" + _objChart.StartDate + "' AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfCostSalesByDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 4 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate > '" + _objChart.StartDate + "' AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfExpenseByDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 5 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate > '" + _objChart.StartDate + "' AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsChartBankAcct(Chart _objChart)
        {

            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  CAST( \n");
                varname.Append("    CASE WHEN EXISTS(SELECT * FROM Chart  \n");
                varname.Append("    WHERE ID = " + _objChart.ID + " AND Type = 6) \n");
                varname.Append("    THEN 1 \n");
                varname.Append("    ELSE 0 \n");
                varname.Append("    END \n");
                varname.Append("    AS BIT)  \n");
                return _objChart.IsBankAcct = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankBalance(Chart _objChart)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spUpdateBankBalance", _objChart.ID, _objChart.Amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccountLedger(Chart _objChart)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spAccountLedger", _objChart.ID, _objChart.StartDate, _objChart.EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CalChartBalance(Chart _objChart)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spCalChartBalance");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankCharge(Chart _objChart)             // Bank Charges
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D6000' AND Status=0 ORDER BY ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPOAcct(Chart _objChart)       // Purchase Order
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM Chart WHERE DefaultNo='D9991' AND Status=0 ORDER BY ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetChartByAcct(Chart objChart)
        {
            try
            {
                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, "SELECT * FROM Chart WHERE Acct = '" + objChart.Acct + "' AND Type<>7 ");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Inventory Implementation
        public List<Chart> GetChartByType(int type)
        {
            DataSet ds = null;
            List<Chart> chart = new List<Chart>();
            try
            {
                string constring = string.Empty;
                if (HttpContext.Current.Session["config"] != null)
                {
                    constring = HttpContext.Current.Session["config"].ToString();
                }

                if (string.IsNullOrEmpty(constring))
                    return chart;

                ds = SqlHelper.ExecuteDataset(constring, "spGetChartByType", type);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Chart objchart = new Chart();
                            objchart.ID = ds.Tables[0].Rows[i]["ID"] != DBNull.Value ? (int)ds.Tables[0].Rows[i]["ID"] : 0;
                            objchart.Acct = ds.Tables[0].Rows[i]["Acct"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Acct"] : "";
                            objchart.fDesc = ds.Tables[0].Rows[i]["fDesc"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["fDesc"] : "";
                            chart.Add(objchart);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chart;
        }


        //API
        public List<Chart> GetChartByType(GetChartByTypeParam _GetChartByTypeParam, string ConnectionString)
        {
            DataSet ds = null;
            List<Chart> chart = new List<Chart>();
            try
            {
                string constring = string.Empty;
                if (ConnectionString != null)
                {
                    constring = ConnectionString;
                }

                if (string.IsNullOrEmpty(constring))
                    return chart;

                ds = SqlHelper.ExecuteDataset(constring, "spGetChartByType", _GetChartByTypeParam.type);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Chart objchart = new Chart();
                            objchart.ID = ds.Tables[0].Rows[i]["ID"] != DBNull.Value ? (int)ds.Tables[0].Rows[i]["ID"] : 0;
                            objchart.Acct = ds.Tables[0].Rows[i]["Acct"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Acct"] : "";
                            objchart.fDesc = ds.Tables[0].Rows[i]["fDesc"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["fDesc"] : "";
                            chart.Add(objchart);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chart;
        }
        public double GetSumOfRevenueByAsOfDate(Chart _objChart) //To get Revenue Total details
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("	ISNULL(SUM(t.Amount),0.00) \n");
                varname.Append("    FROM Trans as t, Chart as c  \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 3 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfCostSalesByAsOfDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 4 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetSumOfExpenseByAsOfDate(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT  \n");
                varname.Append("    ISNULL(SUM(t.Amount),0.00)  \n");
                varname.Append("    FROM Trans as t, Chart as c \n");
                varname.Append("    WHERE c.ID = t.Acct \n");
                varname.Append("    AND c.Type = 5 \n");
                varname.Append("    AND t.Amount <> 0.00 \n");
                varname.Append("    AND t.fDate < '" + _objChart.EndDate + "'  \n");
                return _objChart.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(_objChart.ConnConfig, CommandType.Text, varname.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetGeneralLedger(Chart _objChart) //To get Revenue Total details
        {
            try
            {
                //StringBuilder varname = new StringBuilder();
                //varname.Append("select c.ID, t.Acct, t.fDate, t.Batch,   \n");
                //varname.Append("	isnull(t.Ref,0) as Ref,  \n");
                //varname.Append("    dbo.TransTypeToText(t.type) as TypeText,  \n");
                //varname.Append("   (CASE t.Type WHEN 50 THEN '1'  \n");
                //varname.Append("   WHEN 40 THEN '2'  \n");
                //varname.Append("   WHEN 41 THEN '2'  \n");
                //varname.Append("   WHEN 21 THEN '3'  \n");
                //varname.Append("   WHEN 20 THEN '3'  \n");
                //varname.Append("   WHEN 5 THEN '4'   \n");
                //varname.Append("   WHEN 6 THEN '4'   \n");
                //varname.Append("   WHEN 5 THEN '5'  \n");
                //varname.Append("   WHEN 6 THEN '5'   \n");
                //varname.Append("   WHEN 1 THEN '6'   \n");
                //varname.Append("   WHEN 2 THEN '6'   \n");
                //varname.Append("   WHEN 3 THEN '6'   \n");
                //varname.Append("   WHEN 40 THEN '8'  \n");
                //varname.Append("   WHEN 41 THEN '8'  \n");
                //varname.Append("   WHEN 98 THEN '9'  \n");
                //varname.Append("   WHEN 99 THEN '9'  \n");
                //varname.Append("   WHEN 30 THEN '7'  \n");
                //varname.Append("   WHEN 31 THEN '7'  \n");
                //varname.Append("   ELSE t.Type END)  \n");
                //varname.Append("   as Type,   \n");

                //varname.Append("    isnull(c.fDesc,'') as ChartName,  \n");
                //varname.Append("    isnull(t.fDesc,'') as fDesc,   \n");
                //varname.Append("    isnull(t.Amount,0) as Amount,  \n");
                //varname.Append("     0 As Balance,  \n");
                //varname.Append("    (CASE WHEN t.Amount > 0    \n");
                //varname.Append("    THEN t.Amount   \n");
                //varname.Append("    ELSE 0 END) As Debit,  \n");
                //varname.Append("    (CASE WHEN t.Amount < 0   \n");
                //varname.Append("    THEN (t.Amount * -1)  \n");
                //varname.Append("   ELSE 0 END) As Credit \n");
                //varname.Append("    FROM   Chart c	INNER JOIN Trans t ON c.ID=t.Acct  \n");
                //varname.Append("    Where c.fDesc = 'Cash in Bank' and t.fDate >= '2016-01-01 00:00:00:000' And t.fDate <= '2016-01-31 00:00:00:000' \n");
                //varname.Append("    ORDER BY t.fDate \n");                               
                //varname.Append("    AND t.fDate < '" + _objChart.EndDate + "'  \n");

                return SqlHelper.ExecuteDataset(_objChart.ConnConfig, "GeneralLedger", _objChart.ID, _objChart.StartDate, _objChart.EndDate);
                //return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetGeneralLedgerByDate(Chart _objChart) //To get Revenue Total details
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spGeneralLedger", _objChart.ID, _objChart.StartDate, _objChart.EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllAccountDetails(Chart _objChart) // Get Vendors details who's expense is exists in PJ table.
        {
            try
            {
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT DISTINCT ID, Acct +' - '+fDesc as fDesc from Chart Order By fDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetBankAcctID(Chart objChart)             // Selected bank account
        {
            try
            {
                return objChart.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(objChart.ConnConfig, CommandType.Text, "SELECT ISNULL((SELECT Chart FROM Bank WHERE ID = " + objChart.Bank + "),0)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 GetBankAcctID(GetBankAcctIDParam objGetBankAcctIDParam, string ConnectionString)             // Selected bank account
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT ISNULL((SELECT Chart FROM Bank WHERE ID = " + objGetBankAcctIDParam.Bank + "),0)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAccountLedgerPaging(Chart _objChart)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objChart.ConnConfig, "spAccountLedgerPaging",
                    _objChart.ID, _objChart.StartDate, _objChart.EndDate,_objChart.filterDate,_objChart.filterTypeText,
                    _objChart.filterRef,_objChart.filterfDesc,_objChart.filterDebit,_objChart.filterCredit,
                    _objChart.filterBalance,_objChart.pageNumber,_objChart.PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
