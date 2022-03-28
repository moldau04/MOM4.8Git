using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.InventoryModel;
using BusinessEntity.Payroll;
using BusinessEntity.Recurring;
using BusinessEntity.Reports.IncomeStatements;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class BL_Report
    {
        DL_Report _objDLReport = new DL_Report();
        //public DataSet GetChartByAcctType(Chart _objChart)
        //{
        //    return _objDLReport.GetChartByAcctType(_objChart);
        //}
        public DataSet GetTypeForBalanceSheet(Chart _objChart)
        {
            return _objDLReport.GetTypeForBalanceSheet(_objChart);
        }
        public DataSet GetDataForBalanceSheet(Chart _objChart) //For Balance sheet
        {
            return _objDLReport.GetDataForBalanceSheet(_objChart);
        }
        public DataSet GetTypeForTrialBalance(Chart _objChart)
        {
            return _objDLReport.GetTypeForTrialBalance(_objChart);
        }
        public DataSet GetTypeForIncome(Chart _objChart)
        {
            return _objDLReport.GetTypeForIncome(_objChart);
        }
        public DataSet GetSubCategory(Chart _objChart)
        {
            return _objDLReport.GetSubCategory(_objChart);
        }
        public DataSet GetAcctDetailsBySubCat(Chart _objChart)
        {
            return _objDLReport.GetAcctDetailsBySubCat(_objChart);
        }
        public DataSet GetOtherAcctDetails(Chart _objChart)
        {
            return _objDLReport.GetOtherAcctDetails(_objChart);
        }
        public int GetFiscalYearData(User objPropUser)
        {
            return _objDLReport.GetFiscalYearData(objPropUser);
        }
        public int GetFiscalYear(User objPropUser)
        {
            return _objDLReport.GetFiscalYear(objPropUser);
        }
        public List<ActualvsBudgetDataResponse> GetActualvsBudgetedRevenue(string consString,string bID)
        {
            int? budgetID = null;
            if (string.IsNullOrEmpty(bID))
                budgetID = null;
            else
                 budgetID = Convert.ToInt32(bID);


            BL_User objBL_User = new BL_User();
            BL_Report objBL_Report = new BL_Report();
            var result = new List<ActualvsBudgetDataResponse>();
            List<ActualvsBudgetDataResponse> actualvsBudgetDataList = new List<ActualvsBudgetDataResponse>();
            User  objPropUser = new User();
            objPropUser.ConnConfig = consString;

            var currentDate = DateTime.Now;
            var fiscalYearMonth = objBL_Report.GetFiscalYearData(objPropUser);

            var endMonth = fiscalYearMonth + 1;
            var startMonth = endMonth == 12 ? 1 : endMonth + 1;

            var fiscalYear = currentDate.Year;
            if (DateTime.Now.Month > endMonth)
            {
                fiscalYear = currentDate.Year + 1;
                objPropUser.FStart = new DateTime(currentDate.Year, startMonth, 1);
                objPropUser.FEnd = new DateTime(currentDate.Year + 1, endMonth, DateTime.DaysInMonth(currentDate.Year + 1, endMonth));
            }
            else
            {
                if (startMonth == 1)
                {
                    objPropUser.FStart = new DateTime(currentDate.Year, startMonth, 1);
                }
                else
                {
                    fiscalYear = currentDate.Year - 1;
                    objPropUser.FStart = new DateTime(currentDate.Year - 1, startMonth, 1);
                }
                objPropUser.FEnd = new DateTime(currentDate.Year, endMonth, DateTime.DaysInMonth(currentDate.Year, endMonth));
            }

            var dSet = objBL_User.Get12MonthActualvsBudgetGraphData(objPropUser, budgetID, fiscalYear);

            var dtTable = dSet.Tables[0];

            for (int row = 0; row < dtTable.Rows.Count; row++)
            {
                var actualvsBudgetData = new ActualvsBudgetDataResponse()
                {
                    NTotal = double.Parse(dtTable.Rows[row]["NTotal"].ToString()),
                    NBudget = double.Parse(dtTable.Rows[row]["NBudget"].ToString()),
                    NMonth = dtTable.Rows[row]["NMonth"].ToString()
                };

                actualvsBudgetDataList.Add(actualvsBudgetData);
            }

            var listMonth = GetMonthLegend(objPropUser.FStart, objPropUser.FEnd);

            foreach (var month in listMonth)
            {
                ActualvsBudgetDataResponse data = new ActualvsBudgetDataResponse();
                var temp = actualvsBudgetDataList.FirstOrDefault(x => x.NMonth == month);
                data.NMonth = month;
                data.NBudget = temp != null ? temp.NBudget : 0;
                data.NTotal = temp != null ? temp.NTotal : 0;

                result.Add(data);
            }

            return result;
        }
        private IEnumerable<string> GetMonthLegend(DateTime startDate, DateTime endDate)
        {
            List<string> listMonth = new List<string>();

            var start = new DateTime(startDate.Year, startDate.Month, 1);
            while (start < endDate)
            {
                listMonth.Add(start.Date.ToString("MMM"));
                start = start.AddMonths(1);
            }

            return listMonth;
        }
        public DataSet GetCenterNames(User objPropUser)
        {
            return _objDLReport.GetCenterNames(objPropUser);
        }
        public DataSet GetCenters(User objPropUser)
        {
            return _objDLReport.GetCenters(objPropUser);
        }
        public DataSet GetBalanceSheetDetails(Chart _objChart, bool includeZero = false)
        {
            return _objDLReport.GetBalanceSheetDetails(_objChart, includeZero);
        }
        public DataSet GetIncomeStatementDetails(Chart _objChart)
        {
            return _objDLReport.GetIncomeStatementDetails(_objChart);
        }
        public DataSet GetIncomeStatementSummary(Chart _objChart)
        {
            return _objDLReport.GetIncomeStatementSummary(_objChart);
        }
        public DataSet GetIncomeStatementTotal(Chart _objChart)
        {
            return _objDLReport.GetIncomeStatementTotal(_objChart);
        }
        public DataSet GetIncomeStatementDetailsWithCenters(Chart _objChart)
        {
            return _objDLReport.GetIncomeStatementDetailsWithCenters(_objChart);
        }
        public DataSet GetIncomeStatementDetailsWithCentersAndBudgets(Chart _objChart)
        {
            return _objDLReport.GetIncomeStatementDetailsWithCentersAndBudgets(_objChart);
        }
        public DataSet GetDepartmentByCompanyID(User _objPropUser)
        {
            return _objDLReport.GetDepartmentByCompanyID(_objPropUser);
        }
        public DataSet GetDepartmentByDefaultCompanyID(User _objPropUser)
        {
            return _objDLReport.GetDepartmentByDefaultCompanyID(_objPropUser);
        }
        public DataSet GetTrialBalanceDetails(Chart _objChart, bool isCloseOut)
        {
            return _objDLReport.GetTrialBalanceDetails(_objChart, isCloseOut);
        }
        public DataSet GetTrialBalanceActivity(Chart _objChart, bool isCloseOut)
        {
            return _objDLReport.GetTrialBalanceActivity(_objChart, isCloseOut);
        }
        public DataSet GetPeriodClosedYear(User _objPropUser)
        {
            return _objDLReport.GetPeriodClosedYear(_objPropUser);
        }
        public DataSet GetIncomestatementBalance(Chart objChart)
        {
            return _objDLReport.GetIncomestatementBalance(objChart);
        }

        public DataSet GetPurchaseJournal(OpenAP _objOpenAp)
        {
            return _objDLReport.GetPurchaseJournal(_objOpenAp);
        }
        public List<OpenAPViewModel> GetPurchaseJournal(GetPurchaseJournalParam _GetPurchaseJournalParam, string ConnectionString)
        {
            DataSet ds = _objDLReport.GetPurchaseJournal( _GetPurchaseJournalParam,  ConnectionString);
            List<OpenAPViewModel> _lstPJViewModel = new List<OpenAPViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstPJViewModel.Add(
                    new OpenAPViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Post = Convert.ToDateTime(DBNull.Value.Equals(dr["Post"]) ? null : dr["Post"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        WeekCount = Convert.ToDouble(DBNull.Value.Equals(dr["WeekCount"]) ? 0 : dr["WeekCount"]),
                        WeekDate = Convert.ToDateTime(DBNull.Value.Equals(dr["WeekDate"]) ? null : dr["WeekDate"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        //Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                    }
                    );

            }
            return _lstPJViewModel;
        }
        public DataSet GetPOWeekly(PO objPO)
        {
            return _objDLReport.GetPOWeekly(objPO);
        }

        //API
        public List<GetPOWeeklyViewModel> GetPOWeekly(GetPOWeeklyParam _GetPOWeekly, string ConnectionString)
        {
            DataSet ds = _objDLReport.GetPOWeekly(_GetPOWeekly, ConnectionString);

            List<GetPOWeeklyViewModel> _lstGetPOWeekly = new List<GetPOWeeklyViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetPOWeekly.Add(
                    new GetPOWeeklyViewModel()
                    {
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        Post = Convert.ToDateTime(DBNull.Value.Equals(dr["Post"]) ? null : dr["Post"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                        VendorName = Convert.ToString(dr["VendorName"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        WeekCount = Convert.ToInt32(DBNull.Value.Equals(dr["WeekCount"]) ? 0 : dr["WeekCount"]),
                        WeekDate = Convert.ToDateTime(DBNull.Value.Equals(dr["WeekDate"]) ? null : dr["WeekDate"]),
                    }
                    );

            }

            return _lstGetPOWeekly;
        }

        public DataSet GetCustomerLabel(Customer objCust, Int32 IsSalesAsigned = 0)
        {
            return _objDLReport.GetCustomerLabel(objCust, IsSalesAsigned);
        }

        //Customer Ticket Category by Last Service Bl Function
        public DataSet GetCustomerTicketCategoryByLastService(Customer objCust,string[] category)
        {
            return _objDLReport.GetCustomerTikcketCategoryByLastService(objCust, category);
        }

        //Payroll- Federal 940 Form Report
        public DataSet GetPayrollFederal940FormReport(Contracts objPropContracts)
        {
            return _objDLReport.GetPayrollFederal940FormReport(objPropContracts);
        }

        //Payroll - Deduction Summary by Other Deduction report
        public DataSet GetPayrollDeductionSummarybyOtherDeductionReport(Contracts objPropContracts)
        {
            return _objDLReport.GetPayrollDeductionSummarybyOtherDeductionReport(objPropContracts);
        }

        //Payroll Register GL Cross-Reference
        public DataSet GetPayrollRegisterGLCrossReferenceReport(Contracts objPropContracts)
        {
            return _objDLReport.GetPayrollRegisterGLCrossReferenceReport(objPropContracts);
        }

        //Get BANK RECONCILIATION ITEMS CLEARED  REPORT
        public DataSet GetBankReconciliationItemsCleared(Customer objCust)
        {
            return _objDLReport.GetBankReconciliationItemsCleared(objCust);
        }

        //Get BANK RECONCILIATION ITEMS CLEARED  List
        public DataSet GetBankReconciliationItemsClearedList(Customer objCust)
        {
            return _objDLReport.GetBankReconciliationItemsClearedList(objCust);
        }

        //API
        public List<GetCustomerLabelViewModel> GetCustomerLabel(GetCustomerLabelParam _GetCustomerLabel, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds = _objDLReport.GetCustomerLabel(_GetCustomerLabel, ConnectionString, IsSalesAsigned);

            List<GetCustomerLabelViewModel> _lstGetCustomerLabel = new List<GetCustomerLabelViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerLabel.Add(
                    new GetCustomerLabelViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                    }
                    );
            }
            return _lstGetCustomerLabel;
        }

        public DataSet GetAccountLabel(Loc objLoc, Int32 IsSalesAsigned = 0)
        {
            return _objDLReport.GetAccountLabel(objLoc, IsSalesAsigned);
        }

        //API
        public List<GetAccountLabelViewModel> GetAccountLabel(GetAccountLabelParam _GetAccountLabel, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds = _objDLReport.GetAccountLabel(_GetAccountLabel, ConnectionString, IsSalesAsigned);
            List<GetAccountLabelViewModel> _lstGetAccountLabelViewModel = new List<GetAccountLabelViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAccountLabelViewModel.Add(
                    new GetAccountLabelViewModel()
                    {
                        Tag = Convert.ToString(dr["Tag"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                    }
                    );
            }

            return _lstGetAccountLabelViewModel;
        }
        public DataSet GetVendorLabel(Customer objCust)
        {
            return _objDLReport.GetVendorLabel(objCust);
        }
        public List<RolViewModel> GetVendorLabel(GetVendorLabelParam _GetVendorLabelParam, string ConnectionString)
        {
            DataSet ds = _objDLReport.GetVendorLabel( _GetVendorLabelParam,  ConnectionString);
            List<RolViewModel> _lstRolViewModel = new List<RolViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstRolViewModel.Add(
                    new RolViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                    }
                    );
            }

            return _lstRolViewModel;
        }
        public DataSet GetUserReport(Rol objRol, bool incInactive)
        {
            return _objDLReport.GetUserReport(objRol, incInactive);
        }

        public DataSet GetUserPaymentReport(User objPropUser, List<RetainFilter> filters)
        {
            return _objDLReport.GetUserPaymentReport(objPropUser, filters);
        }

        public DataSet GetFederalReport(CD _objCD)
        {
            return _objDLReport.GetFederalReport(_objCD);
        }

        public List<VendorViewModel> GetFederalReport(GetFederalReportParam _GetFederalReportParam, string ConnectionString)
        {
            DataSet ds = _objDLReport.GetFederalReport( _GetFederalReportParam,  ConnectionString);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Label = Convert.ToString(dr["Label"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Remit = Convert.ToString(dr["Remit"]),
                        FID = Convert.ToString(dr["FID"]),
                        intBox = Convert.ToInt16(DBNull.Value.Equals(dr["intBox"]) ? 0 : dr["intBox"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }
        public DataSet GetIncomeStatement12PeriodWithCenters(Chart objChart)
        {
            return _objDLReport.GetIncomeStatement12PeriodWithCenters(objChart);
        }
        public DataSet GetIncomeStatement12Period(Chart objChart)
        {
            return _objDLReport.GetIncomeStatement12Period(objChart);
        }

        public DataSet GetIncomeStatementYTD(Chart objChart)
        {
            return _objDLReport.GetIncomeStatementYTD(objChart);
        }

        public DataSet GetParentTables(string connString, string module)
        {
            return _objDLReport.GetParentTables(connString, module);
        }
        public DataSet GetChildTables(string connString, string parentTableName)
        {
            return _objDLReport.GetChildTables(connString, parentTableName);
        }
        public DataSet GetReportColumns(string connString, string reportTable, string module)
        {
            return _objDLReport.GetReportColumns(connString, reportTable, module);
        }
        public DataSet GetCustomColumns(string connString)
        {
            return _objDLReport.GetCustomColumns(connString);
        }
        public DataSet GetCompanyLogo(string connString)
        {
            return _objDLReport.GetCompanyLogo(connString);
        }
        public DataSet GetCompanyDetails(string connString)
        {
            return _objDLReport.GetCompanyDetails(connString);
        }
        public List<GetCompanyDetailsViewModel> GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
        {
            DataSet ds = _objDLReport.GetCompanyDetails(_GetCompanyDetailsParam, ConnectionString);
            List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCompanyDetailsViewModel.Add(
                    new GetCompanyDetailsViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Address = Convert.ToString(dr["Address"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Email = Convert.ToString(dr["Email"]),
                        //Logo = Encoding.ASCII.GetBytes(dr["Logo"].ToString()),
                        Logo = Convert.ToString(dr["Logo"]),
                        //Logo = Encoding.ASCII.GetBytes(dr["Logo"].ToString()),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        GSTreg = Convert.ToString(dr["GSTreg"]),
                        YE = Convert.ToInt32(DBNull.Value.Equals(dr["YE"]) ? 0 : dr["YE"]),
                        Version = Convert.ToInt32(DBNull.Value.Equals(dr["Version"]) ? 0 : dr["Version"]),
                        CDesc = Convert.ToString(dr["CDesc"]),
                        MSM = Convert.ToString(dr["MSM"]),
                        DSN = Convert.ToString(dr["DSN"]),
                        Username = Convert.ToString(dr["Username"]),
                        Password = Convert.ToString(dr["Password"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        BusinessStart = Convert.ToDateTime(DBNull.Value.Equals(dr["BusinessStart"]) ? null : dr["BusinessStart"]),
                        BusinessEnd = Convert.ToDateTime(DBNull.Value.Equals(dr["BusinessEnd"]) ? null : dr["BusinessEnd"]),
                    }
                    );;
            }

            return _lstGetCompanyDetailsViewModel;
        }

        public DataSet StandardIncomeStatementComparativeFsWithCenter(string connString, ComparativeFSWithCenterParam param)
        {
            return _objDLReport.StandardIncomeStatementComparativeFsWithCenter(connString, param);
        }
        public DataSet GetDepartmentByReceiptID(string connString, int receiptID)
        {
            return _objDLReport.GetDepartmentByReceiptID(connString, receiptID);
        }

        public int AddComparativeReport(string connString, int userID, string reportName, string departments, string states)
        {
            return _objDLReport.AddComparativeReport(connString, userID, reportName, departments, states);
        }

        public void UpdateComparativeReport(string connString, int reportID, string reportName, string departments)
        {
            _objDLReport.UpdateComparativeReport(connString, reportID, reportName, departments);
        }

        public DataSet GetComparativeReportByID(string connString, int reportID)
        {
            return _objDLReport.GetComparativeReportByID(connString, reportID);
        }

        public DataSet GetComparativeReportByName(string connString, string name, string states, int reportID)
        {
            return _objDLReport.GetComparativeReportByName(connString, name, states, reportID);
        }

        public DataSet GetComparativeReport(string connString, string states)
        {
            return _objDLReport.GetComparativeReport(connString, states);
        }
        
        public void DeleteComparativeReportByID(string connString, int reportID)
        {
            _objDLReport.DeleteComparativeReportByID(connString, reportID);
        }

        public DataSet GetComparativeReportColumns(string connString, int reportID)
        {
            return _objDLReport.GetComparativeReportColumns(connString, reportID);
        }

        public void AddComparativeColumn(string connString, int reportID, ComparativeStatementRequest request)
        {
            _objDLReport.AddComparativeColumn(connString, reportID, request);
        }

        public void DeleteComparativeColumnByReportID(string connString, int reportID)
        {
            _objDLReport.DeleteComparativeColumnByReportID(connString, reportID);
        }

        public DataSet GetLocationEquipment(string connString, int locID)
        {
            return _objDLReport.GetLocationEquipment(connString, locID);
        }


        //New Mwethod
        public DataSet GetQRCode(Loc objLoc)
        {
            //  return _objDLReport.GetAccountLabel(objLoc);
            return _objDLReport.GetAccountLabel(objLoc);
        }
        //End This 

        public DataSet GetContractListingByRoute(Chart objChart, string routes, bool isActiveOnly)
        {
            try
            {
                return _objDLReport.GetContractListingByRoute(objChart, routes, isActiveOnly);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectListingByRouteWithBudgetedHours(Chart objChart, string routes, string department, bool includeClose)
        {
            try
            {
                return _objDLReport.GetProjectListingByRouteWithBudgetedHours(objChart, routes, department, includeClose);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractRoute(Chart objChart)
        {
            try
            {
                return _objDLReport.GetContractRoute(objChart);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractGroupRoute(Chart objChart)
        {
            try
            {
                return _objDLReport.GetContractGroupRoute(objChart);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCompletedTicket(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                return _objDLReport.GetCompletedTicket(objPropMapData, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<GetCompletedTicketViewModel> GetCompletedTicket(GetCompletedTicketParam _GetCompletedTicket, List<RetainFilter> filters, string ConnectionString)
        {
            try
            {
                DataSet ds = _objDLReport.GetCompletedTicket(_GetCompletedTicket, filters, ConnectionString);

                List<GetCompletedTicketViewModel> _lstGetCompletedTicket = new List<GetCompletedTicketViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstGetCompletedTicket.Add(
                        new GetCompletedTicketViewModel()
                        {
                            ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                            CDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CDate"]) ? null : dr["CDate"]),
                            DDate = Convert.ToDateTime(DBNull.Value.Equals(dr["DDate"]) ? null : dr["DDate"]),
                            EDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EDate"]) ? null : dr["EDate"]),
                            fWork = Convert.ToInt32(DBNull.Value.Equals(dr["fWork"]) ? 0 : dr["fWork"]),
                            Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                            Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                            Elev = Convert.ToInt32(DBNull.Value.Equals(dr["Elev"]) ? 0 : dr["Elev"]),
                            Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                            fDesc = Convert.ToString(dr["Remarks"]),
                            DescRes = Convert.ToString(dr["DescRes"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Reg = Convert.ToDouble(DBNull.Value.Equals(dr["Reg"]) ? 0 : dr["Reg"]),
                            OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                            DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                            TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
                            Zone = Convert.ToDouble(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
                            Toll = Convert.ToDouble(DBNull.Value.Equals(dr["Toll"]) ? 0 : dr["Toll"]),
                            OtherE = Convert.ToDouble(DBNull.Value.Equals(dr["OtherE"]) ? 0 : dr["OtherE"]),
                            Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                            Invoice = Convert.ToInt32(DBNull.Value.Equals(dr["Invoice"]) ? 0 : dr["Invoice"]),
                            Level = Convert.ToInt16(DBNull.Value.Equals(dr["Level"]) ? 0 : dr["Level"]),
                            Est = Convert.ToDouble(DBNull.Value.Equals(dr["Est"]) ? 0 : dr["Est"]),
                            Cat = Convert.ToString(dr["Cat"]),
                            Who = Convert.ToString(dr["Who"]),
                            fBy = Convert.ToString(dr["fBy"]),
                            fLong = Convert.ToInt32(DBNull.Value.Equals(dr["fLong"]) ? 0 : dr["fLong"]),
                            Latt = Convert.ToInt32(DBNull.Value.Equals(dr["Latt"]) ? 0 : dr["Latt"]),
                            WageC = Convert.ToInt32(DBNull.Value.Equals(dr["WageC"]) ? 0 : dr["WageC"]),
                            Phase = Convert.ToInt16(DBNull.Value.Equals(dr["Phase"]) ? 0 : dr["Phase"]),
                            Car = Convert.ToInt32(DBNull.Value.Equals(dr["Car"]) ? 0 : dr["Car"]),
                            CallIn = Convert.ToInt16(DBNull.Value.Equals(dr["CallIn"]) ? 0 : dr["CallIn"]),
                            Mileage = Convert.ToDouble(DBNull.Value.Equals(dr["Mileage"]) ? 0 : dr["Mileage"]),
                            NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                            CauseID = Convert.ToInt32(DBNull.Value.Equals(dr["CauseID"]) ? 0 : dr["CauseID"]),
                            CauseDesc = Convert.ToString(dr["CauseDesc"]),
                            fGroup = Convert.ToString(dr["fGroup"]),
                            PriceL = Convert.ToInt32(DBNull.Value.Equals(dr["PriceL"]) ? 0 : dr["PriceL"]),
                            WorkOrder = Convert.ToString(dr["WorkOrder"]),
                            TimeRoute = Convert.ToDateTime(DBNull.Value.Equals(dr["TimeRoute"]) ? null : dr["TimeRoute"]),
                            TimeSite = Convert.ToDateTime(DBNull.Value.Equals(dr["TimeSite"]) ? null : dr["TimeSite"]),
                            TimeComp = Convert.ToDateTime(DBNull.Value.Equals(dr["TimeComp"]) ? null : dr["TimeComp"]),
                            JobType = Convert.ToString(dr["JobType"]),
                            Mech = Convert.ToString(dr["Mech"]),
                            Tag = Convert.ToString(dr["Tag"]),
                            Address = Convert.ToString(dr["Address"]),
                            City = Convert.ToString(dr["City"]),
                            State = Convert.ToString(dr["State"]),
                            Zip = Convert.ToString(dr["Zip"]),
                            Assignname = Convert.ToString(dr["Assignname"]),
                            Unit = Convert.ToString(dr["Unit"]),
                            Signature = Convert.ToString(dr["Signature"]),
                        }
                        );
                }

                return _lstGetCompletedTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEntrapmentTickets(MapData objPropMapData, List<RetainFilter> filters, string levels)
        {
            try
            {
                return _objDLReport.GetEntrapmentTickets(objPropMapData, filters, levels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCompletedTicketWarning(MapData objPropMapData, List<RetainFilter> filters, bool isNewCall, string installationFilter)
        {
            try
            {
                return _objDLReport.GetCompletedTicketWarning(objPropMapData, filters, isNewCall, installationFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketListForReport(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                return _objDLReport.GetTicketListForReport(objPropMapData, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMaintenanceUnitCount(Chart objChart, string routes)
        {
            try
            {
                return _objDLReport.GetMaintenanceUnitCount(objChart, routes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Maintenance Equipment Count By Start Date
        public DataSet GetMaintenanceUnitCountByDate(Contracts objPropContracts, List<RetainFilter> filters)
        {
            try
            {
                return _objDLReport.GetMaintenanceUnitCountByDate(objPropContracts, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<GetMaintenanceUnitCountViewModel> GetMaintenanceUnitCount(GetMaintenanceUnitCountParam _GetMaintenanceUnitCount, string ConnectionString, string routes)
        {
            try
            {
                DataSet ds = _objDLReport.GetMaintenanceUnitCount(_GetMaintenanceUnitCount, ConnectionString, routes);
                List<GetMaintenanceUnitCountViewModel> _lstGetMaintenanceUnitCount = new List<GetMaintenanceUnitCountViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstGetMaintenanceUnitCount.Add(
                        new GetMaintenanceUnitCountViewModel()
                        {
                            Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                            Tag = Convert.ToString(dr["Tag"]),
                            LocType = Convert.ToString(dr["LocType"]),
                            Address = Convert.ToString(dr["Address"]),
                            RouteID = Convert.ToInt32(DBNull.Value.Equals(dr["RouteID"]) ? 0 : dr["RouteID"]),
                            Route = Convert.ToString(dr["Route"]),
                            Mech = Convert.ToString(dr["Mech"]),
                            UnitType = Convert.ToString(dr["UnitType"]),
                            Cat = Convert.ToString(dr["Cat"]),
                            UnitCount = Convert.ToInt32(DBNull.Value.Equals(dr["UnitCount"]) ? 0 : dr["UnitCount"]),
                            Area = Convert.ToString(dr["Area"]),
                            Jobs = Convert.ToString(dr["Jobs"]),
                            TotalCount = Convert.ToInt32(DBNull.Value.Equals(dr["TotalCount"]) ? 0 : dr["TotalCount"]),
                            AreaCount = Convert.ToInt32(DBNull.Value.Equals(dr["AreaCount"]) ? 0 : dr["AreaCount"]),
                            RouteCount = Convert.ToInt32(DBNull.Value.Equals(dr["RouteCount"]) ? 0 : dr["RouteCount"]),
                            LocCount = Convert.ToInt32(DBNull.Value.Equals(dr["LocCount"]) ? 0 : dr["LocCount"]),
                            RouteCatTypeCount = Convert.ToInt32(DBNull.Value.Equals(dr["RouteCatTypeCount"]) ? 0 : dr["RouteCatTypeCount"]),
                            AreaCatTypeCount = Convert.ToInt32(DBNull.Value.Equals(dr["AreaCatTypeCount"]) ? 0 : dr["AreaCatTypeCount"]),
                            TotalCatTypeCount = Convert.ToInt32(DBNull.Value.Equals(dr["TotalCatTypeCount"]) ? 0 : dr["TotalCatTypeCount"]),
                        }
                        );
                }

                return _lstGetMaintenanceUnitCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMaintenanceUnitCountRoute(Chart objChart)
        {
            try
            {
                return _objDLReport.GetMaintenanceUnitCountRoute(objChart);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<GetMaintenanceUnitCountRouteViewModel> GetMaintenanceUnitCountRoute(GetMaintenanceUnitCountRouteParam _GetMaintenanceUnzitCountRoute, string ConnectionString)
        {
            try
            {
                DataSet ds = _objDLReport.GetMaintenanceUnitCountRoute(_GetMaintenanceUnzitCountRoute, ConnectionString);

                List<GetMaintenanceUnitCountRouteViewModel> _lstGetMaintenanceUnitCountRoute = new List<GetMaintenanceUnitCountRouteViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstGetMaintenanceUnitCountRoute.Add(
                        new GetMaintenanceUnitCountRouteViewModel()
                        {
                            Area = Convert.ToString(dr["Area"]),
                        }
                        );
                }

                return _lstGetMaintenanceUnitCountRoute;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMaintenanceControlPlan(Chart objChart, int elevID)
        {
            try
            {
                return _objDLReport.GetMaintenanceControlPlan(objChart, elevID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMCPBuildingHistory(Chart objChart, int elevID)
        {
            try
            {
                return _objDLReport.GetMCPBuildingHistory(objChart, elevID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurringMaintenanceHistory(Chart objChart, int locID)
        {
            try
            {
                return _objDLReport.GetRecurringMaintenanceHistory(objChart, locID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetShutDownHistory(Chart objChart, int locID)
        {
            try
            {
                return _objDLReport.GetShutDownHistory(objChart, locID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationAndContractDetail(Chart objChart, int locID)
        {
            try
            {
                return _objDLReport.GetLocationAndContractDetail(objChart, locID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUnitInspectedTrimNotCompleteReport(string connString)
        {
            try
            {
                return _objDLReport.GetUnitInspectedTrimNotCompleteReport(connString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUnitFinishedTrimNotCompleteReport(string connString)
        {
            try
            {
                return _objDLReport.GetUnitFinishedTrimNotCompleteReport(connString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSubstantialCompletionFinalNotPaidReport(string connString)
        {
            try
            {
                return _objDLReport.GetSubstantialCompletionFinalNotPaidReport(connString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSubstantialCompletionDeliveryNotPaidReport(string connString)
        {
            try
            {
                return _objDLReport.GetSubstantialCompletionDeliveryNotPaidReport(connString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCompletedTicketByRoute(Chart objChart, string routes)
        {
            try
            {
                return _objDLReport.GetCompletedTicketByRoute(objChart, routes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeStatementData(Chart objChart, List<ComparativeStatementRequest> objComparative)
        {
            return _objDLReport.GetComparativeStatementData(objChart, objComparative);
        }

        public DataSet GetComparativeStatementSummaryData(Chart objChart, List<ComparativeStatementRequest> objComparative)
        {
            return _objDLReport.GetComparativeStatementSummaryData(objChart, objComparative);
        }

        public DataSet GetComparativeBalanceSheetData(Chart objChart, List<ComparativeStatementRequest> objComparative)
        {
            return _objDLReport.GetComparativeBalanceSheetData(objChart, objComparative);
        }

        public DataSet GetPastDueMCPData(Chart objChart)
        {
            return _objDLReport.GetPastDueMCPData(objChart);
        }

        //API
        public List<GetPastDueMCPDataViewModel> GetPastDueMCPData(GetPastDueMCPDataParam _GetPastDueMCPData, string ConnectionString)
        {
            DataSet ds = _objDLReport.GetPastDueMCPData(_GetPastDueMCPData, ConnectionString);

            List<GetPastDueMCPDataViewModel> _lstGetPastDueMCPData = new List<GetPastDueMCPDataViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetPastDueMCPData.Add(
                    new GetPastDueMCPDataViewModel()
                    {
                        LocationName = Convert.ToString(dr["LocationName"]),
                        Unit = Convert.ToString(dr["Unit"]),
                        DefaultWorker = Convert.ToString(dr["DefaultWorker"]),
                        Name = Convert.ToString(dr["Name"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        EquipT = Convert.ToInt32(DBNull.Value.Equals(dr["EquipT"]) ? 0 : dr["EquipT"]),
                        Code = Convert.ToString(dr["Code"]),
                        Section = Convert.ToString(dr["Section"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Lastdate = Convert.ToDateTime(DBNull.Value.Equals(dr["Lastdate"]) ? null : dr["Lastdate"]),
                        NextDateDue = Convert.ToDateTime(DBNull.Value.Equals(dr["NextDateDue"]) ? null : dr["NextDateDue"]),
                        Frequency = Convert.ToInt32(DBNull.Value.Equals(dr["Frequency"]) ? 0 : dr["Frequency"]),
                        FrequencyName = Convert.ToString(dr["FrequencyName"]),
                    }
                    );
            }

            return _lstGetPastDueMCPData;

        }

        public DataSet GetMonthlyRecurringHours(Chart objChart, string routes)
        {
            return _objDLReport.GetMonthlyRecurringHours(objChart, routes);
        }

        //API
        public List<GetMonthlyRecurringHoursViewModel> GetMonthlyRecurringHours(GetMonthlyRecurringHoursParam _GetMonthlyRecurringHours, string ConnectionString, string routes)
        {
            DataSet ds = _objDLReport.GetMonthlyRecurringHours(_GetMonthlyRecurringHours, ConnectionString, routes);
            List<GetMonthlyRecurringHoursViewModel> _lstGetMonthlyRecurringHours = new List<GetMonthlyRecurringHoursViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetMonthlyRecurringHours.Add(
                    new GetMonthlyRecurringHoursViewModel()
                    {
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        MonthlyHours = Convert.ToInt32(DBNull.Value.Equals(dr["MonthlyHours"]) ? 0 : dr["MonthlyHours"]),
                        RouteName = Convert.ToString(dr["RouteName"]),
                        MechName = Convert.ToString(dr["MechName"]),
                        ActualHours = Convert.ToInt32(DBNull.Value.Equals(dr["ActualHours"]) ? 0 : dr["ActualHours"]),
                        EquipCount = Convert.ToInt32(DBNull.Value.Equals(dr["EquipCount"]) ? 0 : dr["EquipCount"]),
                    }
                    );
            }

            return _lstGetMonthlyRecurringHours;
        }

        public DataSet GetMonthlyRecurringHoursTEI(Chart objChart, string routes, bool isRecurring = true)
        {
            return _objDLReport.GetMonthlyRecurringHoursTEI(objChart, routes, isRecurring);
        }

        //API
        public List<GetMonthlyRecurringHoursTEIViewModel> GetMonthlyRecurringHoursTEI(GetMonthlyRecurringHoursTEIParam _GetMonthlyRecurringHoursTEI, string ConnectionString, string routes)
        {
            DataSet ds = _objDLReport.GetMonthlyRecurringHoursTEI(_GetMonthlyRecurringHoursTEI, ConnectionString, routes);

            List<GetMonthlyRecurringHoursTEIViewModel> _lstGetMonthlyRecurringHoursTEI = new List<GetMonthlyRecurringHoursTEIViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetMonthlyRecurringHoursTEI.Add(
                    new GetMonthlyRecurringHoursTEIViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        MonthlyHours = Convert.ToInt32(DBNull.Value.Equals(dr["MonthlyHours"]) ? 0 : dr["MonthlyHours"]),
                        RouteName = Convert.ToString(dr["RouteName"]),
                        MechName = Convert.ToString(dr["MechName"]),
                        Cat = Convert.ToString(dr["Cat"]),
                        Department = Convert.ToString(dr["Department"]),
                        ActualHours = Convert.ToInt32(DBNull.Value.Equals(dr["ActualHours"]) ? 0 : dr["ActualHours"]),
                        EquipCount = Convert.ToInt32(DBNull.Value.Equals(dr["EquipCount"]) ? 0 : dr["EquipCount"]),
                    }
                    );
            }

            return _lstGetMonthlyRecurringHoursTEI;
        }

        public DataSet GetChecksReportData(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                return _objDLReport.GetChecksReportData(objPropMapData, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CDViewModel> GetChecksReportData(GetChecksReportDataParam _GetChecksReportDataParam, string ConnectionString)
        {
            try
            {
                DataSet ds = _objDLReport.GetChecksReportData(_GetChecksReportDataParam, ConnectionString);

                List<CDViewModel> _lstCDViewModel = new List<CDViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstCDViewModel.Add(
                        new CDViewModel()
                        {
                            ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                            Vendor = Convert.ToInt32(DBNull.Value.Equals(dr["Vendor"]) ? 0 : dr["Vendor"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            Bank = Convert.ToInt32(DBNull.Value.Equals(dr["Bank"]) ? 0 : dr["Bank"]),
                            BankName = Convert.ToString(dr["BankName"]),
                            Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                            Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                            ACH = Convert.ToInt16(DBNull.Value.Equals(dr["ACH"]) ? 0 : dr["ACH"]),
                            Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                            French = Convert.ToString(dr["French"]),
                            Memo = Convert.ToString(dr["Memo"]),
                            VoidR = Convert.ToString(dr["VoidR"]),
                            Sel = Convert.ToInt32(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                            TypeName = Convert.ToString(dr["TypeName"]),
                            StatusName = Convert.ToString(dr["StatusName"]),
                        }
                        );
                }

                return _lstCDViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationEquipmentList(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                return _objDLReport.GetLocationEquipmentList(objPropMapData, filters, includeInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<GetLocationEquipmentListViewModel> GetLocationEquipmentList(GetLocationEquipmentListParam _GetLocationEquipmentList, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                DataSet ds = _objDLReport.GetLocationEquipmentList(_GetLocationEquipmentList, ConnectionString, filters, includeInactive);

                List<GetLocationEquipmentListViewModel> _lstGetLocationEquipmentList = new List<GetLocationEquipmentListViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstGetLocationEquipmentList.Add(
                        new GetLocationEquipmentListViewModel()
                        {
                            ID = Convert.ToString(dr["ID"]),
                            Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                            Tag = Convert.ToString(dr["Tag"]),
                            Address = Convert.ToString(dr["Address"]),
                            City = Convert.ToString(dr["City"]),
                            State = Convert.ToString(dr["State"]),
                            Zip = Convert.ToString(dr["Zip"]),
                            RolName = Convert.ToString(dr["RolName"]),
                            RolAddress = Convert.ToString(dr["RolAddress"]),
                            RolCity = Convert.ToString(dr["RolCity"]),
                            RolState = Convert.ToString(dr["RolState"]),
                            RolZip = Convert.ToString(dr["RolZip"]),
                            Contact = Convert.ToString(dr["Contact"]),
                            Unit = Convert.ToString(dr["Unit"]),
                        }
                        );
                }

                return _lstGetLocationEquipmentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationByBusinessType(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                return _objDLReport.GetLocationByBusinessType(objPropMapData, filters, includeInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public ListGetLocationByBusinessType GetLocationByBusinessType(GetLocationByBusinessTypeParam _GetLocationByBusinessType, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                DataSet ds = _objDLReport.GetLocationByBusinessType(_GetLocationByBusinessType, ConnectionString, filters, includeInactive);

                ListGetLocationByBusinessType _ds = new ListGetLocationByBusinessType();
                List<GetLocationByBusinessTypeTable> _lstTable = new List<GetLocationByBusinessTypeTable>();
                List<GetLocationByBusinessTypeTable1> _lstTable1 = new List<GetLocationByBusinessTypeTable1>();
                List<GetLocationByBusinessTypeTable2> _lstTable2 = new List<GetLocationByBusinessTypeTable2>();
                List<GetLocationByBusinessTypeTable3> _lstTable3 = new List<GetLocationByBusinessTypeTable3>();
                List<GetLocationByBusinessTypeTable4> _lstTable4 = new List<GetLocationByBusinessTypeTable4>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable.Add(
                        new GetLocationByBusinessTypeTable()
                        {
                            ID = Convert.ToString(dr["ID"]),
                            Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                            Tag = Convert.ToString(dr["Tag"]),
                            Address = Convert.ToString(dr["Address"]),
                            City = Convert.ToString(dr["City"]),
                            State = Convert.ToString(dr["State"]),
                            Zip = Convert.ToString(dr["Zip"]),
                            Type = Convert.ToString(dr["Type"]),
                            Terr = Convert.ToInt32(DBNull.Value.Equals(dr["Terr"]) ? 0 : dr["Terr"]),
                            SMan = Convert.ToInt32(DBNull.Value.Equals(dr["SMan"]) ? 0 : dr["SMan"]),
                            Salesperson  = Convert.ToString(dr["Salesperson"]),
                            BusinessType = Convert.ToString(dr["BusinessType"]),
                            Elevs = Convert.ToInt32(DBNull.Value.Equals(dr["Elevs"]) ? 0 : dr["Elevs"]),
                            Unit = Convert.ToString(dr["Unit"]),
                        }
                        );
                }

                _ds.lstTable = _lstTable;

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    _lstTable1.Add(
                        new GetLocationByBusinessTypeTable1()
                        {
                            BusinessType = Convert.ToString(dr["BusinessType"]),
                            LocCount = Convert.ToInt32(DBNull.Value.Equals(dr["LocCount"]) ? 0 : dr["LocCount"]),
                        }
                        );
                }
                _ds.lstTable1 = _lstTable1;

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    _lstTable2.Add(
                        new GetLocationByBusinessTypeTable2()
                        {
                            BusinessType = Convert.ToString(dr["BusinessType"]),
                            Terr = Convert.ToInt32(DBNull.Value.Equals(dr["Terr"]) ? 0 : dr["Terr"]),
                            Salesperson = Convert.ToString(dr["Salesperson"]),
                            LocCount = Convert.ToInt32(DBNull.Value.Equals(dr["LocCount"]) ? 0 : dr["LocCount"]),
                        }
                        );
                }
                _ds.lstTable2 = _lstTable2;


                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    _lstTable3.Add(
                        new GetLocationByBusinessTypeTable3()
                        {
                            BusinessType = Convert.ToString(dr["BusinessType"]),
                            ElevCount = Convert.ToInt32(DBNull.Value.Equals(dr["ElevCount"]) ? 0 : dr["ElevCount"]),
                        }
                        );
                }
                _ds.lstTable3 = _lstTable3;

                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    _lstTable4.Add(
                        new GetLocationByBusinessTypeTable4()
                        {
                            BusinessType = Convert.ToString(dr["BusinessType"]),
                            Terr = Convert.ToInt32(DBNull.Value.Equals(dr["Terr"]) ? 0 : dr["Terr"]),
                            Salesperson = Convert.ToString(dr["Salesperson"]),
                            ElevCount = Convert.ToInt32(DBNull.Value.Equals(dr["ElevCount"]) ? 0 : dr["ElevCount"]),
                        }
                        );
                }

                _ds.lstTable4 = _lstTable4;

                return _ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationDetailsReport(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                return _objDLReport.GetLocationDetailsReport(objPropMapData, filters, includeInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationWithHomeOwnerReport(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                return _objDLReport.GetLocationWithHomeOwnerReport(objPropMapData, filters, includeInactive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<GetLocationDetailsReportViewModel> GetLocationDetailsReport(GetLocationDetailsReportParam _GetLocationDetailsReport, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                DataSet ds = _objDLReport.GetLocationDetailsReport(_GetLocationDetailsReport, ConnectionString, filters, includeInactive);

                List<GetLocationDetailsReportViewModel> _lstGetLocationDetailsReport = new List<GetLocationDetailsReportViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstGetLocationDetailsReport.Add(
                        new GetLocationDetailsReportViewModel()
                        {
                            Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                            ID = Convert.ToString(dr["ID"]),
                            Tag = Convert.ToString(dr["Tag"]),
                            LocAddress = Convert.ToString(dr["LocAddress"]),
                            LocCity = Convert.ToString(dr["LocCity"]),
                            LocState = Convert.ToString(dr["LocState"]),
                            LocZip = Convert.ToString(dr["LocZip"]),
                            LocCountry = Convert.ToString(dr["LocCountry"]),
                            Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                            Consult = Convert.ToInt32(DBNull.Value.Equals(dr["Consult"]) ? 0 : dr["Consult"]),
                            Type = Convert.ToString(dr["Type"]),
                            Route = Convert.ToInt32(DBNull.Value.Equals(dr["Route"]) ? 0 : dr["Route"]),
                            Terr = Convert.ToInt32(DBNull.Value.Equals(dr["Terr"]) ? 0 : dr["Terr"]),
                            Terr2 = Convert.ToInt32(DBNull.Value.Equals(dr["Terr2"]) ? 0 : dr["Terr2"]),
                            City = Convert.ToString(dr["City"]),
                            State = Convert.ToString(dr["State"]),
                            Zip = Convert.ToString(dr["Zip"]),
                            Country = Convert.ToString(dr["Country"]),
                            Address = Convert.ToString(dr["Address"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Contact = Convert.ToString(dr["Contact"]),
                            Name = Convert.ToString(dr["Name"]),
                            Phone = Convert.ToString(dr["Phone"]),
                            Website = Convert.ToString(dr["Website"]),
                            EMail = Convert.ToString(dr["EMail"]),
                            Cellular = Convert.ToString(dr["Cellular"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                            Stax = Convert.ToString(dr["Stax"]),
                            STax2 = Convert.ToString(dr["STax2"]),
                            UTax = Convert.ToString(dr["UTax"]),
                            Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
                            PrintInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["PrintInvoice"]) ? false : dr["PrintInvoice"]),
                            EmailInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailInvoice"]) ? false : dr["EmailInvoice"]),
                            Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                            Billing = Convert.ToInt16(DBNull.Value.Equals(dr["Billing"]) ? 0 : dr["Billing"]),
                            QBLocID = Convert.ToString(dr["QBLocID"]),
                            DefaultTerms = Convert.ToInt32(DBNull.Value.Equals(dr["DefaultTerms"]) ? 0 : dr["DefaultTerms"]),
                            Lat = Convert.ToString(dr["Lat"]),
                            Lng = Convert.ToString(dr["Lng"]),
                            Custom1 = Convert.ToString(dr["Custom1"]),
                            Custom2 = Convert.ToString(dr["Custom2"]),
                            Custom12 = Convert.ToString(dr["Custom12"]),
                            Custom13 = Convert.ToString(dr["Custom13"]),
                            Custom14 = Convert.ToString(dr["Custom14"]),
                            Custom15 = Convert.ToString(dr["Custom15"]),
                            Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                            DefWork = Convert.ToString(dr["DefWork"]),
                            Credit = Convert.ToInt16(DBNull.Value.Equals(dr["Credit"]) ? 0 : dr["Credit"]),
                            Dispalert = Convert.ToInt16(DBNull.Value.Equals(dr["Dispalert"]) ? 0 : dr["Dispalert"]),
                            CreditReason = Convert.ToString(dr["CreditReason"]),
                            CustomerSageID = Convert.ToString(dr["CustomerSageID"]),
                            BillRate = Convert.ToDouble(DBNull.Value.Equals(dr["BillRate"]) ? 0 : dr["BillRate"]),
                            RateOT = Convert.ToDouble(DBNull.Value.Equals(dr["RateOT"]) ? 0 : dr["RateOT"]),
                            RateNT = Convert.ToDouble(DBNull.Value.Equals(dr["RateNT"]) ? 0 : dr["RateNT"]),
                            RateDT = Convert.ToDouble(DBNull.Value.Equals(dr["RateDT"]) ? 0 : dr["RateDT"]),
                            RateTravel = Convert.ToDouble(DBNull.Value.Equals(dr["RateTravel"]) ? 0 : dr["RateTravel"]),
                            RateMileage = Convert.ToDouble(DBNull.Value.Equals(dr["RateMileage"]) ? 0 : dr["RateMileage"]),
                            Rate = Convert.ToDouble(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                            GstRate = Convert.ToDouble(DBNull.Value.Equals(dr["GstRate"]) ? 0 : dr["GstRate"]),
                            NoCustomerStatement = Convert.ToBoolean(DBNull.Value.Equals(dr["NoCustomerStatement"]) ? false : dr["NoCustomerStatement"]),
                            Salesperson = Convert.ToString(dr["Salesperson"]),
                            RouteName = Convert.ToString(dr["RouteName"]),
                            ConsultantName = Convert.ToString(dr["ConsultantName"]),
                            OwnerName = Convert.ToString(dr["OwnerName"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BusinessTypeID = Convert.ToInt32(DBNull.Value.Equals(dr["BusinessTypeID"]) ? 0 : dr["BusinessTypeID"]),
                            sTaxType = Convert.ToInt16(DBNull.Value.Equals(dr["sTaxType"]) ? 0 : dr["sTaxType"]),

                        }
                        );
                }

                return _lstGetLocationDetailsReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Job Project Approval DCA
        /// Only for Accredited
        /// </summary>
        /// <param name="objChart"></param>
        /// <returns></returns>
        public DataSet GetJobProjectApprovalDCA(Chart objChart)
        {
            return _objDLReport.GetJobProjectApprovalDCA(objChart);
        }

        /// <summary>
        /// Accredited - Get Category Due Report
        /// Only for Accredited
        /// </summary>
        /// <param name="objPropMapData"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public DataSet GetCategoryDueReport(MapData objPropMapData, List<RetainFilter> filters)
        {
            return _objDLReport.GetCategoryDueReport(objPropMapData, filters);
        }

        public DataSet GetOpenMaintenanceByEquipment(MapData objPropMapData, string defaultCategory)
        {
            try
            {
                return _objDLReport.GetOpenMaintenanceByEquipment(objPropMapData, defaultCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<GetOpenMaintenanceByEquipmentViewModel> GetOpenMaintenanceByEquipment(GetOpenMaintenanceByEquipmentParam _GetOpenMaintenanceByEquipment, string ConnectionString, string defaultCategory)
        {
            try
            {
                DataSet ds = _objDLReport.GetOpenMaintenanceByEquipment(_GetOpenMaintenanceByEquipment, ConnectionString, defaultCategory);

                List<GetOpenMaintenanceByEquipmentViewModel> _lstGetOpenMaintenanceByEquipment = new List<GetOpenMaintenanceByEquipmentViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstGetOpenMaintenanceByEquipment.Add(
                        new GetOpenMaintenanceByEquipmentViewModel()
                        {
                            ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                            CDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CDate"]) ? null : dr["CDate"]),
                            EDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EDate"]) ? null : dr["EDate"]),
                            fWork = Convert.ToInt32(DBNull.Value.Equals(dr["fWork"]) ? 0 : dr["fWork"]),
                            Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                            Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                            Elev = Convert.ToInt32(DBNull.Value.Equals(dr["Elev"]) ? 0 : dr["Elev"]),
                            Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Cat = Convert.ToString(dr["Cat"]),
                            DWork = Convert.ToString(dr["DWork"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            LocAddress = Convert.ToString(dr["LocAddress"]),
                            EquipmentID = Convert.ToString(dr["EquipmentID"]),
                            EquipmentDesc = Convert.ToString(dr["EquipmentDesc"]),
                            EquipmentType = Convert.ToString(dr["EquipmentType"]),
                            EquipmentCat = Convert.ToString(dr["EquipmentCat"]),
                            EquipmentUnique = Convert.ToString(dr["EquipmentUnique"]),
                            TicketFreq = Convert.ToString(dr["TicketFreq"]),
                        }
                        );
                }

                return _lstGetOpenMaintenanceByEquipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListVendorDemand(Customer objPropCustomer)
        {
            return _objDLReport.GetListVendorDemand(objPropCustomer);
        }

        public DataSet GetProjectVendorDemand(Customer objPropCustomer, List<RetainFilter> filters, string vendor, string department, int includeClosedProject = 0, int includeClosedPO = 0)
        {
            return _objDLReport.GetProjectVendorDemand(objPropCustomer, filters, vendor, department, includeClosedProject, includeClosedPO);
        }

        public DataSet GetEquipmentContractByCustomer(Contracts objPropContracts, List<RetainFilter> filters, bool includeClose)
        {
            return _objDLReport.GetEquipmentContractByCustomer(objPropContracts, filters, includeClose);
        }

        public DataSet GetProjectSummary(Customer objPropCustomer, List<RetainFilter> filters, string departments, int includeClose = 0, bool isExport = false, bool isDBTotalService = false)
        {
            return _objDLReport.GetProjectSummary(objPropCustomer, filters, departments, includeClose, isExport, isDBTotalService);
        }

        public DataSet GetDashboardReportData(Chart objChart, List<DashboardColumnRequest> objColumns)
        {
            return _objDLReport.GetDashboardReportData(objChart, objColumns);
        }

        public DataSet GetServiceSalesCheckUpReport(Contracts objPropContracts, List<RetainFilter> filters, bool includeClose = false, bool isPassedInspection = false)
        {
            return _objDLReport.GetServiceSalesCheckUpReport(objPropContracts, filters, includeClose, isPassedInspection);
        }

        public DataSet GetTicketListPayrollHours(MapData objPropMapData, List<RetainFilter> filters)
        {
            return _objDLReport.GetTicketListPayrollHours(objPropMapData, filters);
        }
    }
}
