using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;

namespace DataLayer
{
    public class DL_Customer
    {
        private bool CheckFilterHasCommaDelimited(List<RetainFilter> filters)
        {
            bool isFilterHasCommaDelimited = false;
            if (filters != null && filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0; string[] filterArrayValue;
                        StringBuilder filteredQuery = new StringBuilder();
                        if (items.FilterColumn.Equals("ID", StringComparison.InvariantCultureIgnoreCase))
                        {
                            filterArrayValue = items.FilterValue.ToString().Split(',');
                            foreach (var filtered in filterArrayValue)
                            {
                                if (int.TryParse(filtered, out FilterValue))
                                {
                                    if (filteredQuery.Length == 0)
                                    {
                                        filteredQuery.Append(filtered);
                                    }
                                    else
                                    {
                                        isFilterHasCommaDelimited = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return isFilterHasCommaDelimited;
        }
        public DataSet getCustomers(Customer objPropCustomer, Int32 UserID, Int32 IsSelesAsigned = 0)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetCustomers", DBNull.Value, DBNull.Value, objPropCustomer.DBName, IsSelesAsigned, 0, UserID, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getProspectByID(Customer objPropCustomer)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT p.ID, \n");
            //varname1.Append("       p.Rol, \n");
            //varname1.Append("       p.status, \n");
            //varname1.Append("       p.type, \n");
            //varname1.Append("       p.Address AS billaddress, \n");
            //varname1.Append("       p.City AS billcity, \n");
            //varname1.Append("       p.State AS billstate, \n");
            //varname1.Append("       p.Zip AS billzip, \n");
            //varname1.Append("       p.phone AS billphone, \n");
            //varname1.Append("       p.CustomerName, \n");
            //varname1.Append("       p.Terr, \n");
            //varname1.Append("       r.Name, \n");
            //varname1.Append("       r.Address , \n");
            //varname1.Append("       r.City    , \n");
            //varname1.Append("       r.State   , \n");
            //varname1.Append("       r.Zip     , \n");
            //varname1.Append("       r.Phone   , \n");
            //varname1.Append("       r.Cellular, \n");
            //varname1.Append("       r.email, \n");
            //varname1.Append("       r.Website, \n");
            //varname1.Append("       r.Fax, \n");
            //varname1.Append("       r.Contact, \n");
            //varname1.Append("       r.Remarks, \n");
            //varname1.Append("       r.lat, r.lng \n");
            //varname1.Append("FROM   Prospect p \n");
            //varname1.Append("       INNER JOIN Rol r \n");
            //varname1.Append("               ON r.ID = p.Rol ");
            //varname1.Append(" where p.ID=" + objPropCustomer.ProspectID);

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProspectByID", objPropCustomer.ProspectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getProspectByID(GetProspectByIDParam _GetProspectByID, string ConnectionString)
        {
            try
            {
                return _GetProspectByID.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, "spGetProspectByID", _GetProspectByID.ProspectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getContactList(Customer objPropCustomer)
        {


            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "SpgetcontactList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getProspect(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProspects", objPropCustomer.SearchBy, objPropCustomer.SearchValue, objPropCustomer.Status, IsSalesAsigned, objPropCustomer.EN, objPropCustomer.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getProspectByContact(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProspectsByContact", objPropCustomer.SearchBy, objPropCustomer.SearchValue, IsSalesAsigned, objPropCustomer.EN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTasks(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetTasks", objPropCustomer.SearchBy
                    , objPropCustomer.SearchValue, objPropCustomer.StartDate, objPropCustomer.EndDate, objPropCustomer.Mode, IsSalesAsigned, objPropCustomer.EN
                    , objPropCustomer.UserID, objPropCustomer.Screen, objPropCustomer.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet UpdateTaskToClose(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spUpdateTaskToClose", objPropCustomer.TaskID, objPropCustomer.DueDate, objPropCustomer.TimeDue, objPropCustomer.Username, objPropCustomer.Desc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTasksByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetTaskByID", objPropCustomer.TemplateID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpportunityByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetOpportunityByID", objPropCustomer.OpportunityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String getCompanyName(Customer objPropCustomer)
        {
            try
            {
                string CompanyName = "";
                CompanyName = Convert.ToString(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spGetCompanyName", objPropCustomer.OpportunityID));
                return CompanyName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String getLocID(Customer objPropCustomer)
        {
            try
            {
                string LocID = "";
                String query = "select Loc from LOC where Rol=" + objPropCustomer.RoleID;
                LocID = Convert.ToString(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.Text, query));
                return LocID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //public String UpdateEstimateWithLoc(Customer objPropCustomer)
        //{
        //    try
        //    {
        //        String ret = Convert.ToString(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "UpdateEstimateWithLoc", objPropCustomer.LocID));
        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet getRecentProspect(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetRecentItems");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getProspectType(Customer objPropCustomer)
        {
            try
            {
                //return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from ptype");
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select *,(select count(*) from Prospect Where Type=ptype.Type) AS NewCount from ptype");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getStages(Customer objPropCustomer)
        {
            try
            {
                //return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from stage");
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "SELECT ID,Description, Description + ' ' + CONVERT(varchar(10),Convert(int, ISNULL(Probability,0))) + '%' DescWithProbability,[Count],Label,Type,Probability,Convert(int, ISNULL(Probability,0)) ProbabilityInt, [Chart Colors],(SELECT COUNT(*) FROM Lead WHERE OpportunityStageID=stage.ID) AS NewCount FROM stage Order by Type desc, ProbabilityInt asc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getRepTemplateName(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select ID,fdesc, 0 as CBcheckStatus from EquipTemp  order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public DataSet getRepTemplateName(GetRepTemplateNameParam _GetRepTemplateName, string ConnectionString)
        {
            try
            {
                return _GetRepTemplateName.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select ID,fdesc, 0 as CBcheckStatus from EquipTemp  order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataSet getRepTemplate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select *,( SELECT Count(distinct Elev) FROM   dbo.EquipTItem WHERE  EquipT = EquipTemp.ID and Elev <>0) as TotalLocUnits, ( SELECT Count(distinct LeadEquip) FROM   dbo.EquipTItem WHERE  EquipT = EquipTemp.ID and LeadEquip <>0) as TotalLeadUnits from EquipTemp  order by ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomTemplate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from elevt order by fdesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getCustomTemplate(GetCustomTemplateParam _GetCustomTemplate, string ConnectionString)
        {
            try
            {
                return _GetCustomTemplate.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from elevt order by fdesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTemplateItemByID(Customer objPropCustomer)
        {
            try
            {
                if (objPropCustomer.IsLeadEquip)
                {
                    return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select *, (select top 1 fdesc from equiptemp where id= ei.equipt)as name from EquipTItem ei where EquipT ='" + objPropCustomer.TemplateID + "' and LeadEquip =0 or LeadEquip is null");
                }
                else
                {
                    return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select *, (select top 1 fdesc from equiptemp where id= ei.equipt)as name from EquipTItem ei where EquipT ='" + objPropCustomer.TemplateID + "' and Elev =0 ");
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public DataSet getTemplateItemByID(GetTemplateItemByIDParam _GetTemplateItemByID, string ConnectionString)
        {
            try
            {
                if (_GetTemplateItemByID.IsLeadEquip)
                {
                    return _GetTemplateItemByID.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select *, (select top 1 fdesc from equiptemp where id= ei.equipt)as name from EquipTItem ei where EquipT ='" + _GetTemplateItemByID.TemplateID + "' and LeadEquip =0 or LeadEquip is null");
                }
                else
                {
                    return _GetTemplateItemByID.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select *, (select top 1 fdesc from equiptemp where id= ei.equipt)as name from EquipTItem ei where EquipT ='" + _GetTemplateItemByID.TemplateID + "' and Elev =0 ");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataSet getCustTemplateItemByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select isnull(orderno,line) as orderno,*,(select format from elevTItem eti where eti.ID=ei.customid)as formatMOM, (select top 1 fdesc from elevt where id= ei.elevt)as name from elevTItem ei where elevT =" + objPropCustomer.TemplateID + " and Elev =0 order by ei.OrderNO select * from tblcustomvalues where elevt=" + objPropCustomer.TemplateID + " order by Value asc");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetEquipmentCustTemplateItem(string connConfig, int custTemplate, int equipId, int leadEquipId)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "TemplateId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = custTemplate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "EquipId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = equipId;

                para[2] = new SqlParameter();
                para[2].ParameterName = "LeadEquipId";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = leadEquipId;
                return SqlHelper.ExecuteDataset(connConfig, CommandType.StoredProcedure, "spGetEquipmentCustTemplateItem", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetCustomValuesOfEquip(string connConfig, int equipId)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = equipId
                };
                return SqlHelper.ExecuteDataset(connConfig, CommandType.StoredProcedure, "spGetCustomValuesOfEquip", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public DataSet getCustTemplateItemByID(GetCustTemplateItemByIDParam _GetCustTemplateItemByID, string ConnectionString)
        {
            try
            {
                return _GetCustTemplateItemByID.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select isnull(orderno,line) as orderno,*,(select format from elevTItem eti where eti.ID=ei.customid)as formatMOM, (select top 1 fdesc from elevt where id= ei.elevt)as name from elevTItem ei where elevT =" + _GetCustTemplateItemByID.TemplateID + " and Elev =0 order by ei.OrderNO select * from tblcustomvalues where elevt=" + _GetCustTemplateItemByID.TemplateID);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getCustomValues(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from tblcustomvalues where itemid= " + objPropCustomer.ItemID + " order by value");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ISshowHomeowner(string ConnConfig, int ISshowHomeowner, int IsLocAddressBlank)
        {
            try
            {
                SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "UPDATE Control SET ISshowHomeowner=" + ISshowHomeowner + " , IsLocAddressBlank=" + IsLocAddressBlank);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getTemplateItemCodes(Customer objPropCustomer)
        {
            string strQuery = "select distinct code from EquipTItem where isnull(code,'')<> ''";
            if (objPropCustomer.TemplateID != 0)
            {
                strQuery += "  and EquipT <> " + objPropCustomer.TemplateID;
            }
            if (!string.IsNullOrEmpty(objPropCustomer.SearchValue))
            {
                strQuery += "  and code like '" + objPropCustomer.SearchValue + "%'";
            }

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getTemplateItemByMultipleID(Customer objPropCustomer, string id)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.EquipT in (" + id + ")");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getTemplateItemByElevAndEquipT(Customer objPropCustomer, string EquipId, string Elev)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "if exists(select 1 from EquipTItem where EquipT = " + EquipId + " and Elev =" + Elev + ") Begin  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.EquipT = " + EquipId + " and eti.Elev=" + Elev + " End  Else Begin select	et.fdesc as Name,et.Remarks,eti.EquipT,	eti.fDesc,eti.Lastdate,eti.NextDateDue,	eti.Frequency from	EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID   where eti.EquipT = " + EquipId + " and eti.Elev=" + Elev + " End");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int AddProspect(Customer objPropCustomer)
        {
            var para = new SqlParameter[33];

            para[0] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@address",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Address
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@type",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Type
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Status
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@cell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@CustomerName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CustomerName
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@SalesPerson",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Terr
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@BillAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@BillCity",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillCity
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@BillState",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillState
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@Billzip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillZip
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@Billphone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillPhone
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Website
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@Lat",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lat
            };
            para[22] = new SqlParameter
            {
                ParameterName = "@Lng",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lng
            };
            para[23] = new SqlParameter
            {
                ParameterName = "ContactData",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.ContactData
            };
            para[24] = new SqlParameter
            {
                ParameterName = "@UpdateUser",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.LastUpdateUser
            };
            para[25] = new SqlParameter
            {
                ParameterName = "@Source",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Source
            };
            para[26] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Country
            };
            para[27] = new SqlParameter
            {
                ParameterName = "@BillCountry",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillCountry
            };
            para[28] = new SqlParameter
            {
                ParameterName = "@Referral",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer._Referral
            };

            para[29] = new SqlParameter
            {
                ParameterName = "@BusinessType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer._BT
            };
            para[30] = new SqlParameter
            {
                ParameterName = "@ReferralType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.ReferralType
            };
            para[31] = new SqlParameter
            {
                ParameterName = "@EN",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.EN
            };
            try
            {

                int prospectID = 0;
                prospectID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProspect", para));
                return prospectID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddContactByRol(PhoneModel objPhone)
        {
            var para = new SqlParameter[7];

            para[0] = new SqlParameter
            {
                ParameterName = "@RolID",
                SqlDbType = SqlDbType.Int,
                Value = objPhone.Rol
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Phone
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Fax
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Cell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Cell
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Email
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Title",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Title
            };

            try
            {

                int prospectID = 0;
                prospectID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPhone.ConnConfig, CommandType.StoredProcedure, "spAddContactAsPerRole", para));
                return prospectID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateContactByID(PhoneModel objPhone)
        {
            var para = new SqlParameter[8];

            para[0] = new SqlParameter
            {
                ParameterName = "@RolID",
                SqlDbType = SqlDbType.Int,
                Value = objPhone.Rol
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Phone
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Fax
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Cell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Cell
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Email
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Title",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Title
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.ID
            };

            try
            {

                int prospectID = 0;
                prospectID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPhone.ConnConfig, CommandType.StoredProcedure, "spUpdateContactByID", para));
                return prospectID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProspect(Customer objPropCustomer)
        {
            var para = new SqlParameter[34];

            para[0] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@address",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Address
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@type",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Type
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Status
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@cell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@CustomerName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CustomerName
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@SalesPerson",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Terr
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@BillAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@BillCity",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillCity
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@BillState",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillState
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@Billzip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillZip
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@Billphone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillPhone
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Website
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@Lat",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lat
            };
            para[22] = new SqlParameter
            {
                ParameterName = "@Lng",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lng
            };
            para[23] = new SqlParameter
            {
                ParameterName = "@ContactData",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.ContactData
            };
            para[24] = new SqlParameter
            {
                ParameterName = "@ProspectID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProspectID
            };
            para[25] = new SqlParameter
            {
                ParameterName = "@UpdateUser",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.LastUpdateUser
            };
            para[26] = new SqlParameter
            {
                ParameterName = "@Source",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Source
            };
            para[27] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Country
            };

            para[28] = new SqlParameter
            {
                ParameterName = "@BillCountry",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillCountry
            };
            para[29] = new SqlParameter
            {
                ParameterName = "@Referral",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer._Referral

            };
            para[30] = new SqlParameter
            {
                ParameterName = "@BusinessType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer._BT
            };

            para[31] = new SqlParameter
            {
                ParameterName = "@ReferralType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.ReferralType
            };
            para[32] = new SqlParameter
            {
                ParameterName = "@EN",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.EN
            };
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateProspect", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDefaultSalesPerson(Customer objCustomer)
        {
            string strQuery = "Select Terr from Loc WHERE Loc=" + objCustomer.LocID;

            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objCustomer.ConnConfig, CommandType.Text, strQuery));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddProspectContact(Customer objPropCustomer)
        {
            var para = new SqlParameter[2];
            para[0] = new SqlParameter
            {
                ParameterName = "@ContactData",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.ContactData
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ProspectID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProspectID
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProspectContact", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeletePhone(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.Text, "delete from phone where ID=" + objPropCustomer.contact);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProspect(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spDeleteProspect", objPropCustomer.ProspectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddProspectType(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spAddProspectType", objPropCustomer.Type, objPropCustomer.Remarks, objPropCustomer.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddSaleSource(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spAddSalesSource", objPropCustomer.Type, objPropCustomer.SourceDescription, objPropCustomer.Mode, objPropCustomer.OldSourceDescription);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateStages(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spUpdateStage", objPropCustomer.Stage.ID, objPropCustomer.Stage.Description, objPropCustomer.Stage.Type, objPropCustomer.Stage.Probability, objPropCustomer.Stage.ChartColor,
                    objPropCustomer.Stage.Count, objPropCustomer.Stage.Label, objPropCustomer.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocCoordinates(Customer objPropCustomer)
        {
            StringBuilder varname1 = new StringBuilder();
            if (objPropCustomer.Status == 0)
            {
                varname1.Append(" update r set r.Lat='',r.lng='' FROM   loc l   \n");
                varname1.Append(" INNER JOIN rol r ON r.id = l.rol \n");
                varname1.Append(" Inner join Contract c on c.Loc = l.Loc \n");
                varname1.Append(" where(len(isnull(r.lng, '')) <= 5  or  len(isnull(r.lng, '')) <= 5) \n");
                varname1.Append("SELECT CASE l.route \n");
                varname1.Append("         WHEN 0 THEN 'Unassigned' \n");
                varname1.Append("         ELSE ((SELECT NAME \n");
                varname1.Append("         FROM   route \n");
                varname1.Append("         WHERE  id = l.route)) \n");
                varname1.Append("         END  AS worker,");
                varname1.Append("       l.loc, \n");
                varname1.Append("        (ISNULL( r.lat,'') + ',' +ISNULL( r.lng,'') )      AS coordinates, \n");
                varname1.Append("       Replace(tag, '''', '`') AS tagrep, \n");
                varname1.Append("       tag, \n");
                varname1.Append("       l.address, \n");
                varname1.Append("       r.EN, \n");
                varname1.Append("       isnull(B.Name, '') As Company, \n");
                varname1.Append("       l.city, \n");
                varname1.Append("       lat, \n");
                varname1.Append("       lng, \n");
                varname1.Append("       tag                     AS title, \n");
                varname1.Append("       (l.address +' ,'+ l.city +' ,' +l.state)              AS description, \n");

                //if (objPropCustomer.Status == 0)
                //{
                varname1.Append("       isnull(Round (CASE c.BCycle \n");

                varname1.Append("                WHEN 0 THEN c.BAmt    --Monthly  \n");
                varname1.Append("                WHEN 1 THEN c.BAmt / 2  --Bi-Monthly  \n");
                varname1.Append("                WHEN 2 THEN c.BAmt / 3 --Quarterly  \n");
                varname1.Append("                WHEN 3 THEN c.BAmt / 4 --3 Times/Year  \n");
                varname1.Append("                WHEN 4 THEN c.BAmt / 6 --Semi-Annually   \n");
                varname1.Append("				 WHEN 5 THEN c.BAmt / 12 --Annually \n");
                varname1.Append("                WHEN 6 THEN   0         --'Never'  \n");
                varname1.Append("                WHEN 7 THEN c.BAmt / (12*3)   --'3 Years'  \n");
                varname1.Append("                WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");
                varname1.Append("                WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");
                varname1.Append("                else 0 \n");
                varname1.Append("              END, 2) ,0)         AS MonthlyBill, \n");

                //Monthly Hours
                varname1.Append("   isnull(Round (CASE c.SCycle \n");
                varname1.Append("   WHEN 0 THEN ( c.Hours ) --Monthly  \n");
                varname1.Append("   WHEN 1 THEN ( c.Hours / ( 2 ) ) --Bi-Monthly  \n");
                varname1.Append("   WHEN 2 THEN ( c.Hours / ( 3 ) ) --Quarterly  \n");
                varname1.Append("   WHEN 3 THEN ( c.Hours / ( 6 ) ) --Semi-Annually  \n");
                varname1.Append("   WHEN 4 THEN ( c.Hours / ( 12 ) ) --Annually  \n");
                varname1.Append("   WHEN 5 THEN ( c.Hours * 4.3 ) --Weekly  \n");
                varname1.Append("   WHEN 6 THEN ( c.Hours * ( 2.15 ) ) --Bi-Weekly  \n");
                varname1.Append("   WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks  \n");
                varname1.Append("   WHEN 10 THEN ( c.Hours / ( 12 * 2 ) ) --Every 2 Years  \n");
                varname1.Append("   WHEN 8 THEN ( c.Hours / ( 12 * 3 ) ) --Every 3 Years  \n");
                varname1.Append("   WHEN 9 THEN ( c.Hours / ( 12 * 5 ) ) --Every 5 Years  \n");
                varname1.Append("   WHEN 11 THEN ( c.Hours / ( 12 * 7 ) ) --Every 7 Years \n");
                varname1.Append("   WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
                varname1.Append("   WHEN 14 THEN (( c.Hours * ( 2 ) )) --Twice a Month \n");
                varname1.Append("   WHEN 15 THEN (c.Hours / (4) ) --3 Times/Year  \n");
                varname1.Append("                else 0 \n");
                varname1.Append("              END, 2) ,0)         AS MonthlyHours, \n");

                varname1.Append("       (SELECT Count(1) \n");
                varname1.Append("        FROM   tblJoinElevJob \n");
                varname1.Append("        WHERE  Job = c.job)    AS elevcount, \n");

                varname1.Append("       isnull(c.BAmt,0) as bamt, \n");
                varname1.Append("       isnull(c.Hours,0) as Hours, \n");
                varname1.Append("       c.job , \n");

                varname1.Append("  CASE c.bcycle  \n");
                varname1.Append("  WHEN 0 THEN 'Monthly'  \n");
                varname1.Append("WHEN 1 THEN 'Bi-Monthly'  \n");
                varname1.Append(" WHEN 2 THEN 'Quarterly'  \n");
                varname1.Append(" WHEN 3 THEN '3 Times/Year'  \n");
                varname1.Append(" WHEN 4 THEN 'Semi-Annually'  \n");
                varname1.Append("  WHEN 5 THEN 'Annually'  \n");
                varname1.Append("  WHEN 6 THEN 'Never'  \n");
                varname1.Append("  WHEN 7 THEN '3 Years'  \n");
                varname1.Append("  WHEN 8 THEN '5 Years'  \n");
                varname1.Append("  WHEN 9 THEN '2 Years'  \n");
                varname1.Append(" END Freqency,  \n");

                varname1.Append(" CASE c.scycle  \n");
                varname1.Append("   WHEN - 1 THEN 'Never'  \n");
                varname1.Append("   WHEN 0 THEN 'Monthly'  \n");
                varname1.Append("  WHEN 1 THEN 'Bi-Monthly'  \n");
                varname1.Append("  WHEN 2 THEN 'Quarterly'  \n");
                varname1.Append("  WHEN 3 THEN 'Semi-Annually'  \n");
                varname1.Append("  WHEN 4 THEN 'Annually'  \n");
                varname1.Append("  WHEN 5 THEN 'Weekly'  \n");
                varname1.Append("  WHEN 6 THEN 'Bi-Weekly'  \n");
                varname1.Append("  WHEN 7 THEN 'Every 13 Weeks'  \n");
                varname1.Append("  WHEN 10 THEN 'Every 2 Years'  \n");
                varname1.Append("  WHEN 8 THEN 'Every 3 Years'  \n");
                varname1.Append("  WHEN 9 THEN 'Every 5 Years'  \n");
                varname1.Append("  WHEN 11 THEN 'Every 7 Years'  \n");
                varname1.Append("  WHEN 12 THEN 'On-Demand'  \n");
                varname1.Append("  WHEN 13 THEN 'Daily'  \n");
                varname1.Append(" WHEN 14 THEN 'Twice a Month' \n");
                varname1.Append("  WHEN 15 THEN '3 Times/Year' \n");
                varname1.Append(" END TicketFreq  \n");

                varname1.Append("FROM   loc l \n");
                varname1.Append("       INNER JOIN rol r ON r.id = l.rol \n");
                varname1.Append("       LEFT OUTER JOIN Branch B on B.ID = r.EN \n");
                varname1.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN \n");
                varname1.Append(" inner join Contract c on c.Loc=l.Loc \n");
                varname1.Append(" inner join job j on c.job=j.id \n");
                varname1.Append(" where l.id is not null \n");

                if (objPropCustomer.NullAddressOnly == true)
                {
                    varname1.Append(" and isnull(r.lat,'')= '' \n");
                }
                if (objPropCustomer.LocIDs != null)
                {
                    if (objPropCustomer.LocIDs != string.Empty)
                        varname1.Append(" and l.loc in(" + objPropCustomer.LocIDs + ") \n");
                    else
                        varname1.Append(" and l.loc = 0 \n");
                }
                if (objPropCustomer.Worker != -1)
                {
                    ///0   Get loc which Default Worker is Unassigned
                    varname1.Append(" and l.route=" + objPropCustomer.Worker + "\n");
                }

                varname1.Append(
                    "  AND (((select top 1 name from rol where id=(select top 1 Rol from Owner o where o.ID=l.Owner)) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(Tag LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.ID LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.city LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.state LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (r.Address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')   OR (r.City LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.Zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')) \n");
                varname1.Append(" and c.status=0 \n");
                if (objPropCustomer.EN == 1)
                {
                    varname1.Append(" and UC.IsSel = 1 and UC.UserID =" + objPropCustomer.UserID);
                }
                varname1.Append("            order by tag ");
            }
            else
            {
                varname1.Append("SELECT worker, \n");
                varname1.Append("       loc, \n");
                varname1.Append("       coordinates, \n");
                varname1.Append("       tagrep, \n");
                varname1.Append("       tag, \n");
                varname1.Append("       address, \n");
                varname1.Append("       EN, \n");
                varname1.Append("       Company, \n");
                varname1.Append("       city, \n");
                varname1.Append("       lat, \n");
                varname1.Append("       lng, \n");
                varname1.Append("       title, \n");
                varname1.Append("       description, \n");
                varname1.Append("       Route, \n");
                varname1.Append("       sum(elevcount) as elevcount, \n");
                varname1.Append("       Sum(MonthlyBill)  AS MonthlyBill, \n");
                varname1.Append("       Sum(MonthlyHours) AS MonthlyHours, \n");
                varname1.Append("       Sum(bamt)         AS bamt, \n");
                varname1.Append("       Sum(Hours)        AS Hours, \n");
                varname1.Append("       0        AS job , Freqency ,  TicketFreq \n");




                varname1.Append("FROM   (SELECT (SELECT name \n");
                varname1.Append("                FROM   route \n");
                varname1.Append("                WHERE  id = l.route)                             AS worker, \n");
                varname1.Append("               l.loc, \n");
                varname1.Append("               ( Isnull( r.lat, '') + ',' + Isnull( r.lng, '') ) AS coordinates, \n");
                varname1.Append("               Replace(tag, '''', '`')                           AS tagrep, \n");
                varname1.Append("               tag, \n");
                varname1.Append("               l.address, \n");
                varname1.Append("       r.EN, \n");
                varname1.Append("       isnull(B.Name, '') As Company, \n");
                varname1.Append("               l.city, \n");
                varname1.Append("               lat, \n");
                varname1.Append("               lng, \n");
                varname1.Append("               tag                                               AS title, \n");
                varname1.Append("               ( l.address + ' ,' + l.city + ' ,' + l.state )    AS description, \n");
                varname1.Append("       isnull(Round (CASE c.BCycle \n");
                varname1.Append("                WHEN 0 THEN c.bamt --Monthly \n");
                varname1.Append("                WHEN 1 THEN c.bamt / 2 --Bi-Monthly \n");
                varname1.Append("                WHEN 2 THEN c.bamt / 3 --Quarterly \n");
                varname1.Append("                WHEN 3 THEN c.bamt / 4 --3timesyr \n");
                varname1.Append("                WHEN 4 THEN c.bamt / 6 --semiannual \n");
                varname1.Append("                WHEN 5 THEN c.bamt / 12 --annual \n");
                varname1.Append("                WHEN 6 THEN  0     --never \n");
                varname1.Append("                WHEN 7 THEN c.BAmt / (12*3)   --'3 Years'  \n");
                varname1.Append("                WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");
                varname1.Append("                WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");
                varname1.Append("                else 0 \n");
                varname1.Append("              END, 2) ,0)         AS MonthlyBill, \n");

                varname1.Append("Isnull(Round (CASE c.SCycle WHEN 0 THEN ( CASE SWE WHEN 1 THEN Hours * 30 ELSE Hours * 21.66 END ) --daily, \n");
                varname1.Append("WHEN 1 THEN ( Hours * 12.99 ) -- threeXweek \n");
                varname1.Append("WHEN 2 THEN ( Hours * 8.60 ) -- twoXweek \n");
                varname1.Append("WHEN 3 THEN ( Hours * 4.30 ) -- weekly \n");
                varname1.Append("WHEN 4 THEN ( Hours * 2.17 ) -- biweekly \n");
                varname1.Append("WHEN 5 THEN ( Hours * 2 ) -- semimonthly \n");
                varname1.Append("WHEN 6 THEN Hours -- monthly \n");
                varname1.Append("WHEN 7 THEN ( Hours / 1.37991 ) -- evry6weeks \n");
                varname1.Append("WHEN 8 THEN ( Hours / 2.00 )-- bimonyhly \n");
                varname1.Append("WHEN 9 THEN ( Hours / 3.00 )-- quart \n");
                varname1.Append("WHEN 10 THEN ( Hours / 4.00 )-- threetimeperyr \n");
                varname1.Append("WHEN 11 THEN ( Hours / 6.00 )-- semiannualy \n");
                varname1.Append("WHEN 12 THEN ( Hours / 12.00 )-- annualy \n");
                varname1.Append("WHEN 13 THEN 0.00 --never \n");
                varname1.Append("WHEN 14 THEN ( Hours * 1.44 )-- every4w \n");
                varname1.Append("WHEN 15 THEN ( Hours * 1.08 )-- every5w \n");
                varname1.Append("WHEN 16 THEN ( Hours * 0.87 )-- every3w \n");
                varname1.Append("WHEN 17 THEN ( Hours / 1.83988 )-- every8w \n");
                varname1.Append("WHEN 18 THEN ( Hours / 2.9898 )-- every13w \n");
                varname1.Append("WHEN 19 THEN ( Hours / 2.29984 )-- every10w \n");
                varname1.Append("else 0 \n");
                varname1.Append("END, 2), 0) AS MonthlyHours, ");

                //varname1.Append(" (select count(1) from elev el where el.loc=l.loc and el.status=0 ) as elevcount, \n");
                varname1.Append(" (case  when j.fgroup is not null then( select count(1)from elev el where el.loc=l.loc and el.status=0 and el.fGroup=j.fGroup ) else (select count(1)from elev el where el.loc=l.loc and el.status=0) end) as elevcount, \n");

                varname1.Append("               Isnull(c.BAmt, 0)                                 AS bamt, \n");
                varname1.Append("               Isnull(c.Hours, 0)                                AS Hours, \n");
                varname1.Append("               l.route , \n");


                varname1.Append("  CASE c.bcycle  \n");
                varname1.Append("  WHEN 0 THEN 'Monthly'  \n");
                varname1.Append("WHEN 1 THEN 'Bi-Monthly'  \n");
                varname1.Append(" WHEN 2 THEN 'Quarterly'  \n");
                varname1.Append(" WHEN 3 THEN '3 Times/Year'  \n");
                varname1.Append(" WHEN 4 THEN 'Semi-Annually'  \n");
                varname1.Append("  WHEN 5 THEN 'Annually'  \n");
                varname1.Append("  WHEN 6 THEN 'Never'  \n");
                varname1.Append("  WHEN 7 THEN '3 Years'  \n");
                varname1.Append("  WHEN 8 THEN '5 Years'  \n");
                varname1.Append("  WHEN 9 THEN '2 Years'  \n");
                varname1.Append(" END Freqency,  \n");

                varname1.Append(" CASE c.scycle  \n");
                varname1.Append("   WHEN - 1 THEN 'Never'  \n");
                varname1.Append("   WHEN 0 THEN 'Monthly'  \n");
                varname1.Append("  WHEN 1 THEN 'Bi-Monthly'  \n");
                varname1.Append("  WHEN 2 THEN 'Quarterly'  \n");
                varname1.Append("  WHEN 3 THEN 'Semi-Annually'  \n");
                varname1.Append("  WHEN 4 THEN 'Annually'  \n");
                varname1.Append("  WHEN 5 THEN 'Weekly'  \n");
                varname1.Append("  WHEN 6 THEN 'Bi-Weekly'  \n");
                varname1.Append("  WHEN 7 THEN 'Every 13 Weeks'  \n");
                varname1.Append("  WHEN 10 THEN 'Every 2 Years'  \n");
                varname1.Append("  WHEN 8 THEN 'Every 3 Years'  \n");
                varname1.Append("  WHEN 9 THEN 'Every 5 Years'  \n");
                varname1.Append("  WHEN 11 THEN 'Every 7 Years'  \n");
                varname1.Append("  WHEN 12 THEN 'On-Demand'  \n");
                varname1.Append("  WHEN 13 THEN 'Daily'  \n");
                varname1.Append(" WHEN 14 THEN 'Twice a Month' \n");
                varname1.Append(" END TicketFreq  \n");


                varname1.Append("        FROM   loc l \n");
                varname1.Append("               INNER JOIN rol r \n");
                varname1.Append("                       ON r.id = l.rol \n");
                varname1.Append("       LEFT OUTER JOIN Branch B on B.ID = r.EN \n");
                varname1.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("               INNER JOIN job j \n");
                varname1.Append("                       ON c.job = j.id \n");
                varname1.Append("        WHERE  l.id IS NOT NULL \n");

                if (objPropCustomer.NullAddressOnly == true)
                {
                    varname1.Append(" and isnull(r.lat,'')= '' \n");
                }
                if (objPropCustomer.LocIDs != null)
                {
                    if (objPropCustomer.LocIDs != string.Empty)
                        varname1.Append(" and l.loc in(" + objPropCustomer.LocIDs + ") \n");
                    else
                        varname1.Append(" and l.loc = 0 \n");
                }
                if (objPropCustomer.Worker != -1)
                {
                    ///0   Get loc which Default Worker is Unassigned
                    varname1.Append(" and l.route=" + objPropCustomer.Worker + "\n");
                }

                varname1.Append(
                    "  AND (((select top 1 name from rol where id=(select top 1 Rol from Owner o where o.ID=l.Owner)) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(Tag LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.ID LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.city LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.state LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (r.Address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')   OR (r.City LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.Zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')) \n");
                varname1.Append("               AND c.status = 0 \n");
                if (objPropCustomer.EN == 1)
                {
                    varname1.Append(" and UC.IsSel = 1  and UC.UserID =" + objPropCustomer.UserID);
                }
                varname1.Append("          ) t \n");
                varname1.Append("GROUP  BY worker, \n");
                varname1.Append("          loc, \n");
                varname1.Append("          coordinates, \n");
                varname1.Append("          tagrep, \n");
                varname1.Append("          tag, \n");
                varname1.Append("          address, \n");
                varname1.Append("          EN, \n");
                varname1.Append("          Company, \n");
                varname1.Append("          city, \n");
                varname1.Append("          lat, \n");
                varname1.Append("          lng, \n");
                //varname1.Append("          elevcount, \n");
                varname1.Append("          title, \n");
                varname1.Append("          description, \n");
                varname1.Append("          Route , Freqency ,TicketFreq  \n");
                varname1.Append("ORDER  BY tag ");

            }
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWorkers(Customer objPropCustomer)
        {
            StringBuilder varname1 = new StringBuilder();


            varname1.Append("CREATE TABLE #tempRoute \n");
            varname1.Append("  ( \n");
            varname1.Append("     ID   INT, \n");
            varname1.Append("     NAME VARCHAR(50) \n");
            varname1.Append("  ) \n");
            varname1.Append("");
            varname1.Append("INSERT INTO #tempRoute \n");
            varname1.Append("            (ID, \n");
            varname1.Append("             NAME) \n");
            varname1.Append("VALUES     (0, \n");
            varname1.Append("            'Unassigned') \n");
            varname1.Append("");
            varname1.Append("INSERT INTO #tempRoute \n");
            varname1.Append("SELECT ID, \n");
            varname1.Append("       NAME \n");
            varname1.Append("FROM   Route \n");


            varname1.Append("SELECT Name, \n");
            varname1.Append("       id, \n");

            if (objPropCustomer.Status == 0)
            {
                varname1.Append("       (SELECT Count (1) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("               ) AS contr, \n");

                varname1.Append("       (SELECT Count(1) \n");
                varname1.Append("        FROM   tblJoinElevJob ej \n");
                varname1.Append("               INNER JOIN Contract cc \n");
                varname1.Append("                       ON cc.Job = ej.Job \n");
                varname1.Append("               INNER JOIN job jj \n");
                varname1.Append("                       ON jj.ID = cc.Job \n");
                varname1.Append("        WHERE  jj.Custom20 = r.ID and cc.status =0)AS units, \n");

                varname1.Append(" Isnull((SELECT Sum (Isnull(Round (CASE c.sCycle \n");
                // Monthly Hours
                varname1.Append("   WHEN 0 THEN ( c.Hours ) --Monthly  \n");
                varname1.Append("   WHEN 1 THEN ( c.Hours / ( 2 ) ) --Bi-Monthly  \n");
                varname1.Append("   WHEN 2 THEN ( c.Hours / ( 3 ) ) --Quarterly  \n");
                varname1.Append("   WHEN 3 THEN ( c.Hours / ( 6 ) ) --Semi-Annually  \n");
                varname1.Append("   WHEN 4 THEN ( c.Hours / ( 12 ) ) --Annually  \n");
                varname1.Append("   WHEN 5 THEN ( c.Hours * 4.3 ) --Weekly  \n");
                varname1.Append("   WHEN 6 THEN ( c.Hours * ( 2.15 ) ) --Bi-Weekly  \n");
                varname1.Append("   WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks  \n");
                varname1.Append("   WHEN 10 THEN ( c.Hours / ( 12 * 2 ) ) --Every 2 Years  \n");
                varname1.Append("   WHEN 8 THEN ( c.Hours / ( 12 * 3 ) ) --Every 3 Years  \n");
                varname1.Append("   WHEN 9 THEN ( c.Hours / ( 12 * 5 ) ) --Every 5 Years  \n");
                varname1.Append("   WHEN 11 THEN ( c.Hours / ( 12 * 7 ) ) --Every 7 Years \n");
                varname1.Append("   WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE  21.66 END) ) --Daily \n");
                varname1.Append("   WHEN 14 THEN (( c.Hours * ( 2 ) )) --Twice a Month \n");
                varname1.Append("                                    ELSE 0 \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("              ),0)  AS MonthlyHours, \n");

                varname1.Append(" Isnull((SELECT Sum (Isnull(Round (CASE c.BCycle \n");


                varname1.Append("                WHEN 0 THEN c.BAmt    --Monthly  \n");
                varname1.Append("                WHEN 1 THEN c.BAmt / 2  --Bi-Monthly  \n");
                varname1.Append("                WHEN 2 THEN c.BAmt / 3 --Quarterly  \n");
                varname1.Append("                WHEN 3 THEN c.BAmt / 4 --3 Times/Year  \n");
                varname1.Append("                WHEN 4 THEN c.BAmt / 6 --Semi-Annually   \n");
                varname1.Append("				 WHEN 5 THEN c.BAmt / 12 --Annually \n");
                varname1.Append("                WHEN 6 THEN   0         --'Never'  \n");
                varname1.Append("                WHEN 7 THEN c.BAmt / (12*3)   --'3 Years'  \n");
                varname1.Append("                WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");
                varname1.Append("                WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");
                varname1.Append("                                    ELSE 0 \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID \n");
                varname1.Append("               AND c.status = 0) , 0) AS MonthlyBill ");
            }
            else
            {
                varname1.Append("       (SELECT  Count (distinct l.loc) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("               ) AS contr, \n");

                //varname1.Append("  (select dbo.CalculateRouteUnits(r.id)) as units, \n");
                //varname1.Append("   (select count(1) from Elev el where el.status=0 and el.loc in ( select l.loc from loc l inner join contract c on l.loc=c.loc where l.route=r.id and c.status=0 )  ) as units, \n");
                varname1.Append("(SELECT isnull( Sum (elevcount),0) \n");
                varname1.Append(" FROM   (SELECT ( CASE \n");
                varname1.Append("                    WHEN j.fgroup IS NOT NULL THEN(SELECT Count(1) \n");
                varname1.Append("                                                   FROM   elev el \n");
                varname1.Append("                                                   WHERE  el.loc = l.loc \n");
                varname1.Append("                                                          AND el.status = 0 \n");
                varname1.Append("                                                          AND el.fGroup = j.fGroup) \n");
                varname1.Append("                    ELSE 1 \n");
                varname1.Append("                  END ) AS elevcount \n");
                varname1.Append("         FROM   Loc l \n");
                varname1.Append("                INNER JOIN Contract c \n");
                varname1.Append("                        ON c.Loc = l.Loc \n");
                varname1.Append("                INNER JOIN Job j \n");
                varname1.Append("                        ON c.Job = j.ID \n");
                varname1.Append("         WHERE  c.Status = 0 \n");
                varname1.Append("                AND l.Route = r.ID) t)  as units,");

                varname1.Append("isnull( (SELECT Sum (Isnull(Round (CASE c.sCycle \n");
                varname1.Append("WHEN 0 THEN ( CASE SWE WHEN 1 THEN Hours * 30 ELSE Hours * 21.66 END ) --daily, \n");
                varname1.Append("WHEN 1 THEN ( Hours * 12.99 ) -- threeXweek \n");
                varname1.Append("WHEN 2 THEN ( Hours * 8.60 ) -- twoXweek \n");
                varname1.Append("WHEN 3 THEN ( Hours * 4.30 ) -- weekly \n");
                varname1.Append("WHEN 4 THEN ( Hours * 2.17 ) -- biweekly \n");
                varname1.Append("WHEN 5 THEN ( Hours * 2 ) -- semimonthly \n");
                varname1.Append("WHEN 6 THEN Hours -- monthly \n");
                varname1.Append("WHEN 7 THEN ( Hours / 1.37991 ) -- evry6weeks \n");
                varname1.Append("WHEN 8 THEN ( Hours / 2.00 )-- bimonyhly \n");
                varname1.Append("WHEN 9 THEN ( Hours / 3.00 )-- quart \n");
                varname1.Append("WHEN 10 THEN ( Hours / 4.00 )-- threetimeperyr \n");
                varname1.Append("WHEN 11 THEN ( Hours / 6.00 )-- semiannualy \n");
                varname1.Append("WHEN 12 THEN ( Hours / 12.00 )-- annualy \n");
                varname1.Append("WHEN 13 THEN 0.00 --never \n");
                varname1.Append("WHEN 14 THEN ( Hours * 1.44 )-- every4w \n");
                varname1.Append("WHEN 15 THEN ( Hours * 1.08 )-- every5w \n");
                varname1.Append("WHEN 16 THEN ( Hours * 0.87 )-- every3w \n");
                varname1.Append("WHEN 17 THEN ( Hours / 1.83988 )-- every8w \n");
                varname1.Append("WHEN 18 THEN ( Hours / 2.9898 )-- every13w \n");
                varname1.Append("WHEN 19 THEN ( Hours / 2.29984 )-- every10w \n");
                varname1.Append("else 0 \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("              ),0)  AS MonthlyHours, \n");

                varname1.Append("isnull((SELECT Sum (Isnull(Round (CASE c.BCycle \n");
                varname1.Append("                WHEN 0 THEN c.bamt --Monthly \n");
                varname1.Append("                WHEN 1 THEN c.bamt / 2 --Bi-Monthly \n");
                varname1.Append("                WHEN 2 THEN c.bamt / 3 --Quarterly \n");
                varname1.Append("                WHEN 3 THEN c.bamt / 4 --3timesyr \n");
                varname1.Append("                WHEN 4 THEN c.bamt / 6 --semiannual \n");
                varname1.Append("                WHEN 5 THEN c.bamt / 12 --annual \n");
                varname1.Append("                WHEN 6 THEN 0 --never \n");
                varname1.Append("                WHEN 7 THEN c.BAmt / (12*3)   --'3 Years'  \n");
                varname1.Append("                WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");
                varname1.Append("                WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");
                varname1.Append("                else 0  \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID \n");
                varname1.Append("               AND c.status = 0),0) AS MonthlyBill ");
            }

            varname1.Append("FROM   #tempRoute r \n ");

            if (!string.IsNullOrEmpty(objPropCustomer.Name))
            {
                varname1.Append("WHERE  r.Name IN ( " + objPropCustomer.Name + " ) \n");
            }

            varname1.Append("order by name \n  ");

            varname1.Append(" \n  Drop table #tempRoute \n ");

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWorkerMonthly(Customer objPropCustomer)
        {
            StringBuilder varname1 = new StringBuilder();

            //if (objPropCustomer.Status == 0)
            //{
            //    varname1.Append("SELECT r.Name, \n");
            //    varname1.Append("      isnull( Sum (c.BAmt),0)        BAmt, \n");
            //    varname1.Append("      isnull( Sum(c.Hours),0)        Hours, \n");
            //    varname1.Append("      isnull( Sum(Round (CASE c.BCycle \n");
            //    varname1.Append("                WHEN 0 THEN c.BAmt \n");
            //    varname1.Append("                WHEN 1 THEN c.BAmt / 2 \n");
            //    varname1.Append("                WHEN 2 THEN c.BAmt / 3 \n");
            //    varname1.Append("                WHEN 3 THEN c.BAmt / 6 \n");
            //    varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
            //    varname1.Append("                    WHEN 6 THEN 0 \n");
            //    varname1.Append("                else 0  \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyBill, \n");
            //    varname1.Append("      isnull( Sum(Round (CASE c.SCycle \n");
            //    varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
            //    varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
            //    varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
            //    varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
            //    varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
            //    //varname1.Append("                WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
            //    //varname1.Append("                WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
            //    varname1.Append("                else 0  \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyHours, \n");

            //    varname1.Append("       Count(c.job)                   AS contr, \n");

            //    varname1.Append("       (SELECT Count(1) \n");
            //    varname1.Append("        FROM   tblJoinElevJob ej \n");
            //    varname1.Append("               INNER JOIN Contract cc \n");
            //    varname1.Append("                       ON cc.Job = ej.Job \n");
            //    varname1.Append("               INNER JOIN job jj \n");
            //    varname1.Append("                       ON jj.ID = cc.Job \n");
            //    varname1.Append("        WHERE  jj.Custom20 = r.ID and cc.status=0)AS units \n");

            //    varname1.Append("FROM   route r \n");
            //    varname1.Append("       LEFT OUTER JOIN Loc l \n");
            //    varname1.Append("                    ON r.ID = l.Route \n");
            //    varname1.Append("       LEFT OUTER JOIN Contract c \n");
            //    varname1.Append("                    ON l.Loc = c.Loc \n");
            //    varname1.Append("                       AND c.Status = 0 \n");
            //    varname1.Append("WHERE  r.Name IN ( " + objPropCustomer.Name + " ) \n"); 
            //    varname1.Append("GROUP  BY r.Name, r.id ");
            //}
            //else
            //{
            //    varname1.Append("SELECT r.Name, \n");
            //    varname1.Append("      isnull( Sum (c.BAmt),0)        BAmt, \n");
            //    varname1.Append("      isnull( Sum(c.Hours),0)        Hours, \n");

            //    varname1.Append("      isnull( Sum(Round (CASE c.BCycle \n");
            //    varname1.Append("                WHEN 0 THEN c.bamt --Monthly \n");
            //    varname1.Append("                WHEN 1 THEN c.bamt / 2 --Bi-Monthly \n");
            //    varname1.Append("                WHEN 2 THEN c.bamt / 3 --Quarterly \n");
            //    varname1.Append("                WHEN 3 THEN c.bamt / 4 --3timesyr \n");
            //    varname1.Append("                WHEN 4 THEN c.bamt / 6 --semiannual \n");
            //    varname1.Append("                WHEN 5 THEN c.bamt / 12 --annual \n");
            //    varname1.Append("                WHEN 6 THEN 0 --never \n");
            //    varname1.Append("                else 0  \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyBill, \n");

            //    varname1.Append("      isnull( Sum(Round (CASE c.SCycle \n");
            //    varname1.Append("WHEN 0 THEN ( CASE SWE WHEN 1 THEN Hours * 30 ELSE Hours * 21.66 END ) --daily, \n");
            //    varname1.Append("WHEN 1 THEN ( Hours * 12.99 ) -- threeXweek \n");
            //    varname1.Append("WHEN 2 THEN ( Hours * 8.60 ) -- twoXweek \n");
            //    varname1.Append("WHEN 3 THEN ( Hours * 4.30 ) -- weekly \n");
            //    varname1.Append("WHEN 4 THEN ( Hours * 2.17 ) -- biweekly \n");
            //    varname1.Append("WHEN 5 THEN ( Hours * 2 ) -- semimonthly \n");
            //    varname1.Append("WHEN 6 THEN Hours -- monthly \n");
            //    varname1.Append("WHEN 7 THEN ( Hours / 1.37991 ) -- evry6weeks \n");
            //    varname1.Append("WHEN 8 THEN ( Hours / 2.00 )-- bimonyhly \n");
            //    varname1.Append("WHEN 9 THEN ( Hours / 3.00 )-- quart \n");
            //    varname1.Append("WHEN 10 THEN ( Hours / 4.00 )-- threetimeperyr \n");
            //    varname1.Append("WHEN 11 THEN ( Hours / 6.00 )-- semiannualy \n");
            //    varname1.Append("WHEN 12 THEN ( Hours / 12.00 )-- annualy \n");
            //    varname1.Append("WHEN 13 THEN 0.00 --never \n");
            //    varname1.Append("WHEN 14 THEN ( Hours * 1.44 )-- every4w \n");
            //    varname1.Append("WHEN 15 THEN ( Hours * 1.08 )-- every5w \n");
            //    varname1.Append("WHEN 16 THEN ( Hours * 0.87 )-- every3w \n");
            //    varname1.Append("WHEN 17 THEN ( Hours / 1.83988 )-- every8w \n");
            //    varname1.Append("WHEN 18 THEN ( Hours / 2.9898 )-- every13w \n");
            //    varname1.Append("WHEN 19 THEN ( Hours / 2.29984 )-- every10w \n");
            //    varname1.Append("else 0 \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyHours, \n");

            //    varname1.Append("       Count( distinct l.loc)                   AS contr, \n");

            //    //varname1.Append("   (select count(1) from Elev el where el.status=0 and el.loc in ( select l.loc from loc l inner join contract c on l.loc=c.loc where l.route=r.id and c.status=0 )  ) as units \n");
            //    //varname1.Append("  (select dbo.CalculateRouteUnits(r.id)) as units \n");
            //    varname1.Append("(SELECT isnull( Sum (elevcount),0) \n");
            //    varname1.Append(" FROM   (SELECT ( CASE \n");
            //    varname1.Append("                    WHEN j.fgroup IS NOT NULL THEN(SELECT Count(1) \n");
            //    varname1.Append("                                                   FROM   elev el \n");
            //    varname1.Append("                                                   WHERE  el.loc = l.loc \n");
            //    varname1.Append("                                                          AND el.status = 0 \n");
            //    varname1.Append("                                                          AND el.fGroup = j.fGroup) \n");
            //    varname1.Append("                    ELSE 1 \n");
            //    varname1.Append("                  END ) AS elevcount \n");
            //    varname1.Append("         FROM   Loc l \n");
            //    varname1.Append("                INNER JOIN Contract c \n");
            //    varname1.Append("                        ON c.Loc = l.Loc \n");
            //    varname1.Append("                INNER JOIN Job j \n");
            //    varname1.Append("                        ON c.Job = j.ID \n");
            //    varname1.Append("         WHERE  c.Status = 0 \n");
            //    varname1.Append("                AND l.Route = r.ID) t)  as units");

            //    varname1.Append("FROM   route r \n");
            //    varname1.Append("       LEFT OUTER JOIN Loc l \n");
            //    varname1.Append("                    ON r.ID = l.Route \n");
            //    varname1.Append("       LEFT OUTER JOIN Contract c \n");
            //    varname1.Append("                    ON l.Loc = c.Loc \n");
            //    varname1.Append("                       AND c.Status = 0 \n");
            //    varname1.Append("WHERE  r.Name IN ( " + objPropCustomer.Name + " ) \n"); 
            //    varname1.Append("GROUP  BY r.Name, r.id ");
            //}

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocRoute(Customer objPropCustomer)
        {
            SqlParameter para;
            para = new SqlParameter
            {
                ParameterName = "Locations",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtTemplateData
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateLocRoute", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetWorkerCalculations(Customer objPropCustomer)
        {
            SqlParameter para;
            para = new SqlParameter
            {
                ParameterName = "WorkerData",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.dtWorkerData
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spWorkerChangeCalculation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int AddRouteTemplate(Customer objPropCustomer)
        {
            var para = new SqlParameter[10];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };

            para[1] = new SqlParameter
            {
                ParameterName = "sequence",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RouteSequence
            };

            para[2] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };

            para[3] = new SqlParameter { ParameterName = "Mode", SqlDbType = SqlDbType.Int, Value = objPropCustomer.Mode };

            para[4] = new SqlParameter
            {
                ParameterName = "TemplateID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };

            //para[5] = new SqlParameter();
            //para[5].ParameterName = "TemplateData";
            //para[5].SqlDbType = SqlDbType.Structured;
            //para[5].Value = objPropCustomer.DtTemplateData;

            para[5] = new SqlParameter { ParameterName = "worker", SqlDbType = SqlDbType.Int, Value = DBNull.Value };

            para[6] = new SqlParameter
            {
                ParameterName = "Center",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Center
            };

            para[7] = new SqlParameter
            {
                ParameterName = "Radius",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Radius
            };

            para[8] = new SqlParameter
            {
                ParameterName = "Overlay",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Overlay
            };

            para[9] = new SqlParameter
            {
                ParameterName = "PolygonCoord",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.PolygonCoord
            };

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spRouteTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getRouteTemplate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from tblroutetemplate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTemplateByID(Customer objPropCustomer)
        {
            string str = "select (select name from route where id=t.worker) as workername, t.* from tblroutetemplate t where templateid=" + objPropCustomer.TemplateID + "";

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT ID, \n");
            //varname1.Append("       TemplateID, \n");
            //varname1.Append("       t.Loc, \n");
            //varname1.Append("       Worker                            AS workerid, \n");
            //varname1.Append("       (SELECT name \n");
            //varname1.Append("        FROM   route \n");
            //varname1.Append("        WHERE  id = (SELECT Route \n");
            //varname1.Append("                     FROM   Loc \n");
            //varname1.Append("                     WHERE  Loc = t.Loc))AS worker, \n");
            //varname1.Append("       (SELECT Tag \n");
            //varname1.Append("        FROM   Loc \n");
            //varname1.Append("        WHERE  Loc = t.Loc)              AS tag, \n");
            //varname1.Append("      isnull( Round (CASE c.BCycle \n");
            //varname1.Append("                WHEN 0 THEN c.BAmt \n");
            //varname1.Append("                WHEN 1 THEN c.BAmt / 6 \n");
            //varname1.Append("                WHEN 2 THEN c.BAmt / 4 \n");
            //varname1.Append("                WHEN 3 THEN c.BAmt / 2 \n");
            //varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
            //varname1.Append("                    WHEN 6 THEN 0 \n");
            //varname1.Append("              END, 2)  ,0)                  AS MonthlyBill, \n");
            //varname1.Append("      isnull( Round (CASE c.SCycle \n");
            //varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
            //varname1.Append("                WHEN 1 THEN c.Hours / 6 --Bi-Monthly \n");
            //varname1.Append("                WHEN 2 THEN c.Hours / 4 --Quarterly \n");
            //varname1.Append("                WHEN 3 THEN c.Hours / 2 --Semi-Anually \n");
            //varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
            //varname1.Append("                WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
            //varname1.Append("                WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
            //varname1.Append("              END, 2) ,0)                   AS MonthlyHours \n");

            //if (objPropCustomer.Status == 0)
            //{
            //    varname1.Append("       ,(SELECT Count(1) \n");
            //    varname1.Append("        FROM   tblJoinElevJob \n");
            //    varname1.Append("        WHERE  Job = c.job)    AS elevcount \n");
            //}
            //else
            //{
            //    varname1.Append("        ,0    AS elevcount \n");
            //}


            //varname1.Append("FROM   tblTemplateDetails t \n");
            //varname1.Append("       INNER JOIN Contract c \n");
            //varname1.Append("               ON c.Loc = t.Loc ");

            //str += varname1.ToString();
            //str += " where templateid=" + objPropCustomer.TemplateID + "";


            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTask(Customer objPropCustomer)
        {
            try
            {
                objPropCustomer.TaskID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spAddTask", objPropCustomer.ROL, objPropCustomer.DueDate, objPropCustomer.TimeDue
                    , objPropCustomer.Subject, objPropCustomer.Remarks, objPropCustomer.AssignedTo, objPropCustomer.Name, objPropCustomer.Contact
                    , objPropCustomer.Mode, objPropCustomer.TaskID, objPropCustomer.Status, objPropCustomer.Resolution, objPropCustomer.LastUpdateUser
                    , objPropCustomer.Duration, objPropCustomer.Phone, objPropCustomer.Email, objPropCustomer.Screen, objPropCustomer.Ref
                    , objPropCustomer.Category, objPropCustomer.IsAlert));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTasks(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetToDoListForOppotunity", objPropCustomer.OpportunityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpportunity(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetOpportunity", objPropCustomer.SearchBy, objPropCustomer.SearchValue, objPropCustomer.StartDate, objPropCustomer.EndDate, objPropCustomer.ROL, IsSalesAsigned, objPropCustomer.EN, objPropCustomer.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpportunityNew(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetOpportunityNew", objPropCustomer.SearchBy, objPropCustomer.SearchValue, objPropCustomer.StartDate, objPropCustomer.EndDate, objPropCustomer.ROL, IsSalesAsigned, objPropCustomer.EN, objPropCustomer.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getOpportunityNew(GetOpportunityNewParam _GetOpportunityNew, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                return _GetOpportunityNew.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, "spGetOpportunityNew", _GetOpportunityNew.SearchBy, _GetOpportunityNew.SearchValue, _GetOpportunityNew.StartDate, _GetOpportunityNew.EndDate, _GetOpportunityNew.ROL, IsSalesAsigned, _GetOpportunityNew.EN, _GetOpportunityNew.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpportunityOfCustomer(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetOpportunityOfCustomer", objPropCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet getOpportunityOfCustomer(GetOpportunityOfCustomerParam _GetOpportunityOfCustomer, string ConnectionString)
        {
            try
            {
                return _GetOpportunityOfCustomer.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, "spGetOpportunityOfCustomer", _GetOpportunityOfCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEditOpportunity(Customer objPropCustomer, int? intDept)
        {
            try
            {
                #region Parametters
                var para = new SqlParameter[25];

                para[0] = new SqlParameter
                {
                    ParameterName = "ID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.OpportunityID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "fdesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Name
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "rol",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.ROL
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "Probability",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.Probability
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.Status
                };

                para[5] = new SqlParameter
                {
                    ParameterName = "Remarks",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Remarks
                };

                para[6] = new SqlParameter
                {
                    ParameterName = "closedate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropCustomer.EndDate
                };

                para[7] = new SqlParameter
                {
                    ParameterName = "Mode",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.Mode
                };

                para[8] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.ProspectID
                };


                para[9] = new SqlParameter
                {
                    ParameterName = "NextStep",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.NextStep
                };

                para[10] = new SqlParameter
                {
                    ParameterName = "desc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Description
                };

                para[11] = new SqlParameter
                {
                    ParameterName = "Source",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Source
                };

                para[12] = new SqlParameter
                {
                    ParameterName = "Amount",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objPropCustomer.Amount
                };

                para[13] = new SqlParameter
                {
                    ParameterName = "Fuser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Fuser
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "AssignedToID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.AssignedToID
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "UpdateUser",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.LastUpdateUser
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "closed",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.Close
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "TicketID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.ticketID
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "BusinessType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer._BT
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "Product",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer._Service
                };

                para[20] = new SqlParameter
                {
                    ParameterName = "OpportunityStageID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.OpportunityStageID
                };

                para[21] = new SqlParameter
                {
                    ParameterName = "CompanyName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.CompanyName
                };

                para[22] = new SqlParameter
                {
                    ParameterName = "IsSendMailToSalesPer",
                    SqlDbType = SqlDbType.Bit,
                    Value = false
                };

                para[23] = new SqlParameter
                {
                    ParameterName = "Department",
                    SqlDbType = SqlDbType.Int,
                    Value = intDept
                };

                para[24] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                #endregion
                //int oppid = 0;
                //oppid = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spAddOpportunity", para));
                //return oppid;

                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddOpportunity", para);
                int oppid = Convert.ToInt32(para[24].Value);
                return oppid;

                //int oppid = 0;
                //oppid = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spAddOpportunity", objPropCustomer.OpportunityID, objPropCustomer.Name, objPropCustomer.ROL, objPropCustomer.Probability, objPropCustomer.Status, objPropCustomer.Remarks, objPropCustomer.EndDate, objPropCustomer.Mode, objPropCustomer.ProspectID, objPropCustomer.NextStep, objPropCustomer.Description, objPropCustomer.Source, objPropCustomer.Amount, objPropCustomer.Fuser, objPropCustomer.AssignedToID, objPropCustomer.LastUpdateUser, objPropCustomer.Close, objPropCustomer.ticketID, objPropCustomer._BT, objPropCustomer._Service, objPropCustomer.OpportunityStageID, objPropCustomer.CompanyName));
                //return oppid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteOpportunity(Customer objPropCustomer)
        {
            try
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("DELETE FROM lead WHERE ID={0}; \n", objPropCustomer.OpportunityID);
                strBuilder.AppendFormat("UPDATE Estimate SET Opportunity = null WHERE Opportunity ={0};", objPropCustomer.OpportunityID);

                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.Text, strBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void DeleteOpportunity(DeleteOpportunityParam _DeleteOpportunity, string ConnectionString)
        {
            try
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendFormat("DELETE FROM lead WHERE ID={0}; \n", _DeleteOpportunity.OpportunityID);
                strBuilder.AppendFormat("UPDATE Estimate SET Opportunity = null WHERE Opportunity ={0};", _DeleteOpportunity.OpportunityID);

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, strBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTask(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.Text, "delete from todo where ID=" + objPropCustomer.TaskID + " delete from done where ID=" + objPropCustomer.TaskID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getContactByRolID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "Spgetcontactbyrol", objPropCustomer.ROL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetRolLocID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select l.Loc,l.Owner from loc as l inner join Owner as o on  o.id= l.Owner where l.Rol="+ objPropCustomer.ROL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetContactAllByRolID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetContactAllByRolID", objPropCustomer.ROL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesDashboard(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spSalesDashboard");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet getLocationRole(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from tbllocationrole where owner=" + objPropCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByRoleID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select loc from loc where roleID=" + objPropCustomer.RoleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddLocationRole(Customer objPropCustomer)
        {
            try
            {
                var para = new SqlParameter[5];

                para[0] = new SqlParameter
                {
                    ParameterName = "role",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.LocationRole
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Username",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Username
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "password",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Password
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "owner",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.CustomerID
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddLocationRole", para);

                return Convert.ToInt32(para[4].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocationRole(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spUpdateLocationRole", objPropCustomer.LocationRole, objPropCustomer.Username, objPropCustomer.Password, objPropCustomer.CustomerID, objPropCustomer.RoleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocationRole(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spDeleteLocationRole", objPropCustomer.RoleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateLabor(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select id, item, amount from tblestimatelabour");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateLaborForEstimate(Customer objPropCustomer)
        {
            try
            {
                //return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select itemid as id, item, amount from tblEstimateLabourItems where EstimateID = " + objPropCustomer.TemplateID);
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select ID, Estimate , Line , fDesc as scope, Quan as quantity,Cost,Price as amount,Amount as total ,vendor, isnull( currency ,'') as currency,isnull( measure,1) measure, code,fdesc, amount as budget from EstimateI where Estimate =" + objPropCustomer.TemplateID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEstimateTemplate(Customer objPropCustomer)
        {
            var para = new SqlParameter[7];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Description
            };
            para[2] = new SqlParameter
            {
                ParameterName = "remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };

            para[3] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[4] = new SqlParameter
            {
                ParameterName = "mode",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Mode
            };

            if (objPropCustomer.dtItems != null)
            {
                para[5] = new SqlParameter
                {
                    ParameterName = "items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "LaborItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtLaborItems
                };
            }

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddEstimateTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateTemplate(Customer objPropCustomer)
        {
            string strQuery = "Select ID,Name,fdesc,remarks from Estimate where EstTemplate=1";

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //saleperson
        public DataSet GetSalePersonByJob(Customer objPropCustomer)
        {
            try
            {
                string sql = "select distinct Terr.ID,Terr.SDesc from Estimate inner join Terr on Estimate.EstimateUserId = Terr.ID where Estimate.Job='" + objPropCustomer.ProjectJobID + "'";
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEstimates(Customer objPropCustomer, List<RetainFilter> filters, Int32 isSalesAsigned = 0, bool isEmailProposalsFilter = false)
        {
            bool isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);

            StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT Convert(varchar(50),e.ID) as ID, \n");
            varname1.Append("SELECT Distinct e.ID, \n");
            varname1.Append("       e.NAME, \n");
            varname1.Append("       e.fdesc, \n");
            varname1.Append("       e.fDate, \n");
            varname1.Append("       e.Opportunity, \n");
            varname1.Append("       ls.Description As OpportunityStage, \n");
            varname1.Append("       l.OpportunityStageID,  \n");
            varname1.Append("       e.Category, \n");
            varname1.Append("       e.EstimateAddress, \n");
            varname1.Append("       e.EstimateEmail, \n");
            varname1.Append("       e.remarks, \n");
            varname1.Append("       e.job, \n");
            varname1.Append("       e.CompanyName, \n");
            varname1.Append("       t.SDesc as AssignTo, \n");
            //varname1.Append("       ph.fDesc as Contact, \n");
            varname1.Append("       e.Contact, \n");
            varname1.Append("       e.SoldDate, \n"); // Added by Vishal Gupta - ES-9176
            varname1.Append("       ffor, \n");
            varname1.Append("       s.Name as [Status], \n");
            varname1.Append("       r.EN, \n");
            varname1.Append("       ISNULL(B.Name, '') As Company, \n");
            varname1.Append("       ISNULL(e.Price,0) As EstimatePrice,   \n");
            //varname1.Append("       ISNULL(e.Quoted,0) As QuotedPrice,     \n");
            varname1.Append("       CASE WHEN ISNULL(e.Quoted,0) = 0 THEN ISNULL(e.Price,0) ELSE e.Quoted END As QuotedPrice,     \n");
            varname1.Append("       CASE ISNULL(e.Discounted,0) WHEN 0 THEN 'No' ELSE 'Yes' END As Discounted,     \n");
            varname1.Append("       dep.Type Department,     \n");
            varname1.Append("       jt.fDesc Template,     \n");
            //varname1.Append("       (SELECT Top 1 1 FROM EstimateForm WHERE Estimate = e.ID) Attached \n");
            //varname1.Append("       (SELECT TOP 1 FileName FROM EstimateForm WHERE Estimate = e.ID ORDER BY AddedOn desc, ID desc) FileName, \n");
            //varname1.Append("       (SELECT TOP 1 FilePath FROM EstimateForm WHERE Estimate = e.ID ORDER BY AddedOn desc, ID desc) FilePath \n");
            varname1.Append("       ep.ID ProposalID, \n");
            varname1.Append("       ep.Attached, \n");
            varname1.Append("       ep.AddedOn, \n");
            varname1.Append("       ep.AddedBy, \n");
            varname1.Append("       ep.FileName, \n");
            varname1.Append("       ep.PdfFilePath, \n");
            varname1.Append("       ep.FilePath, \n");
            varname1.Append("       CASE WHEN ISNULL(ep.JobTID, 0) = 0 THEN 0 ELSE 1 END ManualUpload, \n");
            varname1.Append("       ep.Name ProposalName, \n");
            varname1.Append("       ep.SendTo, \n");
            varname1.Append("       ep.SendFrom, \n");
            varname1.Append("       ep.SendOn, \n");
            varname1.Append("       ep.SendBy, \n");
            varname1.Append("       EstimateFirstSent.FirstSentDate, \n"); // ES-9177 - Vishal Gupta           
            varname1.Append("       CASE WHEN s.Name = 'Sold' AND EstimateFirstSent.FirstSentDate IS NOT NULL AND e.SoldDate IS NOT NULL THEN DATEDIFF(DD,EstimateFirstSent.FirstSentDate,e.SoldDate) ELSE '' END As CycleDays, \n"); // ES-9302 - Vishal Gupta           
            varname1.Append("       Isnull(h1.NewStatus, 0) NewStatus, \n");
            varname1.Append("       CASE WHEN ep.SendTo is not null and ep.SendTo != '' THEN 'Yes' ELSE 'No' END Emailed, \n");
            varname1.Append("       CASE WHEN h1.NewStatus = 1 THEN 'Approved' \n");
            varname1.Append("       WHEN h1.NewStatus = 2 THEN 'Changes Required' \n");
            varname1.Append("       ELSE 'Pending' END ApprStatus, \n");
            varname1.Append("       h1.ApprBy, \n");
            varname1.Append("       h1.Comment \n");
            varname1.Append(" FROM  Estimate e  \n");
            varname1.Append("       LEFT OUTER JOIN OEStatus s ON e.[Status]= s.ID \n");
            varname1.Append("       LEFT OUTER JOIN terr t ON E.EstimateUserId=t.ID \n");
            varname1.Append("       LEFT OUTER JOIN Rol r on e.RolID = r.ID \n");
            varname1.Append("       LEFT OUTER JOIN Branch B on B.ID = r.EN  \n");
            varname1.Append("       LEFT OUTER JOIN Lead l on l.ID = e.Opportunity \n");
            varname1.Append("       LEFT JOIN  Stage ls on ls.ID = l.OpportunityStageID \n");
            //varname1.Append("       LEFT JOIN Phone ph on Convert(int,ISNULL(e.Contact,'0')) = ph.ID \n");
            varname1.Append("       left join tblCommonCustomFieldsValue cv on cv.Ref = e.ID and cv.Screen = 'Estimate' \n");
            varname1.Append("       left join tblCommonCustomFields cl on cl.ID = cv.tblCommonCustomFieldsID \n");
            varname1.Append("       left join JobT jt on jt.id = e.Template \n");
            varname1.Append("       left join JobType dep on dep.ID = jt.Type \n");

            varname1.Append("       LEFT OUTER JOIN (SELECT * FROM (SELECT ROW_NUMBER() OVER (PARTITION BY EstimateForm.Estimate ORDER BY EstimateForm.AddedOn DESC) as Attached \n");
            varname1.Append("           , Estimate, FileName, PdfFilePath, SendTo, SendFrom, SendOn, SendBy, AddedOn, AddedBy, Name, ID, FilePath, JobTID FROM EstimateForm) a  WHere a.Attached = 1) ep ON ep.Estimate = e.ID  \n");
            varname1.Append("       OUTER APPLY (SELECT TOP(1) ef.SendOn AS FirstSentDate FROM EstimateForm ef WHERE ef.Estimate = e.ID ORDER BY ef.SendOn ASC) EstimateFirstSent  \n"); // ES-9177 - Vishal Gupta
            varname1.Append("       LEFT OUTER JOIN (SELECT * FROM (SELECT ROW_NUMBER() OVER (PARTITION BY h.EstimateID ORDER BY h.ApprDate DESC) as RowNum \n");
            varname1.Append("           , h.EstimateID, h.ApprBy, h.ApprDate, h.NewStatus, h.Comment FROM tblEstimateApprovalStatusHistory h) a  WHere a.RowNum = 1) h1 ON h1.EstimateID = e.ID  \n");
            if (isEmailProposalsFilter)
            {
                varname1.Append("       LEFT JOIN (SELECT TOP 1 SalesApproveEstimate FROM Control) c on 1=1 \n");
            }
            varname1.Append("        \n");
            varname1.Append("        \n");

            if (objPropCustomer.EN == 1)
            {
                varname1.Append(" LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN  \n");
            }
            varname1.Append(" WHERE  EstTemplate = 0 ");

            if (objPropCustomer.Close == 1)
            {
                varname1.Append(" and ffor ='ACCOUNT' ");
                if (objPropCustomer.Center == "1")
                    varname1.Append(" and   job is null ");

            }
            if (objPropCustomer.EN == 1)
            {
                varname1.Append(" and UC.IsSel = 1 and UC.UserID =" + objPropCustomer.UserID);
            }

            //if (objPropCustomer.StartDate != string.Empty && objPropCustomer.EndDate != string.Empty)
            //{
            //    varname1.Append("   and   e.fDate >= '" + objPropCustomer.StartDate + "' and e.fDate <= '" + objPropCustomer.EndDate + "' \n");
            //}

            if (filters != null && filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0; string[] filterArrayValue;
                        var filterColumn = items.FilterColumn.ToLower();
                        StringBuilder filteredQuery = new StringBuilder();
                        if (filterColumn == "id")
                        {
                            filterArrayValue = items.FilterValue.ToString().Split(',');
                            // Reset start date, end date if filter by Id
                            objPropCustomer.StartDate = string.Empty;
                            objPropCustomer.EndDate = string.Empty;
                            foreach (var filtered in filterArrayValue)
                            {
                                if (int.TryParse(filtered, out FilterValue))
                                {
                                    if (filteredQuery.Length == 0)
                                    {
                                        filteredQuery.Append(filtered);
                                    }
                                    else
                                    {
                                        filteredQuery.Append("," + filtered);
                                    }
                                }


                            }
                            varname1.Append(" and e.ID  in (" + filteredQuery.ToString() + ")");
                        }
                        else if (filterColumn == "status")
                        {
                            //if(!string.IsNullOrEmpty(items.FilterValue) && items.FilterValue.ToLower == "op")
                            varname1.Append(" and s.Name like '%" + items.FilterValue + "%'");
                        }
                        else if (filterColumn == "ffor")
                        {
                            if (items.FilterValue.Trim().ToLower() == "existing")
                            {
                                varname1.Append(" and ffor = 'ACCOUNT'");
                            }
                            else if (items.FilterValue.Trim().ToLower() == "lead")
                            {
                                varname1.Append(" and ffor <> 'ACCOUNT'");
                            }
                            else
                            {
                                varname1.Append(" and ffor = '" + items.FilterValue + "'");
                            }

                        }
                        else if (filterColumn == "template")// || (objPropCustomer.SearchBy).ToLower() == "l.opportunitystageid")
                        {
                            varname1.Append(" and jt.fDesc like '%" + items.FilterValue + "%'");
                        }
                        else if (filterColumn == "department")// || (objPropCustomer.SearchBy).ToLower() == "l.opportunitystageid")
                        {
                            varname1.Append(" and dep.type like '%" + items.FilterValue + "%'");
                        }
                        else if (filterColumn == "opportunitystage")
                        {
                            varname1.Append(" and ls.Description LIKE '%" + items.FilterValue + "%'");
                        }
                        else if (filterColumn == "companyname" ||
                            filterColumn == "estimateaddress" ||
                            filterColumn == "opportunity" ||
                            filterColumn == "job" ||
                            filterColumn == "category" ||
                            filterColumn == "fdesc" ||
                            filterColumn == "contact"
                            )
                        {
                            varname1.Append("   and e." + filterColumn + " LIKE '%" + items.FilterValue + "%' \n");
                        }
                        else if (filterColumn == "assignto")
                        {
                            varname1.Append("   and ( t.SDesc LIKE '%" + items.FilterValue + "%' ) \n");
                        }
                        //else if (filterColumn == "contact")
                        //{
                        //    varname1.Append("   AND ph.fDesc LIKE '%" + items.FilterValue + "%' \n");
                        //}
                        else if (filterColumn == "quotedprice")
                        {
                            double quoteValue;
                            if (double.TryParse(items.FilterValue, out quoteValue))
                            {
                                varname1.Append("   AND e.Quoted = " + quoteValue + " \n");
                            }
                        }
                        else if (filterColumn == "estimateprice")
                        {
                            double priceValue;
                            if (double.TryParse(items.FilterValue, out priceValue))
                            {
                                varname1.Append("   AND e.Price = " + priceValue + " \n");
                            }
                        }
                        else if (filterColumn == "discounted")
                        {
                            if (items.FilterValue.Trim().ToLower() == "yes")
                            {
                                varname1.Append("   AND e.Discounted = 1 \n");
                            }
                            else if (items.FilterValue.Trim().ToLower() == "no")
                            {
                                varname1.Append("   AND isnull(e.Discounted,0) = 0 \n");
                            }
                            else
                            {
                                varname1.Append("   AND 1=0 \n");
                            }
                        }
                        else if (filterColumn == "ddlapprstatus")
                        {
                            varname1.Append("   AND (CASE WHEN h1.NewStatus = 1 THEN 'Approved' \n");
                            varname1.Append("       WHEN h1.NewStatus = 2 THEN 'Changes Required' \n");
                            varname1.AppendFormat("       ELSE 'Pending' END) like '%{0}%' \n", items.FilterValue.Trim());
                        }
                        else if (filterColumn == "emailed")
                        {
                            varname1.AppendFormat(" AND (CASE WHEN ep.SendTo is not null and ep.SendTo != '' THEN 'Yes' ELSE 'No' END) like '%{0}%' \n", items.FilterValue.Trim());
                        }
                        else if (filterColumn == "apprby")
                        {
                            varname1.AppendFormat("   AND h1.ApprBy like '%{0}%'\n", items.FilterValue.Trim());
                        }
                        else if (filterColumn == "apprcomment")
                        {
                            varname1.AppendFormat("   AND h1.Comment like '%{0}%'\n", items.FilterValue.Trim());
                        }
                    }
                }
            }

            if (objPropCustomer.SearchBy != string.Empty && objPropCustomer.SearchValue != string.Empty)
            {
                if ((objPropCustomer.SearchBy).ToLower() == "e.id")
                {
                    int FilterValue = 0; string[] filterArrayValue;
                    filterArrayValue = objPropCustomer.SearchValue.Split(',');
                    StringBuilder filteredQuery = new StringBuilder();
                    // Reset start date, end date if filter by Id
                    objPropCustomer.StartDate = string.Empty;
                    objPropCustomer.EndDate = string.Empty;
                    foreach (var filtered in filterArrayValue)
                    {
                        if (int.TryParse(filtered, out FilterValue))
                        {
                            if (filteredQuery.Length == 0)
                            {
                                filteredQuery.Append(filtered);
                            }
                            else
                            {
                                filteredQuery.Append("," + filtered);
                            }
                        }
                    }
                    varname1.Append(" and e.ID  in (" + filteredQuery.ToString() + ")");
                }

                if ((objPropCustomer.SearchBy).ToLower() == "e.status"
                    || (objPropCustomer.SearchBy).ToLower() == "e.ffor"
                    || (objPropCustomer.SearchBy).ToLower() == "e.template"
                    || (objPropCustomer.SearchBy).ToLower() == "l.opportunitystageid"
                    || (objPropCustomer.SearchBy).ToLower() == "dep.id"
                    || (objPropCustomer.SearchBy).ToLower() == "e.iscertifiedproject"
                    )
                {
                    varname1.Append("   and " + objPropCustomer.SearchBy + " = '" + objPropCustomer.SearchValue + "'       \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.companyname")
                {
                    varname1.Append("   and " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.estimateaddress")
                {
                    varname1.Append("   and " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.opportunity")
                {
                    varname1.Append("   and " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }

                else if ((objPropCustomer.SearchBy).ToLower() == "e.job")
                {
                    varname1.Append("   and " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }

                else if ((objPropCustomer.SearchBy).ToLower() == "e.category")
                {
                    varname1.Append("   and " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }

                else if ((objPropCustomer.SearchBy).ToLower() == "em.ffirst")
                {
                    varname1.Append("   and ( t.SDesc LIKE '%" + objPropCustomer.SearchValue + "%' ) \n");
                }

                else if ((objPropCustomer.SearchBy).ToLower() == "e.contact")
                {
                    varname1.Append("   AND e.contact LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.iscertifiedproject")
                {
                    varname1.Append("   AND e.contact LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "customfield")
                {
                    varname1.Append("   AND ISNULL(cl.IsDeleted, 0) = 0 AND cl.ID = '" + objPropCustomer.SearchValue + "' AND cv.Value LIKE '%" + objPropCustomer.SearchValueExt + "%'\n");
                }
            }

            //if (objPropCustomer.SearchBy != string.Empty && (objPropCustomer.SearchBy).ToLower() == "e.bdate")
            //{
            //    if (string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && !string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            //    {
            //        varname1.Append(" AND e.bdate <= '" + objPropCustomer.SearchValueToDt + "' \n");
            //    }
            //    else if (!string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            //    {
            //        varname1.Append(" AND e.bdate >= '" + objPropCustomer.SearchValueFrDt + "' \n");
            //    }
            //    else if (!string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && !string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            //    {
            //        varname1.Append(" AND e.bdate >= '" + objPropCustomer.SearchValueFrDt + "' and e.bdate <= '" + objPropCustomer.SearchValueToDt + "' \n");
            //    }
            //}

            if (objPropCustomer.Range == 1)
            {
                //if (objPropCustomer.StartDate != string.Empty && objPropCustomer.EndDate != string.Empty)
                //if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                //{
                //    varname1.Append("   and   e.fDate >= '" + objPropCustomer.StartDate + "' and e.fDate <= '" + objPropCustomer.EndDate + "' \n");
                //}

                if (string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    varname1.Append(" AND e.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    varname1.Append(" AND e.fDate >= '" + objPropCustomer.StartDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    varname1.Append(" AND e.fDate >= '" + objPropCustomer.StartDate + "' and e.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }
            }
            else if (objPropCustomer.Range == 2)
            {
                if (string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    varname1.Append(" AND e.bdate <= '" + objPropCustomer.EndDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    varname1.Append(" AND e.bdate >= '" + objPropCustomer.StartDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    varname1.Append(" AND e.bdate >= '" + objPropCustomer.StartDate + "' and e.bdate <= '" + objPropCustomer.EndDate + "' \n");
                }
            }

            if (isSalesAsigned > 0)// If User is salesperson and IsSalesAsigned is True
            {
                varname1.Append(@"    AND e.RolID = ( CASE e.LocID    WHEN 0 THEN (SELECT TOP 1 Rol  FROM   Prospect     WHERE  Prospect.Terr = (SELECT Isnull(id, 0)      FROM   Terr    WHERE  NAME = (SELECT fUser   FROM   tblUser  WHERE  id = " + isSalesAsigned + "))     AND Prospect.Rol = e.RolID)      ELSE (SELECT TOP 1 Rol   FROM   loc    WHERE  loc.Loc = e.LocID   AND ( Isnull(loc.Terr, 0) = (SELECT Isnull(id, 0)            FROM   Terr          WHERE  NAME = (SELECT fUser      FROM   tblUser     WHERE  id = " + isSalesAsigned + "))    OR Isnull(loc.Terr2, 0) = (SELECT Isnull(id, 0)     FROM   Terr  WHERE  NAME = (SELECT fUser             FROM   tblUser                  WHERE  id = " + isSalesAsigned + ")) ))              END)    \n");
            }

            if (isEmailProposalsFilter)
            {
                varname1.Append(@"    AND ep.Attached = 1 AND (IsNull(c.SalesApproveEstimate, 0) = 0 OR (c.SalesApproveEstimate = 1 AND h1.NewStatus is not null AND h1.NewStatus = 1)) ");
            }


            varname1.Append(" ORDER BY e.fDate desc, e.ID desc");

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWeeklySaleReportQuoted(Customer objPropCustomer)
        {
            var para = new SqlParameter[2];

            para[0] = new SqlParameter
            {
                ParameterName = "StartDate",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.StartDate
            };

            para[1] = new SqlParameter
            {
                ParameterName = "EndDate",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.EndDate
            };

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetWeeklySaleReportForQuotedJobs", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateTemplateByID(Customer objPropCustomer)
        {
            string strQuery = "select ID, Name ,fDesc,Remarks, rolid, locid,( case ffor when 'ACCOUNT' then (select tag from loc where loc = locid) when 'PROSPECT' then (select name from rol where id=rolid) end )as contact, isnull(cadexchange,0) as cadexchange, status, job,Template,EstimateBillAddress,BDate,Phone,Fax,EstimateUserId,EstimateAddress,EstimateEmail,EstimateCell from Estimate where ID=" + objPropCustomer.TemplateID;
            strQuery += " select ID, Estimate , Line , fDesc as scope, Quan as quantity,Cost,Price as amount,Amount as total , vendor, isnull( currency ,'') as currency,isnull( measure,1) measure, code,fdesc, amount as budget from EstimateI where Estimate=" + objPropCustomer.TemplateID;
            strQuery += " select ID, Line, TemplateID, LabourID, Amount from tblJoinLaborTemplate where TemplateID = " + objPropCustomer.TemplateID;
            strQuery += "SELECT EstimateI.ID as EstimateItemId,j.JobT,j.Job,m.JobTItemID as JobTItem,EstimateI.Type as jtype," +
               "EstimateI.fDesc, EstimateI.Code as jcode, EstimateI.Line, m.MilestoneName as MilesName, m.RequiredBy as RequiredBy,j.ETCMod as LeadTime," +
               "ProjAcquistDate,ActAcquistDate, Comments, isnull(m.Type,0) as Type, isnull(o.Department,'') AS Department, isnull(m.Amount, 0) as Amount " +
   "from EstimateI inner join Milestone m on EstimateI.ID=m.EstimateIId left outer join JobTItem j on m.JobTItemID=j.ID " +
   "left outer join Job on j.Job=Job.ID LEFT JOIN OrgDep o ON o.ID = m.Type WHERE (j.job=0 or j.job is null) and EstimateI.Estimate=" + objPropCustomer.TemplateID;



            strQuery += "select  j.id, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count, j.Type  from JobT j where j.ID = " + objPropCustomer.TemplateID + "order by j.ID ";
            strQuery += "select EstimateI.ID as EstimateItemId,EstimateI.Code  as jcode,EstimateI.fDesc,EstimateI.Type as jtype,b.Type as Btype,isnull(b.Item,0) as BItem,b.QtyRequired as QtyReq," + "b.UM as UM, b.ScrapFactor as ScrapFact, b.BudgetUnit as BudgetUnit,b.BudgetExt,EstimateI.Line,b.Vendor,b.TotalPrice," +
                         "LTRIM(RTRIM(b.Currency)) as currency,b.AmountDollars as Amount,b.Percentage as jPercent from EstimateI " + "inner join BOM b on EstimateI.ID=b.EstimateIId where EstimateI.Estimate=" + objPropCustomer.TemplateID;
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateBucket(Customer objPropCustomer)
        {
            string strQuery = "select ID, Name , [Desc] from tblEstimateBucket";

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateBucketItems(Customer objPropCustomer)
        {
            string strQuery = "select ID, line, item, vendor, unit, cost,isnull( measure,1) measure,code from tblEstimateBucketItems where bucketid=" + objPropCustomer.BucketID;

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEstimateBucket(Customer objPropCustomer)
        {
            var para = new SqlParameter[5];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "desc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Description
            };
            para[2] = new SqlParameter
            {
                ParameterName = "bucketID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.BucketID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "mode",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Mode
            };

            if (objPropCustomer.dtItems != null)
            {
                para[4] = new SqlParameter
                {
                    ParameterName = "items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
            }

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddEstimateBucket", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateBucketByID(Customer objPropCustomer)
        {
            string strQuery = "select ID, Name , [Desc] from tblEstimateBucket where ID=" + objPropCustomer.BucketID;
            strQuery += " select ID, Line , item as scope, vendor, code, cost, unit,isnull( measure,1) measure  from tblEstimateBucketItems where BucketID=" + objPropCustomer.BucketID;

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEstimateLabor(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "AddLaborItem", objPropCustomer.Name, objPropCustomer.Amount, objPropCustomer.BucketID, objPropCustomer.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public int AddEstimate(Customer objPropCustomer)
        //{
        //    var para = new SqlParameter[19];

        //    para[0] = new SqlParameter
        //    {
        //        ParameterName = "Name",
        //        SqlDbType = SqlDbType.VarChar,
        //        Value = objPropCustomer.Name
        //    };
        //    para[1] = new SqlParameter
        //    {
        //        ParameterName = "fdesc",
        //        SqlDbType = SqlDbType.VarChar,
        //        Value = objPropCustomer.Description
        //    };
        //    para[2] = new SqlParameter
        //    {
        //        ParameterName = "remarks",
        //        SqlDbType = SqlDbType.VarChar,
        //        Value = objPropCustomer.Remarks
        //    };

        //    para[3] = new SqlParameter
        //    {
        //        ParameterName = "template",
        //        SqlDbType = SqlDbType.Int,
        //        Value = objPropCustomer.TemplateID
        //    };
        //    para[4] = new SqlParameter
        //    {
        //        ParameterName = "mode",
        //        SqlDbType = SqlDbType.Int,
        //        Value = objPropCustomer.Mode
        //    };
        //    para[5] = new SqlParameter
        //    {
        //        ParameterName = "loc",
        //        SqlDbType = SqlDbType.Int,
        //        Value = objPropCustomer.LocID
        //    };

        //    para[6] = new SqlParameter
        //    {
        //        ParameterName = "rol",
        //        SqlDbType = SqlDbType.Int,
        //        Value = objPropCustomer.ROL
        //    };

        //    para[7] = new SqlParameter
        //    {
        //        ParameterName = "CADExchange",
        //        SqlDbType = SqlDbType.Money,
        //        Value = objPropCustomer.CADExchange
        //    };

        //    para[8] = new SqlParameter
        //    {
        //        ParameterName = "Edited",
        //        SqlDbType = SqlDbType.SmallInt,
        //        Value = objPropCustomer.IsItemEdited
        //    };

        //    para[12] = new SqlParameter
        //    {
        //        ParameterName = "Status",
        //        SqlDbType = SqlDbType.SmallInt,
        //        Value = objPropCustomer.Status
        //    };


        //    if (objPropCustomer.dtItems != null)
        //    {
        //        para[9] = new SqlParameter
        //        {
        //            ParameterName = "items",
        //            SqlDbType = SqlDbType.Structured,
        //            Value = objPropCustomer.dtItems
        //        };
        //        para[10] = new SqlParameter
        //        {
        //            ParameterName = "LaborItems",
        //            SqlDbType = SqlDbType.Structured,
        //            Value = objPropCustomer.dtLaborItems
        //        };
        //        para[11] = new SqlParameter
        //        {
        //            ParameterName = "LaborColumnItems",
        //            SqlDbType = SqlDbType.Structured,
        //            Value = objPropCustomer.dtLaborItemsEstimate
        //        };

        //    }
        //    if (objPropCustomer.DtMilestone != null)
        //    {
        //        para[13] = new SqlParameter
        //        {
        //            ParameterName = "MilestonItem",
        //            SqlDbType = SqlDbType.Structured,
        //            Value = objPropCustomer.DtMilestone
        //        };
        //    }
        //    if (objPropCustomer._dtBomEstimate != null)
        //    {
        //        para[14] = new SqlParameter
        //        {
        //            ParameterName = "BomItem",
        //            SqlDbType = SqlDbType.Structured,
        //            Value = objPropCustomer._dtBomEstimate
        //        };
        //    }
        //    para[15] = new SqlParameter
        //    {
        //        ParameterName = "Contact",
        //        SqlDbType = SqlDbType.VarChar,
        //        Value = objPropCustomer.Contact
        //    };
        //    para[16] = new SqlParameter
        //    {
        //        ParameterName = "estDate",
        //        SqlDbType = SqlDbType.DateTime,
        //        Value = objPropCustomer.date
        //    };
        //    para[17] = new SqlParameter
        //    {
        //        ParameterName = "EstimateNo",
        //        SqlDbType = SqlDbType.SmallInt,
        //        Value = objPropCustomer.estimateno
        //    };
        //    para[18] = new SqlParameter
        //    {
        //        ParameterName = "Jobtype",
        //        SqlDbType = SqlDbType.VarChar,
        //        Value = objPropCustomer.type
        //    };

        //    try
        //    {
        //        return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddEstimate", para));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public int AddProject(Customer objPropCustomer, string groupIds)
        {
            var para = new SqlParameter[80];

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
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProject", para);
                return Convert.ToInt32(para[42].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateOrderNoProject(Customer objPropCustomer)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "jobTItemId",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.JobTItemId
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "OrderNo",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.OrderNo
                };
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateOrderNoProject", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddProjectTemplate(JobT _objJob)
        {
            var para = new SqlParameter[37];

            para[0] = new SqlParameter
            {
                ParameterName = "jobT",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.fDesc
            };
            para[2] = new SqlParameter
            {
                ParameterName = "Type",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Type
            };
            para[3] = new SqlParameter
            {
                ParameterName = "NRev",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NRev
            };
            para[4] = new SqlParameter
            {
                ParameterName = "NDed",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NDed
            };
            para[5] = new SqlParameter
            {
                ParameterName = "Count",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Count
            };
            para[6] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.Remarks
            };
            para[7] = new SqlParameter
            {
                ParameterName = "InvExp",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvExp
            };
            para[8] = new SqlParameter
            {
                ParameterName = "InvServ",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvServ
            };
            para[9] = new SqlParameter
            {
                ParameterName = "Wage",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Wage
            };
            para[10] = new SqlParameter
            {
                ParameterName = "CType",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.CType
            };
            para[11] = new SqlParameter
            {
                ParameterName = "Status",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Status
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Charge",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Charge
            };
            para[13] = new SqlParameter
            {
                ParameterName = "Post",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Post
            };
            para[14] = new SqlParameter
            {
                ParameterName = "fInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.fInt
            };
            para[15] = new SqlParameter
            {
                ParameterName = "GLInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.GLInt
            };
            para[16] = new SqlParameter
            {
                ParameterName = "JobClose",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.JobClose
            };
            para[17] = new SqlParameter
            {
                ParameterName = "tempRev",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.TemplateRev
            };
            para[18] = new SqlParameter
            {
                ParameterName = "RevRemarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.RevRemarks
            };
            para[19] = new SqlParameter
            {
                ParameterName = "alertType",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.AlertType
            };
            para[20] = new SqlParameter
            {
                ParameterName = "alertMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.AlertMgr
            };
            para[21] = new SqlParameter
            {
                ParameterName = "MilestoneMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.MilestoneMgr
            };
            if (_objJob.ProjectDt.Rows.Count > 0)
            {
                para[22] = new SqlParameter
                {
                    ParameterName = "BomItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objJob.ProjectDt
                };
            }
            if (_objJob.MilestoneDt != null)
            {
                if (_objJob.MilestoneDt.Rows.Count > 0)
                {
                    para[23] = new SqlParameter
                    {
                        ParameterName = "MilestonItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.MilestoneDt
                    };
                }
            }
            if (_objJob.CustomTabItem != null)
            {
                if (_objJob.CustomTabItem.Rows.Count > 0)
                {
                    para[24] = new SqlParameter
                    {
                        ParameterName = "CustomTabItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomTabItem
                    };
                }
            }
            if (_objJob.CustomItem != null)
            {
                if (_objJob.CustomItem.Rows.Count > 0)
                {
                    para[25] = new SqlParameter
                    {
                        ParameterName = "CustomItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomItem
                    };
                }
            }
            para[26] = new SqlParameter
            {
                ParameterName = "UnrecognizedRevenue",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.UnrecognizedRevenue
            };
            para[27] = new SqlParameter
            {
                ParameterName = "UnrecognizedExpense",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.UnrecognizedExpense
            };
            para[28] = new SqlParameter
            {
                ParameterName = "RetainageReceivable",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.RetainageReceivable
            };
            para[29] = new SqlParameter
            {
                ParameterName = "TargetHPermission",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.TargetHPermission
            };

            para[30] = new SqlParameter
            {
                ParameterName = "OHPer",
                SqlDbType = SqlDbType.Float,
                Value = _objJob.OHPer
            };
            para[31] = new SqlParameter
            {
                ParameterName = "COMMSPer",
                SqlDbType = SqlDbType.Float,
                Value = _objJob.COMMSPer
            };
            para[32] = new SqlParameter
            {
                ParameterName = "MARKUPPer",
                SqlDbType = SqlDbType.Float,
                Value = _objJob.MARKUPPer
            };
            para[33] = new SqlParameter
            {
                ParameterName = "STaxName",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.STaxName
            };
            para[34] = new SqlParameter
            {
                ParameterName = "EstimateType",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.EstimateType
            };
            para[35] = new SqlParameter
            {
                ParameterName = "IsSglBilAmt",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.IsSglBilAmt
            };
            para[36] = new SqlParameter
            {
                ParameterName = "IsBilFrmBOM",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.IsBilFrmBOM
            };
            //}
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.StoredProcedure, "spAddProjectTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBomt(Customer objPropCustomer)
        {
            string strQuery = "SELECT ID,Type FROM BOMT";

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public String ConvertEstimateToProject(Customer objPropCustomer)
        {
            try
            {
                var val = SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spConvertEstimateToProject", objPropCustomer.estimateno, objPropCustomer.Username);
                return Convert.ToString(val);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEstimateToProject(String ConnConfig, Int32 JobID, Decimal BRev, Decimal BLabour, Decimal BMat, Decimal BOther, Decimal BCost, Decimal BProfit, Decimal BRatio, Decimal BHour)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnConfig, CommandType.Text, "UPDATE Job SET BRev = isnull(BRev,0) + " + BRev + ", BLabor=isnull(BLabor,0) + " + BLabour + ", BMat=isnull(BMat,0) + " + BMat + ", BOther=isnull(BOther,0) + " + BOther + ", BCost=isnull(BCost,0) + " + BCost + ", BProfit=isnull(BProfit,0) + " + BProfit + ", BRatio=isnull(BRatio,0) + " + BRatio + ", BHour=isnull(BHour,0) + " + BHour + " WHERE ID = " + JobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJobEstimate(Customer objPropCustomer)
        {
            string strQuery = string.Empty;
            if (objPropCustomer.Mode == 1)
            {
                strQuery = "select j.*,j.GLRev as acct from Job j  where  Loc =" + objPropCustomer.LocID + " order by Fdate desc";//inner join Estimate e on e.Job = j.ID

            }
            else
            {
                strQuery = "select j.*,j.GLRev as acct from Job j  where j.status=0 and Loc =" + objPropCustomer.LocID + " order by Fdate desc";//inner join Estimate e on e.Job = j.ID

            }

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJobProject(Customer objPropCustomer, DataTable filtersData, Int32 IsSelesAsigned = 0, int IncludeClose = 1, string Size = "0", string Page = "0", string OrderBY = "0")
        {
            try
            {

                SqlParameter[] para = new SqlParameter[15];
                para[0] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.SearchBy
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.SearchValue
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.StartDate
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.EndDate
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "Range",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPropCustomer.Range
                };


                para[5] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPropCustomer.JobType
                };

                para[6] = new SqlParameter
                {
                    ParameterName = "IsSalesAsigned",
                    SqlDbType = SqlDbType.Int,
                    Value = IsSelesAsigned
                };

                para[7] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.EN
                };

                para[8] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.UserID
                };

                para[9] = new SqlParameter
                {
                    ParameterName = "IncludeClose",
                    SqlDbType = SqlDbType.Int,
                    Value = IncludeClose
                };


                para[10] = new SqlParameter
                {
                    ParameterName = "FiltersData",
                    SqlDbType = SqlDbType.Structured,
                    Value = filtersData
                };

                para[11] = new SqlParameter
                {
                    ParameterName = "SearchTeamMemberValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Username
                };

                para[12] = new SqlParameter
                {
                    ParameterName = "@Size",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Size.ToString()
                };

                para[13] = new SqlParameter
                {
                    ParameterName = "@Page",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Page.ToString()
                };

                para[14] = new SqlParameter
                {
                    ParameterName = "OrderBY",
                    SqlDbType = SqlDbType.VarChar,
                    Value = OrderBY.ToString()
                };
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spGetJobProject", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Project VS Budget Report DL By Prateek 06/18/2021
        public DataSet getJobProjectReport(Customer objPropCustomer, DataTable filtersData, Int32 IsSelesAsigned = 0, int IncludeClose = 1, string Size = "0", string Page = "0", string OrderBY = "0")
        {
            try
            {

                SqlParameter[] para = new SqlParameter[15];
                para[0] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.SearchBy
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.SearchValue
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.StartDate
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.EndDate
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "Range",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPropCustomer.Range
                };


                para[5] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPropCustomer.JobType
                };

                para[6] = new SqlParameter
                {
                    ParameterName = "IsSalesAsigned",
                    SqlDbType = SqlDbType.Int,
                    Value = IsSelesAsigned
                };

                para[7] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.EN
                };

                para[8] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.UserID
                };

                para[9] = new SqlParameter
                {
                    ParameterName = "IncludeClose",
                    SqlDbType = SqlDbType.Int,
                    Value = IncludeClose
                };


                para[10] = new SqlParameter
                {
                    ParameterName = "FiltersData",
                    SqlDbType = SqlDbType.Structured,
                    Value = filtersData
                };

                para[11] = new SqlParameter
                {
                    ParameterName = "SearchTeamMemberValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Username
                };

                para[12] = new SqlParameter
                {
                    ParameterName = "@Size",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Size.ToString()
                };

                para[13] = new SqlParameter
                {
                    ParameterName = "@Page",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Page.ToString()
                };

                para[14] = new SqlParameter
                {
                    ParameterName = "OrderBY",
                    SqlDbType = SqlDbType.VarChar,
                    Value = OrderBY.ToString()
                };
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spGetJobProjectReport", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobProjectWIP(Customer objPropCustomer, int includeClose, bool isPeriodPost = false)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.EndDate
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPropCustomer.JobType
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.EN
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.UserID
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "IncludeClose",
                    SqlDbType = SqlDbType.Int,
                    Value = includeClose
                };

                if (isPeriodPost)
                {
                    return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spGetProjectWIPPost", para);
                }

                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spGetProjectWIP", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobProjectWIPSummary(Customer objPropCustomer, int includeClose)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.EndDate
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objPropCustomer.JobType
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.EN
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.UserID
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "IncludeClose",
                    SqlDbType = SqlDbType.Int,
                    Value = includeClose
                };

                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spGetProjectWIPSummary", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getJobProject(GetJobProjectParam _GetJobProject, string ConnectionString, Int32 IsSalesAsigned = 0, int IncludeClose = 1)
        {

            try
            {

                SqlParameter[] para = new SqlParameter[12];
                para[0] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetJobProject.SearchBy
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetJobProject.SearchValue
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetJobProject.StartDate
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetJobProject.EndDate
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "Range",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _GetJobProject.Range
                };


                para[5] = new SqlParameter
                {
                    ParameterName = "Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = _GetJobProject.JobType
                };

                para[6] = new SqlParameter
                {
                    ParameterName = "IsSalesAsigned",
                    SqlDbType = SqlDbType.Int,
                    Value = IsSalesAsigned
                };

                para[7] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetJobProject.EN
                };

                para[8] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetJobProject.UserID
                };

                para[9] = new SqlParameter
                {
                    ParameterName = "IncludeClose",
                    SqlDbType = SqlDbType.Int,
                    Value = IncludeClose
                };


                para[10] = new SqlParameter
                {
                    ParameterName = "FiltersData",
                    SqlDbType = SqlDbType.Structured,
                    Value = _GetJobProject.filtersData
                };

                para[11] = new SqlParameter
                {
                    ParameterName = "SearchTeamMemberValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetJobProject.Username
                };

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetJobProject", para);

                //return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetJobProject", objPropCustomer.SearchBy, 
                //    objPropCustomer.SearchValue, objPropCustomer.StartDate, objPropCustomer.EndDate, objPropCustomer.Range, 
                //    objPropCustomer.JobType, IsSelesAsigned, objPropCustomer.EN, objPropCustomer.UserID, IncludeClose, filtersData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddCustomBomT(Customer objPropCustomer)
        {
            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@Label",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.label
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@TabID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Tab
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Percentage",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Percentage
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@Amount",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.PerAmount
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddCustomEstimate", para);
                //SqlHelper.ExecuteNonQuery(_objJob.ConnConfig, "spDeleteProjectTemplate", _objJob.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProjectTemplate(Customer objPropCustomer)
        {
            string strQuery = "";
            if (objPropCustomer.Status == 1)
            {
                strQuery = "select  j.id,jt.Type as Dept, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count, j.Type from JobT j left outer join  JobType jt on j.Type=jt.ID order by j.ID ";
            }
            else
            {
                strQuery = "select  j.id,jt.Type as Dept, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count, j.Type from JobT j left outer join  JobType jt on j.Type=jt.ID where j.status=0 order by j.ID ";
            }
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProjectTemp(Customer objPropCustomer, int Job = 0)
        {

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectTemplate", Job);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getWage(Customer objPropCustomer)
        {
            string strQuery = "select id,fdesc,remarks from PRWage where Field = 1 ";

            if (objPropCustomer.Fuser != string.Empty)
            {
                strQuery += @"AND id IN (SELECT wage   FROM   PRWageItem   WHERE isnull(Status,0)=0 and  emp = (SELECT TOP 1 ID    FROM   emp
                                WHERE  CallSign = '" + objPropCustomer.Fuser + "' )  UNION  SELECT Isnull(WageC, 0)    FROM   TicketD         WHERE  id = " + objPropCustomer.ticketID + ")";

            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProjectByJobID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectByJobID", objPropCustomer.ProjectJobID, objPropCustomer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJobRatebyID(Customer objPropCustomer)
        {
            string strQuery = "select j.BillRate BT, j.RateOT  OT, j.RateNT NT , j.RateDT DT, j.RateTravel TT  , j.RateMileage RM  from Job j where j.ID=" + objPropCustomer.job;

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet GetjobcodeInfo(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetjobcodeInfo", objPropCustomer.ProjectJobID, objPropCustomer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProject_BOM(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProject_BOM", objPropCustomer.ProjectJobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProject_Team(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProject_Team", objPropCustomer.ProjectJobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProject_Milestone(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProject_Milestone", objPropCustomer.ProjectJobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomReport(Customer objPropCustomer)
        {
            string strQuery = "SELECT  [ID]      ,[ReportName]      ,[ReportDesc]      ,[ReportType]  FROM [tblCustomReport]";

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobTemplateByID(Customer objPropCustomer)
        {
            //string strQuery = "declare @GLIntName varchar(75) select @GLIntName=c.fdesc from JobT j left join Chart c on j.GLInt = c.ID where j.ID=" + objPropCustomer.ProjectJobID;
            //strQuery += " select j.*,p.fDesc as WageName, i.Name as InvServiceName, c.fDesc as InvExpName, @GLIntName as GLName from JobT j left join PRWage p on j.Wage = p.ID left join Inv i on j.InvServ = i.ID left join Chart c on j.InvExp = c.ID where j.id=" + objPropCustomer.ProjectJobID;
            //strQuery += " select * from jobtitem j where j.job is null and j.jobT=" + objPropCustomer.ProjectJobID;

            try
            {
                //return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectTemplateByID", objPropCustomer.ProjectJobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getAllCustomers(Loc objLoc)
        {
            try
            {
                return objLoc.DsLoc = SqlHelper.ExecuteDataset(objLoc.ConnConfig, CommandType.Text, "SELECT Loc,ID,Tag FROM Loc Order by Tag ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getAllLocationOnCustomer(Loc objLoc, int _ownerId)
        {
            try
            {
                return objLoc.DsLoc = SqlHelper.ExecuteDataset(objLoc.ConnConfig, CommandType.Text, "SELECT Loc,ID,Tag FROM Loc where Owner =" + _ownerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getAllLocationOnCustomer(GetAllLocationOnCustomerParam _GetAllLocationOnCustomer, int _ownerId, string ConnectionString)
        {
            try
            {
                return _GetAllLocationOnCustomer.DsLoc = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Loc,ID,Tag FROM Loc where Owner =" + _ownerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteProject(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spDeleteProject", objPropCustomer.ProjectJobID, objPropCustomer.Username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerBalance(Owner _objOwner)
        {
            try
            {
                string query = "UPDATE Owner SET Balance = @Balance WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objOwner.ID));
                parameters.Add(new SqlParameter("@Balance", _objOwner.Balance));
                SqlHelper.ExecuteNonQuery(_objOwner.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOwnerByID(Owner _objOwner)
        {
            try
            {
                return _objOwner.Ds = SqlHelper.ExecuteDataset(_objOwner.ConnConfig, CommandType.Text, "SELECT ID,Status,Locs,Elevs,Balance,Type,Billing,Central,Rol,Internet,TicketO,TicketD,Ledger,Request,Password,fLogin,Statement,Custom1,Custom2,NeedsFullSync,MerchantServicesId,idCreditCardDefault,QBCustomerID,msmuser,msmpass,SageID,CPEquipment,OwnerID FROM Owner WHERE ID=" + _objOwner.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOwnerByLoc(Owner _objOwner)
        {
            try
            {
                return _objOwner.Ds = SqlHelper.ExecuteDataset(_objOwner.ConnConfig, CommandType.Text, "SELECT l.Loc,o.ID,o.Status,o.Locs,o.Elevs,o.Balance,o.Type,o.Billing,o.Central,o.Rol,o.Internet,o.TicketO,o.TicketD,o.Ledger,o.Request,o.Password,o.fLogin,o.Statement,o.Custom1,o.Custom2,o.NeedsFullSync,o.MerchantServicesId,o.idCreditCardDefault,o.QBCustomerID,o.msmuser,o.msmpass,o.SageID,o.CPEquipment,o.OwnerID FROM Owner o, Loc l WHERE l.Owner=o.ID AND l.Loc=" + _objOwner.Locs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteProjectTemplate(JobT _objJob)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objJob.ConnConfig, "spDeleteProjectTemplate", _objJob.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetCustomerBalanceByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.Text, "SELECT isnull(Balance,0) as Balance FROM Owner where ID=" + objPropCustomer.CustomerID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetLocBalanceByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.Text, "SELECT isnull(Balance,0) as Balance FROM Loc where Loc=" + objPropCustomer.CustomerID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateProjectTemplate(JobT _objJob)
        {
            var para = new SqlParameter[38];

            para[0] = new SqlParameter
            {
                ParameterName = "jobT",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.fDesc
            };
            para[2] = new SqlParameter
            {
                ParameterName = "Type",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Type
            };
            para[3] = new SqlParameter
            {
                ParameterName = "NRev",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NRev
            };
            para[4] = new SqlParameter
            {
                ParameterName = "NDed",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NDed
            };
            para[5] = new SqlParameter
            {
                ParameterName = "Count",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Count
            };
            para[6] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.Remarks
            };
            para[7] = new SqlParameter
            {
                ParameterName = "InvExp",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvExp
            };
            para[8] = new SqlParameter
            {
                ParameterName = "InvServ",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvServ
            };
            para[9] = new SqlParameter
            {
                ParameterName = "Wage",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Wage
            };
            para[10] = new SqlParameter
            {
                ParameterName = "CType",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.CType
            };
            para[11] = new SqlParameter
            {
                ParameterName = "Status",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Status
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Charge",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Charge
            };
            para[13] = new SqlParameter
            {
                ParameterName = "Post",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Post
            };
            para[14] = new SqlParameter
            {
                ParameterName = "fInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.fInt
            };
            para[15] = new SqlParameter
            {
                ParameterName = "GLInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.GLInt
            };
            para[16] = new SqlParameter
            {
                ParameterName = "JobClose",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.JobClose
            };
            para[17] = new SqlParameter
            {
                ParameterName = "tempRev",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.TemplateRev
            };
            para[18] = new SqlParameter
            {
                ParameterName = "RevRemarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.RevRemarks
            };
            para[19] = new SqlParameter
            {
                ParameterName = "alertType",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.AlertType
            };
            para[20] = new SqlParameter
            {
                ParameterName = "alertMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.AlertMgr
            };
            para[21] = new SqlParameter
            {
                ParameterName = "MilestoneMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.MilestoneMgr
            };
            para[22] = new SqlParameter
            {
                ParameterName = "BomItem",
                SqlDbType = SqlDbType.Structured,
                Value = _objJob.ProjectDt
            };
            if (_objJob.MilestoneDt != null)
            {
                if (_objJob.MilestoneDt.Rows.Count > 0)
                {
                    para[23] = new SqlParameter
                    {
                        ParameterName = "MilestonItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.MilestoneDt
                    };
                }
            }
            if (_objJob.CustomTabItem != null)
            {
                if (_objJob.CustomTabItem.Rows.Count > 0)
                {
                    para[24] = new SqlParameter
                    {
                        ParameterName = "CustomTabItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomTabItem
                    };
                }
            }
            if (_objJob.CustomItem != null)
            {
                if (_objJob.CustomItem.Rows.Count > 0)
                {
                    para[25] = new SqlParameter
                    {
                        ParameterName = "CustomItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomItem
                    };
                }
            }
            if (_objJob.CustomItemDelete != null)
            {
                if (_objJob.CustomItemDelete.Rows.Count > 0)
                {
                    para[26] = new SqlParameter
                    {
                        ParameterName = "CustomItemDelete",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomItemDelete
                    };
                }
            }
            para[27] = new SqlParameter
            {
                ParameterName = "UnrecognizedRevenue",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.UnrecognizedRevenue
            };
            para[28] = new SqlParameter
            {
                ParameterName = "UnrecognizedExpense",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.UnrecognizedExpense
            };
            para[29] = new SqlParameter
            {
                ParameterName = "RetainageReceivable",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.RetainageReceivable
            };
            para[30] = new SqlParameter
            {
                ParameterName = "TargetHPermission",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.TargetHPermission
            };
            para[31] = new SqlParameter
            {
                ParameterName = "OHPer",
                SqlDbType = SqlDbType.Float,
                Value = _objJob.OHPer
            };
            para[32] = new SqlParameter
            {
                ParameterName = "COMMSPer",
                SqlDbType = SqlDbType.Float,
                Value = _objJob.COMMSPer
            };
            para[33] = new SqlParameter
            {
                ParameterName = "MARKUPPer",
                SqlDbType = SqlDbType.Float,
                Value = _objJob.MARKUPPer
            };
            para[34] = new SqlParameter
            {
                ParameterName = "STaxName",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.STaxName
            };
            para[35] = new SqlParameter
            {
                ParameterName = "EstimateType",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.EstimateType
            };
            para[36] = new SqlParameter
            {
                ParameterName = "IsSglBilAmt",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.IsSglBilAmt
            };
            para[37] = new SqlParameter
            {
                ParameterName = "IsBilFrmBOM",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.IsBilFrmBOM
            };
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.StoredProcedure, "spUpdateProjectTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTemplateStatus(JobT _objJob)
        {
            try
            {
                SqlHelper.ExecuteScalar(_objJob.ConnConfig, "spUpdateProjectStatus", _objJob.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getProspectIDbyEstimateID(Customer objPropCustomer)
        {
            string query = "select p.ID from Prospect p inner join Estimate e on p.Rol = e.RolID where e.ID ='" + objPropCustomer.estimateno + "'";
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getEstimateAgreement(Customer objPropCustomer)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       BDate, \n");
            varname1.Append("       Remarks, \n");
            varname1.Append("       Quoted \n");
            varname1.Append("       from Estimate \n");
            varname1.Append("       Where ID = " + objPropCustomer.TemplateID + " \n");
            varname1.Append(" Select r.ID, r.Name, r.City, r.State, r.Zip, r.Address from Rol r \n");
            varname1.Append(" inner join Estimate e on r.ID = e.RolID Where e.ID = " + objPropCustomer.TemplateID + " \n");
            varname1.Append(" Select p.ID, p.fFirst, p.Last from Emp p inner join Estimate e \n");
            varname1.Append(" on p.ID = e.EmpID Where e.ID =  " + objPropCustomer.TemplateID + " \n");

            //string strQuery = "select ID, Name ,fDesc,Remarks, rolid, locid,( case ffor when 'ACCOUNT' then (select tag from loc where loc = locid) when 'PROSPECT' then (select name from rol where id=rolid) end )as contact, isnull(cadexchange,0) as cadexchange, status, job from Estimate where ID=" + objPropCustomer.TemplateID;
            //strQuery += " select ID, Estimate , Line , fDesc as scope, Quan as quantity,Cost,Price as amount,Amount as total , vendor, isnull( currency ,'') as currency,isnull( measure,1) measure, code,fdesc, amount as budget from EstimateI where Estimate=" + objPropCustomer.TemplateID;
            //strQuery += " select ID, Line, TemplateID, LabourID, Amount from tblJoinLaborTemplate where TemplateID = " + objPropCustomer.TemplateID;
            //strQuery += "SELECT j.JobT,j.Job,m.JobTItemID as JobTItem,j.Type as jtype,j.fDesc, j.Code as jcode, j.Line, m.MilestoneName as MilesName, m.RequiredBy as RequiredBy,j.ETCMod as LeadTime,ProjAcquistDate,ActAcquistDate, Comments, isnull(m.Type,0) as Type, isnull(o.Department,'') AS Department, isnull(m.Amount, 0) as Amount FROM jobtitem j INNER JOIN Milestone m ON m.JobtItemId = j.ID LEFT JOIN OrgDep o ON o.ID = m.Type WHERE (j.job=0 or j.job is null) AND j.jobT= " + objPropCustomer.TemplateID;
            //strQuery += "select  j.id, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count, j.Type  from JobT j where j.ID = " + objPropCustomer.TemplateID + "order by j.ID ";
            //strQuery += "SELECT j.Code as jcode, j.fDesc, j.Type as jtype, b.Type as Btype, b.Item as BItem,b.QtyRequired as QtyReq, b.UM as UM, b.ScrapFactor as ScrapFact, b.BudgetUnit as BudgetUnit,b.BudgetExt, j.Line,b.Vendor,b.TotalPrice,LTRIM(RTRIM(b.Currency)) as currency,b.AmountDollars as Amount,b.Percentage as jPercent FROM jobtitem j INNER JOIN Bom b ON b.JobtItemId = j.ID WHERE (j.job=0 or j.job is null) AND j.jobT=" + objPropCustomer.TemplateID + "";
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddEstimate(Customer objPropCustomer)
        {
            var para = new SqlParameter[70];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Description
            };
            para[2] = new SqlParameter
            {
                ParameterName = "remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };

            para[3] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[4] = new SqlParameter
            {
                ParameterName = "mode",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Mode
            };
            para[5] = new SqlParameter
            {
                ParameterName = "loc",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };

            para[6] = new SqlParameter
            {
                ParameterName = "rol",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ROL
            };
            para[7] = new SqlParameter
            {
                ParameterName = "CADExchange",
                SqlDbType = SqlDbType.Money,
                Value = objPropCustomer.CADExchange
            };
            para[8] = new SqlParameter
            {
                ParameterName = "Edited",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.IsItemEdited
            };
            para[9] = new SqlParameter
            {
                ParameterName = "Status",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Status
            };
            if (objPropCustomer.DtMilestone != null)
            {
                para[10] = new SqlParameter
                {
                    ParameterName = "MilestonItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtMilestone
                };
            }
            if (objPropCustomer.DtBOM != null)
            {
                para[11] = new SqlParameter
                {
                    ParameterName = "BomItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtBOM
                };
            }
            para[12] = new SqlParameter
            {
                ParameterName = "Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[13] = new SqlParameter
            {
                ParameterName = "estDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.date
            };
            para[14] = new SqlParameter
            {
                ParameterName = "Jobtype",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Type
            };
            para[15] = new SqlParameter
            {
                ParameterName = "SalesManUerId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Worker
            };
            para[16] = new SqlParameter
            {
                ParameterName = "bidDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.TimeDue
            };
            para[17] = new SqlParameter
            {
                ParameterName = "billAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[18] = new SqlParameter
            {
                ParameterName = "phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[19] = new SqlParameter
            {
                ParameterName = "fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[20] = new SqlParameter
            {
                ParameterName = "Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[21] = new SqlParameter
            {
                ParameterName = "EstimateAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[22] = new SqlParameter
            {
                ParameterName = "EstimateCell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[23] = new SqlParameter
            {
                ParameterName = "BidPrice",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.BidPrice
            };
            para[24] = new SqlParameter
            {
                ParameterName = "Override",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Override
            };
            para[25] = new SqlParameter
            {
                ParameterName = "Cont",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Cont
            };
            para[26] = new SqlParameter
            {
                ParameterName = "OH",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.OH
            };
            para[27] = new SqlParameter
            {
                ParameterName = "SalesTax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.STax
            };
            para[28] = new SqlParameter
            {
                ParameterName = "Opportunity",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.OpportunityID
            };
            para[29] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            para[30] = new SqlParameter
            {
                ParameterName = "OHPer",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.OHPer
            };

            para[31] = new SqlParameter
            {
                ParameterName = "MarkupPer",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.MarkupPer
            };

            para[32] = new SqlParameter
            {
                ParameterName = "CommissionPer",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CommissionPer
            };

            para[33] = new SqlParameter
            {
                ParameterName = "CommissionVal",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CommissionVal
            };

            para[34] = new SqlParameter
            {
                ParameterName = "STaxRate",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.STaxRate
            };

            para[35] = new SqlParameter
            {
                ParameterName = "Category",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Category
            };
            para[36] = new SqlParameter
            {
                ParameterName = "CompanyName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CompanyName
            };
            para[37] = new SqlParameter
            {
                ParameterName = "Sales_Tax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Sales_Tax
            };

            para[38] = new SqlParameter
            {
                ParameterName = "MarkupVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.MarkupVal
            };
            para[39] = new SqlParameter
            {
                ParameterName = "STaxVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.STaxVal
            };
            para[40] = new SqlParameter
            {
                ParameterName = "MatExp",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.MatExp
            };
            para[41] = new SqlParameter
            {
                ParameterName = "LabExp",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.LabExp
            };
            para[42] = new SqlParameter
            {
                ParameterName = "OtherExp",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.OtherExp
            };
            para[43] = new SqlParameter
            {
                ParameterName = "SubToalVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.SubToalVal
            };
            para[44] = new SqlParameter
            {
                ParameterName = "TotalCostVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.TotalCostVal
            };
            para[45] = new SqlParameter
            {
                ParameterName = "PretaxTotalVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.PretaxTotalVal
            };
            para[46] = new SqlParameter
            {
                ParameterName = "PType",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.PType
            };
            para[47] = new SqlParameter
            {
                ParameterName = "Amount",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Amount
            };
            para[48] = new SqlParameter
            {
                ParameterName = "BillRate",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.BillRate
            };
            para[49] = new SqlParameter
            {
                ParameterName = "OT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateOT
            };
            para[50] = new SqlParameter
            {
                ParameterName = "RateTravel",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateTravel
            };
            para[51] = new SqlParameter
            {
                ParameterName = "DT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateDT
            };
            para[52] = new SqlParameter
            {
                ParameterName = "RateMileage",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Mileage
            };
            para[53] = new SqlParameter
            {
                ParameterName = "RateNT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateNT
            };

            para[54] = new SqlParameter
            {
                ParameterName = "ContPer",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.ContPer
            };

            para[55] = new SqlParameter
            {
                ParameterName = "Discounted",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.Discounted
            };

            para[56] = new SqlParameter
            {
                ParameterName = "DiscountedNotes",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.DiscountedNotes
            };

            if (objPropCustomer.DtEquips != null)
            {
                para[57] = new SqlParameter
                {
                    ParameterName = "EquipItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtEquips
                };
            }

            //para[58] = new SqlParameter
            //{
            //    ParameterName = "GroupName",
            //    SqlDbType = SqlDbType.VarChar,
            //    Value = objPropCustomer.GroupName
            //};

            para[58] = new SqlParameter
            {
                ParameterName = "GroupId",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.GroupId
            };

            if (objPropCustomer.DtGridUserSettings != null)
            {
                para[59] = new SqlParameter
                {
                    ParameterName = "GridUserSettings",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtGridUserSettings
                };
            }

            para[60] = new SqlParameter
            {
                ParameterName = "UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Username
            };

            para[61] = new SqlParameter
            {
                ParameterName = "CustomItems",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtCustom
            };

            para[62] = new SqlParameter
            {
                ParameterName = "EstimateType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.EstimateType
            };

            para[63] = new SqlParameter
            {
                ParameterName = "IsSglBilAmt",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.IsSglBilAmt
            };

            para[64] = new SqlParameter
            {
                ParameterName = "OpportunityStageID",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.OpportunityStageID
            };

            para[65] = new SqlParameter
            {
                ParameterName = "OpportunityName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.OpportunityName
            };
            para[66] = new SqlParameter
            {
                ParameterName = "IsCertifiedProject",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.IsCertifiedProject
            };
            para[67] = new SqlParameter
            {
                ParameterName = "SoldDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.SoldDate
            };

            para[68] = new SqlParameter
            {
                ParameterName = "IsBilFrmBOM",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.IsBilFrmBOM
            };
            para[69] = new SqlParameter
            {
                ParameterName = "Comment",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Comment
            };

            try
            {
                var id = SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddEstimate", para);
                //return Convert.ToInt32(para[23].Value);
                return Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEstimate(Customer objPropCustomer)
        {
            var para = new SqlParameter[70];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Description
            };
            para[2] = new SqlParameter
            {
                ParameterName = "remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };

            para[3] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[4] = new SqlParameter
            {
                ParameterName = "mode",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Mode
            };
            para[5] = new SqlParameter
            {
                ParameterName = "loc",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };

            para[6] = new SqlParameter
            {
                ParameterName = "rol",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ROL
            };
            para[7] = new SqlParameter
            {
                ParameterName = "CADExchange",
                SqlDbType = SqlDbType.Money,
                Value = objPropCustomer.CADExchange
            };
            para[8] = new SqlParameter
            {
                ParameterName = "Edited",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.IsItemEdited
            };
            para[9] = new SqlParameter
            {
                ParameterName = "Status",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Status
            };
            if (objPropCustomer.DtMilestone != null)
            {
                para[10] = new SqlParameter
                {
                    ParameterName = "MilestonItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtMilestone
                };
            }
            para[11] = new SqlParameter
            {
                ParameterName = "BomItem",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtBOM
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[13] = new SqlParameter
            {
                ParameterName = "estDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.date
            };
            para[14] = new SqlParameter
            {
                ParameterName = "EstimateNo",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.estimateno
            };
            para[15] = new SqlParameter
            {
                ParameterName = "Jobtype",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Type
            };
            para[16] = new SqlParameter
            {
                ParameterName = "SalesManUerId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Worker
            };
            para[17] = new SqlParameter
            {
                ParameterName = "bidDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.TimeDue
            };
            para[18] = new SqlParameter
            {
                ParameterName = "billAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[19] = new SqlParameter
            {
                ParameterName = "phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[20] = new SqlParameter
            {
                ParameterName = "fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[21] = new SqlParameter
            {
                ParameterName = "Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[22] = new SqlParameter
            {
                ParameterName = "EstimateAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[23] = new SqlParameter
            {
                ParameterName = "EstimateCell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[24] = new SqlParameter
            {
                ParameterName = "BidPrice",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.BidPrice
            };
            para[25] = new SqlParameter
            {
                ParameterName = "Override",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Override
            };
            para[26] = new SqlParameter
            {
                ParameterName = "Cont",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Cont
            };
            para[27] = new SqlParameter
            {
                ParameterName = "OH",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.OH
            };
            para[28] = new SqlParameter
            {
                ParameterName = "SalesTax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.STax
            };

            para[29] = new SqlParameter
            {
                ParameterName = "OHPer",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.OHPer
            };

            para[30] = new SqlParameter
            {
                ParameterName = "MarkupPer",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.MarkupPer
            };

            para[31] = new SqlParameter
            {
                ParameterName = "CommissionPer",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CommissionPer
            };

            para[32] = new SqlParameter
            {
                ParameterName = "CommissionVal",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CommissionVal
            };

            para[33] = new SqlParameter
            {
                ParameterName = "STaxRate",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.STaxRate
            };

            para[34] = new SqlParameter
            {
                ParameterName = "Category",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Category
            };
            para[35] = new SqlParameter
            {
                ParameterName = "CompanyName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CompanyName
            };

            para[36] = new SqlParameter
            {
                ParameterName = "Sales_Tax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Sales_Tax
            };


            para[37] = new SqlParameter
            {
                ParameterName = "MarkupVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.MarkupVal
            };

            para[38] = new SqlParameter
            {
                ParameterName = "STaxVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.STaxVal
            };
            para[39] = new SqlParameter
            {
                ParameterName = "MatExp",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.MatExp
            };
            para[40] = new SqlParameter
            {
                ParameterName = "LabExp",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.LabExp
            };
            para[41] = new SqlParameter
            {
                ParameterName = "OtherExp",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.OtherExp
            };
            para[42] = new SqlParameter
            {
                ParameterName = "SubToalVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.SubToalVal
            };
            para[43] = new SqlParameter
            {
                ParameterName = "TotalCostVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.TotalCostVal
            };
            para[44] = new SqlParameter
            {
                ParameterName = "PretaxTotalVal",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.PretaxTotalVal
            };
            para[45] = new SqlParameter
            {
                ParameterName = "PType",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.PType
            };
            para[46] = new SqlParameter
            {
                ParameterName = "Amount",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Amount
            };
            para[47] = new SqlParameter
            {
                ParameterName = "BillRate",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.BillRate
            };
            para[48] = new SqlParameter
            {
                ParameterName = "OT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateOT
            };
            para[49] = new SqlParameter
            {
                ParameterName = "RateTravel",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateTravel
            };
            para[50] = new SqlParameter
            {
                ParameterName = "DT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateDT
            };
            para[51] = new SqlParameter
            {
                ParameterName = "RateMileage",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Mileage
            };
            para[52] = new SqlParameter
            {
                ParameterName = "RateNT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateNT
            };

            para[53] = new SqlParameter
            {
                ParameterName = "ContPer",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.ContPer
            };

            para[54] = new SqlParameter
            {
                ParameterName = "Discounted",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.Discounted
            };

            para[55] = new SqlParameter
            {
                ParameterName = "DiscountedNotes",
                SqlDbType = SqlDbType.NVarChar,
                Value = objPropCustomer.DiscountedNotes
            };

            if (objPropCustomer.DtEquips != null)
            {
                para[56] = new SqlParameter
                {
                    ParameterName = "EquipItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtEquips
                };
            }

            //para[57] = new SqlParameter
            //{
            //    ParameterName = "GroupName",
            //    SqlDbType = SqlDbType.VarChar,
            //    Value = objPropCustomer.GroupName
            //};

            para[57] = new SqlParameter
            {
                ParameterName = "GroupId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.GroupId
            };

            para[58] = new SqlParameter
            {
                ParameterName = "Opportunity",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.OpportunityID
            };

            if (objPropCustomer.DtGridUserSettings != null)
            {
                para[59] = new SqlParameter
                {
                    ParameterName = "GridUserSettings",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtGridUserSettings
                };
            }

            para[60] = new SqlParameter
            {
                ParameterName = "UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Username
            };

            para[61] = new SqlParameter
            {
                ParameterName = "CustomItems",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtCustom
            };

            para[62] = new SqlParameter
            {
                ParameterName = "EstimateType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.EstimateType
            };

            para[63] = new SqlParameter
            {
                ParameterName = "IsSglBilAmt",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.IsSglBilAmt
            };
            para[64] = new SqlParameter
            {
                ParameterName = "OpportunityStageID",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.OpportunityStageID
            };

            para[65] = new SqlParameter
            {
                ParameterName = "OpportunityName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.OpportunityName
            };
            para[66] = new SqlParameter
            {
                ParameterName = "IsCertifiedProject",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.IsCertifiedProject
            };
            para[67] = new SqlParameter
            {
                ParameterName = "SoldDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.SoldDate
            };
            para[68] = new SqlParameter
            {
                ParameterName = "IsBilFrmBOM",
                SqlDbType = SqlDbType.Bit,
                Value = objPropCustomer.IsBilFrmBOM
            };
            para[69] = new SqlParameter
            {
                ParameterName = "Comment",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Comment
            };

            try
            {
                SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateEstimate", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void DeleteEstimate(Customer objCustomer)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "DELETE FROM EstimateForm WHERE Estimate=" + objCustomer.estimateno);
        //        SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "DELETE FROM EstimateRevisionNotes Where EstimateID=" + objCustomer.estimateno);
        //        SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "DELETE FROM BOM WHERE EstimateIId IN(SELECT ID FROM EstimateI WHERE Estimate=" + objCustomer.estimateno + " AND Type=1)");
        //        SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "DELETE FROM Milestone WHERE EstimateIId IN(SELECT ID FROM EstimateI WHERE Estimate=" + objCustomer.estimateno + " AND Type=0)");
        //        SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "DELETE FROM EstimateI Where Estimate=" + objCustomer.estimateno);
        //        SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "DELETE FROM Estimate Where ID=" + objCustomer.estimateno);
        //        SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "UPDATE Lead SET Estimate=NULL Where Estimate=" + objCustomer.estimateno);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void DeleteEstimate(Customer objCustomer)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "EstimateId",
                    SqlDbType = SqlDbType.Int,
                    Value = objCustomer.estimateno
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCustomer.Username
                };

                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.StoredProcedure, "spDeleteEstimateById", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJobTasks(Customer _objcsut)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objcsut.ConnConfig, CommandType.Text, "select * from Ticket_Task_Codes where job = " + _objcsut.job);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getJobTasksCategory(Customer _objcsut)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(_objcsut.ConnConfig, CommandType.Text, "select top 1 category from Ticket_Task_Codes where job = " + _objcsut.job));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetWorker(Customer _objcsut)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objcsut.ConnConfig, "spGetWorker", _objcsut.SearchBy, _objcsut.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEstimateByID(Customer objCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, "spGetEstimateByID", objCustomer.estimateno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEstimateByID(GetEstimateByIDParam _GetEstimateByID, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetEstimateByID", _GetEstimateByID.estimateno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllEstimate(Customer objCustomer, string estimateIDs)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "EstimateNo",
                    SqlDbType = SqlDbType.NText,
                    Value = estimateIDs
                };

                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, CommandType.StoredProcedure, "spGetAllEstimate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateBOM(Customer objCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, "spGetEstimateBOM", objCustomer.estimateno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetEstimateMilestone(Customer objCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, "spGetEstimateMilestone", objCustomer.estimateno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetEstimateOpportunityByEstimateID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetEstimateInfoForTags", objPropCustomer.estimateno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getBT(Customer objPropCustomer)
        {
            try
            {
                // return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from BusinessType");
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select ID, Description, Label,( (SELECT COUNT(*) FROM Prospect WHERE Prospect.BusinessType = BusinessType.Description) +(SELECT COUNT(*) FROM Loc WHERE Loc.BusinessType = BusinessType.ID) ) AS[Count] from BusinessType order By Description");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getReportName(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "Select top 1 * From tblSchedule");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool chkInvoiceOnlinePaymentPermission(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.OnlinePaymentPermission = Convert.ToBoolean(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.Text, "Select top 1 IsOnlinePaymentApply From Control"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getBT(GetBTParam _GetBT, string ConnectionString)
        {
            try
            {
                // return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from BusinessType");
                return _GetBT.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select ID, Description, Label,( (SELECT COUNT(*) FROM Prospect WHERE Prospect.BusinessType = BusinessType.Description) +(SELECT COUNT(*) FROM Loc WHERE Loc.BusinessType = BusinessType.ID) ) AS[Count] from BusinessType order By Description");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSetupDropDownValue(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "SELECT  ID,NAME FROM INV WHERE TYPE = 1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getService(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select ID,Description,Label,(SELECT COUNT(*) FROM Lead WHERE Lead.Product=CONVERT(NVARCHAR,Service.ID)) AS [Count] from Service order by Description");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSourceCount(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "SELECT fDesc,Type ,(SELECT COUNT(*) FROM Prospect WHERE Prospect.Source=SalesSource.fDesc) AS [Count] from SalesSource");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpportunityStatus(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from OEStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateCategory(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select distinct Category from Estimate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getPhoneByRol(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from phone where rol=" + objPropCustomer.ROL);
                //if (objPropCustomer.ProspectID != 0)
                //    return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from phone where rol=" + objPropCustomer.ROL);
                //else
                //    return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select ID, fDesc from phone where rol=" + objPropCustomer.ROL + " union select 0, contact from rol where id =" + objPropCustomer.ROL + " AND Contact is not null AND Contact != ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getPhoneByID(Customer objPropCustomer)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (objPropCustomer.PhoneID != 0)
                {
                    stringBuilder.AppendFormat("SELECT * FROM phone where ID={0}", objPropCustomer.PhoneID);
                }
                else
                {
                    stringBuilder.AppendFormat("SELECT '0' ID, Contact fDesc, Phone, Cellular Cell, Fax, Email FROM rol where ID={0}", objPropCustomer.RoleID);
                }
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBT(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spUpdateBT", objPropCustomer.BT.ID, objPropCustomer.BT.Description, objPropCustomer.BT.Count, objPropCustomer.Mode, objPropCustomer.BT.Label);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateService(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spUpdateService", objPropCustomer.Service.ID, objPropCustomer.Service.Description, objPropCustomer.Service.Count, objPropCustomer.Mode, objPropCustomer.Service.Label);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getSource(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select fdesc from SalesSource where fdesc !=''");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateStageHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update Stage set Label='" + objCustomer.HeaderStage + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBTHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update BusinessType set Label='" + objCustomer.HeaderBT + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateServicesHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update Service set Label='" + objCustomer.HeaderServices + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEquipmentCategoryHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update ElevatorSpec set Label='" + objCustomer.HeaderServices + "'Where ECat = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEquipmentTypeHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update ElevatorSpec set Label='" + objCustomer.HeaderServices + "'Where ECat = 1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEquipmentBuildingHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update ElevatorSpec set Label='" + objCustomer.HeaderServices + "'Where ECat = 2");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEquipmentClassificationHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update ElevatorSpec set Label='" + objCustomer.HeaderServices + "'Where ECat = 3");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddSource(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "INSERT INTO SalesSource (fdesc, type)values('" + objCustomer.Source + "','A')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerContact(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "UPDATE r SET  Contact = '" + objCustomer.Contact + "', Fax = '" + objCustomer.Fax + "', Phone = '" + objCustomer.Phone + "', Email = '" + objCustomer.Email + "', Cellular = '" + objCustomer.Cellular + "' FROM Owner o INNER JOIN Rol r ON o.Rol = r.ID WHERE o.ID = " + objCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerCollectionNote(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "UPDATE Owner SET CNotes = '" + objCustomer.RenewalNotes + "' WHERE ID = " + objCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAIAReportData(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "JobId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.job;

                para[1] = new SqlParameter();
                para[1].ParameterName = "WIPID";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objProp_Customer.WIPID;

                return objProp_Customer.WIP = SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spGetAIAReportHeader", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectVarianceReport(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "JobId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.job;

                return objProp_Customer.ProjectVariance = SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spGetProjectVarianceReport_Head", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetWIP(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "JobId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.job;

                para[1] = new SqlParameter();
                para[1].ParameterName = "WIPId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objProp_Customer.WIPID;

                return objProp_Customer.WIP = SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spGetWIPHeader", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DeleteWIP(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "JobId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.job;

                para[1] = new SqlParameter();
                para[1].ParameterName = "WIPId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objProp_Customer.WIPID;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spDeleteWIP", para));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateWIPStatus(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter();
                para[0].ParameterName = "JobId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.job;

                para[1] = new SqlParameter();
                para[1].ParameterName = "WIPId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objProp_Customer.WIPID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "WIPStatus";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objProp_Customer.WIPStatus;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Username";
                para[3].SqlDbType = SqlDbType.NVarChar;
                para[3].Value = objProp_Customer.Username;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spUpdateWIPStatus", para));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet UpdateMailFields(Customer objProp_Customer)
        {
            try
            {
                DataTable dt = objProp_Customer.WIP.Tables[0];

                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Id";
                para[0].SqlDbType = SqlDbType.NVarChar;
                para[0].Value = dt.Rows[0]["Id"];

                para[1] = new SqlParameter();
                para[1].ParameterName = "JobId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = dt.Rows[0]["JobId"];

                para[2] = new SqlParameter();
                para[2].ParameterName = "SendTo";
                para[2].SqlDbType = SqlDbType.NVarChar;
                para[2].Value = dt.Rows[0]["SendTo"];

                para[3] = new SqlParameter();
                para[3].ParameterName = "SendBy";
                para[3].SqlDbType = SqlDbType.NVarChar;
                para[3].Value = dt.Rows[0]["SendBy"];

                para[4] = new SqlParameter();
                para[4].ParameterName = "SendOn";
                para[4].SqlDbType = SqlDbType.DateTime;
                para[4].Value = dt.Rows[0]["SendOn"];

                return objProp_Customer.WIP = SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spUpdateWIPHeader_MailFields", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SaveWIP(Customer objProp_Customer)
        {
            try
            {
                DataTable dt = objProp_Customer.WIP.Tables[0];
                DataTable dt2 = objProp_Customer.WIP.Tables[1];
                SqlParameter[] para = new SqlParameter[10];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = dt.Rows[0]["Id"];

                para[1] = new SqlParameter();
                para[1].ParameterName = "JobId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = dt.Rows[0]["JobId"];

                para[2] = new SqlParameter();
                para[2].ParameterName = "ProgressBillingNo";
                para[2].SqlDbType = SqlDbType.NVarChar;
                para[2].Value = dt.Rows[0]["ProgressBillingNo"].ToString();

                para[3] = new SqlParameter();
                para[3].ParameterName = "InvoiceId";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = (dt.Rows[0]["InvoiceId"] == null) ? DBNull.Value : dt.Rows[0]["InvoiceId"];

                para[4] = new SqlParameter();
                para[4].ParameterName = "BillingDate";
                para[4].SqlDbType = SqlDbType.DateTime;
                para[4].Value = dt.Rows[0]["BillingDate"];

                para[5] = new SqlParameter();
                para[5].ParameterName = "ApplicationStatusId";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = dt.Rows[0]["ApplicationStatusId"];

                para[6] = new SqlParameter();
                para[6].ParameterName = "Terms";
                para[6].SqlDbType = SqlDbType.Int;
                para[6].Value = dt.Rows[0]["Terms"];

                para[7] = new SqlParameter();
                para[7].ParameterName = "SalesTax";
                para[7].SqlDbType = SqlDbType.Decimal;
                para[7].Value = dt.Rows[0]["SalesTax"];

                para[8] = new SqlParameter();
                para[8].ParameterName = "PeriodDate";
                para[8].SqlDbType = SqlDbType.DateTime;
                para[8].Value = dt.Rows[0]["PeriodDate"];

                para[9] = new SqlParameter();
                para[9].ParameterName = "RevisionDate";
                para[9].SqlDbType = SqlDbType.DateTime;
                para[9].Value = dt.Rows[0]["RevisionDate"];
                //para[8] = new SqlParameter();
                //para[8].ParameterName = "ArchitectName";
                //para[8].SqlDbType = SqlDbType.NVarChar;
                //para[8].Value = dt.Rows[0]["ArchitectName"].ToString();

                //para[9] = new SqlParameter();
                //para[9].ParameterName = "ArchitectAddress";
                //para[9].SqlDbType = SqlDbType.NVarChar;
                //para[9].Value = dt.Rows[0]["ArchitectAddress"].ToString();

                int WIPId = 0;

                try
                {
                    WIPId = Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spAddWIPHeader", para));
                    if (WIPId > 0 && dt2.Rows.Count > 0)
                    {
                        SqlParameter[] para2 = new SqlParameter[3];

                        para2[0] = new SqlParameter();
                        para2[0].ParameterName = "JobId";
                        para2[0].SqlDbType = SqlDbType.Int;
                        para2[0].Value = dt.Rows[0]["JobId"];

                        para2[1] = new SqlParameter();
                        para2[1].ParameterName = "WIPId";
                        para2[1].SqlDbType = SqlDbType.Int;
                        para2[1].Value = WIPId;

                        para2[2] = new SqlParameter();
                        para2[2].ParameterName = "WIPDetails";
                        para2[2].SqlDbType = SqlDbType.Structured;
                        para2[2].Value = dt2;

                        WIPId = Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spAddWIPDetails", para2));
                    }
                    return WIPId;
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

        public void AddRevisionNotes(Customer objPropCustomer)
        {
            var para = new SqlParameter[5];

            para[0] = new SqlParameter
            {
                ParameterName = "@EstimateID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.estimateno
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Notes",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RevisionNotes
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Version",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RevisionVersion
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@CreatedDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.RevisionCreated
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@CreatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RevisionUser
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddRevisionNotes", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddCollectionNotes(Customer objPropCustomer)
        {
            var para = new SqlParameter[6];

            para[0] = new SqlParameter
            {
                ParameterName = "@OwnerID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.OwnerID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Notes",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RevisionNotes
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@CreatedDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.RevisionCreated
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@CreatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RevisionUser
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@DefaultNotes",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.DefaultNote
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@locID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddCollectionNotes", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet RevisionNotesByEstimate(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "EstimateID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.estimateno;



                return objProp_Customer.WIP = SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "RevisionNotesByEstimateID", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCollectionNotes(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter();
                para[0].ParameterName = "OwnerID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.OwnerID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "LocID";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objProp_Customer.LocID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "ShowAll";
                para[2].SqlDbType = SqlDbType.Bit;
                para[2].Value = objProp_Customer.ShowAllNote;

                return objProp_Customer.WIP = SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spGetCollectionNotes", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDescAndAmountOfEstimateByID(Customer objProp_Customer)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT Milestone.MilestoneName as Name, EstimateI.fDesc as Description, EstimateI.Amount as Amount");
            sql.Append(" FROM EstimateI");
            sql.Append(" LEFT JOIN Milestone ON EstimateI.ID = Milestone.EstimateIId");
            sql.Append(" LEFT JOIN OrgDep ON Milestone.Type = OrgDep.ID");
            sql.Append(" WHERE EstimateI.Estimate = " + objProp_Customer.estimateno + " AND EstimateI.Type = 0");

            try
            {
                return objProp_Customer.DsCustomer = SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProspectLogs(Customer objProp_Customer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objProp_Customer.ProspectID + "  and Screen='Prospect' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTasksLogs(Customer objProp_Customer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objProp_Customer.TaskID + "  and Screen='Tasks' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProjectExportToExcel(Customer objPropCustomer, DataTable listJPIID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "ProjectIdTable";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = listJPIID;

                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "JPIGetData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getAllProjectTemplateCustomField(Customer objPropCustomer, DataTable dtJob)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Job";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = dtJob;

                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetAllProjectTemplateCustomField", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerLocationRemarks(Customer objPropCustomer)
        {
            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@OwnerID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.OwnerID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@LocID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.LastUpdateUser
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomerLocationRemarks", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateLogs(Customer objProp_Customer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objProp_Customer.estimateno + "  and Screen='Estimate' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTodoTasksOfUserForTheDate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetTodoTasksOfUserForTheDate", objPropCustomer.UserID, objPropCustomer.DueDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTemplateName(Customer objPropCustomer)
        {
            string strQuery = "";
            if (objPropCustomer.Status == 1)
            {
                strQuery = "select j.fdesc from JobT j left outer join  JobType jt on j.Type=jt.ID where j.ID =" + objPropCustomer.TemplateID + " order by j.ID ";
            }
            else
            {
                strQuery = "select j.fdesc  from JobT j left outer join  JobType jt on j.Type=jt.ID where j.status=0 and j.ID =" + objPropCustomer.TemplateID + " order by j.ID ";
            }
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllTemplateName(Customer objPropCustomer)
        {
            string strQuery = "";
            if (objPropCustomer.Status == 1)
            {
                strQuery = "select j.ID, j.fdesc from JobT j left outer join  JobType jt on j.Type=jt.ID order by j.ID ";
            }
            else
            {
                strQuery = "select  j.ID, j.fdesc  from JobT j left outer join  JobType jt on j.Type=jt.ID where j.status=0 order by j.ID ";
            }
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOppStatus(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "SELECT lead.Status, OEStatus.Name FROM lead Left join OEStatus ON OEStatus.ID = lead.Status WHERE lead.id = " + objPropCustomer.OpportunityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllOppStatus(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from OEStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateByIDToCalculateReport(Customer objPropCustomer)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT EstimateI.Code");
                sql.Append(" 	,EstimateI.fDesc");
                sql.Append(" 	,EstimateI.Line");
                sql.Append(" 	,ISNULL(EstimateI.Price, 0) AS BudgetUnit");
                sql.Append(" 	,BOMT.Type AS BType");
                sql.Append(" 	,ISNULL(EstimateI.Quan, 0) AS QtyReq");
                sql.Append(" 	,ISNULL(EstimateI.Cost, 0) AS BudgetExt");
                sql.Append(" 	,ISNULL(EstimateI.Labor, 0) AS LabExt");
                sql.Append(" 	,ISNULL(EstimateI.Rate, 0) AS LabRate");
                sql.Append(" 	,ISNULL(EstimateI.Hours, 0) AS LabHours");
                sql.Append(" 	,ISNULL(EstimateI.Amount, 0) AS TotalExt");
                sql.Append(" 	,ISNULL(EstimateI.LStax, 0) AS LSTax");
                sql.Append(" 	,ISNULL(EstimateI.STax, 0) AS STax");
                sql.Append(" 	,ISNULL(EstimateI.LMUAmt, 0) AS LabPrice");
                sql.Append(" 	,ISNULL(EstimateI.MMUAmt, 0) AS MatPrice");
                sql.Append(" 	,EstimateI.Orderno");
                sql.Append(" 	,EstimateI.Estimate");
                sql.Append(" FROM EstimateI");
                sql.Append(" INNER JOIN BOM ON BOM.EstimateIId = EstimateI.ID");
                sql.Append(" 	AND EstimateI.Type = 1");
                sql.Append(" INNER JOIN BOMT ON BOM.Type = BOMT.ID");
                sql.AppendFormat(" WHERE EstimateI.Estimate =" + objPropCustomer.estimateno);
                sql.Append(" 	AND EstimateI.Type = 1");
                sql.Append(" ORDER BY EstimateI.OrderNo");
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllEstimateToCalculateReport(Customer objPropCustomer)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT EstimateI.Code");
                sql.Append(" 	,EstimateI.fDesc");
                sql.Append(" 	,EstimateI.Line");
                sql.Append(" 	,ISNULL(EstimateI.Price, 0) AS BudgetUnit");
                sql.Append(" 	,BOMT.Type AS BType");
                sql.Append(" 	,ISNULL(EstimateI.Quan, 0) AS QtyReq");
                sql.Append(" 	,ISNULL(EstimateI.Cost, 0) AS BudgetExt");
                sql.Append(" 	,ISNULL(EstimateI.Labor, 0) AS LabExt");
                sql.Append(" 	,ISNULL(EstimateI.Rate, 0) AS LabRate");
                sql.Append(" 	,ISNULL(EstimateI.Hours, 0) AS LabHours");
                sql.Append(" 	,ISNULL(EstimateI.Amount, 0) AS TotalExt");
                sql.Append(" 	,ISNULL(EstimateI.LStax, 0) AS LSTax");
                sql.Append(" 	,ISNULL(EstimateI.STax, 0) AS STax");
                sql.Append(" 	,ISNULL(EstimateI.LMUAmt, 0) AS LabPrice");
                sql.Append(" 	,ISNULL(EstimateI.MMUAmt, 0) AS MatPrice");
                sql.Append(" 	,EstimateI.Orderno");
                sql.Append(" 	,EstimateI.Estimate");
                sql.Append(" FROM EstimateI");
                sql.Append(" INNER JOIN BOM ON BOM.EstimateIId = EstimateI.ID");
                sql.Append(" 	AND EstimateI.Type = 1");
                sql.Append(" INNER JOIN BOMT ON BOM.Type = BOMT.ID");
                sql.AppendFormat(" WHERE EstimateI.Type = 1");
                sql.Append(" ORDER BY EstimateI.OrderNo");
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDefaultWorkerHeader(Customer objCustomer)
        {
            try
            {
                SqlParameter param = new SqlParameter("@Label", SqlDbType.NVarChar, 50) { Value = objCustomer.HeaderServices };
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateDefaultRoute", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetDefaultWorkerHeader(Customer objCustomer)
        {
            string getValue = string.Empty;
            try
            {
                getValue = Convert.ToString(SqlHelper.ExecuteScalar(objCustomer.ConnConfig, CommandType.Text, "SELECT TOP 1 Label FROM tblSchedule WHERE Type = 'DefaultWorker'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return getValue;
        }

        //API
        public string GetDefaultWorkerHeader(GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader, string ConnectionString)
        {
            string getValue = string.Empty;
            try
            {
                getValue = Convert.ToString(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT TOP 1 Label FROM tblSchedule WHERE Type = 'DefaultWorker'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return getValue;
        }

        public DataSet getJobProjectByJobIDRatesByIdPersonByJob(Customer objCustomer)
        {
            try
            {
                return objCustomer.DsCustomer = SqlHelper.ExecuteDataset(objCustomer.ConnConfig, "spGetProjectByJobIDRatesByIdPersonByJob", objCustomer.ProjectJobID, objCustomer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEquipmentShutdownForReport(Customer objCustomer, DateTime endDate)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, "spGetEquipShutdownDataForReport", endDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEquipmentShutdownForReport(GetEquipmentShutdownForReportParam _GetEquipmentShutdownForReport, string ConnectionString, DateTime endDate)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetEquipShutdownDataForReport", endDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEquipmentShutdownActivityForReport(Customer objCustomer, DateTime startDate, DateTime endDate, string edId, bool filtered)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, "spGetEquipShutdownHistoryForReport", startDate, endDate, edId, filtered);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDowntimeEquipmentReport(Customer objCustomer, List<RetainFilter> filters, bool inclInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	temp.* \n");
                sb.Append("	,CASE WHEN temp.ReturnDate IS NULL THEN DATEDIFF(MINUTE, temp.[Date], '" + objCustomer.EndDate + "') ELSE DATEDIFF(MINUTE, temp.[Date], temp.ReturnDate) END AS Downtime \n");
                sb.Append("FROM ( \n");
                sb.Append("	SELECT  \n");
                sb.Append("		e.ID \n");
                sb.Append("		,l.loc \n");
                sb.Append("		,l.Tag AS LocName \n");
                sb.Append("		,r.Name AS CustomerName \n");
                sb.Append("		,e.Unit AS Equipment \n");
                sb.Append("		,CASE ISNULL(ed.ticket_id, 0)  \n");
                sb.Append("			WHEN 0 THEN ''  \n");
                sb.Append("			ELSE CONVERT(VARCHAR(50),ed.ticket_id) END Ticket \n");
                sb.Append("		,ed.created_on AS [Date] \n");
                sb.Append("		,(SELECT TOP 1 created_on  \n");
                sb.Append("			FROM ElevShutDownLog  \n");
                sb.Append("			WHERE elev_id = ed.elev_id AND created_on > ed.created_on AND [status] = 0  \n");
                sb.Append("			ORDER BY created_on) AS ReturnDate \n");
                sb.Append("		,m.fDesc AS Mechanic \n");
                sb.Append("		,CASE ed.[status]  \n");
                sb.Append("		WHEN 1 THEN  \n");
                sb.Append("			CASE ISNULL(ed.planned,0)  \n");
                sb.Append("				WHEN 0 THEN 'No'  \n");
                sb.Append("				ELSE 'Yes' END \n");
                sb.Append("			ELSE '' END AS Planned \n");
                sb.Append("		,ed.Reason \n");
                sb.Append("		,ed.LongDesc \n");
                sb.Append("		,CASE ed.[status]  \n");
                sb.Append("			WHEN 1 THEN 'Down' \n");
                sb.Append("			ELSE 'Return' END [Status] \n");
                sb.Append("		,CASE e.shut_down  \n");
                sb.Append("			WHEN 1 THEN 'Down' \n");
                sb.Append("			ELSE 'Return' END CurrentStatus \n");
                sb.Append("		,m.Super AS Supervisor \n");
                sb.Append("		,CASE  \n");
                sb.Append("			WHEN DPDA.DescRes IS NOT NULL THEN DPDA.DescRes  \n");
                sb.Append("			WHEN D.DescRes IS NOT NULL THEN D.DescRes  \n");
                sb.Append("			ELSE '-' END AS WorkCompleted \n");
                sb.Append("		,CASE ISNULL(ticket_id, 0)  \n");
                sb.Append("			WHEN 0 THEN UPPER(ISNULL(w.fDesc,u.fUser)) \n");
                sb.Append("			ELSE UPPER(wk.fDesc) END AS Worker \n");
                sb.Append("	FROM Elev e  \n");
                sb.Append("		INNER JOIN ElevShutDownLog ed ON ed.elev_id = e.ID \n");
                sb.Append("		INNER JOIN Loc l ON l.Loc = e.Loc \n");
                sb.Append("		INNER JOIN Owner o ON o.ID = l.Owner \n");
                sb.Append("		INNER JOIN Rol r ON r.ID = o.Rol \n");
                sb.Append("		INNER JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("		LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                sb.Append("		LEFT JOIN TicketDPDA AS DPDA ON DPDA.ID = ed.ticket_id \n");
                sb.Append("		LEFT JOIN TicketD AS D ON D.ID = ed.ticket_id \n");
                sb.Append("		LEFT JOIN tblUser u ON u.ID = ed.created_by \n");
                sb.Append("		LEFT JOIN tblWork w ON w.fDesc = u.fUser \n");
                sb.Append("		LEFT OUTER JOIN tblWork wk ON D.fWork = wk.ID OR DPDA.fWork = wk.ID \n");
                sb.Append("	WHERE ed.[status] = 1 \n");
                sb.Append("		AND ((ed.created_on >= '" + objCustomer.StartDate + "' AND ed.created_on < '" + objCustomer.EndDate + "') \n");
                sb.Append("		    OR (e.shut_down = 1 AND ed.created_on < '" + objCustomer.EndDate + "')) \n");

                if (!inclInactive)
                {
                    sb.Append("		AND e.Status = 0 \n");
                }

                if (!string.IsNullOrEmpty(objCustomer.SearchBy) && !string.IsNullOrEmpty(objCustomer.SearchValue))
                {
                    if (objCustomer.SearchBy == "address")
                    {
                        sb.Append("		AND (l.Address+', ' + l.City + ', ' + l.State+', ' + l.Zip) LIKE '%" + objCustomer.SearchValue + "%' \n");
                    }
                    else if (objCustomer.SearchBy == "r.name" || objCustomer.SearchBy == "l.id" || objCustomer.SearchBy == "l.tag" || objCustomer.SearchBy == "l.state")
                    {
                        sb.Append("		AND " + objCustomer.SearchBy + " LIKE '%" + objCustomer.SearchValue + "%' \n");
                    }
                    else if (objCustomer.SearchBy == "e.unit")
                    {
                        sb.Append("		AND " + objCustomer.SearchBy + " LIKE '%" + objCustomer.SearchValue + "%'\n");
                    }
                    else if (objCustomer.SearchBy == "e.fdesc")
                    {
                        sb.Append("		AND " + objCustomer.SearchBy + " LIKE '%" + objCustomer.SearchValue + "%' \n");
                    }
                    else if (objCustomer.SearchBy == "e.status")
                    {
                        sb.Append("		AND e.Status = " + objCustomer.SearchValue + "\n");
                    }
                    else
                    {
                        sb.Append("		AND " + objCustomer.SearchBy + " LIKE '%" + objCustomer.SearchValue + "%' \n");
                    }
                }

                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "unit")
                    {
                        sb.Append("		AND e.Unit LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "manuf")
                    {
                        sb.Append("		AND e.Manuf LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "fdesc")
                    {
                        sb.Append("		AND e.fDesc LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append("		AND e.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Classification")
                    {
                        sb.Append("		AND e.Classification LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "cat")
                    {
                        sb.Append("		AND e.Cat LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "status")
                    {
                        sb.Append("		AND (CASE e.Status WHEN 1 THEN 'Inactive' ELSE 'Active' END) LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "shut_down")
                    {
                        sb.Append("		AND (CASE e.shut_down WHEN 1 THEN 'Down' ELSE 'Return' END) LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "building")
                    {
                        sb.Append("		AND e.building LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Category")
                    {
                        sb.Append("		AND e.Category LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "name")
                    {
                        sb.Append("		AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "locid")
                    {
                        sb.Append("		AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Tag")
                    {
                        sb.Append("		AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append("		AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Price")
                    {
                        sb.Append("		AND e.Price = " + filter.FilterValue.Trim() + " \n");
                    }
                    if (filter.FilterColumn == "last")
                    {
                        sb.Append("		AND CONVERT(varchar, e.Last, 101) LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Install")
                    {
                        sb.Append("		AND CONVERT(varchar, e.Install, 101) LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                sb.Append(") AS temp \n");
                //sb.Append("WHERE temp.ReturnDate IS NOT NULL \n");

                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEquipmentShutdownActivityForReport(GetEquipShutdownActivityForReportParam _GetEquipShutdownActivityForReport, string ConnectionString, DateTime startDate, DateTime endDate, string edId, bool filtered)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetEquipShutdownHistoryForReport", startDate, endDate, edId, filtered);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ConvertLeadEquipment(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteScalar(objCustomer.ConnConfig, "spConvertEquipment", objCustomer.ProspectID, objCustomer.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void ConvertLeadEquipment(ConvertLeadEquipmentParam _ConvertLeadEquipment, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteScalar(ConnectionString, "spConvertEquipment", _ConvertLeadEquipment.ProspectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimatesByOpportunityID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetEstimatesByOpportunityID", objPropCustomer.OpportunityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimatesByOpportunityIDForProjectLinking(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetEstimatesByOpportunityIDForProjectLinking", objPropCustomer.OpportunityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEquipmentsOfEstimate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetEstimateEquipments", objPropCustomer.estimateno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTemplateByOppID(Customer objPropCustomer)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@oppId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.OpportunityID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@status",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Status
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@estimateId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.estimateno
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spGetTemplateByOppID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetAllOpportunityIDs(Customer objCustomer)
        //{
        //    try
        //    {
        //        var sql = "SELECT ID FROM Lead Order By ID";
        //        return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, CommandType.Text, sql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetAllOpportunityIDs(Customer objCustomer)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@rol",
                SqlDbType = SqlDbType.Int,
                Value = objCustomer.RoleID
            };
            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, CommandType.StoredProcedure, "spGetOpportunityIDsByRolID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateGroupNames(Customer objCustomer)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int,
                Value = objCustomer.RoleID
            };

            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, CommandType.StoredProcedure, "spGetEstimateGroupNameByRol", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEquipmentsByGroupId(Customer objPropCustomer)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@GroupId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.GroupId
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spGetEstimateEquipmentsByGroupId", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetProspectID(Customer objPropCustomer)
        {
            try
            {
                string prospectID = "";
                String query = string.Format("SELECT p.ID FROM Prospect p WHERE rol = {0}", objPropCustomer.RoleID);
                prospectID = Convert.ToString(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.Text, query));
                return prospectID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddUpdateEstimateGroup(Customer objPropCustomer)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@GroupId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.GroupId
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@GroupName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.GroupName
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@RolId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.RoleID
            };


            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddUpdateEstimateGroup", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet AddProjectGroup(Customer objPropCustomer)
        {
            var para = new SqlParameter[5];

            para[0] = new SqlParameter
            {
                ParameterName = "@LocID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@GroupName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.GroupName
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@ProjectId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectJobID
            };

            para[3] = new SqlParameter
            {
                ParameterName = "@GroupId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.GroupId
            };

            para[4] = new SqlParameter
            {
                ParameterName = "@EquipItem",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtEquips
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProjectGroup", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet AddJobProject_Notes(Customer objPropCustomer)
        {
            var para = new SqlParameter[3];
            para[0] = new SqlParameter
            {
                ParameterName = "@JobId",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.job
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Note",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Notes
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@CreatedBy",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.UserID
            };
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProjectNotes", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobProject_Notes(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectNotes", objPropCustomer.job);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobProject_NotesExport(Customer objPropCustomer, String lsProjectNoteID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectNotesToExport", lsProjectNoteID, objPropCustomer.job);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetLogs(Customer objProp_Customer)
        //{
        //    try
        //    {
        //        return SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objProp_Customer.LogRefId + "  and Screen='" + objProp_Customer.LogScreen + "' order by CreatedStamp desc ");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetLogs(Customer objProp_Customer)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Screen",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objProp_Customer.LogScreen
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.Int,
                    Value = objProp_Customer.LogRefId
                };

                return SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spGetLogs", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateAmountWIPInvoice(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "WIPId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.WIPID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Username";
                para[1].SqlDbType = SqlDbType.NVarChar;
                para[1].Value = objProp_Customer.Username;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spUpdateAmountWIPInvoice", para));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationOfCustomerInCaseUnique(Customer objProp_Customer)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("IF((SELECT count(*) FROM Owner o INNER JOIN loc l ON l.Owner = o.id WHERE o.id = {0}) = 1) \n", objProp_Customer.CustomerID);
                stringBuilder.AppendFormat("    SELECT l.Tag, l.Loc, l.Rol as locRol FROM Owner o INNER JOIN loc l ON l.Owner = o.id WHERE o.id = {0}", objProp_Customer.CustomerID);
                return SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.Text, stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectGroupNames(Customer objCustomer)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@ProjectId",
                SqlDbType = SqlDbType.Int,
                Value = objCustomer.ProjectJobID
            };

            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, CommandType.StoredProcedure, "spGetProjectGroupNames", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpportunityForecast(Customer objPropCustomer, int type)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("DROP TABLE IF EXISTS #temp \n");
                sb.Append("SELECT DISTINCT \n");
                sb.Append("	l.ID, \n");
                sb.Append("	r.Name, \n");
                sb.Append("	l.fDesc, \n");
                sb.Append("	l.RolType, \n");
                sb.Append("	r.EN, \n");
                sb.Append("	B.Name As Company, \n");
                sb.Append("	s.Name as Status, \n");
                sb.Append("	CASE l.Probability \n");
                sb.Append("		WHEN 0 THEN 'Excellent' \n");
                sb.Append("		WHEN 1 THEN 'Very Good' \n");
                sb.Append("		WHEN 2 THEN 'Good' \n");
                sb.Append("		WHEN 3 THEN 'Average' \n");
                sb.Append("		WHEN 4 THEN 'Poor' \n");
                sb.Append("	END AS Probability, \n");
                sb.Append("	l.Profit, \n");
                sb.Append("	l.CreateDate, \n");
                sb.Append("	l.CloseDate, \n");
                sb.Append("	FORMAT(l.CloseDate, 'MM-yyyy') AS Period, \n");
                sb.Append("	FORMAT(l.CloseDate, 'MMM-yyyy') AS PeriodLabel, \n");
                sb.Append("	FORMAT(l.CloseDate, 'MMMM') AS BidMonth, \n");
                sb.Append("	YEAR(l.CloseDate) AS BidYear, \n");
                sb.Append("	l.Rol, \n");
                sb.Append("	l.Remarks, \n");
                sb.Append("	CASE ISNULL(l.Closed,0) \n");
                sb.Append("		WHEN 1 THEN 'Yes' \n");
                sb.Append("		ELSE 'No' \n");
                sb.Append("	END AS Closed, \n");
                sb.Append("	l.revenue, \n");
                sb.Append("	l.fuser, \n");
                sb.Append("	l.CompanyName, \n");
                sb.Append("	p.Referral, \n");
                sb.Append("	CASE r.Type \n");
                sb.Append("		WHEN 4 THEN bt1.Description \n");
                sb.Append("		ELSE bt.Description \n");
                sb.Append("	END AS BusinessType, \n");
                sb.Append("	CASE r.Type \n");
                sb.Append("		WHEN 0 THEN 'Customer' \n");
                sb.Append("		WHEN 1 THEN 'Vendor' \n");
                sb.Append("		WHEN 2 THEN 'Bank' \n");
                sb.Append("		WHEN 3 THEN 'Lead' \n");
                sb.Append("		WHEN 4 THEN 'ACCOUNT' \n");
                sb.Append("		WHEN 5 THEN 'Employee' \n");
                sb.Append("		ELSE '' \n");
                sb.Append("	END AS fFor, \n");
                sb.Append("	CASE ISNULL((SELECT TOP 1 Discounted FROM Estimate WHERE Opportunity = l.ID AND Discounted = 1), 0) \n");
                sb.Append("		WHEN 0 THEN 'No' \n");
                sb.Append("		WHEN 1 THEN 'Yes' \n");
                sb.Append("		ELSE '' \n");
                sb.Append("	END AS EstimateDiscounted, \n");
                sb.Append("	ISNULL((SELECT SUM(Price) FROM Estimate WHERE Opportunity = l.ID), 0) as BidPrice, \n");
                sb.Append("	ISNULL((SELECT SUM(Quoted) FROM Estimate WHERE Opportunity = l.ID), 0) as FinalBid, \n");
                sb.Append("	CASE \n");
                sb.Append("		WHEN l.Department IS NULL THEN ( SELECT TOP 1 jt1.Type FROM Estimate e1 LEFT JOIN JobT j1 ON j1.ID = e1.Template LEFT JOIN JobType jt1 ON jt1.ID = j1.Type WHERE e1.Opportunity = l.ID) \n");
                sb.Append("		ELSE jt.Type \n");
                sb.Append("	END AS Dept \n");
                sb.Append("INTO #temp \n");
                sb.Append("FROM Lead l  \n");
                sb.Append("	INNER JOIN Rol r ON l.Rol = r.ID \n");
                sb.Append("	LEFT OUTER JOIN Prospect p ON p.Rol = l.Rol \n");
                sb.Append("	LEFT OUTER JOIN OEStatus s ON l.Status= s.ID \n");
                sb.Append("	LEFT JOIN Branch B ON B.ID = r.EN \n");
                sb.Append("	LEFT JOIN Loc lc ON lc.Rol = l.Rol \n");
                sb.Append("	LEFT JOIN BusinessType bt ON bt.Description = p.BusinessType \n");
                sb.Append("	LEFT JOIN BusinessType bt1 ON bt1.ID = lc.BusinessType  \n");
                sb.Append("	LEFT JOIN JobType jt ON jt.ID = l.Department \n");
                sb.Append("WHERE r.Type IN (3,4) \n");
                sb.Append("	AND CONVERT(date,l.CloseDate,101) >= CONVERT(date,'" + objPropCustomer.StartDate + "',101) \n");
                sb.Append("	AND CONVERT(date,l.CloseDate,101) <= CONVERT(date,'" + objPropCustomer.EndDate + "',101) \n");
                sb.Append("SELECT * FROM #temp \n");

                if (type != 0)
                {
                    sb.Append("	WHERE Dept IN ('Maintenance', 'Modernization') \n");
                }
                else
                {
                    sb.Append("	WHERE Dept IN ('M&R', 'Repair', 'Other') \n");
                }

                sb.Append("SELECT \n");
                sb.Append("	PeriodLabel AS Label, \n");
                sb.Append("	Period, \n");
                sb.Append("	BidMonth, \n");
                sb.Append("	BidYear, \n");
                sb.Append("	SUM(FinalBid) AS TotalAmount \n");
                sb.Append("FROM #temp \n");

                if (type != 0)
                {
                    sb.Append("	WHERE Dept IN ('Maintenance', 'Modernization') \n");
                }
                else
                {
                    sb.Append("	WHERE Dept IN ('M&R', 'Repair', 'Other') \n");
                }

                sb.Append("GROUP BY PeriodLabel, Period, BidMonth, BidYear \n");
                sb.Append("ORDER BY Period \n");
                sb.Append("DROP TABLE #temp \n");

                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateBacklog(Customer objPropCustomer, List<RetainFilter> filters, string status, bool isConvertToJob = false)
        {
            bool isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT Distinct \n");
            sb.Append("	e.ID,  \n");
            sb.Append("    e.Name,  \n");
            sb.Append("    e.fDesc,  \n");
            sb.Append("    e.fDate,  \n");
            sb.Append("    e.Opportunity,  \n");
            sb.Append("    ls.Description AS OpportunityStage,  \n");
            sb.Append("    l.OpportunityStageID,   \n");
            sb.Append("    e.Category,  \n");
            sb.Append("    e.EstimateAddress,  \n");
            sb.Append("    e.Remarks,  \n");
            sb.Append("    e.Job, \n");
            sb.Append("	j.fDate AS ConversionDate,  \n");
            sb.Append("    e.CompanyName,  \n");
            sb.Append("    t.SDesc AS AssignTo,  \n");
            sb.Append("    e.Contact,  \n");
            //sb.Append("    ph.fDesc AS Contact,  \n");
            sb.Append("    fFor,  \n");
            sb.Append("    s.Name AS [Status],  \n");
            sb.Append("    r.EN,  \n");
            sb.Append("    ISNULL(B.Name, '') AS Company,  \n");
            sb.Append("    ISNULL(e.Price,0) AS EstimatePrice,    \n");
            sb.Append("    ISNULL(e.Quoted,0) AS QuotedPrice,      \n");
            sb.Append("    CASE ISNULL(e.Discounted,0) WHEN 0 THEN 'No' ELSE 'Yes' END AS Discounted      \n");
            sb.Append("FROM  Estimate e   \n");
            sb.Append("	LEFT OUTER JOIN Job j ON j.ID = e.Job \n");
            sb.Append("    LEFT OUTER JOIN OEStatus s ON e.[Status]= s.ID  \n");
            sb.Append("    LEFT OUTER JOIN terr t ON E.EstimateUserId=t.ID  \n");
            sb.Append("    LEFT OUTER JOIN Rol r ON e.RolID = r.ID  \n");
            sb.Append("    LEFT OUTER JOIN Branch B ON B.ID = r.EN   \n");
            sb.Append("    LEFT OUTER JOIN Lead l ON l.ID = e.Opportunity  \n");
            sb.Append("    LEFT JOIN  Stage ls ON ls.ID = l.OpportunityStageID  \n");
            //sb.Append("    LEFT JOIN Phone ph ON CONVERT(int,ISNULL(e.Contact,'0')) = ph.ID  \n");
            sb.Append(" left join tblCommonCustomFieldsValue cv on cv.Ref = e.ID and cv.Screen = 'Estimate' \n");
            sb.Append(" left join tblCommonCustomFields cl on cl.ID = cv.tblCommonCustomFieldsID \n");

            if (objPropCustomer.EN == 1)
            {
                sb.Append(" LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN  \n");
            }
            sb.Append(" WHERE  EstTemplate = 0 \n");

            if (!string.IsNullOrEmpty(status))
            {
                sb.Append(" AND e.[Status] IN (" + status + ") \n");
            }

            if (objPropCustomer.Close == 1)
            {
                sb.Append(" AND fFor = 'ACCOUNT' \n");
                if (objPropCustomer.Center == "1")
                {
                    sb.Append(" AND e.Job IS NULL ");
                }
            }

            if (objPropCustomer.EN == 1)
            {
                sb.Append(" AND UC.IsSel = 1 AND UC.UserID =" + objPropCustomer.UserID);
            }

            if (isConvertToJob)
            {
                sb.Append(" AND e.Job > 0 \n");
            }

            if (filters != null && filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0; string[] filterArrayValue;
                        var filterColumn = items.FilterColumn.ToLower();
                        StringBuilder filteredQuery = new StringBuilder();
                        if (filterColumn == "id")
                        {
                            filterArrayValue = items.FilterValue.ToString().Split(',');

                            // Reset start date, end date if filter by Id
                            objPropCustomer.StartDate = string.Empty;
                            objPropCustomer.EndDate = string.Empty;

                            foreach (var filtered in filterArrayValue)
                            {
                                if (int.TryParse(filtered, out FilterValue))
                                {
                                    if (filteredQuery.Length == 0)
                                    {
                                        filteredQuery.Append(filtered);
                                    }
                                    else
                                    {
                                        filteredQuery.Append("," + filtered);
                                    }
                                }


                            }

                            sb.Append(" AND e.ID IN (" + filteredQuery.ToString() + ")");
                        }
                        else if (filterColumn == "status")
                        {
                            sb.Append(" AND s.Name = '" + items.FilterValue + "'");
                        }
                        else if (filterColumn == "ffor")
                        {
                            if (items.FilterValue.Trim().ToLower() == "existing")
                            {
                                sb.Append(" AND fFor = 'ACCOUNT'");
                            }
                            else if (items.FilterValue.Trim().ToLower() == "lead")
                            {
                                sb.Append(" AND fFor <> 'ACCOUNT'");
                            }
                            else
                            {
                                sb.Append(" AND fFor = '" + items.FilterValue + "'");
                            }

                        }
                        else if (filterColumn == "template")
                        {
                            sb.Append(" AND e." + filterColumn + " = " + items.FilterValue);
                        }
                        else if (filterColumn == "opportunitystage")
                        {
                            sb.Append(" AND ls.Description LIKE '%" + items.FilterValue + "%'");
                        }
                        else if (filterColumn == "companyname" ||
                            filterColumn == "estimateaddress" ||
                            filterColumn == "opportunity" ||
                            filterColumn == "job" ||
                            filterColumn == "category" ||
                            filterColumn == "fdesc" ||
                            filterColumn == "contact"
                            )
                        {
                            sb.Append("   AND e." + filterColumn + " LIKE '%" + items.FilterValue + "%' \n");
                        }
                        else if (filterColumn == "assignto")
                        {
                            sb.Append("   AND ( t.SDesc LIKE '%" + items.FilterValue + "%' ) \n");
                        }
                        //else if (filterColumn == "contact")
                        //{
                        //    sb.Append("   AND ph.fDesc LIKE '%" + items.FilterValue + "%' \n");
                        //}
                        else if (filterColumn == "quotedprice")
                        {
                            double quoteValue;
                            if (double.TryParse(items.FilterValue, out quoteValue))
                            {
                                sb.Append("   AND e.Quoted = " + quoteValue + " \n");
                            }
                        }
                        else if (filterColumn == "estimateprice")
                        {
                            double priceValue;
                            if (double.TryParse(items.FilterValue, out priceValue))
                            {
                                sb.Append("   AND e.Price = " + priceValue + " \n");
                            }
                        }
                        else if (filterColumn == "discounted")
                        {
                            if (items.FilterValue.Trim().ToLower() == "yes")
                            {
                                sb.Append("   AND e.Discounted = 1 \n");
                            }
                            else if (items.FilterValue.Trim().ToLower() == "no")
                            {
                                sb.Append("   AND isnull(e.Discounted,0) = 0 \n");
                            }
                            else
                            {
                                sb.Append("   AND 1=0 \n");
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(objPropCustomer.SearchBy) && !string.IsNullOrEmpty(objPropCustomer.SearchValue))
            {
                if ((objPropCustomer.SearchBy).ToLower() == "e.id")
                {
                    int FilterValue = 0; string[] filterArrayValue;
                    filterArrayValue = objPropCustomer.SearchValue.Split(',');
                    StringBuilder filteredQuery = new StringBuilder();

                    // Reset start date, end date if filter by Id
                    objPropCustomer.StartDate = string.Empty;
                    objPropCustomer.EndDate = string.Empty;
                    foreach (var filtered in filterArrayValue)
                    {
                        if (int.TryParse(filtered, out FilterValue))
                        {
                            if (filteredQuery.Length == 0)
                            {
                                filteredQuery.Append(filtered);
                            }
                            else
                            {
                                filteredQuery.Append("," + filtered);
                            }
                        }
                    }
                    sb.Append(" AND e.ID  IN (" + filteredQuery.ToString() + ")");
                }

                if ((objPropCustomer.SearchBy).ToLower() == "e.status"
                    || (objPropCustomer.SearchBy).ToLower() == "e.ffor"
                    || (objPropCustomer.SearchBy).ToLower() == "e.template"
                    || (objPropCustomer.SearchBy).ToLower() == "l.opportunitystageid"
                    )
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " = '" + objPropCustomer.SearchValue + "'       \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.companyname")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.estimateaddress")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.opportunity")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.job")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.category")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "em.ffirst")
                {
                    sb.Append("   AND ( t.SDesc LIKE '%" + objPropCustomer.SearchValue + "%' ) \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.contact")
                {
                    sb.Append("   AND e.contact LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "customfield")
                {
                    sb.Append("   AND ISNULL(cl.IsDeleted, 0) = 0 AND cl.ID = '" + objPropCustomer.SearchValue + "' AND cv.Value LIKE '%" + objPropCustomer.SearchValueExt + "%'\n");
                }
            }

            //if (!string.IsNullOrEmpty(objPropCustomer.SearchBy) && (objPropCustomer.SearchBy).ToLower() == "e.bdate")
            //{
            //if (string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && !string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            //{
            //    sb.Append(" AND e.bdate <= '" + objPropCustomer.SearchValueToDt + "' \n");
            //}
            //else if (!string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            //{
            //    sb.Append(" AND e.bdate >= '" + objPropCustomer.SearchValueFrDt + "' \n");
            //}
            //else if (!string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && !string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            //{
            //    sb.Append(" AND e.bdate >= '" + objPropCustomer.SearchValueFrDt + "' and e.bdate <= '" + objPropCustomer.SearchValueToDt + "' \n");
            //}
            //}

            //if (objPropCustomer.StartDate != string.Empty && objPropCustomer.EndDate != string.Empty)
            //if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
            //{
            //    sb.Append("   AND  e.fDate >= '" + objPropCustomer.StartDate + "' AND e.fDate <= '" + objPropCustomer.EndDate + "' \n");
            //}

            if (string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && !string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            {
                sb.Append(" AND e.bdate <= '" + objPropCustomer.SearchValueToDt + "' \n");
            }
            else if (!string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            {
                sb.Append(" AND e.bdate >= '" + objPropCustomer.SearchValueFrDt + "' \n");
            }
            else if (!string.IsNullOrEmpty(objPropCustomer.SearchValueFrDt) && !string.IsNullOrEmpty(objPropCustomer.SearchValueToDt))
            {
                sb.Append(" AND e.bdate >= '" + objPropCustomer.SearchValueFrDt + "' and e.bdate <= '" + objPropCustomer.SearchValueToDt + "' \n");
            }

            if (string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
            {
                sb.Append(" AND e.fDate <= '" + objPropCustomer.EndDate + "' \n");
            }
            else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && string.IsNullOrEmpty(objPropCustomer.EndDate))
            {
                sb.Append(" AND e.fDate >= '" + objPropCustomer.StartDate + "' \n");
            }
            else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
            {
                sb.Append(" AND e.fDate >= '" + objPropCustomer.StartDate + "' and e.fDate <= '" + objPropCustomer.EndDate + "' \n");
            }

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateRate(Customer objPropCustomer, List<RetainFilter> filters, string status, bool isConvertToJob = false)
        {
            bool isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT Distinct  \n");
            sb.Append("	e.ID AS [Tender],   \n");
            sb.Append("	'Normal' AS [Structure],   \n");
            sb.Append("	'712' AS [Company],   \n");
            sb.Append("	'TEI' AS [BU],   \n");
            sb.Append("	'TEI' AS [Department],   \n");
            sb.Append("	'USA' AS [ProjectRegion],   \n");
            sb.Append("	e.EstimateAddress AS [TenderName],   \n");
            sb.Append("	'Direct contract' AS [CONTRACTUALARRANGEMENT],   \n");
            sb.Append("	e.fDesc AS [JobDescription],   \n");
            sb.Append("	(CASE ISNULL(e.Quoted,0) WHEN 0 THEN ISNULL(e.Price,0) ELSE e.Quoted END) AS [TenderAmount],   \n");
            sb.Append("	'USD' AS [Currency],   \n");
            sb.Append("	CASE s.Name \n");
            sb.Append("		WHEN 'Open' THEN 'Despatched ' \n");
            sb.Append("		WHEN 'Cancelled' THEN 'Regret ' \n");
            sb.Append("		WHEN 'Withdrawn' THEN 'Regret ' \n");
            sb.Append("		WHEN 'Disqualified' THEN 'Regret' \n");
            sb.Append("		WHEN 'Sold' THEN 'Award ' \n");
            sb.Append("		WHEN 'Competitor' THEN 'Fail' \n");
            sb.Append("		WHEN 'Quoted' THEN 'Despatched ' \n");
            sb.Append("		ELSE '' \n");
            sb.Append("	END AS [Status],   \n");
            sb.Append("	CASE  \n");
            sb.Append("		WHEN s.Name = 'Open' OR s.Name = 'Quoted' THEN e.fDate \n");
            sb.Append("		WHEN s.Name = 'Cancelled' OR s.Name = 'Withdrawn' OR s.Name = 'Disqualified' THEN e.BDate \n");
            sb.Append("		ELSE NULL \n");
            sb.Append("	END AS [SentDate],   \n");
            sb.Append("	CASE ISNULL(j.BRev, 0) \n");
            sb.Append("		WHEN 0 THEN (CASE ISNULL(e.Quoted,0) WHEN 0 THEN ISNULL(e.Price,0) ELSE e.Quoted END) \n");
            sb.Append("		ELSE j.BRev \n");
            sb.Append("	END AS [ContractSum],   \n");
            sb.Append("	CASE  \n");
            sb.Append("		WHEN s.Name = 'Sold' THEN e.BDate \n");
            sb.Append("		ELSE NULL \n");
            sb.Append("	END AS [AwardDate],   \n");
            sb.Append("	'External' AS [ClientGroup],   \n");
            sb.Append("	'NG0999' AS [ParentCompanyGroup], \n");
            sb.Append("	CASE tp.Type \n");
            sb.Append("		WHEN 'Construction' THEN 'C ' \n");
            sb.Append("		WHEN 'Modernization' THEN 'C ' \n");
            sb.Append("		ELSE 'M' \n");
            sb.Append("	END AS [ProjectGroup],   \n");
            sb.Append("	'Non-government' AS [MarketSegment2],   \n");
            sb.Append("	'LE' AS [Segment]    \n");
            sb.Append("FROM  Estimate e WITH (NOLOCK)  \n");
            sb.Append("	LEFT OUTER JOIN Job j ON j.ID = e.Job  \n");
            sb.Append("	LEFT JOIN JobT jt ON e.Template = jt.ID \n");
            sb.Append("	LEFT JOIN JobType tp ON jt.Type = tp.ID \n");
            sb.Append(" LEFT OUTER JOIN OEStatus s ON e.[Status]= s.ID  \n");
            sb.Append(" LEFT OUTER JOIN terr t ON E.EstimateUserId=t.ID  \n");
            sb.Append(" LEFT OUTER JOIN Rol r ON e.RolID = r.ID  \n");
            sb.Append(" LEFT OUTER JOIN Lead l ON l.ID = e.Opportunity  \n");
            sb.Append(" LEFT JOIN  Stage ls ON ls.ID = l.OpportunityStageID  \n");
            sb.Append(" LEFT JOIN tblCommonCustomFieldsValue cv ON cv.Ref = e.ID and cv.Screen = 'Estimate' \n");
            sb.Append(" LEFT JOIN tblCommonCustomFields cl ON cl.ID = cv.tblCommonCustomFieldsID \n");

            if (objPropCustomer.EN == 1)
            {
                sb.Append(" LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN  \n");
            }
            sb.Append("WHERE  EstTemplate = 0 \n");
            sb.Append(" AND (s.Name = 'Sold' \n");
            sb.Append("     OR (s.Name <> 'Sold' AND tp.Type <> 'Modernization' AND tp.Type <> 'Construction') \n");
            sb.Append("     OR (s.Name <> 'Sold' AND (tp.Type = 'Modernization' OR tp.Type = 'Construction') \n");
            sb.Append("         AND e.Category <> 'Guaranteed Maint.' AND e.Category <> 'Maintenance' AND e.Category <> 'Alternate' AND e.Category <> 'Mod Altrenate')) \n");
            sb.Append(" AND (jt.fDesc <> 'Covered Repair' OR (jt.fDesc = 'Covered Repair' AND e.fDesc NOT LIKE '%Non billable%' AND e.fDesc NOT LIKE '%Non-billable%' )) \n");
            sb.Append(" AND (jt.fDesc <> 'Covered MR' OR (jt.fDesc = 'Covered MR' AND e.fDesc NOT LIKE '%Non billable%' AND e.fDesc NOT LIKE '%Non-billable%' )) \n");

            if (!string.IsNullOrEmpty(status))
            {
                sb.Append(" AND e.[Status] IN (" + status + ") \n");
            }

            if (objPropCustomer.Close == 1)
            {
                sb.Append(" AND fFor = 'ACCOUNT' \n");
                if (objPropCustomer.Center == "1")
                {
                    sb.Append(" AND e.Job IS NULL ");
                }
            }

            if (objPropCustomer.EN == 1)
            {
                sb.Append(" AND UC.IsSel = 1 AND UC.UserID =" + objPropCustomer.UserID);
            }

            if (isConvertToJob)
            {
                sb.Append(" AND e.Job > 0 \n");
            }

            if (filters != null && filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0; string[] filterArrayValue;
                        var filterColumn = items.FilterColumn.ToLower();
                        StringBuilder filteredQuery = new StringBuilder();
                        if (filterColumn == "id")
                        {
                            filterArrayValue = items.FilterValue.ToString().Split(',');

                            // Reset start date, end date if filter by Id
                            objPropCustomer.StartDate = string.Empty;
                            objPropCustomer.EndDate = string.Empty;

                            foreach (var filtered in filterArrayValue)
                            {
                                if (int.TryParse(filtered, out FilterValue))
                                {
                                    if (filteredQuery.Length == 0)
                                    {
                                        filteredQuery.Append(filtered);
                                    }
                                    else
                                    {
                                        filteredQuery.Append("," + filtered);
                                    }
                                }


                            }

                            sb.Append(" AND e.ID IN (" + filteredQuery.ToString() + ")");
                        }
                        else if (filterColumn == "status")
                        {
                            sb.Append(" AND s.Name = '" + items.FilterValue + "'");
                        }
                        else if (filterColumn == "ffor")
                        {
                            if (items.FilterValue.Trim().ToLower() == "existing")
                            {
                                sb.Append(" AND fFor = 'ACCOUNT'");
                            }
                            else if (items.FilterValue.Trim().ToLower() == "lead")
                            {
                                sb.Append(" AND fFor <> 'ACCOUNT'");
                            }
                            else
                            {
                                sb.Append(" AND fFor = '" + items.FilterValue + "'");
                            }

                        }
                        else if (filterColumn == "template")
                        {
                            sb.Append(" AND e." + filterColumn + " = " + items.FilterValue);
                        }
                        else if (filterColumn == "opportunitystage")
                        {
                            sb.Append(" AND ls.Description LIKE '%" + items.FilterValue + "%'");
                        }
                        else if (filterColumn == "companyname" ||
                            filterColumn == "estimateaddress" ||
                            filterColumn == "opportunity" ||
                            filterColumn == "job" ||
                            filterColumn == "category" ||
                            filterColumn == "fdesc" ||
                            filterColumn == "contact"
                            )
                        {
                            sb.Append("   AND e." + filterColumn + " LIKE '%" + items.FilterValue + "%' \n");
                        }
                        else if (filterColumn == "assignto")
                        {
                            sb.Append("   AND ( t.SDesc LIKE '%" + items.FilterValue + "%' ) \n");
                        }
                        //else if (filterColumn == "contact")
                        //{
                        //    sb.Append("   AND ph.fDesc LIKE '%" + items.FilterValue + "%' \n");
                        //}
                        else if (filterColumn == "quotedprice")
                        {
                            double quoteValue;
                            if (double.TryParse(items.FilterValue, out quoteValue))
                            {
                                sb.Append("   AND e.Quoted = " + quoteValue + " \n");
                            }
                        }
                        else if (filterColumn == "estimateprice")
                        {
                            double priceValue;
                            if (double.TryParse(items.FilterValue, out priceValue))
                            {
                                sb.Append("   AND e.Price = " + priceValue + " \n");
                            }
                        }
                        else if (filterColumn == "discounted")
                        {
                            if (items.FilterValue.Trim().ToLower() == "yes")
                            {
                                sb.Append("   AND e.Discounted = 1 \n");
                            }
                            else if (items.FilterValue.Trim().ToLower() == "no")
                            {
                                sb.Append("   AND isnull(e.Discounted,0) = 0 \n");
                            }
                            else
                            {
                                sb.Append("   AND 1=0 \n");
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(objPropCustomer.SearchBy) && !string.IsNullOrEmpty(objPropCustomer.SearchValue))
            {
                if ((objPropCustomer.SearchBy).ToLower() == "e.id")
                {
                    int FilterValue = 0; string[] filterArrayValue;
                    filterArrayValue = objPropCustomer.SearchValue.Split(',');
                    StringBuilder filteredQuery = new StringBuilder();

                    // Reset start date, end date if filter by Id
                    objPropCustomer.StartDate = string.Empty;
                    objPropCustomer.EndDate = string.Empty;
                    foreach (var filtered in filterArrayValue)
                    {
                        if (int.TryParse(filtered, out FilterValue))
                        {
                            if (filteredQuery.Length == 0)
                            {
                                filteredQuery.Append(filtered);
                            }
                            else
                            {
                                filteredQuery.Append("," + filtered);
                            }
                        }
                    }
                    sb.Append(" AND e.ID  IN (" + filteredQuery.ToString() + ")");
                }

                if ((objPropCustomer.SearchBy).ToLower() == "e.status"
                    || (objPropCustomer.SearchBy).ToLower() == "e.ffor"
                    || (objPropCustomer.SearchBy).ToLower() == "e.template"
                    || (objPropCustomer.SearchBy).ToLower() == "l.opportunitystageid"
                    )
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " = '" + objPropCustomer.SearchValue + "'       \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.companyname")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.estimateaddress")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.opportunity")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.job")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.category")
                {
                    sb.Append("   AND " + objPropCustomer.SearchBy + " LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "em.ffirst")
                {
                    sb.Append("   AND ( t.SDesc LIKE '%" + objPropCustomer.SearchValue + "%' ) \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "e.contact")
                {
                    sb.Append("   AND e.contact LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if ((objPropCustomer.SearchBy).ToLower() == "customfield")
                {
                    sb.Append("   AND ISNULL(cl.IsDeleted, 0) = 0 AND cl.ID = '" + objPropCustomer.SearchValue + "' AND cv.Value LIKE '%" + objPropCustomer.SearchValueExt + "%'\n");
                }
            }

            if (objPropCustomer.Range == 1)
            {
                if (string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    sb.Append(" AND e.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    sb.Append(" AND e.fDate >= '" + objPropCustomer.StartDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    sb.Append(" AND e.fDate >= '" + objPropCustomer.StartDate + "' and e.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }
            }
            else if (objPropCustomer.Range == 2)
            {
                if (string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    sb.Append(" AND e.bdate <= '" + objPropCustomer.EndDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    sb.Append(" AND e.bdate >= '" + objPropCustomer.StartDate + "' \n");
                }
                else if (!string.IsNullOrEmpty(objPropCustomer.StartDate) && !string.IsNullOrEmpty(objPropCustomer.EndDate))
                {
                    sb.Append(" AND e.bdate >= '" + objPropCustomer.StartDate + "' and e.bdate <= '" + objPropCustomer.EndDate + "' \n");
                }
            }

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTaskCategories(Customer objPropCustomer)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Select ID, [Name],[Remarks], \n");
                sb.Append("	( \n");
                sb.Append(" (select Count(Keyword) from todo where Keyword = tblTaskCategory.Name) + \n");
                sb.Append(" (select  count(Keyword) from done  where Keyword = tblTaskCategory.Name) \n");
                sb.Append("	) Count \n");
                sb.Append("	From tblTaskCategory \n");
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CRUDTaskCategory(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spCRUDTaskCategory", objPropCustomer.Mode, objPropCustomer.TaskCategory.ID, objPropCustomer.TaskCategory.Name
                    , objPropCustomer.TaskCategory.Remarks, objPropCustomer.TaskCategory.CreatedBy, objPropCustomer.TaskCategory.CreatedDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EditWIPStatus(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter();
                para[0].ParameterName = "JobId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.job;

                para[1] = new SqlParameter();
                para[1].ParameterName = "WIPId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objProp_Customer.WIPID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "WIPStatus";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objProp_Customer.WIPStatus;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Username";
                para[3].SqlDbType = SqlDbType.NVarChar;
                para[3].Value = objProp_Customer.Username;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spEditWIPStatus", para));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveWIPNew(Customer objProp_Customer)
        {
            try
            {
                DataTable dt = objProp_Customer.WIP.Tables[0];
                DataTable dt2 = objProp_Customer.WIP.Tables[1];
                SqlParameter[] para = new SqlParameter[11];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Id";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = dt.Rows[0]["Id"];

                para[1] = new SqlParameter();
                para[1].ParameterName = "JobId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = dt.Rows[0]["JobId"];

                para[2] = new SqlParameter();
                para[2].ParameterName = "ProgressBillingNo";
                para[2].SqlDbType = SqlDbType.NVarChar;
                para[2].Value = dt.Rows[0]["ProgressBillingNo"].ToString();

                para[3] = new SqlParameter();
                para[3].ParameterName = "InvoiceId";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = (dt.Rows[0]["InvoiceId"] == null) ? DBNull.Value : dt.Rows[0]["InvoiceId"];

                para[4] = new SqlParameter();
                para[4].ParameterName = "BillingDate";
                para[4].SqlDbType = SqlDbType.DateTime;
                para[4].Value = dt.Rows[0]["BillingDate"];

                para[5] = new SqlParameter();
                para[5].ParameterName = "ApplicationStatusId";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = dt.Rows[0]["ApplicationStatusId"];

                para[6] = new SqlParameter();
                para[6].ParameterName = "Terms";
                para[6].SqlDbType = SqlDbType.Int;
                para[6].Value = dt.Rows[0]["Terms"];

                para[7] = new SqlParameter();
                para[7].ParameterName = "SalesTax";
                para[7].SqlDbType = SqlDbType.Decimal;
                para[7].Value = dt.Rows[0]["SalesTax"];

                para[8] = new SqlParameter();
                para[8].ParameterName = "PeriodDate";
                para[8].SqlDbType = SqlDbType.DateTime;
                para[8].Value = dt.Rows[0]["PeriodDate"];

                para[9] = new SqlParameter();
                para[9].ParameterName = "RevisionDate";
                para[9].SqlDbType = SqlDbType.DateTime;
                para[9].Value = dt.Rows[0]["RevisionDate"];

                para[10] = new SqlParameter();
                para[10].ParameterName = "Remarks";
                para[10].SqlDbType = SqlDbType.NVarChar;
                para[10].Value = dt.Rows[0]["Remarks"];

                int WIPId = 0;

                try
                {
                    WIPId = Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spAddWIPHeader", para));
                    if (WIPId > 0 && dt2.Rows.Count > 0)
                    {
                        SqlParameter[] para2 = new SqlParameter[3];

                        para2[0] = new SqlParameter();
                        para2[0].ParameterName = "JobId";
                        para2[0].SqlDbType = SqlDbType.Int;
                        para2[0].Value = dt.Rows[0]["JobId"];

                        para2[1] = new SqlParameter();
                        para2[1].ParameterName = "WIPId";
                        para2[1].SqlDbType = SqlDbType.Int;
                        para2[1].Value = WIPId;

                        para2[2] = new SqlParameter();
                        para2[2].ParameterName = "WIPDetails";
                        para2[2].SqlDbType = SqlDbType.Structured;
                        para2[2].Value = dt2;

                        WIPId = Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spCreateWIPDetails", para2));
                    }
                    return WIPId;
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

        public void AddUpdateContact(PhoneModel objPhone)
        {
            var para = new SqlParameter[8];

            para[0] = new SqlParameter
            {
                ParameterName = "@RolID",
                SqlDbType = SqlDbType.Int,
                Value = objPhone.Rol
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Phone
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Fax
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Cell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Cell
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Email
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Title",
                SqlDbType = SqlDbType.VarChar,
                Value = objPhone.Title
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = objPhone.ID
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPhone.ConnConfig, CommandType.StoredProcedure, "spAddUpdateContact", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteContact(PhoneModel objPhone)
        {
            var para = new SqlParameter[1];
            para[0] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = objPhone.ID
            };
            try
            {

                SqlHelper.ExecuteNonQuery(objPhone.ConnConfig, CommandType.StoredProcedure, "spDeleteContact", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateByLoc(string conn, int loc)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@loc",
                SqlDbType = SqlDbType.Int,
                Value = loc
            };

            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetEstimateByLoc", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void LinkEstimateToProject(string conn, int estimateId, int job, int oppId, string updatedBy)
        {
            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@EstimateId",
                SqlDbType = SqlDbType.Int,
                Value = estimateId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Job",
                SqlDbType = SqlDbType.VarChar,
                Value = job
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@OppId",
                SqlDbType = SqlDbType.Int,
                Value = oppId
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = updatedBy
            };


            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "spLinkEstimateToProject", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllEstimateLinkToProject(string conn, int job)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT * FROM  Estimate where Job=" + job.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllProjectByLoc(string conn, int loc)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT * FROM  Job where Loc=" + loc.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EditAmountWIPInvoice(Customer objProp_Customer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "WIPId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objProp_Customer.WIPID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Username";
                para[1].SqlDbType = SqlDbType.NVarChar;
                para[1].Value = objProp_Customer.Username;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spEditAmountWIPInvoice", para));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateByProject(string conn, int job)
        {

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT Distinct e.ID, \n");
            varname1.Append("       e.NAME, \n");
            varname1.Append("       e.fdesc, \n");
            varname1.Append("       e.fDate, \n");
            varname1.Append("       e.Opportunity, \n");
            varname1.Append("       ls.Description As OpportunityStage, \n");
            varname1.Append("       l.OpportunityStageID,  \n");
            varname1.Append("       e.Category, \n");
            varname1.Append("       e.EstimateAddress, \n");
            varname1.Append("       e.remarks, \n");
            varname1.Append("       e.job, \n");
            varname1.Append("       e.CompanyName, \n");
            varname1.Append("       t.SDesc as AssignTo, \n");
            varname1.Append("       e.Contact, \n");
            varname1.Append("       ffor, \n");
            varname1.Append("       s.Name as [Status], \n");
            varname1.Append("       r.EN, \n");
            varname1.Append("       ISNULL(B.Name, '') As Company, \n");
            varname1.Append("       ISNULL(e.Price,0) As EstimatePrice,   \n");
            //varname1.Append("       ISNULL(e.Quoted,0) As QuotedPrice,     \n");
            varname1.Append("       CASE WHEN ISNULL(e.Quoted,0) = 0 THEN ISNULL(e.Price,0) ELSE e.Quoted END As QuotedPrice,     \n");
            varname1.Append("       CASE ISNULL(e.Discounted,0) WHEN 0 THEN 'No' ELSE 'Yes' END As Discounted     \n");
            varname1.Append(" FROM  Estimate e  \n");
            varname1.Append("       LEFT OUTER JOIN OEStatus s ON e.[Status]= s.ID \n");
            varname1.Append("       LEFT OUTER JOIN terr t ON E.EstimateUserId=t.ID \n");
            varname1.Append("       LEFT OUTER JOIN Rol r on e.RolID = r.ID \n");
            varname1.Append("       LEFT OUTER JOIN Branch B on B.ID = r.EN  \n");
            varname1.Append("       LEFT OUTER JOIN Lead l on l.ID = e.Opportunity \n");
            varname1.Append("       LEFT JOIN  Stage ls on ls.ID = l.OpportunityStageID \n");
            varname1.Append("       left join tblCommonCustomFieldsValue cv on cv.Ref = e.ID and cv.Screen = 'Estimate' \n");
            varname1.Append("       left join tblCommonCustomFields cl on cl.ID = cv.tblCommonCustomFieldsID \n");
            varname1.Append(" WHERE  EstTemplate = 0 and e.Job=" + job);


            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecentCollectionNotes(Customer objProp_Customer)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT *  \n");
            sb.Append("FROM ( \n");
            sb.Append("	SELECT  \n");
            sb.Append("		*, \n");
            sb.Append("		ROW_NUMBER() OVER(PARTITION BY LocID ORDER BY CreatedDate DESC) AS rn \n");
            sb.Append("	FROM CollectionNotes \n");
            sb.Append("	WHERE LocID IS NOT NULL AND LocID <> 0 \n");
            sb.Append(") AS t \n");
            sb.Append("WHERE t.rn = 1 \n");
            sb.Append("SELECT *  \n");
            sb.Append("FROM ( \n");
            sb.Append("	SELECT  \n");
            sb.Append("		*, \n");
            sb.Append("		ROW_NUMBER() OVER(PARTITION BY OwnerID ORDER BY CreatedDate DESC) AS rn \n");
            sb.Append("	FROM CollectionNotes \n");
            sb.Append("	WHERE LocID IS NULL OR LocID = 0 \n");
            sb.Append(") AS t \n");
            sb.Append("WHERE t.rn = 1 \n");

            try
            {
                return SqlHelper.ExecuteDataset(objProp_Customer.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteCollectionNotes(String conn, int noteId)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "noteId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = noteId;

                return Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "spDeleteCollectionNote", para));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCollectionNotes(string conn, int noteId, String note, string updatedBy)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "ID",
                SqlDbType = SqlDbType.Int,
                Value = noteId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Notes",
                SqlDbType = SqlDbType.VarChar,
                Value = note
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = updatedBy
            };

            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "spUpdateCollectionNotes", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBOMItemsByVendor(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetBOMItemsByVendor", objPropCustomer.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectsWithWorkflows(Customer objPropCustomer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "Job";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objPropCustomer.dtCustom;

                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectsWithWorkflowsNew", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EstimateConversionUndo(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spEstimateConversionUndo", objPropCustomer.estimateno, objPropCustomer.job, objPropCustomer.Username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet LinkEstimateToProjectNew(string conn, int estimateId, int job, string updatedBy)
        {
            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@EstimateId",
                SqlDbType = SqlDbType.Int,
                Value = estimateId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Job",
                SqlDbType = SqlDbType.VarChar,
                Value = job
            };
            // We are setting OppID = 0 because we do this function from Estimate screen
            // We don't need to check update estimate location
            para[2] = new SqlParameter
            {
                ParameterName = "@OppId",
                SqlDbType = SqlDbType.Int,
                Value = 0
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = updatedBy
            };


            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spLinkEstimateToProject", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void UpdateStageProjectHeader(Customer objCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, CommandType.Text, "Update tblProjectStage Set Label='" + objCustomer.HeaderStage + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllProjectStages(string connStr)
        {
            try
            {
                var strdb = new StringBuilder();

                strdb.AppendLine(" SELECT");
                strdb.AppendLine(" ps.ID");
                strdb.AppendLine(" , ps.Description");
                strdb.AppendLine(" , ps.Label");
                strdb.AppendLine(" , ps.ChartColors");
                //strdb.AppendLine(" , (SELECT COUNT(*) FROM Job WHERE Job.Stage is not null AND Job.Stage = ps.ID) Count");
                //strdb.AppendLine(" , jt.Type Department");
                //strdb.AppendLine(" FROM tblProjectStage ps LEFT JOIN JobType jt ON jt.StageID = ps.ID");
                strdb.AppendLine(" , (SELECT COUNT(*) FROM Job WHERE Job.Stage is not null AND Job.Stage in (SELECT tblProjectStageItem.ID FROM tblProjectStageItem WHERE tblProjectStageItem.StageID = ps.ID)) Count");
                strdb.AppendLine(" , (SELECT STUFF((SELECT ', ' + CAST(e.Type AS VARCHAR(1000)) FROM JobType e WHERE e.StageID = ps.ID FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) Department");
                strdb.AppendLine(" FROM tblProjectStage ps");
                return SqlHelper.ExecuteDataset(connStr, CommandType.Text, strdb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetProjectStageHeader(Customer objCustomer)
        {
            string getValue = string.Empty;
            try
            {
                getValue = Convert.ToString(SqlHelper.ExecuteScalar(objCustomer.ConnConfig, CommandType.Text, "SELECT TOP 1 Label FROM tblProjectStage"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return getValue;
        }

        public void UpdateProjectStage(Customer objPropCustomer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropCustomer.Stage.ID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Stage.Description
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "@Label",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Stage.Label
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "@ChartColor",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objPropCustomer.Stage.ChartColor
                };

                para[4] = new SqlParameter
                {
                    ParameterName = "@DepartmentIDs",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropCustomer.Stage.DepartmentIDs
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@StageItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.Stage.Items
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@StageItemsDelete",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.Stage.DeleteItems
                };
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spUpdateProjectStage", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProjectStage(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spDeleteProjectStage", objPropCustomer.Stage.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetStagesByDepartment(string connStr, string depId)
        {
            try
            {
                var strdb = new StringBuilder();

                //Select sti.* from tblProjectStageItem sti
                //inner
                //            join tblProjectStage st on st.ID = sti.StageID
                //      inner
                //            join JobType jt on jt.StageID = st.ID
                //where jt.ID = 2

                strdb.AppendLine(" Select sti.ID, sti.Label Stage, st.Description Label from tblProjectStageItem sti");
                strdb.AppendLine(" inner join tblProjectStage st on st.ID = sti.StageID");
                strdb.AppendLine(" inner join JobType jt on jt.StageID = st.ID");
                strdb.AppendFormat(" where jt.ID = {0}", depId);
                strdb.AppendLine(" order by sti.OrderNo, sti.Label");
                return SqlHelper.ExecuteDataset(connStr, CommandType.Text, strdb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectStageItemByID(string connStr, string stageItemId)
        {
            try
            {
                var strdb = new StringBuilder();

                //Select sti.* from tblProjectStageItem sti
                //inner
                //            join tblProjectStage st on st.ID = sti.StageID
                //      inner
                //            join JobType jt on jt.StageID = st.ID
                //where jt.ID = 2

                strdb.AppendLine(" select * from tblProjectStageItem ");
                strdb.AppendFormat(" where ID = {0}", stageItemId);
                return SqlHelper.ExecuteDataset(connStr, CommandType.Text, strdb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractByLoc(string connStr, int loc)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@Loc",
                SqlDbType = SqlDbType.Int,
                Value = loc
            };

            try
            {
                return SqlHelper.ExecuteDataset(connStr, CommandType.StoredProcedure, "spGetContractByLoc", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveReverseWip(Customer objProp_Customer)
        {
            try
            {
                DataTable dt = objProp_Customer.Reverse.Tables[0];
                SqlParameter[] para = new SqlParameter[10];

                para[0] = new SqlParameter();
                para[0].ParameterName = "listWipId";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = dt.Rows[0]["WIPs"];

                para[1] = new SqlParameter();
                para[1].ParameterName = "JobId";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = dt.Rows[0]["JobId"];

                para[2] = new SqlParameter();
                para[2].ParameterName = "ProgressBillingNo";
                para[2].SqlDbType = SqlDbType.NVarChar;
                para[2].Value = dt.Rows[0]["ProgressBillingNo"].ToString();

                para[3] = new SqlParameter();
                para[3].ParameterName = "BillingDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = dt.Rows[0]["BillingDate"];

                para[4] = new SqlParameter();
                para[4].ParameterName = "ApplicationStatusId";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = dt.Rows[0]["ApplicationStatusId"];

                para[5] = new SqlParameter();
                para[5].ParameterName = "Terms";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = dt.Rows[0]["Terms"];

                para[6] = new SqlParameter();
                para[6].ParameterName = "SalesTax";
                para[6].SqlDbType = SqlDbType.Decimal;
                para[6].Value = dt.Rows[0]["SalesTax"];

                para[7] = new SqlParameter();
                para[7].ParameterName = "PeriodDate";
                para[7].SqlDbType = SqlDbType.DateTime;
                para[7].Value = dt.Rows[0]["PeriodDate"];

                para[8] = new SqlParameter();
                para[8].ParameterName = "RevisionDate";
                para[8].SqlDbType = SqlDbType.DateTime;
                para[8].Value = dt.Rows[0]["RevisionDate"];

                para[9] = new SqlParameter();
                para[9].ParameterName = "Username";
                para[9].SqlDbType = SqlDbType.VarChar;
                para[9].Value = dt.Rows[0]["UserName"];

                try
                {
                    SqlHelper.ExecuteScalar(objProp_Customer.ConnConfig, CommandType.StoredProcedure, "spReverseWip", para);
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

        public DataSet GetEstimateApprovedStatusHistory(Customer objCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objCustomer.ConnConfig, "spGetEstimateApprovedStatusHistory", objCustomer.estimateno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEstimateApprovalStatus(Customer objCustomer)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "EstimateId";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objCustomer.estimateno;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Status";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objCustomer.Status;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Comment";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = objCustomer.Notes;

                para[3] = new SqlParameter();
                para[3].ParameterName = "UpdatedBy";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = objCustomer.Username;

                para[4] = new SqlParameter();
                para[4].ParameterName = "UpdatedDate";
                para[4].SqlDbType = SqlDbType.DateTime;
                para[4].Value = objCustomer.date;

                SqlHelper.ExecuteNonQuery(objCustomer.ConnConfig, "spUpdateEstimateApprovalStatus", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool CanRevertLocCustOfOppAndEstOnUnlinkProject(Customer objPropCustomer)
        //{
        //    try
        //    {
        //        var retVal = (bool)SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spCheckToAllowRevertChangesOnUnLinkProject", objPropCustomer.estimateno, objPropCustomer.job);
        //        return retVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}

