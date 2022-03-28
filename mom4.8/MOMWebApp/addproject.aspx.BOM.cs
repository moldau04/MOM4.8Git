//------------------------------------------------------------------------------>
//  
//    BOM tap Code 
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


public partial class addProject : System.Web.UI.Page
{

    protected void gvBOM_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

                if (totalCount == 0) totalCount = 1000;
                GeneralFunctions obj = new GeneralFunctions();
                var sizes = obj.TelerikPageSize(totalCount);
                dropDown.Items.Clear();

                foreach (var size in sizes)
                {
                    var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                    cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                    if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                    dropDown.Items.Add(cboItem);
                }
            }
        }
        catch { }
    }



    /// <summary>
    /// /////////////////////////// BOM tap 
    /// </summary>
    /// 
    protected void gvBOM_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            #region BOM Type
            HiddenField hdnBType = (HiddenField)e.Item.FindControl("hdnBType");

            DropDownList drpBType = (DropDownList)e.Item.FindControl("ddlBType");

            drpBType.DataTextField = "Type";

            drpBType.DataValueField = "ID";

            drpBType.DataSource = dtBomType;

            drpBType.DataBind();

            drpBType.Items.Insert(0, new ListItem("Select Type", "-1"));

            drpBType.Items.Insert(1, new ListItem(" < Add New > ", "0"));

            drpBType.SelectedValue = hdnBType.Value;

            #endregion

            HiddenField hdnLabType = (HiddenField)e.Item.FindControl("hdntxtLabItem");

            TextBox drpLabType = (TextBox)e.Item.FindControl("txtLabItem");



        }


    }

    protected void gvBOM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = GetBOMGridItems();
        BindgvBOM(dt, false);
    }

    #region BOM


    protected void gvBOM_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddProject"))
        {
            DataTable dt = GetBOMGridItems();

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

            for (int j = 0; j < 1; j++)
            {
                DataRow dr = dt.NewRow();
                dr["OrderNo"] = _orderNo;
                dr["Line"] = _line;
                dr["QtyReq"] = "0.00";
                dr["BudgetUnit"] = "0.00";
                dr["MatMod"] = "0.00";
                dr["BudgetExt"] = "0.00";
                dr["LabHours"] = "0.00";
                dr["LabRate"] = "0.00";
                dr["LabMod"] = "0.00";
                dr["LabExt"] = "0.00";
                dr["TotalExt"] = "0.00";
                dr["TargetHours"] = "0.00";
                dr["BudgetHours"] = "0.00";
                dr["GroupId"] = "0";
                dr["Code"] = "100";


               int gvBOMPageSize = gvBOM.PageSize;

              int  gvBOMPageIndex = gvBOM.CurrentPageIndex;

                if (Request.QueryString["uid"] != null)
                {
                    dr["JobTItemID"] =   AddNewBomLine(((gvBOMPageIndex * gvBOMPageSize) + 1 ), 1); 
                }

                dt.Rows.InsertAt(dr, ((gvBOMPageIndex * gvBOMPageSize)));

                _line = _line + 1;

                _orderNo = ((gvBOMPageIndex * gvBOMPageSize) + 1) ;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["OrderNo"] = i + 1;
            }

            ViewState["TempBOM"] = dt; 

            BindgvBOM(dt);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "funMaterialLabor", "funMaterialLabor()", true);

        }
    }

    protected int AddNewBomLine(int OrderNo , int type) {
        int job = Convert.ToInt32(Request.QueryString["uid"].ToString()); 
        int TemplateID= Convert.ToInt32(ddlTemplate.SelectedValue);
        return new BL_Job().AddJobtItemNew(Session["config"].ToString(), job, OrderNo, TemplateID, type, 0);
    }

    protected void ibDeleteBom_Click(object sender, EventArgs e)
    {
        try
        {

            List<int> listItemDelete = new List<int>();
          
            foreach (GridDataItem gr in gvBOM.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    Label lblLine = gr.FindControl("lblLine") as Label;
                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                    if (Request.QueryString["uid"] != null)
                    {

                        int IsExist  = DeleteBomItems(Convert.ToInt32(hdnID.Value)); 
                         
                        if (IsExist == 1)
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "noty({text: 'Selected job item is in use, it cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                           
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

            if (listItemDelete.Count > 0)
            {
                DeleteGridItem(listItemDelete, "Bom");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    protected int DeleteBomItems(int JobTItemID)
    {

        return new BL_Job().DeleteBOMItem(Session["config"].ToString(), JobTItemID);
    }
    
    protected void RadMenuBomGrid_ItemClick(object sender, RadMenuEventArgs e)
    {
        DataTable dt = GetBOMGridItems();
        switch (e.Item.Text)
        {
            case "Add Row Above":
                AddNewRowGrid(dt, "above", "Bom");
                break;

            case "Add Row Below":
                AddNewRowGrid(dt, "below", "Bom");
                break;
        }


    }

    private void AddNewRowGrid(DataTable dt, string position, string gridName)
    {
        Int32 _orderNo = 0;

        DataRow dr = dt.NewRow();

        string maxvalue = dt.AsEnumerable().Max(row => row["Line"]).ToString();

        Int32 _line = Convert.ToInt32(maxvalue) + 1;

        if (gridName == "Bom")
        {
            _orderNo = Int32.Parse(radGridClickedRowIndex.Value);
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["QtyReq"] = "0.00";
            dr["BudgetUnit"] = "0.00";
            dr["MatMod"] = "0.00";
            dr["BudgetExt"] = "0.00";
            dr["LabHours"] = "0.00";
            dr["LabRate"] = "0.00";
            dr["LabMod"] = "0.00";
            dr["LabExt"] = "0.00";
            dr["TotalExt"] = "0.00";

            if (Request.QueryString["uid"] != null)
            {
                dr["JobTItemID"] = AddNewBomLine(_orderNo , 1);
            }

        }

        else if (gridName == "Milestones")
        {
            _orderNo = Int32.Parse(radMilGridClickedRowIndex.Value);
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["Amount"] = "0.00";

            if (Request.QueryString["uid"] != null)
            {
                dr["ID"] = AddNewBomLine(_orderNo, 0);
            }
        }

        if (position == "above")
        {
            dt.Rows.InsertAt(dr, _orderNo);
        }

        else if (position == "below")
        {
            dt.Rows.InsertAt(dr, _orderNo + 1);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["OrderNo"] = i + 1;
        }

        if (gridName == "Bom")
        {
            ViewState["TempBOM"] = dt;
            BindgvBOM(dt);
        }

        else if (gridName == "Milestones")
        {
            ViewState["TempMilestone"] = dt;
            BindgvMilestones(dt);

        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "funMaterialLabor", "funMaterialLabor()", true);

    }

    
    private DataTable GetBOMGridItems()
    {
        DataTable dt = GetemptydBOMdatatable(); 

        DataTable dtemp = (DataTable)ViewState["TempBOM"];


        double budgetExt = 0;
        double _qtyReq = 0;
        double labExt = 0;
        try
        {

            foreach (GridDataItem item in gvBOM.Items)
            {
                HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
                HiddenField hdnLine = (HiddenField)item.FindControl("hdnLine");
                //bom items




                DropDownList ddlBType = (DropDownList)item.FindControl("ddlBType");

                TextBox txtBType = (TextBox)item.FindControl("txtBType");


                TextBox txtScope = (TextBox)item.FindControl("txtScope");
              

                HiddenField hdntxtLabItem1 = (HiddenField)item.FindControl("hdntxtLabItem");

                TextBox txtLabItem = (TextBox)item.FindControl("txtLabItem");

                HiddenField hdnddlMatItemId = (HiddenField)item.FindControl("hdnMatItem");

                TextBox txtQtyReq = (TextBox)item.FindControl("txtQtyReq");

                TextBox txtUM = (TextBox)item.FindControl("txtUM");

                TextBox txtBudgetUnit = (TextBox)item.FindControl("txtBudgetUnit");

                TextBox txtMatMod = (TextBox)item.FindControl("txtMatMod");

                TextBox txtLabMod = (TextBox)item.FindControl("txtLabMod");

                TextBox txtVendor = (TextBox)item.FindControl("txtVendor");

                HiddenField hdnVendorId = (HiddenField)item.FindControl("hdnVendorId");

                TextBox txtCode = (TextBox)item.FindControl("txtCode");

                TextBox txtHours = (TextBox)item.FindControl("txtHours");

                TextBox txtLabRate = (TextBox)item.FindControl("txtLabRate");

                TextBox txtSDate = (TextBox)item.FindControl("txtSDate");

                TextBox txtddlMatItem = (TextBox)item.FindControl("txtMatItem");

                HiddenField hdnOrderNo = (HiddenField)item.FindControl("hdnOrderNo");

                TextBox txtGroup = (TextBox)item.FindControl("txtGroup");

                HiddenField hdnGroupID = (HiddenField)item.FindControl("hdnGroupID");

                TextBox lblCodeDesc = (TextBox)item.FindControl("lblCodeDesc");

                TextBox txtTargetHours = (TextBox)item.FindControl("txtTargetHours");

                TextBox txtBudgetHours = (TextBox)item.FindControl("txtBudgetHours");

                //HiddenField hdnChangeOrderChk = (HiddenField)item.FindControl("hdnChangeOrderChk"); 

                CheckBox chkChangeOrder = (CheckBox)item.FindControl("chkChangeOrder");

                if (txtScope.Text.Trim() != string.Empty && ddlBType.SelectedValue != "0" && ddlBType.SelectedValue != "Select Type")
                {
                    _qtyReq = 0; budgetExt = 0; labExt = 0;

                    DataRow dr = dt.NewRow();


                    if (hdnID.Value.Trim() != string.Empty) dr["JobTItemID"] = Convert.ToInt32(hdnID.Value);


                    if (hdnLine.Value.Trim() == string.Empty) return dt;


                    if (hdnLine.Value.Trim() != string.Empty) dr["Line"] = Convert.ToInt32(hdnLine.Value);


                    dr["fDesc"] = txtScope.Text;

                    dr["Code"] = txtCode.Text;

                    dr["BTypes"] = txtBType.Text;

                    if (ddlBType.SelectedValue != "0") dr["BType"] = Convert.ToInt32(ddlBType.SelectedValue);

                    int hdntxtLabItem = 0; int.TryParse(hdntxtLabItem1.Value.ToString(), out hdntxtLabItem);

                    if (!hdntxtLabItem.Equals(0)) dr["LabItem"] = hdntxtLabItem;

                    if ((txtLabItem.Text.ToString().Trim() != string.Empty)) dr["txtLabItem"] = txtLabItem.Text;




                    int _hdnddlMatItemId = 0;

                    int.TryParse(hdnddlMatItemId.Value, out _hdnddlMatItemId);

                    if (!_hdnddlMatItemId.Equals(0)) dr["MatItem"] = _hdnddlMatItemId;


                    if (txtQtyReq.Text.Trim() != string.Empty && txtQtyReq.Text.Trim() != "0.00") dr["QtyReq"] = Convert.ToDouble(txtQtyReq.Text.Trim());
                    else dr["QtyReq"] = 0;

                    if (txtUM.Text != string.Empty) dr["UM"] = txtUM.Text;



                    if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && txtBudgetUnit.Text.Trim() != "0.00") dr["BudgetUnit"] = Convert.ToDouble(txtBudgetUnit.Text);

                    else dr["BudgetUnit"] = 0;



                    if (txtBudgetUnit.Text != string.Empty && !string.IsNullOrEmpty(txtQtyReq.Text) && txtBudgetUnit.Text != "0.00" && txtQtyReq.Text != "0.00")
                    {
                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);

                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                        }
                        budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);

                        dr["BudgetExt"] = budgetExt;
                    }
                    else
                    {
                        dr["BudgetExt"] = 0;
                    }



                    if (txtMatMod.Text.Trim() != string.Empty && txtMatMod.Text.Trim() != "0.00")
                    {
                        dr["MatMod"] = Convert.ToDouble(txtMatMod.Text);
                    }
                    else
                    {
                        dr["MatMod"] = 0;
                    }



                    if (txtLabMod.Text.Trim() != string.Empty && txtLabMod.Text.Trim() != "0.00") dr["LabMod"] = Convert.ToDouble(txtLabMod.Text);

                    else dr["LabMod"] = 0;




                    if (hdnVendorId.Value.Trim() != string.Empty && txtVendor.Text.Trim() != string.Empty) dr["VendorId"] = Convert.ToInt32(hdnVendorId.Value);


                    if (txtVendor.Text.Trim() != string.Empty) dr["Vendor"] = txtVendor.Text;




                    if (txtHours.Text != string.Empty && txtHours.Text != "0.00") dr["LabHours"] = Convert.ToDouble(txtHours.Text);

                    else dr["LabHours"] = 0;



                    if (!string.IsNullOrEmpty(txtLabRate.Text) && txtLabRate.Text != "0.00") dr["LabRate"] = txtLabRate.Text;

                    else dr["LabRate"] = 0;


                    if (!string.IsNullOrEmpty(txtLabRate.Text) && !string.IsNullOrEmpty(txtHours.Text)
                            && txtLabRate.Text.Trim() != "0.00" && txtHours.Text.Trim() != "0.00")
                    {
                        labExt = Convert.ToDouble(txtLabRate.Text);

                        if (txtHours.Text.Trim() != string.Empty) labExt = labExt * Convert.ToDouble(txtHours.Text.Trim());
                        dr["LabExt"] = labExt;
                    }

                    else dr["LabExt"] = 0;

                    dr["TotalExt"] = labExt + budgetExt;



                    if (txtSDate.Text != string.Empty) { dr["SDate"] = Convert.ToDateTime(txtSDate.Text); }


                    dr["MatDesc"] = (txtddlMatItem.Text);


                    if (hdnOrderNo.Value != string.Empty) dr["OrderNo"] = Convert.ToInt32(hdnOrderNo.Value);


                    if (txtGroup.Text != string.Empty) dr["GroupName"] = (txtGroup.Text);


                    if (hdnGroupID.Value != string.Empty) dr["GroupID"] = Convert.ToInt32(hdnGroupID.Value); else { dr["GroupID"] = 0; }



                    if (lblCodeDesc.Text != string.Empty) dr["CodeDesc"] = lblCodeDesc.Text;

                    if (txtTargetHours.Text.Trim() != string.Empty && txtTargetHours.Text.Trim() != "0")
                    {
                        dr["TargetHours"] = Convert.ToDouble(txtTargetHours.Text);
                    }
                    else
                    {
                        dr["TargetHours"] = 0;
                    }


                    if (txtBudgetHours.Text.Trim() != string.Empty && txtBudgetHours.Text.Trim() != "0")
                    {
                        dr["BudgetHours"] = Convert.ToDouble(txtBudgetHours.Text);
                    }
                    else
                    {
                        dr["BudgetHours"] = 0;
                    }

                    //if (hdnChangeOrderChk.Value != string.Empty) dr["ChangeOrder"] = Convert.ToInt32(hdnChangeOrderChk.Value); else { dr["ChangeOrder"] = 0; }
                    dr["ChangeOrder"] = Convert.ToInt32(chkChangeOrder.Checked);

                    dt.Rows.Add(dr);


                }
            }



            dt.AcceptChanges();


            foreach (DataRow drtemp in dtemp.Rows)
            {
                bool IsNotExists = true;

                foreach (DataRow dtdr in dt.Rows)
                {
                    if (drtemp["JobTItemID"].ToString() == dtdr["JobTItemID"].ToString())
                    {
                        IsNotExists = false;
                    }

                }


                if (IsNotExists)
                {
                    DataRow dr = dt.NewRow();


                    if (drtemp["JobTItemID"].ToString() != string.Empty) dr["JobTItemID"] = Convert.ToInt32(drtemp["JobTItemID"].ToString());

                    if (drtemp["Line"].ToString() != string.Empty) dr["Line"] = Convert.ToInt32(drtemp["Line"].ToString());

                    if (drtemp["Code"].ToString() != string.Empty) dr["Code"] = (drtemp["Code"].ToString());

                    if (drtemp["fDesc"].ToString() != string.Empty) dr["fDesc"] = (drtemp["fDesc"].ToString());
                     

                    if (drtemp["BType"].ToString() != string.Empty) dr["BType"] = Convert.ToInt32(drtemp["BType"].ToString());

                    if (drtemp["BTypes"].ToString() != string.Empty) dr["BTypes"] = (drtemp["BTypes"].ToString());

                    if (drtemp["LabItem"].ToString() != string.Empty) dr["LabItem"] = Convert.ToInt32(drtemp["LabItem"].ToString());

                    if (drtemp["txtLabItem"].ToString() != string.Empty) dr["txtLabItem"] = (drtemp["txtLabItem"].ToString());

                    if (drtemp["MatItem"].ToString() != string.Empty) dr["MatItem"] = Convert.ToInt32(drtemp["MatItem"].ToString());

                    if (drtemp["QtyReq"].ToString() != string.Empty) dr["QtyReq"] = Convert.ToDouble(drtemp["QtyReq"].ToString());

                    if (drtemp["UM"].ToString() != string.Empty) dr["UM"] = (drtemp["UM"].ToString());

                    if (drtemp["BudgetUnit"].ToString() != string.Empty) dr["BudgetUnit"] = Convert.ToDouble(drtemp["BudgetUnit"].ToString());

                    if (drtemp["BudgetExt"].ToString() != string.Empty) dr["BudgetExt"] = Convert.ToDouble(drtemp["BudgetExt"].ToString());

                    if (drtemp["MatMod"].ToString() != string.Empty) dr["MatMod"] = Convert.ToDouble(drtemp["MatMod"].ToString());

                    if (drtemp["LabMod"].ToString() != string.Empty) dr["LabMod"] = Convert.ToDouble(drtemp["LabMod"].ToString());

                    if (drtemp["VendorId"].ToString() != string.Empty) dr["VendorId"] = Convert.ToInt32(drtemp["VendorId"].ToString());

                    if (drtemp["Vendor"].ToString() != string.Empty) dr["Vendor"] = (drtemp["Vendor"].ToString());

                    if (drtemp["LabHours"].ToString() != string.Empty) dr["LabHours"] = Convert.ToDouble(drtemp["LabHours"].ToString());

                    if (drtemp["LabRate"].ToString() != string.Empty) dr["LabRate"] = Convert.ToDouble(drtemp["LabRate"].ToString());

                    if (drtemp["LabExt"].ToString() != string.Empty) dr["LabExt"] = Convert.ToDouble(drtemp["LabExt"].ToString());

                    if (drtemp["TotalExt"].ToString() != string.Empty) dr["TotalExt"] = Convert.ToDouble(drtemp["TotalExt"].ToString());

                    if (drtemp["SDate"].ToString() != string.Empty) dr["SDate"] = Convert.ToDateTime(drtemp["SDate"].ToString());

                    if (drtemp["MatDesc"].ToString() != string.Empty) dr["MatDesc"] = (drtemp["MatDesc"].ToString());

                    if (drtemp["OrderNo"].ToString() != string.Empty) dr["OrderNo"] = (drtemp["OrderNo"].ToString());

                    if (drtemp["GroupName"].ToString() != string.Empty) dr["GroupName"] = (drtemp["GroupName"].ToString());

                    if (drtemp["GroupID"].ToString() != string.Empty) dr["GroupID"] = Convert.ToInt32(drtemp["GroupID"].ToString());

                    if (drtemp["CodeDesc"].ToString() != string.Empty) dr["CodeDesc"] = (drtemp["CodeDesc"].ToString());

                    if (drtemp["TargetHours"].ToString() != string.Empty) dr["TargetHours"] = Convert.ToDouble(drtemp["TargetHours"].ToString());

                    else { dr["TargetHours"] = Convert.ToDouble(0); }

                    if (drtemp["BudgetHours"].ToString() != string.Empty) dr["BudgetHours"] = Convert.ToDouble(drtemp["BudgetHours"].ToString());

                    else { dr["BudgetHours"] = Convert.ToDouble(0); }

                    if (drtemp["ChangeOrder"].ToString() != string.Empty) dr["ChangeOrder"] = Convert.ToInt32(drtemp["ChangeOrder"].ToString());
                    else { dr["ChangeOrder"] = 0; }

                    dt.Rows.Add(dr);

                }
            }

            dt.AcceptChanges();

            ViewState["TempBOM"] = dt;

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            dt = (DataTable)ViewState["TempBOM"];
        }

        return dt;
    }





    public void SaveIntoTempDt(DataTable tempdt) {


        DataTable dt = GetemptydBOMdatatable();

        foreach (DataRow drtemp in tempdt.Rows)
        { 

            DataRow dr = dt.NewRow();

            if (drtemp["JobTItemID"].ToString() != string.Empty) dr["JobTItemID"] = Convert.ToInt32(drtemp["JobTItemID"].ToString());

            if (drtemp["Line"].ToString() != string.Empty) dr["Line"] = Convert.ToInt32(drtemp["Line"].ToString());

            if (drtemp["Code"].ToString() != string.Empty) dr["Code"] = (drtemp["Code"].ToString());

            if (drtemp["fDesc"].ToString() != string.Empty) dr["fDesc"] = (drtemp["fDesc"].ToString()); 

            if (drtemp["BType"].ToString() != string.Empty) dr["BType"] = Convert.ToInt32(drtemp["BType"].ToString());

            if (drtemp["BTypes"].ToString() != string.Empty) dr["BTypes"] = (drtemp["BTypes"].ToString());

            if (drtemp["LabItem"].ToString() != string.Empty) dr["LabItem"] = Convert.ToInt32(drtemp["LabItem"].ToString());

            if (drtemp["txtLabItem"].ToString() != string.Empty) dr["txtLabItem"] = (drtemp["txtLabItem"].ToString());

            if (drtemp["MatItem"].ToString() != string.Empty) dr["MatItem"] = Convert.ToInt32(drtemp["MatItem"].ToString());

            if (drtemp["QtyReq"].ToString() != string.Empty) dr["QtyReq"] = Convert.ToDouble(drtemp["QtyReq"].ToString());

            if (drtemp["UM"].ToString() != string.Empty) dr["UM"] = (drtemp["UM"].ToString());

            if (drtemp["BudgetUnit"].ToString() != string.Empty) dr["BudgetUnit"] = Convert.ToDouble(drtemp["BudgetUnit"].ToString());

            if (drtemp["BudgetExt"].ToString() != string.Empty) dr["BudgetExt"] = Convert.ToDouble(drtemp["BudgetExt"].ToString());

            if (drtemp["MatMod"].ToString() != string.Empty) dr["MatMod"] = Convert.ToDouble(drtemp["MatMod"].ToString());

            if (drtemp["LabMod"].ToString() != string.Empty) dr["LabMod"] = Convert.ToDouble(drtemp["LabMod"].ToString());

            if (drtemp["VendorId"].ToString() != string.Empty) dr["VendorId"] = Convert.ToInt32(drtemp["VendorId"].ToString());

            if (drtemp["Vendor"].ToString() != string.Empty) dr["Vendor"] = (drtemp["Vendor"].ToString());

            if (drtemp["LabHours"].ToString() != string.Empty) dr["LabHours"] = Convert.ToDouble(drtemp["LabHours"].ToString());

            if (drtemp["LabRate"].ToString() != string.Empty) dr["LabRate"] = Convert.ToDouble(drtemp["LabRate"].ToString());

            if (drtemp["LabExt"].ToString() != string.Empty) dr["LabExt"] = Convert.ToDouble(drtemp["LabExt"].ToString());

            if (drtemp["TotalExt"].ToString() != string.Empty) dr["TotalExt"] = Convert.ToDouble(drtemp["TotalExt"].ToString());

            if (drtemp["SDate"].ToString() != string.Empty) dr["SDate"] = Convert.ToDateTime(drtemp["SDate"].ToString());

            if (drtemp["MatDesc"].ToString() != string.Empty) dr["MatDesc"] = (drtemp["MatDesc"].ToString());

            if (drtemp["OrderNo"].ToString() != string.Empty) dr["OrderNo"] = (drtemp["OrderNo"].ToString());

            if (drtemp["GroupName"].ToString() != string.Empty) dr["GroupName"] = (drtemp["GroupName"].ToString());

            if (drtemp["GroupID"].ToString() != string.Empty) dr["GroupID"] = Convert.ToInt32(drtemp["GroupID"].ToString());

            if (drtemp["CodeDesc"].ToString() != string.Empty) dr["CodeDesc"] = (drtemp["CodeDesc"].ToString());

            if (drtemp["TargetHours"].ToString() != string.Empty) dr["TargetHours"] = Convert.ToDouble(drtemp["TargetHours"].ToString());

            else { dr["TargetHours"] = Convert.ToDouble(0); }

            if (drtemp["BudgetHours"].ToString() != string.Empty) dr["BudgetHours"] = Convert.ToDouble(drtemp["BudgetHours"].ToString());

            else { dr["BudgetHours"] = Convert.ToDouble(0); }

            if (drtemp["ChangeOrder"].ToString() != string.Empty) dr["ChangeOrder"] = Convert.ToInt32(drtemp["ChangeOrder"].ToString());
            else { dr["ChangeOrder"] = 0; }

            dt.Rows.Add(dr);


        }

            dt.AcceptChanges();

            ViewState["TempBOM"] = dt;
        

    }


    public DataTable GetemptydBOMdatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItemID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("BType", typeof(int));
        dt.Columns.Add("BTypes", typeof(string));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));
        dt.Columns.Add("LabItem", typeof(int));
        dt.Columns.Add("txtLabItem", typeof(string));
        dt.Columns.Add("MatItem", typeof(int));
        dt.Columns.Add("MatMod", typeof(double));
        dt.Columns.Add("LabMod", typeof(double));
        dt.Columns.Add("LabExt", typeof(double));
        dt.Columns.Add("LabRate", typeof(double));
        dt.Columns.Add("LabHours", typeof(double));
        dt.Columns.Add("SDate", typeof(DateTime));
        dt.Columns.Add("VendorId", typeof(int));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("TotalExt", typeof(double));
        dt.Columns.Add("MatDesc", typeof(string));
        dt.Columns.Add("OrderNo", typeof(string));
        dt.Columns.Add("GroupId", typeof(int));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("TargetHours", typeof(double));
        dt.Columns.Add("BudgetHours", typeof(double));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("LSTax", typeof(int));
        dt.Columns.Add("ChangeOrder", typeof(int));

        return dt;

    }


    //////////BOM TAB
    ///

    private void CreateBOMTable()
    {
        try
        {
            DataTable dt = GetemptydBOMdatatable();

            DataRow dr = dt.NewRow();
            dr["JobTItemID"] = 0;
            dr["Line"] = 1;
            dr["OrderNo"] = 1;
            dr["QtyReq"] = "0.00";
            dr["BudgetUnit"] = "0.00";
            dr["MatMod"] = "0.00";
            dr["BudgetExt"] = "0.00";
            dr["LabHours"] = "0.00";
            dr["LabRate"] = "0.00";
            dr["LabMod"] = "0.00";
            dr["LabExt"] = "0.00";
            dr["TotalExt"] = "0.00";
            dr["GroupId"] = 0;
            dr["TargetHours"] = 0;
            dr["BudgetHours"] = 0;
            dr["Code"] = 100;
            dr["ChangeOrder"] = 0;
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["JobTItemID"] = 0;
            dr1["Line"] = 2;
            dr1["OrderNo"] = 2;
            dr1["QtyReq"] = "0.00";
            dr1["BudgetUnit"] = "0.00";
            dr1["MatMod"] = "0.00";
            dr1["BudgetExt"] = "0.00";
            dr1["LabHours"] = "0.00";
            dr1["LabRate"] = "0.00";
            dr1["LabMod"] = "0.00";
            dr1["LabExt"] = "0.00";
            dr1["TotalExt"] = "0.00";
            dr1["GroupId"] = 0;
            dr1["TargetHours"] = 0;
            dr1["BudgetHours"] = 0;
            dr1["Code"] = 100;
            dr["ChangeOrder"] = 0;
            dt.Rows.Add(dr1);

            //ViewState["ProjectTemplate"] = dt;
            BindgvBOM(dt);


            //Session["gvBOM"] = dt;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private bool IsExistsBOM()
    {
        try
        {

            foreach (GridDataItem item in gvBOM.Items)
            {

                //bom items

                HiddenField hdnID = (HiddenField)item.FindControl("hdnID");

                TextBox txtScope = (TextBox)item.FindControl("txtScope");

                if (txtScope.Text == string.Empty)
                {
                    return false;
                }

                if (hdnID.Value != string.Empty)
                {
                    if (Convert.ToInt32(hdnID.Value) > 0)
                    {
                        return true;
                    }
                }
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrIsExistsBOM", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return false;
    }



    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();

            objJob.ConnConfig = Session["config"].ToString();

            ds = objBL_Job.GetBomType(objJob);



            dtBomType = ds.Tables[0];


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion
}
