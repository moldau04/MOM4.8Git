<%@ Page Title="Wage Deduction || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="WageDeduction" CodeBehind="WageDeduction.aspx.cs" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <style>
        .spanStyle {
            color: grey;
            font-weight: normal;
            font-size: 14px;
            font-family: sans-serif;
            text-align: center;
            align-self: center;
        }

        .form-control1, .form-control_2.input-sm {
            border: 1px solid #e0e0e0;
            padding: 5px;
            color: #808080;
            background: #fff;
            width: 100%;
            font-size: small;
            font-weight: 300;
            font-family: sans-serif;
            border-radius: 2px !important;
            -webkit-appearance: none;
            outline: none;
            height: 28px;
            display: initial !important;
        }

        .form-control {
            display: initial !important;
        }

        .btn {
            border: 1px solid #0086b3;
            font-weight: 700;
            letter-spacing: 1px;
            outline: ridge;
            height: 28px;
            margin-top: -4px !important;
        }

            .btn:focus, .btn:active:focus, .btn.active:focus {
                outline: ridge;
            }

        .btn-primary {
            background: #0099cc !important;
            border-color: #208eb3 !important;
            /*color: #ffffff !important;*/
            /*border-right-color:#007399!important;
            border-right:thin!important;*/
            padding: 3px 10px !important;
            /*box-shadow: 0 3px 0 0 #4380b5;*/
        }

            .btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .open > .dropdown-toggle.btn-primary {
                background: #33a6cc !important;
                height: 28px;
            }

            .btn-primary:active, .btn-primary.active {
                background: #007299 !important;
                box-shadow: none;
                height: 28px;
            }

        .istyle {
            padding-right: 0px;
            margin-right: -1px;
            margin-top: -4px !important;
            padding: 2px !important;
            margin-left: -1px !important;
        }

        .checkbox-custom, .radio-custom {
            opacity: 0;
            position: absolute;
        }

        .checkbox-custom, .checkbox-custom-label, .radio-custom, .radio-custom-label {
            display: inline-block;
            vertical-align: middle;
            margin: 5px;
            cursor: pointer;
        }

        .checkbox-custom-label, .radio-custom-label {
            position: relative;
        }

        .checkbox-custom + .checkbox-custom-label:before, .radio-custom + .radio-custom-label:before {
            content: '';
            background: #fff;
            border: 2px solid #3EAFE6;
            display: inline-block;
            vertical-align: middle;
            width: 20px;
            height: 20px;
            padding: 2px;
            margin-right: 10px;
            text-align: center;
        }

        .checkbox-custom:checked + .checkbox-custom-label:before {
            background: #3EAFE6;
        }

        .radio-custom + .radio-custom-label:before {
            border-radius: 50%;
        }

        .radio-custom:checked + .radio-custom-label:before {
            background: #3EAFE6;
        }


        .checkbox-custom:focus + .checkbox-custom-label, .radio-custom:focus + .radio-custom-label {
            outline: 1px solid #ddd;
            / focus style /;
        }

        .rd-flt {
            float: left;
            margin-right: 20px;
            margin-bottom: 5px;
        }

        .trost-label2 a {
            color: #1565C0 !important;
        }

        [id$='RadGrid_VendorTran'] .rgHeader > a {
            white-space: nowrap;
            padding-left: 0 !important;
        }
        /*[id$='RadGrid_VendorTran'] .rgRow > td {
            white-space: nowrap;
            padding-left: 30px !important;
        }
        [id$='RadGrid_VendorTran'] .rgAltRow > td {
            white-space: nowrap;
            padding-left: 30px !important;
        }*/
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-------$$$$$$$$$$$$$$$ RAD AJAX MANAGER  $$$$$$$$$$$$$$$-----%>

    <telerik:RadAjaxManager ID="RadAjaxManager_SerType" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkEditWageDeduction">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="WageDeductionWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkAddWageDeduction">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="WageDeductionWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>


    <script type="text/javascript">
        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();
        }
    </script>
    <script type="text/javascript">


        function hideDropDown(selector, combo, e) {
            var tgt = e.relatedTarget;
            var parent = $telerik.$(selector)[0];
            var parents = $telerik.$(tgt).parents(selector);

            if (tgt != parent && parents.length == 0) {
                if (combo.get_dropDownVisible())
                    combo.hideDropDown();
            }
        }
    </script>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Vendor" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-social-person-outline"></i>&nbsp;
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Wage Deduction</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" ValidationGroup="deduction" OnClientClick="testconfirmSubmit();" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                            OnClick="lnkClose_Click" PostBackUrl="~/WageDeductList.aspx?WageDeduction=Y"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <%--<div class="tblnks">
                                <ul class="anchor-links">
                                    <li><a href="#accrdVendorInfo">Vendor Info</a></li>
                                    <li><a href="#accrdContacts">Contacts</a></li>
                                    <li runat="server" id="liTransactions"><a href="#accrdTransactions">Transactions</a></li>
                                    <li id="liLogs" runat="server" style="                                            display: none
                                    "><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>--%>
                            <div class="tblnksright">
                                <div class="nextprev">

                                    <asp:Panel ID="pnlNext" runat="server">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click" CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" OnClick="lnkPrevious_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" OnClick="lnkNext_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" OnClick="lnkLast_Click" runat="server" CssClass="icon-last" CausesValidation="False">
                                                        <i class="fa fa-angle-double-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                    </asp:Panel>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdVendorInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Deduction Info.</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    <%--<div class="section-ttle">Vendor Details</div>--%>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator166" runat="server" ControlToValidate="txtDeductionDesc"
                                                                    Display="None" ErrorMessage="Description Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender166" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="RequiredFieldValidator166">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtDeductionDesc" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtDeductionDesc">Description</label>

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlDeductionType"
                                                                    Display="None" ErrorMessage="Type Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="RequiredFieldValidator1">
                                                                </asp:ValidatorCalloutExtender>
                                                                <label class="drpdwn-label">Type</label>
                                                                <asp:DropDownList ID="ddlDeductionType" runat="server" CssClass="browser-default">

                                                                    <asp:ListItem Text="Federal" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="State" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="City" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Local" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="Other" Value="4"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlDeductionBasedon"
                                                                    Display="None" ErrorMessage="Based on Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="RequiredFieldValidator3">
                                                                </asp:ValidatorCalloutExtender>
                                                                <label class="drpdwn-label">Based On</label>
                                                                <asp:DropDownList ID="ddlDeductionBasedon" runat="server" CssClass="browser-default">

                                                                    <asp:ListItem Text="FIT Wages" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="FICA Wages" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="MEDI Wages" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="FUTA Wages" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="SIT Wages" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="Vacation Wages" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="Worker's Comp Wages" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="Union Wages" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="Flat Amount" Value="8"></asp:ListItem>

                                                                </asp:DropDownList>


                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlDeductionAccuredon"
                                                                    Display="None" ErrorMessage="Accured on Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="RequiredFieldValidator5">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:DropDownList ID="ddlDeductionAccuredon" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Text="Number of Hours" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Dollar Amount" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Flat Amount" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="drpdwn-label">Accured On</label>
                                                            </div>
                                                        </div>
                                                      <%--  <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlDeductionJobSp"
                                                                    Display="None" ErrorMessage="Job Specific Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender17" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="RequiredFieldValidator17">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:DropDownList ID="ddlDeductionJobSp" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="drpdwn-label">Project Specific</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="ddlDeductionFrequency"
                                                                    Display="None" ErrorMessage="Frequency Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender18" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="RequiredFieldValidator18">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:DropDownList ID="ddlDeductionFrequency" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Text="Every Check" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Monthly - 1st Check" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Monthly - 2nd Check" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Monthly - 3rd Check" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="Monthly - 4th Check" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="Monthly - Last Check" Value="5"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="drpdwn-label">Frequency</label>
                                                            </div>
                                                        </div>--%>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvPaidBy" runat="server" ControlToValidate="ddlDeductionPaidBy"
                                                                    Display="None" ErrorMessage="Paid By Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvPaidBy" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="rfvPaidBy">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:DropDownList ID="ddlDeductionPaidBy" runat="server" onchange="GetSelectedPaidBy(this)" CssClass="browser-default">
                                                                    <asp:ListItem Text="Company" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Employee" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Both" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="drpdwn-label">Paid By</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvCompanyRate" runat="server" ControlToValidate="txtDeductionCompnayRate"
                                                                    Display="None" ErrorMessage="Company Rate Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvCompanyRate" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvCompanyRate"></asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtDeductionCompnayRate" runat="server" onkeypress="return numericOnly(this);" Text="0.00"></asp:TextBox>
                                                                <label for="txtDeductionCompnayRate">Company Rate</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvCompanyCeiling" runat="server" ControlToValidate="txtDeductionCompnayCeiling"
                                                                    Display="None" ErrorMessage="Company Ceiling Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvCompanyCeiling" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                    TargetControlID="rfvCompanyCeiling">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtDeductionCompnayCeiling" runat="server" onkeypress="return numericOnly(this);" Text="0.00"></asp:TextBox>
                                                                <label for="txtDeductionCompnayCeiling">Company Max Rate</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvCompanyGL" runat="server" ControlToValidate="txtComapnyGLAcct"
                                                                    Display="None" ErrorMessage="Company GL Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvCompanyGL" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvCompanyGL">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtComapnyGLAcct" runat="server" MaxLength="375" Placeholder="Search by acct# and name" />
                                                                <asp:HiddenField ID="hdnComapnyGLAcct" runat="server" />
                                                                <asp:Label runat="server" ID="lblComapnyGLAcct" AssociatedControlID="txtComapnyGLAcct">Company GL</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvCompanyExp" runat="server" ControlToValidate="txtCompanyExpGLAcct"
                                                                    Display="None" ErrorMessage="Company Exp GL Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvCompanyExp" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvCompanyExp"></asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtCompanyExpGLAcct" runat="server" MaxLength="75" Placeholder="Search by acct# and name" />
                                                                <asp:HiddenField ID="hdnCompanyExpGLAcct" runat="server" />
                                                                <asp:Label runat="server" ID="lblCompanyExpGLAcct" AssociatedControlID="txtCompanyExpGLAcct">Company Exp GL</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvEmployeeRate" runat="server" ControlToValidate="txtDeductionEmpRate"
                                                                    Display="None" ErrorMessage="Employee Rate Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvEmployeeRate" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvEmployeeRate"></asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtDeductionEmpRate" runat="server" onkeypress="return numericOnly(this);" Text="0.00" disabled></asp:TextBox>
                                                                <label for="txtDeductionEmpRate">Employee Rate</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvEmployeeCeiling" runat="server" ControlToValidate="txtDeductionEmpCeiling"
                                                                    Display="None" ErrorMessage="Employee Ceiling Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvEmployeeCeiling" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvEmployeeCeiling"></asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtDeductionEmpCeiling" runat="server" onkeypress="return numericOnly(this);" Text="0.00" disabled></asp:TextBox>
                                                                <label for="txtDeductionEmpCeiling">Employee Max Rate</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvEmployeeGL" runat="server" ControlToValidate="txtEmpGLAcct"
                                                                    Display="None" ErrorMessage="Employee GL Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvEmployeeGL" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvEmployeeGL"></asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtEmpGLAcct" runat="server" MaxLength="375" Placeholder="Search by acct# and name" disabled />
                                                                <asp:HiddenField ID="hdnEmpGLAcct" runat="server" />
                                                                <asp:Label runat="server" ID="lblEmpGLAcct" AssociatedControlID="txtEmpGLAcct">Employee GL</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlDeductionPaid"
                                                                    Display="None" ErrorMessage="Paid Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator8"></asp:ValidatorCalloutExtender>
                                                                <asp:DropDownList ID="ddlDeductionPaid" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Text="Must Be Remitted" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Posted To GL Acct" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="drpdwn-label">Paid</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvVendor" runat="server" ControlToValidate="txtVendor"
                                                                    Display="None" ErrorMessage="Vendor Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vcrfvVendor" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvVendor"></asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtVendor" runat="server" MaxLength="375" Placeholder="Search by vendor name" />
                                                                <asp:HiddenField ID="hdntxtVendor" runat="server" />
                                                                <asp:Label runat="server" ID="lblVendor" AssociatedControlID="txtVendor">Vendor</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlDeduction401"
                                                                    Display="None" ErrorMessage="401K-Type Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender12" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator12"></asp:ValidatorCalloutExtender>
                                                                <asp:DropDownList ID="ddlDeduction401" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                <label class="drpdwn-label">Deduction-Type</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="ddlDeductionW2Box"
                                                                    Display="None" ErrorMessage="W2 Box Required" SetFocusOnError="True" ValidationGroup="deduction"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender19" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator19"></asp:ValidatorCalloutExtender>
                                                                <asp:DropDownList ID="ddlDeductionW2Box" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Text="None" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 9" Value="9"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 10" Value="10"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 11" Value="11"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 12" Value="12"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 16" Value="16"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 16B" Value="16B"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 17" Value="17"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 18" Value="18"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 19" Value="19"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 20" Value="20"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 21" Value="21"></asp:ListItem>
                                                                    <asp:ListItem Text="Box 14" Value="14"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label class="drpdwn-label">W2 Box</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="Label3" AssociatedControlID="txtdeductionremark">Remark</asp:Label>
                                                                <asp:TextBox ID="txtdeductionremark" runat="server" CssClass="materialize-textarea remit-ht" TextMode="MultiLine" MaxLength="255" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkProcessDeduction" runat="server" Text="Process this deduction even if the Process Other Deductions is disabled in the Process Payroll Screen" CssClass="css-checkbox" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>


    <div style="clear: both;"></div>
    <footer style="float: left; padding-left: 0 !important;">
        <div class="btnlinks">
            <asp:HiddenField ID="hdnWageDeductionID" runat="server" />
            <asp:HiddenField ID="hdnAddEdit" runat="server" />
            <asp:HiddenField ID="hdnFlage" runat="server" />
        </div>
    </footer>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">


    <script type="text/javascript">
        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();
        }
        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <script type="text/javascript">  
        var specialKeys = new Array();

        specialKeys.push(8); //Backspace  

        function numericOnly(elementRef) {

            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;

            if ((keyCodeEntered >= 48) && (keyCodeEntered <= 57)) {

                return true;

            }

            // '.' decimal point...  

            else if (keyCodeEntered == 46) {
                if ((elementRef.value) && (elementRef.value.indexOf('.') >= 0))
                    return false;
                else
                    return true;
            }

            return false;

        }
    </script>

    <script type="text/javascript"> 

        function testconfirmSubmit() {
            var spaidby = $("#<%=ddlDeductionPaidBy.ClientID%>").val(); //0--Company,1--Employee,2--Both

            if (spaidby == "0") {
                //alert('A');
                ValidatorEnable($('#<%=rfvCompanyRate.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvCompanyCeiling.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvCompanyGL.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvCompanyExp.ClientID %>')[0], true);

                ValidatorEnable($('#<%=rfvEmployeeRate.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvEmployeeCeiling.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvEmployeeGL.ClientID %>')[0], false);

            }

            if (spaidby == "1") {
                //alert('B');

                ValidatorEnable($('#<%=rfvCompanyRate.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvCompanyCeiling.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvCompanyGL.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvCompanyExp.ClientID %>')[0], false);

                ValidatorEnable($('#<%=rfvEmployeeRate.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvEmployeeCeiling.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvEmployeeGL.ClientID %>')[0], true);
            }

            if (spaidby == "2") {
                //alert('C');


                ValidatorEnable($('#<%=rfvCompanyRate.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvCompanyCeiling.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvCompanyGL.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvCompanyExp.ClientID %>')[0], true);

                ValidatorEnable($('#<%=rfvEmployeeRate.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvEmployeeCeiling.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvEmployeeGL.ClientID %>')[0], true);
            }

        }


        $(document).ready(function () {
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
            $("#<%=txtEmpGLAcct.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAccountName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load accounts");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtEmpGLAcct.ClientID%>").val(ui.item.label);
                    $("#<%=hdnEmpGLAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtEmpGLAcct.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.acct;
                    var result_desc = item.label;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_value == 0) {
                        //return $("<li></li>")
                        //.data("item.autocomplete", item)
                        //.append("<a>" + result_item + "</a>")
                        //.appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };

            $("#<%=txtComapnyGLAcct.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAccountName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load accounts");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtComapnyGLAcct.ClientID%>").val(ui.item.label);
                    $("#<%=hdnComapnyGLAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtComapnyGLAcct.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.acct;
                    var result_desc = item.label;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_value == 0) {
                        //return $("<li></li>")
                        //.data("item.autocomplete", item)
                        //.append("<a>" + result_item + "</a>")
                        //.appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };

            $("#<%=txtCompanyExpGLAcct.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAccountName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load accounts");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtCompanyExpGLAcct.ClientID%>").val(ui.item.label);
                    $("#<%=hdnCompanyExpGLAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtCompanyExpGLAcct.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.acct;
                    var result_desc = item.label;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_value == 0) {
                        //return $("<li></li>")
                        //.data("item.autocomplete", item)
                        //.append("<a>" + result_item + "</a>")
                        //.appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };



            $("#<%=txtVendor.ClientID%>").autocomplete({

                open: function (e, ui) {
                    $(".ui-autocomplete").mCustomScrollbar({
                        setHeight: 182,
                        theme: "dark-3",
                        autoExpandScrollbar: true
                    });
                },
                response: function (e, ui) {
                    $(".ui-autocomplete").mCustomScrollbar("destroy");
                },
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetVendorName",
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
                    if (str == "No Record Found!") {
                        $("#<%=txtVendor.ClientID%>").val("");
                    }
                    else {

                        $("#<%=txtVendor.ClientID%>").val(ui.item.Name);
                        $("#<%=hdntxtVendor.ClientID%>").val(ui.item.ID);

                    }

                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtVendor.ClientID%>").val(ui.item.Name);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.Name;
                    var result_Company = item.Company;
                    var result_desc = item.desc;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span>' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span>' + FullMatch + '</span>'
                        });
                    }
                    if (result_value == 0) {

                        return $("<li></li>")
                            .data("ui-autocomplete-item", item)
                            .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                            .appendTo(ul);
                    }
                    else {
                        var append_data = "";
                        //Premission Check.....
                        var chk = '<%=Convert.ToString(Session["COPer"]) %>';
                        if (chk == "1") {
                            append_data = "<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>" + "<span class='con_hide' style='color:Gray;'> ," + result_Company + "</span></a>";
                        }
                        else {
                            append_data = "<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>";
                        }

                        return $("<li ></li>")
                            .data("ui-autocomplete-item", item)
                            .append(append_data)
                            .appendTo(ul);
                    }
                };


        });

        $("#<%=txtVendor.ClientID%>").keyup(function (event) {

            var hdntxtVendor = document.getElementById('<%=hdntxtVendor.ClientID%>');
            if (document.getElementById('<%=txtVendor.ClientID%>').value == '') {
                hdntxtVendor.value = '';
            }
        });


        function myFunction() {
            var addeditcase = $('#<%=hdnAddEdit.ClientID%>').val();
            if (addeditcase != '0') {
                var choice = window.confirm('Would you like to update all projects with these changes?');
                if (choice == true) {
                    $('#<%=hdnFlage.ClientID%>').val('1');
                    return true;
                }
                else {
                    $('#<%=hdnFlage.ClientID%>').val('0');
                    return true;
                }
            }
            else {
                return true;
            }

        }


        function OpenWageDeductionWindowEditDoubleclick(ID) {

            <%-- $("#<%=RadGrid_WageDeduction.ClientID %>").find('tr:not(:first,:last)').each(function () {
                 var $tr = $(this);
                 $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                     ID = $tr.find('span[id*=lblId]').text();


                 });
             });--%>
            if (ID != "") {
                <%-- $('#<%=txtServiceType.ClientID%>').val(ID);
                $('#<%=hdnAddEdit.ClientID%>').val(ID);
                var wnd = $find('<%=WageDeductionWindow.ClientID %>');
                 wnd.set_title("Edit Wage Deduction");
                 Materialize.updateTextFields();
                 document.getElementById('<%=lnkEditWageDeduction.ClientID%>').click();--%>
            }
            else {
                ChkWarning();
            }
        }


    </script>


    <script type="text/javascript">
        function GetSelectedPaidBy(ddlDeductionPaidBy) {
            //var selectedText = ddlDeductionPaidBy.options[ddlDeductionPaidBy.selectedIndex].innerHTML;
            var _sValue = ddlDeductionPaidBy.value;
            var EmpRate = document.getElementById('<%=txtDeductionEmpRate.ClientID%>');
            var EmpMaxRate = document.getElementById('<%=txtDeductionEmpCeiling.ClientID%>');
            var EmpGL = document.getElementById('<%=txtEmpGLAcct.ClientID%>');

            var CompRate = document.getElementById('<%=txtDeductionCompnayRate.ClientID%>');
            var CompMaxRate = document.getElementById('<%=txtDeductionCompnayCeiling.ClientID%>');
            var CompGL = document.getElementById('<%=txtComapnyGLAcct.ClientID%>');
            var CompExpGL = document.getElementById('<%=txtCompanyExpGLAcct.ClientID%>');
            if (_sValue == 0) {

                EmpRate.disabled = true;
                EmpMaxRate.disabled = true;
                EmpGL.disabled = true;
                CompRate.disabled = false;
                CompMaxRate.disabled = false;
                CompGL.disabled = false;
                CompExpGL.disabled = false;

            }
            if (_sValue == 1) {
                EmpRate.disabled = false;
                EmpMaxRate.disabled = false;
                EmpGL.disabled = false;
                CompRate.disabled = true;
                CompMaxRate.disabled = true;
                CompGL.disabled = true;
                CompExpGL.disabled = true;

            }
            if (_sValue == 2) {

                EmpRate.disabled = false;
                EmpMaxRate.disabled = false;
                EmpGL.disabled = false;
                CompRate.disabled = false;
                CompMaxRate.disabled = false;
                CompGL.disabled = false;
                CompExpGL.disabled = false;

            }
        }
    </script>

    <script type="text/javascript">
        $("#<%=ddlDeductionPaid.ClientID%>").change(function () {
            var paid = $(this).val(); //0--Posted To GL Acct,1--Must Be Remitted
            if (paid == 1) {
                ValidatorEnable($('#<%=rfvVendor.ClientID%>')[0], true);
            }
            else if (paid == 0) {
                ValidatorEnable($('#<%=rfvVendor.ClientID%>')[0], false);
            }
        })
    </script>
</asp:Content>
