using System.Data;
using DataLayer;
using BusinessEntity;
using BusinessEntity.CustomersModel;
using System.Collections.Generic;
using System;

namespace BusinessLayer
{
    public class BL_Alerts
    {
        DL_Alerts objDL_Alerts = new DL_Alerts();
        public DataSet GetAlertType(Alerts objPropAlerts)
        {
            return objDL_Alerts.GetAlertType(objPropAlerts);
        }

        //API
        public List<GetAlertTypeViewModel> GetAlertType(GetAlertTypeParam _GetAlertType, string ConnectionString)
        {
            DataSet ds = objDL_Alerts.GetAlertType(_GetAlertType, ConnectionString);

            List<GetAlertTypeViewModel> _lstGetAlertType = new List<GetAlertTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAlertType.Add(
                    new GetAlertTypeViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        AlertName = Convert.ToString(dr["AlertName"]),
                        Code = Convert.ToString(dr["Code"]),
                    }
                    );
            }
            return _lstGetAlertType;
        }
        public DataSet getAlertContactSearch(Alerts objPropAlerts)
        {
            return objDL_Alerts.getAlertContactSearch(objPropAlerts);
        }

        public DataSet getAlerts(Alerts objPropAlerts)
        {
            return objDL_Alerts.getAlerts(objPropAlerts);
        }

        //API
        public ListGetAlerts getAlerts(GetAlertsParam _GetAlerts, string ConnectionString)
        {
            DataSet ds = objDL_Alerts.getAlerts(_GetAlerts, ConnectionString);

            ListGetAlerts _ds = new ListGetAlerts();
            List<GetAlertsTable1> _lstTable1 = new List<GetAlertsTable1>();
            List<GetAlertsTable2> _lstTable2 = new List<GetAlertsTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetAlertsTable1()
                    {
                        AlertID = Convert.ToInt32(DBNull.Value.Equals(dr["AlertID"]) ? 0 : dr["AlertID"]),
                        ScreenID = Convert.ToInt32(DBNull.Value.Equals(dr["ScreenID"]) ? 0 : dr["ScreenID"]),
                        ScreenName = Convert.ToString(dr["ScreenName"]),
                        AlertCode = Convert.ToString(dr["AlertCode"]),
                        AlertSubject = Convert.ToString(dr["AlertSubject"]),
                        AlertMessage = Convert.ToString(dr["AlertMessage"]),
                    }
                    );
            }
            _ds.lstTable1 = _lstTable1;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetAlertsTable2()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        ScreenID = Convert.ToInt32(DBNull.Value.Equals(dr["ScreenID"]) ? 0 : dr["ScreenID"]),
                        ScreenName = Convert.ToString(dr["ScreenName"]),
                        AlertID = Convert.ToInt32(DBNull.Value.Equals(dr["AlertID"]) ? 0 : dr["AlertID"]),
                        Email = Convert.ToBoolean(DBNull.Value.Equals(dr["Email"]) ? false : dr["Email"]),
                        Text = Convert.ToBoolean(DBNull.Value.Equals(dr["Text"]) ? false : dr["Text"]),
                        AlertCode = Convert.ToString(dr["AlertCode"]),
                        NAME = Convert.ToString(dr["NAME"]),
                    }
                    );
            }
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
    }

}
