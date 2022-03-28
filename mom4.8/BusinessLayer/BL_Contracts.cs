using DataLayer;
using BusinessEntity;
using System.Data;
using System.Collections.Generic;
using BusinessEntity.CustomersModel;
using System;
using BusinessEntity.Recurring;

namespace BusinessLayer
{
    public class BL_Contracts
    {
        DL_Contracts objDL_Contracts = new DL_Contracts();

        public DataSet getContractsData(Contracts objPropContracts)
        {
            return objDL_Contracts.getContractsData(objPropContracts);
        }

        //API
        public List<GetContractsDataViewModel> getContractsData(GetContractsDataParam _GetContractsData, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.getContractsData(_GetContractsData, ConnectionString);

            List<GetContractsDataViewModel> _lstGetContractsData = new List<GetContractsDataViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetContractsData.Add(
                    new GetContractsDataViewModel()
                    {
                        SREMARKS = Convert.ToString(dr["SREMARKS"]),
                        expirationdate = Convert.ToDateTime(DBNull.Value.Equals(dr["expirationdate"]) ? null : dr["expirationdate"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        ctype = Convert.ToString(dr["ctype"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        BAmt = Convert.ToDouble(DBNull.Value.Equals(dr["BAmt"]) ? 0 : dr["BAmt"]),
                        Hours = Convert.ToDouble(DBNull.Value.Equals(dr["Hours"]) ? 0 : dr["Hours"]),
                        locid = Convert.ToString(dr["locid"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        credit = Convert.ToInt16(DBNull.Value.Equals(dr["credit"]) ? 0 : dr["credit"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        State = Convert.ToString(dr["State"]),
                        name = Convert.ToString(dr["name"]),
                        MonthlyBill = Convert.ToDouble(DBNull.Value.Equals(dr["MonthlyBill"]) ? 0 : dr["MonthlyBill"]),
                        MonthlyHours = Convert.ToDouble(DBNull.Value.Equals(dr["MonthlyHours"]) ? 0 : dr["MonthlyHours"]),
                        Freqency = Convert.ToString(dr["Freqency"]),
                        TicketFreq = Convert.ToString(dr["TicketFreq"]),
                        Status = Convert.ToString(dr["Status"]),
                        Worker = Convert.ToString(dr["Worker"]),
                    }
                    );
            }

            return _lstGetContractsData;
        }

        public DataSet GetContract(Contracts objPropContracts)
        {
            return objDL_Contracts.GetContract(objPropContracts);
        }

        //API
        public List<GetContractViewModel> GetContract(GetContractParam _GetContract, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetContract(_GetContract, ConnectionString);

            List<GetContractViewModel> _lstGetContract = new List<GetContractViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetContract.Add(
                    new GetContractViewModel()
                    {
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        Custom20 = Convert.ToString(dr["Custom20"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        BStart = Convert.ToDateTime(DBNull.Value.Equals(dr["BStart"]) ? null : dr["BStart"]),
                        BCycle = Convert.ToInt16(DBNull.Value.Equals(dr["BCycle"]) ? 0 : dr["BCycle"]),
                        BAmt = Convert.ToDouble(DBNull.Value.Equals(dr["BAmt"]) ? 0 : dr["BAmt"]),
                        SStart = Convert.ToDateTime(DBNull.Value.Equals(dr["SStart"]) ? null : dr["SStart"]),
                        sCycle = Convert.ToInt16(DBNull.Value.Equals(dr["sCycle"]) ? 0 : dr["sCycle"]),
                        SDate = Convert.ToInt16(DBNull.Value.Equals(dr["SDate"]) ? 0 : dr["SDate"]),
                        SDay = Convert.ToInt16(DBNull.Value.Equals(dr["SDay"]) ? 0 : dr["SDay"]),
                        STime = Convert.ToDateTime(DBNull.Value.Equals(dr["STime"]) ? null : dr["STime"]),
                        CreditCard = Convert.ToInt16(DBNull.Value.Equals(dr["CreditCard"]) ? 0 : dr["CreditCard"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        locname = Convert.ToString(dr["locname"]),
                        credit = Convert.ToInt16(DBNull.Value.Equals(dr["credit"]) ? 0 : dr["credit"]),
                        swe = Convert.ToInt16(DBNull.Value.Equals(dr["swe"]) ? 0 : dr["swe"]),
                        hours = Convert.ToDouble(DBNull.Value.Equals(dr["hours"]) ? 0 : dr["hours"]),
                        ctype = Convert.ToString(dr["ctype"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        ExpirationDate = Convert.ToDateTime(DBNull.Value.Equals(dr["ExpirationDate"]) ? null : dr["ExpirationDate"]),
                        Expiration = Convert.ToInt16(DBNull.Value.Equals(dr["Expiration"]) ? 0 : dr["Expiration"]),
                        frequencies = Convert.ToInt16(DBNull.Value.Equals(dr["frequencies"]) ? 0 : dr["frequencies"]),
                        Billing = Convert.ToInt16(DBNull.Value.Equals(dr["Billing"]) ? 0 : dr["Billing"]),
                        CustBilling = Convert.ToInt16(DBNull.Value.Equals(dr["CustBilling"]) ? 0 : dr["CustBilling"]),
                        Central = Convert.ToInt32(DBNull.Value.Equals(dr["Central"]) ? 0 : dr["Central"]),
                        Chart = Convert.ToInt32(DBNull.Value.Equals(dr["Chart"]) ? 0 : dr["Chart"]),
                        GLAcct = Convert.ToString(dr["GLAcct"]),
                        BEscType = Convert.ToInt16(DBNull.Value.Equals(dr["BEscType"]) ? 0 : dr["BEscType"]),
                        BEscCycle = Convert.ToInt16(DBNull.Value.Equals(dr["BEscCycle"]) ? 0 : dr["BEscCycle"]),
                        BEscFact = Convert.ToDouble(DBNull.Value.Equals(dr["BEscFact"]) ? 0 : dr["BEscFact"]),
                        EscLast = Convert.ToDateTime(DBNull.Value.Equals(dr["EscLast"]) ? null : dr["EscLast"]),
                        BillRate = Convert.ToDouble(DBNull.Value.Equals(dr["BillRate"]) ? 0 : dr["BillRate"]),
                        RateOT = Convert.ToDouble(DBNull.Value.Equals(dr["RateOT"]) ? 0 : dr["RateOT"]),
                        RateNT = Convert.ToDouble(DBNull.Value.Equals(dr["RateNT"]) ? 0 : dr["RateNT"]),
                        RateMileage = Convert.ToDouble(DBNull.Value.Equals(dr["RateMileage"]) ? 0 : dr["RateMileage"]),
                        RateDT = Convert.ToDouble(DBNull.Value.Equals(dr["RateDT"]) ? 0 : dr["RateDT"]),
                        RateTravel = Convert.ToDouble(DBNull.Value.Equals(dr["RateTravel"]) ? 0 : dr["RateTravel"]),
                        PO = Convert.ToString(dr["PO"]),
                        SPHandle = Convert.ToInt16(DBNull.Value.Equals(dr["SPHandle"]) ? 0 : dr["SPHandle"]),
                        SRemarks = Convert.ToString(dr["SRemarks"]),
                        IsRenewalNotes = Convert.ToInt16(DBNull.Value.Equals(dr["IsRenewalNotes"]) ? 0 : dr["IsRenewalNotes"]),
                        RenewalNotes = Convert.ToString(dr["RenewalNotes"]),
                        Detail = Convert.ToInt16(DBNull.Value.Equals(dr["Detail"]) ? 0 : dr["Detail"]),
                        DepartmentID = Convert.ToInt16(DBNull.Value.Equals(dr["DepartmentID"]) ? 0 : dr["DepartmentID"]),
                        TaskCategory = Convert.ToString(dr["TaskCategory"]),
                        Route = Convert.ToInt32(DBNull.Value.Equals(dr["Route"]) ? 0 : dr["Route"]),
                    }
                    );
            }

            return _lstGetContract;
        }

        public DataSet GetElevContract(Contracts objPropContracts)
        {
            return objDL_Contracts.GetElevContract(objPropContracts);
        }

        //API
        public List<GetElevContractViewModel> GetElevContract(GetElevContractParam _GetElevContract, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetElevContract(_GetElevContract, ConnectionString);

            List<GetElevContractViewModel> _lstGetElevContract = new List<GetElevContractViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetElevContract.Add(
                    new GetElevContractViewModel()
                    {
                        Elev = Convert.ToInt32(DBNull.Value.Equals(dr["Elev"]) ? 0 : dr["Elev"]),
                        Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                        hours = Convert.ToDouble(DBNull.Value.Equals(dr["hours"]) ? 0 : dr["hours"]),
                    }
                    );
            }

            return _lstGetElevContract;
        }


        public DataSet getJstatus(Contracts objPropContracts)
        {
            return objDL_Contracts.getJstatus(objPropContracts);
        }

        //API
        public List<GetJstatusViewModel> getJstatus(GetJstatusParam _GetJstatus, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.getJstatus(_GetJstatus, ConnectionString);

            List<GetJstatusViewModel> _lstGetJstatus = new List<GetJstatusViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetJstatus.Add(
                    new GetJstatusViewModel()
                    {
                        status = Convert.ToString(dr["status"]),
                    }
                    );
            }

            return _lstGetJstatus;
        }

        public void AddContract(Contracts objPropContracts)
        {
            objDL_Contracts.AddContract(objPropContracts);
        }

        //API
        public void AddContract(AddContractParam _AddContract, string ConnectionString)
        {
            objDL_Contracts.AddContract(_AddContract, ConnectionString);
        }


        public void AddContractTemp(Contracts objPropContracts)
        {
            objDL_Contracts.AddContractTemp(objPropContracts);
        }

        public void UpdateContract(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateContract(objPropContracts);
        }

        //API
        public void UpdateContract(UpdateContractParam _UpdateContract, string ConnectionString)
        {
            objDL_Contracts.UpdateContract(_UpdateContract, ConnectionString);
        }

        public void DeleteContract(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteContract(objPropContracts);
        }

        //API
        public void DeleteContract(DeleteContractParam _DeleteContract, string ConnectionString)
        {
            objDL_Contracts.DeleteContract(_DeleteContract, ConnectionString);
        }

        public DataSet AddRecurringTickets(Contracts objPropContracts)
        {
            return objDL_Contracts.AddRecurringTickets(objPropContracts);
        }

        public DataSet GetLastProcessDate(Contracts objPropContracts)
        {
            return objDL_Contracts.GetLastProcessDate(objPropContracts);
        }

        public DataSet GetInvoiceLastProcessDate(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceLastProcessDate(objPropContracts);
        }

        public void CreateRecurringTickets(Contracts objPropContracts)
        {
            objDL_Contracts.CreateRecurringTickets(objPropContracts);
        }

        public DataSet CreateRecurringInvoices(Contracts objPropContracts, int IncludeContractRemarks = 0)
        {
            return objDL_Contracts.CreateRecurringInvoices(objPropContracts, IncludeContractRemarks);
        }

        public DataSet GetBillingFieldByLoc(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillingFieldByLoc(objPropContracts);
        }

        public int CreateInvoice(Contracts objPropContracts)
        {
            return objDL_Contracts.CreateInvoice(objPropContracts);
        }

        public int CreateOnlinePayment(Contracts objPropContracts)
        {
            return objDL_Contracts.CreateOnlinePayment(objPropContracts);
        }

        public void CreateQBInvoice(Contracts objPropContracts)
        {
            objDL_Contracts.CreateQBInvoice(objPropContracts);
        }

        public void CreateQBInvoiceMapping(Contracts objPropContracts)
        {
            objDL_Contracts.CreateQBInvoiceMapping(objPropContracts);
        }

        public void UpdateInvoice(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateInvoice(objPropContracts);
        }

        public DataSet GetInvoicesByID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesByID(objPropContracts);
        }

        //API
        public ListGetInvoicesByID GetInvoicesByID(GetInvoicesByIDParam _GetInvoicesByID, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetInvoicesByID(_GetInvoicesByID, ConnectionString);

            ListGetInvoicesByID _ds = new ListGetInvoicesByID();
            List<GetInvoicesByIDTable1> _lstTable1 = new List<GetInvoicesByIDTable1>();
            List<GetInvoicesByIDTable2> _lstTable2 = new List<GetInvoicesByIDTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetInvoicesByIDTable1()
                    {
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        STax = Convert.ToDouble(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        TaxRegion = Convert.ToString(dr["TaxRegion"]),
                        TaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["TaxRate"]) ? 0 : dr["TaxRate"]),
                        TaxFactor = Convert.ToDouble(DBNull.Value.Equals(dr["TaxFactor"]) ? 0 : dr["TaxFactor"]),
                        Taxable = Convert.ToDouble(DBNull.Value.Equals(dr["Taxable"]) ? 0 : dr["Taxable"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToString(dr["PO"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        GTax = Convert.ToDouble(DBNull.Value.Equals(dr["GTax"]) ? 0 : dr["GTax"]),
                        Mech = Convert.ToInt32(DBNull.Value.Equals(dr["Mech"]) ? 0 : dr["Mech"]),
                        Pricing = Convert.ToInt16(DBNull.Value.Equals(dr["Pricing"]) ? 0 : dr["Pricing"]),
                        TaxRegion2 = Convert.ToString(dr["TaxRegion2"]),
                        TaxRate2 = Convert.ToDouble(DBNull.Value.Equals(dr["TaxRate2"]) ? 0 : dr["TaxRate2"]),
                        BillToOpt = Convert.ToInt16(DBNull.Value.Equals(dr["BillToOpt"]) ? 0 : dr["BillToOpt"]),
                        BillTo = Convert.ToString(dr["BillTo"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        fUser = Convert.ToString(dr["fUser"]),
                        Custom3 = Convert.ToString(dr["Custom3"]),
                        QBInvoiceID = Convert.ToString(dr["QBInvoiceID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        DDate = Convert.ToDateTime(DBNull.Value.Equals(dr["DDate"]) ? null : dr["DDate"]),
                        GSTRate = Convert.ToDouble(DBNull.Value.Equals(dr["GSTRate"]) ? 0 : dr["GSTRate"]),
                        AssignedTo = Convert.ToInt32(DBNull.Value.Equals(dr["AssignedTo"]) ? 0 : dr["AssignedTo"]),
                        IsRecurring = Convert.ToInt32(DBNull.Value.Equals(dr["IsRecurring"]) ? 0 : dr["IsRecurring"]),
                        JobDecs = Convert.ToString(dr["JobDecs"]),
                        JobRemarks = Convert.ToString(dr["JobRemarks"]),
                        SPHandle = Convert.ToInt16(DBNull.Value.Equals(dr["SPHandle"]) ? 0 : dr["SPHandle"]),
                        SRemarks = Convert.ToString(dr["SRemarks"]),
                        InvServ = Convert.ToInt32(DBNull.Value.Equals(dr["InvServ"]) ? 0 : dr["InvServ"]),
                        //ProgressBillingNo = Convert.ToString(dr["ProgressBillingNo"]),
                        TotalTax = Convert.ToDouble(DBNull.Value.Equals(dr["TotalTax"]) ? 0 : dr["TotalTax"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        CCEMail = Convert.ToString(dr["CCEMail"]),
                        locname = Convert.ToString(dr["locname"]),
                        owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                        Address = Convert.ToString(dr["Address"]),
                        statusname = Convert.ToString(dr["statusname"]),
                        MechName = Convert.ToString(dr["MechName"]),
                        typeName = Convert.ToString(dr["typeName"]),
                        termsText = Convert.ToString(dr["termsText"]),
                        paidcc = Convert.ToInt16(DBNull.Value.Equals(dr["paidcc"]) ? 0 : dr["paidcc"]),
                        balance = Convert.ToDouble(DBNull.Value.Equals(dr["balance"]) ? 0 : dr["balance"]),
                        amtpaid = Convert.ToDouble(DBNull.Value.Equals(dr["amtpaid"]) ? 0 : dr["amtpaid"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        EmailTo = Convert.ToString(dr["EmailTo"]),
                        EmailCC = Convert.ToString(dr["EmailCC"]),
                        jobStatus = Convert.ToInt16(DBNull.Value.Equals(dr["jobStatus"]) ? 0 : dr["jobStatus"]),
                        locStatus = Convert.ToInt16(DBNull.Value.Equals(dr["locStatus"]) ? 0 : dr["locStatus"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetInvoicesByIDTable2()
                    {
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        GTaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["GTaxAmt"]) ? 0 : dr["GTaxAmt"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        JobItem = Convert.ToInt32(DBNull.Value.Equals(dr["JobItem"]) ? 0 : dr["JobItem"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        Measure = Convert.ToString(dr["Measure"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        JobOrg = Convert.ToInt32(DBNull.Value.Equals(dr["JobOrg"]) ? 0 : dr["JobOrg"]),
                        StaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["StaxAmt"]) ? 0 : dr["StaxAmt"]),
                        TotalTax = Convert.ToDouble(DBNull.Value.Equals(dr["TotalTax"]) ? 0 : dr["TotalTax"]),
                        pricequant = Convert.ToDouble(DBNull.Value.Equals(dr["pricequant"]) ? 0 : dr["pricequant"]),
                        billcode = Convert.ToString(dr["billcode"]),
                        code = Convert.ToInt32(DBNull.Value.Equals(dr["code"]) ? 0 : dr["code"]),
                        INVType = Convert.ToInt16(DBNull.Value.Equals(dr["INVType"]) ? 0 : dr["INVType"]),
                        Warehouse = Convert.ToString(dr["Warehouse"]),
                        WHLocID = Convert.ToInt32(DBNull.Value.Equals(dr["WHLocID"]) ? 0 : dr["WHLocID"]),
                        InvStatus = Convert.ToInt16(DBNull.Value.Equals(dr["InvStatus"]) ? 0 : dr["InvStatus"]),
                        AStatus = Convert.ToInt16(DBNull.Value.Equals(dr["AStatus"]) ? 0 : dr["AStatus"]),
                        ProgressBillingNo = Convert.ToString(dr["ProgressBillingNo"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }


        public DataSet GetStatusNameByInvoiceId(Contracts objPropContracts)
        {
            return objDL_Contracts.GetStatusNameByInvoiceId(objPropContracts);
        }

        public DataSet GetInvoicesAmount(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesAmount(objPropContracts);
        }

        public DataSet GetInvoicesStatus(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesStatus(objPropContracts);
        }
        public DataSet GetACHCustomerAccounts(int OwnerID, string ConnConfig)
        {
            return objDL_Contracts.GetACHCustomerAccounts(OwnerID, ConnConfig);
        }
        public void AddACHCustomerAccounts(int OwnerID, string RoutingNo, string AccountNo, string Name, string ConnConfig)
        {
            objDL_Contracts.AddACHCustomerAccounts(OwnerID, RoutingNo, AccountNo, Name, ConnConfig);
        }
        public DataSet GetBillcodesforticket(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillcodesforticket(objPropContracts);
        }



        public DataSet GetBillcodesforQBChargeableticket(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillcodesforQBChargeableticket(objPropContracts);
        }

        public DataSet GetRecurringInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetRecurringInvoices(objPropContracts);
        }

        public DataSet GetInvoices(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {
            return objDL_Contracts.GetInvoices(objPropContracts, filters, fromDate, toDate);
        }

        //Maintenance Cancelled
        public DataSet GetManitenanceCancelled(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {
            return objDL_Contracts.GetMaintenanaceCancelled(objPropContracts, filters, fromDate, toDate);
        }

        //PayRoll Check Report
        public DataSet GetPayrollCheck(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {
            return objDL_Contracts.GetPayRollReportData(objPropContracts, filters, fromDate, toDate);
        }

        //Payroll Liability Report
        public DataSet GetPayrollLiabilityReport(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {
            return objDL_Contracts.GetPayrollLiabilityReportData(objPropContracts, filters, fromDate, toDate);
        }

        //Payroll Liability Min Date
        public DateTime GetPayrollLiabilityMinDate(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPayrollLiabilityMinDate(objPropContracts);
        }

        //Payroll Liability Max Date
        public DateTime GetPayrollLiabilityMaxDate(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPayrollLiabilityMaxDate(objPropContracts);
        }

        //PayRoll Check Report By Title
        public DataSet GetCheckReportByTitle(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {
            return objDL_Contracts.GetCheckReportByTitle(objPropContracts, filters, fromDate, toDate);
        }

        //Comprehensive Report Get Data
        public DataSet GetComprehensiveReport(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {
            return objDL_Contracts.GetComprehensiveReport(objPropContracts, filters, fromDate, toDate);
        }
        public DataSet GetProjectARInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetProjectARInvoices(objPropContracts);
        }

        public DataSet GetInvoicesReceivePayment(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesReceivePayment(objPropContracts);
        }

        public DataSet GetInvoiceByRecurringFrequency(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceByRecurringFrequency(objPropContracts);
        }

        public DataSet GetAPInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetAPInvoices(objPropContracts);
        }

        public DataSet GetJobCostItems(Contracts objPropContracts)
        {
            return objDL_Contracts.GetJobCostItems(objPropContracts);
        }

        public void DeleteInvoice(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteInvoice(objPropContracts);
        }

        public void DeleteInvoiceByListID(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteInvoiceByListID(objPropContracts);
        }

        public object AddPayment(Contracts objPropContracts)
        {
            return objDL_Contracts.AddPayment(objPropContracts);
        }
        public object UpdateACHpaymentHistry(Contracts objPropContracts, string Responsestatus, string PaymentUID)
        {
            return objDL_Contracts.UpdateACHpaymentHistry(objPropContracts, Responsestatus, PaymentUID);
        }
        public DataSet GetPaymentHistory(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPaymentHistory(objPropContracts);
        }
        public DataSet GetACHPaymentHistory(Contracts objPropContracts, string Status)
        {
            return objDL_Contracts.GetACHPaymentHistory(objPropContracts, Status);
        }
        public DataSet getPaymentGatewayInfo(Contracts objPropContracts)
        {
            return objDL_Contracts.getPaymentGatewayInfo(objPropContracts);
        }

        public void AddMerchant(Contracts objPropContracts)
        {
            objDL_Contracts.AddMerchant(objPropContracts);
        }

        public void DeleteMerchant(Contracts objPropContracts)
        {
            objDL_Contracts.DeleteMerchant(objPropContracts);
        }

        public int GetMaxQBInvoiceID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetMaxQBInvoiceID(objPropContracts);
        }

        public DataSet GetBillcodesforTimeSheet(Contracts objPropContracts)
        {
            return objDL_Contracts.GetBillcodesforTimeSheet(objPropContracts);
        }
        public DataSet GetPayrollforTimeSheet(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPayrollforTimeSheet(objPropContracts);
        }
        public DataSet GetPayrollByAccount(Contracts objPropContracts)
        {
            return objDL_Contracts.GetPayrollByAccount(objPropContracts);
        }
        public DataSet getCustomerAddress(Contracts objPropContracts)
        {
            return objDL_Contracts.getCustomerAddress(objPropContracts);
        }
        public DataSet GetInvoicesByBatch(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesByBatch(objPropContracts);
        }
        public void UpdateCustomerBalance(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateCustomerBalance(objPropContracts);
        }

        //API
        public void UpdateCustomerBalance(UpdateCustomerBalanceParam _UpdateCustomerBalance, string ConnectionString)
        {
            objDL_Contracts.UpdateCustomerBalance(_UpdateCustomerBalance, ConnectionString);
        }

        public bool IsExistContractByLoc(Contracts objPropContracts)
        {
            return objDL_Contracts.IsExistContractByLoc(objPropContracts);
        }

        //API
        public bool IsExistContractByLoc(IsExistContractByLocParam _IsExistContractByLoc, string ConnectionString)
        {
            return objDL_Contracts.IsExistContractByLoc(_IsExistContractByLoc, ConnectionString);
        }
        public DataSet GetLastProcessDateOfInvoice(Contracts objPropContracts)
        {
            return objDL_Contracts.GetLastProcessDateOfInvoice(objPropContracts);
        }
        public DataSet GetInvoicesDetailsByID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesDetailsByID(objPropContracts);
        }
        public DataSet GetEmailDetailByLoc(Contracts objPropContracts)
        {
            return objDL_Contracts.GetEmailDetailByLoc(objPropContracts);
        }

        //API
        public List<GetEmailDetailByLocViewModel> GetEmailDetailByLoc(GetEmailDetailByLocParam _GetEmailDetailByLoc, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetEmailDetailByLoc(_GetEmailDetailByLoc, ConnectionString);

            List<GetEmailDetailByLocViewModel> _lstGetEmailDetailByLoc = new List<GetEmailDetailByLocViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEmailDetailByLoc.Add(
                    new GetEmailDetailByLocViewModel()
                    {
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        ID = Convert.ToString(dr["ID"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        custom12 = Convert.ToString(dr["custom12"]),
                        custom13 = Convert.ToString(dr["custom13"]),
                    }
                    );
            }

            return _lstGetEmailDetailByLoc;
        }

        public DataSet GetInvoicesByRef(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesByRef(objPropContracts);
        }

        //API
        public List<GetInvoicesByRefViewModel> GetInvoicesByRef(GetInvoicesByRefParam _GetInvoicesByRef, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetInvoicesByRef(_GetInvoicesByRef, ConnectionString);

            List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInvoicesByRef.Add(
                    new GetInvoicesByRefViewModel()
                    {
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        STax = Convert.ToDouble(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        TaxRegion = Convert.ToString(dr["TaxRegion"]),
                        TaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["TaxRate"]) ? 0 : dr["TaxRate"]),
                        TaxFactor = Convert.ToDouble(DBNull.Value.Equals(dr["TaxFactor"]) ? 0 : dr["TaxFactor"]),
                        Taxable = Convert.ToDouble(DBNull.Value.Equals(dr["Taxable"]) ? 0 : dr["Taxable"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        PO = Convert.ToString(dr["PO"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Batch = Convert.ToInt32(DBNull.Value.Equals(dr["Batch"]) ? 0 : dr["Batch"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        GTax = Convert.ToDouble(DBNull.Value.Equals(dr["GTax"]) ? 0 : dr["GTax"]),
                        Mech = Convert.ToInt32(DBNull.Value.Equals(dr["Mech"]) ? 0 : dr["Mech"]),
                        Pricing = Convert.ToInt16(DBNull.Value.Equals(dr["Pricing"]) ? 0 : dr["Pricing"]),
                        TaxRegion2 = Convert.ToString(dr["TaxRegion2"]),
                        TaxRate2 = Convert.ToDouble(DBNull.Value.Equals(dr["TaxRate2"]) ? 0 : dr["TaxRate2"]),
                        BillToOpt = Convert.ToInt16(DBNull.Value.Equals(dr["BillToOpt"]) ? 0 : dr["BillToOpt"]),
                        BillTo = Convert.ToString(dr["BillTo"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        IDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IDate"]) ? null : dr["IDate"]),
                        fUser = Convert.ToString(dr["fUser"]),
                        Custom3 = Convert.ToString(dr["Custom3"]),
                        QBInvoiceID = Convert.ToString(dr["QBInvoiceID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        DDate = Convert.ToDateTime(DBNull.Value.Equals(dr["DDate"]) ? null : dr["DDate"]),
                        GSTRate = Convert.ToDouble(DBNull.Value.Equals(dr["GSTRate"]) ? 0 : dr["GSTRate"]),
                        AssignedTo = Convert.ToInt32(DBNull.Value.Equals(dr["AssignedTo"]) ? 0 : dr["AssignedTo"]),
                        IsRecurring = Convert.ToInt32(DBNull.Value.Equals(dr["IsRecurring"]) ? 0 : dr["IsRecurring"]),
                        DueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["DueDate"]) ? null : dr["DueDate"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        Billing = Convert.ToInt16(DBNull.Value.Equals(dr["Billing"]) ? 0 : dr["Billing"]),
                        LocName = Convert.ToString(dr["LocName"]),
                        BillToAddress = Convert.ToString(dr["BillToAddress"]),
                        BillToCity = Convert.ToString(dr["BillToCity"]),
                        BillToState = Convert.ToString(dr["BillToState"]),
                        BillToZip = Convert.ToString(dr["BillToZip"]),
                        owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                        Address = Convert.ToString(dr["Address"]),
                        ID = Convert.ToString(dr["ID"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        CCEMail = Convert.ToString(dr["CCEMail"]),
                        TerrName = Convert.ToString(dr["TerrName"]),
                        RouteName = Convert.ToString(dr["RouteName"]),
                        StatusName = Convert.ToString(dr["StatusName"]),
                        MechName = Convert.ToString(dr["MechName"]),
                        TypeName = Convert.ToString(dr["TypeName"]),
                        TermsText = Convert.ToString(dr["TermsText"]),
                        Payterms = Convert.ToInt16(DBNull.Value.Equals(dr["Payterms"]) ? 0 : dr["Payterms"]),
                        PO1 = Convert.ToString(dr["PO"]),
                        PaidCC = Convert.ToDouble(DBNull.Value.Equals(dr["PaidCC"]) ? 0 : dr["PaidCC"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        AmtPaid = Convert.ToDouble(DBNull.Value.Equals(dr["AmtPaid"]) ? 0 : dr["AmtPaid"]),
                        IsExistsEmail = Convert.ToInt32(DBNull.Value.Equals(dr["IsExistsEmail"]) ? 0 : dr["IsExistsEmail"]),
                        EmailInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailInvoice"]) ? false : dr["EmailInvoice"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        EmailTo = Convert.ToString(dr["EmailTo"]),
                        EmailCC = Convert.ToString(dr["EmailCC"]),
                        JobRemarks = Convert.ToString(dr["JobRemarks"]),
                        ProgressBillingNo = Convert.ToString(dr["ProgressBillingNo"]),
                        Rate = Convert.ToDouble(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                        PSTReg = Convert.ToString(dr["PSTReg"]),
                        STaxType = Convert.ToInt16(DBNull.Value.Equals(dr["STaxType"]) ? 0 : dr["STaxType"]),
                    }
                    );
            }

            return _lstGetInvoicesByRef;
        }

        public DataSet GetAllTicketID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetAllTicketID(objPropContracts);
        }

        //API
        public List<GetTicketIDViewModel> GetAllTicketID(GetAllTicketIDParam _GetAllTicketID, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetAllTicketID(_GetAllTicketID, ConnectionString);

            List<GetTicketIDViewModel> _lstGetAllTicketID = new List<GetTicketIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllTicketID.Add(
                    new GetTicketIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    }
                    );
            }

            return _lstGetAllTicketID;
        }

        public DataSet GetTicketID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetTicketID(objPropContracts);
        }

        //API
        public List<GetTicketIDViewModel> GetTicketID(GetTicketIDParam _GetTicketID, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetTicketID(_GetTicketID, ConnectionString);

            List<GetTicketIDViewModel> _lstGetTicketID = new List<GetTicketIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetTicketID.Add(
                    new GetTicketIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    }
                    );
            }

            return _lstGetTicketID;
        }
        public DataSet GetInvoiceItemByRef(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceItemByRef(objPropContracts);
        }

        //API
        public List<GetInvoiceItemByRefViewModel> GetInvoiceItemByRef(GetInvoiceItemByRefParam _GetInvoiceItemByRef, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetInvoiceItemByRef(_GetInvoiceItemByRef, ConnectionString);

            List<GetInvoiceItemByRefViewModel> _lstGetInvoiceItemByRef = new List<GetInvoiceItemByRefViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInvoiceItemByRef.Add(
                    new GetInvoiceItemByRefViewModel()
                    {
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Acct = Convert.ToInt32(DBNull.Value.Equals(dr["Acct"]) ? 0 : dr["Acct"]),
                        Quan = Convert.ToDouble(DBNull.Value.Equals(dr["Quan"]) ? 0 : dr["Quan"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        JobItem = Convert.ToInt32(DBNull.Value.Equals(dr["JobItem"]) ? 0 : dr["JobItem"]),
                        Measure = Convert.ToString(dr["Measure"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        ProgressBillingNo = Convert.ToString(dr["ProgressBillingNo"]),
                        staxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["staxAmt"]) ? 0 : dr["staxAmt"]),
                        GTaxAmt = Convert.ToDouble(DBNull.Value.Equals(dr["GTaxAmt"]) ? 0 : dr["GTaxAmt"]),
                        TotalTax = Convert.ToDouble(DBNull.Value.Equals(dr["TotalTax"]) ? 0 : dr["TotalTax"]),
                        pricequant = Convert.ToDouble(DBNull.Value.Equals(dr["pricequant"]) ? 0 : dr["pricequant"]),
                        billcode = Convert.ToString(dr["billcode"]),
                        code = Convert.ToInt32(DBNull.Value.Equals(dr["code"]) ? 0 : dr["code"]),
                        balance = Convert.ToDouble(DBNull.Value.Equals(dr["balance"]) ? 0 : dr["balance"]),
                        amtpaid = Convert.ToDouble(DBNull.Value.Equals(dr["amtpaid"]) ? 0 : dr["amtpaid"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        JobOrg = Convert.ToInt32(DBNull.Value.Equals(dr["JobOrg"]) ? 0 : dr["JobOrg"]),
                        INVSTax = Convert.ToDouble(DBNull.Value.Equals(dr["INVSTax"]) ? 0 : dr["INVSTax"]),
                        INVAmount = Convert.ToDouble(DBNull.Value.Equals(dr["INVAmount"]) ? 0 : dr["INVAmount"]),
                        Taxable = Convert.ToDouble(DBNull.Value.Equals(dr["Taxable"]) ? 0 : dr["Taxable"]),
                        Description = Convert.ToString(dr["Description"]),
                        TaxRegion = Convert.ToString(dr["TaxRegion"]),
                    }
                    );
            }

            return _lstGetInvoiceItemByRef;
        }
        public void UpdateVoidInvoiceDetails(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateVoidInvoiceDetails(objPropContracts);
        }
        public void UpdateExpirationDate(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateExpirationDate(objPropContracts);
        }

        //API
        public void UpdateExpirationDate(UpdateExpirationDateParam _UpdateExpirationDate, string ConnectionString)
        {
            objDL_Contracts.UpdateExpirationDate(_UpdateExpirationDate, ConnectionString);
        }
        public DataSet GetCustomerStatementInvoices(Contracts objPropContracts, bool includeCredit)
        {
            return objDL_Contracts.GetCustomerStatementInvoices(objPropContracts, includeCredit);
        }

        //API
        public List<GetCustStatementInvSouthernViewModel> GetCustomerStatementInvoices(GetCustomerStatementInvoicesParam _GetCustomerStatementInvoices, string ConnectionString, bool includeCredit)
        {
            DataSet ds = objDL_Contracts.GetCustomerStatementInvoices(_GetCustomerStatementInvoices, ConnectionString, includeCredit);

            List<GetCustStatementInvSouthernViewModel> _lstGetCustomerStatementInvoices = new List<GetCustStatementInvSouthernViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerStatementInvoices.Add(
                    new GetCustStatementInvSouthernViewModel()
                    {
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Type = Convert.ToString(dr["Type"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Days = Convert.ToInt32(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                    }
                    );
            }

            return _lstGetCustomerStatementInvoices;
        }

        public DataSet GetCustomerStatementInvoicesByOwner(Contracts objPropContracts, bool includeCredit)
        {
            return objDL_Contracts.GetCustomerStatementInvoicesByOwner(objPropContracts, includeCredit);
        }


        //By Customer(Owner)
        public DataSet GetCustomerStatementInvoicesByOwnerByCustId(Contracts objPropContracts, bool includeCredit,string Customer)
        {
            return objDL_Contracts.GetCustomerStatementInvoicesByOwnerByCustId(objPropContracts, includeCredit, Customer);
        }

        public DataSet GetCustomerStatementInvoicesByLocation(Contracts objPropContracts, bool includeCredit)
        {
            return objDL_Contracts.GetCustomerStatementInvoicesByLocation(objPropContracts, includeCredit);
        }

        //API
        public List<GetCustStatementInvSouthernViewModel> GetCustomerStatementInvoicesByLocation(GetCustStatementInvByLocationParam _GetCustStatementInvByLocation, string ConnectionString, bool includeCredit)
        {
            DataSet ds = objDL_Contracts.GetCustomerStatementInvoicesByLocation(_GetCustStatementInvByLocation, ConnectionString, includeCredit);

            List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvByLocation = new List<GetCustStatementInvSouthernViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustStatementInvByLocation.Add(
                    new GetCustStatementInvSouthernViewModel()
                    {
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Type = Convert.ToString(dr["Type"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Days = Convert.ToInt32(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                    }
                    );
            }

            return _lstGetCustStatementInvByLocation;
        }

        public DataSet GetCustomerStatementInvoicesSouthern(Contracts objPropContracts, bool includeCredit)
        {
            return objDL_Contracts.GetCustomerStatementInvoicesSouthern(objPropContracts, includeCredit);
        }

        //API
        public List<GetCustStatementInvSouthernViewModel> GetCustomerStatementInvoicesSouthern(GetCustomerStatementInvoicesSouthernParam _GetCustStatementInvSouthern, string ConnectionString, bool includeCredit)
        {
            DataSet ds = objDL_Contracts.GetCustomerStatementInvoicesSouthern(_GetCustStatementInvSouthern, ConnectionString, includeCredit);

            List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvSouthern = new List<GetCustStatementInvSouthernViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustStatementInvSouthern.Add(
                    new GetCustStatementInvSouthernViewModel()
                    {
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Type = Convert.ToString(dr["Type"]),
                        Ref = Convert.ToString(dr["Ref"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Days = Convert.ToInt32(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                    }
                    );
            }

            return _lstGetCustStatementInvSouthern;

        }

        public DataSet GetCustomerStatement(Contracts objPropContracts, bool includeCredit, bool includeCustomerCredit)
        {
            return objDL_Contracts.GetCustomerStatement(objPropContracts, includeCredit, includeCustomerCredit);
        }

        //Get Report By Customer 
        //By Prateek
        public DataSet GetCustomerStatementByCustomer(Contracts objPropContracts, bool includeCredit, bool includeCustomerCredit, string Customer)
        {
            return objDL_Contracts.GetCustomerStatementByCustomer(objPropContracts, includeCredit, includeCustomerCredit, Customer);
        }

        public DataSet GetCustomerStatementDetails(Contracts objPropContracts)
        {
            return objDL_Contracts.GetCustomerStatementDetails(objPropContracts);
        }

        public DataSet GetCustomerStatementByLoc(Contracts objPropContracts)
        {
            return objDL_Contracts.GetCustomerStatementByLoc(objPropContracts);
        }

        //API
        public List<GetCustomerStatementByLocViewModel> GetCustomerStatementByLoc(GetCustomerStatementByLocParam _GetCustomerStatementByLoc, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetCustomerStatementByLoc(_GetCustomerStatementByLoc, ConnectionString);

            List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLoc = new List<GetCustomerStatementByLocViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerStatementByLoc.Add(
                    new GetCustomerStatementByLocViewModel()
                    {
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        locname = Convert.ToString(dr["locname"]),
                        locAddress = Convert.ToString(dr["locAddress"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        custAddress = Convert.ToString(dr["custAddress"]),
                        Status = Convert.ToString(dr["Status"]),
                        Type = Convert.ToString(dr["Type"]),
                        Terr = Convert.ToString(dr["Terr"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        Selected = Convert.ToDouble(DBNull.Value.Equals(dr["Selected"]) ? 0 : dr["Selected"]),
                        ZeroDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroDay"]) ? 0 : dr["ZeroDay"]),
                        ThirtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyOneDay"]) ? 0 : dr["ThirtyOneDay"]),
                        SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        IsExistsEmail = Convert.ToInt32(DBNull.Value.Equals(dr["IsExistsEmail"]) ? 0 : dr["IsExistsEmail"]),
                        Custom12 = Convert.ToString(dr["Custom12"]),
                        Custom13 = Convert.ToString(dr["Custom13"]),
                    }
                    );
            }

            return _lstGetCustomerStatementByLoc;
        }

        public DataSet GetCustomerStatementByLocs(Contracts objPropContracts)
        {
            return objDL_Contracts.GetCustomerStatementByLocs(objPropContracts);
        }

        //API
        public List<GetCustomerStatementByLocViewModel> GetCustomerStatementByLocs(GetCustomerStatementByLocsParam _GetCustomerStatementByLocs, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetCustomerStatementByLocs(_GetCustomerStatementByLocs, ConnectionString);

            List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLocs = new List<GetCustomerStatementByLocViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerStatementByLocs.Add(
                    new GetCustomerStatementByLocViewModel()
                    {
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        locname = Convert.ToString(dr["locname"]),
                        locAddress = Convert.ToString(dr["locAddress"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        custAddress = Convert.ToString(dr["custAddress"]),
                        Status = Convert.ToString(dr["Status"]),
                        Type = Convert.ToString(dr["Type"]),
                        Terr = Convert.ToString(dr["Terr"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        Selected = Convert.ToDouble(DBNull.Value.Equals(dr["Selected"]) ? 0 : dr["Selected"]),
                        ZeroDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroDay"]) ? 0 : dr["ZeroDay"]),
                        ThirtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyOneDay"]) ? 0 : dr["ThirtyOneDay"]),
                        SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        IsExistsEmail = Convert.ToInt32(DBNull.Value.Equals(dr["IsExistsEmail"]) ? 0 : dr["IsExistsEmail"]),
                        Custom12 = Convert.ToString(dr["Custom12"]),
                        Custom13 = Convert.ToString(dr["Custom13"]),
                    }
                    );
            }

            return _lstGetCustomerStatementByLocs;
        }

        public DataSet GetCustomerStatementCollection(Contracts objPropContracts, bool includeCredit, bool includeCustomerCredit)
        {
            return objDL_Contracts.GetCustomerStatementCollection(objPropContracts, includeCredit, includeCustomerCredit);
        }

        //API
        public List<GetCustomerStatementCollectionViewModel> GetCustomerStatementCollection(GetCustomerStatementCollectionParam _GetCustomerStatementCollection, string ConnectionString, bool includeCredit, bool includeCustomerCredit)
        {
            DataSet ds = objDL_Contracts.GetCustomerStatementCollection(_GetCustomerStatementCollection, ConnectionString, includeCredit, includeCustomerCredit);

            List<GetCustomerStatementCollectionViewModel> _lstGetCustomerStatementCollection = new List<GetCustomerStatementCollectionViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerStatementCollection.Add(
                    new GetCustomerStatementCollectionViewModel()
                    {
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        locname = Convert.ToString(dr["locname"]),
                        locAddress = Convert.ToString(dr["locAddress"]),
                        customerName = Convert.ToString(dr["customerName"]),
                        custAddress = Convert.ToString(dr["custAddress"]),
                        Status = Convert.ToString(dr["Status"]),
                        Type = Convert.ToString(dr["Type"]),
                        Terr = Convert.ToString(dr["Terr"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        Selected = Convert.ToDouble(DBNull.Value.Equals(dr["Selected"]) ? 0 : dr["Selected"]),
                        ZeroDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroDay"]) ? 0 : dr["ZeroDay"]),
                        ThirtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyOneDay"]) ? 0 : dr["ThirtyOneDay"]),
                        SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        IsExistsEmail = Convert.ToInt32(DBNull.Value.Equals(dr["IsExistsEmail"]) ? 0 : dr["IsExistsEmail"]),
                        Custom12 = Convert.ToString(dr["Custom12"]),
                        Custom13 = Convert.ToString(dr["Custom13"]),
                    }
                    );
            }

            return _lstGetCustomerStatementCollection;
        }


        public DataSet GetARAgingByAsOfDate(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByAsOfDate(objPropContracts, creditFlag);
        }

        public DataSet GetARAgingByAsOfDateOver90DaysReport(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByAsOfDateOver90DaysReport(objPropContracts, creditFlag);
        }
        public DataSet GetARAgingByLocation(Contracts objPropContracts)
        {
            return objDL_Contracts.GetARAgingByLocation(objPropContracts);
        }

        public DataSet GetARAgingSummaryByLocation(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingSummaryByLocation(objPropContracts, creditFlag);
        }
        //API
        public ListGetARAgingByAsOfDate GetARAgingByAsOfDate(GetARAgingByAsOfDateParam _GetARAgingByAsOfDate, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetARAgingByAsOfDate(_GetARAgingByAsOfDate, ConnectionString);

            ListGetARAgingByAsOfDate _ds = new ListGetARAgingByAsOfDate();

            if (_GetARAgingByAsOfDate.isDBTotalService)
            {
                List<GetARAgingByAsOfDateTable1> _lstTable1 = new List<GetARAgingByAsOfDateTable1>();
                List<GetARAgingByAsOfDateTable2> _lstTable2 = new List<GetARAgingByAsOfDateTable2>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable1.Add(
                        new GetARAgingByAsOfDateTable1()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            Type = Convert.ToString(dr["Type"]),
                            LocType = Convert.ToString(dr["LocType"]),
                            cid = Convert.ToString(dr["cid"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                            CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                            SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                            ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                            SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                            SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                            ZeroThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroThirtyDay"]) ? 0 : dr["ZeroThirtyDay"]),
                            NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                            NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                            OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                            Status = Convert.ToString(dr["Status"]),
                            Custom1 = Convert.ToString(dr["Custom1"]),
                        }
                        );
                }
                _ds.lstTable1 = _lstTable1;

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    _lstTable2.Add(
                        new GetARAgingByAsOfDateTable2()
                        {
                            Column1 = Convert.ToDouble(DBNull.Value.Equals(dr["Column1"]) ? 0 : dr["Column1"]),
                        }
                        );
                }
                _ds.lstTable2 = _lstTable2;
            }
            else
            {
                List<GetARAgingByAsOfDateTable3> _lstTable3 = new List<GetARAgingByAsOfDateTable3>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable3.Add(
                        new GetARAgingByAsOfDateTable3()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            type = Convert.ToInt16(DBNull.Value.Equals(dr["type"]) ? 0 : dr["type"]),
                            cid = Convert.ToString(dr["cid"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                            CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                            SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                            ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                            SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                            SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                            ZeroThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroThirtyDay"]) ? 0 : dr["ZeroThirtyDay"]),
                            NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                            NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                            OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                            Sel = Convert.ToInt16(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        }
                        );
                }
                _ds.lstTable3 = _lstTable3;
            }

            return _ds;
        }

        public DataSet GetARAgingByAsOfDateDep(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByAsOfDateDep(objPropContracts, creditFlag);
        }

        //API
        public ListGetARAgingByAsOfDateDep GetARAgingByAsOfDateDep(GetARAgingByAsOfDateDepParam _GetARAgingByAsOfDateDep, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetARAgingByAsOfDateDep(_GetARAgingByAsOfDateDep, ConnectionString);

            ListGetARAgingByAsOfDateDep _ds = new ListGetARAgingByAsOfDateDep();

            if (_GetARAgingByAsOfDateDep.isDBTotalService)
            {
                List<GetARAgingByAsOfDateDepTable1> _lstTable1 = new List<GetARAgingByAsOfDateDepTable1>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable1.Add(
                        new GetARAgingByAsOfDateDepTable1()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            type = Convert.ToString(dr["type"]),
                            cid = Convert.ToString(dr["cid"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                            CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                            SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                            ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                            SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                            SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                            ZeroThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroThirtyDay"]) ? 0 : dr["ZeroThirtyDay"]),
                            NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                            NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                            OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                            Department = Convert.ToString(dr["Department"]),
                        }
                        );
                }
                _ds.lstTable1 = _lstTable1;
            }
            else
            {
                List<GetARAgingByAsOfDateDepTable2> _lstTable2 = new List<GetARAgingByAsOfDateDepTable2>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable2.Add(
                        new GetARAgingByAsOfDateDepTable2()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            type = Convert.ToString(dr["type"]),
                            cid = Convert.ToString(dr["cid"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                            CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                            SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                            ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                            SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                            SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                            ZeroThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroThirtyDay"]) ? 0 : dr["ZeroThirtyDay"]),
                            NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                            NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                            OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                            Sel = Convert.ToInt16(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                            Custom1 = Convert.ToString(dr["Custom1"]),
                            Department = Convert.ToString(dr["Department"]),
                        }
                        );
                }
                _ds.lstTable2 = _lstTable2;
            }

            return _ds;
        }

        public DataSet GetARAging(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAging(objPropContracts, creditFlag);
        }

        public DataSet GetARAgingOver90DaysReport(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingOver90DaysReport(objPropContracts, creditFlag);
        }

        //API
        public ListGetARAging GetARAging(GetARAgingParam _GetARAging, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetARAging(_GetARAging, ConnectionString);

            ListGetARAging _ds = new ListGetARAging();
            List<GetARAgingTable1> _lstTable1 = new List<GetARAgingTable1>();
            List<GetARAgingTable2> _lstTable2 = new List<GetARAgingTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetARAgingTable1()
                    {
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        Type = Convert.ToString(dr["Type"]),
                        LocType = Convert.ToString(dr["LocType"]),
                        cid = Convert.ToString(dr["cid"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        LocName = Convert.ToString(dr["LocName"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                        CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                        CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                        SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                        ZeroThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroThirtyDay"]) ? 0 : dr["ZeroThirtyDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                        Status = Convert.ToString(dr["Status"]),
                    }
                    );
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetARAgingTable2()
                    {
                        Column1 = Convert.ToDouble(DBNull.Value.Equals(dr["Column1"]) ? 0 : dr["Column1"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }

        public DataSet GetARAgingDep(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingDep(objPropContracts, creditFlag);
        }

        //API
        public List<GetARAgingDepViewModel> GetARAgingDep(GetARAgingDepParam _GetARAgingDep, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetARAgingDep(_GetARAgingDep, ConnectionString);

            List<GetARAgingDepViewModel> _lstGetARAgingDep = new List<GetARAgingDepViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetARAgingDep.Add(
                    new GetARAgingDepViewModel()
                    {
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        type = Convert.ToString(dr["type"]),
                        Department = Convert.ToString(dr["Department"]),
                        cid = Convert.ToString(dr["cid"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        LocName = Convert.ToString(dr["LocName"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                        CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                        SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                        ZeroThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroThirtyDay"]) ? 0 : dr["ZeroThirtyDay"]),
                    }
                    );
            }

            return _lstGetARAgingDep;
        }

        public DataSet GetEscalationContracts(Contracts objPropContracts ,string Load , bool include)
        {
            return objDL_Contracts.GetEscalationContracts(objPropContracts, Load, include);
        }

        //API
        public List<GetEscalationContractsViewModel> GetEscalationContracts(GetEscalationContractsParam _GetEscalationContracts, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetEscalationContracts(_GetEscalationContracts, ConnectionString);

            List<GetEscalationContractsViewModel> _lstGetEscalationContracts = new List<GetEscalationContractsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEscalationContracts.Add(
                    new GetEscalationContractsViewModel()
                    {
                        DuplicateCount = Convert.ToInt64(DBNull.Value.Equals(dr["DuplicateCount"]) ? 0 : dr["DuplicateCount"]),
                        locid = Convert.ToString(dr["locid"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        CType = Convert.ToString(dr["CType"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        Freqency = Convert.ToString(dr["Freqency"]),
                        EscType = Convert.ToString(dr["EscType"]),
                        Action = Convert.ToString(dr["Action"]),
                        BEscCycle = Convert.ToInt16(DBNull.Value.Equals(dr["BEscCycle"]) ? 0 : dr["BEscCycle"]),
                        BEscType = Convert.ToInt16(DBNull.Value.Equals(dr["BEscType"]) ? 0 : dr["BEscType"]),
                        BEscFact = Convert.ToDouble(DBNull.Value.Equals(dr["BEscFact"]) ? 0 : dr["BEscFact"]),
                        EscLast = Convert.ToString(dr["EscLast"]),
                        BStart = Convert.ToString(dr["BStart"]),
                        Bfinish = Convert.ToString(dr["Bfinish"]),
                        nextdue = Convert.ToString(dr["nextdue"]),
                        Bamt = Convert.ToDouble(DBNull.Value.Equals(dr["Bamt"]) ? 0 : dr["Bamt"]),
                        newamt = Convert.ToDouble(DBNull.Value.Equals(dr["newamt"]) ? 0 : dr["newamt"]),
                        BLenght = Convert.ToInt16(DBNull.Value.Equals(dr["BLenght"]) ? 0 : dr["BLenght"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        ExpirationDate = Convert.ToString(dr["ExpirationDate"]),
                        RenewalNotes = Convert.ToString(dr["RenewalNotes"]),
                        Status = Convert.ToString(dr["Status"]),
                        Company = Convert.ToString(dr["Company"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        LocationCompanyName = Convert.ToString(dr["LocationCompanyName"]),
                    }
                    );
            }

            return _lstGetEscalationContracts;
        }

        public void EscalateContract(Contracts objPropContracts)
        {
            objDL_Contracts.EscalateContract(objPropContracts);
        }

        public void UpdateEscalationFactor(Contracts objPropContracts)
        {
            objDL_Contracts.UpdateEscalationFactor(objPropContracts);
        }

        

        //API
        public void EscalateContract(EscalateContractParam _EscalateContract, string ConnectionString)
        {
            objDL_Contracts.EscalateContract(_EscalateContract, ConnectionString);
        }
        public DataSet GetARGLReg(Contracts objContract)
        {
            return objDL_Contracts.GetARGLReg(objContract);
        }

        //Invoice By Billing Code For ES-6940 by PS
        public DataSet GetARGLRegByBillingCode(Contracts objContract)
        {
            return objDL_Contracts.GetARGLRegByBillingCode(objContract);
        }

        public DataSet GetBillableServiceReport(Contracts objContract)
        {
            return objDL_Contracts.GetBillableServiceReport(objContract);
        }
        public DataSet GetCollectionInvoices(Contracts objPropContracts)
        {
            return objDL_Contracts.GetCollectionInvoices(objPropContracts);
        }
        public DataSet GetCollectionInvoicesCompany(Contracts objPropContracts)
        {
            return objDL_Contracts.GetCollectionInvoicesCompany(objPropContracts);
        }
        public DataSet GetOutstandingInvoice(Contracts objPropContracts)
        {
            return objDL_Contracts.GetOutstandingInvoice(objPropContracts);
        }
        public DataSet GetEquipmentByInvoice(Contracts objPropContracts)
        {
            return objDL_Contracts.GetEquipmentByInvoice(objPropContracts);
        }
        public DataSet GetRecurringContractLogs(Contracts objPropContracts)
        {
            return objDL_Contracts.GetRecurringContractLogs(objPropContracts);
        }

        //API
        public List<GetRecurringContractLogsViewModel> GetRecurringContractLogs(GetRecurringContractLogsParam _GetRecurringContractLogs, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetRecurringContractLogs(_GetRecurringContractLogs, ConnectionString);

            List<GetRecurringContractLogsViewModel> _lstGetRecurringContractLogs = new List<GetRecurringContractLogsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetRecurringContractLogs.Add(
                    new GetRecurringContractLogsViewModel()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt64(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }

            return _lstGetRecurringContractLogs;
        }
        public DataSet GetInvoiceLogs(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceLogs(objPropContracts);
        }
        public DataSet GetARAgingByTerritory(Contracts objPropContracts, string territories, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByTerritory(objPropContracts, territories, creditFlag);
        }

        //API
        public ListGetARAgingByTerritory GetARAgingByTerritory(GetARAgingByTerritoryParam _GetARAgingByTerritory, string ConnectionString, string territories)
        {
            DataSet ds = objDL_Contracts.GetARAgingByTerritory(_GetARAgingByTerritory, ConnectionString, territories);

            ListGetARAgingByTerritory _ds = new ListGetARAgingByTerritory();

            if (_GetARAgingByTerritory.isDBTotalService)
            {
                List<GetARAgingByTerritoryTable1> _lstTable1 = new List<GetARAgingByTerritoryTable1>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable1.Add(
                        new GetARAgingByTerritoryTable1()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            Salesperson = Convert.ToString(dr["Salesperson"]),
                            DefaultSalesperson = Convert.ToString(dr["DefaultSalesperson"]),
                            Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                            CID = Convert.ToString(dr["CID"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            Custom1 = Convert.ToString(dr["Custom1"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                            ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                            SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                            SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                            NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                            OverNintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OverNintyDay"]) ? 0 : dr["OverNintyDay"]),
                            OverOneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OverOneTwentyDay"]) ? 0 : dr["OverOneTwentyDay"]),
                            OnetwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OnetwentyDay"]) ? 0 : dr["OnetwentyDay"]),
                            Status = Convert.ToString(dr["Status"]),
                        }
                        );
                }

                _ds.lstTable1 = _lstTable1;
            }
            else
            {
                List<GetARAgingByTerritoryTable2> _lstTable2 = new List<GetARAgingByTerritoryTable2>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable2.Add(
                        new GetARAgingByTerritoryTable2()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            Salesperson = Convert.ToString(dr["Salesperson"]),
                            DefaultSalesperson = Convert.ToString(dr["DefaultSalesperson"]),
                            Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                            CID = Convert.ToString(dr["CID"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            Custom1 = Convert.ToString(dr["Custom1"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                            SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                            NintyoneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyoneDay"]) ? 0 : dr["NintyoneDay"]),
                            OverOneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OverOneTwentyDay"]) ? 0 : dr["OverOneTwentyDay"]),
                            OnetwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OnetwentyDay"]) ? 0 : dr["OnetwentyDay"]),
                        }
                        );
                }

                _ds.lstTable2 = _lstTable2;
            }

            return _ds;
        }
        public DataSet GetARAgingByJobType(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByJobType(objPropContracts, creditFlag);
        }
        public DataSet GetARAgingByJobTypeDetail(Contracts objPropContracts)
        {
            return objDL_Contracts.GetARAgingByJobTypeDetail(objPropContracts);
        }
        public DataSet GetInvoiceByDate(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceByDate(objPropContracts);
        }

        //API
        public List<GetInvoiceByDateViewModel> GetInvoiceByDate(GetInvoiceByDateParam _GetInvoiceByDate, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetInvoiceByDate(_GetInvoiceByDate, ConnectionString);

            List<GetInvoiceByDateViewModel> _lstGetInvoiceByDate = new List<GetInvoiceByDateViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetInvoiceByDate.Add(
                    new GetInvoiceByDateViewModel()
                    {
                        InvDate = Convert.ToDateTime(DBNull.Value.Equals(dr["InvDate"]) ? null : dr["InvDate"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Description = Convert.ToString(dr["Description"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Dep = Convert.ToInt16(DBNull.Value.Equals(dr["Dep"]) ? 0 : dr["Dep"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Location = Convert.ToString(dr["Location"]),
                        ID = Convert.ToString(dr["ID"]),
                        Type = Convert.ToString(dr["Type"]),
                        Age = Convert.ToInt32(DBNull.Value.Equals(dr["Age"]) ? 0 : dr["Age"]),
                        DueDate = Convert.ToDateTime(DBNull.Value.Equals(dr["DueDate"]) ? null : dr["DueDate"]),
                        Retainage = Convert.ToInt32(DBNull.Value.Equals(dr["Retainage"]) ? 0 : dr["Retainage"]),
                        CurrentDay = Convert.ToInt32(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                        ThirtyDay = Convert.ToInt32(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToInt32(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        NintyDay = Convert.ToInt32(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        OverNintyDay = Convert.ToInt32(DBNull.Value.Equals(dr["OverNintyDay"]) ? 0 : dr["OverNintyDay"]),
                    }
                    );
            }

            return _lstGetInvoiceByDate;
        }
        public DataSet GetHistoryPayment(Contracts objPropContracts)
        {
            return objDL_Contracts.GetHistoryPayment(objPropContracts);
        }
        public DataSet GetARAgingByLocType(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByLocType(objPropContracts, creditFlag);
        }

        //API
        public List<GetARAgingByLocTypeViewModel> GetARAgingByLocType(GetARAgingByLocTypeParam _GetARAgingByLocType, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetARAgingByLocType(_GetARAgingByLocType, ConnectionString);

            List<GetARAgingByLocTypeViewModel> _lstGetARAgingByLocType = new List<GetARAgingByLocTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetARAgingByLocType.Add(
                    new GetARAgingByLocTypeViewModel()
                    {
                        LocName = Convert.ToString(dr["LocName"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        OverNintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OverNintyDay"]) ? 0 : dr["OverNintyDay"]),
                        OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                        LocType = Convert.ToString(dr["LocType"]),
                    }
                    );
            }

            return _lstGetARAgingByLocType;
        }
        public DataSet GetARAgingByLocTypeDetail(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByLocTypeDetail(objPropContracts, creditFlag);
        }

        //API
        public List<GetARAgingByLocTypeDetailViewModel> GetARAgingByLocTypeDetail(GetARAgingByLocTypeDetailParam _GetARAgingByLocTypeDetail, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetARAgingByLocTypeDetail(_GetARAgingByLocTypeDetail, ConnectionString);
            List<GetARAgingByLocTypeDetailViewModel> _lstGetARAgingByLocTypeDetail = new List<GetARAgingByLocTypeDetailViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetARAgingByLocTypeDetail.Add(
                    new GetARAgingByLocTypeDetailViewModel()
                    {
                        TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                        Type = Convert.ToString(dr["Type"]),
                        LocType = Convert.ToString(dr["LocType"]),
                        cid = Convert.ToString(dr["cid"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        LocID = Convert.ToString(dr["LocID"]),
                        LocName = Convert.ToString(dr["LocName"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                        Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                        CurrentDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrentDay"]) ? 0 : dr["CurrentDay"]),
                        CurrSevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["CurrSevenDay"]) ? 0 : dr["CurrSevenDay"]),
                        SevenDay = Convert.ToDouble(DBNull.Value.Equals(dr["SevenDay"]) ? 0 : dr["SevenDay"]),
                        ThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDay"]) ? 0 : dr["ThirtyDay"]),
                        SixtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyDay"]) ? 0 : dr["SixtyDay"]),
                        SixtyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["SixtyOneDay"]) ? 0 : dr["SixtyOneDay"]),
                        ZeroThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroThirtyDay"]) ? 0 : dr["ZeroThirtyDay"]),
                        NintyDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyDay"]) ? 0 : dr["NintyDay"]),
                        NintyOneDay = Convert.ToDouble(DBNull.Value.Equals(dr["NintyOneDay"]) ? 0 : dr["NintyOneDay"]),
                        OneTwentyDay = Convert.ToDouble(DBNull.Value.Equals(dr["OneTwentyDay"]) ? 0 : dr["OneTwentyDay"]),
                        Status = Convert.ToString(dr["Status"]),
                    }
                    );
            }

            return _lstGetARAgingByLocTypeDetail;

        }

        //public DataSet GetInvoiceByRef(string config, int id)
        //{
        //    return objDL_Contracts.GetInvoiceByRef(config,id);
        //}
        public DataSet GetInvoiceStatus(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceStatus(objPropContracts);

        }
        public DataSet GetInvoicesPaging(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesPaging(objPropContracts);
        }
        public DataSet GetInvoicesToExport(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoicesToExport(objPropContracts);
        }
        public DataSet GetInvoiceByInvoiceID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetInvoiceByInvoiceID(objPropContracts);
        }

        public int AddInvoice(Contracts objPropContracts)
        {
            return objDL_Contracts.AddInvoice(objPropContracts);
        }
        public void EditInvoice(Contracts objPropContracts)
        {
            objDL_Contracts.EditInvoice(objPropContracts);
        }
        public DataSet GetListRecurringInvoices(Contracts objPropContracts, string GriDCust, string GriDLoc, string GriDLocAcc)
        {
            return objDL_Contracts.GetListRecurringInvoices(objPropContracts, GriDCust, GriDLoc, GriDLocAcc);
        }
        public DataSet AddRecurringInvoices(Contracts objPropContracts, int IncludeContractRemarks = 0)
        {
            return objDL_Contracts.AddRecurringInvoices(objPropContracts, IncludeContractRemarks);
        }

        public void AddRecurringInvoicesUIHistory(Contracts objPropContracts)
        {
            objDL_Contracts.AddRecurringInvoicesUIHistory(objPropContracts);
        }
        public DataSet GetRecurringInvoicesUIHistory(Contracts objPropContracts)
        {
            return objDL_Contracts.GetRecurringInvoicesUIHistory(objPropContracts);
        }
        public DataSet GetRecurringInvoicesLogs(string conn)
        {
            return objDL_Contracts.GetRecurringInvoicesLogs(conn);
        }

        public DataSet GetARAging360ByAsOfDate(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAging360ByAsOfDate(objPropContracts, creditFlag);
        }

        //API
        public ListGetARAging360ByAsOfDate GetARAging360ByAsOfDate(GetARAging360ByAsOfDateParam _GetARAging360ByAsOfDate, string ConnectionString)
        {
            DataSet ds = objDL_Contracts.GetARAging360ByAsOfDate(_GetARAging360ByAsOfDate, ConnectionString);

            ListGetARAging360ByAsOfDate _ds = new ListGetARAging360ByAsOfDate();

            if (_GetARAging360ByAsOfDate.isDBTotalService)
            {
                List<GetARAging360ByAsOfDateTable1> _lstTable1 = new List<GetARAging360ByAsOfDateTable1>();
                List<GetARAging360ByAsOfDateTable2> _lstTable2 = new List<GetARAging360ByAsOfDateTable2>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable1.Add(
                        new GetARAging360ByAsOfDateTable1()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            Type = Convert.ToString(dr["Type"]),
                            LocType = Convert.ToString(dr["LocType"]),
                            cid = Convert.ToString(dr["cid"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            ZeroToThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroToThirtyDay"]) ? 0 : dr["ZeroToThirtyDay"]),
                            ThirtyDayToNinety = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDayToNinety"]) ? 0 : dr["ThirtyDayToNinety"]),
                            NinetyTo360 = Convert.ToDouble(DBNull.Value.Equals(dr["NinetyTo360"]) ? 0 : dr["NinetyTo360"]),
                            Over360 = Convert.ToDouble(DBNull.Value.Equals(dr["Over360"]) ? 0 : dr["Over360"]),
                            Status = Convert.ToString(dr["Status"]),
                            Custom1 = Convert.ToString(dr["Custom1"]),
                        }
                        );
                }
                _ds.lstTable1 = _lstTable1;

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    _lstTable2.Add(
                        new GetARAging360ByAsOfDateTable2()
                        {
                            Column1 = Convert.ToDouble(DBNull.Value.Equals(dr["Column1"]) ? 0 : dr["Column1"]),
                        }
                        );
                }
                _ds.lstTable2 = _lstTable2;
            }
            else
            {
                List<GetARAging360ByAsOfDateTable3> _lstTable3 = new List<GetARAging360ByAsOfDateTable3>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstTable3.Add(
                        new GetARAging360ByAsOfDateTable3()
                        {
                            TransID = Convert.ToInt32(DBNull.Value.Equals(dr["TransID"]) ? 0 : dr["TransID"]),
                            type = Convert.ToInt16(DBNull.Value.Equals(dr["type"]) ? 0 : dr["type"]),
                            cid = Convert.ToString(dr["cid"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            LocID = Convert.ToString(dr["LocID"]),
                            LocName = Convert.ToString(dr["LocName"]),
                            fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                            Due = Convert.ToDateTime(DBNull.Value.Equals(dr["Due"]) ? null : dr["Due"]),
                            Original = Convert.ToDouble(DBNull.Value.Equals(dr["Original"]) ? 0 : dr["Original"]),
                            Total = Convert.ToDouble(DBNull.Value.Equals(dr["Total"]) ? 0 : dr["Total"]),
                            Paid = Convert.ToDouble(DBNull.Value.Equals(dr["Paid"]) ? 0 : dr["Paid"]),
                            fDesc = Convert.ToString(dr["fDesc"]),
                            Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                            DueIn = Convert.ToInt32(DBNull.Value.Equals(dr["DueIn"]) ? 0 : dr["DueIn"]),
                            ZeroToThirtyDay = Convert.ToDouble(DBNull.Value.Equals(dr["ZeroToThirtyDay"]) ? 0 : dr["ZeroToThirtyDay"]),
                            ThirtyDayToNinety = Convert.ToDouble(DBNull.Value.Equals(dr["ThirtyDayToNinety"]) ? 0 : dr["ThirtyDayToNinety"]),
                            NinetyTo360 = Convert.ToDouble(DBNull.Value.Equals(dr["NinetyTo360"]) ? 0 : dr["NinetyTo360"]),
                            Over360 = Convert.ToDouble(DBNull.Value.Equals(dr["Over360"]) ? 0 : dr["Over360"]),
                            Sel = Convert.ToInt16(DBNull.Value.Equals(dr["Sel"]) ? 0 : dr["Sel"]),
                        }
                        );
                }
                _ds.lstTable3 = _lstTable3;
            }

            return _ds;
        }

        public DataSet GetProjectGLCrossReference(Contracts objPropContracts, int dateRangeType)
        {
            try
            {
                return objDL_Contracts.GetProjectGLCrossReference(objPropContracts, dateRangeType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCreditByID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetCreditByID(objPropContracts);
        }

        public DataSet GetARAgingByBusinessTypeOfDate(Contracts objPropContracts, int creditFlag = 0)
        {
            return objDL_Contracts.GetARAgingByBusinessTypeOfDate(objPropContracts,  creditFlag);
        }
        public DataSet GetDepositByID(Contracts objPropContracts)
        {
            return objDL_Contracts.GetDepositByID(objPropContracts);
        }


        public DataSet GetContractsExpireIn10Days(Contracts objPropContracts)
        {
            return objDL_Contracts.GetContractsExpireIn10Days(objPropContracts);
        }
    }
}
