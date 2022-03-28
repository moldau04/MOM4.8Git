using BusinessEntity;
using BusinessEntity.CustomersModel;
using DataLayer.SchduleModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessLayer.Schedule
{
   public class BL_Tickets
    {
        /// <summary>
        /// GetTicketListData
        /// </summary>
        /// <param name="objMapData"></param>
        /// <param name="filters"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <param name="strORDERBY"></param>
        /// <param name="RadGvTicketListminimumRows"></param>
        /// <param name="RadGvTicketListmaximumRows"></param>
        /// <returns></returns>
        public DataSet GetTicketListData(MapData objMapData, List<RetainFilter> filters, string fromDate,string toDate,
            Int32 IsSalesAsigned = 0, string strORDERBY = "EDATE ASC", int RadGvTicketListminimumRows = 0, 
            int RadGvTicketListmaximumRows = 50, bool inclCustomField = false)
        {
            return new DL_Tickets().GetTicketListData(objMapData, filters, fromDate,toDate, IsSalesAsigned, strORDERBY, 
                RadGvTicketListminimumRows, RadGvTicketListmaximumRows, inclCustomField);
        }

        public string ExportTicketListDataToExcel(MapData objMapData, List<RetainFilter> filters, string fromDate, string toDate,
            Int32 IsSalesAsigned = 0, string strORDERBY = "EDATE ASC", string fileName = "", int rowsPerSheet = 500, bool inclCustomField = false)
        {
            //return new DL_Tickets().ExportTicketListDataToExcel(objMapData, filters, fromDate, toDate, IsSalesAsigned, strORDERBY);
            return new DL_Tickets().ExportTicketListDataToExcelNew(objMapData, filters, fromDate, toDate, IsSalesAsigned, strORDERBY, fileName, rowsPerSheet, inclCustomField);
        }

        /// <summary>
        /// GetTicketListReportData
        /// </summary>
        /// <param name="objPropMapData"></param>
        /// <param name="filters"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <param name="IsCallForTicketReport"></param>
        /// <param name="strORDERBY"></param>
        /// <returns></returns>
        public DataSet GetTicketListReportData(MapData objPropMapData, List<RetainFilter> filters,string fromDate,string toDate, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0, string strORDERBY = "EDATE ASC", bool GetReportData = false)
        {
            return new DL_Tickets().GetTicketListReportData(  objPropMapData,   filters, fromDate,toDate,  IsSalesAsigned , IsCallForTicketReport ,   strORDERBY ,GetReportData);
        }


        public DataSet GetTicketListReportDatabyQuery(string query, string ConnConfig)
        {
            return new DL_Tickets().GetTicketListReportDatabyQuery(query, ConnConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name = "objMapData" ></ param >
        public int UpdateReviewStatus(MapData objMapData)
        {
           return new DL_Tickets().UpdateReviewStatus(objMapData);
        }
 

        public void VoidedTickets(string ConnConfig, int LocID, string UpdatedBy, int Tickets)
        {
              new DL_Tickets().VoidedTickets( ConnConfig,  LocID,  UpdatedBy,  Tickets);
        }

        public DataSet getCallHistory(MapData objMapData, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0)
        {
            return new DL_Tickets().getCallHistory(objMapData, IsSalesAsigned, IsCallForTicketReport);
        }

        //API
        public List<GetCallHistoryViewModel> getCallHistory(GetCallHistoryParam _GetCallHistory, string ConnectionString, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0)
        {
            DataSet ds = new DL_Tickets().getCallHistory(_GetCallHistory, ConnectionString, IsSalesAsigned, IsCallForTicketReport);

            List<GetCallHistoryViewModel> _lstGetCallHistory = new List<GetCallHistoryViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCallHistory.Add(
                    new GetCallHistoryViewModel()
                    {
                        who = Convert.ToString(dr["who"]),
                        CPhone = Convert.ToString(dr["CPhone"]),
                        lid = Convert.ToInt32(DBNull.Value.Equals(dr["lid"]) ? 0 : dr["lid"]),
                        assigned = Convert.ToInt16(DBNull.Value.Equals(dr["assigned"]) ? 0 : dr["assigned"]),
                        fulladdress = Convert.ToString(dr["fulladdress"]),
                        city = Convert.ToString(dr["city"]),
                        WorkOrder = Convert.ToString(dr["WorkOrder"]),
                        Reg = Convert.ToDouble(DBNull.Value.Equals(dr["Reg"]) ? 0 : dr["Reg"]),
                        OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                        NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                        DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                        TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
                        BT = Convert.ToDouble(DBNull.Value.Equals(dr["BT"]) ? 0 : dr["BT"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        ClearCheck = Convert.ToInt32(DBNull.Value.Equals(dr["ClearCheck"]) ? 0 : dr["ClearCheck"]),
                        charge = Convert.ToInt16(DBNull.Value.Equals(dr["charge"]) ? 0 : dr["charge"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        TimeRoute = Convert.ToDateTime(DBNull.Value.Equals(dr["TimeRoute"]) ? null : dr["TimeRoute"]),
                        TimeSite = Convert.ToDateTime(DBNull.Value.Equals(dr["TimeSite"]) ? null : dr["TimeSite"]),
                        TimeComp = Convert.ToDateTime(DBNull.Value.Equals(dr["TimeComp"]) ? null : dr["TimeComp"]),
                        comp = Convert.ToInt32(DBNull.Value.Equals(dr["comp"]) ? 0 : dr["comp"]),
                        dwork = Convert.ToString(dr["dwork"]),
                        lastname = Convert.ToString(dr["lastname"]),
                        hourlyrate = Convert.ToDouble(DBNull.Value.Equals(dr["hourlyrate"]) ? 0 : dr["hourlyrate"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        customername = Convert.ToString(dr["customername"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        locname = Convert.ToString(dr["locname"]),
                        address = Convert.ToString(dr["address"]),
                        phone = Convert.ToString(dr["phone"]),
                        Cat = Convert.ToString(dr["Cat"]),
                        edate = Convert.ToDateTime(DBNull.Value.Equals(dr["edate"]) ? null : dr["edate"]),
                        CDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CDate"]) ? null : dr["CDate"]),
                        descres = Convert.ToString(dr["descres"]),
                        assignname = Convert.ToString(dr["assignname"]),
                        Est = Convert.ToDouble(DBNull.Value.Equals(dr["Est"]) ? 0 : dr["Est"]),
                        Tottime = Convert.ToDouble(DBNull.Value.Equals(dr["Tottime"]) ? 0 : dr["Tottime"]),
                        timediff = Convert.ToDouble(DBNull.Value.Equals(dr["timediff"]) ? 0 : dr["timediff"]),
                        workorder1 = Convert.ToString(dr["workorder"]),
                        expenses = Convert.ToDouble(DBNull.Value.Equals(dr["expenses"]) ? 0 : dr["expenses"]),
                        zone = Convert.ToDouble(DBNull.Value.Equals(dr["zone"]) ? 0 : dr["zone"]),
                        toll = Convert.ToDouble(DBNull.Value.Equals(dr["toll"]) ? 0 : dr["toll"]),
                        othere = Convert.ToDouble(DBNull.Value.Equals(dr["othere"]) ? 0 : dr["othere"]),
                        extraexp = Convert.ToDouble(DBNull.Value.Equals(dr["extraexp"]) ? 0 : dr["extraexp"]),
                        mileagetravel = Convert.ToInt32(DBNull.Value.Equals(dr["mileagetravel"]) ? 0 : dr["mileagetravel"]),
                        mileage = Convert.ToInt32(DBNull.Value.Equals(dr["mileage"]) ? 0 : dr["mileage"]),
                        signatureCount = Convert.ToInt32(DBNull.Value.Equals(dr["signatureCount"]) ? 0 : dr["signatureCount"]),
                        DocumentCount = Convert.ToInt32(DBNull.Value.Equals(dr["DocumentCount"]) ? 0 : dr["DocumentCount"]),
                        workerid = Convert.ToInt32(DBNull.Value.Equals(dr["workerid"]) ? 0 : dr["workerid"]),
                        description = Convert.ToString(dr["description"]),
                        fdescreason = Convert.ToString(dr["fdescreason"]),
                        invoice = Convert.ToInt32(DBNull.Value.Equals(dr["invoice"]) ? 0 : dr["invoice"]),
                        Confirmed = Convert.ToInt16(DBNull.Value.Equals(dr["Confirmed"]) ? 0 : dr["Confirmed"]),
                        manualinvoice = Convert.ToString(dr["manualinvoice"]),
                        invoiceno = Convert.ToString(dr["invoiceno"]),
                        ownerid = Convert.ToInt32(DBNull.Value.Equals(dr["ownerid"]) ? 0 : dr["ownerid"]),
                        QBinvoiceid = Convert.ToString(dr["QBinvoiceid"]),
                        TransferTime = Convert.ToInt32(DBNull.Value.Equals(dr["TransferTime"]) ? 0 : dr["TransferTime"]),
                        serviceitem = Convert.ToString(dr["serviceitem"]),
                        PayrollItem = Convert.ToString(dr["PayrollItem"]),
                        RTOTTT = Convert.ToDouble(DBNull.Value.Equals(dr["RTOTTT"]) ? 0 : dr["RTOTTT"]),
                        WorkerLastName = Convert.ToString(dr["WorkerLastName"]),
                        //timesign = Encoding.ASCII.GetBytes(dr["timesign"].ToString()),
                        dispalert = Convert.ToInt32(DBNull.Value.Equals(dr["dispalert"]) ? 0 : dr["dispalert"]),
                        credithold = Convert.ToInt32(DBNull.Value.Equals(dr["credithold"]) ? 0 : dr["credithold"]),
                        high = Convert.ToInt32(DBNull.Value.Equals(dr["high"]) ? 0 : dr["high"]),
                        unitid = Convert.ToInt32(DBNull.Value.Equals(dr["unitid"]) ? 0 : dr["unitid"]),
                        unit = Convert.ToString(dr["unit"]),
                        unittype = Convert.ToString(dr["unittype"]),
                        defaultworker = Convert.ToString(dr["defaultworker"]),
                        defaultmech = Convert.ToString(dr["defaultmech"]),
                        department = Convert.ToString(dr["department"]),
                        bremarks = Convert.ToString(dr["bremarks"]),
                        laborexp = Convert.ToDouble(DBNull.Value.Equals(dr["laborexp"]) ? 0 : dr["laborexp"]),
                        //signature = Encoding.ASCII.GetBytes(dr["signature"].ToString()),
                        signature = Convert.ToString(dr["signature"]),
                        state = Convert.ToString(dr["state"]),
                        mileagepr = Convert.ToDouble(DBNull.Value.Equals(dr["mileagepr"]) ? 0 : dr["mileagepr"]),
                        afterhours = Convert.ToBoolean(DBNull.Value.Equals(dr["afterhours"]) ? null : dr["afterhours"]),
                        weekends = Convert.ToBoolean(DBNull.Value.Equals(dr["weekends"]) ? null : dr["weekends"]),
                        EmailNotified = Convert.ToInt32(DBNull.Value.Equals(dr["EmailNotified"]) ? 0 : dr["EmailNotified"]),
                        EmailTime = Convert.ToDateTime(DBNull.Value.Equals(dr["EmailTime"]) ? null : dr["EmailTime"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        ProjectDescription = Convert.ToString(dr["ProjectDescription"]),
                        //Custom6 = Encoding.ASCII.GetBytes(dr["Custom6"].ToString()),
                        //Custom7 = Encoding.ASCII.GetBytes(dr["Custom7"].ToString()),
                        Custom6 = Convert.ToString(dr["Custom6"]),
                        Custom7 = Convert.ToString(dr["Custom7"]),
                        fBy = Convert.ToString(dr["fBy"]),
                        WageCategory = Convert.ToString(dr["WageCategory"]),
                    });
            }

            return _lstGetCallHistory;
        }

        //inventory
        public DataSet IsItemOnHand(TicketI ticket)
        {
            return new DL_Tickets().IsItemOnHand(ticket);
        }

        public DataSet GetTicketIByID(TicketI ticket)
        {
            return new DL_Tickets().GetTicketIByID(ticket);
        }
        
        public DataSet GetTicketDataIByID(int TicketID, string ConnConfig)
        {
            return new DL_Tickets().GetTicketDataIByID( TicketID,  ConnConfig);
        }

    }
}
