<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LandMark.aspx.cs" Inherits="MOMWebApp.LandMark" %>

  
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="SHORTCUT ICON" href="Images/logoicon.png" />
    <title>Mobile Office Manager 5.0</title>
    <link href="css/MS_style_LandmarkEle.css?ver=1.0" rel="stylesheet" type="text/css" runat="server" id="loginCss" />
   
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
</head>
<body id="login_body">
    <form id="form1" runat="server" defaultbutton="btnLogin">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="wrap">
        <div class="login_box_div">
            <div class="login_box_outer">
                <div class="login_box_inner shadow">
                    <div class="login_box_inner_wht">
                        <div class="login_logo">
                            <div class="login_logo_bg" ></div>
                        </div>
                        <div align="center">
                             <div class="valid-box" runat="server" id="validation"></div>
                        <div class="valid-box" visible="false" id="divValidationBox" runat="server">
                            <asp:Label ID="lblMsg" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                            </div>
                        </div>
                        <div class="login_input_fields" runat="server" id="companyname">
                            <div class="login_lbl">
                                Company</div>
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="login_input" Width="198px">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="login_input_fields">
                            <div class="login_lbl">
                                User Name</div>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="login_input" ></asp:TextBox>
                                
                            <asp:FilteredTextBoxExtender ID="txtUsername_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                TargetControlID="txtUsername">
                            </asp:FilteredTextBoxExtender>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="login_input_fields">
                            <div class="login_lbl">
                                Password</div>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="login_input" TextMode="Password" ></asp:TextBox>
                              
                            <asp:FilteredTextBoxExtender ID="txtPassword_FilteredTextBoxExtender" runat="server"
                                Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                TargetControlID="txtPassword">
                            </asp:FilteredTextBoxExtender>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="sign_in_btn">
                            <asp:LinkButton ID="btnLogin" runat="server" OnClick="btnLogin_Click">                                                      
                                    <div class="sign_in_btn_bg" ></div>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="remember_me">
                        <asp:CheckBox ID="chkRemember" runat="server" Text="Remember me on this computer" />
                    </div>
                    <div class="forgot_password">
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
        BackgroundCssClass="pnlUpdateoverlay" RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff;
        border: solid; border-color: Silver;" CssClass="white_rounded">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td align="right">
                                <asp:LinkButton ID="btnOK" runat="server" OnClick="btnOK_Click1">Close</asp:LinkButton>
                            </td>
                        </tr>
                        <tr align="center">
                            <td>
                                <asp:Label ID="lblMSGRegistr" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px 10px 10px 10px; font-size: 14px;">
                                <asp:Label ID="lblTrial" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="lnkRegister" runat="server" OnClick="lnkRegister_Click">Register Now</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlReg" runat="server" Visible="false">
                                    <table>
                                        <tr align="center">
                                            <td>
                                                <asp:Label ID="lblKeylab" runat="server" Text="ID : "></asp:Label>
                                                <asp:Label ID="lblKey" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblSer" runat="server" Text="Enter Serial # : "></asp:Label>
                                                <asp:TextBox ID="txtSerial" runat="server" CssClass="register_input_bg"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSerial"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="reg"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnRegSerial" runat="server" OnClick="btnOK_Click" Text="Register"
                                                    ValidationGroup="reg" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Button runat="server" ID="btnHidden" Style="display: none" CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopupUser" BehaviorID="programmaticModalPopupBehavior2"
        TargetControlID="btnHidden" PopupControlID="pnlPopup" BackgroundCssClass="pnlUpdateoverlay"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlPopup" Style="display: none; background: #fff; border: solid;
        border-color: Silver; background-color: #fff;" CssClass="white_rounded">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div>
                    <asp:LinkButton ID="btncancel" runat="server" OnClick="btncancel_Click" Style="float: right;
                        margin-right: 15px;">Cancel</asp:LinkButton>
                    <table style="width: 100%;">
                        <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr align="center">
                            <td>
                                <asp:Label ID="lblMSgUser" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px 10px 10px 10px; font-size: 14px;">
                                <asp:Label ID="lblTRialUser" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnOKuser" runat="server" Text="OK" OnClick="btnOKuser_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="lnkRegisterUser" runat="server" OnClick="lnkRegisterUser_Click">Register Now</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlUserreg" runat="server" Visible="false">
                                    <table>
                                        <tr align="center">
                                            <td>
                                                <asp:Label ID="lblKeylabuser" runat="server" Text="ID : "></asp:Label>
                                                <asp:Label ID="lblKeyuser" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblSeruser" runat="server" Text="Enter Serial # : "></asp:Label>
                                                <asp:TextBox ID="txtSerialUser" runat="server" CssClass="register_input_bg"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSerialUser"
                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="reguser"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnRegserialUser" runat="server" Text="Register" ValidationGroup="reguser"
                                                    OnClick="btnRegserialUser_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
 
      <div class="cf"></div>
            <asp:Button runat="server" ID="btnHdnUpdateForgotPw" Style="display: none;" CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopupUpdateForgotPw" BehaviorID="programmaticModalPopupBehavior6"
                TargetControlID="btnHdnUpdateForgotPw" PopupControlID="pnlPopupUpdateForgotPw" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <div class="clearfix"></div>
            <asp:Panel runat="server" ID="pnlPopupUpdateForgotPw"  Width="600" Style="display: none; color: #fff !important; border-radius: 8px; background-color: #222; box-shadow: #222 0px 0px 8px 0px; padding-bottom: 15px;">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Reset Password</span>
                                <span style="float: right;">
                                    <asp:LinkButton ID="lnkCancelUpdateForgotPw" runat="server" OnClick="lnkCancelUpdateForgotPw_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblUpdateForgotPwMsg" CssClass="lblMsg" runat="server" ForeColor="#fff" Style="color: red; text-align: center; width: 100%; float: left;"></asp:Label>
                                <asp:Label Style="width: 100%; float: left;" ID="Label2" runat="server"></asp:Label>
                                <asp:Panel ID="Panel3" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                    <div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblFogotPwNew" runat="server" Text="New Password:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtFogotPwNew" runat="server" CssClass="register_input_bg" Cname="password" TextMode="Password" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                    Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                    TargetControlID="txtFogotPwNew">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblFogotPwConfirm" runat="server" Text="Confirm Password:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtFogotPwConfirm" runat="server" CssClass="register_input_bg" Cname="password" TextMode="Password" AutoComplete="off" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                                                    Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                    TargetControlID="txtFogotPwConfirm">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="col s8">&nbsp;</div>
                                        <div class="col s4">
                                            <asp:Button ID="btnUpdateFogotPw" CssClass="btn waves-effect waves-light col s12 light-blue" runat="server" Text="Submit"
                                                OnClick="btnUpdateFogotPw_Click" />
                                        </div>
                                    </div>

                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

      <div class="cf"></div>
            <asp:Button runat="server" ID="btnHdnUpdatePassword" Style="display: none;" CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopupUpdatePassword" BehaviorID="programmaticModalPopupBehavior3"
                TargetControlID="btnHdnUpdatePassword" PopupControlID="pnlPopupUpdatePassword" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <div class="clearfix"></div>
            <asp:Panel runat="server" ID="pnlPopupUpdatePassword" Width="600" Style="display: none; color: #fff !important; border-radius: 8px; background-color: #222; box-shadow: #222 0px 0px 8px 0px; padding-bottom: 15px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Update password</span>
                                <span style="float: right;">
                                    <asp:LinkButton ID="lnkCancelUpdate" runat="server" OnClick="lnkCancelUpdate_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblUpdatePwErrMsg" CssClass="lblMsg" runat="server" ForeColor="#fff" Style="color: red; text-align: center; width: 100%; float: left;"></asp:Label>
                                <asp:Label Style="width: 100%; float: left;" ID="lblUpdateMessage" runat="server"></asp:Label>
                                <asp:Panel ID="Panel2" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                    <div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblCurrentPw" runat="server" Text="Current Password:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtCurrentPw" runat="server" CssClass="register_input_bg" onkeydown = "return (event.keyCode!=13);" Cname="password" TextMode="Password" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                    Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                    TargetControlID="txtCurrentPw">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblNewPasswrod" runat="server" Text="New Password:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="register_input_bg" onkeydown = "return (event.keyCode!=13);" Cname="password" TextMode="Password" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                    Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                    TargetControlID="txtNewPassword">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="register_input_bg"  onkeydown = "return (event.keyCode!=13);" Cname="password" TextMode="Password" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                    Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                    TargetControlID="txtConfirmPassword">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                           <div class="row">
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkPasswordResetting" OnClick="lnkPasswordResetting_Click" runat="server" Text="Forgot Password"></asp:LinkButton>
                        </span>
                    </div>
                                        <asp:Button ID="btnUpdatePassword" CssClass="btn waves-effect waves-light col s12 light-blue" runat="server" Text="Update" ValidationGroup="reguser"
                                            OnClick="btnUpdatePassword_Click" />
                                    </div>

                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

       <div class="cf"></div>
            <asp:Button runat="server" ID="btnHdnForgotPassword" Style="display: none;" CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopupForgotPassword" BehaviorID="programmaticModalPopupBehavior5"
                TargetControlID="btnHdnForgotPassword" PopupControlID="pnlPopupForgotPassword" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <div class="clearfix"></div>
            <asp:Panel runat="server" ID="pnlPopupForgotPassword" Width="600" Style="display: none; color: #fff !important; border-radius: 8px; background-color: #222; box-shadow: #222 0px 0px 8px 0px; padding-bottom: 15px;">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Forgot Password</span>
                                <span style="float: right;">
                                    <asp:LinkButton ID="lnkCancelForgotPw" runat="server" OnClick="lnkCancelForgotPw_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblForgotPwMsg" CssClass="lblMsg" runat="server" ForeColor="#fff" Style="color: red; text-align: center; width: 100%; float: left;">
                                        Please input registered Email Address, an email will be sent to the following Email
                                </asp:Label>
                                <asp:Label Style="width: 100%; float: left;" ID="Label7" runat="server"></asp:Label>
                                <asp:Panel ID="pnlForgotPw" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblForgotUsername" runat="server" Text="User Name:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtForgotUsername" runat="server" CssClass="register_input_bg" onkeydown = "return (event.keyCode!=13);" AutoComplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtForgotUsername"
                                                    Display="None" ErrorMessage="User Name Required" SetFocusOnError="True" ValidationGroup="forgotPassword"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator6">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                        </div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblForgotEmail" runat="server" Text="Email Address:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtForgotEmail" runat="server" CssClass="register_input_bg" onkeydown = "return (event.keyCode!=13);" AutoComplete="off"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                    ControlToValidate="txtForgotEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="forgotPassword"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                </asp:ValidatorCalloutExtender>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                    TargetControlID="txtForgotEmail">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtForgotEmail"
                                                    Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="forgotPassword"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                        </div>

                                        <div class="col s8">&nbsp;</div>
                                        <div class="col s4">
                                            <asp:Button ID="btnForgotPassword" CssClass="btn waves-effect waves-light col s12 light-blue" ValidationGroup="forgotPassword" runat="server" Text="Submit"
                                                OnClick="btnForgotPassword_Click" />
                                        </div>
                                        
                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

        <div class="cf"></div>
            <asp:Button runat="server" ID="btnHdnResetPassword" Style="display: none;" CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopupResetPassword" BehaviorID="programmaticModalPopupBehavior4"
                TargetControlID="btnHdnResetPassword" PopupControlID="pnlPopupResetPassword" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <div class="clearfix"></div>
            <asp:Panel runat="server" ID="pnlPopupResetPassword" Width="600"  Style="display: none; color: #fff !important; border-radius: 8px; background-color: #222; box-shadow: #222 0px 0px 8px 0px; padding-bottom: 15px;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Reset Password</span>
                                <span style="float: right;">
                                    <asp:LinkButton ID="lnkCancelReset" runat="server" OnClick="lnkCancelReset_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblResetPwMsg" CssClass="lblMsg" runat="server" ForeColor="#fff" Style="color: red; text-align: center; width: 100%; float: left;">
                                        Please input the following details to request for Reset Password to Admin
                                </asp:Label>
                                <asp:Panel ID="pnlResetPw" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                    <div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblResetUserName" runat="server" Text="User Name:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtResetUserName" runat="server" CssClass="register_input_bg" onkeydown = "return (event.keyCode!=13);" AutoComplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtResetUserName"
                                                    Display="None" ErrorMessage="User Name Required" SetFocusOnError="True" ValidationGroup="resetPassword"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                        </div>
                                        <div class="row margin">
                                            <div class="input-field col s4" style="padding-top: 20px;">
                                                <asp:Label ID="lblResetEmailAdmin" runat="server" Text="Email Administrator:"></asp:Label>
                                            </div>
                                            <div class="input-field col s8">
                                                <asp:TextBox ID="txtResetEmailAdmin" runat="server" CssClass="register_input_bg"  onkeydown = "return (event.keyCode!=13);" AutoComplete="off"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="resetPassword"
                                                    ControlToValidate="txtResetEmailAdmin" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                                </asp:ValidatorCalloutExtender>
                                                <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                    TargetControlID="txtResetEmailAdmin">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtResetEmailAdmin"
                                                    Display="None" ErrorMessage="Email Administrator Required" SetFocusOnError="True" ValidationGroup="resetPassword"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                        </div>
                                        <div class="col s8">&nbsp;</div>
                                        <div class="col s4">
                                            <asp:Button ID="btnResetPassword" CssClass="btn waves-effect waves-light col s12 light-blue" ValidationGroup="resetPassword" runat="server" Text="Submit"
                                                OnClick="btnResetPassword_Click" />
                                        </div>
                                    </div>

                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
       </form>
</body>
</html>
