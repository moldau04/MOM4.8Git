<%@ Page Title="Equipment || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Equipments" EnableEventValidation="true" CodeBehind="Equipments.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <script type="text/javascript">
        function AddEquipmentClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeEquipment.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditEquipmentClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeEquipment.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteEquipmentClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteEquipment.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('ctl00_ContentPlaceHolder1_gvEquip', 'Equipment');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function CopyEquipmentClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeEquipment.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            debugger;
            $('#dynamicUI li').remove();
            $(reports).each(function (index, report) {


                $('#ctl00_ContentPlaceHolder1_dynamicUI').append('<li><a href="EquipmentReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Equipment"> ' + report.ReportName + '</a></li>')



            });
        });
    </script>
   <style type="text/css">
       @media only screen and (max-width: 992px) {
        .selectst{
            width:100%;
        }
        }
       </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">


                                    <div class="page-title"><i class="mdi-maps-place"></i>&nbsp;Equipment</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkAddnew" ToolTip="Add New" runat="server" OnClientClick='return AddEquipmentClick(this)' OnClick="lnkAddnew_Click">Add</asp:LinkButton>

                                        </div>

                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnEdit" runat="server" ToolTip="Edit" OnClientClick='return EditEquipmentClick(this)' OnClick="btnSubmit_Click">Edit</asp:LinkButton>
                                        </div>

                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>
                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnCopy" runat="server" ToolTip="Copy" OnClientClick='return CopyEquipmentClick(this)' OnClick="btnCopy_Click">Copy</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnDelete" runat="server" ToolTip="Delete" OnClientClick='return DeleteEquipmentClick(this)' OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>

                                                <div class="btnlinks">
                                                    <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="ctl00_ContentPlaceHolder1_dynamicUI" runat="server" id="printbtn">Reports
                                                    </a>
                                                </div>
                                                <ul id="dynamicUI" class="dropdown-content" runat="server">
                                                    <li>
                                                        <a href="EquipmentReport.aspx?type=Equipment">Add New Report</a>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkDowntimeEquipmentReport" OnClick="lnkDowntimeEquipmentReport_Click" runat="server">Downtime Equipment Report</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <a href="EquipmentShutdownReport.aspx?type=0">Equipment Shut Down Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="EquipmentShutdownReport.aspx?type=1&filtered=1">Equipment Shut Down Activity Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="EquipmentQRCodeAvery5163.aspx?f=c">Equipment QR Code Avery 5163 Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="MaintenanceEquipmentCount.aspx">Maintenance Equipment Count Report</a>
                                                    </li>
                                                    <li>
                                                        <a href="PastDueMCPReport.aspx?redirect=Equipments.aspx">Past Due MCP Report</a>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>

                                </div>

                            </div>

                        </div>
                    </div>
                </header>
            </div>


            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="tblnks-advanced">
                            <ul class="anchor-links">
                                <li><a class="add-btn-click">Advanced Search</a></li>
                            </ul>
                        </div>


                    </div>
                </div>
            </div>

            <%--ADVANCED SEARCH DROPDOWN--%>
            <div id="stats" style="background-color: #fff !important;">
                <div id="addinfo" class="form-section-row infoDiv inline-css">
                    <div class="form-section-row">
                        <asp:UpdatePanel runat="server" ID="updPanelAdvSrch" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-sectioncustom1">
                                    <div class="section-ttle">Search Criteria</div>
                                    <div class="collapse_wrap">
                                        <div class="form-collapsewrap1">
                                            <div class="form_collapserow">
                                                <div class="form_collapsehalf1">
                                                    <div class="input-field col s12">
                                                        <label class="drpdwn-label">Status</label>
                                                        <asp:DropDownList ID="ddlFilterStatus" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="-1">All</asp:ListItem>
                                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>

                                                <div class="form_collapsehalf2">
                                                    <div class="input-field col s12">
                                                        <label class="drpdwn-label">
                                                            <asp:Label ID="lblType" runat="server"></asp:Label></label>
                                                        <asp:DropDownList ID="ddlFilterType" runat="server" class="browser-default">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblCategory" runat="server"></asp:Label></label>
                                                    <asp:DropDownList ID="ddlFilterCategory" runat="server" class="browser-default">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblClassification" runat="server"></asp:Label></label>
                                                    <asp:DropDownList ID="ddlFilterClassification" runat="server" class="browser-default">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label class="drpdwn-label">
                                                        <asp:Label ID="lblBuilding" runat="server"></asp:Label></label>


                                                    <asp:DropDownList ID="ddlBuilding" runat="server" class="browser-default">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>


                                        </div>

                                        <div class="form-collapsewrap2">
                                            <div class="form_collapserow">
                                                <div class="input-field col s12">
                                                    <label>Manufacturer</label>
                                                    <asp:TextBox ID="txtManufact" runat="server"></asp:TextBox>

                                                </div>
                                            </div>

                                            <div class="form_collapserow">
                                                <div class="form_collapsehalf1">
                                                    <div class="form_collapsehalf1a">
                                                        <div class="form_collapsehalf1alabel">
                                                            <div class="input-field col s12">
                                                                Last Service Date
                                                            </div>
                                                        </div>
                                                        <div class="form_collapsehalf11a">
                                                            <div class="form_collapsehalf11half">
                                                                <div class="input-field col s12">
                                                                    <asp:DropDownList ID="ddlcompare" runat="server" CssClass="browser-default">
                                                                        <asp:ListItem Value="0">=</asp:ListItem>
                                                                        <asp:ListItem Value="1">>=</asp:ListItem>
                                                                        <asp:ListItem Value="2"><=</asp:ListItem>
                                                                        <asp:ListItem Value="3">></asp:ListItem>
                                                                        <asp:ListItem Value="4"><</asp:ListItem>
                                                                    </asp:DropDownList>

                                                                </div>
                                                            </div>
                                                            <div class="form_collapsehalf22half">
                                                                <div class="input-field col s12">

                                                                    <asp:TextBox ID="txtLastServiceDt" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="form_collapsehalf2">
                                                    <div class="form_collapsehalf1b">
                                                        <div class="form_collapsehalf1alabel">
                                                            <div class="input-field col s12">
                                                                Install Date
                                                            </div>
                                                        </div>
                                                        <div class="form_collapsehalf11a">
                                                            <div class="form_collapsehalf11half">
                                                                <div class="input-field col s12">
                                                                    <asp:DropDownList ID="ddlComareI" runat="server" CssClass="browser-default">
                                                                        <asp:ListItem Value="0">=</asp:ListItem>
                                                                        <asp:ListItem Value="1">>=</asp:ListItem>
                                                                        <asp:ListItem Value="2"><=</asp:ListItem>
                                                                        <asp:ListItem Value="3">></asp:ListItem>
                                                                        <asp:ListItem Value="4"><</asp:ListItem>
                                                                    </asp:DropDownList>

                                                                </div>
                                                            </div>
                                                            <div class="form_collapsehalf22half">
                                                                <div class="input-field col s12">
                                                                    <asp:TextBox ID="txtInstallDt" runat="server"></asp:TextBox>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form_collapserow">
                                                <div class="form_collapsehalf1b">
                                                    <div class="form_collapsehalf1alabel">Price</div>
                                                    <div class="form_collapsehalf11a">
                                                        <div class="form_collapsehalf11half">
                                                            <div class="input-field col s12">
                                                                <asp:DropDownList ID="ddlCompareP" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">=</asp:ListItem>
                                                                    <asp:ListItem Value="1">&gt;=</asp:ListItem>
                                                                    <asp:ListItem Value="2">&lt;=</asp:ListItem>
                                                                    <asp:ListItem Value="3">&gt;</asp:ListItem>
                                                                    <asp:ListItem Value="4">&lt;</asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>
                                                        </div>
                                                        <div class="form_collapsehalf22half">
                                                            <div class="input-field col s12">
                                                                <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="form-sectioncustom2">
                                    <div class="section-ttle">Stats</div>
                                    <div class="collapse_wrap">
                                        <div class="nearest-wrap">
                                            <div class="nearest-head">
                                                <span class="worker-name"><b>Contract Type:</b></span>
                                                <span class="nearest-time">Personal</span>
                                            </div>
                                        </div>
                                        <div class="nearest-wrap">
                                            <div class="nearest-head">
                                                <span class="worker-name"><b># of Calls on Avg:</b></span>
                                                <span class="nearest-time">45</span>
                                            </div>
                                        </div>
                                        <div class="nearest-wrap">
                                            <div class="nearest-head">
                                                <span class="worker-name"><b>Random Info #1</b></span>
                                                <span class="nearest-time">Random</span>
                                            </div>
                                        </div>
                                        <div class="nearest-wrap">
                                            <div class="nearest-head">
                                                <span class="worker-name"><b>Random Info #2</b></span>
                                                <span class="nearest-time">Random</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="container">
        <div class="row">

            <div class="srchpane-advanced">
                <%-- <asp:UpdatePanel runat="server" ID="updPanelSrch" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <div class="srchtitle srchtitlecustomwidth">
                    Search
                </div>
                <div class="srchinputwrap">
                    <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst"
                        onchange="showFilterSearch();">
                        <asp:ListItem Value="">Select</asp:ListItem>
                        <asp:ListItem Value="e.Unit">Name</asp:ListItem>
                        <asp:ListItem Value="e.type">Type</asp:ListItem>
                        <asp:ListItem Value="e.fdesc">Description</asp:ListItem>
                        <asp:ListItem Value="e.Cat">Service Type</asp:ListItem>
                        <asp:ListItem Value="e.Status">Status</asp:ListItem>
                        <asp:ListItem Value="r.name">Customer</asp:ListItem>
                        <asp:ListItem Value="l.id">Location ID</asp:ListItem>
                        <asp:ListItem Value="l.tag">Location</asp:ListItem>
                        <asp:ListItem Value="address">Address</asp:ListItem>
                        <asp:ListItem Value="B.Name">Company</asp:ListItem>
                        <asp:ListItem Value="l.state">State</asp:ListItem>
                        <asp:ListItem Value="e.Serial">Serial #</asp:ListItem>
                        <asp:ListItem Value="e.state">Unique #</asp:ListItem>
                        <asp:ListItem Value="rt.Name">Route</asp:ListItem>
                        <asp:ListItem Value="tr.Name">Salesperson</asp:ListItem>
                    </asp:DropDownList>
                </div>


                <div class="srchinputwrap">
                    <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default selectst"
                        Style="display: none">
                        <asp:ListItem Value="0">Active</asp:ListItem>
                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default selectst" Style="display: none">
                    </asp:DropDownList>

                    <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="browser-default selectst"
                        Style="display: none">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default selectst"
                        Style="display: none">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>

                    <telerik:RadComboBox RenderMode="Auto" ID="ddlRoute" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CssClass="browser-default selectst selectsml"
                        Style="display: none">
                    </telerik:RadComboBox>
                    <asp:DropDownList ID="ddlTerr" runat="server" CssClass="browser-default selectst selectsml"
                        Style="display: none">
                    </asp:DropDownList>

                </div>
                <div class="srchinputwrap srchclr btnlinksicon">
                    <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false"
                        OnClick="lnkSearch_Click" ToolTip="Search">  <i class="mdi-action-search"></i></asp:LinkButton>
                </div>

                <div class="col lblsz2 lblszfloat">
                    <div class="row">
                        <div class="trobtns">
                            <span class="tro btnlinks">
                                <asp:LinkButton ID="lnkQRCode"
                                    runat="server" OnClick="lnkQRCode_Click">QR Codes</asp:LinkButton>
                            </span>
                            <span class="tro btnlinks">
                                <asp:LinkButton ID="lnkMassMCP"
                                    runat="server" OnClick="lnkMassMCP_Click">Mass MCP</asp:LinkButton>
                            </span>
                        </div>

                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive" OnCheckedChanged="lnkchk_Click"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClientClick="clearShowAll();" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>

                        </span>
                        <span class="tro trost">
                            <%-- <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>--%>
                            <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                            <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>


                        </span>
                    </div>
                </div>
                <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>


        </div>
        <div class="row">
        </div>

        <div class="grid_container">
            <div class="form-section-row">
                <telerik:RadAjaxManager ID="RadAjaxManager_Equip" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Equip" LoadingPanelID="RadAjaxLoadingPanel_Equip" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Equip" LoadingPanelID="RadAjaxLoadingPanel_Equip" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Equip" LoadingPanelID="RadAjaxLoadingPanel_Equip" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                                <telerik:AjaxUpdatedControl ControlID="rbStatus" />
                                <telerik:AjaxUpdatedControl ControlID="ddlType" />
                                <telerik:AjaxUpdatedControl ControlID="ddlServiceType" />
                                <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                                <telerik:AjaxUpdatedControl ControlID="lnkChk" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_Equip">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Equip" LoadingPanelID="RadAjaxLoadingPanel_Equip" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>

                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Equip" runat="server">
                </telerik:RadAjaxLoadingPanel>

                <div class="RadGrid RadGrid_Material">
                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= RadGrid_Equip.ClientID %>");
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
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Equip" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Equip" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Equip" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_Equip_PreRender" OnItemCreated="RadGrid_Equip_ItemCreated"
                            AllowCustomPaging="True" OnNeedDataSource="RadGrid_Equip_NeedDataSource" OnExcelMLExportRowCreated="RadGrid_Equip_ExcelMLExportRowCreated" OnItemEvent="RadGrid_Equip_ItemEvent"
                            OnItemDataBound="RadGrid_Equip_ItemDataBound" PagerStyle-AlwaysVisible="true" OnPageIndexChanged="RadGrid_Equip_PageIndexChanged" OnPageSizeChanged="RadGrid_Equip_PageSizeChanged">
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>

                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                <%--<Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>--%>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Name">
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="28" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false" HeaderStyle-Width="10" AllowFiltering="false" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderStyle-Width="130"
                                        DataField="unit" SortExpression="unit" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" HeaderText="Name" ShowFilterIcon="false" UniqueName="unit">
                                        <ItemTemplate>
                                            <%--    <asp:Label ID="lblUnit" runat="server"><a><%# Eval("unit") %></a></asp:Label>--%>
                                            <asp:HyperLink ID="lnkUnit" runat="server"><%#Eval("unit")%></asp:HyperLink>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server">Total :-</asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--  <telerik:GridBoundColumn  DataField="unit" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        HeaderText="Name" SortExpression="unit"
                                        UniqueName="unit" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridBoundColumn DataField="manuf" AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains"
                                        HeaderText="Manuf" SortExpression="manuf" HeaderStyle-Width="130"
                                        UniqueName="manuf">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fdesc" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        HeaderText="Description" SortExpression="fdesc" HeaderStyle-Width="100"
                                        UniqueName="fdesc" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Type" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Type" SortExpression="Type" HeaderStyle-Width="80"
                                        UniqueName="Type">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Classification" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Classification" SortExpression="Classification" HeaderStyle-Width="100"
                                        UniqueName="Classification">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cat" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Service Type" SortExpression="cat" HeaderStyle-Width="120"
                                        UniqueName="cat">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="status" SortExpression="status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                        HeaderText="Status" ShowFilterIcon="false" UniqueName="status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server"><%# Eval("status")%></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div style="padding: 0 0 5px 0">
                                                <asp:Label ID="lblTotalActive" runat="server" />
                                            </div>
                                            <div>
                                                <asp:Label ID="lblTotalInActive" runat="server" />
                                            </div>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <%--<telerik:GridBoundColumn DataField="status" SortExpression="status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="70"
                                        HeaderText="Status" ShowFilterIcon="false" UniqueName="status">
                                        
                                    </telerik:GridBoundColumn>--%>
                                    <%--<telerik:GridTemplateColumn  DataField="shut_down" AutoPostBackOnFilter="true" AllowFiltering="true" SortExpression="shut_down"
                                        CurrentFilterFunction="Contains" UniqueName="shut_down" HeaderText="Shut Down" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShutdown" runat="server"><%# Convert.ToInt32( Eval("shut_down")) == 0 ? "No" : "Yes"%></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                      <telerik:GridBoundColumn DataField="State" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Unique" SortExpression="State" HeaderStyle-Width="120"
                                        UniqueName="State">
                                    </telerik:GridBoundColumn>
                                      <telerik:GridBoundColumn DataField="Serial" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Serial" SortExpression="Serial" HeaderStyle-Width="120"
                                        UniqueName="Serial">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="shut_down" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Shut Down" SortExpression="shut_down" HeaderStyle-Width="120"
                                        UniqueName="shut_down">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="building" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Building" SortExpression="building" HeaderStyle-Width="100"
                                        UniqueName="building">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Company" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Company" SortExpression="Company" HeaderStyle-Width="80"
                                        UniqueName="Company">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Category" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" HeaderText="Category" SortExpression="Category" HeaderStyle-Width="100"
                                        UniqueName="Category">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="name" AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains"
                                        HeaderText="Customer" SortExpression="name" HeaderStyle-Width="120"
                                        UniqueName="name">
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn DataField="locid" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        HeaderText="Location ID" SortExpression="locid" HeaderStyle-Width="140"
                                        UniqueName="locid" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="tag" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        HeaderText="Location" SortExpression="tag" HeaderStyle-Width="120"
                                        UniqueName="tag" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Address" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        HeaderText="Address" SortExpression="Address"
                                        UniqueName="Address" ShowFilterIcon="false" HeaderStyle-Width="200">
                                    </telerik:GridBoundColumn>


                                    <telerik:GridTemplateColumn DataField="Price" FooterAggregateFormatString="{0:c}"
                                        Aggregate="Sum" SortExpression="Price" AutoPostBackOnFilter="true" HeaderStyle-Width="80"
                                        CurrentFilterFunction="EqualTo" HeaderText="Price" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrice" runat="server"><%#Eval("Price")%></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="last" SortExpression="last" AutoPostBackOnFilter="true" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderStyle-Width="100" HeaderText="Last Service" ShowFilterIcon="false" UniqueName="last">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllast" runat="server"><%# Eval("last")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "last"))):""%></asp:Label>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="Install" SortExpression="Install" AutoPostBackOnFilter="true" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Installed" HeaderStyle-Width="100" ShowFilterIcon="false" UniqueName="Install">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSince" runat="server"><%# Eval("Install") != DBNull.Value ? String.Format("{0:M/d/yyyy}", Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "Install"))) : ""%></asp:Label>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="Salesperson" SortExpression="Salesperson" AutoPostBackOnFilter="true" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Salesperson" HeaderStyle-Width="100" ShowFilterIcon="false" UniqueName="Salesperson">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesperson" runat="server"><%# Eval("Salesperson") %></asp:Label>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="RouteName" SortExpression="RouteName" AutoPostBackOnFilter="true" DataType="System.String"
                                        CurrentFilterFunction="Contains" HeaderText="Route Name" HeaderStyle-Width="100" ShowFilterIcon="false" UniqueName="RouteName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRouteName" runat="server"><%# Eval("RouteName") %></asp:Label>
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

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteEquipment" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewEquipment" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>

    <script>
        $(document).ready(function () {


            $('#addinfo').hide();
            $('.add-btn-click').click(function () {

                $('#addinfo').slideToggle('2000', "swing", function () {
                    // Animation complete.

                });

                if ($('.divbutton-container').height() != 65)
                    $('.divbutton-container').animate({ height: 65 }, 500);
                else
                    $('.divbutton-container').animate({ height: 350 }, 500);


            });


            $('.link-slide').on('click', function (e) {
                e.preventDefault();

                var target = this.hash;
                var $target = $(target);
                if ($(target).hasClass('active') || target == "") {

                }
                else {
                    $(target).click();
                }

                $('html, body').stop().animate({
                    'scrollTop': $target.offset().top - 125
                }, 900, 'swing');
            });

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });

        });

        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtLastServiceDt'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                maxDate: new Date(2020, 12, 31),
                yearRange: [2000, 2020]
            });
    </script>

    <script>
        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtInstallDt'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                maxDate: new Date(2020, 12, 31),
                yearRange: [2000, 2020]
            });

        function showFilterSearch() {
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var ddlServiceType = $("#<%=ddlServiceType.ClientID%>");
            var ddlType = $("#<%=ddlType.ClientID%>");
            var ddlStatus = $("#<%=rbStatus.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            var ddlCompany = $("#<%=ddlCompany.ClientID%>");
            var ddlRoute = $("#<%=ddlRoute.ClientID%>");
            var ddlTerr = $("#<%=ddlTerr.ClientID%>");


            ddlTerr.css("display", "none");
            ddlRoute.css("display", "none");
            ddlServiceType.css("display", "none");
            ddlType.css("display", "none");
            ddlStatus.css("display", "none");
            ddlCompany.css("display", "none");
            txtSearch.css("display", "none");
            txtSearch.val('');

            if (ddlSearch.val() === "e.type") {
                ddlType.css("display", "block");
            }
            else if (ddlSearch.val() === "e.Cat") {
                ddlServiceType.css("display", "block");
            }
            else if (ddlSearch.val() === "e.Status") {
                ddlStatus.css("display", "block");
            } else if (ddlSearch.val() === "B.Name") {
                ddlCompany.css("display", "block");
            }
            else if (ddlSearch.val() === "rt.Name") {
                ddlRoute.css("display", "block");
            } else if (ddlSearch.val() === "tr.Name") {
                ddlTerr.css("display", "block");
            }
            else {
                txtSearch.css("display", "block");

            }
            try {
                ddlType.get(0).selectedIndex = 0;
                ddlServiceType.get(0).selectedIndex = 0;
                ddlStatus.get(0).selectedIndex = 0;
                ddlCompany.get(0).selectedIndex = 0;

                ddlRoute.trackChanges();
                for (var i = 0; i < ddlRoute.get_items().get_count(); i++) {
                    ddlRoute.get_items().getItem(i).set_checked(false);
                }
                ddlRoute.commitChanges();
            } catch (ex) {

            }
        }
        function clearShowAll() {
            var ddlSearch = $("#<%=ddlSearch.ClientID%>");
            var ddlServiceType = $("#<%=ddlServiceType.ClientID%>");
            var ddlType = $("#<%=ddlType.ClientID%>");
            var ddlStatus = $("#<%=rbStatus.ClientID%>");
            var txtSearch = $("#<%=txtSearch.ClientID%>");
            var ddlCompany = $("#<%=ddlCompany.ClientID%>");


            ddlServiceType.css("display", "none");
            ddlType.css("display", "none");
            ddlStatus.css("display", "none");
            ddlCompany.css("display", "none");
            txtSearch.css("display", "block");

            txtSearch.val('');

            try {
                ddlType.get(0).selectedIndex = 0;
                ddlServiceType.get(0).selectedIndex = 0;
                ddlStatus.get(0).selectedIndex = 0;
                ddlCompany.get(0).selectedIndex = 0;
            } catch (ex) {

            }
        }
    </script>


</asp:Content>
