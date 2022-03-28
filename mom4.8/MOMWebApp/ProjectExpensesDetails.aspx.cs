using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;

public partial class ProjectExpensesDetails : System.Web.UI.Page
{
    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
    protected void Page_Load(object sender, EventArgs e)
    {
        string txt = "Project # " + Request.QueryString["uid"] + " Expenses Details";
        lblHeader.Text = txt;
        Page.Title = txt;

        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        try
        {

            objJob.ConnConfig = Session["config"].ToString();
            objJob.Type = Convert.ToInt16(Request.QueryString["type"]);
            objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
            objJob.Phase = Convert.ToInt32(Request.QueryString["phase"]);

            //objJob.StartDate = 
            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                {
                    objJob.StartDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["s"].ToString()));
                    objJob.EndDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["e"].ToString()));
                }
            }

            DataSet ds = new DataSet();

            ds = objBL_Job.GetJobCostInvoicesByJob(objJob);

            gvJobCostInvoice.DataSource = ds.Tables[0];
            gvJobCostInvoice.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Label lblTotalAmount = (Label)gvJobCostInvoice.FooterRow.FindControl("lblTotalAmount");
                lblTotalAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Amount)", string.Empty));
            }
            if (objJob.Type == 1)
            {
                DataSet dsTicket = new DataSet();

                dsTicket = objBL_Job.GetJobCostTicketsByJob(objJob);

                gvJobCostTicket.DataSource = dsTicket.Tables[0];
                gvJobCostTicket.DataBind();

                if (dsTicket.Tables[0].Rows.Count > 0)
                {
                    DataTable dtTicket = dsTicket.Tables[0];
                    Label lblTotalEstHr = (Label)gvJobCostTicket.FooterRow.FindControl("lblTotalEstHr");
                    Label lblTotalActualHr = (Label)gvJobCostTicket.FooterRow.FindControl("lblTotalActualHr");
                    Label lblTotalLaborExp = (Label)gvJobCostTicket.FooterRow.FindControl("lblTotalLaborExp");
                    Label lblOtherExp = (Label)gvJobCostTicket.FooterRow.FindControl("lblOtherExp");
                    Label lblTotalExp = (Label)gvJobCostTicket.FooterRow.FindControl("lblTotalExp");

                    lblTotalEstHr.Text = string.Format("{0:0.00}", dtTicket.Compute("SUM(Est)", string.Empty));
                    lblTotalActualHr.Text = string.Format("{0:0.00}", dtTicket.Compute("SUM(ActualHr)", string.Empty));
                    lblTotalLaborExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(LaborExp)", string.Empty));
                    lblOtherExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(Expenses)", string.Empty));
                    lblTotalExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(TotalExp)", string.Empty));
                }

                if (objJob.Type.Equals(0))
                {
                    gvJobCostInvoice.Columns[2].Visible = false;
                }

                DataSet dsJe = new DataSet();
                dsJe = objBL_Job.GetJobCostJEByJob(objJob);
                gvJobCostJe.DataSource = dsJe.Tables[0];
                gvJobCostJe.DataBind();

                if (dsJe.Tables[0].Rows.Count > 0)
                {
                    DataTable dtJe = dsJe.Tables[0];
                    Label lblTotalAmount = (Label)gvJobCostJe.FooterRow.FindControl("lblTotalAmount");
                    lblTotalAmount.Text = string.Format("{0:c}", dtJe.Compute("SUM(Amount)", string.Empty));
                }

                DataSet dsPo = new DataSet();
                dsPo = objBL_Job.GetJobCostPOByJob(objJob);
                gvJobCostPO.DataSource = dsPo.Tables[0];
                gvJobCostPO.DataBind();
                if (dsPo.Tables[0].Rows.Count > 0)
                {
                    DataTable dtPo = dsPo.Tables[0];
                    Label lblTotalAmount = (Label)gvJobCostPO.FooterRow.FindControl("lblTotalAmount");
                    lblTotalAmount.Text = string.Format("{0:c}", dtPo.Compute("SUM(Balance)", string.Empty));
                }
            }
            else
            {
                gvJobCostTicket.DataSource = null;
                gvJobCostTicket.DataBind();

                gvJobCostJe.DataSource = null;
                gvJobCostJe.DataBind();

                gvJobCostPO.DataSource = null;
                gvJobCostPO.DataBind();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


}