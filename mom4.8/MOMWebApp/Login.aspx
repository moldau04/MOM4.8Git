<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html lang="en">
<head runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="msapplication-tap-highlight" content="no" />
    <title>ESS - LOGIN</title>
    <link href="Design/css/layouts/page-center.css" rel="stylesheet" />
    <link href="Design/css/custom/custom.css" rel="stylesheet" />
    <link href="Design/css/materialize.css" rel="stylesheet" />
    <link href="Design/css/style.css" rel="stylesheet" />
    <link href="Appearance/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <script src="Design/js/plugins/jquery-1.11.2.min.js"></script>
    <%--<script  src="https://code.jquery.com/jquery-1.11.2.min.js"></script>--%>
    <script src="Design/js/materialize.js"></script>
    <script src="Design/js/plugins/prism/prism.js"></script>
    <%--<script src="Design/js/plugins.js"></script>--%>


    <style>
        video {
            position: fixed;
            top: 50%;
            left: 50%;
            height: 100vh;
            width: auto;
            height: auto;
            z-index: -100;
            transform: translateX(-40%) translateY(-40%);
            background-size: cover;
            transition: 1s opacity;
            min-width: 130%;
        }

        .mgntop {
            margin-top: 40px !important;
        }

        .mgnbtm {
            margin-bottom: 15px !important;
        }

        .stopfade {
            opacity: .5;
        }

        input:-webkit-autofill,
        input:-webkit-autofill:hover,
        input:-webkit-autofill:focus,
        input:-webkit-autofill:active {
            transition: background-color 5000s ease-in-out 0s;
            -webkit-text-fill-color: white !important;
        }

        #polina {
            font-family: Agenda-Light, Agenda Light, Agenda, Arial Narrow, sans-serif;
            font-weight: 100;
            background: rgba(0,0,0,0.3);
            color: white;
            padding: 2rem;
            width: 33%;
            margin: 2rem;
            float: right;
            font-size: 1.2rem;
        }

        h1 {
            font-size: 3rem;
            text-transform: uppercase;
            margin-top: 0;
            letter-spacing: .3rem;
        }

        #polina button {
            display: block;
            width: 80%;
            padding: .4rem;
            border: none;
            margin: 1rem auto;
            font-size: 1.3rem;
            background: rgba(255,255,255,0.23);
            color: #fff;
            border-radius: 3px;
            cursor: pointer;
            transition: .3s background;
        }

            #polina button:hover {
                background: rgba(0,0,0,0.5);
            }

        a {
            display: inline-block;
            color: #fff;
            text-decoration: none;
            background: rgba(0,0,0,0.5);
            padding: .5rem;
            transition: .6s background;
        }

            a:hover {
                background: rgba(0,0,0,0.9);
            }

        .lgpd {
            padding: 10px !important;
            padding-bottom: 30px !important;
            padding-top: 30px !important;
        }

        #lnkPasswordResetting {
            background: none;
            font-size: 1.2rem;
            width:inherit;
            text-align:center;
            color:#03a9f4 !important;
        }

        i>#btnForgotPassword,
        i>#btnUpdateFogotPw,
        i>#btnResetPassword,
        i>#btnUpdatePassword
        {
            height:inherit;
            width: inherit;
        }
        .ajax__validatorcallout_error_message_cell{
            color:black;
        }

        @media screen and (max-device-width: 800px) {
            html {
            }

            #bgvid {
                display: none;
            }
        }

        @media screen and (max-width:3000px) {
            video {
                position: fixed;
                top: 30%;
                left: 40%;
                height: auto;
                z-index: -100;
                transform: translateX(-40%) translateY(-40%);
                background-size: cover;
                transition: 1s opacity;
                min-width: 160%;
            }
        }



        @media screen and (max-width: 768px) {
            .loginlogowrap {
                float: left !important;
                clear: left !important;
                width: 100% !important;
            }

            .loginformwrap {
                float: left !important;
                clear: left !important;
                width: 100% !important;
            }

            .loginalign {
                margin-left: -185px !important;
            }

            .lgpd {
                padding: 0 150px 50px 150px !important;
            }
        }

        @media screen and (max-width: 500px) {
            .lgpd {
                padding: 0 170px 50px 170px !important;
            }
        }

        span.lgpbtnClose{
            float: right;margin-top:-10px;
        }
            span.lgpbtnClose > a {
                width:inherit;
                border-top-right-radius:8px;
                padding: .7rem;
            }

        .lblpErrMsg{
            color:#03a9f4 !important;
            text-align: center; width: 100%; float: left;
            /*min-height: 30px;*/
        }

        .lblpMsg {
            width: 100%;
            float: left;
            text-align: center;
        }

        #pnlPopupForgotPassword,
        #pnlPopupResetPassword,
        #pnlPopupUpdateForgotPw{
            background: rgba(0, 0, 0, 0.59);
            border-radius: 8px; 
            box-shadow: #222 0px 0px 8px 0px; 
            padding-bottom: 15px;
            color: #fff !important;
            min-height:200px !important;
        }
        
        .errorMessMargin{
            margin-top:40px;
        }
    </style>
    <script>
        function pageLoad(sender, args) {
            $(function () {
                Materialize.updateTextFields();
            });
        }
        function showLoginForm() {
            $('#loginbox').show();

        }
        function hideLoginForm() {
            $('#loginbox').hide();
        }

        function addRemoveMarginForErrorMessage(isAdd, messControlID) {
            var messControl = $(messControlID);
            if (messControl != null) {
                if (isAdd == true) {
                    messControl.addClass('errorMessMargin');
                } else {
                    messControl.removeClass('errorMessMargin');
                }
            }
        }
    </script>
