using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ProjectBudgetDetail : System.Web.UI.Page
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

             int TypeID = Convert.ToInt16(Request.QueryString["type"]);

             int jobID = Convert.ToInt32(Request.QueryString["uid"]);

             int PhaseID = Convert.ToInt32(Request.QueryString["Phase"]==null ? "0" : Request.QueryString["Phase"]); // It is not considered now

             string Actual = Request.QueryString["Actual"] == null ? "0" : Request.QueryString["Actual"];
            
           

             string StartDate = "NA";   string EndDate = "NA";

            //objJob.StartDate = 
            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                {
                      StartDate = Request.QueryString["s"].ToString();
                      EndDate   = Request.QueryString["e"].ToString();
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

            
            ds= objBL_Job.GetFinance_Budget_Grid_Popup_ByJob(ConnConfig, jobID, PhaseID, TypeID, StartDate, EndDate);

            if (TypeID == 0)
            {
                /// AR Invoice //////////////////>

                DataTable dtARInvoice = ds.Tables[0];

                RadGridARInvoice.DataSource = dtARInvoice;

                RadGridARInvoice.Visible = dtARInvoice.Rows.Count == 0 ? false : true;


                //  JE

                DataTable dtRevenueJE = ds.Tables[5];

                gvJobCostRevenueJE.DataSource = dtRevenueJE;

                gvJobCostRevenueJE.Visible = dtRevenueJE.Rows.Count == 0 ? false : true;
            }else
            {

                RadGridARInvoice.Visible = gvJobCostRevenueJE.Visible = false;
            }

            /////////////////

            // AP  Invoice ////////////////////>

            DataTable dtAPInvoice = ds.Tables[1];

            gvJobCostInvoice.DataSource = dtAPInvoice;
             
            //DivAPInvoices.Visible = gvJobCostInvoice.Visible = dtAPInvoice.Rows.Count == 0 ? false : true;


            ///  Ticket  ////////////////////////>

            DataTable dtTicket = ds.Tables[2];

            gvJobCostTicket.DataSource = dtTicket;
            
            //DivTickets.Visible= gvJobCostTicket.Visible = dtTicket.Rows.Count == 0 ? false : true;


            // PO

            DataTable dtPO = ds.Tables[3];

            RadGridTotalOnOrder.DataSource = dtPO;
           
            //DivPO.Visible=RadGridTotalOnOrder.Visible = dtPO.Rows.Count == 0 ? false : true;


            //  Receive PO

            DataTable dtRPO = ds.Tables[4];

            RadGrid1ReceivePO.DataSource = dtRPO;
             
            //DivReceivePO.Visible= RadGrid1ReceivePO.Visible = dtRPO.Rows.Count == 0 ? false : true;


            //  JE

            DataTable dtJE = ds.Tables[5];

            gvJobCostJe.DataSource = dtJE;

            //DivJournalEntries.Visible = gvJobCostJe.Visible = dtJE.Rows.Count == 0 ? false : true;

            

            //if (gvJobCostJe.Visible == false & RadGrid1ReceivePO.Visible == false & RadGridTotalOnOrder.Visible == false & gvJobCostTicket.Visible == false & gvJobCostInvoice.Visible == false)
            //{
            //    DivNODATA.Visible = true;
            //}

            String PhaseName = ""; String CodeName = ""; String GroupName = ""; String CodeDesc = ""; string Locname = "";

            DataTable dsinfo =new DataTable();

            dsinfo= ds.Tables[6];
            
            PhaseName = dsinfo.Rows[0]["PhaseName"].ToString();

            CodeName =  dsinfo.Rows[0]["CodeName"].ToString();

            GroupName = dsinfo.Rows[0]["GroupName"].ToString();

            CodeDesc = dsinfo.Rows[0]["CodeDesc"].ToString();

            Locname = dsinfo.Rows[0]["Locname"].ToString();


            decimal decimalMoneyValue = Convert.ToDecimal(Actual);
            string formattedMoneyValue = String.Format("{0:C}", decimalMoneyValue);

            spanProjectNo.InnerText = "Project # " + Request.QueryString["uid"] +" | "+ Locname + " | Group: " + GroupName + " | Code: " + CodeName + " | CodeDesc: " + CodeDesc + " | Type: " + PhaseName  + " | Actual: " + formattedMoneyValue;

            
            string txt = "Project Budget Detail # " + Request.QueryString["uid"] ;

            Page.Title = txt;

            spanProjectName.InnerText = TypeID == 0 ? "Revenue" : "Cost";

            divCost.Visible= TypeID == 0 ? false : true;

            divrevenue.Visible = TypeID == 1 ? false : true;



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