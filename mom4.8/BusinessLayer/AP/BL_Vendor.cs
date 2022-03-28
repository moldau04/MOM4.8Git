using BusinessEntity;
using BusinessEntity.APModels;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_Vendor
    {
        DL_Vendor _objDLVendor = new DL_Vendor();
        public int AddVendor(Vendor objVendor)
        {
            return _objDLVendor.AddVendor(objVendor);
        }
        public int AddVendor(AddVendorParam _AddVendorParam, string ConnectionString)
        {
            int VendorID = _objDLVendor.AddVendor(_AddVendorParam, ConnectionString);
            return VendorID;
        }


        public void UpdateVendorContact(Vendor objVendor)
        {
            _objDLVendor.UpdateVendorContact(objVendor);
        }
        public void UpdateVendorContact(UpdateVendorContactParam _UpdateVendorContactParam, string ConnectionString)
        {
            _objDLVendor.UpdateVendorContact(_UpdateVendorContactParam, ConnectionString);
        }
        public void UpdateVendor(Vendor objVendor)
        {
            _objDLVendor.UpdateVendor(objVendor);
        }
        public void UpdateVendor(UpdateVendorParam _UpdateVendorParam, string ConnectionString)
        {
            _objDLVendor.UpdateVendor(_UpdateVendorParam,ConnectionString);
        }
        public void UpdateVendorSTax(string ConnConfig, string sTax, int VendorId) { _objDLVendor.UpdateVendorSTax(ConnConfig, sTax, VendorId); }

        public void UpdateVendorSTax(UpdateVendorSTaxParam _UpdateVendorSTaxParam, string ConnectionString)
        {
            _objDLVendor.UpdateVendorSTax(_UpdateVendorSTaxParam, ConnectionString);
        }
        public DataSet getVendorContactByRolID(Vendor objVendor)
        {
            return _objDLVendor.getVendorContactByRolID(objVendor);
        }
        public List<GetVendorContactByRolIDViewModel> getVendorContactByRolID(getVendorContactByRolIDParam _getVendorContactByRolIDParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.getVendorContactByRolID( _getVendorContactByRolIDParam,  ConnectionString);

            List<GetVendorContactByRolIDViewModel> _lstGetVendorContactByRolIDViewModel = new List<GetVendorContactByRolIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetVendorContactByRolIDViewModel.Add(
                    new GetVendorContactByRolIDViewModel()
                    {
                        contactid = Convert.ToInt32(DBNull.Value.Equals(dr["contactid"]) ? 0 : dr["contactid"]),
                        //Rol = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["Rol"]),
                        name = Convert.ToString(dr["name"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Email = Convert.ToString(dr["Email"]),
                        Title = Convert.ToString(dr["Title"]),
                        Cell = Convert.ToString(dr["Cell"]),
                        EmailRecPO = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecPO"]) ? null : dr["EmailRecPO"]),
                    }
                    );
            }

            return _lstGetVendorContactByRolIDViewModel;
        }
        public DataSet GetVendor(Vendor objVendor)
        {
            return _objDLVendor.GetVendor(objVendor);
        }
        public List<VendorViewModel> GetVendor(GetVendorParam _GetVendorParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetVendor(_GetVendorParam, ConnectionString);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        VType = Convert.ToString(dr["Type"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        CLimit = Convert.ToDouble(DBNull.Value.Equals(dr["CLimit"]) ? 0 : dr["CLimit"]),
                        Vendor1099 = Convert.ToInt16(DBNull.Value.Equals(dr["1099"]) ? 0 : dr["1099"]),
                        FID = Convert.ToString(dr["FID"]),
                        DA = Convert.ToInt32(DBNull.Value.Equals(dr["DA"]) ? 0 : dr["DA"]),
                        AcctNumber = Convert.ToString(dr["Acct#"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Days = Convert.ToInt16(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                        InUse = Convert.ToInt16(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        Remit = Convert.ToString(dr["Remit"]),
                        OnePer = Convert.ToInt16(DBNull.Value.Equals(dr["OnePer"]) ? 0 : dr["OnePer"]),
                        DBank = Convert.ToString(dr["DBank"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        Custom3 = Convert.ToString(dr["Custom3"]),
                        Custom4 = Convert.ToString(dr["Custom4"]),
                        Custom5 = Convert.ToString(dr["Custom5"]),
                        Custom6 = Convert.ToString(dr["Custom6"]),
                        Custom7 = Convert.ToString(dr["Custom7"]),
                        Custom8 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom8"]) ? null : dr["Custom8"]),
                        Custom9 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom9"]) ? null : dr["Custom9"]),
                        Custom10 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom10"]) ? null : dr["Custom10"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }
        public void DeleteVendor(Vendor objVendor)
        {
            _objDLVendor.DeleteVendor(objVendor);
        }
        public void DeleteVendor(DeleteVendorParam _DeleteVendorParam, string connectionString)
        {
            _objDLVendor.DeleteVendor(_DeleteVendorParam, connectionString);
        }
        public DataSet GetAll(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendor(objVendor);
        }
        public DataSet IsExistsForInsertVendor(Vendor objVendor)
        {
            return _objDLVendor.IsExistsForInsertVendor(objVendor);
        }
        public List<VendorViewModel> IsExistsForInsertVendor(IsExistsForInsertVendorParam _IsExistsForInsertVendorParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.IsExistsForInsertVendor(_IsExistsForInsertVendorParam, ConnectionString);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        Count = Convert.ToInt64(DBNull.Value.Equals(dr["CountVendor"]) ? 0 : dr["CountVendor"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }
        public DataSet IsExistForUpdateVendor(Vendor objVendor)
        {
            return _objDLVendor.IsExistForUpdateVendor(objVendor);
        }
        public List<VendorViewModel> IsExistForUpdateVendor(IsExistForUpdateVendorParam _IsExistForUpdateVendorParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.IsExistForUpdateVendor(_IsExistForUpdateVendorParam, ConnectionString);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        Count = Convert.ToInt64(DBNull.Value.Equals(dr["CountVendor"]) ? 0 : dr["CountVendor"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }
        public DataSet GetAllVendorDetails(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendorDetails(objVendor);
        }
        public DataSet GetAllVenderGridview(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendorGridview(objVendor);
        }
        public DataSet GetVendorEdit(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendorEdit(objVendor);
        }
        public List<VendorViewModel> GetVendorEdit(GetVendorEditParam _GetVendorEditParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetAllVendorEdit( _GetVendorEditParam,  ConnectionString);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        Name = Convert.ToString(dr["Name"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        AcctNumber = Convert.ToString(dr["Acct#"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        Email = Convert.ToString(dr["EMail"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        ContactName = Convert.ToString(dr["Contact"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Company = Convert.ToString(dr["Company"]),
                        Address = Convert.ToString(dr["Address"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Country = Convert.ToString(dr["Country"]),
                        Website = Convert.ToString(dr["Website"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        GeoLock = Convert.ToInt32(DBNull.Value.Equals(dr["GeoLock"]) ? 0 : dr["GeoLock"]),
                        Since = Convert.ToDateTime(DBNull.Value.Equals(dr["Since"]) ? null : dr["Since"]),
                        Last = Convert.ToDateTime(DBNull.Value.Equals(dr["Last"]) ? null : dr["Last"]),
                        Days = Convert.ToInt16(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        CLimit = Convert.ToInt32(DBNull.Value.Equals(dr["CLimit"]) ? 0 : dr["CLimit"]),
                        ShipVia = Convert.ToString(dr["ShipVia"]),
                        InUse = Convert.ToInt16(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        Vendor1099 = Convert.ToInt16(DBNull.Value.Equals(dr["1099"]) ? 0 : dr["1099"]),
                        DA = Convert.ToInt32(DBNull.Value.Equals(dr["DA"]) ? 0 : dr["DA"]),
                        DefaultAcct = Convert.ToString(dr["DefaultAcct"]),
                        Remit = Convert.ToString(dr["Remit"]),
                        intBox = Convert.ToInt16(DBNull.Value.Equals(dr["intBox"]) ? 0 : dr["intBox"]),
                        FID = Convert.ToString(dr["FID"]),
                        EmailRecPO = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecPO"]) ? 0 : dr["EmailRecPO"]),
                        VType = Convert.ToString(dr["VType"]),
                        STax = Convert.ToString(dr["STax"]),
                        UTax = Convert.ToString(dr["Utax"]),
                        Remarks = Convert.ToString(dr["Remarks"])
                    }
                    );
            }

            return _lstVendorViewModel;
        }
        public DataSet GetAllVendors(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendors(objVendor);
        }
        public DataSet GetVendorSearch(Vendor objVendor)
        {
            return _objDLVendor.GetVendorSearch(objVendor);
        }
        public List<VendorViewModel> GetVendorSearch(string ConnectionString, GetVendorSearchParam _GetVendorSearchParam)
        {
            DataSet ds = _objDLVendor.GetVendorSearch(ConnectionString, _GetVendorSearchParam);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Company = Convert.ToString(dr["Company"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        desc = Convert.ToString(dr["desc"]),
                        Days = Convert.ToInt16(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                        VState = Convert.ToString(dr["VState"]),
                        Term = Convert.ToInt32(DBNull.Value.Equals(dr["Term"]) ? 0 : dr["Term"]),
                        STaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["STaxRate"]) ? 0 : dr["STaxRate"]),
                        UTaxRate = Convert.ToDouble(DBNull.Value.Equals(dr["UTaxRate"]) ? 0 : dr["UTaxRate"]),
                        STaxType = Convert.ToInt32(DBNull.Value.Equals(dr["STaxType"]) ? 0 : dr["STaxType"]),
                        UTaxType = Convert.ToInt32(DBNull.Value.Equals(dr["UTaxType"]) ? 0 : dr["UTaxType"]),
                        STaxName = Convert.ToString(dr["STaxName"]),
                        UTaxName = Convert.ToString(dr["UTaxName"]),
                        STaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["STaxGL"]) ? 0 : dr["STaxGL"]),
                        SUaxGL = Convert.ToInt32(DBNull.Value.Equals(dr["SUaxGL"]) ? 0 : dr["SUaxGL"]),
                        STax = Convert.ToString(dr["STax"]),
                        UTax = Convert.ToString(dr["UTax"]),
                    }
                    );
            }
            return _lstVendorViewModel;
        }

        public DataSet GetVendorByName(Vendor objVendor)
        {
            return _objDLVendor.GetVendorByName(objVendor);
        }

        public List<VendorViewModel> GetVendorByName(GetVendorByNameParam _GetVendorByNameParam, string ConnectionString)
        {
             DataSet ds = _objDLVendor.GetVendorByName(_GetVendorByNameParam, ConnectionString);

            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name =Convert.ToString(dr["Name"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }

        public DataSet GetVendorSearchProject(Vendor objVendor)
        {
            return _objDLVendor.GetVendorSearchProject(objVendor);
        }

        public bool IsExistVendorDetails(Vendor _objVendor)
        {
            return _objDLVendor.IsExistVendorDetails(_objVendor);
        }

        public bool IsExistVendorDetails(IsExistVendorDetailsParam _IsExistVendorDetailsParam, string connectionString)
        {
            bool _IsExist = _objDLVendor.IsExistVendorDetails(_IsExistVendorDetailsParam, connectionString);
            return _IsExist;
        }
        public DataSet GetVendorRolDetails(Vendor _objVendor)
        {
            return _objDLVendor.GetVendorRolDetails(_objVendor);
        }

        public List<VendorViewModel> GetVendorRolDetails(GetVendorRolDetailsParam _objGetVendorRolDetailsParam,string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetVendorRolDetails(_objGetVendorRolDetailsParam, ConnectionString);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        VType = Convert.ToString(dr["Type"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        CLimit = Convert.ToDouble(DBNull.Value.Equals(dr["CLimit"]) ? 0 : dr["CLimit"]),
                        Vendor1099 = Convert.ToInt16(DBNull.Value.Equals(dr["1099"]) ? 0 : dr["1099"]),
                        FID = Convert.ToString(dr["FID"]),
                        DA = Convert.ToInt32(DBNull.Value.Equals(dr["DA"]) ? 0 : dr["DA"]),
                        AcctNumber = Convert.ToString(dr["Acct#"]),
                        Terms = Convert.ToInt16(DBNull.Value.Equals(dr["Terms"]) ? 0 : dr["Terms"]),
                        Disc = Convert.ToDouble(DBNull.Value.Equals(dr["Disc"]) ? 0 : dr["Disc"]),
                        Days = Convert.ToInt16(DBNull.Value.Equals(dr["Days"]) ? 0 : dr["Days"]),
                        InUse = Convert.ToInt16(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        //Remit = Convert.ToString(dr["RemitAddress"]),
                        RemitAddress = Convert.ToString(dr["RemitAddress"]),
                        OnePer = Convert.ToInt16(DBNull.Value.Equals(dr["OnePer"]) ? 0 : dr["OnePer"]),
                        DBank = Convert.ToString(dr["DBank"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        Custom3 = Convert.ToString(dr["Custom3"]),
                        Custom4 = Convert.ToString(dr["Custom4"]),
                        Custom5 = Convert.ToString(dr["Custom5"]),
                        Custom6 = Convert.ToString(dr["Custom6"]),
                        Custom7 = Convert.ToString(dr["Custom7"]),
                        Custom8 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom8"]) ? null : dr["Custom8"].ToString()),
                        Custom9 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom9"]) ? null : dr["Custom9"].ToString()),
                        Custom10 = Convert.ToDateTime(DBNull.Value.Equals(dr["Custom10"]) ? null : dr["Custom10"].ToString()),
                        ShipVia = Convert.ToString(dr["ShipVia"]),
                        QBVendorID = Convert.ToString(dr["QBVendorID"]),
                        intBox = Convert.ToInt16(DBNull.Value.Equals(dr["intBox"]) ? 0 : dr["intBox"]),
                        STax = Convert.ToString(dr["STax"]),
                        UTax = Convert.ToString(dr["UTax"]),
                        VendorAddress = Convert.ToString(dr["VendorAddress"]),
                        Name = Convert.ToString(dr["Name"]),
                        State = Convert.ToString(dr["State"]),
                        City = Convert.ToString(dr["City"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Address = Convert.ToString(dr["Address"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }


        public DataSet GetVendorBasedPORemitAddress(Vendor objVendor)
        {
            return _objDLVendor.GetVendorBasedPORemitAddress(objVendor);
        }

        public DataSet GetVendorGLById(Vendor objVendor)
        {
            return _objDLVendor.GetVendorGLById(objVendor);
        }

        public DataSet GetVendorListDetails(Vendor objVendor)
        {
            return _objDLVendor.GetVendorListDetails(objVendor);
        }

        //RAHIL's
        public DataSet GetVendorAcct(Vendor _objVendor)
        {
            return _objDLVendor.GetVendorAcct(_objVendor);
        }

        public List<GetVendorAcctList> GetVendorAcct(GetVendorAcctParam _GetVendorAcctParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetVendorAcct(_GetVendorAcctParam, ConnectionString);
            List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetVendorAcctList.Add(
                    new GetVendorAcctList()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        AcctNumber = Convert.ToString(dr["Acct#"]),
                    }
                    );
            }

            return _lstGetVendorAcctList;
        }

        public DataSet GetAllVenderAjaxSearch(Vendor objVendor)
        {

            return _objDLVendor.GetAllVenderAjaxSearch(objVendor);
        }

        public List<GetAllVenderAjaxSearchModel> GetAllVenderAjaxSearch(GetAllVenderAjaxSearchParam _GetAllVenderAjaxSearchParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetAllVenderAjaxSearch(_GetAllVenderAjaxSearchParam, ConnectionString);

            List<GetAllVenderAjaxSearchModel> _lstGetAllVenderAjaxSearch = new List<GetAllVenderAjaxSearchModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllVenderAjaxSearch.Add(
                    new GetAllVenderAjaxSearchModel()
                    {
                        RowNumber = Convert.ToInt64(DBNull.Value.Equals(dr["RowNumber"]) ? 0 : dr["RowNumber"]),
                        TotalRow = Convert.ToInt32(DBNull.Value.Equals(dr["TotalRow"]) ? 0 : dr["TotalRow"]),
                        State = Convert.ToString(dr["State"]),
                        City = Convert.ToString(dr["City"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Address = Convert.ToString(dr["Address"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        Name = Convert.ToString(dr["Name"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Acct = Convert.ToString(dr["Acct"]),
                        Type = Convert.ToString(dr["Type"]),
                        Status = Convert.ToString(dr["Status"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    }
                    );
            }
            return _lstGetAllVenderAjaxSearch;

        }
        public DataSet GetOpenBillVendor(Vendor objVendor)
        {
            return _objDLVendor.GetOpenBillVendor(objVendor);
        }

        public List<VendorViewModel> GetOpenBillVendor(GetOpenBillVendorParam _GetOpenBillVendorParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetOpenBillVendor(_GetOpenBillVendorParam,ConnectionString);
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }

        public DataSet GetOpenBillVendorByCompany(Vendor objVendor)
        {
            return _objDLVendor.GetOpenBillVendorByCompany(objVendor);
        }
        public List<VendorViewModel> GetOpenBillVendorByCompany(GetOpenBillVendorByCompanyParam _GetOpenBillVendorByCompanyParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetOpenBillVendorByCompany(_GetOpenBillVendorByCompanyParam,ConnectionString);

            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstVendorViewModel.Add(
                    new VendorViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                    }
                    );
            }

            return _lstVendorViewModel;
        }
        public void UpdateVendorBalance(Vendor objVendor)
        {
            _objDLVendor.UpdateVendorBalance(objVendor);
        }
        public void UpdateVendorBalance(UpdateVendorBalanceParam objUpdateVendorBalanceParam, string ConnectionString)
        {
            _objDLVendor.UpdateVendorBalance(objUpdateVendorBalanceParam, ConnectionString);
        }
        public DataSet GetVendorLogs(Vendor objVendor)
        {
            return _objDLVendor.GetVendorLogs(objVendor);
        }
        public List<LogViewModel> GetVendorLogs(GetVendorLogsParam _GetVendorLogsParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.GetVendorLogs(_GetVendorLogsParam, ConnectionString);
            List<LogViewModel> _lstLogViewModel = new List<LogViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstLogViewModel.Add(
                    new LogViewModel()
                    {
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        //customDate = Convert.ToString(dr["fDate"]),
                        //customtime = Convert.ToString(dr["fTime"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                        fUser = Convert.ToString(dr["fUser"]),
                        Field = Convert.ToString(dr["Field"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"])
                    }
                    );
            }

            return _lstLogViewModel;
        }
        public void IsSalesTaxAPBill(string ConnConfig, int IsSalesTaxAPBill, int IsUseTaxAPBill) { _objDLVendor.IsSalesTaxAPBill(ConnConfig, IsSalesTaxAPBill, IsUseTaxAPBill); }
        public void UpdateVendorTax(Vendor objVendor)
        {
            _objDLVendor.UpdateVendorTax(objVendor);
        }
        public void UpdateVendorTax(UpdateVendorTaxParam _UpdateVendorTaxParam, string ConnectionString)
        {
            _objDLVendor.UpdateVendorTax(_UpdateVendorTaxParam, ConnectionString);
        }


        //API
        public List<GetVendorTypeViewModel> getVendorType(getVendorTypeParam _getVendorTypeParam, string ConnectionString)
        {
            DataSet ds = _objDLVendor.getVendorType(_getVendorTypeParam, ConnectionString);

            List<GetVendorTypeViewModel> _GetVendorTypeViewModel = new List<GetVendorTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _GetVendorTypeViewModel.Add(new GetVendorTypeViewModel()
                {
                    Type = Convert.ToString(dr["Type"]),
                    Remarks = Convert.ToString(dr["Remarks"]),
                    Count = Convert.ToInt64(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                });
            }
            return _GetVendorTypeViewModel;
        }

        public string GetVendorNameById(int vendorId, string ConnectionString)
        {
            return _objDLVendor.GetVendorNameById(vendorId, ConnectionString);
        }
    }
}
