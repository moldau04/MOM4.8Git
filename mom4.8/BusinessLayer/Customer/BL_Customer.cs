using System;
using System.Data;
using DataLayer;
using BusinessEntity;
using System.Collections.Generic;
using BusinessEntity.CustomersModel;
using BusinessEntity.Recurring;

namespace BusinessLayer
{
    public class BL_Customer
    {
        DL_Customer objDL_Customer = new DL_Customer();

        public DataSet getUserAuthorization(Customer objPropCustomer, Int32 UserID, Int32 IsSalesAsigned = 0)
        {
            return objDL_Customer.getCustomers(objPropCustomer, UserID, IsSalesAsigned);
        }

        public DataSet getProspectByID(Customer objPropCustomer)
        {
            return objDL_Customer.getProspectByID(objPropCustomer);
        }

        //API
        public ListGetProspectByID getProspectByID(GetProspectByIDParam _GetProspectByID, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getProspectByID(_GetProspectByID, ConnectionString);
            ListGetProspectByID _ds = new ListGetProspectByID();
            List<GetProspectByIDTable1> _lstTable1 = new List<GetProspectByIDTable1>();
            List<GetProspectByIDTable2> _lstTable2 = new List<GetProspectByIDTable2>();
            List<GetProspectByIDTable3> _lstTable3 = new List<GetProspectByIDTable3>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(new GetProspectByIDTable1()
                {
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    rol = Convert.ToInt32(DBNull.Value.Equals(dr["rol"]) ? 0 : dr["rol"]),
                    status = Convert.ToInt16(DBNull.Value.Equals(dr["status"]) ? 0 : dr["status"]),
                    type = Convert.ToString(dr["type"]),
                    billaddress = Convert.ToString(dr["billaddress"]),
                    billcity = Convert.ToString(dr["billcity"]),
                    billstate = Convert.ToString(dr["billstate"]),
                    billzip = Convert.ToString(dr["billzip"]),
                    billphone = Convert.ToString(dr["billphone"]),
                    CustomerName = Convert.ToString(dr["CustomerName"]),
                    Terr = Convert.ToInt32(DBNull.Value.Equals(dr["Terr"]) ? 0 : dr["Terr"]),
                    Name = Convert.ToString(dr["Name"]),
                    Address = Convert.ToString(dr["Address"]),
                    City = Convert.ToString(dr["City"]),
                    State = Convert.ToString(dr["State"]),
                    Zip = Convert.ToString(dr["Zip"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Cellular = Convert.ToString(dr["Cellular"]),
                    email = Convert.ToString(dr["email"]),
                    Website = Convert.ToString(dr["Website"]),
                    Fax = Convert.ToString(dr["Fax"]),
                    Contact = Convert.ToString(dr["Contact"]),
                    Remarks = Convert.ToString(dr["Remarks"]),
                    lat = Convert.ToString(dr["lat"]),
                    lng = Convert.ToString(dr["lng"]),
                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    CreateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CreateDate"]) ? null : dr["CreateDate"]),
                    LastUpdatedBy = Convert.ToString(dr["LastUpdatedBy"]),
                    LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                    Source = Convert.ToString(dr["Source"]),
                    Country = Convert.ToString(dr["Country"]),
                    billCountry = Convert.ToString(dr["billCountry"]),
                    Referral = Convert.ToString(dr["Referral"]),
                    ReferralType = Convert.ToString(dr["ReferralType"]),
                    BusinessType = Convert.ToString(dr["BusinessType"]),
                    BusinessTypeID = Convert.ToInt32(DBNull.Value.Equals(dr["BusinessTypeID"]) ? 0 : dr["BusinessTypeID"]),
                    EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                    Company = Convert.ToString(dr["Company"]),
                });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(new GetProspectByIDTable2()
                {
                    contactid = Convert.ToInt32(DBNull.Value.Equals(dr["contactid"]) ? 0 : dr["contactid"]),
                    name = Convert.ToString(dr["name"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Fax = Convert.ToString(dr["Fax"]),
                    Cell = Convert.ToString(dr["Cell"]),
                    Email = Convert.ToString(dr["Email"]),
                    Title = Convert.ToString(dr["Title"]),
                    
                });
            }

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable3.Add(new GetProspectByIDTable3()
                {
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    Name = Convert.ToString(dr["Name"]),
                    EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                    Company = Convert.ToString(dr["Company"]),
                    City = Convert.ToString(dr["City"]),
                    State = Convert.ToString(dr["State"]),
                    Zip = Convert.ToString(dr["Zip"]),
                    Address = Convert.ToString(dr["Address"]),
                });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;
            _ds.lstTable3 = _lstTable3;

            return _ds;
        }

        public DataSet getContactList(Customer objPropCustomer)
        {
            return objDL_Customer.getContactList(objPropCustomer);
        }


        public DataSet getProspect(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            return objDL_Customer.getProspect(objPropCustomer, IsSalesAsigned);
        }


        public DataSet getProspectByContact(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            return objDL_Customer.getProspectByContact(objPropCustomer, IsSalesAsigned);
        }


        public DataSet getTasks(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            return objDL_Customer.getTasks(objPropCustomer, IsSalesAsigned);
        }

        public DataSet UpdateTaskToClose(Customer objPropCustomer)
        {
            return objDL_Customer.UpdateTaskToClose(objPropCustomer);
        }

        public DataSet getTasksByID(Customer objPropCustomer)
        {
            return objDL_Customer.getTasksByID(objPropCustomer);
        }

        public DataSet getOpportunityByID(Customer objPropCustomer)
        {
            return objDL_Customer.getOpportunityByID(objPropCustomer);
        }
        public String getCompanyName(Customer objPropCustomer)
        {
            return objDL_Customer.getCompanyName(objPropCustomer);
        }

        public String getLocID(Customer objPropCustomer)
        {
            return objDL_Customer.getLocID(objPropCustomer);
        }



        //public String UpdateEstimateWithLoc(Customer objPropCustomer)
        //{
        //    return objDL_Customer.UpdateEstimateWithLoc(objPropCustomer);
        //}

        public DataSet getRecentProspect(Customer objPropCustomer)
        {
            return objDL_Customer.getRecentProspect(objPropCustomer);
        }

        public DataSet getProspectType(Customer objPropCustomer)
        {
            return objDL_Customer.getProspectType(objPropCustomer);
        }

        public DataSet getStages(Customer objPropCustomer)
        {
            return objDL_Customer.getStages(objPropCustomer);
        }

        public DataSet getRepTemplateName(Customer objPropCustomer)
        {
            return objDL_Customer.getRepTemplateName(objPropCustomer);
        }

        //API
        public List<GetRepTemplateNameViewModel> getRepTemplateName(GetRepTemplateNameParam _GetRepTemplateName, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getRepTemplateName(_GetRepTemplateName, ConnectionString);

            List<GetRepTemplateNameViewModel> _lstGetRepTemplateName = new List<GetRepTemplateNameViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetRepTemplateName.Add(
                    new GetRepTemplateNameViewModel()
                {
                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    fdesc = Convert.ToString(dr["fdesc"]),
                    CBcheckStatus = Convert.ToInt32(DBNull.Value.Equals(dr["CBcheckStatus"]) ? 0 : dr["CBcheckStatus"]),
                });
            }

            return _lstGetRepTemplateName;
        }

        public DataSet getRepTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getRepTemplate(objPropCustomer);
        }

        public DataSet getCustomTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getCustomTemplate(objPropCustomer);
        }

        //API
        public List<GetCustomTemplateViewModel> getCustomTemplate(GetCustomTemplateParam _GetCustomTemplate, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getCustomTemplate(_GetCustomTemplate, ConnectionString);

            List<GetCustomTemplateViewModel> _lstGetCustomTemplate = new List<GetCustomTemplateViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomTemplate.Add(
                    new GetCustomTemplateViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        PrimarySyncID = Convert.ToInt32(DBNull.Value.Equals(dr["PrimarySyncID"]) ? 0 : dr["PrimarySyncID"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                    });
            }

            return _lstGetCustomTemplate;
        }

        public DataSet getTemplateItemByID(Customer objPropCustomer)
        {
            return objDL_Customer.getTemplateItemByID(objPropCustomer);
        }

