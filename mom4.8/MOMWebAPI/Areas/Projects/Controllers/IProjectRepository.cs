using BusinessEntity.Projects;
using System;
using System.Collections.Generic;
using System.Data;

namespace MOMWebAPI.Areas.Projects.Controllers
{
    public  interface IProjectRepository
    {
        public List<ProjectListGridModel>  spGetProjectListDataMVC(ProjectListGridParam _ProjectParam, String ConnectionString);
    }
}
