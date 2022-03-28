 <%@ Page Title="Receive PO || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Codebehind="ManageReceivePO.aspx.cs" Inherits="ManageReceivePO" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        @media only screen and (min-width: 250px) and (max-width: 700px) {
            .srchpane{
                text-align:center;
            }
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
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <div class="page-title">
                                                <i class="mdi-communication-contacts"></i>&nbsp;Manage Reception PO</div>
                                            <div class="btnlinks page-title">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                 <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                                  <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                                </a>
                                            </div>
                                           
                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <%--      <a href="ReceivePOReport.aspx" class="-text">ReceivePO Report</a>--%>
                                                    <asp:LinkButton ID="lnkReceivePOReport" runat="server" CausesValidation="False" OnClick="lnkReceivePOReport_Click">Receive Item Report</asp:LinkButton>
                                                </li>
                                            </ul>
                                            <div class="rght-content">
                                                <div class="btnclosewrap">
                                                    <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                                </div>
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
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Date
                    </div>
                    <div class="srchinputwrap pd-negatenw input-field">
                        <%--<label for="txtFromDate">From</label>--%>
                        <asp:TextBox ID="txtFromDate" placeholder="From" runat="server" CssClass="datepicker_mom" MaxLength="28"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap pd-negatenw input-field">
                        <%--<label for="txtToDate">To</label>--%>
                        <asp:TextBox ID="txtToDate" placeholder="To" runat="server" CssClass="datepicker_mom" MaxLength="28"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap pd-negatenw">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblDay', 'hdnReceivePODate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnReceivePODate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnReceivePODate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnReceivePODate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnReceivePODate', 'rdCal')" />
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
                                <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
                                <asp:Label ID="lblChkSelect" runat="server">Incl. Closed</asp:Label>
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
                <%--<asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Search
                            </div>
                            <div class="srchinputwrap pd-negatenw">
                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst selectsml" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="p.po">PO #</asp:ListItem>
                                    <asp:ListItem Value="r.id">RID #</asp:ListItem>
                                    <asp:ListItem Value="p.vendor">VendorID #</asp:ListItem>
                                    <asp:ListItem Value="ro.Name">VendorName</asp:ListItem>
                                    <asp:ListItem Value="r.status">Status</asp:ListItem>
                                    <asp:ListItem Value="p.inventorytype">InventoryType</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst" Visible="false">
                                    <asp:ListItem Value="0">Open </asp:ListItem>
                                    <asp:ListItem Value="1">Closed </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap pd-negatenw input-field">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap pd-negatenw input-field">
                                <asp:TextBox ID="txtReceivePoSearch" runat="server" CssClass="srchcstm" onkeypress="return isNumberKey(this,event)" Visible="false"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap srchclr btnlinksicon" >
                                <div class="btnlinksicon">
                                    <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">

                    <telerik:RadAjaxManager ID="RadAjaxManager_ReceivePO" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ReceivePO" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ReceivePO" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RadGrid_ReceivePO">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ReceivePO" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ReceivePO" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate"  />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_ReceivePO" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <%--<telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_ReceivePO.ClientID %>");
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
                        </telerik:RadCodeBlock>--%>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_ReceivePO" runat="server" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" >
                          <%--  <telerik:RadPersistenceManager ID="RadPersistenceReceivePO" runat="server">
                                <PersistenceSettings>
                                    <telerik:PersistenceSetting ControlID="RadGrid_ReceivePO" />
                                </PersistenceSettings>
                            </telerik:RadPersistenceManager>--%>
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_ReceivePO" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_ReceivePO_NeedDataSource" OnPreRender="RadGrid_ReceivePO_PreRender" OnItemCreated="RadGrid_ReceivePO_ItemCreated" OnItemEvent="RadGrid_ReceivePO_ItemEvent">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn DataField="ID" SortExpression="ID" AutoPostBackOnFilter="true" DataType="System.String" UniqueName="ID"
                                            CurrentFilterFunction="Contains" HeaderText="Reception No#" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
                                                <asp:Label ID="lblID" Text='<%# Eval("ID")%>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="true" DataType="System.String" UniqueName="fDate"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDate" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> ' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" DataType="System.String" UniqueName="Ref"
                                            CurrentFilterFunction="Contains" HeaderText="Ref #" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkRef" runat="server" Text='<%# Bind("Ref") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="fDesc" HeaderText="Description" SortExpression="fDesc" UniqueName="fDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="200">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName" UniqueName="VendorName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="StatusName" HeaderText="Status" SortExpression="StatusName" UniqueName="StatusName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" UniqueName="Company"
                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="PO" HeaderText="PO" SortExpression="PO" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String" UniqueName="PO"
                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="ReceivedAmount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="ReceivedAmount" AutoPostBackOnFilter="true" UniqueName="ReceivedAmount"
                                            CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReceivedAmount", "{0:c}")%>'
                                                    ForeColor='<%# Convert.ToDouble(Eval("ReceivedAmount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
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
    <asp:HiddenField runat="server" ID="hdnReceivePOSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
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
        <%--$(document).ready(function () {

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            if (typeof (Storage) !== "undefined") {
                debugger;
                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblReceivePOActive"])%>';
                var val;
                val = localStorage.getItem("hdnReceivePODate");
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
        });--%>
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
                CssClearLabel();
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReceivePOSelectDtRange.ClientID%>').value = "Day";
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
                CssClearLabel();
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReceivePOSelectDtRange.ClientID%>').value = "Week";
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
                CssClearLabel();
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReceivePOSelectDtRange.ClientID%>').value = "Month";
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
                CssClearLabel();
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReceivePOSelectDtRange.ClientID%>').value = "Quarter";
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
                CssClearLabel();
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnReceivePOSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnReceivePOSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
</asp:Content>


