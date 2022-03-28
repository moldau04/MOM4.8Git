using BusinessEntity;
using BusinessEntity.CustomersModel;
using DataLayer;
using DataLayer.Billing;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer.Billing
{
    public class BL_BillCodes
    {
        DL_User objDL_User = new DL_User();

        public DataSet GetAutoCompleteBillCodes(User objPropUser, string prefixText = "")
        {
            return  new DL_BillCodes().GetAutoCompleteBillCodes(objPropUser, prefixText);
        }

        //API
        public List<GetAutoCompleteBillCodesViewModel> GetAutoCompleteBillCodes(GetAutoCompleteBillCodesParam _GetAutoCompleteBillCodes, string ConnectionString, string prefixText = "")
        {
            DataSet ds = new DL_BillCodes().GetAutoCompleteBillCodes(_GetAutoCompleteBillCodes, ConnectionString, prefixText);

            List<GetAutoCompleteBillCodesViewModel> _lstGetAutoCompleteBillCodes = new List<GetAutoCompleteBillCodesViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAutoCompleteBillCodes.Add(
                    new GetAutoCompleteBillCodesViewModel()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        type = Convert.ToInt16(DBNull.Value.Equals(dr["type"]) ? 0 : dr["type"]),
                        Hand = Convert.ToDouble(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
                        BillType = Convert.ToString(dr["BillType"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Price1 = Convert.ToDouble(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        AStatus = Convert.ToInt16(DBNull.Value.Equals(dr["AStatus"]) ? 0 : dr["AStatus"]),
                    }
                    );
            }

            return _lstGetAutoCompleteBillCodes;
        }

        public DataSet GetAllBillCodes(User objPropUser)        // get all active inactive billing code
        {
            return new DL_BillCodes().GetAllBillCodes(objPropUser);
        }
        public DataSet getBillCodes(User objPropUser)
        {
            return new DL_BillCodes().getBillCodes(objPropUser);
        }

        public DataSet getBillCodesByID(User objPropUser)
        {
            return  new DL_BillCodes().getBillCodesByID(objPropUser);
        }

        public void DeleteBillingCode(User objPropUser)
        {
           new DL_BillCodes().DeleteBillingCode(objPropUser);
        }

        public void DeleteBillingCodebyListID(User objPropUser)
        {
          new  DL_BillCodes().DeleteBillingCodebyListID(objPropUser);
        }
        public DataSet GetActiveBillingCode(string conn)
        {
            return new DL_BillCodes().GetActiveBillingCode(conn);
        }

        //API
        public List<GetActiveBillingCodeViewModel> GetActiveBillingCode(GetActiveBillingCodeParam _GetActiveBillingCode, string ConnectionString)
        {
            DataSet ds = new DL_BillCodes().GetActiveBillingCode(_GetActiveBillingCode, ConnectionString);

            List<GetActiveBillingCodeViewModel> _lstGetActiveBillingCode = new List<GetActiveBillingCodeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetActiveBillingCode.Add(
                    new GetActiveBillingCodeViewModel()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        TYPE = Convert.ToInt16(DBNull.Value.Equals(dr["TYPE"]) ? 0 : dr["TYPE"]),
                        Hand = Convert.ToDouble(DBNull.Value.Equals(dr["Hand"]) ? 0 : dr["Hand"]),
                        BillType = Convert.ToString(dr["BillType"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Price1 = Convert.ToDouble(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        AStatus = Convert.ToInt16(DBNull.Value.Equals(dr["AStatus"]) ? 0 : dr["AStatus"]),
                        SAcct = Convert.ToInt32(DBNull.Value.Equals(dr["SAcct"]) ? 0 : dr["SAcct"]),
                    }
                    );
            }

            return _lstGetActiveBillingCode;
        }
        public void UpdateDefaultBillingCode(string ConnConfig, int DefaultBillingCode, string DefaultBillingCodeDesc) 
        {
            DL_BillCodes _objDL_BillCodes = new DL_BillCodes();
            _objDL_BillCodes.UpdateDefaultBillingCode(ConnConfig, DefaultBillingCode, DefaultBillingCodeDesc); 
        }
    }
}
