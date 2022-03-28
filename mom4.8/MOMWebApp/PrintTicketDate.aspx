<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/HomeMaster.master" Inherits="PrintTicketDate" Codebehind="PrintTicketDate.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function cancel() {
            window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
        }

        function pageLoad() {
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
            $addHandler($get("A1"), 'click', hideModalPopupViaClientCust);
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="#">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Customer Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Locations</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Edit Locations</span>
                </li>
            </ul>
            <div class="page-bar-right">
                <a runat="server" id="lnkCancelContact" href="scheduler.aspx"
                    tabindex="24">Back</a>

                <asp:LinkButton ID="LinkButton1" runat="server" 
                    OnClick="LinkButton1_Click">Mail Report</asp:LinkButton>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <span>Print Ticket </span>
                    <asp:RadioButtonList ID="rbSelect" style="float:right" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbSelect_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="0">Detailed Ticket Report</asp:ListItem>
                        <asp:ListItem Value="1">Detailed Ticket Report by Worker</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="search-customer">
                        <div class="sc-form">
                            Worker
                            <asp:DropDownList ID="ddlworker" runat="server" TabIndex="14" CssClass="form-control input-sm input-small">
                            </asp:DropDownList>
                            Start Date
                             <asp:TextBox ID="txtStart" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                            <asp:CalendarExtender ID="txtStart_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtStart">
                            </asp:CalendarExtender>
                            End Date
                            <asp:TextBox ID="txtEnd" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                            <asp:CalendarExtender ID="txtEnd_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtEnd">
                            </asp:CalendarExtender>
                            <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary" style="padding:5px !important" TabIndex="23">Get report</asp:LinkButton>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="ddlworker"
                                Display="None" ErrorMessage="Worker Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator23_ValidatorCalloutExtender"
                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator23">
                            </asp:ValidatorCalloutExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtStart"
                                Display="None" ErrorMessage="Start date Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator21_ValidatorCalloutExtender"
                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator21">
                            </asp:ValidatorCalloutExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtEnd"
                                Display="None" ErrorMessage="End date required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator22_ValidatorCalloutExtender"
                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator22">
                            </asp:ValidatorCalloutExtender>
                        </div>
                    </div>
                    <div>
                        <table style="width: 964px;" runat="server" id="tblfilter">
                        </table>
                        &nbsp;<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="550px"
                            BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="False" AsyncRendering="false"
                            ShowZoomControl="False">
                        </rsweb:ReportViewer>
                    </div>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                        PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="pnlUpdateoverlay"
                        DropShadow="false" RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel runat="server" ID="programmaticPopup" Style="display: block; background: #fff; border: 1px solid #316b9d; width: 550px;">
                        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move;">
                            <div class="title_bar_popup">
                                <asp:Label ID="Label8" CssClass="title_text" runat="server" style="color:white">Mail Report</asp:Label>
                                <a id="hideModalPopupViaClientButton" href="#" style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                    Style="float: right; color: #fff; margin-left: 10px;"
                                    ValidationGroup="mail" />
                            </div>
                        </asp:Panel>
                        <div style="padding: 20px;">
                            <table style="width: 100%; height:200px">
                                <tr>
                                    <td>From</td>
                                    <td>
                                        <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" Width="400px"></asp:TextBox>
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
                                        <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" Width="400px"></asp:TextBox>
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
                                        <asp:TextBox ID="txtCC" runat="server" CssClass="form-control" Width="400px"></asp:TextBox>
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
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
