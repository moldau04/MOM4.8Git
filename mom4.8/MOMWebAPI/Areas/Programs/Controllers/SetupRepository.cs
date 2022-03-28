using BusinessEntity.Programs;
using BusinessLayer.Programs;
using System.Collections.Generic;

namespace MOMWebAPI.Areas.Programs.Controllers
{
    public class SetupRepository: ISetupRepository
    {

        public List<ServiceTypeDDLData> GetServiceTypeViewDataModel(string ConnectionString, string SearchBy, string Case) {

           return new BL_ServiceType().GetSetupServiceTypeDropDownValue(ConnectionString, SearchBy, Case);
        }
    }
}
