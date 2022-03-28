using BusinessEntity.Projects;
using DataLayer.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Project
{
    public class BL_Project
    {
        
        public DataSet GetProjectServiceType(string ConnectionString, string ServiceType)
        { 
            return new DL_Project().GetProjectServiceType(ConnectionString, ServiceType);
            
        }
        public ProjectServiceTypeInfo GetProjectServiceTypeinfo(string ConnectionString, string ServiceType)
        {

          ProjectServiceTypeInfo _ProjectServiceTypeInfo = new ProjectServiceTypeInfo();


            DataSet ds = new DL_Project().GetProjectServiceTypeinfo( ConnectionString,  ServiceType);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                _ProjectServiceTypeInfo.ServiceTypeName = dr["ServiceTypeName"].ToString();
                _ProjectServiceTypeInfo.ServiceTypeValue = dr["ServiceTypeValue"].ToString();

                _ProjectServiceTypeInfo.BillingName = dr["BillingName"].ToString();
                _ProjectServiceTypeInfo.BillingValue = dr["BillingValue"].ToString();

                _ProjectServiceTypeInfo.ExpenseGLName = dr["ExpenseGLName"].ToString();
                _ProjectServiceTypeInfo.ExpenseGLValue = dr["ExpenseGLValue"].ToString();

                _ProjectServiceTypeInfo.InterestGLName = dr["InterestGLName"].ToString();
                _ProjectServiceTypeInfo.InterestGLValue = dr["InterestGLValue"].ToString();  

                _ProjectServiceTypeInfo.LaborWageName = dr["LaborWageName"].ToString();
                _ProjectServiceTypeInfo.LaborWageValue = dr["LaborWageValue"].ToString();


                break;
            }

            return _ProjectServiceTypeInfo;
        }
    }
}
