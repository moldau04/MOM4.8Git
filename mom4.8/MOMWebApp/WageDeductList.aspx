<%@ Page Title="Deduction Categories || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="WageDeductList" Codebehind="WageDeductList.aspx.cs" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

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
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Deduction Categories</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClientClick='return AddWageDeductionClick(this)' OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditWageDecutionClick(this)' OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>

                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                 <li>
                                                    <div class="btnlinks">
                                                        <a id="btnCopy" runat="server" onclientclick='return AddWageDeductionClick(this)' onserverclick="btnCopy_Click">Copy
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick='return DeleteWageDeductionClick(this)' OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>
                                                
                                            </ul>
                                        </div>
                                    </asp:Panel>

                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">

                    <ContentTemplate>
                        <div class="srchtitle" style="padding-left: 15px; display:none;">
                            Search
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlSearch" runat="server" class="browser-default selectst selectsml" AutoPostBack="true" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"  style="display:none;">
                            </asp:DropDownList>
                            
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default selectst" Visible="false"  style="display:none;">
                                
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst" Visible="false"  style="display:none;">
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                <asp:ListItem Value="Hold">Hold</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..."  style="display:none;"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap srchclr btnlinksicon">

                            <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"  style="display:none;"></i>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col lblsz2 lblszfloat">
                    <div class="row">
                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive"  style="display:none;"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click"  style="display:none;">Show All </asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" style="display:none;">Clear</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                             <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                            
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="grid_container">
            <div class="form-section-row m-b-0" >

                <telerik:RadAjaxManager ID="RadAjaxManager_WageDeduction" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageDeduction" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageDeduction" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageDeduction" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageDeduction" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_WageDeduction">
                            <UpdatedControls>   
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageDeduction" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <%--<telerik:AjaxSetting AjaxControlID="ddlSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageDeduction" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="ddlType"  />
                                <telerik:AjaxUpdatedControl ControlID="ddlStatus"  />
                                <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                                
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                        
                        <%--<telerik:AjaxSetting AjaxControlID="lnkClear">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageDeduction" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_WageDeduction" runat="server">
                </telerik:RadAjaxLoadingPanel>

                <div class="RadGrid RadGrid_Material">
                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= RadGrid_WageDeduction.ClientID %>");
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
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Deduction" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <%--OnItemEvent="RadGrid_WageDeduction_ItemEvent" OnExcelMLExportRowCreated="RadGrid_WageDeduction_ExcelMLExportRowCreated"--%>
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_WageDeduction" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"  FilterType="CheckList" 
                                                                                                OnNeedDataSource="RadGrid_WageDeduction_NeedDataSource"  OnExcelMLExportRowCreated="RadGrid_WageDeduction_ExcelMLExportRowCreated"
                                                                                                 OnItemCreated="RadGrid_WageDeduction_ItemCreated" OnItemEvent="RadGrid_WageDeduction_ItemEvent"
                                                                                                PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_WageDeduction_PreRender"
                                                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                                    <Columns>
                                                                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect"  HeaderStyle-Width="40">
                                                                                                        </telerik:GridClientSelectColumn>
                                                                                                        <telerik:GridTemplateColumn   Display="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>                                                                                                                
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>                                                                                                        
                                                                                                        <telerik:GridTemplateColumn   DataField="fdesc" HeaderText="Description" UniqueName="fdesc" SortExpression="fdesc"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:HyperLink ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn   DataField="TypeName" HeaderText="Type" SortExpression="TypeName" UniqueName="TypeName"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn   DataField="PaidBy" HeaderText="Paid By" SortExpression="PaidBy" UniqueName="PaidBy"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblByW" runat="server" Text='<%# Eval("PaidBy") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn   DataField="Count" HeaderText="Count" UniqueName="Count" SortExpression="Count"
                                                                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Count") %>'></asp:Label>
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

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDedcutions" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function AddWageDeductionClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddDedcutions.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditWageDecutionClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditDedcutions.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteWageDeductionClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteDedcutions.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_WageDeduction.ClientID%>', 'Vendor');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        
    function clear() {
        ("#ddlType");
    }
        function jsFunction(value) {
            alert(value);

            if (value == "Rol.Type") {
                //$(txtSearch).val("");
                //$('#searchField').attr("value", "");
                document.getElementById('<%=txtSearch.ClientID%>').value = '';
                document.getElementById('<%=ddlType.ClientID%>').style.display = 'block';  
                document.getElementById('<%=ddlStatus.ClientID%>').style.display = 'none';  
                document.getElementById('<%=txtSearch.ClientID%>').style.display = 'none';  
                
            }
            else if (value == "Vendor.Status") {
                //$(txtSearch).val("");
                document.getElementById('<%=txtSearch.ClientID%>').value = '';
                document.getElementById('<%=ddlType.ClientID%>').style.display = 'none';
                document.getElementById('<%=ddlStatus.ClientID%>').style.display = 'block';
                document.getElementById('<%=txtSearch.ClientID%>').style.display = 'none';  

            }
            else {
                document.getElementById('<%=ddlType.ClientID%>').style.display = 'none';
                document.getElementById('<%=ddlStatus.ClientID%>').style.display = 'none';
                document.getElementById('<%=txtSearch.ClientID%>').style.display = 'block'; 
            }

        }
        
    </script>


    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
            //$(reports).each(function (index, report) {
            //    var imagePath = null;
            //    if (report.IsGlobal == true) {
            //        imagePath = "images/globe.png";
            //    }
            //    else {
            //        imagePath = "images/blog_private.png";
            //    }

            //    $('#dynamicUI').append('<li><a href="VendorListReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Vendor"><span><img src=images/reportfolder.png> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

            //});


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


