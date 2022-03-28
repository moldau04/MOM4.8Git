using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;

namespace DataLayer
{
    public class DL_MapData
    {
        public void AddMapData(MapData objMapData)
        {
            SqlParameter para = new SqlParameter();
            para.ParameterName = "DtMapData";
            para.SqlDbType = SqlDbType.Structured;
            para.Value = objMapData.LocData;

            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.StoredProcedure, "spAddMapData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Create New For MS TEAM
        public void AddMapDataNew(MapData objMapData)
        {
            SqlParameter para = new SqlParameter();
            para.ParameterName = "DtMapDataNew";
            para.SqlDbType = SqlDbType.Structured;
            para.Value = objMapData.LocData;



            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.StoredProcedure, "spAddMapDataNew", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertMapDataNew(MapData objMapData)
        {
            var para = new SqlParameter[2];
            para[0] = new SqlParameter
            {
                ParameterName = "DtMapDataNew",
                SqlDbType = SqlDbType.Structured,
                Value = objMapData.LocData
            };
            para[1] = new SqlParameter
            {
                ParameterName = "database",
                SqlDbType = SqlDbType.VarChar,
                Value = objMapData.Database
            };
            //string test = HttpContext.Current.Session["config"].ToString();
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.StoredProcedure, "spInsertMapDataNew", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateTicket(MapData objMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "update TicketO set edate='" + objMapData.Date + "', Assigned=" + objMapData.Assigned + " ,DWork='" + objMapData.Tech + "', fwork=(select top 1 w.ID from tblwork w where w.fdesc= '" + objMapData.Tech + "') where ID=" + objMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTicket(MapData objMapData)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from TicketO where ID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from TicketD where ID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from TicketDPDA where ID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from Documents where screen='Ticket' and screenID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from PDATicketSignature where PDATicketID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "insert into tblticketdeleted(ticketid, date) values (" + objMapData.TicketID + ",getdate())");

                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, "spDeleteTicket", objMapData.TicketID, objMapData.Worker);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateTicketStatus(MapData objMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "update TicketO set  Assigned=" + objMapData.Assigned + " where ID=" + objMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTicketResize(MapData objMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "update TicketO set edate='" + objMapData.Date + "',est=" + objMapData.Resize + " where ID=" + objMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //inventory
        //public void UpdateTicketI(TicketI ticket)
        //{
        //    try
        //    {
        //        string query = "update TicketI set Item='"+ticket.Item+ "',Quan='"+ticket.Quan+ "',fDesc='"+ticket.fDesc+"',Charge='"+ticket.Charge+"',Amount='"+ticket.Amount+"',Phase='"+ticket.Phase+"',TypeID='"+ticket.TypeID+"',WarehouseID='"+ticket.WarehouseID+"',LocationID='"+ticket.WHLocationID+"',PhaseName='"+ticket.PhaseName+"' where Ticket='"+ticket.TicketID+"' AND Line='"+ticket.Line+"'";
        //        SqlHelper.ExecuteScalar(ticket.ConnConfig, CommandType.Text, query);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void DeleteTicketI(TicketI ticket)
        //{
        //    try
        //    {
        //        string query = "delete from TicketI where Ticket='"+ ticket.TicketID + "' and Line='"+ticket.Line+"'";
        //        SqlHelper.ExecuteScalar(ticket.ConnConfig, CommandType.Text, query);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet IsItemExisting(TicketI ticket)
        //{
        //    try
        //    {
        //        string query = "select * from TicketI where Ticket='"+ticket.TicketID+"' and Line ='"+ticket.Line+"'";
        //        return ticket.DS = SqlHelper.ExecuteDataset(ticket.ConnConfig, CommandType.Text, query);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void AddTicketI(TicketI ticket)
        //{
        //    try
        //    {
        //        string query = "insert into TicketI (Ticket,Line,Item,Quan,fDesc,Charge,Amount,Phase,TypeID,WarehouseID,LocationID,PhaseName) values('" + ticket.TicketID + "'," +
        //                                                                                                                                    "'" + ticket.Line + "'," +
        //                                                                                                                                     "'" + ticket.Item + "'," +
        //                                                                                                                                     "'" + ticket.Quan + "'," +
        //                                                                                                                                     "'" + ticket.fDesc + "'," +
        //                                                                                                                                     "'" + ticket.Charge + "'," +
        //                                                                                                                                     "'" + ticket.Amount + "'," +
        //                                                                                                                                     "'" + ticket.Phase +"',"+
        //                                                                                                                                     "'" + ticket.TypeID + "',"+
        //                                                                                                                                     "'" + ticket.WarehouseID + "',"+
        //                                                                                                                                     "'" + ticket.WHLocationID + "',"+
        //                                                                                                                                     "'" + ticket.PhaseName + "'" + ")";


        //        SqlHelper.ExecuteScalar(ticket.ConnConfig, CommandType.Text, query);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public void AddTicket(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraticketout = new SqlParameter();
                paraticketout.ParameterName = "TicketIDOut";
                paraticketout.SqlDbType = SqlDbType.Int;
                paraticketout.Direction = ParameterDirection.Output;
                int ticid;

                SqlParameter paraEquip = new SqlParameter();
                paraEquip.ParameterName = "Equipments";
                paraEquip.SqlDbType = SqlDbType.Structured;
                paraEquip.Value = objMapData.dtEquips;
                paraEquip.TypeName = "tblTypeMultipleEequipments";

                SqlParameter paraTask = new SqlParameter();
                paraTask.ParameterName = "@TaskCodes";
                paraTask.SqlDbType = SqlDbType.Structured;
                paraTask.Value = objMapData.dtTasks;
                paraTask.TypeName = "tblTypeTaskCodes";

                SqlParameter ParamTicketINV = new SqlParameter();
                ParamTicketINV.ParameterName = "dtTicketINV";
                ParamTicketINV.SqlDbType = SqlDbType.Structured;
                ParamTicketINV.Value = objMapData.dtTicketINV;
                ParamTicketINV.TypeName = "tblTypeTicketINV";

                //ticid = Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spAddTicket", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.EST, objMapData.CompDescription, paraticketout, System.Guid.NewGuid(), objMapData.Who, objMapData.Signature, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Remarks, objMapData.Level, objMapData.Department, objMapData.jobid, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.IsRecurring, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobcode, objMapData.JobTemplateID, objMapData.WageID, objMapData.fBy, DBNull.Value, paraEquip, 1, paraTask, objMapData.BT, objMapData.Comments, objMapData.PartsUsed, objMapData.Zone, ParamTicketINV));
                //objMapData.TicketID =Convert.ToInt32( paraticketout.Value);
                //objMapData.TicketID = ticid;

                var ds = SqlHelper.ExecuteDataset(objMapData.ConnConfig, "spAddTicket", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.EST, objMapData.CompDescription, paraticketout, System.Guid.NewGuid(), objMapData.Who, objMapData.Signature, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Remarks, objMapData.Level, objMapData.Department, objMapData.jobid, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.IsRecurring, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobcode, objMapData.JobTemplateID, objMapData.WageID, objMapData.fBy, DBNull.Value, paraEquip, 1, paraTask, objMapData.BT, objMapData.Comments, objMapData.PartsUsed, objMapData.Zone, ParamTicketINV, "");
                var count = ds.Tables.Count;
                if (count > 0)
                {
                    ticid = Convert.ToInt32(ds.Tables[count - 1].Rows[0][0].ToString());
                    objMapData.TicketID = ticid;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTicketTotalService(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraticketout = new SqlParameter();
                paraticketout.ParameterName = "TicketIDOut";
                paraticketout.SqlDbType = SqlDbType.Int;
                paraticketout.Direction = ParameterDirection.Output;
                int ticid;

                SqlParameter paraEquip = new SqlParameter();
                paraEquip.ParameterName = "Equipments";
                paraEquip.SqlDbType = SqlDbType.Structured;
                paraEquip.Value = objMapData.dtEquips;
                paraEquip.TypeName = "tblTypeMultipleEequipments";

                SqlParameter paraTask = new SqlParameter();
                paraTask.ParameterName = "@TaskCodes";
                paraTask.SqlDbType = SqlDbType.Structured;
                paraTask.Value = objMapData.dtTasks;
                paraTask.TypeName = "tblTypeTaskCodes";

                ticid = Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spAddTicketTotalService", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.EST, objMapData.CompDescription, paraticketout, System.Guid.NewGuid(), objMapData.Who, objMapData.Signature, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Remarks, objMapData.Level, objMapData.Department, objMapData.jobid, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.IsRecurring, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobcode, objMapData.JobTemplateID, objMapData.WageID, objMapData.fBy, DBNull.Value, paraEquip, 1, paraTask, objMapData.BT, objMapData.Comments, objMapData.PartsUsed, objMapData.Zone));
                //objMapData.TicketID =Convert.ToInt32( paraticketout.Value);
                objMapData.TicketID = ticid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTicketTS(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraticketout = new SqlParameter();
                paraticketout.ParameterName = "TicketIDOut";
                paraticketout.SqlDbType = SqlDbType.Int;
                paraticketout.Direction = ParameterDirection.Output;
                int ticid;

                ticid = Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spAddTicketTS", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.EST, objMapData.CompDescription, paraticketout, System.Guid.NewGuid(), objMapData.Who, objMapData.Signature, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Remarks, objMapData.Level, objMapData.Department, objMapData.jobid, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.IsRecurring, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobcode, objMapData.JobTemplateID, objMapData.fBy));
                //objMapData.TicketID =Convert.ToInt32( paraticketout.Value);
                objMapData.TicketID = ticid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateTicketInfo(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraEquip = new SqlParameter();
                paraEquip.ParameterName = "Equipments";
                paraEquip.SqlDbType = SqlDbType.Structured;
                paraEquip.Value = objMapData.dtEquips;
                paraEquip.TypeName = "tblTypeMultipleEequipments";

                SqlParameter paraTask = new SqlParameter();
                paraTask.ParameterName = "@TaskCodes";
                paraTask.SqlDbType = SqlDbType.Structured;
                paraTask.Value = objMapData.dtTasks;
                paraTask.TypeName = "tblTypeTaskCodes";


                SqlParameter ParamTicketINV = new SqlParameter();
                ParamTicketINV.ParameterName = "dtTicketINV";
                ParamTicketINV.SqlDbType = SqlDbType.Structured;
                ParamTicketINV.Value = objMapData.dtTicketINV;
                ParamTicketINV.TypeName = "tblTypeTicketINV";

                string inv = null;

                if (objMapData.InvoiceID > 0) { inv = objMapData.InvoiceID.ToString(); }

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spUpdateTicket",
                    objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip
                    , objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned
                    , paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName
                    , objMapData.CustID, objMapData.TicketID, objMapData.EST, objMapData.CompDescription, objMapData.RT, objMapData.OT
                    , objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review
                    , objMapData.Who, objMapData.Signature, objMapData.Remarks, objMapData.Department, objMapData.Custom1
                    , objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6
                    , objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense
                    , objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, inv
                    , objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.QBServiceID
                    , objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1
                    , objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker
                    , objMapData.CustomTick5, objMapData.jobid, objMapData.jobcode, objMapData.JobTemplateID, objMapData.WageID
                    , objMapData.fBy, paraEquip, 1, paraTask, objMapData.BT, objMapData.Comments, objMapData.PartsUsed, objMapData.IsCreateJob
                    , objMapData.Zone, objMapData.Level, ParamTicketINV, objMapData.PayRoll, objMapData.ManualInvoiceID));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateTicketInfoTotalService(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraEquip = new SqlParameter();
                paraEquip.ParameterName = "Equipments";
                paraEquip.SqlDbType = SqlDbType.Structured;
                paraEquip.Value = objMapData.dtEquips;
                paraEquip.TypeName = "tblTypeMultipleEequipments";

                SqlParameter paraTask = new SqlParameter();
                paraTask.ParameterName = "@TaskCodes";
                paraTask.SqlDbType = SqlDbType.Structured;
                paraTask.Value = objMapData.dtTasks;
                paraTask.TypeName = "tblTypeTaskCodes";

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spUpdateTicketTotalService", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.TicketID, objMapData.EST, objMapData.CompDescription, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Who, objMapData.Signature, objMapData.Remarks, objMapData.Department, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobid, objMapData.jobcode, objMapData.JobTemplateID, objMapData.WageID, objMapData.fBy, paraEquip, 1,
                    paraTask, objMapData.BT, objMapData.Comments, objMapData.PartsUsed, objMapData.Zone, objMapData.Level));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateTicketInfoTS(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spUpdateTicketTS", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.TicketID, objMapData.EST, objMapData.CompDescription, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Who, objMapData.Signature, objMapData.Remarks, objMapData.Department, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobid, objMapData.jobcode, objMapData.JobTemplateID, objMapData.fBy));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTimestmpLocation(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTimestmpLocation", objPropMapData.Tech, objPropMapData.Date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool chkOneMonthExist()
        {
            try
            {
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, "SELECT COUNT(*)  FROM MSM2_Admin.SYS.TABLES WHERE NAME = 'MapDataOfOneMonth'"));

                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTimestmpLocationLatest(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTimestmpLocationLatest", objPropMapData.Tech, objPropMapData.Date, objPropMapData.Category, objPropMapData.ISTicketD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetTimestmpLocationTest(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetLocationTimeStmp", objPropMapData.Tech, objPropMapData.Date, objPropMapData.Category, objPropMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetLogData(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(Config.MS, "spGetLogData", objPropMapData.Tech, objPropMapData.Date, objPropMapData.Database);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTokenAndDeviceType(MapData objPropMapData)
        {
            try
            {
                //return objPropMapData.Ds = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select TokenId,DeviceType from PushNotifications  where fuser ='" + objPropMapData.fuser + "' ");
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select TokenId,DeviceType from PushNotifications  where fuser ='" + objPropMapData.fuser + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetTimestmpLocationTest1(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTimestmpLocationTest1", objPropMapData.Tech, objPropMapData.Date, objPropMapData.Category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getlocationAddress(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "SpgetlocationLatlong", objPropMapData.Tech, objPropMapData.Date, objPropMapData.CallDate, objPropMapData.TicketID, objPropMapData.TempId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenTicket(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select distinct t.cat, t.id, assigned, ldesc1, edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address  from TicketO t where DWork='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' and Assigned not in (0) union select distinct d.cat, d.ID ,4 as assigned ,l.ID as ldesc1 ,edate,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address  from TicketD d inner join tblWork w on d.fWork=w.ID inner join Loc l on d.Loc=l.Loc where w.fDesc='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' order by EDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public DataSet GetOpenTicketScheduler(MapData objPropMapData)
        {

            try
            {

                #region TicketO
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT DISTINCT ''                                                         AS descres, CONVERT(VARCHAR(MAX), t.fdesc) as fdesc , \n");
                varname1.Append("                t.cat, \n");
                varname1.Append("                Isnull(Confirmed, 0)                                       AS Confirmed, \n");
                varname1.Append("                0                                                          AS comp, \n");
                varname1.Append("                CASE \n");
                varname1.Append("                  WHEN Assigned = 1 THEN 'Assigned' \n");
                varname1.Append("                  WHEN Assigned = 2 THEN 'Enroute' \n");
                varname1.Append("                  WHEN Assigned = 3 THEN 'Onsite' \n");
                varname1.Append("                  WHEN Assigned = 4 THEN 'Completed' \n");
                varname1.Append("                  WHEN Assigned = 5 THEN 'Hold' \n");
                varname1.Append("                END                                                        AS assignname, \n");
                varname1.Append("                t.phone, \n");
                varname1.Append("                edate, \n");
                varname1.Append("                t.id, \n");
                varname1.Append("                assigned, \n");
                varname1.Append("                ( 'Ticket #: ' + isnull(CONVERT(VARCHAR(50), t.id ),'') \n");
                varname1.Append("                  + ', ' + isnull((select top 1 Name from Rol where ID=(select top 1 Rol from Owner where  id= t.Owner)),'') +' , ' + isnull(ldesc1,'') + ' - ' + isnull(ldesc2,'') )  AS name, \n");

                varname1.Append("                (select top 1 Name from Rol where ID=(select top 1 Rol from Owner where  id= t.Owner)) As Customer, \n");
                varname1.Append("                (select Tag from Loc l where l.Loc=t.LID) as Location, \n");
                varname1.Append("                est, \n");
                varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
                varname1.Append("                Upper(dwork)                                               AS [column], \n");
                varname1.Append("                0                                                          AS allday, \n");
                varname1.Append("                CASE \n");
                varname1.Append("                  WHEN Assigned = 1 THEN 'White' \n");
                varname1.Append("                  WHEN Assigned = 2 THEN '#9EF767' \n");
                varname1.Append("                  WHEN Assigned = 3 THEN 'orange' \n");
                varname1.Append("                  WHEN Assigned = 4 THEN 'DeepSkyBlue' \n");
                varname1.Append("                  WHEN Assigned = 5 THEN 'yellow' \n");

                varname1.Append("                END                                                        AS color, \n");
                varname1.Append("                CASE \n");
                varname1.Append("                  WHEN Assigned IN ( 4, 3, 2 ) THEN Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                varname1.Append("                                                    +cast( Cast(Isnull( TimeRoute, '7/9/2012 12:00:00 AM') AS TIME)as datetime) \n");
                varname1.Append("                  ELSE EDate \n");
                varname1.Append("                END                                                        AS start, \n");

                #region 24 Hours Feature

                // 24Hours Feature Time range end date 


                varname1.Append("   CASE \n");
                varname1.Append("         WHEN Assigned = 4 THEN ( CASE \n");
                varname1.Append("                                    WHEN ( TimeRoute IS NULL \n");
                varname1.Append("                                            OR TimeSite IS NULL \n");
                varname1.Append("                                            OR TimeComp IS NULL )THEN Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                varname1.Append("                                    ELSE Cast(Cast( ( CASE WHEN ( Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME) AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME) ) THEN Dateadd(day, 2, EDate) ELSE (( CASE WHEN ( Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME) OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME) ) THEN Dateadd(day, 1, EDate) ELSE EDate END )) END ) AS DATE) AS DATETIME) \n");
                varname1.Append("                                         + Cast(( Cast( TimeComp AS TIME))AS DATETIME) \n");
                varname1.Append("                                  END ) \n");
                varname1.Append("         WHEN assigned = 3 THEN (( CASE \n");
                varname1.Append("                                     WHEN ( TimeRoute IS NULL \n");
                varname1.Append("                                             OR TimeSite IS NULL )THEN Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                varname1.Append("                                     ELSE Cast(Cast( ( CASE WHEN ( Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME) ) THEN Dateadd(day, 1, EDate) END ) AS DATE) AS DATETIME) \n");
                varname1.Append("                                          + Cast(( Cast( TimeSite AS TIME))AS DATETIME) \n");
                varname1.Append("                                   END )) \n");
                varname1.Append("         WHEN assigned = 2 THEN ( Cast( Cast(edate AS DATE) AS DATETIME ) \n");
                varname1.Append("                                  + Cast( Cast(Isnull( TimeRoute, '7/9/2012 12:01:00 AM') AS TIME)AS DATETIME) ) \n");
                varname1.Append("         ELSE Dateadd(MINUTE, Isnull(Est, 0) * 60, edate) \n");
                varname1.Append("       END AS [endDesc] , \n");

                // 24 hour service so a ticket can be started on one day and closed the next day

                varname1.Append("        CASE \n");
                varname1.Append("                   WHEN Assigned = 4 THEN Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                varname1.Append("                                          + CASE WHEN ( TimeSite is null or TimeComp is null or TimeRoute is null) THEN (  Cast( Cast(Isnull( TimeComp, '7/9/2012 12:01:00 AM') AS TIME ) AS DATETIME ) ) \n");
                varname1.Append(" 										  ELSE  ( \n");
                varname1.Append(" 										  Cast((CASE WHEN ( Cast( TimeComp AS TIME) >= Cast ( TimeSite AS TIME) ) and  ( Cast( TimeSite AS TIME) >= Cast ( TimeRoute AS TIME) )  THEN Cast( TimeComp AS TIME) ELSE (CONVERT(TIME, '23:59:59' )) END) AS DATETIME) \n");
                varname1.Append(" 										    \n");
                varname1.Append(" 										  ) END \n");
                varname1.Append("                   WHEN assigned = 3 THEN Cast(Cast(edate AS DATE) AS DATETIME) \n");
                varname1.Append("                                          + case when ( TimeRoute is null or TimeSite is null) then Cast( Cast(Isnull( TimeSite, '7/9/2012 12:01:00 AM') AS TIME)AS datetime) \n");
                varname1.Append(" 										 else( \n");
                varname1.Append(" 										   Cast((CASE WHEN Cast( TimeSite AS TIME) >= Cast ( TimeRoute AS TIME) THEN Cast( TimeSite AS TIME) ELSE (CONVERT(TIME, '23:59:59' )) END) AS DATETIME) \n");
                varname1.Append(" 										 ) \n");
                varname1.Append(" 										 end \n");
                varname1.Append("                   WHEN assigned = 2 THEN Dateadd(MINUTE, est * 60, Cast( Cast(edate AS DATE) AS DATETIME ) \n");
                varname1.Append("                                                                    +Cast( Cast(Isnull( TimeRoute, '7/9/2012 12:01:00 AM') AS TIME)AS datetime)) \n");
                varname1.Append("                   ELSE Dateadd(MINUTE, isnull(Est,0) * 60, edate) \n");
                varname1.Append("                 END                                                        AS [end] , \n");

                #endregion

                varname1.Append("                0                                                          AS ClearCheck, \n");
                varname1.Append("                (SELECT Count(1) \n");
                varname1.Append("                 FROM   documents \n");
                varname1.Append("                 WHERE  screen = 'Ticket' \n");
                varname1.Append("                        AND screenid = t.id)                               AS DocumentCount, \n");
                varname1.Append("                t.fwork                                                    AS workerid, \n");
                varname1.Append("                0                                                          AS invoice, \n");
                varname1.Append("                t.charge                                                          AS charge, \n");
                varname1.Append("                ''                                                         AS manualinvoice, \n");
                varname1.Append("                ''                                                         AS qbinvoiceid, \n");
                varname1.Append("                l.owner                                                      AS ownerid, \n");
                varname1.Append("               (select ISNULL(Credit,0) from Loc l where l.Loc=t.LID)as credithold, \n");
                varname1.Append("               (select ISNULL(DispAlert,0) from Loc l where l.Loc=t.LID)as DispAlert, \n");
                varname1.Append("                t.WorkOrder, \n");
                varname1.Append("               t.City \n");
                varname1.Append("              , 0 Recommendation \n");
                varname1.Append("FROM   TicketO t \n");
                varname1.Append("       LEFT OUTER JOIN tblWork w \n");
                varname1.Append("                    ON w.fDesc = t.DWork \n");
                varname1.Append("       INNER JOIN loc l \n");
                varname1.Append("               ON l.loc = t.lid \n");
                varname1.Append("WHERE  Assigned NOT IN ( 0, 4 ) ");



                string str = varname1.ToString();

                str += " and t.ID is not null ";

                if (objPropMapData.Worker != string.Empty)
                {
                    str += " and DWork='" + objPropMapData.Worker + "'";
                }

                if (objPropMapData.Assigned != -1)
                {
                    str += " and Assigned=" + objPropMapData.Assigned;
                }

                if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                {
                    str += " and w.super ='" + objPropMapData.Supervisor + "'";
                }

                if (objPropMapData.StartDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) >='" + objPropMapData.StartDate + "'";
                }

                if (objPropMapData.EndDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) <='" + objPropMapData.EndDate + "'";
                }

                if (objPropMapData.Department != 0)
                {
                    str += " and t.type=" + objPropMapData.Department;
                }

                if (objPropMapData.Category != string.Empty)
                {
                    str += " and t.Cat='" + objPropMapData.Category + "'";
                }
                if (objPropMapData.LocTag != string.Empty)
                {
                    str += " and  l.tag like    '%" + objPropMapData.LocTag + "%' ";
                }

                if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                {


                    str += " union all ";

                    #endregion TicketO

                    #region TicketD

                    StringBuilder varname2 = new StringBuilder();
                    varname2.Append("SELECT DISTINCT CONVERT(VARCHAR(MAX), descres)                                AS descres, CONVERT(VARCHAR(MAX), d.fdesc) as fdesc , \n");
                    varname2.Append("                d.cat, \n");
                    varname2.Append("                0                                                             AS Confirmed, \n");
                    varname2.Append("                1                                                             AS comp, \n");
                    varname2.Append("                   case    d.assigned when 6 then 'Voided' else    'Completed'  end                                                  AS assignname, \n");
                    varname2.Append("                ''                                                            AS phone, \n");
                    varname2.Append("                d.edate, \n");
                    varname2.Append("                d.id, \n");
                    varname2.Append("               case    d.assigned when 6 then 6 else 4 end                                                              AS assigned, \n");
                    varname2.Append("                ( 'Ticket #: ' + isnull(CONVERT(VARCHAR(50), d.id ),'') \n");
                    varname2.Append("                  + ', ' + isnull((select top 1 Name from Rol where ID=(select top 1 Rol from Owner where  id= l.Owner)),'')+' , ' + isnull(l.ID,'') + ' - ' + isnull(l.Tag,'') )    AS name, \n");
                    varname2.Append("                (select top 1 Name from Rol where ID=(select top 1 Rol from Owner where  id= l.Owner)) As Customer, \n");
                    varname2.Append("                ( L.Tag  ) as Location, \n");
                    varname2.Append("                d.Est, \n");
                    varname2.Append("                ( l.Address + ', ' + l.City + ', ' + l.State + ', ' + l.Zip ) AS address, \n");
                    varname2.Append("                Upper(w.fDesc)                                                AS [column], \n");
                    varname2.Append("                0                                                             AS allday, \n");
                    varname2.Append("                'DeepSkyBlue'                                                 AS color, \n");

                    varname2.Append("                CASE \n");
                    varname2.Append("                  WHEN Cast(Isnull(TimeRoute, '7/9/2012 12:00:00 AM') AS TIME) = Cast('7/9/2012 12:00:00 AM' AS TIME) THEN edate \n");
                    varname2.Append("                  ELSE Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                    varname2.Append("                       + Cast( Cast( TimeRoute AS TIME)as datetime) \n");
                    varname2.Append("                END                                                           AS start, \n");

                    #region 24 Hours Feature

                    //  24 hour service so a ticket can be started on one day and closed the next day

                    varname2.Append("                CASE \n");
                    varname2.Append("     WHEN (TimeRoute is null or TimeSite is null or TimeComp is null )THEN Dateadd(MINUTE, Isnull(d.total, 0) * 60, edate) \n");
                    varname2.Append("         ELSE Cast(Cast( ( CASE WHEN ( Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME) AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME) ) THEN Dateadd(day, 2, EDate) ELSE (( CASE WHEN ( Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME) OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME) ) THEN Dateadd(day, 1, EDate) ELSE EDate END )) END ) AS DATE) AS DATETIME) \n");
                    varname2.Append("              + Cast(( Cast( TimeComp AS TIME))AS DATETIME) \n");
                    varname2.Append("       END AS [enddesc] ,");

                    // 24Hours Feature
                    varname2.Append("     CASE \n");
                    varname2.Append("     WHEN (TimeRoute is null or TimeSite is null or TimeComp is null )THEN Dateadd(MINUTE, Isnull(d.total, 0) * 60, edate) \n");
                    varname2.Append("                  ELSE Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                    varname2.Append("                       + Cast((CASE WHEN ( Cast( TimeSite AS TIME) >= Cast ( TimeRoute AS TIME) and Cast( TimeComp AS TIME) >= Cast ( TimeSite AS TIME)) THEN Cast( TimeComp AS TIME) ELSE (CONVERT(TIME, '23:59:59' )) END)AS DATETIME) \n");
                    varname2.Append("                END AS [end] ,");

                    #endregion


                    varname2.Append("                Isnull(ClearCheck, 0)                                         AS ClearCheck, \n");
                    varname2.Append("                (SELECT Count(1) \n");
                    varname2.Append("                 FROM   documents \n");
                    varname2.Append("                 WHERE  screen = 'Ticket' \n");
                    varname2.Append("                        AND screenid = d.id)                                  AS DocumentCount, \n");
                    varname2.Append("                d.fwork                                                       AS workerid, \n");
                    varname2.Append("                Isnull(invoice, 0)                                            AS invoice, \n");
                    varname2.Append("                d.charge, \n");
                    varname2.Append("                manualinvoice, \n");
                    varname2.Append("               isnull( qbinvoiceid,'') as qbinvoiceid, \n");
                    varname2.Append("                0                                                             AS ownerid, \n");
                    varname2.Append("                l.credit as  credithold,l.DispAlert, \n");
                    varname2.Append("                d.WorkOrder, \n");
                    varname2.Append("                l.City \n");
                    varname2.Append("                , len(isnull(d.BRemarks,''))   Recommendation \n");
                    varname2.Append("FROM   TicketD d \n");
                    varname2.Append("       INNER JOIN Loc l \n");
                    varname2.Append("               ON l.Loc = d.Loc \n");
                    varname2.Append("       INNER JOIN tblWork w \n");
                    varname2.Append("               ON d.fWork = w.ID ");


                    str += varname2.ToString();

                    str += " where d.ID is not null ";//and TimeComp is not null and TimeRoute is not null
                    if (objPropMapData.Worker != string.Empty)
                    {
                        str += " and w.fdesc='" + objPropMapData.Worker + "'";
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and w.super ='" + objPropMapData.Supervisor + "'";
                    }

                    if (objPropMapData.StartDate != DateTime.MinValue)
                    {
                        str += " and DATEADD(d, 0, DATEDIFF(d, 0, d.edate)) >='" + objPropMapData.StartDate + "'";
                    }

                    if (objPropMapData.EndDate != DateTime.MinValue)
                    {
                        str += " and DATEADD(d, 0, DATEDIFF(d, 0, d.edate)) <='" + objPropMapData.EndDate + "'";
                    }

                    if (objPropMapData.Department != 0)
                    {
                        str += " and d.type=" + objPropMapData.Department;
                    }

                    if (objPropMapData.Category != string.Empty)
                    {
                        str += " and d.Cat='" + objPropMapData.Category + "'";
                    }
                    if (objPropMapData.LocTag != string.Empty)
                    {
                        str += " and  l.tag like    '%" + objPropMapData.LocTag + "%' ";
                    }

                }



                str += " union all ";

                #endregion TicketD

                #region TicketDPDA
                StringBuilder varname3 = new StringBuilder();
                varname3.Append("SELECT DISTINCT CONVERT(VARCHAR(MAX), descres)                                AS descres, CONVERT(VARCHAR(MAX), dp.fdesc) as fdesc , \n");
                varname3.Append("                t.cat, \n");
                varname3.Append("                1                                                             AS Confirmed, \n");
                varname3.Append("                2                                                             AS comp, \n");
                varname3.Append("                'Completed'                                                   AS assignname, \n");
                varname3.Append("                t.phone, \n");
                varname3.Append("                dp.edate, \n");
                varname3.Append("                t.id, \n");
                varname3.Append("                4                                                             AS assigned, \n");
                varname3.Append("                ( 'Ticket #: ' + isnull(CONVERT(VARCHAR(50), t.id ),'') \n");
                varname3.Append("                  + ', '+ isnull((select top 1 Name from Rol where ID=(select top 1 Rol from Owner where  id= t.Owner)),'')+' , ' + isnull(ldesc1,'') + ' - ' + isnull(ldesc2,'') )   AS name, \n");
                varname3.Append("                (select top 1 Name from Rol where ID=(select top 1 Rol from Owner where  id= t.owner)) As Customer, \n");
                varname3.Append("                ( select Tag from Loc l where l.Loc=t.LID  ) as Location, \n");
                varname3.Append("                dp.Est, \n");
                varname3.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip )    AS address, \n");
                varname3.Append("                Upper(dwork)                                                  AS [column], \n");
                varname3.Append("                0                                                             AS allday, \n");
                varname3.Append("                'DeepSkyBlue'                                                 AS color, \n");
                varname3.Append("                Cast(Cast(dp.EDate AS DATE) AS DATETIME) \n");
                varname3.Append("                + Cast(Cast(Isnull( dp.TimeRoute, '7/9/2012 12:00:00 AM') AS TIME)as datetime) AS start, \n");

                #region 24 Hours Feature

                // 24Hours Feature

                varname3.Append("            CASE \n");
                varname3.Append("                 WHEN (dp.TimeRoute is null or dp.TimeSite is null or dp.TimeComp is null )THEN Dateadd(MINUTE, Isnull(dp.total, 0) * 60, dp.EDate) \n");
                varname3.Append("                     ELSE Cast(Cast( ( CASE WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME) AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 2, dp.EDate) ELSE (( CASE WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME) OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 1, dp.EDate) ELSE dp.EDate END )) END ) AS DATE) AS DATETIME) \n");
                varname3.Append("                          + Cast(( Cast( dp.TimeComp AS TIME))AS DATETIME) \n");
                varname3.Append("                   END AS [enddesc] , \n");

                // 24Hours Feature

                varname3.Append(" \n");
                varname3.Append(" Cast(Cast(dp.EDate AS DATE) AS DATETIME) \n");
                varname3.Append("       + Cast(Cast(Isnull((CASE WHEN (Cast( dp.TimeSite AS TIME) >= Cast ( dp.TimeRoute AS TIME)  and Cast( dp.TimeComp AS TIME) >= Cast ( dp.TimeSite AS TIME) )THEN Cast( dp.TimeComp AS TIME) ELSE (CONVERT(TIME, '23:59:59' )) END), '7/9/2012 12:01:00 AM') AS TIME)AS DATETIME) AS [end] , \n");

                #endregion

                varname3.Append("                0                                                             AS ClearCheck, \n");
                varname3.Append("                (SELECT Count(1) \n");
                varname3.Append("                 FROM   documents \n");
                varname3.Append("                 WHERE  screen = 'Ticket' \n");
                varname3.Append("                        AND screenid = t.id)                                  AS DocumentCount, \n");
                varname3.Append("                t.fwork                                                       AS workerid, \n");
                varname3.Append("                0                                                             AS invoice, \n");
                varname3.Append("                dp.charge, \n");
                varname3.Append("                ''                                                            AS manualinvoice, \n");
                varname3.Append("                ''                                                            AS qbinvoiceid, \n");
                varname3.Append("                t.owner                                                       AS ownerid, \n");
                varname3.Append("               (select ISNULL(Credit,0) from Loc l where l.Loc=t.LID)as credithold, \n");
                varname3.Append("               (select ISNULL(DispAlert,0) from Loc l where l.Loc=t.LID)as DispAlert, \n");
                varname3.Append("               dp.WorkOrder, \n");
                varname3.Append("               t.City \n");
                varname3.Append("              , 0 Recommendation \n");
                varname3.Append("FROM   TicketDPDA dp \n");
                varname3.Append("       INNER JOIN TicketO t \n");
                varname3.Append("               ON t.ID = dp.ID \n");
                varname3.Append("       INNER JOIN tblWork w \n");
                varname3.Append("               ON dp.fWork = w.ID ");
                varname3.Append("       INNER JOIN loc l \n");
                varname3.Append("               ON l.loc = t.lid \n");

                str += varname3.ToString();

                str += " where t.ID is not null "; //and dp.TimeComp is not null and dp.TimeRoute is not null
                if (objPropMapData.Worker != string.Empty)
                {
                    str += " and DWork='" + objPropMapData.Worker + "'";
                }

                if (objPropMapData.Assigned != -1)
                {
                    str += " and Assigned=" + objPropMapData.Assigned;
                }
                if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                {
                    str += " and w.super ='" + objPropMapData.Supervisor + "'";
                }
                if (objPropMapData.StartDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) >='" + objPropMapData.StartDate + "'";
                }

                if (objPropMapData.EndDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) <='" + objPropMapData.EndDate + "'";
                }

                if (objPropMapData.Department != 0)
                {
                    str += " and t.type=" + objPropMapData.Department;
                }

                if (objPropMapData.Category != string.Empty)
                {
                    str += " and t.Cat='" + objPropMapData.Category + "'";
                }

                if (objPropMapData.LocTag != string.Empty)
                {
                    str += " and  l.tag like    '%" + objPropMapData.LocTag + "%' ";
                }

                str += " order by EDate";

                #endregion TicketDPDA

                str = @"declare  @tt  table ( descres nvarchar(max),
                                    fdesc nvarchar(max),
                                    cat nvarchar(1000),
                                    Confirmed int,
                                    comp int,
                                    assignname nvarchar(1000),	phone nvarchar(1000),
                                    edate datetime,
                                    id  int,
                                    assigned int,
                                    name nvarchar(1000),
                                    Customer nvarchar(1000),
                                    Location nvarchar(1000),
                                    est nvarchar(1000),
                                    address nvarchar(1000),	column1 nvarchar(1000), 
                                    allday int,
                                    color nvarchar(1000),
                                    start1 DATETIME,
                                    endDesc nvarchar(1000),
                                    end1 DATETIME,
                                    ClearCheck int,
                                    DocumentCount int,
                                    workerid int,
                                    invoice int,
                                    charge int,
                                    manualinvoice nvarchar(1000),
                                    qbinvoiceid nvarchar(1000),
                                    ownerid int,
                                    credithold int,
                                    DispAlert int,
                                    WorkOrder nvarchar(1000),	
                                    City nvarchar(1000)  , Recommendation  int)

                                    INSERT INTO @tt " + str;


                str += " select * from @tt where    start1 <= end1 ";


                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRequestForServiceCall(MapData objPropMapData, Int32 IsSalesAsigned = 0)
        {

            try
            {
                #region All parameters
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "EN";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropMapData.EN;

                para[1] = new SqlParameter();
                para[1].ParameterName = "UserID";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPropMapData.UserID;

                para[2] = new SqlParameter();
                para[2].ParameterName = "IsSalesAsigned";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = IsSalesAsigned;


                #endregion
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "SpGetRequestForServiceCall", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectTickets(MapData objPropMapData, string StartDate = "", string EndDate = "")
        {
            try
            {

                #region All parameters
                SqlParameter[] para = new SqlParameter[7];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Job";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropMapData.jobid;

                para[1] = new SqlParameter();
                para[1].ParameterName = "OrderBy";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = objPropMapData.OrderBy;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Assigned";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objPropMapData.Assigned;

                para[3] = new SqlParameter();
                para[3].ParameterName = "PageIndex";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = objPropMapData.PageIndex;

                para[4] = new SqlParameter();
                para[4].ParameterName = "PageSize";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = objPropMapData.PageSize;

                para[5] = new SqlParameter();
                para[5].ParameterName = "StartDate";
                para[5].SqlDbType = SqlDbType.NVarChar;
                para[5].Value = StartDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "EndDate";
                para[6].SqlDbType = SqlDbType.NVarChar;
                para[6].Value = EndDate;

                #endregion

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetProjectTickets", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMonthlyCompleteOpenTickets(MapData objPropMapData)
        {
            try
            {

                #region All parameters
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.FromDate
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Todate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Todate
                };




                #endregion

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetMonthlyCompleteOpenTickets", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenTicketsByMechanicReport(MapData objPropMapData)
        {
            try
            {

                #region All parameters
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.FromDate
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Todate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Todate
                };

                #endregion

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spOpenTicketsByMechanics", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCompletedTicketsByMechanicReport(MapData objPropMapData)
        {
            try
            {

                #region All parameters
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "User",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Worker
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.FromDate
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "Todate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Todate
                };

                #endregion

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spCompletedTicketsByUser", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCategories(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetCategories");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllWorkers(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetAllWorkers");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCategoriesByMechanic(MapData objPropMapData)
        {
            try
            {

                #region All parameters
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "Worker",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Worker
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.FromDate
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "Todate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Todate
                };

                #endregion

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "SpGetCategoriesByMechanic", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketDetailsByMechanic(MapData objPropMapData)
        {
            try
            {

                #region All parameters
                var para = new SqlParameter[4];

                para[0] = new SqlParameter
                {
                    ParameterName = "Worker",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Worker
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Category",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Category
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.FromDate
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "Todate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Todate
                };

                #endregion

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "SpGetTicketDetailsByMechanic", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllServiceCallBackReport(MapData objPropMapData)
        {
            try
            {

                #region All parameters
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.FromDate
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Todate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropMapData.Todate
                };




                #endregion

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetAllServiceCallBacks", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportTicket(MapData objPropMapData)
        {
            try
            {
                string str = " select cat,timeroute,timesite,timecomp, isnull(Confirmed,0) as Confirmed, 0 as comp, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname, t.phone, edate, t.id, assigned,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName,ldesc2 as locname,est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address ,fdesc , '' as descres, dwork, 0.00 as Reg, 0.00 as OT,0.00 as  NT, 0.00 as DT,0.00 as TT,0.00 as  Total,0.00 as  Tottime  from TicketO t  where Assigned not in (0,4)   ";

                //if (objPropMapData.Worker != "-1")
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {
                    str += " and  DWork='" + objPropMapData.Worker + "'";
                }
                if (objPropMapData.StartDate != System.DateTime.MinValue)
                {
                    str += " and edate >='" + objPropMapData.StartDate + "'";
                }
                if (objPropMapData.EndDate != System.DateTime.MinValue)
                {
                    str += " and edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                }

                str += " union all select cat,timeroute,timesite,timecomp,  1 as Confirmed, 1 as comp, 'Completed' as assignname,'' as phone, d.edate, d.id, 4 as assigned,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=l.Owner)) as customerName, l.Tag as locname, d.Est,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address , d.fdesc, descres,w.fDesc as dwork, reg, OT,nt,dt,tt,total, total as tottime from TicketD d inner join Loc l on l.Loc=d.Loc inner join tblWork w on d.fWork=w.ID ";
                str += " where d.ID is not null and TimeComp is not null and TimeRoute is not null";
                //if (objPropMapData.Worker != "-1")
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {
                    str += " and  w.fDesc='" + objPropMapData.Worker + "'";
                }
                if (objPropMapData.StartDate != System.DateTime.MinValue)
                {
                    str += " and edate >='" + objPropMapData.StartDate + "'";
                }
                if (objPropMapData.EndDate != System.DateTime.MinValue)
                {
                    str += " and edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                }

                str += " union all select t.cat, t.timeroute,t.timesite,t.timecomp, 1 as Confirmed, 2 as comp, 'Completed' as assignname, t.phone, dp.edate, t.id, 4 as assigned,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName,ldesc2 as locname,dp.Est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address , t.fDesc, descres,w.fDesc as dwork, reg, OT,nt,dt,tt,total, total as tottime from TicketDPDA dp inner join TicketO t on t.ID= dp.ID inner join tblWork w on dp.fWork=w.ID ";
                str += " where t.ID is not null and dp.TimeComp is not null and dp.TimeRoute is not null";
                //if (objPropMapData.Worker != "-1")
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {
                    str += " and  t.DWork='" + objPropMapData.Worker + "'";
                }
                if (objPropMapData.StartDate != System.DateTime.MinValue)
                {
                    str += " and t.edate >='" + objPropMapData.StartDate + "'";
                }
                if (objPropMapData.EndDate != System.DateTime.MinValue)
                {
                    str += " and t.edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                }


                str += " order by EDate";

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketsByWorkerDateOLD(MapData objPropMapData)
        {
            string str = " select 0.00 as tottime, timeroute, timesite, timecomp,dwork,ID, (select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName, t.LDesc2 as locname, t.Ldesc4 as address, Phone, Cat, EDate, CDate, '' as descres, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname ,Est from TicketO t where  assigned not in ( 0) ";

            if (objPropMapData.Worker != "-1")
            {
                str += " and  DWork='" + objPropMapData.Worker + "'";
            }
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                str += " and edate >='" + objPropMapData.StartDate + "'";
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                str += " and edate <='" + objPropMapData.EndDate.AddDays(1) + "'";
            }

            str += " union all select total as tottime, timeroute, timesite, timecomp, w.fDesc as dwork,t.ID, l.Tag as customerName, l.tag as locname, l.Address,  (select top 1 Phone from rol where ID=l.Rol) as Phone, cat, EDate, CDate, DescRes, 'Completed' as assignname , Est from TicketD t inner join Loc l on l.Loc=t.Loc inner join tblWork w on t.fWork=w.ID where t.id is not null ";

            if (objPropMapData.Worker != "-1")
            {
                str += " and w.fDesc='" + objPropMapData.Worker + "'";
            }
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                str += " and edate >='" + objPropMapData.StartDate + "'";
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                str += " and edate <='" + objPropMapData.EndDate.AddDays(1) + "'";
            }

            str += " order by edate desc";

            //select dwork,ID, (select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName, t.LDesc2 as locname, t.Ldesc4 as address, Phone, Cat, EDate, CDate, '' as descres, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname ,Est from TicketO t where  assigned not in ( 0) and  DWork='" + objPropMapData.Worker + "' and EDate between '" + objPropMapData.StartDate + "' and DATEADD(day,1,'" + objPropMapData.EndDate + "') union all select w.fDesc as dwork,t.ID, l.Tag as customerName, l.tag as locname, l.Address,  '' as Phone, '' as cat, EDate, CDate, DescRes, 'Completed' as assignname , Est from TicketD t inner join Loc l on l.Loc=t.Loc inner join tblWork w on t.fWork=w.ID where w.fDesc='" + objPropMapData.Worker + "' and EDate between '" + objPropMapData.StartDate + "' and DATEADD(day,1,'" + objPropMapData.EndDate + "')"

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetClosedTicket(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select distinct t.id,  ldesc1, edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address  from TicketO t inner join tblUser u on t.DWork=u.fUser where DWork='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' and Assigned =4  order by EDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetClosedTicketDTable(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select distinct  l.ID , edate,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address  from TicketD d inner join tblWork w on d.fWork=w.ID inner join Loc l on d.Loc=l.Loc where w.fDesc='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' order by EDate ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetNearWorkers(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT TOP 10 dbo.Distancebetween(latitude, longitude, " + objPropMapData.Latitude + ", " + objPropMapData.Longitude + ") AS distance, \n");
            varname1.Append("                       latitude, \n");
            varname1.Append("                       longitude, \n");
            varname1.Append("                       deviceId, \n");
            varname1.Append("                       date, \n");
            varname1.Append("                       (SELECT CallSign \n");
            varname1.Append("                        FROM   emp e \n");
            varname1.Append("                        WHERE  e.deviceid = m.deviceId)                                                             AS emp \n");
            varname1.Append("FROM   [MSM2_Admin].dbo.MapData m \n");
            varname1.Append("WHERE  m.ID IN (SELECT DISTINCT Max(ID) \n");
            varname1.Append("                FROM   [MSM2_Admin].dbo.mapdata \n");
            varname1.Append("                WHERE  date BETWEEN Dateadd(MINUTE, -15, '" + objPropMapData.Date + "') AND Dateadd(MINUTE, 15, '" + objPropMapData.Date + "') \n");
            varname1.Append("                       AND deviceId IN (SELECT DISTINCT e.deviceid \n");
            varname1.Append("                                        FROM   emp e \n");
            varname1.Append("                                        WHERE  e.deviceid IS NOT NULL \n");
            varname1.Append("                                               AND e.deviceid <> '') \n");
            varname1.Append("                GROUP  BY deviceId) \n");
            varname1.Append("ORDER  BY distance ");

            //////varname1.Append("select name as emp, 20 as distance from route");

            try
            {
                //return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetNearWorkers", objPropMapData.Date, objPropMapData.Latitude, objPropMapData.Longitude);
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetNearWorkersByTime(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetNearWorkerByTime", objPropMapData.Lat, objPropMapData.Lng, objPropMapData.Worker);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetNearWorkersDummy(MapData objPropMapData)
        {
            string str = "select name as emp, 20 as distance from route";
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTimestmpLocationList(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTimestmpLocationList", objPropMapData.Tech, objPropMapData.Date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCurrentLocation(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT latitude, \n");
            varname1.Append("                longitude, \n");
            varname1.Append("                deviceId, \n");
            varname1.Append("                date, \n");
            varname1.Append("                (SELECT TOP 1 e.CallSign \n");
            varname1.Append("                 FROM   emp e \n");
            varname1.Append("                 WHERE  e.deviceid = m.deviceId) AS callsign \n");
            varname1.Append("FROM   [MSM2_Admin].dbo.MapData m \n");
            varname1.Append("WHERE  m.ID IN (SELECT Max(ID) \n");
            varname1.Append("                FROM   [MSM2_Admin].dbo.mapdata \n");
            varname1.Append("                WHERE  Dateadd(DAY, Datediff(DAY, 0, date), 0) = Dateadd(DAY, Datediff(DAY, 0, Getdate()), 0) \n");
            varname1.Append("                       AND deviceId IN (SELECT DISTINCT e.deviceid \n");
            varname1.Append("                                        FROM   emp e \n");
            varname1.Append("                                        WHERE  e.deviceid IS NOT NULL \n");
            varname1.Append("                                               AND e.deviceid <> '') \n");
            varname1.Append("                GROUP  BY deviceId) ");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTechCurrentLocation(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTechCurrentLocation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTechCurrentLocationNew(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTechCurrentLocationNew");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetWorkorderDate(MapData objPropMapData)
        {
            string str = string.Empty;
            if (objPropMapData.CustID != 0)
            {
                str = " select top 1 WorkOrder,ID, edate, fdesc,who, cdate from ticketd where WorkOrder=cast(id as varchar(10)) and WorkOrder = '" + objPropMapData.Workorder + "'";
                str += " select top 1 WorkOrder,ID, edate from ticketd where  WorkOrder <> cast(id as varchar(10)) and WorkOrder = '" + objPropMapData.Workorder + "' order by EDate";
            }
            else
            {
                str = " select top 1 WorkOrder,ID, edate, fdesc,who, cdate from ticketd where WorkOrder=cast(id as varchar(10)) and WorkOrder = '" + objPropMapData.Workorder + "'";
                str += " union  all select top 1 WorkOrder,ID, edate, fdesc,who, cdate from TicketO where WorkOrder=cast(id as varchar(10)) and WorkOrder =  '" + objPropMapData.Workorder + "'";

                System.Text.StringBuilder varname1 = new System.Text.StringBuilder();
                varname1.Append("SELECT TOP 1 * \n");
                varname1.Append("FROM   (SELECT WorkOrder, \n");
                varname1.Append("               ID, \n");
                varname1.Append("               edate \n");
                varname1.Append("        FROM   ticketd \n");
                varname1.Append("        WHERE  WorkOrder <> Cast(id AS VARCHAR(10)) \n");
                varname1.Append("               AND WorkOrder =  '" + objPropMapData.Workorder + "' \n");
                varname1.Append("        UNION ALL \n");
                varname1.Append("        SELECT WorkOrder, \n");
                varname1.Append("               ID, \n");
                varname1.Append("               edate \n");
                varname1.Append("        FROM   TicketO \n");
                varname1.Append("        WHERE  WorkOrder <> Cast(id AS VARCHAR(10)) \n");
                varname1.Append("               AND WorkOrder =  '" + objPropMapData.Workorder + "')AS tt \n");
                varname1.Append("ORDER  BY EDate ");

                str += varname1.ToString();
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketbyWorkorder(MapData objPropMapData)
        {
            System.Text.StringBuilder varname1 = new System.Text.StringBuilder();
            varname1.Append("SELECT 1 as comp, ID, \n");
            varname1.Append("     cdate,  EDate, \n");
            varname1.Append("     workorder, \n");
            varname1.Append("       REPLACE(REPLACE(convert(varchar(max),fdesc), CHAR(13), ''), CHAR(10), '') fdesc, \n");
            varname1.Append("       REPLACE(REPLACE(convert(varchar(max),descres), CHAR(13), ''), CHAR(10), '') descres, \n");
            varname1.Append("       (SELECT TOP 1 fDesc \n");
            varname1.Append("        FROM   tblwork \n");
            varname1.Append("        WHERE  ID = d.fWork)                              AS dwork, \n");
            varname1.Append("       fWork, \n");
            varname1.Append("       CONVERT(VARCHAR(15), Cast(timeroute AS TIME), 100) ER, \n");
            varname1.Append("       CONVERT(VARCHAR(15), Cast(timesite AS TIME), 100)  OS, \n");
            varname1.Append("       CONVERT(VARCHAR(15), Cast(timecomp AS TIME), 100)  CT, \n");
            varname1.Append("       Cat \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            else
            {
                varname1.Append("union ALL \n");
                varname1.Append("  \n");
                varname1.Append(" SELECT 0 as comp, o.ID, \n");
                varname1.Append("     o.CDate,  o.EDate, \n");
                varname1.Append("     o.WorkOrder, \n");
                varname1.Append("       REPLACE(REPLACE(CONVERT(varchar(max),o.fDesc), CHAR(13), ''), CHAR(10), '') fdesc, \n");
                varname1.Append("       REPLACE(REPLACE(CONVERT(varchar(max),descres), CHAR(13), ''), CHAR(10), '') descres, \n");
                varname1.Append("       (SELECT TOP 1 fDesc \n");
                varname1.Append("        FROM   tblwork \n");
                varname1.Append("        WHERE  ID = o.fWork)                              AS dwork, \n");
                varname1.Append("       o.fWork, \n");
                varname1.Append("       CONVERT(VARCHAR(15), Cast(o.TimeRoute AS TIME), 100) ER, \n");
                varname1.Append("       CONVERT(VARCHAR(15), Cast(o.TimeSite AS TIME), 100)  OS, \n");
                varname1.Append("       CONVERT(VARCHAR(15), Cast(o.TimeComp AS TIME), 100)  CT, \n");
                varname1.Append("       o.Cat \n");
                varname1.Append("FROM   TicketO o LEFT OUTER JOIN TicketDPDA dp ON dp.ID=o.ID \n");
                varname1.Append("WHERE  o.WorkOrder = '" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND o.LID =  " + objPropMapData.LocID + " \n");
            }

            varname1.Append("       order by edate ");


            varname1.Append(" SELECT distinct e.Unit, \n");
            varname1.Append("       e.Type, \n");
            varname1.Append("       e.Manuf, \n");
            varname1.Append("       e.Serial, \n");
            varname1.Append("       e.State \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("       INNER JOIN Elev e \n");
            varname1.Append("               ON e.ID = d.Elev ");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            varname1.Append("union SELECT DISTINCT e.Unit, \n");
            varname1.Append("                e.Type, \n");
            varname1.Append("                e.Manuf, \n");
            varname1.Append("                e.Serial, \n");
            varname1.Append("                e.State \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("       INNER JOIN multiple_equipments me \n");
            varname1.Append("               ON d.ID = me.ticket_id \n");
            varname1.Append("       INNER JOIN elev e \n");
            varname1.Append("               ON e.id = me.elev_id ");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            else
            {
                varname1.Append("union \n");
                varname1.Append("  \n");
                varname1.Append("SELECT DISTINCT e.Unit, \n");
                varname1.Append("                e.Type, \n");
                varname1.Append("                e.Manuf, \n");
                varname1.Append("                e.Serial, \n");
                varname1.Append("                e.State \n");
                varname1.Append("FROM   TicketO o \n");
                varname1.Append("       INNER JOIN Elev e \n");
                varname1.Append("               ON e.ID = o.LElev \n");
                varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND o.LID = " + objPropMapData.LocID + " \n");

                varname1.Append("union SELECT DISTINCT e.Unit, \n");
                varname1.Append("                e.Type, \n");
                varname1.Append("                e.Manuf, \n");
                varname1.Append("                e.Serial, \n");
                varname1.Append("                e.State \n");
                varname1.Append("FROM   Ticketo d \n");
                varname1.Append("       INNER JOIN multiple_equipments me \n");
                varname1.Append("               ON d.ID = me.ticket_id \n");
                varname1.Append("       INNER JOIN elev e \n");
                varname1.Append("               ON e.id = me.elev_id ");
                varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND d.LID = " + objPropMapData.LocID + " \n");

            }
            varname1.Append("       order by e.Unit ");



            varname1.Append(" SELECT distinct \n");
            varname1.Append("       (SELECT TOP 1 fDesc \n");
            varname1.Append("        FROM   tblwork \n");
            varname1.Append("        WHERE  ID = d.fWork)                              AS dwork \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            else
            {
                varname1.Append("union \n");
                varname1.Append("  \n");
                varname1.Append("SELECT DISTINCT (SELECT TOP 1 fDesc \n");
                varname1.Append("                 FROM   tblwork \n");
                varname1.Append("                 WHERE  ID = o.fWork) AS dwork \n");
                varname1.Append("FROM   TicketO o \n");
                varname1.Append("WHERE  WorkOrder ='" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND o.lid =" + objPropMapData.LocID + " \n");
            }
            varname1.Append("       order by dwork ");


            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetTicketbyLocation(MapData objPropMapData)
        {
            System.Text.StringBuilder varname1 = new System.Text.StringBuilder();
            varname1.Append("SELECT d.ID as ID, \n");
            varname1.Append("     d.workorder, \n");
            varname1.Append("      isnull(tblwork.fDesc,'')  AS dwork, \n");
            varname1.Append("       d.fWork as fWork  \n");
            //  varname1.Append("       Case d.Status  when 0 then 'Un-Assigned' when 1 then 'Assigned' when 2 then 'Enroute' when 3 then 'Onsite' when 4 then 'Completed' when 5 then 'Hold' end as Status, \n");         
            varname1.Append("FROM   TicketD d left join tblwork  on tblwork.ID= d.fWork \n");
            varname1.Append("WHERE  d.loc = '" + objPropMapData.LocID + "' \n");

            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            else
            {
                varname1.Append("union ALL \n");
                varname1.Append("  \n");
                varname1.Append(" SELECT o.ID, \n");

                varname1.Append("     o.WorkOrder, \n");
                varname1.Append("       tblwork.fDesc  AS dwork, \n");

                varname1.Append("       o.fWork \n");
                // varname1.Append("       Case o.Status  when 0 then 'Un-Assigned' when 1 then 'Assigned' when 2 then 'Enroute' when 3 then 'Onsite' when 4 then 'Completed' when 5 then 'Hold' end as Status, \n");
                varname1.Append("FROM   TicketO o LEFT OUTER JOIN TicketDPDA dp ON dp.ID=o.ID left join tblwork  on tblwork.ID= o.fWork \n");
                varname1.Append("WHERE  o.LID = '" + objPropMapData.LocID + "' \n");

            }




            //varname1.Append(" SELECT distinct e.Unit, \n");
            //varname1.Append("       e.Type, \n");
            //varname1.Append("       e.Manuf, \n");
            //varname1.Append("       e.Serial, \n");
            //varname1.Append("       e.State \n");
            //varname1.Append("FROM   TicketD d \n");
            //varname1.Append("       INNER JOIN Elev e \n");
            //varname1.Append("               ON e.ID = d.Elev ");
            //varname1.Append("WHERE  d.loc = '" + objPropMapData.LocID + "' \n");            
            //if (objPropMapData.CustID != 0)
            //{
            //    varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
            //    varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
            //    varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            //}
            //varname1.Append("union SELECT DISTINCT e.Unit, \n");
            //varname1.Append("                e.Type, \n");
            //varname1.Append("                e.Manuf, \n");
            //varname1.Append("                e.Serial, \n");
            //varname1.Append("                e.State \n");
            //varname1.Append("FROM   TicketD d \n");
            //varname1.Append("       INNER JOIN multiple_equipments me \n");
            //varname1.Append("               ON d.ID = me.ticket_id \n");
            //varname1.Append("       INNER JOIN elev e \n");
            //varname1.Append("               ON e.id = me.elev_id ");
            //varname1.Append("WHERE  d.loc = '" + objPropMapData.LocID + "' \n");            
            //if (objPropMapData.CustID != 0)
            //{
            //    varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
            //    varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
            //    varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            //}
            //else
            //{
            //    varname1.Append("union \n");
            //    varname1.Append("  \n");
            //    varname1.Append("SELECT DISTINCT e.Unit, \n");
            //    varname1.Append("                e.Type, \n");
            //    varname1.Append("                e.Manuf, \n");
            //    varname1.Append("                e.Serial, \n");
            //    varname1.Append("                e.State \n");
            //    varname1.Append("FROM   TicketO o \n");
            //    varname1.Append("       INNER JOIN Elev e \n");
            //    varname1.Append("               ON e.ID = o.LElev \n");
            //    varname1.Append("WHERE  o.LID = '" + objPropMapData.LocID + "' \n");              

            //    varname1.Append("union SELECT DISTINCT e.Unit, \n");
            //    varname1.Append("                e.Type, \n");
            //    varname1.Append("                e.Manuf, \n");
            //    varname1.Append("                e.Serial, \n");
            //    varname1.Append("                e.State \n");
            //    varname1.Append("FROM   Ticketo d \n");
            //    varname1.Append("       INNER JOIN multiple_equipments me \n");
            //    varname1.Append("               ON d.ID = me.ticket_id \n");
            //    varname1.Append("       INNER JOIN elev e \n");
            //    varname1.Append("               ON e.id = me.elev_id ");
            //    varname1.Append("WHERE  d.LID = '" + objPropMapData.LocID + "' \n");


            //}
            //varname1.Append("       order by e.Unit ");



            //varname1.Append(" SELECT distinct \n");
            //varname1.Append("       (SELECT TOP 1 fDesc \n");
            //varname1.Append("        FROM   tblwork \n");
            //varname1.Append("        WHERE  ID = d.fWork)                              AS dwork \n");
            //varname1.Append("FROM   TicketD d \n");
            //varname1.Append("WHERE  d.loc = '" + objPropMapData.LocID + "' \n");          
            //if (objPropMapData.CustID != 0)
            //{
            //    varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
            //    varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
            //    varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            //}
            //else
            //{
            //    varname1.Append("union \n");
            //    varname1.Append("  \n");
            //    varname1.Append("SELECT DISTINCT (SELECT TOP 1 fDesc \n");
            //    varname1.Append("                 FROM   tblwork \n");
            //    varname1.Append("                 WHERE  ID = o.fWork) AS dwork \n");
            //    varname1.Append("FROM   TicketO o \n");
            //    varname1.Append("WHERE  o.lid ='" + objPropMapData.LocID + "' \n");              
            //}
            //varname1.Append("       order by dwork ");


            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet SearchTicketInLocation(String conn, int Loc, String prefix, int ticketYear)
        {

            try
            {
                return SqlHelper.ExecuteDataset(conn, "spSearchTicketInLocation", Loc, prefix, ticketYear);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet getticketdtlbywday(MapData objPropMapData)
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter
            {
                ParameterName = "@TicketID",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.TicketID
            };


            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "Spgetticketdtlbywday", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractInfo(MapData objPropMapData, Int32 EquipID, string Type)
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter
            {
                ParameterName = "@LocID",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.LocID
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@EquipID",
                SqlDbType = SqlDbType.Int,
                Value = EquipID
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@Type",
                SqlDbType = SqlDbType.NVarChar,
                Value = Type
            };


            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "SpGetContractInfo", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetSalesPerInfo2(string LID, string ConnConfig)
        {
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter
            {
                ParameterName = "@LID",
                SqlDbType = SqlDbType.Int,
                Value = LID
            };

           

            try
            {
                return  (string) SqlHelper.ExecuteScalar(ConnConfig, CommandType.StoredProcedure, "SpGetSalesPerInfo2", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSalesPerInfo(MapData objPropMapData)
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter
            {
                ParameterName = "@LocID",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.LocID
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@TicketID",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.TicketID
            };

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "SpGetSalesPerInfo", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ResetisSendmailtosalesper(MapData objPropMapData)
        {
            SqlParameter[] para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@TicketID",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.TicketID
            };

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "SpResetisSendmailtosalesper", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ResetisSendmailtosalesper2(int opptID , string ConnConfig)
        {
           
            try
            {
                 SqlHelper.ExecuteDataset(ConnConfig, CommandType.Text, "update lead set IsSendMailToSalesPer=1 where iD="+ opptID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketByID(MapData objPropMapData)
        {
            try
            {
                StringBuilder varname3 = new StringBuilder();
                varname3.Append(" SELECT Quan, fDesc FROM POItem where Ticket = " + objPropMapData.TicketID);
                varname3.Append(" SELECT Quan, fDesc FROM TicketI where Ticket = " + objPropMapData.TicketID);

                StringBuilder varname2 = new StringBuilder();
                varname2.Append("SELECT CASE \n");
                varname2.Append("WHEN ( p.id IS NULL ) THEN 0 \n");
                varname2.Append("ELSE 2 \n");
                varname2.Append("END \n");
                varname2.Append("FROM TicketO o WITH(NOLOCK) \n");
                varname2.Append("LEFT OUTER JOIN TicketDPDA p WITH(NOLOCK) \n");
                varname2.Append("ON o.ID = p.ID \n");
                varname2.Append("WHERE  o.ID = " + objPropMapData.TicketID + " \n");
                varname2.Append("UNION \n");
                varname2.Append("SELECT 1 \n");
                varname2.Append("FROM Ticketd WITH(NOLOCK) \n");
                varname2.Append("WHERE  ID = " + objPropMapData.TicketID);

                int ISTicketD = 0;
                string strComp = Convert.ToString(SqlHelper.ExecuteScalar(objPropMapData.ConnConfig, CommandType.Text, varname2.ToString()));
                if (!string.IsNullOrEmpty(strComp))
                    ISTicketD = Convert.ToInt16(strComp);

                #region OpenTicket
                if (ISTicketD == 0)
                {
                    StringBuilder varname1 = new StringBuilder();
                    varname1.Append("SELECT 'ticketo' tablename , t.*, \n");
                    varname1.Append("dbo.TicketEquips(t.ID) as unit , \n");
                    varname1.Append("(SELECT unit \n");
                    varname1.Append("FROM   elev \n");
                    varname1.Append("WHERE  id = t.lelev)      AS unitname, \n");
                    varname1.Append("       (SELECT state \n");
                    varname1.Append("        FROM   elev \n");
                    varname1.Append("        WHERE  id = t.lelev)               AS unitstate, \n");
                    varname1.Append("       0                                   AS ClearCheck1, \n");
                    varname1.Append("       0                                   AS ClearPR, \n");
                    //varname1.Append("       0                                   AS Charge, \n");
                    varname1.Append("       0.00                                AS Reg, \n");
                    varname1.Append("       0                                   AS OT, \n");
                    varname1.Append("       0                                   AS NT, \n");
                    varname1.Append("       0                                   AS DT, \n");
                    varname1.Append("       0                                   AS TT, \n");
                    varname1.Append("       0                                   AS BT, \n");
                    varname1.Append("       '' as Comments, \n");
                    varname1.Append("       '' as PartsUsed, \n");
                    varname1.Append("       0                                   AS Total, \n");
                    varname1.Append("       Upper(t.DWork)                      AS dworkup, \n");
                    varname1.Append("       (SELECT Super \n");
                    varname1.Append("        FROM   tblWork w \n");
                    varname1.Append("        WHERE  w.fdesc = t.dwork)          AS superv, \n");
                    varname1.Append("       (SELECT TOP 1 signature \n");
                    varname1.Append("        FROM   pdaticketsignature \n");
                    varname1.Append("        WHERE  pdaticketid = t.ID)         AS signature, \n");
                    varname1.Append("       0                                   AS tottime, \n");
                    varname1.Append("       0                                   AS Reg, \n");
                    varname1.Append("       0                                   AS NT, \n");
                    varname1.Append("       0                                   AS OT, \n");
                    varname1.Append("       0                                   AS TT, \n");
                    varname1.Append("       0                                   AS DT, \n");
                    varname1.Append("       t.LDesc2                            AS locname, \n");
                    varname1.Append("       t.LID                               AS LID, \n");
                    varname1.Append("       l.Type                              AS LocType, \n");
                    varname1.Append("       l.Custom1                           AS LocCustom1, \n");
                    varname1.Append("       l.Custom2                           AS LocCustom2, \n");
                    varname1.Append("       (SELECT TOP 1 Name \n");
                    varname1.Append("        FROM   rol \n");
                    varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
                    varname1.Append("                     FROM   Owner \n");
                    varname1.Append("                     WHERE  ID = t.Owner)) AS customerName, \n");
                    varname1.Append("       ''                                  AS descres, \n");
                    varname1.Append("       CASE \n");
                    varname1.Append("         WHEN Assigned = 1 THEN 'Assigned' \n");
                    varname1.Append("         WHEN Assigned = 2 THEN 'Enroute' \n");
                    varname1.Append("         WHEN Assigned = 3 THEN 'Onsite' \n");
                    varname1.Append("         WHEN Assigned = 4 THEN 'Completed' \n");
                    varname1.Append("         WHEN Assigned = 5 THEN 'Hold' \n");
                    varname1.Append("       END                                 AS assignname, \n");
                    varname1.Append("       t.bremarks, \n");
                    varname1.Append("       ( ldesc3 + ' ' + ldesc4 )           AS address, \n");
                    varname1.Append("       l.Address as ldesc3, \n");
                    varname1.Append("       l.City, \n");
                    varname1.Append("       l.State, \n");
                    varname1.Append("       l.Zip, \n");
                    varname1.Append("       1                                   AS workcmpl, \n");
                    varname1.Append("       0                                   AS othere, \n");
                    varname1.Append("       0                                   AS toll, \n");
                    varname1.Append("       0                                   AS zone, \n");
                    varname1.Append("       0                                   AS Smile, \n");
                    varname1.Append("       0                                   AS emile, \n");
                    varname1.Append("       0                                   AS internet, \n");
                    varname1.Append("       0                                   AS invoice, \n");
                    varname1.Append("       ''                                  AS manualinvoice, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.contact \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS contact, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Phone \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Phone, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Cellular \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Cellular, \n");
                    varname1.Append("      (SELECT l.Remarks \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         AS Remarks, \n");
                    varname1.Append("       ''                                  AS QBinvoiceID, \n");
                    varname1.Append("       0                                   AS timetransfer, \n");
                    varname1.Append("       ''                                   AS QBServiceItem, \n");
                    varname1.Append("       ''                                   AS QBPayrollItem, \n");
                    varname1.Append("       isnull(t.high,0) as highdecline, \n");
                    varname1.Append("       isnull(Customtick3,0)AS Customticket3, \n");
                    varname1.Append("       isnull(Customtick4,0)AS Customticket4, \n");
                    varname1.Append("       (select top 1 sageid from owner where id = t.owner) as sagecust,");
                    varname1.Append("       (select top 1 id from loc where loc = t.lid) as sageloc,");
                    varname1.Append("       0 as wagec,0 as Phase, (select top 1 type from jobtype where ID = t.type) as department, ");
                    varname1.Append("       (select convert(varchar(20),t.job )+'-'+ fdesc from job where ID = t.job) as jobdesc,(select fDesc from job where ID = t.job) as Reconfdesc, ");
                    varname1.Append("       jobitemdesc as jobitemdesc1");
                    varname1.Append(" , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime");
                    varname1.Append(" ,    (select  charge from job where ID = t.job) as isJobChargeable ,(select SRemarks from job where ID = t.job) as SpecialRemark, t.JobCode as JobCode1 ");

                    varname1.Append(" FROM   TicketO t WITH(NOLOCK) INNER JOIN Loc l ON l.Loc = t.LID where t.ID =" + objPropMapData.TicketID);

                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString() + varname3.ToString());

                }
                #endregion OpenTicket

                #region CloseTicket
                else if (ISTicketD == 1)
                {
                    #region TicketD
                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text,
                        "select 'TicketD' tablename, t.*,  (select  charge from job where ID = t.job) as isJobChargeable ,dbo.TicketEquips(t.ID) as unit,(select unit from elev where id =t.elev)as unitname,(select state from elev where id =t.elev)as unitstate,isnull(ClearCheck ,0 ) as ClearCheck1 , isnull(ClearPR ,0 )  ClearPR ," +
                        " Isnull(t.Reg,0) as Reg, Isnull(t.OT,0) as OT,Isnull(t.NT,0) as NT ,Isnull(t.DT,0) as DT ,Isnull(t.TT,0) as TT,Isnull(t.break_time,0)  as BT  ,t.Comments as Comments, t.PartsUsed as PartsUsed,  t.Total,UPPER(w.fDesc)as dworkup, w.super as superv,(select top 1 signature from pdaticketsignature where pdaticketid=t.ID)as signature,(reg + NT + OT + TT + DT)as tottime, (select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=l.Owner)) as customerName,l.tag as locname,l.Status as locStatus, l.Type AS LocType, l.Custom1 AS LocCustom1, l.Custom2 AS LocCustom2, l.Owner,l.Loc as lid, l.Address as ldesc3,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as ldesc4,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as Address, cat, l.City, l.State, l.Zip, Elev as lelev, (select top 1 Phone from rol where ID=l.Rol) as phone, CPhone,4 as assigned,UPPER( w.fDesc )as dwork, descres, 'Completed' as assignname,bremarks, isnull( t.workcomplete,0) as workcmpl, isnull(invoice,0) as invoice, manualinvoice,r.contact , r.phone, r.cellular,r.remarks, isnull( QBinvoiceID,'') as QBinvoiceID, isnull( transfertime,0) as timetransfer , isnull(Customtick3,0)AS Customticket3, isnull(Customtick4,0)AS Customticket4, 0 as highdecline ,  (select top 1 sageid from owner where id = (select top 1 owner from loc where loc = t.loc)) as sagecust, (select top 1 id from loc where loc = t.loc) as sageloc, (select top 1 type from jobtype where ID = t.type) as department ,(select convert(varchar(20),t.job ) +'-'+ fdesc from job where ID = t.job) as jobdesc,(select fDesc from job where ID = t.job) as Reconfdesc,(select SRemarks from job where ID = t.job) as SpecialRemark,jobitemdesc as jobitemdesc1, dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, 0 as EmailNotified, null as EmailTime   , t.JobCode as JobCode1" +
                        " from TicketD t WITH(NOLOCK) " +
                        " inner join Loc l WITH(NOLOCK) on l.Loc=t.Loc " +
                        " left outer join tblWork w on t.fWork=w.ID " +
                        " inner join rol r WITH(NOLOCK) on r.id=l.rol " +
                        " where t.ID =" + objPropMapData.TicketID + "  " + varname3.ToString());
                    #endregion TicketD
                }
                else
                {
                    #region TicketDPDA

                    DataSet ds = new DataSet();
                    int Workid = 0;
                    ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select fWork from TicketO where ID=" + objPropMapData.TicketID);
                    Workid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    string PDA = (SqlHelper.ExecuteScalar(objPropMapData.ConnConfig, CommandType.Text, "IF exists(select 1 from sysobjects where name =  'PDA_" + Workid + "')    BEGIN  select 1 PDA  END  else  BEGIN	  select 0 PDA  END")).ToString();



                    StringBuilder varname1 = new StringBuilder();

                    varname1.Append("SELECT 'TicketDPDA' tablename,  dp.Recommendations as bremarks,");
                    varname1.Append("       Isnull(dp.Charge, 0)                   AS charge,   t.*, \n");
                    varname1.Append("       dbo.TicketEquips(t.ID) as unit , \n");
                    varname1.Append("       (SELECT unit \n");
                    varname1.Append("        FROM   elev \n");
                    varname1.Append("        WHERE  id = t.lelev)               AS unitname, \n");
                    varname1.Append("       (SELECT state \n");
                    varname1.Append("        FROM   elev \n");
                    varname1.Append("        WHERE  id = t.lelev)               AS unitstate,  \n");
                    varname1.Append("       0                                   AS ClearCheck1, \n");
                    varname1.Append("       0                                   AS ClearPR,      \n");
                    //varname1.Append("       Isnull(t.Charge, 0)                   AS ClearPR, \n");
                    varname1.Append("       Isnull(dp.Reg,0) as Reg, \n");
                    varname1.Append("       Isnull(dp.OT,0) as OT, \n");
                    varname1.Append("       Isnull(dp.NT,0) as NT, \n");
                    varname1.Append("       Isnull(dp.DT,0) as DT, \n");
                    varname1.Append("       Isnull(dp.TT,0) as TT, \n");
                    varname1.Append("       Isnull(dp.break_time,0) as BT, \n");
                    varname1.Append("       dp.Comments as Comments, \n");
                    varname1.Append("       dp.PartsUsed as PartsUsed, \n");
                    varname1.Append("       dp.Total, \n");
                    varname1.Append("       Upper(DWork)                        AS dworkup, \n");
                    varname1.Append("       (SELECT Super \n");
                    varname1.Append("        FROM   tblWork w \n");
                    varname1.Append("        WHERE  w.fdesc = DWork)            AS superv, \n");
                    if (PDA == "1") { varname1.Append("   (SELECT TOP 1 signature   FROM   PDA_" + Workid + "     WHERE  pdaticketid = t.ID)         AS signature, \n"); }
                    else { varname1.Append("   (SELECT TOP 1 signature   FROM   pdaticketsignature     WHERE  pdaticketid = t.ID)         AS signature, \n"); }
                    varname1.Append("       dp.Total                            AS tottime, \n");
                    varname1.Append("       0                                   AS Reg, \n");
                    varname1.Append("       0                                   AS NT, \n");
                    varname1.Append("       0                                   AS OT, \n");
                    varname1.Append("       0                                   AS TT, \n");
                    varname1.Append("       0                                   AS DT, \n");
                    varname1.Append("       t.LDesc2                            AS locname, \n");
                    varname1.Append("       t.LID                               AS LID, \n");
                    varname1.Append("       l.Type                              AS LocType, \n");
                    varname1.Append("       l.Custom1                           AS LocCustom1, \n");
                    varname1.Append("       l.Custom2                           AS LocCustom2, \n");
                    varname1.Append("       (SELECT TOP 1 NAME \n");
                    varname1.Append("        FROM   rol \n");
                    varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
                    varname1.Append("                     FROM   Owner \n");
                    varname1.Append("                     WHERE  ID = t.Owner)) AS customerName, \n");
                    varname1.Append("       dp.descres, \n");
                    varname1.Append("       CASE \n");
                    varname1.Append("         WHEN Assigned = 1 THEN 'Assigned' \n");
                    varname1.Append("         WHEN Assigned = 2 THEN 'Enroute' \n");
                    varname1.Append("         WHEN Assigned = 3 THEN 'Onsite' \n");
                    varname1.Append("         WHEN Assigned = 4 THEN 'Completed' \n");
                    varname1.Append("         WHEN Assigned = 5 THEN 'Hold' \n");
                    varname1.Append("       END                                 AS assignname, \n");
                    //varname1.Append("       dp.Recommendations as bremarks, \n");
                    varname1.Append("       ( ldesc3 + ' ' + ldesc4 )           AS address, \n");
                    varname1.Append("       l.Address as ldesc3, \n");
                    varname1.Append("       l.City, \n");
                    varname1.Append("       l.State, \n");
                    varname1.Append("       l.Zip, \n");
                    varname1.Append("       Isnull(dp.workcomplete, 0)          AS workcmpl, \n");
                    varname1.Append("       dp.othere, \n");
                    varname1.Append("       dp.toll, \n");
                    varname1.Append("       dp.zone, \n");
                    varname1.Append("       dp.Smile, \n");
                    varname1.Append("       dp.emile, \n");
                    varname1.Append("       dp.internet, \n");
                    varname1.Append("       dp.TimeCheckOut, \n");
                    varname1.Append("       dp.Invoice                                   AS invoice, \n");
                    varname1.Append("       ''                                  AS manualinvoice, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.contact \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS contact, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Phone \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Phone, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Cellular \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Cellular, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Remarks \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT l.Remarks \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Remarks, \n");
                    varname1.Append("       ''                                  AS QBinvoiceID, \n");
                    varname1.Append("       0                                   AS timetransfer, \n");
                    varname1.Append("       ''                                  AS QBServiceItem, \n");
                    varname1.Append("       ''                                  AS QBPayrollItem, \n");
                    varname1.Append("       0                                  AS highdecline, \n");
                    varname1.Append("       Isnull(Customtick3, 0)              AS Customticket3, \n");
                    varname1.Append("       Isnull(Customtick4, 0)              AS Customticket4, \n");
                    varname1.Append("       (select top 1 sageid from owner where id = t.owner) as sagecust,  \n");
                    varname1.Append("       (select top 1 id from loc where loc = t.lid) as sageloc,");
                    varname1.Append("       dp.wagec, isnull(dp.Phase,(select top 1  line  from JobTItem where Job=t.job and Type=1 and fDesc='Labor' and job <> 0)) Phase, (select top 1 type from jobtype where ID = t.type) as department, (select convert(varchar(20),t.job ) +'-'+ fdesc from job where ID = t.job) as jobdesc, (select fDesc from job where ID = t.job) as Reconfdesc,");

                    varname1.Append("         case isnull(t.jobitemdesc, '''') when   '''' then isnull(t.jobitemdesc, (select top 1  fDesc  from JobTItem where Job = t.job and Type = 1 and fDesc = 'Labor' and job <> 0))          else t.jobitemdesc end  jobitemdesc1  \n");


                    varname1.Append(" , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime");
                    varname1.Append(" ,    (select  charge from job where ID = t.job) as isJobChargeable  ,isnull(t.jobCode,(select top 1  Code  from JobTItem where Job=t.job and Type=1 and fDesc='Labor' and job <>0)) JobCode1 ");

                    varname1.Append(" FROM   TicketO t WITH(NOLOCK) \n");
                    varname1.Append("       INNER JOIN TicketDPDA dp WITH(NOLOCK) ON dp.ID = t.ID \n");
                    varname1.Append("       left JOIN Loc l WITH(NOLOCK) ON l.Loc = t.LID \n");
                    varname1.Append("WHERE  t.ID = " + objPropMapData.TicketID);

                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString() + varname3.ToString());
                    #endregion TicketDPDA

                }
                #endregion CloseTicket
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTicketdetailsReport(MapData objPropMapData)
        {
            SqlParameter para = new SqlParameter
            {
                ParameterName = "@Tickets",
                SqlDbType = SqlDbType.Structured,
                Value = objPropMapData.dtTickets
            };

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spGetTicketDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetChargeableTickets(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetChargeableTickets");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetChargeableTicketsMapping(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetChargeableTicketsMapping");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetopportunityTicket(MapData objPropMapData)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropMapData.ConnConfig, CommandType.Text, "select top 1 ID from Lead where TicketID=" + objPropMapData.TicketID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBInvoiceTicketID(MapData objPropMapData)
        {
            string strQuery = " update ticketd set QBinvoiceID='" + objPropMapData.QBInvoiceID + "' where ID='" + objPropMapData.TicketID + "'";

            //strQuery += " update ticketdpda set QBinvoiceID='" + objPropMapData.QBInvoiceID + "' where ID='" + objPropMapData.TicketID + "'";

            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBTimeTxnIDTicket(MapData objPropMapData)
        {
            string strQuery = " update ticketd set qbtimetxnid = '" + objPropMapData.QBInvoiceID + "'";
            strQuery += " where ID='" + objPropMapData.TicketID + "'";

            strQuery += " Insert into tblQBtimesheetticket (ticketid,time,qbtimetxnid) values (" + objPropMapData.TicketID + ",'" + objPropMapData.Custom1 + "','" + objPropMapData.QBInvoiceID + "' )";

            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurringTickets(MapData objPropMapData)
        {
            string str = "SELECT  case when exists ( select 1 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp, (select Unit from elev where id=t.LElev)as unit,  dwork, t.ID, (SELECT TOP 1 name FROM rol WHERE id = (SELECT TOP 1 rol FROM owner WHERE id = t.owner)) AS customername,t.LDesc1 as locid, t.ldesc2 AS locname, t.ldesc4 AS address, phone, t.Cat, t.EDate, t.CDate,  (select top 1 descres from TicketDPDA where ID=t.ID) AS descres, CASE WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, isnull( (select top 1 isnull(Total,0.00) from TicketDPDA where ID=t.ID),0.00)as Tottime FROM ticketo t left outer join TicketDPDA dp on t.ID=dp.ID left outer join tblWork w on w.fDesc=t.DWork  WHERE assigned NOT IN (0) and t.Level=10 ";

            if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
            {
                str += " and w.fdesc='" + objPropMapData.Worker + "'";
            }
            if (objPropMapData.LocTag != "")
            {
                str += " and ldesc2 like '" + objPropMapData.LocTag + "%'";
            }
            if (objPropMapData.CustomerName != "")
            {
                str += " and (SELECT TOP 1 name FROM rol WHERE id = (SELECT TOP 1 rol FROM owner WHERE id = t.owner)) like '" + objPropMapData.CustomerName + "%'";
            }

            str += " order by id desc";

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddFile(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, "Spadddocument", objPropMapData.Screen, objPropMapData.TicketID, objPropMapData.FileName
                    , objPropMapData.FilePath, objPropMapData.DocTypeMIME, objPropMapData.TempId, objPropMapData.Subject, objPropMapData.Body
                    , objPropMapData.Mode, objPropMapData.DocID, objPropMapData.Worker);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddFile(AddFileParam _AddFileParam, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "Spadddocument", _AddFileParam.Screen, _AddFileParam.TicketID, _AddFileParam.FileName
                    , _AddFileParam.FilePath, _AddFileParam.DocTypeMIME, _AddFileParam.TempId, _AddFileParam.Subject, _AddFileParam.Body
                    , _AddFileParam.Mode, _AddFileParam.DocID, _AddFileParam.Worker);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateFile(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, "Spupdatedocument", objPropMapData.Screen, objPropMapData.TicketID, objPropMapData.TempId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateFileMSVisible(MapData objPropMapData, DataTable dt)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@Docs";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = dt;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@UpdatedBy";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = "";
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spUpdateDocInfo", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteFile(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, "spDeleteDocument", objPropMapData.DocumentID, objPropMapData.Worker);
                //SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, "delete from documents where id=" + objPropMapData.DocumentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void DeleteFile(DeleteFileParam _DeleteFileParam, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "spDeleteDocument", _DeleteFileParam.DocumentID, _DeleteFileParam.Worker);
                //SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, "delete from documents where id=" + objPropMapData.DocumentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet SelectTempDocumentFile(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select id,path FROM Documents \n");
            varname1.Append("WHERE  Screen = 'Temp' \n");
            varname1.Append("       AND ScreenID = 0 \n");
            varname1.Append("       AND Dateadd(D, 0, Datediff(D, 0, date)) < Dateadd(D, 0, Datediff(D, 0, Getdate())) ");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocuments(MapData objPropMapData)
        {
            string strQuerytext = "select isnull(MSVisible,0) MSVisible " +
                ", case when isnull(MSVisible,0) = 0 then 'No' else 'Yes' end MSVisibleText  " +
                ", case when isnull(filename,'') <> '' then case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture' else 'Document' end else '' end as doctype " +
                ", d.* " +
                " from documents d where screenid=" + objPropMapData.TicketID + " and screen='" + objPropMapData.Screen + "'";

            if (objPropMapData.TicketID == 0)
            {
                strQuerytext += " and tempid='" + objPropMapData.TempId + "'";
            }

            if (objPropMapData.Mode == 1)
            {
                strQuerytext += " and isnull(filename,'') <> ''";
            }

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, strQuerytext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetDocuments(GetDocumentsParam _GetDocumentsParam, string ConnectionString)
        {
            string strQuerytext = "select d.*, case when isnull(filename,'') <> '' then case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture' else 'Document' end else '' end as doctype  from documents d where screenid=" + _GetDocumentsParam.TicketID + " and screen='" + _GetDocumentsParam.Screen + "'";

            if (_GetDocumentsParam.TicketID == 0)
            {
                strQuerytext += " and tempid='" + _GetDocumentsParam.TempId + "'";
            }

            if (_GetDocumentsParam.Mode == 1)
            {
                strQuerytext += " and isnull(filename,'') <> ''";
            }

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuerytext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationDocuments(MapData objPropMapData, bool isShowAll, bool isLocation)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.TicketID
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@IsShowAll",
                SqlDbType = SqlDbType.Bit,
                Value = isShowAll
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@IsLoc",
                SqlDbType = SqlDbType.Bit,
                Value = isLocation
            };

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spGetLocationDocuments", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectDocuments(MapData objPropMapData, bool isShowAll)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.TicketID
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@IsShowAll",
                SqlDbType = SqlDbType.Bit,
                Value = isShowAll
            };

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spGetProjectDocuments", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetLocationDocuments(GetLocationDocumentsParam _GetLocationDocuments, string ConnectionString, bool isShowAll, bool isLocation)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int,
                Value = _GetLocationDocuments.TicketID
            };

            para[1] = new SqlParameter
            {
                ParameterName = "@IsShowAll",
                SqlDbType = SqlDbType.Bit,
                Value = isShowAll
            };

            para[2] = new SqlParameter
            {
                ParameterName = "@IsLoc",
                SqlDbType = SqlDbType.Bit,
                Value = isLocation
            };

            try
            {
                return _GetLocationDocuments.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetLocationDocuments", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLibrary(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();

            varname1.Append("SELECT d.ID, \n");
            varname1.Append("       Screen, \n");
            varname1.Append("       ScreenID, \n");
            varname1.Append("       Line, \n");
            varname1.Append("       Filename, \n");
            varname1.Append("       Path, \n");
            varname1.Append("       Type, \n");
            varname1.Append("       Remarks, \n");
            varname1.Append("       TempID, \n");
            varname1.Append("       Date, \n");
            varname1.Append("       Subject, \n");
            varname1.Append("       body, \n");
            varname1.Append("       '' AS location \n");
            varname1.Append("FROM   documents d \n");
            varname1.Append("WHERE  screenid = " + objPropMapData.TicketID + " \n");
            varname1.Append("       AND screen = 'customer' and Portal = 1 \n");
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date >='" + objPropMapData.StartDate + "'\n");
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date <'" + objPropMapData.EndDate.AddDays(1) + "'");
            }
            if (objPropMapData.SearchBy == "filename" || objPropMapData.SearchBy == "remarks")
            {
                varname1.Append(" and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'");
            }
            if (objPropMapData.SearchBy == "loc")
            {
                varname1.Append(" and screenid = -1");
            }

            varname1.Append(" UNION \n");

            varname1.Append("SELECT d.ID, \n");
            varname1.Append("       Screen, \n");
            varname1.Append("       ScreenID, \n");
            varname1.Append("       Line, \n");
            varname1.Append("       Filename, \n");
            varname1.Append("       Path, \n");
            varname1.Append("       Type, \n");
            varname1.Append("       Remarks, \n");
            varname1.Append("       TempID, \n");
            varname1.Append("       Date, \n");
            varname1.Append("       Subject, \n");
            varname1.Append("       body, \n");
            varname1.Append("       (SELECT TOP 1 tag \n");
            varname1.Append("        FROM   Loc \n");
            varname1.Append("        WHERE  Loc = ScreenID) \n");
            varname1.Append("FROM   documents d \n");
            varname1.Append("WHERE  (SELECT TOP 1 Owner \n");
            varname1.Append("        FROM   Loc \n");
            varname1.Append("        WHERE  Loc = ScreenID) = " + objPropMapData.TicketID + " \n");
            varname1.Append("       AND screen = 'location' and Portal = 1 ");
            if (objPropMapData.roleid != 0)
            {
                varname1.Append(" and (SELECT TOP 1 Isnull(RoleID,0) \n");
                varname1.Append("        FROM   Loc \n");
                varname1.Append("        WHERE  Loc = ScreenID) = " + objPropMapData.roleid);
            }
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date >='" + objPropMapData.StartDate + "'\n");
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date <'" + objPropMapData.EndDate.AddDays(1) + "'");
            }
            if (objPropMapData.SearchBy == "filename" || objPropMapData.SearchBy == "remarks")
            {
                varname1.Append(" and " + objPropMapData.SearchBy + " like '" + objPropMapData.SearchValue + "%'");
            }
            if (objPropMapData.SearchBy == "loc")
            {
                varname1.Append(" and screenid = " + objPropMapData.SearchValue);
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSignature(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetSignature", objPropMapData.WorkID, objPropMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketSignature(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTicketSignature", objPropMapData.WorkID, objPropMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceTicketByWorkorder(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select ID, total, (othere+toll+zone) as expenses, (emile-Smile) as mileage from TicketD where Charge=1 and WorkOrder=(select top 1 WorkOrder from TicketD where ID=" + objPropMapData.TicketID + ")");//and ID<>" + objPropMapData.TicketID + "
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketsByWorkorder(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       Assigned, \n");
            varname1.Append("       EDate, \n");
            varname1.Append("       LDesc2                              AS locname, \n");
            varname1.Append("       CONVERT(VARCHAR(30), fDesc) AS fdesc, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Assigned = 1 THEN 'Assigned' \n");
            varname1.Append("         WHEN Assigned = 2 THEN 'Enroute' \n");
            varname1.Append("         WHEN Assigned = 3 THEN 'Onsite' \n");
            varname1.Append("         WHEN Assigned = 4 THEN 'Completed' \n");
            varname1.Append("         WHEN Assigned = 5 THEN 'Hold' \n");
            varname1.Append("       END                                 AS assignname, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Assigned = 1 THEN 'White' \n");
            varname1.Append("         WHEN Assigned = 2 THEN '#9EF767' \n");
            varname1.Append("         WHEN Assigned = 3 THEN 'orange' \n");
            varname1.Append("         WHEN Assigned = 4 THEN 'DeepSkyBlue' \n");
            varname1.Append("         WHEN Assigned = 5 THEN 'yellow' \n");
            varname1.Append("       END                                 AS color, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                      FROM   TicketDPDA \n");
            varname1.Append("                      WHERE  ID = t.ID) THEN 2 \n");
            varname1.Append("         ELSE 0 \n");
            varname1.Append("       END                                 AS comp \n");
            varname1.Append("FROM   TicketO t \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND t.ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("UNION all \n");
            varname1.Append("SELECT ID, \n");
            varname1.Append("       4                                   AS Assigned, \n");
            varname1.Append("       EDate, \n");
            varname1.Append("       (SELECT TOP 1 tag \n");
            varname1.Append("        FROM   loc l \n");
            varname1.Append("        WHERE  l.Loc = d.Loc)              AS locname, \n");
            varname1.Append("       CONVERT(VARCHAR(30), fDesc) AS fdesc, \n");
            varname1.Append("       'Completed'                         AS assignname, \n");
            varname1.Append("       'DeepSkyBlue'                       AS color, \n");
            varname1.Append("       1                                   AS comp \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' ");
            varname1.Append("       AND d.ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("       order by id");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecentCallsLoc(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT TOP 5 *, \n");
            varname1.Append("             CASE \n");
            varname1.Append("               WHEN Assigned = 1 THEN 'Assigned' \n");
            varname1.Append("               WHEN Assigned = 2 THEN 'Enroute' \n");
            varname1.Append("               WHEN Assigned = 3 THEN 'Onsite' \n");
            varname1.Append("               WHEN Assigned = 4 THEN 'Completed' \n");
            varname1.Append("               WHEN Assigned = 5 THEN 'Hold' \n");
            varname1.Append("             END AS assignname \n");
            varname1.Append("FROM   (SELECT 1                          AS comp, \n");
            varname1.Append("               4                          AS assigned, \n");
            varname1.Append("               CONVERT(VARCHAR(20), EDate)EDate, \n");
            varname1.Append("               ID, elev, \n");
            varname1.Append("               (SELECT Unit \n");
            varname1.Append("                FROM   Elev \n");
            varname1.Append("                WHERE  ID = Elev)         AS elevname, cat, (select fdesc from tblwork where id = fwork ) as worker \n");

            varname1.Append("        FROM   TicketD \n");
            varname1.Append("        WHERE  Loc = " + objPropMapData.LocID + " and ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("        UNION ALL \n");
            varname1.Append("        SELECT CASE \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketDPDA \n");
            varname1.Append("                              WHERE  ID = d.ID) THEN 2 \n");
            varname1.Append("                 ELSE 0 \n");
            varname1.Append("               END                 AS comp, \n");
            varname1.Append("               Assigned, \n");
            varname1.Append("               EDate, \n");
            varname1.Append("               ID, lelev as elev, \n");
            varname1.Append("               (SELECT Unit \n");
            varname1.Append("                FROM   Elev \n");
            varname1.Append("                WHERE  ID = LElev) AS elevname, cat, (select fdesc from tblwork where id = fwork ) as worker \n");
            varname1.Append("        FROM   TicketO d \n");
            varname1.Append("        WHERE  LID = " + objPropMapData.LocID + "  and ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("               AND Isnull(Owner, 0) <> 0)AS t \n");
            varname1.Append("ORDER  BY EDate DESC ");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketTime(MapData objPropMapData)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT t.ID, \n");
            //varname1.Append("       t.DescRes, \n");
            //varname1.Append("       EDate, \n");
            //varname1.Append("       fUser, \n");
            //varname1.Append("       QBEmployeeID, \n");
            //varname1.Append("       QBTimeTxnID, \n");
            //varname1.Append("       t.LastUpdateDate, \n");
            //varname1.Append("       reg, \n");
            //varname1.Append("       ot, \n");
            //varname1.Append("       dt, \n");
            //varname1.Append("       tt, \n");
            //varname1.Append("       nt, \n");
            //varname1.Append("       Total, \n");
            //varname1.Append("       (SELECT qblocid \n");
            //varname1.Append("        FROM   loc \n");
            //varname1.Append("        WHERE  loc = t.loc) AS QBcustID, \n");
            //varname1.Append("       CASE \n");
            //varname1.Append("         WHEN Isnull(QBPayrollItem, '') = '' THEN (SELECT TOP 1 qbwageid \n");
            //varname1.Append("                                                   FROM   prwage \n");
            //varname1.Append("                                                   WHERE  fdesc = 'Mobile Service Manager') \n");
            //varname1.Append("         ELSE qbpayrollitem \n");
            //varname1.Append("       END                  AS qbwageid, \n");
            //varname1.Append("       CASE \n");
            //varname1.Append("         WHEN Isnull(QBServiceItem, '') = '' THEN (SELECT QBInvID \n");
            //varname1.Append("                                                   FROM   Inv \n");
            //varname1.Append("                                                   WHERE  Name = 'time spent') \n");
            //varname1.Append("         ELSE QBServiceItem \n");
            //varname1.Append("       END                  AS QBitemID \n");
            //varname1.Append("FROM   TicketD t \n");
            //varname1.Append("       INNER JOIN tblWork w \n");
            //varname1.Append("               ON w.ID = t.fWork \n");
            //varname1.Append("       INNER JOIN tblUser u \n");
            //varname1.Append("               ON u.fUser = w.fDesc \n");
            //varname1.Append("WHERE  Isnull(clearcheck, 0) = 1 \n");
            //varname1.Append("       AND Isnull(TransferTime, 0) = 1 ");

            //if (objPropMapData.SearchValue == "1")
            //{
            //    varname1.Append(" and  QBTimeTxnID not NULL and t.LastUpdateDate >= (select QBLastSync from Control) ");
            //}
            //else
            //{
            //    varname1.Append(" and  QBTimeTxnID IS NULL and t.LastUpdateDate >= (select QBLastSync from Control)");
            //}

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "GetTicketTime");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketTimeMapping(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT t.ID, \n");
            varname1.Append("       t.DescRes, \n");
            varname1.Append("       EDate, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       QBEmployeeID, \n");
            varname1.Append("       QBTimeTxnID, \n");
            varname1.Append("       t.LastUpdateDate, \n");
            varname1.Append("       reg, \n");
            varname1.Append("       ot, \n");
            varname1.Append("       dt, \n");
            varname1.Append("       tt, \n");
            varname1.Append("       nt, \n");
            varname1.Append("       Total, \n");
            varname1.Append("       (SELECT qblocid \n");
            varname1.Append("        FROM   loc \n");
            varname1.Append("        WHERE  loc = t.loc) AS QBcustID, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(QBPayrollItem, '') = '' THEN (SELECT TOP 1 qbwageid \n");
            varname1.Append("                                                   FROM   prwage \n");
            varname1.Append("                                                   WHERE  fdesc = 'Mobile Service Manager') \n");
            varname1.Append("         ELSE qbpayrollitem \n");
            varname1.Append("       END                  AS qbwageid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(QBServiceItem, '') = '' THEN (SELECT QBInvID \n");
            varname1.Append("                                                   FROM   Inv \n");
            varname1.Append("                                                   WHERE  Name = 'time spent') \n");
            varname1.Append("         ELSE QBServiceItem \n");
            varname1.Append("       END                  AS QBitemID \n");
            varname1.Append("FROM   TicketD t \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON w.ID = t.fWork \n");
            varname1.Append("       INNER JOIN tblUser u \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  Isnull(clearcheck, 0) = 1 \n");
            varname1.Append("       AND Isnull(TransferTime, 0) = 1 ");

            if (objPropMapData.SearchValue == "1")
            {
                varname1.Append(" and  QBTimeTxnID not NULL and QBEmployeeID is not null and (select top 1 QBLocID from Loc where Loc = t.Loc) is not null and t.LastUpdateDate >= (select QBLastSync from Control) ");
            }
            else
            {
                varname1.Append(" and  QBTimeTxnID IS NULL and QBEmployeeID is not null and (select top 1 QBLocID from Loc where Loc = t.Loc) is not null ");
            }

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void IndexMapdata(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteDataset(Config.MS, CommandType.StoredProcedure, "spIndexMapdata");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevByTicket(MapData objPropMapData)
        {
            string str = "select ticket_id as ticketid, (select top 1 unit from elev where id = me.elev_id) as unit, elev_id, labor_percentage,  (select top 1 serial from elev where id = me.elev_id) as serial, (select top 1 state from elev where id = me.elev_id) as state, (select top 1 owner from elev where id = me.elev_id) as owner from multiple_equipments me";
            str += " where ticket_id=" + objPropMapData.TicketID;
            str += " union select t.id as ticketid, (select top 1 unit from elev where id = t.lelev) as unit, t.lelev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.lelev  and ticket_id=t.id) as labor_percentage ,(select top 1 serial from elev where id = t.lelev) as serial, (select top 1 state from elev where id = t.lelev) as state, (select top 1 owner from elev where id = t.lelev) as owner from ticketo t where t.lelev is not null and  t.lelev <> 0 and  id = " + objPropMapData.TicketID;
            str += " union select t.id as ticketid, (select top 1 unit from elev where id = t.elev) as unit,  t.elev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.elev  and ticket_id=t.id) as labor_percentage, (select top 1 serial from elev where id = t.elev) as serial, (select top 1 state from elev where id = t.elev) as state, (select top 1 owner from elev where id = t.elev) as owner  from ticketd t  where t.elev is not null and t.elev <> 0 and id = " + objPropMapData.TicketID;

            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getElevByTicket(GetElevByTicketParam _GetElevByTicket, string ConnectionString)
        {
            string str = "select ticket_id as ticketid, (select top 1 unit from elev where id = me.elev_id) as unit, elev_id, labor_percentage,  (select top 1 serial from elev where id = me.elev_id) as serial, (select top 1 state from elev where id = me.elev_id) as state, (select top 1 owner from elev where id = me.elev_id) as owner from multiple_equipments me";
            str += " where ticket_id=" + _GetElevByTicket.TicketID;
            str += " union select t.id as ticketid, (select top 1 unit from elev where id = t.lelev) as unit, t.lelev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.lelev  and ticket_id=t.id) as labor_percentage ,(select top 1 serial from elev where id = t.lelev) as serial, (select top 1 state from elev where id = t.lelev) as state, (select top 1 owner from elev where id = t.lelev) as owner from ticketo t where t.lelev is not null and  t.lelev <> 0 and  id = " + _GetElevByTicket.TicketID;
            str += " union select t.id as ticketid, (select top 1 unit from elev where id = t.elev) as unit,  t.elev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.elev  and ticket_id=t.id) as labor_percentage, (select top 1 serial from elev where id = t.elev) as serial, (select top 1 state from elev where id = t.elev) as state, (select top 1 owner from elev where id = t.elev) as owner  from ticketd t  where t.elev is not null and t.elev <> 0 and id = " + _GetElevByTicket.TicketID;

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevByTicketID(MapData objPropMapData)
        {
            string str = "select Ticket_ID,(select top 1 unit from elev where id = me.elev_id) as unit, elev_id, labor_percentage,  (select top 1 serial from elev where id = me.elev_id) as serial, (select top 1 state from elev where id = me.elev_id) as state, (select top 1 owner from elev where id = me.elev_id) as owner from multiple_equipments me";
            str += " where ticket_id=" + objPropMapData.TicketID;
            str += " union select id, (select top 1 unit from elev where id = t.lelev) as unit, t.lelev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.lelev  and ticket_id=t.id) as labor_percentage ,(select top 1 serial from elev where id = t.lelev) as serial, (select top 1 state from elev where id = t.lelev) as state, (select top 1 owner from elev where id = t.lelev) as owner from ticketo t where t.lelev is not null and  t.lelev <> 0 and  id = " + objPropMapData.TicketID;
            str += " union select id, (select top 1 unit from elev where id = t.elev) as unit,  t.elev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.elev  and ticket_id=t.id) as labor_percentage, (select top 1 serial from elev where id = t.elev) as serial, (select top 1 state from elev where id = t.elev) as state, (select top 1 owner from elev where id = t.elev) as owner  from ticketd t  where t.elev is not null and t.elev <> 0 and id = " + objPropMapData.TicketID;

            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getElevByTicketID(GetElevByTicketIDParam _GetElevByTicketID, string ConnectionString)
        {
            string str = "select Ticket_ID,(select top 1 unit from elev where id = me.elev_id) as unit, elev_id, labor_percentage,  (select top 1 serial from elev where id = me.elev_id) as serial, (select top 1 state from elev where id = me.elev_id) as state, (select top 1 owner from elev where id = me.elev_id) as owner from multiple_equipments me";
            str += " where ticket_id=" + _GetElevByTicketID.TicketID;
            str += " union select id, (select top 1 unit from elev where id = t.lelev) as unit, t.lelev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.lelev  and ticket_id=t.id) as labor_percentage ,(select top 1 serial from elev where id = t.lelev) as serial, (select top 1 state from elev where id = t.lelev) as state, (select top 1 owner from elev where id = t.lelev) as owner from ticketo t where t.lelev is not null and  t.lelev <> 0 and  id = " + _GetElevByTicketID.TicketID;
            str += " union select id, (select top 1 unit from elev where id = t.elev) as unit,  t.elev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.elev  and ticket_id=t.id) as labor_percentage, (select top 1 serial from elev where id = t.elev) as serial, (select top 1 state from elev where id = t.elev) as state, (select top 1 owner from elev where id = t.elev) as owner  from ticketd t  where t.elev is not null and t.elev <> 0 and id = " + _GetElevByTicketID.TicketID;

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetElevByTicketIDs(MapData objPropMapData, string ticketIDs)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	ticket_id AS TicketID, \n");
                sb.Append("	elev_id,  \n");
                sb.Append("	labor_percentage,   \n");
                sb.Append("	(SELECT TOP 1 Unit FROM Elev WHERE ID = me.elev_id) AS Unit,  \n");
                sb.Append("	(SELECT TOP 1 Serial FROM Elev WHERE ID = me.elev_id) AS Serial,  \n");
                sb.Append("	(SELECT TOP 1 State FROM elev WHERE ID = me.elev_id) AS State,  \n");
                sb.Append("	(SELECT TOP 1 Owner FROM Elev WHERE ID = me.elev_id) AS Owner  \n");
                sb.Append($"FROM multiple_equipments me WHERE ticket_id IN ({ticketIDs}) \n");
                sb.Append("UNION  \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.ID AS TicketID, \n");
                sb.Append("	t.LElev AS elev_id,  \n");
                sb.Append("	(SELECT TOP 1 labor_percentage FROM multiple_equipments WHERE elev_id =  t.LElev AND ticket_id = t.ID) AS labor_percentage, \n");
                sb.Append("	(SELECT TOP 1 Unit FROM Elev WHERE ID = t.LElev) AS Unit, \n");
                sb.Append("	(SELECT TOP 1 Serial FROM Elev WHERE ID = t.LElev) AS Serial,  \n");
                sb.Append("	(SELECT TOP 1 State FROM Elev WHERE ID = t.LElev) AS State,  \n");
                sb.Append("	(SELECT TOP 1 Owner FROM Elev WHERE ID = t.LElev) AS Owner  \n");
                sb.Append($"FROM Ticketo t WHERE t.LElev IS NOT NULL AND t.LElev <> 0 AND t.ID IN ({ticketIDs}) \n");
                sb.Append("UNION  \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.ID AS TicketID, \n");
                sb.Append("	t.Elev AS elev_id,  \n");
                sb.Append("	(SELECT TOP 1 labor_percentage FROM multiple_equipments WHERE elev_id =  t.Elev AND ticket_id = t.ID) AS labor_percentage,  \n");
                sb.Append("	(SELECT TOP 1 Unit FROM Elev WHERE id = t.Elev) AS Unit,   \n");
                sb.Append("	(SELECT TOP 1 Serial FROM Elev WHERE id = t.Elev) AS Serial,  \n");
                sb.Append("	(SELECT TOP 1 State FROM Elev WHERE id = t.Elev) AS State,  \n");
                sb.Append("	(SELECT TOP 1 Owner FROM Elev WHERE id = t.Elev) AS Owner   \n");
                sb.Append($"FROM TicketD t  WHERE t.Elev IS NOT NULL AND t.Elev <> 0 AND t.ID IN ({ticketIDs}) \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetElevByTicketIDs(GetElevByTicketIDsParam _GetElevByTicketIDs, string ConnectionString, string ticketIDs)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	ticket_id AS TicketID, \n");
                sb.Append("	elev_id,  \n");
                sb.Append("	labor_percentage,   \n");
                sb.Append("	(SELECT TOP 1 Unit FROM Elev WHERE ID = me.elev_id) AS Unit,  \n");
                sb.Append("	(SELECT TOP 1 Serial FROM Elev WHERE ID = me.elev_id) AS Serial,  \n");
                sb.Append("	(SELECT TOP 1 State FROM elev WHERE ID = me.elev_id) AS State,  \n");
                sb.Append("	(SELECT TOP 1 Owner FROM Elev WHERE ID = me.elev_id) AS Owner  \n");
                sb.Append($"FROM multiple_equipments me WHERE ticket_id IN ({ticketIDs}) \n");
                sb.Append("UNION  \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.ID AS TicketID, \n");
                sb.Append("	t.LElev AS elev_id,  \n");
                sb.Append("	(SELECT TOP 1 labor_percentage FROM multiple_equipments WHERE elev_id =  t.LElev AND ticket_id = t.ID) AS labor_percentage, \n");
                sb.Append("	(SELECT TOP 1 Unit FROM Elev WHERE ID = t.LElev) AS Unit, \n");
                sb.Append("	(SELECT TOP 1 Serial FROM Elev WHERE ID = t.LElev) AS Serial,  \n");
                sb.Append("	(SELECT TOP 1 State FROM Elev WHERE ID = t.LElev) AS State,  \n");
                sb.Append("	(SELECT TOP 1 Owner FROM Elev WHERE ID = t.LElev) AS Owner  \n");
                sb.Append($"FROM Ticketo t WHERE t.LElev IS NOT NULL AND t.LElev <> 0 AND t.ID IN ({ticketIDs}) \n");
                sb.Append("UNION  \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.ID AS TicketID, \n");
                sb.Append("	t.Elev AS elev_id,  \n");
                sb.Append("	(SELECT TOP 1 labor_percentage FROM multiple_equipments WHERE elev_id =  t.Elev AND ticket_id = t.ID) AS labor_percentage,  \n");
                sb.Append("	(SELECT TOP 1 Unit FROM Elev WHERE id = t.Elev) AS Unit,   \n");
                sb.Append("	(SELECT TOP 1 Serial FROM Elev WHERE id = t.Elev) AS Serial,  \n");
                sb.Append("	(SELECT TOP 1 State FROM Elev WHERE id = t.Elev) AS State,  \n");
                sb.Append("	(SELECT TOP 1 Owner FROM Elev WHERE id = t.Elev) AS Owner   \n");
                sb.Append($"FROM TicketD t  WHERE t.Elev IS NOT NULL AND t.Elev <> 0 AND t.ID IN ({ticketIDs}) \n");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketItemByIDs(MapData objPropMapData, string ticketIDs)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"SELECT Ticket AS TicketID, Quan, fDesc FROM POItem WHERE Ticket IN ({ticketIDs}) \n");
                sb.Append($"SELECT Ticket AS TicketID, Quan, fDesc FROM TicketI WHERE Ticket IN ({ticketIDs}) \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEmailNotificationStatus(MapData objPropMapData)
        {
            string str = "update ticketo set EmailNotified = 1, EmailTime=getdate() where ID = " + objPropMapData.TicketID;
            try
            {
                SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTicketTaskByTicketID(MapData objPropMapData)
        {
            string str = "select id, ticket_id, task_code from Ticket_Task_Codes ";
            str += " where ticket_id=" + objPropMapData.TicketID;
            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMultipleTicket(MapData objMapData)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[18];


                para[0] = new SqlParameter() { ParameterName = "@Equipments", SqlDbType = SqlDbType.Structured, Value = objMapData.dtEquips, TypeName = "tblTypeMultipleEequipments" };
                para[1] = new SqlParameter() { ParameterName = "@Workers", SqlDbType = SqlDbType.NVarChar, Value = objMapData.Worker };
                para[2] = new SqlParameter() { ParameterName = "@days", SqlDbType = SqlDbType.Int, Value = objMapData.days };
                para[3] = new SqlParameter() { ParameterName = "@ProjectID", SqlDbType = SqlDbType.Int, Value = objMapData.jobid };
                para[4] = new SqlParameter() { ParameterName = "@CallDt", SqlDbType = SqlDbType.DateTime, Value = objMapData.CallDate };
                para[5] = new SqlParameter() { ParameterName = "@SchDt", SqlDbType = SqlDbType.DateTime, Value = objMapData.SchDate };
                para[6] = new SqlParameter() { ParameterName = "@cat", SqlDbType = SqlDbType.NVarChar, Value = objMapData.Category };
                para[7] = new SqlParameter() { ParameterName = "@Reason", SqlDbType = SqlDbType.NVarChar, Value = objMapData.Reason };
                para[8] = new SqlParameter() { ParameterName = "@Caller", SqlDbType = SqlDbType.NVarChar, Value = objMapData.Who };
                para[9] = new SqlParameter() { ParameterName = "@Status", SqlDbType = SqlDbType.Int, Value = objMapData.Status };
                para[10] = new SqlParameter() { ParameterName = "@EST", SqlDbType = SqlDbType.Decimal, Value = objMapData.EST };
                para[11] = new SqlParameter() { ParameterName = "@DispAlert", SqlDbType = SqlDbType.SmallInt, Value = objMapData.DispAlert };
                para[12] = new SqlParameter() { ParameterName = "@CreditReason", SqlDbType = SqlDbType.NVarChar, Value = objMapData.CreditReason };
                para[13] = new SqlParameter() { ParameterName = "@Unit", SqlDbType = SqlDbType.Int, Value = objMapData.Unit };
                para[14] = new SqlParameter() { ParameterName = "@LocID", SqlDbType = SqlDbType.Int, Value = objMapData.LocID };
                para[15] = new SqlParameter() { ParameterName = "@Department", SqlDbType = SqlDbType.Int, Value = objMapData.Department };
                para[16] = new SqlParameter() { ParameterName = "@WorkOrder", SqlDbType = SqlDbType.NVarChar, Value = objMapData.Workorder };
                para[17] = new SqlParameter() { ParameterName = "@FBy", SqlDbType = SqlDbType.NVarChar, Value = objMapData.fBy };
                SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spAddMultipleTicket", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddticketfrmCustPortal(MapData objMapData)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[15];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@Equipments";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objMapData.dtEquips;
                para[0].TypeName = "tblTypeMultipleEequipments";

                para[1] = new SqlParameter();
                para[1].ParameterName = "@Unit";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objMapData.Unit;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@Worker";
                para[2].SqlDbType = SqlDbType.NVarChar;
                para[2].Value = objMapData.Worker;


                para[3] = new SqlParameter();
                para[3].ParameterName = "@CallDt";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = objMapData.CallDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@SchDt";
                para[4].SqlDbType = SqlDbType.DateTime;
                para[4].Value = objMapData.SchDate;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@cat";
                para[5].SqlDbType = SqlDbType.NVarChar;
                para[5].Value = objMapData.Category;

                para[6] = new SqlParameter();
                para[6].ParameterName = "@Reason";
                para[6].SqlDbType = SqlDbType.NVarChar;
                para[6].Value = objMapData.Reason;


                para[7] = new SqlParameter();
                para[7].ParameterName = "@Caller";
                para[7].SqlDbType = SqlDbType.NVarChar;
                para[7].Value = objMapData.Who;

                para[8] = new SqlParameter();
                para[8].ParameterName = "@cellerphone";
                para[8].SqlDbType = SqlDbType.NVarChar;
                para[8].Value = objMapData.Cell;


                para[9] = new SqlParameter();
                para[9].ParameterName = "@EST";
                para[9].SqlDbType = SqlDbType.Decimal;
                para[9].Value = objMapData.EST;

                para[10] = new SqlParameter();
                para[10].ParameterName = "@LocID";
                para[10].SqlDbType = SqlDbType.Int;
                para[10].Value = objMapData.LocID;

                para[11] = new SqlParameter();
                para[11].ParameterName = "@fby";
                para[11].SqlDbType = SqlDbType.NVarChar;
                para[11].Value = objMapData.fBy;

                para[12] = new SqlParameter();
                para[12].ParameterName = "@Level";
                para[12].SqlDbType = SqlDbType.NVarChar;
                para[12].Value = objMapData.Level;

                objMapData.TicketID = Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, CommandType.StoredProcedure, "SpAddticketfrmCustPortal", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddTicketFromProject(MapData objMapData)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[15];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@Equipments";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = objMapData.dtEquips;
                para[0].TypeName = "tblTypeMultipleEequipments";

                para[1] = new SqlParameter();
                para[1].ParameterName = "@Workers";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = objMapData.Worker;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@days";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objMapData.days;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@ProjectID";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = objMapData.jobid;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@CallDt";
                para[4].SqlDbType = SqlDbType.DateTime;
                para[4].Value = objMapData.CallDate;

                para[5] = new SqlParameter();
                para[5].ParameterName = "@SchDt";
                para[5].SqlDbType = SqlDbType.DateTime;
                para[5].Value = objMapData.SchDate;

                para[6] = new SqlParameter();
                para[6].ParameterName = "@cat";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = objMapData.Category;

                para[7] = new SqlParameter();
                para[7].ParameterName = "@Reason";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = objMapData.Reason;

                para[8] = new SqlParameter();
                para[8].ParameterName = "@Caller";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = objMapData.Who;

                para[9] = new SqlParameter();
                para[9].ParameterName = "@Status";
                para[9].SqlDbType = SqlDbType.Int;
                para[9].Value = objMapData.Status;


                para[10] = new SqlParameter();
                para[10].ParameterName = "@EST";
                para[10].SqlDbType = SqlDbType.Decimal;
                para[10].Value = objMapData.EST;


                para[11] = new SqlParameter();
                para[11].ParameterName = "@DispAlert";
                para[11].SqlDbType = SqlDbType.SmallInt;
                para[11].Value = objMapData.DispAlert;


                para[12] = new SqlParameter();
                para[12].ParameterName = "@CreditReason";
                para[12].SqlDbType = SqlDbType.Text;
                para[12].Value = objMapData.CreditReason;


                para[13] = new SqlParameter();
                para[13].ParameterName = "@Unit";
                para[13].SqlDbType = SqlDbType.Int;
                para[13].Value = objMapData.Unit;



                string TicketID = (SqlHelper.ExecuteScalar(objMapData.ConnConfig, CommandType.StoredProcedure, "spAddProjectTicket", para)).ToString();

                return TicketID;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Rahil
        public string getTicketListSignature(MapData objPropMapData)
        {
            string str = "";

            //if (objPropMapData.Mobile != 1)
            //{
            if (objPropMapData.Status != 1)
            {
                if (objPropMapData.FilterReview != "1")
                {
                    str = "SELECT t.who, t.lid, l.id as locid, assigned, ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT, dp.Total, 0 AS ClearCheck, t.Charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, dwork, dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername, ";
                    str += " l.Tag  AS locname, l.Address  AS address, t.phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime, Isnull(dp.Total, 0.00) - DATEDIFF(HOUR,dp.TimeRoute,dp.TimeComp ) as timediff, t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";
                    str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += ", isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, isnull(t.high,0) as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, (select Name from Route where ID=l.Route ) as defaultworker";
                    str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                    str += ", (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature, l.state ";
                    str += ", (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                    str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join Loc l on l.Loc=t.lid  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol  left outer join Elev e on e.ID=t.LElev WHERE t.id is not null and t.owner is not null "; //assigned NOT IN ( 0 )

                    //if (objPropMapData.IsList != 1)
                    //{
                    //    str += " and assigned NOT IN ( 0 )";
                    //}

                    //if (objPropMapData.Assigned != -1)//&& objPropMapData.Assigned != 0
                    //{
                    //    if (objPropMapData.Assigned == -2)
                    //    {
                    //        str += " and t.Assigned <> 4";
                    //    }
                    //    else
                    //    {
                    //        str += " and t.Assigned=" + objPropMapData.Assigned;
                    //    }
                    //}
                    if (objPropMapData.StartDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                    }
                    if (objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1) + "'";
                    }
                    //if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    //{
                    //    str += " and t.DWork='" + objPropMapData.Worker + "'";
                    //}
                    //if (objPropMapData.LocID != 0)
                    //{
                    //    str += " and t.LID=" + objPropMapData.LocID;
                    //}
                    //if (objPropMapData.CustID != 0)
                    //{
                    //    str += " and t.Owner=" + objPropMapData.CustID;
                    //}
                    //if (objPropMapData.jobid != 0)
                    //{
                    //    str += " and t.job =" + objPropMapData.jobid;
                    //}
                    //if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    //{
                    //    str += " and isnull(charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                    //}
                    //if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    //{
                    //    str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                    //}
                    //if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    //{
                    //    str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                    //}
                    ////if (objPropMapData.FilterReview == "1")
                    ////{
                    ////    str += " and charge =9";                   
                    ////}                
                    //if (objPropMapData.Mobile == 2)
                    //{
                    //    str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=2";
                    //}
                    //if (objPropMapData.Mobile == 1)
                    //{
                    //    str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=0";
                    //}
                    //if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    //{
                    //    str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    //}
                    //if (objPropMapData.Department != -1)
                    //{
                    //    str += " and t.type=" + objPropMapData.Department;
                    //}
                    //if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    //{
                    //    // str += " and t.cat='" + objPropMapData.Category + "'";
                    //    str += " and t.cat in (" + objPropMapData.Category + ")";
                    //}
                    //if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    //{
                    //    if (objPropMapData.Bremarks == "1")
                    //        str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                    //    else
                    //        str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    //}

                    //if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null)
                    //{
                    //    string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                    //    if (SearchBy == "t.ID")
                    //    {
                    //        str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                    //    }
                    //    else
                    //    {
                    //        str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                    //    }
                    //}

                    //if (objPropMapData.InvoiceID != 0)
                    //{
                    //    if (objPropMapData.InvoiceID == 1)
                    //    {
                    //        str += " and isnull(Invoice,0) <> 0";
                    //    }
                    //    else if (objPropMapData.InvoiceID == 2)
                    //    {
                    //        str += " and isnull(Invoice,0) = 0 and isnull(charge,0)= 1";
                    //    }
                    //}

                    //}

                    if (objPropMapData.LocID == 0)
                    {
                        str += " Union all ";
                        str += " SELECT t.who, t.lid, '--' as locid, assigned, ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip ) AS fulladdress, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT, dp.Total, 0 AS ClearCheck, t.Charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, dwork,dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername, ";
                        str += " r.name  AS locname, l.Address  AS address, t.phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime, Isnull(dp.Total, 0.00) - DATEDIFF(HOUR,dp.TimeRoute,dp.TimeComp ) as timediff, t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";
                        str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                        str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                        str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                        str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                        if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                        }
                        str += ", 0 as dispalert, 0 as credithold, 0 as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, '' as defaultworker";
                        str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                        str += ", (select top 1 signature  from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                        str += ",  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                        str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                        str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join prospect l on l.ID=t.lid INNER JOIN Rol r ON r.ID = l.Rol  left outer join Elev e on e.ID=t.LElev WHERE t.id is not null and t.owner is null and t.LType=1 ";

                        //if (objPropMapData.IsList != 1)
                        //{
                        //    str += " and assigned NOT IN ( 0 )";
                        //}

                        //if (objPropMapData.Assigned != -1)
                        //{
                        //    if (objPropMapData.Assigned == -2)
                        //    {
                        //        str += " and t.Assigned <> 4";
                        //    }
                        //    else
                        //    {
                        //        str += " and t.Assigned=" + objPropMapData.Assigned;
                        //    }
                        //}
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1) + "'";
                        }
                        //if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                        //{
                        //    str += " and t.DWork='" + objPropMapData.Worker + "'";
                        //}
                        //if (objPropMapData.LocID != 0)
                        //{
                        //    str += " and t.LID=" + objPropMapData.LocID;
                        //}
                        //if (objPropMapData.CustID != 0)
                        //{
                        //    str += " and t.Owner=" + objPropMapData.CustID;
                        //}
                        //if (objPropMapData.jobid != 0)
                        //{
                        //    str += " and t.job =" + objPropMapData.jobid;
                        //}
                        //if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                        //{
                        //    str += " and isnull(charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        //}
                        //if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                        //{
                        //    str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                        //}
                        //if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                        //{
                        //    str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                        //}
                        //if (objPropMapData.Mobile == 2)
                        //{
                        //    str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=2";
                        //}
                        //if (objPropMapData.Mobile == 1)
                        //{
                        //    str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=0";
                        //}
                        //if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                        //{
                        //    str += " and t.workorder='" + objPropMapData.Workorder + "'";
                        //}
                        //if (objPropMapData.Department != -1)
                        //{
                        //    str += " and t.type=" + objPropMapData.Department;
                        //}
                        //if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                        //{
                        //    //str += " and t.cat='" + objPropMapData.Category + "'";
                        //    str += " and t.cat in (" + objPropMapData.Category + ")";
                        //}
                        //if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                        //{
                        //    if (objPropMapData.Bremarks == "1")
                        //        str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        //    else
                        //        str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                        //}
                        //if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null)
                        //{
                        //    string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                        //    if (SearchBy == "t.ID")
                        //    {
                        //        str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                        //    }
                        //    else
                        //    {
                        //        if (SearchBy == "l.tag")
                        //            SearchBy = "r.name";
                        //        str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        //    }
                        //}
                        //if (objPropMapData.InvoiceID != 0)
                        //{
                        //    if (objPropMapData.InvoiceID == 1)
                        //    {
                        //        str += " and isnull(Invoice,0) <> 0";
                        //    }
                        //    else if (objPropMapData.InvoiceID == 2)
                        //    {
                        //        str += " and isnull(Invoice,0) = 0 and isnull(charge,0)= 1";
                        //    }
                        //}
                    }



                }
            }
            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)//|| objPropMapData.Assigned == 0
            {
                if (objPropMapData.Mobile != 2)
                {
                    //if (objPropMapData.Mobile == 0)
                    //{
                    if (objPropMapData.Status != 1)
                    {
                        if (objPropMapData.FilterReview != "1")
                        {
                            str += " UNION ALL";
                        }
                    }
                    //}
                    //if (objPropMapData.Mobile != 2)
                    //{
                    str += " SELECT t.who, t.loc as lid, l.id as locid, 4 as assigned, (l.address+', '+l.city+', '+l.state+', '+l.zip) as fulladdress, t.WorkOrder, Reg, OT, NT, DT,TT, Total,isnull( ClearCheck ,0) as ClearCheck ,t.Charge, t.fdesc,timeroute, timesite,timecomp, 1 as comp, (select w.fdesc from tblWork w where t.fwork = w.id) AS dwork,(select w.fdesc from tblWork w where t.fwork = w.id) as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.id,  r.Name  AS customername, l.tag AS locname, l.address, (select top 1 Phone from rol where ID=l.Rol) AS phone,  t.cat, edate, cdate, descres, 'Completed' AS assignname, est,Total as tottime , Isnull(Total, 0.00) - DATEDIFF(HOUR,TimeRoute,TimeComp ) as timediff, t.workorder, (isnull(t.zone,0)+ isnull(t.toll,0) + isnull(t.othere,0)) as expenses, isnull( t.zone,0) as zone, isnull( t.toll,0) as toll , isnull(t.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(t.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(t.custom2)) else 0 end as extraexp, ((isnull(t.emile,0)-isnull(t.smile,0))*0.26) as mileagetravel, (isnull(t.emile,0)-isnull(t.smile,0)) as mileage, (select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount,  (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description,(select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, isnull(invoice,0) as invoice, 0 as Confirmed,  manualinvoice, case  when ( Isnull(invoice, 0) =  0 ) then Manualinvoice else CONVERT(varchar(50), Invoice) end as invoiceno, 0 as ownerid, isnull(QBinvoiceID,'')as QBinvoiceid, isnull(TransferTime,0) as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += ", isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, 0 as high ,e.id as unitid , dbo.TicketEquips(t.ID) as unit, (select Name from Route where ID=l.Route ) as defaultworker";
                    str += ", (select type from jobtype where id = t.type) as department, rtrim(ltrim(t.bremarks)) as bremarks ";
                    //str += ", CONVERT(NUMERIC(30, 2),(((isnull(t.Reg,0) + isnull(t.RegTrav,0)) +  ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + ((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + (isnull(t.TT,0))) * (SELECT Isnull(w.HourlyRate, 0)FROM   tblWork w WHERE  w.ID = t.fWork))) AS LaborExp ";
                    str += " , (select sum( isnull(Amount ,0))from jobi where Labor = 1 and TransID<0 and ref = t.id group by Ref) as laborexp";
                    str += ", (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                    str += ",  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, 0 as EmailNotified, null as EmailTime";
                    str += " FROM ticketd t INNER JOIN loc l ON l.loc = t.loc  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol   left outer join Elev e on e.ID=t.Elev WHERE t.id is not null ";

                    if (objPropMapData.StartDate != System.DateTime.MinValue)
                    {
                        str += " and edate >='" + objPropMapData.StartDate + "'";
                    }
                    if (objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " and edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                    }
                    //if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    //{
                    //    str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker + "'";
                    //}
                    //if (objPropMapData.LocID != 0)
                    //{
                    //    str += " and l.loc=" + objPropMapData.LocID;
                    //}
                    //if (objPropMapData.CustID != 0)
                    //{
                    //    str += " and l.Owner=" + objPropMapData.CustID;
                    //}
                    //if (objPropMapData.jobid != 0)
                    //{
                    //    str += " and t.job =" + objPropMapData.jobid;
                    //}
                    //if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    //{
                    //    str += " and isnull(charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                    //}
                    //if (objPropMapData.Timesheet != null && objPropMapData.Timesheet != string.Empty)
                    //{
                    //    str += " and isnull(TransferTime,0)=" + Convert.ToInt32(objPropMapData.Timesheet);
                    //}
                    //if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    //{
                    //    str += " and (select Super from tblWork w where w.ID=t.fwork ) ='" + objPropMapData.Supervisor + "'";
                    //}
                    //if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    //{
                    //    str += " and (select Super from tblWork w where w.ID=t.fwork ) <>'" + objPropMapData.NonSuper + "'";
                    //}
                    //if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                    //{
                    //    str += " and isnull( ClearCheck ,0) =" + Convert.ToInt32(objPropMapData.FilterReview);
                    //}
                    //if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    //{
                    //    str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    //}
                    //if (objPropMapData.Department != -1)
                    //{
                    //    str += " and t.type=" + objPropMapData.Department;
                    //}
                    //if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    //{
                    //    //str += " and t.cat='" + objPropMapData.Category + "'";
                    //    str += " and t.cat in (" + objPropMapData.Category + ")";
                    //}
                    //if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    //{
                    //    if (objPropMapData.Bremarks == "1")
                    //        str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                    //    else
                    //        str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    //}
                    //if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null)
                    //{
                    //    if (objPropMapData.SearchBy == "t.ldesc4")
                    //    {
                    //        str += " and l.address like '%" + objPropMapData.SearchValue + "%'";
                    //    }
                    //    else if (objPropMapData.SearchBy == "t.ID")
                    //    {
                    //        str += " and " + objPropMapData.SearchBy + " = '" + objPropMapData.SearchValue + "'";
                    //    }
                    //    else
                    //    {
                    //        str += " and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                    //    }
                    //}
                    //if (objPropMapData.Status == 1)
                    //{
                    //    str += " and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1";
                    //}
                    ////if (!string.IsNullOrEmpty( objPropMapData.LocIDs ))
                    ////{
                    ////    str += " and l.loc in (" + objPropMapData.LocIDs + ") ";
                    ////}
                    //if (objPropMapData.RoleID != 0)
                    //    str += " and isnull(l.roleid,0)=" + objPropMapData.RoleID;

                    ////}
                    //if (objPropMapData.InvoiceID != 0)
                    //{
                    //    if (objPropMapData.InvoiceID == 1)
                    //    {
                    //        str += " and isnull(Invoice,0) <> 0  or manualinvoice <> ''";
                    //    }
                    //    else if (objPropMapData.InvoiceID == 2)
                    //    {
                    //        str += " and (isnull(Invoice,0) = 0 or manualinvoice = '') and isnull(charge,0)= 1";
                    //    }
                    //}
                }
            }

            if (str != string.Empty)
            {
                if (!string.IsNullOrEmpty(objPropMapData.OrderBy))
                {
                    string order = objPropMapData.OrderBy;
                    if (order == "WorkOrder")
                        order = "t." + order;

                    str += " order by " + order;
                }
                else
                {
                    str += " order by ID";
                }
            }

            try
            {
                if (str != string.Empty)
                    return str;
                else
                    return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketLogs(MapData objPropMapData)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objPropMapData.TicketID + "  and Screen='Ticket' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ImportDataForMassAttachDocuments(string config, DataTable dataTable)
        {
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter()
                {
                    ParameterName = "MassAttachDocs",
                    SqlDbType = SqlDbType.Structured,
                    Value = dataTable
                };
                SqlHelper.ExecuteNonQuery(config, "spImportDataForMassAttachDocuments", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void ImportDataForMassAttachDocuments(ImportDataForMassAttachDocumentsParam _ImportDataForMassAttachDocuments, string ConnectionString, DataTable dataTable)
        {
            try
            {
                var para = new SqlParameter[1];
                para[0] = new SqlParameter()
                {
                    ParameterName = "MassAttachDocs",
                    SqlDbType = SqlDbType.Structured,
                    Value = dataTable
                };
                SqlHelper.ExecuteNonQuery(ConnectionString, "spImportDataForMassAttachDocuments", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet PostInventoryItemsToProject(MapData objMapData)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@CallDt";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = objMapData.CallDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@Caller";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = objMapData.Who;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@cat";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = objMapData.Category;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Reason";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = objMapData.Reason;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@InventoryItems";
                para[4].SqlDbType = SqlDbType.Structured;
                para[4].Value = objMapData.dtTicketINV;
                para[4].TypeName = "tblTypePostToProject";

                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.StoredProcedure, "spPostToProject", para);

                return SqlHelper.ExecuteDataset(objMapData.ConnConfig, CommandType.StoredProcedure, "spPostToProject", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet PostInventoryItemsToProject(PostInventoryItemsToProjectParam _PostInventoryItemsToProjectParam, string ConnectionString)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@CallDt";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = _PostInventoryItemsToProjectParam.CallDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@Caller";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _PostInventoryItemsToProjectParam.Who;

                para[2] = new SqlParameter();
                para[2].ParameterName = "@cat";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = _PostInventoryItemsToProjectParam.Category;

                para[3] = new SqlParameter();
                para[3].ParameterName = "@Reason";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = _PostInventoryItemsToProjectParam.Reason;

                para[4] = new SqlParameter();
                para[4].ParameterName = "@InventoryItems";
                para[4].SqlDbType = SqlDbType.Structured;
                para[4].Value = _PostInventoryItemsToProjectParam.dtTicketINV;
                para[4].TypeName = "tblTypePostToProject";

                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.StoredProcedure, "spPostToProject", para);

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spPostToProject", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTicketManuaInvoice(MapData objMapData)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "LastUpdatedBy";
                para[0].SqlDbType = SqlDbType.VarChar;
                para[0].Value = objMapData.LastUpdatedBy;

                para[1] = new SqlParameter();
                para[1].ParameterName = "dtTicketINV";
                para[1].SqlDbType = SqlDbType.Structured;
                para[1].Value = objMapData.dtTicketINV;

                SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spUpdateTicketManuaInvoice", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
