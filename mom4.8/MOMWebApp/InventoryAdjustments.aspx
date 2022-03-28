<%@ Page Title="Item Adjustment || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="InventoryAdjustments" Codebehind="InventoryAdjustments.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <script src="js/jquery-ui-1.9.2.custom.js"></script>
    <link href="css/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
    <style>
        .srchtitlecustomwidth {
            min-width: 52px;
            }
    </style>

    <script type="text/javascript">

        function IsRoleValid() {

            var id = document.getElementById('<%= hdnAddInventoryAdjustment.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function CheckDelete() {
            var hasPermission = IsRoleValid();

            if (!hasPermission) {
                return;
            }
            var result = false;
            var gridCount = $("#<%=RadGrid_InventoryAdjustment.ClientID%> tbody tr input[type='checkbox']:checked").length;
            result = gridCount > 0;

            if (result == true) {
                return confirm('Do you really want to delete this ?');
            }
            else {
                ShowMessage('Please select a record to delete.', 'warning');
                return false;
            }
        }

        function ShowMessage(message, messageType) {
            noty({
                text: message,
                type: messageType,
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: true,
                theme: 'noty_theme_default',
                closable: true,
                timeout: 3000
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Hidden Field -->

    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-payment"></i>&nbsp;Item Adjustment</div>
                                    <div class="buttonContainer">
                                        <asp:Panel runat="server" ID="Panel1">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return IsRoleValid()" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>

                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick="return IsRoleValid()" OnClick="btnEdit_Click" Visible="false">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="return CheckDelete()" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                            </div>
                                        </asp:Panel>
                                        <ul id="dropdown1" class="dropdown-content">
                                            <li>
                                                <a href="CustomersReport.aspx?type=Customer">Add New Report</a>
                                            </li>
                                            <li>
                                                <a href="POWeeklyReport.aspx">PO Weekly Report</a>
                                            </li>

                                        </ul>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                            </a>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <%--<a id="A1" runat="server" tooltip="Close" causesvalidation="false" onclick="lnkClose_Click"><i class="mdi-content-clear"></i></a>--%>
                                        <asp:LinkButton ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" CssClass="mdi-content-clear" OnClick="lnkClose_Click" />

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
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">

                    <ContentTemplate>
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Date
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="datepicker_mom"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="datepicker_mom"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap tabcontainer">
                                <ul class="tabselect accrd-tabselect" id="testradiobutton">
                                    <li>
                                        <asp:LinkButton AutoPostBack="False" ID="LinkButton3" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClick="decDate_Click"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <label id="lblDay" runat="server">
                                            <asp:RadioButton runat="server" ID="rdDay" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdDay_CheckedChanged" />
                                            Day
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblWeek" runat="server">
                                            <asp:RadioButton runat="server" ID="rdWeek" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdWeek_CheckedChanged" />
                                            Week
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblMonth" runat="server">
                                            <asp:RadioButton runat="server" ID="rdMonth" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdMonth_CheckedChanged" />
                                            Month
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblQuarter" runat="server">
                                            <%--<input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnRecvDate', 'rdCal')" />--%>
                                            <asp:RadioButton runat="server" ID="rdQuarter" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdQuarter_CheckedChanged" />
                                            Quarter
                                        </label>
                                    </li>
                                    <li>
                                        <label id="lblYear" runat="server">
                                            <asp:RadioButton runat="server" ID="rdYear" GroupName="rdCal" AutoPostBack="True" OnCheckedChanged="rdYear_CheckedChanged" />
                                            Year
                                        </label>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LinkButton4" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>" OnClick="incDate_Click"></asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                            <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                Search
                            </div>
                            <div class="srchinputwrap">

                                <asp:DropDownList ID="ddlSearch" runat="server"
                                    CssClass="browser-default select selectst selectsml" ClientIDMode="Static">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="pd-negate srchcstm"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap srchclr btnlinksicon">
                                <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false"
                                    OnClick="lnkSearch_Click">
                                                                <i class="mdi-action-search"></i>
                                </asp:LinkButton>
                            </div>
                            <div class="col lblsz2 lblszfloat">
                                <div class="row">
                                    <span class="tro trost">
                                        <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">
                                   Clear </asp:LinkButton>
                                    </span>
                                    <span class="tro trost">
                                        <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" Visible="false">Show All </asp:LinkButton>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_container">
            <div class="form-section-row" style="margin-bottom: 0 !important;">

                <telerik:RadAjaxManager ID="RadAjaxManager_InventoryAdjustment" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_InventoryAdjustment" LoadingPanelID="RadAjaxLoadingPanel_InventoryAdjustment" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_InventoryAdjustment" LoadingPanelID="RadAjaxLoadingPanel_InventoryAdjustment" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkClear">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_InventoryAdjustment" LoadingPanelID="RadAjaxLoadingPanel_InventoryAdjustment" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_InventoryAdjustment" LoadingPanelID="RadAjaxLoadingPanel_InventoryAdjustment" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_InventoryAdjustment">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_InventoryAdjustment" LoadingPanelID="RadAjaxLoadingPanel_InventoryAdjustment" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_InventoryAdjustment" runat="server">
                </telerik:RadAjaxLoadingPanel>

                <div class="RadGrid RadGrid_Material">
                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= RadGrid_InventoryAdjustment.ClientID %>");
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
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_InventoryAdjustment" runat="server" LoadingPanelID="RadAjaxLoadingPanel_InventoryAdjustment" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <telerik:RadPersistenceManager ID="RadPersistence1" runat="server">
                            <PersistenceSettings>
                                <telerik:PersistenceSetting ControlID="RadGrid_InventoryAdjustment" />
                            </PersistenceSettings>
                        </telerik:RadPersistenceManager>
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_InventoryAdjustment" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                            OnNeedDataSource="RadGrid_InventoryAdjustment_NeedDataSource"
                            OnPreRender="RadGrid_InventoryAdjustment_PreRender"
                            OnItemCreated="RadGrid_InventoryAdjustment_ItemCreated"
                            OnItemEvent="RadGrid_InventoryAdjustment_ItemEvent">
                            <%--
                                     
                                     OnPreRender="RadGrid_InventoryAdjustment_PreRender" 
                                OnItemEvent="RadGrid_InventoryAdjustment_ItemEvent"--%>
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" DataBinding-EnableCaching="true">
                                <Selecting AllowRowSelect="True"></Selecting>

                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                <Columns>
                                    <%-- <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSelected" runat="server" />
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnStatus" Value='<%# Bind("Status") %>' runat="server" />
                                                <asp:HiddenField ID="hdnid" runat="server" Value='<%# Bind("ID") %>' ClientIDMode="Static" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>

                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="30">
                                    </telerik:GridClientSelectColumn>

                                    <telerik:GridTemplateColumn DataField="ID" HeaderText="Ref" SortExpression="ID" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="80">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="fDate" HeaderText="Date" SortExpression="ID" DataType="System.String"
                                        AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="80">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("fDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--Description--%>
                                    <telerik:GridTemplateColumn DataField="fDesc" HeaderText="Description" SortExpression="fDesc" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="150">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("fDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--Name--%>
                                    <telerik:GridTemplateColumn DataField="Name" HeaderText="Item" SortExpression="Name" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="130">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                      <%--Itemsfdesc--%>
                                    <telerik:GridTemplateColumn DataField="Itemsfdesc" HeaderText="Item Desc" SortExpression="Itemsfdesc" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="150">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNameDesc" runat="server" Text='<%# Eval("Itemsfdesc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                      <%--Warehouse--%>
                                    <telerik:GridTemplateColumn DataField="WHName" HeaderText="Warehouse" SortExpression="WHName" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="130">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarehouse" runat="server" Text='<%# Eval("WHName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                     <%--WarehouseLocation--%>
                                    <telerik:GridTemplateColumn DataField="WHLoc" HeaderText="Warehouse Location" SortExpression="WHLoc" DataType="System.String"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="130">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWarehouseLocation" runat="server" Text='<%# Eval("WHLoc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--Quantity--%>
                                    <telerik:GridTemplateColumn DataField="Quan" HeaderText="Adjusted Quantity" SortExpression="Quan" DataType="System.String"
                                        AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderStyle-Width="80">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quan") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--Amount--%>

                                    <telerik:GridTemplateColumn DataField="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Adjusted Amount" ShowFilterIcon="false" HeaderStyle-Width="80">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>
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
    <asp:HiddenField runat="server" ID="hdnAddInventoryAdjustment" Value="Y" />
</asp:Content>

