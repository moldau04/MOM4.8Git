<%@ Page Title="Bills || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ManageBills" CodeBehind="ManageBills.aspx.cs" EnableEventValidation="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
        @media only screen and (min-width: 250px) and (max-width: 700px) {
            .w-100 {
                width: 100%!important;
            }
            .srchinputwrap .selectst{
                width:100%;
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
                                            <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Manage Bills</div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddnew" runat="server" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks menuAction">
                                                    <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                    </a>
                                                </div>
                                                <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkCopy" runat="server" OnClick="lnkCopy_Click">Copy</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkDelete" OnClientClick="return ConfirmDelete();" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:HyperLink ID="lnkEditBillDate" runat="server" Style="cursor: pointer;" onclick="OpenBillPopupEdit(this);">Bill Date</asp:HyperLink>
                                                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                                                <Windows>
                                                                    <telerik:RadWindow ID="BillCodeWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" Title="Edit Bill Date" VisibleStatusbar="false" MinWidth="450"
                                                                        runat="server" Modal="true" Width="500" Height="300">
                                                                        <ContentTemplate>
                                                                            <div class="m-t-15">
                                                                                <div class="form-section-row">
                                                                                    <div class="form-section2">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtReference" runat="server" Enabled="false"></asp:TextBox>
                                                                                                <label for="txtReference">Ref#</label>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section2-blank">
                                                                                        &nbsp;
                                                                                    </div>
                                                                                    <div class="form-section2">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker_mom"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="rfvDate" ValidationGroup="bills"
                                                                                                    runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Date is Required"
                                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True"
                                                                                                    PopupPosition="Right" TargetControlID="rfvDate" />
                                                                                                <asp:RegularExpressionValidator ID="revDate" ControlToValidate="txtDate" ValidationGroup="bills"
                                                                                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                                                </asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="revDate" />
                                                                                                <label for="txtDate">Date</label>
                                                                                            </div>
                                                                                        </div>

                                                                                    </div>


                                                                                </div>
                                                                                <div class="form-section-row">

                                                                                    <div class="form-section2">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtPostingDate" runat="server" TabIndex="2" CssClass="datepicker_mom"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="rfvPostDate" ValidationGroup="bills"
                                                                                                    runat="server" ControlToValidate="txtPostingDate" Display="None" ErrorMessage="Posting date is Required"
                                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vcePostDate" runat="server" Enabled="True"
                                                                                                    PopupPosition="Right" TargetControlID="rfvPostDate" />
                                                                                                <asp:RegularExpressionValidator ID="revPostDate" ControlToValidate="txtPostingDate" ValidationGroup="bills"
                                                                                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                                                </asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vcePostDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="revPostDate" />
                                                                                                <label for="txtPostingDate">Posting Date</label>
                                                                                            </div>
                                                                                        </div>

                                                                                    </div>
                                                                                    <div class="form-section2-blank">
                                                                                        &nbsp;
                                                                                    </div>
                                                                                    <div class="form-section2">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">


                                                                                                <asp:TextBox ID="txtDueDate" runat="server" TabIndex="2" CssClass="datepicker_mom"> </asp:TextBox>

                                                                                                <asp:RequiredFieldValidator ID="rfvDueDate"
                                                                                                    runat="server" ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due date is Required" ValidationGroup="bills"
                                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceDueDate" runat="server" Enabled="True"
                                                                                                    PopupPosition="Right" TargetControlID="rfvDueDate" />
                                                                                                <asp:RegularExpressionValidator ID="revDueDate" ControlToValidate="txtDueDate" ValidationGroup="bills"
                                                                                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                                                </asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceDueDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="revDueDate" />
                                                                                                <label for="txtDueDate">Due</label>
                                                                                            </div>
                                                                                        </div>

                                                                                    </div>


                                                                                </div>
                                                                                <div style="clear: both;"></div>

                                                                                <footer class="footer-css-top-btn" >
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lbtnDateSave" runat="server" OnClick="lbtnDateSave_Click" CausesValidation="true"> Save </asp:LinkButton>
                                                                                    </div>
                                                                                </footer>
                                                                            </div>
                                                                        </ContentTemplate>

                                                                    </telerik:RadWindow>
                                                                </Windows>
                                                            </telerik:RadWindowManager>
                                                        </div>
                                                    </li>
                                                    
                                                    <li>
                                                        <div class="btnlinks" id="LI1pnlGridButtons" runat="server">
                                                            <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dynamicUI">Reports
                                                            </a>
                                                        </div>
                                                        <ul id="dynamicUI" class="dropdown-content">
                                                           <li>
                                                                <a class='dropdown-button2' data-activates='dropdown2' data-hover="hover" data-alignment="left" id="lnkARAgingReportsnew">AP Aging Reports<i class="float-right mdi-av-play-arrow"></i></a>
                                                            </li>
                                                            <li>
                                                                <a class='dropdown-button2' data-activates='dropdown3' data-hover="hover" data-alignment="left" id="lnkInvoiceReportsnew">Bills Reports <i  class=" float-right mdi-av-play-arrow"></i></a>
                                                             </li>
                                                            <li>
                                                                <a class='dropdown-button2' data-activates='dropdown4' data-hover="hover" data-alignment="left" id="lnkInvoiceReportsnew">AP Bill Sales/Use Tax <i  class=" float-right mdi-av-play-arrow"></i></a>
                                                             </li>
                                                        </ul>
                                                       <ul id='dropdown2' class='dropdown-content'> 
                                                            <li>
                                                                <a href="BillListingReport.aspx?type=Bill" class="-text">Add New Report</a>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkAPAgingReport" PostBackUrl="~/APAgingReport.aspx" runat="server" CssClass="-text">AP Aging Based on Past <br> Due Report</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkAPAgingBasedDateReport" PostBackUrl="~/APAgingBasedDateReport.aspx" runat="server" CssClass="-text">AP Aging Report</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkAPAgingOver90DaysReport" PostBackUrl="~/APAgingOver90DaysReport.aspx" runat="server" CssClass="-text">AP Aging Over 90 Days</asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <a href="APAging360Report.aspx" class="-text">AP Aging 360 Report</a>
                                                            </li>
                                                        </ul>
                                                         <ul id='dropdown3' class='dropdown-content'>   
                                                            <li>
                                                                <asp:LinkButton ID="lnkBillsReport" OnClick="lnkBillsReport_Click" runat="server">Bill Summary Report</asp:LinkButton>
                                                            </li>
                                                            
                                                            <li>
                                                                <a href="PurchaseJournalReport.aspx" class="-text">Bill Weekly Report</a>
                                                            </li>
                                                            
                                                            <li>
                                                                <asp:LinkButton ID="lnkBillRegisterGL" OnClick="lnkBillRegisterGL_Click" runat="server">Bill Register GL Report</asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                            <ul id='dropdown4' class='dropdown-content'>   
                                                           
                                                            <li>
                                                                <a href="UseTaxReport.aspx" class="-text">Use Tax Report</a>
                                                            </li>
                                                            <li>
                                                                <a href="UTaxLocReport.aspx" class="-text">UTax Location Report</a>
                                                            </li>
                                                            
                                                        </ul>

                                                    </li>
                                                    <li>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkProcess" ForeColor="Red" runat="server" OnClientClick="return CheckProcess();" OnClick="lnkProcess_Click"> Process</asp:LinkButton>
                                                        </div>
                                                    </li>
                                                </ul>






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
                        <%--<asp:Label ID="lblStart" runat="server" Text="Date"></asp:Label>--%>
                        <asp:DropDownList ID="ddlFilterDate" runat="server" CssClass="browser-default selectst selectsml" AutoPostBack="True" OnSelectedIndexChanged="ddlFilterDate_SelectedIndexChanged">
                            <asp:ListItem Value="0">Date </asp:ListItem>
                            <asp:ListItem Value="1">Payment Date </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <%--<label>From</label>--%>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker_mom w-100" ></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <%--<label>To</label>--%>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker_mom w-100"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnBillsSelectDtRange', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnBillsSelectDtRange', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnBillsSelectDtRange', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnBillsSelectDtRange', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnBillsSelectDtRange', 'rdCal')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                    <div class="srchinputwrap rdleftmgn">
                        <div class="rdpairing">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpRdo">
                                <ContentTemplate>
                                    <div class="rd-flt">
                                        <asp:RadioButton ID="rdobill" CssClass="with-gap rdoJournal" runat="server" Text=" Bills" GroupName="JE" AutoPostBack="true" OnCheckedChanged="rdobill_CheckedChanged" />
                                    </div>
                                    <div class="rd-flt">
                                        <asp:RadioButton ID="rdoRecurring" CssClass="with-gap" runat="server" Text=" Recurring Bills" GroupName="JE" AutoPostBack="true" OnCheckedChanged="rdoRecurring_CheckedChanged" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True" Text="Incl. Closed"></asp:CheckBox>
                                <%--<asp:Label ID="lblChkSelect" runat="server">Incl. Closed</asp:Label>--%>
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
                <%-- <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth ser-css2">
                        Search
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst selectsml" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>

                    <div class="srchinputwrap">
                        <telerik:RadComboBox EmptyMessage="Select Status" RenderMode="Auto" ID="ddlStatus" runat="server" Visible="False" CssClass="browser-default selectst selectsml" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="Open" Value="0" />
                                <telerik:RadComboBoxItem Text="Closed" Value="1" />
                                <telerik:RadComboBoxItem Text="Void" Value="2" />
                                <telerik:RadComboBoxItem Text="Partial" Value="3" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlBOMType" runat="server" CssClass="browser-default selectst" Visible="False" AutoPostBack="true">
                            <asp:ListItem Value="1">Materials </asp:ListItem>
                            <asp:ListItem Value="2">Labor </asp:ListItem>
                            <asp:ListItem Value="3">Sub-Contract</asp:ListItem>
                            <asp:ListItem Value="4">Permit </asp:ListItem>
                            <asp:ListItem Value="5">Consulting </asp:ListItem>
                            <asp:ListItem Value="6">Equipment Rental</asp:ListItem>
                            <asp:ListItem Value="7">Inspections </asp:ListItem>
                            <asp:ListItem Value="8">Inventory </asp:ListItem>
                            <asp:ListItem Value="9">Submissions</asp:ListItem>
                            <asp:ListItem Value="10">Misc. </asp:ListItem>
                            <asp:ListItem Value="11">Travel </asp:ListItem>
                            <asp:ListItem Value="14">Commission </asp:ListItem>
                            <asp:ListItem Value="15">Other</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default selectst" Visible="False">
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm" onkeypress="return isNumberKey(this,event)"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtVendorSearch" runat="server" CssClass="srchcstm" Visible="false"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <%--<asp:DropDownList ID="ddlInvoice" runat="server" CssClass="browser-default selectst" OnSelectedIndexChanged="ddlInvoice_SelectedIndexChanged" AutoPostBack="true">--%>
                        <asp:DropDownList ID="ddlInvoice" runat="server" CssClass="browser-default selectst">
                            <asp:ListItem Value="0">All </asp:ListItem>
                            <asp:ListItem Value="1">Outstanding </asp:ListItem>
                            <asp:ListItem Value="2">Due </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap">

                        <asp:TextBox ID="txtRef" runat="server" CssClass="srchcstm" placeholder="Ref"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <div class="btnlinksicon">
                            <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
            <div class="grid_container">
                <div class="form-section-row m-b-0">

                    <telerik:RadAjaxManager ID="RadAjaxManager_Bills" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkDelete">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkChk">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkCopy">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlBOMType" />
                                    <telerik:AjaxUpdatedControl ControlID="txtVendorSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="txtRef" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkChk" />


                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="rdobill">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkProcess" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rdoRecurring">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkProcess" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkProcess">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Bills" LoadingPanelID="RadAjaxLoadingPanel_Bills" />
                                    <telerik:AjaxUpdatedControl ControlID="txtSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlBOMType" />
                                    <telerik:AjaxUpdatedControl ControlID="txtVendorSearch" />
                                    <telerik:AjaxUpdatedControl ControlID="txtRef" />
                                    <telerik:AjaxUpdatedControl ControlID="ddlType" />


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
                                    var grid = $find("<%= RadGrid_Bills.ClientID %>");
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

                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Bills" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_Bills_NeedDataSource" OnPreRender="RadGrid_Bills_PreRender" OnItemEvent="RadGrid_Bills_ItemEvent" OnItemCreated="RadGrid_Bills_ItemCreated" OnExcelMLExportRowCreated="RadGrid_Bills_ExcelMLExportRowCreated">
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
                                                <asp:HiddenField ID="hdnSel" Value='<%# Eval("Status") %>' runat="server" />
                                                <asp:Label ID="lblIDate" Text='<%# Eval("IDate") %>' runat="server" />
                                                <asp:Label ID="lblIndex" Text='<%# Eval("ID")%>' runat="server" />
                                                <asp:Label ID="lblStatus" Text='<%# Eval("Status")%>' runat="server" />
                                                <asp:Label ID="lblDate" Text='<%# Eval("Date")%>' runat="server" />
                                                <asp:Label ID="lblDue" Text='<%# Eval("Due")%>' runat="server" />
                                                <asp:Label ID="lblTRID" Text='<%# Eval("TRID") %>' runat="server" />
                                                <asp:Label ID="lblBatch" Text='<%# Eval("Batch") %>' runat="server" />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="fdate" DataField="fdate" SortExpression="fdate" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPostDate" Text='<%# Eval("PostingDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> ' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Ref" DataField="Ref" HeaderText="Ref" SortExpression="Ref"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lblRef" runat="server" Text='<%# Bind("Ref") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="fDesc" DataField="fDesc" HeaderText="Description" SortExpression="fDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="150">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="VendorName" DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="StatusName" DataField="StatusName" HeaderText="Status" SortExpression="StatusName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Comapny" DataField="Company" HeaderText="Company" SortExpression="Company" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="Amount" DataField="Amount" HeaderStyle-Width="100" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Amount" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="Balance" DataField="Balance" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderStyle-Width="100" SortExpression="Balance" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Amount Due" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("Balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PayDate" DataField="PayDate" SortExpression="fdate" AutoPostBackOnFilter="true" HeaderStyle-Width="100" DataType="System.String" CurrentFilterFunction="Contains" HeaderText="Payment Date" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayDate" Text='<%# Eval("PayDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "PayDate"))):""%> ' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="po" DataField="po" HeaderText="PO#" SortExpression="po" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Int32"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="RPO" DataField="RPO" HeaderText="RPO#" SortExpression="RPO" HeaderStyle-Width="100"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Int32"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="usetax" DataField="usetax" HeaderStyle-Width="100" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="usetax" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Use Tax" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUseTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "usetax", "{0:c}")%>'></asp:Label>
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
        function OpenBillPopupEdit() {
            debugger;
            var grid = $find("<%=RadGrid_Bills.ClientID %>");
            var MasterTable = grid.get_masterTableView();
            var selectedRows = MasterTable.get_selectedItems();
            var ID = "";
            for (var i = 0; i < selectedRows.length; i++) {
                var row = selectedRows[i];
                ID = MasterTable.getCellByColumnUniqueName(row, "lblIndexID").innerHTML;
            }
            if (ID != "") {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "AccountAutoFill.asmx/ManageBillCodeEdit",
                    data: '{"lblIndex": "' + ID + '"}',
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var ui = $.parseJSON(data.d);
                        if (ui.length > 0) {
                            $('#<%=hdnID.ClientID%>').val(ui[0].ID);
                            $('#<%=hdnTRID.ClientID%>').val(ui[0].TRID);
                            $('#<%=txtReference.ClientID%>').val(ui[0].Ref);
                            $('#<%=txtPostingDate.ClientID%>').val(ui[0].PostingDate);
                            $('#<%=txtDate.ClientID%>').val(ui[0].Date);
                            $('#<%=txtDueDate.ClientID%>').val(ui[0].Due);
                            $('#<%=hdnBatch.ClientID%>').val(ui[0].Batch);

                            window.radopen(null, "BillCodeWindow");
                            Materialize.updateTextFields();
                        }
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
            else {
                ChkWarning();
            }
        }
        function DeleteBillMsg() {
            noty({
                text: 'Bill deleted successfully!',
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
                text: 'Recurring Bill deleted successfully!',
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
                text: 'This bill is not open and can therefore not be deleted.',
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
                text: 'Please select an bill to edit bill date.',
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
            $("#<%=RadGrid_Bills.ClientID %>").find('tr:not(:first,:last)').each(function () {
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
                    text: 'This bill has closed and can therefore not be voided.',
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
                    text: 'This bill is already voided and can therefore not be voided.',
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
                    return confirm('Are you sure that you want to delete this bill ?');
                }
                else {
                    noty({
                        text: 'Please select an bill to delete.',
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
            $("#<%=RadGrid_Bills.ClientID%> tr").each(function () {
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

            //efficient-&-compact JQuery way
            $(reports).each(function (index, report) {

                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }
                $('#dynamicUI').append('<li><a href="BillListingReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Bill"><span> <img src=images/reportfolder.png>  ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')
            });
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
                val = localStorage.getItem("hdnBillsSelectDtRange");
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
            $('.dropdown-button2').dropdown({
                inDuration: 300,
                outDuration: 225,
                constrain_width: false, // Does not change width of dropdown to that of the activator
                hover: true, // Activate on hover
                gutter: ($('.dropdown-content').width() * 2.4) / 2.5 + 5, // Spacing from edge
                belowOrigin: false, // Displays dropdown below the button
                alignment: 'left' // Displays dropdown with edge aligned to the left of button
            });
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


    </script>

</asp:Content>
