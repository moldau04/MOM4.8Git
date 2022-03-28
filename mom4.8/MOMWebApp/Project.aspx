<%@ Page Title="Projects || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Project" EnableEventValidation="false" CodeBehind="Project.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.9/angular.min.js"></script>
    <style>
        div.RadFilterMenu_CheckList {
            height: 350px !important;
        }

            div.RadFilterMenu_CheckList .RadListBox {
                height: 300px !important;
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

        .labelButton {
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
        .RadGrid_Material th
        a {
            padding: 0px!important;
            font-size: 0.75rem!important;
        }
       
        .rgHeader {
            padding: 15px 8px!important;
        }
    </style>
    <script type="text/javascript">
        function AddJobClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeJob.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function EditJobClick(hyperlink) {
            var id = document.getElementById('<%= hdnEditeJob.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function DeleteJobClick(hyperlink) {
            var projectId = $("[id$='ClientSelectColumnSelectCheckBox']:checked").closest("tr").find("[id$='lnkJob']").text();

            var id = document.getElementById('<%= hdnDeleteJob.ClientID%>').value;
            if (id === "Y") {
                if (projectId !== "") {
                    if (!confirm("Are you sure you want to delete project #" + projectId)) {
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return false;
                }
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function RLEJobClick(hyperlink) {
            if (!confirm("Are you sure you want to Proceed ")) {
                return false;
            }
            else {
                return true;
            }
        }

        function CheckJobAddPer() {
            var Editjob = document.getElementById('<%= hdnEditeJob.ClientID%>').value;
            var viewjob = document.getElementById('<%= hdnViewJob.ClientID%>').value;
            if (Editjob == "Y" || viewjob == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function CheckExistData() {
            var result = false;
            var hdnCheckExistData = $(".RadGrid_Project .rgNoRecords").length;

            if (hdnCheckExistData === 0) {
                result = true;
            }
            else {
                alert("Nothing to export.");
                result = false;
            }
            return result;
        }
    </script>
    <script>
        function ShowRestoreGridSettingsButton() {
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }

        function OnGridCreated(sender, args) {
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
            var menu = args.get_menu();

            for (var i = 0; i < menu.get_items().get_count(); i++) {
                var item = menu.get_items().getItem(i);
                if (item.get_value() != 'ColumnsContainer') {
                    item.get_element().style.display = 'none';
                }
            }

            var columnsItem = menu.findItemByText("Columns");
            columnsItem.get_items().getItem(0).get_element().style.display = "none";
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
            document.getElementById("overlay").style.display = "block";
            document.getElementById('<%=lnkSaveGridSettings.ClientID%>').click();
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }

        function RestoreGridSettings() {
            document.getElementById("overlay").style.display = "block";
            document.getElementById('<%=lnkRestoreGridSettings.ClientID%>').click();
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
        }
    </script>
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function pageLoad() {
                $('#ctl00_ContentPlaceHolder1_ddlDateRange').change(function () { fddldate(); });
                $('#ctl00_ContentPlaceHolder1_ddlSearch').change(function () { ddlSearch(); });
            }

            function dllsearch() {
                if ($('#ctl00_ContentPlaceHolder1_ddlStatus').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlStatus').style.display = "none";
                }

                if ($('#ctl00_ContentPlaceHolder1_txtSearch').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_txtSearch').style.display = "none";
                }

                if ($('#ctl00_ContentPlaceHolder1_ddlProgressBilling').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlProgressBilling').style.display = "none";
                }

                if ($('#ctl00_ContentPlaceHolder1_ddlCertified').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlCertified').style.display = "none";
                }

                if ($('#ctl00_ContentPlaceHolder1_ddlTeamTitle').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_ddlTeamTitle').style.display = "none";
                }

                if ($('#ctl00_ContentPlaceHolder1_txtSearchTeamMember').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_txtSearchTeamMember').style.display = "none";
                }

                var SelectedValue = $('#ctl00_ContentPlaceHolder1_ddlSearch').val();

                if (SelectedValue == "j.Status") {
                    //ddlStatus.Visible = true;
                    //txtSearch.Visible = false;                      
                    //ddlProgressBilling.Visible = false;
                    //ddlCertified.Visible = false; 
                    //ddlTeamTitle.Visible = false;
                    //txtSearchTeamMember.Visible = false;

                    if ($('#ctl00_ContentPlaceHolder1_ddlStatus').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_ddlStatus').style.display = "block";
                    }
                }
                if (SelectedValue == "j.PWIP") {
                    //ddlProgressBilling.Visible = true;
                    //ddlStatus.Visible = false;
                    //txtSearch.Visible = false;                     
                    //ddlCertified.Visible = false;
                    //ddlTeamTitle.Visible = false;
                    //txtSearchTeamMember.Visible = false;

                    if ($('#ctl00_ContentPlaceHolder1_ddlProgressBilling').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_ddlProgressBilling').style.display = "block";
                    }
                }

                if (SelectedValue == "j.id" || SelectedValue == "l.tag" || SelectedValue == "l.City" || SelectedValue == "l.State" || SelectedValue == "j.fdesc" || SelectedValue == "r.Name"
                    || SelectedValue == "j.Custom1"
                    || SelectedValue == "j.Custom2"
                    || SelectedValue == "j.Custom3"
                    || SelectedValue == "j.Custom4"
                    || SelectedValue == "j.Custom5"
                    || SelectedValue == "j.Custom6"
                    || SelectedValue == "j.Custom7"
                    || SelectedValue == "j.Custom8"
                    || SelectedValue == "j.Custom9"
                    || SelectedValue == "j.Custom10"
                    || SelectedValue == "j.Custom11"
                    || SelectedValue == "j.Custom12"
                    || SelectedValue == "j.Custom13"
                    || SelectedValue == "j.Custom14"
                    || SelectedValue == "j.Custom15"
                    || SelectedValue == "j.Custom16"
                    || SelectedValue == "j.Custom17"
                    || SelectedValue == "j.Custom18"
                    || SelectedValue == "j.Custom19"
                    || SelectedValue == "j.Custom20"
                    || SelectedValue == "j.Custom21"
                    || SelectedValue == "j.Custom22"
                    || SelectedValue == "j.Custom23"
                    || SelectedValue == "j.Custom24"
                    || SelectedValue == "j.Custom25"
                    || SelectedValue == "") {

                    if ($('#ctl00_ContentPlaceHolder1_txtSearch').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_txtSearch').style.display = "block";
                    }
                }

                if (SelectedValue == "j.id") {
                    //ddlDateRange.SelectedIndex = 0;
                    //txtfromDate.Visible = false;
                    //txtToDate.Visible = false;
                    //txtfromDate.Text = txtToDate.Text = "";
                    //ddlProgressBilling.Visible = false;
                    //ddlCertified.Visible = false;
                    //ddlTeamTitle.Visible = false;
                    //txtSearchTeamMember.Visible = false;

                    if ($('#ctl00_ContentPlaceHolder1_txtSearch').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_txtSearch').style.display = "block";
                    }
                }

                if (SelectedValue == "j.Certified") {
                    //ddlProgressBilling.Visible = false;
                    //ddlStatus.Visible = false;
                    //txtSearch.Visible = false;                      
                    //ddlCertified.Visible = true;

                    if ($('#ctl00_ContentPlaceHolder1_ddlCertified').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_ddlCertified').style.display = "block";
                    }
                }

                if (SelectedValue == "t.title") {
                    //ddlTeamTitle.Visible = true;
                    //txtSearchTeamMember.Visible = true;

                    if ($('#ctl00_ContentPlaceHolder1_ddlTeamTitle').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_ddlTeamTitle').style.display = "block";
                    }

                    if ($('#ctl00_ContentPlaceHolder1_txtSearchTeamMember').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_txtSearchTeamMember').style.display = "block";
                    }
                }
            }

            function fddldate() {
                var SelectedValue = $('#ctl00_ContentPlaceHolder1_ddlDateRange').val();
                var sessiontype = $('#ctl00_ContentPlaceHolder1_hdnsessiontype').val();

                if ($('#ctl00_ContentPlaceHolder1_txtfromDate').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_txtfromDate').style.display = "none";
                    document.getElementById('ctl00_ContentPlaceHolder1_txtToDate').style.display = "none";
                }

                if ($('#ctl00_ContentPlaceHolder1_lnkLaborExpenses').length) {
                    document.getElementById('ctl00_ContentPlaceHolder1_lnkLaborExpenses').style.display = "none";
                }

                if (sessiontype == "am" && SelectedValue == "2" || SelectedValue == "5") {
                    if ($('#ctl00_ContentPlaceHolder1_lnkLaborExpenses').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_lnkLaborExpenses').style.display = "block";
                    }
                }

                if (SelectedValue == "2" || SelectedValue == "3" || SelectedValue == "4" || SelectedValue == "5") {
                    if ($('#ctl00_ContentPlaceHolder1_txtfromDate').length) {
                        document.getElementById('ctl00_ContentPlaceHolder1_txtfromDate').style.display = "block";
                        document.getElementById('ctl00_ContentPlaceHolder1_txtToDate').style.display = "block";
                    }
                }
            }
        </script>
    </telerik:RadCodeBlock>

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
        <img src="images/wheel.GIF" class="lodder" alt="Be patient..." />
    </div>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-contacts"></i>&nbsp;
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Project</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:HyperLink ID="lnkAdd" runat="server" NavigateUrl="addproject.aspx" OnClick='return AddJobClick(this)' Target="_self">Add</asp:HyperLink>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton OnClientClick='return EditJobClick(this)' ID="lnkEdit" runat="server" CausesValidation="False" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>
                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks hide">
                                                    <asp:LinkButton ID="lnkCopy" runat="server" CausesValidation="False" OnClick="lnkCopy_Click">Copy</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton OnClientClick='return DeleteJobClick(this)' ID="lnkDelete" runat="server" CausesValidation="False" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <ul id="dropdown1" class="dropdown-content">
                                                    <li>
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkExporttoExcelCustomLabel" OnClientClick="return CheckExistData();" runat="server" OnClick="lnkExporttoExcelCustomLabel_Click">Export to Excel Custom Label</asp:LinkButton>
                                                    </li>
                                                </ul>
                                                <div class="btnlinks">
                                                    <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown1">Export To Excel
                                                    </a>
                                                </div>
                                            </li>

                                            <li>
                                                <div class="btnlinks">
                                                    <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dynamicUI">Reports
                                                    </a>
                                                </div>
                                                <ul id="dynamicUI" class="dropdown-content">
                                                    <li>
                                                        <a target="_blank" href="ProjectListingReport.aspx?type=Projects">Add New Report</a>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkDelivered" PostBackUrl="~/DeliveredNoDeliveryPaymentReport.aspx?type=Project" runat="server">Delivered No Delivery Payment Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkInspected" PostBackUrl="~/InspectedNoFinalPaymentReport.aspx?type=Delivered" runat="server">Inspected No Final Payment Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkDcaApproval" PostBackUrl="~/DCALocalApprovalNoPermit.aspx?type=Approval" runat="server">DCA & Local Approval No Permit</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkApprovedByDca" PostBackUrl="~/ProjectApprovedByDCA.aspx" runat="server">Project Approved by DCA</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkDrawing" PostBackUrl="~/DrawingsSentNotRcvdBack.aspx?type=Drawings" runat="server">Drawings Sent Not Rcvd Back</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkReDrawing" PostBackUrl="~/ReDrawingsSubmittedForApproval.aspx?type=ReDrawings" runat="server">Re-Drawings Submitted for Approval</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkOpenJob" Visible="false" PostBackUrl="~/OpenJobReport.aspx?type=OpenJob" runat="server">Open Job Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkOutofWarrantyreport" Visible="false" PostBackUrl="~/OutofWarrantyreport.aspx" runat="server">Out of Warranty Report</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkUnitInspectedReport" PostBackUrl="~/UnitInspectedTrimNotCompleteReport.aspx?type=Approval" runat="server">Unit Inspected Trim Not Complete Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkUnitFinishedReport" PostBackUrl="~/UnitFinishedTrimNotCompleteReport.aspx?type=Drawings" runat="server">Unit Finished Trim Not Complete Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkSubstantialCompleteDeliveryNotPaidReport" Visible="false" PostBackUrl="~/SubstantialCompletionDeliveryNotPaidReport.aspx?type=OpenJob" runat="server">Substantial Complete Delivery Not Paid Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkSubstantialCompleteFinalNotPaidReport" Visible="false" PostBackUrl="~/SubstantialCompletionFinalNotPaidReport.aspx" runat="server">Substantial Complete Final Not Paid Report</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <a href="#" onserverclick="lnkactualVsbudgeted_Click" runat="server">Project Actual Vs Budget Report</a>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkProjectActualVsBudgetReportGable" runat="server" OnClick="lnkProjectActualVsBudgetReportGable_Click" Visible="false">Project Actual Vs Budget Report-Gable</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <a href="#" onserverclick="lnkJobSummary_Click" runat="server">Project Summary Listing Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="ContractListingByRoute.aspx" runat="server">Contract Listing by Route Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="ContractListingByRouteActiveOnly.aspx" runat="server">Contract Listing by Route Active Only Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="ProjectListingByRouteWithBudgetedHours.aspx" runat="server">Project Listing by Route with Budgeted Hours Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="#" onserverclick="lnkJobWithBudgets_Click" runat="server">Project Backlog with Budgets Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="#" onserverclick="lnkProjectBacklog_Click" runat="server">Project Backlog Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="#" onserverclick="lnkProjectVendorByProject_Click" runat="server">Project Vendor Demand by Project</a>
                                                    </li>
                                                    <li>
                                                        <a href="#" onserverclick="lnkProjectVendorByVendor_Click" runat="server">Project Vendor Demand by Vendor</a>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkProjectSummaryReport" runat="server" OnClick="lnkProjectSummaryReport_Click">Project Summary Report</asp:LinkButton>
                                                    </li>
                                                      <li>
                                                        <asp:LinkButton ID="lnkProjectSummarybyProjectManagerReport" runat="server" OnClick="lnkProjectSummarybyProjectManagerReport_Click">Project Summary By Project Manager Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkProjectLaborCostReport" runat="server" OnClick="lnkProjectLaborCostReport_Click">Project Labor Cost Report</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                        <div class="btnlinks m-t-3">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="lnkLaborExpenses" OnClientClick='return RLEJobClick(this)' runat="server" CausesValidation="False" Style="line-height: 1.23;" OnClick="lnkLaborExpenses_Click">Recalculate Labor expenses</asp:LinkButton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkWIP" runat="server" CausesValidation="False" OnClick="lnkWIP_Click">WIP</asp:LinkButton>
                                        </div>

                                        <ul id="drpMenuGrid" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkRestoreGridSettings" runat="server" CausesValidation="False" OnClick="lnkRestoreGridSettings_Click"
                                                        Style="display: none">Restore Grid</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkSaveGridSettings" runat="server" CausesValidation="False" OnClick="lnkSaveGridSettings_Click"
                                                        Style="display: none">Save Grid</asp:LinkButton>
                                                </div>

                                                <label id="lbSaveGridSettings" runat="server" class="labelButton" tooltip="Save Grid Settings" style="display: none">
                                                    <input type="radio" id="rdSaveGridSettings" onclick="SaveGridSettings();" />
                                                    Save Grid
                                                </label>
                                                <label id="lbRestoreGridSettings" runat="server" class="labelButton" tooltip="Restore Default Settings for Grid" style="display: none">
                                                    <input type="radio" id="rdRestoreGridSettings" onclick="RestoreGridSettings();" />
                                                    Restore Grid
                                                </label>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>
    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="srchpane">

                        <telerik:RadAjaxPanel ID="RadAjaxPanelTicketInfo" runat="server">
                            <div class="srchtitle pad-le-15" >
                                Search
                            </div>
                            <div class="srchinputwrap">

                                <%-- OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"--%>

                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default select selectst" AutoPostBack="false" onchange="dllsearch(); return false">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default select selectst" AutoPostBack="false">
                                    <asp:ListItem Value="0">Open</asp:ListItem>
                                    <asp:ListItem Value="1">Closed</asp:ListItem>
                                    <asp:ListItem Value="2">Hold</asp:ListItem>
                                    <asp:ListItem Value="3">Completed</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtInvDt" Visible="false" runat="server" CssClass="srchcstm datepicker_mom" placeholder="Date"></asp:TextBox>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search"></asp:TextBox>
                                <asp:DropDownList ID="ddlProgressBilling" runat="server" CssClass="browser-default select selectst" AutoPostBack="false">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlCertified" runat="server" CssClass="browser-default select selectst" AutoPostBack="false">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlTeamTitle" runat="server" CssClass="browser-default select selectst" AutoPostBack="false">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchTeamMember" runat="server" CssClass="srchcstm" placeholder="Search by team member"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <%--OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged"--%>
                                <asp:DropDownList ID="ddlDateRange" runat="server" CssClass="browser-default select selectst" AutoPostBack="false" onchange="fddldate(); return false">
                                    <asp:ListItem Value="1">Cumulative</asp:ListItem>
                                    <asp:ListItem Value="2">Date Range</asp:ListItem>
                                    <asp:ListItem Value="5">Date Range - Activity</asp:ListItem>
                                    <asp:ListItem Value="3">Date Range - Closed</asp:ListItem>
                                    <asp:ListItem Value="4">Date Range - Created</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="srchcstm datepicker_mom daterage" placeholder="Date From" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm datepicker_mom daterage" placeholder="Date To" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap srchclr btnlinksicon srch-m " >
                                <asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server"><i class="mdi-action-search"></i></asp:LinkButton>
                            </div>
                            <div class="col lblsz2 lblszfloat">
                                <div class="row">
                                    <span class="tro trost accrd-trost">
                                        <asp:CheckBox ID="lnkChk" CssClass="filled-in" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
                                        <asp:Label ID="lblChkSelect" AssociatedControlID="lnkChk" CssClass="title-check-text" runat="server">Incl. Closed</asp:Label>
                                    </span>
                                    <span class="tro trost accrd-trost">
                                        <asp:LinkButton ID="lnkClear" OnClick="lnkClear_Click" runat="server">Clear</asp:LinkButton>
                                    </span>
                                    <span class="tro trost accrd-trost">
                                        <asp:LinkButton ID="lnkShowAll" OnClick="lnkShowAll_Click" runat="server">Show All Projects</asp:LinkButton>
                                    </span>
                                    <span class="tro trost accrd-trost">

                                        <span>
                                            <asp:Label ID="lblRecordCount" runat="server"></asp:Label></span>

                                    </span>
                                </div>
                            </div>
                            <div class="cf"></div>
                        </telerik:RadAjaxPanel>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="container accordian-wrap">
        <div class="row">
            <asp:HiddenField ID="hdnTabId" runat="server" />
            <ul class="tabs tab-demo-active white tabProject w-100p" id="tabProject" >
                <asp:Repeater ID="rptDepartmentTab" runat="server">
                    <ItemTemplate>

                        <li class="tab col s2 " style="font-size: 13px !important">
                            <a class="waves-effect waves-light prodept" title='<%# Eval("type") %>' id="<%# Eval("ID") %>" href="#activeone" onclick='selectTab("<%# Eval("ID") %>")'>&nbsp;<%# Eval("type") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <br />
            <div id="activeone" class="tab-container-border lighten-4" style="display: block;">
                <div class="grid_container">
                    <div class="form-section-row m-b-0" >
                        <telerik:RadAjaxManager ID="RadAjaxManager_Project" runat="server">
                            <AjaxSettings>
                                <telerik:AjaxSetting AjaxControlID="lnkChk">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="lnkClear">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" />
                                        <telerik:AjaxUpdatedControl ControlID="ddlDateRange" />
                                        <telerik:AjaxUpdatedControl ControlID="txtfromDate" />
                                        <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="RadGrid_Project">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="lnkLaborExpenses">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                            </AjaxSettings>
                        </telerik:RadAjaxManager>
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Project" runat="server">
                        </telerik:RadAjaxLoadingPanel>
                        <div class="RadGrid RadGrid_Material">
                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdDept" Value="-1" />
                            <telerik:RadCodeBlock ID="RadCodeBlock_Project" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= RadGrid_Project.ClientID %>");
                                        var columns = grid.get_masterTableView().get_columns();
                                        for (var i = 0; i < columns.length; i++) {
                                            columns[i].resizeToFit(false, true);
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
                                            if (selectionStart != null)
                                                element.selectionStart = selectionStart;
                                        }
                                    }

                                    function RowDblClick(sender, eventArgs) {
                                        if ($("#<%=hdnEditeJob.ClientID %>").val() == "Y" || $("#<%=hdnViewJob.ClientID %>").val() == "Y") {
                                            var selectColumnID = eventArgs.get_gridDataItem().get_element().cells[0].firstChild.id;
                                            var jobColumnId = selectColumnID.replace('ClientSelectColumnSelectCheckBox', 'lnkJob')
                                            var jobId = document.getElementById(jobColumnId).innerText;
                                            window.open('addProject.aspx?uid=' + jobId, '_self');
                                        }
                                        else {
                                            noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                                        }
                                    }

                                    function MenuShowing(menu, args) {
                                        // Iterate through filter menu items.
                                        var items = menu.get_items();
                                        for (i = 0; i < items.get_count(); i++) {
                                            var item = items.getItem(i);
                                            if (item === null)
                                                continue;
                                        }

                                        menu.repaint();
                                    }
                                </script>

                            </telerik:RadCodeBlock>
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Project" CssClass="RadGrid_Project" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true"
                                OnItemCreated="RadGrid_Project_ItemCreated"
                                AllowCustomPaging="true" OnNeedDataSource="RadGrid_Project_NeedDataSource"
                                OnItemDataBound="RadGrid_Project_ItemDataBound"
                                OnItemCommand="RadGrid_Project_ItemCommand"
                                OnPageIndexChanged="RadGrid_Project_PageIndexChanged"
                                OnPageSizeChanged="RadGrid_Project_PageSizeChanged"
                                FilterType="CheckList"
                                OnPreRender="RadGrid_Project_PreRender"
                                EnableLinqExpressions="false"
                                ShowGroupPanel="false">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" ClientEvents-OnRowDblClick="RowDblClick">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnRowDblClick="RowDblClick" OnGridCreated="OnGridCreated" OnHeaderMenuShowing="headerMenuShowing"
                                        OnColumnHidden="ColumnSettingsChange" OnColumnShown="ColumnSettingsChange"
                                        OnColumnResized="ColumnSettingsChange" OnColumnSwapped="ColumnSettingsChange" />
                                </ClientSettings>
                                <FilterMenu OnClientShowing="MenuShowing" />
                                <MasterTableView AllowPaging="true" AllowCustomPaging="true" AutoGenerateColumns="false" EnableHeaderContextMenu="true"
                                    AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Customer" CheckListWebServicePath="AccountAutoFill.asmx">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="10">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn DataField="Customer" SortExpression="Customer" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" UniqueName="Customer" HeaderText="Customer" ShowFilterIcon="false" HeaderStyle-Width="250">
                                            <ItemTemplate>
                                                <%#Eval("Customer")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Location" SortExpression="Tag" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" UniqueName="Tag" HeaderText="Location" ShowFilterIcon="false" HeaderStyle-Width="250">
                                            <ItemTemplate>
                                                <%#Eval("Tag")%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Project#" DataField="ID" SortExpression="ID" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" UniqueName="ID" ShowFilterIcon="false" HeaderStyle-Width="80" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkJob" runat="server" NavigateUrl='<%# "addProject.aspx?uid=" + Eval("ID") %>'><%#Eval("ID")%></asp:HyperLink>
                                                <asp:Label runat="server" Visible="false" Text='<%#Eval("ID")%>' ID="lblJob"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Location" SortExpression="fdesc" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" UniqueName="fdesc" HeaderText="Desc" ShowFilterIcon="false" HeaderStyle-Width="200">
                                            <ItemTemplate>
                                                <%#Eval("fdesc")%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" SortExpression="Status"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Stage" DataField="Stage" SortExpression="Stage" UniqueName="Stage"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            FilterCheckListEnableLoadOnDemand="false" FilterCheckListWebServiceMethod="LoadProjectStages" FilterControlAltText="Filter Stage">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Company" DataField="Company" SortExpression="Company"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Date Created"
                                            DataField="fDate" SortExpression="fDate"
                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                            AllowFiltering="false"
                                            HeaderStyle-Width="150" DataFormatString="{0:MM/dd/yyyy}"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Service Type" DataField="CType" SortExpression="CType"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Template Type" DataField="TemplateDesc" SortExpression="TemplateDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Department" DataField="Type" SortExpression="Type" UniqueName="Department"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                            FilterCheckListEnableLoadOnDemand="false" FilterCheckListWebServiceMethod="LoadProjectDepartments" FilterControlAltText="Filter Department">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Default Salesperson" DataField="Salesperson" SortExpression="Salesperson"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="180"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Route" SortExpression="Route" UniqueName="DRoute"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                            FilterCheckListEnableLoadOnDemand="false" FilterCheckListWebServiceMethod="LoadProjectRoutes" FilterControlAltText="Filter Route">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn HeaderText="Contract Price" DataField="ContractPrice" SortExpression="ContractPrice"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="ContractPrice" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("ContractPrice")%>' ID="lblContractPrice"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerContractPrice" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Not Billed Yet" DataField="NotBilledYet" SortExpression="NotBilledYet"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NotBilledYet" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NotBilledYet")%>' ID="lblNotBilledYet"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNotBilledYet" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Total Billed" DataField="NRev" SortExpression="NRev"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NRev" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NRev")%>' ID="lblNRev"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNRev" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Open AR" DataField="OpenARBalance" SortExpression="OpenARBalance"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="OpenARBalance" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("OpenARBalance")%>' ID="lblOpenARBalance"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerOpenARBalance" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Open AP" DataField="OpenAPBalance" SortExpression="OpenAPBalance"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="OpenAPBalance" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("OpenAPBalance")%>' ID="lblOpenAPBalance"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerOpenAPBalance" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Actual Hours" DataField="NHour" SortExpression="NHour"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NHour" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NHour")%>' ID="lblNHour"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNHour" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Labor Expense" DataField="NLabor" SortExpression="NLabor"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NLabor" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NLabor")%>' ID="lblNLabor"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNLabor" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Material Expense" DataField="NMat" SortExpression="NMat"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NMat" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NMat")%>' ID="lblNMat"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNMat" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Other Expense" DataField="NCost" SortExpression="NOMat"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NOMat" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NOMat")%>' ID="lblNOMat"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNOMat" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Total Expenses" DataField="NCost" SortExpression="NCost"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NCost" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NCost")%>' ID="lblNCost"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNCost" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Total Budgeted Expense" DataField="TotalBudgetedExpense" SortExpression="TotalBudgetedExpense"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="TotalBudgetedExpense" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("TotalBudgetedExpense")%>' ID="lblTotalBudgetedExpense"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerTotalBudgetedExpense" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Budgeted Labor" DataField="BLabor" SortExpression="BLabor"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="BLabor" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("BLabor")%>' ID="lblBLabor"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerBLabor" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Budgeted Hours" DataField="BHour" SortExpression="BHour"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="BHour" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("BHour")%>' ID="lblBHour"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerBHour" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Total PO Order" DataField="NComm" SortExpression="NComm"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NComm" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NComm")%>' ID="lblNComm"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNComm" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="ReceivePO" DataField="ReceivePO" SortExpression="ReceivePO"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="ReceivePO" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("ReceivePO")%>' ID="lblReceivePO"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerReceivePO" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Net Profit" DataField="NProfit" SortExpression="NProfit"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NProfit" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NProfit")%>' ID="lblNProfit"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNProfit" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="% in Profit" DataField="NRatio" SortExpression="NRatio"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150" UniqueName="NRatio" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("NRatio")%>' ID="lblNet"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="footerNet" Text="0"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Remarks" UniqueName="Remarks" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Remarks" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Remarks")%>' ID="lblRemarks"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn HeaderText="Project Manager" DataField="ProjectManagerUserName" SortExpression="ProjectManagerUserName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="113"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Supervisor" DataField="SupervisorUserName" SortExpression="SupervisorUserName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Location Type" DataField="LocationType" SortExpression="LocationType"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderText="Building Type" DataField="BuildingType" SortExpression="BuildingType"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="estimate" UniqueName="estimate" SortExpression="estimate" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Estimate #" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnGridEstimate" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "estimate")%>'></asp:HiddenField>
                                                <asp:Repeater ID="rptEstimates" runat="server">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEstimate" runat="server" CommandName="Estimate #" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EstimateID")%>' OnCommand="LinkButton_Click"><%#DataBinder.Eval(Container.DataItem, "EstimateID")%></asp:LinkButton>
                                                        <asp:Label ID="lblComma" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Last").ToString() == "false" ? ", " : ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn HeaderText="Expected Closing Date" UniqueName="ExpectedClosingDate" DataField="ExpectedClosingDate" SortExpression="ExpectedClosingDate"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            AllowFiltering="true"
                                            HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="ExpenseGL" UniqueName="ExpenseGL" DataField="ExpenseGL" SortExpression="ExpenseGL"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            AllowFiltering="true"
                                            HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderText="IntersetGL" UniqueName="IntersetGL" DataField="IntersetGL" SortExpression="IntersetGL"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            AllowFiltering="true"
                                            HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="BillingCode" UniqueName="BillingCode" DataField="BillingCode" SortExpression="BillingCode"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            AllowFiltering="true"
                                            HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="BillingCodeGL" UniqueName="BillingCodeGL" DataField="BillingCodeGL" SortExpression="BillingCodeGL"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            AllowFiltering="true"
                                            HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderText="LaborWage" UniqueName="Laborwage" DataField="Laborwage" SortExpression="Laborwage"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            AllowFiltering="true"
                                            HeaderStyle-Width="150"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <telerik:RadWindow ID="VendorScheduleWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server">
                <div class="p-t-15">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row m-b-0" >
                                <div class="grid_container" id="divVendorScheduleGrid" runat="server">
                                    <div class="RadGrid RadGrid_Material RadGrid_Popup">
                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_VendorSchedule" AllowFilteringByColumn="true" ShowFooter="True" PageSize="5000"
                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" Height="516px"
                                            PagerStyle-AlwaysVisible="true"
                                            EmptyDataText="No Projects Found...">
                                            <%--OnNeedDataSource="RadGrid_VendorSchedule_NeedDataSource"--%>
                                            <CommandItemStyle />
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="True"></Selecting>

                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                            </ClientSettings>
                                            <MasterTableView DataKeyNames="id" UseAllDataFields="true" AutoGenerateColumns="false" AllowFilteringByColumn="true" ShowFooter="false">
                                                <Columns>
                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                    </telerik:GridClientSelectColumn>

                                                    <telerik:GridTemplateColumn DataField="ID"
                                                        UniqueName="ID"
                                                        SortExpression="ID"
                                                        AutoPostBackOnFilter="true"
                                                        AllowFiltering="true"
                                                        DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="Planner #" HeaderStyle-Width="90" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkName" runat="server" Text='<%# Bind("ID") %>' NavigateUrl='<%# "VendorSchedule.aspx?plnid=" + Eval("ID") + "&venid=" + Eval("VendorID") %>'></asp:HyperLink>
                                                            <asp:HiddenField ID="hdnPlannerID" runat="server" Value='<%# Eval("ID") %>'></asp:HiddenField>
                                                        </ItemTemplate>

                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                        ShowFilterIcon="false" HeaderStyle-Width="200">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Desc" HeaderText="Description" SortExpression="Desc" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                        ShowFilterIcon="false" HeaderStyle-Width="200">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="CreatedDt" SortExpression="CreatedDt" AutoPostBackOnFilter="false" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="Created Date" ShowFilterIcon="false" HeaderStyle-Width="100" AllowFiltering="false"
                                                        UniqueName="CreatedDt">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreatedDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("CreatedDt").ToString())) ? "" : Eval("CreatedDt", "{0:M/d/yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                        ShowFilterIcon="false" HeaderStyle-Width="200">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>

                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mgntp10" >
                    <div class="btnlinks">

                        <asp:LinkButton ID="lnkGenerateVendorSchedule" runat="server" OnClick="lnkGenerateVendorSchedule_Click">Save</asp:LinkButton>

                    </div>
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
    <!-- Hidden Field -->

    <asp:HiddenField runat="server" ID="hdnAddeJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdPageNumber" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSelectedTab" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSortColumn" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSort" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSelectedRow" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnProjectList" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnsessiontype" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
            $('#activeone').show();
            var id = $("#hdDept").val();
            $("#tabProject > li > a#" + id + "")[0].click();

            document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();

        });
        function selectTab(id) {
            $('#<%=hdnTabId.ClientID%>').val(id);
            $("#hdDept").val(id);
            var grid = $find("<%= RadGrid_Project.ClientID %>");
            if (grid) {
                var tableView = grid.get_masterTableView();
                var filterExpressions = tableView.get_filterExpressions();
                console.log(tableView.get_filterExpressions());
                if (filterExpressions.length > 0) { var expression = filterExpressions[0]; }
                tableView.rebind();
            }
        }
    </script>
</asp:Content>
