using System;
using System.Data;
using DataLayer;
using BusinessEntity;
using BusinessEntity.Recurring;

namespace BusinessLayer
{
    public class BL_SafetyTest
    {
        DL_SafetyTest objDL_Test = new DL_SafetyTest();

        public DataSet GetAllTestTypes(TestTypes testtypes)
        {
            return objDL_Test.GetAllTestTypes(testtypes);
        }

        public DataSet GetAllCategory(TestTypes testtypes)
        {
            return objDL_Test.GetAllCategory(testtypes);
        }

        public int CreateTestType(TestTypes testtypes)
        {
            return objDL_Test.CreateTestType(testtypes);
        }
        
        public void UpdateTestType(TestTypes testtypes)
        {
            objDL_Test.UpdateTestType(testtypes);
        }

        public void DeleteTestType(TestTypes testtypes)
        {
            objDL_Test.DeleteTestType(testtypes);
        }

        public DataSet GetAllTestStatus(TestTypes testtypes)
        {
            return objDL_Test.GetAllTestStatus(testtypes);
        }

        public SafetyTest CreateTest(SafetyTest test)
        {
            return objDL_Test.CreateTest(test);
        }

        public SafetyTest UpdateTest(SafetyTest test)
        {
            return objDL_Test.UpdateTest(test);
        }

        public SafetyTest CreateTestByYear(SafetyTest test)
        {
            return objDL_Test.CreateTestByYear(test);
        }

        public SafetyTest UpdateTestByYear(SafetyTest test)
        {
            return objDL_Test.UpdateTestByYear(test);
        }


        public DataSet GetTestHistory(SafetyTest test)
        {
            return objDL_Test.GetTestHistory(test);
        }

        public DataSet ValidateTest(SafetyTest test)
        {
            return objDL_Test.ValidateTest(test);
        }

        public DataSet GetTestDetails(SafetyTest test)
        {
            return objDL_Test.GetTestDetails(test);
        }
        public DataSet GetTestDetailsByYear(SafetyTest test)
        {
            return objDL_Test.GetTestDetailsByYear(test);
        }

        public DataSet GetTestDetailsAjaxSearch(SafetyTest test, string column, string searchTerm,String sqlFilter,String sortOrderby,String sortType , string dateRage,

              string CustomerFiter = "NA",
        string LocationFiter = "NA",
        string LocationAddressFiter = "NA",
        string LocationAcctFiter = "NA",
        string LocationCityFiter = "NA",
        string LocationStateFiter = "NA",
        string EuipmentIDFiter = "NA",
        string UnitFiter = "NA" 
        )
        {
            return objDL_Test.GetTestDetailsAjaxSearch(test, column, searchTerm, sqlFilter, sortOrderby, sortType , dateRage,
                  CustomerFiter,
                  LocationFiter,
                  LocationAddressFiter,
                  LocationAcctFiter,
                  LocationCityFiter,
                  LocationStateFiter,
                  EuipmentIDFiter,
                  UnitFiter );
        }
        public DataSet GetTestReport(SafetyTest test, string column, string searchTerm, String sqlFilter, String sortOrderby, String sortType)
        {
            return objDL_Test.GetTestReport(test, column, searchTerm, sqlFilter, sortOrderby, sortType);
        }

        public int CreateTicket(SafetyTest test)
        {
            return objDL_Test.CreateTicket(test);
        }
        public int CreateTicketByYear(SafetyTest test)
        {
            return objDL_Test.CreateTicketByYear(test);
        }
        public int CreateTicketsByYearForAllTestInLocation(SafetyTest test)
        {
            return objDL_Test.CreateTicketsByYearForAllTestInLocation(test);
        }

        public DataSet GetTestTypesByName(TestTypes testtypes)
         {
             return objDL_Test.GetTestTypesByName(testtypes);
         }
        public DataSet GetTestTypesLikeName(TestTypes testtypes)
        {
            return objDL_Test.GetTestTypesLikeName(testtypes);
        }

        public int DeleteTest(SafetyTest test)
         {
             return objDL_Test.DeleteTest(test);
         }
        public DataSet GetAllTestCustom(String conn, String dbName)
        {
            return objDL_Test.GetAllTestCustom(conn, dbName);
        }

        public int CreateAndUpdateTestCustom(TestCustom testcustom)
        {
            return objDL_Test.CreateAndUpdateTestCustom(testcustom);
        }
        public DataSet GetTestCustomValueByEquipTest(String conn, int testId, int equipId)
        {
            return objDL_Test.GetTestCustomValueByEquipTest(conn, testId, equipId);
        }
        public int CreateAndUpdateTestCustomItemValue(String conn, DataTable dtTestItemValue)
        {
            return objDL_Test.CreateAndUpdateTestCustomItemValue(conn, dtTestItemValue);
        }

