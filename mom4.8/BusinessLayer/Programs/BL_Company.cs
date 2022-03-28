using System.Data;
using DataLayer;
using BusinessEntity;
using BusinessEntity.APModels;
using System.Collections.Generic;
using System;
using BusinessEntity.CustomersModel;

namespace BusinessLayer
{
    public class BL_Company
    {
       DL_Company objDL_Company = new DL_Company();

       public DataSet getCompanies(CompanyOffice objCompany)
       {
           return objDL_Company.getCompanies(objCompany);
       }
       public DataSet getCompanySearch(CompanyOffice objCompany)
       {
           return objDL_Company.getCompanySearch(objCompany);
       }
        public DataSet getOfficeSearch(CompanyOffice objCompany)
        {
            return objDL_Company.getOfficeSearch(objCompany);
        }
        public DataSet getCompanyType(CompanyOffice objCompany)
       {
           return objDL_Company.getCompanyType(objCompany);
       }
       public void AddCompany(CompanyOffice objCompany)
       {
           objDL_Company.AddCompany(objCompany);
       }
       public void AddOffice(CompanyOffice objCompany)
       {
           objDL_Company.AddOffice(objCompany);
       }
       public void UpdateCompany(CompanyOffice objCompany)
       {
           objDL_Company.UpdateCompany(objCompany);
       }
       public void UpdateOffice(CompanyOffice objCompany)
       {
           objDL_Company.UpdateOffice(objCompany);
       }
       public DataSet getCompanyByID(CompanyOffice objCompany)
       {
           return objDL_Company.getCompanyByID(objCompany);
       }
       public DataSet getOfficeByCompanyID(CompanyOffice objCompany)
       {
           return objDL_Company.getOfficeByCompanyID(objCompany);
       }
       public DataSet getOfficeByID(CompanyOffice objCompany)
       {
           return objDL_Company.getOfficeByID(objCompany);
       }
        public DataSet getTerritory(CompanyOffice objCompany)
        {
            return objDL_Company.getTerritory(objCompany);
        }
        public DataSet getRoute(CompanyOffice objCompany)
        {
            return objDL_Company.getRoute(objCompany);
        }
        public DataSet getZone(CompanyOffice objCompany)
        {
            return objDL_Company.getZone(objCompany);
        }
        public DataSet getSTax(CompanyOffice objCompany)
        {
            return objDL_Company.getSTax(objCompany);
        }
        public DataSet getUseTax(CompanyOffice objCompany)
        {
            return objDL_Company.getUseTax(objCompany);
        }
        public DataSet getLocType(CompanyOffice objCompany)
        {
            return objDL_Company.getLocType(objCompany);
        }
        public DataSet getARTerms(CompanyOffice objCompany)
        {
            return objDL_Company.getARTerms(objCompany);
        }
        public DataSet getCompanyByCompanyUserID(CompanyOffice objCompany)
        {
            return objDL_Company.getCompanyByCompanyUserID(objCompany);
        }
        public DataSet getCompanyByUserID(CompanyOffice objCompany)
        {
            return objDL_Company.getCompanyByUserID(objCompany);
        }

        public List<GetCompanyByUserIDViewModel> getCompanyByUserID(GetCompanyByUserIDParam _GetCompanyByUserID, string ConnectionString)
        {
            DataSet ds = objDL_Company.getCompanyByUserID(_GetCompanyByUserID, ConnectionString);

            List<GetCompanyByUserIDViewModel> _lstGetCompanyByUserID = new List<GetCompanyByUserIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetCompanyByUserID.Add(
                    new GetCompanyByUserIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"]) ? 0 : dr["UserID"]),
                        CompanyID = Convert.ToInt32(DBNull.Value.Equals(dr["CompanyID"]) ? 0 : dr["CompanyID"]),
                        OfficeID = Convert.ToInt32(DBNull.Value.Equals(dr["OfficeID"]) ? 0 : dr["OfficeID"]),
                        IsSel = Convert.ToBoolean(DBNull.Value.Equals(dr["IsSel"]) ? false : dr["IsSel"]),
                    });
            }

            return _lstGetCompanyByUserID;
        }

        //API
        public List<CompanyResponse> getCompanyListByUserID(CompanyOffice objCompany)
        {
            List<CompanyResponse> lst = new List<CompanyResponse>();
            DataSet ds = objDL_Company.getCompanyByUserID(objCompany);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(new CompanyResponse()
               {
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = Convert.ToString(dr["Name"]),
                    UserID = Convert.ToInt32(dr["UserID"]),
                    CompanyID = Convert.ToInt32(dr["CompanyID"]),
                    OfficeID = Convert.ToInt32(dr["OfficeID"]),
                    IsSel = Convert.ToString(dr["IsSel"]),
                });
            }
            return lst;
        }

        public DataSet getCompanyUserCoID(CompanyOffice objCompany)
        {
            return objDL_Company.getCompanyUserCoID(objCompany);
        }
        public DataSet getUserDefaultCompany(CompanyOffice objCompany)
        {
            return objDL_Company.getUserDefaultCompany(objCompany);
        }

        public List<UserViewModel> getUserDefaultCompany(getUserDefaultCompanyParam _getUserDefaultCompanyParam, string ConnectionString)
        {
            DataSet ds = objDL_Company.getUserDefaultCompany(_getUserDefaultCompanyParam, ConnectionString);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstUserViewModel.Add(new UserViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Name = Convert.ToString(dr["Name"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["NoOfCompany"]) ? 0 : dr["NoOfCompany"]),
                    });
            }

            return _lstUserViewModel;
        }
        public void UpdateUserCompany(CompanyOffice objCompany)
        {
            objDL_Company.UpdateUserCompany(objCompany);
        }
        public void UserCompanyAccess(CompanyOffice objCompany)
        {
            objDL_Company.UserCompanyAccess(objCompany);
        }
        public void UserCompanyReset(CompanyOffice objCompany)
        {
            objDL_Company.UserCompanyReset(objCompany);
        }
        public DataSet getCompanyByCustomer(CompanyOffice objCompany)
        {
            return objDL_Company.getCompanyByCustomer(objCompany);
        }
        public List<CompanyOfficeViewModel> getCompanyByCustomer(GetCompanyByCustomerParam _GetCompanyByCustomerParam, string ConnectionString)
        {
            DataSet ds = objDL_Company.getCompanyByCustomer(_GetCompanyByCustomerParam, ConnectionString);

            List<CompanyOfficeViewModel> _lstCompanyOfficeViewModel = new List<CompanyOfficeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstCompanyOfficeViewModel.Add(
                    new CompanyOfficeViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserID = Convert.ToInt32(DBNull.Value.Equals(dr["UserID"]) ? 0 : dr["UserID"]),
                        CompanyID = Convert.ToInt32(DBNull.Value.Equals(dr["CompanyID"]) ? 0 : dr["CompanyID"]),
                        OfficeID = Convert.ToInt32(DBNull.Value.Equals(dr["OfficeID"]) ? 0 : dr["OfficeID"]),
                        IsSel = Convert.ToBoolean(DBNull.Value.Equals(dr["IsSel"]) ? 0 : dr["IsSel"]),
                    }
                    );
            }

            return _lstCompanyOfficeViewModel;
        }
        public void DeleteCompanyUserCo(CompanyOffice objCompany)
        {
            objDL_Company.DeleteCompanyUserCo(objCompany);
        }
    }
}
