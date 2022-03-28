<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/HomeMaster.master" Inherits="MaddenServiceAgreement" Codebehind="MaddenServiceAgreement.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
        .styleDiv1 {
            float: left;
            width: 385px;
            margin-right: 15px;
        }

        .styleDiv2 {
            float: right;
            width: 100px;
        }
    </style>
    <script type="text/javascript">

        function showMailReport() {
            jQuery("#txtTo").text = "";
            jQuery("#txtCC").text = "";
            $("#programmaticModalPopup").show();
            $('#balancesheetpopup').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <%--<ul class="page-breadcrumb">
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
            </ul>--%>
            <div class="page-bar-right">
                
         
               
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                      <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Print Estimate</asp:Label></li>
                           <li><a id="LinkButton1" onclick="showMailReport();" class="icon-mail" title="Mail Report"></a></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                        TabIndex="39"  OnClick="lnkClose_Click" ></asp:LinkButton>

                            <%-- <asp:LinkButton CssClass="close_button_Form" ID="lnkClose" ForeColor="Red" runat="server" CausesValidation="false" 
                    OnClick="lnkClose_Click" TabIndex="14"><i class="fa fa-times"></i></asp:LinkButton>--%>
                        </li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div>
                        <div runat="server" id="lnkCancelContact">
                        </div>
                        <div style="margin-left: 5px;" align="center">
                            <rsweb:ReportViewer ID="rvServiceAgreement" runat="server" Width="100%" Height="1000px" OnReportRefresh="rvServiceAgreement_ReportRefresh"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="False" AsyncRendering="false"
                                ShowZoomControl="False">
                            </rsweb:ReportViewer>
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
                                    <table style="width: 100%; height:400px">
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

