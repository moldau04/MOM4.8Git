using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using System.Web;
using System.Collections;
using BusinessEntity.Utility;

namespace DataLayer
{
    public class DL_Wage
    {
        /// <summary>
        /// GetWage (Get filtered wage list with paging and shorting)
        /// </summary>
        /// <param name="objWage, PageNumber, PageSize, SortDirection, Sortfield"></param>
        /// <returns></returns>
        public DataSet getWage(Wage objWage, int PageNumber, int PageSize, string Sortfield, int SortOrder)
        {
            SqlParameter[] parameterCheck = {
                    new SqlParameter("@PageNumber", PageNumber),
                    new SqlParameter("@PageSize", PageSize),
                    new SqlParameter("@Sortfield", Sortfield),
                    new SqlParameter("@SortOrder", SortOrder),
                    new SqlParameter("@fDesc", objWage.Name),
                    new SqlParameter("@Field", objWage.Field),
                    new SqlParameter("@Reg", objWage.Reg),
                    new SqlParameter("@OT1", objWage.OT1),
                    new SqlParameter("@OT2", objWage.OT2),
                    new SqlParameter("@NT", objWage.NT),
                    new SqlParameter("@TT", objWage.TT),
                    new SqlParameter("@Status", objWage.Status),
                };

            try
            {
                return SqlHelper.ExecuteDataset(objWage.ConnConfig, CommandType.StoredProcedure, "[payroll].[spGetWageList]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetWageTypes 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DataSet getWageTypes(Wage objWage)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objWage.ConnConfig, CommandType.StoredProcedure, "[Payroll].[getRegularWageRateNames]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetWage
        /// </summary>
        /// <param name="objWage"></param>
        /// <returns></returns>
        public DataSet getWage(Wage objWage)                                                // get Active/Inactive Wage
        {
            //string strQuery = "select id,fdesc,remarks,ID as value, fDesc as Name,ID as LabItem, fDesc as LabDesc from PRWage order by fdesc"; // where Field = 1
            string strQuery = "SELECT  PRWage.ID, PRWage.fDesc, CASE WHEN PRWage.Field =0 THEN 'No' ELSE 'Yes' END AS [Field], PRWage.Reg, PRWage.OT1, PRWage.OT2, PRWage.NT, PRWage.TT, PRWage.Count, PRWage.Status,PRWage.remarks,PRWage.ID as value, PRWage.fDesc as Name,PRWage.ID as LabItem, PRWage.fDesc as LabDesc,CASE WHEN PRWage.Status = 0 THEN 'Active' Else 'InActive' END AS sStatus FROM PRWage as PRWage  ORDER BY PRWage.fDesc ";

            if (objWage.SearchValue != "")
            {
                strQuery = "select PRWage.ID, PRWage.fDesc, CASE WHEN PRWage.Field =0 THEN 'No' ELSE 'Yes' END AS [Field], PRWage.Reg, PRWage.OT1, PRWage.OT2, PRWage.NT, PRWage.TT, PRWage.Count, PRWage.Status,PRWage.remarks,PRWage.ID as value, PRWage.fDesc as Name,PRWage.ID as LabItem, PRWage.fDesc as LabDesc,CASE WHEN PRWage.Status = 0 THEN 'Active' Else 'InActive' END AS sStatus FROM PRWage as PRWage where PRWage.fdesc like'%" + objWage.SearchValue + "%' ";
            }

            try
            {
                return SqlHelper.ExecuteDataset(objWage.ConnConfig, CommandType.Text, strQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetWageByID
        /// </summary>
        /// <param name="_objWage"></param>
        /// <returns></returns>
        public DataSet GetWageByID(Wage _objWage)
        {
            try
            {
                return _objWage.Ds = SqlHelper.ExecuteDataset(_objWage.ConnConfig, "spGetWageByID", _objWage.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DeleteWageByID
        /// </summary>
        /// <param name="_objWage"></param>
        public void DeleteWageByID(Wage _objWage)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, "spDelWageCategory", _objWage.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetAllWage
        /// </summary>
        /// <param name="_objWage"></param>
        /// <returns></returns>
        public DataSet GetAllWage(Wage _objWage) // display all active wage details
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objWage.ConnConfig, CommandType.Text, "SELECT *, ID as Wage, ID as value, fDesc, fDesc as label, ID as LabItem, fDesc as LabDesc FROM PRWage WHERE Status = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// IsWageRateIsUsed
        /// </summary>
        /// <param name="_objWage"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet IsWageRateIsUsed(Wage _objWage, Int32 userID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objWage.ConnConfig, CommandType.Text, @"SELECT top 1 fWork,WageC,* FROM TicketD where  fWork in (select ID from tblWork where fdesc=(select fuser from tblUser where id =" + userID + ") ) and WageC=" + _objWage.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddWage
        /// </summary>
        /// <param name="_objWage"></param>
        //public void AddWage(Wage _objWage, DataTable dt)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, "[payroll].[spAddWage]", _objWage.Name, _objWage.Field, _objWage.Reg, _objWage.OT1, _objWage.OT2, _objWage.TT, _objWage.FIT, _objWage.FICA, _objWage.MEDI, _objWage.FUTA, _objWage.SIT, _objWage.Vac, _objWage.WC, _objWage.Uni, _objWage.Sick, _objWage.GL, _objWage.NT, _objWage.MileageGL, _objWage.ReimGL, _objWage.ZoneGL, _objWage.Globe, _objWage.Status, _objWage.CReg, _objWage.COT, _objWage.CDT, _objWage.CNT, _objWage.CTT, _objWage.Remarks, _objWage.RegGL, _objWage.OTGL, _objWage.NTGL, _objWage.DTGL, _objWage.TTGL, 1, dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void AddWage(Wage _objWage)
        {

            SqlParameter[] parameterCheck = {
                new SqlParameter("@Name",_objWage.Name),
                new SqlParameter("@Field" , _objWage.Field),
                new SqlParameter("@Reg" , _objWage.Reg),
                new SqlParameter("@OT1" , _objWage.OT1),
                new SqlParameter("@OT2" ,_objWage.OT2),
                new SqlParameter("@TT" , _objWage.TT),
                new SqlParameter("@FIT" , _objWage.FIT),
                new SqlParameter("@FICA" , _objWage.FICA),
                new SqlParameter("@MEDI" , _objWage.MEDI),
                new SqlParameter("@FUTA" , _objWage.FUTA),
                new SqlParameter("@SIT" , _objWage.SIT),
                new SqlParameter("@Vac" , _objWage.Vac),
                new SqlParameter("@WC" , _objWage.WC),
                new SqlParameter("@Uni" , _objWage.Uni),
                new SqlParameter("@Sick" , _objWage.Sick),
                new SqlParameter("@GL" , _objWage.GL),
                new SqlParameter("@NT" , _objWage.NT),
                new SqlParameter("@MileageGL" , _objWage.MileageGL),
                new SqlParameter("@ReimGL" , _objWage.ReimGL),
                new SqlParameter("@ZoneGL" , _objWage.ZoneGL),
                new SqlParameter("@Globe" , _objWage.Globe),
                new SqlParameter("@Status" , _objWage.Status),
                new SqlParameter("@CReg" , _objWage.CReg),
                new SqlParameter("@COT" , _objWage.COT),
                new SqlParameter("@CDT" , _objWage.CDT),
                new SqlParameter("@CNT" , _objWage.CNT),
                new SqlParameter("@CTT" , _objWage.CTT),
                new SqlParameter("@Remarks" , _objWage.Remarks),
                new SqlParameter("@RegGL" , _objWage.RegGL),
                new SqlParameter("@OTGL" , _objWage.OTGL),
                new SqlParameter("@NTGL" , _objWage.NTGL),
                new SqlParameter("@DTGL" , _objWage.DTGL),
                new SqlParameter("@TTGL" , _objWage.TTGL),
                new SqlParameter("@CreatedBy" , null),
            };

            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, CommandType.StoredProcedure, "[payroll].[spAddWage]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// UpdateWage
        /// </summary>
        /// <param name="_objWage"></param>
        public void UpdateWage(Wage _objWage)
        {
            try
            {
                SqlParameter[] parameterCheck = {
                    new SqlParameter("@ID" , _objWage.ID),
                    new SqlParameter("@Name",_objWage.Name),
                    new SqlParameter("@Field" , _objWage.Field),
                    new SqlParameter("@Reg" , _objWage.Reg),
                    new SqlParameter("@OT1" , _objWage.OT1),
                    new SqlParameter("@OT2" ,_objWage.OT2),
                    new SqlParameter("@TT" , _objWage.TT),
                    new SqlParameter("@FIT" , _objWage.FIT),
                    new SqlParameter("@FICA" , _objWage.FICA),
                    new SqlParameter("@MEDI" , _objWage.MEDI),
                    new SqlParameter("@FUTA" , _objWage.FUTA),
                    new SqlParameter("@SIT" , _objWage.SIT),
                    new SqlParameter("@Vac" , _objWage.Vac),
                    new SqlParameter("@WC" , _objWage.WC),
                    new SqlParameter("@Uni" , _objWage.Uni),
                    new SqlParameter("@GL" , _objWage.GL),
                    new SqlParameter("@NT" , _objWage.NT),
                    new SqlParameter("@MileageGL" , _objWage.MileageGL),
                    new SqlParameter("@ReimGL" , _objWage.ReimGL),
                    new SqlParameter("@ZoneGL" , _objWage.ZoneGL),
                    new SqlParameter("@Globe" , _objWage.Globe),
                    new SqlParameter("@Status" , _objWage.Status),
                    new SqlParameter("@CReg" , _objWage.CReg),
                    new SqlParameter("@COT" , _objWage.COT),
                    new SqlParameter("@CDT" , _objWage.CDT),
                    new SqlParameter("@CNT" , _objWage.CNT),
                    new SqlParameter("@CTT" , _objWage.CTT),
                    new SqlParameter("@Remarks" , _objWage.Remarks),
                    new SqlParameter("@RegGL" , _objWage.RegGL),
                    new SqlParameter("@OTGL" , _objWage.OTGL),
                    new SqlParameter("@NTGL" , _objWage.NTGL),
                    new SqlParameter("@DTGL" , _objWage.DTGL),
                    new SqlParameter("@TTGL" , _objWage.TTGL),
                     new SqlParameter("@Sick" , _objWage.Sick),
                    new SqlParameter("@ModifiedBy" , null),
                    //new SqlParameter("@dtPRWageRate" , dtRate),
                    //new SqlParameter("@dtPRWageTax " , dtTax),
                
                };
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, CommandType.StoredProcedure, "[payroll].[spUpdateWage]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetWageDeduction
        /// </summary>
        /// <param name="_objPRDed"></param>
        /// <returns></returns>
        public DataSet GetWageDeduction(PRDed _objPRDed)
        {
            try
            {
                return _objPRDed.Ds = SqlHelper.ExecuteDataset(_objPRDed.ConnConfig, "spGetWageDedcuion");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DeleteWageDeductionByID
        /// </summary>
        /// <param name="_objPRDed"></param>
        public void DeleteWageDeductionByID(PRDed _objPRDed)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objPRDed.ConnConfig, "spDelWageDeduction", _objPRDed.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetWageDeductionByID
        /// </summary>
        /// <param name="_objPRDed"></param>
        /// <returns></returns>
        public DataSet GetWageDeductionByID(PRDed _objPRDed)
        {
            try
            {
                return _objPRDed.Ds = SqlHelper.ExecuteDataset(_objPRDed.ConnConfig, "spGetWageDedcuionByID", _objPRDed.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddWageDeduction
        /// </summary>
        /// <param name="_objPRDed"></param>
        public void AddWageDeduction(PRDed _objPRDed)
        {
            try
            {
                SqlParameter[] parameterCheck = {
                        new SqlParameter("@Id", _objPRDed.ID),
                        new SqlParameter("@fDesc", _objPRDed.fDesc),
                        new SqlParameter("@Type", _objPRDed.Type),
                        new SqlParameter("@ByW", _objPRDed.ByW),
                        new SqlParameter("@BasedOn", _objPRDed.BasedOn),
                        new SqlParameter("@AccruedOn", _objPRDed.AccruedOn),
                        new SqlParameter("@Count   ", _objPRDed.Count),
                        new SqlParameter("@EmpRate ", _objPRDed.EmpRate),
                        new SqlParameter("@EmpTop  ", _objPRDed.EmpTop),
                        new SqlParameter("@EmpGL   ", _objPRDed.EmpGL),
                        new SqlParameter("@CompRate", _objPRDed.CompRate),
                        new SqlParameter("@CompTop", _objPRDed.CompTop),
                        new SqlParameter("@CompGL", _objPRDed.CompGL),
                        new SqlParameter("@CompGLE", _objPRDed.CompGLE),
                        new SqlParameter("@Paid", _objPRDed.Paid),
                        new SqlParameter("@Vendor", _objPRDed.Vendor),
                        new SqlParameter("@Balance", _objPRDed.Balance),
                        new SqlParameter("@InUse", _objPRDed.InUse),
                        new SqlParameter("@Remarks", _objPRDed.Remarks),
                        new SqlParameter("@DedType", _objPRDed.DedType),
                        new SqlParameter("@Reimb", _objPRDed.Reimb),
                        new SqlParameter("@Job", _objPRDed.Job),
                        new SqlParameter("@Box", _objPRDed.Box),
                        new SqlParameter("@Frequency", _objPRDed.Frequency),
                        new SqlParameter("@Process", _objPRDed.Process),
                        new SqlParameter("@VertexDeductionId", _objPRDed.VertexDeductionId),
                        new SqlParameter("@CreatedBy", null)
                         };
                SqlHelper.ExecuteNonQuery(_objPRDed.ConnConfig, "[payroll].[spAddDeduction]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetEmployeeList
        /// </summary>
        /// <param name="_objEmp"></param>
        /// <returns></returns>
        public DataSet GetEmployeeList(Emp _objEmp)
        {
            try
            {
                return _objEmp.Ds = SqlHelper.ExecuteDataset(_objEmp.ConnConfig, "spGetEmployeeList");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetEmployeeListByID
        /// </summary>
        /// <param name="_objEmp"></param>
        /// <returns></returns>
        public DataSet GetEmployeeListByID(Emp _objEmp)
        {
            try
            {
                return _objEmp.Ds = SqlHelper.ExecuteDataset(_objEmp.ConnConfig, "spGetEmployeeListbyID", _objEmp.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// DeleteEmployeeByID
        /// </summary>
        /// <param name="_objEmp"></param>
        public void DeleteEmployeeByID(Emp _objEmp)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objEmp.ConnConfig, CommandType.Text, "DELETE FROM Emp WHERE ID=" + _objEmp.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// getPayrollTaxAccount
        /// </summary>
        /// <param name="_objWage"></param>
        /// <returns></returns>
        public DataSet getPayrollTaxAccount(Wage _objWage) // display all active wage details
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objWage.ConnConfig, CommandType.Text, "SELECT Chart.fDesc,Chart.ID FROM Chart WHERE Chart.ID=7");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddEmp
        /// </summary>
        /// <param name="_objEmp"></param>
        public void AddEmp(Emp _objEmp)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[133];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objEmp.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "fFirst";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _objEmp.fFirst;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Last";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = _objEmp.Last;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Middle";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = _objEmp.Middle;

                para[4] = new SqlParameter();
                para[4].ParameterName = "Name";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _objEmp.Name;

                para[5] = new SqlParameter();
                para[5].ParameterName = "Rol";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = _objEmp.Rol;

                para[6] = new SqlParameter();
                para[6].ParameterName = "SSN";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = _objEmp.SSN;

                para[7] = new SqlParameter();
                para[7].ParameterName = "Title";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = _objEmp.Title;

                para[8] = new SqlParameter();
                para[8].ParameterName = "Sales";
                para[8].SqlDbType = SqlDbType.Int;
                para[8].Value = _objEmp.Sales;

                para[9] = new SqlParameter();
                para[9].ParameterName = "Field";
                para[9].SqlDbType = SqlDbType.Int;
                para[9].Value = _objEmp.Field;

                para[10] = new SqlParameter();
                para[10].ParameterName = "Status";
                para[10].SqlDbType = SqlDbType.Int;
                para[10].Value = _objEmp.Status;

                para[11] = new SqlParameter();
                para[11].ParameterName = "Pager";
                para[11].SqlDbType = SqlDbType.VarChar;
                para[11].Value = _objEmp.Pager;

                para[12] = new SqlParameter();
                para[12].ParameterName = "InUse";
                para[12].SqlDbType = SqlDbType.Int;
                para[12].Value = _objEmp.InUse;

                para[13] = new SqlParameter();
                para[13].ParameterName = "PayPeriod";
                para[13].SqlDbType = SqlDbType.Int;
                para[13].Value = _objEmp.PayPeriod;

                para[14] = new SqlParameter();
                para[14].ParameterName = "DHired";
                para[14].SqlDbType = SqlDbType.DateTime;
                para[14].Value = _objEmp.DHired;

                para[15] = new SqlParameter();
                para[15].ParameterName = "DFired";
                para[15].SqlDbType = SqlDbType.DateTime;
                // para[15].Value = _objEmp.DFired;
                if (_objEmp.DFired != System.DateTime.MinValue)
                    para[15].Value = _objEmp.DFired;
                else
                    para[15].Value = DBNull.Value;

                para[16] = new SqlParameter();
                para[16].ParameterName = "DBirth";
                para[16].SqlDbType = SqlDbType.DateTime;
                para[16].Value = _objEmp.DBirth;

                para[17] = new SqlParameter();
                para[17].ParameterName = "DReview";
                para[17].SqlDbType = SqlDbType.DateTime;
                // para[17].Value = _objEmp.DReview;
                if (_objEmp.DReview != System.DateTime.MinValue)
                    para[17].Value = _objEmp.DReview;
                else
                    para[17].Value = DBNull.Value;

                para[18] = new SqlParameter();
                para[18].ParameterName = "DLast";
                para[18].SqlDbType = SqlDbType.DateTime;
                //para[18].Value = _objEmp.DLast;
                if (_objEmp.DLast != System.DateTime.MinValue)
                    para[18].Value = _objEmp.DLast;
                else
                    para[18].Value = DBNull.Value;

                para[19] = new SqlParameter();
                para[19].ParameterName = "FStatus";
                para[19].SqlDbType = SqlDbType.Int;
                para[19].Value = _objEmp.FStatus;

                para[20] = new SqlParameter();
                para[20].ParameterName = "FAllow";
                para[20].SqlDbType = SqlDbType.Int;
                para[20].Value = _objEmp.FAllow;

                para[21] = new SqlParameter();
                para[21].ParameterName = "FAdd";
                para[21].SqlDbType = SqlDbType.Decimal;
                para[21].Value = _objEmp.FAdd;

                para[22] = new SqlParameter();
                para[22].ParameterName = "SStatus";
                para[22].SqlDbType = SqlDbType.Int;
                para[22].Value = _objEmp.SStatus;

                para[23] = new SqlParameter();
                para[23].ParameterName = "SAllow";
                para[23].SqlDbType = SqlDbType.Int;
                para[23].Value = _objEmp.SAllow;

                para[24] = new SqlParameter();
                para[24].ParameterName = "SAdd";
                para[24].SqlDbType = SqlDbType.Decimal;
                para[24].Value = _objEmp.SAdd;

                para[25] = new SqlParameter();
                para[25].ParameterName = "CallSign";
                para[25].SqlDbType = SqlDbType.VarChar;
                para[25].Value = _objEmp.CallSign;

                para[26] = new SqlParameter();
                para[26].ParameterName = "VRate";
                para[26].SqlDbType = SqlDbType.Decimal;
                para[26].Value = _objEmp.VRate;

                para[27] = new SqlParameter();
                para[27].ParameterName = "VBase";
                para[27].SqlDbType = SqlDbType.Int;
                para[27].Value = _objEmp.VBase;

                para[28] = new SqlParameter();
                para[28].ParameterName = "VLast";
                para[28].SqlDbType = SqlDbType.Decimal;
                para[28].Value = _objEmp.VLast;

                para[29] = new SqlParameter();
                para[29].ParameterName = "VThis";
                para[29].SqlDbType = SqlDbType.Decimal;
                para[29].Value = _objEmp.VThis;

                para[30] = new SqlParameter();
                para[30].ParameterName = "Sick";
                para[30].SqlDbType = SqlDbType.Decimal;
                para[30].Value = _objEmp.Sick;

                para[31] = new SqlParameter();
                para[31].ParameterName = "PMethod";
                para[31].SqlDbType = SqlDbType.Int;
                para[31].Value = _objEmp.PMethod;

                para[32] = new SqlParameter();
                para[32].ParameterName = "PFixed";
                para[32].SqlDbType = SqlDbType.Int;
                if (_objEmp.PFixed == 99)
                {
                    para[32].Value = DBNull.Value;
                }
                else
                {
                    para[32].Value = _objEmp.PFixed;
                }

                para[33] = new SqlParameter();
                para[33].ParameterName = "PHour";
                para[33].SqlDbType = SqlDbType.Decimal;
                para[33].Value = _objEmp.PHour;

                para[34] = new SqlParameter();
                para[34].ParameterName = "LName";
                para[34].SqlDbType = SqlDbType.Int;
                para[34].Value = _objEmp.LName;

                para[35] = new SqlParameter();
                para[35].ParameterName = "LStatus";
                para[35].SqlDbType = SqlDbType.Int;
                para[35].Value = _objEmp.LStatus;

                para[36] = new SqlParameter();
                para[36].ParameterName = "LAllow";
                para[36].SqlDbType = SqlDbType.Int;
                para[36].Value = _objEmp.LAllow;

                para[37] = new SqlParameter();
                para[37].ParameterName = "PRTaxE";
                para[37].SqlDbType = SqlDbType.Int;
                para[37].Value = _objEmp.PRTaxE;

                para[38] = new SqlParameter();
                para[38].ParameterName = "State";
                para[38].SqlDbType = SqlDbType.VarChar;
                para[38].Value = _objEmp.State;

                para[39] = new SqlParameter();
                para[39].ParameterName = "Salary";
                para[39].SqlDbType = SqlDbType.Int;
                para[39].Value = _objEmp.Salary;

                para[40] = new SqlParameter();
                para[40].ParameterName = "SalaryF";
                para[40].SqlDbType = SqlDbType.Int;
                para[40].Value = _objEmp.SalaryF;

                para[41] = new SqlParameter();
                para[41].ParameterName = "SalaryGL";
                para[41].SqlDbType = SqlDbType.Int;
                para[41].Value = _objEmp.SalaryGL;

                para[42] = new SqlParameter();
                para[42].ParameterName = "fWork";
                para[42].SqlDbType = SqlDbType.Int;
                para[42].Value = _objEmp.fWork;

                para[43] = new SqlParameter();
                para[43].ParameterName = "NPaid";
                para[43].SqlDbType = SqlDbType.Decimal;
                para[43].Value = _objEmp.NPaid;

                para[44] = new SqlParameter();
                para[44].ParameterName = "Balance";
                para[44].SqlDbType = SqlDbType.Decimal;
                para[44].Value = _objEmp.Balance;

                para[45] = new SqlParameter();
                para[45].ParameterName = "PBRate";
                para[45].SqlDbType = SqlDbType.Decimal;
                para[45].Value = _objEmp.PBRate;

                para[46] = new SqlParameter();
                para[46].ParameterName = "FITYTD";
                para[46].SqlDbType = SqlDbType.Decimal;
                para[46].Value = _objEmp.FITYTD;

                para[47] = new SqlParameter();
                para[47].ParameterName = "FICAYTD";
                para[47].SqlDbType = SqlDbType.Decimal;
                para[47].Value = _objEmp.FICAYTD;

                para[48] = new SqlParameter();
                para[48].ParameterName = "MEDIYTD";
                para[48].SqlDbType = SqlDbType.Decimal;
                para[48].Value = _objEmp.MEDIYTD;

                para[49] = new SqlParameter();
                para[49].ParameterName = "FUTAYTD";
                para[49].SqlDbType = SqlDbType.Decimal;
                para[49].Value = _objEmp.FUTAYTD;

                para[50] = new SqlParameter();
                para[50].ParameterName = "SITYTD";
                para[50].SqlDbType = SqlDbType.Decimal;
                para[50].Value = _objEmp.SITYTD;

                para[51] = new SqlParameter();
                para[51].ParameterName = "LocalYTD";
                para[51].SqlDbType = SqlDbType.Decimal;
                para[51].Value = _objEmp.LocalYTD;

                para[52] = new SqlParameter();
                para[52].ParameterName = "BonusYTD";
                para[52].SqlDbType = SqlDbType.Decimal;
                para[52].Value = _objEmp.BonusYTD;

                para[53] = new SqlParameter();
                para[53].ParameterName = "HolH";
                para[53].SqlDbType = SqlDbType.Decimal;
                para[53].Value = _objEmp.HolH;

                para[54] = new SqlParameter();
                para[54].ParameterName = "HolYTD";
                para[54].SqlDbType = SqlDbType.Decimal;
                para[54].Value = _objEmp.HolYTD;

                para[55] = new SqlParameter();
                para[55].ParameterName = "VacH";
                para[55].SqlDbType = SqlDbType.Decimal;
                para[55].Value = _objEmp.VacH;

                para[56] = new SqlParameter();
                para[56].ParameterName = "VacYTD";
                para[56].SqlDbType = SqlDbType.Decimal;
                para[56].Value = _objEmp.VacYTD;

                para[57] = new SqlParameter();
                para[57].ParameterName = "ZoneH";
                para[57].SqlDbType = SqlDbType.Decimal;
                para[57].Value = _objEmp.ZoneH;

                para[58] = new SqlParameter();
                para[58].ParameterName = "ZoneYTD";
                para[58].SqlDbType = SqlDbType.Decimal;
                para[58].Value = _objEmp.ZoneYTD;

                para[59] = new SqlParameter();
                para[59].ParameterName = "ReimbYTD";
                para[59].SqlDbType = SqlDbType.Decimal;
                para[59].Value = _objEmp.ReimbYTD;

                para[60] = new SqlParameter();
                para[60].ParameterName = "MileH";
                para[60].SqlDbType = SqlDbType.Decimal;
                para[60].Value = _objEmp.MileH;

                para[61] = new SqlParameter();
                para[61].ParameterName = "MileYTD";
                para[61].SqlDbType = SqlDbType.Decimal;
                para[61].Value = _objEmp.MileYTD;

                para[62] = new SqlParameter();
                para[62].ParameterName = "Race";
                para[62].SqlDbType = SqlDbType.VarChar;
                para[62].Value = _objEmp.Race;

                para[63] = new SqlParameter();
                para[63].ParameterName = "Sex";
                para[63].SqlDbType = SqlDbType.VarChar;
                para[63].Value = _objEmp.Sex;

                para[64] = new SqlParameter();
                para[64].ParameterName = "Ref";
                para[64].SqlDbType = SqlDbType.VarChar;
                para[64].Value = _objEmp.Ref;

                para[65] = new SqlParameter();
                para[65].ParameterName = "ACH";
                para[65].SqlDbType = SqlDbType.Int;
                para[65].Value = _objEmp.ACH;

                para[66] = new SqlParameter();
                para[66].ParameterName = "ACHType";
                para[66].SqlDbType = SqlDbType.Int;
                para[66].Value = _objEmp.ACHType;

                para[67] = new SqlParameter();
                para[67].ParameterName = "ACHRoute";
                para[67].SqlDbType = SqlDbType.VarChar;
                para[67].Value = _objEmp.ACHRoute;

                para[68] = new SqlParameter();
                para[68].ParameterName = "ACHBank";
                para[68].SqlDbType = SqlDbType.VarChar;
                para[68].Value = _objEmp.ACHBank;

                para[69] = new SqlParameter();
                para[69].ParameterName = "Anniversary";
                para[69].SqlDbType = SqlDbType.DateTime;
                //para[69].Value = _objEmp.Anniversary;
                if (_objEmp.Anniversary != System.DateTime.MinValue)
                    para[69].Value = _objEmp.Anniversary;
                else
                    para[69].Value = DBNull.Value;

                para[70] = new SqlParameter();
                para[70].ParameterName = "Level";
                para[70].SqlDbType = SqlDbType.Int;
                para[70].Value = _objEmp.Level;

                para[71] = new SqlParameter();
                para[71].ParameterName = "WageCat";
                para[71].SqlDbType = SqlDbType.Int;
                para[71].Value = _objEmp.WageCat;

                para[72] = new SqlParameter();
                para[72].ParameterName = "DSenior";
                para[72].SqlDbType = SqlDbType.DateTime;
                para[72].Value = _objEmp.DSenior;

                para[73] = new SqlParameter();
                para[73].ParameterName = "PRWBR";
                para[73].SqlDbType = SqlDbType.Int;
                para[73].Value = _objEmp.PRWBR;

                para[74] = new SqlParameter();
                para[74].ParameterName = "PDASerialNumber_1";
                para[74].SqlDbType = SqlDbType.VarChar;
                para[74].Value = _objEmp.PDASerialNumber_1;

                para[75] = new SqlParameter();
                para[75].ParameterName = "StatusChange";
                para[75].SqlDbType = SqlDbType.Int;
                para[75].Value = _objEmp.StatusChange;

                para[76] = new SqlParameter();
                para[76].ParameterName = "SCDate";
                para[76].SqlDbType = SqlDbType.DateTime;
                para[76].Value = _objEmp.SCDate;

                para[77] = new SqlParameter();
                para[77].ParameterName = "SCReason";
                para[77].SqlDbType = SqlDbType.VarChar;
                para[77].Value = _objEmp.SCReason;

                para[78] = new SqlParameter();
                para[78].ParameterName = "DemoChange";
                para[78].SqlDbType = SqlDbType.Int;
                para[78].Value = _objEmp.DemoChange;

                para[79] = new SqlParameter();
                para[79].ParameterName = "Language";
                para[79].SqlDbType = SqlDbType.VarChar;
                para[79].Value = _objEmp.Language;

                para[80] = new SqlParameter();
                para[80].ParameterName = "TicketD";
                para[80].SqlDbType = SqlDbType.Int;
                para[80].Value = _objEmp.TicketD;

                para[81] = new SqlParameter();
                para[81].ParameterName = "Custom1";
                para[81].SqlDbType = SqlDbType.VarChar;
                para[81].Value = _objEmp.Custom1;

                para[82] = new SqlParameter();
                para[82].ParameterName = "Custom2";
                para[82].SqlDbType = SqlDbType.VarChar;
                para[82].Value = _objEmp.Custom2;

                para[83] = new SqlParameter();
                para[83].ParameterName = "Custom3";
                para[83].SqlDbType = SqlDbType.VarChar;
                para[83].Value = _objEmp.Custom3;

                para[84] = new SqlParameter();
                para[84].ParameterName = "Custom4";
                para[84].SqlDbType = SqlDbType.VarChar;
                para[84].Value = _objEmp.Custom4;

                para[85] = new SqlParameter();
                para[85].ParameterName = "Custom5";
                para[85].SqlDbType = SqlDbType.VarChar;
                para[85].Value = _objEmp.Custom5;

                para[86] = new SqlParameter();
                para[86].ParameterName = "DDType";
                para[86].SqlDbType = SqlDbType.Int;
                para[86].Value = _objEmp.DDType;

                para[87] = new SqlParameter();
                para[87].ParameterName = "DDRate";
                para[87].SqlDbType = SqlDbType.Decimal;
                para[87].Value = _objEmp.DDRate;

                para[88] = new SqlParameter();
                para[88].ParameterName = "ACHType2";
                para[88].SqlDbType = SqlDbType.Int;
                para[88].Value = _objEmp.ACHType2;

                para[89] = new SqlParameter();
                para[89].ParameterName = "ACHRoute2";
                para[89].SqlDbType = SqlDbType.VarChar;
                para[89].Value = _objEmp.ACHRoute2;

                para[90] = new SqlParameter();
                para[90].ParameterName = "ACHBank2";
                para[90].SqlDbType = SqlDbType.VarChar;
                para[90].Value = _objEmp.ACHBank2;

                para[91] = new SqlParameter();
                para[91].ParameterName = "BillRate";
                para[91].SqlDbType = SqlDbType.Decimal;
                para[91].Value = _objEmp.BillRate;

                para[92] = new SqlParameter();
                para[92].ParameterName = "BMSales";
                para[92].SqlDbType = SqlDbType.Decimal;
                para[92].Value = _objEmp.BMSales;

                para[93] = new SqlParameter();
                para[93].ParameterName = "BMInvAve";
                para[93].SqlDbType = SqlDbType.Decimal;
                para[93].Value = _objEmp.BMInvAve;

                para[94] = new SqlParameter();
                para[94].ParameterName = "BMClosing";
                para[94].SqlDbType = SqlDbType.Decimal;
                para[94].Value = _objEmp.BMClosing;

                para[95] = new SqlParameter();
                para[95].ParameterName = "BMBillEff";
                para[95].SqlDbType = SqlDbType.Decimal;
                para[95].Value = _objEmp.BMBillEff;

                para[96] = new SqlParameter();
                para[96].ParameterName = "BMProdEff";
                para[96].SqlDbType = SqlDbType.Decimal;
                para[96].Value = _objEmp.BMProdEff;

                para[97] = new SqlParameter();
                para[97].ParameterName = "BMAveTask";
                para[97].SqlDbType = SqlDbType.Int;
                para[97].Value = _objEmp.BMAveTask;

                para[98] = new SqlParameter();
                para[98].ParameterName = "BMCustom1";
                para[98].SqlDbType = SqlDbType.Int;
                para[98].Value = _objEmp.BMCustom1;

                para[99] = new SqlParameter();
                para[99].ParameterName = "BMCustom2";
                para[99].SqlDbType = SqlDbType.Int;
                para[99].Value = _objEmp.BMCustom2;

                para[100] = new SqlParameter();
                para[100].ParameterName = "BMCustom3";
                para[100].SqlDbType = SqlDbType.Int;
                para[100].Value = _objEmp.BMCustom3;

                para[101] = new SqlParameter();
                para[101].ParameterName = "BMCustom4";
                para[101].SqlDbType = SqlDbType.Int;
                para[101].Value = _objEmp.BMCustom4;

                para[102] = new SqlParameter();
                para[102].ParameterName = "BMCustom5";
                para[102].SqlDbType = SqlDbType.Int;
                para[102].Value = _objEmp.BMCustom5;

                para[103] = new SqlParameter();
                para[103].ParameterName = "TaxCodeNR";
                para[103].SqlDbType = SqlDbType.VarChar;
                para[103].Value = _objEmp.TaxCodeNR;

                para[104] = new SqlParameter();
                para[104].ParameterName = "TaxCodeR";
                para[104].SqlDbType = SqlDbType.VarChar;
                para[104].Value = _objEmp.TaxCodeR;

                para[105] = new SqlParameter();
                para[105].ParameterName = "DeviceID";
                para[105].SqlDbType = SqlDbType.VarChar;
                para[105].Value = _objEmp.DeviceID;

                para[106] = new SqlParameter();
                para[106].ParameterName = "MileageRate";
                para[106].SqlDbType = SqlDbType.Decimal;
                para[106].Value = _objEmp.MileageRate;

                para[107] = new SqlParameter();
                para[107].ParameterName = "Import1";
                para[107].SqlDbType = SqlDbType.Int;
                para[107].Value = _objEmp.Import1;

                para[108] = new SqlParameter();
                para[108].ParameterName = "MSDeviceId";
                para[108].SqlDbType = SqlDbType.VarChar;
                para[108].Value = _objEmp.MSDeviceId;

                para[109] = new SqlParameter();
                para[109].ParameterName = "TechnicianBio";
                para[109].SqlDbType = SqlDbType.VarChar;
                para[109].Value = _objEmp.TechnicianBio;

                para[110] = new SqlParameter();
                para[110].ParameterName = "PayPortalPassword";
                para[110].SqlDbType = SqlDbType.VarChar;
                para[110].Value = _objEmp.PayPortalPassword;

                para[111] = new SqlParameter();
                para[111].ParameterName = "SickRate";
                para[111].SqlDbType = SqlDbType.Decimal;
                para[111].Value = _objEmp.SickRate;

                para[112] = new SqlParameter();
                para[112].ParameterName = "SickAccrued";
                para[112].SqlDbType = SqlDbType.Decimal;
                para[112].Value = _objEmp.SickAccrued;

                para[113] = new SqlParameter();
                para[113].ParameterName = "SickUsed";
                para[113].SqlDbType = SqlDbType.Decimal;
                para[113].Value = _objEmp.SickUsed;

                para[114] = new SqlParameter();
                para[114].ParameterName = "SickYTD";
                para[114].SqlDbType = SqlDbType.Decimal;
                para[114].Value = _objEmp.SickYTD;

                para[115] = new SqlParameter();
                para[115].ParameterName = "VacAccrued";
                para[115].SqlDbType = SqlDbType.Decimal;
                para[115].Value = _objEmp.VacAccrued;

                para[116] = new SqlParameter();
                para[116].ParameterName = "SCounty";
                para[116].SqlDbType = SqlDbType.Int;
                para[116].Value = _objEmp.SCounty;

                para[117] = new SqlParameter();
                para[117].ParameterName = "PDASerialNumber";
                para[117].SqlDbType = SqlDbType.VarChar;
                para[117].Value = _objEmp.PDASerialNumber;

                para[118] = new SqlParameter();
                para[118].ParameterName = "City";
                para[118].SqlDbType = SqlDbType.VarChar;
                para[118].Value = _objEmp.City;

                para[119] = new SqlParameter();
                para[119].ParameterName = "Zip";
                para[119].SqlDbType = SqlDbType.VarChar;
                para[119].Value = _objEmp.Zip;

                para[120] = new SqlParameter();
                para[120].ParameterName = "Tel";
                para[120].SqlDbType = SqlDbType.VarChar;
                para[120].Value = _objEmp.Tel;

                para[121] = new SqlParameter();
                para[121].ParameterName = "Address";
                para[121].SqlDbType = SqlDbType.VarChar;
                para[121].Value = _objEmp.Address;

                para[122] = new SqlParameter();
                para[122].ParameterName = "Email";
                para[122].SqlDbType = SqlDbType.VarChar;
                para[122].Value = _objEmp.Email;

                para[123] = new SqlParameter();
                para[123].ParameterName = "Cell";
                para[123].SqlDbType = SqlDbType.VarChar;
                para[123].Value = _objEmp.Cell;

                para[124] = new SqlParameter();
                para[124].ParameterName = "Remarks";
                para[124].SqlDbType = SqlDbType.VarChar;
                para[124].Value = _objEmp.Remarks;

                para[125] = new SqlParameter();
                para[125].ParameterName = "Type";
                para[125].SqlDbType = SqlDbType.Int;
                para[125].Value = _objEmp.Type;

                para[126] = new SqlParameter();
                para[126].ParameterName = "Contact";
                para[126].SqlDbType = SqlDbType.VarChar;
                para[126].Value = _objEmp.Contact;

                para[127] = new SqlParameter();
                para[127].ParameterName = "Website";
                para[127].SqlDbType = SqlDbType.VarChar;
                para[127].Value = _objEmp.Website;

                para[128] = new SqlParameter();
                para[128].ParameterName = "Fax";
                para[128].SqlDbType = SqlDbType.VarChar;
                para[128].Value = _objEmp.Fax;

                para[129] = new SqlParameter();
                para[129].ParameterName = "Country";
                para[129].SqlDbType = SqlDbType.VarChar;
                para[129].Value = _objEmp.Country;

                para[130] = new SqlParameter();
                para[130].ParameterName = "dtWageCategory";
                para[130].SqlDbType = SqlDbType.Structured;
                para[130].Value = _objEmp.dtWageCategory;

                para[131] = new SqlParameter();
                para[131].ParameterName = "dtWageDeduction";
                para[131].SqlDbType = SqlDbType.Structured;
                para[131].Value = _objEmp.dtWageDeduction;

                para[132] = new SqlParameter();
                para[132].ParameterName = "dtOtherIncome";
                para[132].SqlDbType = SqlDbType.Structured;
                para[132].Value = _objEmp.dtOtherIncome;

                SqlHelper.ExecuteNonQuery(_objEmp.ConnConfig, "spAddEmp", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// UpdateEmp
        /// </summary>
        /// <param name="_objEmp"></param>
        public void UpdateEmp(Emp _objEmp)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[136];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objEmp.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "fFirst";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _objEmp.fFirst;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Last";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = _objEmp.Last;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Middle";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = _objEmp.Middle;

                para[4] = new SqlParameter();
                para[4].ParameterName = "Name";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _objEmp.Name;

                para[5] = new SqlParameter();
                para[5].ParameterName = "Rol";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = _objEmp.Rol;

                para[6] = new SqlParameter();
                para[6].ParameterName = "SSN";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = _objEmp.SSN;

                para[7] = new SqlParameter();
                para[7].ParameterName = "Title";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = _objEmp.Title;

                para[8] = new SqlParameter();
                para[8].ParameterName = "Sales";
                para[8].SqlDbType = SqlDbType.Int;
                para[8].Value = _objEmp.Sales;

                para[9] = new SqlParameter();
                para[9].ParameterName = "Field";
                para[9].SqlDbType = SqlDbType.Int;
                para[9].Value = _objEmp.Field;

                para[10] = new SqlParameter();
                para[10].ParameterName = "Status";
                para[10].SqlDbType = SqlDbType.Int;
                para[10].Value = _objEmp.Status;

                para[11] = new SqlParameter();
                para[11].ParameterName = "Pager";
                para[11].SqlDbType = SqlDbType.VarChar;
                para[11].Value = _objEmp.Pager;

                para[12] = new SqlParameter();
                para[12].ParameterName = "InUse";
                para[12].SqlDbType = SqlDbType.Int;
                para[12].Value = _objEmp.InUse;

                para[13] = new SqlParameter();
                para[13].ParameterName = "PayPeriod";
                para[13].SqlDbType = SqlDbType.Int;
                para[13].Value = _objEmp.PayPeriod;

                para[14] = new SqlParameter();
                para[14].ParameterName = "DHired";
                para[14].SqlDbType = SqlDbType.DateTime;
                para[14].Value = _objEmp.DHired;

                para[15] = new SqlParameter();
                para[15].ParameterName = "DFired";
                para[15].SqlDbType = SqlDbType.DateTime;
                //para[15].Value = _objEmp.DFired;
                if (_objEmp.DFired != System.DateTime.MinValue)
                    para[15].Value = _objEmp.DFired;
                else
                    para[15].Value = DBNull.Value;

                para[16] = new SqlParameter();
                para[16].ParameterName = "DBirth";
                para[16].SqlDbType = SqlDbType.DateTime;
                para[16].Value = _objEmp.DBirth;

                para[17] = new SqlParameter();
                para[17].ParameterName = "DReview";
                para[17].SqlDbType = SqlDbType.DateTime;
                //para[17].Value = _objEmp.DReview;
                if (_objEmp.DReview != System.DateTime.MinValue)
                    para[17].Value = _objEmp.DReview;
                else
                    para[17].Value = DBNull.Value;

                para[18] = new SqlParameter();
                para[18].ParameterName = "DLast";
                para[18].SqlDbType = SqlDbType.DateTime;
                //para[18].Value = _objEmp.DLast;
                if (_objEmp.DLast != System.DateTime.MinValue)
                    para[18].Value = _objEmp.DLast;
                else
                    para[18].Value = DBNull.Value;

                para[19] = new SqlParameter();
                para[19].ParameterName = "FStatus";
                para[19].SqlDbType = SqlDbType.Int;
                para[19].Value = _objEmp.FStatus;

                para[20] = new SqlParameter();
                para[20].ParameterName = "FAllow";
                para[20].SqlDbType = SqlDbType.Int;
                para[20].Value = _objEmp.FAllow;

                para[21] = new SqlParameter();
                para[21].ParameterName = "FAdd";
                para[21].SqlDbType = SqlDbType.Decimal;
                para[21].Value = _objEmp.FAdd;

                para[22] = new SqlParameter();
                para[22].ParameterName = "SStatus";
                para[22].SqlDbType = SqlDbType.Int;
                para[22].Value = _objEmp.SStatus;

                para[23] = new SqlParameter();
                para[23].ParameterName = "SAllow";
                para[23].SqlDbType = SqlDbType.Int;
                para[23].Value = _objEmp.SAllow;

                para[24] = new SqlParameter();
                para[24].ParameterName = "SAdd";
                para[24].SqlDbType = SqlDbType.Decimal;
                para[24].Value = _objEmp.SAdd;

                para[25] = new SqlParameter();
                para[25].ParameterName = "CallSign";
                para[25].SqlDbType = SqlDbType.VarChar;
                para[25].Value = _objEmp.CallSign;

                para[26] = new SqlParameter();
                para[26].ParameterName = "VRate";
                para[26].SqlDbType = SqlDbType.Decimal;
                para[26].Value = _objEmp.VRate;

                para[27] = new SqlParameter();
                para[27].ParameterName = "VBase";
                para[27].SqlDbType = SqlDbType.Int;
                para[27].Value = _objEmp.VBase;

                para[28] = new SqlParameter();
                para[28].ParameterName = "VLast";
                para[28].SqlDbType = SqlDbType.Decimal;
                para[28].Value = _objEmp.VLast;

                para[29] = new SqlParameter();
                para[29].ParameterName = "VThis";
                para[29].SqlDbType = SqlDbType.Decimal;
                para[29].Value = _objEmp.VThis;

                para[30] = new SqlParameter();
                para[30].ParameterName = "Sick";
                para[30].SqlDbType = SqlDbType.Decimal;
                para[30].Value = _objEmp.Sick;

                para[31] = new SqlParameter();
                para[31].ParameterName = "PMethod";
                para[31].SqlDbType = SqlDbType.Int;
                para[31].Value = _objEmp.PMethod;

                para[32] = new SqlParameter();
                para[32].ParameterName = "PFixed";
                para[32].SqlDbType = SqlDbType.Int;
                if (_objEmp.PFixed == 99)
                {
                    para[32].Value = DBNull.Value;
                }
                else
                {
                    para[32].Value = _objEmp.PFixed;
                }

                para[33] = new SqlParameter();
                para[33].ParameterName = "PHour";
                para[33].SqlDbType = SqlDbType.Decimal;
                para[33].Value = _objEmp.PHour;

                para[34] = new SqlParameter();
                para[34].ParameterName = "LName";
                para[34].SqlDbType = SqlDbType.Int;
                para[34].Value = _objEmp.LName;

                para[35] = new SqlParameter();
                para[35].ParameterName = "LStatus";
                para[35].SqlDbType = SqlDbType.Int;
                para[35].Value = _objEmp.LStatus;

                para[36] = new SqlParameter();
                para[36].ParameterName = "LAllow";
                para[36].SqlDbType = SqlDbType.Int;
                para[36].Value = _objEmp.LAllow;

                para[37] = new SqlParameter();
                para[37].ParameterName = "PRTaxE";
                para[37].SqlDbType = SqlDbType.Int;
                para[37].Value = _objEmp.PRTaxE;

                para[38] = new SqlParameter();
                para[38].ParameterName = "State";
                para[38].SqlDbType = SqlDbType.VarChar;
                para[38].Value = _objEmp.State;

                para[39] = new SqlParameter();
                para[39].ParameterName = "Salary";
                para[39].SqlDbType = SqlDbType.Int;
                para[39].Value = _objEmp.Salary;

                para[40] = new SqlParameter();
                para[40].ParameterName = "SalaryF";
                para[40].SqlDbType = SqlDbType.Int;
                para[40].Value = _objEmp.SalaryF;

                para[41] = new SqlParameter();
                para[41].ParameterName = "SalaryGL";
                para[41].SqlDbType = SqlDbType.Int;
                para[41].Value = _objEmp.SalaryGL;

                para[42] = new SqlParameter();
                para[42].ParameterName = "fWork";
                para[42].SqlDbType = SqlDbType.Int;
                para[42].Value = _objEmp.fWork;

                para[43] = new SqlParameter();
                para[43].ParameterName = "NPaid";
                para[43].SqlDbType = SqlDbType.Decimal;
                para[43].Value = _objEmp.NPaid;

                para[44] = new SqlParameter();
                para[44].ParameterName = "Balance";
                para[44].SqlDbType = SqlDbType.Decimal;
                para[44].Value = _objEmp.Balance;

                para[45] = new SqlParameter();
                para[45].ParameterName = "PBRate";
                para[45].SqlDbType = SqlDbType.Decimal;
                para[45].Value = _objEmp.PBRate;

                para[46] = new SqlParameter();
                para[46].ParameterName = "FITYTD";
                para[46].SqlDbType = SqlDbType.Decimal;
                para[46].Value = _objEmp.FITYTD;

                para[47] = new SqlParameter();
                para[47].ParameterName = "FICAYTD";
                para[47].SqlDbType = SqlDbType.Decimal;
                para[47].Value = _objEmp.FICAYTD;

                para[48] = new SqlParameter();
                para[48].ParameterName = "MEDIYTD";
                para[48].SqlDbType = SqlDbType.Decimal;
                para[48].Value = _objEmp.MEDIYTD;

                para[49] = new SqlParameter();
                para[49].ParameterName = "FUTAYTD";
                para[49].SqlDbType = SqlDbType.Decimal;
                para[49].Value = _objEmp.FUTAYTD;

                para[50] = new SqlParameter();
                para[50].ParameterName = "SITYTD";
                para[50].SqlDbType = SqlDbType.Decimal;
                para[50].Value = _objEmp.SITYTD;

                para[51] = new SqlParameter();
                para[51].ParameterName = "LocalYTD";
                para[51].SqlDbType = SqlDbType.Decimal;
                para[51].Value = _objEmp.LocalYTD;

                para[52] = new SqlParameter();
                para[52].ParameterName = "BonusYTD";
                para[52].SqlDbType = SqlDbType.Decimal;
                para[52].Value = _objEmp.BonusYTD;

                para[53] = new SqlParameter();
                para[53].ParameterName = "HolH";
                para[53].SqlDbType = SqlDbType.Decimal;
                para[53].Value = _objEmp.HolH;

                para[54] = new SqlParameter();
                para[54].ParameterName = "HolYTD";
                para[54].SqlDbType = SqlDbType.Decimal;
                para[54].Value = _objEmp.HolYTD;

                para[55] = new SqlParameter();
                para[55].ParameterName = "VacH";
                para[55].SqlDbType = SqlDbType.Decimal;
                para[55].Value = _objEmp.VacH;

                para[56] = new SqlParameter();
                para[56].ParameterName = "VacYTD";
                para[56].SqlDbType = SqlDbType.Decimal;
                para[56].Value = _objEmp.VacYTD;

                para[57] = new SqlParameter();
                para[57].ParameterName = "ZoneH";
                para[57].SqlDbType = SqlDbType.Decimal;
                para[57].Value = _objEmp.ZoneH;

                para[58] = new SqlParameter();
                para[58].ParameterName = "ZoneYTD";
                para[58].SqlDbType = SqlDbType.Decimal;
                para[58].Value = _objEmp.ZoneYTD;

                para[59] = new SqlParameter();
                para[59].ParameterName = "ReimbYTD";
                para[59].SqlDbType = SqlDbType.Decimal;
                para[59].Value = _objEmp.ReimbYTD;

                para[60] = new SqlParameter();
                para[60].ParameterName = "MileH";
                para[60].SqlDbType = SqlDbType.Decimal;
                para[60].Value = _objEmp.MileH;

                para[61] = new SqlParameter();
                para[61].ParameterName = "MileYTD";
                para[61].SqlDbType = SqlDbType.Decimal;
                para[61].Value = _objEmp.MileYTD;

                para[62] = new SqlParameter();
                para[62].ParameterName = "Race";
                para[62].SqlDbType = SqlDbType.VarChar;
                para[62].Value = _objEmp.Race;

                para[63] = new SqlParameter();
                para[63].ParameterName = "Sex";
                para[63].SqlDbType = SqlDbType.VarChar;
                para[63].Value = _objEmp.Sex;

                para[64] = new SqlParameter();
                para[64].ParameterName = "Ref";
                para[64].SqlDbType = SqlDbType.VarChar;
                para[64].Value = _objEmp.Ref;

                para[65] = new SqlParameter();
                para[65].ParameterName = "ACH";
                para[65].SqlDbType = SqlDbType.Int;
                para[65].Value = _objEmp.ACH;

                para[66] = new SqlParameter();
                para[66].ParameterName = "ACHType";
                para[66].SqlDbType = SqlDbType.Int;
                para[66].Value = _objEmp.ACHType;

                para[67] = new SqlParameter();
                para[67].ParameterName = "ACHRoute";
                para[67].SqlDbType = SqlDbType.VarChar;
                para[67].Value = _objEmp.ACHRoute;

                para[68] = new SqlParameter();
                para[68].ParameterName = "ACHBank";
                para[68].SqlDbType = SqlDbType.VarChar;
                para[68].Value = _objEmp.ACHBank;

                para[69] = new SqlParameter();
                para[69].ParameterName = "Anniversary";
                para[69].SqlDbType = SqlDbType.DateTime;
                // para[69].Value = _objEmp.Anniversary;
                if (_objEmp.Anniversary != System.DateTime.MinValue)
                    para[69].Value = _objEmp.Anniversary;
                else
                    para[69].Value = DBNull.Value;

                para[70] = new SqlParameter();
                para[70].ParameterName = "Level";
                para[70].SqlDbType = SqlDbType.Int;
                para[70].Value = _objEmp.Level;

                para[71] = new SqlParameter();
                para[71].ParameterName = "WageCat";
                para[71].SqlDbType = SqlDbType.Int;
                para[71].Value = _objEmp.WageCat;

                para[72] = new SqlParameter();
                para[72].ParameterName = "DSenior";
                para[72].SqlDbType = SqlDbType.DateTime;
                para[72].Value = _objEmp.DSenior;

                para[73] = new SqlParameter();
                para[73].ParameterName = "PRWBR";
                para[73].SqlDbType = SqlDbType.Int;
                para[73].Value = _objEmp.PRWBR;

                para[74] = new SqlParameter();
                para[74].ParameterName = "PDASerialNumber_1";
                para[74].SqlDbType = SqlDbType.VarChar;
                para[74].Value = _objEmp.PDASerialNumber_1;

                para[75] = new SqlParameter();
                para[75].ParameterName = "StatusChange";
                para[75].SqlDbType = SqlDbType.Int;
                para[75].Value = _objEmp.StatusChange;

                para[76] = new SqlParameter();
                para[76].ParameterName = "SCDate";
                para[76].SqlDbType = SqlDbType.DateTime;
                para[76].Value = _objEmp.SCDate;

                para[77] = new SqlParameter();
                para[77].ParameterName = "SCReason";
                para[77].SqlDbType = SqlDbType.VarChar;
                para[77].Value = _objEmp.SCReason;

                para[78] = new SqlParameter();
                para[78].ParameterName = "DemoChange";
                para[78].SqlDbType = SqlDbType.Int;
                para[78].Value = _objEmp.DemoChange;

                para[79] = new SqlParameter();
                para[79].ParameterName = "Language";
                para[79].SqlDbType = SqlDbType.VarChar;
                para[79].Value = _objEmp.Language;

                para[80] = new SqlParameter();
                para[80].ParameterName = "TicketD";
                para[80].SqlDbType = SqlDbType.Int;
                para[80].Value = _objEmp.TicketD;

                para[81] = new SqlParameter();
                para[81].ParameterName = "Custom1";
                para[81].SqlDbType = SqlDbType.VarChar;
                para[81].Value = _objEmp.Custom1;

                para[82] = new SqlParameter();
                para[82].ParameterName = "Custom2";
                para[82].SqlDbType = SqlDbType.VarChar;
                para[82].Value = _objEmp.Custom2;

                para[83] = new SqlParameter();
                para[83].ParameterName = "Custom3";
                para[83].SqlDbType = SqlDbType.VarChar;
                para[83].Value = _objEmp.Custom3;

                para[84] = new SqlParameter();
                para[84].ParameterName = "Custom4";
                para[84].SqlDbType = SqlDbType.VarChar;
                para[84].Value = _objEmp.Custom4;

                para[85] = new SqlParameter();
                para[85].ParameterName = "Custom5";
                para[85].SqlDbType = SqlDbType.VarChar;
                para[85].Value = _objEmp.Custom5;

                para[86] = new SqlParameter();
                para[86].ParameterName = "DDType";
                para[86].SqlDbType = SqlDbType.Int;
                para[86].Value = _objEmp.DDType;

                para[87] = new SqlParameter();
                para[87].ParameterName = "DDRate";
                para[87].SqlDbType = SqlDbType.Decimal;
                para[87].Value = _objEmp.DDRate;

                para[88] = new SqlParameter();
                para[88].ParameterName = "ACHType2";
                para[88].SqlDbType = SqlDbType.Int;
                para[88].Value = _objEmp.ACHType2;

                para[89] = new SqlParameter();
                para[89].ParameterName = "ACHRoute2";
                para[89].SqlDbType = SqlDbType.VarChar;
                para[89].Value = _objEmp.ACHRoute2;

                para[90] = new SqlParameter();
                para[90].ParameterName = "ACHBank2";
                para[90].SqlDbType = SqlDbType.VarChar;
                para[90].Value = _objEmp.ACHBank2;

                para[91] = new SqlParameter();
                para[91].ParameterName = "BillRate";
                para[91].SqlDbType = SqlDbType.Decimal;
                para[91].Value = _objEmp.BillRate;

                para[92] = new SqlParameter();
                para[92].ParameterName = "BMSales";
                para[92].SqlDbType = SqlDbType.Decimal;
                para[92].Value = _objEmp.BMSales;

                para[93] = new SqlParameter();
                para[93].ParameterName = "BMInvAve";
                para[93].SqlDbType = SqlDbType.Decimal;
                para[93].Value = _objEmp.BMInvAve;

                para[94] = new SqlParameter();
                para[94].ParameterName = "BMClosing";
                para[94].SqlDbType = SqlDbType.Decimal;
                para[94].Value = _objEmp.BMClosing;

                para[95] = new SqlParameter();
                para[95].ParameterName = "BMBillEff";
                para[95].SqlDbType = SqlDbType.Decimal;
                para[95].Value = _objEmp.BMBillEff;

                para[96] = new SqlParameter();
                para[96].ParameterName = "BMProdEff";
                para[96].SqlDbType = SqlDbType.Decimal;
                para[96].Value = _objEmp.BMProdEff;

                para[97] = new SqlParameter();
                para[97].ParameterName = "BMAveTask";
                para[97].SqlDbType = SqlDbType.Int;
                para[97].Value = _objEmp.BMAveTask;

                para[98] = new SqlParameter();
                para[98].ParameterName = "BMCustom1";
                para[98].SqlDbType = SqlDbType.Int;
                para[98].Value = _objEmp.BMCustom1;

                para[99] = new SqlParameter();
                para[99].ParameterName = "BMCustom2";
                para[99].SqlDbType = SqlDbType.Int;
                para[99].Value = _objEmp.BMCustom2;

                para[100] = new SqlParameter();
                para[100].ParameterName = "BMCustom3";
                para[100].SqlDbType = SqlDbType.Int;
                para[100].Value = _objEmp.BMCustom3;

                para[101] = new SqlParameter();
                para[101].ParameterName = "BMCustom4";
                para[101].SqlDbType = SqlDbType.Int;
                para[101].Value = _objEmp.BMCustom4;

                para[102] = new SqlParameter();
                para[102].ParameterName = "BMCustom5";
                para[102].SqlDbType = SqlDbType.Int;
                para[102].Value = _objEmp.BMCustom5;

                para[103] = new SqlParameter();
                para[103].ParameterName = "TaxCodeNR";
                para[103].SqlDbType = SqlDbType.VarChar;
                para[103].Value = _objEmp.TaxCodeNR;

                para[104] = new SqlParameter();
                para[104].ParameterName = "TaxCodeR";
                para[104].SqlDbType = SqlDbType.VarChar;
                para[104].Value = _objEmp.TaxCodeR;

                para[105] = new SqlParameter();
                para[105].ParameterName = "DeviceID";
                para[105].SqlDbType = SqlDbType.VarChar;
                para[105].Value = _objEmp.DeviceID;

                para[106] = new SqlParameter();
                para[106].ParameterName = "MileageRate";
                para[106].SqlDbType = SqlDbType.Decimal;
                para[106].Value = _objEmp.MileageRate;

                para[107] = new SqlParameter();
                para[107].ParameterName = "Import1";
                para[107].SqlDbType = SqlDbType.Int;
                para[107].Value = _objEmp.Import1;

                para[108] = new SqlParameter();
                para[108].ParameterName = "MSDeviceId";
                para[108].SqlDbType = SqlDbType.VarChar;
                para[108].Value = _objEmp.MSDeviceId;

                para[109] = new SqlParameter();
                para[109].ParameterName = "TechnicianBio";
                para[109].SqlDbType = SqlDbType.VarChar;
                para[109].Value = _objEmp.TechnicianBio;

                para[110] = new SqlParameter();
                para[110].ParameterName = "PayPortalPassword";
                para[110].SqlDbType = SqlDbType.VarChar;
                para[110].Value = _objEmp.PayPortalPassword;

                para[111] = new SqlParameter();
                para[111].ParameterName = "SickRate";
                para[111].SqlDbType = SqlDbType.Decimal;
                para[111].Value = _objEmp.SickRate;

                para[112] = new SqlParameter();
                para[112].ParameterName = "SickAccrued";
                para[112].SqlDbType = SqlDbType.Decimal;
                para[112].Value = _objEmp.SickAccrued;

                para[113] = new SqlParameter();
                para[113].ParameterName = "SickUsed";
                para[113].SqlDbType = SqlDbType.Decimal;
                para[113].Value = _objEmp.SickUsed;

                para[114] = new SqlParameter();
                para[114].ParameterName = "SickYTD";
                para[114].SqlDbType = SqlDbType.Decimal;
                para[114].Value = _objEmp.SickYTD;

                para[115] = new SqlParameter();
                para[115].ParameterName = "VacAccrued";
                para[115].SqlDbType = SqlDbType.Decimal;
                para[115].Value = _objEmp.VacAccrued;

                para[116] = new SqlParameter();
                para[116].ParameterName = "SCounty";
                para[116].SqlDbType = SqlDbType.Int;
                para[116].Value = _objEmp.SCounty;

                para[117] = new SqlParameter();
                para[117].ParameterName = "PDASerialNumber";
                para[117].SqlDbType = SqlDbType.VarChar;
                para[117].Value = _objEmp.PDASerialNumber;

                para[118] = new SqlParameter();
                para[118].ParameterName = "City";
                para[118].SqlDbType = SqlDbType.VarChar;
                para[118].Value = _objEmp.City;

                para[119] = new SqlParameter();
                para[119].ParameterName = "Zip";
                para[119].SqlDbType = SqlDbType.VarChar;
                para[119].Value = _objEmp.Zip;

                para[120] = new SqlParameter();
                para[120].ParameterName = "Tel";
                para[120].SqlDbType = SqlDbType.VarChar;
                para[120].Value = _objEmp.Tel;

                para[121] = new SqlParameter();
                para[121].ParameterName = "Address";
                para[121].SqlDbType = SqlDbType.VarChar;
                para[121].Value = _objEmp.Address;

                para[122] = new SqlParameter();
                para[122].ParameterName = "Email";
                para[122].SqlDbType = SqlDbType.VarChar;
                para[122].Value = _objEmp.Email;

                para[123] = new SqlParameter();
                para[123].ParameterName = "Cell";
                para[123].SqlDbType = SqlDbType.VarChar;
                para[123].Value = _objEmp.Cell;

                para[124] = new SqlParameter();
                para[124].ParameterName = "Remarks";
                para[124].SqlDbType = SqlDbType.VarChar;
                para[124].Value = _objEmp.Remarks;

                para[125] = new SqlParameter();
                para[125].ParameterName = "Type";
                para[125].SqlDbType = SqlDbType.Int;
                para[125].Value = _objEmp.Type;

                para[126] = new SqlParameter();
                para[126].ParameterName = "Contact";
                para[126].SqlDbType = SqlDbType.VarChar;
                para[126].Value = _objEmp.Contact;

                para[127] = new SqlParameter();
                para[127].ParameterName = "Website";
                para[127].SqlDbType = SqlDbType.VarChar;
                para[127].Value = _objEmp.Website;

                para[128] = new SqlParameter();
                para[128].ParameterName = "Fax";
                para[128].SqlDbType = SqlDbType.VarChar;
                para[128].Value = _objEmp.Fax;

                para[129] = new SqlParameter();
                para[129].ParameterName = "Country";
                para[129].SqlDbType = SqlDbType.VarChar;
                para[129].Value = _objEmp.Country;

                para[130] = new SqlParameter();
                para[130].ParameterName = "dtWageCategory";
                para[130].SqlDbType = SqlDbType.Structured;
                para[130].Value = _objEmp.dtWageCategory;

                para[131] = new SqlParameter();
                para[131].ParameterName = "dtWageDeduction";
                para[131].SqlDbType = SqlDbType.Structured;
                para[131].Value = _objEmp.dtWageDeduction;

                para[132] = new SqlParameter();
                para[132].ParameterName = "dtOtherIncome";
                para[132].SqlDbType = SqlDbType.Structured;
                para[132].Value = _objEmp.dtOtherIncome;

                para[133] = new SqlParameter();
                para[133].ParameterName = "Geocode";
                para[133].SqlDbType = SqlDbType.VarChar;
                para[133].Value = _objEmp.Geocode;

                para[134] = new SqlParameter();
                para[134].ParameterName = "FillingState";
                para[134].SqlDbType = SqlDbType.VarChar;
                para[134].Value = _objEmp.FillingState;

                para[135] = new SqlParameter();
                para[135].ParameterName = "CreatedBy";
                para[135].SqlDbType = SqlDbType.VarChar;
                para[135].Value = _objEmp.MOMUSer;




                SqlHelper.ExecuteNonQuery(_objEmp.ConnConfig, "spUpdtEmp", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddWageProject
        /// </summary>
        /// <param name="_objEmp"></param>
        public void AddWageProject(Emp _objEmp)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[4];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Job";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objEmp.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "JobWageC";
                para[1].SqlDbType = SqlDbType.Structured;
                para[1].Value = _objEmp.dtWageCategory;

                para[2] = new SqlParameter();
                para[2].ParameterName = "JobDed";
                para[2].SqlDbType = SqlDbType.Structured;
                para[2].Value = _objEmp.dtWageDeduction;

                para[3] = new SqlParameter();
                para[3].ParameterName = "AddEdit";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = _objEmp.Type;

                SqlHelper.ExecuteNonQuery(_objEmp.ConnConfig, "spAddJobWage", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// getWageCategorybyProjectID
        /// </summary>
        /// <param name="objWage"></param>
        /// <returns></returns>
        public DataSet getWageCategorybyProjectID(Wage objWage)
        {
            try
            {
                return objWage.Ds = SqlHelper.ExecuteDataset(objWage.ConnConfig, "spgetWageCategorybyProjectID", objWage.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// getDeductionCategorybyProjectID
        /// </summary>
        /// <param name="_objPRDed"></param>
        /// <returns></returns>
        public DataSet getDeductionCategorybyProjectID(PRDed _objPRDed)
        {
            try
            {
                return _objPRDed.Ds = SqlHelper.ExecuteDataset(_objPRDed.ConnConfig, "spgetDeductionCategorybyProjectID", _objPRDed.ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetRunPayroll
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetRunPayroll(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetRunPayroll", objPropUser.FStart, objPropUser.Edate, objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.ID, objPropUser.WorkId, -1, objPropUser.PayPeriod);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetRunPayrollFromTicket
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetRunPayrollFromTicket(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetRunPayrollFromTicket", objPropUser.FStart, objPropUser.Edate, objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.ID, objPropUser.WorkId, -1);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetPayrollHour
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetPayrollHour(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetRunPayrollByEmpID", objPropUser.FStart, objPropUser.Edate, objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.ID, objPropUser.WorkId, -1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetPayrollHour
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetPayrollHourByEmpId(User objPropUser, int registerId)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "[payroll].[spGetRunPayrollByEmpId]", objPropUser.FStart, objPropUser.Edate, objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.ID, objPropUser.WorkId, -1, objPropUser.HolidayAm, objPropUser.VacAm, objPropUser.ZoneAm, objPropUser.ReimbAm, objPropUser.MilageAm, objPropUser.BonusAm, objPropUser.SickAm, registerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetPayrollHourWithLocation
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetPayrollHourWithLocation(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetHoursiWithLocByEmpId", objPropUser.FStart, objPropUser.Edate, objPropUser.ID, objPropUser.HolidayAm, objPropUser.VacAm, objPropUser.ZoneAm, objPropUser.ReimbAm, objPropUser.MilageAm, objPropUser.BonusAm, objPropUser.SickAm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetPayrollRevenues
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetPayrollRevenues(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetRunPayrollCheckDetailByEmpID", objPropUser.FStart, objPropUser.Edate, objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.ID, objPropUser.WorkId, -1,
                    objPropUser.HolidayAm, objPropUser.VacAm, objPropUser.ZoneAm, objPropUser.ReimbAm, objPropUser.MilageAm, objPropUser.BonusAm, objPropUser.SickAm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetPayrollDeductions
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetPayrollDeductions(User objPropUser, int ProcessDed)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "uspDedcutionEmpDetail", objPropUser.FStart, objPropUser.Edate, objPropUser.ID,
                    objPropUser.HolidayAm, objPropUser.VacAm, objPropUser.ZoneAm, objPropUser.ReimbAm, objPropUser.MilageAm, objPropUser.BonusAm, ProcessDed);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddWageProject
        /// </summary>
        /// <param name="_objPRReg"></param>
        public DataSet ProcessPayroll(PRReg _objPRReg)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[13];

                para[0] = new SqlParameter();
                para[0].ParameterName = "PayrollEmp";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = _objPRReg.Dt;

                para[1] = new SqlParameter();
                para[1].ParameterName = "startdate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = _objPRReg.StartDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "enddate";
                para[2].SqlDbType = SqlDbType.DateTime;
                para[2].Value = _objPRReg.EndDate;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Bank";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = _objPRReg.Bank;

                para[4] = new SqlParameter();
                para[4].ParameterName = "Memo";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _objPRReg.Memo;

                para[5] = new SqlParameter();
                para[5].ParameterName = "Week";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = _objPRReg.WeekNo;

                para[6] = new SqlParameter();
                para[6].ParameterName = "PeriodDescription";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = _objPRReg.Description;

                para[7] = new SqlParameter();
                para[7].ParameterName = "ProcessMethod";
                para[7].SqlDbType = SqlDbType.VarChar;
                para[7].Value = _objPRReg.ProcessMethod;

                para[8] = new SqlParameter();
                para[8].ParameterName = "Supervisor";
                para[8].SqlDbType = SqlDbType.VarChar;
                para[8].Value = _objPRReg.Supervisor;

                para[9] = new SqlParameter();
                para[9].ParameterName = "ProcessDed";
                para[9].SqlDbType = SqlDbType.Int;
                para[9].Value = _objPRReg.PrcessDed;

                para[10] = new SqlParameter();
                para[10].ParameterName = "Check";
                para[10].SqlDbType = SqlDbType.Int;
                para[10].Value = _objPRReg.Checkno;

                para[11] = new SqlParameter();
                para[11].ParameterName = "CDate";
                para[11].SqlDbType = SqlDbType.DateTime;
                para[11].Value = _objPRReg.CDate;

                para[12] = new SqlParameter();
                para[12].ParameterName = "MOMUser";
                para[12].SqlDbType = SqlDbType.VarChar;
                para[12].Value = _objPRReg.MOMUSer;

                //cmd.Parameters.AddRange(para);
                //int iii = cmd.ExecuteNonQuery();
                //cnn.Close();


                return _objPRReg.Ds = SqlHelper.ExecuteDataset(_objPRReg.ConnConfig, "uspProcessPayroll", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddPayrollRagister(PRReg _objPRReg)
        
        {
            try
            {
                SqlParameter[] parameterCheck = {
                    new SqlParameter("@payrollRegisterId" , _objPRReg.ID),
                    new SqlParameter("@PayrollEmp" , _objPRReg.Dt),
                    new SqlParameter("@startdate",_objPRReg.StartDate),
                    new SqlParameter("@enddate" , _objPRReg.EndDate),
                    new SqlParameter("@week" , _objPRReg.WeekNo),
                    new SqlParameter("@PeriodDescription" , _objPRReg.Description),
                    new SqlParameter("@ProcessMethod" ,_objPRReg.ProcessMethod),
                    new SqlParameter("@SupervisorId" , _objPRReg.SupervisorId),
                    new SqlParameter("@DepartmentId" , _objPRReg.DepartmentId),
                    new SqlParameter("@ProcessDed" , _objPRReg.PrcessDed),
                    new SqlParameter("@FrequencyId" , _objPRReg.FrequencyId),
                    new SqlParameter("@GrossPay" , _objPRReg.GrossPay),
                    new SqlParameter("@TotalDeduction" , _objPRReg.TotalDeduction),
                    new SqlParameter("@NetPay" , _objPRReg.NetPay),
                    new SqlParameter("@RegisterId",_objPRReg.RegisterId)
                };
                parameterCheck[14].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteScalar(_objPRReg.ConnConfig, "[payroll].[spAddPayrollRegister]", parameterCheck);

                if (parameterCheck[14].Value != DBNull.Value)
                {
                    int registerId = Convert.ToInt32(parameterCheck[14].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditPayrollRagister(string conConfig, string Ids)
        {
            try
            {
                SqlParameter[] parameterCheck = {
                    new SqlParameter("@EmployeeIds" , Ids)
                };
                SqlHelper.ExecuteNonQuery(conConfig, "[payroll].[spEditPayrollRegister]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetPayrollRegisterId(PRReg _objPRReg)
        {
            try
            {
                SqlParameter[] parameterCheck = {
                    new SqlParameter("@startdate",_objPRReg.StartDate),
                    new SqlParameter("@enddate" , _objPRReg.EndDate),
                    new SqlParameter("@ProcessMethod" ,_objPRReg.ProcessMethod),
                    new SqlParameter("@SupervisorId" , _objPRReg.SupervisorId),
                    new SqlParameter("@DepartmentId" , _objPRReg.DepartmentId),
                    new SqlParameter("@FrequencyId" , _objPRReg.FrequencyId),
                };
                int Id = Convert.ToInt32( SqlHelper.ExecuteScalar(_objPRReg.ConnConfig, "[payroll].[spGetRegisterId]", parameterCheck));
                return Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdatePayrollCalculation(Wage _objWage, int payrollResigerId, int Eid)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, "[payroll].[spUpdatePayrollCalculation]", Eid, _objWage.ID, _objWage.Reg, _objWage.OT1, _objWage.OT2, _objWage.TT, _objWage.NT, payrollResigerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateOtherWage(Wage _objWage, int OtherWageId, double OtherWageValue, char? CalculationType, int payrollResigerId, int Eid)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, "[payroll].[spUpdateOtherWage]", Eid, OtherWageId, OtherWageValue, CalculationType, payrollResigerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetEmployeeList
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public DataSet GetPayrollRegister(PRReg _objPRReg)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];

                para[0] = new SqlParameter();
                para[0].ParameterName = "sDate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = _objPRReg.StartDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "eDate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = _objPRReg.EndDate;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Emp";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _objPRReg.EmpID;

                para[3] = new SqlParameter();
                para[3].ParameterName = "PageNumber";
                para[3].SqlDbType = SqlDbType.Int;
                para[3].Value = _objPRReg.PageNumber;

                para[4] = new SqlParameter();
                para[4].ParameterName = "PageSize";
                para[4].SqlDbType = SqlDbType.Int;
                para[4].Value = _objPRReg.PageSize;

                para[5] = new SqlParameter();
                para[5].ParameterName = "SortBy";
                para[5].SqlDbType = SqlDbType.VarChar;
                para[5].Value = _objPRReg.SortBy;

                para[6] = new SqlParameter();
                para[6].ParameterName = "SortType";
                para[6].SqlDbType = SqlDbType.VarChar;
                para[6].Value = _objPRReg.SortType;


                return _objPRReg.Ds = SqlHelper.ExecuteDataset(_objPRReg.ConnConfig, "spGetPayrollRegister", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVertexData(string con, int Id)
        {
            try
            {
                return SqlHelper.ExecuteDataset(con, CommandType.Text, "select top 1 EmpName as Id, Request, Response from tblVertex_XMLlog where EmpName = " + Id + " order by ID desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetEmployeeList
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public DataSet GetPayrollCheckDetail(PRReg _objPRReg)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];


                para[0] = new SqlParameter();
                para[0].ParameterName = "CheckID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objPRReg.ID;



                return _objPRReg.Ds = SqlHelper.ExecuteDataset(_objPRReg.ConnConfig, "spGetPayrollCheckDetail", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetEmployeeTitle
        /// </summary>
        /// <param name="_objEmp"></param>
        /// <returns></returns>
        public DataSet GetEmployeeTitle(Emp _objEmp)
        {
            try
            {
                return _objEmp.Ds = SqlHelper.ExecuteDataset(_objEmp.ConnConfig, "spGetEmployeeTitle");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// UpdateCheckDate
        /// </summary>
        /// <param name="_objPRReg"></param>
        public void UpdateCheckDate(PRReg _objPRReg)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objPRReg.ConnConfig, CommandType.Text, "UPDATE PRReg SET fDate = '" + _objPRReg.CDate + "' WHERE ID = " + _objPRReg.ID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// UpdateCheckNo
        /// </summary>
        /// <param name="_objPRReg"></param>
        public void UpdateCheckNo(PRReg _objPRReg)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[6];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objPRReg.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Bank";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = _objPRReg.Bank;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Check";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _objPRReg.Checkno;

                para[3] = new SqlParameter();
                para[3].ParameterName = "CDate";
                para[3].SqlDbType = SqlDbType.DateTime;
                para[3].Value = _objPRReg.CDate;

                para[4] = new SqlParameter();
                para[4].ParameterName = "MOMUser";
                para[4].SqlDbType = SqlDbType.VarChar;
                para[4].Value = _objPRReg.MOMUSer;

                para[5] = new SqlParameter();
                para[5].ParameterName = "UpdtType";
                para[5].SqlDbType = SqlDbType.Int;
                para[5].Value = _objPRReg.ProcessMethod;


                SqlHelper.ExecuteNonQuery(_objPRReg.ConnConfig, "spUpdatePayCheckDate", para);
                //SqlHelper.ExecuteNonQuery(_objPRReg.ConnConfig, CommandType.Text,  "UPDATE PRReg SET Ref = '" + _objPRReg.Ref + "' WHERE ID = " + _objPRReg.ID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// IsExistCheckNumOnEdit
        /// </summary>
        /// <param name="_objPRReg"></param>
        public bool IsExistCheckNumOnEdit(PRReg _objPRReg)
        {
            try
            {
                bool IsExistCheckNo = false;
                IsExistCheckNo = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objPRReg.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Ref FROM PRReg WHERE Ref= " + _objPRReg.Ref + " AND Bank=" + _objPRReg.Bank + " AND ID <> " + _objPRReg.ID + ")THEN 1  ELSE 0  END AS BIT)"));
                return IsExistCheckNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // <summary>
        /// VoidPayCheck
        /// </summary>
        /// <param name="_objPRReg"></param>
        public void VoidPayCheck(PRReg _objPRReg)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objPRReg.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "UpdatedBy";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _objPRReg.MOMUSer;

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objPRReg.ConnConfig, "spVoidPayCheck", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // <summary>
        /// getEmpByID
        /// </summary>
        /// <param name="_objEmp"></param>
        public DataSet getEmpByID(Emp _objEmp)
        {
            try
            {
                return _objEmp.Ds = SqlHelper.ExecuteDataset(_objEmp.ConnConfig, "spGetEmpByID", _objEmp.ID, _objEmp.Type, _objEmp.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // <summary>
        /// getEmpByIDforGeocode
        /// </summary>
        /// <param name="_objEmp"></param>
        public DataSet getEmpByIDforGeocode(Emp _objEmp)
        {
            try
            {
                return _objEmp.Ds = SqlHelper.ExecuteDataset(_objEmp.ConnConfig, "spGetEmpByIDGeocode", _objEmp.ID, _objEmp.Type, _objEmp.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // <summary>
        /// GetTimeCardInput
        /// </summary>
        /// <param name="objPropUser"></param>
        public DataSet GetTimeCardInput(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetEmpTimeCard", objPropUser.Supervisor, objPropUser.DepartmentID, objPropUser.EN, objPropUser.ID, objPropUser.WorkId, -1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // <summary>
        /// GetEmpACHDetail
        /// </summary>
        /// <param name="_objEmp"></param>
        public DataSet GetEmpACHDetail(PRReg _objPRReg)
        {
            try
            {
                return _objPRReg.Ds = SqlHelper.ExecuteDataset(_objPRReg.ConnConfig, "spGetEmpByID", _objPRReg.EmpID, _objPRReg.stRef, _objPRReg.edRef);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddPRWItemSession
        /// </summary>
        /// <param name="_objWage"></param>
        public void AddPRWItemSession(Wage _objWage, DateTime FStart, DateTime Edate, int Eid, string _Perioddesc, int _WeekNo)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, "spPRRegWItemSession", 0, Eid, FStart, Edate, _Perioddesc, _WeekNo, _objWage.ID, _objWage.Reg, _objWage.OT1, _objWage.OT2, _objWage.TT, _objWage.NT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        /// <summary>
        /// UpdateGeocode
        /// </summary>
        /// <param name="_objEmp"></param>
        public void UpdateGeocode(Emp _objEmp)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objEmp.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Geocode";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _objEmp.Geocode;

                para[2] = new SqlParameter();
                para[2].ParameterName = "CreatedBy";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = _objEmp.MOMUSer;

                SqlHelper.ExecuteNonQuery(_objEmp.ConnConfig, "spUpdateGeocode", para);
                //SqlHelper.ExecuteNonQuery(_objPRReg.ConnConfig, CommandType.Text,  "UPDATE PRReg SET Ref = '" + _objPRReg.Ref + "' WHERE ID = " + _objPRReg.ID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetVertexDeductionList
        /// </summary>
        /// <param name="_objPRDed"></param>
        /// <returns></returns>
        public DataSet GetVertexDeductionList(PRDed _objPRDed)
        {
            try
            {
                return _objPRDed.Ds = SqlHelper.ExecuteDataset(_objPRDed.ConnConfig, "spGetVertexDeductionList");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddWage
        /// </summary>
        /// <param name=""></param>
        public void AddVertexLog(string config, string name, string request, string response)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(config, "spAddVertexLog", name, request, response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPayrollRegister(PayrollRegisterModel payrollRegister, int PageNumber, int PageSize, string Sortfield, int SortOrder)
        {
            SqlParameter[] parameterCheck = {
                    new SqlParameter("@PageNumber", PageNumber),
                    new SqlParameter("@PageSize", PageSize),
                    new SqlParameter("@Sortfield", Sortfield),
                    new SqlParameter("@SortOrder", SortOrder),
                    new SqlParameter("@PayFrequency", payrollRegister.FrequencyId),
                    new SqlParameter("@StartDate", null),
                    new SqlParameter("@EndDate", null),
                    new SqlParameter("@CreatedBy", null),
                    new SqlParameter("@CreatedOn", null),
                    new SqlParameter("@ModifiedBy", null),
                    new SqlParameter("@ModifiedOn", null),
                    new SqlParameter("@Status", payrollRegister.Status),
                };

            try
            {
                return SqlHelper.ExecuteDataset(payrollRegister.ConnConfig, CommandType.StoredProcedure, "[payroll].[spGetPayrollRegister]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPayrollRegisterById(PayrollRegisterModel payrollRegister)
        {
            SqlParameter[] parameterCheck = {
                    new SqlParameter("@PayrollRegisterId", payrollRegister.PayrollRegisterId),
                };
            try
            {
                return SqlHelper.ExecuteDataset(payrollRegister.ConnConfig, CommandType.StoredProcedure, "[payroll].[spGetEditPayrollById]", parameterCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}