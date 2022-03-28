using System;
using System.Data;
using DataLayer;
using BusinessEntity;
using BusinessEntity.InventoryModel;
using System.Collections.Generic;
using BusinessEntity.CustomersModel;

namespace BusinessLayer
{
    public class BL_MapData
    {
        DL_MapData objDL_MapData = new DL_MapData();

        public void AddMapData(MapData objMapData)
        {
            objDL_MapData.AddMapData(objMapData);
        }

        /// <summary>
        /// New Map Data For MS TEAM
        /// </summary>
        /// <param name="objMapData"></param>

        public void AddMapNewData(MapData objMapData1)
        {
            objDL_MapData.AddMapDataNew(objMapData1);
        }

        public void InsertMapDataNew(MapData objMapData1)
        {
            objDL_MapData.InsertMapDataNew(objMapData1);        
        }
        // End 




        public void UpdateTicket(MapData objMapData)
        {
            objDL_MapData.UpdateTicket(objMapData);
        }

        public void DeleteTicket(MapData objMapData)
        {
            objDL_MapData.DeleteTicket(objMapData);
        }

        public void UpdateTicketStatus(MapData objMapData)
        {
            objDL_MapData.UpdateTicketStatus(objMapData);
        }

        public void UpdateTicketResize(MapData objMapData)
        {
            objDL_MapData.UpdateTicketResize(objMapData);
        }

      
        public void AddTicket(MapData objMapData)
        {
            objDL_MapData.AddTicket(objMapData);
        }

        public void AddTicketTotalService(MapData objMapData)
        {
            objDL_MapData.AddTicketTotalService(objMapData);
        }

        public void AddTicketTS(MapData objMapData)
        {
            objDL_MapData.AddTicketTS(objMapData);
        }

        public int UpdateTicketInfo(MapData objMapData)
        {
            return objDL_MapData.UpdateTicketInfo(objMapData);
        }

        public int UpdateTicketInfoTotalService(MapData objMapData)
        {
            return objDL_MapData.UpdateTicketInfoTotalService(objMapData);
        }

        public int UpdateTicketInfoTS(MapData objMapData)
        {
            return objDL_MapData.UpdateTicketInfoTS(objMapData);
        }


        public DataSet GetTimestmpLocation(MapData objMapData)
        {
            return objDL_MapData.GetTimestmpLocation(objMapData);
        }

        public bool chkOneMonthExist()
        {
            return objDL_MapData.chkOneMonthExist();
        }
        public DataSet GetTimestmpLocationLatest(MapData objMapData)
        {
            return objDL_MapData.GetTimestmpLocationLatest(objMapData);
        }
         
        public DataSet GetTimestmpLocationTest(MapData objMapData)
        {
            return objDL_MapData.GetTimestmpLocationTest(objMapData);
        }

        public DataSet GetLogData(MapData objMapData)
        {
            return objDL_MapData.GetLogData(objMapData);
        }

        public DataSet GetTokenAndDeviceType(MapData objMapData)
        {
            return objDL_MapData.GetTokenAndDeviceType(objMapData);
        }
        
        public DataSet GetTimestmpLocationTest1(MapData objMapData)
        {
            return objDL_MapData.GetTimestmpLocationTest1(objMapData);
        }
        
        public DataSet getlocationAddress(MapData objMapData)
        {
            return objDL_MapData.getlocationAddress(objMapData);
        }

        public DataSet GetOpenTicket(MapData objMapData)
        {
            return objDL_MapData.GetOpenTicket(objMapData);
        }
                
        public DataSet GetOpenTicketScheduler(MapData objMapData)
        {
            return objDL_MapData.GetOpenTicketScheduler(objMapData);
        }

        public DataSet GetReportTicket(MapData objMapData)
        {
            return objDL_MapData.GetReportTicket(objMapData);
        }

        public DataSet GetClosedTicket(MapData objMapData)
        {
            return objDL_MapData.GetClosedTicket(objMapData);
        }

        public DataSet GetClosedTicketDTable(MapData objMapData)
        {
            return objDL_MapData.GetClosedTicketDTable(objMapData);
        }

