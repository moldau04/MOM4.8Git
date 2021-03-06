using BusinessEntity;
using BusinessEntity.APModels;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessLayer
{
    public class BL_BankAccount
    {
        DL_BankAccount _objDLBank = new DL_BankAccount();

        public int AddRol(Rol objRol)
        {
            return _objDLBank.AddRol(objRol);
        }
        public int AddRol(AddRolParam _AddRolParam, string ConnectionString)
        {
            int Rol = _objDLBank.AddRol(_AddRolParam, ConnectionString);
            return Rol;
        }
        public void AddBank(Bank objBank)
        {
            _objDLBank.AddBank(objBank);
        }
        public void UpdateRol(Rol objRol)
        {
            _objDLBank.UpdateRol(objRol);
        }
        public void UpdateRol(UpdateRolParam _UpdateRolParam, string ConnectionString)
        {
            _objDLBank.UpdateRol(_UpdateRolParam, ConnectionString);
        }
        public void UpdateRoles(Rol objRol)
        {
            _objDLBank.UpdateRoles(objRol);
        }
        public void UpdateRoles(UpdateRolesParam _UpdateRolesParam, string ConnectionString)
        {
            _objDLBank.UpdateRoles(_UpdateRolesParam, ConnectionString);
        }
        public void UpdateBank(Bank objBank)
        {
            _objDLBank.UpdateBank(objBank);
        }
        public void UpdateNextCheck(Bank _objBank)
        {
            _objDLBank.UpdateNextCheck(_objBank);
        }
        public void UpdateNextCheck(UpdateNextCheckParam _UpdateNextCheckParam, string ConnectionString)
        {
            _objDLBank.UpdateNextCheck(_UpdateNextCheckParam, ConnectionString);
        }
        public DataSet GetStates(State _objState)
        {
            return _objDLBank.GetStates(_objState);
        }
        public List<StateViewModel> GetStates(GetStatesParam _GetStatesParam, string ConnectionString)
        {
            DataSet ds = _objDLBank.GetStates(_GetStatesParam, ConnectionString);

            List<StateViewModel> _lstStateViewModel = new List<StateViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstStateViewModel.Add(
                    new StateViewModel()
                    {
                        Name = Convert.ToString(dr["Name"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Country = Convert.ToString(dr["Country"]),
                    }
                    );
            }

            return _lstStateViewModel;
        }
        public DataSet GetRolByID(Rol objRol)
        {
            return _objDLBank.GetRolByID(objRol);
        }
        public DataSet GetBankByChart(Bank objBank)
        {
            return _objDLBank.GetBankByChart(objBank);
        }
        public DataSet IsExistBankAcct(Bank objBank)
        {
            return _objDLBank.IsExistBankAcct(objBank);
        }
        public DataSet spGetBankACH(Bank objBank)
        {
            return _objDLBank.spGetBankACH(objBank);
        }

        public DataSet spGetACHForCheck(Bank objBank)
        {
            return _objDLBank.spGetACHForCheck(objBank);
        }
        public DataSet GetAllBankNames(Bank objBank)
        {
            return _objDLBank.GetAllBankNames(objBank);
        }

        public List<GetAllBankNamesViewModel> GetAllBankNames(GetAllBankNamesParam objGetAllBankNamesParam, string ConnectionString)
        {
            DataSet ds = _objDLBank.GetAllBankNames(objGetAllBankNamesParam, ConnectionString);

            List<GetAllBankNamesViewModel> _lstGetAllBankNamesViewModel = new List<GetAllBankNamesViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetAllBankNamesViewModel.Add(
                    new GetAllBankNamesViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                    }
                    );
            }

            return _lstGetAllBankNamesViewModel;
        }


        public DataSet GetAllBankNamesByCompany(Bank objBank)
        {
            return _objDLBank.GetAllBankNamesByCompany(objBank);
        }

        public List<BankViewModel> GetAllBankNamesByCompany(GetAllBankNamesByCompanyParam _GetAllBankNamesByCompanyParam, string ConnectionString)
        {
            DataSet ds = _objDLBank.GetAllBankNamesByCompany(_GetAllBankNamesByCompanyParam, ConnectionString);
            List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstBankViewModel.Add(
                    new BankViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        EN = Convert.ToInt32(DBNull.Value.Equals(dr["EN"]) ? 0 : dr["EN"]),
                        Company = Convert.ToString(dr["Company"]),
                    }
                    );
            }

            return _lstBankViewModel;
        }
        public DataSet GetBankByID(Bank objBank)
        {
            return _objDLBank.GetBankByID(objBank);
        }
        public List<GetBankByIDViewModel> GetBankByID(GetBankByIDParam _GetBankByIDParam, string ConnectionString)
        {
            DataSet ds = _objDLBank.GetBankByID(_GetBankByIDParam, ConnectionString);
            List<GetBankByIDViewModel> _lstGetBankByID = new List<GetBankByIDViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetBankByID.Add(
                    new GetBankByIDViewModel()
                    {
                        ID = Convert.ToInt32(DBNull.Value.Equals(dr["ID"]) ? 0 : dr["ID"]),
                        fDesc = Convert.ToString(dr["fDesc"]),
                        Rol = Convert.ToInt32(DBNull.Value.Equals(dr["Rol"]) ? 0 : dr["Rol"]),
                        NBranch = Convert.ToString(dr["NBranch"]),
                        NAcct = Convert.ToString(dr["NAcct"]),
                        NRoute = Convert.ToString(dr["NRoute"]),
                        NextC = Convert.ToInt64(DBNull.Value.Equals(dr["NextC"]) ? 0 : dr["NextC"]),
                        NextD = Convert.ToInt32(DBNull.Value.Equals(dr["NextD"]) ? 0 : dr["NextD"]),
                        NextE = Convert.ToInt32(DBNull.Value.Equals(dr["NextE"]) ? 0 : dr["NextE"]),
                        Rate = Convert.ToDouble(DBNull.Value.Equals(dr["Rate"]) ? 0 : dr["Rate"]),
                        CLimit = Convert.ToDouble(DBNull.Value.Equals(dr["CLimit"]) ? 0 : dr["CLimit"]),
                        Warn = Convert.ToInt16(DBNull.Value.Equals(dr["Warn"]) ? 0 : dr["Warn"]),
                        Recon = Convert.ToDouble(DBNull.Value.Equals(dr["Recon"]) ? 0 : dr["Recon"]),
                        Balance = Convert.ToDouble(DBNull.Value.Equals(dr["Balance"]) ? 0 : dr["Balance"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        InUse = Convert.ToInt16(DBNull.Value.Equals(dr["InUse"]) ? 0 : dr["InUse"]),
                        ACHFileHeaderStringA = Convert.ToString(dr["ACHFileHeaderStringA"]),
                        ACHFileHeaderStringC = Convert.ToString(dr["ACHFileHeaderStringC"]),
                        ACHCompanyHeaderString1 = Convert.ToString(dr["ACHCompanyHeaderString1"]),
                        ACHCompanyHeaderString2 = Convert.ToString(dr["ACHCompanyHeaderString2"]),
                        ACHBatchControlString1 = Convert.ToString(dr["ACHBatchControlString1"]),
                        ACHBatchControlString2 = Convert.ToString(dr["ACHBatchControlString2"]),
                        ACHBatchControlString3 = Convert.ToString(dr["ACHBatchControlString3"]),
                        ACHFileControlString1 = Convert.ToString(dr["ACHFileControlString1"]),
                        Chart = Convert.ToInt32(DBNull.Value.Equals(dr["Chart"]) ? 0 : dr["Chart"]),
                        LastReconDate = Convert.ToDateTime(DBNull.Value.Equals(dr["LastReconDate"]) ? null : dr["LastReconDate"]),
                        BankBalance = Convert.ToDouble(DBNull.Value.Equals(dr["BankBalance"]) ? 0 : dr["BankBalance"]),
                        NextCash = Convert.ToInt64(DBNull.Value.Equals(dr["NextCash"]) ? 0 : dr["NextCash"]),
                        NextWire = Convert.ToInt64(DBNull.Value.Equals(dr["NextWire"]) ? 0 : dr["NextWire"]),
                        NextACH = Convert.ToInt64(DBNull.Value.Equals(dr["NextACH"]) ? 0 : dr["NextACH"]),
                        NextCC = Convert.ToInt64(DBNull.Value.Equals(dr["NextCC"]) ? 0 : dr["NextCC"]),
                        BankType = Convert.ToInt32(DBNull.Value.Equals(dr["BankType"]) ? 0 : dr["BankType"]),
                        ChartID = Convert.ToInt32(DBNull.Value.Equals(dr["ChartID"]) ? 0 : dr["ChartID"]),
                        APACHCompanyID = Convert.ToString(dr["APACHCompanyID"]),
                        APImmediateOrigin = Convert.ToString(dr["APImmediateOrigin"]),
                        ACHFileHeaderStringB = Convert.ToString(dr["ACHFileHeaderStringB"]),
                    }
                    );
            }

            return _lstGetBankByID;
        }
        public void UpdateBankBalance(Bank _objBank)
        {
            _objDLBank.UpdateBankBalance(_objBank);
        }
        public void UpdateBankBalanceNcheck(Bank _objBank)
        {
            _objDLBank.UpdateBankBalanceNcheck(_objBank);
        }
        public void UpdateBankRecon(Bank _objBank)
        {
            _objDLBank.UpdateBankRecon(_objBank);
        }
        public int GetChartByBank(Bank _objBank)
        {
            return _objDLBank.GetChartByBank(_objBank);
        }
        public int GetBankIDByChart(Bank _objBank)
        {
            return _objDLBank.GetBankIDByChart(_objBank);
        }
        public DataSet GetBankRolByID(Bank objBank)
        {
            return _objDLBank.GetBankRolByID(objBank);
        }
        public void DeleteRolByID(Rol objRol)
        {
            _objDLBank.DeleteRolByID(objRol);
        }
        public void DeleteRolByID(DeleteRolByIDParam _DeleteRolByIDParam, string connectionString)
        {
            _objDLBank.DeleteRolByID(_DeleteRolByIDParam, connectionString);
        }
        public void BankRecon(Bank objBank)
        {
            _objDLBank.BankRecon(objBank);
        }
        //Return BankReconID For Cleared Item
        //Created by Prateek 17-03-2021
        public int BankReconID(Bank objBank)
        {
            int Id = _objDLBank.BankReconId(objBank);
            return Id;
        }
        public void StoreBankRecon(Bank objBank)
        {
            _objDLBank.StoreBankRecon(objBank);
        }
        public DataSet GetStoredBankRecon(Bank objBank)
        {
            return _objDLBank.GetStoredBankRecon(objBank);
        }
        public DataSet GetBankDetailByDate(Bank objBank)
        {
            return _objDLBank.GetBankDetailByDate(objBank);
        }
    }
}
