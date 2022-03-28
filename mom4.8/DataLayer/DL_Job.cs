using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using BusinessEntity.Projects;
using System.Collections.Generic;

namespace DataLayer
{
    public class DL_Job
    {

        public int  DeleteBillingItem(String ConnConfig,   int JobtItemID)
        {

            var para = new SqlParameter[1];

            

            para[0] = new SqlParameter
            {
                ParameterName = "JobtItemID",
                SqlDbType = SqlDbType.Int,
                Value = JobtItemID,
                
            };

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnConfig, CommandType.StoredProcedure, "spDeleteBillingItem", para));
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int DeleteBOMItem(String ConnConfig, int JobtItemID)
        {

            var para = new SqlParameter[1];



            para[0] = new SqlParameter
            {
                ParameterName = "JobtItemID",
                SqlDbType = SqlDbType.Int,
                Value = JobtItemID,

            };

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnConfig, CommandType.StoredProcedure, "spDeleteBOMItem", para));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int AddJobtItemNew( String ConnConfig, int job , int OrderNo, int JobT , int Type, int JobtItemID)
        {

            var para = new SqlParameter[5];

            para[0] = new SqlParameter
            {
                ParameterName = "job",
                SqlDbType = SqlDbType.Int,
                Value = job
            };

            para[1] = new SqlParameter
            {
                ParameterName = "JobT",
                SqlDbType = SqlDbType.Int,
                Value = JobT
            };

            para[2] = new SqlParameter
            {
                ParameterName = "Type",
                SqlDbType = SqlDbType.Int,
                Value = Type
            };

            para[3] = new SqlParameter
            {
                ParameterName = "OrderNo",
                SqlDbType = SqlDbType.Int,
                Value = OrderNo
            };

            para[4] = new SqlParameter
            {
                ParameterName = "JobtItemID",
                SqlDbType = SqlDbType.Int,
                Value = 0,
                Direction = ParameterDirection.ReturnValue
            };

            try
            {
                SqlHelper.ExecuteNonQuery( ConnConfig, CommandType.StoredProcedure, "AddJobtItemNew", para);
                return Convert.ToInt32(para[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int deleteJobtItem(String ConnConfig, int job, int OrderNo, int JobT, int Type , int JobtItemID)
        {

            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "job",
                SqlDbType = SqlDbType.Int,
                Value = job
            };

            para[1] = new SqlParameter
            {
                ParameterName = "JobT",
                SqlDbType = SqlDbType.Int,
                Value = JobT
            };

            para[2] = new SqlParameter
            {
                ParameterName = "Type",
                SqlDbType = SqlDbType.Int,
                Value = Type
            };

            para[3] = new SqlParameter
            {
                ParameterName = "OrderNo",
                SqlDbType = SqlDbType.Int,
                Value = OrderNo
            };

            para[4] = new SqlParameter
            {
                ParameterName = "JobtItemID",
                SqlDbType = SqlDbType.Int,
                Value = 0,
                Direction = ParameterDirection.ReturnValue
            };

            try
            {
                SqlHelper.ExecuteNonQuery(ConnConfig, CommandType.StoredProcedure, "DeleteJobtItem", para);
                return Convert.ToInt32(para[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddProject_New2021(Customer objPropCustomer, string groupIds)
        {
            var para = new SqlParameter[81];

            para[0] = new SqlParameter
            {
                ParameterName = "job",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectJobID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "owner",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.CustomerID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "loc",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[4] = new SqlParameter
            {
                ParameterName = "status",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Status
            };
            para[5] = new SqlParameter
            {
                ParameterName = "type",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Type
            };
            para[6] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[7] = new SqlParameter
            {
                ParameterName = "ctype",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.ctypeName
            };
            para[8] = new SqlParameter
            {
                ParameterName = "ProjCreationDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.ProjectCreationDate
            };
            para[9] = new SqlParameter
            {
                ParameterName = "PO",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.PO
            };
            para[10] = new SqlParameter
            {
                ParameterName = "SO",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.SO
            };
            para[11] = new SqlParameter
            {
                ParameterName = "Certified",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Certified
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Custom1",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom1
            };
            para[13] = new SqlParameter
            {
                ParameterName = "Custom2",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom2
            };
            para[14] = new SqlParameter
            {
                ParameterName = "Custom3",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom3
            };
            para[15] = new SqlParameter
            {
                ParameterName = "Custom4",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom4
            };
            if (objPropCustomer.Custom5 != DateTime.MinValue)
            {
                para[16] = new SqlParameter
                {
                    ParameterName = "Custom5",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropCustomer.Custom5
                };
            }
            para[17] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[18] = new SqlParameter
            {
                ParameterName = "RolName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RolName
            };
            para[19] = new SqlParameter
            {
                ParameterName = "city",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.City
            };
            para[20] = new SqlParameter
            {
                ParameterName = "state",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.State
            };
            para[21] = new SqlParameter
            {
                ParameterName = "zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Zip
            };
            para[22] = new SqlParameter
            {
                ParameterName = "country",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Country
            };
            para[23] = new SqlParameter
            {
                ParameterName = "phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[24] = new SqlParameter
            {
                ParameterName = "cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[25] = new SqlParameter
            {
                ParameterName = "fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[26] = new SqlParameter
            {
                ParameterName = "contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[27] = new SqlParameter
            {
                ParameterName = "email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[28] = new SqlParameter
            {
                ParameterName = "rolRemarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RolRemarks
            };
            para[29] = new SqlParameter
            {
                ParameterName = "rolType",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.RolType
            };
            para[30] = new SqlParameter
            {
                ParameterName = "InvExp",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.InvExp
            };
            para[31] = new SqlParameter
            {
                ParameterName = "InvServ",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.InvServ
            };
            para[32] = new SqlParameter
            {
                ParameterName = "Wage",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Wage
            };
            para[33] = new SqlParameter
            {
                ParameterName = "GLInt",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.GLInt
            };
            para[34] = new SqlParameter
            {
                ParameterName = "jobtCType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.JobTempCtype
            };
            para[35] = new SqlParameter
            {
                ParameterName = "Post",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Post
            };
            para[36] = new SqlParameter
            {
                ParameterName = "Charge",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Charge
            };
            para[37] = new SqlParameter
            {
                ParameterName = "JobClose",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.JobClose
            };
            para[38] = new SqlParameter
            {
                ParameterName = "fInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.fInt
            };
            para[39] = new SqlParameter
            {
                ParameterName = "TeamItems",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtTeam
            };
            if (objPropCustomer.DtBOM != null)
            {
                para[40] = new SqlParameter
                {
                    ParameterName = "BomItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtBOM
                };
            }
            if (objPropCustomer.dtItems != null)
            {
                para[41] = new SqlParameter
                {
                    ParameterName = "Items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
            }
            para[42] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            if (objPropCustomer.DtMilestone != null)
            {
                if (objPropCustomer.DtMilestone.Rows.Count > 0)
                {
                    para[43] = new SqlParameter
                    {
                        ParameterName = "MilestonItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = objPropCustomer.DtMilestone
                    };
                }
            }
            if (objPropCustomer.DtCustom != null)
            {
                if (objPropCustomer.DtCustom.Rows.Count > 0)
                {
                    para[44] = new SqlParameter
                    {
                        ParameterName = "CustomItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = objPropCustomer.DtCustom
                    };
                }
            }
            para[45] = new SqlParameter
            {
                ParameterName = "BillRate",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.BillRate
            };
            para[46] = new SqlParameter
            {
                ParameterName = "RateOT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateOT
            };
            para[47] = new SqlParameter
            {
                ParameterName = "RateNT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateNT
            };
            para[48] = new SqlParameter
            {
                ParameterName = "RateDT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateDT
            };
            para[49] = new SqlParameter
            {
                ParameterName = "RateTravel",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateTravel
            };
            para[50] = new SqlParameter
            {
                ParameterName = "Mileage",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Mileage
            };
            para[51] = new SqlParameter
            {
                ParameterName = "rolid",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.RoleID
            };
            para[52] = new SqlParameter
            {
                ParameterName = "TaskCodes",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.dtTaskCode
            };
            para[53] = new SqlParameter
            {
                ParameterName = "taskcategory",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.taskcategory
            };
            para[54] = new SqlParameter
            {
                ParameterName = "SPHandle",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Handle
            };
            para[55] = new SqlParameter
            {
                ParameterName = "SPRemarks",
                SqlDbType = SqlDbType.Text,
                Value = objPropCustomer.SRemarks
            };

            para[56] = new SqlParameter
            {
                ParameterName = "IsRenewalNotes",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.IsRenewalNotes
            };
            para[57] = new SqlParameter
            {
                ParameterName = "RenewalNotes",
                SqlDbType = SqlDbType.Text,
                Value = objPropCustomer.RenewalNotes
            };
            para[58] = new SqlParameter
            {
                ParameterName = "tblGCandHomeOwner",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.tblGCandHomeOwner
            };
            para[59] = new SqlParameter
            {
                ParameterName = "PWIP",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.PWIP
            };
            para[60] = new SqlParameter
            {
                ParameterName = "UnrecognizedRevenue",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.UnrecognizedRevenue
            };
            para[61] = new SqlParameter
            {
                ParameterName = "UnrecognizedExpense",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.UnrecognizedExpense
            };
            para[62] = new SqlParameter
            {
                ParameterName = "RetainageReceivable",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.RetainageReceivable
            };
            para[63] = new SqlParameter
            {
                ParameterName = "ArchitectName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.ArchitectName
            };
            para[64] = new SqlParameter
            {
                ParameterName = "ArchitectAdress",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.ArchitectAdress
            };
            para[65] = new SqlParameter
            {
                ParameterName = "PType",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.PType
            };
            para[66] = new SqlParameter
            {
                ParameterName = "JobAmount",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Amount
            };
            if (objPropCustomer.DtBOMCannotDelete != null)
            {
                para[67] = new SqlParameter
                {
                    ParameterName = "BomItemCannotDelete",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtBOMCannotDelete
                };
            }
            para[68] = new SqlParameter
            {
                ParameterName = "ProjectManagerUserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectManagerUserID
            };
            para[69] = new SqlParameter
            {
                ParameterName = "AssignedProjectUserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.AssignedProjectUserID
            };
            para[70] = new SqlParameter
            {
                ParameterName = "UpdatedByUserId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.UserID
            };

            para[71] = new SqlParameter
            {
                ParameterName = "TargetHPermission",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TargetHPermission
            };

            para[72] = new SqlParameter
            {
                ParameterName = "GroupIds",
                SqlDbType = SqlDbType.VarChar,
                Value = groupIds
            };
            para[73] = new SqlParameter
            {
                ParameterName = "SupervisorUserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.SupervisorUserID
            };

            para[74] = new SqlParameter
            {
                ParameterName = "ProjectStageID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectStageID
            };

            para[75] = new SqlParameter
            {
                ParameterName = "JBillingDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.JBillingDate
            };

            para[76] = new SqlParameter
            {
                ParameterName = "JPeriodDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.JPeriodDate
            };

            para[77] = new SqlParameter
            {
                ParameterName = "JRevisionDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.JRevisionDate
            };

            para[78] = new SqlParameter
            {
                ParameterName = "IRemarks",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.IRemarks
            };

            para[79] = new SqlParameter
            {
                ParameterName = "ExpectedClosingDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.ExpectedClosingDate
            };

            para[80] = new SqlParameter
            {
                ParameterName = "CloseDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.CloseDate
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProject_New2021", para);
                return Convert.ToInt32(para[42].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update_New2021(Customer objPropCustomer, string groupIds)
        {
            var para = new SqlParameter[81];

            para[0] = new SqlParameter
            {
                ParameterName = "job",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectJobID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "owner",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.CustomerID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "loc",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[4] = new SqlParameter
            {
                ParameterName = "status",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Status
            };
            para[5] = new SqlParameter
            {
                ParameterName = "type",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Type
            };
            para[6] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[7] = new SqlParameter
            {
                ParameterName = "ctype",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.ctypeName
            };
            para[8] = new SqlParameter
            {
                ParameterName = "ProjCreationDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.ProjectCreationDate
            };
            para[9] = new SqlParameter
            {
                ParameterName = "PO",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.PO
            };
            para[10] = new SqlParameter
            {
                ParameterName = "SO",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.SO
            };
            para[11] = new SqlParameter
            {
                ParameterName = "Certified",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Certified
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Custom1",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom1
            };
            para[13] = new SqlParameter
            {
                ParameterName = "Custom2",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom2
            };
            para[14] = new SqlParameter
            {
                ParameterName = "Custom3",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom3
            };
            para[15] = new SqlParameter
            {
                ParameterName = "Custom4",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom4
            };
            if (objPropCustomer.Custom5 != DateTime.MinValue)
            {
                para[16] = new SqlParameter
                {
                    ParameterName = "Custom5",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropCustomer.Custom5
                };
            }
            para[17] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[18] = new SqlParameter
            {
                ParameterName = "RolName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RolName
            };
            para[19] = new SqlParameter
            {
                ParameterName = "city",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.City
            };
            para[20] = new SqlParameter
            {
                ParameterName = "state",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.State
            };
            para[21] = new SqlParameter
            {
                ParameterName = "zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Zip
            };
            para[22] = new SqlParameter
            {
                ParameterName = "country",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Country
            };
            para[23] = new SqlParameter
            {
                ParameterName = "phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[24] = new SqlParameter
            {
                ParameterName = "cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[25] = new SqlParameter
            {
                ParameterName = "fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[26] = new SqlParameter
            {
                ParameterName = "contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[27] = new SqlParameter
            {
                ParameterName = "email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[28] = new SqlParameter
            {
                ParameterName = "rolRemarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RolRemarks
            };
            para[29] = new SqlParameter
            {
                ParameterName = "rolType",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.RolType
            };
            para[30] = new SqlParameter
            {
                ParameterName = "InvExp",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.InvExp
            };
            para[31] = new SqlParameter
            {
                ParameterName = "InvServ",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.InvServ
            };
            para[32] = new SqlParameter
            {
                ParameterName = "Wage",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Wage
            };
            para[33] = new SqlParameter
            {
                ParameterName = "GLInt",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.GLInt
            };
            para[34] = new SqlParameter
            {
                ParameterName = "jobtCType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.JobTempCtype
            };
            para[35] = new SqlParameter
            {
                ParameterName = "Post",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Post
            };
            para[36] = new SqlParameter
            {
                ParameterName = "Charge",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Charge
            };
            para[37] = new SqlParameter
            {
                ParameterName = "JobClose",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.JobClose
            };
            para[38] = new SqlParameter
            {
                ParameterName = "fInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.fInt
            };
            para[39] = new SqlParameter
            {
                ParameterName = "TeamItems",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtTeam
            };


            if (objPropCustomer.DtBOM != null)
            {
                para[40] = new SqlParameter
                {
                    ParameterName = "BomItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtBOM
                };
            }
            if (objPropCustomer.dtItems != null)
            {
                para[41] = new SqlParameter
                {
                    ParameterName = "Items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
            }
            para[42] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            if (objPropCustomer.DtMilestone != null)
            {
                if (objPropCustomer.DtMilestone.Rows.Count > 0)
                {
                    para[43] = new SqlParameter
                    {
                        ParameterName = "MilestonItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = objPropCustomer.DtMilestone
                    };
                }
            }
             
           
            if (objPropCustomer.DtCustom != null)
            {
                if (objPropCustomer.DtCustom.Rows.Count > 0)
                {
                    para[44] = new SqlParameter
                    {
                        ParameterName = "CustomItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = objPropCustomer.DtCustom
                    };
                }
            }
            para[45] = new SqlParameter
            {
                ParameterName = "BillRate",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.BillRate
            };
            para[46] = new SqlParameter
            {
                ParameterName = "RateOT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateOT
            };
            para[47] = new SqlParameter
            {
                ParameterName = "RateNT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateNT
            };
            para[48] = new SqlParameter
            {
                ParameterName = "RateDT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateDT
            };
            para[49] = new SqlParameter
            {
                ParameterName = "RateTravel",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateTravel
            };
            para[50] = new SqlParameter
            {
                ParameterName = "Mileage",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Mileage
            };
            para[51] = new SqlParameter
            {
                ParameterName = "rolid",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.RoleID
            };
            para[52] = new SqlParameter
            {
                ParameterName = "TaskCodes",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.dtTaskCode
            };
            para[53] = new SqlParameter
            {
                ParameterName = "taskcategory",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.taskcategory
            };
            para[54] = new SqlParameter
            {
                ParameterName = "SPHandle",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Handle
            };
            para[55] = new SqlParameter
            {
                ParameterName = "SPRemarks",
                SqlDbType = SqlDbType.Text,
                Value = objPropCustomer.SRemarks
            };

            para[56] = new SqlParameter
            {
                ParameterName = "IsRenewalNotes",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.IsRenewalNotes
            };
            para[57] = new SqlParameter
            {
                ParameterName = "RenewalNotes",
                SqlDbType = SqlDbType.Text,
                Value = objPropCustomer.RenewalNotes
            };
            para[58] = new SqlParameter
            {
                ParameterName = "tblGCandHomeOwner",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.tblGCandHomeOwner
            };
            para[59] = new SqlParameter
            {
                ParameterName = "PWIP",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.PWIP
            };
            para[60] = new SqlParameter
            {
                ParameterName = "UnrecognizedRevenue",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.UnrecognizedRevenue
            };
            para[61] = new SqlParameter
            {
                ParameterName = "UnrecognizedExpense",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.UnrecognizedExpense
            };
            para[62] = new SqlParameter
            {
                ParameterName = "RetainageReceivable",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.RetainageReceivable
            };
            para[63] = new SqlParameter
            {
                ParameterName = "ArchitectName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.ArchitectName
            };
            para[64] = new SqlParameter
            {
                ParameterName = "ArchitectAdress",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.ArchitectAdress
            };
            para[65] = new SqlParameter
            {
                ParameterName = "PType",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.PType
            };
            para[66] = new SqlParameter
            {
                ParameterName = "JobAmount",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Amount
            };
         
            para[68] = new SqlParameter
            {
                ParameterName = "ProjectManagerUserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectManagerUserID
            };
            para[69] = new SqlParameter
            {
                ParameterName = "AssignedProjectUserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.AssignedProjectUserID
            };
            para[70] = new SqlParameter
            {
                ParameterName = "UpdatedByUserId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.UserID
            };

            para[71] = new SqlParameter
            {
                ParameterName = "TargetHPermission",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TargetHPermission
            };

            para[72] = new SqlParameter
            {
                ParameterName = "GroupIds",
                SqlDbType = SqlDbType.VarChar,
                Value = groupIds
            };
            para[73] = new SqlParameter
            {
                ParameterName = "SupervisorUserID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.SupervisorUserID
            };

            para[74] = new SqlParameter
            {
                ParameterName = "ProjectStageID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectStageID
            };

            para[75] = new SqlParameter
            {
                ParameterName = "JBillingDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.JBillingDate
            };

            para[76] = new SqlParameter
            {
                ParameterName = "JPeriodDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.JPeriodDate
            };

            para[77] = new SqlParameter
            {
                ParameterName = "JRevisionDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.JRevisionDate
            };

            para[78] = new SqlParameter
            {
                ParameterName = "IRemarks",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.IRemarks
            };

            para[79] = new SqlParameter
            {
                ParameterName = "ExpectedClosingDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.ExpectedClosingDate
            };

            para[80] = new SqlParameter
            {
                ParameterName = "CloseDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.CloseDate
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateProject_New2021", para);
                return Convert.ToInt32(para[42].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet GetAllJobType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID,Type,Count,Color,Remarks,IsDefault FROM JobType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID,Type,Count,Color,Remarks,IsDefault FROM JobType Order by Type");// Where ID <> 0
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetContractType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT Type, fDesc, MatCharge, Reg, OT,DT, WReg, WOT, WDT, WCharge, HReg, HOT, HDT, HCharge, Count, Remarks, Serv, LTest, NT, Travel, fOver, WNT, HNT, NonContract, Free, FGL, En FROM LType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvService(JobT _objJob)
        {
            try
            {
                if (!string.IsNullOrEmpty(_objJob.SearchValue))
                    return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label FROM Inv WHERE Type = 1 AND Status = 0 AND Name like '%" + _objJob.SearchValue + "%' ORDER BY Name");
                else
                    return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label FROM Inv WHERE Type = 1 AND Status = 0 ORDER BY Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvService_TypeZero(JobT _objJob)
        {
            try
            {
                int Type = 0;
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label FROM Inv WHERE Type = " + Type.ToString() + " AND Status = 0 ORDER BY Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPosting(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID, Post FROM Posting");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCode(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID as value, Code as label ,Code as CodeDesc FROM JobCode");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetDeptID(string ConnConfig, string DeptName)
        {
            try
            {  
                return  Convert.ToInt32(SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "select isnull((select top 1 ID from jobtype where type='"+ DeptName + "'),0)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobCodebyDept(JobT _objJob,int DeptID=0)
        {
            try
            {

                var param = new SqlParameter[2];

                param[0] = new SqlParameter
                {
                    ParameterName = "DeptID",
                    SqlDbType = SqlDbType.Int,
                    Value = DeptID
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "Searchvalues",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _objJob.SearchValue
                };
             

                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.StoredProcedure, "SpGetJobCodeBy_DeptID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetGroup(JobT _objJob)
        {
            try
            {
                var param = new SqlParameter[2];

                param[0] = new SqlParameter
                {
                    ParameterName = "ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objJob.ID
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _objJob.SearchValue
                };

                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.StoredProcedure, "SpGetGroup", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetGroupImport(JobT _objJob)
        {
            try
            {
                var param = new SqlParameter[2];

                param[0] = new SqlParameter
                {
                    ParameterName = "ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objJob.ID
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _objJob.SearchValue
                };

                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.StoredProcedure, "SpGetGroupImport", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLocGroupNotInProj(JobT _objJob)
        {
            try
            {
                var param = new SqlParameter[3];

                param[0] = new SqlParameter
                {
                    ParameterName = "locID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objJob.ID
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "projectID",
                    SqlDbType = SqlDbType.Int,
                    Value = _objJob.Job
                };
                param[2] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _objJob.SearchValue
                };

                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.StoredProcedure, "spGetLocGroupNotInProj", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllUM(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID as value, fDesc as label FROM UM");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllInvDetails(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label FROM Inv WHERE Status = 0 ORDER BY Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID, Department, ID as value, Department as label FROM OrgDep ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobStatus(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, " SELECT IDENTITY (INT, 0, 1) AS ID, Status INTO #tempJStatus FROM [JStatus] SELECT * FROM #tempJStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet spGetJobStatus(JobT _objJob)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objJob.ConnConfig, "spGetJobStatus", _objJob.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetApplicationStatus(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "Select Id,StatusName from ApplicationStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobTFinanceByID(JobT _objJob)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT j.ID, j.fDesc,   \n");
                varname.Append("         j.InvExp, c.fDesc as InvExpName, j.InvServ, i.Name AS InvServiceName, j.Wage, p.fDesc as WageName,     \n");
                varname.Append("         j.GLInt, j.CType, j.Post, j.Charge, j.fInt, j.JobClose,\n");
                varname.Append("         (SELECT c.fdesc from JobT j left join Chart c on j.GLInt = c.ID where j.ID=" + _objJob.ID + " ) as GLName \n");
                varname.Append("         ,J.[UnrecognizedRevenue],J.[UnrecognizedExpense],J.[RetainageReceivable] \n");
                varname.Append("         ,UnrecognizedRevenueName = (Select i1.Name from Inv i1 where i1.id = J.UnrecognizedRevenue)  \n");
                varname.Append("         ,UnrecognizedExpenseName = (Select i1.fDesc from Chart i1 where i1.id = J.UnrecognizedExpense) \n");
                varname.Append("         ,RetainageReceivableName = (Select i1.fDesc from Chart i1 where i1.id = J.RetainageReceivable) \n");
                varname.Append("    FROM JobT j LEFT JOIN   \n");
                varname.Append("        PRWage p on j.Wage = p.ID LEFT JOIN     \n");
                varname.Append("        Inv i on j.InvServ = i.ID LEFT JOIN     \n");
                varname.Append("        Chart c on j.InvExp = c.ID              \n");
                varname.Append("        	WHERE j.id=" + _objJob.ID + "           \n");
                //varname.Append("     \n");
                //varname.Append("    SELECT j.JobTID, t.* \n");
                //varname.Append("        FROM tblCustomJobT j INNER JOIN tblCustomFields t \n");
                //varname.Append("        ON t.ID = j.tblCustomFieldsID WHERE j.JobTID = " + _objJob.ID + "  \n");
                //varname.Append("     \n");
                //varname.Append("    SELECT j.JobTID, t.* \n");
                //varname.Append("        FROM tblCustomJobT j INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID\n");
                //varname.Append("        RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID WHERE j.JobTID = " + _objJob.ID + "  \n");
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBomType(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID, Type FROM BOMT");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBomType(GetBomTypeParam _GetBomTypeParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ID, Type FROM BOMT");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet spSetTargetHours(string ConnConfig , int ProjectId , string Code , string GroupName , string TargetHours, int HoursReduce, int isMassupdatetargetedhoursby, int isCopytargetedhoursoverbudgethours, int isMassupdatetargeted)
        { 
            try
            {
                return   SqlHelper.ExecuteDataset(ConnConfig, "spSetTargetHours", ProjectId , Code , GroupName , TargetHours, HoursReduce, isMassupdatetargetedhoursby, isCopytargetedhoursoverbudgethours, isMassupdatetargeted);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet spGetTargetHours(string ConnConfig, int ProjectId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnConfig, "spGetTargetHours", ProjectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTabByPageUrl(JobT _objJob)
        {
            try
            {
                // Removing Task tab out of the Tab list on project workflow
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT t.ID, t.tblPageID, t.TabName FROM tblTabs t WHERE t.TabName != 'Task'");
                //return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT t.ID, t.tblPageID, t.TabName FROM tblTabs t ");
                //return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT t.ID, t.tblPageID, t.TabName FROM tblTabs t INNER JOIN tblPages p ON p.ID = t.tblPageID WHERE p.URL = '" + _objJob.PageUrl + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRecurringCustom(JobT objJob)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "JobId";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objJob.Job;

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetRecurCustomFields", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetRecurringCustom(GetRecurringCustomParam _GetRecurringCustom, string ConnectionString)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "JobId";
                param.SqlDbType = SqlDbType.Int;
                param.Value = _GetRecurringCustom.Job;

                return _GetRecurringCustom.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetRecurCustomFields", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetRecurringJobCustom(JobT _objJob)
        //{
        //    StringBuilder QueryText = new StringBuilder();
        //    QueryText.Append("        SELECT jt.ID AS JobT, t.* ,     \n");
        //    QueryText.Append("         		(CASE t.Format WHEN 1 THEN 'Currency'    \n");
        //    QueryText.Append("                  WHEN 2 THEN 'Date'                \n");
        //    QueryText.Append("        		    WHEN 3 THEN 'Text'              \n");
        //    QueryText.Append("     		        WHEN 4 THEN 'Dropdown'                    \n");
        //    QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl     \n");
        //    QueryText.Append("       	        ,j.Value as Value    \n");
        //    QueryText.Append("       	 FROM tblCustomJob j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID    \n");
        //    QueryText.Append("           INNER JOIN Job jt ON jt.ID = j.JobID   \n");
        //    QueryText.Append("           WHERE jt.ID = '" + _objJob.ID + "' AND (t.IsDeleted is null OR t.IsDeleted = 0)  \n");
        //    QueryText.Append("              \n");
        //    QueryText.Append("      SELECT jt.ID AS JobT, t.*, tc.Label, tc.Format, tc.tblTabID,  \n");
        //    QueryText.Append("      		(CASE tc.Format WHEN 1 THEN 'Currency'  \n");
        //    QueryText.Append("                  WHEN 2 THEN 'Date'    \n");
        //    QueryText.Append("      	        WHEN 3 THEN 'Text'    \n");
        //    QueryText.Append("     	            WHEN 4 THEN 'Dropdown'   \n");
        //    QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl   \n");
        //    QueryText.Append("     	    FROM tblCustomJob j INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID   \n");
        //    QueryText.Append("          RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID  \n");
        //    QueryText.Append("          INNER JOIN Job jt ON jt.ID = j.JobID               \n");
        //    QueryText.Append("          WHERE jt.ID = " + _objJob.ID + " AND (tc.IsDeleted is null OR tc.IsDeleted = 0)      \n");
        //    try
        //    {
        //        return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, QueryText.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public bool IsExistRecurrJobT(JobT _objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("      SELECT CAST(Count(*) AS BIT) FROM JobT WHERE TYPE = 0 AND Status = 0 \n");
                if (!_objJob.ID.Equals(0))
                {
                    QueryText.Append("   AND ID!='" + _objJob.ID + "'       \n");
                }
                return _objJob.IsExistRecurr = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.Text, QueryText.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectTemplateCustomFields(JobT objJob)
        {
            try
            {
                var param = new SqlParameter[2];

                param[0] = new SqlParameter
                {
                    ParameterName = "jobt",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.ID
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetProjectTemplateCustomFields", param);

                //StringBuilder QueryText = new StringBuilder();
                //QueryText.Append("        SELECT jt.ID AS JobT,jt.type, t.* ,     \n");
                //QueryText.Append("         		(CASE t.Format WHEN 1 THEN 'Currency'    \n");
                //QueryText.Append("                  WHEN 2 THEN 'Date'                \n");
                //QueryText.Append("        		    WHEN 3 THEN 'Text'              \n");
                //QueryText.Append("     		        WHEN 4 THEN 'Dropdown'                    \n");
                //QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl     \n");
                //QueryText.Append("       	        ,j.Value as Value    \n");
                //QueryText.Append("       	 FROM tblCustomJobT j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID    \n");
                //QueryText.Append("           INNER JOIN JobT jt ON jt.ID = j.JobTID   \n");
                //QueryText.Append("           WHERE jt.ID = '" + _objJob.ID + "' AND (t.IsDeleted is null OR t.IsDeleted = 0)   \n");
                //QueryText.Append("              \n");
                //QueryText.Append("      SELECT jt.ID AS JobT, t.*, tc.Label, tc.Format, tc.tblTabID,  \n");
                //QueryText.Append("      		(CASE tc.Format WHEN 1 THEN 'Currency'  \n");
                //QueryText.Append("                  WHEN 2 THEN 'Date'    \n");
                //QueryText.Append("      	        WHEN 3 THEN 'Text'    \n");
                //QueryText.Append("     	            WHEN 4 THEN 'Dropdown'   \n");
                //QueryText.Append("     	            WHEN 5 THEN 'Checkbox' END) AS FieldControl   \n");
                //QueryText.Append("     	    FROM tblCustomJobT j INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID   \n");
                //QueryText.Append("          RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID  \n");
                //QueryText.Append("          INNER JOIN JobT jt ON jt.ID = j.JobTID               \n");
                //QueryText.Append("          WHERE jt.ID = " + _objJob.ID + " AND (tc.IsDeleted is null OR tc.IsDeleted = 0)     \n");
                //return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectCustomTab_BK(JobT _objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("        SELECT t.tblTabID \n");
                QueryText.Append("         		FROM tblCustomJobT j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID      \n");
                QueryText.Append("              INNER JOIN JobT jt ON jt.ID = j.JobTID                   \n");
                QueryText.Append("        		WHERE jt.ID = '" + _objJob.ID + "' AND (t.IsDeleted is null OR t.IsDeleted = 0)               \n");
                QueryText.Append("     		    GROUP by tblTabID                   \n");
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectCustomTab(JobT _objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                if (_objJob.Job != 0)
                {
                    QueryText.Append("        SELECT cj.tblTabID  \n");
                    QueryText.Append("         		FROM tblCustomJob cj     \n");
                    QueryText.Append("        		WHERE cj.JobID = '" + _objJob.Job + "' \n");
                    QueryText.Append("     		    GROUP by cj.tblTabID                   \n");
                }
                else
                {
                    QueryText.Append("        SELECT t.tblTabID \n");
                    QueryText.Append("         		FROM tblCustomJobT j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID      \n");
                    QueryText.Append("              INNER JOIN JobT jt ON jt.ID = j.JobTID                   \n");
                    QueryText.Append("        		WHERE jt.ID = '" + _objJob.ID + "' AND (t.IsDeleted is null OR t.IsDeleted = 0)               \n");
                    QueryText.Append("     		    GROUP by tblTabID                   \n");
                }
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExistProjectTempByType(JobT _objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("      SELECT CAST(Count(*) AS BIT) FROM JobT ");
                QueryText.Append("      WHERE Type = '" + _objJob.Type + "' AND ID = '" + _objJob.ID + "' ");
                return _objJob.IsExist = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.Text, QueryText.ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobTById(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT * FROM JobT WHERE ID='" + _objJob.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataByUM(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT ID as value, fDesc as label FROM UM WHERE fDesc = '" + objJob.UM + "'    ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostByJob(JobT objJob)
        {
            try
            {
                SqlParameter jobParam = new SqlParameter();
                jobParam.ParameterName = "job";
                jobParam.SqlDbType = SqlDbType.Int;
                jobParam.Value = objJob.Job;

                SqlParameter sdateParam = new SqlParameter();
                sdateParam.ParameterName = "sdate";
                if (objJob.StartDate != DateTime.MinValue)
                {
                    sdateParam.SqlDbType = SqlDbType.DateTime;
                    sdateParam.Value = objJob.StartDate;
                }

                SqlParameter edateParam = new SqlParameter();
                edateParam.ParameterName = "edate";
                if (objJob.EndDate != DateTime.MinValue)
                {
                    edateParam.SqlDbType = SqlDbType.DateTime;
                    edateParam.Value = objJob.EndDate;
                }

                SqlParameter PageIndexParam = new SqlParameter();
                PageIndexParam.ParameterName = "PageIndex";
                PageIndexParam.SqlDbType = SqlDbType.Int;
                PageIndexParam.Value = objJob.PageIndex;

                SqlParameter PageSizeParam = new SqlParameter();
                PageSizeParam.ParameterName = "PageSize";
                PageSizeParam.SqlDbType = SqlDbType.Int;
                PageSizeParam.Value = objJob.PageSize;

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostByJob", jobParam, sdateParam, edateParam, PageIndexParam, PageSizeParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobCostByJobReport(JobT objJob)
        {
            try
            {
                SqlParameter jobParam = new SqlParameter();
                jobParam.ParameterName = "job";
                jobParam.SqlDbType = SqlDbType.Int;
                jobParam.Value = objJob.Job;

                SqlParameter sdateParam = new SqlParameter();
                sdateParam.ParameterName = "sdate";
                if (objJob.StartDate != DateTime.MinValue)
                {
                    sdateParam.SqlDbType = SqlDbType.DateTime;
                    sdateParam.Value = objJob.StartDate;
                }

                SqlParameter edateParam = new SqlParameter();
                edateParam.ParameterName = "edate";
                if (objJob.EndDate != DateTime.MinValue)
                {
                    edateParam.SqlDbType = SqlDbType.DateTime;
                    edateParam.Value = objJob.EndDate;
                }

                SqlParameter PageIndexParam = new SqlParameter();
                PageIndexParam.ParameterName = "PageIndex";
                PageIndexParam.SqlDbType = SqlDbType.Int;
                PageIndexParam.Value = objJob.PageIndex;

                SqlParameter PageSizeParam = new SqlParameter();
                PageSizeParam.ParameterName = "PageSize";
                PageSizeParam.SqlDbType = SqlDbType.Int;
                PageSizeParam.Value = objJob.PageSize;

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostByJobReport", jobParam, sdateParam, edateParam, PageIndexParam, PageSizeParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTypeItemByExpCode(JobT objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                //QueryText.Append("     SELECT '" + objJob.Code + "' as Code , b.Type as Item, 1 as Type, 'cost' as TypeName,      \n");
                QueryText.Append("      SELECT '" + objJob.Code + "' as Code, b.type as ItemTypeId, '" + objJob.Type + "' as Type,   \n");
                QueryText.Append("          bt.Type as Item,        \n");
                QueryText.Append("          'Cost' as TypeName,     \n");
                QueryText.Append("      isnull(sum(j.Budget),0) as TotalBudget, isnull(sum(j.Actual),0) as TotalActual, isnull(sum(j.Budget),0) - isnull(sum(j.Actual),0) as TotalVariance,   \n");
                QueryText.Append("      0.00 as TotalCommited, 0.00 as TotalOutstand      \n");
                QueryText.Append("      FROM JobTItem j         \n");
                QueryText.Append("      INNER JOIN Bom b ON j.ID = b.JobTItemID     \n");
                QueryText.Append("          LEFT JOIN BOMT bt on b.Type = bt.ID     \n");
                QueryText.Append("          WHERE j.Type = 1 and j.Job = '" + objJob.Job + "'        \n");
                QueryText.Append("          and j.code = '" + objJob.Code + "'      \n");
                QueryText.Append("          GROUP by b.Type, bt.Type    \n");
                QueryText.Append("          ORDER BY bt.Type");
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistExpJobItemByJob(JobT objJob)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objJob.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM JobI as ji INNER JOIN JobTItem as j ON j.Job = ji.Job AND j.Line = ji.Phase INNER JOIN Trans as t ON ji.TransID = t.ID WHERE ji.Job = '" + objJob.Job + "' AND ji.Phase = '" + objJob.Phase + "' and j.Type = 1 and t.Type = 41)THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsNotDeleteBomType(JobT objJob)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objJob.ConnConfig, CommandType.Text, "SELECT CAST(CASE WHEN EXISTS (Select * FROM BOM b INNER JOIN JobTItem j ON b.JobTItemID = j.ID WHERE j.Job = '" + objJob.Job + "' and j.Type=2  and b.JobTItemID = '" + objJob.JobTItemId + "' )THEN 5 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistRevJobItemByJob(JobT objJob)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(objJob.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM JobI as ji INNER JOIN JobTItem as j ON j.Job = ji.Job AND j.Line = ji.Phase INNER JOIN Trans as t ON ji.TransID = t.ID WHERE ji.Job = '" + objJob.Job + "' AND ji.Phase = '" + objJob.Phase + "' and j.Type = 0 and (t.Type = 2 or t.Type = 4))THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRevenueJobItemsByJob(JobT objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("      SELECT Top 1 *                                \n");
                QueryText.Append("          FROM JobTItem as jobt               \n");
                QueryText.Append("          INNER JOIN Milestone as m           \n");
                QueryText.Append("              ON jobt.ID = m.JobTItemID       \n");
                QueryText.Append("              WHERE jobt.Type = 0 AND jobt.Line='" + objJob.Line + "' AND jobt.Job = '" + objJob.Job + "'    \n");

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int16 AddBOMItem(JobT objJob)
        {
            try
            {
                var param = new SqlParameter[12];

                param[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                param[1] = new SqlParameter
                {
                    ParameterName = "fdesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.fDesc
                };
                param[2] = new SqlParameter
                {
                    ParameterName = "code",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.Code
                };
                param[3] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                param[4] = new SqlParameter
                {
                    ParameterName = "item",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.ItemID
                };
                param[5] = new SqlParameter
                {
                    ParameterName = "qty",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.QtyReq
                };
                param[6] = new SqlParameter
                {
                    ParameterName = "um",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.UM
                };
                param[7] = new SqlParameter
                {
                    ParameterName = "scrapfactor",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.ScrapFact
                };
                param[8] = new SqlParameter
                {
                    ParameterName = "budgetunit",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.BudgetUnit
                };
                param[9] = new SqlParameter
                {
                    ParameterName = "budgetext",
                    SqlDbType = SqlDbType.Float,
                    Value = objJob.BudgetExt
                };
                param[10] = new SqlParameter
                {
                    ParameterName = "IsDefault",
                    SqlDbType = SqlDbType.Bit,
                    Value = objJob.IsDefault
                };
                param[11] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spAddBOMItem", param);
                objJob.Line = Convert.ToInt16(param[10].Value);
                return objJob.Line;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostCodeByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostCodeByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInventoryItem(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT *,ID as value, Name as label,  ID as MatItem, Name as MatDesc FROM Inv WHERE Status = 0 AND (Type = 0 OR Type = 2) and Name is not null ORDER BY Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInventoryItemSearch(JobT objJob)
        {
            try
            {
                if(objJob.SearchValue=="")
                {
                    return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT top 100 *,ID as value, Name as label,  ID as MatItem, Name as MatDesc FROM Inv WHERE Status = 0 AND (Type = 0 OR Type = 2) and Name is not null ORDER BY Name");
                }
                else
                {
                    return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT top 100 *,ID as value, Name as label,  ID as MatItem, Name as MatDesc FROM Inv WHERE Status = 0 AND (Type = 0 OR Type = 2) and Name is not null AND (Name like '%"+ objJob.SearchValue + "%' OR fDesc like '%"+ objJob.SearchValue + "%') ORDER BY Name");
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInventoryItemProject(String ConnConfig, String Text)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(ConnConfig, CommandType.Text, "SELECT TOP 50  ID as MatItem, Name as MatDesc, fDesc FROM Inv WHERE Status = 0 AND (Type = 0 OR Type = 2) AND Name like '%" + Text + "%' ORDER BY Name");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvById(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT * FROM Inv WHERE ID='" + _objJob.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLabourMaterial(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT *,ID as MatItem,fDesc as MatDesc  FROM PRWage WHERE Status = 0 ORDER BY fDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLabourMaterialProject(String ConnConfig, String Text)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(ConnConfig, CommandType.Text, "SELECT TOP 50 ID as MatItem,fDesc as MatDesc,fDesc  FROM PRWage WHERE Status = 0 AND fDesc like '%" + Text + "%' ORDER BY fDesc");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobCostTypeByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "code",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.Code
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostTypeByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostInvoicesByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[5];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "phase",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Phase
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                if (objJob.StartDate != DateTime.MinValue)
                {
                    para[3] = new SqlParameter
                    {
                        ParameterName = "sdate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.StartDate
                    };
                }
                if (objJob.EndDate != DateTime.MinValue)
                {
                    para[4] = new SqlParameter
                    {
                        ParameterName = "edate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.EndDate
                    };
                }
                //para[3] = new SqlParameter          // Bom.Type or Milestone.Type Id
                //{
                //    ParameterName = "typeId",
                //    SqlDbType = SqlDbType.Int,
                //    Value = objJob.TypeId
                //};
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostInvoicesByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet spGetProjectBudgetSummaryDetail(string ConnConfig, int jobID,string Type,  string StartDate = "", string EndDate = "")
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = jobID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = Type
                };

                if (StartDate != "")
                {
                    para[2] = new SqlParameter
                    {
                        ParameterName = "sdate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = StartDate
                    };
                }
                if (EndDate != "")
                {
                    para[3] = new SqlParameter
                    {
                        ParameterName = "edate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = EndDate
                    };
                }

                return SqlHelper.ExecuteDataset(ConnConfig, CommandType.StoredProcedure, "spGetProjectBudgetSummaryDetail", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetFinance_Budget_Grid_Popup_ByJob(string ConnConfig, int jobID, int PhaseID, int TypeID, string StartDate="", string EndDate="")
        {
            try
            {
                var para = new SqlParameter[5];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = jobID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "phase",
                    SqlDbType = SqlDbType.Int,
                    Value = PhaseID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = TypeID
                };
                if (StartDate !="")
                {
                    para[3] = new SqlParameter
                    {
                        ParameterName = "sdate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value =  StartDate
                    };
                }
                if (EndDate != "")
                {
                    para[4] = new SqlParameter
                    {
                        ParameterName = "edate",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = EndDate
                    };
                }
                
                return SqlHelper.ExecuteDataset(ConnConfig, CommandType.StoredProcedure, "SpGetFinance_Budget_Grid_Popup_ByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBOMTByTypeName(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT top 1 ID AS Value, Type AS Label FROM BOMT WHERE Type like '%" + objJob.TypeName + "%'   ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public DataSet GetPhaseExpByJobType(JobT objJob) // get phase expense type details
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@SearchText",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.SearchValue
                };

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, "spGetPhaseExpByJobType", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetPhaseExpByJobTypePO(JobT objJob) // get phase expense type details
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@SearchText",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.SearchValue
                };

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, "spGetPhaseExpByJobTypePO", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Need to check again, the sp spGetPhaseExpByJobTypeOpSequence is not exist
        public DataSet GetPhaseExpByJobTypeOpSequence(JobT objJob) // get phase expense type details
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objJob.Type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@SearchText",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.SearchValue
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@Opsq",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Opsq
                };

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, "spGetPhaseExpByJobTypeOpSequence", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostTicketsByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[5];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "phase",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Phase
                };
                para[2] = new SqlParameter          // Bom.Type 
                {
                    ParameterName = "type",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Type
                };
                if (objJob.StartDate != DateTime.MinValue)
                {
                    para[3] = new SqlParameter
                    {
                        ParameterName = "sdate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.StartDate
                    };
                }
                if (objJob.EndDate != DateTime.MinValue)
                {
                    para[4] = new SqlParameter
                    {
                        ParameterName = "edate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.EndDate
                    };
                }
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostTicketsByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostJEByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "phase",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Phase
                };
                if (objJob.StartDate != DateTime.MinValue)
                {
                    para[2] = new SqlParameter
                    {
                        ParameterName = "sdate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.StartDate
                    };
                }
                if (objJob.EndDate != DateTime.MinValue)
                {
                    para[3] = new SqlParameter
                    {
                        ParameterName = "edate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.EndDate
                    };
                }
                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostJEByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBudgetSummaryGridDataByJob(JobT objJob , string StartDate="",string EndDate="")
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "job";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objJob.Job;

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "StartDate";
                param1.SqlDbType = SqlDbType.NVarChar;
                param1.Value = StartDate;

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "EndDate";
                param2.SqlDbType = SqlDbType.NVarChar;
                param2.Value = EndDate;

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetBudgetByJob", param, param1, param2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContactForJob(JobT objJob, Int32 IsSelesAsigned = 0)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Job";
                param.SqlDbType = SqlDbType.Int;
                param.Value = objJob.Job;


                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@IsSalesAsigned";
                param1.SqlDbType = SqlDbType.Int;
                param1.Value = IsSelesAsigned;

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetContactForJob", param, param1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddJoinPhoneJob(JobT objJob, int PhoneID, int IsHighLighted)
        {
            try
            {


                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobID",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@PhoneID",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = PhoneID
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@IsHighLighted",
                    SqlDbType = SqlDbType.VarChar,
                    Value = @IsHighLighted
                };

                SqlHelper.ExecuteNonQuery(objJob.ConnConfig, CommandType.StoredProcedure, "spAddJoinPhoneJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllJobTypeForSearch(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobTypeForWIP(JobT _objJob)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objJob.EndDate
                };

                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobTypeForWIP", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllJobTypeForAjaxSearch(int type)
        {


            DataSet ds = new DataSet();


            try
            {

                ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["config"].ToString(), "spGetProjectDetails", type);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public DataSet GetEstimateProjectTemplateByID(JobT _objJob)
        {


            DataSet ds = new DataSet();


            try
            {

                ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["config"].ToString(), "spGetEstimateProjectTemplateByID", _objJob.ID);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public DataSet GetBomTypeForEstimateCalculation(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT ID, Type FROM BOMT where BOMT.ID not in(select EstimateCalculationTemplate_BOMTID from EstimateCalculationTemplate where EstimateCalculationTemplate.EstimateCalculationTemplateIsBOM=1)");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAttachment(JobT _objJob, Int32 IsSalesAsigned = 0)
        {


            DataSet ds = new DataSet();


            try
            {

                ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, "spReadProjectAttachment", _objJob.TypeName, _objJob.Job, _objJob.sort, IsSalesAsigned);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public string GetAttachmentByID(JobT _objJob)
        {
            try
            {
                return SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.Text, "select Path from Documents where id=" + _objJob.docid).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetExpenseJobCost(JobT objJob)
        {
            try
            {
                SqlParameter jobParam = new SqlParameter();
                jobParam.ParameterName = "job";
                jobParam.SqlDbType = SqlDbType.Int;
                jobParam.Value = objJob.Job;

                SqlParameter PageIndexParam = new SqlParameter();
                PageIndexParam.ParameterName = "PageIndex";
                PageIndexParam.SqlDbType = SqlDbType.Int;
                PageIndexParam.Value = objJob.PageIndex;

                SqlParameter PageSizeParam = new SqlParameter();
                PageSizeParam.ParameterName = "PageSize";
                PageSizeParam.SqlDbType = SqlDbType.Int;
                PageSizeParam.Value = objJob.PageSize;

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "GetJobExpenseByJob", jobParam, PageIndexParam, PageSizeParam);


                //StringBuilder QueryText = new StringBuilder();
                //QueryText.Append("  SELECT isnull(Code,'') as Code,  	                \n");
                //QueryText.Append("          Line as Phase,                              \n");
                //QueryText.Append("          fDesc,                                      \n");
                //QueryText.Append("          (CASE Type WHEN 0 THEN 'Revenues' ELSE 'Costs' END) AS JobType,                        \n");
                //QueryText.Append("          Type,                                       \n");
                //QueryText.Append("          ISNULL(Actual,0)    As MatAct,              \n");
                //QueryText.Append("          ISNULL(Budget,0)    As MatBgt,              \n");
                //QueryText.Append("          ISNULL(Actual,0) - (ISNULL(Budget,0) + ISNULL(Modifier,0)) As MatDiff,                  \n");
                //QueryText.Append("          ISNULL(Modifier,0)  As MatMod,              \n");
                //QueryText.Append("          ISNULL(THours,0)    As HourAct,             \n");
                //QueryText.Append("          ISNULL(BHours,0)    As HourBgt,             \n");
                //QueryText.Append("          ISNULL(ETC,0)       As LaborBgt,            \n");
                //QueryText.Append("          ISNULL(ETCMod,0)    As LaborMod,            \n");
                //QueryText.Append("          ISNULL(Labor,0)     As LaborAct,            \n");
                //QueryText.Append("          ISNULL(Labor,0) - (ISNULL(ETC,0) + ISNULL(ETCMod,0)) As LabDiff                         \n");
                //QueryText.Append("      FROM JobTItem WHERE Type=1 AND Job='" + objJob.Job + "'                       \n");
                //QueryText.Append("      ORDER BY Line           \n");

                //return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetExpenseJobCostByDate(JobT objJob)
        {
            try
            {
                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("  SELECT	JobItem.Code,               \n");
                QueryText.Append("          JobItem.Line AS Phase,       \n");
                QueryText.Append("          JobItem.fDesc,               \n");
                QueryText.Append("          (CASE jobitem.Type WHEN 0 THEN 'Revenues' ELSE 'Costs' END) AS JobType,               \n");
                QueryText.Append("          JobItem.Type,                                                 \n");
                QueryText.Append("          Sum(CASE WHEN (JobI.Type = 1 AND (JobI.TransID > 0 or ISNULL(JobI.Labor,0) = 0)) THEN ");
                QueryText.Append("              ISNULL(JobI.Amount,0) ELSE 0 END) AS MatAct,        \n");
                QueryText.Append("          ISNULL(JobItem.Budget,0) As MatBgt,                 \n");
                QueryText.Append("          Sum(CASE WHEN (JobI.Type = 1 AND (JobI.TransID > 0 or ISNULL(JobI.Labor,0) = 0)) THEN ISNULL(JobI.Amount,0) ELSE 0 END) \n");
                QueryText.Append("              - (ISNULL(JobItem.Budget,0) + ISNULL(JobItem.Modifier,0)) As MatDiff,                  \n");

                QueryText.Append("          ISNULL(JobItem.Modifier,0) As MatMod,               \n");

                QueryText.Append("          ISNULL((SELECT  SUM(((ISNULL(Reg,0) + ISNULL(RegTrav,0)) +       \n");
                QueryText.Append("                              ((ISNULL(OT,0) + ISNULL(OTTrav,0))) +        \n");
                QueryText.Append("                              ((ISNULL(NT,0) + ISNULL(NTTrav,0))) +        \n");
                QueryText.Append(" 				                ((ISNULL(DT,0) + ISNULL(DTTrav,0))) +        \n");
                QueryText.Append("                              (ISNULL(TT,0)))     \n");
                QueryText.Append(" 			    ) as NActual	\n");
                QueryText.Append(" 	            FROM TicketD                                    \n");
                QueryText.Append("                WHERE Job = '" + objJob.Job + "'              \n");
                QueryText.Append("                  AND EDate >= '" + objJob.StartDate + "'     \n");
                QueryText.Append("                  AND	EDate <= '" + objJob.EndDate + "'       \n");
                QueryText.Append("                  AND Phase = JobItem.Line),0) AS HourAct,    \n");

                QueryText.Append("          ISNULL(jobitem.BHours,0) AS HourBgt,                \n");

                QueryText.Append("          Sum(CASE JobI.Labor WHEN 1 THEN ISNULL(JobI.Amount,0) ELSE 0 END) AS LaborAct,    \n");

                QueryText.Append("          ISNULL(ETC,0) As LaborBgt,                          \n");
                QueryText.Append("          ISNULL(ETCMod,0) As LaborMod,                       \n");
                QueryText.Append("          Sum(CASE JobI.Labor WHEN 1 THEN ISNULL(JobI.Amount,0) ELSE 0 END) - (ISNULL(ETC,0) + ISNULL(ETCMod,0)) As LabDiff       \n");
                QueryText.Append("  FROM (JobI LEFT JOIN JobTItem JobItem ON JobI.Job = JobItem.Job AND JobI.Phase = JobItem.Line AND JobI.Type = JobItem.Type      \n");
                QueryText.Append("        AND  (JobI.fDate>= '" + objJob.StartDate + "' AND JobI.fDate<= '" + objJob.EndDate + "')        \n");
                QueryText.Append("     ) 	            \n");
                QueryText.Append("     LEFT JOIN        \n");
                QueryText.Append("        (	SELECT t.Job, t.Phase, t.Type, SUM(t.Comm) AS Comm FROM \n");
                QueryText.Append("            (         ");
                QueryText.Append("                (SELECT p.Job, p.Phase, 1 AS Type, Sum(ISNULL(p.Balance,0)) AS Comm   \n");
                QueryText.Append("					                FROM POItem p                                       \n");
                QueryText.Append("						                INNER JOIN PO on p.po = po.po                   \n");
                QueryText.Append("							                WHERE PO.Status in (0,3,4) AND p.Job = '" + objJob.Job + "' \n");
                QueryText.Append("							                GROUP BY p.Job, p.Phase)                    \n");
                QueryText.Append("                UNION                                                                 \n");
                QueryText.Append("                (SELECT p.Job, p.Phase, 1 AS Type, Sum(ISNULL(rp.Amount,0)) AS Comm   \n");
                QueryText.Append("					                FROM RPOItem rp                                     \n");
                QueryText.Append("						                INNER JOIN ReceivePO r on r.ID = rp.ReceivePO   \n");
                QueryText.Append("						                LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line \n");
                QueryText.Append("							                WHERE ISNULL(r.Status,0) = 0 AND p.Job = '" + objJob.Job + "' \n");
                QueryText.Append("							                GROUP By p.Job, p.Phase)    \n");
                QueryText.Append("            ) AS t                                                    \n");
                QueryText.Append("            GROUP BY t.Job,t.Phase, t.Type                            \n");
                QueryText.Append("        ) AS PO ON		PO.Job = JobItem.Job                        \n");
                QueryText.Append("	                AND PO.Phase = JobItem.Line                         \n");
                QueryText.Append("	                AND PO.Type = JobItem.Type                          \n");
                QueryText.Append("     WHERE jobitem.Job = '" + objJob.Job + "' AND jobitem.type = 1                    \n");
                QueryText.Append("  GROUP BY JobItem.fDesc, JobItem.Type, JobItem.Code, JobItem.Line, PO.Comm,JobItem.BHours,   \n");
                QueryText.Append("              JobItem.Budget, JobItem.ETC, JobItem.Modifier, JobItem.ETCMod   \n");
                QueryText.Append("     ORDER BY JobItem.Type, JobItem.Line                              \n");

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobCostPOByJob(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "job",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "phase",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Phase
                };
                if (objJob.StartDate != DateTime.MinValue)
                {
                    para[2] = new SqlParameter
                    {
                        ParameterName = "sdate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.StartDate
                    };
                }
                if (objJob.EndDate != DateTime.MinValue)
                {
                    para[3] = new SqlParameter
                    {
                        ParameterName = "edate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objJob.EndDate
                    };
                }

                return SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCostPOByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobExpGLByJob(JobT objJob)
        {
            try
            {
                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.Text, "SELECT ISNULL(Job.GL, 0) AS GLExp, Chart.Acct, Chart.fDesc AS DefaultAcct FROM Job LEFT JOIN JobT ON Job.Template = JobT.ID LEFT JOIN Chart ON Job.GL = Chart.ID WHERE Job.ID = '" + objJob.Job + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBOMByJobID(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "projectId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "GetBOMByJobID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMilestoneByJobID(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "projectId",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "GetMilestoneByJobID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddBOMType(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.TypeName
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                SqlHelper.ExecuteNonQuery(objJob.ConnConfig, CommandType.StoredProcedure, "spAddBOMType", para);
                return Convert.ToInt32(para[1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobRoute(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT rt.ID,rt.Name,rt.Status  , rt.Name + (select( case   when tblwork.fdesc = rt.Name then '' else ' - ' + tblwork.fdesc   end)  from tblwork where tblwork.id = rt.mech   ) as Name1  FROM Route rt Order by rt.Name");// Where ID <> 0
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobStage(JobT _objJob)
        {
            try
            {
                var strdb = new StringBuilder();

                strdb.AppendLine(" Select psi.ID, LTRIM(RTRIM(ps.Description)) + ':' + psi.Label Stage ");
                strdb.AppendLine(" from tblProjectStageItem psi ");
                strdb.AppendLine(" inner join tblProjectStage ps on ps.ID = psi.StageID ");
                strdb.AppendLine(" Order By psi.StageID, psi.OrderNo, psi.Label");
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, strdb.ToString());
                //return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "Select ps.ID, ps.Description Stage from tblProjectStage ps INNER JOIN Job j ON j.Stage is not null and j.Stage = ps.ID");// Where ID <> 0
                //return SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "Select ps.ID, ps.Label Stage from tblProjectStageItem ps Order By ps.OrderNo, ps.Label");// Where ID <> 0
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllStageItems(string strConn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "spGetAllStageItems");
                //return SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "Select ps.ID, ps.Label Stage from tblProjectStageItem ps Order By ps.OrderNo, ps.Label");// Where ID <> 0
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepartmentByTemplateId(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, string.Format("SELECT JobType.Type from JobT inner join JobType on JobT.Type=JobType.ID where JobT.ID = {0}", _objJob.ID));// Where ID <> 0
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataOnInitialEstimate(string strConnConfig, int salesAsigned = 0, int estimateId = 0, int uType = 1)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@UType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = uType
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@IsSalesAsigned",
                    SqlDbType = SqlDbType.Int,
                    Value = salesAsigned
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@EstimateId",
                    SqlDbType = SqlDbType.Int,
                    Value = estimateId
                };

                return SqlHelper.ExecuteDataset(strConnConfig, CommandType.StoredProcedure, "spGetDataOnInitialEstimate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobCustomValueByJobId(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobID",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobCustomByJobID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobAttributeGeneralCustomByJobID(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobID",
                    SqlDbType = SqlDbType.Int,
                    Value = objJob.Job
                };

                return objJob.Ds = SqlHelper.ExecuteDataset(objJob.ConnConfig, CommandType.StoredProcedure, "spGetJobAttributeGeneralCustomByJobID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateJobCustomValue(JobT objJob)
        {
            try
            {
                var para = new SqlParameter[22];

                para[0] = new SqlParameter
                {
                    ParameterName = "Custom1",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom1
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Custom2",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom2
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "Custom3",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom3
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "Custom4",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom4
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "Custom5",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom5
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "Custom6",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom6
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Custom7",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom7
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "Custom8",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom8
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Custom9",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom9
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "Custom10",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom10
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Custom11",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom11
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "Custom12",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom12
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "Custom13",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom13
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "Custom14",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom14
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "Custom15",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom15
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "Custom16",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom16
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "Custom17",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom17
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "Custom18",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom18
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "Custom19",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom19
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "Custom20",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.custom20
                };
                para[20] = new SqlParameter
                {
                    ParameterName = "@jobID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.Job
                };
                para[21] = new SqlParameter
                {
                    ParameterName = "UpdatedByUserID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJob.UserID
                };

                SqlHelper.ExecuteNonQuery(objJob.ConnConfig, CommandType.StoredProcedure, "spUpdateJobCustomValue", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectManagerByTypeId(JobT _objJob)
        {


            try
            {
                return SqlHelper.ExecuteDataset(_objJob.ConnConfig, "spGetProjectManagerByTypeId", _objJob.TypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetAssingedProjectByTypeId(JobT _objJob)
        {


            try
            {
                return SqlHelper.ExecuteDataset(_objJob.ConnConfig, "spGetAssignedProjectByTypeId", _objJob.TypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetUserDetailByEmpID(string strConfig, int empId)
        {


            try
            {
                return SqlHelper.ExecuteDataset(strConfig, "spGetUserDetailByEmpID", empId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet spGetProjectListDataMVC(ProjectListGridParam _ProjectParam, String ConnectionString)
        { 


            try
            {

                SqlParameter[] para = new SqlParameter[10];
                para[0] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _ProjectParam.SearchBy
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _ProjectParam.SearchValue
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _ProjectParam.StartDate
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _ProjectParam.EndDate
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "Range",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _ProjectParam.Range
                };


                para[5] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _ProjectParam.JobType
                };

                para[6] = new SqlParameter
                {
                    ParameterName = "IsSalesAsigned",
                    SqlDbType = SqlDbType.Int,
                    Value = _ProjectParam.IsSelesAsigned
                };

                para[7] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _ProjectParam.EN
                };

                para[8] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _ProjectParam.UserID
                };

                para[9] = new SqlParameter
                {
                    ParameterName = "IncludeClose",
                    SqlDbType = SqlDbType.Int,
                    Value = _ProjectParam.IncludeClose
                };



                return SqlHelper.ExecuteDataset(ConnectionString, "spGetProjectListDataMVC", para);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobById(JobT _objJob)
        {
            try
            {
                return _objJob.Ds = SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, "SELECT * FROM Job WHERE ID='" + _objJob.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Start -- Juily - 12/08/2020-- //
        public void RecalculateLaborExpenses(JobT _objJob)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objJob.StartDate
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "ToDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _objJob.EndDate
                };

                SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.StoredProcedure, "spReCalCulateLaborexpensebyDate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //End- Juily - 12/08/2020-- //

        public DataSet GetSupervisorByTypeId(JobT _objJob)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objJob.ConnConfig, "spGetSupervisorByTypeId", _objJob.TypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region Start - Project WIP

        public DataSet GetProjectWIPByPeriod(JobT _objJob, int period)
        {
            try
            {
                string query = $"SELECT * FROM ProjectWIP WHERE Period = {period}";

                return SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLastProjectWIPPostedByPeriod(JobT _objJob, int period)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT TOP 1 u.fUser, pw.* \n");
                sb.Append("FROM ProjectWIP pw \n");
                sb.Append("INNER JOIN tbluser u ON pw.UserID = u.ID \n");
                sb.Append("WHERE Period >= " + period + " AND IsPost = 1 \n");
                sb.Append("ORDER BY Period DESC \n");
                sb.Append("SELECT TOP 1 u.fUser, pw.* \n");
                sb.Append("FROM ProjectWIP pw \n");
                sb.Append("INNER JOIN tbluser u ON pw.UserID = u.ID \n");
                sb.Append("WHERE Period = " + period + " \n");
                sb.Append("SELECT TOP 1 * FROM ProjectWIP WHERE IsPost = 1 ORDER BY Period DESC \n");

                return SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLastPostProjectWIP(JobT _objJob)
        {
            try
            {
                string query = "SELECT TOP 1 * FROM ProjectWIP WHERE IsPost = 1 ORDER BY Period DESC";

                return SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddProjectWIP(JobT _objJob, DateTime fDate, bool isPost = false)
        {
            try
            {
                string query = "INSERT INTO [ProjectWIP](UserID, Period, fDate, LastUpdate) VALUES(@UserID, @Period, @fDate, @LastUpdate); SELECT SCOPE_IDENTITY()";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserID", _objJob.UserID));
                parameters.Add(new SqlParameter("@Period", fDate.Year * 100 + fDate.Month));
                parameters.Add(new SqlParameter("@fDate", fDate));
                parameters.Add(new SqlParameter("@LastUpdate", DateTime.Now));

                if (isPost)
                {
                    query = "INSERT INTO [ProjectWIP](UserID, Period, fDate, IsPost, PostDate, LastUpdate) VALUES(@UserID, @Period, @fDate, 1, @PostDate, @LastUpdate); SELECT SCOPE_IDENTITY()";
                    parameters.Add(new SqlParameter("@PostDate", DateTime.Now));
                }

                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.Text, query, parameters.ToArray()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProjectWIP(JobT _objJob, DateTime fDate, bool isPost = false)
        {
            try
            {
                string query = "UPDATE [ProjectWIP] SET UserID = @UserID, Period = @Period, fDate = @fDate, LastUpdate = @LastUpdate WHERE ID = @ID";
         
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objJob.ID));
                parameters.Add(new SqlParameter("@UserID", _objJob.UserID));
                parameters.Add(new SqlParameter("@Period", fDate.Year * 100 + fDate.Month));
                parameters.Add(new SqlParameter("@fDate", fDate));
                parameters.Add(new SqlParameter("@LastUpdate", DateTime.Now));

                if (isPost)
                {
                    query = "UPDATE [ProjectWIP] SET UserID = @UserID, Period = @Period, fDate = @fDate, IsPost = 1, PostDate = @PostDate, LastUpdate = @LastUpdate WHERE ID = @ID";
                    parameters.Add(new SqlParameter("@PostDate", DateTime.Now));
                }

                SqlHelper.ExecuteDataset(_objJob.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddProjectWIPDetail(ProjectWIP _objProWip)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("IF (EXISTS (SELECT 1 FROM ProjectWIPDetail WHERE WIPID = @WIPID AND Job = @Job)) \n");
                sb.Append("BEGIN \n");
                sb.Append("	UPDATE [dbo].[ProjectWIPDetail] \n");
                sb.Append("	   SET [WIPID] = @WIPID \n");
                sb.Append("		  ,[Job] = @Job \n");
                sb.Append("		  ,[Type] = @Type \n");
                sb.Append("		  ,[Department] = @Department \n");
                sb.Append("		  ,[fDesc] = @fDesc \n");
                sb.Append("		  ,[Tag] = @Tag \n");
                sb.Append("		  ,[Status] = @Status \n");
                sb.Append("		  ,[ContractPrice] = @ContractPrice \n");
                sb.Append("		  ,[ConstModAdjmts] = @ConstModAdjmts \n");
                sb.Append("		  ,[AccountingAdjmts] = @AccountingAdjmts \n");
                sb.Append("		  ,[TotalBudgetedExpense] = @TotalBudgetedExpense \n");
                sb.Append("		  ,[TotalEstimatedCost] = @TotalEstimatedCost \n");
                sb.Append("		  ,[EstimatedProfit] = @EstimatedProfit \n");
                sb.Append("		  ,[ContractCosts] = @ContractCosts \n");
                sb.Append("		  ,[CostToComplete] = @CostToComplete \n");
                sb.Append("		  ,[PercentageComplete] = @PercentageComplete \n");
                sb.Append("		  ,[RevenuesEarned] = @RevenuesEarned \n");
                sb.Append("		  ,[GrossProfit] = @GrossProfit \n");
                sb.Append("		  ,[BilledToDate] = @BilledToDate \n");
                sb.Append("		  ,[ToBeBilled] = @ToBeBilled \n");
                sb.Append("		  ,[OpenARAmount] = @OpenARAmount \n");
                sb.Append("		  ,[RetainageBilling] = @RetainageBilling \n");
                sb.Append("		  ,[IsUpdateRetainage] = @IsUpdateRetainage \n");
                sb.Append("		  ,[TotalBilling] = @TotalBilling \n");
                sb.Append("		  ,[Billings] = @Billings \n");
                sb.Append("		  ,[Earnings] = @Earnings \n");
                sb.Append("		  ,[NPer] = @NPer \n");
                sb.Append("		  ,[NPerLastMonth] = @NPerLastMonth \n");
                sb.Append("		  ,[NPerLastYear] = @NPerLastYear \n");
                sb.Append("		  ,[NPerLastMonthYear] = @NPerLastMonthYear \n");
                sb.Append("		  ,[BillingContract] = @BillingContract \n");
                sb.Append("		  ,[JobBorrow] = @JobBorrow \n");
                sb.Append("		  ,[fDate] = @fDate \n");
                sb.Append("		  ,[CloseDate] = @CloseDate \n");
                sb.Append("		  ,[ExpectedClosingDate] = @ExpectedClosingDate \n");
                sb.Append("	 WHERE WIPID = @WIPID AND Job = @Job \n");
                sb.Append("END \n");
                sb.Append("ELSE \n");
                sb.Append("BEGIN \n");
                sb.Append("	INSERT INTO [dbo].[ProjectWIPDetail] \n");
                sb.Append("				([WIPID] \n");
                sb.Append("				,[Job] \n");
                sb.Append("				,[Type] \n");
                sb.Append("				,[Department] \n");
                sb.Append("				,[fDesc] \n");
                sb.Append("				,[Tag] \n");
                sb.Append("				,[Status] \n");
                sb.Append("				,[ContractPrice] \n");
                sb.Append("				,[ConstModAdjmts] \n");
                sb.Append("				,[AccountingAdjmts] \n");
                sb.Append("				,[TotalBudgetedExpense] \n");
                sb.Append("				,[TotalEstimatedCost] \n");
                sb.Append("				,[EstimatedProfit] \n");
                sb.Append("				,[ContractCosts] \n");
                sb.Append("				,[CostToComplete] \n");
                sb.Append("				,[PercentageComplete] \n");
                sb.Append("				,[RevenuesEarned] \n");
                sb.Append("				,[GrossProfit] \n");
                sb.Append("				,[BilledToDate] \n");
                sb.Append("				,[ToBeBilled] \n");
                sb.Append("				,[OpenARAmount] \n");
                sb.Append("				,[RetainageBilling] \n");
                sb.Append("				,[IsUpdateRetainage] \n");
                sb.Append("				,[TotalBilling] \n");
                sb.Append("				,[Billings] \n");
                sb.Append("				,[Earnings] \n");
                sb.Append("				,[NPer] \n");
                sb.Append("				,[NPerLastMonth] \n");
                sb.Append("				,[NPerLastYear] \n");
                sb.Append("				,[NPerLastMonthYear] \n");
                sb.Append("				,[BillingContract] \n");
                sb.Append("				,[JobBorrow] \n");
                sb.Append("				,[fDate] \n");
                sb.Append("				,[CloseDate] \n");
                sb.Append("				,[ExpectedClosingDate]) \n");
                sb.Append("		 VALUES \n");
                sb.Append("				(@WIPID \n");
                sb.Append("				,@Job \n");
                sb.Append("				,@Type \n");
                sb.Append("				,@Department \n");
                sb.Append("				,@fDesc \n");
                sb.Append("				,@Tag \n");
                sb.Append("				,@Status \n");
                sb.Append("				,@ContractPrice \n");
                sb.Append("				,@ConstModAdjmts \n");
                sb.Append("				,@AccountingAdjmts \n");
                sb.Append("				,@TotalBudgetedExpense \n");
                sb.Append("				,@TotalEstimatedCost \n");
                sb.Append("				,@EstimatedProfit \n");
                sb.Append("				,@ContractCosts \n");
                sb.Append("				,@CostToComplete \n");
                sb.Append("				,@PercentageComplete \n");
                sb.Append("				,@RevenuesEarned \n");
                sb.Append("				,@GrossProfit \n");
                sb.Append("				,@BilledToDate \n");
                sb.Append("				,@ToBeBilled \n");
                sb.Append("				,@OpenARAmount \n");
                sb.Append("				,@RetainageBilling \n");
                sb.Append("				,@IsUpdateRetainage \n");
                sb.Append("				,@TotalBilling \n");
                sb.Append("				,@Billings \n");
                sb.Append("				,@Earnings \n");
                sb.Append("				,@NPer \n");
                sb.Append("				,@NPerLastMonth \n");
                sb.Append("				,@NPerLastYear \n");
                sb.Append("				,@NPerLastMonthYear \n");
                sb.Append("				,@BillingContract \n");
                sb.Append("				,@JobBorrow \n");
                sb.Append("				,@fDate \n");
                sb.Append("				,@CloseDate \n");
                sb.Append("				,@ExpectedClosingDate) \n");
                sb.Append("END \n");

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@WIPID", _objProWip.WIPID));
                parameters.Add(new SqlParameter("@Job", _objProWip.Job));
                parameters.Add(new SqlParameter("@Type", _objProWip.Type));
                parameters.Add(new SqlParameter("@Department", _objProWip.Department));
                parameters.Add(new SqlParameter("@fDesc", _objProWip.fDesc));
                parameters.Add(new SqlParameter("@Tag", _objProWip.Tag));
                parameters.Add(new SqlParameter("@Status", _objProWip.Status));
                parameters.Add(new SqlParameter("@ContractPrice", _objProWip.ContractPrice));
                parameters.Add(new SqlParameter("@ConstModAdjmts", _objProWip.ConstModAdjmts));
                parameters.Add(new SqlParameter("@AccountingAdjmts", _objProWip.AccountingAdjmts));
                parameters.Add(new SqlParameter("@TotalBudgetedExpense", _objProWip.TotalBudgetedExpense));
                parameters.Add(new SqlParameter("@TotalEstimatedCost", _objProWip.TotalEstimatedCost));
                parameters.Add(new SqlParameter("@EstimatedProfit", _objProWip.EstimatedProfit));
                parameters.Add(new SqlParameter("@ContractCosts", _objProWip.ContractCosts));
                parameters.Add(new SqlParameter("@CostToComplete", _objProWip.CostToComplete));
                parameters.Add(new SqlParameter("@PercentageComplete", _objProWip.PercentageComplete));
                parameters.Add(new SqlParameter("@RevenuesEarned", _objProWip.RevenuesEarned));
                parameters.Add(new SqlParameter("@GrossProfit", _objProWip.GrossProfit));
                parameters.Add(new SqlParameter("@BilledToDate", _objProWip.BilledToDate));
                parameters.Add(new SqlParameter("@ToBeBilled", _objProWip.ToBeBilled));
                parameters.Add(new SqlParameter("@OpenARAmount", _objProWip.OpenARAmount));
                parameters.Add(new SqlParameter("@RetainageBilling", _objProWip.RetainageBilling));
                parameters.Add(new SqlParameter("@IsUpdateRetainage", _objProWip.IsUpdateRetainage));
                parameters.Add(new SqlParameter("@TotalBilling", _objProWip.TotalBilling));
                parameters.Add(new SqlParameter("@Billings", _objProWip.Billings));
                parameters.Add(new SqlParameter("@Earnings", _objProWip.Earnings));
                parameters.Add(new SqlParameter("@NPer", _objProWip.NPer));
                parameters.Add(new SqlParameter("@NPerLastMonth", _objProWip.NPerLastMonth));
                parameters.Add(new SqlParameter("@NPerLastYear", _objProWip.NPerLastYear));
                parameters.Add(new SqlParameter("@NPerLastMonthYear", _objProWip.NPerLastMonthYear));
                parameters.Add(new SqlParameter("@BillingContract", _objProWip.BillingContract));
                parameters.Add(new SqlParameter("@JobBorrow", _objProWip.JobBorrow));
                parameters.Add(new SqlParameter("@fDate", _objProWip.fDate));
                parameters.Add(new SqlParameter("@CloseDate", _objProWip.CloseDate));
                parameters.Add(new SqlParameter("@ExpectedClosingDate", _objProWip.ExpectedClosingDate));

                SqlHelper.ExecuteDataset(_objProWip.ConnConfig, CommandType.Text, sb.ToString(), parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
