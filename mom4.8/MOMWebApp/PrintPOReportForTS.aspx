<%@ Page Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="PrintPOReportForTS" Codebehind="PrintPOReportForTS.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /**/
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .highlight {
            background-color: Yellow;
        }

        .textBoxCCS {
            background: none;
            border: none;
        }

        .EmailBody {
            margin-left: 95px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Print Purchase Order</asp:Label></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                TabIndex="39" OnClick="lnkClose_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Text="Send Mail" Width="70px"
                        CausesValidation="False" />
                    <div>
                        <div runat="server" id="lnkCancelContact">
                        </div>
                        <div style="margin-left: 5px;" align="center">
                            <cc1:StiWebViewer ID="StiWebDesigner1" runat="server" ShowFileMenuExit="true"></cc1:StiWebViewer>
                            <%--<cc1:StiWebViewer ID="StiWebViewerCustomReport" runat="server" ScrollbarsMode="true" Visible="false" OnGetReport="StiWebViewerCustomReport_GetReport" 
               OnGetReportData="StiWebViewerCustomReport_GetReportData" />--%>
                        </div>
                    </div>
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
                                        <asp:Label CssClass="title_text" ID="Label8" runat="server">Mail Report</asp:Label>
                                        <a id="hideModalPopupViaClientButton" onclick="$find('programmaticModalPopupBehavior').hide()" href="#" style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
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
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>

