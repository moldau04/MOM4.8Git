<%@ Page Title="Employee || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="EmployeeList" CodeBehind="EmployeeList.aspx.cs" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--<metaname = "viewport" content = "width=device-width, initial-scale=1" />--%>
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
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Employee</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkAddnew" runat="server" OnClientClick='return AddEmployeeClick(this)' OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditEmployeeClick(this)' OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>

                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                <li>
                                                    <div class="btnlinks">
                                                        <a id="btnCopy" runat="server" onclientclick='return AddEmployeeClick(this)' onserverclick="btnCopy_Click">Copy
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick='return DeleteEmployeeClick(this)' OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <%--<li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkAdjustYTD" runat="server" OnClick="lnkAdjustYTD_Click">Adjust YTD</asp:LinkButton>
                                                    </div>
                                                </li>--%>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkUpdateGeocode" runat="server" OnClick="lnkUpdateGeocode_Click">Update Geocode</asp:LinkButton>
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
                        <div class="srchtitle " style="padding-left: 15px; display: none;">
                            Search
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlSearch" runat="server" class="browser-default selectst selectsml" AutoPostBack="true" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" Style="display: none;">
                            </asp:DropDownList>

                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default selectst" Visible="false" Style="display: none;">
                            </asp:DropDownList>
                        </div>
                        <div class="srchinputwrap">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst" Visible="false" Style="display: none;">
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                <asp:ListItem Value="Hold">Hold</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" placeholder="Search..." Style="display: none;"></asp:TextBox>
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
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>

                        </span>
                    </div>
                </div>
            </div>
        </div>

        <div class="grid_container">
            <div class="form-section-row pmd-card" style="margin-bottom: 0 !important;">

                <telerik:RadAjaxManager ID="RadAjaxManager_WageDeduction" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />

                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_Employee">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkUpdateGeocode">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                <telerik:AjaxUpdatedControl ControlID="gv_Errorrows" />
                                <telerik:AjaxUpdatedControl ControlID="lblInvalidRows" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>

                        <%--<telerik:AjaxSetting AjaxControlID="ddlSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="ddlType"  />
                                <telerik:AjaxUpdatedControl ControlID="ddlStatus"  />
                                <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                                
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>

                        <%--<telerik:AjaxSetting AjaxControlID="lnkClear">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Employee" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
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
                                var grid = $find("<%= RadGrid_Employee.ClientID %>");
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
                        <%--OnItemEvent="RadGrid_Employee_ItemEvent" OnExcelMLExportRowCreated="RadGrid_Employee_ExcelMLExportRowCreated"--%>
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Employee" AllowFilteringByColumn="true" ShowFooter="True" PageSize="15" FilterType="CheckList"
                            OnNeedDataSource="RadGrid_Employee_NeedDataSource" OnExcelMLExportRowCreated="RadGrid_Employee_ExcelMLExportRowCreated"
                            OnItemCreated="RadGrid_Employee_ItemCreated" OnItemEvent="RadGrid_Employee_ItemEvent"
                            PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_Employee_PreRender"
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
                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                    </telerik:GridClientSelectColumn>
                                    <telerik:GridTemplateColumn Reorderable="false" Resizable="false" UniqueName="Geocode" Visible="true" AutoPostBackOnFilter="false" AllowFiltering="false"
                                        HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="30">
                                        <ItemTemplate>
                                            <%--<asp:Image ID="imgGeocode" runat="server" Width="15px" ToolTip="Geocode" Visible='<%# Eval("Geocode").ToString().Trim() == "" ? false : true%>' ImageUrl="~/images/GeocodeIcon.png" />--%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                    <telerik:GridTemplateColumn Display="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                            <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                                            <asp:Label ID="lblTypeid" runat="server" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Last" HeaderText="Last" UniqueName="Last" SortExpression="Last"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lblLast" runat="server" Text='<%# Eval("Last") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="fFirst" HeaderText="First" SortExpression="fFirst" UniqueName="fFirst"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFirst" runat="server" Text='<%# Eval("fFirst") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Title" HeaderText="Title" SortExpression="Title" UniqueName="Title"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="SStatus" HeaderText="Status" UniqueName="SStatus" SortExpression="SStatus"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("SStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="SSN" HeaderText="SSN" UniqueName="SSN" SortExpression="SSN"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSSN" runat="server" Text='<%# Eval("SSN") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="FField" HeaderText="Field" UniqueName="FField" SortExpression="FField"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblField" runat="server" Text='<%# Eval("FField") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Ref" HeaderText="Employee ID" UniqueName="Ref" SortExpression="Ref"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>

                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="errorWindow" Skin="Material" VisibleTitlebar="true" Title="MOM" Behaviors="Default" CenterIfModal="true"
                                    Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                    runat="server" Modal="true" Width="1000" Height="600">
                                    <ContentTemplate>
                                        <di class="m-t-15" style="margin-top: 15px;">
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
                                            <div class="RadGrid " style="max-height: 100% !important; overflow: auto;">

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
                                                                    <asp:Label ID="lblEmployee" Text='<%# Bind("Employee")%>' runat="server" />
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
                                        </di>
                                    </ContentTemplate>
                                </telerik:RadWindow>

                            </Windows>
                        </telerik:RadWindowManager>
                    </telerik:RadAjaxPanel>
                </div>

            </div>
        </div>
    </div>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddEmployee" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditEmployee" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteEmployee" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDedcutions" Value="Y" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function AddEmployeeClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddEmployee.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditEmployeeClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditEmployee.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteEmployeeClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteEmployee.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_Employee.ClientID%>', 'Vendor');
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
        function OpenErrorModal() {
            window.radopen(null, "errorWindow");
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


