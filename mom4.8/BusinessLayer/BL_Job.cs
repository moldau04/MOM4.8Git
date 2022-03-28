using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.Projects;
using BusinessEntity.Recurring;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_Job
    {
        DL_Job objDL_Job = new DL_Job();

        public int DeleteBillingItem(String ConnConfig, int JobtItemID)
        {

            return objDL_Job.DeleteBillingItem(ConnConfig,   JobtItemID);
        }


        public int DeleteBOMItem(String ConnConfig, int JobtItemID)
        {

            
          return objDL_Job.DeleteBOMItem(ConnConfig,    JobtItemID);

            
        }



        public int AddJobtItemNew(String ConnConfig, int job, int OrderNo, int JobT, int Type, int JobtItemID)
        {
            return objDL_Job.AddJobtItemNew(ConnConfig, job, OrderNo, JobT, Type, JobtItemID);
        }

            public int AddProject_New2021(Customer objPropCustomer, string groupIds)
        {
            return objDL_Job.AddProject_New2021(objPropCustomer, groupIds);
        }

        public int Update_New2021(Customer objPropCustomer, string groupIds)
        {
            return objDL_Job.Update_New2021(objPropCustomer, groupIds);
        }
 
        
        public DataSet GetAllJobType(JobT _objJobT)
        {
            return objDL_Job.GetAllJobType(_objJobT);
        }
        public DataSet GetJobType(JobT _objJobT)
        {
            return objDL_Job.GetJobType(_objJobT);
        }
        public DataSet GetContractType(JobT _objJob)
        {
            return objDL_Job.GetContractType(_objJob);
        }
        public DataSet GetInvService(JobT _objJob)
        {
            return objDL_Job.GetInvService(_objJob);
        }
        public DataSet GetInvService_TypeZero(JobT _objJob)
        {
            return objDL_Job.GetInvService_TypeZero(_objJob);
        }
        public DataSet GetPosting(JobT _objJob)
        {
            return objDL_Job.GetPosting(_objJob);
        }
        public DataSet GetJobCode(JobT _objJob)
        {
            return objDL_Job.GetJobCode(_objJob);
        }
        public int GetDeptID(string ConnConfig, string DeptName)
        {
            return objDL_Job.GetDeptID(ConnConfig, DeptName);
        }
        public DataSet GetJobCodebyDept(JobT _objJob, int DeptID = 0)
        {
            return objDL_Job.GetJobCodebyDept(_objJob, DeptID);
        }

        public DataSet GetGroup(JobT _objJob)
        {
            return objDL_Job.GetGroup(_objJob);
        }

        public DataSet GetLocGroupNotInProj(JobT _objJob)
        {
            return objDL_Job.GetLocGroupNotInProj(_objJob);
        }

        public DataSet GetAllUM(JobT _objJob)
        {
            return objDL_Job.GetAllUM(_objJob);
        }
        public DataSet GetAllInvDetails(JobT _objJob)
        {
            return objDL_Job.GetAllInvDetails(_objJob);
        }
        public DataSet GetServiceType(JobT _objJob)
        {
            return objDL_Job.GetServiceType(_objJob);
        }
        public DataSet GetJobStatus(JobT _objJob)
        {
            return objDL_Job.GetJobStatus(_objJob);
        }
        public DataSet GetApplicationStatus(JobT _objJob)
        {
            return objDL_Job.GetApplicationStatus(_objJob);
        }
        public DataSet GetJobTFinanceByID(JobT _objJob)
        {
            return objDL_Job.GetJobTFinanceByID(_objJob);
        }
        public DataSet GetBomType(JobT _objJob)
        {
            return objDL_Job.GetBomType(_objJob);
        }
        public List<BOMTViewModel> GetBomType(GetBomTypeParam _GetBomTypeParam, string ConnectionString)
        {
            DataSet ds = objDL_Job.GetBomType(_GetBomTypeParam, ConnectionString);
            List<BOMTViewModel> _lstBOMTViewModel = new List<BOMTViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstBOMTViewModel.Add(
                    new BOMTViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Type = Convert.ToString(dr["Type"]),
                    }
                    );
            }
            return _lstBOMTViewModel;
        }

        public DataSet spSetTargetHours(string ConnConfig, int ProjectId, string Code, string GroupName, string TargetHours, int HoursReduce, int isMassupdatetargetedhoursby, int isCopytargetedhoursoverbudgethours, int isMassupdatetargeted)
        {
            return objDL_Job.spSetTargetHours(ConnConfig, ProjectId, Code, GroupName, TargetHours, HoursReduce, isMassupdatetargetedhoursby, isCopytargetedhoursoverbudgethours, isMassupdatetargeted);
        }

        public DataSet spGetTargetHours(string ConnConfig, int ProjectId)
        {
            return objDL_Job.spGetTargetHours(ConnConfig, ProjectId);
        }
        public DataSet GetTabByPageUrl(JobT _objJob)
        {
            return objDL_Job.GetTabByPageUrl(_objJob);
        }
        public DataSet GetRecurringCustom(JobT _objJob)
        {
            return objDL_Job.GetRecurringCustom(_objJob);
        }

        //API
        public ListGetRecurringCustom GetRecurringCustom(GetRecurringCustomParam _GetRecurringCustom, string ConnectionString)
        {
            DataSet ds = objDL_Job.GetRecurringCustom(_GetRecurringCustom, ConnectionString);

            ListGetRecurringCustom _ds = new ListGetRecurringCustom();
            List<GetRecurringCustomTable1> _lstTable1 = new List<GetRecurringCustomTable1>();
            List<GetRecurringCustomTable2> _lstTable2 = new List<GetRecurringCustomTable2>();
            List<GetRecurringCustomTable3> _lstTable3 = new List<GetRecurringCustomTable3>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetRecurringCustomTable1()
                    {
                        JobT = Convert.ToInt32(DBNull.Value.Equals(dr["JobT"]) ? 0 : dr["JobT"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        tblTabID = Convert.ToInt32(DBNull.Value.Equals(dr["tblTabID"]) ? 0 : dr["tblTabID"]),
                        Label = Convert.ToString(dr["Label"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Format = Convert.ToInt16(DBNull.Value.Equals(dr["Format"]) ? 0 : dr["Format"]),
                        IsDeleted = Convert.ToBoolean(DBNull.Value.Equals(dr["IsDeleted"]) ? false : dr["IsDeleted"]),
                        FieldControl = Convert.ToString(dr["FieldControl"]),
                        Value = Convert.ToString(dr["Value"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable2.Add(
                    new GetRecurringCustomTable2()
                    {
                        JobT = Convert.ToInt32(DBNull.Value.Equals(dr["JobT"]) ? 0 : dr["JobT"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        tblCustomFieldsID = Convert.ToInt32(DBNull.Value.Equals(dr["tblCustomFieldsID"]) ? 0 : dr["tblCustomFieldsID"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Value = Convert.ToString(dr["Value"]),
                        Label = Convert.ToString(dr["Label"]),
                        Format = Convert.ToInt16(DBNull.Value.Equals(dr["Format"]) ? 0 : dr["Format"]),
                        tblTabID = Convert.ToInt32(DBNull.Value.Equals(dr["tblTabID"]) ? 0 : dr["tblTabID"]),
                        FieldControl = Convert.ToString(dr["FieldControl"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable3.Add(
                    new GetRecurringCustomTable3()
                    {
                        JobT = Convert.ToInt32(DBNull.Value.Equals(dr["JobT"]) ? 0 : dr["JobT"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;
            _ds.lstTable3 = _lstTable3;

            return _ds;
        }

        //public DataSet GetRecurringJobCustom(JobT _objJob)
        //{
        //    return objDL_Job.GetRecurringJobCustom(_objJob);
        //}
        public bool IsExistRecurrJobT(JobT _objJob)
        {
            return objDL_Job.IsExistRecurrJobT(_objJob);
        }
        public DataSet GetProjectTemplateCustomFields(JobT objJob)
        {
            return objDL_Job.GetProjectTemplateCustomFields(objJob);
        }
        public DataSet GetProjectCustomTab(JobT _objJob)
        {
            return objDL_Job.GetProjectCustomTab(_objJob);
        }
        public bool IsExistProjectTempByType(JobT _objJob)
        {
            return objDL_Job.IsExistProjectTempByType(_objJob);
        }
        public DataSet GetJobTById(JobT _objJob)
        {
            return objDL_Job.GetJobTById(_objJob);
        }
        public DataSet GetDataByUM(JobT objJob)
        {
            return objDL_Job.GetDataByUM(objJob);
        }
        public DataSet GetJobCostByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostByJob(objJob);
        }

        public DataSet GetJobCostByJobReport(JobT objJob)
        {
            return objDL_Job.GetJobCostByJobReport(objJob);
        }
        public DataSet GetTypeItemByExpCode(JobT objJob)
        {
            return objDL_Job.GetTypeItemByExpCode(objJob);
        }
        public bool IsExistExpJobItemByJob(JobT objJob)
        {
            return objDL_Job.IsExistExpJobItemByJob(objJob);
        }
        public bool IsNotDeleteBomType(JobT objJob)
        {
            return objDL_Job.IsNotDeleteBomType(objJob);
        }
        public bool IsExistRevJobItemByJob(JobT objJob)
        {
            return objDL_Job.IsExistRevJobItemByJob(objJob);
        }
        public DataSet GetRevenueJobItemsByJob(JobT objJob)
        {
            return objDL_Job.GetRevenueJobItemsByJob(objJob);
        }
        public Int16 AddBOMItem(JobT objJob)
        {
            return objDL_Job.AddBOMItem(objJob);
        }
        public DataSet GetInventoryItem(JobT objJob)
        {
            return objDL_Job.GetInventoryItem(objJob);
        }
        public DataSet GetInventoryItemSearch(JobT objJob)
        {
            return objDL_Job.GetInventoryItemSearch(objJob);
        }
        public DataSet GetInventoryItemProject(String ConnConfig, String Text)
        {
            return objDL_Job.GetInventoryItemProject(ConnConfig, Text);
        }

        public DataSet GetInvById(JobT objJob)
        {
            return objDL_Job.GetInvById(objJob);
        }

        public DataSet GetLabourMaterial(JobT objJob)
        {
            return objDL_Job.GetLabourMaterial(objJob);
        }
        public DataSet GetLabourMaterialProject(String ConnConfig, String Text)
        {
            return objDL_Job.GetLabourMaterialProject(ConnConfig, Text);
        }
        public DataSet GetJobCostCodeByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostCodeByJob(objJob);
        }
        public DataSet GetJobCostTypeByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostTypeByJob(objJob);
        }
        public DataSet spGetProjectBudgetSummaryDetail (string ConnConfig, int jobID,    string Type , string StartDate = "", string EndDate = "")
        {
            return objDL_Job.spGetProjectBudgetSummaryDetail(ConnConfig, jobID,   Type,  StartDate, EndDate    );
        }
            public DataSet GetFinance_Budget_Grid_Popup_ByJob(string ConnConfig, int jobID, int PhaseID, int TypeID, string StartDate, string EndDate)
        {
            return objDL_Job.GetFinance_Budget_Grid_Popup_ByJob(ConnConfig, jobID, PhaseID, TypeID, StartDate, EndDate);
        }

        public DataSet GetJobCostInvoicesByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostInvoicesByJob(objJob);
        }
        public DataSet GetPhaseExpByJobType(JobT objJob)
        {
            return objDL_Job.GetPhaseExpByJobType(objJob);
        }

        public DataSet GetPhaseExpByJobTypeOpSequence(JobT objJob)
        {
            return objDL_Job.GetPhaseExpByJobTypeOpSequence(objJob);
        }


        public DataSet GetPhaseExpByJobTypePO(JobT objJob)
        {
            return objDL_Job.GetPhaseExpByJobTypePO(objJob);
        }
        public DataSet GetBOMTByTypeName(JobT objJob)
        {
            return objDL_Job.GetBOMTByTypeName(objJob);
        }
        public DataSet GetJobCostTicketsByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostTicketsByJob(objJob);
        }
        public DataSet GetBudgetSummaryGridDataByJob(JobT objJob, string StartDate = "", string EndDate = "")
        {
            return objDL_Job.GetBudgetSummaryGridDataByJob(objJob, StartDate, EndDate);
        }
        public DataSet GetContactForJob(JobT objJob, Int32 IsSalesAsigned = 0)
        {
            return objDL_Job.GetContactForJob(objJob, IsSalesAsigned);
        }
        public void AddJoinPhoneJob(JobT objJob, int PhoneID, int IsHighLighted)
        {
            objDL_Job.AddJoinPhoneJob(objJob, PhoneID, IsHighLighted);
        }


        //public DataSet GetInventoryByName(JobT objJob)
        //{
        //    return objDL_Job.GetInventoryByName(objJob);
        //}

        public DataSet GetAllJobTypeForSearch(JobT _objJob)
        {
            return objDL_Job.GetAllJobTypeForSearch(_objJob);
        }

        public DataSet GetJobTypeForWIP(JobT _objJob)
        {
            return objDL_Job.GetJobTypeForWIP(_objJob);
        }

        public DataSet GetAllJobTypeForAjaxSearch(int type)
        {
            return objDL_Job.GetAllJobTypeForAjaxSearch(type);
        }

        public DataSet GetEstimateProjectTemplateByID(JobT objJob)
        {
            return objDL_Job.GetEstimateProjectTemplateByID(objJob);
        }

        public DataSet GetBomTypeForEstimateCalculation(JobT _objJob)
        {
            return objDL_Job.GetBomTypeForEstimateCalculation(_objJob);
        }
        public DataSet GetAttachment(JobT objJob)
        {
            return objDL_Job.GetAttachment(objJob);
        }
        public string GetAttachmentByID(JobT objJob)
        {
            return objDL_Job.GetAttachmentByID(objJob);
        }
        public DataSet GetJobCostJEByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostJEByJob(objJob);
        }
        public DataSet GetExpenseJobCost(JobT objJob)
        {
            return objDL_Job.GetExpenseJobCost(objJob);
        }
        public DataSet GetExpenseJobCostByDate(JobT objJob)
        {
            return objDL_Job.GetExpenseJobCostByDate(objJob);
        }
        public DataSet GetJobCostPOByJob(JobT objJob)
        {
            return objDL_Job.GetJobCostPOByJob(objJob);
        }
        public DataSet GetJobExpGLByJob(JobT objJob)
        {
            return objDL_Job.GetJobExpGLByJob(objJob);
        }

        public DataSet GetBOMByJobID(JobT objJob)
        {
            return objDL_Job.GetBOMByJobID(objJob);
        }

        public DataSet GetMilestoneByJobID(JobT objJob)
        {
            return objDL_Job.GetMilestoneByJobID(objJob);
        }
        public int AddBOMType(JobT objJob)
        {
            return objDL_Job.AddBOMType(objJob);
        }
        public DataSet GetJobRoute(JobT _objJobT)
        {
            return objDL_Job.GetJobRoute(_objJobT);
        }

        public DataSet GetJobStage(JobT _objJobT)
        {
            return objDL_Job.GetJobStage(_objJobT);
        }

        public DataSet GetAllStageItems(string strConn)
        {
            return objDL_Job.GetAllStageItems(strConn);
        }

        public DataSet GetDepartmentByTemplateId(JobT objJob)
        {
            return objDL_Job.GetDepartmentByTemplateId(objJob);
        }

        public DataSet GetDataOnInitialEstimate(string strConfig, int salesAsigned = 0, int estimateId = 0, int uType = 1)
        {
            return objDL_Job.GetDataOnInitialEstimate(strConfig, salesAsigned, estimateId, uType);
        }

        public DataSet GetJobAttributeGeneralCustomValueByJobId(JobT objJob)
        {
            return objDL_Job.GetJobAttributeGeneralCustomByJobID(objJob);
        }
        public DataSet GetJobCustomValueByJobId(JobT objJob)
        {
            return objDL_Job.GetJobCustomValueByJobId(objJob);
        }
        public void UpdateJobCustomValue(JobT objJob)
        {
            objDL_Job.UpdateJobCustomValue(objJob);
        }

        public DataSet GetProjectManagerByTypeId(JobT _objJob)
        {
            return objDL_Job.GetProjectManagerByTypeId(_objJob);
        }
        public DataSet GetAssingedProjectByTypeId(JobT _objJob)
        {
            return objDL_Job.GetAssingedProjectByTypeId(_objJob);
        }

        public DataSet GetUserDetailByEmpID(string strConfig, int empId)
        {
            return objDL_Job.GetUserDetailByEmpID(strConfig, empId);
        }
        public List<ProjectListGridModel> spGetProjectListDataMVC(ProjectListGridParam _ProjectParam, String ConnectionString)
        {
            DataSet ds = objDL_Job.spGetProjectListDataMVC(_ProjectParam, ConnectionString);


            List<ProjectListGridModel> jobs = new List<ProjectListGridModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                jobs.Add(
                    new ProjectListGridModel()
                    {
                        ProjectID = dr["ID"].ToString(),
                        ProjectName = dr["fdesc"].ToString(),
                        CustomerName = dr["Customer"].ToString(),
                        LocationName = dr["tag"].ToString(),
                        Status = dr["Status"].ToString(),
                        DateCreated = Convert.ToDateTime(dr["fDate"].ToString()),
                        ServiceType = dr["CType"].ToString(),
                        TemplateType = dr["TemplateDesc"].ToString(),
                        Department = dr["Type"].ToString(),
                        DefaultSalesPerson = dr["Salesperson"].ToString(),
                        DefaultWorker = dr["Route"].ToString(),
                        ContractPrice = Convert.ToDouble(dr["ContractPrice"].ToString()),
                        NotBilledYet = Convert.ToDouble(dr["NotBilledYet"].ToString()),
                        TotalBilled = Convert.ToDouble(dr["NRev"].ToString()),
                        ActualHours = Convert.ToDouble(dr["NHour"].ToString()),
                        LaborExpense = Convert.ToDouble(dr["NLabor"].ToString()),
                        MaterialExpense = Convert.ToDouble(dr["NMat"].ToString()),
                        OtherExpense = Convert.ToDouble(dr["Nomat"].ToString()),
                        TotalExpense = Convert.ToDouble(dr["NCost"].ToString()),
                        TotalPOOrder = Convert.ToDouble(dr["NComm"].ToString()),
                        ReceivePO = Convert.ToDouble(dr["ReceivePO"].ToString()),
                        NetProfit = Convert.ToDouble(dr["NProfit"].ToString()),
                        perInProfit = Convert.ToDouble(dr["NRatio"].ToString()),
                        ProjectManager = ""

                    }
                    );
            }

            return jobs;
        }
        public DataSet GetJobById(JobT _objJob)
        {
            return objDL_Job.GetJobById(_objJob);
        }

        //Start -- Juily - 12/08/2020-- //
        public void RecalculateLaborExpenses(JobT _objJob)
        {
            objDL_Job.RecalculateLaborExpenses(_objJob);
        }

        //End- Juily - 12/08/2020-- //

        public DataSet GetSupervisorByTypeId(JobT _objJob)
        {
            return objDL_Job.GetSupervisorByTypeId(_objJob);
        }


        #region Start - Project WIP

        public DataSet GetProjectWIPByPeriod(JobT _objJob, int period)
        {
            return objDL_Job.GetProjectWIPByPeriod(_objJob, period);
        }

        public DataSet GetLastProjectWIPPostedByPeriod(JobT _objJob, int period)
        {
            return objDL_Job.GetLastProjectWIPPostedByPeriod(_objJob, period);
        }

        public DataSet GetLastPostProjectWIP(JobT _objJob)
        {
            return objDL_Job.GetLastPostProjectWIP(_objJob);
        }

        public int AddProjectWIP(JobT _objJob, DateTime fDate, bool isPost = false)
        {
            return objDL_Job.AddProjectWIP(_objJob, fDate, isPost);
        }

        public void UpdateProjectWIP(JobT _objJob, DateTime fDate, bool isPost = false)
        {
            objDL_Job.UpdateProjectWIP(_objJob, fDate, isPost);
        }

        public void AddProjectWIPDetail(ProjectWIP _objProWip)
        {
            objDL_Job.AddProjectWIPDetail(_objProWip);
        }

        #endregion

        public DataSet GetGroupImport(JobT _objJob)
        {
            return objDL_Job.GetGroupImport(_objJob);
        }
        public DataSet spGetJobStatus(JobT _objJob)
        {
            return objDL_Job.spGetJobStatus(_objJob);
        }
    }
}