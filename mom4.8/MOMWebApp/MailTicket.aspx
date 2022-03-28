<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="MailTicket" Codebehind="MailTicket.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <script type="text/javascript">
        function cancel() {
            var conf = confirm('Do you want to close the ticket screen?');
            if (conf) window.close();
        }
    </script>
    <style type="text/css">
        .mailtitcketcontainer input[type=text] {
            height: 1.5rem !important;
            margin: 0 !important;
        }

        .txtBody {
            min-height: 380px;
        }

        [id$='FontName'] ,[id$='FontSize']  
        {
            display: inline-block !important;
            margin: 0 !important;
            font-size: 12px !important;
        }

        .breadcrumb-mailticket ul.anchor-links li{
            border-right: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="fa fa-envelope"></i>&nbsp;Mail Ticket</div>
                                    <div class="buttonContainer">
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="LinkButton1" runat="server"
                                            OnClick="LnkSend_Click">Send</asp:LinkButton>

                                    </div>
                                    <div class="btnlinks">
                                        <a runat="server" id="A2" href="#" onclick="cancel();"
                                            tabindex="24">Close</a>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>

    <div class="container mailtitcketcontainer">
        <div class="row">
            <div class="srchpane-advanced" style="margin: 0 !important;">
                <div class="srchpaneinner">
                    <div class="form-col">
                        <div class="fc-label">
                            <label>From</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtEmailFrom" runat="server"
                                CssClass="form-control"
                                TabIndex="9" ToolTip="From" Placeholder="From"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtEmailFrom_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                TargetControlID="txtEmailFrom">
                            </asp:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                ControlToValidate="txtEmailFrom" Display="None"
                                ErrorMessage="Invalid Email" SetFocusOnError="True"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator7_ValidatorCalloutExtender"
                                runat="server" Enabled="True"
                                TargetControlID="RegularExpressionValidator7">
                            </asp:ValidatorCalloutExtender>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>To</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtEmail" runat="server"
                                CssClass="form-control"
                                TabIndex="9" ToolTip="To" Placeholder="To"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                TargetControlID="txtEmail">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtEmail"
                                Display="None" ErrorMessage="Email Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator9_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator9">
                            </asp:ValidatorCalloutExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid E-Mail Address"
                                ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                ValidationGroup="mail"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                runat="server" Enabled="True"
                                TargetControlID="RegularExpressionValidator1">
                            </asp:ValidatorCalloutExtender>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>CC</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtEmailCc" runat="server"
                                CssClass="form-control"
                                TabIndex="9" ToolTip="Cc" Placeholder="Cc"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtEmailCc_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                TargetControlID="txtEmailCc">
                            </asp:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                ControlToValidate="txtEmailCc" Display="None" ErrorMessage="Invalid E-Mail Address"
                                ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                ValidationGroup="mail"></asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator6_ValidatorCalloutExtender"
                                runat="server" Enabled="True"
                                TargetControlID="RegularExpressionValidator6">
                            </asp:ValidatorCalloutExtender>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Subject</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtSubject" runat="server"
                                CssClass="form-control"
                                TabIndex="9" ToolTip="Subject" Placeholder="Subject"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Body</label>
                        </div>
                        <div class="fc-input">
                            <%--<asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Columns="50"
                                Rows="10"  CssClass="form-control txtBody"></asp:TextBox>--%>
                            <%--<asp:HtmlEditorExtender ID="htmlEditorExtender1" TargetControlID="txtBody"
                                runat="server" EnableSanitization="False" Enabled="True">
                            </asp:HtmlEditorExtender>--%>
                            <CKEditor:CKEditorControl ID="txtBody" runat="server" Width="100%" Height="357" Toolbar="Full"
                                BasePath="js/ckeditor" TemplatesFiles="js/ckeditor/plugins/templates/templates/default.js"
                                ContentsCss="js/ckeditor/contents.css" FilebrowserImageUploadUrl="CKimageupload.ashx"
                                ExtraPlugins="imagepaste,preventdrop">
                            </CKEditor:CKEditorControl>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

