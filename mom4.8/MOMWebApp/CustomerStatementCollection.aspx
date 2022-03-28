<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="CustomerStatementCollection" Codebehind="CustomerStatementCollection.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body:nth-of-type(1) img[src*="Blank.gif"] {
            display: none;
        }

        table input[type="text"], input[type="password"], input[type="email"], input[type="url"], input[type="time"], input[type="date"], input[type="datetime-local"], input[type="tel"], input[type="number"], input[type="search"] {
            background-color: white !important;
            margin: 0px !important;
            height: 17px !important;
        }

        td, th {
            padding: 0px 5px !important;
            display: table-cell;
            text-align: left;
            vertical-align: middle;
        }

        body {
            background-color: white !important;
        }
        .com-cont{
            padding-left: 5px;
            margin-bottom: 10px;
        }

        .btn-reload{
            border: 0.5px solid #1C5FB1 !important;
            color: #1C5FB1 !important;
            padding: 5px 7px 5px 7px !important;
            border-radius: 3px !important;
            width: auto !important;
            font-size: 1.2em !important;
            background-image: url(../images/accrd.gif) !important;
            background-repeat: repeat-x !important;
            line-height: 15px !important;
            margin-left: 13px;
            margin-top: 2px !important;
        }
    </style>
    <script type="text/javascript">

        function pageLoad() {

            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
            //$addHandler($get("A1"), 'click', hideModalPopupViaClientCust);

        }
        function dispWarningMesg() {
            $('#myModal').modal('show');
        }
        function hideModel() {
            $('#myModal').modal({
                show: 'false'
            });
            $('#myModal').fadeOut('slow');
            //$('#btnModelClose').click();
        }
        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        function unsuccessMesg(strLoc) {
            noty({ text: 'Email sent unsuccessfully to ' + strLoc + '!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 50000, theme: 'noty_theme_default', closable: false });
        }

        function Confirm(strMess) {
            var returnValue;

            if (confirm(strMess) === true) {
                returnValue = true;
            } else {
                returnValue = false;
            }

            return returnValue;
        }
    </script>
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
                                            <div class="page-title">
                                                <i class="mdi-editor-insert-drive-file"></i>&nbsp;
                                               <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Customer Statement Report</asp:Label>
                                            </div>                          
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkMailReport" runat="server" OnClick="lnkMailReport_Click" OnClientClick="return Confirm('Are you want to email all?');">Email All</asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkPrintOnly" runat="server" OnClick="lnkPrintOnly_Click">Print Only</asp:LinkButton>
                                                </div>
                                            </div>

                                            <div class="btnlinks">
                                                <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                                </a>
                                            </div>
                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <a href="CustomersReport.aspx?type=Customer" class="-text">Add New Report</a>
                                                </li>
                                                <li>
                                                    <a href="InvoicesReport.aspx" class="-text">Invoice Summary Report</a>
                                                </li>
                                            </ul>
                                            <div class="btnclosewrap">
                                                <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
            <div class="com-cont">
                <asp:CheckBox ID="chkIncludeCredit" Text="Include Credits" runat="server" GroupName="rdExpColl" Checked="true" />
                <asp:CheckBox ID="chkIncludeCustomerCredit" Text="Include Customer with Credits" runat="server" GroupName="rdExpColl" Checked="true" />

                <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" CssClass="btn-reload"  CausesValidation="false" ToolTip="Load Report"><i class="fa fa-refresh"></i></asp:LinkButton>
            </div>
           
            <div>
                <asp:UpdatePanel ID="updCustomer" runat="server">
                    <ContentTemplate>
                        <rsweb:ReportViewer ID="rvCustomer" runat="server" Width="1280px" Height="1500px"
                            BorderColor="Gray" BorderStyle="None" BorderWidth="1px" PageCountMode="Actual"
                            ShowZoomControl="False" OnReportRefresh="rvCustomer_ReportRefresh">
                        </rsweb:ReportViewer>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkMailReport" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <telerik:RadWindowManager ID="RadWindowManagerCustomer" runat="server">
        <Windows>
            <telerik:RadWindow ID="mailWindow" Skin="Material" VisibleTitlebar="true" Title="Mail" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="450">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="form-section-row">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                            TargetControlID="txtFrom">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ControlToValidate="txtFrom" Display="None"
                                            ErrorMessage="Invalid E-Mail Address"
                                            ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                            ValidationGroup="mail"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="txtFrom" Display="None"
                                            ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                            ValidationGroup="mail"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtFrom">From</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                            TargetControlID="txtTo">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ControlToValidate="txtTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                            ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                            ValidationGroup="mail"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtTo" Display="None"
                                            ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                            ValidationGroup="mail"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtTo"> To</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row">

                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">

                                        <asp:FilteredTextBoxExtender ID="txtCC_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                            TargetControlID="txtCC">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ControlToValidate="txtCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                            ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                            ValidationGroup="mail"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtCC" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtCC">CC</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtCC">Subject</asp:Label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="form-section-row">

                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:Label runat="server" AssociatedControlID="txtBody">Mail Body</asp:Label>
                                        <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" CssClass="materialize-textarea" Text="This is report email sent from Mobile Office Manager. Please find the AR Aging Report By Due Date attached."></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                        </div>


                        <div style="clear: both;"></div>

                        <footer style="float: right;">
                            <div class="btnlinks">
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Save" OnClick="hideModalPopupViaServerConfirm_Click"
                                    ValidationGroup="mail" />
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>

