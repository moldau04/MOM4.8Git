<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSignaturePopup.aspx.cs" Inherits="MOMWebApp.EmailSignaturePopup" MasterPageFile="~/MOM.master"  ValidateRequest="false"%>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        #main {
            padding-left:0px !important;
        }
       
    </style>
    <script>
        function closeEmailPopup1() {
            window.close();
        }
    </script>
       <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

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

        .materialize-textarea {
            max-height: 100px !important;
            height: 100px !important;
        }

        body {
            background-color: white !important;
        }

        .RadComboBox_Metro .rcbInner {
            padding-top: 0px !important;
        }

        .stiJsViewerPageShadow {
            height: auto !important;
        }

        .RadComboBoxDropDown.RadComboBoxDropDown_Metro {
            width: 220px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.Application.add_init(appl_init);

            function appl_init() {
                var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
                pgRegMgr.add_beginRequest(BlockUI);
                pgRegMgr.add_endRequest(UnblockUI);
            }

            function BlockUI(sender, args) {
                document.getElementById("overlay").style.display = "block";
            }
            function UnblockUI(sender, args) {
                document.getElementById("overlay").style.display = "none";
            }
        });

        function showMailReport() {
            //window.radopen(null, "RadCreateWindow");
            $('#mainWindow').attr('style', 'display:none');
            $('#dvEmailOpen').attr('style', 'display:block');
        }        
       

        function cancel() {
            window.close();
            return false;
        }

        function splitEmail(txt) {
            var regExp = /\(([^)]+)\)/;
            return regExp.exec(txt);
        }
        
        function updateparent() {
            window.parent.document.getElementById('lnkRefreshScreen').click();
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="dvEmailOpen" style="display: block;margin-top: 15px;">
        <div class="container mailtitcketcontainer" runat="server" id="pnlSignature">
            <div class="row" style="margin-bottom: 0;">
                <div class="srchpaneinner">
                    <div class="form-section-row">
                        <div class="form-section2">
                            <div class="input-field col s12">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21_txtSignName" runat="server" ControlToValidate="txtSignName"
                                Display="None" ErrorMessage="Signature Name Required"  SetFocusOnError="True" ValidationGroup="signature"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5_ValidateCalloutExtender" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator21_txtSignName" PopupPosition="BottomLeft">
                                </asp:ValidatorCalloutExtender>

                                <asp:TextBox ID="txtSignName" runat="server" MaxLength="50"></asp:TextBox>
                                <label for="txtSignName">Name</label>
                            </div>
                        </div>
                        <div class="form-section2-blank">
                            &nbsp;
                        </div>
                        <div class="form-section2">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkDefaultSign" runat="server" CssClass="filled-in" />
                                <asp:Label runat="server" Text="Is Default"></asp:Label>
                            </div>
                            <%--<div class="form-col">
                                <div class="fc-label">
                                    <label>Is Default</label>
                                </div>
                                <div class="fc-input">
                                    <asp:CheckBox ID="chkDefaultSign" runat="server"></asp:CheckBox>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                    <div class="form-section-row">
                        <div class="form-col">
                            <div class="fc-label">
                                <label>Body</label>
                            </div>
                        
                            <div class="fc-input">
                                <CKEditor:CKEditorControl ID="txtSignBody" Enabled="true" runat="server" Width="100%" Height="150" Toolbar="Full"
                                    TemplatesFiles="js/ckeditor/plugins/templates/templates/default.js" BasePath="js/ckeditor/"
                                    ContentsCss="js/ckeditor/contents.css" FilebrowserImageUploadUrl="CKimageupload.ashx"
                                    ExtraPlugins="imagepaste,preventdrop">
                                </CKEditor:CKEditorControl>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both;"></div>
        <footer style="float: left; padding-left: 0 !important;">
            <div class="btnlinks">
                <asp:LinkButton ID="lnkSaveSignature" OnClick="lnkSaveSignature_Click" runat="server" ValidationGroup="signature">Save</asp:LinkButton>
            </div>
        </footer>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            
        });
    </script>
</asp:Content>
