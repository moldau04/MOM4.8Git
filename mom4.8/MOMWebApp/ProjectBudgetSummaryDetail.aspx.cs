using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class ProjectBudgetSummaryDetail : System.Web.UI.Page
    {

        JobT objJob = new JobT();
        BL_Job objBL_Job = new BL_Job();

        BL_Customer objBL_Customer = new BL_Customer();
        Customer objProp_Customer = new Customer();
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                BindData();

            }
        }





        private void BindData()
        {
            try
            {
            
            string ConnConfig = Session["config"].ToString();

            string Type= Convert.ToString(Request.QueryString["type"]);

            int jobID = Convert.ToInt32(Request.QueryString["uid"]); 

            string Amount = Request.QueryString["Actual"] == null ? "0" : Request.QueryString["Actual"];

            string StartDate = "NA"; string EndDate = "NA";

                //objJob.StartDate = 
                if (Request.QueryString["uid"] != null)
                {
                    if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                    {
                        StartDate = Request.QueryString["s"].ToString();
                        EndDate = Request.QueryString["e"].ToString();
                    }

                }
                if (Session["txtfrmDtValforEditjob"] != null && Session["txttoDtValforEditJob"] != null && Session["ddlDateRangeFieldforEditJob"] != null)
                {
                    if (Session["ddlDateRangeFieldforEditJob"].ToString() == "2" || Session["ddlDateRangeFieldforEditJob"].ToString() == "5")
                    {
                        StartDate = Session["txtfrmDtValforEditjob"].ToString();
                        EndDate = Session["txttoDtValforEditJob"].ToString();
                    }
                }



                DataSet ds = new DataSet();

          
            ds = objBL_Job.spGetProjectBudgetSummaryDetail(ConnConfig, jobID,   Type,   StartDate, EndDate   );

           
                    /// AR Invoice //////////////////>

                    DataTable dtARInvoice = ds.Tables[0];

                    RadGridARInvoice.DataSource = dtARInvoice;
  

                

                /////////////////

                // AP  Invoice ////////////////////>

                DataTable dtAPInvoice = ds.Tables[1];

                gvJobCostInvoice.DataSource = dtAPInvoice;

              


                ///  Ticket  ////////////////////////>

                DataTable dtTicket = ds.Tables[2];

                gvJobCostTicket.DataSource = dtTicket;

            


                // PO

                DataTable dtPO = ds.Tables[3];

                RadGridTotalOnOrder.DataSource = dtPO;

              


                //  Receive PO

                DataTable dtRPO = ds.Tables[4];

                RadGrid1ReceivePO.DataSource = dtRPO;

        


                //  JE

                DataTable dtJE = ds.Tables[5];

                gvJobCostJe.DataSource = dtJE;

                
                String PhaseName = ""; String CodeName = ""; String GroupName = ""; String CodeDesc = ""; string Locname = "";

                DataTable dsinfo = new DataTable();

                dsinfo = ds.Tables[6];

                PhaseName = dsinfo.Rows[0]["PhaseName"].ToString();

                CodeName = dsinfo.Rows[0]["CodeName"].ToString();

                GroupName = dsinfo.Rows[0]["GroupName"].ToString();

                CodeDesc = dsinfo.Rows[0]["CodeDesc"].ToString();

                Locname = dsinfo.Rows[0]["Locname"].ToString();


                decimal decimalMoneyValue = Convert.ToDecimal(Amount);
                string formattedMoneyValue = String.Format("{0:C}", decimalMoneyValue);

                spanProjectNo.InnerText = "Project # " + Request.QueryString["uid"] + " | " + Locname  ;


                string txt = "Project Summary Detail # " + Request.QueryString["uid"];

                Page.Title = txt;
 



            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

        protected void gvJobCostInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }

        protected void gvJobCostTicket_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }

        protected void RadGridTotalOnOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }

        protected void RadGrid1ReceivePO_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }

        protected void gvJobCostJe_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }

        protected void RadGridARInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }

        protected void gvJobCostRevenueJE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindData();
        }
    }