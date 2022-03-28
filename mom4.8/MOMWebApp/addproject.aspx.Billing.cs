//------------------------------------------------------------------------------>
//  
//     Billing tap Code 
//     
//  
//------------------------------------------------------------------------------>

using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using BusinessLayer.Schedule;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Data.OleDb;
using System.Threading;
using Telerik.Web.UI.GridExcelBuilder;
using System.Collections;
using System.Globalization;

public partial class addProject : System.Web.UI.Page
{

    #region Milestones
    private void CreateMilestoneTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquistDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("isUsed", typeof(int));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("ChangeOrder", typeof(int));

        DataRow dr = dt.NewRow();
        dr["Line"] = 1;
        dr["OrderNo"] = 1;
        dr["Amount"] = "0.00";
        dr["GroupId"] = "0";
        dr["isUsed"] = 0;
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Line"] = 2;
        dr1["OrderNo"] = 2;
        dr1["Amount"] = "0.00";
        dr1["GroupId"] = "0";
        dr1["isUsed"] = 0;
        dt.Rows.Add(dr1);

        ViewState["MProjectTemplate"] = dt;
        BindgvMilestones(dt);

    }

    private bool IsExistsMilestone()
    {
        string strItems = hdnMilestone.Value.Trim();
        //try
        //{
        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["txtScope"].ToString().Trim() == string.Empty)
                {
                    return false;
                }
                i++;
                if (dict["hdnID"].ToString().Trim() != string.Empty)
                {
                    if (Convert.ToInt32(dict["hdnID"].ToString()) > 0)
                    {
                        return true;
                    }
                }
            }
        }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
        return false;
    }

    private DataTable GetMilestoneItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("isUsed", typeof(int));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("ChangeOrder", typeof(int));
        try
        {
            string strItems = hdnMilestone.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtScope"].ToString().Trim() != string.Empty)
                    {
                        //    return dt;
                        //}
                        i++;
                        DataRow dr = dt.NewRow();
                        if (dict["hdnID"].ToString().Trim() != string.Empty)
                        {
                            dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                        }
                        else
                        {
                            dr["ID"] = 0;
                        }
                        if (dict["hdnLine"].ToString().Trim() != string.Empty)
                        {
                            dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                        }
                        dr["fDesc"] = dict["txtScope"].ToString().Trim();
                        dr["jcode"] = dict["txtCode"].ToString().Trim();
                        dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                        dr["MilesName"] = dict["txtMilesName"].ToString().Trim();
                        if (dict["txtRequiredBy"].ToString() != string.Empty)
                        {
                            dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                        }
                        if (dict["txtAmount"].ToString() != string.Empty && dict["txtAmount"].ToString().Trim() != "0.00")
                        {
                            dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                        }
                        else
                        {
                            dr["Amount"] = 0;
                        }

                        if (dict["txtQuantity"].ToString() != string.Empty && dict["txtQuantity"].ToString().Trim() != "0.00")
                        {
                            dr["Quantity"] = Convert.ToDouble(dict["txtQuantity"]);
                        }
                        else
                        {
                            dr["Quantity"] = 0;
                        }

                        if (dict["txtPrice"].ToString() != string.Empty && dict["txtPrice"].ToString().Trim() != "0.00")
                        {
                            dr["Price"] = Convert.ToDouble(dict["txtPrice"]);
                        }
                        else
                        {
                            dr["Price"] = 0;
                        }

                        //dr["LeadTime"] = dict["txtLeadTime"].ToString();
                        if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                        {
                            dr["Type"] = dict["hdnType"].ToString();
                            dr["Department"] = dict["txtSType"].ToString();
                        }

                        if (dict["hdnOrderNoMil"].ToString().Trim() != string.Empty)
                        {
                            dr["OrderNo"] = Convert.ToInt32(dict["hdnOrderNoMil"].ToString());
                        }

                        if (dict["txtGroup"].ToString().Trim() != string.Empty)
                        {
                            dr["GroupName"] = (dict["txtGroup"].ToString());
                        }

                        if (dict["hdnGroupID"].ToString().Trim() != string.Empty)
                        {
                            dr["GroupID"] = Convert.ToInt32(dict["hdnGroupID"].ToString());
                        }
                        else { dr["GroupID"] = 0; }

                        if (dict["lblCodeDesc"].ToString().Trim() != string.Empty)
                        {
                            dr["CodeDesc"] = (dict["lblCodeDesc"].ToString());
                        }
                        if (dict["hdnIsUsed"].ToString().Trim() != string.Empty)
                        {
                            dr["isUsed"] = Convert.ToInt32(dict["hdnIsUsed"].ToString());
                        }
                        else { dr["isUsed"] = 0; }

                        if (dict["hdnChangeOrderChk"].ToString().Trim() != string.Empty)
                        {
                            if (dict["hdnChangeOrderChk"].ToString().ToLower() == "true")
                            {
                                dr["ChangeOrder"] = 1;
                            }
                            else
                            {
                                dr["ChangeOrder"] = 0;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }


    private DataTable GetMilestoneGridItems()       //get all items in milestone grid
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("isUsed", typeof(int));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("ChangeOrder", typeof(int));

        try
        {
            string strItems = hdnMilestone.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnLine"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["ID"] = 0;
                    }
                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();

                    dr["jcode"] = dict["txtCode"].ToString().Trim();

                    dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    dr["MilesName"] = dict["txtMilesName"].ToString().Trim();

                    if (dict["txtRequiredBy"].ToString() != string.Empty)
                    {
                        dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                    }
                    if (dict["txtAmount"].ToString() != string.Empty && dict["txtAmount"].ToString().Trim() != "0.00")
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    else
                    {
                        dr["Amount"] = 0;
                    }

                    if (dict["txtQuantity"].ToString() != string.Empty && dict["txtQuantity"].ToString().Trim() != "0.00")
                    {
                        dr["Quantity"] = Convert.ToDouble(dict["txtQuantity"]);
                    }
                    else
                    {
                        dr["Quantity"] = 0;
                    }

                    if (dict["txtPrice"].ToString() != string.Empty && dict["txtPrice"].ToString().Trim() != "0.00")
                    {
                        dr["Price"] = Convert.ToDouble(dict["txtPrice"]);
                    }
                    else
                    {
                        dr["Price"] = 0;
                    }

                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }

                    if (dict["hdnOrderNoMil"].ToString().Trim() != string.Empty)
                    {
                        dr["OrderNo"] = Convert.ToInt32(dict["hdnOrderNoMil"].ToString());
                    }

                    if (dict["txtGroup"].ToString().Trim() != string.Empty)
                    {
                        dr["GroupName"] = (dict["txtGroup"].ToString());
                    }

                    if (dict["hdnGroupID"].ToString().Trim() != string.Empty)
                    {
                        dr["GroupID"] = Convert.ToInt32(dict["hdnGroupID"].ToString());
                    }
                    else { dr["GroupID"] = 0; }

                    if (dict["lblCodeDesc"].ToString().Trim() != string.Empty)
                    {
                        dr["CodeDesc"] = (dict["lblCodeDesc"].ToString());
                    }
                    if (dict["hdnIsUsed"].ToString().Trim() != string.Empty)
                    {
                        dr["isUsed"] = Convert.ToInt32(dict["hdnIsUsed"].ToString());
                    }
                    else { dr["isUsed"] = 0; }

                    if (dict["hdnChangeOrderChk"].ToString().Trim() != string.Empty)
                    {
                        if (dict["hdnChangeOrderChk"].ToString().ToLower() == "true")
                        {
                            dr["ChangeOrder"] = 1;
                        }
                        else
                        {
                            dr["ChangeOrder"] = 0;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }


    #endregion

    protected int DeleteBillingItems(int JobTItemID)
    {

        return new BL_Job().DeleteBillingItem(Session["config"].ToString(), JobTItemID);
    }

    protected void ibDeleteMilestone_Click(object sender, EventArgs e)
    {
        List<int> listItemDelete = new List<int>();
        bool checkDelete = true;
        try
        {
            foreach (GridDataItem gr in gvMilestones.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    Label lblLine = gr.FindControl("lblLine") as Label;

                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                    if (Request.QueryString["uid"] != null)
                    { 

                        int IsExist = DeleteBillingItems(Convert.ToInt32(hdnID.Value));

                        if (IsExist == 1)
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "noty({text: 'Selected job item is in use, it cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                            checkDelete = false;
                        }
                        else
                        {
                            listItemDelete.Add(Convert.ToInt32(lblLine.Text));
                        }
                    }
                    else
                    {
                        listItemDelete.Add(Convert.ToInt32(lblLine.Text));
                    }
                }
            }
            if (checkDelete)
            {
                DeleteGridItem(listItemDelete, "Milestones");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteGridItem(List<int> listItemDelete, string gridName)
    {
        DataTable dt = new DataTable();
        if (gridName == "Bom")
        {
            dt = GetBOMGridItems();
        }
        else if (gridName == "Milestones")
        {
            dt = GetMilestoneGridItems();
        }
        List<DataRow> rowsToDelete = new List<DataRow>();

        foreach (DataRow row in dt.Rows)
        {

            if (listItemDelete.Contains(Int32.Parse(row["Line"].ToString())))
            {
                rowsToDelete.Add(row);
            }

        }
        foreach (DataRow row in rowsToDelete)
        {
            dt.Rows.Remove(row);
        }
        if (gridName == "Bom")
        {
            BindgvBOM(dt);
        }
        if (gridName == "Milestones")
        {
            BindgvMilestones(dt);
        }
    }



    protected void gvMilestones_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddMilestone"))
        {
            DataTable dt = GetMilestoneGridItems();

            string maxvalueLine = "";
            string maxValueOrderNo = "";

            if (dt.Rows.Count == 0)
            {
                maxvalueLine = "1";
                maxValueOrderNo = "1";
            }
            else
            {
                maxvalueLine = dt.AsEnumerable().Max(row => row["Line"]).ToString();
                if (string.IsNullOrEmpty(dt.Rows[0]["OrderNo"].ToString()))
                {
                    maxValueOrderNo = "1";
                }
                else
                {
                    maxValueOrderNo = dt.AsEnumerable().Max(row => row["OrderNo"]).ToString();
                }
            }
            Int32 _line = Convert.ToInt32(maxvalueLine) + 1;

            Int32 _orderNo = Convert.ToInt32(maxValueOrderNo) + 1;

            
                DataRow dr = dt.NewRow();
                dr["OrderNo"] = _orderNo;
                dr["Line"] = _line;
                dr["Amount"] = "0.00";
            
            if (Request.QueryString["uid"] != null)
            {
                dr["ID"] = AddNewBomLine(_orderNo, 0);
            }


                dt.Rows.Add(dr);
                
             
 
            BindgvMilestones(dt);

        }
    }


    protected void gvMilestones_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            if (Session["type"].ToString() != "am")
            {
                string MilestonesPermission = ds.Rows[0]["MilestonesPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["MilestonesPermission"].ToString();

                string stEditMilestones = MilestonesPermission.Length < 2 ? "Y" : MilestonesPermission.Substring(1, 1);

                if (stEditMilestones == "N")
                {
                    DropDownList ddlType = (DropDownList)e.Item.FindControl("ddlType");
                    TextBox txtCode = (TextBox)e.Item.FindControl("txtCode");
                    TextBox txtSType = (TextBox)e.Item.FindControl("txtSType");
                    TextBox txtName = (TextBox)e.Item.FindControl("txtMilesName");
                    TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmount");
                    TextBox txtRequiredBy = (TextBox)e.Item.FindControl("txtRequiredBy");
                    TextBox txtScope = (TextBox)e.Item.FindControl("txtScope");

                    txtScope.ReadOnly = txtSType.ReadOnly = txtName.ReadOnly = txtAmount.ReadOnly = txtRequiredBy.ReadOnly = txtCode.ReadOnly = true;

                    //ddlType.Enabled = false;

                    e.Item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                }

            }
        }
    }

}