        public DataSet GetNearWorkers(MapData objMapData)
        {
            return objDL_MapData.GetNearWorkers(objMapData);
        }

        public DataSet GetNearWorkersByTime(MapData objMapData)
        {
            return objDL_MapData.GetNearWorkersByTime(objMapData);
        }

        public DataSet GetNearWorkersDummy(MapData objMapData)
        {
            return objDL_MapData.GetNearWorkersDummy(objMapData);
        }

        public DataSet GetTimestmpLocationList(MapData objMapData)
        {
            return objDL_MapData.GetTimestmpLocationList(objMapData);
        }

        public DataSet GetCurrentLocation(MapData objMapData)
        {
            return objDL_MapData.GetCurrentLocation(objMapData);
        }

        public DataSet GetTechCurrentLocation(MapData objMapData)
        {
            return objDL_MapData.GetTechCurrentLocation(objMapData);
        }


        public DataSet GetTechCurrentLocationNew(MapData objMapData)
        {
            return objDL_MapData.GetTechCurrentLocationNew(objMapData);
        }
        public DataSet GetTicketByID(MapData objMapData)
        {
            return objDL_MapData.GetTicketByID(objMapData);
        }
        public DataSet GetContractInfo(MapData objPropMapData, Int32 EquipID, string Type)
        {
            return objDL_MapData.GetContractInfo(objPropMapData, EquipID, Type);
        }

        public DataSet GetSalesPerInfo(MapData objPropMapData)
        {
            return objDL_MapData.GetSalesPerInfo(objPropMapData);
        }
        
        public DataSet ResetisSendmailtosalesper(MapData objPropMapData)
        {
            return objDL_MapData.ResetisSendmailtosalesper(objPropMapData);
        }

        public void ResetisSendmailtosalesper2( int opptID, string ConnConfig)
        {
             objDL_MapData.ResetisSendmailtosalesper2(opptID,ConnConfig);
        }

        public string GetSalesPerInfo2(  string LID , string ConnConfig)
        {
           return objDL_MapData.GetSalesPerInfo2( LID, ConnConfig);
        }
        
        public DataSet getticketdtlbywday(MapData objMapData)
        {
            return objDL_MapData.getticketdtlbywday(objMapData);
        }

        public DataSet GetTicketbyWorkorder(MapData objMapData)
        {
            return objDL_MapData.GetTicketbyWorkorder(objMapData);
        }
        public DataSet GetTicketbyLocation(MapData objMapData)
        {
            return objDL_MapData.GetTicketbyLocation(objMapData);
        }

        public DataSet GetWorkorderDate(MapData objMapData)
        {
            return objDL_MapData.GetWorkorderDate(objMapData);
        }

        public string GetopportunityTicket(MapData objMapData)
        {
            return objDL_MapData.GetopportunityTicket(objMapData);
        }

        public DataSet GetChargeableTickets(MapData objMapData)
        {
            return objDL_MapData.GetChargeableTickets(objMapData);
        }

        public DataSet GetChargeableTicketsMapping(MapData objMapData)
        {
            return objDL_MapData.GetChargeableTicketsMapping(objMapData);
        }

        
        
        public DataSet GetRequestForServiceCall(MapData objPropMapData, Int32 IsSalesAsigned = 0)
        {
            return objDL_MapData.GetRequestForServiceCall(objPropMapData, IsSalesAsigned);
        }
        public DataSet GetProjectTickets(MapData objMapData, string StartDate = "", string EndDate = "")
        {
            return objDL_MapData.GetProjectTickets(objMapData, StartDate, EndDate);
        }
        
        public DataSet GetMonthlyCompleteOpenTickets(MapData objMapData)
        {
            return objDL_MapData.GetMonthlyCompleteOpenTickets(objMapData);
        }

        public DataSet GetOpenTicketsByMechanicReport(MapData objMapData)
        {
            return objDL_MapData.GetOpenTicketsByMechanicReport(objMapData);
        }

        public DataSet GetCompletedTicketsByMechanicReport(MapData objMapData)
        {
            return objDL_MapData.GetCompletedTicketsByMechanicReport(objMapData);
        }

        public DataSet GetCategories(MapData objMapData)
        {
            return objDL_MapData.GetCategories(objMapData);
        }

