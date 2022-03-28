<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="CustomerStatement" CodeBehind="CustomerStatement.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body:nth-of-type(1) img[src*="Blank.gif"] {
            display: none;
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
            noty({ text: 'Mail sent unsuccessfully to ' + strLoc + '!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 50000, theme: 'noty_theme_default', closable: false });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Customer Statement Report</asp:Label></li>
                        <%-- <li><a id="LinkButton1" onclick="showMailReport();" title="Mail Report" class="icon-mail"></a></li>--%>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print"
                                        data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                    <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                        <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                        </a>
                                        </li>
                                        <li style="margin-left: 0px;"><a href="InvoicesReport.aspx"><span>Invoice Summary Report</span><div style="clear: both;"></div>
                                        </a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="close"
                                OnClick="lnkClose_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
            <!-- Modal -->
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="container">
                        <div class="modal fade" id="myModal" role="dialog">
                            <div class="modal-dialog">
                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header" style="background-color: #316b9d">
                                        <label class="modal-title" style="color: white; font-size: large">Email Status</label>
                                        <button type="btnModelClose" class="close" data-dismiss="modal">&times;</button>

                                    </div>
                                    <div class="modal-body">
                                        <label class="modal-title" style="font-size: medium">
                                            Would you like to print customer statement for accounts with no EmailId ? 
                                        </label>
                                        <br />
                                        <br />
                                        <asp:Button ID="btnYes" Text="Yes" runat="server" OnClick="btnYes_Click" CssClass="btn btn-primary" />
                                        <button type="button" class="btn btn-primary" data-dismiss="modal">No</button>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lnkMailReport" />
                </Triggers>
            </asp:UpdatePanel>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="sc-form">
                        <span style="margin-left: 15px;">
                            <asp:CheckBox ID="chkIncludeCredit" Text="Include Credits" runat="server" GroupName="rdExpColl" Checked="true" />
                            <asp:CheckBox ID="chkIncludeCustomerCredit" Text="Include Customer with Credits" runat="server" GroupName="rdExpColl" Checked="true" />
                        </span>
                        <asp:LinkButton ID="lnkSearch" runat="server" CssClass="btn submit" OnClick="lnkSearch_Click" CausesValidation="false" ToolTip="Load Report" style="margin-left: 15px;"><i class="fa fa-refresh"></i></asp:LinkButton>
                    </div>
                    <div class="page-bar-right">
                        <asp:LinkButton ID="lnkMailReport" runat="server" OnClick="lnkMailReport_Click">Mail All</asp:LinkButton>
                        <asp:LinkButton ID="lnkPrintOnly" runat="server" OnClick="lnkPrintOnly_Click">Print Only</asp:LinkButton>
                    </div>

                    <div class="table-scrollable" style="border: none">
                        <div class="clearfix"></div>
                        <div class="col-lg-12 col-md-12">

                            <asp:UpdatePanel ID="updCustomer" runat="server">
                                <ContentTemplate>
                                    <rsweb:ReportViewer ID="rvCustomer" runat="server" Width="1280px" Height="1500px"
                                        BorderColor="Gray" BorderStyle="None" BorderWidth="1px" PageCountMode="Actual"
                                        ShowZoomControl="False" OnReportRefresh="rvCustomer_ReportRefresh">
                                        <%--ShowPageNavigationControls="false"--%>
                                    </rsweb:ReportViewer>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lnkMailReport" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                        PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="pnlUpdateoverlay"
                        DropShadow="false" RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel runat="server" ID="programmaticPopup" Style="display: block; background: #fff; border: 1px solid #316b9d; width: 550px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; background-color: #DDDDDD;">
                                    <div class="model-popup-body">
                                        <asp:Label CssClass="title_text" ID="Label8" runat="server">Mail Invoice</asp:Label>
                                        <a id="hideModalPopupViaClientButton" href="#" style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                        <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                            Style="float: right; color: #fff; margin-left: 10px;"
                                            ValidationGroup="mail" />
                                    </div>
                                </asp:Panel>
                                <div style="padding: 20px;">
                                    <table style="width: 100%; height: 400px">
                                        <tr>
                                            <td>From</td>
                                            <td>
                                                <asp:TextBox ID="txtFrom" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>To</td>
                                            <td>
                                                <asp:TextBox ID="txtTo" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CC</td>
                                            <td>
                                                <asp:TextBox ID="txtCC" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Subject</td>
                                            <td>
                                                <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtBody" CssClass="form-control" runat="server" TextMode="MultiLine"
                                                    Height="200px" Width="450px"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
