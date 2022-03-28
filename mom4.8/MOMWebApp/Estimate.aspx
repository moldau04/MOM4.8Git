<%@ Page Title="Estimate || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Estimate" CodeBehind="Estimate.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        div.RadGrid .rgHeader {
            white-space: nowrap;
        }
        
        .EstimatelistTooltip {
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
            margin-bottom: 20px;
        }

            .EstimatelistTooltip:after {
                top: 0%;
                left: 0%;
                border: solid transparent;
                content: " ";
                height: 0;
                width: 0;
                position: absolute;
                pointer-events: none;
                border-color: rgba(0, 0, 0, 0);
                border-top-color: #000;
                border-width: 10px;
                margin-left: -10px;
                margin-bottom: 20px;
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

        div.rmSlide {
            top: 0 !important;
        }

            div.rmSlide > ul.rmVertical, div.rmSlide > div.rmScrollWrap > ul.rmVertical {
                padding-left: 10px !important;
                padding-right: 10px !important;
            }

            div.rmSlide input[type=checkbox] {
                vertical-align: middle !important;
                display: inline-block !important;
            }
    </style>

    <script type="text/javascript">
        function ShowRestoreGridSettingsButton() {
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Estimate" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" LoadingPanelID="RadAjaxLoadingPanel_Estimate" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_TotalRecords" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkProject">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" LoadingPanelID="RadAjaxLoadingPanel_Estimate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" LoadingPanelID="RadAjaxLoadingPanel_Estimate" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_TotalRecords" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_DateRange" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" LoadingPanelID="RadAjaxLoadingPanel_Estimate" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_DateRange" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_TotalRecords" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_SearchPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" LoadingPanelID="RadAjaxLoadingPanel_Estimate" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_DateRange" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_TotalRecords" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_SearchPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Estimate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_TotalRecords" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_DateRange" />
                    <%--<telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_SearchPanel" />
                    <telerik:AjaxUpdatedControl ControlID="lnkSearch" />
                    <telerik:AjaxUpdatedControl ControlID="lnkClear" />
                    <telerik:AjaxUpdatedControl ControlID="lnkShowAll" />--%>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClosePopup">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" LoadingPanelID="RadAjaxLoadingPanel_Estimate" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_TotalRecords" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Estimate_DateRange" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRestoreGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSaveGridSettings">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Estimate" />
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
                                            <div class="page-title"><i class="mdi-editor-insert-drive-file"></i>&nbsp;Estimate</div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks menuAction">
                                                    <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                    </a>
                                                </div>

                                                <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkCopy" runat="server" OnClick="lnkCopy_Click">Copy</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkDelete" OnClientClick="return CheckDelete();" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                            <a class="dropdown-button" id="lnkExport" runat="server" data-beloworigin="true" href="#" data-activates="dropdown2" visible="false">Export</a>
                                                        </div>
                                                        <ul id="dropdown2" class="dropdown-content">
                                                            <li>
                                                                <asp:LinkButton ID="lnkExcelMenu" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkEstimateRateTender" runat="server" OnClick="lnkEstimateRateTender_Click">Export Tender.csv</asp:LinkButton>
                                                            </li>
                                                            <%--<li>
                                                                <asp:LinkButton ID="lnkEstimateRateTenderVO" runat="server" OnClick="lnkEstimateRateTenderVO_Click">Export TenderVO.csv</asp:LinkButton>
                                                            </li>--%>
                                                        </ul>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkProject" runat="server" OnClick="lnkProject_Click"
                                                                OnClientClick="return confirm('Do you really want to convert this estimate to project?');">Convert to Project</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <a class="dropdown-button" id="lnkReport" runat="server" data-beloworigin="true" href="#" data-activates="dropdown1">Reports
                                                            </a>
                                                        </div>
                                                        <ul id="dropdown1" class="dropdown-content">
                                                            <%--<li>
                                                                <asp:LinkButton ID="lnk_Estimate" runat="server" OnClick="lnk_Estimate_Click" CssClass="-text">Estimate Agreement</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnk_Service" runat="server" OnClick="lnk_Service_Click" CssClass="-text">Service Agreement</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkWeeklySalereport" OnClick="lnkWeeklySalereport_Click" runat="server" CssClass="-text">Weekly Sales Report</asp:LinkButton>
                                                            </li>--%>

                                                            <li>
                                                                <asp:LinkButton ID="lnkExportEstimateProfile" runat="server" OnClick="lnkExportEstimateProfile_Click"
                                                                    CausesValidation="true" ValidationGroup="search">Estimate Profile</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkEstimateBacklog" runat="server" OnClick="lnkEstimateBacklog_Click"
                                                                    CausesValidation="true" ValidationGroup="search">Estimate Backlog Report</asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkMailAll" OnClick="lnkMailAll_Click" OnClientClick="return EmailAllConfirm();" runat="server">Email All</asp:LinkButton>
                                                        </div>
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

                                        </div>
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
                <div class="srchpaneinner">
                    <telerik:RadAjaxPanel runat="server" ID="RadAP_Estimate_DateRange">
                        <%--<div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                            Date
                        </div>--%>
                        <div class="srchinputwrap ser-css2" >
                            <asp:DropDownList ID="ddlDateRange" runat="server" CssClass="browser-default select selectst" style="width: 119px;">
                                <asp:ListItem Value="1">Estimate Date</asp:ListItem>
                                <asp:ListItem Value="2">Bid Closed Date</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="From" CssClass="datepicker_mom srchcstm" MaxLength="28" style="width: 88px;"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtToDate" runat="server" placeholder="To" CssClass="datepicker_mom srchcstm" MaxLength="28" style="width: 88px;"></asp:TextBox>
                        </div>
                    </telerik:RadAjaxPanel>
                    <div class="srchinputwrap">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnEstimateDate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnEstimateDate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnEstimateDate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnEstimateDate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnEstimateDate', 'rdCal')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                     <div class="srchpaneinner1">
                        <div class="srchtitle srchtitlecustomwidth ser-css2" >
                            Search
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst selectsml" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="e.ID">Estimate #</asp:ListItem>
                                <asp:ListItem Value="e.Opportunity">Opp #</asp:ListItem>
                                <asp:ListItem Value="e.job">Project #</asp:ListItem>
                                <asp:ListItem Value="e.Contact">Contact</asp:ListItem>
                                <asp:ListItem Value="e.Category">Category</asp:ListItem>
                                <asp:ListItem Value="e.iscertifiedproject">Certified Payroll</asp:ListItem>
                                <asp:ListItem Value="e.Template">Template</asp:ListItem>
                                <asp:ListItem Value="dep.ID">Department</asp:ListItem>
                                <asp:ListItem Value="e.EstimateAddress">Estimate Name</asp:ListItem>
                                <asp:ListItem Value="em.fFirst">Assigned To</asp:ListItem>
                                <asp:ListItem Value="e.CompanyName">Customer Name</asp:ListItem>
                                <asp:ListItem Value="e.Status">Status</asp:ListItem>
                                <asp:ListItem Value="e.ffor">Type</asp:ListItem>
                                <asp:ListItem Value="l.OpportunityStageID">Opportunity Stage</asp:ListItem>
                                <asp:ListItem Value="CustomField">Custom Field</asp:ListItem>
                                <%--<asp:ListItem Value="e.BDate">Bid Close Date</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst selectsml" Visible="false">
                                <asp:ListItem Value="1">Open</asp:ListItem>
                                <asp:ListItem Value="2">Cancelled</asp:ListItem>
                                <asp:ListItem Value="3">Withdrawn</asp:ListItem>
                                <asp:ListItem Value="4">Disqualified</asp:ListItem>
                                <asp:ListItem Value="5">Sold</asp:ListItem>
                                <asp:ListItem Value="6">Competitor</asp:ListItem>
                                <asp:ListItem Value="7">Quoted</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" placeholder="Search" runat="server" CssClass="srchcstm" style="width: 100px;"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlType" CssClass="browser-default selectst selectsml" runat="server" Visible="false">
                                <asp:ListItem Value="PROSPECT">Lead</asp:ListItem>
                                <asp:ListItem Value="ACCOUNT">Existing</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlCertified" CssClass="browser-default selectst selectsml" runat="server" Visible="false">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlTemplate" CssClass="browser-default selectst selectsml" runat="server" Visible="false">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlDepartment" CssClass="browser-default selectst selectsml" runat="server" Visible="false">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlOppStage" CssClass="browser-default selectst selectsml" runat="server" Visible="false">
                            </asp:DropDownList>
                        </div>
                        <%--<div class="srchinputwrap pd-negatenw input-field">
                                <asp:TextBox ID="txtBidCloseDate" placeholder="From" runat="server" CssClass="srchcstm datepicker_mom" Visible="false"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap pd-negatenw input-field">
                                <asp:TextBox ID="txtBidCloseDateTo" placeholder="To" runat="server" CssClass="srchcstm datepicker_mom" Visible="false"></asp:TextBox>
                            </div>--%>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlCustomFields" CssClass="browser-default selectst selectsml" runat="server" Visible="false">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtCustomSearch" placeholder="Custom field content" runat="server" CssClass="srchcstm" Visible="false"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap srchclr btnlinksicon m-lm-t" >
                            <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                            </asp:LinkButton>
                        </div>
                    </div>



                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <telerik:RadAjaxPanel runat="server" ID="RadAP_Estimate_TotalRecords">
                                    <%--<asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                    <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </telerik:RadAjaxPanel>
                            </span>
                        </div>
                    </div>
                </div>
                <telerik:RadAjaxPanel runat="server" ID="RadAP_Estimate_SearchPanel">
                    <%--<asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                   
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
                </telerik:RadAjaxPanel>
            </div>

            <div class="grid_container">
                <div class="form-section-row pmd-card">

                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Estimate" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Estimate.ClientID %>");
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
                        <%--<telerik:RadAjaxPanel ID="RadAjaxPanel_Estimate" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Estimate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">--%>
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Estimate" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%"
                                PagerStyle-AlwaysVisible="true"
                                EnableLinqExpressions="false"
                                OnNeedDataSource="RadGrid_Estimate_NeedDataSource"
                                OnExcelMLExportRowCreated="RadGrid_Estimate_ExcelMLExportRowCreated"
                                OnItemCreated="RadGrid_Estimate_ItemCreated"
                                OnItemDataBound="RadGrid_Estimate_ItemDataBound"
                                OnPreRender="RadGrid_Estimate_PreRender"
                                EmptyDataText="No Estimates Found..."
                                ClientSettings-AllowColumnsReorder="true"
                                ClientSettings-ReorderColumnsOnClient="true"
                                >
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
                                <MasterTableView DataKeyNames="id" UseAllDataFields="true" AutoGenerateColumns="false" EnableHeaderContextMenu="true" AllowFilteringByColumn="true" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40"  Reorderable="false" Resizable="false" >
                                        </telerik:GridClientSelectColumn>
                                        <%--<telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">  <%--changed here
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("ID") %>' ClientIDMode="Static" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                        <telerik:GridTemplateColumn UniqueName="Attachment" Reorderable="false" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="40">
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ID="Image2" ImageWidth="15px" Visible='<%# Eval("Attached").ToString() == "1"%>' ImageUrl='<%# Eval("Attached").ToString() == "1"  ? "images/Document.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' onclick='<%# string.Format("downloadFile({0},{1})",Eval("ID"),1) %>'></asp:HyperLink>
                                                <asp:Label ID="lblRes" runat="server" CssClass="EstimatelistTooltip" Visible='<%# Eval("Attached").ToString() == "1"%>'
                                                    Text='<%# ShowHoverText(Eval("ID"), Eval("FileName"), Eval("AddedOn"), Eval("AddedBy"), Eval("ManualUpload")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Emailed" UniqueName="Emailed" HeaderText="Send Email" SortExpression="Emailed"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmailed" runat="server" Text='<%# Bind("Emailed") %>'></asp:Label>
                                                <asp:Label ID="lblEmailedInfo" runat="server" CssClass="EstimatelistTooltip" Visible='<%# Eval("Emailed").ToString().ToLower() == "yes"%>'
                                                    Text='<%# ShowHoverEmailedText(Eval("SendFrom"), Eval("SendTo"), Eval("SendOn"), Eval("SendBy")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--<telerik:GridBoundColumn UniqueName="lblApprStatus" DataField="ApprStatus" HeaderText="Approval Status" SortExpression="ApprStatus"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="150">
                                        </telerik:GridBoundColumn>--%>

                                        <telerik:GridTemplateColumn DataField="ApprStatus" HeaderText="Approval Status" SortExpression="ApprStatus" DataType="System.String"
                                            AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains"
                                            HeaderStyle-Width="150"
                                            ShowFilterIcon="false" UniqueName="ddlApprStatus">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblApprStatus" runat="server" Text='<%# Eval("NewStatus") %>' Visible="false"></asp:Label>--%>
                                                <asp:DropDownList ID="ddlApprStatus" onchange="ShowApproveStatusCommentModal(this);"
                                                    runat="server" CssClass="browser-default">
                                                    <asp:ListItem Value="0">Pending</asp:ListItem>
                                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                                    <asp:ListItem Value="2">Required Changes</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnApprStatus" runat="server" Value='<%# Eval("NewStatus") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="ApprBy" UniqueName="ApprBy" HeaderText="Approved By" SortExpression="ApprBy"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="150">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Comment" UniqueName="ApprComment" HeaderText="Approved Status Comment" SortExpression="Comment"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="200">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="ID"
                                            UniqueName="ID"
                                            SortExpression="ID"
                                            AutoPostBackOnFilter="true"
                                            AllowFiltering="true"
                                            DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Estimate#" HeaderStyle-Width="90" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("ID") %>' ClientIDMode="Static" />
                                                <asp:HyperLink ID="lnkName" runat="server" Text='<%# Bind("ID") %>'></asp:HyperLink>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="EstimateAddress" HeaderText="Name" SortExpression="EstimateAddress"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="180"
                                            ShowFilterIcon="false"
                                            UniqueName="EstimateAddress">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimate" runat="server" Text='<%# Bind("EstimateAddress") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="CompanyName" UniqueName="CompanyName" HeaderText="Customer" SortExpression="CompanyName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="150">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Category" UniqueName="Category" HeaderText="Category" SortExpression="Category"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="120">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="AssignTo" UniqueName="AssignTo" HeaderText="AssignTo" SortExpression="AssignTo"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="150">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="ffor" HeaderText="Type" SortExpression="ffor"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false"
                                            UniqueName="ffor">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Convert.ToString(Eval("ffor"))=="ACCOUNT"?"Existing":"Lead" %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="Contact" DataField="Contact" HeaderText="Contact" SortExpression="Contact" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="180">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="fdesc" DataField="fdesc" HeaderText="Desc" SortExpression="fdesc" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="200">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="false" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100" AllowFiltering="false"
                                            UniqueName="fDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("fDate").ToString())) ? "" : Eval("fDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Opportunity" HeaderText="Opportunity #" SortExpression="Opportunity" HeaderStyle-Width="90" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            UniqueName="Opportunity">
                                            <ItemTemplate>
                                                <a href="addopprt.aspx?uid=<%# Eval("Opportunity") %>" style="color: #3175af; text-decoration: none;"><%# Eval("Opportunity") %></a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="job" HeaderText="Project #" SortExpression="job" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100"
                                            UniqueName="job">
                                            <ItemTemplate>
                                                <a href="addproject.aspx?uid=<%# Eval("job") %>" style="color: #3175af; text-decoration: none;"><%# Eval("job") %></a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="Company" DataField="Company" HeaderText="Company" SortExpression="Company" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="140">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="status" DataField="status" HeaderText="Status" SortExpression="status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="80">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="EstimatePrice" FooterAggregateFormatString="{0:c}"
                                            Aggregate="Sum" SortExpression="EstimatePrice" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            HeaderText="Bid Price" ShowFilterIcon="false" UniqueName="EstimatePrice">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimatePrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EstimatePrice", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="100" DataField="QuotedPrice"
                                            FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="QuotedPrice"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Final Bid Price" ShowFilterIcon="false"
                                            UniqueName="QuotedPrice">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "QuotedPrice", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="OpportunityStage" DataField="OpportunityStage" HeaderText="Opportunity Stage" SortExpression="OpportunityStage" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="130">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Discounted" DataField="Discounted" HeaderText="Discounted" SortExpression="Discounted" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="130">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Department" DataField="Department" HeaderText="Department" SortExpression="Department" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="130">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Template" DataField="Template" HeaderText="Template" SortExpression="Template" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="130">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="SoldDate" SortExpression="SoldDate" AutoPostBackOnFilter="false" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Sold Date" ShowFilterIcon="false" HeaderStyle-Width="100" AllowFiltering="false"
                                            UniqueName="fDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoldDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("SoldDate").ToString())) ? "" : Eval("SoldDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--// ES-9177 - Vishal Gupta--%>
                                        <telerik:GridTemplateColumn DataField="FirstSentDate" SortExpression="FirstSentDate" AutoPostBackOnFilter="false" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Sent Date" ShowFilterIcon="false" HeaderStyle-Width="100" AllowFiltering="false"
                                            UniqueName="FirstSentDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFirstSentDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("FirstSentDate").ToString())) ? "" : Eval("FirstSentDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--// ES-9302 - Vishal Gupta--%>
                                        <telerik:GridTemplateColumn DataField="CycleDays" SortExpression="CycleDays" AutoPostBackOnFilter="false" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Cycle Days" ShowFilterIcon="false" HeaderStyle-Width="100" AllowFiltering="false"
                                            UniqueName="Cycle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCycleDays" runat="server" Text='<%# Eval("CycleDays").ToString().Equals("0") ? "" : Eval("CycleDays")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <%--<FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>--%>
                            </telerik:RadGrid>
                        <%--</telerik:RadAjaxPanel>--%>
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
                                        <div class="form-section-row pmd-card">
                                            <div class="RadGrid RadGrid_Material FormGrid">
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
                                                                    CurrentFilterFunction="Contains" HeaderText="Estimate #" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref").ToString() == "0" ? "" : Eval("Ref")%>'></asp:Label>
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

    <asp:HiddenField runat="server" ID="hdnEstimateSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
    <div style="display: none">
        <asp:Button runat="server" ID="btnProcessDownload" OnClick="btnProcessDownload_Click" />
        <asp:HiddenField runat="server" ID="hdnDownloadID" Value="0" />

    </div>
    <%--<asp:CustomValidator ID="ConfirmDropDownValidator" runat="server"
        ClientValidationFunction="ConfirmDropDownValueChange" Display="Dynamic" ValidationGroup="ApprStatus"  />--%>
    <telerik:RadWindow ID="RadWindowApproveStatusComment" Skin="Material" VisibleTitlebar="false" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
        runat="server" Modal="true" Width="460" Height="190">
        <ContentTemplate>
            <div>
                <div class="form-section-row status-css">
                    <div class="input-field col s12">
                        <div class="row">
                            <asp:HiddenField ID="hdnEstId" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hdnApprStatus" runat="server"></asp:HiddenField>
                            <asp:Label runat="server" ID="Label22" AssociatedControlID="txtApproveStatusComment">Approved Status Comment</asp:Label>
                            <asp:TextBox ID="txtApproveStatusComment" runat="server" CssClass="materialize-textarea" MaxLength="255" AutoCompleteType="None" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div style="clear: both;"></div>
                <div class="btnlinks">
                    <asp:LinkButton ID="lnkClosePopup" OnClick="lnkClosePopup_Click" OnClientClick="CloseApproveStatusCommentModal();" runat="server">OK</asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
        function CheckDelete() {
            var result = false;
            $("#<%=RadGrid_Estimate.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });
            if (result == true) {
                return confirm('Do you really want to delete this Estimate ?');
            }
            else {
                alert('Please select a Estimate to delete.')
                return false;
            }
        }
        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
            $(reports).each(function (index, report) {
                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }

                $('#dynamicUI').append('<li><a href="EstimateListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Estimate"><span><img src=images/reportfolder.png> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

            });

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblEstimateActive"])%>';
                var val;
                val = localStorage.getItem("hdnEstimateDate");
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
        function SelectDate(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
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
                document.getElementById('<%= hdnEstimateSelectDtRange.ClientID%>').value = "Day";
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
                document.getElementById('<%= hdnEstimateSelectDtRange.ClientID%>').value = "Week";
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
                document.getElementById('<%= hdnEstimateSelectDtRange.ClientID%>').value = "Month";
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
                document.getElementById('<%= hdnEstimateSelectDtRange.ClientID%>').value = "Quarter";
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
                document.getElementById('<%= hdnEstimateSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnEstimateSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }

        function downloadFile(obj, type) {

            $("#<%= hdnDownloadID.ClientID %>").val(obj);
            var btn = document.getElementById('<%=btnProcessDownload.ClientID%>');
            btn.click();
        }

        function ShowApproveStatusCommentModal(obj) {
            debugger;

            var permission = "<%=ViewState["EstimateApproveProposalPermission"]%>";
            var allowChange = permission == "Y";
            var apprStatus = $(obj).val();
            var currApproveStatus = $(obj).attr("hdnCurrApproveStatus");

            if (!allowChange) {
                // TODO: Get Approve Status Before change
                allowChange = currApproveStatus == "2" && apprStatus == "0";
            }

            if (allowChange) {
                var estid = $(obj).attr("hdnEstId");

                $('#<%=hdnEstId.ClientID%>').val(estid);
                $('#<%=hdnApprStatus.ClientID%>').val(apprStatus);
                $('#<%=txtApproveStatusComment.ClientID%>').val("");

                var wnd = $find('<%=RadWindowApproveStatusComment.ClientID %>');
                wnd.Show();

                Materialize.updateTextFields();
            } else {
                $(obj).val(currApproveStatus);
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function CloseApproveStatusCommentModal() {
            var wnd = $find('<%=RadWindowApproveStatusComment.ClientID %>');
            wnd.Close();
        }

        <%--function ConfirmDropDownValueChange(source, arguments) {
            arguments.IsValid = confirm("Are you sure?");

            $('#<%=txtApproveStatusComment.ClientID%>').val("");

            var wnd = $find('<%=RadWindowApproveStatusComment.ClientID %>');
            wnd.Show();

            Materialize.updateTextFields();
        }--%>

        function HoverMenutext(row, tooltip, event) {
            var left = event.pageX + 'px';
            $('#' + tooltip).css({ left: left }).show();
        }

        function EmailAllConfirm() {
            var mess = "Are you sure you want to email all proposals?";
            if (confirm(mess)) { return true; }
            else { return false; }
        }




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
            var menu = args.get_menu();

            for (var i = 0; i < menu.get_items().get_count(); i++) {
                var item = menu.get_items().getItem(i);
                if (item.get_value() != 'ColumnsContainer') {
                    item.get_element().style.display = 'none';
                }
            }

            var columnsItem = menu.findItemByText("Columns");
            columnsItem.get_items().getItem(0).get_element().style.display = "none";
            //columnsItem.get_items().getItem(1).get_element().style.display = "none";
            //columnsItem.get_items().getItem(2).get_element().style.display = "none";
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