        //API
        public List<GetTemplateItemByIDViewModel> getTemplateItemByID(GetTemplateItemByIDParam _GetTemplateItemByID, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getTemplateItemByID(_GetTemplateItemByID, ConnectionString);

            List<GetTemplateItemByIDViewModel> _lstGetTemplateItemByID = new List<GetTemplateItemByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetTemplateItemByID.Add(
                    new GetTemplateItemByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        EquipT = Convert.ToInt32(DBNull.Value.Equals(dr["EquipT"]) ? 0 : dr["EquipT"]),
                        Elev = Convert.ToInt32(DBNull.Value.Equals(dr["Elev"]) ? 0 : dr["Elev"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Line = Convert.ToInt32(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Lastdate = Convert.ToDateTime(DBNull.Value.Equals(dr["Lastdate"]) ? null : dr["Lastdate"]),
                        NextDateDue = Convert.ToDateTime(DBNull.Value.Equals(dr["NextDateDue"]) ? null : dr["NextDateDue"]),
                        Frequency = Convert.ToInt32(DBNull.Value.Equals(dr["Frequency"]) ? 0 : dr["Frequency"]),
                        Code = Convert.ToString(dr["Code"]),
                        section = Convert.ToString(dr["section"]),
                        PrimarySyncID = Convert.ToInt32(DBNull.Value.Equals(dr["PrimarySyncID"]) ? 0 : dr["PrimarySyncID"]),
                        Notes = Convert.ToString(dr["Notes"]),
                        LeadEquip = Convert.ToInt16(DBNull.Value.Equals(dr["LeadEquip"]) ? 0 : dr["LeadEquip"]),
                        name = Convert.ToString(dr["name"]),
                    });
            }

            return _lstGetTemplateItemByID;
        }

        public DataSet getCustTemplateItemByID(Customer objPropCustomer)
        {
            return objDL_Customer.getCustTemplateItemByID(objPropCustomer);
        }

        public DataSet GetEquipmentCustTemplateItem(string connConfig, int custTemplate, int equipId, int leadEquipId)
        {
            return objDL_Customer.GetEquipmentCustTemplateItem(connConfig, custTemplate, equipId, leadEquipId);
        }

        public DataSet GetCustomValuesOfEquip(string connConfig, int equipId)
        {
            return objDL_Customer.GetCustomValuesOfEquip(connConfig, equipId);
        }

        //API
        public ListGetCustTemplateItemByID getCustTemplateItemByID(GetCustTemplateItemByIDParam _GetCustTemplateItemByID, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getCustTemplateItemByID(_GetCustTemplateItemByID, ConnectionString);

            ListGetCustTemplateItemByID _ds = new ListGetCustTemplateItemByID();
            List<GetCustTemplateItemByIDTable1> lstTable1 = new List<GetCustTemplateItemByIDTable1>();
            List<GetCustTemplateItemByIDTable2> lstTable2 = new List<GetCustTemplateItemByIDTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstTable1.Add(
                    new GetCustTemplateItemByIDTable1()
                    {
                        orderno = Convert.ToInt32(DBNull.Value.Equals(dr["orderno"]) ? 0 : dr["orderno"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        ElevT = Convert.ToInt32(DBNull.Value.Equals(dr["ElevT"]) ? 0 : dr["ElevT"]),
                        Elev = Convert.ToInt32(DBNull.Value.Equals(dr["Elev"]) ? 0 : dr["Elev"]),
                        CustomID = Convert.ToInt32(DBNull.Value.Equals(dr["CustomID"]) ? 0 : dr["CustomID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Value = Convert.ToString(dr["Value"]),
                        Format = Convert.ToString(dr["Format"]),
                        fExists = Convert.ToInt16(DBNull.Value.Equals(dr["fExists"]) ? 0 : dr["fExists"]),
                        PrimarySyncID = Convert.ToInt32(DBNull.Value.Equals(dr["PrimarySyncID"]) ? 0 : dr["PrimarySyncID"]),
                        LastUpdated = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdated"]) ? null : dr["LastUpdated"]),
                        LastUpdateUser = Convert.ToString(dr["LastUpdateUser"]),
                        OrderNo = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                        LeadEquip = Convert.ToInt16(DBNull.Value.Equals(dr["LeadEquip"]) ? 0 : dr["LeadEquip"]),
                        formatMOM = Convert.ToString(dr["formatMOM"]),
                        name = Convert.ToString(dr["name"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                lstTable2.Add(
                    new GetCustTemplateItemByIDTable2()
                    {
                        ElevT = Convert.ToInt32(DBNull.Value.Equals(dr["ElevT"]) ? 0 : dr["ElevT"]),
                        ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Value = Convert.ToString(dr["Value"]),
                    });
            }

            _ds.lstTable1 = lstTable1;
            _ds.lstTable2 = lstTable2;

            return _ds;
        }

        public DataSet getCustomValues(Customer objPropCustomer)
        {
            return objDL_Customer.getCustomValues(objPropCustomer);
        }
        public void ISshowHomeowner(string ConnConfig, int ISshowHomeowner, int IsLocAddressBlank) { objDL_Customer.ISshowHomeowner(ConnConfig, ISshowHomeowner, IsLocAddressBlank); }
        public DataSet getTemplateItemCodes(Customer objPropCustomer)
        {
            return objDL_Customer.getTemplateItemCodes(objPropCustomer);
        }

        public DataSet getTemplateItemByMultipleID(Customer objPropCustomer, string id)
        {
            return objDL_Customer.getTemplateItemByMultipleID(objPropCustomer, id);
        }

        public DataSet getTemplateItemByElevAndEquipT(Customer objPropCustomer, string EquipId, string Elev)
        {
            return objDL_Customer.getTemplateItemByElevAndEquipT(objPropCustomer, EquipId, Elev);
        }

        public int AddProspect(Customer objPropCustomer)
        {
            return objDL_Customer.AddProspect(objPropCustomer);
        }

        public int AddContactByRol(PhoneModel objPhone)
        {
            return objDL_Customer.AddContactByRol(objPhone);
        }

        public int UpdateContactByID(PhoneModel objPhone)
        {
            return objDL_Customer.UpdateContactByID(objPhone);
        }


        public void UpdateProspect(Customer objPropCustomer)
        {
            objDL_Customer.UpdateProspect(objPropCustomer);
        }

        public void AddProspectContact(Customer objPropCustomer)
        {
            objDL_Customer.AddProspectContact(objPropCustomer);
        }

        public void DeletePhone(Customer objPropCustomer)
        {
            objDL_Customer.DeletePhone(objPropCustomer);
        }

        public void DeleteProspect(Customer objPropCustomer)
        {
            objDL_Customer.DeleteProspect(objPropCustomer);
        }

        public void AddProspectType(Customer objPropCustomer)
        {
            objDL_Customer.AddProspectType(objPropCustomer);
        }

        public void AddSaleSource(Customer objPropCustomer)
        {
            objDL_Customer.AddSaleSource(objPropCustomer);
        }
        public void AddStages(Customer objPropCustomer)
        {
            objDL_Customer.UpdateStages(objPropCustomer);
        }

        public DataSet getLocCoordinates(Customer objPropCustomer)
        {
            return objDL_Customer.getLocCoordinates(objPropCustomer);
        }

        public DataSet GetWorkerCalculations(Customer objPropCustomer)
        {
            return objDL_Customer.GetWorkerCalculations(objPropCustomer);
        }

        public DataSet getWorkerMonthly(Customer objPropCustomer)
        {
            return objDL_Customer.getWorkerMonthly(objPropCustomer);
        }

        public int AddRouteTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.AddRouteTemplate(objPropCustomer);
        }

        public void UpdateLocRoute(Customer objPropCustomer)
        {
            objDL_Customer.UpdateLocRoute(objPropCustomer);
        }

        public DataSet getRouteTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getRouteTemplate(objPropCustomer);
        }

        public DataSet getTemplateByID(Customer objPropCustomer)
        {
            return objDL_Customer.getTemplateByID(objPropCustomer);
        }

        public DataSet getWorkers(Customer objPropCustomer)
        {
            return objDL_Customer.getWorkers(objPropCustomer);
        }

        public void AddTask(Customer objPropCustomer)
        {
            objDL_Customer.AddTask(objPropCustomer);
        }

        public DataSet GetTasks(Customer objPropCustomer)
        {
            return objDL_Customer.GetTasks(objPropCustomer);
        }

        public DataSet getOpportunity(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            return objDL_Customer.getOpportunity(objPropCustomer, IsSalesAsigned);
        }

        public DataSet getOpportunityNew(Customer objPropCustomer, Int32 IsSalesAsigned = 0)
        {
            return objDL_Customer.getOpportunityNew(objPropCustomer, IsSalesAsigned);
        }

        //API
        public List<GetOpportunityNewViewModel> getOpportunityNew(GetOpportunityNewParam _GetOpportunityNew, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds= objDL_Customer.getOpportunityNew(_GetOpportunityNew, ConnectionString, IsSalesAsigned);

            List<GetOpportunityNewViewModel> _lstGetOpportunityNew = new List<GetOpportunityNewViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetOpportunityNew.Add(
                    new GetOpportunityNewViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        RolType = Convert.ToInt16(DBNull.Value.Equals(dr["RolType"]) ? 0 : dr["RolType"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Status = Convert.ToString(dr["Status"]),
                        Probability = Convert.ToString(dr["Probability"]),
                        Product = Convert.ToString(dr["Product"]),
                        Profit = Convert.ToDouble(DBNull.Value.Equals(dr["Profit"]) ? 0 : dr["Profit"]),
                        CreateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CreateDate"]) ? null : dr["CreateDate"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        closedate = Convert.ToDateTime(DBNull.Value.Equals(dr["closedate"]) ? null : dr["closedate"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        closed = Convert.ToString(dr["closed"]),
                        revenue = Convert.ToDouble(DBNull.Value.Equals(dr["revenue"]) ? 0 : dr["revenue"]),
                        fuser = Convert.ToString(dr["fuser"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        defsales = Convert.ToString(dr["defsales"]),
                        DocumentCount = Convert.ToInt32(DBNull.Value.Equals(dr["DocumentCount"]) ? 0 : dr["DocumentCount"]),
                        Estimate = Convert.ToString(dr["Estimate"]),
                        Referral = Convert.ToString(dr["Referral"]),
                        Stage = Convert.ToString(dr["Stage"]),
                        job = Convert.ToString(dr["job"]),
                        fFor = Convert.ToString(dr["fFor"]),
                        EstimateDiscounted = Convert.ToString(dr["EstimateDiscounted"]),
                        BidPrice = Convert.ToDouble(DBNull.Value.Equals(dr["BidPrice"]) ? 0 : dr["BidPrice"]),
                        FinalBid = Convert.ToDouble(DBNull.Value.Equals(dr["FinalBid"]) ? 0 : dr["FinalBid"]),
                        Dept = Convert.ToString(dr["Dept"]),
                    }
                    );
            }

            return _lstGetOpportunityNew;
        }

        public DataSet getOpportunityOfCustomer(Customer objPropCustomer)
        {
            return objDL_Customer.getOpportunityOfCustomer(objPropCustomer);
        }

        //API
        public List<GetOpportunityOfCustomerViewModel> getOpportunityOfCustomer(GetOpportunityOfCustomerParam _GetOpportunityOfCustomer, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getOpportunityOfCustomer(_GetOpportunityOfCustomer, ConnectionString);

            List<GetOpportunityOfCustomerViewModel> _lstGetOpportunityOfCustomer = new List<GetOpportunityOfCustomerViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetOpportunityOfCustomer.Add(
                    new GetOpportunityOfCustomerViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        RolType = Convert.ToInt16(DBNull.Value.Equals(dr["RolType"]) ? 0 : dr["RolType"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Status = Convert.ToString(dr["Status"]),
                        Probability = Convert.ToString(dr["Probability"]),
                        Product = Convert.ToString(dr["Product"]),
                        Profit = Convert.ToDouble(DBNull.Value.Equals(dr["Profit"]) ? 0 : dr["Profit"]),
                        CreateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CreateDate"]) ? null : dr["CreateDate"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        closedate = Convert.ToDateTime(DBNull.Value.Equals(dr["closedate"]) ? null : dr["closedate"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        closed = Convert.ToString(dr["closed"]),
                        revenue = Convert.ToDouble(DBNull.Value.Equals(dr["revenue"]) ? 0 : dr["revenue"]),
                        fuser = Convert.ToString(dr["fuser"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        defsales = Convert.ToString(dr["defsales"]),
                        DocumentCount = Convert.ToInt32(DBNull.Value.Equals(dr["DocumentCount"]) ? 0 : dr["DocumentCount"]),
                        Estimate = Convert.ToString(dr["Estimate"]),
                        Referral = Convert.ToString(dr["Referral"]),
                        Stage = Convert.ToString(dr["Stage"]),
                        job = Convert.ToString(dr["job"]),
                        fFor = Convert.ToString(dr["fFor"]),
                        EstimateDiscounted = Convert.ToString(dr["EstimateDiscounted"]),
                        BidPrice = Convert.ToDouble(DBNull.Value.Equals(dr["BidPrice"]) ? 0 : dr["BidPrice"]),
                        FinalBid = Convert.ToDouble(DBNull.Value.Equals(dr["FinalBid"]) ? 0 : dr["FinalBid"]),
                        Dept = Convert.ToString(dr["Dept"]),
                    }
                    );
            }

            return _lstGetOpportunityOfCustomer;

        }

        public int AddEditOpportunity(Customer objPropCustomer, int? intDept)
        {
            return objDL_Customer.AddEditOpportunity(objPropCustomer, intDept);
        }

        public void DeleteOpportunity(Customer objPropCustomer)
        {
            objDL_Customer.DeleteOpportunity(objPropCustomer);
        }

        //API
        public void DeleteOpportunity(DeleteOpportunityParam _DeleteOpportunity, string ConnectionString)
        {
            objDL_Customer.DeleteOpportunity(_DeleteOpportunity, ConnectionString);
        }
        public void DeleteStages(Customer objPropCustomer)
        {
            objDL_Customer.UpdateStages(objPropCustomer);
        }
        public void DeleteTask(Customer objPropCustomer)
        {
            objDL_Customer.DeleteTask(objPropCustomer);
        }

        public DataSet getContactByRolID(Customer objPropCustomer)
        {
            return objDL_Customer.getContactByRolID(objPropCustomer);
        }

        public DataSet GetRolLocID(Customer objPropCustomer)
        {
            return objDL_Customer.GetRolLocID(objPropCustomer);
        }

        public DataSet GetContactAllByRolID(Customer objPropCustomer)
        {
            return objDL_Customer.GetContactAllByRolID(objPropCustomer);
        }

        public DataSet getSalesDashboard(Customer objPropCustomer)
        {
            return objDL_Customer.getSalesDashboard(objPropCustomer);
        }

        public DataSet getLocationRole(Customer objPropCustomer)
        {
            return objDL_Customer.getLocationRole(objPropCustomer);
        }

        public DataSet getLocationByRoleID(Customer objPropCustomer)
        {
            return objDL_Customer.getLocationByRoleID(objPropCustomer);
        }

        public int AddLocationRole(Customer objPropCustomer)
        {
            return objDL_Customer.AddLocationRole(objPropCustomer);
        }

        public void UpdateLocationRole(Customer objPropCustomer)
        {
            objDL_Customer.UpdateLocationRole(objPropCustomer);
        }

        public void DeleteLocationRole(Customer objPropCustomer)
        {
            objDL_Customer.DeleteLocationRole(objPropCustomer);
        }

        public DataSet GetEstimateLabor(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimateLabor(objPropCustomer);
        }

        public DataSet GetEstimateLaborForEstimate(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimateLaborForEstimate(objPropCustomer);
        }

        public void AddEstimateTemplate(Customer objPropCustomer)
        {
            objDL_Customer.AddEstimateTemplate(objPropCustomer);
        }

        public DataSet getEstimateTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateTemplate(objPropCustomer);
        }
        public DataSet GetEstimates(Customer objPropCustomer, Int32 isSalesAsigned = 0, List<RetainFilter> filters = null, bool isEmailProposalsFilter = false)
        {
            return objDL_Customer.GetEstimates(objPropCustomer, filters, isSalesAsigned, isEmailProposalsFilter);
        }
        //saleperson
        public DataSet GetSalePersonByJob(Customer objPropCustomer)
        {
            return objDL_Customer.GetSalePersonByJob(objPropCustomer);
        }

        public DataSet getWeeklySaleReportQuoted(Customer objPropCustomer)
        {
            return objDL_Customer.getWeeklySaleReportQuoted(objPropCustomer);
        }
        public DataSet getJobProjectByJobID(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProjectByJobID(objPropCustomer);
        }

        public DataSet GetJobRatebyID(Customer objPropCustomer)
        {
            return objDL_Customer.GetJobRatebyID(objPropCustomer);
        }
        public DataSet GetjobcodeInfo(Customer objPropCustomer)
        {
            return objDL_Customer.GetjobcodeInfo(objPropCustomer);
        }
        public DataSet getJobProject_BOM(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProject_BOM(objPropCustomer);
        }
        public DataSet getJobProject_Team(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProject_Team(objPropCustomer);
        }
        public DataSet getJobProject_Milestone(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProject_Milestone(objPropCustomer);
        }
        public DataSet GetCustomReport(Customer objPropCustomer)
        {
            return objDL_Customer.GetCustomReport(objPropCustomer);
        }
        public DataSet getJobTemplateByID(Customer objPropCustomer)
        {
            return objDL_Customer.getJobTemplateByID(objPropCustomer);
        }
        public DataSet getJobProject(Customer objPropCustomer, DataTable filtersData, Int32 IsSalesAsigned = 0, int IncludeClose = 1, string Size = "0", string Page="0" , string OrderBY="0")
        {
            return objDL_Customer.getJobProject(objPropCustomer, filtersData, IsSalesAsigned, IncludeClose, Size, Page, OrderBY);
        }

        public DataSet getJobProjectReport(Customer objPropCustomer, DataTable filtersData, Int32 IsSalesAsigned = 0, int IncludeClose = 1, string Size = "0", string Page = "0", string OrderBY = "0")
        {
            return objDL_Customer.getJobProjectReport(objPropCustomer, filtersData, IsSalesAsigned, IncludeClose, Size, Page, OrderBY);
        }

        public DataSet GetJobProjectWIP(Customer objPropCustomer, int includeClose, bool isPeriodPost = false)
        {
            return objDL_Customer.GetJobProjectWIP(objPropCustomer, includeClose, isPeriodPost);
        }

        public DataSet GetJobProjectWIPSummary(Customer objPropCustomer, int includeClose)
        {
            return objDL_Customer.GetJobProjectWIPSummary(objPropCustomer, includeClose);
        }

        //API
        public List<GetJobProjectViewModel> getJobProject(GetJobProjectParam _GetJobProject, string ConnectionString, Int32 IsSalesAsigned = 0, int IncludeClose = 1)
        {
            DataSet ds =  objDL_Customer.getJobProject(_GetJobProject, ConnectionString, IsSalesAsigned, IncludeClose);
            List<GetJobProjectViewModel> _lstGetJobProject = new List<GetJobProjectViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetJobProject.Add(
                    new GetJobProjectViewModel()
                    {
                        
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        LID = Convert.ToString(dr["LID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Type = Convert.ToString(dr["Type"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        LocationType = Convert.ToString(dr["LocationType"]),
                        BuildingType = Convert.ToString(dr["BuildingType"]),
                        tag = Convert.ToString(dr["tag"]),
                        Address = Convert.ToString(dr["Address"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        Elev = Convert.ToInt32(DBNull.Value.Equals(dr["Elev"]) ? 0 : dr["Elev"]),
                        Status = Convert.ToString(dr["Status"]),
                        PO = Convert.ToString(dr["PO"]),
                        Rev = Convert.ToDouble(DBNull.Value.Equals(dr["Rev"]) ? 0 : dr["Rev"]),
                        Mat = Convert.ToDouble(DBNull.Value.Equals(dr["Mat"]) ? 0 : dr["Mat"]),
                        OtherExp = Convert.ToDouble(DBNull.Value.Equals(dr["OtherExp"]) ? 0 : dr["OtherExp"]),
                        Labor = Convert.ToDouble(DBNull.Value.Equals(dr["Labor"]) ? 0 : dr["Labor"]),
                        Cost = Convert.ToDouble(DBNull.Value.Equals(dr["Cost"]) ? 0 : dr["Cost"]),
                        Profit = Convert.ToDouble(DBNull.Value.Equals(dr["Profit"]) ? 0 : dr["Profit"]),
                        Ratio = Convert.ToDouble(DBNull.Value.Equals(dr["Ratio"]) ? 0 : dr["Ratio"]),
                        Reg = Convert.ToDouble(DBNull.Value.Equals(dr["Reg"]) ? 0 : dr["Reg"]),
                        OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                        DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                        TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
                        Hour = Convert.ToDouble(DBNull.Value.Equals(dr["Hour"]) ? 0 : dr["Hour"]),
                        BRev = Convert.ToDouble(DBNull.Value.Equals(dr["BRev"]) ? 0 : dr["BRev"]),
                        BMat = Convert.ToDouble(DBNull.Value.Equals(dr["BMat"]) ? 0 : dr["BMat"]),
                        BLabor = Convert.ToDouble(DBNull.Value.Equals(dr["BLabor"]) ? 0 : dr["BLabor"]),
                        BCost = Convert.ToDouble(DBNull.Value.Equals(dr["BCost"]) ? 0 : dr["BCost"]),
                        BProfit = Convert.ToDouble(DBNull.Value.Equals(dr["BProfit"]) ? 0 : dr["BProfit"]),
                        BRatio = Convert.ToDouble(DBNull.Value.Equals(dr["BRatio"]) ? 0 : dr["BRatio"]),
                        BHour = Convert.ToDouble(DBNull.Value.Equals(dr["BHour"]) ? 0 : dr["BHour"]),
                        Template = Convert.ToInt32(DBNull.Value.Equals(dr["Template"]) ? 0 : dr["Template"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        CloseDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CloseDate"]) ? null : dr["CloseDate"]),
                        Comm = Convert.ToDouble(DBNull.Value.Equals(dr["Comm"]) ? 0 : dr["Comm"]),
                        WageC = Convert.ToInt32(DBNull.Value.Equals(dr["WageC"]) ? 0 : dr["WageC"]),
                        NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                        Post = Convert.ToInt16(DBNull.Value.Equals(dr["Post"]) ? 0 : dr["Post"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Certified = Convert.ToInt16(DBNull.Value.Equals(dr["Certified"]) ? 0 : dr["Certified"]),
                        Apprentice = Convert.ToInt16(DBNull.Value.Equals(dr["Apprentice"]) ? 0 : dr["Apprentice"]),
                        UseCat = Convert.ToInt16(DBNull.Value.Equals(dr["UseCat"]) ? 0 : dr["UseCat"]),
                        UseDed = Convert.ToInt16(DBNull.Value.Equals(dr["UseDed"]) ? 0 : dr["UseDed"]),
                        BillRate = Convert.ToDouble(DBNull.Value.Equals(dr["BillRate"]) ? 0 : dr["BillRate"]),
                        Markup = Convert.ToDouble(DBNull.Value.Equals(dr["Markup"]) ? 0 : dr["Markup"]),
                        PType = Convert.ToInt16(DBNull.Value.Equals(dr["PType"]) ? 0 : dr["PType"]),
                        Charge = Convert.ToInt16(DBNull.Value.Equals(dr["Charge"]) ? 0 : dr["Charge"]),
                        Amount = Convert.ToInt32(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        GL = Convert.ToInt32(DBNull.Value.Equals(dr["GL"]) ? 0 : dr["GL"]),
                        GLRev = Convert.ToInt32(DBNull.Value.Equals(dr["GLRev"]) ? 0 : dr["GLRev"]),
                        GandA = Convert.ToDouble(DBNull.Value.Equals(dr["GandA"]) ? 0 : dr["GandA"]),
                        OHLabor = Convert.ToDouble(DBNull.Value.Equals(dr["OHLabor"]) ? 0 : dr["OHLabor"]),
                        LastOH = Convert.ToDouble(DBNull.Value.Equals(dr["LastOH"]) ? 0 : dr["LastOH"]),
                        etc = Convert.ToDouble(DBNull.Value.Equals(dr["etc"]) ? 0 : dr["etc"]),
                        ETCModifier = Convert.ToDouble(DBNull.Value.Equals(dr["ETCModifier"]) ? 0 : dr["ETCModifier"]),
                        FP = Convert.ToString(dr["FP"]),
                        fGroup = Convert.ToString(dr["fGroup"]),
                        CType = Convert.ToString(dr["CType"]),
                        Elevs = Convert.ToInt32(DBNull.Value.Equals(dr["Elevs"]) ? 0 : dr["Elevs"]),
                        RateTravel = Convert.ToDouble(DBNull.Value.Equals(dr["RateTravel"]) ? 0 : dr["RateTravel"]),
                        RateOT = Convert.ToDouble(DBNull.Value.Equals(dr["RateOT"]) ? 0 : dr["RateOT"]),
                        RateNT = Convert.ToDouble(DBNull.Value.Equals(dr["RateNT"]) ? 0 : dr["RateNT"]),
                        RateDT = Convert.ToDouble(DBNull.Value.Equals(dr["RateDT"]) ? 0 : dr["RateDT"]),
                        RateMileage = Convert.ToDouble(DBNull.Value.Equals(dr["RateMileage"]) ? 0 : dr["RateMileage"]),
                        NType = Convert.ToInt16(DBNull.Value.Equals(dr["NType"]) ? 0 : dr["NType"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        Custom3 = Convert.ToString(dr["Custom3"]),
                        Custom4 = Convert.ToString(dr["Custom4"]),
                        Custom5 = Convert.ToString(dr["Custom5"]),
                        Custom6 = Convert.ToString(dr["Custom6"]),
                        StartDate = Convert.ToDateTime(DBNull.Value.Equals(dr["StartDate"]) ? null : dr["StartDate"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        TemplateDesc = Convert.ToString(dr["TemplateDesc"]),
                        Salesperson = Convert.ToString(dr["Salesperson"]),
                        Route = Convert.ToString(dr["Route"]),
                        NHour = Convert.ToDouble(DBNull.Value.Equals(dr["NHour"]) ? 0 : dr["NHour"]),
                        NLabor = Convert.ToDouble(DBNull.Value.Equals(dr["NLabor"]) ? 0 : dr["NLabor"]),
                        NMat = Convert.ToDouble(DBNull.Value.Equals(dr["NMat"]) ? 0 : dr["NMat"]),
                        NOtherExp = Convert.ToDouble(DBNull.Value.Equals(dr["NOtherExp"]) ? 0 : dr["NOtherExp"]),
                        NTicketOtherExp = Convert.ToDouble(DBNull.Value.Equals(dr["NTicketOtherExp"]) ? 0 : dr["NTicketOtherExp"]),
                        NCost = Convert.ToDouble(DBNull.Value.Equals(dr["NCost"]) ? 0 : dr["NCost"]),
                        NRev = Convert.ToDouble(DBNull.Value.Equals(dr["NRev"]) ? 0 : dr["NRev"]),
                        NotBilledYet = Convert.ToDouble(DBNull.Value.Equals(dr["NotBilledYet"]) ? 0 : dr["NotBilledYet"]),
                        NComm = Convert.ToDouble(DBNull.Value.Equals(dr["NComm"]) ? 0 : dr["NComm"]),
                        ReceivePO = Convert.ToDouble(DBNull.Value.Equals(dr["ReceivePO"]) ? 0 : dr["ReceivePO"]),
                        NProfit = Convert.ToDouble(DBNull.Value.Equals(dr["NProfit"]) ? 0 : dr["NProfit"]),
                        NRatio = Convert.ToDouble(DBNull.Value.Equals(dr["NRatio"]) ? 0 : dr["NRatio"]),
                        Bill = Convert.ToInt32(DBNull.Value.Equals(dr["Bill"]) ? 0 : dr["Bill"]),
                        BillPercent = Convert.ToInt32(DBNull.Value.Equals(dr["BillPercent"]) ? 0 : dr["BillPercent"]),
                        Url = Convert.ToString(dr["Url"]),
                        ContractPrice = Convert.ToDouble(DBNull.Value.Equals(dr["ContractPrice"]) ? 0 : dr["ContractPrice"]),
                        TotalBudgetedExpense = Convert.ToDouble(DBNull.Value.Equals(dr["TotalBudgetedExpense"]) ? 0 : dr["TotalBudgetedExpense"]),
                        ProjectManagerUserID = Convert.ToInt32(DBNull.Value.Equals(dr["ProjectManagerUserID"]) ? 0 : dr["ProjectManagerUserID"]),
                        AssignedProjectUserID = Convert.ToInt32(DBNull.Value.Equals(dr["AssignedProjectUserID"]) ? 0 : dr["AssignedProjectUserID"]),
                        ProjectManagerUserName = Convert.ToString(dr["ProjectManagerUserName"]),
                        Nomat = Convert.ToDouble(DBNull.Value.Equals(dr["Nomat"]) ? 0 : dr["Nomat"]),
                    }
                    );
            }

            return _lstGetJobProject;
        }
        public DataSet getJobProjectTemplate(Customer objPropCustomer)
        {
            return objDL_Customer.getJobProjectTemplate(objPropCustomer);
        }
        public DataSet getJobProjectTemp(Customer objPropCustomer , int Job = 0)
        {
            return objDL_Customer.getJobProjectTemp(objPropCustomer, Job);
        }
        public DataSet getWage(Customer objPropCustomer)
        {
            return objDL_Customer.getWage(objPropCustomer);
        }
        public DataSet getEstimateTemplateByID(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateTemplateByID(objPropCustomer);
        }
        public DataSet getEstimateBucket(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateBucket(objPropCustomer);
        }
        public DataSet getEstimateBucketItems(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateBucketItems(objPropCustomer);
        }
        public void AddEstimateBucket(Customer objPropCustomer)
        {
            objDL_Customer.AddEstimateBucket(objPropCustomer);
        }
        public DataSet getEstimateBucketByID(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateBucketByID(objPropCustomer);
        }
        public DataSet getJobEstimate(Customer objPropCustomer)
        {
            return objDL_Customer.getJobEstimate(objPropCustomer);
        }
        public void AddEstimateLabor(Customer objPropCustomer)
        {
            objDL_Customer.AddEstimateLabor(objPropCustomer);
        }
        //public void AddEstimate(Customer objPropCustomer)
        //{
        //    objDL_Customer.AddEstimate(objPropCustomer);
        //}
        public DataSet GetCustomerBomT(Customer objPropCustomer)
        {
            return objDL_Customer.GetBomt(objPropCustomer);
        }
        public void AddCustomBomValues(Customer objPropCustomer)
        {
            objDL_Customer.AddCustomBomT(objPropCustomer);
        }
        public int AddProject(Customer objPropCustomer, string groupIds)
        {
            return objDL_Customer.AddProject(objPropCustomer, groupIds);
        }
        public void UpdateOrderNoProject(Customer objPropCustomer)
        {
            objDL_Customer.UpdateOrderNoProject(objPropCustomer);
        }

        public int AddProjectTemplate(JobT _objJob)
        {
            return objDL_Customer.AddProjectTemplate(_objJob);
        }
        public String ConvertEstimateToProject(Customer objPropCustomer)
        {
            return objDL_Customer.ConvertEstimateToProject(objPropCustomer);
        }
        public void UpdateEstimateToProject(String ConnConfig, Int32 JobID, Decimal BRev, Decimal BLabour, Decimal BMat, Decimal BOther, Decimal BCost, Decimal BProfit, Decimal BRatio, Decimal BHour)
        {
            objDL_Customer.UpdateEstimateToProject(ConnConfig, JobID, BRev, BLabour, BMat, BOther, BCost, BProfit, BRatio, BHour);
        }
        public DataSet getAllCustomers(Loc objLoc)
        {
            return objDL_Customer.getAllCustomers(objLoc);
        }

        public void DeleteProject(Customer objPropCustomer)
        {
            objDL_Customer.DeleteProject(objPropCustomer);
        }
        public void UpdateCustomerBalance(Owner _objOwner)
        {
            objDL_Customer.UpdateCustomerBalance(_objOwner);
        }
        public DataSet GetOwnerByID(Owner _objOwner)
        {
            return objDL_Customer.GetOwnerByID(_objOwner);
        }
        public DataSet GetOwnerByLoc(Owner _objOwner)
        {
            return objDL_Customer.GetOwnerByLoc(_objOwner);
        }
        public DataSet getAllLocationOnCustomer(Loc objLoc, int _ownerId)
        {
            return objDL_Customer.getAllLocationOnCustomer(objLoc, _ownerId);
        }

        //API
        public List<GetAllLocationOnCustomerViewModel> getAllLocationOnCustomer(GetAllLocationOnCustomerParam _GetAllLocationOnCustomer, int _ownerId, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getAllLocationOnCustomer(_GetAllLocationOnCustomer, _ownerId, ConnectionString);

            List<GetAllLocationOnCustomerViewModel> _lstGetAllLocationOnCustomer = new List<GetAllLocationOnCustomerViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllLocationOnCustomer.Add(
                    new GetAllLocationOnCustomerViewModel()
                    {
                        Tag = Convert.ToString(dr["Tag"]),
                        ID = Convert.ToString(dr["ID"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                    }
                    );
            }

            return _lstGetAllLocationOnCustomer;
        }
        public void DeleteProjectTemplate(JobT _objJob)
        {
            objDL_Customer.DeleteProjectTemplate(_objJob);
        }
        public double GetCustomerBalanceByID(Customer objPropCustomer)
        {
            return objDL_Customer.GetCustomerBalanceByID(objPropCustomer);
        }
        public double GetLocBalanceByID(Customer objPropCustomer)
        {
            return objDL_Customer.GetLocBalanceByID(objPropCustomer);
        }
        public int UpdateProjectTemplate(JobT _objJob)
        {
            return objDL_Customer.UpdateProjectTemplate(_objJob);
        }
        public void UpdateTemplateStatus(JobT _objJob)
        {
            objDL_Customer.UpdateTemplateStatus(_objJob);
        }
        public DataSet getProspectIDbyEstimateID(Customer objPropCustomer)
        {
            return objDL_Customer.getProspectIDbyEstimateID(objPropCustomer);
        }
        public DataSet getEstimateAgreement(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateAgreement(objPropCustomer);
        }
        public int AddEstimate(Customer objPropCustomer)
        {
            return objDL_Customer.AddEstimate(objPropCustomer);
        }
        public void UpdateEstimate(Customer objPropCustomer)
        {
            objDL_Customer.UpdateEstimate(objPropCustomer);
        }

        public void DeleteEstimate(Customer objPropCustomer)
        {
            objDL_Customer.DeleteEstimate(objPropCustomer);
        }

        public DataSet getJobTasks(Customer objPropCustomer)
        {
            return objDL_Customer.getJobTasks(objPropCustomer);
        }

        public string getJobTasksCategory(Customer objPropCustomer)
        {
            return objDL_Customer.getJobTasksCategory(objPropCustomer);
        }

        public DataSet GetWorker(Customer objPropCustomer)
        {
            return objDL_Customer.GetWorker(objPropCustomer);
        }
        public DataSet GetEstimateByID(Customer objCustomer)
        {
            return objDL_Customer.GetEstimateByID(objCustomer);
        }

        //API
        public ListGetEstimateByID GetEstimateByID(GetEstimateByIDParam _GetEstimateByID, string ConnectionString)
        {
            DataSet ds = objDL_Customer.GetEstimateByID(_GetEstimateByID, ConnectionString);

            ListGetEstimateByID _ds = new ListGetEstimateByID();
            List<GetEstimateByIDTable1> _lstTable1 = new List<GetEstimateByIDTable1>();
            List<GetEstimateByIDTable2> _lstTable2 = new List<GetEstimateByIDTable2>();
            List<GetEstimateByIDTable3> _lstTable3 = new List<GetEstimateByIDTable3>();
            List<GetEstimateByIDTable4> _lstTable4 = new List<GetEstimateByIDTable4>();
            List<GetEstimateByIDTable5> _lstTable5 = new List<GetEstimateByIDTable5>();
            List<GetEstimateByIDTable6> _lstTable6 = new List<GetEstimateByIDTable6>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetEstimateByIDTable1()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        rolid = Convert.ToInt32(DBNull.Value.Equals(dr["rolid"]) ? 0 : dr["rolid"]),
                        locid = Convert.ToInt32(DBNull.Value.Equals(dr["locid"]) ? 0 : dr["locid"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Category = Convert.ToString(dr["Category"]),
                        Opportunity = Convert.ToInt32(DBNull.Value.Equals(dr["Opportunity"]) ? 0 : dr["Opportunity"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        cadexchange = Convert.ToDouble(DBNull.Value.Equals(dr["cadexchange"]) ? 0 : dr["cadexchange"]),
                        status = Convert.ToInt16(DBNull.Value.Equals(dr["status"]) ? 0 : dr["status"]),
                        job = Convert.ToInt32(DBNull.Value.Equals(dr["job"]) ? 0 : dr["job"]),
                        Template = Convert.ToInt32(DBNull.Value.Equals(dr["Template"]) ? 0 : dr["Template"]),
                        EstimateBillAddress = Convert.ToString(dr["EstimateBillAddress"]),
                        BDate = Convert.ToDateTime(DBNull.Value.Equals(dr["BDate"]) ? null : dr["BDate"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        EstimateUserId = Convert.ToInt32(DBNull.Value.Equals(dr["EstimateUserId"]) ? 0 : dr["EstimateUserId"]),
                        EstimateAddress = Convert.ToString(dr["EstimateAddress"]),
                        EstimateEmail = Convert.ToString(dr["EstimateEmail"]),
                        EstimateCell = Convert.ToString(dr["EstimateCell"]),
                        JobType = Convert.ToString(dr["JobType"]),
                        Cont = Convert.ToDouble(DBNull.Value.Equals(dr["Cont"]) ? 0 : dr["Cont"]),
                        BidPrice = Convert.ToDouble(DBNull.Value.Equals(dr["BidPrice"]) ? 0 : dr["BidPrice"]),
                        FinalBid = Convert.ToDouble(DBNull.Value.Equals(dr["FinalBid"]) ? 0 : dr["FinalBid"]),
                        OH = Convert.ToDouble(DBNull.Value.Equals(dr["OH"]) ? 0 : dr["OH"]),
                        OHPer = Convert.ToDouble(DBNull.Value.Equals(dr["OHPer"]) ? 0 : dr["OHPer"]),
                        MarkupPer = Convert.ToDouble(DBNull.Value.Equals(dr["MarkupPer"]) ? 0 : dr["MarkupPer"]),
                        MarkupVal = Convert.ToDouble(DBNull.Value.Equals(dr["MarkupVal"]) ? 0 : dr["MarkupVal"]),
                        CommissionPer = Convert.ToDouble(DBNull.Value.Equals(dr["CommissionPer"]) ? 0 : dr["CommissionPer"]),
                        CommissionVal = Convert.ToDouble(DBNull.Value.Equals(dr["CommissionVal"]) ? 0 : dr["CommissionVal"]),
                        STax = Convert.ToString(dr["STax"]),
                        STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        ContPer = Convert.ToDouble(DBNull.Value.Equals(dr["ContPer"]) ? 0 : dr["ContPer"]),
                        PType = Convert.ToInt16(DBNull.Value.Equals(dr["PType"]) ? 0 : dr["PType"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        BillRate = Convert.ToDouble(DBNull.Value.Equals(dr["BillRate"]) ? 0 : dr["BillRate"]),
                        OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                        RateTravel = Convert.ToDouble(DBNull.Value.Equals(dr["RateTravel"]) ? 0 : dr["RateTravel"]),
                        DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                        RateMileage = Convert.ToDouble(DBNull.Value.Equals(dr["RateMileage"]) ? 0 : dr["RateMileage"]),
                        RateNT = Convert.ToDouble(DBNull.Value.Equals(dr["RateNT"]) ? 0 : dr["RateNT"]),
                        ffor = Convert.ToString(dr["ffor"]),
                        EstimateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EstimateDate"]) ? null : dr["EstimateDate"]),
                        Discounted = Convert.ToBoolean(DBNull.Value.Equals(dr["Discounted"]) ? false : dr["Discounted"]),
                        DiscountedNotes = Convert.ToString(dr["DiscountedNotes"]),
                        ProspectID = Convert.ToInt32(DBNull.Value.Equals(dr["ProspectID"]) ? 0 : dr["ProspectID"]),
                        GroupName = Convert.ToString(dr["GroupName"]),
                        GroupId = Convert.ToInt32(DBNull.Value.Equals(dr["GroupId"]) ? 0 : dr["GroupId"]),
                        EstimateType = Convert.ToString(dr["EstimateType"]),
                        IsSglBilAmt = Convert.ToBoolean(DBNull.Value.Equals(dr["IsSglBilAmt"]) ? false : dr["IsSglBilAmt"]),
                    }
                    );
            }
           

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetEstimateByIDTable2()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Line = Convert.ToInt32(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        TemplateID = Convert.ToInt32(DBNull.Value.Equals(dr["TemplateID"]) ? 0 : dr["TemplateID"]),
                        LabourID = Convert.ToInt32(DBNull.Value.Equals(dr["LabourID"]) ? 0 : dr["LabourID"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                    }
                    );
            }
            

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable3.Add(
                    new GetEstimateByIDTable3()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        status = Convert.ToString(dr["status"]),
                        jStatus = Convert.ToInt16(DBNull.Value.Equals(dr["jStatus"]) ? 0 : dr["jStatus"]),
                        TemplateRev = Convert.ToString(dr["TemplateRev"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                    }
                    );
            }
           

            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable4.Add(
                    new GetEstimateByIDTable4()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        JCode = Convert.ToString(dr["JCode"]),
                        CodeDesc = Convert.ToString(dr["CodeDesc"]),
                        Line = Convert.ToInt32(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        JType = Convert.ToInt16(DBNull.Value.Equals(dr["JType"]) ? 0 : dr["JType"]),
                        MilesName = Convert.ToString(dr["MilesName"]),
                        RequiredBy = Convert.ToDateTime(DBNull.Value.Equals(dr["RequiredBy"]) ? null : dr["RequiredBy"]),
                        ActAcquistDate = Convert.ToDateTime(DBNull.Value.Equals(dr["ActAcquistDate"]) ? null : dr["ActAcquistDate"]),
                        Comments = Convert.ToString(dr["Comments"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Department = Convert.ToString(dr["Department"]),
                        Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                        Line1 = Convert.ToInt32(DBNull.Value.Equals(dr["Line1"]) ? 0 : dr["Line1"]),
                        EstimateItemID = Convert.ToInt32(DBNull.Value.Equals(dr["EstimateItemID"]) ? 0 : dr["EstimateItemID"]),
                        AmountPer = Convert.ToString(dr["AmountPer"]),
                        OrderNo = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                        Quantity = Convert.ToDouble(DBNull.Value.Equals(dr["Quantity"]) ? 0 : dr["Quantity"]),
                        Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                    }
                    );
            }
            

            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable5.Add(
                    new GetEstimateByIDTable5()
                    {
                        JobT = Convert.ToInt32(DBNull.Value.Equals(dr["JobT"]) ? 0 : dr["JobT"]),
                        Job = Convert.ToInt32(DBNull.Value.Equals(dr["Job"]) ? 0 : dr["Job"]),
                        JobTItemID = Convert.ToInt32(DBNull.Value.Equals(dr["JobTItemID"]) ? 0 : dr["JobTItemID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Code = Convert.ToString(dr["Code"]),
                        CodeDesc = Convert.ToString(dr["CodeDesc"]),
                        Line = Convert.ToInt32(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        BType = Convert.ToInt16(DBNull.Value.Equals(dr["BType"]) ? 0 : dr["BType"]),
                        QtyReq = Convert.ToDouble(DBNull.Value.Equals(dr["QtyReq"]) ? 0 : dr["QtyReq"]),
                        UM = Convert.ToString(dr["UM"]),
                        BudgetUnit = Convert.ToDouble(DBNull.Value.Equals(dr["BudgetUnit"]) ? 0 : dr["BudgetUnit"]),
                        BudgetExt = Convert.ToDouble(DBNull.Value.Equals(dr["BudgetExt"]) ? 0 : dr["BudgetExt"]),
                        MatItem = Convert.ToInt32(DBNull.Value.Equals(dr["MatItem"]) ? 0 : dr["MatItem"]),
                        MatMod = Convert.ToDouble(DBNull.Value.Equals(dr["MatMod"]) ? 0 : dr["MatMod"]),
                        MatPrice = Convert.ToDouble(DBNull.Value.Equals(dr["MatPrice"]) ? 0 : dr["MatPrice"]),
                        MatMarkup = Convert.ToDouble(DBNull.Value.Equals(dr["MatMarkup"]) ? 0 : dr["MatMarkup"]),
                        STax = Convert.ToInt16(DBNull.Value.Equals(dr["STax"]) ? 0 : dr["STax"]),
                        Currency = Convert.ToString(dr["Currency"]),
                        LabItem = Convert.ToInt32(DBNull.Value.Equals(dr["LabItem"]) ? 0 : dr["LabItem"]),
                        MatName = Convert.ToString(dr["MatName"]),
                        LabMod = Convert.ToDouble(DBNull.Value.Equals(dr["LabMod"]) ? 0 : dr["LabMod"]),
                        LabExt = Convert.ToDouble(DBNull.Value.Equals(dr["LabExt"]) ? 0 : dr["LabExt"]),
                        LabRate = Convert.ToDouble(DBNull.Value.Equals(dr["LabRate"]) ? 0 : dr["LabRate"]),
                        LabHours = Convert.ToDouble(DBNull.Value.Equals(dr["LabHours"]) ? 0 : dr["LabHours"]),
                        SDate = Convert.ToDateTime(DBNull.Value.Equals(dr["SDate"]) ? null : dr["SDate"]),
                        VendorId = Convert.ToString(dr["VendorId"]),
                        Vendor = Convert.ToString(dr["Vendor"]),
                        TotalExt = Convert.ToDouble(DBNull.Value.Equals(dr["TotalExt"]) ? 0 : dr["TotalExt"]),
                        LabPrice = Convert.ToDouble(DBNull.Value.Equals(dr["LabPrice"]) ? 0 : dr["LabPrice"]),
                        LabMarkup = Convert.ToDouble(DBNull.Value.Equals(dr["LabMarkup"]) ? 0 : dr["LabMarkup"]),
                        LSTax = Convert.ToInt16(DBNull.Value.Equals(dr["LSTax"]) ? 0 : dr["LSTax"]),
                        EstimateItemID = Convert.ToInt32(DBNull.Value.Equals(dr["EstimateItemID"]) ? 0 : dr["EstimateItemID"]),
                        OrderNo = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                    }
                    );
            }
           

            foreach (DataRow dr in ds.Tables[5].Rows)
            {
                _lstTable6.Add(
                    new GetEstimateByIDTable6()
                    {
                        AssignTo = Convert.ToString(dr["AssignTo"]),
                        Address = Convert.ToString(dr["Address"]),
                        QuotedPrice = Convert.ToDouble(DBNull.Value.Equals(dr["QuotedPrice"]) ? 0 : dr["QuotedPrice"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;
            _ds.lstTable3 = _lstTable3;
            _ds.lstTable4 = _lstTable4;
            _ds.lstTable5 = _lstTable5;
            _ds.lstTable6 = _lstTable6;

            return _ds;

        }

        public DataSet GetAllEstimate(Customer objCustomer, string estimateIDs)
        {
            return objDL_Customer.GetAllEstimate(objCustomer, estimateIDs);
        }

        public DataSet GetEstimateBOM(Customer objCustomer)
        {
            return objDL_Customer.GetEstimateBOM(objCustomer);
        }

        public DataSet GetEstimateMilestone(Customer objCustomer)
        {
            return objDL_Customer.GetEstimateMilestone(objCustomer);
        }
        public DataSet GetEstimateOpportunityByEstimateID(Customer objCustomer)
        {
            return objDL_Customer.GetEstimateOpportunityByEstimateID(objCustomer);
        }
        public DataSet getBT(Customer objPropCustomer)
        {
            return objDL_Customer.getBT(objPropCustomer);
        }

        public DataSet getReportName(Customer objPropCustomer)
        {
            return objDL_Customer.getReportName(objPropCustomer);
        }

        public bool chkInvoiceOnlinePaymentPermission(Customer objPropCustomer)
        {
            return objDL_Customer.chkInvoiceOnlinePaymentPermission(objPropCustomer);
        }

        //API
        public List<GetBTViewModel> getBT(GetBTParam _GetBT, string ConnectionString)
        {
            DataSet ds = objDL_Customer.getBT(_GetBT, ConnectionString);

            List<GetBTViewModel> _lstGetBT = new List<GetBTViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBT.Add(
                    new GetBTViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        Description = Convert.ToString(dr["Description"]),
                        Label = Convert.ToString(dr["Label"]),
                    }
                    );
            }

            return _lstGetBT;
        }
        public DataSet GetSetupDropDownValue(Customer objPropCustomer)
        {
            return objDL_Customer.GetSetupDropDownValue(objPropCustomer);
        }

        public DataSet getService(Customer objPropCustomer)
        {
            return objDL_Customer.getService(objPropCustomer);
        }

        public DataSet getSourceCount(Customer objPropCustomer)
        {
            return objDL_Customer.getSourceCount(objPropCustomer);
        }

        public DataSet getOpportunityStatus(Customer objPropCustomer)
        {
            return objDL_Customer.getOpportunityStatus(objPropCustomer);
        }

        public DataSet getEstimateCategory(Customer objPropCustomer)
        {
            return objDL_Customer.getEstimateCategory(objPropCustomer);
        }

        public DataSet getPhoneByRol(Customer objPropCustomer)
        {
            return objDL_Customer.getPhoneByRol(objPropCustomer);
        }
        public DataSet getPhoneByID(Customer objPropCustomer)
        {
            return objDL_Customer.getPhoneByID(objPropCustomer);
        }
        public void AddService(Customer objPropCustomer)
        {
            objDL_Customer.UpdateService(objPropCustomer);
        }
        public void DeleteBT(Customer objPropCustomer)
        {
            objDL_Customer.UpdateBT(objPropCustomer);
        }
        public void DeleteService(Customer objPropCustomer)
        {
            objDL_Customer.UpdateService(objPropCustomer);
        }
        public DataSet getSource(Customer objPropCustomer)
        {
            return objDL_Customer.getSource(objPropCustomer);
        }

        public void AddBT(Customer objPropCustomer)
        {
            objDL_Customer.UpdateBT(objPropCustomer);
        }
        public void UpdateStageHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateStageHeader(objCustomer);
        }
        public void UpdateBTHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateBTHeader(objCustomer);
        }
        public void UpdateServicesHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateServicesHeader(objCustomer);
        }
        public void UpdateEquipmentCategoryHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateEquipmentCategoryHeader(objCustomer);
        }
        public void UpdateEquipmentTypeHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateEquipmentTypeHeader(objCustomer);
        }
        public void UpdateEquipmentBuildingHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateEquipmentBuildingHeader(objCustomer);
        }

        public void UpdateEquipmentClassificationHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateEquipmentClassificationHeader(objCustomer);
        }
        public void AddSource(Customer objPropCustomer)
        {
            objDL_Customer.AddSource(objPropCustomer);
        }

        public void UpdateCustomerContact(Customer objCustomer)
        {
            objDL_Customer.UpdateCustomerContact(objCustomer);
        }
        public void UpdateCustomerCollectionNote(Customer objCustomer)
        {
            objDL_Customer.UpdateCustomerCollectionNote(objCustomer);
        }
        public DataSet GetAIAReportData(Customer objProp_Customer)
        {
            return objDL_Customer.GetAIAReportData(objProp_Customer);
        }
        public DataSet GetProjectVarianceReport(Customer objProp_Customer)
        {
            return objDL_Customer.GetProjectVarianceReport(objProp_Customer);
        }
        public DataSet GetWIP(Customer objProp_Customer)
        {
            return objDL_Customer.GetWIP(objProp_Customer);
        }
        public int SaveWIP(Customer objProp_Customer)
        {
            return objDL_Customer.SaveWIP(objProp_Customer);
        }
        public int DeleteWIP(Customer objProp_Customer)
        {
            return objDL_Customer.DeleteWIP(objProp_Customer);
        }
        public int UpdateWIPStatus(Customer objProp_Customer)
        {
            return objDL_Customer.UpdateWIPStatus(objProp_Customer);
        }
        public DataSet UpdateMailFields(Customer objProp_Customer)
        {
            return objDL_Customer.UpdateMailFields(objProp_Customer);
        }

        public void AddRevisionNotes(Customer objPropCustomer)
        {
            objDL_Customer.AddRevisionNotes(objPropCustomer);
        }

        public void AddCollectionNotes(Customer objPropCustomer)
        {
            objDL_Customer.AddCollectionNotes(objPropCustomer);
        }
        public void UpdateCollectionNotes(string conn, int noteId, String note,string updatedBy)
        {
            objDL_Customer.UpdateCollectionNotes(conn, noteId,note, updatedBy);
        }
        public DataSet RevisionNotesByEstimate(Customer objProp_Customer)
        {
            return objDL_Customer.RevisionNotesByEstimate(objProp_Customer);
        }

        public DataSet GetCollectionNotes(Customer objProp_Customer)
        {
            return objDL_Customer.GetCollectionNotes(objProp_Customer);
        }

        public DataSet GetDescAndAmountOfEstimateByID(Customer objCustomer)
        {
            return objDL_Customer.GetDescAndAmountOfEstimateByID(objCustomer);
        }
        public DataSet GetProspectLogs(Customer objPropCustomer)
        {
            return objDL_Customer.GetProspectLogs(objPropCustomer);
        }
        public DataSet GetTasksLogs(Customer objPropCustomer)
        {
            return objDL_Customer.GetTasksLogs(objPropCustomer);
        }
        public DataSet getJobProjectExportToExcel(Customer objPropCustomer, DataTable listJPIID)
        {
            return objDL_Customer.getJobProjectExportToExcel(objPropCustomer, listJPIID);
        }
        public DataSet getAllProjectTemplateCustomField(Customer objPropCustomer, DataTable dtJob)
        {
            return objDL_Customer.getAllProjectTemplateCustomField(objPropCustomer, dtJob);
        }
        public void UpdateCustomerLocationRemarks(Customer objPropCustomer)
        {
            objDL_Customer.UpdateCustomerLocationRemarks(objPropCustomer);
        }
       
        public DataSet GetEstimateLogs(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimateLogs(objPropCustomer);
        }

        public DataSet GetTodoTasksOfUserForTheDate(Customer objPropCustomer)
        {
            return objDL_Customer.GetTodoTasksOfUserForTheDate(objPropCustomer);
        }
        public DataSet GetTemplateName(Customer objPropCustomer)
        {
            return objDL_Customer.GetTemplateName(objPropCustomer);
        }
        public DataSet GetAllTemplateName(Customer objPropCustomer)
        {
            return objDL_Customer.GetAllTemplateName(objPropCustomer);
        }



        public DataSet GetOppStatus(Customer objPropCustomer)
        {
            return objDL_Customer.GetOppStatus(objPropCustomer);
        }

        public DataSet GetAllOppStatus(Customer objPropCustomer)
        {
            return objDL_Customer.GetAllOppStatus(objPropCustomer);
        }


        public DataSet GetEstimateByIDToCalculateReport(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimateByIDToCalculateReport(objPropCustomer);
        }
        public DataSet GetAllEstimateToCalculateReport(Customer objPropCustomer)
        {
            return objDL_Customer.GetAllEstimateToCalculateReport(objPropCustomer);
        }



        public void UpdateDefaultWorkerHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateDefaultWorkerHeader(objCustomer);
        }

        public string GetDefaultWorkerHeader(Customer objCustomer)
        {
            return objDL_Customer.GetDefaultWorkerHeader(objCustomer);
        }

        //API
        public string GetDefaultWorkerHeader(GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader, string ConnectionString)
        {
            return objDL_Customer.GetDefaultWorkerHeader(_GetDefaultWorkerHeader, ConnectionString);
        }

        public DataSet getJobProjectByJobIDRatesByIdPersonByJob(Customer objCustomer)
        {
            return objDL_Customer.getJobProjectByJobIDRatesByIdPersonByJob(objCustomer);
        }

        public DataSet GetEquipmentShutdownForReport(Customer objCustomer, DateTime endDate)
        {
            return objDL_Customer.GetEquipmentShutdownForReport(objCustomer, endDate);
        }

        //API
        public List<GetEquipmentShutdownForReportViewModel> GetEquipmentShutdownForReport(GetEquipmentShutdownForReportParam _GetEquipmentShutdownForReport, string ConnectionString, DateTime endDate)
        {
            DataSet ds = objDL_Customer.GetEquipmentShutdownForReport(_GetEquipmentShutdownForReport, ConnectionString, endDate);

            List<GetEquipmentShutdownForReportViewModel> _lstGetEquipmentShutdownForReport = new List<GetEquipmentShutdownForReportViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipmentShutdownForReport.Add(
                    new GetEquipmentShutdownForReportViewModel()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        Location = Convert.ToString(dr["Location"]),
                        Equipment = Convert.ToString(dr["Equipment"]),
                        Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                        Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
                        Mechanic = Convert.ToString(dr["Mechanic"]),
                        Planned = Convert.ToString(dr["Planned"]),
                        reason = Convert.ToString(dr["reason"]),
                        longdesc = Convert.ToString(dr["longdesc"]),
                        Status = Convert.ToString(dr["Status"]),
                        Supervisor = Convert.ToString(dr["Supervisor"]),
                        WorkCompleted = Convert.ToString(dr["WorkCompleted"]),
                        Worker = Convert.ToString(dr["Worker"]),
                        Row = Convert.ToInt32(DBNull.Value.Equals(dr["Row#"]) ? 0 : dr["Row#"]),
                    });
            }

            return _lstGetEquipmentShutdownForReport;
        }

        public DataSet GetEquipmentShutdownActivityForReport(Customer objCustomer, DateTime startDate, DateTime endDate, string eqId, bool filtered)
        {
            return objDL_Customer.GetEquipmentShutdownActivityForReport(objCustomer, startDate, endDate, eqId, filtered);
        }

        public DataSet GetDowntimeEquipmentReport(Customer objCustomer, List<RetainFilter> filters, bool inclInactive)
        {
            return objDL_Customer.GetDowntimeEquipmentReport(objCustomer, filters, inclInactive);
        }

        //API
        public ListGetEquipShutdownActivityForReport GetEquipmentShutdownActivityForReport(GetEquipShutdownActivityForReportParam _GetEquipShutdownActivityForReport, string ConnectionString, DateTime startDate, DateTime endDate, string eqId, bool filtered)
        {
            DataSet ds = objDL_Customer.GetEquipmentShutdownActivityForReport(_GetEquipShutdownActivityForReport, ConnectionString, startDate, endDate, eqId, filtered);

            ListGetEquipShutdownActivityForReport _ds = new ListGetEquipShutdownActivityForReport();
            List<GetEquipShutdownActivityForReportTable1> _lstTable1 = new List<GetEquipShutdownActivityForReportTable1>();
            List<GetEquipShutdownActivityForReportTable2> _lstTable2 = new List<GetEquipShutdownActivityForReportTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetEquipShutdownActivityForReportTable1()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Location = Convert.ToString(dr["Location"]),
                        Equipment = Convert.ToString(dr["Equipment"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetEquipShutdownActivityForReportTable2()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Location = Convert.ToString(dr["Location"]),
                        Equipment = Convert.ToString(dr["Equipment"]),
                        Ticket = Convert.ToString(dr["Ticket"]),
                        Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
                        Mechanic = Convert.ToString(dr["Mechanic"]),
                        Planned = Convert.ToString(dr["Planned"]),
                        reason = Convert.ToString(dr["reason"]),
                        longdesc = Convert.ToString(dr["longdesc"]),
                        Status = Convert.ToString(dr["Status"]),
                        Supervisor = Convert.ToString(dr["Supervisor"]),
                        WorkCompleted = Convert.ToString(dr["WorkCompleted"]),
                        Worker = Convert.ToString(dr["Worker"]),
                        Row = Convert.ToInt32(DBNull.Value.Equals(dr["Row#"]) ? 0 : dr["Row#"]),
                    });
            }

            _ds.lstTable1 = _lstTable1;
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }

        public string GetDefaultSalesPerson(Customer objCustomer)
        {
            return objDL_Customer.GetDefaultSalesPerson(objCustomer);
        }

        public void ConvertLeadEquipment(Customer objCustomer)
        {
            objDL_Customer.ConvertLeadEquipment(objCustomer);
        }

        //API
        public void ConvertLeadEquipment(ConvertLeadEquipmentParam _ConvertLeadEquipment, string ConnectionString)
        {
            objDL_Customer.ConvertLeadEquipment(_ConvertLeadEquipment, ConnectionString);
        }

        public DataSet GetEstimatesByOpportunityID(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimatesByOpportunityID(objPropCustomer);
        }

        public DataSet GetEstimatesByOpportunityIDForProjectLinking(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimatesByOpportunityIDForProjectLinking(objPropCustomer);
        }

        public DataSet GetEquipmentsOfEstimate(Customer objPropCustomer)
        {
            return objDL_Customer.GetEquipmentsOfEstimate(objPropCustomer);
        }

        public DataSet GetTemplateByOppID(Customer objPropCustomer)
        {
            return objDL_Customer.GetTemplateByOppID(objPropCustomer);
        }

        public DataSet GetAllOpportunityIDs(Customer objPropCustomer)
        {
            return objDL_Customer.GetAllOpportunityIDs(objPropCustomer);
        }

        public DataSet GetEstimateGroupNames(Customer objPropCustomer)
        {
            return objDL_Customer.GetEstimateGroupNames(objPropCustomer);
        }

        public DataSet GetEquipmentsByGroupId(Customer objPropCustomer)
        {
            return objDL_Customer.GetEquipmentsByGroupId(objPropCustomer);
        }

        public String GetProspectID(Customer objPropCustomer)
        {
            return objDL_Customer.GetProspectID(objPropCustomer);
        }

        public DataSet AddUpdateEstimateGroup(Customer objPropCustomer)
        {
            return objDL_Customer.AddUpdateEstimateGroup(objPropCustomer);
        }
        public DataSet AddProjectGroup(Customer objPropCustomer)
        {
            return objDL_Customer.AddProjectGroup(objPropCustomer);
        }

        public DataSet AddJobProject_Notes(Customer objPropCustomer)
        {
            return objDL_Customer.AddJobProject_Notes(objPropCustomer);
        }
        public DataSet GetJobProject_Notes(Customer objPropCustomer)
        {
            return objDL_Customer.GetJobProject_Notes(objPropCustomer);
        }
        public DataSet GetJobProject_NotesExport(Customer objPropCustomer, String lsProjectNoteID)
        {
            return objDL_Customer.GetJobProject_NotesExport(objPropCustomer, lsProjectNoteID);
        }

        //public DataSet GetLogs(Customer objPropCustomer)
        //{
        //    return objDL_Customer.GetLogs(objPropCustomer);
        //}

        public DataSet GetLogs(Customer objPropCustomer)
        {
            return objDL_Customer.GetLogs(objPropCustomer);
        }

        public int UpdateAmountWIPInvoice(Customer objProp_Customer)
        {
            return objDL_Customer.UpdateAmountWIPInvoice(objProp_Customer);
        }
        public DataSet GetLocationOfCustomerInCaseUnique(Customer objPropCustomer)
        {
            return objDL_Customer.GetLocationOfCustomerInCaseUnique(objPropCustomer);
        }

        public DataSet GetProjectGroupNames(Customer objPropCustomer)
        {
            return objDL_Customer.GetProjectGroupNames(objPropCustomer);
        }

        public DataSet GetOpportunityForecast(Customer objPropCustomer, int type)
        {
            return objDL_Customer.GetOpportunityForecast(objPropCustomer, type);
        }

        public DataSet GetEstimateBacklog(Customer objPropCustomer, List<RetainFilter> filters, string status, bool isConvertToJob = false)
        {
            return objDL_Customer.GetEstimateBacklog(objPropCustomer, filters, status, isConvertToJob);
        }

        public DataSet GetEstimateRate(Customer objPropCustomer, List<RetainFilter> filters, string status, bool isConvertToJob = false)
        {
            return objDL_Customer.GetEstimateRate(objPropCustomer, filters, status, isConvertToJob);
        }

        public DataSet GetTaskCategories(Customer objPropCustomer)
        {
            return objDL_Customer.GetTaskCategories(objPropCustomer);
        }
        public void CRUDTaskCategory(Customer objPropCustomer)
        {
            objDL_Customer.CRUDTaskCategory(objPropCustomer);
        }

        public int EditWIPStatus(Customer objProp_Customer)
        {
            return objDL_Customer.EditWIPStatus(objProp_Customer);
        }
        public int SaveWIPNew(Customer objProp_Customer)
        {
            return objDL_Customer.SaveWIPNew(objProp_Customer);
        }
        public void AddUpdateContact(PhoneModel objPhone)
        {
            objDL_Customer.AddUpdateContact(objPhone);
        }

        public void DeleteContact(PhoneModel objPhone)
        {
            objDL_Customer.DeleteContact(objPhone);
        }

        public DataSet GetEstimateByLoc(String conn, int loc)
        {
            return objDL_Customer.GetEstimateByLoc(conn,loc);
        }

        public void LinkEstimateToProject(String conn, int estimateID,int job, int oppId,String updatedby)
        {
            objDL_Customer.LinkEstimateToProject(conn, estimateID, job, oppId, updatedby);
        }

        public DataSet GetAllEstimateLinkToProject(String conn,  int job)
        {
            return objDL_Customer.GetAllEstimateLinkToProject(conn, job);
        }

        public DataSet GetAllProjectByLoc(String conn, int loc)
        {
            return objDL_Customer.GetAllProjectByLoc(conn, loc);
        }

        public int EditAmountWIPInvoice(Customer objProp_Customer)
        {
            return objDL_Customer.EditAmountWIPInvoice(objProp_Customer);
        }

        public DataSet getEstimateByProject(String conn, int job)
        {
            return objDL_Customer.getEstimateByProject(conn, job);
        }

        public DataSet GetRecentCollectionNotes(Customer objProp_Customer)
        {
            return objDL_Customer.GetRecentCollectionNotes(objProp_Customer);
        }

        public void DeleteCollectionNotes(String conn, int noteId)
        {
            objDL_Customer.DeleteCollectionNotes(conn,noteId);
        }

        public DataSet GetBOMItemsByVendor(Customer objPropCustomer)
        {
            return objDL_Customer.GetBOMItemsByVendor(objPropCustomer);
        }

        public DataSet GetProjectsWithWorkflows(Customer objPropCustomer)
        {
            return objDL_Customer.GetProjectsWithWorkflows(objPropCustomer);
        }

        public void EstimateConversionUndo(Customer objPropCustomer)
        {
            objDL_Customer.EstimateConversionUndo(objPropCustomer);
        }

        public DataSet LinkEstimateToProjectNew(String conn, int estimateID, int job, String updatedby)
        {
            return objDL_Customer.LinkEstimateToProjectNew(conn, estimateID, job, updatedby);
        }

        public void DeleteProjectStage(Customer objPropCustomer)
        {
            objDL_Customer.DeleteProjectStage(objPropCustomer);
        }

        //public void AddStagesProject(Customer objPropCustomer)
        //{
        //    objDL_Customer.UpdateStagesProject(objPropCustomer);
        //}

        public void UpdateStageProjectHeader(Customer objCustomer)
        {
            objDL_Customer.UpdateStageProjectHeader(objCustomer);
        }

        public DataSet GetAllProjectStages(string connStr)
        {
            return objDL_Customer.GetAllProjectStages(connStr);
        }

        public string GetProjectStageHeader(Customer objCustomer)
        {
            return objDL_Customer.GetProjectStageHeader(objCustomer);
        }

        public void UpdateProjectStage(Customer objPropCustomer)
        {
            objDL_Customer.UpdateProjectStage(objPropCustomer);
        }

        public DataSet GetStagesByDepartment(string connStr, string depId)
        {
            return objDL_Customer.GetStagesByDepartment(connStr, depId);
        }

        public DataSet GetProjectStageItemByID(string connStr, string stageItemId)
        {
            return objDL_Customer.GetProjectStageItemByID(connStr, stageItemId);
        }
        public DataSet GetContractByLoc(string connStr, int loc)
        {
            return objDL_Customer.GetContractByLoc(connStr,loc);
        }
        public void SaveReverseWip(Customer objProp_Customer)
        {
            objDL_Customer.SaveReverseWip(objProp_Customer);
        }

        public DataSet GetEstimateApprovedStatusHistory(Customer objCustomer)
        {
            return objDL_Customer.GetEstimateApprovedStatusHistory(objCustomer);
        }

        public void UpdateEstimateApprovalStatus(Customer objCustomer)
        {
            objDL_Customer.UpdateEstimateApprovalStatus(objCustomer);
        }

        //public bool CanRevertLocCustOfOppAndEstOnUnlinkProject(Customer objPropCustomer)
        //{
        //    return objDL_Customer.CanRevertLocCustOfOppAndEstOnUnlinkProject(objPropCustomer);
        //}
    }
}
