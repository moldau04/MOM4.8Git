<%@ Page Title="Collections || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true"
    CodeBehind="iCollections.aspx.cs" Inherits="iCollections" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <style>
        .switch.round label .lever {
            width: 54px;
            height: 30px;
            border-radius: 10em;
        }
        .fa-file-pdf-o{
                color: red;
    background-color: transparent;
        }
        
        .fa-download{
            background-color: transparent;
        }

            .switch.round label .lever:after {
                width: 26px;
                height: 22px;
                border-radius: 50%;
                left: 4px;
                top: 4px;
            }

        .RadTreeView {
            line-height: 0.3 !important;
        }
       
        .RadTreeView_Bootstrap .rtPlus, .RadTreeView_Bootstrap .rtMinus {
            margin-top: -6px !important;
        }

        .RadComboBox .rcbInner {
            padding: 0px !important;
        }

        .RadTreeView_Bootstrap .rtHover .rtIn {
            background-color: #1c5fb1 !important;
            border-color: #1c5fb1 !important;
            color: #ffffff;
        }
        .RadGrid_Bootstrap .RadComboBox .rcbInput{
            width:18px;
        }

        .CustomerColer {
            color: #1c5fb1;
            font-weight: 700;
            font-size: 0.9rem;
        }
    .RadComboBox .rcbInner {
    padding: 4px 2em 1px 4px!important;
        margin-left: 9px;
    }

        .TotalColer {
            color: #333;
            font-weight: 600;
        }

        .rtHover {
            background-color: #1c5fb1 !important;
        }

        .RadGrid_Material {
            font-size: 0.8rem !important;
        }

        .RadGrid .rgRow > td {
            font-size: 0.8rem !important;
        }

        .RadGrid_Material .rgAltRow > td {
            font-size: 0.8rem !important;
        }

            .RadGrid_Material .rgRow > td > a, .RadGrid_Material .rgAltRow > td > a {
                font-size: 0.8rem !important;
            }

        .RadGrid_Material .rgFooter td {
            font-size: 0.8rem !important;
        }

        .rcbActionButton {
            margin-top: -10px !important;
        }

        .css_owner {
            cursor: pointer;
            color: #1c5fb1;
            font-weight: 600 !important;
        }

        .css_owner_tr {
            background-color: #d2ddeb  !important;
        }
        .RadGrid_Bootstrap tr td, .RadGrid_Bootstrap .rgRow>td{
            padding-left: 13px !important;
        }
        .css_owner:before {
            content: "\f007";
            font-family: FontAwesome;
            margin-right: 10px !important;
        }

        .css_loc {
            cursor: pointer;
            color: #1c5fb1;
        }

            .css_loc:before {
                content: "\f041";
                font-family: FontAwesome;
                margin-right: 10px !important;
            }

        .rgDataDiv .rgFooter {
            display: none !important;
        }

        .RadGrid_Bootstrap .rgRow > td, .RadGrid_Bootstrap .rgAltRow > td {
            padding-left: 13px !important;
        }

        .RadGrid .rgFooter > td {
            padding-left: 0px !important;
        }

        .textrightalign {
            text-align: center !important;
        }

        .headerCollection .rwTitleWrapper {
            padding: 0 !important;
            font-size: 1.42857em;
            font-family: inherit;
            line-height: 1em;
            color: #fff;
            text-transform: capitalize;
        }

        .headerCollection .rwTitleBar {
            background-color: #1c5fb1 !important;
            padding: 5px;
        }

        [id*=RadGrid_gvLogs_GridData] .rgRow > td,
        [id*=RadGrid_gvLogs_GridData] .rgAltRow > td {
            padding-left: 13px !important;
            padding-right: 13px !important;
        }
        .width-r20{
            width:200px!important;
        }
        .form-section3 {
          float: left;
         width: 100%;
        }
         .RadWindow.rwShadow{
            width:65%!important;
        }
        @media only screen and (min-width: 250px) and (max-width: 700px) {
            .row .col.s5 {
                width: 100%;
            }
        .form-section3{
                padding-top:19px;
            }
            .row .col.s2 {
                width: 32.666667%;
                padding: 0px;
            }
            .RadWindow.rwShadow{
                width:98%!important;
                left: 3px!important;
                top: 89px!important;
            }
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-maps-place"></i>&nbsp;Collections</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks" style="display: none;">
                                            <asp:LinkButton ID="lnkAddnew" ToolTip="Issue Credit" runat="server">Issue Credit</asp:LinkButton>
                                        </div>

                                        <div class="btnlinks" style="display: none;">
                                            <asp:LinkButton ID="btnEdit" runat="server" ToolTip="Issue Refund">Issue Refund</asp:LinkButton>
                                        </div>

                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>

                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkWriteOff" runat="server" ToolTip="Write Off" OnClick="lnkWriteOff_Click" OnClientClick="return OpenWriteOffWindow(this);">Write Off</asp:LinkButton>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks" >                                           
                                                    <asp:LinkButton ID="lnkCredit" runat="server" ToolTip="Credit" OnClientClick="return OpenCreditWindow(this);"                       OnClick="lnkCredit_Click" CausesValidation="true" >Credit</asp:LinkButton>                                        
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                </div>

                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <a class="dropdown-button" id="lnkEmailAll" runat="server" data-beloworigin="true" href="#!" data-activates="EmailAll">Email</a>
                                                </div>
                                                <ul id="EmailAll" class="dropdown-content">
                                                    <li>
                                                        <asp:LinkButton ID="lnkEmailCustomerStatements" OnClick="lnkEmailCustomerStatements_Click" runat="server" ToolTip="Customer Statement All" OnClientClick="return confirm('Are you sure you want to email customer statement all?');">Customer Statement All</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkEmailCustomerStatementSelected" OnClick="lnkEmailCustomerStatementSelected_Click" runat="server" ToolTip="Customer Statement Selected" OnClientClick="return confirm('Are you sure you want to email customer statement selected?');">Customer Statement Selected</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkEmailInvoices" runat="server" OnClick="lnkEmailInvoices_Click" ToolTip="Invoice All" OnClientClick="return confirm('Are you sure you want to email invoice all?');">Invoice All</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkEmailInvoiceSelected" OnClick="lnkEmailInvoiceSelected_Click" runat="server" ToolTip="Invoice Selected" OnClientClick="return confirm('Are you sure you want to email invoice selected?');">Invoice Selected</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <a class="dropdown-button"  id="lnkPDF" runat="server" data-beloworigin="true" href="#!" data-activates="PDF">PDF</a>
                                                </div>
                                                <ul id="PDF" class="dropdown-content">
                                                    <li>
                                                        <asp:LinkButton ID="lnkPDFCustomerStatements" OnClick="lnkPDFCustomerStatements_Click" runat="server" ToolTip="Customer Statement All" OnClientClick="return confirm('Are you sure you want to PDF customer statement all?');">Customer Statement All</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPDFCustomerStatementSelected" OnClick="lnkPDFCustomerStatementSelected_Click" runat="server" ToolTip="Customer Statement Selected" OnClientClick="return confirm('Are you sure you want to PDF customer statement selected?');">Customer Statement Selected</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPDFInvoices" OnClick="lnkPDFInvoices_Click" runat="server" ToolTip="Invoice All" OnClientClick="return confirm('Are you sure you want to PDF invoice all?');">Invoice All</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPDFInvoiceSelected" OnClick="lnkPDFInvoiceSelected_Click" runat="server" ToolTip="Invoice Selected" OnClientClick="return confirm('Are you sure you want to PDF invoice selected?');">Invoice Selected</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                            <li>
                                                <ul id="dropdown1" class="dropdown-content" >
                                                    <li>
                                                        <asp:LinkButton ID="lnkPrint" runat="server" Enabled="true" OnClick="lnkPrint_Click"> <i class="fa fa-file-pdf-o "  aria-hidden="true"></i>&nbsp;&nbsp; Invoices <i class="fa fa-download" aria-hidden="true" ></i></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkMaintenance" runat="server" CausesValidation="true"
                                                            Enabled="true" OnClick="lnkMaintenance_Click"> <i class="fa fa-file-pdf-o"  aria-hidden="true" ></i>&nbsp;&nbsp; Invoice Maintenance Report &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" ></i></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkException" runat="server" CausesValidation="true"
                                                            Enabled="true" OnClick="lnkException_Click"> <i class="fa fa-file-pdf-o"  aria-hidden="true" ></i>&nbsp;&nbsp; Invoice Exception Report &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" ></i></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPDFTI" runat="server" CausesValidation="true"
                                                            Enabled="true" OnClick="lnkPDFTI_Click"> <i class="fa fa-file-pdf-o"  aria-hidden="true" ></i>&nbsp;&nbsp; Invoice With Ticket &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" ></i></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkAdamMaintenance" runat="server" CausesValidation="true"
                                                            Enabled="true" OnClick="lnkAdamMaintenance_Click"> <i class="fa fa-file-pdf-o"  aria-hidden="true" ></i>&nbsp;&nbsp; Maintenance Invoices &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" ></i></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkAdamBilling" runat="server" CausesValidation="true"
                                                            Enabled="true" OnClick="lnkAdamBilling_Click"> <i class="fa fa-file-pdf-o"  aria-hidden="true" ></i>&nbsp;&nbsp; Billing Invoice &nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true" ></i></asp:LinkButton>
                                                    </li>
                                                </ul>
                                                <div class="btnlinks">
                                                    <a class="dropdown-button" id="lnkInvoices" runat="server" data-beloworigin="true" href="#!" data-activates="dropdown1">Invoices 
                                                    </a>
                                                </div>
                                            </li>
                                            <li>
                                                  <div class="btnlinks" id="LI1pnlGridButtons" runat="server">
                                                        <a class="dropdown-button" id="lnkReports" runat="server" data-beloworigin="true" href="#!"  data-activates="dynamicUI">Reports 
                                                        </a>
                                                </div> 
                                                  <ul id="dynamicUI" class="dropdown-content">
                                                           <li>
                                                                <a class='dropdown-button2' data-activates='dropdown2' data-hover="hover" data-alignment="left" id="lnkARAgingReportsnew">AR Aging Reports<i class="float-right mdi-av-play-arrow"></i></a>
                                                            </li>
                                                            <li>
                                                                <a class='dropdown-button2' data-activates='dropdown3' data-hover="hover" data-alignment="left" id="lnkInvoiceReportsnew"> Customer Statement <i  class=" float-right mdi-av-play-arrow"></i></a>
                                                             </li>
                                                            
                                                </ul>      
                                                <ul id="dropdown2" class="dropdown-content">
                                                    <li>
                                                        <a href="ARAgingReportCust.aspx?page=iCollections">AR Aging by Custom</a>
                                                    </li>
                                                    <li>
                                                        <a href="ARAgingReportCollection.aspx?page=iCollections">AR Aging by Customer</a>
                                                    </li>
                                                    <li>
                                                        <a href="ARAgingReportByTerritoryCollection.aspx?page=iCollections">AR Aging by Default<br> Salesperson</a>
                                                    </li>
                                                    <li runat="server" id="isNoneTS">
                                                        <a href="ARAgingReportDep.aspx?page=iCollections">AR Aging by Department</a>
                                                    </li>
                                                    <li runat="server" id="isTS">
                                                        <a href="ARAgingReportByJobType.aspx?page=iCollections">AR Aging by Department</a>
                                                    </li>
                                                    <li>
                                                        <a href="ARAgingReportByLocationCollection.aspx?page=iCollections">AR Aging by<br> Location</a>
                                                    </li>
                                                       

                                                    <li>
                                                        <a href="ARAgingOver90Days.aspx?page=iCollections">AR Aging Over 90 Days</a>
                                                     </li>
                                                    <li>
                                                        <a href="ARAgingReportByLocType.aspx?page=iCollections">AR Aging by Location Type</a>
                                                    </li>
                                                    <li>
                                                        <a href="ARAgingSummaryByLocation.aspx?page=iCollections">AR Aging Summary by<br> Location</a>
                                                    </li>
                                                    <li>
                                                        <a href="ARAgingReport360ByCustomer.aspx?page=iCollections">AR Aging 360 by Customer</a>
                                                    </li>
                                                    <li>
                                                        <a href="ARAgingReport360ByLocation.aspx?page=iCollections">AR Aging 360 by Location</a>
                                                    </li>
                                                    <li>
                                                        <a href="ARAgingReport360ByLocationType.aspx?page=iCollections">AR Aging 360 by<br> Location Type</a>
                                                    </li>
                                                      <li>
                                                       <asp:LinkButton ID="lnkARAgingReportByBusinessType" runat="server" CausesValidation="true" OnClick="lnkARAgingReportByBusinessType_Click" Enabled="true">AR Aging by Business Type</asp:LinkButton>
                                                    </li>
                                                       
                                                </ul>
                                                <ul id="dropdown3" class="dropdown-content">
                                                  <li>
                                                        <asp:LinkButton runat="server" OnClick="lnkColCustomerStatement_Click" ID="lnkColCustomerStatement">Customer Statement</asp:LinkButton>
                                                    </li>
                                                </ul>
                                               
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>


            <div class="container breadcrumbs-bg-custom" style="display: none;">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="tblnks-advanced">
                            <ul class="anchor-links">
                                <li><a class="add-btn-click">Advanced Search</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

            <%--ADVANCED SEARCH DROPDOWN--%>
            <div id="stats" style="background-color: #fff !important; display: none;">
                <div id="addinfo" class="form-section-row infoDiv adv-serch" >
                    <div class="form-section-row mb">
                        <div class="section-ttle">Search Criteria</div>
                        <div class="collapse_wrap">
                            <div class="row mb" >
                                <div class="col s12 m12 l12">
                                    <div class="row mb" >
                                        <div class="input-field col s2">
                                            <div class="row">
                                                <label class="drpdwn-label" style="margin-left: 37px!important;">30 Days</label>
                                                <%--   <div class="switch square blue-white-switch">--%>
                                                <div class="switch round blue-white-switch">
                                                    <label>
                                                        <%--  Off--%>
                                                        <asp:CheckBox ID="chk30Days" runat="server" />
                                                        <span class="lever"></span>
                                                        <%--     On--%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-field col s2">
                                            <div class="row">
                                                <label class="drpdwn-label" style="margin-left: 37px!important;">60 Days</label>
                                                <div class="switch round blue-white-switch">
                                                    <label>
                                                        <%--  Off--%>
                                                        <asp:CheckBox ID="chk60Days" runat="server" />
                                                        <span class="lever"></span>
                                                        <%--  On--%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-field col s2">
                                            <div class="row">
                                                <label class="drpdwn-label" style="margin-left: 37px!important;">90 Days</label>
                                                <div class="switch round blue-white-switch">
                                                    <label>
                                                        <%--   Off--%>
                                                        <asp:CheckBox ID="chk90Days" runat="server" />
                                                        <span class="lever"></span>
                                                        <%--   On--%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-field col s2">
                                            <div class="row">
                                                <label class="drpdwn-label" style="margin-left: 33px!important;">120 Days+</label>
                                                <div class="switch round blue-white-switch">
                                                    <label>
                                                        <%--     Off--%>
                                                        <asp:CheckBox ID="chk120Days" runat="server" />
                                                        <span class="lever"></span>
                                                        <%--   On--%>
                                                    </label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-field col s1">
                                            <div class="row">
                                                <asp:TextBox ID="txtCustomDay" runat="server" AutoCompleteType="None"></asp:TextBox>
                                                <asp:Label runat="server" ID="Label8" AssociatedControlID="txtCustomDay">Custom Days</asp:Label>
                                                <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ForeColor="red"
                                                    ControlToValidate="txtCustomDay" ErrorMessage="Custom day should number only." />
                                            </div>
                                        </div>

                                        <div class="input-field col s2">
                                            <div class="row">
                                                <label class="drpdwn-label" style="margin-left: 27px!important;">Custom Days</label>
                                                <div class="switch round blue-white-switch">
                                                    <label>
                                                        <%--  Off--%>
                                                        <asp:CheckBox ID="chkCustomDays" runat="server" />
                                                        <span class="lever"></span>
                                                        <%--   On--%>
                                                    </label>
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
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane-advanced mb" style="padding: 20px 15px 0px 15px;">
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth pl">
                        As of Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtEndDate" runat="server" TabIndex="3" class="srchcstm datepicker_mom" MaxLength="50" AutoPostBack="false"></asp:TextBox>
                    </div>

                    <div class="srchinputwrap">
                        <div class="form-section3">
                            <div class="input-field col s5">
                                <div class="row">
                                    <label class="drpdwn-label">Print/Email</label>
                                    <asp:DropDownList ID="drpPrintEmail" runat="server" CssClass="browser-default selectst selectsml">
                                        <asp:ListItem Value="All">All</asp:ListItem>
                                        <asp:ListItem Value="Print">Print</asp:ListItem>
                                        <asp:ListItem Value="Email">Email</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="srchinputwrap">
                        <div class="form-section3">
                           
                                <div class="row">
                                    <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcDepartment" class="width-r20"  runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                        EmptyMessage="--Select Department--" AutoPostBack="true" OnItemChecked="rcDepartment_ItemChecked" OnCheckAllCheck="rcDepartment_CheckAllCheck">
                                    </telerik:RadComboBox>
                                </div>
                            
                        </div>
                    </div>

                    <div class="srchinputwrap" id="dvCompanyPermission" runat="server" style="display: none;">
                        <div class="form-section3">
                            <div class="input-field col s5">
                                <div class="row">
                                    <%-- <label class="drpdwn-label">Company</label>
                                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default selectst selectsml">
                                    </asp:DropDownList>--%>
                                    <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcCompany" Style="width: 160px!important;" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                        EmptyMessage="--Select Company--">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="srchinputwrap srchclr btnlinksicon" style="margin-right: 25px!important; margin-left: 10px!important;">
                        <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" CausesValidation="false"><i class="mdi-notification-sync"></i></asp:LinkButton>

                    </div>
                    <div style="display: none;">
                        <a id="lnkRefressScreen" style="display: none;" onclick="RefressScreen()"></a>
                        <asp:LinkButton ID="btnRefressScreen" Style="display: none;" runat="server" CausesValidation="false" OnClick="btnRefressScreen_Click"></asp:LinkButton>

                    </div>
                     <div class="input-field col s2" >
                        <div class="checkrow" style="margin-bottom: 18px;">
                            <asp:CheckBox ID="chkHideLocation" CssClass="css-checkbox" Text="Hide Location" runat="server" AutoPostBack="true" OnCheckedChanged="chkHideLocation_OnClick"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="input-field col s2" >
                        <div class="checkrow">
                            <asp:CheckBox ID="chkHideDetails" CssClass="css-checkbox" Text="Hide Details" runat="server" AutoPostBack="true" OnCheckedChanged="chkHideDetails_OnClick" />
                        </div>
                    </div>
                    <div class="input-field col s2" >
                        <div class="checkrow">
                            <asp:CheckBox ID="chkHidePartial" CssClass="css-checkbox" Text="Hide Partial Payment" runat="server" AutoPostBack="true" OnCheckedChanged="chkHidePartial_CheckedChanged" />
                        </div>
                    </div>
                </div>
                <div class="srchpaneinner" style="margin-bottom: -10px!important;">
                   
                </div>
            </div>
        </div>
        <div class="row" style="display: none;">
            <div class="srchpane-advanced mb-10" >
                <div class="srchpaneinner mb">
                    <div class="form-section-row">
                        <div class="input-field col s2 mr-25">
                            <div class="row">
                                <asp:TextBox ID="txtBalance" runat="server" AutoCompleteType="None"></asp:TextBox>
                                <asp:Label runat="server" ID="Label1" AssociatedControlID="txtBalance">Balance</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s2 mr-25" >
                            <div class="row">
                                <asp:TextBox ID="txt030Days" runat="server" AutoCompleteType="None"></asp:TextBox>
                                <asp:Label runat="server" ID="Label2" AssociatedControlID="txt030Days">0-30 Days</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s2 mr-25">
                            <div class="row">
                                <asp:TextBox ID="txt3160Days" runat="server" AutoCompleteType="None"></asp:TextBox>
                                <asp:Label runat="server" ID="Label3" AssociatedControlID="txt3160Days">31-60 Days</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s2 mr-25">
                            <div class="row">
                                <asp:TextBox ID="txt6190Days" runat="server" AutoCompleteType="None"></asp:TextBox>
                                <asp:Label runat="server" ID="Label4" AssociatedControlID="txtBalance">61-90 Days</asp:Label>
                            </div>
                        </div>

                        <div class="input-field col s2 mr-25">
                            <div class="row">
                                <asp:TextBox ID="txt91UP" runat="server" AutoCompleteType="None"></asp:TextBox>
                                <asp:Label runat="server" ID="Label5" AssociatedControlID="txt91UP">91 & UP</asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <telerik:RadAjaxManager ID="RadAjaxManager_Collections" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="chkHideLocation">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="chkHideDetails">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="chkHidePartial">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnSearch">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>

                    <telerik:AjaxSetting AjaxControlID="btnSelectCustomer">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="rcDepartment">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="lnkSaveWriteOff">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                            <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelWriteOff" LoadingPanelID="RadAjaxLoadingPanel_Collections" />

                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="lnkSaveCredit">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                            <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCredit" LoadingPanelID="RadAjaxLoadingPanel_Collections" />

                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="lnkWriteOff">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelWriteOff" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="lnkCredit">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCredit" LoadingPanelID="RadAjaxLoadingPanel_Collections" />                            </UpdatedControls>
                            </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="btnRefressScreen">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid_Collections" LoadingPanelID="RadAjaxLoadingPanel_Collections" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>

            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Collections" runat="server">
            </telerik:RadAjaxLoadingPanel>
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="grid_container">
                        <div class="form-section-row mb">
                            <div class="srchpaneinner">
                                <asp:Button ID="btnSelectCustomer" runat="server" CausesValidation="False" OnClick="btnSelectCustomer_Click"
                                    Style="display: none;" Text="Button" />
                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="srchcstm" onkeydown="return (event.keyCode!=13);" placeholder="Search by Customer and Location..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-section-row mb">

                            <div class="RadGrid RadGrid_Material">
                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Collections" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Collections">
                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Collections" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" AllowMultiRowSelection="true"
                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                        AllowCustomPaging="True" OnNeedDataSource="RadGrid_Collections_NeedDataSource"
                                        OnPreRender="RadGrid_Collections_PreRender"
                                        OnItemDataBound="RadGrid_Collections_ItemDataBound"
                                        OnItemCreated="RadGrid_Collections_ItemCreated"
                                        OnExcelMLExportRowCreated="RadGrid_Collections_ExcelMLExportRowCreated"
                                        OnPageIndexChanged="RadGrid_Collections_PageIndexChanged"
                                        OnPageSizeChanged="RadGrid_Collections_PageSizeChanged"
                                        OnExcelMLWorkBookCreated="RadGrid_Collections_ExcelMLWorkBookCreated">
                                        <CommandItemStyle />
                                        <GroupingSettings CaseSensitive="false" ShowUnGroupButton="true" RetainGroupFootersVisibility="false" />
                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowDragToGroup="true">
                                            <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true"></Selecting>
                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        </ClientSettings>
                                        <MasterTableView DataKeyNames="Owner,Loc" AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="true" ShowGroupFooter="true" ShowHeader="true" >

                                            <Columns>
                                                <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                </telerik:GridClientSelectColumn>
                                                <telerik:GridBoundColumn DataField="fDate" HeaderText="Date" SortExpression="fDate" UniqueName="fDate" HeaderStyle-Width="80" DataFormatString="{0:M/d/yyyy}"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="Due" HeaderText="Due Date" SortExpression="Due" UniqueName="Due" HeaderStyle-Width="100" DataFormatString="{0:M/d/yyyy}"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="DueIn" HeaderText="Due In" SortExpression="DueIn" UniqueName="DueIn" HeaderStyle-Width="70" DataType="System.String"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>




                                                <telerik:GridTemplateColumn DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName" UniqueName="CustomerName" HeaderStyle-Width="200"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    <ItemTemplate>

                                                        <asp:Label ID="lblCusName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                        <telerik:RadToolTip RenderMode="Auto" ID="CusNote" runat="server" TargetControlID="lblCusName" Width="150px"
                                                            RelativeTo="Element" Position="MiddleRight">
                                                            <%# ShowCusNote(Eval("Owner")) %>
                                                        </telerik:RadToolTip>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridTemplateColumn DataField="LocName" HeaderText="Location" SortExpression="LocName" UniqueName="LocName" HeaderStyle-Width="150"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <img id="imgCreditH" runat="server" visible='<%# (Eval("credit").ToString() == "1")?true:false %>' title="Credit Hold" src="images/MSCreditHold.png" style="float: left; width: 16px; background-color: rgba(255, 0, 0, 0.34)">
                                                        <asp:Label ID="lblLocName" runat="server" Text='<%# Eval("LocName") %>'></asp:Label>
                                                        <telerik:RadToolTip RenderMode="Auto" ID="LocNote" runat="server" TargetControlID="lblLocName" Width="150px"
                                                            RelativeTo="Element" Position="MiddleRight">
                                                            <%# ShowLocNote(Eval("Loc")) %>
                                                        </telerik:RadToolTip>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridBoundColumn DataField="LocIID" HeaderText="Location ID" SortExpression="LocIID" UniqueName="LocIID" HeaderStyle-Width="150"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                    <ItemStyle CssClass="textrightalign" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Department" HeaderText="Department" SortExpression="Department" UniqueName="Department" HeaderStyle-Width="100"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="DefaultSalesperson" HeaderText="Defa. Salesperson" SortExpression="DefaultSalesperson" UniqueName="DefaultSalesperson" HeaderStyle-Width="140"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company" UniqueName="Company" HeaderStyle-Width="100"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" HeaderStyle-Width="70"
                                                    CurrentFilterFunction="EqualTo" UniqueName="Ref" HeaderText="Ref #" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" ID="hyRef" NavigateUrl='<%#Eval("RefURL")%>'><%#Eval("Ref")%></asp:HyperLink>
                                                        <asp:HiddenField runat="server" ID="hdType" Value='<%#Eval("type")%>' />
                                                        <asp:HiddenField runat="server" ID="hdRef" Value='<%#Eval("Ref")%>' />
                                                        <asp:HiddenField runat="server" ID="hdLoc" Value='<%#Eval("Loc")%>' />
                                                        <asp:HiddenField runat="server" ID="hdLocID" Value='<%#Eval("LocIID")%>' />
                                                        <asp:HiddenField runat="server" ID="hdCustName" Value='<%#Eval("CustomerName")%>' />
                                                        <asp:HiddenField runat="server" ID="hdnPrevDue" Value='<%#Eval("Total")%>' />
                                                        <%--<asp:HiddenField runat="server" ID="hdnJob" Value='<%#Eval("JobID")%>' />--%>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="textrightalign" />
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridBoundColumn DataField="fDesc" HeaderText="Description" SortExpression="fDesc" UniqueName="fDesc" HeaderStyle-Width="190"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="Total" HeaderText="Total" SortExpression="Total" UniqueName="Total" HeaderStyle-Width="100" DataFormatString="{0:c}" GroupByExpression="Total Group By Total" Aggregate="Sum"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ThirtyDay" HeaderText="0-30 Days " SortExpression="ThirtyDay" UniqueName="ThirtyDay" HeaderStyle-Width="100" DataFormatString="{0:c}" GroupByExpression="ThirtyDay Group By ThirtyDay" Aggregate="Sum"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="SixtyDay" HeaderText="31-60 Days" SortExpression="SixtyDay" UniqueName="SixtyDay" HeaderStyle-Width="100" DataFormatString="{0:c}" GroupByExpression="SixtyDay Group By SixtyDay" Aggregate="Sum"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="NintyDay" HeaderText="61-90 Days" SortExpression="NintyDay" UniqueName="NintyDay" HeaderStyle-Width="100" DataFormatString="{0:c}" GroupByExpression="NintyDay Group By NintyDay" Aggregate="Sum"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="NintyOneDay" HeaderText="91-120 Days" SortExpression="NintyOneDay" UniqueName="NintyOneDay" HeaderStyle-Width="120" DataFormatString="{0:c}" GroupByExpression="NintyOneDay Group By NintyOneDay" Aggregate="Sum"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="OneTwentyDay" HeaderText="120 Days +" SortExpression="OneTwentyDay" UniqueName="OneTwentyDay" HeaderStyle-Width="120" DataFormatString="{0:c}" GroupByExpression="OneTwentyDay Group By OneTwentyDay" Aggregate="Sum"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                            </Columns>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="Customer"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="Customer" SortOrder="Ascending"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="Location"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="Location" SortOrder="Ascending"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
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

        <%--Email Sending Logs--%>
        <div class=" accordian-wrap">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li id="tbLogs" runat="server" style="display: block">
                            <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Email History Log</div>
                            <div class="alert alert-success" runat="server" id="divSuccess">
                                <button type="button" class="close" data-dismiss="alert">×</button>
                                These month/year period is closed out. You do not have permission to add/update this record.
                            </div>
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
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_gvLogs_ItemCreated"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvLogs_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                                                <%--<GroupByExpressions>
                                                                    <telerik:GridGroupByExpression>
                                                                        <SelectFields>
                                                                            <telerik:GridGroupByField FieldAlias="SessionNo" FieldName="SessionNo" HeaderText="Session No"></telerik:GridGroupByField>
                                                                        </SelectFields>
                                                                        <GroupByFields>
                                                                            <telerik:GridGroupByField FieldName="SessionNo"></telerik:GridGroupByField>
                                                                        </GroupByFields>
                                                                    </telerik:GridGroupByExpression>
                                                                </GroupByExpressions>--%>
                                                                <Columns>
                                                                    <%--<telerik:GridTemplateColumn DataField="SessionNo" SortExpression="SessionNo" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Session No" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSessionNo" runat="server" Text='<%# Eval("SessionNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>--%>
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
                                                                        CurrentFilterFunction="Contains" HeaderText="Ref #" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
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
                                <%--<div style="clear: both;"></div>--%>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableShadow="true" VisibleStatusbar="false" CssClass="headerCollection">
        <Windows>
            <telerik:RadWindow ID="CollectionPopWindow" Skin="Material" VisibleTitlebar="true" Title="Collection" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="100%" ShowContentDuringLoad="false" Height="600">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>



    <telerik:RadWindowManager ID="RadWindowManagerAddOpp" runat="server">
        <Windows>


            <telerik:RadWindow ID="RadWindowWriteOff" Skin="Material" VisibleTitlebar="true" Title="Write off" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="550">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="RadAjaxPanelWriteOff" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WriteOff">

                        <div style="margin-top: 15px;">

                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <label for="txtWriteOffDate">Date</label>
                                        <asp:TextBox ID="txtWriteOffDate" CssClass="datepicker_mom" runat="server" autocomplete="off"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDateWriteOff" ControlToValidate="txtWriteOffDate"
                                            ErrorMessage="Please enter Date." Display="None"
                                            ValidationGroup="PaymentWirteOff">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvDateWriteOff" />
                                        <asp:RegularExpressionValidator ID="revWriteOff" ControlToValidate="txtWriteOffDate" ValidationGroup="PaymentWirteOff"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revWriteOff" />

                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>

                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:DropDownList ID="ddlCode" runat="server" DataTextField="BillCode" DataValueField="ID" />
                                    </div>
                                </div>



                            </div>
                            <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtDescription" runat="server" />
                                    <label for="txtDescription">Description</label>
                                </div>
                            </div>


                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtInvoiceWriteOff" runat="server" Enabled="false" />
                                        <label for="txtInvoiceWriteOff">Invoice</label>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtWriteOffAmount" runat="server" Enabled="false" ClientIDMode="Static" />
                                        <label for="txtWriteOffAmount">Amount</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtWriteOffProject" runat="server" />
                                    <label for="txtWriteOffProject">Project</label>
                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtWriteOffCustID" runat="server" />
                                        <label for="txtWriteOffCustID">Customer ID</label>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtWriteOffCust" runat="server" />
                                        <label for="txtWriteOffCust">Customer</label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtWriteOffLoc" runat="server" />
                                    <label for="txtWriteOffLoc">Location</label>
                                </div>
                            </div>

                            <div style="clear: both;"></div>
                            <div style="margin-top: 20px;">
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkSaveWriteOff" runat="server" OnClick="lnkSaveWriteOff_Click">Save</asp:LinkButton>
                                </div>
                            </div>
                        </div>

                    </telerik:RadAjaxPanel>
                </ContentTemplate>

            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowCredit" Skin="Material" VisibleTitlebar="true" Title="Credit" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="250">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="RadAjaxPanelCredit" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Credit" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                        <div class="margin-tp">

                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <label for="txtCreditDate">Date</label>
                                        <asp:TextBox ID="txtCreditDate" CssClass="datepicker_mom" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDateCredit" ControlToValidate="txtCreditDate"
                                            ErrorMessage="Please enter Date." Display="None"
                                            ValidationGroup="PaymentCredit">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvDateCredit" />
                                        <asp:RegularExpressionValidator ID="revCredit" ControlToValidate="txtCreditDate" ValidationGroup="PaymentCredit"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revCredit" />

                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>

                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <%--<label class="active" >Billing Code</label>
                                        <asp:DropDownList ID="DropDownList1" runat="server" DataTextField="BillCode" DataValueField="ID" />--%>
                                        
                                        <asp:TextBox ID="txtCreditAmount" runat="server" Enabled="false" ClientIDMode="Static" />
                                        <label for="txtCreditAmount">Amount</label>
                                        <asp:HiddenField ID="hdnInvoiceCredit" runat="server" />
                                        <asp:HiddenField ID="hdnJobIDCredit" runat="server" />
                                    </div>
                                </div>

                                <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtDescriptionCredit" runat="server" />
                                    <label for="txtDescriptionCredit">Description</label>
                                </div>
                            </div>


                            </div>

                            
                            
                            <div style="clear: both;"></div>
                            <div class="top-area">
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkSaveCredit" runat="server" OnClick="lnkSaveCredit_Click" >Save</asp:LinkButton>
                                    <asp:HiddenField ID="hdnIsCreditWriteOff1" runat="server" value="1"/>
                                        <asp:HiddenField ID="hdnTransIDCredit" runat="server" value="0"/>
                                </div>
                            </div>
                        </div>

                     </telerik:RadAjaxPanel>
                </ContentTemplate>

            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <asp:HiddenField runat="server" ID="hdnOwnerID" Value="0" />
    <asp:HiddenField runat="server" ID="hdnLocID" Value="0" />
    <asp:HiddenField ID="hdnCon" runat="server" />
    <input id="hdnPatientId" runat="server" type="hidden" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script>
        $(document).ready(function () {
            $('#addinfo').hide();
            $('.add-btn-click').click(function () {
                $('#addinfo').slideToggle('2000', "swing", function () {
                    // Animation complete.
                });

                if ($('.divbutton-container').height() != 65)
                    $('.divbutton-container').animate({ height: 65 }, 500);
                else
                    $('.divbutton-container').animate({ height: 180 }, 500);
            });


            $('.link-slide').on('click', function (e) {
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

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });
            //var num = $(this).('td[colspan]').val();//or
            //alert("H1..." + num);
            //var span2 = $(this).attr('colspan')
            //alert("H2.." + span2);
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
        function OpenCollectionPop(Owner, Loc) {
            $('#<%=hdnOwnerID.ClientID%>').val(Owner);
            $('#<%=hdnLocID.ClientID%>').val(Loc);
            var fdate = $('#<%=txtEndDate.ClientID%>').val();
            var oWnd = radopen("CollectionPopup.aspx?o=1 &uid=" + Owner + "&Loc=" + Loc + "&fdate=" + fdate, "<%=CollectionPopWindow.ClientID%>"); //Pass parameter using URL 
            oWnd.show();
           /* oWnd.setSize(1010, 620);*/
            oWnd.minimize();
            oWnd.maximize();
            oWnd.restore();
            oWnd.center();
        }

        function OpenCollectionPopNew(Owner, Loc) {
            var fdate = $('#<%=txtEndDate.ClientID%>').val();
            var RefIDs = "";
            $("#<%=RadGrid_Collections.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                if ($tr.find('input[type="checkbox"]').prop("checked") == true) {
                    var val = $tr.find('input[id*=hdRef]').val();
                    RefIDs = RefIDs + val + ",";
                }
            });
            var strUrl = "CollectionPopup.aspx?o=1 &uid=" + encodeURIComponent(Owner) + "&Loc=" + encodeURIComponent(Loc) + "&Ref=" + RefIDs + "&fdate=" + fdate;
            var oWnd = radopen(strUrl,"<%=CollectionPopWindow.ClientID%>"); //Pass parameter using URL 
            oWnd.show();
            oWnd.setSize(1010, 620);
            oWnd.minimize();
            oWnd.maximize();
            oWnd.restore();
            oWnd.center();
            //oWnd.add_beforeClose(OnClientBeforeClose);
        }
        //function OnClientBeforeClose(sender, eventArgs)
        //{
        //        RefressScreen();
        //}
    </script>
    <script type="text/javascript">
        function pageLoad(sender, args) {


            ///////////// Ajax call for customer auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }

            $("#<%=txtCustomer.ClientID%>").autocomplete({

                open: function (e, ui) {
                    /* create the scrollbar each time autocomplete menu opens/updates */
                    $(".ui-autocomplete").mCustomScrollbar({
                        setHeight: 182,
                        theme: "dark-3",
                        autoExpandScrollbar: true
                    });
                },
                response: function (e, ui) {
                    /* destroy the scrollbar after each search completes, before the menu is shown */
                    $(".ui-autocomplete").mCustomScrollbar("destroy");
                },

                source: function (request, response) {


                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomer",
                        //data: '{"prefixText":' + JSON.stringify(request.term) + ',"con":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value) + '}',
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        //                        error: function(result) {
                        //                            alert("Due to unexpected errors we were unable to load customers");
                        //                        }
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message);
                        }
                    });

                },
                select: function (event, ui) {
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    {
                        $("#<%=hdnOwnerID.ClientID%>").val(ui.item.value);
                        document.getElementById('<%=btnSelectCustomer.ClientID%>').click();
                    }
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {

                    var result_item = item.label;
                    var result_desc = item.desc;
                    var result_Prospect = item.prospect;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...     


                    if (query != "") {

                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span>' + FullMatch + '</span>'
                        });

                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span>' + FullMatch + '</span>'
                            });
                        }
                    }
                    var color = '#222';
                    if (result_Prospect != 0) {
                        display = "inline-block";
                    }
                    else {
                        display = "none";
                    }
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<span class='auto_item'><i style='display:" + display + ";margin-right:8px;width:auto;color:#1565C0 !important;' class='fas fa-thumbs-up' title='Prospect'></i>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                        .appendTo(ul);
                };
            $("#<%=txtWriteOffDate.ClientID %>").pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });
            Materialize.updateTextFields();
        }
    </script>
    <script>


        function cleanUpCurrency(s) {

            var expression = '-';

            //Check if it is in the proper format
            if (s.match(expression)) {
                //It matched - strip out - and append parentheses 
                return s.replace("$-", "\($") + ")";

            }
            else {
                return s;
            }
        }
    </script>
    <script>

        function OpenWriteOffWindow() {
            var ID = "";
            var Amount = "";
            var today = new Date();
            var isCredit = "";
            var countcheck = 0;
            var custID = "";
            $('#<%=txtWriteOffDate.ClientID%>').val(today.toLocaleDateString("en-US"));

              $('#<%=txtDescription.ClientID%>').val('');
              $('#<%=txtInvoiceWriteOff.ClientID%>').val('');
              $('#<%=txtWriteOffAmount.ClientID%>').val('');
              $('#<%=txtWriteOffCust.ClientID%>').val('');
              $('#<%=txtWriteOffLoc.ClientID%>').val('');

              $('#<%=txtWriteOffCustID.ClientID%>').val("");
              $("#<%=RadGrid_Collections.ClientID %>").find('tr:not(:first,:last)').each(function () {
                  var $tr = $(this);
                  $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                      // debugger
                      if (countcheck == 0) {
                          ID = $tr.find('input[id*=hdRef]').val();
                          Amount = $tr.find('input[id*=hdnPrevDue]').val();
                          isCredit = $tr.find('input[id*=hdType]').val();
                          custID = $tr.find('input[id*=hdLocID]').val();
                          countcheck = 1;
                      }
                  });
              });

              if (ID != "") {
                  if (isCredit != 1) {
                      noty({
                          text: 'Please select a invoice',
                          type: 'warning',
                          layout: 'topCenter',
                          closeOnSelfClick: false,
                          timeout: false,
                          theme: 'noty_theme_default',
                          closable: true
                      });
                      return false;
                  } else {
                      var str = "Are you sure you want to write off this item in the amount off $" + cleanUpCurrency('$' + parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 })) + " ?";
                      if (confirm(str)) {
                          $('#<%=txtInvoiceWriteOff.ClientID%>').val(ID);
                          $('#<%=txtWriteOffCustID.ClientID%>').val(custID);
                          $('#<%=txtWriteOffAmount.ClientID%>').val(parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                          var wnd = $find('<%=RadWindowWriteOff.ClientID %>');
                        wnd.Show();
                        Materialize.updateTextFields();
                        return true;
                    } else {
                        return false;
                    }
                }
            } else {
                return false;
            }


        }


        function CloseWriteOffWindow() {
            var wnd = $find('<%=RadWindowWriteOff.ClientID %>');
            wnd.Close();
        }
        function OpenCreditWindow() {
            var countItem = 0;            
            var ID = "";
            var Amount = "";
            var today = new Date();
            var isCredit = "";
            var countcheck = 0;
            var custID = "";
            var hdnTransID = 0;
            $('#<%=txtCreditDate.ClientID%>').val(today.toLocaleDateString("en-US"));

            $('#<%=txtDescriptionCredit.ClientID%>').val('');
            $('#<%=hdnInvoiceCredit.ClientID%>').val('');
            $('#<%=txtCreditAmount.ClientID%>').val('');
            $('#<%=hdnTransIDCredit.ClientID%>').val('');

            
            $("#<%=RadGrid_Collections.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    // debugger
                    if (countcheck == 0) {
                        ID = $tr.find('input[id*=hdRef]').val();
                        Amount = $tr.find('input[id*=hdnPrevDue]').val();
                        isCredit = $tr.find('input[id*=hdType]').val();
                        custID = $tr.find('input[id*=hdLocID]').val();
                        countcheck = 1;
                    }
                    countItem = countItem + 1;
                });
            });
            if (countItem > 1) {
                noty({
                    text: 'Please select only one invoice',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            
            if (ID != "") {
                if (isCredit != 1) {
                    noty({
                        text: 'Please select a invoice',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: false,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                    return false;
                } else {
                    var str = "Are you sure you want to credit this item in the amount of $" + cleanUpCurrency('$' + parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 })) + " ?";
                    if (confirm(str)) {
                        $('#<%=hdnInvoiceCredit.ClientID%>').val(ID);                          
                        $('#<%=txtCreditAmount.ClientID%>').val(parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        $("#<%=hdnIsCreditWriteOff1.ClientID %>").val(isCredit);
                        $("#<%=hdnTransIDCredit.ClientID %>").val(hdnTransID);
                          var wnd = $find('<%=RadWindowCredit.ClientID %>');
                        wnd.Show();
                        Materialize.updateTextFields();
                        return true;
                    } else {
                        return false;
                    }
                }
            } else {
                return false;
            }


        }
        function CloseCreditWindow() {
            var wnd = $find('<%=RadWindowCredit.ClientID %>');
            wnd.Close();
        }
        function RefressScreen() {

            document.getElementById("<%=btnRefressScreen.ClientID%>").click();

        }


    </script>
</asp:Content>

