<%@ Page Title="Make Deposit || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ManageDeposit" Codebehind="ManageDeposit.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
      <style>
        [id$='PageSizeComboBox']{
           width:5.1em !important;
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
                                    <div class="page-title"><i class="mdi-action-payment"></i>&nbsp;Deposit</div>
                                    <div class="buttonContainer">
                                        <asp:Panel runat="server" ID="pnlGridButtons">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="notyConfirm(); return false; " >Delete</asp:LinkButton>
                                                           <asp:Button ID="lnkDeleteDeposit" runat="server" Text="Delete" Style="display: none;" OnClick="lnkDelete_Click" CausesValidation="False" />
                        
                                                         <asp:HiddenField ID="Confirm_Value" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hdnReconciled" runat="server" ClientIDMode="Static" />
                                                    </div>
                                                </li>
                                                <li>
                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                       
                                                            <asp:LinkButton ID="lnkDepositList" runat="server"  OnClick="lnkDepositList_Click" >Deposit List</asp:LinkButton>
                                                        </li>
                                                         <li>                                                           
                                                             <asp:LinkButton ID="lnkDepositListSalesPerson" runat="server"  OnClick="lnkDepositListSalesPerson_Click" >Deposit List with Default Salesperson</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>

                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>
                                            </ul>


                                        </asp:Panel>
                                    </div>

                                    <div class="btnclosewrap"><asp:LinkButton ID="btnClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton></div>
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
                    <div class="srchtitle">
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap srchclr btnlinksicon margin-t-l">
                        <asp:LinkButton ID="lnkSearch" runat="server"  OnClientClick="return ResetSearch();" OnClick="lnkSearch_Click" CausesValidation="false" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                        <div style="display:none">
                             <asp:LinkButton ID="lnkSearchDate" runat="server"   OnClick="lnkSearchDate_Click" CausesValidation="false" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                        </div>
                    </div>
                    <div class="srchinputwrap tabcontainer">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'hdnDepDate')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'hdnDepDate')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month','hdnDepDate')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter','hdnDepDate')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'hdnDepDate')" />
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
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click"  OnClientClick="ResetValue();">Clear </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" OnClientClick="ResetShowAll()">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <%--<asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                   <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </span>
                        </div>
                    </div>
                </div>

            </div>
            <div class="grid_container">
                <div class="form-section-row mb" >
                    <telerik:RadAjaxManager ID="RadAjaxManager_Deposit" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkDeleteDeposit">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deposit" LoadingPanelID="RadAjaxLoadingPanel_Deposit" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                             <telerik:AjaxSetting AjaxControlID="RadGrid_Deposit">
                                <UpdatedControls>
                                       <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deposit" LoadingPanelID="RadAjaxLoadingPanel_Deposit" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deposit" LoadingPanelID="RadAjaxLoadingPanel_Deposit" />
                                         <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"/>
                                       <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                       <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                              <telerik:AjaxSetting AjaxControlID="lnkSearchDate">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deposit" LoadingPanelID="RadAjaxLoadingPanel_Deposit" />                                        
                                       <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"/>
                                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice"/>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deposit" LoadingPanelID="RadAjaxLoadingPanel_Deposit" />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deposit" LoadingPanelID="RadAjaxLoadingPanel_Deposit" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate"/>
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice"/>
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Deposit" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Deposit.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Deposit" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Deposit" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Deposit" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"  
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True"                               
                                OnNeedDataSource="RadGrid_Deposit_NeedDataSource" 
                                OnPreRender="RadGrid_Deposit_PreRender" 
                                OnItemEvent="RadGrid_Deposit_ItemEvent" 
                                OnItemCreated="RadGrid_Deposit_ItemCreated" 
                                OnExcelMLExportRowCreated="RadGrid_Deposit_ExcelMLExportRowCreated"
                                 OnPageSizeChanged="RadGrid_Deposit_PageSizeChanged"
                                OnPageIndexChanged="RadGrid_Deposit_PageIndexChanged"                               
                                OnItemDataBound="RadGrid_Deposit_ItemDataBound">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True" ></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                            <ItemTemplate>

                                                <asp:Label ID="lblID" Text='<%# Bind("Ref") %>' runat="server"></asp:Label>
                                                <asp:Label ID="lblStatus" Text='<%# Bind("IsRecon") %>' runat="server" />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40" >
                                        </telerik:GridClientSelectColumn>                                        
                                        <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" UniqueName="fDate"  HeaderStyle-Width="40">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDate" runat="server" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> '></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridBoundColumn DataField="Ref" HeaderText="Ref" SortExpression="Ref"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            ShowFilterIcon="false" DataType="System.Int32"  HeaderStyle-Width="40">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fDesc" HeaderText="Description" SortExpression="fDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" UniqueName="fDesc" DataType="System.String"
                                            ShowFilterIcon="false"  HeaderStyle-Width="200">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="BankName" UniqueName="BankName" SortExpression="BankName" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Bank" ShowFilterIcon="false" DataType="System.String"  HeaderStyle-Width="40">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkBank" runat="server" Text='<%# Bind("BankName") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Status" HeaderText="Status" SortExpression="Status" UniqueName="Status"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"  DataType="System.String"
                                            ShowFilterIcon="false"  HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company" UniqueName="Company" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="Amount" UniqueName="Amount"  HeaderStyle-Width="100" FooterAggregateFormatString="{0:c}" Aggregate="Sum" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false"  SortExpression="Amount" DataType="System.Decimal">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
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
    <asp:HiddenField runat="server" ID="hdnDepositSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
      <asp:HiddenField runat="server" ID="ishowAllInvoice" Value="" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function ResetShowAll() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $('#<%=lblQuarter.ClientID%>').removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");           
           
        }
        function ResetSearch() {
            var SesVar =$('#<%= hdnCssActive.ClientID%>').val();
          
            ResetShowAll();
           
           <%-- if ($("#<%=txtFromDate.ClientID%>").val === '' || $("#<%=txtToDate.ClientID%>").val === '') {
                   return false;
                noty({
                    text: 'Please enter valid start date and end date.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
             
            }--%>
            return true;
        }
        function ResetValue() {
             if (document.getElementById('<%= ishowAllInvoice.ClientID%>').value == 1) {
                
                document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value = "Week";
               $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                document.getElementById("rdWeek").checked = true;
            } 
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
        });
        function closedMesg() {
            noty({
                text: 'This deposit is cleared and cannot be deleted.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function DatePermissionAlert(mesg) {
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
        function DeleteSuccessMesg() {
            noty({
                text: 'Deposit deleted successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
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
                var SesVar = '<%= Convert.ToString(Session["lblDepActive"])%>';
                var val;
               // val = localStorage.getItem("hdnDepDate");
                  if (document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value == 'Week') { val = "Week"; }
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
            if ( document.getElementById(txtDateTo).value != '') {
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
                var tt = document.getElementById('<%= txtFromDate.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%= txtFromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%= txtToDate.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%= txtToDate.ClientID%>').value = someFormattedDATE;
            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById('<%= txtFromDate.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%= txtFromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%= txtToDate.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%= txtToDate.ClientID%>').value = someFormattedDATE;
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
                var tt = document.getElementById('<%= txtFromDate.ClientID%>').value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%= txtFromDate.ClientID%>').value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById('<%= txtToDate.ClientID%>').value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById('<%= txtToDate.ClientID%>').value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById('<%= txtToDate.ClientID%>').value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById('<%= txtToDate.ClientID%>').value = someFormattedDate;
                }
            }


            else if (selected == 'rdQuarter') {
                //dec the from date 
                var tt = document.getElementById('<%= txtFromDate.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%= txtFromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%= txtToDate.ClientID%>').value;

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
                document.getElementById('<%= txtToDate.ClientID%>').value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById('<%= txtFromDate.ClientID%>').value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById('<%= txtFromDate.ClientID%>').value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById('<%= txtToDate.ClientID%>').value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById('<%= txtToDate.ClientID%>').value = someFormattedDATE;
            }

            }
            
            return false;

        }
        function SelectDate(type, UniqueVal) {
            ResetShowAll();
            var type = type;           
            var UniqueVal = UniqueVal;          
            debugger
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
               $('#<%= txtToDate.ClientID%>').val(datestring);
                $('#<%= txtFromDate.ClientID%>').val(datestring);
                $('#<%= lblDay.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value = "Day";
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
                 $('#<%= txtFromDate.ClientID%>').val(datestring);
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString2= month + "/" + day + "/" + year;
                  $('#<%= txtToDate.ClientID%>').val(dateString2);            
                 $('#<%= lblWeek.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value = "Week";
            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);
                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                $('#<%= txtFromDate.ClientID%>').val(datestring);
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString2 = month + "/" + day + "/" + year;
                 $('#<%= txtToDate.ClientID%>').val(dateString2);           
                   $('#<%= lblMonth.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value = "Month";
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
                 $('#<%= txtFromDate.ClientID%>').val(datestring);               
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString2 = month + "/" + day + "/" + year;
                $('#<%= txtToDate.ClientID%>').val(dateString2);           
                   $('#<%= lblQuarter.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value = "Quarter";
            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);
                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                  $('#<%= txtFromDate.ClientID%>').val(datestring);
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString2 = month + "/" + day + "/" + year;
               
                 $('#<%= txtToDate.ClientID%>').val(dateString2);    
                  $('#<%= lblYear.ClientID%>').addClass("labelactive");
                document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnDepositSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearchDate.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
       
        function notyConfirm() {
            var reconval = document.getElementById("hdnReconciled").value;
            if (reconval == "true")
            {
                noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'Would you like to delete all associated receive payments for the selected deposit?',
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
                                document.getElementById("Confirm_Value").value = "true";
                                
                                  $("#<%=lnkDeleteDeposit.ClientID%>").click(); 
                                $noty.close();
                              
                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {
                                document.getElementById("Confirm_Value").value = "false";
                                   $("#<%=lnkDeleteDeposit.ClientID%>").click(); 
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
        }
        else 
        {
        noty({
                text: 'This deposit has cleared and cannot be deleted.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
          
            }
            function closedMesg1(val) {
                document.getElementById("hdnReconciled").value = val;                
            
        }
    </script>
</asp:Content>

