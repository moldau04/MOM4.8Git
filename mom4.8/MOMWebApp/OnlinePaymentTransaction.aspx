<%@ Page Title="Online Payment Transaction || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="OnlinePaymentTransaction" Codebehind="OnlinePaymentTransaction.aspx.cs" %>

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
                                    <div class="page-title"><i class="mdi-action-payment"></i>&nbsp;Online Payment</div>
                                    <div class="buttonContainer">
                                        <asp:Panel runat="server" ID="pnlGridButtons">
<%--                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>--%>
<%--                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkApprove" runat="server" OnClick="lnkApprove_Click">Approve</asp:LinkButton>
                                            </div>--%>
                                             <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddMulti" runat="server" OnClick="lnkAddMulti_Click" Visible="false"> Batch Receipt</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" Visible="false">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkApprove" runat="server" OnClick="lnkApprove_Click">Approve</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
<%--                                                <li>
                                                    <div class="btnlinks">                                                       
                                                        <a class="dropdown-button" data-beloworigin="true" data-activates="dropdown1">Reports</a>
                                                    </div>

                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                            <asp:LinkButton ID="lnkOnlinePaymentTransaction" runat="server" OnClick="lnkOnlinePaymentTransaction_Click">Online Payment Report</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </li>--%>
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
                    <div class="srchtitle" >
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="srchcstm datepicker_mom"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap srchclr btnlinksicon" >
                        <asp:LinkButton ID="lnkSearch" runat="server" OnClientClick="return ResetSearch();" OnClick="lnkSearch_Click" CausesValidation="false" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                        <div style="display:none">
                            <asp:LinkButton ID="lnkSearchSelectDate" runat="server"  OnClick="lnkSearchSelectDate_Click" CausesValidation="false" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                        </div>
                    </div>
                    <div class="srchinputwrap">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnRecvDate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnRecvDate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnRecvDate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnRecvDate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnRecvDate', 'rdCal')" />
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
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" OnClientClick="ResetValue();">Clear </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClientClick="ResetShowAll()" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <%--<asp:UpdatePanel ID="updpnl" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                 <%--   </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="grid_container">
                <div class="form-section-row" >

                    <telerik:RadAjaxManager ID="RadAjaxManager_OnlinePaymentTransaction" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OnlinePaymentTransaction" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkApprove">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OnlinePaymentTransaction" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OnlinePaymentTransaction" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" />
                                    <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice" />
                                      <telerik:AjaxUpdatedControl ControlID="txtFromDate"/>
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearchSelectDate">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OnlinePaymentTransaction" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" />
                                     <telerik:AjaxUpdatedControl ControlID="txtFromDate"  />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                                      <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice"  />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OnlinePaymentTransaction" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" />
                                      <telerik:AjaxUpdatedControl ControlID="txtFromDate"/>
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                                      <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice"/>
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OnlinePaymentTransaction" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate"  />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate"  />
                                      <telerik:AjaxUpdatedControl ControlID="ishowAllInvoice"  />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="RadGrid_OnlinePaymentTransaction">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_OnlinePaymentTransaction" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" />
 <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                   
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_OnlinePaymentTransaction" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_OnlinePaymentTransaction.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_OnlinePaymentTransaction" runat="server" LoadingPanelID="RadAjaxLoadingPanel_OnlinePaymentTransaction" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_OnlinePaymentTransaction" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowMultiRowSelection="True"  AllowCustomPaging="True"                               
                                OnNeedDataSource="RadGrid_OnlinePaymentTransaction_NeedDataSource" 
                                OnPreRender="RadGrid_OnlinePaymentTransaction_PreRender" 
                                OnItemCreated="RadGrid_OnlinePaymentTransaction_ItemCreated" 
                                OnExcelMLExportRowCreated="RadGrid_OnlinePaymentTransaction_ExcelMLExportRowCreated"
                                OnPageIndexChanged="RadGrid_OnlinePaymentTransaction_PageIndexChanged"
                                OnPageSizeChanged="RadGrid_OnlinePaymentTransaction_PageSizeChanged"
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
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false">
                                            <HeaderTemplate>
                                                <input id="chkAll" type="checkbox" class="css-checkbox" />
                                                <label for="chkAll" class="css-label">&nbsp;</label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSelected" runat="server" />
                                                <%--<asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>--%>
                                                <asp:HiddenField ID="hdnStatus" Value='<%# Eval("Status") %>' runat="server" />
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("OnlinePaymentTransactionId") %>' ClientIDMode="Static" />
                                                <asp:HiddenField ID="hidInvoiceId" runat="server" Value='<%# Eval("InvoiceId") %>' ClientIDMode="Static" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="5%">

                                        </telerik:GridClientSelectColumn>
                                      <telerik:GridTemplateColumn DataField="OnlinePaymentTransactionId" UniqueName="ID" HeaderText="ID" SortExpression="OnlinePaymentTransactionId" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" AllowFiltering="true"
                                            ShowFilterIcon="false" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("OnlinePaymentTransactionId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server">Total :-</asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn SortExpression="TransactionDate" HeaderStyle-Width="10%" UniqueName="TransactionDate" AutoPostBackOnFilter="true" DataField="TransactionDate" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Online Payment Date" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionDate" runat="server" Text='<%# Eval("TransactionDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
<%--                                        <telerik:GridTemplateColumn DataField="GatewayTransactionId" UniqueName="GatewayTransactionId" HeaderText="GatewayTransactionId" SortExpression="GatewayTransactionId"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="10%"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGatewayTransactionId" runat="server" Text='<%# Eval("GatewayTransactionId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                        <telerik:GridBoundColumn DataField="GatewayTransactionId" HeaderText="GatewayTransactionId" HeaderStyle-Width="10%" SortExpression="GatewayTransactionId" UniqueName="GatewayTransactionId"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="CustomerName" UniqueName="CustomerName" HeaderStyle-Width="20%" SortExpression="CustomerName" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Customer Name" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkCustomerName" runat="server" Text='<%# Bind("CustomerName") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Location" HeaderText="Location" HeaderStyle-Width="20%" SortExpression="Location" UniqueName="Location"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="PaymentMode" HeaderText="Payment Method" SortExpression="PaymentMode" UniqueName="PaymentMode"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="10%"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="Amount" UniqueName="Amount" HeaderStyle-Width="10%" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="TransactionStatus" HeaderText="Status" HeaderStyle-Width="10%" SortExpression="TransactionStatus" UniqueName="TransactionStatus"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
<%--                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company" UniqueName="Company" HeaderStyle-Width="8"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn DataField="Amount" UniqueName="Amount" HeaderStyle-Width="7%" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="DepID" UniqueName="DepID" SortExpression="DepID" HeaderStyle-Width="5%" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Deposit #" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                    <asp:HyperLink ID="lnkDepID" Visible='<%# Eval("DepID").ToString()=="0"?false:true %>' runat="server" Text='<%# Eval("DepID") %>' Target="_blank" NavigateUrl='<%# "adddeposit.aspx?id=" +Eval("DepID")  %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="BatchReceipt" UniqueName="BatchReceipt" HeaderStyle-Width="8%" SortExpression="BatchReceipt" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Batch Receipt" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                    <asp:HyperLink ID="lnkBatchID" Visible='<%# Eval("BatchReceipt").ToString()=="0"?false:true %>' runat="server" Text='<%# Eval("BatchReceipt") %>' Target="_blank" NavigateUrl='<%# "EditMultiOnlinePaymentTransaction?uid=" +Eval("BatchReceipt")  %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnRcvPymtSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
     <asp:HiddenField runat="server" ID="ishowAllInvoice" Value="0" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="js/jquery-ui-1.9.2.custom.js"></script>
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
        });


        function displyDeleteAlert() {
            alert("These month/year period is closed out. You do not have permission to delete this record.");
        }

        function closedMesg() {
            noty({
                //text: 'This Online payment has applied and cannot be deleted.',
                text:'This Online payment has been successful and cannot be deleted.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }

        function ApproveFailedMsg() {
            noty({
                //text: 'This Online payment has been failed and cannot be approved.',
                text: 'This Online payment has been failed and cannot be approved.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }

        function ApproveSuccessMsg() {
            noty({
                text: 'This Online payment has been successfully approved.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }

        function DatePermissionAlert(mesg) {
            // alert("These month/year period is closed out. You do not have permission to " + mesg + " this record.");
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
                text: 'Payment deleted successfully!',
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

        function ResetShowAll() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
        function ResetSearch() {
            <%--debugger;
           
            var SesVar = $('#<%= hdnCssActive.ClientID%>').val();
            if ($("#<%=txtFromDate.ClientID%>").val === '' || $("#<%=txtToDate.ClientID%>").val === '') {

                  return false;
                 noty({
                    text: 'Please enter valid start date and end date.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });
              
            }--%>

        }
        function ResetValue() {
             debugger
            if (document.getElementById('<%= ishowAllInvoice.ClientID%>').value == 1) {
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Week";
                $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                document.getElementById("rdWeek").checked = true;
            } 
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
                var SesVar = '<%= Convert.ToString(Session["lblActive"])%>';
                var val;
                val = localStorage.getItem("hdnRecvDate");
                if (document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value == 'Week') { val = "Week"; }
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
            //debugger;
            $("#<%=lblDay.ClientID%>").removeClass("labelactive"); 
            $("#<%=lblWeek.ClientID%>").removeClass("labelactive"); 
            $("#<%=lblMonth.ClientID%>").removeClass("labelactive"); 
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive"); 
            $("#<%=lblYear.ClientID%>").removeClass("labelactive"); 
              
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
                $("#<%=lblDay.ClientID%>").addClass("labelactive"); 
          
              
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
                  $('#<%= lblWeek.ClientID%>').addClass("labelactive"); 
          
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
                   $('#<%= lblMonth.ClientID%>').addClass("labelactive"); 
            

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
                $('#<%= lblQuarter.ClientID%>').addClass("labelactive"); 
           
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
                 $('#<%= lblYear.ClientID%>').addClass("labelactive"); 
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
            //document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
             document.getElementById('<%= hdnCssActive.ClientID%>').value = "2";
           <%-- var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();--%>
           // document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
            return false;

        }
        function SelectDate(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
            ResetShowAll();
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
                //$(label).addClass("labelactive");
                  $('#<%= lblDay.ClientID%>').addClass("labelactive"); 
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Day";
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
                //$(label).addClass("labelactive");
                  $('#<%= lblWeek.ClientID%>').addClass("labelactive"); 
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Week";
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
                //$(label).addClass("labelactive");
                 $('#<%= lblMonth.ClientID%>').addClass("labelactive"); 
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Month";
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
               // $(label).addClass("labelactive");
                 $('#<%= lblQuarter.ClientID%>').addClass("labelactive"); 
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Quarter";
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
               // $(label).addClass("labelactive");
                document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value = "Year";
                $('#<%= lblYear.ClientID%>').addClass("labelactive"); 
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnRcvPymtSelectDtRange.ClientID%>').value);
            }
           // document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
             document.getElementById('<%= hdnCssActive.ClientID%>').value = "2";
            var clickSearchButton = document.getElementById("<%= lnkSearchSelectDate.ClientID %>");
            clickSearchButton.click();
            //document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
</asp:Content>