</head>
<body>
    <form method="post" id="form1" defaultbutton="btnLogin" class="login-form" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
        </asp:ToolkitScriptManager>
        <div id="login-page" class="row loginalign">
            <video id="bgvid" playsinline autoplay muted loop>
                <source src="Design/images/vid.mp4" type="video/webm">
                <source src="Design/images/vid.mp4" type="video/mp4">
            </video>
            <div id="loginbox" class="col s12 z-depth-4 card-panel test-width lgpd">
                <div class="col s5 loginlogowrap">
                    <div class="input-field col s12 center">
                        <img src="Design/images/login-logo.png" alt="" class="circle responsive-img valign profile-image-login logologin mgntop">
                        <p class="center login-form-text webkitmgnnil">
                            MOBILE OFFICE MANAGER
                            <br />
                            v5.0
                        </p>
                    </div>
                </div>
                <div class="col s7 loginformwrap">
                    <div class="row margin mgnbtm">
                        <div class="input-field col s12">
                            <i class="mdi-communication-business prefix"></i>
                            <label for="drp" class="drpdwn-label drpdwn-label-white lbl-big">Company Name</label>
                            <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" autocomplete="off" class="browser-default mgnbtm dbdropdown" Style="padding-left: 8px; margin-left: 3rem; width: calc(100% - 3rem); margin-top: 20px !important; border-bottom: 2px solid #fff;">
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="row margin mgnbtm">
                        <div class="input-field col s12">
                            <i class="mdi-social-person-outline prefix"></i>
                            <div>
                                <label class="center-align drpdwn-label drpdwn-label-white drpdwn-labelpd40  lbl-big">Username</label>
                            </div>
                            <asp:TextBox ID="txtUsername" runat="server" name="username" autocomplete="off" Style="padding-left: 8px; margin-top: 10px !important; border-bottom: 2px solid #fff;" CssClass="mgnbtm"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtUsername_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                TargetControlID="txtUsername">
                            </asp:FilteredTextBoxExtender>
                        </div>
                    </div>
                    <div class="row margin mgnbtm">
                        <div class="input-field col s12">
                            <i class="mdi-action-lock-outline prefix"></i>
                            <div>
                                <label class="center-align drpdwn-label drpdwn-label-white drpdwn-labelpd40 lbl-big">Password</label>
                            </div>
                            <asp:TextBox ID="txtPassword" runat="server" Style="padding-left: 8px; margin-top: 10px !important; border-bottom: 2px solid #fff;" Cname="password" TextMode="Password" AutoComplete="off" CssClass="mgnbtm"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtPassword_FilteredTextBoxExtender" runat="server"
                                Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                TargetControlID="txtPassword">
                            </asp:FilteredTextBoxExtender>

                        </div>
                    </div>
                    <div class="row">
                        <span class="col s12">
                            <asp:LinkButton ID="lnkPasswordResetting" OnClick="lnkPasswordResetting_Click" runat="server" Text="Forgot Password"></asp:LinkButton>
                        </span>
                    </div>
                    <div class="row">
                        <div class="valid-box" runat="server" id="validation"></div>
                        <div class="valid-box" visible="false" id="divValidationBox" runat="server">
                            <button data-close="alert" class="close" style="display: none;"></button>
                            <span>
                                <asp:Label ID="lblMsg" CssClass="lblMsg" runat="server" Style="color: red; text-align: center; width: 100%; float: left;"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <%--<div class="row">
                        <span class="col s12">
                            <asp:Label ID="lblUpdatePasswordMsg" CssClass="lblMsg" runat="server"  Style="color: red; text-align: center; width: 100%; float: left;"></asp:Label>
                        </span>
                    </div>--%>

                    <div class="row margin">
                        <div class="input-field col s7">
                            <div class="switch" style="color: #fff;">
                                <label class="drpdwn-label-white">

                                    <asp:CheckBox ID="chkRemember" runat="server" />
                                    Remember me
                                    <span class="lever"></span>
                                </label>
                            </div>
                        </div>
                        <div class="input-field col s5" style="float: right;">
                            <asp:LinkButton ID="btnLogin" class="btn waves-effect waves-light col s12 light-blue" Style="border-radius: 6px !important;" runat="server" OnClick="btnLogin_Click"> Login </asp:LinkButton>
                        </div>
                    </div>
                </div>

            </div>
            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" class="btn green-haze pull-right"
                CausesValidation="False" Style="display: none" />

            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; color: #fff !important; border-radius: 8px; background-color: #222; box-shadow: #222 0px 0px 8px 0px; padding-bottom: 15px;">
                <div class="section-ttle-popupheading">
                    <span>Register
                    </span>
                    <span class="lgpbtnClose">
                        <asp:LinkButton ID="btnOK" runat="server" OnClick="btnOK_Click1" Style="float: right" CssClass="regi-close"><i class="fa fa-times"></i></asp:LinkButton>
                    </span>

                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="regi-box modal-content company-modal">
                            <asp:Label ID="lblMSGRegistr" CssClass="lblMsg" runat="server" ForeColor="#337ab7" Style="color: red; text-align: center; width: 100%; float: left;"></asp:Label>
                            <asp:Label ID="lblTrial" runat="server"></asp:Label>

                            <asp:Panel ID="pnlReg" runat="server" Visible="false" CssClass="regi-id">
                                <asp:Label ID="lblKeylab" runat="server" Text="ID : "></asp:Label>
                                <asp:Label ID="lblKey" runat="server" Text=""></asp:Label>
                                <%--<asp:Label ID="lblSer" runat="server" Text="Enter Serial # : "></asp:Label>--%>
                                <div class="login-box input-serialkey">
                                    <i class="fa fa-key"></i>
                                    <asp:TextBox ID="txtSerial" runat="server" placeholder="SERIAL NUMBER" CssClass="form-control "></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSerial"
                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="reg"></asp:RequiredFieldValidator>
                                <div class="modal-footer btnwrap-right">
                                    <asp:Button ID="btnRegSerial" runat="server" class="btn waves-effect waves-light col s12 light-blue" OnClick="btnOK_Click" Text="Register"
                                        ValidationGroup="reg" />
                                </div>
                            </asp:Panel>
                            <div class="modal-footer btnwrap-right" style="float: left; clear: both; width: 100%; margin-top: 10px; border-top: 1px solid #f9f9f9">
                                <asp:LinkButton ID="lnkRegister" class="btn waves-effect waves-light col s12 light-blue" runat="server" OnClick="lnkRegister_Click">Register Now</asp:LinkButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div class="cf"></div>
            <asp:Button runat="server" ID="btnHidden" Style="display: none;" CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopupUser" BehaviorID="programmaticModalPopupBehavior2"
                TargetControlID="btnHidden" PopupControlID="pnlPopup" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <div class="clearfix"></div>
            <asp:Panel runat="server" ID="pnlPopup" Style="display: none; color: #fff !important; border-radius: 8px; background-color: #222; box-shadow: #222 0px 0px 8px 0px; padding-bottom: 15px;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; width: 20%; font-size: 1.5em;">Register</span>
                                <span class="lgpbtnClose">
                                    <asp:LinkButton ID="btncancel" runat="server" OnClick="btncancel_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblMSgUser" CssClass="lblMsg" runat="server" ForeColor="#fff" Style="color: red; text-align: center; width: 100%; float: left;"></asp:Label>
                                <asp:Label Style="text-align: center; width: 100%; float: left;" ID="lblTRialUser" runat="server"></asp:Label>
                                <div style="padding: 25px 6px; height: 56px; width: 100%;">
                                    <asp:Button ID="btnOKuser" CssClass="btn waves-effect waves-light col s12 light-blue" Style="border-radius: 6px;" runat="server" Text="OK" OnClick="btnOKuser_Click" />
                                </div>
                                <div style="padding: 25px 6px; height: 56px; width: 100%;">
                                    <asp:LinkButton ID="lnkRegisterUser" runat="server" Style="border-radius: 6px;" Class="btn waves-effect waves-light col s12 light-blue" OnClick="lnkRegisterUser_Click">Register Now</asp:LinkButton>
                                </div>
                                <asp:Panel ID="pnlUserreg" runat="server" Style="margin-top: 10px; text-align: center;" Visible="false">
                                    <asp:Label ID="lblKeylabuser" Style="font-weight: bold;" runat="server" Text="ID : "></asp:Label>
                                    <asp:Label ID="lblKeyuser" Style="font-weight: bold;" runat="server" Text=""></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblSeruser" runat="server" Text="Enter Serial # : "></asp:Label>
                                    <asp:TextBox ID="txtSerialUser" runat="server" CssClass="register_input_bg"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSerialUser"
                                        Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="reguser"></asp:RequiredFieldValidator>
                                    <div>
                                        <asp:Button ID="btnRegserialUser" CssClass="btn waves-effect waves-light col s12 light-blue" runat="server" Text="Register" ValidationGroup="reguser"
                                            OnClick="btnRegserialUser_Click" />
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
            <asp:Panel runat="server" ID="pnlPopupUpdatePassword" Width="700" Style="display: none; color: #fff !important; border-radius: 8px; background-color: #222; box-shadow: #222 0px 0px 8px 0px; padding-bottom: 15px;min-height:200px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Update Password</span>
                                <span class="lgpbtnClose">
                                    <asp:LinkButton ID="lnkCancelUpdate" runat="server" OnClick="lnkCancelUpdate_Click"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblUpdatePwErrMsg" CssClass="lblpErrMsg" runat="server"></asp:Label>
                                
                                <asp:Panel ID="Panel2" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                    <div>
                                        <asp:Label CssClass="lblpMsg" ID="lblUpdateMessage" runat="server"></asp:Label>
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
                                        <div class="row">
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
                                        <div class="col s4">&nbsp;</div>
                                        <div class="col s4">
                                            <asp:Button ID="btnUpdatePassword" CssClass="btn waves-effect waves-light col s12 light-blue" runat="server" Text="Update" ValidationGroup="reguser"
                                                OnClick="btnUpdatePassword_Click" />
                                        </div>
                                        <div class="col s4">&nbsp;</div>
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
            <asp:Panel runat="server" ID="pnlPopupResetPassword" Width="700"  Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Reset Password</span>
                                <span class="lgpbtnClose">
                                    <asp:LinkButton ID="lnkCancelReset" runat="server" OnClick="lnkCancelReset_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblResetPwMsg" CssClass="lblpErrMsg" runat="server" Text=""></asp:Label>
                                <asp:Panel ID="pnlResetPw" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                    <asp:Label ID="Label1" runat="server" CssClass="lblpMsg">
                                        Please input the below required details to request Password Reset from Admin
                                    </asp:Label>
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
                                    <div class="row">
                                        <div class="input-field col s4" style="padding-top: 20px;">
                                            <asp:Label ID="lblResetEmailAdmin" runat="server" Text="Email Address:"></asp:Label>
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
                                    <div class="col s4">&nbsp;</div>
                                    <div class="col s4">
                                        <asp:Button ID="btnResetPassword" CssClass="btn waves-effect waves-light col s12 light-blue" ValidationGroup="resetPassword" runat="server" Text="Submit"
                                            OnClick="btnResetPassword_Click" />
                                    </div>
                                    <div class="col s4">&nbsp;</div>

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
            <asp:Panel runat="server" ID="pnlPopupForgotPassword" Width="700" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Forgot Password</span>
                                <span class="lgpbtnClose">
                                    <asp:LinkButton ID="lnkCancelForgotPw" runat="server" OnClick="lnkCancelForgotPw_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblForgotPwMsg" CssClass="lblpErrMsg" runat="server" Text=" "></asp:Label>
                                <asp:Panel ID="pnlForgotPw" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                    <asp:Label CssClass="lblpMsg" ID="Label7" runat="server">
                                        Please enter your username and email address to reset password.
                                    </asp:Label>
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
                                    <div class="row">
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

                                    <div class="col s4">&nbsp;</div>
                                    <div class="col s4">
                                        <asp:Button ID="btnForgotPassword" CssClass="btn waves-effect waves-light col s12 light-blue" ValidationGroup="forgotPassword" runat="server" Text="Submit"
                                            OnClick="btnForgotPassword_Click" />
                                    </div>
                                    <div class="col s4">&nbsp;</div>
                                </asp:Panel>
                            </div>
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
            <asp:Panel runat="server" ID="pnlPopupUpdateForgotPw"  Width="700" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="section-ttle white-text" style="padding-top: 10px; padding-left: 10px">
                                <span style="float: left; font-size: 1.5em;">Reset Password</span>
                                <span class="lgpbtnClose">
                                    <asp:LinkButton ID="lnkCancelUpdateForgotPw" runat="server" OnClick="lnkCancelUpdateForgotPw_Click" Style="float: right;"><i class="fa fa-times"></i></asp:LinkButton></span>
                            </div>
                            <div style="padding: 20px;">
                                <asp:Label ID="lblUpdateForgotPwMsg" CssClass="lblpErrMsg" runat="server"></asp:Label>
                                
                                <asp:Panel ID="pnlUpdateForgotPw" runat="server" Style="margin-top: 10px; text-align: center;" Visible="true">
                                    <div>
                                        <asp:Label CssClass="lblpMsg" ID="Label2" runat="server"></asp:Label>
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
                                        <div class="row">
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
                                        <div class="col s4">&nbsp;</div>
                                        <div class="col s4">
                                            <asp:Button ID="btnUpdateFogotPw" CssClass="btn waves-effect waves-light col s12 light-blue" runat="server" Text="Submit"
                                                OnClick="btnUpdateFogotPw_Click" />
                                        </div>
                                        <div class="col s4">&nbsp;</div>
                                    </div>

                                </asp:Panel>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </form>
    <script src="Design/js/custom-script.js"></script>
</body>
</html>

