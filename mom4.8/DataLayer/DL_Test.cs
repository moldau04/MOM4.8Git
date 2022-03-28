using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using BusinessEntity.Recurring;

namespace DataLayer
{
    public class DL_SafetyTest
    {
        public DataSet GetAllTestTypes(TestTypes testtypes)
        {
            try
            {
                return SqlHelper.ExecuteDataset(testtypes.ConnConfig, "spGetAllTestType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public DataSet GetTestTypesByName(TestTypes testtypes)
        {
            try
            {

                return SqlHelper.ExecuteDataset(testtypes.ConnConfig, CommandType.Text, "SELECT 	[ID] ,[Name],[Authority],[Frequency],[Remarks]	,[Count],[Level],[Cat],[fDesc],[NextDateCalcMode] ,ISNULL([Charge],0) AS Charge ,ISNULL([ThirdParty],0) AS ThirdParty,ISNULL([Status],0) AS Status, (CASE WHEN Remarks IS Null THEN ' ' ELSE Remarks END) AS Remarks FROM LoadTest where LOWER(REPLACE(LTRIM(RTRIM(Name)), ' ', ''))='" + testtypes.Name.Replace(" ", "").ToLower() + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTestTypesLikeName(TestTypes testtypes)
        {
            try
            {

                return SqlHelper.ExecuteDataset(testtypes.ConnConfig, CommandType.Text, "SELECT 	[ID] ,[Name],[Authority],[Frequency],[Remarks]	,[Count],[Level],[Cat],[fDesc],[NextDateCalcMode] ,ISNULL([Charge],0) AS Charge ,ISNULL([ThirdParty],0) AS ThirdParty,ISNULL([Status],0) AS Status, (CASE WHEN Remarks IS Null THEN ' ' ELSE Remarks END) AS Remarks FROM LoadTest where LOWER(REPLACE(LTRIM(RTRIM(Name)), ' ', '')) like '%" + testtypes.Name.Replace(" ", "").ToLower() + "%'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllCategory(TestTypes testtypes)
        {
            try
            {
                return SqlHelper.ExecuteDataset(testtypes.ConnConfig, CommandType.Text, "SELECT * FROM Category");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int CreateTestType(TestTypes testtypes)
        {
            int success = 0;
            try
            {
                SqlHelper.ExecuteScalar(testtypes.ConnConfig, "spCreateTestTypes",
                 testtypes.Name,
                 testtypes.Authority,
                 testtypes.Frequency,
                 testtypes.Remarks,
                 testtypes.Count,
                 testtypes.Cat,
                 testtypes.fDesc,
                 testtypes.NextDateCalcMode,
                 testtypes.Charge,
                 testtypes.Status,
                  testtypes.TestTypeCover,
                  testtypes.TicketCovered);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return success;

        }

        public void UpdateTestType(TestTypes testtypes)
        {
            try
            {



                SqlHelper.ExecuteNonQuery(testtypes.ConnConfig, "spUpdateTestTypes",
                    testtypes.ID,
                 testtypes.Name,
                 testtypes.Authority,
                 testtypes.Frequency,
                 testtypes.Remarks,
                 testtypes.Count,
                 testtypes.Cat,
                 testtypes.fDesc,
                 testtypes.NextDateCalcMode,
                 testtypes.Charge,
                 testtypes.Status,
                 testtypes.TestTypeCover,
                 testtypes.TicketCovered);


            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        public void DeleteTestType(TestTypes testtypes)
        {
            try
            {



                SqlHelper.ExecuteNonQuery(testtypes.ConnConfig, "spDeleteTestTypes",
                    testtypes.ID);


            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        public DataSet GetAllTestStatus(TestTypes testtypes)
        {
            try
            {
                return SqlHelper.ExecuteDataset(testtypes.ConnConfig, CommandType.Text, "SELECT ListConfig.ItemValue as ID,ListConfig.ItemName as Status FROM ListConfig where ListConfig.ListName='Test.Status'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SafetyTest CreateTest(SafetyTest test)
        {
            int success = 0;

            try
            {


                string constring = test.ConnConfig;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionTest"))
                    {
                        try
                        {

                            //1:Create Test by inserting into loadtestitems table
                            test.LID = (int)SqlHelper.ExecuteScalar(tans, "spCreateTest", test.Typeid, test.Locid, test.Equipid,
                              test.Last, test.Next, test.Status, null, "", test.Lastdate, test.Job, test.Custom1, test.Custom2, test.Custom3, test.Custom4, test.Amount, test.OverrideAmount, test.ThirdPartyName, test.ThirdPartyPhone,test.TestDueBy,test.Charge,test.ThirdParty,test.PriceYear,test.UserName);

                            //2:Update the count of types
                            SqlHelper.ExecuteScalar(tans, "spUpdateTestTypeCount", test.Typeid, "Increment");


                            //3: Update Custom 
                            CreateAndUpdateTestCustomItemValue(constring, test.LID, test.Equipid, test.Cus_TestItemValue);

                            //4:Create Log
                            SqlHelper.ExecuteScalar(tans, "spCreateTestHistory", test.LID, test.UserName, test.Statusstr, test.Last, test.Status, null, "Unassigned");
                            success = 1;

                            tans.Commit();


                        }
                        catch (Exception exx)
                        {
                            success = -1;

                            tans.Rollback("TransactionTest");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return test;
        }

        public SafetyTest UpdateTest(SafetyTest test)
        {
            int success = 0;

            try
            {


                string constring = test.ConnConfig;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    using (SqlTransaction tans = con.BeginTransaction("TransactionTest1"))
                    {
                        try
                        {

                            //1:Update existing test



                            SqlHelper.ExecuteScalar(tans, "spUpdateTest", test.LID, test.Typeid, test.Locid, test.Equipid,
                              test.Last, test.Next, test.Status, test.Ticket, "", test.Lastdate, test.Job, test.Workerid, test.Worker, test.Custom1, test.Custom2, test.Custom3, test.Custom4, test.Amount, test.OverrideAmount, test.ThirdPartyName, test.ThirdPartyPhone,test.TestDueBy,test.Charge,test.ThirdParty,test.PriceYear,test.UserName);

                            if (test.Typeid != test.OldTypeid)
                            {
                                //2:Update the count of types
                                SqlHelper.ExecuteScalar(tans, "spUpdateTestTypeCount", test.OldTypeid, "Decrement");
                                SqlHelper.ExecuteScalar(tans, "spUpdateTestTypeCount", test.Typeid, "Increment");
                            }                          

                            //3: Update Custom 
                            CreateAndUpdateTestCustomItemValueByYear(constring, test.LID, test.Equipid, test.Cus_TestItemValue, test.PriceYear);

                            if (test.CreateTestHistory)
                            {
                                //4:Create Log
                                SqlHelper.ExecuteScalar(tans, "spCreateTestHistory", test.LID, test.UserName, test.Statusstr, test.Last, test.Status, test.Ticket, test.Ticket == null ? "Unassigned" : "Assigned");
                            }
                            success = 1;

                            tans.Commit();


                        }
                        catch (Exception exx)
                        {
                            success = -1;

                            tans.Rollback("TransactionTest1");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return test;
        }

        public SafetyTest CreateTestByYear(SafetyTest test)
        {
            int success = 0;          

            try
            { 
                SqlParameter[] param = new SqlParameter[26];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@typeID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = test.Typeid;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Loc";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = test.Locid;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Elev";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = test.Equipid;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@Last";
                param[3].SqlDbType = SqlDbType.DateTime;
                param[3].Value = test.Last;


                param[4] = new SqlParameter();
                param[4].ParameterName = "@Next";
                param[4].SqlDbType = SqlDbType.DateTime;
                param[4].Value = test.Next;

                param[5] = new SqlParameter();
                param[5].ParameterName = "@Status";
                param[5].SqlDbType = SqlDbType.Int;
                param[5].Value = test.Status;

                param[6] = new SqlParameter();
                param[6].ParameterName = "@Ticket";
                param[6].SqlDbType = SqlDbType.Int;
                param[6].Value = test.Ticket;

                param[7] = new SqlParameter();
                param[7].ParameterName = "@Remarks";
                param[7].SqlDbType = SqlDbType.VarChar;
                param[7].Value = test.Remarks;

                param[8] = new SqlParameter();
                param[8].ParameterName = "@LastDue";
                param[8].SqlDbType = SqlDbType.DateTime;
                param[8].Value = test.Last;

                param[9] = new SqlParameter();
                param[9].ParameterName = "@JobId";
                param[9].SqlDbType = SqlDbType.Int;
                param[9].Value = test.Job;

                param[10] = new SqlParameter();
                param[10].ParameterName = "@Custom1";
                param[10].SqlDbType = SqlDbType.VarChar;
                param[10].Value = test.Custom1;

                param[11] = new SqlParameter();
                param[11].ParameterName = "@Custom2";
                param[11].SqlDbType = SqlDbType.VarChar;
                param[11].Value = test.Custom2;

                param[12] = new SqlParameter();
                param[12].ParameterName = "@Custom3";
                param[12].SqlDbType = SqlDbType.VarChar;
                param[12].Value = test.Custom3;

                param[13] = new SqlParameter();
                param[13].ParameterName = "@Custom4";
                param[13].SqlDbType = SqlDbType.VarChar;
                param[13].Value = test.Custom4;


                param[14] = new SqlParameter();
                param[14].ParameterName = "@Amount";
                param[14].SqlDbType = SqlDbType.Decimal;
                param[14].Value = test.Amount;

                param[15] = new SqlParameter();
                param[15].ParameterName = "@OverrideAmount";
                param[15].SqlDbType = SqlDbType.Decimal;
                param[15].Value = test.OverrideAmount;


                param[16] = new SqlParameter();
                param[16].ParameterName = "@ThirdPartyName";
                param[16].SqlDbType = SqlDbType.VarChar;
                param[16].Value = test.ThirdPartyName;


                param[17] = new SqlParameter();
                param[17].ParameterName = "@ThirdPartyPhone";
                param[17].SqlDbType = SqlDbType.VarChar;
                param[17].Value = test.ThirdPartyPhone;

                param[18] = new SqlParameter();
                param[18].ParameterName = "@TestDueBy";
                param[18].SqlDbType = SqlDbType.Int;
                param[18].Value = test.TestDueBy;

                param[19] = new SqlParameter();
                param[19].ParameterName = "@Charge";
                param[19].SqlDbType = SqlDbType.Int;
                param[19].Value = test.Charge;

                param[20] = new SqlParameter();
                param[20].ParameterName = "@ThirdParty";
                param[20].SqlDbType = SqlDbType.Int;
                param[20].Value = test.ThirdParty;

                param[21] = new SqlParameter();
                param[21].ParameterName = "@PriceYear";
                param[21].SqlDbType = SqlDbType.Int;
                param[21].Value = test.PriceYear;

                param[22] = new SqlParameter();
                param[22].ParameterName = "@CreatedBy";
                param[22].SqlDbType = SqlDbType.VarChar;
                param[22].Value = test.UserName;

                param[23] = new SqlParameter();
                param[23].ParameterName = "@TestCustomItemValue";
                param[23].SqlDbType = SqlDbType.Structured;
                param[23].Value = test.Cus_TestItemValue;

                param[24] = new SqlParameter();
                param[24].ParameterName = "@ID";
                param[24].SqlDbType = SqlDbType.Int;
                param[24].Direction = ParameterDirection.Output;


                param[25] = new SqlParameter();
                param[25].ParameterName = "@Classification";
                param[25].SqlDbType = SqlDbType.VarChar;
                param[25].Value = test.Classification;



                SqlHelper.ExecuteNonQuery(test.ConnConfig, CommandType.StoredProcedure, "spCreateTestByYear", param);

                test.LID = Convert.ToInt32(param[24].Value);

                //string constring = test.ConnConfig;
                //using (SqlConnection con = new SqlConnection(constring))
                //{
                //    con.Open();
                //    using (SqlTransaction tans = con.BeginTransaction("TransactionTest"))
                //    {
                //        try
                //        {

                //            //1:Create Test by inserting into loadtestitems table
                //           SqlHelper.ExecuteNonQuery(tans, "spCreateTestByYear", test.Typeid, test.Locid, test.Equipid,
                //              test.Last, test.Next, test.Status, null, "", test.Lastdate, test.Job, test.Custom1, test.Custom2, test.Custom3, test.Custom4, test.Amount, test.OverrideAmount, test.ThirdPartyName, test.ThirdPartyPhone, test.TestDueBy, test.Charge, test.ThirdParty, test.PriceYear, test.UserName, test.Cus_TestItemValue);

                //            //2:Update the count of types
                //            SqlHelper.ExecuteScalar(tans, "spUpdateTestTypeCount", test.Typeid, "Increment");


                //            ////3: Update Custom 
                //            CreateAndUpdateTestCustomItemValueByYear(constring, test.LID, test.Equipid, test.Cus_TestItemValue, test.PriceYear);

                //            //4:Create Log
                //            SqlHelper.ExecuteScalar(tans, "spCreateTestHistory", test.LID, test.UserName, test.Statusstr, test.Last, test.Status, null, "Unassigned");
                //            success = 1;

                //          //  tans.Commit();


                //        }
                //        catch (Exception exx)
                //        {
                //            success = -1;

                //           // tans.Rollback("TransactionTest");
                //        }
                //    }

                //}

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return test;
        }
        public SafetyTest UpdateTestByYear(SafetyTest test)
        {
            int success = 0;

            try
            {
                SqlParameter[] param = new SqlParameter[29];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@typeID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = test.Typeid;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Loc";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = test.Locid;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Elev";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = test.Equipid;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@Last";
                param[3].SqlDbType = SqlDbType.DateTime;
                param[3].Value = test.Last;


                param[4] = new SqlParameter();
                param[4].ParameterName = "@Next";
                param[4].SqlDbType = SqlDbType.DateTime;
                param[4].Value = test.Next;

                param[5] = new SqlParameter();
                param[5].ParameterName = "@Status";
                param[5].SqlDbType = SqlDbType.Int;
                param[5].Value = test.Status;

                param[6] = new SqlParameter();
                param[6].ParameterName = "@Ticket";
                param[6].SqlDbType = SqlDbType.Int;
                param[6].Value = test.Ticket;

                param[7] = new SqlParameter();
                param[7].ParameterName = "@Remarks";
                param[7].SqlDbType = SqlDbType.VarChar;
                param[7].Value = test.Remarks;

                param[8] = new SqlParameter();
                param[8].ParameterName = "@LastDue";
                param[8].SqlDbType = SqlDbType.DateTime;
                param[8].Value = test.Last;

                param[9] = new SqlParameter();
                param[9].ParameterName = "@JobId";
                param[9].SqlDbType = SqlDbType.Int;
                param[9].Value = test.Job;

                param[10] = new SqlParameter();
                param[10].ParameterName = "@Custom1";
                param[10].SqlDbType = SqlDbType.VarChar;
                param[10].Value = test.Custom1;

                param[11] = new SqlParameter();
                param[11].ParameterName = "@Custom2";
                param[11].SqlDbType = SqlDbType.VarChar;
                param[11].Value = test.Custom2;

                param[12] = new SqlParameter();
                param[12].ParameterName = "@Custom3";
                param[12].SqlDbType = SqlDbType.VarChar;
                param[12].Value = test.Custom3;

                param[13] = new SqlParameter();
                param[13].ParameterName = "@Custom4";
                param[13].SqlDbType = SqlDbType.VarChar;
                param[13].Value = test.Custom4;


                param[14] = new SqlParameter();
                param[14].ParameterName = "@Amount";
                param[14].SqlDbType = SqlDbType.Decimal;
                param[14].Value = test.Amount;

                param[15] = new SqlParameter();
                param[15].ParameterName = "@OverrideAmount";
                param[15].SqlDbType = SqlDbType.Decimal;
                param[15].Value = test.OverrideAmount;


                param[16] = new SqlParameter();
                param[16].ParameterName = "@ThirdPartyName";
                param[16].SqlDbType = SqlDbType.VarChar;
                param[16].Value = test.ThirdPartyName;


                param[17] = new SqlParameter();
                param[17].ParameterName = "@ThirdPartyPhone";
                param[17].SqlDbType = SqlDbType.VarChar;
                param[17].Value = test.ThirdPartyPhone;

                param[18] = new SqlParameter();
                param[18].ParameterName = "@TestDueBy";
                param[18].SqlDbType = SqlDbType.Int;
                param[18].Value = test.TestDueBy;

                param[19] = new SqlParameter();
                param[19].ParameterName = "@Charge";
                param[19].SqlDbType = SqlDbType.Int;
                param[19].Value = test.Charge;

                param[20] = new SqlParameter();
                param[20].ParameterName = "@ThirdParty";
                param[20].SqlDbType = SqlDbType.Int;
                param[20].Value = test.ThirdParty;

                param[21] = new SqlParameter();
                param[21].ParameterName = "@PriceYear";
                param[21].SqlDbType = SqlDbType.Int;
                param[21].Value = test.PriceYear;

                param[22] = new SqlParameter();
                param[22].ParameterName = "@CreatedBy";
                param[22].SqlDbType = SqlDbType.VarChar;
                param[22].Value = test.UserName;

                param[23] = new SqlParameter();
                param[23].ParameterName = "@TestCustomItemValue";
                param[23].SqlDbType = SqlDbType.Structured;
                param[23].Value = test.Cus_TestItemValue;

                param[24] = new SqlParameter();
                param[24].ParameterName = "@isDefautTest";
                param[24].SqlDbType = SqlDbType.Int;
                param[24].Value = test.isDefautTest;

                param[25] = new SqlParameter();
                param[25].ParameterName = "@ID";
                param[25].SqlDbType = SqlDbType.Int;
                param[25].Value = test.LID;

                param[26] = new SqlParameter();
                param[26].ParameterName = "@CreateTestHistory";
                param[26].SqlDbType = SqlDbType.Bit;
                param[26].Value = test.CreateTestHistory;

                param[27] = new SqlParameter();
                param[27].ParameterName = "@UpdateThirdPartyForAll";
                param[27].SqlDbType = SqlDbType.Bit;
                param[27].Value = test.UpdateThirdPartyForAll;

                param[28] = new SqlParameter();
                param[28].ParameterName = "@Classification";
                param[28].SqlDbType = SqlDbType.VarChar;
                param[28].Value = test.Classification;

                SqlHelper.ExecuteNonQuery(test.ConnConfig, CommandType.StoredProcedure, "spUpdateTestByYear", param);

                CreateAndUpdateTestCustomItemValueByYear(test.ConnConfig, test.LID, test.Equipid, test.Cus_TestItemValue, test.PriceYear);
                //string constring = test.ConnConfig;
                //using (SqlConnection con = new SqlConnection(constring))
                //{
                //    con.Open();

                //    using (SqlTransaction tans = con.BeginTransaction("TransactionTest1"))
                //    {
                //        try
                //        {

                //            //1:Update existing test



                //            SqlHelper.ExecuteScalar(tans, "spUpdateTestByYear", test.LID, test.Typeid, test.Locid, test.Equipid,
                //              test.Last, test.Next, test.Status, test.Ticket, "", test.Lastdate, test.Job, test.Workerid, test.Worker, test.Custom1, test.Custom2, test.Custom3, test.Custom4, test.Amount, test.OverrideAmount, test.ThirdPartyName, test.ThirdPartyPhone, test.TestDueBy, test.Charge, test.ThirdParty, test.PriceYear, test.UserName);

                //            //if (test.Typeid != test.OldTypeid)
                //            //{
                //            //    //2:Update the count of types
                //            //    SqlHelper.ExecuteScalar(tans, "spUpdateTestTypeCount", test.OldTypeid, "Decrement");
                //            //    SqlHelper.ExecuteScalar(tans, "spUpdateTestTypeCount", test.Typeid, "Increment");
                //            //}

                //            //3: Update Custom 
                //            CreateAndUpdateTestCustomItemValueByYear(constring, test.LID, test.Equipid, test.Cus_TestItemValue, test.PriceYear);

                //            if (test.CreateTestHistory)
                //            {
                //                //4:Create Log
                //                SqlHelper.ExecuteScalar(tans, "spCreateTestHistory", test.LID, test.UserName, test.Statusstr, test.Last, test.Status, test.Ticket, test.Ticket == null ? "Unassigned" : "Assigned");
                //            }
                //            success = 1;

                //            tans.Commit();


                //        }
                //        catch (Exception exx)
                //        {
                //            success = -1;

                //            tans.Rollback("TransactionTest1");
                //        }
                //    }

                //}

            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return test;
        }
        public DataSet GetTestHistory(SafetyTest test)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, CommandType.Text, "SELECT idTestHistory as id, StatusDate Edit, ActualDate Actual, UserName, TestStatus Status, LastDate ,TicketID,TicketStatus,[NextDueDate],[LastDueDate]  FROM TestHistory WHERE idTest = " + test.LID + " ORDER BY StatusDate DESC,idTestHistory DESC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ValidateTest(SafetyTest test)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, CommandType.Text, "SELECT TOP 1 * FROM LoadTestItem WHERE LoadTestItem.ID=" + test.Typeid + " AND LoadTestItem.Elev=" + test.Equipid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTestDetails(SafetyTest test)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGetTestDetails", test.LID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTestDetailsByYear(SafetyTest test)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGetTestDetailsByYear", test.LID,test.PriceYear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTestDetailsAjaxSearch(SafetyTest test, string column, string searchTerm,String sqlfilter, String sortOrderby, String sortType, string dateRage,
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
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGetSafetyTestDetails", test.Startdate, test.Enddate, test.Typeid,
                    string.IsNullOrEmpty(column) ? null : column, string.IsNullOrEmpty(searchTerm) ? null : searchTerm, test.FlagEN, test.UserID,test.Proposal, sqlfilter, sortOrderby, sortType , dateRage,
                     CustomerFiter,
                  LocationFiter,
                  LocationAddressFiter,
                  LocationAcctFiter,
                  LocationCityFiter,
                  LocationStateFiter,
                  EuipmentIDFiter, 
                    UnitFiter );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTestReport(SafetyTest test, string column, string searchTerm, String sqlfilter, String sortOrderby, String sortType)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGenerateTestReport", test.Startdate, test.Enddate, test.Typeid,
                    string.IsNullOrEmpty(column) ? null : column, string.IsNullOrEmpty(searchTerm) ? null : searchTerm, test.FlagEN, test.UserID, test.Proposal, sqlfilter, sortOrderby, sortType, test.ScheduleDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateTicket(SafetyTest test)
        {
            int success = 0;

            try
            {                
                string constring = test.ConnConfig;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionTckt"))
                    {
                        try
                        {
                            //1:Create Test by inserting into loadtestitems table
                            test.Ticket = (int)SqlHelper.ExecuteScalar(tans, "spCreateTickets", test.LID, test.UserName);
                            if (test.Ticket > 0)
                            {
                                //2:Update existing test
                                SqlHelper.ExecuteNonQuery(tans, "spUpdateTestTicket", test.LID, test.Status, test.Ticket,test.CreateTicketForAll, test.UserName);

                                if (test.Status != test.OldStatus)
                                {
                                    //3:Create Log
                                    SqlHelper.ExecuteNonQuery(tans, "spCreateTestHistory", test.LID, test.UserName, test.Statusstr, test.Lastdate, test.Status, test.Ticket, "Assigned");
                                }
                                success = 1;
                            }
                            else
                                success = -1;
                            tans.Commit();
                        }
                        catch (Exception exx)
                        {
                            success = -1;
                            tans.Rollback("TransactionTckt");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }
        public int CreateTicketByYear(SafetyTest test)
        {
            int success = 0;

            try
            {
               
              
                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter();
                    param[0].ParameterName = "@LID";
                    param[0].SqlDbType = SqlDbType.Int;
                    param[0].Value = test.LID;

                    param[1] = new SqlParameter();
                    param[1].ParameterName = "@Username";
                    param[1].SqlDbType = SqlDbType.VarChar;
                    param[1].Value = test.UserName;

                    param[2] = new SqlParameter();
                    param[2].ParameterName = "@TestYear";
                    param[2].SqlDbType = SqlDbType.Int;
                    param[2].Value = test.PriceYear;   


                    param[3] = new SqlParameter();
                    param[3].ParameterName = "@ListTicketID";
                    param[3].SqlDbType = SqlDbType.VarChar;
                    param[3].Size = 100;
                    param[3].Direction = ParameterDirection.Output;



                    SqlHelper.ExecuteNonQuery(test.ConnConfig, CommandType.StoredProcedure, "spCreateTicketsByYear", param);

                test.Ticket = Convert.ToInt32(param[3].Value);

                success = 1;
               
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }
        public int CreateTicketsByYearForAllTestInLocation(SafetyTest test)
        {
            int success = 0;

            try
            {
                
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@LID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = test.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Username";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = test.UserName;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@TestYear";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = test.PriceYear;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@ListTicketID";
                param[3].SqlDbType = SqlDbType.VarChar;
                param[3].Size = 100;
                param[3].Direction = ParameterDirection.Output;



                SqlHelper.ExecuteNonQuery(test.ConnConfig, CommandType.StoredProcedure, "spCreateTicketsByYearForAllTestInLocation", param);

                test.Ticket = Convert.ToInt32(Convert.ToString(param[3].Value));

                success = 1;
              
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }
        public int DeleteTest(SafetyTest test)
        {
            int success = 0;

            try
            {


                //string constring = test.ConnConfig;
                //using (SqlConnection con = new SqlConnection(constring))
                //{
                //    con.Open();
                //    using (SqlTransaction tans = con.BeginTransaction("TransactionDeleteTest"))
                //    {
                //        try
                //        {


                //            //1:Delete test
                //            SqlHelper.ExecuteNonQuery(tans, "spDeleteTest", test.LID);

                //            //2:Delete test
                //            SqlHelper.ExecuteNonQuery(tans, "spUpdateTestTypeCount", test.Typeid, "Decrement");

                //            //3:Delete Custom
                //            SqlHelper.ExecuteNonQuery(tans, "spDeleteTestCustomFieldValue", test.LID,test.Equipid);

                //            //3:Delete Custom
                //            SqlHelper.ExecuteNonQuery(tans, "spDeleteProposalFormByID", test.ProposalId);

                //            success = 1;



                //            tans.Commit();


                //        }
                //        catch (Exception exx)
                //        {
                //            success = -1;

                //            tans.Rollback("TransactionDeleteTest");
                //        }
                //    }

                //}


                try
                {



                    SqlHelper.ExecuteNonQuery(test.ConnConfig, "spDeleteTest",       test.LID);
                    success = 1;

                }
                catch (Exception ex)
                {
                    success = -1;
                    throw ex;
                }


            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }

            return success;
        }


        public DataSet GetAllTestCustom(String conn, String dbName)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, "spGetTestCustomFields", dbName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateAndUpdateTestCustom(TestCustom test)
        {
            int success = 0;
            String strConnString = test.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateTestCustomField";
            cmd.Parameters.Add("@TestCustomItem", SqlDbType.Structured).Value = test.TestCustomItem;
            cmd.Parameters.Add("@DeleteTestCustomItem", SqlDbType.Structured).Value = test.TestCustomItemDelete;
            cmd.Parameters.Add("@TestCustom", SqlDbType.Structured).Value = test.TestCustomValue;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return success;

        }

        public DataSet GetTestCustomValueByEquipTest(string conn, int testId, int equiqId)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sqGetTestCustomFieldsValueByEquipTest";
            cmd.Parameters.Add("@TestID", SqlDbType.Int).Value = testId;
            cmd.Parameters.Add("@EquipmentId", SqlDbType.Int).Value = equiqId;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "TestCustomValue");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }
        public DataSet GetTestCustomValueByEquipTestByYear(string conn, int testId, int equiqId,int year)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetTestCustomFieldsValueByTestYear";
            cmd.Parameters.Add("@TestID", SqlDbType.Int).Value = testId;
            cmd.Parameters.Add("@EquipmentId", SqlDbType.Int).Value = equiqId;
            cmd.Parameters.Add("@TestYear", SqlDbType.Int).Value = year;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "TestCustomValue");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }
        public int CreateAndUpdateTestCustomItemValue(string conn, DataTable dtTestItemValue)
        {
            int success = 0;
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateTestCustomFieldsValue";
            cmd.Parameters.Add("@TestCustomItemValue", SqlDbType.Structured).Value = dtTestItemValue;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return success;

        }

        public int CreateAndUpdateTestCustomItemValue(string conn, int testId, int equipmentID, DataTable dtTestItemValue)
        {
            int success = 0;
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateTestCustomFieldsValue";
            cmd.Parameters.Add("@TestID", SqlDbType.Int).Value = testId;
            cmd.Parameters.Add("@EquipmentID", SqlDbType.Int).Value = equipmentID;
            cmd.Parameters.Add("@TestCustomItemValue", SqlDbType.Structured).Value = dtTestItemValue;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return success;

        }
        public int CreateAndUpdateTestCustomItemValueByYear(string conn, int testId, int equipmentID, DataTable dtTestItemValue,int testyear)
        {
            int success = 0;
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateTestCustomFieldsValueByYear";
            cmd.Parameters.Add("@TestID", SqlDbType.Int).Value = testId;
            cmd.Parameters.Add("@EquipmentID", SqlDbType.Int).Value = equipmentID;
            cmd.Parameters.Add("@TestCustomItemValue", SqlDbType.Structured).Value = dtTestItemValue;
            cmd.Parameters.Add("@TestYear", SqlDbType.Int).Value = testyear;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return success;

        }

        public DataSet GetTestTypeById(string conn,int testId)
        {
            try
            {

                return SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT *, (CASE WHEN Remarks IS Null THEN ' ' ELSE Remarks END) AS Remarks FROM LoadTest where ID=" + testId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "Test Setup Forms"

        public DataSet GetAllTestSetupForms(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, "spGetTestSetupForms");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetTestSetupFormsById(String conn, int id)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetTestSetupFormsById";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "TestSetupForm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;
        }

        public int AddTestSetupForms(TestSetupForm testForm)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(testForm.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAddTestSetupForms";
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = testForm.Name;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = testForm.FileName;
            cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = testForm.FilePath;
            cmd.Parameters.Add("@MIME", SqlDbType.VarChar).Value = testForm.MIME;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = testForm.Type;
            cmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = testForm.IsActive;
            cmd.Parameters.Add("@AddedBy", SqlDbType.VarChar).Value = testForm.AddedBy;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                id = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            if (param.Value != DBNull.Value)
                id = Convert.ToInt32(param.Value);
            else
                id = -1;
            return id;

        }

        public void UpdateTestSetupForms(TestSetupForm testForm)
        {

            SqlConnection con = new SqlConnection(testForm.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateTestSetupForms";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = testForm.Id;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = testForm.Name;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = testForm.FileName;
            cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = testForm.FilePath;
            cmd.Parameters.Add("@MIME", SqlDbType.VarChar).Value = testForm.MIME;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = testForm.Type;
            cmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = testForm.IsActive;
            cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = testForm.UpdatedBy;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
        }
        public void DeleteTestSetupForms(string conn, int id)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(conn, "spDeleteTestSetupForms", id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllTestSetupFormsByType(String conn, int type)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetTestSetupFormsByType";
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
                cmd.Connection = con;
                try
                {
                    con.Open();
                    var sqlda = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    sqlda.Fill(ds, "TestSetupForms");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();

                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region "Proposal"
        public DataSet GetProposalFormByID(String conn, int id)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetProposalFormByID";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "ProposalForm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }
     
        public int AddProposalForm(ProposalForm proposalForm)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(proposalForm.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAddProposalForm";
            cmd.Parameters.Add("@LocID", SqlDbType.Int).Value = proposalForm.LocID;
            cmd.Parameters.Add("@Classification", SqlDbType.VarChar).Value = proposalForm.Classification;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = proposalForm.FileName;
            cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = proposalForm.FilePath;
            cmd.Parameters.Add("@PdfFilePath", SqlDbType.VarChar).Value = proposalForm.PdfFilePath;
            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = proposalForm.FromDate;
            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = proposalForm.ToDate;
            cmd.Parameters.Add("@AddedBy", SqlDbType.VarChar).Value = proposalForm.AddedBy;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = proposalForm.Type;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = proposalForm.Status;            
            cmd.Parameters.Add("@ListEquipment", SqlDbType.VarChar).Value = proposalForm.ListEquipment;
            cmd.Parameters.Add("@YearProposal", SqlDbType.Int).Value = proposalForm.YearProposal;
            cmd.Parameters.Add("@Chargable", SqlDbType.Bit).Value = proposalForm.Chargable;
            cmd.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = proposalForm.TestTypeID    ;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                id = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            if (param.Value != DBNull.Value)
                id = Convert.ToInt32(param.Value);
            else
                id = -1;
            return id;

        }
        public int CreateProposalForm(ProposalForm proposalForm)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(proposalForm.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spCreateProposalForm";
            cmd.Parameters.Add("@LocID", SqlDbType.Int).Value = proposalForm.LocID;
            cmd.Parameters.Add("@Classification", SqlDbType.VarChar).Value = proposalForm.Classification;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = proposalForm.FileName;
            cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = proposalForm.FilePath;
            cmd.Parameters.Add("@PdfFilePath", SqlDbType.VarChar).Value = proposalForm.PdfFilePath;
            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = proposalForm.FromDate;
            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = proposalForm.ToDate;
            cmd.Parameters.Add("@AddedBy", SqlDbType.VarChar).Value = proposalForm.AddedBy;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = proposalForm.Type;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = proposalForm.Status;
            cmd.Parameters.Add("@ListEquipment", SqlDbType.VarChar).Value = proposalForm.ListEquipment;
            cmd.Parameters.Add("@YearProposal", SqlDbType.Int).Value = proposalForm.YearProposal;
            cmd.Parameters.Add("@Chargable", SqlDbType.Bit).Value = proposalForm.Chargable;
            cmd.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = proposalForm.TestTypeID;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                id = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            if (param.Value != DBNull.Value)
                id = Convert.ToInt32(param.Value);
            else
                id = -1;
            return id;

        }
        public int AddProposalFormDetail(ProposalFormDetail proposalFormDetail)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(proposalFormDetail.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAddProposalFormDetail";
            cmd.Parameters.Add("@ProposalID", SqlDbType.Int).Value = proposalFormDetail.ProposalID;
            cmd.Parameters.Add("@EquipmentID", SqlDbType.Int).Value = proposalFormDetail.EquipmentID;
            cmd.Parameters.Add("@TestID", SqlDbType.Int).Value = proposalFormDetail.TestID;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = proposalFormDetail.Status;
            cmd.Parameters.Add("@YearProposal", SqlDbType.Int).Value = proposalFormDetail.YearProposal;
            cmd.Parameters.Add("@Chargable", SqlDbType.Bit).Value = proposalFormDetail.Chargable;

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                id = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            if (param.Value != DBNull.Value)
                id = Convert.ToInt32(param.Value);
            else
                id = -1;
            return id;

        }
        public void UpdateStatusProposalForm(String conn, int id, String status, String updatedBy)
        {

            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateStatusProposalForm";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
            cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = updatedBy;            
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetProposalByEquipmentID(String conn, int equipmentId)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllProposalByEquipmentID";
            cmd.Parameters.Add("@EquipmentId", SqlDbType.Int).Value = equipmentId;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "ProposalForm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }
        public DataSet GetProposalByTestID(String conn, int testId)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllProposalByTestID";
            cmd.Parameters.Add("@TestID", SqlDbType.Int).Value = testId;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "ProposalForm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }

        public DataSet GetAllTestForEquipmentInYear(String conn, int equipmentId,int yearProposal)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllTestForEquipment";
            cmd.Parameters.Add("@EquipmentId", SqlDbType.Int).Value = equipmentId;
            cmd.Parameters.Add("@YearProposal", SqlDbType.Int).Value = yearProposal;
          
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "ProposalForm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;

        }
        public DataSet GetSafetyTestForProposal(SafetyTest test, string column, string searchTerm)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGetSafetyTestForProposal", test.Startdate, test.Enddate, test.Typeid,
                    string.IsNullOrEmpty(column) ? null : column, string.IsNullOrEmpty(searchTerm) ? null : searchTerm, test.FlagEN, test.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SearchEmailTeam(String conn, String searchTerm)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, "spGetEmailTeam", searchTerm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProposalForm(String conn, int id)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(conn, "spDeleteProposalFormByID", id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateSenderInfoProposalForm(String conn, int id,  String sendTo, String sendFrom)
        {

            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateSenderInfoProposalForm";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@SendTo", SqlDbType.VarChar).Value = sendTo;
            cmd.Parameters.Add("@SendFrom", SqlDbType.VarChar).Value = sendFrom;
            
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public int AddProposalFormForEquipment(ProposalForm proposalForm)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(proposalForm.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAddProposalFormForEquipment";
            cmd.Parameters.Add("@LocID", SqlDbType.Int).Value = proposalForm.LocID;
            cmd.Parameters.Add("@Classification", SqlDbType.VarChar).Value = proposalForm.Classification;
            cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = proposalForm.FileName;
            cmd.Parameters.Add("@FilePath", SqlDbType.VarChar).Value = proposalForm.FilePath;
            cmd.Parameters.Add("@PdfFilePath", SqlDbType.VarChar).Value = proposalForm.PdfFilePath;
            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = proposalForm.FromDate;
            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = proposalForm.ToDate;
            cmd.Parameters.Add("@AddedBy", SqlDbType.VarChar).Value = proposalForm.AddedBy;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = proposalForm.Type;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = proposalForm.Status;
            cmd.Parameters.Add("@ListEquipment", SqlDbType.VarChar).Value = proposalForm.ListEquipment;
            cmd.Parameters.Add("@YearProposal", SqlDbType.Int).Value = proposalForm.YearProposal;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                id = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            if (param.Value != DBNull.Value)
                id = Convert.ToInt32(param.Value);
            else
                id = -1;
            return id;

        }
        #endregion
        public int UpdateTestCustomItemValue(SafetyTest obj)
        {
            int success = 0;
            SqlConnection con = new SqlConnection(obj.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateListTestCustomFieldValue";
            cmd.Parameters.Add("@tblSafetyTestUpdate", SqlDbType.Structured).Value = obj.Cus_TestItemValue;            
            cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = obj.UserName;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return success;

        }
        public int UpdateTestCustomItemValueByYear(SafetyTest obj)
        {
            int success = 0;
            SqlConnection con = new SqlConnection(obj.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateListTestCustomFieldValueByYear";
            cmd.Parameters.Add("@tblSafetyTestUpdate", SqlDbType.Structured).Value = obj.Cus_TestItemValue;
            cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = obj.UserName;
            cmd.Parameters.Add("@TestYear", SqlDbType.Int).Value = obj.PriceYear;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return success;

        }

        public DataSet GetTestLogs(SafetyTest obj)
        {
            try
            {
                return SqlHelper.ExecuteDataset(obj.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + obj.LID + "  and Screen='SafetyTest' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTestLogsByYear(SafetyTest obj,int LogYear)
        {      
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@LID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@LogYear";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = LogYear;             


               return SqlHelper.ExecuteDataset(obj.ConnConfig, CommandType.StoredProcedure, "spGetTestLogData", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomFieldAlert(SafetyTest obj)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter();
                para[0].ParameterName = "@tblSafetyTestUpdate";
                para[0].SqlDbType = SqlDbType.Structured;
                para[0].Value = obj.Cus_TestItemValue;


                return SqlHelper.ExecuteDataset(obj.ConnConfig, CommandType.StoredProcedure, "spGetCustomFieldAlert", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetAllWorkflows(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "spGetWorkflowFields");
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public int CreateAndUpdateWorkflow(Workflow wf)
        {
            int success = 0;
            String strConnString = wf.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateWorkflow";
            cmd.Parameters.Add("@WorkflowItem", SqlDbType.Structured).Value = wf.workflowItem;
            cmd.Parameters.Add("@DeleteWorkflowItem", SqlDbType.Structured).Value = wf.workflowItemDelete;
            cmd.Parameters.Add("@Workflow", SqlDbType.Structured).Value = wf.workflowValue;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return success;

        }

        public DataSet GetAllViolationStatus(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, "spGetAllViolationStatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void AddViolationStatus(ViolationStatus obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@Type";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = obj.Type;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Remarks";
                param[1].SqlDbType = SqlDbType.Text;
                param[1].Value = obj.Remark;

              
                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spAddViolationStatus", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateViolationStatus(ViolationStatus obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.ID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Type";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = obj.Type;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Remarks";
                param[2].SqlDbType = SqlDbType.Text;
                param[2].Value = obj.Remark;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spUpdateViolationStatus", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void DeleteViolationStatus(ViolationStatus obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.ID;            


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spDeleteViolationStatus", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region "violation Code"
        public DataSet GetAllViolationSection(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT ID, Name FROM ViolationSection");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetAllViolationCategory(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT ID, Name FROM ViolationCategory");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetAllViolationCode(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, "spGetAllViolationCode");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void AddViolationCode(ViolationCode obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@Code";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = obj.Code;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Desc";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = obj.Desc;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@SectionID";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = obj.SectionID;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@CategoryID";
                param[3].SqlDbType = SqlDbType.Int;
                param[3].Value = obj.CategoryID;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spAddViolationCode", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateViolationCode(ViolationCode obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.ID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Code";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = obj.Code;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Desc";
                param[2].SqlDbType = SqlDbType.VarChar;
                param[2].Value = obj.Desc;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@SectionID";
                param[3].SqlDbType = SqlDbType.Int;
                param[3].Value = obj.SectionID;

                param[4] = new SqlParameter();
                param[4].ParameterName = "@CategoryID";
                param[4].SqlDbType = SqlDbType.Int;
                param[4].Value = obj.CategoryID;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spUpdateViolationCode", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void DeleteViolationCode(ViolationCode obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.ID;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spDeleteViolationCode", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void AddViolationSection(ViolationCode obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@Name";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = obj.SectionName;      
              


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spAddViolationSection", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void AddViolationCategory(ViolationCode obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@Name";
                param[0].SqlDbType = SqlDbType.VarChar;
                param[0].Value = obj.CategoryName;



                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spAddViolationCategory", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        public DataSet GetPriceHistory(SafetyTest test)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGetPriceHistory", test.LID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTestSetupEmailFormsById(String conn, int id)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetTestSetupEmailFormsById";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            cmd.Connection = con;
            try
            {
                con.Open();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "TestSetupEmailForm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            return ds;
        }

        public DataSet GetAllTestSetupEmailForms(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, "spGetTestSetupEmailForms");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteTestSetupEmailForms(string conn, int id)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(conn, "spDeleteTestSetupEmailForms", id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateTestSetupEmailForms(TestSetupEmailForm testForm)
        {
            int id = 1;
            SqlConnection con = new SqlConnection(testForm.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateTestSetupEmailForms";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = testForm.Id;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = testForm.Name;
            cmd.Parameters.Add("@Body", SqlDbType.VarChar).Value = testForm.Body;           
            cmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = testForm.IsActive;
            cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = testForm.UpdatedBy;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@Result",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                id = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            if (param.Value != DBNull.Value)
                id = Convert.ToInt32(param.Value);
            else
                id = -1;
            return id;
            
        }

        public int AddTestSetupEmailForms(TestSetupEmailForm testForm)
        {
            int id = 0;
            SqlConnection con = new SqlConnection(testForm.ConnConfig);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spAddTestSetupEmailForms";
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = testForm.Name;
            cmd.Parameters.Add("@Body", SqlDbType.VarChar).Value = testForm.Body;   
            cmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = testForm.IsActive;
            cmd.Parameters.Add("@AddedBy", SqlDbType.VarChar).Value = testForm.AddedBy;
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                id = -1;
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
            if (param.Value != DBNull.Value)
                id = Convert.ToInt32(param.Value);
            else
                id = -1;
            return id;

        }

        public String AssignTicketToTest(SafetyTest obj, int oldTicket)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@id";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@Ticket";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = obj.Ticket;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@TestYear";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = obj.PriceYear;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@UpdatedBy";
                param[3].SqlDbType = SqlDbType.VarChar;
                param[3].Value = obj.UserName;

                param[4] = new SqlParameter();
                param[4].ParameterName = "returnval";
                param[4].SqlDbType = SqlDbType.Int;
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter();
                param[5].ParameterName = "@OldTicket";
                param[5].SqlDbType = SqlDbType.Int;
                param[5].Value = oldTicket;
                
              
                String msg = "";
                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spAssignTicketToTest", param);
                switch (Convert.ToInt32(param[4].Value))
                {
                    case 1:
                        msg = "Ticket is assigned successful";
                        break;
                    case 0:
                        msg = "Ticket does not exist";
                        break;
                    default:
                        msg = "Unable to assign ticket";
                        break;
                }
                return msg;               
               
            }
            catch (Exception ex)
            {
                return "Unable to assign ticket";
            }

        }

        public void UpdateTestScheduledStatus(string ConnConfig, string ScheduleStatusID , string ScheduledYear , string LID )
        {
            try
            {

                  SqlHelper.ExecuteDataset(ConnConfig, CommandType.Text,
                    "update  LoadTestItemSchedule set ScheduledStatus = " + ScheduleStatusID + "  where ScheduledYear = " + ScheduledYear + " and LID = " + LID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            public String UpdateTestScheduled(SafetyTest obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@id";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@ScheduledDate";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = obj.ScheduleDate;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@ScheduledYear";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = obj.PriceYear;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@ScheduledStatus";
                param[3].SqlDbType = SqlDbType.Int;
                param[3].Value = obj.ScheduleStatusID;


                param[4] = new SqlParameter();
                param[4].ParameterName = "@ScheduledWorker";
                param[4].SqlDbType = SqlDbType.VarChar;
                param[4].Value = obj.ScheduleWorker;

                param[5] = new SqlParameter();
                param[5].ParameterName = "@UpdatedBy";
                param[5].SqlDbType = SqlDbType.VarChar;
                param[5].Value = obj.UserName;

                param[6] = new SqlParameter();
                param[6].ParameterName = "@ReturnValue";
                param[6].SqlDbType = SqlDbType.Int;
                param[6].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spUpdateTestScheduled", param);
                String msg = "";
                switch (Convert.ToInt32(param[6].Value))
                {
                    case 1:
                        msg = "Scheduled is updated successful.";
                        break;
                    case 2:
                        msg = "Invalid date";
                        break;
                    default:
                        msg = "Worker does not exist";
                        break;
                }

                return msg;
                
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        public String UpdateTestScheduledDetail(SafetyTest obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@id";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@ScheduledDate";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = obj.ScheduleDate;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@ScheduledYear";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = obj.PriceYear;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@ScheduledStatus";
                param[3].SqlDbType = SqlDbType.Int;
                param[3].Value = obj.ScheduleStatusID;


                param[4] = new SqlParameter();
                param[4].ParameterName = "@ScheduledWorker";
                param[4].SqlDbType = SqlDbType.VarChar;
                param[4].Value = obj.ScheduleWorker;

                param[5] = new SqlParameter();
                param[5].ParameterName = "@UpdatedBy";
                param[5].SqlDbType = SqlDbType.VarChar;
                param[5].Value = obj.UserName;

                param[6] = new SqlParameter();
                param[6].ParameterName = "@ScheduleID";
                param[6].SqlDbType = SqlDbType.Int;
                param[6].Value = obj.ScheduleID;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spUpdateTestScheduledDetail", param);
                return "Scheduled is updated successful.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        public void DeleteTestScheduled(SafetyTest obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@LID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;                

                param[1] = new SqlParameter();
                param[1].ParameterName = "@ScheduledYear";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = obj.PriceYear;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spDeleteTestScheduled", param);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteTestScheduledDetail(SafetyTest obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@ScheduleID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.ScheduleID;

              


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spDeleteTestScheduledDetail", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteTestScheduleService(SafetyTest obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@LID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@ServiceYear";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = obj.PriceYear;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spDeleteTestService", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public String UpdateTestService(SafetyTest obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@id";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@ServiceDate";
                param[1].SqlDbType = SqlDbType.VarChar;
                param[1].Value = obj.ServiceDate;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@ServiceYear";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = obj.PriceYear;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@ServiceStatus";
                param[3].SqlDbType = SqlDbType.Int;
                param[3].Value = obj.ServiceStatusID;


                param[4] = new SqlParameter();
                param[4].ParameterName = "@ServiceWorker";
                param[4].SqlDbType = SqlDbType.VarChar;
                param[4].Value = obj.ServiceWorker;

                param[5] = new SqlParameter();
                param[5].ParameterName = "@UpdatedBy";
                param[5].SqlDbType = SqlDbType.VarChar;
                param[5].Value = obj.UserName;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spUpdateTestService", param);
                return "Service is updated successful.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public DataSet GetAllLoadTestItemSchedule(SafetyTest test)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGetAllLoadTestItemSchedule", test.LID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllLoadTestItemService(SafetyTest test)
        {
            try
            {
                return SqlHelper.ExecuteDataset(test.ConnConfig, "spGetAllLoadTestItemService", test.LID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdatePriceDetailByYear(SafetyTest obj, int updateAll,  int isNew)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@LID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@PriceYear";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = obj.PriceYear;

                param[2] = new SqlParameter();
                param[2].ParameterName = "@Charagable";
                param[2].SqlDbType = SqlDbType.Int;
                param[2].Value = obj.Charge;

                param[3] = new SqlParameter();
                param[3].ParameterName = "@DefaultAmount";
                param[3].SqlDbType = SqlDbType.Money;
                param[3].Value = obj.Amount;

                param[4] = new SqlParameter();
                param[4].ParameterName = "@OverrideAmount";
                param[4].SqlDbType = SqlDbType.Money;
                param[4].Value = obj.OverrideAmount;


                param[5] = new SqlParameter();
                param[5].ParameterName = "@ThirdParty";
                param[5].SqlDbType = SqlDbType.Int;
                param[5].Value = obj.ThirdParty;

                param[6] = new SqlParameter();
                param[6].ParameterName = "@ThirdPartyName";
                param[6].SqlDbType = SqlDbType.VarChar;
                param[6].Value = obj.ThirdPartyName;

                param[7] = new SqlParameter();
                param[7].ParameterName = "@ThirdPartyPhone";
                param[7].SqlDbType = SqlDbType.VarChar;
                param[7].Value = obj.ThirdPartyPhone;

                param[8] = new SqlParameter();
                param[8].ParameterName = "@UpdateBy";
                param[8].SqlDbType = SqlDbType.VarChar;
                param[8].Value = obj.UserName;

                param[9] = new SqlParameter();
                param[9].ParameterName = "@UpdateAll";
                param[9].SqlDbType = SqlDbType.Int;
                param[9].Value = updateAll;

                param[10] = new SqlParameter();
                param[10].ParameterName = "@IsNew";
                param[10].SqlDbType = SqlDbType.Int;
                param[10].Value = isNew;

                param[11] = new SqlParameter();
                param[11].ParameterName = "@ReturnValue";
                param[11].SqlDbType = SqlDbType.Int;
                param[11].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spUpdateTestPriceByYear", param);
                return Convert.ToInt32(param[11].Value);
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public void DeleteTestPriceByYear(SafetyTest obj)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter();
                param[0].ParameterName = "@LID";
                param[0].SqlDbType = SqlDbType.Int;
                param[0].Value = obj.LID;

                param[1] = new SqlParameter();
                param[1].ParameterName = "@PriceYear";
                param[1].SqlDbType = SqlDbType.Int;
                param[1].Value = obj.PriceYear;


                SqlHelper.ExecuteNonQuery(obj.ConnConfig, CommandType.StoredProcedure, "spDeleteTestPriceByYear", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
