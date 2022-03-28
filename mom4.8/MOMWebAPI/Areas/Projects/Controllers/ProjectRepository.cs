using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using BusinessEntity.Projects;

namespace MOMWebAPI.Areas.Projects.Controllers
{
    public class ProjectRepository : IProjectRepository
    {

        public List<ProjectListGridModel>  spGetProjectListDataMVC(ProjectListGridParam _ProjectParam , String ConnectionString)
        {
           return  new BusinessLayer.BL_Job().spGetProjectListDataMVC( _ProjectParam,  ConnectionString);
             
        }
    }
}
