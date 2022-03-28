<%@ Page Title="" Language="C#" MasterPageFile="~/mom.master" AutoEventWireup="true" Inherits="Planner" CodeBehind="Planner.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="js/jquery.ns-autogrow.js"></script>
    <script type="text/javascript" src="js/quickcodes.js"></script>
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            <asp:ScriptReference Path="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"/>
        </Scripts>
    </asp:ScriptManagerProxy>

    
    <div id="breadcrumbs-wrapper">
        <header>
            <div class="container row-color-grey">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="page-title">
                                <i class="mdi-action-trending-up"></i>&nbsp;
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Edit Planner</asp:Label>
                            </div>
                            <%--<input type="button" onclick="exportElement()" value="export" />--%>
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkExcel" runat="server" CausesValidation="False" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                            </div>
                            <div class="btnclosewrap">
                                <asp:LinkButton ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                            </div>
                            <div class="rght-content">
                                <div class="editlabel" id="trProj" runat="server">
                                </div>
                                <div class="editlabel">
                                    <asp:Label CssClass="title_text_Name" ID="lblPlannerNo" Text="-" runat="server">ssss</asp:Label>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </header>
    </div>
    <div class="page-content" style="height: 100%; width: 100%">
        <telerik:RadClientExportManager runat="server" ID="RadClientExportManager1">
            <PdfSettings FileName="MyFile.pdf" PaperSize="auto" Landscape="true" />
        </telerik:RadClientExportManager>
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
        
        <div class="row" style="height: 100%; width: 100%; margin: 0;">
            <div id="gantt_chart">
                <%--<iframe style="border: none;height:100%;width:100%" src="TelerikCustomGantt.aspx"></iframe>--%>
                <telerik:RadGantt runat="server"
                    ID="gantt"
                    ClientIDMode="Static"
                    AutoGenerateColumns="false"
                    SelectedView="WeekView"
                    EnableResources="true"
                    EnablePdfExport="false"
                    OnClientPdfExporting="OnClientPdfExporting">
                    <Columns>
                        <%--<telerik:GanttBoundColumn DataField="Id" HeaderText="Task ID" Width="80"></telerik:GanttBoundColumn>--%>
                        <telerik:GanttBoundColumn DataField="PlannerTaskID" HeaderText="Task ID" DataType="Number"  UniqueName="PlannerTaskID" AllowEdit="false" Width="80"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="Title" Width="250"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="Start" HeaderText="Start Date" Width="149"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="End" HeaderText="End Date" Width="149"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="CusDuration" HeaderText="Duration" Width="80" DataType="Number" UniqueName="CusDuration"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="PercentComplete" HeaderText="% Completed" Width="100"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="CusActualHour" HeaderText="Actual Hour" Width="80" DataType="Number" UniqueName="CusActualHour" AllowEdit="false"></telerik:GanttBoundColumn>
                        <%--<telerik:GanttResourceColumn HeaderText="Assigned Resources" Width="120"></telerik:GanttResourceColumn>--%>
                        <%--<telerik:GanttDe HeaderText="Assigned Resources"></telerik:GanttDe>--%>
                        
                        <telerik:GanttBoundColumn DataField="Vendor" HeaderText="Vendor" DataType="String" UniqueName="Vendor" Width="155"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="Description" HeaderText="Notes" DataType="String" UniqueName="Description" Width="120"></telerik:GanttBoundColumn>
                        <telerik:GanttBoundColumn DataField="Dependency" HeaderText="Dependency" DataType="String" UniqueName="Dependency" Width="120" AllowEdit="true"></telerik:GanttBoundColumn>
                        <%--<telerik:GanttBoundColumn DataField="ItemRefID" HeaderText="Ref ID" DataType="Number" UniqueName="ItemRefID" Width="120" AllowEdit="false"></telerik:GanttBoundColumn>--%>

                    </Columns>
                    <CustomTaskFields>
                        <telerik:GanttCustomField PropertyName="CusActualHour" ClientPropertyName="cusActualHour" />
                        <telerik:GanttCustomField PropertyName="Description" ClientPropertyName="description" />
                        <telerik:GanttCustomField PropertyName="CusDuration" ClientPropertyName="cusDuration" />
                        <telerik:GanttCustomField PropertyName="Vendor" ClientPropertyName="vendor" />
                        <telerik:GanttCustomField PropertyName="VendorID" ClientPropertyName="vendorId" />
                        <telerik:GanttCustomField PropertyName="Dependency" ClientPropertyName="dependency" />
                        <telerik:GanttCustomField PropertyName="PlannerTaskID" ClientPropertyName="plannerTaskID" />
                        <telerik:GanttCustomField PropertyName="ItemRefID" ClientPropertyName="itemRefID" />
                    </CustomTaskFields>
                    <WebServiceSettings Path="GanttCustomService.asmx" />
                </telerik:RadGantt>
            </div>
            <telerik:RadWindow
                ID="RadWindow2"
                RenderMode="Auto"
                Skin="Material"
                VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn"
                EnableShadow="true"
                RestrictionZoneID="RestrictionZone"
                Modal="true" 
                Title="Edit Task"
                runat="server"
                Width="750px"
                Height="500px"
                VisibleStatusbar="False">
                <ContentTemplate>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkPostback">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PO" LoadingPanelID="RadAjaxLoadingPanel_PO"/>
                                    <%--<telerik:AjaxUpdatedControl ControlID="hdn" LoadingPanelID="RadAjaxLoadingPanel_PO"/>--%>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_PO" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div id="Div1" runat="server" class="card-reveal">
                        <div class="form-section-row" style="margin-bottom:0;">
                            <div class="btnlinks" style="height:35px;">
                                <telerik:RadButton ID="RadButton1" runat="server" Text="Save" AutoPostBack="false" OnClientClicking="OnClientSaveClicking"></telerik:RadButton>
                            </div>
                            <div class="btnlinks" style="height:35px;">
                                <telerik:RadButton ID="RadButton2" runat="server" Text="Cancel" AutoPostBack="false" OnClientClicked="OnClientCancelClicked"></telerik:RadButton>
                            </div>
                            <div class="btnlinks" style="height:35px;">
                                <telerik:RadButton ID="RadButton3" runat="server" Text="Delete" AutoPostBack="false" OnClientClicked="OnClientDeleteClicked"></telerik:RadButton>
                            </div>
                            <div class="col s12 m12 l12">
                                <ul class="tabs tab-demo-active white" style="width: 100%;">
                                    <li id="Li1" runat="server" class="tab col s2">
                                        <a class="waves-effect waves-light prodept" id="userDetTab" href="#up1">General</a>
                                    </li>
                                    <li class="tab col s2"  id="Li3" runat="server" >
                                        <a class="waves-effect waves-light prodept" href="#up4">Notes</a>
                                    </li>
                                    <li class="tab col s2">
                                        <a class="waves-effect waves-light prodept" id="addressTab" href="#up2">PO</a>
                                    </li>
                                    <li class="tab col s2"  id="Li2" runat="server" >
                                        <a class="waves-effect waves-light prodept" href="#up3">Documents</a>
                                    </li>
                                </ul>
                            </div>
                            <div class="col s12 m12">
                                <div style="display: block;">
                                    <div class="form-content-wrap">
                                        <div class="tabs-custom-mgn1" style="padding-top: 10px;">
                                            <div class="form-section-row"  style="margin-bottom: 0;">
                                                <div class="row" style="margin-bottom: 0;">
                                                    <div id="up1" class="col s12 tab-container-border lighten-4" style="display: block;">
                                                        <div class="tabs-custom-mgn1">
                                                            <div class="form-section-row" style="margin-bottom: 0;">
                                                                <div class="form-section">
                                                                    <div class="input-field col s12">
                                                                        <div class="row" style="margin-bottom: 10px;">
                                                                            <%--Title--%>
                                                                            <asp:Label ID="Label2" Text="Title" runat="server" Width="85px" />
                                                                            <asp:TextBox ID="TextBox2" runat="server" Width="420px">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <%--Start--%>
                                                                            <asp:Label ID="Label3" Text="Start"  runat="server" Width="85px" />
                                                                            <telerik:RadDateTimePicker ID="RadDatePicker1" Width="225px" Height="35" runat="server">
                                                                                <DateInput DateFormat="yyyy/M/d HH:mm" DisplayDateFormat="yyyy/M/d HH:mm"></DateInput>
                                                                            </telerik:RadDateTimePicker>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="RadDatePicker1"
                                                                                ErrorMessage="Start date is required!" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>


                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <%--End--%>
                                                                            <asp:Label ID="Label4" Text="End" runat="server" Width="85px" />
                                                                            <telerik:RadDateTimePicker ID="RadDatePicker2" Width="225px" Height="35" runat="server">
                                                                                <DateInput DateFormat="yyyy/M/d HH:mm" DisplayDateFormat="yyyy/M/d HH:mm"></DateInput>
                                                                            </telerik:RadDateTimePicker>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="RadDatePicker2"
                                                                                ErrorMessage="End date is required!" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </div>
                                                                    <%--<div style="text-align: right; float: left; padding-top: 8px;">
                                                                        <span>
                                                                            <asp:CustomValidator ID="dateCompareValidator" EnableClientScript="true" runat="server"
                                                                                ControlToValidate="RadDatePicker2" ClientValidationFunction="validateStartEndDate"
                                                                                ErrorMessage="End date should be after or equal to the start date!" ForeColor="Red">
                                                                            </asp:CustomValidator>
                                                                        </span>
                                                                    </div>--%>

                                                                    <%--Complete--%>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:Label ID="Label5" Text="Complete" runat="server" Width="85px" />
                                                                            <telerik:RadNumericTextBox runat="server" ID="TextBox3" Width="225" Height="35" ShowSpinButtons="true"
                                                                                IncrementSettings-Step="0.01" Type="Number" MinValue="0" MaxValue="1" NumberFormat-DecimalSeparato=",">
                                                                            </telerik:RadNumericTextBox>
                                                                            <%--<telerik:RadNumericTextBox runat="server" ID="TextBox3" Width="225"  Height="35" ShowSpinButtons="true"
                                                                                IncrementSettings-Step="1" Type="Number" MinValue="0" MaxValue="100" >
                                                                            </telerik:RadNumericTextBox>--%>
                                                                        </div>
                                                                    </div>

                                                                    <%--<div class="input-field col s12">
                                                                        <div class="row" style="margin-bottom:0;">
                                                                            <asp:Label ID="Label6" Text="Complete" runat="server" Width="85px" />
                                                                            <telerik:RadNumericTextBox runat="server" ID="RadNumericTextBox1" Width="225" Height="35" ShowSpinButtons="true"
                                                                                IncrementSettings-Step="0.01" Type="Number" MinValue="0" MaxValue="1" NumberFormat-DecimalSeparato=",">
                                                                            </telerik:RadNumericTextBox>
                                                                        </div>
                                                                    </div>--%>

                                                                    <asp:HiddenField runat="server" ID="UidHiddenField" />
                                                                    <asp:HiddenField runat="server" ID="hdnTaskID" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="up4" class="col s12 tab-container-border lighten-4">
                                                        <div class="tabs-custom-mgn1">
                                                            <div class="form-section-row">
                                                                <div class="form-section">
                                                                    <%--<div class="input-field col s12" style="padding:0;">
                                                                        <div class="row" style="margin-bottom:0;">
                                                                            <div>
                                                                                <label id="lblNotes" for="txtNotes" class="drpdwn-label">Location Address</label>
                                                                            </div>
                                                                            <asp:TextBox TextMode="MultiLine" style="max-height:270px;" CssClass="materialize-textarea" ID="txtNotes" runat="server" placeholder=""
                                                                                ></asp:TextBox>
                                                                        </div>
                                                                    </div>--%>
                                                                    <div class="input-field col s12" style="padding:0;">
                                                                        <div class="row" style="margin-bottom:0;">
                                                                            <asp:TextBox ID="txtNotes" runat="server" MaxLength="8000" 
                                                                                TextMode="MultiLine" Style="padding: 1rem 0.4rem !important;max-height:296px !important;max-width:685px !important;"
                                                                                CssClass="materialize-textarea textarea-border"></asp:TextBox>
                                                                            <label for="txtNotes" class="txtbrdlbl">
                                                                                <asp:Label ID="lblNotes" runat="server" Text="Notes"></asp:Label>
                                                                            </label>
                                                            
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="up2" class="col s12 tab-container-border lighten-4" style="padding:0;">
                                                        <div class="tabs-custom-mgn1">
                                                            <div class="form-section-row" style="margin-bottom: 0;">
                                                                

                                                               <%-- <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                    <asp:LinkButton ID="lnkPOPostback" runat="server" CausesValidation="False" OnClick="lnkPOPostback_Click" Style="display: none"></asp:LinkButton>--%>
                                                                <div class="grid_container">
                                                                    <div class="form-section">
                                                                        <div class="row" style="margin-bottom:0;">
                                                                            <div class="RadGrid RadGrid_Material">
                                                                                <telerik:RadGrid RenderMode="Auto" LoadingPanelID="RadAjaxLoadingPanel_PO" ID="RadGrid_PO" CssClass="RadGrid_PO" ShowFooter="True" PageSize="50"
                                                                                    ShowStatusBar="true" runat="server"
                                                                                    emptydatatext="No PO to display" howFooter="True" showheaderwhenempty="true"
                                                                                    Height="350px" ClientSettings-Scrolling-UseStaticHeaders="true"
                                                                                    ClientSettings-Scrolling-ScrollHeight="350px"
                                                                                    ClientSettings-Scrolling-AllowScroll="true"
                                                                                    
                                                                                    >
                                                                                    <CommandItemStyle />
                                                                                    <GroupingSettings CaseSensitive="false" />
                                                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                    </ClientSettings>
                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false" PagerStyle-AlwaysVisible="true">
                                                                                        <Columns>
                                                                                            <%--<telerik:GridBoundColumn HeaderText="PO #" DataField="PO" SortExpression="PO"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="200"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>--%>
                                                                                            <telerik:GridTemplateColumn HeaderText="PO #" DataField="PO" SortExpression="PO" AutoPostBackOnFilter="true"
                                                                                                CurrentFilterFunction="Contains" UniqueName="PO" ShowFilterIcon="false" HeaderStyle-Width="90" DataType="System.String">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HyperLink ID="lnkPO" runat="server" Text='<%# Eval("PO") %>' Target="_blank" NavigateUrl='<%# "addpo?id=" +Eval("PO")  %>'></asp:HyperLink>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                        
                                                                                            <telerik:GridBoundColumn HeaderText="Vendor" DataField="Vendor" SortExpression="Vendor"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="200"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridBoundColumn HeaderText="Desc" DataField="fdesc" SortExpression="fdesc"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"  HeaderStyle-Width="200"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridBoundColumn HeaderText="Status" DataField="StatusName" SortExpression="StatusName"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridBoundColumn HeaderText="Sales Order #" DataField="SalesOrderNo" SortExpression="SalesOrderNo"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" SortExpression="Amount"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="90"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridBoundColumn HeaderText="PO Amount" DataField="TotalAmount" SortExpression="TotalAmount"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="120"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                                                CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false"
                                                                                                HeaderStyle-Width="100">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblfDate" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> ' runat="server" />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                        </Columns>
                                                                                    </MasterTableView>
                                                                                </telerik:RadGrid>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <%--</telerik:RadAjaxPanel>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="up3" class="col s12 tab-container-border lighten-4" style="padding:0;">
                                                        <div class="tabs-custom-mgn1">
                                                            <div class="row" style="margin-bottom:0;">
                                                                <div class="form-section-row">
                                                                    <div class="col s12" style="padding:0;">
                                                                        <div class="row"  style="margin-bottom:0;">
                                                                            <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" onchange="ConfirmUpload(this.value);" />
                                                                        </div>
                                                                        <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click" Style="display: none">Upload</asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                                
                                                                <div class="form-section-row" style="margin-bottom: 0;">
                                                                    <telerik:RadCodeBlock ID="RadCodeBlock_Documents" runat="server">
                                                                        <script type="text/javascript">
                                                                            function pageLoad() {
                                                                                var grid = $find("<%= RadGrid_Documents.ClientID %>");
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

                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents"  LoadingPanelID="RadAjaxLoadingPanel_PO" PostBackControls="lblName" runat="server"  ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                        <div class="btnlinks">
                                                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return checkdelete();">Delete</asp:LinkButton>
                                                                        </div>
                                                                        <asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" OnClick="lnkPostback_Click" Style="display: none">Postback</asp:LinkButton>
                                                                                
                                                                        <div class="grid_container"  style="margin-top: 10px;">
                                                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" CssClass="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                    PagerStyle-AlwaysVisible="true"
                                                                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="false"
                                                                                    emptydatatext="No PO to display" howFooter="True" showheaderwhenempty="true"
                                                                                    Height="225px" ClientSettings-Scrolling-UseStaticHeaders="true"
                                                                                    ClientSettings-Scrolling-ScrollHeight="225px"
                                                                                    ClientSettings-Scrolling-AllowScroll="true"
                                                                                    >
                                                                                    <CommandItemStyle />
                                                                                    <GroupingSettings CaseSensitive="false" />
                                                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                    </ClientSettings>
                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                                        <Columns>
                                                                                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                                            </telerik:GridClientSelectColumn>

                                                                                            <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                                                    <asp:HiddenField runat="server" ID="hdnTempId" Value='<%# Eval("id").ToString() == "0"? Eval("TempId"): string.Empty %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridTemplateColumn SortExpression="filename" HeaderText="File Name" DataField="filename" ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                                        CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                                        OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                                                    </asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>

                                                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="doctype" HeaderText="File Type" HeaderStyle-Width="140"
                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
                                                                                                ShowFilterIcon="false">
                                                                                            </telerik:GridBoundColumn>

                                                                                            <%--<telerik:GridTemplateColumn SortExpression="portal" HeaderText="Portal" DataField="portal" ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>--%>

                                                                                            <%--<telerik:GridTemplateColumn SortExpression="remarks" HeaderText="Remarks" DataField="remarks" ShowFilterIcon="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>--%>

                                                                                        </Columns>
                                                                                    </MasterTableView>
                                                                                </telerik:RadGrid>
                                                                            </div>
                                                                        </div>
                                                                    </telerik:RadAjaxPanel>
                                                                        
                                                                </div>
                                                            </div>
                                                            <div style="clear: both;"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <%--</div>--%>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadGrid RenderMode="Auto" ID="hdnRadGanttTasks" CssClass="hdnRadProject" ShowFooter="True" PageSize="50" Visible="false"
                ShowStatusBar="true" runat="server" Width="100%" 
                OnNeedDataSource="hdnRadGanttTasks_NeedDataSource"
                >
                <CommandItemStyle />
                <GroupingSettings CaseSensitive="false" />
                
                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="TaskID">
                    <Columns>
                        <telerik:GridBoundColumn DataField="TaskID" SortExpression="TaskID" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" UniqueName="TaskID" HeaderText="Task ID" ShowFilterIcon="false" HeaderStyle-Width="100">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Parent ID" DataField="ParentID" SortExpression="ParentID"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Title" DataField="Title" SortExpression="Title"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Start" DataField="Start" SortExpression="Start" DataFormatString="{0:MM/dd/yyyy HH:mm}"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Start" DataField="Start" SortExpression="Start" DataType="System.DateTime"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                            <ItemTemplate>
                                <asp:Label ID="lblStart"  runat="server" Text='<%#Eval("Start")%>' >
                                </asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridBoundColumn HeaderText="End" DataField="End" SortExpression="End" DataFormatString="{0:MM/dd/yyyy HH:mm}"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Duration" DataField="CusDuration" SortExpression="CusDuration"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="% Complete" DataField="PercentComplete" SortExpression="PercentComplete"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Actual Hour" DataField="CusActualHour" SortExpression="CusActualHour"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Vendor" DataField="Vendor" SortExpression="Vendor"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Notes" DataField="Description" SortExpression="Description"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="200"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Dependency" SortExpression="Dependency" UniqueName="Dependency" HeaderText="Dependency"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                            ShowFilterIcon="false">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:HiddenField ID="hdnTempTaskID" runat="server" />
            <asp:HiddenField ID="hdnVendorID" runat="server" />
            <asp:HiddenField ID="hdnIsVendorUpdate" runat="server" />
        </div>
        <div class="clearfix"></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/calculateBusinessDays.js"></script>
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <style>
        @media only screen and (min-height: 895px) {
            #gantt_chart {
                height: 91vh !important;
            }
        }

        @media only screen and (max-height: 894px) and (min-height: 825px) {
            #gantt_chart {
                height: 90vh !important;
            }
        }

        @media only screen and (max-height: 824px) and (min-height: 698px) {
            #gantt_chart {
                height: 88vh !important;
            }

        }

        @media only screen and (max-height: 697px) and (min-height: 657px) {
            #gantt_chart {
                height: 87vh !important;
            }

        }

        @media only screen and (max-height: 657px) and (min-height: 597px) {
            #gantt_chart {
                height: 86vh !important;
            }

        }

        @media only screen and (max-height: 596px) and (min-height: 553px) {
            #gantt_chart {
                height: 85vh !important;
            }

            
        }

        @media only screen and (max-height: 552px) and (min-height: 511px) {
            #gantt_chart {
                height: 83vh !important;
            }

        }

        @media only screen and (max-height: 510px) and (min-height: 461px) {
            #gantt_chart {
                height: 82vh !important;
            }

        }

        @media only screen and (max-height: 460px) {
            #gantt_chart {
                height: 81vh !important;
            }
        }

        #main > .wrapper {
            height: 100% !important;
        }

        #main > .wrapper {
            height: 100% !important;
        }

            #main > .wrapper > #content {
                height: 100% !important;
            }

        #gantt_chart{
            height:100%;
            margin: 0;
            padding: 0;
            font-family:Lato, sans-serif;
            font-size:13px;
        }
        #gantt {
            height: 100% !important;
            margin: 0;
            padding: 0;
            border: none;
            font-family: Lato, sans-serif;
            font-size: 13px;
        }
        div.k-widget.k-window.radSkin_Bootstrap{
            font-family: Lato, sans-serif;
            font-size: 13px;
        }

        /* Hide toolbars during export */
        .k-pdf-export .rgtToolbar {
            display: none;
        }

        .rgtToolbar.rgtFooter {
            height:0px;
        }

        /*.rgtTimelineContent {
            overflow-x: visible
        }

        .rgtTimelineWrapper {
            overflow-x: visible
        }*/
        /*.btnlinksCus {
            position: relative;
            top: 42px;
            left: 0px;
            margin-left: -23px;
            z-index: 1000;
        }
        .btnlinksCus>a{
            padding-bottom:7px;
            padding-top:7px;
        }*/
        .RadGrid_PO > .rgDataDiv{
            height:296px !important;
        }

        .RadGrid_Documents > .rgDataDiv{
            height: 171px !important
        }

        .rgHeaderWrapper{
            border-radius: 5px 5px 0 0;
        }

        .rgFooterWrapper{
            border-radius: 0 0 5px 5px;
        }

        /*.rgHeader{
            margin: 0 5px 0 5px !important;
        }*/
        .dropify-wrapper{
            height:80px !important;
        }
        .dropify-wrapper .dropify-message span.file-icon>p{
            font-size:13px !important;
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
    </style>
    <script>
        function getTaskByUid(uid) {
            var gantt = $find("<%= gantt.ClientID %>");
            var tasks = gantt.get_allTasks();

            for (var i = 0; i < tasks.length; i++) {
                if (tasks[i]._uid === uid) {
                    return tasks[i];
                    break;
                }
            }
            return null;
        }

        function getTaskById(id) {
            var id = parseInt(id);
            var gantt = $find("<%= gantt.ClientID %>");
            var tasks = gantt.get_allTasks();

            for (var i = 0; i < tasks.length; i++) {
                if (tasks[i].get_id() === id) {
                    return tasks[i];
                    break;
                }
            }

            return null;
        }

        function gantt_add(e) {
            setTimeout(function () {
                var gantt = $("#gantt").getKendoGantt();
                gantt.dataSource.read()
                gantt.dependencies.read()
            }, 100);
        }
        function gantt_remove(e) {
            setTimeout(function () {
                var gantt = $("#gantt").getKendoGantt();
                gantt.dataSource.read()
                gantt.dependencies.read()
            }, 100);
        }
        function gantt_dataBinding(e) {
            //alert("dataBinding: ");
        }
        function gantt_dataBound(e) {
            //alert("dataBoun: ");
        }
        function gantt_navigate(e) {
            //alert("navigat: ");
        }
        function dtaa() {
            this.prefixText = null;
            this.con = null;
            this.custID = null;
        }
        function gantt_edit(e) {
            debugger
            setTimeout(function () {
                debugger
                var taskid = e.task.id;
                $("#<%=hdnTempTaskID.ClientID%>").val(taskid);
                if ($('.rgtTreelistContent.radGridContent>table>tbody>tr>td>input[name="vendor"]').length > 0) {
                    $('.rgtTreelistContent.radGridContent>table>tbody>tr>td>input[name="vendor"]').autocomplete({
                        
                        source: function (request, response) {
                            
                            $("#<%=hdnIsVendorUpdate.ClientID%>").val(1);
                            var dtaaa = new dtaa();
                            dtaaa.prefixText = request.term;
                            query = request.term;
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "AccountAutoFill.asmx/GetVendorNameProject",
                                data: JSON.stringify(dtaaa),
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    response($.parseJSON(data.d));
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load vendor name");
                                }
                            });
                        },
                        select: function (event, ui) {
                            var str = ui.item.Name;
                            var strId = ui.item.ID;
                            if (str == "No Record Found!") {
                                $(this).val("");
                                $("#<%=hdnVendorID.ClientID%>").val(0);
                                //$("#<%=hdnIsVendorUpdate.ClientID%>").val(1);
                            }
                            else {
                                $(this).val(str);
                                $("#<%=hdnVendorID.ClientID%>").val(strId);
                                //$("#<%=hdnIsVendorUpdate.ClientID%>").val(1);
                            }
                            return false;
                        },
                        focus: function (event, ui) {
                            var str = ui.item.Name;
                            var strId = ui.item.ID;
                            if (str == "No Record Found!") {
                                $(this).val("");
                                //$("#<%=hdnIsVendorUpdate.ClientID%>").val(1);
                                $("#<%=hdnVendorID.ClientID%>").val(0);
                            }
                            else {
                                $(this).val(str);
                                
                                $("#<%=hdnVendorID.ClientID%>").val(strId);
                            }
                            return false;
                        },
                        minLength: 0,
                        delay: 250
                    }).bind('click', function () { $(this).autocomplete("search"); })
                    $.each($(".radTextbox"), function (index, item) {
                        $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                            var ula = ul;
                            var itema = item;
                            var result_value = item.ID;
                            var result_item = item.Name;
                            var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                            result_item = result_item.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                            if (result_value == 0) {
                                return $("<li></li>")
                                    .data("item.autocomplete", item)
                                    .append("<a>" + result_item + "</a>")
                                    .appendTo(ul);
                            }
                            else {
                                return $("<li></li>")
                                    .data("item.autocomplete", item)
                                    .append("<a>" + result_item + "</a>")
                                    .appendTo(ul);
                            }
                        };
                    });
                }
            });
        }
        function gantt_change(e) {
            var uid = $("div.rgtTreelistContent>table>tbody>tr.radStateSelected").attr("data-uid");
            if (uid != null) {
                var task = getTaskByUid(uid);
                //alert("Editing task: " + task.get_id());
            }
        }
        function gantt_save(e) {
            
            setTimeout(function () {
                var hdnTaskID = $("#<%=hdnTempTaskID.ClientID%>").val();
                var hdnVendorID = $("#<%=hdnVendorID.ClientID%>").val();
                var hdnIsVendorUpdate = $("#<%=hdnIsVendorUpdate.ClientID%>").val();
                
                var task = getTaskById(hdnTaskID);
                if (task != null && hdnIsVendorUpdate == "1") {
                    $("#<%=hdnTempTaskID.ClientID%>").val("");
                    $("#<%=hdnVendorID.ClientID%>").val("");
                    $("#<%=hdnIsVendorUpdate.ClientID%>").val("");
                    if (hdnVendorID == "") hdnVendorID = 0;
                    task.set_vendorId(hdnVendorID);
                }
                var gantt = $("#gantt").getKendoGantt();
                gantt.dataSource.read()
                gantt.dependencies.read()
            }, 100);
        }

        
        $(document).ready(function () {
            reOpenPopup();
        });

        function pageLoad(sender, args) {
            //Dropify Basic
            $('.dropify').dropify();
            // Used events
            var drEvent = $('.dropify-event').dropify();

            drEvent.on('dropify.beforeClear', function (event, element) {
                return confirm("Do you really want to delete \"" + element.filename + "\" ?");
            });

            drEvent.on('dropify.afterClear', function (event, element) {
                alert('File deleted');
            });

            var gantt = $("#gantt").getKendoGantt();//$("#gantt").data("kendoGantt");
            gantt.unbind("save").bind("save", gantt_save);
            gantt.unbind("add").bind("add", gantt_add);
            gantt.unbind("remove").bind("remove", gantt_remove);
            gantt.unbind("edit").bind("edit", gantt_edit);
            //gan.unbind("")tt.bind("change", gantt_change);
            gantt.unbind("navigate").bind("navigate", gantt_navigate);
            gantt.unbind("dataBinding").bind("dataBinding", gantt_dataBinding);
            gantt.unbind("dataBound").bind("dataBound", gantt_dataBound);

            $('.rgtToolbar.rgtFooter').hide();

            var gantt = $find("<%= gantt.ClientID %>");
            $(gantt.get_element()).find(".rgtTimelineContent").unbind("dblclick").bind("dblclick", ".rgtTask", function (e) {
                e.stopPropagation();
                var $element = $(e.target);
                if (!$element.is(".rgtTask")) {
                    $element = $element.parents(".rgtTask").first();
                }
                var task = getTaskByUid($element.attr("data-uid"));
                $get("<%=hdnTaskID.ClientID %>").value = task.get_id();

                document.getElementById('<%= lnkPostback.ClientID %>').click();
                //showDialog(task);
            });
        };

        function reOpenPopup() {
            var wnd = $find("<%= RadWindow2.ClientID%>");
            var taskID = $get("<%=hdnTaskID.ClientID %>").value;
            if (taskID != null && taskID != "" && wnd.IsClosed()) {
                wnd.show();
            }
        }

        function showDialog(task) {
            var wnd = $find("<%= RadWindow2.ClientID%>");
            wnd.show();
            <%--$get("<%=UidHiddenField.ClientID %>").value = task._uid;--%>
            $get("<%=hdnTaskID.ClientID %>").value = task.get_id();

            $get("<%=TextBox2.ClientID %>").value = task.get_title();

            $find("<%=RadDatePicker1.DateInput.ClientID %>").set_value(task.get_start());
            $find("<%=RadDatePicker2.DateInput.ClientID %>").set_value(task.get_end());

            <%--$get("<%=TextBox3.ClientID %>").value = task.get_percentComplete() * 100;--%>
            $get("<%=TextBox3.ClientID %>").value = task.get_percentComplete();
            $get("<%=txtNotes.ClientID %>").value = task.get_description();

            document.getElementById('<%= lnkPostback.ClientID %>').click();

            Materialize.updateTextFields();
        }

        function showDialogById() {
            var hdnTaskID = $("#<%=hdnTaskID.ClientID%>").val();

            var task = getTaskById(hdnTaskID);
            var wnd = $find("<%= RadWindow2.ClientID%>");
            wnd.show();
            <%--$get("<%=UidHiddenField.ClientID %>").value = task._uid;--%>
            <%--$get("<%=hdnTaskID.ClientID %>").value = task.get_id();--%>

            $get("<%=TextBox2.ClientID %>").value = task.get_title();

            $find("<%=RadDatePicker1.DateInput.ClientID %>").set_value(task.get_start());
            $find("<%=RadDatePicker2.DateInput.ClientID %>").set_value(task.get_end());

            <%--$get("<%=TextBox3.ClientID %>").value = task.get_percentComplete() * 100;--%>
            $get("<%=TextBox3.ClientID %>").value = task.get_percentComplete();
            $get("<%=txtNotes.ClientID %>").value = task.get_description();

            

            Materialize.updateTextFields();
        }

        function OnClientSaveClicking(sender, args) {
            var isValid = Page_ClientValidate();
            if (!isValid) {
                args.set_cancel(true);
            }
            else {
                <%--var uidValue = $get("<%=UidHiddenField.ClientID %>").value;--%>
                var taskId =  $get("<%=hdnTaskID.ClientID %>").value

                var titleFieldValue = $get("<%=TextBox2.ClientID %>").value;
                var startDatePicker = $find("<%=RadDatePicker1.DateInput.ClientID %>");
                var endDatePicker = $find("<%=RadDatePicker2.DateInput.ClientID %>");
                var notesFieldValue = $get("<%=txtNotes.ClientID %>").value;

                var startFieldValue = startDatePicker.get_value();
                var endFieldValue = endDatePicker.get_value();

                var percentCompleteFieldValue = $get("<%=TextBox3.ClientID %>").value;

                var newStartDate = new Date(Date.parse(startFieldValue, startDatePicker.get_dateFormat()));
                var newEndDate = new Date(Date.parse(endFieldValue, endDatePicker.get_dateFormat()));

                //var newPercentComplete = parseFloat(percentCompleteFieldValue.replace(",", ".")) / 100;
                var newPercentComplete = parseFloat(percentCompleteFieldValue.replace(",", "."));
                //var task = getTaskByUid(uidValue);
                var task = getTaskById(taskId);
                
                //var curDuration = task.get_cusDuration();
                var curTitle = task.get_title();
                var curStart = task.get_start();
                var curEnd = task.get_end();
                var curPercentComplete = task.get_percentComplete();
                //var curCusActualHour = task.get_cusActualHour();
                var curNote = task.get_description();
                if (curPercentComplete != newPercentComplete) task.set_percentComplete(newPercentComplete);
                //var newActualHour = curDuration * newPercentComplete;
                if (titleFieldValue != curTitle) task.set_title(titleFieldValue);
                if (curNote != notesFieldValue) task.set_description(notesFieldValue);
                if ((curStart - newStartDate) != 0) { task.set_start(newStartDate); }
                if ((curEnd - newEndDate) != 0) task.set_end(newEndDate);
                //if ((curStart - newStartDate) != 0 || (curEnd - newEndDate) != 0) {
                //    var newDuration = CalculateBusinessHours(newStartDate, newEndDate);
                //    if (newDuration != curDuration) {
                //        task.set_cusDuration(newDuration);
                //    }
                //}

                //if ((curStart - newStartDate) != 0) { task.set_start(newStartDate); }
                //if ((curEnd - newEndDate) != 0) task.set_end(newEndDate);
                //if ((curStart - newStartDate) != 0 || (curEnd - newEndDate) != 0) {
                //    var newDuration = CalculateBusinessHours(newStartDate, newEndDate);
                //    if (newDuration != curDuration) {
                //        task.set_cusDuration(newDuration);
                //    }
                //}
                
                //if (curCusActualHour != newActualHour) task.set_cusActualHour(newActualHour);
                
                $find("<%= RadWindow2.ClientID%>").close();
            }
        }

        function validateStartEndDate(sender, args) {
            var RadDatePicker1 = $find("<%= RadDatePicker1.ClientID%>")
            var RadDatePicker2 = $find("<%= RadDatePicker2.ClientID%>")
            var Date1 = new Date(RadDatePicker1.get_selectedDate());
            var Date2 = new Date(RadDatePicker2.get_selectedDate());

            if ((Date2 - Date1) < 0) {
                args.IsValid = false;
            }
        }

        function OnClientCancelClicked(sender) {
            $find("<%= RadWindow2.ClientID%>").close();
        }

        function OnClientDeleteClicked(sender) {
            <%--var uidValue = $get("<%=UidHiddenField.ClientID %>").value;--%>
            //var task = getTaskByUid(uidValue);

            var taskId = $get("<%=hdnTaskID.ClientID %>").value;
            var task = getTaskById(taskId);
            var parentTaskId = task.get_parentId();

            if (parentTaskId) {
                var parentTask = getTaskById(parentTaskId);
                parentTask.get_tasks().remove(task);
            }
            else {
                var gantt = $find("<%= gantt.ClientID %>");
                gantt.get_tasks().remove(task);
            }

            $find("<%= RadWindow2.ClientID%>").close();
        }

        function exportElement() {
            var exp = $find("<%=RadClientExportManager1.ClientID%>");
            exp.exportPDF($telerik.$("div.rgtTimelineWrapper>div.radGrid.rgtTimeline"));
        }

        var $ = $ || $telerik.$;
        function OnClientPdfExporting(sender, args) {
            var elem = sender.get_element();
            var originalWidth = sender.get_width();
            var originalListWidth = sender.get_listWidth();
            var width = //$(elem).find(".rgtTimelineWrapper").width() +
                $(elem).find(".radFauxRows").width();

            sender.set_listWidth($(elem).find(".rgtTimelineWrapper").width())
            sender.set_width(width);

            // http://stackoverflow.com/questions/779379/why-is-settimeoutfn-0-sometimes-useful
            setTimeout(function () {
                sender.set_width(originalWidth);
                sender.set_listWidth(originalListWidth);
            })
        }

        function CalculateBusinessHours(firstDate, secondDate) {
            var dtStart = moment(firstDate);
            var dtEnd = moment(secondDate);

            var StartingHour = 8;
            var EndingHour = 17;
            var luncnStart = 12;
            var luncnEnd = 13;

            // initialze our return value
            var OverAllMinutes = 0.0;

            // start time must be less than end time
            if (dtStart > dtEnd) {
                return OverAllMinutes;
            }

            var ctTempStart = moment(firstDate).startOf('day');
            var ctTempEnd = moment(secondDate).startOf('day');

            debugger
            

            // check if startdate and enddate are the same day
            var bSameDay = ctTempStart === ctTempEnd;
            // calculate the business days between the dates
            //var iBusinessDays = calculateBusinessDays(ctTempStart, ctTempEnd);
            var iBusinessDays = BusinessDaysUntil(ctTempStart, ctTempEnd);

            var ctMaxTime = moment(ctTempStart).add(EndingHour, 'hours');
            var ctMinTime = moment(ctTempStart).add(StartingHour, 'hours');

            var ctTempStart1 = moment(firstDate).startOf('minute');
            var ctTempEnd1 = moment(secondDate).startOf('minute');
            var FirstDaySec = CorrectFirstDayTime(ctTempStart1, ctMaxTime, ctMinTime, luncnStart, luncnEnd);

            var ctMaxTime1 = moment(ctTempEnd).add(EndingHour, 'hours');
            var ctMinTime1 = moment(ctTempEnd).add(StartingHour, 'hours');
            var LastDaySec = CorrectLastDayTime(ctTempEnd1, ctMaxTime1, ctMinTime1, luncnStart, luncnEnd);

            var OverAllSec = 0;
            // now sum-up all values
            if (bSameDay) {
                if (iBusinessDays != 0) {
                    var lunchTimeStart = moment(dtStart).startOf('day').add(luncnStart, 'hours');//new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, luncnStart, 0, 0);
                    var lunchTimeEnd = moment(dtStart).startOf('day').add(luncnEnd, 'hours');
                    var dwBusinessDaySeconds = ctMaxTime.diff(lunchTimeEnd, 'seconds') + lunchTimeStart.diff(ctMinTime, 'seconds');
                    OverAllSec = FirstDaySec + LastDaySec - dwBusinessDaySeconds;
                }
            }
            else {
                if (iBusinessDays > 1) {
                    var iStartDay = dtStart.weekday();
                    if (iStartDay == 0 || iStartDay == 6) iBusinessDays += 1;
                    var iEndDay = dtEnd.weekday();
                    if (iEndDay == 0 || iEndDay == 6) iBusinessDays += 1;

                    OverAllSec = ((iBusinessDays - 2) * 8 * 60 * 60) + FirstDaySec + LastDaySec;
                }
            }
            OverAllMinutes = OverAllSec / 60;

            return OverAllMinutes / 60;
        }

        function BusinessDaysUntil(firstDate, secondDate, bankHolidays) {
            var dtStart = moment(firstDate);
            var dtEnd = moment(secondDate);

            if (dtStart > dtEnd)
                return 0;

            var businessDays = dtEnd.diff(dtStart, 'days') + 1;
            var fullWeekCount = parseInt(businessDays / 7);
            if (businessDays > fullWeekCount * 7) {
                var firstDayOfWeek = 0;
                var lastDayOfWeek = 0;
                if (dtStart.weekday() == 0) firstDayOfWeek = 7
                else firstDayOfWeek = dtStart.weekday();

                if (dtEnd.weekday() == 0) lastDayOfWeek = 7
                else lastDayOfWeek = dtEnd.weekday();

                if (lastDayOfWeek < firstDayOfWeek) lastDayOfWeek += 7;

                if (firstDayOfWeek <= 6) {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }
            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            //foreach(DateTime bankHoliday in bankHolidays)
            //{
            //    DateTime bh = bankHoliday.Date;
            //    if (firstDay <= bh && bh <= lastDay)
            //        --businessDays;
            //}

            return businessDays;
        }

        function CorrectFirstDayTime(ctStart, ctMaxTime, ctMinTime, lunchStart, lunchEnd) {
            debugger
            var daysec = 0;
            var lunchTimeStart = moment(ctStart).startOf('day').add(lunchStart, 'hours');
            var lunchTimeEnd = moment(ctStart).startOf('day').add(lunchEnd, 'hours');
            if (ctMaxTime < ctStart) // start time is after max time
            {
                return 0; // zero seconds for the first day
            }

            var iStartDay = ctStart.weekday();
            if (iStartDay == 0 || iStartDay == 6) {
                return 0;
            }

            //var ctSpan = moment(ctStart).startOf('day');
            //var adHour = 0;
            if (ctStart < ctMinTime) // start time is befor min time
            {
                daysec = ctMaxTime.diff(lunchTimeEnd, 'seconds') + lunchTimeStart.diff(ctMinTime, 'seconds');
            }
            else if (ctStart < lunchTimeStart) {
                daysec = ctMaxTime.diff(lunchTimeEnd, 'seconds') + lunchTimeStart.diff(ctStart, 'seconds');

            }
            else if (ctStart >= lunchTimeStart && ctStart <= lunchTimeEnd) {
                daysec = ctMaxTime.diff(lunchTimeEnd, 'seconds');
            }
            else if (ctStart > lunchTimeEnd) {
                daysec = ctMaxTime.diff(ctStart, 'seconds');
            }
            //daysec = (ctSpan.Days * 24 * 60 * 60) + (ctSpan.Hours * 60 * 60) + (ctSpan.Minutes * 60) + ctSpan.Seconds;
            return daysec;
        }

        function CorrectLastDayTime(ctEnd, ctMaxTime, ctMinTime, lunchStart, lunchEnd) {
            debugger
            var daysec = 0;
            var lunchTimeStart = moment(ctEnd).startOf('day').add(lunchStart, 'hours');
            var lunchTimeEnd = moment(ctEnd).startOf('day').add(lunchEnd, 'hours');
            if (ctMinTime > ctEnd) // start time is after max time
            {
                return 0; // zero seconds for the last day
            }

            var iEndDay = ctEnd.weekday();
            if (iEndDay == 0 || iEndDay == 6) {
                return 0;
            }

            //var ctSpan = moment(ctStart).startOf('day');
            //var adHour = 0;
            if (ctEnd <= lunchTimeStart) // start time is befor min time
            {
                daysec = ctEnd.diff(ctMinTime, 'seconds');
            }
            else if (ctEnd > lunchTimeStart && ctEnd <= lunchTimeEnd) {
                daysec = lunchTimeStart.diff(ctMinTime, 'seconds');
            }
            else if (ctEnd > lunchTimeEnd && ctEnd <= ctMaxTime) {
                daysec = ctEnd.diff(lunchTimeEnd, 'seconds') + lunchTimeStart.diff(ctMinTime, 'seconds');
            }
            else if (ctEnd > ctMaxTime) {
                daysec = ctMaxTime.diff(lunchTimeEnd, 'seconds') + lunchTimeStart.diff(ctMinTime, 'seconds');
            }
            //daysec = (ctSpan.Days * 24 * 60 * 60) + (ctSpan.Hours * 60 * 60) + (ctSpan.Minutes * 60) + ctSpan.Seconds;
            return daysec;
        }

        function ConfirmUpload(value) {
            //
            var filename;
            var fullPath = value;
            if (fullPath) {
                var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                filename = fullPath.substring(startIndex);
                if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                    filename = filename.substring(1);
                }
            }

            if (confirm('Upload ' + filename + '?')) { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
            else { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
        }

        function ViewDocumentClick(hyperlink) {
            var IsView = "Y"<%--document.getElementById('<%= hdnViewDocument.ClientID%>').value;--%>
            if (IsView == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>
</asp:Content>