        public DataSet GetAllWorkers(MapData objMapData)
        {
            return objDL_MapData.GetAllWorkers(objMapData);
        }

        public DataSet GetCategoriesByMechanic(MapData objMapData)
        {
            return objDL_MapData.GetCategoriesByMechanic(objMapData);
        }

        public DataSet GetTicketDetailsByMechanic(MapData objMapData)
        {
            return objDL_MapData.GetTicketDetailsByMechanic(objMapData);
        }

        public DataSet GetAllServiceCallBackReport(MapData objMapData)
        {
            return objDL_MapData.GetAllServiceCallBackReport(objMapData);
        }

        public DataSet getTicketdetailsReport(MapData objMapData)
        {
            return objDL_MapData.getTicketdetailsReport(objMapData);
        }

        public DataSet GetTicketsByWorkerDateOLD(MapData objMapData)
        {
            return objDL_MapData.GetTicketsByWorkerDateOLD(objMapData);
        }

        public DataSet GetRecurringTickets(MapData objMapData)
        {
            return objDL_MapData.GetRecurringTickets(objMapData);
        }

        public void AddFile(MapData objMapData)
        {
            objDL_MapData.AddFile(objMapData);
        }

        //API
        public void AddFile(AddFileParam _AddFileParam, string ConnectionString)
        {
            objDL_MapData.AddFile(_AddFileParam, ConnectionString);
        }

        public void UpdateQBInvoiceTicketID(MapData objMapData)
        {
            objDL_MapData.UpdateQBInvoiceTicketID(objMapData);
        }

        public void UpdateQBTimeTxnIDTicket(MapData objMapData)
        {
            objDL_MapData.UpdateQBTimeTxnIDTicket(objMapData);
        }

        public void UpdateFile(MapData objMapData)
        {
            objDL_MapData.UpdateFile(objMapData);
        }

        public void UpdateFileMSVisible(MapData objMapData,DataTable dt)
        {
            objDL_MapData.UpdateFileMSVisible(objMapData,dt);
        }

        public void DeleteFile(MapData objMapData)
        {
            objDL_MapData.DeleteFile(objMapData);
        }

        //API
        public void DeleteFile(DeleteFileParam _DeleteFileParam, string ConnectionString)
        {
            objDL_MapData.DeleteFile(_DeleteFileParam, ConnectionString);
        }

        public DataSet SelectTempDocumentFile(MapData objMapData)
        {
            return objDL_MapData.SelectTempDocumentFile(objMapData);
        }

        public DataSet GetDocuments(MapData objMapData)
        {
            return objDL_MapData.GetDocuments(objMapData);
        }

