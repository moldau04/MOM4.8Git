<%@ Page Title="Purchase Orders || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" CodeBehind="ManagePO.aspx.cs" Inherits="ManagePO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        .dropdown-content.po-report-dropdown, 
        .dropdown-content.po-email-report-dropdown,
        .po-report-dropdown-content {
            width: auto !important;
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
        .pdf-trn {
    background-color: transparent;
        }
        .RadGrid_Material th {
            font-size:0.8rem;
        }
        .RadGrid_Material .rgHeader {
                padding: 5px 9px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.dropdown-button2').dropdown({
                inDuration: 300,
                outDuration: 225,
                constrain_width: false, // Does not change width of dropdown to that of the activator
                hover: true, // Activate on hover
                gutter: $('.po-report-dropdown').width() + 35, // Spacing from edge
                belowOrigin: false, // Displays dropdown below the button
                alignment: 'left' // Displays dropdown with edge aligned to the left of button
            });
        });
        function ShowRestoreGridSettingsButton() {
            debugger
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
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder"  />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_ManagePO" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ManagePO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkChk">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ManagePO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ManagePO" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ManagePO" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate"  />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ManagePO" />
                    <telerik:AjaxUpdatedControl ControlID="txtFromDate"  />
                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                    <telerik:AjaxUpdatedControl ControlID="upPannelSearch"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkMailPOReport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkMailCustomPOReport">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkMailPOReport2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRestoreGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ManagePO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSaveGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ManagePO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Manage PO</div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddPO" runat="server" OnClick="lnkAddPO_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkCopy" runat="server" OnClick="lnkCopy_Click">Copy</asp:LinkButton>
                                                </div>

                                                <div class="btnlinks menuAction">
                                                    <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                    </a>
                                                </div>

                                                <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                                            </a>
                                                        </div>
                                                        <ul id="dropdown1" class="dropdown-content">
                                                            <li>
                                                                <a href="CustomersReport.aspx?type=Customer" class="-text">Add New Report</a>
                                                            </li>
                                                            <li>
                                                                <a href="POWeeklyReport.aspx" class="-text">PO Weekly Report</a>
                                                            </li>
                                                        </ul>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks" id="LI2pnlGridButtons" runat="server">
                                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dynamicPDF">PO Report
                                                            </a>
                                                        </div>
                                                        <ul id="dynamicPDF" class="dropdown-content po-report-dropdown">
                                                            <li>
                                                                <asp:LinkButton ID="lnkPrintPOReport" runat="server" Enabled="true" OnClick="lnkPrintPOReport_Click"> <i class="fa fa-file-pdf-o pdfdy "  aria-hidden="true"></i>&nbsp; PO Report  <i class="fa fa-download pdf-trn" aria-hidden="true" ></i></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkPrintCustomPOReport" runat="server" CausesValidation="true" Enabled="true" OnClick="lnkPrintCustomPOReport_Click"></asp:LinkButton>
                                                            </li>
                                                            <asp:ListView ID="listCustomPO" runat="server" Visible="true">
                                                                <LayoutTemplate>
                                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <li id="poTemplateItem" runat="server" class="border-btm">
                                                                        <asp:LinkButton runat="server" Enabled="true" OnCommand="btnDownloadPOTemplate_Click" CommandArgument="<%# Container.DataItem %>">
                                                                            <i class="fa fa-file-pdf-o pdfdy"  aria-hidden="true"></i>&nbsp; <%# Container.DataItem %> <i class="fa fa-download pdf-trn" aria-hidden="true" ></i>
                                                                        </asp:LinkButton>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </ul>
                                                    </li>
                                                    <li>
                                                        <asp:HiddenField ID="Confirm_Value" runat="server" ClientIDMode="Static" />
                                                        <div class="btnlinks">
                                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dynamicMail">Email All
                                                            </a>
                                                        </div>
                                                        <ul id="dynamicMail" class="dropdown-content po-email-report-dropdown">
                                                            <li>
                                                                <asp:LinkButton ID="lnkMailPOReport" runat="server" Enabled="true" OnClick="lnkMailPOReport_Click" OnClientClick="return Confirm('PO Report');"> <i class="fa fa-file-pdf-o pdfdy" aria-hidden="true"></i>&nbsp; PO Report  <i class="fa fa-download pdf-trn" aria-hidden="true" ></i></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkMailCustomPOReport" runat="server" CausesValidation="true" Enabled="true" OnClick="lnkMailCustomPOReport_Click" OnClientClick="return Confirm('Madden PO Report');"></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkMailPOReport2" runat="server" CausesValidation="true" Enabled="true" OnClick="lnkMail_Click" OnClientClick="return Confirm('PO Approval Report');"> <i class="fa fa-file-pdf-o pdfdy"  aria-hidden="true" ></i>&nbsp; PO Approval Report &nbsp;&nbsp;<i class="fa fa-download pdf-trn" aria-hidden="true" ></i></asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </li>
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
                                                <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                            </div>
                                            <div class="rght-content">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>

            <%--<div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <ul class="anchor-links">
                                <li id="liLogs" runat="server"><a href="#accrdlogs">Email Sending Logs</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>--%>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane">
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth ser-css2" >
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtFromDate" placeholder="From"  runat="server" CssClass="datepicker_mom" MaxLength="28"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtToDate" placeholder="To"  runat="server" CssClass="datepicker_mom" MaxLength="28"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap tabcontainer">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <%--<input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnManagePODate', 'rdCal')" />--%>
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <%--<input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnManagePODate', 'rdCal')" />--%>
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <%--<input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnManagePODate', 'rdCal')" />--%>
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <%--<input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnManagePODate', 'rdCal')" />--%>
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <%--<input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnManagePODate', 'rdCal')" />--%>
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
                                <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" CssClass="css-checkbox" Text="Incl. Closed" AutoPostBack="True"></asp:CheckBox>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="srchpaneinner">
                    <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="srchtitle srchtitlecustomwidth ser-css2">
                                Search
                            </div>
                            <div class="srchinputwrap">

                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst selectsml" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                    <%--<asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="p.PO">PO #</asp:ListItem>
                                    <asp:ListItem Value="Projectnumber">Projectnumber</asp:ListItem>
                                    <asp:ListItem Value="v.Acct">VendorID #</asp:ListItem>
                                    <asp:ListItem Value="r.Name">VendorName</asp:ListItem>
                                    <asp:ListItem Value="p.Status">Status</asp:ListItem>
                                    <asp:ListItem Value="vs.Status">Approval Status</asp:ListItem>
                                    <asp:ListItem Value="p.fBy">Created by</asp:ListItem>
                                    <asp:ListItem Value="p.RequestedBy">Requested by</asp:ListItem>
                                      <asp:ListItem Value="p.Custom">Custom</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst selectsml" Visible="false">
                                    <asp:ListItem Value="0">Open </asp:ListItem>
                                    <asp:ListItem Value="1">Closed </asp:ListItem>
                                    <asp:ListItem Value="2">Void </asp:ListItem>
                                    <asp:ListItem Value="3">Partial-Quantity </asp:ListItem>
                                    <asp:ListItem Value="4">Partial-Amount</asp:ListItem>
                                    <asp:ListItem Value="5">Closed At Receive PO</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlApprovalStatus" runat="server" CssClass="browser-default selectst selectsml" Width="155px" Visible="false">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="0">Pending</asp:ListItem>
                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                    <asp:ListItem Value="2">Decline</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">

                                <asp:TextBox ID="txtSearch" placeholder="Search"  runat="server" CssClass="srchcstm"></asp:TextBox>

                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtManagePOSearch"  placeholder="Search" runat="server" CssClass="srchcstm" onkeypress="return isNumberKey(this,event)" Visible="false"></asp:TextBox>
                            </div>

                            <div class="srchinputwrap btnlinksicon srchclr">
                                <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                                </asp:LinkButton>
                            </div>
                            <div class="srchinputwrap m-t-5">
                                <asp:CheckBox Text="Approval Status updated by me" runat="server" CssClass="css-checkbox" ID="chkUserApprove" Width="220px" Style="width: 215px; margin-top: 5px;" />
                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_container">
                <div class="form-section-row pmd-card" >


                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_ManagePO" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_ManagePO.ClientID %>");
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
                                        element.selectionStart = selectionStart;
                                    }
                                }
                            </script>
                        </telerik:RadCodeBlock>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_ManagePO" runat="server" LoadingPanelID="RadAjaxLoadingPanel_ManagePO" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_ManagePO" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_ManagePO_NeedDataSource" OnPreRender="RadGrid_ManagePO_PreRender" OnItemEvent="RadGrid_ManagePO_ItemEvent" OnItemCreated="RadGrid_ManagePO_ItemCreated" OnExcelMLExportRowCreated="RadGrid_ManagePO_ExcelMLExportRowCreated">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents
                                        OnGridCreated="OnGridCreated" OnHeaderMenuShowing="headerMenuShowing"
                                        OnColumnHidden="ColumnSettingsChange" OnColumnShown="ColumnSettingsChange"
                                        OnColumnResized="ColumnSettingsChange" OnColumnSwapped="ColumnSettingsChange" />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="PO" AllowNaturalSort="false" EnableHeaderContextMenu="true">
                                   <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn UniqueName="PO" DataField="PO" SortExpression="PO" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="PO" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnID" Value='<%# Bind("PO") %>' runat="server" />
                                                <asp:Label ID="lblID" Text='<%# Eval("PO")%>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>

                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="fDates" DataField="fDates" SortExpression="fDates" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDate" Text='<%# Eval("fDates")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDates"))):""%> ' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="fDesc" DataField="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Description" ShowFilterIcon="false"
                                            HeaderStyle-Width="310">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkfDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="VendorName" DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="StatusName" DataField="StatusName" HeaderText="Status" SortExpression="StatusName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="ApproveStatus" DataField="ApproveStatus" HeaderText="Approve Status" SortExpression="ApproveStatus"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Company" DataField="Company" HeaderText="Company" SortExpression="Company"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="Amount" DataField="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                            SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            HeaderText="Amount" ShowFilterIcon="false" HeaderStyle-Width="110">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'
                                                    ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="OpenAmount" DataField="OpenAmount" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                            SortExpression="OpenAmount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            HeaderText="Open Amount" ShowFilterIcon="false" HeaderStyle-Width="110">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOpenAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OpenAmount", "{0:c}")%>'
                                                    ForeColor='<%# Convert.ToDouble(Eval("OpenAmount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="fBy" DataField="fBy" HeaderText="Created by" SortExpression="fBy"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="RequestedBy" DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Project" DataField="Project" HeaderText="Project" SortExpression="Project"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Department" DataField="Department" HeaderText="Department" SortExpression="Department"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Code" DataField="Code" HeaderText="Code" SortExpression="Code"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                           <telerik:GridBoundColumn UniqueName="Address" DataField="Address" HeaderText="Address" SortExpression="Address"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                           <telerik:GridBoundColumn UniqueName="Location" DataField="Location" HeaderText="Location" SortExpression="Location"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>

                                    </Columns>
                                </MasterTableView>
                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--Email Sending Logs--%>
    <div class="container accordian-wrap">
        <div class="col s12 m12 l12">
            <div class="row">
                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                    <li id="tbLogs" runat="server" style="display: block">
                        <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Email History Log</div>
                        <div class="collapsible-body">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="grid_container">
                                        <div class="form-section-row pmd-card" >
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
                                                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                                            <%--<GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="SessionNo" FieldName="SessionNo" HeaderText="Session No" SortOrder="None"></telerik:GridGroupByField>
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldName="SessionNo"  SortOrder="None"></telerik:GridGroupByField>
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
                                                                    CurrentFilterFunction="Contains" HeaderText="PO #" ShowFilterIcon="false">
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
                                                                <telerik:GridTemplateColumn DataField="MailTo" SortExpression="MailTo" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Mail To" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmailTo" runat="server" Text='<%# Eval("MailTo") %>'></asp:Label>
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

    <asp:HiddenField runat="server" ID="hdnManagePOSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
    <asp:HiddenField ID="hdnPOLimit" runat="server" />
    <asp:HiddenField ID="hdnMinAmount" runat="server" />
    <asp:HiddenField ID="hdnMaxAmount" runat="server" />
    <asp:HiddenField ID="hdnPOApproveAmt" runat="server" />
    <asp:HiddenField ID="hdnPOApprove" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function WarningMsg() {
            noty({
                text: 'This PO is not open and can therefore not be deleted',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function DeleteSuccessMesg() {
            noty({
                text: 'PO deleted successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function closedMesg() {
            noty({
                text: 'Please select a PO to delete.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function isNumberKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (charCode == 45) {
                return true;
            }

            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            if (number.length > 1 && charCode == 46) {
                return false;
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
    </script>
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

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });




            });
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblManagePOActive"])%>';
                var val;
                val = localStorage.getItem("hdnManagePODate");
                if (SesVar == '2') {
                    $("#<%=lblDay.ClientID%>").addClass("");
                    $("#<%=lblWeek.ClientID%>").addClass("");
                    $("#<%=lblMonth.ClientID%>").addClass("");
                    $("#<%=lblQuarter.ClientID%>").addClass("");
                    $("#<%=lblYear.ClientID%>").addClass("");
                }
                else {
                    if (val == 'Day') {
                        $("#<%=lblDay.ClientID%>").addClass("labelactive");
                        document.getElementById("rdDay").checked = true;
                    }
                    else if (val == 'Week') {

                        $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                        document.getElementById("rdWeek").checked = true;
                    }
                    else if (val == 'Month') {

                        $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                        document.getElementById("rdMonth").checked = true;
                    }
                    else if (val == 'Quarter') {

                        $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                        document.getElementById("rdQuarter").checked = true;
                    }
                    else if (val == 'Year') {

                        $("#<%=lblYear.ClientID%>").addClass("labelactive");
                        document.getElementById("rdYear").checked = true;
                    }
                    else {
                        $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                        document.getElementById("rdWeek").checked = true;
                    }
                }
            }
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
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdWeek';
                }
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
        function SelectDate(type) {
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
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Day";
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
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Week";
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
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Month";
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
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Quarter";
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
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Year";
            }

            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, type);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
        <%--function SelectDate(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
            var type = type;
            var txtDateFrom = txtDateFrom;
            var txtdateTo = txtdateTo;
            var UniqueVal = UniqueVal;
            var label = label;
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Day";
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
                document.getElementById(txtDateFrom).value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Week";
            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);
                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Month";

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
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Quarter";
            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);
                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnManagePOSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }--%>

        <%--//////////////// Confirm Mail Send to worker ///////////////////
        function notyConfirm() {
            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Do you want to send a text message to the assigned worker at this time?',
                type: 'alert',
                speed: 500,
                timeout: false,
                closeButton: false,
                closeOnSelfClick: true,
                closeOnSelfOver: false,
                force: false,
                onShow: false,
                onShown: false,
                onClose: false,
                onClosed: false,
                buttons: [
                            {
                                type: 'btn-primary', text: 'Yes', click: function ($noty) { 
                                    $noty.close();
                                    $("#<%=lnkMailPOReport.ClientID%>").click(); 
                                }
                            },
                            {
                                type: 'btn-danger', text: 'No', click: function ($noty) {
                                    $noty.close();                                       
                                }
                            }
                ],
                modal: true,
                template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
                cssPrefix: 'noty_',
                custom:
                {
                    container: null
                }
            });
        }--%>

        function Confirm(fName) {
            var Val;
            var returnValue;
            if (confirm("Are you sure you want to email all " + fName + "?") === true) {
                Val = "Yes";
                returnValue = true;
            } else {
                Val = "No";
                returnValue = false;
            }

            document.getElementById("Confirm_Value").value = Val;
            return returnValue;
        }
        function OnGridCreated(sender, args) {
            //alert('OnGridCreated');
            debugger;
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
            debugger
            for (var i = 0; i < menu.get_items().get_count(); i++) {
                var item = menu.get_items().getItem(i);
                if (item.get_value() != 'ColumnsContainer') {
                    item.get_element().style.display = 'none';
                }
            }

            var columnsItem = menu.findItemByText("Columns");
            columnsItem.get_items().getItem(0).get_element().style.display = "none";
            columnsItem.get_items().getItem(1).get_element().style.display = "none";
            //columnsItem.get_items().getItem(2).get_element().style.display = "none";
        }
        function ColumnSettingsChange(menu, args) {
            debugger
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



