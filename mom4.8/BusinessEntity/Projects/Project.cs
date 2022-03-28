using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Projects
{
    public class ProjectServiceTypeInfo
    {

        public string ServiceTypeValue { get; set; }

        public string ServiceTypeName { get; set; }

        public string BillingName { get; set; }

        public string BillingValue { get; set; }

        public string LaborWageName { get; set; }

        public string LaborWageValue { get; set; }

        public string ExpenseGLName { get; set; }

        public string ExpenseGLValue { get; set; }

        public string InterestGLName { get; set; }

        public string InterestGLValue { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentValue { get; set; }

    }

    public class ProjectServiceTypeDDL
    {
        public string Value { get; set; }

        public string Name { get; set; }
    }
}
