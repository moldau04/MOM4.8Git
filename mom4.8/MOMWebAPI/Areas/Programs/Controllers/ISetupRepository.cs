using BusinessEntity.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Programs.Controllers
{
    public interface ISetupRepository
    {

        public List<ServiceTypeDDLData> GetServiceTypeViewDataModel(string ConnectionString, string SearchBy, string Case);

    }
}
