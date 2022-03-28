<%@ Page Title="Wage Category || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="WageCategory" CodeBehind="WageCategory.aspx.cs" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
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
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Wage Category</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" ValidationGroup="wage" runat="server" CausesValidation="true" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                            OnClick="lnkClose_Click" PostBackUrl="~/WageCategoryList.aspx"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click" CausesValidation="false">
                                                        <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" OnClick="lnkPrevious_Click" runat="server" CausesValidation="false">
                                                        <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" OnClick="lnkNext_Click" runat="server" CausesValidation="false">
                                                        <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" OnClick="lnkLast_Click" runat="server" CssClass="icon-last" CausesValidation="false">
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
                            <div id="accrdVendorInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Category Info.</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    <div class="section-ttle">General</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator166" runat="server" ControlToValidate="txtDesc" EnableClientScript="true"
                                                                    Display="None" ErrorMessage="Description Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender166" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator166">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtDesc" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtDesc">Description</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvmiscglexpRq" runat="server" ControlToValidate="txtGLAcct" EnableClientScript="true"
                                                                    Display="None" ErrorMessage="Misc. Exp Account Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvmiscglexpRq">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtGLAcct" runat="server" MaxLength="75" Placeholder="Search by acct# and name" CssClass="glAccount" />
                                                                <asp:HiddenField ID="hdnGLAcct" runat="server" />
                                                                <asp:Label runat="server" ID="lblCategoryMiscGLExp" AssociatedControlID="txtGLAcct">Misc. Exp Account</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvmilageglexpRq" runat="server" ControlToValidate="txtMileageAcct"
                                                                    Display="None" ErrorMessage="Milage Exp Account Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvmilageglexpRq">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtMileageAcct" runat="server" MaxLength="75" Placeholder="Search by acct# and name" CssClass="glAccount" />
                                                                <asp:HiddenField ID="hdnMilegAcct" runat="server" />
                                                                <asp:Label runat="server" ID="lblCategoryMilageGLExp" AssociatedControlID="txtMileageAcct">Milage Exp Account</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvreimbglexpRq" runat="server" ControlToValidate="txtReimbAcct"
                                                                    Display="None" ErrorMessage="Reimb. Exp Account Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvreimbglexpRq">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtReimbAcct" runat="server" MaxLength="75" Placeholder="Search by acct# and name" CssClass="glAccount" />
                                                                <asp:HiddenField ID="hdnReimbAcct" runat="server" />
                                                                <asp:Label runat="server" ID="lblCategoryReimbGLExp" AssociatedControlID="txtReimbAcct">Reimb. Exp Account</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>

                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvrzoneglexpRq" runat="server" ControlToValidate="txtZoneAcct"
                                                                    Display="None" ErrorMessage="Zone Exp Account Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvrzoneglexpRq">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtZoneAcct" runat="server" MaxLength="75" Placeholder="Search by acct# and name" CssClass="glAccount" />
                                                                <asp:HiddenField ID="hdnZoneAcct" runat="server" />
                                                                <asp:Label runat="server" ID="lblCategoryZoneGLExp" AssociatedControlID="txtZoneAcct">Zone Exp Account</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Status</label>
                                                                <asp:DropDownList ID="ddlWageStatus" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Project Specific</label>
                                                                <asp:DropDownList ID="ddlGlobal" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2 mgntp10">
                                                            <div class="row">
                                                                <asp:CheckBox ID="chkField" Text="Field" CssClass="css-checkbox" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="section-ttle">General</div>
                                                    <div class="form-section3">
                                                        <div class="section-ttle">Pay Rate</div>
                                                        <asp:Repeater ID="rptPayRate" runat="server">
                                                            <ItemTemplate>
                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <asp:Label runat="server" ID="lblPayRateType"><%# DataBinder.Eval(Container.DataItem, "WageRateTypeName") %>  </asp:Label>
                                                                        <asp:HiddenField ID="hdnWageRateType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WageRateTypeName") %>' />
                                                                        <asp:HiddenField ID="hdnWageRateTypeId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WageRateTypeId") %>' />
                                                                        <asp:TextBox ID="txtPayRateType" runat="server" Style="text-align: right" onkeypress="return numericOnly(this);" Text='<%# Eval("WageReateTypeValue") %>' />
                                                                        <asp:RegularExpressionValidator ID="REValidatorPayRateType" runat="server" ValidationExpression="((\d+)+(\.\d+))$" ErrorMessage="Please enter valid decimal number with any decimal places." ControlToValidate="txtPayRateType" Display="None">
                                                                        </asp:RegularExpressionValidator>
                                                                        <asp:ValidatorCalloutExtender ID="VCEPayRateType" runat="server" Enabled="True" TargetControlID="REValidatorPayRateType"></asp:ValidatorCalloutExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvPayRateType" runat="server" ControlToValidate="txtPayRateType" Display="None" ErrorMessage="Rate Required" SetFocusOnError="True" ValidationGroup="wage">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender ID="VCEPayRateType1" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvPayRateType">
                                                                        </asp:ValidatorCalloutExtender>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="form-sectioncustom1">
                                                        <div class="form-section2">
                                                            <div class="section-ttle">Burden Rate</div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="form-section2">
                                                            <div class="section-ttle">GL Exp Account</div>
                                                        </div>

                                                        <asp:Repeater ID="rptBurdenRate" runat="server">
                                                            <ItemTemplate>
                                                                <div class="row">
                                                                    <div class="input-field col s6-small" style="width: 50%">
                                                                        <asp:Label runat="server" ID="lblBurdenRateType"><%# DataBinder.Eval(Container.DataItem, "WageRateTypeName") %> </asp:Label>
                                                                        <asp:HiddenField ID="hdnWageRateType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WageRateTypeName") %>' />
                                                                        <asp:HiddenField ID="hdnWageRateTypeId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "WageRateTypeId") %>' />
                                                                        <asp:TextBox ID="txtBurdenRateType" runat="server" Style="text-align: right" onkeypress="return numericOnly(this);" Text='<%# Eval("WageReateTypeValue") %>' />
                                                                        <asp:RegularExpressionValidator ID="REValidatorBurdenRateType" runat="server" ValidationExpression="((\d+)+(\.\d+))$" ErrorMessage="Please enter valid decimal number with any decimal places." ControlToValidate="txtBurdenRateType" Display="None">
                                                                        </asp:RegularExpressionValidator>
                                                                        <asp:ValidatorCalloutExtender ID="VCEBurdenRateType" runat="server" Enabled="True" TargetControlID="REValidatorBurdenRateType"></asp:ValidatorCalloutExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvBurdenRateType" runat="server" ControlToValidate="txtBurdenRateType" Display="None" ErrorMessage="Rate Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender ID="VCEBurdenRateType1" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvBurdenRateType"></asp:ValidatorCalloutExtender>
                                                                    </div>
                                                                    <div class="input-field col s-gap"></div>
                                                                    <div class="input-field col s6-small" style="width: 50%">
                                                                        <asp:Label runat="server" ID="LblGL"> <%# DataBinder.Eval(Container.DataItem, "WageRateTypeName") %> GL Account </asp:Label>
                                                                        <asp:RequiredFieldValidator ID="rfvGL" runat="server" ControlToValidate="txtGL" Display="None" ErrorMessage="Please select GL account" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>
                                                                        <asp:ValidatorCalloutExtender ID="vceGL" runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvGL"></asp:ValidatorCalloutExtender>
                                                                        <asp:TextBox ID="txtGL" runat="server" MaxLength="75" placeholder="Search by acct# and name" CssClass="glAccount" Text='<%# Eval("WageRateGLName") %>' />
                                                                        <asp:HiddenField ID="hdnGL" runat="server" Value='<%# Eval("WageRateGLId") %>' />
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>

                                                    <div class="form-section3">
                                                        <div class="section-ttle" style="margin-bottom: 8px;">Subject To</div>
                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkFIT" CssClass="css-checkbox" Text="FIT" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkSIT" CssClass="css-checkbox" Text="SIT" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkFICA" CssClass="css-checkbox" Text="FICA" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkVacation" CssClass="css-checkbox" Text="Vacation" runat="server" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkMEDI" CssClass="css-checkbox" Text="MEDI" runat="server" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkWorkComp" CssClass="css-checkbox" Text="Work Comp" runat="server" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkFUTA" CssClass="css-checkbox" Text="FUTA" runat="server" />
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkUnion" CssClass="css-checkbox" Text="UNION" runat="server" />
                                                            </div>
                                                        </div>
                                                      <%--  <div class="input-field col s4 mgntp10">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkSick" CssClass="css-checkbox" Text="Sick" runat="server" />
                                                            </div>
                                                        </div>--%>
                                                    </div>

                                                    <div class="form-section3-blank">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="section-ttle">Remarks</div>
                                                        <asp:TextBox ID="txtRemark" runat="server" class="materialize-textarea" TextMode="MultiLine" MaxLength="8000"></asp:TextBox>
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
            <asp:HiddenField ID="hdnWageID" runat="server" />
            <asp:HiddenField ID="hdnAddEdit" runat="server" />
            <asp:HiddenField ID="hdnFlage" runat="server" />

        </div>
    </footer>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
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
                // Allow only 1 decimal point ('.')...  
                if ((elementRef.value) && (elementRef.value.indexOf('.') >= 0))
                    return false;
                else
                    return true;
            }
            return false;
        }
    </script>
    <script type="text/javascript"> 
        $(document).ready(function () {
            var glAccount = $('.glAccount');
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
            $(glAccount).autocomplete({
                source: function (request, response) {
                   // debugger;
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
                    $(this).val(ui.item.label);
                    $(this).closest('.input-field').find(':hidden').val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                   // debugger;
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
                        return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
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

    </script>
</asp:Content>
