using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;

namespace DataLayer
{
    public class DL_Company
    {

       public DataSet getCompanies(CompanyOffice objCompany)
       {

           try
           {
               return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetCompany", DBNull.Value, DBNull.Value, objCompany.DBName);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public DataSet getCompanyByID(CompanyOffice objCompany)
       {
           try
           {
               return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetCompanyByID", objCompany.ID, objCompany.DBName);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet getOfficeByCompanyID(CompanyOffice objCompany)
       {
           try
           {
               return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetOfficeByCompanyID", objCompany.ID, objCompany.DBName);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet getOfficeByID(CompanyOffice objCompany)
       {
           try
           {
               return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetOfficeByID", objCompany.ID, objCompany.DBName);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public DataSet getCompanySearch(CompanyOffice objCompany)
       {

           try
           {
               return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetCompany", objCompany.SearchBy, objCompany.SearchValue, objCompany.DBName);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

        public DataSet getOfficeSearch(CompanyOffice objCompany)
        {

            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetOfficeSearch", objCompany.SearchBy, objCompany.SearchValue, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCompanyType(CompanyOffice objCompany)
       {
           try
           {
               return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, CommandType.Text, "select Type,Remarks, (select Count(1)from Branch where Type= t.Type ) as Count from TypeCoOfc t");
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void AddCompany(CompanyOffice objCompany)
       {
           int CompOffID;
           SqlParameter[] para = new SqlParameter[40];

           para[0] = new SqlParameter();
           para[0].ParameterName = "CompanyID";
           para[0].SqlDbType = SqlDbType.Int;
           para[0].Value = objCompany.ID;

           para[1] = new SqlParameter();
           para[1].ParameterName = "Name";
           para[1].SqlDbType = SqlDbType.VarChar;
           para[1].Value = objCompany.Name;

           para[2] = new SqlParameter();
           para[2].ParameterName = "Manager";
           para[2].SqlDbType = SqlDbType.VarChar;
           para[2].Value = objCompany.Manager;

           para[3] = new SqlParameter();
           para[3].ParameterName = "Address";
           para[3].SqlDbType = SqlDbType.VarChar;
           para[3].Value = objCompany.Address;

           para[4] = new SqlParameter();
           para[4].ParameterName = "City";
           para[4].SqlDbType = SqlDbType.VarChar;
           para[4].Value = objCompany.City;

           para[5] = new SqlParameter();
           para[5].ParameterName = "State";
           para[5].SqlDbType = SqlDbType.VarChar;
           para[5].Value = objCompany.State;

           para[6] = new SqlParameter();
           para[6].ParameterName = "Zip";
           para[6].SqlDbType = SqlDbType.VarChar;
           para[6].Value = objCompany.Zip;

           para[7] = new SqlParameter();
           para[7].ParameterName = "Phone";
           para[7].SqlDbType = SqlDbType.VarChar;
           para[7].Value = objCompany.Phone;

           para[8] = new SqlParameter();
           para[8].ParameterName = "Fax";
           para[8].SqlDbType = SqlDbType.VarChar;
           para[8].Value = objCompany.Fax;

           para[9] = new SqlParameter();
           para[9].ParameterName = "CostCenter";
           para[9].SqlDbType = SqlDbType.VarChar;
           para[9].Value = objCompany.CostCenter;

           para[10] = new SqlParameter();
           para[10].ParameterName = "InvRemarks";
           para[10].SqlDbType = SqlDbType.VarChar;
           para[10].Value = objCompany.InvRemarks;

           para[11] = new SqlParameter();
           para[11].ParameterName = "Logo";
           para[11].SqlDbType = SqlDbType.Image;
           para[11].Value = objCompany.Logo;

           para[12] = new SqlParameter();
           para[12].ParameterName = "LogoPath";
           para[12].SqlDbType = SqlDbType.VarChar;
           para[12].Value = objCompany.LogoPath;

           para[13] = new SqlParameter();
           para[13].ParameterName = "BillRemit";
           para[13].SqlDbType = SqlDbType.VarChar;
           para[13].Value = objCompany.BillRemit;

           para[14] = new SqlParameter();
           para[14].ParameterName = "PORemit";
           para[14].SqlDbType = SqlDbType.VarChar;
           para[14].Value = objCompany.PORemit;

           para[15] = new SqlParameter();
           para[15].ParameterName = "LocDTerr";
           para[15].SqlDbType = SqlDbType.VarChar;
           para[15].Value = objCompany.LocDTerr;

           para[16] = new SqlParameter();
           para[16].ParameterName = "LocDRoute";
           para[16].SqlDbType = SqlDbType.VarChar;
           para[16].Value = objCompany.LocDRoute;

           para[17] = new SqlParameter();
           para[17].ParameterName = "LocDZone";
           para[17].SqlDbType = SqlDbType.VarChar;
           para[17].Value = objCompany.LocDZone;

           para[18] = new SqlParameter();
           para[18].ParameterName = "LocDStax";
           para[18].SqlDbType = SqlDbType.VarChar;
           para[18].Value = objCompany.LocDSTax;

           para[19] = new SqlParameter();
           para[19].ParameterName = "LocType";
           para[19].SqlDbType = SqlDbType.VarChar;
           para[19].Value = objCompany.LocType;

           para[20] = new SqlParameter();
           para[20].ParameterName = "ARTerms";
           para[20].SqlDbType = SqlDbType.VarChar;
           para[20].Value = objCompany.ARTerms;

           para[21] = new SqlParameter();
           para[21].ParameterName = "ADP";
           para[21].SqlDbType = SqlDbType.VarChar;
           para[21].Value = objCompany.ADP;

           para[22] = new SqlParameter();
           para[22].ParameterName = "CB";
           para[22].SqlDbType = SqlDbType.Float;
           para[22].Value = objCompany.CB;

           para[23] = new SqlParameter();
           para[23].ParameterName = "ARContact";
           para[23].SqlDbType = SqlDbType.VarChar;
           para[23].Value = objCompany.ARContact;

           para[24] = new SqlParameter();
           para[24].ParameterName = "OType";
           para[24].SqlDbType = SqlDbType .VarChar;
           para[24].Value = objCompany.OType;

           para[25] = new SqlParameter();
           para[25].ParameterName = "DArea";
           para[25].SqlDbType = SqlDbType.VarChar;
           para[25].Value = objCompany.DArea;

           para[26] = new SqlParameter();
           para[26].ParameterName = "DState";
           para[26].SqlDbType = SqlDbType.VarChar;
           para[26].Value = objCompany.DState;

           para[27] = new SqlParameter();
           para[27].ParameterName = "MileRate";
           para[27].SqlDbType = SqlDbType.Float;
           para[27].Value = objCompany.MileRate;

           para[28] = new SqlParameter();
           para[28].ParameterName = "PriceD1";
           para[28].SqlDbType = SqlDbType.Decimal;
           para[28].Value = objCompany.PriceD1;

           para[29] = new SqlParameter();
           para[29].ParameterName = "PriceD2";
           para[29].SqlDbType = SqlDbType.Decimal;
           para[29].Value = objCompany.PriceD2;

           para[30] = new SqlParameter();
           para[30].ParameterName = "PriceD3";
           para[30].SqlDbType = SqlDbType.Decimal;
           para[30].Value = objCompany.PriceD3;

           para[31] = new SqlParameter();
           para[31].ParameterName = "PriceD4";
           para[31].SqlDbType = SqlDbType.Decimal;
           para[31].Value = objCompany.PriceD4;

           para[32] = new SqlParameter();
           para[32].ParameterName = "PriceD5";
           para[32].SqlDbType = SqlDbType.Decimal;
           para[32].Value = objCompany.PriceD5;

           para[33] = new SqlParameter();
           para[33].ParameterName = "UTaxR";
           para[33].SqlDbType = SqlDbType.Int;
           para[33].Value = objCompany.UTaxR;

           para[34] = new SqlParameter();
           para[34].ParameterName = "UTax";
           para[34].SqlDbType = SqlDbType.VarChar;
           para[34].Value = objCompany.UTax;

           para[35] = new SqlParameter();
           para[35].ParameterName = "Status";
           para[35].SqlDbType = SqlDbType.Int;
           para[35].Value = objCompany.Status;

            para[36] = new SqlParameter();
            para[36].ParameterName = "DInvAcct";
            para[36].SqlDbType = SqlDbType.Int;
            para[36].Value = objCompany.DInvAcct;

            para[37] = new SqlParameter();
            para[37].ParameterName = "Longitude";
            para[37].SqlDbType = SqlDbType.NVarChar;
            para[37].Value = objCompany.Longitude;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Latitude";
            para[38].SqlDbType = SqlDbType.NVarChar;
            para[38].Value = objCompany.Latitude;

            para[39] = new SqlParameter();
            para[39].ParameterName = "Country";
            para[39].SqlDbType = SqlDbType.NVarChar;
            para[39].Value = objCompany.Country;
            try
           {
               //CompOffID = Convert.ToInt32(SqlHelper.ExecuteScalar(objCompany.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para));                
               SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, CommandType.StoredProcedure, "spAddCompanyOffice", para);
               CompOffID = Convert.ToInt32(para[0].Value);
               objCompany.ID= CompOffID;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void AddOffice(CompanyOffice objCompany)
       {
           int CompOffID;
           SqlParameter[] para = new SqlParameter[38];

           para[0] = new SqlParameter();
           para[0].ParameterName = "CompanyID";
           para[0].SqlDbType = SqlDbType.Int;
           para[0].Value = objCompany.ID;

           para[1] = new SqlParameter();
           para[1].ParameterName = "Name";
           para[1].SqlDbType = SqlDbType.VarChar;
           para[1].Value = objCompany.Name;

           para[2] = new SqlParameter();
           para[2].ParameterName = "Manager";
           para[2].SqlDbType = SqlDbType.VarChar;
           para[2].Value = objCompany.Manager;

           para[3] = new SqlParameter();
           para[3].ParameterName = "Address";
           para[3].SqlDbType = SqlDbType.VarChar;
           para[3].Value = objCompany.Address;

           para[4] = new SqlParameter();
           para[4].ParameterName = "City";
           para[4].SqlDbType = SqlDbType.VarChar;
           para[4].Value = objCompany.City;

           para[5] = new SqlParameter();
           para[5].ParameterName = "State";
           para[5].SqlDbType = SqlDbType.VarChar;
           para[5].Value = objCompany.State;

           para[6] = new SqlParameter();
           para[6].ParameterName = "Zip";
           para[6].SqlDbType = SqlDbType.VarChar;
           para[6].Value = objCompany.Zip;

           para[7] = new SqlParameter();
           para[7].ParameterName = "Phone";
           para[7].SqlDbType = SqlDbType.VarChar;
           para[7].Value = objCompany.Phone;

           para[8] = new SqlParameter();
           para[8].ParameterName = "Fax";
           para[8].SqlDbType = SqlDbType.VarChar;
           para[8].Value = objCompany.Fax;

           para[9] = new SqlParameter();
           para[9].ParameterName = "CostCenter";
           para[9].SqlDbType = SqlDbType.VarChar;
           para[9].Value = objCompany.CostCenter;

           para[10] = new SqlParameter();
           para[10].ParameterName = "InvRemarks";
           para[10].SqlDbType = SqlDbType.VarChar;
           para[10].Value = objCompany.InvRemarks;

           para[11] = new SqlParameter();
           para[11].ParameterName = "Logo";
           para[11].SqlDbType = SqlDbType.Image;
           para[11].Value = objCompany.Logo;

           para[12] = new SqlParameter();
           para[12].ParameterName = "LogoPath";
           para[12].SqlDbType = SqlDbType.VarChar;
           para[12].Value = objCompany.LogoPath;

           para[13] = new SqlParameter();
           para[13].ParameterName = "BillRemit";
           para[13].SqlDbType = SqlDbType.VarChar;
           para[13].Value = objCompany.BillRemit;

           para[14] = new SqlParameter();
           para[14].ParameterName = "PORemit";
           para[14].SqlDbType = SqlDbType.VarChar;
           para[14].Value = objCompany.PORemit;

           para[15] = new SqlParameter();
           para[15].ParameterName = "LocDTerr";
           para[15].SqlDbType = SqlDbType.VarChar;
           para[15].Value = objCompany.LocDTerr;

           para[16] = new SqlParameter();
           para[16].ParameterName = "LocDRoute";
           para[16].SqlDbType = SqlDbType.VarChar;
           para[16].Value = objCompany.LocDRoute;

           para[17] = new SqlParameter();
           para[17].ParameterName = "LocDZone";
           para[17].SqlDbType = SqlDbType.VarChar;
           para[17].Value = objCompany.LocDZone;

           para[18] = new SqlParameter();
           para[18].ParameterName = "LocDStax";
           para[18].SqlDbType = SqlDbType.VarChar;
           para[18].Value = objCompany.LocDSTax;

           para[19] = new SqlParameter();
           para[19].ParameterName = "LocType";
           para[19].SqlDbType = SqlDbType.VarChar;
           para[19].Value = objCompany.LocType;

           para[20] = new SqlParameter();
           para[20].ParameterName = "ARTerms";
           para[20].SqlDbType = SqlDbType.VarChar;
           para[20].Value = objCompany.ARTerms;

           para[21] = new SqlParameter();
           para[21].ParameterName = "ADP";
           para[21].SqlDbType = SqlDbType.VarChar;
           para[21].Value = objCompany.ADP;

           para[22] = new SqlParameter();
           para[22].ParameterName = "CB";
           para[22].SqlDbType = SqlDbType.Float;
           para[22].Value = objCompany.CB;

           para[23] = new SqlParameter();
           para[23].ParameterName = "ARContact";
           para[23].SqlDbType = SqlDbType.VarChar;
           para[23].Value = objCompany.ARContact;

           para[24] = new SqlParameter();
           para[24].ParameterName = "OType";
           para[24].SqlDbType = SqlDbType.VarChar;
           para[24].Value = objCompany.OType;

           para[25] = new SqlParameter();
           para[25].ParameterName = "DArea";
           para[25].SqlDbType = SqlDbType.VarChar;
           para[25].Value = objCompany.DArea;

           para[26] = new SqlParameter();
           para[26].ParameterName = "DState";
           para[26].SqlDbType = SqlDbType.VarChar;
           para[26].Value = objCompany.DState;

           para[27] = new SqlParameter();
           para[27].ParameterName = "MileRate";
           para[27].SqlDbType = SqlDbType.Float;
           para[27].Value = objCompany.MileRate;

           para[28] = new SqlParameter();
           para[28].ParameterName = "PriceD1";
           para[28].SqlDbType = SqlDbType.Decimal;
           para[28].Value = objCompany.PriceD1;

           para[29] = new SqlParameter();
           para[29].ParameterName = "PriceD2";
           para[29].SqlDbType = SqlDbType.Decimal;
           para[29].Value = objCompany.PriceD2;

           para[30] = new SqlParameter();
           para[30].ParameterName = "PriceD3";
           para[30].SqlDbType = SqlDbType.Decimal;
           para[30].Value = objCompany.PriceD3;

           para[31] = new SqlParameter();
           para[31].ParameterName = "PriceD4";
           para[31].SqlDbType = SqlDbType.Decimal;
           para[31].Value = objCompany.PriceD4;

           para[32] = new SqlParameter();
           para[32].ParameterName = "PriceD5";
           para[32].SqlDbType = SqlDbType.Decimal;
           para[32].Value = objCompany.PriceD5;

           para[33] = new SqlParameter();
           para[33].ParameterName = "UTaxR";
           para[33].SqlDbType = SqlDbType.Int;
           para[33].Value = objCompany.UTaxR;

           para[34] = new SqlParameter();
           para[34].ParameterName = "UTax";
           para[34].SqlDbType = SqlDbType.VarChar;
           para[34].Value = objCompany.UTax;

           para[35] = new SqlParameter();
           para[35].ParameterName = "Status";
           para[35].SqlDbType = SqlDbType.Int;
           para[35].Value = objCompany.Status;

           para[36] = new SqlParameter();
           para[36].ParameterName = "Company";
           para[36].SqlDbType = SqlDbType.VarChar;
           para[36].Value = objCompany.Company;

           para[37] = new SqlParameter();
           para[37].ParameterName = "ChargeInt";
           para[37].SqlDbType = SqlDbType.VarChar;
           para[37].Value = objCompany.ChargeInt;
           try
           {
               //CompOffID = Convert.ToInt32(SqlHelper.ExecuteScalar(objCompany.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para));                
               SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, CommandType.StoredProcedure, "spAddOffice", para);
               CompOffID = Convert.ToInt32(para[0].Value);
               objCompany.ID = CompOffID;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void UpdateCompany(CompanyOffice objCompany)
       {
           int CompOffID;
           SqlParameter[] para = new SqlParameter[40];

           para[0] = new SqlParameter();
           para[0].ParameterName = "CompanyID";
           para[0].SqlDbType = SqlDbType.Int;
           para[0].Value = objCompany.ID;

           para[1] = new SqlParameter();
           para[1].ParameterName = "Name";
           para[1].SqlDbType = SqlDbType.VarChar;
           para[1].Value = objCompany.Name;

           para[2] = new SqlParameter();
           para[2].ParameterName = "Manager";
           para[2].SqlDbType = SqlDbType.VarChar;
           para[2].Value = objCompany.Manager;

           para[3] = new SqlParameter();
           para[3].ParameterName = "Address";
           para[3].SqlDbType = SqlDbType.VarChar;
           para[3].Value = objCompany.Address;

           para[4] = new SqlParameter();
           para[4].ParameterName = "City";
           para[4].SqlDbType = SqlDbType.VarChar;
           para[4].Value = objCompany.City;

           para[5] = new SqlParameter();
           para[5].ParameterName = "State";
           para[5].SqlDbType = SqlDbType.VarChar;
           para[5].Value = objCompany.State;

           para[6] = new SqlParameter();
           para[6].ParameterName = "Zip";
           para[6].SqlDbType = SqlDbType.VarChar;
           para[6].Value = objCompany.Zip;

           para[7] = new SqlParameter();
           para[7].ParameterName = "Phone";
           para[7].SqlDbType = SqlDbType.VarChar;
           para[7].Value = objCompany.Phone;

           para[8] = new SqlParameter();
           para[8].ParameterName = "Fax";
           para[8].SqlDbType = SqlDbType.VarChar;
           para[8].Value = objCompany.Fax;

           para[9] = new SqlParameter();
           para[9].ParameterName = "CostCenter";
           para[9].SqlDbType = SqlDbType.VarChar;
           para[9].Value = objCompany.CostCenter;

           para[10] = new SqlParameter();
           para[10].ParameterName = "InvRemarks";
           para[10].SqlDbType = SqlDbType.VarChar;
           para[10].Value = objCompany.InvRemarks;

           para[11] = new SqlParameter();
           para[11].ParameterName = "Logo";
           para[11].SqlDbType = SqlDbType.Image;
           para[11].Value = objCompany.Logo;

           para[12] = new SqlParameter();
           para[12].ParameterName = "LogoPath";
           para[12].SqlDbType = SqlDbType.VarChar;
           para[12].Value = objCompany.LogoPath;

           para[13] = new SqlParameter();
           para[13].ParameterName = "BillRemit";
           para[13].SqlDbType = SqlDbType.VarChar;
           para[13].Value = objCompany.BillRemit;

           para[14] = new SqlParameter();
           para[14].ParameterName = "PORemit";
           para[14].SqlDbType = SqlDbType.VarChar;
           para[14].Value = objCompany.PORemit;

           para[15] = new SqlParameter();
           para[15].ParameterName = "LocDTerr";
           para[15].SqlDbType = SqlDbType.VarChar;
           para[15].Value = objCompany.LocDTerr;

           para[16] = new SqlParameter();
           para[16].ParameterName = "LocDRoute";
           para[16].SqlDbType = SqlDbType.VarChar;
           para[16].Value = objCompany.LocDRoute;

           para[17] = new SqlParameter();
           para[17].ParameterName = "LocDZone";
           para[17].SqlDbType = SqlDbType.VarChar;
           para[17].Value = objCompany.LocDZone;

           para[18] = new SqlParameter();
           para[18].ParameterName = "LocDStax";
           para[18].SqlDbType = SqlDbType.VarChar;
           para[18].Value = objCompany.LocDSTax;

           para[19] = new SqlParameter();
           para[19].ParameterName = "LocType";
           para[19].SqlDbType = SqlDbType.VarChar;
           para[19].Value = objCompany.LocType;

           para[20] = new SqlParameter();
           para[20].ParameterName = "ARTerms";
           para[20].SqlDbType = SqlDbType.VarChar;
           para[20].Value = objCompany.ARTerms;

           para[21] = new SqlParameter();
           para[21].ParameterName = "ADP";
           para[21].SqlDbType = SqlDbType.VarChar;
           para[21].Value = objCompany.ADP;

           para[22] = new SqlParameter();
           para[22].ParameterName = "CB";
           para[22].SqlDbType = SqlDbType.Float;
           para[22].Value = objCompany.CB;

           para[23] = new SqlParameter();
           para[23].ParameterName = "ARContact";
           para[23].SqlDbType = SqlDbType.VarChar;
           para[23].Value = objCompany.ARContact;

           para[24] = new SqlParameter();
           para[24].ParameterName = "OType";
           para[24].SqlDbType = SqlDbType.VarChar;
           para[24].Value = objCompany.OType;

           para[25] = new SqlParameter();
           para[25].ParameterName = "DArea";
           para[25].SqlDbType = SqlDbType.VarChar;
           para[25].Value = objCompany.DArea;

           para[26] = new SqlParameter();
           para[26].ParameterName = "DState";
           para[26].SqlDbType = SqlDbType.VarChar;
           para[26].Value = objCompany.DState;

           para[27] = new SqlParameter();
           para[27].ParameterName = "MileRate";
           para[27].SqlDbType = SqlDbType.Float;
           para[27].Value = objCompany.MileRate;

           para[28] = new SqlParameter();
           para[28].ParameterName = "PriceD1";
           para[28].SqlDbType = SqlDbType.Decimal;
           para[28].Value = objCompany.PriceD1;

           para[29] = new SqlParameter();
           para[29].ParameterName = "PriceD2";
           para[29].SqlDbType = SqlDbType.Decimal;
           para[29].Value = objCompany.PriceD2;

           para[30] = new SqlParameter();
           para[30].ParameterName = "PriceD3";
           para[30].SqlDbType = SqlDbType.Decimal;
           para[30].Value = objCompany.PriceD3;

           para[31] = new SqlParameter();
           para[31].ParameterName = "PriceD4";
           para[31].SqlDbType = SqlDbType.Decimal;
           para[31].Value = objCompany.PriceD4;

           para[32] = new SqlParameter();
           para[32].ParameterName = "PriceD5";
           para[32].SqlDbType = SqlDbType.Decimal;
           para[32].Value = objCompany.PriceD5;

           para[33] = new SqlParameter();
           para[33].ParameterName = "UTaxR";
           para[33].SqlDbType = SqlDbType.Int;
           para[33].Value = objCompany.UTaxR;

           para[34] = new SqlParameter();
           para[34].ParameterName = "UTax";
           para[34].SqlDbType = SqlDbType.VarChar;
           para[34].Value = objCompany.UTax;

           para[35] = new SqlParameter();
           para[35].ParameterName = "Status";
           para[35].SqlDbType = SqlDbType.Int;
           para[35].Value = objCompany.Status;

            para[36] = new SqlParameter();
            para[36].ParameterName = "DInvAcct";
            para[36].SqlDbType = SqlDbType.Int;
            para[36].Value = objCompany.DInvAcct;

            para[37] = new SqlParameter();
            para[37].ParameterName = "Longitude";
            para[37].SqlDbType = SqlDbType.NVarChar;
            para[37].Value = objCompany.Longitude;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Latitude";
            para[38].SqlDbType = SqlDbType.NVarChar;
            para[38].Value = objCompany.Latitude;

            para[39] = new SqlParameter();
            para[39].ParameterName = "Country";
            para[39].SqlDbType = SqlDbType.NVarChar;
            para[39].Value = objCompany.Country;
            try
           {
               //CompOffID = Convert.ToInt32(SqlHelper.ExecuteScalar(objCompany.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para));                
               SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, CommandType.StoredProcedure, "spUpdateCompanyOffice", para);
               CompOffID = Convert.ToInt32(para[0].Value);
               objCompany.ID = CompOffID;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void UpdateOffice(CompanyOffice objCompany)
       {
           int CompOffID;
           SqlParameter[] para = new SqlParameter[38];

           para[0] = new SqlParameter();
           para[0].ParameterName = "CompanyID";
           para[0].SqlDbType = SqlDbType.Int;
           para[0].Value = objCompany.ID;

           para[1] = new SqlParameter();
           para[1].ParameterName = "Name";
           para[1].SqlDbType = SqlDbType.VarChar;
           para[1].Value = objCompany.Name;

           para[2] = new SqlParameter();
           para[2].ParameterName = "Manager";
           para[2].SqlDbType = SqlDbType.VarChar;
           para[2].Value = objCompany.Manager;

           para[3] = new SqlParameter();
           para[3].ParameterName = "Address";
           para[3].SqlDbType = SqlDbType.VarChar;
           para[3].Value = objCompany.Address;

           para[4] = new SqlParameter();
           para[4].ParameterName = "City";
           para[4].SqlDbType = SqlDbType.VarChar;
           para[4].Value = objCompany.City;

           para[5] = new SqlParameter();
           para[5].ParameterName = "State";
           para[5].SqlDbType = SqlDbType.VarChar;
           para[5].Value = objCompany.State;

           para[6] = new SqlParameter();
           para[6].ParameterName = "Zip";
           para[6].SqlDbType = SqlDbType.VarChar;
           para[6].Value = objCompany.Zip;

           para[7] = new SqlParameter();
           para[7].ParameterName = "Phone";
           para[7].SqlDbType = SqlDbType.VarChar;
           para[7].Value = objCompany.Phone;

           para[8] = new SqlParameter();
           para[8].ParameterName = "Fax";
           para[8].SqlDbType = SqlDbType.VarChar;
           para[8].Value = objCompany.Fax;

           para[9] = new SqlParameter();
           para[9].ParameterName = "CostCenter";
           para[9].SqlDbType = SqlDbType.VarChar;
           para[9].Value = objCompany.CostCenter;

           para[10] = new SqlParameter();
           para[10].ParameterName = "InvRemarks";
           para[10].SqlDbType = SqlDbType.VarChar;
           para[10].Value = objCompany.InvRemarks;

           para[11] = new SqlParameter();
           para[11].ParameterName = "Logo";
           para[11].SqlDbType = SqlDbType.Image;
           para[11].Value = objCompany.Logo;

           para[12] = new SqlParameter();
           para[12].ParameterName = "LogoPath";
           para[12].SqlDbType = SqlDbType.VarChar;
           para[12].Value = objCompany.LogoPath;

           para[13] = new SqlParameter();
           para[13].ParameterName = "BillRemit";
           para[13].SqlDbType = SqlDbType.VarChar;
           para[13].Value = objCompany.BillRemit;

           para[14] = new SqlParameter();
           para[14].ParameterName = "PORemit";
           para[14].SqlDbType = SqlDbType.VarChar;
           para[14].Value = objCompany.PORemit;

           para[15] = new SqlParameter();
           para[15].ParameterName = "LocDTerr";
           para[15].SqlDbType = SqlDbType.VarChar;
           para[15].Value = objCompany.LocDTerr;

           para[16] = new SqlParameter();
           para[16].ParameterName = "LocDRoute";
           para[16].SqlDbType = SqlDbType.VarChar;
           para[16].Value = objCompany.LocDRoute;

           para[17] = new SqlParameter();
           para[17].ParameterName = "LocDZone";
           para[17].SqlDbType = SqlDbType.VarChar;
           para[17].Value = objCompany.LocDZone;

           para[18] = new SqlParameter();
           para[18].ParameterName = "LocDStax";
           para[18].SqlDbType = SqlDbType.VarChar;
           para[18].Value = objCompany.LocDSTax;

           para[19] = new SqlParameter();
           para[19].ParameterName = "LocType";
           para[19].SqlDbType = SqlDbType.VarChar;
           para[19].Value = objCompany.LocType;

           para[20] = new SqlParameter();
           para[20].ParameterName = "ARTerms";
           para[20].SqlDbType = SqlDbType.VarChar;
           para[20].Value = objCompany.ARTerms;

           para[21] = new SqlParameter();
           para[21].ParameterName = "ADP";
           para[21].SqlDbType = SqlDbType.VarChar;
           para[21].Value = objCompany.ADP;

           para[22] = new SqlParameter();
           para[22].ParameterName = "CB";
           para[22].SqlDbType = SqlDbType.Float;
           para[22].Value = objCompany.CB;

           para[23] = new SqlParameter();
           para[23].ParameterName = "ARContact";
           para[23].SqlDbType = SqlDbType.VarChar;
           para[23].Value = objCompany.ARContact;

           para[24] = new SqlParameter();
           para[24].ParameterName = "OType";
           para[24].SqlDbType = SqlDbType.VarChar;
           para[24].Value = objCompany.OType;

           para[25] = new SqlParameter();
           para[25].ParameterName = "DArea";
           para[25].SqlDbType = SqlDbType.VarChar;
           para[25].Value = objCompany.DArea;

           para[26] = new SqlParameter();
           para[26].ParameterName = "DState";
           para[26].SqlDbType = SqlDbType.VarChar;
           para[26].Value = objCompany.DState;

           para[27] = new SqlParameter();
           para[27].ParameterName = "MileRate";
           para[27].SqlDbType = SqlDbType.Float;
           para[27].Value = objCompany.MileRate;

           para[28] = new SqlParameter();
           para[28].ParameterName = "PriceD1";
           para[28].SqlDbType = SqlDbType.Decimal;
           para[28].Value = objCompany.PriceD1;

           para[29] = new SqlParameter();
           para[29].ParameterName = "PriceD2";
           para[29].SqlDbType = SqlDbType.Decimal;
           para[29].Value = objCompany.PriceD2;

           para[30] = new SqlParameter();
           para[30].ParameterName = "PriceD3";
           para[30].SqlDbType = SqlDbType.Decimal;
           para[30].Value = objCompany.PriceD3;

           para[31] = new SqlParameter();
           para[31].ParameterName = "PriceD4";
           para[31].SqlDbType = SqlDbType.Decimal;
           para[31].Value = objCompany.PriceD4;

           para[32] = new SqlParameter();
           para[32].ParameterName = "PriceD5";
           para[32].SqlDbType = SqlDbType.Decimal;
           para[32].Value = objCompany.PriceD5;

           para[33] = new SqlParameter();
           para[33].ParameterName = "UTaxR";
           para[33].SqlDbType = SqlDbType.Int;
           para[33].Value = objCompany.UTaxR;

           para[34] = new SqlParameter();
           para[34].ParameterName = "UTax";
           para[34].SqlDbType = SqlDbType.VarChar;
           para[34].Value = objCompany.UTax;

           para[35] = new SqlParameter();
           para[35].ParameterName = "Status";
           para[35].SqlDbType = SqlDbType.Int;
           para[35].Value = objCompany.Status;

           para[36] = new SqlParameter();
           para[36].ParameterName = "Company";
           para[36].SqlDbType = SqlDbType.VarChar;
           para[36].Value = objCompany.Company;

           para[37] = new SqlParameter();
           para[37].ParameterName = "ChargeInt";
           para[37].SqlDbType = SqlDbType.VarChar;
           para[37].Value = objCompany.ChargeInt;
           try
           {
               //CompOffID = Convert.ToInt32(SqlHelper.ExecuteScalar(objCompany.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para));                
               SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, CommandType.StoredProcedure, "spUpdateOffice", para);
               CompOffID = Convert.ToInt32(para[0].Value);
               objCompany.ID = CompOffID;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

        public DataSet getTerritory(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetTerritory", objCompany.ID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getRoute(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetRoute", objCompany.ID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getZone(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetZone", objCompany.ID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getSTax(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetSTax", objCompany.ID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getUseTax(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetUseTax", objCompany.ID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getLocType(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetLocType", objCompany.ID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getARTerms(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetARTerms", objCompany.ID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCompanyByCompanyUserID(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetCompanyByCompanyUserID", objCompany.UserID, objCompany.CompanyID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCompanyByUserID(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetCompanyByUserID", objCompany.UserID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getCompanyByUserID(GetCompanyByUserIDParam _GetCompanyByUserID, string ConnectionString)
        {
            try
            {
                return _GetCompanyByUserID.Ds = SqlHelper.ExecuteDataset(ConnectionString, "spGetCompanyByUserID", _GetCompanyByUserID.UserID, _GetCompanyByUserID.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCompanyUserCoID(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetCompanyUserCoID", objCompany.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getUserDefaultCompany(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetUserDefaultCompany", objCompany.UserID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserDefaultCompany(getUserDefaultCompanyParam _getUserDefaultCompanyParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetUserDefaultCompany", _getUserDefaultCompanyParam.UserID, _getUserDefaultCompanyParam.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateUserCompany(CompanyOffice objCompany)
        {
            int CompOffID;
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objCompany.UserID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "EN";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objCompany.CompanyID;

            try
            {      
                SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, CommandType.StoredProcedure, "spUpdateUserCompanyID", para);
                CompOffID = Convert.ToInt32(para[0].Value);
                objCompany.UserID = CompOffID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UserCompanyAccess(CompanyOffice objCompany)
        {
            int CompOffID;
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objCompany.ID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "IsSel";
            para[1].SqlDbType = SqlDbType.Bit;
            para[1].Value = objCompany.IsSel;

            try
            {
                SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, CommandType.StoredProcedure, "spUpdateUserCompany", para);
                CompOffID = Convert.ToInt32(para[0].Value);
                objCompany.ID= CompOffID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UserCompanyReset(CompanyOffice objCompany)
        {
            int CompOffID;
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objCompany.ID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "IsSel";
            para[1].SqlDbType = SqlDbType.Bit;
            para[1].Value = objCompany.IsSel;

            para[2] = new SqlParameter();
            para[2].ParameterName = "UserID";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objCompany.UserID;

            try
            {
                SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, CommandType.StoredProcedure, "spUpdateUserCompanyReset", para);
                CompOffID = Convert.ToInt32(para[0].Value);
                objCompany.ID = CompOffID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCompanyByCustomer(CompanyOffice objCompany)
        {
            try
            {
                return objCompany.Ds = SqlHelper.ExecuteDataset(objCompany.ConnConfig, "spGetCompanyByCustomer", objCompany.UserID, objCompany.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetCompanyByCustomer", _GetCompanyByCustomerParam.UserID, _GetCompanyByCustomerParam.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCompanyUserCo(CompanyOffice objCompany)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objCompany.ConnConfig, "spDeleteCompanyUserCoByUserID", objCompany.UserID, objCompany.CompanyID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