        public DataSet GetTestTypeById(String conn, int testId)
        {
            return objDL_Test.GetTestTypeById(conn,testId);
        }

        #region "Test Setup Forms"

        public DataSet GetAllTestSetupForms(String conn)
        {
            return objDL_Test.GetAllTestSetupForms(conn);
        }
        public DataSet GetTestSetupFormsById(String conn, int id)
        {
            return objDL_Test.GetTestSetupFormsById(conn, id);
        }

        public int AddTestSetupForms(TestSetupForm testForm)
        {
            return objDL_Test.AddTestSetupForms(testForm);
        }

        public void UpdateTestSetupForms(TestSetupForm testForm)
        {
            objDL_Test.UpdateTestSetupForms(testForm);
        }
        public void DeleteTestSetupForms(string conn, int id)
        {
            objDL_Test.DeleteTestSetupForms(conn, id);
        }
        public DataSet GetTestSetupFormsByType(String conn, int type)
        {
            return objDL_Test.GetAllTestSetupFormsByType(conn, type);
        }
        #endregion
        #region "ProposalForm"
        public DataSet GetProposalFormByID(String conn, int id)
        {
            return objDL_Test.GetProposalFormByID(conn, id);
        }
        //public DataSet GetProposalFormByLocAndEquipType(String conn, int locID, String classification, DateTime fromDate, DateTime toDate)
        //{
        //    return objDL_Test.GetProposalFormByLocAndClassification(conn, locID, classification, fromDate, toDate);
        //}
        public int AddProposalForm(ProposalForm proposalForm)
        {
            return objDL_Test.AddProposalForm(proposalForm);

        }
        public int CreateProposalForm(ProposalForm proposalForm)
        {
            return objDL_Test.CreateProposalForm(proposalForm);

        }
        public int AddProposalFormDetail(ProposalFormDetail proposalFormDetail)
        {
            return objDL_Test.AddProposalFormDetail(proposalFormDetail);

        }
        public void UpdateStatusProposalForm(String conn, int id, String status, String updatedBy)
        {
            objDL_Test.UpdateStatusProposalForm(conn, id, status, updatedBy );

        }
        public DataSet GetProposalByEquipmentID(String conn, int equipmentId)
        {
            return objDL_Test.GetProposalByEquipmentID(conn, equipmentId);
        }

        public DataSet GetProposalByTestID(String conn, int testID)
        {
            return objDL_Test.GetProposalByTestID(conn, testID);
        }

        public DataSet GetAllTestForEquipmentInYear(String conn, int equipmentId, int yearProposal)
        {
            return objDL_Test.GetAllTestForEquipmentInYear(conn, equipmentId, yearProposal);
        }
        public DataSet GetSafetyTestForProposal(SafetyTest test, string column, string searchTerm)
        {
            return objDL_Test.GetSafetyTestForProposal(test, column, searchTerm);
        }
        public DataSet SearchEmailTeam(String conn, string searchTerm)
        {
            return objDL_Test.SearchEmailTeam(conn,  searchTerm);
        }
        public void DeleteProposalForm(String conn,int id)
        {
            objDL_Test.DeleteProposalForm(conn,id);
        }
        public void UpdateSenderInfoProposalForm(String conn,int id, string sendTo, String sendFrom)
        {
            objDL_Test.UpdateSenderInfoProposalForm(conn,id, sendTo, sendFrom);

        }
        public int AddProposalFormForEquipment(ProposalForm proposalForm)
        {
            return objDL_Test.AddProposalFormForEquipment(proposalForm);

        }
        public int UpdateTestCustomItemValue(SafetyTest obj)
        {
            return objDL_Test.UpdateTestCustomItemValue( obj);

        }
        public int UpdateTestCustomItemValueByYear(SafetyTest obj)
        {
            return objDL_Test.UpdateTestCustomItemValueByYear(obj);
            

        }
        public DataSet GetTestLogs(SafetyTest obj)
        {
            return objDL_Test.GetTestLogs(obj);
        }
        public DataSet GetTestLogsByYear(SafetyTest obj, int LocYear)
        {
            return objDL_Test.GetTestLogsByYear(obj, LocYear);
        }
        public DataSet GetCustomFieldAlert(SafetyTest obj)
        {
            return objDL_Test.GetCustomFieldAlert(obj);
        }
        #endregion
        #region "workfolow"
        public DataSet GetAllWorkflows(String conn )
        {
            return objDL_Test.GetAllWorkflows(conn);
        }
        public int CreateAndUpdateWorkflow(Workflow wf)
        {
            return objDL_Test.CreateAndUpdateWorkflow(wf);
        }
        #endregion
        #region "ViolationStatus"
        public DataSet GetAllViolationStatus(String conn)
        {
            return objDL_Test.GetAllViolationStatus(conn);
        }
        public void AddViolationStatus(ViolationStatus obj)
        {
             objDL_Test.AddViolationStatus(obj);

        }
        public void UpdateViolationStatus(ViolationStatus obj)
        {
            objDL_Test.UpdateViolationStatus(obj);

        }
        public void DeleteViolationStatus(ViolationStatus obj)
        {
            objDL_Test.DeleteViolationStatus(obj);

        }
        #endregion
        #region "ViolationCode"
        public DataSet GetAllViolationSection(String conn)
        {
            return objDL_Test.GetAllViolationSection(conn);
        }
        public DataSet GetAllViolationCategory(String conn)
        {
            return objDL_Test.GetAllViolationCategory(conn);
        }
        public DataSet GetAllViolationCode(String conn)
        {
            return objDL_Test.GetAllViolationCode(conn);
        }
        public void AddViolationCode(ViolationCode obj)
        {
            objDL_Test.AddViolationCode(obj);

        }
        public void UpdateViolationCode(ViolationCode obj)
        {
            objDL_Test.UpdateViolationCode(obj);

        }
        public void DeleteViolationCode(ViolationCode obj)
        {
            objDL_Test.DeleteViolationCode(obj);

        }

