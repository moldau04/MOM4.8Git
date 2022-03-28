using System.Data;
using DataLayer;
using BusinessEntity;
using BusinessEntity.APModels;
using System.Collections.Generic;
using System;
using BusinessEntity.Payroll;
using BusinessEntity.InventoryModel;
using BusinessEntity.CustomersModel;
using BusinessEntity.Recurring;

namespace BusinessLayer
{
    public class BL_ReportsData
    {
        DL_ReportsData objDL_Reports = new DL_ReportsData();

        public DataSet getCustomerDetails(User objPropUser)
        {
            return objDL_Reports.GetCustomerDetails(objPropUser);
        }

        public DataSet getCustomerDetailsTest(User objPropUser)
        {
            return objDL_Reports.GetCustomerDetailsTest(objPropUser);
        }

        //api

        public List<CustomerFilterViewModel> getCustomerDetailsTest(getCustomerDetailsTestParam _getCustomerDetailsTest, string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetCustomerDetailsTest(_getCustomerDetailsTest, ConnectionString);

            List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerFilterViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Address = Convert.ToString(dr["address"]),
                        Email = Convert.ToString(dr["Email"]),
                        Country = Convert.ToString(dr["Country"]),
                        Website = Convert.ToString(dr["Website"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        Type = Convert.ToString(dr["Type"]),
                        Balance = Convert.ToString(dr["Balance"]),
                        Status = Convert.ToString(dr["Status"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        equip = Convert.ToInt32(DBNull.Value.Equals(dr["equip"]) ? 0 : dr["equip"]),
                        opencall = Convert.ToInt32(DBNull.Value.Equals(dr["opencall"]) ? 0 : dr["opencall"]),

                    });
            }
            return _customer;
        }

        public DataSet GetDeliveryDetails(User objPropUser)
        {
            return objDL_Reports.GetDeliveryDetails(objPropUser);
        }
        public DataSet GetDrawingsDetails(User objPropUser)
        {
            return objDL_Reports.GetDrawingsDetails(objPropUser);
        }

        public DataSet GetReDrawingsColumns(User objPropUser)
        {
            return objDL_Reports.GetReDrawingsColumns(objPropUser);
        }

        public DataSet GetApprovalDetails(User objPropUser)
        {
            return objDL_Reports.GetApprovalDetails(objPropUser);
        }
        public DataSet GetOpenJobDetails(User objPropUser)
        {
            return objDL_Reports.GetOpenJobDetails(objPropUser);
        }
        public DataSet GetInspectedDetails(User objPropUser)
        {
            return objDL_Reports.GetInspectedDetails(objPropUser);
        }
        public DataSet getRecurringDetailsTest(User objPropUser)
        {
            return objDL_Reports.getRecurringDetailsTest(objPropUser);
        }

        
        public DataSet GetUTaxLocReport(Invoices objInvoice)
        {
            return objDL_Reports.GetUTaxLocReport(objInvoice);
        }
        public List<GetUTaxLocReportViewModel> GetUTaxLocReport(GetUTaxLocReportParam _GetUTaxLocReportParam, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetUTaxLocReport( _GetUTaxLocReportParam, ConnectionString);
            List<GetUTaxLocReportViewModel> _lstGetUTaxLocReportViewModel = new List<GetUTaxLocReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetUTaxLocReportViewModel.Add(
                    new GetUTaxLocReportViewModel()
                    {
                        vendor = Convert.ToString(dr["vendor"]),
                        PJRef = Convert.ToString(dr["PJRef"]),
                        JobID = Convert.ToInt32(DBNull.Value.Equals(dr["JobID"]) ? 0 : dr["JobID"]),
                        PJItemAmount = Convert.ToDouble(DBNull.Value.Equals(dr["PJItemAmount"]) ? 0 : dr["PJItemAmount"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        JobType = Convert.ToString(dr["JobType"]),
                        PO = Convert.ToInt32(DBNull.Value.Equals(dr["PO"]) ? 0 : dr["PO"]),
                        Descp = Convert.ToString(dr["Descp"]),
                        PJfDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PJfDate"]) ? null : dr["PJfDate"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        State = Convert.ToString(dr["State"]),
                        TaxDesc = Convert.ToString(dr["TaxDesc"]),
                        STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        TransBatch = Convert.ToInt32(DBNull.Value.Equals(dr["TransBatch"]) ? 0 : dr["TransBatch"]),
                        TransfDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TransfDate"]) ? null : dr["TransfDate"]),
                        LineItemDesc = Convert.ToString(dr["LineItemDesc"]),
                        TransType = Convert.ToString(dr["TransType"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                    }
                    );

            }
            return _lstGetUTaxLocReportViewModel;
        }

        public DataSet GetAccountSummaryListingDetail(User objPropUser)
        {
            return objDL_Reports.GetAccSummaryDetail(objPropUser);
        }

        public List<UserViewModel> GetAccountSummaryListingDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetAccSummaryDetail(_GetAccountSummaryListingDetailParam, ConnectionString);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstUserViewModel.Add(
                    new UserViewModel()
                    {
                        Name = Convert.ToString(dr["Route"]),

                    });
            }
            return _lstUserViewModel;
        }

        public DataSet InsertCustomerReport(CustomerReport objCustReport)
        {
            return objDL_Reports.InsertCustomerReport(objCustReport);
        }

        public List<CustomerReportViewModel> InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString)
        {
            DataSet ds= objDL_Reports.InsertCustomerReport(_InsertCustomerReportParam, ConnectionString);
            List<CustomerReportViewModel> _customer = new List<CustomerReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerReportViewModel()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        ReportName = Convert.ToString(dr["ReportName"]),
                        ReportType = Convert.ToString(dr["ReportType"]),
                        UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
                        IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
                        IsAscending = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscendingOrder"]) ? 0 : dr["IsAscendingOrder"]),
                        SortBy = Convert.ToString(dr["SortBy"]),
                        IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? 0 : dr["IsStock"]),
                        Module = Convert.ToString(dr["Module"]),
                        Condition = Convert.ToString(dr["Condition"]),
                        ReportId = Convert.ToInt32(DBNull.Value.Equals(dr["ReportId"]) ? 0 : dr["ReportId"])

                    });
            }
            return _customer;

        }

        public void DeleteCustomerReport(CustomerReport objCustReport)
        {
            objDL_Reports.DeleteCustomerReport(objCustReport);
        }

        //api
        public void DeleteCustomerReport(DeleteCustomerReportParam objCustReport,string ConnectionString)
        {
            objDL_Reports.DeleteCustomerReport(objCustReport,ConnectionString);
        }

        public void UpdateCustomerReport(CustomerReport objCustReport)
        {
            objDL_Reports.UpdateCustomerReport(objCustReport);
        }

        //api
        public void UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString)
        {
            objDL_Reports.UpdateCustomerReport(_UpdateCustomerReportParam, ConnectionString);
        }

        public DataSet GetReports(User objPropUser)
        {
            return objDL_Reports.GetReports(objPropUser);
        }

        public DataSet GetDynamicReports(User objPropUser,string type)
        {
            return objDL_Reports.GetDynamicReports(objPropUser, type);
        }
        //api
        public List<CustomerReportViewModel> GetDynamicReports(GetDynamicReportsParam objPropUser, string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetDynamicReports(objPropUser, ConnectionString);

            List<CustomerReportViewModel> _customer = new List<CustomerReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerReportViewModel()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        ReportName = Convert.ToString(dr["ReportName"]),
                        ReportType = Convert.ToString(dr["ReportType"]),
                        UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
                        IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
                        IsAscending = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscendingOrder"]) ? 0 : dr["IsAscendingOrder"]),
                        SortBy = Convert.ToString(dr["SortBy"]),
                        IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? 0 : dr["IsStock"]),
                        Module = Convert.ToString(dr["Module"]),
                        Condition = Convert.ToString(dr["Condition"])
                    });
            }
            return _customer;

        }

        public DataSet GetStockReports(User objPropUser)
        {
             return objDL_Reports.GetStockReports(objPropUser);
        }

        public List<CustomerReportViewModel> GetStockReports(GetStockReportsParam _GetStockReportsParam, string connectionString)
        {
            DataSet ds = objDL_Reports.GetStockReports(_GetStockReportsParam, connectionString);

            List<CustomerReportViewModel> _lstCustomerReportViewModel = new List<CustomerReportViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCustomerReportViewModel.Add(new CustomerReportViewModel()
                {
                    Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                    ReportName = Convert.ToString(dr["ReportName"]),
                    ReportType = Convert.ToString(dr["ReportType"]),
                    UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
                    IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
                    IsAscendingOrder = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscendingOrder"]) ? 0 : dr["IsAscendingOrder"]),
                    SortBy = Convert.ToString(dr["SortBy"]),
                    IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? 0 : dr["IsStock"]),
                    Module = Convert.ToString(dr["Module"]),
                    Condition = Convert.ToString(dr["Condition"]),
                });
            }
            return _lstCustomerReportViewModel;
        }


        public DataSet GetMultipleStockReports(User objPropUser)
        {
            return objDL_Reports.GetMultipleStockReports(objPropUser);
        }

        //API
        public List<GetMultipleStockReportsViewModel> GetMultipleStockReports(GetMultipleStockReportsParam _GetMultipleStockReports, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetMultipleStockReports(_GetMultipleStockReports, ConnectionString);

            List<GetMultipleStockReportsViewModel> _lstGetMultipleStockReports = new List<GetMultipleStockReportsViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetMultipleStockReports.Add(
                    new GetMultipleStockReportsViewModel()
                {
                    Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                    ReportName = Convert.ToString(dr["ReportName"]),
                    ReportType = Convert.ToString(dr["ReportType"]),
                    UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
                    IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
                    IsAscendingOrder = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscendingOrder"]) ? 0 : dr["IsAscendingOrder"]),
                    SortBy = Convert.ToString(dr["SortBy"]),
                    IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? 0 : dr["IsStock"]),
                    Module = Convert.ToString(dr["Module"]),
                    Condition = Convert.ToString(dr["Condition"]),
                });
            }

            return _lstGetMultipleStockReports;
        }

        public DataSet GetReportColByRepId(CustomerReport objCustReport)
        {
            return objDL_Reports.GetReportColByRepId(objCustReport);
        }

        //api
        public List<CustomerFilterViewModel> GetReportColByRepId(GetReportColByRepIdParam objCustReport,string ConnectionString)
        {
           DataSet ds= objDL_Reports.GetReportColByRepId(objCustReport, ConnectionString);

            List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerFilterViewModel()
                    {
                        ColumnName = Convert.ToString(dr["ColumnName"]),

                    });
            }
            return _customer;
        }


        public DataSet GetReportFiltersByRepId(CustomerReport objCustReport)
        {
            return objDL_Reports.GetReportFiltersByRepId(objCustReport);
        }

        public List<CustomerFilterViewModel> GetReportFiltersByRepId(GetReportFiltersByRepIdParam objCustReport,string ConnectionString)
        {
           DataSet ds= objDL_Reports.GetReportFiltersByRepId(objCustReport, ConnectionString);

            List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerFilterViewModel()
                    {
                        FilterColumn = Convert.ToString(dr["ColumnName"]),
                        FilterSet = Convert.ToString(dr["FilterSet"]),
                    });
            }
            return _customer;
        }

        public DataSet GetOwners(string query, User objPropUser)
        {
            return objDL_Reports.GetOwners(query, objPropUser);
        }
        //api
        public List<CustomerFilterViewModel> GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
        {

            DataSet ds = objDL_Reports.GetOwners(_GetOwnersParam, ConnectionString);
            List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerFilterViewModel()
                    {
                        equip = Convert.ToInt32(dr["equip"].ToString())
                      
                    });
            }
            return _customer;
        }

        public DataSet GetReportDetailById(CustomerReport objCustReport)
        {
            return objDL_Reports.GetReportDetailById(objCustReport);
        }
        public List<CustomerReportViewModel> GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetReportDetailById(_GetReportDetailByIdParam, ConnectionString);
            List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCustomerReport.Add(
                    new CustomerReportViewModel()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        ReportName = Convert.ToString(dr["ReportName"]),
                        ReportType = Convert.ToString(dr["ReportType"]),
                        UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
                        IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
                        IsAscendingOrder = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscendingOrder"]) ? false : dr["IsAscendingOrder"]),
                        SortBy = Convert.ToString(dr["SortBy"]),
                        IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? false : dr["IsStock"]),
                        Module = Convert.ToString(dr["Module"]),
                        Condition = Convert.ToString(dr["Condition"])
                    });
            }
            return _lstCustomerReport;
        }


        //api
        public List<CustomerReportViewModel> GetReportDetailById(CustomerReportParam objCustReport,string ConnectionString)
        {
           DataSet ds= objDL_Reports.GetReportDetailById(objCustReport, ConnectionString);

            List<CustomerReportViewModel> _customer = new List<CustomerReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerReportViewModel()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        ReportName = Convert.ToString(dr["ReportName"]),
                        ReportType = Convert.ToString(dr["ReportType"]),
                        UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
                        IsGlobal = Convert.ToBoolean(DBNull.Value.Equals(dr["IsGlobal"]) ? 0 : dr["IsGlobal"]),
                        IsAscending = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAscendingOrder"]) ? 0 : dr["IsAscendingOrder"]),
                        SortBy = Convert.ToString(dr["SortBy"]),
                        IsStock = Convert.ToBoolean(DBNull.Value.Equals(dr["IsStock"]) ? 0 : dr["IsStock"]),
                        Module = Convert.ToString(dr["Module"]),
                        Condition = Convert.ToString(dr["Condition"])
                    });
            }
            return _customer;
        }


        public bool CheckExistingReport(CustomerReport objCustReport, string reportAction)
        {
            return objDL_Reports.CheckExistingReport(objCustReport, reportAction);
        }

        //api
        public bool CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString)
        {
            return objDL_Reports.CheckExistingReport(_CheckExistingReportParam, ConnectionString);
        }

        public bool IsStockReportExist(CustomerReport objCustReport, string reportAction)
        {
            return objDL_Reports.IsStockReportExist(objCustReport, reportAction);
        }
        //api
        public bool IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString)
        {
            return objDL_Reports.IsStockReportExist(_IsStockReportExistParam, ConnectionString);
        }

        public bool CheckForDelete(CustomerReport objCustReport)
        {
            return objDL_Reports.CheckForDelete(objCustReport);
        }


        public DataSet GetControlForReports(User objPropUser)
        {
            return objDL_Reports.GetControlForReports(objPropUser);
        }

        public List<CustomerFilterViewModel> GetControlForReports(getConnectionConfigParam objPropUser,string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetControlForReports(objPropUser,ConnectionString);

            List<CustomerFilterViewModel> _customer = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerFilterViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Email = Convert.ToString(dr["Email"]),
                        WebAddress = Convert.ToString(dr["WebAddress"]),
                        Logo = Convert.ToString(dr["Logo"]),
                        DBName = Convert.ToString(dr["dbname"]),

                    });
            }
            return _customer;
        }

        public DataSet GetCustomerType(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerType(objCustReport);
        }
        //Payroll Menu
        public DataSet GetControlForPayroll(User objPropUser)
        {
            return objDL_Reports.GetControlForPayroll(objPropUser);
        }

        //api
        public List<CustomerReportViewModel> GetCustomerType(GetCustomerTypeParam _GetCustomerType, string ConnectionString)
        {
           DataSet ds= objDL_Reports.GetCustomerType(_GetCustomerType, ConnectionString);

            List<CustomerReportViewModel> _customers = new List<CustomerReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customers.Add(
                    new CustomerReportViewModel()
                    {
                        Type = Convert.ToString(dr["Type"])

                    });
            }
            return _customers;
        }

        public DataSet GetGroupedCustomersLocation(User objPropUser)
        {
            return objDL_Reports.GetGroupedCustomersLocation(objPropUser);
        }

        public DataSet GetCustomerName(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerName(objCustReport);
        }
        //api

        public List<CustomerFilterViewModel> GetCustomerName(GetCustomerNameParam objCustReport,string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetCustomerName(objCustReport,ConnectionString);
            List<CustomerFilterViewModel> _customers = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customers.Add(
                    new CustomerFilterViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),

                    });
            }
            return _customers;

        }

        public DataSet GetCustomerAddress(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerAddress(objCustReport);
        }

        //api
        public List<CustomerFilterViewModel> GetCustomerAddress(GetCustomerAddressParam objCustReport,string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetCustomerAddress(objCustReport,ConnectionString);
            List<CustomerFilterViewModel> _customers = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customers.Add(
                    new CustomerFilterViewModel()
                    {
                        Address = Convert.ToString(dr["Address"]),

                    });
            }
            return _customers;
        }


        public DataSet GetCustomerCity(CustomerReport objCustReport)
        {
            return objDL_Reports.GetCustomerCity(objCustReport);
        }

        //api
        public List<CustomerFilterViewModel> GetCustomerCity(GetCustomerCityParam objCustReport,string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetCustomerCity(objCustReport,ConnectionString);
            List<CustomerFilterViewModel> _customers = new List<CustomerFilterViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customers.Add(
                    new CustomerFilterViewModel()
                    {
                        City = Convert.ToString(dr["City"]),

                    });
            }
            return _customers;
        }

        public DataSet GetHeaderFooterDetail(CustomerReport objCustReport)
        {
            return objDL_Reports.GetHeaderFooterDetail(objCustReport);
        }

        //api
        public List<HeaderFooterDetailViewModel> GetHeaderFooterDetail(GetHeaderFooterDetailParam objCustReport,string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetHeaderFooterDetail(objCustReport,ConnectionString);


            List<HeaderFooterDetailViewModel> _customer = new List<HeaderFooterDetailViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new HeaderFooterDetailViewModel()
                    {
                        Id = Convert.ToInt32(DBNull.Value.Equals(dr["Id"]) ? 0 : dr["Id"]),
                        ReportId = Convert.ToInt32(DBNull.Value.Equals(dr["ReportId"]) ? 0 : dr["ReportId"]),
                        MainHeader = Convert.ToBoolean(DBNull.Value.Equals(dr["MainHeader"]) ? false : dr["MainHeader"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        ReportTitle = Convert.ToString(dr["ReportTitle"]),
                        SubTitle = Convert.ToString(dr["SubTitle"]),
                        DatePrepared = Convert.ToString(dr["DatePrepared"]),
                        TimePrepared = Convert.ToBoolean(DBNull.Value.Equals(dr["TimePrepared"]) ? false : dr["TimePrepared"]),
                        ReportBasis = Convert.ToBoolean(DBNull.Value.Equals(dr["ReportBasis"]) ? 0 : dr["ReportBasis"]),
                        PageNumber = Convert.ToString(dr["PageNumber"]),
                        ExtraFooterLine = Convert.ToString(dr["ExtraFooterLine"]),
                        Alignment = Convert.ToString(dr["Alignment"]),
                        PDFSize = Convert.ToString(dr["PDFSize"]),

                    });
            }
            return _customer;
        }


        public DataSet GetColumnWidthByReportId(CustomerReport objCustReport)
        {
            return objDL_Reports.GetColumnWidthByReportId(objCustReport);
        }

        //api
        public List<CustomerReportViewModel> GetColumnWidthByReportId(GetColumnWidthByReportIdParam objCustReport,string ConnectionString)
        {
           DataSet ds= objDL_Reports.GetColumnWidthByReportId(objCustReport, ConnectionString);

            List<CustomerReportViewModel> _customer = new List<CustomerReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _customer.Add(
                    new CustomerReportViewModel()
                    {
                        ColumnWidth = Convert.ToString(dr["ColumnWidth"]),

                    });
            }
            return _customer;
        }

        public void UpdateCustomerReportResizedWidth(CustomerReport objCustReport)
        {
            objDL_Reports.UpdateCustomerReportResizedWidth(objCustReport);
        }

       // api
        public void UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam objCustReport,string ConnectionString)
        {
            objDL_Reports.UpdateCustomerReportResizedWidth(objCustReport, ConnectionString);
        }

        public DataSet GetCustReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetCustReportFiltersValue(objPropUser);
        }

        //api
        public ListGetCustReportFiltersValue GetCustReportFiltersValue(GetCustReportFiltersValueParam _GetCustReportFiltersValue, string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetCustReportFiltersValue(_GetCustReportFiltersValue, ConnectionString);

            ListGetCustReportFiltersValue _lstGetCustReportFiltersValue = new ListGetCustReportFiltersValue();
            List<GetCustReportFiltersValueTable> _lstTable = new List<GetCustReportFiltersValueTable>();
            List<GetCustReportFiltersValueTable1> _lstTable1 = new List<GetCustReportFiltersValueTable1>();
            List<GetCustReportFiltersValueTable2> _lstTable2 = new List<GetCustReportFiltersValueTable2>();
            List<GetCustReportFiltersValueTable3> _lstTable3 = new List<GetCustReportFiltersValueTable3>();
            List<GetCustReportFiltersValueTable4> _lstTable4 = new List<GetCustReportFiltersValueTable4>();
            List<GetCustReportFiltersValueTable5> _lstTable5 = new List<GetCustReportFiltersValueTable5>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable.Add(
                    new GetCustReportFiltersValueTable()
                    {
                        Name = Convert.ToString(dr["Name"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable1.Add(
                    new GetCustReportFiltersValueTable1()
                    {
                        Address = Convert.ToString(dr["Address"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable2.Add(
                    new GetCustReportFiltersValueTable2()
                    {
                        City = Convert.ToString(dr["City"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable3.Add(
                    new GetCustReportFiltersValueTable3()
                    {
                        State = Convert.ToString(dr["State"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable4.Add(
                    new GetCustReportFiltersValueTable4()
                    {
                        Type = Convert.ToString(dr["Type"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                _lstTable5.Add(
                    new GetCustReportFiltersValueTable5()
                    {
                        Status = Convert.ToString(dr["Status"]),

                    });
            }

            _lstGetCustReportFiltersValue.lstTable = _lstTable;
            _lstGetCustReportFiltersValue.lstTable1 = _lstTable1;
            _lstGetCustReportFiltersValue.lstTable2 = _lstTable2;
            _lstGetCustReportFiltersValue.lstTable3 = _lstTable3;
            _lstGetCustReportFiltersValue.lstTable4 = _lstTable4;
            _lstGetCustReportFiltersValue.lstTable5 = _lstTable5;

            return _lstGetCustReportFiltersValue;
        }

        public DataSet GetDeliveryReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetDeliveryReportFiltersValue(objPropUser);
        }
        public DataSet GetDrawingsReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetDrawingsReportFiltersValue(objPropUser);
        }
        public DataSet GetReDrawingsReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetReDrawingsReportFiltersValue(objPropUser);
        }
        public DataSet GetApprovalReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetApprovalReportFiltersValue(objPropUser);
        }
        public DataSet GetOpenJobReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetOpenJobReportFiltersValue(objPropUser);
        }
        public DataSet GetInspectionReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetInspectionReportFiltersValue(objPropUser);
        }
        public DataSet GetEquipReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetEquipReportFiltersValue(objPropUser);
        }

        //API
        public ListGetEquipReportFiltersValue GetEquipReportFiltersValue(GetEquipReportFiltersValueParam _GetEquipReportFiltersValue, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetEquipReportFiltersValue(_GetEquipReportFiltersValue, ConnectionString);

            ListGetEquipReportFiltersValue _ds = new ListGetEquipReportFiltersValue();
            List<GetEquipReportFiltersValueTable> _lstTable = new List<GetEquipReportFiltersValueTable>();
            List<GetEquipReportFiltersValueTable1> _lstTable1 = new List<GetEquipReportFiltersValueTable1>();
            List<GetEquipReportFiltersValueTable2> _lstTable2 = new List<GetEquipReportFiltersValueTable2>();
            List<GetEquipReportFiltersValueTable3> _lstTable3 = new List<GetEquipReportFiltersValueTable3>();
            List<GetEquipReportFiltersValueTable4> _lstTable4 = new List<GetEquipReportFiltersValueTable4>();
            List<GetEquipReportFiltersValueTable5> _lstTable5 = new List<GetEquipReportFiltersValueTable5>();
            List<GetEquipReportFiltersValueTable6> _lstTable6 = new List<GetEquipReportFiltersValueTable6>();
            List<GetEquipReportFiltersValueTable7> _lstTable7 = new List<GetEquipReportFiltersValueTable7>();
            List<GetEquipReportFiltersValueTable8> _lstTable8 = new List<GetEquipReportFiltersValueTable8>();
            List<GetEquipReportFiltersValueTable9> _lstTable9 = new List<GetEquipReportFiltersValueTable9>();
            List<GetEquipReportFiltersValueTable10> _lstTable10 = new List<GetEquipReportFiltersValueTable10>();
            List<GetEquipReportFiltersValueTable11> _lstTable11 = new List<GetEquipReportFiltersValueTable11>();
            List<GetEquipReportFiltersValueTable12> _lstTable12 = new List<GetEquipReportFiltersValueTable12>();
            List<GetEquipReportFiltersValueTable13> _lstTable13 = new List<GetEquipReportFiltersValueTable13>();
            List<GetEquipReportFiltersValueTable14> _lstTable14 = new List<GetEquipReportFiltersValueTable14>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable.Add(
                    new GetEquipReportFiltersValueTable()
                    {
                        Location = Convert.ToString(dr["Location"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable1.Add(
                    new GetEquipReportFiltersValueTable1()
                    {
                        OwnerID = Convert.ToInt32(DBNull.Value.Equals(dr["OwnerID"]) ? 0 : dr["OwnerID"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable2.Add(
                    new GetEquipReportFiltersValueTable2()
                    {
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable3.Add(
                    new GetEquipReportFiltersValueTable3()
                    {
                        equipment = Convert.ToString(dr["equipment"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable4.Add(
                    new GetEquipReportFiltersValueTable4()
                    {
                        Unique = Convert.ToString(dr["Unique#"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                _lstTable5.Add(
                    new GetEquipReportFiltersValueTable5()
                    {
                        five_year_Insp_Date = Convert.ToString(dr["five_year_Insp_Date"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[6].Rows)
            {
                _lstTable6.Add(
                    new GetEquipReportFiltersValueTable6()
                    {
                        annual_Insp_Date = Convert.ToString(dr["annual_Insp_Date"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[7].Rows)
            {
                _lstTable7.Add(
                    new GetEquipReportFiltersValueTable7()
                    {
                        customer = Convert.ToString(dr["customer"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[8].Rows)
            {
                _lstTable8.Add(
                    new GetEquipReportFiltersValueTable8()
                    {
                        Inspector_Name = Convert.ToString(dr["Inspector_Name"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[9].Rows)
            {
                _lstTable9.Add(
                    new GetEquipReportFiltersValueTable9()
                    {
                        Manuf = Convert.ToString(dr["Manuf"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[10].Rows)
            {
                _lstTable10.Add(
                    new GetEquipReportFiltersValueTable10()
                    {
                        EquipmentType = Convert.ToString(dr["EquipmentType"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[11].Rows)
            {
                _lstTable11.Add(
                    new GetEquipReportFiltersValueTable11()
                    {
                        ServiceType = Convert.ToString(dr["ServiceType"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[12].Rows)
            {
                _lstTable12.Add(
                    new GetEquipReportFiltersValueTable12()
                    {
                        InstalledOn = Convert.ToDateTime(DBNull.Value.Equals(dr["InstalledOn"]) ? null : dr["InstalledOn"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[13].Rows)
            {
                _lstTable13.Add(
                    new GetEquipReportFiltersValueTable13()
                    {
                        BuildingType = Convert.ToString(dr["BuildingType"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[14].Rows)
            {
                _lstTable14.Add(
                    new GetEquipReportFiltersValueTable14()
                    {
                        EquipmentState = Convert.ToString(dr["EquipmentState"]),
                    });
            }
            
            _ds.lstTable = _lstTable;
            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;
            _ds.lstTable3 = _lstTable3;
            _ds.lstTable4 = _lstTable4;
            _ds.lstTable5 = _lstTable5;
            _ds.lstTable6 = _lstTable6;
            _ds.lstTable7 = _lstTable7;
            _ds.lstTable8 = _lstTable8;
            _ds.lstTable9 = _lstTable9;
            _ds.lstTable10 = _lstTable10;
            _ds.lstTable11 = _lstTable11;
            _ds.lstTable12 = _lstTable12;
            _ds.lstTable13 = _lstTable13;
            _ds.lstTable14 = _lstTable14;

            return _ds;
        }
        public DataSet GetTicketReportFiltersValue(string SqlQuery,User objPropUser)
        {
            return objDL_Reports.GetTicketReportFiltersValue(SqlQuery, objPropUser);
        }
        public DataSet GetVendorReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetVendorReportFiltersValue(objPropUser);
        }
        public DataSet GetEstimateReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetEstimateReportFiltersValue(objPropUser);
        }
        public DataSet GetLeadReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetLeadReportFiltersValue(objPropUser);
        }
        public DataSet GetTaskReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetTaskReportFiltersValue(objPropUser);
        }
        public DataSet GetJournalReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetJournalReportFiltersValue(objPropUser);
        }
        public DataSet GetOpportunityReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetOpportunityReportFiltersValue(objPropUser);
        }
        public DataSet GetBillReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetBillReportFiltersValue(objPropUser);
        }
        public ListGetBillReportFiltersValue GetBillReportFiltersValue(GetBillReportFiltersValueParam _GetBillReportFiltersValueParam, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetBillReportFiltersValue(_GetBillReportFiltersValueParam, ConnectionString);

            ListGetBillReportFiltersValue _lstBillReportDetails = new ListGetBillReportFiltersValue();
            List<GetBillReportFiltersValueTable> _lstTable = new List<GetBillReportFiltersValueTable>();
            List<GetBillReportFiltersValueTable1> _lstTable1 = new List<GetBillReportFiltersValueTable1>();
            List<GetBillReportFiltersValueTable2> _lstTable2 = new List<GetBillReportFiltersValueTable2>();
            List<GetBillReportFiltersValueTable3> _lstTable3 = new List<GetBillReportFiltersValueTable3>();
            List<GetBillReportFiltersValueTable4> _lstTable4 = new List<GetBillReportFiltersValueTable4>();
            List<GetBillReportFiltersValueTable5> _lstTable5 = new List<GetBillReportFiltersValueTable5>();
            List<GetBillReportFiltersValueTable6> _lstTable6 = new List<GetBillReportFiltersValueTable6>();
            List<GetBillReportFiltersValueTable7> _lstTable7 = new List<GetBillReportFiltersValueTable7>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable.Add(
                    new GetBillReportFiltersValueTable()
                    {
                        InvoiceDate = Convert.ToDateTime(DBNull.Value.Equals(dr["Invoice Date"]) ? 0 : dr["Invoice Date"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable1.Add(
                    new GetBillReportFiltersValueTable1()
                    {
                        Ref = Convert.ToString(dr["Ref"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable2.Add(
                    new GetBillReportFiltersValueTable2()
                    {
                        Description = Convert.ToString(dr["Description"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable3.Add(
                    new GetBillReportFiltersValueTable3()
                    {
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable4.Add(
                    new GetBillReportFiltersValueTable4()
                    {
                        Status = Convert.ToString(dr["Status"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                _lstTable5.Add(
                    new GetBillReportFiltersValueTable5()
                    {
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["Use Tax"]) ? 0 : dr["Use Tax"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[6].Rows)
            {
                _lstTable6.Add(
                    new GetBillReportFiltersValueTable6()
                    {
                        VendorName = Convert.ToString(dr["Vendor Name"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[7].Rows)
            {
                _lstTable7.Add(
                    new GetBillReportFiltersValueTable7()
                    {
                        PostingDate = Convert.ToString(dr["Posting Date"]),
                    });
            }

            _lstBillReportDetails.lstTable = _lstTable;
            _lstBillReportDetails.lstTable1 = _lstTable1;
            _lstBillReportDetails.lstTable2 = _lstTable2;
            _lstBillReportDetails.lstTable3 = _lstTable3;
            _lstBillReportDetails.lstTable4 = _lstTable4;
            _lstBillReportDetails.lstTable5 = _lstTable5;
            _lstBillReportDetails.lstTable6 = _lstTable6;
            _lstBillReportDetails.lstTable7 = _lstTable7;


            return _lstBillReportDetails;
        }


        public DataSet GetRouteReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetRouteReportFiltersValue(objPropUser);
        }
        public DataSet GetInvoiceReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetInvoiceReportFiltersValue(objPropUser);
        }
        public DataSet GetProjectReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetProjectReportFiltersValue(objPropUser);
        }

        public DataSet GetRecReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetRecReportFiltersValue(objPropUser);
        }
        public DataSet GetEscalationReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetEscalationReportFiltersValue(objPropUser);
        }

        //API
        public ListGetEscalationReportFiltersValue GetEscalationReportFiltersValue(GetEscalationReportFiltersValueParam _GetEscalationReportFiltersValue, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetEscalationReportFiltersValue(_GetEscalationReportFiltersValue, ConnectionString);

            ListGetEscalationReportFiltersValue _ds = new ListGetEscalationReportFiltersValue();
            List<GetEscalationReportFiltersValueTable> _lstTable = new List<GetEscalationReportFiltersValueTable>();
            List<GetEscalationReportFiltersValueTable1> _lstTable1 = new List<GetEscalationReportFiltersValueTable1>();
            List<GetEscalationReportFiltersValueTable2> _lstTable2 = new List<GetEscalationReportFiltersValueTable2>();
            List<GetEscalationReportFiltersValueTable3> _lstTable3 = new List<GetEscalationReportFiltersValueTable3>();
            List<GetEscalationReportFiltersValueTable4> _lstTable4 = new List<GetEscalationReportFiltersValueTable4>();
            List<GetEscalationReportFiltersValueTable5> _lstTable5 = new List<GetEscalationReportFiltersValueTable5>();
            List<GetEscalationReportFiltersValueTable6> _lstTable6 = new List<GetEscalationReportFiltersValueTable6>();
            List<GetEscalationReportFiltersValueTable7> _lstTable7 = new List<GetEscalationReportFiltersValueTable7>();
            List<GetEscalationReportFiltersValueTable8> _lstTable8 = new List<GetEscalationReportFiltersValueTable8>();
            List<GetEscalationReportFiltersValueTable9> _lstTable9 = new List<GetEscalationReportFiltersValueTable9>();
            List<GetEscalationReportFiltersValueTable10> _lstTable10 = new List<GetEscalationReportFiltersValueTable10>();
            List<GetEscalationReportFiltersValueTable11> _lstTable11 = new List<GetEscalationReportFiltersValueTable11>();
            List<GetEscalationReportFiltersValueTable12> _lstTable12 = new List<GetEscalationReportFiltersValueTable12>();
            List<GetEscalationReportFiltersValueTable13> _lstTable13 = new List<GetEscalationReportFiltersValueTable13>();
            List<GetEscalationReportFiltersValueTable14> _lstTable14 = new List<GetEscalationReportFiltersValueTable14>();
            List<GetEscalationReportFiltersValueTable15> _lstTable15 = new List<GetEscalationReportFiltersValueTable15>();
            List<GetEscalationReportFiltersValueTable16> _lstTable16 = new List<GetEscalationReportFiltersValueTable16>();
            List<GetEscalationReportFiltersValueTable17> _lstTable17 = new List<GetEscalationReportFiltersValueTable17>();
            List<GetEscalationReportFiltersValueTable18> _lstTable18 = new List<GetEscalationReportFiltersValueTable18>();
            

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable.Add(
                    new GetEscalationReportFiltersValueTable()
                    {
                        LocationId = Convert.ToString(dr["Location Id"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable1.Add(
                    new GetEscalationReportFiltersValueTable1()
                    {
                        LocationName = Convert.ToString(dr["Location Name"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable2.Add(
                    new GetEscalationReportFiltersValueTable2()
                    {
                        ServiceType = Convert.ToString(dr["Service Type"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable3.Add(
                    new GetEscalationReportFiltersValueTable3()
                    {
                        Description = Convert.ToString(dr["Description"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable4.Add(
                    new GetEscalationReportFiltersValueTable4()
                    {
                        BillingFreqency = Convert.ToString(dr["Billing Freqency"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                _lstTable5.Add(
                    new GetEscalationReportFiltersValueTable5()
                    {
                        EscType = Convert.ToString(dr["Esc Type"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[6].Rows)
            {
                _lstTable6.Add(
                    new GetEscalationReportFiltersValueTable6()
                    {
                        Action = Convert.ToString(dr["Action"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[7].Rows)
            {
                _lstTable7.Add(
                    new GetEscalationReportFiltersValueTable7()
                    {
                        EscCycle = Convert.ToInt16(DBNull.Value.Equals(dr["Esc Cycle"]) ? 0 : dr["Esc Cycle"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[8].Rows)
            {
                _lstTable8.Add(
                    new GetEscalationReportFiltersValueTable8()
                    {
                        EscFactor = Convert.ToDouble(DBNull.Value.Equals(dr["Esc Factor"]) ? 0 : dr["Esc Factor"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[9].Rows)
            {
                _lstTable9.Add(
                    new GetEscalationReportFiltersValueTable9()
                    {
                        LastEsc = Convert.ToString(dr["Last Esc"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[10].Rows)
            {
                _lstTable10.Add(
                    new GetEscalationReportFiltersValueTable10()
                    {
                        StartEsc = Convert.ToString(dr["Start Esc"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[11].Rows)
            {
                _lstTable11.Add(
                    new GetEscalationReportFiltersValueTable11()
                    {
                        FinishEsc = Convert.ToString(dr["Finish Esc"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[12].Rows)
            {
                _lstTable12.Add(
                    new GetEscalationReportFiltersValueTable12()
                    {
                        NextDue = Convert.ToString(dr["Next Due"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[13].Rows)
            {
                _lstTable13.Add(
                    new GetEscalationReportFiltersValueTable13()
                    {
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[14].Rows)
            {
                _lstTable14.Add(
                    new GetEscalationReportFiltersValueTable14()
                    {
                        NewAmount = Convert.ToDouble(DBNull.Value.Equals(dr["New Amount"]) ? 0 : dr["New Amount"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[15].Rows)
            {
                _lstTable15.Add(
                    new GetEscalationReportFiltersValueTable15()
                    {
                        Length = Convert.ToInt16(DBNull.Value.Equals(dr["Length"]) ? 0 : dr["Length"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[16].Rows)
            {
                _lstTable16.Add(
                    new GetEscalationReportFiltersValueTable16()
                    {
                        Contract = Convert.ToInt32(DBNull.Value.Equals(dr["Contract"]) ? 0 : dr["Contract"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[17].Rows)
            {
                _lstTable17.Add(
                    new GetEscalationReportFiltersValueTable17()
                    {
                        ExpirationDate = Convert.ToString(dr["Expiration Date"]),
                    }
                    );
            }
            foreach (DataRow dr in ds.Tables[18].Rows)
            {
                _lstTable18.Add(
                    new GetEscalationReportFiltersValueTable18()
                    {
                        RenewalNotes = Convert.ToString(dr["Renewal Notes"]),
                    }
                    );
            }

            _ds.lstTable = _lstTable;
            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;
            _ds.lstTable3 = _lstTable3;
            _ds.lstTable4 = _lstTable4;
            _ds.lstTable5 = _lstTable5;
            _ds.lstTable6 = _lstTable6;
            _ds.lstTable7 = _lstTable7;
            _ds.lstTable8 = _lstTable8;
            _ds.lstTable9 = _lstTable9;
            _ds.lstTable10 = _lstTable10;
            _ds.lstTable11 = _lstTable11;
            _ds.lstTable12 = _lstTable12;
            _ds.lstTable13 = _lstTable13;
            _ds.lstTable14 = _lstTable14;
            _ds.lstTable15 = _lstTable15;
            _ds.lstTable16 = _lstTable16;
            _ds.lstTable17 = _lstTable17;
            _ds.lstTable18 = _lstTable18;

            return _ds;
        }
        public DataSet GetUseTax(PJ objPJ)
        {
            return objDL_Reports.GetUseTax(objPJ);
        }
        public List<GetUseTaxViewModel> GetUseTax(GetUseTaxForReportsParam _GetUseTaxForReportsParam, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetUseTax( _GetUseTaxForReportsParam,  ConnectionString);
            List<GetUseTaxViewModel> _lstGetUseTax = new List<GetUseTaxViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetUseTax.Add(
                    new GetUseTaxViewModel()
                    {
                        PJfDate = Convert.ToDateTime(DBNull.Value.Equals(dr["PJfDate"]) ? null : dr["PJfDate"]),
                        PJRef = Convert.ToString(dr["PJRef"]),
                        PJBatch = Convert.ToInt32(DBNull.Value.Equals(dr["PJBatch"]) ? 0 : dr["PJBatch"]),
                        PJItemAmount = Convert.ToDouble(DBNull.Value.Equals(dr["PJItemAmount"]) ? 0 : dr["PJItemAmount"]),
                        RolName = Convert.ToString(dr["RolName"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        TransBatch = Convert.ToInt32(DBNull.Value.Equals(dr["TransBatch"]) ? 0 : dr["TransBatch"]),
                        TransfDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TransfDate"]) ? null : dr["TransfDate"]),
                        TransfDesc = Convert.ToString(dr["TransfDesc"]),
                        TransType = Convert.ToString(dr["TransType"]),
                        TransAmount = Convert.ToDouble(DBNull.Value.Equals(dr["TransAmount"]) ? 0 : dr["TransAmount"]),
                    }
                    );

            }
            return _lstGetUseTax;
        }
        public DataSet GetRecurringDetails(User objPropUser)
        {
            return objDL_Reports.GetRecurringDetails(objPropUser);
        }

        //API
        public List<GetRecurringDetailsViewModel> GetRecurringDetails(GetRecurringDetailsParam _GetRecurringDetails, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetRecurringDetails(_GetRecurringDetails, ConnectionString);

            List<GetRecurringDetailsViewModel> _lstGetRecurringDetails = new List<GetRecurringDetailsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetRecurringDetails.Add(
                    new GetRecurringDetailsViewModel()
                    {
                        Customer = Convert.ToString(dr["Customer"]),
                        LocationId = Convert.ToString(dr["Location Id"]),
                        Location = Convert.ToString(dr["Location"]),
                        LocType = Convert.ToString(dr["Loc Type"]),
                        ServiceType = Convert.ToString(dr["Service Type"]),
                        Description = Convert.ToString(dr["Description"]),
                        PreferredWorker = Convert.ToString(dr["Preferred Worker"]),
                        TicketStart = Convert.ToString(dr["Ticket Start"]),
                        TicketTime = Convert.ToDateTime(DBNull.Value.Equals(dr["Ticket Time"]) ? null : dr["Ticket Time"]),
                        Hours = Convert.ToDouble(DBNull.Value.Equals(dr["Hours"]) ? 0 : dr["Hours"]),
                        TicketFreq = Convert.ToString(dr["Ticket Freq"]),
                        BillStart = Convert.ToString(dr["Bill Start"]),
                        BillAmount = Convert.ToDouble(DBNull.Value.Equals(dr["Bill Amount"]) ? 0 : dr["Bill Amount"]),
                        BillFreqency = Convert.ToString(dr["Bill Freqency"]),
                        Status = Convert.ToString(dr["Status"]),
                        Expiration = Convert.ToInt16(DBNull.Value.Equals(dr["Expiration"]) ? 0 : dr["Expiration"]),
                        ExpirationDate = Convert.ToString(dr["Expiration Date"]),
                        Equipment = Convert.ToString(dr["Equipment"]),
                        PhoneMonitoring = Convert.ToString(dr["Phone Monitoring"]),
                        ContractType = Convert.ToString(dr["Contract Type"]),
                        OccupancyDiscount = Convert.ToString(dr["Occupancy Discount"]),
                        Exclusions = Convert.ToString(dr["Exclusions"]),
                        TermofContract = Convert.ToString(dr["Term of Contract"]),
                        PriceAdjustmentCap = Convert.ToString(dr["Price Adjustment Cap"]),
                        FireServiceTestingIncluded = Convert.ToString(dr["Fire Service Testing Included"]),
                        SpecialRates = Convert.ToString(dr["Special Rates"]),
                        ContractExpiration = Convert.ToString(dr["Contract Expiration"]),
                        ProratedItems = Convert.ToString(dr["Prorated Items"]),
                        AnnualTestIncluded = Convert.ToString(dr["Annual Test Included"]),
                        FiveYearStateTestIncluded = Convert.ToString(dr["Five Year State Test Included"]),
                        FireServiceTestedIncluded = Convert.ToString(dr["Fire Service Tested Included"]),
                        CancellationNotificationDays = Convert.ToString(dr["Cancellation Notification Days"]),
                        PriceAdjustmentNotificationDays = Convert.ToString(dr["Price Adjustment Notification Days"]),
                        AfterHoursCallsIncluded = Convert.ToString(dr["After Hours Calls Included"]),
                        OGServiceCallsIncluded = Convert.ToString(dr["OG Service Calls Included"]),
                        ContractHours = Convert.ToString(dr["Contract Hours"]),
                        ContractFormat = Convert.ToString(dr["Contract Format"]),
                    }
                    );

            }
            return _lstGetRecurringDetails;
        }
        public DataSet GetEscalationDetails(User objPropUser)
        {
            return objDL_Reports.GetEscalationDetails(objPropUser);
        }


        //API
        public List<GetEscalationDetailsViewModel> GetEscalationDetails(GetEscalationDetailsParam _GetEscalationDetails, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetEscalationDetails(_GetEscalationDetails, ConnectionString);

            List<GetEscalationDetailsViewModel> _lstGetEscalationDetails = new List<GetEscalationDetailsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEscalationDetails.Add(
                    new GetEscalationDetailsViewModel()
                    {
                        LocationId = Convert.ToString(dr["Location Id"]),
                        LocationName = Convert.ToString(dr["Location Name"]),
                        ServiceType = Convert.ToString(dr["Service Type"]),
                        Description = Convert.ToString(dr["Description"]),
                        BillingFreqency = Convert.ToString(dr["Billing Freqency"]),
                        EscType = Convert.ToString(dr["Esc Type"]),
                        Action = Convert.ToString(dr["Action"]),
                        EscCycle = Convert.ToInt16(DBNull.Value.Equals(dr["Esc Cycle"]) ? 0 : dr["Esc Cycle"]),
                        EscFactor = Convert.ToDouble(DBNull.Value.Equals(dr["Esc Factor"]) ? 0 : dr["Esc Factor"]),
                        LastEsc = Convert.ToString(dr["Last Esc"]),
                        StartEsc = Convert.ToString(dr["Start Esc"]),
                        FinishEsc = Convert.ToString(dr["Finish Esc"]),
                        NextDue = Convert.ToString(dr["Next Due"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        NewAmount = Convert.ToDouble(DBNull.Value.Equals(dr["New Amount"]) ? 0 : dr["New Amount"]),
                        Length = Convert.ToInt16(DBNull.Value.Equals(dr["Length"]) ? 0 : dr["Length"]),
                        Contract = Convert.ToInt32(DBNull.Value.Equals(dr["Contract"]) ? 0 : dr["Contract"]),
                        ExpirationDate = Convert.ToString(dr["Expiration Date"]),
                        RenewalNotes = Convert.ToString(dr["Renewal Notes"]),
                    }
                    );

            }
            return _lstGetEscalationDetails;
        }
        public DataSet GetVendorDetails(User objPropUser)
        {
            return objDL_Reports.GetVendorDetails(objPropUser);
        }
        public DataSet GetOpportunityDetails(User objPropUser)
        {
            return objDL_Reports.GetOpportunityDetails(objPropUser);
        }
        public DataSet GetEstimateDetails(User objPropUser)
        {
            return objDL_Reports.GetEstimateDetails(objPropUser);
        }
        public DataSet GetLeadDetails(User objPropUser)
        {
            return objDL_Reports.GetLeadDetails(objPropUser);
        }
        public DataSet GetTaskDetails(User objPropUser)
        {
            return objDL_Reports.GetTaskDetails(objPropUser);
        }
        public DataSet GetJournalDetails(User objPropUser)
        {
            return objDL_Reports.GetJournalDetails(objPropUser);
        }
        public DataSet GetBillDetails(User objPropUser)
        {
            return objDL_Reports.GetBillDetails(objPropUser);
        }

        public List<BillReportDetails> GetBillDetails(GetBillDetailsParam _GetBillDetailsParam, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetBillDetails(_GetBillDetailsParam, ConnectionString);

            List<BillReportDetails> _lstBillReportDetails = new List<BillReportDetails>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstBillReportDetails.Add(
                    new BillReportDetails()
                    {
                        PostingDate = Convert.ToString(dr["Posting Date"]),
                        InvoiceDate = Convert.ToString(dr["Invoice Date"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        Description = Convert.ToString(dr["Description"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Status = Convert.ToString(dr["Status"]),
                        UseTax = Convert.ToDouble(DBNull.Value.Equals(dr["Use Tax"]) ? 0 : dr["Use Tax"]),
                        VendorName = Convert.ToString(dr["Vendor Name"]),
                    });
            }
            return _lstBillReportDetails;
        }
        public DataSet GetRouteDetails(User objPropUser)
        {
            return objDL_Reports.GetRouteDetails(objPropUser);
        }
        public DataSet GetProjectDetails(User objPropUser)
        {
            return objDL_Reports.GetProjectDetails(objPropUser);
        }
        public DataSet GetInvoiceDetails(User objPropUser)
        {
            return objDL_Reports.GetInvoiceDetails(objPropUser);
        }      
        public DataSet GetTicketList(PJ objPJ)
        {
            return objDL_Reports.GetTicketList(objPJ);
        }

        //api
        public List<TicketViewModel> GetTicketList(PJ objPJ,string ConnectionString)
        {
            DataSet ds= objDL_Reports.GetTicketList(objPJ,ConnectionString);

            List<TicketViewModel> _ticket = new List<TicketViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _ticket.Add(
                    new TicketViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        TWokrOrder = Convert.ToString(dr["TWokrOrder"]),
                        TCDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TCDate"]) ? null : dr["TCDate"]),
                        TDDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TDDate"]) ? null : dr["TDDate"]),
                        TEDate = Convert.ToDateTime(DBNull.Value.Equals(dr["TEDate"]) ? null : dr["TEDate"]),
                        JType = Convert.ToInt32(DBNull.Value.Equals(dr["JType"]) ? 0 : dr["JType"]),
                        WfDesc = Convert.ToString(dr["WfDesc"]),
                        TfDesc = Convert.ToString(dr["TfDesc"]),
                        TTotal = Convert.ToInt32(DBNull.Value.Equals(dr["TTotal"]) ? 0 : dr["TTotal"]),
                        WIReg = Convert.ToInt32(DBNull.Value.Equals(dr["WIReg"]) ? 0 : dr["WIReg"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        LocName = Convert.ToString(dr["LocName"])
                    });
            }
            return _ticket;
        }

        //Rahil
        public DataSet getTicketDetails(User objPropUser,string query)
        {
            return objDL_Reports.getTicketDetails(objPropUser,query);
        }
        public DataSet getEquipmentInspection(User objPropUser)
        {
            return objDL_Reports.getEquipmentInspection(objPropUser);
        }

        //API
        public ListGetEquipmentInspection getEquipmentInspection(GetEquipmentInspectionParam _GetEquipmentInspection, string ConnectionString)
        {
            DataSet ds = objDL_Reports.getEquipmentInspection(_GetEquipmentInspection, ConnectionString);

            ListGetEquipmentInspection _ds = new ListGetEquipmentInspection();
            List<GetEquipmentInspectionTable1>  _lstTable1  = new List<GetEquipmentInspectionTable1>();
            List<GetEquipmentInspectionTable2> _lstTable2 = new List<GetEquipmentInspectionTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetEquipmentInspectionTable1()
                    {
                        EquipmentName = Convert.ToString(dr["Equipment Name"]),
                        State = Convert.ToString(dr["State"]),
                        ServiceType = Convert.ToString(dr["Service Type"]),
                        Category = Convert.ToString(dr["Category"]),
                        Manuf = Convert.ToString(dr["Manuf"]),
                        Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                        LastService = Convert.ToDateTime(DBNull.Value.Equals(dr["Last Service"]) ? null : dr["Last Service"]),
                        Installed = Convert.ToDateTime(DBNull.Value.Equals(dr["Installed"]) ? null : dr["Installed"]),
                        BuildingType = Convert.ToString(dr["Building Type"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Type = Convert.ToString(dr["Type"]),
                        Description = Convert.ToString(dr["Description"]),
                        Status = Convert.ToString(dr["Status"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        LocationID = Convert.ToString(dr["Location ID"]),
                        Location = Convert.ToString(dr["Location"]),
                        Address = Convert.ToString(dr["Address"]),
                        Shutdown = Convert.ToString(dr["Shutdown"]),
                        Classification = Convert.ToString(dr["Classification"]),
                        ShutdownReason = Convert.ToString(dr["ShutdownReason"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetEquipmentInspectionTable2()
                    {
                        Location = Convert.ToString(dr["Location"]),
                        OwnerID = Convert.ToInt32(DBNull.Value.Equals(dr["OwnerID"]) ? 0 : dr["OwnerID"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        equipment = Convert.ToString(dr["equipment"]),
                        Unique = Convert.ToString(dr["Unique#"]),
                        AnnualInspDate = Convert.ToString(dr["Annual Insp Date"]),
                        InspectorName = Convert.ToString(dr["Inspector Name"]),
                        Balastrades = Convert.ToString(dr["Balastrades"]),
                        BuildingELBI = Convert.ToString(dr["Building ELBI"]),
                        BuildingPower = Convert.ToString(dr["Building Power"]),
                        Capacity = Convert.ToString(dr["Capacity"]),
                        CarStation = Convert.ToString(dr["Car Station"]),
                        CarStationBulb = Convert.ToString(dr["Car Station Bulb"]),
                        CarStationMfg = Convert.ToString(dr["Car Station Mfg"]),
                        CarWeight = Convert.ToString(dr["Car Weight"]),
                        CodeYear = Convert.ToString(dr["Code Year"]),
                        CoilPartNumber = Convert.ToString(dr["Coil Part Number"]),
                        CounterweightRollerPartNumber = Convert.ToString(dr["Counterweight Roller Part Number"]),
                        DoorOpeningWidth = Convert.ToString(dr["Door Opening Width"]),
                        DoorOperator = Convert.ToString(dr["Door Operator"]),
                        DoorOperatorBelt = Convert.ToString(dr["Door Operator Belt"]),
                        DoorRollerPartNumber = Convert.ToString(dr["Door Roller Part Number"]),
                        DoorType = Convert.ToString(dr["Door Type"]),
                        DueDate = Convert.ToString(dr["Due Date"]),
                        EmergencyBatteryCharger = Convert.ToString(dr["Emergency Battery Charger"]),
                        Emergencylightbattery = Convert.ToString(dr["Emergency light battery"]),
                        EmergencyLightBulb = Convert.ToString(dr["Emergency Light Bulb"]),
                        EscalatorCombTeeth = Convert.ToString(dr["Escalator Comb Teeth"]),
                        EscalatorHandrail = Convert.ToString(dr["Escalator Handrail"]),
                        EscalatorHandrailColor = Convert.ToString(dr["Escalator Handrail Color"]),
                        EscalatorHandrailLength = Convert.ToString(dr["Escalator Handrail Length"]),
                        EscalatorHandrailReplacementDate = Convert.ToString(dr["Escalator Handrail Replacement Date"]),
                        EscalatorRollerPartNumber = Convert.ToString(dr["Escalator Roller Part Number"]),
                        EscalatorSkirtSwitch = Convert.ToString(dr["Escalator Skirt Switch"]),
                        EscalatorSpeed = Convert.ToString(dr["Escalator Speed"]),
                        EscalatorStepWidth = Convert.ToString(dr["Escalator Step Width"]),
                        FireServiceKey = Convert.ToString(dr["Fire Service Key"]),
                        FireServicePhaseII = Convert.ToString(dr["Fire Service Phase II"]),
                        Floors = Convert.ToString(dr["Floors"]),
                        FPM = Convert.ToString(dr["FPM"]),
                        FiveYearInspDate = Convert.ToString(dr["Five Year Insp Date"]),
                        GateSwitchPartNumber = Convert.ToString(dr["Gate Switch Part Number"]),
                        GeneratorBrushPartNumber = Convert.ToString(dr["Generator Brush Part Number"]),
                        GeneratorMfg = Convert.ToString(dr["Generator Mfg"]),
                        GibPartNumber = Convert.ToString(dr["Gib Part Number"]),
                        GovernorMfg = Convert.ToString(dr["Governor Mfg"]),
                        GovernorRopeLength = Convert.ToString(dr["Governor Rope Length"]),
                        GovernorRopeSize = Convert.ToString(dr["Governor Rope Size"]),
                        GuideRollerPartNumber = Convert.ToString(dr["Guide Roller Part Number"]),
                        HallLanternLensCap = Convert.ToString(dr["Hall Lantern Lens Cap"]),
                        HallLanternMfg = Convert.ToString(dr["Hall Lantern Mfg"]),
                        HallStationbulb = Convert.ToString(dr["Hall Station bulb"]),
                        HallStationManufacturer = Convert.ToString(dr["Hall Station Manufacturer"]),
                        HoistwayAccessSwitch = Convert.ToString(dr["Hoistway Access Switch"]),
                        InputBoard = Convert.ToString(dr["Input Board"]),
                        InterlockPartNumber = Convert.ToString(dr["Interlock Part Number"]),
                        MfgJobNumber = Convert.ToString(dr["Mfg Job Number"]),
                        ModelType = Convert.ToString(dr["Model Type"]),
                        MotorRPM = Convert.ToString(dr["Motor RPM"]),
                        MotorType = Convert.ToString(dr["Motor Type"]),
                        OilLineSize = Convert.ToString(dr["Oil Line Size"]),
                        Openings = Convert.ToString(dr["Openings"]),
                        OutputBoard = Convert.ToString(dr["Output Board"]),
                        PackingNumber = Convert.ToString(dr["Packing Number"]),
                        PistonDiameter = Convert.ToString(dr["Piston Diameter"]),
                        PositionIndicatorBulb = Convert.ToString(dr["Position Indicator Bulb"]),
                        PumpMotorMfg = Convert.ToString(dr["Pump Motor Mfg"]),
                        PumpUnitMfg = Convert.ToString(dr["Pump Unit Mfg"]),
                        PumpMotorBelt = Convert.ToString(dr["Pump Motor Belt"]),
                        purchaseDate = Convert.ToString(dr["purchase Date"]),
                        AlteredDate = Convert.ToString(dr["Altered Date"]),
                        RopeLength = Convert.ToString(dr["Rope Length"]),
                        RopeSize = Convert.ToString(dr["Rope Size"]),
                        RefNo = Convert.ToString(dr["Ref No"]),
                        SafetyEdges = Convert.ToString(dr["Safety Edges"]),
                        SerialNo = Convert.ToString(dr["Serial No."]),
                        ServiceInterval = Convert.ToString(dr["Service Interval"]),
                        ServiceIntervalUnit = Convert.ToString(dr["Service Interval Unit"]),
                        SpiratorPartNumber = Convert.ToString(dr["Spirator Part Number"]),
                        StartType = Convert.ToString(dr["Start Type"]),
                        StarterContactMfgandPartNo = Convert.ToString(dr["Starter Contact Mfg and Part No"]),
                        TimeClock = Convert.ToString(dr["Time Clock"]),
                        TimeClockModel = Convert.ToString(dr["Time Clock Model"]),
                        TXE = Convert.ToString(dr["TXE"]),
                        ValveMfg = Convert.ToString(dr["Valve Mfg"]),
                        WarrantyExpirationDate = Convert.ToString(dr["Warranty Expiration Date"]),
                        WherePurchased = Convert.ToString(dr["Where Purchased"]),
                        AnnualInspectionViolations = Convert.ToString(dr["Annual Inspection Violations"]),
                        AnnualInspectorCustomerPreference = Convert.ToString(dr["Annual Inspector-Customer Preference"]),
                    });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public DataSet GetLocationReportFiltersValue(User objPropUser)
        {
            return objDL_Reports.GetLocationReportFiltersValue(objPropUser);
        }

        //API
        public ListGetLocationReportFiltersValue GetLocationReportFiltersValue(GetLocationReportFiltersValueParam _GetLocationReportFiltersValue, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetLocationReportFiltersValue(_GetLocationReportFiltersValue, ConnectionString);

            ListGetLocationReportFiltersValue _ds = new ListGetLocationReportFiltersValue();
            List<GetLocationReportFiltersValueTable1> _lstTable1 = new List<GetLocationReportFiltersValueTable1>();
            List<GetLocationReportFiltersValueTable2> _lstTable2 = new List<GetLocationReportFiltersValueTable2>();
            List<GetLocationReportFiltersValueTable3> _lstTable3 = new List<GetLocationReportFiltersValueTable3>();
            List<GetLocationReportFiltersValueTable4> _lstTable4 = new List<GetLocationReportFiltersValueTable4>();
            List<GetLocationReportFiltersValueTable5> _lstTable5 = new List<GetLocationReportFiltersValueTable5>();
            List<GetLocationReportFiltersValueTable6> _lstTable6 = new List<GetLocationReportFiltersValueTable6>();
            List<GetLocationReportFiltersValueTable7> _lstTable7 = new List<GetLocationReportFiltersValueTable7>();
            List<GetLocationReportFiltersValueTable8> _lstTable8 = new List<GetLocationReportFiltersValueTable8>();
            List<GetLocationReportFiltersValueTable9> _lstTable9 = new List<GetLocationReportFiltersValueTable9>();
            List<GetLocationReportFiltersValueTable10> _lstTable10 = new List<GetLocationReportFiltersValueTable10>();
            List<GetLocationReportFiltersValueTable11> _lstTable11 = new List<GetLocationReportFiltersValueTable11>();
            List<GetLocationReportFiltersValueTable12> _lstTable12 = new List<GetLocationReportFiltersValueTable12>();
            List<GetLocationReportFiltersValueTable13> _lstTable13 = new List<GetLocationReportFiltersValueTable13>();
            List<GetLocationReportFiltersValueTable14> _lstTable14 = new List<GetLocationReportFiltersValueTable14>();
            List<GetLocationReportFiltersValueTable15> _lstTable15 = new List<GetLocationReportFiltersValueTable15>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetLocationReportFiltersValueTable1()
                    {
                        Customer = Convert.ToString(dr["Customer"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetLocationReportFiltersValueTable2()
                    {
                        Location = Convert.ToString(dr["Location"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable3.Add(
                    new GetLocationReportFiltersValueTable3()
                    {
                        City = Convert.ToString(dr["City"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable4.Add(
                    new GetLocationReportFiltersValueTable4()
                    {
                        State = Convert.ToString(dr["State"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable5.Add(
                    new GetLocationReportFiltersValueTable5()
                    {
                        Zip = Convert.ToString(dr["Zip"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                _lstTable6.Add(
                    new GetLocationReportFiltersValueTable6()
                    {
                        Address = Convert.ToString(dr["Address"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[6].Rows)
            {
                _lstTable7.Add(
                    new GetLocationReportFiltersValueTable7()
                    {
                        LocationSTax = Convert.ToString(dr["LocationSTax"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[7].Rows)
            {
                _lstTable8.Add(
                    new GetLocationReportFiltersValueTable8()
                    {
                        Type = Convert.ToString(dr["Type"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[8].Rows)
            {
                _lstTable9.Add(
                    new GetLocationReportFiltersValueTable9()
                    {
                        TaxDesc = Convert.ToString(dr["TaxDesc"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[9].Rows)
            {
                _lstTable10.Add(
                    new GetLocationReportFiltersValueTable10()
                    {
                        TaxName = Convert.ToString(dr["TaxName"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[10].Rows)
            {
                _lstTable11.Add(
                    new GetLocationReportFiltersValueTable11()
                    {
                        TaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["TaxRate"]) ? 0 : dr["TaxRate"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[11].Rows)
            {
                _lstTable12.Add(
                    new GetLocationReportFiltersValueTable12()
                    {
                        SalesPerson = Convert.ToString(dr["SalesPerson"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[12].Rows)
            {
                _lstTable13.Add(
                    new GetLocationReportFiltersValueTable13()
                    {
                        DefaultWorker = Convert.ToString(dr["DefaultWorker"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[13].Rows)
            {
                _lstTable14.Add(
                    new GetLocationReportFiltersValueTable14()
                    {
                        Acct = Convert.ToString(dr["Acct#"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[14].Rows)
            {
                _lstTable15.Add(
                    new GetLocationReportFiltersValueTable15()
                    {
                        PreferredWorker = Convert.ToString(dr["PreferredWorker"]),
                    });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;
            _ds.lstTable3 = _lstTable3;
            _ds.lstTable4 = _lstTable4;
            _ds.lstTable5 = _lstTable5;
            _ds.lstTable6 = _lstTable6;
            _ds.lstTable7 = _lstTable7;
            _ds.lstTable8 = _lstTable8;
            _ds.lstTable9 = _lstTable9;
            _ds.lstTable10 = _lstTable10;
            _ds.lstTable11 = _lstTable11;
            _ds.lstTable12 = _lstTable12;
            _ds.lstTable13 = _lstTable13;
            _ds.lstTable14 = _lstTable14;
            _ds.lstTable15 = _lstTable15;

            return _ds;
        }
        public DataSet GetLocationDetails(User objPropUser)
        {
             return objDL_Reports.GetLocationDetails(objPropUser);
        }

        //API
        public ListGetLocationDetails GetLocationDetails(GetLocationDetailsParam _GetLocationDetails, string ConnectionString)
        {
            DataSet ds = objDL_Reports.GetLocationDetails(_GetLocationDetails, ConnectionString);

            ListGetLocationDetails _ds = new ListGetLocationDetails();
            List<GetLocationDetailsTable1> _lstTable1 = new List<GetLocationDetailsTable1>();
            List<GetLocationDetailsTable2> _lstTable2 = new List<GetLocationDetailsTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetLocationDetailsTable1()
                    {
                        Acct = Convert.ToString(dr["Acct#"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        Location = Convert.ToString(dr["Location"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Type = Convert.ToString(dr["Type"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        InvoiceToEmail = Convert.ToString(dr["InvoiceToEmail"]),
                        InvoiceCCEmail = Convert.ToString(dr["InvoiceCCEmail"]),
                        ServiceToEmail = Convert.ToString(dr["ServiceToEmail"]),
                        ServiceCCEmail = Convert.ToString(dr["ServiceCCEmail"]),
                        PrintInvoice = Convert.ToString(dr["PrintInvoice"]),
                        EmailInvoice = Convert.ToString(dr["EmailInvoice"]),
                        NoCustomerStatement = Convert.ToString(dr["NoCustomerStatement"]),
                        Status = Convert.ToString(dr["Status"]),
                        BillingRate = Convert.ToDouble(DBNull.Value.Equals(dr["BillingRate"]) ? 0 : dr["BillingRate"]),
                        LocationSTax = Convert.ToString(dr["LocationSTax"]),
                        EquipmentCounts = Convert.ToInt16(DBNull.Value.Equals(dr["EquipmentCounts"]) ? 0 : dr["EquipmentCounts"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Terms = Convert.ToString(dr["Terms"]),
                        SalesPerson = Convert.ToString(dr["SalesPerson"]),
                        DefaultWorker = Convert.ToString(dr["DefaultWorker"]),
                        PreferredWorker = Convert.ToString(dr["PreferredWorker"])
                    });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetLocationDetailsTable2()
                    {
                        TaxDesc = Convert.ToString(dr["TaxDesc"]),
                        TaxName = Convert.ToString(dr["TaxName"]),
                        TaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["TaxRate"]) ? 0 : dr["TaxRate"]),
                    });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public DataSet GetLocationReport(User objPropUser)
        {
            return objDL_Reports.GetLocationReport(objPropUser);
        }
    }
}
