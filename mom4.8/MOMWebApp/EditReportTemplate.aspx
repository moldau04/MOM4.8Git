<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="EditReportTemplate" Codebehind="EditReportTemplate.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        .InvoicelistTooltip {
            background: #000 none repeat scroll 0 0;
            filter: alpha(opacity=80);
            -moz-opacity: 0.80;
            opacity: 0.80;
            border-radius: 0px !important;
            color: #fff;
            display: none;
            padding: 10px;
            position: absolute;
            width: 300px;
            z-index: 1000;
            margin-top: 120px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">

                                    <div class="page-title"><i class="mdi-action-payment"></i>&nbsp;Edit Invoice Template</div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>

 <%--   
    <div style="float:left" >
        <label class="drpdwn-label" style="margin-top: -10px !important;">Select Invoices from</label>
        </div>--%>

       <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;float:left;margin-top:10px">
                              Select Report
                            </div>
    <div style="float:left">
    <asp:DropDownList ID="ddlInvoicesForLoad" runat="server" 
        CssClass="browser-default" TabIndex="2" Width="300" OnSelectedIndexChanged="ddlInvoicesForLoad_SelectedIndexChanged" AutoPostBack="true">
    </asp:DropDownList>
    </div>
   
    <%--<div class="btnlinks" style="float:left;margin-top:10px;margin-left:14px" >
        <asp:LinkButton ID="lnkPreview" runat="server" OnClick="lnkPreview_Click">Preview</asp:LinkButton>
    </div>--%>
     <div class="btnlinks" style="float:left;margin-top:10px;margin-left:14px" >
        <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edit/Preview</asp:LinkButton>
    </div>
    <div class="btnlinks" style="float:left;margin-top:10px;margin-left:14px" >
        <asp:LinkButton ID="lnkSaveAsDefault" runat="server" OnClick="lnkSaveAsDefault_Click">Save As Default</asp:LinkButton>
    </div>
    <div class="btnlinks" style="float:left;margin-top:10px;margin-left:14px">
        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
    </div>

    <div id="EditTemplate" runat="server" style="background-color:transparent;" >
      <%--  <a href="#" runat="server" value="Add New" id="btnAddNew"></a> --%>
        <br />
        <br /><br />
         <div class="fc-input" style="float: right; text-align: right;" id="stiReport1Header" runat="server">
                            <asp:LinkButton ID="lnkCancel" Text="X" runat="server" OnClick="lnkCancel_Click" ForeColor="White"></asp:LinkButton>
                        </div>
        <cc1:StiWebDesigner ID="StiWebDesigner1" RequestTimeout="9000000" runat="server" Visible="false" OnSaveReport="StiWebDesigner1_SaveReport" Height="750" OnSaveReportAs="StiWebDesigner1_SaveReportAs" OnExit="StiWebDesigner1_Exit" />
        <cc1:StiWebViewer ID="StiWebViewerInvoice" RequestTimeout="9000000" Height="800px" runat="server" ScrollbarsMode="true"
           OnGetReport="StiWebViewerInvoice_GetReport" OnGetReportData="StiWebViewerInvoice_GetReportData" Visible="true"  />
    </div>
     <%--<telerik:RadAjaxManager ID="RadAjaxManager_EditInvoice" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="ddlInvoicesForLoad" EventName="SelectedIndexChanged">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="StiWebDesigner1"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            </AjaxSettings>
         </telerik:RadAjaxManager>--%>
</asp:Content>

