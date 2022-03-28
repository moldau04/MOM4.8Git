<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MOMRadWindow.Master" CodeBehind="EmpPayrollDetail.aspx.cs" Inherits="MOMWebApp.EmpPayrollDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <style type="text/css">
        /*/ custom scrollbar /*/
        ::-webkit-scrollbar {
            width: 10px;
        }


        ::-webkit-scrollbar-track {
            background-color: transparent;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #d6dee1;
            border-radius: 20px;
            border: 6px solid transparent;
            /* background-clip: content-box;*/
        }

            ::-webkit-scrollbar-thumb:hover {
                background-color: #a8bbbf;
            }

        .FormGrid .rgDataDiv {
            min-height: 318px;
        }

        .RadGrid_Bootstrap .rgFooter {
            font: 12px/1.42857143 "Helvetica Neue",Helvetica,Arial,sans-serif;
            font-weight: bold;
        }

        .RadGrid_Bootstrap .rgMasterTable, .RadGrid_Bootstrap .rgDetailTable, .RadGrid_Bootstrap .rgGroupPanel table, .RadGrid_Bootstrap .rgCommandRow table, .RadGrid_Bootstrap .rgEditForm table, .RadGrid_Bootstrap .rgPager table {
            font: 12px/1.42857143 "Helvetica Neue",Helvetica,Arial,sans-serif !important;
        }

        .RadGrid_Bootstrap .rgFooter td {
            padding: 7px 0px 0px 7px !important;
        }

            .RadGrid_Bootstrap .rgFooter td:nth-child(n) {
                padding: 7px 0px 0px 15px !important;
            }

            .RadGrid_Bootstrap .rgFooter td:last-child {
                padding: 7px 12px 0px 15px !important;
            }

        .RadGrid_Material thead th:first-child {
            padding: 7px 0px 0px 15px !important;
        }

        .RadGrid_Material thead th:last-child {
            padding: 7px 12px 0px 15px !important;
        }

        .RadGrid_Bootstrap .rgAltRow > td:first-child {
            border-left-width: 0;
            padding-left: 8px !important;
        }

        .RadGrid_Bootstrap .rgRow > td:first-child {
            border-left-width: 0;
            padding-left: 8px !important;
        }

        .RadGrid_Bootstrap .rgAltRow > td:last-child {
            border-left-width: 0;
            padding-right: 8px !important;
        }

        .RadGrid_Bootstrap .rgRow > td:last-child {
            border-left-width: 0;
            padding-right: 8px !important;
        }

        .RadGrid_Bootstrap .rgRow [type="text"], .RadGrid_Bootstrap .rgAltRow [type="text"], .RadGrid_Bootstrap .rgEditForm [type="text"] {
            height: 25px !important;
        }

        .rgDataDiv {
            padding-bottom: 10px;
        }

        .main-name {
            width: 31% !important;
            padding-left: 22px !important;
            top: -8px;
        }

            .main-name span:first-of-type {
                color: #1565c0;
            }


        .main-filed {
            width: 20% !important;
            margin: 8px 17px;
        }

        .tabs {
            border-top: 1px solid #46464673 !important;
        }

        .main-filed .width {
            width: 90% !important;
        }

        .tabs {
            border-bottom: 0px;
        }

        .header-css-new {
            padding-right: 0px !important;
        }

        .tabs .indicator {
            background-color: #464646 !important;
        }

        .RadGrid_Bootstrap .rgRow > td > [type="text"], .RadGrid_Bootstrap .rgAltRow > td > [type="text"], .RadGrid_Bootstrap .rgEditForm > td > [type="text"] {
            border-bottom: 0px solid #9e9e9e !important;
            margin-bottom: 0px;
        }

        @media screen and (max-width: 2048px) {

            .rgDataDiv {
                height: 50vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 2304px) {

            .rgDataDiv {
                height: 52vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1920px) {

            .rgDataDiv {
                height: 47vh !important;
            }
        }

        @media screen and (max-width: 1706px) {

            .rgDataDiv {
                height: 42vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1688px) {

            .rgDataDiv {
                height: 42vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1366px) {

            .rgDataDiv {
                height: 50vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }
    </style>
    <style>
        .lbltxtalign {
            text-align: right;
            font-size: 12px;
        }

        .lbltxtalignheader {
            text-align: right;
        }

        .lbltxtalignfooter {
            text-align: right;
            padding-right: 12px !important;
        }

        .RadGrid_Bootstrap .rgRow > td, .RadGrid_Bootstrap .rgAltRow > td {
            /* padding-left: 13px !important; */
            /* padding-right: 13px !important; */
        }

        .RadGrid_Bootstrap .rgRow > td, .RadGrid_Bootstrap .rgAltRow > td, .RadGrid_Bootstrap .rgEditRow > td {
            padding: 0px !important;
        }

        .cols-css {
            width: 14.2%;
            margin: 0px;
            padding: 2px;
        }

        .label-css {
            font-size: 12px;
            color: #105099;
            font-weight: 600;
        }

        .wrapper .main-name .row {
            margin-bottom: 0px !important;
        }

        .tabs .tab a {
            font-size: 12px;
        }


        .table-heading {
            font-size: 12px;
            font-weight: 600;
            color: #fff;
            line-height: 35px;
            margin-right: 0%;
        }

        .RadGrid_Bootstrap .rgHeaderDiv {
            margin-right: 10px !important;
        }

        .rgFooterWrapper .rgFooterDiv {
            margin-right: 10px !important;
        }

        .heading-css .btnlinks {
            float: right !important;
            margin-top: 7px !important;
        }

        .heading-css {
            width: 100%;
            text-align: center;
            border-bottom: 2px solid #1565c0;
            border-top: 2px solid #1565c0;
            margin-bottom: 2px;
            background-color: #1c5fb1;
        }

        .section-ttle2 {
            float: left;
            clear: left;
            font-size: 0.9rem;
            width: 100%;
            border-bottom: 2px solid #1865BE;
            margin-bottom: 2px;
            padding-bottom: 2px;
            color: #222;
            font-weight: normal;
        }

        .RadGrid .rgRow {
            height: 28px;
        }

        .RadGrid .rgAltRow {
            height: 28px;
        }

        .RadGrid_Bootstrap .rgHeader, .RadGrid_Bootstrap .rgHeader a, .rgFooter {
            color: #2e6b89 !important;
            font-weight: bold;
        }

        .tabs .tab {
            line-height: 40px;
        }

        .rgHeader {
            padding-right: 0px !important;
        }

        .RadGrid_Material .rgHeader {
            padding: 5px 0px !important;
        }

        }
    </style>
    <script type="text/javascript">
        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-------$$$$$$$$$$$$$$$ RAD AJAX MANAGER  $$$$$$$$$$$$$$$-----%>

    <telerik:RadAjaxManager ID="RadAjaxManager_SerType" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkEditService">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ServiceTypeWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkAddService">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ServiceTypeWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkUpdateHours">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridPayrollHours" LoadingPanelID="RadAjaxLoadingPanel_EmpPayrollDetail" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Revenues" LoadingPanelID="RadAjaxLoadingPanel_EmpPayrollDetail"  />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deduction" LoadingPanelID="RadAjaxLoadingPanel_EmpPayrollDetail"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>

         <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridPayrollHours">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridPayrollHours" LoadingPanelID="RadAjaxLoadingPanel_EmpPayrollDetail" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Revenues" LoadingPanelID="RadAjaxLoadingPanel_EmpPayrollDetail"  />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Deduction" LoadingPanelID="RadAjaxLoadingPanel_EmpPayrollDetail"  />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>

    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_EmpPayrollDetail" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="container">
        <div style="padding-top: 12px;">
            <div class="row" style="margin-bottom: 0px!important; padding-left: 0; padding-right: 0;">
                <div class="input-field col s3 main-name">
                    <div class="row">
                        <asp:Label ID="lblname" runat="server" Text="" Style="font-weight: 700;"></asp:Label>
                        <br />
                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="lblCityStateZip" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <div class="input-field col s3 main-filed">
                    <div class="row " style="margin-bottom: 0px!important">
                        <asp:TextBox ID="txttotalWages" runat="server" Enabled="false" Style="font-weight: bold; color: #000000bf;"></asp:TextBox>
                        <label for="txttotalWages" style="font-weight: bold; color: #0000009e;">Total Wages</label>
                    </div>
                </div>
                <div class="input-field col s3 3 main-filed">
                    <div class="row " style="margin-bottom: 0px!important">
                        <asp:TextBox ID="txttotaldeductions" runat="server" ReadOnly="true" Style="font-weight: bold; color: #000000bf;"></asp:TextBox>
                        <label for="txttotaldeductions" style="font-weight: bold; color: #0000009e;">Total Deductions</label>
                    </div>
                </div>
                <div class="input-field col s3 3 main-filed">
                    <div class="row " style="margin-bottom: 0px!important">
                        <asp:TextBox ID="txtnetpay" runat="server" ReadOnly="true" Style="font-weight: bold; color: #000000bf;"></asp:TextBox>
                        <label for="txtnetpay" style="font-weight: bold; color: #0000009e;">Net Pay</label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <ul class="tabs tab-demo-active white" id="tabProject" runat="server" style="width: 100%;">
                <li class="tab col s2" id="liVendorCheckhead" runat="server">
                    <a class="white-text waves-effect waves-light active" id="liVendorCheck" href="#dvHours" runat="server">Hours</a>
                </li>
                <li class="tab col s2" id="liPayCheckhead" runat="server">
                    <a class="white-text waves-effect waves-light" id="liPayCheck" href="#dvCheck" runat="server">Check</a>
                </li>
            </ul>
            <div id="dvHours" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
                <div style="">
                    <div class="grid_container" style="box-shadow: none; border: none!important;">
                        <div class="form-section-row" style="margin: auto;">
                            <div class="col heading-css">
                                <span class="table-heading" style="margin-right: -12%;">WAGES</span>
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkBtnUpdateHours" runat="server" OnClick="lnkUpdateHours_Click">Update</asp:LinkButton>
                                </div>
                            </div>
                            <div class="row">
                                <ul class="tabs tab-demo-active white" id="ulHours" runat="server" style="width: 100%;">
                                    <li class="tab col s2" id="liRegular" runat="server">
                                        <a class="white-text waves-effect waves-light active" id="liRegularWage" href="#dvRegularWage" runat="server">Regular Wages</a>
                                    </li>
                                    <li class="tab col s2" id="liOther" runat="server">
                                        <a class="white-text waves-effect waves-light" id="liOtherWage" href="#dvOtherWage" runat="server">Other Wages</a>
                                    </li>
                                </ul>
                                <div id="dvRegularWage" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
                                    <div style="">
                                        <div class="grid_container" style="box-shadow: none; border: none!important;">
                                            <div class="form-section-row" style="margin: auto;">
                                                <%--  <div class="col heading-css">
                                                    <span class="table-heading" style="margin-right: -12%;">Regular</span>
                                                </div>--%>

                                                <%--  <div class="col cols-css">
                                                    <label class="label-css" for="txtHoliday" style="">Holiday(Hr)</label>
                                                    <asp:TextBox ID="txtHoliday" runat="server" Style="height: 1.5rem"></asp:TextBox>
                                                </div>
                                                <div class="col cols-css">
                                                    <label class="label-css" for="txtVacation" style="">Vacation(Hr)</label>
                                                    <asp:TextBox ID="txtVacation" runat="server" Style="height: 1.5rem"></asp:TextBox>
                                                </div>
                                                <div class="col cols-css">
                                                    <label class="label-css" for="txtZone" style="">Zone($)</label>
                                                    <asp:TextBox ID="txtZone" runat="server" Style="height: 1.5rem" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                </div>
                                                <div class="col cols-css">
                                                    <label class="label-css" for="txtReimb" style="">Reimb($)</label>
                                                    <asp:TextBox ID="txtReimb" runat="server" Style="height: 1.5rem" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                </div>
                                                <div class="col cols-css">
                                                    <label class="label-css" for="txtMileage" style="">Mileage</label>
                                                    <asp:TextBox ID="txtMileage" runat="server" Style="height: 1.5rem" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                </div>
                                                <div class="col cols-css">
                                                    <label class="label-css" for="txtBonus" style="">Bonus($)</label>
                                                    <asp:TextBox ID="txtBonus" runat="server" Style="height: 1.5rem" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                </div>
                                                <div class="col cols-css">
                                                    <label class="label-css" for="txtSick" style="">Sick Leave($)</label>
                                                    <asp:TextBox ID="txtSick" runat="server" Style="height: 1.5rem" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                </div>--%>
                                            </div>
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%=RadGridPayrollHours.ClientID %>");
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

                                                    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">--%>
                                                        <telerik:RadGrid ID="RadGridPayrollHours" runat="server" AutoGenerateColumns="False" FilterType="CheckList"
                                                            CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                                                            AllowSorting="true"
                                                            ShowFooter="true"
                                                            EnableViewState="true" OnNeedDataSource="RadGridPayrollHours_NeedDataSource" OnItemDataBound="RadGridPayrollHours_ItemDataBound">
                                                            <AlternatingItemStyle CssClass="oddrowcolor" />
                                                            <ActiveItemStyle CssClass="evenrowcolor" />
                                                            <FooterStyle CssClass="footer" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True" DataKeyNames="ID" HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn HeaderText="ID" SortExpression="ID" DataField="ID" AllowFiltering="false" Visible="false" UniqueName="ID">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDeductionId" runat="server" Style="font-size: 12px;" Text='<%# Bind("ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Description" DataField="fDesc" UniqueName="fDesc" SortExpression="Description" AllowFiltering="false" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDedID" runat="server" Style="font-size: 12px; padding: 5px 0px 5px 9px !important;" Text='<%# Bind("ID") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblDesc" runat="server" Style="font-size: 12px; padding: 5px 0px 5px 9px !important;" Text='<%# Bind("WageName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Regular" UniqueName="Reg" DataField="Reg" HeaderStyle-CssClass="header-css-new" FooterAggregateFormatString="{0:N2}" Aggregate="Sum" SortExpression="Regular" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign header-css-new" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblRegular" runat="server" Text='<%# Bind("Reg") %>' Style="text-align: right; font-size: 12px; padding: 5px 0px !important;" onchange="CalTotalVal(this);"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Overtime" UniqueName="OT" DataField="OT" FooterAggregateFormatString="{0:N2}" Aggregate="Sum" SortExpression="Overtime" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblOvertime" runat="server" Text='<%# Bind("OT") %>' Style="text-align: right; font-size: 12px; padding: 5px 0 !important;" onchange="CalTotalVal(this);"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="1.7" UniqueName="NT" DataField="NT" SortExpression="1.7" FooterAggregateFormatString="{0:N2}" Aggregate="Sum" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lbl17Cat" runat="server" Text='<%# Bind("NT") %>' Style="text-align: right; font-size: 12px; padding: 5px 0 !important;" onchange="CalTotalVal(this);"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="DoubleTime" UniqueName="DT" DataField="DT" SortExpression="DoubleTime" FooterAggregateFormatString="{0:N2}" Aggregate="Sum" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblDoubleTime" runat="server" Style="text-align: right; font-size: 12px; padding: 5px 0 !important;" Text='<%# Bind("DT") %>' onchange="CalTotalVal(this);"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="TravelTime" UniqueName="TT" DataField="TT" SortExpression="TravelTime" FooterAggregateFormatString="{0:N2}" Aggregate="Sum" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblTravelTime" runat="server" Style="text-align: right; font-size: 12px; padding: 5px 0 !important;" Text='<%# Bind("TT") %>' onchange="CalTotalVal(this);"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Ticket#" UniqueName="TotalTicket" DataField="TotalTicket" SortExpression="Ticket#" FooterAggregateFormatString="{0}" Aggregate="Sum" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTicket" runat="server" Style="text-align: right; font-size: 12px; padding: 5px 13px !important;" Text='<%# Bind("TotalTicket") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                            <SelectedItemStyle CssClass="selectedrowcolor" />
                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                            </FilterMenu>
                                                        </telerik:RadGrid>
                                                    <%--</telerik:RadAjaxPanel>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div id="dvOtherWage" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
                                    <%--  <div class="col heading-css">
                                        <span class="table-heading">Other</span>
                                    </div>--%>
                                    <div style="">
                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <%-- <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%=RadGridPayrollHours.ClientID %>");
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
                                                    </telerik:RadCodeBlock>--%>

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid ID="RadGridOtherWage" runat="server" AutoGenerateColumns="False" FilterType="CheckList" CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                                                        AllowSorting="true" ShowFooter="true" EnableViewState="true" OnItemDataBound="RadGridOtherWage_ItemDataBound">
                                                        <AlternatingItemStyle CssClass="oddrowcolor" />
                                                        <ActiveItemStyle CssClass="evenrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True" DataKeyNames="ID" HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn HeaderText="ID" SortExpression="ID" DataField="ID" AllowFiltering="false" Visible="false" UniqueName="ID">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbID" runat="server" Style="font-size: 12px;" Text='<%# Bind("ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Description" DataField="Description" UniqueName="Description" SortExpression="Description" AllowFiltering="false" HeaderStyle-Width="180px" ItemStyle-Width="180px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRateCategoryId" runat="server" Style="font-size: 12px; padding: 5px 0px 5px 9px !important;" Text='<%# Bind("RateCategoryId") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblOtherWageId" runat="server" Style="font-size: 12px; padding: 5px 0px 5px 9px !important;" Text='<%# Bind("ID") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblDescription" runat="server" Style="font-size: 12px; padding: 5px 0px 5px 9px !important;" Text='<%# Bind("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                    </FooterTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="Quantity" DataField="Quantity" HeaderStyle-CssClass="header-css-new" FooterAggregateFormatString="{0:N2}" SortExpression="Quantity" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign header-css-new" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>' Style="text-align: right; font-size: 12px; padding: 5px 0px !important;"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Rate" UniqueName="Rate" DataField="Rate" HeaderStyle-CssClass="header-css-new" FooterAggregateFormatString="{0:N2}" SortExpression="Rate" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign header-css-new" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblRate" runat="server" Text='<%# Bind("Rate") %>' Style="text-align: right; font-size: 12px; padding: 5px 0px !important;"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="Amount" DataField="Amount" DataType="System.Decimal" HeaderStyle-CssClass="header-css-new" FooterAggregateFormatString="{0:N2}" Aggregate="Sum" SortExpression="Amount" AllowFiltering="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign header-css-new" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>' Style="text-align: right; font-size: 12px; padding: 5px 0px !important;"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                        <SelectedItemStyle CssClass="selectedrowcolor" />
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
            </div>
        </div>

        <div id="dvCheck" class="col s12 tab-container-border lighten-4" style="display: block; padding: 1px 0px;">
            <div class="col heading-css">
                <span class="table-heading">CHECK</span>
                <div class="btnlinks">
                    <asp:LinkButton ID="lnkView" runat="server" OnClick="lnkView_Click">View Details</asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col s7" style="width: 60%; padding: 0 0.25rem;">
                    <div class="input-field col s12">
                        <%--<div class="row">--%>
                        <telerik:RadGrid ID="RadGrid_Revenues" runat="server" AutoGenerateColumns="False" FilterType="CheckList"
                            CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                            AllowSorting="true"
                            ShowFooter="true" EnableViewState="true" OnNeedDataSource="RadGrid_Revenues_NeedDataSource" OnItemDataBound="RadGrid_Revenues_ItemDataBound">
                            <AlternatingItemStyle CssClass="oddrowcolor" />
                            <ActiveItemStyle CssClass="evenrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True" DataKeyNames="ID">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Wages" SortExpression="Desc" AllowFiltering="false" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRDesc" runat="server" Style="font-size: 12px;" Text='<%# Bind("fDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Quan" HeaderText="Quan" SortExpression="Quan" AllowFiltering="false" FooterAggregateFormatString="{0:N2}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRQuan" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("Quan") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Rate" HeaderText="Rate($)" SortExpression="Rate" AllowFiltering="false" FooterAggregateFormatString="{0:C}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRRate" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("Rate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Amount" HeaderText="Amount($)" SortExpression="Amount" AllowFiltering="false" FooterAggregateFormatString="{0:C}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRAmt" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("Amount")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="YTDAmount" HeaderText="YTD($)" SortExpression="YTD" AllowFiltering="false" FooterAggregateFormatString="{0:C}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRAmtYTD" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("YTDAmount")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <SelectedItemStyle CssClass="selectedrowcolor" />
                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                            </FilterMenu>
                        </telerik:RadGrid>
                        <%--</div>--%>
                    </div>
                </div>

                <div class="col s5" style="width: 40%; padding: 0 0.25rem;">
                    <%--<div class="section-ttle2"><strong>DEDUCTIONS</strong> </div>--%>
                    <div class="input-field col s12">
                        <%--<div class="row">--%>
                        <telerik:RadGrid ID="RadGrid_Deduction" runat="server" AutoGenerateColumns="False" FilterType="CheckList"
                            CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                            AllowSorting="true"
                            ShowFooter="true" EnableViewState="true" OnNeedDataSource="RadGrid_Deduction_NeedDataSource" OnItemDataBound="RadGrid_Deduction_ItemDataBound">
                            <AlternatingItemStyle CssClass="oddrowcolor" />
                            <ActiveItemStyle CssClass="evenrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True" DataKeyNames="ID">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Deductions" SortExpression="Desc" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDDesc" runat="server" Style="font-size: 12px;" Text='<%# Bind("fDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" Text="Total :-"></asp:Label>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Amount" HeaderText="Amount($)" SortExpression="Amount" AllowFiltering="false" FooterAggregateFormatString="{0:C}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDAmt" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("Amount")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="YTDAmount" HeaderText="YTD($)" SortExpression="YTD" AllowFiltering="false" FooterAggregateFormatString="{0:C}" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="lbltxtalign" FooterStyle-HorizontalAlign="Right" FooterStyle-CssClass="lbltxtalignfooter" Aggregate="Sum">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDAmtYTD" runat="server" Style="text-align: right; font-size: 12px;" Text='<%# Bind("YTDAmount")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <SelectedItemStyle CssClass="selectedrowcolor" />
                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                            </FilterMenu>
                        </telerik:RadGrid>
                        <%--</div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script language="javascript">
        function RefreshParent() {
            location.reload(true);
            //window.parent.location.href = window.parent.location.href;
        }
    </script>

    <script type="text/javascript">
        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();
        }
        function onLoad(sender) {
            var div = sender.get_element();
            $telerik.$(".RadComboBoxDropDown").mouseleave(function (e) {
                hideDropDown("#" + sender.get_id(), sender, e);
            });
            $telerik.$(div).mouseleave(function (e) {
                hideDropDown(".RadComboBoxDropDown", sender, e);
            });

        }
        function hideDropDown(selector, combo, e) {
            var tgt = e.relatedTarget;
            var parent = $telerik.$(selector)[0];
            var parents = $telerik.$(tgt).parents(selector);

            if (tgt != parent && parents.length == 0) {
                if (combo.get_dropDownVisible())
                    combo.hideDropDown();
            }
        }

        /////////////////// To calculate Total and to make Gridview Amount Value to 2 decimal ////////////
        function CalTotalVal(obj) {
            var txt = obj.id;
            var lblRegular;
            var lblOvertime;
            var lbl17Cat;
            var lblDoubleTime;
            var lblTravelTime;

            if (txt.indexOf("Regular") >= 0) {
                lblRegular = document.getElementById(txt);
                lblOvertime = document.getElementById(txt.replace('lblRegular', 'lblOvertime'));
                lbl17Cat = document.getElementById(txt.replace('lblRegular', 'lbl17Cat'));
                lblDoubleTime = document.getElementById(txt.replace('lblRegular', 'lblDoubleTime'));
                lblTravelTime = document.getElementById(txt.replace('lblRegular', 'lblTravelTime'));
            }
            else if (txt.indexOf("Overtime") >= 0) {
                lblOvertime = document.getElementById(txt);
                lblRegular = document.getElementById(txt.replace('lblOvertime', 'lblRegular'));
                lbl17Cat = document.getElementById(txt.replace('lblOvertime', 'lbl17Cat'));
                lblDoubleTime = document.getElementById(txt.replace('lblOvertime', 'lblDoubleTime'));
                lblTravelTime = document.getElementById(txt.replace('lblOvertime', 'lblTravelTime'));
            }
            else if (txt.indexOf("17Cat") >= 0) {
                lbl17Cat = document.getElementById(txt);
                lblRegular = document.getElementById(txt.replace('lbl17Cat', 'lblRegular'));
                lblOvertime = document.getElementById(txt.replace('lbl17Cat', 'lblOvertime'));
                lblDoubleTime = document.getElementById(txt.replace('lbl17Cat', 'lblDoubleTime'));
                lblTravelTime = document.getElementById(txt.replace('lbl17Cat', 'lblTravelTime'));
            }
            else if (txt.indexOf("DoubleTime") >= 0) {
                lblDoubleTime = document.getElementById(txt);
                lblOvertime = document.getElementById(txt.replace('lblDoubleTime', 'lblOvertime'));
                lbl17Cat = document.getElementById(txt.replace('lblDoubleTime', 'lbl17Cat'));
                lblRegular = document.getElementById(txt.replace('lblDoubleTime', 'lblRegular'));
                lblTravelTime = document.getElementById(txt.replace('lblDoubleTime', 'lblTravelTime'));
            }
            else if (txt.indexOf("TravelTime") >= 0) {
                lblTravelTime = document.getElementById(txt);
                lblOvertime = document.getElementById(txt.replace('lblTravelTime', 'lblOvertime'));
                lbl17Cat = document.getElementById(txt.replace('lblTravelTime', 'lbl17Cat'));
                lblDoubleTime = document.getElementById(txt.replace('lblTravelTime', 'lblDoubleTime'));
                lblRegular = document.getElementById(txt.replace('lblTravelTime', 'lblRegular'));
            }

            //if (!jQuery.trim($(txtGvQuan).val()) == '') {

            //}

            if (isNaN(parseFloat($(lblRegular).val()))) {
                $(lblRegular).val('0.00');
            } else if (parseFloat($(lblRegular).val()) == 0) {
                $(lblRegular).val('0.00');
            }

            if (isNaN(parseFloat($(lblOvertime).val()))) {
                $(lblOvertime).val('0.00');
            } else if (parseFloat($(lblOvertime).val()) == 0) {
                $(lblOvertime).val('0.00');
            }

            if (isNaN(parseFloat($(lbl17Cat).val()))) {
                $(lbl17Cat).val('0.00');
            } else if (parseFloat($(lbl17Cat).val()) == 0) {
                $(lbl17Cat).val('0.00');
            }

            if (isNaN(parseFloat($(lblDoubleTime).val()))) {
                $(lblDoubleTime).val('0.00');
            } else if (parseFloat($(lblDoubleTime).val()) == 0) {
                $(lblDoubleTime).val('0.00');
            }

            if (isNaN(parseFloat($(lblTravelTime).val()))) {
                $(lblTravelTime).val('0.00');
            } else if (parseFloat($(lblTravelTime).val()) == 0) {
                $(lblTravelTime).val('0.00');
            }
            CalculateTotalAmt();
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function CalculateTotalAmt() {

            var totallblRegular = 0.00;
            var totallblOvertime = 0.00;
            var totallbl17Cat = 0.00;
            var totallblDoubleTime = 0.00;
            var totallblTravelTime = 0.00;

            $("[id*=lblRegular]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totallblRegular = totallblRegular + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotallblRegular]').text(totallblRegular.toFixed(2));

            $("[id*=lblOvertime]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totallblOvertime = totallblOvertime + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotallblOvertime]').text(totallblOvertime.toFixed(2));

            $("[id*=lbl17Cat]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totallbl17Cat = totallbl17Cat + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotallbl17Cat]').text(totallbl17Cat.toFixed(2));

            $("[id*=lblDoubleTime]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totallblDoubleTime = totallblDoubleTime + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotallblDoubleTime]').text(totallblDoubleTime.toFixed(2));

            $("[id*=lblTravelTime]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totallblTravelTime = totallblTravelTime + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotallblTravelTime]').text(totallblTravelTime.toFixed(2));
        }

        function isDecimalKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }
    </script>


</asp:Content>
