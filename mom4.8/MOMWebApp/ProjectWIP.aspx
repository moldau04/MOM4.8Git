<%@ Page Title="Projects WIP || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ProjectWIP" CodeBehind="ProjectWIP.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.7.9/angular.min.js"></script>
    <style>
        div.RadFilterMenu_CheckList {
            height: 350px !important;
        }

            div.RadFilterMenu_CheckList .RadListBox {
                height: 300px !important;
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

        #ctl00_ContentPlaceHolder1_vceEndDt_popupTable,
        #ctl00_ContentPlaceHolder1_vceEndDt1_popupTable {
            width: 250px !important;
        }

        .edit-column {
            font-size: 1rem !important;
        }
    </style>
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <script>
        function PostWIPClick() {
            if (!confirm("Are you sure you want to post for this period?")) {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script>
        function ShowRestoreGridSettingsButton() {
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }

        function OnGridCreated(sender, args) {
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
            document.getElementById("overlay").style.display = "block";
            document.getElementById('<%=lnkSaveGridSettings.ClientID%>').click();
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "block";
        }

        function RestoreGridSettings() {
            document.getElementById("overlay").style.display = "block";
            document.getElementById('<%=lnkRestoreGridSettings.ClientID%>').click();
            document.getElementById("<%=lbRestoreGridSettings.ClientID %>").style.display = "none";
            document.getElementById("<%=lbSaveGridSettings.ClientID %>").style.display = "none";
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
        <img src="images/wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 25%; margin-left: 50%;" />
    </div>

    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-contacts"></i>&nbsp;
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Project WIP</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click" CausesValidation="False">Save</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkPost" runat="server" OnClick="lnkPost_Click" OnClientClick='return PostWIPClick()' CausesValidation="False">Post</asp:LinkButton>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkExportExcel" runat="server" OnClick="lnkExportExcel_Click" CausesValidation="False">Export to Excel</asp:LinkButton>
                                        </div>
                                         <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dropdown1">Reports
                                            </a>
                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <asp:LinkButton ID="lnkPostReport" runat="server" OnClick="lnkPostReport_Click">Post Report</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
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
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblLastPost" runat="server" Visible="false"></asp:Label>
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
    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="srchpane">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="srchtitle" style="padding-left: 15px;">
                                    As of Date
                                </div>
                                <div class="srchinputwrap">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" Height="30px"
                                        MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtToDate">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfvEndDt"
                                        runat="server" ControlToValidate="txtToDate" Display="None" ErrorMessage="End date is Required" ValidationGroup="search"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="rfvEndDt" />
                                    <asp:RegularExpressionValidator ID="rfvEndDt1" ControlToValidate="txtToDate" ValidationGroup="search"
                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                    </asp:RegularExpressionValidator>
                                    <asp:ValidatorCalloutExtender ID="vceEndDt1" runat="server" Enabled="True" PopupPosition="Right"
                                        TargetControlID="rfvEndDt1" />
                                </div>

                                <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -10px; margin-top: -2px;">
                                    <asp:LinkButton ID="lnkSearch" OnClick="lnkSearch_Click" runat="server"><i class="mdi-action-search"></i></asp:LinkButton>
                                </div>
                                <div class="srchinputwrap rdleftmgn">
                                    <div class="rdpairing">
                                        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="col lblsz2 lblszfloat">
                                    <div class="row">
                                        <span class="tro trost accrd-trost">
                                            <asp:CheckBox ID="lnkChk" CssClass="filled-in" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
                                            <asp:Label ID="lblChkSelect" AssociatedControlID="lnkChk" CssClass="title-check-text" runat="server">Incl. Closed</asp:Label>
                                        </span>
                                        <span class="tro trost accrd-trost">
                                            <span>
                                                <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                            </span>
                                        </span>
                                    </div>
                                </div>
                                <div class="cf"></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container accordian-wrap">
        <div class="row">
            <asp:HiddenField ID="hdnTabId" runat="server" />
            <ul class="tabs tab-demo-active white tabProject" id="tabProject" style="width: 100%;">
                <asp:Repeater ID="rptDepartmentTab" runat="server">
                    <ItemTemplate>
                        <li class="tab col s2" style="font-size: 13px !important">
                            <a class="waves-effect waves-light prodept" title='<%# Eval("type") %>' id="<%# Eval("ID") %>" href="#activeone" onclick='selectTab("<%# Eval("ID") %>")'>&nbsp;<%# Eval("type") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <br />
            <div id="activeone" class="tab-container-border lighten-4" style="display: block;">
                <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                        <div class="RadGrid RadGrid_Material">
                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdDept" Value="-1" />
                            <telerik:RadCodeBlock ID="RadCodeBlock_Project" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= RadGrid_Project.ClientID %>");
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
                                            if (selectionStart != null)
                                                element.selectionStart = selectionStart;
                                        }
                                    }
                                    function RowDblClick(sender, eventArgs) {
                                        if ($("#<%=hdnEditeJob.ClientID %>").val() == "Y" || $("#<%=hdnViewJob.ClientID %>").val() == "Y") {
                                            var selectColumnID = eventArgs.get_gridDataItem().get_element().cells[0].firstChild.id;
                                            var jobColumnId = selectColumnID.replace('ClientSelectColumnSelectCheckBox', 'lnkJob')
                                            var jobId = document.getElementById(jobColumnId).innerText;
                                            window.open('addProject.aspx?uid=' + jobId, '_self');
                                        }
                                        else {
                                            noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                                        }
                                    }

                                    function MenuShowing(menu, args) {
                                        // Iterate through filter menu items.
                                        var items = menu.get_items();
                                        for (i = 0; i < items.get_count(); i++) {
                                            var item = items.getItem(i);
                                            if (item === null)
                                                continue;
                                        }

                                        menu.repaint();
                                    }
                                </script>
                            </telerik:RadCodeBlock>
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Project" CssClass="RadGrid_Project" AllowFilteringByColumn="true" ShowFooter="false" PageSize="50" HeaderStyle-CssClass="cent"
                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" AllowCustomPaging="false" FilterType="CheckList" EnableLinqExpressions="false" ShowGroupPanel="false"
                                OnItemCreated="RadGrid_Project_ItemCreated"
                                OnExcelMLExportRowCreated="RadGrid_Project_ExcelMLExportRowCreated"
                                OnNeedDataSource="RadGrid_Project_NeedDataSource"
                                OnItemCommand="RadGrid_Project_ItemCommand"
                                OnPreRender="RadGrid_Project_PreRender">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnRowDblClick="RowDblClick" OnGridCreated="OnGridCreated" OnHeaderMenuShowing="headerMenuShowing"
                                        OnColumnHidden="ColumnSettingsChange" OnColumnShown="ColumnSettingsChange"
                                        OnColumnResized="ColumnSettingsChange" OnColumnSwapped="ColumnSettingsChange" />
                                </ClientSettings>
                                <FilterMenu OnClientShowing="MenuShowing" />
                                <MasterTableView AllowPaging="false" AllowCustomPaging="false" AutoGenerateColumns="false" EnableHierarchyExpandAll="false" EditMode="Batch" EnableHeaderContextMenu="true"
                                    AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="ID">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="10">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn HeaderText="Project#" DataField="ID" UniqueName="ID" SortExpression="ID" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ShowFilterIcon="false" HeaderStyle-Width="80" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkJob" runat="server" NavigateUrl='<%# "addProject.aspx?uid=" + Eval("ID") %>'><%#Eval("ID")%></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Department" DataField="Type" UniqueName="Type" SortExpression="Type" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="120" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hndDepartment" Value='<%# DataBinder.Eval(Container.DataItem, "Department") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Description" DataField="fDesc" UniqueName="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="200" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "fDesc") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Location Name" DataField="Tag" UniqueName="Tag" SortExpression="Tag" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="200" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTag" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tag") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Status" DataField="Status" UniqueName="Status" SortExpression="Tag" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnDate" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "fDate")%>' />
                                                <asp:HiddenField ID="hdnCloseDate" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "CloseDate")%>' />
                                                <telerik:RadToolTip RenderMode="Auto" ID="RadToolTip1" runat="server" TargetControlID="lblStatus" RelativeTo="Element"
                                                    Position="BottomCenter" RenderInPageRoot="true" AutoCloseDelay="0">
                                                    Created Date: <%# DataBinder.Eval(Container.DataItem, "fDate", "{0:MM/dd/yyyy}")%><br />
                                                    Closed Date: <%# DataBinder.Eval(Container.DataItem, "CloseDate", "{0:MM/dd/yyyy}")%>
                                                </telerik:RadToolTip>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Contract Price and Charge Orders" DataField="BRev" UniqueName="BRev" SortExpression="BRev" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150"
                                            FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Panel runat="server" ID="panelBRev" Visible="<%# !_isPost && _editAllow %>" CssClass="edit-column">
                                                    <asp:TextBox ID="txtBRev" runat="server" Width="100%" autocomplete="off" ForeColor='<%# Convert.ToDouble(Eval("BRev")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "BRev", "{0:N}") %>' CssClass="textbox"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtBRev_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" TargetControlID="txtBRev" ValidChars="1234567890,.-">
                                                    </asp:FilteredTextBoxExtender>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="panelPostBRev" Visible="<%# _isPost || !_editAllow %>">
                                                    <asp:Label ID="lblBRev" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BRev")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "BRev", "{0:N}") %>'></asp:Label>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Estimated Cost Per MOM" DataField="TotalBudgetedExpense" UniqueName="TotalBudgetedExpense" SortExpression="TotalBudgetedExpense" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150"
                                            FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalBudgetedExpense" runat="server" ForeColor='<%# Convert.ToDouble(Eval("TotalBudgetedExpense")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "TotalBudgetedExpense", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Cumulative Adjustments" DataField="ConstModAdjmts" UniqueName="ConstModAdjmts" SortExpression="ConstModAdjmts" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" Visible="True" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" HeaderStyle-Width="150" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Panel runat="server" ID="panelConstModAdjmts" Visible="<%# !_isPost && _editAllow %>" CssClass="edit-column">
                                                    <asp:TextBox ID="txtConstModAdjmts" runat="server" Width="100%" autocomplete="off" ForeColor='<%# Convert.ToDouble(Eval("ConstModAdjmts")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ConstModAdjmts", "{0:N}") %>' CssClass="textbox"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtConstModAdjmts_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" TargetControlID="txtConstModAdjmts" ValidChars="1234567890,.-">
                                                    </asp:FilteredTextBoxExtender>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="panelPostConstModAdjmts" Visible="<%# _isPost || !_editAllow %>">
                                                    <asp:Label ID="lblConstModAdjmts" runat="server" ForeColor='<%# Convert.ToDouble(Eval("ConstModAdjmts")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ConstModAdjmts", "{0:N}") %>'></asp:Label>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Current Adjustment" DataField="AccountingAdjmts" UniqueName="AccountingAdjmts" SortExpression="AccountingAdjmts" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" Visible="True" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" HeaderStyle-Width="150" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Panel runat="server" ID="panelAccountingAdjmts" Visible="<%# !_isPost && _editAllow %>" CssClass="edit-column">
                                                    <asp:TextBox ID="txtAccountingAdjmts" runat="server" Width="100%" autocomplete="off" ForeColor='<%# Convert.ToDouble(Eval("AccountingAdjmts")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "AccountingAdjmts", "{0:N}") %>' CssClass="textbox"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtAccountingAdjmts_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" TargetControlID="txtAccountingAdjmts" ValidChars="1234567890,.-">
                                                    </asp:FilteredTextBoxExtender>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="panelPostAccountingAdjmts" Visible="<%# _isPost || !_editAllow %>">
                                                    <asp:Label ID="lblAccountingAdjmts" runat="server" ForeColor='<%# Convert.ToDouble(Eval("AccountingAdjmts")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "AccountingAdjmts", "{0:N}") %>'></asp:Label>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Total Estimated Cost" DataField="TotalEstimatedCost" UniqueName="TotalEstimatedCost" SortExpression="TotalEstimatedCost" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalEstimatedCost" runat="server" ForeColor='<%# Convert.ToDouble(Eval("TotalEstimatedCost")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "TotalEstimatedCost", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Estimated Gross Profit" DataField="EstimatedProfit" UniqueName="EstimatedProfit" SortExpression="EstimatedProfit" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimatedProfit" runat="server" ForeColor='<%# Convert.ToDouble(Eval("EstimatedProfit")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "EstimatedProfit", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Contract Costs Total" DataField="NCost" UniqueName="NCost" SortExpression="NCost" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNCost" runat="server" ForeColor='<%# Convert.ToDouble(Eval("NCost")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "NCost", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Estimated Cost to Complete" DataField="CostToComplete" UniqueName="CostToComplete" SortExpression="CostToComplete" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCostToComplete" runat="server" ForeColor='<%# Convert.ToDouble(Eval("CostToComplete")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "CostToComplete", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Percentage Complete" DataField="PercentageComplete" UniqueName="PercentageComplete" SortExpression="PercentageComplete" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPercentageComplete" runat="server" ForeColor='<%# Convert.ToDouble(Eval("PercentageComplete")) <= 1 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "PercentageComplete", "{0:0.0000%}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Revenues Earned" DataField="RevenuesEarned" UniqueName="RevenuesEarned" SortExpression="RevenuesEarned" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRevenuesEarned" runat="server" ForeColor='<%# Convert.ToDouble(Eval("RevenuesEarned")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "RevenuesEarned", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Gross Profit" DataField="GrossProfit" UniqueName="GrossProfit" SortExpression="GrossProfit" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossProfit" runat="server" ForeColor='<%# Convert.ToDouble(Eval("GrossProfit")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "GrossProfit", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Billing to Date" DataField="NRev" UniqueName="NRev" SortExpression="NRev" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNRev" runat="server" ForeColor='<%# Convert.ToDouble(Eval("NRev")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "NRev", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Retainage Billing" DataField="RetainageBilling" UniqueName="RetainageBilling" SortExpression="RetainageBilling" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" Visible="True" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="150" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Panel runat="server" ID="panel" Visible="<%# !_isPost && _editAllow%>" CssClass="edit-column">
                                                    <asp:TextBox ID="txtRetainageBilling" runat="server" Width="100%" autocomplete="off" ForeColor='<%# Convert.ToBoolean(Eval("IsUpdateRetainage")) ? System.Drawing.Color.Blue : System.Drawing.Color.Black %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "RetainageBilling", "{0:N}") %>' CssClass="textbox"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtRetainageBilling_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" TargetControlID="txtRetainageBilling" ValidChars="1234567890,.-">
                                                    </asp:FilteredTextBoxExtender>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="panelPost" Visible="<%# _isPost || !_editAllow %>">
                                                    <asp:Label ID="lblRetainageBilling" runat="server" ForeColor='<%# Convert.ToBoolean(Eval("IsUpdateRetainage")) ? System.Drawing.Color.Blue : System.Drawing.Color.Black %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "RetainageBilling", "{0:N}") %>'></asp:Label>
                                                </asp:Panel>
                                                <asp:HiddenField runat="server" ID="hndIsUpdateRetainage" Value='<%# DataBinder.Eval(Container.DataItem, "IsUpdateRetainage") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Total Billing" DataField="TotalBilling" UniqueName="TotalBilling" SortExpression="TotalBilling" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalBilling" runat="server" ForeColor='<%# Convert.ToDouble(Eval("TotalBilling")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "TotalBilling", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="To be Billed" DataField="ToBeBilled" UniqueName="ToBeBilled" SortExpression="ToBeBilled" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToBeBilled" runat="server" ForeColor='<%# Convert.ToDouble(Eval("ToBeBilled")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "ToBeBilled", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Open AR" DataField="OpenARAmount" UniqueName="OpenARAmount" SortExpression="OpenARAmount" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOpenARAmount" runat="server" ForeColor='<%# Convert.ToDouble(Eval("OpenARAmount")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "OpenARAmount", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Costs and Estimated Earnings in Excess of Billings" DataField="Billings" UniqueName="Billings" SortExpression="Billings" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillings" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Billings")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Billings", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Billings in Excess of Costs and Estimated Earnings" DataField="Earnings" UniqueName="Earnings" SortExpression="Earnings" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEarnings" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Earnings")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "Earnings", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Percentage" DataField="NPer" UniqueName="NPer" SortExpression="NPer" AllowFiltering="false"
                                            ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNPer" runat="server" ForeColor='<%# Convert.ToDouble(Eval("NPer")) <= 1 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "NPer", "{0:0.00%}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Percentage" DataField="NPerLastMonth" UniqueName="NPerLastMonth" SortExpression="NPerLastMonth" AllowFiltering="false"
                                            ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNPerLastMonth" runat="server" ForeColor='<%# Convert.ToDouble(Eval("NPerLastMonth")) <= 1 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "NPerLastMonth", "{0:0.00%}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Percentage" DataField="NPerLastYear" UniqueName="NPerLastYear" SortExpression="NPerLastYear" AllowFiltering="false"
                                            ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNPerLastYear" runat="server" ForeColor='<%# Convert.ToDouble(Eval("NPerLastYear")) <= 1 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "NPerLastYear", "{0:0.00%}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Percentage" DataField="NPerLastMonthYear" UniqueName="NPerLastMonthYear" SortExpression="NPerLastMonthYear" AllowFiltering="false"
                                            ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNPerLastMonthYear" runat="server" ForeColor='<%# Convert.ToDouble(Eval("NPerLastMonthYear")) <= 1 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "NPerLastMonthYear", "{0:0.00%}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="BILLING ABOVE CONTRACT PRICE" DataField="BillingContract" UniqueName="BillingContract" SortExpression="BillingContract" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillingContract" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BillingContract")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "BillingContract", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Job Borrow (Assuming ALL Collected)" DataField="JobBorrow" UniqueName="JobBorrow" SortExpression="JobBorrow" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="EqualTo" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150" FooterAggregateFormatString="{0:c}" Aggregate="Sum" ShowFilterIcon="false" DataType="System.Double">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobBorrow" runat="server" ForeColor='<%# Convert.ToDouble(Eval("JobBorrow")) >= 0 ? System.Drawing.Color.Black : System.Drawing.Color.Red %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem, "JobBorrow", "{0:N}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Expected Closing Date" DataField="ExpectedClosingDate" UniqueName="ExpectedClosingDate" SortExpression="ExpectedClosingDate" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150"
                                            ShowFilterIcon="false" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Panel runat="server" ID="panelExpectedClosingDate" Visible="<%# !_isPost && _editAllow %>" CssClass="edit-column">
                                                   <asp:TextBox ID="txtExpectedClosingDate" runat="server" Text='<%# Eval("ExpectedClosingDate")!=DBNull.Value? (!(Eval("ExpectedClosingDate").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("ExpectedClosingDate"))) : "" ) : "" %>'
                                                        ValidationGroup="comp" CssClass="datepicker_mom" Style="width: 100%!important; font-size: inherit;" autocomplete="off"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revExpectedClosingDate" ControlToValidate="txtExpectedClosingDate" ValidationGroup="comp"
                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                        runat="server" ErrorMessage="Invalid Date format!" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceExpectedClosingDate" runat="server" Enabled="True" PopupPosition="Right" Width="200" TargetControlID="revExpectedClosingDate" />
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="panelPostExpectedClosingDate" Visible="<%# _isPost || !_editAllow %>">
                                                    <asp:Label ID="lblExpectedClosingDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpectedClosingDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewJob" Value="Y" />
    <asp:HiddenField runat="server" ID="hdPageNumber" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSelectedTab" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSortColumn" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSort" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdSelectedRow" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnProjectList" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
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
            $('#activeone').show();
            var id = $("#hdDept").val();
            $("#tabProject > li > a#" + id + "")[0].click();

        });

        function pageLoad(sender, args) {
            Materialize.updateTextFields();

            $("[id*=txtBRev]").change(function () {
                var txtBRevID = $(this).attr('id');
                CalculateAdjmts(txtBRevID, 'txtBRev');
            });

            $("[id*=txtConstModAdjmts]").change(function () {
                var txtConstModAdjmtsID = $(this).attr('id');
                CalculateAdjmts(txtConstModAdjmtsID, 'txtConstModAdjmts');
            });

            $("[id*=txtAccountingAdjmts]").change(function () {
                var txtAccountingAdjmtsID = $(this).attr('id');
                CalculateAdjmts(txtAccountingAdjmtsID, 'txtAccountingAdjmts');
            });

            $("[id*=txtRetainageBilling]").change(function () {
                var txtRetainageBillingID = $(this).attr('id');

                var txtRetainageBilling = document.getElementById(txtRetainageBillingID);
                var hndIsUpdateRetainage = document.getElementById(txtRetainageBillingID.replace('txtRetainageBilling', 'hndIsUpdateRetainage'));

                hndIsUpdateRetainage.value = "True";
                txtRetainageBilling.style.color = "blue";
                CalculateAdjmts(txtRetainageBillingID, 'txtRetainageBilling');
            });
        }

        function CalculateAdjmts(txtAdjmtsID, replaceID) {
            try {
                var txtBRev = document.getElementById(txtAdjmtsID.replace(replaceID, 'txtBRev'));
                var lblTotalBudgetedExpense = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblTotalBudgetedExpense'));
                var txtConstModAdjmts = document.getElementById(txtAdjmtsID.replace(replaceID, 'txtConstModAdjmts'));
                var txtAccountingAdjmts = document.getElementById(txtAdjmtsID.replace(replaceID, 'txtAccountingAdjmts'));
                var lblTotalEstimatedCost = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblTotalEstimatedCost'));
                var lblEstimatedProfit = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblEstimatedProfit'));
                var lblNCost = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblNCost'));
                var lblCostToComplete = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblCostToComplete'));
                var lblPercentageComplete = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblPercentageComplete'));
                var lblRevenuesEarned = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblRevenuesEarned'));
                var lblGrossProfit = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblGrossProfit'));
                var lblNRev = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblNRev'));
                var lblToBeBilled = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblToBeBilled'));
                var lblNPer = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblNPer'));
                var lblBillings = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblBillings'));
                var lblEarnings = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblEarnings'));
                var lblBillingContract = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblBillingContract'));
                var lblJobBorrow = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblJobBorrow'));
                var txtRetainageBilling = document.getElementById(txtAdjmtsID.replace(replaceID, 'txtRetainageBilling'));
                var lblTotalBilling = document.getElementById(txtAdjmtsID.replace(replaceID, 'lblTotalBilling'));

                var bRev = txtBRev.value != '' ? parseFloat(txtBRev.value.replace(/[\$\(\),]/g, '')) : 0;
                var totalBudgetedExpense = lblTotalBudgetedExpense.innerText != '' ? parseFloat(lblTotalBudgetedExpense.innerText.replace(/[\$\(\),]/g, '')) : 0;
                var constModAdjmtsID = txtConstModAdjmts.value != '' ? parseFloat(txtConstModAdjmts.value.replace(/[\$\(\),]/g, '')) : 0;
                var accountingAdjmts = txtAccountingAdjmts.value != '' ? parseFloat(txtAccountingAdjmts.value.replace(/[\$\(\),]/g, '')) : 0;
                var nCost = lblNCost.innerText != '' ? parseFloat(lblNCost.innerText.replace(/[\$\(\),]/g, '')) : 0;
                var nRev = lblNRev.innerText != '' ? parseFloat(lblNRev.innerText.replace(/[\$\(\),]/g, '')) : 0;
                var retainageBilling = txtRetainageBilling.value != '' ? parseFloat(txtRetainageBilling.value.replace(/[\$\(\),]/g, '')) : 0;

                // Calculate Adjmts
                var totalEstimatedCost = totalBudgetedExpense + constModAdjmtsID + accountingAdjmts;
                var estimatedProfit = bRev - totalEstimatedCost;
                var costToComplete = totalEstimatedCost - nCost;
                var percentageComplete = nCost / totalEstimatedCost;
                var revenuesEarned = parseFloat((nCost / totalEstimatedCost) * bRev);
                var grossProfit = parseFloat((nCost / totalEstimatedCost) * (bRev - totalEstimatedCost));
                var nPer = estimatedProfit / bRev;
                var totalBilling = nRev + retainageBilling;
                var billings = revenuesEarned > totalBilling ? revenuesEarned - totalBilling : 0;
                var earnings = totalBilling > revenuesEarned ? totalBilling - revenuesEarned : 0;
                var tobeBilled = bRev - totalBilling;
                var billingContract = totalBilling > bRev ? totalBilling - bRev : 0;
                var jobBorrow = totalBilling - grossProfit - nCost;

                //Update value
                document.getElementById(txtAdjmtsID).value = parseFloat(document.getElementById(txtAdjmtsID).value.replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblTotalEstimatedCost.innerHTML = totalEstimatedCost.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblEstimatedProfit.innerHTML = estimatedProfit.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblCostToComplete.innerHTML = costToComplete.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblPercentageComplete.innerHTML = percentageComplete.toLocaleString("en-US", { style: 'percent', minimumFractionDigits: 2 });
                lblRevenuesEarned.innerHTML = revenuesEarned.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblGrossProfit.innerHTML = grossProfit.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblNPer.innerHTML = nPer.toLocaleString("en-US", { style: "percent", minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblBillings.innerHTML = billings.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblEarnings.innerHTML = earnings.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblToBeBilled.innerHTML = tobeBilled.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblBillingContract.innerHTML = billingContract.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblJobBorrow.innerHTML = jobBorrow.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                lblTotalBilling.innerHTML = totalBilling.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });

                //Update color
                txtBRev.style.color = bRev < 0 ? "red" : "black";
                txtConstModAdjmts.style.color = constModAdjmtsID < 0 ? "red" : "black";
                txtAccountingAdjmts.style.color = accountingAdjmts < 0 ? "red" : "black";
                lblTotalEstimatedCost.style.color = totalEstimatedCost < 0 ? "red" : "black";
                lblEstimatedProfit.style.color = estimatedProfit < 0 ? "red" : "black";
                lblCostToComplete.style.color = costToComplete < 0 ? "red" : "black";
                lblPercentageComplete.style.color = percentageComplete > 1 ? "red" : "black";
                lblRevenuesEarned.style.color = revenuesEarned < 0 ? "red" : "black";
                lblGrossProfit.style.color = grossProfit < 0 ? "red" : "black";
                lblNPer.style.color = nPer > 1 ? "red" : "black";
                lblBillings.style.color = billings < 0 ? "red" : "black";
                lblEarnings.style.color = earnings < 0 ? "red" : "black";
                lblToBeBilled.style.color = tobeBilled < 0 ? "red" : "black";
                lblBillingContract.style.color = billingContract < 0 ? "red" : "black";
                lblJobBorrow.style.color = jobBorrow < 0 ? "red" : "black";
                lblTotalBilling.style.color = totalBilling < 0 ? "red" : "black";
            } catch (e) {

            }
        }

        function selectTab(id) {
            $('#<%=hdnTabId.ClientID%>').val(id);
            $("#hdDept").val(id);
            var grid = $find("<%= RadGrid_Project.ClientID %>");
            if (grid) {
                var tableView = grid.get_masterTableView();
                var filterExpressions = tableView.get_filterExpressions();
                console.log(tableView.get_filterExpressions());
                if (filterExpressions.length > 0) { var expression = filterExpressions[0]; }
                tableView.rebind();
            }
        }

    </script>
</asp:Content>
