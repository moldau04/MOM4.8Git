<%@ Page Title="Payroll Register || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="PayrollRegister" CodeBehind="PayrollRegister.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style type="text/css">
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
                height: 30vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
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
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Payroll Register</div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click" Style="display: none;">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" Style="display: none;">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks menuAction">
                                                    <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                    </a>
                                                </div>
                                                <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkCopy" runat="server" OnClick="lnkCopy_Click" Style="display: none;">Copy</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkDelete" OnClientClick="return ConfirmDelete();" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                        </div>
                                                    </li>

                                                    <li>
                                                        <ul id="dropdown1" class="dropdown-content">
                                                            <li>
                                                                <asp:LinkButton ID="CheckReport" href="PayRollCheckReport.aspx?type=PayrollRegister" runat="server">Check Report
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="CheckReportByTitle" href="CheckReportByTitle.aspx?type=PayrollRegister" runat="server">Check Report By Title
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="ComprehensiveReport" href="ComprehensiveReport.aspx?type=PayrollRegister" runat="server">Comprehensive Report
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkFederal940Form" href="Federal940Form.aspx?type=PayrollRegister" runat="server">Federal 940 Form
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="LinkButton1" href="PayrollDeductionSummaryByOtherDeductionReport.aspx?type=PayrollRegister" runat="server">Payroll - Deduction Summary By Other Deduction Report
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="LinkButton2" href="PayrollGLCrossReferenceReport.aspx?type=PayrollRegister" runat="server">Payroll Register GL Cross-Reference Report
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkPayrollliabilityreport" href="PayrollLiabilityReport.aspx?type=PayrollRegister" runat="server">Payroll Liability Report
                                                                </asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                        <div class="btnlinks">
                                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                                            </a>
                                                        </div>
                                                    </li>

                                                </ul>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnReprintRange" runat="server" OnClientClick="OpenReprintCheckRangeModal();return false">
                                                     Reprint Check 
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="btnclosewrap">
                                                <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                            </div>
                                            <div class="rght-content">
                                            </div>
                                        </div>
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
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth ser-css2">
                        <asp:Label ID="lblStart" runat="server" Text="Date"></asp:Label>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker_mom" Width="100px"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker_mom" Width="100px"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnBillsDate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnBillsDate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnBillsDate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnBillsDate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnBillsDate', 'rdCal')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>

                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True" Text="Incl. Closed" Style="display: none;"></asp:CheckBox>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton>
                            </span>
                            <span class="tro trost" style="display: none;">
                                <i class="mdi-social-notifications"></i>
                            </span>
                            <span class="tro trost">
                                <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth ser-css2 " >
                        Employee
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlEmp" runat="server" CssClass="browser-default selectst">
                        </asp:DropDownList>
                    </div>

                    <div class="srchinputwrap">
                        <div class="btnlinksicon">
                            <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <ul class="tabs tab-demo-active white tabProject w-100p" id="tabProject">
                <asp:Repeater ID="rptDepartmentTab" runat="server">
                    <ItemTemplate>
                        <li class="tab col s2 " style="font-size: 13px !important">
                            <a class="waves-effect waves-light prodept" title='<%# Eval("tablabel") %>' id="<%# Eval("ID") %>" onclick='selectTab("<%# Eval("tablabel") %>")'>&nbsp;<%# Eval("tablabel") %></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <br />

            <telerik:RadWindow ID="VoidCheckWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="300">
                <ContentTemplate>
                    <div class="m-t-15">
                        <div class="form-section-row" id="dvVoid" runat="server">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtVoidDate" runat="server" CssClass="datepicker_mom"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvVoidDate" runat="server"
                                            ControlToValidate="txtVoidDate" Display="None" ErrorMessage="Void date is Required"
                                            ValidationGroup="SubCheck" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceVoidDate" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="rfvVoidDate" />
                                        <asp:RegularExpressionValidator ID="rfvVoidDate1" ControlToValidate="txtVoidDate"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vceVoidDate1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvVoidDate1" />
                                        <label>Please enter check Void Date. </label>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <div class="form-section-row" id="dvEditCheck" runat="server">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtCheckNo" runat="server" MaxLength="9" autocomplete="off"
                                            onkeypress="return isNumberKey(event,this)" onchange="IsExistCheckNo();"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCheckNo" runat="server"
                                            ControlToValidate="txtCheckNo" Display="None" ErrorMessage="Check number is Required"
                                            ValidationGroup="Check" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceCheckNo" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="rfvCheckNo" />
                                        <label for="txtCheckNo">Check No. </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>

                        <footer class="footer-pr">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkSave" runat="server" OnClientClick="CloseVoidModal()" ValidationGroup="SubCheck" CausesValidation="true"> Save </asp:LinkButton>
                            </div>
                            <div class="btnlinks">
                                <asp:LinkButton ID="lbtnCheckSave" runat="server" CausesValidation="true" ValidationGroup="Check"> Save </asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="ReprintCheckRange" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="250">
                <ContentTemplate>
                    <div class="m-t-15" >
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Bank Account </label>
                                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default" ValidationGroup="Check">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                            ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                            ValidationGroup="Check"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvBank" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtcheckfrom" runat="server" MaxLength="19" CssClass="Contact-search"></asp:TextBox>
                                        <label for="txtcheckfrom">Starting CheckNo</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtcheckto" runat="server" MaxLength="19" CssClass="Contact-search"></asp:TextBox>
                                        <label for="txtcheckto">Ending CheckNo</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <footer class="footer-css" >
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkVendSave" runat="server" OnClientClick="OpentemplateModal();return false">Print Checks</asp:LinkButton>
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
                            <div class='cr-title-new2'>Select a check template.</div>
                        </div>

                        <div class='col s5 cr-main2'>
                            <div class='cr-box'>
                                <%--<div class='cr-title'>AP – check top </div>--%>


                                <div class='cr-date'>
                                    <div class='cr-iocn'>
                                        <asp:DropDownList ID="ddlApTopCheckForLoad" runat="server"
                                            CssClass="browser-default">
                                        </asp:DropDownList>
                                        <label for="ddlApTopCheckTransType">Transaction Type</label>
                                        <asp:DropDownList ID="ddlApTopCheckTransType" runat="server" CssClass="browser-default">
                                            <asp:ListItem Text="All (printed to paper)" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Checks Only (to paper)" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Direct Deposit only (ACH File)" Value="2"></asp:ListItem>
                                          <%--  <asp:ListItem Text="Direct Deposit only (Not Balanced)" Value="3"></asp:ListItem>
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
                        <div style="clear: both;"></div>
                    </div>
                    <div class="btnlinks">
                        <asp:LinkButton ID="btnSave2" runat="server" Visible="false" ValidationGroup="Check" CausesValidation="true">
                                                                                                   Cut Check
                        </asp:LinkButton>
                        <asp:Label ID="txtMessage" runat="server" ForeColor="Green"></asp:Label>
                    </div>
                    <div id="loaders" class="lodder-new" >
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


            <div class="grid_container">
                <div class="form-section-row pmd-card" >

                    <telerik:RadAjaxManager ID="RadAjaxManager_Bills" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />

                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkCopy">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                                    <telerik:AjaxUpdatedControl ControlID="txtVendorSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="txtRef" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" />

                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="rdobill">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkProcess" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rdoRecurring">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkProcess" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkProcess">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayRegister" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                                    <telerik:AjaxUpdatedControl ControlID="txtVendorSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="txtRef" />

                                </UpdatedControls>
                            </telerik:AjaxSetting>

                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Bills" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_PayRegister.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Bills" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Bills" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_PayRegister" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_PayRegister_NeedDataSource" OnPreRender="RadGrid_PayRegister_PreRender" OnItemEvent="RadGrid_PayRegister_ItemEvent" OnItemCreated="RadGrid_PayRegister_ItemCreated" OnExcelMLExportRowCreated="RadGrid_PayRegister_ExcelMLExportRowCreated">
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
                                        <telerik:GridTemplateColumn UniqueName="lblIndexID" Display="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <%# Container.ItemIndex %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnID" Value='<%# Eval("ID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnSel" Value='<%# Eval("Sel") %>' runat="server" />
                                                <asp:HiddenField ID="hdnStatus" Value='<%# Eval("Status") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="fdate" DataField="fdate" SortExpression="fdate" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDate" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> ' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Ref" DataField="Ref" HeaderText="Ref" SortExpression="Ref"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lblRef" runat="server" Text='<%# Bind("Ref") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="Name" DataField="Name" HeaderText="Payee" SortExpression="Name"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="TInc" DataField="TInc" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="TInc" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Gross" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTInc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TInc", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("TInc"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FIT" DataField="FIT" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FICA" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="FIT" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFIT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FIT", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("FIT"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FICA" DataField="FICA" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FICA" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="FICA" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFica" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FICA", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("FICA"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="MEDI" DataField="MEDI" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FICA" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="MEDI" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMedi" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MEDI", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("MEDI"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="SIT" DataField="SIT" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="SIT" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="SIT" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSIT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SIT", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("SIT"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TOther" DataField="TOther" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="TOther" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Other" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTOther" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TOther", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("TOther"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TDed" DataField="TDed" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="TDed" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Total" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDed" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TDed", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("TDed"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Net" DataField="Net" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Net" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Net" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Net", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("Net"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
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
    </div>
    <asp:HiddenField ID="hdnID" runat="server" />
    <asp:HiddenField ID="hdnTRID" runat="server" />
    <asp:HiddenField ID="hdnBatch" runat="server" />
    <asp:HiddenField runat="server" ID="hdnBillsSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">

        function DeleteBillMsg() {
            noty({
                text: 'Check voided successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function DeleteRecrrBillMsg() {
            noty({
                text: 'Recurring Bill voided successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function closedMesg() {
            noty({
                text: 'This check is not open and can therefore not be voided.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function DatePermissionAlert(mesg) {
            // alert("These month/year period is closed out. You do not have permission to " + mesg + " this record.");
            noty({
                text: 'These month/year period is closed out. You do not have permission to ' + mesg + ' this record.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }

        function ChkWarning() {
            noty({
                text: 'Please select an check to edit check date.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
            return false;
        }
        //delete bill
        function ConfirmDelete() {
            debugger;
            var sel = "0";
            var result = false;
            $("#<%=RadGrid_PayRegister.ClientID %>").find('tr:not(:first,:last)').each(function () {
                debugger;
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    result = true;
                    var hdnID = $tr.find('span[id*=hdnID]');
                    ref = $(hdnID).val();
                    var hdnSel = $tr.find('span[id*=hdnSel]');
                    sel = $(hdnSel).val();
                });
            });
            debugger;
            if (sel == "1") {
                noty({
                    text: 'This check has closed and can therefore not be voided.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            else if (sel == "2") {
                noty({
                    text: 'This check is already voided and can therefore not be voided.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            else {
                if (result == true) {
                    return confirm('Are you sure that you want to void this check ?');
                }
                else {
                    noty({
                        text: 'Please select an check to void.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                    return false;
                }
            }
        }
        function CheckProcess() {
            var result = false;
            $("#<%=RadGrid_PayRegister.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Are you sure you want to process this adjustment?');
            }
            else {
                alert('Please select a Recurring entry to process.')
                return false;
            }
        }
        function isNumberKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (charCode == 45) {
                return true;
            }

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
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#colorNav #dynamicUI li').remove();


        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            debugger;
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblBillActive"])%>';
                var val;
                val = localStorage.getItem("hdnBillsDate");
                if (SesVar == '2') {
                    $("#<%=lblDay.ClientID%>").addClass("");
                    $("#<%=lblWeek.ClientID%>").addClass("");
                    $("#<%=lblMonth.ClientID%>").addClass("");
                    $("#<%=lblQuarter.ClientID%>").addClass("");
                    $("#<%=lblYear.ClientID%>").addClass("");
                }
                else {
                    if (val == 'Day') {
                        $("#<%=lblDay.ClientID%>").addClass("labelactive");
                        document.getElementById("rdDay").checked = true;
                    }
                    else if (val == 'Week') {

                        $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                        document.getElementById("rdWeek").checked = true;
                    }
                    else if (val == 'Month') {

                        $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                        document.getElementById("rdMonth").checked = true;
                    }
                    else if (val == 'Quarter') {

                        $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                        document.getElementById("rdQuarter").checked = true;
                    }
                    else if (val == 'Year') {

                        $("#<%=lblYear.ClientID%>").addClass("labelactive");
                        document.getElementById("rdYear").checked = true;
                    }
                    else {
                        $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                        document.getElementById("rdWeek").checked = true;
                    }
                }
            }
        });
    </script>
    <script type="text/javascript">
        function dec_date(select, txtDateTo, txtDateFrom, rdGroup) {
            var select = select;
            var txtDateTo = txtDateTo;
            var txtDateFrom = txtDateFrom;
            var rdGroup = rdGroup;
            var xday;
            var xWeek;
            var xMonth;
            var xYear;
            var xQuarter;
            if (select == "dec") {
                xday = -1;
                xWeek = -7;
                xMonth = -1;
                xQuarter = -3;
                xYear = -1;
            }
            if (select == "inc") {

                xday = 1;
                xWeek = 7;
                xMonth = 1;
                xQuarter = 3;
                xYear = 1;
            }
            var radio = document.getElementsByName(rdGroup); //Client ID of the RadioButtonList1 
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdWeek';
                }
            }
            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdMonth') {
                //dec the from date

                Date.isLeapYear = function (year) {
                    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
                };

                Date.getDaysInMonth = function (year, month) {
                    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
                };

                Date.prototype.isLeapYear = function () {
                    return Date.isLeapYear(this.getFullYear());
                };

                Date.prototype.getDaysInMonth = function () {
                    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
                };

                Date.prototype.addMonths = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + value);
                    this.setDate(Math.min(n, this.getDaysInMonth()));
                    return this;
                };

                Date.prototype.addMonthsLast = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };

                Date.prototype.addMonthsLastDec = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() - 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateTo).value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById(txtDateTo).value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }
            }


            else if (selected == 'rdQuarter') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                //decrease date range 
                if (select == 'dec') {
                    xQuarter = -3;

                    if (DATE.getMonth() == 11) {
                        NEWDATE.setDate(NEWDATE.getDate() - 1);
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }
                    else if (DATE.getMonth() == 5) {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                        NEWDATE.setDate(NEWDATE.getDate() + 1);

                    }
                    else {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }

                }
                else {
                    xQuarter = 3;
                    NEWDATE.setDate(NEWDATE.getDate() - 1);
                    NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    if (NEWDATE.getMonth() == 11 || NEWDATE.getMonth() == 12 || DATE.getMonth() == 11) {
                        NEWDATE.setDate(31);
                    } else {
                        if (DATE.getMonth() == 5) { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else { NEWDATE.setDate(NEWDATE.getDate()); }

                    }
                }
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }

            return false;

        }
        function SelectDate(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
            var type = type;
            var txtDateFrom = txtDateFrom;
            var txtdateTo = txtdateTo;
            var UniqueVal = UniqueVal;
            var label = label;
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnBillsSelectDtRange.ClientID%>').value = "Day";
            }
            if (type == 'Week') {

                Date.prototype.GetFirstDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay())));
                }

                Date.prototype.GetLastDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay() + 6)));
                }
                var today = new Date();
                var Firstdate = today.GetFirstDayOfWeek();
                var day = Firstdate.getDate();
                var month = Firstdate.getMonth() + 1;
                var year = Firstdate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnBillsSelectDtRange.ClientID%>').value = "Week";
            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);
                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnBillsSelectDtRange.ClientID%>').value = "Month";
            }
            if (type == 'Quarter') {
                var d = new Date();
                var quarter = Math.floor((d.getMonth() / 3));
                var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
                var lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);
                var day = firstDate.getDate();
                var month = firstDate.getMonth() + 1;
                var year = firstDate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnBillsSelectDtRange.ClientID%>').value = "Quarter";
            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);
                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnBillsSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnBillsSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }

        function notyConfirm(ticid) {
            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'This account is inactive. Please change the account name before proceeding.',
                type: 'alert',
                speed: 500,
                timeout: false,
                closeButton: false,
                closeOnSelfClick: true,
                closeOnSelfOver: false,
                force: false,
                onShow: false,
                onShown: false,
                onClose: false,
                onClosed: false,
                buttons: [
                    {
                        type: 'btn-primary', text: 'Yes', click: function ($noty) {
                            $noty.close();
                            //window.open("mailticket.aspx?id=" + ticid + "&c=0", "_blank"); 
                            window.open("addbills.aspx?id=" + ticid + "&t=c", "_self");


                        }
                    },
                    {
                        type: 'btn-danger', text: 'No', click: function ($noty) {
                            $noty.close();
                            //window.open("addticket.aspx?id=" + ticid + "&comp=0&pop=1&fr=tlv", "_self");
                        }
                    }
                ],
                modal: true,
                template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
                cssPrefix: 'noty_',
                custom:
                {
                    container: null
                }
            });
        }
        function selectTab(tablabel) {
            <%--$('#<%=hdnTabId.ClientID%>').val(id);
            $("#hdDept").val(id);--%>
            var grid = $find("<%= RadGrid_PayRegister.ClientID %>");
            if (grid) {
                var tableView = grid.get_masterTableView();
                var filterExpressions = tableView.get_filterExpressions();
                console.log(tableView.get_filterExpressions());
                if (filterExpressions.length > 0) { var expression = filterExpressions[0]; }
                tableView.rebind();
            }
        }
    </script>
    <script type="text/javascript">
        function OpenReprintCheckRangeModal() {
            <%--$('#<%=txtVendorType.ClientID%>').val("");
            $('#<%=txtremarksvendor.ClientID%>').val("");
            $('#<%=txtVendorType.ClientID%>').prop("readonly", false);--%>
            var wnd = $find('<%=ReprintCheckRange.ClientID %>');
            wnd.set_title("Reprint Payroll Check");
            wnd.Show();
        }
        function CloseReprintCheckRangeModal() {
            var wnd = $find('<%=ReprintCheckRange.ClientID %>');
            wnd.Close();

        }
        function OpentemplateModal() {
            debugger;
            var d1 = parseInt($('#<%=txtcheckfrom.ClientID%>').val());
            var d2 = parseInt($('#<%=txtcheckto.ClientID%>').val());

            if (d2 < d1) {
                noty({
                    text: 'Ending checkno is not less then starting checkno.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return;
            }
            CloseReprintCheckRangeModal();
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            //wnd.set_title("Re-Print Check Range");
            wnd.Show();
        }
        function ClosetemplateModal() {
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            wnd.Close();
            //$('html, body').animate({ scrollTop: $('#vendorType').offset().top }, 'slow');
        }
        function CloseVoidModal() {
            var wnd = $find('<%=VoidCheckWindow.ClientID %>');
            wnd.Close();
        }
    </script>
</asp:Content>
