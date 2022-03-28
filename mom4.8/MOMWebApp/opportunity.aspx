<%@ Page Title="Opportunities || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="opportunity" Codebehind="opportunity.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function CheckDelete() {
            var result = false;
            $("#<%=RadGrid_Opportunity.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this Opportunity ?');
            }
            else {
                alert('Please select an Opportunity to delete.')
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

                $('#dynamicUI').append('<li><a href="OpportunityListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Opportunity"><span><img src=images/reportfolder.png> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

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
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <style>
        div.RadGrid .rgHeader {
            white-space: nowrap;
        }
    </style>
    <%--Receive Payment GRID--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Opportunity" runat="server">
        <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="RadGrid_Opportunity">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Opportunity" LoadingPanelID="RadAjaxLoadingPanel_Opportunity" />
                
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
 
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Opportunity" LoadingPanelID="RadAjaxLoadingPanel_Opportunity" />
                    <telerik:AjaxUpdatedControl ControlID="txtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="txtTo" /> 
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
 
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Opportunity" LoadingPanelID="RadAjaxLoadingPanel_Opportunity" />
                <%--    <telerik:AjaxUpdatedControl ControlID="RadAP_Opp_DateRange" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Opp_SearchPanel" />--%>
                      <telerik:AjaxUpdatedControl ControlID="txtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="txtTo" />
                     <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                     <telerik:AjaxUpdatedControl ControlID="ddlProbab" />
                     <telerik:AjaxUpdatedControl ControlID="ddlAssigned" />
                     <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                       <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                        <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkCLear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Opportunity" LoadingPanelID="RadAjaxLoadingPanel_Opportunity" />
                 <%--   <telerik:AjaxUpdatedControl ControlID="RadAP_Opp_DateRange" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Opp_SearchPanel" />--%>
                  <telerik:AjaxUpdatedControl ControlID="txtFrom" />
                    <telerik:AjaxUpdatedControl ControlID="txtTo" />
                     <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                     <telerik:AjaxUpdatedControl ControlID="ddlProbab" />
                     <telerik:AjaxUpdatedControl ControlID="ddlAssigned" />
                     <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                       <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                        <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                </UpdatedControls>
            </telerik:AjaxSetting>
           <%-- <telerik:AjaxSetting AjaxControlID="ddlSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Opportunity" LoadingPanelID="RadAjaxLoadingPanel_Opportunity" />
                        <telerik:AjaxUpdatedControl ControlID="RadAP_Opp_DateRange" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Opp_SearchPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
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
                                    <div class="page-title">
                                        <i class="mdi-image-style"></i>&nbsp;
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Opportunities</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <a id="lnknAdd" runat="server" href="addopprt.aspx">Add</a>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkCopy" runat="server" CausesValidation="False" OnClick="lnkCopy_Click">Copy</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>
                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return CheckDelete();" CausesValidation="False" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <ul id="dropdown1" class="dropdown-content">
                                                    <li>
                                                        <asp:LinkButton ID="lnkOpportunityReport" runat="server" CausesValidation="False" OnClick="lnkOpportunityReport_Click">Opportunity Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkOpportunityForecastReport" runat="server" CausesValidation="False" OnClick="lnkOpportunityForecastReport_Click">Open Opportunities by Month [M&R, Repair, Other]</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkOpportunityForecast1Report" runat="server" CausesValidation="False" OnClick="lnkOpportunityForecast1Report_Click">Open Opportunities by Month [Maintenance & Modernization]</asp:LinkButton>
                                                    </li>
                                                </ul>
                                                <div class="btnlinks">
                                                    <a class="dropdown-button" id="lnkReport" runat="server" data-beloworigin="true" href="#" data-activates="dropdown1">Reports
                                                    </a>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false"
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
                <div class="srchpaneinner">
                  <%--  <telerik:RadAjaxPanel runat="server" ID="RadAP_Opp_DateRange">--%>
                        <div class="srchtitle srchtitlecustomwidth ser-css2" style="padding-left: 15px;">
                            Date
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtFrom" runat="server" CssClass="datepicker_mom srchcstm" placeholder="From" MaxLength="28" style="width: 88px;"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtTo" runat="server" CssClass="datepicker_mom srchcstm" placeholder="To" MaxLength="28" style="width: 88px;"></asp:TextBox>
                        </div>
                   <%-- </telerik:RadAjaxPanel>--%>
                    <div class="srchinputwrap">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="True" ID="decDate" Style="margin-right: 3px;" runat="server" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtTo','ctl00_ContentPlaceHolder1_txtFrom','rdCal');return false;" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>"></asp:LinkButton>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#lblDay', 'hdnOppDate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnOppDate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnOppDate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnOppDate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFrom', 'ctl00_ContentPlaceHolder1_txtTo', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnOppDate', 'rdCal')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton AutoPostBack="True" ID="incDate" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtTo','ctl00_ContentPlaceHolder1_txtFrom','rdCal');return false" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>

                    </div>
                     <div class="srchpaneinner1">
                            <div class="srchtitle srchtitlecustomwidth ser-css2" style="padding-left: 15px;">
                                Search
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="false" onchange="ShowHideFilterSearch();"
                                    CssClass="browser-default selectst selectsml">
                                    <asp:ListItem Value=" ">--Select--</asp:ListItem>
                                    <asp:ListItem Value="l.ID">Opp #</asp:ListItem>
                                    <%--<asp:ListItem Value="l.estimate">Estimate #</asp:ListItem>--%>
                                    <%--<asp:ListItem Value="job">Project #</asp:ListItem>--%>
                                    <asp:ListItem Value="r.name">Location Name</asp:ListItem>
                                    <asp:ListItem Value="l.fdesc">Opp Name</asp:ListItem>
                                    <asp:ListItem Value="l.CompanyName">Customer Name</asp:ListItem>
                                    <asp:ListItem Value="l.Probability">Probability</asp:ListItem>
                                    <asp:ListItem Value="l.status">Status</asp:ListItem>
                                    <asp:ListItem Value="l.fuser">Salesperson</asp:ListItem>
                                    <asp:ListItem Value="l.CloseDate">Bid Date</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..." style="width: 100px;"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlStatus" Style="display: none" runat="server" CssClass="browser-default selectst">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlProbab" runat="server" CssClass="browser-default selectst" Style="display: none; width: 100px;">
                                    <asp:ListItem Value="0">Excellent</asp:ListItem>
                                    <asp:ListItem Value="1">Very Good</asp:ListItem>
                                    <asp:ListItem Value="2">Good</asp:ListItem>
                                    <asp:ListItem Value="3">Average</asp:ListItem>
                                    <asp:ListItem Value="4">Poor</asp:ListItem>
                                </asp:DropDownList>

                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlAssigned" Style="display: none" runat="server" CssClass="browser-default selectst">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap srchclr btnlinksicon m-lm-t" style="margin-left: -15px; margin-top: -2px;">
                                <asp:LinkButton CausesValidation="false" ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                            </div>


                        </div>

                    <div class="col lblsz2 lblszfloat">
                        <div class="row">

                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" CausesValidation="False" OnClick="lnkShowAll_Click" OnClientClick="ResetShowAll();">Show All</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" OnClientClick="ResetShowAll();">Clear </asp:LinkButton>
                            </span>
                           <span class="tro trost">
                                <%--<asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                 <%--   </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </span>
                        </div>
                    </div>
                </div>
                <%-- <telerik:RadAjaxPanel runat="server" ID="RadAP_Opp_SearchPanel">--%>
                <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                       

                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
             <%--   </telerik:RadAjaxPanel>--%>
            </div>
            <div class="grid_container">
                <div class="form-section-row m-b-0" style="margin-bottom: 0 !important;">
                    
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Opportunity" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="RadCodeBlock_Opportunity" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Opportunity.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Opportunity" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Opportunity" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Opportunity" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_Opportunity_PreRender"
                                AllowCustomPaging="True" 
                                OnNeedDataSource="RadGrid_Opportunity_NeedDataSource" 
                                OnExcelMLExportRowCreated="RadGrid_Opportunity_ExcelMLExportRowCreated" 
                                OnItemCreated="RadGrid_Opportunity_ItemCreated" 
                                OnItemDataBound="RadGrid_Opportunity_ItemDataBound" 
                                OnItemEvent="RadGrid_Opportunity_ItemEvent"
                                 OnPageIndexChanged="RadGrid_Opportunity_PageIndexChanged"
                                OnPageSizeChanged="RadGrid_Opportunity_PageSizeChanged"
                                >
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Name">
                                    <Columns>

                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Image ID="imgDoc" runat="server" Width="16px" ToolTip="Documents" ImageUrl='<%# Eval("DocumentCount").ToString() != "0" ? "images/Document.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="id" UniqueName="id" AutoPostBackOnFilter="true" SortExpression="id" HeaderText="Opportunity #" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <a href="addopprt.aspx?uid=<%# Eval("id") %>"><%# Eval("id") %></a>
                                                <asp:Label ID="lblID" Visible="false" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="name" UniqueName="name" HeaderText="Location Name"
                                            AutoPostBackOnFilter="true" SortExpression="name" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="fdesc" UniqueName="fdesc" HeaderText="Opportunity Name" HeaderStyle-Width="130"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fdesc"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="CompanyName" UniqueName="CompanyName" HeaderText="Customer" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CompanyName"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="fuser" UniqueName="fuser" HeaderText="Assigned To" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fuser"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="CreateDate" UniqueName="CreateDate" SortExpression="CreateDate" HeaderText="Date Created" ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="140" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("CreateDate","{0:d}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Probability" UniqueName="Probability" HeaderText="Probability" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Probability"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Product" UniqueName="Product" HeaderText="Product" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Product"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Status" UniqueName="Status" HeaderText="Status" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Status"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Company" UniqueName="Company" HeaderText="Company" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Company"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="fFor" UniqueName="fFor" HeaderText="Type" SortExpression="fFor"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Convert.ToString(Eval("fFor")).ToUpper()=="ACCOUNT"?"Existing":"Lead" %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="StageWithProbability" UniqueName="Stage" HeaderText="Stage" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="StageWithProbability"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Dept" UniqueName="Dept" HeaderText="Department" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Dept"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="revenue" UniqueName="revenue" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="revenue" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Budgeted Amt" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "revenue", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="BidPrice" UniqueName="BidPrice" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="BidPrice" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Bid Price" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBidPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BidPrice", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="FinalBid" UniqueName="FinalBid" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FinalBid" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Final Bid" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFinalBid" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FinalBid", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="estimate" UniqueName="estimate" SortExpression="estimate" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Estimate #" ShowFilterIcon="false">
                                            <ItemTemplate> 
                                                <asp:HiddenField ID="hdnGridEstimate" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "estimate")%>'></asp:HiddenField>
                                                <asp:Repeater ID="rptEstimates" runat="server"> 
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEstimate" runat="server" CommandName="Estimate #" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EstimateID")%>' OnCommand="LinkButton_Click"><%#DataBinder.Eval(Container.DataItem, "EstimateID")%></asp:LinkButton>
                                                        <asp:Label ID="lblComma" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Last") == "false" ? ", " : ""%>'></asp:Label>
                                                    </ItemTemplate> 
                                                </asp:Repeater> 
                                           </ItemTemplate> 
                                        </telerik:GridTemplateColumn> 

                                        <telerik:GridBoundColumn DataField="EstimateDiscounted" UniqueName="EstimateDiscounted" HeaderText="Discounted" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="EstimateDiscounted"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="job" UniqueName="job" SortExpression="job" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Project #" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnGridProject" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "job")%>'></asp:HiddenField>
                                                <asp:Repeater ID="rptProjects" runat="server"> 
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkjob" runat="server" CommandName="Project #" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ProjectID")%>' OnCommand="LinkButton_Click"><%#DataBinder.Eval(Container.DataItem, "ProjectID")%></asp:LinkButton>
                                                        <asp:Label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Last") == "false" ? ", " : ""%>'></asp:Label>
                                                    </ItemTemplate> 
                                                </asp:Repeater> 
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="closedate" UniqueName="closedate" SortExpression="closedate" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Bid Date" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToString(Eval("status"))=="Open"?"":Eval("closedate","{0:d}" )%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Referral" UniqueName="Referral" HeaderText="Referral" HeaderStyle-Width="115"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Referral"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnOppSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
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
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            //debugger;
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblOppActive"])%>';
                var val;
                val = localStorage.getItem("hdnOppDate");
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
                        if (DATE.getMonth() == 5)
                        { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else
                        { NEWDATE.setDate(NEWDATE.getDate()); }

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
                document.getElementById('<%= hdnOppSelectDtRange.ClientID%>').value = "Day";
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
                document.getElementById('<%= hdnOppSelectDtRange.ClientID%>').value = "Week";
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
                document.getElementById('<%= hdnOppSelectDtRange.ClientID%>').value = "Month";
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
                document.getElementById('<%= hdnOppSelectDtRange.ClientID%>').value = "Quarter";
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
                document.getElementById('<%= hdnOppSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnOppSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
    <script>
        function ShowHideFilterSearch() {
            
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var ddlStatus =$("#<%=ddlStatus.ClientID%>"); 
            var ddlProbab =$("#<%=ddlProbab.ClientID%>"); 
            var ddlAssigned = $("#<%=ddlAssigned.ClientID%>");
            var txtSearch =$("#<%=txtSearch.ClientID%>"); 

            
             ddlStatus.css("display", "none");
             ddlProbab.css("display", "none");
             ddlAssigned.css("display", "none");
             txtSearch.css("display", "none");           
             txtSearch.val('');
           
            if (ddlSearch.val() === "l.status") {
                ddlStatus.css("display", "block");
            } else if (ddlSearch.val() === "l.Probability") {
                ddlProbab.css("display", "block");
            } else if (ddlSearch.val() === "l.fuser") {
                ddlAssigned.css("display", "block");
            } else if (ddlSearch.val() === "l.CloseDate") {
                txtSearch.css("display", "none");
            } else {
                  txtSearch.css("display", "block");           
            }

            
             try {
                 ddlStatus.get(0).selectedIndex = 0;
                 ddlProbab.get(0).selectedIndex = 0;
                 ddlAssigned.get(0).selectedIndex = 0;                
             } catch (ex) { }     
           
        }
        function ResetShowAll() {
            
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var ddlStatus =$("#<%=ddlStatus.ClientID%>"); 
            var ddlProbab =$("#<%=ddlProbab.ClientID%>"); 
            var ddlAssigned = $("#<%=ddlAssigned.ClientID%>");
            var txtSearch =$("#<%=txtSearch.ClientID%>"); 

            
             ddlStatus.css("display", "none");
             ddlProbab.css("display", "none");
             ddlAssigned.css("display", "none");
             txtSearch.css("display", "block");           
             txtSearch.val('');          
           

            
             try {
                 ddlStatus.get(0).selectedIndex = 0;
                 ddlProbab.get(0).selectedIndex = 0;
                 ddlAssigned.get(0).selectedIndex = 0;                
             } catch (ex) { }     
           
        }
    </script>
</asp:Content>
