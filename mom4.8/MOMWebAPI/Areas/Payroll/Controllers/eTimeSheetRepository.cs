using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using BusinessEntity.payroll;
using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity.APModels;
using System.Data.SqlClient;
using BusinessLayer;
using DataLayer;
using System.Text;
using BusinessEntity.Payroll;
using BusinessEntity.InventoryModel;

namespace MOMWebAPI.Areas.Payroll.Controllers
{
    public class eTimeSheetRepository : IeTimesheet
    {
        public List<UserViewModel> GetUserById(GetUserByIdParam _GetUserByIdParam, string connectionString)
        {
            //try
            //{
               
                return new BusinessLayer.BL_User().getUserByID(_GetUserByIdParam, connectionString);


            //    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _lstUserViewModel.Add(new UserViewModel()
            //        {
            //            UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"])? 0 : dr["UserID"]),
            //            TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"])? 0 : dr["TypeID"]),
            //            DBName = dr["DbName"].ToString(),
            //        });
            //    }
            //    return _lstUserViewModel;

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<UserViewModel> getSavedTimesheet(getTimesheetParam _getSavedTimesheetParam, string ConnectionString)
        {
            //try
            //{
            //    _eTimesheetParam.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_User().getSavedTimesheet(_getSavedTimesheetParam, ConnectionString);
              
            
            //    List<UserViewModel> _userViewModel = new List<UserViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new UserViewModel()
            //            {
            //                Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"])
                          
            //            }
            //            );
            //    }

            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
           // }

        }


        public List<JobTypeViewModel> getDepartment(GetDepartmentParam _getDepartmentParam, string ConnectionString)
        {
            //try
            //{
            //    User _user = new User();
            //    _user.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_User().getDepartment(_getDepartmentParam, ConnectionString);

              
                //List<JobTypeViewModel> _jobtype = new List<JobTypeViewModel>();

                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    _jobtype.Add(
                //        new JobTypeViewModel()
                //        {
                //            ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                //            Type = Convert.ToString(dr["Type"]),
                //            Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                //            Color = Convert.ToInt32(DBNull.Value.Equals(dr["Color"]) ? 0 : dr["Color"]),
                //            Remark = Convert.ToString(dr["Remark"]),
                //            IsDefault = Convert.ToInt32(DBNull.Value.Equals(dr["IsDefault"]) ? 0 : dr["IsDefault"]),
                //            QBJobTypeID = Convert.ToString(dr["QBJobTypeID"]),
                //            LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]),
                //            PrimarySyncID = Convert.ToInt32(DBNull.Value.Equals(dr["PrimarySyncID"]) ? 0 : dr["PrimarySyncID"])
                            
                //        }
                //        );
                //}

                //return _jobtype;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<UserViewModel> getEMPSuper(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
        //    try
        //    {
        //        User _user = new User();
        //        _user.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_User().getEMPSuper(_getConnectionConfigParam, ConnectionString);

               
            //    List<UserViewModel> _userViewModel = new List<UserViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new UserViewModel()
            //            {
            //                Super = (dr["Super"]).ToString(),
            //            }
            //            );
            //    }

            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


        public List<UserViewModel> getEMPwithDeviceID(getTimesheetParam _getTimesheetParam, string ConnectionString)
        {
            //try
            //{
               
            //    _user.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_User().getEMPwithDeviceID(_getTimesheetParam, ConnectionString);

            //    List<UserViewModel> _userViewModel = new List<UserViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new UserViewModel()
            //            {
            //                fDesc = (dr["fdesc"]).ToString(),
            //                WorkId = Convert.ToInt32(DBNull.Value.Equals(dr["WorkId"]) ? 0 : dr["WorkId"]),
                      
            //            }
            //            );
            //    }

            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public List<eTimesheetViewModel> getTimesheetEmp(getTimesheetParam _getTimesheetParam, int eTimeSheet, string ConnectionString)
        {
            //try
            //{
            //    _usermodel.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_User().getTimesheetEmp(_getTimesheetParam, eTimeSheet,ConnectionString);

            //    List<eTimesheetViewModel> _userViewModel = new List<eTimesheetViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new eTimesheetViewModel()
            //            {
            //                ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
            //                Name = (dr["Name"]).ToString(),
            //                EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
            //                Company = (dr["Company"].ToString()),
            //                fDesc = (dr["fDesc"]).ToString(),
            //                reg = Convert.ToInt32(DBNull.Value.Equals(dr["reg"]) ? 0 : dr["reg"]),

            //                OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                           
            //                DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                            
            //                TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
                           
            //                NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
            //                Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
            //                MileageRate = Convert.ToDouble(DBNull.Value.Equals(dr["MileageRate"]) ? 0 : dr["MileageRate"]),
            //                Mileage = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage"]) ? 0 : dr["Mileage"]),
            //                extra = Convert.ToDouble(DBNull.Value.Equals(dr["extra"]) ? 0 : dr["extra"]),
            //                Toll = Convert.ToDouble(DBNull.Value.Equals(dr["Toll"]) ? 0 : dr["Toll"]),
            //                OtherE = Convert.ToDouble(DBNull.Value.Equals(dr["OtherE"]) ? 0 : dr["OtherE"]),
            //                pay = Convert.ToInt32(DBNull.Value.Equals(dr["pay"]) ? 0 : dr["pay"]),
            //                holiday = Convert.ToInt32(DBNull.Value.Equals(dr["holiday"]) ? 0 : dr["holiday"]),
            //                vacation = Convert.ToInt32(DBNull.Value.Equals(dr["vacation"]) ? 0 : dr["vacation"]),
            //                sicktime = Convert.ToInt32(DBNull.Value.Equals(dr["sicktime"]) ? 0 : dr["sicktime"]),
            //                reimb = Convert.ToInt32(DBNull.Value.Equals(dr["reimb"]) ? 0 : dr["reimb"]),
            //                bonus = Convert.ToInt32(DBNull.Value.Equals(dr["bonus"]) ? 0 : dr["bonus"]),
            //                paymethod = Convert.ToInt32(DBNull.Value.Equals(dr["paymethod"]) ? 0 : dr["paymethod"]),
            //                pmethod = Convert.ToInt32(DBNull.Value.Equals(dr["pmethod"]) ? 0 : dr["pmethod"]),
            //                userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
            //                usertype = (dr["usertype"]).ToString(),
            //                total = Convert.ToDouble(DBNull.Value.Equals(dr["total"]) ? 0 : dr["total"]),
            //                phour = Convert.ToDouble(DBNull.Value.Equals(dr["phour"]) ? 0 : dr["phour"]),
            //                salary = Convert.ToDouble(DBNull.Value.Equals(dr["salary"]) ? 0 : dr["salary"]),
            //                HourlyRate = Convert.ToDouble(DBNull.Value.Equals(dr["HourlyRate"]) ? 0 : dr["HourlyRate"]),
            //                Customtick1 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick1"]) ? 0 : dr["Customtick1"]),
            //                dollaramount = Convert.ToInt32(DBNull.Value.Equals(dr["dollaramount"]) ? 0 : dr["dollaramount"]),
            //                Reg1 = Convert.ToInt32(DBNull.Value.Equals(dr["Reg1"]) ? 0 : dr["Reg1"]),
            //                OT1 = Convert.ToInt32(DBNull.Value.Equals(dr["OT1"]) ? 0 : dr["OT1"]),
            //                DT1 = Convert.ToInt32(DBNull.Value.Equals(dr["DT1"]) ? 0 : dr["DT1"]),
            //                TT1 = Convert.ToInt32(DBNull.Value.Equals(dr["TT1"]) ? 0 : dr["TT1"]),
            //                NT1 = Convert.ToInt32(DBNull.Value.Equals(dr["NT1"]) ? 0 : dr["NT1"]),
            //                Zone1 = Convert.ToInt32(DBNull.Value.Equals(dr["Zone1"]) ? 0 : dr["Zone1"]),
            //                Mileage1 = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage1"]) ? 0 : dr["Mileage1"]),
            //                Extra1 = Convert.ToInt32(DBNull.Value.Equals(dr["Extra1"]) ? 0 : dr["Extra1"]),
            //                Toll1 = Convert.ToInt32(DBNull.Value.Equals(dr["Toll1"]) ? 0 : dr["Toll1"]),
            //                HourRate1 = Convert.ToInt32(DBNull.Value.Equals(dr["HourRate1"]) ? 0 : dr["HourRate1"]),
                           
            //                signature = (dr["signature"]).ToString(),
            //                ref1 = Convert.ToInt32(DBNull.Value.Equals(dr["ref1"]) ? 0 : dr["ref1"]),
            //                custom = Convert.ToInt32(DBNull.Value.Equals(dr["custom"]) ? 0 : dr["custom"]),
            //                countDetail = Convert.ToInt32(DBNull.Value.Equals(dr["countDetail"]) ? 0 : dr["countDetail"]),


            //            }
            //            );
            //    }



            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        
        public List<eTimesheetViewModel> getSavedTimesheetEmp(getTimesheetParam _usermodel, string ConnectionString)
        {
            //try
            //{
            //    _usermodel.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_User().getSavedTimesheetEmp(_usermodel,ConnectionString);


            //    List<eTimesheetViewModel> _userViewModel = new List<eTimesheetViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new eTimesheetViewModel()
            //            {
            //                ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
            //                Name = dr["Name"].ToString(),
            //                fDesc = dr["fDesc"].ToString(),
            //                pay = Convert.ToInt32(DBNull.Value.Equals(dr["pay"]) ? 0 : dr["pay"]),
            //                paymethod = Convert.ToInt32(DBNull.Value.Equals(dr["paymethod"]) ? 0 : dr["paymethod"]),
            //                pmethod = Convert.ToInt32(DBNull.Value.Equals(dr["pmethod"]) ? 0 : dr["pmethod"]),
            //                reg = Convert.ToInt32(DBNull.Value.Equals(dr["reg"]) ? 0 : dr["reg"]),
            //                OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
            //                DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
            //                TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
            //                NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
            //                holiday = Convert.ToInt32(DBNull.Value.Equals(dr["holiday"]) ? 0 : dr["holiday"]),
            //                vacation = Convert.ToInt32(DBNull.Value.Equals(dr["vacation"]) ? 0 : dr["vacation"]),
            //                sicktime = Convert.ToInt32(DBNull.Value.Equals(dr["sicktime"]) ? 0 : dr["sicktime"]),
            //                Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
            //                reimb = Convert.ToInt32(DBNull.Value.Equals(dr["reimb"]) ? 0 : dr["reimb"]),
            //                MileageRate = Convert.ToDouble(DBNull.Value.Equals(dr["MileageRate"]) ? 0 : dr["MileageRate"]),
            //                MileRate = Convert.ToInt32(DBNull.Value.Equals(dr["MileRate"]) ? 0 : dr["MileRate"]),
            //                Mileage = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage"]) ? 0 : dr["Mileage"]),
            //                bonus = Convert.ToInt32(DBNull.Value.Equals(dr["bonus"]) ? 0 : dr["bonus"]),
            //                extra = Convert.ToDouble(DBNull.Value.Equals(dr["extra"]) ? 0 : dr["extra"]),
            //                Toll = Convert.ToDouble(DBNull.Value.Equals(dr["Toll"]) ? 0 : dr["Toll"]),
            //                OtherE = Convert.ToDouble(DBNull.Value.Equals(dr["OtherE"]) ? 0 : dr["OtherE"]),
            //                total = Convert.ToDouble(DBNull.Value.Equals(dr["total"]) ? 0 : dr["total"]),
            //                phour = Convert.ToDouble(DBNull.Value.Equals(dr["phour"]) ? 0 : dr["phour"]),
            //                salary = Convert.ToDouble(DBNull.Value.Equals(dr["salary"]) ? 0 : dr["salary"]),
            //                HourlyRate = Convert.ToDouble(DBNull.Value.Equals(dr["HourlyRate"]) ? 0 : dr["HourlyRate"]),
            //                Processed = Convert.ToInt32(DBNull.Value.Equals(dr["Processed"]) ? 0 : dr["Processed"]),
            //                userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
            //                usertype = dr["usertype"].ToString(),
            //                dollaramount = Convert.ToInt32(DBNull.Value.Equals(dr["dollaramount"]) ? 0 : dr["dollaramount"]),
            //                Reg1 = Convert.ToInt32(DBNull.Value.Equals(dr["Reg1"]) ? 0 : dr["Reg1"]),
            //                OT1 = Convert.ToInt32(DBNull.Value.Equals(dr["OT1"]) ? 0 : dr["OT1"]),
            //                DT1 = Convert.ToInt32(DBNull.Value.Equals(dr["DT1"]) ? 0 : dr["DT1"]),
            //                TT1 = Convert.ToInt32(DBNull.Value.Equals(dr["TT1"]) ? 0 : dr["TT1"]),
            //                NT1 = Convert.ToInt32(DBNull.Value.Equals(dr["NT1"]) ? 0 : dr["NT1"]),
            //                Zone1 = Convert.ToInt32(DBNull.Value.Equals(dr["Zone1"]) ? 0 : dr["Zone1"]),
            //                Mileage1 = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage1"]) ? 0 : dr["Mileage1"]),
            //                Extra1 = Convert.ToInt32(DBNull.Value.Equals(dr["Extra1"]) ? 0 : dr["Extra1"]),
            //                Misc1 = Convert.ToInt32(DBNull.Value.Equals(dr["Misc1"]) ? 0 : dr["Misc1"]),
            //                Toll1 = Convert.ToInt32(DBNull.Value.Equals(dr["Toll1"]) ? 0 : dr["Toll1"]),
            //                HourRate1 = Convert.ToInt32(DBNull.Value.Equals(dr["HourRate1"]) ? 0 : dr["HourRate1"]),
            //                ref1 = Convert.ToInt32(DBNull.Value.Equals(dr["ref1"]) ? 0 : dr["ref1"]),
            //                signature = dr["signature"].ToString(),
            //                custom = Convert.ToInt32(DBNull.Value.Equals(dr["custom"]) ? 0 : dr["custom"]),
            //                Customtick1 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick1"]) ? 0 : dr["Customtick1"]),

            //            }
            //            );
            //    }
            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }



        public List<eTimesheetViewModel> GetTimesheetTicketsByEmp(getTimesheetParam _usermodel, int eTimeSheet, string ConnectionString)
        {
            //try
            //{
            //    _usermodel.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_User().GetTimesheetTicketsByEmp(_usermodel,eTimeSheet,ConnectionString);

            //    List<eTimesheetViewModel> _userViewModel = new List<eTimesheetViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new eTimesheetViewModel()
            //            {
            //                Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? 0 : dr["Date"]),
            //                TicketID = Convert.ToInt32(DBNull.Value.Equals(dr["TicketID"]) ? 0 : dr["TicketID"]),
            //                reg = Convert.ToInt32(DBNull.Value.Equals(dr["reg"]) ? 0 : dr["reg"]),
            //                OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
            //                DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
            //                TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
            //                NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
            //                Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
            //                Mileage = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage"]) ? 0 : dr["Mileage"]),
            //                Toll = Convert.ToDouble(DBNull.Value.Equals(dr["Toll"]) ? 0 : dr["Toll"]),
            //                OtherE = Convert.ToDouble(DBNull.Value.Equals(dr["OtherE"]) ? 0 : dr["OtherE"]),
            //                extra = Convert.ToDouble(DBNull.Value.Equals(dr["extra"]) ? 0 : dr["extra"]),
            //                HourlyRate = Convert.ToDouble(DBNull.Value.Equals(dr["HourlyRate"]) ? 0 : dr["HourlyRate"]),
            //                custom = Convert.ToInt32(DBNull.Value.Equals(dr["custom"]) ? 0 : dr["custom"]),
            //                Customtick2 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick2"]) ? 0 : dr["Customtick2"]),
            //                Customtick1 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick1"]) ? 0 : dr["Customtick1"]),
            //                Customtick3 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick3"]) ? 0 : dr["Customtick3"]),
            //            }
            //            );
            //    }
            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public void AddTimesheet(AddTimesheetParam _AddTimesheetParam, string ConnectionString)
        {
             new BusinessLayer.BL_User().AddTimesheet(_AddTimesheetParam, ConnectionString);

            //try
            //{
            //    SqlParameter[] paraEmpData = new SqlParameter[5];

            //    paraEmpData[0] = new SqlParameter();
            //    paraEmpData[0].ParameterName = "@StartDate";
            //    paraEmpData[0].SqlDbType = SqlDbType.DateTime;
            //    paraEmpData[0].Value = _usermodel.Startdt;

            //    paraEmpData[1] = new SqlParameter();
            //    paraEmpData[1].ParameterName = "@EndDate";
            //    paraEmpData[1].SqlDbType = SqlDbType.DateTime;
            //    paraEmpData[1].Value = _usermodel.Enddt;

            //    paraEmpData[2] = new SqlParameter();
            //    paraEmpData[2].ParameterName = "Processed";
            //    paraEmpData[2].SqlDbType = SqlDbType.Int;
            //    paraEmpData[2].Value = _usermodel.IsSuper;

            //    paraEmpData[3] = new SqlParameter();
            //    paraEmpData[3].ParameterName = "EmpData";
            //    paraEmpData[3].SqlDbType = SqlDbType.Structured;
            //    paraEmpData[3].Value = _usermodel.EmpData;

            //    paraEmpData[4] = new SqlParameter();
            //    paraEmpData[4].ParameterName = "TicketData";
            //    paraEmpData[4].SqlDbType = SqlDbType.Structured;
            //    paraEmpData[4].Value = _usermodel.dtTicketData;

            //    SqlHelper.ExecuteNonQuery(_usermodel.ConnConfig, CommandType.StoredProcedure, "spAddTimesheetEmp", paraEmpData);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public List<SageExportTickets> getGetSageExportTickets(getTimesheetParam _usermodel, string ConnectionString)
        {
            //try
            //{
            //    _usermodel.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_User().getGetSageExportTickets(_usermodel, ConnectionString);

                
            //    List<SageExportTickets> _userViewModel = new List<SageExportTickets>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new SageExportTickets()
            //            {
            //                DC = dr["DC"].ToString(),
            //                SageJob = dr["SageJob"].ToString(),
            //                extra = dr["extra"].ToString(), 
            //                costcode = Convert.ToDouble(DBNull.Value.Equals(dr["costcode"]) ? 0 : dr["costcode"]),
            //                Category = dr["Category"].ToString(),
            //                trantype = Convert.ToInt32(DBNull.Value.Equals(dr["trantype"]) ? 0 : dr["trantype"]),
            //                trandate = dr["trandate"].ToString(),
            //                accdate = dr["accdate"].ToString(),
            //                description = dr["description"].ToString(),
            //                units = Convert.ToDouble(DBNull.Value.Equals(dr["units"]) ? 0 : dr["units"]),
            //                unitcost = Convert.ToDouble(DBNull.Value.Equals(dr["unitcost"]) ? 0 : dr["unitcost"]),
            //                amount = Convert.ToDouble(DBNull.Value.Equals(dr["amount"]) ? 0 : dr["amount"]),
            //                debitacc = dr["debitacc"].ToString(),
            //                creditacc = dr["creditacc"].ToString(),
            //                ticket = Convert.ToInt32(DBNull.Value.Equals(dr["ticket"]) ? 0 : dr["ticket"]),
            //            }
            //            );
            //    }
            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public List<UserViewModel> GetCoCode(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            //try
            //{
            //    User _user = new User();
            //    _user.ConnConfig = ConnectionString;
              return new BusinessLayer.BL_User().GetCoCode(_getConnectionConfigParam, ConnectionString);

            //    List <UserViewModel> _userViewModel = new List<UserViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _userViewModel.Add(
            //            new UserViewModel()
            //            {
            //                CoCode = dr["CoCode"].ToString(),

            //            }
            //            );
            //    }
            //    return _userViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public void UpdateCoCode(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            new BusinessLayer.BL_User().UpdateCoCode(_getConnectionConfigParam, ConnectionString);

            //try
            //{
            //    string query = "Update Control SET CoCode = '"+ _usermodel.CoCode +"'";
            //    List<SqlParameter> parameters = new List<SqlParameter>();
            //    //parameters.Add(new SqlParameter("@code", _usermodel.CoCode));

            //    SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, query);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public List<PayrollViewModel> getPayRoll(getTimesheetParam _getTimesheetParam, string ConnectionString)
        {
            //try
            //{

            //    _usermodel.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_User().getPayRoll(_getTimesheetParam, ConnectionString);

            //    List<PayrollViewModel> _payrollViewModel = new List<PayrollViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _payrollViewModel.Add(
            //            new PayrollViewModel()
            //            {
            //                CoCode = dr["CoCode"].ToString(),
            //                BatchID = dr["BatchID"].ToString(),
            //                EmpRef = dr["EmpRef"].ToString(),
            //                Shift = Convert.ToInt32(DBNull.Value.Equals(dr["Shift"]) ? 0 : dr["Shift"]),
            //                TempDept = dr["TempDept"].ToString(),
            //                RateCode = dr["RateCode"].ToString(),
            //                RegHours = Convert.ToInt32(DBNull.Value.Equals(dr["RegHours"]) ? 0 : dr["RegHours"]),
            //                OTHours = Convert.ToInt32(DBNull.Value.Equals(dr["OTHours"]) ? 0 : dr["OTHours"]),
            //                Hour3Code = dr["Hour3Code"].ToString(),
            //                Hours3Amount = dr["Hour3Amount"].ToString()
            //            }
            //            );
            //    }
            //    return _payrollViewModel;

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public List<GeneralViewModel> getSagelatsync(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            //try
            //{
            //    General _general = new General();
            //    _general.ConnConfig = ConnectionString;
              return new BusinessLayer.BL_General().getSagelatsync(_getConnectionConfigParam, ConnectionString);

            //    List<GeneralViewModel> _generalViewModel = new List<GeneralViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _generalViewModel.Add(
            //            new GeneralViewModel()
            //            {
            //                SageLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["SageLastSync"]) ? 0 : dr["SageLastSync"]),
            //                sageintegration = Convert.ToInt32(DBNull.Value.Equals(dr["sageintegration"]) ? 0 : dr["sageintegration"]),
            //            }
            //            );
            //    }
            //    return _generalViewModel;

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        //user
        public int getLoginSuper(AddUserParam _AddUserParam, string connectionString)
        {
            //try
            //{

            //   _userViewModel.ConnConfig = connectionString;
              return new BusinessLayer.BL_User().getLoginSuper(_AddUserParam,connectionString);
            //    return loginuser;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public int getISSuper(AddUserParam _AddUserParam, string connectionString)
        {
            //try
            //{
            //    _userViewModel.ConnConfig = connectionString;
                return  new BusinessLayer.BL_User().getISSuper(_AddUserParam, connectionString);
            //    return ssuser;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


        public List<SuperUserViewModel> getUserForSupervisor(AddUserParam _AddUserParam, string ConnectionString)
        {
            //try
            //{

            //    _usermodel.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_User().getUserForSupervisor(_AddUserParam, ConnectionString);

            //    List<SuperUserViewModel> _superuserViewModel = new List<SuperUserViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _superuserViewModel.Add(
            //            new SuperUserViewModel()
            //            {
            //                ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
            //                fFirst =Convert.ToString(dr["fFirst"]),
            //                Last = Convert.ToString(dr["fFirst"]),
            //                userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
            //                fUser = Convert.ToString(dr["fUser"]),
            //                Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
            //                super = Convert.ToString(dr["super"]),
            //                usertype = Convert.ToString(dr["usertype"]),
            //                usertypeid = Convert.ToInt32(DBNull.Value.Equals(dr["usertypeid"]) ? 0 : dr["usertypeid"]),
            //                userkey = Convert.ToString(dr["userkey"])

            //            }
            //            );
            //    }
            //    return _superuserViewModel;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        //report
        public List<CustomerReportViewModel> GetReportDetailById(CustomerReportParam _CustomerReportParam, string ConnectionString)
        {
        //    try
        //    {
        //        _customerreport.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().GetReportDetailById(_CustomerReportParam, ConnectionString);

            //    List<CustomerReportViewModel> _customer= new List<CustomerReportViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new CustomerReportViewModel()
            //            {
            //                Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
            //                ReportName = Convert.ToString(dr["ReportName"]),
            //                ReportType = Convert.ToString(dr["ReportType"]),
            //                UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
            //                IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
            //                IsAscending = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscending"]) ? 0 : dr["IsAscending"]),
            //                SortBy = Convert.ToString(dr["SortBy"]),
            //                IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? 0 : dr["IsStock"]),
            //                Module = Convert.ToString(dr["Module"]),
            //                Condition = Convert.ToString(dr["Condition"])
            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<CustomerReportViewModel> GetCustomerType(GetCustomerTypeParam _customer, string ConnectionString)
        {
            //try
            //{
            //    CustomerReport _customer = new CustomerReport();
            //    _customer.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().GetCustomerType(_customer, ConnectionString);

            //    List<CustomerReportViewModel> _customers = new List<CustomerReportViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customers.Add(
            //            new CustomerReportViewModel()
            //            {
            //                Type = Convert.ToString(dr["Type"])
                           
            //            });
            //    }
            //    return _customers;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public ListGetCustReportFiltersValue GetCustReportFiltersValue(GetCustReportFiltersValueParam _getConnectionConfigParam, string ConnectionString)
        {
            //try
            //{
            //    _user.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().GetCustReportFiltersValue(_getConnectionConfigParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customers = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customers.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                Name = Convert.ToString(dr["Name"]),
            //                Address = Convert.ToString(dr["Address"]),
            //                City = Convert.ToString(dr["City"]),
            //                State = Convert.ToString(dr["State"]),
            //                Type = Convert.ToString(dr["Type"]),
            //                Status = Convert.ToString(dr["Status"]),

            //            });
            //    }
            //    return _customers;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public List<CustomerFilterViewModel> GetCustomerName(GetCustomerNameParam _CustomerReportParam, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().GetCustomerName(_CustomerReportParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customers = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customers.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                Name = Convert.ToString(dr["Name"]),

            //            });
            //    }
            //    return _customers;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public List<CustomerFilterViewModel> GetCustomerAddress(GetCustomerAddressParam _CustomerReportParam, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
               return  new BusinessLayer.BL_ReportsData().GetCustomerAddress(_CustomerReportParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customers = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customers.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                Address = Convert.ToString(dr["Address"]),

            //            });
            //    }
            //    return _customers;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public List<CustomerFilterViewModel> GetCustomerCity(GetCustomerCityParam _CustomerReportParam, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_ReportsData().GetCustomerCity(_CustomerReportParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customers = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customers.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                City = Convert.ToString(dr["City"]),

            //            });
            //    }
            //    return _customers;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public List<CustomerReportViewModel> GetDynamicReports(GetDynamicReportsParam _CustomerReportParam, string ConnectionString)
        {
            //try
            //{
            //    _user.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_ReportsData().GetDynamicReports(_CustomerReportParam, ConnectionString);

            //    List<CustomerReportViewModel> _customer = new List<CustomerReportViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new CustomerReportViewModel()
            //            {
            //                Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
            //                ReportName = Convert.ToString(dr["ReportName"]),
            //                ReportType = Convert.ToString(dr["ReportType"]),
            //                UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
            //                IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
            //                IsAscending = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscending"]) ? 0 : dr["IsAscending"]),
            //                SortBy = Convert.ToString(dr["SortBy"]),
            //                IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? 0 : dr["IsStock"]),
            //                Module = Convert.ToString(dr["Module"]),
            //                Condition = Convert.ToString(dr["Condition"])
            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public List<CustomerFilterViewModel> GetReportColByRepId(GetReportColByRepIdParam _CustomerReportParam, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
              return new BusinessLayer.BL_ReportsData().GetReportColByRepId(_CustomerReportParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                ColumnName = Convert.ToString(dr["ColumnName"]),
                           
            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
          //  }

        }


        public List<CustomerFilterViewModel> GetReportFiltersByRepId(GetReportFiltersByRepIdParam _CustomerReportParam, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_ReportsData().GetReportFiltersByRepId(_CustomerReportParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                FilterColumn = Convert.ToString(dr["ColumnName"]),
            //                FilterSet = Convert.ToString(dr["FilterSet"]),
            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }


        public List<CustomerFilterViewModel> getCustomerDetailsTest(getCustomerDetailsTestParam _getConnectionConfigParam, string ConnectionString)
        {
            //try
            //{
            //    _user.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().getCustomerDetailsTest(_getConnectionConfigParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                Name = Convert.ToString(dr["Name"]),
            //                City = Convert.ToString(dr["City"]),
            //                State = Convert.ToString(dr["State"]),
            //                Zip = Convert.ToString(dr["Zip"]),
            //                Phone = Convert.ToString(dr["Phone"]),
            //                Fax = Convert.ToString(dr["Fax"]),
            //                Contact = Convert.ToString(dr["Contact"]),
            //                Address = Convert.ToString(dr["address"]),
            //                Email = Convert.ToString(dr["Email"]),
            //                Country = Convert.ToString(dr["Country"]),
            //                Website = Convert.ToString(dr["Website"]),
            //                Cellular = Convert.ToString(dr["Cellular"]),
            //                Type = Convert.ToString(dr["Type"]),
            //                Balance = Convert.ToString(dr["Balance"]),
            //                Status= Convert.ToString(dr["Status"]),
            //                loc= Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
            //                equip = Convert.ToInt32(DBNull.Value.Equals(dr["equip"]) ? 0 : dr["equip"]),
            //                opencall = Convert.ToInt32(DBNull.Value.Equals(dr["opencall"]) ? 0 : dr["opencall"]),

            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public bool CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_ReportsData().CheckExistingReport(_CheckExistingReportParam, ConnectionString);

            //    return test;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public bool IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
               return new BusinessLayer.BL_ReportsData().IsStockReportExist(_IsStockReportExistParam, ConnectionString);

            //    return test;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReport(_UpdateCustomerReportParam, ConnectionString);
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
            //    var para = new SqlParameter[24];

            //    para[0] = new SqlParameter
            //    {
            //        ParameterName = "@RptId",
            //        SqlDbType = SqlDbType.Int,
            //        Value = _customerreport.ReportId
            //    };
            //    para[1] = new SqlParameter
            //    {
            //        ParameterName = "@ReportName",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.ReportName
            //    };
            //    para[2] = new SqlParameter
            //    {
            //        ParameterName = "@ReportType",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.ReportType
            //    };
            //    para[3] = new SqlParameter
            //    {
            //        ParameterName = "@UserId",
            //        SqlDbType = SqlDbType.Int,
            //        Value = _customerreport.UserId
            //    };
            //    para[4] = new SqlParameter
            //    {
            //        ParameterName = "@IsGlobal",
            //        SqlDbType = SqlDbType.Bit,
            //        Value = _customerreport.IsGlobal
            //    };
            //    para[5] = new SqlParameter
            //    {
            //        ParameterName = "@IsAscendingOrder",
            //        SqlDbType = SqlDbType.Bit,
            //        Value = _customerreport.IsAscending
            //    };
            //    para[6] = new SqlParameter
            //    {
            //        ParameterName = "@SortBy",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.SortBy
            //    };
            //    para[7] = new SqlParameter
            //    {
            //        ParameterName = "@ColumnName",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.ColumnName
            //    };
            //    para[8] = new SqlParameter
            //    {
            //        ParameterName = "@FilterColumns",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.FilterColumns
            //    };
            //    para[9] = new SqlParameter
            //    {
            //        ParameterName = "@FilterValues",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.FilterValues
            //    };
            //    para[10] = new SqlParameter
            //    {
            //        ParameterName = "@CompanyName",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.CompanyName
            //    };
            //    para[11] = new SqlParameter
            //    {
            //        ParameterName = "@ReportTitle",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.ReportTitle
            //    };
            //    para[12] = new SqlParameter
            //    {
            //        ParameterName = "@SubTitle",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.SubTitle
            //    };
            //    para[13] = new SqlParameter
            //    {
            //        ParameterName = "@DatePrepared",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.DatePrepared
            //    };
            //    para[14] = new SqlParameter
            //    {
            //        ParameterName = "@TimePrepared",
            //        SqlDbType = SqlDbType.Bit,
            //        Value = _customerreport.TimePrepared
            //    };
            //    para[15] = new SqlParameter
            //    {
            //        ParameterName = "@PageNumber",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.PageNumber
            //    };
            //    para[16] = new SqlParameter
            //    {
            //        ParameterName = "@ExtraFooterLine",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.ExtraFooterLine
            //    };
            //    para[17] = new SqlParameter
            //    {
            //        ParameterName = "@Alignment",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.Alignment
            //    };
            //    para[18] = new SqlParameter
            //    {
            //        ParameterName = "@ColumnWidth",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.ColumnWidth
            //    };
            //    para[19] = new SqlParameter
            //    {
            //        ParameterName = "@MainHeader",
            //        SqlDbType = SqlDbType.Bit,
            //        Value = _customerreport.MainHeader
            //    };
            //    para[20] = new SqlParameter
            //    {
            //        ParameterName = "@PDFSize",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.PDFSize
            //    };
            //    para[21] = new SqlParameter
            //    {
            //        ParameterName = "@IsStock",
            //        SqlDbType = SqlDbType.Bit,
            //        Value = _customerreport.IsStock
            //    };
            //    para[22] = new SqlParameter
            //    {
            //        ParameterName = "@Module",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.Module
            //    };
            //    para[23] = new SqlParameter
            //    {
            //        ParameterName = "@Condition",
            //        SqlDbType = SqlDbType.NVarChar,
            //        Value = _customerreport.Condition
            //    };

            // SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateCustomerReportDetails", para);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public List<CustomerReportViewModel> InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().InsertCustomerReport(_InsertCustomerReportParam, ConnectionString);

        }

        public void DeleteCustomerReport(DeleteCustomerReportParam _customerreport, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().DeleteCustomerReport(_customerreport, ConnectionString);
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;

            //    var para = new SqlParameter[1];

            //    para[0] = new SqlParameter
            //    {
            //        ParameterName = "@ReportId",
            //        SqlDbType = SqlDbType.Int,
            //        Value = _customerreport.ReportId
            //    };

            //     SqlHelper.ExecuteNonQuery(_customerreport.ConnConfig, CommandType.StoredProcedure, "spDeleteCustomerReport", para);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<CustomerFilterViewModel> GetControlForReports(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            //try
            //{
            //    _user.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().GetControlForReports(_getConnectionConfigParam, ConnectionString);

            //    List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new CustomerFilterViewModel()
            //            {
            //                Name = Convert.ToString(dr["Name"]),
            //                Address = Convert.ToString(dr["Address"]),
            //                City = Convert.ToString(dr["City"]),
            //                State = Convert.ToString(dr["State"]),
            //                Zip = Convert.ToString(dr["Zip"]),
            //                Phone = Convert.ToString(dr["Phone"]),
            //                Fax = Convert.ToString(dr["Fax"]),
            //                Email = Convert.ToString(dr["Email"]),
            //                WebAddress = Convert.ToString(dr["WebAddress"]),
            //                Logo = Convert.ToString(dr["Logo"]),
            //                DBName = Convert.ToString(dr["dbname"]),
                           
            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<CustomerFilterViewModel> GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
        {
            return new BusinessLayer.BL_ReportsData().GetOwners(_GetOwnersParam, ConnectionString);
        }

        public List<HeaderFooterDetailViewModel> GetHeaderFooterDetail(GetHeaderFooterDetailParam _customerreport, string ConnectionString)
        {
        //    try
        //    {
        //        _customerreport.ConnConfig = ConnectionString;
                  return new BusinessLayer.BL_ReportsData().GetHeaderFooterDetail(_customerreport,ConnectionString);

            //    List<HeaderFooterDetailViewModel> _customer = new List<HeaderFooterDetailViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new HeaderFooterDetailViewModel()
            //            {
            //                Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
            //                ReportId = Convert.ToInt32(DBNull.Value.Equals(dr["ReportId"]) ? 0 : dr["ReportId"]),
            //                MainHeader = Convert.ToBoolean(DBNull.Value.Equals(dr["MainHeader"]) ? 0 : dr["MainHeader"]),
            //                CompanyName = Convert.ToString(dr["CompanyName"]),
            //                ReportTitle = Convert.ToString(dr["ReportTitle"]),
            //                SubTitle = Convert.ToString(dr["SubTitle"]),
            //                DatePrepared = Convert.ToString(dr["DatePrepared"]),
            //                TimePrepared = Convert.ToBoolean(DBNull.Value.Equals(dr["TimePrepared"]) ? 0 : dr["TimePrepared"]),
            //                ReportBasis = Convert.ToBoolean(DBNull.Value.Equals(dr["ReportBasis"]) ? 0 : dr["ReportBasis"]),
            //                PageNumber = Convert.ToString(dr["PageNumber"]),
            //                ExtraFooterLine = Convert.ToString(dr["ExtraFooterLine"]),
            //                Alignment = Convert.ToString(dr["Alignment"]),
            //                PDFSize = Convert.ToString(dr["PDFSize"]),

            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<CustomerReportViewModel> GetColumnWidthByReportId(GetColumnWidthByReportIdParam _customerreport, string ConnectionString)
        {
            //try
            //{
            //    _customerreport.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().GetColumnWidthByReportId(_customerreport,ConnectionString);

            //    List<CustomerReportViewModel> _customer = new List<CustomerReportViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _customer.Add(
            //            new CustomerReportViewModel()
            //            {
            //                ColumnWidth = Convert.ToString(dr["ColumnWidth"]),

            //            });
            //    }
            //    return _customer;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam _customerreport, string ConnectionString)
        {
            new BusinessLayer.BL_ReportsData().UpdateCustomerReportResizedWidth(_customerreport, ConnectionString);
        }

        public List<TicketViewModel> GetTicketList(PJ objPJ, string ConnectionString)
        {
            //try
            //{
            //    objPJ.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_ReportsData().GetTicketList(objPJ,ConnectionString);

            //    List<TicketViewModel> _ticket = new List<TicketViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _ticket.Add(
            //            new TicketViewModel()
            //            {
            //                ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
            //                TWokrOrder = Convert.ToString(dr["TWokrOrder"]),
            //                TCDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TCDate"]) ? 0 : dr["TCDate"]),
            //                TDDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TDDate"]) ? 0 : dr["TDDate"]),
            //                TEDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TEDate"]) ? 0 : dr["TEDate"]),
            //                JType = Convert.ToInt32(DBNull.Value.Equals(dr["JType"]) ? 0 : dr["JType"]),
            //                WfDesc = Convert.ToString(dr["WfDesc"]),
            //                TfDesc = Convert.ToString(dr["TfDesc"]),
            //                TTotal = Convert.ToInt32(DBNull.Value.Equals(dr["TTotal"]) ? 0 : dr["TTotal"]),
            //                WIReg = Convert.ToInt32(DBNull.Value.Equals(dr["WIReg"]) ? 0 : dr["WIReg"]),
            //                Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
            //                LocName = Convert.ToString(dr["LocName"])
            //            });
            //    }
            //    return _ticket;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<GetControlViewModel> getControl(getConnectionConfigParam _user, string ConnectionString)
        {
            //try
            //{
            //    _user.ConnConfig = ConnectionString;
                return new BusinessLayer.BL_User().getControl(_user,ConnectionString);

            //    List<ControlViewModel> _ticket = new List<ControlViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _ticket.Add(
            //            new ControlViewModel()
            //            {
            //                Name = Convert.ToString(dr["Name"]),
            //                City = Convert.ToString(dr["City"]),
            //                State = Convert.ToString(dr["State"]),
            //                Zip = Convert.ToString(dr["Zip"]),
            //                Phone = Convert.ToString(dr["Phone"]),
            //                Fax = Convert.ToString(dr["Fax"]),
            //                fLong = Convert.ToInt32(DBNull.Value.Equals(dr["fLong"]) ? 0 : dr["fLong"]),
            //                Latt = Convert.ToInt32(DBNull.Value.Equals(dr["Latt"]) ? 0 : dr["Latt"]),
            //                GeoLock = Convert.ToInt32(DBNull.Value.Equals(dr["GeoLock"]) ? 0 : dr["GeoLock"]),
            //                YE = Convert.ToInt32(DBNull.Value.Equals(dr["YE"]) ? 0 : dr["YE"]),
            //                Version = Convert.ToInt32(DBNull.Value.Equals(dr["Version"]) ? 0 : dr["Version"]),
            //                CDesc = Convert.ToString(dr["CDesc"]),
            //                Build = Convert.ToInt32(DBNull.Value.Equals(dr["Build"]) ? 0 : dr["Build"]),
            //                Minor = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
            //                Address = Convert.ToString(dr["Address"]),
            //                AgeRemark = Convert.ToString(dr["AgeRemark"]),
            //                SDate = Convert.ToString(dr["SDate"]),
            //                EDate = Convert.ToString(dr["EDate"]),
            //                YDate = Convert.ToString(dr["YDate"]),
            //                GSTreg = Convert.ToString(dr["GSTreg"]),
            //                IDesc = Convert.ToString(dr["IDesc"]),
            //                PortalsID = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
            //                PrContractRemark = Convert.ToString(dr["PrContractRemark"]),
            //                RepUser = Convert.ToString(dr["RepUser"]),
            //                RepTitle = Convert.ToString(dr["RepTitle"]),
            //                Logo = Convert.ToString(dr["Logo"]),
            //                LogoPath = Convert.ToString(dr["LogoPath"]),
            //                ExeBuildDate_Max = Convert.ToDateTime(DBNull.Value.Equals(dr["ExeBuildDate_Max"]) ? 0 : dr["ExeBuildDate_Max"]),
            //                ExeBuildDate_Min = Convert.ToDateTime(DBNull.Value.Equals(dr["ExeBuildDate_Min"]) ? 0 : dr["ExeBuildDate_Min"]),
            //                ExeVersion_Min = Convert.ToString(dr["ExeVersion_Min"]),
            //                ExeVersion_Max = Convert.ToString(dr["ExeVersion_Max"]),
            //                MerchantServicesConfig = Convert.ToString(dr["MerchantServicesConfig"]),
            //                Email = Convert.ToString(dr["Email"]),
            //                WebAddress = Convert.ToString(dr["WebAddress"]),
            //                MSM = Convert.ToString(dr["MSM"]),
            //                DSN = Convert.ToString(dr["DSN"]),
            //                Username = Convert.ToString(dr["Username"]),
            //                Password = Convert.ToString(dr["Password"]),
            //                DBName = Convert.ToString(dr["DBName"]),
            //                Remarks = Convert.ToString(dr["Remarks"]),
            //                Map = Convert.ToInt32(DBNull.Value.Equals(dr["Map"]) ? 0 : dr["Map"]),
            //                Custweb = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
            //                QBPath = Convert.ToString(dr["QBPath"]),
            //                MultiLang = Convert.ToInt32(DBNull.Value.Equals(dr["MultiLang"]) ? 0 : dr["MultiLang"]),
            //                QBIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["QBIntegration"]) ? 0 : dr["QBIntegration"]),
            //                QBLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["QBLastSync"]) ? 0 : dr["QBLastSync"]),
            //                QBFirstSync = Convert.ToInt32(DBNull.Value.Equals(dr["QBFirstSync"]) ? 0 : dr["QBFirstSync"]),
            //                MSEmail = Convert.ToInt32(DBNull.Value.Equals(dr["MSEmail"]) ? 0 : dr["MSEmail"]),
            //                MSREP = Convert.ToInt32(DBNull.Value.Equals(dr["MSREP"]) ? 0 : dr["MSREP"]),
            //                MSSignTime = Convert.ToInt32(DBNull.Value.Equals(dr["MSSignTime"]) ? 0 : dr["MSSignTime"]),
            //                GrossInc = Convert.ToInt32(DBNull.Value.Equals(dr["GrossInc"]) ? 0 : dr["GrossInc"]),
            //                Month = Convert.ToInt32(DBNull.Value.Equals(dr["Month"]) ? 0 : dr["Month"]),
            //                SalesAnnual = Convert.ToInt32(DBNull.Value.Equals(dr["SalesAnnual"]) ? 0 : dr["SalesAnnual"]),
            //                Payment = Convert.ToInt32(DBNull.Value.Equals(dr["Payment"]) ? 0 : dr["Payment"]),
            //                QBServiceItem = Convert.ToString(dr["QBServiceItem"]),
            //                QBServiceItemLabor = Convert.ToString(dr["QBServiceItemLabor"]),
            //                QBServiceItemExp = Convert.ToString(dr["QBServiceItemExp"]),
            //                GPS = Convert.ToInt32(DBNull.Value.Equals(dr["AcctSub"]) ? 0 : dr["AcctSub"]),
            //                SageLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["SageLastSync"]) ? 0 : dr["SageLastSync"]),
            //                SageIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["SageIntegration"]) ? 0 : dr["SageIntegration"]),
            //                MSAttachReport = Convert.ToInt32(DBNull.Value.Equals(dr["MSAttachReport"]) ? 0 : dr["MSAttachReport"]),
            //                MSRTLabel = Convert.ToString(dr["MSRTLabel"]),
            //                MSOTLabel = Convert.ToString(dr["MSOTLabel"]),
            //                MSNTLabel = Convert.ToString(dr["MSNTLabel"]),
            //                MSDTLabel = Convert.ToString(dr["MSDTLabel"]),
            //                MSTTLabel = Convert.ToString(dr["MSTTLabel"]),
            //                MSTRTLabel = Convert.ToString(dr["MSTRTLabel"]),
            //                MSTOTLabel = Convert.ToString(dr["MSTOTLabel"]),
            //                MSTNTLabel = Convert.ToString(dr["MSTNTLabel"]),
            //                MSTDTLabel = Convert.ToString(dr["MSTDTLabel"]),
            //                MSTimeDataFieldVisibility = Convert.ToString(dr["MSTimeDataFieldVisibility"]),
            //                TsIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["TsIntegration"]) ? 0 : dr["TsIntegration"]),
            //                TInternet = Convert.ToInt32(DBNull.Value.Equals(dr["TInternet"]) ? 0 : dr["TInternet"]),
            //                SyncLast = Convert.ToDateTime(DBNull.Value.Equals(dr["SyncLast"]) ? 0 : dr["SyncLast"]),
            //                SCDate = Convert.ToDateTime(DBNull.Value.Equals(dr["SCDate"]) ? 0 : dr["SCDate"]),
            //                IntDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IntDate"]) ? 0 : dr["IntDate"]),
            //                SCAmount = Convert.ToInt32(DBNull.Value.Equals(dr["SCAmount"]) ? 0 : dr["SCAmount"]),
            //                IntAmount = Convert.ToInt32(DBNull.Value.Equals(dr["IntAmount"]) ? 0 : dr["IntAmount"]),
            //                EndBalance = Convert.ToInt32(DBNull.Value.Equals(dr["EndBalance"]) ? 0 : dr["EndBalance"]),
            //                StatementDate = Convert.ToDateTime(DBNull.Value.Equals(dr["StatementDate"]) ? 0 : dr["StatementDate"]),
            //                bank = Convert.ToInt32(DBNull.Value.Equals(dr["bank"]) ? 0 : dr["bank"]),
            //                BusinessStart = Convert.ToDateTime(DBNull.Value.Equals(dr["BusinessStart"]) ? 0 : dr["BusinessStart"]),
            //                BusinessEnd = Convert.ToDateTime(DBNull.Value.Equals(dr["BusinessEnd"]) ? 0 : dr["BusinessEnd"]),
            //                JobCostLabor = Convert.ToInt32(DBNull.Value.Equals(dr["JobCostLabor"]) ? 0 : dr["JobCostLabor"]),
            //                MSIsTaskCodesRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["MSIsTaskCodesRequired"]) ? 0 : dr["MSIsTaskCodesRequired"]),
            //                Codes = Convert.ToInt32(DBNull.Value.Equals(dr["Codes"]) ? 0 : dr["Codes"]),
            //                ISshowHomeowner = Convert.ToBoolean(DBNull.Value.Equals(dr["ISshowHomeowner"]) ? 0 : dr["ISshowHomeowner"]),
            //                IsLocAddressBlank = Convert.ToBoolean(DBNull.Value.Equals(dr["IsLocAddressBlank"]) ? 0 : dr["IsLocAddressBlank"]),
            //                PGUsername = Convert.ToString(dr["PGUsername"]),
            //                PGPassword = Convert.ToString(dr["PGPassword"]),
            //                PGSecretKey = Convert.ToString(dr["PGSecretKey"]),
            //                MSAppendMCPText = Convert.ToBoolean(DBNull.Value.Equals(dr["MSAppendMCPText"]) ? 0 : dr["MSAppendMCPText"]),
            //                MSSHAssignedTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["MSSHAssignedTicket"]) ? 0 : dr["MSSHAssignedTicket"]),
            //                MSHistoryShowLastTenTickets = Convert.ToBoolean(DBNull.Value.Equals(dr["MSHistoryShowLastTenTickets"]) ? 0 : dr["MSHistoryShowLastTenTickets"]),
            //                MS = Convert.ToBoolean(DBNull.Value.Equals(dr["MS"]) ? 0 : dr["MS"]),
            //                ContactType = Convert.ToInt32(DBNull.Value.Equals(dr["ContactType"]) ? 0 : dr["AcctSub"]),
            //                Lat = Convert.ToString(dr["Lat"]),
            //                Lng = Convert.ToString(dr["Lng"]),
            //                MSFollowupTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["MSFollowupTicket"]) ? 0 : dr["MSFollowupTicket"]),
            //                consultAPI = Convert.ToInt32(DBNull.Value.Equals(dr["consultAPI"]) ? 0 : dr["consultAPI"]),
            //                businesssEnd = Convert.ToDateTime(DBNull.Value.Equals(dr["businesssEnd"]) ? 0 : dr["businesssEnd"]),
            //                CoCode = Convert.ToString(dr["CoCode"]),
            //                ShutdownAlert = Convert.ToInt32(DBNull.Value.Equals(dr["ShutdownAlert"]) ? 0 : dr["ShutdownAlert"]),
            //                MSCategoryPermission = Convert.ToInt32(DBNull.Value.Equals(dr["MSCategoryPermission"]) ? 0 : dr["MSCategoryPermission"]),
            //                IsSalesTaxAPBill = Convert.ToBoolean(DBNull.Value.Equals(dr["IsSalesTaxAPBill"]) ? 0 : dr["IsSalesTaxAPBill"]),
            //                IsUseTaxAPBill = Convert.ToBoolean(DBNull.Value.Equals(dr["IsUseTaxAPBill"]) ? 0 : dr["IsUseTaxAPBill"]),
            //                ApplyPasswordRules = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPasswordRules"]) ? 0 : dr["ApplyPasswordRules"]),
            //                ApplyPwRulesToFieldUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToFieldUser"]) ? 0 : dr["ApplyPwRulesToFieldUser"]),
            //                ApplyPwRulesToOfficeUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToOfficeUser"]) ? 0 : dr["ApplyPwRulesToOfficeUser"]),
            //                ApplyPwRulesToCustomerUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToCustomerUser"])? 0 : dr["ApplyPwRulesToCustomerUser"]),
            //                ApplyPwReset = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwReset"]) ? 0 : dr["ApplyPwReset"]),
            //                PwResetDays = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetDays"]) ? 0 : dr["PwResetDays"]),
            //                PwResetting = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetting"]) ? 0 : dr["PwResetting"]),
            //                PwResetUserID = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetUserID"]) ? 0 : dr["PwResetUserID"]),
            //                JobCostLabor1 = Convert.ToInt32(DBNull.Value.Equals(dr["JobCostLabor1"]) ? 0 : dr["JobCostLabor1"]),
            //                msemailnull = Convert.ToInt32(DBNull.Value.Equals(dr["msemailnull"]) ? 0 : dr["msemailnull"]),
            //                EmpSync = Convert.ToInt32(DBNull.Value.Equals(dr["EmpSync"]) ? 0 : dr["EmpSync"]),
            //                msreptemp = Convert.ToInt32(DBNull.Value.Equals(dr["msreptemp"]) ? 0 : dr["msreptemp"]),
            //                tinternett = Convert.ToInt32(DBNull.Value.Equals(dr["tinternett"]) ? 0 : dr["tinternett"]),
            //                businessstart = Convert.ToInt32(DBNull.Value.Equals(dr["businessstart"]) ? 0 : dr["businessstart"]),
            //                businesssend = Convert.ToInt32(DBNull.Value.Equals(dr["businesssend"]) ? 0 : dr["businesssend"]),
            //                TaskCode = Convert.ToInt32(DBNull.Value.Equals(dr["TaskCode"]) ? 0 : dr["TaskCode"]),
            //                Year = Convert.ToInt32(DBNull.Value.Equals(dr["Year"]) ? 0 : dr["Year"]),
            //                IsSalesTaxAPBills = Convert.ToInt32(DBNull.Value.Equals(dr["IsSalesTaxAPBills"]) ? 0 : dr["IsSalesTaxAPBills"]),
            //                TargetHPermission = Convert.ToInt32(DBNull.Value.Equals(dr["TargetHPermission"]) ? 0 : dr["TargetHPermission"]),
            //                PwResetAdminEmail = Convert.ToString(dr["PwResetAdminEmail"]),
            //                PwResetUsername = Convert.ToString(dr["PwResetUsername"])

            //            });
            //    }
            //    return _ticket;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<SMTPEmailViewModel> getSMTPByUserID(GetSMTPByUserIDParam _user, string ConnectionString)
        {
            //try
            //{
            //    _user.ConnConfig = ConnectionString;
                return  new BusinessLayer.BL_User().getSMTPByUserID(_user,ConnectionString);

            //    List<SMTPEmailViewModel> _ticket = new List<SMTPEmailViewModel>();

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        _ticket.Add(
            //            new SMTPEmailViewModel()
            //            {
            //                Host = Convert.ToString(dr["Host"]),
            //                UserName = Convert.ToString(dr["UserName"]),
            //                Password = Convert.ToString(dr["Password"]),
            //                Port = Convert.ToInt32(DBNull.Value.Equals(dr["Port"]) ? 0 : dr["Port"]),
            //                SSL = Convert.ToBoolean(DBNull.Value.Equals(dr["SSL"]) ? 0 : dr["SSL"]),
            //                From = Convert.ToString(dr["From"]),
            //                BCCEmail = Convert.ToString(dr["BCCEmail"])
            //            });
            //    }
            //    return _ticket;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


    }
}

