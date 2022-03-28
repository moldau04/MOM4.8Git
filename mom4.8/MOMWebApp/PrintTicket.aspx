<%@ Page Language="C#" AutoEventWireup="true" Inherits="PrintTicket"
    MasterPageFile="~/HomeMaster.master" Codebehind="PrintTicket.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function cancel() {
            //window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
            debugger
            var conf = confirm('Do you want to close the ticket screen?');
            if (conf) window.close();
        }

        function pageLoad() {
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        //        $(document).ready(function() {
        //        if (!$.browser.msie) {
        //                try {
        //                    var ControlName = 'ReportViewer1';
        //                    var innerScript = '<scr' + 'ipt type="text/javascript">document.getElementById("' + ControlName + '_print").Controller = new ReportViewerHoverButton("' + ControlName + '_print", false, "", "", "", "#ECE9D8", "#DDEEF7", "#99BBE2", "1px #ECE9D8 Solid", "1px #336699 Solid", "1px #336699 Solid");</scr' + 'ipt>';
        //                    var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="images/Reserved_ReportViewerWebControl.gif" title="Print"></td></tr></tbody>';
        //                    var innerTable = '<table title="Print" onmouseout="this.Controller.OnNormal();" onmouseover="this.Controller.OnHover();" onclick="PrintFunc(\'' + ControlName + '\'); return false;" id="' + ControlName + '_print" style="border: 1px solid rgb(236, 233, 216); background-color: rgb(236, 233, 216); cursor: default;">' + innerScript + innerTbody + '</table>'
        //                    var outerScript = '<scr' + 'ipt type="text/javascript">document.getElementById("' + ControlName + '_print").Controller.OnNormal();</scr' + 'ipt>';
        //                    var outerDiv = '<div style="display: inline; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + outerScript + '</td></tr></tbody></table></div>';

        //                    $("#" + ControlName + " > div > div").append(outerDiv);

        //                }
        //                catch (e) { alert(e); }
        //            }
        //        });

        //        function PrintFunc(ControlName) {
        //            setTimeout('ReportFrame' + ControlName + '.print();', 100);
        //        }

    </script>
    <style>
        #programmaticModalPopupBehavior_backgroundElement {
            background: rgba(0,0,0,0.5);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Print Ticket</asp:Label>
                        </li>
                        <li>
                            <div runat="server" id="lnkCancelContact">
                                <ul>
                                    <li style="margin-right:0">
                                        <asp:CheckBox ID="chkCoord" Text="Coordinates"
                                            runat="server" AutoPostBack="True" OnCheckedChanged="chkCoord_CheckedChanged"
                                            ToolTip="Show address on site times." /></li>
                                    <li>
                                        <asp:LinkButton ID="lnkMail" title="Send Email" CssClass="icon-mail" runat="server"
                                            OnClick="lnkMail_Click"></asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="lnkClose"   CssClass="icon-closed" runat="server"
                                            OnClick="lnkClose_Click"></asp:LinkButton>
                                    </li>
                                     <%--<li>
                                        <asp:LinkButton ID="A1"   CssClass="icon-closed" runat="server"
                                            OnClientClick="cancel();"></asp:LinkButton>

                                     </li>--%>
                                        
                                       <%-- <a runat="server" id="A1" href="#" onclick="cancel();" CssClass="icon-closed"
                                        tabindex="24"><i class="fa fa-times"></i></a>--%> 
                                </ul>
                            </div>
                        </li>
                    </ul>
                </div>
            </div> 
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div>
                        <div style="margin-left: 150px;">
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="85%" Height="727px"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="False"
                                AsyncRendering="false" ShowZoomControl="False">
                            </rsweb:ReportViewer>
                        </div>
                    </div>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                        PopupDragHandleControlID="programmaticPopupDragHandle"
                        DropShadow="false" RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: 1px solid #316b9d; width: 550px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel runat="Server" ID="programmaticPopupDragHandle">
                                    <div class="model-popup-body">
                                        <asp:Label CssClass="title_text" ID="Label8" runat="server">Email Ticket</asp:Label>
                                        <a id="hideModalPopupViaClientButton" href="#" style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                        <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                            Style="float: right; color: #fff; margin-left: 10px;" ValidationGroup="mail" />
                                    </div>
                                </asp:Panel>
                                <div style="padding: 20px;">
                                    <table style="width: 100%; height: 400px">
                                        <tr>
                                            <td>From
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFrom" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtFrom">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtFrom"
                                                    Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                                </asp:ValidatorCalloutExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFrom"
                                                    Display="None" ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                </asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>To
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" Width="400px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                    FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtTo">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTo"
                                                    Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                                </asp:ValidatorCalloutExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTo"
                                                    Display="None" ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                </asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CC
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCC" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtCC_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                    FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtCC">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCC"
                                                    Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                </asp:ValidatorCalloutExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Subject
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtBody" CssClass="form-control" runat="server" TextMode="MultiLine" Height="200px" Width="450px"></asp:TextBox>
                                            </td>
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
