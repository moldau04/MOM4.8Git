<%@ Page Title="Locations || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="Locations" CodeBehind="Locations.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
    <script type="text/javascript">
        function AddLocClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeLoc.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {

                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditLocClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeLoc.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteLocClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteLoc.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_Location.ClientID%>', 'location');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function CopyLocClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeLoc.ClientID%>').value;
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
            // showfilter();
            $('#colorNav #dynamicUI li').remove();
            $(reports).each(function (index, report) {
                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }

                $('#dynamicUI').append('<li><a href="LocationReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Location"><span><img src=images/reportfolder.png> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

            });
        });
    </script>
       <style>
        [id$='PageSizeComboBox']{
           width:5.1em !important;
        }
    </style>
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
                                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Locations</asp:Label>
                                        </div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:HyperLink ID="lnkAddnew" runat="server" onclick='return AddLocClick(this)' NavigateUrl="addlocation.aspx">Add</asp:HyperLink>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditLocClick(this)' OnClick="btnCustEdit_Click">Edit</asp:LinkButton>
                                            </div>

                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>
                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnCopy" runat="server" OnClientClick='return CopyLocClick(this)' OnClick="btnCopy_Click">Copy</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="btnDelete" runat="server" OnClientClick='return DeleteLocClick(this)' OnClick="btnDelete_Click">Delete</asp:LinkButton>
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
                                                            <a href="LocationReport.aspx?type=Location" class="-text">Add New Report</a>
                                                        </li>
                                                        <li>
                                                            <a href="AcctLabels5160.aspx" class="-text">Location Label 5160</a>
                                                        </li>
                                                        <li>
                                                            <a href="AcctLabels5163.aspx" class="-text">Location Label 5163</a>
                                                        </li>
                                                        <li runat="server" visible="false">
                                                            <asp:LinkButton ID="lnkCustomReports" runat="server" OnClick="lnkCustomReports_Click">Custom Report</asp:LinkButton>
                                                        </li>
                                                         <li>
                                                             <asp:LinkButton ID="lnkEquipmentListReport" runat="server" OnClick="lnkEquipmentListReport_Click">Location Equipment List Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                             <asp:LinkButton ID="lnkLocationBusinessType" runat="server" OnClick="lnkLocationBusinessType_Click" Text ="Location by Business Type Report"></asp:LinkButton>
                                                        </li>
                                                         <li>
                                                             <asp:LinkButton ID="lnkLocationDetails" runat="server" OnClick="lnkLocationDetails_Click">Location Details Report</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                             <asp:LinkButton ID="lnkLocationHomeOwner" runat="server" OnClick="lnkLocationHomeOwner_Click">Location with Home Owner Report</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                    <div class="btnlinks">
                                                        <a class="dropdown-button" data-beloworigin="true" href="LocationReport.aspx" data-activates="dropdown1">Reports
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkUpdateGeocode" runat="server" OnClick="lnkUpdateGeocode_Click">Update Geocode</asp:LinkButton>
                                                    </div>
                                                </li>
                                            </ul>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkMassAttachDocuments" runat="server" OnClick="lnkMassAttachDocuments_Click">Mass Attach Documents</asp:LinkButton>
                                            </div>
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
                <div class="srchtitle">
                    Search
                </div>
                <div class="srchinputwrap">
                    <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" onchange="showfilter();return false;"
                        OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                        CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbBilling" style="display: none" runat="server">
                    <asp:DropDownList ID="rbBilling" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbCredit" style="display: none" runat="server">
                    <asp:DropDownList ID="rbCredit" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>

                <div class="srchinputwrap" id="div_rbDispAlert" style="display: none" runat="server">
                    <asp:DropDownList ID="rbDispAlert" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbPrintInvoice" style="display: none" runat="server">
                    <asp:DropDownList ID="rbPrintInvoice" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbEmailInvoice" style="display: none" runat="server">
                    <asp:DropDownList ID="rbEmailInvoice" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbNoCustomerStatement" style="display: none" runat="server">
                    <asp:DropDownList ID="rbNoCustomerStatement" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbStatus" style="display: none" runat="server">
                    <asp:DropDownList ID="rbStatus" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_rbMaint" style="display: none" runat="server">
                    <asp:DropDownList ID="rbMaint" runat="server" Visible="true" CssClass="browser-default selectst selectsml">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_ddlUserType" style="display: none" runat="server">
                    <telerik:RadComboBox RenderMode="Auto" ID="ddlUserType" runat="server" CssClass="browser-default selectst selectsml"
                        Visible="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                    </telerik:RadComboBox>
                </div>
                <div class="srchinputwrap" id="div_ddlCompany" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default selectst selectsml"
                        Visible="true">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_ddlTerr" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlTerr" runat="server" CssClass="browser-default selectst selectsml"
                        Visible="true">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_ddlTerr2" style="display: none" runat="server">
                    <asp:DropDownList ID="ddlTerr2" runat="server" CssClass="browser-default selectst selectsml"
                        Visible="true">
                    </asp:DropDownList>
                </div>
                <div class="srchinputwrap" id="div_ddlZone1" style="display: none" runat="server">
                    <telerik:RadComboBox RenderMode="Auto" ID="ddlZone1" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CssClass="browser-default selectst selectsml"
                        Visible="true">
                    </telerik:RadComboBox>
                </div>
                <div class="srchinputwrap" id="div_ddlRoute" style="display: none;width:150px !important;" runat="server">
                    <telerik:RadComboBox RenderMode="Auto" ID="ddlRoute" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CssClass="browser-default selectst selectsml"
                        Visible="true">
                    </telerik:RadComboBox>
                </div>
                <div class="srchinputwrap" id="div_ddlBusinessType" style="display: none" runat="server">


                    <asp:DropDownList ID="ddlBusinessType" runat="server" CssClass="browser-default selectst selectsml"
                        Visible="true">
                    </asp:DropDownList>

                </div>
                <div class="srchinputwrap" id="div_txtSearch" style="display: block" runat="server">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."></asp:TextBox>
                </div>

                <div class="srchinputwrap srchclr btnlinksicon" >

                    <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="false"
                        OnClick="lnkSearch_Click">
                         <i class="mdi-action-search"></i>
                    </asp:LinkButton>
                </div>
               

                <div class="col lblsz2 lblszfloat">
                    <div class="row">                                               
                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click"  OnClientClick="resetShowAll();">Show All </asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" OnClientClick="resetShowAll();">Clear</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>--%>
                                    <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                               <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </span>
                    </div>
                </div>                
            </div>

            <div class="grid_container">
                <div class="form-section-row" aria-autocomplete="none" style="margin-bottom: 0 !important;">
                    <telerik:RadAjaxManager ID="RadAjaxManager_Location" runat="server">
                        <AjaxSettings>
                          
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Location" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                     <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="RadGrid_Location">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Location" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <%-- <telerik:AjaxSetting AjaxControlID="lnkSearch" EventName="Click">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="lstMemo" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>--%>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Location" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                      <telerik:AjaxUpdatedControl ControlID="ddlSearch"  />
                                     <telerik:AjaxUpdatedControl ControlID="ddlRoute" />
                                     <telerik:AjaxUpdatedControl ControlID="ddlUserType" />
                                     <telerik:AjaxUpdatedControl ControlID="ddlZone1" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Location" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                      <telerik:AjaxUpdatedControl ControlID="lblRecordCount" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlRoute" />
                                     <telerik:AjaxUpdatedControl ControlID="ddlUserType" />
                                     <telerik:AjaxUpdatedControl ControlID="ddlZone1" />
                                     <telerik:AjaxUpdatedControl ControlID="lnkChk" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkUpdateGeocode">
                            <UpdatedControls>   
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Location" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                <telerik:AjaxUpdatedControl ControlID="gv_Errorrows" />
                                <telerik:AjaxUpdatedControl ControlID="lblInvalidRows" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Location" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Location.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Location" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Location" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Location" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" 
                                OnPreRender="RadGrid_Location_PreRender" 
                                OnItemCreated="RadGrid_Location_ItemCreated"
                                OnItemCommand="RadGrid_Location_ItemCommand"
                                 OnNeedDataSource="RadGrid_Location_NeedDataSource" 
                                OnExcelMLExportRowCreated="RadGrid_Location_ExcelMLExportRowCreated" 
                                OnFilterCheckListItemsRequested="RadGrid_Location_FilterCheckListItemsRequested"
                                
                                >
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="Name">
                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                    
                                    <Columns>
                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblloc" runat="server" Text='<%# Eval("loc") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                        </telerik:GridClientSelectColumn>
                                       
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="50" UniqueName="ImageQB">
                                            <ItemTemplate>
                                                <asp:Image ID="imgQB" runat="server" Width="16px" ToolTip="Synced in QB" ImageUrl='<%# Eval("qblocid").ToString() != "" ? "images/qb32.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <%--<asp:Image ID="imgsageid" runat="server" Width="16px" ToolTip="Synced in Sage300" ImageUrl='<%# Eval("SageID").ToString() != "" && Eval("SageID").ToString() != "NA" ? "images/sage300.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>     

                                       <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="40" UniqueName="sageid">
                                            <ItemTemplate>
                                                <asp:Image ID="imgsageid" runat="server" Width="16px" ToolTip="Synced in Sage300" ImageUrl='<%# Eval("sageid").ToString() != "" && Eval("SageID").ToString() != "NA" ? "images/sage300.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                <%#Eval("sageid")%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Reorderable="false" Resizable="false" UniqueName="Geocode" Visible="true" AutoPostBackOnFilter="false" AllowFiltering="false"  HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="30">
                                            <ItemTemplate>
                                                <asp:Image ID="imgGeocode" runat="server" Width="15px" ToolTip="Geocode" Visible='<%# Eval("Geocode").ToString().Trim() == "" ? false : true%>' ImageUrl="~/images/GeocodeIcon.png" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="locid" SortExpression="locid" HeaderText="Acct #" UniqueName="locid"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="30" UniqueName="ImgCreditH">
                                            <ItemTemplate>
                                               
                                                <asp:Image ID="imgCreditH" runat="server" title="Credit Hold" Width="16px" ImageUrl='<%# Eval("credit").ToString() == "1" ? "images/MSCreditHold.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        


                                        <telerik:GridTemplateColumn DataField="tag" SortExpression="tag" AutoPostBackOnFilter="true" HeaderStyle-Width="140"
                                            CurrentFilterFunction="Contains" UniqueName="Location" HeaderText="Location Name" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                 <asp:HyperLink ID="lnkName" runat="server" Text='<%#Eval("tag")%>'></asp:HyperLink>

                                         <%--      <asp:HyperLink ID="lnkName" runat="server" Text='<%#Eval("tag")%>' Target="_blank" NavigateUrl='<%# "AddLocation.aspx?uid=" +Eval("loc")  %>' ForeColor="#0066CC"></asp:HyperLink>--%>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Address" SortExpression="Address" UniqueName="Address" HeaderText="Address" HeaderStyle-Width="190"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="City" SortExpression="City" UniqueName="City" HeaderText="City" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                         <telerik:GridBoundColumn DataField="Zip" SortExpression="Zip" UniqueName="Zip" HeaderText="Zip" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Type" SortExpression="Type" UniqueName="Type" HeaderText="Type" HeaderStyle-Width="80" FilterCheckListEnableLoadOnDemand="true" CurrentFilterFunction="Contains"
                                            FilterControlAltText="Filter Type" AutoPostBackOnFilter="true" >
                                        </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="locStatus" SortExpression="locStatus" UniqueName="locStatus" HeaderText="Status" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>


                                         <telerik:GridTemplateColumn DataField="ContractStatus" HeaderText="Recurring Status" SortExpression="ContractStatus" UniqueName="ContractStatus" HeaderStyle-Width="100"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    <ItemTemplate>

                                                        <asp:Label ID="lblContractStatus" runat="server" Text='<%# Eval("ContractStatus") %>'></asp:Label>
                                                        <telerik:RadToolTip RenderMode="Auto" ID="ContractStatusTooltip" runat="server" TargetControlID="lblContractStatus" Width="500px"
                                                            RelativeTo="Element" Position="MiddleRight">
                                                            <%#getAllContractByID(Eval("loc"),Eval("ContractStatus")) %>
                                                        </telerik:RadToolTip>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>


                                       

                                       <%-- <telerik:GridTemplateColumn DataField="Status" UniqueName="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>--%>
                                         <telerik:GridTemplateColumn DataField="Customer" SortExpression="Customer" AutoPostBackOnFilter="true" HeaderStyle-Width="120"
                                            CurrentFilterFunction="Contains" UniqueName="Customer" HeaderText="Customer" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                   <asp:HyperLink ID="lblCus" runat="server" Text='<%# Bind("Customer") %>' Target="_blank" NavigateUrl='<%# "addcustomer.aspx?uid=" +Eval("CusID")  %>' ForeColor="#0066CC"></asp:HyperLink>
                                             
                                             <%--   <asp:Label runat="server" Text='<%#Eval("Customer")%>' ID="lblCus"></asp:Label>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Company" SortExpression="Company" UniqueName="Company" HeaderText="Company" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="opencall" SortExpression="opencall" UniqueName="opencall" FooterAggregateFormatString="{0:d}" Aggregate="Sum" HeaderText="Tickets" HeaderStyle-Width="70"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Elevs" SortExpression="Elevs" UniqueName="Elevs" FooterAggregateFormatString="{0:d}" Aggregate="Sum" HeaderText="Equip" HeaderStyle-Width="70"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="ActiveElevs" SortExpression="ActiveElevs" UniqueName="ActiveElevs" FooterAggregateFormatString="{0:d}" Aggregate="Sum" HeaderText="Active Equip" HeaderStyle-Width="70"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn UniqueName="Balance" DataField="balance" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="balance" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Balance" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="CustomerName" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="CustomerName" ShowFilterIcon="false" Visible="false" HeaderText="Customer Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("CustomerName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="LocationName" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="LocationName" ShowFilterIcon="false" Visible="false" HeaderText="Location Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("LocationName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn DataField="Email" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="Email" ShowFilterIcon="false" Visible="false" HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn DataField="ContactName" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" UniqueName="ContactName" ShowFilterIcon="false" Visible="false" HeaderText="Contact Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContactName" runat="server" Text='<%#Eval("ContactName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                              <telerik:GridBoundColumn DataField="State" SortExpression="State" UniqueName="State" HeaderText="State" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                         <telerik:GridBoundColumn DataField="Salesperson" SortExpression="Salesperson" UniqueName="Salesperson" HeaderText="Salesperson" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="Salesperson2" SortExpression="Salesperson2" UniqueName="Salesperson2" HeaderText="Salesperson2" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                          <telerik:GridBoundColumn DataField="RouteName" SortExpression="RouteName" UniqueName="RouteName" HeaderText="RouteName" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        
                                          <telerik:GridBoundColumn DataField="Zone" SortExpression="Zone" UniqueName="Zone" HeaderText="Zone" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" Visible="false">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="BusinessTypeName" SortExpression="BusinessTypeName" UniqueName="BusinessType" HeaderText="BusinessType" HeaderStyle-Width="120"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" Visible="false">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>                                
                            </telerik:RadGrid>





                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="errorWindow" Skin="Material" VisibleTitlebar="true" Title="MOM" Behaviors="Default" CenterIfModal="true"
                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                        runat="server" Modal="true" Width="1000" Height="600">
                                        <ContentTemplate>
                                            <div style="margin-top: 15px;">
                                                <div class="col-lg-12 col-md-12 form-section-row">
                                                    <div style="float: right;">
                                                        <%--<span>Total Rows :
                                                            <asp:Label ID="lblTotalRows" runat="server" />
                                                            |</span>
                                                        <span style="color: green">Valid Rows :
                                                            <asp:Label ID="lblValidRows" runat="server" />
                                                            |</span>--%>
                                                        <span style="color: black">
                                                            <asp:Label ID="lblInvalidRows" runat="server" /></span>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>
                                                <div class="RadGrid" style="max-height: 100% !important; overflow: auto;">
                                                   
                                                    <telerik:RadGrid RenderMode="Auto" ID="gv_Errorrows" ShowFooter="false"
                                                        ShowStatusBar="false" runat="server" AllowSorting="false" Width="100%">
                                                        <CommandItemStyle />
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                            <Selecting AllowRowSelect="false"></Selecting>
                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            <Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" ShowFooter="True" DataKeyNames="ID">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn DataField="Employee" SortExpression="Employee" AutoPostBackOnFilter="true"
                                                                    HeaderText="Employee" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmployee" Text='<%# Bind("Location")%>' runat="server" />
                                                                        <asp:HiddenField ID="hdnID" Value='<%# Bind("ID")%>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Address" SortExpression="Address" AutoPostBackOnFilter="true"
                                                                    HeaderText="Address" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddress" Text='<%# Bind("Address") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="City" SortExpression="City" AutoPostBackOnFilter="true"
                                                                    HeaderText="City" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCity" Text='<%# Bind("City") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="State" SortExpression="State" AutoPostBackOnFilter="true"
                                                                    HeaderText="State" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblState" Text='<%# Bind("State") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Zip" SortExpression="Zip" AutoPostBackOnFilter="true"
                                                                    HeaderText="Zip" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblZip" Text='<%# Bind("Zip") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>



                                                </div>
                                                <div class="col-lg-12 col-md-12 form-section-row">
                                                    <h6>Here are the list of employees having invalid address please correct the address .</h6>
                                                </div>
                                                <div style="clear: both;"></div>
                                                <footer>
                                                    <%--<div class="btnlinks" style="float: right;">
                                                        <asp:LinkButton Text="Continue" runat="server" ID="btnContinue"  />
                                                        <asp:LinkButton Text="Cancel" runat="server" ID="btnCancel"  />
                                                    </div>--%>
                                                </footer>
                                            </div>
                                        </ContentTemplate>
                                    </telerik:RadWindow>
                                   
                                </Windows>
                            </telerik:RadWindowManager>

                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnAddeLoc" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeLoc" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteLoc" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewLoc" Value="Y" />   
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script>
        $(document).ready(function () {

            $("#<%=RadGrid_Location.ClientID%>").attr("autocomplete", "off");
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
            ddlSearch.val("selected");
            var txt = $('#<%=txtSearch.ClientID%>');
            txt.val("");
            var rbMaint = document.getElementById('<%=div_rbMaint.ClientID%>');
            var rbStatus = document.getElementById('<%=div_rbStatus.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearch.ClientID%>');
            var ddlCompany = document.getElementById('<%=div_ddlCompany.ClientID%>');
            var rbBilling = document.getElementById('<%=div_rbBilling.ClientID%>');
            var rbCredit = document.getElementById('<%=div_rbCredit.ClientID%>');
            var rbDispAlert = document.getElementById('<%=div_rbDispAlert.ClientID%>');
            var rbEmailInvoice = document.getElementById('<%=div_rbEmailInvoice.ClientID%>');
            var rbPrintInvoice = document.getElementById('<%=div_rbPrintInvoice.ClientID%>');
            var ddlTerr = document.getElementById('<%=div_ddlTerr.ClientID%>');
            var ddlTerr2 = document.getElementById('<%=div_ddlTerr2.ClientID%>');
            var rbNoCustomerStatement = document.getElementById('<%=div_rbNoCustomerStatement.ClientID%>');
            var ddlZone1 = document.getElementById('<%=div_ddlZone1.ClientID%>');
            var ddlUserType = document.getElementById('<%=div_ddlUserType.ClientID%>');
            var ddlRoute = document.getElementById('<%=div_ddlRoute.ClientID%>');
            var ddlBusinessType = document.getElementById('<%=div_ddlBusinessType.ClientID%>');

            try {

                $('#<%=rbStatus.ClientID%>').val($("#<%=rbStatus.ClientID%> option:first").val());
                 $('#<%=rbBilling.ClientID%>').val($("#<%=rbBilling.ClientID%> option:first").val());
                 $('#<%=rbCredit.ClientID%>').val($("#<%=rbCredit.ClientID%> option:first").val());
                 $('#<%=rbDispAlert.ClientID%>').val($("#<%=rbDispAlert.ClientID%> option:first").val());
                 $('#<%=rbEmailInvoice.ClientID%>').val($("#<%=rbEmailInvoice.ClientID%> option:first").val());
                 $('#<%=rbPrintInvoice.ClientID%>').val($("#<%=rbPrintInvoice.ClientID%> option:first").val());
                 $('#<%=rbNoCustomerStatement.ClientID%>').val($("#<%=rbNoCustomerStatement.ClientID%> option:first").val());
                 $('#<%=rbMaint.ClientID%>').val($("#<%=rbMaint.ClientID%> option:first").val());
                 $('#<%=ddlTerr.ClientID%>').val($("#<%=ddlTerr.ClientID%> option:first").val());
                 $('#<%=ddlTerr2.ClientID%>').val($("#<%=ddlTerr2.ClientID%> option:first").val());

            } catch (ex) { }
            rbMaint.style.display = "none";
            rbStatus.style.display = "none";
            txtSearch.style.display = "none";
            ddlCompany.style.display = "none";
            rbBilling.style.display = "none";
            rbCredit.style.display = "none";
            rbDispAlert.style.display = "none";
            rbEmailInvoice.style.display = "none";
            rbPrintInvoice.style.display = "none";
            ddlTerr.style.display = "none";
            ddlTerr2.style.display = "none";
            rbNoCustomerStatement.style.display = "none";
            ddlZone1.style.display = "none";
            ddlUserType.style.display = "none";
            ddlRoute.style.display = "none";
            ddlBusinessType.style.display = "none";
            txtSearch.style.display = "block";
            try {

                $('#<%=rbStatus.ClientID%>').val($("#<%=rbStatus.ClientID%> option:first").val());
                 $('#<%=rbBilling.ClientID%>').val($("#<%=rbBilling.ClientID%> option:first").val());
                 $('#<%=rbCredit.ClientID%>').val($("#<%=rbCredit.ClientID%> option:first").val());
                 $('#<%=rbDispAlert.ClientID%>').val($("#<%=rbDispAlert.ClientID%> option:first").val());
                 $('#<%=rbEmailInvoice.ClientID%>').val($("#<%=rbEmailInvoice.ClientID%> option:first").val());
                 $('#<%=rbPrintInvoice.ClientID%>').val($("#<%=rbPrintInvoice.ClientID%> option:first").val());
                 $('#<%=rbNoCustomerStatement.ClientID%>').val($("#<%=rbNoCustomerStatement.ClientID%> option:first").val());
                 $('#<%=rbMaint.ClientID%>').val($("#<%=rbMaint.ClientID%> option:first").val());
                     $('#<%=ddlTerr.ClientID%>').val($("#<%=ddlTerr.ClientID%> option:first").val());

                    $('#<%=ddlTerr2.ClientID%>').val($("#<%=ddlTerr2.ClientID%> option:first").val());
                var comboUserType = $find("<%= ddlUserType.ClientID %>");
                comboUserType.trackChanges();
                for (var i = 0; i < comboUserType.get_items().get_count(); i++) {
                    comboUserType.get_items().getItem(i).set_checked(false);
                }
                comboUserType.commitChanges();


                   var comboRoute = $find("<%= ddlRoute.ClientID %>");
                comboRoute.trackChanges();
                for (var i = 0; i < comboRoute.get_items().get_count(); i++) {
                    comboRoute.get_items().getItem(i).set_checked(false);
                }
                comboRoute.commitChanges();

                    var comboZone1 = $find("<%= ddlZone1.ClientID %>");
                comboZone1.trackChanges();
                for (var i = 0; i < comboZone1.get_items().get_count(); i++) {
                    comboZone1.get_items().getItem(i).set_checked(false);
                }
                comboZone1.commitChanges();

                    var comboBusinessType = $find("<%= ddlBusinessType.ClientID %>");
                comboBusinessType.trackChanges();
                for (var i = 0; i < comboBusinessType.get_items().get_count(); i++) {
                    comboBusinessType.get_items().getItem(i).set_checked(false);
                }
                comboBusinessType.commitChanges();

            
               
            } catch (ex) { }
           
        }

        function showfilter() {
            //debugger;
            var ddlSearch = $('#<%=ddlSearch.ClientID%>');
            var rbMaint = document.getElementById('<%=div_rbMaint.ClientID%>');
            var rbStatus = document.getElementById('<%=div_rbStatus.ClientID%>');
            var txtSearch = document.getElementById('<%=div_txtSearch.ClientID%>');
            var ddlCompany = document.getElementById('<%=div_ddlCompany.ClientID%>');
            var rbBilling = document.getElementById('<%=div_rbBilling.ClientID%>');
            var rbCredit = document.getElementById('<%=div_rbCredit.ClientID%>');
            var rbDispAlert = document.getElementById('<%=div_rbDispAlert.ClientID%>');
            var rbEmailInvoice = document.getElementById('<%=div_rbEmailInvoice.ClientID%>');
            var rbPrintInvoice = document.getElementById('<%=div_rbPrintInvoice.ClientID%>');
            var ddlTerr = document.getElementById('<%=div_ddlTerr.ClientID%>');
            var ddlTerr2 = document.getElementById('<%=div_ddlTerr2.ClientID%>');
            var rbNoCustomerStatement = document.getElementById('<%=div_rbNoCustomerStatement.ClientID%>');
            var ddlZone1 = document.getElementById('<%=div_ddlZone1.ClientID%>');
            var ddlUserType = document.getElementById('<%=div_ddlUserType.ClientID%>');
            var ddlRoute = document.getElementById('<%=div_ddlRoute.ClientID%>');
            var ddlBusinessType = document.getElementById('<%=div_ddlBusinessType.ClientID%>');

            rbMaint.style.display = "none";
            rbStatus.style.display = "none";
            txtSearch.style.display = "none";
            ddlCompany.style.display = "none";
            rbBilling.style.display = "none";
            rbCredit.style.display = "none";
            rbDispAlert.style.display = "none";
            rbEmailInvoice.style.display = "none";
            rbPrintInvoice.style.display = "none";
            ddlTerr.style.display = "none";
            ddlTerr2.style.display = "none";
            rbNoCustomerStatement.style.display = "none";
            ddlZone1.style.display = "none";
            ddlRoute.style.display = "none";
            ddlUserType.style.display = "none";
            ddlBusinessType.style.display = "none";
            switch (String(ddlSearch.val())) {
                case "l.Status":
                    rbStatus.style.display = "block";
                    break;
                case "b.Name":
                    ddlCompany.style.display = "block";
                    break;
                case "l.Maint":
                    rbMaint.style.display = "block";
                    break;
                case "l.Billing":
                    rbBilling.style.display = "block";
                    break;
                case "l.Credit":
                    rbCredit.style.display = "block";
                    break;
                case "l.EmailInvoice":
                    rbEmailInvoice.style.display = "block";
                    break;
                case "l.PrintInvoice":
                    rbPrintInvoice.style.display = "block";
                    break;
                case "l.DispAlert":
                    rbDispAlert.style.display = "block";
                    break;
                case "l.Terr":
                    ddlTerr.style.display = "block";
                    break;
                case "l.Terr2":
                    ddlTerr2.style.display = "block";
                    break;
                case "l.NoCustomerStatement":
                    rbNoCustomerStatement.style.display = "block";
                    break;
                case "l.Zone":
                    ddlZone1.style.display = "block";
                    break;
                case "l.type":
                    ddlUserType.style.display = "block";
                    break;
                case "rt.Name":
                    ddlRoute.style.display = "block";
                    break;
                case "l.businesstype":
                    ddlBusinessType.style.display = "block";
                    break;
                default:
                    txtSearch.style.display = "block";
                    var txt = $('#<%=txtSearch.ClientID%>');
                    txt.val("");
                    break;
            }
            
          
            try {

                $('#<%=rbStatus.ClientID%>').val($("#<%=rbStatus.ClientID%> option:first").val());
                 $('#<%=rbBilling.ClientID%>').val($("#<%=rbBilling.ClientID%> option:first").val());
                 $('#<%=rbCredit.ClientID%>').val($("#<%=rbCredit.ClientID%> option:first").val());
                 $('#<%=rbDispAlert.ClientID%>').val($("#<%=rbDispAlert.ClientID%> option:first").val());
                 $('#<%=rbEmailInvoice.ClientID%>').val($("#<%=rbEmailInvoice.ClientID%> option:first").val());
                 $('#<%=rbPrintInvoice.ClientID%>').val($("#<%=rbPrintInvoice.ClientID%> option:first").val());
                 $('#<%=rbNoCustomerStatement.ClientID%>').val($("#<%=rbNoCustomerStatement.ClientID%> option:first").val());
                 $('#<%=rbMaint.ClientID%>').val($("#<%=rbMaint.ClientID%> option:first").val());
                     $('#<%=ddlTerr.ClientID%>').val($("#<%=ddlTerr.ClientID%> option:first").val());

                $('#<%=ddlTerr2.ClientID%>').val($("#<%=ddlTerr2.ClientID%> option:first").val());
                var comboUserType = $find("<%= ddlUserType.ClientID %>");
                comboUserType.trackChanges();
                for (var i = 0; i < comboUserType.get_items().get_count(); i++) {
                    comboUserType.get_items().getItem(i).set_checked(false);
                }
                comboUserType.commitChanges();


                var comboRoute = $find("<%= ddlRoute.ClientID %>");
                comboRoute.trackChanges();
                for (var i = 0; i < comboRoute.get_items().get_count(); i++) {
                    comboRoute.get_items().getItem(i).set_checked(false);
                }
                comboRoute.commitChanges();

                var comboZone1 = $find("<%= ddlZone1.ClientID %>");
                comboZone1.trackChanges();
                for (var i = 0; i < comboZone1.get_items().get_count(); i++) {
                    comboZone1.get_items().getItem(i).set_checked(false);
                }
                comboZone1.commitChanges();

                var comboBusinessType = $find("<%= ddlBusinessType.ClientID %>");
                comboBusinessType.trackChanges();
                for (var i = 0; i < comboBusinessType.get_items().get_count(); i++) {
                    comboBusinessType.get_items().getItem(i).set_checked(false);
                }
                comboBusinessType.commitChanges();



            } catch (ex) { }

        }

    </script>
</asp:Content>
