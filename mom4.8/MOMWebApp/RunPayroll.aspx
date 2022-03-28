<%@ Page Title="Run Payroll || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="RunPayroll" CodeBehind="RunPayroll.aspx.cs" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style type="text/css">
        /*/ custom scrollbar /*/
        ::-webkit-scrollbar {
            width: 10px;
        }

        .p-l-141 {
            padding-left: 197px;
        }

        .p-l-270 {
            padding-left: 328px;
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

        input[type=text], input[type=password], input[type=email], input[type=url], input[type=time], input[type=date], input[type=datetime-local], input[type=tel], input[type=number], input[type=search], textarea.materialize-textarea {
            margin: 8px 12px 5px 0 !important;
            height: 2rem;
            font-size: 12px;
        }

        select {
            font-size: 12px !important;
        }

        .trost {
            font-size: 12px;
        }

        .m-t-12 {
            margin-top: 7px !important;
        }

        .p-l-220 {
            padding-left: 220px;
        }

        .drpdwn-label-css {
            padding-left: 270px;
        }

        .rgHeader a {
            font-size: 12px !important;
        }

        .RadGrid_Material .rgRow > td > a, .RadGrid_Material .rgAltRow > td > a {
            font-size: 12px !important;
        }

        .RadGrid .rgRow > td, .RadGrid_Material .rgAltRow > td {
            font-size: 12px !important;
        }

        .srchpane {
            padding: 23px 15px 0px 15px;
        }

        .RadGrid_Bootstrap .rgHeaderDiv {
            margin-right: 10px !important;
        }

        .rgFooterWrapper .rgFooterDiv {
            margin-right: 10px !important;
        }

        .RadGrid .RadAjaxPanel .rgDataDiv {
            height: 500px !important;
        }

        .RadGrid .rgFilterRow > td {
            margin: 0px 0px;
            padding: 0px 0px;
        }

        .RadGrid .rgFilterRow > th {
            margin: 0px 0px;
            padding: 0px 0px;
        }

        .m-t-12 {
            margin-top: 12px !important;
        }

        #ck-button {
            margin: 4px;
            background-color: #EFEFEF;
            border-radius: 4px;
            border: 1px solid #D0D0D0;
            overflow: auto;
            float: left;
        }

            #ck-button label {
                float: left;
                width: 12.0em;
            }

                #ck-button label span {
                    text-align: center;
                    padding: 3px 0px;
                    display: block;
                }

                #ck-button label input {
                    position: absolute;
                    top: -20px;
                }

            #ck-button input:checked + span {
                background-color: #911;
                color: #fff;
            }

        input[type=search], textarea.materialize-textarea {
            font-size: 12.5px !important;
        }

        /* select {
            font-size: 12.5px !important;
        }*/

        .form-section-row {
            margin-bottom: 0px !important;
        }
    </style>
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
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Run Payroll</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <%-- <div class="btnlinks">
                                                <asp:LinkButton ID="lnksubmit" runat="server" ToolTip="Save" CausesValidation="true" OnClientClick="disableButton(this,''); javascript:return ConfirmRef(this); itemJSON();" OnClick="lnksubmit_Click">Save</asp:LinkButton>
                                            </div>--%>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkGetTicketTime" runat="server" ToolTip="Get Time From Ticket" CausesValidation="true" OnClientClick="disableButton(this,'');" OnClick="lnkGetTicketTime_Click">Get Time From Ticket</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkProcessPayroll" runat="server" ToolTip="Process Payroll" CausesValidation="true" OnClientClick="itemJSON(); OpenBankModal();return false">Process Payroll</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks m-t-12">
                                                <asp:CheckBox ID="chkProcessOtherDeduction" runat="server" />
                                                <label for="chkProcessOtherDeduction">Process Other Deductions</label>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        <%--<asp:LinkButton ID="lnkClose" runat="server"><i class="mdi-content-clear" onclick="javascript:window.location.href='/PayrollList.aspx';"></i></asp:LinkButton>--%>
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
                <div class="form-section-row">
                    <div class="col s12">
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row" style="display: flex">

                                        <label class="">Pay Frequency </label>
                                        <asp:DropDownList ID="ddlPayFrequency" runat="server" CssClass="browser-default width-250"></asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <label for="txtstartDt" class="p-l-141">Start Date</label>
                                        <asp:TextBox ID="txtstartDt" runat="server" CssClass="txtstartDt datepicker_mom " MaxLength="10" Style="width: 33%; margin: 0px 8px 0px 0px;" onblur="validateDatetime();"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvtxtstartDt" runat="server" ControlToValidate="txtstartDt" Display="None" ErrorMessage="Start Date Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vcrfvtxtstartDt" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField" TargetControlID="rfvtxtstartDt"></asp:ValidatorCalloutExtender>
                                        <label for="txtendDt" class="p-l-270">End Date</label>
                                        <asp:TextBox ID="txtendDt" runat="server" CssClass="txtendDt datepicker_mom txtendDt-css" MaxLength="10" onblur="validateDatetime();"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvtxtendDt" runat="server" ControlToValidate="txtendDt" Display="None" ErrorMessage="End Date Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vcrfvtxtendDt" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField" TargetControlID="rfvtxtendDt"></asp:ValidatorCalloutExtender>

                                        <%--  <asp:TextBox ID="txtweek" runat="server" class="txtweek-css" ></asp:TextBox>
                                        <label for="txtweek" class="p-l-270" ">Week #</label>
                                        <asp:RequiredFieldValidator ID="rfvtxtweek" runat="server" ControlToValidate="txtweek" Display="None" ErrorMessage="Week # Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vcrfvtxtweek" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField" TargetControlID="rfvtxtweek"></asp:ValidatorCalloutExtender>--%>
                                    </div>
                                </div>
                            </div>

                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row" style="display: flex">
                                        <asp:TextBox ID="txtperioddesc" class="w-55" runat="server" ReadOnly="true"></asp:TextBox>
                                        <label for="txtperioddesc">Period Description</label>
                                        <asp:RequiredFieldValidator ID="rfvtxtperioddesc" runat="server" ControlToValidate="txtperioddesc" Display="None" ErrorMessage="Period Description Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vcrfvtxtperioddesc" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField" TargetControlID="rfvtxtperioddesc"></asp:ValidatorCalloutExtender>
                                        &nbsp; &nbsp;
                                         <label class="drpdwn-label drpdwn-label-css">Get Time Method</label>
                                        <asp:DropDownList ID="ddlGetTimeMethod" runat="server" CssClass="browser-default w-44p">
                                            <asp:ListItem Text="Date Range Only" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Payroll Check & Date Range" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Payroll Check Only" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row" style="display: flex">
                                        <label class="drpdwn-label p-l-19">Supervisor</label>
                                        <asp:DropDownList ID="ddlSuper" runat="server" CssClass="browser-default ddlSuper-css">
                                        </asp:DropDownList>
                                        <label class="drpdwn-label p-l-220">Department</label>
                                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="browser-default ddlSuper-css1"></asp:DropDownList>
                                        <div class="srchinputwrap " style="padding-left: 16px;">
                                            <div class="btnlinksicon">
                                                <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="cf"></div>
                    </div>
                    <div class="cf"></div>
                </div>
                <div class="col lblsz2 lblszfloat ibf-css">
                    <div class="row">
                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive" Style="display: none;"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All</asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                        </span>
                    </div>
                    <div class="row">
                        <span class="tro trost">
                            <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="grid_container" style="width: 99%!important; margin: 0px 7px;">
        <div class="row m-b-0">
            <div class="col" style="width: 45%; padding-left: 0; padding-right: 0;">
                <telerik:RadAjaxManager ID="RadAjaxManager_WageDeduction" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" LoadingPanelID="RadAjaxLoadingPanel_RunPayroll" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_RunPayroll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" LoadingPanelID="RadAjaxLoadingPanel_RunPayroll" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="btnGetDetail">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="shdnEmpID" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="ddlBank">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="txtcheck" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkprintchecktemp">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadWindowBank" />
                                <telerik:AjaxUpdatedControl ControlID="RadWindowTemplates" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="imgPrintTemp1">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadWindowBank" />
                                <telerik:AjaxUpdatedControl ControlID="RadWindowTemplates" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_RunPayroll" runat="server">
                </telerik:RadAjaxLoadingPanel>
                <div class="RadGrid RadGrid_Material">
                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_RunPayroll" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" FilterType="CheckList"
                        OnNeedDataSource="RadGrid_RunPayroll_NeedDataSource" OnExcelMLExportRowCreated="RadGrid_RunPayroll_ExcelMLExportRowCreated"
                        OnItemCreated="RadGrid_RunPayroll_ItemCreated" OnItemEvent="RadGrid_RunPayroll_ItemEvent" PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_RunPayroll_PreRender" 
                        OnItemDataBound="RadGrid_RunPayroll_ItemDataBound" ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="false">
                        <CommandItemStyle />
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="True"></Selecting>
                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                            <%--<ClientEvents OnRowDataBound="rowDataBound" />--%>
                        </ClientSettings>
                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="27" ShowFilterIcon="false" UniqueName="ClientSelectColumn">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="False" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" AutoPostBack="false" />
                                        <asp:HiddenField ID="hdnpmethod" Value='<%# Bind("pmethod") %>' runat="server" />
                                        <asp:HiddenField ID="hdnphour" Value='<%# Bind("phour") %>' runat="server" />
                                        <asp:HiddenField ID="hdnsalary" Value='<%# Bind("salary") %>' runat="server" />
                                        <asp:HiddenField ID="hdnpay" Value='<%# Bind("pay") %>' runat="server" />
                                        <asp:HiddenField ID="hdnusertype" Value='<%# Bind("usertype") %>' runat="server" />
                                        <asp:HiddenField ID="hdnuserid" Value='<%# Bind("userid") %>' runat="server" />
                                        <asp:HiddenField ID="hdnid" Value='<%# Bind("ID") %>' runat="server" />
                                        <asp:HiddenField ID="hdnName" Value='<%# Bind("Name") %>' runat="server" />
                                        <asp:HiddenField ID="hdnOtherE" Value='<%# Bind("OtherE") %>' runat="server" />
                                        <asp:HiddenField ID="hdnsicktime" Value='<%# Bind("sicktime") %>' runat="server" />
                                        <asp:HiddenField ID="hdntotal" Value='<%# Bind("total") %>' runat="server" />
                                        <asp:HiddenField ID="hdnHourlyRate" Value='<%# Bind("HourlyRate") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblName" Value='<%# Bind("Name") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblMethod" Value='<%# Bind("paymethod") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblReg" Value='<%# Bind("Reg") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblOT" Value='<%# Bind("OT") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblNT" Value='<%# Bind("NT") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblDT" Value='<%# Bind("DT") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblTT" Value='<%# Bind("TT") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblHoliday" Value='<%# Bind("holiday") %>' runat="server" />
                                        <asp:HiddenField ID="hdnlblVac" Value='<%# Bind("vacation") %>' runat="server" />
                                        <asp:HiddenField ID="hdnGeocode" Value='<%# Bind("Geocode") %>' runat="server" />
                                        <asp:HiddenField ID="hdnPayPeriod" Value='<%# Bind("PayPeriod") %>' runat="server" />
                                        <asp:HiddenField ID="hdnFStatus" Value='<%# Bind("FStatus") %>' runat="server" />
                                        <asp:HiddenField ID="hdnFAllow" Value='<%# Bind("FAllow") %>' runat="server" />
                                        <asp:HiddenField ID="hdnZone" Value='<%# Bind("ZONE") %>' runat="server" />
                                        <asp:HiddenField ID="hdnReimb" Value='<%# Bind("reimb") %>' runat="server" />
                                        <asp:HiddenField ID="hdnMileage" Value='<%# Bind("Mileage") %>' runat="server" />
                                        <asp:HiddenField ID="hdnBonus" Value='<%# Bind("Bonus") %>' runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="lblName" FilterDelay="5" DataField="Name" HeaderText="Name" SortExpression="Name"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" ItemStyle-Width="120" HeaderStyle-Width="120">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="lblEmpID" FilterDelay="5" DataField="ref" HeaderText="EmpID" SortExpression="ref"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" ItemStyle-Width="55" HeaderStyle-Width="55">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblref" runat="server" Text='<%# Eval("ref") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="lblusertype" FilterDelay="5" DataField="usertype" HeaderText="UserType" SortExpression="usertype"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" ItemStyle-Width="70" HeaderStyle-Width="70">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblusertype" runat="server" Text='<%# Eval("usertype") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Method" HeaderText="Method" UniqueName="Method" SortExpression="Method"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMethod" runat="server" Text='<%# Eval("paymethod") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Reg_Holi" HeaderText="Reg" UniqueName="Reg" SortExpression="Reg"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReg" runat="server" Text='<%# Eval("Reg_Holi") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="OtherHr" HeaderText="Other" UniqueName="Other" SortExpression="Other"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOther" runat="server" Text='<%# Eval("OtherHr") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="TotalHr" HeaderText="Total." UniqueName="Total" SortExpression="Other"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TotalHr") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="payFrequency" HeaderText="PayFreq." UniqueName="payFrequency" SortExpression="payFrequency"
                                    AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" FilterControlAltText="Filter payFrequency" ItemStyle-Width="85" HeaderStyle-Width="85">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpayFrequency" runat="server" Text='<%# Eval("payFrequency") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="PayrollDetail" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                runat="server" Modal="true" Width="1000" Height="990" OnClientClose="RefreshParentPage">
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindowBank" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                runat="server" Modal="true" Width="500" Height="400">
                                <ContentTemplate>
                                    <div style="margin-top: 15px;">
                                        <div class="form-section-row">
                                            <div class="form-section">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Bank Account </label>
                                                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default" ValidationGroup="Check" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                                            ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                                            ValidationGroup="Check"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="Right"
                                                            TargetControlID="rfvBank" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label for="txtcheckdate">Check Date</label>
                                                        <asp:TextBox ID="txtcheckdate" runat="server" CssClass="txtendDt datepicker_mom validate" MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtcheck" runat="server" MaxLength="19" CssClass="Contact-search"></asp:TextBox>
                                                        <label for="txtcheck">Check No</label>
                                                        <asp:TextBox ID="txtcheckfrom" runat="server" MaxLength="19" CssClass="Contact-search" Style="display: none;"></asp:TextBox>
                                                        <asp:TextBox ID="txtcheckto" runat="server" MaxLength="19" CssClass="Contact-search" Style="display: none;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtcheckmemo" runat="server" CssClass="Contact-search"></asp:TextBox>
                                                        <label for="txtcheckmemo">Memo on Check</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <footer class="footer-css-run">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkprintchecktemp" runat="server" OnClick="lnkprintchecktemp_Click">Print Checks</asp:LinkButton>
                                            </div>
                                        </footer>
                                    </div>
                                </ContentTemplate>
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindowTemplates" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="400"
                                runat="server" Modal="true" Width="400" Title="Check Templates">
                                <ContentTemplate>
                                    <div>
                                        <div class='col s5 cr-title-css'>
                                            <div class='cr-title'>Select a check template.</div>
                                        </div>
                                        <div class='col s5 cr-box-main' style="width: 100%; float: left; padding-top: 5px; margin-bottom: 15px; margin-right: 30px;">
                                            <div class='cr-box'>
                                                <div class='cr-date'>
                                                    <div class='cr-iocn'>
                                                        <label for="ddlApTopCheckForLoad">Template</label>
                                                        <asp:DropDownList ID="ddlApTopCheckForLoad" runat="server"
                                                            CssClass="browser-default" OnSelectedIndexChanged="ddlApTopCheckForLoad_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <label for="ddlApTopCheckTransType">Transaction Type</label>
                                                        <asp:DropDownList ID="ddlApTopCheckTransType" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Text="All (printed to paper)" Value="0"></asp:ListItem>
                                                            <%--<asp:ListItem Text="Checks Only (to paper)" Value="1"></asp:ListItem>--%>
                                                            <asp:ListItem Text="Direct Deposit only (ACH File)" Value="2"></asp:ListItem>
                                                            <%-- <asp:ListItem Text="Direct Deposit only (Not Balanced)" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Direct Deposit Yellowstone Bank" Value="4"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                        <div class='cr-iocn'>
                                                            <asp:ImageButton ID="imgPrintTemp1" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp1_Click" ToolTip="Export to PDF" />
                                                            <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="25px" Width="25px" OnClick="ImageButton7_Click" ToolTip="Edit Template" />
                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveDefault_Click" ToolTip="Set as Default" />
                                                            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton3_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="clear: both;"></div>
                                        </div>
                                        <div class='col s5 cr-box-main1' style="width: 30%; float: left; padding-top: 5px; margin-bottom: 15px; margin-right: 30px; display: none;">
                                            <div class='cr-box'>
                                                <div class='cr-title'>AP – check middle </div>
                                                <div class='cr-date'>
                                                    <asp:DropDownList ID="ddlApMiddleCheckForLoad" runat="server" CssClass="browser-default">
                                                    </asp:DropDownList>
                                                    <div class='cr-iocn'>
                                                        <asp:ImageButton ID="imgPrintTemp2" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp2_Click" ToolTip="Export to PDF" />
                                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton8_Click" ToolTip="Edit Template" />
                                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveApMiddleCheck_Click" ToolTip="Set as Default" />
                                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton6_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="clear: both;"></div>
                                        </div>
                                        <div class='col s5 cr-box-main1' style="width: 30%; float: left; padding-top: 5px; margin-bottom: 15px; margin-right: 30px; display: none;">
                                            <div class='cr-box'>
                                                <div class='cr-title'>AP – Detailed check top </div>
                                                <div class='cr-date'>
                                                    <asp:DropDownList ID="ddlTopChecksForLoad" runat="server" CssClass="browser-default">
                                                    </asp:DropDownList>
                                                    <div class='cr-iocn'>
                                                        <asp:ImageButton ID="imgPrintTemp6" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp6_Click" ToolTip="Export to PDF" />
                                                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton9_Click" ToolTip="Edit Template" />
                                                        <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkTopChecks_Click" />
                                                        <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton14_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="btnSave2" runat="server" Visible="false" ValidationGroup="Check" CausesValidation="true">
                                        </asp:LinkButton>
                                        <asp:Label ID="txtMessage" runat="server" ForeColor="Green"></asp:Label>
                                    </div>
                                    <div id="loaders" class="loaders-css1">
                                        <img src="images/ajax-loader-trans.gif" style="height: 30px;" />
                                    </div>
                                </ContentTemplate>
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindowFirstReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                runat="server" Modal="true" Width="1200" Height="700">
                                <ContentTemplate>
                                    <div class="rptSti">
                                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner1" runat="server" OnSaveReport="StiWebDesigner1_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner1_SaveReportAs" OnExit="StiWebDesigner1_Exit" />
                                    </div>
                                </ContentTemplate>
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindowSecondReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                runat="server" Modal="true" Width="1200" Height="700">
                                <ContentTemplate>
                                    <div class="rptSti">
                                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner2" runat="server" OnSaveReport="StiWebDesigner2_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner2_SaveReportAs" OnExit="StiWebDesigner2_Exit" />
                                    </div>
                                </ContentTemplate>
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindowThirdReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                runat="server" Modal="true" Width="1200" Height="700">
                                <ContentTemplate>
                                    <div class="rptSti">
                                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner3" runat="server" OnSaveReport="StiWebDesigner3_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner3_SaveReportAs" OnExit="StiWebDesigner3_Exit" />
                                    </div>
                                </ContentTemplate>
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
            </div>
            <div class="col second-ifram" style="width: 55%; padding-top: 5px; padding-left: 0; padding-right: 0;">
                <div id="EmpPayrollDetail" class="iframe-css">
                    <iframe id="iframePayrollDetail" style="border: none!important; width: 100%!important; height: 594px!important"></iframe>
                </div>
            </div>
        </div>
    </div>
    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDedcutions" Value="Y" />
    <asp:HiddenField ID="hdnGLItem" runat="server" />
    <asp:HiddenField runat="server" ID="shdnEmpID" />
    <asp:Button ID="btnGetDetail" runat="server" OnClick="btnGetDetail_Click" CausesValidation="false" Style="display: none;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="js/commonAPI.js"></script>
    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
        <script type="text/javascript">
            function pageLoad() {
                var grid = $find("<%= RadGrid_RunPayroll.ClientID %>");
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

    <script type="text/javascript">
       <%-- function checkPayrollSession() {
            var payrollId =<%: Session["userId"] %>;
        }--%>
        $(function () {

            $("[id*=txtstartDt]").change(function () {
                var startDate = $("[id*=txtstartDt]").val();
                var endDate = $("[id*=txtendDt]").val();
                $("[id*=txtperioddesc]").val('Period of ' + startDate + ' to ' + endDate);
            });
            $("[id*=txtendDt]").change(function () {
                var startDate = $("[id*=txtstartDt]").val();
                var endDate = $("[id*=txtendDt]").val();
                $("[id*=txtperioddesc]").val('Period of ' + startDate + ' to ' + endDate);
            });

            $("[id*=chkSelectAll]").change(function () {
                var ret = $(this).prop('checked');
                $("#<%=RadGrid_RunPayroll.ClientID %>").find('tr:not(:first, :last)').each(function () {
                     var $tr = $(this);
                     var chk = $tr.find('input[id*=chkSelect]');
                     if (ret == true) {
                         $tr.css('background-color', '#c3dcf8');
                         chk.prop('checked', true);
                         CheckSelect();
                     }
                     else {
                         chk.prop('checked', false);
                         $tr.removeAttr("style");
                         checkUnselect();
                     }
                     var ch_id = chk.attr('id');
                     if (ch_id != undefined) {
                         if (ch_id != "ctl00_ContentPlaceHolder1_RadGrid_RunPayroll_ctl00_ctl02_ctl00_chkSelectAll") {
                             CalGrid(ch_id);
                         }
                     }
                 })
             });
            $("[id*=chkSelect]").change(function () {
                var ck = $(this).prop('checked');
                if (ck == true) {
                    CheckSelect();
                }
                else if (ck == false) {
                    checkUnselect();
                }
            });

            $("[id*=txtPay]").change(function () {
                var txtPay = $(this).attr('id');
                var hdnDedID = document.getElementById(txtPay.replace('txtPay', 'hdnDedID'));
                var hdnBalance = document.getElementById(txtPay.replace('txtPay', 'hdnBalance'));
                var lblDue = document.getElementById(txtPay.replace('txtPay', 'lblBalance'));
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var pay = $(this).val().toString().replace(/[\$\(\),]/g, '');
                if (pay == '') {
                    pay = 0;
                    $(this).val('$0.00')
                }
                var DueAmt = parseFloat($(hdnBalance).val().replace(/[\$\(\),]/g, ''));
                var PayAmount = parseFloat(pay);
                var DueAmount = parseFloat($(hdnBalance).val().replace(/[\$\(\),]/g, ''));
                if (parseFloat(PayAmount) > parseFloat(DueAmount)) {
                    noty({
                        text: 'OverPayment is not allowed.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: true,
                        timeout: false,
                        theme: 'noty_theme_default',
                        closable: false
                    });
                    pay = 0;
                    $(lblDue).text(cleanUpCurrency('$' + DueAmount.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    total = parseFloat(pay);
                    if (total == 0) {
                        $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        $(chk).prop('checked', true);
                        SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
                    }
                    else {
                        $(chk).prop('checked', false);
                        $(this).closest('tr').removeAttr("style");
                    }
                }
                else {
                    $(lblDue).text(cleanUpCurrency('$' + parseFloat(DueAmt - PayAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    total = parseFloat(PayAmount);
                    if (total != 0) {
                        $(this).val(PayAmount.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        $(chk).prop('checked', true);
                        SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
                    }
                    else {
                        $(chk).prop('checked', false);
                        $(this).closest('tr').removeAttr("style");
                    }
                }
            });
            $("[id*=chkSelect]").change(function () {
                try {
                    var chk = $(this).attr('id');
                    var hdnDedID = document.getElementById(chk.replace('chkSelect', 'hdnDedID'));
                    var hdnBalance = document.getElementById(chk.replace('chkSelect', 'hdnBalance'));
                    var lblDue = document.getElementById(chk.replace('chkSelect', 'lblBalance'));
                    var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPay'));
                    var pay = $(txtPay).val().toString().replace(/[\$\(\),]/g, '');
                    var dueAmt = parseFloat($(hdnBalance).val().toString().replace(/[\$\(\),]/g, ''))
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                    var pay = 0;
                    var rpay = pay.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    var rprevDue = due.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    if ($(this).prop('checked') == true) {
                        $(txtPay).val(rprevDue)
                        $(lblDue).text(cleanUpCurrency('$' + rpay))
                        SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
                    }
                    else if ($(this).prop('checked') == false) {
                        $(txtPay).val(rpay)

                        $(lblDue).text(cleanUpCurrency('$' + dueAmt))
                        $(this).closest('tr').removeAttr("style");
                    }
                } catch (e) {
                }
            });
        });

        function SelectedRowStyle(gridview) {
            var grid = document.getElementById(gridview);
            $('#' + gridview + ' tr').each(function () {
                var $tr = $(this);
                var chk = $tr.find('input[id*=chkSelect]');
                if (chk.prop('checked') == true) {
                    $tr.css('background-color', '#c3dcf8');
                    $tr.css('font-weight', 'bold');
                }
            })
        }
        function cleanUpCurrency(s) {
            var expression = '-';
            if (s.match(expression)) {
                return s.replace("$-", "\($") + ")";
            }
            else {
                return s;
            }
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
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
        (function ($) {
            $.extend({
                toDictionary: function (query) {
                    var parms = {};
                    var items = query.split("&");
                    for (var i = 0; i < items.length; i++) {
                        var values = items[i].split("=");
                        var key1 = decodeURIComponent(values.shift().replace(/\+/g, '%20'));
                        var key = key1.split('$')[key1.split('$').length - 1];
                        var value = values.join("=")
                        parms[key] = decodeURIComponent(value.replace(/\+/g, '%20'));
                    }
                    return (parms);
                }
            })
        })(jQuery);
        (function ($) {
            $.fn.serializeFormJSON = function () {
                var o = [];
                $(this).find('tr:not(:first, :last)').each(function () {
                    var elements = $(this).find('input, textarea, select')
                    if (elements.size() > 0) {
                        var serialized = $(this).find('input, textarea, select').serialize();
                        var item = $.toDictionary(serialized);
                        o.push(item);
                    }
                });
                return o;
            };
        })(jQuery);
        function itemJSON() {
            var rawData = $('#<%=RadGrid_RunPayroll.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnGLItem.ClientID%>').val(formData);
        }
        function disableControl(control) {
            $(control).css("pointer-events", "none");
        }
        function enableControl(control) {
            setTimeout(function () { $(control).css("pointer-events", "all"); }, 1000);
        }
        function ConfirmRef(control) {
            disableControl(control);
        }
        function OnRowSelected(sender, args) {
            var rawData = $('#<%=RadGrid_RunPayroll.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnGLItem.ClientID%>').val(formData);
        }
    </script>

    <script type="text/javascript">
        function validateDatetime() {
            var valueFromDt = new Date($(".txtstartDt").val());
            var valueEndDt = new Date($(".txtendDt").val());
            var str = "From Date cannot be greater than To Date.";
            if (valueFromDt > valueEndDt) {
                noty({ text: str, type: 'error', layout: 'topCenter', closeOnSelfClick: false, timeout: 5000, theme: 'noty_theme_default', closable: true });
            }
        }
        function AddRemitTaxClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddDedcutions.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditRemitTaxClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditDedcutions.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteRemitTaxClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteDedcutions.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_RunPayroll.ClientID%>', 'Vendor');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function clear() {
            ("#ddlType");
        }
        function CalGrid(chk) {
            if ($("#" + chk).prop('checked') == true) {
                SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
            }
            else if ($("#" + chk).prop('checked') == false) {
                $(this).closest('tr').removeAttr("style");
            }
        }
      <%--  function pageLoad(sender, args) {
           $("[id*=chkSelectAll]").change(function () {
                var ret = $(this).prop('checked');
                $("#<%=RadGrid_RunPayroll.ClientID %>").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    var chk = $tr.find('input[id*=chkSelect]');
                    if (ret == true) {
                        $tr.css('background-color', '#c3dcf8');
                        chk.prop('checked', true);
                        CheckSelect();
                    }
                    else {
                        chk.prop('checked', false);
                        $tr.removeAttr("style");
                        checkUnselect();
                    }
                    var ch_id = chk.attr('id');
                    if (ch_id != undefined) {
                        if (ch_id != "ctl00_ContentPlaceHolder1_RadGrid_RunPayroll_ctl00_ctl02_ctl00_chkSelectAll") {
                            CalGrid(ch_id);
                        }
                    }
                })
            });
            $("[id*=chkSelect]").change(function () {
                var ck = $(this).prop('checked');
                if (ck == true) {
                    CheckSelect();
                }
                else if (ck == false) {
                    checkUnselect();
                }
            })
        }--%>

        function CheckSelect() {
            var rawData = $('#<%=RadGrid_RunPayroll.ClientID%>').serializeFormJSON();
            var gridData = JSON.stringify(rawData);
            var frequencyId = $("[id*=ddlPayFrequency]").val();
            var startDate = $("[id*=txtstartDt]").val();
            var endDate = $("[id*=txtendDt]").val();
            var supervisorId = $("[id*=ddlSuper]").val();
            var deparptmentId = $("[id*=ddlDepartment]").val();
            var description = $("[id*=txtperioddesc]").val();
            var processMethod = $("[id*=ddlGetTimeMethod]").val();
            var processOtherDeduction = document.getElementById("<%= chkProcessOtherDeduction.ClientID %>").checked;
            var Model = {
                PayrollData: gridData,
                FrequencyId: frequencyId,
                StartDate: startDate,
                EndDate: endDate,
                SupervisorId: supervisorId,
                DeparptmentId: deparptmentId,
                ProcessOtherDeduction: processOtherDeduction,
                Description: description,
                ProcessMethod: processMethod
            };
            AccessControler.Post(Model, 30000, "./api/PayrollApi/AddPayrollRagister", function (response) {
                if (response != null) {
                    //alert(response.d);
                }
            });
        }

        function checkUnselect() {
            var rawData = $('#<%=RadGrid_RunPayroll.ClientID%>').serializeFormJSON();
            var gridData = JSON.stringify(rawData);
            var Model = {
                PayrollData: gridData
            };
            AccessControler.Post(Model, 30000, "./api/PayrollApi/EditPayrollRagister", function (response) {
                if (response != null) {
                    //alert(response.d);
                }
            });
        }
    </script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

        });

        function OpenPayrollDetailModal(sEmpID, sHoliday, sVac, sZone, sReimb, sMilage, sBonus, sProcessDed, sPeriodDesc, sWeekNo, sSick, ckId, payrollRegisterId) {
            document.getElementById(ckId).checked = true;
            CheckSelect();
            var url = '';
            var iframePayrollDetail = $('#iframePayrollDetail')
            var ProcessDeduction = document.getElementById('<%= chkProcessOtherDeduction.ClientID%>').checked;
            if (ProcessDeduction == false) {
                sProcessDed = 0;
            }
            else {
                sProcessDed = 1;
            }
            var masterTable = $find("<%=RadGrid_RunPayroll.ClientID%>").get_masterTableView();
            var count = masterTable.get_dataItems().length;
            var item;

            if (sEmpID != null) {
                url = "EmpPayrollDetail.aspx?id=" + sEmpID + "&sDt=" + $('#<%=txtstartDt.ClientID%>').val() + "&eDt=" + $('#<%=txtendDt.ClientID%>').val() + "&Super='" + $('#<%=ddlSuper.ClientID%>').val() + "'&sHol=" + sHoliday + "&sVac= " + sVac + "&sZone=" + sZone + "&sReimb=" + sReimb + "&sMilage=" + sMilage + "&sBonus=" + sBonus + "&sProcessDed=" + sProcessDed + "&sPeriodDesc=" + sPeriodDesc + "&sWeekNo=" + sWeekNo + "&sSick=" + sSick + "";
            }
            $(iframePayrollDetail).prop('src', url);
        }
        function ClosePayrollDetailModal() {
            var wnd = $find('<%=PayrollDetail.ClientID %>');
            wnd.Close();
        }
        function OpenBankModal() {
            var wnd = $find('<%=RadWindowBank.ClientID %>');
            wnd.set_title("Generate Payroll Checks");
            wnd.Show();
        }
        function CloseBankModal() {
            var wnd = $find('<%=RadWindowBank.ClientID %>');
            wnd.Close();
        }
        function OpentemplateModal() {
            CloseBankModal();
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            //wnd.set_title("Re-Print Check Range");
            wnd.Show();
        }
        function ClosetemplateModal() {
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            wnd.Close();
            //$('html, body').animate({ scrollTop: $('#vendorType').offset().top }, 'slow');
        }
        function AlertModal() {
            $("label[for='txtcheck']").addClass("active");
            //$('html, body').animate({ scrollTop: $('#vendorType').offset().top }, 'slow');
        }
        function RefreshParentPage()//function in parent page
        {
            document.getElementById('<%=lnkSearch.ClientID%>').click();
        }
    </script>

</asp:Content>


