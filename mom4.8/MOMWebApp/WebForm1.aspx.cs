using BusinessEntity.Projects;
using BusinessEntity.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace MOMWebApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            ProjectListGridParam _ProjectParam = new ProjectListGridParam();             

            _ProjectParam.StartDate = "12/01/2019";
            _ProjectParam.EndDate = "12/01/2019";
            _ProjectParam.Range = 2;
            _ProjectParam.JobType = -1;
            _ProjectParam.EN = 0;
            _ProjectParam.IncludeClose = 0;
            _ProjectParam.UserID = 1;
            _ProjectParam.IsSelesAsigned = 0; 


            List<ProjectListGridModel> _ProjectListGridModel = new List<ProjectListGridModel>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {

                  string APINAME = "APIProjects/GetProjectListData"; 

                  APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ProjectParam);

                 _ProjectListGridModel = (new JavaScriptSerializer()).Deserialize<List<ProjectListGridModel>>(_APIResponse.ResponseData);
            }
            else {

                _ProjectListGridModel = new BusinessLayer.BL_Job().spGetProjectListDataMVC(_ProjectParam, Session["config"].ToString());
            }

            GridView1.DataSource = _ProjectListGridModel;

            GridView1.DataBind();

        }
         

       
    }

    

}