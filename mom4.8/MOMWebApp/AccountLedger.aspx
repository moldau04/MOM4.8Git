<%@ Page Title="Account Ledger || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AccountLedger" CodeBehind="AccountLedger.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="height: 65px !important;">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-content-reply-all"></i>&nbsp;Account Ledger</div>


                                    <div class="btnlinks">
                                        <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                        </a>
                                    </div>
                                    <ul id="dropdown1" class="dropdown-content">
                                        <li>
                                            <a href="AccountLedger.aspx?type=Customer" class="-text">Add New Report</a>
                                        </li>
                                        <li>
                                            <a href="AccountLedgerReport.aspx" class="-text">Account Ledger Report</a>
                                        </li>
                                    </ul>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                    </div>
                                   
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblAccountName" runat="server"></asp:Label>
                                            &nbsp; |
                                        </div>
                                        <div class="editlabel">
                                            <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClosed" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="srchcstm datepicker_mom" placeholder="Start"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="srchcstm datepicker_mom" placeholder="End"></asp:TextBox>
                    </div>

                     <div class="srchinputwrap tabcontainer">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtEndDate','ctl00_ContentPlaceHolder1_txtStartDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
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
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtEndDate','ctl00_ContentPlaceHolder1_txtStartDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>

                    <div class="srchinputwrap srchclr btnlinksicon">
                        <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i></asp:LinkButton>
                    </div>

                    <div class="col lblsz2 lblszfloat">
                        <div class="row">

                            <%--   <span class="tro trost">
                                <asp:CheckBox ID="lnkChk" runat="server" CssClass="css-checkbox" Text="Incl. Inactive"></asp:CheckBox>
                            </span>--%>
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
            </div>
            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">

                    <telerik:RadAjaxManager ID="RadAjaxManager_AccountLedger" runat="server">
                        <AjaxSettings>

                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_AccountLedger" LoadingPanelID="RadAjaxLoadingPanel_AccountLedger" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_AccountLedger" LoadingPanelID="RadAjaxLoadingPanel_AccountLedger" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_AccountLedger" LoadingPanelID="RadAjaxLoadingPanel_AccountLedger" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_AccountLedger" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_AccountLedger.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_AccountLedger" runat="server" LoadingPanelID="RadAjaxLoadingPanel_AccountLedger" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_AccountLedger" 
                                AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" 
                                AllowCustomPaging="True" 
                                VirtualItemCount="1000"
                                AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" 
                                OnPreRender="RadGrid_AccountLedger_PreRender" 
                                OnItemDataBound="RadGrid_AccountLedger_ItemDataBound"
                      
                                OnNeedDataSource="RadGrid_AccountLedger_NeedDataSource" 
                                OnItemEvent="RadGrid_AccountLedger_ItemEvent" 
                                OnItemCreated="RadGrid_AccountLedger_ItemCreated"
                       
                                >
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn SortExpression="fDates" UniqueName="fDates" AutoPostBackOnFilter="true" DataField="fDates" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFDate" runat="server" Text='<%# Eval("fDates", "{0:M/d/yyyy}") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnBatch" runat="server" Value='<%#Eval("Batch") %>' />
                                                <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("Type") %>' />
                                                <asp:HiddenField ID="hdnRef" runat="server" Value='<%#Eval("Ref") %>' />
                                                 <asp:HiddenField ID="hdnlink" runat="server" Value='<%#Eval("link") %>' />
                                                <asp:HiddenField ID="hdnTypeText" runat="server" Value='<%#Eval("TypeText") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label Text="Account Balance" runat="server" Font-Bold="True" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="TypeText" UniqueName="Type" HeaderText="Type" SortExpression="TypeText"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("TypeText") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblAccountBalance" runat="server" Font-Bold="True" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="Ref" UniqueName="Ref" AutoPostBackOnFilter="true" DataField="Ref" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Ref" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlkRef" runat="server" Text='<%# Bind("Ref") %>' Target="_blank" NavigateUrl='<%# Eval("link") %>' ForeColor="#0066CC"></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="fDesc" UniqueName="Description" HeaderText="Description" SortExpression="fDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesc" Text='<%#Bind("fDesc") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div style="padding: 0 0 5px 0">
                                                    <asp:Label Text="Page Total" runat="server" Font-Bold="True" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Grand Total" runat="server" Font-Bold="True" />
                                                </div>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Debit" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Debit" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Debit" ShowFilterIcon="false" UniqueName="Debit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Debit", "{0:c}")%>' CommandArgument='<%#Eval("ID")%>' />
                                                <asp:Label ID="lblBDebit" Text='<%# Eval("Debit")%>' runat="server" Style="display: none;" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div style="padding: 0 0 5px 0">
                                                    <asp:Label ID="lblTotalDebit" runat="server" Font-Bold="True" />
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblGrandTotalDebit" runat="server" Font-Bold="True" />
                                                </div>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Credit" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Credit" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Credit" ShowFilterIcon="false" UniqueName="Credit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCredit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Credit", "{0:c}")%>' CommandArgument='<%#Eval("ID")%>' />
                                                <asp:Label ID="lblBCredit" Text='<%# Eval("Credit")%>' runat="server" Style="display: none;" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div style="padding: 0 0 5px 0">
                                                    <asp:Label ID="lblTotalCredit" runat="server" Font-Italic="False" Font-Bold="True" />
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblGrandTotalCredit" runat="server" Font-Bold="True" />
                                                </div>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Balance" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Balance" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Balance" ShowFilterIcon="false" UniqueName="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("Balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                <asp:Label ID="lblACBalance" Text='<%# Eval("Balance")%>' runat="server" Style="display: none;" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div style="padding: 0 0 5px 0">
                                                    <asp:Label ID="lblTotalBalance" runat="server" Font-Bold="True" />
                                                </div>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>

                        <div style="display=block">
                                <telerik:RadAjaxPanel ID="RadAjaxPanel_AccountLedgerExport" runat="server"  ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_AccountLedgerExport" 
                                AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" 
                                AllowCustomPaging="True"                               
                                AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true"      
                                OnPreRender="RadGrid_AccountLedgerExport_PreRender"
                                OnNeedDataSource="RadGrid_AccountLedgerExport_NeedDataSource"                            
                                OnExcelMLExportRowCreated="RadGrid_AccountLedgerExport_ExcelMLExportRowCreated">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                              
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>                                       
                                        <telerik:GridTemplateColumn  UniqueName="fDates"  DataField="fDates" HeaderText="Date" >                                        
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="Type" HeaderText="Type" DataField="TypeText" >                                            
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn  UniqueName="Ref" DataField="Ref" HeaderText="Ref" > 
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="fDesc" UniqueName="Description" HeaderText="Description" >
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Debit"   HeaderText="Debit"  UniqueName="Debit">
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Credit"  HeaderStyle-Width="100" HeaderText="Credit" UniqueName="Credit">
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Balance" HeaderStyle-Width="100" HeaderText="Balance"  UniqueName="Balance">
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Job" UniqueName="Job" ShowFilterIcon="false"  HeaderText="Project #">
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Name"  UniqueName="Name" ShowFilterIcon="false" HeaderText="Name">
                                        </telerik:GridTemplateColumn>
                                      
                                       <telerik:GridTemplateColumn DataField="PO" UniqueName="PO" ShowFilterIcon="false"  HeaderText="PO #">
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Comment"  UniqueName="Comment" ShowFilterIcon="false" HeaderText="PO Comment">
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="OriginalJE"  UniqueName="OriginalJE" ShowFilterIcon="false" HeaderText="Original JE">
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        </div>
                     
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnInvoiceSelectDtRange" Value="Week" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#addinfo').hide();
            $('.add-btn-click').click(function () {
                $('#addinfo').slideToggle('1000000', "swing", function () {
                    // Animation complete.
                });
                if ($('.divbutton-container').height() != 65)
                    $('.divbutton-container').animate({ height: 65 }, 500);
                else
                    $('.divbutton-container').animate({ height: 350 }, 500);
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
            var selected = "rd" + $("#<%=hdnInvoiceSelectDtRange.ClientID%>").val();

            if (document.getElementById(txtDateFrom).value == "") {
                return false;
            }
            if (selected == "rd") {
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
                $("#<%=txtStartDate.ClientID%>").val(datestring);
                $("#<%=txtEndDate.ClientID%>").val(datestring);
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
                $("#<%=txtStartDate.ClientID%>").val(datestring);
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                //document.getElementById(txtdateTo).value = dateString;

                $("#<%=txtEndDate.ClientID%>").val(dateString);
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
                $("#<%=txtStartDate.ClientID%>").val(datestring);
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                $("#<%=txtEndDate.ClientID%>").val(dateString);
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
                $("#<%=txtStartDate.ClientID%>").val(datestring);
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                $("#<%=txtEndDate.ClientID%>").val(dateString);
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
                $("#<%=txtStartDate.ClientID%>").val(datestring);
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                $("#<%=txtEndDate.ClientID%>").val(dateString);
                $("#<%=lblYear.ClientID%>").addClass("labelactive");
                document.getElementById('<%= hdnInvoiceSelectDtRange.ClientID%>').value = "Year";
            }

            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, type);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
</asp:Content>
