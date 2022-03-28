<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="InvoicesReportwithPayment" Codebehind="InvoicesReportwithPayment.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <link href="Design/css/grid.css" rel="stylesheet" />
 
      <style type="text/css">
        body:nth-of-type(1) img[src*="Blank.gif"] {
                display: none;
        }
        .style-lstview {
            padding: 0 10px;
        }
        .srchtitle-css{
    padding-left: 15px;
    padding-top: 18px;
}
    </style>
    <script type="text/javascript">
         function showMailReport() {
             window.radopen(null, "RadCreateWindow");
             jQuery("<%=txtFrom.ClientID %>").text = "";
             jQuery("<%=txtCC.ClientID %>").text = "";
        }
          function CloseMailReport() {
            var wnd = $find('<%=RadCreateWindow.ClientID %>');
            wnd.Close();
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <telerik:RadAjaxManager ID="RadAjaxManager_Invoice" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btn_SendEmail">
                 <updatedcontrols>
                    <telerik:AjaxUpdatedControl ControlID="reportViewer" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                </updatedcontrols>
            </telerik:AjaxSetting>           
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Invoice" runat="server">
                    </telerik:RadAjaxLoadingPanel>
    <%--Email Popup--%>
    <telerik:RadWindowManager ID="LoadInvoicesModalPopupExtender" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadCreateWindow" Skin="Material" VisibleTitlebar="true" Title="Setup" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="450">
                <ContentTemplate>
                    <div class="form-section-row">
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                    TargetControlID="txtFrom">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                    ControlToValidate="txtFrom" Display="None"
                                    ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                </asp:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtFrom" Display="None"
                                    ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
                                <label for="txtFrom">From</label>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                    TargetControlID="txtTo">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txtTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                </asp:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtTo" Display="None"
                                    ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                                <label for="txtTo">To</label>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtCC" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtCC_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                    TargetControlID="txtCC">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ControlToValidate="txtCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                </asp:ValidatorCalloutExtender>
                                <label for="txtCC">CC</label>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Columns="50"  CssClass="tag-div materialize-textarea textarea-border"
                                    Rows="8"  Text="This is report email sent from Mobile Office Manager. Please find the Invoice Summary Report attached."></asp:TextBox>
                            </div>
                        </div>

                    </div>

                     <div style="clear: both;"></div>

                    <div class="btnlinks">
                        <asp:LinkButton runat="server" ID="btn_SendEmail" Text="Send" OnClick="btn_SendEmail_Click"
                            ValidationGroup="mail" />
                    </div>

                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
   
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row h-40" >
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-swap-vert-circle"></i>&nbsp; Invoices Report</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <%--<a id="mailReport" onclick="return showMailReport();">Mail Report</a>--%>
                                            <asp:LinkButton runat="server" ID="mailReport" OnClientClick="showMailReport();return false;" Text="Email Report" />

                                        </div>

                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dynamicUI">Reports
                                            </a>
                                        </div>
                                        <ul class="nav navbar-nav pull-right">
                                            <li class="dropdown dropdown-user">
                                                <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print dropdown-css" data-hover="dropdown" data-close-others="true" ></a>
                                                <ul id="dynamicUI" class="dropdown-content">
                                                    <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                                    </a></li>
                                                    <li style="margin-left: 0px;"><a href="ARAgingReport.aspx"><span>AR Aging Report</span><div style="clear: both;"></div>
                                                    </a></li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="close"
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
    <div class="container">
        <div class="row">
            <div class="srchpane">
                <div class="srchpaneinner" id="srchPanel" runat="server">
                      <div class="col-lg-12 col-md-12">
                            <div class="search-customer">
                                <div class="sc-form">
                                    <div class="srchinputwrap">
                                        <div class="srchtitle srchtitlecustomwidth srchtitle-css">
                                            Start
                                        </div>
                                        <div class="srchinputwrap">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"
                                                MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtStartDate">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="rfvStartDt"
                                                runat="server" ControlToValidate="txtStartDate" Display="None" ErrorMessage="Start date is Required" ValidationGroup="search"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceStartDt" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvStartDt" />
                                            <asp:RegularExpressionValidator ID="rfvStartDt1" ControlToValidate="txtStartDate" ValidationGroup="search"
                                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                            </asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceStartDt1" runat="server" Enabled="True" PopupPosition="Right"
                                                TargetControlID="rfvStartDt1" />
                                        </div>
                                        <div class="srchtitle srchtitlecustomwidth srchtitle-css" >
                                            End
                                        </div>
                                         <div class="srchinputwrap">
                                             <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"
                                            MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtEndDate">
                                        </asp:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvEndDt"
                                            runat="server" ControlToValidate="txtEndDate" Display="None" ErrorMessage="End date is Required"  ValidationGroup="search"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="rfvEndDt" />
                                        <asp:RegularExpressionValidator ID="rfvEndDt1" ControlToValidate = "txtEndDate" ValidationGroup="search" 
                                            ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEndDt1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvEndDt1" />
                                             </div>

                                        <%--  <label for="" style="margin-left: 39px; margin-bottom: 19px;">End</label>--%>
                                       
                                        
                                                                                                             </div>
                                </div>
                                <div class="srchinputwrap btnlinksicon srchclr">
                                    <asp:LinkButton ID="lnkSearch"  runat="server" CausesValidation="true" ToolTip="Refresh" ValidationGroup="search"
                                            OnClick="lnkSearch_Click"><i class="fa fa-refresh"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                </div>
            </div>
            <div class="grid_container">
                <div class="form-section-row pmd-card">
                    <%--view report--%>
                    <div class="col-lg-12 col-md-12">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel21" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup">
                            <div runat="server" id="reportViewer"></div>
                            <rsweb:ReportViewer ID="rvInvoices" runat="server" Width="1200px" Height="1500px" Visible="false"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px" PageCountMode="Actual"
                                AsyncRendering="false" ShowZoomControl="False" OnReportRefresh="rvInvoices_ReportRefresh">
                            </rsweb:ReportViewer>
                            <cc1:StiWebViewer ID="StiWebViewerInvoicesReport" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="90000"
                                OnGetReport="StiWebViewerInvoicesReport_GetReport" OnGetReportData="StiWebViewerInvoicesReport_GetReportData" />

                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
  
</asp:Content>

