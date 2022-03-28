<%@ Page Title="Chart of Accounts || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ChartOfAccount" EnableEventValidation="false" Codebehind="ChartOfAccount.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        .RadGrid_Material .rgPagerCell .RadComboBox{
            min-width: 55px !important;
        }
    </style>
     <script type="text/javascript">
         function CheckDelete() {
             var result = false;
             $("#<%=RadGrid_COA.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this Account ?');
            }
            else {
                alert('Please select an account to delete.')
                return false;
            }
        }
        function CheckSelect() {
            var result = false;
            $("#<%=RadGrid_COA.ClientID%> tr").each(function () {
                 var checkBox = $(this).find("input[type='checkbox']");
                 if (checkBox.is(":checked")) {
                     result = true;
                 }
             });

             if (result == true) {
                 return confirm('Do you really want to copy this record?');
             }
             else {
                 alert('Please select an account to copy.')
                 return false;
             }
         }
         function notifyDelete() {
             noty({
                 text: 'You can not delete this record!',
                 type: 'warning',
                 layout: 'topCenter',
                 closeOnSelfClick: false,
                 timeout: 5000,
                 theme: 'noty_theme_default',
                 closable: true
             });
         }
         function defaultNotifyDelete() {
             noty({
                 text: 'You can not delete default account!',
                 type: 'warning',
                 layout: 'topCenter',
                 closeOnSelfClick: false,
                 timeout: 5000,
                 theme: 'noty_theme_default',
                 closable: true
             });
         }
         function cannotDeleteAccount() {
             noty({
                 text: 'You can not delete default account! This account is used for the system',
                 type: 'warning',
                 layout: 'topCenter',
                 closeOnSelfClick: false,
                 timeout: 5000,
                 theme: 'noty_theme_default',
                 closable: true
             });
         }
     </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title">
                                            <i class="mdi-social-people"></i>&nbsp;
                                              <asp:Label CssClass="title_text" ID="Label1" runat="server">Chart of Accounts</asp:Label>
                                        </div>
                                        <div class="buttonContainer">
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
                                                        <asp:LinkButton ID="lnkCopy" runat="server" OnClick="lnkCopy_Click" OnClientClick="return CheckSelect();">Copy</asp:LinkButton>
                                                    </div>
                                                </li>
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
                                                            <a href="GeneralLedger.aspx" class="-text">General Ledger Report</a>
                                                        </li>
                                                        <li>
                                                           <asp:LinkButton ID="lnkDashboardReport" runat="server" PostBackUrl="DashboardReport.aspx" Visible="false">DashboardReport</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click">
                                                <i class="mdi-content-clear"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="srchtitle ser-css2" >
                            Search
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst selectsml" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Acct No.</asp:ListItem>
                                <asp:ListItem Value="2">Account Name</asp:ListItem>
                                <asp:ListItem Value="3">Balance</asp:ListItem>
                                <asp:ListItem Value="4">Center</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlBalanceCondition" runat="server" CssClass="browser-default selectst selectsml">
                                <asp:ListItem Value="0"> -- Condition -- </asp:ListItem>
                                <asp:ListItem Value="1">&lt;&gt;</asp:ListItem>
                                <asp:ListItem Value="2">&lt;</asp:ListItem>
                                <asp:ListItem Value="3">&gt;</asp:ListItem>
                                <asp:ListItem Value="4">&lt;=</asp:ListItem>
                                <asp:ListItem Value="5">&gt;=</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default selectst selectsml" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlSubAcCategory" runat="server" CssClass="browser-default selectst selectsml">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">

                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst selectsml">
                            </asp:DropDownList>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlType" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="srchinputwrap srchclr btnlinksicon">
                    <asp:LinkButton ID="lnkSearch" ToolTip="Search" runat="server" CausesValidation="false"
                        OnClick="lnkSearch_Click"><i class="mdi-action-search"></i></asp:LinkButton>
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
                            <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </span>
                    </div>
                </div>

            </div>
            <div class="grid_container">
                <div class="form-section-row pmd-card">
                    <telerik:RadAjaxManager ID="RadAjaxManager_COA" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_COA" LoadingPanelID="RadAjaxLoadingPanel_COA" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_COA" LoadingPanelID="RadAjaxLoadingPanel_COA" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_COA" LoadingPanelID="RadAjaxLoadingPanel_COA" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_COA" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="RadCodeBlock_COA" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_COA.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_COA" runat="server" LoadingPanelID="RadAjaxLoadingPanel_COA" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_COA" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_COA_PreRender"
                                AllowCustomPaging="True" OnItemDataBound="RadGrid_COA_RowDataBound" OnNeedDataSource="RadGrid_COA_NeedDataSource" OnItemEvent="RadGrid_COA_ItemEvent" OnItemCreated="RadGrid_COA_ItemCreated" OnExcelMLExportRowCreated="RadGrid_COA_ExcelMLExportRowCreated">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>

                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnTypeId" Value='<%#Eval("Type") %>' runat="server" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Acct" SortExpression="Acct" AutoPostBackOnFilter="true" UniqueName="Acct"
                                            CurrentFilterFunction="Contains" HeaderText="Acct" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcct" runat="server" Text='<%#Eval("Acct") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div class="total-page">
                                                    <asp:Label Text="Page Total" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Grand Total" runat="server" />
                                                </div>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="fDesc" SortExpression="fDesc" HeaderText="Description" UniqueName="fDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Department" SortExpression="Department" HeaderText="Center" UniqueName="Department"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="AcctType" SortExpression="AcctType" AutoPostBackOnFilter="true" UniqueName="AcctType"
                                            CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("AcctType") %>' CommandArgument='<%#Eval("ID")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Sub" SortExpression="Sub" AutoPostBackOnFilter="true" UniqueName="Sub"
                                            CurrentFilterFunction="Contains" HeaderText="Sub" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSub" runat="server" Text='<%# Bind("Sub") %>' />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Debit" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Debit" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Debit" ShowFilterIcon="false" UniqueName="Debit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebit" runat="server" CommandArgument='<%#Eval("ID")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Debit", "{0:c}") %>' />
                                                <asp:Label ID="lblBDebit" Text='<%# Eval("Debit")%>' runat="server" Style="display: none;" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div class="total-page" >
                                                    <asp:Label ID="lblTotalDebit" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblGrandTotalDebit" runat="server" />
                                                </div>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Credit" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Credit" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Credit" ShowFilterIcon="false" UniqueName="Credit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCredit" runat="server" CommandArgument='<%#Eval("ID")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Credit", "{0:c}") %>' />
                                                <asp:Label ID="lblBCredit" Text='<%# Eval("Credit")%>' runat="server" Style="display: none;" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div class="total-page">
                                                    <asp:Label ID="lblTotalCredit" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblGrandTotalCredit" runat="server" />
                                                </div>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="AcctStatus" SortExpression="Status" AutoPostBackOnFilter="true" UniqueName="AcctStatus"
                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" CommandArgument='<%#Eval("ID")%>' Text='<%#Eval("AcctStatus")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Company" SortExpression="Company" AutoPostBackOnFilter="true" UniqueName="Company"
                                            CurrentFilterFunction="Contains" HeaderText="Company" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompany" runat="server"><%#Eval("Company")%></asp:Label>
                                            </ItemTemplate>
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

    <script>
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
    </script>
      
</asp:Content>
