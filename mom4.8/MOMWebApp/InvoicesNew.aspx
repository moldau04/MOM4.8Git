<%@ Page Title="Invoices || MOM" Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false"  AutoEventWireup="true" Inherits="InvoicesNew" CodeBehind="InvoicesNew.aspx.cs" %>

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
         #overlay {
            position: fixed; /* Sit on top of the page content */
            display: none; /* Hidden by default */
            width: 100%; /* Full width (cover the whole page) */
            height: 100%; /* Full height (cover the whole page) */
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5); /* Black background with opacity */
            z-index: 1000000; /* Specify a stack order in case you're using a different order for other elements */
            cursor: pointer; /* Add a pointer on hover */
        }
         .labelButton{
            padding: 5px 10px 5px 10px;
            font-size: 0.9em;
            float: left;
            line-height: 19px !important;
            border-radius: 3px;
            background-color: #1C5FB1 !important;
            color: #fff !important;
            margin: 3px -9px;
            cursor: pointer;
        }
    </style>
    <%--INVOICES GRID--%>

    <script>


        function opencCeateForm() {
            window.radopen(null, "RadCreateWindow");
        }

        function emailAllConfirm(strName) {
            var mess = "Are you sure you want to send emails to all customers?";

            if (strName !== null && strName !== '') {
                mess = "Are you sure you want to send emails to all " + strName + "?";
            } else {
                mess = "Are you sure you want to send emails to all customers?";
            }

            if (confirm(mess)) { return true; }
            else { return false; }
        }

        function deleteAlert() {
            var invoiceIds = $("[id$='chkSelectSelectCheckBox']:checked").closest("tr").find("[id$='lblRefId']").text();
            var WipInvoice = $("[id$='chkSelectSelectCheckBox']:checked").closest("tr").find("[id$='hdnWipInvoice']").val();
            if (invoiceIds !== "") {
                if (WipInvoice != "0") {
                    WipInvWarning();
                    return false;
                } else {
                    if (!confirm("Are you sure you want to delete invoice #" + invoiceIds)) {
                        return false;
                    }
                    else {
                        return true;
                    }
                }

            }
            else {
                return false;
            }
        }

        function voidAlert() {
            var invoiceIds = $("[id$='chkSelectSelectCheckBox']:checked").closest("tr").find("[id$='lblRefId']").text();
            if (invoiceIds !== "") {
                if (!confirm("Are you sure you want to void invoice #" + invoiceIds)) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
        }
           function pageLoad() {
                var grid = $find("<%= RadGrid_Invoice.ClientID %>");
                var columns = grid.get_masterTableView().get_columns();
                for (var i = 0; i < columns.length; i++) {
                    columns[i].resizeToFit(false, true);
                }

               
            }
         function ShowRestoreGridSettingsButton() {
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        Sys.Application.add_init(appl_init);

        function appl_init() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_beginRequest(BlockUI);
            pgRegMgr.add_endRequest(UnblockUI);
        }

        function BlockUI(sender, args) {
            document.getElementById("overlay").style.display = "block";
        }
        function UnblockUI(sender, args) {
            document.getElementById("overlay").style.display = "none";
        }
    </script>  

    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." style="position:fixed; margin-top: 25%; margin-left: 50%;" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_Invoice" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Invoice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice"/>
                    <telerik:AjaxUpdatedControl ControlID="srchPanel" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlDepartment" />
                    <telerik:AjaxUpdatedControl ControlID="ddllocation"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlpaidunpaid"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlPrintOnly"  />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt"  />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="lblDay" />
                    <telerik:AjaxUpdatedControl ControlID="lblWeek" />
                    <telerik:AjaxUpdatedControl ControlID="lblMonth" />
                    <telerik:AjaxUpdatedControl ControlID="lblQuarter" />
                    <telerik:AjaxUpdatedControl ControlID="lblYear" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate"  />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnVoidInvoice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice"/>
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch"/>
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus"/>
                    <telerik:AjaxUpdatedControl ControlID="ddlDepartment" />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="lblDay" />
                    <telerik:AjaxUpdatedControl ControlID="lblWeek" />
                    <telerik:AjaxUpdatedControl ControlID="lblMonth" />
                    <telerik:AjaxUpdatedControl ControlID="lblQuarter" />
                    <telerik:AjaxUpdatedControl ControlID="lblQuarter" />
                    <telerik:AjaxUpdatedControl ControlID="ddlpaidunpaid"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlPrintOnly"  />
                    <telerik:AjaxUpdatedControl ControlID="hdnInvoiceSelectDtRange" />
                     <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate"/> 
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="lnkSearchFilter">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice"/>
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate"/>  
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice"/>
                    <telerik:AjaxUpdatedControl ControlID="srchPanel" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                    <telerik:AjaxUpdatedControl ControlID="ddlDepartment" />
                    <telerik:AjaxUpdatedControl ControlID="ddllocation" />
                    <telerik:AjaxUpdatedControl ControlID="ddlpaidunpaid"/>
                    <telerik:AjaxUpdatedControl ControlID="ddlPrintOnly"  />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch"/>
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="lblDay" />
                    <telerik:AjaxUpdatedControl ControlID="lblWeek" />
                    <telerik:AjaxUpdatedControl ControlID="lblMonth" />
                    <telerik:AjaxUpdatedControl ControlID="lblQuarter" />
                    <telerik:AjaxUpdatedControl ControlID="lblYear" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice"/>
                    <telerik:AjaxUpdatedControl ControlID="srchPanel" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus"  />
                    <telerik:AjaxUpdatedControl ControlID="ddlDepartment"  />
                    <telerik:AjaxUpdatedControl ControlID="ddllocation"  />
                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                    <telerik:AjaxUpdatedControl ControlID="txtInvDt" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate"/>
                    <telerik:AjaxUpdatedControl ControlID="isShowAll" />
                    <telerik:AjaxUpdatedControl ControlID="lblDay" />
                    <telerik:AjaxUpdatedControl ControlID="lblWeek" />
                    <telerik:AjaxUpdatedControl ControlID="lblMonth" />
                    <telerik:AjaxUpdatedControl ControlID="lblQuarter" />
                    <telerik:AjaxUpdatedControl ControlID="lblYear" />
                    <telerik:AjaxUpdatedControl ControlID="ddlpaidunpaid" />
                    <telerik:AjaxUpdatedControl ControlID="ddlPrintOnly" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkMailAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="lnkSaveGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="lnkRestoreGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <asp:HiddenField runat="server" ID="hdnHideDates" Value="0" />


    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">

                                    <div class="page-title"><i class="mdi-action-payment"></i>&nbsp;Invoice</div>
                                    <div class="buttonContainer">
                                        <asp:Panel runat="server" ID="pnlGridButtons">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnCopy"
                                                    runat="server" OnClick="btnCopy_Click">Copy</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>       

                                            <ul id="drpMenu" class="nomgn hideMenu menuList">

                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnVoidInvoice" OnClick="btnVoidInvoice_Click" OnClientClick="return voidAlert();" runat="server">Void</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <%--<asp:LinkButton ID="btnDelete"     runat="server" OnClick="btnDelete_Click" OnClientClick="return SelectedRowVoid('ctl00_ContentPlaceHolder1_RadGrid_Invoice','Invoice');">Delete</asp:LinkButton>--%>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return deleteAlert();">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>

                                                <li>
                                                    <ul id="dynamicUI" class="dropdown-content">
                                                        <li><a href="InvoiceListingReport.aspx?type=Invoices">Add New Report</a></li>
                                                         <li>
                                                            <asp:LinkButton ID="lnkARAging360ReportByLocation" runat="server" CausesValidation="true" OnClick="lnkARAging360ReportByLocation_Click" Enabled="true">AR Aging 360 by Location</asp:LinkButton>
                                                        </li>
                                                         <li>
                                                            <asp:LinkButton ID="lnkARAging360ReportByCust" runat="server" CausesValidation="true" OnClick="lnkARAging360ReportByCust_Click" Enabled="true">AR Aging 360 by Customer</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnk_EditReportTemplate" runat="server" CausesValidation="true" OnClick="lnk_EditReportTemplate_Click" Enabled="true">Edit Report Template</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkInvoicesReport" runat="server" CausesValidation="true" OnClick="lnkInvoicesReport_Click" Enabled="true">Invoice Summary Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkInvoicesReportwithPayment" runat="server" CausesValidation="true" OnClick="lnkInvoicesReportwithPayment_Click" Enabled="true">Invoice with Payment</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkARAgingReport" runat="server" CausesValidation="true" OnClick="lnkARAgingReport_Click" Enabled="true">Customer AR Aging Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkARAgingReportCust" runat="server" CausesValidation="true" OnClick="lnkARAgingReportCust_Click" Enabled="true">Customer AR Aging by Custom</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkARAgingReportByLocation" runat="server" CausesValidation="true" OnClick="lnkARAgingReportByLocation_Click" Enabled="true">Location AR Aging Report</asp:LinkButton>
                                                        </li>
                                                        <li runat="server" id="isNoneTS">
                                                            <asp:LinkButton ID="lnkARAgingReportDep" runat="server" CausesValidation="true" OnClick="lnkARAgingReportDep_Click" Enabled="true">Customer AR Aging by Department</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkARAgingReportByTerritory" runat="server" CausesValidation="true" OnClick="lnkARAgingReportByTerritory_Click" Enabled="true">Customer AR Aging by Default Salesperson</asp:LinkButton>
                                                        </li>
                                                        <li>

                                                            <asp:LinkButton ID="lnkCustomerStatement" OnClick="lnkCustomerStatement_Click" runat="server">Customer Statement</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkSalesTaxReport" runat="server" CausesValidation="true" OnClick="lnkSalesTaxReport_Click" Enabled="true">Sales Tax Summary Report</asp:LinkButton>
                                                        </li>
                                                        <li runat="server" id="isSeco">
                                                            <asp:LinkButton ID="lnkSalesTax2Report" runat="server" CausesValidation="true" OnClick="lnkSalesTax2Report_Click" Enabled="true">Sales Tax 2 Summary Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkSalesTaxCollectedReport" runat="server" CausesValidation="true" OnClick="lnkSalesTaxCollectedReport_Click" Enabled="true">Sales Tax Collected Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPrintInvoiceRegister" OnClick="lnkPrintInvoiceRegister_Click" runat="server">Invoice Weekly Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPrintInvoiceRegisterGL" runat="server" CausesValidation="true" OnClick="lnkPrintInvoiceRegisterGL_Click" Enabled="true">Invoice Register GL Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkInvoiceByRecurringFrequency" runat="server" CausesValidation="true" OnClick="lnkInvoiceByRecurringFrequency_Click" Enabled="true">Invoice By Recurring Frequency</asp:LinkButton>
                                                        </li>
                                                        <li runat="server" id="isTS">
                                                            <asp:LinkButton ID="lnkARAgingReportByJobType" runat="server" CausesValidation="true" OnClick="lnkARAgingReportByJobType_Click" Enabled="true">AR Aging Report By Department</asp:LinkButton>
                                                        </li>
                                                        <li runat="server" id="Li1">
                                                            <asp:LinkButton ID="lnkARAgingReportByLocType" runat="server" CausesValidation="true" OnClick="lnkARAgingReportByLocType_Click" Enabled="true">AR Aging Report By Location Type</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkAgedReceivableReport" runat="server" CausesValidation="true" OnClick="lnkAgedReceivableReport_Click" Enabled="true">Aged Receivable Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnk_InvoiceMaint" runat="server" CausesValidation="true"
                                                                OnClick="lnk_InvoiceMaint_Click" Enabled="true">Maintenance Report</asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnk_InvoiceException" runat="server" CausesValidation="true"
                                                                OnClick="lnk_InvoiceException_Click" Enabled="true">Exception Report</asp:LinkButton></li>

                                                    </ul>
                                                    <div class="btnlinks" id="LI1pnlGridButtons" runat="server">
                                                        <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dynamicUI">Reports
                                                        </a>
                                                      <%--  <asp:LinkButton ID="lnkReport" class="dropdown-button" data-beloworigin="true" data-activates="dynamicUI"  runat="server" CausesValidation="true" OnClick="lnkInvoicesReport_Click" OnClientClick="return false;">Reports</asp:LinkButton>
                                                        --%>
                                                    </div>
                                                </li>
                                                <li>
                                                    <ul id="dynamicPDF" class="dropdown-content">
                                                        <li>

                                                            <asp:LinkButton ID="lnkPrint" runat="server" Enabled="true" OnClick="lnkPDF_Click"> <i class="fa fa-file-pdf-o" style="color:red; background-color:transparent" aria-hidden="true"></i>&nbsp;&nbsp; Invoices <i class="fa fa-download" aria-hidden="true" style="background-color:transparent;"></i></asp:LinkButton>

                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkMaintenance" runat="server" CausesValidation="true"
                                                                Enabled="true" OnClick="lnkMaintenance_Click"> <i class="fa fa-file-pdf-o" style="color:red; background-color:transparent" aria-hidden="true" ></i>&nbsp;&nbsp; Invoice Maintenance Report &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" style="background-color:transparent;"></i></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkException" runat="server" CausesValidation="true"
                                                                Enabled="true" OnClick="lnkException_Click"> <i class="fa fa-file-pdf-o" style="color:red; background-color:transparent" aria-hidden="true" ></i>&nbsp;&nbsp; Invoice Exception Report &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" style="background-color:transparent;"></i></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPDFTI" runat="server" CausesValidation="true"
                                                                Enabled="true" OnClick="lnkPDFTI_Click"> <i class="fa fa-file-pdf-o" style="color:red; background-color:transparent" aria-hidden="true" ></i>&nbsp;&nbsp; Invoice With Ticket &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" style="background-color:transparent;"></i></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkAdamMaintenance" runat="server" CausesValidation="true"
                                                                Enabled="true" OnClick="lnkPDF_Click"> <i class="fa fa-file-pdf-o" style="color:red; background-color:transparent" aria-hidden="true" ></i>&nbsp;&nbsp; Maintenance Invoices &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" style="background-color:transparent;"></i></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkAdamBilling" runat="server" CausesValidation="true"
                                                                Enabled="true" OnClick="lnkPDF_Click"> <i class="fa fa-file-pdf-o" style="color:red; background-color:transparent" aria-hidden="true" ></i>&nbsp;&nbsp; Billing Invoice &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" style="background-color:transparent;"></i></asp:LinkButton></li>
                                                    </ul>
                                                    <div class="btnlinks" id="LI2pnlGridButtons" runat="server">
                                                        <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dynamicPDF" id="lnkPdf" runat="server">Invoice
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkMailAll" OnClick="lnkMailAll_Click" OnClientClick="return emailAllConfirm('');" runat="server">Email All</asp:LinkButton>
                                                    </div>
                                                </li>

                                                <li>
                                                        <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkRestoreGridSettings" runat="server" CausesValidation="False" OnClick="lnkRestoreGridSettings_Click" 
                                                        style="display:none">Restore Grid</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkSaveGridSettings" runat="server" CausesValidation="False" OnClick="lnkSaveGridSettings_Click" 
                                                        style="display:none">Save Grid</asp:LinkButton>
                                                </div>
                                                
                                                <label id="lbSaveGridSettings" runat="server" class="labelButton" ToolTip="Save Grid Settings" style="display:none">
                                                    <input type="radio" id="rdSaveGridSettings" onclick="SaveGridSettings();"/>
                                                    Save Grid
                                                </label>
                                                <label id="lbRestoreGridSettings" runat="server" class="labelButton" ToolTip="Restore Default Settings for Grid" style="display:none">
                                                    <input type="radio" id="rdRestoreGridSettings" onclick="RestoreGridSettings();"/>
                                                    Restore Grid
                                                </label>
                                                </li>
                                            </ul>
                                        </asp:Panel>
                                    </div>
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

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <ul class="anchor-links">
                                <li id="liLogs" style="text-align: center;" runat="server"><a href="#accrdlogs">Email History Log</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane" style="margin-top:0;">

                <div class="srchpaneinner" id="srchPanel" runat="server">

                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap tabcontainer">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server" >
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" >Clear </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                              
                                <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                               
                            </span>
                        </div>
                    </div>

                </div>

                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Search
                    </div>
                   
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst"  onchange="showFilterSearchHistory();">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="i.ref">Invoice#</asp:ListItem>
                                    <asp:ListItem Value="i.fdate">Invoice Date</asp:ListItem>
                                    <asp:ListItem Value="l.ID">Location ID</asp:ListItem>
                                    <asp:ListItem Value="l.loc">Location</asp:ListItem>
                                    <asp:ListItem Value="r.name">Customer</asp:ListItem>
                                    <asp:ListItem Value="i.Status">Status</asp:ListItem>
                                    <asp:ListItem Value="i.Type">Department</asp:ListItem>
                                    <asp:ListItem Value="i.PO">PO</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="browser-default selectst"  Style="display: none" ></asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                              <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst"  Style="display: none" >
                                   <asp:ListItem Value="">Select Status</asp:ListItem>
                                    <asp:ListItem Value="0">Open</asp:ListItem>
                                    <asp:ListItem Value="1">Paid</asp:ListItem>
                                    <asp:ListItem Value="2">Voided</asp:ListItem>
                                    <asp:ListItem Value="3">Partially Paid</asp:ListItem>
                                    <asp:ListItem Value="4">Marked as Pending</asp:ListItem>
                                    <asp:ListItem Value="5">Paid by Credit Card</asp:ListItem>
                                </asp:DropDownList>

                               
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddllocation" runat="server" CssClass="browser-default selectst" Style="display: none" ></asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlUserType" runat="server" CssClass="browser-default selectst"  Style="display: none" >
                                      <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="0">Office</asp:ListItem>
                                    <asp:ListItem Value="1">Field</asp:ListItem>
                                    <asp:ListItem Value="2">Customer</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlSuper" runat="server" CssClass="browser-default selectst"  Style="display: none" ></asp:DropDownList>
                            </div>
                            <div class="srchinputwrap pd-negatenw input-field">
                                <asp:TextBox ID="txtInvDt" runat="server" CssClass="srchcstm datepicker_mom"  Style="display: none" ></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" ></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <telerik:RadComboBox EmptyMessage="Select" RenderMode="Auto" ID="ddlpaidunpaid" runat="server" CssClass="browser-default selectst selectsml" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                    <Items>                                        
                                        <telerik:RadComboBoxItem Text="Open" Value="0" />
                                        <telerik:RadComboBoxItem Text="Paid" Value="1" />
                                         <telerik:RadComboBoxItem Text="Partially Paid" Value="3" />
                                        <telerik:RadComboBoxItem Text="Void" Value="2" />
                                    </Items>

                                </telerik:RadComboBox>
                            </div>
                     
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlPrintOnly" runat="server" CssClass="browser-default selectst"  ></asp:DropDownList>
                            </div>
                   

                    <div class="srchinputwrap srchclr btnlinksicon">
                        <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" OnClientClick="return validateValue();"><i class="mdi-action-search"></i>
                        </asp:LinkButton>
                        <div style="display:none">
                         <asp:LinkButton ID="lnkSearchFilter" runat="server" OnClick="lnkSearchFilter_Click"><i class="mdi-action-search"></i>
                        </asp:LinkButton>
                            </div>
                    </div>
                </div>
            </div>
            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">


                     
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Invoice" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">

                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Invoice" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Invoice">
                            <%--ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd"--%>

                            <%--   OnItemEvent="RadGrid_Invoice_ItemEvent"
                                OnItemCommand ="RadGrid_Invoice_ItemCommand"--%>
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Invoice"
                                OnPreRender="RadGrid_Invoice_PreRender"
                                AllowFilteringByColumn="true"
                                ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%"
                                PagerStyle-AlwaysVisible="true" FilterType="CheckList"                               
                                EnableLinqExpressions="false"
                                OnNeedDataSource="RadGrid_Invoice_NeedDataSource"
                                OnItemCreated="RadGrid_Invoice_ItemCreated"
                                OnExcelMLExportRowCreated="RadGrid_Invoice_ExcelMLExportRowCreated"                               
                                OnPageIndexChanged="RadGrid_Invoice_PageIndexChanged"
                                OnPageSizeChanged="RadGrid_Invoice_PageSizeChanged"
                                OnItemCommand="RadGrid_Invoice_ItemCommand"   
                                 ClientSettings-AllowColumnsReorder="true"
                                    ClientSettings-ReorderColumnsOnClient="true"
                               OnFilterCheckListItemsRequested="RadGrid_Invoice_FilterCheckListItemsRequested"
                          
                                >
                                
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                               <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true"/>
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        <ClientEvents 
                                            OnGridCreated="OnGridCreated" OnHeaderMenuShowing="headerMenuShowing" 
                                            OnColumnHidden="ColumnSettingsChange" OnColumnShown="ColumnSettingsChange"
                                            OnColumnResized="ColumnSettingsChange" OnColumnSwapped="ColumnSettingsChange" 
                                            />
                                    </ClientSettings>
                                <MasterTableView DataKeyNames="ref" AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True"  EnableHeaderContextMenu="true">
                                    <Columns>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                <asp:Label ID="lblLoc" runat="server" Text='<%# Bind("Loc") %>' />
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Total") %>' />
                                                <asp:Label ID="lblInvStatus" runat="server" Text='<%# Bind("InvStatus") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40" Reorderable="false"  Resizable="false">
                                        </telerik:GridClientSelectColumn> 

                                           <telerik:GridTemplateColumn UniqueName="isRecurring" DataField="isRecurring" AllowFiltering="false"  SortExpression="isRecurring" AutoPostBackOnFilter="false"  HeaderText="   " ShowFilterIcon="false" HeaderStyle-Width="50"   >
                                            <ItemTemplate>
                                                <asp:Label ID="lblisRecurring" runat="server" Visible="false" Text='<%# Bind("isRecurring")%>'></asp:Label>
                                                 <i class="mdi-notification-sync" style="font-weight: 800;" width="16px" title="Recurring invoice" id="imgisRecurring" runat="server" visible='<%# Eval("isRecurring").ToString().Trim() == "1" ? true:false %>'></i>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="ref"
                                            HeaderText="Invoice #" SortExpression="ref" HeaderStyle-Width="100" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AllowFiltering="true"
                                            ShowFilterIcon="false" UniqueName="InvoiceRef"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRefId" Style="display: none" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                               <%-- <asp:HyperLink ID="lblInv" runat="server" Text='<%# Bind("ref") %>'></asp:HyperLink>--%>
                                                <asp:HiddenField ID="hdnBatch" runat="server" Value='<%# Bind("Batch") %>' />
                                                 <asp:HiddenField ID="hdnWipInvoice" runat="server" Value='<%# Bind("WipInvoice") %>' />
                                                  <asp:HiddenField ID="hdnJobStatus" runat="server" Value='<%# Bind("JobStatus") %>' />

                                                 <asp:HyperLink ID="lblInv" runat="server" NavigateUrl='<%# "addInvoicenew.aspx?uid=" + Eval("ref") %>'><%#Eval("ref")%></asp:HyperLink>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn UniqueName="ManualInv" DataField="manualInv" HeaderText="Manual Inv. #" SortExpression="manualInv" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="Job" DataField="Job" SortExpression="Job" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            HeaderText="Project #" ShowFilterIcon="false" HeaderStyle-Width="100"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbljob" runat="server" Text='<%# Convert.ToString(Eval("Job"))=="0" ? "" : Eval("Job") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FDate" DataField="fdate" SortExpression="fdate" AutoPostBackOnFilter="false" HeaderStyle-Width="110" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Invoice date"
                                            AllowFiltering="false"
                                            ShowFilterIcon="false"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:M/d/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="ID" DataField="id" HeaderText="Location ID" SortExpression="id" HeaderStyle-Width="140"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Tag" DataField="tag" HeaderText="Location" SortExpression="tag" HeaderStyle-Width="140"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Company" DataField="Company" HeaderText="Company" SortExpression="Company" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="Amount" DataField="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Pretax Amount" ShowFilterIcon="false" HeaderStyle-Width="110"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPretaxAmout" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="SalesTax" DataField="stax" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="stax" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Sales Tax" ShowFilterIcon="false" HeaderStyle-Width="110"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PSTTax", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn UniqueName="GSTTax" DataField="GSTTax" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="GSTTax" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="GST Tax" ShowFilterIcon="false" HeaderStyle-Width="110"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGSTTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GSTTax", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Total" DataField="total" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="total" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Invoice Total" ShowFilterIcon="false" HeaderStyle-Width="110"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceTotal" runat="server" ForeColor='<%# Convert.ToDouble(Eval("total"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "total", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Status" SortExpression="Status" UniqueName="Status" HeaderText="Status" HeaderStyle-Width="80" FilterCheckListEnableLoadOnDemand="true" CurrentFilterFunction="Contains"
                                            FilterControlAltText="Status" AutoPostBackOnFilter="true">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn UniqueName="PO" DataField="po" HeaderText="PO" SortExpression="po" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <%--  <telerik:GridBoundColumn  DataField="Job" HeaderText="Project #" SortExpression="Job" UniqueName="Job" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>--%>
                                        <telerik:GridBoundColumn UniqueName="CustomerName" DataField="customername" HeaderText="Customer" SortExpression="customername" HeaderStyle-Width="140"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="DType" DataField="type" HeaderText="Department Type" SortExpression="type" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="Balance" DataField="balance"
                                            FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="balance"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount Due" ShowFilterIcon="false" HeaderStyle-Width="110"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDue" runat="server" ForeColor='<%# Convert.ToDouble(Eval("balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PRDate" DataField="PaymentReceivedDate" SortExpression="PaymentReceivedDate" AutoPostBackOnFilter="true" HeaderStyle-Width="150" DataType="System.String"
                                            CurrentFilterFunction="Contains" AllowFiltering="false"
                                            HeaderText="Payment Date" ShowFilterIcon="false"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("PaymentReceivedDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="SDesc" DataField="SDesc" HeaderText="Salesperson" SortExpression="SDesc" HeaderStyle-Width="140"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false"  Reorderable="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="LocRemarks" DataField="locRemarks" SortExpression="locRemarks" AutoPostBackOnFilter="true" HeaderStyle-Width="150"
                                            CurrentFilterFunction="Contains" HeaderText="Location Remark" ShowFilterIcon="false"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocRemark" runat="server"><%# Eval("locRemarks").ToString().PadRight(60).Substring(0,60).TrimEnd() %></asp:Label>
                                                <asp:Label ID="lblLocRemarkHover" runat="server" CssClass="InvoicelistTooltip" Text='<%# Eval("locRemarks").ToString().Trim() %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="JobRemarks" DataField="JobRemarks" SortExpression="JobRemarks" AutoPostBackOnFilter="true" HeaderStyle-Width="150"
                                            CurrentFilterFunction="Contains" HeaderText="Project Remark" ShowFilterIcon="false"  Reorderable="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobRemark" runat="server"><%# Eval("JobRemarks").ToString().PadRight(60).Substring(0,60).TrimEnd()%></asp:Label>
                                                <asp:Label ID="lblJobRemarkHover" runat="server" CssClass="InvoicelistTooltip" Text='<%# Eval("JobRemarks").ToString().Trim() %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridBoundColumn DataField="CreatedBy" SortExpression="CreatedBy" UniqueName="CreatedBy" HeaderText="Created By" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" >
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                                <%--   <FilterMenu CssClass="RadFilterMenu_CheckList"  >     </FilterMenu>--%>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>

                </div>

            </div>

            <%--Email Sending Logs--%>
            <div class="accordian-wrap"  >
                <div class="col s12 m12 l12">
                    <div class="row">
                        <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                            <li id="tbLogs" runat="server" style="display: block">
                                <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Email History Log</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="grid_container">
                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                    <div class="RadGrid RadGrid_Material">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoadLog() {
                                                                    try {
                                                                        var grid = $find("<%= RadGrid_gvLogs.ClientID %>");
                                                                        var columns = grid.get_masterTableView().get_columns();
                                                                        for (var i = 0; i < columns.length; i++) {
                                                                            columns[i].resizeToFit(false, true);
                                                                        }
                                                                    } catch (e) {

                                                                    }
                                                                }

                                                                var requestInitiator = null;
                                                                var selectionStart = null;

                                                                function requestStart(sender, args) {
                                                                    requestInitiator = document.activeElement.id;
                                                                    if (document.activeElement.tagName == "INPUT") {
                                                                        selectionStart = document.activeElement.selectionStart;
                                                                    }
                                                                }

                                                                function responseEnd(sender, args) {
                                                                    var element = document.getElementById(requestInitiator);
                                                                    if (element && element.tagName == "INPUT") {
                                                                        element.focus();
                                                                        element.selectionStart = selectionStart;
                                                                    }
                                                                }
                                                            </script>
                                                        </telerik:RadCodeBlock>
                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_gvLogs_ItemCreated"
                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvLogs_NeedDataSource">
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                                                    <%--<GroupByExpressions>
                                                                        <telerik:GridGroupByExpression>
                                                                            <SelectFields>
                                                                                <telerik:GridGroupByField FieldAlias="SessionNo" FieldName="SessionNo" HeaderText="Session No"></telerik:GridGroupByField>
                                                                            </SelectFields>
                                                                            <GroupByFields>
                                                                                <telerik:GridGroupByField FieldName="SessionNo"></telerik:GridGroupByField>
                                                                            </GroupByFields>
                                                                        </telerik:GridGroupByExpression>
                                                                    </GroupByExpressions>--%>
                                                                    <Columns>
                                                                        <%--<telerik:GridTemplateColumn DataField="SessionNo" SortExpression="SessionNo" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Session No" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSessionNo" runat="server" Text='<%# Eval("SessionNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>
                                                                        <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldate" runat="server" Text='<%# Eval("EmailDate", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbltime" runat="server" Text='<%# Eval("EmailDate","{0: hh:mm tt}") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Username" SortExpression="Username" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Invoice #" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="EmailFunction" SortExpression="EmailFunction" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Function" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmailFunction" runat="server" Text='<%# Eval("EmailFunction") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="UsrErrMessage" SortExpression="UsrErrMessage" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Error Message" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUsrErrMessage" runat="server" Text='<%# Eval("UsrErrMessage") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadAjaxPanel>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="cf"></div>
                                        </div>
                                    </div>
                                    <%--<div style="clear: both;"></div>--%>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

            <%-- $$$$$$$$$$$$ Open Invoices for Payment $$$$   --%>
            <br />
            <br />
            <br />
            <br />
            <asp:Panel ID="pnlPaymentList" runat="server" Visible="false">
                <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">

                        <b>&nbsp;Open Invoices for Payment</b>
                        <br />
                        <div>
                            <div class="btnlinks" id="sdfsdf" runat="server">
                                &nbsp;<asp:LinkButton OnClientClick="return SelectedRow('ctl00_ContentPlaceHolder1_RadGrid_PaymentInv','Invoice');"
                                    ID="lnkMakePayment" runat="server" OnClick="lnkMakePayment_Click" ValidationGroup="selinv">Credit Card
   
                                </asp:LinkButton>
                            </div>
                            <div class="btnlinks" id="Div1" runat="server">
                                &nbsp;<asp:LinkButton OnClientClick="return SelectedRow('ctl00_ContentPlaceHolder1_RadGrid_PaymentInv','Invoice');"
                                    ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" ValidationGroup="selinv">ACH
                                                   
                                </asp:LinkButton>
                            </div>

                            <div class="btnlinks" id="Div2" runat="server">
                                <asp:Label ID="Label1" runat="server" Style="color: Black; float: left; margin-right: 10px;"
                                    Text="Total Payment Amount:"></asp:Label>
                            </div>
                            <div class="btnlinks" id="Div3" runat="server">
                                <asp:TextBox ID="txtTotalInvoiceAmt" runat="server" onfocus="this.blur();"
                                    Style="font-weight: bold; float: left" Text="$0"></asp:TextBox>
                            </div>
                            <asp:Label ID="lblCountPayment" runat="server" Style="float: right; color: Gray; float: right; margin-right: 10px; font-style: italic;">
                            </asp:Label>
                        </div>
                        <div class="grid_container">
                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                <div class="RadGrid RadGrid_Material">
                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_PaymentInv"
                                        AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                        ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                        AllowCustomPaging="True" OnNeedDataSource="RadGrid_PaymentInv_NeedDataSource">
                                        <CommandItemStyle />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="True"></Selecting>

                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="false">
                                            <Columns>
                                                <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                        <asp:Label ID="lblID" Style="display: none;" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <%--<telerik:GridClientSelectColumn UniqueName="chkPaySelect"   HeaderStyle-Width="40">
                                                </telerik:GridClientSelectColumn>--%>


                                                <telerik:GridTemplateColumn DataField="ref" HeaderText="Invoice #" SortExpression="ref" DataType="System.String"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lblInv" runat="server" Text='<%# Bind("ref") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridBoundColumn DataField="manualInv" HeaderText="Manual Inv. #" SortExpression="manualInv"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn DataField="fdate" SortExpression="fdate" AutoPostBackOnFilter="true" DataType="System.String"
                                                    CurrentFilterFunction="Contains" HeaderText="Invoice date" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:M/d/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="id" HeaderText="Location ID" SortExpression="id"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="tag" HeaderText="Location" SortExpression="tag"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn DataField="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Pretax Amount" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPretaxAmout" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DataField="stax" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="stax" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Sales Tax" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSalesTax1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "stax", "{0:c}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DataField="total" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="total" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Invoice Total" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceTotal" runat="server" ForeColor='<%# Convert.ToDouble(Eval("total"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "total", "{0:c}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="status" HeaderText="Status" SortExpression="status"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="po" HeaderText="PO" SortExpression="po"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="customername" HeaderText="Customer" SortExpression="customername"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="type" HeaderText="Department Type" SortExpression="type"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn DataField="balance" AutoPostBackOnFilter="true" HeaderText="Amount Due" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnBal" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Invbalance", "{0:0.00}")%>' />
                                                        $<asp:TextBox ID="txtBalance"
                                                            Style="text-align: right;"
                                                            onkeyup="calculate();" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Invbalance", "{0:0.00}")%>'></asp:TextBox>

                                                        <asp:RequiredFieldValidator ID="rfvBalance" runat="server" ControlToValidate="txtBalance" ValidationGroup="selinv"
                                                            Display="Static" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <FilterMenu CssClass="RadFilterMenu_CheckList">
                                        </FilterMenu>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>

            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                CausesValidation="False" />

            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="pnlUpdateoverlay"
                DropShadow="false" RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>

            <asp:ModalPopupExtender ID="ReportModalPopupExtender" BackgroundCssClass="ModalPopupBG" BehaviorID="ReportModalPopupExtender"
                runat="server" OkControlID="btnOkay" TargetControlID="btnAddNew" Drag="false"
                PopupControlID="StiReport1" X="5" Y="10">
            </asp:ModalPopupExtender>

            <a href="#" runat="server" value="Add New" id="btnAddNew"></a>

            <div id="StiReport1" class="table-subcategory" style="display: none; width: 1360px; height: 750px; border: groove; border-width: thin; border-color: blue;">
                <div class="fc-input" style="float: right; text-align: right;" id="stiReport1Header" runat="server">
                    <asp:LinkButton ID="lnkCancel" Text="X" runat="server" OnClick="lnkCancel_Click" ForeColor="White"></asp:LinkButton>
                </div>
                <cc1:StiWebDesigner Visible="false" ID="StiWebDesigner1" ViewMode="Continuous" runat="server" OnSaveReport="StiWebDesigner1_SaveReport" Height="700" OnSaveReportAs="StiWebDesigner1_SaveReportAs" OnExit="StiWebDesigner1_Exit" />





                <telerik:RadWindowManager ID="LoadInvoicesModalPopupExtender" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadCreateWindow" Skin="Material" VisibleTitlebar="true" Title="Load Invoices" Behaviors="Default" CenterIfModal="true"
                            Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                            runat="server" Modal="true" Width="600" Height="400">
                            <ContentTemplate>

                                <div style="margin-top: 15px; font-size: 1.1em !important;">
                                    <div class="form-section-row">
                                        <div class="form-section2">
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <label class="drpdwn-label" style="margin-top: -10px !important">Select Invoices from</label>
                                                    <asp:DropDownList ID="ddlInvoicesForLoad" runat="server"
                                                        CssClass="browser-default" TabIndex="2" Width="300">
                                                    </asp:DropDownList>


                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section3-blank">
                                            &nbsp;
                                        </div>




                                    </div>
                                    <div style="clear: both;"></div>

                                    <div class="row">
                                        <div style="float: left; margin-left: 130px">
                                            <div style="margin-top: 30px; width: 15%; float: left">
                                                <asp:LinkButton ID="LinkButton2" Text="Load" OnClick="lnkPDF_Click" runat="server" ForeColor="Blue"></asp:LinkButton>
                                            </div>
                                            <div style="margin-top: 30px; width: 15%; float: left">
                                                <asp:LinkButton ID="LinkButton3" Text="Edit" OnClick="lnkEditInvoices_Click" runat="server" ForeColor="Blue"></asp:LinkButton>
                                            </div>
                                            <div style="margin-top: 30px; width: 15%; float: left">
                                                <asp:LinkButton ID="LinkButton4" Text="Save" OnClick="lnkSaveInvoices_Click" runat="server" Width="100" ForeColor="Blue"></asp:LinkButton>
                                            </div>
                                            <div style="margin-top: 30px; width: 15%; float: left">
                                                <asp:LinkButton ID="LinkButton5" Text="Delete" OnClick="lnkDeleteInvoices_Click" runat="server" Width="100" ForeColor="Blue"></asp:LinkButton>
                                            </div>
                                            <div style="margin-top: 30px; width: 15%; float: left">
                                                <asp:LinkButton ID="LinkButton6" Text="Cancel" OnClick="lnkCancelInvoices_Click" runat="server" Width="100" ForeColor="Blue"></asp:LinkButton>
                                            </div>
                                        </div>


                                        <div class="form-section2">
                                            <div class="input-field col s2">
                                                <div class="row">
                                                    <div class="col-lg-2" style="margin-top: 20px;">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section2">
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        <div class="col-lg-2" style="margin-top: 20px;">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section2">
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            <div class="col-lg-2" style="margin-top: 20px;">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section2">
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                <div class="col-lg-2" style="margin-top: 20px;">
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="form-section-row">
                                                        <div class="row">
                                                            <div class="btnlinks">
                                                                <div class="form-section2">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                            </ContentTemplate>
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>



                <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: 1px solid #316b9d; width: 550px;">
                    <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; background-color: #DDDDDD;">
                        <div class="model-popup-body">
                            <asp:Label CssClass="title_text" ID="Label8" runat="server">Mail Invoice</asp:Label>

                            <asp:LinkButton runat="server" ID="hideModalPopupViaClientButton" Text="Close" OnClick="hideModalPopupViaClientButton_Click"
                                Style="float: right; color: #fff; margin-left: 10px; height: 16px;" />
                            <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                Style="float: right; color: #fff; margin-left: 10px;"
                                ValidationGroup="mail" />
                        </div>
                    </asp:Panel>
                    <div style="padding: 20px;">
                        <table style="width: 100%; height: 400px">
                            <tr>
                                <td>From</td>
                                <td>
                                    <asp:TextBox ID="txtFrom" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
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
                                </td>
                            </tr>
                            <tr>
                                <td>To</td>
                                <td>
                                    <asp:TextBox ID="txtTo" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
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
                                </td>
                            </tr>
                            <tr>
                                <td>CC</td>
                                <td>
                                    <asp:TextBox ID="txtCC" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
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
                                </td>
                            </tr>
                            <tr>
                                <td>Subject</td>
                                <td>
                                    <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Attachment</td>
                                <td>
                                    <asp:HiddenField ID="hdnattcahment" runat="server" />
                                    <asp:LinkButton ID="lnkattachment" runat="server" OnClick="lnkattachment_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtBody" CssClass="form-control" runat="server" TextMode="MultiLine"
                                        Height="200px" Width="450px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>

            </div>

        </div>

        <%--   <div id="loaInvoices" class="table-subcategory" style="display: none; width: 600px; height: 300px; border: groove; background:#fff; border-width: thin; border-color: white;">
                                <div class="popup_Container">
                                    <div class="popup_Body">
                                        <div class="model-popup-body" style="padding-bottom: 24px; height: 350px; background:#fff">
                                            <div class="col-lg-12 col-md-12">
                                                <div class="pc-title" style="height: 40px; margin-bottom:10px" >
                                                    <div class="col-lg-4"></div>
                                                          <asp:Label CssClass="title_text" Style="float: left" ID="Label5" runat="server">Invoices</asp:Label>
                                                     </div>
                                                   <div class="fc-input">
                                                    <asp:DropDownList ID="ddlInvoicesForLoad" runat="server"
                                                        CssClass="form-control" TabIndex="4" Width="300" OnSelectedIndexChanged="ddlInvoicesForLoad_SelectedIndexChanged" >
                                                    </asp:DropDownList>
                                                   <%-- <div class="col-lg-4"></div>--%>

        <div class="col-lg-2" style="margin-top: 20px;">
            <asp:LinkButton ID="lnkLoadInvoices" Text="Load" Width="100" OnClick="lnkLoadInvoices_Click" runat="server" ForeColor="Blue"></asp:LinkButton>
        </div>

        <div class="col-lg-2" style="margin-top: 20px;">
            <asp:LinkButton ID="lnkEditInvoices" Text="Edit" OnClick="lnkEditInvoices_Click" runat="server" Width="100" ForeColor="Blue"></asp:LinkButton>
        </div>

        <div class="col-lg-2" style="margin-top: 20px;">
            <asp:LinkButton ID="lnkSaveInvoices" Text="Save" OnClick="lnkSaveInvoices_Click" runat="server" Width="100" ForeColor="Blue"></asp:LinkButton>
        </div>

        <div class="col-lg-2" style="margin-top: 20px;">
            <asp:LinkButton ID="lnkDeleteInvoices" Text="Delete" OnClick="lnkDeleteInvoices_Click" runat="server" Width="100" ForeColor="Blue"></asp:LinkButton>
        </div>

        <div class="col-lg-2" style="margin-top: 20px;">
            <asp:LinkButton ID="lnkCancelInvoices" Text="Cancel" OnClick="lnkCancelInvoices_Click" runat="server" Width="100" ForeColor="Blue"></asp:LinkButton>
        </div>

    </div>
    <%--</div>--%>

    <%-- </div>--%>
    <%-- </div>--%>
    <%--  </div>--%>
    <%--  </div>--%>


    <%-- </div>--%>
    <asp:HiddenField runat="server" ID="hdnInvoiceSelectDtRange" Value="Week" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />    
      <asp:HiddenField runat="server" ID="isShowAll" Value="0" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <%--  <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>--%>
    <script>
       
    </script>
    <script type="text/javascript" src="js/jquery.inputmask.bundle.min.js"></script>
    <script type="text/javascript">
        function CssClearLabel() {
            //debugger
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
        function displyDeleteAlert(mesg) {
            noty({
                text: 'These month/year period is closed out. You do not have permission to ' + mesg + ' this record.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
         function VoidedInvoiceInProjectClose() {
            noty({
                text: 'This invoice cannot be voided because the project is not opened. ',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
         function EditInvoiceInProjectClose() {
            noty({
                text: 'This invoice cannot be edit because the project is not opened. ',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function DeleteInvoiceInProjectClose () {
            noty({
                text: 'This invoice cannot be deleted because the project is not opened.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
         function WipInvWarning() {
            noty({
                text: 'This is a WIP invoice please delete from the project screen.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function PaidInvWarning() {
            noty({
                text: 'This invoice is not open and can therefore not be voided.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function ClosedInvoice(ref) {
            noty({ text: 'Invoice #' + ref + ' is not open and can therefore not be deleted.', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 5000, theme: 'noty_theme_default', closable: true });
        }
        function SelectedRow(gridview, message) {
            var grid = document.getElementById(gridview);
            var cell;
            var cellName;
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    cellName = grid.rows[i].cells[1];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            if (cell.childNodes[j].checked == true) {
                                return;
                            }
                        }
                    }
                }
            }
            alert('Please select ' + message + '.');
            return false;
        }

        function HoverMenutext(row, tooltip, event) {
            var left = event.pageX + (-370) + 'px';
            var top = event.pageY + (-135) + 'px';
            $('#' + tooltip).css({ top: top, left: left }).show();
        }



        $(document).ready(function () {



            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

            $('#colorNav #dynamicUI li').remove();
            //efficient-&-compact JQuery way
            $(reports).each(function (index, report) {
                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }
                $('#dynamicUI').append('<li><a href="InvoiceListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Invoices"><span> <img src=images/reportfolder.png>  ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')
            });

            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();

                var target = this.hash;
                var $target = $(target);
                if ($(target).hasClass('active') || target == "") {

                }
                else {
                    $(target).click();
                }

                $('html, body').stop().animate({
                    'scrollTop': $target.offset().top - 125
                }, 900, 'swing');
            });
        });
    </script>
   
    <script type="text/javascript">
        function dec_date(select, txtDateTo, txtDateFrom, rdGroup) {
            var select = select;
            var txtDateTo = txtDateTo;
            var txtDateFrom = txtDateFrom;
            var rdGroup = rdGroup;
            var xday;
            var xWeek;
            var xMonth;
            var xYear;
            var xQuarter;
            if (select == "dec") {
                xday = -1;
                xWeek = -7;
                xMonth = -1;
                xQuarter = -3;
                xYear = -1;
            }
            if (select == "inc") {

                xday = 1;
                xWeek = 7;
                xMonth = 1;
                xQuarter = 3;
                xYear = 1;
            }
            var radio = document.getElementsByName(rdGroup); //Client ID of the RadioButtonList1 
            var selected ="rd"+ $("#<%=hdnInvoiceSelectDtRange.ClientID%>").val(); 
            //for (var i = 0; i < radio.length; i++) {
            //    if (radio[i].checked) { // Checked property to check radio Button check or not
            //        //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
            //        selected = radio[i].value;

            //    }
               
            //}

            if (document.getElementById(txtDateFrom).value == "") {
                return false;
            } 
             if (selected == "rd" ) {
                    selected = 'rdWeek';
                }
            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdMonth') {
                //dec the from date

                Date.isLeapYear = function (year) {
                    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
                };

                Date.getDaysInMonth = function (year, month) {
                    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
                };

                Date.prototype.isLeapYear = function () {
                    return Date.isLeapYear(this.getFullYear());
                };

                Date.prototype.getDaysInMonth = function () {
                    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
                };

                Date.prototype.addMonths = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + value);
                    this.setDate(Math.min(n, this.getDaysInMonth()));
                    return this;
                };

                Date.prototype.addMonthsLast = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };

                Date.prototype.addMonthsLastDec = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() - 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateTo).value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById(txtDateTo).value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }
            }


            else if (selected == 'rdQuarter') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                //decrease date range 
                if (select == 'dec') {
                    xQuarter = -3;

                    if (DATE.getMonth() == 11) {
                        NEWDATE.setDate(NEWDATE.getDate() - 1);
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }
                    else if (DATE.getMonth() == 5) {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                        NEWDATE.setDate(NEWDATE.getDate() + 1);

                    }
                    else {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }

                }
                else {
                    xQuarter = 3;
                    NEWDATE.setDate(NEWDATE.getDate() - 1);
                    NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    if (NEWDATE.getMonth() == 11 || NEWDATE.getMonth() == 12 || DATE.getMonth() == 11) {
                        NEWDATE.setDate(31);
                    } else {
                        if (DATE.getMonth() == 5) { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else { NEWDATE.setDate(NEWDATE.getDate()); }

                    }
                }
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }

            return false;

        }
        function SelectDate(type ) {
            $("#<%=lblDay.ClientID%>").removeClass("labelactive");
            $("#<%=lblWeek.ClientID%>").removeClass("labelactive");
            $("#<%=lblMonth.ClientID%>").removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
             $("#<%=lblYear.ClientID%>").removeClass("labelactive");
           // debugger
            var type = type;           
            var UniqueVal = "hdnInvoiceDate";
            var label = label;
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                $("#<%=txtFromDate.ClientID%>").val(datestring);
                $("#<%=txtToDate.ClientID%>").val(datestring);
               // document.getElementById(txtdateTo).value = datestring;
               // document.getElementById(txtDateFrom).value = datestring;
                $("#<%=lblDay.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnInvoiceSelectDtRange.ClientID%>').value = "Day";
            }
            if (type == 'Week') {

                Date.prototype.GetFirstDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay())));
                }

                Date.prototype.GetLastDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay() + 6)));
                }
                var today = new Date();
                var Firstdate = today.GetFirstDayOfWeek();
                var day = Firstdate.getDate();
                var month = Firstdate.getMonth() + 1;
                var year = Firstdate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                //document.getElementById(txtDateFrom).value = datestring;
                 $("#<%=txtFromDate.ClientID%>").val(datestring);
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                //document.getElementById(txtdateTo).value = dateString;
                
                $("#<%=txtToDate.ClientID%>").val(dateString);
                $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnInvoiceSelectDtRange.ClientID%>').value = "Week";
            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);
                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                  $("#<%=txtFromDate.ClientID%>").val(datestring);
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                 $("#<%=txtToDate.ClientID%>").val(dateString);
                 $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnInvoiceSelectDtRange.ClientID%>').value = "Month";
            }
            if (type == 'Quarter') {
                var d = new Date();
                var quarter = Math.floor((d.getMonth() / 3));
                var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
                var lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);
                var day = firstDate.getDate();
                var month = firstDate.getMonth() + 1;
                var year = firstDate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                $("#<%=txtFromDate.ClientID%>").val(datestring);
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
               $("#<%=txtToDate.ClientID%>").val(dateString);
                $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnInvoiceSelectDtRange.ClientID%>').value = "Quarter";
            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);
                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
             $("#<%=txtFromDate.ClientID%>").val(datestring);
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
              $("#<%=txtToDate.ClientID%>").val(dateString);
                 $("#<%=lblYear.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnInvoiceSelectDtRange.ClientID%>').value = "Year";
            }
          
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, type);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
           var clickSearchButton = document.getElementById("<%= lnkSearchFilter.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
    <script>      

        function showFilterSearchHistory() {
            debugger
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var ddlStatus = $("#<%=ddlStatus.ClientID%>");
            var ddlDepartment = $("#<%=ddlDepartment.ClientID%>");
            var ddllocation = $("#<%=ddllocation.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            var txtInvDt = $("#<%=txtInvDt.ClientID%>");
            ddlStatus.css("display", "none");
            ddlDepartment.css("display", "none");
            ddllocation.css("display", "none");
            txtSearch.css("display", "none");
            txtInvDt.css("display", "none");
            txtSearch.val('');
            txtInvDt.val('');           
           
           


            if (ddlSearch.val() === "i.Type") {              
                ddlDepartment.css("display", "block");                          
            }
            else if (ddlSearch.val() === "i.Status") {   
                ddlStatus.css("display", "block");              
            }
            else if (ddlSearch.val() === "i.fdate") {   
                txtInvDt.css("display", "block");    
            }
            else if (ddlSearch.val() === "l.loc") {                   
                ddllocation.css("display", "block");            
            }else {                
                txtSearch.css("display", "block");
               
            }
             try {
                 ddlStatus.get(0).selectedIndex = 0;
            ddlDepartment.get(0).selectedIndex = 0;
            ddllocation.get(0).selectedIndex = 0;
            } catch (ex) {

            }
        }
        function ResetValueAll() {          
              $("#<%=lblDay.ClientID%>").removeClass("labelactive");
            $("#<%=lblWeek.ClientID%>").removeClass("labelactive");
            $("#<%=lblMonth.ClientID%>").removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
             $("#<%=lblYear.ClientID%>").removeClass("labelactive");
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var ddlStatus = $("#<%=ddlStatus.ClientID%>");
            var ddlDepartment = $("#<%=ddlDepartment.ClientID%>");
            var ddllocation = $("#<%=ddllocation.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            var txtInvDt = $("#<%=txtInvDt.ClientID%>");
            ddlStatus.css("display", "none");
            ddlDepartment.css("display", "none");
            ddllocation.css("display", "none");
            txtSearch.css("display", "block");
            txtInvDt.css("display", "none");
            txtSearch.val('');
            txtInvDt.val('');
            try {
                ddlSearch.get(0).selectedIndex = 0;
                ddlStatus.get(0).selectedIndex = 0;
                ddlDepartment.get(0).selectedIndex = 0;
                ddllocation.get(0).selectedIndex = 0;
            } catch (ex) {}
           
            
           
           $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                     
        }
        function ResetValue() {
            $("#<%=lblDay.ClientID%>").removeClass("labelactive");
             $("#<%=lblWeek.ClientID%>").removeClass("labelactive");
             $("#<%=lblMonth.ClientID%>").removeClass("labelactive");
             $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
             $("#<%=lblYear.ClientID%>").removeClass("labelactive");
             var ddlSearch = $("#<%=ddlSearch.ClientID%>");
             var ddlStatus = $("#<%=ddlStatus.ClientID%>");
             var ddlDepartment = $("#<%=ddlDepartment.ClientID%>");
             var ddllocation = $("#<%=ddllocation.ClientID%>");
             var txtSearch = $("#<%=txtSearch.ClientID%>");
             var txtInvDt = $("#<%=txtInvDt.ClientID%>");
             ddlStatus.css("display", "none");
             ddlDepartment.css("display", "none");
             ddllocation.css("display", "none");
             txtSearch.css("display", "block");
             txtInvDt.css("display", "none");
             txtSearch.val('');
             txtInvDt.val('');
             try {
                 ddlSearch.get(0).selectedIndex = 0;
                 ddlStatus.get(0).selectedIndex = 0;
                 ddlDepartment.get(0).selectedIndex = 0;
                 ddllocation.get(0).selectedIndex = 0;
             } catch (ex) { }                       

             SelectDate(document.getElementById('<%= hdnInvoiceSelectDtRange.ClientID%>').value)

        }

        function validateValue() {
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            
            if (ddlSearch.val() === "i.ref") {              
                var ls = txtSearch.val();

            }
        }
         $(document).ready(function () {

             $('label input[type=radio]').click(function () {
                $('input[name="rdCal"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
          
        });
        function pageLoad(sender, args) {
           // debugger
            $('label input[type=radio]').click(function () {
                $('input[name="rdCal"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
             $("#<%=txtFromDate.ClientID %>").pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });
               $("#<%=txtToDate.ClientID %>").pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });
            $("#<%=txtInvDt.ClientID %>").pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });
        }
    </script>
    <script>
          function OnGridCreated(sender, args) {
            //alert('OnGridCreated');
            var frozenScroll = $get(sender.get_id() + "_Frozen");
            var allColumns = sender.get_masterTableView().get_columns();
            var scrollLeftOffset = 0;
            var allColumnsWidth = new Array;
            var grid = sender.get_element();
            for (var i = 0; i < allColumns.length; i++) {
                allColumnsWidth[i] = allColumns[i].get_element().offsetWidth;
            }

            $get(sender.get_id() + "_GridData").onscroll = function (e) {
                for (var i = 0; i < allColumns.length; i++) {
                    if (!allColumns[i].get_visible()) {
                        scrollLeftOffset += allColumnsWidth[i];
                    }
                    if ($telerik.isIE7) {
                        var thisColumn = grid.getElementsByTagName("colgroup")[0].getElementsByTagName("col")[i];
                        if (thisColumn.style.display == "none") {
                            scrollLeftOffset += parseInt(thisColumn.style.width);
                        }
                    }
                }
                var thisScrollLeft = this.scrollLeft;
                if (frozenScroll != null) {
                    if (thisScrollLeft > 0)
                        frozenScroll.scrollLeft = thisScrollLeft + scrollLeftOffset + 300;
                    this.scrollLeft = 0;
                }

                scrollLeftOffset = 0;
            };
        }

        function headerMenuShowing(sender, args) {
            var session = '<%= Session["COPer"] %>';
           
            var menu = args.get_menu();

            for (var i = 0; i < menu.get_items().get_count(); i++) {
                var item = menu.get_items().getItem(i);
                if (item.get_value() != 'ColumnsContainer') {
                    item.get_element().style.display = 'none';
                }
            }

            var columnsItem = menu.findItemByText("Columns");
            columnsItem.get_items().getItem(0).get_element().style.display = "none";
           // columnsItem.get_items().getItem(1).get_element().style.display = "none";
           // columnsItem.get_items().getItem(2).get_element().style.display = "none";
            if (session != 1) {
                columnsItem.get_items().getItem(7).get_element().style.display = "none";
            }
        }

    function ColumnSettingsChange(menu, args) {
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "block";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
        }

        function GridCommand(sender, args) {
            if (args.get_commandName() == "Sort") {
                ColumnSettingsChange();
            }
        }

        function SaveGridSettings() {
            document.getElementById('<%=lnkSaveGridSettings.ClientID%>').click();
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }

        function RestoreGridSettings() {
            document.getElementById('<%=lnkRestoreGridSettings.ClientID%>').click();
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
        }
        
      


       
    </script>
</asp:Content>

