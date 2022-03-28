<%@ Page Title="Leads || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Prospects" CodeBehind="Prospects.aspx.cs" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />

    <script type="text/javascript">

        function CheckDelete() {
            var result = false;
            $("#<%=RadGrid_Pro.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this Lead ?');
            }
            else {
                alert('Please select a Lead to delete.')
                return false;
            }
        }

        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
            $(reports).each(function (index, report) {
                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }

                $('#dynamicUI').append('<li><a href="LeadListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Lead"><span><i class="fa fa-book" Style="color:Blue;font-size:16px;" aria-hidden="true"></i> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

            });

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

    <style>
        .RadComboBox_Bootstrap {
            width: 4.2em !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Pro" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Pro" LoadingPanelID="RadAjaxLoadingPanel_Pro" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Pro_TotalRecords" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Pro" LoadingPanelID="RadAjaxLoadingPanel_Pro" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Pro_TotalRecords" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Pro_SearchPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkClear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Pro" LoadingPanelID="RadAjaxLoadingPanel_Pro" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Pro_TotalRecords" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Pro_SearchPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkChk">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Pro" LoadingPanelID="RadAjaxLoadingPanel_Pro" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Pro_TotalRecords" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Pro">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Pro" />
                    <telerik:AjaxUpdatedControl ControlID="RadAP_Pro_TotalRecords" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Pro" runat="server">
    </telerik:RadAjaxLoadingPanel>
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
                                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Leads</asp:Label>
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton CssClass="icon-addnew" ID="lnkAdd" runat="server" CausesValidation="False" OnClick="lnkAdd_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkCopy" runat="server" CausesValidation="False" OnClick="lnkCopy_Click">Copy</asp:LinkButton>
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
                                                            <a href="LeadListingReport.aspx?type=Lead" class="-text">Add New Report</a>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkListOFLeadsreport" OnClick="lnkListOFLeadsreport_Click" runat="server">List Of Leads</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkListOfLeadsByContactReport" OnClick="lnkListOfLeadsByContactReport_Click" runat="server">Leads by Contact</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" id="lnkReport" runat="server" data-beloworigin="true" href="LocationReport.aspx" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false"
                                                OnClick="lnkClose_Click">  <i class="mdi-content-clear"></i></asp:LinkButton>
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
                <telerik:RadAjaxPanel runat="server" ID="RadAP_Pro_SearchPanel">
                    <%--<asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                    <div class="srchtitle srchtitlecustomwidth ser-css2 " >
                        Search
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                            CssClass="browser-default selectst selectsml">
                            <asp:ListItem Value=" ">Select</asp:ListItem>
                            <asp:ListItem Value="r.name">Location Lead Name</asp:ListItem>
                            <asp:ListItem Value="p.CustomerName">Customer Lead Name</asp:ListItem>
                            <asp:ListItem Value="p.terr">Salesperson</asp:ListItem>
                            <asp:ListItem Value="r.address">Address</asp:ListItem>
                            <asp:ListItem Value="r.city">City</asp:ListItem>
                            <asp:ListItem Value="r.state">State</asp:ListItem>
                            <asp:ListItem Value="p.type">Type</asp:ListItem>
                            <asp:ListItem Value="p.status">Status</asp:ListItem>
                            <asp:ListItem Value="days">Days</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlcompare" runat="server" Visible="false" CssClass="browser-default selectst " Style="width: 50px;">
                            <asp:ListItem Value="0">=</asp:ListItem>
                            <asp:ListItem Value="1">&#62;=</asp:ListItem>
                            <asp:ListItem Value="2">&#60;=</asp:ListItem>
                            <asp:ListItem Value="3">&#62;</asp:ListItem>
                            <asp:ListItem Value="4">&#60;</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlTypeSearch" runat="server" CssClass="browser-default selectst selectsml" Visible="False">
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlStatusSearch" runat="server" CssClass="browser-default selectst selectsml" Visible="False">
                            <asp:ListItem Value="0">Active</asp:ListItem>
                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlStateSearch" runat="server" CssClass="browser-default selectst selectsml"
                            ToolTip="State" Visible="False">
                            <asp:ListItem Value="State">State</asp:ListItem>
                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                            <asp:ListItem Value="CA">California</asp:ListItem>
                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
                            <asp:ListItem Value="FL">Florida</asp:ListItem>
                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                            <asp:ListItem Value="ME">Maine</asp:ListItem>
                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
                            <asp:ListItem Value="MT">Montana</asp:ListItem>
                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                            <asp:ListItem Value="NY">New York</asp:ListItem>
                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                            <asp:ListItem Value="TX">Texas</asp:ListItem>
                            <asp:ListItem Value="UT">Utah</asp:ListItem>
                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
                            <asp:ListItem Value="WA">Washington</asp:ListItem>
                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                            <asp:ListItem Value="AB">Alberta</asp:ListItem>
                            <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                            <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                            <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                            <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                            <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                            <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                            <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                            <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                            <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                            <asp:ListItem Value="ON">Ontario</asp:ListItem>
                            <asp:ListItem Value="QC">Quebec</asp:ListItem>
                            <asp:ListItem Value="YT">Yukon</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlSalesperson" runat="server"
                            CssClass="browser-default selectst selectsml" Visible="False">
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap srchclr btnlinksicon ">
                        <asp:LinkButton CausesValidation="false" ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" ToolTip="Search">
                                <i class="mdi-action-search"></i>
                        </asp:LinkButton>
                    </div>
                </telerik:RadAjaxPanel>
                <telerik:RadAjaxPanel runat="server" ID="RadAP_Pro_TotalRecords">
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:CheckBox ID="lnkChk" runat="server" Text="Incl. Inactive" CssClass="css-checkbox" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="true" Style="color: #222 !important;"></asp:CheckBox>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" CausesValidation="False" OnClick="lnkShowAll_Click">Show All</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" OnClick="lnkClear_Click" runat="server">Clear</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>--%>
                           
                                    <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                            
                                <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                            </span>
                        </div>
                    </div>
                </telerik:RadAjaxPanel>
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
                
            </div>
            <div class="grid_container">
                <div class="form-section-row m-b-0">
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="RadCodeBlock_Pro" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Pro.ClientID %>");
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

                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Pro" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Pro" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Pro" AllowFilteringByColumn="true" ShowFooter="False" PageSize="50"
                                ShowStatusBar="true" runat="server" PagerStyle-AlwaysVisible="true" AllowPaging="True" AllowSorting="true" Width="100%" OnPreRender="RadGrid_Pro_PreRender"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_Pro_NeedDataSource" OnItemCreated="RadGrid_Pro_ItemCreated" OnExcelMLExportRowCreated="RadGrid_Pro_ExcelMLExportRowCreated">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="true">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>

                                        <telerik:GridTemplateColumn HeaderStyle-Width="220" AutoPostBackOnFilter="true" HeaderText="Customer Lead Name" DataField="CustomerName" SortExpression="CustomerName" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                <asp:HyperLink ID="lnkCustomerName" runat="server"><%#Eval("CustomerName")%></asp:HyperLink>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server">Total :-</asp:Label>
                                            </FooterTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="name" HeaderText="Location Lead Name" SortExpression="name"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="180"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="city" HeaderText="City" SortExpression="city"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="State" HeaderText="State" HeaderStyle-Width="60"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="state"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="zip" Display="false" HeaderText="Zip" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" SortExpression="zip"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Contact"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="salesp" HeaderText="Salesperson" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="salesp"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="type" HeaderText="Type" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="type"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="StatusName" HeaderText="Status" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="StatusName"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <%--<telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="70" SortExpression="status" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>

                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Company"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Remarks" HeaderText="Notes" HeaderStyle-Width="200"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="remarks"
                                            ShowFilterIcon="false" Display="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="days" HeaderText="# Days" HeaderStyle-Width="90"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" SortExpression="days"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="numopp" HeaderText="# Opps" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" SortExpression="numopp" FooterAggregateFormatString="{0:d}" Aggregate="Sum"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="BusinessType" SortExpression="BusinessType" UniqueName="BusinessType" HeaderText="BusinessType" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" Visible="false">
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


</asp:Content>
