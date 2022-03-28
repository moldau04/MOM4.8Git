using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.InventoryModel;
using BusinessEntity.Utility;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Web.Script.Serialization;

namespace BusinessLayer
{
    public class BL_Inventory
    {
        DL_Inventory _objDLInventory = new DL_Inventory();
        DL_UnitOfMeasure _objUom = new DL_UnitOfMeasure();
        DL_Itype _objItype = new DL_Itype();
        DL_Chart _objChart = new DL_Chart();
        DL_User _objUser = new DL_User();
        DL_Vendor _objVendor = new DL_Vendor();


        public DataSet GetInventory(Inventory _objInv)
        {


            return _objDLInventory.GetInventory(_objInv);
        }

        public ListGetInventory GetInventory(string ConnectionString, GetInventoryParam _GetInventoryParam)
        {

            DataSet ds = _objDLInventory.GetInventory(ConnectionString, _GetInventoryParam);

            ListGetInventory _ds = new ListGetInventory();
            List<GetInventoryTable1> _lstTable1 = new List<GetInventoryTable1>();
            List<GetInventoryTable2> _lstTable2 = new List<GetInventoryTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetInventoryTable1()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Name = Convert.ToString(dr["Name"]),
                        Part = Convert.ToString(dr["Part"]),
                        StrStatus = Convert.ToString(dr["StrStatus"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        SAcct = Convert.ToInt32(DBNull.Value.Equals(dr["SAcct"]) ? 0 : dr["SAcct"]),
                        Measure = Convert.ToString(dr["Measure"]),
                        Tax = Convert.ToInt32(DBNull.Value.Equals(dr["Tax"]) ? 0 : dr["Tax"]),
                        Price1 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        Price2 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price2"]) ? 0 : dr["Price2"]),
                        Price3 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price3"]) ? 0 : dr["Price3"]),
                        Price4 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price4"]) ? 0 : dr["Price4"]),
                        Price5 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price5"]) ? 0 : dr["Price5"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Cat = Convert.ToInt32(DBNull.Value.Equals(dr["Cat"]) ? 0 : dr["Cat"]),
                        LVendor = Convert.ToInt32(DBNull.Value.Equals(dr["LVendor"]) ? 0 : dr["LVendor"]),
                        LCost = Convert.ToDecimal(DBNull.Value.Equals(dr["LCost"]) ? 0 : dr["LCost"]),
                        AllowZero = Convert.ToInt32(DBNull.Value.Equals(dr["AllowZero"]) ? 0 : dr["AllowZero"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Aisle = Convert.ToString(dr["Aisle"]),
                        Min = Convert.ToDecimal(DBNull.Value.Equals(dr["Min"]) ? 0 : dr["Min"]),
                        Shelf = Convert.ToString(dr["Shelf"]),
                        Bin = Convert.ToString(dr["Bin"]),
                        Requ = Convert.ToDecimal(DBNull.Value.Equals(dr["Requ"]) ? 0 : dr["Requ"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        Price6 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price6"]) ? 0 : dr["Price6"]),
                        QBInvID = Convert.ToString(dr["QBInvID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        QBAccountID = Convert.ToString(dr["QBAccountID"]),
                        IssuedOpenJobs = Convert.ToDecimal(DBNull.Value.Equals(dr["IssuedOpenJobs"]) ? 0 : dr["IssuedOpenJobs"]),
                        Description2 = Convert.ToString(dr["Description2"]),
                        Description3 = Convert.ToString(dr["Description3"]),
                        Description4 = Convert.ToString(dr["Description4"]),
                        DateCreated = Convert.ToDateTime(DBNull.Value.Equals(dr["DateCreated"]) ? null : dr["DateCreated"]),
                        Class = Convert.ToString(dr["Description4"]),
                        Specification = Convert.ToString(dr["Specification"]),
                        Specification2 = Convert.ToString(dr["Specification2"]),
                        Specification3 = Convert.ToString(dr["Specification3"]),
                        Specification4 = Convert.ToString(dr["Specification4"]),
                        Revision = Convert.ToString(dr["Revision"]),
                        LastRevisionDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastRevisionDate"]) ? null : dr["LastRevisionDate"]),
                        Eco = Convert.ToString(dr["Eco"]),
                        Drawing = Convert.ToString(dr["Drawing"]),
                        Reference = Convert.ToString(dr["Reference"]),
                        Length = Convert.ToString(dr["Length"]),
                        Width = Convert.ToString(dr["Width"]),
                        Weight = Convert.ToString(dr["Weight"]),
                        InspectionRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["InspectionRequired"]) ? 0 : dr["InspectionRequired"]),
                        CoCRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["CoCRequired"]) ? 0 : dr["CoCRequired"]),
                        ShelfLife = Convert.ToDecimal(DBNull.Value.Equals(dr["ShelfLife"]) ? 0 : dr["ShelfLife"]),
                        SerializationRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["SerializationRequired"]) ? 0 : dr["SerializationRequired"]),
                        GLcogs = Convert.ToString(dr["GLcogs"]),
                        GLPurchases = Convert.ToString(dr["GLPurchases"]),
                        ABCClass = Convert.ToString(dr["ABCClass"]),
                        OHValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OHValue"]) ? 0 : dr["OHValue"]),
                        OOValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OOValue"]) ? 0 : dr["OOValue"]),
                        OverIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["OverIssueAllowance"]) ? 0 : dr["OverIssueAllowance"]),
                        UnderIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["UnderIssueAllowance"]) ? 0 : dr["UnderIssueAllowance"]),
                        InventoryTurns = Convert.ToDecimal(DBNull.Value.Equals(dr["InventoryTurns"]) ? 0 : dr["InventoryTurns"]),
                        MOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["MOQ"]) ? 0 : dr["MOQ"]),
                        EOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["EOQ"]) ? 0 : dr["EOQ"]),
                        MinInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MinInvQty"]) ? 0 : dr["MinInvQty"]),
                        MaxInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxInvQty"]) ? 0 : dr["MaxInvQty"]),
                        Commodity = Convert.ToString(dr["Commodity"]),
                        LastReceiptDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReceiptDate"]) ? null : dr["LastReceiptDate"]),
                        MPN = Convert.ToString(dr["MPN"]),
                        ApprovedManufacturer = Convert.ToString(dr["ApprovedManufacturer"]),
                        ApprovedVendor = Convert.ToString(dr["ApprovedVendor"]),
                        EAU = Convert.ToDecimal(DBNull.Value.Equals(dr["EAU"]) ? 0 : dr["EAU"]),
                        EOLDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EOLDate"]) ? null : dr["EOLDate"]),
                        WarrantyPeriod = Convert.ToInt32(DBNull.Value.Equals(dr["WarrantyPeriod"]) ? 0 : dr["WarrantyPeriod"]),
                        PODueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PODueDate"]) ? null : dr["PODueDate"]),
                        DefaultReceivingLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultReceivingLocation"]) ? 0 : dr["DefaultReceivingLocation"]),
                        DefaultInspectionLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultInspectionLocation"]) ? 0 : dr["DefaultInspectionLocation"]),
                        LastSalePrice = Convert.ToDecimal(DBNull.Value.Equals(dr["LastSalePrice"]) ? 0 : dr["LastSalePrice"]),
                        AnnualSalesQty = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesQty"]) ? 0 : dr["AnnualSalesQty"]),
                        AnnualSalesAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesAmt"]) ? 0 : dr["AnnualSalesAmt"]),
                        QtyAllocatedToSO = Convert.ToDecimal(DBNull.Value.Equals(dr["QtyAllocatedToSO"]) ? 0 : dr["QtyAllocatedToSO"]),
                        MaxDiscountPercentage = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxDiscountPercentage"]) ? 0 : dr["MaxDiscountPercentage"]),
                        Height = Convert.ToString(dr["Height"]),
                        GLSales = Convert.ToString(dr["GLSales"]),
                        leadTime = Convert.ToInt32(DBNull.Value.Equals(dr["leadTime"]) ? 0 : dr["leadTime"]),
                        DateLastPurchase = Convert.ToDateTime(DBNull.Value.Equals(dr["DateLastPurchase"]) ? null : dr["DateLastPurchase"]),
                        WarehouseCount = Convert.ToInt32(DBNull.Value.Equals(dr["WarehouseCount"]) ? 0 : dr["WarehouseCount"]),
                        Hand = Convert.ToDecimal(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
                        Balance = Convert.ToDecimal(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        fOrder = Convert.ToDecimal(DBNull.Value.Equals(dr["fOrder"]) ? 0 : dr["fOrder"]),
                        Committed = Convert.ToDecimal(DBNull.Value.Equals(dr["Committed"]) ? 0 : dr["Committed"]),
                        Available = Convert.ToDecimal(DBNull.Value.Equals(dr["Available"]) ? 0 : dr["Available"]),
                        UnitCost = Convert.ToDecimal(DBNull.Value.Equals(dr["UnitCost"]) ? 0 : dr["UnitCost"]),
                        catName = Convert.ToString(dr["catName"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetInventoryTable2()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        DisplayName = Convert.ToString(dr["DisplayName"]),
                        MappingColumn = Convert.ToString(dr["MappingColumn"]),

                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;

        }


        public DataSet GetSearchInventory(Inventory _objInv)
        {


            return _objDLInventory.GetSearchInventory(_objInv);
        }

        public ListGetSearchInventory GetSearchInventory(string ConnectionString, GetSearchInventoryParam _GetSearchInventoryParam)
        {
            DataSet ds = _objDLInventory.GetSearchInventory(ConnectionString, _GetSearchInventoryParam);

            ListGetSearchInventory _ds = new ListGetSearchInventory();
            List<GetSearchInventoryTable1> _lstTable1 = new List<GetSearchInventoryTable1>();
            List<GetSearchInventoryTable2> _lstTable2 = new List<GetSearchInventoryTable2>();

            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetSearchInventoryTable1()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Name = Convert.ToString(dr["Name"]),
                        Part = Convert.ToString(dr["Part"]),
                        StrStatus = Convert.ToString(dr["StrStatus"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        SAcct = Convert.ToInt32(DBNull.Value.Equals(dr["SAcct"]) ? 0 : dr["SAcct"]),
                        Measure = Convert.ToString(dr["Measure"]),
                        Tax = Convert.ToInt32(DBNull.Value.Equals(dr["Tax"]) ? 0 : dr["Tax"]),
                        Price1 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        Price2 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price2"]) ? 0 : dr["Price2"]),
                        Price3 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price3"]) ? 0 : dr["Price3"]),
                        Price4 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price4"]) ? 0 : dr["Price4"]),
                        Price5 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price5"]) ? 0 : dr["Price5"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Cat = Convert.ToInt32(DBNull.Value.Equals(dr["Cat"]) ? 0 : dr["Cat"]),
                        LVendor = Convert.ToInt32(DBNull.Value.Equals(dr["LVendor"]) ? 0 : dr["LVendor"]),
                        LCost = Convert.ToDecimal(DBNull.Value.Equals(dr["LCost"]) ? 0 : dr["LCost"]),
                        AllowZero = Convert.ToInt32(DBNull.Value.Equals(dr["AllowZero"]) ? 0 : dr["AllowZero"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Aisle = Convert.ToString(dr["Aisle"]),
                        Min = Convert.ToDecimal(DBNull.Value.Equals(dr["Min"]) ? 0 : dr["Min"]),
                        Shelf = Convert.ToString(dr["Shelf"]),
                        Bin = Convert.ToString(dr["Bin"]),
                        Requ = Convert.ToDecimal(DBNull.Value.Equals(dr["Requ"]) ? 0 : dr["Requ"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        Price6 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price6"]) ? 0 : dr["Price6"]),
                        QBInvID = Convert.ToString(dr["QBInvID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        QBAccountID = Convert.ToString(dr["QBAccountID"]),
                        IssuedOpenJobs = Convert.ToDecimal(DBNull.Value.Equals(dr["IssuedOpenJobs"]) ? 0 : dr["IssuedOpenJobs"]),
                        Description2 = Convert.ToString(dr["Description2"]),
                        Description3 = Convert.ToString(dr["Description3"]),
                        Description4 = Convert.ToString(dr["Description4"]),
                        DateCreated = Convert.ToDateTime(DBNull.Value.Equals(dr["DateCreated"]) ? null : dr["DateCreated"]),
                        Class = Convert.ToString(dr["Description4"]),
                        Specification = Convert.ToString(dr["Specification"]),
                        Specification2 = Convert.ToString(dr["Specification2"]),
                        Specification3 = Convert.ToString(dr["Specification3"]),
                        Specification4 = Convert.ToString(dr["Specification4"]),
                        Revision = Convert.ToString(dr["Revision"]),
                        LastRevisionDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastRevisionDate"]) ? null : dr["LastRevisionDate"]),
                        Eco = Convert.ToString(dr["Eco"]),
                        Drawing = Convert.ToString(dr["Drawing"]),
                        Reference = Convert.ToString(dr["Reference"]),
                        Length = Convert.ToString(dr["Length"]),
                        Width = Convert.ToString(dr["Width"]),
                        Weight = Convert.ToString(dr["Weight"]),
                        InspectionRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["InspectionRequired"]) ? 0 : dr["InspectionRequired"]),
                        CoCRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["CoCRequired"]) ? 0 : dr["CoCRequired"]),
                        ShelfLife = Convert.ToDecimal(DBNull.Value.Equals(dr["ShelfLife"]) ? 0 : dr["ShelfLife"]),
                        SerializationRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["SerializationRequired"]) ? 0 : dr["SerializationRequired"]),
                        GLcogs = Convert.ToString(dr["GLcogs"]),
                        GLPurchases = Convert.ToString(dr["GLPurchases"]),
                        ABCClass = Convert.ToString(dr["ABCClass"]),
                        OHValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OHValue"]) ? 0 : dr["OHValue"]),
                        OOValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OOValue"]) ? 0 : dr["OOValue"]),
                        OverIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["OverIssueAllowance"]) ? 0 : dr["OverIssueAllowance"]),
                        UnderIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["UnderIssueAllowance"]) ? 0 : dr["UnderIssueAllowance"]),
                        InventoryTurns = Convert.ToDecimal(DBNull.Value.Equals(dr["InventoryTurns"]) ? 0 : dr["InventoryTurns"]),
                        MOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["MOQ"]) ? 0 : dr["MOQ"]),
                        EOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["EOQ"]) ? 0 : dr["EOQ"]),
                        MinInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MinInvQty"]) ? 0 : dr["MinInvQty"]),
                        MaxInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxInvQty"]) ? 0 : dr["MaxInvQty"]),
                        Commodity = Convert.ToString(dr["Commodity"]),
                        LastReceiptDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReceiptDate"]) ? null : dr["LastReceiptDate"]),
                        MPN = Convert.ToString(dr["MPN"]),
                        ApprovedManufacturer = Convert.ToString(dr["ApprovedManufacturer"]),
                        ApprovedVendor = Convert.ToString(dr["ApprovedVendor"]),
                        EAU = Convert.ToDecimal(DBNull.Value.Equals(dr["EAU"]) ? 0 : dr["EAU"]),
                        EOLDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EOLDate"]) ? null : dr["EOLDate"]),
                        WarrantyPeriod = Convert.ToInt32(DBNull.Value.Equals(dr["WarrantyPeriod"]) ? 0 : dr["WarrantyPeriod"]),
                        PODueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PODueDate"]) ? null : dr["PODueDate"]),
                        DefaultReceivingLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultReceivingLocation"]) ? 0 : dr["DefaultReceivingLocation"]),
                        DefaultInspectionLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultInspectionLocation"]) ? 0 : dr["DefaultInspectionLocation"]),
                        LastSalePrice = Convert.ToDecimal(DBNull.Value.Equals(dr["LastSalePrice"]) ? 0 : dr["LastSalePrice"]),
                        AnnualSalesQty = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesQty"]) ? 0 : dr["AnnualSalesQty"]),
                        AnnualSalesAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesAmt"]) ? 0 : dr["AnnualSalesAmt"]),
                        QtyAllocatedToSO = Convert.ToDecimal(DBNull.Value.Equals(dr["QtyAllocatedToSO"]) ? 0 : dr["QtyAllocatedToSO"]),
                        MaxDiscountPercentage = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxDiscountPercentage"]) ? 0 : dr["MaxDiscountPercentage"]),
                        Height = Convert.ToString(dr["Height"]),
                        GLSales = Convert.ToString(dr["GLSales"]),
                        leadTime = Convert.ToInt32(DBNull.Value.Equals(dr["leadTime"]) ? 0 : dr["leadTime"]),
                        DateLastPurchase = Convert.ToDateTime(DBNull.Value.Equals(dr["DateLastPurchase"]) ? null : dr["DateLastPurchase"]),
                        WarehouseCount = Convert.ToInt32(DBNull.Value.Equals(dr["WarehouseCount"]) ? 0 : dr["WarehouseCount"]),
                        Hand = Convert.ToDecimal(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
                        Balance = Convert.ToDecimal(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        fOrder = Convert.ToDecimal(DBNull.Value.Equals(dr["fOrder"]) ? 0 : dr["fOrder"]),
                        Committed = Convert.ToDecimal(DBNull.Value.Equals(dr["Committed"]) ? 0 : dr["Committed"]),
                        Available = Convert.ToDecimal(DBNull.Value.Equals(dr["Available"]) ? 0 : dr["Available"]),
                        UnitCost = Convert.ToDecimal(DBNull.Value.Equals(dr["UnitCost"]) ? 0 : dr["UnitCost"]),
                        catName = Convert.ToString(dr["catName"]),
                    }
                    );
            }
            _ds.lstTable1 = _lstTable1;

            if (ds.Tables.Count == 2)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    _lstTable2.Add(
                        new GetSearchInventoryTable2()
                        {
                            Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                            DisplayName = Convert.ToString(dr["DisplayName"]),
                            MappingColumn = Convert.ToString(dr["MappingColumn"]),
                        }
                        );
                }

                _ds.lstTable2 = _lstTable2;
            }

            return _ds;

        }
        public Inventory GetInventoryByID(Inventory _objInv)
        {
            DataSet ds = _objDLInventory.GetInventoryByID(_objInv);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            #region Inventory Info
                            _objInv.Name = ds.Tables[0].Rows[i]["Name"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Name"] : string.Empty;
                            _objInv.fDesc = ds.Tables[0].Rows[i]["fDesc"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["fDesc"] : string.Empty;
                            _objInv.Part = ds.Tables[0].Rows[i]["Part"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Part"] : string.Empty;
                            _objInv.Status = ds.Tables[0].Rows[i]["Status"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Status"]) : 0;
                            _objInv.SAcct = ds.Tables[0].Rows[i]["SAcct"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["SAcct"]) : 0;
                            _objInv.Measure = ds.Tables[0].Rows[i]["Measure"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Measure"] : "0";
                            _objInv.Tax = ds.Tables[0].Rows[i]["Tax"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Tax"]) : 0;
                            _objInv.Balance = ds.Tables[0].Rows[i]["Balance"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Balance"]) : 0;
                            _objInv.Price1 = ds.Tables[0].Rows[i]["Price1"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price1"]) : 0;
                            _objInv.Price2 = ds.Tables[0].Rows[i]["Price2"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price2"]) : 0;
                            _objInv.Price3 = ds.Tables[0].Rows[i]["Price3"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price3"]) : 0;
                            _objInv.Price4 = ds.Tables[0].Rows[i]["Price4"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price4"]) : 0;
                            _objInv.Price5 = ds.Tables[0].Rows[i]["Price5"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price5"]) : 0;
                            _objInv.Remarks = ds.Tables[0].Rows[i]["Remarks"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Remarks"] : string.Empty;
                            _objInv.Cat = ds.Tables[0].Rows[i]["Cat"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Cat"]) : 0;
                            _objInv.LVendor = ds.Tables[0].Rows[i]["LVendor"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["LVendor"]) : 0;
                            _objInv.LCost = ds.Tables[0].Rows[i]["LCost"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["LCost"]) : 0;
                            _objInv.AllowZero = ds.Tables[0].Rows[i]["AllowZero"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["AllowZero"]) : 0;
                            //_objInv.Type = ds.Tables[0].Rows[i]["Type"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Type"]) : 0;
                            _objInv.InUse = ds.Tables[0].Rows[i]["InUse"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["InUse"]) : 0;
                            _objInv.EN = ds.Tables[0].Rows[i]["EN"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EN"]) : 0;
                            _objInv.Hand = ds.Tables[0].Rows[i]["Hand"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Hand"]) : 0;
                            _objInv.Aisle = ds.Tables[0].Rows[i]["Aisle"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Aisle"] : string.Empty;
                            _objInv.fOrder = ds.Tables[0].Rows[i]["fOrder"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["fOrder"]) : 0;
                            _objInv.Min = ds.Tables[0].Rows[i]["Min"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Min"]) : 0;
                            _objInv.Shelf = ds.Tables[0].Rows[i]["Shelf"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Shelf"] : string.Empty;
                            _objInv.Bin = ds.Tables[0].Rows[i]["Bin"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Bin"] : string.Empty;
                            _objInv.Requ = ds.Tables[0].Rows[i]["Requ"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Requ"]) : 0;
                            _objInv.Warehouse = ds.Tables[0].Rows[i]["Warehouse"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Warehouse"] : string.Empty;
                            _objInv.Price6 = ds.Tables[0].Rows[i]["Price6"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price6"]) : 0;
                            _objInv.Committed = ds.Tables[0].Rows[i]["Committed"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Committed"]) : 0;

                            _objInv.QBInvID = ds.Tables[0].Rows[i]["QBInvID"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["QBInvID"] : string.Empty;
                            _objInv.LastUpdateDate = ds.Tables[0].Rows[i]["LastUpdateDate"] != DBNull.Value ? (DateTime)ds.Tables[0].Rows[i]["LastUpdateDate"] : (Nullable<DateTime>)null;
                            _objInv.QBAccountID = ds.Tables[0].Rows[i]["QBAccountID"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["QBAccountID"] : string.Empty;
                            _objInv.Available = ds.Tables[0].Rows[i]["Available"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Available"]) : 0;
                            _objInv.IssuedOpenJobs = ds.Tables[0].Rows[i]["IssuedOpenJobs"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["IssuedOpenJobs"]) : 0;
                            _objInv.Description2 = ds.Tables[0].Rows[i]["Description2"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Description2"] : string.Empty;
                            _objInv.Description3 = ds.Tables[0].Rows[i]["Description3"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Description3"] : string.Empty;
                            _objInv.Description4 = ds.Tables[0].Rows[i]["Description4"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Description4"] : string.Empty;
                            _objInv.DateCreated = ds.Tables[0].Rows[i]["DateCreated"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["DateCreated"]) : (Nullable<DateTime>)null;
                            _objInv.Class = ds.Tables[0].Rows[i]["Class"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Class"] : string.Empty;
                            _objInv.Specification = ds.Tables[0].Rows[i]["Specification"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification"] : string.Empty;
                            _objInv.Specification2 = ds.Tables[0].Rows[i]["Specification2"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification2"] : string.Empty;
                            _objInv.Specification3 = ds.Tables[0].Rows[i]["Specification3"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification3"] : string.Empty;
                            _objInv.Specification4 = ds.Tables[0].Rows[i]["Specification4"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification4"] : string.Empty;
                            _objInv.Revision = ds.Tables[0].Rows[i]["Revision"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Revision"] : string.Empty;
                            _objInv.LastRevisionDate = ds.Tables[0].Rows[i]["LastRevisionDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["LastRevisionDate"]) : (Nullable<DateTime>)null;
                            //_objInv.Eco = ds.Tables[0].Rows[i]["Eco"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Eco"] : string.Empty;
                            //_objInv.Drawing = ds.Tables[0].Rows[i]["Drawing"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Drawing"] : string.Empty;
                            _objInv.Reference = ds.Tables[0].Rows[i]["Reference"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Reference"] : string.Empty;
                            _objInv.Length = ds.Tables[0].Rows[i]["Length"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Length"] : string.Empty;
                            _objInv.Width = ds.Tables[0].Rows[i]["Width"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Width"] : string.Empty;
                            _objInv.Weight = ds.Tables[0].Rows[i]["Weight"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Weight"] : string.Empty;
                            _objInv.InspectionRequired = ds.Tables[0].Rows[i]["InspectionRequired"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["InspectionRequired"] : false;
                            _objInv.CoCRequired = ds.Tables[0].Rows[i]["CoCRequired"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["CoCRequired"] : false;
                            _objInv.ShelfLife = ds.Tables[0].Rows[i]["ShelfLife"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["ShelfLife"]) : 0;
                            _objInv.SerializationRequired = ds.Tables[0].Rows[i]["SerializationRequired"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["SerializationRequired"] : false;
                            _objInv.GLcogs = ds.Tables[0].Rows[i]["GLcogs"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["GLcogs"] : string.Empty;
                            _objInv.GLPurchases = ds.Tables[0].Rows[i]["GLPurchases"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["GLPurchases"] : string.Empty;
                            _objInv.ABCClass = ds.Tables[0].Rows[i]["ABCClass"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["ABCClass"] : string.Empty;
                            _objInv.OHValue = ds.Tables[0].Rows[i]["OHValue"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["OHValue"]) : 0;
                            _objInv.OOValue = ds.Tables[0].Rows[i]["OOValue"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["OOValue"]) : 0;
                            _objInv.OverIssueAllowance = ds.Tables[0].Rows[i]["OverIssueAllowance"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["OverIssueAllowance"] : false;
                            _objInv.UnderIssueAllowance = ds.Tables[0].Rows[i]["UnderIssueAllowance"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["UnderIssueAllowance"] : false;
                            _objInv.InventoryTurns = ds.Tables[0].Rows[i]["InventoryTurns"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["InventoryTurns"]) : 0;
                            _objInv.MOQ = ds.Tables[0].Rows[i]["MOQ"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MOQ"]) : 0;
                            _objInv.EOQ = ds.Tables[0].Rows[i]["EOQ"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EOQ"]) : 0;
                            _objInv.MinInvQty = ds.Tables[0].Rows[i]["MinInvQty"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MinInvQty"]) : 0;
                            _objInv.MaxInvQty = ds.Tables[0].Rows[i]["MaxInvQty"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MaxInvQty"]) : 0;
                            _objInv.Commodity = ds.Tables[0].Rows[i]["Commodity"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Commodity"] : string.Empty;
                            _objInv.LastReceiptDate = ds.Tables[0].Rows[i]["LastReceiptDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["LastReceiptDate"]) : (Nullable<DateTime>)null;
                            _objInv.EAU = ds.Tables[0].Rows[i]["EAU"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EAU"]) : 0;
                            _objInv.EOLDate = ds.Tables[0].Rows[i]["EOLDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["EOLDate"]) : (Nullable<DateTime>)null;
                            _objInv.WarrantyPeriod = ds.Tables[0].Rows[i]["WarrantyPeriod"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["WarrantyPeriod"]) : (Nullable<int>)null;
                            _objInv.PODueDate = ds.Tables[0].Rows[i]["PODueDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["PODueDate"]) : (Nullable<DateTime>)null;
                            _objInv.DefaultReceivingLocation = ds.Tables[0].Rows[i]["DefaultReceivingLocation"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["DefaultReceivingLocation"] : false;
                            _objInv.DefaultInspectionLocation = ds.Tables[0].Rows[i]["DefaultInspectionLocation"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["DefaultInspectionLocation"] : false;
                            _objInv.LastSalePrice = ds.Tables[0].Rows[i]["LastSalePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["LastSalePrice"]) : 0;
                            _objInv.AnnualSalesQty = ds.Tables[0].Rows[i]["AnnualSalesQty"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["AnnualSalesQty"]) : 0;
                            _objInv.AnnualSalesAmt = ds.Tables[0].Rows[i]["AnnualSalesAmt"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["AnnualSalesAmt"]) : 0;
                            _objInv.QtyAllocatedToSO = ds.Tables[0].Rows[i]["QtyAllocatedToSO"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["QtyAllocatedToSO"]) : 0;
                            _objInv.MaxDiscountPercentage = ds.Tables[0].Rows[i]["MaxDiscountPercentage"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MaxDiscountPercentage"]) : 0;
                            _objInv.Height = ds.Tables[0].Rows[i]["Height"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Height"]) : string.Empty;
                            _objInv.UnitCost = ds.Tables[0].Rows[i]["UnitCost"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["UnitCost"]) : 0;
                            _objInv.GLSales = ds.Tables[0].Rows[i]["GLSales"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["GLSales"]) : string.Empty;
                            _objInv.LeadTime = ds.Tables[0].Rows[i]["LeadTime"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["LeadTime"]) : 0;
                            #endregion
                        }

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<InvParts> invParts = new List<InvParts>();

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            #region Vendor Info
                            InvParts vendorinfo = new InvParts();
                            vendorinfo.ID = ds.Tables[1].Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["ID"]) : 0;
                            vendorinfo.ItemID = ds.Tables[1].Rows[i]["ItemID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["ItemID"]) : 0;
                            vendorinfo.MPN = ds.Tables[1].Rows[i]["MPN"] != DBNull.Value ? (string)ds.Tables[1].Rows[i]["MPN"] : string.Empty;
                            vendorinfo.Part = ds.Tables[1].Rows[i]["Part"] != DBNull.Value ? (string)ds.Tables[1].Rows[i]["Part"] : string.Empty;
                            vendorinfo.Supplier = ds.Tables[1].Rows[i]["Supplier"] != DBNull.Value ? (string)ds.Tables[1].Rows[i]["Supplier"] : string.Empty;
                            vendorinfo.Price = ds.Tables[1].Rows[i]["Price"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["Price"]) : 0;
                            vendorinfo.VendorID = ds.Tables[1].Rows[i]["VendorID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["VendorID"]) : 0;
                            vendorinfo.Mfg = ds.Tables[1].Rows[i]["Mfg"] != DBNull.Value ? (string)ds.Tables[1].Rows[i]["Mfg"] : string.Empty;
                            vendorinfo.MfgPrice = ds.Tables[1].Rows[i]["MfgPrice"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["MfgPrice"]) : 0;

                            invParts.Add(vendorinfo);
                            #endregion
                        }

                        _objInv.InvPartslist = invParts.ToArray();

                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        List<InvItemRev> InvItemRev = new List<InvItemRev>();

                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            #region Revision Info
                            InvItemRev Revisioninfo = new InvItemRev();
                            Revisioninfo.ID = ds.Tables[2].Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[i]["ID"]) : 0;
                            Revisioninfo.InvID = ds.Tables[2].Rows[i]["InvID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[i]["InvID"]) : 0;
                            Revisioninfo.Date = ds.Tables[2].Rows[i]["Date"] != DBNull.Value ? (DateTime)ds.Tables[2].Rows[i]["Date"] : DateTime.Now;
                            Revisioninfo.Version = ds.Tables[2].Rows[i]["Version"] != DBNull.Value ? (string)ds.Tables[2].Rows[i]["Version"] : string.Empty;
                            Revisioninfo.Comment = ds.Tables[2].Rows[i]["Comment"] != DBNull.Value ? (string)ds.Tables[2].Rows[i]["Comment"] : string.Empty;
                            Revisioninfo.Eco = ds.Tables[2].Rows[i]["Eco"] != DBNull.Value ? (string)ds.Tables[2].Rows[i]["Eco"] : string.Empty;
                            Revisioninfo.Drawing = ds.Tables[2].Rows[i]["Drawing"] != DBNull.Value ? (string)ds.Tables[2].Rows[i]["Drawing"] : string.Empty;


                            InvItemRev.Add(Revisioninfo);
                            #endregion
                        }

                        _objInv.InvItemRevlist = InvItemRev.ToArray();

                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        List<InvWarehouse> invInvWarehouse = new List<InvWarehouse>();

                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            #region InvWarehouseinfo
                            InvWarehouse InvWarehouseinfo = new InvWarehouse();
                            InvWarehouseinfo.ID = ds.Tables[3].Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[3].Rows[i]["ID"]) : 0;
                            InvWarehouseinfo.InvID = ds.Tables[3].Rows[i]["InvID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[3].Rows[i]["InvID"]) : 0;
                            InvWarehouseinfo.WarehouseID = ds.Tables[3].Rows[i]["WarehouseID"] != DBNull.Value ? (string)ds.Tables[3].Rows[i]["WarehouseID"] : string.Empty;
                            InvWarehouseinfo.WarehouseName = ds.Tables[3].Rows[i]["WarehouseName"] != DBNull.Value ? (string)ds.Tables[3].Rows[i]["WarehouseName"] : string.Empty;
                            InvWarehouseinfo.Hand = ds.Tables[3].Rows[i]["Hand"] != DBNull.Value ? (decimal)ds.Tables[3].Rows[i]["Hand"] : 0;
                            InvWarehouseinfo.Balance = ds.Tables[3].Rows[i]["Balance"] != DBNull.Value ? (decimal)ds.Tables[3].Rows[i]["Balance"] : 0;
                            InvWarehouseinfo.Committed = ds.Tables[3].Rows[i]["Committed"] != DBNull.Value ? (decimal)ds.Tables[3].Rows[i]["Committed"] : 0;
                            InvWarehouseinfo.fOrder = ds.Tables[3].Rows[i]["fOrder"] != DBNull.Value ? (decimal)ds.Tables[3].Rows[i]["fOrder"] : 0;
                            InvWarehouseinfo.Available = ds.Tables[3].Rows[i]["Available"] != DBNull.Value ? (decimal)ds.Tables[3].Rows[i]["Available"] : 0;
                            InvWarehouseinfo.Company = ds.Tables[3].Rows[i]["Company"] != DBNull.Value ? (string)ds.Tables[3].Rows[i]["Company"] : string.Empty;
                            InvWarehouseinfo.EN = ds.Tables[3].Rows[i]["EN"] != DBNull.Value ? Convert.ToInt32(ds.Tables[3].Rows[i]["EN"]) : 0;

                            invInvWarehouse.Add(InvWarehouseinfo);
                            #endregion
                        }

                        _objInv.InvWarehouseMergelist = invInvWarehouse.ToArray();

                    }

                }


            }


            return _objInv;
        }

        //API
        //public InventoryViewModel GetInventoryByID(GetInventoryByIDParam _GetInventoryByIDParam)
        //{
        //    DataSet ds = _objDLInventory.GetInventoryByID(_GetInventoryByIDParam);

           


        //    return _GetInventoryByIDParam;
        //}


        public DataSet GetAllInventory(Inventory _objInv)
        {


            return _objDLInventory.GetALLInventory(_objInv);
        }

        public ListGetALLInventory GetAllInventory(string ConnectionString, GetALLInventoryParam _GetALLInventoryParam)
        {
            DataSet ds = _objDLInventory.GetALLInventory(ConnectionString, _GetALLInventoryParam);

            ListGetALLInventory _ds = new ListGetALLInventory();
            List<GetALLInventoryTable1> _lstTable1 = new List<GetALLInventoryTable1>();
            List<GetALLInventoryTable2> _lstTable2 = new List<GetALLInventoryTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetALLInventoryTable1()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Name = Convert.ToString(dr["Name"]),
                        Part = Convert.ToString(dr["Part"]),
                        StrStatus = Convert.ToString(dr["StrStatus"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        SAcct = Convert.ToInt32(DBNull.Value.Equals(dr["SAcct"]) ? 0 : dr["SAcct"]),
                        Measure = Convert.ToString(dr["Measure"]),
                        Tax = Convert.ToInt32(DBNull.Value.Equals(dr["Tax"]) ? 0 : dr["Tax"]),
                        Price1 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        Price2 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price2"]) ? 0 : dr["Price2"]),
                        Price3 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price3"]) ? 0 : dr["Price3"]),
                        Price4 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price4"]) ? 0 : dr["Price4"]),
                        Price5 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price5"]) ? 0 : dr["Price5"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Cat = Convert.ToInt32(DBNull.Value.Equals(dr["Cat"]) ? 0 : dr["Cat"]),
                        LVendor = Convert.ToInt32(DBNull.Value.Equals(dr["LVendor"]) ? 0 : dr["LVendor"]),
                        LCost = Convert.ToDecimal(DBNull.Value.Equals(dr["LCost"]) ? 0 : dr["LCost"]),
                        AllowZero = Convert.ToInt32(DBNull.Value.Equals(dr["AllowZero"]) ? 0 : dr["AllowZero"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Aisle = Convert.ToString(dr["Aisle"]),
                        Min = Convert.ToDecimal(DBNull.Value.Equals(dr["Min"]) ? 0 : dr["Min"]),
                        Shelf = Convert.ToString(dr["Shelf"]),
                        Bin = Convert.ToString(dr["Bin"]),
                        Requ = Convert.ToDecimal(DBNull.Value.Equals(dr["Requ"]) ? 0 : dr["Requ"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        Price6 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price6"]) ? 0 : dr["Price6"]),
                        QBInvID = Convert.ToString(dr["QBInvID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        QBAccountID = Convert.ToString(dr["QBAccountID"]),
                        IssuedOpenJobs = Convert.ToDecimal(DBNull.Value.Equals(dr["IssuedOpenJobs"]) ? 0 : dr["IssuedOpenJobs"]),
                        Description2 = Convert.ToString(dr["Description2"]),
                        Description3 = Convert.ToString(dr["Description3"]),
                        Description4 = Convert.ToString(dr["Description4"]),
                        DateCreated = Convert.ToDateTime(DBNull.Value.Equals(dr["DateCreated"]) ? null : dr["DateCreated"]),
                        Class = Convert.ToString(dr["Description4"]),
                        Specification = Convert.ToString(dr["Specification"]),
                        Specification2 = Convert.ToString(dr["Specification2"]),
                        Specification3 = Convert.ToString(dr["Specification3"]),
                        Specification4 = Convert.ToString(dr["Specification4"]),
                        Revision = Convert.ToString(dr["Revision"]),
                        LastRevisionDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastRevisionDate"]) ? null : dr["LastRevisionDate"]),
                        Eco = Convert.ToString(dr["Eco"]),
                        Drawing = Convert.ToString(dr["Drawing"]),
                        Reference = Convert.ToString(dr["Reference"]),
                        Length = Convert.ToString(dr["Length"]),
                        Width = Convert.ToString(dr["Width"]),
                        Weight = Convert.ToString(dr["Weight"]),
                        InspectionRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["InspectionRequired"]) ? 0 : dr["InspectionRequired"]),
                        CoCRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["CoCRequired"]) ? 0 : dr["CoCRequired"]),
                        ShelfLife = Convert.ToDecimal(DBNull.Value.Equals(dr["ShelfLife"]) ? 0 : dr["ShelfLife"]),
                        SerializationRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["SerializationRequired"]) ? 0 : dr["SerializationRequired"]),
                        GLcogs = Convert.ToString(dr["GLcogs"]),
                        GLPurchases = Convert.ToString(dr["GLPurchases"]),
                        ABCClass = Convert.ToString(dr["ABCClass"]),
                        OHValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OHValue"]) ? 0 : dr["OHValue"]),
                        OOValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OOValue"]) ? 0 : dr["OOValue"]),
                        OverIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["OverIssueAllowance"]) ? 0 : dr["OverIssueAllowance"]),
                        UnderIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["UnderIssueAllowance"]) ? 0 : dr["UnderIssueAllowance"]),
                        InventoryTurns = Convert.ToDecimal(DBNull.Value.Equals(dr["InventoryTurns"]) ? 0 : dr["InventoryTurns"]),
                        MOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["MOQ"]) ? 0 : dr["MOQ"]),
                        EOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["EOQ"]) ? 0 : dr["EOQ"]),
                        MinInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MinInvQty"]) ? 0 : dr["MinInvQty"]),
                        MaxInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxInvQty"]) ? 0 : dr["MaxInvQty"]),
                        Commodity = Convert.ToString(dr["Commodity"]),
                        LastReceiptDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReceiptDate"]) ? null : dr["LastReceiptDate"]),
                        MPN = Convert.ToString(dr["MPN"]),
                        ApprovedManufacturer = Convert.ToString(dr["ApprovedManufacturer"]),
                        ApprovedVendor = Convert.ToString(dr["ApprovedVendor"]),
                        EAU = Convert.ToDecimal(DBNull.Value.Equals(dr["EAU"]) ? 0 : dr["EAU"]),
                        EOLDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EOLDate"]) ? null : dr["EOLDate"]),
                        WarrantyPeriod = Convert.ToInt32(DBNull.Value.Equals(dr["WarrantyPeriod"]) ? 0 : dr["WarrantyPeriod"]),
                        PODueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PODueDate"]) ? null : dr["PODueDate"]),
                        DefaultReceivingLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultReceivingLocation"]) ? 0 : dr["DefaultReceivingLocation"]),
                        DefaultInspectionLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultInspectionLocation"]) ? 0 : dr["DefaultInspectionLocation"]),
                        LastSalePrice = Convert.ToDecimal(DBNull.Value.Equals(dr["LastSalePrice"]) ? 0 : dr["LastSalePrice"]),
                        AnnualSalesQty = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesQty"]) ? 0 : dr["AnnualSalesQty"]),
                        AnnualSalesAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesAmt"]) ? 0 : dr["AnnualSalesAmt"]),
                        QtyAllocatedToSO = Convert.ToDecimal(DBNull.Value.Equals(dr["QtyAllocatedToSO"]) ? 0 : dr["QtyAllocatedToSO"]),
                        MaxDiscountPercentage = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxDiscountPercentage"]) ? 0 : dr["MaxDiscountPercentage"]),
                        Height = Convert.ToString(dr["Height"]),
                        GLSales = Convert.ToString(dr["GLSales"]),
                        leadTime = Convert.ToInt32(DBNull.Value.Equals(dr["leadTime"]) ? 0 : dr["leadTime"]),
                        DateLastPurchase = Convert.ToDateTime(DBNull.Value.Equals(dr["DateLastPurchase"]) ? null : dr["DateLastPurchase"]),
                        WarehouseCount = Convert.ToInt32(DBNull.Value.Equals(dr["WarehouseCount"]) ? 0 : dr["WarehouseCount"]),
                        Hand = Convert.ToDecimal(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
                        Balance = Convert.ToDecimal(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        fOrder = Convert.ToDecimal(DBNull.Value.Equals(dr["fOrder"]) ? 0 : dr["fOrder"]),
                        Committed = Convert.ToDecimal(DBNull.Value.Equals(dr["Committed"]) ? 0 : dr["Committed"]),
                        Available = Convert.ToDecimal(DBNull.Value.Equals(dr["Available"]) ? 0 : dr["Available"]),
                        UnitCost = Convert.ToDecimal(DBNull.Value.Equals(dr["UnitCost"]) ? 0 : dr["UnitCost"]),
                        catName = Convert.ToString(dr["catName"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetALLInventoryTable2()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        DisplayName = Convert.ToString(dr["DisplayName"]),
                        MappingColumn = Convert.ToString(dr["MappingColumn"]),

                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;

        }
        public DataSet GetInventoryTransactionByInvID(Inventory _objInv)
        {


            return _objDLInventory.GetInventoryTransactionByInvID(_objInv);
        }

        //API
        public List<InventoryTransactionByInvIDViewModel> GetInventoryTransactionByInvID(GetInventoryTransactionByInvIDParam _GetInventoryTransactionByInvIDParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.GetInventoryTransactionByInvID(_GetInventoryTransactionByInvIDParam, ConnectionString);
            List<InventoryTransactionByInvIDViewModel> _lstInventoryTransaction = new List<InventoryTransactionByInvIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstInventoryTransaction.Add(
                    new InventoryTransactionByInvIDViewModel()
                    {
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        URLref = Convert.ToString(dr["URLref"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        TType = Convert.ToString(dr["TType"]),
                        MDesc = Convert.ToString(dr["MDesc"]),
                        INVID = Convert.ToInt32(DBNull.Value.Equals(dr["INVID"]) ? 0 : dr["INVID"]),
                        Quan = Convert.ToString(dr["Quan"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Charges = Convert.ToDouble(DBNull.Value.Equals(dr["Charges"]) ? 0 : dr["Charges"]),
                        Credits = Convert.ToDouble(DBNull.Value.Equals(dr["Credits"]) ? 0 : dr["Credits"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    }
                    );
            }

            return _lstInventoryTransaction;
        }
        public DataSet DeleteInventory(string inventoryxml)
        {
            return _objDLInventory.DeleteInventory(inventoryxml);
        }

        public String DeleteInventoryByInvID(Inventory _objInv)
        {
            return _objDLInventory.DeleteInventoryByInvID(_objInv);
        }

        public String DeleteInventoryByInvID(string ConnectionString, DeleteInventoryByInvIDParam _DeleteInventoryByInvIDParam)
        {
            String Retval = _objDLInventory.DeleteInventoryByInvID(ConnectionString, _DeleteInventoryByInvIDParam);

            return Retval;
        }

        public List<UnitOfMeasure> GetALLUnitOfMeasure()
        {


            return _objUom.GetALLUnitOfMeasure();
        }

        public List<Itype> GetALLItype()
        {
            return _objItype.GetALLItype();
        }

        public List<Chart> GetChartByType(int type)
        {
            return _objChart.GetChartByType(type);
        }

        //API
        public List<Chart> GetChartByType(GetChartByTypeParam _GetChartByTypeParam, string ConnectionString)
        {
            return _objChart.GetChartByType(_GetChartByTypeParam, ConnectionString);
        }

        public bool ChkWareHouseExistForInv(int inv)
        {
            return _objDLInventory.ChkWareHouseExistForInv(inv);
        }

        public bool chkInvForOpen(int inv)
        {
            return  _objDLInventory.chkInvForOpen(inv);
        }

        //API
        public bool chkInvForOpen(checkkInvForOpenParam _checkkInvForOpenParam, string ConnectionString)
        {
            return _objDLInventory.chkInvForOpen(_checkkInvForOpenParam, ConnectionString);
        }

        public DataSet chkStatusOfChart(int ID)
        {
            return _objDLInventory.chkStatusOfChart(ID);
        }

        //API
        public List<CheckStatusOfChartViewModel> chkStatusOfChart(chkStatusOfChartParam _chkStatusOfChartParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.chkStatusOfChart(_chkStatusOfChartParam, ConnectionString);

            List<CheckStatusOfChartViewModel> _lstCheckStatusOfChart = new List<CheckStatusOfChartViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCheckStatusOfChart.Add(
                    new CheckStatusOfChartViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        status = Convert.ToInt32(DBNull.Value.Equals(dr["status"]) ? 0 : dr["status"]),
                    }
                    );
            }
            return _lstCheckStatusOfChart;
        }


        public int CheckWarehouseIsActive(string WarehouseID)
        {
            return _objDLInventory.CheckWarehouseIsActive(WarehouseID);
        }

        public int CheckWarehouseIsActive(CheckWarehouseIsActiveParam _CheckWarehouseIsActiveParam, string ConnectionString)
        {
            return _objDLInventory.CheckWarehouseIsActive(_CheckWarehouseIsActiveParam, ConnectionString);
        }

        public string GetPartNumber(string strInventory)
        {
            return _objDLInventory.GetPartNumber(strInventory);
        }

        //API
        public string GetPartNumber(GetPartNumberParam _GetPartNumberParam, string ConnectionString)
        {
            return _objDLInventory.GetPartNumber(_GetPartNumberParam, ConnectionString);
        }


        public Inventory CreateInventory(Inventory inv)
        {
            Inventory invdt = null;

            if (inv != null)
            {

                string strxmlparam = string.Empty;
                strxmlparam += "<Inventory>";
                strxmlparam += "<Item>";
                #region Xml Param
                strxmlparam += "<Name>" + inv.Name + "</Name>";
                strxmlparam += "<fDesc>" + inv.fDesc + "</fDesc>";
                strxmlparam += "<Part>" + inv.MPN + "</Part>";
                strxmlparam += "<Status>" + inv.Status + "</Status>";
                strxmlparam += "<SAcct>" + inv.SAcct + "</SAcct>";
                strxmlparam += "<Measure>" + inv.Measure + "</Measure>";
                strxmlparam += "<Tax>" + inv.Tax + "</Tax>";
                strxmlparam += "<Balance>" + inv.Balance + "</Balance>";
                strxmlparam += "<Price1>" + inv.Price1 + "</Price1>";
                strxmlparam += "<Price2>" + inv.Price2 + "</Price2>";
                strxmlparam += "<Price3>" + inv.Price3 + "</Price3>";
                strxmlparam += "<Price4>" + inv.Price4 + "</Price4>";
                strxmlparam += "<Price5>" + inv.Price5 + "</Price5>";
                strxmlparam += "<Remarks>" + inv.Remarks + "</Remarks>";
                strxmlparam += "<Cat>" + inv.Cat + "</Cat>";
                strxmlparam += "<LVendor>" + inv.LVendor + "</LVendor>";
                strxmlparam += "<LCost>" + inv.LCost + "</LCost>";
                strxmlparam += "<AllowZero>" + inv.AllowZero + "</AllowZero>";
                // strxmlparam += "<Type>" + inv.Type + "</Type>";
                strxmlparam += "<InUse>" + inv.InUse + "</InUse>";
                strxmlparam += "<EN>" + inv.EN + "</EN>";
                strxmlparam += "<Hand>" + inv.Hand + "</Hand>";
                strxmlparam += "<Aisle>" + inv.Aisle + "</Aisle>";
                strxmlparam += "<fOrder>" + inv.fOrder + "</fOrder>";
                strxmlparam += "<Min>" + inv.Min + "</Min>";
                strxmlparam += "<Shelf>" + inv.Shelf + "</Shelf>";
                strxmlparam += "<Bin>" + inv.Bin + "</Bin>";
                strxmlparam += "<Requ>" + inv.Requ + "</Requ>";
                strxmlparam += "<Warehouse>" + inv.Warehouse + "</Warehouse>";
                strxmlparam += "<Price6>" + inv.Price6 + "</Price6>";
                strxmlparam += "<Committed>" + inv.Committed + "</Committed>";
                strxmlparam += "<QBInvID>" + inv.QBInvID + "</QBInvID>";
                strxmlparam += "<LastUpdateDate>" + inv.LastUpdateDate + "</LastUpdateDate>";
                strxmlparam += "<QBAccountID>" + inv.QBAccountID + "</QBAccountID>";
                strxmlparam += "<Available>" + inv.Available + "</Available>";
                strxmlparam += "<IssuedOpenJobs>" + inv.IssuedOpenJobs + "</IssuedOpenJobs>";
                strxmlparam += "<Description2>" + inv.Description2 + "</Description2>";
                strxmlparam += "<Description3>" + inv.Description3 + "</Description3>";
                strxmlparam += "<Description4>" + inv.Description4 + "</Description4>";
                if (inv.DateCreated != null & inv.DateCreated != DateTime.MinValue)
                {
                    strxmlparam += "<DateCreated>" + inv.DateCreated + "</DateCreated>";
                }
                else
                {
                    strxmlparam += "<DateCreated></DateCreated>";
                }

                strxmlparam += "<Specification>" + inv.Specification + "</Specification>";
                strxmlparam += "<Specification2>" + inv.Specification2 + "</Specification2>";
                strxmlparam += "<Specification3>" + inv.Specification2 + "</Specification3>";
                strxmlparam += "<Specification4>" + inv.Specification4 + "</Specification4>";
                strxmlparam += "<Revision>" + inv.Revision + "</Revision>";
                if (inv.LastRevisionDate != null & inv.LastRevisionDate != DateTime.MinValue)
                {
                    strxmlparam += "<LastRevisionDate>" + inv.LastRevisionDate + "</LastRevisionDate>";
                }
                else
                {
                    strxmlparam += "<LastRevisionDate></LastRevisionDate>";
                }
                strxmlparam += "<Eco>" + inv.Eco + "</Eco>";
                strxmlparam += "<Drawing>" + inv.Drawing + "</Drawing>";
                strxmlparam += "<Reference>" + inv.Reference + "</Reference>";
                strxmlparam += "<Length>" + inv.Length + "</Length>";
                strxmlparam += "<Width>" + inv.Width + "</Width>";
                strxmlparam += "<Weight>" + inv.Weight + "</Weight>";
                strxmlparam += "<InspectionRequired>" + inv.InspectionRequired + "</InspectionRequired>";
                strxmlparam += "<CoCRequired>" + inv.CoCRequired + "</CoCRequired>";
                strxmlparam += "<ShelfLife>" + inv.ShelfLife + "</ShelfLife>";
                strxmlparam += "<SerializationRequired>" + inv.SerializationRequired + "</SerializationRequired>";
                strxmlparam += "<GLcogs>" + inv.GLcogs + "</GLcogs>";
                strxmlparam += "<GLPurchases>" + inv.GLPurchases + "</GLPurchases>";
                strxmlparam += "<ABCClass>" + inv.ABCClass + "</ABCClass>";
                strxmlparam += "<OHValue>" + inv.OHValue + "</OHValue>";
                strxmlparam += "<OOValue>" + inv.OOValue + "</OOValue>";
                strxmlparam += "<OverIssueAllowance>" + inv.OverIssueAllowance + "</OverIssueAllowance>";
                strxmlparam += "<UnderIssueAllowance>" + inv.UnderIssueAllowance + "</UnderIssueAllowance>";
                strxmlparam += "<InventoryTurns>" + inv.InventoryTurns + "</InventoryTurns>";
                strxmlparam += "<MOQ>" + inv.MOQ + "</MOQ>";
                strxmlparam += "<MinInvQty>" + inv.MinInvQty + "</MinInvQty>";
                strxmlparam += "<MaxInvQty>" + inv.MaxInvQty + "</MaxInvQty>";
                strxmlparam += "<Commodity>" + inv.Commodity + "</Commodity>";

                if (inv.LastReceiptDate != null & inv.LastReceiptDate != DateTime.MinValue)
                {
                    strxmlparam += "<LastReceiptDate>" + inv.LastReceiptDate + "</LastReceiptDate>";
                }
                else
                {
                    strxmlparam += "<LastReceiptDate></LastReceiptDate>";
                }


                strxmlparam += "<EAU>" + inv.EAU + "</EAU>";
                if (inv.EOLDate != null & inv.EOLDate != DateTime.MinValue)
                {
                    strxmlparam += "<EOLDate>" + inv.EOLDate + "</EOLDate>";
                }
                else
                {
                    strxmlparam += "<EOLDate></EOLDate>";
                }

                strxmlparam += "<WarrantyPeriod>" + inv.WarrantyPeriod + "</WarrantyPeriod>";

                if (inv.PODueDate != null & inv.PODueDate != DateTime.MinValue)
                {
                    strxmlparam += "<PODueDate>" + inv.PODueDate + "</PODueDate>";
                }
                else
                {
                    strxmlparam += "<PODueDate></PODueDate>";
                }

                strxmlparam += "<DefaultReceivingLocation>" + inv.DefaultReceivingLocation + "</DefaultReceivingLocation>";
                strxmlparam += "<DefaultInspectionLocation>" + inv.DefaultInspectionLocation + "</DefaultInspectionLocation>";
                strxmlparam += "<LastSalePrice>" + inv.LastSalePrice + "</LastSalePrice>";
                strxmlparam += "<AnnualSalesQty>" + inv.AnnualSalesQty + "</AnnualSalesQty>";
                strxmlparam += "<AnnualSalesAmt>" + inv.AnnualSalesAmt + "</AnnualSalesAmt>";
                strxmlparam += "<QtyAllocatedToSO>" + inv.QtyAllocatedToSO + "</QtyAllocatedToSO>";
                strxmlparam += "<MaxDiscountPercentage>" + inv.MaxDiscountPercentage + "</MaxDiscountPercentage>";
                // strxmlparam += "<ITypeCategory>" + inv.Type + "</ITypeCategory>";
                strxmlparam += "<Height>" + inv.Height + "</Height>";
                strxmlparam += "<UnitCost>" + inv.UnitCost + "</UnitCost>";
                strxmlparam += "<GLSales>" + inv.GLSales + "</GLSales>";
                strxmlparam += "<EOQ>" + inv.EOQ + "</EOQ>";
                strxmlparam += "<LeadTime>" + inv.LeadTime + "</LeadTime>";
                #endregion
                strxmlparam += "</Item>";
                strxmlparam += "</Inventory>";

                invdt = _objDLInventory.CreateInventory(strxmlparam, inv);


            }


            return invdt;

        }

        //public InventoryViewModel CreateInventory(CreateInventoryParam _CreateInventoryParam, string ConnectionString)
        //{
        //    Inventory invdt = null;
        //    List<InventoryViewModel> _lstInventoryViewModel = new List<InventoryViewModel>();
        //    _lstInventoryViewModel = null;

        //    if (_CreateInventoryParam != null)
        //    {

        //        string strxmlparam = string.Empty;
        //        strxmlparam += "<Inventory>";
        //        strxmlparam += "<Item>";
        //        #region Xml Param
        //        strxmlparam += "<Name>" + _CreateInventoryParam.Name + "</Name>";
        //        strxmlparam += "<fDesc>" + _CreateInventoryParam.fDesc + "</fDesc>";
        //        strxmlparam += "<Part>" + _CreateInventoryParam.MPN + "</Part>";
        //        strxmlparam += "<Status>" + _CreateInventoryParam.Status + "</Status>";
        //        strxmlparam += "<SAcct>" + _CreateInventoryParam.SAcct + "</SAcct>";
        //        strxmlparam += "<Measure>" + _CreateInventoryParam.Measure + "</Measure>";
        //        strxmlparam += "<Tax>" + _CreateInventoryParam.Tax + "</Tax>";
        //        strxmlparam += "<Balance>" + _CreateInventoryParam.Balance + "</Balance>";
        //        strxmlparam += "<Price1>" + _CreateInventoryParam.Price1 + "</Price1>";
        //        strxmlparam += "<Price2>" + _CreateInventoryParam.Price2 + "</Price2>";
        //        strxmlparam += "<Price3>" + _CreateInventoryParam.Price3 + "</Price3>";
        //        strxmlparam += "<Price4>" + _CreateInventoryParam.Price4 + "</Price4>";
        //        strxmlparam += "<Price5>" + _CreateInventoryParam.Price5 + "</Price5>";
        //        strxmlparam += "<Remarks>" + _CreateInventoryParam.Remarks + "</Remarks>";
        //        strxmlparam += "<Cat>" + _CreateInventoryParam.Cat + "</Cat>";
        //        strxmlparam += "<LVendor>" + _CreateInventoryParam.LVendor + "</LVendor>";
        //        strxmlparam += "<LCost>" + _CreateInventoryParam.LCost + "</LCost>";
        //        strxmlparam += "<AllowZero>" + _CreateInventoryParam.AllowZero + "</AllowZero>";
        //        // strxmlparam += "<Type>" + _CreateInventoryParam.Type + "</Type>";
        //        strxmlparam += "<InUse>" + _CreateInventoryParam.InUse + "</InUse>";
        //        strxmlparam += "<EN>" + _CreateInventoryParam.EN + "</EN>";
        //        strxmlparam += "<Hand>" + _CreateInventoryParam.Hand + "</Hand>";
        //        strxmlparam += "<Aisle>" + _CreateInventoryParam.Aisle + "</Aisle>";
        //        strxmlparam += "<fOrder>" + _CreateInventoryParam.fOrder + "</fOrder>";
        //        strxmlparam += "<Min>" + _CreateInventoryParam.Min + "</Min>";
        //        strxmlparam += "<Shelf>" + _CreateInventoryParam.Shelf + "</Shelf>";
        //        strxmlparam += "<Bin>" + _CreateInventoryParam.Bin + "</Bin>";
        //        strxmlparam += "<Requ>" + _CreateInventoryParam.Requ + "</Requ>";
        //        strxmlparam += "<Warehouse>" + _CreateInventoryParam.Warehouse + "</Warehouse>";
        //        strxmlparam += "<Price6>" + _CreateInventoryParam.Price6 + "</Price6>";
        //        strxmlparam += "<Committed>" + _CreateInventoryParam.Committed + "</Committed>";
        //        strxmlparam += "<QBInvID>" + _CreateInventoryParam.QBInvID + "</QBInvID>";
        //        strxmlparam += "<LastUpdateDate>" + _CreateInventoryParam.LastUpdateDate + "</LastUpdateDate>";
        //        strxmlparam += "<QBAccountID>" + _CreateInventoryParam.QBAccountID + "</QBAccountID>";
        //        strxmlparam += "<Available>" + _CreateInventoryParam.Available + "</Available>";
        //        strxmlparam += "<IssuedOpenJobs>" + _CreateInventoryParam.IssuedOpenJobs + "</IssuedOpenJobs>";
        //        strxmlparam += "<Description2>" + _CreateInventoryParam.Description2 + "</Description2>";
        //        strxmlparam += "<Description3>" + _CreateInventoryParam.Description3 + "</Description3>";
        //        strxmlparam += "<Description4>" + _CreateInventoryParam.Description4 + "</Description4>";
        //        if (_CreateInventoryParam.DateCreated != null & _CreateInventoryParam.DateCreated != DateTime.MinValue)
        //        {
        //            strxmlparam += "<DateCreated>" + _CreateInventoryParam.DateCreated + "</DateCreated>";
        //        }
        //        else
        //        {
        //            strxmlparam += "<DateCreated></DateCreated>";
        //        }

        //        strxmlparam += "<Specification>" + _CreateInventoryParam.Specification + "</Specification>";
        //        strxmlparam += "<Specification2>" + _CreateInventoryParam.Specification2 + "</Specification2>";
        //        strxmlparam += "<Specification3>" + _CreateInventoryParam.Specification2 + "</Specification3>";
        //        strxmlparam += "<Specification4>" + _CreateInventoryParam.Specification4 + "</Specification4>";
        //        strxmlparam += "<Revision>" + _CreateInventoryParam.Revision + "</Revision>";
        //        if (_CreateInventoryParam.LastRevisionDate != null & _CreateInventoryParam.LastRevisionDate != DateTime.MinValue)
        //        {
        //            strxmlparam += "<LastRevisionDate>" + _CreateInventoryParam.LastRevisionDate + "</LastRevisionDate>";
        //        }
        //        else
        //        {
        //            strxmlparam += "<LastRevisionDate></LastRevisionDate>";
        //        }
        //        strxmlparam += "<Eco>" + _CreateInventoryParam.Eco + "</Eco>";
        //        strxmlparam += "<Drawing>" + _CreateInventoryParam.Drawing + "</Drawing>";
        //        strxmlparam += "<Reference>" + _CreateInventoryParam.Reference + "</Reference>";
        //        strxmlparam += "<Length>" + _CreateInventoryParam.Length + "</Length>";
        //        strxmlparam += "<Width>" + _CreateInventoryParam.Width + "</Width>";
        //        strxmlparam += "<Weight>" + _CreateInventoryParam.Weight + "</Weight>";
        //        strxmlparam += "<InspectionRequired>" + _CreateInventoryParam.InspectionRequired + "</InspectionRequired>";
        //        strxmlparam += "<CoCRequired>" + _CreateInventoryParam.CoCRequired + "</CoCRequired>";
        //        strxmlparam += "<ShelfLife>" + _CreateInventoryParam.ShelfLife + "</ShelfLife>";
        //        strxmlparam += "<SerializationRequired>" + _CreateInventoryParam.SerializationRequired + "</SerializationRequired>";
        //        strxmlparam += "<GLcogs>" + _CreateInventoryParam.GLcogs + "</GLcogs>";
        //        strxmlparam += "<GLPurchases>" + _CreateInventoryParam.GLPurchases + "</GLPurchases>";
        //        strxmlparam += "<ABCClass>" + _CreateInventoryParam.ABCClass + "</ABCClass>";
        //        strxmlparam += "<OHValue>" + _CreateInventoryParam.OHValue + "</OHValue>";
        //        strxmlparam += "<OOValue>" + _CreateInventoryParam.OOValue + "</OOValue>";
        //        strxmlparam += "<OverIssueAllowance>" + _CreateInventoryParam.OverIssueAllowance + "</OverIssueAllowance>";
        //        strxmlparam += "<UnderIssueAllowance>" + _CreateInventoryParam.UnderIssueAllowance + "</UnderIssueAllowance>";
        //        strxmlparam += "<InventoryTurns>" + _CreateInventoryParam.InventoryTurns + "</InventoryTurns>";
        //        strxmlparam += "<MOQ>" + _CreateInventoryParam.MOQ + "</MOQ>";
        //        strxmlparam += "<MinInvQty>" + _CreateInventoryParam.MinInvQty + "</MinInvQty>";
        //        strxmlparam += "<MaxInvQty>" + _CreateInventoryParam.MaxInvQty + "</MaxInvQty>";
        //        strxmlparam += "<Commodity>" + _CreateInventoryParam.Commodity + "</Commodity>";

        //        if (_CreateInventoryParam.LastReceiptDate != null & _CreateInventoryParam.LastReceiptDate != DateTime.MinValue)
        //        {
        //            strxmlparam += "<LastReceiptDate>" + _CreateInventoryParam.LastReceiptDate + "</LastReceiptDate>";
        //        }
        //        else
        //        {
        //            strxmlparam += "<LastReceiptDate></LastReceiptDate>";
        //        }


        //        strxmlparam += "<EAU>" + _CreateInventoryParam.EAU + "</EAU>";
        //        if (_CreateInventoryParam.EOLDate != null & _CreateInventoryParam.EOLDate != DateTime.MinValue)
        //        {
        //            strxmlparam += "<EOLDate>" + _CreateInventoryParam.EOLDate + "</EOLDate>";
        //        }
        //        else
        //        {
        //            strxmlparam += "<EOLDate></EOLDate>";
        //        }

        //        strxmlparam += "<WarrantyPeriod>" + _CreateInventoryParam.WarrantyPeriod + "</WarrantyPeriod>";

        //        if (_CreateInventoryParam.PODueDate != null & _CreateInventoryParam.PODueDate != DateTime.MinValue)
        //        {
        //            strxmlparam += "<PODueDate>" + _CreateInventoryParam.PODueDate + "</PODueDate>";
        //        }
        //        else
        //        {
        //            strxmlparam += "<PODueDate></PODueDate>";
        //        }

        //        strxmlparam += "<DefaultReceivingLocation>" + _CreateInventoryParam.DefaultReceivingLocation + "</DefaultReceivingLocation>";
        //        strxmlparam += "<DefaultInspectionLocation>" + _CreateInventoryParam.DefaultInspectionLocation + "</DefaultInspectionLocation>";
        //        strxmlparam += "<LastSalePrice>" + _CreateInventoryParam.LastSalePrice + "</LastSalePrice>";
        //        strxmlparam += "<AnnualSalesQty>" + _CreateInventoryParam.AnnualSalesQty + "</AnnualSalesQty>";
        //        strxmlparam += "<AnnualSalesAmt>" + _CreateInventoryParam.AnnualSalesAmt + "</AnnualSalesAmt>";
        //        strxmlparam += "<QtyAllocatedToSO>" + _CreateInventoryParam.QtyAllocatedToSO + "</QtyAllocatedToSO>";
        //        strxmlparam += "<MaxDiscountPercentage>" + _CreateInventoryParam.MaxDiscountPercentage + "</MaxDiscountPercentage>";
        //        // strxmlparam += "<ITypeCategory>" + _CreateInventoryParam.Type + "</ITypeCategory>";
        //        strxmlparam += "<Height>" + _CreateInventoryParam.Height + "</Height>";
        //        strxmlparam += "<UnitCost>" + _CreateInventoryParam.UnitCost + "</UnitCost>";
        //        strxmlparam += "<GLSales>" + _CreateInventoryParam.GLSales + "</GLSales>";
        //        strxmlparam += "<EOQ>" + _CreateInventoryParam.EOQ + "</EOQ>";
        //        strxmlparam += "<LeadTime>" + _CreateInventoryParam.LeadTime + "</LeadTime>";
        //        #endregion
        //        strxmlparam += "</Item>";
        //        strxmlparam += "</Inventory>";

        //        InventoryViewModel _inventory = _objDLInventory.CreateInventory(strxmlparam, _CreateInventoryParam, ConnectionString);

        //        DataSet ds = _inventory.

        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            _lstInventoryViewModel.Add(
        //                new InventoryViewModel()
        //                {
        //                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
        //                    fDesc = Convert.ToString(dr["fDesc"]),
        //                    Name = Convert.ToString(dr["Name"]),
        //                    Part = Convert.ToString(dr["Part"]),
        //                    Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
        //                    SAcct = Convert.ToInt32(DBNull.Value.Equals(dr["SAcct"]) ? 0 : dr["SAcct"]),
        //                    Measure = Convert.ToString(dr["Measure"]),
        //                    Tax = Convert.ToInt32(DBNull.Value.Equals(dr["Tax"]) ? 0 : dr["Tax"]),
        //                    Balance = Convert.ToDecimal(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
        //                    Price1 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
        //                    Price2 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price2"]) ? 0 : dr["Price2"]),
        //                    Price3 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price3"]) ? 0 : dr["Price3"]),
        //                    Price4 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price4"]) ? 0 : dr["Price4"]),
        //                    Price5 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price5"]) ? 0 : dr["Price5"]),
        //                    Remarks = Convert.ToString(dr["Remarks"]),
        //                    Cat = Convert.ToInt32(DBNull.Value.Equals(dr["Cat"]) ? 0 : dr["Cat"]),
        //                    LVendor = Convert.ToInt32(DBNull.Value.Equals(dr["LVendor"]) ? 0 : dr["LVendor"]),
        //                    LCost = Convert.ToDecimal(DBNull.Value.Equals(dr["LCost"]) ? 0 : dr["LCost"]),
        //                    AllowZero = Convert.ToInt32(DBNull.Value.Equals(dr["AllowZero"]) ? 0 : dr["AllowZero"]),
        //                    Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
        //                    InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
        //                    EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
        //                    UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"]) ? 0 : dr["UserID"]),

        //                    Hand = Convert.ToDecimal(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
        //                    Aisle = Convert.ToString(dr["Aisle"]),
        //                    fOrder = Convert.ToDecimal(DBNull.Value.Equals(dr["fOrder"]) ? 0 : dr["fOrder"]),
        //                    Min = Convert.ToDecimal(DBNull.Value.Equals(dr["Min"]) ? 0 : dr["Min"]),
        //                    Shelf = Convert.ToString(dr["Shelf"]),
        //                    Bin = Convert.ToString(dr["Bin"]),
        //                    Requ = Convert.ToDecimal(DBNull.Value.Equals(dr["Requ"]) ? 0 : dr["Requ"]),
        //                    Warehouse = Convert.ToString(dr["Warehouse"]),
        //                    Price6 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price6"]) ? 0 : dr["Price6"]),
        //                    Committed = Convert.ToDecimal(DBNull.Value.Equals(dr["Committed"]) ? 0 : dr["Committed"]),
        //                    QBInvID = Convert.ToString(dr["QBInvID"]),
        //                    LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
        //                    QBAccountID = Convert.ToString(dr["QBAccountID"]),
        //                    Available = Convert.ToDecimal(DBNull.Value.Equals(dr["Available"]) ? 0 : dr["Available"]),
        //                    IssuedOpenJobs = Convert.ToDecimal(DBNull.Value.Equals(dr["IssuedOpenJobs"]) ? 0 : dr["IssuedOpenJobs"]),
        //                    Description2 = Convert.ToString(dr["Description2"]),
        //                    Description3 = Convert.ToString(dr["Description3"]),
        //                    Description4 = Convert.ToString(dr["Description4"]),
        //                    DateCreated = Convert.ToDateTime(DBNull.Value.Equals(dr["DateCreated"]) ? null : dr["DateCreated"]),
        //                    Class = Convert.ToString(dr["Description4"]),
        //                    Specification = Convert.ToString(dr["Specification"]),
        //                    Specification2 = Convert.ToString(dr["Specification2"]),
        //                    Specification3 = Convert.ToString(dr["Specification3"]),
        //                    Specification4 = Convert.ToString(dr["Specification4"]),
        //                    Revision = Convert.ToString(dr["Revision"]),
        //                    LastRevisionDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastRevisionDate"]) ? null : dr["LastRevisionDate"]),

        //                    Eco = Convert.ToString(dr["Eco"]),
        //                    Drawing = Convert.ToString(dr["Drawing"]),
        //                    Reference = Convert.ToString(dr["Reference"]),
        //                    Length = Convert.ToString(dr["Length"]),
        //                    Width = Convert.ToString(dr["Width"]),
        //                    Weight = Convert.ToString(dr["Weight"]),
        //                    Height = Convert.ToString(dr["Height"]),
        //                    InspectionRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["InspectionRequired"]) ? 0 : dr["InspectionRequired"]),
        //                    CoCRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["CoCRequired"]) ? 0 : dr["CoCRequired"]),
        //                    ShelfLife = Convert.ToDecimal(DBNull.Value.Equals(dr["ShelfLife"]) ? 0 : dr["ShelfLife"]),
        //                    SerializationRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["SerializationRequired"]) ? 0 : dr["SerializationRequired"]),
        //                    GLSales = Convert.ToString(dr["GLSales"]),
        //                    GLcogs = Convert.ToString(dr["GLcogs"]),
        //                    GLPurchases = Convert.ToString(dr["GLPurchases"]),
        //                    ABCClass = Convert.ToString(dr["ABCClass"]),
        //                    OHValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OHValue"]) ? 0 : dr["OHValue"]),
        //                    OOValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OOValue"]) ? 0 : dr["OOValue"]),
        //                    OverIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["OverIssueAllowance"]) ? 0 : dr["OverIssueAllowance"]),
        //                    UnderIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["UnderIssueAllowance"]) ? 0 : dr["UnderIssueAllowance"]),
        //                    InventoryTurns = Convert.ToDecimal(DBNull.Value.Equals(dr["InventoryTurns"]) ? 0 : dr["InventoryTurns"]),
        //                    MOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["MOQ"]) ? 0 : dr["MOQ"]),

        //                    EOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["EOQ"]) ? 0 : dr["EOQ"]),
        //                    MinInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MinInvQty"]) ? 0 : dr["MinInvQty"]),
        //                    MaxInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxInvQty"]) ? 0 : dr["MaxInvQty"]),
        //                    Commodity = Convert.ToString(dr["Commodity"]),
        //                    LastReceiptDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReceiptDate"]) ? null : dr["LastReceiptDate"]),
        //                    MPN = Convert.ToString(dr["MPN"]),
        //                    ApprovedManufacturer = Convert.ToString(dr["ApprovedManufacturer"]),
        //                    ApprovedVendor = Convert.ToString(dr["ApprovedVendor"]),
        //                    EAU = Convert.ToDecimal(DBNull.Value.Equals(dr["EAU"]) ? 0 : dr["EAU"]),
        //                    EOLDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EOLDate"]) ? null : dr["EOLDate"]),
        //                    WarrantyPeriod = Convert.ToInt32(DBNull.Value.Equals(dr["WarrantyPeriod"]) ? 0 : dr["WarrantyPeriod"]),
        //                    PODueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PODueDate"]) ? null : dr["PODueDate"]),
        //                    DefaultReceivingLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultReceivingLocation"]) ? 0 : dr["DefaultReceivingLocation"]),
        //                    DefaultInspectionLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultInspectionLocation"]) ? 0 : dr["DefaultInspectionLocation"]),
        //                    LastSalePrice = Convert.ToDecimal(DBNull.Value.Equals(dr["LastSalePrice"]) ? 0 : dr["LastSalePrice"]),
        //                    AnnualSalesQty = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesQty"]) ? 0 : dr["AnnualSalesQty"]),
        //                    AnnualSalesAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesAmt"]) ? 0 : dr["AnnualSalesAmt"]),
        //                    QtyAllocatedToSO = Convert.ToDecimal(DBNull.Value.Equals(dr["QtyAllocatedToSO"]) ? 0 : dr["QtyAllocatedToSO"]),
        //                    MaxDiscountPercentage = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxDiscountPercentage"]) ? 0 : dr["MaxDiscountPercentage"]),
        //                    UnitCost = Convert.ToDecimal(DBNull.Value.Equals(dr["UnitCost"]) ? 0 : dr["UnitCost"]),
        //                    LeadTime = Convert.ToInt32(DBNull.Value.Equals(dr["LeadTime"]) ? 0 : dr["LeadTime"]),
        //                    DateLastPurchase = Convert.ToDateTime(DBNull.Value.Equals(dr["DateLastPurchase"]) ? null : dr["DateLastPurchase"]),
        //                    SearchField = Convert.ToString(dr["SearchField"]),

        //                    SearchValue = Convert.ToString(dr["SearchValue"]),
        //                    //dtDocs =
        //                    Acct = Convert.ToString(dr["Acct"]),
        //                    FromDate = Convert.ToDateTime(DBNull.Value.Equals(dr["FromDate"]) ? null : dr["FromDate"]),
        //                    EndDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EndDate"]) ? null : dr["EndDate"]),

        //                    DisplayName = Convert.ToString(dr["DisplayName"]),
        //                    MappingColumn = Convert.ToString(dr["MappingColumn"]),

        //                }
        //                );
        //        }

        //    }


        //    return _lstInventoryViewModel;

        //}

        public void UpdateInventory(Inventory inv)
        {
            Inventory invdt = null;

            if (inv != null)
            {

                string strxmlparam = string.Empty;
                strxmlparam += "<Inventory>";
                strxmlparam += "<Item>";
                #region Xml Param
                strxmlparam += "<ID>" + inv.ID + "</ID>";
                strxmlparam += "<fDesc>" + inv.fDesc + "</fDesc>";
                strxmlparam += "<Name>" + inv.Name + "</Name>";
                strxmlparam += "<Part>" + inv.Part + "</Part>";
                strxmlparam += "<Status>" + inv.Status + "</Status>";
                strxmlparam += "<SAcct>" + inv.SAcct + "</SAcct>";
                strxmlparam += "<Measure>" + inv.Measure + "</Measure>";
                strxmlparam += "<Tax>" + inv.Tax + "</Tax>";
                strxmlparam += "<Balance>" + inv.Balance + "</Balance>";
                strxmlparam += "<Price1>" + inv.Price1 + "</Price1>";
                strxmlparam += "<Price2>" + inv.Price2 + "</Price2>";
                strxmlparam += "<Price3>" + inv.Price3 + "</Price3>";
                strxmlparam += "<Price4>" + inv.Price4 + "</Price4>";
                strxmlparam += "<Price5>" + inv.Price5 + "</Price5>";
                strxmlparam += "<Remarks>" + inv.Remarks + "</Remarks>";
                strxmlparam += "<Cat>" + inv.Cat + "</Cat>";
                strxmlparam += "<LVendor>" + inv.LVendor + "</LVendor>";
                strxmlparam += "<LCost>" + inv.LCost + "</LCost>";
                strxmlparam += "<AllowZero>" + inv.AllowZero + "</AllowZero>";
                // strxmlparam += "<Type>" + inv.Type + "</Type>";
                strxmlparam += "<InUse>" + inv.InUse + "</InUse>";
                strxmlparam += "<EN>" + inv.EN + "</EN>";
                strxmlparam += "<Hand>" + inv.Hand + "</Hand>";
                strxmlparam += "<Aisle>" + inv.Aisle + "</Aisle>";
                strxmlparam += "<fOrder>" + inv.fOrder + "</fOrder>";
                strxmlparam += "<Min>" + inv.Min + "</Min>";
                strxmlparam += "<Shelf>" + inv.Shelf + "</Shelf>";
                strxmlparam += "<Bin>" + inv.Bin + "</Bin>";
                strxmlparam += "<Requ>" + inv.Requ + "</Requ>";
                strxmlparam += "<Warehouse>" + inv.Warehouse + "</Warehouse>";
                strxmlparam += "<Price6>" + inv.Price6 + "</Price6>";
                strxmlparam += "<Committed>" + inv.Committed + "</Committed>";
                //  strxmlparam += "<QBInvID>" + inv.QBInvID + "</QBInvID>";
                // strxmlparam += "<LastUpdateDate>" + inv.LastUpdateDate + "</LastUpdateDate>";
                //  strxmlparam += "<QBAccountID>" + inv.QBAccountID + "</QBAccountID>";
                strxmlparam += "<Available>" + inv.Available + "</Available>";
                strxmlparam += "<IssuedOpenJobs>" + inv.IssuedOpenJobs + "</IssuedOpenJobs>";
                //strxmlparam += "<Description2>" + inv.Description2 + "</Description2>";
                //strxmlparam += "<Description3>" + inv.Description3 + "</Description3>";
                //strxmlparam += "<Description4>" + inv.Description4 + "</Description4>";
                //if (inv.DateCreated != null & inv.DateCreated != DateTime.MinValue)
                //{
                //    strxmlparam += "<DateCreated>" + inv.DateCreated + "</DateCreated>";
                //}
                //else
                //{
                //    strxmlparam += "<DateCreated></DateCreated>";
                //}

                strxmlparam += "<Specification>" + inv.Specification + "</Specification>";
                //strxmlparam += "<Specification2>" + inv.Specification2 + "</Specification2>";
                //strxmlparam += "<Specification3>" + inv.Specification2 + "</Specification3>";
                //strxmlparam += "<Specification4>" + inv.Specification4 + "</Specification4>";
                strxmlparam += "<Revision>" + inv.Revision + "</Revision>";
                //if (inv.LastRevisionDate != null & inv.LastRevisionDate != DateTime.MinValue)
                //{
                //    strxmlparam += "<LastRevisionDate>" + inv.LastRevisionDate + "</LastRevisionDate>";
                //}
                //else
                //{
                //    strxmlparam += "<LastRevisionDate></LastRevisionDate>";
                //}
                strxmlparam += "<Eco>" + inv.Eco + "</Eco>";
                strxmlparam += "<Drawing>" + inv.Drawing + "</Drawing>";
                //strxmlparam += "<Reference>" + inv.Reference + "</Reference>";
                //strxmlparam += "<Length>" + inv.Length + "</Length>";
                //strxmlparam += "<Width>" + inv.Width + "</Width>";
                //strxmlparam += "<Weight>" + inv.Weight + "</Weight>";
                //strxmlparam += "<InspectionRequired>" + inv.InspectionRequired + "</InspectionRequired>";
                //strxmlparam += "<CoCRequired>" + inv.CoCRequired + "</CoCRequired>";
                strxmlparam += "<ShelfLife>" + inv.ShelfLife + "</ShelfLife>";
                //  strxmlparam += "<SerializationRequired>" + inv.SerializationRequired + "</SerializationRequired>";
                strxmlparam += "<GLcogs>" + inv.GLcogs + "</GLcogs>";
                strxmlparam += "<GLPurchases>" + inv.GLPurchases + "</GLPurchases>";
                strxmlparam += "<ABCClass>" + inv.ABCClass + "</ABCClass>";
                strxmlparam += "<OHValue>" + inv.OHValue + "</OHValue>";
                strxmlparam += "<OOValue>" + inv.OOValue + "</OOValue>";
                strxmlparam += "<OverIssueAllowance>" + inv.OverIssueAllowance + "</OverIssueAllowance>";
                strxmlparam += "<UnderIssueAllowance>" + inv.UnderIssueAllowance + "</UnderIssueAllowance>";
                strxmlparam += "<InventoryTurns>" + inv.InventoryTurns + "</InventoryTurns>";
                strxmlparam += "<MOQ>" + inv.MOQ + "</MOQ>";
                strxmlparam += "<MinInvQty>" + inv.MinInvQty + "</MinInvQty>";
                strxmlparam += "<MaxInvQty>" + inv.MaxInvQty + "</MaxInvQty>";
                strxmlparam += "<Commodity>" + inv.Commodity + "</Commodity>";

                if (inv.LastReceiptDate != null & inv.LastReceiptDate != DateTime.MinValue)
                {
                    strxmlparam += "<LastReceiptDate>" + inv.LastReceiptDate + "</LastReceiptDate>";
                }
                else
                {
                    strxmlparam += "<LastReceiptDate></LastReceiptDate>";
                }


                strxmlparam += "<EAU>" + inv.EAU + "</EAU>";
                if (inv.EOLDate != null & inv.EOLDate != DateTime.MinValue)
                {
                    strxmlparam += "<EOLDate>" + inv.EOLDate + "</EOLDate>";
                }
                else
                {
                    strxmlparam += "<EOLDate></EOLDate>";
                }

                strxmlparam += "<WarrantyPeriod>" + inv.WarrantyPeriod + "</WarrantyPeriod>";

                if (inv.PODueDate != null & inv.PODueDate != DateTime.MinValue)
                {
                    strxmlparam += "<PODueDate>" + inv.PODueDate + "</PODueDate>";
                }
                else
                {
                    strxmlparam += "<PODueDate></PODueDate>";
                }

                strxmlparam += "<DefaultReceivingLocation>" + inv.DefaultReceivingLocation + "</DefaultReceivingLocation>";
                strxmlparam += "<DefaultInspectionLocation>" + inv.DefaultInspectionLocation + "</DefaultInspectionLocation>";
                //strxmlparam += "<LastSalePrice>" + inv.LastSalePrice + "</LastSalePrice>";
                //strxmlparam += "<AnnualSalesQty>" + inv.AnnualSalesQty + "</AnnualSalesQty>";
                //strxmlparam += "<AnnualSalesAmt>" + inv.AnnualSalesAmt + "</AnnualSalesAmt>";
                strxmlparam += "<QtyAllocatedToSO>" + inv.QtyAllocatedToSO + "</QtyAllocatedToSO>";
                //   strxmlparam += "<MaxDiscountPercentage>" + inv.MaxDiscountPercentage + "</MaxDiscountPercentage>";
                // strxmlparam += "<ITypeCategory>" + inv.Type + "</ITypeCategory>";
                //  strxmlparam += "<Height>" + inv.Height + "</Height>";
                strxmlparam += "<UnitCost>" + inv.UnitCost + "</UnitCost>";
                strxmlparam += "<GLSales>" + inv.GLSales + "</GLSales>";
                strxmlparam += "<EOQ>" + inv.EOQ + "</EOQ>";
                strxmlparam += "<LeadTime>" + inv.LeadTime + "</LeadTime>";
                #endregion
                strxmlparam += "</Item>";
                strxmlparam += "</Inventory>";

                _objDLInventory.UpdateInventory(inv);


            }



        }

        //API
        public void UpdateInventory(UpdateInventoryParam _UpdateInventoryParam, string ConnectionString)
        {
            if (_UpdateInventoryParam != null)
            {

                string strxmlparam = string.Empty;
                strxmlparam += "<Inventory>";
                strxmlparam += "<Item>";
                #region Xml Param
                strxmlparam += "<ID>" + _UpdateInventoryParam.ID + "</ID>";
                strxmlparam += "<fDesc>" + _UpdateInventoryParam.fDesc + "</fDesc>";
                strxmlparam += "<Name>" + _UpdateInventoryParam.Name + "</Name>";
                strxmlparam += "<Part>" + _UpdateInventoryParam.Part + "</Part>";
                strxmlparam += "<Status>" + _UpdateInventoryParam.Status + "</Status>";
                strxmlparam += "<SAcct>" + _UpdateInventoryParam.SAcct + "</SAcct>";
                strxmlparam += "<Measure>" + _UpdateInventoryParam.Measure + "</Measure>";
                strxmlparam += "<Tax>" + _UpdateInventoryParam.Tax + "</Tax>";
                strxmlparam += "<Balance>" + _UpdateInventoryParam.Balance + "</Balance>";
                strxmlparam += "<Price1>" + _UpdateInventoryParam.Price1 + "</Price1>";
                strxmlparam += "<Price2>" + _UpdateInventoryParam.Price2 + "</Price2>";
                strxmlparam += "<Price3>" + _UpdateInventoryParam.Price3 + "</Price3>";
                strxmlparam += "<Price4>" + _UpdateInventoryParam.Price4 + "</Price4>";
                strxmlparam += "<Price5>" + _UpdateInventoryParam.Price5 + "</Price5>";
                strxmlparam += "<Remarks>" + _UpdateInventoryParam.Remarks + "</Remarks>";
                strxmlparam += "<Cat>" + _UpdateInventoryParam.Cat + "</Cat>";
                strxmlparam += "<LVendor>" + _UpdateInventoryParam.LVendor + "</LVendor>";
                strxmlparam += "<LCost>" + _UpdateInventoryParam.LCost + "</LCost>";
                strxmlparam += "<AllowZero>" + _UpdateInventoryParam.AllowZero + "</AllowZero>";
                // strxmlparam += "<Type>" + _UpdateInventoryParam.Type + "</Type>";
                strxmlparam += "<InUse>" + _UpdateInventoryParam.InUse + "</InUse>";
                strxmlparam += "<EN>" + _UpdateInventoryParam.EN + "</EN>";
                strxmlparam += "<Hand>" + _UpdateInventoryParam.Hand + "</Hand>";
                strxmlparam += "<Aisle>" + _UpdateInventoryParam.Aisle + "</Aisle>";
                strxmlparam += "<fOrder>" + _UpdateInventoryParam.fOrder + "</fOrder>";
                strxmlparam += "<Min>" + _UpdateInventoryParam.Min + "</Min>";
                strxmlparam += "<Shelf>" + _UpdateInventoryParam.Shelf + "</Shelf>";
                strxmlparam += "<Bin>" + _UpdateInventoryParam.Bin + "</Bin>";
                strxmlparam += "<Requ>" + _UpdateInventoryParam.Requ + "</Requ>";
                strxmlparam += "<Warehouse>" + _UpdateInventoryParam.Warehouse + "</Warehouse>";
                strxmlparam += "<Price6>" + _UpdateInventoryParam.Price6 + "</Price6>";
                strxmlparam += "<Committed>" + _UpdateInventoryParam.Committed + "</Committed>";
                //  strxmlparam += "<QBInvID>" + inv.QBInvID + "</QBInvID>";
                // strxmlparam += "<LastUpdateDate>" + inv.LastUpdateDate + "</LastUpdateDate>";
                //  strxmlparam += "<QBAccountID>" + inv.QBAccountID + "</QBAccountID>";
                strxmlparam += "<Available>" + _UpdateInventoryParam.Available + "</Available>";
                strxmlparam += "<IssuedOpenJobs>" + _UpdateInventoryParam.IssuedOpenJobs + "</IssuedOpenJobs>";
                //strxmlparam += "<Description2>" + inv.Description2 + "</Description2>";
                //strxmlparam += "<Description3>" + inv.Description3 + "</Description3>";
                //strxmlparam += "<Description4>" + inv.Description4 + "</Description4>";
                //if (inv.DateCreated != null & inv.DateCreated != DateTime.MinValue)
                //{
                //    strxmlparam += "<DateCreated>" + inv.DateCreated + "</DateCreated>";
                //}
                //else
                //{
                //    strxmlparam += "<DateCreated></DateCreated>";
                //}

                strxmlparam += "<Specification>" + _UpdateInventoryParam.Specification + "</Specification>";
                //strxmlparam += "<Specification2>" + inv.Specification2 + "</Specification2>";
                //strxmlparam += "<Specification3>" + inv.Specification2 + "</Specification3>";
                //strxmlparam += "<Specification4>" + inv.Specification4 + "</Specification4>";
                strxmlparam += "<Revision>" + _UpdateInventoryParam.Revision + "</Revision>";
                //if (inv.LastRevisionDate != null & inv.LastRevisionDate != DateTime.MinValue)
                //{
                //    strxmlparam += "<LastRevisionDate>" + inv.LastRevisionDate + "</LastRevisionDate>";
                //}
                //else
                //{
                //    strxmlparam += "<LastRevisionDate></LastRevisionDate>";
                //}
                strxmlparam += "<Eco>" + _UpdateInventoryParam.Eco + "</Eco>";
                strxmlparam += "<Drawing>" + _UpdateInventoryParam.Drawing + "</Drawing>";
                //strxmlparam += "<Reference>" + inv.Reference + "</Reference>";
                //strxmlparam += "<Length>" + inv.Length + "</Length>";
                //strxmlparam += "<Width>" + inv.Width + "</Width>";
                //strxmlparam += "<Weight>" + inv.Weight + "</Weight>";
                //strxmlparam += "<InspectionRequired>" + inv.InspectionRequired + "</InspectionRequired>";
                //strxmlparam += "<CoCRequired>" + inv.CoCRequired + "</CoCRequired>";
                strxmlparam += "<ShelfLife>" + _UpdateInventoryParam.ShelfLife + "</ShelfLife>";
                //  strxmlparam += "<SerializationRequired>" + inv.SerializationRequired + "</SerializationRequired>";
                strxmlparam += "<GLcogs>" + _UpdateInventoryParam.GLcogs + "</GLcogs>";
                strxmlparam += "<GLPurchases>" + _UpdateInventoryParam.GLPurchases + "</GLPurchases>";
                strxmlparam += "<ABCClass>" + _UpdateInventoryParam.ABCClass + "</ABCClass>";
                strxmlparam += "<OHValue>" + _UpdateInventoryParam.OHValue + "</OHValue>";
                strxmlparam += "<OOValue>" + _UpdateInventoryParam.OOValue + "</OOValue>";
                strxmlparam += "<OverIssueAllowance>" + _UpdateInventoryParam.OverIssueAllowance + "</OverIssueAllowance>";
                strxmlparam += "<UnderIssueAllowance>" + _UpdateInventoryParam.UnderIssueAllowance + "</UnderIssueAllowance>";
                strxmlparam += "<InventoryTurns>" + _UpdateInventoryParam.InventoryTurns + "</InventoryTurns>";
                strxmlparam += "<MOQ>" + _UpdateInventoryParam.MOQ + "</MOQ>";
                strxmlparam += "<MinInvQty>" + _UpdateInventoryParam.MinInvQty + "</MinInvQty>";
                strxmlparam += "<MaxInvQty>" + _UpdateInventoryParam.MaxInvQty + "</MaxInvQty>";
                strxmlparam += "<Commodity>" + _UpdateInventoryParam.Commodity + "</Commodity>";

                if (_UpdateInventoryParam.LastReceiptDate != null & _UpdateInventoryParam.LastReceiptDate != DateTime.MinValue)
                {
                    strxmlparam += "<LastReceiptDate>" + _UpdateInventoryParam.LastReceiptDate + "</LastReceiptDate>";
                }
                else
                {
                    strxmlparam += "<LastReceiptDate></LastReceiptDate>";
                }


                strxmlparam += "<EAU>" + _UpdateInventoryParam.EAU + "</EAU>";
                if (_UpdateInventoryParam.EOLDate != null & _UpdateInventoryParam.EOLDate != DateTime.MinValue)
                {
                    strxmlparam += "<EOLDate>" + _UpdateInventoryParam.EOLDate + "</EOLDate>";
                }
                else
                {
                    strxmlparam += "<EOLDate></EOLDate>";
                }

                strxmlparam += "<WarrantyPeriod>" + _UpdateInventoryParam.WarrantyPeriod + "</WarrantyPeriod>";

                if (_UpdateInventoryParam.PODueDate != null & _UpdateInventoryParam.PODueDate != DateTime.MinValue)
                {
                    strxmlparam += "<PODueDate>" + _UpdateInventoryParam.PODueDate + "</PODueDate>";
                }
                else
                {
                    strxmlparam += "<PODueDate></PODueDate>";
                }

                strxmlparam += "<DefaultReceivingLocation>" + _UpdateInventoryParam.DefaultReceivingLocation + "</DefaultReceivingLocation>";
                strxmlparam += "<DefaultInspectionLocation>" + _UpdateInventoryParam.DefaultInspectionLocation + "</DefaultInspectionLocation>";
                //strxmlparam += "<LastSalePrice>" + inv.LastSalePrice + "</LastSalePrice>";
                //strxmlparam += "<AnnualSalesQty>" + inv.AnnualSalesQty + "</AnnualSalesQty>";
                //strxmlparam += "<AnnualSalesAmt>" + inv.AnnualSalesAmt + "</AnnualSalesAmt>";
                strxmlparam += "<QtyAllocatedToSO>" + _UpdateInventoryParam.QtyAllocatedToSO + "</QtyAllocatedToSO>";
                //   strxmlparam += "<MaxDiscountPercentage>" + inv.MaxDiscountPercentage + "</MaxDiscountPercentage>";
                // strxmlparam += "<ITypeCategory>" + inv.Type + "</ITypeCategory>";
                //  strxmlparam += "<Height>" + inv.Height + "</Height>";
                strxmlparam += "<UnitCost>" + _UpdateInventoryParam.UnitCost + "</UnitCost>";
                strxmlparam += "<GLSales>" + _UpdateInventoryParam.GLSales + "</GLSales>";
                strxmlparam += "<EOQ>" + _UpdateInventoryParam.EOQ + "</EOQ>";
                strxmlparam += "<LeadTime>" + _UpdateInventoryParam.LeadTime + "</LeadTime>";
                #endregion
                strxmlparam += "</Item>";
                strxmlparam += "</Inventory>";

                _objDLInventory.UpdateInventory(_UpdateInventoryParam, ConnectionString);
            }
        }

        public void CreateInventoryParts(InvParts objInvParts)
        {
            _objDLInventory.CreateInventoryParts(objInvParts);
        }

        //API
        public void CreateInventoryParts(CreateInventoryPartsParam _CreateInventoryPartsParam, string ConnectionString)
        {
            _objDLInventory.CreateInventoryParts(_CreateInventoryPartsParam, ConnectionString);
        }
        public void CreateInvMergeWarehouse(InvWarehouse objInvWarehouse)
        {
            _objDLInventory.CreateInvMergeWarehouse(objInvWarehouse);
        }

        //API
        public void CreateInvMergeWarehouse(CreateInvMergeWarehouseParam _CreateInvMergeWarehouseParam, string ConnectionString)
        {
            _objDLInventory.CreateInvMergeWarehouse(_CreateInvMergeWarehouseParam, ConnectionString);
        }

        public void DeleteInvMergeWarehouse(int invID, string WareHouseID)
        {
            _objDLInventory.DeleteInvMergeWarehouse(invID, WareHouseID);
        }
        public void DeleteInventoryParts(int invpartID)
        {
            _objDLInventory.DeleteInventoryParts(invpartID);
        }

        //API
        public void DeleteInventoryParts(DeleteInventoryPartsParam _DeleteInventoryPartsParam, string ConnectionString)
        {
            _objDLInventory.DeleteInventoryParts(_DeleteInventoryPartsParam, ConnectionString);
        }


        public DataSet GetInventoryWarehouse(User objProp_User)
        {

            DataSet ds = _objUser.GetInventoryWarehouse(objProp_User);

            return ds;
        }

        public DataSet GetInventoryActiveWarehouse(User objProp_User)
        {

            DataSet ds = _objUser.GetInventoryActiveWarehouse(objProp_User);

            return ds;
        }

        //API
        public List<GetInventoryActiveWarehouseViewModel> GetInventoryActiveWarehouse(GetInventoryActiveWarehouseParam _GetInventoryActiveWarehouseParam, string ConnectionString)
        {

            DataSet ds = _objUser.GetInventoryActiveWarehouse(_GetInventoryActiveWarehouseParam, ConnectionString);

            List<GetInventoryActiveWarehouseViewModel> _lstGetInventoryActiveWarehouse = new List<GetInventoryActiveWarehouseViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInventoryActiveWarehouse.Add(
                    new GetInventoryActiveWarehouseViewModel()
                    {
                        ID = Convert.ToString(dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Company = Convert.ToString(dr["Company"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        En = Convert.ToInt32(DBNull.Value.Equals(dr["En"]) ? 0 : dr["En"]),
                        Location = Convert.ToInt32(DBNull.Value.Equals(dr["Location"]) ? 0 : dr["Location"]),
                        Multi = Convert.ToBoolean(DBNull.Value.Equals(dr["Multi"]) ? false : dr["Multi"]),
                        status = Convert.ToInt32(DBNull.Value.Equals(dr["status"]) ? 0 : dr["status"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        TypeName = Convert.ToString(dr["TypeName"]),
                    }
                    );

            }
            return _lstGetInventoryActiveWarehouse;

        }




        public DataSet GetDeaultWarehouse()
        {
            DataSet ds = _objDLInventory.GetDeaultWarehouse();
            return ds;
        }

        public Dictionary<string, string> GetAllVendor(string constr)
        {
            Vendor objVendor = new Vendor();
            objVendor.ConnConfig = constr;

            GetAllVendorParam _GetAllVendor = new GetAllVendorParam();
            _GetAllVendor.ConnConfig = constr;
            DataSet ds = new DataSet();

            //List<VendorViewModel> _lstVendor = new List<VendorViewModel>();

            //string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            //if (IsAPIIntegrationEnable == "YES")
            //{
            //    string APINAME = "InventoryAPI/InventoryList_GetStockReports";

            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllVendor);

            //    JavaScriptSerializer serializer = new JavaScriptSerializer();

            //    serializer.MaxJsonLength = Int32.MaxValue;

            //    _lstVendor = serializer.Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            //    ds = CommonMethods.ToDataSet<VendorViewModel>(_lstVendor);
            //}
            //else
            //{
                ds = _objVendor.GetAllVendor(objVendor);
            //}

            Dictionary<string, string> vendordetails = new Dictionary<string, string>();

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        vendordetails.Add(Convert.ToString(ds.Tables[0].Rows[i]["ID"]), Convert.ToString(ds.Tables[0].Rows[i]["Acct"]));
                    }

                }
            }

            return vendordetails;
        }



      
        public void CreateItemRevision(InvItemRev objItemRev)
        {
            _objDLInventory.CreateItemRevision(objItemRev);
        }

        //API
        public void CreateItemRevision(CreateItemRevisionParam _CreateItemRevisionParam, string ConnectionString)
        {
            _objDLInventory.CreateItemRevision(_CreateItemRevisionParam, ConnectionString);
        }

        public DataSet GetItemQuantity()
        {
            return _objDLInventory.GetItemQuantity();
        }

        public DataSet GetItemPurchaseOrder(int invId)
        {
            return _objDLInventory.GetItemPurchaseOrder(invId);
        }

        public DataSet GetItemPurchaseOrderByInvID(int invId)
        {
            return _objDLInventory.GetItemPurchaseOrderByInvID(invId);
        }

        //API
        public List<GetItemPurchaseOrderByInvIDViewModel> GetItemPurchaseOrderByInvID(GetItemPurchaseOrderByInvIDParam _GetItemPurchaseOrderByInvIDParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.GetItemPurchaseOrderByInvID(_GetItemPurchaseOrderByInvIDParam, ConnectionString);

            List<GetItemPurchaseOrderByInvIDViewModel> _lstItemPurchaseOrder = new List<GetItemPurchaseOrderByInvIDViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstItemPurchaseOrder.Add(
                    new GetItemPurchaseOrderByInvIDViewModel()
                    {
                        LastPurchaseDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastPurchaseDate"]) ? null : dr["LastPurchaseDate"]),
                        LastPurchasePrice = Convert.ToDouble(DBNull.Value.Equals(dr["LastPurchasePrice"]) ? 0.00 : dr["LastPurchasePrice"]),
                        LastReceiptDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReceiptDate"]) ? null : dr["LastReceiptDate"]),
                        NextPODate = Convert.ToDateTime(DBNull.Value.Equals(dr["NextPODate"]) ? null : dr["NextPODate"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                    }
                    );
            }
            return _lstItemPurchaseOrder;
        }



        public DataSet GetAllItemQuantityByInvID(int invId, int userID, int EN)
        {
            return _objDLInventory.GetAllItemQuantityByInvID(invId, userID, EN);
        }

        //API
        public List<GetAllItemQuantityByInvIDViewModel> GetAllItemQuantityByInvID(GetAllItemQuantityByInvIDParam _GetAllItemQuantityByInvIDParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.GetAllItemQuantityByInvID(_GetAllItemQuantityByInvIDParam, ConnectionString);

            List<GetAllItemQuantityByInvIDViewModel> _lstAllItemQuantity = new List<GetAllItemQuantityByInvIDViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstAllItemQuantity.Add(
                    new GetAllItemQuantityByInvIDViewModel()
                    {
                        Hand = Convert.ToDouble(DBNull.Value.Equals(dr["Hand"]) ? 0.00 : dr["Hand"]),
                        Available = Convert.ToDouble(DBNull.Value.Equals(dr["Available"]) ? 0.00 : dr["Available"]),
                        Committed = Convert.ToDouble(DBNull.Value.Equals(dr["Committed"]) ? 0.00 : dr["Committed"]),
                        CommittedValue = Convert.ToDouble(DBNull.Value.Equals(dr["CommittedValue"]) ? 0.00 : dr["CommittedValue"]),
                        fOrder = Convert.ToDouble(DBNull.Value.Equals(dr["fOrder"]) ? 0.00 : dr["fOrder"]),
                        IssuesToOpenJobs = Convert.ToDouble(DBNull.Value.Equals(dr["IssuesToOpenJobs"]) ? 0.00 : dr["IssuesToOpenJobs"]),
                        OHValue = Convert.ToDouble(DBNull.Value.Equals(dr["OHValue"]) ? 0.00 : dr["OHValue"]),
                        OOValue = Convert.ToDouble(DBNull.Value.Equals(dr["OOValue"]) ? 0.00 : dr["OOValue"]),
                        UnitCost = Convert.ToDouble(DBNull.Value.Equals(dr["UnitCost"]) ? 0.00 : dr["UnitCost"]),
                    }
                    );
            }
            return _lstAllItemQuantity;
        }

        public DataSet GetInvManufacturerInfoByInvAndVendorId(int InventoryId, int ApprovedVendorId)
        {
            List<InventoryManufacturerInformation> lstinvManufactInfo = new List<InventoryManufacturerInformation>();

            InventoryManufacturerInformation invManufactInfo = new InventoryManufacturerInformation();

            DataSet dsinv = _objDLInventory.GetInvManufacturerInfoByInvAndVendorId(InventoryId, ApprovedVendorId);


            //if (dsinv != null)
            //{
            //    if (dsinv.Tables.Count > 0)
            //    {
            //        if (dsinv.Tables[0].Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dsinv.Tables[0].Rows.Count; i++)
            //            {
            //                invManufactInfo = new InventoryManufacturerInformation();
            //                invManufactInfo.ID = dsinv.Tables[0].Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(dsinv.Tables[0].Rows[i]["ID"]) : 0;
            //                invManufactInfo.InventoryID = dsinv.Tables[0].Rows[i]["InventoryManufacturerInformation_InvID"] != DBNull.Value ? Convert.ToInt32(dsinv.Tables[0].Rows[i]["InventoryManufacturerInformation_InvID"]) : 0;
            //                invManufactInfo.MPN = dsinv.Tables[0].Rows[i]["MPN"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["MPN"]) : string.Empty;
            //                invManufactInfo.ApprovedManufacturer = dsinv.Tables[0].Rows[i]["ApprovedManufacturer"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["ApprovedManufacturer"]) : string.Empty;
            //                invManufactInfo.ApprovedVendorId = dsinv.Tables[0].Rows[i]["ApprovedVendor"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["ApprovedVendor"]) : "0";
            //                invManufactInfo.ApprovedVendorEmail = dsinv.Tables[0].Rows[i]["Email"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["Email"]) : string.Empty;
            //                lstinvManufactInfo.Add(invManufactInfo);
            //            }
            //        }
            //    }
            //}

            return dsinv;
        }

        //API
        public List<InventoryViewModel> GetInvManufacturerInfoByInvAndVendorId(GetInvManufacturerInfoByInvAndVendorIdParam _GetInvManufacturerInfoByInvAndVendorIdParam, string ConnectionString)
        {
            List<InventoryManufacturerInformation> lstinvManufactInfo = new List<InventoryManufacturerInformation>();

            InventoryManufacturerInformation invManufactInfo = new InventoryManufacturerInformation();

            DataSet dsinv = _objDLInventory.GetInvManufacturerInfoByInvAndVendorId(_GetInvManufacturerInfoByInvAndVendorIdParam, ConnectionString);

            List<InventoryViewModel> _lstInventoryViewModel = new List<InventoryViewModel>();

            foreach (DataRow dr in dsinv.Tables[0].Rows)
            {
                _lstInventoryViewModel.Add(
                    new InventoryViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Name = Convert.ToString(dr["Name"]),
                        Part = Convert.ToString(dr["Part"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        SAcct = Convert.ToInt32(DBNull.Value.Equals(dr["SAcct"]) ? 0 : dr["SAcct"]),
                        Measure = Convert.ToString(dr["Measure"]),
                        Tax = Convert.ToInt32(DBNull.Value.Equals(dr["Tax"]) ? 0 : dr["Tax"]),
                        Balance = Convert.ToDecimal(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Price1 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        Price2 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price2"]) ? 0 : dr["Price2"]),
                        Price3 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price3"]) ? 0 : dr["Price3"]),
                        Price4 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price4"]) ? 0 : dr["Price4"]),
                        Price5 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price5"]) ? 0 : dr["Price5"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Cat = Convert.ToInt32(DBNull.Value.Equals(dr["Cat"]) ? 0 : dr["Cat"]),
                        LVendor = Convert.ToInt32(DBNull.Value.Equals(dr["LVendor"]) ? 0 : dr["LVendor"]),
                        LCost = Convert.ToDecimal(DBNull.Value.Equals(dr["LCost"]) ? 0 : dr["LCost"]),
                        AllowZero = Convert.ToInt32(DBNull.Value.Equals(dr["AllowZero"]) ? 0 : dr["AllowZero"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        InUse = Convert.ToInt32(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"]) ? 0 : dr["UserID"]),

                        Hand = Convert.ToDecimal(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
                        Aisle = Convert.ToString(dr["Aisle"]),
                        fOrder = Convert.ToDecimal(DBNull.Value.Equals(dr["fOrder"]) ? 0 : dr["fOrder"]),
                        Min = Convert.ToDecimal(DBNull.Value.Equals(dr["Min"]) ? 0 : dr["Min"]),
                        Shelf = Convert.ToString(dr["Shelf"]),
                        Bin = Convert.ToString(dr["Bin"]),
                        Requ = Convert.ToDecimal(DBNull.Value.Equals(dr["Requ"]) ? 0 : dr["Requ"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        Price6 = Convert.ToDecimal(DBNull.Value.Equals(dr["Price6"]) ? 0 : dr["Price6"]),
                        Committed = Convert.ToDecimal(DBNull.Value.Equals(dr["Committed"]) ? 0 : dr["Committed"]),
                        QBInvID = Convert.ToString(dr["QBInvID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        QBAccountID = Convert.ToString(dr["QBAccountID"]),
                        Available = Convert.ToDecimal(DBNull.Value.Equals(dr["Available"]) ? 0 : dr["Available"]),
                        IssuedOpenJobs = Convert.ToDecimal(DBNull.Value.Equals(dr["IssuedOpenJobs"]) ? 0 : dr["IssuedOpenJobs"]),
                        Description2 = Convert.ToString(dr["Description2"]),
                        Description3 = Convert.ToString(dr["Description3"]),
                        Description4 = Convert.ToString(dr["Description4"]),
                        DateCreated = Convert.ToDateTime(DBNull.Value.Equals(dr["DateCreated"]) ? null : dr["DateCreated"]),
                        Class = Convert.ToString(dr["Description4"]),
                        Specification = Convert.ToString(dr["Specification"]),
                        Specification2 = Convert.ToString(dr["Specification2"]),
                        Specification3 = Convert.ToString(dr["Specification3"]),
                        Specification4 = Convert.ToString(dr["Specification4"]),
                        Revision = Convert.ToString(dr["Revision"]),
                        LastRevisionDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastRevisionDate"]) ? null : dr["LastRevisionDate"]),

                        Eco = Convert.ToString(dr["Eco"]),
                        Drawing = Convert.ToString(dr["Drawing"]),
                        Reference = Convert.ToString(dr["Reference"]),
                        Length = Convert.ToString(dr["Length"]),
                        Width = Convert.ToString(dr["Width"]),
                        Weight = Convert.ToString(dr["Weight"]),
                        Height = Convert.ToString(dr["Height"]),
                        InspectionRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["InspectionRequired"]) ? 0 : dr["InspectionRequired"]),
                        CoCRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["CoCRequired"]) ? 0 : dr["CoCRequired"]),
                        ShelfLife = Convert.ToDecimal(DBNull.Value.Equals(dr["ShelfLife"]) ? 0 : dr["ShelfLife"]),
                        SerializationRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["SerializationRequired"]) ? 0 : dr["SerializationRequired"]),
                        GLSales = Convert.ToString(dr["GLSales"]),
                        GLcogs = Convert.ToString(dr["GLcogs"]),
                        GLPurchases = Convert.ToString(dr["GLPurchases"]),
                        ABCClass = Convert.ToString(dr["ABCClass"]),
                        OHValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OHValue"]) ? 0 : dr["OHValue"]),
                        OOValue = Convert.ToDecimal(DBNull.Value.Equals(dr["OOValue"]) ? 0 : dr["OOValue"]),
                        OverIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["OverIssueAllowance"]) ? 0 : dr["OverIssueAllowance"]),
                        UnderIssueAllowance = Convert.ToBoolean(DBNull.Value.Equals(dr["UnderIssueAllowance"]) ? 0 : dr["UnderIssueAllowance"]),
                        InventoryTurns = Convert.ToDecimal(DBNull.Value.Equals(dr["InventoryTurns"]) ? 0 : dr["InventoryTurns"]),
                        MOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["MOQ"]) ? 0 : dr["MOQ"]),

                        EOQ = Convert.ToDecimal(DBNull.Value.Equals(dr["EOQ"]) ? 0 : dr["EOQ"]),
                        MinInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MinInvQty"]) ? 0 : dr["MinInvQty"]),
                        MaxInvQty = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxInvQty"]) ? 0 : dr["MaxInvQty"]),
                        Commodity = Convert.ToString(dr["Commodity"]),
                        LastReceiptDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReceiptDate"]) ? null : dr["LastReceiptDate"]),
                        MPN = Convert.ToString(dr["MPN"]),
                        ApprovedManufacturer = Convert.ToString(dr["ApprovedManufacturer"]),
                        ApprovedVendor = Convert.ToString(dr["ApprovedVendor"]),
                        EAU = Convert.ToDecimal(DBNull.Value.Equals(dr["EAU"]) ? 0 : dr["EAU"]),
                        EOLDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EOLDate"]) ? null : dr["EOLDate"]),
                        WarrantyPeriod = Convert.ToInt32(DBNull.Value.Equals(dr["WarrantyPeriod"]) ? 0 : dr["WarrantyPeriod"]),
                        PODueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PODueDate"]) ? null : dr["PODueDate"]),
                        DefaultReceivingLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultReceivingLocation"]) ? 0 : dr["DefaultReceivingLocation"]),
                        DefaultInspectionLocation = Convert.ToBoolean(DBNull.Value.Equals(dr["DefaultInspectionLocation"]) ? 0 : dr["DefaultInspectionLocation"]),
                        LastSalePrice = Convert.ToDecimal(DBNull.Value.Equals(dr["LastSalePrice"]) ? 0 : dr["LastSalePrice"]),
                        AnnualSalesQty = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesQty"]) ? 0 : dr["AnnualSalesQty"]),
                        AnnualSalesAmt = Convert.ToDecimal(DBNull.Value.Equals(dr["AnnualSalesAmt"]) ? 0 : dr["AnnualSalesAmt"]),
                        QtyAllocatedToSO = Convert.ToDecimal(DBNull.Value.Equals(dr["QtyAllocatedToSO"]) ? 0 : dr["QtyAllocatedToSO"]),
                        MaxDiscountPercentage = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxDiscountPercentage"]) ? 0 : dr["MaxDiscountPercentage"]),
                        UnitCost = Convert.ToDecimal(DBNull.Value.Equals(dr["UnitCost"]) ? 0 : dr["UnitCost"]),
                        LeadTime = Convert.ToInt32(DBNull.Value.Equals(dr["LeadTime"]) ? 0 : dr["LeadTime"]),
                        DateLastPurchase = Convert.ToDateTime(DBNull.Value.Equals(dr["DateLastPurchase"]) ? null : dr["DateLastPurchase"]),
                        SearchField = Convert.ToString(dr["SearchField"]),

                        SearchValue = Convert.ToString(dr["SearchValue"]),
                        //dtDocs =
                        Acct = Convert.ToString(dr["Acct"]),
                        FromDate = Convert.ToDateTime(DBNull.Value.Equals(dr["FromDate"]) ? null : dr["FromDate"]),
                        EndDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EndDate"]) ? null : dr["EndDate"]),

                        DisplayName = Convert.ToString(dr["DisplayName"]),
                        MappingColumn = Convert.ToString(dr["MappingColumn"]),

                    }
                    );
            }
            return _lstInventoryViewModel;
        }

        public Commodity CreateCommodity(Commodity comm)
        {

            return _objDLInventory.CreateCommodity(comm);


        }
        public DataSet ReadCommodityById(Commodity com)
        {
            DataSet comdt = null;
            try
            {
                comdt = _objDLInventory.ReadCommodityById(com);
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
                comdt = _objDLInventory.ReadAllCommodity(com);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }



        public List<CommodityViewModel> ReadAllCommodity(string ConnectionString, ReadAllCommodityParam _ReadAllCommodityParam)
        {
            // DataSet ds = null;
            try
            {
                DataSet ds = _objDLInventory.ReadAllCommodity(ConnectionString, _ReadAllCommodityParam);

                List<CommodityViewModel> _lstCommodityViewModel = new List<CommodityViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstCommodityViewModel.Add(
                        new CommodityViewModel()
                        {


                            //ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                            //Code = Convert.ToString(dr["Code"]),
                            //Desc = Convert.ToString(dr["Desc"]),

                            //IsActive = Convert.ToBoolean(DBNull.Value.Equals(dr["CoCRequired"]) ? 0 : dr["IsActive"]),


                        }
                        );
                }
                return _lstCommodityViewModel;


            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public Commodity UpdateCommodity(Commodity com)
        {
            Commodity comdt = com;
            try
            {
                comdt = _objDLInventory.UpdateCommodity(com);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        //public void UpdateInvoiceTransDetails(Invoices _objInvoices)
        //{
        //    _objDLInvoice.UpdateInvoiceTransDetails(_objInvoices);
        ////}
        //public void DeleteTransInvoiceByRef(Transaction _objTrans)
        //{
        //    _objDLInvoice.DeleteTransInvoiceByRef(_objTrans);
        //}
        //public DataSet GetInvoiceByID(Invoices _objInvoice)
        //{
        //    return _objDLInvoice.GetInvoiceByID(_objInvoice);
        //}

        public DataSet ReadCostTypes(Inventory inv)
        {
            return _objDLInventory.ReadCostTypes(inv);
        }

        public void UseCostingType(Inventory inv)
        {
            _objDLInventory.UseCostingType(inv);
        }
        public DataSet GetAllInventoryAdjustmentByDate(InventoryAdjustment invadjustitem)
        {
            return _objDLInventory.GetAllInventoryAdjustmentByDate(invadjustitem);
        }

        //API
        public List<GetAllInvAdjustmentByDateViewModel> GetAllInventoryAdjustmentByDate(GetAllInventoryAdjustmentByDateParam _GetAllInvAdjustmentByDateParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.GetAllInventoryAdjustmentByDate(_GetAllInvAdjustmentByDateParam, ConnectionString);

            List<GetAllInvAdjustmentByDateViewModel> _lstGetInventoryTrans = new List<GetAllInvAdjustmentByDateViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInventoryTrans.Add(
                    new GetAllInvAdjustmentByDateViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? null : dr["Amount"]),
                        Name = Convert.ToString(dr["Name"]),
                        Itemsfdesc = Convert.ToString(dr["Itemsfdesc"]),
                        WarehouseID = Convert.ToString(dr["WarehouseID"]),
                        Company = Convert.ToString(dr["Company"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? null : dr["EN"]),
                        WHName = Convert.ToString(dr["WHName"]),
                        WHLoc = Convert.ToString(dr["WHName"]),
                    }
                    );

            }
            return _lstGetInventoryTrans;


        }

        public DataSet GetInventoryAdjustmentByID(InventoryAdjustment invadjustitem)
        {
            return _objDLInventory.GetInventoryAdjustmentByID(invadjustitem);
        }

        //API
        public List<GetInventoryAdjustmentByIDViewModel> GetInventoryAdjustmentByID(GetInventoryAdjustmentByIDParam _GetInventoryAdjustmentByIDParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.GetInventoryAdjustmentByID(_GetInventoryAdjustmentByIDParam, ConnectionString);

            List<GetInventoryAdjustmentByIDViewModel> _lstGetInventoryAdjustmentByID = new List<GetInventoryAdjustmentByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInventoryAdjustmentByID.Add(
                    new GetInventoryAdjustmentByIDViewModel()
                    {
                        AdjID = Convert.ToInt32(DBNull.Value.Equals(dr["AdjID"]) ? 0 : dr["AdjID"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? null : dr["Amount"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? null : dr["Balance"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? null : dr["Batch"]),
                        Chart = Convert.ToString(dr["Chart"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        ChartID = Convert.ToInt32(DBNull.Value.Equals(dr["ChartID"]) ? 0 : dr["ChartID"]),
                        Company = Convert.ToString(dr["Company"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? null : dr["EN"]),
                        Hand = Convert.ToDouble(DBNull.Value.Equals(dr["Hand"]) ? null : dr["Hand"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        InvDesc = Convert.ToString(dr["InvDesc"]),
                        InvID = Convert.ToInt32(DBNull.Value.Equals(dr["InvID"]) ? null : dr["InvID"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        LocationID = Convert.ToInt32(DBNull.Value.Equals(dr["LocationID"]) ? null : dr["LocationID"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        Quantity = Convert.ToDouble(DBNull.Value.Equals(dr["Quantity"]) ? null : dr["Quantity"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? null : dr["TransID"]),
                        WarehouseID = Convert.ToString(dr["WarehouseID"]),
                        WarehouseName = Convert.ToString(dr["WarehouseName"]),
                    }
                    );

            }
            return _lstGetInventoryAdjustmentByID;
        }

        public DataSet GetInvItems(Inventory inv, string searchterm)
        {
            return _objDLInventory.GetInvItems(inv, searchterm);
        }

        public int CreateInventoryAdjustments(InventoryAdjustment invadj,bool _chkInvTracking)
        {
            return _objDLInventory.CreateInventoryAdjustments(invadj, _chkInvTracking);
        }

        //API
        public int CreateInventoryAdjustments(CreateInventoryAdjustmentsParam _CreateInventoryAdjustmentsParam, string ConnectionString)
        {
            return _objDLInventory.CreateInventoryAdjustments(_CreateInventoryAdjustmentsParam, ConnectionString);
        }

        public int DeleteAdjustment(InventoryAdjustment invadj)
        {
            return _objDLInventory.DeleteAdjustment(invadj);
        }

        //API
        public int DeleteAdjustment(DeleteAdjustmentParam _DeleteAdjustment, string ConnectionString)
        {
            return _objDLInventory.DeleteAdjustment(_DeleteAdjustment, ConnectionString);
        }

        #region
        public DataSet GetAutoFillWarehouse(InvWarehouse _objInvWarehouse)
        {
            return _objDLInventory.GetAutoFillWarehouse(_objInvWarehouse);
        }

        public DataSet GetAutoFillOnHandBalance(IWarehouseLocAdj _objIWarehouseLocAdj)
        {
            return _objDLInventory.GetAutoFillOnHandBalance(_objIWarehouseLocAdj);
        }

        public List<GetAutoFillOnHandBalanceViewModel> GetAutoFillOnHandBalance(GetAutoFillOnHandBalanceParam _GetAutoFillOnHandBalanceParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.GetAutoFillOnHandBalance(_GetAutoFillOnHandBalanceParam, ConnectionString);
            List<GetAutoFillOnHandBalanceViewModel> _lstGetAutoFillOnHandBalanceViewModel = new List<GetAutoFillOnHandBalanceViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAutoFillOnHandBalanceViewModel.Add(
                    new GetAutoFillOnHandBalanceViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Hand = Convert.ToDecimal(DBNull.Value.Equals(dr["Hand"]) ? 0.00 : dr["Hand"]),
                        Balance = Convert.ToDecimal(DBNull.Value.Equals(dr["Balance"]) ? 0.00 : dr["Balance"]),
                    }
                    );
            }

            return _lstGetAutoFillOnHandBalanceViewModel;
        }
        public void CreateReceivePOInvWarehouse(IWarehouseLocAdj _objIWarehouseLocAdj, Transaction trans)
        {
             _objDLInventory.CreateReceivePOInvWarehouse(_objIWarehouseLocAdj,trans);
        }
        public void CreateReceivePOInvWarehouse(CreateReceivePOInvWarehouseTransParam _CreateReceivePOInvWarehouseTransParam, string ConnectionString)
        {
            _objDLInventory.CreateReceivePOInvWarehouse(_CreateReceivePOInvWarehouseTransParam, ConnectionString);
        }
        public void CreateReceivePOInvWarehouseTrans(Transaction trans)
        {
            _objDLInventory.CreateReceivePOInvWarehouseTrans(trans);
        }

        public void CreateReceivePOInvWarehouseTrans(ReceivePOInvWarehouseTransParam _ReceivePOInvWarehouseTransParam, string ConnectionString)
        {
            _objDLInventory.CreateReceivePOInvWarehouseTrans(_ReceivePOInvWarehouseTransParam, ConnectionString);
        }

        public bool IS_INVENTORY_TRACKING_ISON(string ConnConfig)
        {
          return  _objDLInventory.ISINVENTORYTRACKINGISON(ConnConfig);

        }
        #endregion

        public bool ISINVENTORYTRACKINGISON(string ConnConfig)
        {
            return _objDLInventory.ISINVENTORYTRACKINGISON(ConnConfig);
        }

        public DataSet GetInventoryTrans(Inventory _objInv, List<RetainFilter> filters, bool inclInactive)
        {
            return _objDLInventory.GetInventoryTrans(_objInv, filters, inclInactive);
        }

        //API
        public List<GetInventoryTransViewModel> GetInventoryTrans(GetInventoryTransParam _GetInventoryTransParam, string ConnectionString)
        {
            DataSet ds = _objDLInventory.GetInventoryTrans(_GetInventoryTransParam, ConnectionString);

            List<GetInventoryTransViewModel> _lstGetInventoryTrans = new List<GetInventoryTransViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInventoryTrans.Add(
                    new GetInventoryTransViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        InventoryGL = Convert.ToString(dr["InventoryGL"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? null : dr["Amount"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? null : dr["Balance"]),
                        Charges = Convert.ToDouble(DBNull.Value.Equals(dr["Charges"]) ? null : dr["Charges"]),
                        Credits = Convert.ToDouble(DBNull.Value.Equals(dr["Credits"]) ? null : dr["Credits"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        INVID = Convert.ToInt32(DBNull.Value.Equals(dr["INVID"]) ? 0 : dr["INVID"]),
                        MDesc = Convert.ToString(dr["MDesc"]),
                        Quan = Convert.ToInt32(DBNull.Value.Equals(dr["Quan"]) ? null : dr["Quan"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? null : dr["Ref"]),
                        TType = Convert.ToString(dr["TType"]),
                    }
                    );

            }
            return _lstGetInventoryTrans;

        }
        public void DeleteInventoryCommodity(string Connstr, string CommodityID)
        {
            _objDLInventory.DeleteInventoryCommodity(Connstr, CommodityID);
        }
    }
}
