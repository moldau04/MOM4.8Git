using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CommonModel;
using BusinessEntity.CustomersModel;
using BusinessEntity.InventoryModel;
using BusinessEntity.payroll;
using BusinessEntity.Payroll;
using BusinessEntity.Recurring;
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
    public class BL_User
    {
        DL_User objDL_User = new DL_User();


        public void getUserAuthorization_New(UserAuthentication _UA)
        {
            objDL_User.getUserAuthorization_New(_UA);
        }
        public DataSet getUserAuthorization(User objPropUser)
        {
            return objDL_User.getUserAuthorization(objPropUser);
        }

        public DataSet getMCPTemplatesByMechanic(User objPropUser)
        {
            return objDL_User.getMCPTemplatesByMechanic(objPropUser);
        }

        public DataSet GetCustomerType(User objPropUserr)
        {
            return objDL_User.GetCustomerType(objPropUserr);
        }

        public DataSet GetLocationType(User objPropUserr)
        {
            return objDL_User.GetLocationType(objPropUserr);
        }

        //API
        public List<GetLocationTypeViewModel> GetLocationType(GetLocationTypeParam _GetLocationType, string ConnectionString)
        {
            DataSet ds = objDL_User.GetLocationType(_GetLocationType, ConnectionString);

            List<GetLocationTypeViewModel> _lstGetLocationType = new List<GetLocationTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationType.Add(
                    new GetLocationTypeViewModel()
                    {
                        Type = Convert.ToString(dr["Type"]),
                        //Remarks = Convert.ToString(dr["Remarks"]),
                        //Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetLocationType;

        }

        public DataSet getUserLoginAuthorization(User objPropUser)
        {
            return objDL_User.getUserLoginAuthorization(objPropUser);
        }

        public void SetDefaultData(User objPropUser)
        {
            objDL_User.SetDefaultData(objPropUser);
        }

        public DataSet getTSUserAuthorization(User objPropUser)
        {
            return objDL_User.getTSUserAuthorization(objPropUser);
        }

        public DataSet getCustomerType(User objPropUser)
        {
            return objDL_User.getCustomerType(objPropUser);
        }

        //API
        public List<GetCustomerTypeViewModel> getCustomerType(getCustomerTypeParam _getCustomerType, string ConnectionString)
        {
            DataSet ds = objDL_User.getCustomerType(_getCustomerType, ConnectionString);

            List<GetCustomerTypeViewModel> _lstGetCustomerType = new List<GetCustomerTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerType.Add(
                    new GetCustomerTypeViewModel()
                    {
                        Type = Convert.ToString(dr["Type"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetCustomerType;
        }

        public DataSet getVendorType(User objPropUserr)
        {
            return objDL_User.getVendorType(objPropUserr);
        }


        public DataSet getcategoryAll(User objPropUser)
        {
            return objDL_User.getcategoryAll(objPropUser);
        }

        public DataSet getEquiptype(User objPropUser)
        {
            return objDL_User.getEquiptype(objPropUser);
        }

        //API
        public List<GetEquiptypeViewModel> getEquiptype(GetEquiptypeParam _GetEquiptype, string ConnectionString)
        {
            DataSet ds = objDL_User.getEquiptype(_GetEquiptype, ConnectionString);

            List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquiptype.Add(
                    new GetEquiptypeViewModel()
                    {
                        edesc = Convert.ToString(dr["edesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetEquiptype;
        }

        public DataSet getLeadEquiptype(User objPropUser)
        {
            return objDL_User.getLeadEquiptype(objPropUser);
        }

        //API
        public List<GetEquiptypeViewModel> getLeadEquiptype(GetLeadEquiptypeParam _GetLeadEquiptype, string ConnectionString)
        {
            DataSet ds = objDL_User.getLeadEquiptype(_GetLeadEquiptype, ConnectionString);

            List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquiptype.Add(
                    new GetEquiptypeViewModel()
                    {
                        edesc = Convert.ToString(dr["edesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetEquiptype;
        }

        public DataSet getEquipmentCategory(User objPropUser)
        {
            return objDL_User.getEquipmentCategory(objPropUser);
        }

        //API
        public List<GetEquiptypeViewModel> getEquipmentCategory(GetEquipmentCategoryParam _GetEquipmentCategory, string ConnectionString)
        {
            DataSet ds = objDL_User.getEquipmentCategory(_GetEquipmentCategory, ConnectionString);

            List<GetEquiptypeViewModel> _lstGetEquipmentCategory = new List<GetEquiptypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipmentCategory.Add(
                    new GetEquiptypeViewModel()
                    {
                        edesc = Convert.ToString(dr["edesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetEquipmentCategory;
        }

        public DataSet getLeadEquipmentCategory(User objPropUser)
        {
            return objDL_User.getLeadEquipmentCategory(objPropUser);
        }

        //API
        public List<GetEquiptypeViewModel> getLeadEquipmentCategory(GetLeadEquipmentCategoryParam _GetLeadEquipmentCategory, string ConnectionString)
        {
            DataSet ds = objDL_User.getLeadEquipmentCategory(_GetLeadEquipmentCategory, ConnectionString);

            List<GetEquiptypeViewModel> _lstGetEquipmentCategory = new List<GetEquiptypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipmentCategory.Add(
                    new GetEquiptypeViewModel()
                    {
                        edesc = Convert.ToString(dr["edesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetEquipmentCategory;
        }

        public DataSet getEquipShutdownReason(User objPropUser)
        {
            return objDL_User.getEquipShutdownReason(objPropUser);
        }

        public DataSet getMCPS(User objPropUser)
        {
            return objDL_User.getMCPS(objPropUser);
        }



        public DataSet getBuilding(User objPropUser)
        {
            return objDL_User.getBuilding(objPropUser);
        }

        public DataSet getBuildingElev(User objPropUser)
        {
            return objDL_User.getBuildingElev(objPropUser);
        }

        //API
        public List<GetBuildingElevViewModel> getBuildingElev(GetBuildingElevParam _GetBuildingElev, string ConnectionString)
        {
            DataSet ds = objDL_User.getBuildingElev(_GetBuildingElev, ConnectionString);

            List<GetBuildingElevViewModel> _lstGetBuildingElev = new List<GetBuildingElevViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBuildingElev.Add(
                    new GetBuildingElevViewModel()
                    {
                        EDesc = Convert.ToString(dr["EDesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }

            return _lstGetBuildingElev;
        }

        public DataSet getBuildingLeadEquip(User objPropUser)
        {
            return objDL_User.getBuildingLeadEquip(objPropUser);
        }

        //API
        public List<GetBuildingElevViewModel> getBuildingLeadEquip(GetBuildingLeadEquipParam _GetBuildingLeadEquip, string ConnectionString)
        {
            DataSet ds = objDL_User.getBuildingLeadEquip(_GetBuildingLeadEquip, ConnectionString);

            List<GetBuildingElevViewModel> _lstGetBuildingLeadEquip = new List<GetBuildingElevViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBuildingLeadEquip.Add(
                    new GetBuildingElevViewModel()
                    {
                        EDesc = Convert.ToString(dr["EDesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }

            return _lstGetBuildingLeadEquip;
        }


        public void AddSalesTax(User objPropUser)
        {
            objDL_User.AddSalesTax(objPropUser);
        }

        public void UpdateSalesTax(User objPropUser)
        {
            objDL_User.UpdateSalesTax(objPropUser);
        }

        public void AddQBSalesTax(User objPropUser)
        {
            objDL_User.AddQBSalesTax(objPropUser);
        }

        //API
        public void AddQBSalesTax(AddQBSalesTaxParam _AddQBSalesTax, string ConnectionString)
        {
            objDL_User.AddQBSalesTax(_AddQBSalesTax, ConnectionString);
        }

        public void AddBillCode(User objPropUser)
        {
            objDL_User.AddBillCode(objPropUser);
        }

        public void AddQBBillCode(User objPropUser)
        {
            objDL_User.AddQBBillCode(objPropUser);
        }

        public void InsertWareHouse(User objPropUser)
        {
            objDL_User.InsertWareHouse(objPropUser);
        }

        public void DeleteCustomerQB(User objPropUser)
        {
            objDL_User.DeleteCustomerQB(objPropUser);
        }

        public void UpdateBillCode(User objPropUser)
        {
            objDL_User.UpdateBillCode(objPropUser);
        }

        public void UpdateWarehouse(User objPropUser)
        {
            objDL_User.UpdateWarehouse(objPropUser);
        }

        public void DeleteDiagnostic(User objPropUser)
        {
            objDL_User.DeleteDiagnostic(objPropUser);
        }

        public DataSet getDepartment(User objPropUser)
        {
            return objDL_User.getDepartment(objPropUser);
        }

        public DataSet getPayFrequencies(User objPropUser)
        {
            return objDL_User.getPayFrequencies(objPropUser);
        }

        public DataSet fillSupervisor(User objPropUser)
        {
            return objDL_User.fillSupervisor(objPropUser);
        }
        
        //Get Title
        public DataSet getTitle(User objPropUser)
        {
            return objDL_User.getTitle(objPropUser);
        }

        //api
        public List<JobTypeViewModel> getDepartment(GetDepartmentParam _getDepartmentParam, string ConnectionString)
        {
            DataSet ds = objDL_User.getDepartment(_getDepartmentParam, ConnectionString);

            List<JobTypeViewModel> _jobtype = new List<JobTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _jobtype.Add(
                    new JobTypeViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Type = Convert.ToString(dr["Type"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        Color = Convert.ToInt32(DBNull.Value.Equals(dr["Color"]) ? 0 : dr["Color"]),
                        Remark = Convert.ToString(dr["Remarks"]),
                        IsDefault = Convert.ToInt32(DBNull.Value.Equals(dr["IsDefault"]) ? 0 : dr["IsDefault"]),
                        QBJobTypeID = Convert.ToString(dr["QBJobTypeID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        PrimarySyncID = Convert.ToInt32(DBNull.Value.Equals(dr["PrimarySyncID"]) ? 0 : dr["PrimarySyncID"])

                    }
                    );
            }

            return _jobtype;
        }

        public DataSet getCentral(User objPropUser)
        {
            return objDL_User.getCentral(objPropUser);
        }

        public DataSet getDepartmentPercent(User objPropUser)
        {
            return objDL_User.getDepartmentPercent(objPropUser);
        }

        public DataSet getSalesTax(User objPropUser)
        {
            return objDL_User.getSalesTax(objPropUser);
        }

        public DataSet getSalesTaxByTaxType(User objPropUser)
        {
            return objDL_User.getSalesTaxByTaxType(objPropUser);
        }

        public DataSet getMSMSalesTax(User objPropUser)
        {
            return objDL_User.getMSMSalesTax(objPropUser);
        }

        //API
        public List<GetMSMSalesTaxViewModel> getMSMSalesTax(GetMSMSalesTaxParam _GetMSMSalesTax, string ConnectionString)
        {
            DataSet ds = objDL_User.getMSMSalesTax(_GetMSMSalesTax, ConnectionString);

            List<GetMSMSalesTaxViewModel> _lstGetMSMSalesTax = new List<GetMSMSalesTaxViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetMSMSalesTax.Add(
                    new GetMSMSalesTaxViewModel()
                    {
                        IsTax = Convert.ToBoolean(DBNull.Value.Equals(dr["IsTax"]) ? false : dr["IsTax"]),
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Rate = Convert.ToDouble(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                        State = Convert.ToString(dr["State"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        GL = Convert.ToInt32(DBNull.Value.Equals(dr["GL"]) ? 0 : dr["GL"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        UType = Convert.ToInt16(DBNull.Value.Equals(dr["UType"]) ? 0 : dr["UType"]),
                        PSTReg = Convert.ToString(dr["PSTReg"]),
                        QBStaxID = Convert.ToInt16(DBNull.Value.Equals(dr["QBStaxID"]) ? 0 : dr["QBStaxID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        IsTaxable = Convert.ToBoolean(DBNull.Value.Equals(dr["IsTaxable"]) ? false : dr["IsTaxable"]),
                        QBvendorID = Convert.ToString(dr["QBvendorID"]),
                    }
                    );
            }

            return _lstGetMSMSalesTax;
        }

        public DataSet getQBSalesTax(User objPropUser)
        {
            return objDL_User.getQBSalesTax(objPropUser);
        }

        public DataSet getMSMLoctype(User objPropUser)
        {
            return objDL_User.getMSMLoctype(objPropUser);
        }

        //API
        public List<GetMSMLoctypeViewModel> getMSMLoctype(GetMSMLoctypeParam _GetMSMLoctype, string ConnectionString)
        {
            DataSet ds = objDL_User.getMSMLoctype(_GetMSMLoctype, ConnectionString);

            List<GetMSMLoctypeViewModel> _lstGetMSMLoctype = new List<GetMSMLoctypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetMSMLoctype.Add(
                    new GetMSMLoctypeViewModel()
                    {
                        Type = Convert.ToString(dr["Type"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        QBlocTypeID = Convert.ToString(dr["QBlocTypeID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                    }
                    );
            }

            return _lstGetMSMLoctype;
        }

        public DataSet getMSMDepartment(User objPropUser)
        {
            return objDL_User.getMSMDepartment(objPropUser);
        }

        public DataSet getMSMBillcode(User objPropUser)
        {
            return objDL_User.getMSMBillcode(objPropUser);
        }

        public DataSet getMSMterms(User objPropUser)
        {
            return objDL_User.getMSMterms(objPropUser);
        }

        public DataSet getMSMAccount(User objPropUser)
        {
            return objDL_User.getMSMAccount(objPropUser);
        }

        public DataSet getMSMPatrollWage(User objPropUser)
        {
            return objDL_User.getMSMPatrollWage(objPropUser);
        }

        public DataSet getMSMVendor(User objPropUser)
        {
            return objDL_User.getMSMVendor(objPropUser);
        }

        public DataSet getQBDepartment(User objPropUser)
        {
            return objDL_User.getQBDepartment(objPropUser);
        }

        public DataSet getMSMCustomertype(User objPropUser)
        {
            return objDL_User.getMSMCustomertype(objPropUser);
        }

        //API
        public List<GetMSMCustomertypeViewModel> getMSMCustomertype(GetMSMCustomertypeParam _GetMSMCustomertype, string ConnectionString)
        {
            DataSet ds = objDL_User.getMSMCustomertype(_GetMSMCustomertype, ConnectionString);

            List<GetMSMCustomertypeViewModel> _lstgetMSMCustomertype = new List<GetMSMCustomertypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstgetMSMCustomertype.Add(
                    new GetMSMCustomertypeViewModel()
                    {
                        Type = Convert.ToString(dr["Type"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        QBCustomerTypeID = Convert.ToString(dr["QBCustomerTypeID"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                    }
                    );
            }

            return _lstgetMSMCustomertype;

        }

        public DataSet getQBCustomertype(User objPropUser)
        {
            return objDL_User.getQBCustomertype(objPropUser);
        }

        public DataSet getSalesTaxByID(User objPropUser)
        {
            return objDL_User.getSalesTaxByID(objPropUser);
        }

        public DataSet getlocationType(User objPropUser)
        {
            return objDL_User.getlocationType(objPropUser);
        }

        //API
        public List<GetLocationTypeViewModel> getlocationType(getlocationTypeParam _getlocationType, string ConnectionString)
        {
            DataSet ds = objDL_User.getlocationType(_getlocationType, ConnectionString);

            List<GetLocationTypeViewModel> _lstgetlocationType = new List<GetLocationTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstgetlocationType.Add(
                    new GetLocationTypeViewModel()
                    {
                        Type = Convert.ToString(dr["Type"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstgetlocationType;
        }

        //consultant
        public DataSet getConsultant(tblConsult objPropConsult)
        {
            return objDL_User.getConsultant(objPropConsult);
        }

        public DataSet getSingleConsultant(tblConsult objPropConsult)
        {
            return objDL_User.getSingleConsultant(objPropConsult);
        }

        //API
        public List<GetSingleConsultantViewModel> getSingleConsultant(GetSingleConsultantParam _GetSingleConsultant, string ConnectionString)
        {
            DataSet ds = objDL_User.getSingleConsultant(_GetSingleConsultant, ConnectionString);

            List<GetSingleConsultantViewModel> _lstGetSingleConsultant = new List<GetSingleConsultantViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetSingleConsultant.Add(
                    new GetSingleConsultantViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        API = Convert.ToInt16(DBNull.Value.Equals(dr["API"]) ? 0 : dr["API"]),
                        Username = Convert.ToString(dr["Username"]),
                        Password = Convert.ToString(dr["Password"]),
                        IP = Convert.ToString(dr["IP"]),
                    });
            }
            return _lstGetSingleConsultant;
        }

        public DataSet getEMP(User objPropUser)
        {
            return objDL_User.getEMP(objPropUser);
        }

        //API
        public List<GetEMPViewModel> getEMP(GetEMPParam _GetEMP, string ConnectionString)
        {
            DataSet ds = objDL_User.getEMP(_GetEMP, ConnectionString);

            List<GetEMPViewModel> _lstGetEMP = new List<GetEMPViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEMP.Add(
                    new GetEMPViewModel()
                    {
                        fDesc = Convert.ToString(dr["fdesc"]),
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    }
                    );
            }

            return _lstGetEMP;
        }

        public int getEMPStatus(User objPropUser)
        {
            return objDL_User.getEMPStatus(objPropUser);
        }

        public DataSet getEMPwithDeviceID(User objPropUser)
        {
            return objDL_User.getEMPwithDeviceID(objPropUser);
        }

        //api
        public List<UserViewModel> getEMPwithDeviceID(getTimesheetParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getEMPwithDeviceID(objPropUser, ConnectionString);

            List<UserViewModel> _userViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new UserViewModel()
                    {
                        fDesc = (dr["fdesc"]).ToString(),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),

                    }
                    );
            }

            return _userViewModel;
        }

        public DataSet getEMPScheduler(User objPropUser)
        {
            return objDL_User.getEMPScheduler(objPropUser);
        }

        public DataSet getEMPSuper(User objPropUser)
        {
            return objDL_User.getEMPSuper(objPropUser);
        }
        //api
        public List<UserViewModel> getEMPSuper(getConnectionConfigParam _getConnectionConfigParam, string ConnectionString)
        {
            DataSet ds = objDL_User.getEMPSuper(_getConnectionConfigParam, ConnectionString);

            List<UserViewModel> _userViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new UserViewModel()
                    {
                        Super = (dr["Super"]).ToString(),
                    }
                    );
            }

            return _userViewModel;
        }

        public int getLoginSuper(User objPropUser)
        {
            return objDL_User.getLoginSuper(objPropUser);
        }

        //api
        public int getLoginSuper(AddUserParam objPropUser, string ConnectionString)
        {
            return objDL_User.getLoginSuper(objPropUser, ConnectionString);
        }

        public int getISSuper(User objPropUser)
        {
            return objDL_User.getISSuper(objPropUser);
        }

        //api
        public int getISSuper(AddUserParam objPropUser, string connectionString)
        {
            return objDL_User.getISSuper(objPropUser, connectionString);
        }

        public DataSet getAlltblWork(User objPropUser)
        {
            return objDL_User.getAlltblWork(objPropUser);
        }


        public DataSet getDatabases(User objPropUser)
        {
            return objDL_User.getDatabases(objPropUser);
        }

        public DataSet getAdminControlByID(User objPropUser)
        {
            return objDL_User.getAdminControlByID(objPropUser);
        }

        public DataSet CheckDB(User objPropUser)
        {
            return objDL_User.CheckDB(objPropUser);
        }

        public DataSet GetAdminPassword(User objPropUser)
        {
            return objDL_User.GetAdminPassword(objPropUser);
        }

        public int AddUser(User objPropUser, string createdBy)
        {
            return objDL_User.AddUser(objPropUser, createdBy);
        }

        public void AddUserCustom(User objPropUser)
        {
            objDL_User.UpdateUserCustomFieldsValue(objPropUser.ConnConfig, objPropUser.UserID, objPropUser.Cus_UserCustomValue);
        }

        public DataSet AddQBUser(User objPropUser)
        {
            return objDL_User.AddQBUser(objPropUser);
        }

        public DataSet getRoute(User objPropUser, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
        {
            return objDL_User.getRoute(objPropUser, IsActive, LocID, ContractID);
        }

        //API
        public ListGetRouteViewModel getRoute(GetRouteParam _GetRoute, string ConnectionString, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
        {
            DataSet ds = objDL_User.getRoute(_GetRoute, ConnectionString, IsActive, LocID, ContractID);

            ListGetRouteViewModel _ds = new ListGetRouteViewModel();
            List<GetRouteTable1> _lstTable1 = new List<GetRouteTable1>();
            List<GetRouteTable2> _lstTable2 = new List<GetRouteTable2>();
            List<GetRouteTable3> _lstTable3 = new List<GetRouteTable3>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetRouteTable1()
                    {
                        name = Convert.ToString(dr["name"]),
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        Color = Convert.ToString(dr["Color"]),
                        mechname = Convert.ToString(dr["mechname"]),
                        remarks = Convert.ToString(dr["remarks"]),
                        label = Convert.ToString(dr["label"]),
                    }
                    );
            }

            _ds.lstTable1 = _lstTable1;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetRouteTable2()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                    }
                    );
            }

            _ds.lstTable2 = _lstTable2;

            if (ds.Tables.Count == 3)
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    _lstTable3.Add(
                        new GetRouteTable3()
                        {
                            fUser = Convert.ToString(dr["fUser"]),
                            ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        }
                        );
                }

                _ds.lstTable3 = _lstTable3;
            }

            return _ds;
        }

        public DataSet getRouteActive(User objPropUser)
        {
            try
            {
                return objDL_User.getRouteActive(objPropUser);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public List<GetRouteActiveViewModel> getRouteActive(GetRouteActiveParam _GetRouteActive, string ConnectionString)
        {
            try
            {
                DataSet ds = objDL_User.getRouteActive(_GetRouteActive, ConnectionString);

                List<GetRouteActiveViewModel> _lstGetRouteActive = new List<GetRouteActiveViewModel>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    _lstGetRouteActive.Add(
                        new GetRouteActiveViewModel()
                        {
                            ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                            Name = Convert.ToString(dr["Name"]),
                            Mech = Convert.ToInt32(DBNull.Value.Equals(dr["Mech"]) ? 0 : dr["Mech"]),
                            Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                            Elev = Convert.ToInt32(DBNull.Value.Equals(dr["Elev"]) ? 0 : dr["Elev"]),
                            Hour = Convert.ToDouble(DBNull.Value.Equals(dr["Hour"]) ? 0 : dr["Hour"]),
                            Amount = Convert.ToDouble(DBNull.Value.Equals(dr["Amount"]) ? 0 : dr["Amount"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Symbol = Convert.ToInt16(DBNull.Value.Equals(dr["Symbol"]) ? 0 : dr["Symbol"]),
                            EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                            Color = Convert.ToString(dr["Color"]),
                            Status = Convert.ToBoolean(DBNull.Value.Equals(dr["Status"]) ? false : dr["Status"]),
                            MechName = Convert.ToString(dr["MechName"]),
                        }
                        );
                }

                return _lstGetRouteActive;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getRoutesGrid(User objPropUser, int IsActive = 0, Int32 LocID = 0, Int32 ContractID = 0)
        {
            return objDL_User.getRoutesGrid(objPropUser, IsActive, LocID, ContractID);
        }

        public DataSet getTerritory(User objPropUser, Int32 IsSalesAsigned = 0, int EstimateID = 0, int OpportunityID = 0, string orderby = "t.name")
        {
            return objDL_User.getTerritory(objPropUser, IsSalesAsigned, EstimateID, OpportunityID, orderby);

        }

        public DataSet GetSalesPerson(User objPropUser, Int32 IsSalesAsigned = 0, int refId = 0, string screen = "", string orderby = "t.name")
        {
            return objDL_User.GetSalesPerson(objPropUser, IsSalesAsigned, refId, screen, orderby);

        }

        //API
        public List<GetTerritoryViewModel> getTerritory(GetTerritoryParam _GetTerritory, string ConnectionString, Int32 IsSalesAsigned = 0, int EstimateID = 0, int OpportunityID = 0)
        {
            DataSet ds = objDL_User.getTerritory(_GetTerritory, ConnectionString, IsSalesAsigned, EstimateID, OpportunityID);

            List<GetTerritoryViewModel> _lstGetTerritory = new List<GetTerritoryViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetTerritory.Add(
                    new GetTerritoryViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        SDesc = Convert.ToString(dr["SDesc"]),
                    }
                    );
            }

            return _lstGetTerritory;
        }

        public DataSet GetAllTerritory(User objPropUser)
        {
            return objDL_User.GetAllTerritory(objPropUser);

        }

        //API
        public List<GetTerritoryViewModel> GetAllTerritory(GetAllTerritoryParam _GetAllTerritory, string ConnectionString)
        {
            DataSet ds = objDL_User.GetAllTerritory(_GetAllTerritory, ConnectionString);
            List<GetTerritoryViewModel> _lstGetAllTerritory = new List<GetTerritoryViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllTerritory.Add(
                    new GetTerritoryViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        SDesc = Convert.ToString(dr["SDesc"]),
                    }
                    );
            }

            return _lstGetAllTerritory;
        }
        public void AddCustomer(User objPropUser)
        {
            objDL_User.AddCustomer(objPropUser);
        }

        //API
        public void AddCustomer(AddCustomerParam _AddCustomer, string ConnectionString)
        {
            objDL_User.AddCustomer(_AddCustomer, ConnectionString);
        }
        public int AddRol(User objPropUser)
        {
            return objDL_User.AddRol(objPropUser);
        }
        //consult
        public DataSet IsConsultNameExist(User objPropUser)
        {
            return objDL_User.IsConsultNameExist(objPropUser);
        }
        public void AddConsult(tblConsult objPropConsult)
        {
            objDL_User.AddConsult(objPropConsult);
        }
        public void AddCustomerQB(User objPropUser)
        {
            objDL_User.AddCustomerQB(objPropUser);
        }

        //API
        public void AddCustomerQB(AddCustomerQBParam _AddCustomerQB, string ConnectionString)
        {
            objDL_User.AddCustomerQB(_AddCustomerQB, ConnectionString);
        }

        public void AddCustomerQBMapping(User objPropUser)
        {
            objDL_User.AddCustomerQBMapping(objPropUser);
        }

        public int AddCustomerSage(User objPropUser)
        {
            return objDL_User.AddCustomerSage(objPropUser);
        }

        public void AddCustomertest(User objPropUser)
        {
            objDL_User.AddCustomertest(objPropUser);
        }

        public Int32 AddEquipment(User objPropUser)
        {
            return objDL_User.AddEquipment(objPropUser);
        }

        //API
        public Int32 AddEquipment(AddEquipmentParam _AddEquipment, string ConnectionString)
        {
            return objDL_User.AddEquipment(_AddEquipment, ConnectionString);
        }

        public Int32 AddEquipmentForLead(User objPropUser)
        {
            return objDL_User.AddEquipmentForLead(objPropUser);
        }

        //API
        public Int32 AddEquipmentForLead(AddEquipmentForLeadParam _AddEquipmentForLead, string ConnectionString)
        {
            return objDL_User.AddEquipmentForLead(_AddEquipmentForLead, ConnectionString);
        }


        public void AddEquipmentImport(User objPropUser)
        {
            objDL_User.AddEquipmentImport(objPropUser);
        }
        public void DeleteRoutes(User objPropUser)
        {
            objDL_User.DeleteRoutes(objPropUser);
        }
        public void DeleteCompany(User objPropUser)
        {
            objDL_User.DeleteCompany(objPropUser);
        }

        public void AddCompany(User objPropUser)
        {
            objDL_User.AddCompany(objPropUser);
        }

        public DataSet getTerms(User objPropUser)
        {
            return objDL_User.getTerms(objPropUser);
        }

        public List<TermsViewModel> getTerms(GetTermsParam _GetTermsParam, string ConnectionString)
        {
            DataSet ds = objDL_User.getTerms(_GetTermsParam, ConnectionString);
            List<TermsViewModel> _lstTermsViewModel = new List<TermsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTermsViewModel.Add(
                    new TermsViewModel()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        name = Convert.ToString(dr["Name"]),
                        QBTermsID = Convert.ToString(dr["QBTermsID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                    }
                    );
            }

            return _lstTermsViewModel;
        }

        public void UpdateCompany(User objPropUser)
        {
            objDL_User.UpdateCompany(objPropUser);
        }

        //User Add Log
        public void AddUserLog(User objPropUser)
        {
            objDL_User.AddUserLog(objPropUser);
        }

        public void UpdateControl(User objPropUser)
        {
            objDL_User.UpdateControl(objPropUser);
        }
        /// <summary>
        /// UpdateControlProjectDefaults
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <param name="ContactType"></param>
        /// 
        public void UpdateControlProjectDefaults(User objPropUser, int ContactType)
        {
            objDL_User.UpdateControlProjectDefaults(objPropUser, ContactType);
        }

        public void CreateDatabase(User objPropUser)
        {
            objDL_User.CreateDatabase(objPropUser);
        }

        public void AddCustomerType(User objPropUser)
        {
            objDL_User.AddCustomerType(objPropUser);
        }
        public void AddVendorType(User objPropUser)
        {
            objDL_User.AddVendorType(objPropUser);
        }
        public void AddQBCustomerType(User objPropUser)
        {
            objDL_User.AddQBCustomerType(objPropUser);
        }

        //API
        public void AddQBCustomerType(AddQBCustomerTypeParam _AddQBCustomerType, string ConnectionString)
        {
            objDL_User.AddQBCustomerType(_AddQBCustomerType, ConnectionString);
        }


        public void AddQBLocType(User objPropUser)
        {
            objDL_User.AddQBLocType(objPropUser);
        }

        //API
        public void AddQBLocType(AddQBLocTypeParam _AddQBLocType, string ConnectionString)
        {
            objDL_User.AddQBLocType(_AddQBLocType, ConnectionString);
        }


        public void AddCategory(User objPropUser)
        {
            objDL_User.AddCategory(objPropUser);
        }

        public void AddEquipType(User objPropUser)
        {
            objDL_User.AddEquipType(objPropUser);
        }
        public void AddEquipBuilding(User objPropUser)
        {
            objDL_User.AddEquipBuilding(objPropUser);
        }
        public void AddEquipCateg(User objPropUser)
        {
            objDL_User.AddEquipCateg(objPropUser);
        }

        public void AddMCPS(User objPropUser)
        {
            objDL_User.AddMCPS(objPropUser);
        }
        public void AddServiceType(string ConnConfig, string TYPE, string FDESC, string REMARKS, int REG, int OT, int NT, int DT, int STATUS, string LocType, int ExpenseGL, int InterestGL, int LaborWageC, int InvID, string route, string strddldepartment)
        {
            objDL_User.AddServiceType(ConnConfig, TYPE, FDESC, REMARKS, REG, OT, NT, DT, STATUS, LocType, ExpenseGL, InterestGL, LaborWageC, InvID, route, strddldepartment);
        }

        public void UpdateServiceType(string ConnConfig, string TYPE, string FDESC, string REMARKS, int REG, int OT, int NT, int DT, int STATUS, string LocType, int ExpenseGL, int InterestGL, int LaborWageC, int InvID, string route, int Flage, string strddldepartment, string userName)
        {
            objDL_User.UpdateServiceType(ConnConfig, TYPE, FDESC, REMARKS, REG, OT, NT, DT, STATUS, LocType, ExpenseGL, InterestGL, LaborWageC, InvID, route, Flage, strddldepartment, userName);
        }

        public DataSet GetServiceType(string ConnConfig, string ServiceType)
        {
            try
            {

                return objDL_User.GetServiceType(ConnConfig, ServiceType);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddLocationType(User objPropUser)
        {
            objDL_User.AddLocationType(objPropUser);
        }
        public void AddZone(User objPropUser)
        {
            objDL_User.AddZone(objPropUser);
        }
        public void UpdateZone(User objPropUser)
        {
            objDL_User.UpdateZone(objPropUser);
        }
        public void DeleteZone(User objPropUser)
        {
            objDL_User.DeleteZone(objPropUser);
        }
        public void AddTicketInvoiceEmail(User objPropUser)
        {
            objDL_User.AddTicketInvoiceEmail(objPropUser);
        }
        public void UpdateTicketInvoiceEmail(User objPropUser)
        {
            objDL_User.UpdateTicketInvoiceEmail(objPropUser);
        }
        public DataSet getTicketInvoiceEmail(User objPropUser)
        {
            return objDL_User.getTicketInvoiceEmail(objPropUser);
        }
        public void UpdateCustomerType(User objPropUser)
        {
            objDL_User.UpdateCustomerType(objPropUser);
        }
        public void UpdateVendorType(User objPropUser)
        {
            objDL_User.UpdateVendorType(objPropUser);
        }
        public void UpdateCategory(User objPropUser)
        {
            objDL_User.UpdateCategory(objPropUser);
        }

        public void UpdateLocationType(User objPropUser)
        {
            objDL_User.UpdateLocationType(objPropUser);
        }

        //update consultant
        public void UpdateConsultant(User objPropUser, tblConsult objPropConsult)
        {
            objDL_User.UpdateConsultant(objPropUser, objPropConsult);
        }

        public void DeleteCustomerType(User objPropUser)
        {
            objDL_User.DeleteCustomerType(objPropUser);
        }
        public void DeleteVendorType(User objPropUser)
        {
            objDL_User.DeleteVendorType(objPropUser);
        }
        public void DeleteCustomerTypeByListID(User objPropUser)
        {
            objDL_User.DeleteCustomerTypeByListID(objPropUser);
        }

        public void DeleteCategory(User objPropUser)
        {
            objDL_User.DeleteCategory(objPropUser);
        }
        public void DeleteEquiptype(User objPropUser)
        {
            objDL_User.DeleteEquiptype(objPropUser);
        }
        public void DeleteEquipBuilding(User objPropUser)
        {
            objDL_User.DeleteEquipBuilding(objPropUser);
        }
        public void DeleteEquipCateg(User objPropUser)
        {
            objDL_User.DeleteEquipCateg(objPropUser);
        }
        public void DeleteMCPS(User objPropUser)
        {
            objDL_User.DeleteMCPS(objPropUser);
        }
        public void DeleteServicetype(User objPropUser)
        {
            objDL_User.DeleteServicetype(objPropUser);
        }

        public void DeleteSalesTax(User objPropUser)
        {
            objDL_User.DeleteSalesTax(objPropUser);
        }

        public void DeleteSalesTaxByListID(User objPropUser)
        {
            objDL_User.DeleteSalesTaxByListID(objPropUser);
        }

        public void DeleteDepartment(User objPropUser)
        {
            objDL_User.DeleteDepartment(objPropUser);
        }

        public void DeleteDepartmentByListID(User objPropUser)
        {
            objDL_User.DeleteDepartmentByListID(objPropUser);
        }

        public void DeleteLocType(User objPropUser)
        {
            objDL_User.DeleteLocType(objPropUser);
        }

        //delete consultant
        public void DeleteConsultant(tblConsult objPropConsult)
        {
            objDL_User.DeleteConsultant(objPropConsult);
        }

        public void DeleteLocTypeByListID(User objPropUser)
        {
            objDL_User.DeleteLocTypeByListID(objPropUser);
        }
        public void DeleteContactByID(User objPropUser)
        {
            objDL_User.DeleteContactByID(objPropUser);
        }

        public void AddDepartment(User objPropUser)
        {
            objDL_User.AddDepartment(objPropUser);
        }

        public void AddWage(Wage _objWage)
        {
            objDL_User.AddWage(_objWage);
        }

        public void UpdateDepartment(User objPropUser)
        {
            objDL_User.UpdateDepartment(objPropUser);
        }

        public void UpdateWage(Wage _objWage)
        {
            objDL_User.UpdateWage(_objWage);
        }

        public void AddQBDepartment(User objPropUser)
        {
            objDL_User.AddQBDepartment(objPropUser);
        }

        public void AddQBTerms(User objPropUser)
        {
            objDL_User.AddQBTerms(objPropUser);
        }

        public void AddQBPayrollWage(User objPropUser)
        {
            objDL_User.AddQBPayrollWage(objPropUser);
        }

        public void AddDatabaseName(User objPropUser)
        {
            objDL_User.AddDatabaseName(objPropUser);
        }

        public void UpdateDatabaseName(User objPropUser)
        {
            objDL_User.UpdateDatabaseName(objPropUser);
        }


        public void UpdateAdminPassword(User objPropUser)
        {
            objDL_User.UpdateAdminPassword(objPropUser);
        }

        /// <summary>
        /// UpdateLocGeocode
        /// </summary>
        /// <param name="_objLoc"></param>
        /// <returns></returns>
        public void spUpdateLocGeocode(Loc _objLoc)
        {
            objDL_User.spUpdateLocGeocode(_objLoc);
        }
        public int AddLocation(User objPropUser)
        {
            return objDL_User.AddLocation(objPropUser);
        }

        //API
        public int AddLocation(AddLocationParam _AddLocation, string ConnectionString)
        {
            return objDL_User.AddLocation(_AddLocation, ConnectionString);
        }


        public void AddQBLocation(User objPropUser)
        {
            objDL_User.AddQBLocation(objPropUser);
        }

        //API
        public void AddQBLocation(AddQBLocationParam _AddQBLocation, string ConnectionString)
        {
            objDL_User.AddQBLocation(_AddQBLocation, ConnectionString);
        }

        public void AddQBLocationMapping(User objPropUser)
        {
            objDL_User.AddQBLocationMapping(objPropUser);
        }

        public void UpdateLocation(User objPropUser, bool CopyToLocAndJob = false, int ApplyServiceTypeRule = 0, string ServiceTypeName = "", int ProjectPerDepartmentCount = 0)
        {
            objDL_User.UpdateLocation(objPropUser, CopyToLocAndJob, ApplyServiceTypeRule, ServiceTypeName, ProjectPerDepartmentCount);
        }

        public void Update_Loc_Terr(User objPropUser)
        {
            objDL_User.Update_Loc_Terr(objPropUser);
        }
        
        //API
        public void UpdateLocation(UpdateLocationParam _UpdateLocation, string ConnectionString, bool CopyToLocAndJob = false, int ApplyServiceTypeRule = 0, string ServiceTypeName = "", int ProjectPerDepartmentCount = 0)
        {
            objDL_User.UpdateLocation(_UpdateLocation, ConnectionString, CopyToLocAndJob, ApplyServiceTypeRule, ServiceTypeName, ProjectPerDepartmentCount);
        }

        public void UpdateCustomer(User objPropUser, bool CopyToLocAndJob = false)
        {
            objDL_User.UpdateCustomer(objPropUser, CopyToLocAndJob);
        }

        //API
        public void UpdateCustomer(UpdateCustomerParam _UpdateCustomer, bool CopyToLocAndJob, string ConnectionString)
        {
            objDL_User.UpdateCustomer(_UpdateCustomer, CopyToLocAndJob, ConnectionString);
        }

        public void UpdateEquipment(User objPropUser)
        {
            objDL_User.UpdateEquipment(objPropUser);
        }

        //API
        public void UpdateEquipment(UpdateEquipmentParam _UpdateEquipment, string ConnectionString)
        {
            objDL_User.UpdateEquipment(_UpdateEquipment, ConnectionString);
        }

        public void UpdateLeadEquipment(User objPropUser)
        {
            objDL_User.UpdateLeadEquipment(objPropUser);
        }

        //API
        public void UpdateLeadEquipment(UpdateLeadEquipmentParam _UpdateLeadEquipment, string ConnectionString)
        {
            objDL_User.UpdateLeadEquipment(_UpdateLeadEquipment, ConnectionString);
        }

        public void AddMassMCP(User objPropUser)
        {
            objDL_User.AddMassMCP(objPropUser);
        }

        //API
        public void AddMassMCP(AddMassMCPParam _AddMassMCP, string ConnectionString)
        {
            objDL_User.AddMassMCP(_AddMassMCP, ConnectionString);
        }

        public void UpdateCustomerContact(User objPropUser)
        {
            objDL_User.UpdateCustomerContact(objPropUser);
        }
        public void UpdateLocPrintEmail(User objPropUser)
        {
            objDL_User.UpdateLocPrintEmail(objPropUser);
        }

        public void UpdateLocCustom12(User objPropUser)
        {
            objDL_User.UpdateLocCustom12(objPropUser);
        }
        public DataSet GetLocByID(User objPropUser)
        {
            return objDL_User.GetLocByID(objPropUser);
        }
        public void UpdateLocationContact(User objPropUser)
        {
            objDL_User.UpdateLocationContact(objPropUser);
        }
        public void UpdateCollectionContact(CollectionContacts objPropUser)
        {
            objDL_User.UpdateCollectionContact(objPropUser);
        }
        public void AddContactFromProjectScreen(User objPropUser, string ContactType, int ContactTypeID, int JobID)
        {
            objDL_User.AddContactFromProjectScreen(objPropUser, ContactType, ContactTypeID, JobID);
        }

        public void UpdateUser(User objPropUser, string UpdatedBy)
        {
            objDL_User.UpdateUser(objPropUser, UpdatedBy);
        }

        public void UpdateUserProfile(User objPropUser)
        {
            objDL_User.UpdateUserProfile(objPropUser);
        }

        public void UpdateUserAvatar(User objPropUser)
        {
            objDL_User.UpdateUserAvatar(objPropUser);
        }

        public void UpdateUserCoverImage(User objPropUser)
        {
            objDL_User.UpdateUserCoverImage(objPropUser);
        }

        //public void UpdateUserCustomerAvatar(User objPropUser)
        //{
        //    objDL_User.UpdateUserCustomerAvatar(objPropUser);
        //}

        //public void UpdateUserCustomerCoverImage(User objPropUser)
        //{
        //    objDL_User.UpdateUserCustomerCoverImage(objPropUser);
        //}

        public void UpdateUserCustomerProfile(User objPropUser)
        {
            objDL_User.UpdateUserCustomerProfile(objPropUser);
        }


        public void UpdateTSUser(User objPropUser)
        {
            objDL_User.UpdateTSUser(objPropUser);
        }

        public void UpdateQBCustomerID(User objPropUser)
        {
            objDL_User.UpdateQBCustomerID(objPropUser);
        }

        //API
        public void UpdateQBCustomerID(UpdateQBCustomerIDParam _UpdateQBCustomerID, string ConnectionString)
        {
            objDL_User.UpdateQBCustomerID(_UpdateQBCustomerID, ConnectionString);
        }

        public void UpdateQBsalestaxID(User objPropUser)
        {
            objDL_User.UpdateQBsalestaxID(objPropUser);
        }

        //API
        public void UpdateQBsalestaxID(UpdateQBsalestaxIDParam _UpdateQBsalestaxID, string ConnectionString)
        {
            objDL_User.UpdateQBsalestaxID(_UpdateQBsalestaxID, ConnectionString);
        }

        public void UpdateQBDepartmentID(User objPropUser)
        {
            objDL_User.UpdateQBDepartmentID(objPropUser);
        }

        public void UpdateQBInvID(User objPropUser)
        {
            objDL_User.UpdateQBInvID(objPropUser);
        }

        public void UpdateQBTermsID(User objPropUser)
        {
            objDL_User.UpdateQBTermsID(objPropUser);
        }

        public void UpdateQBAccountID(User objPropUser)
        {
            objDL_User.UpdateQBAccountID(objPropUser);
        }

        public void UpdateQBVendorID(User objPropUser)
        {
            objDL_User.UpdateQBVendorID(objPropUser);
        }

        public void UpdateQBWageID(User objPropUser)
        {
            objDL_User.UpdateQBWageID(objPropUser);
        }

        public void UpdateQBJobtypeID(User objPropUser)
        {
            objDL_User.UpdateQBJobtypeID(objPropUser);
        }

        //API
        public void UpdateQBJobtypeID(UpdateQBJobtypeIDParam _UpdateQBJobtypeID, string ConnectionString)
        {
            objDL_User.UpdateQBJobtypeID(_UpdateQBJobtypeID, ConnectionString);
        }


        public void UpdateQBcustomertypeID(User objPropUser)
        {
            objDL_User.UpdateQBcustomertypeID(objPropUser);
        }

        //API
        public void UpdateQBcustomertypeID(UpdateQBcustomertypeIDParam _UpdateQBcustomertypeID, string ConnectionString)
        {
            objDL_User.UpdateQBcustomertypeID(_UpdateQBcustomertypeID, ConnectionString);
        }

        public void UpdateQBLocationID(User objPropUser)
        {
            objDL_User.UpdateQBLocationID(objPropUser);
        }

        //API
        public void UpdateQBLocationID(UpdateQBLocationIDParam _UpdateQBLocationID, string ConnectionString)
        {
            objDL_User.UpdateQBLocationID(_UpdateQBLocationID, ConnectionString);
        }

        public void CreateDBObjects(User objPropUser)
        {
            objDL_User.CreateDBObjects(objPropUser);
        }

        public DataSet getUser(User objPropUser)
        {
            return objDL_User.getUser(objPropUser);
        }

        public bool getPRUserByID(User objPropUser)
        {
            return objDL_User.getPRUserByID(objPropUser);
        }

        public DataSet getUserForSupervisor(User objPropUser)
        {
            return objDL_User.getUserForSupervisor(objPropUser);
        }

        //api
        public List<SuperUserViewModel> getUserForSupervisor(AddUserParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getUserForSupervisor(objPropUser, ConnectionString);

            List<SuperUserViewModel> _superuserViewModel = new List<SuperUserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _superuserViewModel.Add(
                    new SuperUserViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fFirst = Convert.ToString(dr["fFirst"]),
                        Last = Convert.ToString(dr["fFirst"]),
                        userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
                        fUser = Convert.ToString(dr["fUser"]),
                        Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        super = Convert.ToString(dr["super"]),
                        usertype = Convert.ToString(dr["usertype"]),
                        usertypeid = Convert.ToInt32(DBNull.Value.Equals(dr["usertypeid"]) ? 0 : dr["usertypeid"]),
                        userkey = Convert.ToString(dr["userkey"])

                    }
                    );
            }
            return _superuserViewModel;
        }

        public DataSet getSelectedUser(User objPropUser)
        {
            return objDL_User.getSelectedUser(objPropUser);
        }


        public DataSet getUsersSuper(User objPropUser)
        {
            return objDL_User.getUsersSuper(objPropUser);
        }

        public DataSet getSupervisor(User objPropUser)
        {
            return objDL_User.getSupervisor(objPropUser);
        }

        //Get SupperVisor
        public DataSet getSupervisorActive(User objPropUser)
        {
            return objDL_User.getSupervisorActive(objPropUser);
        }

        public DataSet getUnassignedCalls(User objPropUser)
        {
            return objDL_User.getUnassignedCalls(objPropUser);
        }

        public DataSet getOpenCallsMapScreen(User objPropUser)
        {
            return objDL_User.getOpenCallsMapScreen(objPropUser);
        }
        public DataSet getOpenCallsOnMap(User objPropUser)
        {
            return objDL_User.getOpenCallsOnMap(objPropUser);
        }
        /// <summary>
        /// This function for MOM4 and MOM5 db also
        /// Please don't update this
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <returns></returns>
        public DataSet GetControlForQB(User objPropUser)
        {
            return objDL_User.GetControlForQB(objPropUser);
        }


        public DataSet getControl(User objPropUser)
        {
            return objDL_User.getControl(objPropUser);
        }

        //api
        public List<GetControlViewModel> getControl(getConnectionConfigParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getControl(objPropUser, ConnectionString);
            //DataColumnCollection columns = ds.Tables[0].Columns;

            List<GetControlViewModel> _lstGetControlViewModel = new List<GetControlViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                #region comment
                //GetControlViewModel obj = new GetControlViewModel();
                //if (CommonMethods.ContainColumn("Name", ds.Tables[0]))
                //    obj.Name = Convert.ToString(dr["Name"]);
                //if (CommonMethods.ContainColumn("City", ds.Tables[0]))
                //    obj.City = Convert.ToString(dr["City"]);
                //if (CommonMethods.ContainColumn("State", ds.Tables[0]))
                //    obj.State = Convert.ToString(dr["State"]);
                //if (CommonMethods.ContainColumn("Zip", ds.Tables[0]))
                //    obj.Zip = Convert.ToString(dr["Zip"]);
                //if (CommonMethods.ContainColumn("Phone", ds.Tables[0]))
                //    obj.Phone = Convert.ToString(dr["Phone"]);
                //if (CommonMethods.ContainColumn("Fax", ds.Tables[0]))
                //    obj.Fax = Convert.ToString(dr["Fax"]);
                //if (CommonMethods.ContainColumn("fLong", ds.Tables[0]))
                //    obj.fLong = Convert.ToInt32(DBNull.Value.Equals(dr["fLong"]) ? 0 : dr["fLong"]);
                //if (CommonMethods.ContainColumn("Latt", ds.Tables[0]))
                //    obj.Latt = Convert.ToInt32(DBNull.Value.Equals(dr["Latt"]) ? 0 : dr["Latt"]);
                //if (CommonMethods.ContainColumn("GeoLock", ds.Tables[0]))
                //    obj.GeoLock = Convert.ToInt32(DBNull.Value.Equals(dr["GeoLock"]) ? 0 : dr["GeoLock"]);
                //if (CommonMethods.ContainColumn("Year", ds.Tables[0]))
                //    obj.Year = Convert.ToInt32(DBNull.Value.Equals(dr["Year"]) ? 0 : dr["Year"]);
                //if (CommonMethods.ContainColumn("Version", ds.Tables[0]))
                //    obj.Version = Convert.ToInt32(DBNull.Value.Equals(dr["Version"]) ? 0 : dr["Version"]);
                //if (CommonMethods.ContainColumn("CDesc", ds.Tables[0]))
                //    obj.CDesc = Convert.ToString(dr["CDesc"]);
                //if (CommonMethods.ContainColumn("Build", ds.Tables[0]))
                //    obj.Build = Convert.ToInt32(DBNull.Value.Equals(dr["Build"]) ? 0 : dr["Build"]);
                //if (CommonMethods.ContainColumn("Minor", ds.Tables[0]))
                //    obj.Minor = Convert.ToInt32(DBNull.Value.Equals(dr["Minor"]) ? 0 : dr["Minor"]);
                //if (CommonMethods.ContainColumn("Address", ds.Tables[0]))
                //    obj.Address = Convert.ToString(dr["Address"]);
                //if (CommonMethods.ContainColumn("AgeRemark", ds.Tables[0]))
                //    obj.AgeRemark = Convert.ToString(dr["AgeRemark"]);
                //if (CommonMethods.ContainColumn("SDate", ds.Tables[0]))
                //    obj.SDate = Convert.ToString(dr["SDate"]);
                //if (CommonMethods.ContainColumn("EDate", ds.Tables[0]))
                //    obj.EDate = Convert.ToString(dr["EDate"]);
                //if (CommonMethods.ContainColumn("YDate", ds.Tables[0]))
                //    obj.YDate = Convert.ToString(dr["YDate"]);
                //if (CommonMethods.ContainColumn("GSTreg", ds.Tables[0]))
                //    obj.GSTreg = Convert.ToString(dr["GSTreg"]);
                //if (CommonMethods.ContainColumn("IDesc", ds.Tables[0]))
                //    obj.IDesc = Convert.ToString(dr["IDesc"]);
                //if (CommonMethods.ContainColumn("PortalsID", ds.Tables[0]))
                //    obj.PortalsID = Convert.ToInt32(DBNull.Value.Equals(dr["PortalsID"]) ? 0 : dr["PortalsID"]);
                //if (CommonMethods.ContainColumn("PrContractRemark", ds.Tables[0]))
                //    obj.PrContractRemark = Convert.ToString(dr["PrContractRemark"]);
                //if (CommonMethods.ContainColumn("RepUser", ds.Tables[0]))
                //    obj.RepUser = Convert.ToString(dr["RepUser"]);
                //if (CommonMethods.ContainColumn("RepTitle", ds.Tables[0]))
                //    obj.RepTitle = Convert.ToString(dr["RepTitle"]);
                //if (CommonMethods.ContainColumn("LogoPath", ds.Tables[0]))
                //    obj.LogoPath = Convert.ToString(dr["LogoPath"]);
                //if (CommonMethods.ContainColumn("ExeBuildDate_Max", ds.Tables[0]))
                //    obj.ExeBuildDate_Max = Convert.ToDateTime(DBNull.Value.Equals(dr["ExeBuildDate_Max"]) ? null : dr["ExeBuildDate_Max"]);
                //if (CommonMethods.ContainColumn("ExeBuildDate_Min", ds.Tables[0]))
                //    obj.ExeBuildDate_Min = Convert.ToDateTime(DBNull.Value.Equals(dr["ExeBuildDate_Min"]) ? null : dr["ExeBuildDate_Min"]);
                //if (CommonMethods.ContainColumn("ExeVersion_Min", ds.Tables[0]))
                //    obj.ExeVersion_Min = Convert.ToString(dr["ExeVersion_Min"]);
                //if (CommonMethods.ContainColumn("ExeVersion_Max", ds.Tables[0]))
                //    obj.ExeVersion_Max = Convert.ToString(dr["ExeVersion_Max"]);
                //if (CommonMethods.ContainColumn("MerchantServicesConfig", ds.Tables[0]))
                //    obj.MerchantServicesConfig = Convert.ToString(dr["MerchantServicesConfig"]);
                //if (CommonMethods.ContainColumn("Email", ds.Tables[0]))
                //    obj.Email = Convert.ToString(dr["Email"]);
                //if (CommonMethods.ContainColumn("WebAddress", ds.Tables[0]))
                //    obj.WebAddress = Convert.ToString(dr["WebAddress"]);
                //if (CommonMethods.ContainColumn("MSM", ds.Tables[0]))
                //    obj.MSM = Convert.ToString(dr["MSM"]);
                //if (CommonMethods.ContainColumn("DSN", ds.Tables[0]))
                //    obj.DSN = Convert.ToString(dr["DSN"]);
                //if (CommonMethods.ContainColumn("Username", ds.Tables[0]))
                //    obj.Username = Convert.ToString(dr["Username"]);
                //if (CommonMethods.ContainColumn("Password", ds.Tables[0]))
                //    obj.Password = Convert.ToString(dr["Password"]);
                //if (CommonMethods.ContainColumn("DBName", ds.Tables[0]))
                //    obj.DBName = Convert.ToString(dr["DBName"]);
                //if (CommonMethods.ContainColumn("Remarks", ds.Tables[0]))
                //    obj.Remarks = Convert.ToString(dr["Remarks"]);
                //if (CommonMethods.ContainColumn("Map", ds.Tables[0]))
                //    obj.Map = Convert.ToInt32(DBNull.Value.Equals(dr["Map"]) ? 0 : dr["Map"]);
                //if (CommonMethods.ContainColumn("Custweb", ds.Tables[0]))
                //    obj.Custweb = Convert.ToInt32(DBNull.Value.Equals(dr["Custweb"]) ? 0 : dr["Custweb"]);
                //if (CommonMethods.ContainColumn("QBPath", ds.Tables[0]))
                //    obj.QBPath = Convert.ToString(dr["QBPath"]);
                //if (CommonMethods.ContainColumn("MultiLang", ds.Tables[0]))
                //    obj.MultiLang = Convert.ToInt32(DBNull.Value.Equals(dr["MultiLang"]) ? 0 : dr["MultiLang"]);
                //if (CommonMethods.ContainColumn("QBIntegration", ds.Tables[0]))
                //    obj.QBIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["QBIntegration"]) ? 0 : dr["QBIntegration"]);
                //if (CommonMethods.ContainColumn("QBLastSync", ds.Tables[0]))
                //    obj.QBLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["QBLastSync"]) ? null : dr["QBLastSync"]);
                //if (CommonMethods.ContainColumn("MSSignTime", ds.Tables[0]))
                //    obj.MSSignTime = Convert.ToInt32(DBNull.Value.Equals(dr["MSSignTime"]) ? 0 : dr["MSSignTime"]);
                //if (CommonMethods.ContainColumn("GrossInc", ds.Tables[0]))
                //    obj.GrossInc = Convert.ToInt32(DBNull.Value.Equals(dr["GrossInc"]) ? 0 : dr["GrossInc"]);
                //if (CommonMethods.ContainColumn("Month", ds.Tables[0]))
                //    obj.Month = Convert.ToInt32(DBNull.Value.Equals(dr["Month"]) ? 0 : dr["Month"]);
                //if (CommonMethods.ContainColumn("SalesAnnual", ds.Tables[0]))
                //    obj.SalesAnnual = Convert.ToInt32(DBNull.Value.Equals(dr["SalesAnnual"]) ? 0 : dr["SalesAnnual"]);
                //if (CommonMethods.ContainColumn("Payment", ds.Tables[0]))
                //    obj.Payment = Convert.ToInt32(DBNull.Value.Equals(dr["Payment"]) ? 0 : dr["Payment"]);
                //if (CommonMethods.ContainColumn("QBServiceItem", ds.Tables[0]))
                //    obj.QBServiceItem = Convert.ToString(dr["QBServiceItem"]);
                //if (CommonMethods.ContainColumn("QBServiceItemLabor", ds.Tables[0]))
                //    obj.QBServiceItemLabor = Convert.ToString(dr["QBServiceItemLabor"]);
                //if (CommonMethods.ContainColumn("QBServiceItemExp", ds.Tables[0]))
                //    obj.QBServiceItemExp = Convert.ToString(dr["QBServiceItemExp"]);
                //if (CommonMethods.ContainColumn("GPS", ds.Tables[0]))
                //    obj.GPS = Convert.ToInt32(DBNull.Value.Equals(dr["GPS"]) ? 0 : dr["GPS"]);
                //if (CommonMethods.ContainColumn("SageLastSync", ds.Tables[0]))
                //    obj.SageLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["SageLastSync"]) ? null : dr["SageLastSync"]);
                //if (CommonMethods.ContainColumn("SageIntegration", ds.Tables[0]))
                //    obj.SageIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["SageIntegration"]) ? 0 : dr["SageIntegration"]);
                //if (CommonMethods.ContainColumn("MSAttachReport", ds.Tables[0]))
                //    obj.MSAttachReport = Convert.ToInt32(DBNull.Value.Equals(dr["MSAttachReport"]) ? 0 : dr["MSAttachReport"]);
                //if (CommonMethods.ContainColumn("MSRTLabel", ds.Tables[0]))
                //    obj.MSRTLabel = Convert.ToString(dr["MSRTLabel"]);
                //if (CommonMethods.ContainColumn("MSOTLabel", ds.Tables[0]))
                //    obj.MSOTLabel = Convert.ToString(dr["MSOTLabel"]);
                //if (CommonMethods.ContainColumn("MSNTLabel", ds.Tables[0]))
                //    obj.MSNTLabel = Convert.ToString(dr["MSNTLabel"]);
                //if (CommonMethods.ContainColumn("MSDTLabel", ds.Tables[0]))
                //    obj.MSDTLabel = Convert.ToString(dr["MSDTLabel"]);
                //if (CommonMethods.ContainColumn("MSTTLabel", ds.Tables[0]))
                //    obj.MSTTLabel = Convert.ToString(dr["MSTTLabel"]);
                //if (CommonMethods.ContainColumn("MSTRTLabel", ds.Tables[0]))
                //    obj.MSTRTLabel = Convert.ToString(dr["MSTRTLabel"]);
                //if (CommonMethods.ContainColumn("MSTOTLabel", ds.Tables[0]))
                //    obj.MSTOTLabel = Convert.ToString(dr["MSTOTLabel"]);
                //if (CommonMethods.ContainColumn("MSTNTLabel", ds.Tables[0]))
                //    obj.MSTNTLabel = Convert.ToString(dr["MSTNTLabel"]);
                //if (CommonMethods.ContainColumn("MSTDTLabel", ds.Tables[0]))
                //    obj.MSTDTLabel = Convert.ToString(dr["MSTDTLabel"]);
                //if (CommonMethods.ContainColumn("MSTimeDataFieldVisibility", ds.Tables[0]))
                //    obj.MSTimeDataFieldVisibility = Convert.ToString(dr["MSTimeDataFieldVisibility"]);
                //if (CommonMethods.ContainColumn("TsIntegration", ds.Tables[0]))
                //    obj.TsIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["TsIntegration"]) ? 0 : dr["TsIntegration"]);
                //if (CommonMethods.ContainColumn("SyncLast", ds.Tables[0]))
                //    obj.SyncLast = Convert.ToDateTime(DBNull.Value.Equals(dr["SyncLast"]) ? null : dr["SyncLast"]);
                //if (CommonMethods.ContainColumn("SCDate", ds.Tables[0]))
                //    obj.SCDate = Convert.ToDateTime(DBNull.Value.Equals(dr["SCDate"]) ? null : dr["SCDate"]);
                //if (CommonMethods.ContainColumn("IntDate", ds.Tables[0]))
                //    obj.IntDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IntDate"]) ? null : dr["IntDate"]);
                //if (CommonMethods.ContainColumn("SCAmount", ds.Tables[0]))
                //    obj.SCAmount = Convert.ToInt32(DBNull.Value.Equals(dr["SCAmount"]) ? 0 : dr["SCAmount"]);
                //if (CommonMethods.ContainColumn("IntAmount", ds.Tables[0]))
                //    obj.IntAmount = Convert.ToInt32(DBNull.Value.Equals(dr["IntAmount"]) ? 0 : dr["IntAmount"]);
                //if (CommonMethods.ContainColumn("EndBalance", ds.Tables[0]))
                //    obj.EndBalance = Convert.ToInt32(DBNull.Value.Equals(dr["EndBalance"]) ? 0 : dr["EndBalance"]);
                //if (CommonMethods.ContainColumn("StatementDate", ds.Tables[0]))
                //    obj.StatementDate = Convert.ToDateTime(DBNull.Value.Equals(dr["StatementDate"]) ? null : dr["StatementDate"]);
                //if (CommonMethods.ContainColumn("bank", ds.Tables[0]))
                //    obj.bank = Convert.ToInt32(DBNull.Value.Equals(dr["bank"]) ? 0 : dr["bank"]);
                //if (CommonMethods.ContainColumn("MSIsTaskCodesRequired", ds.Tables[0]))
                //    obj.MSIsTaskCodesRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["MSIsTaskCodesRequired"]) ? false : dr["MSIsTaskCodesRequired"]);
                //if (CommonMethods.ContainColumn("Codes", ds.Tables[0]))
                //    obj.Codes = Convert.ToInt32(DBNull.Value.Equals(dr["Codes"]) ? false : dr["Codes"]);
                //if (CommonMethods.ContainColumn("ISshowHomeowner", ds.Tables[0]))
                //    obj.ISshowHomeowner = Convert.ToBoolean(DBNull.Value.Equals(dr["ISshowHomeowner"]) ? false : dr["ISshowHomeowner"]);
                //if (CommonMethods.ContainColumn("IsLocAddressBlank", ds.Tables[0]))
                //    obj.IsLocAddressBlank = Convert.ToBoolean(DBNull.Value.Equals(dr["IsLocAddressBlank"]) ? false : dr["IsLocAddressBlank"]);
                //if (CommonMethods.ContainColumn("PGUsername", ds.Tables[0]))
                //    obj.PGUsername = Convert.ToString(dr["PGUsername"]);
                //if (CommonMethods.ContainColumn("PGPassword", ds.Tables[0]))
                //    obj.PGPassword = Convert.ToString(dr["PGPassword"]);
                //if (CommonMethods.ContainColumn("PGSecretKey", ds.Tables[0]))
                //    obj.PGSecretKey = Convert.ToString(dr["PGSecretKey"]);
                //if (CommonMethods.ContainColumn("MSAppendMCPText", ds.Tables[0]))
                //    obj.MSAppendMCPText = Convert.ToBoolean(DBNull.Value.Equals(dr["MSAppendMCPText"]) ? false : dr["MSAppendMCPText"]);
                //if (CommonMethods.ContainColumn("MSSHAssignedTicket", ds.Tables[0]))
                //    obj.MSSHAssignedTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["MSSHAssignedTicket"]) ? false : dr["MSSHAssignedTicket"]);
                //if (CommonMethods.ContainColumn("MSHistoryShowLastTenTickets", ds.Tables[0]))
                //    obj.MSHistoryShowLastTenTickets = Convert.ToBoolean(DBNull.Value.Equals(dr["MSHistoryShowLastTenTickets"]) ? false : dr["MSHistoryShowLastTenTickets"]);
                //if (CommonMethods.ContainColumn("MS", ds.Tables[0]))
                //    obj.MS = Convert.ToBoolean(DBNull.Value.Equals(dr["MS"]) ? false : dr["MS"]);
                //if (CommonMethods.ContainColumn("ContactType", ds.Tables[0]))
                //    obj.ContactType = Convert.ToInt32(DBNull.Value.Equals(dr["ContactType"]) ? 0 : dr["ContactType"]);
                //if (CommonMethods.ContainColumn("Lat", ds.Tables[0]))
                //    obj.Lat = Convert.ToString(dr["Lat"]);
                //if (CommonMethods.ContainColumn("Lng", ds.Tables[0]))
                //    obj.Lng = Convert.ToString(dr["Lng"]);
                //if (CommonMethods.ContainColumn("consultAPI", ds.Tables[0]))
                //    obj.consultAPI = Convert.ToInt32(DBNull.Value.Equals(dr["consultAPI"]) ? 0 : dr["consultAPI"]);
                //if (CommonMethods.ContainColumn("CoCode", ds.Tables[0]))
                //    obj.CoCode = Convert.ToString(dr["CoCode"]);
                //if (CommonMethods.ContainColumn("ShutdownAlert", ds.Tables[0]))
                //    obj.ShutdownAlert = Convert.ToInt32(DBNull.Value.Equals(dr["ShutdownAlert"]) ? 0 : dr["ShutdownAlert"]);
                //if (CommonMethods.ContainColumn("MSCategoryPermission", ds.Tables[0]))
                //    obj.MSCategoryPermission = Convert.ToInt32(DBNull.Value.Equals(dr["MSCategoryPermission"]) ? 0 : dr["MSCategoryPermission"]);
                //if (CommonMethods.ContainColumn("IsUseTaxAPBills", ds.Tables[0]))
                //    obj.IsUseTaxAPBills = Convert.ToBoolean(DBNull.Value.Equals(dr["IsUseTaxAPBills"]) ? false : dr["IsUseTaxAPBills"]);
                //if (CommonMethods.ContainColumn("ApplyPasswordRules", ds.Tables[0]))
                //    obj.ApplyPasswordRules = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPasswordRules"]) ? false : dr["ApplyPasswordRules"]);
                //if (CommonMethods.ContainColumn("ApplyPwRulesToFieldUser", ds.Tables[0]))
                //    obj.ApplyPwRulesToFieldUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToFieldUser"]) ? false : dr["ApplyPwRulesToFieldUser"]);
                //if (CommonMethods.ContainColumn("ApplyPwRulesToOfficeUser", ds.Tables[0]))
                //    obj.ApplyPwRulesToOfficeUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToOfficeUser"]) ? false : dr["ApplyPwRulesToOfficeUser"]);
                //if (CommonMethods.ContainColumn("ApplyPwRulesToCustomerUser", ds.Tables[0]))
                //    obj.ApplyPwRulesToCustomerUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToCustomerUser"]) ? false : dr["ApplyPwRulesToCustomerUser"]);
                //if (CommonMethods.ContainColumn("ApplyPwReset", ds.Tables[0]))
                //    obj.ApplyPwReset = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwReset"]) ? false : dr["ApplyPwReset"]);
                //if (CommonMethods.ContainColumn("PwResetDays", ds.Tables[0]))
                //    obj.PwResetDays = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetDays"]) ? 0 : dr["PwResetDays"]);
                //if (CommonMethods.ContainColumn("PwResetting", ds.Tables[0]))
                //    obj.PwResetting = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetting"]) ? 0 : dr["PwResetting"]);
                //if (CommonMethods.ContainColumn("PwResetUserID", ds.Tables[0]))
                //    obj.PwResetUserID = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetUserID"]) ? 0 : dr["PwResetUserID"]);
                //if (CommonMethods.ContainColumn("JobCostLabor1", ds.Tables[0]))
                //    obj.JobCostLabor1 = Convert.ToInt32(DBNull.Value.Equals(dr["JobCostLabor1"]) ? 0 : dr["JobCostLabor1"]);
                //if (CommonMethods.ContainColumn("msemailnull", ds.Tables[0]))
                //    obj.msemailnull = Convert.ToInt32(DBNull.Value.Equals(dr["msemailnull"]) ? 0 : dr["msemailnull"]);
                //if (CommonMethods.ContainColumn("EmpSync", ds.Tables[0]))
                //    obj.EmpSync = Convert.ToInt32(DBNull.Value.Equals(dr["EmpSync"]) ? 0 : dr["EmpSync"]);
                //if (CommonMethods.ContainColumn("Name", ds.Tables[0]))
                //    obj.msreptemp = Convert.ToInt32(DBNull.Value.Equals(dr["msreptemp"]) ? 0 : dr["msreptemp"]);
                //if (CommonMethods.ContainColumn("tinternett", ds.Tables[0]))
                //    obj.tinternett = Convert.ToInt32(DBNull.Value.Equals(dr["tinternett"]) ? 0 : dr["tinternett"]);
                //if (CommonMethods.ContainColumn("businessstart", ds.Tables[0]))
                //    obj.businessstart = Convert.ToDateTime(DBNull.Value.Equals(dr["businessstart"]) ? null : dr["businessstart"]);
                //if (CommonMethods.ContainColumn("businesssend", ds.Tables[0]))
                //    obj.businesssend = Convert.ToDateTime(DBNull.Value.Equals(dr["businesssend"]) ? null : dr["businesssend"]);
                //if (CommonMethods.ContainColumn("TaskCode", ds.Tables[0]))
                //    obj.TaskCode = Convert.ToInt32(DBNull.Value.Equals(dr["TaskCode"]) ? 0 : dr["TaskCode"]);
                //if (CommonMethods.ContainColumn("IsSalesTaxAPBills", ds.Tables[0]))
                //    obj.IsSalesTaxAPBills = Convert.ToBoolean(DBNull.Value.Equals(dr["IsSalesTaxAPBills"]) ? false : dr["IsSalesTaxAPBills"]);
                //if (CommonMethods.ContainColumn("TargetHPermission", ds.Tables[0]))
                //    obj.TargetHPermission = Convert.ToInt32(DBNull.Value.Equals(dr["TargetHPermission"]) ? 0 : dr["TargetHPermission"]);
                //if (CommonMethods.ContainColumn("PwResetAdminEmail", ds.Tables[0]))
                //    obj.PwResetAdminEmail = Convert.ToString(dr["PwResetAdminEmail"]);
                //if (CommonMethods.ContainColumn("PwResetUsername", ds.Tables[0]))
                //    obj.PwResetUsername = Convert.ToString(dr["PwResetUsername"]);

                //_lstGetControlViewModel.Add(obj);
                #endregion

                _lstGetControlViewModel.Add(
                    new GetControlViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        fLong = Convert.ToInt32(DBNull.Value.Equals(dr["fLong"]) ? 0 : dr["fLong"]),
                        Latt = Convert.ToInt32(DBNull.Value.Equals(dr["Latt"]) ? 0 : dr["Latt"]),
                        GeoLock = Convert.ToInt32(DBNull.Value.Equals(dr["GeoLock"]) ? 0 : dr["GeoLock"]),
                        Version = Convert.ToInt32(DBNull.Value.Equals(dr["Version"]) ? 0 : dr["Version"]),
                        CDesc = Convert.ToString(dr["CDesc"]),
                        Build = Convert.ToInt32(DBNull.Value.Equals(dr["Build"]) ? 0 : dr["Build"]),
                        Minor = Convert.ToInt32(DBNull.Value.Equals(dr["Minor"]) ? 0 : dr["Minor"]),
                        Address = Convert.ToString(dr["Address"]),
                        AgeRemark = Convert.ToString(dr["AgeRemark"]),
                        SDate = Convert.ToString(dr["SDate"]),
                        EDate = Convert.ToString(dr["EDate"]),
                        YDate = Convert.ToString(dr["YDate"]),
                        GSTreg = Convert.ToString(dr["GSTreg"]),
                        IDesc = Convert.ToString(dr["IDesc"]),
                        PortalsID = Convert.ToInt32(DBNull.Value.Equals(dr["PortalsID"]) ? 0 : dr["PortalsID"]),
                        PrContractRemark = Convert.ToString(dr["PrContractRemark"]),
                        RepUser = Convert.ToString(dr["RepUser"]),
                        RepTitle = Convert.ToString(dr["RepTitle"]),
                        //Logo = Encoding.UTF8.GetBytes((char[])dr["Logo"]),
                        LogoPath = Convert.ToString(dr["LogoPath"]),
                        ExeBuildDate_Max = Convert.ToDateTime(DBNull.Value.Equals(dr["ExeBuildDate_Max"]) ? null : dr["ExeBuildDate_Max"]),
                        ExeBuildDate_Min = Convert.ToDateTime(DBNull.Value.Equals(dr["ExeBuildDate_Min"]) ? null : dr["ExeBuildDate_Min"]),
                        ExeVersion_Min = Convert.ToString(dr["ExeVersion_Min"]),
                        ExeVersion_Max = Convert.ToString(dr["ExeVersion_Max"]),
                        MerchantServicesConfig = Convert.ToString(dr["MerchantServicesConfig"]),
                        Email = Convert.ToString(dr["Email"]),
                        WebAddress = Convert.ToString(dr["WebAddress"]),
                        MSM = Convert.ToString(dr["MSM"]),
                        DSN = Convert.ToString(dr["DSN"]),
                        Username = Convert.ToString(dr["Username"]),
                        Password = Convert.ToString(dr["Password"]),
                        DBName = Convert.ToString(dr["DBName"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Map = Convert.ToInt32(DBNull.Value.Equals(dr["Map"]) ? 0 : dr["Map"]),
                        Custweb = Convert.ToInt32(DBNull.Value.Equals(dr["Custweb"]) ? 0 : dr["Custweb"]),
                        QBPath = Convert.ToString(dr["QBPath"]),
                        MultiLang = Convert.ToInt32(DBNull.Value.Equals(dr["MultiLang"]) ? 0 : dr["MultiLang"]),
                        QBIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["QBIntegration"]) ? 0 : dr["QBIntegration"]),
                        QBLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["QBLastSync"]) ? null : dr["QBLastSync"]),
                        MSSignTime = Convert.ToInt32(DBNull.Value.Equals(dr["MSSignTime"]) ? 0 : dr["MSSignTime"]),
                        GrossInc = Convert.ToInt32(DBNull.Value.Equals(dr["GrossInc"]) ? 0 : dr["GrossInc"]),
                        Month = Convert.ToInt32(DBNull.Value.Equals(dr["Month"]) ? 0 : dr["Month"]),
                        SalesAnnual = Convert.ToInt32(DBNull.Value.Equals(dr["SalesAnnual"]) ? 0 : dr["SalesAnnual"]),
                        Payment = Convert.ToInt32(DBNull.Value.Equals(dr["Payment"]) ? 0 : dr["Payment"]),
                        QBServiceItem = Convert.ToString(dr["QBServiceItem"]),
                        QBServiceItemLabor = Convert.ToString(dr["QBServiceItemLabor"]),
                        QBServiceItemExp = Convert.ToString(dr["QBServiceItemExp"]),
                        GPS = Convert.ToInt32(DBNull.Value.Equals(dr["GPS"]) ? 0 : dr["GPS"]),
                        SageLastSync = Convert.ToDateTime(DBNull.Value.Equals(dr["SageLastSync"]) ? null : dr["SageLastSync"]),
                        SageIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["SageIntegration"]) ? 0 : dr["SageIntegration"]),
                        MSAttachReport = Convert.ToInt32(DBNull.Value.Equals(dr["MSAttachReport"]) ? 0 : dr["MSAttachReport"]),
                        MSRTLabel = Convert.ToString(dr["MSRTLabel"]),
                        MSOTLabel = Convert.ToString(dr["MSOTLabel"]),
                        MSNTLabel = Convert.ToString(dr["MSNTLabel"]),
                        MSDTLabel = Convert.ToString(dr["MSDTLabel"]),
                        MSTTLabel = Convert.ToString(dr["MSTTLabel"]),
                        MSTRTLabel = Convert.ToString(dr["MSTRTLabel"]),
                        MSTOTLabel = Convert.ToString(dr["MSTOTLabel"]),
                        MSTNTLabel = Convert.ToString(dr["MSTNTLabel"]),
                        MSTDTLabel = Convert.ToString(dr["MSTDTLabel"]),
                        MSTimeDataFieldVisibility = Convert.ToString(dr["MSTimeDataFieldVisibility"]),
                        TsIntegration = Convert.ToInt32(DBNull.Value.Equals(dr["TsIntegration"]) ? 0 : dr["TsIntegration"]),
                        SyncLast = Convert.ToDateTime(DBNull.Value.Equals(dr["SyncLast"]) ? null : dr["SyncLast"]),
                        SCDate = Convert.ToDateTime(DBNull.Value.Equals(dr["SCDate"]) ? null : dr["SCDate"]),
                        IntDate = Convert.ToDateTime(DBNull.Value.Equals(dr["IntDate"]) ? null : dr["IntDate"]),
                        SCAmount = Convert.ToInt32(DBNull.Value.Equals(dr["SCAmount"]) ? 0 : dr["SCAmount"]),
                        IntAmount = Convert.ToInt32(DBNull.Value.Equals(dr["IntAmount"]) ? 0 : dr["IntAmount"]),
                        EndBalance = Convert.ToInt32(DBNull.Value.Equals(dr["EndBalance"]) ? 0 : dr["EndBalance"]),
                        StatementDate = Convert.ToDateTime(DBNull.Value.Equals(dr["StatementDate"]) ? null : dr["StatementDate"]),
                        bank = Convert.ToInt32(DBNull.Value.Equals(dr["bank"]) ? 0 : dr["bank"]),
                        //MSIsTaskCodesRequired = Convert.ToBoolean(DBNull.Value.Equals(dr["MSIsTaskCodesRequired"]) ? false : dr["MSIsTaskCodesRequired"]),
                        Codes = Convert.ToInt32(DBNull.Value.Equals(dr["Codes"]) ? false : dr["Codes"]),
                        ISshowHomeowner = Convert.ToBoolean(DBNull.Value.Equals(dr["ISshowHomeowner"]) ? false : dr["ISshowHomeowner"]),
                        IsLocAddressBlank = Convert.ToBoolean(DBNull.Value.Equals(dr["IsLocAddressBlank"]) ? false : dr["IsLocAddressBlank"]),
                        PGUsername = Convert.ToString(dr["PGUsername"]),
                        PGPassword = Convert.ToString(dr["PGPassword"]),
                        PGSecretKey = Convert.ToString(dr["PGSecretKey"]),
                        MSAppendMCPText = Convert.ToBoolean(DBNull.Value.Equals(dr["MSAppendMCPText"]) ? false : dr["MSAppendMCPText"]),
                        MSSHAssignedTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["MSSHAssignedTicket"]) ? false : dr["MSSHAssignedTicket"]),
                        MSHistoryShowLastTenTickets = Convert.ToBoolean(DBNull.Value.Equals(dr["MSHistoryShowLastTenTickets"]) ? false : dr["MSHistoryShowLastTenTickets"]),
                        MS = Convert.ToBoolean(DBNull.Value.Equals(dr["MS"]) ? false : dr["MS"]),
                        ContactType = Convert.ToInt32(DBNull.Value.Equals(dr["ContactType"]) ? 0 : dr["ContactType"]),
                        Lat = Convert.ToString(dr["Lat"]),
                        Lng = Convert.ToString(dr["Lng"]),
                        consultAPI = Convert.ToInt32(DBNull.Value.Equals(dr["consultAPI"]) ? 0 : dr["consultAPI"]),
                        CoCode = Convert.ToString(dr["CoCode"]),
                        ShutdownAlert = Convert.ToInt32(DBNull.Value.Equals(dr["ShutdownAlert"]) ? 0 : dr["ShutdownAlert"]),
                        MSCategoryPermission = Convert.ToInt32(DBNull.Value.Equals(dr["MSCategoryPermission"]) ? 0 : dr["MSCategoryPermission"]),
                        IsUseTaxAPBills = Convert.ToBoolean(DBNull.Value.Equals(dr["IsUseTaxAPBills"]) ? false : dr["IsUseTaxAPBills"]),
                        ApplyPasswordRules = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPasswordRules"]) ? false : dr["ApplyPasswordRules"]),
                        ApplyPwRulesToFieldUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToFieldUser"]) ? false : dr["ApplyPwRulesToFieldUser"]),
                        ApplyPwRulesToOfficeUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToOfficeUser"]) ? false : dr["ApplyPwRulesToOfficeUser"]),
                        ApplyPwRulesToCustomerUser = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwRulesToCustomerUser"]) ? false : dr["ApplyPwRulesToCustomerUser"]),
                        ApplyPwReset = Convert.ToBoolean(DBNull.Value.Equals(dr["ApplyPwReset"]) ? false : dr["ApplyPwReset"]),
                        PwResetDays = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetDays"]) ? 0 : dr["PwResetDays"]),
                        PwResetting = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetting"]) ? 0 : dr["PwResetting"]),
                        PwResetUserID = Convert.ToInt32(DBNull.Value.Equals(dr["PwResetUserID"]) ? 0 : dr["PwResetUserID"]),
                        PR = Convert.ToBoolean(DBNull.Value.Equals(dr["PR"]) ? false : dr["PR"]),
                        JobCostLabor1 = Convert.ToInt32(DBNull.Value.Equals(dr["JobCostLabor1"]) ? 0 : dr["JobCostLabor1"]),
                        msemailnull = Convert.ToInt32(DBNull.Value.Equals(dr["msemailnull"]) ? 0 : dr["msemailnull"]),
                        EmpSync = Convert.ToInt32(DBNull.Value.Equals(dr["EmpSync"]) ? 0 : dr["EmpSync"]),
                        msreptemp = Convert.ToInt32(DBNull.Value.Equals(dr["msreptemp"]) ? 0 : dr["msreptemp"]),
                        tinternett = Convert.ToInt32(DBNull.Value.Equals(dr["tinternett"]) ? 0 : dr["tinternett"]),
                        businessstart = Convert.ToDateTime(DBNull.Value.Equals(dr["businessstart"]) ? null : dr["businessstart"]),
                        businessend = Convert.ToDateTime(DBNull.Value.Equals(dr["businessend"]) ? null : dr["businessend"]),
                        TaskCode = Convert.ToInt32(DBNull.Value.Equals(dr["TaskCode"]) ? 0 : dr["TaskCode"]),
                        Year = Convert.ToInt32(DBNull.Value.Equals(dr["Year"]) ? 0 : dr["Year"]),
                        IsSalesTaxAPBills = Convert.ToBoolean(DBNull.Value.Equals(dr["IsSalesTaxAPBills"]) ? false : dr["IsSalesTaxAPBills"]),
                        TargetHPermission = Convert.ToInt32(DBNull.Value.Equals(dr["TargetHPermission"]) ? 0 : dr["TargetHPermission"]),
                        PwResetAdminEmail = Convert.ToString(dr["PwResetAdminEmail"]),
                        PwResetUsername = Convert.ToString(dr["PwResetUsername"])

                    });

            }
            return _lstGetControlViewModel;
        }

        public string getCompanyAddress(User objPropUser)
        {
            return objDL_User.getCompanyAddress(objPropUser);
        }

        public DataSet getLogo(User objPropUser)
        {
            return objDL_User.getLogo(objPropUser);
        }

        public DataSet getControlBranch(User objPropUser)
        {
            return objDL_User.getControlBranch(objPropUser);
        }

        public List<UserViewModel> getControlBranch(GetControlBranchParam objGetControlBranchParam, string ConnectionString)
        {
            DataSet ds = objDL_User.getControlBranch(objGetControlBranchParam, ConnectionString);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstUserViewModel.Add(
                    new UserViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Email = Convert.ToString(dr["Email"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Logo = Encoding.ASCII.GetBytes(dr["Logo"].ToString()),
                    }
                    );
            }

            return _lstUserViewModel;
        }


        public DataSet getAdminControl(User objPropUser)
        {
            return objDL_User.getAdminControl(objPropUser);
        }

        public DataSet getAdminAuthorization(User objPropUser)
        {
            return objDL_User.getAdminAuthorization(objPropUser);
        }

        public DataSet getCustomers(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getCustomers(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetCustomersViewModel> getCustomers(GetCustomersParam _GetCustomers, Int32 IsSalesAsigned, string ConnectionString)
        {
            DataSet ds = objDL_User.getCustomers(_GetCustomers, IsSalesAsigned, ConnectionString);

            List<GetCustomersViewModel> _lstGetCustomer = new List<GetCustomersViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomer.Add(
                    new GetCustomersViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        fLogin = Convert.ToString(dr["fLogin"]),
                        Status = Convert.ToString(dr["Status"]),
                        Address = Convert.ToString(dr["Address"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        type = Convert.ToString(dr["type"]),
                        city = Convert.ToString(dr["city"]),
                        State = Convert.ToString(dr["State"]),
                        phone = Convert.ToString(dr["phone"]),
                        website = Convert.ToString(dr["website"]),
                        cellular = Convert.ToString(dr["cellular"]),
                        email = Convert.ToString(dr["email"]),
                        qbcustomerid = Convert.ToString(dr["qbcustomerid"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        equip = Convert.ToInt32(DBNull.Value.Equals(dr["equip"]) ? 0 : dr["equip"]),
                        opencall = Convert.ToInt32(DBNull.Value.Equals(dr["opencall"]) ? 0 : dr["opencall"]),
                        sageid = Convert.ToString(dr["sageid"]),
                    }
                    );
            }

            return _lstGetCustomer;
        }

        public DataSet getMSMCustomers(User objPropUser)
        {
            return objDL_User.getMSMCustomers(objPropUser);
        }

        //API
        public List<GetMSMCustomersViewModel> getMSMCustomers(GetMSMCustomersParam _GetMSMCustomers, string ConnectionString)
        {
            DataSet ds = objDL_User.getMSMCustomers(_GetMSMCustomers, ConnectionString);

            List<GetMSMCustomersViewModel> _lstGetMSMCustomers = new List<GetMSMCustomersViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetMSMCustomers.Add(
                    new GetMSMCustomersViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        QBCustomerID = Convert.ToString(dr["QBCustomerID"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Country = Convert.ToString(dr["Country"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Name = Convert.ToString(dr["Name"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        balance = Convert.ToDouble(DBNull.Value.Equals(dr["balance"]) ? 0 : dr["balance"]),
                        QBCustomertypeID = Convert.ToString(dr["QBCustomertypeID"]),
                    }
                    );
            }

            return _lstGetMSMCustomers;
        }

        public DataSet getMSMCustomersMapping(User objPropUser)
        {
            return objDL_User.getMSMCustomersMapping(objPropUser);
        }

        public DataSet getCustomersSageAdd(User objPropUser)
        {
            return objDL_User.getCustomersSageAdd(objPropUser);
        }

        public DataSet getLocationsSageAdd(User objPropUser)
        {
            return objDL_User.getLocationsSageAdd(objPropUser);
        }

        public DataSet getLocationsSageNA(User objPropUser)
        {
            return objDL_User.getLocationsSageNA(objPropUser);
        }

        public DataSet geCustomersSageNA(User objPropUser)
        {
            return objDL_User.geCustomersSageNA(objPropUser);
        }

        public void UpdateSageID(User objPropUser)
        {
            objDL_User.UpdateSageID(objPropUser);
        }

        public void UpdateLocSageID(User objPropUser)
        {
            objDL_User.UpdateLocSageID(objPropUser);
        }

        public DataSet getCustomersSageUpdate(User objPropUser)
        {
            return objDL_User.getCustomersSageUpdate(objPropUser);
        }

        public DataSet getLocationsSageUpdate(User objPropUser)
        {
            return objDL_User.getLocationsSageUpdate(objPropUser);
        }

        public DataSet getCustomersForSageDelete(User objPropUser)
        {
            return objDL_User.getCustomersForSageDelete(objPropUser);
        }

        public DataSet getLocationsForSageDelete(User objPropUser)
        {
            return objDL_User.getLocationsForSageDelete(objPropUser);
        }

        public DataSet getMSMLocation(User objPropUser)
        {
            return objDL_User.getMSMLocation(objPropUser);
        }

        //API
        public List<GetMSMLocationViewModel> getMSMLocation(GetMSMLocationParam _GetMSMLocation, string ConnectionString)
        {
            DataSet ds = objDL_User.getMSMLocation(_GetMSMLocation, ConnectionString);

            List<GetMSMLocationViewModel> _lstGetMSMLocation = new List<GetMSMLocationViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetMSMLocation.Add(
                    new GetMSMLocationViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        qbcustomerid = Convert.ToString(dr["qbcustomerid"]),
                        tag = Convert.ToString(dr["tag"]),
                        QBLocID = Convert.ToString(dr["QBLocID"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Country = Convert.ToString(dr["Country"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Name = Convert.ToString(dr["Name"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        balance = Convert.ToDouble(DBNull.Value.Equals(dr["balance"]) ? 0 : dr["balance"]),
                        QBCustomertypeID = Convert.ToString(dr["QBCustomertypeID"]),
                        QBlocTypeID = Convert.ToString(dr["QBlocTypeID"]),
                        QBStaxID = Convert.ToString(dr["QBStaxID"]),
                        shipaddress = Convert.ToString(dr["shipaddress"]),
                        shipcity = Convert.ToString(dr["shipcity"]),
                        shipstate = Convert.ToString(dr["shipstate"]),
                        shipzip = Convert.ToString(dr["shipzip"]),
                    }
                    );
            }

            return _lstGetMSMLocation;
        }


        public DataSet getMSMLocationMapping(User objPropUser)
        {
            return objDL_User.getMSMLocationMapping(objPropUser);
        }

        public DataSet getQBCustomers(User objPropUser)
        {
            return objDL_User.getQBCustomers(objPropUser);
        }

        //API
        public List<GetQBCustomersViewModel> getQBCustomers(GetQBCustomersParam _GetQBCustomers, string ConnectionString)
        {
            DataSet ds = objDL_User.getQBCustomers(_GetQBCustomers, ConnectionString);
            List<GetQBCustomersViewModel> _lstGetQBCustomers = new List<GetQBCustomersViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetQBCustomers.Add(
                    new GetQBCustomersViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        QBCustomerID = Convert.ToString(dr["QBCustomerID"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Country = Convert.ToString(dr["Country"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Name = Convert.ToString(dr["Name"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        QBCustomertypeID = Convert.ToString(dr["QBCustomertypeID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        Type = Convert.ToString(dr["Type"]),
                    }
                    );
            }

            return _lstGetQBCustomers;
        }

        public DataSet getQBLocation(User objPropUser)
        {
            return objDL_User.getQBLocation(objPropUser);
        }

        //API
        public List<GetQBLocationViewModel> getQBLocation(GetQBLocationParam _GetQBLocation, string ConnectionString)
        {
            DataSet ds = objDL_User.getQBLocation(_GetQBLocation, ConnectionString);

            List<GetQBLocationViewModel> _lstGetQBLocation = new List<GetQBLocationViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetQBLocation.Add(
                    new GetQBLocationViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        qbcustomerid = Convert.ToString(dr["qbcustomerid"]),
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Country = Convert.ToString(dr["Country"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Name = Convert.ToString(dr["Name"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        QBCustomertypeID = Convert.ToString(dr["QBCustomertypeID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        balance = Convert.ToDouble(DBNull.Value.Equals(dr["balance"]) ? 0 : dr["balance"]),
                        QBlocTypeID = Convert.ToString(dr["QBlocTypeID"]),
                        QBStaxID = Convert.ToString(dr["QBStaxID"]),
                        shipaddress = Convert.ToString(dr["shipaddress"]),
                        shipcity = Convert.ToString(dr["shipcity"]),
                        shipstate = Convert.ToString(dr["shipstate"]),
                        shipzip = Convert.ToString(dr["shipzip"]),
                        QBLocID = Convert.ToString(dr["QBLocID"]),
                    }
                    );
            }

            return _lstGetQBLocation;
        }


        public DataSet getLocations(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocations(objPropUser, IsSalesAsigned);
        }

        public DataSet getUserSearch(User objPropUser)
        {
            return objDL_User.getUserSearch(objPropUser);
        }

        public DataSet GetUserfForEstimate(User objPropUser)
        {
            return objDL_User.GetUserfForEstimate(objPropUser);
        }

        public DataSet getCustomerSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getCustomerSearch(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetCustomerSearchViewModel> getCustomerSearch(GetCustomerSearchParam _GetCustomerSearch, Int32 IsSalesAsigned, string ConnectionString)
        {
            DataSet ds = objDL_User.getCustomerSearch(_GetCustomerSearch, IsSalesAsigned, ConnectionString);

            List<GetCustomerSearchViewModel> _lstGetCustomerSearch = new List<GetCustomerSearchViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomerSearch.Add(
                    new GetCustomerSearchViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        fLogin = Convert.ToString(dr["fLogin"]),
                        Status = Convert.ToString(dr["Status"]),
                        Address = Convert.ToString(dr["Address"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        type = Convert.ToString(dr["type"]),
                        city = Convert.ToString(dr["city"]),
                        State = Convert.ToString(dr["State"]),
                        phone = Convert.ToString(dr["phone"]),
                        website = Convert.ToString(dr["website"]),
                        cellular = Convert.ToString(dr["cellular"]),
                        email = Convert.ToString(dr["email"]),
                        qbcustomerid = Convert.ToString(dr["qbcustomerid"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        equip = Convert.ToInt32(DBNull.Value.Equals(dr["equip"]) ? 0 : dr["equip"]),
                        opencall = Convert.ToInt32(DBNull.Value.Equals(dr["opencall"]) ? 0 : dr["opencall"]),
                        sageid = Convert.ToInt32(DBNull.Value.Equals(dr["sageid"]) ? 0 : dr["sageid"]),
                    }
                    );
            }

            return _lstGetCustomerSearch;
        }

        public DataSet getCustomerAuto(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getCustomerAuto(objPropUser, IsSalesAsigned);
        }

        public DataSet getAccountAuto(User objPropUser)
        {
            return objDL_User.getAccountAuto(objPropUser);
        }

        public DataSet getCustomerAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getCustomerAutojquery(objPropUser, IsSalesAsigned);
        }

        public DataSet getCustomerWithInactive(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getCustomerWithInactive(objPropUser, IsSalesAsigned);
        }

        public DataSet getCustomerProspectAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getCustomerProspectAutojquery(objPropUser, IsSalesAsigned);
        }

        public DataSet getTaskContactsSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getTaskContactsSearch(objPropUser, IsSalesAsigned);
        }

        public DataSet GetContactsSearchbyRolid(User objPropUser)
        {
            return objDL_User.GetContactsSearchbyRolid(objPropUser);
        }

        public DataSet getElevSearch(User objPropUser)
        {
            return objDL_User.getElevSearch(objPropUser);
        }

        public DataSet getLocationsData(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocationsData(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetLocationDataSearchViewModel> getLocationsData(GetLocationsDataParam _GetLocationsData, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds = objDL_User.getLocationsData(_GetLocationsData, ConnectionString, IsSalesAsigned);

            List<GetLocationDataSearchViewModel> _lstGetLocationDataSearch = new List<GetLocationDataSearchViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationDataSearch.Add(
                    new GetLocationDataSearchViewModel()
                    {
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Name = Convert.ToString(dr["Name"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        BusinessType = Convert.ToInt32(DBNull.Value.Equals(dr["BusinessType"]) ? 0 : dr["BusinessType"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Address = Convert.ToString(dr["Address"]),
                        BusinessTypeName = Convert.ToString(dr["BusinessTypeName"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        ConsultantName = Convert.ToString(dr["ConsultantName"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        credit = Convert.ToInt16(DBNull.Value.Equals(dr["credit"]) ? 0 : dr["credit"]),
                        Email = Convert.ToString(dr["Email"]),
                        CusID = Convert.ToInt32(DBNull.Value.Equals(dr["CusID"]) ? 0 : dr["CusID"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        opencall = Convert.ToInt32(DBNull.Value.Equals(dr["opencall"]) ? 0 : dr["opencall"]),
                        SageID = Convert.ToString(dr["SageID"]),
                        Elevs = Convert.ToInt16(DBNull.Value.Equals(dr["Elevs"]) ? 0 : dr["Elevs"]),
                        Location = Convert.ToString(dr["Location"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        locid = Convert.ToString(dr["locid"]),
                        NoCustomerStatement = Convert.ToBoolean(DBNull.Value.Equals(dr["NoCustomerStatement"]) ? 0 : dr["NoCustomerStatement"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        qblocid = Convert.ToString(dr["qblocid"]),
                        RouteName = Convert.ToString(dr["RouteName"]),
                        Salesperson = Convert.ToString(dr["Salesperson"]),
                        Salesperson2 = Convert.ToString(dr["Salesperson2"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Type = Convert.ToString(dr["Type"]),
                        Zone = Convert.ToString(dr["Zone"]),
                        locStatus = Convert.ToString(dr["locStatus"]),
                    }
                    );
            }

            return _lstGetLocationDataSearch;
        }

        public DataSet getLocationDataSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocationDataSearch(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetLocationDataSearchViewModel> getLocationDataSearch(GetLocationDataSearchParam _GetLocationDataSearch, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds = objDL_User.getLocationDataSearch(_GetLocationDataSearch, ConnectionString, IsSalesAsigned);

            List<GetLocationDataSearchViewModel> _lstGetLocationDataSearch = new List<GetLocationDataSearchViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationDataSearch.Add(
                    new GetLocationDataSearchViewModel()
                    {
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Name = Convert.ToString(dr["Name"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        BusinessType = Convert.ToInt32(DBNull.Value.Equals(dr["BusinessType"]) ? 0 : dr["BusinessType"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Address = Convert.ToString(dr["Address"]),
                        BusinessTypeName = Convert.ToString(dr["BusinessTypeName"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        ConsultantName = Convert.ToString(dr["ConsultantName"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        credit = Convert.ToInt16(DBNull.Value.Equals(dr["credit"]) ? 0 : dr["credit"]),
                        Email = Convert.ToString(dr["Email"]),
                        CusID = Convert.ToInt32(DBNull.Value.Equals(dr["CusID"]) ? 0 : dr["CusID"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        opencall = Convert.ToInt32(DBNull.Value.Equals(dr["opencall"]) ? 0 : dr["opencall"]),
                        SageID = Convert.ToString(dr["SageID"]),
                        Elevs = Convert.ToInt16(DBNull.Value.Equals(dr["Elevs"]) ? 0 : dr["Elevs"]),
                        Location = Convert.ToString(dr["Location"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        locid = Convert.ToString(dr["locid"]),
                        NoCustomerStatement = Convert.ToBoolean(DBNull.Value.Equals(dr["NoCustomerStatement"]) ? 0 : dr["NoCustomerStatement"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        qblocid = Convert.ToString(dr["qblocid"]),
                        RouteName = Convert.ToString(dr["RouteName"]),
                        Salesperson = Convert.ToString(dr["Salesperson"]),
                        Salesperson2 = Convert.ToString(dr["Salesperson2"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Type = Convert.ToString(dr["Type"]),
                        Zone = Convert.ToString(dr["Zone"]),
                    }
                    );
            }

            return _lstGetLocationDataSearch;
        }

        public DataSet getLocationAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocationAutojquery(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetLocationAutojqueryViewModel> getLocationAutojquery(GetLocationAutojqueryParam _GetLocationAutojquery, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds = objDL_User.getLocationAutojquery(_GetLocationAutojquery, ConnectionString, IsSalesAsigned);

            List<GetLocationAutojqueryViewModel> _lstGetLocationAutojquery = new List<GetLocationAutojqueryViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationAutojquery.Add(
                    new GetLocationAutojqueryViewModel()
                    {
                        value = Convert.ToInt32(DBNull.Value.Equals(dr["value"]) ? 0 : dr["value"]),
                        label = Convert.ToString(dr["label"]),
                        desc = Convert.ToString(dr["desc"]),
                        custsageid = Convert.ToString(dr["custsageid"]),
                        rolid = Convert.ToInt32(DBNull.Value.Equals(dr["rolid"]) ? 0 : dr["rolid"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        STaxRate = Convert.ToString(dr["STaxRate"]),
                        STax = Convert.ToString(dr["STax"]),
                    }
                    );
            }
            return _lstGetLocationAutojquery;
        }

        public DataSet getLocationWithInactive(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocationWithInactive(objPropUser, IsSalesAsigned);
        }

        public DataSet GetLocationProspectSearch(User objPropUser, Int32 IsSalesAsigned = 0, Int32 IsProspect = 0)
        {
            return objDL_User.GetLocationProspectSearch(objPropUser, IsSalesAsigned, IsProspect);
        }

        public DataSet MainSearch(User objPropUser)
        {
            return objDL_User.MainSearch(objPropUser);
        }
        public DataSet getLocationSearch(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocationSearch(objPropUser, IsSalesAsigned);
        }

        public DataSet getCompany(User objPropUser)
        {
            return objDL_User.getCompany(objPropUser);
        }

        // Thomas
        // This function was updated to get on the permission for user: merging permission of user and user role also
        // Please use the new function GetUserInfoByID for instead of this
        // This function now will get user permission from Session that create while logging in
        public DataSet GetUserPermissionByUserID(User objPropUser)
        {
            if (HttpContext.Current.Session["userinfo"] != null)
            {
                try
                {
                    var userinfoDT = (DataTable)HttpContext.Current.Session["userinfo"];
                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(userinfoDT);
                    return dataSet;
                }
                catch (Exception)
                {
                    return objDL_User.getUserByID(objPropUser);
                }
            }
            else
            {
                return objDL_User.getUserByID(objPropUser);
            }

        }

        //API
        //public ListGetUserPermissionByUserID GetUserPermissionByUserID(GetUserByIdParam _GetUserById, string ConnectionString)
        //{
        //    //if (HttpContext.Current.Session["userinfo"] != null)
        //    if (HttpContext.Current.Session["userinfo"] != null)
        //    {
        //        try
        //        {
        //            //var userinfoDT = (DataTable)HttpContext.Current.Session["userinfo"];
        //            var userinfoDT = (DataTable)HttpContext.Current.Session["userinfo"];
        //            DataSet dataSet = new DataSet();
        //            dataSet.Tables.Add(userinfoDT);

        //            ListGetUserPermissionByUserID _ds = new ListGetUserPermissionByUserID();
        //            List<GetUserPermissionByUserIDTable1> _lstTable1 = new List<GetUserPermissionByUserIDTable1>();
        //            List<GetUserPermissionByUserIDTable2> _lstTable2 = new List<GetUserPermissionByUserIDTable2>();
        //            List<GetUserPermissionByUserIDTable3> _lstTable3 = new List<GetUserPermissionByUserIDTable3>();

        //            foreach (DataRow dr in dataSet.Tables[0].Rows)
        //            {
        //                _lstTable1.Add(new GetUserPermissionByUserIDTable1()
        //                {
        //                    fStart = Convert.ToDateTime(DBNull.Value.Equals(dr["fStart"]) ? null : dr["fStart"]),
        //                    fEnd = Convert.ToDateTime(DBNull.Value.Equals(dr["fEnd"]) ? null : dr["fEnd"]),
        //                    PDASerialNumber = Convert.ToString(dr["PDASerialNumber"]),
        //                    Empid = Convert.ToInt32(DBNull.Value.Equals(dr["Empid"]) ? 0 : dr["Empid"]),
        //                    userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
        //                    rolid = Convert.ToInt32(DBNull.Value.Equals(dr["rolid"]) ? 0 : dr["rolid"]),
        //                    workID = Convert.ToInt32(DBNull.Value.Equals(dr["workID"]) ? 0 : dr["workID"]),
        //                    fUser = Convert.ToString(dr["fUser"]),
        //                    Password = Convert.ToString(dr["Password"]),
        //                    Dispatch = Convert.ToString(dr["Dispatch"]),
        //                    Location = Convert.ToString(dr["Location"]),
        //                    PO = Convert.ToString(dr["PO"]),
        //                    Control = Convert.ToString(dr["Control"]),
        //                    UserS = Convert.ToString(dr["UserS"]),
        //                    City = Convert.ToString(dr["City"]),
        //                    State = Convert.ToString(dr["State"]),
        //                    Zip = Convert.ToString(dr["Zip"]),
        //                    Phone = Convert.ToString(dr["Phone"]),
        //                    Address = Convert.ToString(dr["Address"]),
        //                    EMail = Convert.ToString(dr["EMail"]),
        //                    Cellular = Convert.ToString(dr["Cellular"]),
        //                    Field = Convert.ToInt32(DBNull.Value.Equals(dr["Field"]) ? 0 : dr["Field"]),
        //                    fFirst = Convert.ToString(dr["fFirst"]),
        //                    Middle = Convert.ToString(dr["Middle"]),
        //                    Last = Convert.ToString(dr["Last"]),
        //                    DHired = Convert.ToDateTime(DBNull.Value.Equals(dr["DHired"]) ? null : dr["DHired"]),
        //                    DFired = Convert.ToDateTime(DBNull.Value.Equals(dr["DFired"]) ? null : dr["DFired"]),
        //                    CallSign = Convert.ToString(dr["CallSign"]),
        //                    Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
        //                    fWork = Convert.ToInt32(DBNull.Value.Equals(dr["fWork"]) ? 0 : dr["fWork"]),
        //                    Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
        //                    Remarks = Convert.ToString(dr["Remarks"]),
        //                    ticketo = Convert.ToString(dr["ticketo"]),
        //                    ticketd = Convert.ToString(dr["ticketd"]),
        //                    ticket = Convert.ToString(dr["ticket"]),
        //                    UserSales = Convert.ToString(dr["UserSales"]),
        //                    employeeMaint = Convert.ToString(dr["employeeMaint"]),
        //                    TC = Convert.ToString(dr["TC"]),
        //                    pager = Convert.ToString(dr["pager"]),
        //                    super = Convert.ToString(dr["super"]),
        //                    sales = Convert.ToInt16(DBNull.Value.Equals(dr["sales"]) ? 0 : dr["sales"]),
        //                    Lang = Convert.ToString(dr["Lang"]),
        //                    merchantinfoid = Convert.ToInt32(DBNull.Value.Equals(dr["merchantinfoid"]) ? 0 : dr["merchantinfoid"]),
        //                    dboard = Convert.ToInt16(DBNull.Value.Equals(dr["dboard"]) ? 0 : dr["dboard"]),
        //                    DefaultWorker = Convert.ToInt32(DBNull.Value.Equals(dr["DefaultWorker"]) ? 0 : dr["DefaultWorker"]),
        //                    massreview = Convert.ToInt32(DBNull.Value.Equals(dr["massreview"]) ? 0 : dr["massreview"]),
        //                    msmpass = Convert.ToString(dr["msmpass"]),
        //                    msmuser = Convert.ToString(dr["msmuser"]),
        //                    emailaccount = Convert.ToInt16(DBNull.Value.Equals(dr["emailaccount"]) ? 0 : dr["emailaccount"]),
        //                    hourlyrate = Convert.ToDouble(DBNull.Value.Equals(dr["hourlyrate"]) ? 0 : dr["hourlyrate"]),
        //                    pmethod = Convert.ToInt16(DBNull.Value.Equals(dr["pmethod"]) ? 0 : dr["pmethod"]),
        //                    phour = Convert.ToDouble(DBNull.Value.Equals(dr["phour"]) ? 0 : dr["phour"]),
        //                    salary = Convert.ToDouble(DBNull.Value.Equals(dr["salary"]) ? 0 : dr["salary"]),
        //                    payperiod = Convert.ToInt16(DBNull.Value.Equals(dr["payperiod"]) ? 0 : dr["payperiod"]),
        //                    mileagerate = Convert.ToDouble(DBNull.Value.Equals(dr["mileagerate"]) ? 0 : dr["mileagerate"]),
        //                    Ref = Convert.ToString(dr["Ref"]),
        //                    elevator = Convert.ToString(dr["elevator"]),
        //                    Chart = Convert.ToString(dr["Chart"]),
        //                    GLAdj = Convert.ToString(dr["GLAdj"]),
        //                    CustomerPayment = Convert.ToString(dr["CustomerPayment"]),
        //                    Deposit = Convert.ToString(dr["Deposit"]),
        //                    Financial = Convert.ToString(dr["Financial"]),
        //                    Vendor = Convert.ToString(dr["Vendor"]),
        //                    Bill = Convert.ToString(dr["Bill"]),
        //                    BillSelect = Convert.ToString(dr["BillSelect"]),
        //                    BillPay = Convert.ToString(dr["BillPay"]),
        //                    Owner = Convert.ToString(dr["Owner"]),
        //                    Job = Convert.ToString(dr["Job"]),
        //                    MSAuthorisedDeviceOnly = Convert.ToInt32(DBNull.Value.Equals(dr["MSAuthorisedDeviceOnly"]) ? 0 : dr["MSAuthorisedDeviceOnly"]),
        //                    ProjectListPermission = Convert.ToString(dr["ProjectListPermission"]),
        //                    FinancePermission = Convert.ToString(dr["FinancePermission"]),
        //                    BOMPermission = Convert.ToString(dr["BOMPermission"]),
        //                    MilestonesPermission = Convert.ToString(dr["MilestonesPermission"]),
        //                    Item = Convert.ToString(dr["Item"]),
        //                    InvAdj = Convert.ToString(dr["InvAdj"]),
        //                    Warehouse = Convert.ToString(dr["Warehouse"]),
        //                    InvSetup = Convert.ToString(dr["InvSetup"]),
        //                    InvViewer = Convert.ToString(dr["InvViewer"]),
        //                    DocumentPermission = Convert.ToString(dr["DocumentPermission"]),
        //                    ContactPermission = Convert.ToString(dr["ContactPermission"]),
        //                    SalesAssigned = Convert.ToBoolean(DBNull.Value.Equals(dr["SalesAssigned"]) ? false : dr["SalesAssigned"]),
        //                    ProjecttempPermission = Convert.ToString(dr["ProjecttempPermission"]),
        //                    NotificationOnAddOpportunity = Convert.ToBoolean(DBNull.Value.Equals(dr["NotificationOnAddOpportunity"]) ? false : dr["NotificationOnAddOpportunity"]),
        //                    POLimit = Convert.ToDecimal(DBNull.Value.Equals(dr["POLimit"]) ? 0 : dr["POLimit"]),
        //                    POApprove = Convert.ToInt16(DBNull.Value.Equals(dr["POApprove"]) ? 0 : dr["POApprove"]),
        //                    POApproveAmt = Convert.ToInt16(DBNull.Value.Equals(dr["POApproveAmt"]) ? 0 : dr["POApproveAmt"]),
        //                    MinAmount = Convert.ToDecimal(DBNull.Value.Equals(dr["MinAmount"]) ? 0 : dr["MinAmount"]),
        //                    MaxAmount = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxAmount"]) ? 0 : dr["MaxAmount"]),
        //                    Lng = Convert.ToString(dr["Lng"]),
        //                    Lat = Convert.ToString(dr["Lat"]),
        //                    Country = Convert.ToString(dr["Country"]),
        //                    MSDeviceId = Convert.ToString(dr["MSDeviceId"]),
        //                    Website = Convert.ToString(dr["Website"]),
        //                    Contact = Convert.ToString(dr["Contact"]),
        //                    Title = Convert.ToString(dr["Title"]),
        //                    ProfileImage = Convert.ToString(dr["ProfileImage"]),
        //                    CoverImage = Convert.ToString(dr["CoverImage"]),
        //                    BillingCodesPermission = Convert.ToString(dr["BillingCodesPermission"]),
        //                    Invoice = Convert.ToString(dr["Invoice"]),
        //                    PurchasingmodulePermission = Convert.ToString(dr["PurchasingmodulePermission"]),
        //                    BillingmodulePermission = Convert.ToString(dr["BillingmodulePermission"]),
        //                    RPO = Convert.ToString(dr["RPO"]),
        //                    AccountPayablemodulePermission = Convert.ToString(dr["AccountPayablemodulePermission"]),
        //                    PaymentHistoryPermission = Convert.ToString(dr["PaymentHistoryPermission"]),
        //                    CustomermodulePermission = Convert.ToString(dr["CustomermodulePermission"]),
        //                    Apply = Convert.ToString(dr["Apply"]),
        //                    Collection = Convert.ToString(dr["Collection"]),
        //                    bankrec = Convert.ToString(dr["bankrec"]),
        //                    FinancialmodulePermission = Convert.ToString(dr["FinancialmodulePermission"]),
        //                    RCmodulePermission = Convert.ToString(dr["RCmodulePermission"]),
        //                    ProcessRCPermission = Convert.ToString(dr["ProcessRCPermission"]),
        //                    ProcessC = Convert.ToString(dr["ProcessC"]),
        //                    ProcessT = Convert.ToString(dr["ProcessT"]),
        //                    SafetyTestsPermission = Convert.ToString(dr["SafetyTestsPermission"]),
        //                    RCRenewEscalatePermission = Convert.ToString(dr["RCRenewEscalatePermission"]),
        //                    SchedulemodulePermission = Convert.ToString(dr["SchedulemodulePermission"]),
        //                    Resolve = Convert.ToString(dr["Resolve"]),
        //                    TicketPermission = Convert.ToString(dr["TicketPermission"]),
        //                    MTimesheet = Convert.ToString(dr["MTimesheet"]),
        //                    ETimesheet = Convert.ToString(dr["ETimesheet"]),
        //                    MapR = Convert.ToString(dr["MapR"]),
        //                    RouteBuilder = Convert.ToString(dr["RouteBuilder"]),
        //                    MassTimesheetCheck = Convert.ToString(dr["MassTimesheetCheck"]),
        //                    CreditHold = Convert.ToString(dr["CreditHold"]),
        //                    LocCount = Convert.ToInt32(DBNull.Value.Equals(dr["LocCount"]) ? 0 : dr["LocCount"]),
        //                    salesmanager = Convert.ToString(dr["salesmanager"]),
        //                    Sales = Convert.ToString(dr["Sales"]),
        //                    ToDo = Convert.ToInt16(DBNull.Value.Equals(dr["ToDo"]) ? 0 : dr["ToDo"]),
        //                    ToDoC = Convert.ToInt16(DBNull.Value.Equals(dr["ToDoC"]) ? 0 : dr["ToDoC"]),
        //                    FU = Convert.ToString(dr["FU"]),
        //                    Proposal = Convert.ToString(dr["Proposal"]),
        //                    Estimates = Convert.ToString(dr["Estimates"]),
        //                    AwardEstimates = Convert.ToString(dr["AwardEstimates"]),
        //                    salessetup = Convert.ToString(dr["salessetup"]),
        //                    PONotification = Convert.ToString(dr["PONotification"]),
        //                    WriteOff = Convert.ToString(dr["WriteOff"]),
        //                    SSN = Convert.ToString(dr["SSN"]),
        //                    Sex = Convert.ToString(dr["Sex"]),
        //                    DBirth = Convert.ToDateTime(DBNull.Value.Equals(dr["DBirth"]) ? null : dr["DBirth"]),
        //                    Race = Convert.ToString(dr["Race"]),
        //                    ProjectModulePermission = Convert.ToString(dr["ProjectModulePermission"]),
        //                    InventoryModulePermission = Convert.ToString(dr["InventoryModulePermission"]),
        //                    JobClosePermission = Convert.ToString(dr["JobClosePermission"]),
        //                    JobCompletedPermission = Convert.ToString(dr["JobCompletedPermission"]),
        //                    JobReopenPermission = Convert.ToString(dr["JobReopenPermission"]),
        //                    IsProjectManager = Convert.ToBoolean(DBNull.Value.Equals(dr["IsProjectManager"]) ? false : dr["IsProjectManager"]),
        //                    IsAssignedProject = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAssignedProject"]) ? false : dr["IsAssignedProject"]),
        //                    TicketVoidPermission = Convert.ToInt32(DBNull.Value.Equals(dr["TicketVoidPermission"]) ? null : dr["TicketVoidPermission"]),
        //                    Employee = Convert.ToString(dr["Employee"]),
        //                    PRProcess = Convert.ToString(dr["PRProcess"]),
        //                    PRRegister = Convert.ToString(dr["PRRegister"]),
        //                    PRReport = Convert.ToString(dr["PRReport"]),
        //                    PRWage = Convert.ToString(dr["PRWage"]),
        //                    PRDeduct = Convert.ToString(dr["PRDeduct"]),
        //                    PR = Convert.ToBoolean(DBNull.Value.Equals(dr["PR"]) ? false : dr["PR"]),
        //                    RoleId = Convert.ToInt32(DBNull.Value.Equals(dr["RoleId"]) ? null : dr["RoleId"]),
        //                    ApplyUserRolePermission = Convert.ToInt32(DBNull.Value.Equals(dr["ApplyUserRolePermission"]) ? null : dr["ApplyUserRolePermission"]),
        //                });
        //            }

        //            foreach (DataRow dr in dataSet.Tables[1].Rows)
        //            {
        //                _lstTable2.Add(new GetUserPermissionByUserIDTable2()
        //                {
        //                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
        //                    InServer = Convert.ToString(dr["InServer"]),
        //                    InServerType = Convert.ToString(dr["InServerType"]),
        //                    InUsername = Convert.ToString(dr["InUsername"]),
        //                    InPassword = Convert.ToString(dr["InPassword"]),
        //                    InPort = Convert.ToInt32(DBNull.Value.Equals(dr["InPort"]) ? 0 : dr["InPort"]),
        //                    OutServer = Convert.ToString(dr["OutServer"]),
        //                    OutUsername = Convert.ToString(dr["OutUsername"]),
        //                    OutPassword = Convert.ToString(dr["OutUsername"]),
        //                    OutPort = Convert.ToInt32(DBNull.Value.Equals(dr["OutPort"]) ? 0 : dr["OutPort"]),
        //                    SSL = Convert.ToBoolean(DBNull.Value.Equals(dr["SSL"]) ? false : dr["SSL"]),
        //                    TakeASentEmailCopy = Convert.ToBoolean(DBNull.Value.Equals(dr["TakeASentEmailCopy"]) ? false : dr["TakeASentEmailCopy"]),
        //                    BccEmail = Convert.ToString(dr["BccEmail"]),
        //                    UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
        //                });
        //            }

        //            foreach (DataRow dr in dataSet.Tables[2].Rows)
        //            {
        //                _lstTable3.Add(new GetUserPermissionByUserIDTable3()
        //                {
        //                    department = Convert.ToInt32(DBNull.Value.Equals(dr["department"]) ? 0 : dr["department"]),
        //                });
        //            }

        //            _ds.lstTable1 = _lstTable1;
        //            _ds.lstTable2 = _lstTable2;
        //            _ds.lstTable3 = _lstTable3;

        //            return _ds;
        //        }
        //        catch (Exception)
        //        {
        //            DataSet ds = objDL_User.getUserByID(_GetUserById, ConnectionString);

        //            ListGetUserPermissionByUserID _ds = new ListGetUserPermissionByUserID();
        //            List<GetUserPermissionByUserIDTable1> _lstTable1 = new List<GetUserPermissionByUserIDTable1>();
        //            List<GetUserPermissionByUserIDTable2> _lstTable2 = new List<GetUserPermissionByUserIDTable2>();
        //            List<GetUserPermissionByUserIDTable3> _lstTable3 = new List<GetUserPermissionByUserIDTable3>();

        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                _lstTable1.Add(new GetUserPermissionByUserIDTable1()
        //                {
        //                    fStart = Convert.ToDateTime(DBNull.Value.Equals(dr["fStart"]) ? null : dr["fStart"]),
        //                    fEnd = Convert.ToDateTime(DBNull.Value.Equals(dr["fEnd"]) ? null : dr["fEnd"]),
        //                    PDASerialNumber = Convert.ToString(dr["PDASerialNumber"]),
        //                    Empid = Convert.ToInt32(DBNull.Value.Equals(dr["Empid"]) ? 0 : dr["Empid"]),
        //                    userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
        //                    rolid = Convert.ToInt32(DBNull.Value.Equals(dr["rolid"]) ? 0 : dr["rolid"]),
        //                    workID = Convert.ToInt32(DBNull.Value.Equals(dr["workID"]) ? 0 : dr["workID"]),
        //                    fUser = Convert.ToString(dr["fUser"]),
        //                    Password = Convert.ToString(dr["Password"]),
        //                    Dispatch = Convert.ToString(dr["Dispatch"]),
        //                    Location = Convert.ToString(dr["Location"]),
        //                    PO = Convert.ToString(dr["PO"]),
        //                    Control = Convert.ToString(dr["Control"]),
        //                    UserS = Convert.ToString(dr["UserS"]),
        //                    City = Convert.ToString(dr["City"]),
        //                    State = Convert.ToString(dr["State"]),
        //                    Zip = Convert.ToString(dr["Zip"]),
        //                    Phone = Convert.ToString(dr["Phone"]),
        //                    Address = Convert.ToString(dr["Address"]),
        //                    EMail = Convert.ToString(dr["EMail"]),
        //                    Cellular = Convert.ToString(dr["Cellular"]),
        //                    Field = Convert.ToInt32(DBNull.Value.Equals(dr["Field"]) ? 0 : dr["Field"]),
        //                    fFirst = Convert.ToString(dr["fFirst"]),
        //                    Middle = Convert.ToString(dr["Middle"]),
        //                    Last = Convert.ToString(dr["Last"]),
        //                    DHired = Convert.ToDateTime(DBNull.Value.Equals(dr["DHired"]) ? null : dr["DHired"]),
        //                    DFired = Convert.ToDateTime(DBNull.Value.Equals(dr["DFired"]) ? null : dr["DFired"]),
        //                    CallSign = Convert.ToString(dr["CallSign"]),
        //                    Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
        //                    fWork = Convert.ToInt32(DBNull.Value.Equals(dr["fWork"]) ? 0 : dr["fWork"]),
        //                    Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
        //                    Remarks = Convert.ToString(dr["Remarks"]),
        //                    ticketo = Convert.ToString(dr["ticketo"]),
        //                    ticketd = Convert.ToString(dr["ticketd"]),
        //                    ticket = Convert.ToString(dr["ticket"]),
        //                    UserSales = Convert.ToString(dr["UserSales"]),
        //                    employeeMaint = Convert.ToString(dr["employeeMaint"]),
        //                    TC = Convert.ToString(dr["TC"]),
        //                    pager = Convert.ToString(dr["pager"]),
        //                    super = Convert.ToString(dr["super"]),
        //                    sales = Convert.ToInt16(DBNull.Value.Equals(dr["sales"]) ? 0 : dr["sales"]),
        //                    Lang = Convert.ToString(dr["Lang"]),
        //                    merchantinfoid = Convert.ToInt32(DBNull.Value.Equals(dr["merchantinfoid"]) ? 0 : dr["merchantinfoid"]),
        //                    dboard = Convert.ToInt16(DBNull.Value.Equals(dr["dboard"]) ? 0 : dr["dboard"]),
        //                    DefaultWorker = Convert.ToInt32(DBNull.Value.Equals(dr["DefaultWorker"]) ? 0 : dr["DefaultWorker"]),
        //                    massreview = Convert.ToInt32(DBNull.Value.Equals(dr["massreview"]) ? 0 : dr["massreview"]),
        //                    msmpass = Convert.ToString(dr["msmpass"]),
        //                    msmuser = Convert.ToString(dr["msmuser"]),
        //                    emailaccount = Convert.ToInt16(DBNull.Value.Equals(dr["emailaccount"]) ? 0 : dr["emailaccount"]),
        //                    hourlyrate = Convert.ToDouble(DBNull.Value.Equals(dr["hourlyrate"]) ? 0 : dr["hourlyrate"]),
        //                    pmethod = Convert.ToInt16(DBNull.Value.Equals(dr["pmethod"]) ? 0 : dr["pmethod"]),
        //                    phour = Convert.ToDouble(DBNull.Value.Equals(dr["phour"]) ? 0 : dr["phour"]),
        //                    salary = Convert.ToDouble(DBNull.Value.Equals(dr["salary"]) ? 0 : dr["salary"]),
        //                    payperiod = Convert.ToInt16(DBNull.Value.Equals(dr["payperiod"]) ? 0 : dr["payperiod"]),
        //                    mileagerate = Convert.ToDouble(DBNull.Value.Equals(dr["mileagerate"]) ? 0 : dr["mileagerate"]),
        //                    Ref = Convert.ToString(dr["Ref"]),
        //                    elevator = Convert.ToString(dr["elevator"]),
        //                    Chart = Convert.ToString(dr["Chart"]),
        //                    GLAdj = Convert.ToString(dr["GLAdj"]),
        //                    CustomerPayment = Convert.ToString(dr["CustomerPayment"]),
        //                    Deposit = Convert.ToString(dr["Deposit"]),
        //                    Financial = Convert.ToString(dr["Financial"]),
        //                    Vendor = Convert.ToString(dr["Vendor"]),
        //                    Bill = Convert.ToString(dr["Bill"]),
        //                    BillSelect = Convert.ToString(dr["BillSelect"]),
        //                    BillPay = Convert.ToString(dr["BillPay"]),
        //                    Owner = Convert.ToString(dr["Owner"]),
        //                    Job = Convert.ToString(dr["Job"]),
        //                    MSAuthorisedDeviceOnly = Convert.ToInt32(DBNull.Value.Equals(dr["MSAuthorisedDeviceOnly"]) ? 0 : dr["MSAuthorisedDeviceOnly"]),
        //                    ProjectListPermission = Convert.ToString(dr["ProjectListPermission"]),
        //                    FinancePermission = Convert.ToString(dr["FinancePermission"]),
        //                    BOMPermission = Convert.ToString(dr["BOMPermission"]),
        //                    MilestonesPermission = Convert.ToString(dr["MilestonesPermission"]),
        //                    Item = Convert.ToString(dr["Item"]),
        //                    InvAdj = Convert.ToString(dr["InvAdj"]),
        //                    Warehouse = Convert.ToString(dr["Warehouse"]),
        //                    InvSetup = Convert.ToString(dr["InvSetup"]),
        //                    InvViewer = Convert.ToString(dr["InvViewer"]),
        //                    DocumentPermission = Convert.ToString(dr["DocumentPermission"]),
        //                    ContactPermission = Convert.ToString(dr["ContactPermission"]),
        //                    SalesAssigned = Convert.ToBoolean(DBNull.Value.Equals(dr["SalesAssigned"]) ? false : dr["SalesAssigned"]),
        //                    ProjecttempPermission = Convert.ToString(dr["ProjecttempPermission"]),
        //                    NotificationOnAddOpportunity = Convert.ToBoolean(DBNull.Value.Equals(dr["NotificationOnAddOpportunity"]) ? false : dr["NotificationOnAddOpportunity"]),
        //                    POLimit = Convert.ToDecimal(DBNull.Value.Equals(dr["POLimit"]) ? 0 : dr["POLimit"]),
        //                    POApprove = Convert.ToInt16(DBNull.Value.Equals(dr["POApprove"]) ? 0 : dr["POApprove"]),
        //                    POApproveAmt = Convert.ToInt16(DBNull.Value.Equals(dr["POApproveAmt"]) ? 0 : dr["POApproveAmt"]),
        //                    MinAmount = Convert.ToDecimal(DBNull.Value.Equals(dr["MinAmount"]) ? 0 : dr["MinAmount"]),
        //                    MaxAmount = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxAmount"]) ? 0 : dr["MaxAmount"]),
        //                    Lng = Convert.ToString(dr["Lng"]),
        //                    Lat = Convert.ToString(dr["Lat"]),
        //                    Country = Convert.ToString(dr["Country"]),
        //                    MSDeviceId = Convert.ToString(dr["MSDeviceId"]),
        //                    Website = Convert.ToString(dr["Website"]),
        //                    Contact = Convert.ToString(dr["Contact"]),
        //                    Title = Convert.ToString(dr["Title"]),
        //                    ProfileImage = Convert.ToString(dr["ProfileImage"]),
        //                    CoverImage = Convert.ToString(dr["CoverImage"]),
        //                    BillingCodesPermission = Convert.ToString(dr["BillingCodesPermission"]),
        //                    Invoice = Convert.ToString(dr["Invoice"]),
        //                    PurchasingmodulePermission = Convert.ToString(dr["PurchasingmodulePermission"]),
        //                    BillingmodulePermission = Convert.ToString(dr["BillingmodulePermission"]),
        //                    RPO = Convert.ToString(dr["RPO"]),
        //                    AccountPayablemodulePermission = Convert.ToString(dr["AccountPayablemodulePermission"]),
        //                    PaymentHistoryPermission = Convert.ToString(dr["PaymentHistoryPermission"]),
        //                    CustomermodulePermission = Convert.ToString(dr["CustomermodulePermission"]),
        //                    Apply = Convert.ToString(dr["Apply"]),
        //                    Collection = Convert.ToString(dr["Collection"]),
        //                    bankrec = Convert.ToString(dr["bankrec"]),
        //                    FinancialmodulePermission = Convert.ToString(dr["FinancialmodulePermission"]),
        //                    RCmodulePermission = Convert.ToString(dr["RCmodulePermission"]),
        //                    ProcessRCPermission = Convert.ToString(dr["ProcessRCPermission"]),
        //                    ProcessC = Convert.ToString(dr["ProcessC"]),
        //                    ProcessT = Convert.ToString(dr["ProcessT"]),
        //                    SafetyTestsPermission = Convert.ToString(dr["SafetyTestsPermission"]),
        //                    RCRenewEscalatePermission = Convert.ToString(dr["RCRenewEscalatePermission"]),
        //                    SchedulemodulePermission = Convert.ToString(dr["SchedulemodulePermission"]),
        //                    Resolve = Convert.ToString(dr["Resolve"]),
        //                    TicketPermission = Convert.ToString(dr["TicketPermission"]),
        //                    MTimesheet = Convert.ToString(dr["MTimesheet"]),
        //                    ETimesheet = Convert.ToString(dr["ETimesheet"]),
        //                    MapR = Convert.ToString(dr["MapR"]),
        //                    RouteBuilder = Convert.ToString(dr["RouteBuilder"]),
        //                    MassTimesheetCheck = Convert.ToString(dr["MassTimesheetCheck"]),
        //                    CreditHold = Convert.ToString(dr["CreditHold"]),
        //                    LocCount = Convert.ToInt32(DBNull.Value.Equals(dr["LocCount"]) ? 0 : dr["LocCount"]),
        //                    salesmanager = Convert.ToString(dr["salesmanager"]),
        //                    Sales = Convert.ToString(dr["Sales"]),
        //                    ToDo = Convert.ToInt16(DBNull.Value.Equals(dr["ToDo"]) ? 0 : dr["ToDo"]),
        //                    ToDoC = Convert.ToInt16(DBNull.Value.Equals(dr["ToDoC"]) ? 0 : dr["ToDoC"]),
        //                    FU = Convert.ToString(dr["FU"]),
        //                    Proposal = Convert.ToString(dr["Proposal"]),
        //                    Estimates = Convert.ToString(dr["Estimates"]),
        //                    AwardEstimates = Convert.ToString(dr["AwardEstimates"]),
        //                    salessetup = Convert.ToString(dr["salessetup"]),
        //                    PONotification = Convert.ToString(dr["PONotification"]),
        //                    WriteOff = Convert.ToString(dr["WriteOff"]),
        //                    SSN = Convert.ToString(dr["SSN"]),
        //                    Sex = Convert.ToString(dr["Sex"]),
        //                    DBirth = Convert.ToDateTime(DBNull.Value.Equals(dr["DBirth"]) ? null : dr["DBirth"]),
        //                    Race = Convert.ToString(dr["Race"]),
        //                    ProjectModulePermission = Convert.ToString(dr["ProjectModulePermission"]),
        //                    InventoryModulePermission = Convert.ToString(dr["InventoryModulePermission"]),
        //                    JobClosePermission = Convert.ToString(dr["JobClosePermission"]),
        //                    JobCompletedPermission = Convert.ToString(dr["JobCompletedPermission"]),
        //                    JobReopenPermission = Convert.ToString(dr["JobReopenPermission"]),
        //                    IsProjectManager = Convert.ToBoolean(DBNull.Value.Equals(dr["IsProjectManager"]) ? false : dr["IsProjectManager"]),
        //                    IsAssignedProject = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAssignedProject"]) ? false : dr["IsAssignedProject"]),
        //                    TicketVoidPermission = Convert.ToInt32(DBNull.Value.Equals(dr["TicketVoidPermission"]) ? null : dr["TicketVoidPermission"]),
        //                    Employee = Convert.ToString(dr["Employee"]),
        //                    PRProcess = Convert.ToString(dr["PRProcess"]),
        //                    PRRegister = Convert.ToString(dr["PRRegister"]),
        //                    PRReport = Convert.ToString(dr["PRReport"]),
        //                    PRWage = Convert.ToString(dr["PRWage"]),
        //                    PRDeduct = Convert.ToString(dr["PRDeduct"]),
        //                    PR = Convert.ToBoolean(DBNull.Value.Equals(dr["PR"]) ? false : dr["PR"]),
        //                    RoleId = Convert.ToInt32(DBNull.Value.Equals(dr["RoleId"]) ? null : dr["RoleId"]),
        //                    ApplyUserRolePermission = Convert.ToInt32(DBNull.Value.Equals(dr["ApplyUserRolePermission"]) ? null : dr["ApplyUserRolePermission"]),
        //                });
        //            }

        //            foreach (DataRow dr in ds.Tables[1].Rows)
        //            {
        //                _lstTable2.Add(new GetUserPermissionByUserIDTable2()
        //                {
        //                    ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
        //                    InServer = Convert.ToString(dr["InServer"]),
        //                    InServerType = Convert.ToString(dr["InServerType"]),
        //                    InUsername = Convert.ToString(dr["InUsername"]),
        //                    InPassword = Convert.ToString(dr["InPassword"]),
        //                    InPort = Convert.ToInt32(DBNull.Value.Equals(dr["InPort"]) ? 0 : dr["InPort"]),
        //                    OutServer = Convert.ToString(dr["OutServer"]),
        //                    OutUsername = Convert.ToString(dr["OutUsername"]),
        //                    OutPassword = Convert.ToString(dr["OutUsername"]),
        //                    OutPort = Convert.ToInt32(DBNull.Value.Equals(dr["OutPort"]) ? 0 : dr["OutPort"]),
        //                    SSL = Convert.ToBoolean(DBNull.Value.Equals(dr["SSL"]) ? false : dr["SSL"]),
        //                    TakeASentEmailCopy = Convert.ToBoolean(DBNull.Value.Equals(dr["TakeASentEmailCopy"]) ? false : dr["TakeASentEmailCopy"]),
        //                    BccEmail = Convert.ToString(dr["BccEmail"]),
        //                    UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
        //                });
        //            }

        //            foreach (DataRow dr in ds.Tables[2].Rows)
        //            {
        //                _lstTable3.Add(new GetUserPermissionByUserIDTable3()
        //                {
        //                    department = Convert.ToInt32(DBNull.Value.Equals(dr["department"]) ? 0 : dr["department"]),
        //                });
        //            }

        //            _ds.lstTable1 = _lstTable1;
        //            _ds.lstTable2 = _lstTable2;
        //            _ds.lstTable3 = _lstTable3;

        //            return _ds;

        //        }
        //    }
        //    else
        //    {
        //        DataSet ds = objDL_User.getUserByID(_GetUserById, ConnectionString);

        //        ListGetUserPermissionByUserID _ds = new ListGetUserPermissionByUserID();
        //        List<GetUserPermissionByUserIDTable1> _lstTable1 = new List<GetUserPermissionByUserIDTable1>();
        //        List<GetUserPermissionByUserIDTable2> _lstTable2 = new List<GetUserPermissionByUserIDTable2>();
        //        List<GetUserPermissionByUserIDTable3> _lstTable3 = new List<GetUserPermissionByUserIDTable3>();

        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            _lstTable1.Add(new GetUserPermissionByUserIDTable1()
        //            {
        //                fStart = Convert.ToDateTime(DBNull.Value.Equals(dr["fStart"]) ? null : dr["fStart"]),
        //                fEnd = Convert.ToDateTime(DBNull.Value.Equals(dr["fEnd"]) ? null : dr["fEnd"]),
        //                PDASerialNumber = Convert.ToString(dr["PDASerialNumber"]),
        //                Empid = Convert.ToInt32(DBNull.Value.Equals(dr["Empid"]) ? 0 : dr["Empid"]),
        //                userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
        //                rolid = Convert.ToInt32(DBNull.Value.Equals(dr["rolid"]) ? 0 : dr["rolid"]),
        //                workID = Convert.ToInt32(DBNull.Value.Equals(dr["workID"]) ? 0 : dr["workID"]),
        //                fUser = Convert.ToString(dr["fUser"]),
        //                Password = Convert.ToString(dr["Password"]),
        //                Dispatch = Convert.ToString(dr["Dispatch"]),
        //                Location = Convert.ToString(dr["Location"]),
        //                PO = Convert.ToString(dr["PO"]),
        //                Control = Convert.ToString(dr["Control"]),
        //                UserS = Convert.ToString(dr["UserS"]),
        //                City = Convert.ToString(dr["City"]),
        //                State = Convert.ToString(dr["State"]),
        //                Zip = Convert.ToString(dr["Zip"]),
        //                Phone = Convert.ToString(dr["Phone"]),
        //                Address = Convert.ToString(dr["Address"]),
        //                EMail = Convert.ToString(dr["EMail"]),
        //                Cellular = Convert.ToString(dr["Cellular"]),
        //                Field = Convert.ToInt32(DBNull.Value.Equals(dr["Field"]) ? 0 : dr["Field"]),
        //                fFirst = Convert.ToString(dr["fFirst"]),
        //                Middle = Convert.ToString(dr["Middle"]),
        //                Last = Convert.ToString(dr["Last"]),
        //                DHired = Convert.ToDateTime(DBNull.Value.Equals(dr["DHired"]) ? null : dr["DHired"]),
        //                DFired = Convert.ToDateTime(DBNull.Value.Equals(dr["DFired"]) ? null : dr["DFired"]),
        //                CallSign = Convert.ToString(dr["CallSign"]),
        //                Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
        //                fWork = Convert.ToInt32(DBNull.Value.Equals(dr["fWork"]) ? 0 : dr["fWork"]),
        //                Status = Convert.ToInt32(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
        //                Remarks = Convert.ToString(dr["Remarks"]),
        //                ticketo = Convert.ToString(dr["ticketo"]),
        //                ticketd = Convert.ToString(dr["ticketd"]),
        //                ticket = Convert.ToString(dr["ticket"]),
        //                UserSales = Convert.ToString(dr["UserSales"]),
        //                employeeMaint = Convert.ToString(dr["employeeMaint"]),
        //                TC = Convert.ToString(dr["TC"]),
        //                pager = Convert.ToString(dr["pager"]),
        //                super = Convert.ToString(dr["super"]),
        //                sales = Convert.ToInt16(DBNull.Value.Equals(dr["sales"]) ? 0 : dr["sales"]),
        //                Lang = Convert.ToString(dr["Lang"]),
        //                merchantinfoid = Convert.ToInt32(DBNull.Value.Equals(dr["merchantinfoid"]) ? 0 : dr["merchantinfoid"]),
        //                dboard = Convert.ToInt16(DBNull.Value.Equals(dr["dboard"]) ? 0 : dr["dboard"]),
        //                DefaultWorker = Convert.ToInt32(DBNull.Value.Equals(dr["DefaultWorker"]) ? 0 : dr["DefaultWorker"]),
        //                massreview = Convert.ToInt32(DBNull.Value.Equals(dr["massreview"]) ? 0 : dr["massreview"]),
        //                msmpass = Convert.ToString(dr["msmpass"]),
        //                msmuser = Convert.ToString(dr["msmuser"]),
        //                emailaccount = Convert.ToInt16(DBNull.Value.Equals(dr["emailaccount"]) ? 0 : dr["emailaccount"]),
        //                hourlyrate = Convert.ToDouble(DBNull.Value.Equals(dr["hourlyrate"]) ? 0 : dr["hourlyrate"]),
        //                pmethod = Convert.ToInt16(DBNull.Value.Equals(dr["pmethod"]) ? 0 : dr["pmethod"]),
        //                phour = Convert.ToDouble(DBNull.Value.Equals(dr["phour"]) ? 0 : dr["phour"]),
        //                salary = Convert.ToDouble(DBNull.Value.Equals(dr["salary"]) ? 0 : dr["salary"]),
        //                payperiod = Convert.ToInt16(DBNull.Value.Equals(dr["payperiod"]) ? 0 : dr["payperiod"]),
        //                mileagerate = Convert.ToDouble(DBNull.Value.Equals(dr["mileagerate"]) ? 0 : dr["mileagerate"]),
        //                Ref = Convert.ToString(dr["Ref"]),
        //                elevator = Convert.ToString(dr["elevator"]),
        //                Chart = Convert.ToString(dr["Chart"]),
        //                GLAdj = Convert.ToString(dr["GLAdj"]),
        //                CustomerPayment = Convert.ToString(dr["CustomerPayment"]),
        //                Deposit = Convert.ToString(dr["Deposit"]),
        //                Financial = Convert.ToString(dr["Financial"]),
        //                Vendor = Convert.ToString(dr["Vendor"]),
        //                Bill = Convert.ToString(dr["Bill"]),
        //                BillSelect = Convert.ToString(dr["BillSelect"]),
        //                BillPay = Convert.ToString(dr["BillPay"]),
        //                Owner = Convert.ToString(dr["Owner"]),
        //                Job = Convert.ToString(dr["Job"]),
        //                MSAuthorisedDeviceOnly = Convert.ToInt32(DBNull.Value.Equals(dr["MSAuthorisedDeviceOnly"]) ? 0 : dr["MSAuthorisedDeviceOnly"]),
        //                ProjectListPermission = Convert.ToString(dr["ProjectListPermission"]),
        //                FinancePermission = Convert.ToString(dr["FinancePermission"]),
        //                BOMPermission = Convert.ToString(dr["BOMPermission"]),
        //                MilestonesPermission = Convert.ToString(dr["MilestonesPermission"]),
        //                Item = Convert.ToString(dr["Item"]),
        //                InvAdj = Convert.ToString(dr["InvAdj"]),
        //                Warehouse = Convert.ToString(dr["Warehouse"]),
        //                InvSetup = Convert.ToString(dr["InvSetup"]),
        //                InvViewer = Convert.ToString(dr["InvViewer"]),
        //                DocumentPermission = Convert.ToString(dr["DocumentPermission"]),
        //                ContactPermission = Convert.ToString(dr["ContactPermission"]),
        //                SalesAssigned = Convert.ToBoolean(DBNull.Value.Equals(dr["SalesAssigned"]) ? false : dr["SalesAssigned"]),
        //                ProjecttempPermission = Convert.ToString(dr["ProjecttempPermission"]),
        //                NotificationOnAddOpportunity = Convert.ToBoolean(DBNull.Value.Equals(dr["NotificationOnAddOpportunity"]) ? false : dr["NotificationOnAddOpportunity"]),
        //                POLimit = Convert.ToDecimal(DBNull.Value.Equals(dr["POLimit"]) ? 0 : dr["POLimit"]),
        //                POApprove = Convert.ToInt16(DBNull.Value.Equals(dr["POApprove"]) ? 0 : dr["POApprove"]),
        //                POApproveAmt = Convert.ToInt16(DBNull.Value.Equals(dr["POApproveAmt"]) ? 0 : dr["POApproveAmt"]),
        //                MinAmount = Convert.ToDecimal(DBNull.Value.Equals(dr["MinAmount"]) ? 0 : dr["MinAmount"]),
        //                MaxAmount = Convert.ToDecimal(DBNull.Value.Equals(dr["MaxAmount"]) ? 0 : dr["MaxAmount"]),
        //                Lng = Convert.ToString(dr["Lng"]),
        //                Lat = Convert.ToString(dr["Lat"]),
        //                Country = Convert.ToString(dr["Country"]),
        //                MSDeviceId = Convert.ToString(dr["MSDeviceId"]),
        //                Website = Convert.ToString(dr["Website"]),
        //                Contact = Convert.ToString(dr["Contact"]),
        //                Title = Convert.ToString(dr["Title"]),
        //                ProfileImage = Convert.ToString(dr["ProfileImage"]),
        //                CoverImage = Convert.ToString(dr["CoverImage"]),
        //                BillingCodesPermission = Convert.ToString(dr["BillingCodesPermission"]),
        //                Invoice = Convert.ToString(dr["Invoice"]),
        //                PurchasingmodulePermission = Convert.ToString(dr["PurchasingmodulePermission"]),
        //                BillingmodulePermission = Convert.ToString(dr["BillingmodulePermission"]),
        //                RPO = Convert.ToString(dr["RPO"]),
        //                AccountPayablemodulePermission = Convert.ToString(dr["AccountPayablemodulePermission"]),
        //                PaymentHistoryPermission = Convert.ToString(dr["PaymentHistoryPermission"]),
        //                CustomermodulePermission = Convert.ToString(dr["CustomermodulePermission"]),
        //                Apply = Convert.ToString(dr["Apply"]),
        //                Collection = Convert.ToString(dr["Collection"]),
        //                bankrec = Convert.ToString(dr["bankrec"]),
        //                FinancialmodulePermission = Convert.ToString(dr["FinancialmodulePermission"]),
        //                RCmodulePermission = Convert.ToString(dr["RCmodulePermission"]),
        //                ProcessRCPermission = Convert.ToString(dr["ProcessRCPermission"]),
        //                ProcessC = Convert.ToString(dr["ProcessC"]),
        //                ProcessT = Convert.ToString(dr["ProcessT"]),
        //                SafetyTestsPermission = Convert.ToString(dr["SafetyTestsPermission"]),
        //                RCRenewEscalatePermission = Convert.ToString(dr["RCRenewEscalatePermission"]),
        //                SchedulemodulePermission = Convert.ToString(dr["SchedulemodulePermission"]),
        //                Resolve = Convert.ToString(dr["Resolve"]),
        //                TicketPermission = Convert.ToString(dr["TicketPermission"]),
        //                MTimesheet = Convert.ToString(dr["MTimesheet"]),
        //                ETimesheet = Convert.ToString(dr["ETimesheet"]),
        //                MapR = Convert.ToString(dr["MapR"]),
        //                RouteBuilder = Convert.ToString(dr["RouteBuilder"]),
        //                MassTimesheetCheck = Convert.ToString(dr["MassTimesheetCheck"]),
        //                CreditHold = Convert.ToString(dr["CreditHold"]),
        //                LocCount = Convert.ToInt32(DBNull.Value.Equals(dr["LocCount"]) ? 0 : dr["LocCount"]),
        //                salesmanager = Convert.ToString(dr["salesmanager"]),
        //                Sales = Convert.ToString(dr["Sales"]),
        //                ToDo = Convert.ToInt16(DBNull.Value.Equals(dr["ToDo"]) ? 0 : dr["ToDo"]),
        //                ToDoC = Convert.ToInt16(DBNull.Value.Equals(dr["ToDoC"]) ? 0 : dr["ToDoC"]),
        //                FU = Convert.ToString(dr["FU"]),
        //                Proposal = Convert.ToString(dr["Proposal"]),
        //                Estimates = Convert.ToString(dr["Estimates"]),
        //                AwardEstimates = Convert.ToString(dr["AwardEstimates"]),
        //                salessetup = Convert.ToString(dr["salessetup"]),
        //                PONotification = Convert.ToString(dr["PONotification"]),
        //                WriteOff = Convert.ToString(dr["WriteOff"]),
        //                SSN = Convert.ToString(dr["SSN"]),
        //                Sex = Convert.ToString(dr["Sex"]),
        //                DBirth = Convert.ToDateTime(DBNull.Value.Equals(dr["DBirth"]) ? null : dr["DBirth"]),
        //                Race = Convert.ToString(dr["Race"]),
        //                ProjectModulePermission = Convert.ToString(dr["ProjectModulePermission"]),
        //                InventoryModulePermission = Convert.ToString(dr["InventoryModulePermission"]),
        //                JobClosePermission = Convert.ToString(dr["JobClosePermission"]),
        //                JobCompletedPermission = Convert.ToString(dr["JobCompletedPermission"]),
        //                JobReopenPermission = Convert.ToString(dr["JobReopenPermission"]),
        //                IsProjectManager = Convert.ToBoolean(DBNull.Value.Equals(dr["IsProjectManager"]) ? false : dr["IsProjectManager"]),
        //                IsAssignedProject = Convert.ToBoolean(DBNull.Value.Equals(dr["IsAssignedProject"]) ? false : dr["IsAssignedProject"]),
        //                TicketVoidPermission = Convert.ToInt32(DBNull.Value.Equals(dr["TicketVoidPermission"]) ? null : dr["TicketVoidPermission"]),
        //                Employee = Convert.ToString(dr["Employee"]),
        //                PRProcess = Convert.ToString(dr["PRProcess"]),
        //                PRRegister = Convert.ToString(dr["PRRegister"]),
        //                PRReport = Convert.ToString(dr["PRReport"]),
        //                PRWage = Convert.ToString(dr["PRWage"]),
        //                PRDeduct = Convert.ToString(dr["PRDeduct"]),
        //                PR = Convert.ToBoolean(DBNull.Value.Equals(dr["PR"]) ? false : dr["PR"]),
        //                RoleId = Convert.ToInt32(DBNull.Value.Equals(dr["RoleId"]) ? null : dr["RoleId"]),
        //                ApplyUserRolePermission = Convert.ToInt32(DBNull.Value.Equals(dr["ApplyUserRolePermission"]) ? null : dr["ApplyUserRolePermission"]),
        //            });
        //        }

        //        foreach (DataRow dr in ds.Tables[1].Rows)
        //        {
        //            _lstTable2.Add(new GetUserPermissionByUserIDTable2()
        //            {
        //                ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
        //                InServer = Convert.ToString(dr["InServer"]),
        //                InServerType = Convert.ToString(dr["InServerType"]),
        //                InUsername = Convert.ToString(dr["InUsername"]),
        //                InPassword = Convert.ToString(dr["InPassword"]),
        //                InPort = Convert.ToInt32(DBNull.Value.Equals(dr["InPort"]) ? 0 : dr["InPort"]),
        //                OutServer = Convert.ToString(dr["OutServer"]),
        //                OutUsername = Convert.ToString(dr["OutUsername"]),
        //                OutPassword = Convert.ToString(dr["OutUsername"]),
        //                OutPort = Convert.ToInt32(DBNull.Value.Equals(dr["OutPort"]) ? 0 : dr["OutPort"]),
        //                SSL = Convert.ToBoolean(DBNull.Value.Equals(dr["SSL"]) ? false : dr["SSL"]),
        //                TakeASentEmailCopy = Convert.ToBoolean(DBNull.Value.Equals(dr["TakeASentEmailCopy"]) ? false : dr["TakeASentEmailCopy"]),
        //                BccEmail = Convert.ToString(dr["BccEmail"]),
        //                UserId = Convert.ToInt32(DBNull.Value.Equals(dr["UserId"]) ? 0 : dr["UserId"]),
        //            });
        //        }

        //        foreach (DataRow dr in ds.Tables[2].Rows)
        //        {
        //            _lstTable3.Add(new GetUserPermissionByUserIDTable3()
        //            {
        //                department = Convert.ToInt32(DBNull.Value.Equals(dr["department"]) ? 0 : dr["department"]),
        //            });
        //        }


        //        _ds.lstTable1 = _lstTable1;
        //        _ds.lstTable2 = _lstTable2;
        //        _ds.lstTable3 = _lstTable3;

        //        return _ds;
        //    }

        //}


        //API
        public List<UserViewModel> getUserByID(GetUserByIdParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getUserByID(objPropUser, ConnectionString);

            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstUserViewModel.Add(new UserViewModel()
                {
                    UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"]) ? 0 : dr["UserID"]),
                    TypeID = Convert.ToInt32(DBNull.Value.Equals(dr["TypeID"]) ? 0 : dr["TypeID"]),
                    DBName = Convert.ToString(dr["DbName"]),
                    Vendor = Convert.ToString(dr["Vendor"]),
                    AccountPayablemodulePermission = Convert.ToString(dr["AccountPayablemodulePermission"]),
                });
            }
            return _lstUserViewModel;
        }
        public DataSet GetUserInfoByID(User objPropUser)
        {

            return objDL_User.getUserByID(objPropUser);
        }


        public DataSet getSMTPByUserID(User objPropUser)
        {
            return objDL_User.getSMTPByUserID(objPropUser);
        }

        //api
        public List<SMTPEmailViewModel> getSMTPByUserID(GetSMTPByUserIDParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getSMTPByUserID(objPropUser, ConnectionString);
            List<SMTPEmailViewModel> _ticket = new List<SMTPEmailViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _ticket.Add(
                    new SMTPEmailViewModel()
                    {
                        Host = Convert.ToString(dr["Host"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        Password = Convert.ToString(dr["Password"]),
                        Port = Convert.ToInt32(DBNull.Value.Equals(dr["Port"]) ? 0 : dr["Port"]),
                        //SSL = Convert.ToBoolean(DBNull.Value.Equals(dr["SSL"]) ? 0 : dr["SSL"]),
                        SSL = Convert.ToString(dr["SSL"]),
                        From = Convert.ToString(dr["From"]),
                        BCCEmail = Convert.ToString(dr["BCCEmail"])
                    });
            }
            return _ticket;
        }

        public string getUserLangByID(User objPropUser)
        {
            return objDL_User.getUserLangByID(objPropUser);
        }

        public DataSet getCustomerByID(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getCustomerByID(objPropUser, IsSalesAsigned);
        }

        //API
        public ListGetCustomerByID getCustomerByID(GetCustomerByIDParam _GetCustomerByID, Int32 IsSalesAsigned, string ConnectionString)
        {
            DataSet ds = objDL_User.getCustomerByID(_GetCustomerByID, IsSalesAsigned, ConnectionString);

            ListGetCustomerByID _lstGetCustomerByID = new ListGetCustomerByID();
            List<GetCustomerByIDTable1> _lstTable1 = new List<GetCustomerByIDTable1>();
            List<GetCustomerByIDTable2> _lstTable2 = new List<GetCustomerByIDTable2>();
            List<GetCustomerByIDTable3> _lstTable3 = new List<GetCustomerByIDTable3>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetCustomerByIDTable1()
                    {
                        Address = Convert.ToString(dr["Address"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Password = Convert.ToString(dr["Password"]),
                        Billing = Convert.ToInt16(DBNull.Value.Equals(dr["Billing"]) ? 0 : dr["Billing"]),
                        BillRate = Convert.ToDouble(DBNull.Value.Equals(dr["BillRate"]) ? 0 : dr["BillRate"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        Central = Convert.ToInt32(DBNull.Value.Equals(dr["Central"]) ? 0 : dr["Central"]),
                        City = Convert.ToString(dr["City"]),
                        CNotes = Convert.ToString(dr["CNotes"]),
                        Company = Convert.ToString(dr["Company"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Country = Convert.ToString(dr["Country"]),
                        CoverImage = Convert.ToString(dr["CoverImage"]),
                        CPEquipment = Convert.ToInt16(DBNull.Value.Equals(dr["CPEquipment"]) ? 0 : dr["CPEquipment"]),
                        Custom1 = Convert.ToString(dr["Custom1"]),
                        Custom2 = Convert.ToString(dr["Custom2"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        fax = Convert.ToString(dr["fax"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        fLogin = Convert.ToString(dr["fLogin"]),
                        GeoLock = Convert.ToInt16(DBNull.Value.Equals(dr["GeoLock"]) ? 0 : dr["GeoLock"]),
                        GroupbyWO = Convert.ToInt16(DBNull.Value.Equals(dr["GroupbyWO"]) ? 0 : dr["GroupbyWO"]),
                        Internet = Convert.ToInt16(DBNull.Value.Equals(dr["Internet"]) ? 0 : dr["Internet"]),
                        Lat = Convert.ToString(dr["Lat"]),
                        ledger = Convert.ToInt16(DBNull.Value.Equals(dr["ledger"]) ? 0 : dr["ledger"]),
                        Lng = Convert.ToString(dr["Lng"]),
                        msmpass = Convert.ToString(dr["msmpass"]),
                        msmuser = Convert.ToString(dr["msmuser"]),
                        Name = Convert.ToString(dr["Name"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        openticket = Convert.ToInt16(DBNull.Value.Equals(dr["openticket"]) ? 0 : dr["openticket"]),
                        ProfileImage = Convert.ToString(dr["ProfileImage"]),
                        QBcustomerID = Convert.ToString(dr["QBcustomerID"]),
                        ownerid = Convert.ToString(dr["ownerid"]),
                        RateDT = Convert.ToDouble(DBNull.Value.Equals(dr["RateDT"]) ? 0 : dr["RateDT"]),
                        RateMileage = Convert.ToDouble(DBNull.Value.Equals(dr["RateMileage"]) ? 0 : dr["RateMileage"]),
                        RateNT = Convert.ToDouble(DBNull.Value.Equals(dr["RateNT"]) ? 0 : dr["RateNT"]),
                        RateOT = Convert.ToDouble(DBNull.Value.Equals(dr["RateOT"]) ? 0 : dr["RateOT"]),
                        RateTravel = Convert.ToDouble(DBNull.Value.Equals(dr["RateTravel"]) ? 0 : dr["RateTravel"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        sageid = Convert.ToString(dr["sageid"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        State = Convert.ToString(dr["State"]),
                        TicketD = Convert.ToInt16(DBNull.Value.Equals(dr["TicketD"]) ? 0 : dr["TicketD"]),
                        TicketO = Convert.ToInt16(DBNull.Value.Equals(dr["TicketO"]) ? 0 : dr["TicketO"]),
                        Type = Convert.ToString(dr["Type"]),
                        Title = Convert.ToString(dr["Title"]),
                        Website = Convert.ToString(dr["Website"]),
                        Zip = Convert.ToString(dr["Zip"]),

                    });
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetCustomerByIDTable2()
                    {
                        Title = Convert.ToString(dr["Title"]),
                        Cell = Convert.ToString(dr["Cell"]),
                        contactid = Convert.ToInt32(DBNull.Value.Equals(dr["contactid"]) ? 0 : dr["contactid"]),
                        Email = Convert.ToString(dr["Email"]),
                        EmailRecInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecInvoice"]) ? false : dr["EmailRecInvoice"]),
                        EmailRecTestProp = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecTestProp"]) ? false : dr["EmailRecTestProp"]),
                        EmailTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailTicket"]) ? false : dr["EmailTicket"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        name = Convert.ToString(dr["name"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        ShutdownAlert = Convert.ToBoolean(DBNull.Value.Equals(dr["ShutdownAlert"]) ? false : dr["ShutdownAlert"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable3.Add(
                    new GetCustomerByIDTable3()
                    {
                        Address = Convert.ToString(dr["Address"]),
                        City = Convert.ToString(dr["City"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Elevs = Convert.ToInt16(DBNull.Value.Equals(dr["Elevs"]) ? 0 : dr["Elevs"]),
                        locid = Convert.ToString(dr["locid"]),
                        Name = Convert.ToString(dr["Name"]),
                        roleid = Convert.ToInt32(DBNull.Value.Equals(dr["roleid"]) ? 0 : dr["roleid"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Type = Convert.ToString(dr["Type"]),
                    });
            }


            _lstGetCustomerByID.lstTable1 = _lstTable1;
            _lstGetCustomerByID.lstTable2 = _lstTable2;
            _lstGetCustomerByID.lstTable3 = _lstTable3;

            return _lstGetCustomerByID;
        }

        public DataSet GetCustomerLocationContacts(User objPropUser)
        {
            return objDL_User.GetCustomerLocationContacts(objPropUser);
        }

        public DataSet getCustomerAddress(User objPropUser)
        {
            return objDL_User.getCustomerAddress(objPropUser);
        }

        public string getCustomerEmail(User objPropUser)
        {
            return objDL_User.getCustomerEmail(objPropUser);
        }

        public DataSet getequipByID(User objPropUser)
        {
            return objDL_User.getequipByID(objPropUser);
        }
        public DataSet GetAllEquipHasTheSameTest(String conn, int Loc, int testType, int year, bool Chargeable,string Classification,int ElevID)
        {
            return objDL_User.GetAllEquipHasTheSameTest( conn,  Loc,  testType, year, Chargeable, Classification, ElevID);
        }
        public DataSet GetAllEquipmentHaveSameTestCover(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            return objDL_User.GetAllEquipmentHaveSameTestCover(conn, Loc, testType, year, Chargeable, Classification);
        }
        public DataSet GetExistTestCoverInLocByTestType(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            return objDL_User.GetExistTestCoverInLocByTestType(conn, Loc, testType, year, Chargeable, Classification);
        }

        public DataSet GetAllTestCoverInLocationWithClassification(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            return objDL_User.GetAllTestCoverInLocationWithClassification(conn, Loc, testType, year, Chargeable, Classification);
        }
        public DataSet GetAllEquipmentCoverByTest(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            return objDL_User.GetAllEquipmentCoverByTest(conn, Loc, testType, year, Chargeable, Classification);
        }
        public DataSet GetAllEquipmentHaveSameTestChargable(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            return objDL_User.GetAllEquipmentHaveSameTestChargable(conn, Loc, testType, year, Chargeable, Classification);
        }

        public DataSet GetExistTestInLocByTestTypeAndChargable(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            return objDL_User.GetExistTestInLocByTestTypeAndChargable(conn, Loc, testType, year, Chargeable, Classification);
        }
        public DataSet GetAllTestInLocationByChargableAndClassification(String conn, int Loc, int testType, int year, bool Chargeable, string Classification)
        {
            return objDL_User.GetAllTestInLocationByChargableAndClassification(conn, Loc, testType, year, Chargeable, Classification);
        }
        //API
        public ListGetequipByID getequipByID(GetequipByIDParam _GetequipByID, string ConnectionString)
        {
            DataSet ds = objDL_User.getequipByID(_GetequipByID, ConnectionString);

            ListGetequipByID _ds = new ListGetequipByID();
            List<GetequipByIDTable1> _lstTable1 = new List<GetequipByIDTable1>();
            List<GetequipByIDTable2> _lstTable2 = new List<GetequipByIDTable2>();
            List<GetequipByIDTable3> _lstTable3 = new List<GetequipByIDTable3>();
            List<GetequipByIDTable4> _lstTable4 = new List<GetequipByIDTable4>();
            List<GetequipByIDTable5> _lstTable5 = new List<GetequipByIDTable5>();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetequipByIDTable1()
                    {
                        location = Convert.ToString(dr["location"]),
                        locationID = Convert.ToString(dr["locationID"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        Unit = Convert.ToString(dr["Unit"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Type = Convert.ToString(dr["Type"]),
                        Cat = Convert.ToString(dr["Cat"]),
                        Manuf = Convert.ToString(dr["Manuf"]),
                        Serial = Convert.ToString(dr["Serial"]),
                        State = Convert.ToString(dr["State"]),
                        Since = Convert.ToDateTime(DBNull.Value.Equals(dr["Since"]) ? null : dr["Since"]),
                        Last = Convert.ToDateTime(DBNull.Value.Equals(dr["Last"]) ? null : dr["Last"]),
                        Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Building = Convert.ToString(dr["Building"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        fGroup = Convert.ToString(dr["fGroup"]),
                        Template = Convert.ToInt32(DBNull.Value.Equals(dr["Template"]) ? 0 : dr["Template"]),
                        InstallBy = Convert.ToString(dr["InstallBy"]),
                        install = Convert.ToDateTime(DBNull.Value.Equals(dr["install"]) ? null : dr["install"]),
                        category = Convert.ToString(dr["category"]),
                        unitid = Convert.ToInt32(DBNull.Value.Equals(dr["unitid"]) ? 0 : dr["unitid"]),
                        Classification = Convert.ToString(dr["Classification"]),
                        shut_down = Convert.ToBoolean(DBNull.Value.Equals(dr["shut_down"]) ? false : dr["shut_down"]),
                        ShutdownReason = Convert.ToString(dr["ShutdownReason"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetequipByIDTable2()
                    {
                        Code = Convert.ToString(dr["Code"]),
                        Name = Convert.ToString(dr["Name"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        EquipT = Convert.ToInt32(DBNull.Value.Equals(dr["EquipT"]) ? 0 : dr["EquipT"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Lastdate = Convert.ToDateTime(DBNull.Value.Equals(dr["Lastdate"]) ? null : dr["Lastdate"]),
                        NextDateDue = Convert.ToDateTime(DBNull.Value.Equals(dr["NextDateDue"]) ? null : dr["NextDateDue"]),
                        Frequency = Convert.ToInt32(DBNull.Value.Equals(dr["Frequency"]) ? 0 : dr["Frequency"]),
                        Section = Convert.ToString(dr["Section"]),
                        Notes = Convert.ToString(dr["Notes"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable3.Add(
                    new GetequipByIDTable3()
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
                        OrderNo1 = Convert.ToInt32(DBNull.Value.Equals(dr["OrderNo"]) ? 0 : dr["OrderNo"]),
                        LeadEquip = Convert.ToInt16(DBNull.Value.Equals(dr["LeadEquip"]) ? 0 : dr["LeadEquip"]),
                        formatMOM = Convert.ToString(dr["formatMOM"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable4.Add(
                    new GetequipByIDTable4()
                    {
                        ElevT = Convert.ToInt32(DBNull.Value.Equals(dr["ElevT"]) ? 0 : dr["ElevT"]),
                        ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Value = Convert.ToString(dr["Value"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable5.Add(
                    new GetequipByIDTable5()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    });
            }

            _ds.lstTable1= _lstTable1;
            _ds.lstTable2 = _lstTable2;
            _ds.lstTable3 = _lstTable3;
            _ds.lstTable4 = _lstTable4;
            _ds.lstTable5 = _lstTable5;

            return _ds;
        }

        public DataSet getLeadEquipByID(User objPropUser)
        {
            return objDL_User.getLeadEquipByID(objPropUser);
        }

        //API
        public ListGetLeadEquipByID getLeadEquipByID(GetLeadEquipByIDParam _GetLeadEquipByID, string ConnectionString)
        {
            DataSet ds = objDL_User.getLeadEquipByID(_GetLeadEquipByID, ConnectionString);

            ListGetLeadEquipByID _lstGetLeadEquipByID = new ListGetLeadEquipByID();
            List<GetLeadEquipByIDTable> _lstTable = new List<GetLeadEquipByIDTable>();
            List<GetLeadEquipByIDTable1> _lstTable1 = new List<GetLeadEquipByIDTable1>();
            List<GetLeadEquipByIDTable2> _lstTable2 = new List<GetLeadEquipByIDTable2>();
            List<GetLeadEquipByIDTable3> _lstTable3 = new List<GetLeadEquipByIDTable3>();
            List<GetLeadEquipByIDTable4> _lstTable4 = new List<GetLeadEquipByIDTable4>();
            

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable.Add(
                    new GetLeadEquipByIDTable()
                    {
                        Lead = Convert.ToInt32(DBNull.Value.Equals(dr["Lead"]) ? 0 : dr["Lead"]),
                        Owner = Convert.ToInt32(DBNull.Value.Equals(dr["Owner"]) ? 0 : dr["Owner"]),
                        Unit = Convert.ToString(dr["Unit"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Type = Convert.ToString(dr["Type"]),
                        Cat = Convert.ToString(dr["Cat"]),
                        Manuf = Convert.ToString(dr["Manuf"]),
                        Serial = Convert.ToString(dr["Serial"]),
                        State = Convert.ToString(dr["State"]),
                        Since = Convert.ToDateTime(DBNull.Value.Equals(dr["Since"]) ? null : dr["Since"]),
                        Last = Convert.ToDateTime(DBNull.Value.Equals(dr["Last"]) ? null : dr["Last"]),
                        Price = Convert.ToDouble(DBNull.Value.Equals(dr["Price"]) ? 0 : dr["Price"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Building = Convert.ToString(dr["Building"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        fGroup = Convert.ToString(dr["fGroup"]),
                        Template = Convert.ToInt32(DBNull.Value.Equals(dr["Template"]) ? 0 : dr["Template"]),
                        InstallBy = Convert.ToString(dr["InstallBy"]),
                        install = Convert.ToDateTime(DBNull.Value.Equals(dr["install"]) ? null : dr["install"]),
                        category = Convert.ToString(dr["category"]),
                        unitid = Convert.ToInt32(DBNull.Value.Equals(dr["unitid"]) ? 0 : dr["unitid"]),
                        Classification = Convert.ToString(dr["Classification"]),
                        shut_down = Convert.ToBoolean(DBNull.Value.Equals(dr["shut_down"]) ? false : dr["shut_down"]),
                        ShutdownReason = Convert.ToString(dr["ShutdownReason"]),
                    });
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable1.Add(
                    new GetLeadEquipByIDTable1()
                    {
                        Code = Convert.ToString(dr["Code"]),
                        Name = Convert.ToString(dr["Name"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        EquipT = Convert.ToInt32(DBNull.Value.Equals(dr["EquipT"]) ? 0 : dr["EquipT"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Lastdate = Convert.ToDateTime(DBNull.Value.Equals(dr["Lastdate"]) ? null : dr["Lastdate"]),
                        NextDateDue = Convert.ToDateTime(DBNull.Value.Equals(dr["NextDateDue"]) ? null : dr["NextDateDue"]),
                        Frequency = Convert.ToInt32(DBNull.Value.Equals(dr["Frequency"]) ? 0 : dr["Frequency"]),
                        Section = Convert.ToString(dr["Section"]),
                        Notes = Convert.ToString(dr["Notes"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable2.Add(
                    new GetLeadEquipByIDTable2()
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
                    });
            }

            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable3.Add(
                    new GetLeadEquipByIDTable3()
                    {
                        ElevT = Convert.ToInt32(DBNull.Value.Equals(dr["ElevT"]) ? 0 : dr["ElevT"]),
                        ItemID = Convert.ToInt32(DBNull.Value.Equals(dr["ItemID"]) ? 0 : dr["ItemID"]),
                        Line = Convert.ToInt16(DBNull.Value.Equals(dr["Line"]) ? 0 : dr["Line"]),
                        Value = Convert.ToString(dr["Value"]),
                    });
            }

            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable4.Add(
                    new GetLeadEquipByIDTable4()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    });
            }

            _lstGetLeadEquipByID.lstTable = _lstTable;
            _lstGetLeadEquipByID.lstTable1 = _lstTable1;
            _lstGetLeadEquipByID.lstTable2 = _lstTable2;
            _lstGetLeadEquipByID.lstTable3 = _lstTable3;
            _lstGetLeadEquipByID.lstTable4 = _lstTable4;

            return _lstGetLeadEquipByID;
        }

        public DataSet getequipREPDetails(User objPropUser)
        {
            return objDL_User.getequipREPDetails(objPropUser);
        }

        //API
        public List<GetequipREPDetailsViewModel> getequipREPDetails(GetequipREPDetailsParam _GetequipREPDetails, string ConnectionString)
        {
            DataSet ds = objDL_User.getequipREPDetails(_GetequipREPDetails, ConnectionString);

            List<GetequipREPDetailsViewModel> _lstGetequipREPDetails = new List<GetequipREPDetailsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetequipREPDetails.Add(
                    new GetequipREPDetailsViewModel()
                    {
                        comp = Convert.ToInt32(DBNull.Value.Equals(dr["comp"]) ? 0 : dr["comp"]),
                        fwork = Convert.ToString(dr["Type"]),
                        Template = Convert.ToString(dr["Type"]),
                        Lastdate = Convert.ToDateTime(DBNull.Value.Equals(dr["Lastdate"]) ? null : dr["Lastdate"]),
                        NextDateDue = Convert.ToDateTime(DBNull.Value.Equals(dr["NextDateDue"]) ? null : dr["NextDateDue"]),
                        ticketID = Convert.ToInt32(DBNull.Value.Equals(dr["ticketID"]) ? 0 : dr["ticketID"]),
                        Code = Convert.ToString(dr["Code"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        freq = Convert.ToString(dr["freq"]),
                        equip = Convert.ToString(dr["equip"]),
                        status = Convert.ToString(dr["status"]),
                        comment = Convert.ToString(dr["comment"]),
                        section = Convert.ToString(dr["section"]),
                    }
                    );
            }

            return _lstGetequipREPDetails;
        }

        public DataSet getCustomerForReport(User objPropUser)
        {
            return objDL_User.getCustomerForReport(objPropUser);
        }


        public DataSet getLocationByCustomerID(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocationByCustomerID(objPropUser, IsSalesAsigned);
        }
        public DataSet getLocationActiveByCustomerID(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLocationActiveByCustomerID(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetLocationByCustomerIDViewModel> getLocationByCustomerID(GetLocationByCustomerIDParam _GetLocationByCustomerID, string ConnectionString)
        {
            DataSet ds = objDL_User.getLocationByCustomerID(_GetLocationByCustomerID, ConnectionString);

            List<GetLocationByCustomerIDViewModel> _lstGetLocationByCustomerID = new List<GetLocationByCustomerIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationByCustomerID.Add(
                    new GetLocationByCustomerIDViewModel()
                    {
                        Address = Convert.ToString(dr["Address"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        City = Convert.ToString(dr["City"]),
                        Elevs = Convert.ToInt32(DBNull.Value.Equals(dr["Elevs"]) ? 0 : dr["Elevs"]),
                        loc = Convert.ToInt32(DBNull.Value.Equals(dr["loc"]) ? 0 : dr["loc"]),
                        locid = Convert.ToString(dr["locid"]),
                        Name = Convert.ToString(dr["Name"]),
                        roleid = Convert.ToInt32(DBNull.Value.Equals(dr["roleid"]) ? 0 : dr["roleid"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        Type = Convert.ToString(dr["Type"]),
                    }
                    );
            }

            return _lstGetLocationByCustomerID;

        }

        public DataSet getLocationByID(User objPropUser)
        {
            return objDL_User.getLocationByID(objPropUser);
        }

        //API
        public ListGetLocationByID getLocationByID(GetLocationByIDParam _GetLocationByID, string ConnectionString)
        {
            DataSet ds = objDL_User.getLocationByID(_GetLocationByID, ConnectionString);

            ListGetLocationByID _ds = new ListGetLocationByID();
            List<GetLocationByIDTable1> _lstTable1=  new List<GetLocationByIDTable1>();
            List<GetLocationByIDTable2> _lstTable2 = new List<GetLocationByIDTable2>();
            List<GetLocationByIDTable3> _lstTable3 = new List<GetLocationByIDTable3>();
            List<GetLocationByIDTable4> _lstTable4 = new List<GetLocationByIDTable4>();
            List<GetLocationByIDTable5> _lstTable5 = new List<GetLocationByIDTable5>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetLocationByIDTable1()
                    {
                        Consult = Convert.ToInt32(DBNull.Value.Equals(dr["Consult"]) ? 0 : dr["Consult"]),
                        ID = Convert.ToString(dr["ID"]),
                        Tag = Convert.ToString(dr["Tag"]),
                        locAddress = Convert.ToString(dr["locAddress"]),
                        locCity = Convert.ToString(dr["locCity"]),
                        locState = Convert.ToString(dr["locState"]),
                        locZip = Convert.ToString(dr["locZip"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
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
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        owner = Convert.ToInt32(DBNull.Value.Equals(dr["owner"]) ? 0 : dr["owner"]),
                        custname = Convert.ToString(dr["custname"]),
                        stax = Convert.ToString(dr["stax"]),
                        STax2 = Convert.ToString(dr["STax2"]),
                        UTax = Convert.ToString(dr["UTax"]),
                        Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
                        locCountry = Convert.ToString(dr["locCountry"]),
                        Lat = Convert.ToString(dr["Lat"]),
                        Lng = Convert.ToString(dr["Lng"]),
                        custom1 = Convert.ToString(dr["custom1"]),
                        custom2 = Convert.ToString(dr["custom2"]),
                        custom14 = Convert.ToString(dr["custom14"]),
                        custom15 = Convert.ToString(dr["custom15"]),
                        custom12 = Convert.ToString(dr["custom12"]),
                        custom13 = Convert.ToString(dr["custom13"]),
                        status = Convert.ToInt16(DBNull.Value.Equals(dr["status"]) ? 0 : dr["status"]),
                        defwork = Convert.ToString(dr["defwork"]),
                        credit = Convert.ToByte(DBNull.Value.Equals(dr["credit"]) ? 0 : dr["credit"]),
                        dispalert = Convert.ToByte(DBNull.Value.Equals(dr["dispalert"]) ? 0 : dr["dispalert"]),
                        creditreason = Convert.ToString(dr["creditreason"]),
                        custsageid = Convert.ToString(dr["custsageid"]),
                        Billing = Convert.ToInt16(DBNull.Value.Equals(dr["Billing"]) ? 0 : dr["Billing"]),
                        qblocid = Convert.ToString(dr["qblocid"]),
                        defaultterms = Convert.ToInt32(DBNull.Value.Equals(dr["defaultterms"]) ? 0 : dr["defaultterms"]),
                        BillRate = Convert.ToDouble(DBNull.Value.Equals(dr["BillRate"]) ? 0 : dr["BillRate"]),
                        RateOT = Convert.ToDouble(DBNull.Value.Equals(dr["RateOT"]) ? 0 : dr["RateOT"]),
                        RateNT = Convert.ToDouble(DBNull.Value.Equals(dr["RateNT"]) ? 0 : dr["RateNT"]),
                        RateDT = Convert.ToDouble(DBNull.Value.Equals(dr["RateDT"]) ? 0 : dr["RateDT"]),
                        RateTravel = Convert.ToDouble(DBNull.Value.Equals(dr["RateTravel"]) ? 0 : dr["RateTravel"]),
                        RateMileage = Convert.ToDouble(DBNull.Value.Equals(dr["RateMileage"]) ? 0 : dr["RateMileage"]),
                        Rate = Convert.ToDouble(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                        GstRate = Convert.ToDouble(DBNull.Value.Equals(dr["GstRate"]) ? 0 : dr["GstRate"]),
                        PrintInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["PrintInvoice"]) ? false : dr["PrintInvoice"]),
                        EmailInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailInvoice"]) ? false : dr["EmailInvoice"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        NoCustomerStatement = Convert.ToBoolean(DBNull.Value.Equals(dr["NoCustomerStatement"]) ? false : dr["NoCustomerStatement"]),
                        LocationName = Convert.ToString(dr["LocationName"]),
                        Salesperson = Convert.ToString(dr["Salesperson"]),
                        RouteName = Convert.ToString(dr["RouteName"]),
                        ConsultantName = Convert.ToString(dr["ConsultantName"]),
                        OwnerName = Convert.ToString(dr["OwnerName"]),
                        Customer = Convert.ToString(dr["Customer"]),
                        Elevs = Convert.ToInt32(DBNull.Value.Equals(dr["Elevs"]) ? 0 : dr["Elevs"]),
                        BusinessTypeID = Convert.ToInt32(DBNull.Value.Equals(dr["BusinessTypeID"]) ? 0 : dr["BusinessTypeID"]),
                        sTaxType = Convert.ToInt16(DBNull.Value.Equals(dr["sTaxType"]) ? 0 : dr["sTaxType"]),
                    }
                    );
            }
            _ds.lstTable1 = _lstTable1;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetLocationByIDTable2()
                    {
                        contactid = Convert.ToInt32(DBNull.Value.Equals(dr["contactid"]) ? 0 : dr["contactid"]),
                        name = Convert.ToString(dr["name"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Cell = Convert.ToString(dr["Cell"]),
                        Email = Convert.ToString(dr["Email"]),
                        Title = Convert.ToString(dr["Title"]),
                        EmailTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailTicket"]) ? false : dr["EmailTicket"]),
                        EmailRecInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecInvoice"]) ? false : dr["EmailRecInvoice"]),
                        ShutdownAlert = Convert.ToBoolean(DBNull.Value.Equals(dr["ShutdownAlert"]) ? false : dr["ShutdownAlert"]),
                        EmailRecTestProp = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecTestProp"]) ? false : dr["EmailRecTestProp"]),
                    }
                    );
            }
            _ds.lstTable2= _lstTable2;

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                _lstTable3.Add(
                    new GetLocationByIDTable3()
                    {
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                    }
                    );
            }
            _ds.lstTable3 = _lstTable3;

            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                _lstTable4.Add(
                    new GetLocationByIDTable4()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }
            _ds.lstTable4 = _lstTable4;

            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                _lstTable5.Add(
                    new GetLocationByIDTable5()
                    {
                        TicketID = Convert.ToInt32(DBNull.Value.Equals(dr["TicketID"]) ? 0 : dr["TicketID"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    }
                    );
            }
            _ds.lstTable5 = _lstTable5;

            return _ds;
        }

        public DataSet getAllLocation(User objPropUser)
        {
            return objDL_User.getAllLocation(objPropUser);
        }
        public DataSet GetGCandHowerLocID(User objPropUser)
        {
            return objDL_User.getGCandHowerLocID(objPropUser);
        }

        //API
        public ListGetGCandHowerLocID GetGCandHowerLocID(GetGCandHowerLocIDParam _GetGCandHowerLocID, string ConnectionString)
        {
            DataSet ds = objDL_User.getGCandHowerLocID(_GetGCandHowerLocID, ConnectionString);

            ListGetGCandHowerLocID _ds = new ListGetGCandHowerLocID();
            List<GetGCandHowerLocIDTable1> _lstTable1 = new List<GetGCandHowerLocIDTable1>();
            List<GetGCandHowerLocIDTable2> _lstTable2 = new List<GetGCandHowerLocIDTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetGCandHowerLocIDTable1()
                    {
                        RolName = Convert.ToString(dr["RolName"]),
                        city = Convert.ToString(dr["city"]),
                        state = Convert.ToString(dr["state"]),
                        zip = Convert.ToString(dr["zip"]),
                        phone = Convert.ToString(dr["phone"]),
                        fax = Convert.ToString(dr["fax"]),
                        contact = Convert.ToString(dr["contact"]),
                        email = Convert.ToString(dr["email"]),
                        country = Convert.ToString(dr["country"]),
                        cellular = Convert.ToString(dr["cellular"]),
                        rolRemarks = Convert.ToString(dr["rolRemarks"]),
                        LocContactType = Convert.ToInt32(DBNull.Value.Equals(dr["LocContactType"]) ? 0 : dr["LocContactType"]),
                        RolID = Convert.ToInt32(DBNull.Value.Equals(dr["RolID"]) ? 0 : dr["RolID"]),
                        Address = Convert.ToString(dr["Address"]),
                    }
                    );
            }
            _ds.lstTable1 = _lstTable1;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetGCandHowerLocIDTable2()
                    {
                        RolName = Convert.ToString(dr["RolName"]),
                        city = Convert.ToString(dr["city"]),
                        state = Convert.ToString(dr["state"]),
                        zip = Convert.ToString(dr["zip"]),
                        phone = Convert.ToString(dr["phone"]),
                        fax = Convert.ToString(dr["fax"]),
                        contact = Convert.ToString(dr["contact"]),
                        email = Convert.ToString(dr["email"]),
                        country = Convert.ToString(dr["country"]),
                        cellular = Convert.ToString(dr["cellular"]),
                        rolRemarks = Convert.ToString(dr["rolRemarks"]),
                        LocContactType = Convert.ToInt32(DBNull.Value.Equals(dr["LocContactType"]) ? 0 : dr["LocContactType"]),
                        RolID = Convert.ToInt32(DBNull.Value.Equals(dr["RolID"]) ? 0 : dr["RolID"]),
                        Address = Convert.ToString(dr["Address"]),
                    }
                    );
            }
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public DataSet getLocationByIDReport(User objPropUser)
        {
            return objDL_User.getLocationByIDReport(objPropUser);
        }

        public DataSet getElevUnit(User objPropUser)
        {
            return objDL_User.getElevUnit(objPropUser);
        }

        public DataSet getElev(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getElev(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetElevViewModel> getElev(GetElevParam _GetElev, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds = objDL_User.getElev(_GetElev, ConnectionString, IsSalesAsigned);
            List<GetElevViewModel> _lstGetElev = new List<GetElevViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetElev.Add(
                    new GetElevViewModel()
                    {
                        state = Convert.ToString(dr["state"]),
                        cat = Convert.ToString(dr["cat"]),
                        category = Convert.ToString(dr["category"]),
                        Classification = Convert.ToString(dr["Classification"]),
                        manuf = Convert.ToString(dr["manuf"]),
                        price = Convert.ToDouble(DBNull.Value.Equals(dr["price"]) ? 0 : dr["price"]),
                        last = Convert.ToDateTime(DBNull.Value.Equals(dr["last"]) ? null : dr["last"]),
                        since = Convert.ToDateTime(DBNull.Value.Equals(dr["since"]) ? null : dr["since"]),
                        Install = Convert.ToDateTime(DBNull.Value.Equals(dr["Install"]) ? null : dr["Install"]),
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        unit = Convert.ToString(dr["unit"]),
                        type = Convert.ToString(dr["type"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        status = Convert.ToInt16(DBNull.Value.Equals(dr["status"]) ? 0 : dr["status"]),
                        shut_down = Convert.ToString(dr["shut_down"]),
                        ShutdownReason = Convert.ToString(dr["ShutdownReason"]),
                        building = Convert.ToString(dr["building"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        name = Convert.ToString(dr["name"]),
                        locid = Convert.ToString(dr["locid"]),
                        tag = Convert.ToString(dr["tag"]),
                        address = Convert.ToString(dr["address"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        unitid = Convert.ToInt32(DBNull.Value.Equals(dr["unitid"]) ? 0 : dr["unitid"]),
                    }
                    );
            }

            return _lstGetElev;
        }

        public DataSet getLeadEquip(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.getLeadEquip(objPropUser, IsSalesAsigned);
        }


        public DataSet getCategory(User objPropUser)
        {
            return objDL_User.getCategory(objPropUser);
        }

        //API
        public List<GetCategoryViewModel> getCategory(GetCategoryParam _GetCategory, string ConnectionString)
        {
            DataSet ds = objDL_User.getCategory(_GetCategory, ConnectionString);
            List<GetCategoryViewModel> _lstGetCategory = new List<GetCategoryViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCategory.Add(
                    new GetCategoryViewModel()
                    {
                        type = Convert.ToString(dr["type"]),
                        Status = Convert.ToBoolean(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                    }
                    );
            }

            return _lstGetCategory;
        }

        public List<CategoryResponse> GetCategoryList(User objPropUser)
        {
            List<CategoryResponse> lst = new List<CategoryResponse>();
            DataSet ds = objDL_User.getCategory(objPropUser);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(new CategoryResponse()
                {
                    Status = Convert.ToString(dr["Status"].ToString()),
                    type = Convert.ToString(dr["type"].ToString()),
                });
            }
            return lst;
        }
        public String getDefaultCategory(User objPropUser)
        {
            return objDL_User.getDefaultCategory(objPropUser);
        }

        //API
        public String getDefaultCategory(GetDefaultCategoryParam _GetDefaultCategory, string ConnectionString)
        {
            return objDL_User.getDefaultCategory(_GetDefaultCategory, ConnectionString);
        }

        public String getDefaultCategoryAPI(User objPropUser)
        {
            return objDL_User.getDefaultCategoryAPI(objPropUser);
        }


        public DataSet gettrial(User objPropUser)
        {
            return objDL_User.gettrial(objPropUser);
        }

        public DataSet gettrialUser(User objPropUser)
        {
            return objDL_User.gettrialUser(objPropUser);
        }

        public DataSet getLicenseInfoUser(User objPropUser)
        {
            return objDL_User.getLicenseInfoUser(objPropUser);
        }

        public void UpdateTrial(User objPropUser)
        {
            objDL_User.UpdateTrial(objPropUser);
        }

        public void UpdateReg(User objPropUser)
        {
            objDL_User.UpdateReg(objPropUser);
        }

        public void UpdateRegUser(User objPropUser)
        {
            objDL_User.UpdateRegUser(objPropUser);
        }

        public void UpdateSupervisorUser(User objPropUser)
        {
            objDL_User.UpdateSupervisorUser(objPropUser);
        }

        public void DeleteUser(User objPropUser)
        {
            objDL_User.DeleteUser(objPropUser);
        }

        public void DeleteCustomer(User objPropUser)
        {
            objDL_User.DeleteCustomer(objPropUser);
        }

        //API
        public void DeleteCustomer(DeleteCustomerParam _DeleteCustomer, string ConnectionString)
        {
            objDL_User.DeleteCustomer(_DeleteCustomer, ConnectionString);
        }

        public void DeleteCustomerBySageID(User objPropUser)
        {
            objDL_User.DeleteCustomerBySageID(objPropUser);
        }

        public void DeleteLocationBySageID(User objPropUser)
        {
            objDL_User.DeleteLocationBySageID(objPropUser);
        }

        public void DeleteCustomerByListID(User objPropUser)
        {
            objDL_User.DeleteCustomerByListID(objPropUser);
        }

        public void DeleteEquipment(User objPropUser)
        {
            objDL_User.DeleteEquipment(objPropUser);
        }

        //API
        public void DeleteEquipment(DeleteEquipmentParam _DeleteEquipment, string ConnectionString)
        {
            objDL_User.DeleteEquipment(_DeleteEquipment, ConnectionString);
        }

        public void DeleteLeadEquipment(User objPropUser)
        {
            objDL_User.DeleteLeadEquipment(objPropUser);
        }

        public void DeleteLocation(User objPropUser)
        {
            objDL_User.DeleteLocation(objPropUser);
        }

        //API
        public void DeleteLocation(DeleteLocationParam _DeleteLocation, string ConnectionString)
        {
            objDL_User.DeleteLocation(_DeleteLocation, ConnectionString);
        }

        public void DeleteLocationByListID(User objPropUser)
        {
            objDL_User.DeleteLocationByListID(objPropUser);
        }

        public void DeleteEmployeeByListID(User objPropUser)
        {
            objDL_User.DeleteEmployeeByListID(objPropUser);
        }

        public void UpdateRolCoordinates(User objPropUser)
        {
            objDL_User.UpdateRolCoordinates(objPropUser);
        }

        public DataSet getWarehouse(User objPropUser)
        {
            return objDL_User.getWarehouse(objPropUser);
        }

        public string getUserEmail(User objPropUser)
        {
            return objDL_User.getUserEmail(objPropUser);
        }

        //API
        public string getUserEmail(GetUserEmailParam _GetUserEmail, string ConnectionString)
        {
            return objDL_User.getUserEmail(_GetUserEmail, ConnectionString);
        }

        public string getUserEmailByUserId(User objPropUser)
        {
            return objDL_User.getUserEmailByUserId(objPropUser);
        }
        public string getUserPager(User objPropUser)
        {
            return objDL_User.getUserPager(objPropUser);
        }

        public string getUserDeviceID(User objPropUser)
        {
            return objDL_User.getUserDeviceID(objPropUser);
        }

        public void UpdateDefaultWorkerLocation(User objPropUser)
        {
            objDL_User.UpdateDefaultWorkerLocation(objPropUser);
        }

        public void UpdateLocationAddress(User objPropUser)
        {
            objDL_User.UpdateLocationAddress(objPropUser);
        }

        public void UserRegistrationTransfer(User objPropUser)
        {
            objDL_User.UserRegistrationTransfer(objPropUser);
        }

        public int GetUserSyncStatus(User objPropUser)
        {
            return objDL_User.GetUserSyncStatus(objPropUser);
        }

        public DataSet GetSyncItems(User objPropUser)
        {
            return objDL_User.GetSyncItems(objPropUser);
        }

        public void AddEquipmentTemplate(User objPropUser)
        {
            objDL_User.AddEquipmentTemplate(objPropUser);
        }

        public void AddCustomTemplate(User objPropUser)
        {
            objDL_User.AddCustomTemplate(objPropUser);
        }

        public void AddCustomTemplateForLeadEquip(User objPropUser)
        {
            objDL_User.AddCustomTemplateForLeadEquip(objPropUser);
        }


        //public DataSet getSalesPerson(User objPropUser, Int32 IsSalesAsigned = 0)
        //{
        //    return objDL_User.getSalesPerson(objPropUser, IsSalesAsigned);
        //}

        public DataSet getTaskUsers(User objPropUser)
        {
            return objDL_User.getTaskUsers(objPropUser);
        }

        public void UpdateAnnualAmount(User objPropUser)
        {
            objDL_User.UpdateAnnualAmount(objPropUser);
        }

        public void UpdateCustomerUser(User objPropUser)
        {
            objDL_User.UpdateCustomerUser(objPropUser);
        }

        public DataSet getTimesheetEmp(User objPropUser, int Etimesheet)
        {
            return objDL_User.getTimesheetEmp(objPropUser, Etimesheet);
        }
        //api
        public List<eTimesheetViewModel> getTimesheetEmp(getTimesheetParam objPropUser, int Etimesheet, string ConnectionString)
        {
            DataSet ds = objDL_User.getTimesheetEmp(objPropUser, Etimesheet, ConnectionString);


            List<eTimesheetViewModel> _userViewModel = new List<eTimesheetViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new eTimesheetViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = (dr["Name"]).ToString(),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = (dr["Company"].ToString()),
                        fDesc = (dr["fDesc"]).ToString(),
                        reg = Convert.ToInt32(DBNull.Value.Equals(dr["reg"]) ? 0 : dr["reg"]),

                        OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),

                        DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),

                        TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),

                        NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                        Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
                        MileageRate = Convert.ToDouble(DBNull.Value.Equals(dr["MileageRate"]) ? 0 : dr["MileageRate"]),
                        Mileage = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage"]) ? 0 : dr["Mileage"]),
                        extra = Convert.ToDouble(DBNull.Value.Equals(dr["extra"]) ? 0 : dr["extra"]),
                        Toll = Convert.ToDouble(DBNull.Value.Equals(dr["Toll"]) ? 0 : dr["Toll"]),
                        OtherE = Convert.ToDouble(DBNull.Value.Equals(dr["OtherE"]) ? 0 : dr["OtherE"]),
                        pay = Convert.ToInt32(DBNull.Value.Equals(dr["pay"]) ? 0 : dr["pay"]),
                        holiday = Convert.ToInt32(DBNull.Value.Equals(dr["holiday"]) ? 0 : dr["holiday"]),
                        vacation = Convert.ToInt32(DBNull.Value.Equals(dr["vacation"]) ? 0 : dr["vacation"]),
                        sicktime = Convert.ToInt32(DBNull.Value.Equals(dr["sicktime"]) ? 0 : dr["sicktime"]),
                        reimb = Convert.ToInt32(DBNull.Value.Equals(dr["reimb"]) ? 0 : dr["reimb"]),
                        bonus = Convert.ToInt32(DBNull.Value.Equals(dr["bonus"]) ? 0 : dr["bonus"]),
                        paymethod = Convert.ToInt32(DBNull.Value.Equals(dr["paymethod"]) ? 0 : dr["paymethod"]),
                        pmethod = Convert.ToInt32(DBNull.Value.Equals(dr["pmethod"]) ? 0 : dr["pmethod"]),
                        userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
                        usertype = (dr["usertype"]).ToString(),
                        total = Convert.ToDouble(DBNull.Value.Equals(dr["total"]) ? 0 : dr["total"]),
                        phour = Convert.ToDouble(DBNull.Value.Equals(dr["phour"]) ? 0 : dr["phour"]),
                        salary = Convert.ToDouble(DBNull.Value.Equals(dr["salary"]) ? 0 : dr["salary"]),
                        HourlyRate = Convert.ToDouble(DBNull.Value.Equals(dr["HourlyRate"]) ? 0 : dr["HourlyRate"]),
                        Customtick1 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick1"]) ? 0 : dr["Customtick1"]),
                        dollaramount = Convert.ToInt32(DBNull.Value.Equals(dr["dollaramount"]) ? 0 : dr["dollaramount"]),
                        Reg1 = Convert.ToInt32(DBNull.Value.Equals(dr["Reg1"]) ? 0 : dr["Reg1"]),
                        OT1 = Convert.ToInt32(DBNull.Value.Equals(dr["OT1"]) ? 0 : dr["OT1"]),
                        DT1 = Convert.ToInt32(DBNull.Value.Equals(dr["DT1"]) ? 0 : dr["DT1"]),
                        TT1 = Convert.ToInt32(DBNull.Value.Equals(dr["TT1"]) ? 0 : dr["TT1"]),
                        NT1 = Convert.ToInt32(DBNull.Value.Equals(dr["NT1"]) ? 0 : dr["NT1"]),
                        Zone1 = Convert.ToInt32(DBNull.Value.Equals(dr["Zone1"]) ? 0 : dr["Zone1"]),
                        Mileage1 = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage1"]) ? 0 : dr["Mileage1"]),
                        Extra1 = Convert.ToInt32(DBNull.Value.Equals(dr["Extra1"]) ? 0 : dr["Extra1"]),
                        Toll1 = Convert.ToInt32(DBNull.Value.Equals(dr["Toll1"]) ? 0 : dr["Toll1"]),
                        HourRate1 = Convert.ToInt32(DBNull.Value.Equals(dr["HourRate1"]) ? 0 : dr["HourRate1"]),

                        signature = (dr["signature"]).ToString(),
                        ref1 = Convert.ToInt32(DBNull.Value.Equals(dr["ref1"]) ? 0 : dr["ref1"]),
                        custom = Convert.ToInt32(DBNull.Value.Equals(dr["custom"]) ? 0 : dr["custom"]),
                        countDetail = Convert.ToInt32(DBNull.Value.Equals(dr["countDetail"]) ? 0 : dr["countDetail"]),


                    }
                    );
            }



            return _userViewModel;
        }

        public DataSet getSavedTimesheetEmp(User objPropUser)
        {
            return objDL_User.getSavedTimesheetEmp(objPropUser);
        }

        //api
        public List<eTimesheetViewModel> getSavedTimesheetEmp(getTimesheetParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getSavedTimesheetEmp(objPropUser, ConnectionString);

            List<eTimesheetViewModel> _userViewModel = new List<eTimesheetViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new eTimesheetViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = dr["Name"].ToString(),
                        fDesc = dr["fDesc"].ToString(),
                        pay = Convert.ToInt32(DBNull.Value.Equals(dr["pay"]) ? 0 : dr["pay"]),
                        paymethod = Convert.ToInt32(DBNull.Value.Equals(dr["paymethod"]) ? 0 : dr["paymethod"]),
                        pmethod = Convert.ToInt32(DBNull.Value.Equals(dr["pmethod"]) ? 0 : dr["pmethod"]),
                        reg = Convert.ToInt32(DBNull.Value.Equals(dr["reg"]) ? 0 : dr["reg"]),
                        OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                        DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                        TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
                        NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                        holiday = Convert.ToInt32(DBNull.Value.Equals(dr["holiday"]) ? 0 : dr["holiday"]),
                        vacation = Convert.ToInt32(DBNull.Value.Equals(dr["vacation"]) ? 0 : dr["vacation"]),
                        sicktime = Convert.ToInt32(DBNull.Value.Equals(dr["sicktime"]) ? 0 : dr["sicktime"]),
                        Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
                        reimb = Convert.ToInt32(DBNull.Value.Equals(dr["reimb"]) ? 0 : dr["reimb"]),
                        MileageRate = Convert.ToDouble(DBNull.Value.Equals(dr["MileageRate"]) ? 0 : dr["MileageRate"]),
                        MileRate = Convert.ToInt32(DBNull.Value.Equals(dr["MileRate"]) ? 0 : dr["MileRate"]),
                        Mileage = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage"]) ? 0 : dr["Mileage"]),
                        bonus = Convert.ToInt32(DBNull.Value.Equals(dr["bonus"]) ? 0 : dr["bonus"]),
                        extra = Convert.ToDouble(DBNull.Value.Equals(dr["extra"]) ? 0 : dr["extra"]),
                        Toll = Convert.ToDouble(DBNull.Value.Equals(dr["Toll"]) ? 0 : dr["Toll"]),
                        OtherE = Convert.ToDouble(DBNull.Value.Equals(dr["OtherE"]) ? 0 : dr["OtherE"]),
                        total = Convert.ToDouble(DBNull.Value.Equals(dr["total"]) ? 0 : dr["total"]),
                        phour = Convert.ToDouble(DBNull.Value.Equals(dr["phour"]) ? 0 : dr["phour"]),
                        salary = Convert.ToDouble(DBNull.Value.Equals(dr["salary"]) ? 0 : dr["salary"]),
                        HourlyRate = Convert.ToDouble(DBNull.Value.Equals(dr["HourlyRate"]) ? 0 : dr["HourlyRate"]),
                        Processed = Convert.ToInt32(DBNull.Value.Equals(dr["Processed"]) ? 0 : dr["Processed"]),
                        userid = Convert.ToInt32(DBNull.Value.Equals(dr["userid"]) ? 0 : dr["userid"]),
                        usertype = dr["usertype"].ToString(),
                        dollaramount = Convert.ToInt32(DBNull.Value.Equals(dr["dollaramount"]) ? 0 : dr["dollaramount"]),
                        Reg1 = Convert.ToInt32(DBNull.Value.Equals(dr["Reg1"]) ? 0 : dr["Reg1"]),
                        OT1 = Convert.ToInt32(DBNull.Value.Equals(dr["OT1"]) ? 0 : dr["OT1"]),
                        DT1 = Convert.ToInt32(DBNull.Value.Equals(dr["DT1"]) ? 0 : dr["DT1"]),
                        TT1 = Convert.ToInt32(DBNull.Value.Equals(dr["TT1"]) ? 0 : dr["TT1"]),
                        NT1 = Convert.ToInt32(DBNull.Value.Equals(dr["NT1"]) ? 0 : dr["NT1"]),
                        Zone1 = Convert.ToInt32(DBNull.Value.Equals(dr["Zone1"]) ? 0 : dr["Zone1"]),
                        Mileage1 = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage1"]) ? 0 : dr["Mileage1"]),
                        Extra1 = Convert.ToInt32(DBNull.Value.Equals(dr["Extra1"]) ? 0 : dr["Extra1"]),
                        Misc1 = Convert.ToInt32(DBNull.Value.Equals(dr["Misc1"]) ? 0 : dr["Misc1"]),
                        Toll1 = Convert.ToInt32(DBNull.Value.Equals(dr["Toll1"]) ? 0 : dr["Toll1"]),
                        HourRate1 = Convert.ToInt32(DBNull.Value.Equals(dr["HourRate1"]) ? 0 : dr["HourRate1"]),
                        ref1 = Convert.ToInt32(DBNull.Value.Equals(dr["ref1"]) ? 0 : dr["ref1"]),
                        signature = dr["signature"].ToString(),
                        custom = Convert.ToInt32(DBNull.Value.Equals(dr["custom"]) ? 0 : dr["custom"]),
                        Customtick1 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick1"]) ? 0 : dr["Customtick1"]),

                    }
                    );
            }
            return _userViewModel;
        }

        public DataSet getSavedTimesheet(User objPropUser)
        {
            return objDL_User.getSavedTimesheet(objPropUser);
        }

        //api
        public List<UserViewModel> getSavedTimesheet(getTimesheetParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getSavedTimesheet(objPropUser, ConnectionString);

            List<UserViewModel> _userViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new UserViewModel()
                    {
                        Processed = Convert.ToInt32(DBNull.Value.Equals(dr["processed"]) ? 0 : dr["processed"])

                    }
                    );
            }

            return _userViewModel;

        }

        public DataSet GetTimesheetTicketsByEmp(User objPropUser, int Etimesheet)
        {
            return objDL_User.GetTimesheetTicketsByEmp(objPropUser, Etimesheet);
        }

        //api
        public List<eTimesheetViewModel> GetTimesheetTicketsByEmp(getTimesheetParam objPropUser, int Etimesheet, string ConnectionString)
        {
            DataSet ds = objDL_User.GetTimesheetTicketsByEmp(objPropUser, Etimesheet, ConnectionString);
            List<eTimesheetViewModel> _userViewModel = new List<eTimesheetViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new eTimesheetViewModel()
                    {
                        Date = Convert.ToDateTime(DBNull.Value.Equals(dr["Date"]) ? null : dr["Date"]),
                        TicketID = Convert.ToInt32(DBNull.Value.Equals(dr["TicketID"]) ? 0 : dr["TicketID"]),
                        reg = Convert.ToInt32(DBNull.Value.Equals(dr["reg"]) ? 0 : dr["reg"]),
                        OT = Convert.ToDouble(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                        DT = Convert.ToDouble(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                        TT = Convert.ToDouble(DBNull.Value.Equals(dr["TT"]) ? 0 : dr["TT"]),
                        NT = Convert.ToDouble(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                        Zone = Convert.ToInt32(DBNull.Value.Equals(dr["Zone"]) ? 0 : dr["Zone"]),
                        Mileage = Convert.ToInt32(DBNull.Value.Equals(dr["Mileage"]) ? 0 : dr["Mileage"]),
                        Toll = Convert.ToDouble(DBNull.Value.Equals(dr["Toll"]) ? 0 : dr["Toll"]),
                        OtherE = Convert.ToDouble(DBNull.Value.Equals(dr["OtherE"]) ? 0 : dr["OtherE"]),
                        extra = Convert.ToDouble(DBNull.Value.Equals(dr["extra"]) ? 0 : dr["extra"]),
                        HourlyRate = Convert.ToDouble(DBNull.Value.Equals(dr["HourlyRate"]) ? 0 : dr["HourlyRate"]),
                        custom = Convert.ToInt32(DBNull.Value.Equals(dr["custom"]) ? 0 : dr["custom"]),
                        Customtick2 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick2"]) ? 0 : dr["Customtick2"]),
                        Customtick1 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick1"]) ? 0 : dr["Customtick1"]),
                        Customtick3 = Convert.ToInt32(DBNull.Value.Equals(dr["Customtick3"]) ? 0 : dr["Customtick3"]),
                    }
                    );
            }
            return _userViewModel;
        }

        public void AddTimesheet(User objPropUser)
        {
            objDL_User.AddTimesheet(objPropUser);
        }

        //api
        public void AddTimesheet(AddTimesheetParam objPropUser, string ConnectionString)
        {
            objDL_User.AddTimesheet(objPropUser, ConnectionString);
        }

        public void ProcessTimesheet(User objPropUser)
        {
            objDL_User.ProcessTimesheet(objPropUser);
        }

        public DataSet getScreens(User objPropUser)
        {
            return objDL_User.getScreens(objPropUser);
        }

        public DataSet getScreensByUser(User objPropUser)
        {
            return objDL_User.getScreensByUser(objPropUser);
        }

        //API
        public List<GetScreensByUserViewModel> getScreensByUser(GetScreensByUserParam _GetScreensByUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getScreensByUser(_GetScreensByUser, ConnectionString);

            List<GetScreensByUserViewModel> _lstGetScreensByUser = new List<GetScreensByUserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetScreensByUser.Add(
                    new GetScreensByUserViewModel()
                    {
                        access = Convert.ToBoolean(DBNull.Value.Equals(dr["access"]) ? false : dr["access"]),
                        edit = Convert.ToBoolean(DBNull.Value.Equals(dr["edit"]) ? false : dr["edit"]),
                        VIEW = Convert.ToBoolean(DBNull.Value.Equals(dr["VIEW"]) ? false : dr["VIEW"]),
                        add = Convert.ToBoolean(DBNull.Value.Equals(dr["add"]) ? false : dr["add"]),
                        DELETE = Convert.ToBoolean(DBNull.Value.Equals(dr["DELETE"]) ? false : dr["DELETE"]),
                    }
                    );
            }
            return _lstGetScreensByUser;
        }

        public void UpdateUserPermission(User objPropUser)
        {
            objDL_User.UpdateUserPermission(objPropUser);
        }

        public DataSet AddSageLocation(User objPropUser)
        {
            return objDL_User.AddSageLocation(objPropUser);
        }

        public DataSet getGetSageExportTickets(User objPropUser)
        {
            return objDL_User.getGetSageExportTickets(objPropUser);
        }

        //api
        public List<SageExportTickets> getGetSageExportTickets(getTimesheetParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getGetSageExportTickets(objPropUser, ConnectionString);

            List<SageExportTickets> _userViewModel = new List<SageExportTickets>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new SageExportTickets()
                    {
                        DC = dr["DC"].ToString(),
                        SageJob = dr["SageJob"].ToString(),
                        extra = dr["extra"].ToString(),
                        costcode = Convert.ToDouble(DBNull.Value.Equals(dr["costcode"]) ? 0 : dr["costcode"]),
                        Category = dr["Category"].ToString(),
                        trantype = Convert.ToInt32(DBNull.Value.Equals(dr["trantype"]) ? 0 : dr["trantype"]),
                        trandate = dr["trandate"].ToString(),
                        accdate = dr["accdate"].ToString(),
                        description = dr["description"].ToString(),
                        units = Convert.ToDouble(DBNull.Value.Equals(dr["units"]) ? 0 : dr["units"]),
                        unitcost = Convert.ToDouble(DBNull.Value.Equals(dr["unitcost"]) ? 0 : dr["unitcost"]),
                        amount = Convert.ToDouble(DBNull.Value.Equals(dr["amount"]) ? 0 : dr["amount"]),
                        debitacc = dr["debitacc"].ToString(),
                        creditacc = dr["creditacc"].ToString(),
                        ticket = Convert.ToInt32(DBNull.Value.Equals(dr["ticket"]) ? 0 : dr["ticket"]),
                    }
                    );
            }
            return _userViewModel;
        }

        public void UpdatePeriodClosedDate(User objPropUser)
        {
            objDL_User.UpdatePeriodClosedDate(objPropUser);
        }
        public DataSet GetUserAddress(User objPropUser)
        {
            return objDL_User.GetUserAddress(objPropUser);
        }
        public DataSet GetBillCodeSearch(User objPropUser)
        {
            return objDL_User.GetBillCodeSearch(objPropUser);
        }

      

         public DataSet updateServiceTypeByjobType(string Sessionconfig , int jobID, string ServiceType, int ExpenseGLValue , int InterestGLValue , int BillingValue , int LaborWageValue)
        {
            return objDL_User.updateServiceTypeByjobType( Sessionconfig,  jobID,  ServiceType,  ExpenseGLValue,  InterestGLValue,  BillingValue,  LaborWageValue);
        }

        public DataSet GetServiceTypeByType(User objPropUser)
        {
            return objDL_User.GetServiceTypeByType(objPropUser);
        }

        //API
        public List<GetServiceTypeByTypeViewModel> GetServiceTypeByType(GetServiceTypeByTypeParam _GetServiceTypeByType, string ConnectionString)
        {
            DataSet ds = objDL_User.GetServiceTypeByType(_GetServiceTypeByType, ConnectionString);

            List<GetServiceTypeByTypeViewModel> _lstGetServiceTypeByType = new List<GetServiceTypeByTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetServiceTypeByType.Add(
                    new GetServiceTypeByTypeViewModel()
                    {
                        type = Convert.ToString(dr["type"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        remarks = Convert.ToString(dr["remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        InvID = Convert.ToInt32(DBNull.Value.Equals(dr["InvID"]) ? 0 : dr["InvID"]),
                        Sacct = Convert.ToString(dr["Sacct"]),
                        GLAcct = Convert.ToString(dr["GLAcct"]),
                    }
                    );
            }
            return _lstGetServiceTypeByType;
        }

        public DataSet getWage(User objPropUser)
        {
            return objDL_User.getWage(objPropUser);
        }
        public DataSet getSTax(User objPropUser)
        {
            return objDL_User.getSTax(objPropUser);
        }
        public List<STaxViewModel> getSTax(getSTaxParam _getSTaxParam, string ConnectionString)
        {
            DataSet ds = objDL_User.getSTax(_getSTaxParam, ConnectionString);

            List<STaxViewModel> _lstSTaxViewModel = new List<STaxViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstSTaxViewModel.Add(
                    new STaxViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Rate = Convert.ToDecimal(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                        State = Convert.ToString(dr["State"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        GL = Convert.ToInt32(DBNull.Value.Equals(dr["GL"]) ? 0 : dr["GL"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        UType = Convert.ToInt32(DBNull.Value.Equals(dr["UType"]) ? 0 : dr["UType"]),
                        PSTReg = Convert.ToString(dr["PSTReg"]),
                        QBStaxID = Convert.ToString(dr["QBStaxID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        IsTaxable = Convert.ToBoolean(DBNull.Value.Equals(dr["IsTaxable"]) ? false : dr["IsTaxable"]),
                        StaxName = Convert.ToString(dr["StaxName"]),
                        AcctDesc = Convert.ToString(dr["AcctDesc"]),
                    }
                    );
            }


            return _lstSTaxViewModel;
        }
        public DataSet getSalesTax2(User objPropUser)
        {
            return objDL_User.getSalesTax2(objPropUser);
        }

        //API
        public List<getSalesTax2ViewModel> getSalesTax2(getSalesTax2Param _getSalesTax2, string ConnectionString)
        {
            DataSet ds = objDL_User.getSalesTax2(_getSalesTax2, ConnectionString);

            List<getSalesTax2ViewModel> _lstgetSalesTax2= new List<getSalesTax2ViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstgetSalesTax2.Add(
                    new getSalesTax2ViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Rate = Convert.ToDouble(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                        State = Convert.ToString(dr["State"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        GL = Convert.ToInt32(DBNull.Value.Equals(dr["GL"]) ? 0 : dr["GL"]),
                        Type = Convert.ToInt16(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        UType = Convert.ToInt16(DBNull.Value.Equals(dr["UType"]) ? 0 : dr["UType"]),
                        PSTReg = Convert.ToString(dr["PSTReg"]),
                        QBStaxID = Convert.ToString(dr["QBStaxID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        IsTaxable = Convert.ToBoolean(DBNull.Value.Equals(dr["IsTaxable"]) ? false : dr["IsTaxable"]),
                        AcctDesc = Convert.ToString(dr["AcctDesc"]),
                    }
                    );
            }

            return _lstgetSalesTax2;
        }
        public DataSet getUseTax(User objPropUser)
        {
            return objDL_User.getUseTax(objPropUser);
        }
        public List<STaxViewModel> getUseTax(getUseTaxParam _getUseTaxParam, string ConnectionString)
        {
            DataSet ds = objDL_User.getUseTax(_getUseTaxParam, ConnectionString);

            List<STaxViewModel> _lstUseTaxViewModel = new List<STaxViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstUseTaxViewModel.Add(
                    new STaxViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Rate = Convert.ToDecimal(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                        State = Convert.ToString(dr["State"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        GL = Convert.ToInt32(DBNull.Value.Equals(dr["GL"]) ? 0 : dr["GL"]),
                        Type = Convert.ToInt32(DBNull.Value.Equals(dr["Type"]) ? 0 : dr["Type"]),
                        UType = Convert.ToInt32(DBNull.Value.Equals(dr["UType"]) ? 0 : dr["UType"]),
                        PSTReg = Convert.ToString(dr["PSTReg"]),
                        QBStaxID = Convert.ToString(dr["QBStaxID"]),
                        LastUpdateDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastUpdateDate"]) ? null : dr["LastUpdateDate"]),
                        IsTaxable = Convert.ToBoolean(DBNull.Value.Equals(dr["IsTaxable"]) ? false : dr["IsTaxable"]),
                    }
                    );
            }


            return _lstUseTaxViewModel;
        }
        public DataSet getZone(User objPropUser)
        {
            return objDL_User.getZone(objPropUser);
        }

        //API
        public List<GetZoneViewModel> getZone(GetZoneParam _GetZone, string ConnectionString)
        {
            DataSet ds = objDL_User.getZone(_GetZone, ConnectionString);

            List<GetZoneViewModel> _lstGetZone = new List<GetZoneViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetZone.Add(
                    new GetZoneViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Bonus = Convert.ToDouble(DBNull.Value.Equals(dr["Bonus"]) ? 0 : dr["Bonus"]),
                        Color = Convert.ToInt16(DBNull.Value.Equals(dr["Color"]) ? 0 : dr["Color"]),
                        Remarks = Convert.ToString(dr["Remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        IDistance = Convert.ToDouble(DBNull.Value.Equals(dr["IDistance"]) ? 0 : dr["IDistance"]),
                        ODistance = Convert.ToDouble(DBNull.Value.Equals(dr["ODistance"]) ? 0 : dr["ODistance"]),
                        Price1 = Convert.ToDouble(DBNull.Value.Equals(dr["Price1"]) ? 0 : dr["Price1"]),
                        Price2 = Convert.ToDouble(DBNull.Value.Equals(dr["Price2"]) ? 0 : dr["Price2"]),
                        Price3 = Convert.ToDouble(DBNull.Value.Equals(dr["Price3"]) ? 0 : dr["Price3"]),
                        Price4 = Convert.ToDouble(DBNull.Value.Equals(dr["Price4"]) ? 0 : dr["Price4"]),
                        Price5 = Convert.ToDouble(DBNull.Value.Equals(dr["Price5"]) ? 0 : dr["Price5"]),
                        Surcharge = Convert.ToDouble(DBNull.Value.Equals(dr["Surcharge"]) ? 0 : dr["Surcharge"]),
                        Tax = Convert.ToInt16(DBNull.Value.Equals(dr["Tax"]) ? 0 : dr["Tax"]),
                    }
                    );
            }
            return _lstGetZone;
        }
        public DataSet GetWageByID(Wage _objWage)
        {
            return objDL_User.GetWageByID(_objWage);
        }
        public void DeleteWageByID(Wage _objWage)
        {
            objDL_User.DeleteWageByID(_objWage);
        }
        public DataSet GetAllWage(Wage _objWage)
        {
            return objDL_User.GetAllWage(_objWage);
        }

        public DataSet GetAllWageRate(string con, string text)
        {
            return objDL_User.GetAllWageRate( con,  text);
        }

        
        public DataSet IsWageRateIsUsed(Wage _objWage, Int32 userID)
        {
            return objDL_User.IsWageRateIsUsed(_objWage, userID);
        }
        public DataSet GetUserSearch(User _objUser)
        {
            return objDL_User.GetUserSearch(_objUser);
        }
        public DataSet GetUserSearch(User _objUser, string userRoleName)
        {
            return objDL_User.GetUserSearch(_objUser, userRoleName);
        }

        public void UpdateDocInfo(User objpropUser)
        {
            objDL_User.UpdateDocInfo(objpropUser);
        }

        //API
        public void UpdateDocInfo(UpdateDocInfoParam _UpdateDocInfoParam, string ConnectionString)
        {
            objDL_User.UpdateDocInfo(_UpdateDocInfoParam, ConnectionString);
        }
        public DataSet getElevByLoc(User objPropUser)
        {
            return objDL_User.getElevByLoc(objPropUser);
        }
        public DataSet GetAllTc(User objPropUser)
        {
            return objDL_User.GetAllTc(objPropUser);
        }
        public DataSet GetSearchPages(User objPropUser)
        {
            return objDL_User.GetSearchPages(objPropUser);
        }
        public void AddTerms(User _objUser)
        {
            objDL_User.AddTerms(_objUser);
        }
        public void UpdateTerms(User _objUser)
        {
            objDL_User.UpdateTerms(_objUser);
        }
        public bool IsExistPage(User _objUser)
        {
            return objDL_User.IsExistPage(_objUser);
        }
        public void DeleteTermsCondition(User _objUser)
        {
            objDL_User.DeleteTermsCondition(_objUser);
        }
        public bool IsExistPageForUpdate(User _objUser)
        {
            return objDL_User.IsExistPageForUpdate(_objUser);
        }
        public DataSet GetAllUseTax(User objPropUser)
        {
            return objDL_User.GetAllUseTax(objPropUser);
        }

        public DataSet GetAllUseTaxSearch(User objPropUser)
        {
            return objDL_User.GetAllUseTaxSearch(objPropUser);
        }



        public String CreateWarehouse(User objPropUser)
        {
            return objDL_User.CreateWarehouse(objPropUser);
        }
        public void UpdateInventoryWarehouse(User objPropUser)
        {
            objDL_User.UpdateInventoryWarehouse(objPropUser);
        }
        public DataSet GetInventoryWarehouse(User objPropUser)
        {
            return objDL_User.GetInventoryWarehouse(objPropUser);
        }

        public DataSet GetInventoryWarehouseByID(User objPropUser)
        {
            return objDL_User.GetInventoryWarehouseByID(objPropUser);
        }


        public void DeleteInventoryWareHouse(User objPropUser)
        {
            objDL_User.DeleteInventoryWareHouse(objPropUser);
        }

        //API
        public void DeleteInventoryWareHouse(DeleteInventoryWareHouseParam _DeleteInventoryWareHouseParam, string ConnectionString)
        {
            objDL_User.DeleteInventoryWareHouse(_DeleteInventoryWareHouseParam, ConnectionString);
        }

        public void CreateInventoryCategory(User objPropUser)
        {
            objDL_User.CreateInventoryCategory(objPropUser);
        }
        public void UpdateInventoryCategory(User objPropUser)
        {
            objDL_User.UpdateInventoryCategory(objPropUser);
        }
        public DataSet GetInventoryCategory(User objPropUser)
        {
            return objDL_User.GetInventoryCategory(objPropUser);
        }
        public void DeleteInventoryCategory(User objPropUser)
        {
            objDL_User.DeleteInventoryCategory(objPropUser);
        }

        public String CreateWareHouseLocation(User objPropUser)
        {
            return objDL_User.CreateWareHouseLocation(objPropUser);
        }
        public string UpdateWareHouseLocation(User objPropUser)
        {
            return objDL_User.UpdateWareHouseLocation(objPropUser);
        }
        public DataSet GetWareHouseLocation(User objPropUser)
        {
            return objDL_User.GetWareHouseLocation(objPropUser);
        }
        public void DeleteWareHouseLocation(User objPropUser)
        {
            objDL_User.DeleteWareHouseLocation(objPropUser);
        }

        public DataSet GetJobBillRatesById(User objPropUser)
        {
            return objDL_User.GetJobBillRatesById(objPropUser);
        }

        public DataSet GetAllWageForEstimate(Wage _objWage)
        {
            return objDL_User.GetAllWageForEstimate(_objWage);
        }
        public DataSet GetEstimateRoleSpecificDetails(Rol _objRol)
        {
            return objDL_User.GetGetEstimateRoleSpecificDetails(_objRol);
        }
        public DataSet GetEstimatePhoneContactSpecificDetails(int id, int estimateId)
        {
            return objDL_User.GetEstimatePhoneContactSpecificDetails(id, estimateId);
        }

        public DataSet getequipCustomItemsByElevID(User objPropUser)
        {
            return objDL_User.getequipCustomItemsByElevID(objPropUser);
        }

        public string getContractType(User objPropUser)
        {
            return objDL_User.getContractType(objPropUser);
        }

        //API
        public string getContractType(GetContractTypeParam _GetContractType, string ConnectionString)
        {
            return objDL_User.getContractType( _GetContractType, ConnectionString);
        }

        public DataSet getGCCustomer(User objPropUser)
        {
            return objDL_User.getGCCustomer(objPropUser);
        }

        //API
        public List<GetGCCustomerViewModel> getGCCustomer(GetGCCustomerParam _GetGCCustomer, string ConnectionString)
        {
            DataSet ds = objDL_User.getGCCustomer(_GetGCCustomer, ConnectionString);

            List<GetGCCustomerViewModel> _lstGetGCCustomer = new List<GetGCCustomerViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetGCCustomer.Add(
                    new GetGCCustomerViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        City = Convert.ToString(dr["City"]),
                        State = Convert.ToString(dr["State"]),
                        Zip = Convert.ToString(dr["Zip"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Address = Convert.ToString(dr["Address"]),
                        EMail = Convert.ToString(dr["EMail"]),
                        Country = Convert.ToString(dr["Country"]),
                        remarks = Convert.ToString(dr["remarks"]),
                        Cellular = Convert.ToString(dr["Cellular"]),
                        rol = Convert.ToInt32(DBNull.Value.Equals(dr["rol"]) ? 0 : dr["rol"]),
                        type = Convert.ToString(dr["type"]),
                    }
                    );
            }

            return _lstGetGCCustomer;
        }

        public DataSet GetEmpWageItems(User objPropUser)
        {
            return objDL_User.GetEmpWageItems(objPropUser);
        }

        public DataSet getGCAutojquery(User objPropUser)
        {
            return objDL_User.getGCAutojquery(objPropUser);
        }
        public DataSet getHomeOwnerAutojquery(User objPropUser)
        {
            return objDL_User.getHomeOwnerAutojquery(objPropUser);
        }

        public DataSet GetEquipmentTests(User objPropUser)
        {
            return objDL_User.GetEquipmentTests(objPropUser);
        }
        public DataSet GetAllTestByEquipmentID(User objPropUser)
        {
            return objDL_User.GetAllTestByEquipmentID(objPropUser);
        }
        //API
        public List<GetEquipmentTestsViewModel> GetEquipmentTests(GetEquipmentTestsParam _GetEquipmentTests, string ConnectionString)
        {
            DataSet ds = objDL_User.GetEquipmentTests(_GetEquipmentTests, ConnectionString);

            List<GetEquipmentTestsViewModel> _lstGetEquipmentTests = new List<GetEquipmentTestsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipmentTests.Add(
                    new GetEquipmentTestsViewModel()
                    {
                        idUnit = Convert.ToInt32(DBNull.Value.Equals(dr["idUnit"]) ? 0 : dr["idUnit"]),
                        idTestItem = Convert.ToInt32(DBNull.Value.Equals(dr["idTestItem"]) ? 0 : dr["idTestItem"]),
                        Name = Convert.ToString(dr["Name"]),
                        Status = Convert.ToString(dr["Status"]),
                        Last = Convert.ToDateTime(DBNull.Value.Equals(dr["Last"]) ? null : dr["Last"]),
                        Next = Convert.ToDateTime(DBNull.Value.Equals(dr["Next"]) ? null : dr["Next"]),
                        Ticketed = Convert.ToInt32(DBNull.Value.Equals(dr["Ticketed"]) ? 0 : dr["Ticketed"]),
                        Ticket = Convert.ToInt32(DBNull.Value.Equals(dr["Ticket"]) ? 0 : dr["Ticket"]),
                    }
                    );
            }

            return _lstGetEquipmentTests;
        }
        public int AddSageContracts(User objPropUser)
        {
            return objDL_User.AddSageContracts(objPropUser);
        }

        public DataSet getUpdateContractsForSage(User objPropUser)
        {
            return objDL_User.getUpdateContractsForSage(objPropUser);
        }


        public void UpdateDoc(User objPropUser)
        {
            objDL_User.UpdateDoc(objPropUser);
        }

        public DataSet getRouteByID(User objPropUser)
        {
            return objDL_User.getRouteByID(objPropUser);
        }
        public DataSet GetRouteLogs(User objPropUser)
        {
            return objDL_User.GetRouteLogs(objPropUser);
        }
        public int AddRoute(User objPropUser)
        {
            return objDL_User.AddRoute(objPropUser);
        }

        public DataSet getDefaultRouteTerr(User objPropUser)
        {
            return objDL_User.getDefaultRouteTerr(objPropUser);
        }

        //API
        public ListGetDefaultRouteTerr getDefaultRouteTerr(GetDefaultRouteTerrParam _GetDefaultRouteTerr, string ConnectionString)
        {
            DataSet ds = objDL_User.getDefaultRouteTerr(_GetDefaultRouteTerr, ConnectionString);

            ListGetDefaultRouteTerr _ds = new ListGetDefaultRouteTerr();
            List<GetDefaultRouteTerrTable1> _lstTable1 = new List<GetDefaultRouteTerrTable1>();
            List<GetDefaultRouteTerrTable2> _lstTable2 = new List<GetDefaultRouteTerrTable2>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstTable1.Add(
                    new GetDefaultRouteTerrTable1()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                    }
                    );
            }
            _ds.lstTable1 = _lstTable1;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                _lstTable2.Add(
                    new GetDefaultRouteTerrTable2()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                    }
                    );
            }
            _ds.lstTable2 = _lstTable2;

            return _ds;
        }
        public DataSet GetSTaxByName(User objPropUser)
        {
            return objDL_User.GetSTaxByName(objPropUser);
        }

        public void AddPreferences(User _user)
        {
            objDL_User.AddPreferences(_user);
        }
        public DataSet getPreferences(User _user)
        {
            return objDL_User.GetPreferences(_user);
        }

        public DataSet GetEquipmentTypeCount(User _user)
        {
            return objDL_User.GetEquipmentTypeCount(_user);
        }

        //API
        public List<EquipmentTypeCountResponse> GetEquipmentTypeCountList(string _conString)
        {
            User _user = new User();
            _user.ConnConfig = _conString;
            DataSet ds = objDL_User.GetEquipmentTypeCountSP(_user);
            List<EquipmentTypeCountResponse> _lstEquipmentTypeCountResponse = new List<EquipmentTypeCountResponse>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstEquipmentTypeCountResponse.Add(new EquipmentTypeCountResponse()
                {
                    TypeName = Convert.ToString(dr["TypeName"].ToString()),
                    Total = Convert.ToInt32(dr["Total"]),
                });
            }
            return _lstEquipmentTypeCountResponse;
        }

        public DataSet GetEquipmentBuildingCount(User _user)
        {
            return objDL_User.GetEquipmentBuildingCount(_user);
        }

        //API
        public List<EquipmentBuildingCountResponse> GetEquipmentBuildingCountList(string conString)
        {
            User _user = new User();
            _user.ConnConfig = conString;
            DataSet ds = objDL_User.GetEquipmentBuildingCountSP(_user);
            List<EquipmentBuildingCountResponse> _EquipmentBuildingCount = new List<EquipmentBuildingCountResponse>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _EquipmentBuildingCount.Add(new EquipmentBuildingCountResponse()
                {
                    Building = Convert.ToString(dr["Building"].ToString()),
                    Total = Convert.ToInt32(dr["Total"]),
                });
            }
            return _EquipmentBuildingCount;
        }

        public DataSet GetLocationsStatus(User _user)
        {
            return objDL_User.GetLocationsStatus(_user);
        }

        public int[] GetTicketStatus(User _user)
        {
            return objDL_User.GetTicketStatus(_user);
        }

        public int[] GetSixtyPlusAR(User _user)
        {
            return objDL_User.GetTicketStatus(_user);
        }

        public DataSet Get12MonthActualvsBudgetData(User _user, string selectedBudget)
        {
            return objDL_User.Get12MonthActualvsBudgetData(_user, selectedBudget);
        }

        public DataSet Get12MonthActualvsBudgetGraphData(User _user, int? budgetID, int fiscalYear)
        {
            return objDL_User.Get12MonthActualvsBudgetGraphData(_user, budgetID, fiscalYear);
        }

        public DataSet GetLast12MonthActualvsBudgetData(User _user, string fiscalYear, int endMonth)
        {
            return objDL_User.GetLast12MonthActualvsBudgetData(_user, fiscalYear, endMonth);
        }

        public DataSet GetListBudgetName(User _user)
        {
            return objDL_User.GetListBudgetName(_user);
        }

        //API
        public List<BudgetResponse> GetBudgetNameList(string ConnConfig)
        {
            User _user = new User();
            _user.ConnConfig = ConnConfig;
            List<BudgetResponse> lst = new List<BudgetResponse>();
            DataSet ds = objDL_User.GetBudgetNameList(_user);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(new BudgetResponse()
                {
                    BudgetID = Convert.ToInt32(dr["BudgetID"].ToString()),
                    Budget = Convert.ToString(dr["Budget"].ToString())
                });
            }
            return lst;
        }
        public DataTable[] GetEstimatedAndTotalHoursCompleted(User _user)
        {
            return objDL_User.GetEstimatedAndTotalHoursCompleted(_user);
        }

        //API 
        public List<RecurringHoursRemainingResponse> GetRecurringHoursRemainingChart(User _user)
        {
            List<RecurringHoursRemainingResponse> lst = new List<RecurringHoursRemainingResponse>();
            DataSet ds = objDL_User.GetEstimatedAndTotalHoursCompletedDS(_user);
            var totalHours = getRouteHour(ds.Tables[0]);
            var completedHours = getRouteHour(ds.Tables[1]);
            var openHours = getRouteHour(ds.Tables[2]);

            var AllRoutes = new HashSet<string>();

            foreach (var obj in totalHours)
            {
                if (!AllRoutes.Contains(obj.Route))
                    AllRoutes.Add(obj.Route);
            }

            foreach (var obj in completedHours)
            {
                if (!AllRoutes.Contains(obj.Route))
                    AllRoutes.Add(obj.Route);
            }

            foreach (var obj in openHours)
            {
                if (!AllRoutes.Contains(obj.Route))
                    AllRoutes.Add(obj.Route);
            }


            foreach (var obj in AllRoutes)
            {
                var item = new RecurringHoursRemainingResponse();
                item.Category = obj == "" ? "Unassigned" : obj;

                var routeHour = totalHours.Where(x => x.Route == obj);
                if (routeHour.FirstOrDefault() != null)
                {
                    item.TotalHours = routeHour.Sum(x => x.Total);
                }
                else
                {
                    item.TotalHours = 0;
                }

                routeHour = completedHours.Where(x => x.Route == obj);
                if (routeHour.FirstOrDefault() != null)
                {
                    item.Completed = routeHour.Sum(x => x.Total);
                }
                else
                {
                    item.Completed = 0;
                }

                routeHour = openHours.Where(x => x.Route == obj);
                if (routeHour.FirstOrDefault() != null)
                {
                    item.Open = routeHour.Sum(x => x.Total);
                }
                else
                {
                    item.Open = 0;
                }

                lst.Add(item);
            }
            return lst;
        }
        private List<RouteHour> getRouteHour(DataTable table)
        {
            var result = new List<RouteHour>();

            if (table != null)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    result.Add(new RouteHour() { Total = double.Parse(table.Rows[i]["Total"].ToString()), Route = table.Rows[i]["Route"].ToString() });
                }
            }
            return result;
        }
        public DataTable[] GetTicketRecurringOpenAndCompleted(User _user)
        {
            return objDL_User.GetTicketRecurringOpenAndCompleted(_user);
        }
        //API
        public List<RecurringHoursRemainingResponse> GetTicketRecurringOpenAndCompletedList(User _user)
        {
            List<RecurringHoursRemainingResponse> lst = new List<RecurringHoursRemainingResponse>();
            DataSet ds = objDL_User.GetTicketRecurringOpenAndCompletedDS(_user);


            var openHours = getRecurringHour(ds.Tables[0]);
            var completedHours = getRecurringHour(ds.Tables[1]);

            var allRoutes = new HashSet<string>();

            foreach (var obj in openHours)
            {
                if (!allRoutes.Contains(obj.Item1))
                    allRoutes.Add(obj.Item1);
            }

            foreach (var obj in completedHours)
            {
                if (!allRoutes.Contains(obj.Item1))
                    allRoutes.Add(obj.Item1);
            }

            foreach (var obj in allRoutes)
            {
                var item = new RecurringHoursRemainingResponse();
                item.Category = obj == "" ? "Unassigned" : obj;

                var complete = completedHours.Where(x => x.Item1 == obj);
                if (complete != null && complete.Count() > 0)
                {
                    item.Completed = complete.Sum(x => x.Item2);
                }
                else
                {
                    item.Completed = 0;
                }

                var open = openHours.Where(x => x.Item1 == obj);
                if (open != null && open.Count() > 0)
                {
                    item.Open = open.Sum(x => x.Item2);
                }
                else
                {
                    item.Open = 0;
                }

                lst.Add(item);
            }

            return lst;
        }
        private List<Tuple<string, double>> getRecurringHour(DataTable table)
        {
            var result = new List<Tuple<string, double>>();

            if (table != null)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    result.Add(Tuple.Create(table.Rows[i]["DWork"].ToString(), double.Parse(table.Rows[i]["Total"].ToString())));
                }
            }
            return result;
        }
        public ArrayList GetAvgEstimate(User _user)
        {
            return objDL_User.GetAvgEstimate(_user);
        }

        public AvgEstimateResponse GetAvgEstimateList(User _user)
        {
            AvgEstimateResponse _objAvgEstimateResponse = new AvgEstimateResponse();
            _objAvgEstimateResponse = objDL_User.GetAvgEstimateList(_user);
            return _objAvgEstimateResponse;
        }

        public DataTable[] ConvertedEstimatesBySalespersonAverageDays(User _user)
        {
            return objDL_User.ConvertedEstimatesBySalespersonAverageDays(_user);
        }

        //API
        // public RecurringChartResponse GetEstimatesBySalespersonAverageDays(User _user)
        public List<LeadAverageResponse> GetEstimatesBySalespersonAverageDays(User _user)

        {
            List<LeadAverageResponse> _lst = new List<LeadAverageResponse>();
            RecurringChartResponse obj = new RecurringChartResponse();
            DataSet ds = objDL_User.ConvertedEstimatesBySalespersonAverageDaysList(_user);
            List<ResponseTable1> lst1 = new List<ResponseTable1>();
            List<ResponseTable2> lst2 = new List<ResponseTable2>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ResponseTable1 o = new ResponseTable1();
                o.SalesPerson = Convert.ToString(dr["SalesPerson"].ToString());
                o.Avg = Convert.ToDouble(dr["Avg"].ToString());
                lst1.Add(o);
                LeadAverageResponse ol = new LeadAverageResponse();
                ol.SalesPerson = Convert.ToString(dr["SalesPerson"].ToString());
                ol.Avg = Convert.ToDouble(dr["Avg"].ToString());
                _lst.Add(ol);

            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                ResponseTable2 o2 = new ResponseTable2();
                o2.SalesPerson = Convert.ToString(dr["SalesPerson"].ToString());
                o2.Avg = Convert.ToDouble(dr["Avg"].ToString());
                lst2.Add(o2);
            }
            obj.ResponseTable1 = lst1;
            obj.ResponseTable2 = lst2;
            return _lst;
        }

        public List<LeadAverageResponse> GetEstimatesBySalespersonAverage(string _ConString)
        {
             User _user = new User();
            _user.ConnConfig = _ConString;
            List<LeadAverageResponse> _lst = new List<LeadAverageResponse>();
            DataSet ds = objDL_User.ConvertedEstimatesBySalespersonAverageDaysList(_user);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LeadAverageResponse ol = new LeadAverageResponse();
                ol.SalesPerson = Convert.ToString(dr["SalesPerson"].ToString());
                ol.Avg = Convert.ToDouble(dr["Avg"].ToString());
                _lst.Add(ol);
            }
            return _lst;
        }
        public List<LeadAverageResponse> GetEstimatesBySalespersonAverageDaysTest(string conString)

        {
            User _user = new User();
            _user.ConnConfig = conString;//HttpContext.Current.Session["config"].ToString();
            List<LeadAverageResponse> _lst = new List<LeadAverageResponse>();
            RecurringChartResponse obj = new RecurringChartResponse();
            DataSet ds = objDL_User.ConvertedEstimatesBySalespersonAverageDaysList(_user);
            List<ResponseTable1> lst1 = new List<ResponseTable1>();
            List<ResponseTable2> lst2 = new List<ResponseTable2>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ResponseTable1 o = new ResponseTable1();
                o.SalesPerson = Convert.ToString(dr["SalesPerson"].ToString());
                o.Avg = Convert.ToDouble(dr["Avg"].ToString());
                lst1.Add(o);
                LeadAverageResponse ol = new LeadAverageResponse();
                ol.SalesPerson = Convert.ToString(dr["SalesPerson"].ToString());
                ol.Avg = Convert.ToDouble(dr["Avg"].ToString());
                _lst.Add(ol);

            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                ResponseTable2 o2 = new ResponseTable2();
                o2.SalesPerson = Convert.ToString(dr["SalesPerson"].ToString());
                o2.Avg = Convert.ToDouble(dr["Avg"].ToString());
                lst2.Add(o2);
            }
            obj.ResponseTable1 = lst1;
            obj.ResponseTable2 = lst2;
            return _lst;
        }
        public DataSet GetUserDatePermission(User objPropUser)
        {
            return objDL_User.GetUserDatePermission(objPropUser);
        }

        public void SetCloseOutDate(User objPropUser)
        {
            objDL_User.SetCloseOutDate(objPropUser);
        }

        public bool CheckNullCODt(User objPropUser)
        {
            return objDL_User.CheckNullCODt(objPropUser);
        }
        public DataSet GetCODt(User objPropUser)
        {
            return objDL_User.GetCODt(objPropUser);
        }
        public DataSet GetCustomersLogs(User objPropUser)
        {
            return objDL_User.GetCustomersLogs(objPropUser);
        }

        //API
        public List<GetCustomersLogsViewModel> GetCustomersLogs(GetCustomersLogsParam _GetCustomersLogs, string ConnectionString)
        {
            DataSet ds = objDL_User.GetCustomersLogs(_GetCustomersLogs, ConnectionString);

            List<GetCustomersLogsViewModel> _lstGetCustomersLogs = new List<GetCustomersLogsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCustomersLogs.Add(
                    new GetCustomersLogsViewModel()
                {
                    fUser = Convert.ToString(dr["fUser"]),
                    Screen = Convert.ToString(dr["Screen"].ToString()),
                    CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                    fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                    Field = Convert.ToString(dr["Field"]),
                    fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    NewVal = Convert.ToString(dr["NewVal"]),
                    OldVal = Convert.ToString(dr["OldVal"].ToString()),
                    Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                });
            }

            return _lstGetCustomersLogs;
        }
        public DataSet GetPeriodCloseoutLogs(User objPropUser)
        {
            return objDL_User.GetPeriodCloseoutLogs(objPropUser);
        }
        public void AddPeriodCloseoutLogs(User objPropUser)
        {
            objDL_User.AddPeriodCloseoutLogs(objPropUser);
        }
        public DataSet getTeamByMonUser(String connConfig, String momUser)
        {
            return objDL_User.getTeamByMonUser(connConfig, momUser);
        }
        public DataSet getTeamByListID(String connConfig, String lsId)
        {
            return objDL_User.getTeamByListID(connConfig, lsId);
        }
        public DataSet GetMonthlyRevenueByCompany(User objPropUser, int? companyId)
        {
            return objDL_User.GetMonthlyRevenueByCompany(objPropUser, companyId);
        }

        //API
        public List<RevenueResponse> GetRevenueByCompany(User objPropUser)
        {
            List<RevenueResponse> lst = new List<RevenueResponse>();
            DataSet ds = objDL_User.GetRevenueByCompany(objPropUser, objPropUser.CompanyID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(new RevenueResponse()
                {
                    Department = Convert.ToString(dr["Department"].ToString()),
                    Revenue = Convert.ToString(dr["Revenue"].ToString())
                });
            }
            return lst;
        }

        public DataSet GetTroubleCallsByEquipment(User objPropUser, TroubleCallsByEquipmentGraphRequest request)
        {
            return objDL_User.GetTroubleCallsByEquipment(objPropUser, request);
        }
        //API
        public List<TroubleCallsEquipmentResponse> GetTroubleCallsByEquipmentList(TroubleCallsByEquipmentGraphRequest request)
        {
            List<TroubleCallsEquipmentResponse> lst = new List<TroubleCallsEquipmentResponse>();
            DataSet ds = objDL_User.GetTroubleCallsByEquipment(request);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(new TroubleCallsEquipmentResponse()
                {
                    Unit = Convert.ToString(dr["Unit"].ToString()),
                    Type = Convert.ToString(dr["Type"].ToString()),
                    Tag = Convert.ToString(dr["Tag"].ToString()),
                    ContractNumber = Convert.ToString(dr["ContractNumber"].ToString()),
                    CType = Convert.ToString(dr["CType"].ToString())
                });
            }
            return lst;
        }

        public DataSet GetTicketCountByCategory(User objPropUser, string categories)
        {
            return objDL_User.GetTicketCountByCategory(objPropUser, categories);
        }
        //API
        public List<CategoryTicketCountResponse> GetTicketCountByCategoryList(GetCategoryTicketCountParam obj)
        {
            List<CategoryTicketCountResponse> lst = new List<CategoryTicketCountResponse>();
            User objPropUser = new User();
            objPropUser.StartDate = Convert.ToString(obj.StartDate);
            objPropUser.EndDate = Convert.ToString(obj.EndDate);
            objPropUser.UserID = obj.UserID;
            objPropUser.EN = obj.EN;
            objPropUser.ConnConfig = obj.ConnConfig;
            DataSet ds = objDL_User.GetTicketCountByCategory(objPropUser, obj.Categories);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(new CategoryTicketCountResponse()
                {
                    Category = Convert.ToString(dr["Category"].ToString()),
                    TicketCount = Convert.ToString(dr["Count"].ToString()),
                });
            }

            return lst;
        }
        public int AddDashboard(User objPropUser, string name, bool isDefault)
        {
            return objDL_User.AddDashboard(objPropUser, name, isDefault);
        }

        public void UpdateDashboard(User objPropUser, int dashboardID, string dashboardName, bool isDefault)
        {
            objDL_User.UpdateDashboard(objPropUser, dashboardID, dashboardName, isDefault);
        }

        public void UpdateDashboardDockStates(User objPropUser, int dashboardID, string dockStates)
        {
            objDL_User.UpdateDashboardDockStates(objPropUser, dashboardID, dockStates);
        }
        // API
        public void UpdateDashboardDockStatesySP(UpdateDashboardDockStatesParam obj)
        {
            objDL_User.UpdateDashboardDockStatesySP(obj);
        }


        public void UnsetDashboardDefault(User objPropUser)
        {
            objDL_User.UnsetDashboardDefault(objPropUser);
        }

        public void DeleteDashboard(User objPropUser, int dashboardID)
        {
            objDL_User.DeleteDashboard(objPropUser, dashboardID);
        }

        public DataSet GetListDashboard(User objPropUser)
        {
            return objDL_User.GetListDashboard(objPropUser);
        }
        public List<Dashboard> GetDashboardByUserId(GetDashboardParam _GetDashboardParam, string ConnectionString)
        {
            List<Dashboard> objlist = new List<Dashboard>();
            objlist = objDL_User.GetDashboardByUserId(_GetDashboardParam, ConnectionString);
            return objlist;
        }
        public DataSet GetDashboardByID(User objPropUser, int dashboardID)
        {
            return objDL_User.GetDashboardByID(objPropUser, dashboardID);
        }

        //API
        public List<Dashboard> GetDashboard(GetDashboardParam _GetDashboardParam, string ConnectionString)
        {
            List<Dashboard> objlist = new List<Dashboard>();
            objlist = objDL_User.GetDashboard(_GetDashboardParam, ConnectionString);
            return objlist;
        }
        public DataSet GetDashboardDefault(User objPropUser)
        {
            return objDL_User.GetDashboardDefault(objPropUser);
        }

        public void AddUserDashboard(User objPropUser, UserDash request)
        {
            objDL_User.AddUserDashboard(objPropUser, request);
        }

        public DataSet GetListKPIs(User objPropUser)
        {
            return objDL_User.GetListKPIs(objPropUser);
        }

        public DataSet GetListDashKPI(User objPropUser, int dashboardID)
        {
            return objDL_User.GetListDashKPI(objPropUser, dashboardID);
        }


        //API
        public List<KPI> GetListDashKPI(GetListDashKPIParam _GetListDashKPIParam, string ConnectionString)
        {
            List<KPI> objlist = new List<KPI>();
            objlist = objDL_User.GetListDashKPI(_GetListDashKPIParam, ConnectionString);
            return objlist;
        }

        public void DeleteUserDash(User objPropUser, int dashboardID)
        {
            objDL_User.DeleteUserDash(objPropUser, dashboardID);
        }

        #region "Classification"
        public DataSet getEquipClassification(User objPropUser)
        {
            return objDL_User.getEquipClassification(objPropUser);
        }
        public DataSet getEquipClassificationActive(User objPropUser)
        {
            return objDL_User.getEquipClassificationActive(objPropUser);
        }
        public DataSet getEquipClassificationLikeName(User objPropUser)
        {
            return objDL_User.getEquipClassificationLikeName(objPropUser);
        }

        //API
        public List<GetEquiptypeViewModel> getEquipClassification(GetEquipClassificationParam _GetEquipClassification, string ConnectionString)
        {
            DataSet ds = objDL_User.getEquipClassification(_GetEquipClassification, ConnectionString);

            List<GetEquiptypeViewModel> _lstGetEquipmentCategory = new List<GetEquiptypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipmentCategory.Add(
                    new GetEquiptypeViewModel()
                    {
                        edesc = Convert.ToString(dr["edesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetEquipmentCategory;
        }

        public DataSet getLeadEquipClassification(User objPropUser)
        {
            return objDL_User.getLeadEquipClassification(objPropUser);
        }

        //API
        public List<GetEquiptypeViewModel> getLeadEquipClassification(GetLeadEquipClassificationParam _GetLeadEquipClassification, string ConnectionString)
        {
            DataSet ds = objDL_User.getLeadEquipClassification(_GetLeadEquipClassification, ConnectionString);
            List<GetEquiptypeViewModel> _lstGetEquipmentCategory = new List<GetEquiptypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipmentCategory.Add(
                    new GetEquiptypeViewModel()
                    {
                        edesc = Convert.ToString(dr["edesc"]),
                        Label = Convert.ToString(dr["Label"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                    });
            }
            return _lstGetEquipmentCategory;
        }

        public void AddEquipClassification(User objPropUser)
        {
            objDL_User.AddEquipClassification(objPropUser);
        }
        public void EditEquipClassification(User objPropUser)
        {
            objDL_User.EditEquipClassification(objPropUser);
        }
        public void DeleteEquipClassification(User objPropUser)
        {
            objDL_User.DeleteEquipClassification(objPropUser);
        }

        #endregion
        #region "EquipmentTestPricing"

        public int AddEquipmentTestPricing(String connConfig, EquipTestPrice equipTest)
        {
            return objDL_User.AddEquipmentTestPricing(connConfig, equipTest);
        }

        public void UpdateEquipmentTestPricing(String connConfig, EquipTestPrice equipTest)
        {
            objDL_User.UpdateEquipmentTestPricing(connConfig, equipTest);

        }

        public void DeleteEquipmentTestPricingById(String connConfig, int Id)
        {
            objDL_User.DeleteEquipmentTestPricingById(connConfig, Id);

        }

        public DataSet GetAllEquipmentTestPricing(String connConfig)
        {
            return objDL_User.GetAllEquipmentTestPricing(connConfig);

        }

        public DataSet ValidateEquipmentTestPricing(String connConfig, String classification, int testTypeId,int priceYear=0)
        {
            return objDL_User.ValidateEquipmentTestPricing(connConfig, classification, testTypeId,priceYear);
        }
        public DataSet DuplicateEquipTestPrice(String connConfig, String classification, int testTypeId, int priceYear = 0)
        {
            return objDL_User.DuplicateEquipTestPrice(connConfig, classification, testTypeId, priceYear);
        }
        #endregion

        public DataSet GetShutdownReasons(User objPropUser)
        {
            return objDL_User.GetShutdownReasons(objPropUser);
        }

        //API
        public List<GetShutdownReasonsViewModel> GetShutdownReasons(GetShutdownReasonsParam _GetShutdownReasons, string ConnectionString)
        {
            DataSet ds = objDL_User.GetShutdownReasons(_GetShutdownReasons, ConnectionString);
            List<GetShutdownReasonsViewModel> _lstGetShutdownReasons = new List<GetShutdownReasonsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetShutdownReasons.Add(
                    new GetShutdownReasonsViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Reason = Convert.ToString(dr["Reason"]),
                        Planned = Convert.ToBoolean(DBNull.Value.Equals(dr["Planned"]) ? 0 : dr["Planned"]),
                        CreatedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedDate"]) ? null : dr["CreatedDate"]),
                        CreatedBy = Convert.ToString(dr["CreatedBy"]),
                        UpdatedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["UpdatedDate"]) ? null : dr["UpdatedDate"]),
                        UpdatedBy = Convert.ToString(dr["UpdatedBy"]),
                    });
            }
            return _lstGetShutdownReasons;

        }

        public DataSet GetShutdownReasonByID(User objPropUser, int eqsdReasonID)
        {
            return objDL_User.GetShutdownReasonByID(objPropUser, eqsdReasonID);
        }

        //API
        public List<GetShutdownReasonsViewModel> GetShutdownReasonByID(GetShutdownReasonByIDParam _GetShutdownReasonByID, string ConnectionString, int eqsdReasonID)
        {
            DataSet ds = objDL_User.GetShutdownReasonByID(_GetShutdownReasonByID, ConnectionString, eqsdReasonID);

            List<GetShutdownReasonsViewModel> _lstGetShutdownReasons = new List<GetShutdownReasonsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetShutdownReasons.Add(
                    new GetShutdownReasonsViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Reason = Convert.ToString(dr["Reason"]),
                        Planned = Convert.ToBoolean(DBNull.Value.Equals(dr["Planned"]) ? 0 : dr["Planned"]),
                        CreatedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedDate"]) ? null : dr["CreatedDate"]),
                        CreatedBy = Convert.ToString(dr["CreatedBy"]),
                        UpdatedDate = Convert.ToDateTime(DBNull.Value.Equals(dr["UpdatedDate"]) ? null : dr["UpdatedDate"]),
                        UpdatedBy = Convert.ToString(dr["UpdatedBy"]),
                    });
            }
            return _lstGetShutdownReasons;
        }

        public void EditShutdownReason(User objPropUser, int eqsdID, string eqsdReason, bool eqsdPlanned)
        {
            objDL_User.EditShutdownReason(objPropUser, eqsdID, eqsdReason, eqsdPlanned);
        }

        //API
        public void EditShutdownReason(EditShutdownReasonParam _EditShutdownReason, string ConnectionString, int eqsdID, string eqsdReason, bool eqsdPlanned)
        {
            objDL_User.EditShutdownReason(_EditShutdownReason, ConnectionString, eqsdID, eqsdReason, eqsdPlanned);
        }

        public void AddShutdownReason(User objPropUser, string eqsdReason, bool eqsdPlanned)
        {
            objDL_User.AddShutdownReason(objPropUser, eqsdReason, eqsdPlanned);
        }

        //API
        public void AddShutdownReason(AddShutdownReasonParam _AddShutdownReason, string ConnectionString, string eqsdReason, bool eqsdPlanned)
        {
            objDL_User.AddShutdownReason(_AddShutdownReason, ConnectionString, eqsdReason, eqsdPlanned);
        }

        public DataSet GetEquipShutdownLogs(User objPropUser)
        {
            return objDL_User.GetEquipShutdownLogs(objPropUser);
        }

        //API
        public List<GetEquipShutdownLogsViewModel> GetEquipShutdownLogs(GetEquipShutdownLogsParam _GetEquipShutdownLogs, string ConnectionString)
        {
            DataSet ds = objDL_User.GetEquipShutdownLogs(_GetEquipShutdownLogs, ConnectionString);

            List<GetEquipShutdownLogsViewModel> _lstGetEquipShutdownLogs = new List<GetEquipShutdownLogsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipShutdownLogs.Add(
                    new GetEquipShutdownLogsViewModel()
                    {
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        ticket_id = Convert.ToString(dr["ticket_id"]),
                        status = Convert.ToString(dr["status"]),
                        elev_id = Convert.ToInt32(DBNull.Value.Equals(dr["elev_id"]) ? 0 : dr["elev_id"]),
                        created_on = Convert.ToDateTime(DBNull.Value.Equals(dr["created_on"]) ? null : dr["created_on"]),
                        worker = Convert.ToString(dr["worker"]),
                        reason = Convert.ToString(dr["reason"]),
                        longdesc = Convert.ToString(dr["longdesc"]),
                    });
            }
            return _lstGetEquipShutdownLogs;
        }

        public DataSet getPayRoll(User objPropUser)
        {
            return objDL_User.getPayRoll(objPropUser);
        }

        //api
        public List<PayrollViewModel> getPayRoll(getTimesheetParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.getPayRoll(objPropUser, ConnectionString);

            List<PayrollViewModel> _payrollViewModel = new List<PayrollViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _payrollViewModel.Add(
                    new PayrollViewModel()
                    {
                        CoCode = dr["CoCode"].ToString(),
                        BatchID = dr["BatchID"].ToString(),
                        EmpRef = dr["EmpRef"].ToString(),
                        Shift = Convert.ToInt32(DBNull.Value.Equals(dr["Shift"]) ? 0 : dr["Shift"]),
                        TempDept = dr["TempDept"].ToString(),
                        RateCode = dr["RateCode"].ToString(),
                        RegHours = Convert.ToInt32(DBNull.Value.Equals(dr["RegHours"]) ? 0 : dr["RegHours"]),
                        OTHours = Convert.ToInt32(DBNull.Value.Equals(dr["OTHours"]) ? 0 : dr["OTHours"]),
                        Hour3Code = dr["Hour3Code"].ToString(),
                        Hours3Amount = dr["Hour3Amount"].ToString()
                    }
                    );
            }
            return _payrollViewModel;
        }

        public void UpdateCoCode(User objPropUser)
        {
            objDL_User.UpdateCoCode(objPropUser);
        }
        //api
        public void UpdateCoCode(getConnectionConfigParam objPropUser, string ConnectionString)
        {
            objDL_User.UpdateCoCode(objPropUser, ConnectionString);
        }

        public DataSet GetCoCode(User objPropUser)
        {
            return objDL_User.GetCoCode(objPropUser);
        }

        //api
        public List<UserViewModel> GetCoCode(getConnectionConfigParam objPropUser, string ConnectionString)
        {
            DataSet ds = objDL_User.GetCoCode(objPropUser, ConnectionString);
            List<UserViewModel> _userViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _userViewModel.Add(
                    new UserViewModel()
                    {
                        CoCode = dr["CoCode"].ToString(),

                    }
                    );
            }
            return _userViewModel;
        }

        public DataSet GetUsersForTeamMemberList(User objPropUser)
        {
            return objDL_User.GetUsersForTeamMemberList(objPropUser);
        }

        public DataSet GetUsersAndRolesForTeamMemberList(User objPropUser)
        {
            return objDL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        }

        public DataSet GetTeamMemberFromTemplate(User objPropUser)//, int customLabelId)
        {
            return objDL_User.GetTeamMemberFromTemplate(objPropUser);//, customLabelId);
        }

        public DataSet GetLocInfoForEstimateByID(User objPropUser)
        {
            return objDL_User.GetLocInfoForEstimateByID(objPropUser);
        }

        public DataSet GetGridUserSettings(User objPropUser)
        {
            return objDL_User.GetGridUserSettings(objPropUser);
        }
        public DataSet getLevels(User objPropUser)
        {
            return objDL_User.getLevels(objPropUser);
        }

        public string getUserEmailFromTS(User objPropUser)
        {
            return objDL_User.getUserEmailFromTS(objPropUser);
        }

        public void UpdateReapproveFromFDESC(string connConfig)
        {
            objDL_User.UpdateReapproveFromFDESC(connConfig);
        }


        public void AddUpdateCustomerQB(User objPropUser)
        {
            objDL_User.AddUpdateCustomerQB(objPropUser);
        }

        public void AddUpdateQBLocation(User objPropUser)
        {
            objDL_User.AddUpdateQBLocation(objPropUser);
        }
        public void UpdateLocationContactRecordLog(User objPropUser)
        {
            objDL_User.UpdateLocationContactRecordLog(objPropUser);
        }

        //API
        public void UpdateLocationContactRecordLog(UpdateLocationContactRecordLogParam _UpdateLocationContactRecordLog, string ConnectionString)
        {
            objDL_User.UpdateLocationContactRecordLog(_UpdateLocationContactRecordLog, ConnectionString);
        }
        public DataSet getLocationLog(User objPropUser)
        {
            return objDL_User.getLocationLog(objPropUser);
        }

        //API
        public List<GetLocationLogViewModel> getLocationLog(GetLocationLogParam _GetLocationLog, string ConnectionString)
        {
            DataSet ds = objDL_User.getLocationLog(_GetLocationLog, ConnectionString);

            List<GetLocationLogViewModel> _lstGetLocationLog = new List<GetLocationLogViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationLog.Add(
                    new GetLocationLogViewModel()
                    {
                        fUser = Convert.ToString(dr["fUser"]),
                        Screen = Convert.ToString(dr["Screen"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }
            return _lstGetLocationLog;
        }
        public DataSet getContactLogByLocID(User objPropUser)
        {
            return objDL_User.getContactLogByLocID(objPropUser);
        }

        //API
        public List<GetContactLogByLocIDViewModel> getContactLogByLocID(GetContactLogByLocIDParam _GetContactLogByLocID, string ConnectionString)
        {
            DataSet ds = objDL_User.getContactLogByLocID(_GetContactLogByLocID, ConnectionString);

            List<GetContactLogByLocIDViewModel> _lstGetContactLogByLocID = new List<GetContactLogByLocIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetContactLogByLocID.Add(
                    new GetContactLogByLocIDViewModel()
                    {
                        fDesc = Convert.ToString(dr["fDesc"]),
                        fUser = Convert.ToString(dr["fUser"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        Field = Convert.ToString(dr["Field"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }
            return _lstGetContactLogByLocID;
        }
        public DataSet getLocContactByRolID(User objPropUser)
        {
            return objDL_User.getLocContactByRolID(objPropUser);
        }

        public DataSet GetLocContactByRolIDCustomer(User objPropUser)
        {
            return objDL_User.GetLocContactByRolIDCustomer(objPropUser);
        }

        //API
        public List<GetLocContactByRolIDViewModel> getLocContactByRolID(GetLocContactByRolIDParam _GetLocContactByRolID, string ConnectionString)
        {
            DataSet ds = objDL_User.getLocContactByRolID(_GetLocContactByRolID, ConnectionString);

            List<GetLocContactByRolIDViewModel> _lstGetLocContactByRolID = new List<GetLocContactByRolIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocContactByRolID.Add(
                    new GetLocContactByRolIDViewModel()
                    {
                        contactid = Convert.ToInt32(DBNull.Value.Equals(dr["contactid"]) ? 0 : dr["contactid"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Cell = Convert.ToString(dr["Cell"]),
                        Email = Convert.ToString(dr["Email"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        name = Convert.ToString(dr["name"]),
                        Title = Convert.ToString(dr["Title"]),
                        EmailTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailTicket"]) ? false : dr["EmailTicket"]),
                        ShutdownAlert = Convert.ToBoolean(DBNull.Value.Equals(dr["ShutdownAlert"]) ? false : dr["ShutdownAlert"]),
                        EmailRecInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecInvoice"]) ? false : dr["EmailRecInvoice"]),
                        EmailRecTestProp = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecTestProp"]) ? false : dr["EmailRecTestProp"]),
                    }
                    );
            }
            return _lstGetLocContactByRolID;
        }


        public void UpdateCustomerContactRecordLog(User objPropUser)
        {
            objDL_User.UpdateCustomerContactRecordLog(objPropUser);
        }

        //API
        public void UpdateCustomerContactRecordLog(UpdateCustomerContactRecordLogParam _UpdateCustomerContactRecordLog, string ConnectionString)
        {
            objDL_User.UpdateCustomerContactRecordLog(_UpdateCustomerContactRecordLog, ConnectionString);
        }

        public DataSet getContactLogByCustomerID(User objPropUser)
        {
            return objDL_User.getContactLogByCustomerID(objPropUser);
        }

        //API
        public List<GetContactLogByCustomerIDViewModel> getContactLogByCustomerID(GetContactLogByCustomerIDParam _GetContactLogByCustomerID, string ConnectionString)
        {
            DataSet ds = objDL_User.getContactLogByCustomerID(_GetContactLogByCustomerID, ConnectionString);

            List<GetContactLogByCustomerIDViewModel> _lstGetContactLogByCustomerID = new List<GetContactLogByCustomerIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetContactLogByCustomerID.Add(
                    new GetContactLogByCustomerIDViewModel()
                    {
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        fUser = Convert.ToString(dr["fUser"]),
                        Field = Convert.ToString(dr["Field"]),
                        NewVal = Convert.ToString(dr["NewVal"]),
                        OldVal = Convert.ToString(dr["OldVal"]),
                        CreatedStamp = Convert.ToDateTime(DBNull.Value.Equals(dr["CreatedStamp"]) ? null : dr["CreatedStamp"]),
                        fDate = Convert.ToDateTime(DBNull.Value.Equals(dr["fDate"]) ? null : dr["fDate"]),
                        fTime = Convert.ToDateTime(DBNull.Value.Equals(dr["fTime"]) ? null : dr["fTime"]),
                    }
                    );
            }
            return _lstGetContactLogByCustomerID;
        }

        public DataSet getContactByRolID(User objPropUser)
        {
            return objDL_User.getContactByRolID(objPropUser);
        }

        //API
        public List<GetContactByRolIDViewModel> getContactByRolID(GetContactByRolIDParam _GetContactByRolID, string ConnectionString)
        {
            DataSet ds = objDL_User.getContactByRolID(_GetContactByRolID, ConnectionString);

            List<GetContactByRolIDViewModel> _lstGetContactByRolID = new List<GetContactByRolIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetContactByRolID.Add(
                    new GetContactByRolIDViewModel()
                    {
                        contactid = Convert.ToInt32(DBNull.Value.Equals(dr["contactid"]) ? 0 : dr["contactid"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Cell = Convert.ToString(dr["Cell"]),
                        Email = Convert.ToString(dr["Email"]),
                        Fax = Convert.ToString(dr["Fax"]),
                        name = Convert.ToString(dr["name"]),
                        Title = Convert.ToString(dr["Title"]),
                        EmailTicket = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailTicket"]) ? false : dr["EmailTicket"]),
                        ShutdownAlert = Convert.ToBoolean(DBNull.Value.Equals(dr["ShutdownAlert"]) ? false : dr["ShutdownAlert"]),
                        EmailRecInvoice = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecInvoice"]) ? false : dr["EmailRecInvoice"]),
                        EmailRecTestProp = Convert.ToBoolean(DBNull.Value.Equals(dr["EmailRecTestProp"]) ? false : dr["EmailRecTestProp"]),
                    }
                    );
            }
            return _lstGetContactByRolID;
        }

        public void UpdateUserGridCustomSettings(User objPropUser, String gridCustomSettings)
        {
            objDL_User.UpdateUserGridCustomSettings(objPropUser, gridCustomSettings);
        }

        public DataSet DeleteUserGridCustomSettings(User objPropUser)
        {
            return objDL_User.DeleteUserGridCustomSettings(objPropUser);
        }

        public DataSet GetDefaultGridCustomSettings(User objPropUser)
        {
            return objDL_User.GetDefaultGridCustomSettings(objPropUser);
        }

        public void UpdateUserExchangeContacts(User objPropUser)
        {
            objDL_User.UpdateUserExchangeContacts(objPropUser);
        }

        public DataSet GetUserExchangeContacts(User objPropUser)
        {
            return objDL_User.GetUserExchangeContacts(objPropUser);
        }
        public DataSet getCategoryActive(User objPropUser)
        {
            return objDL_User.getCategoryActive(objPropUser);
        }

        public void UpdatePassword(User objPropUser)
        {
            objDL_User.UpdatePassword(objPropUser);
        }

        public void UpdateForgotPassword(User objPropUser)
        {
            objDL_User.UpdateForgotPassword(objPropUser);
        }

        public DataSet GetCompanyByName(User objPropUser)
        {
            return objDL_User.GetCompanyByName(objPropUser);
        }

        public DataSet GetUserInfoByUsername(User objPropUser)
        {
            return objDL_User.GetUserInfoByUsername(objPropUser);
        }

        public DataSet GetUserInfoByUsernameAndEmail(User objPropUser)
        {
            return objDL_User.GetUserInfoByUsernameAndEmail(objPropUser);
        }

        public DataSet GetUsersForResetPwAdmin(User objPropUser)
        {
            return objDL_User.GetUsersForResetPwAdmin(objPropUser);
        }

        //GetProjectTeamMemberTitle
        public DataSet GetTeamMemberTitle(User objPropUser, bool isIncludeProjectTeamMemberTitle = false)
        {
            return objDL_User.GetTeamMemberTitle(objPropUser, isIncludeProjectTeamMemberTitle);
        }

        public void AddProjectTeamMemberTitle(TeamMemberTitle objPropUser)
        {
            objDL_User.AddProjectTeamMemberTitle(objPropUser);
        }

        public void UpdateProjectTeamMemberTitle(TeamMemberTitle objPropUser)
        {
            objDL_User.UpdateProjectTeamMemberTitle(objPropUser);
        }

        public void DeleteProjectTeamMemberTitleById(TeamMemberTitle objPropUser)
        {
            objDL_User.DeleteProjectTeamMemberTitleById(objPropUser);
        }

        public DataSet GetTeamMemberTitleSearch(User _objUser)
        {
            return objDL_User.GetTeamMemberTitleSearch(_objUser);
        }

        #region User Role
        public DataSet GetRoleByName(UserRole userRole)
        {
            return objDL_User.GetRoleByName(userRole);
        }

        public DataSet GetRoleByID(UserRole userRole)
        {
            return objDL_User.GetRoleByID(userRole);
        }

        public DataSet GetUsersOfRoleByID(UserRole userRole)
        {
            return objDL_User.GetUsersOfRoleByID(userRole);
        }

        public DataSet GetUsersOfRoleByName(UserRole userRole)
        {
            return objDL_User.GetUsersOfRoleByName(userRole);
        }

        //public DataSet GetRoles(UserRole userRole)
        //{
        //    return objDL_User.GetRoles(userRole);
        //}

        public int AddUpdateUserRole(UserRole userRole, User objPropUser)
        {
            return objDL_User.AddUpdateUserRole(userRole, objPropUser);
        }

        public DataSet GetUsersForRole(User objPropUser)
        {
            return objDL_User.GetUsersForRole(objPropUser);
        }

        public DataSet GetRoleSearch(UserRole objPropUser, bool isIncInactive)
        {
            return objDL_User.GetRoleSearch(objPropUser, isIncInactive);
        }
        #endregion
        public DataSet getFilterCategory(User objPropUser)
        {
            return objDL_User.getFilterCategory(objPropUser);
        }
        public String GetUsersEmailByTypeAndUserID(User objPropUser)
        {
            return objDL_User.GetUsersEmailByTypeAndUserID(objPropUser);
        }
        public DataSet getListEmailByListByTypeAndUserID(String connConfig, String lsStr)
        {
            return objDL_User.getListEmailByListByTypeAndUserID(connConfig, lsStr);
        }
        public DataSet GetEmailFromListRoleID(String connConfig, String lsStr)
        {
            return objDL_User.GetEmailFromListRoleID(connConfig, lsStr);
        }

        //Start-- API Changes : Juily:04/06/2020 --//

        public void UpdateForAPIIntegrationEnable(String connConfig, int ID, int Integration)
        {
            objDL_User.UpdateForAPIIntegrationEnable(connConfig, ID, Integration);
        }

        public DataSet GetAPIIntegrationEnable(String connConfig)
        {
            return objDL_User.GetAPIIntegrationEnable(connConfig);
        }

        //End-- API Changes : Juily:04/06/2020 --//

        public DataSet GetEquipment(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_User.GetEquipment(objPropUser, IsSalesAsigned);
        }

        //API
        public List<GetEquipmentViewModel> GetEquipment(GetEquipmentParam _GetEquipment, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            DataSet ds = objDL_User.GetEquipment(_GetEquipment, ConnectionString, IsSalesAsigned);

            List<GetEquipmentViewModel> _lstGetEquipment = new List<GetEquipmentViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEquipment.Add(
                    new GetEquipmentViewModel()
                    {
                        state = Convert.ToString(dr["state"]),
                        cat = Convert.ToString(dr["cat"]),
                        category = Convert.ToString(dr["category"]),
                        Classification = Convert.ToString(dr["Classification"]),
                        manuf = Convert.ToString(dr["manuf"]),
                        price = Convert.ToDouble(DBNull.Value.Equals(dr["price"]) ? 0 : dr["price"]),
                        last = Convert.ToDateTime(DBNull.Value.Equals(dr["last"]) ? null : dr["last"]),
                        since = Convert.ToDateTime(DBNull.Value.Equals(dr["since"]) ? null : dr["since"]),
                        Install = Convert.ToDateTime(DBNull.Value.Equals(dr["Install"]) ? null : dr["Install"]),
                        id = Convert.ToInt32(DBNull.Value.Equals(dr["id"]) ? 0 : dr["id"]),
                        unit = Convert.ToString(dr["unit"]),
                        type = Convert.ToString(dr["type"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        Status = Convert.ToString(dr["Status"]),
                        shut_down = Convert.ToString(dr["shut_down"]),
                        ShutdownReason = Convert.ToString(dr["ShutdownReason"]),
                        building = Convert.ToString(dr["building"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                        name = Convert.ToString(dr["name"]),
                        locid = Convert.ToString(dr["locid"]),
                        tag = Convert.ToString(dr["tag"]),
                        address = Convert.ToString(dr["address"]),
                        Loc = Convert.ToInt32(DBNull.Value.Equals(dr["Loc"]) ? 0 : dr["Loc"]),
                        unitid = Convert.ToInt32(DBNull.Value.Equals(dr["unitid"]) ? 0 : dr["unitid"]),
                    }
                    );
            }

            return _lstGetEquipment;
        }

        public DataSet GetDefaultTestPricingForEquipment(String connConfig, int elevId, int testTypeId, int priceYear = 0)
        {
            return objDL_User.GetDefaultTestPricingForEquipment(connConfig, elevId, testTypeId, priceYear);
        }

        public DataSet GetUserEmailSignature(User user)
        {
            return objDL_User.GetUserEmailSignature(user);
        }

        public DataSet GetEmailSignatureById(EmailSignature eSignature)
        {
            return objDL_User.GetEmailSignatureById(eSignature);
        }

        public void UpdateEmailSignature(EmailSignature eSignature)
        {
            objDL_User.UpdateEmailSignature(eSignature);
        }

        public int AddEmailSignature(EmailSignature eSignature)
        {
            return objDL_User.AddEmailSignature(eSignature);
        }

        public void DeleteEmailSignature(EmailSignature eSignature)
        {
            objDL_User.DeleteEmailSignature(eSignature);
        }

        public DataSet GetEmailSignatureById(User user)
        {
            return objDL_User.GetUserEmailInfoByUserId(user);
        }

        public string GetDefaultUserEmailSignature(User user)
        {
            return objDL_User.GetDefaultUserEmailSignature(user);
        }

        public DataSet GetOpportunitiesForProjectLinking(User objPropUser)
        {
            return objDL_User.GetOpportunitiesForProjectLinking(objPropUser);
        }

        public void CreateLoginLog(int UserId, string UserName, string Config, string URL, string IP, string MAC)
        {
            objDL_User.CreateLoginLog(UserId, UserName, Config, URL, IP, MAC);
        }
    }
}
