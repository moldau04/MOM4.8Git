using BusinessEntity;
using BusinessEntity.APModels;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace DataLayer
{
    public class DL_Inventory
    {
        public DataSet GetInventory(Inventory _objInv)
        {
            DataSet ds = new DataSet();
            try
            {


                ds = SqlHelper.ExecuteDataset(_objInv.ConnConfig,  Inventory.GET_ALL_INVENTORY,null, _objInv.EN, _objInv.UserID, _objInv.Status);



                _objInv.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetInventory(string ConnectionString, GetInventoryParam _GetInventoryParam)
        {
            DataSet ds = new DataSet();
            try
            {


                ds = SqlHelper.ExecuteDataset(ConnectionString, _GetInventoryParam.GET_ALL_INVENTORY, null, _GetInventoryParam.EN, _GetInventoryParam.UserID, _GetInventoryParam.Status);



                _GetInventoryParam.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetSearchInventory(Inventory _objInv )
        {
            DataSet ds = new DataSet();
            try
            {


                ds = SqlHelper.ExecuteDataset(_objInv.ConnConfig, Inventory.GET_Search_INVENTORY, null, _objInv.EN, _objInv.UserID, _objInv.SearchField,_objInv.SearchValue , _objInv.Status);



                _objInv.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetSearchInventory(string ConnectionString, GetSearchInventoryParam _GetSearchInventoryParam)
        {
            DataSet ds = new DataSet();
            try
            {


                ds = SqlHelper.ExecuteDataset(ConnectionString, _GetSearchInventoryParam.GET_Search_INVENTORY, null, _GetSearchInventoryParam.EN, _GetSearchInventoryParam.UserID, _GetSearchInventoryParam.SearchField, _GetSearchInventoryParam.SearchValue, _GetSearchInventoryParam.Status);



                _GetSearchInventoryParam.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetALLInventory(Inventory _objInv)
        {
            DataSet ds = null;
            try
            {


                ds = SqlHelper.ExecuteDataset(_objInv.ConnConfig,  Inventory.GET_ALL_INVENTORY, null, _objInv.EN, _objInv.UserID, _objInv.Status);



                _objInv.Ds = ds;

                HttpContext.Current.Session["Inventory"] = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetALLInventory(string ConnectionString, GetALLInventoryParam _GetALLInventoryParam)
        {
            DataSet ds = null;
            try
            {


                ds = SqlHelper.ExecuteDataset(ConnectionString, _GetALLInventoryParam.GET_ALL_INVENTORY, null, _GetALLInventoryParam.EN, _GetALLInventoryParam.UserID, _GetALLInventoryParam.Status);



                _GetALLInventoryParam.Ds = ds;

               // System.Web.HttpContext.Current.Session["Inventory"] = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetInventoryByID(Inventory _objInv)
        {
            DataSet ds = null;
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_ALL_INVENTORY_BY_ID, _objInv.ID, _objInv.EN, _objInv.UserID);



                _objInv.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        //API
        public DataSet GetInventoryByID(GetInventoryByIDParam _GetInventoryByIDParam)
        {
            DataSet ds = null;
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, _GetInventoryByIDParam.GET_ALL_INVENTORY_BY_ID, _GetInventoryByIDParam.ID, _GetInventoryByIDParam.EN, _GetInventoryByIDParam.UserID);



                _GetInventoryByIDParam.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetInventoryTransactionByInvID(Inventory _objInv)
        {
            DataSet ds = null;
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET__INVENTORY_TRANSACTION_BYINVID, _objInv.ID, _objInv.FromDate, _objInv.EndDate, _objInv.EN, _objInv.UserID);



                _objInv.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        //API
        public DataSet GetInventoryTransactionByInvID(GetInventoryTransactionByInvIDParam _GetInventoryTransactionByInvIDParam, string ConnectionString)
        {
            DataSet ds = null;
            try
            {

                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                ds = SqlHelper.ExecuteDataset(constring, _GetInventoryTransactionByInvIDParam.GET__INVENTORY_TRANSACTION_BYINVID, _GetInventoryTransactionByInvIDParam.ID, _GetInventoryTransactionByInvIDParam.FromDate, _GetInventoryTransactionByInvIDParam.EndDate, _GetInventoryTransactionByInvIDParam.EN, _GetInventoryTransactionByInvIDParam.UserID);



                _GetInventoryTransactionByInvIDParam.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet DeleteInventory(string inventoryxml)
        {
            DataSet ds = null;
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_DELETE_INVENTORY_BULK, constring);


                Inventory _objInv = new Inventory();
                _objInv.ConnConfig = constring;
                HttpContext.Current.Session["Inventory"] = GetALLInventory(_objInv);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;

        }

        public String DeleteInventoryByInvID(Inventory _objInv)
        {
            String Retval = null;
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();
                Retval = Convert.ToString(SqlHelper.ExecuteScalar(constring, Inventory.GET_DELETE_INVENTORY, _objInv.ID));


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retval;

        }

        public String DeleteInventoryByInvID(string ConnectionString, DeleteInventoryByInvIDParam _DeleteInventoryByInvIDParam)
        {
            String Retval = null;
            try
            {

               // string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                Retval = Convert.ToString(SqlHelper.ExecuteScalar(constring, Inventory.GET_DELETE_INVENTORY, _DeleteInventoryByInvIDParam.ID));


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Retval;

        }


        public string GetPartNumber(string strInventory)
        {
            string constring = HttpContext.Current.Session["config"].ToString();
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT Name FROM Inv where Name = '" + strInventory.Trim() + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public string GetPartNumber(GetPartNumberParam _GetPartNumberParam, string ConnectionString)
        {
            //string constring = HttpContext.Current.Session["config"].ToString();
            string constring = ConnectionString;
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT Name FROM Inv where Name = '" + _GetPartNumberParam.strInventory.Trim() + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ChkWareHouseExistForInv(int inv)
        {
            //bool isExist = false;
            string constring = HttpContext.Current.Session["config"].ToString();
            try
            {
                //return Convert.ToBoolean(SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT CAST( CASE WHEN EXISTS(SELECT Name FROM Inv where Name = '" + strInventory + "')THEN 1  ELSE 0  END AS BIT)"));
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT CASE WHEN ((SELECT COUNT(WareHouseID)  FROM InvWarehouse WHERE InvID = '"+ inv +"') > 0) THEN 1 ELSE 0 END"));
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public bool chkInvForOpen(int inv)
        {
            string constring = HttpContext.Current.Session["config"].ToString();
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@invID",
                    SqlDbType = SqlDbType.Int,
                    Value = inv
                };
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(constring, CommandType.StoredProcedure, "sp_ChkInvForOpen", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        //API
        public bool chkInvForOpen(checkkInvForOpenParam _checkkInvForOpenParam, string ConnectionString)
        {
            //string constring = HttpContext.Current.Session["config"].ToString();
            string constring = ConnectionString;
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@invID",
                    SqlDbType = SqlDbType.Int,
                    Value = _checkkInvForOpenParam.invID
                };
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(constring, CommandType.StoredProcedure, "sp_ChkInvForOpen", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet chkStatusOfChart(int ID)
        {
            string constring = HttpContext.Current.Session["config"].ToString();
            DataSet ds = new DataSet();
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = ID
                };
                ds = SqlHelper.ExecuteDataset(constring, "sp_ChkStatusOfChart", ID);
                return ds;
//                return Convert.ToInt32(SqlHelper.ExecuteScalar(constring, CommandType.StoredProcedure, "sp_ChkStatusOfChart", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet chkStatusOfChart(chkStatusOfChartParam _chkStatusOfChartParam, string ConnectionString)
        {
            //string constring = HttpContext.Current.Session["config"].ToString();
            string constring = ConnectionString;
            DataSet ds = new DataSet();
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _chkStatusOfChartParam.ID
                };
                ds = SqlHelper.ExecuteDataset(constring, "sp_ChkStatusOfChart", _chkStatusOfChartParam.ID);
                return ds;
                //                return Convert.ToInt32(SqlHelper.ExecuteScalar(constring, CommandType.StoredProcedure, "sp_ChkStatusOfChart", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckWarehouseIsActive(string WarehouseID)
        {
            string constring = HttpContext.Current.Session["config"].ToString();
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@WarehouseID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = WarehouseID
                };
                return Convert.ToInt32(SqlHelper.ExecuteScalar(constring, CommandType.StoredProcedure, "sp_CheckWarehouseIsActive", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public int CheckWarehouseIsActive(CheckWarehouseIsActiveParam _CheckWarehouseIsActiveParam, string ConnectionString)
        {
            //string constring = HttpContext.Current.Session["config"].ToString();
            string constring = ConnectionString;
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@WarehouseID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _CheckWarehouseIsActiveParam.WarehouseID
                };
                return Convert.ToInt32(SqlHelper.ExecuteScalar(constring, CommandType.StoredProcedure, "sp_CheckWarehouseIsActive", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Inventory CreateInventory(string xml, Inventory inv)
        {
            Inventory success = inv;

            #region All parameters
            SqlParameter[] para = new SqlParameter[61];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Name";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = inv.Name;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fDesc";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = inv.fDesc;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Part";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = inv.Part;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = inv.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "SAcct";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = inv.SAcct;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Measure";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = inv.Measure;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Tax";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = inv.Tax;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Balance";
            para[7].SqlDbType = SqlDbType.Decimal;
            para[7].Value = inv.Balance;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Price1";
            para[8].SqlDbType = SqlDbType.Decimal;
            para[8].Value = inv.Price1;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Price2";
            para[9].SqlDbType = SqlDbType.Decimal;
            para[9].Value = inv.Price2;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Price3";
            para[10].SqlDbType = SqlDbType.Decimal;
            para[10].Value = inv.Price3;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Price4";
            para[11].SqlDbType = SqlDbType.Decimal;
            para[11].Value = inv.Price4;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Price5";
            para[12].SqlDbType = SqlDbType.Decimal;
            para[12].Value = inv.Price5;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Remarks";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = inv.Remarks;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Cat";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = inv.Cat;

            para[15] = new SqlParameter();
            para[15].ParameterName = "LVendor";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = inv.LVendor;

            para[16] = new SqlParameter();
            para[16].ParameterName = "LCost";
            para[16].SqlDbType = SqlDbType.Decimal;
            para[16].Value = inv.LCost;

            para[17] = new SqlParameter();
            para[17].ParameterName = "AllowZero";
            para[17].SqlDbType = SqlDbType.SmallInt;
            para[17].Value = inv.AllowZero;

            para[18] = new SqlParameter();
            para[18].ParameterName = "InUse";
            para[18].SqlDbType = SqlDbType.SmallInt;
            para[18].Value = inv.InUse;

            para[19] = new SqlParameter();
            para[19].ParameterName = "EN";
            para[19].SqlDbType = SqlDbType.Int;
            para[19].Value = inv.EN;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Hand";
            para[20].SqlDbType = SqlDbType.Decimal;
            para[20].Value = inv.Hand;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Aisle";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = inv.Aisle;

            para[22] = new SqlParameter();
            para[22].ParameterName = "fOrder";
            para[22].SqlDbType = SqlDbType.Decimal;
            para[22].Value = inv.fOrder;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Min";
            para[23].SqlDbType = SqlDbType.Decimal;
            para[23].Value = inv.Min;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Shelf";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = inv.Shelf;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Bin";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = inv.Bin;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Requ";
            para[26].SqlDbType = SqlDbType.Decimal;
            para[26].Value = inv.Requ;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Warehouse";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = inv.Warehouse;

            para[28] = new SqlParameter();
            para[28].ParameterName = "Price6";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = inv.Price6;

            para[29] = new SqlParameter();
            para[29].ParameterName = "Committed";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = inv.Committed;

            para[30] = new SqlParameter();
            para[30].ParameterName = "Available";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = inv.Available;

            para[31] = new SqlParameter();
            para[31].ParameterName = "IssuedOpenJobs";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = inv.IssuedOpenJobs;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Specification";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = inv.Specification;

            para[33] = new SqlParameter();
            para[33].ParameterName = "Revision";
            para[33].SqlDbType = SqlDbType.VarChar;
            para[33].Value = inv.Revision;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Eco";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = inv.Eco;

            para[35] = new SqlParameter();
            para[35].ParameterName = "Drawing";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = inv.Drawing;

            para[36] = new SqlParameter();
            para[36].ParameterName = "ShelfLife";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = inv.ShelfLife;

            para[37] = new SqlParameter();
            para[37].ParameterName = "GLcogs";
            para[37].SqlDbType = SqlDbType.VarChar;
            para[37].Value = inv.GLcogs;

            para[38] = new SqlParameter();
            para[38].ParameterName = "GLPurchases";
            para[38].SqlDbType = SqlDbType.VarChar;
            para[38].Value = inv.GLPurchases;

            para[39] = new SqlParameter();
            para[39].ParameterName = "ABCClass";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = inv.ABCClass;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OHValue";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = inv.OHValue;

            para[41] = new SqlParameter();
            para[41].ParameterName = "OOValue";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = inv.OOValue;

            para[42] = new SqlParameter();
            para[42].ParameterName = "OverIssueAllowance";
            para[42].SqlDbType = SqlDbType.Bit;
            para[42].Value = inv.OverIssueAllowance;

            para[43] = new SqlParameter();
            para[43].ParameterName = "UnderIssueAllowance";
            para[43].SqlDbType = SqlDbType.Bit;
            para[43].Value = inv.UnderIssueAllowance;

            para[44] = new SqlParameter();
            para[44].ParameterName = "InventoryTurns";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = inv.InventoryTurns;

            para[45] = new SqlParameter();
            para[45].ParameterName = "MOQ";
            para[45].SqlDbType = SqlDbType.Decimal;
            para[45].Value = inv.MOQ;

            para[46] = new SqlParameter();
            para[46].ParameterName = "MinInvQty";
            para[46].SqlDbType = SqlDbType.Decimal;
            para[46].Value = inv.MinInvQty;

            para[47] = new SqlParameter();
            para[47].ParameterName = "MaxInvQty";
            para[47].SqlDbType = SqlDbType.Decimal;
            para[47].Value = inv.MaxInvQty;

            para[48] = new SqlParameter();
            para[48].ParameterName = "Commodity";
            para[48].SqlDbType = SqlDbType.VarChar;
            para[48].Value = inv.Commodity;

            para[49] = new SqlParameter();
            para[49].ParameterName = "LastReceiptDate";
            para[49].SqlDbType = SqlDbType.DateTime;
            if (inv.LastReceiptDate != null && inv.LastReceiptDate != System.DateTime.MinValue)
            {
                para[49].Value = inv.LastReceiptDate;
            }


            para[50] = new SqlParameter();
            para[50].ParameterName = "EAU";
            para[50].SqlDbType = SqlDbType.Decimal;
            para[50].Value = inv.EAU;

            para[51] = new SqlParameter();
            para[51].ParameterName = "EOLDate";
            para[51].SqlDbType = SqlDbType.DateTime;
            if (inv.EOLDate != null && inv.EOLDate != System.DateTime.MinValue)
            {
                para[51].Value = inv.EOLDate;
            }


            para[52] = new SqlParameter();
            para[52].ParameterName = "WarrantyPeriod";
            para[52].SqlDbType = SqlDbType.Int;
            para[52].Value = inv.WarrantyPeriod;

            para[53] = new SqlParameter();
            para[53].ParameterName = "PODueDate";
            para[53].SqlDbType = SqlDbType.DateTime;
            if (inv.PODueDate != null && inv.PODueDate != System.DateTime.MinValue)
            {
                para[53].Value = inv.PODueDate;
            }


            para[54] = new SqlParameter();
            para[54].ParameterName = "DefaultReceivingLocation";
            para[54].SqlDbType = SqlDbType.Bit;
            para[54].Value = inv.DefaultReceivingLocation;

            para[55] = new SqlParameter();
            para[55].ParameterName = "DefaultInspectionLocation";
            para[55].SqlDbType = SqlDbType.Bit;
            para[55].Value = inv.DefaultInspectionLocation;

            para[56] = new SqlParameter();
            para[56].ParameterName = "QtyAllocatedToSO";
            para[56].SqlDbType = SqlDbType.Decimal;
            para[56].Value = inv.QtyAllocatedToSO;

            para[57] = new SqlParameter();
            para[57].ParameterName = "UnitCost";
            para[57].SqlDbType = SqlDbType.Decimal;
            para[57].Value = inv.UnitCost;

            para[58] = new SqlParameter();
            para[58].ParameterName = "GLSales";
            para[58].SqlDbType = SqlDbType.Int;
            para[58].Value = inv.GLSales;

            para[59] = new SqlParameter();
            para[59].ParameterName = "EOQ";
            para[59].SqlDbType = SqlDbType.Decimal;
            para[59].Value = inv.EOQ;

            para[60] = new SqlParameter();
            para[60].ParameterName = "LeadTime";
            para[60].SqlDbType = SqlDbType.Int;
            para[60].Value = inv.LeadTime;

            //para[61] = new SqlParameter();
            //para[61].ParameterName = "Type";
            //para[61].SqlDbType = SqlDbType.SmallInt;
            //para[61].Value = inv.Type;

            //para[62] = new SqlParameter();
            //para[62].ParameterName = "LastUpdateDate";
            //para[62].SqlDbType = SqlDbType.DateTime;
            //if (inv.LastUpdateDate != null && inv.LastUpdateDate != System.DateTime.MinValue)
            //{
            //    para[62].Value = inv.LastUpdateDate;
            //}

            //para[63] = new SqlParameter();
            //para[63].ParameterName = "DateCreated";
            //para[63].SqlDbType = SqlDbType.DateTime;
            //if (inv.DateCreated != null && inv.DateCreated != System.DateTime.MinValue)
            //{
            //    para[63].Value = inv.DateCreated;
            //}

            //para[64] = new SqlParameter();
            //para[64].ParameterName = "LastRevisionDate";
            //para[64].SqlDbType = SqlDbType.DateTime;
            //if (inv.LastRevisionDate != null && inv.LastRevisionDate != System.DateTime.MinValue)
            //{
            //    para[64].Value = inv.LastRevisionDate;
            //}









            #endregion

            try
            {


                string constring = HttpContext.Current.Session["config"].ToString();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventoryCreate"))
                    {
                        try
                        {

                            int id = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_XML, xml);
                            //   int id = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_XML, para);
                            success.ID = id;
                            if (inv.InvPartslist != null)
                            {
                                foreach (InvParts _objManInv in inv.InvPartslist)
                                {
                                    _objManInv.ItemID = inv.ID;



                                    _objManInv.ID = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_PARTS, _objManInv.ItemID, _objManInv.MPN, _objManInv.Part, _objManInv.Supplier, _objManInv.VendorID, _objManInv.Price, _objManInv.Mfg, _objManInv.MfgPrice);
                                }
                            }
                            if (inv.InvItemRevlist != null)
                            {
                                foreach (InvItemRev _objInvItemRev in inv.InvItemRevlist)
                                {
                                    _objInvItemRev.InvID = inv.ID;
                                    CreateItemRevision(_objInvItemRev);
                                }
                            }


                            tans.Commit();


                        }
                        catch (Exception ex)
                        {

                            tans.Rollback("TransactionInventoryCreate");
                        }
                    }

                }










            }
            catch (Exception ex)
            {

                throw ex;
            }


            return success;

        }

        //public InventoryViewModel CreateInventory(string xml, CreateInventoryParam _CreateInventoryParam, string ConnectionString)
        //{
        //    //Inventory success = _CreateInventoryParam;
        //    InventoryViewModel success = _CreateInventoryParam.InventoryViewModel;

        //    #region All parameters
        //    SqlParameter[] para = new SqlParameter[61];

        //    para[0] = new SqlParameter();
        //    para[0].ParameterName = "Name";
        //    para[0].SqlDbType = SqlDbType.VarChar;
        //    para[0].Value = _CreateInventoryParam.Name;

        //    para[1] = new SqlParameter();
        //    para[1].ParameterName = "fDesc";
        //    para[1].SqlDbType = SqlDbType.VarChar;
        //    para[1].Value = _CreateInventoryParam.fDesc;

        //    para[2] = new SqlParameter();
        //    para[2].ParameterName = "Part";
        //    para[2].SqlDbType = SqlDbType.VarChar;
        //    para[2].Value = _CreateInventoryParam.Part;

        //    para[3] = new SqlParameter();
        //    para[3].ParameterName = "Status";
        //    para[3].SqlDbType = SqlDbType.SmallInt;
        //    para[3].Value = _CreateInventoryParam.Status;

        //    para[4] = new SqlParameter();
        //    para[4].ParameterName = "SAcct";
        //    para[4].SqlDbType = SqlDbType.Int;
        //    para[4].Value = _CreateInventoryParam.SAcct;

        //    para[5] = new SqlParameter();
        //    para[5].ParameterName = "Measure";
        //    para[5].SqlDbType = SqlDbType.VarChar;
        //    para[5].Value = _CreateInventoryParam.Measure;

        //    para[6] = new SqlParameter();
        //    para[6].ParameterName = "Tax";
        //    para[6].SqlDbType = SqlDbType.VarChar;
        //    para[6].Value = _CreateInventoryParam.Tax;

        //    para[7] = new SqlParameter();
        //    para[7].ParameterName = "Balance";
        //    para[7].SqlDbType = SqlDbType.Decimal;
        //    para[7].Value = _CreateInventoryParam.Balance;

        //    para[8] = new SqlParameter();
        //    para[8].ParameterName = "Price1";
        //    para[8].SqlDbType = SqlDbType.Decimal;
        //    para[8].Value = _CreateInventoryParam.Price1;

        //    para[9] = new SqlParameter();
        //    para[9].ParameterName = "Price2";
        //    para[9].SqlDbType = SqlDbType.Decimal;
        //    para[9].Value = _CreateInventoryParam.Price2;

        //    para[10] = new SqlParameter();
        //    para[10].ParameterName = "Price3";
        //    para[10].SqlDbType = SqlDbType.Decimal;
        //    para[10].Value = _CreateInventoryParam.Price3;

        //    para[11] = new SqlParameter();
        //    para[11].ParameterName = "Price4";
        //    para[11].SqlDbType = SqlDbType.Decimal;
        //    para[11].Value = _CreateInventoryParam.Price4;

        //    para[12] = new SqlParameter();
        //    para[12].ParameterName = "Price5";
        //    para[12].SqlDbType = SqlDbType.Decimal;
        //    para[12].Value = _CreateInventoryParam.Price5;

        //    para[13] = new SqlParameter();
        //    para[13].ParameterName = "Remarks";
        //    para[13].SqlDbType = SqlDbType.VarChar;
        //    para[13].Value = _CreateInventoryParam.Remarks;

        //    para[14] = new SqlParameter();
        //    para[14].ParameterName = "Cat";
        //    para[14].SqlDbType = SqlDbType.Int;
        //    para[14].Value = _CreateInventoryParam.Cat;

        //    para[15] = new SqlParameter();
        //    para[15].ParameterName = "LVendor";
        //    para[15].SqlDbType = SqlDbType.Int;
        //    para[15].Value = _CreateInventoryParam.LVendor;

        //    para[16] = new SqlParameter();
        //    para[16].ParameterName = "LCost";
        //    para[16].SqlDbType = SqlDbType.Decimal;
        //    para[16].Value = _CreateInventoryParam.LCost;

        //    para[17] = new SqlParameter();
        //    para[17].ParameterName = "AllowZero";
        //    para[17].SqlDbType = SqlDbType.SmallInt;
        //    para[17].Value = _CreateInventoryParam.AllowZero;

        //    para[18] = new SqlParameter();
        //    para[18].ParameterName = "InUse";
        //    para[18].SqlDbType = SqlDbType.SmallInt;
        //    para[18].Value = _CreateInventoryParam.InUse;

        //    para[19] = new SqlParameter();
        //    para[19].ParameterName = "EN";
        //    para[19].SqlDbType = SqlDbType.Int;
        //    para[19].Value = _CreateInventoryParam.EN;

        //    para[20] = new SqlParameter();
        //    para[20].ParameterName = "Hand";
        //    para[20].SqlDbType = SqlDbType.Decimal;
        //    para[20].Value = _CreateInventoryParam.Hand;

        //    para[21] = new SqlParameter();
        //    para[21].ParameterName = "Aisle";
        //    para[21].SqlDbType = SqlDbType.VarChar;
        //    para[21].Value = _CreateInventoryParam.Aisle;

        //    para[22] = new SqlParameter();
        //    para[22].ParameterName = "fOrder";
        //    para[22].SqlDbType = SqlDbType.Decimal;
        //    para[22].Value = _CreateInventoryParam.fOrder;

        //    para[23] = new SqlParameter();
        //    para[23].ParameterName = "Min";
        //    para[23].SqlDbType = SqlDbType.Decimal;
        //    para[23].Value = _CreateInventoryParam.Min;

        //    para[24] = new SqlParameter();
        //    para[24].ParameterName = "Shelf";
        //    para[24].SqlDbType = SqlDbType.VarChar;
        //    para[24].Value = _CreateInventoryParam.Shelf;

        //    para[25] = new SqlParameter();
        //    para[25].ParameterName = "Bin";
        //    para[25].SqlDbType = SqlDbType.VarChar;
        //    para[25].Value = _CreateInventoryParam.Bin;

        //    para[26] = new SqlParameter();
        //    para[26].ParameterName = "Requ";
        //    para[26].SqlDbType = SqlDbType.Decimal;
        //    para[26].Value = _CreateInventoryParam.Requ;

        //    para[27] = new SqlParameter();
        //    para[27].ParameterName = "Warehouse";
        //    para[27].SqlDbType = SqlDbType.VarChar;
        //    para[27].Value = _CreateInventoryParam.Warehouse;

        //    para[28] = new SqlParameter();
        //    para[28].ParameterName = "Price6";
        //    para[28].SqlDbType = SqlDbType.Decimal;
        //    para[28].Value = _CreateInventoryParam.Price6;

        //    para[29] = new SqlParameter();
        //    para[29].ParameterName = "Committed";
        //    para[29].SqlDbType = SqlDbType.Decimal;
        //    para[29].Value = _CreateInventoryParam.Committed;

        //    para[30] = new SqlParameter();
        //    para[30].ParameterName = "Available";
        //    para[30].SqlDbType = SqlDbType.Decimal;
        //    para[30].Value = _CreateInventoryParam.Available;

        //    para[31] = new SqlParameter();
        //    para[31].ParameterName = "IssuedOpenJobs";
        //    para[31].SqlDbType = SqlDbType.Decimal;
        //    para[31].Value = _CreateInventoryParam.IssuedOpenJobs;

        //    para[32] = new SqlParameter();
        //    para[32].ParameterName = "Specification";
        //    para[32].SqlDbType = SqlDbType.VarChar;
        //    para[32].Value = _CreateInventoryParam.Specification;

        //    para[33] = new SqlParameter();
        //    para[33].ParameterName = "Revision";
        //    para[33].SqlDbType = SqlDbType.VarChar;
        //    para[33].Value = _CreateInventoryParam.Revision;

        //    para[34] = new SqlParameter();
        //    para[34].ParameterName = "Eco";
        //    para[34].SqlDbType = SqlDbType.VarChar;
        //    para[34].Value = _CreateInventoryParam.Eco;

        //    para[35] = new SqlParameter();
        //    para[35].ParameterName = "Drawing";
        //    para[35].SqlDbType = SqlDbType.VarChar;
        //    para[35].Value = _CreateInventoryParam.Drawing;

        //    para[36] = new SqlParameter();
        //    para[36].ParameterName = "ShelfLife";
        //    para[36].SqlDbType = SqlDbType.Decimal;
        //    para[36].Value = _CreateInventoryParam.ShelfLife;

        //    para[37] = new SqlParameter();
        //    para[37].ParameterName = "GLcogs";
        //    para[37].SqlDbType = SqlDbType.VarChar;
        //    para[37].Value = _CreateInventoryParam.GLcogs;

        //    para[38] = new SqlParameter();
        //    para[38].ParameterName = "GLPurchases";
        //    para[38].SqlDbType = SqlDbType.VarChar;
        //    para[38].Value = _CreateInventoryParam.GLPurchases;

        //    para[39] = new SqlParameter();
        //    para[39].ParameterName = "ABCClass";
        //    para[39].SqlDbType = SqlDbType.VarChar;
        //    para[39].Value = _CreateInventoryParam.ABCClass;

        //    para[40] = new SqlParameter();
        //    para[40].ParameterName = "OHValue";
        //    para[40].SqlDbType = SqlDbType.Decimal;
        //    para[40].Value = _CreateInventoryParam.OHValue;

        //    para[41] = new SqlParameter();
        //    para[41].ParameterName = "OOValue";
        //    para[41].SqlDbType = SqlDbType.Decimal;
        //    para[41].Value = _CreateInventoryParam.OOValue;

        //    para[42] = new SqlParameter();
        //    para[42].ParameterName = "OverIssueAllowance";
        //    para[42].SqlDbType = SqlDbType.Bit;
        //    para[42].Value = _CreateInventoryParam.OverIssueAllowance;

        //    para[43] = new SqlParameter();
        //    para[43].ParameterName = "UnderIssueAllowance";
        //    para[43].SqlDbType = SqlDbType.Bit;
        //    para[43].Value = _CreateInventoryParam.UnderIssueAllowance;

        //    para[44] = new SqlParameter();
        //    para[44].ParameterName = "_CreateInventoryParamentoryTurns";
        //    para[44].SqlDbType = SqlDbType.Decimal;
        //    para[44].Value = _CreateInventoryParam.InventoryTurns;

        //    para[45] = new SqlParameter();
        //    para[45].ParameterName = "MOQ";
        //    para[45].SqlDbType = SqlDbType.Decimal;
        //    para[45].Value = _CreateInventoryParam.MOQ;

        //    para[46] = new SqlParameter();
        //    para[46].ParameterName = "MinInvQty";
        //    para[46].SqlDbType = SqlDbType.Decimal;
        //    para[46].Value = _CreateInventoryParam.MinInvQty;

        //    para[47] = new SqlParameter();
        //    para[47].ParameterName = "MaxInvQty";
        //    para[47].SqlDbType = SqlDbType.Decimal;
        //    para[47].Value = _CreateInventoryParam.MaxInvQty;

        //    para[48] = new SqlParameter();
        //    para[48].ParameterName = "Commodity";
        //    para[48].SqlDbType = SqlDbType.VarChar;
        //    para[48].Value = _CreateInventoryParam.Commodity;

        //    para[49] = new SqlParameter();
        //    para[49].ParameterName = "LastReceiptDate";
        //    para[49].SqlDbType = SqlDbType.DateTime;
        //    if (_CreateInventoryParam.LastReceiptDate != null && _CreateInventoryParam.LastReceiptDate != System.DateTime.MinValue)
        //    {
        //        para[49].Value = _CreateInventoryParam.LastReceiptDate;
        //    }


        //    para[50] = new SqlParameter();
        //    para[50].ParameterName = "EAU";
        //    para[50].SqlDbType = SqlDbType.Decimal;
        //    para[50].Value = _CreateInventoryParam.EAU;

        //    para[51] = new SqlParameter();
        //    para[51].ParameterName = "EOLDate";
        //    para[51].SqlDbType = SqlDbType.DateTime;
        //    if (_CreateInventoryParam.EOLDate != null && _CreateInventoryParam.EOLDate != System.DateTime.MinValue)
        //    {
        //        para[51].Value = _CreateInventoryParam.EOLDate;
        //    }


        //    para[52] = new SqlParameter();
        //    para[52].ParameterName = "WarrantyPeriod";
        //    para[52].SqlDbType = SqlDbType.Int;
        //    para[52].Value = _CreateInventoryParam.WarrantyPeriod;

        //    para[53] = new SqlParameter();
        //    para[53].ParameterName = "PODueDate";
        //    para[53].SqlDbType = SqlDbType.DateTime;
        //    if (_CreateInventoryParam.PODueDate != null && _CreateInventoryParam.PODueDate != System.DateTime.MinValue)
        //    {
        //        para[53].Value = _CreateInventoryParam.PODueDate;
        //    }


        //    para[54] = new SqlParameter();
        //    para[54].ParameterName = "DefaultReceivingLocation";
        //    para[54].SqlDbType = SqlDbType.Bit;
        //    para[54].Value = _CreateInventoryParam.DefaultReceivingLocation;

        //    para[55] = new SqlParameter();
        //    para[55].ParameterName = "DefaultInspectionLocation";
        //    para[55].SqlDbType = SqlDbType.Bit;
        //    para[55].Value = _CreateInventoryParam.DefaultInspectionLocation;

        //    para[56] = new SqlParameter();
        //    para[56].ParameterName = "QtyAllocatedToSO";
        //    para[56].SqlDbType = SqlDbType.Decimal;
        //    para[56].Value = _CreateInventoryParam.QtyAllocatedToSO;

        //    para[57] = new SqlParameter();
        //    para[57].ParameterName = "UnitCost";
        //    para[57].SqlDbType = SqlDbType.Decimal;
        //    para[57].Value = _CreateInventoryParam.UnitCost;

        //    para[58] = new SqlParameter();
        //    para[58].ParameterName = "GLSales";
        //    para[58].SqlDbType = SqlDbType.Int;
        //    para[58].Value = _CreateInventoryParam.GLSales;

        //    para[59] = new SqlParameter();
        //    para[59].ParameterName = "EOQ";
        //    para[59].SqlDbType = SqlDbType.Decimal;
        //    para[59].Value = _CreateInventoryParam.EOQ;

        //    para[60] = new SqlParameter();
        //    para[60].ParameterName = "LeadTime";
        //    para[60].SqlDbType = SqlDbType.Int;
        //    para[60].Value = _CreateInventoryParam.LeadTime;
        //    #endregion

        //    try
        //    {


        //        string constring = HttpContext.Current.Session["config"].ToString();
        //        using (SqlConnection con = new SqlConnection(constring))
        //        {
        //            con.Open();
        //            using (SqlTransaction tans = con.BeginTransaction("TransactionInventoryCreate"))
        //            {
        //                try
        //                {

        //                    int id = (int)SqlHelper.ExecuteScalar(tans, _CreateInventoryParam.CREATE_INVENTORY_XML, xml);
        //                    //   int id = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_XML, para);
        //                    success.ID = id;
        //                    if (_CreateInventoryParam.InvPartslist != null)
        //                    {
        //                        foreach (InvParts _objManInv in _CreateInventoryParam.InvPartslist)
        //                        {
        //                            _objManInv.ItemID = _CreateInventoryParam.ID;



        //                            _objManInv.ID = (int)SqlHelper.ExecuteScalar(tans, _CreateInventoryParam.CREATE_INVENTORY_PARTS, _objManInv.ItemID, _objManInv.MPN, _objManInv.Part, _objManInv.Supplier, _objManInv.VendorID, _objManInv.Price, _objManInv.Mfg, _objManInv.MfgPrice);
        //                        }
        //                    }

        //                    if (_CreateInventoryParam.InvItemRevlist != null)
        //                    {
        //                        foreach (InvItemRev _objInvItemRev in _CreateInventoryParam.InvItemRevlist)
        //                        {
        //                            _objInvItemRev.InvID = _CreateInventoryParam.ID;
        //                            CreateItemRevision(_objInvItemRev);
        //                        }
        //                    }


        //                    tans.Commit();


        //                }
        //                catch (Exception ex)
        //                {

        //                    tans.Rollback("TransactionInventoryCreate");
        //                }
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }


        //    return success;

        //}

        public void UpdateInventory(Inventory inv)
        {
            #region All parameters
            SqlParameter[] para = new SqlParameter[62];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = inv.ID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fDesc";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = inv.fDesc;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Name";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = inv.Name;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = inv.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "SAcct";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = inv.SAcct;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Measure";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = inv.Measure;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Tax";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = inv.Tax;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Balance";
            para[7].SqlDbType = SqlDbType.Decimal;
            para[7].Value = inv.Balance;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Price1";
            para[8].SqlDbType = SqlDbType.Decimal;
            para[8].Value = inv.Price1;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Price2";
            para[9].SqlDbType = SqlDbType.Decimal;
            para[9].Value = inv.Price2;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Price3";
            para[10].SqlDbType = SqlDbType.Decimal;
            para[10].Value = inv.Price3;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Price4";
            para[11].SqlDbType = SqlDbType.Decimal;
            para[11].Value = inv.Price4;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Price5";
            para[12].SqlDbType = SqlDbType.Decimal;
            para[12].Value = inv.Price5;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Remarks";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = inv.Remarks;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Cat";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = inv.Cat;

            para[15] = new SqlParameter();
            para[15].ParameterName = "LVendor";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = inv.LVendor;

            para[16] = new SqlParameter();
            para[16].ParameterName = "LCost";
            para[16].SqlDbType = SqlDbType.Decimal;
            para[16].Value = inv.LCost;

            para[17] = new SqlParameter();
            para[17].ParameterName = "AllowZero";
            para[17].SqlDbType = SqlDbType.SmallInt;
            para[17].Value = inv.AllowZero;

            para[18] = new SqlParameter();
            para[18].ParameterName = "InUse";
            para[18].SqlDbType = SqlDbType.SmallInt;
            para[18].Value = inv.InUse;

            para[19] = new SqlParameter();
            para[19].ParameterName = "EN";
            para[19].SqlDbType = SqlDbType.Int;
            para[19].Value = inv.EN;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Hand";
            para[20].SqlDbType = SqlDbType.Decimal;
            para[20].Value = inv.Hand;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Aisle";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = inv.Aisle;

            para[22] = new SqlParameter();
            para[22].ParameterName = "fOrder";
            para[22].SqlDbType = SqlDbType.Decimal;
            para[22].Value = inv.fOrder;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Min";
            para[23].SqlDbType = SqlDbType.Decimal;
            para[23].Value = inv.Min;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Shelf";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = inv.Shelf;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Bin";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = inv.Bin;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Requ";
            para[26].SqlDbType = SqlDbType.Decimal;
            para[26].Value = inv.Requ;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Warehouse";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = inv.Warehouse;

            para[28] = new SqlParameter();
            para[28].ParameterName = "Price6";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = inv.Price6;

            para[29] = new SqlParameter();
            para[29].ParameterName = "Committed";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = inv.Committed;

            para[30] = new SqlParameter();
            para[30].ParameterName = "Available";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = inv.Available;

            para[31] = new SqlParameter();
            para[31].ParameterName = "IssuedOpenJobs";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = inv.IssuedOpenJobs;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Specification";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = inv.Specification;

            para[33] = new SqlParameter();
            para[33].ParameterName = "Revision";
            para[33].SqlDbType = SqlDbType.VarChar;
            para[33].Value = inv.Revision;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Eco";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = inv.Eco;

            para[35] = new SqlParameter();
            para[35].ParameterName = "Drawing";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = inv.Drawing;

            para[36] = new SqlParameter();
            para[36].ParameterName = "ShelfLife";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = inv.ShelfLife;

            para[37] = new SqlParameter();
            para[37].ParameterName = "GLcogs";
            para[37].SqlDbType = SqlDbType.VarChar;
            para[37].Value = inv.GLcogs;

            para[38] = new SqlParameter();
            para[38].ParameterName = "GLPurchases";
            para[38].SqlDbType = SqlDbType.VarChar;
            para[38].Value = inv.GLPurchases;

            para[39] = new SqlParameter();
            para[39].ParameterName = "ABCClass";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = inv.ABCClass;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OHValue";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = inv.OHValue;

            para[41] = new SqlParameter();
            para[41].ParameterName = "OOValue";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = inv.OOValue;

            para[42] = new SqlParameter();
            para[42].ParameterName = "OverIssueAllowance";
            para[42].SqlDbType = SqlDbType.Bit;
            para[42].Value = inv.OverIssueAllowance;

            para[43] = new SqlParameter();
            para[43].ParameterName = "UnderIssueAllowance";
            para[43].SqlDbType = SqlDbType.Bit;
            para[43].Value = inv.UnderIssueAllowance;

            para[44] = new SqlParameter();
            para[44].ParameterName = "InventoryTurns";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = inv.InventoryTurns;

            para[45] = new SqlParameter();
            para[45].ParameterName = "MOQ";
            para[45].SqlDbType = SqlDbType.Decimal;
            para[45].Value = inv.MOQ;

            para[46] = new SqlParameter();
            para[46].ParameterName = "MinInvQty";
            para[46].SqlDbType = SqlDbType.Decimal;
            para[46].Value = inv.MinInvQty;

            para[47] = new SqlParameter();
            para[47].ParameterName = "MaxInvQty";
            para[47].SqlDbType = SqlDbType.Decimal;
            para[47].Value = inv.MaxInvQty;

            para[48] = new SqlParameter();
            para[48].ParameterName = "Commodity";
            para[48].SqlDbType = SqlDbType.VarChar;
            para[48].Value = inv.Commodity;

            para[49] = new SqlParameter();
            para[49].ParameterName = "LastReceiptDate";
            para[49].SqlDbType = SqlDbType.DateTime;
            if (inv.LastReceiptDate != null && inv.LastReceiptDate != System.DateTime.MinValue)
            {
                para[49].Value = inv.LastReceiptDate;
            }


            para[50] = new SqlParameter();
            para[50].ParameterName = "EAU";
            para[50].SqlDbType = SqlDbType.Decimal;
            para[50].Value = inv.EAU;

            para[51] = new SqlParameter();
            para[51].ParameterName = "EOLDate";
            para[51].SqlDbType = SqlDbType.DateTime;
            if (inv.EOLDate != null && inv.EOLDate != System.DateTime.MinValue)
            {
                para[51].Value = inv.EOLDate;
            }


            para[52] = new SqlParameter();
            para[52].ParameterName = "WarrantyPeriod";
            para[52].SqlDbType = SqlDbType.Int;
            para[52].Value = inv.WarrantyPeriod;

            para[53] = new SqlParameter();
            para[53].ParameterName = "PODueDate";
            para[53].SqlDbType = SqlDbType.DateTime;
            if (inv.PODueDate != null && inv.PODueDate != System.DateTime.MinValue)
            {
                para[53].Value = inv.PODueDate;
            }


            para[54] = new SqlParameter();
            para[54].ParameterName = "DefaultReceivingLocation";
            para[54].SqlDbType = SqlDbType.Bit;
            para[54].Value = inv.DefaultReceivingLocation;

            para[55] = new SqlParameter();
            para[55].ParameterName = "DefaultInspectionLocation";
            para[55].SqlDbType = SqlDbType.Bit;
            para[55].Value = inv.DefaultInspectionLocation;

            para[56] = new SqlParameter();
            para[56].ParameterName = "QtyAllocatedToSO";
            para[56].SqlDbType = SqlDbType.Decimal;
            para[56].Value = inv.QtyAllocatedToSO;

            para[57] = new SqlParameter();
            para[57].ParameterName = "UnitCost";
            para[57].SqlDbType = SqlDbType.Decimal;
            para[57].Value = inv.UnitCost;

            para[58] = new SqlParameter();
            para[58].ParameterName = "GLSales";
            para[58].SqlDbType = SqlDbType.Int;
            para[58].Value = inv.GLSales;

            para[59] = new SqlParameter();
            para[59].ParameterName = "EOQ";
            para[59].SqlDbType = SqlDbType.Decimal;
            para[59].Value = inv.EOQ;

            para[60] = new SqlParameter();
            para[60].ParameterName = "LeadTime";
            para[60].SqlDbType = SqlDbType.Int;
            para[60].Value = inv.LeadTime;

            para[61] = new SqlParameter();
            para[61].ParameterName = "@Docs";
            para[61].SqlDbType = SqlDbType.Structured;
            para[61].Value = inv.dtDocs;


            //para[62] = new SqlParameter();
            //para[62].ParameterName = "Part";
            //para[62].SqlDbType = SqlDbType.VarChar;
            //para[62].Value = inv.Part;


            #endregion



            try
            {

                DataSet dsAppManuinfo = GetInventoryPartsInfoByInventoryID(inv.ID);

                string constring = HttpContext.Current.Session["config"].ToString();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventory"))
                    {

                        try
                        {

                            //  SqlHelper.ExecuteScalar(tans, Inventory.UPDATE_INVENTORY_XML, xml);

                            SqlHelper.ExecuteScalar(tans, Inventory.UPDATE_INVENTORY_XML, para);


                            if (inv.InvPartslist != null)
                            {
                                foreach (InvParts _objManInv in inv.InvPartslist)
                                {
                                    _objManInv.ItemID = inv.ID;

                                    if (_objManInv.ID != 0)//Update when you have and ID associated with the vendor information
                                        SqlHelper.ExecuteNonQuery(tans, Inventory.UPDATE__INVENTORY_PARTS, _objManInv.ID, _objManInv.ItemID, _objManInv.MPN, _objManInv.Part, _objManInv.Supplier, _objManInv.VendorID, _objManInv.Price, _objManInv.Mfg, _objManInv.MfgPrice);
                                    else//create vendors
                                        _objManInv.ID = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_PARTS, _objManInv.ItemID, _objManInv.MPN, _objManInv.Part, _objManInv.Supplier, _objManInv.VendorID, _objManInv.Price, _objManInv.Mfg, _objManInv.MfgPrice);
                                }

                                //check for the list to be deleted
                                if (dsAppManuinfo != null)
                                {
                                    if (dsAppManuinfo.Tables.Count > 0)
                                    {
                                        if (dsAppManuinfo.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dsAppManuinfo.Tables[0].Rows.Count; i++)
                                            {
                                                int ID = Convert.ToInt32(dsAppManuinfo.Tables[0].Rows[i]["ID"]);

                                                if (!inv.InvPartslist.Any(x => x.ID != 0 & x.ID == ID))
                                                {
                                                    SqlHelper.ExecuteNonQuery(tans, Inventory.DELETE__INVENTORY_PARTS, ID);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            tans.Commit();


                        }
                        catch (Exception ex)
                        {

                            tans.Rollback("TransactionInventory");

                            throw ex;
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        //API
        public void UpdateInventory(UpdateInventoryParam _UpdateInventoryParam, string ConnectionString)
        {
            #region All parameters
            SqlParameter[] para = new SqlParameter[62];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = _UpdateInventoryParam.ID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fDesc";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _UpdateInventoryParam.fDesc;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Name";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = _UpdateInventoryParam.Name;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = _UpdateInventoryParam.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "SAcct";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = _UpdateInventoryParam.SAcct;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Measure";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = _UpdateInventoryParam.Measure;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Tax";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = _UpdateInventoryParam.Tax;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Balance";
            para[7].SqlDbType = SqlDbType.Decimal;
            para[7].Value = _UpdateInventoryParam.Balance;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Price1";
            para[8].SqlDbType = SqlDbType.Decimal;
            para[8].Value = _UpdateInventoryParam.Price1;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Price2";
            para[9].SqlDbType = SqlDbType.Decimal;
            para[9].Value = _UpdateInventoryParam.Price2;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Price3";
            para[10].SqlDbType = SqlDbType.Decimal;
            para[10].Value = _UpdateInventoryParam.Price3;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Price4";
            para[11].SqlDbType = SqlDbType.Decimal;
            para[11].Value = _UpdateInventoryParam.Price4;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Price5";
            para[12].SqlDbType = SqlDbType.Decimal;
            para[12].Value = _UpdateInventoryParam.Price5;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Remarks";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = _UpdateInventoryParam.Remarks;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Cat";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = _UpdateInventoryParam.Cat;

            para[15] = new SqlParameter();
            para[15].ParameterName = "LVendor";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = _UpdateInventoryParam.LVendor;

            para[16] = new SqlParameter();
            para[16].ParameterName = "LCost";
            para[16].SqlDbType = SqlDbType.Decimal;
            para[16].Value = _UpdateInventoryParam.LCost;

            para[17] = new SqlParameter();
            para[17].ParameterName = "AllowZero";
            para[17].SqlDbType = SqlDbType.SmallInt;
            para[17].Value = _UpdateInventoryParam.AllowZero;

            para[18] = new SqlParameter();
            para[18].ParameterName = "InUse";
            para[18].SqlDbType = SqlDbType.SmallInt;
            para[18].Value = _UpdateInventoryParam.InUse;

            para[19] = new SqlParameter();
            para[19].ParameterName = "EN";
            para[19].SqlDbType = SqlDbType.Int;
            para[19].Value = _UpdateInventoryParam.EN;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Hand";
            para[20].SqlDbType = SqlDbType.Decimal;
            para[20].Value = _UpdateInventoryParam.Hand;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Aisle";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = _UpdateInventoryParam.Aisle;

            para[22] = new SqlParameter();
            para[22].ParameterName = "fOrder";
            para[22].SqlDbType = SqlDbType.Decimal;
            para[22].Value = _UpdateInventoryParam.fOrder;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Min";
            para[23].SqlDbType = SqlDbType.Decimal;
            para[23].Value = _UpdateInventoryParam.Min;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Shelf";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = _UpdateInventoryParam.Shelf;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Bin";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = _UpdateInventoryParam.Bin;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Requ";
            para[26].SqlDbType = SqlDbType.Decimal;
            para[26].Value = _UpdateInventoryParam.Requ;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Warehouse";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = _UpdateInventoryParam.Warehouse;

            para[28] = new SqlParameter();
            para[28].ParameterName = "Price6";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = _UpdateInventoryParam.Price6;

            para[29] = new SqlParameter();
            para[29].ParameterName = "Committed";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = _UpdateInventoryParam.Committed;

            para[30] = new SqlParameter();
            para[30].ParameterName = "Available";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = _UpdateInventoryParam.Available;

            para[31] = new SqlParameter();
            para[31].ParameterName = "IssuedOpenJobs";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = _UpdateInventoryParam.IssuedOpenJobs;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Specification";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = _UpdateInventoryParam.Specification;

            para[33] = new SqlParameter();
            para[33].ParameterName = "Revision";
            para[33].SqlDbType = SqlDbType.VarChar;
            para[33].Value = _UpdateInventoryParam.Revision;

            para[34] = new SqlParameter();
            para[34].ParameterName = "Eco";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = _UpdateInventoryParam.Eco;

            para[35] = new SqlParameter();
            para[35].ParameterName = "Drawing";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = _UpdateInventoryParam.Drawing;

            para[36] = new SqlParameter();
            para[36].ParameterName = "ShelfLife";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = _UpdateInventoryParam.ShelfLife;

            para[37] = new SqlParameter();
            para[37].ParameterName = "GLcogs";
            para[37].SqlDbType = SqlDbType.VarChar;
            para[37].Value = _UpdateInventoryParam.GLcogs;

            para[38] = new SqlParameter();
            para[38].ParameterName = "GLPurchases";
            para[38].SqlDbType = SqlDbType.VarChar;
            para[38].Value = _UpdateInventoryParam.GLPurchases;

            para[39] = new SqlParameter();
            para[39].ParameterName = "ABCClass";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = _UpdateInventoryParam.ABCClass;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OHValue";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = _UpdateInventoryParam.OHValue;

            para[41] = new SqlParameter();
            para[41].ParameterName = "OOValue";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = _UpdateInventoryParam.OOValue;

            para[42] = new SqlParameter();
            para[42].ParameterName = "OverIssueAllowance";
            para[42].SqlDbType = SqlDbType.Bit;
            para[42].Value = _UpdateInventoryParam.OverIssueAllowance;

            para[43] = new SqlParameter();
            para[43].ParameterName = "UnderIssueAllowance";
            para[43].SqlDbType = SqlDbType.Bit;
            para[43].Value = _UpdateInventoryParam.UnderIssueAllowance;

            para[44] = new SqlParameter();
            para[44].ParameterName = "InventoryTurns";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = _UpdateInventoryParam.InventoryTurns;

            para[45] = new SqlParameter();
            para[45].ParameterName = "MOQ";
            para[45].SqlDbType = SqlDbType.Decimal;
            para[45].Value = _UpdateInventoryParam.MOQ;

            para[46] = new SqlParameter();
            para[46].ParameterName = "MinInvQty";
            para[46].SqlDbType = SqlDbType.Decimal;
            para[46].Value = _UpdateInventoryParam.MinInvQty;

            para[47] = new SqlParameter();
            para[47].ParameterName = "MaxInvQty";
            para[47].SqlDbType = SqlDbType.Decimal;
            para[47].Value = _UpdateInventoryParam.MaxInvQty;

            para[48] = new SqlParameter();
            para[48].ParameterName = "Commodity";
            para[48].SqlDbType = SqlDbType.VarChar;
            para[48].Value = _UpdateInventoryParam.Commodity;

            para[49] = new SqlParameter();
            para[49].ParameterName = "LastReceiptDate";
            para[49].SqlDbType = SqlDbType.DateTime;
            if (_UpdateInventoryParam.LastReceiptDate != null && _UpdateInventoryParam.LastReceiptDate != System.DateTime.MinValue)
            {
                para[49].Value = _UpdateInventoryParam.LastReceiptDate;
            }


            para[50] = new SqlParameter();
            para[50].ParameterName = "EAU";
            para[50].SqlDbType = SqlDbType.Decimal;
            para[50].Value = _UpdateInventoryParam.EAU;

            para[51] = new SqlParameter();
            para[51].ParameterName = "EOLDate";
            para[51].SqlDbType = SqlDbType.DateTime;
            if (_UpdateInventoryParam.EOLDate != null && _UpdateInventoryParam.EOLDate != System.DateTime.MinValue)
            {
                para[51].Value = _UpdateInventoryParam.EOLDate;
            }


            para[52] = new SqlParameter();
            para[52].ParameterName = "WarrantyPeriod";
            para[52].SqlDbType = SqlDbType.Int;
            para[52].Value = _UpdateInventoryParam.WarrantyPeriod;

            para[53] = new SqlParameter();
            para[53].ParameterName = "PODueDate";
            para[53].SqlDbType = SqlDbType.DateTime;
            if (_UpdateInventoryParam.PODueDate != null && _UpdateInventoryParam.PODueDate != System.DateTime.MinValue)
            {
                para[53].Value = _UpdateInventoryParam.PODueDate;
            }


            para[54] = new SqlParameter();
            para[54].ParameterName = "DefaultReceivingLocation";
            para[54].SqlDbType = SqlDbType.Bit;
            para[54].Value = _UpdateInventoryParam.DefaultReceivingLocation;

            para[55] = new SqlParameter();
            para[55].ParameterName = "DefaultInspectionLocation";
            para[55].SqlDbType = SqlDbType.Bit;
            para[55].Value = _UpdateInventoryParam.DefaultInspectionLocation;

            para[56] = new SqlParameter();
            para[56].ParameterName = "QtyAllocatedToSO";
            para[56].SqlDbType = SqlDbType.Decimal;
            para[56].Value = _UpdateInventoryParam.QtyAllocatedToSO;

            para[57] = new SqlParameter();
            para[57].ParameterName = "UnitCost";
            para[57].SqlDbType = SqlDbType.Decimal;
            para[57].Value = _UpdateInventoryParam.UnitCost;

            para[58] = new SqlParameter();
            para[58].ParameterName = "GLSales";
            para[58].SqlDbType = SqlDbType.Int;
            para[58].Value = _UpdateInventoryParam.GLSales;

            para[59] = new SqlParameter();
            para[59].ParameterName = "EOQ";
            para[59].SqlDbType = SqlDbType.Decimal;
            para[59].Value = _UpdateInventoryParam.EOQ;

            para[60] = new SqlParameter();
            para[60].ParameterName = "LeadTime";
            para[60].SqlDbType = SqlDbType.Int;
            para[60].Value = _UpdateInventoryParam.LeadTime;

            para[61] = new SqlParameter();
            para[61].ParameterName = "@Docs";
            para[61].SqlDbType = SqlDbType.Structured;
            if (_UpdateInventoryParam.dtDocs.Rows.Count > 0)
            {
                if (_UpdateInventoryParam.dtDocs.Rows[0]["ID"].ToString() != "0")
                {
                    para[61].Value = _UpdateInventoryParam.dtDocs;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Portal", typeof(int));
                    dt.Columns.Add("Remarks", typeof(string));
                    para[61].Value = dt;
                }

            }
          


            //para[62] = new SqlParameter();
            //para[62].ParameterName = "Part";
            //para[62].SqlDbType = SqlDbType.VarChar;
            //para[62].Value = inv.Part;

            #endregion

            try
            {
                GetInventoryPartsInfoByInventoryIDParam _objInv = new GetInventoryPartsInfoByInventoryIDParam();
                _objInv.ID = Convert.ToInt32(_UpdateInventoryParam.ID);

                DataSet dsAppManuinfo = GetInventoryPartsInfoByInventoryID(_objInv, ConnectionString);

                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventory"))
                    {
                        try
                        {
                            //  SqlHelper.ExecuteScalar(tans, Inventory.UPDATE_INVENTORY_XML, xml);

                            SqlHelper.ExecuteScalar(tans, _UpdateInventoryParam.UPDATE_INVENTORY_XML, para);


                            if (_UpdateInventoryParam.InvPartslist != null)
                            {
                                foreach (InvParts _objManInv in _UpdateInventoryParam.InvPartslist)
                                {
                                    _objManInv.ItemID = _UpdateInventoryParam.ID;

                                    if (_objManInv.ID != 0)//Update when you have and ID associated with the vendor information
                                        SqlHelper.ExecuteNonQuery(tans, _UpdateInventoryParam.UPDATE__INVENTORY_PARTS, _objManInv.ID, _objManInv.ItemID, _objManInv.MPN, _objManInv.Part, _objManInv.Supplier, _objManInv.VendorID, _objManInv.Price, _objManInv.Mfg, _objManInv.MfgPrice);
                                    else//create vendors
                                        _objManInv.ID = (int)SqlHelper.ExecuteScalar(tans, _UpdateInventoryParam.CREATE_INVENTORY_PARTS, _objManInv.ItemID, _objManInv.MPN, _objManInv.Part, _objManInv.Supplier, _objManInv.VendorID, _objManInv.Price, _objManInv.Mfg, _objManInv.MfgPrice);
                                }

                                //check for the list to be deleted
                                if (dsAppManuinfo != null)
                                {
                                    if (dsAppManuinfo.Tables.Count > 0)
                                    {
                                        if (dsAppManuinfo.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dsAppManuinfo.Tables[0].Rows.Count; i++)
                                            {
                                                int ID = Convert.ToInt32(dsAppManuinfo.Tables[0].Rows[i]["ID"]);

                                                if (!_UpdateInventoryParam.InvPartslist.Any(x => x.ID != 0 & x.ID == ID))
                                                {
                                                    SqlHelper.ExecuteNonQuery(tans, _UpdateInventoryParam.DELETE__INVENTORY_PARTS, ID);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            tans.Commit();
                        }
                        catch (Exception ex)
                        {
                            tans.Rollback("TransactionInventory");

                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet GetInventoryPartsInfoByInventoryID(int _objInv)
        {
            DataSet ds = new DataSet();
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();
                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_INVENTORY_PARTS_BY_INVENTORYID, _objInv);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        //API
        public DataSet GetInventoryPartsInfoByInventoryID(GetInventoryPartsInfoByInventoryIDParam _objInv, string ConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {

                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                ds = SqlHelper.ExecuteDataset(constring, _objInv.GET_INVENTORY_PARTS_BY_INVENTORYID, _objInv.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public void CreateInventoryParts(InvParts _objManInv)
        {



            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();


                // SqlHelper.ExecuteScalar(transaction, Inventory.CREATE_INVENTORY_XML, xml);
                SqlHelper.ExecuteScalar(constring, Inventory.CREATE_INVENTORY_PARTS, _objManInv.ItemID, _objManInv.MPN, _objManInv.Part, _objManInv.Supplier, _objManInv.VendorID, _objManInv.Price, _objManInv.Mfg, _objManInv.MfgPrice,_objManInv.ID);



            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        //API
        public void CreateInventoryParts(CreateInventoryPartsParam _CreateInventoryPartsParam, string ConnectionString)
        {
            try
            {
                // string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;

                // SqlHelper.ExecuteScalar(transaction, Inventory.CREATE_INVENTORY_XML, xml);
                SqlHelper.ExecuteScalar(constring, _CreateInventoryPartsParam.CREATE_INVENTORY_PARTS, _CreateInventoryPartsParam.ItemID, _CreateInventoryPartsParam.MPN, _CreateInventoryPartsParam.Part, _CreateInventoryPartsParam.Supplier, _CreateInventoryPartsParam.VendorID, _CreateInventoryPartsParam.Price, _CreateInventoryPartsParam.Mfg, _CreateInventoryPartsParam.MfgPrice, _CreateInventoryPartsParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateInvMergeWarehouse(InvWarehouse _objInvWarehouse)
        {
            try
            {
               string constring = HttpContext.Current.Session["config"].ToString();
                // SqlHelper.ExecuteScalar(transaction, Inventory.CREATE_INVENTORY_XML, xml);
               SqlHelper.ExecuteScalar(constring, Inventory.CREATE_INVENTORY_WAREHOUSEMERGE, _objInvWarehouse.InvID, _objInvWarehouse.WarehouseID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void CreateInvMergeWarehouse(CreateInvMergeWarehouseParam _CreateInvMergeWarehouseParam, string ConnectionString)
        {
            try
            {
                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                // SqlHelper.ExecuteScalar(transaction, Inventory.CREATE_INVENTORY_XML, xml);
                SqlHelper.ExecuteScalar(constring, _CreateInvMergeWarehouseParam.CREATE_INVENTORY_WAREHOUSEMERGE, _CreateInvMergeWarehouseParam.InvID, _CreateInvMergeWarehouseParam.WarehouseID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetDeaultWarehouse()
        {
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();
                DataSet ds = SqlHelper.ExecuteDataset(constring, "sp_GetDefaultWarehouse");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteInvMergeWarehouse(int invID,string WareHouseID)
        {
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();
                SqlHelper.ExecuteNonQuery(constring, CommandType.Text, " DELETE FROM InvWarehouse WHERE InvID = " + invID + " AND WareHouseID = '" + WareHouseID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int CreateInventoryManufacturerInformation(InventoryManufacturerInformation _objManInv)
        {
            int success = 0;


            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();


                // SqlHelper.ExecuteScalar(transaction, Inventory.CREATE_INVENTORY_XML, xml);

                SqlHelper.ExecuteScalar(constring, Inventory.CREATE_INVENTORY_MANUFACTURER_INFORMATION, _objManInv.InventoryID, _objManInv.MPN, _objManInv.ApprovedManufacturer, _objManInv.ApprovedVendor);


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return success;

        }


        public DataSet GetInventoryManufactureInfo(int _objInv)
        {
            DataSet ds = new DataSet();
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();
                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_INVENTORY_MANUFACTURER_INFORMATION_BY_INVENTORYID, _objInv);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public void DeleteInventoryManufactureInfo(int _objManInvid)
        {

            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();
                SqlHelper.ExecuteNonQuery(constring, Inventory.DELETE_INVENTORY_MANUFACTURER_INFORMATION, _objManInvid);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteInventoryParts(int _objManInvid)
        {

            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();
                SqlHelper.ExecuteNonQuery(constring, Inventory.DELETE__INVENTORY_PARTS, _objManInvid);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public void DeleteInventoryParts(DeleteInventoryPartsParam _DeleteInventoryPartsParam, string ConnectionString)
        {

            try
            {

                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                SqlHelper.ExecuteNonQuery(constring, _DeleteInventoryPartsParam.DELETE__INVENTORY_PARTS, _DeleteInventoryPartsParam.invpartID);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void CreateItemRevision(InvItemRev objItemRev)
        {
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();
                SqlHelper.ExecuteScalar(constring, "spCreateItemRev", objItemRev.Date, objItemRev.Version, objItemRev.Comment, objItemRev.Eco, objItemRev.Drawing, objItemRev.InvID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public void CreateItemRevision(CreateItemRevisionParam _CreateItemRevisionParam, string ConnectionString)
        {
            try
            {
                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                SqlHelper.ExecuteScalar(constring, "spCreateItemRev", _CreateItemRevisionParam.Date, _CreateItemRevisionParam.Version, _CreateItemRevisionParam.Comment, _CreateItemRevisionParam.Eco, _CreateItemRevisionParam.Drawing, _CreateItemRevisionParam.InvID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetItemQuantity()
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, Inventory.GET_ALL_ITEM_QUANTITY);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetItemPurchaseOrder(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_ALL_ITEM_PURCHASE_ORDER, ID);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetItemPurchaseOrderByInvID(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_INV_PURCHASINGORDER, ID);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        //API
        public DataSet GetItemPurchaseOrderByInvID(GetItemPurchaseOrderByInvIDParam _GetItemPurchaseOrderByInvIDParam, string ConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;

                ds = SqlHelper.ExecuteDataset(constring, _GetItemPurchaseOrderByInvIDParam.GET_INV_PURCHASINGORDER, _GetItemPurchaseOrderByInvIDParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetAllItemQuantityByInvID(int ID, int userID, int EN)
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_INV_ALLITEMQUANTITY, ID, userID, EN);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        //API
        public DataSet GetAllItemQuantityByInvID(GetAllItemQuantityByInvIDParam _GetAllItemQuantityByInvIDParam, string ConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;

                ds = SqlHelper.ExecuteDataset(constring, _GetAllItemQuantityByInvIDParam.GET_INV_ALLITEMQUANTITY, _GetAllItemQuantityByInvIDParam.ID, _GetAllItemQuantityByInvIDParam.UserID, _GetAllItemQuantityByInvIDParam.EN);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetInvManufacturerInfoByInvAndVendorId(int InventoryId, int ApprovedVendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_INVENTORY_MANUFACTURER_INFORMATION_BY_INVENTORYID_APPROVEDVENDOR, InventoryId, ApprovedVendorId);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        //API
        public DataSet GetInvManufacturerInfoByInvAndVendorId(GetInvManufacturerInfoByInvAndVendorIdParam _GetInvManufacturerInfoByInvAndVendorIdParam, string ConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;

                ds = SqlHelper.ExecuteDataset(constring, _GetInvManufacturerInfoByInvAndVendorIdParam.GET_INVENTORY_MANUFACTURER_INFORMATION_BY_INVENTORYID_APPROVEDVENDOR, _GetInvManufacturerInfoByInvAndVendorIdParam.InventoryID, _GetInvManufacturerInfoByInvAndVendorIdParam.ApprovedVendorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public Commodity CreateCommodity(Commodity com)
        {
            Commodity comdt = com;
            try
            {
                comdt.ID = (int)SqlHelper.ExecuteScalar(com.ConnConfig, Commodity.CREATE_COMMODITY, com.Code, com.Desc);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public DataSet ReadCommodityById(Commodity com)
        {
            DataSet comdt = null;
            try
            {
                comdt = SqlHelper.ExecuteDataset(com.ConnConfig, Commodity.GET_ALL_COMMODITY_BY_ID, com.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public DataSet ReadAllCommodity(Commodity com)
        {
            DataSet comdt = null;
            try
            {
                comdt = SqlHelper.ExecuteDataset(com.ConnConfig, Commodity.GET_ALL_COMMODITY_BY_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public DataSet ReadAllCommodity(string ConnectionString, ReadAllCommodityParam _ReadAllCommodityParam)
        {
            DataSet comdt = null;
            try
            {
                comdt = SqlHelper.ExecuteDataset(ConnectionString, _ReadAllCommodityParam.GET_ALL_COMMODITY_BY_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public Commodity UpdateCommodity(Commodity com)
        {
            Commodity comdt = com;
            try
            {
                SqlHelper.ExecuteScalar(com.ConnConfig, Commodity.UPDATE_COMMODITY, com.ID, com.Code, com.Desc, com.IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        //public void DeleteTransInvoiceByRef(Transaction _objTrans)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, " DELETE FROM Trans WHERE Ref = " + _objTrans.Ref + " AND Batch = " + _objTrans.BatchID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public DataSet GetInvoiceByID(Invoices _objInvoice)
        //{
        //    try
        //    {
        //        return _objInvoice.Ds = SqlHelper.ExecuteDataset(_objInvoice.ConnConfig, CommandType.Text, "SELECT fDate,Ref,Batch,TransID FROM Invoice WHERE Ref=" + _objInvoice.Ref);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet ReadCostTypes(Inventory inv)
        {
            DataSet comdt = null;
            try
            {
                comdt = SqlHelper.ExecuteDataset(inv.ConnConfig, "spReadCostTypes");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public void UseCostingType(Inventory inv)
        {

            try
            {
                SqlHelper.ExecuteScalar(inv.ConnConfig, "spUpdateCostTypes", inv.ID, Convert.ToBoolean(inv.InUse));
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public DataSet GetAllInventoryAdjustmentByDate(InventoryAdjustment invadjustitem)
        {
            DataSet Invadjust = null;
            try
            {
                Invadjust = SqlHelper.ExecuteDataset(invadjustitem.ConnConfig, InventoryAdjustment.GET_ALL_INVENTORY_ADJUSTMENT, null, invadjustitem.Stdate, invadjustitem.Enddate, invadjustitem.EN, invadjustitem.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Invadjust;
        }

        //API
        public DataSet GetAllInventoryAdjustmentByDate(GetAllInventoryAdjustmentByDateParam _GetAllInvAdjustmentByDateParam, string ConnectionString)
        {
            DataSet Invadjust = null;
            try
            {
                Invadjust = SqlHelper.ExecuteDataset(ConnectionString, _GetAllInvAdjustmentByDateParam.GET_ALL_INVENTORY_ADJUSTMENT, null, _GetAllInvAdjustmentByDateParam.Stdate, _GetAllInvAdjustmentByDateParam.Enddate, _GetAllInvAdjustmentByDateParam.EN, _GetAllInvAdjustmentByDateParam.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Invadjust;
        }


        public DataSet GetInventoryAdjustmentByID(InventoryAdjustment invadjustitem)
        {
            DataSet Invadjust = null;
            try
            {
                Invadjust = SqlHelper.ExecuteDataset(invadjustitem.ConnConfig, InventoryAdjustment.GET_ALL_INVENTORY_ADJUSTMENT, invadjustitem.ID, null, null,null,null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Invadjust;
        }

        //API
        public DataSet GetInventoryAdjustmentByID(GetInventoryAdjustmentByIDParam _GetInventoryAdjustmentByIDParam, string ConnectionString)
        {
            DataSet Invadjust = null;
            try
            {
                Invadjust = SqlHelper.ExecuteDataset(ConnectionString, _GetInventoryAdjustmentByIDParam.GET_ALL_INVENTORY_ADJUSTMENT, _GetInventoryAdjustmentByIDParam.ID, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Invadjust;
        }


        public DataSet GetInvItems(Inventory inv, string searchterm)
        {
            DataSet ds = null;
            try
            {
                if (string.IsNullOrEmpty(searchterm.Trim()))
                    ds = SqlHelper.ExecuteDataset(inv.ConnConfig, CommandType.Text, "select top 100  [ID],[Name],[fDesc],[Part],isnull([Hand],0) as Hand,isnull([Balance] ,0.00) as Balance ,isnull([Price1],0.00) as SPrice from Inv with (nolock) where Inv.Type=0 and Status = 0 ");
                else
                    ds = SqlHelper.ExecuteDataset(inv.ConnConfig, CommandType.Text, "select top 100  [ID],[Name],[fDesc],[Part],isnull([Hand],0) as Hand,isnull([Balance] ,0.00) as Balance ,isnull([Price1],0.00) as SPrice  from Inv with (nolock) where Inv.Type=0 and Status = 0 and  ( Name like '%" + searchterm + "%' or fDesc    like '%" + searchterm + "%' )");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public int CreateInventoryAdjustments(InventoryAdjustment invadjustitem,bool _chkInvTracking)
        {
            int success = 0;

            try
            {


                string constring = invadjustitem.ConnConfig;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventoryAdjustment"))
                    {
                        try
                        {
                            String Retacctid = "";
                            if (invadjustitem.EN==1)
                            {
                                Retacctid = (String)SqlHelper.ExecuteScalar(tans, CommandType.Text, "Select convert(nvarchar(50),DInvAcct) AS DInvAcct from Branch where ID=" + invadjustitem.CompanyID +"");
                            }
                            else
                            {
                                 Retacctid = (String)SqlHelper.ExecuteScalar(tans, CommandType.Text, "select Label from Custom where Name='DefaultInvGLAcct'");
                            }
                           

                            //int MaxBatchId = 0;
                            //int TransId = 0;
                            invadjustitem.Trans.BatchID = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans");
                            //1:Update the inventory onhand,balance and required cols
                            //    SqlHelper.ExecuteNonQuery(tans, "spUpdateInventoryForAjustments", invadjustitem.Inv.ID, invadjustitem.Inv.Hand, invadjustitem.Inv.Balance); 
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateInvWarehouseForAjustments", invadjustitem.IWarehouseLocAdj.InvID, invadjustitem.IWarehouseLocAdj.WarehouseID, invadjustitem.IWarehouseLocAdj.locationID, invadjustitem.IWarehouseLocAdj.Hand, invadjustitem.IWarehouseLocAdj.Balance);
                            
                            //2:Create adjustments

                            invadjustitem.ID = (int)SqlHelper.ExecuteScalar(tans, "spCreateInvAdjustments", invadjustitem.fDate, invadjustitem.fDesc, invadjustitem.Quantity,
                                invadjustitem.Amount, invadjustitem.Inv.ID, invadjustitem.Trans.BatchID, invadjustitem.Trans.ID, invadjustitem.Acct.ID, invadjustitem.IWarehouseLocAdj.WarehouseID, invadjustitem.IWarehouseLocAdj.locationID,0);

                            //int acctid = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "select top 1 id from chart where chart.fdesc='inventory'");

                            // Change  by Ravinder for Default Inv Acct

                            
                            if (Retacctid != "")
                            {
                                int acctid = Convert.ToInt32(Retacctid);
                                //3:Once Adjustments are created, create new trasaction for the inventory
                                if (_chkInvTracking == true)
                                {
                                    invadjustitem.Trans.ID = (int)SqlHelper.ExecuteScalar(tans, "AddTransForInvAdjustments", invadjustitem.Trans.BatchID, invadjustitem.fDate, 60, 0, invadjustitem.ID,
                                        invadjustitem.fDesc, invadjustitem.Amount, acctid, invadjustitem.Inv.ID, invadjustitem.Quantity, 0, 0, 0, 0, null);
                                }
                                //4:Once the transaction is created then update the adjustment record with the trans id
                                SqlHelper.ExecuteNonQuery(tans, "spUpdateInvAdjustments", invadjustitem.ID, invadjustitem.fDate, invadjustitem.fDesc, invadjustitem.Quantity,
                                    invadjustitem.Amount, invadjustitem.Inv.ID, invadjustitem.Trans.BatchID, invadjustitem.Trans.ID, invadjustitem.Acct.ID);
                                //5:Update chart balance for chart inventory(add adjustment balance to chart type inventory) 
                                SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjustments", acctid, invadjustitem.Amount);
                                //6:Add Transaction for non inventory account that is the GL account select by user.
                                if (_chkInvTracking == true)
                                {
                                    SqlHelper.ExecuteScalar(tans, "AddTrans", null, invadjustitem.Trans.BatchID, invadjustitem.fDate, 61, 0, invadjustitem.ID,
                                    invadjustitem.fDesc, invadjustitem.Amount * -1, invadjustitem.Acct.ID, null, "", 0, 0, 0, 0, null);
                                }
                                //7:Update chart balance for other chart select for adjustment(substract adjustment balance to chart type) 
                                SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjustments", invadjustitem.Acct.ID, invadjustitem.Amount);


                                success = 1;

                                tans.Commit();
                            }

                        }
                        catch (Exception ex)
                        {
                            success = -1;

                            tans.Rollback("TransactionInventoryAdjustment");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }


        //API
        public int CreateInventoryAdjustments(CreateInventoryAdjustmentsParam _CreateInventoryAdjustmentsParam, string ConnectionString)
        {
            int success = 0;

            try
            {


                string constring = ConnectionString;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventoryAdjustment"))
                    {
                        try
                        {
                            String Retacctid = "";
                            if (_CreateInventoryAdjustmentsParam.EN == 1)
                            {
                                Retacctid = (String)SqlHelper.ExecuteScalar(tans, CommandType.Text, "Select convert(nvarchar(50),DInvAcct) AS DInvAcct from Branch where ID=" + _CreateInventoryAdjustmentsParam.CompanyID + "");
                            }
                            else
                            {
                                Retacctid = (String)SqlHelper.ExecuteScalar(tans, CommandType.Text, "select Label from Custom where Name='DefaultInvGLAcct'");
                            }


                            //int MaxBatchId = 0;
                            //int TransId = 0;
                            _CreateInventoryAdjustmentsParam.Trans_BatchID = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans");
                            //1:Update the inventory onhand,balance and required cols
                            //    SqlHelper.ExecuteNonQuery(tans, "spUpdateInventoryForAjustments", invadjustitem.Inv.ID, invadjustitem.Inv.Hand, invadjustitem.Inv.Balance); 
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateInvWarehouseForAjustments", _CreateInventoryAdjustmentsParam.IWarehouseLocAdj_InvID, _CreateInventoryAdjustmentsParam.IWarehouseLocAdj_WarehouseID, _CreateInventoryAdjustmentsParam.IWarehouseLocAdj_locationID, _CreateInventoryAdjustmentsParam.IWarehouseLocAdj_Hand, _CreateInventoryAdjustmentsParam.IWarehouseLocAdj_Balance);

                            //2:Create adjustments

                            _CreateInventoryAdjustmentsParam.ID = (int)SqlHelper.ExecuteScalar(tans, "spCreateInvAdjustments", _CreateInventoryAdjustmentsParam.fDate, _CreateInventoryAdjustmentsParam.fDesc, _CreateInventoryAdjustmentsParam.Quantity,
                                _CreateInventoryAdjustmentsParam.Amount, _CreateInventoryAdjustmentsParam.Inv_ID, _CreateInventoryAdjustmentsParam.Trans_BatchID, _CreateInventoryAdjustmentsParam.Trans_ID, _CreateInventoryAdjustmentsParam.Acct_ID, _CreateInventoryAdjustmentsParam.IWarehouseLocAdj_WarehouseID, _CreateInventoryAdjustmentsParam.IWarehouseLocAdj_locationID, 0);

                            //int acctid = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "select top 1 id from chart where chart.fdesc='inventory'");

                            // Change  by Ravinder for Default Inv Acct


                            if (Retacctid != "")
                            {
                                int acctid = Convert.ToInt32(Retacctid);
                                //3:Once Adjustments are created, create new trasaction for the inventory
                                _CreateInventoryAdjustmentsParam.Trans_ID = (int)SqlHelper.ExecuteScalar(tans, "AddTransForInvAdjustments", _CreateInventoryAdjustmentsParam.Trans_BatchID, _CreateInventoryAdjustmentsParam.fDate, 60, 0, _CreateInventoryAdjustmentsParam.ID,
                                    _CreateInventoryAdjustmentsParam.fDesc, _CreateInventoryAdjustmentsParam.Amount, acctid, _CreateInventoryAdjustmentsParam.Inv_ID, _CreateInventoryAdjustmentsParam.Quantity, 0, 0, 0, 0, null);
                                //4:Once the transaction is created then update the adjustment record with the trans id
                                SqlHelper.ExecuteNonQuery(tans, "spUpdateInvAdjustments", _CreateInventoryAdjustmentsParam.ID, _CreateInventoryAdjustmentsParam.fDate, _CreateInventoryAdjustmentsParam.fDesc, _CreateInventoryAdjustmentsParam.Quantity,
                                    _CreateInventoryAdjustmentsParam.Amount, _CreateInventoryAdjustmentsParam.Inv_ID, _CreateInventoryAdjustmentsParam.Trans_BatchID, _CreateInventoryAdjustmentsParam.Trans_ID, _CreateInventoryAdjustmentsParam.Acct_ID);
                                //5:Update chart balance for chart inventory(add adjustment balance to chart type inventory) 
                                SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjustments", acctid, _CreateInventoryAdjustmentsParam.Amount);
                                //6:Add Transaction for non inventory account that is the GL account select by user.
                                SqlHelper.ExecuteScalar(tans, "AddTrans", null, _CreateInventoryAdjustmentsParam.Trans_BatchID, _CreateInventoryAdjustmentsParam.fDate, 61, 0, _CreateInventoryAdjustmentsParam.ID,
                                    _CreateInventoryAdjustmentsParam.fDesc, _CreateInventoryAdjustmentsParam.Amount * -1, _CreateInventoryAdjustmentsParam.Acct_ID, null, "", 0, 0, 0, 0, null);
                                //7:Update chart balance for other chart select for adjustment(substract adjustment balance to chart type) 
                                SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjustments", _CreateInventoryAdjustmentsParam.Acct_ID, _CreateInventoryAdjustmentsParam.Amount);


                                success = 1;

                                tans.Commit();
                            }

                        }
                        catch (Exception ex)
                        {
                            success = -1;

                            tans.Rollback("TransactionInventoryAdjustment");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }

        public int DeleteAdjustment(InventoryAdjustment invadjustitem)
        {
            int success = 0;

            try
            {


                string constring = invadjustitem.ConnConfig;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventoryAdjustment"))
                    {
                        try
                        {
                            //int acctid = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "select top 1 id from chart where chart.fdesc='inventory'");
                            int acctid = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "SELECT Acct FROM IAdj WHERE ID =" + invadjustitem.ID);

                            //1:Get the adjustments
                            DataSet _dsadjustment = (DataSet)SqlHelper.ExecuteDataset(tans, CommandType.Text, "SELECT top 1 [ID],[fDate],[fDesc],[Quan],[Amount],[Item],[Batch],[TransID],[Acct] from IAdj where IAdj.ID=" + invadjustitem.ID);
                            //2:Get Inventory
                            DataSet _dsinv = (DataSet)SqlHelper.ExecuteDataset(tans, CommandType.Text, "SELECT top 1 [ID],[Hand],[Balance] from Inv where Inv.ID=" + (int)_dsadjustment.Tables[0].Rows[0]["Item"]);

                            decimal currentinvonhand = (decimal)_dsinv.Tables[0].Rows[0]["Hand"];
                            decimal currentinvBalance = (decimal)_dsinv.Tables[0].Rows[0]["Balance"];



                            decimal newAdjonhand = (decimal)_dsadjustment.Tables[0].Rows[0]["Quan"] * -1;
                            decimal newAdjBalance = (decimal)_dsadjustment.Tables[0].Rows[0]["Amount"] * -1;


                            if (newAdjonhand >= 0)
                                currentinvonhand += newAdjonhand;//Revert back or add quantity if the adjustment was negetive
                            else
                                currentinvonhand -= newAdjonhand;//Revert back or remove quantity if the adjustment was positive


                            if (newAdjBalance >= 0)
                                currentinvBalance += newAdjBalance;//Revert back or add balance if the adjustment was negetive
                            else
                                currentinvBalance -= newAdjBalance;//Revert back or add balance if the adjustment was positive

                            //3:Update the inventory quantity and balance
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateInventoryForAjustments", (int)_dsadjustment.Tables[0].Rows[0]["Item"], currentinvonhand, currentinvBalance);

                            //4:Delete transaction based on the batchid
                            SqlHelper.ExecuteNonQuery(tans, "DeleteTransForInvAdjustments", (int)_dsadjustment.Tables[0].Rows[0]["Batch"]);

                            //5:Remove balance from chart where chart type is inventory
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjRemoval", acctid, (decimal)_dsadjustment.Tables[0].Rows[0]["Amount"]);

                            //6:Add balance to chart where chart type is selected for adjustment
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjRemoval", (int)_dsadjustment.Tables[0].Rows[0]["Acct"], (decimal)_dsadjustment.Tables[0].Rows[0]["Amount"]);

                            //7:Update the iadj or adjustement table record to 0.00 for balance and quatity.
                            SqlHelper.ExecuteNonQuery(tans, "spDeleteInvAdjustments", (int)_dsadjustment.Tables[0].Rows[0]["ID"], (DateTime)_dsadjustment.Tables[0].Rows[0]["fdate"], "Voided by ADMIN on" + ((DateTime)_dsadjustment.Tables[0].Rows[0]["fdate"]).ToString("dd/MM/yyyy") + "Inventory Adjustment",
                                0.00, 0.00, (int)_dsadjustment.Tables[0].Rows[0]["Item"], (int)_dsadjustment.Tables[0].Rows[0]["Batch"], (int)_dsadjustment.Tables[0].Rows[0]["TransID"], (int)_dsadjustment.Tables[0].Rows[0]["Acct"]);

                            success = 1;

                            tans.Commit();


                        }
                        catch (Exception exx)
                        {
                            success = -1;

                            tans.Rollback("TransactionInventoryAdjustment");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }

        //API
        public int DeleteAdjustment(DeleteAdjustmentParam _DeleteAdjustment, string ConnectionString)
        {
            int success = 0;

            try
            {


                string constring = ConnectionString;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventoryAdjustment"))
                    {
                        try
                        {
                            //int acctid = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "select top 1 id from chart where chart.fdesc='inventory'");
                            int acctid = (int)SqlHelper.ExecuteScalar(tans, CommandType.Text, "SELECT Acct FROM IAdj WHERE ID =" + _DeleteAdjustment.ID);

                            //1:Get the adjustments
                            DataSet _dsadjustment = (DataSet)SqlHelper.ExecuteDataset(tans, CommandType.Text, "SELECT top 1 [ID],[fDate],[fDesc],[Quan],[Amount],[Item],[Batch],[TransID],[Acct] from IAdj where IAdj.ID=" + _DeleteAdjustment.ID);
                            //2:Get Inventory
                            DataSet _dsinv = (DataSet)SqlHelper.ExecuteDataset(tans, CommandType.Text, "SELECT top 1 [ID],[Hand],[Balance] from Inv where Inv.ID=" + (int)_dsadjustment.Tables[0].Rows[0]["Item"]);

                            decimal currentinvonhand = (decimal)_dsinv.Tables[0].Rows[0]["Hand"];
                            decimal currentinvBalance = (decimal)_dsinv.Tables[0].Rows[0]["Balance"];



                            decimal newAdjonhand = (decimal)_dsadjustment.Tables[0].Rows[0]["Quan"] * -1;
                            decimal newAdjBalance = (decimal)_dsadjustment.Tables[0].Rows[0]["Amount"] * -1;


                            if (newAdjonhand >= 0)
                                currentinvonhand += newAdjonhand;//Revert back or add quantity if the adjustment was negetive
                            else
                                currentinvonhand -= newAdjonhand;//Revert back or remove quantity if the adjustment was positive


                            if (newAdjBalance >= 0)
                                currentinvBalance += newAdjBalance;//Revert back or add balance if the adjustment was negetive
                            else
                                currentinvBalance -= newAdjBalance;//Revert back or add balance if the adjustment was positive

                            //3:Update the inventory quantity and balance
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateInventoryForAjustments", (int)_dsadjustment.Tables[0].Rows[0]["Item"], currentinvonhand, currentinvBalance);

                            //4:Delete transaction based on the batchid
                            SqlHelper.ExecuteNonQuery(tans, "DeleteTransForInvAdjustments", (int)_dsadjustment.Tables[0].Rows[0]["Batch"]);

                            //5:Remove balance from chart where chart type is inventory
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjRemoval", acctid, (decimal)_dsadjustment.Tables[0].Rows[0]["Amount"]);

                            //6:Add balance to chart where chart type is selected for adjustment
                            SqlHelper.ExecuteNonQuery(tans, "spUpdateChartBalanceForInvAdjRemoval", (int)_dsadjustment.Tables[0].Rows[0]["Acct"], (decimal)_dsadjustment.Tables[0].Rows[0]["Amount"]);

                            //7:Update the iadj or adjustement table record to 0.00 for balance and quatity.
                            SqlHelper.ExecuteNonQuery(tans, "spDeleteInvAdjustments", (int)_dsadjustment.Tables[0].Rows[0]["ID"], (DateTime)_dsadjustment.Tables[0].Rows[0]["fdate"], "Voided by ADMIN on" + ((DateTime)_dsadjustment.Tables[0].Rows[0]["fdate"]).ToString("dd/MM/yyyy") + "Inventory Adjustment",
                                0.00, 0.00, (int)_dsadjustment.Tables[0].Rows[0]["Item"], (int)_dsadjustment.Tables[0].Rows[0]["Batch"], (int)_dsadjustment.Tables[0].Rows[0]["TransID"], (int)_dsadjustment.Tables[0].Rows[0]["Acct"]);

                            success = 1;

                            tans.Commit();


                        }
                        catch (Exception exx)
                        {
                            success = -1;

                            tans.Rollback("TransactionInventoryAdjustment");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }

        public DataSet GetAutoFillWarehouse(InvWarehouse _objInvWarehouse)
        {
            DataSet ds = null;
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();
                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET__INVENTORY_WAREHOUSESEARCH, _objInvWarehouse.SearchValue, _objInvWarehouse.InvID, _objInvWarehouse.EN, _objInvWarehouse.UserID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetAutoFillOnHandBalance(IWarehouseLocAdj _objIWarehouseLocAdj)
        {
            DataSet ds = null;
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "WarehouseID";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _objIWarehouseLocAdj.WarehouseID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "InvID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _objIWarehouseLocAdj.InvID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "locationID";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = _objIWarehouseLocAdj.locationID;
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();
                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET__INVENTORY_WAREHOUSELOCONHAND, para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetAutoFillOnHandBalance(GetAutoFillOnHandBalanceParam _GetAutoFillOnHandBalanceParam, string ConnectionString)
        {
            DataSet ds = null;
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "WarehouseID";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = _GetAutoFillOnHandBalanceParam.WarehouseID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "InvID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _GetAutoFillOnHandBalanceParam.InvID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "locationID";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = _GetAutoFillOnHandBalanceParam.locationID;
            try
            {
                //string constring = HttpContext.Current.Session["config"].ToString();
                string constring = ConnectionString;
                ds = SqlHelper.ExecuteDataset(constring, _GetAutoFillOnHandBalanceParam.GET__INVENTORY_WAREHOUSELOCONHAND, para);
                //ds = SqlHelper.ExecuteDataset(constring, Inventory.GET__INVENTORY_WAREHOUSELOCONHAND, para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public void CreateReceivePOInvWarehouse(IWarehouseLocAdj _objIWarehouseLocAdj, Transaction trans)
        {

            SqlParameter[] para = new SqlParameter[8];

            para[0] = new SqlParameter();
            para[0].ParameterName = "InvID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = _objIWarehouseLocAdj.InvID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "WarehouseID";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _objIWarehouseLocAdj.WarehouseID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "locationID";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = _objIWarehouseLocAdj.locationID;

            para[3] = new SqlParameter();
            para[3].ParameterName = "OnHand";
            para[3].SqlDbType = SqlDbType.Decimal;
            para[3].Value = _objIWarehouseLocAdj.Hand;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Balance";
            para[4].SqlDbType = SqlDbType.Decimal;
            para[4].Value = _objIWarehouseLocAdj.Balance;

            para[5] = new SqlParameter();
            para[5].ParameterName = "fOrder";
            para[5].SqlDbType = SqlDbType.Decimal;
            para[5].Value = _objIWarehouseLocAdj.fOrder;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Committed";
            para[6].SqlDbType = SqlDbType.Decimal;
            para[6].Value = _objIWarehouseLocAdj.Committed;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Available";
            para[7].SqlDbType = SqlDbType.Decimal;
            para[7].Value = _objIWarehouseLocAdj.Available;


            string constring = HttpContext.Current.Session["config"].ToString();
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            //SqlTransaction tans = con.BeginTransaction("TransactionInventoryDetails");
            try
            {

                SqlHelper.ExecuteScalar(constring, Inventory.CREATE__INVENTORY_RECEIVEPOINVWAREHOUSE, para);
                if (trans.BatchID == 0)
                {
                    trans.BatchID = (int)SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans");
                }
                int Acct = (int)SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT TOP 1  ID FROM Chart WHERE DefaultNo = 'D2000' AND Status = 0 ORDER BY ID");


               

                //SqlHelper.ExecuteScalar(constring, "AddTrans", null, trans.BatchID, trans.fDate, 80, trans.Line, 0,
                                   //trans.fDesc, trans.Amount * -1, Acct, null, "-"+trans.Status, 0, 0, 0, 0, trans.strRef);
                SqlHelper.ExecuteScalar(constring, "AddTrans", null, trans.BatchID, trans.fDate, 81, trans.Line, trans.Ref,
                                    trans.fDesc, trans.Amount, trans.Acct, trans.AcctSub, trans.Status, 0, 0, 0, 0, trans.strRef);
                SqlHelper.ExecuteScalar(constring, "spCalChartBalance");



            }
            catch (Exception ex)
            { 
                //tans.Rollback("TransactionInventoryDetails");
                throw ex;
            }

        }

        public void CreateReceivePOInvWarehouse(CreateReceivePOInvWarehouseTransParam _CreateReceivePOInvWarehouseTransParam, string ConnectionString)
        {

            SqlParameter[] para = new SqlParameter[8];

            para[0] = new SqlParameter();
            para[0].ParameterName = "InvID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_InvID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "WarehouseID";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_WarehouseID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "locationID";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_locationID;

            para[3] = new SqlParameter();
            para[3].ParameterName = "OnHand";
            para[3].SqlDbType = SqlDbType.Decimal;
            para[3].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_Hand;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Balance";
            para[4].SqlDbType = SqlDbType.Decimal;
            para[4].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_Balance;

            para[5] = new SqlParameter();
            para[5].ParameterName = "fOrder";
            para[5].SqlDbType = SqlDbType.Decimal;
            para[5].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_fOrder;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Committed";
            para[6].SqlDbType = SqlDbType.Decimal;
            para[6].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_Committed;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Available";
            para[7].SqlDbType = SqlDbType.Decimal;
            para[7].Value = _CreateReceivePOInvWarehouseTransParam.IWarehouseLocAdj_Available;


            //string constring = HttpContext.Current.Session["config"].ToString();
            string constring = ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlTransaction tans = con.BeginTransaction("TransactionInventoryDetails");
            try
            {

                SqlHelper.ExecuteScalar(constring, _CreateReceivePOInvWarehouseTransParam.CREATE__INVENTORY_RECEIVEPOINVWAREHOUSE, para);
                if (_CreateReceivePOInvWarehouseTransParam.BatchID == 0)
                {
                    _CreateReceivePOInvWarehouseTransParam.BatchID = (int)SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans");
                }
                int Acct = (int)SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT TOP 1  ID FROM Chart WHERE DefaultNo = 'D2000' AND Status = 0 ORDER BY ID");

                //SqlHelper.ExecuteScalar(constring, "AddTrans", null, trans.BatchID, trans.fDate, 80, trans.Line, 0,
                //trans.fDesc, trans.Amount * -1, Acct, null, "-"+trans.Status, 0, 0, 0, 0, trans.strRef);
                SqlHelper.ExecuteScalar(constring, "AddTrans", null, _CreateReceivePOInvWarehouseTransParam.BatchID, _CreateReceivePOInvWarehouseTransParam.fDate, 81, _CreateReceivePOInvWarehouseTransParam.Line, _CreateReceivePOInvWarehouseTransParam.Ref,
                                    _CreateReceivePOInvWarehouseTransParam.fDesc, _CreateReceivePOInvWarehouseTransParam.Amount, _CreateReceivePOInvWarehouseTransParam.Acct, _CreateReceivePOInvWarehouseTransParam.AcctSub, _CreateReceivePOInvWarehouseTransParam.Status, 0, 0, 0, 0, _CreateReceivePOInvWarehouseTransParam.strRef);
                SqlHelper.ExecuteScalar(constring, "spCalChartBalance");
            }
            catch (Exception ex)
            {
                tans.Rollback("TransactionInventoryDetails");
                throw ex;
            }

        }

        public void CreateReceivePOInvWarehouseTrans( Transaction trans)
        {
            string constring = HttpContext.Current.Session["config"].ToString();
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            try
            {

                int Acct = (int)SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT TOP 1  ID FROM Chart WHERE DefaultNo = 'D2000' AND Status = 0 ORDER BY ID");

                SqlHelper.ExecuteScalar(constring, "AddTrans", null, trans.BatchID, trans.fDate, 80, trans.Line, 0,
                trans.fDesc, trans.Amount * -1, Acct, null, "-"+trans.Status, 0, 0, 0, 0, trans.strRef);
                SqlHelper.ExecuteScalar(constring, "spCalChartBalance");



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void CreateReceivePOInvWarehouseTrans(ReceivePOInvWarehouseTransParam _ReceivePOInvWarehouseTransParam, string ConnectionString)
        {
            //string constring = HttpContext.Current.Session["config"].ToString();
            string constring = ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            try
            {

                int Acct = (int)SqlHelper.ExecuteScalar(constring, CommandType.Text, "SELECT TOP 1  ID FROM Chart WHERE DefaultNo = 'D2000' AND Status = 0 ORDER BY ID");

                SqlHelper.ExecuteScalar(constring, "AddTrans", null, _ReceivePOInvWarehouseTransParam.BatchID, _ReceivePOInvWarehouseTransParam.fDate, 80, _ReceivePOInvWarehouseTransParam.Line, 0,
                _ReceivePOInvWarehouseTransParam.fDesc, _ReceivePOInvWarehouseTransParam.Amount * -1, Acct, null, "-" + _ReceivePOInvWarehouseTransParam.Status, 0, 0, 0, 0, _ReceivePOInvWarehouseTransParam.strRef);
                SqlHelper.ExecuteScalar(constring, "spCalChartBalance");



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool ISINVENTORYTRACKINGISON(string ConnConfig)
        { 
            try
            {
              return  Convert.ToBoolean(SqlHelper.ExecuteScalar(ConnConfig, CommandType.Text, "IF EXISTS (select 1 from custom  where name ='InvGL' and Label='True') SELECT 1 ELSE  SELECT 0"));
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public DataSet GetInventoryTrans(Inventory _objInv, List<RetainFilter> filters, bool inclInactive)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT   \n");
            sb.Append("	t.ID,  \n");
            sb.Append("	c.Acct + ' - ' + c.fDesc AS InventoryGL,  \n");
            sb.Append("	CASE t.Type  \n");
            sb.Append("		WHEN 41 THEN PJ.ID   \n");
            sb.Append("		WHEN 81 THEN re.ID   \n");
            sb.Append("	ELSE t.Ref END AS Ref, \n");
            sb.Append("	t.fDate,  \n");
            sb.Append("	CASE t.Type  \n");
            sb.Append("		WHEN 60 THEN 'Item Adjustment'   \n");
            sb.Append("		WHEN 97 THEN 'Inventory Item Transfer'  \n");
            sb.Append("		WHEN 81 THEN 'RPO' \n");
            sb.Append("		WHEN 41 THEN 'APBill'  \n");
            sb.Append("		WHEN 70 THEN 'PostToProject/InventoryUsed'  \n");
            sb.Append("	END AS TType, \n");
            sb.Append("	t.fDesc AS MDesc,  \n");
            sb.Append("	t.AcctSub AS INVID, \n");
            sb.Append("	ISNULL(t.Status,0) AS Quan, \n");
            sb.Append("	t.Amount,   \n");
            sb.Append("	ISNULL((CASE WHEN t.Amount >=0 THEN t.Amount END) ,0) AS Charges, \n");
            sb.Append("	ISNULL((CASE WHEN t.Amount < 0 THEN (t.Amount) END),0) AS Credits,   \n");
            sb.Append("	SUM(t.Amount) OVER(ORDER BY t.fDate, t.Id) Balance   \n");
            sb.Append("FROM Trans t  \n");
            sb.Append("	INNER JOIN Chart c ON c.ID = t.Acct \n");
            sb.Append("	INNER JOIN Inv i ON i.ID = t.AcctSub \n");
            sb.Append("	LEFT OUTER JOIN IType it WITH (nolock) ON it.ID = i.Cat \n");
            sb.Append("	LEFT JOIN PJ pj ON pj.Batch = t.Batch \n");
            sb.Append("	LEFT JOIN ReceivePO re ON re.Batch = t.Batch \n");
            sb.Append("WHERE c.Type = 0 \n");
            sb.Append("	AND i.Type = 0  \n");
            sb.Append("	AND t.fDate <= '" + _objInv.EndDate + "' \n");
            sb.Append("	AND t.Type IN (60,97,81,41,70)     \n");

            if (!inclInactive)
            {
                sb.Append("	AND i.Status = 0 \n");
            }

            // Search value
            if (!string.IsNullOrEmpty(_objInv.SearchField) && !string.IsNullOrEmpty(_objInv.SearchValue))
            {
                if (_objInv.SearchField == "Name")
                {
                    sb.Append(" AND i.Name LIKE '%" + _objInv.SearchValue + "%' \n");
                }
                if (_objInv.SearchField == "fDesc")
                {
                    sb.Append(" AND i.fDesc LIKE '%" + _objInv.SearchValue + "%' \n");
                }
                if (_objInv.SearchField == "Status")
                {
                    sb.Append(" AND (CASE WHEN i.Status = 0 THEN 'Active' ELSE 'Inactive' END) LIKE '%" + _objInv.SearchValue + "%' \n");
                }
                if (_objInv.SearchField == "DateCreated")
                {
                    sb.Append(" AND i.DateCreated  LIKE '%" + _objInv.SearchValue + "%' \n");
                }
                if (_objInv.SearchField == "ABCClass")
                {
                    sb.Append(" AND i.ABCClass = " + _objInv.SearchValue + " \n");
                }
                if (_objInv.SearchField == "ShelfLife")
                {
                    sb.Append(" AND i.ShelfLife = " + _objInv.SearchValue + " \n");
                }
                if (_objInv.SearchField == "Commodity")
                {
                    sb.Append(" AND i.Commodity = " + _objInv.SearchValue + " \n");
                }
                if (_objInv.SearchField == "MPN")
                {
                    sb.Append(" AND i.MPN  LIKE '%" + _objInv.SearchValue + "%' \n");
                }
                if (_objInv.SearchField == "ApprovedManufacturer")
                {
                    sb.Append(" AND i.ApprovedManufacturer  LIKE '%" + _objInv.SearchValue + "%' \n");
                }
                if (_objInv.SearchField == "ApprovedVendor")
                {
                    sb.Append(" AND i.ApprovedVendor = " + _objInv.SearchValue + " \n");
                }
            }

            //Inventory filters
            foreach (var filter in filters)
            {
                if (filter.FilterColumn == "Name")
                {
                    sb.Append(" AND i.Name LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "fDesc")
                {
                    sb.Append(" AND i.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "StrStatus")
                {
                    sb.Append(" AND (CASE WHEN i.Status = 0 THEN 'Active' ELSE 'Inactive' END) LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "DateCreated")
                {
                    sb.Append(" AND i.DateCreated  LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "Hand")
                {
                    sb.Append(" AND i.Hand = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "fOrder")
                {
                    sb.Append(" AND i.fOrder = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "Committed")
                {
                    sb.Append(" AND i.Committed = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "Available")
                {
                    sb.Append(" AND i.Committed = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "CatName")
                {
                    sb.Append(" AND it.Type LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "WarehouseCount")
                {
                    sb.Append(" AND i.WarehouseCount= '" + filter.FilterValue + "' \n");
                }
                if (filter.FilterColumn == "UnitCost")
                {
                    sb.Append(" AND CAST ((ISNULL(Inv.Balance,0)) / (CASE ISNULL(Inv.Hand, 0) WHEN 0 THEN 1 ELSE ISNULL(Inv.Hand, 1) END) AS money) = " + filter.FilterValue + " \n");
                }
                
            }

            sb.Append("ORDER BY  t.fDate, t.Id \n");

            return SqlHelper.ExecuteDataset(_objInv.ConnConfig, CommandType.Text, sb.ToString());
        }

        //API
        public DataSet GetInventoryTrans(GetInventoryTransParam _GetInventoryTransParam, string ConnectionString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT   \n");
            sb.Append("	t.ID,  \n");
            sb.Append("	c.Acct + ' - ' + c.fDesc AS InventoryGL,  \n");
            sb.Append("	CASE t.Type  \n");
            sb.Append("		WHEN 41 THEN PJ.ID   \n");
            sb.Append("		WHEN 80 THEN re.ID   \n");
            sb.Append("	ELSE t.Ref END AS Ref, \n");
            sb.Append("	t.fDate,  \n");
            sb.Append("	CASE t.Type  \n");
            sb.Append("		WHEN 60 THEN 'Item Adjustment'   \n");
            sb.Append("		WHEN 97 THEN 'Inventory Item Transfer'  \n");
            sb.Append("		WHEN 81 THEN 'RPO' \n");
            sb.Append("		WHEN 41 THEN 'APBill'  \n");
            sb.Append("		WHEN 70 THEN 'PostToProject/InventoryUsed'  \n");
            sb.Append("	END AS TType, \n");
            sb.Append("	t.fDesc AS MDesc,  \n");
            sb.Append("	t.AcctSub AS INVID, \n");
            sb.Append("	ISNULL(t.Status,0) AS Quan, \n");
            sb.Append("	t.Amount,   \n");
            sb.Append("	ISNULL((CASE WHEN t.Amount >=0 THEN t.Amount END) ,0) AS Charges, \n");
            sb.Append("	ISNULL((CASE WHEN t.Amount < 0 THEN (t.Amount) END),0) AS Credits,   \n");
            sb.Append("	SUM(t.Amount) OVER(ORDER BY t.Id, t.fDate) Balance   \n");
            sb.Append("FROM Trans t  \n");
            sb.Append("	INNER JOIN Chart c ON c.ID = t.Acct \n");
            sb.Append("	INNER JOIN Inv i ON i.ID = t.AcctSub \n");
            sb.Append("	LEFT OUTER JOIN IType it WITH (nolock) ON it.ID = i.Cat \n");
            sb.Append("	LEFT JOIN PJ pj ON pj.Batch = t.Batch \n");
            sb.Append("	LEFT JOIN ReceivePO re ON re.Batch = t.Batch \n");
            sb.Append("WHERE c.Type = 0 \n");
            sb.Append("	AND i.Type = 0  \n");
            sb.Append("	AND t.fDate <= '" + _GetInventoryTransParam.EndDate + "' \n");
            sb.Append("	AND t.Type IN (60,97,81,41,70)     \n");

            if (!_GetInventoryTransParam.inclInactive)
            {
                sb.Append("	AND i.Status = 0 \n");
            }

            // Search value
            if (!string.IsNullOrEmpty(_GetInventoryTransParam.SearchField) && !string.IsNullOrEmpty(_GetInventoryTransParam.SearchValue))
            {
                if (_GetInventoryTransParam.SearchField == "Name")
                {
                    sb.Append(" AND i.Name LIKE '%" + _GetInventoryTransParam.SearchValue + "%' \n");
                }
                if (_GetInventoryTransParam.SearchField == "fDesc")
                {
                    sb.Append(" AND i.fDesc LIKE '%" + _GetInventoryTransParam.SearchValue + "%' \n");
                }
                if (_GetInventoryTransParam.SearchField == "Status")
                {
                    sb.Append(" AND (CASE WHEN i.Status = 0 THEN 'Active' ELSE 'Inactive' END) LIKE '%" + _GetInventoryTransParam.SearchValue + "%' \n");
                }
                if (_GetInventoryTransParam.SearchField == "DateCreated")
                {
                    sb.Append(" AND i.DateCreated  LIKE '%" + _GetInventoryTransParam.SearchValue + "%' \n");
                }
                if (_GetInventoryTransParam.SearchField == "ABCClass")
                {
                    sb.Append(" AND i.ABCClass = " + _GetInventoryTransParam.SearchValue + " \n");
                }
                if (_GetInventoryTransParam.SearchField == "ShelfLife")
                {
                    sb.Append(" AND i.ShelfLife = " + _GetInventoryTransParam.SearchValue + " \n");
                }
                if (_GetInventoryTransParam.SearchField == "Commodity")
                {
                    sb.Append(" AND i.Commodity = " + _GetInventoryTransParam.SearchValue + " \n");
                }
                if (_GetInventoryTransParam.SearchField == "MPN")
                {
                    sb.Append(" AND i.MPN  LIKE '%" + _GetInventoryTransParam.SearchValue + "%' \n");
                }
                if (_GetInventoryTransParam.SearchField == "ApprovedManufacturer")
                {
                    sb.Append(" AND i.ApprovedManufacturer  LIKE '%" + _GetInventoryTransParam.SearchValue + "%' \n");
                }
                if (_GetInventoryTransParam.SearchField == "ApprovedVendor")
                {
                    sb.Append(" AND i.ApprovedVendor = " + _GetInventoryTransParam.SearchValue + " \n");
                }
            }

            //Inventory filters
            foreach (var filter in _GetInventoryTransParam.filters)
            {
                if (filter.FilterColumn == "Name")
                {
                    sb.Append(" AND i.Name LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "fDesc")
                {
                    sb.Append(" AND i.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "StrStatus")
                {
                    sb.Append(" AND (CASE WHEN i.Status = 0 THEN 'Active' ELSE 'Inactive' END) LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "DateCreated")
                {
                    sb.Append(" AND i.DateCreated  LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "Hand")
                {
                    sb.Append(" AND i.Hand = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "fOrder")
                {
                    sb.Append(" AND i.fOrder = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "Committed")
                {
                    sb.Append(" AND i.Committed = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "Available")
                {
                    sb.Append(" AND i.Committed = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "CatName")
                {
                    sb.Append(" AND it.Type LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "WarehouseCount")
                {
                    sb.Append(" AND i.WarehouseCount= '" + filter.FilterValue + "' \n");
                }
                if (filter.FilterColumn == "UnitCost")
                {
                    sb.Append(" AND CAST ((ISNULL(Inv.Balance,0)) / (CASE ISNULL(Inv.Hand, 0) WHEN 0 THEN 1 ELSE ISNULL(Inv.Hand, 1) END) AS money) = " + filter.FilterValue + " \n");
                }

            }

            sb.Append("ORDER BY  t.Id, t.fDate \n");

            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
        }
        public void DeleteInventoryCommodity(string Connstr, string CommodityID)
        {
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@CommodityID";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = CommodityID;

            
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteInventoryWareHouse", objPropUser.WarehouseID);
                SqlHelper.ExecuteNonQuery(Connstr, "spDeleteInventoryCommodity", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
