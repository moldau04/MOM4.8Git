using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DL_Vendor
    {
        //public DataSet AddVendor(Vendor objVendor)
        //{
        //    try
        //    {
        //        string query = "INSERT INTO Vendor ([Rol],[Acct], [Type], [Status], [ShipVia], [Balance], [CLimit],[Terms],[Days],[1099],[InUse],[DA],[Remit],[intBox],[FID]),[Acct#]) OUTPUT INSERTED.ID VALUES (@Rol,@Acct,@Type, @Status, @ShipVia, @Balance, @CLimit, @Terms, @Days, @Vendor1099,@Inuse,@DA,@Remit,@Vendor1099Box,@FID,@Acct#)";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@Rol", objVendor.Rol));
        //        parameters.Add(new SqlParameter("@Acct", objVendor.Acct));
        //        parameters.Add(new SqlParameter("@Type", objVendor.Type));
        //        parameters.Add(new SqlParameter("@Status", objVendor.Status));
        //        parameters.Add(new SqlParameter("@ShipVia", objVendor.ShipVia));
        //        parameters.Add(new SqlParameter("@Balance", objVendor.Balance));
        //        parameters.Add(new SqlParameter("@CLimit", objVendor.CLimit));
        //        parameters.Add(new SqlParameter("@Terms", objVendor.Terms));
        //        parameters.Add(new SqlParameter("@Days", objVendor.Days));
        //        parameters.Add(new SqlParameter("@Vendor1099", objVendor.Vendor1099));
        //        parameters.Add(new SqlParameter("@Inuse", objVendor.InUse));
        //        parameters.Add(new SqlParameter("@DA", objVendor.DA));
        //        parameters.Add(new SqlParameter("@Remit", objVendor.Remit));
        //        parameters.Add(new SqlParameter("@Vendor1099Box", objVendor.Vendor1099Box));
        //        parameters.Add(new SqlParameter("@FID", objVendor.FID));
        //        parameters.Add(new SqlParameter("@Acct#", objVendor.AcctNumber));
        //        return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, query, parameters.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public int AddVendor(Vendor objVendor)
        {
            SqlParameter[] para = new SqlParameter[25];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Rol";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objVendor.Rol;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Acct";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objVendor.Acct;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Type";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objVendor.Type;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objVendor.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "ShipVia";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objVendor.ShipVia;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Balance";
            para[5].SqlDbType = SqlDbType.Decimal;
            para[5].Value = objVendor.Balance;

            para[6] = new SqlParameter();
            para[6].ParameterName = "CLimit";
            para[6].SqlDbType = SqlDbType.Decimal;
            para[6].Value = objVendor.CLimit;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Terms";
            para[7].SqlDbType = SqlDbType.SmallInt;
            para[7].Value = objVendor.Terms;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Days";
            para[8].SqlDbType = SqlDbType.SmallInt;
            para[8].Value = objVendor.Days;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Vendor1099";
            para[9].SqlDbType = SqlDbType.SmallInt;
            para[9].Value = objVendor.Vendor1099;

            para[10] = new SqlParameter();
            para[10].ParameterName = "InUse";
            para[10].SqlDbType = SqlDbType.SmallInt;
            para[10].Value = objVendor.InUse;

            para[11] = new SqlParameter();
            para[11].ParameterName = "DA";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objVendor.DA;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Remit";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objVendor.Remit;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Vendor1099Box";
            para[13].SqlDbType = SqlDbType.SmallInt;
            para[13].Value = objVendor.Vendor1099Box;

            para[14] = new SqlParameter();
            para[14].ParameterName = "FID";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objVendor.FID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "AcctNumber";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objVendor.AcctNumber;

            para[16] = new SqlParameter();
            para[16].ParameterName = "ContactName";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objVendor.ContactName;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Phone";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objVendor.Phone;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Email";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objVendor.Email;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Cell";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objVendor.Cell;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Fax";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objVendor.Fax;

            para[21] = new SqlParameter();
            para[21].ParameterName = "VendorData";
            para[21].SqlDbType = SqlDbType.Structured;
            para[21].Value = objVendor.VendorData;

            para[22] = new SqlParameter();
            para[22].ParameterName = "EmailRecPO";
            para[22].SqlDbType = SqlDbType.Bit;
            para[22].Value = objVendor.EmailRecPO;

            para[23] = new SqlParameter();
            para[23].ParameterName = "@UpdatedBy";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objVendor.MOMUSer;

            para[24] = new SqlParameter();
            para[24].ParameterName = "@CourierAccount";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objVendor.CourierAccount;

            try
            {
                int VendorID = 0;

                VendorID = Convert.ToInt32(SqlHelper.ExecuteScalar(objVendor.ConnConfig, CommandType.StoredProcedure, "spAddVendor", para));
                return VendorID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddVendor(AddVendorParam _AddVendorParam, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Rol";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _AddVendorParam.Rol;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Acct";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _AddVendorParam.Acct;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Type";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _AddVendorParam.Type;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = _AddVendorParam.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "ShipVia";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _AddVendorParam.ShipVia;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Balance";
            para[5].SqlDbType = SqlDbType.Decimal;
            para[5].Value = _AddVendorParam.Balance;

            para[6] = new SqlParameter();
            para[6].ParameterName = "CLimit";
            para[6].SqlDbType = SqlDbType.Decimal;
            para[6].Value = _AddVendorParam.CLimit;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Terms";
            para[7].SqlDbType = SqlDbType.SmallInt;
            para[7].Value = _AddVendorParam.Terms;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Days";
            para[8].SqlDbType = SqlDbType.SmallInt;
            para[8].Value = _AddVendorParam.Days;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Vendor1099";
            para[9].SqlDbType = SqlDbType.SmallInt;
            para[9].Value = _AddVendorParam.Vendor1099;

            para[10] = new SqlParameter();
            para[10].ParameterName = "InUse";
            para[10].SqlDbType = SqlDbType.SmallInt;
            para[10].Value = _AddVendorParam.InUse;

            para[11] = new SqlParameter();
            para[11].ParameterName = "DA";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = _AddVendorParam.DA;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Remit";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = _AddVendorParam.Remit;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Vendor1099Box";
            para[13].SqlDbType = SqlDbType.SmallInt;
            para[13].Value = _AddVendorParam.Vendor1099Box;

            para[14] = new SqlParameter();
            para[14].ParameterName = "FID";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = _AddVendorParam.FID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "AcctNumber";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _AddVendorParam.AcctNumber;

            para[16] = new SqlParameter();
            para[16].ParameterName = "ContactName";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _AddVendorParam.ContactName;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Phone";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _AddVendorParam.Phone;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Email";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _AddVendorParam.Email;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Cell";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _AddVendorParam.Cell;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Fax";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = _AddVendorParam.Fax;


            para[21] = new SqlParameter();
            para[21].ParameterName = "VendorData";
            para[21].SqlDbType = SqlDbType.Structured;
            if (_AddVendorParam.VendorData.Rows.Count > 0)
            {
                if (_AddVendorParam.VendorData.Rows[0]["ContactID"].ToString() != "0")
                {
                    para[21].Value = _AddVendorParam.VendorData;
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
                    dt.Columns.Add("EmailRecPO", typeof(bool));
                    para[21].Value = dt;
                }

            }
            

            para[22] = new SqlParameter();
            para[22].ParameterName = "EmailRecPO";
            para[22].SqlDbType = SqlDbType.Bit;
            para[22].Value = _AddVendorParam.EmailRecPO;

            para[23] = new SqlParameter();
            para[23].ParameterName = "@UpdatedBy";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = _AddVendorParam.MOMUSer;

            try
            {
                int VendorID = 0;

                VendorID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "spAddVendor", para));
                return VendorID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVendorContact(Vendor objVendor)
        {
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "VendorData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objVendor.VendorData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objVendor.RolId;

            try
            {
                SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, CommandType.StoredProcedure, "spUpdateVendorContact", para[0], para[1]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVendorContact(UpdateVendorContactParam _UpdateVendorContactParam, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "VendorData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = _UpdateVendorContactParam.VendorData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _UpdateVendorContactParam.RolId;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateVendorContact", para[0], para[1]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT ID,Rol, Acct, Type, Status, ShipVia, Balance, CLimit, Terms, Days, 1099 FROM Vendor Order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllVendor(string ConnectionString, GetAllVendorParam _GetAllVendorParam)
        {
            try
            {
                return  SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ID,Rol, Acct, Type, Status, ShipVia, Balance, CLimit, Terms, Days, 1099 FROM Vendor Order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetVendorBasedPORemitAddress(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Branch.PORemit FROM Vendor INNER Join Rol on Vendor.Rol = Rol.ID INNER JOIN Branch ON Branch.ID= Rol.EN AND Vendor.ID = " + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateVendor(Vendor objVendor)
        //{
        //    try
        //    {
        //        string query = "UPDATE Vendor SET  [Acct] = @Acct, [Type] = @Type, [Status] = @Status, [ShipVia] = @ShipVia, [Balance] = @Balance, [CLimit]=@CLimit, [Terms] = @Terms, [Days] = @Days, [1099] = @Vendor1099, [DA] = @DA,[Remit]=@Remit, [intBox]=@Vendor1099Box, [FID]=@FID,[Acct#]=@Acct# WHERE [ID] = @ID";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@ID", objVendor.ID));
        //        //parameters.Add(new SqlParameter("@Rol", objVendor.Rol));
        //        parameters.Add(new SqlParameter("@Acct", objVendor.Acct));
        //        parameters.Add(new SqlParameter("@Type", objVendor.Type));
        //        parameters.Add(new SqlParameter("@Status", objVendor.Status));
        //        parameters.Add(new SqlParameter("@ShipVia", objVendor.ShipVia));
        //        parameters.Add(new SqlParameter("@Balance", objVendor.Balance));
        //        parameters.Add(new SqlParameter("@CLimit", objVendor.CLimit));
        //        parameters.Add(new SqlParameter("@Terms", objVendor.Terms));
        //        parameters.Add(new SqlParameter("@Days", objVendor.Days));
        //        parameters.Add(new SqlParameter("@Vendor1099", objVendor.Vendor1099));
        //        parameters.Add(new SqlParameter("@DA", objVendor.DA));
        //        parameters.Add(new SqlParameter("@Remit", objVendor.Remit));
        //        parameters.Add(new SqlParameter("@Vendor1099Box", objVendor.Vendor1099Box));
        //        parameters.Add(new SqlParameter("@FID", objVendor.FID));
        //        parameters.Add(new SqlParameter("@Acct#", objVendor.AcctNumber));
        //        int rowsAffected = SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, CommandType.Text, query, parameters.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateVendor(Vendor objVendor)
        {
            SqlParameter[] para = new SqlParameter[26];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Rol";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objVendor.Rol;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Acct";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objVendor.Acct;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Type";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objVendor.Type;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objVendor.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "ShipVia";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objVendor.ShipVia;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Balance";
            para[5].SqlDbType = SqlDbType.Decimal;
            para[5].Value = objVendor.Balance;

            para[6] = new SqlParameter();
            para[6].ParameterName = "CLimit";
            para[6].SqlDbType = SqlDbType.Decimal;
            para[6].Value = objVendor.CLimit;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Terms";
            para[7].SqlDbType = SqlDbType.SmallInt;
            para[7].Value = objVendor.Terms;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Days";
            para[8].SqlDbType = SqlDbType.SmallInt;
            para[8].Value = objVendor.Days;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Vendor1099";
            para[9].SqlDbType = SqlDbType.SmallInt;
            para[9].Value = objVendor.Vendor1099;

            para[10] = new SqlParameter();
            para[10].ParameterName = "InUse";
            para[10].SqlDbType = SqlDbType.SmallInt;
            para[10].Value = objVendor.InUse;

            para[11] = new SqlParameter();
            para[11].ParameterName = "DA";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objVendor.DA;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Remit";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objVendor.Remit;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Vendor1099Box";
            para[13].SqlDbType = SqlDbType.SmallInt;
            para[13].Value = objVendor.Vendor1099Box;

            para[14] = new SqlParameter();
            para[14].ParameterName = "FID";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objVendor.FID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "AcctNumber";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objVendor.AcctNumber;

            para[16] = new SqlParameter();
            para[16].ParameterName = "ContactName";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objVendor.ContactName;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Phone";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objVendor.Phone;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Email";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objVendor.Email;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Cell";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objVendor.Cell;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Fax";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objVendor.Fax;

            para[21] = new SqlParameter();
            para[21].ParameterName = "VendorData";
            para[21].SqlDbType = SqlDbType.Structured;
            para[21].Value = objVendor.VendorData;

            para[22] = new SqlParameter();
            para[22].ParameterName = "ID";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objVendor.ID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "EmailRecPO";
            para[23].SqlDbType = SqlDbType.Bit;
            para[23].Value = objVendor.EmailRecPO;

            para[24] = new SqlParameter();
            para[24].ParameterName = "@UpdatedBy";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objVendor.MOMUSer;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@CourierAccount";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objVendor.CourierAccount;

            try
            {
                SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, CommandType.StoredProcedure, "spUpdateVendor", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVendor(UpdateVendorParam _UpdateVendorParam, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[25];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Rol";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _UpdateVendorParam.Rol;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Acct";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _UpdateVendorParam.Acct;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Type";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _UpdateVendorParam.Type;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = _UpdateVendorParam.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "ShipVia";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = _UpdateVendorParam.ShipVia;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Balance";
            para[5].SqlDbType = SqlDbType.Decimal;
            para[5].Value = _UpdateVendorParam.Balance;

            para[6] = new SqlParameter();
            para[6].ParameterName = "CLimit";
            para[6].SqlDbType = SqlDbType.Decimal;
            para[6].Value = _UpdateVendorParam.CLimit;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Terms";
            para[7].SqlDbType = SqlDbType.SmallInt;
            para[7].Value = _UpdateVendorParam.Terms;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Days";
            para[8].SqlDbType = SqlDbType.SmallInt;
            para[8].Value = _UpdateVendorParam.Days;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Vendor1099";
            para[9].SqlDbType = SqlDbType.SmallInt;
            para[9].Value = _UpdateVendorParam.Vendor1099;

            para[10] = new SqlParameter();
            para[10].ParameterName = "InUse";
            para[10].SqlDbType = SqlDbType.SmallInt;
            para[10].Value = _UpdateVendorParam.InUse;

            para[11] = new SqlParameter();
            para[11].ParameterName = "DA";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = _UpdateVendorParam.DA;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Remit";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = _UpdateVendorParam.Remit;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Vendor1099Box";
            para[13].SqlDbType = SqlDbType.SmallInt;
            para[13].Value = _UpdateVendorParam.Vendor1099Box;

            para[14] = new SqlParameter();
            para[14].ParameterName = "FID";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = _UpdateVendorParam.FID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "AcctNumber";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = _UpdateVendorParam.AcctNumber;

            para[16] = new SqlParameter();
            para[16].ParameterName = "ContactName";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _UpdateVendorParam.ContactName;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Phone";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = _UpdateVendorParam.Phone;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Email";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _UpdateVendorParam.Email;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Cell";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _UpdateVendorParam.Cell;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Fax";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = _UpdateVendorParam.Fax;

            para[21] = new SqlParameter();
            para[21].ParameterName = "VendorData";
            para[21].SqlDbType = SqlDbType.Structured;
            if (_UpdateVendorParam.VendorData.Rows.Count > 0)
            {
                if (_UpdateVendorParam.VendorData.Rows[0]["ContactID"].ToString() != "0")
                {
                    para[21].Value = _UpdateVendorParam.VendorData;
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
                    dt.Columns.Add("EmailRecPO", typeof(bool));
                    para[21].Value = dt;
                }

            }
            //para[21].Value = _UpdateVendorParam.VendorData;

            para[22] = new SqlParameter();
            para[22].ParameterName = "ID";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = _UpdateVendorParam.ID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "EmailRecPO";
            para[23].SqlDbType = SqlDbType.Bit;
            para[23].Value = _UpdateVendorParam.EmailRecPO;

            para[24] = new SqlParameter();
            para[24].ParameterName = "@UpdatedBy";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = _UpdateVendorParam.MOMUSer;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateVendor", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateVendorSTax(string ConnConfig, string Stax, int VendorId)
        {
            try
            {
                SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "UPDATE Vendor SET STax ='" + Stax + "' WHERE ID=" + VendorId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateVendorSTax(UpdateVendorSTaxParam _UpdateVendorSTaxParam, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "UPDATE Vendor SET STax ='" + _UpdateVendorSTaxParam.sTax + "' WHERE ID=" + _UpdateVendorSTaxParam.VendorId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet getVendorContactByRolID(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, "SpgetVendorcontactbyrol", objVendor.Rol);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getVendorContactByRolID(getVendorContactByRolIDParam _getVendorContactByRolIDParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "SpgetVendorcontactbyrol", _getVendorContactByRolIDParam.Rol);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT ID,Rol,Acct,Type,Status,isnull(Balance,0) as Balance,CLimit,[1099],FID,DA,[Acct#],Terms,Disc,Days,InUse,Remit,OnePer,DBank,Custom1,Custom2,Custom3,Custom4,Custom5,Custom6,Custom7,Custom8,Custom9,Custom10 FROM Vendor WHERE ID = " + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendor(GetVendorParam _GetVendorParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ID,Rol,Acct,Type,Status,isnull(Balance,0) as Balance,CLimit,[1099],FID,DA,[Acct#],Terms,Disc,Days,InUse,Remit,OnePer,DBank,Custom1,Custom2,Custom3,Custom4,Custom5,Custom6,Custom7,Custom8,Custom9,Custom10 FROM Vendor WHERE ID = " + _GetVendorParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteVendor(Vendor objVendor)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, CommandType.Text, " DELETE FROM Vendor WHERE ID = " + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteVendor(DeleteVendorParam _DeleteVendorParam, string connectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, " DELETE FROM Vendor WHERE ID = " + _DeleteVendorParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistsForInsertVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.DsIsExist = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "Select Count(*) as CountVendor FROM Vendor Where Acct='" + objVendor.Acct + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistsForInsertVendor(IsExistsForInsertVendorParam _IsExistsForInsertVendorParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "Select Count(*) as CountVendor FROM Vendor Where Acct='" + _IsExistsForInsertVendorParam.Acct + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistForUpdateVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.DsIsExist = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "Select Count(*) as CountVendor FROM Vendor Where Acct='" + objVendor.Acct + "' AND ID !=" + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistForUpdateVendor(IsExistForUpdateVendorParam _IsExistForUpdateVendorParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "Select Count(*) as CountVendor FROM Vendor Where Acct='" + _IsExistForUpdateVendorParam.Acct + "' AND ID !=" + _IsExistForUpdateVendorParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendorGridview(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Vendor.ID,Vendor.Rol,Rol.Name,Rol.Address,Rol.City,Rol.State,Rol.Country,Rol.EMail,Rol.Website,Rol.Cellular,Rol.Zip,Rol.Phone,Rol.Fax,Rol.Contact,Rol.GeoLock,Rol.Since,Rol.Last, Vendor.Acct, Rol.Type, Vendor.Status, Vendor.ShipVia, (isnull(Vendor.Balance,0)*-1) as Balance ,Vendor.InUse, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099] FROM Vendor Join Rol on Vendor.Rol=Rol.ID Order by Rol.Name");
                //SELECT Vendor.ID,Rol.Name,Rol.Address,Rol.City,Rol.State,Rol.Country,Rol.EMail,Rol.Website,Rol.Cellular,Rol.Zip,Rol.Phone,Rol.Fax,Rol.Contact,Rol.GeoLock,Rol.Since,Rol.Last, Vendor.Acct, Vendor.Type, Vendor.Status, Vendor.ShipVia, Vendor.Balance,Vendor.InUse, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099] FROM Vendor Join Rol on Vendor.Rol=Rol.ID Order by ID);
                //"SELECT Vendor.ID,Rol.Name, Vendor.Acct, Vendor.Type, Vendor.Status, Vendor.ShipVia, Vendor.Balance, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099] FROM Vendor Join Rol on Vendor.Rol=Rol.ID Order by ID;");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendorDetails(Vendor objVendor) // Get Vendors details who's expense is exists in PJ table.
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID,v.Rol,r.Name,v.Acct,v.Type,v.Status,isnull(v.Balance,0) as Balance,v.CLimit,v.FID,v.DA,v.Acct#,v.Terms,v.Disc,v.Days,v.InUse,v.Remit,v.OnePer,v.DBank,v.Custom1,v.Custom2,v.Custom3,v.Custom4,v.Custom5,v.Custom6,v.Custom7,v.Custom8,v.Custom9,v.Custom10,v.ShipVia,v.QBVendorID FROM Vendor v, Rol r, PJ p where v.ID=p.Vendor AND v.Rol=r.ID Order by r.Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendorEdit(Vendor objVendor)
        {
            try
            {
                //return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Vendor.ID, Vendor.Rol,Rol.Name,B.Name As Company,Rol.EN,Rol.Address,Rol.City,Rol.State,Rol.Country,Rol.EMail,Rol.Website,Rol.Cellular,Rol.Zip,Rol.Phone,Rol.Fax,Rol.Contact,Rol.GeoLock,Rol.Since,Rol.Last, Vendor.Acct,Vendor.Acct# ,Rol.Type, Vendor.Status, Vendor.ShipVia, isnull(Vendor.Balance,0)*-1 as Balance, Vendor.InUse, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099], isnull(Vendor.DA,0) as DA, isnull(chart.fDesc,'') as DefaultAcct,Vendor.Remit, Vendor.intBox, Vendor.FID,isnull(Ph.EmailRecPO ,0) As EmailRecPO,Vendor.Type as VType,Vendor.STax,Vendor.Utax,Rol.Remarks  FROM Vendor Left Join Rol on Vendor.Rol=Rol.ID left join Chart on Vendor.DA=Chart.ID left Outer join Branch B on B.ID = Rol.EN left join Phone ph on ph.Rol=Vendor.Rol where Vendor.ID=" + objVendor.ID);
                
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, "spGetVendorByID", objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendorEdit(GetVendorEditParam _GetVendorEditParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT Vendor.ID, Vendor.Rol,Rol.Name,B.Name As Company,Rol.EN,Rol.Address,Rol.City,Rol.State,Rol.Country,Rol.EMail,Rol.Website,Rol.Cellular,Rol.Zip,Rol.Phone,Rol.Fax,Rol.Contact,Rol.GeoLock,Rol.Since,Rol.Last, Vendor.Acct,Vendor.Acct# ,Rol.Type, Vendor.Status, Vendor.ShipVia, isnull(Vendor.Balance,0)*-1 as Balance, Vendor.InUse, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099], isnull(Vendor.DA,0) as DA, isnull(chart.fDesc,'') as DefaultAcct,Vendor.Remit, Vendor.intBox, Vendor.FID,isnull(Ph.EmailRecPO ,0) As EmailRecPO,Vendor.Type as VType,Vendor.STax,Vendor.Utax,Rol.Remarks  FROM Vendor Left Join Rol on Vendor.Rol=Rol.ID left join Chart on Vendor.DA=Chart.ID left Outer join Branch B on B.ID = Rol.EN left join Phone ph on ph.Rol=Vendor.Rol where Vendor.ID=" + _GetVendorEditParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendors(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID,v.Rol,r.Name,v.Acct,v.Type,v.Status,v.Balance,v.CLimit,v.FID,v.DA,v.Acct#,v.Terms,v.Disc,v.Days,v.InUse,v.Remit,v.OnePer,v.DBank,v.Custom1,v.Custom2,v.Custom3,v.Custom4,v.Custom5,v.Custom6,v.Custom7,v.Custom8,v.Custom9,v.Custom10,v.ShipVia,v.QBVendorID FROM Vendor v, Rol r where v.Rol=r.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorSearch(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, "spGetVendorSearch", objVendor.SearchValue, objVendor.EN);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetVendorSearch(string ConnectionString, GetVendorSearchParam _GetVendorSearchParam)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetVendorSearch", _GetVendorSearchParam.SearchValue, _GetVendorSearchParam.EN);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetVendorByName(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, "spGetVendorByName", objVendor.SearchValue, objVendor.EN);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetVendorByName(GetVendorByNameParam _GetVendorByNameParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetVendorByName", _GetVendorByNameParam.SearchValue, _GetVendorByNameParam.EN);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetVendorSearchProject(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, "spGetVendorSearchProject", objVendor.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool IsExistVendorDetails(Vendor _objVendor)
        {
            try
            {
                return _objVendor.IsExist = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objVendor.ConnConfig, "spIsVendorExist", _objVendor.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistVendorDetails(IsExistVendorDetailsParam _IsExistVendorDetailsParam, string connectionString)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(connectionString, "spIsVendorExist", _IsExistVendorDetailsParam.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorRolDetails(Vendor _objVendor)
        {
            try
            {
                return _objVendor.Ds = SqlHelper.ExecuteDataset(_objVendor.ConnConfig, CommandType.Text, "SELECT v.*,isnull(LTRIM(RTRIM(v.Remit)),'') as RemitAddress, r.Address + ', ' + Char(13)+CHAR(10) + r.City + ', ' + r.State + ', ' + r.Zip as VendorAddress, r.Name,r.State,r.City,r.Zip,r.Address FROM Vendor as v, Rol as r WHERE v.Rol = r.ID and v.ID = " + _objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVendorRolDetails(GetVendorRolDetailsParam _objGetVendorRolDetailsParam,string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT v.*,isnull(LTRIM(RTRIM(v.Remit)),'') as RemitAddress, r.Address + ', ' + Char(13)+CHAR(10) + r.City + ', ' + r.State + ', ' + r.Zip as VendorAddress, r.Name,r.State,r.City,r.Zip,r.Address FROM Vendor as v, Rol as r WHERE v.Rol = r.ID and v.ID = " + _objGetVendorRolDetailsParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorGLById(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Vendor.ID, isnull(Vendor.DA,0) as DA, chart.Acct ,isnull(chart.fDesc,'') as DefaultAcct FROM Vendor LEFT JOIN Rol ON Vendor.Rol=Rol.ID LEFT JOIN Chart ON Vendor.DA=Chart.ID WHERE (Vendor.DA <> 0 OR Vendor.DA <> NULL) AND Vendor.ID = '" + objVendor.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVendorListDetails(Vendor objVendor)
        {
            try
            {
                //return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID,v.Rol,r.Name,v.Acct,v.Type,v.Status,v.Balance,v.CLimit,v.FID,v.DA,v.Acct#,v.Terms,v.Disc,v.Days,v.InUse,v.Remit,v.OnePer,v.DBank,v.Custom1,v.Custom2,v.Custom3,v.Custom4,v.Custom5,v.Custom6,v.Custom7,v.Custom8,v.Custom9,v.Custom10,v.ShipVia,v.QBVendorID FROM Vendor v, Rol r where v.Rol=r.ID");

                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.StoredProcedure, "SpVendorList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVendorAcct(Vendor _objVendor)
        {
            try
            {
                return _objVendor.Ds = SqlHelper.ExecuteDataset(_objVendor.ConnConfig, CommandType.Text, "SELECT v.ID,v.Acct# FROM Vendor as v WHERE v.ID = " + _objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVendorAcct(GetVendorAcctParam _GetVendorAcctParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT v.ID,v.Acct# FROM Vendor as v WHERE v.ID = " + _GetVendorAcctParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVenderAjaxSearch(Vendor objVendor)
        {
            DataSet ds = new DataSet();
            try
            {

                var para = new SqlParameter[9];

                para[0] = new SqlParameter
                {
                    ParameterName = "Cols",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objVendor.Cols
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "SearchVal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objVendor.SearchValue
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = objVendor.EN
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objVendor.UserID
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    SqlDbType = SqlDbType.Int,
                    Value = objVendor.PageNumber
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = objVendor.PageSize
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = objVendor.StatusDisplay
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "SortBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objVendor.SortBy
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "SortType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objVendor.SortType
                };
                ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, "spGetAllVendorSearchPaging", para);



                objVendor.Ds = ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public DataSet GetAllVenderAjaxSearch(GetAllVenderAjaxSearchParam _GetAllVenderAjaxSearchParam, string ConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {

                var para = new SqlParameter[9];

                para[0] = new SqlParameter
                {
                    ParameterName = "Cols",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetAllVenderAjaxSearchParam.Cols
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "SearchVal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetAllVenderAjaxSearchParam.SearchValue
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAllVenderAjaxSearchParam.EN
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAllVenderAjaxSearchParam.UserID
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAllVenderAjaxSearchParam.PageNumber
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = _GetAllVenderAjaxSearchParam.PageSize
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "Status",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = _GetAllVenderAjaxSearchParam.StatusDisplay
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "SortBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetAllVenderAjaxSearchParam.SortBy
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "SortType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _GetAllVenderAjaxSearchParam.SortType
                };
                ds = SqlHelper.ExecuteDataset(ConnectionString, "spGetAllVendorSearchPaging", para);

                _GetAllVenderAjaxSearchParam.Ds = ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }
        public DataSet GetOpenBillVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID , R.Name FROM Vendor v INNER JOIN OpenAP o ON v.ID = o.Vendor LEFT JOIN Rol r ON r.ID = v.Rol INNER JOIN PJ p ON p.ID = o.PJID WHERE v.Status = 0 AND o.Type = 0 AND p.Status not in (1,2) AND ((o.Original<>(o.Selected+o.Disc)) OR o.Original =0)  ORDER BY R.Name ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenBillVendor(GetOpenBillVendorParam _GetOpenBillVendorParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT DISTINCT v.ID , R.Name FROM Vendor v INNER JOIN OpenAP o ON v.ID = o.Vendor LEFT JOIN Rol r ON r.ID = v.Rol WHERE v.Status = 0 AND o.Type = 0 AND o.Original<>(o.Selected+o.Disc)  ORDER BY R.Name ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOpenBillVendorByCompany(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID , r.Name ,r.EN,b.Name As Company FROM Vendor v INNER JOIN OpenAP o ON v.ID = o.Vendor LEFT JOIN Rol r ON r.ID = v.Rol left Outer join Branch B on B.ID = r.EN  WHERE v.Status = 0 and r.EN=  " + objVendor.EN + " AND o.Type = 0 AND o.Original<>o.Selected  ORDER BY R.Name  ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenBillVendorByCompany(GetOpenBillVendorByCompanyParam _GetOpenBillVendorByCompanyParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT DISTINCT v.ID , r.Name ,r.EN,b.Name As Company FROM Vendor v INNER JOIN OpenAP o ON v.ID = o.Vendor LEFT JOIN Rol r ON r.ID = v.Rol left Outer join Branch B on B.ID = r.EN  WHERE v.Status = 0 and r.EN=  " + _GetOpenBillVendorByCompanyParam.EN + " AND o.Type = 0 AND o.Original<>o.Selected  ORDER BY R.Name  ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateVendorBalance(Vendor objVendor)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, "spUpdateVendorBalance", objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVendorBalance(UpdateVendorBalanceParam objUpdateVendorBalanceParam, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "spUpdateVendorBalance", objUpdateVendorBalanceParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorLogs(Vendor objVendor)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objVendor.ID + "  and Screen='Vendor' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorLogs(GetVendorLogsParam _GetVendorLogsParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from Log2 where ref =" + _GetVendorLogsParam.ID + "  and Screen='Vendor' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void IsSalesTaxAPBill(string ConnConfig, int IsSalesTaxAPBill, int IsUseTaxAPBill)
        {
            try
            {
                SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "UPDATE Control SET IsSalesTaxAPBill=" + IsSalesTaxAPBill + " , IsUseTaxAPBill=" + IsUseTaxAPBill);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateVendorTax(Vendor objVendor)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, "spUpdateVendorBalance", objVendor.ID);
                SqlHelper.ExecuteScalar(objVendor.ConnConfig, CommandType.Text, "UPDATE Vendor SET STax='" + objVendor.Custom1 + "' , UTax='" + objVendor.Custom2 + "' WHERE ID ="+objVendor.ID+"");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVendorTax(UpdateVendorTaxParam _UpdateVendorTaxParam, string ConnectionString)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, "spUpdateVendorBalance", objVendor.ID);
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "UPDATE Vendor SET STax='" + _UpdateVendorTaxParam.Custom1 + "' , UTax='" + _UpdateVendorTaxParam.Custom2 + "' WHERE ID =" + _UpdateVendorTaxParam.ID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet getVendorType(getVendorTypeParam _getVendorTypeParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select Type,Remarks, (select Count(1)from Vendor where Type= t.Type ) as Count from vtype t order by Type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetVendorNameById(int vendorId, string ConnectionString)
        {
            try
            {
                var query = string.Format("select r.name from Vendor v " +
                            "inner join rol r on r.id = v.Rol " +
                            "where v.id = {0}", vendorId);
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, query);
                if(ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
