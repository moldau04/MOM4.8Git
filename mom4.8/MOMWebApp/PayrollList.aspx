<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" CodeBehind="PayrollList.aspx.cs" Inherits="MOMWebApp.PayrollList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Design/css/grid.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Payroll Preview</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClientClick='return AddPayroll();'>Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditPayrollClick();'>Edit</asp:LinkButton>
                                            </div>
                                            <%-- <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions</a>
                                            </div>--%>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick='return DeletePayrollClick(this)'>Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </asp:Panel>

                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server"><i class="mdi-content-clear" onclick="javascript:window.location.href='/Home.aspx';"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
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
                <div class="col lblsz2 lblszfloat">
                    <div class="row">
                        <%--  <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" CssClass="css-checkbox" Text="Incl. Draft" onclick="javascript:return pageLoad();"></asp:CheckBox>
                        </span>--%>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClientClick="javascript:return pageLoad();">Clear</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="grid_container">
            <div class="form-section-row" style="margin-bottom: 0 !important;">
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Payrolls" runat="server">
                </telerik:RadAjaxLoadingPanel>
                <div class="RadGrid RadGrid_Material">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Payroll" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Payroll" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <telerik:RadGrid RenderMode="Auto" runat="server" ID="RadGrid_Payroll" AllowFilteringByColumn="true" ShowFooter="True" FilterType="CheckList" AllowSorting="true" Width="100%"
                            PagerStyle-AlwaysVisible="true" ShowStatusBar="true" AllowPaging="True" AllowCustomPaging="True" PageSizes="1000" PagerStyle-PageSizes="10,20,25,50,100,500" PagerStyle-EnableAllOptionInPagerComboBox="true" AllowCustomSorting="true">
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowSorting="true" AllowPaging="true" AllowCustomSorting="true">
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40"></telerik:GridClientSelectColumn>
                                    <telerik:GridBoundColumn UniqueName="PayrollRegisterId" FilterDelay="5" DataField="PayrollRegisterId" HeaderText="PayrollRegisterId" SortExpression="PayrollRegisterId" Visible="false" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridHyperLinkColumn DataNavigateUrlFields="PayrollRegisterId" DataNavigateUrlFormatString="RunPayroll.aspx?id={0}" DataTextField="Description" HeaderText="Description" UniqueName="Description" SortExpression="Description" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                    </telerik:GridHyperLinkColumn>
                                    <telerik:GridBoundColumn UniqueName="CheckDate" DataField="" HeaderText="Check Date" AutoPostBackOnFilter="true" SortExpression="CheckDate" ShowFilterIcon="false" DataType="System.DateTime">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="PayFrequency" DataField="PayFrequency" HeaderText="Pay Frequency" AutoPostBackOnFilter="true" SortExpression="PayFrequency" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <%-- <telerik:GridBoundColumn UniqueName="StartDate" DataField="StartDate" HeaderText="Start Date" AutoPostBackOnFilter="true" SortExpression="StartDate" ShowFilterIcon="false" DataType="System.DateTime">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="EndDate" DataField="EndDate" HeaderText="End Date" AutoPostBackOnFilter="true" SortExpression="EndDate" ShowFilterIcon="false" DataType="System.DateTime">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridBoundColumn UniqueName="Status" DataField="Status" HeaderText="Status" AutoPostBackOnFilter="true" SortExpression="Status" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="GrossPay" DataField="GrossPay" HeaderText="Gross Pay" AutoPostBackOnFilter="true" SortExpression="GrossPay" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="NetPay" DataField="Net Pay" HeaderText="NetPay" AutoPostBackOnFilter="true" SortExpression="NetPay" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <%--  <telerik:GridBoundColumn UniqueName="CreatedBy" DataField="CreatedBy" HeaderText="Created By" AutoPostBackOnFilter="true" SortExpression="CreatedBy" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="CreatedOn" DataField="CreatedOn" HeaderText="Created Date" AutoPostBackOnFilter="true" SortExpression="CreatedOn" ShowFilterIcon="false" DataType="System.DateTime">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridBoundColumn UniqueName="ModifiedBy" DataField="ModifiedBy" HeaderText="Modified By" AutoPostBackOnFilter="true" SortExpression="ModifiedBy" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="ModifiedOn" DataField="ModifiedOn" HeaderText="Modified Date" AutoPostBackOnFilter="true" SortExpression="ModifiedOn" ShowFilterIcon="false" DataFormatString="{0:d}" DataType="System.DateTime">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="EmployeeCount" DataField="" HeaderText="Employee Count" AutoPostBackOnFilter="true" SortExpression="EmployeeCount" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <ClientEvents OnCommand="RadGrid_Payroll_OnCommand"></ClientEvents>
                            </ClientSettings>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddPayroll" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditPayroll" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeletePayroll" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewPayroll" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="server">
    <script src="js/commonAPI.js"></script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
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

            //var cbIncludeInActive = $('#');
            var gridData;
            var Filters = new Array();
            var currentPageIndex = 0;
            var PageSize = 25;
            var SortField = null;
            var SortOrder = 1;
            var totalRecords = 0;
            var totalPages = 0;
            var Active = null;

            function pageLoad() {
                currentPageIndex = 0;
                var grid = $find("<%= RadGrid_Payroll.ClientID %>");
                var gridView = grid.get_masterTableView();
                gridView.clearFilter();
                if (gridView != null) {
                    var columns = gridView.get_columns();
                    for (var i = 0; i < columns.length; i++) {
                        columns[i].resizeToFit(false, true);
                    }
                    //BindRedGrid(gridView);
                }
            }

            function RadGrid_Payroll_OnCommand(sender, args) {
                var commandName = args.get_commandName();
                var shouldAllowOperation = commandName == "RebindGrid";
                args.set_cancel(!shouldAllowOperation);
                var gridView = sender.get_masterTableView();
                currentPageIndex = gridView.get_currentPageIndex();
                if (args.get_commandName() === "PageSize" || args.get_commandName() === "Page") {
                    PageSize = gridView.get_pageSize();
                }
                var filterExpressions = gridView.get_filterExpressions();
                filterExpressions._array.forEach(function (data) {
                    if (data._fieldValue.toString() != '') {
                        var col = data._fieldName.toString();
                        var filterValue;
                        if (data._fieldValue.toString().toLowerCase() === 'no' || data._fieldValue.toString().toLowerCase() === 'active') {
                            filterValue = 0;
                        }
                        else if (data._fieldValue.toString().toLowerCase() === 'yes' || data._fieldValue.toString().toLowerCase() === 'inactive') {
                            filterValue = 1;
                        }
                        else {
                            filterValue = data._fieldValue.toString();
                        }
                        var filter = {
                            key: col,
                            value: filterValue
                        }
                        Filters.push(filter);
                    }
                });

                if (args.get_commandName() == "Sort") {
                    gridView.set_currentPageIndex(0);
                }
                var sortExpressions = gridView.get_sortExpressions();
                if (sortExpressions._array.length > 0) {
                    SortField = sortExpressions.getItem(0).get_fieldName();
                    SortOrder = sortExpressions.getItem(0).get_sortOrder();
                }
                BindRedGrid(gridView);
                while (Filters.length) {
                    Filters.pop();
                }
                SortField = null;
                SortOrder = 1;
                Active = null;
                //internalDeselect = false;
            }
            function BindRedGrid(gridView) {
                //if ($(cbIncludeInActive).is(":checked")) {
                //    Active = null;
                //}
                //else if (!$(cbIncludeInActive).is(":checked")) {
                //    Active = 0;
                //}
                var Model = {
                    PageNumber: currentPageIndex + 1,
                    PageSize: PageSize,
                    SortField: SortField,
                    SortOrder: SortOrder,
                    Active: Active,
                    Filters: Filters
                };
                AccessControler.Post(Model, 30000, "./api/PayrollApi/GetPayrollList", function (response) {
                    if (response != null) {
                        gridData = JSON.parse(response);
                        gridView.set_dataSource(gridData);
                        gridView.dataBind();
                        totalRecords = gridData[0].totalRecords;
                        gridView.set_virtualItemCount(totalRecords);
                        totalPages = Math.ceil(totalRecords / PageSize);
                        gridView.set_pageSize(PageSize);
                        $('#<%=lblRecordCount.ClientID%>').text(totalRecords + " Record(s) found");
                    }
                });
            }
        </script>
    </telerik:RadCodeBlock>


    <script type="text/javascript">
        function AddPayroll() {
            var id = document.getElementById('<%= hdnAddPayroll.ClientID%>').value;
            if (id == "Y") {
                '<%Session["PayrollRegisterId"] = null; %>';
                window.location.href = "RunPayroll.aspx";
                return false;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        var PayrollId = '';
        function EditPayrollClick() {
            var id = document.getElementById('<%= hdnEditPayroll.ClientID%>').value;
            if (id == "Y") {
                var grid = $find("<%= RadGrid_Payroll.ClientID %>");
                var gridView = grid.get_masterTableView();
                PayrollId = gridView.get_selectedItems()[0]._dataItem['PayrollRegisterId'];
                window.location.href = "RunPayroll.aspx?id=" + PayrollId;
                return false;
            }
            else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
       <%-- function CopyPayrollClick() {
            var id = document.getElementById('<%= hdnEditPayroll.ClientID%>').value;
            if (id == "Y") {
                var grid = $find("<%= RadGrid_Payroll.ClientID %>");
                var gridView = grid.get_masterTableView();
                PayrollId = gridView.get_selectedItems()[0]._dataItem['ID'];
                window.location.href = "RunPayroll.aspx?id=" + PayrollId + "&t=c";
                return false;
            }
            else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }--%>
        function DeletePayrollClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeletePayroll.ClientID%>').value;
            if (id == "Y") {
                var grid = $find("<%= RadGrid_Payroll.ClientID %>");
                var gridView = grid.get_masterTableView();
                PayrollId = gridView.get_selectedItems()[0]._dataItem['PayrollRegisterId'];
                debugger;
                BindRedGrid(gridView);
            }
            else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
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
