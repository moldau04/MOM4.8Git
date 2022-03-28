<%@ Page Title="Recurring Contracts || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="RecContracts" CodeBehind="RecContracts.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
     <style type="text/css">
         @media only screen and (min-width: 250px) and (max-width: 700px){
         .btnlinksicon {
             float: none;
    padding-top: 11px;
    text-align: center;
         }
         .btnlinksa{
             margin-top:40px!important;
         }
         .pl-18 {
             padding-left: 0px;
            }
         }
     </style>
    <script type="text/javascript">

        function EscfactClickClick() {

            var value = document.getElementById("<%=txtMassUpdateEscFactor.ClientID %>").value;
            if (value > 0) {
                var count = document.getElementById('<%=hdnRenewRrcords.ClientID%>').value;

                if (confirm('Are you sure you want to mass update ' + count + ' records with the new Escalation Factor?')) {
                    return true;
                    function reloadEscalationgrid() { document.getElementById('<%=lnkRefreshEsc.ClientID%>').click; }
                }
                else {
                    return false;
                }
            }
            else if (value < 0) {
                document.getElementById("<%=txtMassUpdateEscFactor.ClientID %>").value = "";
                alert('Please enter a valid escalation factor. !!');
                return false;
            }
            else {
                document.getElementById("<%=txtMassUpdateEscFactor.ClientID %>").value = "";
                alert('Please Enter the value Escalation Factor in !!');
                return false;
            }
        }




    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField runat="server" ID="hdnRenewEsclateView" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEscalationContracts" Value="0" />

    <asp:HiddenField runat="server" ID="hdnTab" Value="0" />


    <telerik:RadAjaxManager ID="RadAjaxManagerRecContractEscalation" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkRefreshEsc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Escalation" LoadingPanelID="RadAjaxLoadingPanel_Escalation" />
                    <telerik:AjaxUpdatedControl ControlID="lblRenewRecord" />
                    <telerik:AjaxUpdatedControl ControlID="hdnRenewRrcords" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Escalation">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblRenewRecord" />
                    <telerik:AjaxUpdatedControl ControlID="hdnRenewRrcords" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkEscalate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Escalation" LoadingPanelID="RadAjaxLoadingPanel_Escalation" />
                    <telerik:AjaxUpdatedControl ControlID="lblRenewRecord" />
                    <telerik:AjaxUpdatedControl ControlID="hdnRenewRrcords" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkChk">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Escalation" LoadingPanelID="RadAjaxLoadingPanel_Escalation" />
                    <telerik:AjaxUpdatedControl ControlID="lblRenewRecord" />
                    <telerik:AjaxUpdatedControl ControlID="hdnRenewRrcords" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkcontractInactive">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_RecContracts" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_RecContracts" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowall">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_RecContracts" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="chkcontractInactive" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_RecContracts" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="chkcontractInactive" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_RecContracts">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_RecContracts" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" />
                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                    <telerik:AjaxUpdatedControl ControlID="chkcontractInactive" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSendExpirationEmails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSendExpirationEmails" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSend">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSend" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_RecContracts" runat="server">
    </telerik:RadAjaxLoadingPanel>

    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-notification-sync"></i>&nbsp;Recurring Contracts</div>

                                    <asp:Panel runat="server" ID="pnlGridButtons">
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
                                                        <asp:LinkButton ID="btnCopy" runat="server" OnClick="btnCopy_Click">Copy</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <ul id="dropdown2" class="dropdown-content">
                                                        <li>
                                                            <asp:LinkButton ID="lnkExcelAllContracts" runat="server" OnClick="lnkExcel_Click">All Contracts</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkExcelRenewEscalte" runat="server" OnClick="lnkExcelRenewEscalte_Click">Renew/Escalate</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkEquipmentContractByCustomer" runat="server" OnClick="lnkEquipmentContractByCustomer_Click" Visible="false">Equipment Contract by Customer Report</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dropdown2">Export to Excel
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <ul id="dropdown1" class="dropdown-content">
                                                        <li>
                                                            <a href="RecContractsModule_New.aspx?type=Recurring">Add New Recurring Report</a>
                                                        </li>
                                                        <li>
                                                            <a href="EscalationListingReport.aspx?type=Escalation">Add New Escalation Report</a>
                                                        </li>
                                                        <li>
                                                            <a href="MaintenanceCancelledReport.aspx?redirect=RecContracts.aspx">Maintenance Cancelled Report</a>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkMaintenanceEquipmentReport" runat="server" CausesValidation="true" OnClick="lnkMaintenanceEquipmentReport_Click" Enabled="true">Maintenance Equipment Details Report</asp:LinkButton>
                                                            <%--                                                            <a href="MaintenanceEquipmentCountByDate.aspx?redirect=RecContracts.aspx">Maintenance Equipment Count By Start Date Report</a>--%>
                                                        </li>
                                                        <li>
                                                            <a href="MonthlyRecurringHoursReport.aspx?redirect=RecContracts.aspx">Monthly Recurring Hours By Location Report</a>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkMonthlyRecurringHoursCategories" PostBackUrl="~/MonthlyRecurringHoursCategoriesReport.aspx?redirect=RecContracts.aspx" runat="server" Visible="false">Monthly Recurring Hours By Location Report with Categories</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <a href="MonthlyRecurringHoursByRouteReport.aspx?redirect=RecContracts.aspx">Monthly Recurring Hours By Route Report</a>
                                                        </li>
                                                        <li>
                                                            <a href="OpenMaintenanceByEquipmentReport.aspx?redirect=RecContracts.aspx">Open Maintenance by Equipment Report</a>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkServiceSalesCheckUpReport" runat="server" OnClick="lnkServiceSalesCheckUpReport_Click" Visible="false">Service Sales Check Up Report</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" href="RecContracts.aspx" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>
                                                </li>
                                            </ul>

                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkExpirationEmail" runat="server" ToolTip="Send email to customers for expiration contracts" OnClick="lnkExpirationEmail_Click">Expiration Email</asp:LinkButton>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="btnClose" ToolTip="Close" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12 right-details">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links">
                                    <li id="li1" runat="server"><a href="#accrdlogs">Email History Log</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="row">
            <div class="card">
                <div class="card-content">
                    <ul class="tabs tab-demo-active white">
                        <li class="tab col s2">
                            <a class="white-text waves-effect waves-light active" href="#activeone" id="tabContract"><i class="mdi-action-done"></i>&nbsp;All Contracts</a>
                        </li>
                        <li class="tab col s2">
                            <a class="white-text waves-effect waves-light" id="tabRenew" onclick="chkRenewEsclatePermission()" href="#two"><i class="mdi-notification-sync-problem"></i>&nbsp;Renew/Escalate</a>
                        </li>

                    </ul>

                    <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">
                        <div>
                            <div class="srchpaneinner p-3">
                                <div class="srchtitle  srchtitlecustomwidth">
                                    Search
                                </div>

                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="false" CssClass="browser-default selectsml selectst" onchange="SelectSearch()"
                                        OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                        <asp:ListItem Value="j.ctype">Service Type</asp:ListItem>
                                        <asp:ListItem Value="c.Status">Status</asp:ListItem>
                                        <asp:ListItem Value="c.bcycle">Billing Freqency</asp:ListItem>
                                        <asp:ListItem Value="c.scycle">Ticket Freqency</asp:ListItem>
                                        <asp:ListItem Value="r.name">Customer</asp:ListItem>
                                        <asp:ListItem Value="l.tag">Location</asp:ListItem>
                                        <asp:ListItem Value="B.Name">Company</asp:ListItem>
                                        <asp:ListItem Value="r.State">State</asp:ListItem>
                                        <asp:ListItem Value="j.SPHandle">Special Notes</asp:ListItem>

                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default selectst selectsml" Style="display: none">
                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="browser-default selectst selectsml" Style="display: none">
                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlBillFreq" runat="server" CssClass="browser-default selectst selectsml" Style="display: none">
                                        <asp:ListItem Value="0">Monthly</asp:ListItem>
                                        <asp:ListItem Value="1">Bi-Monthly</asp:ListItem>
                                        <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                        <asp:ListItem Value="3">3 Times/Year</asp:ListItem>
                                        <asp:ListItem Value="4">Semi-Anually</asp:ListItem>
                                        <asp:ListItem Value="5">Annually</asp:ListItem>
                                        <asp:ListItem Value="6">Never</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlTicketFreq" runat="server" CssClass="browser-default selectst selectsml" Style="display: none">
                                        <asp:ListItem Value="-1">Never</asp:ListItem>
                                        <asp:ListItem Value="0">Monthly</asp:ListItem>
                                        <asp:ListItem Value="1">Bi-Monthly</asp:ListItem>
                                        <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                        <asp:ListItem Value="15">3 Times/Year</asp:ListItem>
                                        <asp:ListItem Value="3">Semi-Annually </asp:ListItem>
                                        <asp:ListItem Value="4">Annually</asp:ListItem>
                                        <asp:ListItem Value="5">Weekly</asp:ListItem>
                                        <asp:ListItem Value="6">Bi-Weekly</asp:ListItem>
                                        <asp:ListItem Value="7">Every 13 Weeks</asp:ListItem>
                                        <asp:ListItem Value="10">Every 2 Years</asp:ListItem>
                                        <asp:ListItem Value="8">Every 3 Years</asp:ListItem>
                                        <asp:ListItem Value="9">Every 5 Years</asp:ListItem>
                                        <asp:ListItem Value="11">Every 7 Years</asp:ListItem>
                                        <asp:ListItem Value="12">On-Demand</asp:ListItem>
                                        <asp:ListItem Value="13">Daily</asp:ListItem>
                                        <asp:ListItem Value="14">Twice a Month</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default selectst selectsml" Style="display: none">
                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlRoute" runat="server" CssClass="browser-default selectst selectsml" Style="display: none">
                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..." Style="display: block"></asp:TextBox>
                                </div>


                                <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlSpecialNotes" Width="100px" Style="display: none" runat="server" CssClass="browser-default selectst selectsml"
                                        TabIndex="7" AutoPostBack="false">
                                        <asp:ListItem Value="-1">All</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="srchinputwrap srchclr btnlinksicon btnlinksa">
                                    <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" CausesValidation="false" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                                </div>
                                <div class="col lblsz2 lblszfloat">
                                    <div class="row">
                                        <span class="tro trost">
                                            <asp:CheckBox ID="chkcontractInactive" runat="server" OnCheckedChanged="chkcontractInactive_CheckedChanged" CssClass="css-checkbox" AutoPostBack="True" Text="Incl. Closed"></asp:CheckBox>
                                            <asp:Label ID="lblChkSelect" runat="server"></asp:Label>
                                        </span>
                                        <span class="tro trost">
                                            <a id="lnkClear" runat="server" onserverclick="lnkClear_Click" onclick="ClearSearch()">Clear </a>
                                        </span>
                                        <span class="tro trost">
                                            <a id="lnkShowAll" runat="server" onserverclick="lnkShowAll_Click" onclick="ClearSearch()">Show All </a>
                                        </span>
                                        <span class="tro trost">
                                            <%--   <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                                            <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                            <%-- </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                            <div class="grid_container">
                                <div class="form-section-row mb">


                                    <div class="RadGrid RadGrid_Material">
                                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                            <script type="text/javascript">
                                                function pageLoad() {
                                                    var grid = $find("<%= RadGrid_RecContracts.ClientID %>");
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
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_RecContracts" runat="server" LoadingPanelID="RadAjaxLoadingPanel_RecContracts" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                            <%--  <telerik:RadPersistenceManager ID="RadPersistenceRecContracts" runat="server">
                                                <PersistenceSettings>
                                                    <telerik:PersistenceSetting ControlID="RadGrid_RecContracts" />
                                                </PersistenceSettings>
                                            </telerik:RadPersistenceManager>--%>

                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_RecContracts" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" FilterType="CheckList"
                                                PagerStyle-AlwaysVisible="true"
                                                AllowCustomPaging="True"
                                                OnNeedDataSource="RadGrid_RecContracts_NeedDataSource"
                                                OnPreRender="RadGrid_RecContracts_PreRender"
                                                OnItemEvent="RadGrid_RecContracts_ItemEvent"
                                                OnExcelMLExportRowCreated="RadGrid_RecContracts_ExcelMLExportRowCreated"
                                                OnItemCreated="RadGrid_RecContracts_ItemCreated"
                                                OnPageSizeChanged="RadGrid_RecContracts_PageSizeChanged"
                                                OnPageIndexChanged="RadGrid_RecContracts_PageIndexChanged">
                                                <CommandItemStyle />
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                </ClientSettings>
                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" ShowHeadersWhenNoRecords="true">
                                                    <Columns>
                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                        </telerik:GridClientSelectColumn>

                                                        <telerik:GridTemplateColumn UniqueName="Job" DataField="Job" HeaderText="Contract #" SortExpression="Job"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Int32"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("Job") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                            </FooterTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn UniqueName="locid" DataField="locid" HeaderText="Acct #" SortExpression="locid"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String"
                                                            ShowFilterIcon="false" HeaderStyle-Width="150">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn UniqueName="Tag" DataField="Tag" SortExpression="Tag" AutoPostBackOnFilter="true"
                                                            CurrentFilterFunction="Contains" HeaderText="Location Name" ShowFilterIcon="false" DataType="System.String" HeaderStyle-Width="150">
                                                            <ItemTemplate>
                                                                <img id="imgCreditH" runat="server" visible='<%# (Eval("credit").ToString() == "1")?true:false %>' title="Credit Hold" src="images/MSCreditHold.png" style="float: left; width: 16px; background-color: rgba(255, 0, 0, 0.34)">
                                                                <asp:HyperLink ID="lblLocName" runat="server"><%#Eval("Tag")%></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                     

                                                        <telerik:GridBoundColumn UniqueName="Name" DataField="Name" HeaderText="Customer Name" SortExpression="Name"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String"
                                                            ShowFilterIcon="false" HeaderStyle-Width="200">
                                                        </telerik:GridBoundColumn>

                                                    
                                                       


                                                        <telerik:GridBoundColumn UniqueName="Company" DataField="Company" HeaderText="Company" SortExpression="Company"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="OriginalContract1" DataField="OriginalContract" HeaderText="Original Contract" SortExpression="OriginalContract"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}"
                                                            ShowFilterIcon="false" HeaderStyle-Width="200">
                                                         </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="CType" DataField="CType" HeaderText="Service Type" SortExpression="CType"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String"
                                                            ShowFilterIcon="false" HeaderStyle-Width="110">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="fdesc" DataField="fdesc" HeaderText="Description" SortExpression="fdesc"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String"
                                                            ShowFilterIcon="false" HeaderStyle-Width="150">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn UniqueName="MonthlyBill" DataField="MonthlyBill" DataType="System.Decimal" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                                            SortExpression="MonthlyBill" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Mon. Billing" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMonthlyBill" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MonthlyBill", "{0:c}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn UniqueName="Freqency" DataField="Freqency" HeaderText="Billing Frequency" SortExpression="Freqency"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataType="System.String"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn UniqueName="BAmt" DataField="BAmt" DataType="System.Decimal" FooterAggregateFormatString="{0:c}" Aggregate="Sum"
                                                            SortExpression="BAmt" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Total Period Billing" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotPerBill" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BAmt", "{0:c}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn UniqueName="MonthlyHours" DataField="MonthlyHours" DataType="System.Decimal" FooterAggregateFormatString="{0:n}" Aggregate="Sum"
                                                            SortExpression="MonthlyHours" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Monthly Hours" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMonthlyHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MonthlyHours", "{0:n}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn UniqueName="TicketFreq" DataField="TicketFreq" DataType="System.String" HeaderText="Ticket Frequency" SortExpression="TicketFreq"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn UniqueName="Hours" DataField="Hours" DataType="System.Decimal" FooterAggregateFormatString="{0:n}" Aggregate="Sum"
                                                            SortExpression="Hours" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Hours" ShowFilterIcon="false" HeaderStyle-Width="70">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotPeriodHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Hours", "{0:n}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn UniqueName="Worker" DataField="Worker" DataType="System.String" SortExpression="Worker"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="Status" DataField="Status" DataType="System.String" HeaderText="Status" SortExpression="Status"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" HeaderStyle-Width="60">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="SREMARKS" DataField="SREMARKS" DataType="System.String" HeaderText="Special Notes" SortExpression="SREMARKS"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" HeaderStyle-Width="250">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="Salesperson" DataField="Salesperson" DataType="System.String" HeaderText="Salesperson" SortExpression="Salesperson"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="true"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="Salesperson2" DataField="Salesperson2" DataType="System.String" HeaderText="Salesperson 2" SortExpression="Salesperson2"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="true"
                                                            ShowFilterIcon="false" HeaderStyle-Width="110">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="ElevCount" DataField="ElevCount" DataType="System.Int32" FooterAggregateFormatString="{0}" Aggregate="Sum"
                                                            HeaderText="Equipment " SortExpression="ElevCount"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" Visible="true"
                                                            ShowFilterIcon="false" HeaderStyle-Width="110">
                                                        </telerik:GridBoundColumn>

                                                    <%--Start: For Accredited--%>
                                                        <telerik:GridBoundColumn UniqueName="OriginalContract" DataField="OriginalContract" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Original Contract" SortExpression="OriginalContract"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="LastRenew" DataField="LastRenew" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Last Renew" SortExpression="LastRenew"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="ExpirationType" DataField="ExpirationType" DataType="System.String" HeaderText="Expiration Type" SortExpression="ExpirationType"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Expiration Date" SortExpression="ExpirationDate"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BLenght" DataField="BLenght" DataType="System.String" HeaderText="Contract Length" SortExpression="BLenght"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BStart" DataField="BStart" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Billing Date" SortExpression="BStart"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="SStart" DataField="SStart" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Schedule Start Date" SortExpression="SStart"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="ScheduledTime" DataField="ScheduledTime" DataType="System.String" HeaderText="Scheduled Time" SortExpression="ScheduledTime"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RenewalNotes" DataField="RenewalNotes" DataType="System.String" HeaderText="Renewal Notes" SortExpression="RenewalNotes"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BillingRate" DataField="BillingRate" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Billing Rate" SortExpression="BillingRate"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateOT" DataField="RateOT" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate OT" SortExpression="RateOT"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateNT" DataField="RateNT" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate NT" SortExpression="RateNT"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateMileage" DataField="RateMileage" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate Mileage" SortExpression="RateMileage"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateDT" DataField="RateDT" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate DT" SortExpression="RateDT"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateTravel" DataField="RateTravel" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate Travel" SortExpression="RateTravel"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="EscalationType" DataField="EscalationType" DataType="System.String" HeaderText="Escalation Type" SortExpression="EscalationType"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BEscCycle" DataField="BEscCycle" DataType="System.String" HeaderText="Escalation Cycle" SortExpression="BEscCycle"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BEscFact" DataField="BEscFact" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Escalation Factor" SortExpression="BEscFact"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="EscLast" DataField="EscLast" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Escalated Last" SortExpression="EscLast"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>
                                                        <%--End: For Accredited--%>
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
                    <div id="two" class="col s12 tab-container-border lighten-4">

                        <div id="divRevnewEsc" runat="server">
                            <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                            <div class="srchpaneinner pt-3">
                                <div class="srchtitle  srchtitlecustomwidth pad-le-15">
                                    Escalation Prior to
                                </div>

                                <div class="srchinputwrap pd-negatenw input-field pl-18">
                                    <asp:TextBox ID="txtEscDate" runat="server" CssClass="srchcstm"></asp:TextBox>
                                </div>

                                <div class="srchinputwrap pd-negatenw">
                                    <div class="btnlinksicon ser-css1">
                                        <asp:LinkButton ID="lnkRefreshEsc" runat="server" OnClick="lnkRefreshEsc_Click" CausesValidation="false" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                                    </div>
                                </div>

                                <div id="divRevnew" runat="server">
                                    <div class="srchtitle  srchtitlecustomwidth pad-le-15">
                                        Select Date
                                    </div>
                                    <div class="srchinputwrap pd-negatenw input-field">
                                        <asp:TextBox ID="txtNextEscdate" runat="server" CssClass="srchcstm"></asp:TextBox>
                                    </div>
                                    <div class="srchtitle  srchtitlecustomwidth btnlinks btnlinksa">
                                        <a id="lnkEscalate" runat="server" validationgroup="esc"  onserverclick="lnkEscalate_Click">Renew/Escalate</a>
                                    </div>

                                </div>



                                <div class="col lblsz2 lblszfloat">
                                    <div class="row">
                                        <span class="tro trost">
                                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="false"></asp:CheckBox>
                                            <asp:Label ID="lblChkEsc" runat="server">Incl. Closed</asp:Label>
                                        </span>
                                        <span class="tro trost">
                                            <asp:Label ID="lblRenewRecord" runat="server">0 Record(s) found</asp:Label>
                                            <asp:HiddenField runat="server" ID="hdnRenewRrcords" />

                                        </span>
                                    </div>
                                </div>

                            </div>


                            <div class="col lblsz2 lblszfloat mb-5">
                                <div class="row">
                                </div>
                            </div>
                            <div class="grid_container">
                                <div class="form-section-row mp">
                                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Escalation" runat="server">
                                    </telerik:RadAjaxLoadingPanel>

                                    <div class="RadGrid RadGrid_Material">
                                        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                            <script type="text/javascript">
                                                function pageLoad() {
                                                    var grid = $find("<%= RadGrid_Escalation.ClientID %>");
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

                                                ///////////// Select all checkbox ////////////////////
                                                function checkAllChecBox() {
                                                    var checked = $(".chkSelectAll input").is(":checked");
                                                    if (checked) {
                                                        $(".chkSelect input").prop("checked", true)

                                                    }
                                                    else {
                                                        $(".chkSelect input").prop("checked", false)

                                                    }
                                                }

                                                ///////////// Unselect all checkbox ////////////////////
                                                function unCheckSelectAll() {
                                                    var checked = $(".chkSelect input").is(":checked");
                                                    var checkedAll = $(".chkSelectAll input").is(":checked");
                                                    var checkCountCheckbox = $(".chkSelect input:checked").length;
                                                    var checkCountCheckboxSelected = $(".chkSelect input").length
                                                    if (checked && checkedAll) {
                                                        $(".chkSelectAll input").prop("checked", false);

                                                    }

                                                    if (checkCountCheckbox === checkCountCheckboxSelected) {
                                                        $(".chkSelectAll input").prop("checked", true);

                                                    }

                                                }

                                                ///////////// Hide select all checkbox ////////////////////
                                                function hideSelectAllChkb() {
                                                    $(".chkSelectAll").hide();
                                                }
                                                ///////////// Show select all checkbox ////////////////////
                                                function showSelectAllChkb() {
                                                    $(".chkSelectAll").show();
                                                }


                                            </script>
                                        </telerik:RadCodeBlock>
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Escalation" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Escalation" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                            <div class="modal fade mode-fade-css" id="exampleModalLong" style="height: 125px !important; padding: 0px; width: 35%!important;" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
                                                <div class="modal-dialog" role="document">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h5 class="modal-title" id="exampleModalLongTitle">Mass Update Esc Factor</h5>
                                                        </div>
                                                        <div class="modal-body">
                                                            <div id="divescfact" runat="server">

                                                                <div class="srchtitle  srchtitlecustomwidth" style="padding-left: 5px;">
                                                                    Escalation Factor in %
                                                                </div>
                                                                <div class="srchinputwrap pd-negatenw input-field" style="margin-left: 25px;">
                                                                    <asp:TextBox ID="txtMassUpdateEscFactor" TextMode="Number" runat="server" CssClass="srchcstm" Text=""></asp:TextBox>
                                                                </div>
                                                                <div class="srchtitle  srchtitlecustomwidth btnlinks">

                                                                    <asp:LinkButton runat="server" ID="lnkESCfact" OnClientClick="return EscfactClickClick()" OnClick="lnkESCfact_ServerClick">Update</asp:LinkButton>

                                                                </div>

                                                                <div class="srchtitle  srchtitlecustomwidth btnlinks">
                                                                    <a id="A1" data-dismiss="modal">Cancel</a>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Escalation" AllowFilteringByColumn="true" ShowFooter="True" PageSize="1000"
                                                ShowStatusBar="true" ShowGroupPanel="false" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" FilterType="CheckList" PagerStyle-AlwaysVisible="true"
                                                AllowCustomPaging="false"
                                                OnNeedDataSource="RadGrid_Escalation_NeedDataSource"
                                                OnPreRender="RadGrid_Escalation_PreRender"
                                                OnExcelMLExportRowCreated="RadGrid_Escalation_ExcelMLExportRowCreated"
                                                OnItemCreated="RadGrid_Escalation_ItemCreated"
                                                OnPageSizeChanged="RadGrid_Escalation_PageSizeChanged"
                                                OnPageIndexChanged="RadGrid_Escalation_PageIndexChanged">
                                                <CommandItemStyle />
                                                <GroupingSettings CaseSensitive="false" />

                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="True"></Selecting>

                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                </ClientSettings>
                                                <MasterTableView AutoGenerateColumns="false" AllowPaging="true" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" ShowHeadersWhenNoRecords="true">

                                                    <Columns>
                                                        <%--     <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                        </telerik:GridClientSelectColumn>--%>

                                                        <telerik:GridTemplateColumn HeaderStyle-Width="30" AllowFiltering="false" ShowFilterIcon="false">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" CssClass="chkSelect" runat="server" onchange="unCheckSelectAll();" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" CssClass="chkSelectAll" onchange="checkAllChecBox();" runat="server" />
                                                            </HeaderTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="Job" HeaderText="Contract #" SortExpression="Job" HeaderStyle-Width="100" DataType="System.String"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" UniqueName="Job">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" Style="display: none" runat="server" Text='<%# Bind("Job") %>'></asp:Label>
                                                                <asp:HyperLink ID="lnkJob" runat="server" Text='<%# Bind("job") %>'></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                            </FooterTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn DataField="locid" HeaderText="Location ID" HeaderStyle-Width="140"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="locid"
                                                            ShowFilterIcon="false" UniqueName="locid" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn DataField="Tag" HeaderText="Location Name" HeaderStyle-Width="200"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Tag"
                                                            ShowFilterIcon="false" UniqueName="Tag" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <%--                                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" HeaderStyle-Width="140"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Tag"
                                                            ShowFilterIcon="false" UniqueName="Company" DataType="System.String">
                                                        </telerik:GridBoundColumn>--%>

                                                        <telerik:GridBoundColumn DataField="CType" HeaderText="Service Type" HeaderStyle-Width="120"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CType"
                                                            ShowFilterIcon="false" UniqueName="CType" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn DataField="fdesc" HeaderText="Description" HeaderStyle-Width="110"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fdesc"
                                                            ShowFilterIcon="false" UniqueName="fdesc" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn DataField="Freqency" HeaderText="Billing Frequency" HeaderStyle-Width="140"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Freqency"
                                                            ShowFilterIcon="false" UniqueName="Freqency" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn DataField="Action" HeaderText="Action" HeaderStyle-Width="120"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Action"
                                                            ShowFilterIcon="false" UniqueName="Action" DataType="System.String">
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridTemplateColumn DataField="ExpirationDate" AllowFiltering="true" AllowSorting="true" SortExpression="ExpirationDate" AutoPostBackOnFilter="true" HeaderStyle-Width="140" DataType="System.String"
                                                            CurrentFilterFunction="Contains" HeaderText="Expiration" ShowFilterIcon="false" UniqueName="ExpirationDate">
                                                            <ItemTemplate>
                                                                <asp:Label Style='<%# (Eval("ExpirationDate", "{0:MM/dd/yyyy}")!="01/01/1900") ?( string.Format("color:{0}",Convert.ToDateTime(Eval("ExpirationDate").ToString())<= System.DateTime.Now ? "RED": "BLACK")):"" %>'
                                                                    ID="lblExpirationdt" runat="server"><%# (Eval("ExpirationDate", "{0:MM/dd/yyyy}")=="01/01/1900") ? "Indefinitely" : Eval("ExpirationDate", "{0:MM/dd/yyyy}") %></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn DataField="BEscCycle" HeaderText="Esc Cycle" HeaderStyle-Width="120"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" UniqueName="BEscCycle" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn DataField="EscType" HeaderText="Esc Type" HeaderStyle-Width="120"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" UniqueName="EscType" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn DataField="BEscFact" HeaderText="Esc Fact" HeaderStyle-Width="100"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" UniqueName="BEscFact" DataType="System.String">
                                                        </telerik:GridBoundColumn>

                                                        <%--                                                        <telerik:GridTemplateColumn HeaderStyle-Width="100" AllowFiltering="true" ShowFilterIcon="false" DataField="BEscFact" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            UniqueName="BEscFact">
                                                            <ItemTemplate>
                                                                <%# Eval("BEscFact") %>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <!-- Button trigger modal -->
                                                                <a style="cursor: pointer;" data-toggle="modal" data-target="#exampleModalLong" title="Click here to sort" onclick="Clear()">Esc Fact
                                                                </a>
                                                                <!-- Modal -->
                                                            </HeaderTemplate>
                                                        </telerik:GridTemplateColumn>--%>

                                                        <telerik:GridTemplateColumn DataField="EscLast" SortExpression="EscLast" AllowFiltering="true" AutoPostBackOnFilter="true" HeaderStyle-Width="140" DataType="System.String"
                                                            CurrentFilterFunction="Contains" HeaderText="Last Esc" ShowFilterIcon="false" UniqueName="EscLast">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExp" runat="server"><%# Eval("EscLast", "{0:MM/dd/yyyy}") %></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="nextdue" SortExpression="nextdue" AllowFiltering="true" AutoPostBackOnFilter="true" HeaderStyle-Width="140" DataType="System.String"
                                                            CurrentFilterFunction="Contains" HeaderText="Next Esc Due" ShowFilterIcon="false" UniqueName="nextdue">
                                                            <ItemTemplate>
                                                                <asp:Label Style='<%#  string.Format("color:{0}",Convert.ToDateTime(Eval("nextdue").ToString())<= System.DateTime.Now ? "RED": "BLACK") %>'
                                                                    ID="lblExpnext" runat="server"><%# Eval("nextdue", "{0:MM/dd/yyyy}") %></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="ContractDate" SortExpression="ContractDate" AllowFiltering="true" AutoPostBackOnFilter="true" HeaderStyle-Width="140" DataType="System.String"
                                                            CurrentFilterFunction="Contains" HeaderText="Contract Date" ShowFilterIcon="false" UniqueName="ContractDate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractDate" runat="server"><%# Eval("ContractDate", "{0:MM/dd/yyyy}") %></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="LastContract" AllowFiltering="true" AllowSorting="true" SortExpression="LastContract" AutoPostBackOnFilter="true" HeaderStyle-Width="140" DataType="System.String"
                                                            CurrentFilterFunction="Contains" HeaderText="Last Contract" ShowFilterIcon="false" UniqueName="LastContract">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLastContract" runat="server"><%# Eval("LastContract", "{0:MM/dd/yyyy}") %></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                       
                                                        <telerik:GridTemplateColumn DataField="BAmt" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="BAmt" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                            HeaderText="Current Amount" HeaderStyle-Width="130" ShowFilterIcon="false" UniqueName="BAmt" DataType="System.Decimal">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBamt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BAmt", "{0:c}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="newamt" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="newamt" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                                            HeaderText="New Amount" HeaderStyle-Width="110" ShowFilterIcon="false" UniqueName="newamt" DataType="System.Decimal">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBamtNew" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "newamt", "{0:c}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="RenewalNotes" HeaderText="Renewal Notes" SortExpression="RenewalNotes" HeaderStyle-Width="140"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" UniqueName="RenewalNotes" DataType="System.String">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRenewDesc" Width="140px" Style="word-wrap: break-word; overflow-y: scroll; max-height: 100px" runat="server" Text='<%# Eval("RenewalNotes")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="Salesperson" HeaderText="Salesperson" SortExpression="Salesperson" HeaderStyle-Width="140"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" UniqueName="Salesperson" DataType="System.String">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSalesperson" runat="server" Text='<%# Eval("Salesperson")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="Salesperson2" HeaderText="Salesperson2" SortExpression="Salesperson2" HeaderStyle-Width="140"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" UniqueName="Salesperson2" DataType="System.String">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSalesperson2" runat="server" Text='<%# Eval("Salesperson2")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn DataField="Customer" DataType="System.String" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="Customer" ShowFilterIcon="false" Visible="false" HeaderText="Customer Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("Customer") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="Address" DataType="System.String" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="Address" ShowFilterIcon="false" Visible="false" HeaderText="Address">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="City" DataType="System.String" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="City" ShowFilterIcon="false" Visible="false" HeaderText="City">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="State" DataType="System.String" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="State" ShowFilterIcon="false" Visible="false" HeaderText="State">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblState" runat="server" Text='<%#Eval("State") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="Zip" DataType="System.String" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="Zip" ShowFilterIcon="false" Visible="false" HeaderText="Zip">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZip" runat="server" Text='<%#Eval("Zip") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn DataField="LocationCompanyName" DataType="System.String" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="LocationCompanyName" ShowFilterIcon="false" Visible="false" HeaderText="Location Company Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLocationCompanyName" runat="server" Text='<%#Eval("LocationCompanyName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <%--Start: For Accredited--%>
                                                        <telerik:GridBoundColumn UniqueName="Hours" DataField="Hours" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Hours" SortExpression="Hours"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="70">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="ScheduleFrequency" DataField="ScheduleFrequency" DataType="System.String" HeaderText="ScheduleFrequency" SortExpression="ScheduleFrequency"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="OriginalContract" DataField="OriginalContract" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Original Contract" SortExpression="OriginalContract"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="LastRenew" DataField="LastRenew" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Last Renew" SortExpression="LastRenew"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="ExpirationType" DataField="ExpirationType" DataType="System.String" HeaderText="Expiration Type" SortExpression="ExpirationType"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BLenght" DataField="BLenght" DataType="System.String" HeaderText="Contract Length" SortExpression="BLenght"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BStart" DataField="BStart" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Billing Date" SortExpression="BStart"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="SStart" DataField="SStart" DataType="System.DateTime" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Schedule Start Date" SortExpression="SStart"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="ScheduledTime" DataField="ScheduledTime" DataType="System.String" HeaderText="Scheduled Time" SortExpression="ScheduledTime"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="BillingRate" DataField="BillingRate" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Billing Rate" SortExpression="BillingRate"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateOT" DataField="RateOT" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate OT" SortExpression="RateOT"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateNT" DataField="RateNT" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate NT" SortExpression="RateNT"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateMileage" DataField="RateMileage" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate Mileage" SortExpression="RateMileage"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateDT" DataField="RateDT" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate DT" SortExpression="RateDT"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn UniqueName="RateTravel" DataField="RateTravel" DataType="System.Decimal" DataFormatString="{0:n}" HeaderText="Rate Travel" SortExpression="RateTravel"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" Visible="false"
                                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                                        </telerik:GridBoundColumn>

                                                        <%--End: For Accredited--%>
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

                </div>
            </div>
        </div>
        <%--Email Sending Logs--%>
        <div class="accordian-wrap">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li id="tbLogs" runat="server" style="display: block">
                            <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Email History Log</div>

                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoadLog() {
                                                                try {
                                                                    var grid = $find("<%= RadGrid_gvLogs.ClientID %>");
                                                                    var columns = grid.get_masterTableView().get_columns();
                                                                    for (var i = 0; i < columns.length; i++) {
                                                                        columns[i].resizeToFit(false, true);
                                                                    }
                                                                } catch (e) {

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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvLogs_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldate" runat="server" Text='<%# Eval("EmailDate", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltime" runat="server" Text='<%# Eval("EmailDate","{0: hh:mm tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Username" SortExpression="Username" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Contract #" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref").ToString() == "0" ? "" : Eval("Ref").ToString() %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="EmailFunction" SortExpression="EmailFunction" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Function" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmailFunction" runat="server" Text='<%# Eval("EmailFunction") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="MailTo" SortExpression="MailTo" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Mail To" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmailTo" runat="server" Text='<%# Eval("MailTo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="UsrErrMessage" SortExpression="UsrErrMessage" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Error Message" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUsrErrMessage" runat="server" Text='<%# Eval("UsrErrMessage") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </telerik:RadAjaxPanel>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <%--<div class="row">
        <asp:Button runat="server" ID="hiddenTargetControlForOpenEmailModalPopup" Style="display: none" CausesValidation="False" />
        
        <asp:ModalPopupExtender runat="server" ID="ModalPopupOpenEmail" BehaviorID="OpenEmailBehavior" TargetControlID="hiddenTargetControlForOpenEmailModalPopup"
            PopupControlID="pnlOpenEmailPop" RepositionMode="RepositionOnWindowResizeAndScroll">
        </asp:ModalPopupExtender>
    </div>--%>
    <telerik:RadWindow ID="RadWindowEmail" Skin="Material" VisibleTitlebar="true" Title="Email" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
        runat="server" Modal="true" Width="850" Height="500">
        <ContentTemplate>

            <div class="col-lg-12 col-md-12">
                <div class="col-md-8 col-lg-8">
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Subject</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" ToolTip="Subject" Placeholder="Subject"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-col">
                        <div class="fc-label">
                            <label>Body</label>
                        </div>
                        <div class="fc-input">
                            <%--<asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Columns="50"
                                Rows="10" CssClass="form-control txtBody" Height="250" Width="817"></asp:TextBox>--%>
                            <%--<asp:HtmlEditorExtender ID="htmlEditorExtender1" TargetControlID="txtBody"
                                            runat="server" EnableSanitization="False" Enabled="True">
                                        </asp:HtmlEditorExtender>--%>
                            <CKEditor:CKEditorControl ID="txtBody" runat="server" Width="100%" Height="200" Toolbar="Full"
                                BasePath="js/ckeditor" TemplatesFiles="js/ckeditor/plugins/templates/templates/default.js"
                                ContentsCss="js/ckeditor/contents.css" FilebrowserImageUploadUrl="CKimageupload.ashx"
                                ExtraPlugins="imagepaste,preventdrop">
                                            Thomas Text
                            </CKEditor:CKEditorControl>
                        </div>
                    </div>
                </div>
            </div>
            <div style="clear: both;"></div>
            <div class="btnlinks">
                <asp:LinkButton ID="lnkSend" runat="server" OnClick="lnkSend_Click">Send</asp:LinkButton>
            </div>
        </ContentTemplate>

    </telerik:RadWindow>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script>
        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtExpirationDate'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                //maxDate: new Date(2020, 12, 31),
                yearRange: [2000, 2050]
            });
        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtEscDate'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                //maxDate: new Date(2020, 12, 31)//,
                yearRange: [2000, 2050]
            });
        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtSetExpiration'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                //maxDate: new Date(2020, 12, 31)//,
                yearRange: [2000, 2050]
            });
        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtNextEscdate'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                //maxDate: new Date(2020, 12, 31)//,
                yearRange: [2000, 2050]
            });
    </script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            var TabValue = document.getElementById('<%= hdnTab.ClientID%>').value;
            if (TabValue == "2") {

                document.getElementById('tabRenew').click();
            }
            else {
                document.getElementById('tabContract').click();
            }


            $('#colorNav #dropdown1 li').remove();
            $(reports).each(function (index, report) {

                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }

                if (report.ReportType == 'Recurring') {
                    $('#dropdown1').append('<li><a href="RecContractsModule.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Recurring"><img src=images/reportfolder.png> ' + report.ReportName + '</a></li>')
                }
                else if (report.ReportType == 'Escalation') {
                    $('#dropdown1').append('<li><a href="EscalationListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Escalation"><img src=images/reportfolder.png> ' + report.ReportName + '</a></li>')
                }

            });

            $('a[href^="#accrd"]').on('click', function (e) {
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

        });
        function chkRenewEsclatePermission(hyperlink) {
            var IsView = document.getElementById('<%= hdnRenewEsclateView.ClientID%>').value;
            if (IsView == "N") {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }

        }

        function Clear() {
            document.getElementById("<%=txtMassUpdateEscFactor.ClientID %>").value = "";

        }
        function SelectSearch() {
            debugger;
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var ddlServiceType = $('#<%=ddlServiceType.ClientID%>');
            var rbStatus = $('#<%=rbStatus.ClientID%>');
            var ddlBillFreq = $('#<%=ddlBillFreq.ClientID%>');
            var ddlTicketFreq = $('#<%=ddlTicketFreq.ClientID%>');
            var txtSearch = $('#<%=txtSearch.ClientID%>');
            var ddlRoute = $('#<%=ddlRoute.ClientID%>');
            var ddlSpecialNotes = $('#<%=ddlSpecialNotes.ClientID%>');

            txtSearch.css("display", "none");
            ddlServiceType.css("display", "none");
            rbStatus.css("display", "none");
            ddlBillFreq.css("display", "none");
            ddlTicketFreq.css("display", "none");
            ddlRoute.css("display", "none");
            ddlSpecialNotes.css("display", "none");

            txtSearch.val('');



            try {

                ddlServiceType.get(0).selectedIndex = 0;
                rbStatus.get(0).selectedIndex = 0;
                ddlBillFreq.get(0).selectedIndex = 0;
                ddlTicketFreq.get(0).selectedIndex = 0;
                ddlRoute.get(0).selectedIndex = 0;
                ddlSpecialNotes.get(0).selectedIndex = 0;

            } catch (ex) {

            }


            if (ddlSearch.val() === "j.ctype") {

                ddlServiceType.css("display", "block");
            }
            else if (ddlSearch.val() == "c.Status") {
                rbStatus.css("display", "block");
            }
            else if (ddlSearch.val() == "c.bcycle") {

                ddlBillFreq.css("display", "block");
            }
            else if (ddlSearch.val() == "c.scycle") {

                ddlTicketFreq.css("display", "block");
            }
            else if (ddlSearch.val() == "j.custom20") {

                ddlRoute.css("display", "block");
            }
            else if (ddlSearch.val() == "j.SPHandle") {

                ddlSpecialNotes.css("display", "block");
            }
            else {

                txtSearch.css("display", "block");
            }
        }

        function ClearSearch() {

            debugger;

            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var ddlServiceType = $('#<%=ddlServiceType.ClientID%>');
            var rbStatus = $('#<%=rbStatus.ClientID%>');
            var ddlBillFreq = $('#<%=ddlBillFreq.ClientID%>');
            var ddlTicketFreq = $('#<%=ddlTicketFreq.ClientID%>');
            var txtSearch = $('#<%=txtSearch.ClientID%>');
            var ddlRoute = $('#<%=ddlRoute.ClientID%>');
            var ddlSpecialNotes = $('#<%=ddlSpecialNotes.ClientID%>');

            txtSearch.css("display", "block");
            ddlServiceType.css("display", "none");
            rbStatus.css("display", "none");
            ddlBillFreq.css("display", "none");
            ddlTicketFreq.css("display", "none");
            ddlRoute.css("display", "none");
            ddlSpecialNotes.css("display", "none");
            txtSearch.val('');

            try {

                ddlSearch.get(0).selectedIndex = 0;
                ddlServiceType.get(0).selectedIndex = 0;
                rbStatus.get(0).selectedIndex = 0;
                ddlBillFreq.get(0).selectedIndex = 0;
                ddlTicketFreq.get(0).selectedIndex = 0;
                ddlRoute.get(0).selectedIndex = 0;
                ddlSpecialNotes.get(0).selectedIndex = 0;

            } catch (ex) {

            }


        }

        function ShowEmailWindow() {
            var wnd = $find('<%=RadWindowEmail.ClientID %>');
            wnd.Show();

            Materialize.updateTextFields();
        }
    </script>
</asp:Content>