        //API
        public List<GetDocumentsViewModel> GetDocuments(GetDocumentsParam _GetDocumentsParam, string ConnectionString)
        {
            DataSet ds = objDL_MapData.GetDocuments(_GetDocumentsParam, ConnectionString);

            List<GetDocumentsViewModel> _lstGetDocuments = new List<GetDocumentsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetDocuments.Add(
                    new GetDocumentsViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        ScreenID = Convert.ToInt32(DBNull.Value.Equals(dr["ScreenID"]) ? 0 : dr["ScreenID"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Filename = Convert.ToString(dr["Filename"]),
                        Path = Convert.ToString(dr["Path"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Custom1 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom1"]) ? null : dr["Custom1"]),
                        Custom2 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom2"]) ? null : dr["Custom2"]),
                        Custom3 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom3"]) ? null : dr["Custom3"]),
                        Custom4 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom4"]) ? null : dr["Custom4"]),
                        Custom5 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom5"]) ? null : dr["Custom5"]),
                        Custom6 = Convert.ToInt16(DBNull.Value.Equals(dr["Custom6"]) ? 0 : dr["Custom6"]),
                        Custom7 = Convert.ToInt16(DBNull.Value.Equals(dr["Custom7"]) ? 0 : dr["Custom7"]),
                        Custom8 = Convert.ToInt16(DBNull.Value.Equals(dr["Custom8"]) ? 0 : dr["Custom8"]),
                        Custom9 = Convert.ToInt16(DBNull.Value.Equals(dr["Custom9"]) ? 0 : dr["Custom9"]),
                        Custom10 = Convert.ToInt16(DBNull.Value.Equals(dr["Custom10"]) ? 0 : dr["Custom10"]),
                        Custom11 = Convert.ToString(dr["Custom11"]),
                        Custom12 = Convert.ToString(dr["Custom12"]),
                        Custom13 = Convert.ToString(dr["Custom13"]),
                        Custom14 = Convert.ToString(dr["Custom14"]),
                        Custom15 = Convert.ToString(dr["Custom15"]),
                        TempID = Convert.ToString(dr["TempID"]),
                        Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
                        Subject = Convert.ToString(dr["Subject"]),
                        Body = Convert.ToString(dr["Body"]),
                        Portal = Convert.ToInt16(DBNull.Value.Equals(dr["Portal"]) ? 0 : dr["Portal"]),
                        MSVisible = Convert.ToBoolean(DBNull.Value.Equals(dr["MSVisible"]) ? false : dr["MSVisible"]),
                        lat = Convert.ToString(dr["lat"]),
                        lng = Convert.ToString(dr["lng"]),
                        attached_on = Convert.ToDateTime(DBNull.Value.Equals(dr["attached_on"]) ? null : dr["attached_on"]),
                        doctype = Convert.ToString(dr["doctype"]),
                    }
                    );

            }
            return _lstGetDocuments;

        }

        public DataSet GetLocationDocuments(MapData objMapData, bool isShowAll, bool isLocation)
        {
            return objDL_MapData.GetLocationDocuments(objMapData, isShowAll, isLocation);
        }

        public DataSet GetProjectDocuments(MapData objMapData, bool isShowAll)
        {
            return objDL_MapData.GetProjectDocuments(objMapData, isShowAll);
        }

        //API
        public List<GetLocationDocumentsViewModel> GetLocationDocuments(GetLocationDocumentsParam _GetLocationDocuments, string ConnectionString, bool isShowAll, bool isLocation)
        {
            DataSet ds = objDL_MapData.GetLocationDocuments(_GetLocationDocuments, ConnectionString, isShowAll, isLocation);

            List<GetLocationDocumentsViewModel> _lstGetLocationDocuments = new List<GetLocationDocumentsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationDocuments.Add(
                    new GetLocationDocumentsViewModel()
                    {
                        Filename = Convert.ToString(dr["Filename"]),
                        doctype = Convert.ToString(dr["doctype"]),
                        Project = Convert.ToInt32(DBNull.Value.Equals(dr["Project"]) ? 0 : dr["Project"]),
                        ProjectName = Convert.ToString(dr["ProjectName"]),
                        Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                        AssignedTo = Convert.ToString(dr["AssignedTo"]),
                        Date = Convert.ToInt32(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
                        Path = Convert.ToString(dr["Path"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Portal = Convert.ToInt16(DBNull.Value.Equals(dr["Portal"]) ? 0 : dr["Portal"]),
                    }
                    );
            }
            return _lstGetLocationDocuments;
        }

        public DataSet GetLibrary(MapData objMapData)
        {
            return objDL_MapData.GetLibrary(objMapData);
        }

        public DataSet GetSignature(MapData objMapData)
        {
            return objDL_MapData.GetSignature(objMapData);
        }

        public DataSet GetTicketSignature(MapData objMapData)
        {
            return objDL_MapData.GetTicketSignature(objMapData);
        }

        public DataSet GetInvoiceTicketByWorkorder(MapData objMapData)
        {
            return objDL_MapData.GetInvoiceTicketByWorkorder(objMapData);
        }

        public DataSet GetTicketsByWorkorder(MapData objMapData)
        {
            return objDL_MapData.GetTicketsByWorkorder(objMapData);
        }

        public DataSet GetRecentCallsLoc(MapData objMapData)
        {
            return objDL_MapData.GetRecentCallsLoc(objMapData);
        }

        public DataSet GetTicketTime(MapData objMapData)
        {
            return objDL_MapData.GetTicketTime(objMapData);
        }

        public DataSet GetTicketTimeMapping(MapData objMapData)
        {
            return objDL_MapData.GetTicketTimeMapping(objMapData);
        }
                

        public void IndexMapdata(MapData objMapData)
        {
            objDL_MapData.IndexMapdata(objMapData);
        }

        public DataSet getElevByTicket(MapData objMapData)
        {
            return objDL_MapData.getElevByTicket(objMapData);
        }

        //API
        public List<GetElevByTicketViewModel> getElevByTicket(GetElevByTicketParam _GetElevByTicket, string ConnectionString)
        {
            DataSet ds = objDL_MapData.getElevByTicket(_GetElevByTicket, ConnectionString);

            List<GetElevByTicketViewModel> _lstGetElevByTicket = new List<GetElevByTicketViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetElevByTicket.Add(
                    new GetElevByTicketViewModel()
                    {
                        ticketid = Convert.ToInt32(DBNull.Value.Equals(dr["ticketid"]) ? 0 : dr["ticketid"]),
                        unit = Convert.ToString(dr["unit"]),
                        elev_id = Convert.ToInt32(DBNull.Value.Equals(dr["elev_id"]) ? 0 : dr["elev_id"]),
                        labor_percentage = Convert.ToBoolean(DBNull.Value.Equals(dr["labor_percentage"]) ? false : dr["labor_percentage"]),
                        serial = Convert.ToString(dr["serial"]),
                        state = Convert.ToString(dr["state"]),
                        owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                    }
                    );
            }
            return _lstGetElevByTicket;
        }


        public List<GetElevByTicketViewModel> getElevByTicketID(GetElevByTicketIDParam _GetElevByTicketID, string ConnectionString)
        {
            DataSet ds = objDL_MapData.getElevByTicketID(_GetElevByTicketID, ConnectionString);
            List<GetElevByTicketViewModel> _lstGetElevByTicketID = new List<GetElevByTicketViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetElevByTicketID.Add(
                    new GetElevByTicketViewModel()
                    {
                        ticketid = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket_ID"]) ? 0 : dr["Ticket_ID"]),
                        unit = Convert.ToString(dr["unit"]),
                        elev_id = Convert.ToInt32(DBNull.Value.Equals(dr["elev_id"]) ? 0 : dr["elev_id"]),
                        labor_percentage = Convert.ToBoolean(DBNull.Value.Equals(dr["labor_percentage"]) ? false : dr["labor_percentage"]),
                        serial = Convert.ToString(dr["serial"]),
                        state = Convert.ToString(dr["state"]),
                        owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                    }
                    );
            }
            return _lstGetElevByTicketID;
        }

        //API
        public DataSet getElevByTicketID(MapData objMapData)
        {
            return objDL_MapData.getElevByTicketID(objMapData);
        }

        public DataSet GetElevByTicketIDs(MapData objMapData, string ticketIDs)
        {
            return objDL_MapData.GetElevByTicketIDs(objMapData, ticketIDs);
        }

        //API
        public List<GetElevByTicketIDsViewModel> GetElevByTicketIDs(GetElevByTicketIDsParam _GetElevByTicketIDs, string ConnectionString, string ticketIDs)
        {
            DataSet ds = objDL_MapData.GetElevByTicketIDs(_GetElevByTicketIDs, ConnectionString, ticketIDs);

            List<GetElevByTicketIDsViewModel> _lstGetElevByTicketIDs = new List<GetElevByTicketIDsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetElevByTicketIDs.Add(
                    new GetElevByTicketIDsViewModel()
                    {
                        TicketID = Convert.ToInt32(DBNull.Value.Equals(dr["TicketID"]) ? 0 : dr["TicketID"]),
                        Unit = Convert.ToString(dr["Unit"]),
                        elev_id = Convert.ToInt32(DBNull.Value.Equals(dr["elev_id"]) ? 0 : dr["elev_id"]),
                        labor_percentage = Convert.ToBoolean(DBNull.Value.Equals(dr["labor_percentage"]) ? false : dr["labor_percentage"]),
                        Serial = Convert.ToString(dr["Serial"]),
                        State = Convert.ToString(dr["State"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                    }
                    );
            }
            return _lstGetElevByTicketIDs;
        }

        public DataSet GetTicketItemByIDs(MapData objMapData, string ticketIDs)
        {
            return objDL_MapData.GetTicketItemByIDs(objMapData, ticketIDs);
        }

        public void UpdateEmailNotificationStatus(MapData objMapData)
        {
            objDL_MapData.UpdateEmailNotificationStatus(objMapData);
        }

        public DataSet getTicketTaskByTicketID(MapData objMapData)
        {
            return objDL_MapData.getTicketTaskByTicketID(objMapData);
        }
        public void AddMultipleTicket(MapData objMapData)
        {
            objDL_MapData.AddMultipleTicket(objMapData);
        }

        public void AddticketfrmCustPortal(MapData objMapData)
        {
            objDL_MapData.AddticketfrmCustPortal(objMapData);
        }
        public string AddTicketFromProject(MapData objMapData)
        {
          return  objDL_MapData.AddTicketFromProject(objMapData);
        }

        //Rahil
        public string getTicketListSignature(MapData objMapData)
        {
            return objDL_MapData.getTicketListSignature(objMapData);
        }
        public DataSet GetTicketLogs(MapData objMapData)
        {
            return objDL_MapData.GetTicketLogs(objMapData);
        }

        public void ImportDataForMassAttachDocuments(string config, DataTable dataTable)
        {
            objDL_MapData.ImportDataForMassAttachDocuments(config, dataTable);
        }

        //API
        public void ImportDataForMassAttachDocuments(ImportDataForMassAttachDocumentsParam _ImportDataForMassAttachDocuments, string ConnectionString, DataTable dataTable)
        {
            objDL_MapData.ImportDataForMassAttachDocuments(_ImportDataForMassAttachDocuments, ConnectionString, dataTable);
        }

        public DataSet PostInventoryItemsToProject(MapData objMapData)
        {
            return objDL_MapData.PostInventoryItemsToProject(objMapData);
        }

        //API
        public ListPostInventoryItemsToProject PostInventoryItemsToProject(PostInventoryItemsToProjectParam _PostInventoryItemsToProjectParam, string ConnectionString)
        {
            DataSet ds = objDL_MapData.PostInventoryItemsToProject(_PostInventoryItemsToProjectParam, ConnectionString);

            ListPostInventoryItemsToProject _ds = new ListPostInventoryItemsToProject();
            List<PostInventoryItemsToProjectTable> _lstTable = new List<PostInventoryItemsToProjectTable>();
            List<PostInventoryItemsToProjectTable1> _lstTable1 = new List<PostInventoryItemsToProjectTable1>();
            List<PostInventoryItemsToProjectTable2> _lstTable2 = new List<PostInventoryItemsToProjectTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable.Add(
                    new PostInventoryItemsToProjectTable()
                    {
                        Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Item = Convert.ToInt32(DBNull.Value.Equals(dr["Item"]) ? 0 : dr["Item"]),
                        Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Charge = Convert.ToInt16(DBNull.Value.Equals(dr["Charge"]) ? 0 : dr["Charge"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Phase = Convert.ToString(dr["Phase"]),
                        AID = Convert.ToString(dr["AID"]),
                        TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"]) ? 0 : dr["TypeID"]),
                        WarehouseID = Convert.ToString(dr["WarehouseID"]),
                        LocationID = Convert.ToInt32(DBNull.Value.Equals(dr["LocationID"]) ? 0 : dr["LocationID"]),
                        PhaseName = Convert.ToString(dr["PhaseName"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable1.Add(
                    new PostInventoryItemsToProjectTable1()
                    {
                       Column1 = Convert.ToInt32(DBNull.Value.Equals(dr["Column1"]) ? 0 : dr["Column1"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable2.Add(
                    new PostInventoryItemsToProjectTable2()
                    {
                        Column1 = Convert.ToString(dr["Column1"]),
                    }
                    );
            }

            _ds.lstTable = _lstTable;
            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }

        public void UpdateTicketManuaInvoice(MapData objMapData)
        {
             objDL_MapData.UpdateTicketManuaInvoice(objMapData);
        }


        public DataSet SearchTicketInLocation(String conn, int loc,string prefix,int ticketYear)
        {
            return objDL_MapData.SearchTicketInLocation(conn,loc,prefix, ticketYear);
        }
    }
}
