using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CommonModel;
using BusinessEntity.CustomersModel;
using BusinessEntity.InventoryModel;
using BusinessEntity.payroll;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using DataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace BusinessLayer
{
    public class BL_Wage
    {
        DL_Wage objDL_Wage = new DL_Wage();

        /// <summary>
        /// GetWage (Get filtered wage list with paging and shorting)
        /// </summary>
        /// <param name="objPropUser, PageNumber, PageSize, SortDirection, Sortfield"></param>
        /// <returns></returns>
        public DataSet getWage(Wage objWage, int PageNumber, int PageSize, string Sortfield, int SortOrder)
        {
            return objDL_Wage.getWage(objWage, PageNumber, PageSize, Sortfield, SortOrder);
        }

        /// <summary>
        /// GetWageTypes 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public DataSet getWageTypes(Wage objWage)
        {
            return objDL_Wage.getWageTypes(objWage);
        }

        /// <summary>
        /// GetWage
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet getWage(Wage objWage)
        {
            return objDL_Wage.getWage(objWage);
        }
        /// <summary>
        /// GetWageByID
        /// </summary>
        /// <param name="_objWage"></param>
        /// <returns></returns>
        public DataSet GetWageByID(Wage _objWage)
        {
            return objDL_Wage.GetWageByID(_objWage);
        }
        
        /// <summary>
        /// DeleteWageByID
        /// </summary>
        /// <param name="_objWage"></param>
        public void DeleteWageByID(Wage _objWage)
        {
            objDL_Wage.DeleteWageByID(_objWage);
        }
        /// <summary>
        /// GetAllWage
        /// </summary>
        /// <param name="_objWage"></param>
        /// <returns></returns>
        public DataSet GetAllWage(Wage _objWage)
        {
            return objDL_Wage.GetAllWage(_objWage);
        }
        /// <summary>
        /// IsWageRateIsUsed
        /// </summary>
        /// <param name="_objWage"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet IsWageRateIsUsed(Wage _objWage, Int32 userID)
        {
            return objDL_Wage.IsWageRateIsUsed(_objWage, userID);
        }
        /// <summary>
        /// AddWage
        /// </summary>
        /// <param name="_objWage"></param>
        public void AddWage(Wage _objWage)
        {
            objDL_Wage.AddWage(_objWage);
        }
        /// <summary>
        /// UpdateWage
        /// </summary>
        /// <param name="_objWage"></param>
        public void UpdateWage(Wage _objWage)
        {
            objDL_Wage.UpdateWage(_objWage);
        }
        /// <summary>
        /// GetWageDeduction
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetWageDeduction(PRDed _objPRDed)
        {
            return objDL_Wage.GetWageDeduction(_objPRDed);
        }
        /// <summary>
        /// DeleteWageDeductionByID
        /// </summary>
        /// <param name="_objPRDed"></param>
        public void DeleteWageDeductionByID(PRDed _objPRDed)
        {
            objDL_Wage.DeleteWageDeductionByID(_objPRDed);
        }
        /// <summary>
        /// GetWageDeductionByID
        /// </summary>
        /// <param name="_objPRDed"></param>
        /// <returns></returns>
        public DataSet GetWageDeductionByID(PRDed _objPRDed)
        {
            return objDL_Wage.GetWageDeductionByID(_objPRDed);
        }
        /// <summary>
        /// AddWageDeduction
        /// </summary>
        /// <param name="_objPRDed"></param>
        public void AddWageDeduction(PRDed _objPRDed)
        {
            objDL_Wage.AddWageDeduction(_objPRDed);
        }
        /// <summary>
        /// GetEmployeeList
        /// </summary>
        /// <param name="_objEmp"></param>
        /// <returns></returns>
        public DataSet GetEmployeeList(Emp _objEmp)
        {
            return objDL_Wage.GetEmployeeList(_objEmp);
        }
        /// <summary>
        /// GetEmployeeListByID
        /// </summary>
        /// <param name="_objEmp"></param>
        /// <returns></returns>
        public DataSet GetEmployeeListByID(Emp _objEmp)
        {
            return objDL_Wage.GetEmployeeListByID(_objEmp);
        }
        /// <summary>
        /// DeleteEmployeeByID
        /// </summary>
        /// <param name="_objEmp"></param>
        public void DeleteEmployeeByID(Emp _objEmp)
        {
            objDL_Wage.DeleteEmployeeByID(_objEmp);
        }
        /// <summary>
        /// getPayrollTaxAccount
        /// </summary>
        /// <param name="objWage"></param>
        /// <returns></returns>
        public DataSet getPayrollTaxAccount(Wage objWage)
        {
            return objDL_Wage.getPayrollTaxAccount(objWage);
        }
        /// <summary>
        /// AddEmp
        /// </summary>
        /// <param name="_objEmp"></param>
        public void AddEmp(Emp _objEmp)
        {
            objDL_Wage.AddEmp(_objEmp);
        }
        /// <summary>
        /// UpdateEmp
        /// </summary>
        /// <param name="_objEmp"></param>
        public void UpdateEmp(Emp _objEmp)
        {
            objDL_Wage.UpdateEmp(_objEmp);
        }
        /// <summary>
        /// AddWageProject
        /// </summary>
        /// <param name="_objEmp"></param>
        public void AddWageProject(Emp _objEmp)
        {
            objDL_Wage.AddWageProject(_objEmp);
        }
        /// <summary>
        /// getWageCategorybyProjectID
        /// </summary>
        /// <param name="objWage"></param>
        /// <returns></returns>
        public DataSet getWageCategorybyProjectID(Wage objWage)
        {
            return objDL_Wage.getWageCategorybyProjectID(objWage);
        }
        /// <summary>
        /// getDeductionCategorybyProjectID
        /// </summary>
        /// <param name="_objPRDed"></param>
        /// <returns></returns>
        public DataSet getDeductionCategorybyProjectID(PRDed _objPRDed)
        {
            return objDL_Wage.getDeductionCategorybyProjectID(_objPRDed);
        }
        /// <summary>
        /// GetRunPayroll
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetRunPayroll(User objPropUser)
        {
            return objDL_Wage.GetRunPayroll(objPropUser);
        }
        /// <summary>
        /// GetRunPayrollFromTicket
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetRunPayrollFromTicket(User objPropUser)
        {
            return objDL_Wage.GetRunPayrollFromTicket(objPropUser);
        }

        /// <summary>
        /// GetPayrollHour
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetPayrollHour(User objPropUser)
        {
            return objDL_Wage.GetPayrollHour(objPropUser);
        }

        /// <summary>
        /// GetPayrollHour
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetPayrollHourByEmpId(User objPropUser, int registerId)
        {
            return objDL_Wage.GetPayrollHourByEmpId(objPropUser, registerId);
        }

        /// <summary>
        /// GetPayrollHour
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetPayrollHourWithLocation(User objPropUser)
        {
            return objDL_Wage.GetPayrollHourWithLocation(objPropUser);
        }
        /// <summary>
        /// GetPayrollRevenues
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetPayrollRevenues(User objPropUser)
        {
            return objDL_Wage.GetPayrollRevenues(objPropUser);
        }
        /// <summary>
        /// GetPayrollDeductions
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetPayrollDeductions(User objPropUser,int ProcessDed)
        {
            return objDL_Wage.GetPayrollDeductions(objPropUser, ProcessDed);
        }
        /// <summary>
        /// ProcessPayroll
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public DataSet ProcessPayroll(PRReg _objPRReg)
        {
            return objDL_Wage.ProcessPayroll(_objPRReg);
        }

        public void AddPayrollRagister(PRReg _objPRReg)
        {
            objDL_Wage.AddPayrollRagister(_objPRReg); 
        }
        public int GetPayrollRegisterId(PRReg _objPRReg)
        {
           return objDL_Wage.GetPayrollRegisterId(_objPRReg);
        }
        public void EditPayrollRagister(string conConfig, string Ids)
        {
            objDL_Wage.EditPayrollRagister(conConfig, Ids);
        }
        public void UpdatePayrollCalculation(Wage _objWage, int payrollResigerId, int Eid)
        {
            objDL_Wage.UpdatePayrollCalculation(_objWage, payrollResigerId, Eid);
        }

        public void UpdateOtherWage(Wage _objWage, int OtherWageId, double OtherWageValue, char? CalculationType, int payrollResigerId, int Eid)
        {
            objDL_Wage.UpdateOtherWage(_objWage, OtherWageId, OtherWageValue, CalculationType, payrollResigerId, Eid);
        }

        /// <summary>
        /// GetPayrollRegister
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public DataSet GetPayrollRegister(PRReg _objPRReg)
        {
            return objDL_Wage.GetPayrollRegister(_objPRReg);
        }

        /// <returns></returns>
        public DataSet GetVertexData(string con, int Id)
        {
            return objDL_Wage.GetVertexData(con, Id);
        }
        /// <summary>
        /// GetPayrollCheckDetail
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public DataSet GetPayrollCheckDetail(PRReg _objPRReg)
        {
            return objDL_Wage.GetPayrollCheckDetail(_objPRReg);
        }
        /// <summary>
        /// GetEmployeeTitle
        /// </summary>
        /// <param name="_objEmp"></param>
        /// <returns></returns>
        public DataSet GetEmployeeTitle(Emp _objEmp)
        {
            return objDL_Wage.GetEmployeeTitle(_objEmp);
        }
        /// <summary>
        /// UpdateCheckDate
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public void UpdateCheckDate(PRReg _objPRReg)
        {
            objDL_Wage.UpdateCheckDate(_objPRReg);
        }
        /// <summary>
        /// UpdateCheckNo
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public void UpdateCheckNo(PRReg _objPRReg)
        {
            objDL_Wage.UpdateCheckNo(_objPRReg);
        }
        /// <summary>
        /// IsExistCheckNumOnEdit
        /// </summary>
        /// <param name="_objPRReg"></param>
        /// <returns></returns>
        public bool IsExistCheckNumOnEdit(PRReg _objPRReg)
        {
            return objDL_Wage.IsExistCheckNumOnEdit(_objPRReg);
        }
        // <summary>
        /// VoidPayCheck
        /// </summary>
        /// <param name="_objPRReg"></param>
        public void VoidPayCheck(PRReg _objPRReg)
        {
            objDL_Wage.VoidPayCheck(_objPRReg);
        }
        // <summary>
        /// GetEmpInfoByID
        /// </summary>
        /// <param name="_objEmp"></param>
        public DataSet GetEmpInfoByID(Emp _objEmp)
        {

            return objDL_Wage.getEmpByID(_objEmp);
        }
        // <summary>
        /// getEmpByIDforGeocode
        /// </summary>
        /// <param name="_objEmp"></param>
        public DataSet getEmpByIDforGeocode(Emp _objEmp)
        {

            return objDL_Wage.getEmpByIDforGeocode(_objEmp);
        }
        
        /// <summary>
        /// GetTimeCardInput
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetTimeCardInput(User objPropUser)
        {
            return objDL_Wage.GetTimeCardInput(objPropUser);
        }
        /// <summary>
        /// GetEmpACHDetail
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetEmpACHDetail(PRReg _objPRReg)
        {
            return objDL_Wage.GetEmpACHDetail(_objPRReg);
        }
        /// <summary>
        /// AddPRWItemSession
        /// </summary>
        /// <param name="_objWage"></param>
        public void AddPRWItemSession(Wage _objWage,DateTime FStart, DateTime Edate,int Eid,string _Perioddesc,int  _WeekNo)
        {
            objDL_Wage.AddPRWItemSession(_objWage, FStart, Edate, Eid, _Perioddesc, _WeekNo);
        }

       
        /// <summary>
        /// UpdateGeocode
        /// </summary>
        /// <param name="_objEmp"></param>
        /// <returns></returns>
        public void UpdateGeocode(Emp _objEmp)
        {
            objDL_Wage.UpdateGeocode(_objEmp);
        }
        /// <summary>
        /// GetVertexDeductionList
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetVertexDeductionList(PRDed _objPRDed)
        {
            return objDL_Wage.GetVertexDeductionList(_objPRDed);
        }
        /// <summary>
        /// AddWage
        /// </summary>
        /// <param name=""></param>
        public void AddVertexLog(string config, string name, string request, string response)
        {
            try
            {
                objDL_Wage.AddVertexLog(config, name,request,response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPayrollRegister(PayrollRegisterModel objPayroll, int PageNumber, int PageSize, string Sortfield, int SortOrder)
        {
            return objDL_Wage.GetPayrollRegister(objPayroll, PageNumber, PageSize, Sortfield, SortOrder);
        }

        public DataSet GetPayrollRegisterById(PayrollRegisterModel objPayroll)
        {
            return objDL_Wage.GetPayrollRegisterById(objPayroll);
        }
    }
}
