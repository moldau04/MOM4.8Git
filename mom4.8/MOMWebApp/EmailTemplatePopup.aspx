<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MomPopup.master" Inherits="EmailTemplatePopup" Codebehind="EmailTemplatePopup.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
      
    <style>
        #main {
            padding-left:0px !important;
        }
       .RadGrid_Bootstrap .rgEditForm{
           border:none !important;
       }
         .RadGrid_Bootstrap .rgEditForm label{
         font-weight:bolder;
       }
          .rwTitleWrapper {
            padding: 0 !important;
            font-size: 1.42857em;
            font-family: inherit;
            line-height: 1em;
            color: #fff;
            text-transform: capitalize;
        }

       .rwTitleBar {
            background-color: #1c5fb1 !important;
            padding: 5px;
        }
    </style>
 
    <script>
      
        $(document).ready(function () {
            if (Sys.Extended && Sys.Extended.UI && Sys.Extended.UI.HtmlEditorExtenderBehavior && Sys.Extended.UI.HtmlEditorExtenderBehavior.prototype && Sys.Extended.UI.HtmlEditorExtenderBehavior.prototype._editableDiv_submit) {
                Sys.Extended.UI.HtmlEditorExtenderBehavior.prototype._editableDiv_submit = function () {
                    //html encode
                    var char = 3;
                    var sel = null;

                    setTimeout(function () {
                        if (this._editableDiv != null)
                            this._editableDiv.focus();
                    }, 0);
                    if (Sys.Browser.agent != Sys.Browser.Firefox) {
                        if (document.selection) {
                            sel = document.selection.createRange();
                            sel.moveStart('character', char);
                            sel.select();
                        }
                        else {
                            if (this._editableDiv.firstChild != null && this._editableDiv.firstChild != undefined) {
                                sel = window.getSelection();
                                sel.collapse(this._editableDiv.firstChild, char);
                            }
                        }
                    }

                    //Encode html tags
                    this._textbox._element.value = this._encodeHtml();
                }
            }
             Materialize.updateTextFields();
        });
         function updateparent()
        {debugger
            window.parent.document.getElementById('lnkRefressScreen').click();
         }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container accordian-wrap" style="margin: 10px;">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="form-section-row">
                        <div class="input-field col s10">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmailForms" Display="none" ForeColor="Red"
                                    ValidationGroup="EmailTemplate" ControlToValidate="txtEmailFormsName" runat="server" ErrorMessage="Please enter Name!">
                                </asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtEmailFormsName" ValidationGroup="EmailTemplate" runat="server" AutoCompleteType="None" MaxLength="50"></asp:TextBox>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                    PopupPosition="BottomRight" TargetControlID="RequiredFieldValidatorEmailForms">
                                </asp:ValidatorCalloutExtender>

                                <label for="txtEmailFormsName">Name</label>
                            </div>
                        </div>
                        <div class="input-field col s2" style="margin-top: 20px; padding-left: 25px">
                            <div class="row">
                                <asp:CheckBox ID="chkActive" runat="server" CssClass="css-checkbox" Text="Active"></asp:CheckBox>

                            </div>
                        </div>
                    </div>
                    <div class="form-section-row">
                        <div class="fc-label">
                            <label>Body</label>
                        </div>
                        <div class="fc-input">
                            <CKEditor:CKEditorControl ID="txtBodyCKE" runat="server" Width="100%" Height="300" Toolbar="Full"
                                BasePath="js/ckeditor" TemplatesFiles="js/ckeditor/plugins/templates/templates/default.js"
                                ContentsCss="js/ckeditor/contents.css" FilebrowserImageUploadUrl="CKimageupload.ashx"
                                ExtraPlugins="imagepaste,preventdrop">
                            </CKEditor:CKEditorControl>
                        </div>

                    </div>
                    <div class="btnlinks">
                        <asp:LinkButton ID="lnkUploadEmail" runat="server" ValidationGroup="EmailTemplate" OnClick="lnkUploadEmail_Click">Save</asp:LinkButton>
                        <asp:HiddenField ID="hdnID" Value="" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>



    <asp:HiddenField ID="hdnDefaultTemplate" Value="" runat="server" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
     <script type="text/javascript">
    function updateparent()
        {debugger
            window.parent.document.getElementById('lnkRefressScreen').click();
         }
         </script>
</asp:Content>