        public void AddViolationSection(ViolationCode obj)
        {
            objDL_Test.AddViolationSection(obj);

        }
        public void AddViolationCategory(ViolationCode obj)
        {
            objDL_Test.AddViolationCategory(obj);

        }
        #endregion
        public DataSet GetPriceHistory(SafetyTest test)
        {
            return objDL_Test.GetPriceHistory(test);
        }
        public DataSet GetTestSetupEmailFormsById(String conn, int id)
        {
            return objDL_Test.GetTestSetupEmailFormsById(conn, id);
        }
        public DataSet GetAllTestSetupEmailForms(String conn)
        {
            return objDL_Test.GetAllTestSetupEmailForms(conn);
        }
        public void DeleteTestSetupEmailForms(string conn, int id)
        {
            objDL_Test.DeleteTestSetupEmailForms(conn, id);
        }
        public int UpdateTestSetupEmailForms(TestSetupEmailForm testForm)
        {
            return objDL_Test.UpdateTestSetupEmailForms(testForm);
        }
        public int AddTestSetupEmailForms(TestSetupEmailForm testForm)
        {
            return objDL_Test.AddTestSetupEmailForms(testForm);
        }


        public String AssignTicketToTest(SafetyTest obj,int OldTicket)
        {
            return objDL_Test.AssignTicketToTest(obj, OldTicket);
        }


        public void UpdateTestScheduledStatus(string ConnConfig, string ScheduleStatusID, string ScheduledYear, string LID)
        {
            objDL_Test.UpdateTestScheduledStatus(ConnConfig, ScheduleStatusID, ScheduledYear, LID);
       }
        public String UpdateTestScheduled(SafetyTest obj)
        {
            return objDL_Test.UpdateTestScheduled(obj);
        }
        public String UpdateTestScheduledDetail(SafetyTest obj)
        {
            return objDL_Test.UpdateTestScheduledDetail(obj);
        }
        public int UpdatePriceDetailByYear(SafetyTest obj, int updateAll, int isNew)
        {
            return objDL_Test.UpdatePriceDetailByYear(obj, updateAll, isNew);
        }
        public void DeleteTestScheduled(SafetyTest obj)
        {
             objDL_Test.DeleteTestScheduled(obj);
        }
        public void DeleteTestScheduledDetail(SafetyTest obj)
        {
            objDL_Test.DeleteTestScheduledDetail(obj);
        }
        public String UpdateTestService(SafetyTest obj)
        {
            return objDL_Test.UpdateTestService(obj);
        }
        public void DeleteTestScheduleService(SafetyTest obj)
        {
            objDL_Test.DeleteTestScheduleService(obj);
        }
        public DataSet GetTestCustomValueByEquipTestByYear(String conn, int testId, int equipId,int year)
        {
            return objDL_Test.GetTestCustomValueByEquipTestByYear(conn, testId, equipId,year);
        }
        public DataSet GetAllLoadTestItemSchedule(SafetyTest test)
        {
            return objDL_Test.GetAllLoadTestItemSchedule(test);
        }
        public DataSet GetAllLoadTestItemService(SafetyTest test)
        {
            return objDL_Test.GetAllLoadTestItemService(test);
        }
        public void DeleteTestPriceByYear(SafetyTest obj)
        {
            objDL_Test.DeleteTestPriceByYear(obj);
        }
    }

}
