<%@ Page Title="Item Master || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Inventory" CodeBehind="Inventory.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />

    <script type="text/javascript">
        function AddInventoryClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddInventoryItem.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function CopyInventoryClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddInventoryItem.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function DeleteInventoryClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteInventoryItem.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_Inv.ClientID%>', 'Inventory');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

    </script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //To remove added li before appending again
            $('#colorNav #dynamicUI li').remove();
            $(reports).each(function (index, report) {
                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }
                $('#dynamicUI').append('<li><a href="CustomersReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Customer"><span> <i class="fa fa-file-text fa-lg" aria-hidden="true"></i>' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')
            });
        });
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
                                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Item Master</asp:Label>
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" OnClientClick='return AddInventoryClick(this)' runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditInventoryClick(this)' OnClick="btnSubmit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>

                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnCopy" runat="server" OnClientClick='return CopyInventoryClick(this)' OnClick="btnCopy_Click">Copy</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnDelete" OnClientClick='return DeleteInventoryClick(this)' runat="server" OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </div>

                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" data-activates="dropdown1">Reports</a>
                                                    </div>
                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                            <asp:LinkButton ID="lnkInventoryReport" runat="server" OnClick="lnkInventoryReport_Click">Inventory Report</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                <div class="srchtitle ser-css2">
                    Search
                </div>
                <div class="srchinputwrap">
                    <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst" ClientIDMode="Static" onchange="showfilter();return false;"></asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbStatus" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlInvStatus" runat="server" CssClass="browser-default selectst" Visible="true">
                    </asp:DropDownList>
                </div>

                <div class="srchinputwrap" id="div_rbABC" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlABC" runat="server" CssClass="browser-default selectst" Width="155px" Visible="true">
                    </asp:DropDownList>
                </div>

                <div class="srchinputwrap" id="div_rbCommodity" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlCommodity" runat="server" CssClass="browser-default selectst" Width="155px" Visible="true">
                    </asp:DropDownList>
                </div>

                <div class="srchinputwrap" id="div_rbApprovedVendor" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlInventoryApprovedVendor" runat="server" CssClass="browser-default selectst" Width="155px" Visible="true">
                    </asp:DropDownList>
                </div>

                <div class="srchinputwrap" id="div_txtSearch" style="display: block" runat="server">
                    <asp:TextBox ID="txtSearch" runat="server" class="srchcstm" placeholder="Search"></asp:TextBox>
                </div>

                <div class="srchinputwrap srchclr btnlinksicon">
                    <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false" CssClass="srchbtn"
                        OnClick="lnkSearch_Click"> <i class="mdi-action-search"></i></asp:LinkButton>
                </div>
                <div class="col lblsz2 lblszfloat">
                    <div class="row">
                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" Text="Incl. Inactive" CssClass="css-checkbox" AutoPostBack="True"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click" OnClientClick="resetShowAll();">Show All Items</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <span>
                                        <asp:Label ID="lblRecordCount" runat="server" ClientIDMode="Static"></asp:Label>
                                    </span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </span>
                    </div>
                </div>

            </div>
            <div class="grid_container">
                <div class="form-section-row pmd-card">
                    <telerik:RadAjaxManager ID="RadAjaxManager_Inv" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Inv" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Inv" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Inv" LoadingPanelID="RadAjaxLoadingPanel_Inv" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Inv" runat="server">
                    </telerik:RadAjaxLoadingPanel>

                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Inv.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Inv" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Inv" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Inv" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_Inv_PreRender" OnItemEvent="RadGrid_Inv_ItemEvent"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_Inv_NeedDataSource" OnExcelMLExportRowCreated="RadGrid_Inv_ExcelMLExportRowCreated" OnItemCreated="RadGrid_Inv_ItemCreated">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Bind("id") %>' ClientIDMode="Static" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" UniqueName="Name"
                                            CurrentFilterFunction="Contains" HeaderText="Part Number" HeaderStyle-Width="198" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPartNumber" Text='<%# Bind("Name")%>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="fDesc" HeaderText="Description" HeaderStyle-Width="140" UniqueName="fDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="StrStatus" HeaderText="Status" HeaderStyle-Width="115" UniqueName="StrStatus"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="DateCreated" SortExpression="DateCreated" AutoPostBackOnFilter="true" UniqueName="DateCreated"
                                            CurrentFilterFunction="Contains" HeaderText="Date Created" ShowFilterIcon="false" HeaderStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateCreated" runat="server" Text='<%# Eval("DateCreated")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "DateCreated"))):""%> '>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Hand" HeaderText="On Hand" HeaderStyle-Width="90" UniqueName="Hand"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" FooterAggregateFormatString="{0:n}" Aggregate="Sum"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="fOrder" HeaderText="On Order" HeaderStyle-Width="90" UniqueName="fOrder"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" FooterAggregateFormatString="{0:n}" Aggregate="Sum"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Committed" HeaderText="Committed" HeaderStyle-Width="90" UniqueName="Committed"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" FooterAggregateFormatString="{0:n}" Aggregate="Sum"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Available" HeaderText="Available" HeaderStyle-Width="90" UniqueName="Available"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="catName" HeaderText="Category" HeaderStyle-Width="90" UniqueName="CatName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="WarehouseCount" SortExpression="WarehouseCount" AutoPostBackOnFilter="true" UniqueName="WarehouseCount"
                                            CurrentFilterFunction="EqualTo" HeaderText="WarehouseCount" ShowFilterIcon="false" HeaderStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkWarehouseCount" runat="server"><%#Eval("WarehouseCount")%></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="UnitCost" HeaderText="Unit Cost" HeaderStyle-Width="90" UniqueName="UnitCost"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Balance" HeaderText="Value" HeaderStyle-Width="90" UniqueName="Balance"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
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

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddInventoryItem" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditInventoryItem" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteInventoryItem" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewInventoryItem" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
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

        function resetShowAll() {
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearch.ClientID%>');
            var rbStatus = document.getElementById('<%=div_rbStatus.ClientID%>');
            var rbABC = document.getElementById('<%=div_rbABC.ClientID%>');
            var rbCommodity = document.getElementById('<%=div_rbCommodity.ClientID%>');
            var rbApprovedVendor = document.getElementById('<%=div_rbApprovedVendor.ClientID%>');

            txtSearch.style.display = "block";
            rbStatus.style.display = "none";
            rbABC.style.display = "none";
            rbCommodity.style.display = "none";
            rbApprovedVendor.style.display = "none";

            try {
                $('#<%=ddlSearch.ClientID%>').val($("#<%=ddlSearch.ClientID%> option:first").val());
                $('#<%=txtSearch.ClientID%>').val("");
                $('#<%=ddlInvStatus.ClientID%>').val($("#<%=ddlInvStatus.ClientID%> option:first").val());
                $('#<%=ddlABC.ClientID%>').val($("#<%=ddlABC.ClientID%> option:first").val());
                $('#<%=ddlCommodity.ClientID%>').val($("#<%=ddlCommodity.ClientID%> option:first").val());
                $('#<%=ddlInventoryApprovedVendor.ClientID%>').val($("#<%=ddlInventoryApprovedVendor.ClientID%> option:first").val());
            } catch (ex) { }
        }

        function showfilter() {
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearch.ClientID%>');
            var rbStatus = document.getElementById('<%=div_rbStatus.ClientID%>');
            var rbABC = document.getElementById('<%=div_rbABC.ClientID%>');
            var rbCommodity = document.getElementById('<%=div_rbCommodity.ClientID%>');
            var rbApprovedVendor = document.getElementById('<%=div_rbApprovedVendor.ClientID%>');

            txtSearch.style.display = "none";
            rbStatus.style.display = "none";
            rbABC.style.display = "none";
            rbCommodity.style.display = "none";
            rbApprovedVendor.style.display = "none";

            switch (String(ddlSearch.val())) {
                case "Status":
                    rbStatus.style.display = "block";
                    break;
                case "ABCClass":
                    rbABC.style.display = "block";
                    break;
                case "Commodity":
                    rbCommodity.style.display = "block";
                    break;
                case "ApprovedVendor":
                    rbApprovedVendor.style.display = "block";
                    break;
                default:
                    txtSearch.style.display = "block";
                    var txt = $('#<%=txtSearch.ClientID%>');
                    txt.val("");
                    break;
            }

            try {
                $('#<%=ddlInvStatus.ClientID%>').val($("#<%=ddlInvStatus.ClientID%> option:first").val());
                $('#<%=ddlABC.ClientID%>').val($("#<%=ddlABC.ClientID%> option:first").val());
                $('#<%=ddlCommodity.ClientID%>').val($("#<%=ddlCommodity.ClientID%> option:first").val());
                $('#<%=ddlInventoryApprovedVendor.ClientID%>').val($("#<%=ddlInventoryApprovedVendor.ClientID%> option:first").val());
            }
            catch (ex) { }
        }
    </script>
</asp:Content>
