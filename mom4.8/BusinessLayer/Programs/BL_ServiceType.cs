using BusinessEntity;
using BusinessEntity.CustomersModel;
using BusinessEntity.Programs;
using DataLayer.Programs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Programs
{
    public class BL_ServiceType
    {
        public List<ServiceTypeDDLData> GetSetupServiceTypeDropDownValue(string ConnectionString, string SearchBy, string Case)
        {
            DataSet ds = new DL_ServiceType().GetSetupServiceTypeDropDownValue(ConnectionString, SearchBy, Case);

            List<ServiceTypeDDLData> _ServiceTypeDDLData = new List<ServiceTypeDDLData>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _ServiceTypeDDLData.Add(new ServiceTypeDDLData()
                {
                    Value = dr["value"].ToString(),
                    Name = dr["NAME"].ToString(),
                });
            }

            return _ServiceTypeDDLData;
        }

        public DataSet GetServiceType(string ConnectionString)
        {
            return new DL_ServiceType().GetServiceType(ConnectionString);
        }



        public DataSet GetActiveServiceType(string ConnectionString)
        {
            return new DL_ServiceType().GetActiveServiceType(ConnectionString);
        }

        //API
        public List<GetActiveServiceTypeViewModel> GetActiveServiceType(GetActiveServiceTypeParam _GetActiveServiceType, string ConnectionString)
        {
            DataSet ds = new DL_ServiceType().GetActiveServiceType(_GetActiveServiceType, ConnectionString);

            List<GetActiveServiceTypeViewModel> _lstGetActiveServiceType = new List<GetActiveServiceTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetActiveServiceType.Add(
                    new GetActiveServiceTypeViewModel()
                    {
                        RT = Convert.ToInt32(DBNull.Value.Equals(dr["RT"]) ? 0 : dr["RT"]),
                        OT = Convert.ToInt32(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                        NT = Convert.ToInt32(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                        DT = Convert.ToInt32(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                        type = Convert.ToString(dr["type"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        remarks = Convert.ToString(dr["remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        InvID = Convert.ToInt32(DBNull.Value.Equals(dr["InvID"]) ? 0 : dr["InvID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusLabel = Convert.ToString(dr["StatusLabel"]),
                    });
            }

            return _lstGetActiveServiceType;
        }

        public DataSet GetActiveServiceTypeContract(string ConnectionString, string LocType, string EditSType, int department = -1, int route = -1)
        {
            return new DL_ServiceType().GetActiveServiceTypeContract(ConnectionString, LocType, EditSType, department, route);
        }

        //API
        public List<GetActiveServiceTypeViewModel> GetActiveServiceTypeContract(GetActiveServiceTypeContractParam _GetActiveServiceTypeContract, string ConnectionString, string LocType, string EditSType, int department = -1, int route = -1)
        {
            DataSet ds = new DL_ServiceType().GetActiveServiceTypeContract(_GetActiveServiceTypeContract, ConnectionString, LocType, EditSType, department = -1, route = -1);

            List<GetActiveServiceTypeViewModel> _lstGetActiveServiceTypeContract = new List<GetActiveServiceTypeViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetActiveServiceTypeContract.Add(
                    new GetActiveServiceTypeViewModel()
                    {
                        RT = Convert.ToInt32(DBNull.Value.Equals(dr["RT"]) ? 0 : dr["RT"]),
                        OT = Convert.ToInt32(DBNull.Value.Equals(dr["OT"]) ? 0 : dr["OT"]),
                        NT = Convert.ToInt32(DBNull.Value.Equals(dr["NT"]) ? 0 : dr["NT"]),
                        DT = Convert.ToInt32(DBNull.Value.Equals(dr["DT"]) ? 0 : dr["DT"]),
                        type = Convert.ToString(dr["type"]),
                        fdesc = Convert.ToString(dr["fdesc"]),
                        remarks = Convert.ToString(dr["remarks"]),
                        Count = Convert.ToInt32(DBNull.Value.Equals(dr["Count"]) ? 0 : dr["Count"]),
                        InvID = Convert.ToInt32(DBNull.Value.Equals(dr["InvID"]) ? 0 : dr["InvID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Status = Convert.ToInt16(DBNull.Value.Equals(dr["Status"]) ? 0 : dr["Status"]),
                        StatusLabel = Convert.ToString(dr["StatusLabel"]),
                    });
            }

            return _lstGetActiveServiceTypeContract;
        }

        public DataSet spGetProjectServiceTypeinfo(string ConnectionString, string ServiceType, int DepartmentID, string LocType, int RoutID)
        {
            return new DL_ServiceType().spGetProjectServiceTypeinfo(ConnectionString, ServiceType, DepartmentID, LocType, RoutID);
        }

        public DataSet spGetLocationServiceTypeinfo(string ConnectionString, string LocType, int RoutID , int LocationID)
        {
            return new DL_ServiceType().spGetLocationServiceTypeinfo(ConnectionString, LocType, RoutID , LocationID);
        }

        //API
        public List<GetLocationServiceTypeinfoViewModel> spGetLocationServiceTypeinfo(spGetLocationServiceTypeinfoParam _GetLocationServiceTypeinfo, string ConnectionString)
        {
            DataSet ds = new DL_ServiceType().spGetLocationServiceTypeinfo(_GetLocationServiceTypeinfo, ConnectionString);

            List<GetLocationServiceTypeinfoViewModel> _lstGetLocationServiceTypeinfo = new List<GetLocationServiceTypeinfoViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetLocationServiceTypeinfo.Add(
                    new GetLocationServiceTypeinfoViewModel()
                    {
                        ServiceTypeName = Convert.ToString(dr["ServiceTypeName"]),
                        ServiceTypeCount = Convert.ToInt32(DBNull.Value.Equals(dr["ServiceTypeCount"]) ? 0 : dr["ServiceTypeCount"]),
                        ProjectPerDepartmentCount = Convert.ToInt32(DBNull.Value.Equals(dr["ProjectPerDepartmentCount"]) ? 0 : dr["ProjectPerDepartmentCount"]),
                        ProjectaregoingtoUpdate = Convert.ToString(dr["ProjectaregoingtoUpdate"]),

                    });
            }

            return _lstGetLocationServiceTypeinfo;
        }


    }
}
