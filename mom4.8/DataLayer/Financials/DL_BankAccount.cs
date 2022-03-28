using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer
{
    public class DL_BankAccount
    {
        public int AddRol(Rol objRol)
        {
            var para = new SqlParameter[22];

            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Fax
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Contact
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Address
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.EMail
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Website
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Country
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Cellular
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@Type",
                SqlDbType = SqlDbType.SmallInt,
                Value = objRol.Type
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@GeoLock",
                SqlDbType = SqlDbType.Int,
                Value = objRol.GeoLock
            };
            if (objRol.Since != DateTime.MinValue)
            {
                para[18] = new SqlParameter
                {
                    ParameterName = "@Since",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objRol.Since
                };
            }
            if (objRol.Last != DateTime.MinValue)
            {
                para[19] = new SqlParameter
                {
                    ParameterName = "@Last",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objRol.Last
                };
            }
            para[20] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            para[21] = new SqlParameter
            {
                ParameterName = "EN",
                SqlDbType = SqlDbType.Int,
                Value = objRol.EN
            };


            try
            {
                SqlHelper.ExecuteNonQuery(objRol.ConnConfig, CommandType.StoredProcedure, "spAddRolDetails", para);
                return Convert.ToInt32(para[20].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddRol(AddRolParam _AddRolParam, string ConnectionString)
        {
            var para = new SqlParameter[22];

            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Zip",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Fax
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Contact
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Address
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.EMail
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Website
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Country
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = _AddRolParam.Cellular
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@Type",
                SqlDbType = SqlDbType.SmallInt,
                Value = _AddRolParam.Type
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@GeoLock",
                SqlDbType = SqlDbType.Int,
                Value = _AddRolParam.GeoLock
            };
            if (_AddRolParam.Since != DateTime.MinValue)
            {
                para[18] = new SqlParameter
                {
                    ParameterName = "@Since",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddRolParam.Since
                };
            }
            if (_AddRolParam.Last != DateTime.MinValue)
            {
                para[19] = new SqlParameter
                {
                    ParameterName = "@Last",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _AddRolParam.Last
                };
            }
            para[20] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            para[21] = new SqlParameter
            {
                ParameterName = "EN",
                SqlDbType = SqlDbType.Int,
                Value = _AddRolParam.EN
            };


            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddRolDetails", para);
                return Convert.ToInt32(para[20].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddBank(Bank objBank)
        {
            var para = new SqlParameter[17];

            para[1] = new SqlParameter
            {
                ParameterName = "@fDesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.fDesc
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Rol",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Rol
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@NBranch",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NBranch
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@NAcct",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NAcct
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@NRoute",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NRoute
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@NextC",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextC
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@NextD",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextD
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@NextE",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextE
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Rate",
                SqlDbType = SqlDbType.Float,
                Value = objBank.Rate
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@CLimit",
                SqlDbType = SqlDbType.Float,
                Value = objBank.CLimit
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Warn",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Warn
            };

            para[14] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Status
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@Chart",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Chart
            };
            try
            {
                SqlHelper.ExecuteNonQuery(objBank.ConnConfig, CommandType.StoredProcedure, "spAddBankDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStates(State _objState)
        {
            try
            {
                return _objState.DsState = SqlHelper.ExecuteDataset(_objState.ConnConfig, CommandType.Text, "SELECT Name, fDesc, Country,VertexGeocode FROM State");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetStates(GetStatesParam _GetStatesParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Name, fDesc, Country FROM State");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRoles(Rol objRol)
        {
            try
            {
                //return _objState.DsState = SqlHelper.ExecuteDataset(_objState.ConnConfig, CommandType.Text, "SELECT Name, fDesc, Country FROM State");
                string query = "UPDATE Rol SET Remarks = @Remarks WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objRol.ID));
                parameters.Add(new SqlParameter("@Remarks", objRol.Remarks));
                int rowsAffected = SqlHelper.ExecuteNonQuery(objRol.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRoles(UpdateRolesParam _UpdateRolesParam, string ConnectionString)
        {
            try
            {
                //return _objState.DsState = SqlHelper.ExecuteDataset(_objState.ConnConfig, CommandType.Text, "SELECT Name, fDesc, Country FROM State");
                string query = "UPDATE Rol SET Remarks = @Remarks WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _UpdateRolesParam.ID));
                parameters.Add(new SqlParameter("@Remarks", _UpdateRolesParam.Remarks));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRol(Rol objRol)
        {
            var para = new SqlParameter[16];

            para[0] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = objRol.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Fax
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Contact
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Address
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.EMail
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Website
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Country
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Cellular
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@Type",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Type
            };
            para[14] = new SqlParameter
            {
                ParameterName = "EN",
                SqlDbType = SqlDbType.Int,
                Value = objRol.EN
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.MOMUSer
            };
            //para[16] = new SqlParameter
            //{
            //    ParameterName = "@Remarks",
            //    SqlDbType = SqlDbType.VarChar,
            //    Value = objRol.Remarks
            //};

            //para[17] = new SqlParameter
            //{
            //    ParameterName = "@GeoLock",
            //    SqlDbType = SqlDbType.Int,
            //    Value = objRol.GeoLock
            //};
            try
            {
                SqlHelper.ExecuteDataset(objRol.ConnConfig, "spUpdateRolDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRol(UpdateRolParam _UpdateRolParam, string ConnectionString)
        {
            var para = new SqlParameter[16];

            para[0] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = _UpdateRolParam.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Zip",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Fax
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Contact
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Address
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.EMail
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Website
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Country
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Cellular
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@Type",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.Type
            };
            para[14] = new SqlParameter
            {
                ParameterName = "EN",
                SqlDbType = SqlDbType.Int,
                Value = _UpdateRolParam.EN
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@UpdatedBy",
                SqlDbType = SqlDbType.VarChar,
                Value = _UpdateRolParam.MOMUSer
            };
            //para[16] = new SqlParameter
            //{
            //    ParameterName = "@Remarks",
            //    SqlDbType = SqlDbType.VarChar,
            //    Value = objRol.Remarks
            //};

            //para[17] = new SqlParameter
            //{
            //    ParameterName = "@GeoLock",
            //    SqlDbType = SqlDbType.Int,
            //    Value = objRol.GeoLock
            //};
            try
            {
                SqlHelper.ExecuteDataset(ConnectionString, "spUpdateRolDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBank(Bank objBank)
        {
            var para = new SqlParameter[13];

            para[0] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = objBank.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@fDesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.fDesc
            };
            //para[2] = new SqlParameter
            //{
            //    ParameterName = "@Rol",
            //    SqlDbType = SqlDbType.Int,
            //    Value = objBank.Rol
            //};
            para[2] = new SqlParameter
            {
                ParameterName = "@NBranch",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NBranch
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@NAcct",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NAcct
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@NRoute",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NRoute
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@NextC",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextC
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@NextD",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextD
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@NextE",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextE
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Rate",
                SqlDbType = SqlDbType.Float,
                Value = objBank.Rate
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@CLimit",
                SqlDbType = SqlDbType.Float,
                Value = objBank.CLimit
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Warn",
                SqlDbType = SqlDbType.TinyInt,
                Value = objBank.Warn
            };

            para[11] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Status
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Rol",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Rol
            };
            //para[12] = new SqlParameter
            //{
            //    ParameterName = "@Chart",
            //    SqlDbType = SqlDbType.Int,
            //    Value = objBank.Chart
            //};

            try
            {
                SqlHelper.ExecuteNonQuery(objBank.ConnConfig, CommandType.StoredProcedure, "spUpdateBankDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankByChart(Bank objBank)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@ChartId",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.Chart
                };
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.StoredProcedure, "spGetBankByChart", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRolByID(Rol objRol)
        {
            try
            {
                return objRol.DsRol = SqlHelper.ExecuteDataset(objRol.ConnConfig, CommandType.Text, "SELECT ID,Name,City,State,Zip,Phone,Fax,Contact,Address,EMail,Website,Cellular,Country,Lat,Lng FROM Rol WHERE ID='" + objRol.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistBankAcct(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT COUNT(*) AS CBANK FROM Bank WHERE Chart='" + objBank.Chart + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet spGetACHForCheck(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT p.ID,p.Ref,p.EmpID,p.Net,e.fFirst+' '+e.Last AS EmpName,e.ACH,e.ACHBank,e.ACHBank2,e.ACHRoute,e.ACHRoute2,e.ACHType,e.ACHType2 FROM PRReg p INNER JOIN Emp e ON e.ID = p.EmpID WHERE p.Ref >= " + objBank.CheckNoFrom + " AND p.Ref <=" + objBank.CheckNoTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet spGetBankACH(Bank objBank)
        {
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@Bankid",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ID
                };

                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.StoredProcedure, "spGetBankACH", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllBankNames(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT Bank.ID, Bank.fDesc from Bank INNER Join Chart ON Bank.Chart = Chart.ID where Bank.Status = 0 order by Bank.fDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllBankNames(GetAllBankNamesParam objGetAllBankNamesParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Bank.ID, Bank.fDesc from Bank INNER Join Chart ON Bank.Chart = Chart.ID where Bank.Status = 0 order by Bank.fDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllBankNamesByCompany(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT bk.ID, bk.fDesc,Rol.EN,b.Name As Company FROM Bank bk left join Rol  on Rol.ID=bk.Rol left Outer join Branch B on B.ID = Rol.EN Where bk.Status = 0 and  Rol.EN=" + objBank.EN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllBankNamesByCompany(GetAllBankNamesByCompanyParam _GetAllBankNamesByCompanyParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT bk.ID, bk.fDesc,Rol.EN,b.Name As Company FROM Bank bk left join Rol  on Rol.ID=bk.Rol left Outer join Branch B on B.ID = Rol.EN Where bk.Status = 0 and  Rol.EN=" + _GetAllBankNamesByCompanyParam.EN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankByID(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT b.[ID],b.[fDesc],b.[Rol],b.[NBranch],b.[NAcct],b.[NRoute],b.[NextC],b.[NextD],b.[NextE],b.[Rate],b.[CLimit],b.[Warn],b.[Recon],b.[Balance],b.[Status],b.[InUse],b.[ACHFileHeaderStringA],b.[ACHFileHeaderStringB],b.[ACHFileHeaderStringC],b.[ACHCompanyHeaderString1],b.[ACHCompanyHeaderString2],b.[ACHBatchControlString1],b.[ACHBatchControlString2],b.[ACHBatchControlString3],b.[ACHFileControlString1],b.[APACHCompanyID],b.[APImmediateOrigin],b.[BankType],b.[ChartID],b.[Chart],b.[LastReconDate],ISNULL(NextCash,100000000) as NextCash,ISNULL(NextWire,200000000) AS NextWire,ISNULL(NextACH,300000000) AS NextACH,ISNULL(NextCC,400000000) AS NextCC, c.Balance AS BankBalance FROM Bank b INNER JOIN Chart c ON b.Chart = c.ID WHERE b.ID = " + objBank.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBankByID(GetBankByIDParam _GetBankByIDParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT b.[ID],b.[fDesc],b.[Rol],b.[NBranch],b.[NAcct],b.[NRoute],b.[NextC],b.[NextD],b.[NextE],b.[Rate],b.[CLimit],b.[Warn],b.[Recon],b.[Balance],b.[Status],b.[InUse],b.[ACHFileHeaderStringA],b.[ACHFileHeaderStringB],b.[ACHFileHeaderStringC],b.[ACHCompanyHeaderString1],b.[ACHCompanyHeaderString2],b.[ACHBatchControlString1],b.[ACHBatchControlString2],b.[ACHBatchControlString3],b.[ACHFileControlString1],b.[APACHCompanyID],b.[APImmediateOrigin],b.[BankType],b.[ChartID],b.[Chart],b.[LastReconDate],ISNULL(NextCash,100000000) as NextCash,ISNULL(NextWire,200000000) AS NextWire,ISNULL(NextACH,300000000) AS NextACH,ISNULL(NextCC,400000000) AS NextCC, c.Balance AS BankBalance FROM Bank b INNER JOIN Chart c ON b.Chart = c.ID WHERE b.ID = " + _GetBankByIDParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankBalance(Bank _objBank)
        {
            try
            {
                string query = "UPDATE Bank SET Balance = @Balance WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objBank.ID));
                parameters.Add(new SqlParameter("@Balance", _objBank.Balance));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objBank.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateNextCheck(Bank _objBank)
        {
            try
            {
                string query = "UPDATE Bank SET NextC = @NextC WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objBank.ID));
                parameters.Add(new SqlParameter("@NextC", _objBank.NextC));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objBank.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateNextCheck(UpdateNextCheckParam _UpdateNextCheckParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Bank SET NextC = @NextC WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _UpdateNextCheckParam.ID));
                parameters.Add(new SqlParameter("@NextC", _UpdateNextCheckParam.NextC));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankBalanceNcheck(Bank _objBank)
        {
            try
            {
                string query = "UPDATE Bank SET Balance = @Balance, NextC = @NextC WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objBank.ID));
                parameters.Add(new SqlParameter("@NextC", _objBank.NextC));
                parameters.Add(new SqlParameter("@Balance", _objBank.Balance));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objBank.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankRecon(Bank _objBank)
        {
            try
            {
                string query = "UPDATE Bank SET Recon = @Recon, LastReconDate=@LastReconDate WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objBank.ID));
                parameters.Add(new SqlParameter("@Recon", _objBank.Recon));
                parameters.Add(new SqlParameter("@LastReconDate", _objBank.LastReconDate));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objBank.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetChartByBank(Bank _objBank)
        {
            try
            {
                return _objBank.Chart = Convert.ToInt32(SqlHelper.ExecuteScalar(_objBank.ConnConfig, CommandType.Text, "SELECT Isnull(Chart,0) FROM Bank WHERE ID=" + _objBank.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetBankIDByChart(Bank _objBank)
        {
            try
            {
                return _objBank.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(_objBank.ConnConfig, CommandType.Text, "SELECT Isnull(ID,0) FROM Bank WHERE Chart=" + _objBank.Chart));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetBankRolByID(Bank objBank)
        //{
        //    try
        //    {
        //        return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT b.ID,b.fDesc,b.Rol,b.NBranch,b.NAcct,b.NRoute,b.NextC,b.NextD,b.NextE,b.Rate,b.CLimit,b.Warn,b.Recon,b.Balance,b.Status,b.InUse,b.Chart,r. FROM Bank as b, Rol as r WHERE ID=" + objBank.ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetBankRolByID(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT b.ID,b.fDesc,b.Rol,b.NBranch,b.NAcct,b.NRoute,b.NextC,b.NextD,b.NextE,b.Rate,b.CLimit,b.Warn,b.Recon,b.Balance,b.Status,b.InUse,b.Chart, r.Name as BankName FROM Bank as b, Rol as r WHERE b.Rol=r.ID and b.ID=" + objBank.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteRolByID(Rol objRol)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objRol.ConnConfig, CommandType.Text, " DELETE FROM Rol WHERE ID = " + objRol.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteRolByID(DeleteRolByIDParam _DeleteRolByIDParam, string connectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, " DELETE FROM Rol WHERE ID = " + _DeleteRolByIDParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BankRecon(Bank objBank)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "@bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@endbalance",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.Balance
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@ReconDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objBank.LastReconDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@ServiceChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.ServiceCharge
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@ServiceAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ServiceAcct
                };
                if (objBank.ServiceDate != System.DateTime.MinValue)
                {
                    para[5] = new SqlParameter
                    {
                        ParameterName = "@ServiceDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.ServiceDate
                    };
                }
                para[6] = new SqlParameter
                {
                    ParameterName = "@InterestChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.InterestCharge
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@InterestAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.InterestAcct
                };
                if (objBank.InterestDate != System.DateTime.MinValue)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "@InterestDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.InterestDate
                    };
                }
                para[9] = new SqlParameter
                {
                    ParameterName = "@BankRecon",
                    SqlDbType = SqlDbType.Structured,
                    Value = objBank.DtBank
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@StatementDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objBank.fDate
                };

                SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.StoredProcedure, "spBankRecon", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Return BankReconID For Cleared Item
        //Created by Prateek 17-03-2021
        public int BankReconId(Bank objBank)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "@bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@endbalance",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.Balance
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@ReconDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objBank.LastReconDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@ServiceChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.ServiceCharge
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@ServiceAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ServiceAcct
                };
                if (objBank.ServiceDate != System.DateTime.MinValue)
                {
                    para[5] = new SqlParameter
                    {
                        ParameterName = "@ServiceDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.ServiceDate
                    };
                }
                para[6] = new SqlParameter
                {
                    ParameterName = "@InterestChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.InterestCharge
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@InterestAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.InterestAcct
                };
                if (objBank.InterestDate != System.DateTime.MinValue)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "@InterestDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.InterestDate
                    };
                }
                para[9] = new SqlParameter
                {
                    ParameterName = "@BankRecon",
                    SqlDbType = SqlDbType.Structured,
                    Value = objBank.DtBank
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "@StatementDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objBank.fDate
                };

                try
                {
                    int Id = Convert.ToInt32(SqlHelper.ExecuteScalar(objBank.ConnConfig, CommandType.StoredProcedure, "spBankRecon", para));
                    return Id;
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void StoreBankRecon(Bank objBank)
        {
            try
            {
                var para = new SqlParameter[10];

                para[0] = new SqlParameter
                {
                    ParameterName = "@bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@endbalance",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.Balance
                };
                if (objBank.LastReconDate != System.DateTime.MinValue)
                {
                    para[2] = new SqlParameter
                    {
                        ParameterName = "@ReconDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.LastReconDate
                    };
                }
                para[3] = new SqlParameter
                {
                    ParameterName = "@ServiceChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.ServiceCharge
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@ServiceAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ServiceAcct
                };
                if (objBank.ServiceDate != System.DateTime.MinValue)
                {
                    para[5] = new SqlParameter
                    {
                        ParameterName = "@ServiceDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.ServiceDate
                    };
                }
                para[6] = new SqlParameter
                {
                    ParameterName = "@InterestChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.InterestCharge
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@InterestAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.InterestAcct
                };
                if (objBank.InterestDate != System.DateTime.MinValue)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "@InterestDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.InterestDate
                    };
                }
                if (objBank.DtBank != null)
                {
                    if (objBank.DtBank.Rows.Count > 0)
                    {
                        para[9] = new SqlParameter
                        {
                            ParameterName = "@BankRecon",
                            SqlDbType = SqlDbType.Structured,
                            Value = objBank.DtBank
                        };
                    }
                }

                SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.StoredProcedure, "spStoreBankRecon", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStoredBankRecon(Bank objBank)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append(" \n");
                varname.Append(" SELECT isnull(Bank,0) as Bank, SCDate, IntDate, isnull(SCAmount,0) as SCAmount,            \n");
                varname.Append("    isnull(IntAmount,0) as IntAmount, isnull(EndBalance,0) as EndBalance, StatementDate,    \n");
                varname.Append("    (select top 1 isnull(Label,'') from custom where Name = 'ReconInt') as IntGL,                 \n");
                varname.Append("    (select  top 1  isnull(Label,'') from custom where Name = 'ReconSC') as SCGL,                   \n");
                varname.Append("    (select fdesc from chart where id = (select  top 1 label from custom where Name = 'ReconInt')) as IntGLName,   \n");
                varname.Append("    (select fdesc from chart where id = (select  top 1 label from custom where Name = 'ReconSC')) as SCGLName      \n");
                varname.Append("        FROM Control           \n");
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankDetailByDate(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT b.ID,b.fDesc,b.Rol,b.NBranch,b.NAcct,b.NRoute,b.NextC,b.NextD,b.NextE,b.Rate,b.CLimit,b.Warn,b.Recon,b.Status,b.InUse,b.Chart, r.Name as BankName,(SELECT Sum(ISNULL(Amount,0)) FROM trans WHERE Acct = b.Chart  AND fDate <='" + objBank.fDate + "') AS Balance ,CAST('" + objBank.fDate + "' as date)  AS StatementDate FROM Bank as b LEFT JOIN Rol as r ON b.Rol=r.ID WHERE b.ID=" + objBank.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
