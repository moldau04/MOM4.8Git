<%@ Page Title="Payment History || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="PaymentHistory" Codebehind="PaymentHistory.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--INVOICES GRID--%>
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

                                    <div class="page-title"><i class="mdi-action-payment"></i>&nbsp;Payment History</div>
                                    <div class="buttonContainer">
                                        <ul id="dropdown1" class="dropdown-content">
                                            <li>
                                                <a href="CustomersReport.aspx?type=Customer">Add New Report</a>
                                            </li>

                                        </ul>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                            </a>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap tabcontainer">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnPaymentDate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnPaymentDate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnPaymentDate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnPaymentDate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnPaymentDate', 'rdCal')" />
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
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRecordCount" runat="server" Style="display: block"></asp:Label>
                                        <asp:Label ID="lblRecordCountAch" runat="server" Style="display: none"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">

                    <ContentTemplate>
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Search
                            </div>
                            <div class="srchinputwrap">

                                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="srchcstm"></asp:TextBox>
                                <label for="txtInvoiceNo">Invoice #</label>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlUser" runat="server" CssClass="browser-default selectst" Visible="False"></asp:DropDownList>
                            </div>
                            <div class="srchtitle srchtitlecustomwidth">
                                <asp:Label ID="lblCustomer" runat="server" Text="Customer"></asp:Label>
                            </div>
                            <div class="srchinputwrap">

                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="browser-default selectst">
                                </asp:DropDownList>
                            </div>
                            <div class="srchtitle srchtitlecustomwidth">
                                Status
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlMedium" runat="server" CssClass="browser-default selectst" Visible="False">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                    <asp:ListItem Value="MSM">MSM</asp:ListItem>
                                    <asp:ListItem Value="Mobile Service">Mobile Service</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">

                                <asp:DropDownList ID="ddlApproved" runat="server" CssClass="browser-default selectst">
                                    <asp:ListItem Value="-1">All</asp:ListItem>
                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                    <asp:ListItem Value="0">Declined</asp:ListItem>
                                    <asp:ListItem Value="2">Sent</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap srchclr btnlinksicon">
                                <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" CausesValidation="false"><i class="mdi-action-search"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col s12">
                <div class="row">
                    <div class="card" style="min-height: 70vh !important; border-radius: 6px; margin-top: -10px;">
                        <div class="card-content">
                            <ul class="tabs tab-demo-active white" style="width: 100%;">
                                <li class="tab col s2">
                                    <a class="white-text waves-effect waves-light active" href="#activeone" onclick="ShowCreditlable()"><i class="mdi-action-done"></i>&nbsp;Credit Card</a>
                                </li>
                                <li class="tab col s2">
                                    <a class="white-text waves-effect waves-light" href="#two" onclick="ShowACHlable()"><i class="mdi-notification-sync-problem"></i>&nbsp;ACH</a>
                                </li>
                            </ul>
                            <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block; margin-top: 20px">
                                <div class="grid_container">
                                    <div class="form-section-row" style="margin-bottom: 0 !important;">

                                        <telerik:RadAjaxManager ID="RadAjaxManager_CreditCard" runat="server">
                                            <AjaxSettings>
                                                <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                                    <UpdatedControls>
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_CreditCard" LoadingPanelID="RadAjaxLoadingPanel_CreditCard" />
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_ACH" LoadingPanelID="RadAjaxLoadingPanel_ACH" />
                                                    </UpdatedControls>
                                                </telerik:AjaxSetting>
                                                <telerik:AjaxSetting AjaxControlID="lnkClear">
                                                    <UpdatedControls>
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_CreditCard" LoadingPanelID="RadAjaxLoadingPanel_CreditCard" />
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_ACH" LoadingPanelID="RadAjaxLoadingPanel_ACH" />
                                                    </UpdatedControls>
                                                </telerik:AjaxSetting>
                                                <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                                    <UpdatedControls>
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_CreditCard" LoadingPanelID="RadAjaxLoadingPanel_CreditCard" />
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_ACH" LoadingPanelID="RadAjaxLoadingPanel_ACH" />
                                                    </UpdatedControls>
                                                </telerik:AjaxSetting>
                                                <telerik:AjaxSetting AjaxControlID="btnDeclined">
                                                    <UpdatedControls>
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_ACH" LoadingPanelID="RadAjaxLoadingPanel_ACH" />
                                                    </UpdatedControls>
                                                </telerik:AjaxSetting>
                                                <telerik:AjaxSetting AjaxControlID="btnApprove">
                                                    <UpdatedControls>
                                                        <telerik:AjaxUpdatedControl ControlID="RadGrid_ACH" LoadingPanelID="RadAjaxLoadingPanel_ACH" />
                                                    </UpdatedControls>
                                                </telerik:AjaxSetting>
                                            </AjaxSettings>
                                        </telerik:RadAjaxManager>
                                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_CreditCard" runat="server">
                                        </telerik:RadAjaxLoadingPanel>
                                        <div class="RadGrid RadGrid_Material">
                                            <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                                <script type="text/javascript">
                                                    function pageLoad() {
                                                        var grid = $find("<%= RadGrid_CreditCard.ClientID %>");
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
                                            <telerik:RadAjaxPanel ID="RadAjaxPanel_CreditCard" runat="server" LoadingPanelID="RadAjaxLoadingPanel_CreditCard" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                <telerik:RadPersistenceManager ID="RadPersistenceCreditCard" runat="server">
                                                    <PersistenceSettings>
                                                        <telerik:PersistenceSetting ControlID="RadGrid_CreditCard" />
                                                    </PersistenceSettings>
                                                </telerik:RadPersistenceManager>
                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_CreditCard" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                    ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                    AllowCustomPaging="True" OnNeedDataSource="RadGrid_CreditCard_NeedDataSource" OnItemEvent="RadGrid_CreditCard_ItemEvent" OnItemCreated="RadGrid_CreditCard_ItemCreated">
                                                    <CommandItemStyle />
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>

                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="TransactionID" HeaderText="ID" SortExpression="TransactionID" DataType="System.String"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="InvoiceID" HeaderText="Invoice #" SortExpression="InvoiceID" DataType="System.String"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn DataField="TransDate" SortExpression="TransDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                CurrentFilterFunction="Contains" HeaderText="Transaction Date" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransDate" runat="server" Text='<%# Eval("TransDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DataField="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="Response" HeaderText="Status" SortExpression="Response"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="owner" HeaderText="Customer" SortExpression="owner"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="tag" HeaderText="Location" SortExpression="tag"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="paymentUID" HeaderText="Transaction ID" SortExpression="paymentUID"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="cardnumber" HeaderText="Card#" SortExpression="cardnumber"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
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
                            <div id="two" class="col s12 tab-container-border lighten-4" style="margin-top: 20px;">
                                <div class="grid_container">
                                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_ACH" runat="server">
                                        </telerik:RadAjaxLoadingPanel>
                                        <div class="RadGrid RadGrid_Material">
                                            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                <script type="text/javascript">
                                                    function pageLoad() {
                                                        var grid = $find("<%= RadGrid_ACH.ClientID %>");
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
                                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_ACH" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_ACH" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                    ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                    AllowCustomPaging="True" OnNeedDataSource="RadGrid_ACH_NeedDataSource" OnItemEvent="RadGrid_ACH_ItemEvent" OnItemCommand="RadGrid_ACH_ItemCommand" OnItemCreated="RadGrid_ACH_ItemCreated">
                                                    <CommandItemStyle />
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Approve" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <div class="btnlinks">
                                                                        <asp:LinkButton ID="btnApprove" Font-Bold="true" AutoPostBack="true" OnClientClick="return confirm('Are you sure？')" Width="75px" Visible='<%# Eval("Response").ToString() =="Sent"?true:false %>' Text="Approve" ForeColor='<%# Eval("Response").ToString() =="Sent"?System.Drawing.Color.Green:System.Drawing.Color.Gray %>'
                                                                            runat="server" CommandName="Approved" CommandArgument='<%# Eval("paymentUID") +":"+ Eval("Invoice")  %>'>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div class="btnlinks">
                                                                        <asp:LinkButton ID="btnDeclined" AutoPostBack="true" Width="75px" OnClientClick="return confirm('Are you sure？')" Visible='<%# Eval("Response").ToString() =="Sent"?true:false %>' Font-Bold="true" ForeColor='<%# Eval("Response").ToString() =="Sent"?System.Drawing.Color.Red:System.Drawing.Color.Gray %>'
                                                                            Text="Decline" runat="server" CommandName="Declined" CommandArgument='<%# Eval("paymentUID") +":"+ Eval("Invoice")  %>'>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div class="btnlinks">
                                                                        <asp:LinkButton ID="LinkButton1" OnClientClick="return false;" AutoPostBack="false" Width="75px" Visible='<%# Eval("Response").ToString() =="Sent"?false:true %>' ForeColor="Gray" Text="Decline" runat="server" />
                                                                        <asp:LinkButton ID="LinkButton2" OnClientClick="return false;" AutoPostBack="false" Width="75px" Visible='<%# Eval("Response").ToString() =="Sent"?false:true %>' ForeColor="Gray" Text="Approve" runat="server" />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="owner" HeaderText="Customer" SortExpression="owner"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="paymentUID" HeaderText="TransactionID" SortExpression="paymentUID"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn DataField="TransDate" SortExpression="TransDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                CurrentFilterFunction="Contains" HeaderText="Transaction Date" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransDateGd" runat="server" Text='<%# Eval("TransDate", "{0:M/d/yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="Invoice" HeaderText="Invoice #" SortExpression="Invoice" DataType="System.String"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Routing" HeaderText="RoutingNo" SortExpression="Routing"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="BankAccNo" HeaderText="AccountNo" SortExpression="BankAccNo"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="NameAccHolder" HeaderText="AccountHolderName" SortExpression="NameAccHolder" HeaderStyle-Width="140"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn DataField="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="Response" HeaderText="Status" SortExpression="Response"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                ShowFilterIcon="false">
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
                    </div>
                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnPymtHstSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script>
        function ShowCreditlable() {

            document.getElementById("ctl00_ContentPlaceHolder1_lblRecordCount").style.display = "block";
            document.getElementById("ctl00_ContentPlaceHolder1_lblRecordCountAch").style.display = "none";
        }
        function ShowACHlable() {

            document.getElementById("ctl00_ContentPlaceHolder1_lblRecordCountAch").style.display = "block";
            document.getElementById("ctl00_ContentPlaceHolder1_lblRecordCount").style.display = "none";
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

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            debugger;
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblPaymtActive"])%>';
                var val;
                val = localStorage.getItem("hdnPaymentDate");
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
                        $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                        document.getElementById("rdMonth").checked = true;
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
                document.getElementById('<%= hdnPymtHstSelectDtRange.ClientID%>').value = "Day";
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
                document.getElementById('<%= hdnPymtHstSelectDtRange.ClientID%>').value = "Week";
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
                document.getElementById('<%= hdnPymtHstSelectDtRange.ClientID%>').value = "Month";
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
                document.getElementById('<%= hdnPymtHstSelectDtRange.ClientID%>').value = "Quarter";
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
                document.getElementById('<%= hdnPymtHstSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnPymtHstSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
</asp:Content>
